﻿namespace PannelloCharger
{
    partial class frmDefinizioneImpianto
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
            this.components = new System.ComponentModel.Container();
            this.tlvStrutturaImpianto = new BrightIdeasSoftware.TreeListView();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.btnCaricaOrfani = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tlvStrutturaImpianto)).BeginInit();
            this.SuspendLayout();
            // 
            // tlvStrutturaImpianto
            // 
            this.tlvStrutturaImpianto.CellEditUseWholeCell = false;
            this.tlvStrutturaImpianto.FullRowSelect = true;
            this.tlvStrutturaImpianto.IsSimpleDragSource = true;
            this.tlvStrutturaImpianto.IsSimpleDropSink = true;
            this.tlvStrutturaImpianto.Location = new System.Drawing.Point(23, 26);
            this.tlvStrutturaImpianto.MultiSelect = false;
            this.tlvStrutturaImpianto.Name = "tlvStrutturaImpianto";
            this.tlvStrutturaImpianto.ShowGroups = false;
            this.tlvStrutturaImpianto.Size = new System.Drawing.Size(1086, 447);
            this.tlvStrutturaImpianto.TabIndex = 1;
            this.tlvStrutturaImpianto.UseCompatibleStateImageBehavior = false;
            this.tlvStrutturaImpianto.View = System.Windows.Forms.View.Details;
            this.tlvStrutturaImpianto.VirtualMode = true;
            this.tlvStrutturaImpianto.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.tlvStrutturaImpianto_CellRightClick);
            this.tlvStrutturaImpianto.Dropped += new System.EventHandler<BrightIdeasSoftware.OlvDropEventArgs>(this.tlvStrutturaImpianto_Dropped);
            this.tlvStrutturaImpianto.ModelCanDrop += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.tlvStrutturaImpianto_ModelCanDrop);
            this.tlvStrutturaImpianto.ModelDropped += new System.EventHandler<BrightIdeasSoftware.ModelDropEventArgs>(this.tlvStrutturaImpianto_ModelDropped);
            this.tlvStrutturaImpianto.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tlvStrutturaImpianto_MouseDoubleClick);
            // 
            // btnChiudi
            // 
            this.btnChiudi.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChiudi.Location = new System.Drawing.Point(984, 505);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(125, 44);
            this.btnChiudi.TabIndex = 3;
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // btnCaricaOrfani
            // 
            this.btnCaricaOrfani.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCaricaOrfani.Location = new System.Drawing.Point(23, 505);
            this.btnCaricaOrfani.Name = "btnCaricaOrfani";
            this.btnCaricaOrfani.Size = new System.Drawing.Size(125, 44);
            this.btnCaricaOrfani.TabIndex = 4;
            this.btnCaricaOrfani.Text = "Carica Orfani";
            this.btnCaricaOrfani.UseVisualStyleBackColor = true;
            this.btnCaricaOrfani.Click += new System.EventHandler(this.btnCaricaOrfani_Click);
            // 
            // frmDefinizioneImpianto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1144, 578);
            this.Controls.Add(this.btnCaricaOrfani);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.tlvStrutturaImpianto);
            this.Name = "frmDefinizioneImpianto";
            this.Text = "Definizione Impianti";
            this.Load += new System.EventHandler(this.frmDefinizioneImpianto_Load);
            this.Resize += new System.EventHandler(this.frmDefinizioneImpianto_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.tlvStrutturaImpianto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.TreeListView tlvStrutturaImpianto;
        private System.Windows.Forms.Button btnChiudi;
        private System.Windows.Forms.Button btnCaricaOrfani;
    }
}