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
            this.lblMsgFail = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblAvanzmentoL
            // 
            resources.ApplyResources(this.lblAvanzmentoL, "lblAvanzmentoL");
            this.lblAvanzmentoL.Name = "lblAvanzmentoL";
            this.lblAvanzmentoL.Click += new System.EventHandler(this.lblAvanzmento_Click);
            // 
            // pgbAvanamentoL
            // 
            resources.ApplyResources(this.pgbAvanamentoL, "pgbAvanamentoL");
            this.pgbAvanamentoL.Name = "pgbAvanamentoL";
            // 
            // btnStartLettura
            // 
            resources.ApplyResources(this.btnStartLettura, "btnStartLettura");
            this.btnStartLettura.Name = "btnStartLettura";
            this.btnStartLettura.UseVisualStyleBackColor = true;
            this.btnStartLettura.Click += new System.EventHandler(this.btnStartLettura_Click);
            // 
            // pgbAvanamentoB
            // 
            resources.ApplyResources(this.pgbAvanamentoB, "pgbAvanamentoB");
            this.pgbAvanamentoB.Name = "pgbAvanamentoB";
            this.pgbAvanamentoB.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // lblAvanzmentoB
            // 
            resources.ApplyResources(this.lblAvanzmentoB, "lblAvanzmentoB");
            this.lblAvanzmentoB.Name = "lblAvanzmentoB";
            // 
            // btnAnnulla
            // 
            resources.ApplyResources(this.btnAnnulla, "btnAnnulla");
            this.btnAnnulla.Name = "btnAnnulla";
            this.btnAnnulla.UseVisualStyleBackColor = true;
            this.btnAnnulla.Click += new System.EventHandler(this.btnAnnulla_Click);
            // 
            // lblTitolo
            // 
            resources.ApplyResources(this.lblTitolo, "lblTitolo");
            this.lblTitolo.Name = "lblTitolo";
            // 
            // btnChiudi
            // 
            resources.ApplyResources(this.btnChiudi, "btnChiudi");
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // lblMessaggioAvanzamento
            // 
            resources.ApplyResources(this.lblMessaggioAvanzamento, "lblMessaggioAvanzamento");
            this.lblMessaggioAvanzamento.Name = "lblMessaggioAvanzamento";
            // 
            // lblMsg01
            // 
            resources.ApplyResources(this.lblMsg01, "lblMsg01");
            this.lblMsg01.Name = "lblMsg01";
            // 
            // lblMsg02
            // 
            resources.ApplyResources(this.lblMsg02, "lblMsg02");
            this.lblMsg02.Name = "lblMsg02";
            // 
            // lblMsgFail
            // 
            resources.ApplyResources(this.lblMsgFail, "lblMsgFail");
            this.lblMsgFail.Name = "lblMsgFail";
            // 
            // frmAvanzamentoCicli
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMsgFail);
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
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAvanzamentoCicli";
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
        private System.Windows.Forms.Label lblMsgFail;
    }
}