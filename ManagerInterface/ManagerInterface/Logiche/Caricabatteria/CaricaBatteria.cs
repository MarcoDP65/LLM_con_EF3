using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;

using System.Threading;
using System.ComponentModel;

using SQLite.Net;

namespace ChargerLogic
{
    public partial class CaricaBatteria
    {

        public SerialPort serialeApparato;
        private static MessaggioLadeLight _mS;
        private parametriSistema _parametri;
        private static Queue<byte> codaDatiSER = new Queue<byte>();

        public DateTime UltimaScrittura;   // Registro l'istante dell'ultima scrittura

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
        delegate void SetTextCallback(string text);
        string InputData = String.Empty;
        byte[] _dataBuffer = new byte[0];
        int lastByte = 0;
        bool readingMessage = false;
        public int TipoRisposta;
        static bool _rxRisposta;
        public SerialMessage.comandoIniziale Intestazione = new SerialMessage.comandoIniziale();
        public SerialMessage.cicliPresenti CicliPresenti = new SerialMessage.cicliPresenti();
        public SerialMessage.comandoRTC OrologioSistema = new SerialMessage.comandoRTC();
        public SerialMessage.cicloAttuale CicloInMacchina = new SerialMessage.cicloAttuale();
        public SerialMessage.VariabiliLadeLight VaribiliAttuali = new SerialMessage.VariabiliLadeLight();


        public llParametriApparato ParametriApparato = new llParametriApparato();
        public llMappaMemoria Memoria = new llMappaMemoria(1);
        public llContatoriApparato ContatoriLL = new llContatoriApparato();
        public LadeLightData ApparatoLL;

        public llVariabili llVariabiliAttuali = new llVariabili();

        public llProgrammaCarica ProgrammaAttivo;
        public List<llProgrammaCarica> ProgrammiDefiniti;

        public List<_llModelloCb> ModelliLL;
        public List<_llProfiloCarica> ProfiliCarica;
        public List<llDurataCarica> DurateCarica;
        public List<llDurataProfilo> DurateProfilo;
        public List<_llProfiloTipoBatt> ProfiloTipoBatt;
        public List<llTensioneBatteria> TensioniBatteria;
        public List<llTensioniModello> TensioniModello;
        public List<llMemoriaCicli> MemoriaCicli;
        public List<llMemBreve> BreviCicloCorrente;


        public const byte SizeCharge = 36;
        public const byte SizeShort = 30;
        public const UInt32 FirstShort = 0x006000;   // dal 08/02/2019 spostato  da 0x5000 a 0x6000
        public const UInt32 MaxByteShort = 0x1AEFFF;



        //public System.Collections.Generic.List<SerialMessage.CicloDiCarica> CicliInMemoria = new System.Collections.Generic.List<SerialMessage.CicloDiCarica>();

        // -------------------------------------------------------
        //    Dichiarazione eventi per la gestione avanzamento
        // -------------------------------------------------------

        public event StepHandler Step;


        public delegate void StepHandler(CaricaBatteria ull, ProgressChangedEventArgs e);

        public llStatoFirmware StatoFirmware = new llStatoFirmware();

        private Boolean _firmwarePresente = false;

        private static EventWaitHandle LL_USBeventWait;
        private DateTime _startRead;

        public byte[] numeroSeriale;
        private string _lastError;
        private bool _cbCollegato;
        public bool apparatoPresente = false;

        public byte[] DatiRisposta;
        public SerialMessage.EsitoRisposta UltimaRisposta;
        private SerialMessage.TipoRisposta _ultimaRisposta = SerialMessage.TipoRisposta.NonValido;   // flag per l'indicazioene del tipo dell'ultimo messaggio ricevuto dalla scheda

        public int AttesaTimeout = 25; // Tempo attesa in decimi di secondo
        public SerialMessage.OcBaudRate BrOCcorrente = SerialMessage.OcBaudRate.OFF;
        public SerialMessage.OcEchoMode EchoOCcorrente = SerialMessage.OcEchoMode.OFF;

        private bool aspettaRisposta(int timeout, int risposteAttese = 1, bool aspettaAck = false, bool runAsync = false)
        {
            object vuoto;
            return aspettaRisposta(timeout, out vuoto, risposteAttese, aspettaAck, runAsync, elementiComuni.tipoMessaggio.NonDefinito);
        }


