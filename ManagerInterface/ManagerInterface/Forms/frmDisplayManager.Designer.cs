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
            this.tbpRealTime = new System.Windows.Forms.TabPage();
            this.tbpImmagini = new System.Windows.Forms.TabPage();
            this.tbpVariabili = new System.Windows.Forms.TabPage();
            this.tbpPagine = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPortState = new System.Windows.Forms.Button();
            this.cboHandShaking = new System.Windows.Forms.ComboBox();
            this.cboParity = new System.Windows.Forms.ComboBox();
            this.cboStopBits = new System.Windows.Forms.ComboBox();
            this.cboDataBits = new System.Windows.Forms.ComboBox();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.cboPorts = new System.Windows.Forms.ComboBox();
            this.btnGetSerialPorts = new System.Windows.Forms.Button();
            this.rtbhex = new System.Windows.Forms.RichTextBox();
            this.rtbOutgoing = new System.Windows.Forms.RichTextBox();
            this.lblRIStatus = new System.Windows.Forms.Label();
            this.lblDSRStatus = new System.Windows.Forms.Label();
            this.lblCTSStatus = new System.Windows.Forms.Label();
            this.lblBreakStatus = new System.Windows.Forms.Label();
            this.rtbIncoming = new System.Windows.Forms.RichTextBox();
            this.pnlComandiImmediati = new System.Windows.Forms.Panel();
            this.btnPrimaLettura = new System.Windows.Forms.Button();
            this.cmdLeggiRTC = new System.Windows.Forms.Button();
            this.btnApriComunicazione = new System.Windows.Forms.Button();
            this.lblPorta = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.chkRtBacklight = new System.Windows.Forms.CheckBox();
            this.txtRtValRed = new System.Windows.Forms.TextBox();
            this.label182 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRtValGreen = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRtValBlu = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRtValTimeOn = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRtValTimeOff = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnRtSetLed = new System.Windows.Forms.Button();
            this.btnRtStopLed = new System.Windows.Forms.Button();
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
            this.tbcMainDisplayManager.SuspendLayout();
            this.tbpConnessione.SuspendLayout();
            this.tbpRealTime.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlComandiImmediati.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbcMainDisplayManager
            // 
            this.tbcMainDisplayManager.Controls.Add(this.tbpConnessione);
            this.tbcMainDisplayManager.Controls.Add(this.tbpRealTime);
            this.tbcMainDisplayManager.Controls.Add(this.tbpImmagini);
            this.tbcMainDisplayManager.Controls.Add(this.tbpVariabili);
            this.tbcMainDisplayManager.Controls.Add(this.tbpPagine);
            this.tbcMainDisplayManager.Location = new System.Drawing.Point(25, 26);
            this.tbcMainDisplayManager.Name = "tbcMainDisplayManager";
            this.tbcMainDisplayManager.SelectedIndex = 0;
            this.tbcMainDisplayManager.Size = new System.Drawing.Size(1332, 640);
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
            this.tbpConnessione.Size = new System.Drawing.Size(1324, 611);
            this.tbpConnessione.TabIndex = 0;
            this.tbpConnessione.Text = "Connessione";
            // 
            // tbpRealTime
            // 
            this.tbpRealTime.BackColor = System.Drawing.Color.LightYellow;
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
            this.tbpRealTime.Size = new System.Drawing.Size(1324, 611);
            this.tbpRealTime.TabIndex = 1;
            this.tbpRealTime.Text = "RealTime";
            // 
            // tbpImmagini
            // 
            this.tbpImmagini.Location = new System.Drawing.Point(4, 25);
            this.tbpImmagini.Name = "tbpImmagini";
            this.tbpImmagini.Size = new System.Drawing.Size(1324, 611);
            this.tbpImmagini.TabIndex = 2;
            this.tbpImmagini.Text = "Immagini";
            this.tbpImmagini.UseVisualStyleBackColor = true;
            // 
            // tbpVariabili
            // 
            this.tbpVariabili.Location = new System.Drawing.Point(4, 25);
            this.tbpVariabili.Name = "tbpVariabili";
            this.tbpVariabili.Size = new System.Drawing.Size(1324, 611);
            this.tbpVariabili.TabIndex = 3;
            this.tbpVariabili.Text = "Variabili";
            this.tbpVariabili.UseVisualStyleBackColor = true;
            // 
            // tbpPagine
            // 
            this.tbpPagine.Location = new System.Drawing.Point(4, 25);
            this.tbpPagine.Name = "tbpPagine";
            this.tbpPagine.Size = new System.Drawing.Size(1324, 611);
            this.tbpPagine.TabIndex = 4;
            this.tbpPagine.Text = "Pagine";
            this.tbpPagine.UseVisualStyleBackColor = true;
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
            this.panel1.Location = new System.Drawing.Point(33, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(537, 568);
            this.panel1.TabIndex = 0;
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
            // pnlComandiImmediati
            // 
            this.pnlComandiImmediati.BackColor = System.Drawing.Color.White;
            this.pnlComandiImmediati.Controls.Add(this.button1);
            this.pnlComandiImmediati.Controls.Add(this.btnPrimaLettura);
            this.pnlComandiImmediati.Controls.Add(this.cmdLeggiRTC);
            this.pnlComandiImmediati.Controls.Add(this.btnApriComunicazione);
            this.pnlComandiImmediati.Location = new System.Drawing.Point(661, 38);
            this.pnlComandiImmediati.Name = "pnlComandiImmediati";
            this.pnlComandiImmediati.Size = new System.Drawing.Size(298, 259);
            this.pnlComandiImmediati.TabIndex = 1;
            // 
            // btnPrimaLettura
            // 
            this.btnPrimaLettura.Location = new System.Drawing.Point(28, 197);
            this.btnPrimaLettura.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrimaLettura.Name = "btnPrimaLettura";
            this.btnPrimaLettura.Size = new System.Drawing.Size(136, 28);
            this.btnPrimaLettura.TabIndex = 35;
            this.btnPrimaLettura.Text = "Prima Lettura";
            this.btnPrimaLettura.UseVisualStyleBackColor = true;
            // 
            // cmdLeggiRTC
            // 
            this.cmdLeggiRTC.Location = new System.Drawing.Point(28, 161);
            this.cmdLeggiRTC.Margin = new System.Windows.Forms.Padding(4);
            this.cmdLeggiRTC.Name = "cmdLeggiRTC";
            this.cmdLeggiRTC.Size = new System.Drawing.Size(136, 28);
            this.cmdLeggiRTC.TabIndex = 34;
            this.cmdLeggiRTC.Text = "Read RTC";
            this.cmdLeggiRTC.UseVisualStyleBackColor = true;
            // 
            // btnApriComunicazione
            // 
            this.btnApriComunicazione.Location = new System.Drawing.Point(28, 22);
            this.btnApriComunicazione.Margin = new System.Windows.Forms.Padding(4);
            this.btnApriComunicazione.Name = "btnApriComunicazione";
            this.btnApriComunicazione.Size = new System.Drawing.Size(192, 28);
            this.btnApriComunicazione.TabIndex = 33;
            this.btnApriComunicazione.Text = "Start Comunicazione";
            this.btnApriComunicazione.UseVisualStyleBackColor = true;
            this.btnApriComunicazione.Click += new System.EventHandler(this.btnApriComunicazione_Click);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 41;
            this.label1.Text = "Baudrate";
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(184, 128);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 17);
            this.label3.TabIndex = 43;
            this.label3.Text = "Bit Stop";
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(184, 195);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 17);
            this.label5.TabIndex = 45;
            this.label5.Text = "Handshake";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(28, 60);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(192, 28);
            this.button1.TabIndex = 36;
            this.button1.Text = "Stop Comunicazione";
            this.button1.UseVisualStyleBackColor = true;
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
            // txtRtValRed
            // 
            this.txtRtValRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValRed.Location = new System.Drawing.Point(177, 129);
            this.txtRtValRed.Name = "txtRtValRed";
            this.txtRtValRed.Size = new System.Drawing.Size(48, 24);
            this.txtRtValRed.TabIndex = 56;
            this.txtRtValRed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label182
            // 
            this.label182.AutoSize = true;
            this.label182.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label182.Location = new System.Drawing.Point(174, 109);
            this.label182.Name = "label182";
            this.label182.Size = new System.Drawing.Size(37, 17);
            this.label182.TabIndex = 55;
            this.label182.Text = "RED";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(64, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 20);
            this.label6.TabIndex = 57;
            this.label6.Text = "LED";
            // 
            // txtRtValGreen
            // 
            this.txtRtValGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValGreen.Location = new System.Drawing.Point(231, 129);
            this.txtRtValGreen.Name = "txtRtValGreen";
            this.txtRtValGreen.Size = new System.Drawing.Size(48, 24);
            this.txtRtValGreen.TabIndex = 59;
            this.txtRtValGreen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(228, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 17);
            this.label7.TabIndex = 58;
            this.label7.Text = "GREEN";
            // 
            // txtRtValBlu
            // 
            this.txtRtValBlu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValBlu.Location = new System.Drawing.Point(285, 129);
            this.txtRtValBlu.Name = "txtRtValBlu";
            this.txtRtValBlu.Size = new System.Drawing.Size(48, 24);
            this.txtRtValBlu.TabIndex = 61;
            this.txtRtValBlu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(285, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 17);
            this.label8.TabIndex = 60;
            this.label8.Text = "BLU";
            // 
            // txtRtValTimeOn
            // 
            this.txtRtValTimeOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValTimeOn.Location = new System.Drawing.Point(369, 129);
            this.txtRtValTimeOn.Name = "txtRtValTimeOn";
            this.txtRtValTimeOn.Size = new System.Drawing.Size(48, 24);
            this.txtRtValTimeOn.TabIndex = 63;
            this.txtRtValTimeOn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(366, 109);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 17);
            this.label9.TabIndex = 62;
            this.label9.Text = "ON";
            // 
            // txtRtValTimeOff
            // 
            this.txtRtValTimeOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValTimeOff.Location = new System.Drawing.Point(423, 129);
            this.txtRtValTimeOff.Name = "txtRtValTimeOff";
            this.txtRtValTimeOff.Size = new System.Drawing.Size(48, 24);
            this.txtRtValTimeOff.TabIndex = 65;
            this.txtRtValTimeOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(420, 109);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 17);
            this.label10.TabIndex = 64;
            this.label10.Text = "OFF";
            // 
            // btnRtSetLed
            // 
            this.btnRtSetLed.Location = new System.Drawing.Point(561, 122);
            this.btnRtSetLed.Name = "btnRtSetLed";
            this.btnRtSetLed.Size = new System.Drawing.Size(136, 41);
            this.btnRtSetLed.TabIndex = 66;
            this.btnRtSetLed.Text = "Set Led";
            this.btnRtSetLed.UseVisualStyleBackColor = true;
            this.btnRtSetLed.Click += new System.EventHandler(this.btnRtSetLed_Click);
            // 
            // btnRtStopLed
            // 
            this.btnRtStopLed.Location = new System.Drawing.Point(703, 122);
            this.btnRtStopLed.Name = "btnRtStopLed";
            this.btnRtStopLed.Size = new System.Drawing.Size(136, 41);
            this.btnRtStopLed.TabIndex = 67;
            this.btnRtStopLed.Text = "Reset Led";
            this.btnRtStopLed.UseVisualStyleBackColor = true;
            this.btnRtStopLed.Click += new System.EventHandler(this.btnRtStopLed_Click);
            // 
            // btnRtDrawLine
            // 
            this.btnRtDrawLine.Location = new System.Drawing.Point(560, 206);
            this.btnRtDrawLine.Name = "btnRtDrawLine";
            this.btnRtDrawLine.Size = new System.Drawing.Size(136, 41);
            this.btnRtDrawLine.TabIndex = 79;
            this.btnRtDrawLine.Text = "Disegna Linea";
            this.btnRtDrawLine.UseVisualStyleBackColor = true;
            this.btnRtDrawLine.Click += new System.EventHandler(this.btnRtDrawLine_Click);
            // 
            // txtRtValLineColor
            // 
            this.txtRtValLineColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValLineColor.Location = new System.Drawing.Point(452, 213);
            this.txtRtValLineColor.Name = "txtRtValLineColor";
            this.txtRtValLineColor.Size = new System.Drawing.Size(48, 24);
            this.txtRtValLineColor.TabIndex = 78;
            this.txtRtValLineColor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(449, 193);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 17);
            this.label11.TabIndex = 77;
            this.label11.Text = "Colore";
            // 
            // txtRtValLineYFine
            // 
            this.txtRtValLineYFine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValLineYFine.Location = new System.Drawing.Point(369, 213);
            this.txtRtValLineYFine.Name = "txtRtValLineYFine";
            this.txtRtValLineYFine.Size = new System.Drawing.Size(48, 24);
            this.txtRtValLineYFine.TabIndex = 76;
            this.txtRtValLineYFine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(365, 193);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 17);
            this.label12.TabIndex = 75;
            this.label12.Text = "Y fine";
            // 
            // txtRtValLineXFine
            // 
            this.txtRtValLineXFine.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValLineXFine.Location = new System.Drawing.Point(311, 213);
            this.txtRtValLineXFine.Name = "txtRtValLineXFine";
            this.txtRtValLineXFine.Size = new System.Drawing.Size(48, 24);
            this.txtRtValLineXFine.TabIndex = 74;
            this.txtRtValLineXFine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(308, 193);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 17);
            this.label13.TabIndex = 73;
            this.label13.Text = "X fine";
            // 
            // txtRtValLineYStart
            // 
            this.txtRtValLineYStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValLineYStart.Location = new System.Drawing.Point(230, 213);
            this.txtRtValLineYStart.Name = "txtRtValLineYStart";
            this.txtRtValLineYStart.Size = new System.Drawing.Size(48, 24);
            this.txtRtValLineYStart.TabIndex = 72;
            this.txtRtValLineYStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(227, 193);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 17);
            this.label14.TabIndex = 71;
            this.label14.Text = "Y start";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(63, 215);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 20);
            this.label15.TabIndex = 70;
            this.label15.Text = "LINEA";
            // 
            // txtRtValLineXStart
            // 
            this.txtRtValLineXStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtRtValLineXStart.Location = new System.Drawing.Point(176, 213);
            this.txtRtValLineXStart.Name = "txtRtValLineXStart";
            this.txtRtValLineXStart.Size = new System.Drawing.Size(48, 24);
            this.txtRtValLineXStart.TabIndex = 69;
            this.txtRtValLineXStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(173, 193);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(49, 17);
            this.label16.TabIndex = 68;
            this.label16.Text = "X start";
            // 
            // frmDisplayManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1402, 735);
            this.Controls.Add(this.tbcMainDisplayManager);
            this.Name = "frmDisplayManager";
            this.Text = "frmDisplayManager";
            this.tbcMainDisplayManager.ResumeLayout(false);
            this.tbpConnessione.ResumeLayout(false);
            this.tbpRealTime.ResumeLayout(false);
            this.tbpRealTime.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlComandiImmediati.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcMainDisplayManager;
        private System.Windows.Forms.TabPage tbpConnessione;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tbpRealTime;
        private System.Windows.Forms.TabPage tbpImmagini;
        private System.Windows.Forms.TabPage tbpVariabili;
        private System.Windows.Forms.TabPage tbpPagine;
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
    }
}