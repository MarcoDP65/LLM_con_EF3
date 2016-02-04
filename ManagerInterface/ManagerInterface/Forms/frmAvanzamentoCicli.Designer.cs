namespace PannelloCharger
{
    partial class frmAvanzamentoCicli
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAvanzamentoCicli));
            this.lblAvanzmentoL = new System.Windows.Forms.Label();
            this.pgbAvanamentoL = new System.Windows.Forms.ProgressBar();
            this.btnStartLettura = new System.Windows.Forms.Button();
            this.pgbAvanamentoB = new System.Windows.Forms.ProgressBar();
            this.lblAvanzmentoB = new System.Windows.Forms.Label();
            this.btnAnnulla = new System.Windows.Forms.Button();
            this.lblTitolo = new System.Windows.Forms.Label();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.lblMessaggioAvanzamento = new System.Windows.Forms.Label();
            this.lblMsg01 = new System.Windows.Forms.Label();
            this.lblMsg02 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblAvanzmentoL
            // 
            this.lblAvanzmentoL.AutoSize = true;
            this.lblAvanzmentoL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvanzmentoL.Location = new System.Drawing.Point(22, 82);
            this.lblAvanzmentoL.Name = "lblAvanzmentoL";
            this.lblAvanzmentoL.Size = new System.Drawing.Size(55, 20);
            this.lblAvanzmentoL.TabIndex = 9;
            this.lblAvanzmentoL.Text = "100%";
            this.lblAvanzmentoL.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblAvanzmentoL.Click += new System.EventHandler(this.lblAvanzmento_Click);
            // 
            // pgbAvanamentoL
            // 
            this.pgbAvanamentoL.Location = new System.Drawing.Point(83, 82);
            this.pgbAvanamentoL.Name = "pgbAvanamentoL";
            this.pgbAvanamentoL.Size = new System.Drawing.Size(764, 20);
            this.pgbAvanamentoL.TabIndex = 8;
            // 
            // btnStartLettura
            // 
            this.btnStartLettura.Location = new System.Drawing.Point(198, 239);
            this.btnStartLettura.Name = "btnStartLettura";
            this.btnStartLettura.Size = new System.Drawing.Size(147, 35);
            this.btnStartLettura.TabIndex = 10;
            this.btnStartLettura.Text = "Carica Cicli ";
            this.btnStartLettura.UseVisualStyleBackColor = true;
            this.btnStartLettura.Click += new System.EventHandler(this.btnStartLettura_Click);
            // 
            // pgbAvanamentoB
            // 
            this.pgbAvanamentoB.Location = new System.Drawing.Point(83, 124);
            this.pgbAvanamentoB.Name = "pgbAvanamentoB";
            this.pgbAvanamentoB.Size = new System.Drawing.Size(764, 20);
            this.pgbAvanamentoB.TabIndex = 11;
            this.pgbAvanamentoB.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // lblAvanzmentoB
            // 
            this.lblAvanzmentoB.AutoSize = true;
            this.lblAvanzmentoB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvanzmentoB.Location = new System.Drawing.Point(22, 124);
            this.lblAvanzmentoB.Name = "lblAvanzmentoB";
            this.lblAvanzmentoB.Size = new System.Drawing.Size(55, 20);
            this.lblAvanzmentoB.TabIndex = 12;
            this.lblAvanzmentoB.Text = "100%";
            this.lblAvanzmentoB.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnAnnulla
            // 
            this.btnAnnulla.Location = new System.Drawing.Point(375, 239);
            this.btnAnnulla.Name = "btnAnnulla";
            this.btnAnnulla.Size = new System.Drawing.Size(147, 35);
            this.btnAnnulla.TabIndex = 13;
            this.btnAnnulla.Text = "Interrompi";
            this.btnAnnulla.UseVisualStyleBackColor = true;
            this.btnAnnulla.Click += new System.EventHandler(this.btnAnnulla_Click);
            // 
            // lblTitolo
            // 
            this.lblTitolo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitolo.Location = new System.Drawing.Point(26, 34);
            this.lblTitolo.Name = "lblTitolo";
            this.lblTitolo.Size = new System.Drawing.Size(821, 25);
            this.lblTitolo.TabIndex = 14;
            this.lblTitolo.Text = "Avanzamento lettura cicli";
            // 
            // btnChiudi
            // 
            this.btnChiudi.Location = new System.Drawing.Point(546, 239);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(147, 35);
            this.btnChiudi.TabIndex = 15;
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // lblMessaggioAvanzamento
            // 
            this.lblMessaggioAvanzamento.Location = new System.Drawing.Point(83, 160);
            this.lblMessaggioAvanzamento.Name = "lblMessaggioAvanzamento";
            this.lblMessaggioAvanzamento.Size = new System.Drawing.Size(764, 28);
            this.lblMessaggioAvanzamento.TabIndex = 16;
            this.lblMessaggioAvanzamento.Text = ".";
            this.lblMessaggioAvanzamento.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMsg01
            // 
            this.lblMsg01.Location = new System.Drawing.Point(80, 199);
            this.lblMsg01.Name = "lblMsg01";
            this.lblMsg01.Size = new System.Drawing.Size(365, 28);
            this.lblMsg01.TabIndex = 17;
            this.lblMsg01.Text = ".";
            this.lblMsg01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMsg02
            // 
            this.lblMsg02.Location = new System.Drawing.Point(482, 199);
            this.lblMsg02.Name = "lblMsg02";
            this.lblMsg02.Size = new System.Drawing.Size(365, 28);
            this.lblMsg02.TabIndex = 18;
            this.lblMsg02.Text = ".";
            this.lblMsg02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmAvanzamentoCicli
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 321);
            this.Controls.Add(this.lblMsg02);
            this.Controls.Add(this.lblMsg01);
            this.Controls.Add(this.lblMessaggioAvanzamento);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.lblTitolo);
            this.Controls.Add(this.btnAnnulla);
            this.Controls.Add(this.lblAvanzmentoB);
            this.Controls.Add(this.pgbAvanamentoB);
            this.Controls.Add(this.btnStartLettura);
            this.Controls.Add(this.lblAvanzmentoL);
            this.Controls.Add(this.pgbAvanamentoL);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAvanzamentoCicli";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Acquisizione Dati";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAvanzamentoCicli_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmAvanzamentoCicli_FormClosed);
            this.Load += new System.EventHandler(this.frmAvanzamentoCicli_Load);
            this.Shown += new System.EventHandler(this.frmAvanzamentoCicli_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAvanzmentoL;
        private System.Windows.Forms.ProgressBar pgbAvanamentoL;
        private System.Windows.Forms.Button btnStartLettura;
        private System.Windows.Forms.ProgressBar pgbAvanamentoB;
        private System.Windows.Forms.Label lblAvanzmentoB;
        private System.Windows.Forms.Button btnAnnulla;
        private System.Windows.Forms.Label lblTitolo;
        private System.Windows.Forms.Button btnChiudi;
        private System.Windows.Forms.Label lblMessaggioAvanzamento;
        private System.Windows.Forms.Label lblMsg01;
        private System.Windows.Forms.Label lblMsg02;
    }
}