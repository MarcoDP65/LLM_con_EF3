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

using SQLite;
using System.Windows.Forms;

namespace ChargerLogic
{
    public delegate void ChargerEvent<CBEventArgs>(CaricaBatteria me, CBEventArgs EvArgs);

    public partial class CaricaBatteria
    {
        public const int ADDR_START_PROGRAMMAZIONI = 0x2000;
        public const byte ADDR_START_PAR_PROGRAM = 0xF6;

        public const int ADDR_START_CONTATORI = 0x3000;
        public const int ADDR_START_BACKUP_CONTATORI = 0x4000;

        public const int ADDR_START_RECORD_LUNGHI = 0x1B3000;
        public const int LEN_AREA_RECORD_LUNGHI = 0x4000;

        public const int ADDR_START_RECORD_BREVI = 0x6000;
        public const int LEN_AREA_RECORD_BREVI = 0x1AD000;
        public const int BLOCCHI_RECORD_BREVI = 429;
        public const int LEN_RECORD_BREVE = 30;

        public const int DATABLOCK = 128;


        public enum EsitoRicalcolo : byte { 
            OK = 0x00, 
            ErrIMax = 0x11, 
            ErrIMin = 0x12, 
            ErrVMax = 0x21, 
            ErrVMin = 0x22, 
            ErrValoreRichiesto = 0x41,
            ErrGenerico = 0xF0, 
            ParNonValidi = 0xF1, 
            ErrCBNonDef = 0xF2,
            CapBattNonValida = 0xF4,
            ErrUndef = 0xFF 
        }
        public enum TipoCaricaBatteria : byte { Generico = 0x00, LadeLight = 0x01, SuperCharger = 0x02 , NonDefinito = 0xFF}


        public SerialPort serialeApparato;
        private static MessaggioLadeLight _mS;
        private parametriSistema _parametri;
        private static Queue<byte> codaDatiSER = new Queue<byte>();

        public DateTime UltimaScrittura;   // Registro l'istante dell'ultima scrittura
        public string UltimoMsgErrore;

        private static ILog Log = LogManager.GetLogger("CaricaBatteria");
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

        public BaudRate BaudRateUSB;
        public event ChargerEvent<CBEventArgs> OnBaudRateChange;

        public cbProgrammazioni Programmazioni = new cbProgrammazioni();

        public llParametriApparato ParametriApparato = new llParametriApparato();
        public _llModelloCb ModelloCorrente;

        public llMappaMemoria Memoria = new llMappaMemoria(1);
        public llContatoriApparato ContatoriLL = new llContatoriApparato();
        public LadeLightData ApparatoLL;
        public llDatiCliente DatiCliente;

        public llVariabili llVariabiliAttuali = new llVariabili();

        public byte[] BloccoLunghi;
        public byte[] BloccoBrevi;


        /*
        public ushort UltimoIdProgamma { get; set; }
        public byte NumeroRecordProg { get; set; }
        public llProgrammaCarica ProgrammaAttivo;
        public List<llProgrammaCarica> ProgrammiDefiniti;
        */
        public DatiConfigCariche DatiBase { get; set; }

        public List<llMemoriaCicli> MemoriaCicli;
        public llLvwCaricheSortableDS MemCicliLvwDS;

        public List<llMemBreve> BreviCicloCorrente;

        public llDataModel ModelloDati = new llDataModel();


        //public const byte SizeCharge = 36;
        public const byte SizeShort = 30;
        public const UInt32 FirstShort = 0x006000;   // dal 08/02/2019 spostato  da 0x5000 a 0x6000
        public const UInt32 MaxByteShort = 0x1AEFFF;



        //public System.Collections.Generic.List<SerialMessage.CicloDiCarica> CicliInMemoria = new System.Collections.Generic.List<SerialMessage.CicloDiCarica>();

        // -------------------------------------------------------
        //    Dichiarazione eventi per la gestione avanzamento
        // -------------------------------------------------------

        public bool RichiestaInterruzione = false;

        public event StepHandler Step;
        public delegate void StepHandler(CaricaBatteria ull, ProgressChangedEventArgs e);

        // -------------------------------------------------------


        public llStatoFirmware StatoFirmware = new llStatoFirmware();
        public scStatoFirmware StatoFirmwareSC = new scStatoFirmware();

        private Boolean _firmwarePresente = false;

        private static EventWaitHandle LL_USBeventWait;
        private DateTime _startRead;

        public byte[] numeroSeriale;
        private string _lastError;
        private bool _cbCollegato;
        public bool apparatoPresente = false;
        public MoriData._db DbAttivo;

        public byte[] DatiRisposta;
        public SerialMessage.EsitoRisposta UltimaRisposta;
        private SerialMessage.TipoRisposta _ultimaRisposta = SerialMessage.TipoRisposta.NonValido;   // flag per l'indicazioene del tipo dell'ultimo messaggio ricevuto dalla scheda

        public int AttesaTimeout = 25; // Tempo attesa in decimi di secondo
        public SerialMessage.OcBaudRate BrOCcorrente = SerialMessage.OcBaudRate.OFF;
        public SerialMessage.OcEchoMode EchoOCcorrente = SerialMessage.OcEchoMode.OFF;

        public TipoCaricaBatteria TipoCB = TipoCaricaBatteria.Generico;
        // Solo ad uso test letture
        public int NumeroTentativiLettura;

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
                    // aspetto 100 mS 
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

                            UltimaScrittura = DateTime.Now;

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