        /// <summary>
        /// Mette il task principale in attesa risposta:  
        /// V2.0 In base al canale attivo aspetto da seriale o leggo da USB 
        /// </summary>
        /// <param name="timeout">numero di cicli di attasa da 100 millisecondi l'uno</param>
        /// <returns>true se ricevuta risposta, altrimenti false se interrotto per timeout</returns>
        private bool aspettaRisposta(int timeout, out object esito, int risposteAttese = 1, bool aspettaAck = false, bool runAsync = false, elementiComuni.tipoMessaggio TipoDati = elementiComuni.tipoMessaggio.NonDefinito)
        {
            DateTime _startRicezione;
            DateTime _startFunzione;
            TimeSpan _tTrascorso;
            bool _trovatoETX = false;
            bool _richiestaCancellata = false;
            int _divider = 1;
            int _lastProgress = -1;
            int _loopAttesa = 0;
            try
            {

                // entro nel loop e aspetto 
                esito = null;
                _loopAttesa++;
                if (_parametri.CanaleLadeLight == parametriSistema.CanaleDispositivo.USB)
                {
                    // mi metto in ascolto sul canale USB fino a EOT o a timeout 
                    // aspetto 10 mS 
                    // System.Threading.Thread.Sleep(100);

                    // Check the amount of data available to read
                    // In this case we know how much data we are expecting, 
                    // so wait until we have all of the bytes we have sent.
                    uint numBytesAvailable = 0;
                    uint numTempToRead = 0;
                    FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

                    int _risposteRicevute = 0;
                    int _ackRicevuti = 0;
                    int _breakRicevuti = 0;
                    SerialMessage.TipoRisposta _msgRicevuto;

                    Log.Debug("Inizio Ascolto LL " + timeout.ToString() + " - " + risposteAttese.ToString() + " - " + aspettaAck.ToString());

                    bool _inAttesa = true;

                    _startFunzione = DateTime.Now;
                    _startRicezione = DateTime.Now;
                    do
                    {
                        // verifico se ci sono dati
                        numBytesAvailable = 0;
                        ftStatus = _parametri.usbLadeLight.GetRxBytesAvailable(ref numBytesAvailable);
                        if (ftStatus != FTDI.FT_STATUS.FT_OK)
                        {
                            // Errore nella lettura dati disponibili
                            Log.Warn("Failed to get number of bytes available to read (error " + ftStatus.ToString() + ")");
                        }

                        //Log.Debug("Dati disponibili LL (USB) " + numBytesAvailable.ToString());
                        if (numBytesAvailable > 0)
                        {
                            // Now that we have the amount of data we want available, read it
                            byte[] readData = new byte[numBytesAvailable];
                            uint numBytesRead = 0;
                            Log.Debug("Ricevuti " + numBytesAvailable.ToString() + " bytes");
                            // Note that the Read method is overloaded, so can read string or byte array data
                            ftStatus = _parametri.usbLadeLight.Read(readData, numBytesAvailable, ref numBytesRead);
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Warn("Failed to read LL data (error " + ftStatus.ToString() + ")");
                                //return false;
                            }

                            // Log.Debug("Dati Ricevuti LL (USB) " + numBytesRead.ToString());
                            for (int _i = 0; _i < numBytesRead; _i++)
                            {

                                codaDatiSER.Enqueue(readData[_i]);

                                if (TipoDati != elementiComuni.tipoMessaggio.DumpMemoria)
                                {
                                    if (readData[_i] == SerialMessage.serETX)
                                    {
                                        Log.Debug("Trovato Etx (USB), faccio ripartire il timeout");
                                        _trovatoETX = true;
                                        Log.Debug("Dati in coda LL (USB) " + codaDatiSER.Count.ToString());
                                        // Ho ricevuto dati validi, faccio ripartire il timer Timeout
                                        _startFunzione = DateTime.Now;
                                    }
                                }
                                else
                                {
                                    if (codaDatiSER.Count >= risposteAttese)
                                    {
                                        _trovatoETX = true;
                                        Log.Debug("DUMPMEM:Dati in coda LL (USB) " + codaDatiSER.Count.ToString());
                                        return true;
                                    }
                                }

                                if (_trovatoETX)
                                {
                                    Log.Debug("trovato ETX");
                                    _msgRicevuto = analizzaCoda();
                                    Log.Debug("Dati in coda LL (USB) " + codaDatiSER.Count.ToString());
                                    _ultimaRisposta = _msgRicevuto;
                                    _trovatoETX = false;

                                    switch (_msgRicevuto)
                                    {

                                        case SerialMessage.TipoRisposta.Ack:
                                            _ackRicevuti++;
                                            if (aspettaAck && _risposteRicevute >= risposteAttese) _inAttesa = false;
                                            break;

                                        case SerialMessage.TipoRisposta.Data:
                                            _risposteRicevute++;
                                            // se la gestione eventi è attiva, lancio un evento 
                                            if (runAsync == true)
                                            {
                                                if (Step != null)
                                                {
                                                    /*
                                                    int _progress = 0;
                                                    double _valProgress = 0;
                                                    sbWaitStep _passo = new sbWaitStep();
                                                    _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                                    _passo.TipoDati = TipoDati;
                                                    _passo.Eventi = risposteAttese;
                                                    _passo.Step = _risposteRicevute;
                                                    _passo.EsecuzioneInterrotta = false;
                                                    if (risposteAttese > 0)
                                                    {
                                                        _valProgress = (_risposteRicevute * 100) / risposteAttese;
                                                    }
                                                    _progress = (int)_valProgress;
                                                    if (_lastProgress != _progress)
                                                    {
                                                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                                        Log.Debug("Passo " + _risposteRicevute.ToString());
                                                        Step(this, _stepEv);
                                                        _lastProgress = _progress;
                                                    }
                                                    */
                                                }
                                            }

                                            if (_risposteRicevute >= risposteAttese)
                                            {
                                                _inAttesa = false;
                                            }
                                            break;

                                        case SerialMessage.TipoRisposta.Break:
                                            Log.Debug("Ricevuto BREAK");
                                            _breakRicevuti++;
                                            _inAttesa = false;
                                            break;

                                        case SerialMessage.TipoRisposta.Nack:
                                        case SerialMessage.TipoRisposta.NonValido:
                                            _breakRicevuti++;
                                            _inAttesa = false;
                                            break;

                                    }
                                }


                            }
                        }

                        System.Threading.Thread.Sleep(10);
                        if (raggiuntoTimeout(_startFunzione, timeout))
                        {
                            Log.Debug("aspettaRisposta.USB raggiunto Timeout");
                            break;
                        }
                        //Log.Debug(DateTime.Now.ToShortTimeString());
                    }
                    while (_inAttesa);
                    Log.Debug("Effettuati " + _loopAttesa.ToString() + " cicli di attesa: " + SecondiTrascorsi(_startFunzione).ToString("0.00"));

                    // se background mode attivo, lancio l'evento di fine elaborazione
                    if (runAsync == true)
                    {
                        elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
                        _tTrascorso = DateTime.Now.Subtract(_startRicezione);

                        _esitoBg.EventiPrevisti = risposteAttese;
                        _esitoBg.UltimoEvento = _risposteRicevute;
                        _esitoBg.SecondiElaborazione = _tTrascorso.TotalSeconds;
                        // RunWorkerCompletedEventArgs _esito = new RunWorkerCompletedEventArgs(_esitoBg, null, _richiestaCacellata);

                    }
                    if (!_inAttesa)
                    {
                        Log.Debug("Aspetta risposta LL ricevuti " + _risposteRicevute.ToString() + " di " + risposteAttese.ToString() + " in " + DateTime.Now.Subtract(_startRicezione).ToString() + " secondi");
                    }
                    else
                    {
                        Log.Debug("Aspetta risposta LL - raggiunto timeout " + DateTime.Now.Subtract(_startRicezione).ToString() + " secondi");

                    }

                    return !_inAttesa;
                }
                else
                {
                    int _cicli = 2;
                    //_rxRisposta = false;
                    string _logx = "";
                    System.Threading.Thread.Sleep(200);
                    do
                    {
                        _cicli++;
                        _logx += "^";
                        if (_rxRisposta)
                        {
                            _logx += "|";
                            Log.Debug(_logx);
                            return true;
                        }
                        System.Threading.Thread.Sleep(50);
                    }
                    while (_rxRisposta == false && (_cicli < (timeout * 10)));
                    Log.Debug(_logx);
                    return _rxRisposta;
                }
            }
            catch (Exception Ex)
            {
                Log.Debug("aspettaRisposta: " + Ex.Message);
                esito = null;
                return false;
            }
        }

