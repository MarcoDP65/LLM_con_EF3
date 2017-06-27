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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctrlPannelloTempo));
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
            resources.ApplyResources(this.mtxDurataCarica, "mtxDurataCarica");
            this.mtxDurataCarica.Name = "mtxDurataCarica";
            this.mtxDurataCarica.ValidatingType = typeof(System.DateTime);
            this.mtxDurataCarica.Leave += new System.EventHandler(this.mtxDurataCarica_Leave);
            // 
            // mtxInizioEqual
            // 
            resources.ApplyResources(this.mtxInizioEqual, "mtxInizioEqual");
            this.mtxInizioEqual.Name = "mtxInizioEqual";
            this.mtxInizioEqual.ValidatingType = typeof(System.DateTime);
            this.mtxInizioEqual.Leave += new System.EventHandler(this.mtxInizioEqual_Leave);
            // 
            // chkEnableEqual
            // 
            resources.ApplyResources(this.chkEnableEqual, "chkEnableEqual");
            this.chkEnableEqual.Name = "chkEnableEqual";
            this.chkEnableEqual.UseVisualStyleBackColor = true;
            this.chkEnableEqual.CheckedChanged += new System.EventHandler(this.chkEnableEqual_CheckedChanged);
            // 
            // nudChargeFactor
            // 
            this.nudChargeFactor.DecimalPlaces = 2;
            resources.ApplyResources(this.nudChargeFactor, "nudChargeFactor");
            this.nudChargeFactor.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
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
            this.nudChargeFactor.Value = new decimal(new int[] {
            100,
            0,
            0,
            131072});
            this.nudChargeFactor.ValueChanged += new System.EventHandler(this.nudChargeFactor_ValueChanged);
            // 
            // lblDurata
            // 
            resources.ApplyResources(this.lblDurata, "lblDurata");
            this.lblDurata.Name = "lblDurata";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // lblOraEqual
            // 
            resources.ApplyResources(this.lblOraEqual, "lblOraEqual");
            this.lblOraEqual.Name = "lblOraEqual";
            // 
            // ctrlPannelloTempo
            // 
            resources.ApplyResources(this, "$this");
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