        public CaricaBatteria(ref parametriSistema parametri, MoriData._db dbCorrente, string IdDispositivo, TipoCaricaBatteria TipoCharger )
        {

            try
            {

                //ControllaAttesa(UltimaScrittura);

                _parametri = parametri;
                //_mS = new MessaggioLadeLight();
                //_mS.Dispositivo = SerialMessage.TipoDispositivo.Charger;
                //byte[] Seriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
                //_mS.SerialNumber = Seriale;

                _cbCollegato = false;
                serialeApparato = _parametri.serialeCorrente;
                DbAttivo = dbCorrente;
                DatiCliente = new llDatiCliente(dbCorrente);
                DatiBase = new DatiConfigCariche();
                //InizializzaDatiLocali();

                // Programmazioni
                Programmazioni.UltimoIdProgamma = 0;
                Programmazioni.NumeroRecordProg = 0;
                TipoCB = TipoCharger;

                BaudRateUSB = new BaudRate();
                switch ( TipoCB )
                {
                    case TipoCaricaBatteria.LadeLight:
                        BaudRateUSB.Mode = BaudRate.BRType.BR_9600;
                        BaudRateUSB.Speed = 0;
                        break;

                    case TipoCaricaBatteria.SuperCharger:
                        BaudRateUSB.Mode = BaudRate.BRType.BR_115200;
                        BaudRateUSB.Speed = 0;
                        break;

                    default:
                        BaudRateUSB.Mode = BaudRate.BRType.BR_9600;
                        BaudRateUSB.Speed = 0;
                        break;
                }



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




        /// <summary>
        /// Inizializzo il caricabatteria; in base al tipo imposto la velocita di comunicazione di default
        /// </summary>
        /// <param name="parametri"></param>
        /// <param name="dbCorrente"></param>
        /// <param name="TipoCharger"></param>
        public CaricaBatteria(ref parametriSistema parametri, MoriData._db dbCorrente, TipoCaricaBatteria TipoCharger)
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
                TipoCB = TipoCharger;
                serialeApparato = _parametri.serialeCorrente;
                DbAttivo = dbCorrente;
                ApparatoLL = new LadeLightData(dbCorrente);
                DatiCliente = new llDatiCliente(dbCorrente);
                DatiBase = new DatiConfigCariche();
                //InizializzaDatiLocali();

                // Programmazioni
                Programmazioni.UltimoIdProgamma = 0;
                Programmazioni.NumeroRecordProg = 0;

                BaudRateUSB = new BaudRate();
                switch (TipoCB)
                {
                    case TipoCaricaBatteria.LadeLight:
                        BaudRateUSB.Mode = BaudRate.BRType.BR_9600;
                        BaudRateUSB.Speed = 0;
                        break;

                    case TipoCaricaBatteria.SuperCharger:
                        BaudRateUSB.Mode = BaudRate.BRType.BR_115200;
                        BaudRateUSB.Speed = 0;
                        break;

                    default:
                        BaudRateUSB.Mode = BaudRate.BRType.BR_9600;
                        BaudRateUSB.Speed = 0;
                        break;
                }

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

        /// <summary>
        ///  Apro la porta di comunicazione ( porta FISICA, non mando l'inizio comunicazione ) 
        ///  Il BAURATE di apertura è quello definito nei parametri generali
        /// </summary>
        /// <returns></returns>
        public bool apriPorta()
        {
            bool _esito;
            _esito = _parametri.apriLadeLight();
            return _esito;

        }

        /// <summary>
        /// Chiudo la porta di comunicazione ( porta FISICA, non mando il messaggio di fine comunicazione ) 
        /// </summary>
        /// <returns></returns>
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
                bool _esito;
                // prima riapro il canale
                _esito = true; // apriPorta();
                if (_esito)
                {
                    _esito = StartComunicazione();
                    if (_esito)
                    {
                        // se ho aperto la porta leggo la testata
                        _esito = CaricaIntestazioneLL();
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

        public bool CaricaApparatoLL()
        {
            try
            {
                bool _esito = CaricaIntestazioneLL();
                return _esito;
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
                if (_esito)
                {
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

        /// <summary>
        /// Carica i dati base del dispositivo dal primo blocco della memora esterna ( Addr 0x0000 )
        /// </summary>
        /// <param name="RunAsinc"></param>
        /// <param name="StepIniziale"></param>
        /// <param name="StepFinale"></param>
        /// <returns></returns>
        public bool CaricaApparatoA0(bool RunAsinc = false, int StepIniziale = 0, int StepFinale = 100)
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;

                if (RunAsinc)
                {
                    //Preparo l'intestazione della finestra di avanzamento                                                                                                          
                    if (Step != null)
                    {
                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                        _passo.Titolo = "Caricamento Parametri ";
                        _passo.Eventi = 1;
                        _passo.Step = -1;
                        _passo.EsecuzioneInterrotta = false;
                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(StepIniziale, _passo);
                        Step(this, _stepEv);
                    }

                }

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
                        ApparatoLL = new LadeLightData(DbAttivo);
                        // ApparatoLL.caricaDati(ParametriApparato.s);
                    }


                    if (ParametriApparato.llParApp != null)
                    {
                        ModelloCorrente = DatiBase.ModelliLL.Find(x => x.IdModelloLL == ParametriApparato.llParApp.TipoApparato);
                        if (ModelloCorrente != null)
                        {
                            // sostituisco i valori teorici con quelli memorizzati sulla macchina
                            ModelloCorrente.TensioneMin = ParametriApparato.llParApp.VMin;
                            ModelloCorrente.TensioneMax = ParametriApparato.llParApp.VMax;
                            ModelloCorrente.CorrenteMin = ParametriApparato.llParApp.Amax / 10;
                            ModelloCorrente.CorrenteMax = ParametriApparato.llParApp.Amax;
                        }
                        if (DbAttivo != null)
                        {
                            // ParametriApparato._database = DbAttivo;
                            ParametriApparato.salvaDati();
                        }
                    }
                    else
                    {
                        ModelloCorrente = null;
                    }


                    //Intestazione.Matricola = BloccoIntestazione.SerialeApparato;
                    //Intestazione.PrimaInstallazione = BloccoIntestazione.DataSetupApparato.ToString();

                }

                if (RunAsinc)
                {
                    //Preparo l'intestazione della finestra di avanzamento                                                                                                          
                    if (Step != null)
                    {
                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                        _passo.Titolo = "Caricamento Parametri ";
                        _passo.Eventi = 1;
                        _passo.Step = -1;
                        _passo.EsecuzioneInterrotta = false;
                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(StepFinale, _passo);
                        Step(this, _stepEv);
                    }

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

        /// <summary>
        /// Carica i dati base dell'apparato da DB in base all'ID
        /// </summary>
        /// <param name="IdApparato"></param>
        /// <returns></returns>
        public bool CaricaApparatoA0(string IdApparato)
        {
            try
            {
                bool _esito = false;

                // Leggo dal primo banco memoria fissa
                ParametriApparato = new llParametriApparato(DbAttivo);
                _esito = ParametriApparato.caricaDati(IdApparato);

                if (_esito)
                {

                    if (ParametriApparato.llParApp != null)
                    {
                        ModelloCorrente = DatiBase.ModelliLL.Find(x => x.IdModelloLL == ParametriApparato.llParApp.TipoApparato);
                    }
                    else
                    {
                        ModelloCorrente = null;
                    }


                    //Intestazione.Matricola = BloccoIntestazione.SerialeApparato;
                    //Intestazione.PrimaInstallazione = BloccoIntestazione.DataSetupApparato.ToString();

                }

                _cbCollegato = false;
                apparatoPresente = false;
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

        public bool CaricaApparatoA0Fake(string IdApparato,byte TipoApp)
        {
            try
            {
                bool _esito = true;

                // Leggo dal primo banco memoria fissa
                ParametriApparato = new llParametriApparato();
                ParametriApparato.llParApp.IdApparato = IdApparato;

                ModelloCorrente = DatiBase.ModelliLL.Find(x => x.IdModelloLL == TipoApp);


                //_cbCollegato = false;
                //apparatoPresente = false;
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
                Programmazioni.ProgrammaAttivo = CaricaProgramma(0);

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

                uint StartAddr = (uint)(0x2000 + (256 * IdPosizione));

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
                Programmazioni.IdCorrente = ParametriApparato.IdApparato;
                Programmazioni._database = DbAttivo;
                Programmazioni.ProgrammiDefiniti = new List<llProgrammaCarica>();

                // Comincio a leggere dal primo messaggio da 240 bytes nell'area programmazioni e continuo a scorrere fino a che non ho
                // tipo record = 0xFF o ho raggiunto l'ultimo ( 15 con inizio 0 ) 

                for (byte contacicli = 0; contacicli < 16; contacicli++)
                {
                    llProgrammaCarica _tempProgramma = CaricaProgramma(contacicli);
                    if (_tempProgramma.TipoRecord != 0xFF)
                    {
                        Programmazioni.ProgrammiDefiniti.Add(_tempProgramma);
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



        public bool CaricaContatori(string IdApparato)
        {
            try
            {
                bool _esito = false;
                ContatoriLL = new llContatoriApparato(DbAttivo);
                _esito = ContatoriLL.caricaDati(IdApparato);  //.llContApp.IdApparato = ParametriApparato.IdApparato;

                return _esito;

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }

        }


        public bool CaricaAreaContatori(bool RunAsinc = false, int StepIniziale = 0, int StepFinale = 100)
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;
                MessaggioLadeLight.MessaggioAreaContatori MsgContatoriLL = new MessaggioLadeLight.MessaggioAreaContatori();
                SerialMessage.EsitoRisposta EsitoMsg;

                ContatoriLL = new llContatoriApparato(DbAttivo);
                ContatoriLL.llContApp.IdApparato = ParametriApparato.IdApparato;


                uint StartAddr = 0x3000;

                byte[] _datiTemp = new byte[240];
                _esito = LeggiBloccoMemoria(StartAddr, 240, out _datiTemp);

                if (_esito)
                {
                    EsitoMsg = MsgContatoriLL.analizzaMessaggio(_datiTemp, 1);
                    if (RunAsinc)
                    {
                        //Preparo l'intestazione della finestra di avanzamento                                                                                                          
                        if (Step != null)
                        {
                            elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                            _passo.Titolo = "Caricamento Contatori";
                            _passo.Eventi = 1;
                            _passo.Step = -1;
                            _passo.EsecuzioneInterrotta = false;
                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(StepFinale, _passo);
                            Step(this, _stepEv);
                        }

                    }

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
                        ContatoriLL.CntCicliOpportunity = MsgContatoriLL.CntCicliOpportunity;
                        ContatoriLL.CntProgrammazioni = MsgContatoriLL.CntProgrammazioni;
                        ContatoriLL.CntCicliBrevi = MsgContatoriLL.CntCicliBrevi;
                        ContatoriLL.PntNextBreve = MsgContatoriLL.PntNextBreve;
                        ContatoriLL.CntCariche = MsgContatoriLL.CntCariche;
                        ContatoriLL.PntNextCarica = MsgContatoriLL.PntNextCarica;
                        ContatoriLL.CntMemReset = MsgContatoriLL.CntMemReset;
                        ContatoriLL.DataUltimaCancellazione = FunzioniComuni.ArrayCopy(MsgContatoriLL.DataUltimaCancellazione);
                        ContatoriLL.valido = true;
                        ContatoriLL.salvaDati();
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


        public bool AzzeraContatori(bool AzzeraTotale = false)
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;
                MessaggioLadeLight.MessaggioAreaContatori MsgContatoriLL = new MessaggioLadeLight.MessaggioAreaContatori();
                SerialMessage.EsitoRisposta EsitoMsg;
                byte[] _datitemp;

                if (!ContatoriLL.valido)
                {
                    return false;
                }

                if ((ContatoriLL.CntCariche == 0) && !AzzeraTotale)
                {
                    // Il numero cariche è già a 0 non ho nulla da azzerare
                    return true;
                }

                for (int cnt = 0; cnt < 5; cnt++)
                {
                    if (AzzeraTotale)
                    {
                        MsgContatoriLL.DataPrimaCarica[cnt] = 0xFF;
                    }
                    else
                    {
                        MsgContatoriLL.DataPrimaCarica[cnt] = ContatoriLL.DataPrimaCarica[cnt];
                    }
                }
                MsgContatoriLL.CntCicliTotali = 0;

                if (!AzzeraTotale) MsgContatoriLL.CntCicliTotali = ContatoriLL.CntCicliTotali;

                MsgContatoriLL.CntCicliStaccoBatt = 0;
                MsgContatoriLL.CntCicliStop = 0;
                MsgContatoriLL.CntCicliLess3H = 0;
                MsgContatoriLL.CntCicli3Hto6H = 0;
                MsgContatoriLL.CntCicli6Hto9H = 0;
                MsgContatoriLL.CntCicliOver9H = 0;

                MsgContatoriLL.CntProgrammazioni = ContatoriLL.CntProgrammazioni;

                MsgContatoriLL.CntCicliBrevi = 0;
                MsgContatoriLL.PntNextBreve = 0;
                MsgContatoriLL.CntCariche = 0;
                MsgContatoriLL.PntNextCarica = 0;
                MsgContatoriLL.CntMemReset = (ushort)(ContatoriLL.CntMemReset + 1);
                DateTime adesso = DateTime.Now;
                MsgContatoriLL.DataUltimaCancellazione[2] = (byte)(adesso.Year - 2000);
                MsgContatoriLL.DataUltimaCancellazione[1] = (byte)(adesso.Month);
                MsgContatoriLL.DataUltimaCancellazione[0] = (byte)(adesso.Day);
                MsgContatoriLL.CntCicliOpportunity = 0;

                if (MsgContatoriLL.GeneraByteArray())
                {
                    _datitemp = MsgContatoriLL.dataBuffer;

                    _esito = CancellaBlocco4K(ADDR_START_CONTATORI);
                    _esito = ScriviBloccoMemoria(ADDR_START_CONTATORI, (ushort)_datitemp.Length, _datitemp);

                    _esito = CancellaBlocco4K(ADDR_START_BACKUP_CONTATORI);
                    _esito = ScriviBloccoMemoria(ADDR_START_BACKUP_CONTATORI, (ushort)_datitemp.Length, _datitemp);
                    for (int delta = 0; delta < 0x4000; delta += 0x1000)
                    {
                        CancellaBlocco4K((uint)(ADDR_START_RECORD_LUNGHI + delta));
                    }
                    CancellaBlocco4K(ADDR_START_RECORD_BREVI);

                    ResetScheda();


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

        public bool IncrementaConteggioProgrammazioni()
        {
            try
            {
                bool _esito = false;

                MessaggioLadeLight.MessaggioAreaContatori MsgContatoriLL = new MessaggioLadeLight.MessaggioAreaContatori();
                SerialMessage.EsitoRisposta EsitoMsg;
                byte[] _datitemp;

                if (!ContatoriLL.valido)
                {
                    return false;
                }

                ContatoriLL.CntProgrammazioni += 1;
                MsgContatoriLL = ContatoriLL.GeneraMessaggio();


                if (MsgContatoriLL.GeneraByteArray())
                {
                    _datitemp = MsgContatoriLL.dataBuffer;

                    _esito = false;

                    if (CancellaBlocco4K(ADDR_START_CONTATORI)) _esito = ScriviBloccoMemoria(ADDR_START_CONTATORI, (ushort)_datitemp.Length, _datitemp);
                    if (CancellaBlocco4K(ADDR_START_BACKUP_CONTATORI)) _esito = _esito && ScriviBloccoMemoria(ADDR_START_BACKUP_CONTATORI, (ushort)_datitemp.Length, _datitemp);
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





        public bool SalvaAreaContatori()
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;
                MessaggioLadeLight.MessaggioAreaContatori MsgContatoriLL = new MessaggioLadeLight.MessaggioAreaContatori();
                SerialMessage.EsitoRisposta EsitoMsg;
                byte[] _datitemp;

                if (!ContatoriLL.valido)
                {
                    return false;
                }
                // carico il messaggio a partire dal record contatori!!
                MsgContatoriLL = ContatoriLL.GeneraMessaggio();

                if (MsgContatoriLL.GeneraByteArray())
                {
                    _datitemp = MsgContatoriLL.dataBuffer;

                    _esito = false;

                    if (CancellaBlocco4K(ADDR_START_CONTATORI)) _esito = ScriviBloccoMemoria(ADDR_START_CONTATORI, (ushort)_datitemp.Length, _datitemp);
                    if (CancellaBlocco4K(ADDR_START_BACKUP_CONTATORI)) _esito = _esito && ScriviBloccoMemoria(ADDR_START_BACKUP_CONTATORI, (ushort)_datitemp.Length, _datitemp);

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

        public bool StopComunicazione(int TimeoutRisposta = 50)
        {
            try
            {

                bool _esito = false;

                if (_cbCollegato)
                {
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

                }
                else
                {
                    _esito = true;
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
                Log.Debug("----------------------------------------");
                Log.Debug("Attesa riconnessione");

                // innazitutto aspetto il tempo di attesa iniziale
                if (AttesaIniziale > 0)
                {
                    Thread.Sleep(AttesaIniziale);
                    Log.Debug("Attesa iniziale ");
                }

                _connessioneAttiva = StartComunicazione();

                while (!_connessioneAttiva)
                {
                    if (raggiuntoTimeout(_startFunzione, Timeout)) break;
                    _connessioneAttiva = StartComunicazione();
                    Log.Debug("Tentativo START");

                }
                _esito = _connessioneAttiva;
                Log.Debug("Tentativo Connessione: " + _esito.ToString());
                return _esito;
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






        public bool LeggiProgrammazioni(bool RunAsinc = false, int StepIniziale = 0, int StepFinale = 100)
        {
            try
            {
                bool _esito;
                llProgrammaCarica _tempPrg;
                Programmazioni._database = DbAttivo;
                Programmazioni.IdCorrente = ParametriApparato.IdApparato;
                ushort TempIdProgamma = 0;
                byte TempNumRecordProg = 0;
                // prima carico l'area indici programmazioni
                uint StartAddr = (uint)(ADDR_START_PROGRAMMAZIONI + ADDR_START_PAR_PROGRAM);

                if (RunAsinc)
                {
                    //Preparo l'intestazione della finestra di avanzamento                                                                                                          
                    if (Step != null)
                    {
                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                        _passo.Titolo = "Caricamento Configurazioni";
                        _passo.Eventi = 1;
                        _passo.Step = -1;
                        _passo.EsecuzioneInterrotta = false;
                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(StepIniziale, _passo);
                        Step(this, _stepEv);
                    }

                }


                byte[] _IndiciProg = new byte[10];
                byte[] _DatiProg = new byte[10];
                _esito = LeggiBloccoMemoria(StartAddr, 10, out _IndiciProg);

                if (_esito)
                {
                    if (_IndiciProg[0] == 0xFF)
                    {
                        Programmazioni.NumeroRecordProg = 0;
                    }
                    else
                    {
                        Programmazioni.NumeroRecordProg = _IndiciProg[0];
                        if (Programmazioni.NumeroRecordProg > 15) Programmazioni.NumeroRecordProg = 15;
                        TempNumRecordProg = Programmazioni.NumeroRecordProg;

                    }

                    ushort _LastId = FunzioniComuni.ArrayToUshort(_IndiciProg, 1, 2);
                    if (_LastId == 0xFFFF)
                    {
                        Programmazioni.UltimoIdProgamma = 0;
                    }
                    else
                    {
                        Programmazioni.UltimoIdProgamma = _LastId;
                        TempIdProgamma = _LastId;
                    }
                    if (Programmazioni.ProgrammiDefiniti == null)
                    {
                        Programmazioni.ProgrammiDefiniti = new List<llProgrammaCarica>();
                    }

                    Programmazioni.ProgrammiDefiniti.Clear();

                    // Leggo la programmazione corrente
                    _tempPrg = CaricaProgramma(0);
                    if (_tempPrg == null)
                    {
                        // La scheda non ha un programma definito
                        Programmazioni.UltimoIdProgamma = 0;
                        Programmazioni.NumeroRecordProg = 0;
                        Programmazioni.ProgrammiDefiniti.Clear();
                        Programmazioni.ProgrammaAttivo = null;
                        _esito = false;

                    }
                    else
                    {
                        if (_tempPrg.IdProfilo == 0XFF)
                        {
                            // La connfigurazione attiva non è valida. Considero tutta l'area inconsistente
                            Programmazioni.UltimoIdProgamma = 0;
                            Programmazioni.NumeroRecordProg = 0;
                            Programmazioni.ProgrammiDefiniti.Clear();
                            Programmazioni.ProgrammaAttivo = null;
                            return false;  //  TRUE ?????? non ho dati, ma non sono in errore

                        }
                        else
                        {
                            _tempPrg.ProgrammaAttivo = true;
                            _tempPrg.PosizioneCorrente = 0;
                            _tempPrg.Parametri = _parametri;
                            Programmazioni.ProgrammaAttivo = _tempPrg;
                            if (Programmazioni.NumeroRecordProg < 1) Programmazioni.NumeroRecordProg = 1;
                            if (Programmazioni.UltimoIdProgamma < _tempPrg.IdProfilo) Programmazioni.UltimoIdProgamma = _tempPrg.IdProgramma;
                            Programmazioni.ProgrammiDefiniti.Add(_tempPrg);

                        }

                        // ora leggo le altre programmazioni
                        for (int cicloP = 1; cicloP < TempNumRecordProg; cicloP++)
                        {
                            _tempPrg = CaricaProgramma((byte)cicloP);
                            _tempPrg.Parametri = _parametri;
                            if (_tempPrg.IdProfilo == 0XFF)
                            {
                                // Non valida, esco
                                return true;
                            }
                            //_tempPrg.IdProgramma = (ushort)cicloP;
                            _tempPrg.AnalizzaListaParametri();
                            _tempPrg.PosizioneCorrente = (byte)cicloP;
                            _tempPrg.ProgrammaAttivo = false;
                            Programmazioni.ProgrammiDefiniti.Add(_tempPrg);

                            if (RunAsinc)
                            {
                                if (!RichiestaInterruzione)
                                {
                                    elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                    int _progress = 0;
                                    double _valProgress = 0;
                                    _passo.TipoDati = elementiComuni.tipoMessaggio.AreaMemLungaLL;
                                    _passo.Titolo = "";
                                    _passo.Eventi = TempNumRecordProg;
                                    _passo.Step = cicloP;
                                    _passo.NumTentativi = 1;
                                    if (_passo.Eventi > 0)
                                    {
                                        _valProgress = cicloP * (StepFinale - StepIniziale) / TempNumRecordProg;
                                    }
                                    _progress = (int)(_valProgress + StepIniziale);

                                    ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                    Step(this, _stepEv);
                                }
                                else
                                {
                                    elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                    _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                    _passo.Titolo = "Caricamento ANNULLATO";
                                    _passo.Eventi = 1;
                                    _passo.Step = -1;
                                    _passo.NumTentativi = 1;
                                    _passo.EsecuzioneInterrotta = true;
                                    ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                    Step(this, _stepEv);
                                    Log.Debug("Lettura Configurazioni => lettura interrotta: " + cicloP.ToString() + "/" + TempNumRecordProg.ToString());

                                    System.Threading.Thread.Sleep(2000);

                                    //SkipOver = true;
                                    break;
                                }
                            }

                        }
                    }
                    _esito = Programmazioni.SalvaDati();

                }
                else
                {
                    Programmazioni.UltimoIdProgamma = 0;
                    Programmazioni.NumeroRecordProg = 0;
                    Programmazioni.ProgrammiDefiniti.Clear();
                    Programmazioni.ProgrammaAttivo = null;
                    _esito = false;
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


        public bool CaricaProgrammazioniDB(string IdApparato)
        {
            try
            {
                bool _esito;
                llProgrammaCarica _tempPrg;
                Programmazioni._database = DbAttivo;
                Programmazioni.IdCorrente = ParametriApparato.IdApparato;
                Programmazioni.ProgrammiDefiniti = new List<llProgrammaCarica>();
                Programmazioni.ProgrammaAttivo = null;


                IEnumerable<_llProgrammaCarica> _TempCicli = DbAttivo.Query<_llProgrammaCarica>("select * from _llProgrammaCarica where IdApparato = ? order by PosizioneCorrente asc", IdApparato);

                foreach (_llProgrammaCarica Elemento in _TempCicli)
                {
                    llProgrammaCarica _cLoc;
                    _cLoc = new llProgrammaCarica(Elemento);
                    _cLoc.Parametri = _parametri;
                    _cLoc.GeneraListaParametri();
                    Programmazioni.ProgrammiDefiniti.Add(_cLoc);
                    if (_cLoc.PosizioneCorrente == 0)
                    {
                        Programmazioni.ProgrammaAttivo = _cLoc;
                    }

                }

                return true;
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
        public bool LeggiBloccoMemoria(uint StartAddr, ushort NumByte, out byte[] Dati, bool MemoriaInterna = false)
        {


            try
            {
                bool _esito = false;
                bool _daticompleti = false;
                int _numciclolettura = 0;

                if (!apparatoPresente)
                {
                    Dati = null;
                    return false;
                }

                // Verifico che il canale sia attivo
                ControllaAttesa(UltimaScrittura);

                if (NumByte < 1) NumByte = 1;
                if (NumByte > 240)
                {
                    Dati = null;
                    return false;
                }

                Dati = new byte[NumByte];

                if (MemoriaInterna)
                {
                    _mS.Comando = SerialMessage.TipoComando.CMD_READ_MEMORY_DF;
                }
                else
                {
                    _mS.Comando = SerialMessage.TipoComando.CMD_READ_MEMORY;
                }
                _mS._pacchettoMem = new SerialMessage.PacchettoReadMem();

                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Lettura di " + NumByte.ToString() + " bytes dall'indirizzo " + StartAddr.ToString("X2"));

                _mS.ComponiMessaggioLeggiMem(StartAddr, NumByte,MemoriaInterna);
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;

                // 08/11/2019  controllo che i bytes ottenuti siano quelli richiesti: se sono meno ritento la lettura, fino a 3 volte


                // 08/11/2019  controllo che i bytes ottenuti siano quelli richiesti:

                while (!_daticompleti && _numciclolettura < 3)
                {
                    _numciclolettura++;
                    _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.Timeout1sec, 1, false);

                    if (_esito)
                    {
                        if (_mS._pacchettoMem.numBytes == NumByte)
                        {
                            for (int _ciclo = 0; ((_ciclo < NumByte) && (_ciclo < _mS._pacchettoMem.numBytes)); _ciclo++)
                            {

                                Dati[_ciclo] = _mS._pacchettoMem.memDataDecoded[_ciclo];
                            }
                            _daticompleti = true;
                        }
                        else
                        {
                            Log.Debug("---------------------------------------------------------------------------------------------");
                            Log.Debug("Lettura Incompleta: richiesti " + NumByte.ToString() + " bytes, ottenuti " + _mS._pacchettoMem.numBytes.ToString());
                            Log.Debug("                    Tentativo lettura n° " + _numciclolettura.ToString());
                            Log.Debug("---------------------------------------------------------------------------------------------");

                            // aggiungo un ritardo di 100 ms
                            System.Threading.Thread.Sleep(100);
                        }

                    }
                }
                NumeroTentativiLettura = _numciclolettura;
                return (_esito && _daticompleti);
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

                _esito = aspettaRisposta(elementiComuni.Timeout5sec, 0, true);

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
        public bool CancellaBlocco4K(uint StartAddr, bool BeepIfZero = false)
        {
            try
            {
                bool _esito;

                // DEBUG: verifica cancellazioni blocco 0

                if (StartAddr < 0x1000)
                {
                    if (!BeepIfZero)
                    {
                        DialogResult risposta = MessageBox.Show("Richiesta cancellazione AREA 0 (Inizializzazione)\nConfermi l'operazione ?\nStart = " + StartAddr.ToString("6x"), "Cancellazione Memoria",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (risposta != System.Windows.Forms.DialogResult.Yes)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        // Area 0 --> faccio un beep ... da cambiare con un toast message
                        System.Media.SystemSounds.Exclamation.Play();
                    }
                }
                //Fine DEBUG

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
                _esito = aspettaRisposta(elementiComuni.Timeout2sec, 0, true);
                Log.Debug("Aspetto 150 ms per l'effettiva riabilitazione della flash");
                System.Threading.Thread.Sleep(150);
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


        /// <summary>
        /// Forzas the orologio.
        /// </summary>
        /// <param name="Giorno">The giorno.</param>
        /// <param name="Mese">The mese.</param>
        /// <param name="Anno">The anno.</param>
        /// <param name="Ore">The ore.</param>
        /// <param name="Minuti">The minuti.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ForzaOrologio(int Giorno, int Mese, int Anno, int Ore, int Minuti)
        {
            try
            {
                bool _esito;
                ControllaAttesa(UltimaScrittura);

                DateTime _now = DateTime.Now;

                DateTime dateValue = new DateTime(Anno, Mese, Giorno, Ore, Minuti, 0);
                _mS.Comando = SerialMessage.TipoComando.CMD_UPDATE_RTC;
                _mS.DatiRTC = new SerialMessage.comandoRTC();
                _mS.DatiRTC.anno = (ushort)Anno;
                _mS.DatiRTC.mese = (byte)Mese;
                _mS.DatiRTC.giorno = (byte)Giorno;
                _mS.DatiRTC.giornoSett = (byte)dateValue.DayOfWeek;
                _mS.DatiRTC.ore = (byte)Ore;
                _mS.DatiRTC.minuti = (byte)Minuti;
                _mS.DatiRTC.secondi = (byte)0;

                _mS.ComponiMessaggioOra();
                _rxRisposta = false;
                Log.Debug("Scrivi RTC");
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);

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
                ParametriApparato = new llParametriApparato(DbAttivo);
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
                        ParametriApparato.llParApp.ModelloMemoria =  ImmagineMemoria.ModelloMemoria;

                        Memoria = new llMappaMemoria(ImmagineMemoria.ModelloMemoria, TipoCB);

                        ParametriApparato.llParApp.IdApparato = ImmagineMemoria.IDApparato;
                         
                        ParametriApparato.llParApp.VMin = ImmagineMemoria.VMin;
                        ParametriApparato.llParApp.VMax = ImmagineMemoria.VMax;
                        ParametriApparato.llParApp.Amax = ImmagineMemoria.Amax;
                        ParametriApparato.llParApp.PresenzaRabboccatore = ImmagineMemoria.PresenzaRabboccatore;
                        ParametriApparato.llParApp.NumeroModuli = ImmagineMemoria.NumeroModuli;
                        ParametriApparato.llParApp.ModVNom = ImmagineMemoria.ModVNom;
                        ParametriApparato.llParApp.ModANom = ImmagineMemoria.ModANom;
                        ParametriApparato.llParApp.ModOpzioni = ImmagineMemoria.ModOpzioni;
                        ParametriApparato.llParApp.ModVMin = ImmagineMemoria.ModVMin;
                        ParametriApparato.llParApp.ModVMax = ImmagineMemoria.ModVMax;


                        DateTime Lettura = DateTime.Now;
                        ParametriApparato.llParApp.UltimaLetturaDati = Lettura;
                        ParametriApparato.llParApp.DtUltimaLetturaDati = Lettura.ToString("yyyy/MM/dd") + " " + Lettura.ToString("hh:mm"); 

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

        public bool ScriviParametriApparato(bool SilentDelete = false)
        {
            try
            {
                bool _esito = false;
                MessaggioLadeLight.PrimoBloccoMemoria ImmagineMemoria = new MessaggioLadeLight.PrimoBloccoMemoria();
                //SerialMessage.EsitoRisposta EsitoMsg;



                // Dati ripresi tal quale dall'originale
                ImmagineMemoria.SerieApparato = (CaricaBatteria.TipoCaricaBatteria)ParametriApparato.llParApp.TipoApparato;
                ImmagineMemoria.ProduttoreApparato = ParametriApparato.llParApp.ProduttoreApparato;
                ImmagineMemoria.NomeApparato = ParametriApparato.llParApp.NomeApparato;
                ImmagineMemoria.MaxRecordBrevi = ParametriApparato.llParApp.MaxRecordBrevi;
                ImmagineMemoria.MaxRecordCarica = ParametriApparato.llParApp.MaxRecordCarica;
                ImmagineMemoria.SizeExternMemory = ParametriApparato.llParApp.SizeExternMemory;
                ImmagineMemoria.MaxProgrammazioni = ParametriApparato.llParApp.MaxProgrammazioni;
                ImmagineMemoria.ModelloMemoria = ParametriApparato.llParApp.ModelloMemoria;


                ImmagineMemoria.SerialeApparato = ParametriApparato.llParApp.SerialeApparato;
                ImmagineMemoria.TipoApparato = ParametriApparato.llParApp.TipoApparato;
                ImmagineMemoria.SerieApparato = (CaricaBatteria.TipoCaricaBatteria)ParametriApparato.llParApp.FamigliaApparato;
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
                ImmagineMemoria.NumeroModuli = ParametriApparato.llParApp.NumeroModuli;
                ImmagineMemoria.ModVNom = ParametriApparato.llParApp.ModVNom;
                ImmagineMemoria.ModANom = ParametriApparato.llParApp.ModANom;
                ImmagineMemoria.ModOpzioni = ParametriApparato.llParApp.ModOpzioni;
                ImmagineMemoria.ModVMin = ParametriApparato.llParApp.ModVMin;
                ImmagineMemoria.ModVMax = ParametriApparato.llParApp.ModVMax;

                if (ImmagineMemoria.GeneraByteArray())
                {
                    // da far diventare unica
                    _esito = CancellaBlocco4K(0,SilentDelete);

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

        public bool PreparaSalvataggioProgrammazioni()
        {
            try
            {

                // Sposto avanti tutti i dati presenti
                foreach (llProgrammaCarica tempPrg in Programmazioni.ProgrammiDefiniti)
                {
                    tempPrg.PosizioneCorrente += 1;
                }

                // poi accodo in posizione 0 la progr corrente
                Programmazioni.ProgrammaAttivo.PosizioneCorrente = 0;
                Programmazioni.UltimoIdProgamma += 1;
                Programmazioni.ProgrammaAttivo.IdProgramma = (ushort)(Programmazioni.UltimoIdProgamma);

                Programmazioni.ProgrammiDefiniti.Add(Programmazioni.ProgrammaAttivo);

                return true;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return false;
            }
        }

        public bool SalvaProgrammazioniApparato()
        {
            try
            {
                bool _esito = false;
                bool _cicloFallito = false;
                // Salvo le programmazioni nella lista in ordine di posizione -> deve esistere una programm. in posizione 0

                // 1 - Devo avere almeno 1 programmazione
                if (Programmazioni.ProgrammiDefiniti.Count < 1)
                {
                    return false;
                }

                // 2 - deve esserci la programmazione 0
                llProgrammaCarica AttualeCorrente = (llProgrammaCarica)Programmazioni.ProgrammiDefiniti.Find(x => x.PosizioneCorrente == 0);

                if (AttualeCorrente == null)
                {
                    return false;
                }

                // 3 Vuoto l'area e comincio a scrivere

                _esito = CancellaBlocco4K(ADDR_START_PROGRAMMAZIONI);

                if (!_esito)
                {
                    // Cancellazione fallita ...
                    return false;

                }

                uint AdrDati = ADDR_START_PROGRAMMAZIONI;
                MessaggioLadeLight.MessaggioProgrammazione NuovoPrg;
                foreach (llProgrammaCarica tempPrg in Programmazioni.ProgrammiDefiniti.OrderBy(prg => prg.PosizioneCorrente))
                {
                    NuovoPrg = new MessaggioLadeLight.MessaggioProgrammazione(tempPrg);
                    if (NuovoPrg.GeneraByteArray())
                    {
                        Log.Info("Programma  " + NuovoPrg.IdProgrammazione.ToString() + ":");
                        Log.Info(FunzioniComuni.HexdumpArray(NuovoPrg.dataBuffer));

                        byte[] _datiTemp = new byte[226];
                        _datiTemp = NuovoPrg.dataBuffer;
                        _esito = ScriviBloccoMemoria(AdrDati, 226, _datiTemp);
                        if (!_esito)
                        {
                            _cicloFallito = true;
                        }
                        AdrDati += 0x100;
                    }
                }
                if (!_cicloFallito)
                {
                    // 4 per ultimo scrivo i contatori
                    Log.Debug("Scrivo i contatori programmazioni");

                    byte[] datiCont = new byte[10];
                    datiCont[0] = (byte)Programmazioni.ProgrammiDefiniti.Count;
                    FunzioniComuni.SplitUshort(Programmazioni.UltimoIdProgamma, ref datiCont[2], ref datiCont[1]);
                    _esito = ScriviBloccoMemoria(0x20F6, 10, datiCont);
                }
                else
                {
                    return false;
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

        public bool CaricaCompleto(MoriData._db dbCorrente, string IdApparato)
        {
            try
            {
                bool _esito;
                if (IdApparato == "")
                    return false;

//                _cb = new CaricaBatteria(ref _parametri, _logiche.dbDati.connessione);

                _esito = CaricaApparatoA0(IdApparato);

                if (_esito)
                {

                    ApparatoLL = new LadeLightData(dbCorrente, ParametriApparato);

                    CaricaContatori(IdApparato);
                    DatiCliente = new llDatiCliente(dbCorrente);
                    DatiCliente.caricaDati(IdApparato, 1);
                    CaricaProgrammazioniDB(IdApparato);

                    // Carico la lista cariche
                    LeggiMemoriaCicliDB(IdApparato);

                    return true;
                }

                return false; ;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return false;
            }
        }

        
        public bool PreparaEsportazione()
        {
            try
            {
                bool _esito;
                elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
                ushort _crc;

                _esito = false;

                ModelloDati.ID = ParametriApparato.IdApparato;



                ModelloDati.Testata = ApparatoLL._ll;
                ModelloDati.Cliente = DatiCliente._lldc;
                ModelloDati.Contatori = ContatoriLL.llContApp;
                ModelloDati.Parametri = ParametriApparato.llParApp;

                ModelloDati.Programmazioni.Clear();

                foreach (llProgrammaCarica _prog in Programmazioni.ProgrammiDefiniti)
                {
                    ModelloDati.Programmazioni.Add(_prog._llprc);
                }


                ModelloDati.CicliCarica.Clear();
                foreach (llMemoriaCicli _memL in MemoriaCicli)
                {
                    ModelloDati.CicliCarica.Add(_memL._llmc);
                }


                ModelloDati.CRC = 0;
                string _tempSer = JsonConvert.SerializeObject(ModelloDati);
                byte[] _tepBSer = FunzioniMR.GetBytes(_tempSer);

                _crc = codCrc.ComputeChecksum(_tepBSer);
                ModelloDati.CRC = _crc;
                _esito = true;
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("PreparaEsportazione: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }

        public bool importaModello(MoriData._db dbCorrente, bool SalvaDati)
        {
            try
            {
                bool _esito;
                _esito = false;

                ApparatoLL = new LadeLightData(dbCorrente);
                ApparatoLL._ll = ModelloDati.Testata;
                if(SalvaDati) ApparatoLL.salvaDati();

                ParametriApparato = new llParametriApparato(dbCorrente);
                ParametriApparato.llParApp = ModelloDati.Parametri;
                if (SalvaDati) ParametriApparato.salvaDati();


                DatiCliente = new llDatiCliente(dbCorrente);
                DatiCliente._lldc = ModelloDati.Cliente;
                if (SalvaDati) DatiCliente.salvaDati();

                ContatoriLL = new llContatoriApparato(dbCorrente);
                ContatoriLL.llContApp = ModelloDati.Contatori;
                if (SalvaDati) ContatoriLL.salvaDati();


                Programmazioni = new cbProgrammazioni(dbCorrente);
                if(Programmazioni.ProgrammiDefiniti == null)
                {
                    Programmazioni.ProgrammiDefiniti = new List<llProgrammaCarica>();
                }
                Programmazioni.ProgrammiDefiniti.Clear();
                foreach (_llProgrammaCarica _prog in ModelloDati.Programmazioni )
                {
                    Programmazioni.ProgrammiDefiniti.Add( new llProgrammaCarica(dbCorrente, _prog));
                }
                if (SalvaDati) Programmazioni.SalvaDati();

                if(MemoriaCicli == null)
                {
                    MemoriaCicli = new List<llMemoriaCicli>();
                }
                MemoriaCicli.Clear();
                foreach (_llMemoriaCicli _memL in ModelloDati.CicliCarica)
                {
                    llMemoriaCicli _TempCiclo = new llMemoriaCicli(dbCorrente, _memL);
                    if (SalvaDati) _TempCiclo.salvaDati();
                    MemoriaCicli.Add(_TempCiclo);
                }

 
                _esito = true;
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("importaModello: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }


        public bool ImpostaBaudRate(BaudRate NewBR)
        {
            try
            {
                bool _esito;
                byte[] DatiComando = new byte[4];
                // DatiComando[0] = (byte)SerialMessage.TipoComando.CMD_UART_SWITCH_BDRATE;
                NewBR.SetCmdData();

                ControllaAttesa(UltimaScrittura);
                for(int _ciclo=0; _ciclo<4; _ciclo++)
                {
                    DatiComando[_ciclo ] = NewBR.CmdData[_ciclo];
                }
                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_UART_SWITCH_BDRATE;
                _mS.ComponiMessaggioNew(DatiComando);

                _rxRisposta = false;
                Log.Debug("Imposta Baudrate: " + NewBR.ToString());
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                if(_esito)
                {
                    // Cambiata velocità della uart rispro la connessione USB con la nuova velocità
                    chiudiPorta();
                    BaudRateUSB = NewBR;
                    _parametri.ActiveBaudRate = NewBR;
                    apriPorta();
                    _esito = VerificaPresenza();
                }
                return _esito; //_esito;
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
                                _inviaRisposta = false;
                                break;
                            case (byte)SerialMessage.TipoComando.CMD_READ_MEMORY_DF:
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

        /******************************************************************************************************/







    }

}
