﻿using System;
using System.Configuration;
//using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using MdiHelper;
using log4net;
using log4net.Config;
using ChargerLogic;
using Utility;
using System.Data.SQLite;
using System.Resources;
//using PannelloCharger.


namespace PannelloCharger
{
    public partial class frmMain : MdiParent
    {
        //private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public LogicheBase logiche;
        public parametriSistema varGlobali;
        private CaricaBatteria cbCorrente;
        public ScannerPorte Dispositivi;
        public ScannerUSB DispositiviUSB;

        public StatoPulsanti Toolbar { get; set; } = new StatoPulsanti();

        // artificio per forzare il caricamento delle DLL Interop
        // private System.Data.SQLite.SQLiteCommand _tampone = new SQLiteCommand();
      

        public frmMain()
        {
            try
            {
               
                XmlConfigurator.Configure();
                Log.Info("");
                Log.Info("");
                Log.Info("");
                Log.Info("----------------------------------------------------------------------------------------------------------------------");
                Log.Info("----    LADELIGHT Manager Session Start                                                                           ----");
                Log.Info("----------------------------------------------------------------------------------------------------------------------");
                Log.Info("");
                
                varGlobali = new parametriSistema();
             


                Thread.CurrentThread.CurrentUICulture = varGlobali.currentCulture;            
                InitializeComponent();
                Log.Debug("Startup Applicazione");
                frmMainInitialize();
                
                varGlobali = new parametriSistema();
                logiche = new LogicheBase();
                cbCorrente = new CaricaBatteria(ref varGlobali);
                Toolbar.reset();
                AggiornaToolbar(Toolbar);
                Log.Debug("Main Caricato ");

                //logiche.currentUser.verificaUtente
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message );
            }
        }


        private void frmMainInitialize()
        {
            try
            {
                Log.Debug("frmMainInitialize");
                this.IsMdiContainer = true;

                sstProgressBase.Visible = false;
            }
            catch (Exception Ex)
            {
                Log.Error("frmMainInitialize: " + Ex.Message);
            }
        }


        private void frmMain_Load(object sender, EventArgs e)
        {

            try
            {
                frmLogin frm = new frmLogin(ref logiche,ref varGlobali); //Create a new instance of the demo child form, with a new number.
                frm.MdiParent = this;
                frm.StartPosition = FormStartPosition.CenterScreen;

                ShowChildDialog(frm, frmLogin_DialogReturned); //Show the form as an dialog, nothing else will be enabled. set the recieving method for the dialog result
            }

            catch (Exception)
            {
                //Do some error handling
                this.ForceReleaseOfControls(); //Release all controls again if an error is raised.
            } 
     
        }
        
        private void frmLogin_DialogReturned(object sender, DialogResultArgs e)
        {
            try {
                //Reciever of the dialogresult, as specified in the method call (ShowChildDialog) above.
                if (e.Result == DialogResult.Abort)
                {
                    Log.Debug("Login --> ANNULLA ");

                    logiche.chiudiConn();

                    this.Close();
                }
                else
                {
                    Log.Debug("Login --> OK");

                    //Aggiungere quì gestione grant e lingua
                    ApplicaAutorizzazioni((settings.LivelloUtente)logiche.currentUser.livello);
                }
            }
            catch (Exception Ex)
            {
                Log.Error("frmLogin_DialogReturned: " + Ex.Message);
            }
        }

        /// <summary>
        /// Aggiorno la toolbar in base alle impostazioni
        /// </summary>
        /// <param name="tb">Impostazioni da applicare
        public void AggiornaToolbar(StatoPulsanti tb)
        {
            try
            {
                tstBtnExport.Visible = tb.ExportVisibile;
                tstBtnExport.Enabled = tb.ExportAttivo;

                tstBtnPrint.Visible = tb.StampaVisibile;
                tstBtnPrint.Enabled = tb.StampaAttiva;

                tstBtnRefresh.Visible = tb.RefreshVisibile;
                tstBtnRefresh.Enabled = tb.RefreshAttivo;

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }
        }


