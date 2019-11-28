using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using log4net;
using log4net.Config;
using ChargerLogic;
using Utility;
using MoriData;

namespace PannelloCharger
{
    public partial class frmAvanzamentoCicli : Form
    {
        private int _minAvanzamento = 0;
        private int _maxAvanzamento = 100;
        private int _valAvanzamento = 50;

        private int _minAvanzamentoB = 0;
        private int _maxAvanzamentoB = 100;
        private int _valAvanzamentoB = 50;

        private static ILog Log = LogManager.GetLogger("--> Form Avanzamento");
        
        private BackgroundWorker sbWorker;

        public enum ControlledDevice : byte { Nessuno = 0, SpyBatt = 1, Display = 2, LadeLight = 3, Desolfatatore = 4 };


        public UnitaSpyBatt sbLocale;
        public CaricaBatteria llLocale;
        public ControlledDevice ElementoPilotato = ControlledDevice.SpyBatt; 

        public elementiComuni.tipoMessaggio TipoComando { get; set; }
        public BloccoFirmware FirmwareBlock { get; set; }
        public BloccoFirmwareLL FirmwareLLBlock { get; set; }
        public byte FirmwareArea { get; set; }
        public int ValStart;
        public UInt32 AddrStart;
        public int ValFine;
        public bool InviaACK = false;
        public bool SalvaHexDump = false;
        public String FileHexDump = "";

        public MoriData._db DbDati;
        public ListaRegistrazioni ListaCicli;
        public bool CaricaBrevi = false;
        public bool CaricaUltimoLungo = false;
        private DateTime _startCompute;
        private DateTime _firstRecord;
        private bool _firstRecordReceived;

        public usParameters ParametriWorker = new usParameters();

        public frmAvanzamentoCicli()
        {
            InitializeComponent();
            Log.Debug("Load Form Avanzamento");
            sbWorker = new BackgroundWorker();


            ParametriWorker.MainName = "Ciclo Primario";
            ParametriWorker.MainCount = 10;

            cEventHelper.RemoveEventHandler(sbWorker, "DoWork");
            sbWorker.DoWork += new DoWorkEventHandler(sbWorker_DoWork);

            cEventHelper.RemoveEventHandler(sbWorker, "ProgressChanged");
            sbWorker.ProgressChanged += new ProgressChangedEventHandler(sbWorker_ProgressChanged);

            cEventHelper.RemoveEventHandler(sbWorker, "RunWorkerCompleted");
            sbWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(sbWorker_RunWorkerCompleted);

            sbWorker.WorkerReportsProgress = true;
            sbWorker.WorkerSupportsCancellation = true;

        }

        public void AvviaRicalcolo()
        {
            bool _esito;

            try
            {

                _startCompute = DateTime.Now;
                _firstRecordReceived = false;
                btnStartLettura.Enabled = false;
                btnAnnulla.Enabled = true;
                btnChiudi.Enabled = false;
                lblAvanzmentoL.Text = "";
                lblAvanzmentoB.Text = "";
                impostaAvanzamento(0, 100);
                Log.Debug("Start Async ");
                sbWorker.RunWorkerAsync();

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return;
            }
            //  Start the async operation here
            /// sbWorker.RunWorkerAsync();

        }


        /// <summary>
        /// Lancia l'attività asincrona in base a quanto richiesto col parametro TipoComando.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        void sbWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            btnAnnulla.Enabled = true;
            btnChiudi.Enabled = false;
            btnStartLettura.Enabled = false;
            elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
            elementiComuni.WaitStep _stepBg = new elementiComuni.WaitStep();
            ProgressChangedEventArgs _stepEv;

            //time consuming operation
            DateTime _inizioChiamata = DateTime.Now;

            //NOTE : Never play with the UI thread here...
            bool _esito;
            switch (ElementoPilotato)
            {
                case ControlledDevice.Nessuno:
                    break;
                case ControlledDevice.SpyBatt:
                    sbLocale.RichiestaInterruzione = false;
                    DoWork_SB(sender, e);

                    break;
                case ControlledDevice.Display:
                    break;
                case ControlledDevice.LadeLight:
                    llLocale.RichiestaInterruzione = false;
                    DoWork_LL(sender, e);
                    break;
                case ControlledDevice.Desolfatatore:
                    break;
                default:
                    break;
            }


            //Report 100% completion on operation completed

            TimeSpan _tTrascorso = DateTime.Now.Subtract(_inizioChiamata);

            _esitoBg.EventiPrevisti = 1;
            _esitoBg.UltimoEvento = 1;
            _esitoBg.SecondiElaborazione = _tTrascorso.TotalSeconds;

            e.Result = _esitoBg;
        }

