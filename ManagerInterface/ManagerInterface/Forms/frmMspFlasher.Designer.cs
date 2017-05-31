namespace PannelloCharger
{
    partial class frmMspFlasher
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
            this.btnEseguiComando = new System.Windows.Forms.Button();
            this.grbGeneraExcel = new System.Windows.Forms.GroupBox();
            this.btnSfoglia = new System.Windows.Forms.Button();
            this.txtFileComandi = new System.Windows.Forms.TextBox();
            this.ofdFileComandi = new System.Windows.Forms.OpenFileDialog();
            this.sfdExportEsito = new System.Windows.Forms.SaveFileDialog();
            this.txtCmdExitCode = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.cctlOsConsole = new ConsoleControl.ConsoleControl();
            this.grbGeneraExcel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEseguiComando
            // 
            this.btnEseguiComando.Location = new System.Drawing.Point(865, 703);
            this.btnEseguiComando.Name = "btnEseguiComando";
            this.btnEseguiComando.Size = new System.Drawing.Size(187, 35);
            this.btnEseguiComando.TabIndex = 23;
            this.btnEseguiComando.Text = "Esegui";
            this.btnEseguiComando.UseVisualStyleBackColor = true;
            this.btnEseguiComando.Click += new System.EventHandler(this.btnEseguiComando_Click);
            // 
            // grbGeneraExcel
            // 
            this.grbGeneraExcel.BackColor = System.Drawing.Color.White;
            this.grbGeneraExcel.Controls.Add(this.btnSfoglia);
            this.grbGeneraExcel.Controls.Add(this.txtFileComandi);
            this.grbGeneraExcel.Location = new System.Drawing.Point(34, 24);
            this.grbGeneraExcel.Name = "grbGeneraExcel";
            this.grbGeneraExcel.Size = new System.Drawing.Size(666, 78);
            this.grbGeneraExcel.TabIndex = 22;
            this.grbGeneraExcel.TabStop = false;
            this.grbGeneraExcel.Text = "Comando";
            // 
            // btnSfoglia
            // 
            this.btnSfoglia.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSfoglia.Location = new System.Drawing.Point(612, 29);
            this.btnSfoglia.Name = "btnSfoglia";
            this.btnSfoglia.Size = new System.Drawing.Size(33, 30);
            this.btnSfoglia.TabIndex = 6;
            this.btnSfoglia.Text = " ...";
            this.btnSfoglia.UseVisualStyleBackColor = true;
            // 
            // txtFileComandi
            // 
            this.txtFileComandi.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtFileComandi.Location = new System.Drawing.Point(22, 31);
            this.txtFileComandi.Name = "txtFileComandi";
            this.txtFileComandi.Size = new System.Drawing.Size(570, 24);
            this.txtFileComandi.TabIndex = 4;
            // 
            // ofdFileComandi
            // 
            this.ofdFileComandi.FileName = "openFileDialog1";
            // 
            // txtCmdExitCode
            // 
            this.txtCmdExitCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCmdExitCode.Location = new System.Drawing.Point(1122, 711);
            this.txtCmdExitCode.Name = "txtCmdExitCode";
            this.txtCmdExitCode.ReadOnly = true;
            this.txtCmdExitCode.Size = new System.Drawing.Size(99, 27);
            this.txtCmdExitCode.TabIndex = 88;
            this.txtCmdExitCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label25.Location = new System.Drawing.Point(1119, 691);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(67, 17);
            this.label25.TabIndex = 87;
            this.label25.Text = "Exit Code";
            // 
            // cctlOsConsole
            // 
            this.cctlOsConsole.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cctlOsConsole.ForeColor = System.Drawing.Color.Red;
            this.cctlOsConsole.IsInputEnabled = true;
            this.cctlOsConsole.Location = new System.Drawing.Point(34, 119);
            this.cctlOsConsole.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cctlOsConsole.Name = "cctlOsConsole";
            this.cctlOsConsole.SendKeyboardCommandsToProcess = false;
            this.cctlOsConsole.ShowDiagnostics = false;
            this.cctlOsConsole.Size = new System.Drawing.Size(1187, 540);
            this.cctlOsConsole.TabIndex = 89;
            this.cctlOsConsole.UseWaitCursor = true;
            // 
            // frmMspFlasher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1270, 763);
            this.Controls.Add(this.cctlOsConsole);
            this.Controls.Add(this.txtCmdExitCode);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.btnEseguiComando);
            this.Controls.Add(this.grbGeneraExcel);
            this.Name = "frmMspFlasher";
            this.Text = "MSP430 Flasher";
            this.Load += new System.EventHandler(this.frmMspFlasher_Load);
            this.grbGeneraExcel.ResumeLayout(false);
            this.grbGeneraExcel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnEseguiComando;
        private System.Windows.Forms.GroupBox grbGeneraExcel;
        private System.Windows.Forms.Button btnSfoglia;
        private System.Windows.Forms.TextBox txtFileComandi;
        private System.Windows.Forms.OpenFileDialog ofdFileComandi;
        private System.Windows.Forms.SaveFileDialog sfdExportEsito;
        private System.Windows.Forms.TextBox txtCmdExitCode;
        private System.Windows.Forms.Label label25;
        private ConsoleControl.ConsoleControl cctlOsConsole;
    }
}