        /// <summary>
        /// Applica le autorizzazioni in base al livello dell'utente
        /// </summary>
        /// <param name="Livello">Livello utente corrente</param>
        public void ApplicaAutorizzazioni(settings.LivelloUtente Livello)
        {
            try
            {
               
                mnuCaricabatteria.Visible = false;
                if ((byte)Livello < 0x01) mnuCaricabatteria.Visible = true;

                //---------------- Menu Spy-batt
                mnuSpybat.Visible = false;
                toolStripMenuItem1.Visible = false;
                importaFileToolStripMenuItem.Visible = false;
                toolStripMenuItem4.Visible = false;
                alimentatoreToolStripMenuItem.Visible = false;

                if ((byte)Livello < 0x04) mnuSpybat.Visible = true;
                if ((byte)Livello < 0x02)
                {
                    toolStripMenuItem1.Visible = true;
                    importaFileToolStripMenuItem.Visible = true;
                    toolStripMenuItem4.Visible = true;
                }
                if ((byte)Livello < 0x01)
                {
                    alimentatoreToolStripMenuItem.Visible = true;
                }
                    
                mnuImpostazioni.Visible = false;
                if ((byte)Livello < 0x02) mnuImpostazioni.Visible = true;
                mnuCercaDispositiviUSB.Visible = false;
                if ((byte)Livello < 0x02) mnuCercaDispositiviUSB.Visible = true;
                mnuCercaDispositiviCOM.Visible = false;
                if ((byte)Livello < 0x02) mnuCercaDispositiviCOM.Visible = true;
                mnuSelezionePorta.Visible = false;
                if ((byte)Livello < 0x02) mnuSelezionePorta.Visible = true;
                mnuServizi.Visible = false;
                if ((byte)Livello < 0x02) mnuServizi.Visible = true;


                // Già in questa fase, se non ho i driver FTDI installati, disabilito la scansione USB

                if(!varGlobali.FtdiCaricato)
                {
                    tstBtnCercaUsb.Enabled = false;
                    caricaToolStripMenuItem.Enabled = false;

                }




            }

            catch (Exception Ex)
            {
                Log.Error("frmMain.selezione porta: " + Ex.Message);
            }
        }

