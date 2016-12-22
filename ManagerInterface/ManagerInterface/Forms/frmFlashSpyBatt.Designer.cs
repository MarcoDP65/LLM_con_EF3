namespace PannelloCharger
{
    partial class frmFlashSpyBatt
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
            this.ofdFileComandi = new System.Windows.Forms.OpenFileDialog();
            this.sfdExportEsito = new System.Windows.Forms.SaveFileDialog();
            this.grbGeneraExcel = new System.Windows.Forms.GroupBox();
            this.btnSfoglia = new System.Windows.Forms.Button();
            this.txtFileComandi = new System.Windows.Forms.TextBox();
            this.btnEseguiComando = new System.Windows.Forms.Button();
            this.txtOutputCoamndo = new System.Windows.Forms.TextBox();
            this.grbGeneraExcel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ofdFileComandi
            // 
            this.ofdFileComandi.FileName = "openFileDialog1";
            // 
            // grbGeneraExcel
            // 
            this.grbGeneraExcel.BackColor = System.Drawing.Color.White;
            this.grbGeneraExcel.Controls.Add(this.btnSfoglia);
            this.grbGeneraExcel.Controls.Add(this.txtFileComandi);
            this.grbGeneraExcel.Location = new System.Drawing.Point(26, 24);
            this.grbGeneraExcel.Name = "grbGeneraExcel";
            this.grbGeneraExcel.Size = new System.Drawing.Size(666, 78);
            this.grbGeneraExcel.TabIndex = 19;
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
            // btnEseguiComando
            // 
            this.btnEseguiComando.Location = new System.Drawing.Point(48, 154);
            this.btnEseguiComando.Name = "btnEseguiComando";
            this.btnEseguiComando.Size = new System.Drawing.Size(187, 35);
            this.btnEseguiComando.TabIndex = 20;
            this.btnEseguiComando.Text = "Esegui";
            this.btnEseguiComando.UseVisualStyleBackColor = true;
            this.btnEseguiComando.Click += new System.EventHandler(this.btnEseguiComando_Click);
            // 
            // txtOutputCoamndo
            // 
            this.txtOutputCoamndo.Location = new System.Drawing.Point(326, 160);
            this.txtOutputCoamndo.Multiline = true;
            this.txtOutputCoamndo.Name = "txtOutputCoamndo";
            this.txtOutputCoamndo.Size = new System.Drawing.Size(501, 348);
            this.txtOutputCoamndo.TabIndex = 21;
            // 
            // frmFlashSpyBatt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(1137, 685);
            this.Controls.Add(this.txtOutputCoamndo);
            this.Controls.Add(this.btnEseguiComando);
            this.Controls.Add(this.grbGeneraExcel);
            this.Name = "frmFlashSpyBatt";
            this.Text = "frmFlashSpyBatt";
            this.grbGeneraExcel.ResumeLayout(false);
            this.grbGeneraExcel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog ofdFileComandi;
        private System.Windows.Forms.SaveFileDialog sfdExportEsito;
        private System.Windows.Forms.GroupBox grbGeneraExcel;
        private System.Windows.Forms.Button btnSfoglia;
        private System.Windows.Forms.TextBox txtFileComandi;
        private System.Windows.Forms.Button btnEseguiComando;
        private System.Windows.Forms.TextBox txtOutputCoamndo;
    }
}