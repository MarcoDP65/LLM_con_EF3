namespace PannelloCharger
{
    partial class frmInserimentoProgramma
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmInserimentoProgramma));
            this.txtProgcBattVdef = new System.Windows.Forms.TextBox();
            this.txtTensioneNom = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtProgcBattAhDef = new System.Windows.Forms.TextBox();
            this.txtProgcCelleV1 = new System.Windows.Forms.TextBox();
            this.lblCelleP1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtProgcCelleTot = new System.Windows.Forms.TextBox();
            this.txtProgcCelleV3 = new System.Windows.Forms.TextBox();
            this.txtProgcCelleV2 = new System.Windows.Forms.TextBox();
            this.lblTipoBatteria = new System.Windows.Forms.Label();
            this.txtProgcBattType = new System.Windows.Forms.TextBox();
            this.chkMemProgrammed = new System.Windows.Forms.CheckBox();
            this.btnInserisciProgramma = new System.Windows.Forms.Button();
            this.btnAnnulla = new System.Windows.Forms.Button();
            this.lblUMVolt = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTipoBatteria = new System.Windows.Forms.ComboBox();
            this.chkSondaElPresente = new System.Windows.Forms.CheckBox();
            this.lblTempMin = new System.Windows.Forms.Label();
            this.txtTempMin = new System.Windows.Forms.TextBox();
            this.lblTempMax = new System.Windows.Forms.Label();
            this.txtTempMax = new System.Windows.Forms.TextBox();
            this.grbVersoCorrente = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pbxInverso = new System.Windows.Forms.PictureBox();
            this.pbxDiretto = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtNumSpire = new System.Windows.Forms.TextBox();
            this.lblNumSpire = new System.Windows.Forms.Label();
            this.grbVersoCorrente.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxInverso)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxDiretto)).BeginInit();
            this.SuspendLayout();
            // 
            // txtProgcBattVdef
            // 
            this.txtProgcBattVdef.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgcBattVdef.Location = new System.Drawing.Point(42, 50);
            this.txtProgcBattVdef.Name = "txtProgcBattVdef";
            this.txtProgcBattVdef.Size = new System.Drawing.Size(91, 30);
            this.txtProgcBattVdef.TabIndex = 2;
            this.txtProgcBattVdef.Text = "0";
            this.txtProgcBattVdef.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtProgcBattVdef.Leave += new System.EventHandler(this.txtProgcBattVdef_Leave);
            // 
            // txtTensioneNom
            // 
            this.txtTensioneNom.AutoSize = true;
            this.txtTensioneNom.Location = new System.Drawing.Point(39, 30);
            this.txtTensioneNom.Name = "txtTensioneNom";
            this.txtTensioneNom.Size = new System.Drawing.Size(128, 17);
            this.txtTensioneNom.TabIndex = 1;
            this.txtTensioneNom.Text = "Tensione nominale";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(210, 29);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(124, 17);
            this.label16.TabIndex = 3;
            this.label16.Text = "Capacità nominale";
            // 
            // txtProgcBattAhDef
            // 
            this.txtProgcBattAhDef.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgcBattAhDef.Location = new System.Drawing.Point(212, 50);
            this.txtProgcBattAhDef.Name = "txtProgcBattAhDef";
            this.txtProgcBattAhDef.Size = new System.Drawing.Size(91, 30);
            this.txtProgcBattAhDef.TabIndex = 4;
            this.txtProgcBattAhDef.Text = "0";
            this.txtProgcBattAhDef.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtProgcCelleV1
            // 
            this.txtProgcCelleV1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgcCelleV1.Location = new System.Drawing.Point(560, 116);
            this.txtProgcCelleV1.Name = "txtProgcCelleV1";
            this.txtProgcCelleV1.Size = new System.Drawing.Size(57, 24);
            this.txtProgcCelleV1.TabIndex = 12;
            this.txtProgcCelleV1.Text = "0";
            this.txtProgcCelleV1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCelleP1
            // 
            this.lblCelleP1.AutoSize = true;
            this.lblCelleP1.Location = new System.Drawing.Point(567, 96);
            this.lblCelleP1.Name = "lblCelleP1";
            this.lblCelleP1.Size = new System.Drawing.Size(60, 17);
            this.lblCelleP1.TabIndex = 11;
            this.lblCelleP1.Text = "Celle V1";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(431, 29);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 17);
            this.label13.TabIndex = 5;
            this.label13.Text = "Celle Tot";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(435, 96);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 17);
            this.label14.TabIndex = 7;
            this.label14.Text = "Celle V3";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(501, 96);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(60, 17);
            this.label15.TabIndex = 9;
            this.label15.Text = "Celle V2";
            // 
            // txtProgcCelleTot
            // 
            this.txtProgcCelleTot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgcCelleTot.Location = new System.Drawing.Point(434, 49);
            this.txtProgcCelleTot.Name = "txtProgcCelleTot";
            this.txtProgcCelleTot.Size = new System.Drawing.Size(57, 30);
            this.txtProgcCelleTot.TabIndex = 6;
            this.txtProgcCelleTot.Text = "0";
            this.txtProgcCelleTot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtProgcCelleV3
            // 
            this.txtProgcCelleV3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgcCelleV3.Location = new System.Drawing.Point(434, 116);
            this.txtProgcCelleV3.Name = "txtProgcCelleV3";
            this.txtProgcCelleV3.Size = new System.Drawing.Size(57, 24);
            this.txtProgcCelleV3.TabIndex = 8;
            this.txtProgcCelleV3.Text = "0";
            this.txtProgcCelleV3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtProgcCelleV2
            // 
            this.txtProgcCelleV2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgcCelleV2.Location = new System.Drawing.Point(497, 116);
            this.txtProgcCelleV2.Name = "txtProgcCelleV2";
            this.txtProgcCelleV2.Size = new System.Drawing.Size(57, 24);
            this.txtProgcCelleV2.TabIndex = 10;
            this.txtProgcCelleV2.Text = "0";
            this.txtProgcCelleV2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTipoBatteria
            // 
            this.lblTipoBatteria.AutoSize = true;
            this.lblTipoBatteria.Location = new System.Drawing.Point(39, 96);
            this.lblTipoBatteria.Name = "lblTipoBatteria";
            this.lblTipoBatteria.Size = new System.Drawing.Size(89, 17);
            this.lblTipoBatteria.TabIndex = 13;
            this.lblTipoBatteria.Text = "Tipo Batteria";
            // 
            // txtProgcBattType
            // 
            this.txtProgcBattType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProgcBattType.Location = new System.Drawing.Point(578, 342);
            this.txtProgcBattType.Name = "txtProgcBattType";
            this.txtProgcBattType.Size = new System.Drawing.Size(99, 24);
            this.txtProgcBattType.TabIndex = 14;
            this.txtProgcBattType.Text = "0";
            this.txtProgcBattType.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtProgcBattType.Visible = false;
            // 
            // chkMemProgrammed
            // 
            this.chkMemProgrammed.AutoSize = true;
            this.chkMemProgrammed.Checked = true;
            this.chkMemProgrammed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMemProgrammed.Location = new System.Drawing.Point(410, 303);
            this.chkMemProgrammed.Name = "chkMemProgrammed";
            this.chkMemProgrammed.Size = new System.Drawing.Size(164, 21);
            this.chkMemProgrammed.TabIndex = 15;
            this.chkMemProgrammed.Text = "Attiva Configurazione";
            this.chkMemProgrammed.UseVisualStyleBackColor = true;
            // 
            // btnInserisciProgramma
            // 
            this.btnInserisciProgramma.Location = new System.Drawing.Point(139, 374);
            this.btnInserisciProgramma.Name = "btnInserisciProgramma";
            this.btnInserisciProgramma.Size = new System.Drawing.Size(172, 38);
            this.btnInserisciProgramma.TabIndex = 16;
            this.btnInserisciProgramma.Text = "Inserisci Configurazione";
            this.btnInserisciProgramma.UseVisualStyleBackColor = true;
            this.btnInserisciProgramma.Click += new System.EventHandler(this.btnInserisciProgramma_Click);
            // 
            // btnAnnulla
            // 
            this.btnAnnulla.Location = new System.Drawing.Point(347, 374);
            this.btnAnnulla.Name = "btnAnnulla";
            this.btnAnnulla.Size = new System.Drawing.Size(172, 38);
            this.btnAnnulla.TabIndex = 17;
            this.btnAnnulla.Text = "Annulla";
            this.btnAnnulla.UseVisualStyleBackColor = true;
            this.btnAnnulla.Click += new System.EventHandler(this.btnAnnulla_Click);
            // 
            // lblUMVolt
            // 
            this.lblUMVolt.AutoSize = true;
            this.lblUMVolt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUMVolt.Location = new System.Drawing.Point(139, 55);
            this.lblUMVolt.Name = "lblUMVolt";
            this.lblUMVolt.Size = new System.Drawing.Size(23, 24);
            this.lblUMVolt.TabIndex = 18;
            this.lblUMVolt.Text = "V";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(310, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 24);
            this.label1.TabIndex = 19;
            this.label1.Text = "Ah";
            // 
            // cmbTipoBatteria
            // 
            this.cmbTipoBatteria.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTipoBatteria.FormattingEnabled = true;
            this.cmbTipoBatteria.Location = new System.Drawing.Point(42, 116);
            this.cmbTipoBatteria.Name = "cmbTipoBatteria";
            this.cmbTipoBatteria.Size = new System.Drawing.Size(125, 26);
            this.cmbTipoBatteria.TabIndex = 20;
            // 
            // chkSondaElPresente
            // 
            this.chkSondaElPresente.AutoSize = true;
            this.chkSondaElPresente.Checked = true;
            this.chkSondaElPresente.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSondaElPresente.Location = new System.Drawing.Point(410, 250);
            this.chkSondaElPresente.Name = "chkSondaElPresente";
            this.chkSondaElPresente.Size = new System.Drawing.Size(194, 21);
            this.chkSondaElPresente.TabIndex = 21;
            this.chkSondaElPresente.Text = "Sonda Elettrolita Installata";
            this.chkSondaElPresente.UseVisualStyleBackColor = true;
            // 
            // lblTempMin
            // 
            this.lblTempMin.AutoSize = true;
            this.lblTempMin.Location = new System.Drawing.Point(431, 160);
            this.lblTempMin.Name = "lblTempMin";
            this.lblTempMin.Size = new System.Drawing.Size(43, 17);
            this.lblTempMin.TabIndex = 23;
            this.lblTempMin.Text = "T Min";
            // 
            // txtTempMin
            // 
            this.txtTempMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTempMin.Location = new System.Drawing.Point(434, 180);
            this.txtTempMin.Name = "txtTempMin";
            this.txtTempMin.Size = new System.Drawing.Size(57, 24);
            this.txtTempMin.TabIndex = 24;
            this.txtTempMin.Text = "0";
            this.txtTempMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTempMax
            // 
            this.lblTempMax.AutoSize = true;
            this.lblTempMax.Location = new System.Drawing.Point(508, 160);
            this.lblTempMax.Name = "lblTempMax";
            this.lblTempMax.Size = new System.Drawing.Size(46, 17);
            this.lblTempMax.TabIndex = 25;
            this.lblTempMax.Text = "T Max";
            // 
            // txtTempMax
            // 
            this.txtTempMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTempMax.Location = new System.Drawing.Point(511, 180);
            this.txtTempMax.Name = "txtTempMax";
            this.txtTempMax.Size = new System.Drawing.Size(57, 24);
            this.txtTempMax.TabIndex = 26;
            this.txtTempMax.Text = "0";
            this.txtTempMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // grbVersoCorrente
            // 
            this.grbVersoCorrente.BackColor = System.Drawing.Color.White;
            this.grbVersoCorrente.Controls.Add(this.pictureBox2);
            this.grbVersoCorrente.Controls.Add(this.pictureBox1);
            this.grbVersoCorrente.Controls.Add(this.pbxInverso);
            this.grbVersoCorrente.Controls.Add(this.pbxDiretto);
            this.grbVersoCorrente.Location = new System.Drawing.Point(42, 198);
            this.grbVersoCorrente.Name = "grbVersoCorrente";
            this.grbVersoCorrente.Size = new System.Drawing.Size(322, 139);
            this.grbVersoCorrente.TabIndex = 27;
            this.grbVersoCorrente.TabStop = false;
            this.grbVersoCorrente.Text = "Verso Installazione";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(76, 83);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(34, 33);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(217, 83);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(34, 33);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // pbxInverso
            // 
            this.pbxInverso.Image = ((System.Drawing.Image)(resources.GetObject("pbxInverso.Image")));
            this.pbxInverso.Location = new System.Drawing.Point(171, 32);
            this.pbxInverso.Name = "pbxInverso";
            this.pbxInverso.Size = new System.Drawing.Size(122, 67);
            this.pbxInverso.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxInverso.TabIndex = 4;
            this.pbxInverso.TabStop = false;
            // 
            // pbxDiretto
            // 
            this.pbxDiretto.Image = ((System.Drawing.Image)(resources.GetObject("pbxDiretto.Image")));
            this.pbxDiretto.InitialImage = ((System.Drawing.Image)(resources.GetObject("pbxDiretto.InitialImage")));
            this.pbxDiretto.Location = new System.Drawing.Point(27, 32);
            this.pbxDiretto.Name = "pbxDiretto";
            this.pbxDiretto.Size = new System.Drawing.Size(121, 67);
            this.pbxDiretto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbxDiretto.TabIndex = 0;
            this.pbxDiretto.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtNumSpire
            // 
            this.txtNumSpire.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumSpire.Location = new System.Drawing.Point(213, 116);
            this.txtNumSpire.Name = "txtNumSpire";
            this.txtNumSpire.Size = new System.Drawing.Size(90, 30);
            this.txtNumSpire.TabIndex = 29;
            this.txtNumSpire.Text = "1";
            this.txtNumSpire.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNumSpire.TextChanged += new System.EventHandler(this.txtNumSpire_TextChanged);
            // 
            // lblNumSpire
            // 
            this.lblNumSpire.AutoSize = true;
            this.lblNumSpire.Location = new System.Drawing.Point(209, 96);
            this.lblNumSpire.Name = "lblNumSpire";
            this.lblNumSpire.Size = new System.Drawing.Size(95, 17);
            this.lblNumSpire.TabIndex = 28;
            this.lblNumSpire.Text = "Numero Spire";
            this.lblNumSpire.Click += new System.EventHandler(this.lblNumSpire_Click);
            // 
            // frmInserimentoProgramma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(689, 441);
            this.Controls.Add(this.txtNumSpire);
            this.Controls.Add(this.lblNumSpire);
            this.Controls.Add(this.grbVersoCorrente);
            this.Controls.Add(this.lblTempMax);
            this.Controls.Add(this.txtTempMax);
            this.Controls.Add(this.lblTempMin);
            this.Controls.Add(this.cmbTipoBatteria);
            this.Controls.Add(this.txtTempMin);
            this.Controls.Add(this.chkSondaElPresente);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblUMVolt);
            this.Controls.Add(this.btnAnnulla);
            this.Controls.Add(this.btnInserisciProgramma);
            this.Controls.Add(this.chkMemProgrammed);
            this.Controls.Add(this.lblTipoBatteria);
            this.Controls.Add(this.txtProgcBattType);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtProgcCelleV1);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblCelleP1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtProgcCelleTot);
            this.Controls.Add(this.txtProgcBattAhDef);
            this.Controls.Add(this.txtProgcCelleV3);
            this.Controls.Add(this.txtProgcBattVdef);
            this.Controls.Add(this.txtProgcCelleV2);
            this.Controls.Add(this.txtTensioneNom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmInserimentoProgramma";
            this.Text = "Nuova Configurazione";
            this.Load += new System.EventHandler(this.frmInserimentoProgramma_Load);
            this.grbVersoCorrente.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxInverso)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxDiretto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtProgcBattVdef;
        private System.Windows.Forms.Label txtTensioneNom;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtProgcBattAhDef;
        private System.Windows.Forms.TextBox txtProgcCelleV1;
        private System.Windows.Forms.Label lblCelleP1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtProgcCelleTot;
        private System.Windows.Forms.TextBox txtProgcCelleV3;
        private System.Windows.Forms.TextBox txtProgcCelleV2;
        private System.Windows.Forms.Label lblTipoBatteria;
        private System.Windows.Forms.TextBox txtProgcBattType;
        private System.Windows.Forms.CheckBox chkMemProgrammed;
        private System.Windows.Forms.Button btnInserisciProgramma;
        private System.Windows.Forms.Button btnAnnulla;
        private System.Windows.Forms.Label lblUMVolt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTipoBatteria;
        private System.Windows.Forms.CheckBox chkSondaElPresente;
        private System.Windows.Forms.Label lblTempMin;
        private System.Windows.Forms.TextBox txtTempMin;
        private System.Windows.Forms.Label lblTempMax;
        private System.Windows.Forms.TextBox txtTempMax;
        private System.Windows.Forms.GroupBox grbVersoCorrente;
        private System.Windows.Forms.PictureBox pbxDiretto;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtNumSpire;
        private System.Windows.Forms.Label lblNumSpire;
        private System.Windows.Forms.PictureBox pbxInverso;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}