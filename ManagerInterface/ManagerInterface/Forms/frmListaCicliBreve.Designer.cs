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
            this.lvwCicliBrevi.Location = new System.Drawing.Point(73, 190);
            this.lvwCicliBrevi.Name = "lvwCicliBrevi";
            this.lvwCicliBrevi.Size = new System.Drawing.Size(177, 241);
            this.lvwCicliBrevi.TabIndex = 0;
            this.lvwCicliBrevi.UseCompatibleStateImageBehavior = false;
            this.lvwCicliBrevi.Visible = false;
            // 
            // btnChiudi
            // 
            this.btnChiudi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChiudi.Location = new System.Drawing.Point(1082, 653);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(170, 37);
            this.btnChiudi.TabIndex = 2;
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // tabCicloBreve
            // 
            this.tabCicloBreve.Controls.Add(this.tbpAndamentoOxy);
            this.tabCicloBreve.Controls.Add(this.tbpTensioniCiclo);
            this.tabCicloBreve.Controls.Add(this.tbpDatiCiclo);
            this.tabCicloBreve.Controls.Add(this.tbpUtilita);
            this.tabCicloBreve.Location = new System.Drawing.Point(20, 20);
            this.tabCicloBreve.Name = "tabCicloBreve";
            this.tabCicloBreve.SelectedIndex = 0;
            this.tabCicloBreve.Size = new System.Drawing.Size(1228, 595);
            this.tabCicloBreve.TabIndex = 10;
            // 
            // tbpAndamentoOxy
            // 
            this.tbpAndamentoOxy.Location = new System.Drawing.Point(4, 25);
            this.tbpAndamentoOxy.Name = "tbpAndamentoOxy";
            this.tbpAndamentoOxy.Size = new System.Drawing.Size(1220, 566);
            this.tbpAndamentoOxy.TabIndex = 4;
            this.tbpAndamentoOxy.Text = "Grafico Ciclo";
            this.tbpAndamentoOxy.UseVisualStyleBackColor = true;
            this.tbpAndamentoOxy.Click += new System.EventHandler(this.tbpAndamentoOxy_Click);
            // 
            // tbpTensioniCiclo
            // 
            this.tbpTensioniCiclo.Location = new System.Drawing.Point(4, 25);
            this.tbpTensioniCiclo.Name = "tbpTensioniCiclo";
            this.tbpTensioniCiclo.Size = new System.Drawing.Size(1220, 566);
            this.tbpTensioniCiclo.TabIndex = 2;
            this.tbpTensioniCiclo.Text = "Tensioni Ciclo";
            this.tbpTensioniCiclo.UseVisualStyleBackColor = true;
            // 
            // tbpDatiCiclo
            // 
            this.tbpDatiCiclo.BackColor = System.Drawing.Color.LightYellow;
            this.tbpDatiCiclo.Controls.Add(this.grbProgrammazione);
            this.tbpDatiCiclo.Controls.Add(this.grbContatori);
            this.tbpDatiCiclo.Controls.Add(this.flvwCicliBrevi);
            this.tbpDatiCiclo.Location = new System.Drawing.Point(4, 25);
            this.tbpDatiCiclo.Name = "tbpDatiCiclo";
            this.tbpDatiCiclo.Padding = new System.Windows.Forms.Padding(3);
            this.tbpDatiCiclo.Size = new System.Drawing.Size(1220, 566);
            this.tbpDatiCiclo.TabIndex = 0;
            this.tbpDatiCiclo.Text = "Dati";
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
            this.grbProgrammazione.Location = new System.Drawing.Point(498, 20);
            this.grbProgrammazione.Name = "grbProgrammazione";
            this.grbProgrammazione.Size = new System.Drawing.Size(621, 93);
            this.grbProgrammazione.TabIndex = 11;
            this.grbProgrammazione.TabStop = false;
            this.grbProgrammazione.Text = "Programmazione";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 17);
            this.label8.TabIndex = 16;
            this.label8.Text = "Num";
            // 
            // txtNumProgramma
            // 
            this.txtNumProgramma.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNumProgramma.Location = new System.Drawing.Point(21, 43);
            this.txtNumProgramma.Name = "txtNumProgramma";
            this.txtNumProgramma.ReadOnly = true;
            this.txtNumProgramma.Size = new System.Drawing.Size(54, 24);
            this.txtNumProgramma.TabIndex = 15;
            this.txtNumProgramma.Text = "0";
            this.txtNumProgramma.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(344, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 14;
            this.label3.Text = "Celle Tot";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(407, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Celle V3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(470, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 12;
            this.label1.Text = "Celle V2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(197, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(98, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Capacità nom.";
            // 
            // txtCapacitaNominale
            // 
            this.txtCapacitaNominale.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCapacitaNominale.Location = new System.Drawing.Point(196, 43);
            this.txtCapacitaNominale.Name = "txtCapacitaNominale";
            this.txtCapacitaNominale.ReadOnly = true;
            this.txtCapacitaNominale.Size = new System.Drawing.Size(99, 24);
            this.txtCapacitaNominale.TabIndex = 10;
            this.txtCapacitaNominale.Text = "0";
            this.txtCapacitaNominale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCelleTot
            // 
            this.txtCelleTot.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCelleTot.Location = new System.Drawing.Point(347, 43);
            this.txtCelleTot.Name = "txtCelleTot";
            this.txtCelleTot.ReadOnly = true;
            this.txtCelleTot.Size = new System.Drawing.Size(57, 24);
            this.txtCelleTot.TabIndex = 9;
            this.txtCelleTot.Text = "0";
            this.txtCelleTot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCelleV3
            // 
            this.txtCelleV3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCelleV3.Location = new System.Drawing.Point(410, 43);
            this.txtCelleV3.Name = "txtCelleV3";
            this.txtCelleV3.ReadOnly = true;
            this.txtCelleV3.Size = new System.Drawing.Size(57, 24);
            this.txtCelleV3.TabIndex = 8;
            this.txtCelleV3.Text = "0";
            this.txtCelleV3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCelleV2
            // 
            this.txtCelleV2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCelleV2.Location = new System.Drawing.Point(473, 43);
            this.txtCelleV2.Name = "txtCelleV2";
            this.txtCelleV2.ReadOnly = true;
            this.txtCelleV2.Size = new System.Drawing.Size(57, 24);
            this.txtCelleV2.TabIndex = 7;
            this.txtCelleV2.Text = "0";
            this.txtCelleV2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCelleV1
            // 
            this.txtCelleV1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCelleV1.Location = new System.Drawing.Point(536, 43);
            this.txtCelleV1.Name = "txtCelleV1";
            this.txtCelleV1.ReadOnly = true;
            this.txtCelleV1.Size = new System.Drawing.Size(57, 24);
            this.txtCelleV1.TabIndex = 6;
            this.txtCelleV1.Text = "0";
            this.txtCelleV1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblCelleP1
            // 
            this.lblCelleP1.AutoSize = true;
            this.lblCelleP1.Location = new System.Drawing.Point(536, 23);
            this.lblCelleP1.Name = "lblCelleP1";
            this.lblCelleP1.Size = new System.Drawing.Size(60, 17);
            this.lblCelleP1.TabIndex = 5;
            this.lblCelleP1.Text = "Celle V1";
            // 
            // txtTensioneNominale
            // 
            this.txtTensioneNominale.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTensioneNominale.Location = new System.Drawing.Point(91, 43);
            this.txtTensioneNominale.Name = "txtTensioneNominale";
            this.txtTensioneNominale.ReadOnly = true;
            this.txtTensioneNominale.Size = new System.Drawing.Size(99, 24);
            this.txtTensioneNominale.TabIndex = 4;
            this.txtTensioneNominale.Text = "0";
            this.txtTensioneNominale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtTensioneNom
            // 
            this.txtTensioneNom.AutoSize = true;
            this.txtTensioneNom.Location = new System.Drawing.Point(89, 23);
            this.txtTensioneNom.Name = "txtTensioneNom";
            this.txtTensioneNom.Size = new System.Drawing.Size(102, 17);
            this.txtTensioneNom.TabIndex = 3;
            this.txtTensioneNom.Text = "Tensione nom.";
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
            this.grbContatori.Location = new System.Drawing.Point(20, 20);
            this.grbContatori.Name = "grbContatori";
            this.grbContatori.Size = new System.Drawing.Size(454, 93);
            this.grbContatori.TabIndex = 10;
            this.grbContatori.TabStop = false;
            this.grbContatori.Text = "Contatori";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(292, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "Fine";
            // 
            // txtFineEvento
            // 
            this.txtFineEvento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFineEvento.Location = new System.Drawing.Point(295, 43);
            this.txtFineEvento.Name = "txtFineEvento";
            this.txtFineEvento.ReadOnly = true;
            this.txtFineEvento.Size = new System.Drawing.Size(129, 24);
            this.txtFineEvento.TabIndex = 7;
            this.txtFineEvento.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtInizioEvento
            // 
            this.txtInizioEvento.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInizioEvento.Location = new System.Drawing.Point(160, 43);
            this.txtInizioEvento.Name = "txtInizioEvento";
            this.txtInizioEvento.ReadOnly = true;
            this.txtInizioEvento.Size = new System.Drawing.Size(129, 24);
            this.txtInizioEvento.TabIndex = 6;
            this.txtInizioEvento.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(157, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Inizio";
            // 
            // txtIdEventoLungo
            // 
            this.txtIdEventoLungo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIdEventoLungo.Location = new System.Drawing.Point(32, 43);
            this.txtIdEventoLungo.Name = "txtIdEventoLungo";
            this.txtIdEventoLungo.ReadOnly = true;
            this.txtIdEventoLungo.Size = new System.Drawing.Size(108, 24);
            this.txtIdEventoLungo.TabIndex = 4;
            this.txtIdEventoLungo.Text = "0";
            this.txtIdEventoLungo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Id Evento";
            // 
            // flvwCicliBrevi
            // 
            this.flvwCicliBrevi.AlternateRowBackColor = System.Drawing.Color.WhiteSmoke;
            this.flvwCicliBrevi.FullRowSelect = true;
            this.flvwCicliBrevi.Location = new System.Drawing.Point(20, 137);
            this.flvwCicliBrevi.Name = "flvwCicliBrevi";
            this.flvwCicliBrevi.ShowGroups = false;
            this.flvwCicliBrevi.Size = new System.Drawing.Size(1163, 390);
            this.flvwCicliBrevi.TabIndex = 9;
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
            this.tbpUtilita.Location = new System.Drawing.Point(4, 25);
            this.tbpUtilita.Name = "tbpUtilita";
            this.tbpUtilita.Size = new System.Drawing.Size(1220, 566);
            this.tbpUtilita.TabIndex = 3;
            this.tbpUtilita.Text = "Utilità";
            // 
            // chkIntervalloRelativo
            // 
            this.chkIntervalloRelativo.AutoSize = true;
            this.chkIntervalloRelativo.Checked = true;
            this.chkIntervalloRelativo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIntervalloRelativo.Location = new System.Drawing.Point(26, 45);
            this.chkIntervalloRelativo.Name = "chkIntervalloRelativo";
            this.chkIntervalloRelativo.Size = new System.Drawing.Size(282, 21);
            this.chkIntervalloRelativo.TabIndex = 11;
            this.chkIntervalloRelativo.Text = "Intervallo Temporale Relativo nei Grafici";
            this.chkIntervalloRelativo.UseVisualStyleBackColor = true;
            this.chkIntervalloRelativo.CheckedChanged += new System.EventHandler(this.chkIntervalloRelativo_CheckedChanged);
            // 
            // grbGeneraExcel
            // 
            this.grbGeneraExcel.BackColor = System.Drawing.Color.White;
            this.grbGeneraExcel.Controls.Add(this.btnGeneraCsv);
            this.grbGeneraExcel.Controls.Add(this.btnSfoglia);
            this.grbGeneraExcel.Controls.Add(this.txtNuovoFile);
            this.grbGeneraExcel.Location = new System.Drawing.Point(26, 100);
            this.grbGeneraExcel.Name = "grbGeneraExcel";
            this.grbGeneraExcel.Size = new System.Drawing.Size(666, 74);
            this.grbGeneraExcel.TabIndex = 10;
            this.grbGeneraExcel.TabStop = false;
            this.grbGeneraExcel.Text = "Espota in Excel";
            // 
            // btnGeneraCsv
            // 
            this.btnGeneraCsv.Location = new System.Drawing.Point(527, 30);
            this.btnGeneraCsv.Name = "btnGeneraCsv";
            this.btnGeneraCsv.Size = new System.Drawing.Size(118, 28);
            this.btnGeneraCsv.TabIndex = 7;
            this.btnGeneraCsv.Text = "Genera File";
            this.btnGeneraCsv.UseVisualStyleBackColor = true;
            this.btnGeneraCsv.Click += new System.EventHandler(this.btnGeneraCsv_Click);
            // 
            // btnSfoglia
            // 
            this.btnSfoglia.Location = new System.Drawing.Point(488, 29);
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
            this.txtNuovoFile.Location = new System.Drawing.Point(32, 31);
            this.txtNuovoFile.Name = "txtNuovoFile";
            this.txtNuovoFile.Size = new System.Drawing.Size(450, 24);
            this.txtNuovoFile.TabIndex = 4;
            this.txtNuovoFile.TextChanged += new System.EventHandler(this.txtNuovoFile_TextChanged_1);
            // 
            // frmListaCicliBreve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1351, 716);
            this.Controls.Add(this.tabCicloBreve);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.lvwCicliBrevi);
            this.Name = "frmListaCicliBreve";
            this.Text = "frmListaCicliBreve";
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