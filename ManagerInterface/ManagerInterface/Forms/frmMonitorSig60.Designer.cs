namespace PannelloCharger
{
    partial class frmMonitorSig60
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
            this.pnlPorta = new System.Windows.Forms.Panel();
            this.btnGetSigRegister = new System.Windows.Forms.Button();
            this.btnSetSigRegister = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPorta = new System.Windows.Forms.Label();
            this.btnPortState = new System.Windows.Forms.Button();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.cboPorts = new System.Windows.Forms.ComboBox();
            this.btnGetSerialPorts = new System.Windows.Forms.Button();
            this.btnSigLogReset = new System.Windows.Forms.Button();
            this.btnSigListCLS = new System.Windows.Forms.Button();
            this.btnSigEchoCLS = new System.Windows.Forms.Button();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.flvListaComandiSIG = new BrightIdeasSoftware.FastObjectListView();
            this.pnlFile = new System.Windows.Forms.Panel();
            this.btnSfogliaCarica = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEchoFileNote = new System.Windows.Forms.TextBox();
            this.btnCariacaRegistrazione = new System.Windows.Forms.Button();
            this.btnSalvaRegistrazione = new System.Windows.Forms.Button();
            this.btnSfogliaSalva = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEchoFilename = new System.Windows.Forms.TextBox();
            this.sfdExportDati = new System.Windows.Forms.SaveFileDialog();
            this.ofdImportDati = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkSendAuto = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNumCaratteri = new System.Windows.Forms.TextBox();
            this.chkSendStop = new System.Windows.Forms.CheckBox();
            this.chkSendStart = new System.Windows.Forms.CheckBox();
            this.btnCmdSendNoise = new System.Windows.Forms.Button();
            this.btnCmdAv = new System.Windows.Forms.Button();
            this.btnCmdQuery = new System.Windows.Forms.Button();
            this.btnCmdStopCom = new System.Windows.Forms.Button();
            this.btnCmdStartCom = new System.Windows.Forms.Button();
            this.pnlComandiLista = new System.Windows.Forms.Panel();
            this.tmrInvioAutomatico = new System.Windows.Forms.Timer(this.components);
            this.rtfSerialEcho = new System.Windows.Forms.RichTextBox();
            this.trbOcMonitorZoom = new System.Windows.Forms.TrackBar();
            this.pnlPorta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvListaComandiSIG)).BeginInit();
            this.pnlFile.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlComandiLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trbOcMonitorZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlPorta
            // 
            this.pnlPorta.BackColor = System.Drawing.Color.White;
            this.pnlPorta.Controls.Add(this.btnGetSigRegister);
            this.pnlPorta.Controls.Add(this.btnSetSigRegister);
            this.pnlPorta.Controls.Add(this.label1);
            this.pnlPorta.Controls.Add(this.lblPorta);
            this.pnlPorta.Controls.Add(this.btnPortState);
            this.pnlPorta.Controls.Add(this.cboBaudRate);
            this.pnlPorta.Controls.Add(this.cboPorts);
            this.pnlPorta.Controls.Add(this.btnGetSerialPorts);
            this.pnlPorta.Location = new System.Drawing.Point(23, 24);
            this.pnlPorta.Name = "pnlPorta";
            this.pnlPorta.Size = new System.Drawing.Size(347, 144);
            this.pnlPorta.TabIndex = 1;
            // 
            // btnGetSigRegister
            // 
            this.btnGetSigRegister.Location = new System.Drawing.Point(15, 100);
            this.btnGetSigRegister.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetSigRegister.Name = "btnGetSigRegister";
            this.btnGetSigRegister.Size = new System.Drawing.Size(151, 28);
            this.btnGetSigRegister.TabIndex = 43;
            this.btnGetSigRegister.Text = "Verifica SIG";
            this.btnGetSigRegister.UseVisualStyleBackColor = true;
            this.btnGetSigRegister.Click += new System.EventHandler(this.btnGetSigRegister_Click);
            // 
            // btnSetSigRegister
            // 
            this.btnSetSigRegister.Location = new System.Drawing.Point(174, 100);
            this.btnSetSigRegister.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetSigRegister.Name = "btnSetSigRegister";
            this.btnSetSigRegister.Size = new System.Drawing.Size(157, 28);
            this.btnSetSigRegister.TabIndex = 42;
            this.btnSetSigRegister.Text = "Imposta SIG";
            this.btnSetSigRegister.UseVisualStyleBackColor = true;
            this.btnSetSigRegister.Click += new System.EventHandler(this.btnSetSigRegister_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(173, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 41;
            this.label1.Text = "Baudrate";
            // 
            // lblPorta
            // 
            this.lblPorta.AutoSize = true;
            this.lblPorta.Location = new System.Drawing.Point(12, 47);
            this.lblPorta.Name = "lblPorta";
            this.lblPorta.Size = new System.Drawing.Size(42, 17);
            this.lblPorta.TabIndex = 40;
            this.lblPorta.Text = "Porta";
            // 
            // btnPortState
            // 
            this.btnPortState.Location = new System.Drawing.Point(174, 12);
            this.btnPortState.Margin = new System.Windows.Forms.Padding(4);
            this.btnPortState.Name = "btnPortState";
            this.btnPortState.Size = new System.Drawing.Size(157, 28);
            this.btnPortState.TabIndex = 28;
            this.btnPortState.Text = "Apri Porta";
            this.btnPortState.UseVisualStyleBackColor = true;
            this.btnPortState.Click += new System.EventHandler(this.btnPortState_Click);
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.FormattingEnabled = true;
            this.cboBaudRate.Location = new System.Drawing.Point(174, 68);
            this.cboBaudRate.Margin = new System.Windows.Forms.Padding(4);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(157, 24);
            this.cboBaudRate.TabIndex = 23;
            // 
            // cboPorts
            // 
            this.cboPorts.FormattingEnabled = true;
            this.cboPorts.Location = new System.Drawing.Point(15, 68);
            this.cboPorts.Margin = new System.Windows.Forms.Padding(4);
            this.cboPorts.Name = "cboPorts";
            this.cboPorts.Size = new System.Drawing.Size(151, 24);
            this.cboPorts.TabIndex = 22;
            // 
            // btnGetSerialPorts
            // 
            this.btnGetSerialPorts.Location = new System.Drawing.Point(15, 12);
            this.btnGetSerialPorts.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetSerialPorts.Name = "btnGetSerialPorts";
            this.btnGetSerialPorts.Size = new System.Drawing.Size(151, 28);
            this.btnGetSerialPorts.TabIndex = 21;
            this.btnGetSerialPorts.Text = "Carica Porte";
            this.btnGetSerialPorts.UseVisualStyleBackColor = true;
            this.btnGetSerialPorts.Click += new System.EventHandler(this.btnGetSerialPorts_Click);
            // 
            // btnSigLogReset
            // 
            this.btnSigLogReset.ForeColor = System.Drawing.Color.Red;
            this.btnSigLogReset.Location = new System.Drawing.Point(233, 15);
            this.btnSigLogReset.Margin = new System.Windows.Forms.Padding(4);
            this.btnSigLogReset.Name = "btnSigLogReset";
            this.btnSigLogReset.Size = new System.Drawing.Size(98, 28);
            this.btnSigLogReset.TabIndex = 50;
            this.btnSigLogReset.Text = "Clr ALL";
            this.btnSigLogReset.UseVisualStyleBackColor = true;
            this.btnSigLogReset.Click += new System.EventHandler(this.btnSigLogReset_Click);
            // 
            // btnSigListCLS
            // 
            this.btnSigListCLS.Location = new System.Drawing.Point(127, 15);
            this.btnSigListCLS.Margin = new System.Windows.Forms.Padding(4);
            this.btnSigListCLS.Name = "btnSigListCLS";
            this.btnSigListCLS.Size = new System.Drawing.Size(98, 28);
            this.btnSigListCLS.TabIndex = 49;
            this.btnSigListCLS.Text = "Clr SIG LIST";
            this.btnSigListCLS.UseVisualStyleBackColor = true;
            // 
            // btnSigEchoCLS
            // 
            this.btnSigEchoCLS.Location = new System.Drawing.Point(15, 15);
            this.btnSigEchoCLS.Margin = new System.Windows.Forms.Padding(4);
            this.btnSigEchoCLS.Name = "btnSigEchoCLS";
            this.btnSigEchoCLS.Size = new System.Drawing.Size(104, 28);
            this.btnSigEchoCLS.TabIndex = 48;
            this.btnSigEchoCLS.Text = "Clr SIG ECHO";
            this.btnSigEchoCLS.UseVisualStyleBackColor = true;
            this.btnSigEchoCLS.Click += new System.EventHandler(this.btnSigEchoCLS_Click);
            // 
            // btnChiudi
            // 
            this.btnChiudi.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChiudi.Location = new System.Drawing.Point(1637, 759);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(125, 44);
            this.btnChiudi.TabIndex = 4;
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // flvListaComandiSIG
            // 
            this.flvListaComandiSIG.CellEditUseWholeCell = false;
            this.flvListaComandiSIG.FullRowSelect = true;
            this.flvListaComandiSIG.Location = new System.Drawing.Point(390, 24);
            this.flvListaComandiSIG.MultiSelect = false;
            this.flvListaComandiSIG.Name = "flvListaComandiSIG";
            this.flvListaComandiSIG.ShowGroups = false;
            this.flvListaComandiSIG.Size = new System.Drawing.Size(1372, 519);
            this.flvListaComandiSIG.TabIndex = 21;
            this.flvListaComandiSIG.UseCompatibleStateImageBehavior = false;
            this.flvListaComandiSIG.View = System.Windows.Forms.View.Details;
            this.flvListaComandiSIG.VirtualMode = true;
            this.flvListaComandiSIG.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.flvListaComandiSIG_FormatRow);
            // 
            // pnlFile
            // 
            this.pnlFile.BackColor = System.Drawing.Color.White;
            this.pnlFile.Controls.Add(this.btnSfogliaCarica);
            this.pnlFile.Controls.Add(this.label3);
            this.pnlFile.Controls.Add(this.txtEchoFileNote);
            this.pnlFile.Controls.Add(this.btnCariacaRegistrazione);
            this.pnlFile.Controls.Add(this.btnSalvaRegistrazione);
            this.pnlFile.Controls.Add(this.btnSfogliaSalva);
            this.pnlFile.Controls.Add(this.label2);
            this.pnlFile.Controls.Add(this.txtEchoFilename);
            this.pnlFile.Location = new System.Drawing.Point(23, 242);
            this.pnlFile.Name = "pnlFile";
            this.pnlFile.Size = new System.Drawing.Size(347, 175);
            this.pnlFile.TabIndex = 22;
            // 
            // btnSfogliaCarica
            // 
            this.btnSfogliaCarica.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSfogliaCarica.Location = new System.Drawing.Point(189, 68);
            this.btnSfogliaCarica.Name = "btnSfogliaCarica";
            this.btnSfogliaCarica.Size = new System.Drawing.Size(36, 33);
            this.btnSfogliaCarica.TabIndex = 48;
            this.btnSfogliaCarica.Text = "...";
            this.btnSfogliaCarica.UseVisualStyleBackColor = true;
            this.btnSfogliaCarica.Click += new System.EventHandler(this.btnSfogliaCarica_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 17);
            this.label3.TabIndex = 47;
            this.label3.Text = "Note";
            // 
            // txtEchoFileNote
            // 
            this.txtEchoFileNote.Location = new System.Drawing.Point(15, 125);
            this.txtEchoFileNote.Multiline = true;
            this.txtEchoFileNote.Name = "txtEchoFileNote";
            this.txtEchoFileNote.Size = new System.Drawing.Size(316, 38);
            this.txtEchoFileNote.TabIndex = 46;
            // 
            // btnCariacaRegistrazione
            // 
            this.btnCariacaRegistrazione.Location = new System.Drawing.Point(232, 68);
            this.btnCariacaRegistrazione.Margin = new System.Windows.Forms.Padding(4);
            this.btnCariacaRegistrazione.Name = "btnCariacaRegistrazione";
            this.btnCariacaRegistrazione.Size = new System.Drawing.Size(99, 33);
            this.btnCariacaRegistrazione.TabIndex = 45;
            this.btnCariacaRegistrazione.Text = "Carica Log";
            this.btnCariacaRegistrazione.UseVisualStyleBackColor = true;
            this.btnCariacaRegistrazione.Click += new System.EventHandler(this.btnCariacaRegistrazione_Click);
            // 
            // btnSalvaRegistrazione
            // 
            this.btnSalvaRegistrazione.Location = new System.Drawing.Point(58, 68);
            this.btnSalvaRegistrazione.Margin = new System.Windows.Forms.Padding(4);
            this.btnSalvaRegistrazione.Name = "btnSalvaRegistrazione";
            this.btnSalvaRegistrazione.Size = new System.Drawing.Size(99, 33);
            this.btnSalvaRegistrazione.TabIndex = 44;
            this.btnSalvaRegistrazione.Text = "Salva Log";
            this.btnSalvaRegistrazione.UseVisualStyleBackColor = true;
            this.btnSalvaRegistrazione.Click += new System.EventHandler(this.btnSalvaRegistrazione_Click);
            // 
            // btnSfogliaSalva
            // 
            this.btnSfogliaSalva.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSfogliaSalva.Location = new System.Drawing.Point(15, 68);
            this.btnSfogliaSalva.Name = "btnSfogliaSalva";
            this.btnSfogliaSalva.Size = new System.Drawing.Size(36, 33);
            this.btnSfogliaSalva.TabIndex = 42;
            this.btnSfogliaSalva.Text = "...";
            this.btnSfogliaSalva.UseVisualStyleBackColor = true;
            this.btnSfogliaSalva.Click += new System.EventHandler(this.btnSfogliaSalva_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 41;
            this.label2.Text = "File Dati";
            // 
            // txtEchoFilename
            // 
            this.txtEchoFilename.Location = new System.Drawing.Point(15, 36);
            this.txtEchoFilename.Name = "txtEchoFilename";
            this.txtEchoFilename.Size = new System.Drawing.Size(316, 22);
            this.txtEchoFilename.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.chkSendAuto);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtNumCaratteri);
            this.panel1.Controls.Add(this.chkSendStop);
            this.panel1.Controls.Add(this.chkSendStart);
            this.panel1.Controls.Add(this.btnCmdSendNoise);
            this.panel1.Controls.Add(this.btnCmdAv);
            this.panel1.Controls.Add(this.btnCmdQuery);
            this.panel1.Controls.Add(this.btnCmdStopCom);
            this.panel1.Controls.Add(this.btnCmdStartCom);
            this.panel1.Location = new System.Drawing.Point(24, 423);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 120);
            this.panel1.TabIndex = 23;
            // 
            // chkSendAuto
            // 
            this.chkSendAuto.AutoSize = true;
            this.chkSendAuto.Location = new System.Drawing.Point(239, 86);
            this.chkSendAuto.Name = "chkSendAuto";
            this.chkSendAuto.Size = new System.Drawing.Size(62, 21);
            this.chkSendAuto.TabIndex = 52;
            this.chkSendAuto.Text = "Loop";
            this.chkSendAuto.UseVisualStyleBackColor = true;
            this.chkSendAuto.CheckedChanged += new System.EventHandler(this.chkSendAuto_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(76, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 17);
            this.label4.TabIndex = 51;
            this.label4.Text = "Bytes";
            // 
            // txtNumCaratteri
            // 
            this.txtNumCaratteri.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumCaratteri.Location = new System.Drawing.Point(119, 84);
            this.txtNumCaratteri.Name = "txtNumCaratteri";
            this.txtNumCaratteri.Size = new System.Drawing.Size(46, 22);
            this.txtNumCaratteri.TabIndex = 50;
            this.txtNumCaratteri.Text = "256";
            this.txtNumCaratteri.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkSendStop
            // 
            this.chkSendStop.AutoSize = true;
            this.chkSendStop.Location = new System.Drawing.Point(171, 86);
            this.chkSendStop.Name = "chkSendStop";
            this.chkSendStop.Size = new System.Drawing.Size(60, 21);
            this.chkSendStop.TabIndex = 49;
            this.chkSendStop.Text = "0x03";
            this.chkSendStop.UseVisualStyleBackColor = true;
            // 
            // chkSendStart
            // 
            this.chkSendStart.AutoSize = true;
            this.chkSendStart.Location = new System.Drawing.Point(15, 85);
            this.chkSendStart.Name = "chkSendStart";
            this.chkSendStart.Size = new System.Drawing.Size(60, 21);
            this.chkSendStart.TabIndex = 48;
            this.chkSendStart.Text = "0x02";
            this.chkSendStart.UseVisualStyleBackColor = true;
            // 
            // btnCmdSendNoise
            // 
            this.btnCmdSendNoise.Location = new System.Drawing.Point(14, 51);
            this.btnCmdSendNoise.Margin = new System.Windows.Forms.Padding(4);
            this.btnCmdSendNoise.Name = "btnCmdSendNoise";
            this.btnCmdSendNoise.Size = new System.Drawing.Size(91, 27);
            this.btnCmdSendNoise.TabIndex = 47;
            this.btnCmdSendNoise.Text = "TX Rumore";
            this.btnCmdSendNoise.UseVisualStyleBackColor = true;
            this.btnCmdSendNoise.Click += new System.EventHandler(this.btnCmdSendNoise_Click);
            // 
            // btnCmdAv
            // 
            this.btnCmdAv.Location = new System.Drawing.Point(180, 51);
            this.btnCmdAv.Margin = new System.Windows.Forms.Padding(4);
            this.btnCmdAv.Name = "btnCmdAv";
            this.btnCmdAv.Size = new System.Drawing.Size(69, 27);
            this.btnCmdAv.TabIndex = 46;
            this.btnCmdAv.Text = "AV";
            this.btnCmdAv.UseVisualStyleBackColor = true;
            this.btnCmdAv.Click += new System.EventHandler(this.btnCmdAv_Click);
            // 
            // btnCmdQuery
            // 
            this.btnCmdQuery.Location = new System.Drawing.Point(259, 51);
            this.btnCmdQuery.Margin = new System.Windows.Forms.Padding(4);
            this.btnCmdQuery.Name = "btnCmdQuery";
            this.btnCmdQuery.Size = new System.Drawing.Size(71, 27);
            this.btnCmdQuery.TabIndex = 45;
            this.btnCmdQuery.Text = "QRY";
            this.btnCmdQuery.UseVisualStyleBackColor = true;
            // 
            // btnCmdStopCom
            // 
            this.btnCmdStopCom.Location = new System.Drawing.Point(180, 15);
            this.btnCmdStopCom.Margin = new System.Windows.Forms.Padding(4);
            this.btnCmdStopCom.Name = "btnCmdStopCom";
            this.btnCmdStopCom.Size = new System.Drawing.Size(151, 28);
            this.btnCmdStopCom.TabIndex = 44;
            this.btnCmdStopCom.Text = "Stop Comunicazione";
            this.btnCmdStopCom.UseVisualStyleBackColor = true;
            this.btnCmdStopCom.Click += new System.EventHandler(this.btnCmdStopCom_Click);
            // 
            // btnCmdStartCom
            // 
            this.btnCmdStartCom.Location = new System.Drawing.Point(15, 15);
            this.btnCmdStartCom.Margin = new System.Windows.Forms.Padding(4);
            this.btnCmdStartCom.Name = "btnCmdStartCom";
            this.btnCmdStartCom.Size = new System.Drawing.Size(151, 28);
            this.btnCmdStartCom.TabIndex = 43;
            this.btnCmdStartCom.Text = "Start Comunicazione";
            this.btnCmdStartCom.UseVisualStyleBackColor = true;
            this.btnCmdStartCom.Click += new System.EventHandler(this.btnCmdStartCom_Click);
            // 
            // pnlComandiLista
            // 
            this.pnlComandiLista.BackColor = System.Drawing.Color.White;
            this.pnlComandiLista.Controls.Add(this.btnSigLogReset);
            this.pnlComandiLista.Controls.Add(this.btnSigListCLS);
            this.pnlComandiLista.Controls.Add(this.btnSigEchoCLS);
            this.pnlComandiLista.Location = new System.Drawing.Point(24, 174);
            this.pnlComandiLista.Name = "pnlComandiLista";
            this.pnlComandiLista.Size = new System.Drawing.Size(346, 62);
            this.pnlComandiLista.TabIndex = 24;
            // 
            // tmrInvioAutomatico
            // 
            this.tmrInvioAutomatico.Interval = 10000;
            this.tmrInvioAutomatico.Tick += new System.EventHandler(this.tmrInvioAutomatico_Tick);
            // 
            // rtfSerialEcho
            // 
            this.rtfSerialEcho.Font = new System.Drawing.Font("Courier New", 7.8F);
            this.rtfSerialEcho.Location = new System.Drawing.Point(23, 562);
            this.rtfSerialEcho.Name = "rtfSerialEcho";
            this.rtfSerialEcho.Size = new System.Drawing.Size(1739, 163);
            this.rtfSerialEcho.TabIndex = 25;
            this.rtfSerialEcho.Text = "";
            // 
            // trbOcMonitorZoom
            // 
            this.trbOcMonitorZoom.Location = new System.Drawing.Point(23, 747);
            this.trbOcMonitorZoom.Minimum = 1;
            this.trbOcMonitorZoom.Name = "trbOcMonitorZoom";
            this.trbOcMonitorZoom.Size = new System.Drawing.Size(352, 56);
            this.trbOcMonitorZoom.TabIndex = 26;
            this.trbOcMonitorZoom.Value = 1;
            this.trbOcMonitorZoom.ValueChanged += new System.EventHandler(this.trbOcMonitorZoom_ValueChanged);
            // 
            // frmMonitorSig60
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(1790, 826);
            this.Controls.Add(this.trbOcMonitorZoom);
            this.Controls.Add(this.rtfSerialEcho);
            this.Controls.Add(this.pnlComandiLista);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlFile);
            this.Controls.Add(this.flvListaComandiSIG);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.pnlPorta);
            this.Name = "frmMonitorSig60";
            this.ShowIcon = false;
            this.Text = "Monitor Sig60";
            this.Load += new System.EventHandler(this.frmMonitorSig60_Load);
            this.Resize += new System.EventHandler(this.frmMonitorSig60_Resize);
            this.pnlPorta.ResumeLayout(false);
            this.pnlPorta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvListaComandiSIG)).EndInit();
            this.pnlFile.ResumeLayout(false);
            this.pnlFile.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlComandiLista.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trbOcMonitorZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlPorta;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPorta;
        private System.Windows.Forms.Button btnPortState;
        private System.Windows.Forms.ComboBox cboBaudRate;
        private System.Windows.Forms.ComboBox cboPorts;
        private System.Windows.Forms.Button btnGetSerialPorts;
        private System.Windows.Forms.Button btnSetSigRegister;
        private System.Windows.Forms.Button btnGetSigRegister;
        private System.Windows.Forms.Button btnSigEchoCLS;
        private System.Windows.Forms.Button btnChiudi;
        private System.Windows.Forms.Button btnSigListCLS;
        private BrightIdeasSoftware.FastObjectListView flvListaComandiSIG;
        private System.Windows.Forms.Panel pnlFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEchoFilename;
        private System.Windows.Forms.Button btnCariacaRegistrazione;
        private System.Windows.Forms.Button btnSalvaRegistrazione;
        private System.Windows.Forms.Button btnSfogliaSalva;
        private System.Windows.Forms.TextBox txtEchoFileNote;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSigLogReset;
        private System.Windows.Forms.Button btnSfogliaCarica;
        private System.Windows.Forms.SaveFileDialog sfdExportDati;
        private System.Windows.Forms.OpenFileDialog ofdImportDati;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCmdAv;
        private System.Windows.Forms.Button btnCmdQuery;
        private System.Windows.Forms.Button btnCmdStopCom;
        private System.Windows.Forms.Button btnCmdStartCom;
        private System.Windows.Forms.Panel pnlComandiLista;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtNumCaratteri;
        private System.Windows.Forms.CheckBox chkSendStop;
        private System.Windows.Forms.CheckBox chkSendStart;
        private System.Windows.Forms.Button btnCmdSendNoise;
        private System.Windows.Forms.CheckBox chkSendAuto;
        private System.Windows.Forms.Timer tmrInvioAutomatico;
        private System.Windows.Forms.RichTextBox rtfSerialEcho;
        private System.Windows.Forms.TrackBar trbOcMonitorZoom;
    }
}