        /// <summary>
        /// Esegue le attività previste all'attivazione asincrona (DoWork) quando il device è uno spybatt.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DoWorkEventArgs"/> instance containing the event data.</param>
        void DoWork_SB(object sender, DoWorkEventArgs e)
        {
            //NOTE : Never play with the UI thread here...
            bool _esito;

            btnAnnulla.Enabled = true;
            btnChiudi.Enabled = false;
            btnStartLettura.Enabled = false;
            elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
            elementiComuni.WaitStep _stepBg = new elementiComuni.WaitStep();
            ProgressChangedEventArgs _stepEv;
            //time consuming operation
            DateTime _inizioChiamata = DateTime.Now;
            usParameters _parametri = (usParameters)e.Argument;



            switch (TipoComando)
            {
                case elementiComuni.tipoMessaggio.vuoto:
                    {
                        break;
                    }
                case elementiComuni.tipoMessaggio.DumpMemoria:
                    {
                        _stepBg.Titolo = StringheComuni.AvTitolo01Dati;  // "Caricamento Memoria Dati";
                        _stepBg.Step = -1;
                        // _stepEv = new ProgressChangedEventArgs(0, _stepBg);
                        sbWorker.ReportProgress(0, _stepBg);
                        Log.Debug("Lancio lettura Memoria ");
                        _esito = sbLocale.LeggiInteraMemoria(sbLocale.Id, true, DbDati, InviaACK, true, SalvaHexDump, FileHexDump);
                        break;
                    }
                case elementiComuni.tipoMessaggio.MemLunga:
                    {
                        _stepBg.Titolo = StringheComuni.AvTitolo02Lunghi;  // "Caricamento Eventi Lunghi";
                        _stepBg.Step = -1;
                        // _stepEv = new ProgressChangedEventArgs(0, _stepBg);
                        sbWorker.ReportProgress(0, _stepBg);
                        Log.Debug("Lancio lettura: MemLunga " + ValStart.ToString() + " - " + ValFine.ToString());
                        object _esitoLunghi;

                        _esito = sbLocale.RicaricaCaricaCicliMemLunga((uint)ValStart, (uint)ValFine, DbDati, true, CaricaBrevi, out _esitoLunghi, true);
                        break;
                    }
                case elementiComuni.tipoMessaggio.MemBreve:
                    {
                        _stepBg.Titolo = StringheComuni.AvTitolo03Brevi;  // "Caricamento Eventi Brevi";
                        _stepBg.Step = -1;

                        _stepEv = new ProgressChangedEventArgs(0, _stepBg);
                        sbWorker.ReportProgress(0, _stepBg);
                        int _stepCorrente = 0;

                        foreach (Registrazione _elemento in ListaCicli.Elenco)
                        {
                            // Se chiedo la cancellazione esco al ciclo successivo
                            if (sbWorker.CancellationPending)
                            {
                                e.Cancel = true;
                                sbLocale.RichiestaInterruzione = true;
                                break;
                            }

                            sbMemLunga _memLunga = (sbMemLunga)_elemento.Record;
                            if (_memLunga != null)
                            {
                                _stepCorrente++;
                                //Scarico i brevi solo se ho un puntatore valido
                                if (_memLunga.PuntatorePrimoBreve < 0xFFFFFFFF)
                                {

                                    //prima avanzo il contatore lunghi
                                    elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                    int _progress = 0;
                                    double _valProgress = 0;
                                    _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                    _passo.Titolo = "";
                                    _passo.Eventi = ListaCicli.Elenco.Count();
                                    _passo.Step = _stepCorrente;
                                    if (_passo.Eventi > 0)
                                    {
                                        _valProgress = (_passo.Step * 100) / _passo.Eventi;
                                    }
                                    _progress = (int)_valProgress;
                                    //ProgressChangedEventArgs _stepEvBreve = new ProgressChangedEventArgs(_progress, _passo);
                                    //Step(this, _stepEvBreve);
                                    sbWorker.ReportProgress(_progress, _passo);
                                    // poi lancio il caricamento brevi

                                    if (sbLocale.RicaricaCaricaCicliMemBreve(_memLunga))
                                    {
                                        _memLunga.CaricaBrevi();
                                        Log.Debug("Caricati Brevi ");

                                    }
                                }
                            }

                        }

                        sbLocale.ConsolidaBrevi();



                        break;
                    }
                case elementiComuni.tipoMessaggio.AggiornamentoFirmware:
                    {
                        _stepBg.Titolo = StringheComuni.AvTitolo04Firmware;  // "Aggiornamento Firmware";
                        _stepBg.Step = -1;
                        // _stepEv = new ProgressChangedEventArgs(0, _stepBg);
                        sbWorker.ReportProgress(0, _stepBg);
                        Log.Debug("Lancio aggiornamento firmware ");

                        _esito = sbLocale.AggiornaFirmware(sbLocale.Id, sbLocale.apparatoPresente, FirmwareArea, FirmwareBlock, true);// InviaACK, true, SalvaHexDump, FileHexDump);
                        break;
                    }

                case elementiComuni.tipoMessaggio.AggiornamentoFirmwareLL:
                    {
                        _stepBg.Titolo = StringheComuni.AvTitolo04Firmware;  // "Aggiornamento Firmware";
                        _stepBg.Step = -1;
                        _stepEv = new ProgressChangedEventArgs(0, _stepBg);
                        sbWorker.ReportProgress(0, _stepBg);
                        Log.Debug("Lancio aggiornamento firmware ");

                        _esito = llLocale.AggiornaFirmware("", llLocale.apparatoPresente, FirmwareArea, FirmwareLLBlock, true); // InviaACK, true, SalvaHexDump, FileHexDump);
                        break;
                    }

            }



            //Report 100% completion on operation completed

            TimeSpan _tTrascorso = DateTime.Now.Subtract(_inizioChiamata);

            _esitoBg.EventiPrevisti = 1;
            _esitoBg.UltimoEvento = 1;
            _esitoBg.SecondiElaborazione = _tTrascorso.TotalSeconds;
            e.Result = _esitoBg;
        }


