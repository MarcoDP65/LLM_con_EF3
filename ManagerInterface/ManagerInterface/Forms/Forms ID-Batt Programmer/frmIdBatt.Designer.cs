namespace PannelloCharger
{
    partial class frmIdBatt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIdBatt));
            this.tmrLetturaAutomatica = new System.Windows.Forms.Timer(this.components);
            this.sfdExportDati = new System.Windows.Forms.SaveFileDialog();
            this.sfdImportDati = new System.Windows.Forms.OpenFileDialog();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabProfiloAttuale = new System.Windows.Forms.TabPage();
            this.tbcPaSottopagina = new System.Windows.Forms.TabControl();
            this.tbpPaListaProfili = new System.Windows.Forms.TabPage();
            this.btnPaCopiaSelezionati = new System.Windows.Forms.Button();
            this.grbPaCaricaFile = new System.Windows.Forms.GroupBox();
            this.btnPaCaricaFile = new System.Windows.Forms.Button();
            this.btnPaFileInProfiliSRC = new System.Windows.Forms.Button();
            this.txtPaCaricaFileProfili = new System.Windows.Forms.TextBox();
            this.btnPaGeneraQr = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPaCancellaSelezionati = new System.Windows.Forms.Button();
            this.grbPaGeneraFile = new System.Windows.Forms.GroupBox();
            this.btnPaSalvaFile = new System.Windows.Forms.Button();
            this.chkPaSoloSelezionati = new System.Windows.Forms.CheckBox();
            this.btnPaNomeFileProfiliSRC = new System.Windows.Forms.Button();
            this.txtPaNomeFileProfili = new System.Windows.Forms.TextBox();
            this.btnPaAttivaConfigurazione = new System.Windows.Forms.Button();
            this.flwPaListaConfigurazioni = new BrightIdeasSoftware.FastObjectListView();
            this.btnPaCaricaListaProfili = new System.Windows.Forms.Button();
            this.tbpPaProfiloAttivo = new System.Windows.Forms.TabPage();
            this.pippo = new System.Windows.Forms.GroupBox();
            this.btnPaProfileClear = new System.Windows.Forms.Button();
            this.grbPaImpostazioniLocali = new System.Windows.Forms.GroupBox();
            this.btnPaProfileNEW = new System.Windows.Forms.Button();
            this.chkPaUsaSafety = new System.Windows.Forms.CheckBox();
            this.lblPaUsaSafety = new System.Windows.Forms.Label();
            this.cmbPaProfilo = new System.Windows.Forms.ComboBox();
            this.txtPaCapacita = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkPaAttivaOppChg = new System.Windows.Forms.CheckBox();
            this.lblPaAttivaOppChg = new System.Windows.Forms.Label();
            this.chkPaAttivaMant = new System.Windows.Forms.CheckBox();
            this.lblPaAttivaMant = new System.Windows.Forms.Label();
            this.txtPaNumCelle = new System.Windows.Forms.TextBox();
            this.lblPaNumCelle = new System.Windows.Forms.Label();
            this.cmbPaTipoBatteria = new System.Windows.Forms.ComboBox();
            this.lblPaTipoBatteria = new System.Windows.Forms.Label();
            this.chkPaAttivaEqual = new System.Windows.Forms.CheckBox();
            this.lblPaAttivaEqual = new System.Windows.Forms.Label();
            this.lblPaTensione = new System.Windows.Forms.Label();
            this.cmbPaTensione = new System.Windows.Forms.ComboBox();
            this.txtPaTensione = new System.Windows.Forms.TextBox();
            this.btnPaCaricaCicli = new System.Windows.Forms.Button();
            this.chkPaSbloccaValori = new System.Windows.Forms.CheckBox();
            this.lblPaSbloccaValori = new System.Windows.Forms.Label();
            this.btnPaProfileRefresh = new System.Windows.Forms.Button();
            this.picPaImmagineProfilo = new System.Windows.Forms.PictureBox();
            this.tbcPaSchedeValori = new System.Windows.Forms.TabControl();
            this.tbpPaGeneraleCiclo = new System.Windows.Forms.TabPage();
            this.txtPaDescrizioneSetup = new System.Windows.Forms.TextBox();
            this.lblPaDescrizioneSetup = new System.Windows.Forms.Label();
            this.txtPaCassone = new System.Windows.Forms.TextBox();
            this.lblPaCassone = new System.Windows.Forms.Label();
            this.txtPaIdSetup = new System.Windows.Forms.TextBox();
            this.lblPaIdSetup = new System.Windows.Forms.Label();
            this.txtPaNomeSetup = new System.Windows.Forms.TextBox();
            this.lblPaNomeSetup = new System.Windows.Forms.Label();
            this.tbpPaPCStep0 = new System.Windows.Forms.TabPage();
            this.txtPaDurataMaxT0 = new System.Windows.Forms.TextBox();
            this.lblPaDurataMaxT0 = new System.Windows.Forms.Label();
            this.txtPaPrefaseI0 = new System.Windows.Forms.TextBox();
            this.lblPaPrefaseI0 = new System.Windows.Forms.Label();
            this.txtPaSogliaV0 = new System.Windows.Forms.TextBox();
            this.lblPaSogliaV0 = new System.Windows.Forms.Label();
            this.tbpPaPCStep1 = new System.Windows.Forms.TabPage();
            this.cmbPaDurataMaxT1 = new System.Windows.Forms.TextBox();
            this.cmbPaDurataCarica = new System.Windows.Forms.ComboBox();
            this.lblPaDurataMaxT1 = new System.Windows.Forms.Label();
            this.txtPaCorrenteI1 = new System.Windows.Forms.TextBox();
            this.lblPaCorrenteI1 = new System.Windows.Forms.Label();
            this.txtPaSogliaVs = new System.Windows.Forms.TextBox();
            this.lblPaSogliaVs = new System.Windows.Forms.Label();
            this.tbpPaPCStep2 = new System.Windows.Forms.TabPage();
            this.txtPaTempoFin = new System.Windows.Forms.TextBox();
            this.lblPaTempoFin = new System.Windows.Forms.Label();
            this.txtPadT = new System.Windows.Forms.TextBox();
            this.lblPadT = new System.Windows.Forms.Label();
            this.txtPadV = new System.Windows.Forms.TextBox();
            this.lblPadV = new System.Windows.Forms.Label();
            this.lblPaCoeffKc = new System.Windows.Forms.Label();
            this.txtPaCoeffKc = new System.Windows.Forms.TextBox();
            this.txtPaVMax = new System.Windows.Forms.TextBox();
            this.lblPaVMax = new System.Windows.Forms.Label();
            this.lblPaCoeffK = new System.Windows.Forms.Label();
            this.txtPaCoeffK = new System.Windows.Forms.TextBox();
            this.lblPaTempoT2Max = new System.Windows.Forms.Label();
            this.txtPaTempoT2Max = new System.Windows.Forms.TextBox();
            this.txtPaTempoT2Min = new System.Windows.Forms.TextBox();
            this.lblPaTempoT2Min = new System.Windows.Forms.Label();
            this.txtPaCorrenteRaccordo = new System.Windows.Forms.TextBox();
            this.lblPaCorrenteRaccordo = new System.Windows.Forms.Label();
            this.txtPaCorrenteF3 = new System.Windows.Forms.TextBox();
            this.lblPaCorrenteF3 = new System.Windows.Forms.Label();
            this.txtPaRaccordoF1 = new System.Windows.Forms.TextBox();
            this.lblPaRaccordoF1 = new System.Windows.Forms.Label();
            this.tbpPaPCStep3 = new System.Windows.Forms.TabPage();
            this.lblPaTempoT3Max = new System.Windows.Forms.Label();
            this.txtPaTempoT3Max = new System.Windows.Forms.TextBox();
            this.tbpPaPCEqual = new System.Windows.Forms.TabPage();
            this.txtPaEqualPulseCurrent = new System.Windows.Forms.TextBox();
            this.lblPaEqualPulseCurrent = new System.Windows.Forms.Label();
            this.txtPaEqualPulseTime = new System.Windows.Forms.TextBox();
            this.lblPaEqualPulseTime = new System.Windows.Forms.Label();
            this.txtPaEqualPulsePause = new System.Windows.Forms.TextBox();
            this.lblPaEqualPulsePause = new System.Windows.Forms.Label();
            this.txtPaEqualNumPulse = new System.Windows.Forms.TextBox();
            this.lblPaEqualNumPulse = new System.Windows.Forms.Label();
            this.txtPaEqualAttesa = new System.Windows.Forms.TextBox();
            this.lblPaEqualAttesa = new System.Windows.Forms.Label();
            this.tbpPaPCMant = new System.Windows.Forms.TabPage();
            this.txtPaMantCorrente = new System.Windows.Forms.TextBox();
            this.lblPaMantCorrente = new System.Windows.Forms.Label();
            this.txtPaMantDurataMax = new System.Windows.Forms.TextBox();
            this.lblPaMantDurataMax = new System.Windows.Forms.Label();
            this.txtPaMantVmax = new System.Windows.Forms.TextBox();
            this.lblPaMantVmax = new System.Windows.Forms.Label();
            this.txtPaMantVmin = new System.Windows.Forms.TextBox();
            this.lblPaMantVmin = new System.Windows.Forms.Label();
            this.txtPaMantAttesa = new System.Windows.Forms.TextBox();
            this.lblPaMantAttesa = new System.Windows.Forms.Label();
            this.tbpPaPCOpp = new System.Windows.Forms.TabPage();
            this.lblPaOppPuntoVerde = new System.Windows.Forms.Label();
            this.ImgPaOppPuntoVerde = new System.Windows.Forms.PictureBox();
            this.chkPaOppNotturno = new System.Windows.Forms.CheckBox();
            this.rslPaOppFinestra = new Syncfusion.Windows.Forms.Tools.RangeSlider();
            this.txtPaOppDurataMax = new System.Windows.Forms.TextBox();
            this.lblPaOppDurataMax = new System.Windows.Forms.Label();
            this.txtPaOppCorrente = new System.Windows.Forms.TextBox();
            this.lblPaOppCorrente = new System.Windows.Forms.Label();
            this.txtPaOppVSoglia = new System.Windows.Forms.TextBox();
            this.lblPaOppVSoglia = new System.Windows.Forms.Label();
            this.txtPaOppOraFine = new System.Windows.Forms.TextBox();
            this.lblPaOppOraFine = new System.Windows.Forms.Label();
            this.txtPaOppOraInizio = new System.Windows.Forms.TextBox();
            this.lblPaOppOraInizio = new System.Windows.Forms.Label();
            this.tbpPaParSoglia = new System.Windows.Forms.TabPage();
            this.txtPaTempLimite = new System.Windows.Forms.TextBox();
            this.lblPaTempLimite = new System.Windows.Forms.Label();
            this.txtPaVMinStop = new System.Windows.Forms.TextBox();
            this.lblPaVMinStop = new System.Windows.Forms.Label();
            this.txtPaBMSTempoAttesa = new System.Windows.Forms.TextBox();
            this.lblPaBMSTempoAttesa = new System.Windows.Forms.Label();
            this.txtPaBMSTempoErogazione = new System.Windows.Forms.TextBox();
            this.lblPaBMSTempoErogazione = new System.Windows.Forms.Label();
            this.chkPaRiarmaBms = new System.Windows.Forms.Label();
            this.chkPaAttivaRiarmoBms = new System.Windows.Forms.CheckBox();
            this.txtPaCorrenteMassima = new System.Windows.Forms.TextBox();
            this.lblPaCorrenteMassima = new System.Windows.Forms.Label();
            this.txtPaVMaxRic = new System.Windows.Forms.TextBox();
            this.lblPaVMaxRic = new System.Windows.Forms.Label();
            this.txtPaVMinRic = new System.Windows.Forms.TextBox();
            this.lblPaVMinRic = new System.Windows.Forms.Label();
            this.txtPaVLimite = new System.Windows.Forms.TextBox();
            this.lblPaVLimite = new System.Windows.Forms.Label();
            this.chkPaUsaSpyBatt = new System.Windows.Forms.CheckBox();
            this.label69 = new System.Windows.Forms.Label();
            this.btnPaSalvaDati = new System.Windows.Forms.Button();
            this.lblPaTitoloLista = new System.Windows.Forms.Label();
            this.tabGenerale = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnGenResetBoard = new System.Windows.Forms.Button();
            this.btnGenAzzzeraContatoriTot = new System.Windows.Forms.Button();
            this.btnGenAzzzeraContatori = new System.Windows.Forms.Button();
            this.btnCaricaContatori = new System.Windows.Forms.Button();
            this.GrbMainDatiApparato = new System.Windows.Forms.GroupBox();
            this.cmbGenModelloCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGenCorrenteMax = new System.Windows.Forms.TextBox();
            this.label143 = new System.Windows.Forms.Label();
            this.txtGenTensioneMax = new System.Windows.Forms.TextBox();
            this.label142 = new System.Windows.Forms.Label();
            this.txtGenIdApparato = new System.Windows.Forms.TextBox();
            this.label137 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtGenAnnoMatricola = new System.Windows.Forms.TextBox();
            this.txtGenMatricola = new System.Windows.Forms.TextBox();
            this.lblMatricola = new System.Windows.Forms.Label();
            this.tabCaricaBatterie = new System.Windows.Forms.TabControl();
            this.tabProfiloAttuale.SuspendLayout();
            this.tbcPaSottopagina.SuspendLayout();
            this.tbpPaListaProfili.SuspendLayout();
            this.grbPaCaricaFile.SuspendLayout();
            this.grbPaGeneraFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flwPaListaConfigurazioni)).BeginInit();
            this.tbpPaProfiloAttivo.SuspendLayout();
            this.pippo.SuspendLayout();
            this.grbPaImpostazioniLocali.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPaImmagineProfilo)).BeginInit();
            this.tbcPaSchedeValori.SuspendLayout();
            this.tbpPaGeneraleCiclo.SuspendLayout();
            this.tbpPaPCStep0.SuspendLayout();
            this.tbpPaPCStep1.SuspendLayout();
            this.tbpPaPCStep2.SuspendLayout();
            this.tbpPaPCStep3.SuspendLayout();
            this.tbpPaPCEqual.SuspendLayout();
            this.tbpPaPCMant.SuspendLayout();
            this.tbpPaPCOpp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImgPaOppPuntoVerde)).BeginInit();
            this.tbpPaParSoglia.SuspendLayout();
            this.tabGenerale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.GrbMainDatiApparato.SuspendLayout();
            this.tabCaricaBatterie.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrLetturaAutomatica
            // 
            this.tmrLetturaAutomatica.Interval = 30000;
            // 
            // sfdImportDati
            // 
            this.sfdImportDati.FileName = "prova";
            // 
            // tabPage1
            // 
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabProfiloAttuale
            // 
            this.tabProfiloAttuale.BackColor = System.Drawing.Color.LightYellow;
            this.tabProfiloAttuale.Controls.Add(this.tbcPaSottopagina);
            this.tabProfiloAttuale.Controls.Add(this.lblPaTitoloLista);
            resources.ApplyResources(this.tabProfiloAttuale, "tabProfiloAttuale");
            this.tabProfiloAttuale.Name = "tabProfiloAttuale";
            // 
            // tbcPaSottopagina
            // 
            resources.ApplyResources(this.tbcPaSottopagina, "tbcPaSottopagina");
            this.tbcPaSottopagina.Controls.Add(this.tbpPaListaProfili);
            this.tbcPaSottopagina.Controls.Add(this.tbpPaProfiloAttivo);
            this.tbcPaSottopagina.Name = "tbcPaSottopagina";
            this.tbcPaSottopagina.SelectedIndex = 0;
            // 
            // tbpPaListaProfili
            // 
            this.tbpPaListaProfili.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tbpPaListaProfili, "tbpPaListaProfili");
            this.tbpPaListaProfili.Controls.Add(this.btnPaCopiaSelezionati);
            this.tbpPaListaProfili.Controls.Add(this.grbPaCaricaFile);
            this.tbpPaListaProfili.Controls.Add(this.btnPaGeneraQr);
            this.tbpPaListaProfili.Controls.Add(this.button1);
            this.tbpPaListaProfili.Controls.Add(this.btnPaCancellaSelezionati);
            this.tbpPaListaProfili.Controls.Add(this.grbPaGeneraFile);
            this.tbpPaListaProfili.Controls.Add(this.btnPaAttivaConfigurazione);
            this.tbpPaListaProfili.Controls.Add(this.flwPaListaConfigurazioni);
            this.tbpPaListaProfili.Controls.Add(this.btnPaCaricaListaProfili);
            this.tbpPaListaProfili.Name = "tbpPaListaProfili";
            this.tbpPaListaProfili.UseVisualStyleBackColor = true;
            // 
            // btnPaCopiaSelezionati
            // 
            resources.ApplyResources(this.btnPaCopiaSelezionati, "btnPaCopiaSelezionati");
            this.btnPaCopiaSelezionati.Name = "btnPaCopiaSelezionati";
            this.btnPaCopiaSelezionati.UseVisualStyleBackColor = true;
            this.btnPaCopiaSelezionati.Click += new System.EventHandler(this.btnPaCopiaSelezionati_Click);
            // 
            // grbPaCaricaFile
            // 
            this.grbPaCaricaFile.BackColor = System.Drawing.Color.White;
            this.grbPaCaricaFile.Controls.Add(this.btnPaCaricaFile);
            this.grbPaCaricaFile.Controls.Add(this.btnPaFileInProfiliSRC);
            this.grbPaCaricaFile.Controls.Add(this.txtPaCaricaFileProfili);
            resources.ApplyResources(this.grbPaCaricaFile, "grbPaCaricaFile");
            this.grbPaCaricaFile.Name = "grbPaCaricaFile";
            this.grbPaCaricaFile.TabStop = false;
            // 
            // btnPaCaricaFile
            // 
            resources.ApplyResources(this.btnPaCaricaFile, "btnPaCaricaFile");
            this.btnPaCaricaFile.Name = "btnPaCaricaFile";
            this.btnPaCaricaFile.TabStop = false;
            this.btnPaCaricaFile.UseVisualStyleBackColor = true;
            this.btnPaCaricaFile.Click += new System.EventHandler(this.btnPaCaricaFile_Click);
            // 
            // btnPaFileInProfiliSRC
            // 
            resources.ApplyResources(this.btnPaFileInProfiliSRC, "btnPaFileInProfiliSRC");
            this.btnPaFileInProfiliSRC.Name = "btnPaFileInProfiliSRC";
            this.btnPaFileInProfiliSRC.UseVisualStyleBackColor = true;
            this.btnPaFileInProfiliSRC.Click += new System.EventHandler(this.btnPaFileInProfiliSRC_Click);
            // 
            // txtPaCaricaFileProfili
            // 
            resources.ApplyResources(this.txtPaCaricaFileProfili, "txtPaCaricaFileProfili");
            this.txtPaCaricaFileProfili.Name = "txtPaCaricaFileProfili";
            // 
            // btnPaGeneraQr
            // 
            resources.ApplyResources(this.btnPaGeneraQr, "btnPaGeneraQr");
            this.btnPaGeneraQr.Name = "btnPaGeneraQr";
            this.btnPaGeneraQr.UseVisualStyleBackColor = true;
            this.btnPaGeneraQr.Click += new System.EventHandler(this.btnPaGeneraQr_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnPaCancellaSelezionati
            // 
            resources.ApplyResources(this.btnPaCancellaSelezionati, "btnPaCancellaSelezionati");
            this.btnPaCancellaSelezionati.Name = "btnPaCancellaSelezionati";
            this.btnPaCancellaSelezionati.UseVisualStyleBackColor = true;
            this.btnPaCancellaSelezionati.Click += new System.EventHandler(this.btnPaCancellaSelezionati_Click);
            // 
            // grbPaGeneraFile
            // 
            this.grbPaGeneraFile.BackColor = System.Drawing.Color.White;
            this.grbPaGeneraFile.Controls.Add(this.btnPaSalvaFile);
            this.grbPaGeneraFile.Controls.Add(this.chkPaSoloSelezionati);
            this.grbPaGeneraFile.Controls.Add(this.btnPaNomeFileProfiliSRC);
            this.grbPaGeneraFile.Controls.Add(this.txtPaNomeFileProfili);
            resources.ApplyResources(this.grbPaGeneraFile, "grbPaGeneraFile");
            this.grbPaGeneraFile.Name = "grbPaGeneraFile";
            this.grbPaGeneraFile.TabStop = false;
            // 
            // btnPaSalvaFile
            // 
            resources.ApplyResources(this.btnPaSalvaFile, "btnPaSalvaFile");
            this.btnPaSalvaFile.Name = "btnPaSalvaFile";
            this.btnPaSalvaFile.UseVisualStyleBackColor = true;
            this.btnPaSalvaFile.Click += new System.EventHandler(this.btnPaSalvaFile_Click);
            // 
            // chkPaSoloSelezionati
            // 
            resources.ApplyResources(this.chkPaSoloSelezionati, "chkPaSoloSelezionati");
            this.chkPaSoloSelezionati.Checked = true;
            this.chkPaSoloSelezionati.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPaSoloSelezionati.Name = "chkPaSoloSelezionati";
            this.chkPaSoloSelezionati.UseVisualStyleBackColor = true;
            // 
            // btnPaNomeFileProfiliSRC
            // 
            resources.ApplyResources(this.btnPaNomeFileProfiliSRC, "btnPaNomeFileProfiliSRC");
            this.btnPaNomeFileProfiliSRC.Name = "btnPaNomeFileProfiliSRC";
            this.btnPaNomeFileProfiliSRC.UseVisualStyleBackColor = true;
            this.btnPaNomeFileProfiliSRC.Click += new System.EventHandler(this.btnPaNomeFileProfiliSRC_Click);
            // 
            // txtPaNomeFileProfili
            // 
            resources.ApplyResources(this.txtPaNomeFileProfili, "txtPaNomeFileProfili");
            this.txtPaNomeFileProfili.Name = "txtPaNomeFileProfili";
            // 
            // btnPaAttivaConfigurazione
            // 
            resources.ApplyResources(this.btnPaAttivaConfigurazione, "btnPaAttivaConfigurazione");
            this.btnPaAttivaConfigurazione.Name = "btnPaAttivaConfigurazione";
            this.btnPaAttivaConfigurazione.UseVisualStyleBackColor = true;
            this.btnPaAttivaConfigurazione.Click += new System.EventHandler(this.btnPaAttivaConfigurazione_Click);
            // 
            // flwPaListaConfigurazioni
            // 
            this.flwPaListaConfigurazioni.CellEditUseWholeCell = false;
            this.flwPaListaConfigurazioni.HideSelection = false;
            resources.ApplyResources(this.flwPaListaConfigurazioni, "flwPaListaConfigurazioni");
            this.flwPaListaConfigurazioni.Name = "flwPaListaConfigurazioni";
            this.flwPaListaConfigurazioni.ShowGroups = false;
            this.flwPaListaConfigurazioni.UseCompatibleStateImageBehavior = false;
            this.flwPaListaConfigurazioni.View = System.Windows.Forms.View.Details;
            this.flwPaListaConfigurazioni.VirtualMode = true;
            this.flwPaListaConfigurazioni.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.flwPaListaConfigurazioni_FormatRow);
            this.flwPaListaConfigurazioni.SelectedIndexChanged += new System.EventHandler(this.flwPaListaConfigurazioni_SelectedIndexChanged);
            this.flwPaListaConfigurazioni.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.flwPaListaConfigurazioni_MouseDoubleClick);
            // 
            // btnPaCaricaListaProfili
            // 
            resources.ApplyResources(this.btnPaCaricaListaProfili, "btnPaCaricaListaProfili");
            this.btnPaCaricaListaProfili.Name = "btnPaCaricaListaProfili";
            this.btnPaCaricaListaProfili.UseVisualStyleBackColor = true;
            // 
            // tbpPaProfiloAttivo
            // 
            this.tbpPaProfiloAttivo.BackColor = System.Drawing.Color.White;
            this.tbpPaProfiloAttivo.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tbpPaProfiloAttivo, "tbpPaProfiloAttivo");
            this.tbpPaProfiloAttivo.Controls.Add(this.pippo);
            this.tbpPaProfiloAttivo.Name = "tbpPaProfiloAttivo";
            this.tbpPaProfiloAttivo.Click += new System.EventHandler(this.tbpPaProfiloAttivo_Click);
            // 
            // pippo
            // 
            this.pippo.BackColor = System.Drawing.Color.White;
            this.pippo.Controls.Add(this.btnPaProfileClear);
            this.pippo.Controls.Add(this.grbPaImpostazioniLocali);
            this.pippo.Controls.Add(this.btnPaCaricaCicli);
            this.pippo.Controls.Add(this.chkPaSbloccaValori);
            this.pippo.Controls.Add(this.lblPaSbloccaValori);
            this.pippo.Controls.Add(this.btnPaProfileRefresh);
            this.pippo.Controls.Add(this.picPaImmagineProfilo);
            this.pippo.Controls.Add(this.tbcPaSchedeValori);
            this.pippo.Controls.Add(this.chkPaUsaSpyBatt);
            this.pippo.Controls.Add(this.label69);
            this.pippo.Controls.Add(this.btnPaSalvaDati);
            resources.ApplyResources(this.pippo, "pippo");
            this.pippo.Name = "pippo";
            this.pippo.TabStop = false;
            // 
            // btnPaProfileClear
            // 
            resources.ApplyResources(this.btnPaProfileClear, "btnPaProfileClear");
            this.btnPaProfileClear.Name = "btnPaProfileClear";
            this.btnPaProfileClear.UseVisualStyleBackColor = true;
            this.btnPaProfileClear.Click += new System.EventHandler(this.btnPaProfileClear_Click);
            // 
            // grbPaImpostazioniLocali
            // 
            this.grbPaImpostazioniLocali.Controls.Add(this.btnPaProfileNEW);
            this.grbPaImpostazioniLocali.Controls.Add(this.chkPaUsaSafety);
            this.grbPaImpostazioniLocali.Controls.Add(this.lblPaUsaSafety);
            this.grbPaImpostazioniLocali.Controls.Add(this.cmbPaProfilo);
            this.grbPaImpostazioniLocali.Controls.Add(this.txtPaCapacita);
            this.grbPaImpostazioniLocali.Controls.Add(this.label5);
            this.grbPaImpostazioniLocali.Controls.Add(this.label4);
            this.grbPaImpostazioniLocali.Controls.Add(this.chkPaAttivaOppChg);
            this.grbPaImpostazioniLocali.Controls.Add(this.lblPaAttivaOppChg);
            this.grbPaImpostazioniLocali.Controls.Add(this.chkPaAttivaMant);
            this.grbPaImpostazioniLocali.Controls.Add(this.lblPaAttivaMant);
            this.grbPaImpostazioniLocali.Controls.Add(this.txtPaNumCelle);
            this.grbPaImpostazioniLocali.Controls.Add(this.lblPaNumCelle);
            this.grbPaImpostazioniLocali.Controls.Add(this.cmbPaTipoBatteria);
            this.grbPaImpostazioniLocali.Controls.Add(this.lblPaTipoBatteria);
            this.grbPaImpostazioniLocali.Controls.Add(this.chkPaAttivaEqual);
            this.grbPaImpostazioniLocali.Controls.Add(this.lblPaAttivaEqual);
            this.grbPaImpostazioniLocali.Controls.Add(this.lblPaTensione);
            this.grbPaImpostazioniLocali.Controls.Add(this.cmbPaTensione);
            this.grbPaImpostazioniLocali.Controls.Add(this.txtPaTensione);
            resources.ApplyResources(this.grbPaImpostazioniLocali, "grbPaImpostazioniLocali");
            this.grbPaImpostazioniLocali.Name = "grbPaImpostazioniLocali";
            this.grbPaImpostazioniLocali.TabStop = false;
            // 
            // btnPaProfileNEW
            // 
            resources.ApplyResources(this.btnPaProfileNEW, "btnPaProfileNEW");
            this.btnPaProfileNEW.ForeColor = System.Drawing.Color.Red;
            this.btnPaProfileNEW.Name = "btnPaProfileNEW";
            this.btnPaProfileNEW.UseVisualStyleBackColor = true;
            this.btnPaProfileNEW.Click += new System.EventHandler(this.btnPaProfileNEW_Click);
            // 
            // chkPaUsaSafety
            // 
            resources.ApplyResources(this.chkPaUsaSafety, "chkPaUsaSafety");
            this.chkPaUsaSafety.Name = "chkPaUsaSafety";
            this.chkPaUsaSafety.UseVisualStyleBackColor = true;
            this.chkPaUsaSafety.CheckedChanged += new System.EventHandler(this.ChkPaUsaSafety_CheckedChanged);
            // 
            // lblPaUsaSafety
            // 
            resources.ApplyResources(this.lblPaUsaSafety, "lblPaUsaSafety");
            this.lblPaUsaSafety.ForeColor = System.Drawing.Color.Red;
            this.lblPaUsaSafety.Name = "lblPaUsaSafety";
            // 
            // cmbPaProfilo
            // 
            this.cmbPaProfilo.BackColor = System.Drawing.Color.White;
            this.cmbPaProfilo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbPaProfilo, "cmbPaProfilo");
            this.cmbPaProfilo.FormattingEnabled = true;
            this.cmbPaProfilo.Name = "cmbPaProfilo";
            this.cmbPaProfilo.SelectedIndexChanged += new System.EventHandler(this.cmbPaProfilo_SelectedIndexChanged);
            // 
            // txtPaCapacita
            // 
            resources.ApplyResources(this.txtPaCapacita, "txtPaCapacita");
            this.txtPaCapacita.Name = "txtPaCapacita";
            this.txtPaCapacita.TextChanged += new System.EventHandler(this.txtPaCapacita_TextChanged);
            this.txtPaCapacita.Leave += new System.EventHandler(this.txtPaCapacita_Leave);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Name = "label4";
            // 
            // chkPaAttivaOppChg
            // 
            resources.ApplyResources(this.chkPaAttivaOppChg, "chkPaAttivaOppChg");
            this.chkPaAttivaOppChg.Name = "chkPaAttivaOppChg";
            this.chkPaAttivaOppChg.UseVisualStyleBackColor = true;
            this.chkPaAttivaOppChg.CheckedChanged += new System.EventHandler(this.ChkPaAttivaOppChg_CheckedChanged);
            this.chkPaAttivaOppChg.EnabledChanged += new System.EventHandler(this.chkPaAttivaOppChg_EnabledChanged);
            // 
            // lblPaAttivaOppChg
            // 
            resources.ApplyResources(this.lblPaAttivaOppChg, "lblPaAttivaOppChg");
            this.lblPaAttivaOppChg.ForeColor = System.Drawing.Color.Blue;
            this.lblPaAttivaOppChg.Name = "lblPaAttivaOppChg";
            // 
            // chkPaAttivaMant
            // 
            resources.ApplyResources(this.chkPaAttivaMant, "chkPaAttivaMant");
            this.chkPaAttivaMant.Name = "chkPaAttivaMant";
            this.chkPaAttivaMant.UseVisualStyleBackColor = true;
            this.chkPaAttivaMant.CheckedChanged += new System.EventHandler(this.chkPaAttivaMant_CheckedChanged);
            this.chkPaAttivaMant.EnabledChanged += new System.EventHandler(this.chkPaAttivaMant_EnabledChanged);
            // 
            // lblPaAttivaMant
            // 
            resources.ApplyResources(this.lblPaAttivaMant, "lblPaAttivaMant");
            this.lblPaAttivaMant.ForeColor = System.Drawing.Color.Blue;
            this.lblPaAttivaMant.Name = "lblPaAttivaMant";
            // 
            // txtPaNumCelle
            // 
            resources.ApplyResources(this.txtPaNumCelle, "txtPaNumCelle");
            this.txtPaNumCelle.Name = "txtPaNumCelle";
            // 
            // lblPaNumCelle
            // 
            resources.ApplyResources(this.lblPaNumCelle, "lblPaNumCelle");
            this.lblPaNumCelle.ForeColor = System.Drawing.Color.Blue;
            this.lblPaNumCelle.Name = "lblPaNumCelle";
            // 
            // cmbPaTipoBatteria
            // 
            resources.ApplyResources(this.cmbPaTipoBatteria, "cmbPaTipoBatteria");
            this.cmbPaTipoBatteria.FormattingEnabled = true;
            this.cmbPaTipoBatteria.Name = "cmbPaTipoBatteria";
            this.cmbPaTipoBatteria.SelectedIndexChanged += new System.EventHandler(this.cmbPaTipoBatteria_SelectedIndexChanged);
            // 
            // lblPaTipoBatteria
            // 
            resources.ApplyResources(this.lblPaTipoBatteria, "lblPaTipoBatteria");
            this.lblPaTipoBatteria.ForeColor = System.Drawing.Color.Blue;
            this.lblPaTipoBatteria.Name = "lblPaTipoBatteria";
            // 
            // chkPaAttivaEqual
            // 
            resources.ApplyResources(this.chkPaAttivaEqual, "chkPaAttivaEqual");
            this.chkPaAttivaEqual.Name = "chkPaAttivaEqual";
            this.chkPaAttivaEqual.UseVisualStyleBackColor = true;
            this.chkPaAttivaEqual.CheckedChanged += new System.EventHandler(this.chkPaAttivaEqual_CheckedChanged);
            this.chkPaAttivaEqual.EnabledChanged += new System.EventHandler(this.chkPaAttivaEqual_EnabledChanged);
            // 
            // lblPaAttivaEqual
            // 
            resources.ApplyResources(this.lblPaAttivaEqual, "lblPaAttivaEqual");
            this.lblPaAttivaEqual.ForeColor = System.Drawing.Color.Blue;
            this.lblPaAttivaEqual.Name = "lblPaAttivaEqual";
            // 
            // lblPaTensione
            // 
            resources.ApplyResources(this.lblPaTensione, "lblPaTensione");
            this.lblPaTensione.ForeColor = System.Drawing.Color.Blue;
            this.lblPaTensione.Name = "lblPaTensione";
            // 
            // cmbPaTensione
            // 
            this.cmbPaTensione.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbPaTensione, "cmbPaTensione");
            this.cmbPaTensione.FormattingEnabled = true;
            this.cmbPaTensione.Name = "cmbPaTensione";
            this.cmbPaTensione.SelectedIndexChanged += new System.EventHandler(this.cmbPaTensione_SelectedIndexChanged);
            // 
            // txtPaTensione
            // 
            resources.ApplyResources(this.txtPaTensione, "txtPaTensione");
            this.txtPaTensione.Name = "txtPaTensione";
            // 
            // btnPaCaricaCicli
            // 
            this.btnPaCaricaCicli.AutoEllipsis = true;
            resources.ApplyResources(this.btnPaCaricaCicli, "btnPaCaricaCicli");
            this.btnPaCaricaCicli.Name = "btnPaCaricaCicli";
            this.btnPaCaricaCicli.UseVisualStyleBackColor = true;
            this.btnPaCaricaCicli.Click += new System.EventHandler(this.btnPaCaricaCicli_Click);
            // 
            // chkPaSbloccaValori
            // 
            resources.ApplyResources(this.chkPaSbloccaValori, "chkPaSbloccaValori");
            this.chkPaSbloccaValori.Name = "chkPaSbloccaValori";
            this.chkPaSbloccaValori.UseVisualStyleBackColor = true;
            this.chkPaSbloccaValori.CheckedChanged += new System.EventHandler(this.chkPaSbloccaValori_CheckedChanged);
            // 
            // lblPaSbloccaValori
            // 
            resources.ApplyResources(this.lblPaSbloccaValori, "lblPaSbloccaValori");
            this.lblPaSbloccaValori.ForeColor = System.Drawing.Color.Red;
            this.lblPaSbloccaValori.Name = "lblPaSbloccaValori";
            // 
            // btnPaProfileRefresh
            // 
            resources.ApplyResources(this.btnPaProfileRefresh, "btnPaProfileRefresh");
            this.btnPaProfileRefresh.Name = "btnPaProfileRefresh";
            this.btnPaProfileRefresh.UseVisualStyleBackColor = true;
            this.btnPaProfileRefresh.Click += new System.EventHandler(this.btnPaProfileRefresh_Click);
            // 
            // picPaImmagineProfilo
            // 
            this.picPaImmagineProfilo.BackColor = System.Drawing.Color.LightGray;
            resources.ApplyResources(this.picPaImmagineProfilo, "picPaImmagineProfilo");
            this.picPaImmagineProfilo.Name = "picPaImmagineProfilo";
            this.picPaImmagineProfilo.TabStop = false;
            // 
            // tbcPaSchedeValori
            // 
            this.tbcPaSchedeValori.Controls.Add(this.tbpPaGeneraleCiclo);
            this.tbcPaSchedeValori.Controls.Add(this.tbpPaPCStep0);
            this.tbcPaSchedeValori.Controls.Add(this.tbpPaPCStep1);
            this.tbcPaSchedeValori.Controls.Add(this.tbpPaPCStep2);
            this.tbcPaSchedeValori.Controls.Add(this.tbpPaPCStep3);
            this.tbcPaSchedeValori.Controls.Add(this.tbpPaPCEqual);
            this.tbcPaSchedeValori.Controls.Add(this.tbpPaPCMant);
            this.tbcPaSchedeValori.Controls.Add(this.tbpPaPCOpp);
            this.tbcPaSchedeValori.Controls.Add(this.tbpPaParSoglia);
            this.tbcPaSchedeValori.HotTrack = true;
            resources.ApplyResources(this.tbcPaSchedeValori, "tbcPaSchedeValori");
            this.tbcPaSchedeValori.Name = "tbcPaSchedeValori";
            this.tbcPaSchedeValori.SelectedIndex = 0;
            // 
            // tbpPaGeneraleCiclo
            // 
            this.tbpPaGeneraleCiclo.Controls.Add(this.txtPaDescrizioneSetup);
            this.tbpPaGeneraleCiclo.Controls.Add(this.lblPaDescrizioneSetup);
            this.tbpPaGeneraleCiclo.Controls.Add(this.txtPaCassone);
            this.tbpPaGeneraleCiclo.Controls.Add(this.lblPaCassone);
            this.tbpPaGeneraleCiclo.Controls.Add(this.txtPaIdSetup);
            this.tbpPaGeneraleCiclo.Controls.Add(this.lblPaIdSetup);
            this.tbpPaGeneraleCiclo.Controls.Add(this.txtPaNomeSetup);
            this.tbpPaGeneraleCiclo.Controls.Add(this.lblPaNomeSetup);
            resources.ApplyResources(this.tbpPaGeneraleCiclo, "tbpPaGeneraleCiclo");
            this.tbpPaGeneraleCiclo.Name = "tbpPaGeneraleCiclo";
            this.tbpPaGeneraleCiclo.UseVisualStyleBackColor = true;
            // 
            // txtPaDescrizioneSetup
            // 
            resources.ApplyResources(this.txtPaDescrizioneSetup, "txtPaDescrizioneSetup");
            this.txtPaDescrizioneSetup.Name = "txtPaDescrizioneSetup";
            // 
            // lblPaDescrizioneSetup
            // 
            resources.ApplyResources(this.lblPaDescrizioneSetup, "lblPaDescrizioneSetup");
            this.lblPaDescrizioneSetup.Name = "lblPaDescrizioneSetup";
            // 
            // txtPaCassone
            // 
            resources.ApplyResources(this.txtPaCassone, "txtPaCassone");
            this.txtPaCassone.Name = "txtPaCassone";
            // 
            // lblPaCassone
            // 
            resources.ApplyResources(this.lblPaCassone, "lblPaCassone");
            this.lblPaCassone.Name = "lblPaCassone";
            // 
            // txtPaIdSetup
            // 
            resources.ApplyResources(this.txtPaIdSetup, "txtPaIdSetup");
            this.txtPaIdSetup.Name = "txtPaIdSetup";
            this.txtPaIdSetup.ReadOnly = true;
            // 
            // lblPaIdSetup
            // 
            resources.ApplyResources(this.lblPaIdSetup, "lblPaIdSetup");
            this.lblPaIdSetup.Name = "lblPaIdSetup";
            // 
            // txtPaNomeSetup
            // 
            resources.ApplyResources(this.txtPaNomeSetup, "txtPaNomeSetup");
            this.txtPaNomeSetup.Name = "txtPaNomeSetup";
            // 
            // lblPaNomeSetup
            // 
            resources.ApplyResources(this.lblPaNomeSetup, "lblPaNomeSetup");
            this.lblPaNomeSetup.Name = "lblPaNomeSetup";
            // 
            // tbpPaPCStep0
            // 
            this.tbpPaPCStep0.Controls.Add(this.txtPaDurataMaxT0);
            this.tbpPaPCStep0.Controls.Add(this.lblPaDurataMaxT0);
            this.tbpPaPCStep0.Controls.Add(this.txtPaPrefaseI0);
            this.tbpPaPCStep0.Controls.Add(this.lblPaPrefaseI0);
            this.tbpPaPCStep0.Controls.Add(this.txtPaSogliaV0);
            this.tbpPaPCStep0.Controls.Add(this.lblPaSogliaV0);
            resources.ApplyResources(this.tbpPaPCStep0, "tbpPaPCStep0");
            this.tbpPaPCStep0.Name = "tbpPaPCStep0";
            this.tbpPaPCStep0.UseVisualStyleBackColor = true;
            // 
            // txtPaDurataMaxT0
            // 
            resources.ApplyResources(this.txtPaDurataMaxT0, "txtPaDurataMaxT0");
            this.txtPaDurataMaxT0.Name = "txtPaDurataMaxT0";
            this.txtPaDurataMaxT0.Leave += new System.EventHandler(this.TxtPaDurataMaxT0_Leave);
            // 
            // lblPaDurataMaxT0
            // 
            resources.ApplyResources(this.lblPaDurataMaxT0, "lblPaDurataMaxT0");
            this.lblPaDurataMaxT0.Name = "lblPaDurataMaxT0";
            // 
            // txtPaPrefaseI0
            // 
            resources.ApplyResources(this.txtPaPrefaseI0, "txtPaPrefaseI0");
            this.txtPaPrefaseI0.Name = "txtPaPrefaseI0";
            this.txtPaPrefaseI0.Leave += new System.EventHandler(this.TxtPaPrefaseI0_Leave);
            // 
            // lblPaPrefaseI0
            // 
            resources.ApplyResources(this.lblPaPrefaseI0, "lblPaPrefaseI0");
            this.lblPaPrefaseI0.Name = "lblPaPrefaseI0";
            // 
            // txtPaSogliaV0
            // 
            resources.ApplyResources(this.txtPaSogliaV0, "txtPaSogliaV0");
            this.txtPaSogliaV0.Name = "txtPaSogliaV0";
            this.txtPaSogliaV0.Leave += new System.EventHandler(this.TxtPaSogliaV0_Leave);
            // 
            // lblPaSogliaV0
            // 
            resources.ApplyResources(this.lblPaSogliaV0, "lblPaSogliaV0");
            this.lblPaSogliaV0.Name = "lblPaSogliaV0";
            // 
            // tbpPaPCStep1
            // 
            this.tbpPaPCStep1.Controls.Add(this.cmbPaDurataMaxT1);
            this.tbpPaPCStep1.Controls.Add(this.cmbPaDurataCarica);
            this.tbpPaPCStep1.Controls.Add(this.lblPaDurataMaxT1);
            this.tbpPaPCStep1.Controls.Add(this.txtPaCorrenteI1);
            this.tbpPaPCStep1.Controls.Add(this.lblPaCorrenteI1);
            this.tbpPaPCStep1.Controls.Add(this.txtPaSogliaVs);
            this.tbpPaPCStep1.Controls.Add(this.lblPaSogliaVs);
            resources.ApplyResources(this.tbpPaPCStep1, "tbpPaPCStep1");
            this.tbpPaPCStep1.Name = "tbpPaPCStep1";
            this.tbpPaPCStep1.UseVisualStyleBackColor = true;
            // 
            // cmbPaDurataMaxT1
            // 
            resources.ApplyResources(this.cmbPaDurataMaxT1, "cmbPaDurataMaxT1");
            this.cmbPaDurataMaxT1.Name = "cmbPaDurataMaxT1";
            this.cmbPaDurataMaxT1.Leave += new System.EventHandler(this.CmbPaDurataMaxT1_Leave);
            // 
            // cmbPaDurataCarica
            // 
            resources.ApplyResources(this.cmbPaDurataCarica, "cmbPaDurataCarica");
            this.cmbPaDurataCarica.FormattingEnabled = true;
            this.cmbPaDurataCarica.Name = "cmbPaDurataCarica";
            this.cmbPaDurataCarica.SelectedIndexChanged += new System.EventHandler(this.cmbPaDurataCarica_SelectedIndexChanged_1);
            // 
            // lblPaDurataMaxT1
            // 
            resources.ApplyResources(this.lblPaDurataMaxT1, "lblPaDurataMaxT1");
            this.lblPaDurataMaxT1.Name = "lblPaDurataMaxT1";
            // 
            // txtPaCorrenteI1
            // 
            resources.ApplyResources(this.txtPaCorrenteI1, "txtPaCorrenteI1");
            this.txtPaCorrenteI1.Name = "txtPaCorrenteI1";
            this.txtPaCorrenteI1.Leave += new System.EventHandler(this.TxtPaCorrenteI1_Leave);
            // 
            // lblPaCorrenteI1
            // 
            resources.ApplyResources(this.lblPaCorrenteI1, "lblPaCorrenteI1");
            this.lblPaCorrenteI1.Name = "lblPaCorrenteI1";
            // 
            // txtPaSogliaVs
            // 
            resources.ApplyResources(this.txtPaSogliaVs, "txtPaSogliaVs");
            this.txtPaSogliaVs.Name = "txtPaSogliaVs";
            this.txtPaSogliaVs.Leave += new System.EventHandler(this.TxtPaSogliaVs_Leave);
            // 
            // lblPaSogliaVs
            // 
            resources.ApplyResources(this.lblPaSogliaVs, "lblPaSogliaVs");
            this.lblPaSogliaVs.Name = "lblPaSogliaVs";
            // 
            // tbpPaPCStep2
            // 
            this.tbpPaPCStep2.Controls.Add(this.txtPaTempoFin);
            this.tbpPaPCStep2.Controls.Add(this.lblPaTempoFin);
            this.tbpPaPCStep2.Controls.Add(this.txtPadT);
            this.tbpPaPCStep2.Controls.Add(this.lblPadT);
            this.tbpPaPCStep2.Controls.Add(this.txtPadV);
            this.tbpPaPCStep2.Controls.Add(this.lblPadV);
            this.tbpPaPCStep2.Controls.Add(this.lblPaCoeffKc);
            this.tbpPaPCStep2.Controls.Add(this.txtPaCoeffKc);
            this.tbpPaPCStep2.Controls.Add(this.txtPaVMax);
            this.tbpPaPCStep2.Controls.Add(this.lblPaVMax);
            this.tbpPaPCStep2.Controls.Add(this.lblPaCoeffK);
            this.tbpPaPCStep2.Controls.Add(this.txtPaCoeffK);
            this.tbpPaPCStep2.Controls.Add(this.lblPaTempoT2Max);
            this.tbpPaPCStep2.Controls.Add(this.txtPaTempoT2Max);
            this.tbpPaPCStep2.Controls.Add(this.txtPaTempoT2Min);
            this.tbpPaPCStep2.Controls.Add(this.lblPaTempoT2Min);
            this.tbpPaPCStep2.Controls.Add(this.txtPaCorrenteRaccordo);
            this.tbpPaPCStep2.Controls.Add(this.lblPaCorrenteRaccordo);
            this.tbpPaPCStep2.Controls.Add(this.txtPaCorrenteF3);
            this.tbpPaPCStep2.Controls.Add(this.lblPaCorrenteF3);
            this.tbpPaPCStep2.Controls.Add(this.txtPaRaccordoF1);
            this.tbpPaPCStep2.Controls.Add(this.lblPaRaccordoF1);
            resources.ApplyResources(this.tbpPaPCStep2, "tbpPaPCStep2");
            this.tbpPaPCStep2.Name = "tbpPaPCStep2";
            this.tbpPaPCStep2.UseVisualStyleBackColor = true;
            // 
            // txtPaTempoFin
            // 
            resources.ApplyResources(this.txtPaTempoFin, "txtPaTempoFin");
            this.txtPaTempoFin.Name = "txtPaTempoFin";
            // 
            // lblPaTempoFin
            // 
            resources.ApplyResources(this.lblPaTempoFin, "lblPaTempoFin");
            this.lblPaTempoFin.Name = "lblPaTempoFin";
            // 
            // txtPadT
            // 
            resources.ApplyResources(this.txtPadT, "txtPadT");
            this.txtPadT.Name = "txtPadT";
            // 
            // lblPadT
            // 
            resources.ApplyResources(this.lblPadT, "lblPadT");
            this.lblPadT.Name = "lblPadT";
            // 
            // txtPadV
            // 
            resources.ApplyResources(this.txtPadV, "txtPadV");
            this.txtPadV.Name = "txtPadV";
            // 
            // lblPadV
            // 
            resources.ApplyResources(this.lblPadV, "lblPadV");
            this.lblPadV.Name = "lblPadV";
            // 
            // lblPaCoeffKc
            // 
            resources.ApplyResources(this.lblPaCoeffKc, "lblPaCoeffKc");
            this.lblPaCoeffKc.Name = "lblPaCoeffKc";
            // 
            // txtPaCoeffKc
            // 
            resources.ApplyResources(this.txtPaCoeffKc, "txtPaCoeffKc");
            this.txtPaCoeffKc.Name = "txtPaCoeffKc";
            this.txtPaCoeffKc.Leave += new System.EventHandler(this.txtPaCoeffKc_Leave);
            // 
            // txtPaVMax
            // 
            resources.ApplyResources(this.txtPaVMax, "txtPaVMax");
            this.txtPaVMax.Name = "txtPaVMax";
            this.txtPaVMax.Leave += new System.EventHandler(this.TxtPaVMax_Leave);
            // 
            // lblPaVMax
            // 
            resources.ApplyResources(this.lblPaVMax, "lblPaVMax");
            this.lblPaVMax.Name = "lblPaVMax";
            // 
            // lblPaCoeffK
            // 
            resources.ApplyResources(this.lblPaCoeffK, "lblPaCoeffK");
            this.lblPaCoeffK.Name = "lblPaCoeffK";
            // 
            // txtPaCoeffK
            // 
            resources.ApplyResources(this.txtPaCoeffK, "txtPaCoeffK");
            this.txtPaCoeffK.Name = "txtPaCoeffK";
            this.txtPaCoeffK.Leave += new System.EventHandler(this.TxtPaCoeffK_Leave);
            // 
            // lblPaTempoT2Max
            // 
            resources.ApplyResources(this.lblPaTempoT2Max, "lblPaTempoT2Max");
            this.lblPaTempoT2Max.Name = "lblPaTempoT2Max";
            // 
            // txtPaTempoT2Max
            // 
            resources.ApplyResources(this.txtPaTempoT2Max, "txtPaTempoT2Max");
            this.txtPaTempoT2Max.Name = "txtPaTempoT2Max";
            this.txtPaTempoT2Max.Leave += new System.EventHandler(this.TxtPaTempoT2Max_Leave);
            // 
            // txtPaTempoT2Min
            // 
            resources.ApplyResources(this.txtPaTempoT2Min, "txtPaTempoT2Min");
            this.txtPaTempoT2Min.Name = "txtPaTempoT2Min";
            this.txtPaTempoT2Min.Leave += new System.EventHandler(this.TxtPaTempoT2Min_Leave);
            // 
            // lblPaTempoT2Min
            // 
            resources.ApplyResources(this.lblPaTempoT2Min, "lblPaTempoT2Min");
            this.lblPaTempoT2Min.Name = "lblPaTempoT2Min";
            // 
            // txtPaCorrenteRaccordo
            // 
            resources.ApplyResources(this.txtPaCorrenteRaccordo, "txtPaCorrenteRaccordo");
            this.txtPaCorrenteRaccordo.Name = "txtPaCorrenteRaccordo";
            this.txtPaCorrenteRaccordo.Leave += new System.EventHandler(this.TxtPaCorrenteRaccordo_Leave);
            // 
            // lblPaCorrenteRaccordo
            // 
            resources.ApplyResources(this.lblPaCorrenteRaccordo, "lblPaCorrenteRaccordo");
            this.lblPaCorrenteRaccordo.Name = "lblPaCorrenteRaccordo";
            // 
            // txtPaCorrenteF3
            // 
            resources.ApplyResources(this.txtPaCorrenteF3, "txtPaCorrenteF3");
            this.txtPaCorrenteF3.Name = "txtPaCorrenteF3";
            this.txtPaCorrenteF3.Leave += new System.EventHandler(this.TxtPaCorrenteF3_Leave);
            // 
            // lblPaCorrenteF3
            // 
            resources.ApplyResources(this.lblPaCorrenteF3, "lblPaCorrenteF3");
            this.lblPaCorrenteF3.Name = "lblPaCorrenteF3";
            // 
            // txtPaRaccordoF1
            // 
            resources.ApplyResources(this.txtPaRaccordoF1, "txtPaRaccordoF1");
            this.txtPaRaccordoF1.Name = "txtPaRaccordoF1";
            this.txtPaRaccordoF1.Leave += new System.EventHandler(this.TxtPaRaccordoF1_Leave);
            // 
            // lblPaRaccordoF1
            // 
            resources.ApplyResources(this.lblPaRaccordoF1, "lblPaRaccordoF1");
            this.lblPaRaccordoF1.Name = "lblPaRaccordoF1";
            // 
            // tbpPaPCStep3
            // 
            this.tbpPaPCStep3.Controls.Add(this.lblPaTempoT3Max);
            this.tbpPaPCStep3.Controls.Add(this.txtPaTempoT3Max);
            resources.ApplyResources(this.tbpPaPCStep3, "tbpPaPCStep3");
            this.tbpPaPCStep3.Name = "tbpPaPCStep3";
            this.tbpPaPCStep3.UseVisualStyleBackColor = true;
            // 
            // lblPaTempoT3Max
            // 
            resources.ApplyResources(this.lblPaTempoT3Max, "lblPaTempoT3Max");
            this.lblPaTempoT3Max.Name = "lblPaTempoT3Max";
            // 
            // txtPaTempoT3Max
            // 
            resources.ApplyResources(this.txtPaTempoT3Max, "txtPaTempoT3Max");
            this.txtPaTempoT3Max.Name = "txtPaTempoT3Max";
            this.txtPaTempoT3Max.Leave += new System.EventHandler(this.TxtPaTempoT3Max_Leave);
            // 
            // tbpPaPCEqual
            // 
            this.tbpPaPCEqual.Controls.Add(this.txtPaEqualPulseCurrent);
            this.tbpPaPCEqual.Controls.Add(this.lblPaEqualPulseCurrent);
            this.tbpPaPCEqual.Controls.Add(this.txtPaEqualPulseTime);
            this.tbpPaPCEqual.Controls.Add(this.lblPaEqualPulseTime);
            this.tbpPaPCEqual.Controls.Add(this.txtPaEqualPulsePause);
            this.tbpPaPCEqual.Controls.Add(this.lblPaEqualPulsePause);
            this.tbpPaPCEqual.Controls.Add(this.txtPaEqualNumPulse);
            this.tbpPaPCEqual.Controls.Add(this.lblPaEqualNumPulse);
            this.tbpPaPCEqual.Controls.Add(this.txtPaEqualAttesa);
            this.tbpPaPCEqual.Controls.Add(this.lblPaEqualAttesa);
            resources.ApplyResources(this.tbpPaPCEqual, "tbpPaPCEqual");
            this.tbpPaPCEqual.Name = "tbpPaPCEqual";
            this.tbpPaPCEqual.UseVisualStyleBackColor = true;
            // 
            // txtPaEqualPulseCurrent
            // 
            resources.ApplyResources(this.txtPaEqualPulseCurrent, "txtPaEqualPulseCurrent");
            this.txtPaEqualPulseCurrent.Name = "txtPaEqualPulseCurrent";
            // 
            // lblPaEqualPulseCurrent
            // 
            resources.ApplyResources(this.lblPaEqualPulseCurrent, "lblPaEqualPulseCurrent");
            this.lblPaEqualPulseCurrent.Name = "lblPaEqualPulseCurrent";
            // 
            // txtPaEqualPulseTime
            // 
            resources.ApplyResources(this.txtPaEqualPulseTime, "txtPaEqualPulseTime");
            this.txtPaEqualPulseTime.Name = "txtPaEqualPulseTime";
            // 
            // lblPaEqualPulseTime
            // 
            resources.ApplyResources(this.lblPaEqualPulseTime, "lblPaEqualPulseTime");
            this.lblPaEqualPulseTime.Name = "lblPaEqualPulseTime";
            // 
            // txtPaEqualPulsePause
            // 
            resources.ApplyResources(this.txtPaEqualPulsePause, "txtPaEqualPulsePause");
            this.txtPaEqualPulsePause.Name = "txtPaEqualPulsePause";
            // 
            // lblPaEqualPulsePause
            // 
            resources.ApplyResources(this.lblPaEqualPulsePause, "lblPaEqualPulsePause");
            this.lblPaEqualPulsePause.Name = "lblPaEqualPulsePause";
            // 
            // txtPaEqualNumPulse
            // 
            resources.ApplyResources(this.txtPaEqualNumPulse, "txtPaEqualNumPulse");
            this.txtPaEqualNumPulse.Name = "txtPaEqualNumPulse";
            // 
            // lblPaEqualNumPulse
            // 
            resources.ApplyResources(this.lblPaEqualNumPulse, "lblPaEqualNumPulse");
            this.lblPaEqualNumPulse.Name = "lblPaEqualNumPulse";
            // 
            // txtPaEqualAttesa
            // 
            resources.ApplyResources(this.txtPaEqualAttesa, "txtPaEqualAttesa");
            this.txtPaEqualAttesa.Name = "txtPaEqualAttesa";
            // 
            // lblPaEqualAttesa
            // 
            resources.ApplyResources(this.lblPaEqualAttesa, "lblPaEqualAttesa");
            this.lblPaEqualAttesa.Name = "lblPaEqualAttesa";
            // 
            // tbpPaPCMant
            // 
            this.tbpPaPCMant.Controls.Add(this.txtPaMantCorrente);
            this.tbpPaPCMant.Controls.Add(this.lblPaMantCorrente);
            this.tbpPaPCMant.Controls.Add(this.txtPaMantDurataMax);
            this.tbpPaPCMant.Controls.Add(this.lblPaMantDurataMax);
            this.tbpPaPCMant.Controls.Add(this.txtPaMantVmax);
            this.tbpPaPCMant.Controls.Add(this.lblPaMantVmax);
            this.tbpPaPCMant.Controls.Add(this.txtPaMantVmin);
            this.tbpPaPCMant.Controls.Add(this.lblPaMantVmin);
            this.tbpPaPCMant.Controls.Add(this.txtPaMantAttesa);
            this.tbpPaPCMant.Controls.Add(this.lblPaMantAttesa);
            resources.ApplyResources(this.tbpPaPCMant, "tbpPaPCMant");
            this.tbpPaPCMant.Name = "tbpPaPCMant";
            this.tbpPaPCMant.UseVisualStyleBackColor = true;
            // 
            // txtPaMantCorrente
            // 
            resources.ApplyResources(this.txtPaMantCorrente, "txtPaMantCorrente");
            this.txtPaMantCorrente.Name = "txtPaMantCorrente";
            // 
            // lblPaMantCorrente
            // 
            resources.ApplyResources(this.lblPaMantCorrente, "lblPaMantCorrente");
            this.lblPaMantCorrente.Name = "lblPaMantCorrente";
            // 
            // txtPaMantDurataMax
            // 
            resources.ApplyResources(this.txtPaMantDurataMax, "txtPaMantDurataMax");
            this.txtPaMantDurataMax.Name = "txtPaMantDurataMax";
            // 
            // lblPaMantDurataMax
            // 
            resources.ApplyResources(this.lblPaMantDurataMax, "lblPaMantDurataMax");
            this.lblPaMantDurataMax.Name = "lblPaMantDurataMax";
            // 
            // txtPaMantVmax
            // 
            resources.ApplyResources(this.txtPaMantVmax, "txtPaMantVmax");
            this.txtPaMantVmax.Name = "txtPaMantVmax";
            this.txtPaMantVmax.Leave += new System.EventHandler(this.TxtPaMantVmax_Leave);
            // 
            // lblPaMantVmax
            // 
            resources.ApplyResources(this.lblPaMantVmax, "lblPaMantVmax");
            this.lblPaMantVmax.Name = "lblPaMantVmax";
            // 
            // txtPaMantVmin
            // 
            resources.ApplyResources(this.txtPaMantVmin, "txtPaMantVmin");
            this.txtPaMantVmin.Name = "txtPaMantVmin";
            this.txtPaMantVmin.Leave += new System.EventHandler(this.TxtPaMantVmin_Leave);
            // 
            // lblPaMantVmin
            // 
            resources.ApplyResources(this.lblPaMantVmin, "lblPaMantVmin");
            this.lblPaMantVmin.Name = "lblPaMantVmin";
            // 
            // txtPaMantAttesa
            // 
            resources.ApplyResources(this.txtPaMantAttesa, "txtPaMantAttesa");
            this.txtPaMantAttesa.Name = "txtPaMantAttesa";
            // 
            // lblPaMantAttesa
            // 
            resources.ApplyResources(this.lblPaMantAttesa, "lblPaMantAttesa");
            this.lblPaMantAttesa.Name = "lblPaMantAttesa";
            // 
            // tbpPaPCOpp
            // 
            this.tbpPaPCOpp.Controls.Add(this.lblPaOppPuntoVerde);
            this.tbpPaPCOpp.Controls.Add(this.ImgPaOppPuntoVerde);
            this.tbpPaPCOpp.Controls.Add(this.chkPaOppNotturno);
            this.tbpPaPCOpp.Controls.Add(this.rslPaOppFinestra);
            this.tbpPaPCOpp.Controls.Add(this.txtPaOppDurataMax);
            this.tbpPaPCOpp.Controls.Add(this.lblPaOppDurataMax);
            this.tbpPaPCOpp.Controls.Add(this.txtPaOppCorrente);
            this.tbpPaPCOpp.Controls.Add(this.lblPaOppCorrente);
            this.tbpPaPCOpp.Controls.Add(this.txtPaOppVSoglia);
            this.tbpPaPCOpp.Controls.Add(this.lblPaOppVSoglia);
            this.tbpPaPCOpp.Controls.Add(this.txtPaOppOraFine);
            this.tbpPaPCOpp.Controls.Add(this.lblPaOppOraFine);
            this.tbpPaPCOpp.Controls.Add(this.txtPaOppOraInizio);
            this.tbpPaPCOpp.Controls.Add(this.lblPaOppOraInizio);
            resources.ApplyResources(this.tbpPaPCOpp, "tbpPaPCOpp");
            this.tbpPaPCOpp.Name = "tbpPaPCOpp";
            this.tbpPaPCOpp.UseVisualStyleBackColor = true;
            // 
            // lblPaOppPuntoVerde
            // 
            resources.ApplyResources(this.lblPaOppPuntoVerde, "lblPaOppPuntoVerde");
            this.lblPaOppPuntoVerde.Name = "lblPaOppPuntoVerde";
            // 
            // ImgPaOppPuntoVerde
            // 
            resources.ApplyResources(this.ImgPaOppPuntoVerde, "ImgPaOppPuntoVerde");
            this.ImgPaOppPuntoVerde.Name = "ImgPaOppPuntoVerde";
            this.ImgPaOppPuntoVerde.TabStop = false;
            // 
            // chkPaOppNotturno
            // 
            resources.ApplyResources(this.chkPaOppNotturno, "chkPaOppNotturno");
            this.chkPaOppNotturno.Name = "chkPaOppNotturno";
            this.chkPaOppNotturno.UseVisualStyleBackColor = true;
            this.chkPaOppNotturno.CheckedChanged += new System.EventHandler(this.ChkPaOppNotturno_CheckedChanged);
            // 
            // rslPaOppFinestra
            // 
            this.rslPaOppFinestra.BeforeTouchSize = new System.Drawing.Size(315, 22);
            resources.ApplyResources(this.rslPaOppFinestra, "rslPaOppFinestra");
            this.rslPaOppFinestra.ForeColor = System.Drawing.Color.Black;
            this.rslPaOppFinestra.HighlightedThumbColor = System.Drawing.Color.Blue;
            this.rslPaOppFinestra.Maximum = 1440;
            this.rslPaOppFinestra.Name = "rslPaOppFinestra";
            this.rslPaOppFinestra.PushedThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rslPaOppFinestra.ShowTicks = false;
            this.rslPaOppFinestra.SliderMax = 1200;
            this.rslPaOppFinestra.SliderMin = 240;
            this.rslPaOppFinestra.ThemeName = "Default";
            this.rslPaOppFinestra.TickColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(197)))), ((int)(((byte)(197)))));
            this.rslPaOppFinestra.TickFrequency = 15;
            this.rslPaOppFinestra.ValueChanged += new System.EventHandler(this.rslPaOppFinestra_ValueChanged);
            // 
            // txtPaOppDurataMax
            // 
            resources.ApplyResources(this.txtPaOppDurataMax, "txtPaOppDurataMax");
            this.txtPaOppDurataMax.Name = "txtPaOppDurataMax";
            // 
            // lblPaOppDurataMax
            // 
            resources.ApplyResources(this.lblPaOppDurataMax, "lblPaOppDurataMax");
            this.lblPaOppDurataMax.Name = "lblPaOppDurataMax";
            // 
            // txtPaOppCorrente
            // 
            resources.ApplyResources(this.txtPaOppCorrente, "txtPaOppCorrente");
            this.txtPaOppCorrente.Name = "txtPaOppCorrente";
            // 
            // lblPaOppCorrente
            // 
            resources.ApplyResources(this.lblPaOppCorrente, "lblPaOppCorrente");
            this.lblPaOppCorrente.Name = "lblPaOppCorrente";
            // 
            // txtPaOppVSoglia
            // 
            resources.ApplyResources(this.txtPaOppVSoglia, "txtPaOppVSoglia");
            this.txtPaOppVSoglia.Name = "txtPaOppVSoglia";
            // 
            // lblPaOppVSoglia
            // 
            resources.ApplyResources(this.lblPaOppVSoglia, "lblPaOppVSoglia");
            this.lblPaOppVSoglia.Name = "lblPaOppVSoglia";
            // 
            // txtPaOppOraFine
            // 
            resources.ApplyResources(this.txtPaOppOraFine, "txtPaOppOraFine");
            this.txtPaOppOraFine.Name = "txtPaOppOraFine";
            // 
            // lblPaOppOraFine
            // 
            resources.ApplyResources(this.lblPaOppOraFine, "lblPaOppOraFine");
            this.lblPaOppOraFine.Name = "lblPaOppOraFine";
            // 
            // txtPaOppOraInizio
            // 
            resources.ApplyResources(this.txtPaOppOraInizio, "txtPaOppOraInizio");
            this.txtPaOppOraInizio.Name = "txtPaOppOraInizio";
            // 
            // lblPaOppOraInizio
            // 
            resources.ApplyResources(this.lblPaOppOraInizio, "lblPaOppOraInizio");
            this.lblPaOppOraInizio.Name = "lblPaOppOraInizio";
            // 
            // tbpPaParSoglia
            // 
            this.tbpPaParSoglia.Controls.Add(this.txtPaTempLimite);
            this.tbpPaParSoglia.Controls.Add(this.lblPaTempLimite);
            this.tbpPaParSoglia.Controls.Add(this.txtPaVMinStop);
            this.tbpPaParSoglia.Controls.Add(this.lblPaVMinStop);
            this.tbpPaParSoglia.Controls.Add(this.txtPaBMSTempoAttesa);
            this.tbpPaParSoglia.Controls.Add(this.lblPaBMSTempoAttesa);
            this.tbpPaParSoglia.Controls.Add(this.txtPaBMSTempoErogazione);
            this.tbpPaParSoglia.Controls.Add(this.lblPaBMSTempoErogazione);
            this.tbpPaParSoglia.Controls.Add(this.chkPaRiarmaBms);
            this.tbpPaParSoglia.Controls.Add(this.chkPaAttivaRiarmoBms);
            this.tbpPaParSoglia.Controls.Add(this.txtPaCorrenteMassima);
            this.tbpPaParSoglia.Controls.Add(this.lblPaCorrenteMassima);
            this.tbpPaParSoglia.Controls.Add(this.txtPaVMaxRic);
            this.tbpPaParSoglia.Controls.Add(this.lblPaVMaxRic);
            this.tbpPaParSoglia.Controls.Add(this.txtPaVMinRic);
            this.tbpPaParSoglia.Controls.Add(this.lblPaVMinRic);
            this.tbpPaParSoglia.Controls.Add(this.txtPaVLimite);
            this.tbpPaParSoglia.Controls.Add(this.lblPaVLimite);
            resources.ApplyResources(this.tbpPaParSoglia, "tbpPaParSoglia");
            this.tbpPaParSoglia.Name = "tbpPaParSoglia";
            this.tbpPaParSoglia.UseVisualStyleBackColor = true;
            // 
            // txtPaTempLimite
            // 
            resources.ApplyResources(this.txtPaTempLimite, "txtPaTempLimite");
            this.txtPaTempLimite.Name = "txtPaTempLimite";
            // 
            // lblPaTempLimite
            // 
            resources.ApplyResources(this.lblPaTempLimite, "lblPaTempLimite");
            this.lblPaTempLimite.Name = "lblPaTempLimite";
            // 
            // txtPaVMinStop
            // 
            resources.ApplyResources(this.txtPaVMinStop, "txtPaVMinStop");
            this.txtPaVMinStop.Name = "txtPaVMinStop";
            // 
            // lblPaVMinStop
            // 
            resources.ApplyResources(this.lblPaVMinStop, "lblPaVMinStop");
            this.lblPaVMinStop.Name = "lblPaVMinStop";
            // 
            // txtPaBMSTempoAttesa
            // 
            resources.ApplyResources(this.txtPaBMSTempoAttesa, "txtPaBMSTempoAttesa");
            this.txtPaBMSTempoAttesa.Name = "txtPaBMSTempoAttesa";
            // 
            // lblPaBMSTempoAttesa
            // 
            resources.ApplyResources(this.lblPaBMSTempoAttesa, "lblPaBMSTempoAttesa");
            this.lblPaBMSTempoAttesa.Name = "lblPaBMSTempoAttesa";
            // 
            // txtPaBMSTempoErogazione
            // 
            resources.ApplyResources(this.txtPaBMSTempoErogazione, "txtPaBMSTempoErogazione");
            this.txtPaBMSTempoErogazione.Name = "txtPaBMSTempoErogazione";
            // 
            // lblPaBMSTempoErogazione
            // 
            resources.ApplyResources(this.lblPaBMSTempoErogazione, "lblPaBMSTempoErogazione");
            this.lblPaBMSTempoErogazione.Name = "lblPaBMSTempoErogazione";
            // 
            // chkPaRiarmaBms
            // 
            resources.ApplyResources(this.chkPaRiarmaBms, "chkPaRiarmaBms");
            this.chkPaRiarmaBms.Name = "chkPaRiarmaBms";
            this.chkPaRiarmaBms.Click += new System.EventHandler(this.chkPaRiarmaBms_Click);
            // 
            // chkPaAttivaRiarmoBms
            // 
            resources.ApplyResources(this.chkPaAttivaRiarmoBms, "chkPaAttivaRiarmoBms");
            this.chkPaAttivaRiarmoBms.Name = "chkPaAttivaRiarmoBms";
            this.chkPaAttivaRiarmoBms.UseVisualStyleBackColor = true;
            // 
            // txtPaCorrenteMassima
            // 
            resources.ApplyResources(this.txtPaCorrenteMassima, "txtPaCorrenteMassima");
            this.txtPaCorrenteMassima.Name = "txtPaCorrenteMassima";
            // 
            // lblPaCorrenteMassima
            // 
            resources.ApplyResources(this.lblPaCorrenteMassima, "lblPaCorrenteMassima");
            this.lblPaCorrenteMassima.Name = "lblPaCorrenteMassima";
            // 
            // txtPaVMaxRic
            // 
            resources.ApplyResources(this.txtPaVMaxRic, "txtPaVMaxRic");
            this.txtPaVMaxRic.Name = "txtPaVMaxRic";
            // 
            // lblPaVMaxRic
            // 
            resources.ApplyResources(this.lblPaVMaxRic, "lblPaVMaxRic");
            this.lblPaVMaxRic.Name = "lblPaVMaxRic";
            // 
            // txtPaVMinRic
            // 
            resources.ApplyResources(this.txtPaVMinRic, "txtPaVMinRic");
            this.txtPaVMinRic.Name = "txtPaVMinRic";
            // 
            // lblPaVMinRic
            // 
            resources.ApplyResources(this.lblPaVMinRic, "lblPaVMinRic");
            this.lblPaVMinRic.Name = "lblPaVMinRic";
            // 
            // txtPaVLimite
            // 
            resources.ApplyResources(this.txtPaVLimite, "txtPaVLimite");
            this.txtPaVLimite.Name = "txtPaVLimite";
            // 
            // lblPaVLimite
            // 
            resources.ApplyResources(this.lblPaVLimite, "lblPaVLimite");
            this.lblPaVLimite.Name = "lblPaVLimite";
            // 
            // chkPaUsaSpyBatt
            // 
            resources.ApplyResources(this.chkPaUsaSpyBatt, "chkPaUsaSpyBatt");
            this.chkPaUsaSpyBatt.Name = "chkPaUsaSpyBatt";
            this.chkPaUsaSpyBatt.UseVisualStyleBackColor = true;
            this.chkPaUsaSpyBatt.CheckedChanged += new System.EventHandler(this.ChkPaUsaSpyBatt_CheckedChanged);
            // 
            // label69
            // 
            resources.ApplyResources(this.label69, "label69");
            this.label69.ForeColor = System.Drawing.Color.Red;
            this.label69.Name = "label69";
            // 
            // btnPaSalvaDati
            // 
            resources.ApplyResources(this.btnPaSalvaDati, "btnPaSalvaDati");
            this.btnPaSalvaDati.Name = "btnPaSalvaDati";
            this.btnPaSalvaDati.UseVisualStyleBackColor = true;
            this.btnPaSalvaDati.Click += new System.EventHandler(this.btnPaSalvaDati_Click);
            // 
            // lblPaTitoloLista
            // 
            resources.ApplyResources(this.lblPaTitoloLista, "lblPaTitoloLista");
            this.lblPaTitoloLista.Name = "lblPaTitoloLista";
            // 
            // tabGenerale
            // 
            this.tabGenerale.BackColor = System.Drawing.Color.LightYellow;
            this.tabGenerale.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tabGenerale, "tabGenerale");
            this.tabGenerale.Controls.Add(this.pictureBox1);
            this.tabGenerale.Controls.Add(this.btnGenResetBoard);
            this.tabGenerale.Controls.Add(this.btnGenAzzzeraContatoriTot);
            this.tabGenerale.Controls.Add(this.btnGenAzzzeraContatori);
            this.tabGenerale.Controls.Add(this.btnCaricaContatori);
            this.tabGenerale.Controls.Add(this.GrbMainDatiApparato);
            this.tabGenerale.Name = "tabGenerale";
            this.tabGenerale.Click += new System.EventHandler(this.tabCb01_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // btnGenResetBoard
            // 
            resources.ApplyResources(this.btnGenResetBoard, "btnGenResetBoard");
            this.btnGenResetBoard.ForeColor = System.Drawing.Color.Red;
            this.btnGenResetBoard.Name = "btnGenResetBoard";
            this.btnGenResetBoard.UseVisualStyleBackColor = true;
            this.btnGenResetBoard.Click += new System.EventHandler(this.BtnGenResetBoard_Click);
            // 
            // btnGenAzzzeraContatoriTot
            // 
            resources.ApplyResources(this.btnGenAzzzeraContatoriTot, "btnGenAzzzeraContatoriTot");
            this.btnGenAzzzeraContatoriTot.Name = "btnGenAzzzeraContatoriTot";
            this.btnGenAzzzeraContatoriTot.UseVisualStyleBackColor = true;
            this.btnGenAzzzeraContatoriTot.Click += new System.EventHandler(this.BtnGenAzzeraContatoriTot_Click);
            // 
            // btnGenAzzzeraContatori
            // 
            resources.ApplyResources(this.btnGenAzzzeraContatori, "btnGenAzzzeraContatori");
            this.btnGenAzzzeraContatori.Name = "btnGenAzzzeraContatori";
            this.btnGenAzzzeraContatori.UseVisualStyleBackColor = true;
            this.btnGenAzzzeraContatori.Click += new System.EventHandler(this.BtnGenAzzeraContatori_Click);
            // 
            // btnCaricaContatori
            // 
            resources.ApplyResources(this.btnCaricaContatori, "btnCaricaContatori");
            this.btnCaricaContatori.Name = "btnCaricaContatori";
            this.btnCaricaContatori.UseVisualStyleBackColor = true;
            this.btnCaricaContatori.Click += new System.EventHandler(this.btnCaricaContatori_Click);
            // 
            // GrbMainDatiApparato
            // 
            this.GrbMainDatiApparato.BackColor = System.Drawing.Color.White;
            this.GrbMainDatiApparato.Controls.Add(this.cmbGenModelloCB);
            this.GrbMainDatiApparato.Controls.Add(this.label1);
            this.GrbMainDatiApparato.Controls.Add(this.txtGenCorrenteMax);
            this.GrbMainDatiApparato.Controls.Add(this.label143);
            this.GrbMainDatiApparato.Controls.Add(this.txtGenTensioneMax);
            this.GrbMainDatiApparato.Controls.Add(this.label142);
            this.GrbMainDatiApparato.Controls.Add(this.txtGenIdApparato);
            this.GrbMainDatiApparato.Controls.Add(this.label137);
            this.GrbMainDatiApparato.Controls.Add(this.label8);
            this.GrbMainDatiApparato.Controls.Add(this.txtGenAnnoMatricola);
            this.GrbMainDatiApparato.Controls.Add(this.txtGenMatricola);
            this.GrbMainDatiApparato.Controls.Add(this.lblMatricola);
            resources.ApplyResources(this.GrbMainDatiApparato, "GrbMainDatiApparato");
            this.GrbMainDatiApparato.Name = "GrbMainDatiApparato";
            this.GrbMainDatiApparato.TabStop = false;
            // 
            // cmbGenModelloCB
            // 
            this.cmbGenModelloCB.BackColor = System.Drawing.Color.White;
            this.cmbGenModelloCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbGenModelloCB, "cmbGenModelloCB");
            this.cmbGenModelloCB.FormattingEnabled = true;
            this.cmbGenModelloCB.Name = "cmbGenModelloCB";
            this.cmbGenModelloCB.SelectedIndexChanged += new System.EventHandler(this.cmbGenModelloCB_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Name = "label1";
            // 
            // txtGenCorrenteMax
            // 
            resources.ApplyResources(this.txtGenCorrenteMax, "txtGenCorrenteMax");
            this.txtGenCorrenteMax.Name = "txtGenCorrenteMax";
            this.txtGenCorrenteMax.ReadOnly = true;
            // 
            // label143
            // 
            resources.ApplyResources(this.label143, "label143");
            this.label143.Name = "label143";
            // 
            // txtGenTensioneMax
            // 
            resources.ApplyResources(this.txtGenTensioneMax, "txtGenTensioneMax");
            this.txtGenTensioneMax.Name = "txtGenTensioneMax";
            this.txtGenTensioneMax.ReadOnly = true;
            // 
            // label142
            // 
            resources.ApplyResources(this.label142, "label142");
            this.label142.Name = "label142";
            // 
            // txtGenIdApparato
            // 
            resources.ApplyResources(this.txtGenIdApparato, "txtGenIdApparato");
            this.txtGenIdApparato.Name = "txtGenIdApparato";
            this.txtGenIdApparato.ReadOnly = true;
            // 
            // label137
            // 
            resources.ApplyResources(this.label137, "label137");
            this.label137.Name = "label137";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // txtGenAnnoMatricola
            // 
            resources.ApplyResources(this.txtGenAnnoMatricola, "txtGenAnnoMatricola");
            this.txtGenAnnoMatricola.Name = "txtGenAnnoMatricola";
            this.txtGenAnnoMatricola.ReadOnly = true;
            // 
            // txtGenMatricola
            // 
            resources.ApplyResources(this.txtGenMatricola, "txtGenMatricola");
            this.txtGenMatricola.Name = "txtGenMatricola";
            this.txtGenMatricola.ReadOnly = true;
            // 
            // lblMatricola
            // 
            resources.ApplyResources(this.lblMatricola, "lblMatricola");
            this.lblMatricola.Name = "lblMatricola";
            // 
            // tabCaricaBatterie
            // 
            this.tabCaricaBatterie.Controls.Add(this.tabGenerale);
            this.tabCaricaBatterie.Controls.Add(this.tabProfiloAttuale);
            resources.ApplyResources(this.tabCaricaBatterie, "tabCaricaBatterie");
            this.tabCaricaBatterie.Name = "tabCaricaBatterie";
            this.tabCaricaBatterie.SelectedIndex = 0;
            this.tabCaricaBatterie.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabCaricaBatterie_Selected);
            // 
            // frmIdBatt
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuBar;
            this.Controls.Add(this.tabCaricaBatterie);
            this.Name = "frmIdBatt";
            this.ShowIcon = false;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCaricabatterie_FormClosed);
            this.Load += new System.EventHandler(this.frmCaricabatterie_Load);
            this.Resize += new System.EventHandler(this.frmCaricabatterie_Resize);
            this.tabProfiloAttuale.ResumeLayout(false);
            this.tabProfiloAttuale.PerformLayout();
            this.tbcPaSottopagina.ResumeLayout(false);
            this.tbpPaListaProfili.ResumeLayout(false);
            this.grbPaCaricaFile.ResumeLayout(false);
            this.grbPaCaricaFile.PerformLayout();
            this.grbPaGeneraFile.ResumeLayout(false);
            this.grbPaGeneraFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flwPaListaConfigurazioni)).EndInit();
            this.tbpPaProfiloAttivo.ResumeLayout(false);
            this.pippo.ResumeLayout(false);
            this.pippo.PerformLayout();
            this.grbPaImpostazioniLocali.ResumeLayout(false);
            this.grbPaImpostazioniLocali.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPaImmagineProfilo)).EndInit();
            this.tbcPaSchedeValori.ResumeLayout(false);
            this.tbpPaGeneraleCiclo.ResumeLayout(false);
            this.tbpPaGeneraleCiclo.PerformLayout();
            this.tbpPaPCStep0.ResumeLayout(false);
            this.tbpPaPCStep0.PerformLayout();
            this.tbpPaPCStep1.ResumeLayout(false);
            this.tbpPaPCStep1.PerformLayout();
            this.tbpPaPCStep2.ResumeLayout(false);
            this.tbpPaPCStep2.PerformLayout();
            this.tbpPaPCStep3.ResumeLayout(false);
            this.tbpPaPCStep3.PerformLayout();
            this.tbpPaPCEqual.ResumeLayout(false);
            this.tbpPaPCEqual.PerformLayout();
            this.tbpPaPCMant.ResumeLayout(false);
            this.tbpPaPCMant.PerformLayout();
            this.tbpPaPCOpp.ResumeLayout(false);
            this.tbpPaPCOpp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImgPaOppPuntoVerde)).EndInit();
            this.tbpPaParSoglia.ResumeLayout(false);
            this.tbpPaParSoglia.PerformLayout();
            this.tabGenerale.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.GrbMainDatiApparato.ResumeLayout(false);
            this.GrbMainDatiApparato.PerformLayout();
            this.tabCaricaBatterie.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrLetturaAutomatica;
        private System.Windows.Forms.SaveFileDialog sfdExportDati;
        private System.Windows.Forms.OpenFileDialog sfdImportDati;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabProfiloAttuale;
        private System.Windows.Forms.TabControl tbcPaSottopagina;
        private System.Windows.Forms.TabPage tbpPaProfiloAttivo;
        private System.Windows.Forms.GroupBox pippo;
        private System.Windows.Forms.Button btnPaProfileClear;
        private System.Windows.Forms.GroupBox grbPaImpostazioniLocali;
        private System.Windows.Forms.Button btnPaProfileNEW;
        private System.Windows.Forms.CheckBox chkPaUsaSafety;
        private System.Windows.Forms.Label lblPaUsaSafety;
        private System.Windows.Forms.ComboBox cmbPaProfilo;
        private System.Windows.Forms.TextBox txtPaCapacita;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkPaAttivaOppChg;
        private System.Windows.Forms.Label lblPaAttivaOppChg;
        private System.Windows.Forms.CheckBox chkPaAttivaMant;
        private System.Windows.Forms.Label lblPaAttivaMant;
        private System.Windows.Forms.TextBox txtPaNumCelle;
        private System.Windows.Forms.Label lblPaNumCelle;
        private System.Windows.Forms.ComboBox cmbPaTipoBatteria;
        private System.Windows.Forms.Label lblPaTipoBatteria;
        private System.Windows.Forms.CheckBox chkPaAttivaEqual;
        private System.Windows.Forms.Label lblPaAttivaEqual;
        private System.Windows.Forms.Label lblPaTensione;
        private System.Windows.Forms.ComboBox cmbPaTensione;
        private System.Windows.Forms.TextBox txtPaTensione;
        private System.Windows.Forms.Button btnPaCaricaCicli;
        private System.Windows.Forms.CheckBox chkPaSbloccaValori;
        private System.Windows.Forms.Label lblPaSbloccaValori;
        private System.Windows.Forms.Button btnPaProfileRefresh;
        private System.Windows.Forms.PictureBox picPaImmagineProfilo;
        private System.Windows.Forms.TabControl tbcPaSchedeValori;
        private System.Windows.Forms.TabPage tbpPaGeneraleCiclo;
        private System.Windows.Forms.TextBox txtPaCassone;
        private System.Windows.Forms.Label lblPaCassone;
        private System.Windows.Forms.TextBox txtPaIdSetup;
        private System.Windows.Forms.Label lblPaIdSetup;
        private System.Windows.Forms.TextBox txtPaNomeSetup;
        private System.Windows.Forms.Label lblPaNomeSetup;
        private System.Windows.Forms.TabPage tbpPaPCStep0;
        private System.Windows.Forms.TextBox txtPaDurataMaxT0;
        private System.Windows.Forms.Label lblPaDurataMaxT0;
        private System.Windows.Forms.TextBox txtPaPrefaseI0;
        private System.Windows.Forms.Label lblPaPrefaseI0;
        private System.Windows.Forms.TextBox txtPaSogliaV0;
        private System.Windows.Forms.Label lblPaSogliaV0;
        private System.Windows.Forms.TabPage tbpPaPCStep1;
        private System.Windows.Forms.TextBox cmbPaDurataMaxT1;
        private System.Windows.Forms.ComboBox cmbPaDurataCarica;
        private System.Windows.Forms.Label lblPaDurataMaxT1;
        private System.Windows.Forms.TextBox txtPaCorrenteI1;
        private System.Windows.Forms.Label lblPaCorrenteI1;
        private System.Windows.Forms.TextBox txtPaSogliaVs;
        private System.Windows.Forms.Label lblPaSogliaVs;
        private System.Windows.Forms.TabPage tbpPaPCStep2;
        private System.Windows.Forms.TextBox txtPaTempoFin;
        private System.Windows.Forms.Label lblPaTempoFin;
        private System.Windows.Forms.TextBox txtPadT;
        private System.Windows.Forms.Label lblPadT;
        private System.Windows.Forms.TextBox txtPadV;
        private System.Windows.Forms.Label lblPadV;
        private System.Windows.Forms.Label lblPaCoeffKc;
        private System.Windows.Forms.TextBox txtPaCoeffKc;
        private System.Windows.Forms.TextBox txtPaVMax;
        private System.Windows.Forms.Label lblPaVMax;
        private System.Windows.Forms.Label lblPaCoeffK;
        private System.Windows.Forms.TextBox txtPaCoeffK;
        private System.Windows.Forms.Label lblPaTempoT2Max;
        private System.Windows.Forms.TextBox txtPaTempoT2Max;
        private System.Windows.Forms.TextBox txtPaTempoT2Min;
        private System.Windows.Forms.Label lblPaTempoT2Min;
        private System.Windows.Forms.TextBox txtPaCorrenteRaccordo;
        private System.Windows.Forms.Label lblPaCorrenteRaccordo;
        private System.Windows.Forms.TextBox txtPaCorrenteF3;
        private System.Windows.Forms.Label lblPaCorrenteF3;
        private System.Windows.Forms.TextBox txtPaRaccordoF1;
        private System.Windows.Forms.Label lblPaRaccordoF1;
        private System.Windows.Forms.TabPage tbpPaPCStep3;
        private System.Windows.Forms.Label lblPaTempoT3Max;
        private System.Windows.Forms.TextBox txtPaTempoT3Max;
        private System.Windows.Forms.TabPage tbpPaPCEqual;
        private System.Windows.Forms.TextBox txtPaEqualPulseCurrent;
        private System.Windows.Forms.Label lblPaEqualPulseCurrent;
        private System.Windows.Forms.TextBox txtPaEqualPulseTime;
        private System.Windows.Forms.Label lblPaEqualPulseTime;
        private System.Windows.Forms.TextBox txtPaEqualPulsePause;
        private System.Windows.Forms.Label lblPaEqualPulsePause;
        private System.Windows.Forms.TextBox txtPaEqualNumPulse;
        private System.Windows.Forms.Label lblPaEqualNumPulse;
        private System.Windows.Forms.TextBox txtPaEqualAttesa;
        private System.Windows.Forms.Label lblPaEqualAttesa;
        private System.Windows.Forms.TabPage tbpPaPCMant;
        private System.Windows.Forms.TextBox txtPaMantCorrente;
        private System.Windows.Forms.Label lblPaMantCorrente;
        private System.Windows.Forms.TextBox txtPaMantDurataMax;
        private System.Windows.Forms.Label lblPaMantDurataMax;
        private System.Windows.Forms.TextBox txtPaMantVmax;
        private System.Windows.Forms.Label lblPaMantVmax;
        private System.Windows.Forms.TextBox txtPaMantVmin;
        private System.Windows.Forms.Label lblPaMantVmin;
        private System.Windows.Forms.TextBox txtPaMantAttesa;
        private System.Windows.Forms.Label lblPaMantAttesa;
        private System.Windows.Forms.TabPage tbpPaPCOpp;
        private System.Windows.Forms.Label lblPaOppPuntoVerde;
        private System.Windows.Forms.PictureBox ImgPaOppPuntoVerde;
        private System.Windows.Forms.CheckBox chkPaOppNotturno;
        private Syncfusion.Windows.Forms.Tools.RangeSlider rslPaOppFinestra;
        private System.Windows.Forms.TextBox txtPaOppDurataMax;
        private System.Windows.Forms.Label lblPaOppDurataMax;
        private System.Windows.Forms.TextBox txtPaOppCorrente;
        private System.Windows.Forms.Label lblPaOppCorrente;
        private System.Windows.Forms.TextBox txtPaOppVSoglia;
        private System.Windows.Forms.Label lblPaOppVSoglia;
        private System.Windows.Forms.TextBox txtPaOppOraFine;
        private System.Windows.Forms.Label lblPaOppOraFine;
        private System.Windows.Forms.TextBox txtPaOppOraInizio;
        private System.Windows.Forms.Label lblPaOppOraInizio;
        private System.Windows.Forms.TabPage tbpPaParSoglia;
        private System.Windows.Forms.TextBox txtPaTempLimite;
        private System.Windows.Forms.Label lblPaTempLimite;
        private System.Windows.Forms.TextBox txtPaVMinStop;
        private System.Windows.Forms.Label lblPaVMinStop;
        private System.Windows.Forms.TextBox txtPaBMSTempoAttesa;
        private System.Windows.Forms.Label lblPaBMSTempoAttesa;
        private System.Windows.Forms.TextBox txtPaBMSTempoErogazione;
        private System.Windows.Forms.Label lblPaBMSTempoErogazione;
        private System.Windows.Forms.Label chkPaRiarmaBms;
        private System.Windows.Forms.CheckBox chkPaAttivaRiarmoBms;
        private System.Windows.Forms.TextBox txtPaCorrenteMassima;
        private System.Windows.Forms.Label lblPaCorrenteMassima;
        private System.Windows.Forms.TextBox txtPaVMaxRic;
        private System.Windows.Forms.Label lblPaVMaxRic;
        private System.Windows.Forms.TextBox txtPaVMinRic;
        private System.Windows.Forms.Label lblPaVMinRic;
        private System.Windows.Forms.TextBox txtPaVLimite;
        private System.Windows.Forms.Label lblPaVLimite;
        private System.Windows.Forms.CheckBox chkPaUsaSpyBatt;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Button btnPaSalvaDati;
        private System.Windows.Forms.TabPage tbpPaListaProfili;
        private System.Windows.Forms.Button btnPaCancellaSelezionati;
        private System.Windows.Forms.GroupBox grbPaGeneraFile;
        private System.Windows.Forms.Button btnPaSalvaFile;
        private System.Windows.Forms.CheckBox chkPaSoloSelezionati;
        private System.Windows.Forms.Button btnPaNomeFileProfiliSRC;
        private System.Windows.Forms.TextBox txtPaNomeFileProfili;
        private System.Windows.Forms.Button btnPaAttivaConfigurazione;
        private BrightIdeasSoftware.FastObjectListView flwPaListaConfigurazioni;
        private System.Windows.Forms.Button btnPaCaricaListaProfili;
        private System.Windows.Forms.Label lblPaTitoloLista;
        private System.Windows.Forms.TabPage tabGenerale;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnGenResetBoard;
        private System.Windows.Forms.Button btnGenAzzzeraContatoriTot;
        private System.Windows.Forms.Button btnGenAzzzeraContatori;
        private System.Windows.Forms.Button btnCaricaContatori;
        private System.Windows.Forms.GroupBox GrbMainDatiApparato;
        private System.Windows.Forms.TextBox txtGenCorrenteMax;
        private System.Windows.Forms.Label label143;
        private System.Windows.Forms.TextBox txtGenTensioneMax;
        private System.Windows.Forms.TabControl tabCaricaBatterie;
        private System.Windows.Forms.ComboBox cmbGenModelloCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label142;
        private System.Windows.Forms.TextBox txtGenIdApparato;
        private System.Windows.Forms.Label label137;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtGenAnnoMatricola;
        private System.Windows.Forms.TextBox txtGenMatricola;
        private System.Windows.Forms.Label lblMatricola;
        private System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Button btnPaGeneraQr;
        private System.Windows.Forms.TextBox txtPaDescrizioneSetup;
        private System.Windows.Forms.Label lblPaDescrizioneSetup;
        private System.Windows.Forms.GroupBox grbPaCaricaFile;
        private System.Windows.Forms.Button btnPaCaricaFile;
        private System.Windows.Forms.Button btnPaFileInProfiliSRC;
        private System.Windows.Forms.TextBox txtPaCaricaFileProfili;
        private System.Windows.Forms.Button btnPaCopiaSelezionati;
    }
}