        private void portaSerialeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSerialPanel frmComm = new frmSerialPanel();
            frmComm.MdiParent = this;
            frmComm.vGlobali = varGlobali;
            frmComm.Show();
        }

        private void mnuConnettiCb_Click(object sender, EventArgs e)
        {
            ApriLadeLight();
        }

        private void mnuSelezionePorta_Click(object sender, EventArgs e)
        {
            try 
            {

                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmSelettoreCom))
                    {
                        form.Activate();
                        return;
                    }
                }

                frmSelettoreCom selettore = new frmSelettoreCom(ref varGlobali);
                selettore.MdiParent = this;
                selettore.StartPosition = FormStartPosition.CenterParent;
                selettore.Show();
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.selezione porta: " + Ex.Message);
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            try
            {
                this.Refresh();
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.Resize: " + Ex.Message);
            }

        }

        private void caricaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DispositiviUSB = new ScannerUSB();


                if (DispositiviUSB.cercaPorte())
                {
                    if (DispositiviUSB.NumDevSpyBatt > 0)
                    {
                        DialogResult risposta = MessageBox.Show(StringheComuni.TrovatoDispositivo + " SPY-BATT " + StringheComuni.CollegatoUSB + " \n " + StringheComuni.AproCollegamento + " ? ", "SPY-BATT", MessageBoxButtons.YesNo);
                        //DialogResult risposta = MessageBox.Show("Trovato dispositivo SPY-BATT collegato alla porta USB \n Apro il collegamento ? ", "SPY-BATT", MessageBoxButtons.YesNo)//;

                        if (risposta == System.Windows.Forms.DialogResult.Yes)
                        {
                            varGlobali.usbSpyBattSerNum = DispositiviUSB.SpyBattUsbSerialNo;
                            varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                            ApriSpyBatt();
                            return;
                        }

                    }

                }
                else
                {
                    DialogResult risposta = MessageBox.Show(StringheComuni.NoDevice, "LADE LIGHT", MessageBoxButtons.OK);
                }


            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }
        }


        /// <summary>
        /// Cerca i dispositivi collegat
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCercaDispositivi_Click(object sender, EventArgs e)
        {
            try
            {
                Dispositivi = new ScannerPorte();


                if (Dispositivi.cercaPorte())
                {
                    if (Dispositivi.PorteSpyBatt > 0)
                    {
                        DialogResult risposta = MessageBox.Show( StringheComuni.TrovatoDispositivo + " SPY-BATT "+ StringheComuni.Collegato +" " + Dispositivi.PortaSpyBatt + "\n " + StringheComuni.AproCollegamento +" ? ", "SPY-BATT", MessageBoxButtons.YesNo);

                        if (risposta == System.Windows.Forms.DialogResult.Yes)
                        {
                            varGlobali.portName = Dispositivi.PortaSpyBatt;
                            varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.Seriale;
                            ApriSpyBatt();
                            return;
                        }

                    }
                    if (Dispositivi.PorteLadeLight > 0)
                    {
                        DialogResult risposta = MessageBox.Show(StringheComuni.TrovatoDispositivo + " LADE LIGHT " + StringheComuni.CollegatoUSB + " \n " + StringheComuni.AproCollegamento + " ? ", " LADE LIGHT", MessageBoxButtons.YesNo);
                       // DialogResult risposta = MessageBox.Show("Trovato dispositivo LADE LIGHT collegato alla porta " + Dispositivi.PortaLadeLight + "\n Apro il collegamento ? ", "LADE LIGHT", MessageBoxButtons.YesNo);

                        if (risposta == System.Windows.Forms.DialogResult.Yes)
                        {
                            varGlobali.portName = Dispositivi.PortaLadeLight;
                            varGlobali.CanaleLadeLight = parametriSistema.CanaleDispositivo.Seriale;
                            ApriLadeLight();
                            return;
                        }

                    }

                }
                else
                {
                    DialogResult risposta = MessageBox.Show(StringheComuni.NoDevice, "LADE LIGHT", MessageBoxButtons.OK);
                }
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }



        }

        private void ApriLadeLight()
        {
            try
            {
                bool esitoCanaleApparato = false;

                Log.Debug("Apri Canale Lade Light LL");


                // se la porta seriale non è aperta , la apro
                // -- rev 14/09  se già aperta chiudo e riapro             
                if (varGlobali.statoCanaleLadeLight())
                {
                    Log.Debug("USB LadeLight aperto - lo richiudo");
                    varGlobali.chiudiCanaleLadeLight();
                }

                esitoCanaleApparato = varGlobali.apriLadeLight();

                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmCaricabatterie))
                    {
                        form.Activate();
                        return;
                    }
                }
                Log.Debug("NUOVO LL");
                //frmSpyBat sbCorrente = new frmSpyBat(ref varGlobali, true, "", logiche, esitoCanaleApparato, true);

                frmCaricabatterie cbCorrente = new frmCaricabatterie(ref varGlobali, true);
                cbCorrente.Cursor = Cursors.WaitCursor;

                cbCorrente.MdiParent = this;
                cbCorrente.StartPosition = FormStartPosition.CenterParent;
                //cbCorrente.Cursor = Cursors.WaitCursor;
                cbCorrente.Show();

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.ApriLadeLight: " + Ex.Message);
            }

        }

        private void ApriSpyBatt()
        {
            try
            {
                bool esitoCanaleApparato = false;
                // se la porta seriale non è aperta , la apro
                // -- rev 14/09  se già aperta chiudo e riapro
                if (varGlobali.statoCanaleSpyBatt()) varGlobali.chiudiCanaleSpyBatt();

                this.Cursor = Cursors.WaitCursor;

                esitoCanaleApparato = varGlobali.apriSpyBat();

                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmSpyBat))
                    {
                        form.Activate();
                        return;
                    }
                }
                Log.Debug("NUOVO SB");
                frmSpyBat sbCorrente = new frmSpyBat(ref varGlobali, true, "", logiche, esitoCanaleApparato, true);
                sbCorrente.MdiParent = this;
                sbCorrente.StartPosition = FormStartPosition.CenterParent;

                this.Cursor = Cursors.Default;

                Log.Debug("PRIMA");
                sbCorrente.Show();
                Log.Debug("DOPO");

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.ApriSpyBatt: " + Ex.Message);
            }

        }

        private void archivioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmSelettoreSpyBatt))
                    {
                        form.Activate();
                        return;
                    }
                }

                frmSelettoreSpyBatt ArchivioCorrente = new frmSelettoreSpyBatt(ref varGlobali, logiche);
                ArchivioCorrente.MdiParent = this;
                ArchivioCorrente.StartPosition = FormStartPosition.CenterParent;
                ArchivioCorrente.MostraLista();
                ArchivioCorrente.Show();



            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }
        }

        private void importaFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {


                frmSbExport sbExport = new frmSbExport(ref varGlobali, true, "", logiche, false, false);
                sbExport.MdiParent = this;
                sbExport.StartPosition = FormStartPosition.CenterParent;
                sbExport.Setmode(elementiComuni.modoDati.Import);
                sbExport.Show();

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }
        }

        private void cercaDispositiviUSBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DispositiviUSB = new ScannerUSB();


                if (DispositiviUSB.cercaPorte())
                {

                    ApriSelettoreDevices();

                }
                else
                {
                    DialogResult risposta = MessageBox.Show(StringheComuni.NoDevice, "LADE LIGHT", MessageBoxButtons.OK);
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }

        }

        private void tstBtnCercaUsb_Click(object sender, EventArgs e)
        {
            cercaDispositiviUSBToolStripMenuItem_Click(sender, e);
        }

        private void tstBtnCercaRS232_Click(object sender, EventArgs e)
        {
            mnuCercaDispositivi_Click(sender, e);
        }

        private void mnuInformazioniSu_Click(object sender, EventArgs e)
        {
            try
            {


                frmAboutBox AboutBox = new frmAboutBox();
                AboutBox.MdiParent = this;
                AboutBox.StartPosition = FormStartPosition.CenterParent;
                AboutBox.Show();

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }
        }

        public void mostraAvanzamento(bool Visibile = true)
        {
            try
            {
                sstProgressBase.Visible = Visibile;

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                System.Environment.Exit(0);
                Log.Info("App Exit ");
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }

        }

        private void caricaParametriToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void mnuStampa_Click(object sender, EventArgs e)
        {
            try
            {
                stampa(false,true);
            }
            catch
            {
            }
        }

        private void mnuEsci_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }

        }


        private void mnuLogout_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult _conferma = MessageBox.Show(StringheMessaggio.ConfermaDisconnessione, "Logout",MessageBoxButtons.YesNo);
                if (_conferma == System.Windows.Forms.DialogResult.Yes)
                {
                    //Chiudo tutti i form figli
                    foreach (Form _figlio in this.MdiChildren)
                    {
                        _figlio.Close();
                    }

                    //poi rifaccio la login

                    frmMain_Load(null, null);
                }
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

        }

        private void alimentatoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmAlimentatore))
                    {
                        form.Activate();
                        return;
                    }
                }

                frmAlimentatore Alimentatore = new frmAlimentatore();
                Alimentatore.MdiParent = this;
                Alimentatore.StartPosition = FormStartPosition.CenterParent;

                Alimentatore.Show();


            }

            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }
        }

        private void flashFTDIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmFlashFTDI))
                    {
                        form.Activate();
                        return;
                    }
                }

                frmFlashFTDI flasFTDI = new frmFlashFTDI();//ref varGlobali);
                flasFTDI.MdiParent = this;
                flasFTDI.StartPosition = FormStartPosition.CenterParent;
                flasFTDI.Show();
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.flasFTDI : " + Ex.Message);
            }
        }

        private void ApriSelettoreDevices()
        {
            frmSelettoreDevice _formCorrente;
            try
            {
                bool _formPresente = false;

                _formCorrente = new frmSelettoreDevice();
                int _numApparati = DispositiviUSB.NumDevSpyBatt + DispositiviUSB.NumDevLadeLight;

                if (false ) //_numApparati == 1)
                {
                    if (DispositiviUSB.NumDevSpyBatt > 0)
                    {

                        varGlobali.usbSpyBattSerNum = DispositiviUSB.SpyBattUsbSerialNo;
                        varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                        ApriSpyBatt();
                        return;

                    }

                }

                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmSelettoreDevice))
                    {
                        _formCorrente = (frmSelettoreDevice)form;
                        _formPresente = true;
                        break;
                    }
                }

                if (!_formPresente)
                {
                    _formCorrente = new frmSelettoreDevice(ref varGlobali);
                    _formCorrente.MdiParent = this;
                    _formCorrente.StartPosition = FormStartPosition.CenterParent;
                }

                // frmSelettoreSpyBatt ArchivioCorrente = new frmSelettoreSpyBatt(ref varGlobali, logiche);
                //ArchivioCorrente.MostraLista();
                //_formCorrente.varGlobali = varGlobali;

                _formCorrente.logiche = logiche;
                _formCorrente.Scanner = DispositiviUSB;
                _formCorrente.ListaPorte = DispositiviUSB.ListaPorte;
                _formCorrente.MostraLista();
                _formCorrente.Show();



            }
            catch (Exception Ex)
            {
                Log.Error("frmMain: " + Ex.Message);
            }
        }

        private void tstBtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                Form _tempChild = this.ActiveMdiChild;
                

                if (_tempChild == null) return;

                if (this.ActiveMdiChild.GetType().Name == "frmSelettoreDevice")
                {
                    frmSelettoreDevice _TempLista;
                    _TempLista = (frmSelettoreDevice)_tempChild;
                    _TempLista.Aggiorna();

                }
                // Se dettaglio ciclo resetto lo zoom
                if (this.ActiveMdiChild.GetType().Name == "frmListaCicliBreve")
                {
                    frmListaCicliBreve _TempLista;
                    _TempLista = (frmListaCicliBreve)_tempChild;
                    _TempLista.Reset();

                }



            }
            catch (Exception Ex)
            {
                Log.Error("frmMain - tstBtnRefresh_Click: " + Ex.Message);
            }
        }

        private void italianoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Setto la lingua di default IT
            impostaCultura("it");

        }



        public void impostaCultura( string CodiceLingua)
        {
            try
            {
                if (varGlobali.currentCultureValue != CodiceLingua)
                {

                    if (CodiceLingua == "")
                        CodiceLingua = "it";


                    DialogResult _conferma = MessageBox.Show(StringheMessaggio.ConfermaCambioLingua, "Lingua", MessageBoxButtons.YesNo);
                    if (_conferma == System.Windows.Forms.DialogResult.Yes)
                    {
                        //Chiudo tutti i form figli
                        foreach (Form _figlio in this.MdiChildren)
                        {
                            _figlio.Close();
                        }


                    

                    varGlobali.impostaCultura(CodiceLingua);
                    Thread.CurrentThread.CurrentUICulture = varGlobali.currentCulture;
                    this.Controls.Clear();
                    InitializeComponent();
                    frmMainInitialize();
                    // Riapplico le autorizzazioni per creare i menu
                    ApplicaAutorizzazioni((settings.LivelloUtente)logiche.currentUser.livello);
                    }
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain - impostaCultura: " + Ex.Message);
            }
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            impostaCultura("en");

        }

        private void tstBtnClose_Click(object sender, EventArgs e)
        {
            try
            {
                Form _tempChild = this.ActiveMdiChild;
                //frmSpyBat _tmpSpyBatt;

                if (_tempChild == null) return;
                _tempChild.Close();

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain - tstBtnRefresh_Click: " + Ex.Message);
            }
        }

        private void impostaStampanteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                PrintDialog _printDialog = new PrintDialog();
                if (varGlobali.ImpostazioniStampante != null)
                {
                    _printDialog.PrinterSettings = varGlobali.ImpostazioniStampante;
                }

                DialogResult _result = _printDialog.ShowDialog();
                if (_result == DialogResult.OK)
                {
                    varGlobali.ImpostazioniStampante = _printDialog.PrinterSettings;
                }
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain - impostaStampanteToolStripMenuItem_Click: " + Ex.Message);
            }
        }

        private void tstBtnPrint_Click(object sender, EventArgs e)
        {
            try
            {


                stampa(false,false);


            }
            catch (Exception Ex)
            {
                Log.Error("frmMain - tstBtnRefresh_Click: " + Ex.Message);
            }
        }


        public void stampa(bool preview = false, bool SelPrinter = false)
        {
            try
            {
                Form _tempChild = this.ActiveMdiChild;

                // Se non ho finestre figlio aperte esco
                if (_tempChild == null) return;
                if (_tempChild is frmSpyBat)
                {
                    frmSpyBat _tempF = (frmSpyBat)_tempChild;
                    _tempF.stampa(preview, SelPrinter);
                    return;
                }
                if (_tempChild is frmListaCicliBreve)
                {
                    frmListaCicliBreve _tempF = (frmListaCicliBreve)_tempChild;
                    _tempF.stampa(preview, SelPrinter);
                    return;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("frmMain - stampa: " + Ex.Message);
            }
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stampa(true, false);
        }

        private void tstBtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Form _tempChild = this.ActiveMdiChild;

                // Se non ho finestre figlio aperte esco
                if (_tempChild == null) return;
                if (_tempChild is frmSpyBat)
                {
                    frmSpyBat _tempF = (frmSpyBat)_tempChild;
                    _tempF.export();
                    return;
                }
                if (_tempChild is frmSelettoreSpyBatt)
                {
                    frmSelettoreSpyBatt _tempF = (frmSelettoreSpyBatt)_tempChild;
                    _tempF.EsportaDatiRiga();
                    return;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("frmMain - stampa: " + Ex.Message);
            }
        }

        private void programmazioniAvanzateToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //UnitaSpyBatt _sb = new UnitaSpyBatt(null,0 );
            frmInserimentoProgramma NuovoProgramma = new frmInserimentoProgramma(logiche);
            //NuovoProgramma._sb = _sb;
            //NuovoProgramma.MdiParent = this.MdiParent;
            NuovoProgramma.TestMode = true;
            NuovoProgramma.StartPosition = FormStartPosition.CenterParent;
            NuovoProgramma.ShowDialog(this);
        }

        private void mnuConsole_Click(object sender, EventArgs e)
        {

        }

        private void mnuLLDisplayManager_Click(object sender, EventArgs e)
        {
            frmDisplayManager frmDisplay = new frmDisplayManager();
            frmDisplay.MdiParent = this;
           // frmDisplay.vGlobali = varGlobali;
            frmDisplay.Show();
        }
    }

    public class StatoPulsanti
    {
        public bool StampaAttiva { get; set; }
        public bool StampaVisibile { get; set; }

        public bool RefreshAttivo { get; set; }
        public bool RefreshVisibile { get; set; }


        public bool ExportAttivo { get; set; }
        public bool ExportVisibile { get; set; }


        public StatoPulsanti()
        {
            reset();
        }

        public void reset()
        {
            StampaAttiva = false;
            StampaVisibile = true;

            RefreshAttivo = false;
            RefreshVisibile = true;

            ExportAttivo = false;
            ExportVisibile = true;

        }


    }
}