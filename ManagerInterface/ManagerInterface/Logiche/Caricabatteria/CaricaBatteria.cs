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

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
        delegate void SetTextCallback(string text);
        string InputData = String.Empty;
        byte[] _dataBuffer = new byte[0];
        int lastByte = 0;
        bool readingMessage = false;
        public int TipoRisposta;
        static bool _rxRisposta ;
        public SerialMessage.comandoIniziale Intestazione = new SerialMessage.comandoIniziale();
        public SerialMessage.cicliPresenti CicliPresenti = new SerialMessage.cicliPresenti();
        public SerialMessage.comandoRTC OrologioSistema = new SerialMessage.comandoRTC();
        public SerialMessage.cicloAttuale CicloInMacchina = new SerialMessage.cicloAttuale();
        public SerialMessage.VariabiliLadeLight VaribiliAttuali = new SerialMessage.VariabiliLadeLight();

        public llVariabili llVariabiliAttuali = new llVariabili();
        public System.Collections.Generic.List< SerialMessage.CicloDiCarica> CicliInMemoria = new System.Collections.Generic.List< SerialMessage.CicloDiCarica>();

        /* ----------------------------------------------------------
        *  Dichiarazione eventi per la gestione avanzamento
        * ---------------------------------------------------------
        */
        public event StepHandler Step;
        public delegate void StepHandler(CaricaBatteria ull, ProgressChangedEventArgs e); //sbWaitEventStep e);
                                                                                          // ----------------------------------------------------------

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
                                    if(codaDatiSER.Count >= risposteAttese)
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
                        Log.Debug("Aspetta risposta LL ricevuti " + _risposteRicevute.ToString() + " di " + risposteAttese.ToString() + " in " + DateTime.Now.Subtract(_startRicezione).ToString() + " secondi" );
                    }
                    else
                    {
                        Log.Debug("Aspetta risposta LL - raggiunto timeout " +  DateTime.Now.Subtract(_startRicezione).ToString() + " secondi");

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
                double _decimiEffettivi = _durata.TotalSeconds * 10;

                if (_decimiEffettivi > DecimiTimeOut)
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
                _parametri = parametri;
                _mS = new MessaggioLadeLight();
                _mS.Dispositivo = SerialMessage.TipoDispositivo.Charger;
                byte[] Seriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
                _mS.SerialNumber = Seriale;
                _cbCollegato = false;
                serialeApparato = _parametri.serialeCorrente;
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


        public bool VerificaPresenza()
        {
            try
            {
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
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            } 
        }


        public bool CaricaCicli()
        {
            try
            {
                bool _esito;
                CicliInMemoria = new System.Collections.Generic.List< SerialMessage.CicloDiCarica>();
                if (_mS.CicliPresenti.NumCicli > 0)
                {
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


        public bool LeggiOrologio()
        {
            try
            {
                bool _esito;

                _mS.Comando = SerialMessage.TipoComando.ReadRTC;
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

        public bool LeggiCicloAttuale()
        {
            try
            {
                bool _esito;

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

        public bool LeggiVariabili()
        {
            try
            {
                bool _esito;

                _mS.Comando = SerialMessage.TipoComando.SB_R_Variabili;
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

        public bool ProxySBSig60(ref byte[] PacchettoDati )
        {
            try
            {
                bool _esito;
                _mS.DatiStrategia = new SerialMessage.ProxyComandoStrategia();
                DatiRisposta = new byte[240];

                _mS.Comando = SerialMessage.TipoComando.LL_SIG60_PROXY;
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
                    for (int _i= 0; _i<240; _i++)
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

                _mS.Comando = SerialMessage.TipoComando.SB_R_DumpMemoria;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("Leggi Ciclo Programmato");
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                object Risposta;

                _esito = aspettaRisposta(AttesaTimeout, out Risposta,2048000,false,false,elementiComuni.tipoMessaggio.DumpMemoria);
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

                if (NumByte < 1) NumByte = 1;
                if (NumByte > 242)
                {
                    Dati = null;
                    return false;
                }

                Dati = new byte[NumByte];


                _mS.Comando = SerialMessage.TipoComando.LL_R_LeggiMemoria;
                _mS._pacchettoMem = new SerialMessage.PacchettoReadMem();

                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Lettura di " + NumByte.ToString() + " bytes dall'indirizzo " + StartAddr.ToString("X2"));

                _mS.ComponiMessaggioLeggiMem(StartAddr, NumByte);
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
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

                if (NumByte < 0) NumByte = 0;
                if (NumByte > 242)
                {
                    return false;
                }


                _mS.Comando = SerialMessage.TipoComando.SB_W_ScriviMemoria;


                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Scrittura di " + NumByte.ToString() + " bytes dall'indirizzo " + StartAddr.ToString("X2"));

                _mS.ComponiMessaggioScriviMem(StartAddr, NumByte, Dati);
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                if (modoDeso != true)
                {
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                }


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

        /// <summary>
        /// Cancella fisicamente un blocco di 4K dalla memoria flash mettendo tutti i Bytes a 0xFF
        /// </summary>
        /// <returns></returns>
        public bool CancellaBlocco4K(uint StartAddr)
        {


            try
            {
                bool _esito;

                _mS.Comando = SerialMessage.TipoComando.SB_Cancella4K;

                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Cancellazione di 4Kbytes dall'indirizzo " + StartAddr.ToString("X2"));

                _mS.ComponiMessaggioCancella4KMem(StartAddr);
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
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

                _mS.Comando = SerialMessage.TipoComando.LL_CancellaInteraMemoria;


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

                _mS.Comando = SerialMessage.TipoComando.CMD_PRG_CYCLE_CRG;
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
                DateTime _now = DateTime.Now; 

                _mS.Comando = SerialMessage.TipoComando.UpdateRTC;
                _mS.DatiRTC = new SerialMessage.comandoRTC();
                _mS.DatiRTC.anno = ( ushort ) _now.Year;
                _mS.DatiRTC.mese = ( byte ) _now.Month;
                _mS.DatiRTC.giorno = ( byte ) _now.Day;
                _mS.DatiRTC.giornoSett = ( byte ) _now.DayOfWeek;
                _mS.DatiRTC.ore = ( byte ) _now.Hour;
                _mS.DatiRTC.minuti = ( byte ) _now.Minute;
                _mS.DatiRTC.secondi = ( byte ) _now.Second;

                _mS.ComponiMessaggioOra();
                _rxRisposta = false;
                Log.Debug("Scrivi RTC");
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                _esito = aspettaRisposta(AttesaTimeout, 0,true,false);

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
                            case (byte)SerialMessage.TipoComando.ACK:
                            case (byte)SerialMessage.TipoComando.ACK_PACKET:
                                Log.Debug("Risposta Ricevuta: ACK");
                                TipoRisposta = 1;
                                _datiRicevuti = SerialMessage.TipoRisposta.Ack;
                                _inviaRisposta = false;
                                break;
                            case (byte)SerialMessage.TipoComando.NACK:
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
                            case (byte)SerialMessage.TipoComando.SB_R_Variabili:
                                Log.Debug("Variabili");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = true;
                                break;
                            case (byte)SerialMessage.TipoComando.LL_SIG60_PROXY:
                                Log.Debug("SIG60 Proxy");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = true;
                                break;
                            case 0x03: // Cicli in memoria
                                Log.Debug("Cicli in memoria");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                break;
                            case 0xD3: // read RTC
                                Log.Debug("Read RTC");
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
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

                            _mS._comando = (byte)SerialMessage.TipoComando.ACK;
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
