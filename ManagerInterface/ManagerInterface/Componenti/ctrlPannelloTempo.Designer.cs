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
            this.chkEnableEqual = new System.Windows.Forms.CheckBox();
            this.nudChargeFactor = new System.Windows.Forms.NumericUpDown();
            this.lblDurata = new System.Windows.Forms.Label();
            this.lblChargeFactor = new System.Windows.Forms.Label();
            this.chkStartDelayed = new System.Windows.Forms.CheckBox();
            this.lblInizioCarica = new System.Windows.Forms.Label();
            this.mtxInizioCarica = new System.Windows.Forms.MaskedTextBox();
            this.lblAttesaMassima = new System.Windows.Forms.Label();
            this.mtxAttesaMassima = new System.Windows.Forms.MaskedTextBox();
            this.chkEnableDeleteDelay = new System.Windows.Forms.CheckBox();
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
            // lblChargeFactor
            // 
            resources.ApplyResources(this.lblChargeFactor, "lblChargeFactor");
            this.lblChargeFactor.Name = "lblChargeFactor";
            // 
            // chkStartDelayed
            // 
            resources.ApplyResources(this.chkStartDelayed, "chkStartDelayed");
            this.chkStartDelayed.Name = "chkStartDelayed";
            this.chkStartDelayed.UseVisualStyleBackColor = true;
            this.chkStartDelayed.CheckedChanged += new System.EventHandler(this.chkStartDelayed_CheckedChanged);
            // 
            // lblInizioCarica
            // 
            resources.ApplyResources(this.lblInizioCarica, "lblInizioCarica");
            this.lblInizioCarica.Name = "lblInizioCarica";
            // 
            // mtxInizioCarica
            // 
            resources.ApplyResources(this.mtxInizioCarica, "mtxInizioCarica");
            this.mtxInizioCarica.Name = "mtxInizioCarica";
            this.mtxInizioCarica.ValidatingType = typeof(System.DateTime);
            this.mtxInizioCarica.Leave += new System.EventHandler(this.mtxInizioCarica_Leave);
            // 
            // lblAttesaMassima
            // 
            resources.ApplyResources(this.lblAttesaMassima, "lblAttesaMassima");
            this.lblAttesaMassima.Name = "lblAttesaMassima";
            // 
            // mtxAttesaMassima
            // 
            resources.ApplyResources(this.mtxAttesaMassima, "mtxAttesaMassima");
            this.mtxAttesaMassima.Name = "mtxAttesaMassima";
            this.mtxAttesaMassima.ValidatingType = typeof(System.DateTime);
            this.mtxAttesaMassima.Leave += new System.EventHandler(this.mtxAttesaMassima_Leave);
            // 
            // chkEnableDeleteDelay
            // 
            resources.ApplyResources(this.chkEnableDeleteDelay, "chkEnableDeleteDelay");
            this.chkEnableDeleteDelay.Name = "chkEnableDeleteDelay";
            this.chkEnableDeleteDelay.UseVisualStyleBackColor = true;
            this.chkEnableDeleteDelay.CheckedChanged += new System.EventHandler(this.chkEnableDeleteDelay_CheckedChanged);
            // 
            // ctrlPannelloTempo
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.chkEnableDeleteDelay);
            this.Controls.Add(this.lblAttesaMassima);
            this.Controls.Add(this.mtxAttesaMassima);
            this.Controls.Add(this.lblInizioCarica);
            this.Controls.Add(this.mtxInizioCarica);
            this.Controls.Add(this.chkStartDelayed);
            this.Controls.Add(this.lblChargeFactor);
            this.Controls.Add(this.lblDurata);
            this.Controls.Add(this.nudChargeFactor);
            this.Controls.Add(this.chkEnableEqual);
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
        private System.Windows.Forms.CheckBox chkEnableEqual;
        private System.Windows.Forms.NumericUpDown nudChargeFactor;
        private System.Windows.Forms.Label lblDurata;
        private System.Windows.Forms.Label lblChargeFactor;
        private System.Windows.Forms.CheckBox chkStartDelayed;
        private System.Windows.Forms.Label lblInizioCarica;
        private System.Windows.Forms.MaskedTextBox mtxInizioCarica;
        private System.Windows.Forms.Label lblAttesaMassima;
        private System.Windows.Forms.MaskedTextBox mtxAttesaMassima;
        private System.Windows.Forms.CheckBox chkEnableDeleteDelay;
    }
}
