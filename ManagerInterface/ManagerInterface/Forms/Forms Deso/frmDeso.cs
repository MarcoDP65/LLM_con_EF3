﻿using System;
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
using MoriData;
using ChargerLogic;
using NextUI.Component;
using NextUI.Frame;

//using Common.Logging;
using BrightIdeasSoftware;
using Utility;
using MdiHelper;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using ExcelDataReader;

using System.Diagnostics;
using Syncfusion.Windows.Forms.Tools.MultiColumnTreeView;

namespace PannelloCharger
{
    public partial class frmDesolfatatore : Form
    {
        const int TabParametri = 6;
        const int DataBlock = 128;

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
        FileSetupRigeneratore PacchettoSetupLingue;


        private DataSet LanguageDataSet;


        /*--------------------*/
        private OxyPlot.PlotModel grCompSOH;
        private OxyPlot.PlotModel grCompRabb;
        private OxyPlot.PlotModel grCompEnConsumata;
        private OxyPlot.PlotModel grCompCO2;



        private OxyPlot.WindowsForms.PlotView oxyContainerGrSingolo;
        private OxyPlot.WindowsForms.PlotView oxyContainerGrAnalisi;
        private OxyPlot.PlotModel oxyGraficoSingolo;
        private OxyPlot.PlotModel oxyGraficoAnalisi;

        public byte[] DataBuffer;
        public byte[] LngDataBuffer;

        private List<string> LingueValide;
        public brInitSetup InitSetup;



        // delegate is used to write to a UI control from a non-UI thread
        public delegate void SetTextDeleg(string text1);

        private bool _hidePhasesButtons = true;

        public System.Collections.Generic.List<mrDataPoint> ValoriPuntualiGrafico = new List<mrDataPoint>();

        public System.Collections.Generic.List<sbAnalisiCorrente> ValoriTestCorrente = new List<sbAnalisiCorrente>();
        public System.Collections.Generic.List<mrDataPoint> ValoriPuntualiGrCorrenti = new List<mrDataPoint>();

        public FileSetupRigeneratore ImmagineDeso = null;


        public string IdCorrente;

        public frmDesolfatatore(ref parametriSistema _par, bool CaricaDati, string IdApparato, LogicheBase Logiche, bool SerialeCollegata, bool AutoUpdate, bool ModoDesolfatatore = true)
        {
            bool _esito;
            try
            {

                _parametri = _par;
                System.Threading.Thread.CurrentThread.CurrentUICulture = _parametri.currentCulture;

                InitializeComponent();
                ResizeRedraw = true;
                _logiche = Logiche;

                Log.Debug("----------------------- frmSpyBat ---------------------------");

                _msg = new MessaggioSpyBatt(ModoDesolfatatore);

                _sb = new UnitaSpyBatt(ref _parametri, _logiche.dbDati.connessione, _logiche.currentUser.livello, ModoDesolfatatore);
                //_sb.Desolfatatore = ModoDesolfatatore;

                _stat = new StatMemLungaSB();
                InitSetup = new brInitSetup(_logiche.dbDati.connessione);
                InitSetup.caricaDati(1);
                MostraInitSetup();

                string _idCorrente = IdApparato;
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
                        //TODO: gestire io DB se la scheda è già in archivio
                        CaricaTestata(IdApparato, Logiche, SerialeCollegata);
                    }
                    else
                    {
                        MostraTestata();
                    }
                }

                IdCorrente = _sb.Id;

                LingueValide = new List<string> { "0) - 0x126000", "1) - 0x127000", "2) - 0x128000", "3) - 0x129000", "4) - 0x12A000", "5) - 0x12B000" };

                cmbLngListaBlocchi.DataSource = LingueValide;

