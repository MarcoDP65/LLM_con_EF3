namespace PannelloCharger
{
    partial class frmListaCicliBreve
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmListaCicliBreve));
            this.lvwCicliBrevi = new System.Windows.Forms.ListView();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.sfdNuovoCSV = new System.Windows.Forms.SaveFileDialog();
            this.tabCicloBreve = new System.Windows.Forms.TabControl();
            this.tbpAndamentoOxy = new System.Windows.Forms.TabPage();
            this.tbpTensioniCiclo = new System.Windows.Forms.TabPage();
            this.tbpDatiCiclo = new System.Windows.Forms.TabPage();
            this.grbProgrammazione = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNumProgramma = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCapacitaNominale = new System.Windows.Forms.TextBox();
            this.txtCelleTot = new System.Windows.Forms.TextBox();
            this.txtCelleV3 = new System.Windows.Forms.TextBox();
            this.txtCelleV2 = new System.Windows.Forms.TextBox();
            this.txtCelleV1 = new System.Windows.Forms.TextBox();
            this.lblCelleP1 = new System.Windows.Forms.Label();
            this.txtTensioneNominale = new System.Windows.Forms.TextBox();
            this.txtTensioneNom = new System.Windows.Forms.Label();
            this.grbContatori = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFineEvento = new System.Windows.Forms.TextBox();
            this.txtInizioEvento = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtIdEventoLungo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.flvwCicliBrevi = new BrightIdeasSoftware.FastObjectListView();
            this.tbpUtilita = new System.Windows.Forms.TabPage();
            this.chkIntervalloRelativo = new System.Windows.Forms.CheckBox();
            this.grbGeneraExcel = new System.Windows.Forms.GroupBox();
            this.btnGeneraCsv = new System.Windows.Forms.Button();
            this.btnSfoglia = new System.Windows.Forms.Button();
            this.txtNuovoFile = new System.Windows.Forms.TextBox();
            this.tabCicloBreve.SuspendLayout();
            this.tbpDatiCiclo.SuspendLayout();
            this.grbProgrammazione.SuspendLayout();
            this.grbContatori.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvwCicliBrevi)).BeginInit();
            this.tbpUtilita.SuspendLayout();
            this.grbGeneraExcel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvwCicliBrevi
            // 
            resources.ApplyResources(this.lvwCicliBrevi, "lvwCicliBrevi");
            this.lvwCicliBrevi.Name = "lvwCicliBrevi";
            this.lvwCicliBrevi.UseCompatibleStateImageBehavior = false;
            // 
            // btnChiudi
            // 
            resources.ApplyResources(this.btnChiudi, "btnChiudi");
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // tabCicloBreve
            // 
            this.tabCicloBreve.Controls.Add(this.tbpAndamentoOxy);
            this.tabCicloBreve.Controls.Add(this.tbpTensioniCiclo);
            this.tabCicloBreve.Controls.Add(this.tbpDatiCiclo);
            this.tabCicloBreve.Controls.Add(this.tbpUtilita);
            resources.ApplyResources(this.tabCicloBreve, "tabCicloBreve");
            this.tabCicloBreve.Name = "tabCicloBreve";
            this.tabCicloBreve.SelectedIndex = 0;
            // 
            // tbpAndamentoOxy
            // 
            resources.ApplyResources(this.tbpAndamentoOxy, "tbpAndamentoOxy");
            this.tbpAndamentoOxy.Name = "tbpAndamentoOxy";
            this.tbpAndamentoOxy.UseVisualStyleBackColor = true;
            this.tbpAndamentoOxy.Click += new System.EventHandler(this.tbpAndamentoOxy_Click);
            // 
            // tbpTensioniCiclo
            // 
            resources.ApplyResources(this.tbpTensioniCiclo, "tbpTensioniCiclo");
            this.tbpTensioniCiclo.Name = "tbpTensioniCiclo";
            this.tbpTensioniCiclo.UseVisualStyleBackColor = true;
            // 
            // tbpDatiCiclo
            // 
            this.tbpDatiCiclo.BackColor = System.Drawing.Color.LightYellow;
            this.tbpDatiCiclo.Controls.Add(this.grbProgrammazione);
            this.tbpDatiCiclo.Controls.Add(this.grbContatori);
            this.tbpDatiCiclo.Controls.Add(this.flvwCicliBrevi);
            resources.ApplyResources(this.tbpDatiCiclo, "tbpDatiCiclo");
            this.tbpDatiCiclo.Name = "tbpDatiCiclo";
            // 
            // grbProgrammazione
            // 
            this.grbProgrammazione.BackColor = System.Drawing.Color.White;
            this.grbProgrammazione.Controls.Add(this.label8);
            this.grbProgrammazione.Controls.Add(this.txtNumProgramma);
            this.grbProgrammazione.Controls.Add(this.label3);
            this.grbProgrammazione.Controls.Add(this.label2);
            this.grbProgrammazione.Controls.Add(this.label1);
            this.grbProgrammazione.Controls.Add(this.label7);
            this.grbProgrammazione.Controls.Add(this.txtCapacitaNominale);
            this.grbProgrammazione.Controls.Add(this.txtCelleTot);
            this.grbProgrammazione.Controls.Add(this.txtCelleV3);
            this.grbProgrammazione.Controls.Add(this.txtCelleV2);
            this.grbProgrammazione.Controls.Add(this.txtCelleV1);
            this.grbProgrammazione.Controls.Add(this.lblCelleP1);
            this.grbProgrammazione.Controls.Add(this.txtTensioneNominale);
            this.grbProgrammazione.Controls.Add(this.txtTensioneNom);
            resources.ApplyResources(this.grbProgrammazione, "grbProgrammazione");
            this.grbProgrammazione.Name = "grbProgrammazione";
            this.grbProgrammazione.TabStop = false;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // txtNumProgramma
            // 
            resources.ApplyResources(this.txtNumProgramma, "txtNumProgramma");
            this.txtNumProgramma.Name = "txtNumProgramma";
            this.txtNumProgramma.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // txtCapacitaNominale
            // 
            resources.ApplyResources(this.txtCapacitaNominale, "txtCapacitaNominale");
            this.txtCapacitaNominale.Name = "txtCapacitaNominale";
            this.txtCapacitaNominale.ReadOnly = true;
            // 
            // txtCelleTot
            // 
            resources.ApplyResources(this.txtCelleTot, "txtCelleTot");
            this.txtCelleTot.Name = "txtCelleTot";
            this.txtCelleTot.ReadOnly = true;
            // 
            // txtCelleV3
            // 
            resources.ApplyResources(this.txtCelleV3, "txtCelleV3");
            this.txtCelleV3.Name = "txtCelleV3";
            this.txtCelleV3.ReadOnly = true;
            // 
            // txtCelleV2
            // 
            resources.ApplyResources(this.txtCelleV2, "txtCelleV2");
            this.txtCelleV2.Name = "txtCelleV2";
            this.txtCelleV2.ReadOnly = true;
            // 
            // txtCelleV1
            // 
            resources.ApplyResources(this.txtCelleV1, "txtCelleV1");
            this.txtCelleV1.Name = "txtCelleV1";
            this.txtCelleV1.ReadOnly = true;
            // 
            // lblCelleP1
            // 
            resources.ApplyResources(this.lblCelleP1, "lblCelleP1");
            this.lblCelleP1.Name = "lblCelleP1";
            // 
            // txtTensioneNominale
            // 
            resources.ApplyResources(this.txtTensioneNominale, "txtTensioneNominale");
            this.txtTensioneNominale.Name = "txtTensioneNominale";
            this.txtTensioneNominale.ReadOnly = true;
            // 
            // txtTensioneNom
            // 
            resources.ApplyResources(this.txtTensioneNom, "txtTensioneNom");
            this.txtTensioneNom.Name = "txtTensioneNom";
            // 
            // grbContatori
            // 
            this.grbContatori.BackColor = System.Drawing.Color.White;
            this.grbContatori.Controls.Add(this.label6);
            this.grbContatori.Controls.Add(this.txtFineEvento);
            this.grbContatori.Controls.Add(this.txtInizioEvento);
            this.grbContatori.Controls.Add(this.label4);
            this.grbContatori.Controls.Add(this.txtIdEventoLungo);
            this.grbContatori.Controls.Add(this.label5);
            resources.ApplyResources(this.grbContatori, "grbContatori");
            this.grbContatori.Name = "grbContatori";
            this.grbContatori.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // txtFineEvento
            // 
            resources.ApplyResources(this.txtFineEvento, "txtFineEvento");
            this.txtFineEvento.Name = "txtFineEvento";
            this.txtFineEvento.ReadOnly = true;
            // 
            // txtInizioEvento
            // 
            resources.ApplyResources(this.txtInizioEvento, "txtInizioEvento");
            this.txtInizioEvento.Name = "txtInizioEvento";
            this.txtInizioEvento.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txtIdEventoLungo
            // 
            resources.ApplyResources(this.txtIdEventoLungo, "txtIdEventoLungo");
            this.txtIdEventoLungo.Name = "txtIdEventoLungo";
            this.txtIdEventoLungo.ReadOnly = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // flvwCicliBrevi
            // 
            this.flvwCicliBrevi.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
            this.flvwCicliBrevi.CellEditUseWholeCell = false;
            this.flvwCicliBrevi.FullRowSelect = true;
            this.flvwCicliBrevi.HighlightBackgroundColor = System.Drawing.Color.Empty;
            this.flvwCicliBrevi.HighlightForegroundColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.flvwCicliBrevi, "flvwCicliBrevi");
            this.flvwCicliBrevi.Name = "flvwCicliBrevi";
            this.flvwCicliBrevi.ShowGroups = false;
            this.flvwCicliBrevi.UseAlternatingBackColors = true;
            this.flvwCicliBrevi.UseCellFormatEvents = true;
            this.flvwCicliBrevi.UseCompatibleStateImageBehavior = false;
            this.flvwCicliBrevi.View = System.Windows.Forms.View.Details;
            this.flvwCicliBrevi.VirtualMode = true;
            // 
            // tbpUtilita
            // 
            this.tbpUtilita.BackColor = System.Drawing.Color.LightYellow;
            this.tbpUtilita.Controls.Add(this.chkIntervalloRelativo);
            this.tbpUtilita.Controls.Add(this.grbGeneraExcel);
            resources.ApplyResources(this.tbpUtilita, "tbpUtilita");
            this.tbpUtilita.Name = "tbpUtilita";
            // 
            // chkIntervalloRelativo
            // 
            resources.ApplyResources(this.chkIntervalloRelativo, "chkIntervalloRelativo");
            this.chkIntervalloRelativo.Checked = true;
            this.chkIntervalloRelativo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIntervalloRelativo.Name = "chkIntervalloRelativo";
            this.chkIntervalloRelativo.UseVisualStyleBackColor = true;
            this.chkIntervalloRelativo.CheckedChanged += new System.EventHandler(this.chkIntervalloRelativo_CheckedChanged);
            // 
            // grbGeneraExcel
            // 
            this.grbGeneraExcel.BackColor = System.Drawing.Color.White;
            this.grbGeneraExcel.Controls.Add(this.btnGeneraCsv);
            this.grbGeneraExcel.Controls.Add(this.btnSfoglia);
            this.grbGeneraExcel.Controls.Add(this.txtNuovoFile);
            resources.ApplyResources(this.grbGeneraExcel, "grbGeneraExcel");
            this.grbGeneraExcel.Name = "grbGeneraExcel";
            this.grbGeneraExcel.TabStop = false;
            // 
            // btnGeneraCsv
            // 
            resources.ApplyResources(this.btnGeneraCsv, "btnGeneraCsv");
            this.btnGeneraCsv.Name = "btnGeneraCsv";
            this.btnGeneraCsv.UseVisualStyleBackColor = true;
            this.btnGeneraCsv.Click += new System.EventHandler(this.btnGeneraCsv_Click);
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
            this.txtNuovoFile.TextChanged += new System.EventHandler(this.txtNuovoFile_TextChanged_1);
            // 
            // frmListaCicliBreve
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tabCicloBreve);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.lvwCicliBrevi);
            this.Name = "frmListaCicliBreve";
            this.Load += new System.EventHandler(this.frmListaCicliBreve_Load);
            this.Resize += new System.EventHandler(this.frmListaCicliBreve_Resize);
            this.tabCicloBreve.ResumeLayout(false);
            this.tbpDatiCiclo.ResumeLayout(false);
            this.grbProgrammazione.ResumeLayout(false);
            this.grbProgrammazione.PerformLayout();
            this.grbContatori.ResumeLayout(false);
            this.grbContatori.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.flvwCicliBrevi)).EndInit();
            this.tbpUtilita.ResumeLayout(false);
            this.tbpUtilita.PerformLayout();
            this.grbGeneraExcel.ResumeLayout(false);
            this.grbGeneraExcel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvwCicliBrevi;
        private System.Windows.Forms.Button btnChiudi;
        private System.Windows.Forms.SaveFileDialog sfdNuovoCSV;
        private System.Windows.Forms.TabControl tabCicloBreve;
        private System.Windows.Forms.TabPage tbpDatiCiclo;
        private BrightIdeasSoftware.FastObjectListView flvwCicliBrevi;
        private System.Windows.Forms.TabPage tbpTensioniCiclo;
        private System.Windows.Forms.TabPage tbpUtilita;
        private System.Windows.Forms.GroupBox grbGeneraExcel;
        public System.Windows.Forms.Button btnGeneraCsv;
        private System.Windows.Forms.Button btnSfoglia;
        private System.Windows.Forms.TextBox txtNuovoFile;
        private System.Windows.Forms.GroupBox grbProgrammazione;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNumProgramma;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCapacitaNominale;
        private System.Windows.Forms.TextBox txtCelleTot;
        private System.Windows.Forms.TextBox txtCelleV3;
        private System.Windows.Forms.TextBox txtCelleV2;
        private System.Windows.Forms.TextBox txtCelleV1;
        private System.Windows.Forms.Label lblCelleP1;
        private System.Windows.Forms.TextBox txtTensioneNominale;
        private System.Windows.Forms.Label txtTensioneNom;
        private System.Windows.Forms.GroupBox grbContatori;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFineEvento;
        private System.Windows.Forms.TextBox txtInizioEvento;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtIdEventoLungo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkIntervalloRelativo;
        private System.Windows.Forms.TabPage tbpAndamentoOxy;
    }
}