        private bool raggiuntoTimeout(DateTime inizio, int DecimiTimeOut)
        {
            try
            {
                DateTime _ora = DateTime.Now;
                TimeSpan _durata = _ora.Subtract(inizio);
                int _moltiplicatore = 1;  // Solo per debug, per estendere il tempo di attesa
                double _decimiEffettivi = _durata.TotalSeconds * 10;

                if (_decimiEffettivi > (DecimiTimeOut * _moltiplicatore))
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch { return true; }
        }

        private double SecondiTrascorsi(DateTime inizio)
        {
            try
            {
                DateTime _ora = DateTime.Now;
                TimeSpan _durata = _ora.Subtract(inizio);
                double _secondiEffettivi = _durata.TotalSeconds;

                return _secondiEffettivi;


            }
            catch { return 0; }
        }

        private void LL_UsbWaiter()
        {
            Log.Info("LLUSB Start Waiting...");
            do
            {

                LL_USBeventWait.WaitOne();                // Wait for notification
                Log.Info("LLUSB Notified");
                usb_DataReceivedLL();

            } while (true);
        }

        private void usb_DataReceivedLL()
        {
            try
            {
                bool _trovatoETX = false;

                // mi metto in ascolto sul canale USB fino a EOT o a timeout 
                // aspetto 10 mS 
                // System.Threading.Thread.Sleep(100);

                // Check the amount of data available to read
                // In this case we know how much data we are expecting, 
                // so wait until we have all of the bytes we have sent.
                Log.Warn("Lettura USB LL: coda iniziale " + codaDatiSER.Count.ToString() + " bytes");

                uint numBytesAvailable = 0;
                FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

                ftStatus = _parametri.usbLadeLight.GetRxBytesAvailable(ref numBytesAvailable);
                Log.Debug("Dati Disponibili LL (USB)" + numBytesAvailable.ToString());
                if (ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    // Errore nella lettura dati disponibili
                    Log.Warn("Failed to get number of bytes available to read LL (error " + ftStatus.ToString() + ")");
                }
                if (numBytesAvailable > 0)
                {
                    // Now that we have the amount of data we want available, read it
                    byte[] readData = new byte[numBytesAvailable];
                    uint numBytesRead = 0;
                    // Note that the Read method is overloaded, so can read string or byte array data
                    ftStatus = _parametri.usbLadeLight.Read(readData, numBytesAvailable, ref numBytesRead);
                    Log.Debug("Lettura di " + numBytesRead.ToString() + " di " + numBytesAvailable.ToString());
                    if (ftStatus != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        Log.Warn("Failed to read data LL (error " + ftStatus.ToString() + ")");
                        //return false;
                    }

                    Log.Debug("Dati Ricevuti LL (USB)" + numBytesRead.ToString());
                    for (int _i = 0; _i < numBytesRead; _i++)
                    {

                        codaDatiSER.Enqueue(readData[_i]);
                        if (readData[_i] == SerialMessage.serETX)
                        {
                            Log.Debug("Trovato Etx LL (USB)");
                            _trovatoETX = true;
                        }
                    }
                    if (_trovatoETX)
                    {
                        Log.Debug("Trovato Etx LL (USB) --> Analizza Coda");
                        analizzaCoda();
                    }

                }

            }
            catch (Exception Ex)
            {
                Log.Error("USB_DataReceived LL: " + Ex.Message);
            }

        }


        public CaricaBatteria(ref parametriSistema parametri)
        {

            try
            {

                //ControllaAttesa(UltimaScrittura);

                _parametri = parametri;
                _mS = new MessaggioLadeLight();
                _mS.Dispositivo = SerialMessage.TipoDispositivo.Charger;
                byte[] Seriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
                _mS.SerialNumber = Seriale;
                _cbCollegato = false;
                serialeApparato = _parametri.serialeCorrente;

                InizializzaDatiLocali();

                // Attivo gli eventi sia USB che COM

                FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

                // USB
                cEventHelper.RemoveEventHandler(serialeApparato, "DataReceived");
                Log.Debug("cEventHelper.RemoveEventHandler serialeApparato");

                serialeApparato.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceivedCB);
                Log.Debug("add EventHandler serialeApparato");

            }

            catch (Exception Ex)
            {
                Log.Error("NEW CaricaBatteria: " + Ex.Message);
            }


        }

        public bool apriPorta()
        {
            bool _esito;
            //_esito = _parametri.apriCom();
            _esito = _parametri.apriLadeLight();
            return _esito;

        }

        public bool chiudiPorta()
        {
            //_esito = _parametri.apriCom();
            _parametri.chiudiCanaleLadeLight();
            return true;

        }

        public bool VerificaPresenza()
        {
            try
            {
                bool _esito = CaricaIntestazioneLL();
                return _esito;

                /*
                bool _esito = false;
                _mS.Comando = SerialMessage.TipoComando.CMD_CONNECT;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("START");
                Log.Debug(_mS.hexdumpMessaggio());

                // Leggo la testata apparato
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(AttesaTimeout, 0,true,false);
                if (_mS._comando == (byte)SerialMessage.TipoComando.ACK_PACKET)  
                {
                    _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                    _mS.Comando = SerialMessage.TipoComando.CMD_UART_HOST_CONNECTED;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    Log.Debug("PRIMA LETTURA");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(AttesaTimeout, 1,true,false);
                    Intestazione = _mS.Intestazione;
                    _cbCollegato = _esito;
                    apparatoPresente = _esito;
                    return _esito;

                }
                return _esito;
                */
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

        public bool CaricaApparatoLL()
        {
            try
            {
                bool _esito = CaricaIntestazioneLL();
                return _esito;


                /*
                bool _esito = false;
                _mS.Comando = SerialMessage.TipoComando.CMD_CONNECT;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("START");
                Log.Debug(_mS.hexdumpMessaggio());

                // Leggo la testata apparato
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(AttesaTimeout, 0,true,false);
                if (_mS._comando == (byte)SerialMessage.TipoComando.ACK_PACKET)  
                {
                    _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                    _mS.Comando = SerialMessage.TipoComando.CMD_UART_HOST_CONNECTED;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    Log.Debug("PRIMA LETTURA");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(AttesaTimeout, 1,true,false);
                    Intestazione = _mS.Intestazione;
                    _cbCollegato = _esito;
                    apparatoPresente = _esito;
                    return _esito;

                }
                return _esito;
                */
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        public bool CaricaIntestazioneLL()
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;
                _mS.Comando = SerialMessage.TipoComando.CMD_READ_MEMORY;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("NEW START");

                byte[] _Dati = new byte[242];

                // Leggo dal primo banco memoria fissa
                _esito = LeggiBloccoMemoria(0, 240, out _Dati);
                Log.Debug(FunzioniComuni.HexdumpArray(_Dati));
                //_mS.
                MessaggioLadeLight.PrimoBloccoMemoria BloccoIntestazione;
                BloccoIntestazione = new MessaggioLadeLight.PrimoBloccoMemoria();
                _esitoMsg = BloccoIntestazione.analizzaMessaggio(_Dati, 1);

                Intestazione = new SerialMessage.comandoIniziale();

                if (_esitoMsg == SerialMessage.EsitoRisposta.MessaggioOk)
                {
                    _esito = true;
                    Intestazione.Matricola = BloccoIntestazione.SerialeApparato;
                    Intestazione.PrimaInstallazione = BloccoIntestazione.DataSetupApparato.ToString();

                }

                _cbCollegato = _esito;
                apparatoPresente = _esito;
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

        public bool CaricaApparatoA0()
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;

                // Leggo dal primo banco memoria fissa
                _esito = LeggiParametriApparato();

                if (_esito)
                {

                    // Se la matricola è inizializzata carico il recors da DB e lo allineo, altrimenti vado in modalità SOLORAM
                    if ((ParametriApparato.llParApp.SerialeApparato == 0x00) || ((ParametriApparato.llParApp.SerialeApparato & 0xFFFFFF) == 0xFFFFFF))
                    {
                        // Matricola vuota
                        ApparatoLL = new LadeLightData();
                        ApparatoLL.Id = "000000";


                    }
                    else
                    {
                        // C'è la matricola --> registro il record
                        ApparatoLL = new LadeLightData();
                        // ApparatoLL.caricaDati(ParametriApparato.s);
                    }




                    //Intestazione.Matricola = BloccoIntestazione.SerialeApparato;
                    //Intestazione.PrimaInstallazione = BloccoIntestazione.DataSetupApparato.ToString();

                }

                _cbCollegato = _esito;
                apparatoPresente = _esito;
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }
        public bool CaricaProgrammaAttivo()
        {
            try
            {
                ProgrammaAttivo = CaricaProgramma(0);

                return true;
        
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }



        public llProgrammaCarica CaricaProgramma(byte IdPosizione)
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;
                llProgrammaCarica tempPrg = new llProgrammaCarica();
                MessaggioLadeLight.MessaggioProgrammazione ImmagineCarica = new MessaggioLadeLight.MessaggioProgrammazione();
                SerialMessage.EsitoRisposta EsitoMsg;

                if (IdPosizione > 15) IdPosizione = 15;

                uint StartAddr = (uint)(0x2000 + ( 256 * IdPosizione ));

                byte[] _datiTemp = new byte[226];
                _esito = LeggiBloccoMemoria(StartAddr, 226, out _datiTemp);

                if (_esito)
                {
                    EsitoMsg = ImmagineCarica.analizzaMessaggio(_datiTemp, 1);
                    if (EsitoMsg == SerialMessage.EsitoRisposta.MessaggioOk)
                    {
                        tempPrg.IdProgramma = ImmagineCarica.IdProgrammazione;
                        tempPrg.TipoRecord = ImmagineCarica.TipoProgrammazione;
                        tempPrg.ProgramName = ImmagineCarica.NomeCiclo;
                        tempPrg.IdProfilo = ImmagineCarica.IdProfilo;


                        tempPrg.ListaParametri = ImmagineCarica.Parametri;
                        tempPrg.AnalizzaListaParametri();

                        return tempPrg;
                    }

                }

                return null;  // llProgrammaCarica
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return null;
            }

        }

        public bool CaricaAreaProgrammi()
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;

                ProgrammiDefiniti = new List<llProgrammaCarica>();

                // Comincio a leggere dal primo messaggio da 240 bytes nell'area programmazioni e continuo a scorrere fino a che non ho
                // tipo record = 0xFF o ho raggiunto l'ultimo ( 15 con inizio 0 ) 

                for(byte contacicli = 0; contacicli <16; contacicli++)
                {
                    llProgrammaCarica _tempProgramma = CaricaProgramma(contacicli);
                    if(_tempProgramma.TipoRecord != 0xFF)
                    {
                        ProgrammiDefiniti.Add(_tempProgramma);
                        _esito = true; // ho almeno 1 programma;
                    }
                    else
                    {
                        break;
                    }

                }

                
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }

        }