        void DoWork_LL(object sender, DoWorkEventArgs e)
        {
            //NOTE : Never play with the UI thread here...
            bool _esito;

            btnAnnulla.Enabled = true;
            btnChiudi.Enabled = false;
            btnStartLettura.Enabled = false;
            elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
            elementiComuni.WaitStep _stepBg = new elementiComuni.WaitStep();
            ProgressChangedEventArgs _stepEv;
            //time consuming operation
            DateTime _inizioChiamata = DateTime.Now;
            usParameters _parametri = (usParameters)e.Argument;
            if(sbWorker.CancellationPending)
            {
                Log.Debug("Richiesta cancellazione pendente. chiudo il BW");
                e.Cancel = true;
            }


            switch (TipoComando)
            {
                case elementiComuni.tipoMessaggio.vuoto:
                    {
                        break;
                    }
                case elementiComuni.tipoMessaggio.DumpMemoria:
                    {
                        break;
                    }
                case elementiComuni.tipoMessaggio.MemLungaLL:
                    {
                        _stepBg.Titolo = StringheComuni.AvTitolo02Lunghi;  // "Caricamento Eventi Lunghi";
                        _stepBg.Step = -1;
                        // _stepEv = new ProgressChangedEventArgs(0, _stepBg);
                        sbWorker.ReportProgress(0, _stepBg);
                        Log.Debug("Lancio lettura: MemLunga " + ValStart.ToString() + " - " + ValFine.ToString());
                        object _esitoLunghi;
                        // _esito = _cb.CaricaListaCicli(StartAddr, NumRows, out EsitoCaricamento, false, true);

                        _esito = llLocale.CaricaListaCicli(AddrStart, (ushort)ValFine, out _esitoLunghi, CaricaBrevi, true);

                        break;
                    }
                case elementiComuni.tipoMessaggio.AreaMemLungaLL:
                    {
                        _stepBg.Titolo = StringheComuni.AvTitolo02Lunghi;  // "Caricamento Eventi Lunghi";
                        _stepBg.Step = -1;
                        // _stepEv = new ProgressChangedEventArgs(0, _stepBg);
                        sbWorker.ReportProgress(0, _stepBg);
                        Log.Debug("Lancio lettura: MemLunga " + ValStart.ToString() + " - " + ValFine.ToString());
                        
                        _esito = llLocale.LeggiBloccoLunghi(true);

                        break;
                    }

                case elementiComuni.tipoMessaggio.MemBreve:
                    {
                        _stepBg.Titolo = StringheComuni.AvTitolo03Brevi;  // "Caricamento Eventi Brevi";
                        _stepBg.Step = -1;

                        _stepEv = new ProgressChangedEventArgs(0, _stepBg);
                        sbWorker.ReportProgress(0, _stepBg);
                        int _stepCorrente = 0;

                        foreach (Registrazione _elemento in ListaCicli.Elenco)
                        {
                            // Se chiedo la cancellazione esco al ciclo successivo
                            if (sbWorker.CancellationPending)
                            {
                                e.Cancel = true;
                                sbLocale.RichiestaInterruzione = true;
                                break;
                            }

                            sbMemLunga _memLunga = (sbMemLunga)_elemento.Record;
                            if (_memLunga != null)
                            {
                                _stepCorrente++;
                                //Scarico i brevi solo se ho un puntatore valido
                                if (_memLunga.PuntatorePrimoBreve < 0xFFFFFFFF)
                                {

                                    //prima avanzo il contatore lunghi
                                    elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                    int _progress = 0;
                                    double _valProgress = 0;
                                    _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                    _passo.Titolo = "";
                                    _passo.Eventi = ListaCicli.Elenco.Count();
                                    _passo.Step = _stepCorrente;
                                    if (_passo.Eventi > 0)
                                    {
                                        _valProgress = (_passo.Step * 100) / _passo.Eventi;
                                    }
                                    _progress = (int)_valProgress;
                                    //ProgressChangedEventArgs _stepEvBreve = new ProgressChangedEventArgs(_progress, _passo);
                                    //Step(this, _stepEvBreve);
                                    sbWorker.ReportProgress(_progress, _passo);
                                    // poi lancio il caricamento brevi

                                    if (sbLocale.RicaricaCaricaCicliMemBreve(_memLunga))
                                    {
                                        _memLunga.CaricaBrevi();
                                        Log.Debug("Caricati Brevi ");

                                    }
                                }
                            }

                        }

                        sbLocale.ConsolidaBrevi();

                        break;
                    }
                case elementiComuni.tipoMessaggio.AggiornamentoFirmware:
                    {
                        break;
                    }

                case elementiComuni.tipoMessaggio.AggiornamentoFirmwareLL:
                    {
                        _stepBg.Titolo = StringheComuni.AvTitolo04Firmware;  // "Aggiornamento Firmware";
                        _stepBg.Step = -1;
                        _stepEv = new ProgressChangedEventArgs(0, _stepBg);
                        sbWorker.ReportProgress(0, _stepBg);
                        Log.Debug("Lancio aggiornamento firmware ");

                        _esito = llLocale.AggiornaFirmware("", llLocale.apparatoPresente, FirmwareArea, FirmwareLLBlock, true); // InviaACK, true, SalvaHexDump, FileHexDump);
                        break;
                    }

            }



            //Report 100% completion on operation completed

            TimeSpan _tTrascorso = DateTime.Now.Subtract(_inizioChiamata);

            _esitoBg.EventiPrevisti = 1;
            _esitoBg.UltimoEvento = 1;
            _esitoBg.SecondiElaborazione = _tTrascorso.TotalSeconds;
            e.Result = _esitoBg;
        }





