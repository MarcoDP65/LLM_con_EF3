namespace PannelloCharger
{
    partial class frmSuperCharger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSuperCharger));
            this.tmrLetturaAutomatica = new System.Windows.Forms.Timer(this.components);
            this.sfdExportDati = new System.Windows.Forms.SaveFileDialog();
            this.sfdImportDati = new System.Windows.Forms.OpenFileDialog();
            this.tabMonitor = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtVarGeneraExcel = new System.Windows.Forms.Button();
            this.btnVarFilesearch = new System.Windows.Forms.Button();
            this.txtVarFileCicli = new System.Windows.Forms.TextBox();
            this.chkParRegistraLetture = new System.Windows.Forms.CheckBox();
            this.flvwLettureParametri = new BrightIdeasSoftware.FastObjectListView();
            this.txtParIntervalloLettura = new System.Windows.Forms.TextBox();
            this.label68 = new System.Windows.Forms.Label();
            this.chkParLetturaAuto = new System.Windows.Forms.CheckBox();
            this.chkDatiDiretti = new System.Windows.Forms.CheckBox();
            this.btnLeggiVariabili = new System.Windows.Forms.Button();
            this.grbVariabiliImmediate = new System.Windows.Forms.GroupBox();
            this.label67 = new System.Windows.Forms.Label();
            this.txtVarTempoTrascorso = new System.Windows.Forms.TextBox();
            this.txtVarMemProgrammed = new System.Windows.Forms.TextBox();
            this.label66 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.txtVarIbatt = new System.Windows.Forms.TextBox();
            this.lblVarVBatt = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.txtVarAhCarica = new System.Windows.Forms.TextBox();
            this.txtVarVBatt = new System.Windows.Forms.TextBox();
            this.tabCb02 = new System.Windows.Forms.TabPage();
            this.grbCavi = new System.Windows.Forms.GroupBox();
            this.txtEsitoVerCavi = new System.Windows.Forms.TextBox();
            this.lblEsito = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPotenzaDispersa = new System.Windows.Forms.TextBox();
            this.lblPotenzaDispersa = new System.Windows.Forms.Label();
            this.btnVerificaCavi = new System.Windows.Forms.Button();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.lblCaviSelVerifica = new System.Windows.Forms.Label();
            this.cmbSezioneProlunga = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblProlunga = new System.Windows.Forms.Label();
            this.cmbSezioneCavo = new System.Windows.Forms.ComboBox();
            this.lblSezioneCavo = new System.Windows.Forms.Label();
            this.txtLunghezzaCavo = new System.Windows.Forms.TextBox();
            this.lblLunghezzaCavo = new System.Windows.Forms.Label();
            this.lblCavoCaricabatterie = new System.Windows.Forms.Label();
            this.tbpProxySig60 = new System.Windows.Forms.TabPage();
            this.label176 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkStratLLRabb = new System.Windows.Forms.CheckBox();
            this.txtStratLLAmax = new System.Windows.Forms.TextBox();
            this.label155 = new System.Windows.Forms.Label();
            this.txtStratLLVmax = new System.Windows.Forms.TextBox();
            this.label154 = new System.Windows.Forms.Label();
            this.txtStratLLVmin = new System.Windows.Forms.TextBox();
            this.label153 = new System.Windows.Forms.Label();
            this.label200 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.grbStratStepCorrente = new System.Windows.Forms.GroupBox();
            this.label206 = new System.Windows.Forms.Label();
            this.txtStratCurrStepTipo = new System.Windows.Forms.TextBox();
            this.label205 = new System.Windows.Forms.Label();
            this.txtStratCurrStepRipetizioni = new System.Windows.Forms.TextBox();
            this.label203 = new System.Windows.Forms.Label();
            this.txtStratCurrStepTon = new System.Windows.Forms.TextBox();
            this.txtStratCurrStepToff = new System.Windows.Forms.TextBox();
            this.label204 = new System.Windows.Forms.Label();
            this.label194 = new System.Windows.Forms.Label();
            this.txtStratCurrStepAh = new System.Windows.Forms.TextBox();
            this.label191 = new System.Windows.Forms.Label();
            this.txtStratCurrStepVmax = new System.Windows.Forms.TextBox();
            this.txtStratCurrStepVmin = new System.Windows.Forms.TextBox();
            this.label192 = new System.Windows.Forms.Label();
            this.label190 = new System.Windows.Forms.Label();
            this.txtStratCurrStepImax = new System.Windows.Forms.TextBox();
            this.txtStratCurrStepImin = new System.Windows.Forms.TextBox();
            this.label195 = new System.Windows.Forms.Label();
            this.label202 = new System.Windows.Forms.Label();
            this.cmbStratIsSelStep = new System.Windows.Forms.ComboBox();
            this.label199 = new System.Windows.Forms.Label();
            this.txtStratIsNumSpire = new System.Windows.Forms.TextBox();
            this.label198 = new System.Windows.Forms.Label();
            this.txtStratIsStep = new System.Windows.Forms.TextBox();
            this.txtStratIsEsito = new System.Windows.Forms.TextBox();
            this.label197 = new System.Windows.Forms.Label();
            this.txtStratIsMinuti = new System.Windows.Forms.TextBox();
            this.txtStratIsAhRich = new System.Windows.Forms.TextBox();
            this.label193 = new System.Windows.Forms.Label();
            this.label196 = new System.Windows.Forms.Label();
            this.label189 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtStratAVStop = new System.Windows.Forms.TextBox();
            this.label201 = new System.Windows.Forms.Label();
            this.label179 = new System.Windows.Forms.Label();
            this.label177 = new System.Windows.Forms.Label();
            this.txtStratAVTempIst = new System.Windows.Forms.TextBox();
            this.txtStratAVCorrenteIst = new System.Windows.Forms.TextBox();
            this.txtStratAVPrevisti = new System.Windows.Forms.TextBox();
            this.label180 = new System.Windows.Forms.Label();
            this.txtStratAVMancanti = new System.Windows.Forms.TextBox();
            this.label181 = new System.Windows.Forms.Label();
            this.txtStratAVTensioneIst = new System.Windows.Forms.TextBox();
            this.label182 = new System.Windows.Forms.Label();
            this.txtStratAVMinutiResidui = new System.Windows.Forms.TextBox();
            this.label187 = new System.Windows.Forms.Label();
            this.txtStratAVErogati = new System.Windows.Forms.TextBox();
            this.label188 = new System.Windows.Forms.Label();
            this.label185 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtStratLivcrgCapacityVer = new System.Windows.Forms.TextBox();
            this.txtStratLivcrgDschgVer = new System.Windows.Forms.TextBox();
            this.txtStratLivcrgChgVer = new System.Windows.Forms.TextBox();
            this.txtStratLivcrgSetCapacity = new System.Windows.Forms.TextBox();
            this.label178 = new System.Windows.Forms.Label();
            this.txtStratLivcrgSetDschg = new System.Windows.Forms.TextBox();
            this.label183 = new System.Windows.Forms.Label();
            this.txtStratLivcrgSetChg = new System.Windows.Forms.TextBox();
            this.label184 = new System.Windows.Forms.Label();
            this.label166 = new System.Windows.Forms.Label();
            this.pnlStratContatoriCarica = new System.Windows.Forms.Panel();
            this.txtStratLivcrgCapNominale = new System.Windows.Forms.TextBox();
            this.label165 = new System.Windows.Forms.Label();
            this.txtStratLivcrgCapResidua = new System.Windows.Forms.TextBox();
            this.label164 = new System.Windows.Forms.Label();
            this.txtStratLivcrgDiscrgTot = new System.Windows.Forms.TextBox();
            this.label162 = new System.Windows.Forms.Label();
            this.txtStratLivcrgCrgTot = new System.Windows.Forms.TextBox();
            this.label163 = new System.Windows.Forms.Label();
            this.txtStratLivcrgDiscrg = new System.Windows.Forms.TextBox();
            this.label160 = new System.Windows.Forms.Label();
            this.txtStratLivcrgCrg = new System.Windows.Forms.TextBox();
            this.label161 = new System.Windows.Forms.Label();
            this.txtStratLivcrgNeg = new System.Windows.Forms.TextBox();
            this.label159 = new System.Windows.Forms.Label();
            this.txtStratLivcrgPos = new System.Windows.Forms.TextBox();
            this.label158 = new System.Windows.Forms.Label();
            this.label167 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtStratQryTrepr = new System.Windows.Forms.TextBox();
            this.label175 = new System.Windows.Forms.Label();
            this.txtStratQryTalm = new System.Windows.Forms.TextBox();
            this.label174 = new System.Windows.Forms.Label();
            this.txtStratQryTatt = new System.Windows.Forms.TextBox();
            this.label173 = new System.Windows.Forms.Label();
            this.txtStratQryTensGas = new System.Windows.Forms.TextBox();
            this.label172 = new System.Windows.Forms.Label();
            this.txtStratQryCapN = new System.Windows.Forms.TextBox();
            this.label171 = new System.Windows.Forms.Label();
            this.txtStratQryTensN = new System.Windows.Forms.TextBox();
            this.label170 = new System.Windows.Forms.Label();
            this.txtStratQryActSeup = new System.Windows.Forms.TextBox();
            this.label169 = new System.Windows.Forms.Label();
            this.txtStratQryVerLib = new System.Windows.Forms.TextBox();
            this.label168 = new System.Windows.Forms.Label();
            this.lblSig60DataReceuved = new System.Windows.Forms.Label();
            this.lblSig60DataSent = new System.Windows.Forms.Label();
            this.txtStratDataGridRx = new System.Windows.Forms.TextBox();
            this.txtStratDataGridTx = new System.Windows.Forms.TextBox();
            this.grbStratComandiTest = new System.Windows.Forms.GroupBox();
            this.btnStratCallSIS = new System.Windows.Forms.Button();
            this.btnStratCallAv = new System.Windows.Forms.Button();
            this.btnStratSetDischarge = new System.Windows.Forms.Button();
            this.btnStratCallIS = new System.Windows.Forms.Button();
            this.btnStratSetCharge = new System.Windows.Forms.Button();
            this.btnStratQuery = new System.Windows.Forms.Button();
            this.btnStratTestERR = new System.Windows.Forms.Button();
            this.btnStratTest02 = new System.Windows.Forms.Button();
            this.btnStratTest01 = new System.Windows.Forms.Button();
            this.tbpFirmware = new System.Windows.Forms.TabPage();
            this.grbFwAttivazioneArea = new System.Windows.Forms.GroupBox();
            this.btnFwSwitchApp = new System.Windows.Forms.Button();
            this.btnFwSwitchArea2 = new System.Windows.Forms.Button();
            this.btnFwSwitchArea1 = new System.Windows.Forms.Button();
            this.btnFwSwitchBL = new System.Windows.Forms.Button();
            this.grbFWPreparaFile = new System.Windows.Forms.GroupBox();
            this.txtFWInFileStruct = new System.Windows.Forms.TextBox();
            this.lvwFWInFileStruct = new BrightIdeasSoftware.FastObjectListView();
            this.txtFwFileCCSa01 = new System.Windows.Forms.TextBox();
            this.txtFwFileCCShex = new System.Windows.Forms.TextBox();
            this.txtFWLibInFileRev = new System.Windows.Forms.TextBox();
            this.label256 = new System.Windows.Forms.Label();
            this.txtFWInFileRevData = new System.Windows.Forms.MaskedTextBox();
            this.label96 = new System.Windows.Forms.Label();
            this.btnFWFileLLFsearch = new System.Windows.Forms.Button();
            this.txtFWFileLLFwr = new System.Windows.Forms.TextBox();
            this.label92 = new System.Windows.Forms.Label();
            this.btnFWFilePubSave = new System.Windows.Forms.Button();
            this.txtFWInFileRev = new System.Windows.Forms.TextBox();
            this.label94 = new System.Windows.Forms.Label();
            this.label95 = new System.Windows.Forms.Label();
            this.btnFWFileCCSLoad = new System.Windows.Forms.Button();
            this.btnFWFileCCSsearch = new System.Windows.Forms.Button();
            this.txtFwFileCCS = new System.Windows.Forms.TextBox();
            this.grbFWArea2 = new System.Windows.Forms.GroupBox();
            this.label86 = new System.Windows.Forms.Label();
            this.txtFwRevA2Size = new System.Windows.Forms.TextBox();
            this.txtFWRevA2Addr5 = new System.Windows.Forms.TextBox();
            this.label80 = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.txtFWRevA2Addr4 = new System.Windows.Forms.TextBox();
            this.txtFWRevA2Addr3 = new System.Windows.Forms.TextBox();
            this.label83 = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.txtFWRevA2Addr2 = new System.Windows.Forms.TextBox();
            this.txtFWRevA2Addr1 = new System.Windows.Forms.TextBox();
            this.label85 = new System.Windows.Forms.Label();
            this.label109 = new System.Windows.Forms.Label();
            this.txtFwRevA2RilFw = new System.Windows.Forms.TextBox();
            this.label87 = new System.Windows.Forms.Label();
            this.txtFwRevA2MsgSize = new System.Windows.Forms.TextBox();
            this.label89 = new System.Windows.Forms.Label();
            this.txtFwRevA2State = new System.Windows.Forms.TextBox();
            this.label90 = new System.Windows.Forms.Label();
            this.txtFwRevA2RevFw = new System.Windows.Forms.TextBox();
            this.GrbFWArea1 = new System.Windows.Forms.GroupBox();
            this.label97 = new System.Windows.Forms.Label();
            this.txtFwRevA1Size = new System.Windows.Forms.TextBox();
            this.label103 = new System.Windows.Forms.Label();
            this.txtFwRevA1RilFw = new System.Windows.Forms.TextBox();
            this.txtFWRevA1Addr5 = new System.Windows.Forms.TextBox();
            this.label81 = new System.Windows.Forms.Label();
            this.label74 = new System.Windows.Forms.Label();
            this.txtFWRevA1Addr4 = new System.Windows.Forms.TextBox();
            this.txtFWRevA1Addr3 = new System.Windows.Forms.TextBox();
            this.label75 = new System.Windows.Forms.Label();
            this.label79 = new System.Windows.Forms.Label();
            this.label78 = new System.Windows.Forms.Label();
            this.txtFWRevA1Addr2 = new System.Windows.Forms.TextBox();
            this.txtFWRevA1Addr1 = new System.Windows.Forms.TextBox();
            this.label77 = new System.Windows.Forms.Label();
            this.txtFwRevA1MsgSize = new System.Windows.Forms.TextBox();
            this.label91 = new System.Windows.Forms.Label();
            this.txtFwRevA1State = new System.Windows.Forms.TextBox();
            this.label93 = new System.Windows.Forms.Label();
            this.txtFwRevA1RevFw = new System.Windows.Forms.TextBox();
            this.grbFWAggiornamento = new System.Windows.Forms.GroupBox();
            this.cmbFWSBFArea = new System.Windows.Forms.ComboBox();
            this.flwFWFileLLFStruct = new BrightIdeasSoftware.FastObjectListView();
            this.txtFWInLLFDispRev = new System.Windows.Forms.TextBox();
            this.txtFWInSBFDtRev = new System.Windows.Forms.TextBox();
            this.label110 = new System.Windows.Forms.Label();
            this.btnFWLanciaTrasmissione = new System.Windows.Forms.Button();
            this.txtFWInLLFEsito = new System.Windows.Forms.TextBox();
            this.label108 = new System.Windows.Forms.Label();
            this.label114 = new System.Windows.Forms.Label();
            this.btnFWPreparaTrasmissione = new System.Windows.Forms.Button();
            this.txtFWInLLFRev = new System.Windows.Forms.TextBox();
            this.label115 = new System.Windows.Forms.Label();
            this.label116 = new System.Windows.Forms.Label();
            this.btnFWFileLLFLoad = new System.Windows.Forms.Button();
            this.btnFWFileLLFReadSearch = new System.Windows.Forms.Button();
            this.txtFWFileSBFrd = new System.Windows.Forms.TextBox();
            this.grbStatoFirmware = new System.Windows.Forms.GroupBox();
            this.label186 = new System.Windows.Forms.Label();
            this.txtFwRevDisplay = new System.Windows.Forms.TextBox();
            this.grbFWDettStato = new System.Windows.Forms.GroupBox();
            this.txtFwStatoSA2 = new System.Windows.Forms.TextBox();
            this.label117 = new System.Windows.Forms.Label();
            this.txtFwStatoSA1 = new System.Windows.Forms.TextBox();
            this.label118 = new System.Windows.Forms.Label();
            this.btnFwCaricaStato = new System.Windows.Forms.Button();
            this.txtFwStatoHA2 = new System.Windows.Forms.TextBox();
            this.label119 = new System.Windows.Forms.Label();
            this.txtFwStatoHA1 = new System.Windows.Forms.TextBox();
            this.label120 = new System.Windows.Forms.Label();
            this.txtFwStatoMicro = new System.Windows.Forms.TextBox();
            this.label121 = new System.Windows.Forms.Label();
            this.txtFwAreaTestata = new System.Windows.Forms.TextBox();
            this.label122 = new System.Windows.Forms.Label();
            this.txtFwRevFirmware = new System.Windows.Forms.TextBox();
            this.label123 = new System.Windows.Forms.Label();
            this.txtFwRevBootloader = new System.Windows.Forms.TextBox();
            this.label124 = new System.Windows.Forms.Label();
            this.tabMemRead = new System.Windows.Forms.TabPage();
            this.grbMemTestLetture = new System.Windows.Forms.GroupBox();
            this.chkMemTestAddrRND = new System.Windows.Forms.CheckBox();
            this.chkMemTestLenRND = new System.Windows.Forms.CheckBox();
            this.label51 = new System.Windows.Forms.Label();
            this.txtMemNumTestERR = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.txtMemNumTestOK = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.txtMemNumTest = new System.Windows.Forms.TextBox();
            this.btnMemTestExac = new System.Windows.Forms.Button();
            this.grbMemAzzeraLogger = new System.Windows.Forms.GroupBox();
            this.chkMemCReboot = new System.Windows.Forms.CheckBox();
            this.btnMemClearLogExec = new System.Windows.Forms.Button();
            this.chkMemCResetCicli = new System.Windows.Forms.CheckBox();
            this.chkMemCResetCont = new System.Windows.Forms.CheckBox();
            this.chkMemCResetProg = new System.Windows.Forms.CheckBox();
            this.label41 = new System.Windows.Forms.Label();
            this.grbMemCaricaLogger = new System.Windows.Forms.GroupBox();
            this.btnMemRewriteExec = new System.Windows.Forms.Button();
            this.chkMemDevWCycle = new System.Windows.Forms.CheckBox();
            this.chkMemDevWCount = new System.Windows.Forms.CheckBox();
            this.chkMemDevWProg = new System.Windows.Forms.CheckBox();
            this.label40 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.txtMemFileRead = new System.Windows.Forms.TextBox();
            this.grbMemSalvaLogger = new System.Windows.Forms.GroupBox();
            this.btnMemSaveExec = new System.Windows.Forms.Button();
            this.chkMemFsCycle = new System.Windows.Forms.CheckBox();
            this.chkMemFsCount = new System.Windows.Forms.CheckBox();
            this.chkMemFsProgr = new System.Windows.Forms.CheckBox();
            this.label37 = new System.Windows.Forms.Label();
            this.btnMemFileSaveSRC = new System.Windows.Forms.Button();
            this.txtMemFileSave = new System.Windows.Forms.TextBox();
            this.grbMemCancFisica = new System.Windows.Forms.GroupBox();
            this.rbtMemLunghi = new System.Windows.Forms.RadioButton();
            this.rbtMemBrevi = new System.Windows.Forms.RadioButton();
            this.rbtMemProgrammazioni = new System.Windows.Forms.RadioButton();
            this.rbtMemContatori = new System.Windows.Forms.RadioButton();
            this.btnMemResetBoard = new System.Windows.Forms.Button();
            this.rbtMemDatiCliente = new System.Windows.Forms.RadioButton();
            this.rbtMemParametriInit = new System.Windows.Forms.RadioButton();
            this.rbtMemAreaApp2 = new System.Windows.Forms.RadioButton();
            this.rbtMemAreaApp1 = new System.Windows.Forms.RadioButton();
            this.rbtMemAreaLibera = new System.Windows.Forms.RadioButton();
            this.label111 = new System.Windows.Forms.Label();
            this.txtMemCFBlocchi = new System.Windows.Forms.TextBox();
            this.chkMemCFStartAddHex = new System.Windows.Forms.CheckBox();
            this.label112 = new System.Windows.Forms.Label();
            this.txtMemCFStartAdd = new System.Windows.Forms.TextBox();
            this.btnMemCFExec = new System.Windows.Forms.Button();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.btnDumpMemoria = new System.Windows.Forms.Button();
            this.txtMemDataGrid = new System.Windows.Forms.TextBox();
            this.grbMemScrittura = new System.Windows.Forms.GroupBox();
            this.chkMemHexW = new System.Windows.Forms.CheckBox();
            this.lblMemVerificaValore = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.txtMemDataW = new System.Windows.Forms.TextBox();
            this.label71 = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.txtMemLenW = new System.Windows.Forms.TextBox();
            this.txtMemAddrW = new System.Windows.Forms.TextBox();
            this.cmdMemWrite = new System.Windows.Forms.Button();
            this.grbMemCancellazione = new System.Windows.Forms.GroupBox();
            this.chkMemClearMantieniCliente = new System.Windows.Forms.CheckBox();
            this.cmdMemClear = new System.Windows.Forms.Button();
            this.grbMemLettura = new System.Windows.Forms.GroupBox();
            this.chkMemInt = new System.Windows.Forms.CheckBox();
            this.chkMemHex = new System.Windows.Forms.CheckBox();
            this.lblReadMemBytes = new System.Windows.Forms.Label();
            this.lblReadMemStartAddr = new System.Windows.Forms.Label();
            this.txtMemLenR = new System.Windows.Forms.TextBox();
            this.txtMemAddrR = new System.Windows.Forms.TextBox();
            this.cmdMemRead = new System.Windows.Forms.Button();
            this.tabInizializzazione = new System.Windows.Forms.TabPage();
            this.grbConnessioni = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.lblGenBRState = new System.Windows.Forms.Label();
            this.btnGenCambiaBaudRate = new System.Windows.Forms.Button();
            this.grbGenBaudrate = new System.Windows.Forms.GroupBox();
            this.optGenBR3M = new System.Windows.Forms.RadioButton();
            this.optGenBR1M = new System.Windows.Forms.RadioButton();
            this.optGenBR115 = new System.Windows.Forms.RadioButton();
            this.btnCaricaMemoria = new System.Windows.Forms.Button();
            this.grbInitLimiti = new System.Windows.Forms.GroupBox();
            this.txtInitModelloMemoria = new System.Windows.Forms.TextBox();
            this.label150 = new System.Windows.Forms.Label();
            this.txtInitMaxProg = new System.Windows.Forms.TextBox();
            this.label149 = new System.Windows.Forms.Label();
            this.txtInitMaxLunghi = new System.Windows.Forms.TextBox();
            this.label148 = new System.Windows.Forms.Label();
            this.txtInitMaxBrevi = new System.Windows.Forms.TextBox();
            this.label146 = new System.Windows.Forms.Label();
            this.btnScriviInizializzazione = new System.Windows.Forms.Button();
            this.btnCaricaInizializzazione = new System.Windows.Forms.Button();
            this.groupBox11 = new System.Windows.Forms.GroupBox();
            this.txtInitBrdVMaxModulo = new System.Windows.Forms.TextBox();
            this.label57 = new System.Windows.Forms.Label();
            this.txtInitBrdVMinModulo = new System.Windows.Forms.TextBox();
            this.label56 = new System.Windows.Forms.Label();
            this.txtInitBrdOpzioniModulo = new System.Windows.Forms.TextBox();
            this.label55 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.chkInitBrdSpareModulo = new System.Windows.Forms.CheckBox();
            this.label53 = new System.Windows.Forms.Label();
            this.txtInitBrdANomModulo = new System.Windows.Forms.NumericUpDown();
            this.label52 = new System.Windows.Forms.Label();
            this.txtInitBrdVNomModulo = new System.Windows.Forms.NumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.txtInitBrdNumModuli = new System.Windows.Forms.NumericUpDown();
            this.txtInitRevHwDISP = new System.Windows.Forms.TextBox();
            this.label131 = new System.Windows.Forms.Label();
            this.txtInitRevFwDISP = new System.Windows.Forms.TextBox();
            this.label127 = new System.Windows.Forms.Label();
            this.txtInitNumSerDISP = new System.Windows.Forms.TextBox();
            this.label128 = new System.Windows.Forms.Label();
            this.grbInitCalibrazione = new System.Windows.Forms.GroupBox();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label101 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label100 = new System.Windows.Forms.Label();
            this.textBox36 = new System.Windows.Forms.TextBox();
            this.label106 = new System.Windows.Forms.Label();
            this.grbInitDatiBase = new System.Windows.Forms.GroupBox();
            this.txtInitAMax = new System.Windows.Forms.NumericUpDown();
            this.label58 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkInitPresenzaRabb = new System.Windows.Forms.CheckBox();
            this.txtInitVMax = new System.Windows.Forms.TextBox();
            this.txtInitVMin = new System.Windows.Forms.TextBox();
            this.txtInitIDApparato = new System.Windows.Forms.TextBox();
            this.label144 = new System.Windows.Forms.Label();
            this.txtInitDataInizializ = new System.Windows.Forms.MaskedTextBox();
            this.label132 = new System.Windows.Forms.Label();
            this.cmbInitTipoApparato = new System.Windows.Forms.ComboBox();
            this.label99 = new System.Windows.Forms.Label();
            this.txtInitSerialeApparato = new System.Windows.Forms.TextBox();
            this.label98 = new System.Windows.Forms.Label();
            this.txtInitNumeroMatricola = new System.Windows.Forms.TextBox();
            this.label88 = new System.Windows.Forms.Label();
            this.txtInitAnnoMatricola = new System.Windows.Forms.TextBox();
            this.Anno = new System.Windows.Forms.Label();
            this.txtInitProductId = new System.Windows.Forms.TextBox();
            this.lblInitProductId = new System.Windows.Forms.Label();
            this.txtInitManufactured = new System.Windows.Forms.TextBox();
            this.lblInitManufactured = new System.Windows.Forms.Label();
            this.tabOrologio = new System.Windows.Forms.TabPage();
            this.grbCalData = new System.Windows.Forms.GroupBox();
            this.txtCalMinuti = new System.Windows.Forms.TextBox();
            this.label266 = new System.Windows.Forms.Label();
            this.txtCalOre = new System.Windows.Forms.TextBox();
            this.label267 = new System.Windows.Forms.Label();
            this.btnCalScriviGiorno = new System.Windows.Forms.Button();
            this.txtCalAnno = new System.Windows.Forms.TextBox();
            this.label250 = new System.Windows.Forms.Label();
            this.txtCalMese = new System.Windows.Forms.TextBox();
            this.label249 = new System.Windows.Forms.Label();
            this.txtCalGiorno = new System.Windows.Forms.TextBox();
            this.label248 = new System.Windows.Forms.Label();
            this.grbAccensione = new System.Windows.Forms.GroupBox();
            this.lblOrarioAccensione = new System.Windows.Forms.Label();
            this.cmbMinAccensione = new System.Windows.Forms.ComboBox();
            this.cmbOraAccensione = new System.Windows.Forms.ComboBox();
            this.rbtAccensione03 = new System.Windows.Forms.RadioButton();
            this.lblOreRitardo = new System.Windows.Forms.Label();
            this.cmbOreRitardo = new System.Windows.Forms.ComboBox();
            this.rbtAccensione02 = new System.Windows.Forms.RadioButton();
            this.rbtAccensione01 = new System.Windows.Forms.RadioButton();
            this.grbOraCorrente = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnLeggiRtc = new System.Windows.Forms.Button();
            this.txtOraRtc = new System.Windows.Forms.TextBox();
            this.lblOraRTC = new System.Windows.Forms.Label();
            this.txtDataRtc = new System.Windows.Forms.TextBox();
            this.lblDataRTC = new System.Windows.Forms.Label();
            this.tabProfiloAttuale = new System.Windows.Forms.TabPage();
            this.tbcPaSottopagina = new System.Windows.Forms.TabControl();
            this.tbpPaProfiloAttivo = new System.Windows.Forms.TabPage();
            this.pippo = new System.Windows.Forms.GroupBox();
            this.btnPaProfileClear = new System.Windows.Forms.Button();
            this.grbPaImpostazioniLocali = new System.Windows.Forms.GroupBox();
            this.btnPaProfileImport = new System.Windows.Forms.Button();
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
            this.btnPaProfileChiudiCanale = new System.Windows.Forms.Button();
            this.btnPaCaricaCicli = new System.Windows.Forms.Button();
            this.chkPaSbloccaValori = new System.Windows.Forms.CheckBox();
            this.lblPaSbloccaValori = new System.Windows.Forms.Label();
            this.btnCicloCorrente = new System.Windows.Forms.Button();
            this.btnPaProfileRefresh = new System.Windows.Forms.Button();
            this.picPaImmagineProfilo = new System.Windows.Forms.PictureBox();
            this.tbcPaSchedeValori = new System.Windows.Forms.TabControl();
            this.tbpPaGeneraleCiclo = new System.Windows.Forms.TabPage();
            this.txtPaCassone = new System.Windows.Forms.TextBox();
            this.label38 = new System.Windows.Forms.Label();
            this.txtPaIdSetup = new System.Windows.Forms.TextBox();
            this.lblPaIdSetup = new System.Windows.Forms.Label();
            this.txtPaNomeSetup = new System.Windows.Forms.TextBox();
            this.label152 = new System.Windows.Forms.Label();
            this.tbpPaPCStep0 = new System.Windows.Forms.TabPage();
            this.txtPaDurataMaxT0 = new System.Windows.Forms.TextBox();
            this.label39 = new System.Windows.Forms.Label();
            this.txtPaPrefaseI0 = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtPaSogliaV0 = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.tbpPaPCStep1 = new System.Windows.Forms.TabPage();
            this.cmbPaDurataMaxT1 = new System.Windows.Forms.TextBox();
            this.cmbPaDurataCarica = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPaCorrenteI1 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPaSogliaVs = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbpPaPCStep2 = new System.Windows.Forms.TabPage();
            this.txtPaTempoFin = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.txtPadT = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtPadV = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtPaCoeffKc = new System.Windows.Forms.TextBox();
            this.txtPaVMax = new System.Windows.Forms.TextBox();
            this.label207 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtPaCoeffK = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.txtPaTempoT2Max = new System.Windows.Forms.TextBox();
            this.txtPaTempoT2Min = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtPaCorrenteRaccordo = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.txtPaCorrenteF3 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtPaRaccordoF1 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbpPaPCStep3 = new System.Windows.Forms.TabPage();
            this.label19 = new System.Windows.Forms.Label();
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
            this.label32 = new System.Windows.Forms.Label();
            this.txtPaMantDurataMax = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.txtPaMantVmax = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.txtPaMantVmin = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.txtPaMantAttesa = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.tbpPaPCOpp = new System.Windows.Forms.TabPage();
            this.lblPaOppPuntoVerde = new System.Windows.Forms.Label();
            this.ImgPaOppPuntoVerde = new System.Windows.Forms.PictureBox();
            this.chkPaOppNotturno = new System.Windows.Forms.CheckBox();
            this.rslPaOppFinestra = new Syncfusion.Windows.Forms.Tools.RangeSlider();
            this.txtPaOppDurataMax = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.txtPaOppCorrente = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.txtPaOppVSoglia = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.txtPaOppOraFine = new System.Windows.Forms.TextBox();
            this.lblPaOppOraFine = new System.Windows.Forms.Label();
            this.txtPaOppOraInizio = new System.Windows.Forms.TextBox();
            this.lblPaOppOraInizio = new System.Windows.Forms.Label();
            this.tbpPaParSoglia = new System.Windows.Forms.TabPage();
            this.txtPaTempLimite = new System.Windows.Forms.TextBox();
            this.label59 = new System.Windows.Forms.Label();
            this.txtPaVMinStop = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.txtPaBMSTempoAttesa = new System.Windows.Forms.TextBox();
            this.label156 = new System.Windows.Forms.Label();
            this.txtPaBMSTempoErogazione = new System.Windows.Forms.TextBox();
            this.label157 = new System.Windows.Forms.Label();
            this.chkPaRiarmaBms = new System.Windows.Forms.Label();
            this.chkPaAttivaRiarmoBms = new System.Windows.Forms.CheckBox();
            this.txtPaCorrenteMassima = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.txtPaVMaxRic = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.txtPaVMinRic = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txtPaVLimite = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.chkPaUsaSpyBatt = new System.Windows.Forms.CheckBox();
            this.label69 = new System.Windows.Forms.Label();
            this.btnPaSalvaDati = new System.Windows.Forms.Button();
            this.tbpPaListaProfili = new System.Windows.Forms.TabPage();
            this.btnPaCancellaSelezionati = new System.Windows.Forms.Button();
            this.grbPaGeneraFile = new System.Windows.Forms.GroupBox();
            this.btnPaSalvaFile = new System.Windows.Forms.Button();
            this.chkPaSoloSelezionati = new System.Windows.Forms.CheckBox();
            this.btnPaNomeFileProfiliSRC = new System.Windows.Forms.Button();
            this.txtPaNomeFileProfili = new System.Windows.Forms.TextBox();
            this.btnPaAttivaConfigurazione = new System.Windows.Forms.Button();
            this.flwPaListaConfigurazioni = new BrightIdeasSoftware.FastObjectListView();
            this.btnPaCaricaListaProfili = new System.Windows.Forms.Button();
            this.tbpPaCfgAvanzate = new System.Windows.Forms.TabPage();
            this.grbVarParametriSig = new System.Windows.Forms.GroupBox();
            this.btnFSerVerificaOC = new System.Windows.Forms.Button();
            this.chkFSerEchoOC = new System.Windows.Forms.CheckBox();
            this.btnFSerImpostaOC = new System.Windows.Forms.Button();
            this.label278 = new System.Windows.Forms.Label();
            this.cmbFSerBaudrateOC = new System.Windows.Forms.ComboBox();
            this.lblPaTitoloLista = new System.Windows.Forms.Label();
            this.tabCb04 = new System.Windows.Forms.TabPage();
            this.flvCicliListaCariche = new BrightIdeasSoftware.FastObjectListView();
            this.grbCicli = new System.Windows.Forms.GroupBox();
            this.btnCicliCaricaCont = new System.Windows.Forms.Button();
            this.btnCicliCaricaArea = new System.Windows.Forms.Button();
            this.btnCicliMostraBrevi = new System.Windows.Forms.Button();
            this.btnCicliCaricaBrevi = new System.Windows.Forms.Button();
            this.chkCicliCaricaBrevi = new System.Windows.Forms.CheckBox();
            this.label223 = new System.Windows.Forms.Label();
            this.txtCicliNumRecord = new System.Windows.Forms.TextBox();
            this.label209 = new System.Windows.Forms.Label();
            this.txtCicliAddrPrmo = new System.Windows.Forms.TextBox();
            this.btnCicliCaricaLista = new System.Windows.Forms.Button();
            this.btnCicliVuotaLista = new System.Windows.Forms.Button();
            this.tabGenerale = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnSalveCliente = new System.Windows.Forms.Button();
            this.btnCaricaCliente = new System.Windows.Forms.Button();
            this.btnGenResetBoard = new System.Windows.Forms.Button();
            this.btnGenAzzzeraContatoriTot = new System.Windows.Forms.Button();
            this.btnGenAzzzeraContatori = new System.Windows.Forms.Button();
            this.btnCaricaContatori = new System.Windows.Forms.Button();
            this.grbMainContatori = new System.Windows.Forms.GroupBox();
            this.txtContCaricheOpportunity = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.txtContNumProgrammazioni = new System.Windows.Forms.TextBox();
            this.label222 = new System.Windows.Forms.Label();
            this.label221 = new System.Windows.Forms.Label();
            this.txtContPntNextBreve = new System.Windows.Forms.TextBox();
            this.label220 = new System.Windows.Forms.Label();
            this.txtContNumCancellazioni = new System.Windows.Forms.TextBox();
            this.label219 = new System.Windows.Forms.Label();
            this.label217 = new System.Windows.Forms.Label();
            this.txtContDtUltimaCanc = new System.Windows.Forms.TextBox();
            this.txtContPntNextCarica = new System.Windows.Forms.TextBox();
            this.txtContBreviSalvati = new System.Windows.Forms.TextBox();
            this.txtContCaricheSalvate = new System.Windows.Forms.TextBox();
            this.txtContCaricheOver9 = new System.Windows.Forms.TextBox();
            this.txtContCariche6to9 = new System.Windows.Forms.TextBox();
            this.txtContCariche3to6 = new System.Windows.Forms.TextBox();
            this.txtContCaricheUnder3 = new System.Windows.Forms.TextBox();
            this.label208 = new System.Windows.Forms.Label();
            this.txtContCaricheStrappo = new System.Windows.Forms.TextBox();
            this.label210 = new System.Windows.Forms.Label();
            this.txtContCaricheStop = new System.Windows.Forms.TextBox();
            this.label211 = new System.Windows.Forms.Label();
            this.label212 = new System.Windows.Forms.Label();
            this.label213 = new System.Windows.Forms.Label();
            this.label214 = new System.Windows.Forms.Label();
            this.label215 = new System.Windows.Forms.Label();
            this.txtContCaricheTotali = new System.Windows.Forms.TextBox();
            this.label216 = new System.Windows.Forms.Label();
            this.txtContDtPrimaCarica = new System.Windows.Forms.TextBox();
            this.label218 = new System.Windows.Forms.Label();
            this.GrbMainDatiApparato = new System.Windows.Forms.GroupBox();
            this.txtGenSerialeZVT = new System.Windows.Forms.TextBox();
            this.label151 = new System.Windows.Forms.Label();
            this.txtGenCorrenteMax = new System.Windows.Forms.TextBox();
            this.label143 = new System.Windows.Forms.Label();
            this.txtGenTensioneMax = new System.Windows.Forms.TextBox();
            this.label142 = new System.Windows.Forms.Label();
            this.txtGenModello = new System.Windows.Forms.TextBox();
            this.label141 = new System.Windows.Forms.Label();
            this.txtGenRevFwDisplay = new System.Windows.Forms.TextBox();
            this.label140 = new System.Windows.Forms.Label();
            this.textBox34 = new System.Windows.Forms.TextBox();
            this.label139 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label138 = new System.Windows.Forms.Label();
            this.txtGenRevHwDisplay = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtGenIdApparato = new System.Windows.Forms.TextBox();
            this.label137 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtGenAnnoMatricola = new System.Windows.Forms.TextBox();
            this.txtGenMatricola = new System.Windows.Forms.TextBox();
            this.lblMatricola = new System.Windows.Forms.Label();
            this.grbDatiCliente = new System.Windows.Forms.GroupBox();
            this.txtCliNomeIntLL = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.label254 = new System.Windows.Forms.Label();
            this.txtCliCodiceLL = new System.Windows.Forms.TextBox();
            this.txtCliNote = new System.Windows.Forms.TextBox();
            this.txtCliDescrizione = new System.Windows.Forms.TextBox();
            this.lblCliIdBatteria = new System.Windows.Forms.Label();
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.lbNoteCliente = new System.Windows.Forms.Label();
            this.lblCliCliente = new System.Windows.Forms.Label();
            this.tabCaricaBatterie = new System.Windows.Forms.TabControl();
            this.directoryEntry1 = new System.DirectoryServices.DirectoryEntry();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabMonitor.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvwLettureParametri)).BeginInit();
            this.grbVariabiliImmediate.SuspendLayout();
            this.tabCb02.SuspendLayout();
            this.grbCavi.SuspendLayout();
            this.tbpProxySig60.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.grbStratStepCorrente.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlStratContatoriCarica.SuspendLayout();
            this.panel3.SuspendLayout();
            this.grbStratComandiTest.SuspendLayout();
            this.tbpFirmware.SuspendLayout();
            this.grbFwAttivazioneArea.SuspendLayout();
            this.grbFWPreparaFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvwFWInFileStruct)).BeginInit();
            this.grbFWArea2.SuspendLayout();
            this.GrbFWArea1.SuspendLayout();
            this.grbFWAggiornamento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flwFWFileLLFStruct)).BeginInit();
            this.grbStatoFirmware.SuspendLayout();
            this.tabMemRead.SuspendLayout();
            this.grbMemTestLetture.SuspendLayout();
            this.grbMemAzzeraLogger.SuspendLayout();
            this.grbMemCaricaLogger.SuspendLayout();
            this.grbMemSalvaLogger.SuspendLayout();
            this.grbMemCancFisica.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.grbMemScrittura.SuspendLayout();
            this.grbMemCancellazione.SuspendLayout();
            this.grbMemLettura.SuspendLayout();
            this.tabInizializzazione.SuspendLayout();
            this.grbConnessioni.SuspendLayout();
            this.grbGenBaudrate.SuspendLayout();
            this.grbInitLimiti.SuspendLayout();
            this.groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitBrdANomModulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitBrdVNomModulo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitBrdNumModuli)).BeginInit();
            this.grbInitCalibrazione.SuspendLayout();
            this.grbInitDatiBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitAMax)).BeginInit();
            this.tabOrologio.SuspendLayout();
            this.grbCalData.SuspendLayout();
            this.grbAccensione.SuspendLayout();
            this.grbOraCorrente.SuspendLayout();
            this.tabProfiloAttuale.SuspendLayout();
            this.tbcPaSottopagina.SuspendLayout();
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
            this.tbpPaListaProfili.SuspendLayout();
            this.grbPaGeneraFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flwPaListaConfigurazioni)).BeginInit();
            this.tbpPaCfgAvanzate.SuspendLayout();
            this.grbVarParametriSig.SuspendLayout();
            this.tabCb04.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvCicliListaCariche)).BeginInit();
            this.grbCicli.SuspendLayout();
            this.tabGenerale.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grbMainContatori.SuspendLayout();
            this.GrbMainDatiApparato.SuspendLayout();
            this.grbDatiCliente.SuspendLayout();
            this.tabCaricaBatterie.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrLetturaAutomatica
            // 
            this.tmrLetturaAutomatica.Interval = 30000;
            this.tmrLetturaAutomatica.Tick += new System.EventHandler(this.tmrLetturaAutomatica_Tick);
            // 
            // sfdImportDati
            // 
            this.sfdImportDati.FileName = "prova";
            // 
            // tabMonitor
            // 
            this.tabMonitor.BackColor = System.Drawing.Color.LightYellow;
            this.tabMonitor.Controls.Add(this.groupBox6);
            this.tabMonitor.Controls.Add(this.chkParRegistraLetture);
            this.tabMonitor.Controls.Add(this.flvwLettureParametri);
            this.tabMonitor.Controls.Add(this.txtParIntervalloLettura);
            this.tabMonitor.Controls.Add(this.label68);
            this.tabMonitor.Controls.Add(this.chkParLetturaAuto);
            this.tabMonitor.Controls.Add(this.chkDatiDiretti);
            this.tabMonitor.Controls.Add(this.btnLeggiVariabili);
            this.tabMonitor.Controls.Add(this.grbVariabiliImmediate);
            resources.ApplyResources(this.tabMonitor, "tabMonitor");
            this.tabMonitor.Name = "tabMonitor";
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.Color.White;
            this.groupBox6.Controls.Add(this.txtVarGeneraExcel);
            this.groupBox6.Controls.Add(this.btnVarFilesearch);
            this.groupBox6.Controls.Add(this.txtVarFileCicli);
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // txtVarGeneraExcel
            // 
            resources.ApplyResources(this.txtVarGeneraExcel, "txtVarGeneraExcel");
            this.txtVarGeneraExcel.Name = "txtVarGeneraExcel";
            this.txtVarGeneraExcel.UseVisualStyleBackColor = true;
            this.txtVarGeneraExcel.Click += new System.EventHandler(this.txtVarGeneraExcel_Click);
            // 
            // btnVarFilesearch
            // 
            resources.ApplyResources(this.btnVarFilesearch, "btnVarFilesearch");
            this.btnVarFilesearch.Name = "btnVarFilesearch";
            this.btnVarFilesearch.UseVisualStyleBackColor = true;
            this.btnVarFilesearch.Click += new System.EventHandler(this.btnVarFilesearch_Click);
            // 
            // txtVarFileCicli
            // 
            resources.ApplyResources(this.txtVarFileCicli, "txtVarFileCicli");
            this.txtVarFileCicli.Name = "txtVarFileCicli";
            // 
            // chkParRegistraLetture
            // 
            resources.ApplyResources(this.chkParRegistraLetture, "chkParRegistraLetture");
            this.chkParRegistraLetture.Name = "chkParRegistraLetture";
            this.chkParRegistraLetture.UseVisualStyleBackColor = true;
            this.chkParRegistraLetture.CheckedChanged += new System.EventHandler(this.chkParRegistraLetture_CheckedChanged);
            // 
            // flvwLettureParametri
            // 
            this.flvwLettureParametri.AllowColumnReorder = true;
            this.flvwLettureParametri.AlternateRowBackColor = System.Drawing.Color.LightYellow;
            this.flvwLettureParametri.CellEditUseWholeCell = false;
            this.flvwLettureParametri.FullRowSelect = true;
            this.flvwLettureParametri.HideSelection = false;
            resources.ApplyResources(this.flvwLettureParametri, "flvwLettureParametri");
            this.flvwLettureParametri.Name = "flvwLettureParametri";
            this.flvwLettureParametri.ShowGroups = false;
            this.flvwLettureParametri.ShowImagesOnSubItems = true;
            this.flvwLettureParametri.UseAlternatingBackColors = true;
            this.flvwLettureParametri.UseCellFormatEvents = true;
            this.flvwLettureParametri.UseCompatibleStateImageBehavior = false;
            this.flvwLettureParametri.View = System.Windows.Forms.View.Details;
            this.flvwLettureParametri.VirtualMode = true;
            // 
            // txtParIntervalloLettura
            // 
            resources.ApplyResources(this.txtParIntervalloLettura, "txtParIntervalloLettura");
            this.txtParIntervalloLettura.Name = "txtParIntervalloLettura";
            // 
            // label68
            // 
            resources.ApplyResources(this.label68, "label68");
            this.label68.Name = "label68";
            // 
            // chkParLetturaAuto
            // 
            resources.ApplyResources(this.chkParLetturaAuto, "chkParLetturaAuto");
            this.chkParLetturaAuto.Name = "chkParLetturaAuto";
            this.chkParLetturaAuto.UseVisualStyleBackColor = true;
            this.chkParLetturaAuto.CheckedChanged += new System.EventHandler(this.chkParLetturaAuto_CheckedChanged);
            // 
            // chkDatiDiretti
            // 
            resources.ApplyResources(this.chkDatiDiretti, "chkDatiDiretti");
            this.chkDatiDiretti.Name = "chkDatiDiretti";
            this.chkDatiDiretti.UseVisualStyleBackColor = true;
            // 
            // btnLeggiVariabili
            // 
            resources.ApplyResources(this.btnLeggiVariabili, "btnLeggiVariabili");
            this.btnLeggiVariabili.Name = "btnLeggiVariabili";
            this.btnLeggiVariabili.UseVisualStyleBackColor = true;
            this.btnLeggiVariabili.Click += new System.EventHandler(this.btnLeggiVariabili_Click);
            // 
            // grbVariabiliImmediate
            // 
            this.grbVariabiliImmediate.BackColor = System.Drawing.Color.White;
            this.grbVariabiliImmediate.Controls.Add(this.label67);
            this.grbVariabiliImmediate.Controls.Add(this.txtVarTempoTrascorso);
            this.grbVariabiliImmediate.Controls.Add(this.txtVarMemProgrammed);
            this.grbVariabiliImmediate.Controls.Add(this.label66);
            this.grbVariabiliImmediate.Controls.Add(this.label73);
            this.grbVariabiliImmediate.Controls.Add(this.txtVarIbatt);
            this.grbVariabiliImmediate.Controls.Add(this.lblVarVBatt);
            this.grbVariabiliImmediate.Controls.Add(this.label76);
            this.grbVariabiliImmediate.Controls.Add(this.txtVarAhCarica);
            this.grbVariabiliImmediate.Controls.Add(this.txtVarVBatt);
            resources.ApplyResources(this.grbVariabiliImmediate, "grbVariabiliImmediate");
            this.grbVariabiliImmediate.Name = "grbVariabiliImmediate";
            this.grbVariabiliImmediate.TabStop = false;
            // 
            // label67
            // 
            resources.ApplyResources(this.label67, "label67");
            this.label67.Name = "label67";
            // 
            // txtVarTempoTrascorso
            // 
            resources.ApplyResources(this.txtVarTempoTrascorso, "txtVarTempoTrascorso");
            this.txtVarTempoTrascorso.Name = "txtVarTempoTrascorso";
            // 
            // txtVarMemProgrammed
            // 
            resources.ApplyResources(this.txtVarMemProgrammed, "txtVarMemProgrammed");
            this.txtVarMemProgrammed.Name = "txtVarMemProgrammed";
            // 
            // label66
            // 
            resources.ApplyResources(this.label66, "label66");
            this.label66.Name = "label66";
            // 
            // label73
            // 
            resources.ApplyResources(this.label73, "label73");
            this.label73.Name = "label73";
            // 
            // txtVarIbatt
            // 
            resources.ApplyResources(this.txtVarIbatt, "txtVarIbatt");
            this.txtVarIbatt.Name = "txtVarIbatt";
            // 
            // lblVarVBatt
            // 
            resources.ApplyResources(this.lblVarVBatt, "lblVarVBatt");
            this.lblVarVBatt.Name = "lblVarVBatt";
            // 
            // label76
            // 
            resources.ApplyResources(this.label76, "label76");
            this.label76.Name = "label76";
            // 
            // txtVarAhCarica
            // 
            resources.ApplyResources(this.txtVarAhCarica, "txtVarAhCarica");
            this.txtVarAhCarica.Name = "txtVarAhCarica";
            // 
            // txtVarVBatt
            // 
            resources.ApplyResources(this.txtVarVBatt, "txtVarVBatt");
            this.txtVarVBatt.Name = "txtVarVBatt";
            // 
            // tabCb02
            // 
            this.tabCb02.BackColor = System.Drawing.Color.LightYellow;
            this.tabCb02.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tabCb02, "tabCb02");
            this.tabCb02.Controls.Add(this.grbCavi);
            this.tabCb02.Name = "tabCb02";
            // 
            // grbCavi
            // 
            this.grbCavi.Controls.Add(this.txtEsitoVerCavi);
            this.grbCavi.Controls.Add(this.lblEsito);
            this.grbCavi.Controls.Add(this.label3);
            this.grbCavi.Controls.Add(this.txtPotenzaDispersa);
            this.grbCavi.Controls.Add(this.lblPotenzaDispersa);
            this.grbCavi.Controls.Add(this.btnVerificaCavi);
            this.grbCavi.Controls.Add(this.comboBox4);
            this.grbCavi.Controls.Add(this.lblCaviSelVerifica);
            this.grbCavi.Controls.Add(this.cmbSezioneProlunga);
            this.grbCavi.Controls.Add(this.label1);
            this.grbCavi.Controls.Add(this.textBox2);
            this.grbCavi.Controls.Add(this.label2);
            this.grbCavi.Controls.Add(this.lblProlunga);
            this.grbCavi.Controls.Add(this.cmbSezioneCavo);
            this.grbCavi.Controls.Add(this.lblSezioneCavo);
            this.grbCavi.Controls.Add(this.txtLunghezzaCavo);
            this.grbCavi.Controls.Add(this.lblLunghezzaCavo);
            this.grbCavi.Controls.Add(this.lblCavoCaricabatterie);
            resources.ApplyResources(this.grbCavi, "grbCavi");
            this.grbCavi.Name = "grbCavi";
            this.grbCavi.TabStop = false;
            // 
            // txtEsitoVerCavi
            // 
            resources.ApplyResources(this.txtEsitoVerCavi, "txtEsitoVerCavi");
            this.txtEsitoVerCavi.Name = "txtEsitoVerCavi";
            // 
            // lblEsito
            // 
            resources.ApplyResources(this.lblEsito, "lblEsito");
            this.lblEsito.Name = "lblEsito";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtPotenzaDispersa
            // 
            resources.ApplyResources(this.txtPotenzaDispersa, "txtPotenzaDispersa");
            this.txtPotenzaDispersa.Name = "txtPotenzaDispersa";
            // 
            // lblPotenzaDispersa
            // 
            resources.ApplyResources(this.lblPotenzaDispersa, "lblPotenzaDispersa");
            this.lblPotenzaDispersa.Name = "lblPotenzaDispersa";
            // 
            // btnVerificaCavi
            // 
            resources.ApplyResources(this.btnVerificaCavi, "btnVerificaCavi");
            this.btnVerificaCavi.Name = "btnVerificaCavi";
            this.btnVerificaCavi.UseVisualStyleBackColor = true;
            // 
            // comboBox4
            // 
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            resources.GetString("comboBox4.Items"),
            resources.GetString("comboBox4.Items1")});
            resources.ApplyResources(this.comboBox4, "comboBox4");
            this.comboBox4.Name = "comboBox4";
            // 
            // lblCaviSelVerifica
            // 
            resources.ApplyResources(this.lblCaviSelVerifica, "lblCaviSelVerifica");
            this.lblCaviSelVerifica.Name = "lblCaviSelVerifica";
            // 
            // cmbSezioneProlunga
            // 
            this.cmbSezioneProlunga.FormattingEnabled = true;
            this.cmbSezioneProlunga.Items.AddRange(new object[] {
            resources.GetString("cmbSezioneProlunga.Items"),
            resources.GetString("cmbSezioneProlunga.Items1"),
            resources.GetString("cmbSezioneProlunga.Items2"),
            resources.GetString("cmbSezioneProlunga.Items3"),
            resources.GetString("cmbSezioneProlunga.Items4"),
            resources.GetString("cmbSezioneProlunga.Items5"),
            resources.GetString("cmbSezioneProlunga.Items6"),
            resources.GetString("cmbSezioneProlunga.Items7"),
            resources.GetString("cmbSezioneProlunga.Items8"),
            resources.GetString("cmbSezioneProlunga.Items9"),
            resources.GetString("cmbSezioneProlunga.Items10"),
            resources.GetString("cmbSezioneProlunga.Items11"),
            resources.GetString("cmbSezioneProlunga.Items12")});
            resources.ApplyResources(this.cmbSezioneProlunga, "cmbSezioneProlunga");
            this.cmbSezioneProlunga.Name = "cmbSezioneProlunga";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox2
            // 
            resources.ApplyResources(this.textBox2, "textBox2");
            this.textBox2.Name = "textBox2";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lblProlunga
            // 
            resources.ApplyResources(this.lblProlunga, "lblProlunga");
            this.lblProlunga.Name = "lblProlunga";
            // 
            // cmbSezioneCavo
            // 
            this.cmbSezioneCavo.FormattingEnabled = true;
            this.cmbSezioneCavo.Items.AddRange(new object[] {
            resources.GetString("cmbSezioneCavo.Items"),
            resources.GetString("cmbSezioneCavo.Items1"),
            resources.GetString("cmbSezioneCavo.Items2"),
            resources.GetString("cmbSezioneCavo.Items3"),
            resources.GetString("cmbSezioneCavo.Items4"),
            resources.GetString("cmbSezioneCavo.Items5"),
            resources.GetString("cmbSezioneCavo.Items6"),
            resources.GetString("cmbSezioneCavo.Items7"),
            resources.GetString("cmbSezioneCavo.Items8"),
            resources.GetString("cmbSezioneCavo.Items9"),
            resources.GetString("cmbSezioneCavo.Items10"),
            resources.GetString("cmbSezioneCavo.Items11"),
            resources.GetString("cmbSezioneCavo.Items12")});
            resources.ApplyResources(this.cmbSezioneCavo, "cmbSezioneCavo");
            this.cmbSezioneCavo.Name = "cmbSezioneCavo";
            // 
            // lblSezioneCavo
            // 
            resources.ApplyResources(this.lblSezioneCavo, "lblSezioneCavo");
            this.lblSezioneCavo.Name = "lblSezioneCavo";
            // 
            // txtLunghezzaCavo
            // 
            resources.ApplyResources(this.txtLunghezzaCavo, "txtLunghezzaCavo");
            this.txtLunghezzaCavo.Name = "txtLunghezzaCavo";
            // 
            // lblLunghezzaCavo
            // 
            resources.ApplyResources(this.lblLunghezzaCavo, "lblLunghezzaCavo");
            this.lblLunghezzaCavo.Name = "lblLunghezzaCavo";
            // 
            // lblCavoCaricabatterie
            // 
            resources.ApplyResources(this.lblCavoCaricabatterie, "lblCavoCaricabatterie");
            this.lblCavoCaricabatterie.Name = "lblCavoCaricabatterie";
            // 
            // tbpProxySig60
            // 
            this.tbpProxySig60.BackColor = System.Drawing.Color.LightYellow;
            this.tbpProxySig60.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tbpProxySig60, "tbpProxySig60");
            this.tbpProxySig60.Controls.Add(this.label176);
            this.tbpProxySig60.Controls.Add(this.panel1);
            this.tbpProxySig60.Controls.Add(this.label200);
            this.tbpProxySig60.Controls.Add(this.panel6);
            this.tbpProxySig60.Controls.Add(this.label189);
            this.tbpProxySig60.Controls.Add(this.panel5);
            this.tbpProxySig60.Controls.Add(this.label185);
            this.tbpProxySig60.Controls.Add(this.panel4);
            this.tbpProxySig60.Controls.Add(this.label166);
            this.tbpProxySig60.Controls.Add(this.pnlStratContatoriCarica);
            this.tbpProxySig60.Controls.Add(this.label167);
            this.tbpProxySig60.Controls.Add(this.panel3);
            this.tbpProxySig60.Controls.Add(this.lblSig60DataReceuved);
            this.tbpProxySig60.Controls.Add(this.lblSig60DataSent);
            this.tbpProxySig60.Controls.Add(this.txtStratDataGridRx);
            this.tbpProxySig60.Controls.Add(this.txtStratDataGridTx);
            this.tbpProxySig60.Controls.Add(this.grbStratComandiTest);
            this.tbpProxySig60.Name = "tbpProxySig60";
            // 
            // label176
            // 
            resources.ApplyResources(this.label176, "label176");
            this.label176.Name = "label176";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.chkStratLLRabb);
            this.panel1.Controls.Add(this.txtStratLLAmax);
            this.panel1.Controls.Add(this.label155);
            this.panel1.Controls.Add(this.txtStratLLVmax);
            this.panel1.Controls.Add(this.label154);
            this.panel1.Controls.Add(this.txtStratLLVmin);
            this.panel1.Controls.Add(this.label153);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // chkStratLLRabb
            // 
            resources.ApplyResources(this.chkStratLLRabb, "chkStratLLRabb");
            this.chkStratLLRabb.Name = "chkStratLLRabb";
            this.chkStratLLRabb.UseVisualStyleBackColor = true;
            // 
            // txtStratLLAmax
            // 
            resources.ApplyResources(this.txtStratLLAmax, "txtStratLLAmax");
            this.txtStratLLAmax.Name = "txtStratLLAmax";
            // 
            // label155
            // 
            resources.ApplyResources(this.label155, "label155");
            this.label155.Name = "label155";
            // 
            // txtStratLLVmax
            // 
            resources.ApplyResources(this.txtStratLLVmax, "txtStratLLVmax");
            this.txtStratLLVmax.Name = "txtStratLLVmax";
            // 
            // label154
            // 
            resources.ApplyResources(this.label154, "label154");
            this.label154.Name = "label154";
            // 
            // txtStratLLVmin
            // 
            resources.ApplyResources(this.txtStratLLVmin, "txtStratLLVmin");
            this.txtStratLLVmin.Name = "txtStratLLVmin";
            // 
            // label153
            // 
            resources.ApplyResources(this.label153, "label153");
            this.label153.Name = "label153";
            // 
            // label200
            // 
            resources.ApplyResources(this.label200, "label200");
            this.label200.Name = "label200";
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.White;
            this.panel6.Controls.Add(this.grbStratStepCorrente);
            this.panel6.Controls.Add(this.label202);
            this.panel6.Controls.Add(this.cmbStratIsSelStep);
            this.panel6.Controls.Add(this.label199);
            this.panel6.Controls.Add(this.txtStratIsNumSpire);
            this.panel6.Controls.Add(this.label198);
            this.panel6.Controls.Add(this.txtStratIsStep);
            this.panel6.Controls.Add(this.txtStratIsEsito);
            this.panel6.Controls.Add(this.label197);
            this.panel6.Controls.Add(this.txtStratIsMinuti);
            this.panel6.Controls.Add(this.txtStratIsAhRich);
            this.panel6.Controls.Add(this.label193);
            this.panel6.Controls.Add(this.label196);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // grbStratStepCorrente
            // 
            this.grbStratStepCorrente.Controls.Add(this.label206);
            this.grbStratStepCorrente.Controls.Add(this.txtStratCurrStepTipo);
            this.grbStratStepCorrente.Controls.Add(this.label205);
            this.grbStratStepCorrente.Controls.Add(this.txtStratCurrStepRipetizioni);
            this.grbStratStepCorrente.Controls.Add(this.label203);
            this.grbStratStepCorrente.Controls.Add(this.txtStratCurrStepTon);
            this.grbStratStepCorrente.Controls.Add(this.txtStratCurrStepToff);
            this.grbStratStepCorrente.Controls.Add(this.label204);
            this.grbStratStepCorrente.Controls.Add(this.label194);
            this.grbStratStepCorrente.Controls.Add(this.txtStratCurrStepAh);
            this.grbStratStepCorrente.Controls.Add(this.label191);
            this.grbStratStepCorrente.Controls.Add(this.txtStratCurrStepVmax);
            this.grbStratStepCorrente.Controls.Add(this.txtStratCurrStepVmin);
            this.grbStratStepCorrente.Controls.Add(this.label192);
            this.grbStratStepCorrente.Controls.Add(this.label190);
            this.grbStratStepCorrente.Controls.Add(this.txtStratCurrStepImax);
            this.grbStratStepCorrente.Controls.Add(this.txtStratCurrStepImin);
            this.grbStratStepCorrente.Controls.Add(this.label195);
            resources.ApplyResources(this.grbStratStepCorrente, "grbStratStepCorrente");
            this.grbStratStepCorrente.Name = "grbStratStepCorrente";
            this.grbStratStepCorrente.TabStop = false;
            // 
            // label206
            // 
            resources.ApplyResources(this.label206, "label206");
            this.label206.Name = "label206";
            // 
            // txtStratCurrStepTipo
            // 
            resources.ApplyResources(this.txtStratCurrStepTipo, "txtStratCurrStepTipo");
            this.txtStratCurrStepTipo.Name = "txtStratCurrStepTipo";
            this.txtStratCurrStepTipo.ReadOnly = true;
            // 
            // label205
            // 
            resources.ApplyResources(this.label205, "label205");
            this.label205.Name = "label205";
            this.label205.Click += new System.EventHandler(this.label205_Click);
            // 
            // txtStratCurrStepRipetizioni
            // 
            resources.ApplyResources(this.txtStratCurrStepRipetizioni, "txtStratCurrStepRipetizioni");
            this.txtStratCurrStepRipetizioni.Name = "txtStratCurrStepRipetizioni";
            this.txtStratCurrStepRipetizioni.ReadOnly = true;
            this.txtStratCurrStepRipetizioni.TextChanged += new System.EventHandler(this.txtStratCurrStepRipetizioni_TextChanged);
            // 
            // label203
            // 
            resources.ApplyResources(this.label203, "label203");
            this.label203.Name = "label203";
            // 
            // txtStratCurrStepTon
            // 
            resources.ApplyResources(this.txtStratCurrStepTon, "txtStratCurrStepTon");
            this.txtStratCurrStepTon.Name = "txtStratCurrStepTon";
            this.txtStratCurrStepTon.ReadOnly = true;
            // 
            // txtStratCurrStepToff
            // 
            resources.ApplyResources(this.txtStratCurrStepToff, "txtStratCurrStepToff");
            this.txtStratCurrStepToff.Name = "txtStratCurrStepToff";
            this.txtStratCurrStepToff.ReadOnly = true;
            // 
            // label204
            // 
            resources.ApplyResources(this.label204, "label204");
            this.label204.Name = "label204";
            // 
            // label194
            // 
            resources.ApplyResources(this.label194, "label194");
            this.label194.Name = "label194";
            // 
            // txtStratCurrStepAh
            // 
            resources.ApplyResources(this.txtStratCurrStepAh, "txtStratCurrStepAh");
            this.txtStratCurrStepAh.Name = "txtStratCurrStepAh";
            this.txtStratCurrStepAh.ReadOnly = true;
            // 
            // label191
            // 
            resources.ApplyResources(this.label191, "label191");
            this.label191.Name = "label191";
            // 
            // txtStratCurrStepVmax
            // 
            resources.ApplyResources(this.txtStratCurrStepVmax, "txtStratCurrStepVmax");
            this.txtStratCurrStepVmax.Name = "txtStratCurrStepVmax";
            this.txtStratCurrStepVmax.ReadOnly = true;
            // 
            // txtStratCurrStepVmin
            // 
            resources.ApplyResources(this.txtStratCurrStepVmin, "txtStratCurrStepVmin");
            this.txtStratCurrStepVmin.Name = "txtStratCurrStepVmin";
            this.txtStratCurrStepVmin.ReadOnly = true;
            // 
            // label192
            // 
            resources.ApplyResources(this.label192, "label192");
            this.label192.Name = "label192";
            // 
            // label190
            // 
            resources.ApplyResources(this.label190, "label190");
            this.label190.Name = "label190";
            // 
            // txtStratCurrStepImax
            // 
            resources.ApplyResources(this.txtStratCurrStepImax, "txtStratCurrStepImax");
            this.txtStratCurrStepImax.Name = "txtStratCurrStepImax";
            this.txtStratCurrStepImax.ReadOnly = true;
            // 
            // txtStratCurrStepImin
            // 
            resources.ApplyResources(this.txtStratCurrStepImin, "txtStratCurrStepImin");
            this.txtStratCurrStepImin.Name = "txtStratCurrStepImin";
            this.txtStratCurrStepImin.ReadOnly = true;
            // 
            // label195
            // 
            resources.ApplyResources(this.label195, "label195");
            this.label195.Name = "label195";
            // 
            // label202
            // 
            resources.ApplyResources(this.label202, "label202");
            this.label202.Name = "label202";
            // 
            // cmbStratIsSelStep
            // 
            this.cmbStratIsSelStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbStratIsSelStep, "cmbStratIsSelStep");
            this.cmbStratIsSelStep.FormattingEnabled = true;
            this.cmbStratIsSelStep.Name = "cmbStratIsSelStep";
            // 
            // label199
            // 
            resources.ApplyResources(this.label199, "label199");
            this.label199.Name = "label199";
            // 
            // txtStratIsNumSpire
            // 
            resources.ApplyResources(this.txtStratIsNumSpire, "txtStratIsNumSpire");
            this.txtStratIsNumSpire.Name = "txtStratIsNumSpire";
            // 
            // label198
            // 
            resources.ApplyResources(this.label198, "label198");
            this.label198.Name = "label198";
            // 
            // txtStratIsStep
            // 
            resources.ApplyResources(this.txtStratIsStep, "txtStratIsStep");
            this.txtStratIsStep.Name = "txtStratIsStep";
            // 
            // txtStratIsEsito
            // 
            this.txtStratIsEsito.AcceptsTab = true;
            resources.ApplyResources(this.txtStratIsEsito, "txtStratIsEsito");
            this.txtStratIsEsito.Name = "txtStratIsEsito";
            // 
            // label197
            // 
            resources.ApplyResources(this.label197, "label197");
            this.label197.Name = "label197";
            // 
            // txtStratIsMinuti
            // 
            resources.ApplyResources(this.txtStratIsMinuti, "txtStratIsMinuti");
            this.txtStratIsMinuti.Name = "txtStratIsMinuti";
            // 
            // txtStratIsAhRich
            // 
            resources.ApplyResources(this.txtStratIsAhRich, "txtStratIsAhRich");
            this.txtStratIsAhRich.Name = "txtStratIsAhRich";
            // 
            // label193
            // 
            resources.ApplyResources(this.label193, "label193");
            this.label193.Name = "label193";
            // 
            // label196
            // 
            resources.ApplyResources(this.label196, "label196");
            this.label196.Name = "label196";
            // 
            // label189
            // 
            resources.ApplyResources(this.label189, "label189");
            this.label189.Name = "label189";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.White;
            this.panel5.Controls.Add(this.txtStratAVStop);
            this.panel5.Controls.Add(this.label201);
            this.panel5.Controls.Add(this.label179);
            this.panel5.Controls.Add(this.label177);
            this.panel5.Controls.Add(this.txtStratAVTempIst);
            this.panel5.Controls.Add(this.txtStratAVCorrenteIst);
            this.panel5.Controls.Add(this.txtStratAVPrevisti);
            this.panel5.Controls.Add(this.label180);
            this.panel5.Controls.Add(this.txtStratAVMancanti);
            this.panel5.Controls.Add(this.label181);
            this.panel5.Controls.Add(this.txtStratAVTensioneIst);
            this.panel5.Controls.Add(this.label182);
            this.panel5.Controls.Add(this.txtStratAVMinutiResidui);
            this.panel5.Controls.Add(this.label187);
            this.panel5.Controls.Add(this.txtStratAVErogati);
            this.panel5.Controls.Add(this.label188);
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Name = "panel5";
            // 
            // txtStratAVStop
            // 
            resources.ApplyResources(this.txtStratAVStop, "txtStratAVStop");
            this.txtStratAVStop.Name = "txtStratAVStop";
            // 
            // label201
            // 
            resources.ApplyResources(this.label201, "label201");
            this.label201.Name = "label201";
            // 
            // label179
            // 
            resources.ApplyResources(this.label179, "label179");
            this.label179.Name = "label179";
            // 
            // label177
            // 
            resources.ApplyResources(this.label177, "label177");
            this.label177.Name = "label177";
            // 
            // txtStratAVTempIst
            // 
            resources.ApplyResources(this.txtStratAVTempIst, "txtStratAVTempIst");
            this.txtStratAVTempIst.Name = "txtStratAVTempIst";
            // 
            // txtStratAVCorrenteIst
            // 
            resources.ApplyResources(this.txtStratAVCorrenteIst, "txtStratAVCorrenteIst");
            this.txtStratAVCorrenteIst.Name = "txtStratAVCorrenteIst";
            // 
            // txtStratAVPrevisti
            // 
            resources.ApplyResources(this.txtStratAVPrevisti, "txtStratAVPrevisti");
            this.txtStratAVPrevisti.Name = "txtStratAVPrevisti";
            // 
            // label180
            // 
            resources.ApplyResources(this.label180, "label180");
            this.label180.Name = "label180";
            // 
            // txtStratAVMancanti
            // 
            resources.ApplyResources(this.txtStratAVMancanti, "txtStratAVMancanti");
            this.txtStratAVMancanti.Name = "txtStratAVMancanti";
            // 
            // label181
            // 
            resources.ApplyResources(this.label181, "label181");
            this.label181.Name = "label181";
            // 
            // txtStratAVTensioneIst
            // 
            resources.ApplyResources(this.txtStratAVTensioneIst, "txtStratAVTensioneIst");
            this.txtStratAVTensioneIst.Name = "txtStratAVTensioneIst";
            // 
            // label182
            // 
            resources.ApplyResources(this.label182, "label182");
            this.label182.Name = "label182";
            // 
            // txtStratAVMinutiResidui
            // 
            resources.ApplyResources(this.txtStratAVMinutiResidui, "txtStratAVMinutiResidui");
            this.txtStratAVMinutiResidui.Name = "txtStratAVMinutiResidui";
            // 
            // label187
            // 
            resources.ApplyResources(this.label187, "label187");
            this.label187.Name = "label187";
            // 
            // txtStratAVErogati
            // 
            resources.ApplyResources(this.txtStratAVErogati, "txtStratAVErogati");
            this.txtStratAVErogati.Name = "txtStratAVErogati";
            // 
            // label188
            // 
            resources.ApplyResources(this.label188, "label188");
            this.label188.Name = "label188";
            // 
            // label185
            // 
            resources.ApplyResources(this.label185, "label185");
            this.label185.Name = "label185";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.txtStratLivcrgCapacityVer);
            this.panel4.Controls.Add(this.txtStratLivcrgDschgVer);
            this.panel4.Controls.Add(this.txtStratLivcrgChgVer);
            this.panel4.Controls.Add(this.txtStratLivcrgSetCapacity);
            this.panel4.Controls.Add(this.label178);
            this.panel4.Controls.Add(this.txtStratLivcrgSetDschg);
            this.panel4.Controls.Add(this.label183);
            this.panel4.Controls.Add(this.txtStratLivcrgSetChg);
            this.panel4.Controls.Add(this.label184);
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // txtStratLivcrgCapacityVer
            // 
            resources.ApplyResources(this.txtStratLivcrgCapacityVer, "txtStratLivcrgCapacityVer");
            this.txtStratLivcrgCapacityVer.Name = "txtStratLivcrgCapacityVer";
            this.txtStratLivcrgCapacityVer.ReadOnly = true;
            // 
            // txtStratLivcrgDschgVer
            // 
            resources.ApplyResources(this.txtStratLivcrgDschgVer, "txtStratLivcrgDschgVer");
            this.txtStratLivcrgDschgVer.Name = "txtStratLivcrgDschgVer";
            this.txtStratLivcrgDschgVer.ReadOnly = true;
            // 
            // txtStratLivcrgChgVer
            // 
            resources.ApplyResources(this.txtStratLivcrgChgVer, "txtStratLivcrgChgVer");
            this.txtStratLivcrgChgVer.Name = "txtStratLivcrgChgVer";
            this.txtStratLivcrgChgVer.ReadOnly = true;
            // 
            // txtStratLivcrgSetCapacity
            // 
            resources.ApplyResources(this.txtStratLivcrgSetCapacity, "txtStratLivcrgSetCapacity");
            this.txtStratLivcrgSetCapacity.Name = "txtStratLivcrgSetCapacity";
            // 
            // label178
            // 
            resources.ApplyResources(this.label178, "label178");
            this.label178.Name = "label178";
            // 
            // txtStratLivcrgSetDschg
            // 
            resources.ApplyResources(this.txtStratLivcrgSetDschg, "txtStratLivcrgSetDschg");
            this.txtStratLivcrgSetDschg.Name = "txtStratLivcrgSetDschg";
            // 
            // label183
            // 
            resources.ApplyResources(this.label183, "label183");
            this.label183.Name = "label183";
            // 
            // txtStratLivcrgSetChg
            // 
            resources.ApplyResources(this.txtStratLivcrgSetChg, "txtStratLivcrgSetChg");
            this.txtStratLivcrgSetChg.Name = "txtStratLivcrgSetChg";
            // 
            // label184
            // 
            resources.ApplyResources(this.label184, "label184");
            this.label184.Name = "label184";
            // 
            // label166
            // 
            resources.ApplyResources(this.label166, "label166");
            this.label166.Name = "label166";
            // 
            // pnlStratContatoriCarica
            // 
            this.pnlStratContatoriCarica.BackColor = System.Drawing.Color.White;
            this.pnlStratContatoriCarica.Controls.Add(this.txtStratLivcrgCapNominale);
            this.pnlStratContatoriCarica.Controls.Add(this.label165);
            this.pnlStratContatoriCarica.Controls.Add(this.txtStratLivcrgCapResidua);
            this.pnlStratContatoriCarica.Controls.Add(this.label164);
            this.pnlStratContatoriCarica.Controls.Add(this.txtStratLivcrgDiscrgTot);
            this.pnlStratContatoriCarica.Controls.Add(this.label162);
            this.pnlStratContatoriCarica.Controls.Add(this.txtStratLivcrgCrgTot);
            this.pnlStratContatoriCarica.Controls.Add(this.label163);
            this.pnlStratContatoriCarica.Controls.Add(this.txtStratLivcrgDiscrg);
            this.pnlStratContatoriCarica.Controls.Add(this.label160);
            this.pnlStratContatoriCarica.Controls.Add(this.txtStratLivcrgCrg);
            this.pnlStratContatoriCarica.Controls.Add(this.label161);
            this.pnlStratContatoriCarica.Controls.Add(this.txtStratLivcrgNeg);
            this.pnlStratContatoriCarica.Controls.Add(this.label159);
            this.pnlStratContatoriCarica.Controls.Add(this.txtStratLivcrgPos);
            this.pnlStratContatoriCarica.Controls.Add(this.label158);
            resources.ApplyResources(this.pnlStratContatoriCarica, "pnlStratContatoriCarica");
            this.pnlStratContatoriCarica.Name = "pnlStratContatoriCarica";
            // 
            // txtStratLivcrgCapNominale
            // 
            resources.ApplyResources(this.txtStratLivcrgCapNominale, "txtStratLivcrgCapNominale");
            this.txtStratLivcrgCapNominale.Name = "txtStratLivcrgCapNominale";
            this.txtStratLivcrgCapNominale.ReadOnly = true;
            // 
            // label165
            // 
            resources.ApplyResources(this.label165, "label165");
            this.label165.Name = "label165";
            // 
            // txtStratLivcrgCapResidua
            // 
            resources.ApplyResources(this.txtStratLivcrgCapResidua, "txtStratLivcrgCapResidua");
            this.txtStratLivcrgCapResidua.Name = "txtStratLivcrgCapResidua";
            this.txtStratLivcrgCapResidua.ReadOnly = true;
            // 
            // label164
            // 
            resources.ApplyResources(this.label164, "label164");
            this.label164.Name = "label164";
            // 
            // txtStratLivcrgDiscrgTot
            // 
            resources.ApplyResources(this.txtStratLivcrgDiscrgTot, "txtStratLivcrgDiscrgTot");
            this.txtStratLivcrgDiscrgTot.Name = "txtStratLivcrgDiscrgTot";
            this.txtStratLivcrgDiscrgTot.ReadOnly = true;
            // 
            // label162
            // 
            resources.ApplyResources(this.label162, "label162");
            this.label162.Name = "label162";
            // 
            // txtStratLivcrgCrgTot
            // 
            resources.ApplyResources(this.txtStratLivcrgCrgTot, "txtStratLivcrgCrgTot");
            this.txtStratLivcrgCrgTot.Name = "txtStratLivcrgCrgTot";
            this.txtStratLivcrgCrgTot.ReadOnly = true;
            // 
            // label163
            // 
            resources.ApplyResources(this.label163, "label163");
            this.label163.Name = "label163";
            // 
            // txtStratLivcrgDiscrg
            // 
            resources.ApplyResources(this.txtStratLivcrgDiscrg, "txtStratLivcrgDiscrg");
            this.txtStratLivcrgDiscrg.Name = "txtStratLivcrgDiscrg";
            this.txtStratLivcrgDiscrg.ReadOnly = true;
            // 
            // label160
            // 
            resources.ApplyResources(this.label160, "label160");
            this.label160.Name = "label160";
            // 
            // txtStratLivcrgCrg
            // 
            resources.ApplyResources(this.txtStratLivcrgCrg, "txtStratLivcrgCrg");
            this.txtStratLivcrgCrg.Name = "txtStratLivcrgCrg";
            this.txtStratLivcrgCrg.ReadOnly = true;
            // 
            // label161
            // 
            resources.ApplyResources(this.label161, "label161");
            this.label161.Name = "label161";
            // 
            // txtStratLivcrgNeg
            // 
            resources.ApplyResources(this.txtStratLivcrgNeg, "txtStratLivcrgNeg");
            this.txtStratLivcrgNeg.Name = "txtStratLivcrgNeg";
            this.txtStratLivcrgNeg.ReadOnly = true;
            // 
            // label159
            // 
            resources.ApplyResources(this.label159, "label159");
            this.label159.Name = "label159";
            // 
            // txtStratLivcrgPos
            // 
            resources.ApplyResources(this.txtStratLivcrgPos, "txtStratLivcrgPos");
            this.txtStratLivcrgPos.Name = "txtStratLivcrgPos";
            this.txtStratLivcrgPos.ReadOnly = true;
            // 
            // label158
            // 
            resources.ApplyResources(this.label158, "label158");
            this.label158.Name = "label158";
            // 
            // label167
            // 
            resources.ApplyResources(this.label167, "label167");
            this.label167.Name = "label167";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.txtStratQryTrepr);
            this.panel3.Controls.Add(this.label175);
            this.panel3.Controls.Add(this.txtStratQryTalm);
            this.panel3.Controls.Add(this.label174);
            this.panel3.Controls.Add(this.txtStratQryTatt);
            this.panel3.Controls.Add(this.label173);
            this.panel3.Controls.Add(this.txtStratQryTensGas);
            this.panel3.Controls.Add(this.label172);
            this.panel3.Controls.Add(this.txtStratQryCapN);
            this.panel3.Controls.Add(this.label171);
            this.panel3.Controls.Add(this.txtStratQryTensN);
            this.panel3.Controls.Add(this.label170);
            this.panel3.Controls.Add(this.txtStratQryActSeup);
            this.panel3.Controls.Add(this.label169);
            this.panel3.Controls.Add(this.txtStratQryVerLib);
            this.panel3.Controls.Add(this.label168);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // txtStratQryTrepr
            // 
            resources.ApplyResources(this.txtStratQryTrepr, "txtStratQryTrepr");
            this.txtStratQryTrepr.Name = "txtStratQryTrepr";
            this.txtStratQryTrepr.ReadOnly = true;
            // 
            // label175
            // 
            resources.ApplyResources(this.label175, "label175");
            this.label175.Name = "label175";
            // 
            // txtStratQryTalm
            // 
            resources.ApplyResources(this.txtStratQryTalm, "txtStratQryTalm");
            this.txtStratQryTalm.Name = "txtStratQryTalm";
            this.txtStratQryTalm.ReadOnly = true;
            // 
            // label174
            // 
            resources.ApplyResources(this.label174, "label174");
            this.label174.Name = "label174";
            // 
            // txtStratQryTatt
            // 
            resources.ApplyResources(this.txtStratQryTatt, "txtStratQryTatt");
            this.txtStratQryTatt.Name = "txtStratQryTatt";
            this.txtStratQryTatt.ReadOnly = true;
            // 
            // label173
            // 
            resources.ApplyResources(this.label173, "label173");
            this.label173.Name = "label173";
            // 
            // txtStratQryTensGas
            // 
            resources.ApplyResources(this.txtStratQryTensGas, "txtStratQryTensGas");
            this.txtStratQryTensGas.Name = "txtStratQryTensGas";
            this.txtStratQryTensGas.ReadOnly = true;
            // 
            // label172
            // 
            resources.ApplyResources(this.label172, "label172");
            this.label172.Name = "label172";
            // 
            // txtStratQryCapN
            // 
            resources.ApplyResources(this.txtStratQryCapN, "txtStratQryCapN");
            this.txtStratQryCapN.Name = "txtStratQryCapN";
            this.txtStratQryCapN.ReadOnly = true;
            // 
            // label171
            // 
            resources.ApplyResources(this.label171, "label171");
            this.label171.Name = "label171";
            // 
            // txtStratQryTensN
            // 
            resources.ApplyResources(this.txtStratQryTensN, "txtStratQryTensN");
            this.txtStratQryTensN.Name = "txtStratQryTensN";
            this.txtStratQryTensN.ReadOnly = true;
            // 
            // label170
            // 
            resources.ApplyResources(this.label170, "label170");
            this.label170.Name = "label170";
            // 
            // txtStratQryActSeup
            // 
            resources.ApplyResources(this.txtStratQryActSeup, "txtStratQryActSeup");
            this.txtStratQryActSeup.Name = "txtStratQryActSeup";
            this.txtStratQryActSeup.ReadOnly = true;
            // 
            // label169
            // 
            resources.ApplyResources(this.label169, "label169");
            this.label169.Name = "label169";
            // 
            // txtStratQryVerLib
            // 
            resources.ApplyResources(this.txtStratQryVerLib, "txtStratQryVerLib");
            this.txtStratQryVerLib.Name = "txtStratQryVerLib";
            this.txtStratQryVerLib.ReadOnly = true;
            // 
            // label168
            // 
            resources.ApplyResources(this.label168, "label168");
            this.label168.Name = "label168";
            // 
            // lblSig60DataReceuved
            // 
            resources.ApplyResources(this.lblSig60DataReceuved, "lblSig60DataReceuved");
            this.lblSig60DataReceuved.Name = "lblSig60DataReceuved";
            // 
            // lblSig60DataSent
            // 
            resources.ApplyResources(this.lblSig60DataSent, "lblSig60DataSent");
            this.lblSig60DataSent.Name = "lblSig60DataSent";
            // 
            // txtStratDataGridRx
            // 
            resources.ApplyResources(this.txtStratDataGridRx, "txtStratDataGridRx");
            this.txtStratDataGridRx.Name = "txtStratDataGridRx";
            // 
            // txtStratDataGridTx
            // 
            resources.ApplyResources(this.txtStratDataGridTx, "txtStratDataGridTx");
            this.txtStratDataGridTx.Name = "txtStratDataGridTx";
            // 
            // grbStratComandiTest
            // 
            this.grbStratComandiTest.BackColor = System.Drawing.Color.White;
            this.grbStratComandiTest.Controls.Add(this.btnStratCallSIS);
            this.grbStratComandiTest.Controls.Add(this.btnStratCallAv);
            this.grbStratComandiTest.Controls.Add(this.btnStratSetDischarge);
            this.grbStratComandiTest.Controls.Add(this.btnStratCallIS);
            this.grbStratComandiTest.Controls.Add(this.btnStratSetCharge);
            this.grbStratComandiTest.Controls.Add(this.btnStratQuery);
            this.grbStratComandiTest.Controls.Add(this.btnStratTestERR);
            this.grbStratComandiTest.Controls.Add(this.btnStratTest02);
            this.grbStratComandiTest.Controls.Add(this.btnStratTest01);
            resources.ApplyResources(this.grbStratComandiTest, "grbStratComandiTest");
            this.grbStratComandiTest.Name = "grbStratComandiTest";
            this.grbStratComandiTest.TabStop = false;
            // 
            // btnStratCallSIS
            // 
            resources.ApplyResources(this.btnStratCallSIS, "btnStratCallSIS");
            this.btnStratCallSIS.Name = "btnStratCallSIS";
            this.btnStratCallSIS.UseVisualStyleBackColor = true;
            // 
            // btnStratCallAv
            // 
            resources.ApplyResources(this.btnStratCallAv, "btnStratCallAv");
            this.btnStratCallAv.Name = "btnStratCallAv";
            this.btnStratCallAv.UseVisualStyleBackColor = true;
            this.btnStratCallAv.Click += new System.EventHandler(this.btnStratCallAv_Click);
            // 
            // btnStratSetDischarge
            // 
            resources.ApplyResources(this.btnStratSetDischarge, "btnStratSetDischarge");
            this.btnStratSetDischarge.Name = "btnStratSetDischarge";
            this.btnStratSetDischarge.UseVisualStyleBackColor = true;
            // 
            // btnStratCallIS
            // 
            resources.ApplyResources(this.btnStratCallIS, "btnStratCallIS");
            this.btnStratCallIS.Name = "btnStratCallIS";
            this.btnStratCallIS.UseVisualStyleBackColor = true;
            // 
            // btnStratSetCharge
            // 
            resources.ApplyResources(this.btnStratSetCharge, "btnStratSetCharge");
            this.btnStratSetCharge.Name = "btnStratSetCharge";
            this.btnStratSetCharge.UseVisualStyleBackColor = true;
            this.btnStratSetCharge.Click += new System.EventHandler(this.btnStratSetCharge_Click);
            // 
            // btnStratQuery
            // 
            resources.ApplyResources(this.btnStratQuery, "btnStratQuery");
            this.btnStratQuery.Name = "btnStratQuery";
            this.btnStratQuery.UseVisualStyleBackColor = true;
            this.btnStratQuery.Click += new System.EventHandler(this.btnStratQuery_Click);
            // 
            // btnStratTestERR
            // 
            resources.ApplyResources(this.btnStratTestERR, "btnStratTestERR");
            this.btnStratTestERR.Name = "btnStratTestERR";
            this.btnStratTestERR.UseVisualStyleBackColor = true;
            this.btnStratTestERR.Click += new System.EventHandler(this.btnStratTestERR_Click);
            // 
            // btnStratTest02
            // 
            resources.ApplyResources(this.btnStratTest02, "btnStratTest02");
            this.btnStratTest02.Name = "btnStratTest02";
            this.btnStratTest02.UseVisualStyleBackColor = true;
            this.btnStratTest02.Click += new System.EventHandler(this.btnStratTest02_Click);
            // 
            // btnStratTest01
            // 
            resources.ApplyResources(this.btnStratTest01, "btnStratTest01");
            this.btnStratTest01.Name = "btnStratTest01";
            this.btnStratTest01.UseVisualStyleBackColor = true;
            this.btnStratTest01.Click += new System.EventHandler(this.btnStratTest01_Click);
            // 
            // tbpFirmware
            // 
            this.tbpFirmware.BackColor = System.Drawing.Color.LightYellow;
            this.tbpFirmware.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tbpFirmware, "tbpFirmware");
            this.tbpFirmware.Controls.Add(this.grbFwAttivazioneArea);
            this.tbpFirmware.Controls.Add(this.grbFWPreparaFile);
            this.tbpFirmware.Controls.Add(this.grbFWArea2);
            this.tbpFirmware.Controls.Add(this.GrbFWArea1);
            this.tbpFirmware.Controls.Add(this.grbFWAggiornamento);
            this.tbpFirmware.Controls.Add(this.grbStatoFirmware);
            this.tbpFirmware.Name = "tbpFirmware";
            // 
            // grbFwAttivazioneArea
            // 
            this.grbFwAttivazioneArea.BackColor = System.Drawing.Color.White;
            this.grbFwAttivazioneArea.Controls.Add(this.btnFwSwitchApp);
            this.grbFwAttivazioneArea.Controls.Add(this.btnFwSwitchArea2);
            this.grbFwAttivazioneArea.Controls.Add(this.btnFwSwitchArea1);
            this.grbFwAttivazioneArea.Controls.Add(this.btnFwSwitchBL);
            resources.ApplyResources(this.grbFwAttivazioneArea, "grbFwAttivazioneArea");
            this.grbFwAttivazioneArea.Name = "grbFwAttivazioneArea";
            this.grbFwAttivazioneArea.TabStop = false;
            // 
            // btnFwSwitchApp
            // 
            resources.ApplyResources(this.btnFwSwitchApp, "btnFwSwitchApp");
            this.btnFwSwitchApp.Name = "btnFwSwitchApp";
            this.btnFwSwitchApp.UseVisualStyleBackColor = true;
            this.btnFwSwitchApp.Click += new System.EventHandler(this.btnFwSwitchApp_Click);
            // 
            // btnFwSwitchArea2
            // 
            resources.ApplyResources(this.btnFwSwitchArea2, "btnFwSwitchArea2");
            this.btnFwSwitchArea2.Name = "btnFwSwitchArea2";
            this.btnFwSwitchArea2.UseVisualStyleBackColor = true;
            this.btnFwSwitchArea2.Click += new System.EventHandler(this.btnFwSwitchArea2_Click);
            // 
            // btnFwSwitchArea1
            // 
            resources.ApplyResources(this.btnFwSwitchArea1, "btnFwSwitchArea1");
            this.btnFwSwitchArea1.Name = "btnFwSwitchArea1";
            this.btnFwSwitchArea1.UseVisualStyleBackColor = true;
            this.btnFwSwitchArea1.Click += new System.EventHandler(this.btnFwSwitchArea1_Click);
            // 
            // btnFwSwitchBL
            // 
            resources.ApplyResources(this.btnFwSwitchBL, "btnFwSwitchBL");
            this.btnFwSwitchBL.Name = "btnFwSwitchBL";
            this.btnFwSwitchBL.UseVisualStyleBackColor = true;
            this.btnFwSwitchBL.Click += new System.EventHandler(this.btnFwSwitchBL_Click);
            // 
            // grbFWPreparaFile
            // 
            this.grbFWPreparaFile.BackColor = System.Drawing.Color.White;
            this.grbFWPreparaFile.Controls.Add(this.txtFWInFileStruct);
            this.grbFWPreparaFile.Controls.Add(this.lvwFWInFileStruct);
            this.grbFWPreparaFile.Controls.Add(this.txtFwFileCCSa01);
            this.grbFWPreparaFile.Controls.Add(this.txtFwFileCCShex);
            this.grbFWPreparaFile.Controls.Add(this.txtFWLibInFileRev);
            this.grbFWPreparaFile.Controls.Add(this.label256);
            this.grbFWPreparaFile.Controls.Add(this.txtFWInFileRevData);
            this.grbFWPreparaFile.Controls.Add(this.label96);
            this.grbFWPreparaFile.Controls.Add(this.btnFWFileLLFsearch);
            this.grbFWPreparaFile.Controls.Add(this.txtFWFileLLFwr);
            this.grbFWPreparaFile.Controls.Add(this.label92);
            this.grbFWPreparaFile.Controls.Add(this.btnFWFilePubSave);
            this.grbFWPreparaFile.Controls.Add(this.txtFWInFileRev);
            this.grbFWPreparaFile.Controls.Add(this.label94);
            this.grbFWPreparaFile.Controls.Add(this.label95);
            this.grbFWPreparaFile.Controls.Add(this.btnFWFileCCSLoad);
            this.grbFWPreparaFile.Controls.Add(this.btnFWFileCCSsearch);
            this.grbFWPreparaFile.Controls.Add(this.txtFwFileCCS);
            resources.ApplyResources(this.grbFWPreparaFile, "grbFWPreparaFile");
            this.grbFWPreparaFile.Name = "grbFWPreparaFile";
            this.grbFWPreparaFile.TabStop = false;
            // 
            // txtFWInFileStruct
            // 
            resources.ApplyResources(this.txtFWInFileStruct, "txtFWInFileStruct");
            this.txtFWInFileStruct.Name = "txtFWInFileStruct";
            this.txtFWInFileStruct.ReadOnly = true;
            // 
            // lvwFWInFileStruct
            // 
            this.lvwFWInFileStruct.CellEditUseWholeCell = false;
            this.lvwFWInFileStruct.HideSelection = false;
            resources.ApplyResources(this.lvwFWInFileStruct, "lvwFWInFileStruct");
            this.lvwFWInFileStruct.Name = "lvwFWInFileStruct";
            this.lvwFWInFileStruct.ShowGroups = false;
            this.lvwFWInFileStruct.UseCompatibleStateImageBehavior = false;
            this.lvwFWInFileStruct.View = System.Windows.Forms.View.Details;
            this.lvwFWInFileStruct.VirtualMode = true;
            // 
            // txtFwFileCCSa01
            // 
            resources.ApplyResources(this.txtFwFileCCSa01, "txtFwFileCCSa01");
            this.txtFwFileCCSa01.Name = "txtFwFileCCSa01";
            // 
            // txtFwFileCCShex
            // 
            resources.ApplyResources(this.txtFwFileCCShex, "txtFwFileCCShex");
            this.txtFwFileCCShex.Name = "txtFwFileCCShex";
            // 
            // txtFWLibInFileRev
            // 
            resources.ApplyResources(this.txtFWLibInFileRev, "txtFWLibInFileRev");
            this.txtFWLibInFileRev.Name = "txtFWLibInFileRev";
            // 
            // label256
            // 
            resources.ApplyResources(this.label256, "label256");
            this.label256.Name = "label256";
            // 
            // txtFWInFileRevData
            // 
            resources.ApplyResources(this.txtFWInFileRevData, "txtFWInFileRevData");
            this.txtFWInFileRevData.Name = "txtFWInFileRevData";
            this.txtFWInFileRevData.ValidatingType = typeof(System.DateTime);
            // 
            // label96
            // 
            resources.ApplyResources(this.label96, "label96");
            this.label96.Name = "label96";
            // 
            // btnFWFileLLFsearch
            // 
            resources.ApplyResources(this.btnFWFileLLFsearch, "btnFWFileLLFsearch");
            this.btnFWFileLLFsearch.Name = "btnFWFileLLFsearch";
            this.btnFWFileLLFsearch.UseVisualStyleBackColor = true;
            this.btnFWFileLLFsearch.Click += new System.EventHandler(this.btnFWFileSBFsearch_Click);
            // 
            // txtFWFileLLFwr
            // 
            resources.ApplyResources(this.txtFWFileLLFwr, "txtFWFileLLFwr");
            this.txtFWFileLLFwr.Name = "txtFWFileLLFwr";
            // 
            // label92
            // 
            resources.ApplyResources(this.label92, "label92");
            this.label92.Name = "label92";
            // 
            // btnFWFilePubSave
            // 
            resources.ApplyResources(this.btnFWFilePubSave, "btnFWFilePubSave");
            this.btnFWFilePubSave.Name = "btnFWFilePubSave";
            this.btnFWFilePubSave.UseVisualStyleBackColor = true;
            this.btnFWFilePubSave.Click += new System.EventHandler(this.btnFWFilePubSave_Click);
            // 
            // txtFWInFileRev
            // 
            resources.ApplyResources(this.txtFWInFileRev, "txtFWInFileRev");
            this.txtFWInFileRev.Name = "txtFWInFileRev";
            // 
            // label94
            // 
            resources.ApplyResources(this.label94, "label94");
            this.label94.Name = "label94";
            // 
            // label95
            // 
            resources.ApplyResources(this.label95, "label95");
            this.label95.Name = "label95";
            // 
            // btnFWFileCCSLoad
            // 
            resources.ApplyResources(this.btnFWFileCCSLoad, "btnFWFileCCSLoad");
            this.btnFWFileCCSLoad.Name = "btnFWFileCCSLoad";
            this.btnFWFileCCSLoad.UseVisualStyleBackColor = true;
            this.btnFWFileCCSLoad.Click += new System.EventHandler(this.btnFWFileCCSLoad_Click);
            // 
            // btnFWFileCCSsearch
            // 
            resources.ApplyResources(this.btnFWFileCCSsearch, "btnFWFileCCSsearch");
            this.btnFWFileCCSsearch.Name = "btnFWFileCCSsearch";
            this.btnFWFileCCSsearch.UseVisualStyleBackColor = true;
            this.btnFWFileCCSsearch.Click += new System.EventHandler(this.btnFWFileCCSsearch_Click);
            // 
            // txtFwFileCCS
            // 
            resources.ApplyResources(this.txtFwFileCCS, "txtFwFileCCS");
            this.txtFwFileCCS.Name = "txtFwFileCCS";
            // 
            // grbFWArea2
            // 
            this.grbFWArea2.BackColor = System.Drawing.Color.White;
            this.grbFWArea2.Controls.Add(this.label86);
            this.grbFWArea2.Controls.Add(this.txtFwRevA2Size);
            this.grbFWArea2.Controls.Add(this.txtFWRevA2Addr5);
            this.grbFWArea2.Controls.Add(this.label80);
            this.grbFWArea2.Controls.Add(this.label82);
            this.grbFWArea2.Controls.Add(this.txtFWRevA2Addr4);
            this.grbFWArea2.Controls.Add(this.txtFWRevA2Addr3);
            this.grbFWArea2.Controls.Add(this.label83);
            this.grbFWArea2.Controls.Add(this.label84);
            this.grbFWArea2.Controls.Add(this.txtFWRevA2Addr2);
            this.grbFWArea2.Controls.Add(this.txtFWRevA2Addr1);
            this.grbFWArea2.Controls.Add(this.label85);
            this.grbFWArea2.Controls.Add(this.label109);
            this.grbFWArea2.Controls.Add(this.txtFwRevA2RilFw);
            this.grbFWArea2.Controls.Add(this.label87);
            this.grbFWArea2.Controls.Add(this.txtFwRevA2MsgSize);
            this.grbFWArea2.Controls.Add(this.label89);
            this.grbFWArea2.Controls.Add(this.txtFwRevA2State);
            this.grbFWArea2.Controls.Add(this.label90);
            this.grbFWArea2.Controls.Add(this.txtFwRevA2RevFw);
            resources.ApplyResources(this.grbFWArea2, "grbFWArea2");
            this.grbFWArea2.Name = "grbFWArea2";
            this.grbFWArea2.TabStop = false;
            // 
            // label86
            // 
            resources.ApplyResources(this.label86, "label86");
            this.label86.Name = "label86";
            // 
            // txtFwRevA2Size
            // 
            resources.ApplyResources(this.txtFwRevA2Size, "txtFwRevA2Size");
            this.txtFwRevA2Size.Name = "txtFwRevA2Size";
            // 
            // txtFWRevA2Addr5
            // 
            resources.ApplyResources(this.txtFWRevA2Addr5, "txtFWRevA2Addr5");
            this.txtFWRevA2Addr5.Name = "txtFWRevA2Addr5";
            // 
            // label80
            // 
            resources.ApplyResources(this.label80, "label80");
            this.label80.Name = "label80";
            // 
            // label82
            // 
            resources.ApplyResources(this.label82, "label82");
            this.label82.Name = "label82";
            // 
            // txtFWRevA2Addr4
            // 
            resources.ApplyResources(this.txtFWRevA2Addr4, "txtFWRevA2Addr4");
            this.txtFWRevA2Addr4.Name = "txtFWRevA2Addr4";
            // 
            // txtFWRevA2Addr3
            // 
            resources.ApplyResources(this.txtFWRevA2Addr3, "txtFWRevA2Addr3");
            this.txtFWRevA2Addr3.Name = "txtFWRevA2Addr3";
            // 
            // label83
            // 
            resources.ApplyResources(this.label83, "label83");
            this.label83.Name = "label83";
            // 
            // label84
            // 
            resources.ApplyResources(this.label84, "label84");
            this.label84.Name = "label84";
            // 
            // txtFWRevA2Addr2
            // 
            resources.ApplyResources(this.txtFWRevA2Addr2, "txtFWRevA2Addr2");
            this.txtFWRevA2Addr2.Name = "txtFWRevA2Addr2";
            // 
            // txtFWRevA2Addr1
            // 
            resources.ApplyResources(this.txtFWRevA2Addr1, "txtFWRevA2Addr1");
            this.txtFWRevA2Addr1.Name = "txtFWRevA2Addr1";
            // 
            // label85
            // 
            resources.ApplyResources(this.label85, "label85");
            this.label85.Name = "label85";
            // 
            // label109
            // 
            resources.ApplyResources(this.label109, "label109");
            this.label109.Name = "label109";
            // 
            // txtFwRevA2RilFw
            // 
            resources.ApplyResources(this.txtFwRevA2RilFw, "txtFwRevA2RilFw");
            this.txtFwRevA2RilFw.Name = "txtFwRevA2RilFw";
            this.txtFwRevA2RilFw.ReadOnly = true;
            // 
            // label87
            // 
            resources.ApplyResources(this.label87, "label87");
            this.label87.Name = "label87";
            // 
            // txtFwRevA2MsgSize
            // 
            resources.ApplyResources(this.txtFwRevA2MsgSize, "txtFwRevA2MsgSize");
            this.txtFwRevA2MsgSize.Name = "txtFwRevA2MsgSize";
            // 
            // label89
            // 
            resources.ApplyResources(this.label89, "label89");
            this.label89.Name = "label89";
            // 
            // txtFwRevA2State
            // 
            resources.ApplyResources(this.txtFwRevA2State, "txtFwRevA2State");
            this.txtFwRevA2State.Name = "txtFwRevA2State";
            // 
            // label90
            // 
            resources.ApplyResources(this.label90, "label90");
            this.label90.Name = "label90";
            // 
            // txtFwRevA2RevFw
            // 
            this.txtFwRevA2RevFw.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.txtFwRevA2RevFw, "txtFwRevA2RevFw");
            this.txtFwRevA2RevFw.Name = "txtFwRevA2RevFw";
            // 
            // GrbFWArea1
            // 
            this.GrbFWArea1.BackColor = System.Drawing.Color.White;
            this.GrbFWArea1.Controls.Add(this.label97);
            this.GrbFWArea1.Controls.Add(this.txtFwRevA1Size);
            this.GrbFWArea1.Controls.Add(this.label103);
            this.GrbFWArea1.Controls.Add(this.txtFwRevA1RilFw);
            this.GrbFWArea1.Controls.Add(this.txtFWRevA1Addr5);
            this.GrbFWArea1.Controls.Add(this.label81);
            this.GrbFWArea1.Controls.Add(this.label74);
            this.GrbFWArea1.Controls.Add(this.txtFWRevA1Addr4);
            this.GrbFWArea1.Controls.Add(this.txtFWRevA1Addr3);
            this.GrbFWArea1.Controls.Add(this.label75);
            this.GrbFWArea1.Controls.Add(this.label79);
            this.GrbFWArea1.Controls.Add(this.label78);
            this.GrbFWArea1.Controls.Add(this.txtFWRevA1Addr2);
            this.GrbFWArea1.Controls.Add(this.txtFWRevA1Addr1);
            this.GrbFWArea1.Controls.Add(this.label77);
            this.GrbFWArea1.Controls.Add(this.txtFwRevA1MsgSize);
            this.GrbFWArea1.Controls.Add(this.label91);
            this.GrbFWArea1.Controls.Add(this.txtFwRevA1State);
            this.GrbFWArea1.Controls.Add(this.label93);
            this.GrbFWArea1.Controls.Add(this.txtFwRevA1RevFw);
            resources.ApplyResources(this.GrbFWArea1, "GrbFWArea1");
            this.GrbFWArea1.Name = "GrbFWArea1";
            this.GrbFWArea1.TabStop = false;
            // 
            // label97
            // 
            resources.ApplyResources(this.label97, "label97");
            this.label97.Name = "label97";
            // 
            // txtFwRevA1Size
            // 
            resources.ApplyResources(this.txtFwRevA1Size, "txtFwRevA1Size");
            this.txtFwRevA1Size.Name = "txtFwRevA1Size";
            // 
            // label103
            // 
            resources.ApplyResources(this.label103, "label103");
            this.label103.Name = "label103";
            this.label103.Click += new System.EventHandler(this.label103_Click);
            // 
            // txtFwRevA1RilFw
            // 
            resources.ApplyResources(this.txtFwRevA1RilFw, "txtFwRevA1RilFw");
            this.txtFwRevA1RilFw.Name = "txtFwRevA1RilFw";
            this.txtFwRevA1RilFw.ReadOnly = true;
            // 
            // txtFWRevA1Addr5
            // 
            resources.ApplyResources(this.txtFWRevA1Addr5, "txtFWRevA1Addr5");
            this.txtFWRevA1Addr5.Name = "txtFWRevA1Addr5";
            // 
            // label81
            // 
            resources.ApplyResources(this.label81, "label81");
            this.label81.Name = "label81";
            // 
            // label74
            // 
            resources.ApplyResources(this.label74, "label74");
            this.label74.Name = "label74";
            // 
            // txtFWRevA1Addr4
            // 
            resources.ApplyResources(this.txtFWRevA1Addr4, "txtFWRevA1Addr4");
            this.txtFWRevA1Addr4.Name = "txtFWRevA1Addr4";
            // 
            // txtFWRevA1Addr3
            // 
            resources.ApplyResources(this.txtFWRevA1Addr3, "txtFWRevA1Addr3");
            this.txtFWRevA1Addr3.Name = "txtFWRevA1Addr3";
            // 
            // label75
            // 
            resources.ApplyResources(this.label75, "label75");
            this.label75.Name = "label75";
            // 
            // label79
            // 
            resources.ApplyResources(this.label79, "label79");
            this.label79.Name = "label79";
            // 
            // label78
            // 
            resources.ApplyResources(this.label78, "label78");
            this.label78.Name = "label78";
            // 
            // txtFWRevA1Addr2
            // 
            resources.ApplyResources(this.txtFWRevA1Addr2, "txtFWRevA1Addr2");
            this.txtFWRevA1Addr2.Name = "txtFWRevA1Addr2";
            // 
            // txtFWRevA1Addr1
            // 
            resources.ApplyResources(this.txtFWRevA1Addr1, "txtFWRevA1Addr1");
            this.txtFWRevA1Addr1.Name = "txtFWRevA1Addr1";
            // 
            // label77
            // 
            resources.ApplyResources(this.label77, "label77");
            this.label77.Name = "label77";
            // 
            // txtFwRevA1MsgSize
            // 
            resources.ApplyResources(this.txtFwRevA1MsgSize, "txtFwRevA1MsgSize");
            this.txtFwRevA1MsgSize.Name = "txtFwRevA1MsgSize";
            // 
            // label91
            // 
            resources.ApplyResources(this.label91, "label91");
            this.label91.Name = "label91";
            // 
            // txtFwRevA1State
            // 
            resources.ApplyResources(this.txtFwRevA1State, "txtFwRevA1State");
            this.txtFwRevA1State.Name = "txtFwRevA1State";
            // 
            // label93
            // 
            resources.ApplyResources(this.label93, "label93");
            this.label93.Name = "label93";
            // 
            // txtFwRevA1RevFw
            // 
            resources.ApplyResources(this.txtFwRevA1RevFw, "txtFwRevA1RevFw");
            this.txtFwRevA1RevFw.Name = "txtFwRevA1RevFw";
            this.txtFwRevA1RevFw.ReadOnly = true;
            // 
            // grbFWAggiornamento
            // 
            this.grbFWAggiornamento.BackColor = System.Drawing.Color.White;
            this.grbFWAggiornamento.Controls.Add(this.cmbFWSBFArea);
            this.grbFWAggiornamento.Controls.Add(this.flwFWFileLLFStruct);
            this.grbFWAggiornamento.Controls.Add(this.txtFWInLLFDispRev);
            this.grbFWAggiornamento.Controls.Add(this.txtFWInSBFDtRev);
            this.grbFWAggiornamento.Controls.Add(this.label110);
            this.grbFWAggiornamento.Controls.Add(this.btnFWLanciaTrasmissione);
            this.grbFWAggiornamento.Controls.Add(this.txtFWInLLFEsito);
            this.grbFWAggiornamento.Controls.Add(this.label108);
            this.grbFWAggiornamento.Controls.Add(this.label114);
            this.grbFWAggiornamento.Controls.Add(this.btnFWPreparaTrasmissione);
            this.grbFWAggiornamento.Controls.Add(this.txtFWInLLFRev);
            this.grbFWAggiornamento.Controls.Add(this.label115);
            this.grbFWAggiornamento.Controls.Add(this.label116);
            this.grbFWAggiornamento.Controls.Add(this.btnFWFileLLFLoad);
            this.grbFWAggiornamento.Controls.Add(this.btnFWFileLLFReadSearch);
            this.grbFWAggiornamento.Controls.Add(this.txtFWFileSBFrd);
            resources.ApplyResources(this.grbFWAggiornamento, "grbFWAggiornamento");
            this.grbFWAggiornamento.Name = "grbFWAggiornamento";
            this.grbFWAggiornamento.TabStop = false;
            // 
            // cmbFWSBFArea
            // 
            resources.ApplyResources(this.cmbFWSBFArea, "cmbFWSBFArea");
            this.cmbFWSBFArea.FormattingEnabled = true;
            this.cmbFWSBFArea.Items.AddRange(new object[] {
            resources.GetString("cmbFWSBFArea.Items"),
            resources.GetString("cmbFWSBFArea.Items1")});
            this.cmbFWSBFArea.Name = "cmbFWSBFArea";
            // 
            // flwFWFileLLFStruct
            // 
            this.flwFWFileLLFStruct.CellEditUseWholeCell = false;
            this.flwFWFileLLFStruct.HideSelection = false;
            resources.ApplyResources(this.flwFWFileLLFStruct, "flwFWFileLLFStruct");
            this.flwFWFileLLFStruct.Name = "flwFWFileLLFStruct";
            this.flwFWFileLLFStruct.ShowGroups = false;
            this.flwFWFileLLFStruct.UseCompatibleStateImageBehavior = false;
            this.flwFWFileLLFStruct.View = System.Windows.Forms.View.Details;
            this.flwFWFileLLFStruct.VirtualMode = true;
            // 
            // txtFWInLLFDispRev
            // 
            resources.ApplyResources(this.txtFWInLLFDispRev, "txtFWInLLFDispRev");
            this.txtFWInLLFDispRev.Name = "txtFWInLLFDispRev";
            this.txtFWInLLFDispRev.ReadOnly = true;
            // 
            // txtFWInSBFDtRev
            // 
            resources.ApplyResources(this.txtFWInSBFDtRev, "txtFWInSBFDtRev");
            this.txtFWInSBFDtRev.Name = "txtFWInSBFDtRev";
            this.txtFWInSBFDtRev.ReadOnly = true;
            // 
            // label110
            // 
            resources.ApplyResources(this.label110, "label110");
            this.label110.Name = "label110";
            // 
            // btnFWLanciaTrasmissione
            // 
            resources.ApplyResources(this.btnFWLanciaTrasmissione, "btnFWLanciaTrasmissione");
            this.btnFWLanciaTrasmissione.Name = "btnFWLanciaTrasmissione";
            this.btnFWLanciaTrasmissione.UseVisualStyleBackColor = true;
            this.btnFWLanciaTrasmissione.Click += new System.EventHandler(this.btnFWLanciaTrasmissione_Click);
            // 
            // txtFWInLLFEsito
            // 
            resources.ApplyResources(this.txtFWInLLFEsito, "txtFWInLLFEsito");
            this.txtFWInLLFEsito.Name = "txtFWInLLFEsito";
            // 
            // label108
            // 
            resources.ApplyResources(this.label108, "label108");
            this.label108.Name = "label108";
            // 
            // label114
            // 
            resources.ApplyResources(this.label114, "label114");
            this.label114.Name = "label114";
            // 
            // btnFWPreparaTrasmissione
            // 
            resources.ApplyResources(this.btnFWPreparaTrasmissione, "btnFWPreparaTrasmissione");
            this.btnFWPreparaTrasmissione.Name = "btnFWPreparaTrasmissione";
            this.btnFWPreparaTrasmissione.UseVisualStyleBackColor = true;
            this.btnFWPreparaTrasmissione.Click += new System.EventHandler(this.btnFWPreparaTrasmissione_Click);
            // 
            // txtFWInLLFRev
            // 
            resources.ApplyResources(this.txtFWInLLFRev, "txtFWInLLFRev");
            this.txtFWInLLFRev.Name = "txtFWInLLFRev";
            this.txtFWInLLFRev.ReadOnly = true;
            // 
            // label115
            // 
            resources.ApplyResources(this.label115, "label115");
            this.label115.Name = "label115";
            // 
            // label116
            // 
            resources.ApplyResources(this.label116, "label116");
            this.label116.Name = "label116";
            // 
            // btnFWFileLLFLoad
            // 
            resources.ApplyResources(this.btnFWFileLLFLoad, "btnFWFileLLFLoad");
            this.btnFWFileLLFLoad.Name = "btnFWFileLLFLoad";
            this.btnFWFileLLFLoad.UseVisualStyleBackColor = true;
            this.btnFWFileLLFLoad.Click += new System.EventHandler(this.btnFWFileSBFLoad_Click);
            // 
            // btnFWFileLLFReadSearch
            // 
            resources.ApplyResources(this.btnFWFileLLFReadSearch, "btnFWFileLLFReadSearch");
            this.btnFWFileLLFReadSearch.Name = "btnFWFileLLFReadSearch";
            this.btnFWFileLLFReadSearch.UseVisualStyleBackColor = true;
            this.btnFWFileLLFReadSearch.Click += new System.EventHandler(this.btnFWFileLLFReadSearch_Click);
            // 
            // txtFWFileSBFrd
            // 
            resources.ApplyResources(this.txtFWFileSBFrd, "txtFWFileSBFrd");
            this.txtFWFileSBFrd.Name = "txtFWFileSBFrd";
            // 
            // grbStatoFirmware
            // 
            this.grbStatoFirmware.BackColor = System.Drawing.Color.White;
            this.grbStatoFirmware.Controls.Add(this.label186);
            this.grbStatoFirmware.Controls.Add(this.txtFwRevDisplay);
            this.grbStatoFirmware.Controls.Add(this.grbFWDettStato);
            this.grbStatoFirmware.Controls.Add(this.txtFwStatoSA2);
            this.grbStatoFirmware.Controls.Add(this.label117);
            this.grbStatoFirmware.Controls.Add(this.txtFwStatoSA1);
            this.grbStatoFirmware.Controls.Add(this.label118);
            this.grbStatoFirmware.Controls.Add(this.btnFwCaricaStato);
            this.grbStatoFirmware.Controls.Add(this.txtFwStatoHA2);
            this.grbStatoFirmware.Controls.Add(this.label119);
            this.grbStatoFirmware.Controls.Add(this.txtFwStatoHA1);
            this.grbStatoFirmware.Controls.Add(this.label120);
            this.grbStatoFirmware.Controls.Add(this.txtFwStatoMicro);
            this.grbStatoFirmware.Controls.Add(this.label121);
            this.grbStatoFirmware.Controls.Add(this.txtFwAreaTestata);
            this.grbStatoFirmware.Controls.Add(this.label122);
            this.grbStatoFirmware.Controls.Add(this.txtFwRevFirmware);
            this.grbStatoFirmware.Controls.Add(this.label123);
            this.grbStatoFirmware.Controls.Add(this.txtFwRevBootloader);
            this.grbStatoFirmware.Controls.Add(this.label124);
            resources.ApplyResources(this.grbStatoFirmware, "grbStatoFirmware");
            this.grbStatoFirmware.Name = "grbStatoFirmware";
            this.grbStatoFirmware.TabStop = false;
            // 
            // label186
            // 
            resources.ApplyResources(this.label186, "label186");
            this.label186.Name = "label186";
            // 
            // txtFwRevDisplay
            // 
            resources.ApplyResources(this.txtFwRevDisplay, "txtFwRevDisplay");
            this.txtFwRevDisplay.Name = "txtFwRevDisplay";
            // 
            // grbFWDettStato
            // 
            resources.ApplyResources(this.grbFWDettStato, "grbFWDettStato");
            this.grbFWDettStato.Name = "grbFWDettStato";
            this.grbFWDettStato.TabStop = false;
            // 
            // txtFwStatoSA2
            // 
            resources.ApplyResources(this.txtFwStatoSA2, "txtFwStatoSA2");
            this.txtFwStatoSA2.Name = "txtFwStatoSA2";
            this.txtFwStatoSA2.DoubleClick += new System.EventHandler(this.txtFwStatoSA2_DoubleClick);
            // 
            // label117
            // 
            resources.ApplyResources(this.label117, "label117");
            this.label117.Name = "label117";
            // 
            // txtFwStatoSA1
            // 
            resources.ApplyResources(this.txtFwStatoSA1, "txtFwStatoSA1");
            this.txtFwStatoSA1.Name = "txtFwStatoSA1";
            // 
            // label118
            // 
            resources.ApplyResources(this.label118, "label118");
            this.label118.Name = "label118";
            // 
            // btnFwCaricaStato
            // 
            resources.ApplyResources(this.btnFwCaricaStato, "btnFwCaricaStato");
            this.btnFwCaricaStato.Name = "btnFwCaricaStato";
            this.btnFwCaricaStato.UseVisualStyleBackColor = true;
            this.btnFwCaricaStato.Click += new System.EventHandler(this.btnFwCaricaStato_Click);
            // 
            // txtFwStatoHA2
            // 
            resources.ApplyResources(this.txtFwStatoHA2, "txtFwStatoHA2");
            this.txtFwStatoHA2.Name = "txtFwStatoHA2";
            // 
            // label119
            // 
            resources.ApplyResources(this.label119, "label119");
            this.label119.Name = "label119";
            // 
            // txtFwStatoHA1
            // 
            resources.ApplyResources(this.txtFwStatoHA1, "txtFwStatoHA1");
            this.txtFwStatoHA1.Name = "txtFwStatoHA1";
            // 
            // label120
            // 
            resources.ApplyResources(this.label120, "label120");
            this.label120.Name = "label120";
            // 
            // txtFwStatoMicro
            // 
            resources.ApplyResources(this.txtFwStatoMicro, "txtFwStatoMicro");
            this.txtFwStatoMicro.Name = "txtFwStatoMicro";
            // 
            // label121
            // 
            resources.ApplyResources(this.label121, "label121");
            this.label121.Name = "label121";
            // 
            // txtFwAreaTestata
            // 
            resources.ApplyResources(this.txtFwAreaTestata, "txtFwAreaTestata");
            this.txtFwAreaTestata.Name = "txtFwAreaTestata";
            // 
            // label122
            // 
            resources.ApplyResources(this.label122, "label122");
            this.label122.Name = "label122";
            // 
            // txtFwRevFirmware
            // 
            resources.ApplyResources(this.txtFwRevFirmware, "txtFwRevFirmware");
            this.txtFwRevFirmware.Name = "txtFwRevFirmware";
            // 
            // label123
            // 
            resources.ApplyResources(this.label123, "label123");
            this.label123.Name = "label123";
            // 
            // txtFwRevBootloader
            // 
            resources.ApplyResources(this.txtFwRevBootloader, "txtFwRevBootloader");
            this.txtFwRevBootloader.Name = "txtFwRevBootloader";
            // 
            // label124
            // 
            resources.ApplyResources(this.label124, "label124");
            this.label124.Name = "label124";
            // 
            // tabMemRead
            // 
            this.tabMemRead.BackColor = System.Drawing.Color.LightYellow;
            this.tabMemRead.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tabMemRead, "tabMemRead");
            this.tabMemRead.Controls.Add(this.grbMemTestLetture);
            this.tabMemRead.Controls.Add(this.grbMemAzzeraLogger);
            this.tabMemRead.Controls.Add(this.grbMemCaricaLogger);
            this.tabMemRead.Controls.Add(this.grbMemSalvaLogger);
            this.tabMemRead.Controls.Add(this.grbMemCancFisica);
            this.tabMemRead.Controls.Add(this.groupBox9);
            this.tabMemRead.Controls.Add(this.txtMemDataGrid);
            this.tabMemRead.Controls.Add(this.grbMemScrittura);
            this.tabMemRead.Controls.Add(this.grbMemCancellazione);
            this.tabMemRead.Controls.Add(this.grbMemLettura);
            this.tabMemRead.Name = "tabMemRead";
            // 
            // grbMemTestLetture
            // 
            this.grbMemTestLetture.BackColor = System.Drawing.Color.White;
            this.grbMemTestLetture.Controls.Add(this.chkMemTestAddrRND);
            this.grbMemTestLetture.Controls.Add(this.chkMemTestLenRND);
            this.grbMemTestLetture.Controls.Add(this.label51);
            this.grbMemTestLetture.Controls.Add(this.txtMemNumTestERR);
            this.grbMemTestLetture.Controls.Add(this.label47);
            this.grbMemTestLetture.Controls.Add(this.txtMemNumTestOK);
            this.grbMemTestLetture.Controls.Add(this.label46);
            this.grbMemTestLetture.Controls.Add(this.txtMemNumTest);
            this.grbMemTestLetture.Controls.Add(this.btnMemTestExac);
            resources.ApplyResources(this.grbMemTestLetture, "grbMemTestLetture");
            this.grbMemTestLetture.Name = "grbMemTestLetture";
            this.grbMemTestLetture.TabStop = false;
            // 
            // chkMemTestAddrRND
            // 
            resources.ApplyResources(this.chkMemTestAddrRND, "chkMemTestAddrRND");
            this.chkMemTestAddrRND.Name = "chkMemTestAddrRND";
            this.chkMemTestAddrRND.UseVisualStyleBackColor = true;
            // 
            // chkMemTestLenRND
            // 
            resources.ApplyResources(this.chkMemTestLenRND, "chkMemTestLenRND");
            this.chkMemTestLenRND.Name = "chkMemTestLenRND";
            this.chkMemTestLenRND.UseVisualStyleBackColor = true;
            // 
            // label51
            // 
            resources.ApplyResources(this.label51, "label51");
            this.label51.Name = "label51";
            // 
            // txtMemNumTestERR
            // 
            resources.ApplyResources(this.txtMemNumTestERR, "txtMemNumTestERR");
            this.txtMemNumTestERR.Name = "txtMemNumTestERR";
            // 
            // label47
            // 
            resources.ApplyResources(this.label47, "label47");
            this.label47.Name = "label47";
            // 
            // txtMemNumTestOK
            // 
            resources.ApplyResources(this.txtMemNumTestOK, "txtMemNumTestOK");
            this.txtMemNumTestOK.Name = "txtMemNumTestOK";
            // 
            // label46
            // 
            resources.ApplyResources(this.label46, "label46");
            this.label46.Name = "label46";
            // 
            // txtMemNumTest
            // 
            resources.ApplyResources(this.txtMemNumTest, "txtMemNumTest");
            this.txtMemNumTest.Name = "txtMemNumTest";
            // 
            // btnMemTestExac
            // 
            resources.ApplyResources(this.btnMemTestExac, "btnMemTestExac");
            this.btnMemTestExac.Name = "btnMemTestExac";
            this.btnMemTestExac.UseVisualStyleBackColor = true;
            this.btnMemTestExac.Click += new System.EventHandler(this.btnMemTestExac_Click);
            // 
            // grbMemAzzeraLogger
            // 
            this.grbMemAzzeraLogger.BackColor = System.Drawing.Color.White;
            this.grbMemAzzeraLogger.Controls.Add(this.chkMemCReboot);
            this.grbMemAzzeraLogger.Controls.Add(this.btnMemClearLogExec);
            this.grbMemAzzeraLogger.Controls.Add(this.chkMemCResetCicli);
            this.grbMemAzzeraLogger.Controls.Add(this.chkMemCResetCont);
            this.grbMemAzzeraLogger.Controls.Add(this.chkMemCResetProg);
            this.grbMemAzzeraLogger.Controls.Add(this.label41);
            resources.ApplyResources(this.grbMemAzzeraLogger, "grbMemAzzeraLogger");
            this.grbMemAzzeraLogger.Name = "grbMemAzzeraLogger";
            this.grbMemAzzeraLogger.TabStop = false;
            // 
            // chkMemCReboot
            // 
            resources.ApplyResources(this.chkMemCReboot, "chkMemCReboot");
            this.chkMemCReboot.Checked = true;
            this.chkMemCReboot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemCReboot.ForeColor = System.Drawing.Color.Red;
            this.chkMemCReboot.Name = "chkMemCReboot";
            this.chkMemCReboot.UseVisualStyleBackColor = true;
            // 
            // btnMemClearLogExec
            // 
            resources.ApplyResources(this.btnMemClearLogExec, "btnMemClearLogExec");
            this.btnMemClearLogExec.Name = "btnMemClearLogExec";
            this.btnMemClearLogExec.UseVisualStyleBackColor = true;
            this.btnMemClearLogExec.Click += new System.EventHandler(this.btnMemClearLogExec_Click);
            // 
            // chkMemCResetCicli
            // 
            resources.ApplyResources(this.chkMemCResetCicli, "chkMemCResetCicli");
            this.chkMemCResetCicli.Checked = true;
            this.chkMemCResetCicli.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemCResetCicli.Name = "chkMemCResetCicli";
            this.chkMemCResetCicli.UseVisualStyleBackColor = true;
            // 
            // chkMemCResetCont
            // 
            resources.ApplyResources(this.chkMemCResetCont, "chkMemCResetCont");
            this.chkMemCResetCont.Checked = true;
            this.chkMemCResetCont.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemCResetCont.Name = "chkMemCResetCont";
            this.chkMemCResetCont.UseVisualStyleBackColor = true;
            // 
            // chkMemCResetProg
            // 
            resources.ApplyResources(this.chkMemCResetProg, "chkMemCResetProg");
            this.chkMemCResetProg.Checked = true;
            this.chkMemCResetProg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemCResetProg.Name = "chkMemCResetProg";
            this.chkMemCResetProg.UseVisualStyleBackColor = true;
            // 
            // label41
            // 
            resources.ApplyResources(this.label41, "label41");
            this.label41.Name = "label41";
            // 
            // grbMemCaricaLogger
            // 
            this.grbMemCaricaLogger.BackColor = System.Drawing.Color.White;
            this.grbMemCaricaLogger.Controls.Add(this.btnMemRewriteExec);
            this.grbMemCaricaLogger.Controls.Add(this.chkMemDevWCycle);
            this.grbMemCaricaLogger.Controls.Add(this.chkMemDevWCount);
            this.grbMemCaricaLogger.Controls.Add(this.chkMemDevWProg);
            this.grbMemCaricaLogger.Controls.Add(this.label40);
            this.grbMemCaricaLogger.Controls.Add(this.button5);
            this.grbMemCaricaLogger.Controls.Add(this.txtMemFileRead);
            resources.ApplyResources(this.grbMemCaricaLogger, "grbMemCaricaLogger");
            this.grbMemCaricaLogger.Name = "grbMemCaricaLogger";
            this.grbMemCaricaLogger.TabStop = false;
            // 
            // btnMemRewriteExec
            // 
            resources.ApplyResources(this.btnMemRewriteExec, "btnMemRewriteExec");
            this.btnMemRewriteExec.Name = "btnMemRewriteExec";
            this.btnMemRewriteExec.UseVisualStyleBackColor = true;
            this.btnMemRewriteExec.Click += new System.EventHandler(this.btnMemRewriteExec_Click);
            // 
            // chkMemDevWCycle
            // 
            resources.ApplyResources(this.chkMemDevWCycle, "chkMemDevWCycle");
            this.chkMemDevWCycle.Name = "chkMemDevWCycle";
            this.chkMemDevWCycle.UseVisualStyleBackColor = true;
            // 
            // chkMemDevWCount
            // 
            resources.ApplyResources(this.chkMemDevWCount, "chkMemDevWCount");
            this.chkMemDevWCount.Name = "chkMemDevWCount";
            this.chkMemDevWCount.UseVisualStyleBackColor = true;
            // 
            // chkMemDevWProg
            // 
            resources.ApplyResources(this.chkMemDevWProg, "chkMemDevWProg");
            this.chkMemDevWProg.Name = "chkMemDevWProg";
            this.chkMemDevWProg.UseVisualStyleBackColor = true;
            // 
            // label40
            // 
            resources.ApplyResources(this.label40, "label40");
            this.label40.Name = "label40";
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // txtMemFileRead
            // 
            resources.ApplyResources(this.txtMemFileRead, "txtMemFileRead");
            this.txtMemFileRead.Name = "txtMemFileRead";
            // 
            // grbMemSalvaLogger
            // 
            this.grbMemSalvaLogger.BackColor = System.Drawing.Color.White;
            this.grbMemSalvaLogger.Controls.Add(this.btnMemSaveExec);
            this.grbMemSalvaLogger.Controls.Add(this.chkMemFsCycle);
            this.grbMemSalvaLogger.Controls.Add(this.chkMemFsCount);
            this.grbMemSalvaLogger.Controls.Add(this.chkMemFsProgr);
            this.grbMemSalvaLogger.Controls.Add(this.label37);
            this.grbMemSalvaLogger.Controls.Add(this.btnMemFileSaveSRC);
            this.grbMemSalvaLogger.Controls.Add(this.txtMemFileSave);
            resources.ApplyResources(this.grbMemSalvaLogger, "grbMemSalvaLogger");
            this.grbMemSalvaLogger.Name = "grbMemSalvaLogger";
            this.grbMemSalvaLogger.TabStop = false;
            // 
            // btnMemSaveExec
            // 
            resources.ApplyResources(this.btnMemSaveExec, "btnMemSaveExec");
            this.btnMemSaveExec.Name = "btnMemSaveExec";
            this.btnMemSaveExec.UseVisualStyleBackColor = true;
            this.btnMemSaveExec.Click += new System.EventHandler(this.btnMemSaveExec_Click);
            // 
            // chkMemFsCycle
            // 
            resources.ApplyResources(this.chkMemFsCycle, "chkMemFsCycle");
            this.chkMemFsCycle.Name = "chkMemFsCycle";
            this.chkMemFsCycle.UseVisualStyleBackColor = true;
            // 
            // chkMemFsCount
            // 
            resources.ApplyResources(this.chkMemFsCount, "chkMemFsCount");
            this.chkMemFsCount.Name = "chkMemFsCount";
            this.chkMemFsCount.UseVisualStyleBackColor = true;
            // 
            // chkMemFsProgr
            // 
            resources.ApplyResources(this.chkMemFsProgr, "chkMemFsProgr");
            this.chkMemFsProgr.Name = "chkMemFsProgr";
            this.chkMemFsProgr.UseVisualStyleBackColor = true;
            // 
            // label37
            // 
            resources.ApplyResources(this.label37, "label37");
            this.label37.Name = "label37";
            // 
            // btnMemFileSaveSRC
            // 
            resources.ApplyResources(this.btnMemFileSaveSRC, "btnMemFileSaveSRC");
            this.btnMemFileSaveSRC.Name = "btnMemFileSaveSRC";
            this.btnMemFileSaveSRC.UseVisualStyleBackColor = true;
            this.btnMemFileSaveSRC.Click += new System.EventHandler(this.btnMemFileSaveSRC_Click);
            // 
            // txtMemFileSave
            // 
            resources.ApplyResources(this.txtMemFileSave, "txtMemFileSave");
            this.txtMemFileSave.Name = "txtMemFileSave";
            // 
            // grbMemCancFisica
            // 
            this.grbMemCancFisica.BackColor = System.Drawing.Color.White;
            this.grbMemCancFisica.Controls.Add(this.rbtMemLunghi);
            this.grbMemCancFisica.Controls.Add(this.rbtMemBrevi);
            this.grbMemCancFisica.Controls.Add(this.rbtMemProgrammazioni);
            this.grbMemCancFisica.Controls.Add(this.rbtMemContatori);
            this.grbMemCancFisica.Controls.Add(this.btnMemResetBoard);
            this.grbMemCancFisica.Controls.Add(this.rbtMemDatiCliente);
            this.grbMemCancFisica.Controls.Add(this.rbtMemParametriInit);
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
            resources.ApplyResources(this.grbMemCancFisica, "grbMemCancFisica");
            this.grbMemCancFisica.Name = "grbMemCancFisica";
            this.grbMemCancFisica.TabStop = false;
            // 
            // rbtMemLunghi
            // 
            resources.ApplyResources(this.rbtMemLunghi, "rbtMemLunghi");
            this.rbtMemLunghi.Name = "rbtMemLunghi";
            this.rbtMemLunghi.UseVisualStyleBackColor = true;
            this.rbtMemLunghi.Click += new System.EventHandler(this.rbtMemLunghi_Click);
            // 
            // rbtMemBrevi
            // 
            resources.ApplyResources(this.rbtMemBrevi, "rbtMemBrevi");
            this.rbtMemBrevi.Name = "rbtMemBrevi";
            this.rbtMemBrevi.UseVisualStyleBackColor = true;
            this.rbtMemBrevi.Click += new System.EventHandler(this.rbtMemBrevi_Click);
            // 
            // rbtMemProgrammazioni
            // 
            resources.ApplyResources(this.rbtMemProgrammazioni, "rbtMemProgrammazioni");
            this.rbtMemProgrammazioni.Name = "rbtMemProgrammazioni";
            this.rbtMemProgrammazioni.UseVisualStyleBackColor = true;
            this.rbtMemProgrammazioni.Click += new System.EventHandler(this.rbtMemProgrammazioni_Click);
            // 
            // rbtMemContatori
            // 
            resources.ApplyResources(this.rbtMemContatori, "rbtMemContatori");
            this.rbtMemContatori.Name = "rbtMemContatori";
            this.rbtMemContatori.UseVisualStyleBackColor = true;
            this.rbtMemContatori.Click += new System.EventHandler(this.rbtMemContatori_Click);
            // 
            // btnMemResetBoard
            // 
            resources.ApplyResources(this.btnMemResetBoard, "btnMemResetBoard");
            this.btnMemResetBoard.Name = "btnMemResetBoard";
            this.btnMemResetBoard.UseVisualStyleBackColor = true;
            this.btnMemResetBoard.Click += new System.EventHandler(this.btnMemResetBoard_Click);
            // 
            // rbtMemDatiCliente
            // 
            resources.ApplyResources(this.rbtMemDatiCliente, "rbtMemDatiCliente");
            this.rbtMemDatiCliente.Name = "rbtMemDatiCliente";
            this.rbtMemDatiCliente.UseVisualStyleBackColor = true;
            this.rbtMemDatiCliente.CheckedChanged += new System.EventHandler(this.rbtMemDatiCliente_CheckedChanged);
            this.rbtMemDatiCliente.Click += new System.EventHandler(this.rbtMemDatiCliente_Click);
            // 
            // rbtMemParametriInit
            // 
            resources.ApplyResources(this.rbtMemParametriInit, "rbtMemParametriInit");
            this.rbtMemParametriInit.Name = "rbtMemParametriInit";
            this.rbtMemParametriInit.UseVisualStyleBackColor = true;
            this.rbtMemParametriInit.CheckedChanged += new System.EventHandler(this.rbtMemParametriInit_CheckedChanged);
            this.rbtMemParametriInit.Click += new System.EventHandler(this.rbtMemParametriInit_Click);
            // 
            // rbtMemAreaApp2
            // 
            resources.ApplyResources(this.rbtMemAreaApp2, "rbtMemAreaApp2");
            this.rbtMemAreaApp2.Name = "rbtMemAreaApp2";
            this.rbtMemAreaApp2.UseVisualStyleBackColor = true;
            this.rbtMemAreaApp2.CheckedChanged += new System.EventHandler(this.rbtMemAreaApp2_CheckedChanged);
            // 
            // rbtMemAreaApp1
            // 
            resources.ApplyResources(this.rbtMemAreaApp1, "rbtMemAreaApp1");
            this.rbtMemAreaApp1.Name = "rbtMemAreaApp1";
            this.rbtMemAreaApp1.UseVisualStyleBackColor = true;
            this.rbtMemAreaApp1.CheckedChanged += new System.EventHandler(this.rbtMemAreaApp1_CheckedChanged);
            // 
            // rbtMemAreaLibera
            // 
            resources.ApplyResources(this.rbtMemAreaLibera, "rbtMemAreaLibera");
            this.rbtMemAreaLibera.Checked = true;
            this.rbtMemAreaLibera.Name = "rbtMemAreaLibera";
            this.rbtMemAreaLibera.TabStop = true;
            this.rbtMemAreaLibera.UseVisualStyleBackColor = true;
            this.rbtMemAreaLibera.CheckedChanged += new System.EventHandler(this.rbtMemAreaLibera_CheckedChanged);
            this.rbtMemAreaLibera.Click += new System.EventHandler(this.rbtMemAreaLibera_Click);
            // 
            // label111
            // 
            resources.ApplyResources(this.label111, "label111");
            this.label111.Name = "label111";
            // 
            // txtMemCFBlocchi
            // 
            resources.ApplyResources(this.txtMemCFBlocchi, "txtMemCFBlocchi");
            this.txtMemCFBlocchi.Name = "txtMemCFBlocchi";
            // 
            // chkMemCFStartAddHex
            // 
            resources.ApplyResources(this.chkMemCFStartAddHex, "chkMemCFStartAddHex");
            this.chkMemCFStartAddHex.Checked = true;
            this.chkMemCFStartAddHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemCFStartAddHex.Name = "chkMemCFStartAddHex";
            this.chkMemCFStartAddHex.UseVisualStyleBackColor = true;
            // 
            // label112
            // 
            resources.ApplyResources(this.label112, "label112");
            this.label112.Name = "label112";
            // 
            // txtMemCFStartAdd
            // 
            resources.ApplyResources(this.txtMemCFStartAdd, "txtMemCFStartAdd");
            this.txtMemCFStartAdd.Name = "txtMemCFStartAdd";
            // 
            // btnMemCFExec
            // 
            resources.ApplyResources(this.btnMemCFExec, "btnMemCFExec");
            this.btnMemCFExec.Name = "btnMemCFExec";
            this.btnMemCFExec.UseVisualStyleBackColor = true;
            this.btnMemCFExec.Click += new System.EventHandler(this.btnMemCFExec_Click);
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.White;
            this.groupBox9.Controls.Add(this.btnDumpMemoria);
            resources.ApplyResources(this.groupBox9, "groupBox9");
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.TabStop = false;
            // 
            // btnDumpMemoria
            // 
            resources.ApplyResources(this.btnDumpMemoria, "btnDumpMemoria");
            this.btnDumpMemoria.Name = "btnDumpMemoria";
            this.btnDumpMemoria.UseVisualStyleBackColor = true;
            // 
            // txtMemDataGrid
            // 
            resources.ApplyResources(this.txtMemDataGrid, "txtMemDataGrid");
            this.txtMemDataGrid.Name = "txtMemDataGrid";
            // 
            // grbMemScrittura
            // 
            this.grbMemScrittura.BackColor = System.Drawing.Color.White;
            this.grbMemScrittura.Controls.Add(this.chkMemHexW);
            this.grbMemScrittura.Controls.Add(this.lblMemVerificaValore);
            this.grbMemScrittura.Controls.Add(this.label70);
            this.grbMemScrittura.Controls.Add(this.txtMemDataW);
            this.grbMemScrittura.Controls.Add(this.label71);
            this.grbMemScrittura.Controls.Add(this.label72);
            this.grbMemScrittura.Controls.Add(this.txtMemLenW);
            this.grbMemScrittura.Controls.Add(this.txtMemAddrW);
            this.grbMemScrittura.Controls.Add(this.cmdMemWrite);
            resources.ApplyResources(this.grbMemScrittura, "grbMemScrittura");
            this.grbMemScrittura.Name = "grbMemScrittura";
            this.grbMemScrittura.TabStop = false;
            // 
            // chkMemHexW
            // 
            resources.ApplyResources(this.chkMemHexW, "chkMemHexW");
            this.chkMemHexW.Checked = true;
            this.chkMemHexW.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemHexW.Name = "chkMemHexW";
            this.chkMemHexW.UseVisualStyleBackColor = true;
            // 
            // lblMemVerificaValore
            // 
            resources.ApplyResources(this.lblMemVerificaValore, "lblMemVerificaValore");
            this.lblMemVerificaValore.BackColor = System.Drawing.Color.Transparent;
            this.lblMemVerificaValore.Name = "lblMemVerificaValore";
            // 
            // label70
            // 
            resources.ApplyResources(this.label70, "label70");
            this.label70.Name = "label70";
            // 
            // txtMemDataW
            // 
            resources.ApplyResources(this.txtMemDataW, "txtMemDataW");
            this.txtMemDataW.Name = "txtMemDataW";
            // 
            // label71
            // 
            resources.ApplyResources(this.label71, "label71");
            this.label71.Name = "label71";
            // 
            // label72
            // 
            resources.ApplyResources(this.label72, "label72");
            this.label72.Name = "label72";
            // 
            // txtMemLenW
            // 
            resources.ApplyResources(this.txtMemLenW, "txtMemLenW");
            this.txtMemLenW.Name = "txtMemLenW";
            this.txtMemLenW.ReadOnly = true;
            // 
            // txtMemAddrW
            // 
            resources.ApplyResources(this.txtMemAddrW, "txtMemAddrW");
            this.txtMemAddrW.Name = "txtMemAddrW";
            // 
            // cmdMemWrite
            // 
            resources.ApplyResources(this.cmdMemWrite, "cmdMemWrite");
            this.cmdMemWrite.Name = "cmdMemWrite";
            this.cmdMemWrite.UseVisualStyleBackColor = true;
            this.cmdMemWrite.Click += new System.EventHandler(this.cmdMemWrite_Click);
            // 
            // grbMemCancellazione
            // 
            this.grbMemCancellazione.BackColor = System.Drawing.Color.White;
            this.grbMemCancellazione.Controls.Add(this.chkMemClearMantieniCliente);
            this.grbMemCancellazione.Controls.Add(this.cmdMemClear);
            resources.ApplyResources(this.grbMemCancellazione, "grbMemCancellazione");
            this.grbMemCancellazione.Name = "grbMemCancellazione";
            this.grbMemCancellazione.TabStop = false;
            // 
            // chkMemClearMantieniCliente
            // 
            resources.ApplyResources(this.chkMemClearMantieniCliente, "chkMemClearMantieniCliente");
            this.chkMemClearMantieniCliente.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.chkMemClearMantieniCliente.Name = "chkMemClearMantieniCliente";
            this.chkMemClearMantieniCliente.UseVisualStyleBackColor = true;
            // 
            // cmdMemClear
            // 
            resources.ApplyResources(this.cmdMemClear, "cmdMemClear");
            this.cmdMemClear.Name = "cmdMemClear";
            this.cmdMemClear.UseVisualStyleBackColor = true;
            // 
            // grbMemLettura
            // 
            this.grbMemLettura.BackColor = System.Drawing.Color.White;
            this.grbMemLettura.Controls.Add(this.chkMemInt);
            this.grbMemLettura.Controls.Add(this.chkMemHex);
            this.grbMemLettura.Controls.Add(this.lblReadMemBytes);
            this.grbMemLettura.Controls.Add(this.lblReadMemStartAddr);
            this.grbMemLettura.Controls.Add(this.txtMemLenR);
            this.grbMemLettura.Controls.Add(this.txtMemAddrR);
            this.grbMemLettura.Controls.Add(this.cmdMemRead);
            this.grbMemLettura.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this.grbMemLettura, "grbMemLettura");
            this.grbMemLettura.Name = "grbMemLettura";
            this.grbMemLettura.TabStop = false;
            // 
            // chkMemInt
            // 
            resources.ApplyResources(this.chkMemInt, "chkMemInt");
            this.chkMemInt.ForeColor = System.Drawing.Color.Blue;
            this.chkMemInt.Name = "chkMemInt";
            this.chkMemInt.UseVisualStyleBackColor = true;
            // 
            // chkMemHex
            // 
            resources.ApplyResources(this.chkMemHex, "chkMemHex");
            this.chkMemHex.Checked = true;
            this.chkMemHex.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemHex.Name = "chkMemHex";
            this.chkMemHex.UseVisualStyleBackColor = true;
            // 
            // lblReadMemBytes
            // 
            resources.ApplyResources(this.lblReadMemBytes, "lblReadMemBytes");
            this.lblReadMemBytes.Name = "lblReadMemBytes";
            // 
            // lblReadMemStartAddr
            // 
            resources.ApplyResources(this.lblReadMemStartAddr, "lblReadMemStartAddr");
            this.lblReadMemStartAddr.Name = "lblReadMemStartAddr";
            // 
            // txtMemLenR
            // 
            resources.ApplyResources(this.txtMemLenR, "txtMemLenR");
            this.txtMemLenR.Name = "txtMemLenR";
            // 
            // txtMemAddrR
            // 
            resources.ApplyResources(this.txtMemAddrR, "txtMemAddrR");
            this.txtMemAddrR.Name = "txtMemAddrR";
            // 
            // cmdMemRead
            // 
            resources.ApplyResources(this.cmdMemRead, "cmdMemRead");
            this.cmdMemRead.Name = "cmdMemRead";
            this.cmdMemRead.UseVisualStyleBackColor = true;
            this.cmdMemRead.Click += new System.EventHandler(this.cmdMemRead_Click);
            // 
            // tabInizializzazione
            // 
            this.tabInizializzazione.BackColor = System.Drawing.Color.LightYellow;
            this.tabInizializzazione.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tabInizializzazione, "tabInizializzazione");
            this.tabInizializzazione.Controls.Add(this.grbConnessioni);
            this.tabInizializzazione.Controls.Add(this.lblGenBRState);
            this.tabInizializzazione.Controls.Add(this.btnGenCambiaBaudRate);
            this.tabInizializzazione.Controls.Add(this.grbGenBaudrate);
            this.tabInizializzazione.Controls.Add(this.btnCaricaMemoria);
            this.tabInizializzazione.Controls.Add(this.grbInitLimiti);
            this.tabInizializzazione.Controls.Add(this.btnScriviInizializzazione);
            this.tabInizializzazione.Controls.Add(this.btnCaricaInizializzazione);
            this.tabInizializzazione.Controls.Add(this.groupBox11);
            this.tabInizializzazione.Controls.Add(this.grbInitCalibrazione);
            this.tabInizializzazione.Controls.Add(this.grbInitDatiBase);
            this.tabInizializzazione.Name = "tabInizializzazione";
            this.tabInizializzazione.Enter += new System.EventHandler(this.tabInizializzazione_Enter);
            // 
            // grbConnessioni
            // 
            resources.ApplyResources(this.grbConnessioni, "grbConnessioni");
            this.grbConnessioni.BackColor = System.Drawing.Color.Transparent;
            this.grbConnessioni.Controls.Add(this.checkBox3);
            this.grbConnessioni.Controls.Add(this.checkBox2);
            this.grbConnessioni.Controls.Add(this.checkBox1);
            this.grbConnessioni.Name = "grbConnessioni";
            this.grbConnessioni.TabStop = false;
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // lblGenBRState
            // 
            this.lblGenBRState.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lblGenBRState, "lblGenBRState");
            this.lblGenBRState.Name = "lblGenBRState";
            // 
            // btnGenCambiaBaudRate
            // 
            resources.ApplyResources(this.btnGenCambiaBaudRate, "btnGenCambiaBaudRate");
            this.btnGenCambiaBaudRate.Name = "btnGenCambiaBaudRate";
            this.btnGenCambiaBaudRate.UseVisualStyleBackColor = true;
            // 
            // grbGenBaudrate
            // 
            this.grbGenBaudrate.Controls.Add(this.optGenBR3M);
            this.grbGenBaudrate.Controls.Add(this.optGenBR1M);
            this.grbGenBaudrate.Controls.Add(this.optGenBR115);
            resources.ApplyResources(this.grbGenBaudrate, "grbGenBaudrate");
            this.grbGenBaudrate.Name = "grbGenBaudrate";
            this.grbGenBaudrate.TabStop = false;
            // 
            // optGenBR3M
            // 
            resources.ApplyResources(this.optGenBR3M, "optGenBR3M");
            this.optGenBR3M.Name = "optGenBR3M";
            this.optGenBR3M.UseVisualStyleBackColor = true;
            // 
            // optGenBR1M
            // 
            resources.ApplyResources(this.optGenBR1M, "optGenBR1M");
            this.optGenBR1M.Name = "optGenBR1M";
            this.optGenBR1M.UseVisualStyleBackColor = true;
            // 
            // optGenBR115
            // 
            resources.ApplyResources(this.optGenBR115, "optGenBR115");
            this.optGenBR115.Checked = true;
            this.optGenBR115.Name = "optGenBR115";
            this.optGenBR115.TabStop = true;
            this.optGenBR115.UseVisualStyleBackColor = true;
            // 
            // btnCaricaMemoria
            // 
            resources.ApplyResources(this.btnCaricaMemoria, "btnCaricaMemoria");
            this.btnCaricaMemoria.Name = "btnCaricaMemoria";
            this.btnCaricaMemoria.UseVisualStyleBackColor = true;
            // 
            // grbInitLimiti
            // 
            this.grbInitLimiti.BackColor = System.Drawing.Color.White;
            this.grbInitLimiti.Controls.Add(this.txtInitModelloMemoria);
            this.grbInitLimiti.Controls.Add(this.label150);
            this.grbInitLimiti.Controls.Add(this.txtInitMaxProg);
            this.grbInitLimiti.Controls.Add(this.label149);
            this.grbInitLimiti.Controls.Add(this.txtInitMaxLunghi);
            this.grbInitLimiti.Controls.Add(this.label148);
            this.grbInitLimiti.Controls.Add(this.txtInitMaxBrevi);
            this.grbInitLimiti.Controls.Add(this.label146);
            resources.ApplyResources(this.grbInitLimiti, "grbInitLimiti");
            this.grbInitLimiti.Name = "grbInitLimiti";
            this.grbInitLimiti.TabStop = false;
            // 
            // txtInitModelloMemoria
            // 
            resources.ApplyResources(this.txtInitModelloMemoria, "txtInitModelloMemoria");
            this.txtInitModelloMemoria.Name = "txtInitModelloMemoria";
            this.txtInitModelloMemoria.ReadOnly = true;
            // 
            // label150
            // 
            resources.ApplyResources(this.label150, "label150");
            this.label150.Name = "label150";
            // 
            // txtInitMaxProg
            // 
            resources.ApplyResources(this.txtInitMaxProg, "txtInitMaxProg");
            this.txtInitMaxProg.Name = "txtInitMaxProg";
            this.txtInitMaxProg.ReadOnly = true;
            // 
            // label149
            // 
            resources.ApplyResources(this.label149, "label149");
            this.label149.Name = "label149";
            // 
            // txtInitMaxLunghi
            // 
            resources.ApplyResources(this.txtInitMaxLunghi, "txtInitMaxLunghi");
            this.txtInitMaxLunghi.Name = "txtInitMaxLunghi";
            this.txtInitMaxLunghi.ReadOnly = true;
            // 
            // label148
            // 
            resources.ApplyResources(this.label148, "label148");
            this.label148.Name = "label148";
            // 
            // txtInitMaxBrevi
            // 
            resources.ApplyResources(this.txtInitMaxBrevi, "txtInitMaxBrevi");
            this.txtInitMaxBrevi.Name = "txtInitMaxBrevi";
            this.txtInitMaxBrevi.ReadOnly = true;
            // 
            // label146
            // 
            resources.ApplyResources(this.label146, "label146");
            this.label146.Name = "label146";
            // 
            // btnScriviInizializzazione
            // 
            resources.ApplyResources(this.btnScriviInizializzazione, "btnScriviInizializzazione");
            this.btnScriviInizializzazione.Name = "btnScriviInizializzazione";
            this.btnScriviInizializzazione.UseVisualStyleBackColor = true;
            this.btnScriviInizializzazione.Click += new System.EventHandler(this.btnScriviInizializzazione_Click);
            // 
            // btnCaricaInizializzazione
            // 
            resources.ApplyResources(this.btnCaricaInizializzazione, "btnCaricaInizializzazione");
            this.btnCaricaInizializzazione.Name = "btnCaricaInizializzazione";
            this.btnCaricaInizializzazione.UseVisualStyleBackColor = true;
            this.btnCaricaInizializzazione.Click += new System.EventHandler(this.btnCaricaInizializzazione_Click);
            // 
            // groupBox11
            // 
            this.groupBox11.BackColor = System.Drawing.Color.White;
            this.groupBox11.Controls.Add(this.txtInitBrdVMaxModulo);
            this.groupBox11.Controls.Add(this.label57);
            this.groupBox11.Controls.Add(this.txtInitBrdVMinModulo);
            this.groupBox11.Controls.Add(this.label56);
            this.groupBox11.Controls.Add(this.txtInitBrdOpzioniModulo);
            this.groupBox11.Controls.Add(this.label55);
            this.groupBox11.Controls.Add(this.label54);
            this.groupBox11.Controls.Add(this.chkInitBrdSpareModulo);
            this.groupBox11.Controls.Add(this.label53);
            this.groupBox11.Controls.Add(this.txtInitBrdANomModulo);
            this.groupBox11.Controls.Add(this.label52);
            this.groupBox11.Controls.Add(this.txtInitBrdVNomModulo);
            this.groupBox11.Controls.Add(this.label30);
            this.groupBox11.Controls.Add(this.txtInitBrdNumModuli);
            this.groupBox11.Controls.Add(this.txtInitRevHwDISP);
            this.groupBox11.Controls.Add(this.label131);
            this.groupBox11.Controls.Add(this.txtInitRevFwDISP);
            this.groupBox11.Controls.Add(this.label127);
            this.groupBox11.Controls.Add(this.txtInitNumSerDISP);
            this.groupBox11.Controls.Add(this.label128);
            resources.ApplyResources(this.groupBox11, "groupBox11");
            this.groupBox11.Name = "groupBox11";
            this.groupBox11.TabStop = false;
            // 
            // txtInitBrdVMaxModulo
            // 
            resources.ApplyResources(this.txtInitBrdVMaxModulo, "txtInitBrdVMaxModulo");
            this.txtInitBrdVMaxModulo.Name = "txtInitBrdVMaxModulo";
            // 
            // label57
            // 
            resources.ApplyResources(this.label57, "label57");
            this.label57.Name = "label57";
            // 
            // txtInitBrdVMinModulo
            // 
            resources.ApplyResources(this.txtInitBrdVMinModulo, "txtInitBrdVMinModulo");
            this.txtInitBrdVMinModulo.Name = "txtInitBrdVMinModulo";
            // 
            // label56
            // 
            resources.ApplyResources(this.label56, "label56");
            this.label56.Name = "label56";
            // 
            // txtInitBrdOpzioniModulo
            // 
            resources.ApplyResources(this.txtInitBrdOpzioniModulo, "txtInitBrdOpzioniModulo");
            this.txtInitBrdOpzioniModulo.Name = "txtInitBrdOpzioniModulo";
            // 
            // label55
            // 
            resources.ApplyResources(this.label55, "label55");
            this.label55.Name = "label55";
            // 
            // label54
            // 
            resources.ApplyResources(this.label54, "label54");
            this.label54.Name = "label54";
            // 
            // chkInitBrdSpareModulo
            // 
            resources.ApplyResources(this.chkInitBrdSpareModulo, "chkInitBrdSpareModulo");
            this.chkInitBrdSpareModulo.Name = "chkInitBrdSpareModulo";
            this.chkInitBrdSpareModulo.UseVisualStyleBackColor = true;
            // 
            // label53
            // 
            resources.ApplyResources(this.label53, "label53");
            this.label53.Name = "label53";
            // 
            // txtInitBrdANomModulo
            // 
            resources.ApplyResources(this.txtInitBrdANomModulo, "txtInitBrdANomModulo");
            this.txtInitBrdANomModulo.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtInitBrdANomModulo.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.txtInitBrdANomModulo.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtInitBrdANomModulo.Name = "txtInitBrdANomModulo";
            this.txtInitBrdANomModulo.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtInitBrdANomModulo.Leave += new System.EventHandler(this.txtInitBrdANomModulo_Leave);
            // 
            // label52
            // 
            resources.ApplyResources(this.label52, "label52");
            this.label52.Name = "label52";
            // 
            // txtInitBrdVNomModulo
            // 
            resources.ApplyResources(this.txtInitBrdVNomModulo, "txtInitBrdVNomModulo");
            this.txtInitBrdVNomModulo.Increment = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.txtInitBrdVNomModulo.Maximum = new decimal(new int[] {
            92,
            0,
            0,
            0});
            this.txtInitBrdVNomModulo.Minimum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.txtInitBrdVNomModulo.Name = "txtInitBrdVNomModulo";
            this.txtInitBrdVNomModulo.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // txtInitBrdNumModuli
            // 
            resources.ApplyResources(this.txtInitBrdNumModuli, "txtInitBrdNumModuli");
            this.txtInitBrdNumModuli.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.txtInitBrdNumModuli.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtInitBrdNumModuli.Name = "txtInitBrdNumModuli";
            this.txtInitBrdNumModuli.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtInitRevHwDISP
            // 
            resources.ApplyResources(this.txtInitRevHwDISP, "txtInitRevHwDISP");
            this.txtInitRevHwDISP.Name = "txtInitRevHwDISP";
            // 
            // label131
            // 
            resources.ApplyResources(this.label131, "label131");
            this.label131.Name = "label131";
            // 
            // txtInitRevFwDISP
            // 
            resources.ApplyResources(this.txtInitRevFwDISP, "txtInitRevFwDISP");
            this.txtInitRevFwDISP.Name = "txtInitRevFwDISP";
            this.txtInitRevFwDISP.ReadOnly = true;
            // 
            // label127
            // 
            resources.ApplyResources(this.label127, "label127");
            this.label127.Name = "label127";
            // 
            // txtInitNumSerDISP
            // 
            resources.ApplyResources(this.txtInitNumSerDISP, "txtInitNumSerDISP");
            this.txtInitNumSerDISP.Name = "txtInitNumSerDISP";
            this.txtInitNumSerDISP.Leave += new System.EventHandler(this.txtInitNumSerDISP_Leave);
            // 
            // label128
            // 
            resources.ApplyResources(this.label128, "label128");
            this.label128.Name = "label128";
            // 
            // grbInitCalibrazione
            // 
            this.grbInitCalibrazione.BackColor = System.Drawing.Color.Transparent;
            this.grbInitCalibrazione.Controls.Add(this.textBox11);
            this.grbInitCalibrazione.Controls.Add(this.label101);
            this.grbInitCalibrazione.Controls.Add(this.textBox10);
            this.grbInitCalibrazione.Controls.Add(this.label100);
            this.grbInitCalibrazione.Controls.Add(this.textBox36);
            this.grbInitCalibrazione.Controls.Add(this.label106);
            resources.ApplyResources(this.grbInitCalibrazione, "grbInitCalibrazione");
            this.grbInitCalibrazione.Name = "grbInitCalibrazione";
            this.grbInitCalibrazione.TabStop = false;
            // 
            // textBox11
            // 
            resources.ApplyResources(this.textBox11, "textBox11");
            this.textBox11.Name = "textBox11";
            // 
            // label101
            // 
            resources.ApplyResources(this.label101, "label101");
            this.label101.Name = "label101";
            // 
            // textBox10
            // 
            resources.ApplyResources(this.textBox10, "textBox10");
            this.textBox10.Name = "textBox10";
            // 
            // label100
            // 
            resources.ApplyResources(this.label100, "label100");
            this.label100.Name = "label100";
            // 
            // textBox36
            // 
            resources.ApplyResources(this.textBox36, "textBox36");
            this.textBox36.Name = "textBox36";
            // 
            // label106
            // 
            resources.ApplyResources(this.label106, "label106");
            this.label106.Name = "label106";
            // 
            // grbInitDatiBase
            // 
            this.grbInitDatiBase.BackColor = System.Drawing.Color.White;
            this.grbInitDatiBase.Controls.Add(this.txtInitAMax);
            this.grbInitDatiBase.Controls.Add(this.label58);
            this.grbInitDatiBase.Controls.Add(this.label26);
            this.grbInitDatiBase.Controls.Add(this.label10);
            this.grbInitDatiBase.Controls.Add(this.label9);
            this.grbInitDatiBase.Controls.Add(this.chkInitPresenzaRabb);
            this.grbInitDatiBase.Controls.Add(this.txtInitVMax);
            this.grbInitDatiBase.Controls.Add(this.txtInitVMin);
            this.grbInitDatiBase.Controls.Add(this.txtInitIDApparato);
            this.grbInitDatiBase.Controls.Add(this.label144);
            this.grbInitDatiBase.Controls.Add(this.txtInitDataInizializ);
            this.grbInitDatiBase.Controls.Add(this.label132);
            this.grbInitDatiBase.Controls.Add(this.cmbInitTipoApparato);
            this.grbInitDatiBase.Controls.Add(this.label99);
            this.grbInitDatiBase.Controls.Add(this.txtInitSerialeApparato);
            this.grbInitDatiBase.Controls.Add(this.label98);
            this.grbInitDatiBase.Controls.Add(this.txtInitNumeroMatricola);
            this.grbInitDatiBase.Controls.Add(this.label88);
            this.grbInitDatiBase.Controls.Add(this.txtInitAnnoMatricola);
            this.grbInitDatiBase.Controls.Add(this.Anno);
            this.grbInitDatiBase.Controls.Add(this.txtInitProductId);
            this.grbInitDatiBase.Controls.Add(this.lblInitProductId);
            this.grbInitDatiBase.Controls.Add(this.txtInitManufactured);
            this.grbInitDatiBase.Controls.Add(this.lblInitManufactured);
            resources.ApplyResources(this.grbInitDatiBase, "grbInitDatiBase");
            this.grbInitDatiBase.Name = "grbInitDatiBase";
            this.grbInitDatiBase.TabStop = false;
            this.grbInitDatiBase.Enter += new System.EventHandler(this.grbInitDatiBase_Enter);
            // 
            // txtInitAMax
            // 
            resources.ApplyResources(this.txtInitAMax, "txtInitAMax");
            this.txtInitAMax.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtInitAMax.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.txtInitAMax.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtInitAMax.Name = "txtInitAMax";
            this.txtInitAMax.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label58
            // 
            resources.ApplyResources(this.label58, "label58");
            this.label58.Name = "label58";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // chkInitPresenzaRabb
            // 
            resources.ApplyResources(this.chkInitPresenzaRabb, "chkInitPresenzaRabb");
            this.chkInitPresenzaRabb.Name = "chkInitPresenzaRabb";
            this.chkInitPresenzaRabb.UseVisualStyleBackColor = true;
            // 
            // txtInitVMax
            // 
            resources.ApplyResources(this.txtInitVMax, "txtInitVMax");
            this.txtInitVMax.Name = "txtInitVMax";
            this.txtInitVMax.ReadOnly = true;
            // 
            // txtInitVMin
            // 
            resources.ApplyResources(this.txtInitVMin, "txtInitVMin");
            this.txtInitVMin.Name = "txtInitVMin";
            this.txtInitVMin.ReadOnly = true;
            // 
            // txtInitIDApparato
            // 
            resources.ApplyResources(this.txtInitIDApparato, "txtInitIDApparato");
            this.txtInitIDApparato.Name = "txtInitIDApparato";
            this.txtInitIDApparato.ReadOnly = true;
            // 
            // label144
            // 
            resources.ApplyResources(this.label144, "label144");
            this.label144.Name = "label144";
            // 
            // txtInitDataInizializ
            // 
            resources.ApplyResources(this.txtInitDataInizializ, "txtInitDataInizializ");
            this.txtInitDataInizializ.Name = "txtInitDataInizializ";
            this.txtInitDataInizializ.ValidatingType = typeof(System.DateTime);
            // 
            // label132
            // 
            resources.ApplyResources(this.label132, "label132");
            this.label132.Name = "label132";
            // 
            // cmbInitTipoApparato
            // 
            resources.ApplyResources(this.cmbInitTipoApparato, "cmbInitTipoApparato");
            this.cmbInitTipoApparato.FormattingEnabled = true;
            this.cmbInitTipoApparato.Name = "cmbInitTipoApparato";
            this.cmbInitTipoApparato.SelectedIndexChanged += new System.EventHandler(this.cmbInitTipoApparato_SelectedIndexChanged);
            // 
            // label99
            // 
            resources.ApplyResources(this.label99, "label99");
            this.label99.Name = "label99";
            // 
            // txtInitSerialeApparato
            // 
            resources.ApplyResources(this.txtInitSerialeApparato, "txtInitSerialeApparato");
            this.txtInitSerialeApparato.Name = "txtInitSerialeApparato";
            this.txtInitSerialeApparato.ReadOnly = true;
            // 
            // label98
            // 
            resources.ApplyResources(this.label98, "label98");
            this.label98.Name = "label98";
            // 
            // txtInitNumeroMatricola
            // 
            resources.ApplyResources(this.txtInitNumeroMatricola, "txtInitNumeroMatricola");
            this.txtInitNumeroMatricola.Name = "txtInitNumeroMatricola";
            this.txtInitNumeroMatricola.Leave += new System.EventHandler(this.txtInitNumeroMatricola_Leave);
            // 
            // label88
            // 
            resources.ApplyResources(this.label88, "label88");
            this.label88.Name = "label88";
            // 
            // txtInitAnnoMatricola
            // 
            resources.ApplyResources(this.txtInitAnnoMatricola, "txtInitAnnoMatricola");
            this.txtInitAnnoMatricola.Name = "txtInitAnnoMatricola";
            this.txtInitAnnoMatricola.Leave += new System.EventHandler(this.txtInitAnnoMatricola_Leave);
            // 
            // Anno
            // 
            resources.ApplyResources(this.Anno, "Anno");
            this.Anno.Name = "Anno";
            // 
            // txtInitProductId
            // 
            resources.ApplyResources(this.txtInitProductId, "txtInitProductId");
            this.txtInitProductId.Name = "txtInitProductId";
            this.txtInitProductId.ReadOnly = true;
            // 
            // lblInitProductId
            // 
            resources.ApplyResources(this.lblInitProductId, "lblInitProductId");
            this.lblInitProductId.Name = "lblInitProductId";
            // 
            // txtInitManufactured
            // 
            resources.ApplyResources(this.txtInitManufactured, "txtInitManufactured");
            this.txtInitManufactured.Name = "txtInitManufactured";
            this.txtInitManufactured.ReadOnly = true;
            // 
            // lblInitManufactured
            // 
            resources.ApplyResources(this.lblInitManufactured, "lblInitManufactured");
            this.lblInitManufactured.Name = "lblInitManufactured";
            // 
            // tabOrologio
            // 
            this.tabOrologio.BackColor = System.Drawing.Color.LightYellow;
            this.tabOrologio.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tabOrologio, "tabOrologio");
            this.tabOrologio.Controls.Add(this.grbCalData);
            this.tabOrologio.Controls.Add(this.grbAccensione);
            this.tabOrologio.Controls.Add(this.grbOraCorrente);
            this.tabOrologio.Name = "tabOrologio";
            // 
            // grbCalData
            // 
            this.grbCalData.BackColor = System.Drawing.Color.White;
            this.grbCalData.Controls.Add(this.txtCalMinuti);
            this.grbCalData.Controls.Add(this.label266);
            this.grbCalData.Controls.Add(this.txtCalOre);
            this.grbCalData.Controls.Add(this.label267);
            this.grbCalData.Controls.Add(this.btnCalScriviGiorno);
            this.grbCalData.Controls.Add(this.txtCalAnno);
            this.grbCalData.Controls.Add(this.label250);
            this.grbCalData.Controls.Add(this.txtCalMese);
            this.grbCalData.Controls.Add(this.label249);
            this.grbCalData.Controls.Add(this.txtCalGiorno);
            this.grbCalData.Controls.Add(this.label248);
            resources.ApplyResources(this.grbCalData, "grbCalData");
            this.grbCalData.Name = "grbCalData";
            this.grbCalData.TabStop = false;
            // 
            // txtCalMinuti
            // 
            resources.ApplyResources(this.txtCalMinuti, "txtCalMinuti");
            this.txtCalMinuti.Name = "txtCalMinuti";
            // 
            // label266
            // 
            resources.ApplyResources(this.label266, "label266");
            this.label266.Name = "label266";
            // 
            // txtCalOre
            // 
            resources.ApplyResources(this.txtCalOre, "txtCalOre");
            this.txtCalOre.Name = "txtCalOre";
            // 
            // label267
            // 
            resources.ApplyResources(this.label267, "label267");
            this.label267.Name = "label267";
            // 
            // btnCalScriviGiorno
            // 
            resources.ApplyResources(this.btnCalScriviGiorno, "btnCalScriviGiorno");
            this.btnCalScriviGiorno.Name = "btnCalScriviGiorno";
            this.btnCalScriviGiorno.UseVisualStyleBackColor = true;
            this.btnCalScriviGiorno.Click += new System.EventHandler(this.btnCalScriviGiorno_Click);
            // 
            // txtCalAnno
            // 
            resources.ApplyResources(this.txtCalAnno, "txtCalAnno");
            this.txtCalAnno.Name = "txtCalAnno";
            // 
            // label250
            // 
            resources.ApplyResources(this.label250, "label250");
            this.label250.Name = "label250";
            // 
            // txtCalMese
            // 
            resources.ApplyResources(this.txtCalMese, "txtCalMese");
            this.txtCalMese.Name = "txtCalMese";
            // 
            // label249
            // 
            resources.ApplyResources(this.label249, "label249");
            this.label249.Name = "label249";
            // 
            // txtCalGiorno
            // 
            resources.ApplyResources(this.txtCalGiorno, "txtCalGiorno");
            this.txtCalGiorno.Name = "txtCalGiorno";
            // 
            // label248
            // 
            resources.ApplyResources(this.label248, "label248");
            this.label248.Name = "label248";
            // 
            // grbAccensione
            // 
            this.grbAccensione.Controls.Add(this.lblOrarioAccensione);
            this.grbAccensione.Controls.Add(this.cmbMinAccensione);
            this.grbAccensione.Controls.Add(this.cmbOraAccensione);
            this.grbAccensione.Controls.Add(this.rbtAccensione03);
            this.grbAccensione.Controls.Add(this.lblOreRitardo);
            this.grbAccensione.Controls.Add(this.cmbOreRitardo);
            this.grbAccensione.Controls.Add(this.rbtAccensione02);
            this.grbAccensione.Controls.Add(this.rbtAccensione01);
            resources.ApplyResources(this.grbAccensione, "grbAccensione");
            this.grbAccensione.Name = "grbAccensione";
            this.grbAccensione.TabStop = false;
            // 
            // lblOrarioAccensione
            // 
            resources.ApplyResources(this.lblOrarioAccensione, "lblOrarioAccensione");
            this.lblOrarioAccensione.Name = "lblOrarioAccensione";
            // 
            // cmbMinAccensione
            // 
            resources.ApplyResources(this.cmbMinAccensione, "cmbMinAccensione");
            this.cmbMinAccensione.FormattingEnabled = true;
            this.cmbMinAccensione.Items.AddRange(new object[] {
            resources.GetString("cmbMinAccensione.Items"),
            resources.GetString("cmbMinAccensione.Items1"),
            resources.GetString("cmbMinAccensione.Items2"),
            resources.GetString("cmbMinAccensione.Items3")});
            this.cmbMinAccensione.Name = "cmbMinAccensione";
            // 
            // cmbOraAccensione
            // 
            resources.ApplyResources(this.cmbOraAccensione, "cmbOraAccensione");
            this.cmbOraAccensione.FormattingEnabled = true;
            this.cmbOraAccensione.Items.AddRange(new object[] {
            resources.GetString("cmbOraAccensione.Items"),
            resources.GetString("cmbOraAccensione.Items1"),
            resources.GetString("cmbOraAccensione.Items2"),
            resources.GetString("cmbOraAccensione.Items3"),
            resources.GetString("cmbOraAccensione.Items4"),
            resources.GetString("cmbOraAccensione.Items5"),
            resources.GetString("cmbOraAccensione.Items6"),
            resources.GetString("cmbOraAccensione.Items7"),
            resources.GetString("cmbOraAccensione.Items8"),
            resources.GetString("cmbOraAccensione.Items9"),
            resources.GetString("cmbOraAccensione.Items10"),
            resources.GetString("cmbOraAccensione.Items11"),
            resources.GetString("cmbOraAccensione.Items12"),
            resources.GetString("cmbOraAccensione.Items13"),
            resources.GetString("cmbOraAccensione.Items14"),
            resources.GetString("cmbOraAccensione.Items15"),
            resources.GetString("cmbOraAccensione.Items16"),
            resources.GetString("cmbOraAccensione.Items17"),
            resources.GetString("cmbOraAccensione.Items18"),
            resources.GetString("cmbOraAccensione.Items19"),
            resources.GetString("cmbOraAccensione.Items20"),
            resources.GetString("cmbOraAccensione.Items21"),
            resources.GetString("cmbOraAccensione.Items22"),
            resources.GetString("cmbOraAccensione.Items23")});
            this.cmbOraAccensione.Name = "cmbOraAccensione";
            // 
            // rbtAccensione03
            // 
            resources.ApplyResources(this.rbtAccensione03, "rbtAccensione03");
            this.rbtAccensione03.Name = "rbtAccensione03";
            this.rbtAccensione03.UseVisualStyleBackColor = true;
            this.rbtAccensione03.CheckedChanged += new System.EventHandler(this.rbtAccensione03_CheckedChanged);
            // 
            // lblOreRitardo
            // 
            resources.ApplyResources(this.lblOreRitardo, "lblOreRitardo");
            this.lblOreRitardo.Name = "lblOreRitardo";
            // 
            // cmbOreRitardo
            // 
            resources.ApplyResources(this.cmbOreRitardo, "cmbOreRitardo");
            this.cmbOreRitardo.FormattingEnabled = true;
            this.cmbOreRitardo.Items.AddRange(new object[] {
            resources.GetString("cmbOreRitardo.Items"),
            resources.GetString("cmbOreRitardo.Items1"),
            resources.GetString("cmbOreRitardo.Items2"),
            resources.GetString("cmbOreRitardo.Items3"),
            resources.GetString("cmbOreRitardo.Items4"),
            resources.GetString("cmbOreRitardo.Items5"),
            resources.GetString("cmbOreRitardo.Items6"),
            resources.GetString("cmbOreRitardo.Items7"),
            resources.GetString("cmbOreRitardo.Items8"),
            resources.GetString("cmbOreRitardo.Items9"),
            resources.GetString("cmbOreRitardo.Items10"),
            resources.GetString("cmbOreRitardo.Items11"),
            resources.GetString("cmbOreRitardo.Items12"),
            resources.GetString("cmbOreRitardo.Items13"),
            resources.GetString("cmbOreRitardo.Items14"),
            resources.GetString("cmbOreRitardo.Items15"),
            resources.GetString("cmbOreRitardo.Items16"),
            resources.GetString("cmbOreRitardo.Items17"),
            resources.GetString("cmbOreRitardo.Items18"),
            resources.GetString("cmbOreRitardo.Items19"),
            resources.GetString("cmbOreRitardo.Items20"),
            resources.GetString("cmbOreRitardo.Items21"),
            resources.GetString("cmbOreRitardo.Items22")});
            this.cmbOreRitardo.Name = "cmbOreRitardo";
            // 
            // rbtAccensione02
            // 
            resources.ApplyResources(this.rbtAccensione02, "rbtAccensione02");
            this.rbtAccensione02.Name = "rbtAccensione02";
            this.rbtAccensione02.UseVisualStyleBackColor = true;
            this.rbtAccensione02.CheckedChanged += new System.EventHandler(this.rbtAccensione02_CheckedChanged);
            // 
            // rbtAccensione01
            // 
            resources.ApplyResources(this.rbtAccensione01, "rbtAccensione01");
            this.rbtAccensione01.Checked = true;
            this.rbtAccensione01.Name = "rbtAccensione01";
            this.rbtAccensione01.TabStop = true;
            this.rbtAccensione01.UseVisualStyleBackColor = true;
            this.rbtAccensione01.CheckedChanged += new System.EventHandler(this.rbtAccensione01_CheckedChanged);
            // 
            // grbOraCorrente
            // 
            this.grbOraCorrente.BackColor = System.Drawing.Color.White;
            this.grbOraCorrente.Controls.Add(this.button1);
            this.grbOraCorrente.Controls.Add(this.btnLeggiRtc);
            this.grbOraCorrente.Controls.Add(this.txtOraRtc);
            this.grbOraCorrente.Controls.Add(this.lblOraRTC);
            this.grbOraCorrente.Controls.Add(this.txtDataRtc);
            this.grbOraCorrente.Controls.Add(this.lblDataRTC);
            resources.ApplyResources(this.grbOraCorrente, "grbOraCorrente");
            this.grbOraCorrente.Name = "grbOraCorrente";
            this.grbOraCorrente.TabStop = false;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnLeggiRtc
            // 
            resources.ApplyResources(this.btnLeggiRtc, "btnLeggiRtc");
            this.btnLeggiRtc.Name = "btnLeggiRtc";
            this.btnLeggiRtc.UseVisualStyleBackColor = true;
            this.btnLeggiRtc.Click += new System.EventHandler(this.btnLeggiRtc_Click);
            // 
            // txtOraRtc
            // 
            resources.ApplyResources(this.txtOraRtc, "txtOraRtc");
            this.txtOraRtc.Name = "txtOraRtc";
            // 
            // lblOraRTC
            // 
            resources.ApplyResources(this.lblOraRTC, "lblOraRTC");
            this.lblOraRTC.Name = "lblOraRTC";
            this.lblOraRTC.Click += new System.EventHandler(this.lblOraRTC_Click);
            // 
            // txtDataRtc
            // 
            resources.ApplyResources(this.txtDataRtc, "txtDataRtc");
            this.txtDataRtc.Name = "txtDataRtc";
            // 
            // lblDataRTC
            // 
            resources.ApplyResources(this.lblDataRTC, "lblDataRTC");
            this.lblDataRTC.Name = "lblDataRTC";
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
            this.tbcPaSottopagina.Controls.Add(this.tbpPaProfiloAttivo);
            this.tbcPaSottopagina.Controls.Add(this.tbpPaListaProfili);
            this.tbcPaSottopagina.Controls.Add(this.tbpPaCfgAvanzate);
            this.tbcPaSottopagina.Name = "tbcPaSottopagina";
            this.tbcPaSottopagina.SelectedIndex = 0;
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
            this.pippo.Controls.Add(this.btnPaProfileChiudiCanale);
            this.pippo.Controls.Add(this.btnPaCaricaCicli);
            this.pippo.Controls.Add(this.chkPaSbloccaValori);
            this.pippo.Controls.Add(this.lblPaSbloccaValori);
            this.pippo.Controls.Add(this.btnCicloCorrente);
            this.pippo.Controls.Add(this.btnPaProfileRefresh);
            this.pippo.Controls.Add(this.picPaImmagineProfilo);
            this.pippo.Controls.Add(this.tbcPaSchedeValori);
            this.pippo.Controls.Add(this.chkPaUsaSpyBatt);
            this.pippo.Controls.Add(this.label69);
            this.pippo.Controls.Add(this.btnPaSalvaDati);
            resources.ApplyResources(this.pippo, "pippo");
            this.pippo.Name = "pippo";
            this.pippo.TabStop = false;
            this.pippo.Enter += new System.EventHandler(this.grbCicloCorrente_Enter);
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
            this.grbPaImpostazioniLocali.Controls.Add(this.btnPaProfileImport);
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
            // btnPaProfileImport
            // 
            resources.ApplyResources(this.btnPaProfileImport, "btnPaProfileImport");
            this.btnPaProfileImport.ForeColor = System.Drawing.Color.Red;
            this.btnPaProfileImport.Name = "btnPaProfileImport";
            this.btnPaProfileImport.UseVisualStyleBackColor = true;
            this.btnPaProfileImport.Click += new System.EventHandler(this.btnPaProfileImport_Click);
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
            // btnPaProfileChiudiCanale
            // 
            resources.ApplyResources(this.btnPaProfileChiudiCanale, "btnPaProfileChiudiCanale");
            this.btnPaProfileChiudiCanale.ForeColor = System.Drawing.Color.Red;
            this.btnPaProfileChiudiCanale.Name = "btnPaProfileChiudiCanale";
            this.btnPaProfileChiudiCanale.UseVisualStyleBackColor = true;
            this.btnPaProfileChiudiCanale.Click += new System.EventHandler(this.btnPaProfileChiudiCanale_Click);
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
            // btnCicloCorrente
            // 
            resources.ApplyResources(this.btnCicloCorrente, "btnCicloCorrente");
            this.btnCicloCorrente.Name = "btnCicloCorrente";
            this.btnCicloCorrente.UseVisualStyleBackColor = true;
            this.btnCicloCorrente.Click += new System.EventHandler(this.btnCicloCorrente_Click);
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
            this.tbpPaGeneraleCiclo.Controls.Add(this.txtPaCassone);
            this.tbpPaGeneraleCiclo.Controls.Add(this.label38);
            this.tbpPaGeneraleCiclo.Controls.Add(this.txtPaIdSetup);
            this.tbpPaGeneraleCiclo.Controls.Add(this.lblPaIdSetup);
            this.tbpPaGeneraleCiclo.Controls.Add(this.txtPaNomeSetup);
            this.tbpPaGeneraleCiclo.Controls.Add(this.label152);
            resources.ApplyResources(this.tbpPaGeneraleCiclo, "tbpPaGeneraleCiclo");
            this.tbpPaGeneraleCiclo.Name = "tbpPaGeneraleCiclo";
            this.tbpPaGeneraleCiclo.UseVisualStyleBackColor = true;
            // 
            // txtPaCassone
            // 
            resources.ApplyResources(this.txtPaCassone, "txtPaCassone");
            this.txtPaCassone.Name = "txtPaCassone";
            // 
            // label38
            // 
            resources.ApplyResources(this.label38, "label38");
            this.label38.Name = "label38";
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
            // label152
            // 
            resources.ApplyResources(this.label152, "label152");
            this.label152.Name = "label152";
            // 
            // tbpPaPCStep0
            // 
            this.tbpPaPCStep0.Controls.Add(this.txtPaDurataMaxT0);
            this.tbpPaPCStep0.Controls.Add(this.label39);
            this.tbpPaPCStep0.Controls.Add(this.txtPaPrefaseI0);
            this.tbpPaPCStep0.Controls.Add(this.label23);
            this.tbpPaPCStep0.Controls.Add(this.txtPaSogliaV0);
            this.tbpPaPCStep0.Controls.Add(this.label22);
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
            // label39
            // 
            resources.ApplyResources(this.label39, "label39");
            this.label39.Name = "label39";
            // 
            // txtPaPrefaseI0
            // 
            resources.ApplyResources(this.txtPaPrefaseI0, "txtPaPrefaseI0");
            this.txtPaPrefaseI0.Name = "txtPaPrefaseI0";
            this.txtPaPrefaseI0.Leave += new System.EventHandler(this.TxtPaPrefaseI0_Leave);
            // 
            // label23
            // 
            resources.ApplyResources(this.label23, "label23");
            this.label23.Name = "label23";
            // 
            // txtPaSogliaV0
            // 
            resources.ApplyResources(this.txtPaSogliaV0, "txtPaSogliaV0");
            this.txtPaSogliaV0.Name = "txtPaSogliaV0";
            this.txtPaSogliaV0.Leave += new System.EventHandler(this.TxtPaSogliaV0_Leave);
            // 
            // label22
            // 
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // tbpPaPCStep1
            // 
            this.tbpPaPCStep1.Controls.Add(this.cmbPaDurataMaxT1);
            this.tbpPaPCStep1.Controls.Add(this.cmbPaDurataCarica);
            this.tbpPaPCStep1.Controls.Add(this.label6);
            this.tbpPaPCStep1.Controls.Add(this.txtPaCorrenteI1);
            this.tbpPaPCStep1.Controls.Add(this.label13);
            this.tbpPaPCStep1.Controls.Add(this.txtPaSogliaVs);
            this.tbpPaPCStep1.Controls.Add(this.label12);
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
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txtPaCorrenteI1
            // 
            resources.ApplyResources(this.txtPaCorrenteI1, "txtPaCorrenteI1");
            this.txtPaCorrenteI1.Name = "txtPaCorrenteI1";
            this.txtPaCorrenteI1.Leave += new System.EventHandler(this.TxtPaCorrenteI1_Leave);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // txtPaSogliaVs
            // 
            resources.ApplyResources(this.txtPaSogliaVs, "txtPaSogliaVs");
            this.txtPaSogliaVs.Name = "txtPaSogliaVs";
            this.txtPaSogliaVs.Leave += new System.EventHandler(this.TxtPaSogliaVs_Leave);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // tbpPaPCStep2
            // 
            this.tbpPaPCStep2.Controls.Add(this.txtPaTempoFin);
            this.tbpPaPCStep2.Controls.Add(this.label31);
            this.tbpPaPCStep2.Controls.Add(this.txtPadT);
            this.tbpPaPCStep2.Controls.Add(this.label21);
            this.tbpPaPCStep2.Controls.Add(this.txtPadV);
            this.tbpPaPCStep2.Controls.Add(this.label20);
            this.tbpPaPCStep2.Controls.Add(this.label11);
            this.tbpPaPCStep2.Controls.Add(this.txtPaCoeffKc);
            this.tbpPaPCStep2.Controls.Add(this.txtPaVMax);
            this.tbpPaPCStep2.Controls.Add(this.label207);
            this.tbpPaPCStep2.Controls.Add(this.label17);
            this.tbpPaPCStep2.Controls.Add(this.txtPaCoeffK);
            this.tbpPaPCStep2.Controls.Add(this.label27);
            this.tbpPaPCStep2.Controls.Add(this.txtPaTempoT2Max);
            this.tbpPaPCStep2.Controls.Add(this.txtPaTempoT2Min);
            this.tbpPaPCStep2.Controls.Add(this.label18);
            this.tbpPaPCStep2.Controls.Add(this.txtPaCorrenteRaccordo);
            this.tbpPaPCStep2.Controls.Add(this.label24);
            this.tbpPaPCStep2.Controls.Add(this.txtPaCorrenteF3);
            this.tbpPaPCStep2.Controls.Add(this.label16);
            this.tbpPaPCStep2.Controls.Add(this.txtPaRaccordoF1);
            this.tbpPaPCStep2.Controls.Add(this.label14);
            resources.ApplyResources(this.tbpPaPCStep2, "tbpPaPCStep2");
            this.tbpPaPCStep2.Name = "tbpPaPCStep2";
            this.tbpPaPCStep2.UseVisualStyleBackColor = true;
            // 
            // txtPaTempoFin
            // 
            resources.ApplyResources(this.txtPaTempoFin, "txtPaTempoFin");
            this.txtPaTempoFin.Name = "txtPaTempoFin";
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.Name = "label31";
            // 
            // txtPadT
            // 
            resources.ApplyResources(this.txtPadT, "txtPadT");
            this.txtPadT.Name = "txtPadT";
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // txtPadV
            // 
            resources.ApplyResources(this.txtPadV, "txtPadV");
            this.txtPadV.Name = "txtPadV";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
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
            // label207
            // 
            resources.ApplyResources(this.label207, "label207");
            this.label207.Name = "label207";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // txtPaCoeffK
            // 
            resources.ApplyResources(this.txtPaCoeffK, "txtPaCoeffK");
            this.txtPaCoeffK.Name = "txtPaCoeffK";
            this.txtPaCoeffK.Leave += new System.EventHandler(this.TxtPaCoeffK_Leave);
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
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
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // txtPaCorrenteRaccordo
            // 
            resources.ApplyResources(this.txtPaCorrenteRaccordo, "txtPaCorrenteRaccordo");
            this.txtPaCorrenteRaccordo.Name = "txtPaCorrenteRaccordo";
            this.txtPaCorrenteRaccordo.Leave += new System.EventHandler(this.TxtPaCorrenteRaccordo_Leave);
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // txtPaCorrenteF3
            // 
            resources.ApplyResources(this.txtPaCorrenteF3, "txtPaCorrenteF3");
            this.txtPaCorrenteF3.Name = "txtPaCorrenteF3";
            this.txtPaCorrenteF3.Leave += new System.EventHandler(this.TxtPaCorrenteF3_Leave);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // txtPaRaccordoF1
            // 
            resources.ApplyResources(this.txtPaRaccordoF1, "txtPaRaccordoF1");
            this.txtPaRaccordoF1.Name = "txtPaRaccordoF1";
            this.txtPaRaccordoF1.Leave += new System.EventHandler(this.TxtPaRaccordoF1_Leave);
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // tbpPaPCStep3
            // 
            this.tbpPaPCStep3.Controls.Add(this.label19);
            this.tbpPaPCStep3.Controls.Add(this.txtPaTempoT3Max);
            resources.ApplyResources(this.tbpPaPCStep3, "tbpPaPCStep3");
            this.tbpPaPCStep3.Name = "tbpPaPCStep3";
            this.tbpPaPCStep3.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
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
            this.tbpPaPCMant.Controls.Add(this.label32);
            this.tbpPaPCMant.Controls.Add(this.txtPaMantDurataMax);
            this.tbpPaPCMant.Controls.Add(this.label33);
            this.tbpPaPCMant.Controls.Add(this.txtPaMantVmax);
            this.tbpPaPCMant.Controls.Add(this.label34);
            this.tbpPaPCMant.Controls.Add(this.txtPaMantVmin);
            this.tbpPaPCMant.Controls.Add(this.label35);
            this.tbpPaPCMant.Controls.Add(this.txtPaMantAttesa);
            this.tbpPaPCMant.Controls.Add(this.label36);
            resources.ApplyResources(this.tbpPaPCMant, "tbpPaPCMant");
            this.tbpPaPCMant.Name = "tbpPaPCMant";
            this.tbpPaPCMant.UseVisualStyleBackColor = true;
            // 
            // txtPaMantCorrente
            // 
            resources.ApplyResources(this.txtPaMantCorrente, "txtPaMantCorrente");
            this.txtPaMantCorrente.Name = "txtPaMantCorrente";
            // 
            // label32
            // 
            resources.ApplyResources(this.label32, "label32");
            this.label32.Name = "label32";
            // 
            // txtPaMantDurataMax
            // 
            resources.ApplyResources(this.txtPaMantDurataMax, "txtPaMantDurataMax");
            this.txtPaMantDurataMax.Name = "txtPaMantDurataMax";
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // txtPaMantVmax
            // 
            resources.ApplyResources(this.txtPaMantVmax, "txtPaMantVmax");
            this.txtPaMantVmax.Name = "txtPaMantVmax";
            this.txtPaMantVmax.Leave += new System.EventHandler(this.TxtPaMantVmax_Leave);
            // 
            // label34
            // 
            resources.ApplyResources(this.label34, "label34");
            this.label34.Name = "label34";
            // 
            // txtPaMantVmin
            // 
            resources.ApplyResources(this.txtPaMantVmin, "txtPaMantVmin");
            this.txtPaMantVmin.Name = "txtPaMantVmin";
            this.txtPaMantVmin.Leave += new System.EventHandler(this.TxtPaMantVmin_Leave);
            // 
            // label35
            // 
            resources.ApplyResources(this.label35, "label35");
            this.label35.Name = "label35";
            // 
            // txtPaMantAttesa
            // 
            resources.ApplyResources(this.txtPaMantAttesa, "txtPaMantAttesa");
            this.txtPaMantAttesa.Name = "txtPaMantAttesa";
            // 
            // label36
            // 
            resources.ApplyResources(this.label36, "label36");
            this.label36.Name = "label36";
            // 
            // tbpPaPCOpp
            // 
            this.tbpPaPCOpp.Controls.Add(this.lblPaOppPuntoVerde);
            this.tbpPaPCOpp.Controls.Add(this.ImgPaOppPuntoVerde);
            this.tbpPaPCOpp.Controls.Add(this.chkPaOppNotturno);
            this.tbpPaPCOpp.Controls.Add(this.rslPaOppFinestra);
            this.tbpPaPCOpp.Controls.Add(this.txtPaOppDurataMax);
            this.tbpPaPCOpp.Controls.Add(this.label50);
            this.tbpPaPCOpp.Controls.Add(this.txtPaOppCorrente);
            this.tbpPaPCOpp.Controls.Add(this.label49);
            this.tbpPaPCOpp.Controls.Add(this.txtPaOppVSoglia);
            this.tbpPaPCOpp.Controls.Add(this.label48);
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
            // label50
            // 
            resources.ApplyResources(this.label50, "label50");
            this.label50.Name = "label50";
            // 
            // txtPaOppCorrente
            // 
            resources.ApplyResources(this.txtPaOppCorrente, "txtPaOppCorrente");
            this.txtPaOppCorrente.Name = "txtPaOppCorrente";
            // 
            // label49
            // 
            resources.ApplyResources(this.label49, "label49");
            this.label49.Name = "label49";
            // 
            // txtPaOppVSoglia
            // 
            resources.ApplyResources(this.txtPaOppVSoglia, "txtPaOppVSoglia");
            this.txtPaOppVSoglia.Name = "txtPaOppVSoglia";
            // 
            // label48
            // 
            resources.ApplyResources(this.label48, "label48");
            this.label48.Name = "label48";
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
            this.tbpPaParSoglia.Controls.Add(this.label59);
            this.tbpPaParSoglia.Controls.Add(this.txtPaVMinStop);
            this.tbpPaParSoglia.Controls.Add(this.label42);
            this.tbpPaParSoglia.Controls.Add(this.txtPaBMSTempoAttesa);
            this.tbpPaParSoglia.Controls.Add(this.label156);
            this.tbpPaParSoglia.Controls.Add(this.txtPaBMSTempoErogazione);
            this.tbpPaParSoglia.Controls.Add(this.label157);
            this.tbpPaParSoglia.Controls.Add(this.chkPaRiarmaBms);
            this.tbpPaParSoglia.Controls.Add(this.chkPaAttivaRiarmoBms);
            this.tbpPaParSoglia.Controls.Add(this.txtPaCorrenteMassima);
            this.tbpPaParSoglia.Controls.Add(this.label25);
            this.tbpPaParSoglia.Controls.Add(this.txtPaVMaxRic);
            this.tbpPaParSoglia.Controls.Add(this.label28);
            this.tbpPaParSoglia.Controls.Add(this.txtPaVMinRic);
            this.tbpPaParSoglia.Controls.Add(this.label29);
            this.tbpPaParSoglia.Controls.Add(this.txtPaVLimite);
            this.tbpPaParSoglia.Controls.Add(this.label15);
            resources.ApplyResources(this.tbpPaParSoglia, "tbpPaParSoglia");
            this.tbpPaParSoglia.Name = "tbpPaParSoglia";
            this.tbpPaParSoglia.UseVisualStyleBackColor = true;
            // 
            // txtPaTempLimite
            // 
            resources.ApplyResources(this.txtPaTempLimite, "txtPaTempLimite");
            this.txtPaTempLimite.Name = "txtPaTempLimite";
            // 
            // label59
            // 
            resources.ApplyResources(this.label59, "label59");
            this.label59.Name = "label59";
            // 
            // txtPaVMinStop
            // 
            resources.ApplyResources(this.txtPaVMinStop, "txtPaVMinStop");
            this.txtPaVMinStop.Name = "txtPaVMinStop";
            // 
            // label42
            // 
            resources.ApplyResources(this.label42, "label42");
            this.label42.Name = "label42";
            // 
            // txtPaBMSTempoAttesa
            // 
            resources.ApplyResources(this.txtPaBMSTempoAttesa, "txtPaBMSTempoAttesa");
            this.txtPaBMSTempoAttesa.Name = "txtPaBMSTempoAttesa";
            // 
            // label156
            // 
            resources.ApplyResources(this.label156, "label156");
            this.label156.Name = "label156";
            // 
            // txtPaBMSTempoErogazione
            // 
            resources.ApplyResources(this.txtPaBMSTempoErogazione, "txtPaBMSTempoErogazione");
            this.txtPaBMSTempoErogazione.Name = "txtPaBMSTempoErogazione";
            // 
            // label157
            // 
            resources.ApplyResources(this.label157, "label157");
            this.label157.Name = "label157";
            // 
            // chkPaRiarmaBms
            // 
            resources.ApplyResources(this.chkPaRiarmaBms, "chkPaRiarmaBms");
            this.chkPaRiarmaBms.Name = "chkPaRiarmaBms";
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
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // txtPaVMaxRic
            // 
            resources.ApplyResources(this.txtPaVMaxRic, "txtPaVMaxRic");
            this.txtPaVMaxRic.Name = "txtPaVMaxRic";
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // txtPaVMinRic
            // 
            resources.ApplyResources(this.txtPaVMinRic, "txtPaVMinRic");
            this.txtPaVMinRic.Name = "txtPaVMinRic";
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // txtPaVLimite
            // 
            resources.ApplyResources(this.txtPaVLimite, "txtPaVLimite");
            this.txtPaVLimite.Name = "txtPaVLimite";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
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
            // tbpPaListaProfili
            // 
            this.tbpPaListaProfili.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tbpPaListaProfili, "tbpPaListaProfili");
            this.tbpPaListaProfili.Controls.Add(this.btnPaCancellaSelezionati);
            this.tbpPaListaProfili.Controls.Add(this.grbPaGeneraFile);
            this.tbpPaListaProfili.Controls.Add(this.btnPaAttivaConfigurazione);
            this.tbpPaListaProfili.Controls.Add(this.flwPaListaConfigurazioni);
            this.tbpPaListaProfili.Controls.Add(this.btnPaCaricaListaProfili);
            this.tbpPaListaProfili.Name = "tbpPaListaProfili";
            this.tbpPaListaProfili.UseVisualStyleBackColor = true;
            // 
            // btnPaCancellaSelezionati
            // 
            resources.ApplyResources(this.btnPaCancellaSelezionati, "btnPaCancellaSelezionati");
            this.btnPaCancellaSelezionati.Name = "btnPaCancellaSelezionati";
            this.btnPaCancellaSelezionati.UseVisualStyleBackColor = true;
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
            // 
            // btnPaCaricaListaProfili
            // 
            resources.ApplyResources(this.btnPaCaricaListaProfili, "btnPaCaricaListaProfili");
            this.btnPaCaricaListaProfili.Name = "btnPaCaricaListaProfili";
            this.btnPaCaricaListaProfili.UseVisualStyleBackColor = true;
            this.btnPaCaricaListaProfili.Click += new System.EventHandler(this.btnCicliCaricaLista_Click);
            // 
            // tbpPaCfgAvanzate
            // 
            this.tbpPaCfgAvanzate.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tbpPaCfgAvanzate, "tbpPaCfgAvanzate");
            this.tbpPaCfgAvanzate.Controls.Add(this.grbVarParametriSig);
            this.tbpPaCfgAvanzate.Name = "tbpPaCfgAvanzate";
            this.tbpPaCfgAvanzate.UseVisualStyleBackColor = true;
            // 
            // grbVarParametriSig
            // 
            this.grbVarParametriSig.BackColor = System.Drawing.Color.White;
            this.grbVarParametriSig.Controls.Add(this.btnFSerVerificaOC);
            this.grbVarParametriSig.Controls.Add(this.chkFSerEchoOC);
            this.grbVarParametriSig.Controls.Add(this.btnFSerImpostaOC);
            this.grbVarParametriSig.Controls.Add(this.label278);
            this.grbVarParametriSig.Controls.Add(this.cmbFSerBaudrateOC);
            resources.ApplyResources(this.grbVarParametriSig, "grbVarParametriSig");
            this.grbVarParametriSig.Name = "grbVarParametriSig";
            this.grbVarParametriSig.TabStop = false;
            // 
            // btnFSerVerificaOC
            // 
            resources.ApplyResources(this.btnFSerVerificaOC, "btnFSerVerificaOC");
            this.btnFSerVerificaOC.Name = "btnFSerVerificaOC";
            this.btnFSerVerificaOC.UseVisualStyleBackColor = true;
            this.btnFSerVerificaOC.Click += new System.EventHandler(this.btnFSerVerificaOC_Click);
            // 
            // chkFSerEchoOC
            // 
            resources.ApplyResources(this.chkFSerEchoOC, "chkFSerEchoOC");
            this.chkFSerEchoOC.ForeColor = System.Drawing.Color.Red;
            this.chkFSerEchoOC.Name = "chkFSerEchoOC";
            this.chkFSerEchoOC.UseVisualStyleBackColor = true;
            // 
            // btnFSerImpostaOC
            // 
            resources.ApplyResources(this.btnFSerImpostaOC, "btnFSerImpostaOC");
            this.btnFSerImpostaOC.Name = "btnFSerImpostaOC";
            this.btnFSerImpostaOC.UseVisualStyleBackColor = true;
            // 
            // label278
            // 
            resources.ApplyResources(this.label278, "label278");
            this.label278.Name = "label278";
            // 
            // cmbFSerBaudrateOC
            // 
            resources.ApplyResources(this.cmbFSerBaudrateOC, "cmbFSerBaudrateOC");
            this.cmbFSerBaudrateOC.FormattingEnabled = true;
            this.cmbFSerBaudrateOC.Name = "cmbFSerBaudrateOC";
            // 
            // lblPaTitoloLista
            // 
            resources.ApplyResources(this.lblPaTitoloLista, "lblPaTitoloLista");
            this.lblPaTitoloLista.Name = "lblPaTitoloLista";
            // 
            // tabCb04
            // 
            this.tabCb04.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tabCb04, "tabCb04");
            this.tabCb04.Controls.Add(this.flvCicliListaCariche);
            this.tabCb04.Controls.Add(this.grbCicli);
            this.tabCb04.Name = "tabCb04";
            // 
            // flvCicliListaCariche
            // 
            this.flvCicliListaCariche.CellEditUseWholeCell = false;
            this.flvCicliListaCariche.HideSelection = false;
            resources.ApplyResources(this.flvCicliListaCariche, "flvCicliListaCariche");
            this.flvCicliListaCariche.Name = "flvCicliListaCariche";
            this.flvCicliListaCariche.ShowGroups = false;
            this.flvCicliListaCariche.UseCompatibleStateImageBehavior = false;
            this.flvCicliListaCariche.View = System.Windows.Forms.View.Details;
            this.flvCicliListaCariche.VirtualMode = true;
            this.flvCicliListaCariche.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.flvCicliListaCariche_FormatRow);
            this.flvCicliListaCariche.SelectedIndexChanged += new System.EventHandler(this.flvCicliListaCariche_SelectedIndexChanged);
            this.flvCicliListaCariche.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.flvCicliListaCariche_MouseDoubleClick);
            // 
            // grbCicli
            // 
            this.grbCicli.BackColor = System.Drawing.Color.White;
            this.grbCicli.Controls.Add(this.btnCicliCaricaCont);
            this.grbCicli.Controls.Add(this.btnCicliCaricaArea);
            this.grbCicli.Controls.Add(this.btnCicliMostraBrevi);
            this.grbCicli.Controls.Add(this.btnCicliCaricaBrevi);
            this.grbCicli.Controls.Add(this.chkCicliCaricaBrevi);
            this.grbCicli.Controls.Add(this.label223);
            this.grbCicli.Controls.Add(this.txtCicliNumRecord);
            this.grbCicli.Controls.Add(this.label209);
            this.grbCicli.Controls.Add(this.txtCicliAddrPrmo);
            this.grbCicli.Controls.Add(this.btnCicliCaricaLista);
            this.grbCicli.Controls.Add(this.btnCicliVuotaLista);
            resources.ApplyResources(this.grbCicli, "grbCicli");
            this.grbCicli.Name = "grbCicli";
            this.grbCicli.TabStop = false;
            // 
            // btnCicliCaricaCont
            // 
            resources.ApplyResources(this.btnCicliCaricaCont, "btnCicliCaricaCont");
            this.btnCicliCaricaCont.Name = "btnCicliCaricaCont";
            this.btnCicliCaricaCont.UseVisualStyleBackColor = true;
            this.btnCicliCaricaCont.Click += new System.EventHandler(this.btnCicliCaricaCont_Click);
            // 
            // btnCicliCaricaArea
            // 
            resources.ApplyResources(this.btnCicliCaricaArea, "btnCicliCaricaArea");
            this.btnCicliCaricaArea.Name = "btnCicliCaricaArea";
            this.btnCicliCaricaArea.UseVisualStyleBackColor = true;
            this.btnCicliCaricaArea.Click += new System.EventHandler(this.btnCicliCaricaArea_Click);
            // 
            // btnCicliMostraBrevi
            // 
            resources.ApplyResources(this.btnCicliMostraBrevi, "btnCicliMostraBrevi");
            this.btnCicliMostraBrevi.Name = "btnCicliMostraBrevi";
            this.btnCicliMostraBrevi.UseVisualStyleBackColor = true;
            this.btnCicliMostraBrevi.Click += new System.EventHandler(this.BtnCicliMostraBrevi_Click);
            // 
            // btnCicliCaricaBrevi
            // 
            resources.ApplyResources(this.btnCicliCaricaBrevi, "btnCicliCaricaBrevi");
            this.btnCicliCaricaBrevi.Name = "btnCicliCaricaBrevi";
            this.btnCicliCaricaBrevi.UseVisualStyleBackColor = true;
            this.btnCicliCaricaBrevi.Click += new System.EventHandler(this.btnCicliCaricaBrevi_Click);
            // 
            // chkCicliCaricaBrevi
            // 
            resources.ApplyResources(this.chkCicliCaricaBrevi, "chkCicliCaricaBrevi");
            this.chkCicliCaricaBrevi.Name = "chkCicliCaricaBrevi";
            this.chkCicliCaricaBrevi.UseVisualStyleBackColor = true;
            // 
            // label223
            // 
            resources.ApplyResources(this.label223, "label223");
            this.label223.Name = "label223";
            // 
            // txtCicliNumRecord
            // 
            resources.ApplyResources(this.txtCicliNumRecord, "txtCicliNumRecord");
            this.txtCicliNumRecord.Name = "txtCicliNumRecord";
            // 
            // label209
            // 
            resources.ApplyResources(this.label209, "label209");
            this.label209.Name = "label209";
            // 
            // txtCicliAddrPrmo
            // 
            resources.ApplyResources(this.txtCicliAddrPrmo, "txtCicliAddrPrmo");
            this.txtCicliAddrPrmo.Name = "txtCicliAddrPrmo";
            // 
            // btnCicliCaricaLista
            // 
            resources.ApplyResources(this.btnCicliCaricaLista, "btnCicliCaricaLista");
            this.btnCicliCaricaLista.Name = "btnCicliCaricaLista";
            this.btnCicliCaricaLista.UseVisualStyleBackColor = true;
            this.btnCicliCaricaLista.Click += new System.EventHandler(this.btnCicliCaricaLista_Click);
            // 
            // btnCicliVuotaLista
            // 
            resources.ApplyResources(this.btnCicliVuotaLista, "btnCicliVuotaLista");
            this.btnCicliVuotaLista.Name = "btnCicliVuotaLista";
            this.btnCicliVuotaLista.UseVisualStyleBackColor = true;
            this.btnCicliVuotaLista.Click += new System.EventHandler(this.btnCicliVuotaLista_Click);
            // 
            // tabGenerale
            // 
            this.tabGenerale.BackColor = System.Drawing.Color.LightYellow;
            this.tabGenerale.BackgroundImage = global::PannelloCharger.Properties.Resources.Base_fondino_Y;
            resources.ApplyResources(this.tabGenerale, "tabGenerale");
            this.tabGenerale.Controls.Add(this.pictureBox1);
            this.tabGenerale.Controls.Add(this.btnSalveCliente);
            this.tabGenerale.Controls.Add(this.btnCaricaCliente);
            this.tabGenerale.Controls.Add(this.btnGenResetBoard);
            this.tabGenerale.Controls.Add(this.btnGenAzzzeraContatoriTot);
            this.tabGenerale.Controls.Add(this.btnGenAzzzeraContatori);
            this.tabGenerale.Controls.Add(this.btnCaricaContatori);
            this.tabGenerale.Controls.Add(this.grbMainContatori);
            this.tabGenerale.Controls.Add(this.GrbMainDatiApparato);
            this.tabGenerale.Controls.Add(this.grbDatiCliente);
            this.tabGenerale.Name = "tabGenerale";
            this.tabGenerale.Click += new System.EventHandler(this.tabCb01_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // btnSalveCliente
            // 
            resources.ApplyResources(this.btnSalveCliente, "btnSalveCliente");
            this.btnSalveCliente.Name = "btnSalveCliente";
            this.btnSalveCliente.UseVisualStyleBackColor = true;
            this.btnSalveCliente.Click += new System.EventHandler(this.btnSalveCliente_Click);
            // 
            // btnCaricaCliente
            // 
            resources.ApplyResources(this.btnCaricaCliente, "btnCaricaCliente");
            this.btnCaricaCliente.Name = "btnCaricaCliente";
            this.btnCaricaCliente.UseVisualStyleBackColor = true;
            this.btnCaricaCliente.Click += new System.EventHandler(this.btnCaricaCliente_Click);
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
            // grbMainContatori
            // 
            this.grbMainContatori.BackColor = System.Drawing.Color.White;
            this.grbMainContatori.Controls.Add(this.txtContCaricheOpportunity);
            this.grbMainContatori.Controls.Add(this.label44);
            this.grbMainContatori.Controls.Add(this.label43);
            this.grbMainContatori.Controls.Add(this.txtContNumProgrammazioni);
            this.grbMainContatori.Controls.Add(this.label222);
            this.grbMainContatori.Controls.Add(this.label221);
            this.grbMainContatori.Controls.Add(this.txtContPntNextBreve);
            this.grbMainContatori.Controls.Add(this.label220);
            this.grbMainContatori.Controls.Add(this.txtContNumCancellazioni);
            this.grbMainContatori.Controls.Add(this.label219);
            this.grbMainContatori.Controls.Add(this.label217);
            this.grbMainContatori.Controls.Add(this.txtContDtUltimaCanc);
            this.grbMainContatori.Controls.Add(this.txtContPntNextCarica);
            this.grbMainContatori.Controls.Add(this.txtContBreviSalvati);
            this.grbMainContatori.Controls.Add(this.txtContCaricheSalvate);
            this.grbMainContatori.Controls.Add(this.txtContCaricheOver9);
            this.grbMainContatori.Controls.Add(this.txtContCariche6to9);
            this.grbMainContatori.Controls.Add(this.txtContCariche3to6);
            this.grbMainContatori.Controls.Add(this.txtContCaricheUnder3);
            this.grbMainContatori.Controls.Add(this.label208);
            this.grbMainContatori.Controls.Add(this.txtContCaricheStrappo);
            this.grbMainContatori.Controls.Add(this.label210);
            this.grbMainContatori.Controls.Add(this.txtContCaricheStop);
            this.grbMainContatori.Controls.Add(this.label211);
            this.grbMainContatori.Controls.Add(this.label212);
            this.grbMainContatori.Controls.Add(this.label213);
            this.grbMainContatori.Controls.Add(this.label214);
            this.grbMainContatori.Controls.Add(this.label215);
            this.grbMainContatori.Controls.Add(this.txtContCaricheTotali);
            this.grbMainContatori.Controls.Add(this.label216);
            this.grbMainContatori.Controls.Add(this.txtContDtPrimaCarica);
            this.grbMainContatori.Controls.Add(this.label218);
            resources.ApplyResources(this.grbMainContatori, "grbMainContatori");
            this.grbMainContatori.Name = "grbMainContatori";
            this.grbMainContatori.TabStop = false;
            // 
            // txtContCaricheOpportunity
            // 
            resources.ApplyResources(this.txtContCaricheOpportunity, "txtContCaricheOpportunity");
            this.txtContCaricheOpportunity.Name = "txtContCaricheOpportunity";
            // 
            // label44
            // 
            resources.ApplyResources(this.label44, "label44");
            this.label44.ForeColor = System.Drawing.Color.Red;
            this.label44.Name = "label44";
            // 
            // label43
            // 
            resources.ApplyResources(this.label43, "label43");
            this.label43.Name = "label43";
            // 
            // txtContNumProgrammazioni
            // 
            resources.ApplyResources(this.txtContNumProgrammazioni, "txtContNumProgrammazioni");
            this.txtContNumProgrammazioni.Name = "txtContNumProgrammazioni";
            // 
            // label222
            // 
            resources.ApplyResources(this.label222, "label222");
            this.label222.Name = "label222";
            // 
            // label221
            // 
            resources.ApplyResources(this.label221, "label221");
            this.label221.Name = "label221";
            // 
            // txtContPntNextBreve
            // 
            resources.ApplyResources(this.txtContPntNextBreve, "txtContPntNextBreve");
            this.txtContPntNextBreve.Name = "txtContPntNextBreve";
            // 
            // label220
            // 
            resources.ApplyResources(this.label220, "label220");
            this.label220.Name = "label220";
            // 
            // txtContNumCancellazioni
            // 
            resources.ApplyResources(this.txtContNumCancellazioni, "txtContNumCancellazioni");
            this.txtContNumCancellazioni.Name = "txtContNumCancellazioni";
            // 
            // label219
            // 
            resources.ApplyResources(this.label219, "label219");
            this.label219.Name = "label219";
            // 
            // label217
            // 
            resources.ApplyResources(this.label217, "label217");
            this.label217.Name = "label217";
            // 
            // txtContDtUltimaCanc
            // 
            resources.ApplyResources(this.txtContDtUltimaCanc, "txtContDtUltimaCanc");
            this.txtContDtUltimaCanc.Name = "txtContDtUltimaCanc";
            // 
            // txtContPntNextCarica
            // 
            resources.ApplyResources(this.txtContPntNextCarica, "txtContPntNextCarica");
            this.txtContPntNextCarica.Name = "txtContPntNextCarica";
            // 
            // txtContBreviSalvati
            // 
            resources.ApplyResources(this.txtContBreviSalvati, "txtContBreviSalvati");
            this.txtContBreviSalvati.Name = "txtContBreviSalvati";
            // 
            // txtContCaricheSalvate
            // 
            resources.ApplyResources(this.txtContCaricheSalvate, "txtContCaricheSalvate");
            this.txtContCaricheSalvate.Name = "txtContCaricheSalvate";
            // 
            // txtContCaricheOver9
            // 
            resources.ApplyResources(this.txtContCaricheOver9, "txtContCaricheOver9");
            this.txtContCaricheOver9.Name = "txtContCaricheOver9";
            // 
            // txtContCariche6to9
            // 
            resources.ApplyResources(this.txtContCariche6to9, "txtContCariche6to9");
            this.txtContCariche6to9.Name = "txtContCariche6to9";
            // 
            // txtContCariche3to6
            // 
            resources.ApplyResources(this.txtContCariche3to6, "txtContCariche3to6");
            this.txtContCariche3to6.Name = "txtContCariche3to6";
            // 
            // txtContCaricheUnder3
            // 
            resources.ApplyResources(this.txtContCaricheUnder3, "txtContCaricheUnder3");
            this.txtContCaricheUnder3.Name = "txtContCaricheUnder3";
            // 
            // label208
            // 
            resources.ApplyResources(this.label208, "label208");
            this.label208.Name = "label208";
            // 
            // txtContCaricheStrappo
            // 
            resources.ApplyResources(this.txtContCaricheStrappo, "txtContCaricheStrappo");
            this.txtContCaricheStrappo.Name = "txtContCaricheStrappo";
            // 
            // label210
            // 
            resources.ApplyResources(this.label210, "label210");
            this.label210.Name = "label210";
            // 
            // txtContCaricheStop
            // 
            resources.ApplyResources(this.txtContCaricheStop, "txtContCaricheStop");
            this.txtContCaricheStop.Name = "txtContCaricheStop";
            // 
            // label211
            // 
            resources.ApplyResources(this.label211, "label211");
            this.label211.Name = "label211";
            // 
            // label212
            // 
            resources.ApplyResources(this.label212, "label212");
            this.label212.Name = "label212";
            // 
            // label213
            // 
            resources.ApplyResources(this.label213, "label213");
            this.label213.Name = "label213";
            // 
            // label214
            // 
            resources.ApplyResources(this.label214, "label214");
            this.label214.Name = "label214";
            // 
            // label215
            // 
            resources.ApplyResources(this.label215, "label215");
            this.label215.Name = "label215";
            // 
            // txtContCaricheTotali
            // 
            resources.ApplyResources(this.txtContCaricheTotali, "txtContCaricheTotali");
            this.txtContCaricheTotali.Name = "txtContCaricheTotali";
            // 
            // label216
            // 
            resources.ApplyResources(this.label216, "label216");
            this.label216.Name = "label216";
            // 
            // txtContDtPrimaCarica
            // 
            resources.ApplyResources(this.txtContDtPrimaCarica, "txtContDtPrimaCarica");
            this.txtContDtPrimaCarica.Name = "txtContDtPrimaCarica";
            // 
            // label218
            // 
            resources.ApplyResources(this.label218, "label218");
            this.label218.Name = "label218";
            // 
            // GrbMainDatiApparato
            // 
            this.GrbMainDatiApparato.BackColor = System.Drawing.Color.White;
            this.GrbMainDatiApparato.Controls.Add(this.txtGenSerialeZVT);
            this.GrbMainDatiApparato.Controls.Add(this.label151);
            this.GrbMainDatiApparato.Controls.Add(this.txtGenCorrenteMax);
            this.GrbMainDatiApparato.Controls.Add(this.label143);
            this.GrbMainDatiApparato.Controls.Add(this.txtGenTensioneMax);
            this.GrbMainDatiApparato.Controls.Add(this.label142);
            this.GrbMainDatiApparato.Controls.Add(this.txtGenModello);
            this.GrbMainDatiApparato.Controls.Add(this.label141);
            this.GrbMainDatiApparato.Controls.Add(this.txtGenRevFwDisplay);
            this.GrbMainDatiApparato.Controls.Add(this.label140);
            this.GrbMainDatiApparato.Controls.Add(this.textBox34);
            this.GrbMainDatiApparato.Controls.Add(this.label139);
            this.GrbMainDatiApparato.Controls.Add(this.textBox12);
            this.GrbMainDatiApparato.Controls.Add(this.label138);
            this.GrbMainDatiApparato.Controls.Add(this.txtGenRevHwDisplay);
            this.GrbMainDatiApparato.Controls.Add(this.label7);
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
            // txtGenSerialeZVT
            // 
            resources.ApplyResources(this.txtGenSerialeZVT, "txtGenSerialeZVT");
            this.txtGenSerialeZVT.Name = "txtGenSerialeZVT";
            // 
            // label151
            // 
            resources.ApplyResources(this.label151, "label151");
            this.label151.Name = "label151";
            // 
            // txtGenCorrenteMax
            // 
            resources.ApplyResources(this.txtGenCorrenteMax, "txtGenCorrenteMax");
            this.txtGenCorrenteMax.Name = "txtGenCorrenteMax";
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
            // 
            // label142
            // 
            resources.ApplyResources(this.label142, "label142");
            this.label142.Name = "label142";
            // 
            // txtGenModello
            // 
            resources.ApplyResources(this.txtGenModello, "txtGenModello");
            this.txtGenModello.Name = "txtGenModello";
            // 
            // label141
            // 
            resources.ApplyResources(this.label141, "label141");
            this.label141.Name = "label141";
            // 
            // txtGenRevFwDisplay
            // 
            resources.ApplyResources(this.txtGenRevFwDisplay, "txtGenRevFwDisplay");
            this.txtGenRevFwDisplay.Name = "txtGenRevFwDisplay";
            // 
            // label140
            // 
            resources.ApplyResources(this.label140, "label140");
            this.label140.Name = "label140";
            // 
            // textBox34
            // 
            resources.ApplyResources(this.textBox34, "textBox34");
            this.textBox34.Name = "textBox34";
            // 
            // label139
            // 
            resources.ApplyResources(this.label139, "label139");
            this.label139.Name = "label139";
            // 
            // textBox12
            // 
            resources.ApplyResources(this.textBox12, "textBox12");
            this.textBox12.Name = "textBox12";
            // 
            // label138
            // 
            resources.ApplyResources(this.label138, "label138");
            this.label138.Name = "label138";
            // 
            // txtGenRevHwDisplay
            // 
            resources.ApplyResources(this.txtGenRevHwDisplay, "txtGenRevHwDisplay");
            this.txtGenRevHwDisplay.Name = "txtGenRevHwDisplay";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // txtGenIdApparato
            // 
            resources.ApplyResources(this.txtGenIdApparato, "txtGenIdApparato");
            this.txtGenIdApparato.Name = "txtGenIdApparato";
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
            // 
            // txtGenMatricola
            // 
            resources.ApplyResources(this.txtGenMatricola, "txtGenMatricola");
            this.txtGenMatricola.Name = "txtGenMatricola";
            // 
            // lblMatricola
            // 
            resources.ApplyResources(this.lblMatricola, "lblMatricola");
            this.lblMatricola.Name = "lblMatricola";
            // 
            // grbDatiCliente
            // 
            this.grbDatiCliente.BackColor = System.Drawing.Color.White;
            this.grbDatiCliente.Controls.Add(this.txtCliNomeIntLL);
            this.grbDatiCliente.Controls.Add(this.label45);
            this.grbDatiCliente.Controls.Add(this.label254);
            this.grbDatiCliente.Controls.Add(this.txtCliCodiceLL);
            this.grbDatiCliente.Controls.Add(this.txtCliNote);
            this.grbDatiCliente.Controls.Add(this.txtCliDescrizione);
            this.grbDatiCliente.Controls.Add(this.lblCliIdBatteria);
            this.grbDatiCliente.Controls.Add(this.txtCliente);
            this.grbDatiCliente.Controls.Add(this.lbNoteCliente);
            this.grbDatiCliente.Controls.Add(this.lblCliCliente);
            resources.ApplyResources(this.grbDatiCliente, "grbDatiCliente");
            this.grbDatiCliente.Name = "grbDatiCliente";
            this.grbDatiCliente.TabStop = false;
            // 
            // txtCliNomeIntLL
            // 
            resources.ApplyResources(this.txtCliNomeIntLL, "txtCliNomeIntLL");
            this.txtCliNomeIntLL.Name = "txtCliNomeIntLL";
            // 
            // label45
            // 
            resources.ApplyResources(this.label45, "label45");
            this.label45.Name = "label45";
            // 
            // label254
            // 
            resources.ApplyResources(this.label254, "label254");
            this.label254.Name = "label254";
            // 
            // txtCliCodiceLL
            // 
            resources.ApplyResources(this.txtCliCodiceLL, "txtCliCodiceLL");
            this.txtCliCodiceLL.Name = "txtCliCodiceLL";
            // 
            // txtCliNote
            // 
            resources.ApplyResources(this.txtCliNote, "txtCliNote");
            this.txtCliNote.Name = "txtCliNote";
            // 
            // txtCliDescrizione
            // 
            resources.ApplyResources(this.txtCliDescrizione, "txtCliDescrizione");
            this.txtCliDescrizione.Name = "txtCliDescrizione";
            // 
            // lblCliIdBatteria
            // 
            resources.ApplyResources(this.lblCliIdBatteria, "lblCliIdBatteria");
            this.lblCliIdBatteria.Name = "lblCliIdBatteria";
            // 
            // txtCliente
            // 
            resources.ApplyResources(this.txtCliente, "txtCliente");
            this.txtCliente.Name = "txtCliente";
            // 
            // lbNoteCliente
            // 
            resources.ApplyResources(this.lbNoteCliente, "lbNoteCliente");
            this.lbNoteCliente.Name = "lbNoteCliente";
            // 
            // lblCliCliente
            // 
            resources.ApplyResources(this.lblCliCliente, "lblCliCliente");
            this.lblCliCliente.Name = "lblCliCliente";
            // 
            // tabCaricaBatterie
            // 
            this.tabCaricaBatterie.Controls.Add(this.tabGenerale);
            this.tabCaricaBatterie.Controls.Add(this.tabCb04);
            this.tabCaricaBatterie.Controls.Add(this.tabProfiloAttuale);
            this.tabCaricaBatterie.Controls.Add(this.tabOrologio);
            this.tabCaricaBatterie.Controls.Add(this.tabInizializzazione);
            this.tabCaricaBatterie.Controls.Add(this.tabMemRead);
            this.tabCaricaBatterie.Controls.Add(this.tbpFirmware);
            this.tabCaricaBatterie.Controls.Add(this.tbpProxySig60);
            this.tabCaricaBatterie.Controls.Add(this.tabCb02);
            this.tabCaricaBatterie.Controls.Add(this.tabMonitor);
            resources.ApplyResources(this.tabCaricaBatterie, "tabCaricaBatterie");
            this.tabCaricaBatterie.Name = "tabCaricaBatterie";
            this.tabCaricaBatterie.SelectedIndex = 0;
            this.tabCaricaBatterie.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabCaricaBatterie_Selected);
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
            // frmSuperCharger
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuBar;
            this.Controls.Add(this.tabCaricaBatterie);
            this.Name = "frmSuperCharger";
            this.ShowIcon = false;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCaricabatterie_FormClosed);
            this.Load += new System.EventHandler(this.frmCaricabatterie_Load);
            this.Resize += new System.EventHandler(this.frmCaricabatterie_Resize);
            this.tabMonitor.ResumeLayout(false);
            this.tabMonitor.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvwLettureParametri)).EndInit();
            this.grbVariabiliImmediate.ResumeLayout(false);
            this.grbVariabiliImmediate.PerformLayout();
            this.tabCb02.ResumeLayout(false);
            this.grbCavi.ResumeLayout(false);
            this.grbCavi.PerformLayout();
            this.tbpProxySig60.ResumeLayout(false);
            this.tbpProxySig60.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.grbStratStepCorrente.ResumeLayout(false);
            this.grbStratStepCorrente.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.pnlStratContatoriCarica.ResumeLayout(false);
            this.pnlStratContatoriCarica.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.grbStratComandiTest.ResumeLayout(false);
            this.tbpFirmware.ResumeLayout(false);
            this.grbFwAttivazioneArea.ResumeLayout(false);
            this.grbFWPreparaFile.ResumeLayout(false);
            this.grbFWPreparaFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvwFWInFileStruct)).EndInit();
            this.grbFWArea2.ResumeLayout(false);
            this.grbFWArea2.PerformLayout();
            this.GrbFWArea1.ResumeLayout(false);
            this.GrbFWArea1.PerformLayout();
            this.grbFWAggiornamento.ResumeLayout(false);
            this.grbFWAggiornamento.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flwFWFileLLFStruct)).EndInit();
            this.grbStatoFirmware.ResumeLayout(false);
            this.grbStatoFirmware.PerformLayout();
            this.tabMemRead.ResumeLayout(false);
            this.tabMemRead.PerformLayout();
            this.grbMemTestLetture.ResumeLayout(false);
            this.grbMemTestLetture.PerformLayout();
            this.grbMemAzzeraLogger.ResumeLayout(false);
            this.grbMemAzzeraLogger.PerformLayout();
            this.grbMemCaricaLogger.ResumeLayout(false);
            this.grbMemCaricaLogger.PerformLayout();
            this.grbMemSalvaLogger.ResumeLayout(false);
            this.grbMemSalvaLogger.PerformLayout();
            this.grbMemCancFisica.ResumeLayout(false);
            this.grbMemCancFisica.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.grbMemScrittura.ResumeLayout(false);
            this.grbMemScrittura.PerformLayout();
            this.grbMemCancellazione.ResumeLayout(false);
            this.grbMemCancellazione.PerformLayout();
            this.grbMemLettura.ResumeLayout(false);
            this.grbMemLettura.PerformLayout();
            this.tabInizializzazione.ResumeLayout(false);
            this.grbConnessioni.ResumeLayout(false);
            this.grbConnessioni.PerformLayout();
            this.grbGenBaudrate.ResumeLayout(false);
            this.grbGenBaudrate.PerformLayout();
            this.grbInitLimiti.ResumeLayout(false);
            this.grbInitLimiti.PerformLayout();
            this.groupBox11.ResumeLayout(false);
            this.groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitBrdANomModulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitBrdVNomModulo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitBrdNumModuli)).EndInit();
            this.grbInitCalibrazione.ResumeLayout(false);
            this.grbInitCalibrazione.PerformLayout();
            this.grbInitDatiBase.ResumeLayout(false);
            this.grbInitDatiBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtInitAMax)).EndInit();
            this.tabOrologio.ResumeLayout(false);
            this.grbCalData.ResumeLayout(false);
            this.grbCalData.PerformLayout();
            this.grbAccensione.ResumeLayout(false);
            this.grbAccensione.PerformLayout();
            this.grbOraCorrente.ResumeLayout(false);
            this.grbOraCorrente.PerformLayout();
            this.tabProfiloAttuale.ResumeLayout(false);
            this.tabProfiloAttuale.PerformLayout();
            this.tbcPaSottopagina.ResumeLayout(false);
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
            this.tbpPaListaProfili.ResumeLayout(false);
            this.grbPaGeneraFile.ResumeLayout(false);
            this.grbPaGeneraFile.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flwPaListaConfigurazioni)).EndInit();
            this.tbpPaCfgAvanzate.ResumeLayout(false);
            this.grbVarParametriSig.ResumeLayout(false);
            this.grbVarParametriSig.PerformLayout();
            this.tabCb04.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.flvCicliListaCariche)).EndInit();
            this.grbCicli.ResumeLayout(false);
            this.grbCicli.PerformLayout();
            this.tabGenerale.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grbMainContatori.ResumeLayout(false);
            this.grbMainContatori.PerformLayout();
            this.GrbMainDatiApparato.ResumeLayout(false);
            this.GrbMainDatiApparato.PerformLayout();
            this.grbDatiCliente.ResumeLayout(false);
            this.grbDatiCliente.PerformLayout();
            this.tabCaricaBatterie.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrLetturaAutomatica;
        private System.Windows.Forms.SaveFileDialog sfdExportDati;
        private System.Windows.Forms.OpenFileDialog sfdImportDati;
        private System.Windows.Forms.TabPage tabMonitor;
        private System.Windows.Forms.GroupBox groupBox6;
        public System.Windows.Forms.Button txtVarGeneraExcel;
        private System.Windows.Forms.Button btnVarFilesearch;
        private System.Windows.Forms.TextBox txtVarFileCicli;
        private System.Windows.Forms.CheckBox chkParRegistraLetture;
        private BrightIdeasSoftware.FastObjectListView flvwLettureParametri;
        private System.Windows.Forms.TextBox txtParIntervalloLettura;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.CheckBox chkParLetturaAuto;
        private System.Windows.Forms.CheckBox chkDatiDiretti;
        private System.Windows.Forms.Button btnLeggiVariabili;
        private System.Windows.Forms.GroupBox grbVariabiliImmediate;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.TextBox txtVarTempoTrascorso;
        private System.Windows.Forms.TextBox txtVarMemProgrammed;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.TextBox txtVarIbatt;
        private System.Windows.Forms.Label lblVarVBatt;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.TextBox txtVarAhCarica;
        private System.Windows.Forms.TextBox txtVarVBatt;
        private System.Windows.Forms.TabPage tabCb02;
        private System.Windows.Forms.GroupBox grbCavi;
        private System.Windows.Forms.TextBox txtEsitoVerCavi;
        private System.Windows.Forms.Label lblEsito;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPotenzaDispersa;
        private System.Windows.Forms.Label lblPotenzaDispersa;
        private System.Windows.Forms.Button btnVerificaCavi;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.Label lblCaviSelVerifica;
        private System.Windows.Forms.ComboBox cmbSezioneProlunga;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblProlunga;
        private System.Windows.Forms.ComboBox cmbSezioneCavo;
        private System.Windows.Forms.Label lblSezioneCavo;
        private System.Windows.Forms.TextBox txtLunghezzaCavo;
        private System.Windows.Forms.Label lblLunghezzaCavo;
        private System.Windows.Forms.Label lblCavoCaricabatterie;
        private System.Windows.Forms.TabPage tbpProxySig60;
        private System.Windows.Forms.Label label176;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkStratLLRabb;
        private System.Windows.Forms.TextBox txtStratLLAmax;
        private System.Windows.Forms.Label label155;
        private System.Windows.Forms.TextBox txtStratLLVmax;
        private System.Windows.Forms.Label label154;
        private System.Windows.Forms.TextBox txtStratLLVmin;
        private System.Windows.Forms.Label label153;
        private System.Windows.Forms.Label label200;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.GroupBox grbStratStepCorrente;
        private System.Windows.Forms.Label label206;
        private System.Windows.Forms.TextBox txtStratCurrStepTipo;
        private System.Windows.Forms.Label label205;
        private System.Windows.Forms.TextBox txtStratCurrStepRipetizioni;
        private System.Windows.Forms.Label label203;
        private System.Windows.Forms.TextBox txtStratCurrStepTon;
        private System.Windows.Forms.TextBox txtStratCurrStepToff;
        private System.Windows.Forms.Label label204;
        private System.Windows.Forms.Label label194;
        private System.Windows.Forms.TextBox txtStratCurrStepAh;
        private System.Windows.Forms.Label label191;
        private System.Windows.Forms.TextBox txtStratCurrStepVmax;
        private System.Windows.Forms.TextBox txtStratCurrStepVmin;
        private System.Windows.Forms.Label label192;
        private System.Windows.Forms.Label label190;
        private System.Windows.Forms.TextBox txtStratCurrStepImax;
        private System.Windows.Forms.TextBox txtStratCurrStepImin;
        private System.Windows.Forms.Label label195;
        private System.Windows.Forms.Label label202;
        private System.Windows.Forms.ComboBox cmbStratIsSelStep;
        private System.Windows.Forms.Label label199;
        private System.Windows.Forms.TextBox txtStratIsNumSpire;
        private System.Windows.Forms.Label label198;
        private System.Windows.Forms.TextBox txtStratIsStep;
        private System.Windows.Forms.TextBox txtStratIsEsito;
        private System.Windows.Forms.Label label197;
        private System.Windows.Forms.TextBox txtStratIsMinuti;
        private System.Windows.Forms.TextBox txtStratIsAhRich;
        private System.Windows.Forms.Label label193;
        private System.Windows.Forms.Label label196;
        private System.Windows.Forms.Label label189;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox txtStratAVStop;
        private System.Windows.Forms.Label label201;
        private System.Windows.Forms.Label label179;
        private System.Windows.Forms.Label label177;
        private System.Windows.Forms.TextBox txtStratAVTempIst;
        private System.Windows.Forms.TextBox txtStratAVCorrenteIst;
        private System.Windows.Forms.TextBox txtStratAVPrevisti;
        private System.Windows.Forms.Label label180;
        private System.Windows.Forms.TextBox txtStratAVMancanti;
        private System.Windows.Forms.Label label181;
        private System.Windows.Forms.TextBox txtStratAVTensioneIst;
        private System.Windows.Forms.Label label182;
        private System.Windows.Forms.TextBox txtStratAVMinutiResidui;
        private System.Windows.Forms.Label label187;
        private System.Windows.Forms.TextBox txtStratAVErogati;
        private System.Windows.Forms.Label label188;
        private System.Windows.Forms.Label label185;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtStratLivcrgCapacityVer;
        private System.Windows.Forms.TextBox txtStratLivcrgDschgVer;
        private System.Windows.Forms.TextBox txtStratLivcrgChgVer;
        private System.Windows.Forms.TextBox txtStratLivcrgSetCapacity;
        private System.Windows.Forms.Label label178;
        private System.Windows.Forms.TextBox txtStratLivcrgSetDschg;
        private System.Windows.Forms.Label label183;
        private System.Windows.Forms.TextBox txtStratLivcrgSetChg;
        private System.Windows.Forms.Label label184;
        private System.Windows.Forms.Label label166;
        private System.Windows.Forms.Panel pnlStratContatoriCarica;
        private System.Windows.Forms.TextBox txtStratLivcrgCapNominale;
        private System.Windows.Forms.Label label165;
        private System.Windows.Forms.TextBox txtStratLivcrgCapResidua;
        private System.Windows.Forms.Label label164;
        private System.Windows.Forms.TextBox txtStratLivcrgDiscrgTot;
        private System.Windows.Forms.Label label162;
        private System.Windows.Forms.TextBox txtStratLivcrgCrgTot;
        private System.Windows.Forms.Label label163;
        private System.Windows.Forms.TextBox txtStratLivcrgDiscrg;
        private System.Windows.Forms.Label label160;
        private System.Windows.Forms.TextBox txtStratLivcrgCrg;
        private System.Windows.Forms.Label label161;
        private System.Windows.Forms.TextBox txtStratLivcrgNeg;
        private System.Windows.Forms.Label label159;
        private System.Windows.Forms.TextBox txtStratLivcrgPos;
        private System.Windows.Forms.Label label158;
        private System.Windows.Forms.Label label167;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtStratQryTrepr;
        private System.Windows.Forms.Label label175;
        private System.Windows.Forms.TextBox txtStratQryTalm;
        private System.Windows.Forms.Label label174;
        private System.Windows.Forms.TextBox txtStratQryTatt;
        private System.Windows.Forms.Label label173;
        private System.Windows.Forms.TextBox txtStratQryTensGas;
        private System.Windows.Forms.Label label172;
        private System.Windows.Forms.TextBox txtStratQryCapN;
        private System.Windows.Forms.Label label171;
        private System.Windows.Forms.TextBox txtStratQryTensN;
        private System.Windows.Forms.Label label170;
        private System.Windows.Forms.TextBox txtStratQryActSeup;
        private System.Windows.Forms.Label label169;
        private System.Windows.Forms.TextBox txtStratQryVerLib;
        private System.Windows.Forms.Label label168;
        private System.Windows.Forms.Label lblSig60DataReceuved;
        private System.Windows.Forms.Label lblSig60DataSent;
        private System.Windows.Forms.TextBox txtStratDataGridRx;
        private System.Windows.Forms.TextBox txtStratDataGridTx;
        private System.Windows.Forms.GroupBox grbStratComandiTest;
        private System.Windows.Forms.Button btnStratCallSIS;
        private System.Windows.Forms.Button btnStratCallAv;
        private System.Windows.Forms.Button btnStratSetDischarge;
        private System.Windows.Forms.Button btnStratCallIS;
        private System.Windows.Forms.Button btnStratSetCharge;
        private System.Windows.Forms.Button btnStratQuery;
        private System.Windows.Forms.Button btnStratTestERR;
        private System.Windows.Forms.Button btnStratTest02;
        private System.Windows.Forms.Button btnStratTest01;
        private System.Windows.Forms.TabPage tbpFirmware;
        private System.Windows.Forms.GroupBox grbFwAttivazioneArea;
        public System.Windows.Forms.Button btnFwSwitchArea2;
        public System.Windows.Forms.Button btnFwSwitchArea1;
        public System.Windows.Forms.Button btnFwSwitchBL;
        private System.Windows.Forms.GroupBox grbFWPreparaFile;
        private System.Windows.Forms.TextBox txtFWInFileStruct;
        private BrightIdeasSoftware.FastObjectListView lvwFWInFileStruct;
        private System.Windows.Forms.TextBox txtFwFileCCSa01;
        private System.Windows.Forms.TextBox txtFwFileCCShex;
        private System.Windows.Forms.TextBox txtFWLibInFileRev;
        private System.Windows.Forms.Label label256;
        private System.Windows.Forms.MaskedTextBox txtFWInFileRevData;
        private System.Windows.Forms.Label label96;
        private System.Windows.Forms.Button btnFWFileLLFsearch;
        private System.Windows.Forms.TextBox txtFWFileLLFwr;
        private System.Windows.Forms.Label label92;
        public System.Windows.Forms.Button btnFWFilePubSave;
        private System.Windows.Forms.TextBox txtFWInFileRev;
        private System.Windows.Forms.Label label94;
        private System.Windows.Forms.Label label95;
        public System.Windows.Forms.Button btnFWFileCCSLoad;
        private System.Windows.Forms.Button btnFWFileCCSsearch;
        private System.Windows.Forms.TextBox txtFwFileCCS;
        private System.Windows.Forms.GroupBox grbFWArea2;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.TextBox txtFwRevA2Size;
        private System.Windows.Forms.TextBox txtFWRevA2Addr5;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.TextBox txtFWRevA2Addr4;
        private System.Windows.Forms.TextBox txtFWRevA2Addr3;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.TextBox txtFWRevA2Addr2;
        private System.Windows.Forms.TextBox txtFWRevA2Addr1;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.Label label109;
        private System.Windows.Forms.TextBox txtFwRevA2RilFw;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.TextBox txtFwRevA2MsgSize;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.TextBox txtFwRevA2State;
        private System.Windows.Forms.Label label90;
        private System.Windows.Forms.TextBox txtFwRevA2RevFw;
        private System.Windows.Forms.GroupBox GrbFWArea1;
        private System.Windows.Forms.Label label97;
        private System.Windows.Forms.TextBox txtFwRevA1Size;
        private System.Windows.Forms.Label label103;
        private System.Windows.Forms.TextBox txtFwRevA1RilFw;
        private System.Windows.Forms.TextBox txtFWRevA1Addr5;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.TextBox txtFWRevA1Addr4;
        private System.Windows.Forms.TextBox txtFWRevA1Addr3;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.TextBox txtFWRevA1Addr2;
        private System.Windows.Forms.TextBox txtFWRevA1Addr1;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.TextBox txtFwRevA1MsgSize;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.TextBox txtFwRevA1State;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.TextBox txtFwRevA1RevFw;
        private System.Windows.Forms.GroupBox grbFWAggiornamento;
        private System.Windows.Forms.ComboBox cmbFWSBFArea;
        private BrightIdeasSoftware.FastObjectListView flwFWFileLLFStruct;
        private System.Windows.Forms.TextBox txtFWInLLFDispRev;
        private System.Windows.Forms.TextBox txtFWInSBFDtRev;
        private System.Windows.Forms.Label label110;
        public System.Windows.Forms.Button btnFWLanciaTrasmissione;
        private System.Windows.Forms.TextBox txtFWInLLFEsito;
        private System.Windows.Forms.Label label108;
        private System.Windows.Forms.Label label114;
        public System.Windows.Forms.Button btnFWPreparaTrasmissione;
        private System.Windows.Forms.TextBox txtFWInLLFRev;
        private System.Windows.Forms.Label label115;
        private System.Windows.Forms.Label label116;
        public System.Windows.Forms.Button btnFWFileLLFLoad;
        private System.Windows.Forms.Button btnFWFileLLFReadSearch;
        private System.Windows.Forms.TextBox txtFWFileSBFrd;
        private System.Windows.Forms.GroupBox grbStatoFirmware;
        private System.Windows.Forms.Label label186;
        private System.Windows.Forms.TextBox txtFwRevDisplay;
        private System.Windows.Forms.GroupBox grbFWDettStato;
        private System.Windows.Forms.TextBox txtFwStatoSA2;
        private System.Windows.Forms.Label label117;
        private System.Windows.Forms.TextBox txtFwStatoSA1;
        private System.Windows.Forms.Label label118;
        public System.Windows.Forms.Button btnFwCaricaStato;
        private System.Windows.Forms.TextBox txtFwStatoHA2;
        private System.Windows.Forms.Label label119;
        private System.Windows.Forms.TextBox txtFwStatoHA1;
        private System.Windows.Forms.Label label120;
        private System.Windows.Forms.TextBox txtFwStatoMicro;
        private System.Windows.Forms.Label label121;
        private System.Windows.Forms.TextBox txtFwAreaTestata;
        private System.Windows.Forms.Label label122;
        private System.Windows.Forms.TextBox txtFwRevFirmware;
        private System.Windows.Forms.Label label123;
        private System.Windows.Forms.TextBox txtFwRevBootloader;
        private System.Windows.Forms.Label label124;
        private System.Windows.Forms.TabPage tabMemRead;
        private System.Windows.Forms.GroupBox grbMemAzzeraLogger;
        private System.Windows.Forms.CheckBox chkMemCReboot;
        private System.Windows.Forms.Button btnMemClearLogExec;
        private System.Windows.Forms.CheckBox chkMemCResetCicli;
        private System.Windows.Forms.CheckBox chkMemCResetCont;
        private System.Windows.Forms.CheckBox chkMemCResetProg;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.GroupBox grbMemCaricaLogger;
        private System.Windows.Forms.Button btnMemRewriteExec;
        private System.Windows.Forms.CheckBox chkMemDevWCycle;
        private System.Windows.Forms.CheckBox chkMemDevWCount;
        private System.Windows.Forms.CheckBox chkMemDevWProg;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox txtMemFileRead;
        private System.Windows.Forms.GroupBox grbMemSalvaLogger;
        private System.Windows.Forms.Button btnMemSaveExec;
        private System.Windows.Forms.CheckBox chkMemFsCycle;
        private System.Windows.Forms.CheckBox chkMemFsCount;
        private System.Windows.Forms.CheckBox chkMemFsProgr;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Button btnMemFileSaveSRC;
        private System.Windows.Forms.TextBox txtMemFileSave;
        private System.Windows.Forms.GroupBox grbMemCancFisica;
        private System.Windows.Forms.RadioButton rbtMemLunghi;
        private System.Windows.Forms.RadioButton rbtMemBrevi;
        private System.Windows.Forms.RadioButton rbtMemProgrammazioni;
        private System.Windows.Forms.RadioButton rbtMemContatori;
        private System.Windows.Forms.Button btnMemResetBoard;
        private System.Windows.Forms.RadioButton rbtMemDatiCliente;
        private System.Windows.Forms.RadioButton rbtMemParametriInit;
        private System.Windows.Forms.RadioButton rbtMemAreaApp2;
        private System.Windows.Forms.RadioButton rbtMemAreaApp1;
        private System.Windows.Forms.RadioButton rbtMemAreaLibera;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.TextBox txtMemCFBlocchi;
        private System.Windows.Forms.CheckBox chkMemCFStartAddHex;
        private System.Windows.Forms.Label label112;
        private System.Windows.Forms.TextBox txtMemCFStartAdd;
        private System.Windows.Forms.Button btnMemCFExec;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button btnDumpMemoria;
        private System.Windows.Forms.TextBox txtMemDataGrid;
        private System.Windows.Forms.GroupBox grbMemScrittura;
        private System.Windows.Forms.CheckBox chkMemHexW;
        private System.Windows.Forms.Label lblMemVerificaValore;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.TextBox txtMemDataW;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.TextBox txtMemLenW;
        private System.Windows.Forms.TextBox txtMemAddrW;
        private System.Windows.Forms.Button cmdMemWrite;
        private System.Windows.Forms.GroupBox grbMemCancellazione;
        private System.Windows.Forms.CheckBox chkMemClearMantieniCliente;
        private System.Windows.Forms.Button cmdMemClear;
        private System.Windows.Forms.GroupBox grbMemLettura;
        private System.Windows.Forms.CheckBox chkMemHex;
        private System.Windows.Forms.Label lblReadMemBytes;
        private System.Windows.Forms.Label lblReadMemStartAddr;
        private System.Windows.Forms.TextBox txtMemLenR;
        private System.Windows.Forms.TextBox txtMemAddrR;
        private System.Windows.Forms.Button cmdMemRead;
        private System.Windows.Forms.TabPage tabInizializzazione;
        private System.Windows.Forms.GroupBox grbInitLimiti;
        private System.Windows.Forms.TextBox txtInitModelloMemoria;
        private System.Windows.Forms.Label label150;
        private System.Windows.Forms.TextBox txtInitMaxProg;
        private System.Windows.Forms.Label label149;
        private System.Windows.Forms.TextBox txtInitMaxLunghi;
        private System.Windows.Forms.Label label148;
        private System.Windows.Forms.TextBox txtInitMaxBrevi;
        private System.Windows.Forms.Label label146;
        private System.Windows.Forms.Button btnScriviInizializzazione;
        private System.Windows.Forms.Button btnCaricaInizializzazione;
        private System.Windows.Forms.GroupBox grbInitCalibrazione;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label100;
        private System.Windows.Forms.TextBox textBox36;
        private System.Windows.Forms.Label label106;
        private System.Windows.Forms.GroupBox grbInitDatiBase;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkInitPresenzaRabb;
        private System.Windows.Forms.TextBox txtInitVMax;
        private System.Windows.Forms.TextBox txtInitVMin;
        private System.Windows.Forms.TextBox txtInitIDApparato;
        private System.Windows.Forms.Label label144;
        private System.Windows.Forms.MaskedTextBox txtInitDataInizializ;
        private System.Windows.Forms.Label label132;
        private System.Windows.Forms.ComboBox cmbInitTipoApparato;
        private System.Windows.Forms.Label label99;
        private System.Windows.Forms.TextBox txtInitSerialeApparato;
        private System.Windows.Forms.Label label98;
        private System.Windows.Forms.TextBox txtInitNumeroMatricola;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.TextBox txtInitAnnoMatricola;
        private System.Windows.Forms.Label Anno;
        private System.Windows.Forms.TextBox txtInitProductId;
        private System.Windows.Forms.Label lblInitProductId;
        private System.Windows.Forms.TextBox txtInitManufactured;
        private System.Windows.Forms.Label lblInitManufactured;
        private System.Windows.Forms.TabPage tabOrologio;
        private System.Windows.Forms.GroupBox grbAccensione;
        private System.Windows.Forms.Label lblOrarioAccensione;
        private System.Windows.Forms.ComboBox cmbMinAccensione;
        private System.Windows.Forms.ComboBox cmbOraAccensione;
        private System.Windows.Forms.RadioButton rbtAccensione03;
        private System.Windows.Forms.Label lblOreRitardo;
        private System.Windows.Forms.ComboBox cmbOreRitardo;
        private System.Windows.Forms.RadioButton rbtAccensione02;
        private System.Windows.Forms.RadioButton rbtAccensione01;
        private System.Windows.Forms.GroupBox grbOraCorrente;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnLeggiRtc;
        private System.Windows.Forms.TextBox txtOraRtc;
        private System.Windows.Forms.Label lblOraRTC;
        private System.Windows.Forms.TextBox txtDataRtc;
        private System.Windows.Forms.Label lblDataRTC;
        private System.Windows.Forms.TabPage tabProfiloAttuale;
        private System.Windows.Forms.TabControl tbcPaSottopagina;
        private System.Windows.Forms.TabPage tbpPaProfiloAttivo;
        private System.Windows.Forms.GroupBox pippo;
        private System.Windows.Forms.CheckBox chkPaSbloccaValori;
        private System.Windows.Forms.Label lblPaSbloccaValori;
        private System.Windows.Forms.CheckBox chkPaAttivaMant;
        private System.Windows.Forms.Label lblPaAttivaMant;
        private System.Windows.Forms.Button btnCicloCorrente;
        private System.Windows.Forms.Button btnPaProfileRefresh;
        private System.Windows.Forms.PictureBox picPaImmagineProfilo;
        private System.Windows.Forms.TabControl tbcPaSchedeValori;
        private System.Windows.Forms.TabPage tbpPaGeneraleCiclo;
        private System.Windows.Forms.TextBox txtPaCassone;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox txtPaIdSetup;
        private System.Windows.Forms.Label lblPaIdSetup;
        private System.Windows.Forms.TextBox txtPaNomeSetup;
        private System.Windows.Forms.Label label152;
        private System.Windows.Forms.TabPage tbpPaPCStep0;
        private System.Windows.Forms.TextBox txtPaDurataMaxT0;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TextBox txtPaPrefaseI0;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txtPaSogliaV0;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TabPage tbpPaPCStep1;
        private System.Windows.Forms.TextBox cmbPaDurataMaxT1;
        private System.Windows.Forms.ComboBox cmbPaDurataCarica;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPaCorrenteI1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPaSogliaVs;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tbpPaPCStep2;
        private System.Windows.Forms.TextBox txtPaVMax;
        private System.Windows.Forms.Label label207;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtPaCoeffK;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox txtPaTempoT2Max;
        private System.Windows.Forms.TextBox txtPaTempoT2Min;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtPaCorrenteRaccordo;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox txtPaCorrenteF3;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtPaRaccordoF1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabPage tbpPaPCStep3;
        private System.Windows.Forms.Label label19;
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
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox txtPaMantDurataMax;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox txtPaMantVmax;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox txtPaMantVmin;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox txtPaMantAttesa;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TabPage tbpPaParSoglia;
        private System.Windows.Forms.TextBox txtPaVMinStop;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.TextBox txtPaBMSTempoAttesa;
        private System.Windows.Forms.Label label156;
        private System.Windows.Forms.TextBox txtPaBMSTempoErogazione;
        private System.Windows.Forms.Label label157;
        private System.Windows.Forms.Label chkPaRiarmaBms;
        private System.Windows.Forms.CheckBox chkPaAttivaRiarmoBms;
        private System.Windows.Forms.TextBox txtPaCorrenteMassima;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox txtPaVMaxRic;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtPaVMinRic;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtPaVLimite;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtPaNumCelle;
        private System.Windows.Forms.Label lblPaNumCelle;
        private System.Windows.Forms.ComboBox cmbPaTipoBatteria;
        private System.Windows.Forms.Label lblPaTipoBatteria;
        private System.Windows.Forms.CheckBox chkPaAttivaEqual;
        private System.Windows.Forms.Label lblPaAttivaEqual;
        private System.Windows.Forms.CheckBox chkPaUsaSpyBatt;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Button btnPaSalvaDati;
        private System.Windows.Forms.Label lblPaTensione;
        private System.Windows.Forms.ComboBox cmbPaProfilo;
        private System.Windows.Forms.TextBox txtPaCapacita;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbPaTensione;
        private System.Windows.Forms.TextBox txtPaTensione;
        private System.Windows.Forms.TabPage tbpPaListaProfili;
        private BrightIdeasSoftware.FastObjectListView flwPaListaConfigurazioni;
        private System.Windows.Forms.Button btnPaCaricaListaProfili;
        private System.Windows.Forms.TabPage tbpPaCfgAvanzate;
        private System.Windows.Forms.GroupBox grbVarParametriSig;
        private System.Windows.Forms.Button btnFSerVerificaOC;
        private System.Windows.Forms.CheckBox chkFSerEchoOC;
        private System.Windows.Forms.Button btnFSerImpostaOC;
        private System.Windows.Forms.Label label278;
        private System.Windows.Forms.ComboBox cmbFSerBaudrateOC;
        private System.Windows.Forms.Label lblPaTitoloLista;
        private System.Windows.Forms.TabPage tabCb04;
        private System.Windows.Forms.GroupBox grbCicli;
        private System.Windows.Forms.Button btnCicliMostraBrevi;
        private System.Windows.Forms.Button btnCicliCaricaBrevi;
        private System.Windows.Forms.CheckBox chkCicliCaricaBrevi;
        private System.Windows.Forms.Label label223;
        private System.Windows.Forms.TextBox txtCicliNumRecord;
        private System.Windows.Forms.Label label209;
        private System.Windows.Forms.TextBox txtCicliAddrPrmo;
        private System.Windows.Forms.Button btnCicliCaricaLista;
        private System.Windows.Forms.Button btnCicliVuotaLista;
        private System.Windows.Forms.TabPage tabGenerale;
        private System.Windows.Forms.Button btnCaricaContatori;
        private System.Windows.Forms.GroupBox grbMainContatori;
        private System.Windows.Forms.Label label222;
        private System.Windows.Forms.Label label221;
        private System.Windows.Forms.TextBox txtContPntNextBreve;
        private System.Windows.Forms.Label label220;
        private System.Windows.Forms.TextBox txtContNumCancellazioni;
        private System.Windows.Forms.Label label219;
        private System.Windows.Forms.Label label217;
        private System.Windows.Forms.TextBox txtContDtUltimaCanc;
        private System.Windows.Forms.TextBox txtContPntNextCarica;
        private System.Windows.Forms.TextBox txtContBreviSalvati;
        private System.Windows.Forms.TextBox txtContCaricheSalvate;
        private System.Windows.Forms.TextBox txtContCaricheOver9;
        private System.Windows.Forms.TextBox txtContCariche6to9;
        private System.Windows.Forms.TextBox txtContCariche3to6;
        private System.Windows.Forms.TextBox txtContCaricheUnder3;
        private System.Windows.Forms.Label label208;
        private System.Windows.Forms.TextBox txtContCaricheStrappo;
        private System.Windows.Forms.Label label210;
        private System.Windows.Forms.TextBox txtContCaricheStop;
        private System.Windows.Forms.Label label211;
        private System.Windows.Forms.Label label212;
        private System.Windows.Forms.Label label213;
        private System.Windows.Forms.Label label214;
        private System.Windows.Forms.Label label215;
        private System.Windows.Forms.TextBox txtContCaricheTotali;
        private System.Windows.Forms.Label label216;
        private System.Windows.Forms.TextBox txtContDtPrimaCarica;
        private System.Windows.Forms.Label label218;
        private System.Windows.Forms.GroupBox GrbMainDatiApparato;
        private System.Windows.Forms.TextBox txtGenSerialeZVT;
        private System.Windows.Forms.Label label151;
        private System.Windows.Forms.TextBox txtGenCorrenteMax;
        private System.Windows.Forms.Label label143;
        private System.Windows.Forms.TextBox txtGenTensioneMax;
        private System.Windows.Forms.Label label142;
        private System.Windows.Forms.TextBox txtGenModello;
        private System.Windows.Forms.Label label141;
        private System.Windows.Forms.TextBox txtGenRevFwDisplay;
        private System.Windows.Forms.Label label140;
        private System.Windows.Forms.TextBox textBox34;
        private System.Windows.Forms.Label label139;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label138;
        private System.Windows.Forms.TextBox txtGenRevHwDisplay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtGenIdApparato;
        private System.Windows.Forms.Label label137;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtGenAnnoMatricola;
        private System.Windows.Forms.TextBox txtGenMatricola;
        private System.Windows.Forms.Label lblMatricola;
        private System.Windows.Forms.GroupBox grbDatiCliente;
        private System.Windows.Forms.Label label254;
        private System.Windows.Forms.TextBox txtCliCodiceLL;
        private System.Windows.Forms.TextBox txtCliNote;
        private System.Windows.Forms.TextBox txtCliDescrizione;
        private System.Windows.Forms.Label lblCliIdBatteria;
        private System.Windows.Forms.TextBox txtCliente;
        private System.Windows.Forms.Label lbNoteCliente;
        private System.Windows.Forms.Label lblCliCliente;
        private System.Windows.Forms.TabControl tabCaricaBatterie;
        private System.Windows.Forms.Button btnPaCaricaCicli;
        private BrightIdeasSoftware.FastObjectListView flvCicliListaCariche;
        private System.Windows.Forms.Button btnCicliCaricaArea;
        private System.Windows.Forms.Button btnPaAttivaConfigurazione;
        private System.Windows.Forms.Button btnGenAzzzeraContatori;
        private System.Windows.Forms.Button btnGenAzzzeraContatoriTot;
        private System.Windows.Forms.Button btnGenResetBoard;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox txtContNumProgrammazioni;
        private System.Windows.Forms.CheckBox chkPaAttivaOppChg;
        private System.Windows.Forms.Label lblPaAttivaOppChg;
        private System.Windows.Forms.CheckBox chkPaUsaSafety;
        private System.Windows.Forms.Label lblPaUsaSafety;
        private System.Windows.Forms.TabPage tbpPaPCOpp;
        private System.Windows.Forms.TextBox txtPaOppDurataMax;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.TextBox txtPaOppCorrente;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TextBox txtPaOppVSoglia;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox txtPaOppOraFine;
        private System.Windows.Forms.Label lblPaOppOraFine;
        private System.Windows.Forms.TextBox txtPaOppOraInizio;
        private System.Windows.Forms.Label lblPaOppOraInizio;
        private Syncfusion.Windows.Forms.Tools.RangeSlider rslPaOppFinestra;
        private System.Windows.Forms.CheckBox chkPaOppNotturno;
        private System.DirectoryServices.DirectoryEntry directoryEntry1;
        private System.Windows.Forms.Label lblPaOppPuntoVerde;
        private System.Windows.Forms.PictureBox ImgPaOppPuntoVerde;
        private System.Windows.Forms.TextBox txtContCaricheOpportunity;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Button btnPaProfileChiudiCanale;
        private System.Windows.Forms.Button btnSalveCliente;
        private System.Windows.Forms.Button btnCaricaCliente;
        private System.Windows.Forms.TextBox txtCliNomeIntLL;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.GroupBox grbMemTestLetture;
        private System.Windows.Forms.CheckBox chkMemTestAddrRND;
        private System.Windows.Forms.CheckBox chkMemTestLenRND;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox txtMemNumTestERR;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox txtMemNumTestOK;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TextBox txtMemNumTest;
        private System.Windows.Forms.Button btnMemTestExac;
        public System.Windows.Forms.Button btnFwSwitchApp;
        private System.Windows.Forms.GroupBox grbPaImpostazioniLocali;
        private System.Windows.Forms.Button btnPaProfileClear;
        private System.Windows.Forms.CheckBox chkMemInt;
        private System.Windows.Forms.GroupBox groupBox11;
        private System.Windows.Forms.NumericUpDown txtInitBrdNumModuli;
        private System.Windows.Forms.TextBox txtInitRevHwDISP;
        private System.Windows.Forms.Label label131;
        private System.Windows.Forms.TextBox txtInitRevFwDISP;
        private System.Windows.Forms.Label label127;
        private System.Windows.Forms.TextBox txtInitNumSerDISP;
        private System.Windows.Forms.Label label128;
        private System.Windows.Forms.TextBox txtInitBrdOpzioniModulo;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.CheckBox chkInitBrdSpareModulo;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.NumericUpDown txtInitBrdANomModulo;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.NumericUpDown txtInitBrdVNomModulo;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox txtInitBrdVMaxModulo;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.TextBox txtInitBrdVMinModulo;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.NumericUpDown txtInitAMax;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtPaCoeffKc;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox grbCalData;
        private System.Windows.Forms.TextBox txtCalMinuti;
        private System.Windows.Forms.Label label266;
        private System.Windows.Forms.TextBox txtCalOre;
        private System.Windows.Forms.Label label267;
        private System.Windows.Forms.Button btnCalScriviGiorno;
        private System.Windows.Forms.TextBox txtCalAnno;
        private System.Windows.Forms.Label label250;
        private System.Windows.Forms.TextBox txtCalMese;
        private System.Windows.Forms.Label label249;
        private System.Windows.Forms.TextBox txtCalGiorno;
        private System.Windows.Forms.Label label248;
        private System.Windows.Forms.Button btnPaProfileNEW;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblGenBRState;
        private System.Windows.Forms.Button btnGenCambiaBaudRate;
        private System.Windows.Forms.GroupBox grbGenBaudrate;
        private System.Windows.Forms.RadioButton optGenBR3M;
        private System.Windows.Forms.RadioButton optGenBR1M;
        private System.Windows.Forms.RadioButton optGenBR115;
        private System.Windows.Forms.Button btnCaricaMemoria;
        private System.Windows.Forms.GroupBox grbConnessioni;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox txtPaTempoFin;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox txtPadT;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtPadV;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtPaTempLimite;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.GroupBox grbPaGeneraFile;
        private System.Windows.Forms.CheckBox chkPaSoloSelezionati;
        private System.Windows.Forms.Button btnPaNomeFileProfiliSRC;
        private System.Windows.Forms.TextBox txtPaNomeFileProfili;
        private System.Windows.Forms.Button btnPaCancellaSelezionati;
        private System.Windows.Forms.Button btnPaSalvaFile;
        private System.Windows.Forms.Button btnCicliCaricaCont;
        private System.Windows.Forms.Button btnPaProfileImport;
    }
}