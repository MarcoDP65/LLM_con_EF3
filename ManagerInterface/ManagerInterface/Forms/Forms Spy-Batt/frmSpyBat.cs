using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using FTD2XX_NET;

using NextUI.Component;
using NextUI.Frame;

using BrightIdeasSoftware;
using Utility;
using MoriData;
using ChargerLogic;
using MdiHelper;
using log4net;
using log4net.Config;



namespace PannelloCharger
{
    public partial class frmSpyBat : Form
    {
        const int TabParametri = 6;
        public const int NumGrafici = 20;
        public enum IdGrafico : int { };
        parametriSistema _parametri;
        MessaggioSpyBatt _msg;
        LogicheBase _logiche;
        BindingSource bindingSource = new BindingSource();
        bool _apparatoPresente = false;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialogSB;

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        private frmAvanzamentoCicli _avCicli = new frmAvanzamentoCicli();
        //static 
        UnitaSpyBatt _sb;
        StatMemLungaSB _stat;
        TabControl TbcStatSettimane;

        // Indicatori Cockpit
        private IndicatoreCruscotto Ic11;
        private IndicatoreCruscotto Ic12;
        private IndicatoreCruscotto Ic13;
        private IndicatoreCruscotto Ic14;
        private IndicatoreCruscotto Ic21;
        private IndicatoreCruscotto Ic22;
        private IndicatoreCruscotto Ic23;
        private IndicatoreCruscotto Ic24;

        /*--------------------*/
        private OxyPlot.PlotModel grCompSOH;
        private OxyPlot.PlotModel grCompRabb;
        private OxyPlot.PlotModel grCompEnConsumata;
        private OxyPlot.PlotModel grCompCO2;

        private OxyPlot.WindowsForms.PlotView oxyContainerGrSingolo;
        private OxyPlot.WindowsForms.PlotView oxyContainerGrAnalisi;
        private OxyPlot.WindowsForms.PlotView oxyContainerGrCalVerifica;
        private OxyPlot.PlotModel oxyGraficoSingolo;
        private OxyPlot.PlotModel oxyGraficoAnalisi;
        private OxyPlot.PlotModel oxyGraficoCalVerifica;

        private OxyPlot.PlotModel due;
        public frmAlimentatore Lambda;


        // delegate is used to write to a UI control from a non-UI thread
        public delegate void SetTextDeleg(string text1);

        private bool _hidePhasesButtons = true;

        public System.Collections.Generic.List<mrDataPoint> ValoriPuntualiGrafico = new List<mrDataPoint>();

        public System.Collections.Generic.List<sbAnalisiCorrente> ValoriTestCorrente = new List<sbAnalisiCorrente>();
        public System.Collections.Generic.List<mrDataPoint> ValoriPuntualiGrCorrenti = new List<mrDataPoint>();
        public System.Collections.Generic.List<mrDataPoint> ValoriPuntualiGrCalCorrenti = new List<mrDataPoint>();
        public System.Collections.Generic.List<StepCarica> PassiStrategia = new List<StepCarica>();

        private ParametriSetupPro _parametriPro = new ParametriSetupPro();

        public string IdCorrente;
        /// <summary>
        /// Flag per la gestione della fase di caricamento. true finche in caricamento; usato per prevenire azioni sull'onChange in fase di apertura
        /// </summary>
        private bool _onUpload = true;

        private bool _datiSalvati = true;

        /// <summary>
        /// Evento attivato al momento della variazione del flag _datiSalvati.
        /// </summary>
        public event EventHandler<DatiCambiatiEventArgs> DatiCambiati;


