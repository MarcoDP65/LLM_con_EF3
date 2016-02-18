namespace PannelloCharger
{
    partial class frmFlashFTDI
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
            this.grbTargetTemplate = new System.Windows.Forms.GroupBox();
            this.optFTDISBFinto = new System.Windows.Forms.RadioButton();
            this.optFTDILadeLight = new System.Windows.Forms.RadioButton();
            this.optFTDISpybatt = new System.Windows.Forms.RadioButton();
            this.optFTDIGenerico = new System.Windows.Forms.RadioButton();
            this.btnChiudi = new System.Windows.Forms.Button();
            this.btnApplicaTemplate = new System.Windows.Forms.Button();
            this.lvwListaPorte = new System.Windows.Forms.ListView();
            this.txtFtdiSerialId = new System.Windows.Forms.TextBox();
            this.btnUsbReload = new System.Windows.Forms.Button();
            this.lblSelectedSerial = new System.Windows.Forms.Label();
            this.txtEsito = new System.Windows.Forms.TextBox();
            this.btnMostraTemplate = new System.Windows.Forms.Button();
            this.btnResetDevice = new System.Windows.Forms.Button();
            this.optFTDIDesolf = new System.Windows.Forms.RadioButton();
            this.grbTargetTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbTargetTemplate
            // 
            this.grbTargetTemplate.BackColor = System.Drawing.Color.White;
            this.grbTargetTemplate.Controls.Add(this.optFTDIDesolf);
            this.grbTargetTemplate.Controls.Add(this.optFTDISBFinto);
            this.grbTargetTemplate.Controls.Add(this.optFTDILadeLight);
            this.grbTargetTemplate.Controls.Add(this.optFTDISpybatt);
            this.grbTargetTemplate.Controls.Add(this.optFTDIGenerico);
            this.grbTargetTemplate.Location = new System.Drawing.Point(27, 226);
            this.grbTargetTemplate.Name = "grbTargetTemplate";
            this.grbTargetTemplate.Size = new System.Drawing.Size(496, 179);
            this.grbTargetTemplate.TabIndex = 0;
            this.grbTargetTemplate.TabStop = false;
            this.grbTargetTemplate.Text = "Modello Interfaccia";
            // 
            // optFTDISBFinto
            // 
            this.optFTDISBFinto.AutoSize = true;
            this.optFTDISBFinto.ForeColor = System.Drawing.Color.Green;
            this.optFTDISBFinto.Location = new System.Drawing.Point(36, 138);
            this.optFTDISBFinto.Name = "optFTDISBFinto";
            this.optFTDISBFinto.Size = new System.Drawing.Size(213, 21);
            this.optFTDISBFinto.TabIndex = 3;
            this.optFTDISBFinto.Text = "Interfaccia FT201x su HW SB";
            this.optFTDISBFinto.UseVisualStyleBackColor = true;
            // 
            // optFTDILadeLight
            // 
            this.optFTDILadeLight.AutoSize = true;
            this.optFTDILadeLight.Location = new System.Drawing.Point(36, 84);
            this.optFTDILadeLight.Name = "optFTDILadeLight";
            this.optFTDILadeLight.Size = new System.Drawing.Size(163, 21);
            this.optFTDILadeLight.TabIndex = 2;
            this.optFTDILadeLight.Text = "Scheda LADE-LIGHT";
            this.optFTDILadeLight.UseVisualStyleBackColor = true;
            // 
            // optFTDISpybatt
            // 
            this.optFTDISpybatt.AutoSize = true;
            this.optFTDISpybatt.Checked = true;
            this.optFTDISpybatt.Location = new System.Drawing.Point(36, 57);
            this.optFTDISpybatt.Name = "optFTDISpybatt";
            this.optFTDISpybatt.Size = new System.Drawing.Size(149, 21);
            this.optFTDISpybatt.TabIndex = 1;
            this.optFTDISpybatt.TabStop = true;
            this.optFTDISpybatt.Text = "Scheda SPY-BATT";
            this.optFTDISpybatt.UseVisualStyleBackColor = true;
            // 
            // optFTDIGenerico
            // 
            this.optFTDIGenerico.AutoSize = true;
            this.optFTDIGenerico.ForeColor = System.Drawing.Color.Red;
            this.optFTDIGenerico.Location = new System.Drawing.Point(36, 30);
            this.optFTDIGenerico.Name = "optFTDIGenerico";
            this.optFTDIGenerico.Size = new System.Drawing.Size(180, 21);
            this.optFTDIGenerico.TabIndex = 0;
            this.optFTDIGenerico.Text = "Interfaccia base FT201x";
            this.optFTDIGenerico.UseVisualStyleBackColor = true;
            // 
            // btnChiudi
            // 
            this.btnChiudi.Location = new System.Drawing.Point(435, 651);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(105, 38);
            this.btnChiudi.TabIndex = 1;
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            // 
            // btnApplicaTemplate
            // 
            this.btnApplicaTemplate.Location = new System.Drawing.Point(138, 651);
            this.btnApplicaTemplate.Name = "btnApplicaTemplate";
            this.btnApplicaTemplate.Size = new System.Drawing.Size(105, 38);
            this.btnApplicaTemplate.TabIndex = 2;
            this.btnApplicaTemplate.Text = "Applica";
            this.btnApplicaTemplate.UseVisualStyleBackColor = true;
            this.btnApplicaTemplate.Click += new System.EventHandler(this.btnApplicaTemplate_Click);
            // 
            // lvwListaPorte
            // 
            this.lvwListaPorte.FullRowSelect = true;
            this.lvwListaPorte.Location = new System.Drawing.Point(27, 22);
            this.lvwListaPorte.Name = "lvwListaPorte";
            this.lvwListaPorte.Size = new System.Drawing.Size(513, 127);
            this.lvwListaPorte.TabIndex = 3;
            this.lvwListaPorte.UseCompatibleStateImageBehavior = false;
            this.lvwListaPorte.View = System.Windows.Forms.View.List;
            this.lvwListaPorte.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwListaPorte_MouseDoubleClick);
            // 
            // txtFtdiSerialId
            // 
            this.txtFtdiSerialId.Location = new System.Drawing.Point(306, 184);
            this.txtFtdiSerialId.Name = "txtFtdiSerialId";
            this.txtFtdiSerialId.Size = new System.Drawing.Size(234, 22);
            this.txtFtdiSerialId.TabIndex = 4;
            // 
            // btnUsbReload
            // 
            this.btnUsbReload.Location = new System.Drawing.Point(27, 168);
            this.btnUsbReload.Name = "btnUsbReload";
            this.btnUsbReload.Size = new System.Drawing.Size(261, 38);
            this.btnUsbReload.TabIndex = 5;
            this.btnUsbReload.Text = "Ricarica Lista";
            this.btnUsbReload.UseVisualStyleBackColor = true;
            this.btnUsbReload.Click += new System.EventHandler(this.btnUsbReload_Click);
            // 
            // lblSelectedSerial
            // 
            this.lblSelectedSerial.AutoSize = true;
            this.lblSelectedSerial.Location = new System.Drawing.Point(303, 164);
            this.lblSelectedSerial.Name = "lblSelectedSerial";
            this.lblSelectedSerial.Size = new System.Drawing.Size(178, 17);
            this.lblSelectedSerial.TabIndex = 6;
            this.lblSelectedSerial.Text = "Seriale scheda selezionata";
            // 
            // txtEsito
            // 
            this.txtEsito.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEsito.Location = new System.Drawing.Point(27, 437);
            this.txtEsito.Multiline = true;
            this.txtEsito.Name = "txtEsito";
            this.txtEsito.ReadOnly = true;
            this.txtEsito.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEsito.Size = new System.Drawing.Size(513, 188);
            this.txtEsito.TabIndex = 7;
            // 
            // btnMostraTemplate
            // 
            this.btnMostraTemplate.Location = new System.Drawing.Point(27, 651);
            this.btnMostraTemplate.Name = "btnMostraTemplate";
            this.btnMostraTemplate.Size = new System.Drawing.Size(105, 38);
            this.btnMostraTemplate.TabIndex = 8;
            this.btnMostraTemplate.Text = "Dettaglio";
            this.btnMostraTemplate.UseVisualStyleBackColor = true;
            this.btnMostraTemplate.Click += new System.EventHandler(this.btnMostraTemplate_Click);
            // 
            // btnResetDevice
            // 
            this.btnResetDevice.Location = new System.Drawing.Point(249, 651);
            this.btnResetDevice.Name = "btnResetDevice";
            this.btnResetDevice.Size = new System.Drawing.Size(105, 38);
            this.btnResetDevice.TabIndex = 9;
            this.btnResetDevice.Text = "Reset Device";
            this.btnResetDevice.UseVisualStyleBackColor = true;
            this.btnResetDevice.Click += new System.EventHandler(this.btnResetDevice_Click);
            // 
            // optFTDIDesolf
            // 
            this.optFTDIDesolf.AutoSize = true;
            this.optFTDIDesolf.Location = new System.Drawing.Point(36, 111);
            this.optFTDIDesolf.Name = "optFTDIDesolf";
            this.optFTDIDesolf.Size = new System.Drawing.Size(252, 21);
            this.optFTDIDesolf.TabIndex = 4;
            this.optFTDIDesolf.Text = "Scheda Display DESOLFATATORE";
            this.optFTDIDesolf.UseVisualStyleBackColor = true;
            // 
            // frmFlashFTDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(569, 713);
            this.Controls.Add(this.btnResetDevice);
            this.Controls.Add(this.btnMostraTemplate);
            this.Controls.Add(this.txtEsito);
            this.Controls.Add(this.lblSelectedSerial);
            this.Controls.Add(this.btnUsbReload);
            this.Controls.Add(this.txtFtdiSerialId);
            this.Controls.Add(this.lvwListaPorte);
            this.Controls.Add(this.btnApplicaTemplate);
            this.Controls.Add(this.btnChiudi);
            this.Controls.Add(this.grbTargetTemplate);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFlashFTDI";
            this.Text = "Flash FTDI";
            this.grbTargetTemplate.ResumeLayout(false);
            this.grbTargetTemplate.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grbTargetTemplate;
        private System.Windows.Forms.RadioButton optFTDILadeLight;
        private System.Windows.Forms.RadioButton optFTDISpybatt;
        private System.Windows.Forms.RadioButton optFTDIGenerico;
        private System.Windows.Forms.Button btnChiudi;
        private System.Windows.Forms.Button btnApplicaTemplate;
        private System.Windows.Forms.ListView lvwListaPorte;
        private System.Windows.Forms.TextBox txtFtdiSerialId;
        private System.Windows.Forms.Button btnUsbReload;
        private System.Windows.Forms.Label lblSelectedSerial;
        private System.Windows.Forms.TextBox txtEsito;
        private System.Windows.Forms.Button btnMostraTemplate;
        private System.Windows.Forms.Button btnResetDevice;
        private System.Windows.Forms.RadioButton optFTDISBFinto;
        private System.Windows.Forms.RadioButton optFTDIDesolf;
    }
}