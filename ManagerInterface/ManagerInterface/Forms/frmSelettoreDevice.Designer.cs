namespace PannelloCharger
{
    partial class frmSelettoreDevice
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
            this.flvwListaDevices = new BrightIdeasSoftware.FastObjectListView();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.btnConnetti = new System.Windows.Forms.Button();
            this.btnAggiorna = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaDevices)).BeginInit();
            this.SuspendLayout();
            // 
            // flvwListaDevices
            // 
            this.flvwListaDevices.Location = new System.Drawing.Point(12, 12);
            this.flvwListaDevices.Name = "flvwListaDevices";
            this.flvwListaDevices.ShowGroups = false;
            this.flvwListaDevices.Size = new System.Drawing.Size(532, 244);
            this.flvwListaDevices.TabIndex = 1;
            this.flvwListaDevices.UseCompatibleStateImageBehavior = false;
            this.flvwListaDevices.View = System.Windows.Forms.View.Details;
            this.flvwListaDevices.VirtualMode = true;
            this.flvwListaDevices.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.flvwListaDevices_MouseDoubleClick);
            // 
            // btnChiudi
            // 
            this.btnChiudi.Location = new System.Drawing.Point(426, 274);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(118, 32);
            this.btnChiudi.TabIndex = 6;
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // btnConnetti
            // 
            this.btnConnetti.Location = new System.Drawing.Point(302, 274);
            this.btnConnetti.Name = "btnConnetti";
            this.btnConnetti.Size = new System.Drawing.Size(118, 32);
            this.btnConnetti.TabIndex = 7;
            this.btnConnetti.Text = "Collega";
            this.btnConnetti.UseVisualStyleBackColor = true;
            this.btnConnetti.Click += new System.EventHandler(this.btnConnetti_Click);
            // 
            // btnAggiorna
            // 
            this.btnAggiorna.Location = new System.Drawing.Point(12, 274);
            this.btnAggiorna.Name = "btnAggiorna";
            this.btnAggiorna.Size = new System.Drawing.Size(118, 32);
            this.btnAggiorna.TabIndex = 8;
            this.btnAggiorna.Text = "Aggiorna";
            this.btnAggiorna.UseVisualStyleBackColor = true;
            this.btnAggiorna.Click += new System.EventHandler(this.btnAggiorna_Click);
            // 
            // frmSelettoreDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 318);
            this.Controls.Add(this.btnAggiorna);
            this.Controls.Add(this.btnConnetti);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.flvwListaDevices);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelettoreDevice";
            this.ShowIcon = false;
            this.Text = "Selezione Dispositivi";
            this.Load += new System.EventHandler(this.frmSelettoreDevice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaDevices)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.FastObjectListView flvwListaDevices;
        private System.Windows.Forms.Button btnChiudi;
        private System.Windows.Forms.Button btnConnetti;
        private System.Windows.Forms.Button btnAggiorna;
    }
}