        void sbWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            string _messaggio1; 
            string _messaggio2;
            int _localProgressPercentage = e.ProgressPercentage;

            elementiComuni.WaitStep _statoE = null;
            elementiComuni.WaitStep _statoLL= null;
            bool _EsecuzioneInterrotta = false;
            string _titolo ="";
            int _step = 0;
            int _retry = 0;
            elementiComuni.tipoMessaggio _msg = elementiComuni.tipoMessaggio.vuoto;

            switch (ElementoPilotato)
            {
                case ControlledDevice.Nessuno:
                    break;
                case ControlledDevice.SpyBatt:
                    _statoE = (elementiComuni.WaitStep)e.UserState;
                    _EsecuzioneInterrotta = _statoE.EsecuzioneInterrotta;
                    _titolo = _statoE.Titolo;
                    _step = _statoE.Step;
                    _msg = _statoE.TipoDati;
                    break;
                case ControlledDevice.Display:
                    break;
                case ControlledDevice.LadeLight:
                    _statoLL = (elementiComuni.WaitStep)e.UserState;
                    _EsecuzioneInterrotta = _statoLL.EsecuzioneInterrotta;
                    _titolo = _statoLL.Titolo;
                    _step = _statoLL.Step;
                    _msg = _statoLL.TipoDati;
                    _retry = _statoLL.NumTentativi;
                    break;
                case ControlledDevice.Desolfatatore:
                    break;
                default:
                    break;
            }

            //Here you play with the main UI thread
            //sbWaitStep _statoE = (sbWaitStep)e.UserState;

            if (_localProgressPercentage > 100)
                _localProgressPercentage = 100;

            if (_EsecuzioneInterrotta)
            {
                lblTitolo.Text = _titolo;
                return;
            }

