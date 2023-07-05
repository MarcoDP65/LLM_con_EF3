namespace PannelloCharger.Forms
{
    partial class frmQRCode
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.picQRCode = new System.Windows.Forms.PictureBox();
            this.lblDataArray = new System.Windows.Forms.Label();
            this.lblNomeProfilo = new System.Windows.Forms.Label();
            this.lblProfilo = new System.Windows.Forms.Label();
            this.lblTensione = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCorrente = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblBatteria = new System.Windows.Forms.Label();
            this.lblDescrizione = new System.Windows.Forms.Label();
            this.btnSalvaFilePDF = new System.Windows.Forms.Button();
            this.btnNomeFilePDFSRC = new System.Windows.Forms.Button();
            this.txtNomeFilePDF = new System.Windows.Forms.TextBox();
            this.sfdExportDati = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picQRCode)).BeginInit();
            this.SuspendLayout();
            // 
            // picQRCode
            // 
            this.picQRCode.Location = new System.Drawing.Point(352, 24);
            this.picQRCode.Name = "picQRCode";
            this.picQRCode.Size = new System.Drawing.Size(200, 200);
            this.picQRCode.TabIndex = 0;
            this.picQRCode.TabStop = false;
            // 
            // lblDataArray
            // 
            this.lblDataArray.BackColor = System.Drawing.Color.White;
            this.lblDataArray.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataArray.Location = new System.Drawing.Point(17, 245);
            this.lblDataArray.Name = "lblDataArray";
            this.lblDataArray.Size = new System.Drawing.Size(541, 98);
            this.lblDataArray.TabIndex = 1;
            // 
            // lblNomeProfilo
            // 
            this.lblNomeProfilo.AutoSize = true;
            this.lblNomeProfilo.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomeProfilo.Location = new System.Drawing.Point(26, 24);
            this.lblNomeProfilo.Name = "lblNomeProfilo";
            this.lblNomeProfilo.Size = new System.Drawing.Size(206, 37);
            this.lblNomeProfilo.TabIndex = 2;
            this.lblNomeProfilo.Text = "nome profilo";
            // 
            // lblProfilo
            // 
            this.lblProfilo.AutoSize = true;
            this.lblProfilo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfilo.Location = new System.Drawing.Point(29, 104);
            this.lblProfilo.Name = "lblProfilo";
            this.lblProfilo.Size = new System.Drawing.Size(68, 24);
            this.lblProfilo.TabIndex = 3;
            this.lblProfilo.Text = "profilo";
            // 
            // lblTensione
            // 
            this.lblTensione.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTensione.Location = new System.Drawing.Point(51, 188);
            this.lblTensione.Name = "lblTensione";
            this.lblTensione.Size = new System.Drawing.Size(80, 29);
            this.lblTensione.TabIndex = 4;
            this.lblTensione.Text = "80";
            this.lblTensione.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(123, 195);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 20);
            this.label5.TabIndex = 5;
            this.label5.Text = "V";
            // 
            // lblCorrente
            // 
            this.lblCorrente.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorrente.Location = new System.Drawing.Point(150, 188);
            this.lblCorrente.Name = "lblCorrente";
            this.lblCorrente.Size = new System.Drawing.Size(82, 24);
            this.lblCorrente.TabIndex = 6;
            this.lblCorrente.Text = "840";
            this.lblCorrente.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(227, 195);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 20);
            this.label7.TabIndex = 7;
            this.label7.Text = "Ah";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(402, 419);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(150, 35);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblBatteria
            // 
            this.lblBatteria.AutoSize = true;
            this.lblBatteria.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBatteria.Location = new System.Drawing.Point(29, 140);
            this.lblBatteria.Name = "lblBatteria";
            this.lblBatteria.Size = new System.Drawing.Size(78, 24);
            this.lblBatteria.TabIndex = 9;
            this.lblBatteria.Text = "batteria";
            // 
            // lblDescrizione
            // 
            this.lblDescrizione.AutoSize = true;
            this.lblDescrizione.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescrizione.Location = new System.Drawing.Point(29, 69);
            this.lblDescrizione.Name = "lblDescrizione";
            this.lblDescrizione.Size = new System.Drawing.Size(89, 20);
            this.lblDescrizione.TabIndex = 10;
            this.lblDescrizione.Text = "descrizione";
            // 
            // btnSalvaFilePDF
            // 
            this.btnSalvaFilePDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSalvaFilePDF.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSalvaFilePDF.Location = new System.Drawing.Point(402, 379);
            this.btnSalvaFilePDF.Margin = new System.Windows.Forms.Padding(2);
            this.btnSalvaFilePDF.Name = "btnSalvaFilePDF";
            this.btnSalvaFilePDF.Size = new System.Drawing.Size(150, 35);
            this.btnSalvaFilePDF.TabIndex = 71;
            this.btnSalvaFilePDF.Text = "Salva File PDF";
            this.btnSalvaFilePDF.UseVisualStyleBackColor = true;
            this.btnSalvaFilePDF.Click += new System.EventHandler(this.btnSalvaFilePDF_Click);
            // 
            // btnNomeFilePDFSRC
            // 
            this.btnNomeFilePDFSRC.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNomeFilePDFSRC.Location = new System.Drawing.Point(347, 384);
            this.btnNomeFilePDFSRC.Margin = new System.Windows.Forms.Padding(2);
            this.btnNomeFilePDFSRC.Name = "btnNomeFilePDFSRC";
            this.btnNomeFilePDFSRC.Size = new System.Drawing.Size(25, 24);
            this.btnNomeFilePDFSRC.TabIndex = 70;
            this.btnNomeFilePDFSRC.Text = "...";
            this.btnNomeFilePDFSRC.UseVisualStyleBackColor = true;
            this.btnNomeFilePDFSRC.Click += new System.EventHandler(this.btnNomeFilePDFSRC_Click);
            // 
            // txtNomeFilePDF
            // 
            this.txtNomeFilePDF.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtNomeFilePDF.Location = new System.Drawing.Point(20, 385);
            this.txtNomeFilePDF.Margin = new System.Windows.Forms.Padding(2);
            this.txtNomeFilePDF.Name = "txtNomeFilePDF";
            this.txtNomeFilePDF.Size = new System.Drawing.Size(323, 21);
            this.txtNomeFilePDF.TabIndex = 69;
            // 
            // frmQRCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 467);
            this.Controls.Add(this.btnSalvaFilePDF);
            this.Controls.Add(this.btnNomeFilePDFSRC);
            this.Controls.Add(this.txtNomeFilePDF);
            this.Controls.Add(this.lblDescrizione);
            this.Controls.Add(this.lblBatteria);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblCorrente);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblTensione);
            this.Controls.Add(this.lblProfilo);
            this.Controls.Add(this.lblNomeProfilo);
            this.Controls.Add(this.lblDataArray);
            this.Controls.Add(this.picQRCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmQRCode";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "QR Code";
            this.Load += new System.EventHandler(this.frmQRCode_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picQRCode)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picQRCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnOK;
        public System.Windows.Forms.Label lblDataArray;
        public System.Windows.Forms.Label lblNomeProfilo;
        public System.Windows.Forms.Label lblProfilo;
        public System.Windows.Forms.Label lblBatteria;
        public System.Windows.Forms.Label lblTensione;
        public System.Windows.Forms.Label lblCorrente;
        public System.Windows.Forms.Label lblDescrizione;
        private System.Windows.Forms.Button btnSalvaFilePDF;
        private System.Windows.Forms.Button btnNomeFilePDFSRC;
        private System.Windows.Forms.TextBox txtNomeFilePDF;
        private System.Windows.Forms.SaveFileDialog sfdExportDati;
    }
}
