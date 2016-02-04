namespace PannelloCharger
{
    partial class frmSbExport
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMatrSB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtManufcturedBy = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.lblFirmwDisp = new System.Windows.Forms.Label();
            this.txtCliente = new System.Windows.Forms.TextBox();
            this.lblFirmwCb = new System.Windows.Forms.Label();
            this.grbGeneraExcel = new System.Windows.Forms.GroupBox();
            this.btnSfoglia = new System.Windows.Forms.Button();
            this.txtNuovoFile = new System.Windows.Forms.TextBox();
            this.chkChiudi = new System.Windows.Forms.Button();
            this.btnDataExport = new System.Windows.Forms.Button();
            this.sfdExportDati = new System.Windows.Forms.SaveFileDialog();
            this.ofdImportDati = new System.Windows.Forms.OpenFileDialog();
            this.btnAnteprima = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.grbGeneraExcel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.txtMatrSB);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtManufcturedBy);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtNote);
            this.groupBox2.Controls.Add(this.lblFirmwDisp);
            this.groupBox2.Controls.Add(this.txtCliente);
            this.groupBox2.Controls.Add(this.lblFirmwCb);
            this.groupBox2.Location = new System.Drawing.Point(17, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(666, 171);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SPY-BATT";
            // 
            // txtMatrSB
            // 
            this.txtMatrSB.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMatrSB.Location = new System.Drawing.Point(22, 52);
            this.txtMatrSB.Name = "txtMatrSB";
            this.txtMatrSB.Size = new System.Drawing.Size(263, 27);
            this.txtMatrSB.TabIndex = 28;
            this.txtMatrSB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 17);
            this.label6.TabIndex = 27;
            this.label6.Text = "Matricola";
            // 
            // txtManufcturedBy
            // 
            this.txtManufcturedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtManufcturedBy.Location = new System.Drawing.Point(291, 52);
            this.txtManufcturedBy.Name = "txtManufcturedBy";
            this.txtManufcturedBy.Size = new System.Drawing.Size(354, 27);
            this.txtManufcturedBy.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(288, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 17);
            this.label7.TabIndex = 25;
            this.label7.Text = "Modello";
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(291, 115);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(354, 22);
            this.txtNote.TabIndex = 16;
            // 
            // lblFirmwDisp
            // 
            this.lblFirmwDisp.AutoSize = true;
            this.lblFirmwDisp.Location = new System.Drawing.Point(288, 95);
            this.lblFirmwDisp.Name = "lblFirmwDisp";
            this.lblFirmwDisp.Size = new System.Drawing.Size(38, 17);
            this.lblFirmwDisp.TabIndex = 15;
            this.lblFirmwDisp.Text = "Note";
            // 
            // txtCliente
            // 
            this.txtCliente.Location = new System.Drawing.Point(22, 115);
            this.txtCliente.Name = "txtCliente";
            this.txtCliente.Size = new System.Drawing.Size(263, 22);
            this.txtCliente.TabIndex = 14;
            // 
            // lblFirmwCb
            // 
            this.lblFirmwCb.AutoSize = true;
            this.lblFirmwCb.Location = new System.Drawing.Point(19, 95);
            this.lblFirmwCb.Name = "lblFirmwCb";
            this.lblFirmwCb.Size = new System.Drawing.Size(51, 17);
            this.lblFirmwCb.TabIndex = 13;
            this.lblFirmwCb.Text = "Cliente";
            // 
            // grbGeneraExcel
            // 
            this.grbGeneraExcel.BackColor = System.Drawing.Color.White;
            this.grbGeneraExcel.Controls.Add(this.btnSfoglia);
            this.grbGeneraExcel.Controls.Add(this.txtNuovoFile);
            this.grbGeneraExcel.Location = new System.Drawing.Point(17, 215);
            this.grbGeneraExcel.Name = "grbGeneraExcel";
            this.grbGeneraExcel.Size = new System.Drawing.Size(666, 74);
            this.grbGeneraExcel.TabIndex = 18;
            this.grbGeneraExcel.TabStop = false;
            this.grbGeneraExcel.Text = "Selezione File";
            // 
            // btnSfoglia
            // 
            this.btnSfoglia.Location = new System.Drawing.Point(612, 29);
            this.btnSfoglia.Name = "btnSfoglia";
            this.btnSfoglia.Size = new System.Drawing.Size(33, 30);
            this.btnSfoglia.TabIndex = 6;
            this.btnSfoglia.Text = " ...";
            this.btnSfoglia.UseVisualStyleBackColor = true;
            this.btnSfoglia.Click += new System.EventHandler(this.btnSfoglia_Click);
            // 
            // txtNuovoFile
            // 
            this.txtNuovoFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNuovoFile.Location = new System.Drawing.Point(22, 31);
            this.txtNuovoFile.Name = "txtNuovoFile";
            this.txtNuovoFile.Size = new System.Drawing.Size(570, 24);
            this.txtNuovoFile.TabIndex = 4;
            // 
            // chkChiudi
            // 
            this.chkChiudi.Location = new System.Drawing.Point(530, 323);
            this.chkChiudi.Name = "chkChiudi";
            this.chkChiudi.Size = new System.Drawing.Size(153, 40);
            this.chkChiudi.TabIndex = 20;
            this.chkChiudi.Text = "Chiudi";
            this.chkChiudi.UseVisualStyleBackColor = true;
            this.chkChiudi.Click += new System.EventHandler(this.chkChiudi_Click);
            // 
            // btnDataExport
            // 
            this.btnDataExport.Location = new System.Drawing.Point(357, 323);
            this.btnDataExport.Name = "btnDataExport";
            this.btnDataExport.Size = new System.Drawing.Size(153, 40);
            this.btnDataExport.TabIndex = 21;
            this.btnDataExport.Text = "Salva Dati";
            this.btnDataExport.UseVisualStyleBackColor = true;
            this.btnDataExport.Click += new System.EventHandler(this.btnDataExport_Click);
            // 
            // ofdImportDati
            // 
            this.ofdImportDati.FileName = "openFileDialog1";
            // 
            // btnAnteprima
            // 
            this.btnAnteprima.Location = new System.Drawing.Point(17, 323);
            this.btnAnteprima.Name = "btnAnteprima";
            this.btnAnteprima.Size = new System.Drawing.Size(153, 40);
            this.btnAnteprima.TabIndex = 22;
            this.btnAnteprima.Text = "Anteprima dati";
            this.btnAnteprima.UseVisualStyleBackColor = true;
            this.btnAnteprima.Click += new System.EventHandler(this.btnAnteprima_Click);
            // 
            // frmSbExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(701, 381);
            this.Controls.Add(this.btnAnteprima);
            this.Controls.Add(this.btnDataExport);
            this.Controls.Add(this.chkChiudi);
            this.Controls.Add(this.grbGeneraExcel);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmSbExport";
            this.Text = "frmSbExport";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grbGeneraExcel.ResumeLayout(false);
            this.grbGeneraExcel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtMatrSB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtManufcturedBy;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Label lblFirmwDisp;
        private System.Windows.Forms.TextBox txtCliente;
        private System.Windows.Forms.Label lblFirmwCb;
        private System.Windows.Forms.GroupBox grbGeneraExcel;
        private System.Windows.Forms.Button btnSfoglia;
        private System.Windows.Forms.TextBox txtNuovoFile;
        private System.Windows.Forms.Button chkChiudi;
        private System.Windows.Forms.Button btnDataExport;
        private System.Windows.Forms.SaveFileDialog sfdExportDati;
        private System.Windows.Forms.OpenFileDialog ofdImportDati;
        private System.Windows.Forms.Button btnAnteprima;
    }
}