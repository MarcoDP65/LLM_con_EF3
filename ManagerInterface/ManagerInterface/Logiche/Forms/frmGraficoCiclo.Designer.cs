namespace PannelloCharger
{
    partial class frmGraficoCiclo
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
            this.button1 = new System.Windows.Forms.Button();
            this.pbCicloIUIa = new System.Windows.Forms.PictureBox();
            this.pbCicloIWa = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbCicloIUIa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCicloIWa)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(411, 522);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "chiudi";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pbCicloIUIa
            // 
            this.pbCicloIUIa.Image = global::PannelloCharger.Properties.Resources.IUIa;
            this.pbCicloIUIa.Location = new System.Drawing.Point(12, 19);
            this.pbCicloIUIa.Name = "pbCicloIUIa";
            this.pbCicloIUIa.Size = new System.Drawing.Size(939, 497);
            this.pbCicloIUIa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCicloIUIa.TabIndex = 2;
            this.pbCicloIUIa.TabStop = false;
            this.pbCicloIUIa.Visible = false;
            // 
            // pbCicloIWa
            // 
            this.pbCicloIWa.Image = global::PannelloCharger.Properties.Resources.IWa;
            this.pbCicloIWa.Location = new System.Drawing.Point(12, 19);
            this.pbCicloIWa.Name = "pbCicloIWa";
            this.pbCicloIWa.Size = new System.Drawing.Size(939, 497);
            this.pbCicloIWa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCicloIWa.TabIndex = 0;
            this.pbCicloIWa.TabStop = false;
            // 
            // frmGraficoCiclo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 569);
            this.Controls.Add(this.pbCicloIUIa);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pbCicloIWa);
            this.Name = "frmGraficoCiclo";
            this.ShowIcon = false;
            this.Text = "Grafico Ciclo";
            ((System.ComponentModel.ISupportInitialize)(this.pbCicloIUIa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCicloIWa)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbCicloIWa;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pbCicloIUIa;
    }
}