        public frmSpyBat(ref parametriSistema _par, bool CaricaDati, string IdApparato, LogicheBase Logiche, bool SerialeCollegata, bool AutoUpdate)
        {
            bool _esito;
            try
            {
                bool _aggiornaStatistiche = false;
                Cursor.Current = Cursors.WaitCursor;
                _onUpload = true;
                _parametri = _par;
                System.Threading.Thread.CurrentThread.CurrentUICulture = _parametri.currentCulture;

                InitializeComponent();
                inizializzaComboPro();
                tabCaricaBatterie.DrawMode = TabDrawMode.OwnerDrawFixed;
                // Setto il bordo invisibile ai due groupbox dell'upd FW
                grbFWdettUpload.Paint += PaintBorderlessGroupBox;
                grbFWDettStato.Paint += PaintBorderlessGroupBox;


                InizializzaOxyGrAnalisi();
                InizializzaOxyGrCalibrazione();
                ResizeRedraw = true;
                _logiche = Logiche;
                cmbFSerBaudrateOC.DataSource = ListaBrSig;
                cmbFSerBaudrateOC.DisplayMember = "descrizione";
                cmbFSerBaudrateOC.ValueMember = "BrSettingValue";

                cmbFSerEchoOC.DataSource = ListaEchoSig;
                cmbFSerEchoOC.DisplayMember = "descrizione";
                cmbFSerEchoOC.ValueMember = "EcSettingValue";

                Log.Debug("----------------------- frmSpyBat ---------------------------");

                _msg = new MessaggioSpyBatt();
                _sb = new UnitaSpyBatt(ref _parametri, _logiche.dbDati.connessione, _logiche.currentUser.livello);

                _stat = new StatMemLungaSB();
                string _idCorrente = IdApparato;
                abilitaSalvataggi(false);
                txtRevSWStratSb.Text = "";

                // in futuro, inserire quì il precaricamento delle statistiche
                //CaricaTestata(IdApparato, Logiche, SerialeCollegata);

                // 12/10/15: inizio leggendo lo stato del bootloader, per verificare se c'è un firmware caricato
                _esito = CaricaStatoFirmware(ref IdApparato, Logiche, SerialeCollegata);
                if (!_esito)
                {
                    // Se non ho il firmware state potrebbe essere una versione precedente
                    // provo a leggere la testata
                    _esito = ApriComunicazione(IdApparato, Logiche, SerialeCollegata);
                    if (IdApparato != "")
                    {
                        CaricaTestata(IdApparato, Logiche, SerialeCollegata);
                    }

                }
                else
                {

                    if (_sb.FirmwarePresente)
                    {
                        // Se sono in stato BL lo evidenzio e mi fermo, altrimenti leggo la testata
                        if ((_sb.StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.BootLoaderInUso) == (byte)FirmwareManager.MascheraStato.BootLoaderInUso)
                        {
                            MostraTestata();
                            txtRevSWSb.Text = "BOOTLOADER";
                            txtRevSWSb.ForeColor = Color.Red;
                            _apparatoPresente = _sb.apparatoPresente;
                            abilitaSalvataggi(_sb.apparatoPresente);
                            Log.Info("Stato scheda SPY-BATT: LD OK, MODO BOOTLOADER ");
                        }
                        else
                        {
                            //TODO: gestire io DB se la scheda è già in archivio
                            CaricaTestata(IdApparato, Logiche, SerialeCollegata);
                        }

                        if (_sb.sbData.Bootloader != _sb.StatoFirmware.RevFirmware)
                            _sb.sbData.Bootloader = _sb.StatoFirmware.RevFirmware;

                        txtRevSWSb.Text = _sb.StatoFirmware.RevFirmware;
                        string _verStrategia = _sb.VersioneStrategia();

                        if (_sb.sbData.StrategyLibrary != _verStrategia)
                            _sb.sbData.StrategyLibrary = _verStrategia;
                        _sb.sbData.salvaDati();
                        txtRevSWStratSb.Text = _verStrategia;


                    }
                    else
                    {
                        MostraTestata();
                    }
                }

                IdCorrente = _sb.Id;
                _apparatoPresente = SerialeCollegata;
                // se l'apparato è presente e le configurazioni su scheda superano quelle in mem, aggiorno
                if (_apparatoPresente)
                    if (_sb.sbData.ProgramCount != _sb.Programmazioni.Count)
                    {

                        RicaricaProgrammazioni();
                    }

                CaricaProgrammazioni();


                if ((_sb.sbData.LongMem > 0) && (_sb.sbData.LongMem < 30000)) _aggiornaStatistiche = true;

                InizializzaOxyGrSingolo();
                //InizializzaOxyGrCalibrazione();
                applicaAutorizzazioni();
                if (_aggiornaStatistiche) RicalcolaStatistiche();
                InizializzaCalibrazioni();
                InizializzaVistaCorrenti();
                InizializzaVistaCorrentiCal();
                if (_aggiornaStatistiche) CaricaComboCalibrazioni();
                if (_aggiornaStatistiche) CaricaComboGrafici();
                _onUpload = false;
                _datiSalvati = true;

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

        }


        public void RileggiSpyBat()
        {
            bool _esito;
            try
            {
                /*

                Non reinizializzo i componenti

                InitializeComponent();
                // Setto il bordo invisibile ai due groupbox dell'upd FW
                grbFWdettUpload.Paint += PaintBorderlessGroupBox;
                grbFWDettStato.Paint += PaintBorderlessGroupBox;
                InizializzaOxyGrAnalisi();
                ResizeRedraw = true;

                Log.Debug("----------------------- frmSpyBat ---------------------------");

                */

                string IdApparato = IdCorrente;
                LogicheBase Logiche = _logiche;
                bool SerialeCollegata = _apparatoPresente;



                _msg = new MessaggioSpyBatt();
                _sb = new UnitaSpyBatt(ref _parametri, _logiche.dbDati.connessione, _logiche.currentUser.livello);

                _stat = new StatMemLungaSB();
                abilitaSalvataggi(false);

                // in futuro, inserire quì il precaricamento delle statistiche
                //CaricaTestata(IdApparato, Logiche, SerialeCollegata);

                // 12/10/15: inizio leggendo lo stato del bootloader, per verificare se c'è un firmware caricato
                _esito = CaricaStatoFirmware(ref IdApparato, Logiche, SerialeCollegata);
                if (!_esito)
                {
                    // Se non ho il firmware state potrebbe essere una versione precedente
                    // provo a legere la testata
                    _esito = ApriComunicazione(IdApparato, Logiche, SerialeCollegata);
                    if (IdApparato != "")
                    {
                        CaricaTestata(IdApparato, Logiche, SerialeCollegata);
                    }

                }
                else
                {

                    if (_sb.FirmwarePresente)
                    {
                        // Se sono in stato BL lo evidenzio e mi fermo, altrimenti leggo la testata
                        if ((_sb.StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.BootLoaderInUso) == (byte)FirmwareManager.MascheraStato.BootLoaderInUso)
                        {
                            MostraTestata();
                            txtRevSWSb.Text = "BOOTLOADER";
                            txtRevSWSb.ForeColor = Color.Red;
                            Log.Info("Stato scheda SPY-BATT: LD OK, MODO BOOTLOADER ");
                        }
                        else
                        {
                            //TODO: gestire io DB se la scheda è già in archivio
                            CaricaTestata(IdApparato, Logiche, SerialeCollegata);
                        }


                    }
                    else
                    {
                        MostraTestata();
                    }
                }

                IdCorrente = _sb.Id;
                _apparatoPresente = SerialeCollegata;
                // se l'apparato è presente e le configurazioni su scheda superano quelle in mem, aggiorno
                if (_apparatoPresente)
                    if (_sb.sbData.ProgramCount != _sb.Programmazioni.Count)
                    {

                        RicaricaProgrammazioni();
                    }

                CaricaProgrammazioni();

                InizializzaOxyGrSingolo();
                applicaAutorizzazioni();
                RicalcolaStatistiche();
                InizializzaCalibrazioni();
                InizializzaVistaCorrenti();
                InizializzaVistaCorrentiCal();
                CaricaComboCalibrazioni();
                CaricaComboGrafici();
                _datiSalvati = true;

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }

        }


        public frmSpyBat()
        {
            Log.Debug("----------------------- frmSpyBat  Easy ---------------------------");
            InitializeComponent();

            // Setto il bordo invisibile ai due groupbox dell'upd FW
            grbFWdettUpload.Paint += PaintBorderlessGroupBox;
            grbFWDettStato.Paint += PaintBorderlessGroupBox;

            InizializzaOxyGrAnalisi();
            InizializzaOxyGrSingolo();
            InizializzaOxyGrCalibrazione();
            RicalcolaStatistiche();
            InizializzaCalibrazioni();
            InizializzaVistaCorrenti();
            InizializzaVistaCorrentiCal();
            CaricaComboCalibrazioni();
            CaricaComboGrafici();
        }

        /// <summary>
        /// Attiva/Disattiva i pulsanti comando in base all'effettivo collegamento dell'apparato
        /// </summary>
        /// <param name="_stato">
        /// True se l'apparato è effettivamente collegato
        /// </param>
        private void abilitaSalvataggi(bool _stato)
        {
            btnRicaricaDati.Enabled = _apparatoPresente;
            btnSalvaCliente.Enabled = _apparatoPresente;
            txtCliente.ReadOnly = !_stato;
            txtNoteCliente.ReadOnly = !_stato;
            txtModelloBat.ReadOnly = !_stato;
            txtMarcaBat.ReadOnly = !_stato;
            txtIdBat.ReadOnly = !_stato;
            btnLeggiRtc.Enabled = _apparatoPresente;
            btnScriviRtc.Enabled = _apparatoPresente;
            btnRicaricaProgr.Enabled = _apparatoPresente;
            btnAttivaProgrammazione.Enabled = _apparatoPresente;
            btnNuovoProgramma.Enabled = _apparatoPresente;
            btnDumpMemoria.Enabled = _apparatoPresente;
            btnCaricaListaLunghi.Enabled = _apparatoPresente;
            btnCaricaListaUltimiLunghi.Enabled = _apparatoPresente;
            btnCaricaDettaglioSel.Enabled = _apparatoPresente;
            btnLeggiVariabili.Enabled = _apparatoPresente;
            chkParLetturaAuto.Enabled = _apparatoPresente;
            grbVarResetScheda.Enabled = _apparatoPresente;
            grbFWAggiornamento.Enabled = _apparatoPresente;
            grbFwAttivazioneArea.Enabled = _apparatoPresente;
            grbStatoFirmware.Enabled = _apparatoPresente;


        }


        public bool reconnectSpyBat()
        {
            bool _esito;
            try
            {

                //_parametri = _par;
                //System.Threading.Thread.CurrentThread.CurrentUICulture = _parametri.currentCulture;

                //InitializeComponent();
                //InizializzaOxyGrAnalisi();
                //ResizeRedraw = true;
                //_logiche = Logiche;

                Log.Debug("----------------------- reconnectSpyBat ---------------------------");

                //_msg = new MessaggioSpyBatt();
                //_sb = new UnitaSpyBatt(ref _parametri, _logiche.dbDati.connessione);

                //_stat = new StatMemLungaSB();
                string _idCorrente = _sb.Id;
                abilitaSalvataggi(false);

                // in futuro, inserire quì il precaricamento delle statistiche
                //CaricaTestata(IdApparato, Logiche, SerialeCollegata);

                // 12/10/15: inizio leggendo lo stato del bootloader, per verificare se c'è un firmware caricato
                _esito = CaricaStatoFirmware(ref _idCorrente, _logiche, _apparatoPresente);
                if (!_esito)
                {
                    // Se non ho il firmware state potrebbe essere una versione precedente
                    // provo a legere la testata
                    _esito = ApriComunicazione(_idCorrente, _logiche, _apparatoPresente);
                    if (_idCorrente != "")
                    {
                        CaricaTestata(_idCorrente, _logiche, _apparatoPresente);
                    }

                }
                else
                {

                    if (_sb.FirmwarePresente)
                    {
                        // Se sono in stato BL lo evidenzio e mi fermo, altrimenti leggo la testata
                        if ((_sb.StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.BootLoaderInUso) == (byte)FirmwareManager.MascheraStato.BootLoaderInUso)
                        {
                            MostraTestata();
                            txtRevSWSb.Text = "BOOTLOADER";
                            txtRevSWSb.ForeColor = Color.Red;

                            // se l'apparato è collegato abilito i salvataggi
                            abilitaSalvataggi(_sb.apparatoPresente);

                            Log.Info("Stato scheda SPY-BATT: LD OK, MODO BOOTLOADER ");

                        }
                        else
                        {
                            //TODO: gestire io DB se la scheda è già in archivio
                            CaricaTestata(_idCorrente, _logiche, _apparatoPresente);
                        }


                    }
                    else
                    {
                        MostraTestata();
                    }
                }

                IdCorrente = _sb.Id;


                InizializzaOxyGrSingolo();
                applicaAutorizzazioni();
                RicalcolaStatistiche();
                InizializzaCalibrazioni();
                InizializzaVistaCorrenti();
                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
                return false;
            }

        }


        private void PaintBorderlessGroupBox(object sender, PaintEventArgs p)
        {
            GroupBox box = (GroupBox)sender;
            p.Graphics.Clear(box.BackColor);//SystemColors.Control);
            p.Graphics.DrawString(box.Text, box.Font, Brushes.Black, 0, 0);
        }


        public bool caricaDati(string IdApparato, LogicheBase Logiche, bool SerialeCollegata)
        {
            bool _risposta = false;
            try
            {
                bool _esito;
                bool _connessioneAttiva = false;

                Log.Debug("----------------------- caricaDati ---------------------------");

                //_sb = new UnitaSpyBatt(ref _parametri, Logiche.dbDati.connessione);
                string _idCorrente = IdApparato;

                if (SerialeCollegata)
                {
                    _msg = new MessaggioSpyBatt();
                    _esito = _sb.apriPorta();
                    if (!_esito)
                    {
                        MessageBox.Show(_parametri.lastError, "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.Hide();  //Close();
                        return false;
                    }
                    byte[] vuoto = { 1, 2, 3, 4, 5, 6, 7, 8 };
                    _sb.numeroSeriale = vuoto;
                    _esito = _sb.VerificaPresenza();

                    if (_esito)
                    {
                        _idCorrente = _sb.Id;
                        _connessioneAttiva = true;
                        //MessageBox.Show("ID Corrente: " + FunzioniMR.StringaSeriale(_idCorrente), "Apparato Collegato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Log.Info("Apparato Collegato: ID Corrente: " + FunzioniMR.StringaSeriale(_idCorrente));
                    }

                    //verificato il codice carico i dati da DB e li aggiorno con quelli da seriale.

                }

                _sb.CaricaTestata(_idCorrente, _connessioneAttiva);

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("caricaDati " + Ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Provo ad aprire la porta di comunicazione (HW) e ad inizializzare il canale
        /// </summary>
        /// <param name="IdApparato"></param>
        /// <param name="Logiche"></param>
        /// <param name="SerialeCollegata"></param>
        /// <returns></returns>
        public bool ApriComunicazione(string IdApparato, LogicheBase Logiche, bool SerialeCollegata)
        {
            bool _risposta = false;
            try
            {
                bool _esito;
                bool _connessioneAttiva = false;

                Log.Debug("----------------------- ApriComunicazione ---------------------------");

                string _idCorrente = IdApparato;

                if (SerialeCollegata)
                {
                    _msg = new MessaggioSpyBatt();
                    _esito = _sb.apriPorta();
                    if (!_esito)
                    {
                        MessageBox.Show(_parametri.lastError, "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.Hide();  //Close();
                        return false;
                    }
                    byte[] vuoto = { 1, 2, 3, 4, 5, 6, 7, 8 };
                    _sb.numeroSeriale = vuoto;
                    _esito = _sb.VerificaPresenza();
                    if (_esito)
                    {
                        _idCorrente = _sb.Id;
                        _connessioneAttiva = true;

                        Log.Info("Apparato Collegato: ID Corrente: " + FunzioniMR.StringaSeriale(_idCorrente));
                    }


                }
                else
                {
                    _sb.CaricaTestata(_idCorrente, _connessioneAttiva);
                }


                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("caricaDati " + Ex.Message);
                return false;
            }
        }


        public bool MostraTestata()
        {
            try
            {
                txtMatrSB.Text = FunzioniMR.StringaSeriale(_sb.Id);
                txtRevHWSb.Text = _sb.sbData.HwVersion.ToString();
                txtRevSWSb.ForeColor = Color.Black;

                if (_sb.StatoFirmware.valido)
                {
                    //scheda con booloader presente
                    txtRevLdrSb.Text = _sb.StatoFirmware.strRevBootloader;
                    txtRevSWSb.Text = _sb.StatoFirmware.strRevFirmware;

                    //TODO: caricare e mostrare versione libreria
                }
                else
                {
                    //scheda senza booloader
                    if (_sb.sbData != null)
                    {
                        if (_sb.sbData.SwVersion != null)
                            txtRevSWSb.Text = _sb.sbData.SwVersion.ToString();
                        else
                            txtRevSWSb.Text = "N.D.";

                        txtRevLdrSb.Text = _sb.sbData.Bootloader;
                        txtRevSWStratSb.Text = _sb.sbData.StrategyLibrary;

                    }
                    else
                    {
                        txtRevSWSb.Text = "N.D.";
                    }


                }



                txtManufcturedBy.Text = _sb.sbData.ProductId;

                txtMainNumLunghi.Text = _sb.sbData.LongMem.ToString();
                txtMainNumProgr.Text = _sb.sbData.ProgramCount.ToString();

                txtEventiCSLunghi.Text = _sb.sbData.LongMem.ToString();
                txtCicliProgrammazione.Text = _sb.sbData.ProgramCount.ToString();
                txtTestataIdBase.Text = _sb.sbData.IdBase;
                txtTestataNumClone.Text = _sb.sbData.NumeroClone.ToString();
                txtTestataDataClone.Text = "";
                if (_sb.sbData.DataClone != DateTime.MinValue)
                    txtTestataDataClone.Text = _sb.sbData.DataClone.ToShortDateString() + " " + _sb.sbData.DataClone.ToShortTimeString();
                txtTestataNoteClone.Text = _sb.sbData.NoteClone;

                // Carico l'area Contatori - Solo dal comando Testata
                // Prima vuoto le tb
                txtTestataContLunghi.Text = "";
                txtTestataPtrLunghi.Text = "";
                txtTestataContBrevi.Text = "";
                txtTestataPtrBrevi.Text = "";
                txtTestataContProgr.Text = "";
                txtTestataPtrProgr.Text = "";

                if (_sb.IntestazioneSb != null)
                {
                    txtTestataContLunghi.Text = _sb.IntestazioneSb.longRecordCounter.ToString();
                    txtTestataPtrLunghi.Text = _sb.IntestazioneSb.longRecordPoiter.ToString();
                    txtTestataContBrevi.Text = _sb.IntestazioneSb.shortRecordCounter.ToString();
                    txtTestataPtrBrevi.Text = _sb.IntestazioneSb.shortRecordPointer.ToString();
                    txtTestataContProgr.Text = _sb.IntestazioneSb.numeroProgramma.ToString();
                }

                return true;
            }

            catch (Exception Ex)
            {
                Log.Error("MostraTestata: " + Ex.Message);
                return false;
            }

        }


        public void applicaAutorizzazioni()
        {
            try
            {
                bool _enabled;
                bool _readonly;
                int LivelloCorrente;
                if (_logiche.currentUser != null)
                {
                    LivelloCorrente = _logiche.currentUser.livello;
                }
                else
                {
                    LivelloCorrente = 99;
                }
                // Tab Generale
                #region "Tab Generale"
                if (LivelloCorrente < 3) _readonly = false; else _readonly = true;
                txtCliente.ReadOnly = _readonly;
                txtMarcaBat.ReadOnly = _readonly;
                txtModelloBat.ReadOnly = _readonly;
                txtIdBat.ReadOnly = _readonly;
                txtNoteCliente.ReadOnly = _readonly;
                btnSalvaCliente.Visible = (_readonly == false);
                txtSerialNumber.ReadOnly = _readonly;

                grbMainDlOptions.Visible = false;
                //btnResetScheda.Visible = false;
                grbTestataContatori.Visible = false;
                grbAbilitazioneReset.Visible = false;
                grbCloneScheda.Visible = false;
                txtSerialNumber.ReadOnly = true;
                if (LivelloCorrente == 0)
                {
                    grbMainDlOptions.Visible = true;
                    //btnResetScheda.Visible = true;
                    grbTestataContatori.Visible = true;
                    grbAbilitazioneReset.Visible = true;
                    grbCloneScheda.Visible = true;
                    txtSerialNumber.ReadOnly = false;
                }

                #endregion

                #region "Tab Memoria Cicli"

                _hidePhasesButtons = true;
                _readonly = true;

                grbEsportaExcel.Visible = false;
                grbMemCicliReadMem.Visible = false;
                grbMemCicliContatori.Visible = false;
                grbMemCicliPulsanti.Visible = false;


                if (LivelloCorrente < 2)
                {
                    _readonly = false;
                    grbEsportaExcel.Visible = true;
                    grbMemCicliReadMem.Visible = true;
                    grbMemCicliContatori.Visible = true;
                    grbMemCicliPulsanti.Visible = true;
                    btnCaricaListaLunghi.Enabled = true;
                    btnCaricaListaUltimiLunghi.Enabled = true;
                    chkCaricaBrevi.Enabled = true;

                }

                /*    
                _enabled = (_readonly == false);
                grbMemCicliReadMem.Visible = _enabled;
                chkCaricaBrevi.Visible = _enabled;
                chkInvertiCorreti.Visible = _enabled;
                btnCaricaListaLunghi.Enabled = _enabled;
                //carica ultimi attivo per tutti
                //btnCaricaListaUltimiLunghi.Enabled = true;
                btnCaricaDettaglioSel.Enabled = _enabled;
                */

                #endregion

                #region "Sonda Termica"
                if (LivelloCorrente < 2)
                    _readonly = false;
                else
                {
                    tabCaricaBatterie.TabPages.Remove(tabCb03);
                    _readonly = true;
                }
                _enabled = (_readonly == false);
                #endregion

                #region "Statistiche"
                if (LivelloCorrente < 2)
                    _readonly = false;
                else
                {
                    tbcStatistiche.TabPages.Remove(tabStatSoglie);
                    _readonly = true;
                }
                _enabled = (_readonly == false);
                #endregion

                #region "Orologio"
                if (LivelloCorrente < 3)
                    _readonly = false;
                else
                {
                    _readonly = true;
                }
                _enabled = (_readonly == false);
                grbAccensione.Visible = false;
               
                if (LivelloCorrente > 2)
                {
                    btnScriviRtc.Enabled = false;
                   
                }
                if (LivelloCorrente > 1)
                {
                   
                    grbCalData.Visible = false;
                }
                #endregion

                #region "Programmazioni"
                btnNuovoProgramma.Visible = false;
                btnAttivaProgrammazione.Visible = false;

                switch (LivelloCorrente)
                {
                    case 0:
                    case 1:
                        btnNuovoProgramma.Visible = true;
                        btnAttivaProgrammazione.Visible = true;
                        grbProgrammazione.Height = 199;
                        grbProgrImpianto.Height = 128;
                        grbProgrImpianto.Top = 228;
                       // grbProgrImpianto.Height = 176;
                       // grbProgrImpianto.Top = 228;

                        break;
                    case 2:
                        btnNuovoProgramma.Visible = true;
                        grbProgrammazione.Height = 108;
                        grbProgrImpianto.Height = 76;
                        grbProgrImpianto.Top = 147;
                        flvwProgrammiCarica.Height = 300;
                        flvwProgrammiCarica.Top = 235;

                        break;
                }



                #endregion

                #region "Variabili"
                if (LivelloCorrente < 1)
                    _readonly = false;
                else
                {
                    _readonly = true;
                }
                _enabled = (_readonly == false);
                chkDatiDiretti.Visible = _enabled;
                grbVariabiliConnVOK.Visible = _enabled;
                btnDumpMemoria.Visible = _enabled;
                btnStampaScheda.Visible = _enabled;

                grbCalibrazionePulsanti.Visible = _enabled;
                grbCalibrazioni.Visible = _enabled;
                grbSvcParametriMedie.Visible = _enabled;
                chkDatiDiretti.Visible = _enabled;
                btnPianRefresh.Visible = _enabled;
                btnPianSalvaCliente.Visible = _enabled;
                grbProgParEqual.Visible = _enabled;
                if (_sb.sbData.fwLevel >= 6)
                    grbVarParametriSig.Visible = _enabled;
                else
                    grbVarParametriSig.Visible = false;


                if (LivelloCorrente > 2)
                    grbVarResetScheda.Visible = false;


                #endregion

                #region "Accesso Memoria"
                // accessibile solo a Factory e service
                if (LivelloCorrente < 2)
                {
                    _readonly = false;
                    if (LivelloCorrente < 1) grbMemScrittura.Enabled = true;
                    else grbMemScrittura.Enabled = false;
                }
                else
                {
                    tabCaricaBatterie.TabPages.Remove(tabMemRead);
                    _readonly = true;
                }
                _enabled = (_readonly == false);
                #endregion

                #region "Verifica"
                // accessibile solo a Factory 
                if (LivelloCorrente > 0)
                {
                    tabCaricaBatterie.TabPages.Remove(tbpCalibrazioni);
                    _readonly = true;
                }

                _enabled = (_readonly == false);
                #endregion

                #region "Clonatura"
                // accessibile solo a Factory 
                if (LivelloCorrente > 0)
                {
                    tabCaricaBatterie.TabPages.Remove(tbpClonaScheda);
                    _readonly = true;
                }

                _enabled = (_readonly == false);
                #endregion

                #region "Firmware"
                // accessibile a power user nuovo file solo a Factory 


                if (LivelloCorrente > 2)
                {
                    tabCaricaBatterie.TabPages.Remove(tbpFirmware);
                    _readonly = true;
                }
                else
                {
                    _readonly = false;
                    grbStatoFirmware.Visible = true;
                    grbFWAggiornamento.Visible = true;
                    // Nascondo tutto poi riattvo se serve
                    grbFwAttivazioneArea.Visible = false;
                    GrbFWArea1.Visible = false;
                    grbFWArea2.Visible = false;
                    grbFWPreparaFile.Visible = false;
                    grbFWDettStato.Visible = true;
                    grbFWdettUpload.Visible = true;

                    if (LivelloCorrente < 2)
                    {
                        // Factory o service
                        grbFwAttivazioneArea.Visible = true;
                        GrbFWArea1.Visible = true;
                        grbFWArea2.Visible = true;
                        grbFWDettStato.Visible = false;
                        grbFWdettUpload.Visible = false;
                    }
                    if (LivelloCorrente < 1)
                    {
                        // Factory
                        grbFWPreparaFile.Visible = true;
                    }
                }

                _enabled = (_readonly == false);
                #endregion

                #region "Calibrazione"
                // accessibile solo a Factory 
                if (LivelloCorrente > 0)
                {
                    tabCaricaBatterie.TabPages.Remove(tabCalibrazione);
                    _readonly = true;
                }

                _enabled = (_readonly == false);
                #endregion

                #region "Strategia"
                // accessibile solo a Factory 
                if (LivelloCorrente > 0)
                {
                    tabCaricaBatterie.TabPages.Remove(tbpStrategia);
                    _readonly = true;
                }

                _enabled = (_readonly == false);
                #endregion


            }
            catch (Exception Ex)
            {
                Log.Error("applicaAutorizzazioni: " + Ex.Message);
            }

        }


        public bool CaricaTestata(string IdApparato, LogicheBase Logiche, bool SerialeCollegata)
        {
            bool _esito;
            try
            {
                Log.Info("Apertura scheda SPY-BATT " + IdApparato);
                // _esito = caricaDati(IdApparato, Logiche, SerialeCollegata);
                _esito = _sb.VerificaPresenza();

                IdApparato = _sb.Id;

                _esito = _sb.CaricaTestata(IdApparato, SerialeCollegata);

                if (_sb.UltimaRispostaRicevuta == SerialMessage.TipoRisposta.Nack)
                {
                    // ho il bootloader ma non la app attiva
                    MostraTestata();
                    txtRevSWSb.Text = "N.D.";
                    txtRevSWSb.ForeColor = Color.Red;
                    Log.Info("Stato scheda SPY-BATT: LD OK, APP KO ");
                    return false;


                }


                if (_esito)
                {
                    IdApparato = _sb.Id;
                    MostraTestata();

                    txtTestataPtrProgr.Text = _sb.sbData.NumeroCloni().ToString();

                    CaricaProgrammazioni();
                    CaricaCicli();

                    if (CaricaCliente(IdApparato, Logiche, SerialeCollegata)) mostraCliente();
                    _apparatoPresente = _sb.apparatoPresente;
                    // se l'apparato è collegato abilito i salvataggi
                    abilitaSalvataggi(_apparatoPresente);

                    _sb.CaricaParametri(_sb.Id, _apparatoPresente);
                    MostraParametriGenerali();

                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
                return false;
            }
        }


        public bool CaricaCliente(string IdApparato, LogicheBase Logiche, bool SerialeCollegata)
        {
            try
            {
                bool _esito;
                bool _connessioneAttiva = false;

                //_sb = new UnitaSpyBatt(ref _parametri, Logiche.dbDati.connessione);
                string _idCorrente = IdApparato;

                if (SerialeCollegata)
                {
                    _connessioneAttiva = true;
                }

                _sb.CaricaDatiCliente(_idCorrente, _connessioneAttiva);

                // carico anche i  parametri di pianificazione
                _esito = AllineaComboPianificazione(_sb.sbCliente.ModoPianificazione);
                _esito = MostraPianificazione(false);

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("caricaClente " + Ex.Message);
                return false;
            }
        }


        private bool AllineaComboPianificazione(byte IdAttivo)
        {
            bool _esito = false;
            try
            {
                bool _tempUpload = _onUpload;
                _onUpload = true;

                foreach (Pianificazione _tempP in cmbModoPianificazione.Items)
                {
                    if (_tempP.Codice == IdAttivo)
                    {
                        cmbModoPianificazione.SelectedItem = _tempP;
                        _esito = true;
                        break;
                    }
                }
                _onUpload = _tempUpload;
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("AllineaComboPianificazione " + Ex.Message);
                return _esito;
            }

        }


        private void ListaLunghiCambiata(UnitaSpyBatt usb, sbListaLunghiEvt sbe)
        {
            try
            {
                Log.Debug("HEARD Listalunghi cambiata: " + sbe.EventiLunghi.ToString() + " righe");
                // flvwCicliBatteria.RedrawItems(1, sbe.EventiLunghi+1, true);
                // Application.DoEvents();
            }

            catch (Exception Ex)
            {
                Log.Error("ListaLunghiCambiata " + Ex.Message);
            }

        }


        private void mostraCliente()
        {
            try
            {
                txtCliente.Text = _sb.sbCliente.Client;
                txtNoteCliente.Text = _sb.sbCliente.ClientNote;
                txtMarcaBat.Text = _sb.sbCliente.BatteryBrand;
                txtModelloBat.Text = _sb.sbCliente.BatteryModel;
                txtIdBat.Text = _sb.sbCliente.BatteryId;
                txtCliCicliAttesi.Text = _sb.sbCliente.CicliAttesi.ToString();
                txtSerialNumber.Text = _sb.sbCliente.SerialNumber;
                txtCliCodiceLL.Text = _sb.sbCliente.BatteryLLId;

                if (_sb.sbCliente.BatteryId != "")
                {
                    this.Text = _sb.sbCliente.BatteryId;
                }

            }

            catch (Exception Ex)
            {
                Log.Error("mostraCliente: " + Ex.Message);
            }
        }


        private void CaricaCicli()
        {

            bool _esito;
            try
            {
                //Vuoto la tabella

                _esito = _sb.CaricaCicliMemLunga(_sb.Id, _logiche.dbDati.connessione);
                Log.Info("CaricaCicli: dopo " + _sb.CicliMemoriaLunga.Count.ToString());
                InizializzaVistaLunghi();


            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }


        }


        private void CaricaVariabili()
        {

            bool _esito;
            try
            {
                //Vuoto la tabella

                _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                Log.Info("CaricaVariabili: " + _sb.sbVariabili.IstanteLettura.ToString());



            }
            catch (Exception Ex)
            {
                Log.Error("CaricaVariabili: " + Ex.Message);
            }


        }

        /// <summary>
        /// Mostra il valore delle variabili istantanee nella relativa tab.
        /// </summary>
        /// <param name="DatiPresenti">if set to <c>true</c> [dati presenti].</param>
        /// <param name="ValoriReali">Se <c>true</c> mostra i dati nel formato originale.</param>
        private void MostraVariabili(bool DatiPresenti, bool ValoriReali = false)
        {

            try
            {
                //Vuoto i campi

                txtVarVBattT.Text = "";
                txtVarVBatt.Text = "";
                txtVarV3.Text = "";
                txtVarV2.Text = "";
                txtVarV1.Text = "";
                txtVaIbatt.Text = "";
                txtVarAhCarica.Text = "";
                txtVarAhScarica.Text = "";
                txtVarTempNTC.Text = "";
                txtVarElettrolita.Text = "";
                txtVarSoC.Text = "";
                txtVarRF.Text = "";
                txtVarWhCarica.Text = "";
                txtVarWhScarica.Text = "";
                txtVarMemProgrammed.Text = "";
                grbVariabiliConnVbatt.CheckState = CheckState.Indeterminate;
                grbVariabiliConnV3.CheckState = CheckState.Indeterminate;
                grbVariabiliConnV2.CheckState = CheckState.Indeterminate;
                grbVariabiliConnV1.CheckState = CheckState.Indeterminate;
                grbVariabiliConnVOK.CheckState = CheckState.Indeterminate;


                if (DatiPresenti)
                {
                    if (ValoriReali)
                    {

                        // Prima i valori sempre validi
                        txtVarVBattT.Text = _sb.sbVariabili.TensioneTampone.ToString();
                        txtVarVBatt.Text = _sb.sbVariabili.TensioneIstantanea.ToString();
                        txtVarTempNTC.Text = _sb.sbVariabili.TempNTC.ToString();
                        //poi, se ho Vbatt
                        if (_sb.sbVariabili.TensioneIstantanea > 9)
                        {
                            txtVarV3.Text = _sb.sbVariabili.Tensione3.ToString();
                            txtVarV2.Text = _sb.sbVariabili.Tensione2.ToString();
                            txtVarV1.Text = _sb.sbVariabili.Tensione1.ToString();
                            txtVaIbatt.Text = _sb.sbVariabili.CorrenteBatteria.ToString();
                            txtVarAhCarica.Text = FunzioniMR.StringaCorrente((short)_sb.sbVariabili.AhCaricati);
                            txtVarAhScarica.Text = FunzioniMR.StringaCorrente((short)_sb.sbVariabili.AhScaricati);
                            txtVarElettrolita.Text = _sb.sbVariabili.PresenzaElettrolita.ToString();
                            txtVarSoC.Text = _sb.sbVariabili.SoC.ToString();
                            txtVarRF.Text = _sb.sbVariabili.RF.ToString();
                            txtVarWhCarica.Text = _sb.sbVariabili.WhCaricati.ToString();
                            txtVarWhScarica.Text = _sb.sbVariabili.WhScaricati.ToString();
                            txtVarMemProgrammed.Text = _sb.sbVariabili.MemProgrammed.ToString();
                            if ((_sb.sbVariabili.ConnectionStatus & 0x01) == 0x01) grbVariabiliConnVbatt.CheckState = CheckState.Checked;
                            else grbVariabiliConnVbatt.CheckState = CheckState.Unchecked;
                            if ((_sb.sbVariabili.ConnectionStatus & 0x02) == 0x02) grbVariabiliConnV3.CheckState = CheckState.Checked;
                            else grbVariabiliConnV3.CheckState = CheckState.Unchecked;
                            if ((_sb.sbVariabili.ConnectionStatus & 0x04) == 0x04) grbVariabiliConnV2.CheckState = CheckState.Checked;
                            else grbVariabiliConnV2.CheckState = CheckState.Unchecked;
                            if ((_sb.sbVariabili.ConnectionStatus & 0x08) == 0x08) grbVariabiliConnV1.CheckState = CheckState.Checked;
                            else grbVariabiliConnV1.CheckState = CheckState.Unchecked;
                            if ((_sb.sbVariabili.ConnectionStatus & 0x10) == 0x10) grbVariabiliConnVOK.CheckState = CheckState.Checked;
                            else grbVariabiliConnVOK.CheckState = CheckState.Unchecked;
                        }

                    }
                    else
                    {
                        txtVarVBattT.Text = _sb.sbVariabili.strTensioneTampone;
                        txtVarVBatt.Text = _sb.sbVariabili.strTensioneIstantanea;
                        txtVarTempNTC.Text = _sb.sbVariabili.strTempNTC;

                        //poi, se ho Vbatt
                        if (_sb.sbVariabili.TensioneIstantanea > 9)
                        {
                            txtVarV3.Text = _sb.sbVariabili.strTensione3;
                            txtVarV2.Text = _sb.sbVariabili.strTensione2;
                            txtVarV1.Text = _sb.sbVariabili.strTensione1;
                            txtVaIbatt.Text = _sb.sbVariabili.strCorrenteBatteria;
                            if (_sb.sbVariabili.CorrenteBatteria < 0) txtVaIbatt.ForeColor = Color.Red;
                            else txtVaIbatt.ForeColor = Color.Black;
                            txtVarAhCarica.Text = _sb.sbVariabili.strAhCaricati;
                            txtVarAhScarica.Text = _sb.sbVariabili.strAhScaricati;
                            txtVarElettrolita.Text = _sb.sbVariabili.strPresenzaElettrolita;
                            txtVarSoC.Text = _sb.sbVariabili.strSoC;
                            txtVarRF.Text = _sb.sbVariabili.strRF;
                            txtVarWhCarica.Text = _sb.sbVariabili.strWhCaricati;
                            txtVarWhScarica.Text = _sb.sbVariabili.strWhScaricati;
                            txtVarMemProgrammed.Text = _sb.sbVariabili.strMemProgrammed;

                            if ((_sb.sbVariabili.ConnectionStatus & 0x01) == 0x01) grbVariabiliConnVbatt.CheckState = CheckState.Checked;
                            else grbVariabiliConnVbatt.CheckState = CheckState.Unchecked;
                            if ((_sb.sbVariabili.ConnectionStatus & 0x02) == 0x02) grbVariabiliConnV3.CheckState = CheckState.Checked;
                            else grbVariabiliConnV3.CheckState = CheckState.Unchecked;
                            if ((_sb.sbVariabili.ConnectionStatus & 0x04) == 0x04) grbVariabiliConnV2.CheckState = CheckState.Checked;
                            else grbVariabiliConnV2.CheckState = CheckState.Unchecked;
                            if ((_sb.sbVariabili.ConnectionStatus & 0x08) == 0x08) grbVariabiliConnV1.CheckState = CheckState.Checked;
                            else grbVariabiliConnV1.CheckState = CheckState.Unchecked;
                            if ((_sb.sbVariabili.ConnectionStatus & 0x10) == 0x10) grbVariabiliConnVOK.CheckState = CheckState.Checked;
                            else grbVariabiliConnVOK.CheckState = CheckState.Unchecked;

                        }

                    }

                }

            }
            catch (Exception Ex)
            {
                Log.Error("MostraVariabili: " + Ex.Message);
            }


        }

        /// <summary>
        /// Mostra il valore delle variabili istantanee nella relativa tab.
        /// </summary>
        /// <param name="DatiPresenti">if set to <c>true</c> [dati presenti].</param>
        /// <param name="ValoriReali">Se <c>true</c> mostra i dati nel formato originale.</param>
        private void MostraCalibrazioni(bool DatiPresenti, bool ValoriReali = false)
        {

            try
            {
                //Vuoto i campi

                txtGainCurrZero.Text = "";
                txtGainCurrPos.Text = "";
                txtGainCurrNeg.Text = "";
                txtGainVBatt.Text = "";
                txtGainV3.Text = "";
                txtGainV2.Text = "";
                txtGainV1.Text = "";

                txtValCurrZero.Text = "";
                txtValCurrPos.Text = "";
                txtValCurrNeg.Text = "";
                txtValVBatt.Text = "";
                txtValV3.Text = "";
                txtValV2.Text = "";
                txtValV1.Text = "";



                if (DatiPresenti)
                {
                    if (ValoriReali)
                    {
                        txtGainCurrZero.Text = _sb.Calibrazioni.AdcCurrentZero.ToString();
                        txtGainCurrPos.Text = _sb.Calibrazioni.AdcCurrentPos.ToString();
                        txtGainCurrNeg.Text = _sb.Calibrazioni.AdcCurrentNeg.ToString();
                        txtGainVBatt.Text = _sb.Calibrazioni.GainVbatt.ToString();
                        txtGainV3.Text = _sb.Calibrazioni.GainVbatt3.ToString();
                        txtGainV2.Text = _sb.Calibrazioni.GainVbatt2.ToString();
                        txtGainV1.Text = _sb.Calibrazioni.GainVbatt1.ToString();

                        txtValCurrPos.Text = _sb.Calibrazioni.CurrentPos.ToString();
                        txtValCurrNeg.Text = _sb.Calibrazioni.CurrentNeg.ToString();
                        txtValVBatt.Text = _sb.Calibrazioni.ValVbatt.ToString();
                        txtValV3.Text = _sb.Calibrazioni.ValVbatt3.ToString();
                        txtValV2.Text = _sb.Calibrazioni.ValVbatt2.ToString();
                        txtValV1.Text = _sb.Calibrazioni.ValVbatt1.ToString();
                    }
                    else
                    {
                        txtGainCurrZero.Text = _sb.Calibrazioni.AdcCurrentZero.ToString();
                        txtGainCurrPos.Text = _sb.Calibrazioni.AdcCurrentPos.ToString();
                        txtGainCurrNeg.Text = _sb.Calibrazioni.AdcCurrentNeg.ToString();
                        txtGainVBatt.Text = _sb.Calibrazioni.GainVbatt.ToString();
                        txtGainV3.Text = _sb.Calibrazioni.GainVbatt3.ToString();
                        txtGainV2.Text = _sb.Calibrazioni.GainVbatt2.ToString();
                        txtGainV1.Text = _sb.Calibrazioni.GainVbatt1.ToString();

                        txtValCurrPos.Text = _sb.Calibrazioni.CurrentPos.ToString();
                        short _prova = (short)_sb.Calibrazioni.CurrentNeg;

                        txtValCurrNeg.Text = _prova.ToString(); //_sb.Calibrazioni.CurrentNeg.ToString();
                        txtValVBatt.Text = _sb.Calibrazioni.ValVbatt.ToString();
                        txtValV3.Text = _sb.Calibrazioni.ValVbatt3.ToString();
                        txtValV2.Text = _sb.Calibrazioni.ValVbatt2.ToString();
                        txtValV1.Text = _sb.Calibrazioni.ValVbatt1.ToString();
                    }

                }

            }
            catch (Exception Ex)
            {
                Log.Error("MostraVariabili: " + Ex.Message);
            }


        }


        private void RicaricaCicli(uint Inizio = 1, bool CaricaBrevi = false)
        {
            bool _esito;

            Log.Debug("RicaricaCicli: ");
            _esito = _sb.RicaricaCaricaCicliMemLunga(Inizio, (uint)_sb.sbData.LongMem, _logiche.dbDati.connessione, true, CaricaBrevi);
            //flvwCicliBatteria.RefreshObject(_sb.CicliMemoriaLunga);
        }

        /// <summary>
        /// Lancia il downoad dalla scheda dei cicli di memoria lunga, leggendo direttamente la memora
        /// </summary>
        private void RicaricaCiclidaMem()
        {
            try
            {
                bool _esito;

                Application.DoEvents();



                Log.Debug("RicaricaCicli da Memoria: ");

                _esito = _sb.CaricaCicliMemLungaDaMem(Convert.ToUInt32(txtMemDa.Text), Convert.ToUInt32(txtMemA.Text));

                Application.DoEvents();
                InizializzaVistaLunghi();
                // flvwCicliBatteria.BuildList();
            }
            catch (Exception Ex)
            {
                DialogResult risposta = MessageBox.Show(Ex.Message, "ERRORE", MessageBoxButtons.OK);

            }
        }


        private void CaricaProgrammazioni()
        {

            bool _esito;
            try
            {
                //Vuoto la tabella

                Log.Debug("CaricaProgrammazioni:");

                _esito = _sb.CaricaProgrammazioni(_sb.Id, _logiche.dbDati.connessione);

                //Aggiorno la testata
                txtCicliProgrammazione.Text = _sb.sbData.ProgramCount.ToString();

                InizializzaVistaProgrammazioni();
                //Vuoto tutte le textbox
                foreach (object ctrl in grbProgrammazione.Controls)
                {
                    if(ctrl.GetType() == typeof(TextBox))
                    {
                        TextBox _tmpTxt = (TextBox)ctrl;
                        _tmpTxt.Text = "";
                    }
                }

                if(_sb.ProgrammaCorrente != null)
                {
                    txtProgcID.Text = _sb.ProgrammaCorrente.IdProgramma.ToString();
                    txtProgcBattVdef.Text = FunzioniMR.StringaTensione(_sb.ProgrammaCorrente.BatteryVdef) + " V";
                    txtProgcBattAhDef.Text = FunzioniMR.StringaCapacita(_sb.ProgrammaCorrente.BatteryAhdef, 10) + " Ah";
                    txtProgcCelleTot.Text = _sb.ProgrammaCorrente.BatteryCells.ToString();
                    txtProgcCelleV3.Text = _sb.ProgrammaCorrente.BatteryCell3.ToString();
                    txtProgcCelleV2.Text = _sb.ProgrammaCorrente.BatteryCell2.ToString();
                    txtProgcCelleV1.Text = _sb.ProgrammaCorrente.BatteryCell1.ToString();
                    txtProgcNumSpire.Text = _sb.ProgrammaCorrente.NumeroSpire.ToString();

                    if (_sb.ProgrammaCorrente.AbilitaPresElett == 0xF0)
                    {
                        txtProgcSondaEl.Text = "Non Abilitata";
                        txtProgcSondaEl.ForeColor = Color.Red;
                    }
                    else
                    {
                        txtProgcSondaEl.Text = "Abilitata";
                        txtProgcSondaEl.ForeColor = Color.Black;
                    }

                    if (_sb.ProgrammaCorrente.VersoCorrente == 0xF0)
                    {
                        txtProgcVersoCorrente.Text = "Inverso";
                        txtProgcVersoCorrente.ForeColor = Color.Red;
                    }
                    else
                    {
                        txtProgcVersoCorrente.Text = "Diretto";
                        txtProgcVersoCorrente.ForeColor = Color.Black;

                    }

                    txtProMinChargeCurr.Text = FunzioniMR.StringaCapacita(_sb.ProgrammaCorrente.CorrenteMinimaCHG, 10) ;
                    txtProMaxChargeCurr.Text = FunzioniMR.StringaCapacita(_sb.ProgrammaCorrente.CorrenteMassimaCHG, 10) ;
                    txtProMinCurrW.Text = FunzioniMR.StringaCapacita(_sb.ProgrammaCorrente.MinCorrenteW, 10);
                    txtProMaxCurrW.Text = FunzioniMR.StringaCapacita(_sb.ProgrammaCorrente.MaxCorrenteW, 10);
                    txtProMaxCurrImp.Text = FunzioniMR.StringaCapacita(_sb.ProgrammaCorrente.MaxCorrenteImp, 10);

                    txtProTensioneGas.Text = FunzioniMR.StringaTensionePerCella(_sb.ProgrammaCorrente.TensioneGas, 1);
                    txtProTensioneRaccordo.Text = FunzioniMR.StringaTensionePerCella(_sb.ProgrammaCorrente.TensioneRaccordo, 1);
                    txtProTensioneFinale.Text = FunzioniMR.StringaTensionePerCella(_sb.ProgrammaCorrente.TensioneFinale, 1);

                    txtProTempAttenzione.Text = FunzioniMR.StringaTemperatura(_sb.ProgrammaCorrente.TempAttenzione);
                    txtProTempAllarme.Text = FunzioniMR.StringaTemperatura(_sb.ProgrammaCorrente.TempAllarme);
                    txtProTempRiavvio.Text = FunzioniMR.StringaTemperatura(_sb.ProgrammaCorrente.TempRipresa);



                }







            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }


        }

        /// <summary>
        /// Ricarica dalla scheda l'elenco programmazioni. All'inizio ricarica i contatori (Testata)
        /// </summary>
        private void RicaricaProgrammazioni()
        {
            try
            {


                bool _esito;

                Log.Debug("RicaricaProgrammazioni: ");

                // 18/11/15 - Prima di ricaricare la lista, ricarico la restata per leggere il contatore aggiornato
                _esito = _sb.CaricaTestata();


                _esito = _sb.RicaricaProgrammazioni(1, (ushort)_sb.sbData.ProgramCount, _logiche.dbDati.connessione, true);
                flvwProgrammiCarica.RefreshObject(_sb.Programmazioni);
            }
            catch (Exception Ex)
            {
                Log.Error("RicaricaProgrammazioni: " + Ex.Message);
            }
        }


        private void btnApparatoOk_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmSpyBat_Load(object sender, EventArgs e)
        {
            frmSpyBat_Resize(sender, e);
        }

        private void frmSpyBat_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.Width > 500)
                {
                    tabCaricaBatterie.Width = this.Width - 50;
                    //lvwCicliBatteriaOld.Width = this.Width - 120;
                    flvwCicliBatteria.Width = this.Width - 120;
                    tbcStatistiche.Width = tabCaricaBatterie.Width - 12;
                    //  buiStatCockpit.Width = tabStatCockpit.Width;
                    if (TbcStatSettimane != null)
                    {
                        TbcStatSettimane.Width = tbcStatistiche.Width - 12;
                    }
                }

                if (tbpCalibrazioni.Width > 600)
                {
                    pnlCalGrafico.Width = tbpCalibrazioni.Width - 540;
                }

                if (tabCalibrazione.Width > 620)
                {
                    pnlCalVerifica.Width = tabCalibrazione.Width - 300;
                }
                if (tabCalibrazione.Height > 300)
                {
                    pnlCalVerifica.Height = tabCalibrazione.Height - 240;
                }

                if (this.Height > 250)
                {
                    tabCaricaBatterie.Height = this.Height - 60;

                    bool _readonly;
                    int LivelloCorrente;
                    if (_logiche != null)
                    {
                        if (_logiche.currentUser != null)
                        {
                            LivelloCorrente = _logiche.currentUser.livello;
                        }
                        else
                        {
                            LivelloCorrente = 99;
                        }
                    }
                    else
                    {
                        LivelloCorrente = 99;
                    }

                    if (LivelloCorrente < 2) _readonly = true; else _readonly = false;


                    if (_readonly)
                        flvwCicliBatteria.Height = this.Height - 290;
                    else
                        flvwCicliBatteria.Height = this.Height - 130;

                    grbEsportaExcel.Top = this.Height - 250;
                    grbMemCicliContatori.Top = this.Height - 250;
                    grbMemCicliReadMem.Top = this.Height - 250;
                    grbMemCicliPulsanti.Top = this.Height - 170;

                    if (tabStatComparazioni != null)
                    {
                        //  InizializzaSchedaConfronti();
                    }

                    //Ridimensiono i tab grafici
                    tbcStatistiche.Height = tabCaricaBatterie.Height - 30;
                    // buiStatCockpit.Height = tabStatCockpit.Height;

                    if (TbcStatSettimane != null)
                    {
                        TbcStatSettimane.Height = tbcStatistiche.Height - 30;
                    }


                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        public void ChiudiScheda()
        {
            try
            {
                if (_parametri.serialeSpyBatt.IsOpen)
                {
                    _parametri.serialeSpyBatt.Close();
                }

            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        /// <summary>
        /// Se per il ciclo lungo corrente sono saricati i cicli  brevi, apre la finestra col dettaglio cicli
        /// </summary>
        private void MostraDettaglioRiga()
        {
            try
            {
                Log.Debug("MostraDettaglioRiga");

                if (flvwCicliBatteria.SelectedObject != null)
                {


                    sbMemLunga _tempLunga = (sbMemLunga)flvwCicliBatteria.SelectedObject;
                    if (_tempLunga.NumEventiBreviCaricati > 0)
                    {
                        Log.Debug("MostraDettaglioRiga Start");
                        frmListaCicliBreve CicliBreve = new frmListaCicliBreve();
                        CicliBreve.MdiParent = this.MdiParent;
                        CicliBreve.StartPosition = FormStartPosition.CenterParent;
                        CicliBreve.parametri = _parametri;

                        //CicliBreve.CicliMemoriaBreve = _sb.CicliMemoriaBreve;
                        CicliBreve.CicloLungo = _tempLunga;
                        CicliBreve._sb = _sb;
                        CicliBreve.Show();
                        Log.Debug("MostraDettaglioRiga Start mostracicli");
                        CicliBreve.MostraCicli();
                        Log.Debug("MostraDettaglioRiga Fine");
                    }

                }
            }
            catch (Exception Ex)
            {
                Log.Error("MostraDettaglioRiga: " + Ex.Message);
            }

        }


        /// <summary>
        /// Se per il ciclo lungo corrente sono saricati i cicli  brevi, apre la finestra col dettaglio cicli
        /// </summary>
        private void MostraDettaglioRiga(sbMemLunga RigaDati)
        {
            try
            {
                Log.Debug("MostraDettaglioRiga - Rigadati");

                if (RigaDati != null)
                {


                    sbMemLunga _tempLunga = RigaDati;
                    if (_tempLunga.NumEventiBreviCaricati > 0)
                    {
                        Log.Debug("MostraDettaglioRiga Start");
                        frmListaCicliBreve CicliBreve = new frmListaCicliBreve();
                        CicliBreve.MdiParent = this.MdiParent;
                        CicliBreve.StartPosition = FormStartPosition.CenterParent;
                        CicliBreve.parametri = _parametri;

                        //CicliBreve.CicliMemoriaBreve = _sb.CicliMemoriaBreve;
                        CicliBreve.CicloLungo = _tempLunga;
                        CicliBreve._sb = _sb;
                        CicliBreve.Show();
                        Log.Debug("MostraDettaglioRiga Start mostracicli");
                        CicliBreve.MostraCicli();
                        Log.Debug("MostraDettaglioRiga Fine");
                    }

                }
            }
            catch (Exception Ex)
            {
                Log.Error("MostraDettaglioRiga: " + Ex.Message);
            }

        }





        private void MostraDettaglioFase(UInt32 IdCiclo)
        {
            try
            {
                Log.Debug("MostraDettaglioFase " + IdCiclo.ToString());

                sbMemLunga _tempLunga = _sb.CicliMemoriaLunga.Find(x => x.IdMemoriaLunga == IdCiclo);

                if (_tempLunga != null)
                {

                    if (_tempLunga.NumEventiBreviCaricati > 0)
                    {
                        Log.Debug("MostraDettaglioRiga Start");
                        frmListaCicliBreve CicliBreve = new frmListaCicliBreve();
                        CicliBreve.MdiParent = this.MdiParent;
                        CicliBreve.StartPosition = FormStartPosition.CenterParent;
                        CicliBreve.parametri = _parametri;

                        //CicliBreve.CicliMemoriaBreve = _sb.CicliMemoriaBreve;
                        CicliBreve.CicloLungo = _tempLunga;
                        CicliBreve._sb = _sb;
                        CicliBreve.Show();
                        Log.Debug("MostraDettaglioRiga Start mostracicli");
                        CicliBreve.MostraCicli();
                        Log.Debug("MostraDettaglioRiga Fine");
                    }

                }
            }
            catch (Exception Ex)
            {
                Log.Error("MostraDettaglioRiga: " + Ex.Message);
            }

        }


        private void RicaricaDettaglioRiga(MessaggioSpyBatt.MemoriaPeriodoLungo _tempciclo)
        {
            try
            {
                if (flvwCicliBatteria.SelectedItems != null)
                {
                    int posizione;
                    int _progressivo;
                    _progressivo = Convert.ToInt32(flvwCicliBatteria.SelectedItems[0].Text);
                    posizione = (int)_sb.IntestazioneSb.longRecordCounter - _progressivo;

                    //MessaggioSpyBatt.MemoriaPeriodoLungo _tempciclo = _sb.CicliMemoriaLunga.ElementAt(_progressivo -1);
                    /*
                    bool _esito = _sb.RicaricaCaricaCicliMemBreve(_tempciclo.IdEvento, _tempciclo.PuntatorePrimoBreve, _tempciclo.NumEventiBrevi);

                    
                    CicliBreve.MdiParent = this.MdiParent;
                    CicliBreve.StartPosition = FormStartPosition.CenterParent;

                    CicliBreve.CicliMemoriaBreve = _sb.CicliMemoriaBreve;
                    CicliBreve.EventoLungo = _tempciclo;
                    CicliBreve._sb = _sb;
                    CicliBreve.Show();
                    CicliBreve.MostraCicli();
                    */




                    /*
                    if (lvwCicliBatteria.SelectedItems.Count > 0)
                    {
                        if (lvwCicliBatteria.SelectedItems[0].SubItems[1].Text == "carica")
                        {
                            frmSpyBatCarica sbDettaglioCar = new frmSpyBatCarica();
                            //sbDettaglioCar.MdiParent = this;
                            sbDettaglioCar.StartPosition = FormStartPosition.CenterParent;
                            sbDettaglioCar.ShowDialog();
                        }
                        else
                        {
                            if (lvwCicliBatteria.SelectedItems[0].SubItems[1].Text == "scarica")
                            {
                                frmSpyBatScarica sbDettaglioScar = new frmSpyBatScarica();
                                //sbDettaglioCar.MdiParent = this;
                                sbDettaglioScar.StartPosition = FormStartPosition.CenterParent;
                                sbDettaglioScar.ShowDialog();
                            }
                        }
                    }
                     */
                }
            }
            catch
            { }

        }

        /// <summary>
        /// Apre il form (modale) per l'inserimento di una nuova configurazione.
        /// </summary>
        private void MostraNuovoProgramma()
        {
            try
            {
                if (_sb.apparatoPresente)
                {


                    frmInserimentoProgramma NuovoProgramma = new frmInserimentoProgramma(_logiche);
                    NuovoProgramma._sb = _sb;
                    //NuovoProgramma.MdiParent = this.MdiParent;
                    NuovoProgramma.StartPosition = FormStartPosition.CenterParent;

                    NuovoProgramma.ShowDialog(this);
                    //NuovoProgramma.MostraCicli();

                    this.Cursor = Cursors.WaitCursor;

                    // 18/11/15 - Prima di ricaricare la lista, ricarico la testata per leggere il contatore aggiornato

                    txtCicliProgrammazione.Text = _sb.sbData.ProgramCount.ToString();

                    RicaricaProgrammazioni();
                    CaricaProgrammazioni();

                    this.Cursor = Cursors.Default;
                }


            }
            catch
            { }

        }


        private void lvwCicliBatteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (flvwCicliBatteria.SelectedItems != null)
                {
                    if (flvwCicliBatteria.SelectedItems.Count > 0)
                    {
                        if (flvwCicliBatteria.SelectedItems[0].SubItems[1].Text == "Carica")
                        {
                            frmSpyBatCarica sbDettaglioCar = new frmSpyBatCarica();
                            sbDettaglioCar.MdiParent = this;
                            sbDettaglioCar.StartPosition = FormStartPosition.CenterParent;
                            sbDettaglioCar.Show();
                        }

                    }
                }
            }
            catch
            {

            }

        }

        private void btnDettaglioCicliBrevi_Click(object sender, EventArgs e)
        {
            MostraDettaglioRiga();
        }

        private void opSonda01_CheckedChanged(object sender, EventArgs e)
        {
            grbComboSonda.Enabled = false;
        }

        private void opSonda02_CheckedChanged(object sender, EventArgs e)
        {
            grbComboSonda.Enabled = true;
        }

        private void rbtAccensione01_CheckedChanged(object sender, EventArgs e)
        {
            cmbOreRitardo.Enabled = false;
            cmbMinAccensione.Enabled = false;
            cmbOraAccensione.Enabled = false;

        }

        private void rbtAccensione02_CheckedChanged(object sender, EventArgs e)
        {
            cmbOreRitardo.Enabled = true;
            cmbMinAccensione.Enabled = false;
            cmbOraAccensione.Enabled = false;

        }

        private void rbtAccensione03_CheckedChanged(object sender, EventArgs e)
        {
            cmbOreRitardo.Enabled = false;
            cmbMinAccensione.Enabled = true;
            cmbOraAccensione.Enabled = true;

        }

        private void pbminIUIa_Click(object sender, EventArgs e)
        {
            frmGraficoCiclo frmGrafico = new frmGraficoCiclo();
            //sbDettaglioCar.MdiParent = this;
            frmGrafico.StartPosition = FormStartPosition.CenterParent;
            frmGrafico.TipoGrafico(1);
            frmGrafico.ShowDialog();
        }

        private void pbxIWAsmall_Click(object sender, EventArgs e)
        {
            frmGraficoCiclo frmGrafico = new frmGraficoCiclo();
            //sbDettaglioCar.MdiParent = this;
            frmGrafico.StartPosition = FormStartPosition.CenterParent;
            frmGrafico.TipoGrafico(0);
            frmGrafico.ShowDialog();
        }

        private void gbStopIUIa_Enter(object sender, EventArgs e)
        {

        }

        private void tabCb02_Click(object sender, EventArgs e)
        {

        }

        private void btnLeggiRtc_Click(object sender, EventArgs e)
        {
            CaricaOrologio();
        }

        private void btnScriviRtc_Click(object sender, EventArgs e)
        {
            ImpostaOrologio();
        }

        public void CaricaOrologio()
        {
            bool _esito;
            try
            {

                txtOraRtc.Text = "";
                txtDataRtc.Text = "";

                _esito = _sb.LeggiOrologio();
                if (_esito && _sb.UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk)
                {
                    txtOraRtc.Text = _sb.OrologioSistema.ore.ToString("00") + ":" + _sb.OrologioSistema.minuti.ToString("00");
                    txtDataRtc.Text = _sb.OrologioSistema.giorno.ToString("00") + "/" + _sb.OrologioSistema.mese.ToString("00") + "/" + _sb.OrologioSistema.anno.ToString("0000");
                }

            }
            catch
            {
            }
        }

        public void ImpostaOrologio()
        {
            bool _esito;
            try
            {
                _esito = _sb.ScriviOrologio();
                if (_esito)
                {

                    _esito = _sb.LeggiOrologio();
                    if (_esito)
                    {
                        txtOraRtc.Text = _sb.OrologioSistema.ore.ToString("00") + ":" + _sb.OrologioSistema.minuti.ToString("00");
                        txtDataRtc.Text = _sb.OrologioSistema.giorno.ToString("00") + "/" + _sb.OrologioSistema.mese.ToString("00") + "/" + _sb.OrologioSistema.anno.ToString("0000");
                    }

                }

            }
            catch
            {
            }
        }

        public void salvaCliente(bool SerialeCollegata)
        {
            try
            {
                Pianificazione _tempP;
                // prima salvo i dati nella classe
                _sb.sbCliente.Client = txtCliente.Text;
                _sb.sbCliente.ClientNote = txtNoteCliente.Text;
                _sb.sbCliente.BatteryBrand = txtMarcaBat.Text;
                _sb.sbCliente.BatteryModel = txtModelloBat.Text;
                _sb.sbCliente.BatteryId = txtIdBat.Text;
                _sb.sbCliente.BatteryLLId = txtCliCodiceLL.Text;
                _sb.sbCliente.SerialNumber = txtSerialNumber.Text;

                _sb.sbCliente.ResetContatori = MessaggioSpyBatt.DatiCliente.NuoviLivelli.MantieniLivelli;

                if (chkCliResetContatori.Checked)
                {
                    _sb.sbCliente.ResetContatori = MessaggioSpyBatt.DatiCliente.NuoviLivelli.ResetLivelli;
                }

                _tempP = (Pianificazione)cmbModoPianificazione.SelectedItem;
                _sb.sbCliente.ModoPianificazione = (byte)_tempP.CodiceTP;
                SalvaGrigliaTempo();

                int _tempInt;
                if (int.TryParse(txtCliCicliAttesi.Text, out _tempInt))
                { _sb.sbCliente.CicliAttesi = (int)_tempInt; }
                else
                { _sb.sbCliente.CicliAttesi = 1000; }

                _sb.ScriviDatiCliente(SerialeCollegata);
                if (CaricaCliente(_sb.Id, _logiche, true)) mostraCliente();
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCaricaListaLunghi_Click(object sender, EventArgs e)
        {
            Log.Debug("Lancio lettura lunghi");
            this.Cursor = Cursors.WaitCursor;
            //frmAvanzamentoCicli _avCicli = new frmAvanzamentoCicli();


            _avCicli.ParametriWorker.MainCount = 100;

            _avCicli.sbLocale = _sb;
            _avCicli.ValStart = 1;
            _avCicli.ValFine = (int)_sb.sbData.LongMem;
            _avCicli.DbDati = _logiche.dbDati.connessione;
            _avCicli.CaricaBrevi = chkCaricaBrevi.Checked;
            _avCicli.TipoComando = elementiComuni.tipoMessaggio.MemLunga;
            Log.Debug("FRM RicaricaCicli: ");

            //_esito = _sb.RicaricaCaricaCicliMemLunga(Inizio, (uint)_sb.sbData.LongMem, _logiche.dbDati.connessione, true, CaricaBrevi);

            // Apro il firm con le progressbar
            _avCicli.ShowDialog(this);
            //bool _caricaBrevi = ( chkCaricaBrevi.Checked == true );
            //RicaricaCicli(1, _caricaBrevi);
            CaricaCicli();

            btnCaricaDettaglioSel.Enabled = _apparatoPresente;
            //_avCicli.Dispose();
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InizializzaVistaLunghi()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                BarRenderer _barraCortiCaricati = new BarRenderer();
                ColumnHeaderStyle _chsLunghi = new ColumnHeaderStyle();
                

                bool _colonnaNascosta = true;
                int LivelloCorrente;
                if (_logiche.currentUser != null)
                {
                    LivelloCorrente = _logiche.currentUser.livello;
                }
                else
                {
                    LivelloCorrente = 99;
                }

                if (LivelloCorrente < 1) _colonnaNascosta = true; else _colonnaNascosta = false;

                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);

                flvwCicliBatteria.Font = new Font("Tahoma", 8, FontStyle.Regular);

                flvwCicliBatteria.HeaderUsesThemes = false;
                flvwCicliBatteria.HeaderFormatStyle = _stile;

                flvwCicliBatteria.AllColumns.Clear();


                flvwCicliBatteria.View = View.Details;
                flvwCicliBatteria.ShowGroups = false;
                flvwCicliBatteria.GridLines = true;
                flvwCicliBatteria.HeaderWordWrap = true;


                BrightIdeasSoftware.OLVColumn Colonna1 = new BrightIdeasSoftware.OLVColumn();
                Colonna1.Text = StringheMessaggio.strVistaLunghiCol01;
                Colonna1.AspectName = "IdMemoriaLunga";
                Colonna1.Width = 50;
                Colonna1.HeaderTextAlign = HorizontalAlignment.Left;
                Colonna1.TextAlign = HorizontalAlignment.Left;
                //               Colonna1.CheckBoxes = false;

                flvwCicliBatteria.AllColumns.Add(Colonna1);


                BrightIdeasSoftware.OLVColumn Colonna2 = new BrightIdeasSoftware.OLVColumn();
                Colonna2.Text = StringheMessaggio.strVistaLunghiCol02;
                Colonna2.Sortable = false;
                Colonna2.AspectName = "TipoEvento";
                Colonna2.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    return _sb.StringaTipoEvento(_tempVal.TipoEvento);
                };
                Colonna2.Width = 70;
                Colonna2.HeaderTextAlign = HorizontalAlignment.Left;
                Colonna2.TextAlign = HorizontalAlignment.Left;
                flvwCicliBatteria.AllColumns.Add(Colonna2);

                BrightIdeasSoftware.OLVColumn Colonna3 = new BrightIdeasSoftware.OLVColumn();
                Colonna3.Text = StringheMessaggio.strVistaLunghiCol03;
                Colonna3.AspectName = "DataOraStart";
                Colonna3.Sortable = false;
                Colonna3.Width = 100;
                Colonna3.HeaderTextAlign = HorizontalAlignment.Left;
                Colonna3.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna3);

                BrightIdeasSoftware.OLVColumn Colonna4 = new BrightIdeasSoftware.OLVColumn();
                Colonna4.Text = StringheMessaggio.strVistaLunghiCol04;
                Colonna4.AspectName = "DataOraFine";
                Colonna4.Sortable = false;
                Colonna4.Width = 100;
                Colonna4.HeaderTextAlign = HorizontalAlignment.Left;
                Colonna4.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna4);

                BrightIdeasSoftware.OLVColumn Colonna5 = new BrightIdeasSoftware.OLVColumn();
                Colonna5.Text = StringheMessaggio.strVistaLunghiCol05;
                Colonna5.AspectName = "Durata";
                Colonna5.Sortable = false;
                Colonna5.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    return _sb.StringaDurata(_tempVal.Durata);
                };
                Colonna5.Width = 60;
                Colonna5.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna5.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna5);

                BrightIdeasSoftware.OLVColumn Colonna6m = new BrightIdeasSoftware.OLVColumn();
                Colonna6m.Text = StringheMessaggio.strVistaLunghiCol06m;
                Colonna6m.AspectName = "strTempMin";
                Colonna6m.Sortable = false;
                Colonna6m.Width = 60;
                Colonna6m.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna6m.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna6m);

                BrightIdeasSoftware.OLVColumn Colonna6 = new BrightIdeasSoftware.OLVColumn();
                Colonna6.Text = StringheMessaggio.strVistaLunghiCol06;
                Colonna6.AspectName = "strTempMax";
                Colonna6.Sortable = false;
                Colonna6.Width = 60;
                Colonna6.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna6.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna6);

                //Tempo Sovreatemperatura
                BrightIdeasSoftware.OLVColumn ColDurataOverTemp = new BrightIdeasSoftware.OLVColumn();
                ColDurataOverTemp.Text = StringheMessaggio.strVistaLunghiColOverT;  //    "T OverT"
                ColDurataOverTemp.AspectName = "DurataOverTemp";
                ColDurataOverTemp.Sortable = false;
                ColDurataOverTemp.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    return _sb.StringaDurata(_tempVal.DurataOverTemp, true);
                };
                ColDurataOverTemp.Width = 65;
                ColDurataOverTemp.HeaderTextAlign = HorizontalAlignment.Center;
                ColDurataOverTemp.TextAlign = HorizontalAlignment.Right;
                ColDurataOverTemp.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(ColDurataOverTemp);

                BrightIdeasSoftware.OLVColumn Colonna7 = new BrightIdeasSoftware.OLVColumn();
                Colonna7.Text = StringheMessaggio.strVistaLunghiCol07;
                Colonna7.AspectName = "strVmin";
                Colonna7.Sortable = false;
                Colonna7.Width = 50;
                Colonna7.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna7.TextAlign = HorizontalAlignment.Right;
                Colonna7.IsVisible = false;
                flvwCicliBatteria.AllColumns.Add(Colonna7);

                BrightIdeasSoftware.OLVColumn Colonna8 = new BrightIdeasSoftware.OLVColumn();
                Colonna8.Text = StringheMessaggio.strVistaLunghiCol08;
                Colonna8.AspectName = "strVmax";
                Colonna8.Sortable = false;
                Colonna8.Width = 50;
                Colonna8.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna8.TextAlign = HorizontalAlignment.Right;
                Colonna8.IsVisible = false;
                flvwCicliBatteria.AllColumns.Add(Colonna8);



                BrightIdeasSoftware.OLVColumn Colonna7c = new BrightIdeasSoftware.OLVColumn();
                Colonna7c.Text = StringheMessaggio.strVistaLunghiCol07c;
                Colonna7c.AspectName = "strVCellMin";
                Colonna7c.Sortable = false;
                Colonna7c.Width = 50;
                Colonna7c.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna7c.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna7c);

