namespace PannelloCharger
{
    partial class frmSelettoreSpyBatt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelettoreSpyBatt));
            this.btnApriSpybatt = new System.Windows.Forms.Button();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.flvwListaApparati = new BrightIdeasSoftware.FastObjectListView();
            this.btnEsportaSpybatt = new System.Windows.Forms.Button();
            this.btnEliminaDati = new System.Windows.Forms.Button();
            this.btnImportaDati = new System.Windows.Forms.Button();
            this.txtIdScheda = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaApparati)).BeginInit();
            this.SuspendLayout();
            // 
            // btnApriSpybatt
            // 
            resources.ApplyResources(this.btnApriSpybatt, "btnApriSpybatt");
            this.btnApriSpybatt.Name = "btnApriSpybatt";
            this.btnApriSpybatt.UseVisualStyleBackColor = true;
            this.btnApriSpybatt.Click += new System.EventHandler(this.btnApriSpybatt_Click);
            // 
            // btnChiudi
            // 
            resources.ApplyResources(this.btnChiudi, "btnChiudi");
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // flvwListaApparati
            // 
            this.flvwListaApparati.CellEditUseWholeCell = false;
            this.flvwListaApparati.SelectedBackColor = System.Drawing.Color.Empty;
            this.flvwListaApparati.SelectedBackColor = System.Drawing.Color.Empty;
            resources.ApplyResources(this.flvwListaApparati, "flvwListaApparati");
            this.flvwListaApparati.Name = "flvwListaApparati";
            this.flvwListaApparati.ShowGroups = false;
            this.flvwListaApparati.UseCompatibleStateImageBehavior = false;
            this.flvwListaApparati.View = System.Windows.Forms.View.Details;
            this.flvwListaApparati.VirtualMode = true;
            this.flvwListaApparati.SelectedIndexChanged += new System.EventHandler(this.flvwListaApparati_SelectedIndexChanged);
            this.flvwListaApparati.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.flvwListaApparati_MouseDoubleClick);
            // 
            // btnEsportaSpybatt
            // 
            resources.ApplyResources(this.btnEsportaSpybatt, "btnEsportaSpybatt");
            this.btnEsportaSpybatt.Name = "btnEsportaSpybatt";
            this.btnEsportaSpybatt.UseVisualStyleBackColor = true;
            this.btnEsportaSpybatt.Click += new System.EventHandler(this.btnEsportaSpybatt_Click);
            // 
            // btnEliminaDati
            // 
            resources.ApplyResources(this.btnEliminaDati, "btnEliminaDati");
            this.btnEliminaDati.Name = "btnEliminaDati";
            this.btnEliminaDati.UseVisualStyleBackColor = true;
            this.btnEliminaDati.Click += new System.EventHandler(this.btnEliminaDati_Click);
            // 
            // btnImportaDati
            // 
            resources.ApplyResources(this.btnImportaDati, "btnImportaDati");
            this.btnImportaDati.Name = "btnImportaDati";
            this.btnImportaDati.UseVisualStyleBackColor = true;
            this.btnImportaDati.Click += new System.EventHandler(this.btnImportaDati_Click);
            // 
            // txtIdScheda
            // 
            resources.ApplyResources(this.txtIdScheda, "txtIdScheda");
            this.txtIdScheda.Name = "txtIdScheda";
            this.txtIdScheda.ReadOnly = true;
            // 
            // frmSelettoreSpyBatt
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtIdScheda);
            this.Controls.Add(this.btnImportaDati);
            this.Controls.Add(this.btnEliminaDati);
            this.Controls.Add(this.btnEsportaSpybatt);
            this.Controls.Add(this.flvwListaApparati);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.btnApriSpybatt);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelettoreSpyBatt";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frmSelettoreSpyBatt_Load);
            this.Shown += new System.EventHandler(this.frmSelettoreSpyBatt_Shown);
            this.Resize += new System.EventHandler(this.frmSelettoreSpyBatt_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.flvwListaApparati)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnApriSpybatt;
        private System.Windows.Forms.Button btnChiudi;
        private BrightIdeasSoftware.FastObjectListView flvwListaApparati;
        private System.Windows.Forms.Button btnEsportaSpybatt;
        private System.Windows.Forms.Button btnEliminaDati;
        private System.Windows.Forms.Button btnImportaDati;
        private System.Windows.Forms.TextBox txtIdScheda;
    }
}