﻿namespace PannelloCharger
{
    partial class frmMain
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Liberare le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.sstMain = new System.Windows.Forms.StatusStrip();
            this.sstProgressBase = new System.Windows.Forms.ToolStripProgressBar();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.impostaStampanteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStampa = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.linguaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.italianoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEsci = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCaricabatteria = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuConnettiCb = new System.Windows.Forms.ToolStripMenuItem();
            this.apparatiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orologioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuLLDisplayManager = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSpybat = new System.Windows.Forms.ToolStripMenuItem();
            this.caricaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.archivioToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.importaFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.alimentatoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuServizi = new System.Windows.Forms.ToolStripMenuItem();
            this.flashFTDIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flashSPYBATTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.programmazioniAvanzateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImpostazioni = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCercaDispositiviUSB = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCercaDispositiviCOM = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSelezionePorta = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPortaSeriale = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuConsole = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInformazioniSu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.tstPulsanti = new System.Windows.Forms.ToolStrip();
            this.tstBtnCercaUsb = new System.Windows.Forms.ToolStripButton();
            this.tstBtnCercaRS232 = new System.Windows.Forms.ToolStripButton();
            this.tstBtnExport = new System.Windows.Forms.ToolStripButton();
            this.tstBtnPrint = new System.Windows.Forms.ToolStripButton();
            this.tstBtnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tstBtnClose = new System.Windows.Forms.ToolStripButton();
            this.pdoStampaForm = new System.Drawing.Printing.PrintDocument();
            this.sstMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.tstPulsanti.SuspendLayout();
            this.SuspendLayout();
            // 
            // sstMain
            // 
            this.sstMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.sstMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sstProgressBase});
            resources.ApplyResources(this.sstMain, "sstMain");
            this.sstMain.Name = "sstMain";
            // 
            // sstProgressBase
            // 
            this.sstProgressBase.Name = "sstProgressBase";
            resources.ApplyResources(this.sstProgressBase, "sstProgressBase");
            // 
            // mnuMain
            // 
            this.mnuMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuCaricabatteria,
            this.mnuSpybat,
            this.mnuServizi,
            this.mnuImpostazioni,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            resources.ApplyResources(this.mnuMain, "mnuMain");
            this.mnuMain.Name = "mnuMain";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.impostaStampanteToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.mnuStampa,
            this.toolStripSeparator1,
            this.linguaToolStripMenuItem,
            this.toolStripMenuItem5,
            this.mnuLogout,
            this.mnuEsci});
            this.mnuFile.Name = "mnuFile";
            resources.ApplyResources(this.mnuFile, "mnuFile");
            // 
            // impostaStampanteToolStripMenuItem
            // 
            this.impostaStampanteToolStripMenuItem.Name = "impostaStampanteToolStripMenuItem";
            resources.ApplyResources(this.impostaStampanteToolStripMenuItem, "impostaStampanteToolStripMenuItem");
            this.impostaStampanteToolStripMenuItem.Click += new System.EventHandler(this.impostaStampanteToolStripMenuItem_Click);
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            resources.ApplyResources(this.printPreviewToolStripMenuItem, "printPreviewToolStripMenuItem");
            this.printPreviewToolStripMenuItem.Click += new System.EventHandler(this.printPreviewToolStripMenuItem_Click);
            // 
            // mnuStampa
            // 
            this.mnuStampa.Name = "mnuStampa";
            resources.ApplyResources(this.mnuStampa, "mnuStampa");
            this.mnuStampa.Click += new System.EventHandler(this.mnuStampa_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // linguaToolStripMenuItem
            // 
            this.linguaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.italianoToolStripMenuItem,
            this.englishToolStripMenuItem});
            this.linguaToolStripMenuItem.Name = "linguaToolStripMenuItem";
            resources.ApplyResources(this.linguaToolStripMenuItem, "linguaToolStripMenuItem");
            // 
            // italianoToolStripMenuItem
            // 
            this.italianoToolStripMenuItem.Name = "italianoToolStripMenuItem";
            resources.ApplyResources(this.italianoToolStripMenuItem, "italianoToolStripMenuItem");
            this.italianoToolStripMenuItem.Click += new System.EventHandler(this.italianoToolStripMenuItem_Click);
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            resources.ApplyResources(this.englishToolStripMenuItem, "englishToolStripMenuItem");
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            resources.ApplyResources(this.toolStripMenuItem5, "toolStripMenuItem5");
            // 
            // mnuLogout
            // 
            this.mnuLogout.Name = "mnuLogout";
            resources.ApplyResources(this.mnuLogout, "mnuLogout");
            this.mnuLogout.Click += new System.EventHandler(this.mnuLogout_Click);
            // 
            // mnuEsci
            // 
            this.mnuEsci.Name = "mnuEsci";
            resources.ApplyResources(this.mnuEsci, "mnuEsci");
            this.mnuEsci.Click += new System.EventHandler(this.mnuEsci_Click);
            // 
            // mnuCaricabatteria
            // 
            this.mnuCaricabatteria.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuConnettiCb,
            this.apparatiToolStripMenuItem,
            this.orologioToolStripMenuItem,
            this.toolStripMenuItem6,
            this.mnuLLDisplayManager});
            this.mnuCaricabatteria.Name = "mnuCaricabatteria";
            resources.ApplyResources(this.mnuCaricabatteria, "mnuCaricabatteria");
            // 
            // mnuConnettiCb
            // 
            this.mnuConnettiCb.Name = "mnuConnettiCb";
            resources.ApplyResources(this.mnuConnettiCb, "mnuConnettiCb");
            this.mnuConnettiCb.Click += new System.EventHandler(this.mnuConnettiCb_Click);
            // 
            // apparatiToolStripMenuItem
            // 
            resources.ApplyResources(this.apparatiToolStripMenuItem, "apparatiToolStripMenuItem");
            this.apparatiToolStripMenuItem.Name = "apparatiToolStripMenuItem";
            // 
            // orologioToolStripMenuItem
            // 
            resources.ApplyResources(this.orologioToolStripMenuItem, "orologioToolStripMenuItem");
            this.orologioToolStripMenuItem.Name = "orologioToolStripMenuItem";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            resources.ApplyResources(this.toolStripMenuItem6, "toolStripMenuItem6");
            // 
            // mnuLLDisplayManager
            // 
            this.mnuLLDisplayManager.Name = "mnuLLDisplayManager";
            resources.ApplyResources(this.mnuLLDisplayManager, "mnuLLDisplayManager");
            this.mnuLLDisplayManager.Click += new System.EventHandler(this.mnuLLDisplayManager_Click);
            // 
            // mnuSpybat
            // 
            this.mnuSpybat.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.caricaToolStripMenuItem,
            this.archivioToolStripMenuItem,
            this.toolStripMenuItem1,
            this.importaFileToolStripMenuItem,
            this.toolStripMenuItem4,
            this.alimentatoreToolStripMenuItem});
            this.mnuSpybat.Name = "mnuSpybat";
            resources.ApplyResources(this.mnuSpybat, "mnuSpybat");
            // 
            // caricaToolStripMenuItem
            // 
            this.caricaToolStripMenuItem.Name = "caricaToolStripMenuItem";
            resources.ApplyResources(this.caricaToolStripMenuItem, "caricaToolStripMenuItem");
            this.caricaToolStripMenuItem.Click += new System.EventHandler(this.caricaToolStripMenuItem_Click);
            // 
            // archivioToolStripMenuItem
            // 
            this.archivioToolStripMenuItem.Name = "archivioToolStripMenuItem";
            resources.ApplyResources(this.archivioToolStripMenuItem, "archivioToolStripMenuItem");
            this.archivioToolStripMenuItem.Click += new System.EventHandler(this.archivioToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // importaFileToolStripMenuItem
            // 
            this.importaFileToolStripMenuItem.Name = "importaFileToolStripMenuItem";
            resources.ApplyResources(this.importaFileToolStripMenuItem, "importaFileToolStripMenuItem");
            this.importaFileToolStripMenuItem.Click += new System.EventHandler(this.importaFileToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            resources.ApplyResources(this.toolStripMenuItem4, "toolStripMenuItem4");
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // alimentatoreToolStripMenuItem
            // 
            this.alimentatoreToolStripMenuItem.Name = "alimentatoreToolStripMenuItem";
            resources.ApplyResources(this.alimentatoreToolStripMenuItem, "alimentatoreToolStripMenuItem");
            this.alimentatoreToolStripMenuItem.Click += new System.EventHandler(this.alimentatoreToolStripMenuItem_Click);
            // 
            // mnuServizi
            // 
            this.mnuServizi.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flashFTDIToolStripMenuItem,
            this.flashSPYBATTToolStripMenuItem,
            this.programmazioniAvanzateToolStripMenuItem});
            this.mnuServizi.Name = "mnuServizi";
            resources.ApplyResources(this.mnuServizi, "mnuServizi");
            // 
            // flashFTDIToolStripMenuItem
            // 
            this.flashFTDIToolStripMenuItem.Name = "flashFTDIToolStripMenuItem";
            resources.ApplyResources(this.flashFTDIToolStripMenuItem, "flashFTDIToolStripMenuItem");
            this.flashFTDIToolStripMenuItem.Click += new System.EventHandler(this.flashFTDIToolStripMenuItem_Click);
            // 
            // flashSPYBATTToolStripMenuItem
            // 
            this.flashSPYBATTToolStripMenuItem.Name = "flashSPYBATTToolStripMenuItem";
            resources.ApplyResources(this.flashSPYBATTToolStripMenuItem, "flashSPYBATTToolStripMenuItem");
            // 
            // programmazioniAvanzateToolStripMenuItem
            // 
            this.programmazioniAvanzateToolStripMenuItem.Name = "programmazioniAvanzateToolStripMenuItem";
            resources.ApplyResources(this.programmazioniAvanzateToolStripMenuItem, "programmazioniAvanzateToolStripMenuItem");
            this.programmazioniAvanzateToolStripMenuItem.Click += new System.EventHandler(this.programmazioniAvanzateToolStripMenuItem_Click);
            // 
            // mnuImpostazioni
            // 
            resources.ApplyResources(this.mnuImpostazioni, "mnuImpostazioni");
            this.mnuImpostazioni.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCercaDispositiviUSB,
            this.mnuCercaDispositiviCOM,
            this.mnuSelezionePorta,
            this.mnuPortaSeriale,
            this.mnuConsole});
            this.mnuImpostazioni.Name = "mnuImpostazioni";
            // 
            // mnuCercaDispositiviUSB
            // 
            this.mnuCercaDispositiviUSB.Name = "mnuCercaDispositiviUSB";
            resources.ApplyResources(this.mnuCercaDispositiviUSB, "mnuCercaDispositiviUSB");
            this.mnuCercaDispositiviUSB.Click += new System.EventHandler(this.cercaDispositiviUSBToolStripMenuItem_Click);
            // 
            // mnuCercaDispositiviCOM
            // 
            this.mnuCercaDispositiviCOM.Name = "mnuCercaDispositiviCOM";
            resources.ApplyResources(this.mnuCercaDispositiviCOM, "mnuCercaDispositiviCOM");
            this.mnuCercaDispositiviCOM.Click += new System.EventHandler(this.mnuCercaDispositivi_Click);
            // 
            // mnuSelezionePorta
            // 
            this.mnuSelezionePorta.Name = "mnuSelezionePorta";
            resources.ApplyResources(this.mnuSelezionePorta, "mnuSelezionePorta");
            this.mnuSelezionePorta.Click += new System.EventHandler(this.mnuSelezionePorta_Click);
            // 
            // mnuPortaSeriale
            // 
            this.mnuPortaSeriale.Name = "mnuPortaSeriale";
            resources.ApplyResources(this.mnuPortaSeriale, "mnuPortaSeriale");
            this.mnuPortaSeriale.Click += new System.EventHandler(this.portaSerialeToolStripMenuItem_Click);
            // 
            // mnuConsole
            // 
            this.mnuConsole.Name = "mnuConsole";
            resources.ApplyResources(this.mnuConsole, "mnuConsole");
            this.mnuConsole.Click += new System.EventHandler(this.mnuConsole_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInformazioniSu});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // mnuInformazioniSu
            // 
            this.mnuInformazioniSu.Name = "mnuInformazioniSu";
            resources.ApplyResources(this.mnuInformazioniSu, "mnuInformazioniSu");
            this.mnuInformazioniSu.Click += new System.EventHandler(this.mnuInformazioniSu_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            resources.ApplyResources(this.toolStripMenuItem3, "toolStripMenuItem3");
            // 
            // tstPulsanti
            // 
            this.tstPulsanti.ImageScalingSize = new System.Drawing.Size(32, 32);
            resources.ApplyResources(this.tstPulsanti, "tstPulsanti");
            this.tstPulsanti.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstBtnCercaUsb,
            this.tstBtnCercaRS232,
            this.tstBtnExport,
            this.tstBtnPrint,
            this.tstBtnRefresh,
            this.toolStripSeparator2,
            this.tstBtnClose});
            this.tstPulsanti.Name = "tstPulsanti";
            // 
            // tstBtnCercaUsb
            // 
            this.tstBtnCercaUsb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tstBtnCercaUsb.Image = global::PannelloCharger.Properties.Resources.usb;
            resources.ApplyResources(this.tstBtnCercaUsb, "tstBtnCercaUsb");
            this.tstBtnCercaUsb.Name = "tstBtnCercaUsb";
            this.tstBtnCercaUsb.Click += new System.EventHandler(this.tstBtnCercaUsb_Click);
            // 
            // tstBtnCercaRS232
            // 
            this.tstBtnCercaRS232.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tstBtnCercaRS232, "tstBtnCercaRS232");
            this.tstBtnCercaRS232.Name = "tstBtnCercaRS232";
            this.tstBtnCercaRS232.Click += new System.EventHandler(this.tstBtnCercaRS232_Click);
            // 
            // tstBtnExport
            // 
            this.tstBtnExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tstBtnExport, "tstBtnExport");
            this.tstBtnExport.Image = global::PannelloCharger.Properties.Resources.export_01;
            this.tstBtnExport.Name = "tstBtnExport";
            this.tstBtnExport.Click += new System.EventHandler(this.tstBtnExport_Click);
            // 
            // tstBtnPrint
            // 
            this.tstBtnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tstBtnPrint, "tstBtnPrint");
            this.tstBtnPrint.Name = "tstBtnPrint";
            this.tstBtnPrint.Click += new System.EventHandler(this.tstBtnPrint_Click);
            // 
            // tstBtnRefresh
            // 
            this.tstBtnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tstBtnRefresh.Image = global::PannelloCharger.Properties.Resources.reset;
            resources.ApplyResources(this.tstBtnRefresh, "tstBtnRefresh");
            this.tstBtnRefresh.Name = "tstBtnRefresh";
            this.tstBtnRefresh.Click += new System.EventHandler(this.tstBtnRefresh_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tstBtnClose
            // 
            this.tstBtnClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tstBtnClose.Image = global::PannelloCharger.Properties.Resources.close;
            resources.ApplyResources(this.tstBtnClose, "tstBtnClose");
            this.tstBtnClose.Name = "tstBtnClose";
            this.tstBtnClose.Click += new System.EventHandler(this.tstBtnClose_Click);
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.tstPulsanti);
            this.Controls.Add(this.sstMain);
            this.Controls.Add(this.mnuMain);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmMain";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.Controls.SetChildIndex(this.mnuMain, 0);
            this.Controls.SetChildIndex(this.sstMain, 0);
            this.Controls.SetChildIndex(this.tstPulsanti, 0);
            this.sstMain.ResumeLayout(false);
            this.sstMain.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.tstPulsanti.ResumeLayout(false);
            this.tstPulsanti.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip sstMain;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuEsci;
        private System.Windows.Forms.ToolStripMenuItem mnuCaricabatteria;
        private System.Windows.Forms.ToolStripMenuItem mnuConnettiCb;
        private System.Windows.Forms.ToolStripMenuItem apparatiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem orologioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuImpostazioni;
        private System.Windows.Forms.ToolStripMenuItem mnuPortaSeriale;
        private System.Windows.Forms.ToolStripMenuItem mnuConsole;
        private System.Windows.Forms.ToolStripMenuItem mnuSelezionePorta;
        private System.Windows.Forms.ToolStripMenuItem mnuSpybat;
        private System.Windows.Forms.ToolStripMenuItem caricaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem archivioToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuCercaDispositiviCOM;
        private System.Windows.Forms.ToolStripProgressBar sstProgressBase;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem importaFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuCercaDispositiviUSB;
        private System.Windows.Forms.ToolStrip tstPulsanti;
        protected System.Windows.Forms.ToolStripButton tstBtnCercaUsb;
        private System.Windows.Forms.ToolStripButton tstBtnCercaRS232;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuInformazioniSu;
        private System.Windows.Forms.ToolStripMenuItem impostaStampanteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuStampa;
        private System.Windows.Forms.ToolStripMenuItem mnuLogout;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem alimentatoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuServizi;
        private System.Windows.Forms.ToolStripMenuItem flashFTDIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flashSPYBATTToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tstBtnRefresh;
        private System.Windows.Forms.ToolStripMenuItem linguaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem italianoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tstBtnClose;
        private System.Drawing.Printing.PrintDocument pdoStampaForm;
        private System.Windows.Forms.ToolStripButton tstBtnPrint;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tstBtnExport;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem programmazioniAvanzateToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem mnuLLDisplayManager;
    }
}