                BrightIdeasSoftware.OLVColumn Colonna8c = new BrightIdeasSoftware.OLVColumn();
                Colonna8c.Text = StringheMessaggio.strVistaLunghiCol08c;
                Colonna8c.AspectName = "strVCellMax";
                Colonna8c.Sortable = false;
                Colonna8c.Width = 50;
                Colonna8c.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna8c.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna8c);





                BrightIdeasSoftware.OLVColumn Colonna9 = new BrightIdeasSoftware.OLVColumn();
                Colonna9.Text = StringheMessaggio.strVistaLunghiCol09;  //"I min";
                Colonna9.AspectName = "olvAmin";
                Colonna9.Sortable = false;
                Colonna9.ToolTipText = "Valore minimo Ampere caricati (nero) o scaricati (rosso)";
                Colonna9.Width = 50;
                Colonna9.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.olvAmin;
                    }
                    else
                    {
                        return "";
                    }
                };
                Colonna9.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna9.TextAlign = HorizontalAlignment.Right;
                Colonna9.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(Colonna9);

                BrightIdeasSoftware.OLVColumn Colonna10 = new BrightIdeasSoftware.OLVColumn();
                Colonna10.Text = StringheMessaggio.strVistaLunghiCol10; //"I max";
                Colonna10.Sortable = false;
                Colonna10.AspectName = "olvAmax";
                Colonna10.ToolTipText = "Valore massimo Ampere caricati (nero) o scaricati (rosso)";
                Colonna10.Width = 50;
                Colonna10.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.olvAmax;
                    }
                    else
                    {
                        return "";
                    }
                };
                Colonna10.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna10.TextAlign = HorizontalAlignment.Right;
                Colonna10.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(Colonna10);

                BrightIdeasSoftware.OLVColumn Colonna11 = new BrightIdeasSoftware.OLVColumn();
                Colonna11.Text = StringheMessaggio.strVistaLunghiCol11; // "Elettrolita";
                Colonna11.Sortable = false;
                Colonna11.AspectName = "strPresenzaElettrolita";
                Colonna11.Width = 60;
                Colonna11.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna11.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna11);


                //Tempo Mancanza Elettrolita 
                BrightIdeasSoftware.OLVColumn ColDurataAssEl = new BrightIdeasSoftware.OLVColumn();
                ColDurataAssEl.Text = "T AssEl"; // StringheMessaggio.strVistaLunghiCol05;
                ColDurataAssEl.AspectName = "DurataMancanzaElettrolita";
                ColDurataAssEl.Sortable = false;
                ColDurataAssEl.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    return _sb.StringaDurata(_tempVal.DurataMancanzaElettrolita, true);
                };
                ColDurataAssEl.Width = 60;
                ColDurataAssEl.HeaderTextAlign = HorizontalAlignment.Center;
                ColDurataAssEl.TextAlign = HorizontalAlignment.Right;
                ColDurataAssEl.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(ColDurataAssEl);



                BrightIdeasSoftware.OLVColumn Colonna12 = new BrightIdeasSoftware.OLVColumn();
                Colonna12.Text = StringheMessaggio.strVistaLunghiCol12; // "Ah";
                Colonna12.AspectName = "strAh";
                Colonna12.Sortable = false;
                Colonna12.Width = 65;
                Colonna12.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.strAh;
                    }
                    else
                    {
                        return "";
                    }
                };
                Colonna12.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna12.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna12);


                BrightIdeasSoftware.OLVColumn ColAhCaricati = new BrightIdeasSoftware.OLVColumn();
                ColAhCaricati.Text = StringheMessaggio.strVistaLunghiColAhCaricati; //"Ah C.";
                ColAhCaricati.Sortable = false;
                ColAhCaricati.ToolTipText = "Ah Caricati";
                ColAhCaricati.AspectName = "strAhCaricati";
                ColAhCaricati.Width = 65;
                ColAhCaricati.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.strAhCaricati;
                    }
                    else
                    {
                        return "";
                    }
                };
                ColAhCaricati.HeaderTextAlign = HorizontalAlignment.Center;
                ColAhCaricati.TextAlign = HorizontalAlignment.Right;
                ColAhCaricati.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(ColAhCaricati);

                BrightIdeasSoftware.OLVColumn ColAhScaricati = new BrightIdeasSoftware.OLVColumn();
                ColAhScaricati.Text = StringheMessaggio.strVistaLunghiColAhScaricati; //"Ah Sc.";
                ColAhScaricati.Sortable = false;
                ColAhScaricati.ToolTipText = "Ah Scaricati";
                ColAhScaricati.AspectName = "strAhScaricati";
                ColAhScaricati.Width = 65;
                ColAhScaricati.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.strAhScaricati;
                    }
                    else
                    {
                        return "";
                    }
                };
                ColAhScaricati.HeaderTextAlign = HorizontalAlignment.Center;
                ColAhScaricati.TextAlign = HorizontalAlignment.Right;
                ColAhScaricati.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(ColAhScaricati);

                BrightIdeasSoftware.OLVColumn Colonna13 = new BrightIdeasSoftware.OLVColumn();
                Colonna13.Text = "KWh";
                Colonna13.AspectName = "strKWh";
                Colonna13.Sortable = false;

                Colonna13.Width = 65;
                Colonna13.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.strKWh;
                    }
                    else
                    {
                        return "";
                    }
                };
                Colonna13.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna13.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna13);

                BrightIdeasSoftware.OLVColumn Colonna13C = new BrightIdeasSoftware.OLVColumn();
                Colonna13C.Text = StringheMessaggio.strVistaLunghiCol13C; //"KWh C.";
                Colonna13C.AspectName = "strKWhCaricati";
                Colonna13C.Sortable = false;

                Colonna13C.Width = 65;
                Colonna13C.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.strKWhCaricati;
                    }
                    else
                    {
                        return "";
                    }
                };
                Colonna13C.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna13C.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna13C);

                BrightIdeasSoftware.OLVColumn Colonna13S = new BrightIdeasSoftware.OLVColumn();
                Colonna13S.Text = StringheMessaggio.strVistaLunghiCol13S; //"KWh S.";
                Colonna13S.AspectName = "strKWhScaricati";
                Colonna13S.Sortable = false;

                Colonna13S.Width = 65;
                Colonna13S.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.strKWhScaricati;
                    }
                    else
                    {
                        return "";
                    }
                };
                Colonna13S.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna13S.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna13S);





                BrightIdeasSoftware.OLVColumn Colonna14 = new BrightIdeasSoftware.OLVColumn();
                Colonna14.Text = "S.o.C.";
                Colonna14.AspectName = "strStatoCarica";
                Colonna14.Sortable = false;
                Colonna14.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.strStatoCarica;
                    }
                    else
                    {
                        return "";
                    }
                };
                Colonna14.Width = 60;
                Colonna14.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna14.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna14);



                /*--------------------------------------------------------------------------------------------------------------------*/

                BrightIdeasSoftware.OLVColumn ColonnaLivStart = new BrightIdeasSoftware.OLVColumn();
                ColonnaLivStart.Text = "Cap IN";
                ColonnaLivStart.AspectName = "strLivelloIniziale";
                ColonnaLivStart.Sortable = false;   
                /*                 
                ColonnaLivStart.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.strLivelloIniziale;
                    }
                    else
                    {
                        return "";
                    }
                };
                */
                ColonnaLivStart.Width = 60;
                ColonnaLivStart.HeaderTextAlign = HorizontalAlignment.Center;
                ColonnaLivStart.TextAlign = HorizontalAlignment.Right;
                ColonnaLivStart.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(ColonnaLivStart);


                BrightIdeasSoftware.OLVColumn ColonnaLivStop = new BrightIdeasSoftware.OLVColumn();
                ColonnaLivStop.Text = "Cap Fin";
                ColonnaLivStop.AspectName = "strLivelloFinale";
                ColonnaLivStop.Sortable = false;
                ColonnaLivStop.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.strLivelloFinale;
                    }
                    else
                    {
                        return "";
                    }
                };
                ColonnaLivStop.Width = 60;
                ColonnaLivStop.HeaderTextAlign = HorizontalAlignment.Center;
                ColonnaLivStop.TextAlign = HorizontalAlignment.Right;
                ColonnaLivStop.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(ColonnaLivStop);




                BrightIdeasSoftware.OLVColumn ColonnaSOC = new BrightIdeasSoftware.OLVColumn();
                ColonnaSOC.Text = "SoC.Eff";
                ColonnaSOC.AspectName = "strStatoCaricaEff";
                ColonnaSOC.Sortable = false;
                ColonnaSOC.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.strStatoCaricaEff;
                    }
                    else
                    {
                        return "";
                    }
                };
                ColonnaSOC.Width = 60;
                ColonnaSOC.HeaderTextAlign = HorizontalAlignment.Center;
                ColonnaSOC.TextAlign = HorizontalAlignment.Right;
                ColonnaSOC.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(ColonnaSOC);

