namespace PannelloCharger
{
    partial class frmSelettoreLadeLight
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
            this.txtIdScheda = new System.Windows.Forms.TextBox();
            this.btnImportaDati = new System.Windows.Forms.Button();
            this.btnEliminaDati = new System.Windows.Forms.Button();
            this.btnEsportaLadeLight = new System.Windows.Forms.Button();
            this.flvwListaApparati = new BrightIdeasSoftware.FastObjectListView();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.btnApriLadeLight = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaApparati)).BeginInit();
            this.SuspendLayout();
            // 
            // txtIdScheda
            // 
            this.txtIdScheda.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.txtIdScheda.Location = new System.Drawing.Point(436, 376);
            this.txtIdScheda.Margin = new System.Windows.Forms.Padding(2);
            this.txtIdScheda.Name = "txtIdScheda";
            this.txtIdScheda.ReadOnly = true;
            this.txtIdScheda.Size = new System.Drawing.Size(196, 23);
            this.txtIdScheda.TabIndex = 14;
            // 
            // btnImportaDati
            // 
            this.btnImportaDati.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnImportaDati.Location = new System.Drawing.Point(309, 370);
            this.btnImportaDati.Margin = new System.Windows.Forms.Padding(2);
            this.btnImportaDati.Name = "btnImportaDati";
            this.btnImportaDati.Size = new System.Drawing.Size(94, 36);
            this.btnImportaDati.TabIndex = 13;
            this.btnImportaDati.Text = "Importa Dati";
            this.btnImportaDati.UseVisualStyleBackColor = true;
            this.btnImportaDati.Click += new System.EventHandler(this.btnImportaDati_Click);
            // 
            // btnEliminaDati
            // 
            this.btnEliminaDati.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEliminaDati.Location = new System.Drawing.Point(113, 370);
            this.btnEliminaDati.Margin = new System.Windows.Forms.Padding(2);
            this.btnEliminaDati.Name = "btnEliminaDati";
            this.btnEliminaDati.Size = new System.Drawing.Size(94, 36);
            this.btnEliminaDati.TabIndex = 12;
            this.btnEliminaDati.Text = "Cancella Dati";
            this.btnEliminaDati.UseVisualStyleBackColor = true;
            this.btnEliminaDati.Click += new System.EventHandler(this.btnEliminaDati_Click);
            // 
            // btnEsportaLadeLight
            // 
            this.btnEsportaLadeLight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEsportaLadeLight.Location = new System.Drawing.Point(211, 370);
            this.btnEsportaLadeLight.Margin = new System.Windows.Forms.Padding(2);
            this.btnEsportaLadeLight.Name = "btnEsportaLadeLight";
            this.btnEsportaLadeLight.Size = new System.Drawing.Size(94, 36);
            this.btnEsportaLadeLight.TabIndex = 11;
            this.btnEsportaLadeLight.Text = "Esporta Dati";
            this.btnEsportaLadeLight.UseVisualStyleBackColor = true;
            this.btnEsportaLadeLight.Click += new System.EventHandler(this.btnEsportaLadeLight_Click);
            // 
            // flvwListaApparati
            // 
            this.flvwListaApparati.CellEditUseWholeCell = false;
            this.flvwListaApparati.HideSelection = false;
            this.flvwListaApparati.Location = new System.Drawing.Point(14, 11);
            this.flvwListaApparati.Margin = new System.Windows.Forms.Padding(2);
            this.flvwListaApparati.Name = "flvwListaApparati";
            this.flvwListaApparati.ShowGroups = false;
            this.flvwListaApparati.Size = new System.Drawing.Size(732, 344);
            this.flvwListaApparati.TabIndex = 10;
            this.flvwListaApparati.UseCompatibleStateImageBehavior = false;
            this.flvwListaApparati.View = System.Windows.Forms.View.Details;
            this.flvwListaApparati.VirtualMode = true;
            this.flvwListaApparati.SelectedIndexChanged += new System.EventHandler(this.flvwListaApparati_SelectedIndexChanged);
            this.flvwListaApparati.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.flvwListaApparati_MouseDoubleClick);
            // 
            // btnChiudi
            // 
            this.btnChiudi.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChiudi.Location = new System.Drawing.Point(652, 370);
            this.btnChiudi.Margin = new System.Windows.Forms.Padding(2);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(94, 36);
            this.btnChiudi.TabIndex = 9;
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            // 
            // btnApriLadeLight
            // 
            this.btnApriLadeLight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnApriLadeLight.Location = new System.Drawing.Point(14, 370);
            this.btnApriLadeLight.Margin = new System.Windows.Forms.Padding(2);
            this.btnApriLadeLight.Name = "btnApriLadeLight";
            this.btnApriLadeLight.Size = new System.Drawing.Size(94, 36);
            this.btnApriLadeLight.TabIndex = 8;
            this.btnApriLadeLight.Text = "Apri Scheda";
            this.btnApriLadeLight.UseVisualStyleBackColor = true;
            this.btnApriLadeLight.Click += new System.EventHandler(this.btnApriLadeLight_Click);
            // 
            // frmSelettoreLadeLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 415);
            this.Controls.Add(this.txtIdScheda);
            this.Controls.Add(this.btnImportaDati);
            this.Controls.Add(this.btnEliminaDati);
            this.Controls.Add(this.btnEsportaLadeLight);
            this.Controls.Add(this.flvwListaApparati);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.btnApriLadeLight);
            this.Name = "frmSelettoreLadeLight";
            this.Text = "Archivio LADE Light";
            this.Resize += new System.EventHandler(this.frmSelettoreLadeLight_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaApparati)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIdScheda;
        private System.Windows.Forms.Button btnImportaDati;
        private System.Windows.Forms.Button btnEliminaDati;
        private System.Windows.Forms.Button btnEsportaLadeLight;
        private BrightIdeasSoftware.FastObjectListView flvwListaApparati;
        private System.Windows.Forms.Button btnChiudi;
        private System.Windows.Forms.Button btnApriLadeLight;
    }
}