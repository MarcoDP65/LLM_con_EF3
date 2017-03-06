namespace PannelloCharger
{
    partial class ctrlPannelloTempo
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.mtxDurataCarica = new System.Windows.Forms.MaskedTextBox();
            this.mtxInizioEqual = new System.Windows.Forms.MaskedTextBox();
            this.chkEnableEqual = new System.Windows.Forms.CheckBox();
            this.nudChargeFactor = new System.Windows.Forms.NumericUpDown();
            this.lblDurata = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblOraEqual = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudChargeFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // mtxDurataCarica
            // 
            this.mtxDurataCarica.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxDurataCarica.Location = new System.Drawing.Point(24, 34);
            this.mtxDurataCarica.Mask = "00:00";
            this.mtxDurataCarica.Name = "mtxDurataCarica";
            this.mtxDurataCarica.Size = new System.Drawing.Size(73, 27);
            this.mtxDurataCarica.TabIndex = 1;
            this.mtxDurataCarica.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mtxDurataCarica.ValidatingType = typeof(System.DateTime);
            this.mtxDurataCarica.Leave += new System.EventHandler(this.mtxDurataCarica_Leave);
            // 
            // mtxInizioEqual
            // 
            this.mtxInizioEqual.Enabled = false;
            this.mtxInizioEqual.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxInizioEqual.Location = new System.Drawing.Point(355, 34);
            this.mtxInizioEqual.Mask = "00:00";
            this.mtxInizioEqual.Name = "mtxInizioEqual";
            this.mtxInizioEqual.Size = new System.Drawing.Size(73, 27);
            this.mtxInizioEqual.TabIndex = 2;
            this.mtxInizioEqual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mtxInizioEqual.ValidatingType = typeof(System.DateTime);
            this.mtxInizioEqual.Leave += new System.EventHandler(this.mtxInizioEqual_Leave);
            // 
            // chkEnableEqual
            // 
            this.chkEnableEqual.AutoSize = true;
            this.chkEnableEqual.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableEqual.Location = new System.Drawing.Point(258, 35);
            this.chkEnableEqual.Name = "chkEnableEqual";
            this.chkEnableEqual.Size = new System.Drawing.Size(78, 24);
            this.chkEnableEqual.TabIndex = 3;
            this.chkEnableEqual.Text = "Equal";
            this.chkEnableEqual.UseVisualStyleBackColor = true;
            this.chkEnableEqual.CheckedChanged += new System.EventHandler(this.chkEnableEqual_CheckedChanged);
            // 
            // nudChargeFactor
            // 
            this.nudChargeFactor.DecimalPlaces = 2;
            this.nudChargeFactor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudChargeFactor.Location = new System.Drawing.Point(120, 34);
            this.nudChargeFactor.Maximum = new decimal(new int[] {
            130,
            0,
            0,
            131072});
            this.nudChargeFactor.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            this.nudChargeFactor.Name = "nudChargeFactor";
            this.nudChargeFactor.Size = new System.Drawing.Size(81, 27);
            this.nudChargeFactor.TabIndex = 4;
            this.nudChargeFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudChargeFactor.Value = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            this.nudChargeFactor.ValueChanged += new System.EventHandler(this.nudChargeFactor_ValueChanged);
            // 
            // lblDurata
            // 
            this.lblDurata.AutoSize = true;
            this.lblDurata.Location = new System.Drawing.Point(21, 14);
            this.lblDurata.Name = "lblDurata";
            this.lblDurata.Size = new System.Drawing.Size(76, 17);
            this.lblDurata.TabIndex = 5;
            this.lblDurata.Text = "Ore Carica";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "F.C.";
            // 
            // lblOraEqual
            // 
            this.lblOraEqual.AutoSize = true;
            this.lblOraEqual.Enabled = false;
            this.lblOraEqual.Location = new System.Drawing.Point(352, 14);
            this.lblOraEqual.Name = "lblOraEqual";
            this.lblOraEqual.Size = new System.Drawing.Size(68, 17);
            this.lblOraEqual.TabIndex = 7;
            this.lblOraEqual.Text = "Ora inizio";
            // 
            // ctrlPannelloTempo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.lblOraEqual);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDurata);
            this.Controls.Add(this.nudChargeFactor);
            this.Controls.Add(this.chkEnableEqual);
            this.Controls.Add(this.mtxInizioEqual);
            this.Controls.Add(this.mtxDurataCarica);
            this.Name = "ctrlPannelloTempo";
            this.Size = new System.Drawing.Size(583, 76);
            this.Load += new System.EventHandler(this.ctrlPannelloTempo_Load);
            this.Enter += new System.EventHandler(this.ctrlPannelloTempo_Enter);
            this.Leave += new System.EventHandler(this.ctrlPannelloTempo_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.nudChargeFactor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MaskedTextBox mtxDurataCarica;
        private System.Windows.Forms.MaskedTextBox mtxInizioEqual;
        private System.Windows.Forms.CheckBox chkEnableEqual;
        private System.Windows.Forms.NumericUpDown nudChargeFactor;
        private System.Windows.Forms.Label lblDurata;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblOraEqual;
    }
}