/*--------------------------------------------------------------------------------------------------------------------*/


                BrightIdeasSoftware.OLVColumn Colonna15 = new BrightIdeasSoftware.OLVColumn();
                Colonna15.Text = "C.F.";
                Colonna15.Sortable = false;
                Colonna15.AspectName = "strFattoreCarica";
                Colonna15.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0)
                    {
                        return _tempVal.strFattoreCarica;
                    }
                    else
                    {
                        return "";
                    }
                };
                Colonna15.Width = 60;
                Colonna15.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna15.TextAlign = HorizontalAlignment.Right;

                flvwCicliBatteria.AllColumns.Add(Colonna15);

                //Tempo Sbilanciamento celle 
                BrightIdeasSoftware.OLVColumn ColDurataSbil = new BrightIdeasSoftware.OLVColumn();
                ColDurataSbil.Text = "T Sbil"; // StringheMessaggio.strVistaLunghiCol05;
                ColDurataSbil.AspectName = "DurataSbilCelle";
                ColDurataSbil.Sortable = false;
                ColDurataSbil.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    return _sb.StringaDurata(_tempVal.DurataSbilCelle, true);
                };
                ColDurataSbil.Width = 60;
                ColDurataSbil.HeaderTextAlign = HorizontalAlignment.Center;
                ColDurataSbil.TextAlign = HorizontalAlignment.Right;
                ColDurataSbil.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(ColDurataSbil);


                //strVMaxSbilanciamentoC
                BrightIdeasSoftware.OLVColumn Colonna20 = new BrightIdeasSoftware.OLVColumn();
                Colonna20.Text = StringheMessaggio.strVistaLunghiCol20; // "max Sbil.";
                Colonna20.Sortable = false;
                Colonna20.AspectName = "strVMaxSbilanciamentoC";
                Colonna20.Width = 50;
                Colonna20.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna20.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna20);


                BrightIdeasSoftware.OLVColumn colCondStop = new BrightIdeasSoftware.OLVColumn();
                colCondStop.Text = "Stop";
                colCondStop.AspectName = "strCondizioneStop";
                colCondStop.Sortable = false;
                colCondStop.Width = 40;
                colCondStop.HeaderTextAlign = HorizontalAlignment.Center;
                colCondStop.TextAlign = HorizontalAlignment.Center;
                colCondStop.IsVisible = false;
                flvwCicliBatteria.AllColumns.Add(colCondStop);

                BrightIdeasSoftware.OLVColumn colIdProgramma = new BrightIdeasSoftware.OLVColumn();
                colIdProgramma.Text = StringheMessaggio.strVistaLunghiColIdProgramma; //"Conf";
                colIdProgramma.Sortable = false;
                colIdProgramma.AspectName = "strIdProgramma";
                colIdProgramma.Width = 40;
                colIdProgramma.HeaderTextAlign = HorizontalAlignment.Center;
                colIdProgramma.TextAlign = HorizontalAlignment.Center;
                flvwCicliBatteria.AllColumns.Add(colIdProgramma);

                BrightIdeasSoftware.OLVColumn Colonna16 = new BrightIdeasSoftware.OLVColumn();
                Colonna16.Text = StringheMessaggio.strVistaLunghiCol16; // "Registrazioni";
                Colonna16.Sortable = false;
                Colonna16.AspectName = "strNumEventiBrevi";
                Colonna16.Width = 60;
                Colonna16.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna16.TextAlign = HorizontalAlignment.Center;
                Colonna16.IsVisible = _colonnaNascosta;

                flvwCicliBatteria.AllColumns.Add(Colonna16);

                BrightIdeasSoftware.OLVColumn Colonna17 = new BrightIdeasSoftware.OLVColumn();
                Colonna17.Text = StringheMessaggio.strVistaLunghiCol17; // "% Reg.";
                Colonna17.Sortable = false;
                Colonna17.AspectName = "PercEventiBreviCaricati";
                Colonna17.Renderer = _barraCortiCaricati;
                Colonna17.Width = 100;
                _barraCortiCaricati.BackgroundColor = System.Drawing.Color.Green;
                _barraCortiCaricati.MaximumValue = 100;
                _barraCortiCaricati.UseStandardBar = true;
                Colonna17.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(Colonna17);

                BrightIdeasSoftware.OLVColumn Colonna18 = new BrightIdeasSoftware.OLVColumn();
                Colonna18.Text = "";
                Colonna18.Width = 500;
                Colonna18.Sortable = false;
                Colonna18.FillsFreeSpace = true;
                flvwCicliBatteria.AllColumns.Add(Colonna18);


                flvwCicliBatteria.RebuildColumns();

                this.flvwCicliBatteria.SetObjects(_sb.CicliMemoriaLunga);
                flvwCicliBatteria.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }

        private void InizializzaVistaStat()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                BarRenderer _barraCortiCaricati = new BarRenderer();
                ColumnHeaderStyle _chsLunghi = new ColumnHeaderStyle();

                bool _colonnaNascosta = true;
                int LivelloCorrente;
                if (_logiche.currentUser != null)
                {
                    LivelloCorrente = _logiche.currentUser.livello;
                }
                else
                {
                    LivelloCorrente = 99;
                }

                if (LivelloCorrente < 1) _colonnaNascosta = true; else _colonnaNascosta = false;

                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);


                flvwCicliBatteria.HeaderUsesThemes = false;
                flvwCicliBatteria.HeaderFormatStyle = _stile;

                flvwCicliBatteria.AllColumns.Clear();


                flvwCicliBatteria.View = View.Details;
                flvwCicliBatteria.ShowGroups = false;
                flvwCicliBatteria.GridLines = true;


                BrightIdeasSoftware.OLVColumn Colonna1 = new BrightIdeasSoftware.OLVColumn();
                Colonna1.Text = StringheMessaggio.strVistaLunghiCol01;
                Colonna1.AspectName = "IdMemoriaLunga";
                Colonna1.Width = 60;
                Colonna1.HeaderTextAlign = HorizontalAlignment.Left;
                Colonna1.TextAlign = HorizontalAlignment.Left;
                //               Colonna1.CheckBoxes = false;

                flvwCicliBatteria.AllColumns.Add(Colonna1);


                BrightIdeasSoftware.OLVColumn Colonna2 = new BrightIdeasSoftware.OLVColumn();
                Colonna2.Text = StringheMessaggio.strVistaLunghiCol02;
                Colonna2.Sortable = false;
                Colonna2.AspectName = "TipoEvento";
                Colonna2.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    return _sb.StringaTipoEvento(_tempVal.TipoEvento);
                };
                Colonna2.Width = 100;
                Colonna2.HeaderTextAlign = HorizontalAlignment.Left;
                Colonna2.TextAlign = HorizontalAlignment.Left;
                flvwCicliBatteria.AllColumns.Add(Colonna2);

                BrightIdeasSoftware.OLVColumn Colonna3 = new BrightIdeasSoftware.OLVColumn();
                Colonna3.Text = StringheMessaggio.strVistaLunghiCol03;
                Colonna3.AspectName = "DataOraStart";
                Colonna3.Sortable = false;
                Colonna3.Width = 100;
                Colonna3.HeaderTextAlign = HorizontalAlignment.Left;
                Colonna3.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna3);

                BrightIdeasSoftware.OLVColumn Colonna4 = new BrightIdeasSoftware.OLVColumn();
                Colonna4.Text = StringheMessaggio.strVistaLunghiCol04;
                Colonna4.AspectName = "DataOraFine";
                Colonna4.Sortable = false;
                Colonna4.Width = 100;
                Colonna4.HeaderTextAlign = HorizontalAlignment.Left;
                Colonna4.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna4);

                BrightIdeasSoftware.OLVColumn Colonna5 = new BrightIdeasSoftware.OLVColumn();
                Colonna5.Text = StringheMessaggio.strVistaLunghiCol05;
                Colonna5.AspectName = "Durata";
                Colonna5.Sortable = false;
                Colonna5.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    return _sb.StringaDurata(_tempVal.Durata);
                };
                Colonna5.Width = 60;
                Colonna5.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna5.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna5);

                BrightIdeasSoftware.OLVColumn Colonna6m = new BrightIdeasSoftware.OLVColumn();
                Colonna6m.Text = StringheMessaggio.strVistaLunghiCol06m;
                Colonna6m.AspectName = "strTempMin";
                Colonna6m.Sortable = false;
                Colonna6m.Width = 60;
                Colonna6m.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna6m.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna6m);

                BrightIdeasSoftware.OLVColumn Colonna6 = new BrightIdeasSoftware.OLVColumn();
                Colonna6.Text = StringheMessaggio.strVistaLunghiCol06;
                Colonna6.AspectName = "strTempMax";
                Colonna6.Sortable = false;
                Colonna6.Width = 60;
                Colonna6.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna6.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna6);

                BrightIdeasSoftware.OLVColumn Colonna7 = new BrightIdeasSoftware.OLVColumn();
                Colonna7.Text = StringheMessaggio.strVistaLunghiCol07;
                Colonna7.AspectName = "strVmin";
                Colonna7.Sortable = false;
                Colonna7.Width = 50;
                Colonna7.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna7.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna7);

                BrightIdeasSoftware.OLVColumn Colonna8 = new BrightIdeasSoftware.OLVColumn();
                Colonna8.Text = StringheMessaggio.strVistaLunghiCol08;
                Colonna8.AspectName = "strVmax";
                Colonna8.Sortable = false;
                Colonna8.Width = 50;
                Colonna8.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna8.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna8);


                BrightIdeasSoftware.OLVColumn Colonna9 = new BrightIdeasSoftware.OLVColumn();
                Colonna9.Text = StringheMessaggio.strVistaLunghiCol09;  //"I min";
                Colonna9.AspectName = "olvAmin";
                Colonna9.Sortable = false;
                Colonna9.ToolTipText = "Valore minimo Ampere caricati (nero) o scaricati (rosso)";
                Colonna9.Width = 50;
                Colonna9.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna9.TextAlign = HorizontalAlignment.Right;
                Colonna9.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(Colonna9);

                BrightIdeasSoftware.OLVColumn Colonna10 = new BrightIdeasSoftware.OLVColumn();
                Colonna10.Text = StringheMessaggio.strVistaLunghiCol10; //"I max";
                Colonna10.Sortable = false;
                Colonna10.AspectName = "olvAmax";
                Colonna10.ToolTipText = "Valore massimo Ampere caricati (nero) o scaricati (rosso)";
                Colonna10.Width = 50;
                Colonna10.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna10.TextAlign = HorizontalAlignment.Right;
                Colonna10.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(Colonna10);

                BrightIdeasSoftware.OLVColumn Colonna11 = new BrightIdeasSoftware.OLVColumn();
                Colonna11.Text = StringheMessaggio.strVistaLunghiCol11; // "Elettrolita";
                Colonna11.Sortable = false;
                Colonna11.AspectName = "strPresenzaElettrolita";
                Colonna11.Width = 60;
                Colonna11.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna11.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna11);

                BrightIdeasSoftware.OLVColumn Colonna12 = new BrightIdeasSoftware.OLVColumn();
                Colonna12.Text = StringheMessaggio.strVistaLunghiCol12; // "Ah";
                Colonna12.AspectName = "strAh";
                Colonna12.Sortable = false;
                Colonna12.Width = 50;
                Colonna12.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna12.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna12);


                BrightIdeasSoftware.OLVColumn ColAhCaricati = new BrightIdeasSoftware.OLVColumn();
                ColAhCaricati.Text = StringheMessaggio.strVistaLunghiColAhCaricati; //"Ah C.";
                ColAhCaricati.Sortable = false;
                ColAhCaricati.ToolTipText = "Ah Caricati";
                ColAhCaricati.AspectName = "strAhCaricati";
                ColAhCaricati.Width = 50;
                ColAhCaricati.HeaderTextAlign = HorizontalAlignment.Center;
                ColAhCaricati.TextAlign = HorizontalAlignment.Right;
                ColAhCaricati.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(ColAhCaricati);

                BrightIdeasSoftware.OLVColumn ColAhScaricati = new BrightIdeasSoftware.OLVColumn();
                ColAhScaricati.Text = StringheMessaggio.strVistaLunghiColAhScaricati; //"Ah Sc.";
                ColAhScaricati.Sortable = false;
                ColAhScaricati.ToolTipText = "Ah Scaricati";
                ColAhScaricati.AspectName = "strAhScaricati";
                ColAhScaricati.Width = 50;
                ColAhScaricati.HeaderTextAlign = HorizontalAlignment.Center;
                ColAhScaricati.TextAlign = HorizontalAlignment.Right;
                ColAhScaricati.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(ColAhScaricati);

                BrightIdeasSoftware.OLVColumn Colonna13 = new BrightIdeasSoftware.OLVColumn();
                Colonna13.Text = "KWh";
                Colonna13.AspectName = "strKWh";
                Colonna13.Sortable = false;

                Colonna13.Width = 50;
                Colonna13.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna13.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna13);

                BrightIdeasSoftware.OLVColumn Colonna13C = new BrightIdeasSoftware.OLVColumn();
                Colonna13C.Text = StringheMessaggio.strVistaLunghiCol13C; //"KWh C.";
                Colonna13C.AspectName = "strKWhCaricati";
                Colonna13C.Sortable = false;

                Colonna13C.Width = 60;
                Colonna13C.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna13C.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna13C);

                BrightIdeasSoftware.OLVColumn Colonna13S = new BrightIdeasSoftware.OLVColumn();
                Colonna13S.Text = StringheMessaggio.strVistaLunghiCol13S; //"KWh S.";
                Colonna13S.AspectName = "strKWhScaricati";
                Colonna13S.Sortable = false;

                Colonna13S.Width = 60;
                Colonna13S.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna13S.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna13S);





                BrightIdeasSoftware.OLVColumn Colonna14 = new BrightIdeasSoftware.OLVColumn();
                Colonna14.Text = "S.o.C.";
                Colonna14.AspectName = "strStatoCarica";
                Colonna14.Sortable = false;
                Colonna14.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0 || _tempVal.TipoEvento == 0x0F)
                    {
                        return _tempVal.strStatoCarica;
                    }
                    else
                    {
                        return "";
                    }
                };
                Colonna14.Width = 60;
                Colonna14.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna14.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna14);

                BrightIdeasSoftware.OLVColumn Colonna15 = new BrightIdeasSoftware.OLVColumn();
                Colonna15.Text = "C.F.";
                Colonna15.Sortable = false;
                Colonna15.AspectName = "strFattoreCarica";
                Colonna15.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    if (_tempVal.TipoEvento == 0xF0)
                    {
                        return _tempVal.strFattoreCarica;
                    }
                    else
                    {
                        return "";
                    }
                };
                Colonna15.Width = 60;
                Colonna15.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna15.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna15);

                //strVMaxSbilanciamentoC
                BrightIdeasSoftware.OLVColumn Colonna20 = new BrightIdeasSoftware.OLVColumn();
                Colonna20.Text = StringheMessaggio.strVistaLunghiCol20; // "max Sbil.";
                Colonna20.Sortable = false;
                Colonna20.AspectName = "strVMaxSbilanciamentoC";
                Colonna20.Width = 50;
                Colonna20.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna20.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna20);


                BrightIdeasSoftware.OLVColumn colCondStop = new BrightIdeasSoftware.OLVColumn();
                colCondStop.Text = "Stop";
                colCondStop.AspectName = "strCondizioneStop";
                colCondStop.Sortable = false;
                colCondStop.Width = 40;
                colCondStop.HeaderTextAlign = HorizontalAlignment.Center;
                colCondStop.TextAlign = HorizontalAlignment.Center;
                colCondStop.IsVisible = false;
                flvwCicliBatteria.AllColumns.Add(colCondStop);

                BrightIdeasSoftware.OLVColumn colIdProgramma = new BrightIdeasSoftware.OLVColumn();
                colIdProgramma.Text = StringheMessaggio.strVistaLunghiColIdProgramma; //"Conf";
                colIdProgramma.Sortable = false;
                colIdProgramma.AspectName = "strIdProgramma";
                colIdProgramma.Width = 40;
                colIdProgramma.HeaderTextAlign = HorizontalAlignment.Center;
                colIdProgramma.TextAlign = HorizontalAlignment.Center;
                flvwCicliBatteria.AllColumns.Add(colIdProgramma);

                BrightIdeasSoftware.OLVColumn Colonna16 = new BrightIdeasSoftware.OLVColumn();
                Colonna16.Text = StringheMessaggio.strVistaLunghiCol16; // "Registrazioni";
                Colonna16.Sortable = false;
                Colonna16.AspectName = "strNumEventiBrevi";
                Colonna16.Width = 60;
                Colonna16.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna16.TextAlign = HorizontalAlignment.Center;
                Colonna16.IsVisible = _colonnaNascosta;

                flvwCicliBatteria.AllColumns.Add(Colonna16);

                BrightIdeasSoftware.OLVColumn Colonna17 = new BrightIdeasSoftware.OLVColumn();
                Colonna17.Text = StringheMessaggio.strVistaLunghiCol17; // "Registrazioni";
                Colonna17.Sortable = false;
                Colonna17.AspectName = "PercEventiBreviCaricati";
                Colonna17.Renderer = _barraCortiCaricati;
                Colonna17.Width = 100;
                _barraCortiCaricati.BackgroundColor = System.Drawing.Color.Green;
                _barraCortiCaricati.MaximumValue = 100;
                _barraCortiCaricati.UseStandardBar = true;
                Colonna17.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(Colonna17);

                BrightIdeasSoftware.OLVColumn Colonna18 = new BrightIdeasSoftware.OLVColumn();
                Colonna18.Text = "";
                Colonna18.Width = 500;
                Colonna18.Sortable = false;
                Colonna18.FillsFreeSpace = true;
                flvwCicliBatteria.AllColumns.Add(Colonna18);


                flvwCicliBatteria.RebuildColumns();

                this.flvwCicliBatteria.SetObjects(_sb.CicliMemoriaLunga);
                flvwCicliBatteria.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }

        public void MostraTensioni()
        {
            //flvwCicliBatteria.Columns[1].is
        }

        /// <summary>
        /// 
        /// </summary>
        private void InizializzaVistaProgrammazioni()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvwProgrammiCarica.HeaderUsesThemes = false;
                flvwProgrammiCarica.HeaderFormatStyle = _stile;
                flvwProgrammiCarica.UseAlternatingBackColors = true;
                flvwProgrammiCarica.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvwProgrammiCarica.AllColumns.Clear();

                flvwProgrammiCarica.View = View.Details;
                flvwProgrammiCarica.ShowGroups = false;
                flvwProgrammiCarica.GridLines = true;

                BrightIdeasSoftware.OLVColumn colIdBreve = new BrightIdeasSoftware.OLVColumn();
                colIdBreve.Text = StringheColonneTabelle.ListaSetup01Id; // "Setup";
                colIdBreve.AspectName = "IdProgramma";
                colIdBreve.Width = 60;
                colIdBreve.HeaderTextAlign = HorizontalAlignment.Left;
                colIdBreve.TextAlign = HorizontalAlignment.Left;
                flvwProgrammiCarica.AllColumns.Add(colIdBreve);

                BrightIdeasSoftware.OLVColumn colDataOra = new BrightIdeasSoftware.OLVColumn();
                colDataOra.Text = StringheColonneTabelle.ListaSetup02Data; //  "Installazione";
                colDataOra.AspectName = "DataInstallazione";
                colDataOra.Width = 100;
                colDataOra.HeaderTextAlign = HorizontalAlignment.Left;
                colDataOra.TextAlign = HorizontalAlignment.Right;
                flvwProgrammiCarica.AllColumns.Add(colDataOra);

                BrightIdeasSoftware.OLVColumn colVdef = new BrightIdeasSoftware.OLVColumn();
                colVdef.Text = StringheColonneTabelle.ListaSetup03Vdef; // "V def";
                colVdef.AspectName = "BatteryVdef";

                colVdef.AspectGetter = delegate (object _Valore)
                {
                    sbProgrammaRicarica _tempVal = (sbProgrammaRicarica)_Valore;
                    return FunzioniMR.StringaTensione(_tempVal.BatteryVdef);
                };
                colVdef.Width = 80;
                colVdef.HeaderTextAlign = HorizontalAlignment.Center;
                colVdef.TextAlign = HorizontalAlignment.Right;
                flvwProgrammiCarica.AllColumns.Add(colVdef);

                BrightIdeasSoftware.OLVColumn colAhdef = new BrightIdeasSoftware.OLVColumn();
                colAhdef.Text = StringheColonneTabelle.ListaSetup04Adef; // "Ah def";
                colAhdef.AspectName = "BatteryAhdef";

                colAhdef.AspectGetter = delegate (object _Valore)
                {
                    sbProgrammaRicarica _tempVal = (sbProgrammaRicarica)_Valore;
                    return FunzioniMR.StringaCapacita(_tempVal.BatteryAhdef, 10);
                };
                colAhdef.Width = 80;
                colAhdef.HeaderTextAlign = HorizontalAlignment.Center;
                colAhdef.TextAlign = HorizontalAlignment.Right;
                flvwProgrammiCarica.AllColumns.Add(colAhdef);

                BrightIdeasSoftware.OLVColumn colBattType = new BrightIdeasSoftware.OLVColumn();
                colBattType.Text = StringheColonneTabelle.ListaSetup05Type; // "Type";
                colBattType.AspectName = "BatteryType";
                colBattType.Width = 80;
                colBattType.HeaderTextAlign = HorizontalAlignment.Center;
                colBattType.TextAlign = HorizontalAlignment.Right;
                flvwProgrammiCarica.AllColumns.Add(colBattType);

                BrightIdeasSoftware.OLVColumn colCelleTot = new BrightIdeasSoftware.OLVColumn();
                colCelleTot.Text = StringheColonneTabelle.ListaSetup06TCell; // "Tot Cells";
                colCelleTot.AspectName = "BatteryCells";
                colCelleTot.Width = 90;
                colCelleTot.HeaderTextAlign = HorizontalAlignment.Center;
                colCelleTot.TextAlign = HorizontalAlignment.Right;
                flvwProgrammiCarica.AllColumns.Add(colCelleTot);

                BrightIdeasSoftware.OLVColumn colV3 = new BrightIdeasSoftware.OLVColumn();
                colV3.Text = StringheColonneTabelle.ListaSetup07Cel3; //  "Celle 3";
                colV3.AspectName = "BatteryCell3";
                colV3.Width = 80;
                colV3.HeaderTextAlign = HorizontalAlignment.Center;
                colV3.TextAlign = HorizontalAlignment.Right;
                flvwProgrammiCarica.AllColumns.Add(colV3);

                BrightIdeasSoftware.OLVColumn colV2 = new BrightIdeasSoftware.OLVColumn();
                colV2.Text = StringheColonneTabelle.ListaSetup08Cel2; // "Celle 2";
                colV2.AspectName = "BatteryCell2";
                colV2.Width = 80;
                colV2.HeaderTextAlign = HorizontalAlignment.Center;
                colV2.TextAlign = HorizontalAlignment.Right;
                flvwProgrammiCarica.AllColumns.Add(colV2);

                BrightIdeasSoftware.OLVColumn colV1 = new BrightIdeasSoftware.OLVColumn();
                colV1.Text = StringheColonneTabelle.ListaSetup09Cel1; // "Celle 1";
                colV1.AspectName = "BatteryCell1";
                colV1.Width = 80;
                colV1.HeaderTextAlign = HorizontalAlignment.Center;
                colV1.TextAlign = HorizontalAlignment.Right;
                flvwProgrammiCarica.AllColumns.Add(colV1);

                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvwProgrammiCarica.AllColumns.Add(colRowFiller);

                flvwProgrammiCarica.RebuildColumns();

                this.flvwProgrammiCarica.SetObjects(_sb.Programmazioni);
                flvwProgrammiCarica.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }

        private void flvwCicliBatteria_FormatRow(object sender, FormatRowEventArgs e)
        {

            //coloro la riga in base al Tipo Evento
            sbMemLunga _memLunga = (sbMemLunga)e.Model;
            switch (_memLunga.TipoEvento)
            {
                case 0xF0:  // "Carica";
                    e.Item.BackColor = Color.LightGreen;
                    break;
                case 0x0F:  //"Scarica";
                    e.Item.BackColor = Color.LightYellow;
                    break;
                case 0xAA:  //"Pausa";
                    e.Item.BackColor = Color.LightGray;
                    break;
                default:    // "Evento Anomalo"
                    e.Item.BackColor = Color.LightPink;
                    break;
            }


            if (_memLunga.NumEventiBrevi == _memLunga.NumEventiBreviCaricati)
            {
                e.Item.CheckState = CheckState.Indeterminate;
            }


        }

        private void flvwCicliBatteria_FormatCell(object sender, FormatCellEventArgs e)
        {
            string _text = e.SubItem.Text;
            if (_text.Contains("** - "))
            {
                e.SubItem.Text = e.SubItem.Text.Substring(5);
                e.SubItem.ForeColor = Color.Red;

            }


        }

        private void flvwCicliBatteria_DoubleClick(object sender, FormatCellEventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;

                if (_lista.SelectedObject != null)
                {
                    MostraDettaglioRiga((sbMemLunga)_lista.SelectedObject);
                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }

        }

        private void flvwCicliBatteria_ItemActivate(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Lancio la lettura dei ciclo brevi per i linghi selezionati
        /// </summary>
        /// <returns></returns>
        private bool RicaricaBreviSelezionati()
        {
            ListaRegistrazioni Lista = new ListaRegistrazioni();  // lista dei lunghi selezionati
            try
            {
                Lista.Elenco.Clear();
                Lista.RecordFigli = 0;
                Lista.RecordMaster = 0;
                foreach (sbMemLunga _memLunga in flvwCicliBatteria.CheckedObjects)
                {

                    Registrazione Elemento = new Registrazione();
                    Elemento.IdLocale = _memLunga.IdLocale;
                    Elemento.Pointer = _memLunga.PuntatorePrimoBreve;
                    Elemento.NumFigli = _memLunga.NumEventiBrevi;
                    Elemento.Record = _memLunga;
                    Lista.Elenco.Add(Elemento);
                    Lista.RecordMaster++;
                    Lista.RecordFigli += Elemento.NumFigli;
                }


                Log.Debug("Lancio lettura Brevi per " + Lista.Elenco.Count.ToString() + " cicli lunghi");
                this.Cursor = Cursors.WaitCursor;
                //frmAvanzamentoCicli _avCicli = new frmAvanzamentoCicli();


                _avCicli.ParametriWorker.MainCount = 100;

                _avCicli.sbLocale = _sb;
                _avCicli.ValStart = 0;
                _avCicli.ValFine = Lista.Elenco.Count();
                _avCicli.ListaCicli = Lista;
                _avCicli.DbDati = _logiche.dbDati.connessione;
                _avCicli.ListaCicli = Lista;
                _avCicli.TipoComando = elementiComuni.tipoMessaggio.MemBreve;
                Log.Debug("FRM RicaricaCicli: <brevi>");

                _avCicli.ShowDialog(this);
                // Ricarico la ListView
                CaricaCicli();

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("RicaricaBreviSelezionati: " + Ex.Message);
                return false;
            }
        }


        private void btnInizializzaFlvw_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            RicaricaCiclidaMem();
            this.Cursor = Cursors.Default;
        }

        private void btnCaricaDettaglioSel_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            RicaricaBreviSelezionati();
            this.Cursor = Cursors.Default;
        }

        private void btnSalvaCliente_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            salvaCliente(_apparatoPresente);
            this.Cursor = Cursors.Default;
        }

        private void flvwCicliBatteria_DoubleClick(object sender, EventArgs e)
        {

        }

        private void flvwCicliBatteria_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;

                if (_lista.SelectedObject != null)
                {
                    MostraDettaglioRiga();
                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }

        }

        private void btnRicaricaProgr_Click(object sender, EventArgs e)
        {

            this.Cursor = Cursors.WaitCursor;

            // 18/11/15 - Prima di ricaricare la lista, ricarico la restata per leggere il contatore aggiornato


            txtCicliProgrammazione.Text = _sb.sbData.ProgramCount.ToString();

            RicaricaProgrammazioni();
            CaricaProgrammazioni();

            this.Cursor = Cursors.Default;
        }

        private void btnCaricaDaMemoria_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //frmAvanzamentoCicli AvCicli = new frmAvanzamentoCicli();

            //AvCicli.Show();

            //impostaAvanzamento(Convert.ToInt32(txtMemDa.Text), Convert.ToInt32(txtMemA.Text));
            //mostraAvanzamento(true);

            RicaricaCiclidaMem();

            //mostraAvanzamento(false);
            //AvCicli.Close();

            this.Cursor = Cursors.Default;
        }

        private void chkParLetturaAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkParLetturaAuto.Checked == true)
            {
                tmrLetturaAutomatica.Enabled = true;
            }
            else
            {
                tmrLetturaAutomatica.Enabled = false;
            }
        }

        private void btnLeggiVariabili_Click(object sender, EventArgs e)
        {
            bool _esito;
            this.Cursor = Cursors.WaitCursor;
            _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
            MostraVariabili(_esito, (chkDatiDiretti.Checked == true));
            this.Cursor = Cursors.Default;


        }

        private void tmrLetturaAutomatica_Tick(object sender, EventArgs e)
        {
            try
            {
                bool _esito;
                this.Cursor = Cursors.WaitCursor;
                _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                MostraVariabili(_esito, (chkDatiDiretti.Checked == true));
            }

            catch
            {

            }

            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnNuovoProgramma_Click(object sender, EventArgs e)
        {
            MostraNuovoProgramma();
        }

        private void btnMemProgrammed_Click(object sender, EventArgs e)
        {
            bool _esito;
            this.Cursor = Cursors.WaitCursor;
            if (_apparatoPresente) _esito = _sb.AttivaProgramma();
            this.Cursor = Cursors.Default;

        }

        private void btnCaricaListaUltimiLunghi_Click(object sender, EventArgs e)
        {
            uint _primoElemento = 1;
            sbMemLunga _tempLungo;
            this.Cursor = Cursors.WaitCursor;
            bool _caricaBrevi = (chkCaricaBrevi.Checked == true);

            if (_sb.CicliMemoriaLunga.Count > 0)
            {
                _tempLungo = _sb.CicliMemoriaLunga[0];
                _primoElemento = _tempLungo.IdMemoriaLunga;
            }

            Log.Debug("Lancio lettura lunghi");
            this.Cursor = Cursors.WaitCursor;
            _avCicli.ParametriWorker.MainCount = 100;
            _avCicli.sbLocale = _sb;
            _avCicli.ValStart = (int)_primoElemento;
            _avCicli.ValFine = (int)_sb.sbData.LongMem;
            _avCicli.DbDati = _logiche.dbDati.connessione;
            _avCicli.CaricaBrevi = chkCaricaBrevi.Checked;
            _avCicli.TipoComando = elementiComuni.tipoMessaggio.MemLunga;
            Log.Debug("FRM RicaricaCicli: ");

            //_esito = _sb.RicaricaCaricaCicliMemLunga(Inizio, (uint)_sb.sbData.LongMem, _logiche.dbDati.connessione, true, CaricaBrevi);

            // Apro il form con le progressbar
            _avCicli.ShowDialog(this);
            //bool _caricaBrevi = ( chkCaricaBrevi.Checked == true );
            //RicaricaCicli(1, _caricaBrevi);
            CaricaCicli();

            btnCaricaDettaglioSel.Enabled = _apparatoPresente;
            //_avCicli.Dispose();
            this.Cursor = Cursors.Default;


        }


        /// <summary>
        /// Evento CLICK del pulsante [Carica Cicli]: se ho meno di 25 cicli da leggere leggo puntualmente lunghi e brevi, 
        /// se no faccio il dump dell'intera memoria. al termine del dump rileggo l'ultimo ciclo per forzare la chiusura 
        /// del lungo corrente.
        /// 
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void cmdRicaricaDati_Click(object sender, EventArgs e)
        {
            try
            {

                if (((_sb.sbData.LongMem - _sb.CicliMemoriaLunga.Count) < 25) && chkAckDumpMem.Checked != true)
                {
                    uint _primoElemento = 1;
                    sbMemLunga _tempLungo;
                    this.Cursor = Cursors.WaitCursor;
                    bool _caricaBrevi = (chkCaricaBrevi.Checked == true);

                    if (_sb.CicliMemoriaLunga.Count > 0)
                    {
                        _tempLungo = _sb.CicliMemoriaLunga[0];
                        _primoElemento = _tempLungo.IdMemoriaLunga;
                    }

                    Log.Debug("Lancio lettura lunghi");
                    this.Cursor = Cursors.WaitCursor;
                    _avCicli.ParametriWorker.MainCount = 100;
                    _avCicli.sbLocale = _sb;
                    _avCicli.ValStart = (int)_primoElemento;
                    _avCicli.ValFine = (int)_sb.sbData.LongMem;
                    _avCicli.DbDati = _logiche.dbDati.connessione;
                    _avCicli.CaricaBrevi = chkCaricaBrevi.Checked;
                    _avCicli.TipoComando = elementiComuni.tipoMessaggio.MemLunga;
                    Log.Debug("FRM RicaricaCicli: ");

                    //_esito = _sb.RicaricaCaricaCicliMemLunga(Inizio, (uint)_sb.sbData.LongMem, _logiche.dbDati.connessione, true, CaricaBrevi);

                    // Apro il form con le progressbar
                    _avCicli.ShowDialog(this);
                    //bool _caricaBrevi = ( chkCaricaBrevi.Checked == true );
                    //RicaricaCicli(1, _caricaBrevi);
                    CaricaCicli();
                    CaricaTestata(_sb.Id, _logiche, _apparatoPresente);
                    btnCaricaDettaglioSel.Enabled = _apparatoPresente;
                    RicalcolaStatistiche();
                    //_avCicli.Dispose();
                    this.Cursor = Cursors.Default;
                }
                else
                {


                    bool _esito;
                    this.Cursor = Cursors.WaitCursor;
                    CaricaTestata(_sb.Id, _logiche, _apparatoPresente);
                    if (chkEraseDB.Checked == true)
                        _esito = _sb.sbData.cancellaDati(_sb.Id);
                    
                    DumpInteraMemoria(false);
                    this.Cursor = Cursors.Default;
                }
            }
            catch
            {
                this.Cursor = Cursors.Default;

            }
        }


        /// <summary>
        /// Scarica dalla scheda i dati dei cicli.      
        /// </summary>
        /// <param name="LimiteCicli">Numero max di cicli scaricabili singolarmente.</param>
        /// <param name="ForzaDump">if set to <c>true</c> [forza dump].</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool LeggiDatiCicli(int LimiteCicli = 25,bool ForzaDump = false,  bool CreaClone = false)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                bool _dumpNecessario = false;

                if (CreaClone) _dumpNecessario = true;
                if (!_dumpNecessario)
                {
                    if ((_sb.sbData.LongMem - _sb.CicliMemoriaLunga.Count) > LimiteCicli) _dumpNecessario = true;
                }


                if (((_sb.sbData.LongMem - _sb.CicliMemoriaLunga.Count) > LimiteCicli) || chkAckDumpMem.Checked == true)
                {
                    // ho un numero di cicli superiore al massimo o ho espressamente chiesto il dump: parto con quello

                    bool _esito;
                   
                    CaricaTestata(_sb.Id, _logiche, _apparatoPresente);
                    if (chkEraseDB.Checked == true)
                        _esito = _sb.sbData.cancellaDati(_sb.Id);
                    ;
                    DumpInteraMemoria(false);
                    this.Cursor = Cursors.Default;

                }

                //........................................

                if (((_sb.sbData.LongMem - _sb.CicliMemoriaLunga.Count) < 25) && chkAckDumpMem.Checked != true)
                {
                    uint _primoElemento = 1;
                    sbMemLunga _tempLungo;
                    this.Cursor = Cursors.WaitCursor;
                    bool _caricaBrevi = (chkCaricaBrevi.Checked == true);

                    if (_sb.CicliMemoriaLunga.Count > 0)
                    {
                        _tempLungo = _sb.CicliMemoriaLunga[0];
                        _primoElemento = _tempLungo.IdMemoriaLunga;
                    }

                    Log.Debug("Lancio lettura lunghi");
                    this.Cursor = Cursors.WaitCursor;
                    _avCicli.ParametriWorker.MainCount = 100;
                    _avCicli.sbLocale = _sb;
                    _avCicli.ValStart = (int)_primoElemento;
                    _avCicli.ValFine = (int)_sb.sbData.LongMem;
                    _avCicli.DbDati = _logiche.dbDati.connessione;
                    _avCicli.CaricaBrevi = chkCaricaBrevi.Checked;
                    _avCicli.TipoComando = elementiComuni.tipoMessaggio.MemLunga;
                    Log.Debug("FRM RicaricaCicli: ");

                    //_esito = _sb.RicaricaCaricaCicliMemLunga(Inizio, (uint)_sb.sbData.LongMem, _logiche.dbDati.connessione, true, CaricaBrevi);

                    // Apro il form con le progressbar
                    _avCicli.ShowDialog(this);
                    //bool _caricaBrevi = ( chkCaricaBrevi.Checked == true );
                    //RicaricaCicli(1, _caricaBrevi);
                    CaricaCicli();
                    CaricaTestata(_sb.Id, _logiche, _apparatoPresente);
                    btnCaricaDettaglioSel.Enabled = _apparatoPresente;
                    RicalcolaStatistiche();
                    //_avCicli.Dispose();
                    this.Cursor = Cursors.Default;
                }
                else
                {


                    bool _esito;
                    this.Cursor = Cursors.WaitCursor;
                    CaricaTestata(_sb.Id, _logiche, _apparatoPresente);
                    if (chkEraseDB.Checked == true)
                        _esito = _sb.sbData.cancellaDati(_sb.Id);
                    ;
                    DumpInteraMemoria(false);
                    this.Cursor = Cursors.Default;
                }

                return true;
            }
            catch
            {
                this.Cursor = Cursors.Default;
                return false;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }




        private void btnRigeneraLista_Click(object sender, EventArgs e)
        {
            CaricaCicli();
        }

        private void chkInvertiCorreti_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInvertiCorreti.Checked == true)
            {
                _sb.InvertiVersoCorrentiML(elementiComuni.VersoCorrente.Inverso);
            }
            else
            {
                _sb.InvertiVersoCorrentiML(elementiComuni.VersoCorrente.Diretto);
            }
            flvwCicliBatteria.RefreshObjects(_sb.CicliMemoriaLunga);
        }

        /// <summary>
        /// Chiude la connessione attiva ( USB o RS232 )
        /// Sb Connesso verifica se ci sono dati da salvere e nel caso eseguue il salvataggio
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSpyBat_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // Attivo il pulsante Salv Cliente per forzare il Leave del controllo attivo
                btnSalvaCliente.Select();

                if (!_datiSalvati)
                {
                    DialogResult risposta = MessageBox.Show(StringheComuni.DatiDaSalvareR1 + "\n" + StringheComuni.DatiDaSalvareR2,
                                                            StringheComuni.SalvaDati,
                                                            MessageBoxButtons.YesNoCancel,
                                                            MessageBoxIcon.Warning);



                    if (risposta == System.Windows.Forms.DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }

                    if (risposta == System.Windows.Forms.DialogResult.Yes)
                    {
                        SalvaDati();
                    }
                }

                _parametri.chiudiCanaleSpyBatt();

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat_FormClosing: " + Ex.Message);

            }
        }

        private void tabCb01_Click(object sender, EventArgs e)
        {

        }

        private void chkCaricaBrevi_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnDumpMemoria_Click(object sender, EventArgs e)
        {
            DumpInteraMemoria();
        }

        /// <summary>
        /// Scarica L'intera immagine memoria, aprendo la finastra pop-up di avanzamento.
        /// </summary>
        /// <param name="InviaACK">Se TRUE attiva l'invio dell'ACK in risposta ad ogni pacchetto</param>
        private void DumpInteraMemoria(bool InviaACK = false,bool ScaricaUltimoLungo = false)
        {
            try
            {
                Log.Debug("Lancio lettura intera memoria");
                this.Cursor = Cursors.WaitCursor;

                _avCicli.ParametriWorker.MainCount = 100;
                _avCicli.sbLocale = _sb;
                _avCicli.ValStart = 1;
                _avCicli.ValFine = (int)_sb.sbData.LongMem;
                _avCicli.DbDati = _logiche.dbDati.connessione;
                _avCicli.CaricaBrevi = false;
                _avCicli.TipoComando = elementiComuni.tipoMessaggio.DumpMemoria;
                _avCicli.InviaACK = InviaACK;
                _avCicli.SalvaHexDump = chkSaveHexDump.Checked;
                _avCicli.FileHexDump = txtNomeFileImmagine.Text;

                Log.Debug("-------------------  FRM RicaricaCicli: -------------------");

                // Apro il form con le progressbar
                _avCicli.ShowDialog(this);

                CaricaCicli();
                CaricaProgrammazioni();

                btnCaricaDettaglioSel.Enabled = _apparatoPresente;
                //_avCicli.Dispose();
                this.Cursor = Cursors.Default;
            }

            catch (Exception Ex)
            {
                Log.Error("frmSpyBat.DumpInteraMemoria: " + Ex.Message);

            }
        }


        /// <summary>
        /// Ricalcola le statistiche per lo SB corrente.
        /// </summary>
        public void RicalcolaStatistiche()
        {
            try
            {
                DatiEstrazione TempStat = new DatiEstrazione();

                _stat = new StatMemLungaSB();
                _stat.SoglieAnalisi = _sb.SoglieAnalisi;
                _stat.caricaSoglie();
                _stat.CicliAttesi = _sb.sbCliente.CicliAttesi;
                if (_sb.CicliMemoriaLunga.Count > 0)
                {
                    _stat.CicliMemoriaLunga = _sb.CicliMemoriaLunga;
                    _stat.aggregaValori(optStatPeriodoSel.Checked, dtpStatInizio.Value, dtpStatFine.Value);
                }

                InizializzaVistaSoglie();

                MostraSintesiStatistiche();
                InizializzaCockpitStat();
                InizializzaSchedaConfronti();

                CaricaSchedeGrafico(_stat);

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat.RicalcolaStatistiche: " + Ex.Message);

            }
        }

        /// <summary>
        /// Carica i dati sulla pagina di sintesi statistiche
        /// </summary>
        public void MostraSintesiStatistiche()
        {
            try
            {
                if (_stat.DatiPronti)
                {
                    //Stato Batteria
                    txtStatAttivazione.Text = _stat.AvvioSistemaSt;
                    txtStatGiorniAtt.Text = _stat.NumeroGiorni.ToString();
                    txtStatNCicli.Text = _stat.NumeroCicli.ToString();
                    double _etot = _stat.Etot();
                    if (_etot > 0)
                    {
                        double _eresidua = (_etot - _stat.EnScaricataNorm()) / _etot;
                        if (_eresidua > 100)
                            txtStatSOH.Text = "N.D.";
                        else
                            txtStatSOH.Text = _eresidua.ToString("p2");
                    }
                    else
                    {
                        txtStatSOH.Text = "";
                    }


                    //Scarica
                    txtTempoInScarica.Text = FunzioniMR.StringaDurataBase(_stat.DurataScarica);
                    txtStatNumScariche.Text = _stat.NumeroScariche.ToString();
                    txtStatNumSovrascariche.Text = _stat.NumeroSovrascariche.ToString();
                    txtStatPauseSC.Text = _stat.NumeroPauseBattScarica.ToString();
                    txtStatDoDMedia.Text = _stat.ProfonditaScaricaMedia().ToString("0.0") + "%";
                    txtStatNumScaricheOverT.Text = _stat.NumeroScaricheSovraT.ToString();

                    //Carica
                    txtStatTempoInCarica.Text = FunzioniMR.StringaDurataBase(_stat.DurataCarica);
                    txtStatNumCariche.Text = _stat.NumeroCariche.ToString();
                    txtStatNumCaricheComp.Text = _stat.NumeroCaricheComplete.ToString();
                    txtStatNumCaricheParz.Text = _stat.NumeroCaricheParziali.ToString();
                    txtStatNumCaricheCOverTemp.Text = _stat.NumeroCaricheCompSovraT.ToString();
                    txtStatNumCarichePOverTemp.Text = _stat.NumeroCaricheParzSovraT.ToString();

                    if (_stat.SecondiTotali > 0)
                    {
                        double _fattoreME = _stat.DurataMancanzaElettrolita / _stat.SecondiTotali;
                        txtStatMancElettr.Text = _fattoreME.ToString("p2");
                    }

                    //Energia
                    double _kwtot = (double)_stat.WHtotali / 100;

                    txtStatTotEnergia.Text = _kwtot.ToString("0.0");
                    /*
                    if (_stat.SuperatoMassimoSbilanciamento)
                        txtStatSbilCelle.ForeColor = Color.Red;
                    else
                        txtStatSbilCelle.ForeColor = Color.Black;
                        */

                    if (_stat.NumeroScariche > 0)
                    {
                        double _kwMed = (double)_stat.WHtotali / (double)(100 * _stat.NumeroScariche);
                        txtStatEnergiaMediaKWh.Text = _kwMed.ToString("0.0");
                        _kwMed = (double)_stat.AhTotali / (double)(_stat.NumeroScariche);
                        txtStatEnergiaMediaAh.Text = _kwMed.ToString("0.0");

                    }
                    else
                    {
                        txtStatEnergiaMediaKWh.Text = "0.0";
                        txtStatEnergiaMediaAh.Text = "0.0";

                    }


                    if (_stat.SecondiTotali > 0)
                    {
                        double _fattoreSB = _stat.DurataSbilanciamento / _stat.SecondiTotali;
                        txtStatSbilCelle.Text = _fattoreSB.ToString("p2");
                    }
                    else
                        txtStatSbilCelle.Text = "";


                    //Anomalie
                    txtStatNAnomali.Text = _stat.NumeroAnomalie.ToString();


                    //Ultima Lettura
                    if (_stat.UltimaLettura > DateTime.MinValue)
                        txtDataLastDownload.Text = _stat.UltimaLettura.ToShortDateString();
                    else
                        txtDataLastDownload.Text = "";


                }
                else
                {
                    txtStatNCicli.Text = "";
                    txtStatNumScariche.Text = "";
                }

            }

            catch (Exception Ex)
            {
                Log.Error("frmSpyBat.MostraSintesiStatistiche: " + Ex.Message);

            }
        }

        /// <summary>
        /// Carico la lista delle soglie per le analisi statistiche
        /// </summary>
        private void InizializzaVistaSoglie()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvwListaSoglie.HeaderUsesThemes = false;
                flvwListaSoglie.HeaderFormatStyle = _stile;
                flvwListaSoglie.UseAlternatingBackColors = true;
                flvwListaSoglie.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvwListaSoglie.AllColumns.Clear();

                flvwListaSoglie.View = View.Details;
                flvwListaSoglie.ShowGroups = false;
                flvwListaSoglie.GridLines = true;

                BrightIdeasSoftware.OLVColumn colIdSoglia = new BrightIdeasSoftware.OLVColumn();
                colIdSoglia.Text = "ID";
                colIdSoglia.AspectName = "IdLocale";
                colIdSoglia.Width = 30;
                colIdSoglia.HeaderTextAlign = HorizontalAlignment.Left;
                colIdSoglia.TextAlign = HorizontalAlignment.Right;
                flvwListaSoglie.AllColumns.Add(colIdSoglia);

                BrightIdeasSoftware.OLVColumn colNomeSoglia = new BrightIdeasSoftware.OLVColumn();
                colNomeSoglia.Text = "Soglia";
                colNomeSoglia.AspectName = "NomeSoglia";
                colNomeSoglia.Width = 300;
                colNomeSoglia.HeaderTextAlign = HorizontalAlignment.Left;
                colNomeSoglia.TextAlign = HorizontalAlignment.Left;
                flvwListaSoglie.AllColumns.Add(colNomeSoglia);

                BrightIdeasSoftware.OLVColumn colValoreSoglia = new BrightIdeasSoftware.OLVColumn();
                colValoreSoglia.Text = "Limite";
                colValoreSoglia.AspectName = "strValoreSoglia";
                colValoreSoglia.Width = 100;
                colValoreSoglia.HeaderTextAlign = HorizontalAlignment.Center;
                colValoreSoglia.TextAlign = HorizontalAlignment.Right;
                flvwListaSoglie.AllColumns.Add(colValoreSoglia);



                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvwListaSoglie.AllColumns.Add(colRowFiller);

                flvwListaSoglie.RebuildColumns();

                this.flvwListaSoglie.SetObjects(_sb.SoglieAnalisi.PacchettoSoglie);
                flvwListaSoglie.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }


        private void tabCaricaBatterie_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                bool _esito;

                if (e.TabPage != tabSbFact)
                {
                    //Se non sono nel tab Funzioni di servizio, spengo la lettura automatica
                    chkParLetturaAuto.Checked = false;
                }

                if (e.TabPage == tabSbFact)
                    {
                        //Entrando nel tab Funzioni di servizio, se sono collegato ad uno SB, leggo i parametri, lo stato del Sig e i parametri di sistema.
                        if (_apparatoPresente)
                        {
                            this.Cursor = Cursors.WaitCursor;
                            _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                            MostraVariabili(_esito, (chkDatiDiretti.Checked == true));
                            _sb.CaricaStatoOC(_sb.Id, _apparatoPresente);
                            MostraParametriOC();
                            this.Cursor = Cursors.Default;
                        }
                    }
                if (e.TabPage == tabCb05)
                {
                    // Se entro nel tab orologio, carico l'ora corrente dallo Spy-Batt
                    if (_apparatoPresente) CaricaOrologio();
                }
                if (e.TabPage == tabCb02)
                {
                    if (_sb.IntestazioneSb.numeroProgramma < 1)
                    {
                        // btnAttivaProgrammazione.Enabled = false;
                    }
                    else
                    {
                        // btnAttivaProgrammazione.Enabled = true;
                    }
                }
                if (e.TabPage == tabStatistiche)
                    frmSpyBat_Resize(null, null);
                if (e.TabPage == tabCalibrazione)
                {
                    txtCalFWRichiesto.Text = PannelloCharger.Properties.Settings.Default.VersioneFwRichiesta;

                    if (_apparatoPresente) VerificaAlimentatore(false);

                    if (tabCalibrazione.Height > 300)
                    {
                        pnlCalVerifica.Height = tabCalibrazione.Height - 240;
                    }
                    if (tbpCalibrazioni.Width > 600)
                    {
                        pnlCalGrafico.Width = tbpCalibrazioni.Width - 540;
                    }
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat_tabCaricaBatterie_Selected: " + Ex.Message);

            }
        }

        private void InizializzaOxyGrSingolo()
        {
            try
            {
                this.oxyContainerGrSingolo = new OxyPlot.WindowsForms.PlotView();
                //this.SuspendLayout();
                // 
                // plot1
                // 
                this.oxyContainerGrSingolo.Dock = System.Windows.Forms.DockStyle.Bottom;
                this.oxyContainerGrSingolo.Location = new System.Drawing.Point(10, 10);
                //this.oxyContainer.Margin = new System.Windows.Forms.Padding(0);
                this.oxyContainerGrSingolo.Name = "oxyContainerGrSingolo";
                this.oxyContainerGrSingolo.PanCursor = System.Windows.Forms.Cursors.Hand;
                this.oxyContainerGrSingolo.Size = new System.Drawing.Size(517, 452);
                //this.oxyContainer.TabIndex = 0;
                this.oxyContainerGrSingolo.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
                this.oxyContainerGrSingolo.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
                this.oxyContainerGrSingolo.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
                this.oxyContainerGrSingolo.Click += new System.EventHandler(this.oxyContainerGrSingolo_Click);
                // 

                // tabStatGrafici.Controls.Add(this.oxyContainerGrSingolo);

                oxyGraficoSingolo = new OxyPlot.PlotModel
                {
                    Title = "",
                    Subtitle = "",
                    PlotType = OxyPlot.PlotType.XY,
                    Background = OxyPlot.OxyColors.White
                };

                oxyContainerGrSingolo.Model = oxyGraficoSingolo;

            }

            catch (Exception Ex)
            {
                Log.Error("InizializzaOxyPlotControl: " + Ex.Message);
            }

        }

        private void oxyContainerGrSingolo_Click(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// Disegna i singoli grafici realizzando le singole schede.
        /// </summary>
        /// <param name="DatiStat">Classe con i dati preaggregati in base ai parametri.</param>
        public void CaricaSchedeGrafico(StatMemLungaSB DatiStat)
        {
            int _primaScheda = 1;
            try
            {

                //prima elimino le esistenti
                foreach (TabPage _pagina in tbcStatistiche.TabPages)
                {
                    if (_pagina.Tag == "GRAFICO")
                        tbcStatistiche.TabPages.Remove(_pagina);
                }

                /***************************************************************************************************************************/
                /*   GRAFICO C.F..                                                                                                        */
                /***************************************************************************************************************************/

                if (chkStatGraficoFC.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrCfChiave);
                    _grafico.ToolTipText = chkStatGraficoFC.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoChargeFactor(StringheStatistica.GrCfTitolo);
                    GraficoLivelliOxy(StringheStatistica.GrCfTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }


                /***************************************************************************************************************************/
                /*   GRAFICO Assenza Elettrolita                                                                   */
                /***************************************************************************************************************************/

                if (chkStatGraficoAssEl.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrAssenzaElChiave, new Size(300, 200));
                    _grafico.ToolTipText = chkStatGraficoAssEl.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoMancanzaEl(StringheStatistica.GrAssenzaElTitolo);
                    GraficoTorta(StringheStatistica.GrAssenzaElTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }
                /***************************************************************************************************************************/
                /*   GRAFICO Differenza Temperature in Carica Parziale                                                                     */
                /***************************************************************************************************************************/

                if (chkStatGraficoDTCP.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrDeltaTCPChiave);
                    _grafico.ToolTipText = chkStatGraficoTminS.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoTempertureCicli(SerialMessage.TipoCiclo.Carica, 2, 2, 0, 20, 0, StringheStatistica.GrDeltaTCPTitolo);
                    _grafico.DatiGrafico.StepSoglia = (DatiStat.SogliaDiffTempScarica) / 2;
                    _grafico.DatiGrafico.VersoSoglia = DatiEstrazione.Direzione.Ascendente;
                    GraficoTemperaturaCiclo(StringheStatistica.GrDeltaTCPTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

                /***************************************************************************************************************************/
                /*   GRAFICO Differenza Temperature in Carica Completa                                                                     */
                /***************************************************************************************************************************/

                if (chkStatGraficoDTCC.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrDeltaTCCChiave);
                    _grafico.ToolTipText = chkStatGraficoTminS.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoTempertureCicli(SerialMessage.TipoCiclo.Carica, 2, 2, 0, 20, 1, StringheStatistica.GrDeltaTCCTitolo);
                    _grafico.DatiGrafico.StepSoglia = (DatiStat.SogliaDiffTempScarica) / 2;
                    _grafico.DatiGrafico.VersoSoglia = DatiEstrazione.Direzione.Ascendente;
                    GraficoTemperaturaCiclo(StringheStatistica.GrDeltaTCCTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

                /***************************************************************************************************************************/
                /*   GRAFICO TEMP MAX Carica Parziale                                                                                      */
                /***************************************************************************************************************************/
                if (chkStatGraficoTmaxCP.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrTmaxCPChiave);
                    _grafico.ToolTipText = chkStatGraficoTminS.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoTempertureCicli(SerialMessage.TipoCiclo.Carica, 1, 5, -10, 80, 0, StringheStatistica.GrTmaxCPTitolo);
                    _grafico.DatiGrafico.StepSoglia = (DatiStat.SogliaTempMaxCaricaParziale + 10) / 5;
                    _grafico.DatiGrafico.VersoSoglia = DatiEstrazione.Direzione.Ascendente;
                    GraficoTemperaturaCiclo(StringheStatistica.GrTmaxCPTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

                /***************************************************************************************************************************/
                /*   GRAFICO TEMP MAX Carica Completa                                                                                      */
                /***************************************************************************************************************************/

                if (chkStatGraficoTmaxCC.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrTmaxCCChiave);
                    _grafico.ToolTipText = chkStatGraficoTminS.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoTempertureCicli(SerialMessage.TipoCiclo.Carica, 1, 5, -10, 80, 1, StringheStatistica.GrTmaxCCTitolo);
                    _grafico.DatiGrafico.StepSoglia = (DatiStat.SogliaTempMaxCaricaCompleta + 10) / 5;
                    _grafico.DatiGrafico.VersoSoglia = DatiEstrazione.Direzione.Ascendente;
                    GraficoTemperaturaCiclo(StringheStatistica.GrTmaxCCTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }
                /***************************************************************************************************************************/
                /*   GRAFICO Differenza Temperature in Scarica                                                                             */
                /***************************************************************************************************************************/
                if (chkStatGraficoDTS.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrDeltaTSChiave);
                    _grafico.ToolTipText = chkStatGraficoTminS.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoTempertureCicli(SerialMessage.TipoCiclo.Scarica, 2, 2, 0, 20, -1, StringheStatistica.GrDeltaTSTitolo);
                    _grafico.DatiGrafico.StepSoglia = (DatiStat.SogliaDiffTempScarica) / 2;
                    _grafico.DatiGrafico.VersoSoglia = DatiEstrazione.Direzione.Ascendente;
                    GraficoTemperaturaCiclo(StringheStatistica.GrDeltaTSTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

                /***************************************************************************************************************************/
                /*   GRAFICO TEMP MIN SCARICA                                                                                              */
                /***************************************************************************************************************************/
                if (chkStatGraficoTminS.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrTminSChiave);
                    _grafico.ToolTipText = chkStatGraficoTminS.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoTempertureCicli(SerialMessage.TipoCiclo.Scarica, 0, 5, -20, 70, -1, StringheStatistica.GrTminSTitolo);
                    _grafico.DatiGrafico.StepSoglia = (DatiStat.SogliaTempMinScarica + 20) / 5;
                    _grafico.DatiGrafico.VersoSoglia = DatiEstrazione.Direzione.Discendente;
                    GraficoTemperaturaCiclo(StringheStatistica.GrTminSTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

                /***************************************************************************************************************************/
                /*   GRAFICO TEMP MAX SCARICA                                                                                              */
                /***************************************************************************************************************************/
                if (chkStatGraficoTmaxS.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrTmaxSChiave);
                    _grafico.ToolTipText = chkStatGraficoTmaxS.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoTempertureCicli(SerialMessage.TipoCiclo.Scarica, 1, 5, -10, 80, -1, StringheStatistica.GrTmaxSTitolo);
                    _grafico.DatiGrafico.StepSoglia = (DatiStat.SogliaTempMaxScarica + 10) / 5;
                    _grafico.DatiGrafico.VersoSoglia = DatiEstrazione.Direzione.Ascendente;
                    GraficoTemperaturaCiclo(StringheStatistica.GrTmaxSTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }
                /***************************************************************************************************************************/
                /*   GRAFICO DURATA CARICHE PARZIALI                                                                                      */
                /***************************************************************************************************************************/


                if (chkStatGraficoDurCP.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrDuCPChiave);
                    _grafico.ToolTipText = chkStatGraficoDurCC.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoDurataCicli(SerialMessage.TipoCiclo.Carica, 0, 15, 16, StringheStatistica.GrDuCPTitolo);
                    GraficoDurataCiclo(StringheStatistica.GrDuCPTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

                /***************************************************************************************************************************/
                /*   GRAFICO DURATA CARICHE COMPLETE                                                                                       */
                /***************************************************************************************************************************/

                if (chkStatGraficoDurCC.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrDuCCChiave);
                    _grafico.ToolTipText = chkStatGraficoDurCC.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoDurataCicli(SerialMessage.TipoCiclo.Carica, 1, 30, 28, StringheStatistica.GrDuCCTitolo);
                    GraficoDurataCiclo(StringheStatistica.GrDuCCTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

                /***************************************************************************************************************************/
                /*   GRAFICO D.O.D.                                                                                                        */
                /***************************************************************************************************************************/

                if (chkStatGraficoDoD.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrDODChiave);
                    _grafico.ToolTipText = chkStatGraficoDoD.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoDeepChg(SerialMessage.TipoCiclo.Scarica, StringheStatistica.GrDODTitolo);
                    GraficoLivelliOxy(StringheStatistica.GrDODTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

                /***************************************************************************************************************************/
                /*   GRAFICO TEMPORALE                                                                                                     */
                /***************************************************************************************************************************/

                if (chkStatGraficoTemporale.Checked == true)
                {
                    TabPage _HostTemporale = new TabPage(StringheStatistica.GrTempChiave);
                    _HostTemporale.Name = "tabTemporale";
                    _HostTemporale.Tag = "GRAFICO";
                    _HostTemporale.BackColor = Color.LightYellow;
                    _HostTemporale.ToolTipText = chkStatGraficoTemporale.Text;
                    TbcStatSettimane = new TabControl();

                    // TbcStatSettimane
                    // 

                    TbcStatSettimane.Location = new System.Drawing.Point(3, 3);
                    TbcStatSettimane.Name = "TbcStatSettimane";
                    TbcStatSettimane.SelectedIndex = 0;
                    //_TbcStatSettimane.Size = new System.Drawing.Size(1136, 541);
                    TbcStatSettimane.TabIndex = 0;
                    TbcStatSettimane.ShowToolTips = true;
                    TbcStatSettimane.Width = tbcStatistiche.Width - 18;
                    TbcStatSettimane.Height = tbcStatistiche.Height - 30;
                    _HostTemporale.Controls.Add(TbcStatSettimane);
                    tbcStatistiche.TabPages.Insert(_primaScheda, _HostTemporale);

                    foreach (SettimanaMR _sett in DatiStat.SettimanePresenti)
                    {
                        oxyTabPage _grafico = new oxyTabPage(_sett.settimana.ToString() + "/" + _sett.anno.ToString("0000"));
                        _grafico.ToolTipText = StringheStatistica.Settimana + _sett.settimana.ToString() + "/" + _sett.anno.ToString("0000");
                        _grafico.Tag = "GRAFICO";
                        _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoSettimana(_sett.chiaveSettimana, _grafico.ToolTipText);
                        _grafico.BackColor = Color.LightYellow;
                        GraficoTemporaleSett(_grafico.ToolTipText, _grafico.DatiGrafico, ref _grafico.GraficoBase);
                        TbcStatSettimane.TabPages.Add(_grafico);
                        Log.Debug("Gr. temporale: pag " + _grafico.Text + " in posizione " + TbcStatSettimane.TabPages.IndexOf(_grafico).ToString() + " di " + TbcStatSettimane.TabPages.Count.ToString());
                    }

                }
            }
            catch
            {

            }

        }


        /// <summary>
        /// GraficoCicloOxy : 
        /// </summary>
        /// <param name="TipoCiclo">Tipo ciclo lungo da visualizzare</param>
        /// <param name="TempoRelativo">Se true Orari di registrazione relativi</param>
        public void GraficoCicloOxy(string TitoloGrafico, DatiEstrazione DatiGraph)
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;

                //tabStatGrafici.BackColor = Color.LightYellow;

                // Preparo le serie di valori

                ValoriPuntualiGrafico.Clear();

                if (DatiGraph == null) return;
                if (DatiGraph.DatiValidi != true) return;

                int ValMinX;
                int ValMaxX;

                int ValMinY;
                int ValMaxY;

                ValMinX = 0;
                ValMinY = 0;
                ValMaxX = 100;


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                oxyGraficoSingolo.Series.Clear();
                oxyGraficoSingolo.Axes.Clear();

                oxyGraficoSingolo.Background = OxyPlot.OxyColors.LightYellow;
                oxyGraficoSingolo.PlotAreaBackground = OxyPlot.OxyColors.White;
                oxyGraficoSingolo.PlotAreaBorderThickness = new OxyPlot.OxyThickness(3, 3, 3, 3);


                oxyGraficoSingolo.Title = TitoloGrafico;
                oxyGraficoSingolo.TitleFont = "Utopia";
                oxyGraficoSingolo.TitleFontSize = 18;


                // Creo gli Assi

                OxyPlot.Axes.CategoryAxis AsseCat = new OxyPlot.Axes.CategoryAxis();
                AsseCat.MinorStep = 1;

                OxyPlot.Axes.LinearAxis AsseConteggi = new OxyPlot.Axes.LinearAxis();
                AsseCat.MinorStep = 1;
                AsseConteggi.Minimum = 0;
                AsseConteggi.Maximum = 100;



                //Creo le serie:
                OxyPlot.Series.LineSeries serValore = new OxyPlot.Series.LineSeries();
                serValore.Title = DatiGraph.Titolo;
                serValore.DataFieldX = "Carica";
                serValore.DataFieldY = "Numero";
                serValore.Color = OxyPlot.OxyColors.Blue;


                OxyPlot.Series.ColumnSeries ColValore = new OxyPlot.Series.ColumnSeries();
                ColValore.StrokeThickness = 1;
                ColValore.Title = DatiGraph.TitoloAsseY;

                //ColValore.YAxis.Maximum = DatiGraph.MaxY * 1.5;

                // carico il Dataset

                ValoriPuntualiGrafico.Clear();
                for (int _ciclo = 0; _ciclo < DatiGraph.NumStep; _ciclo++)
                {
                    AsseCat.Labels.Add(DatiGraph.arrayLabel[_ciclo]);
                    //AsseCat.ActualLabels.Add(DatiGraph.arrayLabel[_ciclo]);

                    OxyPlot.Series.ColumnItem colonna = new OxyPlot.Series.ColumnItem();
                    colonna.Value = DatiGraph.arrayValori[_ciclo];
                    colonna.CategoryIndex = -1;
                    if (_ciclo < DatiGraph.StepSoglia)
                        colonna.Color = OxyPlot.OxyColors.Red;
                    else
                        colonna.Color = OxyPlot.OxyColors.Blue;


                    ColValore.Items.Add(colonna);

                }





                oxyGraficoSingolo.Axes.Add(AsseCat);
                oxyGraficoSingolo.Axes.Add(AsseConteggi);


                serValore.XAxisKey = DatiGraph.KeyAsseX;
                serValore.YAxisKey = DatiGraph.KeyAsseY;

                oxyGraficoSingolo.Series.Add(ColValore);


                oxyGraficoSingolo.InvalidatePlot(true);

            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }

        /// <summary>
        /// GraficoCicloOxy : 
        /// </summary>
        /// <param name="TipoCiclo">Tipo ciclo lungo da visualizzare</param>
        /// <param name="TempoRelativo">Se true Orari di registrazione relativi</param>
        public void GraficoLivelliOxy(string TitoloGrafico, DatiEstrazione DatiGraph, ref OxyPlot.PlotModel Grafico)
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;

                // Preparo le serie di valori

                ValoriPuntualiGrafico.Clear();

                if (DatiGraph == null) return;
                if (DatiGraph.DatiValidi != true) return;

                double ValMinX;
                double ValMaxX;

                double ValMinY;
                double ValMaxY;

                ValMinX = 0;
                ValMinY = 0;
                ValMaxX = 100;


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                Grafico.Series.Clear();
                Grafico.Axes.Clear();

                Grafico.Background = OxyPlot.OxyColors.LightYellow;
                Grafico.PlotAreaBackground = OxyPlot.OxyColors.White;
                Grafico.PlotAreaBorderThickness = new OxyPlot.OxyThickness(1, 1, 1, 1);


                Grafico.Title = TitoloGrafico;
                Grafico.TitleFont = "Utopia";
                Grafico.TitleFontSize = 18;
                Grafico.IsLegendVisible = false;


                // Creo gli Assi

                OxyPlot.Axes.CategoryAxis AsseCat = new OxyPlot.Axes.CategoryAxis();
                AsseCat.MinorStep = 1;
                AsseCat.MajorGridlineStyle = OxyPlot.LineStyle.Dash;
                AsseCat.Title = DatiGraph.TitoloAsseX;

                OxyPlot.Axes.LinearAxis AsseConteggi = new OxyPlot.Axes.LinearAxis();
                AsseCat.MinorStep = 1;
                AsseConteggi.Minimum = 0;
                ValMaxY = DatiGraph.MaxY * 1.5;
                AsseConteggi.Maximum = ValMaxY;
                AsseConteggi.MajorGridlineStyle = OxyPlot.LineStyle.Solid;
                AsseConteggi.MinorGridlineStyle = OxyPlot.LineStyle.Dot;
                AsseCat.IsZoomEnabled = false;
                AsseCat.IsPanEnabled = false;
                AsseConteggi.IsZoomEnabled = false;
                AsseConteggi.IsPanEnabled = false;

                AsseConteggi.Title = DatiGraph.TitoloAsseY;



                //Creo le serie:
                OxyPlot.Series.LineSeries serValore = new OxyPlot.Series.LineSeries();
                serValore.Title = DatiGraph.Titolo;
                serValore.DataFieldX = "Carica";
                serValore.DataFieldY = "Numero";
                serValore.Color = OxyPlot.OxyColors.Blue;


                OxyPlot.Series.ColumnSeries ColValore = new OxyPlot.Series.ColumnSeries();
                ColValore.StrokeThickness = 1;
                ColValore.Title = DatiGraph.TitoloAsseY;
                ColValore.FillColor = OxyPlot.OxyColors.Blue;

                // carico il Dataset

                ValoriPuntualiGrafico.Clear();
                for (int _ciclo = 0; _ciclo < DatiGraph.NumStep; _ciclo++)
                {
                    AsseCat.Labels.Add(DatiGraph.arrayLabel[_ciclo]);

                    OxyPlot.Series.ColumnItem colonna = new OxyPlot.Series.ColumnItem();
                    colonna.Value = DatiGraph.arrayValori[_ciclo];
                    colonna.CategoryIndex = -1;
                    double _percCicli = 0;

                    if (DatiGraph.TotLetture > 0)
                    {
                        _percCicli = (double)DatiGraph.arrayValori[_ciclo] / (double)DatiGraph.TotLetture;
                    }
                    if (DatiGraph.VersoSoglia == DatiEstrazione.Direzione.Ascendente)
                    {
                        if (_ciclo >= DatiGraph.StepSoglia)
                            colonna.Color = OxyPlot.OxyColors.Red;
                        else
                            colonna.Color = OxyPlot.OxyColors.Blue;
                    }
                    else
                    {
                        if (_ciclo < DatiGraph.StepSoglia)
                            colonna.Color = OxyPlot.OxyColors.Red;
                        else
                            colonna.Color = OxyPlot.OxyColors.Blue;
                    }


                    if (colonna.Value > 0)
                    {
                        OxyPlot.Annotations.PointAnnotation NotaPunto = new OxyPlot.Annotations.PointAnnotation();
                        NotaPunto.X = _ciclo;
                        NotaPunto.Y = colonna.Value;
                        NotaPunto.Text = StringheStatistica.Cicli + ": " + colonna.Value.ToString();
                        NotaPunto.Text += "\n" + _percCicli.ToString("P1");
                        NotaPunto.TextVerticalAlignment = OxyPlot.VerticalAlignment.Bottom;
                        NotaPunto.Shape = OxyPlot.MarkerType.Cross;
                        Grafico.Annotations.Add(NotaPunto);
                    }



                    ColValore.Items.Add(colonna);

                }





                Grafico.Axes.Add(AsseCat);
                Grafico.Axes.Add(AsseConteggi);


                serValore.XAxisKey = DatiGraph.KeyAsseX;
                serValore.YAxisKey = DatiGraph.KeyAsseY;

                Grafico.Series.Add(ColValore);


                //Grafico.InvalidatePlot(true);

            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }

        public void GraficoDurataCiclo(string TitoloGrafico, DatiEstrazione DatiGraph, ref OxyPlot.PlotModel Grafico)
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;

                // Preparo le serie di valori

                ValoriPuntualiGrafico.Clear();

                if (DatiGraph == null) return;
                if (DatiGraph.DatiValidi != true) return;

                double ValMinX;
                double ValMaxX;

                double ValMinY;
                double ValMaxY;

                ValMinX = 0;
                ValMinY = 0;
                ValMaxX = 100;


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                Grafico.Series.Clear();
                Grafico.Axes.Clear();

                Grafico.Background = OxyPlot.OxyColors.LightYellow;
                Grafico.PlotAreaBackground = OxyPlot.OxyColors.White;
                Grafico.PlotAreaBorderThickness = new OxyPlot.OxyThickness(1, 1, 1, 1);
                Grafico.IsLegendVisible = false;


                Grafico.Title = TitoloGrafico;
                Grafico.TitleFont = "Utopia";
                Grafico.TitleFontSize = 18;


                // Creo gli Assi

                OxyPlot.Axes.CategoryAxis AsseCat = new OxyPlot.Axes.CategoryAxis();
                AsseCat.MinorStep = 1;
                AsseCat.MajorGridlineStyle = OxyPlot.LineStyle.Dash;
                AsseCat.Title = DatiGraph.TitoloAsseX;

                OxyPlot.Axes.LinearAxis AsseConteggi = new OxyPlot.Axes.LinearAxis();
                AsseCat.MinorStep = 1;
                AsseConteggi.Minimum = 0;
                ValMaxY = DatiGraph.MaxY * 1.5;
                AsseConteggi.Maximum = ValMaxY;
                AsseConteggi.MajorGridlineStyle = OxyPlot.LineStyle.Solid;
                AsseConteggi.MinorGridlineStyle = OxyPlot.LineStyle.Dot;

                AsseConteggi.Title = DatiGraph.TitoloAsseY;

                AsseCat.IsZoomEnabled = false;
                AsseCat.IsPanEnabled = false;
                AsseConteggi.IsZoomEnabled = false;
                AsseConteggi.IsPanEnabled = false;



                //Creo le serie:
                OxyPlot.Series.LineSeries serValore = new OxyPlot.Series.LineSeries();
                serValore.Title = DatiGraph.Titolo;
                serValore.DataFieldX = "Durata";
                serValore.DataFieldY = "Numero";
                serValore.Color = OxyPlot.OxyColors.Blue;


                OxyPlot.Series.ColumnSeries ColValore = new OxyPlot.Series.ColumnSeries();
                ColValore.StrokeThickness = 1;
                ColValore.Title = DatiGraph.TitoloAsseY;
                ColValore.FillColor = OxyPlot.OxyColors.Blue;

                // carico il Dataset

                ValoriPuntualiGrafico.Clear();
                for (int _ciclo = 0; _ciclo <= DatiGraph.NumStep; _ciclo++)
                {
                    AsseCat.Labels.Add(DatiGraph.arrayLabel[_ciclo]);
                    //AsseCat.ActualLabels.Add(DatiGraph.arrayLabel[_ciclo]);

                    OxyPlot.Series.ColumnItem colonna = new OxyPlot.Series.ColumnItem();
                    colonna.Value = DatiGraph.arrayValori[_ciclo];
                    colonna.CategoryIndex = -1;
                    double _percCicli = 0;

                    if (DatiGraph.TotLetture > 0)
                    {
                        _percCicli = (double)DatiGraph.arrayValori[_ciclo] / (double)DatiGraph.TotLetture;
                    }

                    if (_ciclo >= DatiGraph.StepSoglia)
                        colonna.Color = OxyPlot.OxyColors.Red;
                    else
                        colonna.Color = OxyPlot.OxyColors.Blue;

                    if (colonna.Value > 0)
                    {
                        OxyPlot.Annotations.PointAnnotation NotaPunto = new OxyPlot.Annotations.PointAnnotation();
                        NotaPunto.X = _ciclo;
                        NotaPunto.Y = colonna.Value;
                        NotaPunto.Text = StringheStatistica.Cicli + ": " + colonna.Value.ToString();
                        NotaPunto.Text += "\n" + _percCicli.ToString("P1");
                        NotaPunto.TextVerticalAlignment = OxyPlot.VerticalAlignment.Bottom;
                        NotaPunto.Shape = OxyPlot.MarkerType.Cross;
                        Grafico.Annotations.Add(NotaPunto);
                    }



                    ColValore.Items.Add(colonna);

                }





                Grafico.Axes.Add(AsseCat);
                Grafico.Axes.Add(AsseConteggi);


                serValore.XAxisKey = DatiGraph.KeyAsseX;
                serValore.YAxisKey = DatiGraph.KeyAsseY;

                Grafico.Series.Add(ColValore);


                //Grafico.InvalidatePlot(true);

            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }

        public void GraficoTemperaturaCiclo(string TitoloGrafico, DatiEstrazione DatiGraph, ref OxyPlot.PlotModel Grafico)
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;

                // Preparo le serie di valori

                ValoriPuntualiGrafico.Clear();

                if (DatiGraph == null) return;
                if (DatiGraph.DatiValidi != true) return;

                double ValMinX;
                double ValMaxX;

                double ValMinY;
                double ValMaxY;

                ValMinX = 0;
                ValMinY = 0;
                ValMaxX = 100;


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                Grafico.Series.Clear();
                Grafico.Axes.Clear();

                Grafico.Background = OxyPlot.OxyColors.LightYellow;
                Grafico.PlotAreaBackground = OxyPlot.OxyColors.White;
                Grafico.PlotAreaBorderThickness = new OxyPlot.OxyThickness(1, 1, 1, 1);


                Grafico.Title = TitoloGrafico;
                Grafico.TitleFont = "Utopia";
                Grafico.TitleFontSize = 18;
                Grafico.IsLegendVisible = false;


                // Creo gli Assi

                OxyPlot.Axes.CategoryAxis AsseCat = new OxyPlot.Axes.CategoryAxis();
                AsseCat.MinorStep = 1;
                AsseCat.MajorGridlineStyle = OxyPlot.LineStyle.Dash;
                AsseCat.Title = DatiGraph.TitoloAsseX;

                OxyPlot.Axes.LinearAxis AsseConteggi = new OxyPlot.Axes.LinearAxis();
                AsseCat.MinorStep = 1;
                AsseConteggi.Minimum = 0;
                ValMaxY = DatiGraph.MaxY * 1.5;
                AsseConteggi.Maximum = ValMaxY;
                AsseConteggi.MajorGridlineStyle = OxyPlot.LineStyle.Solid;
                AsseConteggi.MinorGridlineStyle = OxyPlot.LineStyle.Dot;

                AsseConteggi.Title = DatiGraph.TitoloAsseY;

                AsseCat.IsZoomEnabled = false;
                AsseCat.IsPanEnabled = false;
                AsseConteggi.IsZoomEnabled = false;
                AsseConteggi.IsPanEnabled = false;




                //Creo le serie:
                OxyPlot.Series.LineSeries serValore = new OxyPlot.Series.LineSeries();
                serValore.Title = DatiGraph.Titolo;
                serValore.DataFieldX = "Durata";
                serValore.DataFieldY = "Numero";
                serValore.Color = OxyPlot.OxyColors.Blue;


                OxyPlot.Series.ColumnSeries ColValore = new OxyPlot.Series.ColumnSeries();
                ColValore.StrokeThickness = 1;
                ColValore.Title = DatiGraph.TitoloAsseY;
                ColValore.FillColor = OxyPlot.OxyColors.Blue;

                //ColValore.YAxis.Maximum = DatiGraph.MaxY * 1.5;

                // carico il Dataset

                ValoriPuntualiGrafico.Clear();
                for (int _ciclo = 0; _ciclo <= DatiGraph.NumStep; _ciclo++)
                {
                    AsseCat.Labels.Add(DatiGraph.arrayLabel[_ciclo]);
                    //AsseCat.ActualLabels.Add(DatiGraph.arrayLabel[_ciclo]);

                    OxyPlot.Series.ColumnItem colonna = new OxyPlot.Series.ColumnItem();
                    colonna.Value = DatiGraph.arrayValori[_ciclo];
                    colonna.CategoryIndex = -1;
                    double _percCicli = 0;

                    if (DatiGraph.TotLetture > 0)
                    {
                        _percCicli = (double)DatiGraph.arrayValori[_ciclo] / (double)DatiGraph.TotLetture;
                    }

                    if (DatiGraph.VersoSoglia == DatiEstrazione.Direzione.Ascendente)
                    {
                        if (_ciclo >= DatiGraph.StepSoglia)
                            colonna.Color = OxyPlot.OxyColors.Red;
                        else
                            colonna.Color = OxyPlot.OxyColors.Blue;
                    }
                    else
                    {
                        if (_ciclo < DatiGraph.StepSoglia)
                            colonna.Color = OxyPlot.OxyColors.Red;
                        else
                            colonna.Color = OxyPlot.OxyColors.Blue;
                    }


                    if (colonna.Value > 0)
                    {
                        OxyPlot.Annotations.PointAnnotation NotaPunto = new OxyPlot.Annotations.PointAnnotation();
                        NotaPunto.X = _ciclo;
                        NotaPunto.Y = colonna.Value;
                        NotaPunto.Text = StringheStatistica.Cicli + ": " + colonna.Value.ToString();
                        NotaPunto.Text += "\n" + _percCicli.ToString("P1");
                        NotaPunto.TextVerticalAlignment = OxyPlot.VerticalAlignment.Bottom;
                        NotaPunto.Shape = OxyPlot.MarkerType.Cross;
                        Grafico.Annotations.Add(NotaPunto);
                    }



                    ColValore.Items.Add(colonna);

                }





                Grafico.Axes.Add(AsseCat);
                Grafico.Axes.Add(AsseConteggi);


                serValore.XAxisKey = DatiGraph.KeyAsseX;
                serValore.YAxisKey = DatiGraph.KeyAsseY;

                Grafico.Series.Add(ColValore);


                //Grafico.InvalidatePlot(true);

            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }


        public void GraficoTorta(string TitoloGrafico, DatiEstrazione DatiGraph, ref OxyPlot.PlotModel Grafico)
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;

                // Preparo le serie di valori

                ValoriPuntualiGrafico.Clear();

                if (DatiGraph == null) return;
                if (DatiGraph.DatiValidi != true) return;

                double ValMinX;
                double ValMaxX;

                double ValMinY;
                double ValMaxY;

                ValMinX = 0;
                ValMinY = 0;
                ValMaxX = 100;


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                Grafico.Series.Clear();
                Grafico.Axes.Clear();

                Grafico.Background = OxyPlot.OxyColors.LightYellow;
                Grafico.PlotAreaBackground = OxyPlot.OxyColors.White;
                Grafico.PlotAreaBorderThickness = new OxyPlot.OxyThickness(1, 1, 1, 1);


                Grafico.Title = TitoloGrafico;
                Grafico.TitleFont = "Utopia";
                Grafico.TitleFontSize = 18;
                Grafico.IsLegendVisible = false;
                Grafico.PlotMargins = new OxyPlot.OxyThickness(50, 50, 50, 50);

                //Creo le serie:
                OxyPlot.Series.PieSeries serValore = new OxyPlot.Series.PieSeries();
                serValore.Title = DatiGraph.Titolo;

                serValore.Slices.Add(new OxyPlot.Series.PieSlice("OK", DatiGraph.NumEvOK) { IsExploded = true, Fill = OxyPlot.OxyColors.LightSkyBlue });
                serValore.Slices.Add(new OxyPlot.Series.PieSlice("KO", DatiGraph.NumEvErrore) { IsExploded = true, Fill = OxyPlot.OxyColors.Red });

                Grafico.Series.Add(serValore);


                //Grafico.InvalidatePlot(true);

            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }


        public void GraficoTemporaleSett(string TitoloGrafico, DatiEstrazione DatiGraph, ref OxyPlot.PlotModel Grafico)
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;

                // Preparo le serie di valori

                ValoriPuntualiGrafico.Clear();

                if (DatiGraph == null) return;
                if (DatiGraph.DatiValidi != true) return;

                double ValMinX;
                double ValMaxX;

                double ValMinY;
                double ValMaxY;

                ValMinX = 0;
                ValMinY = 0;
                ValMaxX = 100;


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                Grafico.Series.Clear();
                Grafico.Axes.Clear();

                Grafico.Background = OxyPlot.OxyColors.LightYellow;
                Grafico.PlotAreaBackground = OxyPlot.OxyColors.White;
                Grafico.PlotAreaBorderThickness = new OxyPlot.OxyThickness(1, 1, 1, 1);
                Grafico.LegendBackground = OxyPlot.OxyColors.White;
                Grafico.LegendBorder = OxyPlot.OxyColors.Black;
                Grafico.LegendPlacement = OxyPlot.LegendPlacement.Outside;


                Grafico.Title = TitoloGrafico;
                Grafico.TitleFont = "Arial";
                Grafico.TitleFontSize = 18;


                // Creo gli Assi

                OxyPlot.Axes.LinearAxis AsseGiorni = new OxyPlot.Axes.LinearAxis();
                AsseGiorni.Minimum = 0;
                AsseGiorni.MinorStep = 96;
                AsseGiorni.MajorStep = 288;
                AsseGiorni.Maximum = 2016;
                AsseGiorni.MinimumPadding = 0;
                AsseGiorni.MaximumPadding = 0;
                //AsseGiorni.MajorGridlineStyle = OxyPlot.LineStyle.Dash;
                AsseGiorni.MajorGridlineStyle = OxyPlot.LineStyle.Solid;
                AsseGiorni.MinorGridlineStyle = OxyPlot.LineStyle.Dot;
                AsseGiorni.Title = DatiGraph.TitoloAsseX;
                AsseGiorni.Position = OxyPlot.Axes.AxisPosition.Bottom;
                AsseGiorni.IsAxisVisible = false;
                AsseGiorni.IsZoomEnabled = false;
                AsseGiorni.IsPanEnabled = false;

                OxyPlot.Axes.LinearAxis AsseCarica = new OxyPlot.Axes.LinearAxis();
                //Asse spostato da .1 a .7
                AsseCarica.StartPosition = 0.05;
                AsseCarica.EndPosition = 0.7;
                AsseCarica.AxislineThickness = 10;
                AsseCarica.AxislineStyle = OxyPlot.LineStyle.Solid;
                AsseCarica.AxislineColor = OxyPlot.OxyColors.Green;
                AsseCarica.IsZoomEnabled = false;
                AsseCarica.IsPanEnabled = false;
                AsseCarica.MinorStep = 5;
                AsseCarica.MajorStep = 10;
                AsseCarica.MaximumPadding = 5;
                AsseCarica.Minimum = 0;
                ValMaxY = 110;
                AsseCarica.Maximum = ValMaxY;
                AsseCarica.MajorGridlineStyle = OxyPlot.LineStyle.Solid;
                AsseCarica.MinorGridlineStyle = OxyPlot.LineStyle.Dot;
                AsseCarica.Key = "Chg";
                AsseCarica.Title = DatiGraph.TitoloAsseY;
                AsseCarica.PositionAtZeroCrossing = true;

                OxyPlot.Axes.LinearAxis AsseTemp = new OxyPlot.Axes.LinearAxis();
                //Asse spostato da .7 a 1
                AsseTemp.StartPosition = 0.7;
                AsseTemp.EndPosition = 1;
                AsseTemp.MinorStep = 5;
                AsseTemp.MajorStep = 10;
                AsseTemp.MajorGridlineStyle = OxyPlot.LineStyle.Solid;
                AsseTemp.MajorGridlineColor = OxyPlot.OxyColors.LightSeaGreen;
                AsseTemp.MinorGridlineStyle = OxyPlot.LineStyle.None;
                //AsseTemp.MaximumPadding = 5;
                AsseTemp.Minimum = 00;
                AsseTemp.Maximum = 70;
                AsseTemp.Key = "Temp";
                AsseTemp.Unit = "°C";
                AsseTemp.IsZoomEnabled = false;
                AsseTemp.IsPanEnabled = false;
                AsseTemp.PositionAtZeroCrossing = true;
                AsseTemp.AxislineStyle = OxyPlot.LineStyle.Solid;
                AsseTemp.AxislineColor = OxyPlot.OxyColors.LightSeaGreen;
                AsseTemp.AxislineThickness = 10;



                OxyPlot.Axes.LinearAxis AsseElAvail = new OxyPlot.Axes.LinearAxis();
                //Asse spostato da .7 a 1
                AsseElAvail.StartPosition = 0;
                AsseElAvail.EndPosition = 0.05;

                AsseElAvail.Minimum = 0;
                AsseElAvail.Maximum = 5;
                AsseElAvail.Key = "ElAvail";
                AsseElAvail.Unit = "El.";
                AsseElAvail.IsZoomEnabled = false;
                AsseElAvail.IsPanEnabled = false;
                AsseElAvail.TickStyle = OxyPlot.Axes.TickStyle.None;
                AsseElAvail.TextColor = OxyPlot.OxyColors.Red;



                OxyPlot.Axes.CategoryAxis AsseLabelGiorni = new OxyPlot.Axes.CategoryAxis();
                AsseLabelGiorni.MinorStep = 1;
                AsseLabelGiorni.MinorStep = 3;
                AsseLabelGiorni.Labels.Add(StringheComuni.Lunedi);
                AsseLabelGiorni.Labels.Add(StringheComuni.Martedi);
                AsseLabelGiorni.Labels.Add(StringheComuni.Mercoledi);
                AsseLabelGiorni.Labels.Add(StringheComuni.Giovedi);
                AsseLabelGiorni.Labels.Add(StringheComuni.Venerdi);
                AsseLabelGiorni.Labels.Add(StringheComuni.Sabato);
                AsseLabelGiorni.Labels.Add(StringheComuni.Domenica);
                AsseLabelGiorni.MajorGridlineStyle = OxyPlot.LineStyle.Solid;
                AsseLabelGiorni.MinorGridlineStyle = OxyPlot.LineStyle.Dot;
                AsseLabelGiorni.Position = OxyPlot.Axes.AxisPosition.Bottom;


                Grafico.Axes.Add(AsseGiorni);
                Grafico.Axes.Add(AsseCarica);
                Grafico.Axes.Add(AsseTemp);
                Grafico.Axes.Add(AsseLabelGiorni);
                Grafico.Axes.Add(AsseElAvail);


                //Creo le serie:
                OxyPlot.Series.RectangleBarSeries serCicli = new OxyPlot.Series.RectangleBarSeries();

                // Creo le serie fittizie per la legenda
                OxyPlot.Series.RectangleBarSeries serCarica = new OxyPlot.Series.RectangleBarSeries();
                serCarica.Title = StringheComuni.FaseCarica;
                serCarica.FillColor = OxyPlot.OxyColors.Green;
                Grafico.Series.Add(serCarica);
                OxyPlot.Series.RectangleBarSeries serEqual = new OxyPlot.Series.RectangleBarSeries();
                serEqual.Title = StringheComuni.FaseEqualizzazione;
                serEqual.FillColor = OxyPlot.OxyColors.LightGreen;
                Grafico.Series.Add(serEqual);
                OxyPlot.Series.RectangleBarSeries serScarica = new OxyPlot.Series.RectangleBarSeries();
                serScarica.Title = StringheComuni.FaseScarica;
                serScarica.FillColor = OxyPlot.OxyColors.LightYellow;
                Grafico.Series.Add(serScarica);
                OxyPlot.Series.RectangleBarSeries serElettrolita = new OxyPlot.Series.RectangleBarSeries();
                serElettrolita.Title = StringheComuni.PresenzaElett;
                serElettrolita.FillColor = OxyPlot.OxyColors.LightSkyBlue;
                Grafico.Series.Add(serElettrolita);

                // Creo le serie per le temperature
                OxyPlot.Series.LineSeries serTmax = new OxyPlot.Series.LineSeries();
                serTmax.Title = StringheStatistica.GrSettserTmax;  // "Temp Max";
                serTmax.Color = OxyPlot.OxyColors.Orange;
                serTmax.YAxisKey = "Temp";
                //Grafico.Series.Add(serTmax);

                OxyPlot.Series.LineSeries serTmin = new OxyPlot.Series.LineSeries();
                serTmin.Title = StringheStatistica.GrSettserTmin;  //"Temp Min";
                serTmin.Color = OxyPlot.OxyColors.Yellow;
                serTmin.YAxisKey = "Temp";


                OxyPlot.Series.LineSeries serTmed = new OxyPlot.Series.LineSeries();
                serTmed.Title = StringheStatistica.GrSettserTmed;  //"Temp Media";
                serTmed.Color = OxyPlot.OxyColors.Red;
                serTmed.YAxisKey = "Temp";


                OxyPlot.Series.AreaSeries serTvar = new OxyPlot.Series.AreaSeries();
                serTvar.Title = StringheStatistica.GrSettserTvar;  //"Range Temp";
                serTvar.Fill = OxyPlot.OxyColors.LightGray;
                serTvar.YAxisKey = "Temp";

                serTvar.DataFieldY2 = "Minimum";
                serTvar.StrokeThickness = 0;
                serTvar.DataFieldY = "Maximum";

                // Creo la serie per la presenza elettrolita
                OxyPlot.Series.RectangleBarSeries serElAvail = new OxyPlot.Series.RectangleBarSeries();
                serElAvail.YAxisKey = "ElAvail";


                OxyPlot.Series.RectangleBarItem OxyItem = new OxyPlot.Series.RectangleBarItem();
                bool _marcaPunto = false;
                bool _elPresente = false;


                foreach (_StatCicloSBPeriodo ciclo in DatiGraph.DatiPeriodo)
                {
                    _marcaPunto = false;
                    switch (ciclo.TipoEvento)
                    {
                        case (byte)SerialMessage.TipoCiclo.Carica:
                            OxyItem = new OxyPlot.Series.RectangleBarItem();
                            OxyItem.Color = OxyPlot.OxyColors.Green;
                            OxyItem.Y0 = 0;
                            OxyItem.Y1 = ciclo.StatoCarica;
                            OxyItem.X0 = ciclo.PeriodoTemporale.minutoInizio;
                            OxyItem.X1 = ciclo.PeriodoTemporale.minutoFine;
                            OxyItem.Title = ciclo.IdMemoriaLunga.ToString();
                            serCicli.Items.Add(OxyItem);
                            Log.Debug("Carica  - Ciclo " + ciclo.IdMemoriaLunga.ToString() + " " + OxyItem.X0.ToString() + " - " + OxyItem.X1.ToString()
                                                                                          + " | " + ciclo.Durata.ToString() + " / " + ciclo.PeriodoTemporale.giornoInizio.ToString()
                                                                                          + " - " + ciclo.PeriodoTemporale.giornoFine.ToString());
                            _marcaPunto = true;
                            break;

                        case (byte)SerialMessage.TipoCiclo.Equal:
                            OxyItem = new OxyPlot.Series.RectangleBarItem();
                            OxyItem.Color = OxyPlot.OxyColors.LightGreen;
                            OxyItem.Y0 = 0;
                            OxyItem.Y1 = ciclo.StatoCarica;
                            OxyItem.X0 = ciclo.PeriodoTemporale.minutoInizio;
                            OxyItem.X1 = ciclo.PeriodoTemporale.minutoFine;
                            OxyItem.Title = ciclo.IdMemoriaLunga.ToString();
                            serCicli.Items.Add(OxyItem);
                            Log.Debug("Carica  - Ciclo " + ciclo.IdMemoriaLunga.ToString() + " " + OxyItem.X0.ToString() + " - " + OxyItem.X1.ToString()
                                                                                          + " | " + ciclo.Durata.ToString() + " / " + ciclo.PeriodoTemporale.giornoInizio.ToString()
                                                                                          + " - " + ciclo.PeriodoTemporale.giornoFine.ToString());
                            _marcaPunto = true;
                            break;

                        case (byte)SerialMessage.TipoCiclo.Scarica:

                            OxyItem = new OxyPlot.Series.RectangleBarItem();
                            OxyItem.Color = OxyPlot.OxyColors.LightYellow;
                            OxyItem.Y0 = 0;
                            OxyItem.Y1 = ciclo.StatoCarica;
                            OxyItem.X0 = ciclo.PeriodoTemporale.minutoInizio;
                            OxyItem.X1 = ciclo.PeriodoTemporale.minutoFine;
                            OxyItem.Title = ciclo.IdMemoriaLunga.ToString();
                            Log.Debug("Scarica - Ciclo " + ciclo.IdMemoriaLunga.ToString() + " " + OxyItem.X0.ToString() + " - " + OxyItem.X1.ToString()
                                                                                          + " | " + ciclo.Durata.ToString() + " / " + ciclo.PeriodoTemporale.giornoInizio.ToString()
                                                                                          + " - " + ciclo.PeriodoTemporale.giornoFine.ToString());
                            serCicli.Items.Add(OxyItem);
                            _marcaPunto = true;
                            break;

                        default:
                            // non faccio nulla
                            break;
                    }

                    // In ogni caso metto i punti temp.

                    int puntomedio = (ciclo.PeriodoTemporale.minutoInizio + ciclo.PeriodoTemporale.minutoFine) / 2;
                    int _durata = ciclo.PeriodoTemporale.minutoFine - ciclo.PeriodoTemporale.minutoInizio;
                    int _posMin = ciclo.PeriodoTemporale.minutoInizio + 5;// (_durata / 10);
                    int _posMax = ciclo.PeriodoTemporale.minutoFine - 5;//  minutoInizio + (_durata * 9 / 10);

                    //----------------------------------------------------------
                    // Temperatura Massima
                    //----------------------------------------------------------
                    OxyPlot.DataPoint _puntoMax = new OxyPlot.DataPoint(_posMin, ciclo.TempMax);

                    //_puntoMax.X = _posMin;
                    //_puntoMax.Y = ciclo.TempMax;
                    serTmax.Points.Add(_puntoMax);
                    serTvar.Points2.Add(_puntoMax);
                    _puntoMax = new OxyPlot.DataPoint(_posMax, ciclo.TempMax);
                    //_puntoMax.X = _posMax;
                    //_puntoMax.Y = ciclo.TempMax;
                    serTmax.Points.Add(_puntoMax);
                    serTvar.Points2.Add(_puntoMax);


                    //----------------------------------------------------------
                    // Temperatura Minima
                    //----------------------------------------------------------
                    OxyPlot.DataPoint _puntoMin = new OxyPlot.DataPoint(_posMin, ciclo.TempMin);
                    //_puntoMin.X = _posMin;
                    //_puntoMin.Y = ciclo.TempMin;
                    serTmin.Points.Add(_puntoMin);
                    serTvar.Points.Add(_puntoMin);
                    _puntoMin = new OxyPlot.DataPoint(_posMax, ciclo.TempMin);
                    //_puntoMin.X = _posMax;
                    //_puntoMin.Y = ciclo.TempMin;
                    serTmin.Points.Add(_puntoMin);
                    serTvar.Points.Add(_puntoMin);


                    //----------------------------------------------------------
                    // Temperatura Media
                    //----------------------------------------------------------

                    OxyPlot.DataPoint _punto = new OxyPlot.DataPoint(_posMin, (double)(ciclo.TempMin + ciclo.TempMax) / 2);
                    serTmed.Points.Add(_punto);
                    _punto = new OxyPlot.DataPoint(_posMax, (double)(ciclo.TempMin + ciclo.TempMax) / 2);
                    serTmed.Points.Add(_punto);

                    //----------------------------------------------------------
                    // Presenza Elettrolita
                    //----------------------------------------------------------

                    OxyItem = new OxyPlot.Series.RectangleBarItem();
                    if (ciclo.PresenzaElettrolita == 0xF0)
                    {
                        OxyItem.Color = OxyPlot.OxyColors.LightSkyBlue;
                    }
                    else
                    {
                        OxyItem.Color = OxyPlot.OxyColors.Red;
                    }


                    OxyItem.Y0 = 1;
                    OxyItem.Y1 = 4;
                    OxyItem.X0 = ciclo.PeriodoTemporale.minutoInizio;
                    OxyItem.X1 = ciclo.PeriodoTemporale.minutoFine;
                    serElAvail.Items.Add(OxyItem);


                    _marcaPunto = false;


                    if (_marcaPunto == true)
                    {
                        OxyPlot.Annotations.PointAnnotation NotaPunto = new OxyPlot.Annotations.PointAnnotation();
                        NotaPunto.X = (OxyItem.X0 + OxyItem.X1) / 2;
                        NotaPunto.Y = OxyItem.Y1;
                        NotaPunto.Text = "Ciclo: " + ciclo.IdMemoriaLunga.ToString();
                        //NotaPunto.Text += "\n" + _percCicli.ToString("P1");
                        NotaPunto.TextVerticalAlignment = OxyPlot.VerticalAlignment.Bottom;
                        NotaPunto.Shape = OxyPlot.MarkerType.Cross;
                        Grafico.Annotations.Add(NotaPunto);
                    }


                }



                Grafico.Series.Add(serCicli);
                Grafico.Series.Add(serTvar);
                Grafico.Series.Add(serTmin);
                Grafico.Series.Add(serTmax);
                Grafico.Series.Add(serTmed);
                Grafico.Series.Add(serElAvail);

                serCicli.MouseDown += SerCicli_MouseDown;
                serTmin.MouseDown += SerTmin_MouseDown;

            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }


        private void SerCicli_MouseDown(object sender, OxyPlot.OxyMouseDownEventArgs e)
        {
            try
            {
                // Se Doppio Click
                if (e.ClickCount > 1)
                {
                    OxyPlot.Series.RectangleBarSeries serie = (OxyPlot.Series.RectangleBarSeries)sender;
                    //Cerco l'elemento corrispondente
                    foreach (OxyPlot.Series.RectangleBarItem cella in serie.Items)
                    {

                        double _px = serie.InverseTransform(e.Position).X;
                        double _py = serie.InverseTransform(e.Position).Y;

                        if ((_px > cella.X0) & (_px < cella.X1) & (_py > cella.Y0) & (_py < cella.Y1))
                        {
                            UInt32 _TempId;
                            if (UInt32.TryParse(cella.Title, out _TempId))
                            {
                                MostraDettaglioFase(_TempId);
                            }

                            return;
                        }
                    }
                }
            }
            catch
            {

            }

        }

        private void SerTmin_MouseDown(object sender, OxyPlot.OxyMouseDownEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                MessageBox.Show(e.ToString());
            }
        }


        private void tabSbFact_Click(object sender, EventArgs e)
        {

        }

        private void btnAttivaProgrammazione_Click(object sender, EventArgs e)
        {
            bool _esito = false;
            this.Cursor = Cursors.WaitCursor;
            if (_apparatoPresente)
            {
                _esito = _sb.AttivaProgramma();
                if (!_esito)
                {
                    MessageBox.Show(StringheComuni.AttivaConfigKO, StringheComuni.TitoloCfg, MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            this.Cursor = Cursors.Default;

        }

        private void txtVarMemProgrammed_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnStampaScheda_Click(object sender, EventArgs e)
        {
            //stampaScheda();
        }

        private void optStatInteroIntervallo_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtMarcaBat_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnStatRicalcola_Click(object sender, EventArgs e)
        {
            RicalcolaStatistiche();
        }



        private void cmdMemRead_Click(object sender, EventArgs e)
        {
            try
            {
                uint _StartAddr;
                ushort _NumByte;
                bool _esito;

                if (chkMemHex.Checked)
                {
                    if (uint.TryParse(txtMemAddrR.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true) return;
                }
                else
                {

                    if (uint.TryParse(txtMemAddrR.Text, out _StartAddr) != true) return;
                }



                if (ushort.TryParse(txtMemLenR.Text, out _NumByte) != true) return;

                if (_NumByte < 1) _NumByte = 1;
                if (_NumByte > 242) _NumByte = 242;
                txtMemLenR.Text = _NumByte.ToString();

                if (_StartAddr < 0) _StartAddr = 0;
                if (chkMemHex.Checked)
                    txtMemAddrR.Text = _StartAddr.ToString("X6");
                else
                    txtMemAddrR.Text = _StartAddr.ToString();

                txtMemDataGrid.Text = "";
                _esito = LeggiBloccoMemoria(_StartAddr, _NumByte);


            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }





        }

        public bool LeggiBloccoMemoria(uint StartAddr, ushort NumByte)
        {
            try
            {

                byte[] _Dati;
                bool _esito;

                txtMemDataGrid.Text = "";
                _Dati = new byte[NumByte];
                _esito = _sb.LeggiBloccoMemoria(StartAddr, NumByte, out _Dati);

                if (_esito == true)
                {


                    string _risposta = "";
                    int _colonne = 0;
                    for (int _i = 0; _i < _Dati.Length; _i++)
                    {
                        _risposta += _Dati[_i].ToString("X2") + " ";
                        _colonne += 1;
                        if (_colonne > 0 && (_colonne % 4) == 0) _risposta += "  ";
                        if (_colonne > 15)
                        {
                            _risposta += "\r\n";
                            _colonne = 0;

                        }
                    }
                    txtMemDataGrid.Text = _risposta;

                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LeggiBloccoMemoria: " + Ex.Message);
                return false;
            }

        }

        private void cmdMemClear_Click(object sender, EventArgs e)
        {
            CancellaMemoria(chkMemClearMantieniCliente.Checked);
        }



        void CancellaMemoria(bool MantieniCliente = false)
        {
            try
            {
                bool _esito;
                SerialMessage.Crc16Ccitt codCrc = new SerialMessage.Crc16Ccitt(SerialMessage.InitialCrcValue.NonZero1);

                // Prima chiedo il codice di autorizzazione, se ho meno di 30 cicli registrati
                if (false) //_sb.sbData.LongMem > 30)
                {
                    frmRichiestaCodice _richiesta = new frmRichiestaCodice();
                    _richiesta.ShowDialog();
                    if (_richiesta.DialogResult == DialogResult.OK)
                    {
                        if (_richiesta.CodiceAutorizzazione.Length != 16)
                        {
                            // Lunghezza codice errata. Esco
                            MessageBox.Show(StringheComuni.CancellaMemoriaR3, StringheComuni.CancellaMemoria, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            _richiesta.Dispose();
                            return;
                        }
                        else
                        {
                            //La lunghezza è giusta, tento la decodifica
                            string _codice = "";
                            string _cR = _richiesta.CodiceAutorizzazione;

                            // step 1, riordino
                            _codice = "" + _cR[0] + _cR[15] + _cR[1] + _cR[14] + _cR[2] + _cR[13] + _cR[3] + _cR[12] + _cR[4] + _cR[11] + _cR[5] + _cR[10] + _cR[6] + _cR[9] + _cR[7] + _cR[8];


                            //Step 2,confronto il crc
                            string _dati = _codice.Substring(0, 12);
                            string _crcOrigine = _codice.Substring(12, 4);

                            byte[] PrimaCodifica = Encoding.ASCII.GetBytes(_dati);
                            ushort _crcEsito = codCrc.ComputeChecksum(PrimaCodifica);
                            string _strcrcEsito = _crcEsito.ToString("x4");
                            _strcrcEsito = _strcrcEsito.ToUpper();
                            if (_crcOrigine != _strcrcEsito)
                            {
                                MessageBox.Show(StringheComuni.CancellaMemoriaR3, StringheComuni.CancellaMemoria, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                _richiesta.Dispose();
                                return;
                            }
                            // Il crc corrisponde, proseguo: Confronto l'ID spybatt


                            int _hexId = 0;
                            byte[] array = Encoding.ASCII.GetBytes(_sb.Id);

                            // Loop through contents of the array.
                            int _step = 0;
                            foreach (byte element in array)
                            {
                                _step += 9;
                                _hexId += (element * _step);
                            }
                            string _codiceSB = _hexId.ToString("x6");
                            _codiceSB = _codiceSB.ToUpper();
                            if (_codice.Substring(0, 6) != _codiceSB)
                            {
                                // Codice Spybatt non corrispondente

                                MessageBox.Show(StringheComuni.CancellaMemoriaR5, StringheComuni.CancellaMemoria, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                _richiesta.Dispose();
                                return;
                            }

                            // Lo spybatt è corretto: se il N° cicli non si scosta di più di 5, cancello
                            uint _cicliOrigine;
                            if (uint.TryParse(_codice.Substring(6, 6), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _cicliOrigine) != true)
                            {
                                // La conversione in numero è fallita, esco
                                MessageBox.Show(StringheComuni.CancellaMemoriaR3, StringheComuni.CancellaMemoria, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                _richiesta.Dispose();
                                return;


                            }

                            //ora vedo lo scostamento
                            if ((_sb.sbData.LongMem - _cicliOrigine) > 5)
                            {
                                // i cicli non corrispondono
                                MessageBox.Show(StringheComuni.CancellaMemoriaR4, StringheComuni.CancellaMemoria, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                _richiesta.Dispose();
                                return;

                            }

                            // TUTTO OK, il codice è valido, proseguo
                            _richiesta.Dispose();

                        }

                    }
                    else
                    {
                        _richiesta.Dispose();
                        return;
                    }
                }
                DialogResult risposta = MessageBox.Show(StringheComuni.CancellaMemoriaR1 + "\n" + StringheComuni.CancellaMemoriaR2,
                StringheComuni.CancellaMemoria,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

                if (risposta == System.Windows.Forms.DialogResult.Yes)
                {
                    string _tmpSernum = _sb.sbCliente.SerialNumber;

                    this.Cursor = Cursors.WaitCursor;
                
                   
                    _esito = _sb.CancellaInteraMemoria();
                    if (_esito)
                    {
                        if (!MantieniCliente)
                        {
                            _sb.sbCliente.VuotaRecord(MantieniCliente);
                        }
                        _sb.sbCliente.SerialNumber = _tmpSernum;
                        _sb.ScriviDatiCliente(true);

                        // e ora ricarico i dati
                        //.....

                        RileggiSpyBat();

                        

                        MessageBox.Show(StringheComuni.MemoriaCancellata, StringheComuni.CancellaMemoria, MessageBoxButtons.OK);

                    }


                    this.Cursor = Cursors.Default;




                }
            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemClear_Click: " + Ex.Message);
            }
        }

        /// <summary>
        /// Verifica che il dato inserito nel campo Valore sia un pacchetto esadecimale valido
        /// </summary>
        void VerificaWriteMem()
        {
            try
            {
                byte[] _tempData;
                int _lunghezzaValida;
                int _scartati;
                _lunghezzaValida = HexEncoding.GetByteCount(txtMemDataW.Text);
                txtMemLenW.Text = _lunghezzaValida.ToString();
                _tempData = HexEncoding.GetBytes(txtMemDataW.Text, out _scartati);
                lblMemVerificaValore.Text = HexEncoding.ToString(_tempData);

            }
            catch (Exception Ex)
            {
                Log.Error("VerificaWriteMem: " + Ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            VerificaWriteMem();
        }

        private void txtMemDataW_TextChanged(object sender, EventArgs e)
        {
            VerificaWriteMem();
        }

        private void cmdMemWrite_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] _tempData;
                int _lunghezzaValida;
                int _scartati;
                uint _StartAddr;
                ushort _NumByte;
                bool _esito;

                if (chkMemHexW.Checked)
                {
                    if (uint.TryParse(txtMemAddrW.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true) return;
                }
                else
                {

                    if (uint.TryParse(txtMemAddrW.Text, out _StartAddr) != true) return;
                }

                // if (uint.TryParse(txtMemAddrW.Text, out _StartAddr) != true) return;
                // if (uint.TryParse(txtMemAddrW.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true) return;


                if (_StartAddr < 0) _StartAddr = 0;
                if (chkMemHex.Checked)
                    txtMemAddrW.Text = _StartAddr.ToString("X6");
                else
                    txtMemAddrW.Text = _StartAddr.ToString();


                _lunghezzaValida = HexEncoding.GetByteCount(txtMemDataW.Text);
                txtMemLenW.Text = _lunghezzaValida.ToString();
                if (_lunghezzaValida > 0)
                {
                    _tempData = HexEncoding.GetBytes(txtMemDataW.Text, out _scartati);
                    _esito = _sb.ScriviBloccoMemoria(_StartAddr, (ushort)_lunghezzaValida, _tempData);
                    if (_esito) MessageBox.Show("Memoria Aggiornata", "Scrittura Memoria", MessageBoxButtons.OK);

                }


            }
            catch (Exception Ex)
            {
                Log.Error("VerificaWriteMem: " + Ex.Message);
            }

        }

        private void btnConsolidaBrevi_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            _sb.ConsolidaBrevi();
            this.Cursor = Cursors.Default;

        }

        private void btnDumpMemoria_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSfoglia_Click(object sender, EventArgs e)
        {
            sfdExportDati.Title = "Export HexDump";
            sfdExportDati.Filter = "SPY-BATT HexDump data (*.sbx)|*.sbx|All files (*.*)|*.*";
            sfdExportDati.ShowDialog();
            txtNomeFileImmagine.Text = sfdExportDati.FileName;
        }


        private void InizializzaCalibrazioni()
        {
            try
            {
                //cmbCalSelParametro.Items.Clear();
                _sb.InizializzaParametriCalibrazione();

                bindingSource.DataSource = _sb.ParametriCalibrazione;
                cmbCalSelParametro.DataSource = bindingSource.DataSource;
                cmbCalSelParametro.DisplayMember = "DescrizioneBase";
                cmbCalSelParametro.ValueMember = "IdParametro";

            }
            catch
            {

            }
        }

        private void btnLeggiCalibrazioni_Click(object sender, EventArgs e)
        {
            bool _esito;
            this.Cursor = Cursors.WaitCursor;
            _esito = _sb.CaricaCalibrazioni(_sb.Id, _apparatoPresente);
            MostraCalibrazioni(_esito, (chkDatiDiretti.Checked == true));
            this.Cursor = Cursors.Default;

        }

        private void btnAttivaCalibrazione_Click(object sender, EventArgs e)
        {
            bool _esito;
            this.Cursor = Cursors.WaitCursor;
            _esito = _sb.ModalitaCalibrazione();
            if (_esito)
            {
                _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                MostraVariabili(_esito, (chkDatiDiretti.Checked == true));
                if (_sb.sbVariabili.MemProgrammed == 0x03)
                {
                    grbComandiCalibrazione.Enabled = true;
                    grbCalibrazioni.BackColor = Color.OrangeRed;
                    btnAttivaCalibrazione.Text = "Chiudi Calibrazione";

                }
                else
                {
                    grbComandiCalibrazione.Enabled = false;
                    grbCalibrazioni.BackColor = Color.White;
                    btnAttivaCalibrazione.Text = "Attiva Calibrazione";

                }

            }
            this.Cursor = Cursors.Default;
        }

        private void btnCalScriviParametro_Click(object sender, EventArgs e)
        {
            if (txtCalValoreParametro.Text.Length > 0)
            {
                ushort _valore;
                if (ushort.TryParse(txtCalValoreParametro.Text, out _valore))
                {
                    byte _idParametro = (byte)cmbCalSelParametro.SelectedValue;
                    bool _esito;

                    _esito = _sb.ScriviParametroCal(_idParametro, _valore);
                    if (_esito) txtCalValoreParametro.Text = "Cal " + _idParametro.ToString() + " Ok";

                }
            }
        }

        public void InizializzaSchedaConfronti()
        {
            int largScheda = tbcStatistiche.Width;
            int altScheda = tbcStatistiche.Height;
            /*
                        // prima vuoto la pagina
                        foreach(var _elemento in tabStatComparazioni.Controls)
                        {
                            if (typeof(_elemento) == null)
                            {

                            }
                        }
            */
            //tabStatComparazioni.Controls.Add(oxyContainer);
            double _eresidua;
            double _etot = _stat.Etot();
            if (_etot > 0)
            {
                _eresidua = (_etot - _stat.EnScaricataNorm()) / _etot;
                if (_eresidua > 100)
                    _eresidua = 100;
            }
            else
            {
                _eresidua = 0;
            }

            _eresidua = _eresidua * 100;
            creaContainerOxy(ref grCompSOH, new Point(100, 50), new Size(500, 220), StringheStatistica.SoH_breve);
            creaGraficoStimaOxy(ref grCompSOH, StringheStatistica.SoH_breve, StringheStatistica.percResidua, _eresidua, (int)(_eresidua * 85 / 100), (int)(_eresidua * 70 / 100), 0, 100);
            //creaGraficoStimaOxy(ref grCompSOH, StringheStatistica.SoH_breve, StringheStatistica.percResidua, 80, 70, 60, 0, 100);

            int _ciclisimulati = _stat.NumeroCariche * 10;

            creaContainerOxy(ref grCompRabb, new Point(650, 50), new Size(500, 220), StringheStatistica.Rabbocchi_breve);
            creaGraficoStimaOxy(ref grCompRabb, StringheStatistica.numRabbocchi, StringheStatistica.Numero, (int)(_ciclisimulati / 30), (int)(_ciclisimulati / 7.5), (int)(_ciclisimulati / 6), 0, (int)(_ciclisimulati / 5));

            creaContainerOxy(ref grCompEnConsumata, new Point(100, 300), new Size(500, 220), StringheStatistica.Energia);
            creaGraficoStimaOxy(ref grCompEnConsumata, StringheStatistica.EnergiaConsumata, "KWh", (_stat.KWhCaricati / 0.9), (_stat.KWhCaricati / 0.814), (_stat.KWhCaricati / 0.745), _stat.KWhCaricati, (_stat.KWhCaricati * 1.5));

            double KgCo2 = _stat.KWhCaricati * 0.53705;

            creaContainerOxy(ref grCompCO2, new Point(650, 300), new Size(500, 220), StringheStatistica.Anidride);
            creaGraficoStimaOxy(ref grCompCO2, StringheStatistica.CO2Risparmiata, "Kg CO2", KgCo2 * 0.2311, KgCo2 * 0.1137, 0, 0, KgCo2 / 2);


        }

        // -------------------------------------------------------------------------------------------------------
        // TODO: Creazione grafici comparativi,Da spostare in classe dedicata
        // -------------------------------------------------------------------------------------------------------
        void creaContainerOxy(ref OxyPlot.PlotModel ModelloGR, System.Drawing.Point Location, System.Drawing.Size Size, string ModelName)
        {
            try
            {
                OxyPlot.WindowsForms.PlotView oxyContainer;

                oxyContainer = new OxyPlot.WindowsForms.PlotView();
                oxyContainer.SuspendLayout();

                oxyContainer.Dock = System.Windows.Forms.DockStyle.None;
                oxyContainer.Location = Location;
                //this.oxyContainer.Margin = new System.Windows.Forms.Padding(0);
                oxyContainer.Name = "Container";
                oxyContainer.PanCursor = System.Windows.Forms.Cursors.Hand;
                oxyContainer.Size = Size;
                oxyContainer.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
                oxyContainer.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
                oxyContainer.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
                //this.oxyContainer.Click += new System.EventHandler(this.oxyContainerGrSingolo_Click);

                ModelloGR = new OxyPlot.PlotModel
                {
                    Title = "",
                    Subtitle = "",
                    PlotType = OxyPlot.PlotType.XY,
                    Background = OxyPlot.OxyColors.Green
                };

                oxyContainer.Model = ModelloGR;
                oxyContainer.BackColor = Color.Red;
                tabStatComparazioni.Controls.Add(oxyContainer);
            }

            catch
            {

            }
        }


        void creaGraficoStimaOxy(ref OxyPlot.PlotModel Grafico, string TitoloGrafico, string TitoloMisura, double ValLL, double ValPSW, double ValEDM, double ValMin, double ValMax)
        {
            try
            {

                try
                {
                    string _Flag;
                    string _titoloGrafico = "";
                    string _modelloIntervallo;
                    double _fattoreCorrente = 0;
                    double _dtInSecondi;

                    // Preparo le serie di valori

                    ValoriPuntualiGrafico.Clear();

                    double ValMinX;
                    double ValMaxX;

                    double ValMinY;
                    double ValMaxY;

                    ValMinX = 0;
                    ValMinY = 0;
                    ValMaxX = 100;


                    // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                    Grafico.Series.Clear();
                    Grafico.Axes.Clear();

                    Grafico.Background = OxyPlot.OxyColors.WhiteSmoke;
                    Grafico.PlotAreaBackground = OxyPlot.OxyColors.White;
                    Grafico.PlotAreaBorderThickness = new OxyPlot.OxyThickness(1, 1, 1, 1);


                    Grafico.Title = TitoloGrafico;
                    Grafico.TitleFont = "Utopia";
                    Grafico.TitleFontSize = 18;

                    Grafico.IsLegendVisible = false;

                    // Creo gli Assi

                    OxyPlot.Axes.CategoryAxis AsseCat = new OxyPlot.Axes.CategoryAxis();
                    AsseCat.MinorStep = 1;
                    AsseCat.MajorGridlineStyle = OxyPlot.LineStyle.Dash;
                    AsseCat.Title = StringheStatistica.Tipologia;



                    OxyPlot.Axes.LinearAxis AsseConteggi = new OxyPlot.Axes.LinearAxis();
                    AsseCat.MinorStep = 1;
                    AsseConteggi.Minimum = ValMin;
                    AsseConteggi.Maximum = ValMax;
                    AsseConteggi.MajorGridlineStyle = OxyPlot.LineStyle.Solid;
                    AsseConteggi.MinorGridlineStyle = OxyPlot.LineStyle.Dot;

                    AsseConteggi.Title = TitoloMisura;



                    //Creo le serie:

                    OxyPlot.Series.LineSeries serValore = new OxyPlot.Series.LineSeries();
                    serValore.Title = "Serie";
                    serValore.DataFieldX = "Tipologia";
                    serValore.DataFieldY = "Numero";
                    serValore.Color = OxyPlot.OxyColors.Blue;


                    OxyPlot.Series.ColumnSeries ColValore = new OxyPlot.Series.ColumnSeries();
                    //ColValore.StrokeThickness = 1;
                    ColValore.Title = "ColumnSeries";
                    ColValore.FillColor = OxyPlot.OxyColors.Blue;


                    ValoriPuntualiGrafico.Clear();
                    OxyPlot.Series.ColumnItem colonna;
                    //----------------------------------- LL -------------
                    AsseCat.Labels.Add("LL");
                    colonna = new OxyPlot.Series.ColumnItem();
                    colonna.Value = ValLL;
                    colonna.CategoryIndex = -1;
                    colonna.Color = OxyPlot.OxyColors.Gold;
                    ColValore.Items.Add(colonna);

                    //----------------------------------- PSW -------------
                    AsseCat.Labels.Add("PSW");
                    colonna = new OxyPlot.Series.ColumnItem();
                    colonna.Value = ValPSW;
                    colonna.CategoryIndex = -1;
                    colonna.Color = OxyPlot.OxyColors.Blue;
                    ColValore.Items.Add(colonna);

                    //----------------------------------- EDM -------------
                    AsseCat.Labels.Add("EDM");
                    colonna = new OxyPlot.Series.ColumnItem();
                    colonna.Value = ValEDM;
                    colonna.CategoryIndex = -1;
                    colonna.Color = OxyPlot.OxyColors.Red;
                    ColValore.Items.Add(colonna);

                    Grafico.Axes.Add(AsseCat);
                    Grafico.Axes.Add(AsseConteggi);


                    serValore.XAxisKey = "Tipologia";
                    serValore.YAxisKey = "Numero";

                    Grafico.Series.Add(ColValore);


                    //Grafico.InvalidatePlot(true);

                }

                catch (Exception Ex)
                {
                    Log.Error("GraficoCiclo: " + Ex.Message);
                }







            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);

            }
        }





        public void InizializzaCockpitStat()
        {
            try
            {

                // Prima riga
                // 1.1  -  S.o.H.

                buiStatCockpit.Frame.Clear();

                Ic11 = new IndicatoreCruscotto();
                Ic11.ValueMask = "0";
                Ic11.MinVal = 0;
                Ic11.Lim1 = 50;
                Ic11.Lim2 = 80;
                Ic11.MaxVal = 100;
                Ic11.Verso = IndicatoreCruscotto.VersoValori.Discendente;
                Ic11.InizializzaIndicatore(this.buiStatCockpit, 30, 20, 280, StringheStatistica.GougeSOHr1 + "\n" + StringheStatistica.GougeSOHr2);

                double _etot = _stat.Etot();
                if (_etot > 0)
                {
                    double _eresidua = 100 * (_etot - _stat.EnScaricataNorm()) / _etot;
                    if (_eresidua > 100)
                    {
                        Ic11.ImpostaValore((float)_eresidua);
                    }
                    else
                        Ic11.ImpostaValore((float)_eresidua);
                }


                //  1.2 - D.o.D.

                Ic12 = new IndicatoreCruscotto();
                Ic12.ValueMask = "0";
                Ic12.MostraValore = true;
                Ic12.MinVal = 0;
                Ic12.Lim1 = 50;
                Ic12.Lim2 = 80;
                Ic12.MaxVal = 100;
                Ic12.Verso = IndicatoreCruscotto.VersoValori.Ascendente;
                Ic12.InizializzaIndicatore(this.buiStatCockpit, 330, 20, 280, StringheStatistica.GougeDODr1 + "\n" + StringheStatistica.GougeDODr2);
                Ic12.ImpostaValore((float)_stat.ProfonditaMacroScaricaMedia());



                //  1.3  Sovrascariche

                Ic13 = new IndicatoreCruscotto();
                Ic13.ValueMask = "0";
                Ic13.MostraValore = true;
                Ic13.MinVal = 0;
                Ic13.Lim1 = 5;
                Ic13.Lim2 = 10;
                Ic13.MaxVal = 100;
                Ic13.Verso = IndicatoreCruscotto.VersoValori.Ascendente;
                Ic13.InizializzaIndicatore(this.buiStatCockpit, 630, 20, 280, StringheStatistica.GougeSovrar1 + "\n" + StringheStatistica.GougeSovrar2);
                Ic13.ImpostaValore((float)_stat.MacroScaricheCritiche());


                //  1.4  Cariche Incomplete

                Ic14 = new IndicatoreCruscotto();
                Ic14.ValueMask = "0";
                Ic14.MostraValore = true;
                Ic14.MinVal = 0;
                Ic14.Lim1 = 10;
                Ic14.Lim2 = 20;
                Ic14.MaxVal = 100;
                Ic14.Verso = IndicatoreCruscotto.VersoValori.Ascendente;
                Ic14.InizializzaIndicatore(this.buiStatCockpit, 930, 20, 280, StringheStatistica.GougeCIncr1 + "\n" + StringheStatistica.GougeCIncr2);
                Ic14.ImpostaValore((float)_stat.CaricheParziali());






                //  2.1  Sbilanciamento

                Ic21 = new IndicatoreCruscotto();
                Ic21.ValueMask = "0";
                Ic21.MinVal = 0;
                Ic21.Lim1 = 10;
                Ic21.Lim2 = 20;
                Ic21.MaxVal = 100;
                Ic21.LabelOffset = 40;
                Ic21.InizializzaIndicatore(this.buiStatCockpit, 30, 300, 280, StringheStatistica.GougeSbilr1 + "\n" + StringheStatistica.GougeSbilr2);
                double _fattoreSB = 0;

                /*
                                if (_stat.SecondiTotali > 0)
                                {
                                    _fattoreSB = 100 * _stat.DurataSbilanciamento / _stat.SecondiTotali;

                                }
                                */
                if (_stat.DurataFasiAttive > 0)
                {
                    _fattoreSB = 100 * _stat.DurataSbilanciamentoFA / _stat.DurataFasiAttive;

                }


                Ic21.ImpostaValore((float)_fattoreSB);


                //  2.2  Overtemp
                Ic22 = new IndicatoreCruscotto();
                Ic22.ValueMask = "0";
                Ic22.MinVal = 0;
                Ic22.Lim1 = 5;
                Ic22.Lim2 = 10;
                Ic22.MaxVal = 100;
                Ic22.LabelOffset = 80;
                Ic22.MostraValore = true;
                Ic22.Verso = IndicatoreCruscotto.VersoValori.Ascendente;
                Ic22.InizializzaIndicatore(this.buiStatCockpit, 330, 300, 280, StringheStatistica.GougeOverTr1 + "\n" + StringheStatistica.GougeOverTr2);
                if (_stat.DurataFasiAttive > 0)
                {
                    _fattoreSB = 100 * _stat.DurataOverTempMax / _stat.DurataFasiAttive;

                }
                Ic22.ImpostaValore((float)_fattoreSB);




                //  2.3  Assenza Elettrolita

                Ic23 = new IndicatoreCruscotto();
                Ic23.ValueMask = "0";
                Ic23.MinVal = 0;
                Ic23.Lim1 = 5;
                Ic23.Lim2 = 10;
                Ic23.MaxVal = 100;
                Ic23.LabelOffset = 50;

                Ic23.InizializzaIndicatore(this.buiStatCockpit, 630, 300, 280, StringheStatistica.GougeAssElr1 + "\n" + StringheStatistica.GougeAssElr2);
                double _fattoreME = 0;
                if (_stat.DurataFasiAttive > 0)
                {
                    _fattoreME = 100 * _stat.DurataMancanzaElettrolita / _stat.DurataFasiAttive;

                }

                Ic23.ImpostaValore((float)_fattoreME);



                //  2.4  Pause Critiche

                Ic24 = new IndicatoreCruscotto();
                Ic24.ValueMask = "0";
                Ic24.MinVal = 0;
                Ic24.Lim1 = 5;
                Ic24.Lim2 = 8;
                Ic24.MaxVal = 10;
                Ic24.LabelOffset = 80;
                Ic24.MostraValore = true;
                Ic24.Verso = IndicatoreCruscotto.VersoValori.Ascendente;
                Ic24.InizializzaIndicatore(this.buiStatCockpit, 930, 300, 280, StringheStatistica.GougePauser1 + "\n" + StringheStatistica.GougePauser2);
                //Ic24.ImpostaValore((float)_stat.NumeroSovrascariche);
                Ic24.ImpostaValore((float)_stat.NumeroPauseBattScarica);


            }

            catch (Exception Ex)
            {
                Log.Error("Inizializza Cockpit: " + Ex.Message);
            }

        }




        public void InizializzaIndicatore(NextUI.BaseUI.BaseUI Cruscotto, int X, int Y, int Size, string Titolo)
        {

            CircularFrame frame = new CircularFrame(new Point(X, Y), Size);
            Cruscotto.Frame.Add(frame);
            frame.BackRenderer.CenterColor = Color.LightGray;
            frame.BackRenderer.EndColor = Color.DimGray;
            frame.FrameRenderer.Outline = NextUI.Renderer.FrameRender.FrameOutline.None;

            CircularScaleBar bar = new CircularScaleBar(frame);
            bar.FillGradientType = NextUI.Renderer.RendererGradient.GradientType.DiagonalRight;
            bar.ScaleBarSize = 4;
            bar.FillColor = Color.DarkGray;
            bar.StartValue = 0;
            bar.EndValue = 100;
            bar.StartAngle = 30;
            bar.SweepAngle = 120;
            bar.MajorTickNumber = 11;
            bar.MinorTicknumber = 2;
            bar.TickMajor.EnableGradient = false;
            bar.TickMajor.EnableBorder = false;
            bar.TickMajor.FillColor = Color.White;
            bar.TickMajor.Height = 15;
            bar.TickMajor.Width = 2;
            bar.TickMajor.Type = TickBase.TickType.RoundedRect;
            bar.TickMinor.EnableGradient = false;
            bar.TickMinor.EnableBorder = false;
            bar.TickMinor.FillColor = Color.Gray;
            bar.TickMajor.TickPosition = TickBase.Position.Inner;
            bar.TickMinor.TickPosition = TickBase.Position.Inner;
            bar.TickLabel.TextDirection = CircularLabel.Direction.Horizontal;
            bar.TickLabel.OffsetFromScale = 35;
            bar.TickLabel.LabelFont = new Font(FontFamily.GenericMonospace, 12, FontStyle.Bold);
            bar.TickLabel.FontColor = Color.LightYellow;

            frame.ScaleCollection.Add(bar);

            CircularRange rangeG = new CircularRange(frame);
            rangeG.EnableGradient = false;
            rangeG.StartValue = 85;
            rangeG.EndValue = 100;
            rangeG.StartWidth = 12;
            rangeG.EndWidth = 15;
            rangeG.RangePosition = RangeBase.Position.Inner;
            rangeG.FillColor = Color.Green;

            CircularRange rangeY = new CircularRange(frame);
            rangeY.EnableGradient = false;
            rangeY.StartValue = 80;
            rangeY.EndValue = 85;
            rangeY.StartWidth = 10;
            rangeY.EndWidth = 12;
            rangeY.RangePosition = RangeBase.Position.Inner;
            rangeY.FillColor = Color.Yellow;


            CircularRange rangeR = new CircularRange(frame);
            rangeR.EnableGradient = false;
            rangeR.StartValue = 0;
            rangeR.EndValue = 80;
            rangeR.StartWidth = 1;
            rangeR.EndWidth = 10;
            rangeR.RangePosition = RangeBase.Position.Inner;
            rangeR.FillColor = Color.Red;



            bar.Range.Add(rangeG);
            bar.Range.Add(rangeY);
            bar.Range.Add(rangeR);

            CircularPointer pointer = new CircularPointer(frame);
            pointer.CapPointer.Visible = true;
            pointer.CapOnTop = false;
            pointer.BasePointer.Length = 150;
            pointer.BasePointer.FillColor = Color.Red;
            pointer.BasePointer.PointerShapeType = Pointerbase.PointerType.Type2;
            pointer.BasePointer.OffsetFromCenter = -30;

            bar.Pointer.Add(pointer);

            NumericalFrame nframe = new NumericalFrame(new Rectangle(frame.Rect.Width / 2 - 50, frame.Rect.Height - 80, 100, 30));
            nframe.FrameRenderer.Outline = NextUI.Renderer.FrameRender.FrameOutline.Type1;
            nframe.FrameRenderer.FrameWidth = 1;

            for (int i = 0; i < 5; i++)
            {
                //DigitalPanel7Segment seg = new DigitalPanel7Segment(nframe);
                DigitalPanel14Segment seg = new DigitalPanel14Segment(nframe);
                seg.BackColor = Color.White;
                seg.FontThickness = 2;
                seg.MainColor = Color.Red;
                nframe.Indicator.Panels.Add(seg);


            }
            DigitalPanel14Segment seg2 = new DigitalPanel14Segment(nframe);
            seg2.BackColor = Color.DarkGray;
            seg2.FontThickness = 2;
            seg2.MainColor = Color.Yellow;
            nframe.Indicator.Panels.Add(seg2);
            frame.FrameCollection.Add(nframe);

        }

        private void buiStatCockpit_Load(object sender, EventArgs e)
        {

        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void btnSelFileCicli_Click(object sender, EventArgs e)
        {
            {
                sfdExportDati.Filter = "Comma Separed (*.csv)|*.csv|All files (*.*)|*.*";
                sfdExportDati.ShowDialog();
                txtFileCicli.Text = sfdExportDati.FileName;

            }
        }

        private void btnGeneraCsv_Click(object sender, EventArgs e)
        {
            string[][] output;
            int _ciclo;
            // try
            {
                string filePath = "";
                _ciclo = 0;


                if (txtFileCicli.Text != "")
                {
                    filePath = txtFileCicli.Text;
                    if (!File.Exists(filePath))
                    {
                        File.Create(filePath).Close();
                    }
                    string delimiter = ";";
                    output = new string[][]
                    {
                         new string[]{"Num Ev.","IdTipo Ev.","Data/Ora Inizio", "Data/Ora Fine", "Durata",
                                      "°C min", "°C max", "V Min", "V max", "I min", "I max",
                                      "Pres. El.", "Ah","Val. Ah", "Ah caricati", "Ah scaricati","KWh", "val KWh", "SoC","RF", "STOP","Prog","Num Brevi","Quota SoH" } /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
                    };

                    int length = output.GetLength(0);
                    StringBuilder sb = new StringBuilder();
                    for (int index = 0; index < length; index++)
                        sb.AppendLine(string.Join(delimiter, output[index]));
                    File.AppendAllText(filePath, sb.ToString());



                    int _elementi = _sb.CicliMemoriaLunga.Count;
                    string[] arr;

                    foreach (sbMemLunga _evento in _sb.CicliMemoriaLunga)
                    {
                        string _valSoH = "";
                        if (_evento.TipoEvento == 0x0F) //Scarica
                        {
                            double _tmed = (double)(_evento.TempMax + _evento.TempMin) / 2;
                            double _coeffTermico = _evento.Wh * FunzioniAnalisi.FattoreTermicoSOH(_tmed) / 100;
                            _valSoH = _coeffTermico.ToString();
                        }



                        output = new string[][]
                          {
                             new string[]{ _evento.IdMemoriaLunga.ToString(),
                                           _evento.TipoEvento.ToString(),
                                           _evento.DataOraStart,
                                           _evento.DataOraFine,
                                           _evento.Durata.ToString(),
                                           _evento.strTempMin,
                                           _evento.strTempMax,
                                           _evento.strVmin,
                                           _evento.strVmax,
                                           _evento.strAmin,
                                           _evento.strAmax,
                                           _evento.PresenzaElettrolita.ToString(),

                                           _evento.strAh,
                                           _evento.Ah.ToString(),
                                           _evento.strAhCaricati,
                                           _evento.strAhScaricati,
                                           _evento.strKWh,
                                           _evento.Wh.ToString(),

                                           _evento.strStatoCarica,
                                           _evento.strFattoreCarica,
                                           _evento.strCondizioneStop,
                                           "",//_evento.ProgrammaAttivo.ToString(),
                                           _evento.NumEventiBrevi.ToString(),
                                           _valSoH,

                             }

                          };

                        length = output.GetLength(0);
                        sb = new StringBuilder();
                        for (int index = 0; index < length; index++)
                            sb.AppendLine(string.Join(delimiter, output[index]));
                        File.AppendAllText(filePath, sb.ToString());
                        _ciclo++;

                    }



                    MessageBox.Show("File generato", "Esportazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Inserire un nome valido", "Esportazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }
            /*
            catch (Exception Ex)
            {
                Log.Error("btnGeneraCsv_Click: " + Ex.Message);
                MessageBox.Show(Ex.Message, "Esportazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            */

        }

        private void chkDatiDiretti_CheckedChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Se il form alimentatore è aperto lo collego, se no lo apro
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalCollegaTdk_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmAlimentatore))
                    {
                        Lambda = (frmAlimentatore)form;
                        form.Activate();
                        lblCalConnessione.Text = "COLLEGATO";
                        lblCalStatoAlim.Text = "COLLEGATO";
                        return;
                    }
                }

                frmAlimentatore Alimentatore = new frmAlimentatore();
                Alimentatore.MdiParent = this.MdiParent;
                Alimentatore.StartPosition = FormStartPosition.CenterParent;

                Alimentatore.Show();
                Lambda = Alimentatore;

            }

            catch (Exception Ex)
            {
                //Log.Error("frmMain: " + Ex.Message);
            }

        }

        private void label51_Click(object sender, EventArgs e)
        {

        }

        private void btnCalEseguiSeq_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            LanciaSequenza();
            this.Cursor = Cursors.Default;

        }
        private void btnCalEseguiSeqExt_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            LanciaSequenzaEstesa();
            this.Cursor = Cursors.Default;
        }




        /// <summary>
        /// Carico la lista delle letture per l'analisi corrente
        /// </summary>
        private void InizializzaVistaCorrenti()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 7, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvwLettureCorrente.HeaderUsesThemes = false;
                flvwLettureCorrente.HeaderFormatStyle = _stile;
                flvwLettureCorrente.UseAlternatingBackColors = true;
                flvwLettureCorrente.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvwLettureCorrente.AllColumns.Clear();

                flvwLettureCorrente.View = View.Details;
                flvwLettureCorrente.ShowGroups = false;
                flvwLettureCorrente.GridLines = true;

                BrightIdeasSoftware.OLVColumn colIdSoglia = new BrightIdeasSoftware.OLVColumn();
                colIdSoglia.Text = "ID";
                colIdSoglia.AspectName = "IdLocale";
                colIdSoglia.Width = 30;
                colIdSoglia.HeaderTextAlign = HorizontalAlignment.Left;
                colIdSoglia.TextAlign = HorizontalAlignment.Right;
                //flvwLettureCorrente.AllColumns.Add(colIdSoglia);

                BrightIdeasSoftware.OLVColumn colLettura = new BrightIdeasSoftware.OLVColumn();
                colLettura.Text = "N.";
                colLettura.AspectName = "Lettura";
                colLettura.Width = 40;
                colLettura.HeaderTextAlign = HorizontalAlignment.Center;
                colLettura.TextAlign = HorizontalAlignment.Right;
                flvwLettureCorrente.AllColumns.Add(colLettura);

                BrightIdeasSoftware.OLVColumn colCorrTeorica = new BrightIdeasSoftware.OLVColumn();
                colCorrTeorica.Text = "A def";
                colCorrTeorica.AspectName = "strAteorici";
                colCorrTeorica.Width = 50;
                colCorrTeorica.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrTeorica.TextAlign = HorizontalAlignment.Right;
                flvwLettureCorrente.AllColumns.Add(colCorrTeorica);


                BrightIdeasSoftware.OLVColumn colCorrEffettiva = new BrightIdeasSoftware.OLVColumn();
                colCorrEffettiva.Text = "A eff";
                colCorrEffettiva.AspectName = "strAreali";
                colCorrEffettiva.Width = 50;
                colCorrEffettiva.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrEffettiva.TextAlign = HorizontalAlignment.Right;
                flvwLettureCorrente.AllColumns.Add(colCorrEffettiva);

                BrightIdeasSoftware.OLVColumn colCorrSpyBatt = new BrightIdeasSoftware.OLVColumn();
                colCorrSpyBatt.Text = "A SB";
                colCorrSpyBatt.AspectName = "strAspybatt";
                colCorrSpyBatt.Width = 50;
                colCorrSpyBatt.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrSpyBatt.TextAlign = HorizontalAlignment.Right;
                flvwLettureCorrente.AllColumns.Add(colCorrSpyBatt);

                //-------------------------------------------- 

                BrightIdeasSoftware.OLVColumn colCorrSpyBattAP = new BrightIdeasSoftware.OLVColumn();
                colCorrSpyBattAP.Text = "AP(A)";
                colCorrSpyBattAP.ToolTipText = "Corrente SPY-BATT - Ciclo Ascendente Positivo ";
                colCorrSpyBattAP.AspectName = "strAspybattAP";
                colCorrSpyBattAP.Width = 40;
                colCorrSpyBattAP.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrSpyBattAP.TextAlign = HorizontalAlignment.Right;
                flvwLettureCorrente.AllColumns.Add(colCorrSpyBattAP);

                BrightIdeasSoftware.OLVColumn colCorrSpyBattDP = new BrightIdeasSoftware.OLVColumn();
                colCorrSpyBattDP.Text = "DP(A)";
                colCorrSpyBattDP.ToolTipText = "Corrente SPY-BATT - Ciclo Discendente Positivo ";
                colCorrSpyBattDP.AspectName = "strAspybattDP";
                colCorrSpyBattDP.Width = 50;
                colCorrSpyBattDP.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrSpyBattDP.TextAlign = HorizontalAlignment.Right;
                flvwLettureCorrente.AllColumns.Add(colCorrSpyBattDP);

                BrightIdeasSoftware.OLVColumn colCorrSpyBattAN = new BrightIdeasSoftware.OLVColumn();
                colCorrSpyBattAN.Text = "AN(A)";
                colCorrSpyBattDP.ToolTipText = "Corrente SPY-BATT - Ciclo Ascendente Negativo ";
                colCorrSpyBattAN.AspectName = "strAspybattAN";
                colCorrSpyBattAN.Width = 50;
                colCorrSpyBattAN.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrSpyBattAN.TextAlign = HorizontalAlignment.Right;
                flvwLettureCorrente.AllColumns.Add(colCorrSpyBattAN);

                BrightIdeasSoftware.OLVColumn colCorrSpyBattDN = new BrightIdeasSoftware.OLVColumn();
                colCorrSpyBattDN.Text = "DN(A)";
                colCorrSpyBattDP.ToolTipText = "Corrente SPY-BATT - Ciclo Discendente Negativo ";
                colCorrSpyBattDN.AspectName = "strAspybattDN";
                colCorrSpyBattDN.Width = 50;
                colCorrSpyBattDN.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrSpyBattDN.TextAlign = HorizontalAlignment.Right;
                flvwLettureCorrente.AllColumns.Add(colCorrSpyBattDN);



                //-------------------------------------------- 


                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 50;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvwLettureCorrente.AllColumns.Add(colRowFiller);

                flvwLettureCorrente.RebuildColumns();

                flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                flvwLettureCorrente.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }


        private void InizializzaOxyGrAnalisi()
        {
            try
            {
                this.oxyContainerGrAnalisi = new OxyPlot.WindowsForms.PlotView();
                //this.SuspendLayout();
                // 
                // plot1
                // 
                this.oxyContainerGrAnalisi.Dock = System.Windows.Forms.DockStyle.Bottom;
                this.oxyContainerGrAnalisi.Location = new System.Drawing.Point(10, 10);
                //this.oxyContainer.Margin = new System.Windows.Forms.Padding(0);
                this.oxyContainerGrAnalisi.Name = "oxyContainerGrSingolo";
                this.oxyContainerGrAnalisi.PanCursor = System.Windows.Forms.Cursors.Hand;
                this.oxyContainerGrAnalisi.Size = new System.Drawing.Size(517, 452);
                //this.oxyContainer.TabIndex = 0;
                this.oxyContainerGrAnalisi.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
                this.oxyContainerGrAnalisi.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
                this.oxyContainerGrAnalisi.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
                this.oxyContainerGrAnalisi.Click += new System.EventHandler(this.oxyContainerGrSingolo_Click);
                // 

                pnlCalGrafico.Controls.Add(this.oxyContainerGrAnalisi);

                oxyGraficoAnalisi = new OxyPlot.PlotModel
                {
                    Title = "",
                    Subtitle = "",
                    PlotType = OxyPlot.PlotType.XY,
                    Background = OxyPlot.OxyColors.White
                };

                oxyContainerGrAnalisi.Model = oxyGraficoAnalisi;

            }

            catch (Exception Ex)
            {
                Log.Error("InizializzaOxyPlotControl: " + Ex.Message);
            }

        }


        public void GraficoCorrentiOxy()
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;

                //tabStatGrafici.BackColor = Color.LightYellow;

                // Preparo le serie di valori

                ValoriPuntualiGrCorrenti.Clear();


                int ValMinX;
                int ValMaxX;

                int ValMinY;
                int ValMaxY;

                ValMinX = 0;
                ValMinY = 0;
                ValMaxX = 100;


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                oxyGraficoAnalisi.Series.Clear();
                oxyGraficoAnalisi.Axes.Clear();

                oxyGraficoAnalisi.Background = OxyPlot.OxyColors.LightYellow;
                oxyGraficoAnalisi.PlotAreaBackground = OxyPlot.OxyColors.White;
                oxyGraficoAnalisi.PlotAreaBorderThickness = new OxyPlot.OxyThickness(1, 1, 1, 1);


                oxyGraficoAnalisi.Title = "Correnti";
                oxyGraficoAnalisi.TitleFont = "Utopia";
                oxyGraficoAnalisi.TitleFontSize = 14;


                // Creo gli Assi

                OxyPlot.Axes.CategoryAxis AsseCat = new OxyPlot.Axes.CategoryAxis();
                // AsseCat.MinorStep = 1;

                OxyPlot.Axes.LinearAxis AsseConteggi = new OxyPlot.Axes.LinearAxis();
                // AsseCat.MinorStep = 1;
                // AsseConteggi.Minimum = 0;
                // AsseCo1nteggi.Maximum = 100;



                //Creo le serie:
                OxyPlot.Series.LineSeries serValore = new OxyPlot.Series.LineSeries();
                serValore.Title = "Erogata";
                serValore.DataFieldX = "Lettura";
                serValore.DataFieldY = "Corrente";
                serValore.Color = OxyPlot.OxyColors.Green;

                OxyPlot.Series.LineSeries serLettura = new OxyPlot.Series.LineSeries();
                serLettura.Title = "I Rilevata";
                serLettura.DataFieldX = "Lettura";
                serLettura.DataFieldY = "Corrente";
                serLettura.Color = OxyPlot.OxyColors.Red;



                // carico il Dataset


                float _errore = 0;

                foreach (sbAnalisiCorrente _vac in ValoriTestCorrente)
                {

                    if (_vac.Areali != 0)
                    {
                        _errore = 100 * (_vac.Aspybatt - _vac.Areali * _vac.Spire) / (_vac.Areali * _vac.Spire);
                    }
                    else
                        _errore = 0;

                    serValore.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.Areali * _vac.Spire));
                    serLettura.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.Aspybatt));


                }





                oxyGraficoAnalisi.Axes.Add(AsseCat);



                //serValore.XAxisKey = "Lettura";
                //serValore.YAxisKey = "Corrente";

                oxyGraficoAnalisi.Series.Add(serValore);
                oxyGraficoAnalisi.Series.Add(serLettura);


                oxyGraficoAnalisi.InvalidatePlot(true);

            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }


        public void GraficoCorrentiComplOxy()
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;

                //tabStatGrafici.BackColor = Color.LightYellow;

                // Preparo le serie di valori

                ValoriPuntualiGrCorrenti.Clear();


                int ValMinX;
                int ValMaxX;

                int ValMinY;
                int ValMaxY;

                ValMinX = 0;
                ValMinY = 0;
                ValMaxX = 100;


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                oxyGraficoAnalisi.Series.Clear();
                oxyGraficoAnalisi.Axes.Clear();

                oxyGraficoAnalisi.Background = OxyPlot.OxyColors.LightYellow;
                oxyGraficoAnalisi.PlotAreaBackground = OxyPlot.OxyColors.White;
                oxyGraficoAnalisi.PlotAreaBorderThickness = new OxyPlot.OxyThickness(1, 1, 1, 1);


                oxyGraficoAnalisi.Title = "Correnti";
                oxyGraficoAnalisi.TitleFont = "Utopia";
                oxyGraficoAnalisi.TitleFontSize = 14;

                //oxyGraficoAnalisi.LegendBackground = OxyPlot.OxyColor.W
                oxyGraficoAnalisi.LegendBorder = OxyPlot.OxyColors.Black;
                oxyGraficoAnalisi.LegendPlacement = OxyPlot.LegendPlacement.Inside;
                oxyGraficoAnalisi.LegendPosition = OxyPlot.LegendPosition.TopLeft;

                // Creo gli Assi

                OxyPlot.Axes.CategoryAxis AsseCat = new OxyPlot.Axes.CategoryAxis();
                // AsseCat.MinorStep = 1;

                OxyPlot.Axes.LinearAxis AsseConteggi = new OxyPlot.Axes.LinearAxis();
                // AsseCat.MinorStep = 1;
                // AsseConteggi.Minimum = 0;
                // AsseCo1nteggi.Maximum = 100;



                //Creo le serie:
                OxyPlot.Series.LineSeries serValore = new OxyPlot.Series.LineSeries();
                serValore.Title = "Erogata";
                serValore.DataFieldX = "Lettura";
                serValore.DataFieldY = "Corrente";
                serValore.Color = OxyPlot.OxyColors.Blue;

                OxyPlot.Series.LineSeries serLetturaAP = new OxyPlot.Series.LineSeries();
                serLetturaAP.Title = "I Asc.Pos.";
                serLetturaAP.DataFieldX = "Lettura";
                serLetturaAP.DataFieldY = "Corrente";
                serLetturaAP.Color = OxyPlot.OxyColors.Green;

                OxyPlot.Series.LineSeries serLetturaDP = new OxyPlot.Series.LineSeries();
                serLetturaDP.Title = "I Disc. Pos";
                serLetturaDP.DataFieldX = "Lettura";
                serLetturaDP.DataFieldY = "Corrente";
                serLetturaDP.Color = OxyPlot.OxyColors.GreenYellow;

                OxyPlot.Series.LineSeries serLetturaAN = new OxyPlot.Series.LineSeries();
                serLetturaAN.Title = "I Asc. Neg.";
                serLetturaAN.DataFieldX = "Lettura";
                serLetturaAN.DataFieldY = "Corrente";
                serLetturaAN.Color = OxyPlot.OxyColors.Red;

                OxyPlot.Series.LineSeries serLetturaDN = new OxyPlot.Series.LineSeries();
                serLetturaDN.Title = "I Disc. Neg";
                serLetturaDN.DataFieldX = "Lettura";
                serLetturaDN.DataFieldY = "Corrente";
                serLetturaDN.Color = OxyPlot.OxyColors.OrangeRed;


                // carico il Dataset


                float _errore = 0;

                foreach (sbAnalisiCorrente _vac in ValoriTestCorrente)
                {

                    if (_vac.Areali != 0)
                    {
                        _errore = 100 * (_vac.Aspybatt - _vac.Areali * _vac.Spire) / (_vac.Areali * _vac.Spire);
                    }
                    else
                        _errore = 0;

                    serValore.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.Areali * _vac.Spire));
                    serLetturaAP.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.AspybattAP));
                    serLetturaDP.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.AspybattDP));

                    serLetturaAN.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.AspybattAN));
                    serLetturaDN.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.AspybattDN));

                }





                oxyGraficoAnalisi.Axes.Add(AsseCat);



                //serValore.XAxisKey = "Lettura";
                //serValore.YAxisKey = "Corrente";

                oxyGraficoAnalisi.Series.Add(serValore);
                oxyGraficoAnalisi.Series.Add(serLetturaAP);
                oxyGraficoAnalisi.Series.Add(serLetturaDP);
                oxyGraficoAnalisi.Series.Add(serLetturaAN);
                oxyGraficoAnalisi.Series.Add(serLetturaDN);


                oxyGraficoAnalisi.InvalidatePlot(true);

            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }




        private void si_DataReceived(string data1)
        {
            txtCalCurr.Text = data1;
            //txtCalCurrStep.Text = data2;
        }


        private void chkAccendiAlim_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnCalFilesearch_Click(object sender, EventArgs e)
        {
            {
                sfdExportDati.Filter = "Comma Separed (*.csv)|*.csv|All files (*.*)|*.*";
                sfdExportDati.ShowDialog();
                txtCalFileCicli.Text = sfdExportDati.FileName;

            }
        }

        /// <summary>
        /// Genero il file Excel con i valori rilevati 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tctCalGeneraExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "";
                int _ciclo = 0;


                if (txtCalFileCicli.Text != "")
                {
                    filePath = txtCalFileCicli.Text;
                    if (!File.Exists(filePath))
                    {
                        File.Create(filePath).Close();
                    }
                    string delimiter = ";";
                    string[][] output = new string[][]
                    {
                         new string[]{"SPY-BATT","Nucleo","Ciclo", "Spire", "Err.Pos.","Err.Neg." }
                    };

                    int length = output.GetLength(0);
                    StringBuilder sb = new StringBuilder();
                    for (int index = 0; index < length; index++)
                        sb.AppendLine(string.Join(delimiter, output[index]));
                    File.AppendAllText(filePath, sb.ToString());

                    output = new string[][]
                    {
                         new string[]{_sb.Id,txtCalNumNucleo.Text, txtCalNumCiclo.Text, txtCalSpire.Text, txtCalErroreMaxPos.Text,txtCalErroreMaxNeg.Text}
                    };

                    length = output.GetLength(0);
                    sb = new StringBuilder();
                    for (int index = 0; index < length; index++)
                        sb.AppendLine(string.Join(delimiter, output[index]));
                    File.AppendAllText(filePath, sb.ToString());


                    output = new string[][]
                    {
                         new string[]{"Num Lettura.","A Impostati","A Effettivi", "A Ril. Asc.Pos.", "A Ril. Disc.Pos.","A Ril. Asc.Neg.", "A Ril. Disc.Neg." }
                    };

                    length = output.GetLength(0);
                    sb = new StringBuilder();
                    for (int index = 0; index < length; index++)
                        sb.AppendLine(string.Join(delimiter, output[index]));
                    File.AppendAllText(filePath, sb.ToString());

                    int _elementi = ValoriTestCorrente.Count;
                    string[] arr;

                    foreach (sbAnalisiCorrente _vac in ValoriTestCorrente)
                    {

                        output = new string[][]
                          {
                             new string[]{ _vac.Lettura.ToString(),
                                           _vac.strAteorici,
                                           _vac.strAreali,
                                           _vac.strAspybattAP,
                                           _vac.strAspybattDP,
                                           _vac.strAspybattAN,
                                           _vac.strAspybattDN,
                               }

                          };

                        length = output.GetLength(0);
                        sb = new StringBuilder();
                        for (int index = 0; index < length; index++)
                            sb.AppendLine(string.Join(delimiter, output[index]));
                        File.AppendAllText(filePath, sb.ToString());
                        _ciclo++;

                    }
                    string _fileImmagine = filePath + ".png";
                    using (var stream = File.Create(_fileImmagine))
                    {
                        var pngExporter = new OxyPlot.WindowsForms.PngExporter();
                        pngExporter.Export(oxyGraficoAnalisi, (Stream)stream);//, 600, 400, Brushes.White);
                    }

                    MessageBox.Show("File generato", "Esportazione dati Corrente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Inserire un nome valido", "Esportazione dati Corrente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }

            catch (Exception Ex)
            {
                Log.Error("btnGeneraCsv_Click: " + Ex.Message);
                MessageBox.Show(Ex.Message, "Esportazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            LanciaSequenzaAssoluta();
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Handles the Click event of the btnFWFileCCSsearch control.
        /// Apre la finestra gestione file per la ricerca del file CCS (txt) da caricare
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnFWFileCCSsearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (sfdImportDati.InitialDirectory == "") sfdImportDati.InitialDirectory = PannelloCharger.Properties.Settings.Default.pathFwSource;

                sfdImportDati.Filter = "CCS Generated File (*.txt)|*.txt|All files (*.*)|*.*";
                sfdImportDati.ShowDialog();
                txtFwFileCCS.Text = sfdImportDati.FileName;
                PannelloCharger.Properties.Settings.Default.pathFwSource = Path.GetDirectoryName(sfdImportDati.FileName);
            }
            catch (Exception Ex)
            {
                Log.Error("btnGeneraCsv_Click: " + Ex.Message);
            }

        }

        private void btnFWFileCCSLoad_Click(object sender, EventArgs e)
        {
            CaricafileCCS();
        }

        private void btnFWFilePubSave_Click(object sender, EventArgs e)
        {
            if (txtFWFileSBFwr.Text != "")
            {
                if (txtFWInFileRev.Text == "")
                {
                    MessageBox.Show("Inserire il numero di release", "Preperazione App", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SalvaFileSBF();
            }
        }

        private void btnFWFileSBFsearch_Click(object sender, EventArgs e)
        {
            sfdExportDati.Filter = "SBF SPY-BATT Firmware File (*.sbf)|*.sbf|All files (*.*)|*.*";
            sfdExportDati.ShowDialog();
            txtFWFileSBFwr.Text = sfdExportDati.FileName;
            btnFWLanciaTrasmissione.Enabled = false;

        }

        private void btnFWFileSBFReadSearch_Click(object sender, EventArgs e)
        {
            sfdImportDati.Filter = "SBF SPY-BATT Firmware File (*.sbf)|*.sbf|All files (*.*)|*.*";
            sfdImportDati.ShowDialog();
            txtFWFileSBFrd.Text = sfdImportDati.FileName;

            bool _preview = CaricafileSBF();
        }

        private void btnFWFileSBFLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFWFileSBFrd.Text == "") return;

                btnFWLanciaTrasmissione.Enabled = false;
                CaricafileSBF();


                PreparaTrasmissioneFW();
                bool _esitocella = false;

                _esitocella = ((byte)FirmwareManager.MascheraStato.Blocco1HW & _sb.StatoFirmware.Stato) == (byte)FirmwareManager.MascheraStato.Blocco1HW;
                if (_esitocella == true)
                {
                    txtFWSBFArea.Text = "1";
                }
                else
                {
                    txtFWSBFArea.Text = "2";
                }
            }
            catch (Exception Ex)
            {
                Log.Error("btnFWFileSBFLoad_Click: " + Ex.Message);
            }
        }

        private void btnFwCaricaStato_Click(object sender, EventArgs e)
        {
            string _tempId = _sb.Id;
            _sb.VerificaPresenza();
            CaricaStatoFirmware(ref _tempId, _logiche, _sb.apparatoPresente);
            CaricaStatoAreaFw(1, _sb.StatoFirmware.Stato);
            CaricaStatoAreaFw(2, _sb.StatoFirmware.Stato);
        }

        private void btnFWPreparaTrasmissione_Click(object sender, EventArgs e)
        {
            PreparaTrasmissioneFW();
        }

        private void txtFWTxFileLenN2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnFWLanciaTrasmissione_Click(object sender, EventArgs e)
        {
            try
            {
                bool _esito;
                this.Cursor = Cursors.WaitCursor;
                AggiornaFirmware();
                //_sb.VerificaPresenza();
                _esito = reconnectSpyBat();
                this.Cursor = Cursors.Default;
            }
            catch
            {
                this.Cursor = Cursors.Default;

            }
        }

        /// <summary>
        /// Handles the Click event of the btnMemCFExec control.
        /// Cancella  il numero di blocchi da 4K definito in txtMemCFBlocchi a patire dall'indirizzo txtMemCFStartAdd
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnMemCFExec_Click(object sender, EventArgs e)
        {
            try
            {
                uint _StartAddr;
                int _bloccoCorrente;
                ushort _NumBlocchi;
                bool _esito;

                if (chkMemHex.Checked)
                {
                    if (uint.TryParse(txtMemCFStartAdd.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true) return;
                }
                else
                {

                    if (uint.TryParse(txtMemCFStartAdd.Text, out _StartAddr) != true) return;
                }


                _NumBlocchi = 0;
                if (ushort.TryParse(txtMemCFBlocchi.Text, out _NumBlocchi) != true) return;


                if (_NumBlocchi > 0)
                {

                    for (int _cicloBlocchi = 0; _cicloBlocchi < _NumBlocchi; _cicloBlocchi++)
                    {
                        _bloccoCorrente = _cicloBlocchi + 1;
                        _esito = _sb.CancellaBlocco4K(_StartAddr);
                        if (!_esito)
                        {
                            MessageBox.Show("Cancellazione del blocco " + _bloccoCorrente.ToString() + " non riuscita", "Cancellazione dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            _StartAddr += 0x1000;
                            txtMemCFStartAdd.Text = _StartAddr.ToString("X6");
                            txtMemCFBlocchi.Text = _bloccoCorrente.ToString();
                            Application.DoEvents();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Inserire un numero di blocchi valido", "Esportazione dati Corrente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }




            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }




        }

        /// <summary>
        /// Handles the Click event of the btnFwSwitchArea control.
        /// Commuta l'app attiva tra BL, Area A1 e Area A2 (definita tramite option button)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void btnFwSwitchArea_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbtFwArea1.Checked)
                {
                    SwitchAreaFw(_sb.Id, _sb.apparatoPresente, 1);
                    return;
                }
                else
                {
                    if (rbtFwArea2.Checked)
                    {
                        SwitchAreaFw(_sb.Id, _sb.apparatoPresente, 2);
                        return;
                    }
                    else
                    {
                        if (rbtFwBootLdr.Checked)
                        {
                            SwitchAreaBl(_sb.Id, _sb.apparatoPresente);
                            return;
                        }

                    }

                }

            }
            catch
            {

            }
        }

        private void btnClonaCaricaOrigine_Click(object sender, EventArgs e)
        {
            bool _esito;
            if (txtClonaIdOrigine.Text != "")
            {
                _esito = CaricaSbOrigine(txtClonaIdOrigine.Text, _logiche.dbDati.connessione, false);
                if (_esito)
                {
                    txtClonaFwOrigine.Text = _sbTemp.sbData.SwVersion;
                }


            }
        }


        private void btnClonaGeneraRecordTestata_Click(object sender, EventArgs e)
        {
            bool _esito;
            txtClonaRecordTestata.Text = "";
            if (_sbTemp.sbData.valido)
            {
                byte[] _tempHexData;
                _tempHexData = _sbTemp.sbData.DataArray;
                txtClonaRecordTestata.Text = FunzioniComuni.HexdumpArray(_tempHexData);
                Log.Warn(" ---------------------------------- Testata -----------------------------------");
                Log.Warn(FunzioniMR.hexdumpArray(_tempHexData, false));

            }
            else
                txtClonaRecordTestata.Text = "Dati non validi";


        }

        private void tbpClonaScheda_Click(object sender, EventArgs e)
        {

        }

        private void btnClonaScriviRecordTestata_Click(object sender, EventArgs e)
        {
            bool _esito;
            if (optCloneDaDB.Checked)
            {
                if (_sbTemp == null)
                {
                    txtClonaStatoAttuale.Text = "Dati Origine non caricati";
                    return;
                }



                if (_sbTemp.sbData.valido)
                {

                    txtClonaStatoAttuale.Text = "Inizio Clonazione";

                    _esito = InizializzaClonazioneScheda();

                    if (_esito)
                    {
                        _esito = EseguiClonazioneScheda();

                        if (_esito)
                            txtClonaStatoAttuale.Text = "Testata Aggiornata";
                        else
                            txtClonaStatoAttuale.Text = "Scrittura fallita";
                    }
                }
                else
                    txtClonaStatoAttuale.Text = "Dati non validi";
            }
            else
            {
                if (!_immagineValida)
                {
                    txtClonaStatoAttuale.Text = "Dati Origine non validi";
                    return;
                }
                else
                {
                    txtClonaStatoAttuale.Text = "Inizio Clonazione";

                    _esito = InizializzaClonazioneScheda();

                    if (_esito)
                    {
                        _esito = EseguiClonazioneSchedaDaDump();

                        if (_esito)
                            txtClonaStatoAttuale.Text = "Scheda Aggiornata";
                        else
                            txtClonaStatoAttuale.Text = "Scrittura fallita";
                    }
                }
            }
        }

        private void btnResetScheda_Click(object sender, EventArgs e)
        {
            try
            {
                bool _esito;
                DialogResult risposta = MessageBox.Show("Vuoi veramente riavviare la scheda ?\nATTENZIONE:",
                "Reset Scheda",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

                if (risposta == System.Windows.Forms.DialogResult.Yes)
                {
                    _esito = _sb.ResetScheda(chkCliResetContatori.Checked);
                    //if (_esito) MessageBox.Show("Memoria Azzerata", "Cancellazione Memoria", MessageBoxButtons.OK);

                }
            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemClear_Click: " + Ex.Message);
            }
        }

        private void btnClonaPreparaMatrici_Click(object sender, EventArgs e)
        {
            bool _esito = PreparaClonazioneScheda();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnSalvaCaricabatteria_Click(object sender, EventArgs e)
        {

        }

        private void btnClonaSchedVuota_Click(object sender, EventArgs e)
        {

            _sbTemp = new UnitaSpyBatt(ref _parametri, _logiche.dbDati.connessione, _logiche.currentUser.livello);
        }

        private void btnFwCheckArea_Click(object sender, EventArgs e)
        {

        }

        private void txtStatTempoInCarica_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblStatNumCaricheComp_Click(object sender, EventArgs e)
        {

        }

        private void btnFSerCancellaMemoria_Click(object sender, EventArgs e)
        {
            CancellaMemoria(chkFSerMantieniCliente.Checked);
        }

        private void lblCelleP1_Click(object sender, EventArgs e)
        {

        }

        private void txtProgcCelleV1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label139_Click(object sender, EventArgs e)
        {

        }

        private void btnGeneraCodice_Click(object sender, EventArgs e)
        {
            txtCodiceSblocco.Text = CodiceSblocco();
        }

        private string CodiceSblocco()
        {
            try
            {
                string _codice = "";
                SerialMessage.Crc16Ccitt codCrc = new SerialMessage.Crc16Ccitt(SerialMessage.InitialCrcValue.NonZero1);

                //Step 1 Genero l'esadecimale dell'ID
                int _hexId = 0;
                byte[] array = Encoding.ASCII.GetBytes(_sb.Id);

                // Loop through contents of the array.
                int _step = 0;
                foreach (byte element in array)
                {
                    _step += 9;
                    _hexId += (element * _step);
                }
                _codice = _hexId.ToString("x6");

                //poi aggiungo il numeo ciclilunghi
                _codice += _sb.sbData.LongMem.ToString("x6");
                _codice = _codice.ToUpper();
                byte[] PrimaCodifica = Encoding.ASCII.GetBytes(_codice);

                //Quindi metto il CRC
                ushort _crc = codCrc.ComputeChecksum(PrimaCodifica);
                _codice += _crc.ToString("x4");
                _codice = _codice.ToUpper();

                // Ultimo passo permuto i caratteri - 6+6+4

                string _permutato = "";

                for (int _ciclo = 0; _ciclo < 8; _ciclo++)
                {
                    // Prima i pari
                    _permutato += _codice[_ciclo * 2];

                }

                for (int _ciclo = 0; _ciclo < 8; _ciclo++)
                {
                    // poi i dispari alla rovescia
                    _permutato += _codice[15 - (_ciclo * 2)];

                }

                //txtCodiceBase.Text = _codice;
                return _permutato;
            }
            catch (Exception Ex)
            {
                Log.Error("CodiceSblocco: " + Ex.Message);
                return "";
            }
        }

        private void lblCliCliente_Click(object sender, EventArgs e)
        {

        }

        private void label48_Click(object sender, EventArgs e)
        {

        }

        private void frmSpyBat_Activated(object sender, EventArgs e)
        {
            try
            {
                frmMain _parent = (frmMain)this.MdiParent;
                _parent.Toolbar.reset();

                _parent.Toolbar.StampaAttiva = true;
                _parent.Toolbar.StampaVisibile = true;

                _parent.Toolbar.ExportAttivo = true;
                _parent.Toolbar.ExportVisibile = true;

                _parent.Toolbar.RefreshAttivo = false;
                _parent.Toolbar.RefreshVisibile = true;
                _parent.AggiornaToolbar(_parent.Toolbar);

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat_Activated: " + Ex.Message);
            }
        }

        private void frmSpyBat_Deactivate(object sender, EventArgs e)
        {
            try
            {
                frmMain _parent = (frmMain)this.MdiParent;
                _parent.Toolbar.reset();
                _parent.AggiornaToolbar(_parent.Toolbar);

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat_Activated: " + Ex.Message);
            }
        }

        public void export()
        {
            try
            {
                if (IdCorrente != "")
                {
                    ApriExportSpyBatt(IdCorrente);
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat_Activated: " + Ex.Message);
            }
        }



        private void ApriExportSpyBatt(string IdApparato)
        {
            try
            {

                frmSbExport sbExport = new frmSbExport(ref _parametri, true, IdApparato, _logiche, false, false);
                sbExport.MdiParent = this.MdiParent;
                sbExport.StartPosition = FormStartPosition.CenterParent;
                sbExport.Setmode(elementiComuni.modoDati.Output);
                sbExport.Show();

            }
            catch (Exception Ex)
            {
                Log.Error("ApriExportSpyBatt: " + Ex.Message);
            }

        }

        private void btnCalCollegaAlim_Click(object sender, EventArgs e)
        {
            try
            {
                VerificaAlimentatore(true);
            }

            catch (Exception Ex)
            {
                Log.Error("btnCalCollegaAlim_Click: " + Ex.Message);
            }

        }

        private void VerificaAlimentatore(bool Popup = true)
        {
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmAlimentatore))
                    {
                        Lambda = (frmAlimentatore)form;
                        if (Popup)
                        {
                            form.Activate();
                        }
                        lblCalStatoAlim.Text = "COLLEGATO";
                        lblCalConnessione.Text = "COLLEGATO";
                        btnCalStartSeq.Enabled = true;
                        return;
                    }
                }

                frmAlimentatore Alimentatore = new frmAlimentatore();
                Alimentatore.MdiParent = this.MdiParent;
                Alimentatore.StartPosition = FormStartPosition.CenterParent;
                if (Alimentatore != null)
                    Alimentatore.Show();

                Lambda = Alimentatore;

            }

            catch (Exception Ex)
            {
                Log.Error("btnCalCollegaAlim_Click: " + Ex.Message);
            }

        }


        /// <summary>
        /// Handles the Click event of the btnCalStartSeq control.
        /// Verifica la versione del firmware attivo e, se valida, lancia la sequenza di calibrazione
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void btnCalStartSeq_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;



                sbTestataCalibrazione _sequenzaCorrente = new sbTestataCalibrazione(_logiche.dbDati.connessione);

                if (tabCalibrazione.Height > 300)
                {
                    pnlCalVerifica.Height = tabCalibrazione.Height - 240;
                }
                if (tbpCalibrazioni.Width > 600)
                {
                    pnlCalGrafico.Width = tbpCalibrazioni.Width - 540;
                }


                int _AmaxCal = 250;
                int _AmaxVer = 300;
                int _spire = 2;
                int _secAttesa = 5;
                int _maxErrA = 5;
                int _secPasso = 2;
                float _coeffPasso = 2;


                int.TryParse(txtCalAcal.Text, out _AmaxCal);
                int.TryParse(txtCalAver.Text, out _AmaxVer);
                int.TryParse(txtCalNumSpire.Text, out _spire);
                int.TryParse(txtCalSecondiAttesa.Text, out _secAttesa);
                int.TryParse(txtCalMaxAerr.Text, out _maxErrA);
                int.TryParse(txtCalSecondiPasso.Text, out _secPasso);
                float.TryParse(txtCalCoeffPasso.Text, out _coeffPasso);

                if (_secAttesa < 1) _secAttesa = 1;
                if (_secAttesa > 60) _secAttesa = 60;


                // Prima di iniziare verifico che il FW sia aggiornato
                if (_sb.sbData.SwVersion != txtCalFWRichiesto.Text)
                {
                    DialogResult _risposta = MessageBox.Show("Versione firmware non corretta; \nAggiornare la scheda prima di eseguire la calibrazione.", "Verifica Firmware", MessageBoxButtons.OKCancel);
                    if (_risposta == DialogResult.OK)
                    {
                        tabCaricaBatterie.SelectedTab = tbpFirmware;
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }


                txtCalAcal.Text = _AmaxCal.ToString();
                txtCalAver.Text = _AmaxVer.ToString();
                txtCalNumSpire.Text = _spire.ToString();
                txtCalSecondiAttesa.Text = _secAttesa.ToString();

                bool _esitoSeq = LanciaSequenzaCalibrazione(_sequenzaCorrente, chkCalRegistraSequenza.Checked, _AmaxCal, _AmaxVer, _spire, _secAttesa, _maxErrA, _secPasso, _coeffPasso);

                if (chkCalRegistraSequenza.Checked)
                {
                    _sequenzaCorrente.LettureCorrente = ValoriTestCorrente;
                    _sequenzaCorrente.salvaDati();
                }

                if (!_esitoSeq)
                {
                    MessageBox.Show("Calibrazione e Verifica NON SUPERATA", "Verifica Calibrazione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Log.Error("btnCalStartSeq_Click: " + Ex.Message);
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCalRicaricaSequenza control.
        /// Ricarica i dati della curva di calibrazione selezionata
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void btnCalRicaricaSequenza_Click(object sender, EventArgs e)
        {
            // pnlCalVerifica.Width = tabCalibrazione.Width - 300;
            // GraficoCorrentiCalComplOxy();
            // TestSequenzaVerificaCalibrazione();
            lbxCalListaStep.Items.Clear();
            if (cmbCalListaEsecuzioni.SelectedItem != null)
            {
                _sbTestataCalibrazione _SelItem = (_sbTestataCalibrazione)cmbCalListaEsecuzioni.SelectedItem;

                lbxCalListaStep.Items.Add("Ciclo Calibrazione " + _SelItem.IdEsecuzione.ToString());
                lbxCalListaStep.Items.Add("del  " + _SelItem.DataEsecuzione.ToString());
                lbxCalListaStep.Items.Add("F.W. " + _SelItem.FirmwareAttivo.ToString());
                sbTestataCalibrazione _sequenzaCorrente = new sbTestataCalibrazione(_logiche.dbDati.connessione);
                _sequenzaCorrente.caricaDati(_SelItem.IdLocale);
                ValoriTestCorrente = _sequenzaCorrente.LettureCorrente;
                GraficoCorrentiCalComplOxy();


            }
            else
            {
                lbxCalListaStep.Items.Add("Nessuna calibrazione selezionata");
            }


            //GraficoCorrentiCalComplOxy();
        }

        private void btnCalRefreshLista_Click(object sender, EventArgs e)
        {
            bool _esito;

            _esito = CaricaTestateCalibrazioni(_sb.Id, _logiche.dbDati.connessione);

            if (_esito)
            {
                cmbCalListaEsecuzioni.DataSource = _ListaCalibrazioni;
                cmbCalListaEsecuzioni.DisplayMember = "IdEsecuzione";
                cmbCalListaEsecuzioni.ValueMember = "IdLocale";
            }

        }

        private void cmbCalTipoGrafico_SelectedValueChanged(object sender, EventArgs e)
        {
            GraficoCorrentiCalComplOxy();
        }

        private void btnGraficiTest_Click(object sender, EventArgs e)
        {
            int _primaScheda = 1;
            DatiEstrazione _tempDati;

            try
            {
                ImpostaCheckStatistiche(false);

                chkStatGraficoDoD.Checked = true;
                chkStatGraficoDTCC.Checked = true;
                chkStatGraficoAssEl.Checked = true;
                chkStatGraficoFC.Checked = true;




                RicalcolaStatisticheDEMO();
            }
            catch
            {

            }
        }

        private void ImpostaCheckStatistiche(bool Stato)
        {
            try
            {
                chkStatGraficoTemporale.Checked = Stato;
                chkStatGraficoDoD.Checked = Stato;
                chkStatGraficoDurCC.Checked = Stato;
                chkStatGraficoDurCP.Checked = Stato;
                chkStatGraficoTmaxCP.Checked = Stato;
                chkStatGraficoDTCC.Checked = Stato;
                chkStatGraficoAssEl.Checked = Stato;
                chkStatGraficoFC.Checked = Stato;

            }
            catch
            {

            }
        }

        public void RicalcolaStatisticheDEMO()
        {
            try
            {
                DatiEstrazione TempStat = new DatiEstrazione();

                _stat = new StatMemLungaSB();
                _stat.SoglieAnalisi = _sb.SoglieAnalisi;
                _stat.caricaSoglie();
                _stat.CicliAttesi = _sb.sbCliente.CicliAttesi;
                if (_sb.CicliMemoriaLunga.Count > 0)
                {
                    _stat.CicliMemoriaLunga = _sb.CicliMemoriaLunga;
                    _stat.aggregaValori(optStatPeriodoSel.Checked, dtpStatInizio.Value, dtpStatFine.Value);
                }

                InizializzaVistaSoglie();

                MostraSintesiStatistiche();
                InizializzaCockpitStatDEMO();
                InizializzaSchedaConfronti();

                CaricaSchedeGraficoDEMO(_stat);

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat.RicalcolaStatistiche: " + Ex.Message);

            }
        }

        private void CaricaSchedeGraficoDEMO(StatMemLungaSB DatiStat)
        {
            int _primaScheda = 1;
            DatiEstrazione _tempDati;

            try
            {
                //prima elimino le esistenti
                foreach (TabPage _pagina in tbcStatistiche.TabPages)
                {
                    if (_pagina.Tag == "GRAFICO")
                        tbcStatistiche.TabPages.Remove(_pagina);
                }


                /***************************************************************************************************************************/
                /*   GRAFICO C.F..                                                                                                         */
                /***************************************************************************************************************************/

                if (chkStatGraficoFC.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrCfChiave);
                    _grafico.ToolTipText = chkStatGraficoFC.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoChargeFactorDEMO(StringheStatistica.GrCfTitolo);
                    GraficoLivelliOxy(StringheStatistica.GrCfTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

                /***************************************************************************************************************************/
                /*   GRAFICO Assenza Elettrolita                                                                      */
                /***************************************************************************************************************************/

                if (chkStatGraficoAssEl.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrAssenzaElChiave, new Size(300, 200));
                    _grafico.ToolTipText = chkStatGraficoAssEl.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoMancanzaElDEMO(StringheStatistica.GrAssenzaElTitolo);
                    GraficoTorta(StringheStatistica.GrAssenzaElTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

                /***************************************************************************************************************************/
                /*   GRAFICO Differenza Temperature in Carica Completa                                                                     */
                /***************************************************************************************************************************/

                if (chkStatGraficoDTCC.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrDeltaTCCChiave);
                    _grafico.ToolTipText = chkStatGraficoTminS.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoTempertureCicliDEMO(SerialMessage.TipoCiclo.Carica, 2, 2, 0, 20, 1, StringheStatistica.GrDeltaTCCTitolo);
                    _grafico.DatiGrafico.StepSoglia = (DatiStat.SogliaDiffTempScarica) / 2;
                    _grafico.DatiGrafico.VersoSoglia = DatiEstrazione.Direzione.Ascendente;
                    GraficoTemperaturaCiclo(StringheStatistica.GrDeltaTCCTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

                /***************************************************************************************************************************/
                /*   GRAFICO D.O.D.                                                                                                        */
                /***************************************************************************************************************************/

                if (chkStatGraficoDoD.Checked == true)
                {
                    oxyTabPage _grafico = new oxyTabPage(StringheStatistica.GrDODChiave);
                    _grafico.ToolTipText = chkStatGraficoDoD.Text;
                    _grafico.Tag = "GRAFICO";
                    _grafico.DatiGrafico = new DatiEstrazione();
                    _grafico.DatiGrafico = DatiStat.CalcolaArrayGraficoDeepChgDEMO(SerialMessage.TipoCiclo.Scarica, StringheStatistica.GrDODTitolo);

                    GraficoLivelliOxy(StringheStatistica.GrDODTitolo, _grafico.DatiGrafico, ref _grafico.GraficoBase);

                    tbcStatistiche.TabPages.Insert(_primaScheda, _grafico);

                }

            }
            catch
            {

            }
        }

        public void InizializzaCockpitStatDEMO()
        {
            try
            {

                // Prima riga
                // 1.1  -  S.o.H.

                buiStatCockpit.Frame.Clear();

                Ic11 = new IndicatoreCruscotto();
                Ic11.ValueMask = "0";
                Ic11.MinVal = 0;
                Ic11.Lim1 = 50;
                Ic11.Lim2 = 80;
                Ic11.MaxVal = 100;
                Ic11.Verso = IndicatoreCruscotto.VersoValori.Discendente;
                Ic11.InizializzaIndicatore(this.buiStatCockpit, 30, 20, 280, StringheStatistica.GougeSOHr1 + "\n" + StringheStatistica.GougeSOHr2);
                Ic11.ImpostaValore((float)91);



                //  1.2 - D.o.D.

                Ic12 = new IndicatoreCruscotto();
                Ic12.ValueMask = "0";
                Ic12.MostraValore = true;
                Ic12.MinVal = 0;
                Ic12.Lim1 = 50;
                Ic12.Lim2 = 80;
                Ic12.MaxVal = 100;
                Ic12.Verso = IndicatoreCruscotto.VersoValori.Ascendente;
                Ic12.InizializzaIndicatore(this.buiStatCockpit, 330, 20, 280, StringheStatistica.GougeDODr1 + "\n" + StringheStatistica.GougeDODr2);
                Ic12.ImpostaValore((float)52);



                //  1.3  Sovrascariche

                Ic13 = new IndicatoreCruscotto();
                Ic13.ValueMask = "0";
                Ic13.MostraValore = true;
                Ic13.MinVal = 0;
                Ic13.Lim1 = 5;
                Ic13.Lim2 = 10;
                Ic13.MaxVal = 100;
                Ic13.Verso = IndicatoreCruscotto.VersoValori.Ascendente;
                Ic13.InizializzaIndicatore(this.buiStatCockpit, 630, 20, 280, StringheStatistica.GougeSovrar1 + "\n" + StringheStatistica.GougeSovrar2);
                Ic13.ImpostaValore((float)20);


                //  1.4  Cariche Incomplete

                Ic14 = new IndicatoreCruscotto();
                Ic14.ValueMask = "0";
                Ic14.MostraValore = true;
                Ic14.MinVal = 0;
                Ic14.Lim1 = 10;
                Ic14.Lim2 = 20;
                Ic14.MaxVal = 100;
                Ic14.Verso = IndicatoreCruscotto.VersoValori.Ascendente;
                Ic14.InizializzaIndicatore(this.buiStatCockpit, 930, 20, 280, StringheStatistica.GougeCIncr1 + "\n" + StringheStatistica.GougeCIncr2);
                Ic14.ImpostaValore((float)16);






                //  2.1  Sbilanciamento

                Ic21 = new IndicatoreCruscotto();
                Ic21.ValueMask = "0";
                Ic21.MinVal = 0;
                Ic21.Lim1 = 10;
                Ic21.Lim2 = 20;
                Ic21.MaxVal = 100;
                Ic21.LabelOffset = 40;
                Ic21.InizializzaIndicatore(this.buiStatCockpit, 30, 300, 280, StringheStatistica.GougeSbilr1 + "\n" + StringheStatistica.GougeSbilr2);
                Ic21.ImpostaValore((float)8);


                //  2.2  Overtemp
                Ic22 = new IndicatoreCruscotto();
                Ic22.ValueMask = "0";
                Ic22.MinVal = 0;
                Ic22.Lim1 = 5;
                Ic22.Lim2 = 10;
                Ic22.MaxVal = 100;
                Ic22.LabelOffset = 80;
                Ic22.MostraValore = true;
                Ic22.Verso = IndicatoreCruscotto.VersoValori.Ascendente;
                Ic22.InizializzaIndicatore(this.buiStatCockpit, 330, 300, 280, StringheStatistica.GougeOverTr1 + "\n" + StringheStatistica.GougeOverTr2);
                Ic22.ImpostaValore((float)9);




                //  2.3  Assenza Elettrolita

                Ic23 = new IndicatoreCruscotto();
                Ic23.ValueMask = "0";
                Ic23.MinVal = 0;
                Ic23.Lim1 = 5;
                Ic23.Lim2 = 10;
                Ic23.MaxVal = 100;
                Ic23.LabelOffset = 50;

                Ic23.InizializzaIndicatore(this.buiStatCockpit, 630, 300, 280, StringheStatistica.GougeAssElr1 + "\n" + StringheStatistica.GougeAssElr2);
                Ic23.ImpostaValore((float)6);



                //  2.4  Pause Critiche

                Ic24 = new IndicatoreCruscotto();
                Ic24.ValueMask = "0";
                Ic24.MinVal = 0;
                Ic24.Lim1 = 5;
                Ic24.Lim2 = 8;
                Ic24.MaxVal = 10;
                Ic24.LabelOffset = 80;
                Ic24.MostraValore = true;
                Ic24.Verso = IndicatoreCruscotto.VersoValori.Ascendente;
                Ic24.InizializzaIndicatore(this.buiStatCockpit, 930, 300, 280, StringheStatistica.GougePauser1 + "\n" + StringheStatistica.GougePauser2);
                Ic24.ImpostaValore((float)2);

            }

            catch (Exception Ex)
            {
                Log.Error("Inizializza Cockpit: " + Ex.Message);
            }

        }

        private void btnStratTest01_Click(object sender, EventArgs e)
        {
            LanciaComandoTestStrategia(0x71);
        }

        private void btnStratTest02_Click(object sender, EventArgs e)
        {

        }

        private void btnStratTestErr_Click(object sender, EventArgs e)
        {
            LanciaComandoTestStrategia(0x7F);
        }

        private void btnStratQuery_Click(object sender, EventArgs e)
        {
            LanciaComandoStrategiaInfo(0xA0);
        }

        private void btnStratSetCharge_Click(object sender, EventArgs e)
        {
            LanciaComandoStrategiaUpdCnt(0x51);
        }


        private void btnStratSetDischarge_Click(object sender, EventArgs e)
        {
            LanciaComandoStrategiaChechCnt(0x50);
        }

        private void btnStratCallIS_Click(object sender, EventArgs e)
        {
            LanciaComandoStrategia(0x02);
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnStratCallAv_Click(object sender, EventArgs e)
        {
            LanciaComandoStrategiaAvanzamentoFase();
        }

        private void cmbStratIsSelStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                StepCarica PassoSelezionato = (StepCarica)cmbStratIsSelStep.SelectedItem;
                MostraPassoCorrente(PassoSelezionato);


            }

            catch
            {
                CancellaPassoCorrente();
            }

        }

        private void btnStratCallSIS_Click(object sender, EventArgs e)
        {
            LanciaComandoStrategia(0x05);
        }

        private void btnStratReadVar_Click(object sender, EventArgs e)
        {
            LanciaComandoStrategiaLetturaVariabili();
        }

        private void cmbModoPianificazione_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool _esito;
            try
            {
                if (_onUpload)
                    return;

                Pianificazione _tempP = (Pianificazione)cmbModoPianificazione.SelectedItem;

                if (_sb.sbCliente.ModoPianificazione != (byte)_tempP.CodiceTP)
                {
                    DialogResult risposta = MessageBox.Show("Vuoi cambiare la modalità di pianificazione delle cariche ? " + "\n" + "L'operazione cancellerà la pianificazione corrente",
                    "Modo Pianificazione",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                    if (risposta == System.Windows.Forms.DialogResult.Yes)
                    {
                        _esito = _sb.sbCliente.PianificazioneCorrente.ImpostaModello(_tempP.CodiceTP);
                        _sb.sbCliente.ModoPianificazione = (byte)_tempP.CodiceTP;
                        _esito = MostraPianificazione();
                        DatiSalvati = false;
                    }

                }


            }

            catch (Exception Ex)
            {
                Log.Error("cmbModoPianificazione_SelectedIndexChanged: " + Ex.Message);
            }
        }

        private void btnPianRefresh_Click(object sender, EventArgs e)
        {
            MostraPianificazione();
        }

        private bool MostraPianificazione(bool InizializzaControllo = true)
        {
            bool _esito = false;
            try
            {
                //_sb.sbCliente.MappaTurni = null;

                switch (_sb.sbCliente.ModoPianificazione)
                {
                    case (byte)ParametriSetupPro.TipoPianificazione.NonDefinita:
                        tlpGrigliaTurni.Visible = false;
                        break;
                    case (byte)ParametriSetupPro.TipoPianificazione.Tempo:
                        MostraGrigliaTempo(InizializzaControllo);
                        break;
                    case (byte)ParametriSetupPro.TipoPianificazione.Turni:
                        MostraGrigliaTurni(InizializzaControllo);
                        break;
                    default:
                        break;
                }

                return _esito;
            }


            catch (Exception Ex)
            {
                Log.Error("MostraPianificazione: " + Ex.Message);
                return _esito;
            }

        }

        /// <summary>
        /// Inizializza la griglia di pianificazione a tempo.
        /// </summary>
        /// <param name="InizializzaControllo">if set to <c>true</c> [inizializza controllo].</param>
        /// <returns></returns>
        private bool MostraGrigliaTempo(bool InizializzaControllo = true)
        {
            bool _esito = false;
            try
            {
                tlpGrigliaTurni.Visible = true;
                if (InizializzaControllo)
                {
                    _sb.sbCliente.MappaTurni = null;
                }


                //  tlpGrigliaTurni.ColumnCount = 4;        
                //  tlpGrigliaTurni.RowCount = 9;
                PanelTestataColonnaTurno _testata;
                PanelTestataColonnaTurno _testata2;
                PanelGiornoTurno _giorno;
                tlpGrigliaTurni.Controls.Clear();

                _testata = new PanelTestataColonnaTurno( PannelloCharger.StringheMessaggio.strTitoloPianificazione);   //"Carica Pianificata");
                tlpGrigliaTurni.Controls.Add(_testata, 1, 0);
                _testata2 = new PanelTestataColonnaTurno(PannelloCharger.StringheMessaggio.strSottoTitoloPianificazione, new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
                tlpGrigliaTurni.Controls.Add(_testata2, 1, 1);


                // Se il cliente è caricato, uso la griglia definita
                if (_sb.sbCliente.MappaTurni == null)
                    _sb.sbCliente.MappaTurni = new byte[1];

                if (_sb.sbCliente.MappaTurni.Length != 84)
                {
                    _sb.sbCliente.MappaTurni = new byte[84] { 0x02, 0x94, 0x65, 0x00, 0, 0, 0, 0,0,0,0,0,
                                                              0x02, 0x94, 0x65, 0x00, 0, 0, 0, 0,0,0,0,0,
                                                              0x02, 0x94, 0x73, 0x00, 0, 0, 0, 0,0,0,0,0,
                                                              0x02, 0x94, 0x65, 0x00, 0, 0, 0, 0,0,0,0,0,
                                                              0x02, 0x94, 0x65, 0x00, 0, 0, 0, 0,0,0,0,0,
                                                              0x02, 0x94, 0x73, 0x00, 0, 0, 0, 0,0,0,0,0,
                                                              0x02, 0x94, 0x73, 0x00, 0, 0, 0, 0,0,0,0,0,  };
                }

                byte[] _tempData = _sb.sbCliente.MappaTurni;
                byte[] _datiTurno = new byte[6];

                for (int _se = 0; _se < 7; _se++)
                {
                    _giorno = new PanelGiornoTurno(DataOraMR.SiglaGiorno(_se + 1));
                    tlpGrigliaTurni.Controls.Add(_giorno, 0, _se + 2);
                    ctrlPannelloTempo P1 = new ctrlPannelloTempo();
                    ModelloTurno _mT = new ModelloTurno();
                    Array.Copy(_tempData, _se * 12, _datiTurno, 0, 6);
                    _mT.fromData(_datiTurno);
                    P1.Turno = _mT;
                    P1.Giorno = (byte)_se;


                    tlpGrigliaTurni.Controls.Add(P1, 1, _se + 2);

                    P1.DatiCambiati += DatiDaSalvare;

                }






                _esito = true;
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("MostraGrigliaTempo: " + Ex.Message);
                return _esito;
            }

        }




        private bool SalvaGrigliaTempo(bool InizializzaMatrice = true)
        {
            bool _esito = false;
            try
            {
                bool _arrayInit = InizializzaMatrice;

                if (_sb.sbCliente.MappaTurni == null)
                {
                    _arrayInit = true;
                }
                else
                {
                    if (_sb.sbCliente.MappaTurni.Length != 84)
                        _arrayInit = true;
                }


                if (_arrayInit)
                {
                    _sb.sbCliente.MappaTurni = new byte[84];
                    for (int _arr = 0; _arr < 84; _arr++)
                    {
                        _sb.sbCliente.MappaTurni[_arr] = 0x00;

                    }

                }
                byte Giornisalvati = 0;
                foreach (Control _ctrl in tlpGrigliaTurni.Controls)
                {
                    if (_ctrl.GetType() == typeof(ctrlPannelloTempo))  // new grid, nuovo controllo con gestione equal
                    {
                        byte[] _datiFase = new byte[6] { 0, 0, 0, 0, 0, 0 };
                        ctrlPannelloTempo P1 = (ctrlPannelloTempo)_ctrl;
                        _datiFase = P1.Turno.toData();
                        Array.Copy(_datiFase, 0, _sb.sbCliente.MappaTurni, P1.Giorno * 12, 6);
                        Giornisalvati += 1;
                    }

                    if (_ctrl.GetType() == typeof(PannelloTempo))  // old grid  
                    {
                        byte[] _datiFase = new byte[4] { 0, 0, 0, 0 };
                        PannelloTempo P1 = (PannelloTempo)_ctrl;
                        _datiFase = P1.Turno.toData();
                        Array.Copy(_datiFase, 0, _sb.sbCliente.MappaTurni, P1.Giorno * 12, 4);
                        Giornisalvati += 1;
                    }

                }
                if (Giornisalvati == 7)
                    _esito = true;

                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("SalvaGrigliaTempo: " + Ex.Message);
                return _esito;
            }

        }





        private void inizializzaGrigliaTurni()
        {
            try
            {
                OraTurnoMR _OraTempI;
                OraTurnoMR _OraTempF;


                for (int giorno = 1; giorno < 8; giorno++)
                {

                    PannelloTurno P1 = new PannelloTurno();
                    _OraTempI = new OraTurnoMR(5, 45);
                    _OraTempF = new OraTurnoMR(6, 15);
                    P1.InizioCambioTurno = _OraTempI;
                    P1.FineCambioTurno = _OraTempF;
                    P1.BackColor = Color.LightYellow;
                    tlpGrigliaTurni.Controls.Add(P1, 1, giorno + 1);

                    PannelloTurno P2 = new PannelloTurno();
                    _OraTempI = new OraTurnoMR(13, 45);
                    _OraTempF = new OraTurnoMR(14, 15);
                    P2.InizioCambioTurno = _OraTempI;
                    P2.FineCambioTurno = _OraTempF;
                    P2.BackColor = Color.LightYellow;
                    tlpGrigliaTurni.Controls.Add(P2, 2, giorno + 1);

                    PannelloTurno P3 = new PannelloTurno();
                    _OraTempI = new OraTurnoMR(21, 45);
                    _OraTempF = new OraTurnoMR(22, 15);
                    P3.InizioCambioTurno = _OraTempI;
                    P3.FineCambioTurno = _OraTempF;
                    P3.BackColor = Color.LightYellow;
                    tlpGrigliaTurni.Controls.Add(P3, 3, giorno + 1);
                }


                lblTurno1.ForeColor = Color.White;
                lblTurno2.ForeColor = Color.White;
                lblTurno3.ForeColor = Color.White;

            }
            catch
            {

            }
        }



        private bool MostraGrigliaTurni(bool InizializzaControllo = true)
        {
            bool _esito = false;
            try
            {
                if (InizializzaControllo)
                {
                    // tlpGrigliaTurni.RowCount = 9;
                    //  tlpGrigliaTurni.ColumnCount = 4;
                    tlpGrigliaTurni.Controls.Clear();
                }

                _esito = true;
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("MostraGrigliaTempo: " + Ex.Message);
                return _esito;
            }

        }

        private void btnCalScriviGiorno_Click(object sender, EventArgs e)
        {
            bool _esito;
            try
            {
                int _giorno = int.Parse(txtCalGiorno.Text);
                int _mese = int.Parse(txtCalMese.Text);
                int _anno = int.Parse(txtCalAnno.Text);
                int _ore = int.Parse(txtCalOre.Text);
                int _minuti = int.Parse(txtCalMinuti.Text);


                _esito = _sb.ForzaOrologio(_giorno, _mese, _anno, _ore, _minuti);
                if (_esito)
                {

                    _esito = _sb.LeggiOrologio();
                    if (_esito)
                    {
                        txtOraRtc.Text = _sb.OrologioSistema.ore.ToString("00") + ":" + _sb.OrologioSistema.minuti.ToString("00");
                        txtDataRtc.Text = _sb.OrologioSistema.giorno.ToString("00") + "/" + _sb.OrologioSistema.mese.ToString("00") + "/" + _sb.OrologioSistema.anno.ToString("0000");
                    }

                }

            }
            catch
            {
            }
        }

        private void btnSvcLeggiParametriMedie_Click(object sender, EventArgs e)
        {
            try
            {
                _sb.CaricaParametri(_sb.Id, _apparatoPresente);
                MostraParametriGenerali();
            }
            catch (Exception Ex)
            {
                Log.Error("btnSvcLeggiParametriMedie_Click: " + Ex.Message);
            }
        }

        /// <summary>
        /// Mostra l'impostazione del numero letture ADC da mediare nei 100 ms.
        /// </summary>
        /// <returns></returns>
        public bool MostraParametriGenerali()
        {
            try
            {
                TimeSpan _durata;
                txtSvcNumLettureCorr.Text = "";
                txtSvcNumLettureTens.Text = "";
                txtSvcSecDurataPause.Text = "";

                if (_sb.ParametriGenerali != null)
                {
                    txtSvcNumLettureCorr.Text = _sb.ParametriGenerali.LettureCorrente.ToString();
                    txtSvcNumLettureCorr.ForeColor = Color.Black;
                    txtSvcNumLettureTens.Text = _sb.ParametriGenerali.LettureTensione.ToString();
                    txtSvcNumLettureTens.ForeColor = Color.Black;


                    _durata = TimeSpan.FromSeconds(_sb.ParametriGenerali.DurataPausa);
                    txtSvcSecDurataPause.Text = _durata.ToString();
                    txtSvcSecDurataPause.ForeColor = Color.Black;


                    if (_sb.sbData.fwLevel<7)
                    {
                        txtSvcCausaLastReset.Text = "";
                    }
                    else
                    {
                        txtSvcCausaLastReset.Text = _sb.ParametriGenerali.CausaUltimoReset.ToString("X4");
                    }

                }




                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("MostraParametriGenerali: " + Ex.Message);
                return false;
            }
        }

        public bool MostraParametriOC()
        {
            try
            {
                //TimeSpan _durata;
                //txtSvcNumLettureCorr.Text = "";
                //txtSvcNumLettureTens.Text = "";
                //txtSvcSecDurataPause.Text = "";

                cmbFSerBaudrateOC.SelectedValue = _sb.BrOCcorrente;

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("MostraParametriGenerali: " + Ex.Message);
                return false;
            }
        }

        private void txtSvcNumLettureCorr_Leave(object sender, EventArgs e)
        {
            try
            {
                bool _VerificaNumero;
                ushort _nuovoValore;

                _VerificaNumero = ushort.TryParse(txtSvcNumLettureCorr.Text, out _nuovoValore);
                if (!_VerificaNumero)
                    _nuovoValore = _sb.ParametriGenerali.LettureCorrente;

                txtSvcNumLettureCorr.Text = _nuovoValore.ToString();
                if (_nuovoValore != _sb.ParametriGenerali.LettureCorrente)
                    txtSvcNumLettureCorr.ForeColor = Color.Red;
            }
            catch (Exception Ex)
            {
                Log.Error("txtSvcNumLettureCorr_Leave: " + Ex.Message);

            }
        }

        private void txtSvcNumLettureTens_Leave(object sender, EventArgs e)
        {
            try
            {
                bool _VerificaNumero;
                ushort _nuovoValore;

                _VerificaNumero = ushort.TryParse(txtSvcNumLettureTens.Text, out _nuovoValore);
                if (!_VerificaNumero)
                    _nuovoValore = _sb.ParametriGenerali.LettureTensione;

                txtSvcNumLettureTens.Text = _nuovoValore.ToString();
                if (_nuovoValore != _sb.ParametriGenerali.LettureTensione)
                    txtSvcNumLettureTens.ForeColor = Color.Red;
            }
            catch (Exception Ex)
            {
                Log.Error("txtSvcNumLettureCorr_Leave: " + Ex.Message);

            }
        }

        private void btnSvcScriviParametriMedie_Click(object sender, EventArgs e)
        {
            try
            {
                bool _VerificaNumero;
                TimeSpan _tsRead;
                ushort _nuovoValoreTens;
                ushort _nuovoValoreCorr;
                ushort _nuovoValorePausa;
                _VerificaNumero = ushort.TryParse(txtSvcNumLettureCorr.Text, out _nuovoValoreCorr);
                if (!_VerificaNumero)
                    return;
                _VerificaNumero = ushort.TryParse(txtSvcNumLettureTens.Text, out _nuovoValoreTens);
                if (!_VerificaNumero)
                    return;


                _VerificaNumero = TimeSpan.TryParse(txtSvcSecDurataPause.Text, out _tsRead);
                if (!_VerificaNumero)
                {
                    _nuovoValorePausa = _sb.ParametriGenerali.DurataPausa;
                }
                else
                {
                    _nuovoValorePausa = (ushort)_tsRead.TotalSeconds;
                }


                _sb.ScriviParametriLettura(_nuovoValoreCorr, _nuovoValoreTens, _nuovoValorePausa);
                //Thread.Sleep(500);
                _sb.CaricaParametri(_sb.Id, _apparatoPresente);
                MostraParametriGenerali();



            }
            catch (Exception Ex)
            {
                Log.Error("btnSvcScriviParametriMedie_Click: " + Ex.Message);

            }
        }

        private void txtSvcSecDurataPause_Leave(object sender, EventArgs e)
        {
            try
            {
                bool _VerificaNumero;
                ushort _nuovoValore;
                TimeSpan _tsRead;

                _VerificaNumero = TimeSpan.TryParse(txtSvcSecDurataPause.Text, out _tsRead);
                if (!_VerificaNumero)
                {
                    _nuovoValore = _sb.ParametriGenerali.DurataPausa;
                }
                else
                {
                    _nuovoValore = (ushort)_tsRead.TotalSeconds;
                }

                _tsRead = TimeSpan.FromSeconds(_nuovoValore);
                txtSvcSecDurataPause.Text = _tsRead.ToString();
                if (_nuovoValore != _sb.ParametriGenerali.DurataPausa)
                    txtSvcNumLettureTens.ForeColor = Color.Red;

            }
            catch (Exception Ex)
            {
                Log.Error("txtSvcSecDurataPause_Leave: " + Ex.Message);

            }
        }

        private void optCloneDaimg_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (optCloneDaimg.Checked)
                    grbCloneDaImg.Enabled = true;
                else
                    grbCloneDaImg.Enabled = false;

            }
            catch (Exception Ex)
            {
                Log.Error("optCloneDaimg_CheckedChanged: " + Ex.Message);

            }
        }

        private void optCloneDaDB_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (optCloneDaDB.Checked)
                    grbCloneDaDatabase.Enabled = true;
                else
                    grbCloneDaDatabase.Enabled = false;

            }
            catch (Exception Ex)
            {
                Log.Error("optCloneDaDB_CheckedChanged: " + Ex.Message);

            }
        }

        private void btnCloneGetFile_Click(object sender, EventArgs e)
        {
            try
            {
                _immagineValida = false;
                sfdImportDati.Title = "HEXDUMP RECOVERY"; // StringheComuni.ImportaDati;
                sfdImportDati.CheckFileExists = false;
                sfdImportDati.Filter = "SPY-BATT HexDump data (*.sbx)|*.sbx|All files (*.*)|*.*";
                // Propongo come directory iniziale  user\documents\LADELIGHT Manager\SPY-BATT
                string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                _pathTeorico += "\\LADELIGHT Manager\\SPY-BATT";
                if (!Directory.Exists(_pathTeorico))
                {
                    Directory.CreateDirectory(_pathTeorico);
                }
                //sfdImportDati.InitialDirectory = _pathTeorico;
                sfdImportDati.ShowDialog();
                txtCloneFileImg.Text = sfdImportDati.FileName;
                _immagineValida = importaHexdump();

                // TODO: loggare l'opetazione

                btnClonaScriviRecordTestata.Enabled = _immagineValida;


            }
            catch (Exception Ex)
            {
                Log.Error("optCloneDaDB_CheckedChanged: " + Ex.Message);

            }

        }

        private void grbFWPreparaFile_Enter(object sender, EventArgs e)
        {

        }

        private void btnStratSetTypeEvent_Click(object sender, EventArgs e)
        {

            LanciaComandoStrategiaWriteTE();
        }

        private void btnStratReadTypeEvent_Click(object sender, EventArgs e)
        {
            LanciaComandoStrategiaReadTE();
        }

        private void btnPianSalvaCliente_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            chkCliResetContatori.Checked = false;
            salvaCliente(_apparatoPresente);
            this.Cursor = Cursors.Default;
        }


        /// <summary>
        /// Ritorna o imposta (protected) lo stato di dati salvati.
        /// </summary>
        /// <value>
        ///   <c>true</c> se tutti i dati sono salvati; otherwise, <c>false</c>.
        /// </value>
        public bool DatiSalvati
        {
            get
            {
                return _datiSalvati;
            }
            protected set
            {
                _datiSalvati = value;
                if (DatiCambiati != null)
                {
                    DatiCambiati(this, new DatiCambiatiEventArgs(_datiSalvati));
                }

            }

        }


        public bool SalvaDati()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                salvaCliente(_apparatoPresente);
                DatiSalvati = true;
                this.Cursor = Cursors.Default;
                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("SalvaDati: " + Ex.Message);
                return false;

            }
        }

        private void txtCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_sb.sbCliente.Client != txtCliente.Text)
                {
                    _sb.sbCliente.Client = txtCliente.Text;
                    DatiSalvati = false;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("txtCliente_Leave: " + Ex.Message);

            }
        }

        private void txtMarcaBat_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_sb.sbCliente.BatteryBrand != txtMarcaBat.Text)
                {
                    _sb.sbCliente.BatteryBrand = txtMarcaBat.Text;
                    DatiSalvati = false;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("txtMarcaBat_Leave: " + Ex.Message);

            }
        }

        private void txtModelloBat_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_sb.sbCliente.BatteryModel != txtModelloBat.Text)
                {
                    _sb.sbCliente.BatteryModel = txtModelloBat.Text;
                    DatiSalvati = false;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("txtModelloBat_leave: " + Ex.Message);

            }
        }

        private void txtCliCodiceLL_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_sb.sbCliente.BatteryLLId != txtCliCodiceLL.Text)
                {
                    _sb.sbCliente.BatteryLLId = txtCliCodiceLL.Text;
                    DatiSalvati = false;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("txtCliCodiceLL_Leave: " + Ex.Message);

            }
        }

        private void txtIdBat_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_sb.sbCliente.BatteryId != txtIdBat.Text)
                {
                    _sb.sbCliente.BatteryId = txtIdBat.Text;
                    DatiSalvati = false;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("txtIdBat_Leave: " + Ex.Message);

            }
        }

        private void txtCliCicliAttesi_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_sb.sbCliente.CicliAttesi.ToString() != txtCliCicliAttesi.Text)
                {
                    int _tempInt;
                    if (int.TryParse(txtCliCicliAttesi.Text, out _tempInt))
                    { _sb.sbCliente.CicliAttesi = (int)_tempInt; }
                    else
                    { _sb.sbCliente.CicliAttesi = 1000; }

                    txtCliCicliAttesi.Text = _sb.sbCliente.CicliAttesi.ToString();
                    DatiSalvati = false;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("txtCliCicliAttesi_Leave: " + Ex.Message);

            }
        }

        private void txtNoteCliente_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_sb.sbCliente.ClientNote != txtNoteCliente.Text)
                {
                    _sb.sbCliente.ClientNote = txtNoteCliente.Text;
                    DatiSalvati = false;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("txtNoteCliente_Leave: " + Ex.Message);

            }
        }

        private void txtSerialNumber_Leave(object sender, EventArgs e)
        {
            try
            {
                if (_sb.sbCliente.SerialNumber != txtSerialNumber.Text)
                {
                    _sb.sbCliente.SerialNumber = txtSerialNumber.Text;
                    DatiSalvati = false;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("txtNoteCliente_Leave: " + Ex.Message);

            }
        }

        /// <summary>
        /// Handles the DrawItem event of the tabCaricaBatterie control.
        /// Evidenzio il tab corrente cambiando font e colore del titolo
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DrawItemEventArgs"/> instance containing the event data.</param>
        private void tabCaricaBatterie_DrawItem(object sender, DrawItemEventArgs e)
        {

            try
            {



                Graphics g = e.Graphics;
                Brush _TextBrush;

                // Get the item from the collection.
                TabPage _TabPage = tabCaricaBatterie.TabPages[e.Index];

                // Get the real bounds for the tab rectangle.
                Rectangle _TabBounds = tabCaricaBatterie.GetTabRect(e.Index);

                // Use our own font. Because we CAN.
                Font _TabFont;

                if (e.State == DrawItemState.Selected)
                {
                    // Draw a different background color, and don't paint a focus rectangle.
                    _TextBrush = new SolidBrush(Color.White);
                    g.FillRectangle(Brushes.DarkGray, e.Bounds);
                    _TabFont = new Font(e.Font.FontFamily, (float)12, FontStyle.Bold, GraphicsUnit.Pixel);
                }
                else
                {
                    _TextBrush = new System.Drawing.SolidBrush(e.ForeColor);
                    _TabFont = new Font(e.Font.FontFamily, (float)11, FontStyle.Regular, GraphicsUnit.Pixel);
                    // e.DrawBackground();
                }

                //Font fnt = new Font(e.Font.FontFamily, (float)7.5, FontStyle.Bold);

                // Draw string. Center the text.
                StringFormat _StringFlags = new StringFormat();
                _StringFlags.Alignment = StringAlignment.Center;
                _StringFlags.LineAlignment = StringAlignment.Center;
                g.DrawString(tabCaricaBatterie.TabPages[e.Index].Text, _TabFont, _TextBrush, _TabBounds, new StringFormat(_StringFlags));


            }
            catch (Exception Ex)
            {
                Log.Error("tabCaricaBatterie_DrawItem: " + Ex.Message);

            }

        }

        public void DatiDaSalvare(object sender, EventArgs args)
        {
            try
            {
                DatiCambiatiEventArgs ArgChiamata;
                if (args is DatiCambiatiEventArgs)
                {
                    ArgChiamata = (DatiCambiatiEventArgs)args;
                }
                else return;

                DatiSalvati = !ArgChiamata.DaSalvare;


            }

            catch (Exception Ex)
            {
                Log.Error("DatiDaSalvare: " + Ex.Message);
            }
        }



        private void btnRicalcolaSoc_Click(object sender, EventArgs e)
        {
            try
            {

                _sb.RicalcolaSoc(chkMemLngSalvaRicalcolo.Checked);

                InizializzaVistaLunghi(); 
            }
            catch (Exception Ex)
            {
                Log.Error("tabCaricaBatterie_DrawItem: " + Ex.Message);

            }
        }

        private void btnRiconsolidaBrevi_Click(object sender, EventArgs e)
        {
            try
            {

                _sb.ConsolidaBrevi();
                InizializzaVistaLunghi();
            }
            catch (Exception Ex)
            {
                Log.Error("tabCaricaBatterie_DrawItem: " + Ex.Message);

            }
        }

        private void label276_Click(object sender, EventArgs e)
        {

        }

        private void grbStratComandiTest_Enter(object sender, EventArgs e)
        {

        }

        private void btnStratReadPar_Click(object sender, EventArgs e)
        {
            LanciaComandoStrategiaChechPar();
        }

        private void btnFSerVerificaOC_Click(object sender, EventArgs e)
        {
            try
            {
                _sb.CaricaStatoOC(_sb.Id, _apparatoPresente);
                MostraParametriOC();
            }
            catch (Exception Ex)
            {
                Log.Error("btnFSerVerificaOC_Click: " + Ex.Message);
            }
        }

        private void btnFSerImpostaOC_Click(object sender, EventArgs e)
        {
            try
            {
                bool _esito;
                FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

                _esito = _sb.ScriviParametriOC(  (SerialMessage.OcBaudRate)cmbFSerBaudrateOC.SelectedValue, (SerialMessage.OcEchoMode)cmbFSerEchoOC.SelectedValue);

                if (_esito && ((SerialMessage.OcEchoMode)cmbFSerEchoOC.SelectedValue != SerialMessage.OcEchoMode.OFF))
                {
                    // apro la schermata moniitor sig 
                    frmMonitorSig60 _monitorCorrente = new frmMonitorSig60();
                    _monitorCorrente.SetAsSbMonitor();

                    // aggancio l'evento di ricezione su usb al monitor Sig
                    // 1 determino la porta com collegata

                    string _ftdiPortName;
                    ftStatus = _parametri.usbSpyBatt.GetCOMPort(out _ftdiPortName);
                    ftStatus = _parametri.usbSpyBatt.Close();
                    _monitorCorrente.ApriPortaSB(_ftdiPortName, 9600);
                    _monitorCorrente.ShowDialog();
                    //_monitorCorrente.BringToFront();
                    this.Close();

                }
                _sb.CaricaStatoOC(_sb.Id, _apparatoPresente);

                MostraParametriOC();

            }
            catch (Exception Ex)
            {
                Log.Error("btnSvcScriviParametriMedie_Click: " + Ex.Message);

            }

        }

        private void btnTestataCreaClone_Click(object sender, EventArgs e)
        {
            try
            {
                if (_sb != null)
                {
                    this.Cursor = Cursors.WaitCursor;
                    _sb.CreaClone();

                    MostraTestata();
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception Ex)
            {
                Log.Error("btnTestataCreaClone_Click: " + Ex.Message);

            }
        }

        private void btnCalSalvaFWVer_Click(object sender, EventArgs e)
        {

            try
            {
                PannelloCharger.Properties.Settings.Default.VersioneFwRichiesta = txtCalFWRichiesto.Text;
            }
            catch (Exception Ex)
            {
                Log.Error("btnCalSalvaFWVer_Click: " + Ex.Message);
            }

        }
    }
}
