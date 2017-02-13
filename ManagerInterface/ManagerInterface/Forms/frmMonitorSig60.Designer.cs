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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSigLogReset = new System.Windows.Forms.Button();
            this.btnSigListCLS = new System.Windows.Forms.Button();
            this.btnSigEchoCLS = new System.Windows.Forms.Button();
            this.btnGetSigRegister = new System.Windows.Forms.Button();
            this.btnSetSigRegister = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPorta = new System.Windows.Forms.Label();
            this.btnPortState = new System.Windows.Forms.Button();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.cboPorts = new System.Windows.Forms.ComboBox();
            this.btnGetSerialPorts = new System.Windows.Forms.Button();
            this.txtSerialEcho = new System.Windows.Forms.TextBox();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.flvListaComandiSIG = new BrightIdeasSoftware.FastObjectListView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
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
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvListaComandiSIG)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnSigLogReset);
            this.panel1.Controls.Add(this.btnSigListCLS);
            this.panel1.Controls.Add(this.btnSigEchoCLS);
            this.panel1.Controls.Add(this.btnGetSigRegister);
            this.panel1.Controls.Add(this.btnSetSigRegister);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblPorta);
            this.panel1.Controls.Add(this.btnPortState);
            this.panel1.Controls.Add(this.cboBaudRate);
            this.panel1.Controls.Add(this.cboPorts);
            this.panel1.Controls.Add(this.btnGetSerialPorts);
            this.panel1.Location = new System.Drawing.Point(23, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(347, 240);
            this.panel1.TabIndex = 1;
            // 
            // btnSigLogReset
            // 
            this.btnSigLogReset.ForeColor = System.Drawing.Color.Red;
            this.btnSigLogReset.Location = new System.Drawing.Point(233, 182);
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
            this.btnSigListCLS.Location = new System.Drawing.Point(127, 182);
            this.btnSigListCLS.Margin = new System.Windows.Forms.Padding(4);
            this.btnSigListCLS.Name = "btnSigListCLS";
            this.btnSigListCLS.Size = new System.Drawing.Size(98, 28);
            this.btnSigListCLS.TabIndex = 49;
            this.btnSigListCLS.Text = "Clr SIG LIST";
            this.btnSigListCLS.UseVisualStyleBackColor = true;
            // 
            // btnSigEchoCLS
            // 
            this.btnSigEchoCLS.Location = new System.Drawing.Point(15, 182);
            this.btnSigEchoCLS.Margin = new System.Windows.Forms.Padding(4);
            this.btnSigEchoCLS.Name = "btnSigEchoCLS";
            this.btnSigEchoCLS.Size = new System.Drawing.Size(104, 28);
            this.btnSigEchoCLS.TabIndex = 48;
            this.btnSigEchoCLS.Text = "Clr SIG ECHO";
            this.btnSigEchoCLS.UseVisualStyleBackColor = true;
            this.btnSigEchoCLS.Click += new System.EventHandler(this.btnSigEchoCLS_Click);
            // 
            // btnGetSigRegister
            // 
            this.btnGetSigRegister.Location = new System.Drawing.Point(15, 146);
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
            this.btnSetSigRegister.Location = new System.Drawing.Point(174, 146);
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
            this.label1.Location = new System.Drawing.Point(12, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 41;
            this.label1.Text = "Baudrate";
            // 
            // lblPorta
            // 
            this.lblPorta.AutoSize = true;
            this.lblPorta.Location = new System.Drawing.Point(12, 71);
            this.lblPorta.Name = "lblPorta";
            this.lblPorta.Size = new System.Drawing.Size(42, 17);
            this.lblPorta.TabIndex = 40;
            this.lblPorta.Text = "Porta";
            // 
            // btnPortState
            // 
            this.btnPortState.Location = new System.Drawing.Point(174, 22);
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
            this.cboBaudRate.Location = new System.Drawing.Point(99, 100);
            this.cboBaudRate.Margin = new System.Windows.Forms.Padding(4);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(119, 24);
            this.cboBaudRate.TabIndex = 23;
            // 
            // cboPorts
            // 
            this.cboPorts.FormattingEnabled = true;
            this.cboPorts.Location = new System.Drawing.Point(99, 68);
            this.cboPorts.Margin = new System.Windows.Forms.Padding(4);
            this.cboPorts.Name = "cboPorts";
            this.cboPorts.Size = new System.Drawing.Size(119, 24);
            this.cboPorts.TabIndex = 22;
            // 
            // btnGetSerialPorts
            // 
            this.btnGetSerialPorts.Location = new System.Drawing.Point(15, 22);
            this.btnGetSerialPorts.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetSerialPorts.Name = "btnGetSerialPorts";
            this.btnGetSerialPorts.Size = new System.Drawing.Size(151, 28);
            this.btnGetSerialPorts.TabIndex = 21;
            this.btnGetSerialPorts.Text = "Carica Porte";
            this.btnGetSerialPorts.UseVisualStyleBackColor = true;
            this.btnGetSerialPorts.Click += new System.EventHandler(this.btnGetSerialPorts_Click);
            // 
            // txtSerialEcho
            // 
            this.txtSerialEcho.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerialEcho.Location = new System.Drawing.Point(23, 517);
            this.txtSerialEcho.Multiline = true;
            this.txtSerialEcho.Name = "txtSerialEcho";
            this.txtSerialEcho.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSerialEcho.Size = new System.Drawing.Size(1739, 202);
            this.txtSerialEcho.TabIndex = 2;
            // 
            // btnChiudi
            // 
            this.btnChiudi.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChiudi.Location = new System.Drawing.Point(1637, 734);
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
            this.flvListaComandiSIG.Size = new System.Drawing.Size(1372, 472);
            this.flvListaComandiSIG.TabIndex = 21;
            this.flvListaComandiSIG.UseCompatibleStateImageBehavior = false;
            this.flvListaComandiSIG.View = System.Windows.Forms.View.Details;
            this.flvListaComandiSIG.VirtualMode = true;
            this.flvListaComandiSIG.FormatRow += new System.EventHandler<BrightIdeasSoftware.FormatRowEventArgs>(this.flvListaComandiSIG_FormatRow);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.btnSfogliaCarica);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtEchoFileNote);
            this.panel2.Controls.Add(this.btnCariacaRegistrazione);
            this.panel2.Controls.Add(this.btnSalvaRegistrazione);
            this.panel2.Controls.Add(this.btnSfogliaSalva);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtEchoFilename);
            this.panel2.Location = new System.Drawing.Point(23, 286);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(347, 210);
            this.panel2.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(272, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 49;
            this.label4.Text = "File Dati";
            // 
            // btnSfogliaCarica
            // 
            this.btnSfogliaCarica.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSfogliaCarica.Location = new System.Drawing.Point(189, 68);
            this.btnSfogliaCarica.Name = "btnSfogliaCarica";
            this.btnSfogliaCarica.Size = new System.Drawing.Size(36, 40);
            this.btnSfogliaCarica.TabIndex = 48;
            this.btnSfogliaCarica.Text = "...";
            this.btnSfogliaCarica.UseVisualStyleBackColor = true;
            this.btnSfogliaCarica.Click += new System.EventHandler(this.btnSfogliaCarica_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 47;
            this.label3.Text = "File Dati";
            // 
            // txtEchoFileNote
            // 
            this.txtEchoFileNote.Location = new System.Drawing.Point(15, 140);
            this.txtEchoFileNote.Multiline = true;
            this.txtEchoFileNote.Name = "txtEchoFileNote";
            this.txtEchoFileNote.Size = new System.Drawing.Size(316, 45);
            this.txtEchoFileNote.TabIndex = 46;
            // 
            // btnCariacaRegistrazione
            // 
            this.btnCariacaRegistrazione.Location = new System.Drawing.Point(232, 68);
            this.btnCariacaRegistrazione.Margin = new System.Windows.Forms.Padding(4);
            this.btnCariacaRegistrazione.Name = "btnCariacaRegistrazione";
            this.btnCariacaRegistrazione.Size = new System.Drawing.Size(99, 40);
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
            this.btnSalvaRegistrazione.Size = new System.Drawing.Size(99, 40);
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
            this.btnSfogliaSalva.Size = new System.Drawing.Size(36, 40);
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
            // frmMonitorSig60
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(1790, 801);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.flvListaComandiSIG);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.txtSerialEcho);
            this.Controls.Add(this.panel1);
            this.Name = "frmMonitorSig60";
            this.ShowIcon = false;
            this.Text = "Monitor Sig60";
            this.Load += new System.EventHandler(this.frmMonitorSig60_Load);
            this.ResizeEnd += new System.EventHandler(this.frmMonitorSig60_ResizeEnd);
            this.Resize += new System.EventHandler(this.frmMonitorSig60_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvListaComandiSIG)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblPorta;
        private System.Windows.Forms.Button btnPortState;
        private System.Windows.Forms.ComboBox cboBaudRate;
        private System.Windows.Forms.ComboBox cboPorts;
        private System.Windows.Forms.Button btnGetSerialPorts;
        private System.Windows.Forms.Button btnSetSigRegister;
        private System.Windows.Forms.Button btnGetSigRegister;
        private System.Windows.Forms.TextBox txtSerialEcho;
        private System.Windows.Forms.Button btnSigEchoCLS;
        private System.Windows.Forms.Button btnChiudi;
        private System.Windows.Forms.Button btnSigListCLS;
        private BrightIdeasSoftware.FastObjectListView flvListaComandiSIG;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEchoFilename;
        private System.Windows.Forms.Button btnCariacaRegistrazione;
        private System.Windows.Forms.Button btnSalvaRegistrazione;
        private System.Windows.Forms.Button btnSfogliaSalva;
        private System.Windows.Forms.TextBox txtEchoFileNote;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSigLogReset;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSfogliaCarica;
        private System.Windows.Forms.SaveFileDialog sfdExportDati;
        private System.Windows.Forms.OpenFileDialog ofdImportDati;
    }
}