            if (_step == -1)
            {
                lblTitolo.Text = _titolo;
                return;
            }
            switch (_msg)
            {

                case elementiComuni.tipoMessaggio.MemLunga:
                    {
                        //if (_statoE.TipoDati == elementiComuni.tipoMessaggio.MemLunga)
                        {
                            if (_firstRecordReceived != true)
                            {
                                _firstRecord = DateTime.Now;
                                TimeSpan _durata = _firstRecord.Subtract(_startCompute);
                                if (_statoE == null)
                                {
                                    _messaggio2 = "Evento non formato - tempo: " + _durata.TotalSeconds.ToString("0.00");
                                }
                                else
                                {
                                    _messaggio2 = "Primo Evento: step " + _statoE.Step.ToString() + " - tempo: " + _durata.TotalSeconds.ToString("0.00");
                                    _firstRecordReceived = true;
                                }
                                lblMsg02.Text = _messaggio2;

                            }


                            TimeSpan _tTrascorso = DateTime.Now.Subtract(_startCompute);
                            if (_statoE == null)
                            {
                                _messaggio1 = "Evento non formato - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000");
                            }
                            else
                            {
                                double _previsto = 0;
                                if (_statoE.Step > 0)
                                {
                                    _previsto = (_tTrascorso.TotalSeconds * _statoE.Eventi) / _statoE.Step;
                                }
                                //                _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000") + " di " + _previsto.ToString("0.000");

                                if (_statoE.TipoDati == elementiComuni.tipoMessaggio.MemLunga)
                                {
                                    _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.00") + " s di " + _previsto.ToString("0.000") + " s";
                                }
                                else
                                {
                                    _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo trascorso: " + _tTrascorso.TotalSeconds.ToString("0.000") + " s";
                                }

                                _firstRecordReceived = true;
                                lblAvanzmentoL.Text = e.ProgressPercentage.ToString() + "%";
                                lblMsg01.Text = _messaggio1;

                            }
                            //Log.Info("Worker Progress " + e.ProgressPercentage.ToString() + " (" + _statoE.Step.ToString() + ")");
                            pgbAvanamentoL.Value = _localProgressPercentage;
                            lblMessaggioAvanzamento.Text = "Processing......" + pgbAvanamentoL.Value.ToString() + "%";
                        }
                        break;
                    }
                case elementiComuni.tipoMessaggio.AreaMemLungaLL:
                    {
                        //if (_statoE.TipoDati == elementiComuni.tipoMessaggio.MemLungaLL)
                        {
                            if (_firstRecordReceived != true)
                            {
                                _firstRecord = DateTime.Now;
                                TimeSpan _durata = _firstRecord.Subtract(_startCompute);
                                if (_statoE == null)
                                {
                                    _messaggio2 = "Evento non formato - tempo: " + _durata.TotalSeconds.ToString("0.00");
                                }
                                else
                                {
                                    _messaggio2 = "Primo Evento: step " + _statoE.Step.ToString() + " - tempo: " + _durata.TotalSeconds.ToString("0.00");
                                    _firstRecordReceived = true;
                                }
                                lblMsg02.Text = _messaggio2;

                            }


                            TimeSpan _tTrascorso = DateTime.Now.Subtract(_startCompute);
                            if (_statoE == null)
                            {
                                _messaggio1 = "Evento non formato - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000");
                            }
                            else
                            {
                                double _previsto = 0;
                                if (_statoE.Step > 0)
                                {
                                    _previsto = (_tTrascorso.TotalSeconds * _statoE.Eventi) / _statoE.Step;
                                }
                                //                _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000") + " di " + _previsto.ToString("0.000");

                                if (_statoE.TipoDati == elementiComuni.tipoMessaggio.MemLunga)
                                {
                                    _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.00") + " s di " + _previsto.ToString("0.000") + " s";
                                }
                                else
                                {
                                    _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo trascorso: " + _tTrascorso.TotalSeconds.ToString("0.000") + " s";
                                }

                                _firstRecordReceived = true;
                                lblAvanzmentoL.Text = e.ProgressPercentage.ToString() + "%";
                                lblMsg01.Text = _messaggio1;

                            }
                            //Log.Info("Worker Progress " + e.ProgressPercentage.ToString() + " (" + _statoE.Step.ToString() + ")");
                            pgbAvanamentoL.Value = _localProgressPercentage;
                            lblMsgFail.Text = "Total packet retry: " + _retry.ToString();
                            lblMessaggioAvanzamento.Text = "Processing......" + pgbAvanamentoL.Value.ToString() + "%";
                        }
                        break;
                    }

                case elementiComuni.tipoMessaggio.AreaMemBreveLL:
                    {
                        //if (_statoE.TipoDati == elementiComuni.tipoMessaggio.MemLungaLL)
                        {
                            if (_firstRecordReceived != true)
                            {
                                _firstRecord = DateTime.Now;
                                TimeSpan _durata = _firstRecord.Subtract(_startCompute);
                                if (_statoE == null)
                                {
                                    _messaggio2 = "Evento non formato - tempo: " + _durata.TotalSeconds.ToString("0.00");
                                }
                                else
                                {
                                    _messaggio2 = "Primo Evento: step " + _statoE.Step.ToString() + " - tempo: " + _durata.TotalSeconds.ToString("0.00");
                                    _firstRecordReceived = true;
                                }
                                lblMsg02.Text = _messaggio2;

                            }


                            TimeSpan _tTrascorso = DateTime.Now.Subtract(_startCompute);
                            if (_statoE == null)
                            {
                                _messaggio1 = "Evento non formato - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000");
                            }
                            else
                            {
                                double _previsto = 0;
                                if (_statoE.Step > 0)
                                {
                                    _previsto = (_tTrascorso.TotalSeconds * _statoE.Eventi) / _statoE.Step;
                                }
                                //                _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000") + " di " + _previsto.ToString("0.000");

                                if (_statoE.TipoDati == elementiComuni.tipoMessaggio.MemLunga)
                                {
                                    _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.00") + " s di " + _previsto.ToString("0.000") + " s";
                                }
                                else
                                {
                                    _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo trascorso: " + _tTrascorso.TotalSeconds.ToString("0.000") + " s";
                                }

                                _firstRecordReceived = true;
                                lblAvanzmentoL.Text = e.ProgressPercentage.ToString() + "%";
                                lblMsg01.Text = _messaggio1;

                            }
                            //Log.Info("Worker Progress " + e.ProgressPercentage.ToString() + " (" + _statoE.Step.ToString() + ")");
                            pgbAvanamentoL.Value = _localProgressPercentage;
                            lblMessaggioAvanzamento.Text = "Processing......" + pgbAvanamentoL.Value.ToString() + "%";
                        }
                        break;
                    }


                case elementiComuni.tipoMessaggio.MemLungaLL:
                    {
                        //if (_statoE.TipoDati == elementiComuni.tipoMessaggio.MemLungaLL)
                        {
                            if (_firstRecordReceived != true)
                            {
                                _firstRecord = DateTime.Now;
                                TimeSpan _durata = _firstRecord.Subtract(_startCompute);
                                if (_statoE == null)
                                {
                                    _messaggio2 = "Evento non formato - tempo: " + _durata.TotalSeconds.ToString("0.00");
                                }
                                else
                                {
                                    _messaggio2 = "Primo Evento: step " + _statoE.Step.ToString() + " - tempo: " + _durata.TotalSeconds.ToString("0.00");
                                    _firstRecordReceived = true;
                                }
                                lblMsg02.Text = _messaggio2;

                            }


                            TimeSpan _tTrascorso = DateTime.Now.Subtract(_startCompute);
                            if (_statoE == null)
                            {
                                _messaggio1 = "Evento non formato - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000");
                            }
                            else
                            {
                                double _previsto = 0;
                                if (_statoE.Step > 0)
                                {
                                    _previsto = (_tTrascorso.TotalSeconds * _statoE.Eventi) / _statoE.Step;
                                }
                                //                _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000") + " di " + _previsto.ToString("0.000");

                                if (_statoE.TipoDati == elementiComuni.tipoMessaggio.MemLunga)
                                {
                                    _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.00") + " s di " + _previsto.ToString("0.000") + " s";
                                }
                                else
                                {
                                    _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo trascorso: " + _tTrascorso.TotalSeconds.ToString("0.000") + " s";
                                }

                                _firstRecordReceived = true;
                                lblAvanzmentoL.Text = e.ProgressPercentage.ToString() + "%";
                                lblMsg01.Text = _messaggio1;

                            }
                            //Log.Info("Worker Progress " + e.ProgressPercentage.ToString() + " (" + _statoE.Step.ToString() + ")");
                            pgbAvanamentoL.Value = _localProgressPercentage;
                            lblMessaggioAvanzamento.Text = "Processing......" + pgbAvanamentoL.Value.ToString() + "%";
                        }
                        break;
                    }


                case elementiComuni.tipoMessaggio.MemBreve:
                    {
                        {

                            TimeSpan _tTrascorso = DateTime.Now.Subtract(_startCompute);
                            if (_statoE == null)
                            {
                                _messaggio1 = "Evento non formato - tempo: " + _tTrascorso.TotalSeconds.ToString("0.00");
                            }
                            else
                            {
                                double _previsto = 0;
                                if (_statoE.Step > 0)
                                {
                                    _previsto = (_tTrascorso.TotalSeconds * _statoE.Eventi) / _statoE.Step;
                                }
                                //                _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000") + " di " + _previsto.ToString("0.000");
                                _messaggio1 = "Step Breve: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000") + " di " + _previsto.ToString("0.000");
                                _firstRecordReceived = true;
                                lblAvanzmentoB.Text = e.ProgressPercentage.ToString() + "%";
                                lblMsg01.Text = _messaggio1;

                            }
                            Log.Info("Worker Progress Evento Breve " + e.ProgressPercentage.ToString() + " (" + _statoE.Step.ToString() + ")");
                            pgbAvanamentoB.Value = _localProgressPercentage;
                            lblMessaggioAvanzamento.Text = "Processing......" + pgbAvanamentoB.Value.ToString() + "%";
                        }
                        break;
                    }
                case elementiComuni.tipoMessaggio.DumpMemoria:
                    {
                        //if (_statoE.TipoDati == elementiComuni.tipoMessaggio.MemBreve)
                        {

                            TimeSpan _tTrascorso = DateTime.Now.Subtract(_startCompute);
                            if (_statoE == null)
                            {
                                _messaggio1 = "Evento non formato - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000");
                            }
                            else
                            {
                                double _previsto = 0;
                                if (_statoE.Step > 0)
                                {
                                    _previsto = (_tTrascorso.TotalSeconds * _statoE.Eventi) / _statoE.Step;
                                }
                                //                _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000") + " di " + _previsto.ToString("0.000");
                                _messaggio1 = "Pacchetto: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000") + " di " + _previsto.ToString("0.000");
                                _firstRecordReceived = true;
                                lblAvanzmentoL.Text = e.ProgressPercentage.ToString() + "%";
                                lblMsg01.Text = _messaggio1;

                            }
                            Log.Info("Worker Progress Dump Mem " + e.ProgressPercentage.ToString() + " (" + _statoE.Step.ToString() + ")");
                            if (_localProgressPercentage <= 100)
                            {
                                pgbAvanamentoL.Value = _localProgressPercentage;
                            }
                            else
                            {
                                pgbAvanamentoL.Value = 100;
                            }

                            lblMessaggioAvanzamento.Text = "Processing......" + pgbAvanamentoB.Value.ToString() + "%";
                        }
                        break;
                    }
                case elementiComuni.tipoMessaggio.AggiornamentoFirmware:
                    {
                        {

                            TimeSpan _tTrascorso = DateTime.Now.Subtract(_startCompute);
                            if (_statoE == null)
                            {
                                _messaggio1 = "Evento non formato - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000");
                            }
                            else
                            {
                                double _previsto = 0;
                                if (_statoE.Step > 0)
                                {
                                    _previsto = (_tTrascorso.TotalSeconds * _statoE.Eventi) / _statoE.Step;
                                }
                                //                _messaggio1 = "Step: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000") + " di " + _previsto.ToString("0.000");
                                _messaggio1 = "Step Breve: " + _statoE.Step.ToString() + " di " + _statoE.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000") + " di " + _previsto.ToString("0.000");
                                _firstRecordReceived = true;
                                lblAvanzmentoB.Text = e.ProgressPercentage.ToString() + "%";
                                lblMsg01.Text = _messaggio1;

                            }
                            Log.Info("Worker Progress Firmware " + e.ProgressPercentage.ToString() + " (" + _statoE.Step.ToString() + ")");
                            pgbAvanamentoB.Value = _localProgressPercentage;
                            lblMessaggioAvanzamento.Text = "Processing......" + pgbAvanamentoB.Value.ToString() + "%";
                        }
                        break;
                    }
                case elementiComuni.tipoMessaggio.AggiornamentoFirmwareLL:
                    {
                        {

                            TimeSpan _tTrascorso = DateTime.Now.Subtract(_startCompute);
                            if (_statoLL == null)
                            {
                                _messaggio1 = "Evento non formato - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000");
                            }
                            else
                            {
                                double _previsto = 0;
                                if (_statoLL.Step > 0)
                                {
                                    _previsto = (_tTrascorso.TotalSeconds * _statoLL.Eventi) / _statoE.Step;
                                }
                                _messaggio1 = "Step Breve: " + _statoLL.Step.ToString() + " di " + _statoLL.Eventi.ToString() + " - tempo: " + _tTrascorso.TotalSeconds.ToString("0.000") + " di " + _previsto.ToString("0.000");
                                _firstRecordReceived = true;
                                lblAvanzmentoB.Text = e.ProgressPercentage.ToString() + "%";
                                lblMsg01.Text = _messaggio1;

                            }
                            Log.Info("Worker Progress Firmware " + e.ProgressPercentage.ToString() + " (" + _statoLL.Step.ToString() + ")");
                            pgbAvanamentoB.Value = _localProgressPercentage;
                            lblMessaggioAvanzamento.Text = "Processing......" + pgbAvanamentoB.Value.ToString() + "%";
                        }
                        break;
                    }

                default:
                    {
                        Log.Info("Worker Progress Evento non riconosciuto: " + _msg.ToString());
                        break;
                    }
            }


        }

        void stepRispostaSB(UnitaSpyBatt sender, ProgressChangedEventArgs e)
        {
            try
            {
                elementiComuni.WaitStep _statoE = (elementiComuni.WaitStep)e.UserState;
                int ValAvanzamento = 0;
                double _valAvanzamento = 0;
                //Here you play with the main UI thread

                if (_statoE.Eventi > 0)
                {
                    _valAvanzamento = (_statoE.Step * 100) / _statoE.Eventi;
                }
                else
                {
                    _valAvanzamento = 0;
                }
                ValAvanzamento = (int)_valAvanzamento;
                Log.Warn("Ricevuto STEP SB "+ ValAvanzamento.ToString() );
                sbWorker.ReportProgress(ValAvanzamento, _statoE);
            }
            catch (Exception Ex)
            {
                Log.Debug("stepRisposta: " + Ex.Message);
                       
            }
        }

        void stepRispostaLL(CaricaBatteria sender, ProgressChangedEventArgs e)
        {
            try
            {
                elementiComuni.WaitStep _statoE = (elementiComuni.WaitStep)e.UserState;
                int ValAvanzamento = 0;
                double _valAvanzamento = 0;
                //Here you play with the main UI thread

                if (_statoE.Eventi > 0)
                {
                    _valAvanzamento = (_statoE.Step * 100) / _statoE.Eventi;
                }
                else
                {
                    _valAvanzamento = 0;
                }
                ValAvanzamento = (int)_valAvanzamento;
                Log.Warn("Ricevuto STEP LL " + ValAvanzamento.ToString());
                sbWorker.ReportProgress(ValAvanzamento, _statoE);
            }
            catch (Exception Ex)
            {
                Log.Debug("stepRisposta: " + Ex.Message);

            }
        }

        void fineElaborazione(UnitaSpyBatt sender, RunWorkerCompletedEventArgs e)
        {
            Log.Debug("Ricevuta fine ricezione");

           // sbWorker.  (sender, e);

        }
        
        void sbWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                string _messaggio1;

                if (e.Cancelled)
                {
                    Log.Debug("sbWorker_RunWorkerCompleted: Task Cancellato");
                    lblMessaggioAvanzamento.Text = "Task Cancelled.";
                }
                else if (e.Error != null)
                {
                    Log.Warn("sbWorker_RunWorkerCompleted: " + e.Error.Message);
                    lblMessaggioAvanzamento.Text = "Error while performing background operation.";
                }
                else
                {
                    Log.Debug("sbWorker_RunWorkerCompleted: Task Completed");
                    if (e.Result != null)
                    {
                        elementiComuni.EndStep _esito = (elementiComuni.EndStep)e.Result;


                        _messaggio1 = "Ricevuti : " + _esito.UltimoEvento.ToString() + " di " + _esito.EventiPrevisti.ToString() + " - tempo: " + FunzioniMR.StringaDurataFull( _esito.SecondiElaborazione);
                        Log.Debug(_messaggio1);
                        //lblAvanzmentoL.Text = "Task Completato";
                        lblMsg01.Text = _messaggio1;
                        lblMessaggioAvanzamento.Text = "Operazione Completata.";

                        // Task Completato aspetto 5 secondi poi chiudo

                        this.Close();

                    }
                    else
                    {
                        lblMessaggioAvanzamento.Text = "Task Completato senza esito";
                    }
                }

                Log.Debug("Task Completed: " + e.Result.ToString());

                btnAnnulla.Enabled = false;
                btnChiudi.Enabled = true;
                
                //sbWorker.Dispose();
                btnStartLettura.Enabled = true;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return;
            }
        }



        private void lblAvanzmento_Click(object sender, EventArgs e)
        {

        }

        private void frmAvanzamentoCicli_Load(object sender, EventArgs e)
        {

        }

        public void mostraAvanzamento(bool Visibile)
        {
            pgbAvanamentoL.Visible = Visibile;
            lblAvanzmentoL.Visible = Visibile;
            this.Refresh();
            Application.DoEvents();
        }

        public void impostaAvanzamento(int Minimo, int Massimo)
        {
            if (Minimo < Massimo)
            {
                _minAvanzamento = Minimo;
                _maxAvanzamento = Massimo;
            }


            pgbAvanamentoL.Minimum = _minAvanzamento;
            pgbAvanamentoL.Maximum = _maxAvanzamento;


        }

        public void CicliDaMemoria()
        { 
        
        }




        public void valoreAvanzamento(int valore)
        {

            if (valore < _minAvanzamento) valore = _minAvanzamento;
            if (valore > _maxAvanzamento) valore = _maxAvanzamento;
            pgbAvanamentoL.Value = valore;
            if (_maxAvanzamento != 0)
            {
                float valcorrene;
                valcorrene = (valore - _minAvanzamento) / (_maxAvanzamento - _minAvanzamento) * 100;
                lblAvanzmentoL.Text = valcorrene.ToString("0.0") + "%";
            }

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            if (sbWorker.IsBusy)
            {
                //Stop/Cancel the async operation here
                sbWorker.CancelAsync();
                switch (ElementoPilotato)
                {
                    case ControlledDevice.Nessuno:
                        break;
                    case ControlledDevice.SpyBatt:
                        sbLocale.RichiestaInterruzione = true; 
                        break;
                    case ControlledDevice.Display:
                        break;
                    case ControlledDevice.LadeLight:
                        llLocale.RichiestaInterruzione = true;
                        sbWorker.CancelAsync();
                        break;
                    case ControlledDevice.Desolfatatore:
                        break;
                    default:
                        break;
                }
                
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAvanzamentoCicli_Shown(object sender, EventArgs e)
        {
            Log.Debug("Show Form Avanzamento");
            switch (ElementoPilotato)
            {
                case ControlledDevice.Nessuno:
                    break;
                case ControlledDevice.SpyBatt:
                    sbLocale.Step += new UnitaSpyBatt.StepHandler(stepRispostaSB);
                    break;
                case ControlledDevice.Display:
                    break;
                case ControlledDevice.LadeLight:
                    llLocale.Step += new CaricaBatteria.StepHandler(stepRispostaLL);
                    break;
                case ControlledDevice.Desolfatatore:
                    break;
                default:
                    break;
            }
            
            AvviaRicalcolo();
        }

        private void btnStartLettura_Click(object sender, EventArgs e)
        {
            AvviaRicalcolo();
        }

        private void btnChiudi_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAvanzamentoCicli_FormClosed(object sender, FormClosedEventArgs e)
        {
            Log.Debug("Closed");
            //sbWorker.Dispose();
            //Log.Debug("sbWorker.Dispose()");
            //this.Dispose();
        }

        private void frmAvanzamentoCicli_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Debug("Closing");
           //sbWorker.Dispose();
           // Log.Debug("sbWorker.Dispose()");
           // this.Dispose();
        }





    }
}
