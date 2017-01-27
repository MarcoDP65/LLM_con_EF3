namespace PannelloCharger
{
    partial class frmTestFunzioni
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
            this.mtxtDataInizio = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNumeroGiorni = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGeneraLista = new System.Windows.Forms.Button();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.flvwListaTest = new BrightIdeasSoftware.FastObjectListView();
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaTest)).BeginInit();
            this.SuspendLayout();
            // 
            // mtxtDataInizio
            // 
            this.mtxtDataInizio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mtxtDataInizio.Location = new System.Drawing.Point(30, 46);
            this.mtxtDataInizio.Mask = "00/00/0000";
            this.mtxtDataInizio.Name = "mtxtDataInizio";
            this.mtxtDataInizio.Size = new System.Drawing.Size(158, 27);
            this.mtxtDataInizio.TabIndex = 0;
            this.mtxtDataInizio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mtxtDataInizio.ValidatingType = typeof(System.DateTime);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Inizio";
            // 
            // txtNumeroGiorni
            // 
            this.txtNumeroGiorni.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumeroGiorni.Location = new System.Drawing.Point(214, 46);
            this.txtNumeroGiorni.Name = "txtNumeroGiorni";
            this.txtNumeroGiorni.Size = new System.Drawing.Size(83, 27);
            this.txtNumeroGiorni.TabIndex = 2;
            this.txtNumeroGiorni.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(211, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Giorni";
            // 
            // btnGeneraLista
            // 
            this.btnGeneraLista.Location = new System.Drawing.Point(322, 40);
            this.btnGeneraLista.Name = "btnGeneraLista";
            this.btnGeneraLista.Size = new System.Drawing.Size(134, 40);
            this.btnGeneraLista.TabIndex = 4;
            this.btnGeneraLista.Text = "Genera";
            this.btnGeneraLista.UseVisualStyleBackColor = true;
            this.btnGeneraLista.Click += new System.EventHandler(this.btnGeneraLista_Click);
            // 
            // btnChiudi
            // 
            this.btnChiudi.Location = new System.Drawing.Point(1021, 691);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(134, 40);
            this.btnChiudi.TabIndex = 5;
            this.btnChiudi.TabStop = false;
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // flvwListaTest
            // 
            this.flvwListaTest.CellEditUseWholeCell = false;
            this.flvwListaTest.Location = new System.Drawing.Point(30, 126);
            this.flvwListaTest.Name = "flvwListaTest";
            this.flvwListaTest.ShowGroups = false;
            this.flvwListaTest.Size = new System.Drawing.Size(432, 527);
            this.flvwListaTest.TabIndex = 6;
            this.flvwListaTest.UseCompatibleStateImageBehavior = false;
            this.flvwListaTest.View = System.Windows.Forms.View.Details;
            this.flvwListaTest.VirtualMode = true;
            // 
            // frmTestFunzioni
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 772);
            this.Controls.Add(this.flvwListaTest);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.btnGeneraLista);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNumeroGiorni);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mtxtDataInizio);
            this.Name = "frmTestFunzioni";
            this.Text = "frmTestFunzioni";
            this.Load += new System.EventHandler(this.frmTestFunzioni_Load);
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaTest)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox mtxtDataInizio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNumeroGiorni;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGeneraLista;
        private System.Windows.Forms.Button btnChiudi;
        private BrightIdeasSoftware.FastObjectListView flvwListaTest;
    }
}