        public bool CaricaAreaContatori()
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;
                MessaggioLadeLight.MessaggioAreaContatori MsgContatoriLL = new MessaggioLadeLight.MessaggioAreaContatori();
                SerialMessage.EsitoRisposta EsitoMsg;

                ContatoriLL = new llContatoriApparato();

                uint StartAddr = 0x3000;

                byte[] _datiTemp = new byte[240];
                _esito = LeggiBloccoMemoria(StartAddr, 240, out _datiTemp);

                if (_esito)
                {
                    EsitoMsg = MsgContatoriLL.analizzaMessaggio(_datiTemp, 1);
                    if (EsitoMsg == SerialMessage.EsitoRisposta.MessaggioOk)
                    {

                        ContatoriLL.DataPrimaCarica = MsgContatoriLL.DataPrimaCarica;
                        ContatoriLL.CntCicliTotali = MsgContatoriLL.CntCicliTotali;
                        ContatoriLL.CntCicliStaccoBatt = MsgContatoriLL.CntCicliStaccoBatt;
                        ContatoriLL.CntCicliStop = MsgContatoriLL.CntCicliStop;
                        ContatoriLL.CntCicliLess3H = MsgContatoriLL.CntCicliLess3H;
                        ContatoriLL.CntCicli3Hto6H = MsgContatoriLL.CntCicli3Hto6H;
                        ContatoriLL.CntCicli6Hto9H = MsgContatoriLL.CntCicli6Hto9H;
                        ContatoriLL.CntCicliOver9H = MsgContatoriLL.CntCicliOver9H;
                        ContatoriLL.CntProgrammazioni = MsgContatoriLL.CntProgrammazioni;
                        ContatoriLL.CntCicliBrevi = MsgContatoriLL.CntCicliBrevi;
                        ContatoriLL.PntNextBreve = MsgContatoriLL.PntNextBreve;
                        ContatoriLL.CntCariche = MsgContatoriLL.CntCariche;
                        ContatoriLL.PntNextCarica = MsgContatoriLL.PntNextCarica;

                        ContatoriLL.valido = true;

                        return true;
                    }

                }

