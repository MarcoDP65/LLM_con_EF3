namespace PannelloCharger
{
    partial class frmMessaggioElettrolita
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAnnulla = new System.Windows.Forms.Button();
            this.lblIstruzioniControllo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pgbAvanzamento = new System.Windows.Forms.ProgressBar();
            this.lblMessaggioChiusura = new System.Windows.Forms.Label();
            this.btnRiavvia = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(173, 197);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(126, 36);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Ignora";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnAnnulla
            // 
            this.btnAnnulla.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnulla.Location = new System.Drawing.Point(316, 199);
            this.btnAnnulla.Name = "btnAnnulla";
            this.btnAnnulla.Size = new System.Drawing.Size(126, 34);
            this.btnAnnulla.TabIndex = 1;
            this.btnAnnulla.Text = "Annulla";
            this.btnAnnulla.UseVisualStyleBackColor = true;
            this.btnAnnulla.Click += new System.EventHandler(this.btnAnnulla_Click);
            // 
            // lblIstruzioniControllo
            // 
            this.lblIstruzioniControllo.AutoSize = true;
            this.lblIstruzioniControllo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIstruzioniControllo.Location = new System.Drawing.Point(45, 28);
            this.lblIstruzioniControllo.Name = "lblIstruzioniControllo";
            this.lblIstruzioniControllo.Size = new System.Drawing.Size(376, 18);
            this.lblIstruzioniControllo.TabIndex = 2;
            this.lblIstruzioniControllo.Text = "Verificare col puntale il collegamento della sonda";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(415, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Al momento del  contatto la finestra si chiuderà automaticamente";
            // 
            // pgbAvanzamento
            // 
            this.pgbAvanzamento.Location = new System.Drawing.Point(49, 99);
            this.pgbAvanzamento.Name = "pgbAvanzamento";
            this.pgbAvanzamento.Size = new System.Drawing.Size(372, 20);
            this.pgbAvanzamento.TabIndex = 4;
            // 
            // lblMessaggioChiusura
            // 
            this.lblMessaggioChiusura.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessaggioChiusura.Location = new System.Drawing.Point(46, 144);
            this.lblMessaggioChiusura.Name = "lblMessaggioChiusura";
            this.lblMessaggioChiusura.Size = new System.Drawing.Size(375, 32);
            this.lblMessaggioChiusura.TabIndex = 5;
            this.lblMessaggioChiusura.Text = "-";
            this.lblMessaggioChiusura.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMessaggioChiusura.Click += new System.EventHandler(this.lblMessaggioChiusura_Click);
            // 
            // btnRiavvia
            // 
            this.btnRiavvia.Location = new System.Drawing.Point(30, 197);
            this.btnRiavvia.Name = "btnRiavvia";
            this.btnRiavvia.Size = new System.Drawing.Size(126, 36);
            this.btnRiavvia.TabIndex = 6;
            this.btnRiavvia.Text = "Riavvia";
            this.btnRiavvia.UseVisualStyleBackColor = true;
            this.btnRiavvia.Click += new System.EventHandler(this.btnRiavvia_Click);
            // 
            // frmMessaggioElettrolita
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnAnnulla;
            this.ClientSize = new System.Drawing.Size(473, 259);
            this.ControlBox = false;
            this.Controls.Add(this.btnRiavvia);
            this.Controls.Add(this.lblMessaggioChiusura);
            this.Controls.Add(this.pgbAvanzamento);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblIstruzioniControllo);
            this.Controls.Add(this.btnAnnulla);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmMessaggioElettrolita";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Verifica Elettrolita";
            this.Load += new System.EventHandler(this.frmMessaggioElettrolita_Load);
            this.Shown += new System.EventHandler(this.frmMessaggioElettrolita_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnAnnulla;
        private System.Windows.Forms.Label lblIstruzioniControllo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar pgbAvanzamento;
        private System.Windows.Forms.Label lblMessaggioChiusura;
        private System.Windows.Forms.Button btnRiavvia;
    }
}