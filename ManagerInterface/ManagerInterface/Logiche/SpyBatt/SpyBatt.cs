using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

using SQLite.Net;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;

namespace ChargerLogic
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UnitaSpyBatt
    {
        public enum StatoScheda : byte { NonCollegata = 0x00, SoloBootloader = 0x01, BLandFW = 0x02, SoloFW = 0x03 };

        public static SerialPort serialeApparato;
        private static MessaggioSpyBatt _mS; // = new MessaggioSpyBatt();
        private parametriSistema _parametri;

        private static Queue<byte> codaDatiSER = new Queue<byte>();  // Buffer per la ricezione dati seriali
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public string FileHexDump = "c:\\Log\\HexDump.txt";

        // Area Dati:
        public spybattData sbData = new spybattData();           // Testata
        public sbDatiCliente sbCliente = new sbDatiCliente();    // Dati Cliante 
        public sbMemLunga sbUltimoCiclo = new sbMemLunga();
        public sbVariabili sbVariabili = new sbVariabili();
        public sbCalibrazioni Calibrazioni = new sbCalibrazioni();
        public sbStatoFirmware StatoFirmware = new sbStatoFirmware();
        public sbParametriGenerali ParametriGenerali = new sbParametriGenerali();
        public sbProgrammaRicarica ProgrammaCorrente;
    

        //Strutture Memoria

        private bool _firmwarePresente = false;  

        private int _timeOut = 5 ;
        private DateTime _startRead;
        public bool dbCollegato;
        public bool apparatoPresente = false;
        private Queue<byte> mappaMemoria = new Queue<byte>();  // Buffer per la ricezione dati seriali
        public const int MemorySize = 2097152;  // 2 MB 
                                                //4194304;  // 4 MB

        // Pachetti inviati durante il dump memoria
        // Fino alla 1.08  8356
        // Fino alla 1.09: 8356
        // dalla 1.09.01 : 8666
        public int MemorySlice = 8666;       //8356;    // Pachetti inviati durante il dump memoria

        
        public bool RichiestaInterruzione = false;
        
        /* ----------------------------------------------------------
         *  Dichiarazione eventi per la gestione avanzamento
         * ---------------------------------------------------------
         */
        public event StepHandler Step;
        public delegate void StepHandler(UnitaSpyBatt usb, ProgressChangedEventArgs e); //sbWaitEventStep e);
       // ----------------------------------------------------------
 
        internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
        delegate void SetTextCallback(string text);
        string InputData = String.Empty;
        byte[] _dataBuffer = new byte[0];
        int lastByte = 0;
        bool readingMessage = false;
        public int TipoRisposta;
        static bool _rxRisposta ;
        bool skipHead = false;

        public int LivelloUser = 2;
        public DateTime UltimaScrittura;   // Registro l'istante dell'ultima scrittura

        public SerialMessage.EsitoRisposta UltimaRisposta;
        public MessaggioSpyBatt.comandoInizialeSB IntestazioneSb = new MessaggioSpyBatt.comandoInizialeSB();
        public MessaggioSpyBatt.cicliPresenti CicliPresenti = new MessaggioSpyBatt.cicliPresenti();
        public MessaggioSpyBatt.comandoRTC OrologioSistema = new MessaggioSpyBatt.comandoRTC();
        public MessaggioSpyBatt.cicloAttuale CicloInMacchina = new MessaggioSpyBatt.cicloAttuale();
        

        private List<MessaggioSpyBatt.MemoriaPeriodoLungo> _CicliMemoriaLunga = new List<MessaggioSpyBatt.MemoriaPeriodoLungo>();
        private List<MessaggioSpyBatt.MemoriaPeriodoBreve> _CicliMemoriaBreve = new List<MessaggioSpyBatt.MemoriaPeriodoBreve>();
        private List<MessaggioSpyBatt.ProgrammaRicarica> _Programmazioni = new List<MessaggioSpyBatt.ProgrammaRicarica>();

        public List<MessaggioSpyBatt.CicloDiCarica> CicliInMemoria = new List<MessaggioSpyBatt.CicloDiCarica>();

        public List<sbMemLunga> CicliMemoriaLunga = new List<sbMemLunga>();
        public List<sbProgrammaRicarica> Programmazioni = new List<sbProgrammaRicarica>();

        public List<_sbMemLunga> DataCicliMemoriaLunga = new List<_sbMemLunga>();
        public List<_sbProgrammaRicarica> DataProgrammazioni = new List<_sbProgrammaRicarica>();

        public List<ParametroCalibrazione> ParametriCalibrazione = new List<ParametroCalibrazione>();

        public sbDataModel ModelloDati = new sbDataModel();

        public sbSetSoglie SoglieAnalisi = new sbSetSoglie();

        public byte[] numeroSeriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
        string _idCorrente;
        private string _lastError;
        private bool _cbCollegato;
        private bool _inviaAckPacchettoDump = false;
        private SerialMessage.TipoRisposta _ultimaRisposta = SerialMessage.TipoRisposta.NonValido;   // flag per l'indicazioene del tipo dell'ultimo messaggio ricevuto dalla scheda
        public elementiComuni.VersoCorrente VersoScarica = elementiComuni.VersoCorrente.Diretto;

        // Eventi pubblici della classe spy Batt

        private static EventWaitHandle SB_USBeventWait;

        public event LongMemListHandler LMListChange ;
        public delegate void LongMemListHandler(UnitaSpyBatt sb, sbListaLunghiEvt lme);

 
        public UnitaSpyBatt(ref parametriSistema parametri, int LivelloAutorizzazione ) 
        {

            _parametri = parametri;
            LivelloUser = LivelloAutorizzazione;
            _mS = new MessaggioSpyBatt();
            _mS.Dispositivo = MessaggioSpyBatt.TipoDispositivo.Charger;
             byte[] Seriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
             byte[] numeroSeriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
             _mS.SerialNumber = Seriale;
            _cbCollegato = false;
            serialeApparato = _parametri.serialeSpyBatt;
            InizializzaParametriCalibrazione();
             // Attivo gli eventi sia USB che COM

            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            // USB
            new Thread(UsbWaiter).Start();
            SB_USBeventWait = new EventWaitHandle(false, EventResetMode.AutoReset);
            ftStatus = _parametri.usbSpyBatt.SetEventNotification(FTDI.FT_EVENTS.FT_EVENT_RXCHAR, SB_USBeventWait);
            // COM
            cEventHelper.RemoveEventHandler(serialeApparato, "DataReceived");
            serialeApparato.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceivedSb);
            dbCollegato = false;
        }

        public UnitaSpyBatt(ref parametriSistema parametri,MoriData._db dbCorrente, int LivelloAutorizzazione)
        {
            LivelloUser = LivelloAutorizzazione;
            _parametri = parametri;
            _mS = new MessaggioSpyBatt();
            _mS.Dispositivo = MessaggioSpyBatt.TipoDispositivo.Charger;
            byte[] Seriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] numeroSeriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
            _mS.SerialNumber = Seriale;
            _cbCollegato = false;
            serialeApparato = _parametri.serialeSpyBatt;

            // Attivo gli eventi sia USB che COM
            /*
             *   24/10  Polling solo su COM
             * 
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            // USB
            new Thread(UsbWaiter).Start();
            USBeventWait = new EventWaitHandle(false, EventResetMode.AutoReset);
            ftStatus = _parametri.usbSpyBatt.SetEventNotification(FTDI.FT_EVENTS.FT_EVENT_RXCHAR, USBeventWait);
            */ 

            // COM
            cEventHelper.RemoveEventHandler(serialeApparato, "DataReceived");
            serialeApparato.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceivedSb);
            dbCollegato = false;


            //cEventHelper.RemoveEventHandler(serialeApparato, "DataReceived");
            //serialeApparato.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceivedSb);
            StatoFirmware = new sbStatoFirmware();
            sbData = new spybattData(dbCorrente);
            sbCliente = new sbDatiCliente(dbCorrente);
            SoglieAnalisi.CaricaSoglie(dbCorrente, "", "");
            InizializzaParametriCalibrazione();
            dbCollegato = true;

        }

        public string Id
        {
            get
            {
                return _idCorrente;
            }
        }

        public bool apriPorta()
        {
            bool _esito;
            _esito = _parametri.apriSpyBat();
            return _esito;

        }

        /// <summary>
        /// Pro
        /// </summary>
        /// <returns></returns>
        public bool VerificaPresenza()
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;
                
                _mS.Comando = MessaggioSpyBatt.TipoComando.SB_Sstart;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("SB START");
                Log.Debug(_mS.hexdumpMessaggio());
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase,0,true);
                if ((_esito) && (_ultimaRisposta == SerialMessage.TipoRisposta.Ack))
                 //   (_mS._comando == (byte)(MessaggioSpyBatt.TipoComando.ACK_SB))  
                {
                    _idCorrente = _mS.idCorrente;
                    numeroSeriale = _mS.arrayIdCorrente;
                    UltimaScrittura = DateTime.Now;
                    apparatoPresente = true;
                    _risposta = true;
                }
                return _risposta;
            }

            catch (Exception Ex)
            {
                Log.Error("VerificaPresenza: " + Ex.Message);
                _lastError = Ex.Message;
                return _risposta;
            } 
        }

        public bool ControllaAttesa(DateTime UltimoIstante, int SecondiTimeOut = 300)
        {
            bool _risposta = false;
            try
            {
                DateTime _ora = DateTime.Now;
                TimeSpan _durata = _ora.Subtract(UltimoIstante);

                if (_durata.TotalSeconds > SecondiTimeOut)
                {
                    Log.Debug("Tempo Stanby comunicazione superato");
                    _risposta = VerificaPresenza();
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

        public bool recordPresente(string IdApparato, MoriData._db dbCorrente)
        {
            try
            {
                spybattData _tempSb = new spybattData(dbCorrente);

                bool _recordPresente;
                _recordPresente = _tempSb.caricaDati(IdApparato);

                return _recordPresente;
            }

            catch (Exception Ex)
            {
                Log.Error("recordPresente: " + Ex.Message);
                _lastError = Ex.Message;
                return false;
            }

        }

        public bool CaricaCompleto(string IdApparato, MoriData._db dbCorrente, bool ApparatoConnesso = false )
        {
            try
            {
                bool _esito;
                bool _recordPresente;

                Log.Debug("CaricaCompleto " );
                if (ApparatoConnesso)
                    ControllaAttesa(UltimaScrittura);

                _esito = false;
                _recordPresente = sbData.caricaDati(IdApparato);

                if (_recordPresente)
                {
                    _idCorrente = IdApparato;
                    _recordPresente = sbCliente.caricaDati(IdApparato, 1);
                    _recordPresente = CaricaProgrammazioni(IdApparato, dbCorrente);
                    _recordPresente = CaricaCicliMemLunga(IdApparato, dbCorrente);

                    _esito = true;
                }
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCompleto: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }

        }

        public bool PreparaEsportazione(bool Testata,bool Cliente,bool Programmi, bool MemLunga,bool MemBreve )
        {
            try
            {
                bool _esito;
                elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
                ushort _crc;

                _esito = false;

                ModelloDati.ID = _idCorrente;
                ModelloDati.testata = Testata;
                ModelloDati.cliente = Cliente;
                ModelloDati.programmazioni = Programmi;
                ModelloDati.cicliLunghi = MemLunga;
                ModelloDati.cicliBrevi = MemBreve;
                // 20/11  - ParametriGenerali


                if (Testata) ModelloDati.Testata = sbData._sb; else ModelloDati.Testata = null;
                if (Cliente) ModelloDati.Cliente = sbCliente._sbdc; else ModelloDati.Cliente = null;

                ModelloDati.Programmazioni.Clear();
                if (Programmi)
                {
                    foreach (sbProgrammaRicarica _prog in Programmazioni)
                    {
                        ModelloDati.Programmazioni.Add(_prog._sbpr);
                    }

                }

                ModelloDati.CicliLunghi.Clear();
                if (MemLunga)
                {
                    foreach (sbMemLunga _memL in CicliMemoriaLunga)
                    {
                        sbDataCicloLungo _cicloL = new sbDataCicloLungo();
                        _cicloL.TestataCiclo = _memL._sblm;
                        _cicloL.CicliBrevi.Clear();
                        if (MemBreve)
                        {
                            foreach (sbMemBreve _memB in _memL.CicliMemoriaBreve)
                            {
                                _sbMemBreve _cicloB = new _sbMemBreve();
                                _cicloB = _memB._sbsm;
                                _cicloL.CicliBrevi.Add(_cicloB);
                            }
                        }
                        ModelloDati.CicliLunghi.Add(_cicloL);
                    }
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

        public bool importaModello(MoriData._db dbCorrente, bool Testata, bool Cliente, bool Programmi, bool MemLunga, bool MemBreve)
        {
            try
            {
                bool _esito;
                _esito = false;


                _idCorrente = ModelloDati.ID;

                if (Testata) sbData._sb = ModelloDati.Testata; else sbData._sb = null;
                if (Cliente) sbCliente._sbdc = ModelloDati.Cliente; else sbCliente._sbdc = null;

                Log.Warn("Fine Carica Testate ");

                Programmazioni.Clear();
                if (Programmi)
                {
                    foreach (_sbProgrammaRicarica _prog in ModelloDati.Programmazioni)
                    {
                        sbProgrammaRicarica prog = new sbProgrammaRicarica(dbCorrente);
                        prog._sbpr = _prog;
                        Programmazioni.Add(prog);
                    }

                }

                Log.Warn("Fine CaricaProgrammazioni: " + ModelloDati.Programmazioni.Count.ToString());

                CicliMemoriaLunga.Clear();
                if (MemLunga)
                {
                    foreach (sbDataCicloLungo _memL in ModelloDati.CicliLunghi)
                    {
                        sbMemLunga _cicloL = new sbMemLunga(dbCorrente);
                        _cicloL._sblm = _memL.TestataCiclo;
                        _cicloL.CicliMemoriaBreve.Clear();
                        //MemBreve = false;
                        if (MemBreve)
                        {
                            foreach (_sbMemBreve _memB in _memL.CicliBrevi)
                            {
                                sbMemBreve _cicloB = new sbMemBreve(dbCorrente);
                                _cicloB._sbsm = _memB;
                                _cicloL.CicliMemoriaBreve.Add(_cicloB);
                            }
                            Log.Warn("Fine CaricaBrevi: " + _memL.CicliBrevi.Count.ToString());

                        }
                        CicliMemoriaLunga.Add(_cicloL);
                    }
                    Log.Warn("Fine CaricaLunghi: " + ModelloDati.CicliLunghi.Count.ToString());
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


        /// <summary>
        /// Legge i parametri iniziali (Contatori) della scheda.
        /// </summary>
        /// <param name="IdApparato">ID dell'apparato collegato</param>
        /// <param name="ApparatoConnesso">Se true tento la lettura diretta</param>
        /// <returns></returns>
        public bool CaricaTestata(string IdApparato,bool ApparatoConnesso)

        {
            try
            {
                bool _esito;
                bool _recordPresente;
                _recordPresente = sbData.caricaDati(IdApparato);
                if (_recordPresente != true)
                {
                    sbData.Id = IdApparato;
                }
                else
                {
                    _idCorrente = IdApparato;
                }

                if (ApparatoConnesso)
                {
                    _mS.Comando = MessaggioSpyBatt.TipoComando.SB_DatiIniziali;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    skipHead = true;
                    Log.Debug("SB CaricaTestata");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutLungo);
                    if (_esito)
                    {
                        IntestazioneSb = _mS.Intestazione;
                        sbData.SwVersion = _mS.Intestazione.revSoftware;
                        sbData.HwVersion = _mS.Intestazione.revHardware;
                        sbData.Manufacturer = _mS.Intestazione.Manufacturer;
                        sbData.ProductId = _mS.Intestazione.productId;
                        sbData.ProgramCount = _mS.Intestazione.numeroProgramma;
                        sbData.BattConnected = _mS.Intestazione.statoBatteria;
                        sbData.LongMem = (int)_mS.Intestazione.longRecordCounter;
                        sbData.salvaDati();
                    }
                }
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaTestata: " + Ex.Message);
                Log.Error( Ex.TargetSite.ToString());
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool CaricaTestata()
        {
            bool _esito = false;
            try
            {
                if (_idCorrente != "")
                { 
                    if (apparatoPresente)
                    {
                        _esito = CaricaTestata(_idCorrente, apparatoPresente);
                    }
                }

                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaTestata: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return _esito;
            }
        }






        /// <summary>
        /// Legge direttamente dallo SPY-BATT collegato i valori attuali di corrente, tensione e temperatura
        /// </summary>
        /// <param name="IdApparato">ID dell'apparato collegato</param>
        /// <param name="ApparatoConnesso">Se true tento la lettura diretta</param>
        /// <returns></returns>
        public bool CaricaVariabili(string IdApparato, bool ApparatoConnesso)
        {
            try
            {
                bool _esito;
//
//                _idCorrente = IdApparato;
//                
                _esito = false;

                if (ApparatoConnesso)
                {
                    // Eseguo solo se la connessione all'apparato è attiva
                    _mS.variabiliScheda = new MessaggioSpyBatt.VariabiliSpybatt();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.SB_R_Variabili;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    skipHead = true;
                    Log.Debug("SB Carica Variabili");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                    if (_esito)
                    {
                        sbVariabili = new MoriData.sbVariabili();
                        sbVariabili.IdApparato = _idCorrente;
                        sbVariabili.TensioneTampone = (ushort)(_mS.variabiliScheda.TensioneBattT );
                        sbVariabili.TensioneIstantanea = _mS.variabiliScheda.TensioneIstantanea;
                        sbVariabili.Tensione1 = _mS.variabiliScheda.Tensione1;
                        sbVariabili.Tensione2 = _mS.variabiliScheda.Tensione2;
                        sbVariabili.Tensione3 = _mS.variabiliScheda.Tensione3;
                        sbVariabili.CorrenteBatteria = _mS.variabiliScheda.CorrenteBatteria;
                        sbVariabili.AhCaricati = _mS.variabiliScheda.AhCaricati;
                        sbVariabili.AhScaricati = _mS.variabiliScheda.AhScaricati;
                        sbVariabili.TempNTC = _mS.variabiliScheda.TempNTC;
                        sbVariabili.PresenzaElettrolita = _mS.variabiliScheda.PresenzaElettrolita;
                        sbVariabili.SoC = _mS.variabiliScheda.SoC;
                        sbVariabili.RF = _mS.variabiliScheda.RF;
                        sbVariabili.WhScaricati = _mS.variabiliScheda.WhScaricati;
                        sbVariabili.WhCaricati = _mS.variabiliScheda.WhCaricati;
                        sbVariabili.MemProgrammed = _mS.variabiliScheda.MemProgrammed;
                        sbVariabili.IstanteLettura = _mS.variabiliScheda.IstanteLettura;
                        sbVariabili.ConnectionStatus = _mS.variabiliScheda.ConnStatus;

                    }
                }
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaTestata: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }

        /// <summary>
        /// Legge direttamente dallo SPY-BATT collegato i valori dei parametri generali
        /// </summary>
        /// <param name="IdApparato">ID dell'apparato collegato</param>
        /// <param name="ApparatoConnesso">Se true tento la lettura diretta</param>
        /// <returns></returns>
        public bool CaricaParametri(string IdApparato, bool ApparatoConnesso)
        {
            try
            {
                bool _esito;
                //
                //                _idCorrente = IdApparato;
                //                
                _esito = false;

                if (ApparatoConnesso)
                {
                    // Eseguo solo se la connessione all'apparato è attiva
                    _mS.ParametriGenerali = new MessaggioSpyBatt.ParametriSpybatt();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.SB_R_ParametriLettura;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    skipHead = true;
                    Log.Debug("SB Lettura Parametri");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                    if (_esito)
                    {
                        ParametriGenerali = new MoriData.sbParametriGenerali();
                        ParametriGenerali.IdApparato = _idCorrente;
                        ParametriGenerali.LettureCorrente = _mS.ParametriGenerali.LettureCorrente;
                        ParametriGenerali.LettureTensione = _mS.ParametriGenerali.LettureTensione;
                        ParametriGenerali.DurataPausa = _mS.ParametriGenerali.DurataPausa;
                        ParametriGenerali.DataInserimento = _mS.ParametriGenerali.IstanteLettura;

                    }
                }
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCalibrazioni: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }




        /// <summary>
        /// Legge direttamente dallo SPY-BATT collegato i valori dei coefficenti di calibrazione
        /// </summary>
        /// <param name="IdApparato">ID dell'apparato collegato</param>
        /// <param name="ApparatoConnesso">Se true tento la lettura diretta</param>
        /// <returns></returns>
        public bool CaricaCalibrazioni(string IdApparato, bool ApparatoConnesso)
        {
            try
            {
                bool _esito;
                //
                //                _idCorrente = IdApparato;
                //                
                _esito = false;

                if (ApparatoConnesso)
                {
                    // Eseguo solo se la connessione all'apparato è attiva
                    _mS.valoriCalibrazione = new MessaggioSpyBatt.CalibrazioniSpybatt();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.SB_Cal_LetturaGain;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    skipHead = true;
                    Log.Debug("SB Lettura Calibrazioni");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                    if (_esito)
                    {
                        Calibrazioni = new MoriData.sbCalibrazioni();
                        Calibrazioni.IdApparato = _idCorrente;
                        Calibrazioni.AdcCurrentZero = _mS.valoriCalibrazione.AdcCurrentZero;
                        Calibrazioni.AdcCurrentPos = _mS.valoriCalibrazione.AdcCurrentPos;
                        Calibrazioni.AdcCurrentNeg = _mS.valoriCalibrazione.AdcCurrentNeg;
                        Calibrazioni.CurrentPos = _mS.valoriCalibrazione.CurrentPos;
                        Calibrazioni.CurrentNeg = _mS.valoriCalibrazione.CurrentNeg;
                        Calibrazioni.GainVbatt = _mS.valoriCalibrazione.GainVbatt;
                        Calibrazioni.ValVbatt = _mS.valoriCalibrazione.ValVbatt;
                        Calibrazioni.GainVbatt3 = _mS.valoriCalibrazione.GainVbatt3;
                        Calibrazioni.ValVbatt3 = _mS.valoriCalibrazione.ValVbatt3;
                        Calibrazioni.GainVbatt2 = _mS.valoriCalibrazione.GainVbatt2;
                        Calibrazioni.ValVbatt2 = _mS.valoriCalibrazione.ValVbatt2;
                        Calibrazioni.GainVbatt1 = _mS.valoriCalibrazione.GainVbatt1;
                        Calibrazioni.ValVbatt1 = _mS.valoriCalibrazione.ValVbatt1;

                        Calibrazioni.IstanteLettura = _mS.valoriCalibrazione.IstanteLettura;

                    }
                }
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCalibrazioni: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }


        /// <summary>
        /// CaricaDatiCliente:  I dati relativi al cliente (4 Record)
        /// se la scheda è collegata aggiorna i dati con quelli presenti sulla scheda
        /// </summary>
        /// <param name="IdApparato"></param>
        /// <param name="ApparatoConnesso">True se l'apparato è connesso via COM/USB port</param>
        /// <returns></returns>
        public bool CaricaDatiCliente(string IdApparato, bool ApparatoConnesso)
        {
            try
            {
                bool _esito;
                bool _recordPresente;
                _recordPresente = sbCliente.caricaDati(IdApparato,1);
                if (_recordPresente != true)
                {
                    sbCliente.IdApparato = IdApparato;
                    sbCliente.IdCliente = 1;
                }
                else
                {
                    _idCorrente = IdApparato;
                }

                if (ApparatoConnesso)
                {
                    _mS.CustomerData = new MessaggioSpyBatt.DatiCliente();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.SB_R_DatiCliente;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    skipHead = true;
                    Log.Debug("SB Dati Cliente");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _startRead = DateTime.Now;

                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                    do
                    {
                        if (raggiuntoTimeout(_startRead, _timeOut))
                        {
                            Log.Error("Raggiunto Timeout - Cliente -  dopo elemento " + _mS.CustomerData.stepReceived.ToString());
                            break;
                            //return true;
                        }
                        _esito = aspettaRisposta(elementiComuni.TimeoutBase, 4, false);
                        

                    }
                    while (_mS.CustomerData.datiPronti == false);
//                    while (!_mS.CustomerData.datiPronti & !_esito) ;
                    Log.Error("Uscito dal ciclo cliente; dati pronti =  " + _mS.CustomerData.datiPronti.ToString());

                    if (_mS.CustomerData.datiPronti)
                    {
                        sbCliente.IdApparato = IdApparato;
                        sbCliente.BatteryBrand = _mS.CustomerData.BatteryBrand;
                        sbCliente.BatteryId = _mS.CustomerData.BatteryId;
                        sbCliente.BatteryModel = _mS.CustomerData.BatteryModel;
                        sbCliente.Client = _mS.CustomerData.Client;
                        sbCliente.ClientNote = _mS.CustomerData.ClientNote;
                        sbCliente.CicliAttesi = _mS.CustomerData.CicliAttesi;
                        sbCliente.SerialNumber = _mS.CustomerData.SerialNumber;
                        sbCliente.ModoPianificazione = _mS.CustomerData.ModoPianificazione;
                        sbCliente.ModoBiberonaggio = _mS.CustomerData.ModoBiberonaggio;
                        sbCliente.ModoRabboccatore = _mS.CustomerData.ModoRabboccatore ;
                        sbCliente.MappaTurni = _mS.CustomerData.ModelloPianificazione;

                        sbCliente.salvaDati();
                    }
                }
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaTestata: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }

        /// <summary>
        /// Leggi l'intera memoria flash dalla scheda.
        /// </summary>
        /// <param name="IdApparato">The identifier apparato.</param>
        /// <param name="ApparatoConnesso">if set to <c>true</c> [apparato connesso].</param>
        /// <param name="dbCorrente">The database corrente.</param>
        /// <param name="AckPacchetto">Se true manda un ACK ad ogni pacchetto ricevuto.</param>
        /// <param name="RunAsinc">if set to <c>true</c> [run asinc].</param>
        /// <returns></returns>
        public bool LeggiInteraMemoria(string IdApparato, bool ApparatoConnesso, MoriData._db dbCorrente, bool AckPacchetto, bool RunAsinc = false, bool CreaHexDump = false, string HexDumpFile = "")
        {
            try
            {
                bool _esito;
                object _dataRx;

                bool _RaggiuntoTO = false;

                sbEndStep _esitoBg = new sbEndStep();
                sbWaitStep _stepBg = new sbWaitStep();
                SerialMessage.LadeLightBool _AckPacchetto = SerialMessage.LadeLightBool.False;
                MappaMemoria _mappaCorrente = new MappaMemoria();

                bool _recordPresente;
                if (ApparatoConnesso)
                {

                    MemorySlice = sbData.NumPacchetti;
                    _mS.fwLevel = sbData.fwLevel;
                    _mS.DumpMem = new MessaggioSpyBatt.ImmagineDumpMem();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.SB_R_DumpMemoria;
                    //_mS.ComponiMessaggio();
                    if(AckPacchetto == true) _AckPacchetto = SerialMessage.LadeLightBool.True;
                    _mS.ComponiMessaggioLeggiInteraMemoria(_AckPacchetto);

                    _rxRisposta = false;
                    skipHead = true;
                    Log.Debug("SB DumpMem");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _startRead = DateTime.Now;
                    _inviaAckPacchettoDump = AckPacchetto;
                    if (RunAsinc)
                    {
                        //Preparo l'intestazione della finestra di avanzamento
                        if (Step != null)
                        {
                            sbWaitStep _passo = new sbWaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                            _passo.Titolo = "Fase 1 - Caricamento Immagine Memoria";
                            _passo.Eventi = 1;
                            _passo.Step = -1;
                            _passo.EsecuzioneInterrotta = false;
                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                            Step(this, _stepEv);
                        }

                    }

                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);


                    if (_parametri.CanaleSpyBat == parametriSistema.CanaleDispositivo.USB)
                    {
                        //_esito = aspettaRisposta(elementiComuni.TimeoutBase, (int)(cicloEnd - cicloStart + 1), false,true);+1 ????
                        //_esito = aspettaRisposta(elementiComuni.TimeoutBase, out _dataRx,8280 , false, true, elementiComuni.tipoMessaggio.DumpMemoria);
                        _esito = aspettaRisposta(elementiComuni.TimeoutBase, out _dataRx, MemorySlice, false, true, elementiComuni.tipoMessaggio.DumpMemoria);
                    }
                    else
                    {

                        _esito = aspettaRisposta(elementiComuni.TimeoutBase);
                        do
                        {
                            if (raggiuntoTimeout(_startRead, _timeOut))
                            {
                                Log.Error("Raggiunto Timeout - Attesa Mem Lunga -  dopo elemento " + _mS.DumpMem.NumStep.ToString());
                                _RaggiuntoTO = true;
                                break;
                                //return true;
                            }
                            _esito = aspettaRisposta(elementiComuni.TimeoutBase);
                            _rxRisposta = false;
                        }
                        while (_esito & _mS.DumpMem.NumStep <= MemorySlice);
                    }
                    Log.Warn("Uscito dal ciclo Memoria; NumStep =  " + _mS.DumpMem.NumStep.ToString());
                    Log.Warn("Bytes Caricati:  numBytes finali" + _mS.DumpMem.numBytes.ToString());

                    if (_mS.DumpMem.NumStep == MemorySlice) //
                    {
                        //apro un'unica transazione sul DB

                        // prima, se previsto salvo  l'immagine su file 
                        if(CreaHexDump == true)
                        {
                            if (HexDumpFile != "")
                            {
                                File.WriteAllBytes(HexDumpFile, _mS.DumpMem.memImage);
                            }
                        }


                        //return SpacchettaMemoria(IdApparato, _mS.DumpMem.memImage, dbCorrente, RunAsinc);


                        // Recupero la mappa di meoria del FW Attuale
                        _mappaCorrente = sbData.ModelloMemoria();

                        dbCorrente.BeginTransaction();

                        //stutture per la decompressione memoria
                        byte[] _TempMessaggio;  //frammento in analisi
                        SerialMessage.EsitoRisposta _esitoLettura;
                        int _areaSize = 0;
                        int _areaStart = 0;
                        int _areaBlock = 0;

                        // Finito il download dalla scheda, comincio a spacchettare la memoria
                        // -------------------------------------------------------------------------------------------
                        // Step 2.1: Carico la testata                        
                        if (RunAsinc)
                        {
                            //Preparo l'intestazione della finestra di avanzamento
                            if (Step != null)
                            {
                                sbWaitStep _passo = new sbWaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = "Fase 2.1 - Scomposizione Memoria: Testata";
                                _passo.Eventi = 10;
                                _passo.Step = -1;
                                _passo.EsecuzioneInterrotta = false;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(1, _passo);
                                Step(this, _stepEv);
                            }

                        }


                        _areaSize = _mappaCorrente.Testata.ElemetSize;
                        _areaStart = _mappaCorrente.Testata.StartAddress;
                        _areaBlock = _mappaCorrente.Testata.NoOfElemets;

                        _TempMessaggio = new byte[_areaSize];

                        Array.Copy(_mS.DumpMem.memImage, _areaStart, _TempMessaggio, 0, _areaSize);
                        Log.Warn("---------------------- TESTATA (NON ATTIVA) ------------------------------");
                        
                        Log.Warn(FunzioniMR.hexdumpArray(_TempMessaggio, true));

                        // Non carico il record testata: i contatori non sono valorizzati: controllo solo che il record sia valido
                        // TODO: 
                        _esitoLettura = _mS.Intestazione.analizzaMessaggio(_TempMessaggio, true);

                        if (_esitoLettura != SerialMessage.EsitoRisposta.MessaggioOk)
                        {
                            return false;
                        }

                        if (RunAsinc)
                        {
                            //Preparo l'intestazione della finestra di avanzamento
                            if (Step != null)
                            {
                                sbWaitStep _passo = new sbWaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = "Fase 2.2 - Scomposizione Memoria: Cliente";
                                _passo.Eventi = 10;
                                _passo.Step = -1;
                                _passo.EsecuzioneInterrotta = false;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                Step(this, _stepEv);
                            }

                        }


                        // Step 2.2: Carico i dati cliente
                        /*
                        _areaSize = 251;
                        _areaStart = 0X40;
                        _areaBlock = 4;
                        */

                        _areaSize = _mappaCorrente.DatiCliente.ElemetSize;
                        _areaStart = _mappaCorrente.DatiCliente.StartAddress;
                        _areaBlock = _mappaCorrente.DatiCliente.NoOfElemets;


                        Log.Warn("---------------------- CLIENTE ------------------------------");
                        _TempMessaggio = new byte[_areaSize];


                        if (RunAsinc)
                        {
                            //Preparo l'intestazione della finestra di avanzamento
                            if (Step != null)
                            {
                                sbWaitStep _passo = new sbWaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = "Fase 2.2 - Scomposizione Memoria: Cliente";
                                _passo.Eventi = 10;
                                _passo.Step = 2;
                                _passo.EsecuzioneInterrotta = false;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                Step(this, _stepEv);
                            }

                        }



                        for (int _cicloCli = 0; _cicloCli < _areaBlock; _cicloCli++)
                        {
                            Array.Copy(_mS.DumpMem.memImage, _areaStart, _TempMessaggio, 0, _areaSize);
                            Log.Warn(FunzioniMR.hexdumpArray(_TempMessaggio, true));
                            _esitoLettura = _mS.CustomerData.analizzaMessaggio(_TempMessaggio, true);
                            _areaStart += _areaSize;
                        }
                        _esitoLettura = _mS.Intestazione.analizzaMessaggio(_TempMessaggio, true);
                        if (_mS.CustomerData.datiPronti)
                        {
                            sbCliente.IdApparato = IdApparato;
                            sbCliente.BatteryBrand = _mS.CustomerData.BatteryBrand;
                            sbCliente.BatteryId = _mS.CustomerData.BatteryId;
                            sbCliente.BatteryModel = _mS.CustomerData.BatteryModel;
                            sbCliente.Client = _mS.CustomerData.Client;
                            sbCliente.ClientNote = _mS.CustomerData.ClientNote;
                            sbCliente.salvaDati();
                        }

                        // Step 2.3: Carico i dati programmazione

                        /*
                        _areaSize = 0x80;
                        _areaStart = 0X440;
                        _areaBlock = sbData.ProgramCount;
                        */

                        _areaSize = _mappaCorrente.Programmazioni.ElemetSize;
                        _areaStart = _mappaCorrente.Programmazioni.StartAddress;
                        _areaBlock = sbData.ProgramCount;

                        _Programmazioni.Clear();

                        if (sbData.ProgramCount > _mappaCorrente.Programmazioni.NoOfElemets)
                        {
                            _areaBlock = _mappaCorrente.Programmazioni.NoOfElemets;  //
                        }

                        Log.Warn("---------------------- Programmazioni: " + _areaBlock.ToString() + " ------------------------------");

                        _TempMessaggio = new byte[_areaSize];
                        for (int _cicloCli = 0; _cicloCli < _areaBlock; _cicloCli++)
                        {
                            _mS.ProgRicarica = new MessaggioSpyBatt.ProgrammaRicarica();
                            Array.Copy(_mS.DumpMem.memImage, _areaStart, _TempMessaggio, 0, _areaSize);
                            Log.Warn(FunzioniMR.hexdumpArray(_TempMessaggio, true));
                            _esitoLettura = _mS.ProgRicarica.analizzaMessaggio(_TempMessaggio, true);
                            if (_esitoLettura == SerialMessage.EsitoRisposta.MessaggioOk)
                            {
                                _Programmazioni.Add(_mS.ProgRicarica);
                            }
                            _areaStart += _areaSize;
                        }
                        if (_Programmazioni.Count > 0)
                        {
                            Programmazioni.Clear();

                            foreach (MessaggioSpyBatt.ProgrammaRicarica _ciclo in _Programmazioni)
                            {
                                sbProgrammaRicarica _progR = new sbProgrammaRicarica(dbCorrente);
                                _progR.caricaDati(_idCorrente, _ciclo.IdProgramma);

                                if (_ciclo.IdProgramma != 0xFFFF)
                                {
                                    // Aggiorno la lunga e ricarico le brevi solo se cambia il N° di brevi o la data fine
                                    // oppure se setto a true il flag AggiornaTutto
                                    _progR.IdProgramma = _ciclo.IdProgramma;
                                    _progR.DataInstallazione = _ciclo.DataInstallazione;
                                    _progR.BatteryType = _ciclo.BatteryType;
                                    _progR.BatteryVdef = _ciclo.BatteryVdef;
                                    _progR.BatteryAhdef = _ciclo.BatteryAhdef;
                                    _progR.BatteryCells = _ciclo.BatteryCells;
                                    _progR.BatteryCell1 = _ciclo.BatteryCell1;
                                    _progR.BatteryCell2 = _ciclo.BatteryCell2;
                                    _progR.BatteryCell3 = _ciclo.BatteryCell3;
                                    _progR.salvaDati();

                                    Programmazioni.Add(_progR);
                                    Log.Debug("Accodato Programma " + _ciclo.IdProgramma.ToString());

                                }
                                else
                                {
                                    Log.Debug("Saltato Programma " + _ciclo.IdProgramma.ToString() + " (" + _Programmazioni.IndexOf(_ciclo) + " )");
                                }

                            }
                        }


                        // Step 2.4: Carico i cicli lunghi
                        /*
                        _areaSize = 0x33;
                        _areaStart = 0X134000;
                        _areaBlock = sbData.LongMem;
                        */

                        _areaSize = _mappaCorrente.MemLunga.ElemetSize;
                        _areaStart = _mappaCorrente.MemLunga.StartAddress;
                        _areaBlock = sbData.LongMem;

                        _CicliMemoriaLunga.Clear();
                        CicliMemoriaLunga.Clear();

                        MessaggioSpyBatt.MemoriaPeriodoLungo _tempLunga;
                        MessaggioSpyBatt.MemoriaPeriodoBreve _tempBreve;

                        if (_areaBlock > _mappaCorrente.MemLunga.NoOfElemets )
                        {
                            _areaBlock = _mappaCorrente.MemLunga.NoOfElemets;   //
                        }

                        Log.Warn("---------------------- Cicli Lunghi: " + _areaBlock.ToString() + " ------------------------------");
                        // private System.Collections.Generic.List<MessaggioSpyBatt.MemoriaPeriodoLungo> _CicliMemoriaLunga = new System.Collections.Generic.List<MessaggioSpyBatt.MemoriaPeriodoLungo>();

                        _TempMessaggio = new byte[_areaSize];

                        //_areaBlock = 106;

                        for (int _cicloCli = 0; _cicloCli < _areaBlock; _cicloCli++)
                        {
                            _tempLunga = new MessaggioSpyBatt.MemoriaPeriodoLungo();
                            Array.Copy(_mS.DumpMem.memImage, _areaStart, _TempMessaggio, 0, _areaSize);
                            //Log.Warn(FunzioniMR.hexdumpArray(_TempMessaggio, true));
                            _esitoLettura = _tempLunga.analizzaMessaggio(_TempMessaggio, sbData.fwLevel, true);
                            if (_esitoLettura == SerialMessage.EsitoRisposta.MessaggioOk)
                            {
                                _CicliMemoriaLunga.Add(_tempLunga);
                            }
                            _areaStart += _areaSize;
                        }

                        int risposteAttese = _CicliMemoriaLunga.Count;
                        int _lastProgress = 0;

                        if (risposteAttese > 0)
                        {
                            CicliMemoriaLunga.Clear();


                            uint _lastId = 0;
                            int _stepCorrente = 0;

                            foreach (MessaggioSpyBatt.MemoriaPeriodoLungo _ciclo in _CicliMemoriaLunga)
                            {
                                sbMemLunga _memLn = new sbMemLunga(dbCorrente);
                                bool _breviPresenti = false;
                                _stepCorrente++;


                                if (_ciclo.IdEvento >= (uint)0xFFFF)
                                {
                                    if (_lastId > 0)
                                    {
                                        // se il record è a FF e non ho un precedente valido salto al record sucessivo
                                        Log.Warn("ID non valido " + _ciclo.IdEvento.ToString("X2") + " ! " + _lastId.ToString("X2"));
                                        _ciclo.IdEvento = ++_lastId;
                                    }
                                    else
                                    {
                                      
                                        continue;
                                    }
                                }

                                _memLn.caricaDati(_idCorrente, (uint)_ciclo.IdEvento);
                                CaricaMessaggioMemLunga(_idCorrente, (uint)_ciclo.IdEvento, _ciclo, ref _memLn);
                                _lastId = _ciclo.IdEvento;
                                _CicliMemoriaBreve.Clear();
                                _memLn.CaricaProgramma();
                                _memLn.CicliMemoriaBreve.Clear();
                                // Ora carico i brevi:
                                if(_memLn.NumEventiBrevi > 00)
                                {
                                    if((_memLn.NumEventiBrevi != 0xFFFF) & (_memLn.PuntatorePrimoBreve < 0xFFFFFF ))
                                    {

                                        _breviPresenti = true;

                                    }
                                }


                                if (_breviPresenti)
                                {
                                    /*
                                    _areaSize = 0x1A;
                                    _areaStart = 0x002000 + (int)( _memLn.PuntatorePrimoBreve *_areaSize) ;
                                    _areaBlock =_memLn.NumEventiBrevi;
                                    */

                                    _areaSize = _mappaCorrente.MemBreve.ElemetSize;
                                    _areaStart = _mappaCorrente.MemBreve.StartAddress + (int)(_memLn.PuntatorePrimoBreve * _areaSize);
                                    _areaBlock =  _memLn.NumEventiBrevi;


                                    Log.Warn("---------------------- Cicli Brevi: " + _areaBlock.ToString() + " ------------------------------");

                                    _TempMessaggio = new byte[_areaSize];

                                    for (int _cicloBreve = 0; _cicloBreve < _areaBlock; _cicloBreve++)
                                    {
                                        _tempBreve = new MessaggioSpyBatt.MemoriaPeriodoBreve();
                                        Array.Copy(_mS.DumpMem.memImage, _areaStart, _TempMessaggio, 0, _areaSize);
                                        //Log.Warn(FunzioniMR.hexdumpArray(_TempMessaggio, true));
                                        _esitoLettura = _tempBreve.analizzaMessaggio(_TempMessaggio, true);
                                        if (_esitoLettura == SerialMessage.EsitoRisposta.MessaggioOk)
                                        {
                                            _CicliMemoriaBreve.Add(_tempBreve);
                                        }
                                        _areaStart += _areaSize;
                                    }
                                   
                                    // ora trasporto i messaggi nel modello
                                    Log.Debug("----------------------------------------------------------------------------------------");
                                    Log.Debug("Caricati gli eventi a breve (" + _CicliMemoriaBreve.Count.ToString() + ") aggiorno il DB");
                                    Log.Debug("----------------------------------------------------------------------------------------");
                                    _memLn.CancellaBrevi();
                                    int _numCiclo = 1;
                                    foreach (MessaggioSpyBatt.MemoriaPeriodoBreve _cicloBr in _CicliMemoriaBreve)
                                    {
                                        sbMemBreve _memBr = new sbMemBreve(dbCorrente);
                                        //_memBr.caricaDati(_idCorrente, (int)IdCicloLungo, _numCiclo);

                                        // Aggiorno la lunga e ricarico le brevi
                                            
                                        _memBr.IdApparato = _idCorrente;
                                        _memBr.IdMemoriaLunga = (int)_memLn.IdMemoriaLunga;
                                        _memBr.IdMemoriaBreve = _numCiclo;
                                        _memBr.DataOraRegistrazione = StringaTimestamp(_cicloBr.DataOraRegistrazione);
                                        _memBr.Vreg = _cicloBr.Vreg;
                                            _memBr.V1 = _cicloBr.V1;
                                            _memBr.V2 = _cicloBr.V2;
                                            _memBr.V3 = _cicloBr.V3;
                                            _memBr.Amed = _cicloBr.Amed;
                                            _memBr.Amin = _cicloBr.Amin;
                                            _memBr.Amax = _cicloBr.Amax;
                                            _memBr.Tntc = _cicloBr.Tntc;
                                            _memBr.PresenzaElettrolita = _cicloBr.PresenzaElettrolita;
                                            _memBr.VbatBk = _cicloBr.VbatBk;
                                            _numCiclo++;

                                        _memLn.CicliMemoriaBreve.Add(_memBr);
                                    }
                                    _memLn.SalvaBrevi();
                                    Log.Warn("CICLI Breve: Fine Salvataggio");
                                }



                               // Il ciclo è completo, lo accodo
                                CicliMemoriaLunga.Add(_memLn);

                                if (RunAsinc == true)
                                {
                                    if (Step != null)
                                    {
                                        int _progress = 0;
                                        double _valProgress = 0;
                                        sbWaitStep _passo = new sbWaitStep();
                                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                        _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                        _passo.Eventi = risposteAttese;
                                        _passo.Step = _stepCorrente;
                                        _passo.EsecuzioneInterrotta = false;
                                        if (risposteAttese > 0)
                                        {
                                            _valProgress = (_stepCorrente * 100) / risposteAttese;
                                        }
                                        _progress = (int)_valProgress;
                                        if (_lastProgress != _progress)
                                        {
                                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                            //Log.Debug("Passo " + _risposteRicevute.ToString());
                                            Step(this, _stepEv);
                                            _lastProgress = _progress;
                                        }
                                    }
                                }


                            }

                            //ora consolido gli intermedi
                            ConsolidaBrevi();
                        }

                        
                    }
                    else
                    {
                        Log.Warn("DumpMem fallito: dati pronti = false  - Pacchetti: " + _mS.DumpMem.NumStep.ToString());
                        return false;
                    }

                    if (dbCorrente.IsInTransaction) dbCorrente.Commit();
                    
                }
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("Dump Memoria: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                if (dbCorrente.IsInTransaction)
                {
                    try
                    {
                        dbCorrente.Rollback();

                    }
                    catch
                    {
                        Log.Error("Dump Memoria: fallito Rollback");

                    }
                }
                return false;
            }
        }


        public bool SpacchettaMemoria(string IdApparato, byte[] Immagine, MoriData._db dbCorrente,  bool RunAsinc = false)
        {
            try
            {
                bool _esito;
                object _dataRx;

                bool _RaggiuntoTO = false;

                sbEndStep _esitoBg = new sbEndStep();
                sbWaitStep _stepBg = new sbWaitStep();
                SerialMessage.LadeLightBool _AckPacchetto = SerialMessage.LadeLightBool.False;
                MappaMemoria _mappaCorrente = new MappaMemoria();

                bool _recordPresente;
                {

                    MemorySlice = sbData.NumPacchetti;
                    _mS.fwLevel = sbData.fwLevel;
                    Log.Warn("Bytes Caricati:  numBytes finali" + Immagine.Length.ToString());

                    if (Immagine.Length == MemorySlice) //
                    {
                        // Recupero la mappa di meoria del FW Attuale
                        _mappaCorrente = sbData.ModelloMemoria();

                        //apro un'unica transazione sul DB
                        dbCorrente.BeginTransaction();

                        //stutture per la decompressione memoria
                        byte[] _TempMessaggio;  //frammento in analisi
                        SerialMessage.EsitoRisposta _esitoLettura;
                        int _areaSize = 0;
                        int _areaStart = 0;
                        int _areaBlock = 0;

                        // Finito il download dalla scheda, comincio a spacchettare la memoria
                        // -------------------------------------------------------------------------------------------
                        // Step 2.1: Carico la testata                        
                        if (RunAsinc)
                        {
                            //Preparo l'intestazione della finestra di avanzamento
                            if (Step != null)
                            {
                                sbWaitStep _passo = new sbWaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = "Fase 2.1 - Scomposizione Memoria: Testata";
                                _passo.Eventi = 10;
                                _passo.Step = -1;
                                _passo.EsecuzioneInterrotta = false;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(1, _passo);
                                Step(this, _stepEv);
                            }

                        }


                        _areaSize = _mappaCorrente.Testata.ElemetSize;
                        _areaStart = _mappaCorrente.Testata.StartAddress;
                        _areaBlock = _mappaCorrente.Testata.NoOfElemets;

                        _TempMessaggio = new byte[_areaSize];

                        Array.Copy(Immagine, _areaStart, _TempMessaggio, 0, _areaSize);
                        Log.Warn("---------------------- TESTATA (NON ATTIVA) ------------------------------");

                        Log.Warn(FunzioniMR.hexdumpArray(_TempMessaggio, true));

                        // Non carico il record testata: i contatori non sono valorizzati: controllo solo che il record sia valido
                        // TODO: 
                        _esitoLettura = _mS.Intestazione.analizzaMessaggio(_TempMessaggio, true);

                        if (_esitoLettura != SerialMessage.EsitoRisposta.MessaggioOk)
                        {
                            return false;
                        }

                        if (RunAsinc)
                        {
                            //Preparo l'intestazione della finestra di avanzamento
                            if (Step != null)
                            {
                                sbWaitStep _passo = new sbWaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = "Fase 2.2 - Scomposizione Memoria: Cliente";
                                _passo.Eventi = 10;
                                _passo.Step = -1;
                                _passo.EsecuzioneInterrotta = false;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                Step(this, _stepEv);
                            }

                        }


                        // Step 2.2: Carico i dati cliente
                        /*
                        _areaSize = 251;
                        _areaStart = 0X40;
                        _areaBlock = 4;
                        */

                        _areaSize = _mappaCorrente.DatiCliente.ElemetSize;
                        _areaStart = _mappaCorrente.DatiCliente.StartAddress;
                        _areaBlock = _mappaCorrente.DatiCliente.NoOfElemets;


                        Log.Warn("---------------------- CLIENTE ------------------------------");
                        _TempMessaggio = new byte[_areaSize];


                        if (RunAsinc)
                        {
                            //Preparo l'intestazione della finestra di avanzamento
                            if (Step != null)
                            {
                                sbWaitStep _passo = new sbWaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = "Fase 2.2 - Scomposizione Memoria: Cliente";
                                _passo.Eventi = 10;
                                _passo.Step = 2;
                                _passo.EsecuzioneInterrotta = false;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                Step(this, _stepEv);
                            }

                        }



                        for (int _cicloCli = 0; _cicloCli < _areaBlock; _cicloCli++)
                        {
                            Array.Copy(Immagine, _areaStart, _TempMessaggio, 0, _areaSize);
                            Log.Warn(FunzioniMR.hexdumpArray(_TempMessaggio, true));
                            _esitoLettura = _mS.CustomerData.analizzaMessaggio(_TempMessaggio, true);
                            _areaStart += _areaSize;
                        }
                        _esitoLettura = _mS.Intestazione.analizzaMessaggio(_TempMessaggio, true);
                        if (_mS.CustomerData.datiPronti)
                        {
                            sbCliente.IdApparato = IdApparato;
                            sbCliente.BatteryBrand = _mS.CustomerData.BatteryBrand;
                            sbCliente.BatteryId = _mS.CustomerData.BatteryId;
                            sbCliente.BatteryModel = _mS.CustomerData.BatteryModel;
                            sbCliente.Client = _mS.CustomerData.Client;
                            sbCliente.ClientNote = _mS.CustomerData.ClientNote;
                            sbCliente.salvaDati();
                        }

                        // Step 2.3: Carico i dati programmazione

                        /*
                        _areaSize = 0x80;
                        _areaStart = 0X440;
                        _areaBlock = sbData.ProgramCount;
                        */

                        _areaSize = _mappaCorrente.Programmazioni.ElemetSize;
                        _areaStart = _mappaCorrente.Programmazioni.StartAddress;
                        _areaBlock = sbData.ProgramCount;

                        _Programmazioni.Clear();

                        if (sbData.ProgramCount > _mappaCorrente.Programmazioni.NoOfElemets)
                        {
                            _areaBlock = _mappaCorrente.Programmazioni.NoOfElemets;  //
                        }

                        Log.Warn("---------------------- Programmazioni: " + _areaBlock.ToString() + " ------------------------------");

                        _TempMessaggio = new byte[_areaSize];
                        for (int _cicloCli = 0; _cicloCli < _areaBlock; _cicloCli++)
                        {
                            _mS.ProgRicarica = new MessaggioSpyBatt.ProgrammaRicarica();
                            Array.Copy(_mS.DumpMem.memImage, _areaStart, _TempMessaggio, 0, _areaSize);
                            Log.Warn(FunzioniMR.hexdumpArray(_TempMessaggio, true));
                            _esitoLettura = _mS.ProgRicarica.analizzaMessaggio(_TempMessaggio, true);
                            if (_esitoLettura == SerialMessage.EsitoRisposta.MessaggioOk)
                            {
                                _Programmazioni.Add(_mS.ProgRicarica);
                            }
                            _areaStart += _areaSize;
                        }
                        if (_Programmazioni.Count > 0)
                        {
                            Programmazioni.Clear();

                            foreach (MessaggioSpyBatt.ProgrammaRicarica _ciclo in _Programmazioni)
                            {
                                sbProgrammaRicarica _progR = new sbProgrammaRicarica(dbCorrente);
                                _progR.caricaDati(_idCorrente, _ciclo.IdProgramma);

                                if (_ciclo.IdProgramma != 0xFFFF)
                                {
                                    // Aggiorno la lunga e ricarico le brevi solo se cambia il N° di brevi o la data fine
                                    // oppure se setto a true il flag AggiornaTutto
                                    _progR.IdProgramma = _ciclo.IdProgramma;
                                    _progR.DataInstallazione = _ciclo.DataInstallazione;
                                    _progR.BatteryType = _ciclo.BatteryType;
                                    _progR.BatteryVdef = _ciclo.BatteryVdef;
                                    _progR.BatteryAhdef = _ciclo.BatteryAhdef;
                                    _progR.BatteryCells = _ciclo.BatteryCells;
                                    _progR.BatteryCell1 = _ciclo.BatteryCell1;
                                    _progR.BatteryCell2 = _ciclo.BatteryCell2;
                                    _progR.BatteryCell3 = _ciclo.BatteryCell3;
                                    _progR.salvaDati();

                                    Programmazioni.Add(_progR);
                                    Log.Debug("Accodato Programma " + _ciclo.IdProgramma.ToString());

                                }
                                else
                                {
                                    Log.Debug("Saltato Programma " + _ciclo.IdProgramma.ToString() + " (" + _Programmazioni.IndexOf(_ciclo) + " )");
                                }

                            }
                        }


                        // Step 2.4: Carico i cicli lunghi
                        /*
                        _areaSize = 0x33;
                        _areaStart = 0X134000;
                        _areaBlock = sbData.LongMem;
                        */

                        _areaSize = _mappaCorrente.MemLunga.ElemetSize;
                        _areaStart = _mappaCorrente.MemLunga.StartAddress;
                        _areaBlock = sbData.LongMem;

                        _CicliMemoriaLunga.Clear();
                        CicliMemoriaLunga.Clear();

                        MessaggioSpyBatt.MemoriaPeriodoLungo _tempLunga;
                        MessaggioSpyBatt.MemoriaPeriodoBreve _tempBreve;

                        if (_areaBlock > _mappaCorrente.MemLunga.NoOfElemets)
                        {
                            _areaBlock = _mappaCorrente.MemLunga.NoOfElemets;   //
                        }

                        Log.Warn("---------------------- Cicli Lunghi: " + _areaBlock.ToString() + " ------------------------------");
                        // private System.Collections.Generic.List<MessaggioSpyBatt.MemoriaPeriodoLungo> _CicliMemoriaLunga = new System.Collections.Generic.List<MessaggioSpyBatt.MemoriaPeriodoLungo>();

                        _TempMessaggio = new byte[_areaSize];

                        //_areaBlock = 106;

                        for (int _cicloCli = 0; _cicloCli < _areaBlock; _cicloCli++)
                        {
                            _tempLunga = new MessaggioSpyBatt.MemoriaPeriodoLungo();
                            Array.Copy(_mS.DumpMem.memImage, _areaStart, _TempMessaggio, 0, _areaSize);
                            //Log.Warn(FunzioniMR.hexdumpArray(_TempMessaggio, true));
                            _esitoLettura = _tempLunga.analizzaMessaggio(_TempMessaggio, sbData.fwLevel, true);
                            if (_esitoLettura == SerialMessage.EsitoRisposta.MessaggioOk)
                            {
                                _CicliMemoriaLunga.Add(_tempLunga);
                            }
                            _areaStart += _areaSize;
                        }

                        int risposteAttese = _CicliMemoriaLunga.Count;
                        int _lastProgress = 0;

                        if (risposteAttese > 0)
                        {
                            CicliMemoriaLunga.Clear();


                            uint _lastId = 0;
                            int _stepCorrente = 0;

                            foreach (MessaggioSpyBatt.MemoriaPeriodoLungo _ciclo in _CicliMemoriaLunga)
                            {
                                sbMemLunga _memLn = new sbMemLunga(dbCorrente);
                                bool _breviPresenti = false;
                                _stepCorrente++;


                                if (_ciclo.IdEvento >= (uint)0xFFFF)
                                {
                                    if (_lastId > 0)
                                    {
                                        // se il record è a FF e non ho un precedente valido salto al record sucessivo
                                        Log.Warn("ID non valido " + _ciclo.IdEvento.ToString("X2") + " ! " + _lastId.ToString("X2"));
                                        _ciclo.IdEvento = ++_lastId;
                                    }
                                    else
                                    {

                                        continue;
                                    }
                                }

                                _memLn.caricaDati(_idCorrente, (uint)_ciclo.IdEvento);
                                CaricaMessaggioMemLunga(_idCorrente, (uint)_ciclo.IdEvento, _ciclo, ref _memLn);
                                _lastId = _ciclo.IdEvento;
                                _CicliMemoriaBreve.Clear();
                                _memLn.CicliMemoriaBreve.Clear();
                                // Ora carico i brevi:
                                if (_memLn.NumEventiBrevi > 00)
                                {
                                    if ((_memLn.NumEventiBrevi != 0xFFFF) & (_memLn.PuntatorePrimoBreve < 0xFFFFFF))
                                    {

                                        _breviPresenti = true;

                                    }
                                }


                                if (_breviPresenti)
                                {
                                    /*
                                    _areaSize = 0x1A;
                                    _areaStart = 0x002000 + (int)( _memLn.PuntatorePrimoBreve *_areaSize) ;
                                    _areaBlock =_memLn.NumEventiBrevi;
                                    */

                                    _areaSize = _mappaCorrente.MemBreve.ElemetSize;
                                    _areaStart = _mappaCorrente.MemBreve.StartAddress + (int)(_memLn.PuntatorePrimoBreve * _areaSize);
                                    _areaBlock = _memLn.NumEventiBrevi;


                                    Log.Warn("---------------------- Cicli Brevi: " + _areaBlock.ToString() + " ------------------------------");

                                    _TempMessaggio = new byte[_areaSize];

                                    for (int _cicloBreve = 0; _cicloBreve < _areaBlock; _cicloBreve++)
                                    {
                                        _tempBreve = new MessaggioSpyBatt.MemoriaPeriodoBreve();
                                        Array.Copy(_mS.DumpMem.memImage, _areaStart, _TempMessaggio, 0, _areaSize);
                                        //Log.Warn(FunzioniMR.hexdumpArray(_TempMessaggio, true));
                                        _esitoLettura = _tempBreve.analizzaMessaggio(_TempMessaggio, true);
                                        if (_esitoLettura == SerialMessage.EsitoRisposta.MessaggioOk)
                                        {
                                            _CicliMemoriaBreve.Add(_tempBreve);
                                        }
                                        _areaStart += _areaSize;
                                    }

                                    // ora trasporto i messaggi nel modello
                                    Log.Debug("----------------------------------------------------------------------------------------");
                                    Log.Debug("Caricati gli eventi a breve (" + _CicliMemoriaBreve.Count.ToString() + ") aggiorno il DB");
                                    Log.Debug("----------------------------------------------------------------------------------------");
                                    _memLn.CancellaBrevi();
                                    int _numCiclo = 1;
                                    foreach (MessaggioSpyBatt.MemoriaPeriodoBreve _cicloBr in _CicliMemoriaBreve)
                                    {
                                        sbMemBreve _memBr = new sbMemBreve(dbCorrente);
                                        //_memBr.caricaDati(_idCorrente, (int)IdCicloLungo, _numCiclo);

                                        // Aggiorno la lunga e ricarico le brevi

                                        _memBr.IdApparato = _idCorrente;
                                        _memBr.IdMemoriaLunga = (int)_memLn.IdMemoriaLunga;
                                        _memBr.IdMemoriaBreve = _numCiclo;
                                        _memBr.DataOraRegistrazione = StringaTimestamp(_cicloBr.DataOraRegistrazione);
                                        _memBr.Vreg = _cicloBr.Vreg;
                                        _memBr.V1 = _cicloBr.V1;
                                        _memBr.V2 = _cicloBr.V2;
                                        _memBr.V3 = _cicloBr.V3;
                                        _memBr.Amed = _cicloBr.Amed;
                                        _memBr.Amin = _cicloBr.Amin;
                                        _memBr.Amax = _cicloBr.Amax;
                                        _memBr.Tntc = _cicloBr.Tntc;
                                        _memBr.PresenzaElettrolita = _cicloBr.PresenzaElettrolita;
                                        _memBr.VbatBk = _cicloBr.VbatBk;
                                        _numCiclo++;

                                        _memLn.CicliMemoriaBreve.Add(_memBr);
                                    }
                                    _memLn.SalvaBrevi();
                                    Log.Warn("CICLI Breve: Fine Salvataggio");
                                }



                                // Il ciclo è completo, lo accodo
                                CicliMemoriaLunga.Add(_memLn);

                                if (RunAsinc == true)
                                {
                                    if (Step != null)
                                    {
                                        int _progress = 0;
                                        double _valProgress = 0;
                                        sbWaitStep _passo = new sbWaitStep();
                                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                        _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                        _passo.Eventi = risposteAttese;
                                        _passo.Step = _stepCorrente;
                                        _passo.EsecuzioneInterrotta = false;
                                        if (risposteAttese > 0)
                                        {
                                            _valProgress = (_stepCorrente * 100) / risposteAttese;
                                        }
                                        _progress = (int)_valProgress;
                                        if (_lastProgress != _progress)
                                        {
                                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                            //Log.Debug("Passo " + _risposteRicevute.ToString());
                                            Step(this, _stepEv);
                                            _lastProgress = _progress;
                                        }
                                    }
                                }


                            }

                            //ora consolido gli intermedi
                            ConsolidaBrevi();
                        }


                    }
                    else
                    {
                        Log.Warn("DumpMem fallito: dati pronti = false  - Pacchetti: " + _mS.DumpMem.NumStep.ToString());
                        return false;
                    }

                    if (dbCorrente.IsInTransaction) dbCorrente.Commit();

                }
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("Dump Memoria: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                if (dbCorrente.IsInTransaction)
                {
                    try
                    {
                        dbCorrente.Rollback();

                    }
                    catch
                    {
                        Log.Error("Dump Memoria: fallito Rollback");

                    }
                }
                return false;
            }
        }


        public bool CaricaCicliMemLunga(string IdApparato, MoriData._db dbCorrente)
        {
            try
            {
                CicliMemoriaLunga.Clear();
                DataCicliMemoriaLunga.Clear();


                if ( LMListChange != null)
                {
                    sbListaLunghiEvt eventoLM = new sbListaLunghiEvt();
                    eventoLM.EventiLunghi = CicliMemoriaLunga.Count;
                    LMListChange(this, eventoLM);
                }
                IEnumerable<_sbMemLunga> _TempCicli = dbCorrente.Query<_sbMemLunga> ( "select * from _sbMemLunga where IdApparato = ? order by IdMemoriaLunga desc", IdApparato);

                foreach ( _sbMemLunga Elemento in _TempCicli )
                {
                    sbMemLunga _cLoc;
                    _cLoc = new sbMemLunga(dbCorrente);
                    _cLoc.LivelloUser = LivelloUser;
                    if (_cLoc.caricaDati(Elemento.IdLocale))
                    {
                        _cLoc.VersoScarica = VersoScarica;
                        CicliMemoriaLunga.Add( _cLoc );
                        DataCicliMemoriaLunga.Add(_cLoc._sblm);
                    }
                
                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return false;
            }
        }


        public bool CaricaProgrammazioni(string IdApparato, MoriData._db dbCorrente)
        {
            try
            {
                Programmazioni.Clear();

                IEnumerable<_sbProgrammaRicarica> _TempCicli = dbCorrente.Query<_sbProgrammaRicarica>("select * from _sbProgrammaRicarica where IdApparato = ? order by IdProgramma desc", IdApparato);

                foreach (_sbProgrammaRicarica Elemento in _TempCicli)
                {
                    sbProgrammaRicarica _pRic;
                    _pRic = new sbProgrammaRicarica(dbCorrente);
                    if (_pRic.caricaDati(Elemento.IdLocale))
                    {
                        Programmazioni.Add(_pRic);
                    }

                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaProgrammazioni " + Ex.Message);
                return false;
            }
        }

        public bool RicaricaCaricaCicliMemLunga(uint cicloStart, uint cicloEnd,MoriData._db dbCorrente, bool AggiornaTutto, bool caricaBrevi )
        {
            object vuoto;
            return RicaricaCaricaCicliMemLunga(cicloStart, cicloEnd, dbCorrente, AggiornaTutto, caricaBrevi, out vuoto);
        }

        protected bool CaricaMessaggioMemLunga(String IdApparato, uint IdEvento, MessaggioSpyBatt.MemoriaPeriodoLungo MsgCiclo, ref sbMemLunga EventoLungo)
        {

            try
            {
                if (EventoLungo != null)
                {

                    // Aggiorno la lunga e ricarico le brevi solo se cambia il N° di brevi o la data fine
                    // oppure se setto a true il flag AggiornaTutto
                    EventoLungo.TipoEvento = MsgCiclo.TipoEvento;
                    EventoLungo.IdProgramma = MsgCiclo.IdProgramma;
                    EventoLungo.PuntatorePrimoBreve = MsgCiclo.PuntatorePrimoBreve;
                    EventoLungo.PuntatorePrimoBreveEff = MsgCiclo.PuntatorePrimoBreve;
                    EventoLungo.NumEventiBrevi = MsgCiclo.NumEventiBrevi;
                    EventoLungo.DataOraStart = FunzioniMR.StringaTimestamp(MsgCiclo.DataOraStart);
                    EventoLungo.DataOraFine = FunzioniMR.StringaTimestamp(MsgCiclo.DataOraFine);
                    EventoLungo.Durata = MsgCiclo.Durata;
                    EventoLungo.TempMin = MsgCiclo.TempMin;
                    EventoLungo.TempMax = MsgCiclo.TempMax;
                    EventoLungo.Vmin = MsgCiclo.Vmin;
                    EventoLungo.Vmax = MsgCiclo.Vmax;
                    EventoLungo.Amin = MsgCiclo.Amin;
                    EventoLungo.Amax = MsgCiclo.Amax;
                    EventoLungo.PresenzaElettrolita = MsgCiclo.PresenzaElettrolita;
                    EventoLungo.Ah = MsgCiclo.Ah;
                    EventoLungo.AhCaricati = MsgCiclo.AhCaricati;
                    EventoLungo.AhScaricati = MsgCiclo.AhScaricati;
                    EventoLungo.Wh = (int)MsgCiclo.Wh;
                    EventoLungo.WhCaricati = (int)MsgCiclo.WhCaricati;
                    EventoLungo.WhScaricati = (int)MsgCiclo.WhScaricati;
                    EventoLungo.CondizioneStop = MsgCiclo.CondizioneStop;
                    EventoLungo.FattoreCarica = MsgCiclo.FattoreCarica;
                    EventoLungo.StatoCarica = MsgCiclo.StatoCatica;
                    EventoLungo.TipoCariatore = MsgCiclo.TipoCariatore;
                    EventoLungo.IdCaricatore = MsgCiclo.IdCaricatore;
                    EventoLungo.DataLastDownload = DateTime.Now;
                    EventoLungo.salvaDati();
                }
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaMemLunga " + Ex.Message);
                return false;
            }

        }






        /// <summary>
        /// Rilegge direttamente da SPY-BATT l'elenco dei cicli lunghi, aggiornando sia la lista in memoria che i dati su DB
        /// Se il firmware è 2.02.04 o successivo l'ultimo ciclo è END + 1 perche il fw chiude il ciclo corrente 
        /// </summary>
        /// <param name="cicloStart">Id primo ciclo da leggere</param>
        /// <param name="cicloEnd">Id ultimo ciclo da leggere</param>
        /// <param name="dbCorrente">Connessione dati attiva</param>
        /// <param name="AggiornaTutto">se TRUE aggiorna anche i record già presenti</param>
        /// <param name="caricaBrevi">se TRUE carica direttamente i brevi</param>
        /// <returns>True se l'operazione termina senza errori</returns>
        public bool RicaricaCaricaCicliMemLunga(uint cicloStart, uint cicloEnd, MoriData._db dbCorrente, bool AggiornaTutto, bool caricaBrevi, out object EsitoCaricamento, bool RunAsinc = false )
        {
            try
            {
                bool _esito;
                object _dataRx;
                sbEndStep _esitoBg = new sbEndStep();
                sbWaitStep _stepBg = new sbWaitStep();


                //_CicliMemoriaLunga = new System.Collections.Generic.List<MessaggioSpyBatt.MemoriaPeriodoLungo>();
                Log.Debug("------  _CicliMemoriaLunga.Clear() --------");
                Log.Debug("Inizio Lettura - " + _CicliMemoriaLunga.Count.ToString() + " Cicli presenti (" + cicloEnd.ToString() + " - " + cicloStart.ToString() + ")"); 
                _CicliMemoriaLunga.Clear();
                RichiestaInterruzione = false;
                if (sbData.fwLevel > 4)
                {

                    cicloStart -= 1;
                    Log.Debug("Inizio Lettura: " + cicloStart.ToString());
                }

                string StringaLog = "";
                int ultimoCiclo = 0;
                Log.Debug("----------------------------------------------------------------------------------------------------------------------------");
                EsitoCaricamento = null;
                if (true)  
                {
                    if (RunAsinc)
                    {
                        //Preparo l'intestazione della finestra di avanzamento
                        if (Step != null)
                        {
                            sbWaitStep _passo = new sbWaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                            _passo.Titolo = "Fase 1 - Caricamento Eventi Lunghi";
                            _passo.Eventi = 1;
                            _passo.Step = -1;
                            _passo.EsecuzioneInterrotta = false;
                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);    
                            Step(this, _stepEv);
                        }

                    }

                    _mS.Comando = SerialMessage.TipoComando.SB_R_CicloLungo;
                    //_mS.ComponiMessaggio();

                    _mS.ComponiMessaggioCicloLungo(cicloStart);
                    _rxRisposta = false;
                    _timeOut = (int)(( cicloEnd - cicloStart)/2) ;
                    if (_timeOut < 10) _timeOut = 10;
                    Log.Debug("CICLI Lunga: start=" + cicloStart.ToString() + "  | Timeout: " + _timeOut.ToString());
                    Log.Debug(_mS.hexdumpMessaggio());
                    _startRead = DateTime.Now;
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                    if (_parametri.CanaleSpyBat == parametriSistema.CanaleDispositivo.USB)
                    {
                        //_esito = aspettaRisposta(elementiComuni.TimeoutBase, (int)(cicloEnd - cicloStart + 1), false,true);+1 ????
                        int _cicliAttesi = (int)(cicloEnd - cicloStart + 1);

                        if (sbData.fwLevel > 4)                                                   
                        {
                  
                            _cicliAttesi += 1;
                        }
                        _esito = aspettaRisposta(elementiComuni.TimeoutBase, out _dataRx, _cicliAttesi, false, true, elementiComuni.tipoMessaggio.MemLunga);
                    }
                    else
                    {

                        _esito = aspettaRisposta(elementiComuni.TimeoutBase);
                        do
                        {
                            if (raggiuntoTimeout(_startRead, _timeOut))
                            {
                                Log.Error("Raggiunto Timeout - Attesa Mem Lunga -  dopo elemento " + CicliMemoriaLunga.Count.ToString());
                                break;
                                //return true;
                            }
                            if (ultimoCiclo != _CicliMemoriaLunga.Count)
                            {
                                StringaLog += "<" + _CicliMemoriaLunga.Count.ToString() + ">";
                            }
                            StringaLog += ".";
                            _esito = aspettaRisposta(elementiComuni.TimeoutBase);
                            _rxRisposta = false;
                            //Log.Debug("Step Lettura - " + _CicliMemoriaLunga.Count.ToString());

                        }
                        while (_esito && _CicliMemoriaLunga.Count <= (cicloEnd - cicloStart));
                    }

 


                    Log.Debug(StringaLog);
                    Log.Debug("Fine Lettura - " + _CicliMemoriaLunga.Count.ToString() + " Cicli caricati (" + cicloStart.ToString() + " - " + cicloEnd.ToString() + ")");
                    Log.Debug("----------------------------------------------------------------------------------------------------------------------------");

                }
                DateTime _startSave = DateTime.Now;
                if( _CicliMemoriaLunga.Count > 0 )
                {
                    CicliMemoriaLunga.Clear();
                    if (RunAsinc && caricaBrevi)

                    { 
                        //Preparo l'intestazione della finestra di avanzamento
                        if (Step != null)
                        {
                            sbWaitStep _passo = new sbWaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                            _passo.Titolo = "Fase 2 - Caricamento Eventi Brevi";
                            _passo.Eventi = 1;
                            _passo.Step = -1;
                            _passo.EsecuzioneInterrotta = false;
                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                            Step(this, _stepEv);
                        }

                    }

                    uint _lastId = 0;
                     int _stepCorrente = 0;

                    foreach (MessaggioSpyBatt.MemoriaPeriodoLungo _ciclo in _CicliMemoriaLunga)
                    {
                        sbMemLunga _memLn = new sbMemLunga(dbCorrente);
                        _stepCorrente++;

                        if (RichiestaInterruzione)
                        {
                            sbWaitStep _passo = new sbWaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                            _passo.Titolo = "Fase 2 - Caricamento Eventi Brevi";
                            _passo.Eventi = 1;
                            _passo.Step = -1;
                            _passo.EsecuzioneInterrotta = true;
                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                            Step(this, _stepEv);
                            break;
                        }


                        if (_ciclo.IdEvento >= (uint)0xFFFF)
                        {
                            if(_lastId > 0)
                            {
                                // se il record è a FF e non ho uh precedente valido salto al record sucessivo
                                Log.Warn("ID non valido " + _ciclo.IdEvento.ToString("X2") + " ! " + _lastId.ToString("X2"));
                                _ciclo.IdEvento = ++_lastId;
                            }
                            else
                            {
                                // se il record è a FF e non ho uh precedente valido salto al record sucessivo
                                continue;
                            }
                        }

                        _memLn.caricaDati(_idCorrente, (uint)_ciclo.IdEvento);
                        _memLn.CaricaProgramma();
                        if ((_memLn.NumEventiBrevi != _ciclo.NumEventiBrevi || _memLn.DataOraFine != FunzioniMR.StringaTimestamp(_ciclo.DataOraFine)) || AggiornaTutto)
                        {
                            // Aggiorno la lunga e ricarico le brevi solo se cambia il N° di brevi o la data fine
                            // oppure se setto a true il flag AggiornaTutto
                            CaricaMessaggioMemLunga(_idCorrente, (uint)_ciclo.IdEvento, _ciclo,ref _memLn);

                            _lastId = _ciclo.IdEvento;
                            //  Se previsto, carico direttamente i brevi
                            if (caricaBrevi)
                            {
                                //Scarico i brevi solo se ho un puntatore valido
                                if (_memLn.PuntatorePrimoBreve < 0xFFFFFFFF)
                                {
                                    //prima avanzo il contatore lunghi
                                    sbWaitStep _passo = new sbWaitStep();
                                    int _progress = 0;
                                    double _valProgress = 0;
                                    _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                    _passo.Titolo = "";
                                    _passo.Eventi = _CicliMemoriaLunga.Count();
                                    _passo.Step = _stepCorrente;
                                    if (_passo.Eventi > 0)
                                    {
                                        _valProgress = (_passo.Step * 100) / _passo.Eventi;
                                    }
                                    _progress = (int)_valProgress;
                                    ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                    Step(this, _stepEv);

                                    // poi lancio il caricamento brevi
                                    RicaricaCaricaCicliMemBreve(_memLn);
                                }
                                else
                                {
                                    Log.Warn("Puntatore brevi non valido");
                                }

                            }


                            CicliMemoriaLunga.Add(_memLn);
                            /*if (LMListChange != null)
                            {
                                sbListaLunghiEvt eventoLM = new sbListaLunghiEvt();
                                eventoLM.EventiLunghi = CicliMemoriaLunga.Count;
                                LMListChange(this, eventoLM);
                            }*/
                            //RicaricaCaricaCicliMemBreve(_ciclo.IdEvento, (uint)_ciclo.PuntatorePrimoBreve, _ciclo.NumEventiBrevi, dbCorrente);

                        }

                    }


                //CaricaCicliMemLunga(_idCorrente, dbCorrente);

                }

                // Se previsto il caricamento dei brevi, consolido i valori intermedi
                if (caricaBrevi) ConsolidaBrevi();
                
               

                TimeSpan _tTrascorso = DateTime.Now.Subtract(_startSave);
                string _messaggio1 = "Durata Salvataggio: " + _tTrascorso.TotalSeconds.ToString("0.000");
                Log.Debug(_messaggio1);
                return true; //_esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                EsitoCaricamento = null;
                return false;
            }
        }

        /// <summary>
        /// Carico direttamente da memoria i cicli lunghi da Start a fine
        /// </summary>
        /// <param name="cicloStart">Primo ciclo da leggere</param>
        /// <param name="cicloEnd">Ultimo ciclo da leggere</param>
        /// <returns></returns>
        public bool CaricaCicliMemLungaDaMem(uint cicloStart, uint cicloEnd)
        {
            // Carico direttamente da memoria i cicli lunghi da Start a fine
            // l'indirizzo base è 0x134000; calcolo in automatico in base alla 
            // lunghezza
            try
            {
                bool _esito;
                UInt32 AddrBase = 0x134000;
                ushort LenMemLunga = 51;

                UInt32 AddrLettura;
                
                MessaggioSpyBatt.MemoriaPeriodoLungo _tempML;

                if ( cicloStart <= cicloEnd )  //_mS.CicliPresenti.NumCicli > 0)
                {

                    CicliMemoriaLunga.Clear();

                    for (uint _cicloM = cicloStart; _cicloM <= cicloEnd; _cicloM++)
                    {

                        _mS.Comando = SerialMessage.TipoComando.SB_R_LeggiMemoria;

                        AddrLettura = AddrBase + LenMemLunga * (_cicloM - 1);

                        Log.Debug("---------------------------------------------------------------------------------------------------------------");
                        Log.Debug("Lettura di " + LenMemLunga.ToString() + " bytes dall'indirizzo " + AddrLettura.ToString("X2"));

                        _mS.ComponiMessaggioLeggiMem(AddrLettura, LenMemLunga);
                        Log.Debug(_mS.hexdumpMessaggio());
                        _rxRisposta = false;
                        _startRead = DateTime.Now;
                        _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                        _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                        Log.Debug(_mS.hexdumpMessaggio());

                        //_tempML = new MessaggioSpyBatt.MemoriaPeriodoLungo();
                        //_tempML.analizzaMessaggio(_mS.messaggioRisposta);

                        MessaggioSpyBatt.MemoriaPeriodoLungo _ciclo = new MessaggioSpyBatt.MemoriaPeriodoLungo();
                        sbMemLunga _memLn = new sbMemLunga();
                        _ciclo.analizzaMessaggio(_mS._pacchettoMem.memData, sbData.fwLevel);

                        // Creo la lunga accodandola alla lista senza metterla su db
                        //e ricarico le brevi solo se cambia il N° di brevi o la data fine
                        // oppure se setto a true il flag AggiornaTutto
                        _memLn.IdMemoriaLunga = _ciclo.IdEvento;
                        _memLn.TipoEvento = _ciclo.TipoEvento;
                        _memLn.IdProgramma = _ciclo.IdProgramma;
                        _memLn.PuntatorePrimoBreve = _ciclo.PuntatorePrimoBreve;
                        _memLn.PuntatorePrimoBreveEff = _ciclo.PuntatorePrimoBreve;
                        _memLn.NumEventiBrevi = _ciclo.NumEventiBrevi;
                        _memLn.DataOraStart = StringaTimestamp(_ciclo.DataOraStart);
                        _memLn.DataOraFine = StringaTimestamp(_ciclo.DataOraFine);
                        _memLn.Durata = _ciclo.Durata;
                        _memLn.TempMin = _ciclo.TempMin;
                        _memLn.TempMax = _ciclo.TempMax;
                        _memLn.Vmin = _ciclo.Vmin;
                        _memLn.Vmax = _ciclo.Vmax;
                        _memLn.Amin = _ciclo.Amin;
                        _memLn.Amax = _ciclo.Amax;
                        _memLn.PresenzaElettrolita = _ciclo.PresenzaElettrolita;
                        _memLn.Ah = _ciclo.Ah;
                        _memLn.Wh = (int)_ciclo.Wh;
                        _memLn.CondizioneStop = _ciclo.CondizioneStop;
                        _memLn.FattoreCarica = _ciclo.FattoreCarica;
                        _memLn.StatoCarica = _ciclo.StatoCatica;
                        _memLn.TipoCariatore = _ciclo.TipoCariatore;
                        _memLn.IdCaricatore = _ciclo.IdCaricatore;

                        CicliMemoriaLunga.Add(_memLn);
                        // _memLn.salvaDati();
                    }
                    

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

        /// <summary>
        /// Rilegge direttamente da SPY-BATT l'elenco delle programmazioni, aggiornando sia la lista in memoria che i dati su DB
        /// </summary>
        /// <param name="cicloStart">Id primo ciclo da leggere</param>
        /// <param name="cicloEnd">Id ultimo ciclo da leggere - da primaliettura</param>
        /// <param name="dbCorrente">Connessione dati attiva</param>
        /// <param name="AggiornaTutto">se TRUE aggiorna anche i record già presenti</param>
        /// <returns>True se l'operazione termina senza errori</returns>
        public bool RicaricaProgrammazioni(ushort cicloStart, ushort cicloEnd, MoriData._db dbCorrente, bool AggiornaTutto)
        {
            try
            {
                bool _esito;
                int _numRecord;
                _Programmazioni = new System.Collections.Generic.List<MessaggioSpyBatt.ProgrammaRicarica>();
                _Programmazioni.Clear();
                RichiestaInterruzione = false;

                if (true)  
                {

                    _mS.Comando = SerialMessage.TipoComando.SB_R_Programmazione;
                    _mS.ComponiMessaggioLeggiProgrammazioni(cicloEnd);
                    _rxRisposta = false;
                    //cicloEnd = 23;
                    Log.Debug("Lettura Programmazioni: start=" + cicloStart.ToString());
                    Log.Debug(_mS.hexdumpMessaggio());
                    _Programmazioni.Clear();
                    _timeOut = 18; // aspetto 23 record
                    _numRecord = cicloEnd;
                    if (_numRecord > 23) _numRecord = 23;
                    _startRead = DateTime.Now;
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    if (_parametri.CanaleSpyBat == parametriSistema.CanaleDispositivo.USB)
                    {
                        _esito = aspettaRisposta(elementiComuni.TimeoutBase, _numRecord, false);
                    }
                    else
                    {
                        _esito = aspettaRisposta(elementiComuni.TimeoutBase, _numRecord, false);
                        do
                        {
                            if (raggiuntoTimeout(_startRead, _timeOut))
                            {
                                Log.Error("Raggiunto Timeout - Attesa Programmazioni -  dopo elemento " + _Programmazioni.Count.ToString());
                                break;
                                //return true;
                            }
                            _esito = aspettaRisposta(elementiComuni.TimeoutBase);
                            //Log.Debug("Step Lettura - " + _CicliMemoriaLunga.Count.ToString());

                        }
                        while (_esito & _Programmazioni.Count < _numRecord); // 23 record fissi (cicloEnd - cicloStart));
                    }
                    Log.Debug("Fine Lettura - " + _Programmazioni.Count.ToString() + "Cicli " + cicloEnd.ToString() + " - " + cicloStart.ToString());

                }

                if (_Programmazioni.Count > 0)
                {
                    Programmazioni.Clear();

                    foreach (MessaggioSpyBatt.ProgrammaRicarica _ciclo in _Programmazioni)
                    {
                        sbProgrammaRicarica _progR = new sbProgrammaRicarica(dbCorrente);
                        _progR.caricaDati(_idCorrente, _ciclo.IdProgramma);
                        if ((_progR.BatteryVdef != _ciclo.BatteryVdef) | AggiornaTutto)
                        {

                            if (_ciclo.IdProgramma != 0xFFFF)
                            {
                                // Aggiorno la lunga e ricarico le brevi solo se cambia il N° di brevi o la data fine
                                // oppure se setto a true il flag AggiornaTutto
                                _progR.IdProgramma = _ciclo.IdProgramma;
                                _progR.DataInstallazione = _ciclo.DataInstallazione;
                                _progR.BatteryType = _ciclo.BatteryType;
                                _progR.BatteryVdef = _ciclo.BatteryVdef;
                                _progR.BatteryAhdef = _ciclo.BatteryAhdef;
                                _progR.BatteryCells = _ciclo.BatteryCells;
                                _progR.BatteryCell1 = _ciclo.BatteryCell1;
                                _progR.BatteryCell2 = _ciclo.BatteryCell2;
                                _progR.BatteryCell3 = _ciclo.BatteryCell3;
                                _progR.salvaDati();

                                Programmazioni.Add(_progR);
                                Log.Debug("Accodato Programma " + _ciclo.IdProgramma.ToString());

                            }
                            else
                            {
                                Log.Debug("Saltato Programma " + _ciclo.IdProgramma.ToString() + " (" + _Programmazioni.IndexOf(_ciclo) + " )");
                            }

                        }

                    }

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

        /// <summary>
        /// Carico direttamente da memoria l'area passata come parametro
        /// </summary>
        /// <param name="StartAddr">Indirizzo (iniziale) del blocco da leggere</param>
        /// <param name="NumByte">Numero di byte da leggere (max 242)</param>
        /// <param name="Dati">bytearray dati letti</param>
        /// <returns></returns>
        public bool LeggiBloccoMemoria(uint StartAddr, ushort NumByte, out byte[] Dati )
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


                _mS.Comando = SerialMessage.TipoComando.SB_R_LeggiMemoria;
                _mS._pacchettoMem = new MessaggioSpyBatt.PacchettoReadMem();

                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Lettura di " + NumByte.ToString() + " bytes dall'indirizzo " + StartAddr.ToString("X2"));

                _mS.ComponiMessaggioLeggiMem(StartAddr, NumByte);
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
               // Log.Debug(_mS.hexdumpMessaggio());
                Log.Debug(_mS.hexdumpArray( _mS._pacchettoMem.memDataDecoded));

                for ( int _ciclo = 0; ((_ciclo<NumByte) && (_ciclo < _mS._pacchettoMem.numBytes )); _ciclo++)
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
                bool _esito;

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
                else
                {
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false,false,modoDeso);

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

                _mS.Comando = SerialMessage.TipoComando.SB_CancellaInteraMemoria;


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
                Application.DoEvents();


                if (_esito)
                {


                    sbData.cancellaDati(_idCorrente);
                }
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


        /// <summary>
        /// Forza il riavvio della scheda
        /// </summary>
        /// <returns></returns>
        public bool ResetScheda()
        {


            try
            {
                bool _esito;

                _mS.Comando = SerialMessage.TipoComando.SB_W_RESETSCHEDA;


                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug(" RESET SCHEDA ");

                _mS.ComponiMessaggio();
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
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


        // -------------------------------
        // strategia

        public bool LanciaComandoTestStrategia(byte ComandoStrategia, out byte[] Dati)
        {


            try
            {
                bool _esito;



                Dati = new byte[252];


                _mS.Comando = SerialMessage.TipoComando.SB_W_chgst_Call;
                _mS.ComandoStrat = new MessaggioSpyBatt.ComandoStrategia();

                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Lancio comando SB_W_chgst_Call -  " + ComandoStrategia.ToString("X2"));

                _mS.ComponiMessaggioTestStrategia(ComandoStrategia);
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                // Log.Debug(_mS.hexdumpMessaggio());
                Log.Debug(_mS.hexdumpArray(_mS.ComandoStrat.memDataDecoded));

                int _totDati = _mS.ComandoStrat.numBytes ;
                Dati = new byte[_totDati];

                for (int _ciclo = 0; (_ciclo < _totDati) ; _ciclo++)
                {

                    Dati[_ciclo] = _mS.ComandoStrat.memDataDecoded[_ciclo];
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

        public bool LanciaComandoStrategia(byte Modo, ushort Vmin, ushort Vmax,ushort Imax, byte Rabb, byte FC, out byte[] Dati)
        {


            try
            {
                bool _esito;
                byte msb = 0;
                byte lsb = 0;

                byte[] _cmdStrat = new byte[10];

                Dati = new byte[252];


                _mS.Comando = SerialMessage.TipoComando.SB_W_chgst_Call;
                _mS.ComandoStrat = new MessaggioSpyBatt.ComandoStrategia();


                _cmdStrat[0] = Modo;  //CMD_IS
                _cmdStrat[1] = 0x0B;  // len

                FunzioniComuni.SplitUshort(Vmin, ref lsb, ref msb);
                _cmdStrat[2] = msb;
                _cmdStrat[3] = lsb;
                FunzioniComuni.SplitUshort(Vmax, ref lsb, ref msb);
                _cmdStrat[4] = msb;
                _cmdStrat[5] = lsb;
                FunzioniComuni.SplitUshort(Imax, ref lsb, ref msb);
                _cmdStrat[6] = msb;
                _cmdStrat[7] = lsb;

                _cmdStrat[8] = Rabb;
                _cmdStrat[9] = FC;

                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Lancio comando base SB_W_chgst_Call -  ");

                _mS.ComponiMessaggioBaseStrategia(_cmdStrat);
                Log.Debug(FunzioniComuni.HexdumpArray(_cmdStrat));
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                // Log.Debug(_mS.hexdumpMessaggio());
                Log.Debug(_mS.hexdumpArray(_mS.ComandoStrat.memDataDecoded));

                int _totDati = _mS.ComandoStrat.numBytes;
                Dati = new byte[_totDati];

                for (int _ciclo = 0; (_ciclo < _totDati); _ciclo++)
                {

                    Dati[_ciclo] = _mS.ComandoStrat.memDataDecoded[_ciclo];
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

        public bool ComandoStrategiaAggiornaContatori(ushort Capacity,ushort Dschg, ushort Chg, out byte[] Dati)
        {


            try
            {
                bool _esito;
                byte msb = 0;
                byte lsb = 0;

                byte[] _cmdStrat = new byte[8];

                Dati = new byte[252];


                _mS.Comando = SerialMessage.TipoComando.SB_W_chgst_Call;
                _mS.ComandoStrat = new MessaggioSpyBatt.ComandoStrategia();

                //_cmdStrat[0] = 0x80;
                _cmdStrat[0] = 0x51;
                _cmdStrat[1] = 0x08;
                //Capacity
                FunzioniComuni.SplitUshort(Capacity, ref lsb, ref msb);
                _cmdStrat[2] = msb;
                _cmdStrat[3] = lsb;
                //Scarica
                FunzioniComuni.SplitUshort(Dschg, ref lsb, ref msb);
                _cmdStrat[4] = msb;
                _cmdStrat[5] = lsb;
                //Carica
                FunzioniComuni.SplitUshort(Chg, ref lsb, ref msb);
                _cmdStrat[6] = msb;
                _cmdStrat[7] = lsb;


                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Lancio comando base SB_W_chgst_Call - CMD_WRCHG ");

                _mS.ComponiMessaggioOpenStrategia(_cmdStrat);
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                // Log.Debug(_mS.hexdumpMessaggio());
                Log.Debug(_mS.hexdumpArray(_mS.ComandoStrat.memDataDecoded));

                int _totDati = _mS.ComandoStrat.numBytes;
                Dati = new byte[_totDati];

                for (int _ciclo = 0; (_ciclo < _totDati); _ciclo++)
                {

                    Dati[_ciclo] = _mS.ComandoStrat.memDataDecoded[_ciclo];
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

        public bool ComandoInfoStrategia(byte ComandoStrategia, out byte[] Dati)
        {


            try
            {
                bool _esito;



                Dati = new byte[252];


                _mS.Comando = SerialMessage.TipoComando.SB_W_chgst_Call;
                _mS.ComandoStrat = new MessaggioSpyBatt.ComandoStrategia();

                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Lancio comando SB_W_chgst_Call -  " + ComandoStrategia.ToString("X2"));

                _mS.ComponiMessaggioTestStrategia(ComandoStrategia);
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                // Log.Debug(_mS.hexdumpMessaggio());
                Log.Debug(_mS.hexdumpArray(_mS.ComandoStrat.memDataDecoded));

                int _totDati = _mS.ComandoStrat.numBytes;
                Dati = new byte[_totDati];

                for (int _ciclo = 0; (_ciclo < _totDati); _ciclo++)
                {

                    Dati[_ciclo] = _mS.ComandoStrat.memDataDecoded[_ciclo];
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
        /// Entra / Esce dallo stato CALIBRAZIONE
        /// </summary>
        /// <returns></returns>
        public bool ModalitaCalibrazione()
        {


            try
            {
                bool _esito;

                _mS.Comando = SerialMessage.TipoComando.SB_Cal_Enable;


                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("SerialMessage.TipoComando.SB_Cal_Enable");

                _mS.ComponiMessaggio();
                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);

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


        public string StringaTensione(uint Tensione)
        {
            try
            {
                string _tensioni = "";
                float _inVolt;
                _inVolt = Tensione / 100;
                _tensioni = _inVolt.ToString("0.0");
                return _tensioni;
            }
            catch (Exception Ex)
            {
                return "";
            }
        }

        public string StringaCorrente(short Corrente)
        {
            try
            {
                string _correnti = "";
                float _inAmpere;
                _inAmpere = Corrente / 10;
                _correnti = _inAmpere.ToString("0");
                return _correnti;
            }
            catch
            {
                return "";
            }
        }

        public string StringaTimestamp(byte[] Dataora )
        {
            try
            {
                if (Dataora == null)
                    return "N.D.";
                string _timestamp = "";
                _timestamp += Dataora[0].ToString("00");
                _timestamp += "/" + Dataora[1].ToString("00");
                _timestamp += "/" + Dataora[2].ToString("00");
                _timestamp += "  " + Dataora[3].ToString("00");
                _timestamp += ":" + Dataora[4].ToString("00");
                return _timestamp;
            }
            catch
            {
                return "";
            }
        }

        public string StringaDurata(uint Secondi, bool BlankIfZero = false )
        {
            try
            {
                string _tempo = "";
                if (Secondi < 60 & BlankIfZero)
                {
                    return "";
                }
                
                TimeSpan t = TimeSpan.FromSeconds(Secondi);
                if (Secondi > 86400)
                {
                    // Se la durata è superiore ad 1 giorno ( 86400 secondi ) mostro anche i giorni
                    _tempo = string.Format("{0}g {1:D2}:{2:D2}",t.Days, t.Hours, t.Minutes);
                }
                else
                {
                    _tempo = string.Format("{0:D2}:{1:D2}", t.Hours, t.Minutes);
                }
                //_tempo += "  / " + Secondi.ToString();
                return _tempo;
            }
            catch
            {
                return "";
            }
        }

        public string StringaPresenza(byte Valore)
        {
            try
            {
                string _Flag = "";
                switch (Valore)
                {
                    case 0xF0:
                        _Flag = "OK";
                        break;
                    case 0x0F:
                        _Flag = "KO";
                        break;
                    default:
                        _Flag = Valore.ToString("x2");
                        break;
                }

                return _Flag;
            }
            catch
            {
                return "";
            }
        }

        public string StringaTipoEvento(int Valore)
        {
            try
            {
                string _Flag = "";
                switch (Valore)
                {
                    case 0xF0:
                        _Flag = PannelloCharger.StringheComuni.Carica; // "Carica";
                        break;
                    case 0x0F:
                        _Flag = PannelloCharger.StringheComuni.Scarica; //"Scarica";
                        break;
                    case 0xAA:
                        _Flag = PannelloCharger.StringheComuni.Pausa; //"Pausa";
                        break;
                    default:
                        _Flag = PannelloCharger.StringheComuni.EventoAnomalo +  "(" + Valore.ToString("x2") + ")";
                        break;
                }

                return _Flag;
            }
            catch
            {
                return "";
            }
        }

        public bool RicaricaCaricaCicliMemBreve(UInt32 IdCicloLungo,UInt32 PtrPrimoBreve,ushort Pacchetti,MoriData._db dbCorrente)
        {
            try
            {
                int _cicloLimite = 0;
                bool _esito;
               // _CicliMemoriaBreve = new System.Collections.Generic.List<MessaggioSpyBatt.MemoriaPeriodoBreve>();
                _CicliMemoriaBreve.Clear();
                RichiestaInterruzione = false;

                if (Pacchetti > 0)  //_mS.CicliPresenti.NumCicli > 0)
                {
                    
                    _mS.Comando = SerialMessage.TipoComando.SB_R_CicloBreve;
                    _mS.ComponiMessaggioCicloBreve(IdCicloLungo, PtrPrimoBreve, Pacchetti);
                    _rxRisposta = false;
                    Log.Warn("CICLI Breve: START");
                    Log.Debug(_mS.hexdumpMessaggio());

                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    //_esito = aspettaRisposta(elementiComuni.TimeoutBase);

                    if (_parametri.CanaleSpyBat == parametriSistema.CanaleDispositivo.USB)
                    {
                        _esito = aspettaRisposta(elementiComuni.TimeoutBase, Pacchetti, false);
                    }
                    else
                    {
                        do
                        {
                            _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                            System.Threading.Thread.Sleep(10);
                            _cicloLimite++;
                        }
                        while (!(_esito & _CicliMemoriaBreve.Count >= (Pacchetti)
                                && UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk)
                                | _cicloLimite > 10000);
                    }
                    Log.Warn("CICLI Breve: Fine lettura da scheda");

                }
                if (UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk  & _cicloLimite < 10000 )
                {  
                    // ho scaricato i pacchetti, li salvo sul db
                    Log.Debug("----------------------------------------------------------------------------------------");
                    Log.Debug("Caricati gli eventi a breve (" + _CicliMemoriaBreve.Count.ToString() + ") aggiorno il DB");
                    Log.Debug("----------------------------------------------------------------------------------------");
                    int _numCiclo = 1;
                    foreach( MessaggioSpyBatt.MemoriaPeriodoBreve _ciclo in _CicliMemoriaBreve )
                    {
                    sbMemBreve _memBr = new sbMemBreve(dbCorrente);
                    _memBr.caricaDati(_idCorrente,(int)IdCicloLungo, _numCiclo );
                    if (_memBr.DataOraRegistrazione != StringaTimestamp(_ciclo.DataOraRegistrazione))
                    {
                        // Aggiorno la lunga e ricarico le brevi solo se cambia il N° di brevi o la data fine
                        _memBr.IdApparato = _idCorrente;
                        _memBr.IdMemoriaLunga = (int)IdCicloLungo;
                        _memBr.IdMemoriaBreve = _numCiclo;
                        _memBr.DataOraRegistrazione = StringaTimestamp(_ciclo.DataOraRegistrazione);
                        _memBr.Vreg = _ciclo.Vreg;
                        _memBr.V1 = _ciclo.V1;
                        _memBr.V2 = _ciclo.V2;
                        _memBr.V3 = _ciclo.V3;
                        _memBr.Amed = _ciclo.Amed;
                        _memBr.Amin = _ciclo.Amin;
                        _memBr.Amax = _ciclo.Amax;
                        _memBr.Tntc = _ciclo.Tntc;
                        _memBr.PresenzaElettrolita = _ciclo.PresenzaElettrolita;
                        _memBr.VbatBk = _ciclo.VbatBk;
                        //_memBr.salvaDati();
                        _numCiclo++;
                        //_memLn.RicaricaBrevi( ref _mS;);
                    }

                }
                    Log.Warn("CICLI Breve: Fine Salvataggio");

                    return true ;
                }
                else
                {
                    return false;
                }; //_esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Caricamento da scheda dei cicli brevi e salvataggio su DB
        /// Variamte 1: passo come parametro il ciclo lungo di riferimento per fare il salvataggio in pacchetto singolo
        /// </summary>
        /// <param name="IdCicloLungo"></param>
        /// <param name="PtrPrimoBreve"></param>
        /// <param name="Pacchetti"></param>
        /// <param name="dbCorrente"></param>
        /// <returns></returns>
        public bool RicaricaCaricaCicliMemBreve(sbMemLunga CicloLungo)
        {
            try
            {
                // _sb.RicaricaCaricaCicliMemBreve((uint)_memLunga.IdMemoriaLunga, (uint)_memLunga.PuntatorePrimoBreve, (ushort)_memLunga.NumEventiBrevi, _logiche.dbDati.connessione))
 
                int _cicloLimite = 0;
                bool _esito;
                bool _esitoCiclo;

                UInt32 IdCicloLungo = (uint) CicloLungo.IdMemoriaLunga;
                UInt32 PtrPrimoBreve = (uint) CicloLungo.PuntatorePrimoBreve;
                ushort Pacchetti = (ushort) CicloLungo.NumEventiBrevi;
                MoriData._db dbCorrente = CicloLungo._database;
                if (CicloLungo.ProgrammaAttivo == null)
                    CicloLungo.CaricaProgramma();
                _CicliMemoriaBreve.Clear();
                CicloLungo.CicliMemoriaBreve.Clear();

                _esito = false;
                Log.Warn("CICLI Breve: Pacchetti " + Pacchetti.ToString());
                if (Pacchetti > 0)  //_mS.CicliPresenti.NumCicli > 0)
                {

                    _mS.Comando = SerialMessage.TipoComando.SB_R_CicloBreve;
                    _mS.ComponiMessaggioCicloBreve(IdCicloLungo, PtrPrimoBreve, Pacchetti);
                    _rxRisposta = false;
                    Log.Warn("CICLI Breve: START");
                    Log.Debug(_mS.hexdumpMessaggio());

                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                    if (_parametri.CanaleSpyBat == parametriSistema.CanaleDispositivo.USB)
                    {
                        object _risposta;
                        _esito = aspettaRisposta(elementiComuni.TimeoutBase, out _risposta, Pacchetti, false, true, elementiComuni.tipoMessaggio.MemBreve);
                    }
                    else
                    {
                        _esito = aspettaRisposta(elementiComuni.TimeoutBase);
                        do
                        {
                            _esitoCiclo = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                            System.Threading.Thread.Sleep(10);
                            _cicloLimite++;
                        }
                        while (!(_esitoCiclo & _CicliMemoriaBreve.Count >= (Pacchetti)
                                && UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk)
                                | _cicloLimite > 10000);

                        _esito = _esitoCiclo 
                                && _CicliMemoriaBreve.Count >= (Pacchetti)
                                && UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk 
                                && _cicloLimite > 10000;

                    }
                    Log.Warn("CICLI Breve: Fine lettura da scheda " + _CicliMemoriaBreve.Count.ToString());

                }



                if (_esito == true)
                {
                    // ho scaricato i pacchetti, li salvo sul db
                    Log.Debug("----------------------------------------------------------------------------------------");
                    Log.Debug("Caricati gli eventi a breve (" + _CicliMemoriaBreve.Count.ToString() + ") aggiorno il DB");
                    Log.Debug("----------------------------------------------------------------------------------------");
                    CicloLungo.CancellaBrevi();
                    int _numCiclo = 1;
                    foreach (MessaggioSpyBatt.MemoriaPeriodoBreve _ciclo in _CicliMemoriaBreve)
                    {
                        sbMemBreve _memBr = new sbMemBreve(dbCorrente);
                        //_memBr.caricaDati(_idCorrente, (int)IdCicloLungo, _numCiclo);

                        if (_memBr.DataOraRegistrazione != StringaTimestamp(_ciclo.DataOraRegistrazione))
                        {
                            // Aggiorno la lunga e ricarico le brevi solo se cambia il N° di brevi o la data fine
                            _memBr.IdApparato = _idCorrente;
                            _memBr.IdMemoriaLunga = (int)IdCicloLungo;
                            _memBr.IdMemoriaBreve = _numCiclo;
                            _memBr.DataOraRegistrazione = StringaTimestamp(_ciclo.DataOraRegistrazione);
                            _memBr.Vreg = _ciclo.Vreg;
                            _memBr.V1 = _ciclo.V1;
                            _memBr.V2 = _ciclo.V2;
                            _memBr.V3 = _ciclo.V3;
                            _memBr.Amed = _ciclo.Amed;
                            _memBr.Amin = _ciclo.Amin;
                            _memBr.Amax = _ciclo.Amax;
                            _memBr.Tntc = _ciclo.Tntc;
                            _memBr.PresenzaElettrolita = _ciclo.PresenzaElettrolita;
                            _memBr.VbatBk = _ciclo.VbatBk;
                            _numCiclo++;
                        }
                        CicloLungo.CicliMemoriaBreve.Add(_memBr);
                    }
                    CicloLungo.CalcolaIntermediBrevi(true);
                    CicloLungo.SalvaBrevi();
                    CicloLungo.salvaDati();

                    Log.Warn("CICLI Breve: Fine Salvataggio");

                    return true;
                }
                else
                {
                    return false;
                }; //_esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        public bool ConsolidaBrevi()
        {
            try
            {
                foreach (sbMemLunga CicloLungo in CicliMemoriaLunga)
                {
                    CicloLungo.CalcolaIntermediBrevi(true);
                    CicloLungo.SalvaBrevi();
                    CicloLungo.salvaDati();
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

        /// <summary>
        /// Legge data e ora dall'RTC sulla scheda.
        /// </summary>
        /// <returns></returns>
        public bool LeggiOrologio()
        {
            try
            {
                bool _esito;

                _mS.Comando = SerialMessage.TipoComando.SB_ReadRTC;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("Leggi SB RTC");
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                _esito = aspettaRisposta(elementiComuni.TimeoutBase);
                OrologioSistema = _mS.DatiRTCSB;

                //    _mS.Comando = SerialMessage.TipoComando.Stop;
                //    _mS.ComponiMessaggio();
                //    _rxRisposta = false;
                //    _parametri.serialeCorrente.Write(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);


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

                _mS.Comando = SerialMessage.TipoComando.SB_UpdateRTC;
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
                Log.Debug("Scrivi SB RTC");
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
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

        public bool ForzaOrologio(int Giorno, int Mese, int Anno)
        {
            try
            {
                bool _esito;
                DateTime _now = DateTime.Now;

                DateTime dateValue = new DateTime(Anno, Mese, Giorno);
                _mS.Comando = SerialMessage.TipoComando.SB_UpdateRTC;
                _mS.DatiRTC = new SerialMessage.comandoRTC();
                _mS.DatiRTC.anno = (ushort)Anno;
                _mS.DatiRTC.mese = (byte)Mese;
                _mS.DatiRTC.giorno = (byte)Giorno;
                _mS.DatiRTC.giornoSett = (byte)dateValue.DayOfWeek;
                _mS.DatiRTC.ore = (byte)_now.Hour;
                _mS.DatiRTC.minuti = (byte)_now.Minute;
                _mS.DatiRTC.secondi = (byte)_now.Second;

                _mS.ComponiMessaggioOra();
                _rxRisposta = false;
                Log.Debug("Scrivi SB RTC");
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
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

        public bool LanciaComandoStrategia()
        {
            try
            {
                bool _esito;
                DateTime _now = DateTime.Now;

                _mS.Comando = SerialMessage.TipoComando.SB_UpdateRTC;
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
                Log.Debug("Scrivi SB RTC");
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
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

        public bool ScriviParametroCal(byte IdParametro, ushort ValoreCalibrazione)
        {
            try
            {
                bool _esito;
                DateTime _now = DateTime.Now;

                _mS.Comando = SerialMessage.TipoComando.SB_Cal_InvioDato;
                _mS.ComponiMsgScriviParCalibrazione( IdParametro,  ValoreCalibrazione);
                _rxRisposta = false;
                Log.Debug("Scrivi SB Calibrazione: " + IdParametro.ToString() + " - " + ValoreCalibrazione.ToString());
                Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));

                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
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

        public bool ScriviDatiCliente( )
        {
            try
            {
                bool _esito;

                Log.Debug("Scrivi SB Cliente ");
                Log.Debug("---------------------------------------------------------------------------------------------------------------");
 
                _rxRisposta = false;

            
                _mS.CustomerData.Client = sbCliente.Client.ToString();
                if (sbCliente.ClientNote == null) sbCliente.ClientNote = "";
                _mS.CustomerData.ClientNote = sbCliente.ClientNote.ToString();
                _mS.CustomerData.BatteryBrand = sbCliente.BatteryBrand.ToString();
                _mS.CustomerData.BatteryId = sbCliente.BatteryId.ToString();
                _mS.CustomerData.BatteryModel = sbCliente.BatteryModel.ToString();
                _mS.CustomerData.CicliAttesi = (ushort)sbCliente.CicliAttesi;
                _mS.CustomerData.SerialNumber = sbCliente.SerialNumber.ToString();
                _mS.CustomerData.ModoPianificazione = sbCliente.ModoPianificazione;
                _mS.CustomerData.ModoBiberonaggio = sbCliente.ModoBiberonaggio;
                _mS.CustomerData.ModoRabboccatore = sbCliente.ModoRabboccatore;
                _mS.CustomerData.ModelloPianificazione = sbCliente.MappaTurni;

                _mS.Dispositivo = SerialMessage.TipoDispositivo.Charger;
                _mS.Comando = SerialMessage.TipoComando.SB_W_DatiCliente;

                _mS.ComponiMessaggioCliente(0);
                Log.Debug("Scrivi SB Cliente - 0 : testata ");
                Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                if (_esito & UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk)
                {
                    _mS.ComponiMessaggioCliente(1);
                    Log.Debug("Scrivi SB Cliente - 1 ");
                    //Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                    _rxRisposta = false;
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutLungo, 0, true);
                    // Operazione complessa, deve spostare i 4K, richiede almeno 1500 ms  --> uso il timeout lungo.

                    if (_esito & UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk)
                    {
                        _mS.ComponiMessaggioCliente(2);
                        Log.Debug("Scrivi SB Cliente - 2 ");
                        _rxRisposta = false;
                        _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                        _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                        if (_esito & UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk)
                        {
                            _mS.ComponiMessaggioCliente(3);
                            Log.Debug("Scrivi SB Cliente - 3 ");
                            _rxRisposta = false;
                            _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                            _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);

                            if (_esito & UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk)
                            {
                                _mS.ComponiMessaggioCliente(4);
                                Log.Debug("Scrivi SB Cliente - 4 ");
                                _rxRisposta = false;
                                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                            }
                        }
                    }
                }
                return (bool)(UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk); //_esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

        /// <summary>
        /// imposta il memprogrammed e attende l'Ok scrittura dati
        /// </summary>
        /// <returns></returns>
        public bool AttivaProgramma()
        {
            try
            {
                bool _esito;

                Log.Debug("Sb imposta MemProgrammed ");

                _rxRisposta = false;
                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.SB_W_MemProgrammed;

                _mS.ComponiMessaggio();
                 Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, true);


                if (_mS.EsitoComando.CodiceEvento == (byte)SerialMessage.TipoComando.SB_W_Programmazione)
                {
                    Log.Debug(" --------------   MemProgrammed ATTIVATO -------------");
                    return true;
                }
                else
                    return false;
            }

            catch (Exception Ex)
            {
                Log.Error("AttivaProgramma: " + Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

        public bool ScriviProgramma( sbProgrammaRicarica NuovoProgramma, bool MemProgrammed)
        {
            try
            {
                bool _esito;

                Log.Debug("Scrivi Programma ");
                Log.Debug("---------------------------------------------------------------------------------------------------------------");

                _mS.ProgRicarica = new MessaggioSpyBatt.ProgrammaRicarica();

                _rxRisposta = false;

                _mS.ProgRicarica.IdProgramma = NuovoProgramma.IdProgramma;
                _mS.ProgRicarica.BatteryVdef = NuovoProgramma.BatteryVdef;
                _mS.ProgRicarica.BatteryAhdef = NuovoProgramma.BatteryAhdef;
                _mS.ProgRicarica.BatteryType = NuovoProgramma.BatteryType;
                _mS.ProgRicarica.BatteryCells = NuovoProgramma.BatteryCells;
                _mS.ProgRicarica.BatteryCell3 = NuovoProgramma.BatteryCell3;
                _mS.ProgRicarica.BatteryCell2 = NuovoProgramma.BatteryCell2;
                _mS.ProgRicarica.BatteryCell1 = NuovoProgramma.BatteryCell1;
                _mS.ProgRicarica.DataInstallazione = NuovoProgramma.DataInstallazione;
                _mS.ProgRicarica.AbilitaPresElett = NuovoProgramma.AbilitaPresElett;
                _mS.ProgRicarica.TempMin = NuovoProgramma.TempMin;
                _mS.ProgRicarica.TempMax = NuovoProgramma.TempMax;
                _mS.ProgRicarica.VersoCorrente = NuovoProgramma.VersoCorrente;
                _mS.ProgRicarica.NumeroSpire = NuovoProgramma.NumeroSpire;

                //----------  Parametri PRO

                _mS.ProgRicarica.ModoPianificazione = NuovoProgramma.ModoPianificazione;
                _mS.ProgRicarica.CorrenteCaricaMin = NuovoProgramma.CorrenteMinimaCHG;
                _mS.ProgRicarica.CorrenteCaricaMax = NuovoProgramma.CorrenteMassimaCHG;
                _mS.ProgRicarica.PulseRabboccatore = NuovoProgramma.ImpulsiRabboccatore;
                _mS.ProgRicarica.FlagBiberonaggio = NuovoProgramma.Biberonaggio;
                _mS.ProgRicarica.CoeffBiberonaggio = NuovoProgramma.FattorBiberonaggio ;
                _mS.ProgRicarica.TempAttenzione = NuovoProgramma.TempAttenzione;
                _mS.ProgRicarica.TempAllarme = NuovoProgramma.TempAllarme;
                _mS.ProgRicarica.TempRipresa = NuovoProgramma.TempRipresa;
                _mS.ProgRicarica.MaxSbilanciamento = NuovoProgramma.MaxSbilanciamento;
                _mS.ProgRicarica.TempoSbilanciamento = NuovoProgramma.DurataSbilanciamento;
                _mS.ProgRicarica.TensioneGas = NuovoProgramma.TensioneGas;
                _mS.ProgRicarica.DerivaInferiore = NuovoProgramma.DerivaInferiore;
                _mS.ProgRicarica.DerivaSuperiore = NuovoProgramma.DerivaSuperiore;
                _mS.ProgRicarica.MinCorrenteW = NuovoProgramma.MinCorrenteW;
                _mS.ProgRicarica.MaxCorrenteW = NuovoProgramma.MaxCorrenteW;
                _mS.ProgRicarica.TensioneRaccordo = NuovoProgramma.TensioneRaccordo;
                _mS.ProgRicarica.TensioneFinale = NuovoProgramma.TensioneFinale;

                _mS.ProgRicarica.EsitoScrittura = 0x00;
                _mS.Dispositivo = SerialMessage.TipoDispositivo.Charger;
                _mS.Comando = SerialMessage.TipoComando.SB_W_Programmazione;

                _mS.ComponiMessaggioNuovoProgramma();
                Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutLungo, 0, true);

               // if(_esito)
               // {
               //    if(_mS.ProgRicarica.EsitoScrittura == (byte) SerialMessage.LadeLightBool.True)
               //     { return true; }
               //     else
               //     { return false; }
               // }

                

                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("ScriviProgramma: " + Ex.Message);
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

        private  void port_DataReceivedSb(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //string testom = "Dati Ricevuti SB: " + serialeApparato.BytesToRead.ToString();
                bool _trovatoETX = false;
                byte[] data = new byte[serialeApparato.BytesToRead];
                serialeApparato.Read(data, 0, data.Length);
                Log.Debug("Dati Ricevuti SB " + data.Length.ToString());
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
                    analizzaCodaSB();
                }
            }
            catch (Exception Ex)
            {
                Log.Error("port_DataReceivedSb: " + Ex.Message);
            }

        }

        private  void usb_DataReceivedSb()
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

                ftStatus = _parametri.usbSpyBatt.GetRxBytesAvailable(ref numBytesAvailable);
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
                    ftStatus = _parametri.usbSpyBatt.Read(readData, numBytesAvailable, ref numBytesRead);
                    if (ftStatus != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        Log.Warn("Failed to read data (error " + ftStatus.ToString() + ")");
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
                        analizzaCodaSB();
                    }

                }

            }
            catch (Exception Ex)
            {
                Log.Error("USB_DataReceivedSb: " + Ex.Message);
            }

        }

        public bool LeggiParametriLettura()
        {
            try
            {
                bool _esito;

                _mS.Comando = SerialMessage.TipoComando.SB_R_ParametriLettura;
                _mS.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("Leggi SB Parametri Lettura");
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                _esito = aspettaRisposta(elementiComuni.TimeoutBase);

                // Carico i valori nel record parametri


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

        public bool ScriviParametriLettura(ushort LettureCorrente, ushort LettureTensione, ushort DurataPausa )
        {
            try
            {
                bool _esito;
                DateTime _now = DateTime.Now;

                _mS.Comando = SerialMessage.TipoComando.SB_W_ParametriLettura;

                _mS.ComponiMessaggioScriviParametriLettura(LettureCorrente, LettureTensione, DurataPausa);
               _rxRisposta = false;
                Log.Debug("Scrivi Parametri lettura");
                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
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










        /// <summary>
        /// In base al messaggio ricevuto (codice comando) definisce l'azione e l'eventuale risposta
        /// </summary>
        /// <returns></returns>
        private SerialMessage.TipoRisposta analizzaCodaSB()
        {

            SerialMessage.EsitoRisposta _esito;
            bool _trovatoSTX = false;
            byte _tempByte;
            string testom = "";
            bool _inviaRisposta = true ;
            SerialMessage.TipoRisposta _datiRicevuti = SerialMessage.TipoRisposta.NonValido;

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
                        Log.Debug("Trovato ETX SB");
                        readingMessage = false;

                        _mS.MessageBuffer = _dataBuffer;
                        //-----------------------------------------------------------------------------------------
                        // Analizzo il contenuto del messaggio 
                        //-----------------------------------------------------------------------------------------
                        _esito = _mS.analizzaMessaggio(_dataBuffer, sbData.fwLevel, skipHead);
                        UltimaRisposta = _esito; // SerialMessage.EsitoRisposta.MessaggioOk;
                        //-----------------------------------------------------------------------------------------

                        _inviaRisposta = true;
                        Log.Debug("Comando: --> 0x" + _mS._comando.ToString("X2"));
                       switch (_mS._comando)
                        {
                           case (byte) SerialMessage.TipoComando.SB_ACK:  //0x6C: // ACK
                                Log.Debug("Comando Ricevuto");
                                _datiRicevuti = SerialMessage.TipoRisposta.Ack;
                                TipoRisposta = 1;
                                _inviaRisposta = false;

                                break;
                           case (byte) SerialMessage.TipoComando.SB_ACK_PKG:  // 0x6D: // ACK Pacchetto
                                Log.Debug("Esito Comando Ricevuto");
                                //_datiRicevuti = SerialMessage.TipoRisposta.Ack;   ???????????????
                                // 16/07/15 il messaggio 0x0D inviato dopo comando che prevede la scrittura su memoria flash esterna per indicare l'esito;
                                //          richiede ACK con due eccezioni: 
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                TipoRisposta = 1;
                                _inviaRisposta = true;
                                switch (_mS.EsitoComando.CodiceEvento)
                                {
                                    case (byte)SerialMessage.TipoComando.SB_W_DatiCliente:
                                        {
                                            _inviaRisposta = false;
                                            break;
                                        }
                                    case (byte)SerialMessage.TipoComando.SB_W_ScriviMemoria:
                                        {
                                            _inviaRisposta = false;
                                            break;
                                        }

                                    default:
                                        {
                                            _inviaRisposta = true;
                                            break;
                                        }
                                }

                                break;
                           case (byte)SerialMessage.TipoComando.SB_NACK:  //0x71: //NAK
                                TipoRisposta = 2;
                                UltimaRisposta = SerialMessage.EsitoRisposta.NonRiconosciuto;
                                _datiRicevuti = SerialMessage.TipoRisposta.Nack;
                                Log.Debug("Comando Errato: Ricevuto NAK");
                                 _inviaRisposta = false;
                                 break;

                           case (byte)SerialMessage.TipoComando.BREAK:  // 0x1C: //BREAK
                                 TipoRisposta = 2;
                                 UltimaRisposta = SerialMessage.EsitoRisposta.MessaggioOk;
                                 _datiRicevuti = SerialMessage.TipoRisposta.Break;
                                 Log.Debug("Comando Corretto: Ricevuto BREAK --> fermo gli invii");
                                 _inviaRisposta = false;
                                 break;

                            case (byte)SerialMessage.TipoComando.SB_R_CicloLungo:
                                _CicliMemoriaLunga.Add(_mS.CicloLungo);
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Warn("Accodato Ciclo Lungo " + _mS.CicloLungo.IdEvento.ToString() + " - " + _CicliMemoriaLunga.Count.ToString());
                                break;

                            case (byte)SerialMessage.TipoComando.SB_R_CicloBreve:
                                _CicliMemoriaBreve.Add(_mS._CicloBreve);
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Warn("Accodato Ciclo Breve per il ciclo Lungo " + _mS._CicloBreve.IdEvento.ToString() + " in posizione " + _CicliMemoriaBreve.Count.ToString() );
                                break;

                            case (byte)SerialMessage.TipoComando.SB_R_LeggiMemoria:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                Log.Debug("Lettura Area Memoria");
                                break;

                            case (byte)SerialMessage.TipoComando.SB_W_chgst_Call:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                Log.Debug("Lettura Comando Strategia Memoria");
                                break;

                            case (byte)SerialMessage.TipoComando.Start: // 0x0F:
                            case (byte)SerialMessage.TipoComando.Stop:  // 0xF0:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Debug("Risposta Lettura Area Memoria 0x" + _mS._comando.ToString("X2"));
                                _inviaRisposta = false;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_R_DatiCliente:
                                Log.Debug("Lettura Dati Cliente " + _mS.CustomerData.stepReceived.ToString());
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = _mS.CustomerData.datiPronti;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_R_Programmazione:
                                _Programmazioni.Add(_mS.ProgRicarica);
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Debug("Accodata programmazione " + _mS.ProgRicarica.IdProgramma.ToString() + " in posizione " + _Programmazioni.Count.ToString());
                                //if ( _Programmazioni.Count < 23)  _inviaRisposta = false;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_W_Programmazione:
                               
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Debug("Nuova programmazione " + _mS.ProgRicarica.IdProgramma.ToString() );
                                _inviaRisposta = false;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_R_Variabili:
                                Log.Debug("Lettura Variabili " + _mS.variabiliScheda.ToString());
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                //_inviaRisposta = _mS.variabiliScheda.datiPronti;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_Cal_LetturaGain:
                                Log.Debug("Lettura Calibrazioni " + _mS.valoriCalibrazione.ToString());
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                //_inviaRisposta = _mS.variabiliScheda.datiPronti;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_R_DumpMemoria:
                                Log.Debug("Lettura Pacchetto Mem " + _mS.DumpMem.NumStep.ToString());
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = _inviaAckPacchettoDump;
                                //_inviaRisposta = _mS.CustomerData.datiPronti;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_DatiIniziali:// Prima Lettura
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = true;
                                Log.Debug("Prima Lettura");
                                break;
                            case (byte)SerialMessage.TipoComando.CicloProgrammato: // Ciclo Programmato
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Debug("Ciclo Programmato");
                                break;
                            case 0x03: // Cicli in memoria
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Debug("Cicli in memoria");
                                break;
                            case 0xD3: // read RTC
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Debug("Read RTC");
                                break;

                            case (byte)SerialMessage.TipoComando.SB_Cancella4K:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Debug("Cancella 4K");
                                //_inviaRisposta = _mS.variabiliScheda.datiPronti;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_CancellaInteraMemoria:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Debug("Cancella Intera Memoria");
                                //_inviaRisposta = _mS.variabiliScheda.datiPronti;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_R_BootloaderInfo:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                // _inviaRisposta = true;
                                _inviaRisposta = true;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_ReadRTC:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = true;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_R_ParametriLettura:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Debug("Lettura Parametri Lettura");
                                _inviaRisposta = false;
                                break;

                            case (byte)SerialMessage.TipoComando.SB_W_ParametriLettura:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Debug("Scrittura Parametri Lettura");
                                _inviaRisposta = false;
                                break;



                            default:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                Log.Debug("Altro Comando " + _mS._comando.ToString("X2"));
                                break;
                        }

                        if (_inviaRisposta)
                        {
                            Log.Debug("Esito: " + _mS._comando.ToString("X2"));
                            if (RichiestaInterruzione)
                            {
                                //Se Richiesta interruzione, invio Break
                                _mS._comando = 0x1C;
                                Log.Debug("Mandato BREAK SB ");
                                _datiRicevuti = SerialMessage.TipoRisposta.Break;
                            }
                            else
                            {
                                _mS._comando = 0x6C;
                                Log.Debug("Mandato ACK SB ");
                               // _datiRicevuti = SerialMessage.TipoRisposta.Ack;
                            }
                            _mS._dispositivo = 0x0000;
                            _mS.componiRisposta(_dataBuffer, _esito);

                            //serialeApparato.Write(_mS.messaggioRisposta, 0, _mS.messaggioRisposta.Length);
                            _parametri.scriviMessaggioSpyBatt(_mS.messaggioRisposta, 0, _mS.messaggioRisposta.Length);
                            Log.Debug(_mS.hexdumpArray(_mS.messaggioRisposta));
                        }
                        _rxRisposta = true;
                        Array.Resize(ref _dataBuffer, 0);
                        //return _datiRicevuti;
                    }

                }
                return _datiRicevuti;
            }  
            
            catch (Exception Ex)
            {
                Log.Error("analizzaCodaSB " + Ex.Message);
                _lastError = Ex.Message;
                _datiRicevuti = SerialMessage.TipoRisposta.NonValido;
                return _datiRicevuti;                           
            }

        }

        /// <summary>
        /// Ritorno il tipo dell'ultima risposta ricevuta dalla scheda
        /// </summary>
        public SerialMessage.TipoRisposta UltimaRispostaRicevuta
        {
            get
            {
                return _ultimaRisposta;
            }
        }

        public bool InvertiVersoCorrentiML(elementiComuni.VersoCorrente Verso)
        {
            try
            {
                bool _esito = true;
                VersoScarica = Verso;
                foreach ( sbMemLunga _ciclo in  CicliMemoriaLunga )
                {
                    _ciclo.VersoScarica = Verso;
                    _ciclo.InvertiVersoCorrentiMB(Verso);
                }

  
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("ScriviProgramma: " + Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }


        public void InizializzaParametriCalibrazione()
        {
            ParametriCalibrazione.Clear();
            ParametriCalibrazione.Add(new ParametroCalibrazione { IdParametro = 0x00, DescrizioneBase = "Corrente Nulla", NumDecimali = 0  });
            ParametriCalibrazione.Add(new ParametroCalibrazione { IdParametro = 0x01, DescrizioneBase = "Corrente Positiva (A/10)", NumDecimali = 1 });
            ParametriCalibrazione.Add(new ParametroCalibrazione { IdParametro = 0x02, DescrizioneBase = "Corrente Negativa (A/10)", NumDecimali = 1 });
            ParametriCalibrazione.Add(new ParametroCalibrazione { IdParametro = 0x03, DescrizioneBase = "Tensione Vbatt (V/100)", NumDecimali = 2 });
            ParametriCalibrazione.Add(new ParametroCalibrazione { IdParametro = 0x04, DescrizioneBase = "Tensione V3 (V/100)", NumDecimali = 2 });
            ParametriCalibrazione.Add(new ParametroCalibrazione { IdParametro = 0x05, DescrizioneBase = "Tensione V2 (V/100)", NumDecimali = 2 });
            ParametriCalibrazione.Add(new ParametroCalibrazione { IdParametro = 0x06, DescrizioneBase = "Tensione V1 (V/100)", NumDecimali = 2 });
        }

    }




}
