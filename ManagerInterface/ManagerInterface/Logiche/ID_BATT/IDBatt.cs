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
    public partial class IDBatt
    {
        public const int ADDR_START_PROGRAMMAZIONI = 0x2000;
        public const byte ADDR_START_PAR_PROGRAM = 0xF6;


        public const int DATABLOCK = 128;


        public enum EsitoRicalcolo : byte
        {
            OK = 0x00,
            ErrIMax = 0x11,
            ErrIMin = 0x12,
            ErrVMax = 0x21,
            ErrVMin = 0x22,
            ErrGenerico = 0xF0,
            ParNonValidi = 0xF1,
            ErrCBNonDef = 0xF2,
            CapBattNonValida = 0xF4,
            ErrUndef = 0xFF
        }     
        public enum TipoCaricaBatteria : byte { Generico = 0x00, LadeLight = 0x01, SuperCharger = 0x02, NonDefinito = 0xFF }


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

        public cbProgrammazioni Programmazioni; //= new cbProgrammazioni();

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
        public delegate void StepHandler(IDBatt ull, ProgressChangedEventArgs e);

        // -------------------------------------------------------


        private DateTime _startRead;

        public byte[] numeroSeriale;
        private string _lastError;
        private bool _cbCollegato;
        public bool apparatoPresente = false;
        public MoriData._db DbAttivo;

        public byte[] DatiRisposta;
       // public SerialMessage.EsitoRisposta UltimaRisposta;
       // private SerialMessage.TipoRisposta _ultimaRisposta = SerialMessage.TipoRisposta.NonValido;   // flag per l'indicazioene del tipo dell'ultimo messaggio ricevuto dalla scheda

        public int AttesaTimeout = 25; // Tempo attesa in decimi di secondo
       // public SerialMessage.OcBaudRate BrOCcorrente = SerialMessage.OcBaudRate.OFF;
       // public SerialMessage.OcEchoMode EchoOCcorrente = SerialMessage.OcEchoMode.OFF;

        public TipoCaricaBatteria TipoCB = TipoCaricaBatteria.Generico;
        // Solo ad uso test letture
        public int NumeroTentativiLettura;

        public IDBatt(ref parametriSistema parametri, MoriData._db dbCorrente, string IdDispositivo, TipoCaricaBatteria TipoCharger)
        {

            try
            {

                //ControllaAttesa(UltimaScrittura);

                _parametri = parametri;
                _cbCollegato = false;
                serialeApparato = _parametri.serialeCorrente;
                DbAttivo = dbCorrente;

                DatiBase = new DatiConfigCariche();
                //InizializzaDatiLocali();

                // Programmazioni
                Programmazioni.UltimoIdProgamma = 0;
                Programmazioni.NumeroRecordProg = 0;
                TipoCB = TipoCharger;


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
        public IDBatt(ref parametriSistema parametri, MoriData._db dbCorrente, TipoCaricaBatteria TipoCharger)
        {

            try
            {

                //ControllaAttesa(UltimaScrittura);

                _parametri = parametri;
                TipoCB = TipoCharger;
                DbAttivo = dbCorrente;
                ApparatoLL = new LadeLightData(dbCorrente);
                DatiCliente = new llDatiCliente(dbCorrente);
                Programmazioni = new cbProgrammazioni(dbCorrente);
                DatiBase = new DatiConfigCariche();
                //InizializzaDatiLocali();

                // Programmazioni
                Programmazioni.UltimoIdProgamma = 0;
                Programmazioni.NumeroRecordProg = 0;

            }

            catch (Exception Ex)
            {
                Log.Error("NEW CaricaBatteria: " + Ex.Message);
            }


        }


        public bool VerificaPresenza()
        {
            try
            {
                bool _esito = true;

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
                //_esito = LeggiBloccoMemoria(StartAddr, 226, out _datiTemp);

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

                return false;

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

                   // if (CancellaBlocco4K(ADDR_START_CONTATORI)) _esito = ScriviBloccoMemoria(ADDR_START_CONTATORI, (ushort)_datitemp.Length, _datitemp);
                   // if (CancellaBlocco4K(ADDR_START_BACKUP_CONTATORI)) _esito = _esito && ScriviBloccoMemoria(ADDR_START_BACKUP_CONTATORI, (ushort)_datitemp.Length, _datitemp);
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







        public bool LeggiCicloAttivo()
        {
            try
            {
                bool _esito = false;

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
                bool _esito = false;
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
               // _esito = LeggiBloccoMemoria(StartAddr, 10, out _IndiciProg);

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


        public bool CaricaProgrammazioniDB(string IdApparato, string TipoApparato)
        {
            try
            {
                bool _esito;

                Programmazioni.CaricaDatiDB(IdApparato, TipoApparato);


                return true;
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
                bool _esito = false;

                // ControllaAttesa(UltimaScrittura);

                _mS.Dispositivo = SerialMessage.TipoDispositivo.PcOrSmart;
                _mS.Comando = SerialMessage.TipoComando.CMD_PROG_CYCLE_CRG;
                _mS.CicloInMacchina = CicloInMacchina;
                _mS.ComponiMessaggioCicloProgrammato();
                _rxRisposta = false;
                Log.Debug("Scrivi Ciclo Programmato");
                Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                //                _parametri.serialeCorrente.Write(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                //_esito = aspettaRisposta(AttesaTimeout, 0, true);

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


        public bool LeggiParametriApparato()
        {
            try
            {
                bool _esito = false;
                ParametriApparato = new llParametriApparato(DbAttivo);
                MessaggioLadeLight.PrimoBloccoMemoria ImmagineMemoria = new MessaggioLadeLight.PrimoBloccoMemoria();
                SerialMessage.EsitoRisposta EsitoMsg;

                byte[] _datiTemp = new byte[240];
                //_esito = LeggiBloccoMemoria(0, 240, out _datiTemp);

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

                        //Memoria = new llMappaMemoria(ImmagineMemoria.ModelloMemoria, TipoCB);

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

        public bool PreparaSalvataggioProgrammazioni()
        {
            try
            {

                // Sposto avanti tutti i dati presenti --> Quì non serve. Se nuovo (senza ID), genero un nuovo ID
                //foreach (llProgrammaCarica tempPrg in Programmazioni.ProgrammiDefiniti)
                //{
                //    tempPrg.PosizioneCorrente += 1;
                //}

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
                        //_esito = ScriviBloccoMemoria(AdrDati, 226, _datiTemp);
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

        public bool CaricaCompleto(MoriData._db dbCorrente, string IdApparato, string TipoApparato)
        {
            try
            {
                bool _esito = false;
                if (IdApparato == "")
                    return false;

                //                _cb = new CaricaBatteria(ref _parametri, _logiche.dbDati.connessione);

                //_esito = CaricaApparatoA0(IdApparato);

                if (_esito)
                {

                    ApparatoLL = new LadeLightData(dbCorrente, ParametriApparato);

                    CaricaContatori(IdApparato);
                    DatiCliente = new llDatiCliente(dbCorrente);
                    DatiCliente.caricaDati(IdApparato, 1);
                    CaricaProgrammazioniDB(IdApparato, TipoApparato);


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
                if (SalvaDati) ApparatoLL.salvaDati();

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
                if (Programmazioni.ProgrammiDefiniti == null)
                {
                    Programmazioni.ProgrammiDefiniti = new List<llProgrammaCarica>();
                }
                Programmazioni.ProgrammiDefiniti.Clear();
                foreach (_llProgrammaCarica _prog in ModelloDati.Programmazioni)
                {
                    Programmazioni.ProgrammiDefiniti.Add(new llProgrammaCarica(dbCorrente, _prog));
                }
                if (SalvaDati) Programmazioni.SalvaDati();

                if (MemoriaCicli == null)
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

    }

}
