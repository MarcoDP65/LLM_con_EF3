namespace PannelloCharger
{
    partial class frmDisplayManager
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
            this.tbcMainDisplayManager = new System.Windows.Forms.TabControl();
            this.tbpConnessione = new System.Windows.Forms.TabPage();
            this.pnlComandiImmediati = new System.Windows.Forms.Panel();
            this.chkRtRiavvioAutomatico = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnPrimaLettura = new System.Windows.Forms.Button();
            this.cmdLeggiRTC = new System.Windows.Forms.Button();
            this.btnApriComunicazione = new System.Windows.Forms.Button();
            this.label77 = new System.Windows.Forms.Label();
            this.btnRtSetBaudRate = new System.Windows.Forms.Button();
            this.cmbRtBaudRate = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPorta = new System.Windows.Forms.Label();
            this.rtbhex = new System.Windows.Forms.RichTextBox();
            this.rtbOutgoing = new System.Windows.Forms.RichTextBox();
            this.lblRIStatus = new System.Windows.Forms.Label();
            this.lblDSRStatus = new System.Windows.Forms.Label();
            this.lblCTSStatus = new System.Windows.Forms.Label();
            this.lblBreakStatus = new System.Windows.Forms.Label();
            this.rtbIncoming = new System.Windows.Forms.RichTextBox();
            this.btnPortState = new System.Windows.Forms.Button();
            this.cboHandShaking = new System.Windows.Forms.ComboBox();
            this.cboParity = new System.Windows.Forms.ComboBox();
            this.cboStopBits = new System.Windows.Forms.ComboBox();
            this.cboDataBits = new System.Windows.Forms.ComboBox();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.cboPorts = new System.Windows.Forms.ComboBox();
            this.btnGetSerialPorts = new System.Windows.Forms.Button();
            this.tbpArchivioModelli = new System.Windows.Forms.TabPage();
            this.grbModInvioModello = new System.Windows.Forms.GroupBox();
            this.txtModVariabiliTrasmesse = new System.Windows.Forms.TextBox();
            this.btnModInviaVariabili = new System.Windows.Forms.Button();
            this.pgbModStatoInvio = new System.Windows.Forms.ProgressBar();
            this.btnModAggiornaDisplay = new System.Windows.Forms.Button();
            this.txtModSchermateTrasmesse = new System.Windows.Forms.TextBox();
            this.txtModImmaginiTrasmesse = new System.Windows.Forms.TextBox();
            this.btnModInviaSchermate = new System.Windows.Forms.Button();
            this.btnModInviaImmagini = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnModNuovoModello = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label37 = new System.Windows.Forms.Label();
            this.txtModNumVar = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.txtModNumDis = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.txtModNumImg = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.txtModDataMod = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.txtModDataCre = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.txtModNote = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.txtModVersione = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.txtModNome = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnModCercaCaricaModello = new System.Windows.Forms.Button();
            this.btnModCercaSalvaModello = new System.Windows.Forms.Button();
            this.btnModCaricaModello = new System.Windows.Forms.Button();
            this.btnModSalvaModello = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtModNomeFile = new System.Windows.Forms.TextBox();
            this.tbpSchermate = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cmdSchNuovaSchermata = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnSchCmdNew = new System.Windows.Forms.Button();
            this.btnSchCmdLoad = new System.Windows.Forms.Button();
            this.btnSchCmdDel = new System.Windows.Forms.Button();
            this.panel14 = new System.Windows.Forms.Panel();
            this.btnSchCaricaFile = new System.Windows.Forms.Button();
            this.btnSchCercaFile = new System.Windows.Forms.Button();
            this.txtSchNuovoFile = new System.Windows.Forms.TextBox();
            this.panel13 = new System.Windows.Forms.Panel();
            this.cmbSchIdVariabile = new System.Windows.Forms.ComboBox();
            this.label75 = new System.Windows.Forms.Label();
            this.txtSchCmdTempoOFF = new System.Windows.Forms.TextBox();
            this.label76 = new System.Windows.Forms.Label();
            this.txtSchCmdTempoON = new System.Windows.Forms.TextBox();
            this.btnSchCmdAdd = new System.Windows.Forms.Button();
            this.label74 = new System.Windows.Forms.Label();
            this.txtSchCmdNum = new System.Windows.Forms.TextBox();
            this.label73 = new System.Windows.Forms.Label();
            this.txtSchCmdText = new System.Windows.Forms.TextBox();
            this.label71 = new System.Windows.Forms.Label();
            this.txtSchCmdLenVarChar = new System.Windows.Forms.TextBox();
            this.txtSchCmdNumImg = new System.Windows.Forms.TextBox();
            this.label72 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.txtSchCmdLenVarPix = new System.Windows.Forms.TextBox();
            this.txtSchCmdNumVar = new System.Windows.Forms.TextBox();
            this.label70 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.txtSchCmdColor = new System.Windows.Forms.TextBox();
            this.label66 = new System.Windows.Forms.Label();
            this.txtSchCmdPosY = new System.Windows.Forms.TextBox();
            this.txtSchCmdPosX = new System.Windows.Forms.TextBox();
            this.label67 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.txtSchCmdHeigh = new System.Windows.Forms.TextBox();
            this.txtSchCmdWidth = new System.Windows.Forms.TextBox();
            this.label65 = new System.Windows.Forms.Label();
            this.cmbSchTipoComando = new System.Windows.Forms.ComboBox();
            this.flvSchListaComandi = new BrightIdeasSoftware.FastObjectListView();
            this.label53 = new System.Windows.Forms.Label();
            this.label51 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cmdSchGeneraByteArray = new System.Windows.Forms.Button();
            this.cmdSchMostrtaSch = new System.Windows.Forms.Button();
            this.cmdSchRimuoviElemento = new System.Windows.Forms.Button();
            this.cmdSchInviaSch = new System.Windows.Forms.Button();
            this.pnlSchImmagineSchermata = new System.Windows.Forms.Panel();
            this.txtSchBaseNomeLista = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.txtSchBaseID = new System.Windows.Forms.TextBox();
            this.label59 = new System.Windows.Forms.Label();
            this.cmdSchGeneraClasse = new System.Windows.Forms.Button();
            this.label60 = new System.Windows.Forms.Label();
            this.txtSchBaseHeigh = new System.Windows.Forms.TextBox();
            this.txtSchBaseWidth = new System.Windows.Forms.TextBox();
            this.label61 = new System.Windows.Forms.Label();
            this.txtSchBaseSize = new System.Windows.Forms.TextBox();
            this.label62 = new System.Windows.Forms.Label();
            this.txtSchBaseName = new System.Windows.Forms.TextBox();
            this.label63 = new System.Windows.Forms.Label();
            this.pbxSchImmagine = new System.Windows.Forms.PictureBox();
            this.label52 = new System.Windows.Forms.Label();
            this.flvSchListaSchermate = new BrightIdeasSoftware.FastObjectListView();
            this.tbpImmagini = new System.Windows.Forms.TabPage();
            this.label42 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btnImgMostraImmagine = new System.Windows.Forms.Button();
            this.btnImgRimuoviImmagine = new System.Windows.Forms.Button();
            this.btnImgInviaImmagine = new System.Windows.Forms.Button();
            this.flvImgListaImmagini = new BrightIdeasSoftware.FastObjectListView();
            this.grbGeneraExcel = new System.Windows.Forms.GroupBox();
            this.btnSfoglia = new System.Windows.Forms.Button();
            this.txtNuovoFile = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtImgNomeImmagineLista = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.txtImgIdImmagine = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.txtImgBaseDimY = new System.Windows.Forms.TextBox();
            this.txtImgBaseDimX = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txtImgBaseSize = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.btnImgGeneraClasse = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.txtImgDimY = new System.Windows.Forms.TextBox();
            this.txtImgDimX = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtImgDimImmagine = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtImgNomeImmagine = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.btnImgGeneraArrayImmagine = new System.Windows.Forms.Button();
            this.btnImgSimulaFileImmagine = new System.Windows.Forms.Button();
            this.pbxImgImmagine1b = new System.Windows.Forms.PictureBox();
            this.pbxImgImmagine8b = new System.Windows.Forms.PictureBox();
            this.btnImgCaricaFileImmagine = new System.Windows.Forms.Button();
            this.pbxImgImmagine = new System.Windows.Forms.PictureBox();
            this.tbpVariabili = new System.Windows.Forms.TabPage();
            this.panel10 = new System.Windows.Forms.Panel();
            this.btnVarSalvaValore = new System.Windows.Forms.Button();
            this.txtVarValore = new System.Windows.Forms.TextBox();
            this.btnVarCancellaVariabile = new System.Windows.Forms.Button();
            this.btnVarInviaValore = new System.Windows.Forms.Button();
            this.flvVarListaVariabili = new BrightIdeasSoftware.FastObjectListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.txtVarIdVariabile = new System.Windows.Forms.TextBox();
            this.btnVarCrea = new System.Windows.Forms.Button();
            this.txtVarNomeVariabile = new System.Windows.Forms.TextBox();
            this.tbpRealTime = new System.Windows.Forms.TabPage();
            this.btnRtResetBoard = new System.Windows.Forms.Button();
            this.grbRtPulsanti = new System.Windows.Forms.GroupBox();
            this.btnRtLeggiPulsanti = new System.Windows.Forms.Button();
            this.txtRtValBtn01 = new System.Windows.Forms.TextBox();
            this.txtRtValBtn02 = new System.Windows.Forms.TextBox();
            this.txtRtValBtn03 = new System.Windows.Forms.TextBox();
            this.txtRtValBtn04 = new System.Windows.Forms.TextBox();
            this.txtRtValBtn05 = new System.Windows.Forms.TextBox();
            this.btnRtTestLed = new System.Windows.Forms.Button();
            this.label79 = new System.Windows.Forms.Label();
            this.txtRtSeqSchTime = new System.Windows.Forms.TextBox();
            this.txtRtSeqSchId = new System.Windows.Forms.TextBox();
            this.btnRtDrawSchSequence = new System.Windows.Forms.Button();
            this.txtRtIdVariabile = new System.Windows.Forms.TextBox();
            this.txtRtValVariabile = new System.Windows.Forms.TextBox();
            this.btnRtImpostaVariabile = new System.Windows.Forms.Button();
            this.cmbRtValVariabile = new System.Windows.Forms.ComboBox();
            this.label78 = new System.Windows.Forms.Label();
            this.btnRtSetRTC = new System.Windows.Forms.Button();
            this.btnRtDrawSchermata = new System.Windows.Forms.Button();
            this.label57 = new System.Windows.Forms.Label();
            this.txtRtValSchId = new System.Windows.Forms.TextBox();
            this.label58 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.txtRtValTimeOffDx = new System.Windows.Forms.TextBox();
            this.txtRtValTimeOnDx = new System.Windows.Forms.TextBox();
            this.txtRtValBluDx = new System.Windows.Forms.TextBox();
            this.txtRtValGreenDx = new System.Windows.Forms.TextBox();
            this.txtRtValRedDx = new System.Windows.Forms.TextBox();
            this.btnRtCLS = new System.Windows.Forms.Button();
            this.btnRtDrawImage = new System.Windows.Forms.Button();
            this.txtRtValImgColor = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtRtValImgPosY = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txtRtValImgPosX = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.txtRtValImgId = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.btnRtDrawLine = new System.Windows.Forms.Button();
            this.txtRtValLineColor = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtRtValLineYFine = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtRtValLineXFine = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtRtValLineYStart = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtRtValLineXStart = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnRtStopLed = new System.Windows.Forms.Button();
            this.btnRtSetLed = new System.Windows.Forms.Button();
            this.txtRtValTimeOff = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRtValTimeOn = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRtValBlu = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRtValGreen = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRtValRed = new System.Windows.Forms.TextBox();
            this.label182 = new System.Windows.Forms.Label();
            this.chkRtBacklight = new System.Windows.Forms.CheckBox();
            this.tbpAccesssoMemoria = new System.Windows.Forms.TabPage();
            this.grbMemScrittura = new System.Windows.Forms.GroupBox();
            this.chkMemHexW = new System.Windows.Forms.CheckBox();
            this.lblMemVerificaValore = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.txtMemDataW = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.txtMemLenW = new System.Windows.Forms.TextBox();
            this.txtMemAddrW = new System.Windows.Forms.TextBox();
            this.cmdMemWrite = new System.Windows.Forms.Button();
            this.grbMemCancFisica = new System.Windows.Forms.GroupBox();
            this.rbtMemAreaApp2 = new System.Windows.Forms.RadioButton();
            this.rbtMemAreaApp1 = new System.Windows.Forms.RadioButton();
            this.rbtMemAreaLibera = new System.Windows.Forms.RadioButton();
            this.label111 = new System.Windows.Forms.Label();
            this.txtMemCFBlocchi = new System.Windows.Forms.TextBox();
            this.chkMemCFStartAddHex = new System.Windows.Forms.CheckBox();
            this.label112 = new System.Windows.Forms.Label();
            this.txtMemCFStartAdd = new System.Windows.Forms.TextBox();
            this.btnMemCFExec = new System.Windows.Forms.Button();
            this.grbMemCancellazione = new System.Windows.Forms.GroupBox();
            this.cmdMemClear = new System.Windows.Forms.Button();
            this.txtMemDataGrid = new System.Windows.Forms.TextBox();
            this.grbMemLettura = new System.Windows.Forms.GroupBox();
            this.chkMemHex = new System.Windows.Forms.CheckBox();
            this.lblReadMemBytes = new System.Windows.Forms.Label();
            this.lblReadMemStartAddr = new System.Windows.Forms.Label();
            this.txtMemLenR = new System.Windows.Forms.TextBox();
            this.txtMemAddrR = new System.Windows.Forms.TextBox();
            this.cmdMemRead = new System.Windows.Forms.Button();
            this.tbpStatoScheda = new System.Windows.Forms.TabPage();
            this.label54 = new System.Windows.Forms.Label();
            this.panel15 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label55 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label56 = new System.Windows.Forms.Label();
            this.btnStatoSchCarica = new System.Windows.Forms.Button();
            this.flvStatoListaSch = new BrightIdeasSoftware.FastObjectListView();
            this.lblStatoImmagini = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.chkStatoImgMostraTutto = new System.Windows.Forms.CheckBox();
            this.txtStatoImgEnd = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.txtStatoImgStart = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.btnStatoImgCarica = new System.Windows.Forms.Button();
            this.flvStatoListaImg = new BrightIdeasSoftware.FastObjectListView();
            this.sfdExportDati = new System.Windows.Forms.SaveFileDialog();
            this.ofdImportDati = new System.Windows.Forms.OpenFileDialog();
            this.pgbStatoAvanzamento = new System.Windows.Forms.ProgressBar();
            this.tbcMainDisplayManager.SuspendLayout();
            this.tbpConnessione.SuspendLayout();
            this.pnlComandiImmediati.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tbpArchivioModelli.SuspendLayout();
            this.grbModInvioModello.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tbpSchermate.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel14.SuspendLayout();
            this.panel13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvSchListaComandi)).BeginInit();
            this.panel5.SuspendLayout();
            this.pnlSchImmagineSchermata.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSchImmagine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flvSchListaSchermate)).BeginInit();
            this.tbpImmagini.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvImgListaImmagini)).BeginInit();
            this.grbGeneraExcel.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxImgImmagine1b)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxImgImmagine8b)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxImgImmagine)).BeginInit();
            this.tbpVariabili.SuspendLayout();
            this.panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvVarListaVariabili)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.tbpRealTime.SuspendLayout();
            this.grbRtPulsanti.SuspendLayout();
            this.tbpAccesssoMemoria.SuspendLayout();
            this.grbMemScrittura.SuspendLayout();
            this.grbMemCancFisica.SuspendLayout();
            this.grbMemCancellazione.SuspendLayout();
            this.grbMemLettura.SuspendLayout();
            this.tbpStatoScheda.SuspendLayout();
            this.panel15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvStatoListaSch)).BeginInit();
            this.panel11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvStatoListaImg)).BeginInit();
            this.SuspendLayout();
            // 
            // tbcMainDisplayManager
            // 
            this.tbcMainDisplayManager.Controls.Add(this.tbpConnessione);
            this.tbcMainDisplayManager.Controls.Add(this.tbpArchivioModelli);
            this.tbcMainDisplayManager.Controls.Add(this.tbpSchermate);
            this.tbcMainDisplayManager.Controls.Add(this.tbpImmagini);
            this.tbcMainDisplayManager.Controls.Add(this.tbpVariabili);
            this.tbcMainDisplayManager.Controls.Add(this.tbpRealTime);
            this.tbcMainDisplayManager.Controls.Add(this.tbpAccesssoMemoria);
            this.tbcMainDisplayManager.Controls.Add(this.tbpStatoScheda);
            this.tbcMainDisplayManager.Location = new System.Drawing.Point(12, 12);
            this.tbcMainDisplayManager.Name = "tbcMainDisplayManager";
            this.tbcMainDisplayManager.SelectedIndex = 0;
            this.tbcMainDisplayManager.Size = new System.Drawing.Size(1633, 752);
            this.tbcMainDisplayManager.TabIndex = 0;
            // 
            // tbpConnessione
            // 
            this.tbpConnessione.BackColor = System.Drawing.Color.LightYellow;
            this.tbpConnessione.Controls.Add(this.pnlComandiImmediati);
            this.tbpConnessione.Controls.Add(this.panel1);
            this.tbpConnessione.Location = new System.Drawing.Point(4, 25);
            this.tbpConnessione.Name = "tbpConnessione";
            this.tbpConnessione.Padding = new System.Windows.Forms.Padding(3);
            this.tbpConnessione.Size = new System.Drawing.Size(1625, 668);
            this.tbpConnessione.TabIndex = 0;
            this.tbpConnessione.Text = "Connessione";
            this.tbpConnessione.Click += new System.EventHandler(this.tbpConnessione_Click);
            // 
            // pnlComandiImmediati
            // 
            this.pnlComandiImmediati.BackColor = System.Drawing.Color.White;
            this.pnlComandiImmediati.Controls.Add(this.chkRtRiavvioAutomatico);
            this.pnlComandiImmediati.Controls.Add(this.button1);
            this.pnlComandiImmediati.Controls.Add(this.btnPrimaLettura);
            this.pnlComandiImmediati.Controls.Add(this.cmdLeggiRTC);
            this.pnlComandiImmediati.Controls.Add(this.btnApriComunicazione);
            this.pnlComandiImmediati.Controls.Add(this.label77);
            this.pnlComandiImmediati.Controls.Add(this.btnRtSetBaudRate);
            this.pnlComandiImmediati.Controls.Add(this.cmbRtBaudRate);
            this.pnlComandiImmediati.Location = new System.Drawing.Point(646, 37);
            this.pnlComandiImmediati.Name = "pnlComandiImmediati";
            this.pnlComandiImmediati.Size = new System.Drawing.Size(307, 236);
            this.pnlComandiImmediati.TabIndex = 1;
            // 
            // chkRtRiavvioAutomatico
            // 
            this.chkRtRiavvioAutomatico.AutoSize = true;
            this.chkRtRiavvioAutomatico.Checked = true;
            this.chkRtRiavvioAutomatico.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRtRiavvioAutomatico.Location = new System.Drawing.Point(28, 191);
            this.chkRtRiavvioAutomatico.Name = "chkRtRiavvioAutomatico";
            this.chkRtRiavvioAutomatico.Size = new System.Drawing.Size(150, 21);
            this.chkRtRiavvioAutomatico.TabIndex = 114;
            this.chkRtRiavvioAutomatico.Text = "Riavvio Automatico";
            this.chkRtRiavvioAutomatico.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(28, 310);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(192, 28);
            this.button1.TabIndex = 36;
            this.button1.Text = "Stop Comunicazione";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnPrimaLettura
            // 
            this.btnPrimaLettura.Location = new System.Drawing.Point(28, 390);
            this.btnPrimaLettura.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrimaLettura.Name = "btnPrimaLettura";
            this.btnPrimaLettura.Size = new System.Drawing.Size(136, 28);
            this.btnPrimaLettura.TabIndex = 35;
            this.btnPrimaLettura.Text = "Prima Lettura";
            this.btnPrimaLettura.UseVisualStyleBackColor = true;
            this.btnPrimaLettura.Visible = false;
            // 
            // cmdLeggiRTC
            // 
            this.cmdLeggiRTC.Location = new System.Drawing.Point(28, 354);
            this.cmdLeggiRTC.Margin = new System.Windows.Forms.Padding(4);
            this.cmdLeggiRTC.Name = "cmdLeggiRTC";
            this.cmdLeggiRTC.Size = new System.Drawing.Size(136, 28);
            this.cmdLeggiRTC.TabIndex = 34;
            this.cmdLeggiRTC.Text = "Read RTC";
            this.cmdLeggiRTC.UseVisualStyleBackColor = true;
            this.cmdLeggiRTC.Visible = false;
            // 
            // btnApriComunicazione
            // 
            this.btnApriComunicazione.Enabled = false;
            this.btnApriComunicazione.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApriComunicazione.Location = new System.Drawing.Point(28, 22);
            this.btnApriComunicazione.Margin = new System.Windows.Forms.Padding(4);
            this.btnApriComunicazione.Name = "btnApriComunicazione";
            this.btnApriComunicazione.Size = new System.Drawing.Size(246, 56);
            this.btnApriComunicazione.TabIndex = 33;
            this.btnApriComunicazione.Text = "Start Comunicazione";
            this.btnApriComunicazione.UseVisualStyleBackColor = true;
            this.btnApriComunicazione.Click += new System.EventHandler(this.btnApriComunicazione_Click);
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label77.Location = new System.Drawing.Point(25, 113);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(142, 17);
            this.label77.TabIndex = 111;
            this.label77.Text = "Nuovo BAUDRATE";
            // 
            // btnRtSetBaudRate
            // 
            this.btnRtSetBaudRate.Location = new System.Drawing.Point(28, 150);
            this.btnRtSetBaudRate.Name = "btnRtSetBaudRate";
            this.btnRtSetBaudRate.Size = new System.Drawing.Size(246, 33);
            this.btnRtSetBaudRate.TabIndex = 113;
            this.btnRtSetBaudRate.Text = "Reimposta BaudRate";
            this.btnRtSetBaudRate.UseVisualStyleBackColor = true;
            this.btnRtSetBaudRate.Click += new System.EventHandler(this.btnRtSetBaudRate_Click);
            // 
            // cmbRtBaudRate
            // 
            this.cmbRtBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRtBaudRate.FormattingEnabled = true;
            this.cmbRtBaudRate.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cmbRtBaudRate.Location = new System.Drawing.Point(187, 110);
            this.cmbRtBaudRate.Name = "cmbRtBaudRate";
            this.cmbRtBaudRate.Size = new System.Drawing.Size(87, 24);
            this.cmbRtBaudRate.TabIndex = 112;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblPorta);
            this.panel1.Controls.Add(this.rtbhex);
            this.panel1.Controls.Add(this.rtbOutgoing);
            this.panel1.Controls.Add(this.lblRIStatus);
            this.panel1.Controls.Add(this.lblDSRStatus);
            this.panel1.Controls.Add(this.lblCTSStatus);
            this.panel1.Controls.Add(this.lblBreakStatus);
            this.panel1.Controls.Add(this.rtbIncoming);
            this.panel1.Controls.Add(this.btnPortState);
            this.panel1.Controls.Add(this.cboHandShaking);
            this.panel1.Controls.Add(this.cboParity);
            this.panel1.Controls.Add(this.cboStopBits);
            this.panel1.Controls.Add(this.cboDataBits);
            this.panel1.Controls.Add(this.cboBaudRate);
            this.panel1.Controls.Add(this.cboPorts);
            this.panel1.Controls.Add(this.btnGetSerialPorts);
            this.panel1.Location = new System.Drawing.Point(49, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(537, 568);
            this.panel1.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(184, 195);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 17);
            this.label5.TabIndex = 45;
            this.label5.Text = "Handshake";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(184, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 17);
            this.label4.TabIndex = 44;
            this.label4.Text = "Parità";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(184, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 17);
            this.label3.TabIndex = 43;
            this.label3.Text = "Bit Stop";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 17);
            this.label2.TabIndex = 42;
            this.label2.Text = "Bit Dati";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 41;
            this.label1.Text = "Baudrate";
            // 
            // lblPorta
            // 
            this.lblPorta.AutoSize = true;
            this.lblPorta.Location = new System.Drawing.Point(184, 29);
            this.lblPorta.Name = "lblPorta";
            this.lblPorta.Size = new System.Drawing.Size(42, 17);
            this.lblPorta.TabIndex = 40;
            this.lblPorta.Text = "Porta";
            // 
            // rtbhex
            // 
            this.rtbhex.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbhex.Location = new System.Drawing.Point(27, 454);
            this.rtbhex.Name = "rtbhex";
            this.rtbhex.Size = new System.Drawing.Size(433, 100);
            this.rtbhex.TabIndex = 39;
            this.rtbhex.Text = "";
            // 
            // rtbOutgoing
            // 
            this.rtbOutgoing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbOutgoing.Location = new System.Drawing.Point(27, 260);
            this.rtbOutgoing.Margin = new System.Windows.Forms.Padding(4);
            this.rtbOutgoing.Name = "rtbOutgoing";
            this.rtbOutgoing.Size = new System.Drawing.Size(433, 37);
            this.rtbOutgoing.TabIndex = 38;
            this.rtbOutgoing.Text = "";
            // 
            // lblRIStatus
            // 
            this.lblRIStatus.AutoSize = true;
            this.lblRIStatus.Location = new System.Drawing.Point(257, 230);
            this.lblRIStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRIStatus.Name = "lblRIStatus";
            this.lblRIStatus.Size = new System.Drawing.Size(21, 17);
            this.lblRIStatus.TabIndex = 37;
            this.lblRIStatus.Text = "RI";
            this.lblRIStatus.Visible = false;
            // 
            // lblDSRStatus
            // 
            this.lblDSRStatus.AutoSize = true;
            this.lblDSRStatus.Location = new System.Drawing.Point(184, 230);
            this.lblDSRStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDSRStatus.Name = "lblDSRStatus";
            this.lblDSRStatus.Size = new System.Drawing.Size(37, 17);
            this.lblDSRStatus.TabIndex = 36;
            this.lblDSRStatus.Text = "DSR";
            this.lblDSRStatus.Visible = false;
            // 
            // lblCTSStatus
            // 
            this.lblCTSStatus.AutoSize = true;
            this.lblCTSStatus.Location = new System.Drawing.Point(116, 230);
            this.lblCTSStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCTSStatus.Name = "lblCTSStatus";
            this.lblCTSStatus.Size = new System.Drawing.Size(35, 17);
            this.lblCTSStatus.TabIndex = 35;
            this.lblCTSStatus.Text = "CTS";
            this.lblCTSStatus.Visible = false;
            // 
            // lblBreakStatus
            // 
            this.lblBreakStatus.AutoSize = true;
            this.lblBreakStatus.Location = new System.Drawing.Point(43, 230);
            this.lblBreakStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBreakStatus.Name = "lblBreakStatus";
            this.lblBreakStatus.Size = new System.Drawing.Size(45, 17);
            this.lblBreakStatus.TabIndex = 34;
            this.lblBreakStatus.Text = "Break";
            this.lblBreakStatus.Visible = false;
            // 
            // rtbIncoming
            // 
            this.rtbIncoming.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtbIncoming.Location = new System.Drawing.Point(27, 314);
            this.rtbIncoming.Margin = new System.Windows.Forms.Padding(4);
            this.rtbIncoming.Name = "rtbIncoming";
            this.rtbIncoming.Size = new System.Drawing.Size(433, 117);
            this.rtbIncoming.TabIndex = 33;
            this.rtbIncoming.Text = "";
            // 
            // btnPortState
            // 
            this.btnPortState.Location = new System.Drawing.Point(27, 59);
            this.btnPortState.Margin = new System.Windows.Forms.Padding(4);
            this.btnPortState.Name = "btnPortState";
            this.btnPortState.Size = new System.Drawing.Size(100, 28);
            this.btnPortState.TabIndex = 28;
            this.btnPortState.Text = "Apri Porta";
            this.btnPortState.UseVisualStyleBackColor = true;
            this.btnPortState.Click += new System.EventHandler(this.btnPortState_Click);
            // 
            // cboHandShaking
            // 
            this.cboHandShaking.FormattingEnabled = true;
            this.cboHandShaking.Location = new System.Drawing.Point(288, 192);
            this.cboHandShaking.Margin = new System.Windows.Forms.Padding(4);
            this.cboHandShaking.Name = "cboHandShaking";
            this.cboHandShaking.Size = new System.Drawing.Size(160, 24);
            this.cboHandShaking.TabIndex = 27;
            // 
            // cboParity
            // 
            this.cboParity.FormattingEnabled = true;
            this.cboParity.Location = new System.Drawing.Point(288, 159);
            this.cboParity.Margin = new System.Windows.Forms.Padding(4);
            this.cboParity.Name = "cboParity";
            this.cboParity.Size = new System.Drawing.Size(160, 24);
            this.cboParity.TabIndex = 26;
            // 
            // cboStopBits
            // 
            this.cboStopBits.FormattingEnabled = true;
            this.cboStopBits.Location = new System.Drawing.Point(288, 125);
            this.cboStopBits.Margin = new System.Windows.Forms.Padding(4);
            this.cboStopBits.Name = "cboStopBits";
            this.cboStopBits.Size = new System.Drawing.Size(160, 24);
            this.cboStopBits.TabIndex = 25;
            // 
            // cboDataBits
            // 
            this.cboDataBits.FormattingEnabled = true;
            this.cboDataBits.Location = new System.Drawing.Point(288, 92);
            this.cboDataBits.Margin = new System.Windows.Forms.Padding(4);
            this.cboDataBits.Name = "cboDataBits";
            this.cboDataBits.Size = new System.Drawing.Size(160, 24);
            this.cboDataBits.TabIndex = 24;
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.FormattingEnabled = true;
            this.cboBaudRate.Location = new System.Drawing.Point(288, 58);
            this.cboBaudRate.Margin = new System.Windows.Forms.Padding(4);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(160, 24);
            this.cboBaudRate.TabIndex = 23;
            // 
            // cboPorts
            // 
            this.cboPorts.FormattingEnabled = true;
            this.cboPorts.Location = new System.Drawing.Point(288, 26);
            this.cboPorts.Margin = new System.Windows.Forms.Padding(4);
            this.cboPorts.Name = "cboPorts";
            this.cboPorts.Size = new System.Drawing.Size(160, 24);
            this.cboPorts.TabIndex = 22;
            // 
            // btnGetSerialPorts
            // 
            this.btnGetSerialPorts.Location = new System.Drawing.Point(27, 23);
            this.btnGetSerialPorts.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetSerialPorts.Name = "btnGetSerialPorts";
            this.btnGetSerialPorts.Size = new System.Drawing.Size(100, 28);
            this.btnGetSerialPorts.TabIndex = 21;
            this.btnGetSerialPorts.Text = "Carica Porte";
            this.btnGetSerialPorts.UseVisualStyleBackColor = true;
            this.btnGetSerialPorts.Click += new System.EventHandler(this.btnGetSerialPorts_Click);
            // 
            // tbpArchivioModelli
            // 
            this.tbpArchivioModelli.BackColor = System.Drawing.Color.LightYellow;
            this.tbpArchivioModelli.Controls.Add(this.grbModInvioModello);
            this.tbpArchivioModelli.Controls.Add(this.panel8);
            this.tbpArchivioModelli.Controls.Add(this.panel7);
            this.tbpArchivioModelli.Controls.Add(this.panel6);
            this.tbpArchivioModelli.Controls.Add(this.groupBox1);
            this.tbpArchivioModelli.Location = new System.Drawing.Point(4, 25);
            this.tbpArchivioModelli.Name = "tbpArchivioModelli";
            this.tbpArchivioModelli.Size = new System.Drawing.Size(1625, 668);
            this.tbpArchivioModelli.TabIndex = 6;
            this.tbpArchivioModelli.Text = "Modello";
            // 
            // grbModInvioModello
            // 
            this.grbModInvioModello.BackColor = System.Drawing.Color.White;
            this.grbModInvioModello.Controls.Add(this.txtModVariabiliTrasmesse);
            this.grbModInvioModello.Controls.Add(this.btnModInviaVariabili);
            this.grbModInvioModello.Controls.Add(this.pgbModStatoInvio);
            this.grbModInvioModello.Controls.Add(this.btnModAggiornaDisplay);
            this.grbModInvioModello.Controls.Add(this.txtModSchermateTrasmesse);
            this.grbModInvioModello.Controls.Add(this.txtModImmaginiTrasmesse);
            this.grbModInvioModello.Controls.Add(this.btnModInviaSchermate);
            this.grbModInvioModello.Controls.Add(this.btnModInviaImmagini);
            this.grbModInvioModello.Enabled = false;
            this.grbModInvioModello.Location = new System.Drawing.Point(835, 275);
            this.grbModInvioModello.Name = "grbModInvioModello";
            this.grbModInvioModello.Size = new System.Drawing.Size(369, 325);
            this.grbModInvioModello.TabIndex = 24;
            this.grbModInvioModello.TabStop = false;
            this.grbModInvioModello.Text = "UPLOAD";
            // 
            // txtModVariabiliTrasmesse
            // 
            this.txtModVariabiliTrasmesse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModVariabiliTrasmesse.Location = new System.Drawing.Point(266, 152);
            this.txtModVariabiliTrasmesse.Name = "txtModVariabiliTrasmesse";
            this.txtModVariabiliTrasmesse.Size = new System.Drawing.Size(74, 24);
            this.txtModVariabiliTrasmesse.TabIndex = 107;
            this.txtModVariabiliTrasmesse.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnModInviaVariabili
            // 
            this.btnModInviaVariabili.Location = new System.Drawing.Point(23, 145);
            this.btnModInviaVariabili.Name = "btnModInviaVariabili";
            this.btnModInviaVariabili.Size = new System.Drawing.Size(201, 40);
            this.btnModInviaVariabili.TabIndex = 106;
            this.btnModInviaVariabili.Text = "Invia Variabili";
            this.btnModInviaVariabili.UseVisualStyleBackColor = true;
            this.btnModInviaVariabili.Click += new System.EventHandler(this.btnModInviaVariabili_Click);
            // 
            // pgbModStatoInvio
            // 
            this.pgbModStatoInvio.Location = new System.Drawing.Point(23, 220);
            this.pgbModStatoInvio.Name = "pgbModStatoInvio";
            this.pgbModStatoInvio.Size = new System.Drawing.Size(317, 27);
            this.pgbModStatoInvio.TabIndex = 105;
            // 
            // btnModAggiornaDisplay
            // 
            this.btnModAggiornaDisplay.Location = new System.Drawing.Point(23, 269);
            this.btnModAggiornaDisplay.Name = "btnModAggiornaDisplay";
            this.btnModAggiornaDisplay.Size = new System.Drawing.Size(317, 40);
            this.btnModAggiornaDisplay.TabIndex = 104;
            this.btnModAggiornaDisplay.Text = "Aggiorna Display";
            this.btnModAggiornaDisplay.UseVisualStyleBackColor = true;
            this.btnModAggiornaDisplay.Click += new System.EventHandler(this.btnModAggiornaDisplay_Click);
            // 
            // txtModSchermateTrasmesse
            // 
            this.txtModSchermateTrasmesse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModSchermateTrasmesse.Location = new System.Drawing.Point(266, 94);
            this.txtModSchermateTrasmesse.Name = "txtModSchermateTrasmesse";
            this.txtModSchermateTrasmesse.Size = new System.Drawing.Size(74, 24);
            this.txtModSchermateTrasmesse.TabIndex = 103;
            this.txtModSchermateTrasmesse.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtModImmaginiTrasmesse
            // 
            this.txtModImmaginiTrasmesse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModImmaginiTrasmesse.Location = new System.Drawing.Point(266, 36);
            this.txtModImmaginiTrasmesse.Name = "txtModImmaginiTrasmesse";
            this.txtModImmaginiTrasmesse.Size = new System.Drawing.Size(74, 24);
            this.txtModImmaginiTrasmesse.TabIndex = 102;
            this.txtModImmaginiTrasmesse.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnModInviaSchermate
            // 
            this.btnModInviaSchermate.Location = new System.Drawing.Point(23, 87);
            this.btnModInviaSchermate.Name = "btnModInviaSchermate";
            this.btnModInviaSchermate.Size = new System.Drawing.Size(201, 40);
            this.btnModInviaSchermate.TabIndex = 4;
            this.btnModInviaSchermate.Text = "Invia Schermate";
            this.btnModInviaSchermate.UseVisualStyleBackColor = true;
            this.btnModInviaSchermate.Click += new System.EventHandler(this.btnModInviaSchermate_Click);
            // 
            // btnModInviaImmagini
            // 
            this.btnModInviaImmagini.Location = new System.Drawing.Point(23, 29);
            this.btnModInviaImmagini.Name = "btnModInviaImmagini";
            this.btnModInviaImmagini.Size = new System.Drawing.Size(201, 40);
            this.btnModInviaImmagini.TabIndex = 3;
            this.btnModInviaImmagini.Text = "Invia Immagini";
            this.btnModInviaImmagini.UseVisualStyleBackColor = true;
            this.btnModInviaImmagini.Click += new System.EventHandler(this.btnModInviaImmagini_Click);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.White;
            this.panel8.Controls.Add(this.btnModNuovoModello);
            this.panel8.Location = new System.Drawing.Point(575, 126);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(205, 96);
            this.panel8.TabIndex = 23;
            // 
            // btnModNuovoModello
            // 
            this.btnModNuovoModello.Location = new System.Drawing.Point(19, 27);
            this.btnModNuovoModello.Name = "btnModNuovoModello";
            this.btnModNuovoModello.Size = new System.Drawing.Size(169, 40);
            this.btnModNuovoModello.TabIndex = 2;
            this.btnModNuovoModello.Text = "Nuovo Modello";
            this.btnModNuovoModello.UseVisualStyleBackColor = true;
            this.btnModNuovoModello.Click += new System.EventHandler(this.btnModNuovoModello_Click);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.White;
            this.panel7.Controls.Add(this.label37);
            this.panel7.Controls.Add(this.txtModNumVar);
            this.panel7.Controls.Add(this.label38);
            this.panel7.Controls.Add(this.txtModNumDis);
            this.panel7.Controls.Add(this.label39);
            this.panel7.Controls.Add(this.txtModNumImg);
            this.panel7.Controls.Add(this.label36);
            this.panel7.Controls.Add(this.txtModDataMod);
            this.panel7.Controls.Add(this.label35);
            this.panel7.Controls.Add(this.txtModDataCre);
            this.panel7.Controls.Add(this.label34);
            this.panel7.Controls.Add(this.txtModNote);
            this.panel7.Controls.Add(this.label33);
            this.panel7.Controls.Add(this.txtModVersione);
            this.panel7.Controls.Add(this.label32);
            this.panel7.Controls.Add(this.txtModNome);
            this.panel7.Location = new System.Drawing.Point(47, 275);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(731, 325);
            this.panel7.TabIndex = 22;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label37.Location = new System.Drawing.Point(456, 249);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(58, 17);
            this.label37.TabIndex = 30;
            this.label37.Text = "Variabili";
            // 
            // txtModNumVar
            // 
            this.txtModNumVar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModNumVar.Location = new System.Drawing.Point(459, 269);
            this.txtModNumVar.Name = "txtModNumVar";
            this.txtModNumVar.ReadOnly = true;
            this.txtModNumVar.Size = new System.Drawing.Size(166, 24);
            this.txtModNumVar.TabIndex = 29;
            this.txtModNumVar.Text = "0";
            this.txtModNumVar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label38.Location = new System.Drawing.Point(272, 249);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(76, 17);
            this.label38.TabIndex = 28;
            this.label38.Text = "Schermate";
            // 
            // txtModNumDis
            // 
            this.txtModNumDis.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModNumDis.Location = new System.Drawing.Point(275, 269);
            this.txtModNumDis.Name = "txtModNumDis";
            this.txtModNumDis.ReadOnly = true;
            this.txtModNumDis.Size = new System.Drawing.Size(166, 24);
            this.txtModNumDis.TabIndex = 27;
            this.txtModNumDis.Text = "0";
            this.txtModNumDis.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label39.Location = new System.Drawing.Point(88, 249);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(63, 17);
            this.label39.TabIndex = 26;
            this.label39.Text = "Immagini";
            // 
            // txtModNumImg
            // 
            this.txtModNumImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModNumImg.Location = new System.Drawing.Point(91, 269);
            this.txtModNumImg.Name = "txtModNumImg";
            this.txtModNumImg.ReadOnly = true;
            this.txtModNumImg.Size = new System.Drawing.Size(166, 24);
            this.txtModNumImg.TabIndex = 25;
            this.txtModNumImg.Text = "0";
            this.txtModNumImg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label36.Location = new System.Drawing.Point(456, 87);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(103, 17);
            this.label36.TabIndex = 24;
            this.label36.Text = "Ultima Modifica";
            // 
            // txtModDataMod
            // 
            this.txtModDataMod.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModDataMod.Location = new System.Drawing.Point(459, 107);
            this.txtModDataMod.Name = "txtModDataMod";
            this.txtModDataMod.ReadOnly = true;
            this.txtModDataMod.Size = new System.Drawing.Size(166, 24);
            this.txtModDataMod.TabIndex = 23;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label35.Location = new System.Drawing.Point(272, 87);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(106, 17);
            this.label35.TabIndex = 22;
            this.label35.Text = "Data Creazione";
            // 
            // txtModDataCre
            // 
            this.txtModDataCre.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModDataCre.Location = new System.Drawing.Point(275, 107);
            this.txtModDataCre.Name = "txtModDataCre";
            this.txtModDataCre.ReadOnly = true;
            this.txtModDataCre.Size = new System.Drawing.Size(166, 24);
            this.txtModDataCre.TabIndex = 21;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label34.Location = new System.Drawing.Point(88, 145);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(38, 17);
            this.label34.TabIndex = 20;
            this.label34.Text = "Note";
            // 
            // txtModNote
            // 
            this.txtModNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModNote.Location = new System.Drawing.Point(91, 165);
            this.txtModNote.Multiline = true;
            this.txtModNote.Name = "txtModNote";
            this.txtModNote.Size = new System.Drawing.Size(534, 67);
            this.txtModNote.TabIndex = 19;
            this.txtModNote.Leave += new System.EventHandler(this.txtModNote_Leave);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label33.Location = new System.Drawing.Point(88, 87);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(64, 17);
            this.label33.TabIndex = 18;
            this.label33.Text = "Versione";
            // 
            // txtModVersione
            // 
            this.txtModVersione.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModVersione.Location = new System.Drawing.Point(91, 107);
            this.txtModVersione.Name = "txtModVersione";
            this.txtModVersione.Size = new System.Drawing.Size(166, 24);
            this.txtModVersione.TabIndex = 17;
            this.txtModVersione.Leave += new System.EventHandler(this.txtModVersione_Leave);
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label32.Location = new System.Drawing.Point(88, 29);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(45, 17);
            this.label32.TabIndex = 16;
            this.label32.Text = "Nome";
            // 
            // txtModNome
            // 
            this.txtModNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModNome.Location = new System.Drawing.Point(91, 49);
            this.txtModNome.Name = "txtModNome";
            this.txtModNome.Size = new System.Drawing.Size(534, 24);
            this.txtModNome.TabIndex = 15;
            this.txtModNome.Leave += new System.EventHandler(this.txtModNome_Leave);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.White;
            this.panel6.Controls.Add(this.btnModCercaCaricaModello);
            this.panel6.Controls.Add(this.btnModCercaSalvaModello);
            this.panel6.Controls.Add(this.btnModCaricaModello);
            this.panel6.Controls.Add(this.btnModSalvaModello);
            this.panel6.Location = new System.Drawing.Point(47, 126);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(505, 96);
            this.panel6.TabIndex = 21;
            // 
            // btnModCercaCaricaModello
            // 
            this.btnModCercaCaricaModello.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnModCercaCaricaModello.Location = new System.Drawing.Point(194, 27);
            this.btnModCercaCaricaModello.Name = "btnModCercaCaricaModello";
            this.btnModCercaCaricaModello.Size = new System.Drawing.Size(42, 40);
            this.btnModCercaCaricaModello.TabIndex = 8;
            this.btnModCercaCaricaModello.Text = " ...";
            this.btnModCercaCaricaModello.UseVisualStyleBackColor = true;
            this.btnModCercaCaricaModello.Click += new System.EventHandler(this.btnModCercaCaricaModello_Click);
            // 
            // btnModCercaSalvaModello
            // 
            this.btnModCercaSalvaModello.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnModCercaSalvaModello.Location = new System.Drawing.Point(441, 27);
            this.btnModCercaSalvaModello.Name = "btnModCercaSalvaModello";
            this.btnModCercaSalvaModello.Size = new System.Drawing.Size(42, 40);
            this.btnModCercaSalvaModello.TabIndex = 7;
            this.btnModCercaSalvaModello.Text = " ...";
            this.btnModCercaSalvaModello.UseVisualStyleBackColor = true;
            this.btnModCercaSalvaModello.Click += new System.EventHandler(this.btnModCercaSalvaModello_Click);
            // 
            // btnModCaricaModello
            // 
            this.btnModCaricaModello.Location = new System.Drawing.Point(19, 27);
            this.btnModCaricaModello.Name = "btnModCaricaModello";
            this.btnModCaricaModello.Size = new System.Drawing.Size(169, 40);
            this.btnModCaricaModello.TabIndex = 1;
            this.btnModCaricaModello.Text = "Carica Modello";
            this.btnModCaricaModello.UseVisualStyleBackColor = true;
            this.btnModCaricaModello.Click += new System.EventHandler(this.btnModCaricaModello_Click);
            // 
            // btnModSalvaModello
            // 
            this.btnModSalvaModello.Location = new System.Drawing.Point(266, 27);
            this.btnModSalvaModello.Name = "btnModSalvaModello";
            this.btnModSalvaModello.Size = new System.Drawing.Size(169, 40);
            this.btnModSalvaModello.TabIndex = 0;
            this.btnModSalvaModello.Text = " Salva Modello";
            this.btnModSalvaModello.UseVisualStyleBackColor = true;
            this.btnModSalvaModello.Click += new System.EventHandler(this.btnModSalvaModello_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.White;
            this.groupBox1.Controls.Add(this.txtModNomeFile);
            this.groupBox1.Location = new System.Drawing.Point(47, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(733, 78);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selezione File";
            // 
            // txtModNomeFile
            // 
            this.txtModNomeFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtModNomeFile.Location = new System.Drawing.Point(19, 32);
            this.txtModNomeFile.Name = "txtModNomeFile";
            this.txtModNomeFile.Size = new System.Drawing.Size(692, 24);
            this.txtModNomeFile.TabIndex = 4;
            // 
            // tbpSchermate
            // 
            this.tbpSchermate.BackColor = System.Drawing.Color.LightYellow;
            this.tbpSchermate.Controls.Add(this.panel4);
            this.tbpSchermate.Controls.Add(this.panel3);
            this.tbpSchermate.Controls.Add(this.panel14);
            this.tbpSchermate.Controls.Add(this.panel13);
            this.tbpSchermate.Controls.Add(this.flvSchListaComandi);
            this.tbpSchermate.Controls.Add(this.label53);
            this.tbpSchermate.Controls.Add(this.label51);
            this.tbpSchermate.Controls.Add(this.label49);
            this.tbpSchermate.Controls.Add(this.panel5);
            this.tbpSchermate.Controls.Add(this.pnlSchImmagineSchermata);
            this.tbpSchermate.Controls.Add(this.flvSchListaSchermate);
            this.tbpSchermate.Location = new System.Drawing.Point(4, 25);
            this.tbpSchermate.Name = "tbpSchermate";
            this.tbpSchermate.Size = new System.Drawing.Size(1625, 668);
            this.tbpSchermate.TabIndex = 4;
            this.tbpSchermate.Text = "Schermate";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.cmdSchNuovaSchermata);
            this.panel4.Location = new System.Drawing.Point(57, 60);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(473, 72);
            this.panel4.TabIndex = 90;
            // 
            // cmdSchNuovaSchermata
            // 
            this.cmdSchNuovaSchermata.Location = new System.Drawing.Point(132, 14);
            this.cmdSchNuovaSchermata.Name = "cmdSchNuovaSchermata";
            this.cmdSchNuovaSchermata.Size = new System.Drawing.Size(224, 41);
            this.cmdSchNuovaSchermata.TabIndex = 84;
            this.cmdSchNuovaSchermata.Text = "Nuova Schermata";
            this.cmdSchNuovaSchermata.UseVisualStyleBackColor = true;
            this.cmdSchNuovaSchermata.Click += new System.EventHandler(this.cmdSchNuovaSchermata_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.btnSchCmdNew);
            this.panel3.Controls.Add(this.btnSchCmdLoad);
            this.panel3.Controls.Add(this.btnSchCmdDel);
            this.panel3.Location = new System.Drawing.Point(619, 249);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(507, 72);
            this.panel3.TabIndex = 89;
            // 
            // btnSchCmdNew
            // 
            this.btnSchCmdNew.BackColor = System.Drawing.Color.Transparent;
            this.btnSchCmdNew.Location = new System.Drawing.Point(387, 14);
            this.btnSchCmdNew.Name = "btnSchCmdNew";
            this.btnSchCmdNew.Size = new System.Drawing.Size(107, 41);
            this.btnSchCmdNew.TabIndex = 85;
            this.btnSchCmdNew.Text = "Nuovo Cmd";
            this.btnSchCmdNew.UseVisualStyleBackColor = false;
            this.btnSchCmdNew.Click += new System.EventHandler(this.btnSchCmdNew_Click);
            // 
            // btnSchCmdLoad
            // 
            this.btnSchCmdLoad.Location = new System.Drawing.Point(15, 14);
            this.btnSchCmdLoad.Name = "btnSchCmdLoad";
            this.btnSchCmdLoad.Size = new System.Drawing.Size(103, 41);
            this.btnSchCmdLoad.TabIndex = 84;
            this.btnSchCmdLoad.Text = "Mostra Cmd";
            this.btnSchCmdLoad.UseVisualStyleBackColor = true;
            this.btnSchCmdLoad.Click += new System.EventHandler(this.btnSchCmdLoad_Click);
            // 
            // btnSchCmdDel
            // 
            this.btnSchCmdDel.Location = new System.Drawing.Point(199, 14);
            this.btnSchCmdDel.Name = "btnSchCmdDel";
            this.btnSchCmdDel.Size = new System.Drawing.Size(107, 41);
            this.btnSchCmdDel.TabIndex = 83;
            this.btnSchCmdDel.Text = "Rimuovi Cmd";
            this.btnSchCmdDel.UseVisualStyleBackColor = true;
            this.btnSchCmdDel.Click += new System.EventHandler(this.btnSchCmdDel_Click);
            // 
            // panel14
            // 
            this.panel14.BackColor = System.Drawing.Color.White;
            this.panel14.Controls.Add(this.btnSchCaricaFile);
            this.panel14.Controls.Add(this.btnSchCercaFile);
            this.panel14.Controls.Add(this.txtSchNuovoFile);
            this.panel14.Location = new System.Drawing.Point(57, 159);
            this.panel14.Name = "panel14";
            this.panel14.Size = new System.Drawing.Size(473, 77);
            this.panel14.TabIndex = 88;
            // 
            // btnSchCaricaFile
            // 
            this.btnSchCaricaFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSchCaricaFile.Location = new System.Drawing.Point(157, 41);
            this.btnSchCaricaFile.Name = "btnSchCaricaFile";
            this.btnSchCaricaFile.Size = new System.Drawing.Size(165, 24);
            this.btnSchCaricaFile.TabIndex = 9;
            this.btnSchCaricaFile.Text = "Carica Immagine";
            this.btnSchCaricaFile.UseVisualStyleBackColor = true;
            this.btnSchCaricaFile.Click += new System.EventHandler(this.btnSchCaricaFile_Click);
            // 
            // btnSchCercaFile
            // 
            this.btnSchCercaFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSchCercaFile.Location = new System.Drawing.Point(424, 12);
            this.btnSchCercaFile.Name = "btnSchCercaFile";
            this.btnSchCercaFile.Size = new System.Drawing.Size(33, 24);
            this.btnSchCercaFile.TabIndex = 8;
            this.btnSchCercaFile.Text = " ...";
            this.btnSchCercaFile.UseVisualStyleBackColor = true;
            this.btnSchCercaFile.Click += new System.EventHandler(this.btnSchCercaFile_Click);
            // 
            // txtSchNuovoFile
            // 
            this.txtSchNuovoFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchNuovoFile.Location = new System.Drawing.Point(16, 11);
            this.txtSchNuovoFile.Name = "txtSchNuovoFile";
            this.txtSchNuovoFile.Size = new System.Drawing.Size(402, 24);
            this.txtSchNuovoFile.TabIndex = 7;
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.White;
            this.panel13.Controls.Add(this.cmbSchIdVariabile);
            this.panel13.Controls.Add(this.label75);
            this.panel13.Controls.Add(this.txtSchCmdTempoOFF);
            this.panel13.Controls.Add(this.label76);
            this.panel13.Controls.Add(this.txtSchCmdTempoON);
            this.panel13.Controls.Add(this.btnSchCmdAdd);
            this.panel13.Controls.Add(this.label74);
            this.panel13.Controls.Add(this.txtSchCmdNum);
            this.panel13.Controls.Add(this.label73);
            this.panel13.Controls.Add(this.txtSchCmdText);
            this.panel13.Controls.Add(this.label71);
            this.panel13.Controls.Add(this.txtSchCmdLenVarChar);
            this.panel13.Controls.Add(this.txtSchCmdNumImg);
            this.panel13.Controls.Add(this.label72);
            this.panel13.Controls.Add(this.label69);
            this.panel13.Controls.Add(this.txtSchCmdLenVarPix);
            this.panel13.Controls.Add(this.txtSchCmdNumVar);
            this.panel13.Controls.Add(this.label70);
            this.panel13.Controls.Add(this.label68);
            this.panel13.Controls.Add(this.txtSchCmdColor);
            this.panel13.Controls.Add(this.label66);
            this.panel13.Controls.Add(this.txtSchCmdPosY);
            this.panel13.Controls.Add(this.txtSchCmdPosX);
            this.panel13.Controls.Add(this.label67);
            this.panel13.Controls.Add(this.label64);
            this.panel13.Controls.Add(this.txtSchCmdHeigh);
            this.panel13.Controls.Add(this.txtSchCmdWidth);
            this.panel13.Controls.Add(this.label65);
            this.panel13.Controls.Add(this.cmbSchTipoComando);
            this.panel13.Location = new System.Drawing.Point(619, 346);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(507, 289);
            this.panel13.TabIndex = 87;
            // 
            // cmbSchIdVariabile
            // 
            this.cmbSchIdVariabile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSchIdVariabile.FormattingEnabled = true;
            this.cmbSchIdVariabile.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cmbSchIdVariabile.Location = new System.Drawing.Point(15, 139);
            this.cmbSchIdVariabile.Name = "cmbSchIdVariabile";
            this.cmbSchIdVariabile.Size = new System.Drawing.Size(109, 24);
            this.cmbSchIdVariabile.TabIndex = 124;
            this.cmbSchIdVariabile.SelectedValueChanged += new System.EventHandler(this.cmbSchIdVariabile_SelectedValueChanged);
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label75.Location = new System.Drawing.Point(205, 172);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(40, 17);
            this.label75.TabIndex = 123;
            this.label75.Text = "T Off";
            // 
            // txtSchCmdTempoOFF
            // 
            this.txtSchCmdTempoOFF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdTempoOFF.Location = new System.Drawing.Point(208, 192);
            this.txtSchCmdTempoOFF.MaxLength = 8;
            this.txtSchCmdTempoOFF.Name = "txtSchCmdTempoOFF";
            this.txtSchCmdTempoOFF.Size = new System.Drawing.Size(42, 24);
            this.txtSchCmdTempoOFF.TabIndex = 122;
            this.txtSchCmdTempoOFF.Text = "0";
            this.txtSchCmdTempoOFF.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label76.Location = new System.Drawing.Point(157, 172);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(40, 17);
            this.label76.TabIndex = 121;
            this.label76.Text = "T On";
            // 
            // txtSchCmdTempoON
            // 
            this.txtSchCmdTempoON.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdTempoON.Location = new System.Drawing.Point(160, 192);
            this.txtSchCmdTempoON.MaxLength = 8;
            this.txtSchCmdTempoON.Name = "txtSchCmdTempoON";
            this.txtSchCmdTempoON.Size = new System.Drawing.Size(42, 24);
            this.txtSchCmdTempoON.TabIndex = 120;
            this.txtSchCmdTempoON.Text = "0";
            this.txtSchCmdTempoON.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnSchCmdAdd
            // 
            this.btnSchCmdAdd.Location = new System.Drawing.Point(387, 233);
            this.btnSchCmdAdd.Name = "btnSchCmdAdd";
            this.btnSchCmdAdd.Size = new System.Drawing.Size(107, 37);
            this.btnSchCmdAdd.TabIndex = 119;
            this.btnSchCmdAdd.Text = "Aggiungi";
            this.btnSchCmdAdd.UseVisualStyleBackColor = true;
            this.btnSchCmdAdd.Click += new System.EventHandler(this.btnSchCmdAdd_Click);
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label74.Location = new System.Drawing.Point(336, 222);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(37, 17);
            this.label74.TabIndex = 118;
            this.label74.Text = "Num";
            // 
            // txtSchCmdNum
            // 
            this.txtSchCmdNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdNum.Location = new System.Drawing.Point(339, 242);
            this.txtSchCmdNum.MaxLength = 8;
            this.txtSchCmdNum.Name = "txtSchCmdNum";
            this.txtSchCmdNum.Size = new System.Drawing.Size(42, 24);
            this.txtSchCmdNum.TabIndex = 117;
            this.txtSchCmdNum.Text = "0";
            this.txtSchCmdNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label73.Location = new System.Drawing.Point(18, 226);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(116, 17);
            this.label73.TabIndex = 116;
            this.label73.Text = "Testo Messaggio";
            // 
            // txtSchCmdText
            // 
            this.txtSchCmdText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdText.Location = new System.Drawing.Point(19, 243);
            this.txtSchCmdText.MaxLength = 50;
            this.txtSchCmdText.Name = "txtSchCmdText";
            this.txtSchCmdText.Size = new System.Drawing.Size(314, 24);
            this.txtSchCmdText.TabIndex = 115;
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label71.Location = new System.Drawing.Point(205, 119);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(45, 17);
            this.label71.TabIndex = 114;
            this.label71.Text = "Len C";
            // 
            // txtSchCmdLenVarChar
            // 
            this.txtSchCmdLenVarChar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdLenVarChar.Location = new System.Drawing.Point(208, 139);
            this.txtSchCmdLenVarChar.MaxLength = 8;
            this.txtSchCmdLenVarChar.Name = "txtSchCmdLenVarChar";
            this.txtSchCmdLenVarChar.Size = new System.Drawing.Size(42, 24);
            this.txtSchCmdLenVarChar.TabIndex = 113;
            this.txtSchCmdLenVarChar.Text = "0";
            this.txtSchCmdLenVarChar.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSchCmdNumImg
            // 
            this.txtSchCmdNumImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdNumImg.Location = new System.Drawing.Point(18, 192);
            this.txtSchCmdNumImg.MaxLength = 8;
            this.txtSchCmdNumImg.Name = "txtSchCmdNumImg";
            this.txtSchCmdNumImg.Size = new System.Drawing.Size(137, 24);
            this.txtSchCmdNumImg.TabIndex = 112;
            this.txtSchCmdNumImg.Text = "0";
            this.txtSchCmdNumImg.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label72.Location = new System.Drawing.Point(16, 172);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(66, 17);
            this.label72.TabIndex = 111;
            this.label72.Text = "Num IMG";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label69.Location = new System.Drawing.Point(157, 119);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(49, 17);
            this.label69.TabIndex = 110;
            this.label69.Text = "Len  P";
            // 
            // txtSchCmdLenVarPix
            // 
            this.txtSchCmdLenVarPix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdLenVarPix.Location = new System.Drawing.Point(160, 139);
            this.txtSchCmdLenVarPix.MaxLength = 8;
            this.txtSchCmdLenVarPix.Name = "txtSchCmdLenVarPix";
            this.txtSchCmdLenVarPix.Size = new System.Drawing.Size(42, 24);
            this.txtSchCmdLenVarPix.TabIndex = 109;
            this.txtSchCmdLenVarPix.Text = "0";
            this.txtSchCmdLenVarPix.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSchCmdNumVar
            // 
            this.txtSchCmdNumVar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdNumVar.Location = new System.Drawing.Point(130, 139);
            this.txtSchCmdNumVar.MaxLength = 8;
            this.txtSchCmdNumVar.Name = "txtSchCmdNumVar";
            this.txtSchCmdNumVar.Size = new System.Drawing.Size(24, 24);
            this.txtSchCmdNumVar.TabIndex = 108;
            this.txtSchCmdNumVar.Text = "0";
            this.txtSchCmdNumVar.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label70.Location = new System.Drawing.Point(15, 119);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(69, 17);
            this.label70.TabIndex = 107;
            this.label70.Text = "Num VAR";
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label68.Location = new System.Drawing.Point(238, 58);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(41, 17);
            this.label68.TabIndex = 106;
            this.label68.Text = "Color";
            // 
            // txtSchCmdColor
            // 
            this.txtSchCmdColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdColor.Location = new System.Drawing.Point(238, 78);
            this.txtSchCmdColor.MaxLength = 8;
            this.txtSchCmdColor.Name = "txtSchCmdColor";
            this.txtSchCmdColor.Size = new System.Drawing.Size(42, 24);
            this.txtSchCmdColor.TabIndex = 105;
            this.txtSchCmdColor.Text = "0";
            this.txtSchCmdColor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label66.Location = new System.Drawing.Point(178, 58);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(45, 17);
            this.label66.TabIndex = 104;
            this.label66.Text = "Pos Y";
            // 
            // txtSchCmdPosY
            // 
            this.txtSchCmdPosY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdPosY.Location = new System.Drawing.Point(178, 78);
            this.txtSchCmdPosY.MaxLength = 8;
            this.txtSchCmdPosY.Name = "txtSchCmdPosY";
            this.txtSchCmdPosY.Size = new System.Drawing.Size(42, 24);
            this.txtSchCmdPosY.TabIndex = 103;
            this.txtSchCmdPosY.Text = "0";
            this.txtSchCmdPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSchCmdPosX
            // 
            this.txtSchCmdPosX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdPosX.Location = new System.Drawing.Point(130, 78);
            this.txtSchCmdPosX.MaxLength = 8;
            this.txtSchCmdPosX.Name = "txtSchCmdPosX";
            this.txtSchCmdPosX.Size = new System.Drawing.Size(42, 24);
            this.txtSchCmdPosX.TabIndex = 102;
            this.txtSchCmdPosX.Text = "0";
            this.txtSchCmdPosX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label67.Location = new System.Drawing.Point(128, 58);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(45, 17);
            this.label67.TabIndex = 101;
            this.label67.Text = "Pos X";
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label64.Location = new System.Drawing.Point(65, 58);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(45, 17);
            this.label64.TabIndex = 100;
            this.label64.Text = "Heigh";
            // 
            // txtSchCmdHeigh
            // 
            this.txtSchCmdHeigh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdHeigh.Location = new System.Drawing.Point(65, 78);
            this.txtSchCmdHeigh.MaxLength = 8;
            this.txtSchCmdHeigh.Name = "txtSchCmdHeigh";
            this.txtSchCmdHeigh.Size = new System.Drawing.Size(42, 24);
            this.txtSchCmdHeigh.TabIndex = 99;
            this.txtSchCmdHeigh.Text = "0";
            this.txtSchCmdHeigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSchCmdWidth
            // 
            this.txtSchCmdWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchCmdWidth.Location = new System.Drawing.Point(17, 78);
            this.txtSchCmdWidth.MaxLength = 8;
            this.txtSchCmdWidth.Name = "txtSchCmdWidth";
            this.txtSchCmdWidth.Size = new System.Drawing.Size(42, 24);
            this.txtSchCmdWidth.TabIndex = 98;
            this.txtSchCmdWidth.Text = "0";
            this.txtSchCmdWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label65.Location = new System.Drawing.Point(15, 58);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(44, 17);
            this.label65.TabIndex = 97;
            this.label65.Text = "Width";
            // 
            // cmbSchTipoComando
            // 
            this.cmbSchTipoComando.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSchTipoComando.FormattingEnabled = true;
            this.cmbSchTipoComando.Location = new System.Drawing.Point(17, 20);
            this.cmbSchTipoComando.Name = "cmbSchTipoComando";
            this.cmbSchTipoComando.Size = new System.Drawing.Size(316, 24);
            this.cmbSchTipoComando.TabIndex = 0;
            // 
            // flvSchListaComandi
            // 
            this.flvSchListaComandi.CellEditUseWholeCell = false;
            this.flvSchListaComandi.FullRowSelect = true;
            this.flvSchListaComandi.Location = new System.Drawing.Point(619, 60);
            this.flvSchListaComandi.MultiSelect = false;
            this.flvSchListaComandi.Name = "flvSchListaComandi";
            this.flvSchListaComandi.ShowGroups = false;
            this.flvSchListaComandi.Size = new System.Drawing.Size(507, 165);
            this.flvSchListaComandi.TabIndex = 85;
            this.flvSchListaComandi.UseCompatibleStateImageBehavior = false;
            this.flvSchListaComandi.View = System.Windows.Forms.View.Details;
            this.flvSchListaComandi.VirtualMode = true;
            this.flvSchListaComandi.DoubleClick += new System.EventHandler(this.flvSchListaComandi_DoubleClick);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label53.Location = new System.Drawing.Point(616, 40);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(63, 17);
            this.label53.TabIndex = 84;
            this.label53.Text = "Comandi";
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label51.Location = new System.Drawing.Point(54, 139);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(68, 17);
            this.label51.TabIndex = 83;
            this.label51.Text = "Immagine";
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label49.Location = new System.Drawing.Point(1200, 40);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(110, 17);
            this.label49.TabIndex = 81;
            this.label49.Text = "Lista Schermate";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.cmdSchGeneraByteArray);
            this.panel5.Controls.Add(this.cmdSchMostrtaSch);
            this.panel5.Controls.Add(this.cmdSchRimuoviElemento);
            this.panel5.Controls.Add(this.cmdSchInviaSch);
            this.panel5.Location = new System.Drawing.Point(1203, 519);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(354, 116);
            this.panel5.TabIndex = 3;
            // 
            // cmdSchGeneraByteArray
            // 
            this.cmdSchGeneraByteArray.Location = new System.Drawing.Point(124, 63);
            this.cmdSchGeneraByteArray.Name = "cmdSchGeneraByteArray";
            this.cmdSchGeneraByteArray.Size = new System.Drawing.Size(103, 41);
            this.cmdSchGeneraByteArray.TabIndex = 85;
            this.cmdSchGeneraByteArray.Text = "Genera Byte";
            this.cmdSchGeneraByteArray.UseVisualStyleBackColor = true;
            this.cmdSchGeneraByteArray.Click += new System.EventHandler(this.cmdSchGeneraByteArray_Click);
            // 
            // cmdSchMostrtaSch
            // 
            this.cmdSchMostrtaSch.Location = new System.Drawing.Point(15, 14);
            this.cmdSchMostrtaSch.Name = "cmdSchMostrtaSch";
            this.cmdSchMostrtaSch.Size = new System.Drawing.Size(103, 41);
            this.cmdSchMostrtaSch.TabIndex = 84;
            this.cmdSchMostrtaSch.Text = "Mostra Sch";
            this.cmdSchMostrtaSch.UseVisualStyleBackColor = true;
            this.cmdSchMostrtaSch.Click += new System.EventHandler(this.cmdSchMostrtaSch_Click);
            // 
            // cmdSchRimuoviElemento
            // 
            this.cmdSchRimuoviElemento.Location = new System.Drawing.Point(124, 14);
            this.cmdSchRimuoviElemento.Name = "cmdSchRimuoviElemento";
            this.cmdSchRimuoviElemento.Size = new System.Drawing.Size(103, 41);
            this.cmdSchRimuoviElemento.TabIndex = 83;
            this.cmdSchRimuoviElemento.Text = "Rimuovi";
            this.cmdSchRimuoviElemento.UseVisualStyleBackColor = true;
            this.cmdSchRimuoviElemento.Click += new System.EventHandler(this.cmdSchRimuoviElemento_Click);
            // 
            // cmdSchInviaSch
            // 
            this.cmdSchInviaSch.Enabled = false;
            this.cmdSchInviaSch.Location = new System.Drawing.Point(233, 63);
            this.cmdSchInviaSch.Name = "cmdSchInviaSch";
            this.cmdSchInviaSch.Size = new System.Drawing.Size(103, 41);
            this.cmdSchInviaSch.TabIndex = 82;
            this.cmdSchInviaSch.Text = "Invia Sch";
            this.cmdSchInviaSch.UseVisualStyleBackColor = true;
            this.cmdSchInviaSch.Click += new System.EventHandler(this.cmdSchInviaSch_Click);
            // 
            // pnlSchImmagineSchermata
            // 
            this.pnlSchImmagineSchermata.BackColor = System.Drawing.Color.White;
            this.pnlSchImmagineSchermata.Controls.Add(this.txtSchBaseNomeLista);
            this.pnlSchImmagineSchermata.Controls.Add(this.label50);
            this.pnlSchImmagineSchermata.Controls.Add(this.txtSchBaseID);
            this.pnlSchImmagineSchermata.Controls.Add(this.label59);
            this.pnlSchImmagineSchermata.Controls.Add(this.cmdSchGeneraClasse);
            this.pnlSchImmagineSchermata.Controls.Add(this.label60);
            this.pnlSchImmagineSchermata.Controls.Add(this.txtSchBaseHeigh);
            this.pnlSchImmagineSchermata.Controls.Add(this.txtSchBaseWidth);
            this.pnlSchImmagineSchermata.Controls.Add(this.label61);
            this.pnlSchImmagineSchermata.Controls.Add(this.txtSchBaseSize);
            this.pnlSchImmagineSchermata.Controls.Add(this.label62);
            this.pnlSchImmagineSchermata.Controls.Add(this.txtSchBaseName);
            this.pnlSchImmagineSchermata.Controls.Add(this.label63);
            this.pnlSchImmagineSchermata.Controls.Add(this.pbxSchImmagine);
            this.pnlSchImmagineSchermata.Controls.Add(this.label52);
            this.pnlSchImmagineSchermata.Location = new System.Drawing.Point(57, 257);
            this.pnlSchImmagineSchermata.Name = "pnlSchImmagineSchermata";
            this.pnlSchImmagineSchermata.Size = new System.Drawing.Size(473, 377);
            this.pnlSchImmagineSchermata.TabIndex = 1;
            // 
            // txtSchBaseNomeLista
            // 
            this.txtSchBaseNomeLista.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchBaseNomeLista.Location = new System.Drawing.Point(13, 311);
            this.txtSchBaseNomeLista.MaxLength = 50;
            this.txtSchBaseNomeLista.Name = "txtSchBaseNomeLista";
            this.txtSchBaseNomeLista.Size = new System.Drawing.Size(309, 24);
            this.txtSchBaseNomeLista.TabIndex = 101;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label50.Location = new System.Drawing.Point(13, 291);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(109, 17);
            this.label50.TabIndex = 100;
            this.label50.Text = "Nome Immagine";
            // 
            // txtSchBaseID
            // 
            this.txtSchBaseID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchBaseID.Location = new System.Drawing.Point(13, 255);
            this.txtSchBaseID.MaxLength = 8;
            this.txtSchBaseID.Name = "txtSchBaseID";
            this.txtSchBaseID.Size = new System.Drawing.Size(52, 24);
            this.txtSchBaseID.TabIndex = 99;
            this.txtSchBaseID.Text = "0";
            this.txtSchBaseID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSchBaseID.Leave += new System.EventHandler(this.txtSchBaseID_Leave);
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label59.Location = new System.Drawing.Point(13, 235);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(19, 17);
            this.label59.TabIndex = 98;
            this.label59.Text = "Id";
            // 
            // cmdSchGeneraClasse
            // 
            this.cmdSchGeneraClasse.Location = new System.Drawing.Point(328, 310);
            this.cmdSchGeneraClasse.Name = "cmdSchGeneraClasse";
            this.cmdSchGeneraClasse.Size = new System.Drawing.Size(129, 29);
            this.cmdSchGeneraClasse.TabIndex = 97;
            this.cmdSchGeneraClasse.Text = "Genera Classe";
            this.cmdSchGeneraClasse.UseVisualStyleBackColor = true;
            this.cmdSchGeneraClasse.Click += new System.EventHandler(this.cmdSchGeneraClasse_Click);
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label60.Location = new System.Drawing.Point(410, 235);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(45, 17);
            this.label60.TabIndex = 96;
            this.label60.Text = "Heigh";
            // 
            // txtSchBaseHeigh
            // 
            this.txtSchBaseHeigh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchBaseHeigh.Location = new System.Drawing.Point(410, 255);
            this.txtSchBaseHeigh.MaxLength = 8;
            this.txtSchBaseHeigh.Name = "txtSchBaseHeigh";
            this.txtSchBaseHeigh.ReadOnly = true;
            this.txtSchBaseHeigh.Size = new System.Drawing.Size(42, 24);
            this.txtSchBaseHeigh.TabIndex = 95;
            this.txtSchBaseHeigh.Text = "0";
            this.txtSchBaseHeigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSchBaseWidth
            // 
            this.txtSchBaseWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchBaseWidth.Location = new System.Drawing.Point(362, 255);
            this.txtSchBaseWidth.MaxLength = 8;
            this.txtSchBaseWidth.Name = "txtSchBaseWidth";
            this.txtSchBaseWidth.ReadOnly = true;
            this.txtSchBaseWidth.Size = new System.Drawing.Size(42, 24);
            this.txtSchBaseWidth.TabIndex = 94;
            this.txtSchBaseWidth.Text = "0";
            this.txtSchBaseWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label61.Location = new System.Drawing.Point(360, 235);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(44, 17);
            this.label61.TabIndex = 93;
            this.label61.Text = "Width";
            // 
            // txtSchBaseSize
            // 
            this.txtSchBaseSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchBaseSize.Location = new System.Drawing.Point(207, 255);
            this.txtSchBaseSize.MaxLength = 8;
            this.txtSchBaseSize.Name = "txtSchBaseSize";
            this.txtSchBaseSize.ReadOnly = true;
            this.txtSchBaseSize.Size = new System.Drawing.Size(149, 24);
            this.txtSchBaseSize.TabIndex = 92;
            this.txtSchBaseSize.Text = "0";
            this.txtSchBaseSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label62.Location = new System.Drawing.Point(204, 235);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(35, 17);
            this.label62.TabIndex = 91;
            this.label62.Text = "Size";
            // 
            // txtSchBaseName
            // 
            this.txtSchBaseName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtSchBaseName.Location = new System.Drawing.Point(71, 255);
            this.txtSchBaseName.MaxLength = 8;
            this.txtSchBaseName.Name = "txtSchBaseName";
            this.txtSchBaseName.ReadOnly = true;
            this.txtSchBaseName.Size = new System.Drawing.Size(130, 24);
            this.txtSchBaseName.TabIndex = 90;
            this.txtSchBaseName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label63.Location = new System.Drawing.Point(68, 235);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(97, 17);
            this.label63.TabIndex = 89;
            this.label63.Text = "Nome Testata";
            // 
            // pbxSchImmagine
            // 
            this.pbxSchImmagine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxSchImmagine.Location = new System.Drawing.Point(80, 16);
            this.pbxSchImmagine.Margin = new System.Windows.Forms.Padding(0);
            this.pbxSchImmagine.Name = "pbxSchImmagine";
            this.pbxSchImmagine.Size = new System.Drawing.Size(313, 165);
            this.pbxSchImmagine.TabIndex = 85;
            this.pbxSchImmagine.TabStop = false;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label52.Location = new System.Drawing.Point(-3, -20);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(68, 17);
            this.label52.TabIndex = 84;
            this.label52.Text = "Immagine";
            // 
            // flvSchListaSchermate
            // 
            this.flvSchListaSchermate.CellEditUseWholeCell = false;
            this.flvSchListaSchermate.FullRowSelect = true;
            this.flvSchListaSchermate.Location = new System.Drawing.Point(1203, 60);
            this.flvSchListaSchermate.MultiSelect = false;
            this.flvSchListaSchermate.Name = "flvSchListaSchermate";
            this.flvSchListaSchermate.ShowGroups = false;
            this.flvSchListaSchermate.Size = new System.Drawing.Size(354, 432);
            this.flvSchListaSchermate.TabIndex = 0;
            this.flvSchListaSchermate.UseCompatibleStateImageBehavior = false;
            this.flvSchListaSchermate.View = System.Windows.Forms.View.Details;
            this.flvSchListaSchermate.VirtualMode = true;
            this.flvSchListaSchermate.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.flvSchListaSchermate_MouseDoubleClick);
            // 
            // tbpImmagini
            // 
            this.tbpImmagini.BackColor = System.Drawing.Color.LightYellow;
            this.tbpImmagini.Controls.Add(this.label42);
            this.tbpImmagini.Controls.Add(this.panel9);
            this.tbpImmagini.Controls.Add(this.flvImgListaImmagini);
            this.tbpImmagini.Controls.Add(this.grbGeneraExcel);
            this.tbpImmagini.Controls.Add(this.panel2);
            this.tbpImmagini.Location = new System.Drawing.Point(4, 25);
            this.tbpImmagini.Name = "tbpImmagini";
            this.tbpImmagini.Size = new System.Drawing.Size(1625, 668);
            this.tbpImmagini.TabIndex = 2;
            this.tbpImmagini.Text = "Immagini";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label42.Location = new System.Drawing.Point(701, 144);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(97, 17);
            this.label42.TabIndex = 80;
            this.label42.Text = "Lista Immagini";
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.White;
            this.panel9.Controls.Add(this.btnImgMostraImmagine);
            this.panel9.Controls.Add(this.btnImgRimuoviImmagine);
            this.panel9.Controls.Add(this.btnImgInviaImmagine);
            this.panel9.Location = new System.Drawing.Point(704, 493);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(526, 69);
            this.panel9.TabIndex = 21;
            // 
            // btnImgMostraImmagine
            // 
            this.btnImgMostraImmagine.Location = new System.Drawing.Point(22, 14);
            this.btnImgMostraImmagine.Name = "btnImgMostraImmagine";
            this.btnImgMostraImmagine.Size = new System.Drawing.Size(103, 41);
            this.btnImgMostraImmagine.TabIndex = 81;
            this.btnImgMostraImmagine.Text = "Mostra Img";
            this.btnImgMostraImmagine.UseVisualStyleBackColor = true;
            this.btnImgMostraImmagine.Click += new System.EventHandler(this.btnImgMostraImmagine_Click);
            // 
            // btnImgRimuoviImmagine
            // 
            this.btnImgRimuoviImmagine.Location = new System.Drawing.Point(141, 14);
            this.btnImgRimuoviImmagine.Name = "btnImgRimuoviImmagine";
            this.btnImgRimuoviImmagine.Size = new System.Drawing.Size(103, 41);
            this.btnImgRimuoviImmagine.TabIndex = 80;
            this.btnImgRimuoviImmagine.Text = "Rimuovi";
            this.btnImgRimuoviImmagine.UseVisualStyleBackColor = true;
            this.btnImgRimuoviImmagine.Click += new System.EventHandler(this.btnImgRimuoviImmagine_Click);
            // 
            // btnImgInviaImmagine
            // 
            this.btnImgInviaImmagine.Location = new System.Drawing.Point(401, 14);
            this.btnImgInviaImmagine.Name = "btnImgInviaImmagine";
            this.btnImgInviaImmagine.Size = new System.Drawing.Size(103, 41);
            this.btnImgInviaImmagine.TabIndex = 79;
            this.btnImgInviaImmagine.Text = "Invia Img";
            this.btnImgInviaImmagine.UseVisualStyleBackColor = true;
            this.btnImgInviaImmagine.Click += new System.EventHandler(this.btnImgInviaImmagine_Click);
            // 
            // flvImgListaImmagini
            // 
            this.flvImgListaImmagini.CellEditUseWholeCell = false;
            this.flvImgListaImmagini.FullRowSelect = true;
            this.flvImgListaImmagini.Location = new System.Drawing.Point(704, 164);
            this.flvImgListaImmagini.MultiSelect = false;
            this.flvImgListaImmagini.Name = "flvImgListaImmagini";
            this.flvImgListaImmagini.ShowGroups = false;
            this.flvImgListaImmagini.Size = new System.Drawing.Size(526, 290);
            this.flvImgListaImmagini.TabIndex = 20;
            this.flvImgListaImmagini.UseCompatibleStateImageBehavior = false;
            this.flvImgListaImmagini.View = System.Windows.Forms.View.Details;
            this.flvImgListaImmagini.VirtualMode = true;
            this.flvImgListaImmagini.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.flvImgListaImmagini_MouseDoubleClick);
            // 
            // grbGeneraExcel
            // 
            this.grbGeneraExcel.BackColor = System.Drawing.Color.White;
            this.grbGeneraExcel.Controls.Add(this.btnSfoglia);
            this.grbGeneraExcel.Controls.Add(this.txtNuovoFile);
            this.grbGeneraExcel.Location = new System.Drawing.Point(704, 28);
            this.grbGeneraExcel.Name = "grbGeneraExcel";
            this.grbGeneraExcel.Size = new System.Drawing.Size(526, 78);
            this.grbGeneraExcel.TabIndex = 19;
            this.grbGeneraExcel.TabStop = false;
            this.grbGeneraExcel.Text = "Selezione File";
            // 
            // btnSfoglia
            // 
            this.btnSfoglia.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSfoglia.Location = new System.Drawing.Point(471, 29);
            this.btnSfoglia.Name = "btnSfoglia";
            this.btnSfoglia.Size = new System.Drawing.Size(33, 30);
            this.btnSfoglia.TabIndex = 6;
            this.btnSfoglia.Text = " ...";
            this.btnSfoglia.UseVisualStyleBackColor = true;
            this.btnSfoglia.Click += new System.EventHandler(this.btnSfoglia_Click);
            // 
            // txtNuovoFile
            // 
            this.txtNuovoFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtNuovoFile.Location = new System.Drawing.Point(22, 31);
            this.txtNuovoFile.Name = "txtNuovoFile";
            this.txtNuovoFile.Size = new System.Drawing.Size(429, 24);
            this.txtNuovoFile.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.txtImgNomeImmagineLista);
            this.panel2.Controls.Add(this.label41);
            this.panel2.Controls.Add(this.txtImgIdImmagine);
            this.panel2.Controls.Add(this.label40);
            this.panel2.Controls.Add(this.label26);
            this.panel2.Controls.Add(this.txtImgBaseDimY);
            this.panel2.Controls.Add(this.txtImgBaseDimX);
            this.panel2.Controls.Add(this.label29);
            this.panel2.Controls.Add(this.txtImgBaseSize);
            this.panel2.Controls.Add(this.label30);
            this.panel2.Controls.Add(this.btnImgGeneraClasse);
            this.panel2.Controls.Add(this.label20);
            this.panel2.Controls.Add(this.txtImgDimY);
            this.panel2.Controls.Add(this.txtImgDimX);
            this.panel2.Controls.Add(this.label19);
            this.panel2.Controls.Add(this.txtImgDimImmagine);
            this.panel2.Controls.Add(this.label18);
            this.panel2.Controls.Add(this.txtImgNomeImmagine);
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.btnImgGeneraArrayImmagine);
            this.panel2.Controls.Add(this.btnImgSimulaFileImmagine);
            this.panel2.Controls.Add(this.pbxImgImmagine1b);
            this.panel2.Controls.Add(this.pbxImgImmagine8b);
            this.panel2.Controls.Add(this.btnImgCaricaFileImmagine);
            this.panel2.Controls.Add(this.pbxImgImmagine);
            this.panel2.Location = new System.Drawing.Point(44, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(577, 599);
            this.panel2.TabIndex = 0;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // txtImgNomeImmagineLista
            // 
            this.txtImgNomeImmagineLista.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtImgNomeImmagineLista.Location = new System.Drawing.Point(366, 512);
            this.txtImgNomeImmagineLista.MaxLength = 50;
            this.txtImgNomeImmagineLista.Name = "txtImgNomeImmagineLista";
            this.txtImgNomeImmagineLista.Size = new System.Drawing.Size(180, 24);
            this.txtImgNomeImmagineLista.TabIndex = 88;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label41.Location = new System.Drawing.Point(363, 491);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(109, 17);
            this.label41.TabIndex = 87;
            this.label41.Text = "Nome Immagine";
            // 
            // txtImgIdImmagine
            // 
            this.txtImgIdImmagine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtImgIdImmagine.Location = new System.Drawing.Point(368, 415);
            this.txtImgIdImmagine.MaxLength = 8;
            this.txtImgIdImmagine.Name = "txtImgIdImmagine";
            this.txtImgIdImmagine.Size = new System.Drawing.Size(52, 24);
            this.txtImgIdImmagine.TabIndex = 86;
            this.txtImgIdImmagine.Text = "0";
            this.txtImgIdImmagine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtImgIdImmagine.Leave += new System.EventHandler(this.txtImgIdImmagine_Leave);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label40.Location = new System.Drawing.Point(365, 395);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(19, 17);
            this.label40.TabIndex = 85;
            this.label40.Text = "Id";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label26.Location = new System.Drawing.Point(463, 116);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(45, 17);
            this.label26.TabIndex = 84;
            this.label26.Text = "Heigh";
            // 
            // txtImgBaseDimY
            // 
            this.txtImgBaseDimY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtImgBaseDimY.Location = new System.Drawing.Point(466, 136);
            this.txtImgBaseDimY.MaxLength = 8;
            this.txtImgBaseDimY.Name = "txtImgBaseDimY";
            this.txtImgBaseDimY.Size = new System.Drawing.Size(78, 24);
            this.txtImgBaseDimY.TabIndex = 83;
            this.txtImgBaseDimY.Text = "0";
            this.txtImgBaseDimY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtImgBaseDimX
            // 
            this.txtImgBaseDimX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtImgBaseDimX.Location = new System.Drawing.Point(366, 136);
            this.txtImgBaseDimX.MaxLength = 8;
            this.txtImgBaseDimX.Name = "txtImgBaseDimX";
            this.txtImgBaseDimX.Size = new System.Drawing.Size(78, 24);
            this.txtImgBaseDimX.TabIndex = 82;
            this.txtImgBaseDimX.Text = "0";
            this.txtImgBaseDimX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label29.Location = new System.Drawing.Point(363, 116);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(44, 17);
            this.label29.TabIndex = 81;
            this.label29.Text = "Width";
            // 
            // txtImgBaseSize
            // 
            this.txtImgBaseSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtImgBaseSize.Location = new System.Drawing.Point(366, 89);
            this.txtImgBaseSize.MaxLength = 8;
            this.txtImgBaseSize.Name = "txtImgBaseSize";
            this.txtImgBaseSize.Size = new System.Drawing.Size(178, 24);
            this.txtImgBaseSize.TabIndex = 80;
            this.txtImgBaseSize.Text = "0";
            this.txtImgBaseSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label30.Location = new System.Drawing.Point(363, 69);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(35, 17);
            this.label30.TabIndex = 79;
            this.label30.Text = "Size";
            // 
            // btnImgGeneraClasse
            // 
            this.btnImgGeneraClasse.Location = new System.Drawing.Point(366, 542);
            this.btnImgGeneraClasse.Name = "btnImgGeneraClasse";
            this.btnImgGeneraClasse.Size = new System.Drawing.Size(180, 29);
            this.btnImgGeneraClasse.TabIndex = 78;
            this.btnImgGeneraClasse.Text = "Genera Classe";
            this.btnImgGeneraClasse.UseVisualStyleBackColor = true;
            this.btnImgGeneraClasse.Click += new System.EventHandler(this.btnImgGeneraClasse_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label20.Location = new System.Drawing.Point(503, 444);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(45, 17);
            this.label20.TabIndex = 77;
            this.label20.Text = "Heigh";
            // 
            // txtImgDimY
            // 
            this.txtImgDimY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtImgDimY.Location = new System.Drawing.Point(503, 464);
            this.txtImgDimY.MaxLength = 8;
            this.txtImgDimY.Name = "txtImgDimY";
            this.txtImgDimY.ReadOnly = true;
            this.txtImgDimY.Size = new System.Drawing.Size(42, 24);
            this.txtImgDimY.TabIndex = 76;
            this.txtImgDimY.Text = "0";
            this.txtImgDimY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtImgDimX
            // 
            this.txtImgDimX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtImgDimX.Location = new System.Drawing.Point(455, 464);
            this.txtImgDimX.MaxLength = 8;
            this.txtImgDimX.Name = "txtImgDimX";
            this.txtImgDimX.ReadOnly = true;
            this.txtImgDimX.Size = new System.Drawing.Size(42, 24);
            this.txtImgDimX.TabIndex = 75;
            this.txtImgDimX.Text = "0";
            this.txtImgDimX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label19.Location = new System.Drawing.Point(453, 444);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(44, 17);
            this.label19.TabIndex = 74;
            this.label19.Text = "Width";
            // 
            // txtImgDimImmagine
            // 
            this.txtImgDimImmagine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtImgDimImmagine.Location = new System.Drawing.Point(367, 464);
            this.txtImgDimImmagine.MaxLength = 8;
            this.txtImgDimImmagine.Name = "txtImgDimImmagine";
            this.txtImgDimImmagine.ReadOnly = true;
            this.txtImgDimImmagine.Size = new System.Drawing.Size(78, 24);
            this.txtImgDimImmagine.TabIndex = 73;
            this.txtImgDimImmagine.Text = "0";
            this.txtImgDimImmagine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(364, 444);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(35, 17);
            this.label18.TabIndex = 72;
            this.label18.Text = "Size";
            // 
            // txtImgNomeImmagine
            // 
            this.txtImgNomeImmagine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtImgNomeImmagine.Location = new System.Drawing.Point(445, 415);
            this.txtImgNomeImmagine.MaxLength = 8;
            this.txtImgNomeImmagine.Name = "txtImgNomeImmagine";
            this.txtImgNomeImmagine.ReadOnly = true;
            this.txtImgNomeImmagine.Size = new System.Drawing.Size(101, 24);
            this.txtImgNomeImmagine.TabIndex = 71;
            this.txtImgNomeImmagine.Text = "IMAGE001";
            this.txtImgNomeImmagine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(442, 395);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(97, 17);
            this.label17.TabIndex = 70;
            this.label17.Text = "Nome Testata";
            // 
            // btnImgGeneraArrayImmagine
            // 
            this.btnImgGeneraArrayImmagine.Location = new System.Drawing.Point(361, 257);
            this.btnImgGeneraArrayImmagine.Name = "btnImgGeneraArrayImmagine";
            this.btnImgGeneraArrayImmagine.Size = new System.Drawing.Size(176, 29);
            this.btnImgGeneraArrayImmagine.TabIndex = 5;
            this.btnImgGeneraArrayImmagine.Text = "Genera Array";
            this.btnImgGeneraArrayImmagine.UseVisualStyleBackColor = true;
            this.btnImgGeneraArrayImmagine.Click += new System.EventHandler(this.btnImgGeneraArrayImmagine_Click);
            // 
            // btnImgSimulaFileImmagine
            // 
            this.btnImgSimulaFileImmagine.Enabled = false;
            this.btnImgSimulaFileImmagine.Location = new System.Drawing.Point(361, 213);
            this.btnImgSimulaFileImmagine.Name = "btnImgSimulaFileImmagine";
            this.btnImgSimulaFileImmagine.Size = new System.Drawing.Size(178, 29);
            this.btnImgSimulaFileImmagine.TabIndex = 4;
            this.btnImgSimulaFileImmagine.Text = "Trasforma immagine";
            this.btnImgSimulaFileImmagine.UseVisualStyleBackColor = true;
            this.btnImgSimulaFileImmagine.Click += new System.EventHandler(this.btnImgSimulaFileImmagine_Click);
            // 
            // pbxImgImmagine1b
            // 
            this.pbxImgImmagine1b.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxImgImmagine1b.Location = new System.Drawing.Point(28, 400);
            this.pbxImgImmagine1b.Margin = new System.Windows.Forms.Padding(0);
            this.pbxImgImmagine1b.Name = "pbxImgImmagine1b";
            this.pbxImgImmagine1b.Size = new System.Drawing.Size(313, 171);
            this.pbxImgImmagine1b.TabIndex = 3;
            this.pbxImgImmagine1b.TabStop = false;
            // 
            // pbxImgImmagine8b
            // 
            this.pbxImgImmagine8b.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxImgImmagine8b.Location = new System.Drawing.Point(28, 213);
            this.pbxImgImmagine8b.Margin = new System.Windows.Forms.Padding(0);
            this.pbxImgImmagine8b.Name = "pbxImgImmagine8b";
            this.pbxImgImmagine8b.Size = new System.Drawing.Size(313, 165);
            this.pbxImgImmagine8b.TabIndex = 2;
            this.pbxImgImmagine8b.TabStop = false;
            // 
            // btnImgCaricaFileImmagine
            // 
            this.btnImgCaricaFileImmagine.Location = new System.Drawing.Point(366, 30);
            this.btnImgCaricaFileImmagine.Name = "btnImgCaricaFileImmagine";
            this.btnImgCaricaFileImmagine.Size = new System.Drawing.Size(178, 29);
            this.btnImgCaricaFileImmagine.TabIndex = 1;
            this.btnImgCaricaFileImmagine.Text = "Carica File";
            this.btnImgCaricaFileImmagine.UseVisualStyleBackColor = true;
            this.btnImgCaricaFileImmagine.Click += new System.EventHandler(this.btnImgCaricaFileImmagine_Click);
            // 
            // pbxImgImmagine
            // 
            this.pbxImgImmagine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxImgImmagine.Location = new System.Drawing.Point(28, 30);
            this.pbxImgImmagine.Margin = new System.Windows.Forms.Padding(0);
            this.pbxImgImmagine.Name = "pbxImgImmagine";
            this.pbxImgImmagine.Size = new System.Drawing.Size(313, 165);
            this.pbxImgImmagine.TabIndex = 0;
            this.pbxImgImmagine.TabStop = false;
            // 
            // tbpVariabili
            // 
            this.tbpVariabili.BackColor = System.Drawing.Color.LightYellow;
            this.tbpVariabili.Controls.Add(this.panel10);
            this.tbpVariabili.Controls.Add(this.flvVarListaVariabili);
            this.tbpVariabili.Controls.Add(this.groupBox2);
            this.tbpVariabili.Location = new System.Drawing.Point(4, 25);
            this.tbpVariabili.Name = "tbpVariabili";
            this.tbpVariabili.Size = new System.Drawing.Size(1625, 668);
            this.tbpVariabili.TabIndex = 3;
            this.tbpVariabili.Text = "Variabili";
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.White;
            this.panel10.Controls.Add(this.btnVarSalvaValore);
            this.panel10.Controls.Add(this.txtVarValore);
            this.panel10.Controls.Add(this.btnVarCancellaVariabile);
            this.panel10.Controls.Add(this.btnVarInviaValore);
            this.panel10.Location = new System.Drawing.Point(32, 481);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(668, 69);
            this.panel10.TabIndex = 22;
            // 
            // btnVarSalvaValore
            // 
            this.btnVarSalvaValore.Location = new System.Drawing.Point(209, 20);
            this.btnVarSalvaValore.Name = "btnVarSalvaValore";
            this.btnVarSalvaValore.Size = new System.Drawing.Size(103, 33);
            this.btnVarSalvaValore.TabIndex = 82;
            this.btnVarSalvaValore.Text = "Salva Valore";
            this.btnVarSalvaValore.UseVisualStyleBackColor = true;
            this.btnVarSalvaValore.Click += new System.EventHandler(this.btnVarSalvaValore_Click);
            // 
            // txtVarValore
            // 
            this.txtVarValore.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVarValore.Location = new System.Drawing.Point(19, 22);
            this.txtVarValore.Name = "txtVarValore";
            this.txtVarValore.Size = new System.Drawing.Size(170, 27);
            this.txtVarValore.TabIndex = 81;
            // 
            // btnVarCancellaVariabile
            // 
            this.btnVarCancellaVariabile.Location = new System.Drawing.Point(539, 16);
            this.btnVarCancellaVariabile.Name = "btnVarCancellaVariabile";
            this.btnVarCancellaVariabile.Size = new System.Drawing.Size(103, 31);
            this.btnVarCancellaVariabile.TabIndex = 80;
            this.btnVarCancellaVariabile.Text = "Rimuovi";
            this.btnVarCancellaVariabile.UseVisualStyleBackColor = true;
            // 
            // btnVarInviaValore
            // 
            this.btnVarInviaValore.Enabled = false;
            this.btnVarInviaValore.Location = new System.Drawing.Point(334, 20);
            this.btnVarInviaValore.Name = "btnVarInviaValore";
            this.btnVarInviaValore.Size = new System.Drawing.Size(103, 33);
            this.btnVarInviaValore.TabIndex = 79;
            this.btnVarInviaValore.Text = "Invia Valore";
            this.btnVarInviaValore.UseVisualStyleBackColor = true;
            this.btnVarInviaValore.Click += new System.EventHandler(this.btnVarInviaValore_Click);
            // 
            // flvVarListaVariabili
            // 
            this.flvVarListaVariabili.CellEditUseWholeCell = false;
            this.flvVarListaVariabili.FullRowSelect = true;
            this.flvVarListaVariabili.Location = new System.Drawing.Point(32, 165);
            this.flvVarListaVariabili.MultiSelect = false;
            this.flvVarListaVariabili.Name = "flvVarListaVariabili";
            this.flvVarListaVariabili.ShowGroups = false;
            this.flvVarListaVariabili.Size = new System.Drawing.Size(668, 290);
            this.flvVarListaVariabili.TabIndex = 21;
            this.flvVarListaVariabili.UseCompatibleStateImageBehavior = false;
            this.flvVarListaVariabili.View = System.Windows.Forms.View.Details;
            this.flvVarListaVariabili.VirtualMode = true;
            this.flvVarListaVariabili.DoubleClick += new System.EventHandler(this.flvVarListaVariabili_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.label44);
            this.groupBox2.Controls.Add(this.label43);
            this.groupBox2.Controls.Add(this.txtVarIdVariabile);
            this.groupBox2.Controls.Add(this.btnVarCrea);
            this.groupBox2.Controls.Add(this.txtVarNomeVariabile);
            this.groupBox2.Location = new System.Drawing.Point(32, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(668, 102);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Nuova Variabile";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label44.Location = new System.Drawing.Point(83, 27);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(45, 17);
            this.label44.TabIndex = 13;
            this.label44.Text = "Nome";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label43.Location = new System.Drawing.Point(16, 27);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(21, 17);
            this.label43.TabIndex = 12;
            this.label43.Text = "ID";
            // 
            // txtVarIdVariabile
            // 
            this.txtVarIdVariabile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVarIdVariabile.Location = new System.Drawing.Point(19, 47);
            this.txtVarIdVariabile.Name = "txtVarIdVariabile";
            this.txtVarIdVariabile.Size = new System.Drawing.Size(52, 27);
            this.txtVarIdVariabile.TabIndex = 7;
            this.txtVarIdVariabile.Leave += new System.EventHandler(this.txtVarIdVariabile_Leave);
            // 
            // btnVarCrea
            // 
            this.btnVarCrea.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnVarCrea.Location = new System.Drawing.Point(568, 46);
            this.btnVarCrea.Name = "btnVarCrea";
            this.btnVarCrea.Size = new System.Drawing.Size(74, 30);
            this.btnVarCrea.TabIndex = 6;
            this.btnVarCrea.Text = "Crea";
            this.btnVarCrea.UseVisualStyleBackColor = true;
            this.btnVarCrea.Click += new System.EventHandler(this.btnVarCrea_Click);
            // 
            // txtVarNomeVariabile
            // 
            this.txtVarNomeVariabile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVarNomeVariabile.Location = new System.Drawing.Point(86, 47);
            this.txtVarNomeVariabile.Name = "txtVarNomeVariabile";
            this.txtVarNomeVariabile.Size = new System.Drawing.Size(458, 27);
            this.txtVarNomeVariabile.TabIndex = 4;
            // 
            // tbpRealTime
            // 
            this.tbpRealTime.BackColor = System.Drawing.Color.LightYellow;
            this.tbpRealTime.Controls.Add(this.btnRtResetBoard);
            this.tbpRealTime.Controls.Add(this.grbRtPulsanti);
            this.tbpRealTime.Controls.Add(this.btnRtTestLed);
            this.tbpRealTime.Controls.Add(this.label79);
            this.tbpRealTime.Controls.Add(this.txtRtSeqSchTime);
            this.tbpRealTime.Controls.Add(this.txtRtSeqSchId);
            this.tbpRealTime.Controls.Add(this.btnRtDrawSchSequence);
            this.tbpRealTime.Controls.Add(this.txtRtIdVariabile);
            this.tbpRealTime.Controls.Add(this.txtRtValVariabile);
            this.tbpRealTime.Controls.Add(this.btnRtImpostaVariabile);
            this.tbpRealTime.Controls.Add(this.cmbRtValVariabile);
            this.tbpRealTime.Controls.Add(this.label78);
            this.tbpRealTime.Controls.Add(this.btnRtSetRTC);
            this.tbpRealTime.Controls.Add(this.btnRtDrawSchermata);
            this.tbpRealTime.Controls.Add(this.label57);
            this.tbpRealTime.Controls.Add(this.txtRtValSchId);
            this.tbpRealTime.Controls.Add(this.label58);
            this.tbpRealTime.Controls.Add(this.label46);
            this.tbpRealTime.Controls.Add(this.label45);
            this.tbpRealTime.Controls.Add(this.txtRtValTimeOffDx);
            this.tbpRealTime.Controls.Add(this.txtRtValTimeOnDx);
            this.tbpRealTime.Controls.Add(this.txtRtValBluDx);
            this.tbpRealTime.Controls.Add(this.txtRtValGreenDx);
            this.tbpRealTime.Controls.Add(this.txtRtValRedDx);
            this.tbpRealTime.Controls.Add(this.btnRtCLS);
            this.tbpRealTime.Controls.Add(this.btnRtDrawImage);
            this.tbpRealTime.Controls.Add(this.txtRtValImgColor);
            this.tbpRealTime.Controls.Add(this.label23);
            this.tbpRealTime.Controls.Add(this.txtRtValImgPosY);
            this.tbpRealTime.Controls.Add(this.label24);
            this.tbpRealTime.Controls.Add(this.txtRtValImgPosX);
            this.tbpRealTime.Controls.Add(this.label25);
            this.tbpRealTime.Controls.Add(this.label27);
            this.tbpRealTime.Controls.Add(this.txtRtValImgId);
            this.tbpRealTime.Controls.Add(this.label28);
            this.tbpRealTime.Controls.Add(this.btnRtDrawLine);
            this.tbpRealTime.Controls.Add(this.txtRtValLineColor);
            this.tbpRealTime.Controls.Add(this.label11);
            this.tbpRealTime.Controls.Add(this.txtRtValLineYFine);
            this.tbpRealTime.Controls.Add(this.label12);
            this.tbpRealTime.Controls.Add(this.txtRtValLineXFine);
            this.tbpRealTime.Controls.Add(this.label13);
            this.tbpRealTime.Controls.Add(this.txtRtValLineYStart);
            this.tbpRealTime.Controls.Add(this.label14);
            this.tbpRealTime.Controls.Add(this.label15);
            this.tbpRealTime.Controls.Add(this.txtRtValLineXStart);
            this.tbpRealTime.Controls.Add(this.label16);
            this.tbpRealTime.Controls.Add(this.btnRtStopLed);
            this.tbpRealTime.Controls.Add(this.btnRtSetLed);
            this.tbpRealTime.Controls.Add(this.txtRtValTimeOff);
            this.tbpRealTime.Controls.Add(this.label10);
            this.tbpRealTime.Controls.Add(this.txtRtValTimeOn);
            this.tbpRealTime.Controls.Add(this.label9);
            this.tbpRealTime.Controls.Add(this.txtRtValBlu);
            this.tbpRealTime.Controls.Add(this.label8);
            this.tbpRealTime.Controls.Add(this.txtRtValGreen);
            this.tbpRealTime.Controls.Add(this.label7);
            this.tbpRealTime.Controls.Add(this.label6);
            this.tbpRealTime.Controls.Add(this.txtRtValRed);
            this.tbpRealTime.Controls.Add(this.label182);
            this.tbpRealTime.Controls.Add(this.chkRtBacklight);
            this.tbpRealTime.Location = new System.Drawing.Point(4, 25);
            this.tbpRealTime.Name = "tbpRealTime";
            this.tbpRealTime.Padding = new System.Windows.Forms.Padding(3);
            this.tbpRealTime.Size = new System.Drawing.Size(1625, 668);
            this.tbpRealTime.TabIndex = 1;
            this.tbpRealTime.Text = "RealTime";
            // 
            // btnRtResetBoard
            // 
            this.btnRtResetBoard.Location = new System.Drawing.Point(1306, 526);
            this.btnRtResetBoard.Name = "btnRtResetBoard";
            this.btnRtResetBoard.Size = new System.Drawing.Size(207, 41);
            this.btnRtResetBoard.TabIndex = 125;
            this.btnRtResetBoard.Text = "Reset Scheda";
            this.btnRtResetBoard.UseVisualStyleBackColor = true;
            this.btnRtResetBoard.Click += new System.EventHandler(this.btnRtResetBoard_Click);
            // 
            // grbRtPulsanti
            // 
            this.grbRtPulsanti.BackColor = System.Drawing.Color.White;
            this.grbRtPulsanti.Controls.Add(this.btnRtLeggiPulsanti);
            this.grbRtPulsanti.Controls.Add(this.txtRtValBtn01);
            this.grbRtPulsanti.Controls.Add(this.txtRtValBtn02);
            this.grbRtPulsanti.Controls.Add(this.txtRtValBtn03);
            this.grbRtPulsanti.Controls.Add(this.txtRtValBtn04);
            this.grbRtPulsanti.Controls.Add(this.txtRtValBtn05);
            this.grbRtPulsanti.Location = new System.Drawing.Point(951, 206);
            this.grbRtPulsanti.Name = "grbRtPulsanti";
            this.grbRtPulsanti.Size = new System.Drawing.Size(246, 260);
            this.grbRtPulsanti.TabIndex = 124;
            this.grbRtPulsanti.TabStop = false;
            this.grbRtPulsanti.Text = "Pulsanti";
            // 
            // btnRtLeggiPulsanti
            // 
            this.btnRtLeggiPulsanti.Location = new System.Drawing.Point(44, 193);
            this.btnRtLeggiPulsanti.Name = "btnRtLeggiPulsanti";
            this.btnRtLeggiPulsanti.Size = new System.Drawing.Size(156, 38);
            this.btnRtLeggiPulsanti.TabIndex = 5;
            this.btnRtLeggiPulsanti.Text = "Leggi Pulsanti";
            this.btnRtLeggiPulsanti.UseVisualStyleBackColor = true;
            this.btnRtLeggiPulsanti.Click += new System.EventHandler(this.btnRtLeggiPulsanti_Click);
            // 
            // txtRtValBtn01
            // 
            this.txtRtValBtn01.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRtValBtn01.Location = new System.Drawing.Point(24, 135);
            this.txtRtValBtn01.Name = "txtRtValBtn01";
            this.txtRtValBtn01.Size = new System.Drawing.Size(81, 30);
            this.txtRtValBtn01.TabIndex = 4;
            this.txtRtValBtn01.Text = "0x00";
            this.txtRtValBtn01.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtRtValBtn02
            // 
            this.txtRtValBtn02.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRtValBtn02.Location = new System.Drawing.Point(133, 135);
            this.txtRtValBtn02.Name = "txtRtValBtn02";
            this.txtRtValBtn02.Size = new System.Drawing.Size(81, 30);
            this.txtRtValBtn02.TabIndex = 3;
            this.txtRtValBtn02.Text = "0x00";
            this.txtRtValBtn02.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtRtValBtn03
            // 
            this.txtRtValBtn03.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRtValBtn03.Location = new System.Drawing.Point(133, 89);
            this.txtRtValBtn03.Name = "txtRtValBtn03";
            this.txtRtValBtn03.Size = new System.Drawing.Size(81, 30);
            this.txtRtValBtn03.TabIndex = 2;
            this.txtRtValBtn03.Text = "0x00";
            this.txtRtValBtn03.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtRtValBtn04
            // 
            this.txtRtValBtn04.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRtValBtn04.Location = new System.Drawing.Point(133, 45);
            this.txtRtValBtn04.Name = "txtRtValBtn04";
            this.txtRtValBtn04.Size = new System.Drawing.Size(81, 30);
            this.txtRtValBtn04.TabIndex = 1;
            this.txtRtValBtn04.Text = "0x00";
            this.txtRtValBtn04.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtRtValBtn05
            // 
            this.txtRtValBtn05.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRtValBtn05.Location = new System.Drawing.Point(24, 45);
            this.txtRtValBtn05.Name = "txtRtValBtn05";
            this.txtRtValBtn05.Size = new System.Drawing.Size(81, 30);
            this.txtRtValBtn05.TabIndex = 0;
            this.txtRtValBtn05.Text = "0x00";
            this.txtRtValBtn05.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnRtTestLed
            // 
            this.btnRtTestLed.Location = new System.Drawing.Point(562, 152);
            this.btnRtTestLed.Name = "btnRtTestLed";
            this.btnRtTestLed.Size = new System.Drawing.Size(177, 41);
            this.btnRtTestLed.TabIndex = 123;
            this.btnRtTestLed.Text = "Test Led";
            this.btnRtTestLed.UseVisualStyleBackColor = true;
            this.btnRtTestLed.Click += new System.EventHandler(this.btnRtTestLed_Click);
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label79.Location = new System.Drawing.Point(483, 404);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(52, 17);
            this.label79.TabIndex = 122;
            this.label79.Text = "Tempo";
            // 
            // txtRtSeqSchTime
            // 
            this.txtRtSeqSchTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtSeqSchTime.Location = new System.Drawing.Point(485, 424);
            this.txtRtSeqSchTime.Name = "txtRtSeqSchTime";
            this.txtRtSeqSchTime.Size = new System.Drawing.Size(48, 24);
            this.txtRtSeqSchTime.TabIndex = 121;
            this.txtRtSeqSchTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRtSeqSchId
            // 
            this.txtRtSeqSchId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtSeqSchId.Location = new System.Drawing.Point(211, 424);
            this.txtRtSeqSchId.Name = "txtRtSeqSchId";
            this.txtRtSeqSchId.Size = new System.Drawing.Size(268, 24);
            this.txtRtSeqSchId.TabIndex = 120;
            this.txtRtSeqSchId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnRtDrawSchSequence
            // 
            this.btnRtDrawSchSequence.Location = new System.Drawing.Point(561, 417);
            this.btnRtDrawSchSequence.Name = "btnRtDrawSchSequence";
            this.btnRtDrawSchSequence.Size = new System.Drawing.Size(177, 41);
            this.btnRtDrawSchSequence.TabIndex = 119;
            this.btnRtDrawSchSequence.Text = "Sequenza Schermate";
            this.btnRtDrawSchSequence.UseVisualStyleBackColor = true;
            this.btnRtDrawSchSequence.Click += new System.EventHandler(this.btnRtDrawSchSequence_Click);
            // 
            // txtRtIdVariabile
            // 
            this.txtRtIdVariabile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRtIdVariabile.Location = new System.Drawing.Point(352, 532);
            this.txtRtIdVariabile.MaxLength = 10;
            this.txtRtIdVariabile.Name = "txtRtIdVariabile";
            this.txtRtIdVariabile.Size = new System.Drawing.Size(43, 27);
            this.txtRtIdVariabile.TabIndex = 118;
            this.txtRtIdVariabile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRtValVariabile
            // 
            this.txtRtValVariabile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRtValVariabile.Location = new System.Drawing.Point(397, 532);
            this.txtRtValVariabile.MaxLength = 10;
            this.txtRtValVariabile.Name = "txtRtValVariabile";
            this.txtRtValVariabile.Size = new System.Drawing.Size(136, 27);
            this.txtRtValVariabile.TabIndex = 117;
            this.txtRtValVariabile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnRtImpostaVariabile
            // 
            this.btnRtImpostaVariabile.Location = new System.Drawing.Point(562, 525);
            this.btnRtImpostaVariabile.Name = "btnRtImpostaVariabile";
            this.btnRtImpostaVariabile.Size = new System.Drawing.Size(177, 41);
            this.btnRtImpostaVariabile.TabIndex = 116;
            this.btnRtImpostaVariabile.Text = "Imposta Variabile";
            this.btnRtImpostaVariabile.UseVisualStyleBackColor = true;
            this.btnRtImpostaVariabile.Click += new System.EventHandler(this.btnRtImpostaVariabile_Click);
            // 
            // cmbRtValVariabile
            // 
            this.cmbRtValVariabile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRtValVariabile.FormattingEnabled = true;
            this.cmbRtValVariabile.Items.AddRange(new object[] {
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cmbRtValVariabile.Location = new System.Drawing.Point(211, 534);
            this.cmbRtValVariabile.Name = "cmbRtValVariabile";
            this.cmbRtValVariabile.Size = new System.Drawing.Size(131, 24);
            this.cmbRtValVariabile.TabIndex = 115;
            this.cmbRtValVariabile.SelectedValueChanged += new System.EventHandler(this.cmbRtValVariabile_SelectedValueChanged);
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label78.Location = new System.Drawing.Point(63, 538);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(104, 20);
            this.label78.TabIndex = 114;
            this.label78.Text = "VARIABILE";
            // 
            // btnRtSetRTC
            // 
            this.btnRtSetRTC.Location = new System.Drawing.Point(1306, 425);
            this.btnRtSetRTC.Name = "btnRtSetRTC";
            this.btnRtSetRTC.Size = new System.Drawing.Size(207, 41);
            this.btnRtSetRTC.TabIndex = 110;
            this.btnRtSetRTC.Text = "Imposta Orologio";
            this.btnRtSetRTC.UseVisualStyleBackColor = true;
            this.btnRtSetRTC.Click += new System.EventHandler(this.btnRtSetRTC_Click);
            // 
            // btnRtDrawSchermata
            // 
            this.btnRtDrawSchermata.Location = new System.Drawing.Point(561, 370);
            this.btnRtDrawSchermata.Name = "btnRtDrawSchermata";
            this.btnRtDrawSchermata.Size = new System.Drawing.Size(177, 41);
            this.btnRtDrawSchermata.TabIndex = 109;
            this.btnRtDrawSchermata.Text = "Disegna Schermata";
            this.btnRtDrawSchermata.UseVisualStyleBackColor = true;
            this.btnRtDrawSchermata.Click += new System.EventHandler(this.btnRtDrawSchermata_Click);
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label57.Location = new System.Drawing.Point(64, 384);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(123, 20);
            this.label57.TabIndex = 102;
            this.label57.Text = "SCHERMATA";
            // 
            // txtRtValSchId
            // 
            this.txtRtValSchId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValSchId.Location = new System.Drawing.Point(211, 380);
            this.txtRtValSchId.Name = "txtRtValSchId";
            this.txtRtValSchId.Size = new System.Drawing.Size(74, 24);
            this.txtRtValSchId.TabIndex = 101;
            this.txtRtValSchId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label58.Location = new System.Drawing.Point(214, 360);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(21, 17);
            this.label58.TabIndex = 100;
            this.label58.Text = "ID";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label46.Location = new System.Drawing.Point(209, 157);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(27, 17);
            this.label46.TabIndex = 99;
            this.label46.Text = "DX";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label45.Location = new System.Drawing.Point(208, 127);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(26, 17);
            this.label45.TabIndex = 98;
            this.label45.Text = "SX";
            // 
            // txtRtValTimeOffDx
            // 
            this.txtRtValTimeOffDx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValTimeOffDx.Location = new System.Drawing.Point(485, 152);
            this.txtRtValTimeOffDx.Name = "txtRtValTimeOffDx";
            this.txtRtValTimeOffDx.Size = new System.Drawing.Size(48, 24);
            this.txtRtValTimeOffDx.TabIndex = 97;
            this.txtRtValTimeOffDx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRtValTimeOnDx
            // 
            this.txtRtValTimeOnDx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValTimeOnDx.Location = new System.Drawing.Point(431, 152);
            this.txtRtValTimeOnDx.Name = "txtRtValTimeOnDx";
            this.txtRtValTimeOnDx.Size = new System.Drawing.Size(48, 24);
            this.txtRtValTimeOnDx.TabIndex = 96;
            this.txtRtValTimeOnDx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRtValBluDx
            // 
            this.txtRtValBluDx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValBluDx.Location = new System.Drawing.Point(349, 152);
            this.txtRtValBluDx.Name = "txtRtValBluDx";
            this.txtRtValBluDx.Size = new System.Drawing.Size(48, 24);
            this.txtRtValBluDx.TabIndex = 95;
            this.txtRtValBluDx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRtValGreenDx
            // 
            this.txtRtValGreenDx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValGreenDx.Location = new System.Drawing.Point(295, 152);
            this.txtRtValGreenDx.Name = "txtRtValGreenDx";
            this.txtRtValGreenDx.Size = new System.Drawing.Size(48, 24);
            this.txtRtValGreenDx.TabIndex = 94;
            this.txtRtValGreenDx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRtValRedDx
            // 
            this.txtRtValRedDx.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValRedDx.Location = new System.Drawing.Point(241, 152);
            this.txtRtValRedDx.Name = "txtRtValRedDx";
            this.txtRtValRedDx.Size = new System.Drawing.Size(48, 24);
            this.txtRtValRedDx.TabIndex = 93;
            this.txtRtValRedDx.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnRtCLS
            // 
            this.btnRtCLS.Location = new System.Drawing.Point(767, 239);
            this.btnRtCLS.Name = "btnRtCLS";
            this.btnRtCLS.Size = new System.Drawing.Size(68, 172);
            this.btnRtCLS.TabIndex = 92;
            this.btnRtCLS.Text = "CLS";
            this.btnRtCLS.UseVisualStyleBackColor = true;
            this.btnRtCLS.Click += new System.EventHandler(this.btnRtCLS_Click);
            // 
            // btnRtDrawImage
            // 
            this.btnRtDrawImage.Location = new System.Drawing.Point(562, 302);
            this.btnRtDrawImage.Name = "btnRtDrawImage";
            this.btnRtDrawImage.Size = new System.Drawing.Size(176, 41);
            this.btnRtDrawImage.TabIndex = 91;
            this.btnRtDrawImage.Text = "Disegna Immagine";
            this.btnRtDrawImage.UseVisualStyleBackColor = true;
            this.btnRtDrawImage.Click += new System.EventHandler(this.btnRtDrawImage_Click);
            // 
            // txtRtValImgColor
            // 
            this.txtRtValImgColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValImgColor.Location = new System.Drawing.Point(485, 310);
            this.txtRtValImgColor.Name = "txtRtValImgColor";
            this.txtRtValImgColor.Size = new System.Drawing.Size(48, 24);
            this.txtRtValImgColor.TabIndex = 90;
            this.txtRtValImgColor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label23.Location = new System.Drawing.Point(482, 290);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(49, 17);
            this.label23.TabIndex = 89;
            this.label23.Text = "Colore";
            // 
            // txtRtValImgPosY
            // 
            this.txtRtValImgPosY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValImgPosY.Location = new System.Drawing.Point(410, 310);
            this.txtRtValImgPosY.Name = "txtRtValImgPosY";
            this.txtRtValImgPosY.Size = new System.Drawing.Size(48, 24);
            this.txtRtValImgPosY.TabIndex = 88;
            this.txtRtValImgPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label24.Location = new System.Drawing.Point(406, 290);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(49, 17);
            this.label24.TabIndex = 87;
            this.label24.Text = "Pos. Y";
            // 
            // txtRtValImgPosX
            // 
            this.txtRtValImgPosX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValImgPosX.Location = new System.Drawing.Point(352, 310);
            this.txtRtValImgPosX.Name = "txtRtValImgPosX";
            this.txtRtValImgPosX.Size = new System.Drawing.Size(48, 24);
            this.txtRtValImgPosX.TabIndex = 86;
            this.txtRtValImgPosX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label25.Location = new System.Drawing.Point(349, 290);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(49, 17);
            this.label25.TabIndex = 85;
            this.label25.Text = "Pos. X";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(63, 313);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(100, 20);
            this.label27.TabIndex = 82;
            this.label27.Text = "IMMAGINE";
            // 
            // txtRtValImgId
            // 
            this.txtRtValImgId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValImgId.Location = new System.Drawing.Point(212, 310);
            this.txtRtValImgId.Name = "txtRtValImgId";
            this.txtRtValImgId.Size = new System.Drawing.Size(102, 24);
            this.txtRtValImgId.TabIndex = 81;
            this.txtRtValImgId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label28.Location = new System.Drawing.Point(209, 290);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(21, 17);
            this.label28.TabIndex = 80;
            this.label28.Text = "ID";
            // 
            // btnRtDrawLine
            // 
            this.btnRtDrawLine.Location = new System.Drawing.Point(561, 239);
            this.btnRtDrawLine.Name = "btnRtDrawLine";
            this.btnRtDrawLine.Size = new System.Drawing.Size(177, 41);
            this.btnRtDrawLine.TabIndex = 79;
            this.btnRtDrawLine.Text = "Disegna Linea";
            this.btnRtDrawLine.UseVisualStyleBackColor = true;
            this.btnRtDrawLine.Click += new System.EventHandler(this.btnRtDrawLine_Click);
            // 
            // txtRtValLineColor
            // 
            this.txtRtValLineColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValLineColor.Location = new System.Drawing.Point(484, 247);
            this.txtRtValLineColor.Name = "txtRtValLineColor";
            this.txtRtValLineColor.Size = new System.Drawing.Size(48, 24);
            this.txtRtValLineColor.TabIndex = 78;
            this.txtRtValLineColor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(481, 227);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 17);
            this.label11.TabIndex = 77;
            this.label11.Text = "Colore";
            // 
            // txtRtValLineYFine
            // 
            this.txtRtValLineYFine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValLineYFine.Location = new System.Drawing.Point(409, 247);
            this.txtRtValLineYFine.Name = "txtRtValLineYFine";
            this.txtRtValLineYFine.Size = new System.Drawing.Size(48, 24);
            this.txtRtValLineYFine.TabIndex = 76;
            this.txtRtValLineYFine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(405, 227);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 17);
            this.label12.TabIndex = 75;
            this.label12.Text = "Y fine";
            // 
            // txtRtValLineXFine
            // 
            this.txtRtValLineXFine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValLineXFine.Location = new System.Drawing.Point(351, 247);
            this.txtRtValLineXFine.Name = "txtRtValLineXFine";
            this.txtRtValLineXFine.Size = new System.Drawing.Size(48, 24);
            this.txtRtValLineXFine.TabIndex = 74;
            this.txtRtValLineXFine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(348, 227);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 17);
            this.label13.TabIndex = 73;
            this.label13.Text = "X fine";
            // 
            // txtRtValLineYStart
            // 
            this.txtRtValLineYStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValLineYStart.Location = new System.Drawing.Point(265, 247);
            this.txtRtValLineYStart.Name = "txtRtValLineYStart";
            this.txtRtValLineYStart.Size = new System.Drawing.Size(48, 24);
            this.txtRtValLineYStart.TabIndex = 72;
            this.txtRtValLineYStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(262, 227);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 17);
            this.label14.TabIndex = 71;
            this.label14.Text = "Y start";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(64, 248);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 20);
            this.label15.TabIndex = 70;
            this.label15.Text = "LINEA";
            // 
            // txtRtValLineXStart
            // 
            this.txtRtValLineXStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValLineXStart.Location = new System.Drawing.Point(211, 247);
            this.txtRtValLineXStart.Name = "txtRtValLineXStart";
            this.txtRtValLineXStart.Size = new System.Drawing.Size(48, 24);
            this.txtRtValLineXStart.TabIndex = 69;
            this.txtRtValLineXStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(208, 227);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(49, 17);
            this.label16.TabIndex = 68;
            this.label16.Text = "X start";
            // 
            // btnRtStopLed
            // 
            this.btnRtStopLed.Location = new System.Drawing.Point(767, 103);
            this.btnRtStopLed.Name = "btnRtStopLed";
            this.btnRtStopLed.Size = new System.Drawing.Size(136, 41);
            this.btnRtStopLed.TabIndex = 67;
            this.btnRtStopLed.Text = "Reset Led";
            this.btnRtStopLed.UseVisualStyleBackColor = true;
            this.btnRtStopLed.Click += new System.EventHandler(this.btnRtStopLed_Click);
            // 
            // btnRtSetLed
            // 
            this.btnRtSetLed.Location = new System.Drawing.Point(561, 103);
            this.btnRtSetLed.Name = "btnRtSetLed";
            this.btnRtSetLed.Size = new System.Drawing.Size(177, 41);
            this.btnRtSetLed.TabIndex = 66;
            this.btnRtSetLed.Text = "Set Led";
            this.btnRtSetLed.UseVisualStyleBackColor = true;
            this.btnRtSetLed.Click += new System.EventHandler(this.btnRtSetLed_Click);
            // 
            // txtRtValTimeOff
            // 
            this.txtRtValTimeOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValTimeOff.Location = new System.Drawing.Point(485, 122);
            this.txtRtValTimeOff.Name = "txtRtValTimeOff";
            this.txtRtValTimeOff.Size = new System.Drawing.Size(48, 24);
            this.txtRtValTimeOff.TabIndex = 65;
            this.txtRtValTimeOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(483, 102);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 17);
            this.label10.TabIndex = 64;
            this.label10.Text = "OFF";
            // 
            // txtRtValTimeOn
            // 
            this.txtRtValTimeOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValTimeOn.Location = new System.Drawing.Point(431, 122);
            this.txtRtValTimeOn.Name = "txtRtValTimeOn";
            this.txtRtValTimeOn.Size = new System.Drawing.Size(48, 24);
            this.txtRtValTimeOn.TabIndex = 63;
            this.txtRtValTimeOn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(429, 102);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 17);
            this.label9.TabIndex = 62;
            this.label9.Text = "ON";
            // 
            // txtRtValBlu
            // 
            this.txtRtValBlu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValBlu.Location = new System.Drawing.Point(348, 122);
            this.txtRtValBlu.Name = "txtRtValBlu";
            this.txtRtValBlu.Size = new System.Drawing.Size(48, 24);
            this.txtRtValBlu.TabIndex = 61;
            this.txtRtValBlu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(348, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 17);
            this.label8.TabIndex = 60;
            this.label8.Text = "BLU";
            // 
            // txtRtValGreen
            // 
            this.txtRtValGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValGreen.Location = new System.Drawing.Point(294, 122);
            this.txtRtValGreen.Name = "txtRtValGreen";
            this.txtRtValGreen.Size = new System.Drawing.Size(48, 24);
            this.txtRtValGreen.TabIndex = 59;
            this.txtRtValGreen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(291, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 17);
            this.label7.TabIndex = 58;
            this.label7.Text = "GREEN";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(65, 143);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 20);
            this.label6.TabIndex = 57;
            this.label6.Text = "LED";
            // 
            // txtRtValRed
            // 
            this.txtRtValRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValRed.Location = new System.Drawing.Point(240, 122);
            this.txtRtValRed.Name = "txtRtValRed";
            this.txtRtValRed.Size = new System.Drawing.Size(48, 24);
            this.txtRtValRed.TabIndex = 56;
            this.txtRtValRed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label182
            // 
            this.label182.AutoSize = true;
            this.label182.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label182.Location = new System.Drawing.Point(237, 102);
            this.label182.Name = "label182";
            this.label182.Size = new System.Drawing.Size(37, 17);
            this.label182.TabIndex = 55;
            this.label182.Text = "RED";
            // 
            // chkRtBacklight
            // 
            this.chkRtBacklight.AutoSize = true;
            this.chkRtBacklight.Location = new System.Drawing.Point(67, 65);
            this.chkRtBacklight.Name = "chkRtBacklight";
            this.chkRtBacklight.Size = new System.Drawing.Size(87, 21);
            this.chkRtBacklight.TabIndex = 0;
            this.chkRtBacklight.Text = "Backlight";
            this.chkRtBacklight.UseVisualStyleBackColor = true;
            this.chkRtBacklight.CheckedChanged += new System.EventHandler(this.chkRtBacklight_CheckedChanged);
            // 
            // tbpAccesssoMemoria
            // 
            this.tbpAccesssoMemoria.BackColor = System.Drawing.Color.LightYellow;
            this.tbpAccesssoMemoria.Controls.Add(this.grbMemScrittura);
            this.tbpAccesssoMemoria.Controls.Add(this.grbMemCancFisica);
            this.tbpAccesssoMemoria.Controls.Add(this.grbMemCancellazione);
            this.tbpAccesssoMemoria.Controls.Add(this.txtMemDataGrid);
            this.tbpAccesssoMemoria.Controls.Add(this.grbMemLettura);
            this.tbpAccesssoMemoria.Location = new System.Drawing.Point(4, 25);
            this.tbpAccesssoMemoria.Name = "tbpAccesssoMemoria";
            this.tbpAccesssoMemoria.Size = new System.Drawing.Size(1625, 668);
            this.tbpAccesssoMemoria.TabIndex = 5;
            this.tbpAccesssoMemoria.Text = "Accesso Memoria";
            // 
            // grbMemScrittura
            // 
            this.grbMemScrittura.BackColor = System.Drawing.Color.White;
            this.grbMemScrittura.Controls.Add(this.chkMemHexW);
            this.grbMemScrittura.Controls.Add(this.lblMemVerificaValore);
            this.grbMemScrittura.Controls.Add(this.label21);
            this.grbMemScrittura.Controls.Add(this.txtMemDataW);
            this.grbMemScrittura.Controls.Add(this.label22);
            this.grbMemScrittura.Controls.Add(this.label31);
            this.grbMemScrittura.Controls.Add(this.txtMemLenW);
            this.grbMemScrittura.Controls.Add(this.txtMemAddrW);
            this.grbMemScrittura.Controls.Add(this.cmdMemWrite);
            this.grbMemScrittura.Location = new System.Drawing.Point(21, 143);
            this.grbMemScrittura.Name = "grbMemScrittura";
            this.grbMemScrittura.Size = new System.Drawing.Size(884, 111);
            this.grbMemScrittura.TabIndex = 85;
            this.grbMemScrittura.TabStop = false;
            this.grbMemScrittura.Text = "Scrittura Memoria";
            // 
            // chkMemHexW
            // 
            this.chkMemHexW.AutoSize = true;
            this.chkMemHexW.Checked = true;
            this.chkMemHexW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemHexW.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkMemHexW.Location = new System.Drawing.Point(207, 76);
            this.chkMemHexW.Name = "chkMemHexW";
            this.chkMemHexW.Size = new System.Drawing.Size(110, 21);
            this.chkMemHexW.TabIndex = 17;
            this.chkMemHexW.Text = "Hex Address";
            this.chkMemHexW.UseVisualStyleBackColor = true;
            // 
            // lblMemVerificaValore
            // 
            this.lblMemVerificaValore.AutoSize = true;
            this.lblMemVerificaValore.BackColor = System.Drawing.Color.Transparent;
            this.lblMemVerificaValore.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F);
            this.lblMemVerificaValore.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblMemVerificaValore.Location = new System.Drawing.Point(375, 77);
            this.lblMemVerificaValore.Name = "lblMemVerificaValore";
            this.lblMemVerificaValore.Size = new System.Drawing.Size(16, 15);
            this.lblMemVerificaValore.TabIndex = 16;
            this.lblMemVerificaValore.Text = "...";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label21.Location = new System.Drawing.Point(375, 26);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(91, 17);
            this.label21.TabIndex = 14;
            this.label21.Text = "Valore (HEX)";
            // 
            // txtMemDataW
            // 
            this.txtMemDataW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtMemDataW.Location = new System.Drawing.Point(378, 46);
            this.txtMemDataW.Name = "txtMemDataW";
            this.txtMemDataW.Size = new System.Drawing.Size(480, 24);
            this.txtMemDataW.TabIndex = 13;
            this.txtMemDataW.Text = "FF";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label22.Location = new System.Drawing.Point(318, 26);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(43, 17);
            this.label22.TabIndex = 12;
            this.label22.Text = "Bytes";
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label31.Location = new System.Drawing.Point(204, 26);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(107, 17);
            this.label31.TabIndex = 11;
            this.label31.Text = "Indirizzo Iniziale";
            // 
            // txtMemLenW
            // 
            this.txtMemLenW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtMemLenW.Location = new System.Drawing.Point(321, 46);
            this.txtMemLenW.Name = "txtMemLenW";
            this.txtMemLenW.ReadOnly = true;
            this.txtMemLenW.Size = new System.Drawing.Size(51, 24);
            this.txtMemLenW.TabIndex = 10;
            this.txtMemLenW.Text = "1";
            this.txtMemLenW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtMemAddrW
            // 
            this.txtMemAddrW.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtMemAddrW.Location = new System.Drawing.Point(207, 46);
            this.txtMemAddrW.Name = "txtMemAddrW";
            this.txtMemAddrW.Size = new System.Drawing.Size(104, 24);
            this.txtMemAddrW.TabIndex = 9;
            this.txtMemAddrW.Text = "0";
            this.txtMemAddrW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmdMemWrite
            // 
            this.cmdMemWrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.cmdMemWrite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdMemWrite.Location = new System.Drawing.Point(24, 38);
            this.cmdMemWrite.Name = "cmdMemWrite";
            this.cmdMemWrite.Size = new System.Drawing.Size(155, 41);
            this.cmdMemWrite.TabIndex = 8;
            this.cmdMemWrite.Text = "Scrittura Diretta";
            this.cmdMemWrite.UseVisualStyleBackColor = true;
            // 
            // grbMemCancFisica
            // 
            this.grbMemCancFisica.BackColor = System.Drawing.Color.White;
            this.grbMemCancFisica.Controls.Add(this.rbtMemAreaApp2);
            this.grbMemCancFisica.Controls.Add(this.rbtMemAreaApp1);
            this.grbMemCancFisica.Controls.Add(this.rbtMemAreaLibera);
            this.grbMemCancFisica.Controls.Add(this.label111);
            this.grbMemCancFisica.Controls.Add(this.txtMemCFBlocchi);
            this.grbMemCancFisica.Controls.Add(this.chkMemCFStartAddHex);
            this.grbMemCancFisica.Controls.Add(this.label112);
            this.grbMemCancFisica.Controls.Add(this.txtMemCFStartAdd);
            this.grbMemCancFisica.Controls.Add(this.btnMemCFExec);
            this.grbMemCancFisica.Cursor = System.Windows.Forms.Cursors.Default;
            this.grbMemCancFisica.Location = new System.Drawing.Point(944, 17);
            this.grbMemCancFisica.Name = "grbMemCancFisica";
            this.grbMemCancFisica.Size = new System.Drawing.Size(405, 237);
            this.grbMemCancFisica.TabIndex = 84;
            this.grbMemCancFisica.TabStop = false;
            this.grbMemCancFisica.Text = "Cancellazione 4K Memoria";
            // 
            // rbtMemAreaApp2
            // 
            this.rbtMemAreaApp2.AutoSize = true;
            this.rbtMemAreaApp2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbtMemAreaApp2.Location = new System.Drawing.Point(18, 165);
            this.rbtMemAreaApp2.Name = "rbtMemAreaApp2";
            this.rbtMemAreaApp2.Size = new System.Drawing.Size(102, 21);
            this.rbtMemAreaApp2.TabIndex = 18;
            this.rbtMemAreaApp2.Text = "Area APP 2";
            this.rbtMemAreaApp2.UseVisualStyleBackColor = true;
            // 
            // rbtMemAreaApp1
            // 
            this.rbtMemAreaApp1.AutoSize = true;
            this.rbtMemAreaApp1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbtMemAreaApp1.Location = new System.Drawing.Point(18, 138);
            this.rbtMemAreaApp1.Name = "rbtMemAreaApp1";
            this.rbtMemAreaApp1.Size = new System.Drawing.Size(102, 21);
            this.rbtMemAreaApp1.TabIndex = 17;
            this.rbtMemAreaApp1.Text = "Area APP 1";
            this.rbtMemAreaApp1.UseVisualStyleBackColor = true;
            // 
            // rbtMemAreaLibera
            // 
            this.rbtMemAreaLibera.AutoSize = true;
            this.rbtMemAreaLibera.Checked = true;
            this.rbtMemAreaLibera.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.rbtMemAreaLibera.Location = new System.Drawing.Point(18, 111);
            this.rbtMemAreaLibera.Name = "rbtMemAreaLibera";
            this.rbtMemAreaLibera.Size = new System.Drawing.Size(113, 21);
            this.rbtMemAreaLibera.TabIndex = 16;
            this.rbtMemAreaLibera.TabStop = true;
            this.rbtMemAreaLibera.Text = "Zona Custom";
            this.rbtMemAreaLibera.UseVisualStyleBackColor = true;
            // 
            // label111
            // 
            this.label111.AutoSize = true;
            this.label111.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label111.Location = new System.Drawing.Point(140, 18);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(53, 17);
            this.label111.TabIndex = 15;
            this.label111.Text = "Blocchi";
            // 
            // txtMemCFBlocchi
            // 
            this.txtMemCFBlocchi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtMemCFBlocchi.Location = new System.Drawing.Point(143, 36);
            this.txtMemCFBlocchi.Name = "txtMemCFBlocchi";
            this.txtMemCFBlocchi.Size = new System.Drawing.Size(51, 24);
            this.txtMemCFBlocchi.TabIndex = 14;
            this.txtMemCFBlocchi.Text = "1";
            this.txtMemCFBlocchi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkMemCFStartAddHex
            // 
            this.chkMemCFStartAddHex.AutoSize = true;
            this.chkMemCFStartAddHex.Checked = true;
            this.chkMemCFStartAddHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemCFStartAddHex.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkMemCFStartAddHex.Location = new System.Drawing.Point(18, 67);
            this.chkMemCFStartAddHex.Name = "chkMemCFStartAddHex";
            this.chkMemCFStartAddHex.Size = new System.Drawing.Size(110, 21);
            this.chkMemCFStartAddHex.TabIndex = 13;
            this.chkMemCFStartAddHex.Text = "Hex Address";
            this.chkMemCFStartAddHex.UseVisualStyleBackColor = true;
            // 
            // label112
            // 
            this.label112.AutoSize = true;
            this.label112.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label112.Location = new System.Drawing.Point(15, 19);
            this.label112.Name = "label112";
            this.label112.Size = new System.Drawing.Size(107, 17);
            this.label112.TabIndex = 11;
            this.label112.Text = "Indirizzo Iniziale";
            // 
            // txtMemCFStartAdd
            // 
            this.txtMemCFStartAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtMemCFStartAdd.Location = new System.Drawing.Point(18, 37);
            this.txtMemCFStartAdd.Name = "txtMemCFStartAdd";
            this.txtMemCFStartAdd.Size = new System.Drawing.Size(104, 24);
            this.txtMemCFStartAdd.TabIndex = 9;
            this.txtMemCFStartAdd.Text = "0";
            this.txtMemCFStartAdd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnMemCFExec
            // 
            this.btnMemCFExec.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnMemCFExec.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMemCFExec.Location = new System.Drawing.Point(223, 36);
            this.btnMemCFExec.Name = "btnMemCFExec";
            this.btnMemCFExec.Size = new System.Drawing.Size(155, 51);
            this.btnMemCFExec.TabIndex = 8;
            this.btnMemCFExec.Text = "Esegui";
            this.btnMemCFExec.UseVisualStyleBackColor = true;
            this.btnMemCFExec.Click += new System.EventHandler(this.btnMemCFExec_Click);
            // 
            // grbMemCancellazione
            // 
            this.grbMemCancellazione.BackColor = System.Drawing.Color.White;
            this.grbMemCancellazione.Controls.Add(this.cmdMemClear);
            this.grbMemCancellazione.Location = new System.Drawing.Point(944, 280);
            this.grbMemCancellazione.Name = "grbMemCancellazione";
            this.grbMemCancellazione.Size = new System.Drawing.Size(202, 109);
            this.grbMemCancellazione.TabIndex = 83;
            this.grbMemCancellazione.TabStop = false;
            this.grbMemCancellazione.Text = "Cancellazione Memoria";
            this.grbMemCancellazione.Visible = false;
            // 
            // cmdMemClear
            // 
            this.cmdMemClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.cmdMemClear.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdMemClear.Location = new System.Drawing.Point(13, 37);
            this.cmdMemClear.Name = "cmdMemClear";
            this.cmdMemClear.Size = new System.Drawing.Size(163, 51);
            this.cmdMemClear.TabIndex = 8;
            this.cmdMemClear.Text = "Cancellazione";
            this.cmdMemClear.UseVisualStyleBackColor = true;
            this.cmdMemClear.Click += new System.EventHandler(this.cmdMemClear_Click);
            // 
            // txtMemDataGrid
            // 
            this.txtMemDataGrid.Font = new System.Drawing.Font("Courier New", 12F);
            this.txtMemDataGrid.Location = new System.Drawing.Point(21, 280);
            this.txtMemDataGrid.Multiline = true;
            this.txtMemDataGrid.Name = "txtMemDataGrid";
            this.txtMemDataGrid.Size = new System.Drawing.Size(884, 291);
            this.txtMemDataGrid.TabIndex = 82;
            // 
            // grbMemLettura
            // 
            this.grbMemLettura.BackColor = System.Drawing.Color.White;
            this.grbMemLettura.Controls.Add(this.chkMemHex);
            this.grbMemLettura.Controls.Add(this.lblReadMemBytes);
            this.grbMemLettura.Controls.Add(this.lblReadMemStartAddr);
            this.grbMemLettura.Controls.Add(this.txtMemLenR);
            this.grbMemLettura.Controls.Add(this.txtMemAddrR);
            this.grbMemLettura.Controls.Add(this.cmdMemRead);
            this.grbMemLettura.Cursor = System.Windows.Forms.Cursors.Default;
            this.grbMemLettura.Location = new System.Drawing.Point(21, 17);
            this.grbMemLettura.Name = "grbMemLettura";
            this.grbMemLettura.Size = new System.Drawing.Size(397, 109);
            this.grbMemLettura.TabIndex = 81;
            this.grbMemLettura.TabStop = false;
            this.grbMemLettura.Text = "Lettura Memoria";
            // 
            // chkMemHex
            // 
            this.chkMemHex.AutoSize = true;
            this.chkMemHex.Checked = true;
            this.chkMemHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemHex.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkMemHex.Location = new System.Drawing.Point(18, 67);
            this.chkMemHex.Name = "chkMemHex";
            this.chkMemHex.Size = new System.Drawing.Size(110, 21);
            this.chkMemHex.TabIndex = 13;
            this.chkMemHex.Text = "Hex Address";
            this.chkMemHex.UseVisualStyleBackColor = true;
            // 
            // lblReadMemBytes
            // 
            this.lblReadMemBytes.AutoSize = true;
            this.lblReadMemBytes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblReadMemBytes.Location = new System.Drawing.Point(140, 19);
            this.lblReadMemBytes.Name = "lblReadMemBytes";
            this.lblReadMemBytes.Size = new System.Drawing.Size(43, 17);
            this.lblReadMemBytes.TabIndex = 12;
            this.lblReadMemBytes.Text = "Bytes";
            // 
            // lblReadMemStartAddr
            // 
            this.lblReadMemStartAddr.AutoSize = true;
            this.lblReadMemStartAddr.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblReadMemStartAddr.Location = new System.Drawing.Point(15, 19);
            this.lblReadMemStartAddr.Name = "lblReadMemStartAddr";
            this.lblReadMemStartAddr.Size = new System.Drawing.Size(107, 17);
            this.lblReadMemStartAddr.TabIndex = 11;
            this.lblReadMemStartAddr.Text = "Indirizzo Iniziale";
            // 
            // txtMemLenR
            // 
            this.txtMemLenR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtMemLenR.Location = new System.Drawing.Point(143, 37);
            this.txtMemLenR.Name = "txtMemLenR";
            this.txtMemLenR.Size = new System.Drawing.Size(51, 24);
            this.txtMemLenR.TabIndex = 10;
            this.txtMemLenR.Text = "64";
            this.txtMemLenR.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtMemAddrR
            // 
            this.txtMemAddrR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtMemAddrR.Location = new System.Drawing.Point(18, 37);
            this.txtMemAddrR.Name = "txtMemAddrR";
            this.txtMemAddrR.Size = new System.Drawing.Size(104, 24);
            this.txtMemAddrR.TabIndex = 9;
            this.txtMemAddrR.Text = "0";
            this.txtMemAddrR.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmdMemRead
            // 
            this.cmdMemRead.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.cmdMemRead.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdMemRead.Location = new System.Drawing.Point(217, 36);
            this.cmdMemRead.Name = "cmdMemRead";
            this.cmdMemRead.Size = new System.Drawing.Size(155, 51);
            this.cmdMemRead.TabIndex = 8;
            this.cmdMemRead.Text = "Lettura Diretta";
            this.cmdMemRead.UseVisualStyleBackColor = true;
            this.cmdMemRead.Click += new System.EventHandler(this.cmdMemRead_Click);
            // 
            // tbpStatoScheda
            // 
            this.tbpStatoScheda.BackColor = System.Drawing.Color.LightYellow;
            this.tbpStatoScheda.Controls.Add(this.pgbStatoAvanzamento);
            this.tbpStatoScheda.Controls.Add(this.label54);
            this.tbpStatoScheda.Controls.Add(this.panel15);
            this.tbpStatoScheda.Controls.Add(this.flvStatoListaSch);
            this.tbpStatoScheda.Controls.Add(this.lblStatoImmagini);
            this.tbpStatoScheda.Controls.Add(this.panel11);
            this.tbpStatoScheda.Controls.Add(this.flvStatoListaImg);
            this.tbpStatoScheda.Location = new System.Drawing.Point(4, 25);
            this.tbpStatoScheda.Name = "tbpStatoScheda";
            this.tbpStatoScheda.Padding = new System.Windows.Forms.Padding(3);
            this.tbpStatoScheda.Size = new System.Drawing.Size(1625, 723);
            this.tbpStatoScheda.TabIndex = 7;
            this.tbpStatoScheda.Text = "Stato Scheda";
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label54.Location = new System.Drawing.Point(467, 30);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(123, 20);
            this.label54.TabIndex = 26;
            this.label54.Text = "SCHERMATE";
            // 
            // panel15
            // 
            this.panel15.BackColor = System.Drawing.Color.White;
            this.panel15.Controls.Add(this.checkBox1);
            this.panel15.Controls.Add(this.textBox1);
            this.panel15.Controls.Add(this.label55);
            this.panel15.Controls.Add(this.textBox2);
            this.panel15.Controls.Add(this.label56);
            this.panel15.Controls.Add(this.btnStatoSchCarica);
            this.panel15.Location = new System.Drawing.Point(471, 53);
            this.panel15.Name = "panel15";
            this.panel15.Size = new System.Drawing.Size(382, 69);
            this.panel15.TabIndex = 25;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(129, 35);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(110, 21);
            this.checkBox1.TabIndex = 85;
            this.checkBox1.Text = "Mostra Tutto";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.textBox1.Location = new System.Drawing.Point(70, 31);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(53, 24);
            this.textBox1.TabIndex = 84;
            this.textBox1.Text = "256";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label55.Location = new System.Drawing.Point(67, 11);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(35, 17);
            this.label55.TabIndex = 83;
            this.label55.Text = "Fine";
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.textBox2.Location = new System.Drawing.Point(11, 31);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(53, 24);
            this.textBox2.TabIndex = 82;
            this.textBox2.Text = "1";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label56.Location = new System.Drawing.Point(8, 11);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(40, 17);
            this.label56.TabIndex = 81;
            this.label56.Text = "Inizio";
            // 
            // btnStatoSchCarica
            // 
            this.btnStatoSchCarica.Location = new System.Drawing.Point(265, 14);
            this.btnStatoSchCarica.Name = "btnStatoSchCarica";
            this.btnStatoSchCarica.Size = new System.Drawing.Size(103, 41);
            this.btnStatoSchCarica.TabIndex = 80;
            this.btnStatoSchCarica.Text = "Carica";
            this.btnStatoSchCarica.UseVisualStyleBackColor = true;
            this.btnStatoSchCarica.Click += new System.EventHandler(this.btnStatoSchCarica_Click);
            // 
            // flvStatoListaSch
            // 
            this.flvStatoListaSch.CellEditUseWholeCell = false;
            this.flvStatoListaSch.Location = new System.Drawing.Point(471, 140);
            this.flvStatoListaSch.Name = "flvStatoListaSch";
            this.flvStatoListaSch.ShowGroups = false;
            this.flvStatoListaSch.Size = new System.Drawing.Size(382, 479);
            this.flvStatoListaSch.TabIndex = 24;
            this.flvStatoListaSch.UseCompatibleStateImageBehavior = false;
            this.flvStatoListaSch.View = System.Windows.Forms.View.Details;
            this.flvStatoListaSch.VirtualMode = true;
            // 
            // lblStatoImmagini
            // 
            this.lblStatoImmagini.AutoSize = true;
            this.lblStatoImmagini.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatoImmagini.Location = new System.Drawing.Point(25, 30);
            this.lblStatoImmagini.Name = "lblStatoImmagini";
            this.lblStatoImmagini.Size = new System.Drawing.Size(93, 20);
            this.lblStatoImmagini.TabIndex = 23;
            this.lblStatoImmagini.Text = "IMMAGINI";
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.White;
            this.panel11.Controls.Add(this.chkStatoImgMostraTutto);
            this.panel11.Controls.Add(this.txtStatoImgEnd);
            this.panel11.Controls.Add(this.label48);
            this.panel11.Controls.Add(this.txtStatoImgStart);
            this.panel11.Controls.Add(this.label47);
            this.panel11.Controls.Add(this.btnStatoImgCarica);
            this.panel11.Location = new System.Drawing.Point(29, 53);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(382, 69);
            this.panel11.TabIndex = 22;
            // 
            // chkStatoImgMostraTutto
            // 
            this.chkStatoImgMostraTutto.AutoSize = true;
            this.chkStatoImgMostraTutto.Location = new System.Drawing.Point(129, 35);
            this.chkStatoImgMostraTutto.Name = "chkStatoImgMostraTutto";
            this.chkStatoImgMostraTutto.Size = new System.Drawing.Size(110, 21);
            this.chkStatoImgMostraTutto.TabIndex = 85;
            this.chkStatoImgMostraTutto.Text = "Mostra Tutto";
            this.chkStatoImgMostraTutto.UseVisualStyleBackColor = true;
            // 
            // txtStatoImgEnd
            // 
            this.txtStatoImgEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtStatoImgEnd.Location = new System.Drawing.Point(70, 31);
            this.txtStatoImgEnd.Name = "txtStatoImgEnd";
            this.txtStatoImgEnd.Size = new System.Drawing.Size(53, 24);
            this.txtStatoImgEnd.TabIndex = 84;
            this.txtStatoImgEnd.Text = "100";
            this.txtStatoImgEnd.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label48.Location = new System.Drawing.Point(67, 11);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(35, 17);
            this.label48.TabIndex = 83;
            this.label48.Text = "Fine";
            // 
            // txtStatoImgStart
            // 
            this.txtStatoImgStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtStatoImgStart.Location = new System.Drawing.Point(11, 31);
            this.txtStatoImgStart.Name = "txtStatoImgStart";
            this.txtStatoImgStart.Size = new System.Drawing.Size(53, 24);
            this.txtStatoImgStart.TabIndex = 82;
            this.txtStatoImgStart.Text = "1";
            this.txtStatoImgStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label47.Location = new System.Drawing.Point(8, 11);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(40, 17);
            this.label47.TabIndex = 81;
            this.label47.Text = "Inizio";
            // 
            // btnStatoImgCarica
            // 
            this.btnStatoImgCarica.Location = new System.Drawing.Point(265, 14);
            this.btnStatoImgCarica.Name = "btnStatoImgCarica";
            this.btnStatoImgCarica.Size = new System.Drawing.Size(103, 41);
            this.btnStatoImgCarica.TabIndex = 80;
            this.btnStatoImgCarica.Text = "Carica";
            this.btnStatoImgCarica.UseVisualStyleBackColor = true;
            this.btnStatoImgCarica.Click += new System.EventHandler(this.btnStatoImgCarica_Click);
            // 
            // flvStatoListaImg
            // 
            this.flvStatoListaImg.CellEditUseWholeCell = false;
            this.flvStatoListaImg.Location = new System.Drawing.Point(29, 140);
            this.flvStatoListaImg.Name = "flvStatoListaImg";
            this.flvStatoListaImg.ShowGroups = false;
            this.flvStatoListaImg.Size = new System.Drawing.Size(382, 479);
            this.flvStatoListaImg.TabIndex = 21;
            this.flvStatoListaImg.UseCompatibleStateImageBehavior = false;
            this.flvStatoListaImg.View = System.Windows.Forms.View.Details;
            this.flvStatoListaImg.VirtualMode = true;
            // 
            // pgbStatoAvanzamento
            // 
            this.pgbStatoAvanzamento.Location = new System.Drawing.Point(29, 676);
            this.pgbStatoAvanzamento.Name = "pgbStatoAvanzamento";
            this.pgbStatoAvanzamento.Size = new System.Drawing.Size(824, 26);
            this.pgbStatoAvanzamento.TabIndex = 27;
            this.pgbStatoAvanzamento.Visible = false;
            // 
            // frmDisplayManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1657, 844);
            this.Controls.Add(this.tbcMainDisplayManager);
            this.Name = "frmDisplayManager";
            this.Text = "frmDisplayManager";
            this.Load += new System.EventHandler(this.frmDisplayManager_Load);
            this.tbcMainDisplayManager.ResumeLayout(false);
            this.tbpConnessione.ResumeLayout(false);
            this.pnlComandiImmediati.ResumeLayout(false);
            this.pnlComandiImmediati.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tbpArchivioModelli.ResumeLayout(false);
            this.grbModInvioModello.ResumeLayout(false);
            this.grbModInvioModello.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tbpSchermate.ResumeLayout(false);
            this.tbpSchermate.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel14.ResumeLayout(false);
            this.panel14.PerformLayout();
            this.panel13.ResumeLayout(false);
            this.panel13.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvSchListaComandi)).EndInit();
            this.panel5.ResumeLayout(false);
            this.pnlSchImmagineSchermata.ResumeLayout(false);
            this.pnlSchImmagineSchermata.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxSchImmagine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flvSchListaSchermate)).EndInit();
            this.tbpImmagini.ResumeLayout(false);
            this.tbpImmagini.PerformLayout();
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.flvImgListaImmagini)).EndInit();
            this.grbGeneraExcel.ResumeLayout(false);
            this.grbGeneraExcel.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxImgImmagine1b)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxImgImmagine8b)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxImgImmagine)).EndInit();
            this.tbpVariabili.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvVarListaVariabili)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tbpRealTime.ResumeLayout(false);
            this.tbpRealTime.PerformLayout();
            this.grbRtPulsanti.ResumeLayout(false);
            this.grbRtPulsanti.PerformLayout();
            this.tbpAccesssoMemoria.ResumeLayout(false);
            this.tbpAccesssoMemoria.PerformLayout();
            this.grbMemScrittura.ResumeLayout(false);
            this.grbMemScrittura.PerformLayout();
            this.grbMemCancFisica.ResumeLayout(false);
            this.grbMemCancFisica.PerformLayout();
            this.grbMemCancellazione.ResumeLayout(false);
            this.grbMemLettura.ResumeLayout(false);
            this.grbMemLettura.PerformLayout();
            this.tbpStatoScheda.ResumeLayout(false);
            this.tbpStatoScheda.PerformLayout();
            this.panel15.ResumeLayout(false);
            this.panel15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvStatoListaSch)).EndInit();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvStatoListaImg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcMainDisplayManager;
        private System.Windows.Forms.TabPage tbpConnessione;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tbpRealTime;
        private System.Windows.Forms.TabPage tbpImmagini;
        private System.Windows.Forms.TabPage tbpVariabili;
        private System.Windows.Forms.TabPage tbpSchermate;
        private System.Windows.Forms.Button btnPortState;
        private System.Windows.Forms.ComboBox cboHandShaking;
        private System.Windows.Forms.ComboBox cboParity;
        private System.Windows.Forms.ComboBox cboStopBits;
        private System.Windows.Forms.ComboBox cboDataBits;
        private System.Windows.Forms.ComboBox cboBaudRate;
        private System.Windows.Forms.ComboBox cboPorts;
        private System.Windows.Forms.Button btnGetSerialPorts;
        private System.Windows.Forms.RichTextBox rtbhex;
        private System.Windows.Forms.RichTextBox rtbOutgoing;
        private System.Windows.Forms.Label lblRIStatus;
        private System.Windows.Forms.Label lblDSRStatus;
        private System.Windows.Forms.Label lblCTSStatus;
        private System.Windows.Forms.Label lblBreakStatus;
        private System.Windows.Forms.RichTextBox rtbIncoming;
        private System.Windows.Forms.Panel pnlComandiImmediati;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnPrimaLettura;
        private System.Windows.Forms.Button cmdLeggiRTC;
        private System.Windows.Forms.Button btnApriComunicazione;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPorta;
        private System.Windows.Forms.CheckBox chkRtBacklight;
        private System.Windows.Forms.Button btnRtSetLed;
        private System.Windows.Forms.TextBox txtRtValTimeOff;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtRtValTimeOn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRtValBlu;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtRtValGreen;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRtValRed;
        private System.Windows.Forms.Label label182;
        private System.Windows.Forms.Button btnRtStopLed;
        private System.Windows.Forms.Button btnRtDrawLine;
        private System.Windows.Forms.TextBox txtRtValLineColor;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtRtValLineYFine;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtRtValLineXFine;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtRtValLineYStart;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtRtValLineXStart;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pbxImgImmagine;
        private System.Windows.Forms.TabPage tbpAccesssoMemoria;
        private System.Windows.Forms.GroupBox grbMemLettura;
        private System.Windows.Forms.CheckBox chkMemHex;
        private System.Windows.Forms.Label lblReadMemBytes;
        private System.Windows.Forms.Label lblReadMemStartAddr;
        private System.Windows.Forms.TextBox txtMemLenR;
        private System.Windows.Forms.TextBox txtMemAddrR;
        private System.Windows.Forms.Button cmdMemRead;
        private System.Windows.Forms.TabPage tbpArchivioModelli;
        private System.Windows.Forms.SaveFileDialog sfdExportDati;
        private System.Windows.Forms.OpenFileDialog ofdImportDati;
        private System.Windows.Forms.GroupBox grbGeneraExcel;
        private System.Windows.Forms.Button btnSfoglia;
        private System.Windows.Forms.TextBox txtNuovoFile;
        private System.Windows.Forms.Button btnImgCaricaFileImmagine;
        private System.Windows.Forms.Button btnImgSimulaFileImmagine;
        private System.Windows.Forms.PictureBox pbxImgImmagine1b;
        private System.Windows.Forms.PictureBox pbxImgImmagine8b;
        private System.Windows.Forms.Button btnImgGeneraArrayImmagine;
        private System.Windows.Forms.Button btnImgGeneraClasse;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtImgDimY;
        private System.Windows.Forms.TextBox txtImgDimX;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtImgDimImmagine;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtImgNomeImmagine;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox grbMemCancellazione;
        private System.Windows.Forms.Button cmdMemClear;
        private System.Windows.Forms.GroupBox grbMemCancFisica;
        private System.Windows.Forms.RadioButton rbtMemAreaApp2;
        private System.Windows.Forms.RadioButton rbtMemAreaApp1;
        private System.Windows.Forms.RadioButton rbtMemAreaLibera;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.TextBox txtMemCFBlocchi;
        private System.Windows.Forms.CheckBox chkMemCFStartAddHex;
        private System.Windows.Forms.Label label112;
        private System.Windows.Forms.TextBox txtMemCFStartAdd;
        private System.Windows.Forms.Button btnMemCFExec;
        private System.Windows.Forms.GroupBox grbMemScrittura;
        private System.Windows.Forms.CheckBox chkMemHexW;
        private System.Windows.Forms.Label lblMemVerificaValore;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtMemDataW;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox txtMemLenW;
        private System.Windows.Forms.TextBox txtMemAddrW;
        private System.Windows.Forms.Button cmdMemWrite;
        private System.Windows.Forms.Button btnRtDrawImage;
        private System.Windows.Forms.TextBox txtRtValImgColor;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtRtValImgPosY;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtRtValImgPosX;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtRtValImgId;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button btnRtCLS;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtImgBaseDimY;
        private System.Windows.Forms.TextBox txtImgBaseDimX;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtImgBaseSize;
        private System.Windows.Forms.Label label30;
        private BrightIdeasSoftware.FastObjectListView flvImgListaImmagini;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel pnlSchImmagineSchermata;
        private BrightIdeasSoftware.FastObjectListView flvSchListaSchermate;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button btnModCercaCaricaModello;
        private System.Windows.Forms.Button btnModCercaSalvaModello;
        private System.Windows.Forms.Button btnModCaricaModello;
        private System.Windows.Forms.Button btnModSalvaModello;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtModNomeFile;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnModNuovoModello;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.TextBox txtModNumVar;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox txtModNumDis;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox txtModNumImg;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox txtModDataMod;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox txtModDataCre;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox txtModNote;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox txtModVersione;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox txtModNome;
        private System.Windows.Forms.TextBox txtMemDataGrid;
        private System.Windows.Forms.TextBox txtImgIdImmagine;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.TextBox txtImgNomeImmagineLista;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button btnImgRimuoviImmagine;
        private System.Windows.Forms.Button btnImgInviaImmagine;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.TextBox txtVarValore;
        private System.Windows.Forms.Button btnVarCancellaVariabile;
        private System.Windows.Forms.Button btnVarInviaValore;
        private BrightIdeasSoftware.FastObjectListView flvVarListaVariabili;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox txtVarIdVariabile;
        private System.Windows.Forms.Button btnVarCrea;
        private System.Windows.Forms.TextBox txtVarNomeVariabile;
        private System.Windows.Forms.TextBox txtRtValBluDx;
        private System.Windows.Forms.TextBox txtRtValGreenDx;
        private System.Windows.Forms.TextBox txtRtValRedDx;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox txtRtValTimeOffDx;
        private System.Windows.Forms.TextBox txtRtValTimeOnDx;
        private System.Windows.Forms.TabPage tbpStatoScheda;
        private System.Windows.Forms.Label lblStatoImmagini;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.CheckBox chkStatoImgMostraTutto;
        private System.Windows.Forms.TextBox txtStatoImgEnd;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox txtStatoImgStart;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Button btnStatoImgCarica;
        private BrightIdeasSoftware.FastObjectListView flvStatoListaImg;
        private System.Windows.Forms.Button btnImgMostraImmagine;
        private System.Windows.Forms.Button btnRtDrawSchermata;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.TextBox txtRtValSchId;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Panel panel14;
        private System.Windows.Forms.Panel panel13;
        private BrightIdeasSoftware.FastObjectListView flvSchListaComandi;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Panel panel15;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Button btnStatoSchCarica;
        private BrightIdeasSoftware.FastObjectListView flvStatoListaSch;
        private System.Windows.Forms.Button btnSchCaricaFile;
        private System.Windows.Forms.Button btnSchCercaFile;
        private System.Windows.Forms.TextBox txtSchNuovoFile;
        private System.Windows.Forms.PictureBox pbxSchImmagine;
        private System.Windows.Forms.TextBox txtSchBaseNomeLista;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.TextBox txtSchBaseID;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Button cmdSchGeneraClasse;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.TextBox txtSchBaseHeigh;
        private System.Windows.Forms.TextBox txtSchBaseWidth;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.TextBox txtSchBaseSize;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.TextBox txtSchBaseName;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Button cmdSchMostrtaSch;
        private System.Windows.Forms.Button cmdSchRimuoviElemento;
        private System.Windows.Forms.Button cmdSchInviaSch;
        private System.Windows.Forms.Button cmdSchGeneraByteArray;
        private System.Windows.Forms.ComboBox cmbSchTipoComando;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnSchCmdLoad;
        private System.Windows.Forms.Button btnSchCmdDel;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.TextBox txtSchCmdTempoOFF;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.TextBox txtSchCmdTempoON;
        private System.Windows.Forms.Button btnSchCmdAdd;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.TextBox txtSchCmdNum;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.TextBox txtSchCmdText;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.TextBox txtSchCmdLenVarChar;
        private System.Windows.Forms.TextBox txtSchCmdNumImg;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.TextBox txtSchCmdLenVarPix;
        private System.Windows.Forms.TextBox txtSchCmdNumVar;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.TextBox txtSchCmdColor;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.TextBox txtSchCmdPosY;
        private System.Windows.Forms.TextBox txtSchCmdPosX;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.TextBox txtSchCmdHeigh;
        private System.Windows.Forms.TextBox txtSchCmdWidth;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button cmdSchNuovaSchermata;
        private System.Windows.Forms.Button btnSchCmdNew;
        private System.Windows.Forms.Button btnRtSetRTC;
        private System.Windows.Forms.Button btnRtSetBaudRate;
        private System.Windows.Forms.ComboBox cmbRtBaudRate;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.TextBox txtRtIdVariabile;
        private System.Windows.Forms.TextBox txtRtValVariabile;
        private System.Windows.Forms.Button btnRtImpostaVariabile;
        private System.Windows.Forms.ComboBox cmbRtValVariabile;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.ComboBox cmbSchIdVariabile;
        private System.Windows.Forms.GroupBox grbModInvioModello;
        private System.Windows.Forms.ProgressBar pgbModStatoInvio;
        private System.Windows.Forms.Button btnModAggiornaDisplay;
        private System.Windows.Forms.TextBox txtModSchermateTrasmesse;
        private System.Windows.Forms.TextBox txtModImmaginiTrasmesse;
        private System.Windows.Forms.Button btnModInviaSchermate;
        private System.Windows.Forms.Button btnModInviaImmagini;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.TextBox txtRtSeqSchTime;
        private System.Windows.Forms.TextBox txtRtSeqSchId;
        private System.Windows.Forms.Button btnRtDrawSchSequence;
        private System.Windows.Forms.Button btnVarSalvaValore;
        private System.Windows.Forms.TextBox txtModVariabiliTrasmesse;
        private System.Windows.Forms.Button btnModInviaVariabili;
        private System.Windows.Forms.Button btnRtTestLed;
        private System.Windows.Forms.Button btnRtResetBoard;
        private System.Windows.Forms.GroupBox grbRtPulsanti;
        private System.Windows.Forms.Button btnRtLeggiPulsanti;
        private System.Windows.Forms.TextBox txtRtValBtn01;
        private System.Windows.Forms.TextBox txtRtValBtn02;
        private System.Windows.Forms.TextBox txtRtValBtn03;
        private System.Windows.Forms.TextBox txtRtValBtn04;
        private System.Windows.Forms.TextBox txtRtValBtn05;
        private System.Windows.Forms.CheckBox chkRtRiavvioAutomatico;
        private System.Windows.Forms.ProgressBar pgbStatoAvanzamento;
    }
}