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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelettoreDevice));
            this.flvwListaDevices = new BrightIdeasSoftware.FastObjectListView();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.btnConnetti = new System.Windows.Forms.Button();
            this.btnAggiorna = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaDevices)).BeginInit();
            this.SuspendLayout();
            // 
            // flvwListaDevices
            // 
            this.flvwListaDevices.CellEditUseWholeCell = false;
            this.flvwListaDevices.SelectedBackColor = System.Drawing.Color.Empty;
            this.flvwListaDevices.SelectedForeColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.flvwListaDevices, "flvwListaDevices");
            this.flvwListaDevices.Name = "flvwListaDevices";
            this.flvwListaDevices.ShowGroups = false;
            this.flvwListaDevices.UseCompatibleStateImageBehavior = false;
            this.flvwListaDevices.View = System.Windows.Forms.View.Details;
            this.flvwListaDevices.VirtualMode = true;
            this.flvwListaDevices.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.flvwListaDevices_MouseDoubleClick);
            // 
            // btnChiudi
            // 
            resources.ApplyResources(this.btnChiudi, "btnChiudi");
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // btnConnetti
            // 
            resources.ApplyResources(this.btnConnetti, "btnConnetti");
            this.btnConnetti.Name = "btnConnetti";
            this.btnConnetti.UseVisualStyleBackColor = true;
            this.btnConnetti.Click += new System.EventHandler(this.btnConnetti_Click);
            // 
            // btnAggiorna
            // 
            resources.ApplyResources(this.btnAggiorna, "btnAggiorna");
            this.btnAggiorna.Name = "btnAggiorna";
            this.btnAggiorna.UseVisualStyleBackColor = true;
            this.btnAggiorna.Click += new System.EventHandler(this.btnAggiorna_Click);
            // 
            // frmSelettoreDevice
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAggiorna);
            this.Controls.Add(this.btnConnetti);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.flvwListaDevices);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelettoreDevice";
            this.ShowIcon = false;
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