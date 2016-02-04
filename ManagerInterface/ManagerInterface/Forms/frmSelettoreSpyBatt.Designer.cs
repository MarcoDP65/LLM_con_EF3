namespace PannelloCharger
{
    partial class frmSelettoreSpyBatt
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
            this.btnApriSpybatt = new System.Windows.Forms.Button();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.flvwListaApparati = new BrightIdeasSoftware.FastObjectListView();
            this.btnEsportaSpybatt = new System.Windows.Forms.Button();
            this.btnEliminaDati = new System.Windows.Forms.Button();
            this.btnImportaDati = new System.Windows.Forms.Button();
            this.txtIdScheda = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaApparati)).BeginInit();
            this.SuspendLayout();
            // 
            // btnApriSpybatt
            // 
            this.btnApriSpybatt.Location = new System.Drawing.Point(22, 454);
            this.btnApriSpybatt.Name = "btnApriSpybatt";
            this.btnApriSpybatt.Size = new System.Drawing.Size(125, 44);
            this.btnApriSpybatt.TabIndex = 1;
            this.btnApriSpybatt.Text = "Apri Scheda";
            this.btnApriSpybatt.UseVisualStyleBackColor = true;
            this.btnApriSpybatt.Click += new System.EventHandler(this.btnApriSpybatt_Click);
            // 
            // btnChiudi
            // 
            this.btnChiudi.Location = new System.Drawing.Point(872, 454);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(125, 44);
            this.btnChiudi.TabIndex = 2;
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // flvwListaApparati
            // 
            this.flvwListaApparati.Location = new System.Drawing.Point(22, 12);
            this.flvwListaApparati.Name = "flvwListaApparati";
            this.flvwListaApparati.ShowGroups = false;
            this.flvwListaApparati.Size = new System.Drawing.Size(975, 423);
            this.flvwListaApparati.TabIndex = 3;
            this.flvwListaApparati.UseCompatibleStateImageBehavior = false;
            this.flvwListaApparati.View = System.Windows.Forms.View.Details;
            this.flvwListaApparati.VirtualMode = true;
            this.flvwListaApparati.SelectedIndexChanged += new System.EventHandler(this.flvwListaApparati_SelectedIndexChanged);
            this.flvwListaApparati.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.flvwListaApparati_MouseDoubleClick);
            // 
            // btnEsportaSpybatt
            // 
            this.btnEsportaSpybatt.Location = new System.Drawing.Point(284, 454);
            this.btnEsportaSpybatt.Name = "btnEsportaSpybatt";
            this.btnEsportaSpybatt.Size = new System.Drawing.Size(125, 44);
            this.btnEsportaSpybatt.TabIndex = 4;
            this.btnEsportaSpybatt.Text = "Esporta Dati";
            this.btnEsportaSpybatt.UseVisualStyleBackColor = true;
            this.btnEsportaSpybatt.Click += new System.EventHandler(this.btnEsportaSpybatt_Click);
            // 
            // btnEliminaDati
            // 
            this.btnEliminaDati.Location = new System.Drawing.Point(153, 454);
            this.btnEliminaDati.Name = "btnEliminaDati";
            this.btnEliminaDati.Size = new System.Drawing.Size(125, 44);
            this.btnEliminaDati.TabIndex = 5;
            this.btnEliminaDati.Text = "Cancella Dati";
            this.btnEliminaDati.UseVisualStyleBackColor = true;
            this.btnEliminaDati.Click += new System.EventHandler(this.btnEliminaDati_Click);
            // 
            // btnImportaDati
            // 
            this.btnImportaDati.Location = new System.Drawing.Point(415, 454);
            this.btnImportaDati.Name = "btnImportaDati";
            this.btnImportaDati.Size = new System.Drawing.Size(125, 44);
            this.btnImportaDati.TabIndex = 6;
            this.btnImportaDati.Text = "Importa Dati";
            this.btnImportaDati.UseVisualStyleBackColor = true;
            this.btnImportaDati.Click += new System.EventHandler(this.btnImportaDati_Click);
            // 
            // txtIdScheda
            // 
            this.txtIdScheda.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIdScheda.Location = new System.Drawing.Point(556, 462);
            this.txtIdScheda.Name = "txtIdScheda";
            this.txtIdScheda.ReadOnly = true;
            this.txtIdScheda.Size = new System.Drawing.Size(260, 27);
            this.txtIdScheda.TabIndex = 7;
            // 
            // frmSelettoreSpyBatt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 514);
            this.Controls.Add(this.txtIdScheda);
            this.Controls.Add(this.btnImportaDati);
            this.Controls.Add(this.btnEliminaDati);
            this.Controls.Add(this.btnEsportaSpybatt);
            this.Controls.Add(this.flvwListaApparati);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.btnApriSpybatt);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelettoreSpyBatt";
            this.ShowIcon = false;
            this.Text = "Archivio Spy-Batt";
            this.Load += new System.EventHandler(this.frmSelettoreSpyBatt_Load);
            this.Shown += new System.EventHandler(this.frmSelettoreSpyBatt_Shown);
            this.Resize += new System.EventHandler(this.frmSelettoreSpyBatt_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaApparati)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnApriSpybatt;
        private System.Windows.Forms.Button btnChiudi;
        private BrightIdeasSoftware.FastObjectListView flvwListaApparati;
        private System.Windows.Forms.Button btnEsportaSpybatt;
        private System.Windows.Forms.Button btnEliminaDati;
        private System.Windows.Forms.Button btnImportaDati;
        private System.Windows.Forms.TextBox txtIdScheda;
    }
}