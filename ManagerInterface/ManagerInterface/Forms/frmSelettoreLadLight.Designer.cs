namespace PannelloCharger.Forms
{
    partial class frmSelettoreLadLight
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
            this.btnEsportaSpybatt = new System.Windows.Forms.Button();
            this.flvwListaApparati = new BrightIdeasSoftware.FastObjectListView();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.btnApriSpybatt = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaApparati)).BeginInit();
            this.SuspendLayout();
            // 
            // txtIdScheda
            // 
            this.txtIdScheda.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold);
            this.txtIdScheda.Location = new System.Drawing.Point(456, 393);
            this.txtIdScheda.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtIdScheda.Name = "txtIdScheda";
            this.txtIdScheda.ReadOnly = true;
            this.txtIdScheda.Size = new System.Drawing.Size(196, 23);
            this.txtIdScheda.TabIndex = 14;
            // 
            // btnImportaDati
            // 
            this.btnImportaDati.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnImportaDati.Location = new System.Drawing.Point(329, 387);
            this.btnImportaDati.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnImportaDati.Name = "btnImportaDati";
            this.btnImportaDati.Size = new System.Drawing.Size(94, 36);
            this.btnImportaDati.TabIndex = 13;
            this.btnImportaDati.Text = "Importa Dati";
            this.btnImportaDati.UseVisualStyleBackColor = true;
            // 
            // btnEliminaDati
            // 
            this.btnEliminaDati.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEliminaDati.Location = new System.Drawing.Point(133, 387);
            this.btnEliminaDati.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEliminaDati.Name = "btnEliminaDati";
            this.btnEliminaDati.Size = new System.Drawing.Size(94, 36);
            this.btnEliminaDati.TabIndex = 12;
            this.btnEliminaDati.Text = "Cancella Dati";
            this.btnEliminaDati.UseVisualStyleBackColor = true;
            // 
            // btnEsportaSpybatt
            // 
            this.btnEsportaSpybatt.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnEsportaSpybatt.Location = new System.Drawing.Point(231, 387);
            this.btnEsportaSpybatt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEsportaSpybatt.Name = "btnEsportaSpybatt";
            this.btnEsportaSpybatt.Size = new System.Drawing.Size(94, 36);
            this.btnEsportaSpybatt.TabIndex = 11;
            this.btnEsportaSpybatt.Text = "Esporta Dati";
            this.btnEsportaSpybatt.UseVisualStyleBackColor = true;
            // 
            // flvwListaApparati
            // 
            this.flvwListaApparati.CellEditUseWholeCell = false;
            this.flvwListaApparati.HideSelection = false;
            this.flvwListaApparati.Location = new System.Drawing.Point(34, 28);
            this.flvwListaApparati.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flvwListaApparati.Name = "flvwListaApparati";
            this.flvwListaApparati.ShowGroups = false;
            this.flvwListaApparati.Size = new System.Drawing.Size(732, 344);
            this.flvwListaApparati.TabIndex = 10;
            this.flvwListaApparati.UseCompatibleStateImageBehavior = false;
            this.flvwListaApparati.View = System.Windows.Forms.View.Details;
            this.flvwListaApparati.VirtualMode = true;
            // 
            // btnChiudi
            // 
            this.btnChiudi.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChiudi.Location = new System.Drawing.Point(672, 387);
            this.btnChiudi.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(94, 36);
            this.btnChiudi.TabIndex = 9;
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            // 
            // btnApriSpybatt
            // 
            this.btnApriSpybatt.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnApriSpybatt.Location = new System.Drawing.Point(34, 387);
            this.btnApriSpybatt.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnApriSpybatt.Name = "btnApriSpybatt";
            this.btnApriSpybatt.Size = new System.Drawing.Size(94, 36);
            this.btnApriSpybatt.TabIndex = 8;
            this.btnApriSpybatt.Text = "Apri Scheda";
            this.btnApriSpybatt.UseVisualStyleBackColor = true;
            // 
            // frmSelettoreLadLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtIdScheda);
            this.Controls.Add(this.btnImportaDati);
            this.Controls.Add(this.btnEliminaDati);
            this.Controls.Add(this.btnEsportaSpybatt);
            this.Controls.Add(this.flvwListaApparati);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.btnApriSpybatt);
            this.Name = "frmSelettoreLadLight";
            this.Text = "Archivio LADE Light";
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaApparati)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIdScheda;
        private System.Windows.Forms.Button btnImportaDati;
        private System.Windows.Forms.Button btnEliminaDati;
        private System.Windows.Forms.Button btnEsportaSpybatt;
        private BrightIdeasSoftware.FastObjectListView flvwListaApparati;
        private System.Windows.Forms.Button btnChiudi;
        private System.Windows.Forms.Button btnApriSpybatt;
    }
}