                applicaAutorizzazioni();

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }

        }

        public frmDesolfatatore()
        {
            Log.Debug("----------------------- frmSpyBat  Easy ---------------------------");
            InitializeComponent();

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
            btnDumpMemoria.Enabled = _apparatoPresente;
            btnCaricaListaLunghi.Enabled = _apparatoPresente;
            btnCaricaListaUltimiLunghi.Enabled = _apparatoPresente;
            btnCaricaDettaglioSel.Enabled = _apparatoPresente;
            btnLeggiVariabili.Enabled = _apparatoPresente;
            chkParLetturaAuto.Enabled = _apparatoPresente;

        }

        public bool caricaDati(string IdApparato, LogicheBase Logiche, bool SerialeCollegata)
        {
            try
            {
                bool _esito;
                bool _connessioneAttiva = false;

                Log.Debug("----------------------- caricaDati ---------------------------");

                //_sb = new UnitaSpyBatt(ref _parametri, Logiche.dbDati.connessione);
                string _idCorrente = IdApparato;

                if (SerialeCollegata)
                {
                    _msg = new MessaggioSpyBatt(true);
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
                    _msg = new MessaggioSpyBatt(true);
      
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

                //_sb.CaricaTestata(_idCorrente, _connessioneAttiva);

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("caricaDati " + Ex.Message);
                return false;
            }
        }


        /*
                public void stampaScheda()
                {

                    try
                    {
                        daReport.DaPrintDocument daPrintDocument;
                        daPrintDocument = new daReport.DaPrintDocument();

                        // Carico i parametri di testata:
                        // (parameter names are case sensitive)
                        Hashtable parameters = new Hashtable();
                        parameters.Add("matricola", FunzioniMR.StringaSeriale(_sb.Id));
                        parameters.Add("modello", _sb.sbData.ProductId);
                        parameters.Add("revHw", _sb.sbData.HwVersion.ToString());
                        parameters.Add("revSw", _sb.sbData.SwVersion.ToString());
                        parameters.Add("cliCliente", _sb.sbCliente.Client);
                        parameters.Add("cliMarca", _sb.sbCliente.BatteryBrand);
                        parameters.Add("cliModBatteria", _sb.sbCliente.BatteryModel);
                        parameters.Add("cliIdBatteria", _sb.sbCliente.BatteryId);
                        parameters.Add("cliNote", _sb.sbCliente.ClientNote);
                        daPrintDocument.SetParameters(parameters);


                        //Carico le tabelle
                        //1. Programmazioni
                        DataTable tblProgrammazioni = new DataTable("tblProgrammazioni");
                        tblProgrammazioni.Columns.Add(new DataColumn("prgNumProg"));
                        tblProgrammazioni.Columns.Add(new DataColumn("prgVdef"));
                        tblProgrammazioni.Columns.Add(new DataColumn("prgAdef"));
                        tblProgrammazioni.Columns.Add(new DataColumn("prgType"));
                        tblProgrammazioni.Columns.Add(new DataColumn("prgTotCelle"));
                        tblProgrammazioni.Columns.Add(new DataColumn("prgCelle3"));
                        tblProgrammazioni.Columns.Add(new DataColumn("prgCelle2"));
                        tblProgrammazioni.Columns.Add(new DataColumn("prgCelle1"));
                        //Carico i dati:
                        foreach (sbProgrammaRicarica _programma in _sb.Programmazioni)
                        {
                            tblProgrammazioni.Rows.Add(new object[] {_programma.IdProgramma.ToString(),
                                                                     _programma.BatteryVdef.ToString(),
                                                                     _programma.BatteryAhdef.ToString(),
                                                                     _programma.BatteryType.ToString(),
                                                                     _programma.BatteryCells.ToString(),
                                                                     _programma.BatteryCell3.ToString(),
                                                                     _programma.BatteryCell2.ToString(),
                                                                     _programma.BatteryCell1.ToString()
                                                                     });
                        }

                        daPrintDocument.AddData(tblProgrammazioni);

                        //2. Cicli Lunghi
                        DataTable tblCicli = new DataTable("cicliLunghi");
                        tblCicli.Columns.Add(new DataColumn("NumLungo"));
                        tblCicli.Columns.Add(new DataColumn("TipoCiclo"));
                        tblCicli.Columns.Add(new DataColumn("StartCiclo"));
                        tblCicli.Columns.Add(new DataColumn("EndCiclo"));
                        tblCicli.Columns.Add(new DataColumn("DurataCiclo"));
                        tblCicli.Columns.Add(new DataColumn("TempMin"));
                        tblCicli.Columns.Add(new DataColumn("TempMax"));
                        tblCicli.Columns.Add(new DataColumn("vmin"));
                        tblCicli.Columns.Add(new DataColumn("vmax"));
                        tblCicli.Columns.Add(new DataColumn("imin"));
                        tblCicli.Columns.Add(new DataColumn("imax"));
                        //Carico i dati:
                        int _maxRighe = 0;
                        foreach (sbMemLunga _ciclo in _sb.CicliMemoriaLunga)
                        {
                            tblCicli.Rows.Add(new object[] {_ciclo.IdMemoriaLunga.ToString(),
                                                            _sb.StringaTipoEvento( _ciclo.TipoEvento),
                                                            _ciclo.DataOraStart,
                                                            _ciclo.DataOraFine,
                                                            _sb.StringaDurata( _ciclo.Durata),
                                                            _ciclo.strTempMin,
                                                            _ciclo.strTempMax,
                                                            _ciclo.strVmin,
                                                            _ciclo.strVmax,
                                                            _ciclo.strAmin,
                                                            _ciclo.strAmax
                                                            });
                            _maxRighe++;
                            if (_maxRighe > 50) break;
                        }

                        daPrintDocument.AddData(tblCicli);




                        // set .xml file for printing
                        daPrintDocument.setXML("SpyBattTemplate.xml");

                        // print preview
                        printPreviewDialogSB = new PrintPreviewDialog();
                        printPreviewDialogSB.Document = daPrintDocument;
                        printPreviewDialogSB.PrintPreviewControl.Zoom = 1.0;
                        printPreviewDialogSB.WindowState = FormWindowState.Maximized;
                        printPreviewDialogSB.ShowDialog(this);

                    }
                    catch (Exception Ex)
                    {
                        Log.Error("MostraTestata: " + Ex.Message);
                    }
                }
        */

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
                }
                else
                {
                    //scheda senza booloader
                    txtRevSWSb.Text = _sb.sbData.SwVersion.ToString();
                    txtRevLdrSb.Text = "";

                }
                


                txtManufcturedBy.Text = _sb.sbData.ProductId;

                txtMainNumLunghi.Text = _sb.sbData.LongMem.ToString();
                txtMainNumProgr.Text = _sb.sbData.ProgramCount.ToString();

                txtEventiCSLunghi.Text = _sb.sbData.LongMem.ToString();
                txtCicliProgrammazione.Text = _sb.sbData.ProgramCount.ToString();


                // Carico l'area Contatori - Solo dal comando Testata
                // Prima vuoto le tb
                txtTestataContLunghi.Text = "";
                txtTestataPtrLunghi.Text = "";
                txtTestataContBrevi.Text = "";
                txtTestataPtrBrevi.Text = "";
                txtTestataContProgr.Text = "";
                txtTestataPtrProgr.Text = "";

                if (_sb.IntestazioneSb != null )
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

                grbMainDlOptions.Visible = false;
                btnResetScheda.Visible = false;
                grbTestataContatori.Visible = false;
                if (LivelloCorrente == 0)
                {
                    grbMainDlOptions.Visible = true;
                    btnResetScheda.Visible = true;
                    grbTestataContatori.Visible = true;
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
                   // tabCaricaBatterie.TabPages.Remove(tabCb03);
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
                    btnScriviRtc.Enabled = false;
                #endregion

                #region "Gestione Lingue"
                // accessibile solo a Factory 
                if (LivelloCorrente > 0)
                {
                    tabCaricaBatterie.TabPages.Remove(tabCb02);
                    _readonly = true;
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
                btnDumpMemoria.Visible = _enabled;
                btnStampaScheda.Visible = _enabled;

                grbCalibrazionePulsanti.Visible = _enabled;
                grbCalibrazioni.Visible = _enabled;

                chkDatiDiretti.Visible = _enabled;

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


                #region "Calibrazione"

             

                #endregion



                #region "Firmware"
                // accessibile solo a Factory 
                if (LivelloCorrente > 0)
                {
                    tabCaricaBatterie.TabPages.Remove(tbpFirmware);
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
                    CaricaCicli();
                    if (caricaCliente(IdApparato, Logiche, SerialeCollegata)) mostraCliente();
                    _apparatoPresente = _sb.apparatoPresente;
                    // se l'apparato è collegato abilito i salvataggi
                    abilitaSalvataggi(_apparatoPresente);

                    CaricaProgrammazioni();
                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
                return false;
            }
        }

        public bool caricaCliente(string IdApparato, LogicheBase Logiche, bool SerialeCollegata)
        {
            try
            {
                //bool _esito;
                bool _connessioneAttiva = false;

                //_sb = new UnitaSpyBatt(ref _parametri, Logiche.dbDati.connessione);
                string _idCorrente = IdApparato;

                if (SerialeCollegata)
                {
                    _connessioneAttiva = true;
                }

                _sb.CaricaDatiCliente(_idCorrente, _connessioneAttiva);

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("caricaClente " + Ex.Message);
                return false;
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
                        txtVarVBattT.Text = _sb.sbVariabili.TensioneTampone.ToString();
                        txtVarVBatt.Text = _sb.sbVariabili.TensioneIstantanea.ToString();
                        txtVarV3.Text = _sb.sbVariabili.Tensione3.ToString();
                        txtVarV2.Text = _sb.sbVariabili.Tensione2.ToString();
                        txtVarV1.Text = _sb.sbVariabili.Tensione1.ToString();
                        txtVaIbatt.Text = _sb.sbVariabili.CorrenteBatteria.ToString();
                        txtVarAhCarica.Text = _sb.sbVariabili.AhCaricati.ToString();
                        txtVarAhScarica.Text = _sb.sbVariabili.AhScaricati.ToString();
                        txtVarTempNTC.Text = _sb.sbVariabili.TempNTC.ToString();
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
                    else
                    {
                        txtVarVBattT.Text = _sb.sbVariabili.strTensioneTampone;
                        txtVarVBatt.Text = _sb.sbVariabili.strTensioneIstantanea;
                        txtVarV3.Text = _sb.sbVariabili.strTensione3;
                        txtVarV2.Text = _sb.sbVariabili.strTensione2;
                        txtVarV1.Text = _sb.sbVariabili.strTensione1;
                        txtVaIbatt.Text = _sb.sbVariabili.strCorrenteBatteria;
                        if (_sb.sbVariabili.CorrenteBatteria < 0) txtVaIbatt.ForeColor = Color.Red;
                        else txtVaIbatt.ForeColor = Color.Black;
                        txtVarAhCarica.Text = _sb.sbVariabili.strAhCaricati;
                        txtVarAhScarica.Text = _sb.sbVariabili.strAhScaricati;
                        txtVarTempNTC.Text = _sb.sbVariabili.strTempNTC;
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

                //frmAvanzamentoCicli AvCicli = new frmAvanzamentoCicli();

                //AvCicli.MdiParent = this.MdiParent;
                //AvCicli.StartPosition = FormStartPosition.CenterScreen;






                //frmAvanzamentoCicli AvCicli = new frmAvanzamentoCicli();
                //AvCicli.TopLevel = true;


                //AvCicli.Show();

                Application.DoEvents();

                //impostaAvanzamento(Convert.ToInt32(txtMemDa.Text), Convert.ToInt32(txtMemA.Text));
                //mostraAvanzamento(true);








                Log.Debug("RicaricaCicli da Memoria: ");

                _esito = _sb.CaricaCicliMemLungaDaMem(Convert.ToUInt32(txtMemDa.Text), Convert.ToUInt32(txtMemA.Text));



                //mostraAvanzamento(false);
                //AvCicli.Close();
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
                //flvwLngSourceFile.RefreshObject(_sb.Programmazioni);
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

                    if (LivelloCorrente < 3) _readonly = true; else _readonly = false;


                    if (_readonly)
                        flvwCicliBatteria.Height = this.Height - 290;
                    else
                        flvwCicliBatteria.Height = this.Height - 130;

                    grbEsportaExcel.Top = this.Height - 250;
                    grbMemCicliContatori.Top = this.Height - 250;
                    grbMemCicliReadMem.Top = this.Height - 250;
                    grbMemCicliPulsanti.Top = this.Height - 170;




                    /*
                    btnCaricaListaLunghi.Top = this.Height - 140;
                    btnRigeneraLista.Top = this.Height - 140;
                    btnCaricaListaUltimiLunghi.Top = this.Height - 140;
                    btnDettaglioCicliBrevi.Top = this.Height - 140;
                    btnCaricaDettaglioSel.Top = this.Height - 140;
                    */


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


        private void MostraDettaglioFase( UInt32 IdCiclo )
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

        private void MostraNuovoProgramma()
        {
            try
            {
                if (_sb.apparatoPresente)
                {


                    frmInserimentoProgramma NuovoProgramma = new frmInserimentoProgramma( _logiche,ref _parametri);
                    NuovoProgramma._sb = _sb;
                    //NuovoProgramma.MdiParent = this.MdiParent;
                    NuovoProgramma.StartPosition = FormStartPosition.CenterParent;

                    NuovoProgramma.ShowDialog(this);
                    //NuovoProgramma.MostraCicli();

                    this.Cursor = Cursors.WaitCursor;

                    // 18/11/15 - Prima di ricaricare la lista, ricarico la restata per leggere il contatore aggiornato


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
            { }

        }

        private void btnDettaglioCicliBrevi_Click(object sender, EventArgs e)
        {
            MostraDettaglioRiga();
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

                _esito = _sb.LeggiOrologio();
                if (_esito)
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

        public void salvaCliente()
        {
            try
            {
                // prima salvo i dati nella classe
                _sb.sbCliente.Client = txtCliente.Text;
                _sb.sbCliente.ClientNote = txtNoteCliente.Text;
                _sb.sbCliente.BatteryBrand = txtMarcaBat.Text;
                _sb.sbCliente.BatteryModel = txtModelloBat.Text;
                _sb.sbCliente.BatteryId = txtIdBat.Text;
                int _tempInt;
                if (int.TryParse(txtCliCicliAttesi.Text, out _tempInt))
                { _sb.sbCliente.CicliAttesi = (int)_tempInt; }
                else
                { _sb.sbCliente.CicliAttesi = 1000; }

                _sb.ScriviDatiCliente(true);
                if (caricaCliente(_sb.Id, _logiche, true)) mostraCliente();
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

                if (LivelloCorrente < 3) _colonnaNascosta = true; else _colonnaNascosta = false;

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
                Colonna6m.AspectName = "TempMin";
                Colonna6m.Sortable = false;
                Colonna6m.Width = 60;
                Colonna6m.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna6m.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna6m);

                BrightIdeasSoftware.OLVColumn Colonna6 = new BrightIdeasSoftware.OLVColumn();
                Colonna6.Text = StringheMessaggio.strVistaLunghiCol06;
                Colonna6.AspectName = "TempMax";
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
                Colonna9.Text = "I min";
                Colonna9.AspectName = "olvAmin";
                Colonna9.Sortable = false;
                Colonna9.ToolTipText = "Valore minimo Ampere caricati (nero) o scaricati (rosso)";
                Colonna9.Width = 50;
                Colonna9.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna9.TextAlign = HorizontalAlignment.Right;
                Colonna9.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(Colonna9);

                BrightIdeasSoftware.OLVColumn Colonna10 = new BrightIdeasSoftware.OLVColumn();
                Colonna10.Text = "I max";
                Colonna10.Sortable = false;
                Colonna10.AspectName = "olvAmax";
                Colonna10.ToolTipText = "Valore massimo Ampere caricati (nero) o scaricati (rosso)";
                Colonna10.Width = 50;
                Colonna10.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna10.TextAlign = HorizontalAlignment.Right;
                Colonna10.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(Colonna10);

                BrightIdeasSoftware.OLVColumn Colonna11 = new BrightIdeasSoftware.OLVColumn();
                Colonna11.Text = "Elettrolita";
                Colonna11.Sortable = false;
                Colonna11.AspectName = "PresenzaElettrolita";
                Colonna11.AspectGetter = delegate (object _Valore)
                {
                    sbMemLunga _tempVal = (sbMemLunga)_Valore;
                    return FunzioniMR.StringaPresenza(_tempVal.PresenzaElettrolita);
                };
                Colonna11.Width = 60;
                Colonna11.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna11.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna11);

                BrightIdeasSoftware.OLVColumn Colonna12 = new BrightIdeasSoftware.OLVColumn();
                Colonna12.Text = "Ah";
                Colonna12.AspectName = "strAh";
                Colonna12.Sortable = false;
                /*
                Colonna12.AspectGetter = delegate(object _Valore)
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
                };*/
                Colonna12.Width = 50;
                Colonna12.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna12.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna12);


                BrightIdeasSoftware.OLVColumn ColAhCaricati = new BrightIdeasSoftware.OLVColumn();
                ColAhCaricati.Text = "Ah C.";
                ColAhCaricati.Sortable = false;
                ColAhCaricati.ToolTipText = "Ah Caricati";
                ColAhCaricati.AspectName = "strAhCaricati";
                ColAhCaricati.Width = 50;
                ColAhCaricati.HeaderTextAlign = HorizontalAlignment.Center;
                ColAhCaricati.TextAlign = HorizontalAlignment.Right;
                ColAhCaricati.IsVisible = _colonnaNascosta;
                flvwCicliBatteria.AllColumns.Add(ColAhCaricati);

                BrightIdeasSoftware.OLVColumn ColAhScaricati = new BrightIdeasSoftware.OLVColumn();
                ColAhScaricati.Text = "Ah Sc.";
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
                Colonna13C.Text = "KWh C.";
                Colonna13C.AspectName = "strKWhCaricati";
                Colonna13C.Sortable = false;

                Colonna13C.Width = 60;
                Colonna13C.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna13C.TextAlign = HorizontalAlignment.Right;
                flvwCicliBatteria.AllColumns.Add(Colonna13C);

                BrightIdeasSoftware.OLVColumn Colonna13S = new BrightIdeasSoftware.OLVColumn();
                Colonna13S.Text = "KWh S.";
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
                Colonna20.Text = "max Sbil.";
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
                colIdProgramma.Text = "Conf";
                colIdProgramma.Sortable = false;
                colIdProgramma.AspectName = "strIdProgramma";
                colIdProgramma.Width = 40;
                colIdProgramma.HeaderTextAlign = HorizontalAlignment.Center;
                colIdProgramma.TextAlign = HorizontalAlignment.Center;
                flvwCicliBatteria.AllColumns.Add(colIdProgramma);

                BrightIdeasSoftware.OLVColumn Colonna16 = new BrightIdeasSoftware.OLVColumn();
                Colonna16.Text = "Registrazioni";
                Colonna16.Sortable = false;
                Colonna16.AspectName = "strNumEventiBrevi";
                Colonna16.Width = 60;
                Colonna16.HeaderTextAlign = HorizontalAlignment.Center;
                Colonna16.TextAlign = HorizontalAlignment.Center;
                Colonna16.IsVisible = _colonnaNascosta;

                flvwCicliBatteria.AllColumns.Add(Colonna16);

                BrightIdeasSoftware.OLVColumn Colonna17 = new BrightIdeasSoftware.OLVColumn();
                Colonna17.Text = "Registrazioni";
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
                    MostraDettaglioRiga();
                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }

        }

        private void flvwCicliBatteria_ItemActivate(object sender, EventArgs e)
        {

            Log.Debug("flvwCicliBatteria_ItemActivate " + sender.ToString());

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
            salvaCliente();
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
            bool _esito;
            this.Cursor = Cursors.WaitCursor;
            _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
            MostraVariabili(_esito, (chkDatiDiretti.Checked == true));
            this.Cursor = Cursors.Default;
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

        private void cmdRicaricaDati_Click(object sender, EventArgs e)
        {
            try
            {

                if ((_sb.sbData.LongMem - _sb.CicliMemoriaLunga.Count) < 25 )
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
                else
                {


                    bool _esito;
                    this.Cursor = Cursors.WaitCursor;
                    CaricaTestata(_sb.Id, _logiche, _apparatoPresente);
                    if (chkEraseDB.Checked == true)
                        _esito = _sb.sbData.cancellaDati(_sb.Id);
                    ;
                    DumpInteraMemoria(chkAckDumpMem.Checked == true);
                    this.Cursor = Cursors.Default;
                }
            }
            catch
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
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSpyBat_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
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
        /// Scarica L'intera immagine memoria, aprendo la finastra pop-up di avanzamento
        /// </summary>
        /// <param name="InviaACK">Se TRUE attiva l'invio dell'ACK in risposta ad ogni pacchetto</param>
        private void DumpInteraMemoria(bool InviaACK = false)
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


 


        private void tabCaricaBatterie_Selected(object sender, TabControlEventArgs e)
        {
            try
            {

                if (e.TabPage != tabSbFact)
                {
                    chkParLetturaAuto.Checked = false;
                }
                if (e.TabPage == tabCb05)
                {
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
                // if (e.TabPage == tabStatistiche)
                //    frmSpyBat_Resize(null, null);


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

                        if ((_px > cella.X0 ) & (_px < cella.X1) & (_py > cella.Y0) & (_py < cella.Y1))
                        {
                            UInt32 _TempId;
                            if (UInt32.TryParse(cella.Title,out _TempId))
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
            bool _esito;
            this.Cursor = Cursors.WaitCursor;
            if (_apparatoPresente) _esito = _sb.AttivaProgramma();
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
            
        }

        private void tabCb05_Click(object sender, EventArgs e)
        {

        }

        private void btnStatProvaInserimento_Click(object sender, EventArgs e)
        {
            // CaricaSchedeGrafico(_stat);
            Log.Info("------------------------------------------------");
            for (int _step = 0; _step < 101; _step++)
            {
                int _stepCarica = (100 - _step) / 10;
                int _stepScarica = ( _step) / 10;

                Log.Info("Step " + _step.ToString("000") + ": " + _stepCarica.ToString() + " / " + _stepScarica.ToString());

            }
            Log.Info("------------------------------------------------");

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
            try
            {
                bool _esito;
                DialogResult risposta = MessageBox.Show("Vuoi veramente azzerare la memoria ?\nATTENZIONE: l'operazione cancellerà anche i dati in archivio",
                "Cancellazione Memoria",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

                if (risposta == System.Windows.Forms.DialogResult.Yes)
                {
                    _esito = _sb.CancellaInteraMemoria();
                    if (_esito) MessageBox.Show("Memoria Azzerata", "Cancellazione Memoria", MessageBoxButtons.OK);

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
                    if (_esito) txtCalValoreParametro.Text = "Cal " + _idParametro.ToString() +  " Ok";

                }
            }
        }

        public void InizializzaSchedaConfronti()
        {

            creaContainerOxy(ref grCompSOH, new Point(100, 50), new Size(500, 280), "SoH");
            creaGraficoStimaOxy(ref grCompSOH, "SoH","% Residua", 80, 70, 60, 0, 100);

            int _ciclisimulati = _stat.NumeroCariche * 10;

            creaContainerOxy(ref grCompRabb, new Point(650, 50), new Size(500, 280), "Rabbocchi");
            creaGraficoStimaOxy(ref grCompRabb, "Numero di Rabbocchi", "Numero", (int)(_ciclisimulati/30), (int)(_ciclisimulati / 7.5), (int)(_ciclisimulati / 6), 0, (int)(_ciclisimulati / 5));
            
            creaContainerOxy(ref grCompEnConsumata, new Point(100, 360), new Size(500, 280), "Energia");
            creaGraficoStimaOxy(ref grCompEnConsumata, "Energia Consumata","KWh", (_stat.KWhCaricati/0.9), (_stat.KWhCaricati/0.814), (_stat.KWhCaricati/0.745), _stat.KWhCaricati, (_stat.KWhCaricati*1.5));

            double KgCo2 = _stat.KWhCaricati*0.53705;

            creaContainerOxy(ref grCompCO2, new Point(650, 360), new Size(500, 280), "Anidride");
            creaGraficoStimaOxy(ref grCompCO2, "CO2 Risparmiata","Kg CO2", KgCo2 * 0.2311, KgCo2*0.1137,0, 0, KgCo2/2);


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
                //tabStatComparazioni.Controls.Add(oxyContainer);
            }

            catch
            {

            }
        }


        void creaGraficoStimaOxy(ref OxyPlot.PlotModel Grafico, string TitoloGrafico,string TitoloMisura, double ValLL, double ValPSW, double ValEDM, double ValMin, double ValMax)
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
                    AsseCat.Title = "Tipologia Caricabatteria";



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
                    
                   
                    OxyPlot.Series.BarSeries ColValore = new OxyPlot.Series.BarSeries();
                    //ColValore.StrokeThickness = 1;
                    ColValore.Title = "ColumnSeries";
                    ColValore.FillColor = OxyPlot.OxyColors.Blue;


                    ValoriPuntualiGrafico.Clear();
                    OxyPlot.Series.BarItem colonna;
                    //----------------------------------- LL -------------
                    AsseCat.Labels.Add("LL");
                    colonna = new OxyPlot.Series.BarItem();
                    colonna.Value = ValLL;
                    colonna.CategoryIndex = -1;
                    colonna.Color = OxyPlot.OxyColors.Gold;
                    ColValore.Items.Add(colonna);

                    //----------------------------------- PSW -------------
                    AsseCat.Labels.Add("PSW");
                    colonna = new OxyPlot.Series.BarItem();
                    colonna.Value = ValPSW;
                    colonna.CategoryIndex = -1;
                    colonna.Color = OxyPlot.OxyColors.Blue;
                    ColValore.Items.Add(colonna);

                    //----------------------------------- EDM -------------
                    AsseCat.Labels.Add("EDM");
                    colonna = new OxyPlot.Series.BarItem();
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

            catch
            {

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
                            double _coeffTermico = _evento.Wh * FunzioniAnalisi.FattoreTermicoSOH(_tmed)/100;
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




        private void btnFWFileCCSsearch_Click(object sender, EventArgs e)
        {
            sfdImportDati.Filter = "CCS Generated File (*.txt)|*.txt|All files (*.*)|*.*";
            sfdImportDati.ShowDialog();
            txtFwFileCCS.Text = sfdImportDati.FileName;

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

        }

        private void btnFWFileSBFReadSearch_Click(object sender, EventArgs e)
        {
            sfdImportDati.Filter = "SBF SPY-BATT Firmware File (*.sbf)|*.sbf|All files (*.*)|*.*";
            sfdImportDati.ShowDialog();
            txtFWFileSBFrd.Text = sfdImportDati.FileName;
        }

        private void btnFWFileSBFLoad_Click(object sender, EventArgs e)
        {
            CaricafileSBF();
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
                //bool _esito;
                this.Cursor = Cursors.WaitCursor;
                AggiornaFirmware();
                this.Cursor = Cursors.Default;
            }
            catch
            {
                this.Cursor = Cursors.Default;

            }
        }

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
                        if(!_esito)
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

        private void btnFwSwitchArea_Click(object sender, EventArgs e)
        {
            try
            {
                if(rbtFwArea1.Checked)
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
                    _esito = _sb.ResetScheda();
                    //if (_esito) MessageBox.Show("Memoria Azzerata", "Cancellazione Memoria", MessageBoxButtons.OK);

                }
            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemClear_Click: " + Ex.Message);
            }
        }


        private void label7_Click(object sender, EventArgs e)
        {

        }


        private void btnFwCheckArea_Click(object sender, EventArgs e)
        {

        }

        private void btnMemCicliCercaFileWr_Click(object sender, EventArgs e)
        {
            sfdExportDati.Filter = "RCF Regeneraton Cicles File (*.sbf)|*.rcf|All files (*.*)|*.*";
            sfdExportDati.ShowDialog();
            txtMemCicliNomeFile.Text = sfdExportDati.FileName;
        }

        private void btnMemCicliCercaFileRd_Click(object sender, EventArgs e)
        {
            sfdImportDati.Filter = "RCF Regeneraton Cicles File (*.sbf)|*.rcf|All files (*.*)|*.*";
            sfdImportDati.ShowDialog();
            txtMemCicliNomeFile.Text = sfdImportDati.FileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMemCicliSalvaImmagine_Click(object sender, EventArgs e)
        {
            try
            {
                
                uint _StartAddr;
                uint _TmpAddr;
                ushort _NumByte;
                ushort _NumSequenze;
                bool _esito;
                byte[] _dataArray;
                int _numBytes;
                int _cicliCompleti;
                int _byteResidui;
                byte[] _tempBuffer;

                if (chkMemCicliStartAddHex.Checked)
                {
                    if (uint.TryParse(txtMemCicliStartAddr.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true) return;
                }
                else
                {

                    if (uint.TryParse(txtMemCicliStartAddr.Text, out _StartAddr) != true) return;
                }

                if(txtMemCicliNomeFile.Text == "")
                {
                    MessageBox.Show("Inserire un nome file  di blocchi valido", "Esportazione dati", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                if (ushort.TryParse(txtMemCicliNumBlocchi.Text, out _NumSequenze) != true) return;
                if (_NumSequenze < 1) _NumSequenze = 1;
                if (_NumSequenze >100 ) _NumSequenze = 100;

                _numBytes = _NumSequenze * 0x1000;

                _dataArray = new byte[_numBytes];

                _cicliCompleti = _numBytes / DataBlock;
                _byteResidui = _numBytes % DataBlock;

                lblMemCicliStatoOp.Text = _cicliCompleti.ToString() + " pacchetti + " + _byteResidui.ToString() + " bytes";

                // Leggo prima i pacchetti interi
                _tempBuffer = new byte[DataBlock];

                for (int _step = 0; _step < _cicliCompleti; _step++)
                {
                    _TmpAddr = (uint)(_step * DataBlock);
                    _esito = _sb.LeggiBloccoMemoria(_StartAddr+ _TmpAddr, DataBlock, out _tempBuffer);
                    if (_esito)
                    {
                        // Pacchetto letto con successo, accodo i dati
                        for(int _ii = 0; _ii< DataBlock; _ii++)
                        {
                            _dataArray[_TmpAddr + _ii] = _tempBuffer[_ii];
                        }
                        lblMemCicliStatoOp.Text = _step.ToString() + " di " + _cicliCompleti.ToString();
                        Application.DoEvents();

                    }
                    else
                    {
                        MessageBox.Show("Errore lettura pacchetto " + _step.ToString(), "Esportazione dati", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }



                // Ora Leggo il residuo
                if (_byteResidui > 0)
                {
                    _tempBuffer = new byte[_byteResidui];


                    _TmpAddr = (uint)(_cicliCompleti * DataBlock);
                    _esito = _sb.LeggiBloccoMemoria(_StartAddr + _TmpAddr, (ushort)_byteResidui, out _tempBuffer);
                    if (_esito)
                    {
                        // Pacchetto letto con successo, accodo i dati
                        for (int _ii = 0; _ii < _byteResidui; _ii++)
                        {
                            _dataArray[_TmpAddr + _ii] = _tempBuffer[_ii];
                        }
                        lblMemCicliStatoOp.Text = "Pacchetto Finale";
                        Application.DoEvents();

                    }
                    else
                    {
                        MessageBox.Show("Errore lettura pacchetto finale ", "Esportazione dati", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                  
                }
                Log.Debug("--- Carica Immagine -------------------");
                Log.Debug(FunzioniMR.hexdumpArray(_dataArray));
                //ora salvo l'immagine
            
                File.WriteAllBytes(txtMemCicliNomeFile.Text, _dataArray);
                lblMemCicliStatoOp.Text = "File Generato";
                Application.DoEvents();

                return;

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }


        }

        private void btnMemCicliCaricaImmagine_Click(object sender, EventArgs e)
        {

            try
            {

                uint _StartAddr;
                //uint _TmpAddr;
                //ushort _NumByte;
                ushort _NumSequenze;
                //bool _esito;
                byte[] _dataArray;
                int _numBytes;
                int _cicliCompleti;
                int _byteResidui;
                byte[] _tempBuffer;

                if (chkMemCicliStartAddHex.Checked)
                {
                    if (uint.TryParse(txtMemCicliStartAddr.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true) return;
                }
                else
                {

                    if (uint.TryParse(txtMemCicliStartAddr.Text, out _StartAddr) != true) return;
                }

                if (txtMemCicliNomeFile.Text == "")
                {
                    MessageBox.Show("Inserire un nome file valido", "Importazione dati", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DataBuffer = File.ReadAllBytes(txtMemCicliNomeFile.Text);

                _numBytes = DataBuffer.Length;
                _NumSequenze = (ushort)(_numBytes / 0x1000 );
                _byteResidui = _numBytes % 0x1000;


                if(_byteResidui != 0 )
                {
                    //formato non valido
                    MessageBox.Show("Formato file non valido", "Importazione dati", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                lblMemCicliStatoOp.Text = _NumSequenze.ToString() + " Sequenze caricate";
                Application.DoEvents();

                txtMemCicliNumBlocchi.Text = _NumSequenze.ToString();
                return;

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }


        }

        private void btnMemCicliScriviImmagine_Click(object sender, EventArgs e)
        {


            try
            {

                uint _StartAddr;
                uint _TmpAddr;
                ushort _NumByte;
                ushort _NumSequenze;
                bool _esito;
                byte[] _dataArray;
                int _numBytes;
                int _cicliCompleti;
                int _byteResidui;
                byte[] _tempBuffer;

                if (chkMemCicliStartAddHex.Checked)
                {
                    if (uint.TryParse(txtMemCicliStartAddr.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true) return;
                }
                else
                {

                    if (uint.TryParse(txtMemCicliStartAddr.Text, out _StartAddr) != true) return;
                }

                DataBuffer = File.ReadAllBytes(txtMemCicliNomeFile.Text);

                _numBytes = DataBuffer.Length;
                _NumSequenze = (ushort)(_numBytes / 0x1000);
                _byteResidui = _numBytes % 0x1000;


                if (_byteResidui != 0)
                {
                    //formato non valido
                    MessageBox.Show("Formato dati non valido", "Importazione dati", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


                // Prima vuoto la memoria

                if (_NumSequenze > 0)
                {
                    int _bloccoCorrente;
                    _TmpAddr = _StartAddr;
                    for (int _cicloBlocchi = 0; _cicloBlocchi < _NumSequenze; _cicloBlocchi++)
                    {
                        _bloccoCorrente = _cicloBlocchi + 1;
                        _esito = _sb.CancellaBlocco4K(_TmpAddr);
                        if (!_esito)
                        {
                            MessageBox.Show("Cancellazione del blocco " + _bloccoCorrente.ToString() + " non riuscita", "Cancellazione dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            _TmpAddr += 0x1000;
                            lblMemCicliStatoOp.Text = "Cancellazione Sequenza " + _bloccoCorrente.ToString();

                            Application.DoEvents();
                        }
                    }

                }
                else
                {
                    return;
                }


                uint _stepSent = 0;
                uint _posizione = 0;
                uint _datiTrasferiti = 0;
                byte[] _Tempbuffer = new byte[DataBlock];
                _TmpAddr = _StartAddr;
                while ((_numBytes - _datiTrasferiti) > DataBlock)
                {

                    for (int _blockStep = 0; _blockStep < DataBlock; _blockStep++)
                    {
                        _Tempbuffer[_blockStep] = DataBuffer[_posizione];
                        _posizione++;
                        _datiTrasferiti++;
                    }
                    _esito = _sb.ScriviBloccoMemoria(_TmpAddr, (ushort)DataBlock, _Tempbuffer);

                    _TmpAddr += DataBlock;
                    _stepSent++;
                    lblMemCicliStatoOp.Text = "Pacchetto " + _stepSent.ToString();
                    Application.DoEvents();

                }


                // Ora trasmetto il residuo
                int _residuo = (int)(_numBytes - _datiTrasferiti);
                _Tempbuffer = new byte[_residuo];
                for (int _blockStep = 0; _blockStep < _residuo; _blockStep++)
                {
                    _Tempbuffer[_blockStep] = DataBuffer[_posizione];
                    _posizione++;
                    _datiTrasferiti++;
                }
                _esito = _sb.ScriviBloccoMemoria(_TmpAddr, (ushort)_residuo, _Tempbuffer);
                _stepSent++;

                lblMemCicliStatoOp.Text = "Pacchetto " + _stepSent.ToString();
                Application.DoEvents();


                lblMemCicliStatoOp.Text = _NumSequenze.ToString() + " Sequenze inserite";
                Application.DoEvents();

                return;

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }








        }

        private void optMemSequenze_CheckedChanged(object sender, EventArgs e)
        {
            SetCloneBaseAddress();
        }

        private void SetCloneBaseAddress()
        {
            if (optMemProcedure.Checked)
            {
                txtMemCicliStartAddr.Text = "12E000";
            }
            if (optMemSequenze.Checked)
            {
                txtMemCicliStartAddr.Text = "2000";
            }
        }

        private void btnMemRegenGeneraImmagine_Click(object sender, EventArgs e)
        {
            try
            {
                // 1 : vuoto l'immagine
                ImmagineDeso = new FileSetupRigeneratore();
                txtMemRegenNumBlocchi.Text = "0";
                lblMemRegenAvanzamentoRead.Visible = true;
                Application.DoEvents();
                AreaDatiRegen BloccoDati;

                // Step 1:sequenze da 1 a 60; addr 0x2000
                BloccoDati = new AreaDatiRegen();
                BloccoDati.IdBlocco = 1;
                BloccoDati.Tipo = AreaDatiRegen.TipoArea.Sequenze;
                BloccoDati.StartAddress = 0x2000;
                BloccoDati.NumBlocchi = 60;

                if(!CaricaBloccoDati(BloccoDati))
                {
                    return;
                }
                ImmagineDeso.ListaBlocchi.Add(BloccoDati);

                // Step 2:sequenze da 100 a 119; addr 0x65000
                BloccoDati = new AreaDatiRegen();
                BloccoDati.IdBlocco = 2;
                BloccoDati.Tipo = AreaDatiRegen.TipoArea.Sequenze;
                BloccoDati.StartAddress = 0x65000;
                BloccoDati.NumBlocchi = 20;

                if (!CaricaBloccoDati(BloccoDati))
                {
                    return;
                }
                ImmagineDeso.ListaBlocchi.Add(BloccoDati);


                // Step 3:procedura 1; addr 0x12C000
                BloccoDati = new AreaDatiRegen();
                BloccoDati.IdBlocco = 3;
                BloccoDati.Tipo = AreaDatiRegen.TipoArea.Sequenze;
                BloccoDati.StartAddress = 0x12C000;
                BloccoDati.NumBlocchi = 1;

                if (!CaricaBloccoDati(BloccoDati))
                {
                    return;
                }
                ImmagineDeso.ListaBlocchi.Add(BloccoDati);


                // Step 4:procedure 2; addr 0x18F000
                BloccoDati = new AreaDatiRegen();
                BloccoDati.IdBlocco = 4;
                BloccoDati.Tipo = AreaDatiRegen.TipoArea.Sequenze;
                BloccoDati.StartAddress = 0x18F000;
                BloccoDati.NumBlocchi = 2;

                if (!CaricaBloccoDati(BloccoDati))
                {
                    return;
                }
                ImmagineDeso.ListaBlocchi.Add(BloccoDati);


                lblMemRegenAvanzamentoRead.Visible = false;
                btnMemRegenSalvaImmegine.Enabled = true;
                InizializzaVistaBlocchi();

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }


        }

        private void btnMemRegenScriviImmDisp_Click(object sender, EventArgs e)
        {
            try
            {
                if (ImmagineDeso == null) return;

                if (ImmagineDeso.ListaBlocchi.Count < 1) return;

                foreach(AreaDatiRegen BloccoDati in ImmagineDeso.ListaBlocchi)
                {
                    bool esito = ScriviBloccoDati(BloccoDati);
                    if (!esito)
                    {
                        MessageBox.Show("Scrittura Area " + BloccoDati.IdBlocco.ToString() + " non riuscita", "Scrittura dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                MessageBox.Show("Scrittura Memoria Sequenze/Procedure completata", "Scrittura dati ", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }
        }

        private bool InizializzaVistaBlocchi()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);
                Font _carTesto = new Font("Tahoma", 10, FontStyle.Regular);

                lvwPackContenuto.HeaderUsesThemes = false;
                lvwPackContenuto.HeaderFormatStyle = _stile;
                lvwPackContenuto.UseAlternatingBackColors = true;
                lvwPackContenuto.AlternateRowBackColor = Color.LightGoldenrodYellow;

                lvwPackContenuto.AllColumns.Clear();

                lvwPackContenuto.View = View.Details;
                lvwPackContenuto.ShowGroups = false;
                lvwPackContenuto.GridLines = true;
                lvwPackContenuto.Font = _carTesto;
                lvwPackContenuto.RowHeight = 25;
                lvwPackContenuto.FullRowSelect = true;

                BrightIdeasSoftware.OLVColumn colIdBlocco = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "ID",
                    AspectName = "strIdBlocco",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                lvwPackContenuto.AllColumns.Add(colIdBlocco);

                BrightIdeasSoftware.OLVColumn colTipo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Tipo",
                    AspectName = "strTipoArea",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                lvwPackContenuto.AllColumns.Add(colTipo);

                BrightIdeasSoftware.OLVColumn colBlocchi = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Blocchi",
                    AspectName = "strNumBlocchi",
                    Width = 80,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                lvwPackContenuto.AllColumns.Add(colBlocchi);

                BrightIdeasSoftware.OLVColumn colAddress = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Start",
                    AspectName = "strStartAddress",
                    Width = 120,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                lvwPackContenuto.AllColumns.Add(colAddress);

                BrightIdeasSoftware.OLVColumn colSize = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Dim",
                    AspectName = "strDataSize",
                    Width = 120,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                lvwPackContenuto.AllColumns.Add(colSize);

                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                lvwPackContenuto.AllColumns.Add(colRowFiller);

                lvwPackContenuto.Sort(colAddress);
                lvwPackContenuto.RebuildColumns();
                lvwPackContenuto.SetObjects(ImmagineDeso.ListaBlocchi);
                lvwPackContenuto.BuildList();




                return true;
            }
            catch
            {
                return false;
            }
        }


        private bool CaricaBloccoDati(AreaDatiRegen BloccoAttivo)
        {
            try
            {

                uint _StartAddr = BloccoAttivo.StartAddress;
                uint _TmpAddr;
                ushort _NumSequenze = (ushort)BloccoAttivo.NumBlocchi;
                bool _esito;
                byte[] _dataArray;
                int _numBytes;
                int _cicliCompleti;
                int _byteResidui;
                byte[] _tempBuffer;

                txtMemRegenNumBlocchi.Text = BloccoAttivo.IdBlocco.ToString();

                _numBytes = _NumSequenze * 0x1000;

                _dataArray = new byte[_numBytes];
                BloccoAttivo.Data = new byte[_numBytes];

                _cicliCompleti = _numBytes / DataBlock;
                _byteResidui = _numBytes % DataBlock;

                lblMemRegenAvanzamentoRead.Text = _cicliCompleti.ToString() + " pacchetti + " + _byteResidui.ToString() + " bytes";

                // Leggo prima i pacchetti interi
                _tempBuffer = new byte[DataBlock];

                for (int _step = 0; _step < _cicliCompleti; _step++)
                {
                    _TmpAddr = (uint)(_step * DataBlock);
                    _esito = _sb.LeggiBloccoMemoria(_StartAddr + _TmpAddr, DataBlock, out _tempBuffer);
                    if (_esito)
                    {
                        // Pacchetto letto con successo, accodo i dati
                        for (int _ii = 0; _ii < DataBlock; _ii++)
                        {
                            _dataArray[_TmpAddr + _ii] = _tempBuffer[_ii];
                        }
                        lblMemRegenAvanzamentoRead.Text =  _step.ToString() + " di " + _cicliCompleti.ToString();
                        Application.DoEvents();

                    }
                    else
                    {
                        MessageBox.Show("Errore lettura pacchetto " + _step.ToString(), "Esportazione dati", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }



                // Ora Leggo il residuo
                if (_byteResidui > 0)
                {
                    _tempBuffer = new byte[_byteResidui];


                    _TmpAddr = (uint)(_cicliCompleti * DataBlock);
                    _esito = _sb.LeggiBloccoMemoria(_StartAddr + _TmpAddr, (ushort)_byteResidui, out _tempBuffer);
                    if (_esito)
                    {
                        // Pacchetto letto con successo, accodo i dati
                        for (int _ii = 0; _ii < _byteResidui; _ii++)
                        {
                            _dataArray[_TmpAddr + _ii] = _tempBuffer[_ii];
                        }
                        lblMemRegenAvanzamentoRead.Text = "Pacchetto Finale";
                        Application.DoEvents();

                    }
                    else
                    {
                        MessageBox.Show("Errore lettura pacchetto finale ", "Esportazione dati", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }


                }

                Log.Debug("--- Carica Immagine -------------------");
                Log.Debug(FunzioniMR.hexdumpArray(_dataArray));
                //ora salvo l'immagine
                BloccoAttivo.Data = _dataArray;
                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
                return false;
            }


        }


        private bool ScriviBloccoDati(AreaDatiRegen BloccoAttivo)
        {


            try
            {

                uint _StartAddr = BloccoAttivo.StartAddress;
                uint _TmpAddr;
                ushort _NumByte;
                ushort _NumSequenze = (ushort)BloccoAttivo.NumBlocchi;
                bool _esito;
                byte[] _dataArray;
                int _numBytes;
                int _cicliCompleti;
                int _byteResidui;
                byte[] _tempBuffer;
                lblMemRegenAvanzamentoWrite.Visible = true;
                txtMemRegenNumBlocchiWR.Text = BloccoAttivo.IdBlocco.ToString();
                // Prima vuoto la memoria

                _numBytes = _NumSequenze * 0x1000;

                if (_NumSequenze > 0)
                {
                    int _bloccoCorrente;
                    _TmpAddr = _StartAddr;
                    for (int _cicloBlocchi = 0; _cicloBlocchi < _NumSequenze; _cicloBlocchi++)
                    {
                        _bloccoCorrente = _cicloBlocchi + 1;
                        _esito = _sb.CancellaBlocco4K(_TmpAddr);
                        if (!_esito)
                        {
                            MessageBox.Show("Cancellazione del blocco " + _bloccoCorrente.ToString() + " non riuscita", "Cancellazione dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                        {
                            _TmpAddr += 0x1000;
                            lblMemRegenAvanzamentoWrite.Text = "Cancellazione pagina " + _bloccoCorrente.ToString();

                            Application.DoEvents();
                        }
                    }

                }
                else
                {
                    return false;
                }

                DataBuffer = BloccoAttivo.Data;


                uint _stepSent = 0;
                uint _posizione = 0;
                uint _datiTrasferiti = 0;
                byte[] _Tempbuffer = new byte[DataBlock];
                _TmpAddr = _StartAddr;
                while ((_numBytes - _datiTrasferiti) > DataBlock)
                {

                    for (int _blockStep = 0; _blockStep < DataBlock; _blockStep++)
                    {
                        _Tempbuffer[_blockStep] = DataBuffer[_posizione];
                        _posizione++;
                        _datiTrasferiti++;
                    }
                    _esito = _sb.ScriviBloccoMemoria(_TmpAddr, (ushort)DataBlock, _Tempbuffer);

                    _TmpAddr += DataBlock;
                    _stepSent++;
                    lblMemRegenAvanzamentoWrite.Text = "Pacchetto " + _stepSent.ToString();
                    Application.DoEvents();

                }


                // Ora trasmetto il residuo
                int _residuo = (int)(_numBytes - _datiTrasferiti);
                _Tempbuffer = new byte[_residuo];
                for (int _blockStep = 0; _blockStep < _residuo; _blockStep++)
                {
                    _Tempbuffer[_blockStep] = DataBuffer[_posizione];
                    _posizione++;
                    _datiTrasferiti++;
                }
                _esito = _sb.ScriviBloccoMemoria(_TmpAddr, (ushort)_residuo, _Tempbuffer);
                _stepSent++;

                lblMemRegenAvanzamentoWrite.Text = "Pacchetto " + _stepSent.ToString();
                Application.DoEvents();


                lblMemRegenAvanzamentoWrite.Text = _NumSequenze.ToString() + " pagine inserite";
                lblMemRegenAvanzamentoWrite.Visible = true;
                Application.DoEvents();

                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
                return false;
            }

        }



        private void btnMemRegenCercaFileWr_Click(object sender, EventArgs e)
        {
            sfdExportDati.Filter = "RSF Regeneraton Setup File (*.rsf)|*.rsf|All files (*.*)|*.*";
            DialogResult esito =  sfdExportDati.ShowDialog();
            if (esito == DialogResult.OK)
            {
                txtMemRegenNomeFile.Text = sfdExportDati.FileName;
                btnMemRegenSalvaImmegine_Click(sender,e);
                //MessageBox.Show("File generato", "Creazione file dati ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            txtMemRegenNomeFile.Text = sfdExportDati.FileName;
        }

        private void btnMemRegenCercaFileRd_Click(object sender, EventArgs e)
        {
            sfdImportDati.Filter = "RSF Regenerator Setup File (*.rsf)|*.rsf|All files (*.*)|*.*";
            DialogResult esito = sfdImportDati.ShowDialog();
            if (esito == DialogResult.OK)
            {
                txtMemRegenNomeFile.Text = sfdImportDati.FileName;
                btnMemRegenCaricaImmagine_Click(sender, e);
                MessageBox.Show("File caricato", "Lettura file dati ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnMemRegenSalvaImmegine_Click(object sender, EventArgs e)
        {
            try
            {

                string filePath = txtMemRegenNomeFile.Text;

                if (filePath == "") return;

                if (!File.Exists(filePath)) File.Create(filePath).Close();
                Log.Debug("file prepara esportazione");

                if(txtPackDataCreazione.Text !="")
                {
                    DateTime TempD;
                    bool isValid = DateTime.TryParseExact(txtPackDataCreazione.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out TempD);
                    if (isValid)
                    {
                        ImmagineDeso.DataCreazionePacchetto = TempD;
                    }
                    else
                    {
                        MessageBox.Show("Inserire una data valida", "Scrittura file dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                ImmagineDeso.Note = txtPackNote.Text;
                ImmagineDeso.FwMin = txtPackFwMin.Text;
                ImmagineDeso.FwMax = txtPackFwMax.Text;
                ImmagineDeso.Release = txtPackRelease.Text;

                string JsonData = JsonConvert.SerializeObject(ImmagineDeso);
                Log.Debug("file generato");
                // Prima comprimo i dati

                if(chkPackComprimiFile.Checked)
                {
                    Log.Debug("file generato");
                    // Prima comprimo i dati
                    string JsonZip = FunzioniComuni.CompressString(JsonData);

                    // Ora cifro i dati
                    // string JsonEncript = StringCipher.Encrypt(JsonZip);
                    // PER INCOMPATIBILITA' DEL METODO DI CIFRATUTA CON NET CORE (XAMARIN)
                    // RIMOSSA LA CIFRATURA DEL PACCHETTO  (16/07/2020)
                    // JsonEncript = JsonData;
                    // Log.Debug("file criptato");

                    JsonData = JsonZip;
                
                }

                // string JsonZip = FunzioniComuni.CompressString(JsonData);

                File.WriteAllText(filePath, JsonData);
                MessageBox.Show("File salvato", "Lettura file dati ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Log.Debug("file salvato");
            }
            catch (Exception Ex)
            {
                Log.Error("btnMemRegenSalvaImmegine_Click: " + Ex.Message);
            }

        }

        private void btnMemRegenCaricaImmagine_Click(object sender, EventArgs e)
        {
            try
            {
                btnMemRegenScriviImmDisp.Enabled = false;
                txtMemRegenNumBlocchiWR.Text = "";

                string _fileDecripted = "";
                string _fileDecompress = "";

                string filePath = txtMemRegenNomeFile.Text;
                if (File.Exists(filePath))
                {
                    string _infile = File.ReadAllText(filePath);
                    // Verifico se è criptato
                    _fileDecripted = StringCipher.Decrypt(_infile);
                    if (_fileDecripted != "")
                    {
                        //il file è cifrato
                        Log.Debug("file criptato");
                        _infile = _fileDecripted;
                    }

                    // verifico se è compresso
                    _fileDecompress = FunzioniComuni.DecompressString(_infile);
                    if (_fileDecompress != "")
                    {

                        //è compresso
                        _infile = _fileDecompress;

                    }

                    ImmagineDeso = JsonConvert.DeserializeObject<FileSetupRigeneratore>(_infile);
                    Log.Debug("file caricato");
                    if(ImmagineDeso != null)
                    {
                        txtMemRegenNumBlocchiWR.Text = ImmagineDeso.ListaBlocchi.Count.ToString();

                        if (ImmagineDeso.ListaBlocchi.Count >0 )
                        {
                            btnMemRegenScriviImmDisp.Enabled = true;

                        }
                    }
                    btnMemRegenScriviImmDisp.Enabled = true;
                    txtPackDataCreazione.Text = ImmagineDeso.DataCreazionePacchetto.ToString("dd/MM/yyyy");
                    txtPackFwMin.Text = ImmagineDeso.FwMin;
                    txtPackFwMax.Text = ImmagineDeso.FwMax;
                    txtPackRelease.Text = ImmagineDeso.Release;
                    txtPackNote.Text = ImmagineDeso.Note;
                    InizializzaVistaBlocchi();
                    return;
                }
                    

                Log.Debug("Caricamento file fallito");
            }
            catch (Exception Ex)
            {
                Log.Error("btnMemRegenSalvaImmegine_Click: " + Ex.Message);
            }

        }

        private void btnLngRegenCercaFile_Click(object sender, EventArgs e)
        {
            sfdImportDati.Filter = "Excel (98/2007) File (*.xls)|*.xls|Excel (13 ->) File (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            DialogResult esito = sfdImportDati.ShowDialog();
            if (esito == DialogResult.OK)
            {
                txtLngRegenNomeFile.Text = sfdImportDati.FileName;
                //btnMemRegenCaricaImmagine_Click(sender, e);
                //MessageBox.Show("File caricato", "Lettura file dati ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnLngRegenCaricaLingue_Click(object sender, EventArgs e)
        {
            try
            {
                Stream stream = new FileStream(txtLngRegenNomeFile.Text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                cmbLngListaLingue.Items.Clear();

                IExcelDataReader XlsReader = ExcelReaderFactory.CreateReader(stream);

                var openTiming = sw.ElapsedMilliseconds;
                // reader.IsFirstRowAsColumnNames = firstRowNamesCheckBox.Checked;
                lblLngRegenStatoOp.Text = "";

                LanguageDataSet = XlsReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    UseColumnDataType = false,
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                lblLngRegenElapsed.Text = "Elapsed: " + sw.ElapsedMilliseconds.ToString() + " ms (" + openTiming.ToString() + " ms to open)";

                if (LanguageDataSet == null)
                {
                    return;
                }

                fdlvLngSourceFile.DataSource = LanguageDataSet.Tables[0];

                if (LanguageDataSet.Tables[0].Columns.Count < 5 )
                {
                    // formato files non valido
                    return;
                }

                // Carico la combo lingue
                cmbLngListaLingue.Items.Clear();
                for (int _colonna = 4; _colonna < LanguageDataSet.Tables[0].Columns.Count; _colonna++)
                {
                    if(LanguageDataSet.Tables[0].Columns[_colonna].ColumnName == "NOTE")
                    {
                        // note -> lingue finite
                        break;
                    }
                    cmbLngListaLingue.Items.Add(LanguageDataSet.Tables[0].Columns[_colonna].ColumnName);
                }
                cmbLngListaLingue.SelectedIndex = 0;

                lblLngRegenElapsed.Text = "Elapsed: " + sw.ElapsedMilliseconds.ToString() + " ms (" + openTiming.ToString() + " ms to open)";
                btnLngRegenGeneraPacchetto.Enabled = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnLngRegenGeneraPacchetto_Click(object sender, EventArgs e)
        {
            try
            {
                const int StartMatrice = 0x0F00;
                const int StartFinale = 0x0FF0;

                int RigheTrattate = 0;
                int BlocchiCreati = 0;
                int UltimoBlocco = 0;
                int PtrBlocchi = 0;
                int PtrMatrice = 0;
                int LunghezzaTesto = 0;
                int NumeroVariabile = 0;
                int NumeroOversize = 0;


                lblLngRegenStatoOp.Text = "";


                int indice = cmbLngListaLingue.SelectedIndex + 4;
                btnLngSalvaBloccoLingua.Enabled = false;

                if (indice < 0)
                {
                    // lingua non selezionata
                    return;
                }


                // vuoto il buffer
                LngDataBuffer = new byte[0x1000];
                for(int _cnt = 0; _cnt < 0x1000; _cnt ++)
                {
                    LngDataBuffer[_cnt] = 0xFF;
                }

                if(LanguageDataSet == null)
                {
                    return;
                }

                foreach(DataRow Riga in LanguageDataSet.Tables[0].Rows)
                {
                    RigheTrattate += 1;

                    if ( ((string)Riga[0]).Length  < 5 )
                    {
                        // Alla prima riga bianca mi fermo
                        lblLngRegenStatoOp.Text = "Trovata riga bianca";

                        break;
                    }
                    if (((string)Riga[0]).Substring(0,5) != "Lang_")
                    {
                        // Formato non valido
                        lblLngRegenStatoOp.Text = "Trovata riga con etichetta non valida";
                        break;
                    }

                    string valore = (string)Riga[indice];
                    if(valore.Trim() == "")
                    {
                        // traduzione mancante
                        lblLngRegenStatoOp.Text = "Traduzione mancante";
                        break;
                    }

                    byte[] ByteArrayValore = FunzioniComuni.StringToArray(valore, valore.Length);
                    if (!Riga.IsNull(3))
                    {
                        if ((string)Riga[3] == "SI")
                        {
                            LngDataBuffer[PtrBlocchi + LunghezzaTesto] = 0x20;
                            LunghezzaTesto += 1;
                        }
                    }
                    for(int _i = 0; _i< ByteArrayValore.Length; _i++)
                    {
                        LngDataBuffer[PtrBlocchi + LunghezzaTesto] = ByteArrayValore[_i];
                        LunghezzaTesto += 1;
                    }
                    if (!Riga.IsNull(3))
                    {
                        if ((string)Riga[3] == "SI")
                        {
                            LngDataBuffer[PtrBlocchi + LunghezzaTesto] = 0x20;
                            LunghezzaTesto += 1;
                        }
                    }
                    if (!Riga.IsNull(1))
                    {
                        int ValoreCella;
                        if (int.TryParse(Riga[1].ToString(), out ValoreCella))
                        {
                            if (ValoreCella < LunghezzaTesto)
                            {
                                NumeroOversize += 1;
                            }
                        }
                    }

                    LngDataBuffer[StartMatrice + NumeroVariabile] = (byte)LunghezzaTesto;
                    NumeroVariabile += 1;
                    int BlocchiUsati = (byte)LunghezzaTesto / 16;
                    if((LunghezzaTesto % 16) >0)
                    {
                        // Non multiplo esatto di i6, aggiungo 1 blocco per il frammento
                        BlocchiUsati += 1;
                    }
                    PtrBlocchi += BlocchiUsati * 16;
                    LunghezzaTesto = 0;
                    if (LunghezzaTesto > 38)
                    {
                        // traduzione mancante
                        lblLngRegenStatoOp.Text = "Stringa superiore a 38 caratteri";
                        break;
                    }

                    txtLngNumBlocchi.Text = (PtrBlocchi/16).ToString();
                    txtLngNumRighe.Text = NumeroVariabile.ToString();
                    txtLngNumOversize.Text = NumeroOversize.ToString();

                    Application.DoEvents();
                }
                // Ultimo step il numero variabili
                LngDataBuffer[0xFF0] = (byte)NumeroVariabile;
                btnLngSalvaBloccoLingua.Enabled = true;
                if (lblLngRegenStatoOp.Text == "") lblLngRegenStatoOp.Text = "Blocco Generato";
            }
            catch
            {

            }
        }

        private void btnLngRegenCercaFileOut_Click(object sender, EventArgs e)
        {
            sfdExportDati.Filter = "RCF Regeneraton Cicles File (*.rcf)|*.rcf|All files (*.*)|*.*";
            sfdExportDati.ShowDialog();
            txtLngRegenNomeOutFile.Text = sfdExportDati.FileName;
        }

        private void btnLngSalvaBloccoLingua_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLngRegenNomeOutFile.Text != "")
                {
                    lblLngRegenStatoOp.Text = "";

                    Log.Debug("--- Carica Immagine -------------------");
                    Log.Debug(FunzioniMR.hexdumpArray(LngDataBuffer));

                    //ora salvo l'immagine

                    File.WriteAllBytes(txtLngRegenNomeOutFile.Text, LngDataBuffer);
                    lblLngRegenStatoOp.Text = "File Generato";
                    Application.DoEvents();

                    return;
                }

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }


        }

        private void btnLngGeneraPacchettoLingue_Click(object sender, EventArgs e)
        {
            try
            {
                // 1 : vuoto l'immagine
                PacchettoSetupLingue = new FileSetupRigeneratore();
                txtLngBlocchiLingua.Text = PacchettoSetupLingue.ListaBlocchi.Count().ToString();


            }
            catch
            {

            }
        }

        private void btnLngRegenCercaFilePacchetto_Click(object sender, EventArgs e)
        {
            sfdExportDati.Filter = "RSF Regeneraton Setup File (*.rsf)|*.rsf|All files (*.*)|*.*";
            DialogResult esito = sfdExportDati.ShowDialog();
            if (esito == DialogResult.OK)
            {
                txtLngNomeFilePacchetto.Text = sfdExportDati.FileName;
            }
        }

        private void btnLngSalvaPacchettoLingue_Click(object sender, EventArgs e)
        {
            try
            {
                if(PacchettoSetupLingue == null)
                {
                    MessageBox.Show("Pacchetto lingue vuoto\nNulla da salvare", "Creazione file dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (PacchettoSetupLingue.ListaBlocchi.Count < 1 )
                {
                    MessageBox.Show("Pacchetto lingue vuoto\nNulla da salvare", "Creazione file dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                string filePath = txtLngNomeFilePacchetto.Text;

                if (filePath == "") return;

                if (!File.Exists(filePath)) File.Create(filePath).Close();
                Log.Debug("file prepara esportazione");
                string JsonData = JsonConvert.SerializeObject(PacchettoSetupLingue);
                Log.Debug("file generato");
                // Prima comprimo i dati
                // string JsonZip = FunzioniComuni.CompressString(JsonData);

                File.WriteAllText(filePath, JsonData);
                MessageBox.Show("File generato", "Creazione file dati ", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Log.Debug("file salvato");
            }
            catch (Exception Ex)
            {
                Log.Error("btnMemRegenSalvaImmegine_Click: " + Ex.Message);
            }
        }

        private void btnLngSearchNomeInFile_Click(object sender, EventArgs e)
        {
            try
            {
                sfdImportDati.Filter = "RCF Regeneraton Cicles File (*.sbf)|*.rcf|All files (*.*)|*.*";
                sfdImportDati.ShowDialog();
                txtLngRegenNomeInFile.Text = sfdImportDati.FileName;
            }
            catch
            {

            }
        }

        private void btnLngAddLingua_Click(object sender, EventArgs e)
        {
            try
            {
                if (optLngMemoria.Checked)
                {
                    if(LngDataBuffer == null)
                    {
                        MessageBox.Show("Lingua non presente", "Creazione Pacchetto lingue ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (LngDataBuffer.Length != 0x1000)
                    {
                        MessageBox.Show("Lingua non valida", "Creazione Pacchetto lingue ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (cmbLngListaLingue.SelectedIndex <0)
                    {
                        MessageBox.Show("Lingua non selezionata", "Creazione Pacchetto lingue ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    uint IndirizzoAttivo = (uint)(0x126000 + (0x1000 * cmbLngListaLingue.SelectedIndex));

                    foreach(AreaDatiRegen Ad in PacchettoSetupLingue.ListaBlocchi)
                    {
                        if (Ad.StartAddress == IndirizzoAttivo)
                        {
                            MessageBox.Show("Lingua già inserita", "Creazione Pacchetto lingue ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    AreaDatiRegen AreaDati = new AreaDatiRegen();
                    AreaDati.IdBlocco = cmbLngListaLingue.SelectedIndex;
                    AreaDati.NumBlocchi = 1;
                    AreaDati.StartAddress = IndirizzoAttivo;
                    AreaDati.Tipo = AreaDatiRegen.TipoArea.Lingua;
                    AreaDati.Data = LngDataBuffer;
                    PacchettoSetupLingue.ListaBlocchi.Add(AreaDati);
                }

                if (optLngFile.Checked)
                {

                    if (txtLngRegenNomeInFile.Text == "")
                    {
                        MessageBox.Show("Inserire nome file valido", "Creazione Pacchetto lingue ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    LngDataBuffer = File.ReadAllBytes(txtLngRegenNomeInFile.Text);

                    if (LngDataBuffer.Length != 0x1000)
                    {
                        //formato non valido
                        MessageBox.Show("Formato file non valido", "Importazione dati", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }


                    if (LngDataBuffer == null)
                    {
                        MessageBox.Show("Lingua non presente", "Creazione Pacchetto lingue ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (LngDataBuffer.Length != 0x1000)
                    {
                        MessageBox.Show("Lingua non valida", "Creazione Pacchetto lingue ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (cmbLngListaBlocchi.SelectedIndex < 0)
                    {
                        MessageBox.Show("Lingua non selezionata", "Creazione Pacchetto lingue ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    uint IndirizzoAttivo = (uint)(0x126000 + (0x1000 * cmbLngListaBlocchi.SelectedIndex));

                    foreach (AreaDatiRegen Ad in PacchettoSetupLingue.ListaBlocchi)
                    {
                        if (Ad.StartAddress == IndirizzoAttivo)
                        {
                            MessageBox.Show("Lingua già inserita", "Creazione Pacchetto lingue ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    AreaDatiRegen AreaDati = new AreaDatiRegen();
                    AreaDati.IdBlocco = cmbLngListaLingue.SelectedIndex;
                    AreaDati.NumBlocchi = 1;
                    AreaDati.StartAddress = IndirizzoAttivo;
                    AreaDati.Tipo = AreaDatiRegen.TipoArea.Lingua;
                    AreaDati.Data = LngDataBuffer;
                    PacchettoSetupLingue.ListaBlocchi.Add(AreaDati);
                }

                txtLngBlocchiLingua.Text = PacchettoSetupLingue.ListaBlocchi.Count().ToString();


            }
            catch
            {

            }
        }

        private void grbClonaRigeneratore_Enter(object sender, EventArgs e)
        {

        }

        private void btnMemRegenAccodaImmagine_Click(object sender, EventArgs e)
        {
            try
            {

                string _fileDecripted = "";
                string _fileDecompress = "";
                FileSetupRigeneratore tempImmagine;

                string filePath = txtMemRegenNomeFile.Text;
                if (File.Exists(filePath))
                {
                    string _infile = File.ReadAllText(filePath);
                    // Verifico se è criptato
                    _fileDecripted = StringCipher.Decrypt(_infile);
                    if (_fileDecripted != "")
                    {
                        //il file è cifrato
                        Log.Debug("file criptato");
                        _infile = _fileDecripted;
                    }

                    // verifico se è compresso
                    _fileDecompress = FunzioniComuni.DecompressString(_infile);
                    if (_fileDecompress != "")
                    {

                        //è compresso
                        _infile = _fileDecompress;

                    }

                    tempImmagine = JsonConvert.DeserializeObject<FileSetupRigeneratore>(_infile);
                    Log.Debug("file caricato");


                    if (tempImmagine != null)
                    {
                        if (ImmagineDeso == null)
                        {
                            ImmagineDeso = tempImmagine;
                        }
                        else
                        {
                            foreach(AreaDatiRegen blocco in tempImmagine.ListaBlocchi)
                            {
                                ImmagineDeso.ListaBlocchi.Add(blocco);
                            }
                        }
                        txtMemRegenNumBlocchiWR.Text = ImmagineDeso.ListaBlocchi.Count.ToString();

                        if (ImmagineDeso.ListaBlocchi.Count > 0)
                        {
                            btnMemRegenScriviImmDisp.Enabled = true;
                        }

                        btnMemRegenScriviImmDisp.Enabled = true;
                        txtPackDataCreazione.Text = ImmagineDeso.DataCreazionePacchetto.ToString("dd/MM/yyyy");
                        txtPackFwMin.Text = ImmagineDeso.FwMin;
                        txtPackFwMax.Text = ImmagineDeso.FwMax;
                        txtPackRelease.Text = ImmagineDeso.Release;
                        txtPackNote.Text = ImmagineDeso.Note;
                    }
                    InizializzaVistaBlocchi();
                    return;
                }


                Log.Debug("Caricamento file fallito");
            }
            catch (Exception Ex)
            {
                Log.Error("btnMemRegenSalvaImmegine_Click: " + Ex.Message);
            }
        }

        private void btnMemRegenCercaFileAppend_Click(object sender, EventArgs e)
        {
            sfdImportDati.Filter = "RSF Regeneraton Cicles File (*.rsf)|*.rsf|All files (*.*)|*.*";
            DialogResult esito = sfdImportDati.ShowDialog();
            if (esito == DialogResult.OK)
            {
                txtMemRegenNomeFile.Text = sfdImportDati.FileName;
                //btnMemRegenCaricaImmagine_Click(sender, e);
                //MessageBox.Show("File caricato", "Lettura file dati ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void tbpFirmware_Click(object sender, EventArgs e)
        {

        }

        private void btnInitCaricaStato_Click(object sender, EventArgs e)
        {
            string _tempId = _sb.Id;
            _sb.VerificaPresenza();
            CaricaStatoFirmware(ref _tempId, _logiche, _sb.apparatoPresente);
            CaricaStatoAreaFw(1, _sb.StatoFirmware.Stato);
            CaricaStatoAreaFw(2, _sb.StatoFirmware.Stato);
            CaricaSeqProc();
            CaricaListaLingueAttive();
        }

        private void textBox33_TextChanged(object sender, EventArgs e)
        {

        }



        private void btnInitFileA1Search_Click(object sender, EventArgs e)
        {
            sfdImportDati.Filter = "SBF SPY-BATT Firmware File (*.sbf)|*.sbf|All files (*.*)|*.*";
            if (txtInitDirFWArea1.Text != "")
            {
                sfdImportDati.FileName = txtInitDirFWArea1.Text;
            }
            sfdImportDati.ShowDialog();
            txtInitDirFWArea1.Text = sfdImportDati.FileName;
        }

        private void btnInitSaveInit_Click(object sender, EventArgs e)
        {
            try
            {
                if (InitSetup == null)
                {
                    InitSetup = new brInitSetup(_logiche.dbDati.connessione);
                    InitSetup.AzzeraValori();
                }

                InitSetup.Valori.FWArea1Enabled = chkInitFWArea1.Checked;
                if (chkInitFWArea1.Checked)
                {
                    InitSetup.Valori.FWArea1Filename = txtInitDirFWArea1.Text;
                }
                else
                {
                    InitSetup.Valori.FWArea1Filename = "";
                }

                InitSetup.Valori.FWArea2Enabled = chkInitFWArea2.Checked;
                if (chkInitFWArea2.Checked)
                {
                    InitSetup.Valori.FWArea2Filename = txtInitDirFWArea2.Text;
                }
                else
                {
                    InitSetup.Valori.FWArea2Filename = "";
                }

                InitSetup.Valori.ProcSeqEnabled = chkInitProcSeq.Checked;
                if (chkInitProcSeq.Checked)
                {
                    InitSetup.Valori.ProcSeqFilename = txtInitDirProcSeq.Text;
                }
                else
                {
                    InitSetup.Valori.ProcSeqFilename = "";
                }

                InitSetup.Valori.LngEnabled = chkInitLingue.Checked;
                if (chkInitLingue.Checked)
                {
                    InitSetup.Valori.LngFilename = txtInitDirLingue.Text;
                }
                else
                {
                    InitSetup.Valori.LngFilename = "";
                }

                InitSetup.Valori.AreaEnabled = chkInitSerArea.Checked;
                if (chkInitSerArea.Checked)
                {
                    if (optInitSerAreaA2.Checked)
                    {
                        InitSetup.Valori.AreaAttiva = 2;
                    }
                    else
                    {
                        InitSetup.Valori.AreaAttiva = 1;
                    }
                }

                InitSetup.Valori.CancellaValori = chkInitDeleteTest.Checked;


                InitSetup.salvaDati();
                MostraInitSetup();
            }
            catch
            {

            }
        }

        private void btnInitFileA2Search_Click(object sender, EventArgs e)
        {
            sfdImportDati.Filter = "SBF SPY-BATT Firmware File (*.sbf)|*.sbf|All files (*.*)|*.*";
            if (txtInitDirFWArea2.Text != "")
            {
                sfdImportDati.FileName = txtInitDirFWArea2.Text;
            }
            sfdImportDati.ShowDialog();
            txtInitDirFWArea2.Text = sfdImportDati.FileName;
        }

        private void btnInitFileProcSearch_Click(object sender, EventArgs e)
        {
            try
            {
                sfdImportDati.Filter = "RSF Regenerator Setup File (*.rsf)|*.rsf|All files (*.*)|*.*";
                if (txtInitDirProcSeq.Text != "")
                {
                    sfdImportDati.FileName = txtInitDirProcSeq.Text;
                }
                sfdImportDati.ShowDialog();
                txtInitDirProcSeq.Text = sfdImportDati.FileName;
            }
            catch
            {
                txtInitDirProcSeq.Text = "";
            }
        }

        private void btnInitFileLngSearch_Click(object sender, EventArgs e)
        {
            try
            {
                sfdImportDati.Filter = "RSF Regenerator Setup File (*.rsf)|*.rsf|All files (*.*)|*.*";
                if (txtInitDirLingue.Text != "")
                {
                    sfdImportDati.FileName = txtInitDirLingue.Text;
                }
                sfdImportDati.ShowDialog();
                txtInitDirLingue.Text = sfdImportDati.FileName;
            }
            catch
            {
                txtInitDirLingue.Text = "";
            }
        }
        private void chkInitFWArea1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInitFWArea1.Checked)
            {
                txtInitDirFWArea1.Enabled = true;
                btnInitFileA1Search.Enabled = true;
            }
            else
            {
                txtInitDirFWArea1.Enabled = false;
                btnInitFileA1Search.Enabled = false;
            }
        }

        private void chkInitFWArea2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInitFWArea2.Checked)
            {
                txtInitDirFWArea2.Enabled = true;
                btnInitFileA2Search.Enabled = true;
            }
            else
            {
                txtInitDirFWArea2.Enabled = false;
                btnInitFileA2Search.Enabled = false;
            }
        }

        private void chkInitProcSeq_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInitProcSeq.Checked)
            {
                txtInitDirProcSeq.Enabled = true;
                btnInitFileProcSearch.Enabled = true;
            }
            else
            {
                txtInitDirProcSeq.Enabled = false;
                btnInitFileProcSearch.Enabled = false;
            }
        }

        private void chkInitLingue_CheckedChanged(object sender, EventArgs e)
        {
            if (chkInitLingue.Checked)
            {
                txtInitDirLingue.Enabled = true;
                btnInitFileLngSearch.Enabled = true;
            }
            else
            {
                txtInitDirLingue.Enabled = false;
                btnInitFileLngSearch.Enabled = false;
            }
        }

        private void btnInitReloadInit_Click(object sender, EventArgs e)
        {
            try
            {
                InitSetup.caricaDati(1);
                MostraInitSetup();
            }
            catch
            {

            }
        }

        public bool InizalizzaRigeneratore()
        {
            try
            {
                Log.Debug("Lancio inizializzazione scheda");
                // Creo una copa temporanea del modello setup da passare al form attività:

                brInitSetup TempSetup = new brInitSetup();
                TempSetup.AzzeraValori();
                TempSetup.Valori.FWArea1Enabled = chkInitFWArea1.Checked;
                TempSetup.Valori.FWArea1Filename = txtInitDirFWArea1.Text;
                TempSetup.Valori.FWArea2Enabled = chkInitFWArea2.Checked;
                TempSetup.Valori.FWArea2Filename = txtInitDirFWArea2.Text;
                TempSetup.Valori.ProcSeqEnabled = chkInitProcSeq.Checked;
                TempSetup.Valori.ProcSeqFilename = txtInitDirProcSeq.Text;
                TempSetup.Valori.LngEnabled = chkInitLingue.Checked;
                TempSetup.Valori.LngFilename = txtInitDirLingue.Text;
                TempSetup.Valori.AreaEnabled = chkInitSerArea.Checked;
                if(optInitSerAreaA2.Checked)
                {
                    TempSetup.Valori.AreaAttiva = 2;
                }
                else
                {
                    TempSetup.Valori.AreaAttiva = 1;
                }
                TempSetup.Valori.CancellaValori = chkInitDeleteTest.Checked;

                this.Cursor = Cursors.WaitCursor;
                //_avCicli.ElementoPilotato = frmAvanzamentoCicli.ControlledDevice.Desolfatatore;
                _avCicli.ParametriWorker.MainCount = 100;
                _avCicli.InitSetup = TempSetup;
                _sb.ModoDesolfatatore = true;
                _avCicli.sbLocale = _sb;
                _avCicli.ValStart = 1;
                _avCicli.ValFine = (int)_sb.sbData.LongMem;
                _avCicli.DbDati = _logiche.dbDati.connessione;
                _avCicli.CaricaBrevi = false;
                _avCicli.TipoComando = elementiComuni.tipoMessaggio.InizializzazioneRigeneratore;
                _avCicli.InviaACK = false;
                _avCicli.SalvaHexDump = false;
                _avCicli.FileHexDump = "";
                _avCicli.Text = "Inizializzazione Rigeneratore";
                Log.Debug("FRM firmwareUPD : ");


                // Apro il form con le progressbar

                _avCicli.ShowDialog(this);

                this.Cursor = Cursors.Default;
                return true;
            }

            catch (Exception Ex)
            {
                Log.Error("frmSpyBat.DumpInteraMemoria: " + Ex.Message);
                return false;
            }

        }

        private void btnInitExecuteInit_Click(object sender, EventArgs e)
        {
            try
            {
                //bool _esito;
                string _tempId = _sb.Id;

                this.Cursor = Cursors.WaitCursor;
                InizalizzaRigeneratore();

                CaricaStatoFirmware(ref _tempId, _logiche, _sb.apparatoPresente);
                CaricaStatoAreaFw(1, _sb.StatoFirmware.Stato);
                CaricaStatoAreaFw(2, _sb.StatoFirmware.Stato);
                CaricaSeqProc();
                CaricaListaLingueAttive();

                this.Cursor = Cursors.Default;
            }
            catch
            {
                this.Cursor = Cursors.Default;

            }
        }
    }

}