                return false; 

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }

        }




        public bool StartComunicazione(int TimeoutRisposta = 50)
        {
            try
            {
                bool _esito = false;
                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_CONNECT;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("START");
                Log.Debug(_mS.hexdumpMessaggio());
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(TimeoutRisposta, 0, true, false);
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

        public bool StopComunicazione(int TimeoutRisposta = 50)
        {
            try
            {
                bool _esito = false;
                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_DISCONNECT;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("STOP");
                Log.Debug(_mS.hexdumpMessaggio());
                _esito = _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                if (_esito)
                {
                    _esito = aspettaRisposta(TimeoutRisposta, 0, true, false);
                }
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }




        /// <summary>
        /// Attende che la scheda sia responsive cercando di aprire il canale di comunicazione.
        /// </summary>
        /// <param name="AttesaIniziale">Millisecondi di attesa iniziale prima di cominciare col polling.</param>
        /// <param name="Timeout">Timeout complessivo per l'operazione.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool AttendiRiconnessione(int AttesaIniziale = 10, int Timeout = 5000)
        {
            DateTime _startFunzione;
            bool _esito = false;
            bool _connessioneAttiva = false;

            try
            {

                _startFunzione = DateTime.Now;

                // innazitutto aspetto il tempo di attesa iniziale
                if (AttesaIniziale > 0)
                {
                    Thread.Sleep(AttesaIniziale);
                }

                _connessioneAttiva = StartComunicazione();

                while (!_connessioneAttiva)
                {
                    if (raggiuntoTimeout(_startFunzione, Timeout)) break;
                    _connessioneAttiva = StartComunicazione();

                }
                _esito = _connessioneAttiva;

                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

/*
        public bool CaricaCicli()
        {
            try
            {
                bool _esito;
                CicliInMemoria = new System.Collections.Generic.List<SerialMessage.CicloDiCarica>();
                if (_mS.CicliPresenti.NumCicli > 0)
                {
                    ControllaAttesa(UltimaScrittura);
                    _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                    _mS.Comando = SerialMessage.TipoComando.CMD_READ_CYCLE_CRG;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    Log.Debug("CICLI");
                    //_parametri.serialeCorrente.Write(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                    _esito = aspettaRisposta(AttesaTimeout);
                    CicliPresenti = _mS.CicliPresenti;
                }

                return true; //_esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

*/
        public bool LeggiOrologio()
        {
            try
            {
                bool _esito;

                ControllaAttesa(UltimaScrittura);

                //_mS.Comando = (SerialMessage.TipoComando)0xD3;// SerialMessage.TipoComando.CMD_READ_RTC;
                _mS.Comando = SerialMessage.TipoComando.CMD_READ_RTC;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("Leggi RTC");
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                _esito = aspettaRisposta(AttesaTimeout);
                OrologioSistema = _mS.DatiRTC;

                return _esito;

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        /// <summary>
        /// Legge la programmazione attiva in pos. 0
        /// </summary>
        /// <returns></returns>
        public bool LeggiCicloAttivo()
        {
            try
            {
                bool _esito;
                ControllaAttesa(UltimaScrittura);



                _mS.Comando = SerialMessage.TipoComando.CMD_READ_CYCLE_PROG;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("Leggi Ciclo Programmato LL");
                Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(AttesaTimeout, 1, true);
                CicloInMacchina = _mS.CicloInMacchina;


                return _esito;

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }






        public bool ControllaStatoAreaFW(byte Area = 1)
        {
            try
            {
                bool _esito;
                ControllaAttesa(UltimaScrittura);
                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_CTRL_APP;
                _mS.ComponiMessaggioVerificaAreaFW(Area);
                _rxRisposta = false;
                Log.Debug("Leggi stato area FW LL");
                Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(AttesaTimeout, 1, true);
                CicloInMacchina = _mS.CicloInMacchina;


                return _esito;

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }



        public bool LeggiVariabili()
        {
            try
            {
                bool _esito;

                ControllaAttesa(UltimaScrittura);
                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_READ_VARIABLE;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("Leggi Variabili LL");
                Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(AttesaTimeout, 1, true);

                if (_esito)
                {
                    llVariabiliAttuali = new MoriData.llVariabili();
                    llVariabiliAttuali.IdApparato = "00";
                    llVariabiliAttuali.IstanteLettura = DateTime.Now;
                    llVariabiliAttuali.TensioneTampone = 0;
                    llVariabiliAttuali.TensioneIstantanea = _mS.VariabiliAttuali.Vbatt;
                    llVariabiliAttuali.CorrenteIstantanea = _mS.VariabiliAttuali.Ibatt;
                    llVariabiliAttuali.AhCaricati = _mS.VariabiliAttuali.AhCaricati;
                    llVariabiliAttuali.SecondsFromStart = _mS.VariabiliAttuali.SecondiTrascorsi;
                    llVariabiliAttuali.StatoLL = _mS.VariabiliAttuali.StatoCorrente;

                    //llVariabiliAttuali.IstanteLettura = DateTime.now();

                }

                //CicloInMacchina = _mS.CicloInMacchina;


                return _esito;

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        public bool ProxySBSig60(ref byte[] PacchettoDati)
        {
            try
            {
                bool _esito;
                ControllaAttesa(UltimaScrittura);

                _mS.DatiStrategia = new SerialMessage.ProxyComandoStrategia();
                DatiRisposta = new byte[240];
                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_SIG60_PROXY;
                _mS.ComponiMessaggioNew(PacchettoDati);
                _rxRisposta = false;
                Log.Debug("---------------------------------------------------------------------------");
                Log.Debug("Leggi ProxySBSig60 LL");
                Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                Log.Debug("---------------------------------------------------------------------------");

                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                //  _esito = aspettaRisposta(AttesaTimeout, 1, true);
                _esito = aspettaRisposta(140, 1, true);

                if (_esito)
                {
                    for (int _i = 0; _i < 240; _i++)
                    {
                        DatiRisposta[_i] = _mS.DatiStrategia.RxBuffer[_i];
                    }

                }
                PacchettoDati = DatiRisposta;

                return _esito;

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        public bool LeggiMemoriaScheda()
        {
            try
            {
                bool _esito;

                ControllaAttesa(UltimaScrittura);

                _mS.Comando = SerialMessage.TipoComando.CMD_READ_ALL_MEMORY;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("Leggi Ciclo Programmato");
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                object Risposta;

                _esito = aspettaRisposta(AttesaTimeout, out Risposta, 2048000, false, false, elementiComuni.tipoMessaggio.DumpMemoria);
                CicloInMacchina = _mS.CicloInMacchina;

                return _esito;

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        /// <summary>
        /// Carico direttamente da memoria l'area passata come parametro
        /// </summary>
        /// <param name="StartAddr">Indirizzo (iniziale) del blocco da leggere</param>
        /// <param name="NumByte">Numero di byte da leggere (max 242)</param>
        /// <param name="Dati">bytearray dati letti</param>
        /// <returns></returns>
        public bool LeggiBloccoMemoria(uint StartAddr, ushort NumByte, out byte[] Dati)
        {


            try
            {
                bool _esito;

                // Verifico che il canale sia attivo

                ControllaAttesa(UltimaScrittura);

                if (NumByte < 1) NumByte = 1;
                if (NumByte > 240 )
                {
                    Dati = null;
                    return false;
                }

                Dati = new byte[NumByte];


                _mS.Comando = SerialMessage.TipoComando.CMD_READ_MEMORY;
                _mS._pacchettoMem = new SerialMessage.PacchettoReadMem();

                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Lettura di " + NumByte.ToString() + " bytes dall'indirizzo " + StartAddr.ToString("X2"));

                _mS.ComponiMessaggioLeggiMem(StartAddr, NumByte);
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                Log.Debug(_mS.hexdumpArray(_mS._pacchettoMem.memDataDecoded));

                for (int _ciclo = 0; ((_ciclo < NumByte) && (_ciclo < _mS._pacchettoMem.numBytes)); _ciclo++)
                {

                    Dati[_ciclo] = _mS._pacchettoMem.memDataDecoded[_ciclo];
                }

                return _esito;


            }


            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                Dati = null;
                return false;
            }
        }


        /// <summary>
        /// Scrivi il blocco dati in memoria flash.
        /// </summary>
        /// <param name="StartAddr">The start addr.</param>
        /// <param name="NumByte">The number of byte.</param>
        /// <param name="Dati">The data.</param>
        /// <returns></returns>
        public bool ScriviBloccoMemoria(uint StartAddr, ushort NumByte, byte[] Dati, bool modoDeso = false)
        {
            try
            {
                bool _esito = true;

                ControllaAttesa(UltimaScrittura);

                if (NumByte < 0) NumByte = 0;
                if (NumByte > 236)
                {
                    return false;
                }

                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_WRITE_MEMORY;


                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Scrittura di " + NumByte.ToString() + " bytes dall'indirizzo " + StartAddr.ToString("X2"));

                _mS.ComponiMessaggioScriviMem(StartAddr, NumByte, Dati);
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);

                Log.Debug(_mS.hexdumpMessaggio());

                return _esito;


            }


            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        public bool ControllaAttesa(DateTime UltimoIstante, int MilliecondiTimeOut = 4800)
        {
            bool _risposta = false;
            try
            {
                DateTime _ora = DateTime.Now;
                TimeSpan _durata = _ora.Subtract(UltimoIstante);

                Log.Debug("Controllo attesa ------>> Last:" + UltimoIstante.ToString() + " - Tempo attesa: " + _durata.TotalMilliseconds.ToString());

                if (_durata.TotalMilliseconds > MilliecondiTimeOut)
                {
                    Log.Debug("------>>           Tempo Stanby comunicazione superato");
                    _risposta = StartComunicazione();
                }

                return _risposta;
            }

            catch (Exception Ex)
            {
                Log.Error("ControllaAttesa: " + Ex.Message);
                _lastError = Ex.Message;
                return _risposta;
            }

        }



        /// <summary>
        /// Cancella fisicamente un blocco di 4K dalla memoria flash mettendo tutti i Bytes a 0xFF
        /// </summary>
        /// <returns></returns>
        public bool CancellaBlocco4K(uint StartAddr)
        {


            try
            {
                bool _esito;
                ControllaAttesa(UltimaScrittura);

                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_ERASE_4K_MEM;

                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Cancellazione di 4Kbytes dall'indirizzo " + StartAddr.ToString("X2"));

                _mS.ComponiMessaggioCancella4KMem(StartAddr);
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);

                //Log.Debug("------------------------------------------------------------------------------------------------------------");

                return _esito;


            }


            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        /// <summary>
        /// Cancella l'intera memoria flash (4MB). Cancella snche i dati relativi alla scheda corrente dal DB Locale
        /// </summary>
        /// <returns></returns>
        public bool CancellaInteraMemoria()
        {


            try
            {
                bool _esito;

                ControllaAttesa(UltimaScrittura);

                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_ERASE_DATA_MEMORY;


                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("SerialMessage.TipoComando.SB_CancellaInteraMemoria");

                _mS.ComponiMessaggio();
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, true);
                // prima di proseguire aspetto 1 secondo
                System.Threading.Thread.Sleep(1000);
                //Application.DoEvents();

                Log.Debug(_mS.hexdumpMessaggio());
                Log.Debug("------------------------------------------------------------------------------------------------------------");

                return _esito;


            }


            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        public bool ScriviCicloCorrente()
        {
            try
            {
                bool _esito;

                ControllaAttesa(UltimaScrittura);

                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_PROG_CYCLE_CRG;
                _mS.CicloInMacchina = CicloInMacchina;
                _mS.ComponiMessaggioCicloProgrammato();
                _rxRisposta = false;
                Log.Debug("Scrivi Ciclo Programmato");
                Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                //                _parametri.serialeCorrente.Write(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                _esito = aspettaRisposta(AttesaTimeout, 0, true);

                return _esito;

                //   }
                //  return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        public bool ScriviOrologio()
        {
            try
            {
                bool _esito;

                ControllaAttesa(UltimaScrittura);

                DateTime _now = DateTime.Now;

                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_UPDATE_RTC;
                _mS.DatiRTC = new SerialMessage.comandoRTC();
                _mS.DatiRTC.anno = (ushort)_now.Year;
                _mS.DatiRTC.mese = (byte)_now.Month;
                _mS.DatiRTC.giorno = (byte)_now.Day;
                _mS.DatiRTC.giornoSett = (byte)_now.DayOfWeek;
                _mS.DatiRTC.ore = (byte)_now.Hour;
                _mS.DatiRTC.minuti = (byte)_now.Minute;
                _mS.DatiRTC.secondi = (byte)_now.Second;

                _mS.ComponiMessaggioOra();
                _rxRisposta = false;
                Log.Debug("Scrivi RTC");
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                _esito = aspettaRisposta(AttesaTimeout, 0, true, false);

                return _esito; //_esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        public bool RiceviMessaggio()
        {
            try {

                return false;
            }
            catch { return false; }

        }


        public bool primaConnessione()
        {
            try
            {

                return true;
            }
            catch
            {
                return false;
            }

        }



        public bool LeggiParametriApparato()
        {
            try
            {
                bool _esito;
                ParametriApparato = new llParametriApparato();
                MessaggioLadeLight.PrimoBloccoMemoria ImmagineMemoria = new MessaggioLadeLight.PrimoBloccoMemoria();
                SerialMessage.EsitoRisposta EsitoMsg;

                byte[] _datiTemp = new byte[240];
                _esito = LeggiBloccoMemoria(0, 240, out _datiTemp);

                if (_esito)
                {
                    EsitoMsg = ImmagineMemoria.analizzaMessaggio(_datiTemp, 1);
                    if (EsitoMsg == SerialMessage.EsitoRisposta.MessaggioOk)
                    {
                        ParametriApparato.llParApp.ProduttoreApparato = ImmagineMemoria.ProduttoreApparato;
                        ParametriApparato.llParApp.NomeApparato = ImmagineMemoria.NomeApparato;
                        ParametriApparato.llParApp.SerialeApparato = ImmagineMemoria.SerialeApparato;
                        ParametriApparato.llParApp.AnnoCodice = ImmagineMemoria.AnnoCodice;
                        ParametriApparato.llParApp.ProgressivoCodice = ImmagineMemoria.ProgressivoCodice;
                        ParametriApparato.llParApp.TipoApparato = ImmagineMemoria.TipoApparato;
                        ParametriApparato.llParApp.DataSetupApparato = ImmagineMemoria.DataSetupApparato;

                        ParametriApparato.llParApp.SerialeZVT = ImmagineMemoria.SerialeZVT;
                        ParametriApparato.llParApp.HardwareZVT = ImmagineMemoria.HardwareZVT;
                        ParametriApparato.llParApp.IdLottoZVT = ImmagineMemoria.LottoZVT;

                        ParametriApparato.llParApp.SerialePFC = ImmagineMemoria.SerialePFC;
                        ParametriApparato.llParApp.HardwarePFC = ImmagineMemoria.HardwarePFC;
                        ParametriApparato.llParApp.SoftwarePFC = ImmagineMemoria.SoftwarePFC;

                        ParametriApparato.llParApp.SerialeDISP = ImmagineMemoria.SerialeDISP;
                        ParametriApparato.llParApp.HardwareDisp = ImmagineMemoria.HardwareDisp;
                        ParametriApparato.llParApp.SoftwareDISP = ImmagineMemoria.SoftwareDISP;

                        ParametriApparato.llParApp.MaxRecordBrevi = ImmagineMemoria.MaxRecordBrevi;
                        ParametriApparato.llParApp.MaxRecordCarica = ImmagineMemoria.MaxRecordCarica;
                        ParametriApparato.llParApp.SizeExternMemory = ImmagineMemoria.SizeExternMemory;
                        ParametriApparato.llParApp.MaxProgrammazioni = ImmagineMemoria.MaxProgrammazioni;
                        ParametriApparato.llParApp.ModelloMemoria = ImmagineMemoria.ModelloMemoria;
                        ParametriApparato.llParApp.IdApparato = ImmagineMemoria.IDApparato;

                        ParametriApparato.llParApp.VMin = ImmagineMemoria.VMin;
                        ParametriApparato.llParApp.VMax = ImmagineMemoria.VMax;
                        ParametriApparato.llParApp.Amax = ImmagineMemoria.Amax;
                        ParametriApparato.llParApp.PresenzaRabboccatore = ImmagineMemoria.PresenzaRabboccatore;

                    }

                }


                return _esito;

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

        public bool ScriviParametriApparato()
        {
            try
            {
                bool _esito = false;
                MessaggioLadeLight.PrimoBloccoMemoria ImmagineMemoria = new MessaggioLadeLight.PrimoBloccoMemoria();
                SerialMessage.EsitoRisposta EsitoMsg;



                // Dati ripresi tal quale dall'originale

                ImmagineMemoria.ProduttoreApparato = ParametriApparato.llParApp.ProduttoreApparato;
                ImmagineMemoria.NomeApparato = ParametriApparato.llParApp.NomeApparato;
                ImmagineMemoria.MaxRecordBrevi = ParametriApparato.llParApp.MaxRecordBrevi;
                ImmagineMemoria.MaxRecordCarica = ParametriApparato.llParApp.MaxRecordCarica;
                ImmagineMemoria.SizeExternMemory = ParametriApparato.llParApp.SizeExternMemory;
                ImmagineMemoria.MaxProgrammazioni = ParametriApparato.llParApp.MaxProgrammazioni;
                ImmagineMemoria.ModelloMemoria = ParametriApparato.llParApp.ModelloMemoria;

                
                ImmagineMemoria.SerialeApparato = ParametriApparato.llParApp.SerialeApparato;
                ImmagineMemoria.TipoApparato = ParametriApparato.llParApp.TipoApparato;
                ImmagineMemoria.DataSetupApparato = ParametriApparato.llParApp.DataSetupApparato;

                ImmagineMemoria.SerialeZVT = ParametriApparato.llParApp.SerialeZVT;
                ImmagineMemoria.HardwareZVT = ParametriApparato.llParApp.HardwareZVT;

                ImmagineMemoria.SerialePFC = ParametriApparato.llParApp.SerialePFC;
                ImmagineMemoria.SoftwarePFC = ParametriApparato.llParApp.SoftwarePFC;
                ImmagineMemoria.HardwarePFC = ParametriApparato.llParApp.HardwarePFC;
                ImmagineMemoria.SerialeDISP = ParametriApparato.llParApp.SerialeDISP;
                ImmagineMemoria.HardwareDisp = ParametriApparato.llParApp.HardwareDisp;
                ImmagineMemoria.SoftwareDISP = ParametriApparato.llParApp.SoftwareDISP;

                ImmagineMemoria.IDApparato = ParametriApparato.llParApp.IdApparato;

                ImmagineMemoria.VMin = ParametriApparato.llParApp.VMin;
                ImmagineMemoria.VMax = ParametriApparato.llParApp.VMax;
                ImmagineMemoria.Amax = ParametriApparato.llParApp.Amax;
                ImmagineMemoria.PresenzaRabboccatore = ParametriApparato.llParApp.PresenzaRabboccatore;



                if (ImmagineMemoria.GeneraByteArray())
                {
                    // da far diventare unica
                    _esito = CancellaBlocco4K(0);

                    byte[] _datiTemp = new byte[2];
                    _datiTemp = ImmagineMemoria.dataBuffer;
                    _esito = ScriviBloccoMemoria(0x00, 236, _datiTemp);
                }


                return _esito;

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

        public bool SalvaProgrammazioniApparato()
        {
            try
            {
                bool _esito = false;

                MessaggioLadeLight.MessaggioProgrammazione NuovoPrg = new MessaggioLadeLight.MessaggioProgrammazione();
                SerialMessage.EsitoRisposta EsitoMsg;

                // Prima vuoto il blocco  --> inserire la gestione dell'intera pagina
                _esito = CancellaBlocco4K(0x2000);

                NuovoPrg.TipoProgrammazione = 0;
                NuovoPrg.OpzioniProgrammazione = 0;
                NuovoPrg.IdProgrammazione = ProgrammaAttivo.IdProgramma;
                NuovoPrg.ProgInUse = ProgrammaAttivo.ProgrammaInUso;
                NuovoPrg.NomeCiclo = ProgrammaAttivo.ProgramName;
                NuovoPrg.DataInserimento = null;
                NuovoPrg.IdProfilo = ProgrammaAttivo.IdProfilo;

                NuovoPrg.Parametri = new List<ParametroLL>();
                NuovoPrg.Parametri.Clear();


                foreach (ParametroLL _par in ProgrammaAttivo.ListaParametri)
                {
                    NuovoPrg.Parametri.Add(_par);
                }


                if (NuovoPrg.GeneraByteArray())
                {
                    byte[] _datiTemp = new byte[226];
                    _datiTemp = NuovoPrg.dataBuffer;
                    _esito = ScriviBloccoMemoria(0x2000, 226, _datiTemp);
                }


                return _esito;

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }




        /*************************************************************************************************************************************/

        private void port_DataReceivedCBOLD(object sender, SerialDataReceivedEventArgs e)
        {
            //string testom = "Dati Ricevuti CB: " + serialeApparato.BytesToRead.ToString();
            bool _trovatoETX = false;
            byte[] data = new byte[serialeApparato.BytesToRead];
            serialeApparato.Read(data, 0, data.Length);
            Log.Debug("Dati Ricevuti CB: " + data.Length.ToString());
            for (int _i = 0; _i < data.Length; _i++)
            {
                codaDatiSER.Enqueue(data[_i]);
                if (data[_i] == SerialMessage.serETX)
                {
                    _trovatoETX = true;
                }
            }
            if (_trovatoETX)
            {
                Log.Debug("trovato ETX");
                analizzaCoda();
            }

        }
        /*************************************************************************************************/

        private void port_DataReceivedCB(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //string testom = "Dati Ricevuti SB: " + serialeApparato.BytesToRead.ToString();
                bool _trovatoETX = false;
                if (serialeApparato != null)
                {
                    byte[] data = new byte[serialeApparato.BytesToRead];
                    serialeApparato.Read(data, 0, data.Length);
                    Log.Debug("Dati Ricevuti CB " + data.Length.ToString());
                    for (int _i = 0; _i < data.Length; _i++)
                    {
                        codaDatiSER.Enqueue(data[_i]);
                        if (data[_i] == SerialMessage.serETX)
                        {
                            _trovatoETX = true;
                        }
                    }
                    if (_trovatoETX)
                    {
                        Log.Debug("trovato ETX");
                        analizzaCoda();
                    }

                }
            }
            catch (Exception Ex)
            {
                Log.Error("port_DataReceivedCB: " + Ex.Message);
            }

        }

        private void usb_DataReceivedSb()
        {
            try
            {
                bool _trovatoETX = false;

                // mi metto in ascolto sul canale USB fino a EOT o a timeout 
                // aspetto 10 mS 
                // System.Threading.Thread.Sleep(100);

                // Check the amount of data available to read
                // In this case we know how much data we are expecting, 
                // so wait until we have all of the bytes we have sent.
                Log.Warn("Lettura USB: coda iniziale " + codaDatiSER.Count.ToString() + " bytes");

                uint numBytesAvailable = 0;
                FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

                ftStatus = _parametri.usbLadeLight.GetRxBytesAvailable(ref numBytesAvailable);
                if (ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    // Errore nella lettura dati disponibili
                    Log.Warn("Failed to get number of bytes available to read (error " + ftStatus.ToString() + ")");
                }
                if (numBytesAvailable > 0)
                {
                    // Now that we have the amount of data we want available, read it
                    byte[] readData = new byte[numBytesAvailable];
                    uint numBytesRead = 0;
                    // Note that the Read method is overloaded, so can read string or byte array data
                    ftStatus = _parametri.usbLadeLight.Read(readData, numBytesAvailable, ref numBytesRead);
                    if (ftStatus != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        Log.Warn("Failed to read data (error " + ftStatus.ToString() + ") - Availables " + numBytesAvailable.ToString());
                        //return false;
                    }

                    Log.Debug("Dati Ricevuti SB (USB)" + numBytesRead.ToString());
                    for (int _i = 0; _i < numBytesRead; _i++)
                    {

                        codaDatiSER.Enqueue(readData[_i]);
                        if (readData[_i] == SerialMessage.serETX)
                        {
                            Log.Debug("Trovato Etx (USB)");
                            _trovatoETX = true;
                        }
                    }
                    if (_trovatoETX)
                    {
                        Log.Debug("Trovato Etx (USB) --> Analizza Coda");
                        analizzaCoda();
                    }

                }

            }
            catch (Exception Ex)
            {
                Log.Error("USB_DataReceivedSb: " + Ex.Message);
            }

        }


        /******************************************************************************************************/

        private SerialMessage.TipoRisposta analizzaCoda()
        {

            SerialMessage.EsitoRisposta _esito;
            SerialMessage.TipoRisposta _datiRicevuti = SerialMessage.TipoRisposta.NonValido;
            bool _trovatoSTX = false;
            byte _tempByte;
            string testom = "";
            bool _inviaRisposta = true;

            try
            {
                testom = "LUNGHEZZA CODA: " + codaDatiSER.Count();
                Log.Debug(testom);

                testom = "";

                while (codaDatiSER.Count() > 0)
                {
                    if (codaDatiSER.Contains(SerialMessage.serETX) == false)
                    {

                        Log.Debug("NON trovato ETX");
                        _datiRicevuti = SerialMessage.TipoRisposta.NonValido;
                        return _datiRicevuti;
                    }


                    _tempByte = codaDatiSER.Peek();
                    if (_tempByte != SerialMessage.serSTX)
                    {
                        _tempByte = codaDatiSER.Dequeue();
                    }
                    else
                    {
                        while (_tempByte != SerialMessage.serETX)
                        {
                            int lastByte = _dataBuffer.Length;
                            Array.Resize(ref _dataBuffer, lastByte + 1);
                            _tempByte = codaDatiSER.Dequeue();
                            _dataBuffer[lastByte] = _tempByte;
                            testom += _tempByte.ToString("X2");
                        }
                        Log.Debug(testom);
                        testom = "";
                        Log.Debug("Trovato ETX LL");
                        readingMessage = false;

                        _mS.MessageBuffer = _dataBuffer;
                        _esito = _mS.analizzaMessaggio(_dataBuffer);
                        //UltimaRisposta = SerialMessage.EsitoRisposta.MessaggioOk;
                        _inviaRisposta = true;
                        Log.Debug("Comando LL: --> 0x" + _mS._comando.ToString("X2"));

                        switch (_mS._comando)
                        {
                            //case (byte)SerialMessage.TipoComando.ACK:
                            case (byte)SerialMessage.TipoComando.ACK_PACKET:
                                Log.Debug("Risposta Ricevuta: ACK");
                                TipoRisposta = 1;
                                _datiRicevuti = SerialMessage.TipoRisposta.Ack;
                                _inviaRisposta = false;
                                break;
                            //case (byte)SerialMessage.TipoComando.NACK:
                            case (byte)SerialMessage.TipoComando.NACK_PACKET:

                                TipoRisposta = 2;
                                _datiRicevuti = SerialMessage.TipoRisposta.Nack;
                                Log.Debug("--------------------------------- Risposta Ricevuta: NACK -----------------------------");
                                _inviaRisposta = false;
                                break;
                            case (byte)SerialMessage.TipoComando.CMD_UART_HOST_CONNECTED: // Prima Lettura
                                Log.Debug("Prima Lettura");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                break;
                            case (byte)SerialMessage.TipoComando.CMD_READ_CYCLE_PROG:
                                Log.Debug("Ciclo Programmato");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                break;
                            case (byte)SerialMessage.TipoComando.CMD_READ_VARIABLE:
                                Log.Debug("Variabili");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = true;
                                break;
                            case (byte)SerialMessage.TipoComando.CMD_SIG60_PROXY:
                                Log.Debug("SIG60 Proxy");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = true;
                                break;
                            case (byte)SerialMessage.TipoComando.CMD_INFO_BL:
                                Log.Debug("INFO Bootloader");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                break;

                            case (byte)SerialMessage.TipoComando.CMD_FW_UPDATE:
                                Log.Debug("ISwitch App");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                break;

                            case (byte)SerialMessage.TipoComando.CMD_CTRL_APP:
                                Log.Debug("LL CMD_CTRL_APP");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                break;

                            case (byte)SerialMessage.TipoComando.CMD_READ_LT_MEMORY:
                                Log.Debug("Cicli in memoria");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                break;
                            case (byte)SerialMessage.TipoComando.CMD_READ_RTC:
                                Log.Debug("Read RTC");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                break;
                            case (byte)SerialMessage.TipoComando.CMD_READ_MEMORY:
                                Log.Debug("Read MEMORY");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                break;
                            case (byte)SerialMessage.TipoComando.EVENT_MEM_CODE:
                                Log.Debug("MEMORY Event");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                break;
                            default:
                                Log.Debug("Altro Comando");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                break;
                        }
                        _rxRisposta = true;
                        Array.Resize(ref _dataBuffer, 0);

                        if (_inviaRisposta)
                        {
                            Log.Debug("Esito: " + _mS._comando.ToString("X2"));

                            _mS._comando = (byte)SerialMessage.TipoComando.ACK_PACKET;
                            _mS._dispositivo = 0x0000;
                            _mS.componiRisposta(_dataBuffer, _esito);

                            _parametri.scriviMessaggioLadeLight(_mS.messaggioRisposta, 0, _mS.messaggioRisposta.Length);
                            Log.Debug("Mandato ACK LL");
                            Log.Debug(_mS.hexdumpArray(_mS.messaggioRisposta));
                        }
                        _rxRisposta = true;
                        Array.Resize(ref _dataBuffer, 0);
                        //return _datiRicevuti;

                    }

                }
                testom = "Lunghezza coda in uscita: " + codaDatiSER.Count();
                Log.Debug(testom);
                return _datiRicevuti;
            }
            catch (Exception Ex)
            {
                Log.Error("analizzaCoda " + Ex.Message);
                _lastError = Ex.Message;
                _datiRicevuti = SerialMessage.TipoRisposta.NonValido;
                return _datiRicevuti;
            }
        }


    }

}
