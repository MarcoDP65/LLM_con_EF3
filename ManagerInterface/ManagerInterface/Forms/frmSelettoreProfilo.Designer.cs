namespace PannelloCharger.Forms
{
    partial class frmSelettoreProfilo
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
            this.flwPaListaConfigurazioni = new BrightIdeasSoftware.FastObjectListView();
            this.btnSelezionaConfigurazione = new System.Windows.Forms.Button();
            this.btnAnnullaSelezione = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.flwPaListaConfigurazioni)).BeginInit();
            this.SuspendLayout();
            // 
            // flwPaListaConfigurazioni
            // 
            this.flwPaListaConfigurazioni.CellEditUseWholeCell = false;
            this.flwPaListaConfigurazioni.HideSelection = false;
            this.flwPaListaConfigurazioni.Location = new System.Drawing.Point(11, 11);
            this.flwPaListaConfigurazioni.Margin = new System.Windows.Forms.Padding(2);
            this.flwPaListaConfigurazioni.Name = "flwPaListaConfigurazioni";
            this.flwPaListaConfigurazioni.ShowGroups = false;
            this.flwPaListaConfigurazioni.Size = new System.Drawing.Size(1035, 402);
            this.flwPaListaConfigurazioni.TabIndex = 64;
            this.flwPaListaConfigurazioni.UseCompatibleStateImageBehavior = false;
            this.flwPaListaConfigurazioni.View = System.Windows.Forms.View.Details;
            this.flwPaListaConfigurazioni.VirtualMode = true;
            this.flwPaListaConfigurazioni.SelectedIndexChanged += new System.EventHandler(this.flwPaListaConfigurazioni_SelectedIndexChanged);
            // 
            // btnSelezionaConfigurazione
            // 
            this.btnSelezionaConfigurazione.Enabled = false;
            this.btnSelezionaConfigurazione.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSelezionaConfigurazione.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelezionaConfigurazione.Location = new System.Drawing.Point(826, 440);
            this.btnSelezionaConfigurazione.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelezionaConfigurazione.Name = "btnSelezionaConfigurazione";
            this.btnSelezionaConfigurazione.Size = new System.Drawing.Size(220, 35);
            this.btnSelezionaConfigurazione.TabIndex = 66;
            this.btnSelezionaConfigurazione.Text = "Carica Configurazione";
            this.btnSelezionaConfigurazione.UseVisualStyleBackColor = true;
            this.btnSelezionaConfigurazione.Click += new System.EventHandler(this.btnSelezionaConfigurazione_Click);
            // 
            // btnAnnullaSelezione
            // 
            this.btnAnnullaSelezione.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnAnnullaSelezione.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAnnullaSelezione.Location = new System.Drawing.Point(602, 440);
            this.btnAnnullaSelezione.Margin = new System.Windows.Forms.Padding(2);
            this.btnAnnullaSelezione.Name = "btnAnnullaSelezione";
            this.btnAnnullaSelezione.Size = new System.Drawing.Size(220, 35);
            this.btnAnnullaSelezione.TabIndex = 67;
            this.btnAnnullaSelezione.Text = "Annulla Selezione";
            this.btnAnnullaSelezione.UseVisualStyleBackColor = true;
            this.btnAnnullaSelezione.Click += new System.EventHandler(this.btnAnnullaSelezione_Click);
            // 
            // frmSelettoreProfilo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 497);
            this.Controls.Add(this.btnAnnullaSelezione);
            this.Controls.Add(this.btnSelezionaConfigurazione);
            this.Controls.Add(this.flwPaListaConfigurazioni);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmSelettoreProfilo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Selezione Profilo";
            ((System.ComponentModel.ISupportInitialize)(this.flwPaListaConfigurazioni)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.FastObjectListView flwPaListaConfigurazioni;
        private System.Windows.Forms.Button btnSelezionaConfigurazione;
        private System.Windows.Forms.Button btnAnnullaSelezione;
    }
}