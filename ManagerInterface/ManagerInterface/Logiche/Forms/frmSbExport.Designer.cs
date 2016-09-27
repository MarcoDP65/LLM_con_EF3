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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSbExport));
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
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // txtMatrSB
            // 
            resources.ApplyResources(this.txtMatrSB, "txtMatrSB");
            this.txtMatrSB.Name = "txtMatrSB";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txtManufcturedBy
            // 
            resources.ApplyResources(this.txtManufcturedBy, "txtManufcturedBy");
            this.txtManufcturedBy.Name = "txtManufcturedBy";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // txtNote
            // 
            resources.ApplyResources(this.txtNote, "txtNote");
            this.txtNote.Name = "txtNote";
            // 
            // lblFirmwDisp
            // 
            resources.ApplyResources(this.lblFirmwDisp, "lblFirmwDisp");
            this.lblFirmwDisp.Name = "lblFirmwDisp";
            // 
            // txtCliente
            // 
            resources.ApplyResources(this.txtCliente, "txtCliente");
            this.txtCliente.Name = "txtCliente";
            // 
            // lblFirmwCb
            // 
            resources.ApplyResources(this.lblFirmwCb, "lblFirmwCb");
            this.lblFirmwCb.Name = "lblFirmwCb";
            // 
            // grbGeneraExcel
            // 
            this.grbGeneraExcel.BackColor = System.Drawing.Color.White;
            this.grbGeneraExcel.Controls.Add(this.btnSfoglia);
            this.grbGeneraExcel.Controls.Add(this.txtNuovoFile);
            resources.ApplyResources(this.grbGeneraExcel, "grbGeneraExcel");
            this.grbGeneraExcel.Name = "grbGeneraExcel";
            this.grbGeneraExcel.TabStop = false;
            // 
            // btnSfoglia
            // 
            resources.ApplyResources(this.btnSfoglia, "btnSfoglia");
            this.btnSfoglia.Name = "btnSfoglia";
            this.btnSfoglia.UseVisualStyleBackColor = true;
            this.btnSfoglia.Click += new System.EventHandler(this.btnSfoglia_Click);
            // 
            // txtNuovoFile
            // 
            resources.ApplyResources(this.txtNuovoFile, "txtNuovoFile");
            this.txtNuovoFile.Name = "txtNuovoFile";
            // 
            // chkChiudi
            // 
            resources.ApplyResources(this.chkChiudi, "chkChiudi");
            this.chkChiudi.Name = "chkChiudi";
            this.chkChiudi.UseVisualStyleBackColor = true;
            this.chkChiudi.Click += new System.EventHandler(this.chkChiudi_Click);
            // 
            // btnDataExport
            // 
            resources.ApplyResources(this.btnDataExport, "btnDataExport");
            this.btnDataExport.Name = "btnDataExport";
            this.btnDataExport.UseVisualStyleBackColor = true;
            this.btnDataExport.Click += new System.EventHandler(this.btnDataExport_Click);
            // 
            // ofdImportDati
            // 
            this.ofdImportDati.FileName = "openFileDialog1";
            // 
            // btnAnteprima
            // 
            resources.ApplyResources(this.btnAnteprima, "btnAnteprima");
            this.btnAnteprima.Name = "btnAnteprima";
            this.btnAnteprima.UseVisualStyleBackColor = true;
            this.btnAnteprima.Click += new System.EventHandler(this.btnAnteprima_Click);
            // 
            // frmSbExport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.Controls.Add(this.btnAnteprima);
            this.Controls.Add(this.btnDataExport);
            this.Controls.Add(this.chkChiudi);
            this.Controls.Add(this.grbGeneraExcel);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmSbExport";
            this.Load += new System.EventHandler(this.frmSbExport_Load);
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