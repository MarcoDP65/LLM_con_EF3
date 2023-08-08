namespace PannelloCharger
{
    partial class frmSetupLLT
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
            this.tbcFunzioni = new System.Windows.Forms.TabControl();
            this.tbpSetupCorrente = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.grbConnessioni = new System.Windows.Forms.GroupBox();
            this.pbRicerca = new System.Windows.Forms.ProgressBar();
            this.btnAggiorna = new System.Windows.Forms.Button();
            this.btnConnetti = new System.Windows.Forms.Button();
            this.flvwListaDevices = new BrightIdeasSoftware.FastObjectListView();
            this.fastObjectListView1 = new BrightIdeasSoftware.FastObjectListView();
            this.tbpListaSetup = new System.Windows.Forms.TabPage();
            this.btnNuovoSetup = new System.Windows.Forms.Button();
            this.btnSelezionaSetup = new System.Windows.Forms.Button();
            this.flvListaSetup = new BrightIdeasSoftware.FastObjectListView();
            this.tbcFunzioni.SuspendLayout();
            this.tbpSetupCorrente.SuspendLayout();
            this.grbConnessioni.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).BeginInit();
            this.tbpListaSetup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvListaSetup)).BeginInit();
            this.SuspendLayout();
            // 
            // tbcFunzioni
            // 
            this.tbcFunzioni.Controls.Add(this.tbpSetupCorrente);
            this.tbcFunzioni.Controls.Add(this.tbpListaSetup);
            this.tbcFunzioni.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcFunzioni.Location = new System.Drawing.Point(0, 0);
            this.tbcFunzioni.Name = "tbcFunzioni";
            this.tbcFunzioni.SelectedIndex = 0;
            this.tbcFunzioni.Size = new System.Drawing.Size(1176, 602);
            this.tbcFunzioni.TabIndex = 0;
            // 
            // tbpSetupCorrente
            // 
            this.tbpSetupCorrente.Controls.Add(this.listBox1);
            this.tbpSetupCorrente.Controls.Add(this.grbConnessioni);
            this.tbpSetupCorrente.Controls.Add(this.fastObjectListView1);
            this.tbpSetupCorrente.Location = new System.Drawing.Point(4, 22);
            this.tbpSetupCorrente.Name = "tbpSetupCorrente";
            this.tbpSetupCorrente.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSetupCorrente.Size = new System.Drawing.Size(1168, 576);
            this.tbpSetupCorrente.TabIndex = 1;
            this.tbpSetupCorrente.Text = "Setup Attivo";
            this.tbpSetupCorrente.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(23, 308);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(233, 108);
            this.listBox1.TabIndex = 2;
            // 
            // grbConnessioni
            // 
            this.grbConnessioni.Controls.Add(this.pbRicerca);
            this.grbConnessioni.Controls.Add(this.btnAggiorna);
            this.grbConnessioni.Controls.Add(this.btnConnetti);
            this.grbConnessioni.Controls.Add(this.flvwListaDevices);
            this.grbConnessioni.Location = new System.Drawing.Point(18, 21);
            this.grbConnessioni.Name = "grbConnessioni";
            this.grbConnessioni.Size = new System.Drawing.Size(452, 268);
            this.grbConnessioni.TabIndex = 1;
            this.grbConnessioni.TabStop = false;
            this.grbConnessioni.Text = "Dispositivi Connessi";
            // 
            // pbRicerca
            // 
            this.pbRicerca.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pbRicerca.Location = new System.Drawing.Point(5, 210);
            this.pbRicerca.Margin = new System.Windows.Forms.Padding(2);
            this.pbRicerca.Name = "pbRicerca";
            this.pbRicerca.Size = new System.Drawing.Size(442, 19);
            this.pbRicerca.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbRicerca.TabIndex = 14;
            this.pbRicerca.Visible = false;
            // 
            // btnAggiorna
            // 
            this.btnAggiorna.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAggiorna.Location = new System.Drawing.Point(5, 231);
            this.btnAggiorna.Margin = new System.Windows.Forms.Padding(2);
            this.btnAggiorna.Name = "btnAggiorna";
            this.btnAggiorna.Size = new System.Drawing.Size(74, 26);
            this.btnAggiorna.TabIndex = 13;
            this.btnAggiorna.Text = "Aggiorna";
            this.btnAggiorna.UseVisualStyleBackColor = true;
            this.btnAggiorna.Click += new System.EventHandler(this.btnAggiorna_Click);
            // 
            // btnConnetti
            // 
            this.btnConnetti.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnConnetti.Location = new System.Drawing.Point(222, 231);
            this.btnConnetti.Margin = new System.Windows.Forms.Padding(2);
            this.btnConnetti.Name = "btnConnetti";
            this.btnConnetti.Size = new System.Drawing.Size(88, 26);
            this.btnConnetti.TabIndex = 12;
            this.btnConnetti.Text = "Collega";
            this.btnConnetti.UseVisualStyleBackColor = true;
            this.btnConnetti.Click += new System.EventHandler(this.btnConnetti_Click);
            // 
            // flvwListaDevices
            // 
            this.flvwListaDevices.CellEditUseWholeCell = false;
            this.flvwListaDevices.HideSelection = false;
            this.flvwListaDevices.Location = new System.Drawing.Point(5, 18);
            this.flvwListaDevices.Margin = new System.Windows.Forms.Padding(2);
            this.flvwListaDevices.Name = "flvwListaDevices";
            this.flvwListaDevices.ShowGroups = false;
            this.flvwListaDevices.Size = new System.Drawing.Size(442, 191);
            this.flvwListaDevices.TabIndex = 11;
            this.flvwListaDevices.UseCompatibleStateImageBehavior = false;
            this.flvwListaDevices.View = System.Windows.Forms.View.Details;
            this.flvwListaDevices.VirtualMode = true;
            // 
            // fastObjectListView1
            // 
            this.fastObjectListView1.CellEditUseWholeCell = false;
            this.fastObjectListView1.HideSelection = false;
            this.fastObjectListView1.Location = new System.Drawing.Point(630, 61);
            this.fastObjectListView1.Name = "fastObjectListView1";
            this.fastObjectListView1.ShowGroups = false;
            this.fastObjectListView1.Size = new System.Drawing.Size(417, 476);
            this.fastObjectListView1.TabIndex = 0;
            this.fastObjectListView1.UseCompatibleStateImageBehavior = false;
            this.fastObjectListView1.View = System.Windows.Forms.View.Details;
            this.fastObjectListView1.VirtualMode = true;
            // 
            // tbpListaSetup
            // 
            this.tbpListaSetup.Controls.Add(this.btnNuovoSetup);
            this.tbpListaSetup.Controls.Add(this.btnSelezionaSetup);
            this.tbpListaSetup.Controls.Add(this.flvListaSetup);
            this.tbpListaSetup.Location = new System.Drawing.Point(4, 22);
            this.tbpListaSetup.Name = "tbpListaSetup";
            this.tbpListaSetup.Padding = new System.Windows.Forms.Padding(3);
            this.tbpListaSetup.Size = new System.Drawing.Size(1168, 576);
            this.tbpListaSetup.TabIndex = 0;
            this.tbpListaSetup.Text = "Lista Setup";
            this.tbpListaSetup.UseVisualStyleBackColor = true;
            // 
            // btnNuovoSetup
            // 
            this.btnNuovoSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnNuovoSetup.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNuovoSetup.Location = new System.Drawing.Point(260, 488);
            this.btnNuovoSetup.Margin = new System.Windows.Forms.Padding(2);
            this.btnNuovoSetup.Name = "btnNuovoSetup";
            this.btnNuovoSetup.Size = new System.Drawing.Size(220, 35);
            this.btnNuovoSetup.TabIndex = 70;
            this.btnNuovoSetup.Text = "Nuova Configurazione";
            this.btnNuovoSetup.UseVisualStyleBackColor = true;
            // 
            // btnSelezionaSetup
            // 
            this.btnSelezionaSetup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnSelezionaSetup.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelezionaSetup.Location = new System.Drawing.Point(20, 488);
            this.btnSelezionaSetup.Margin = new System.Windows.Forms.Padding(2);
            this.btnSelezionaSetup.Name = "btnSelezionaSetup";
            this.btnSelezionaSetup.Size = new System.Drawing.Size(220, 35);
            this.btnSelezionaSetup.TabIndex = 69;
            this.btnSelezionaSetup.Text = "Seleziona Configurazione";
            this.btnSelezionaSetup.UseVisualStyleBackColor = true;
            // 
            // flvListaSetup
            // 
            this.flvListaSetup.CellEditUseWholeCell = false;
            this.flvListaSetup.HideSelection = false;
            this.flvListaSetup.Location = new System.Drawing.Point(20, 23);
            this.flvListaSetup.Name = "flvListaSetup";
            this.flvListaSetup.ShowGroups = false;
            this.flvListaSetup.Size = new System.Drawing.Size(1123, 439);
            this.flvListaSetup.TabIndex = 0;
            this.flvListaSetup.UseCompatibleStateImageBehavior = false;
            this.flvListaSetup.View = System.Windows.Forms.View.Details;
            this.flvListaSetup.VirtualMode = true;
            // 
            // frmSetupLLT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 602);
            this.Controls.Add(this.tbcFunzioni);
            this.Name = "frmSetupLLT";
            this.ShowIcon = false;
            this.Text = "Setup LADELIGHT / SUPERCHARGER DISPLAY";
            this.Load += new System.EventHandler(this.frmSetupLLT_Load);
            this.tbcFunzioni.ResumeLayout(false);
            this.tbpSetupCorrente.ResumeLayout(false);
            this.grbConnessioni.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaDevices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastObjectListView1)).EndInit();
            this.tbpListaSetup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.flvListaSetup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcFunzioni;
        private System.Windows.Forms.TabPage tbpListaSetup;
        private System.Windows.Forms.TabPage tbpSetupCorrente;
        private BrightIdeasSoftware.FastObjectListView flvListaSetup;
        private BrightIdeasSoftware.FastObjectListView fastObjectListView1;
        private System.Windows.Forms.Button btnSelezionaSetup;
        private System.Windows.Forms.Button btnNuovoSetup;
        private System.Windows.Forms.GroupBox grbConnessioni;
        private System.Windows.Forms.ProgressBar pbRicerca;
        private System.Windows.Forms.Button btnAggiorna;
        private System.Windows.Forms.Button btnConnetti;
        private BrightIdeasSoftware.FastObjectListView flvwListaDevices;
        private System.Windows.Forms.ListBox listBox1;
    }
}