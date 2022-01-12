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
            this.optFTDIRegen = new System.Windows.Forms.RadioButton();
            this.optFTDIBattRegen = new System.Windows.Forms.RadioButton();
            this.optFTDIidbatt = new System.Windows.Forms.RadioButton();
            this.optFTDIDesolf = new System.Windows.Forms.RadioButton();
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
            this.optFTDISuperChg = new System.Windows.Forms.RadioButton();
            this.grbTargetTemplate.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbTargetTemplate
            // 
            this.grbTargetTemplate.BackColor = System.Drawing.Color.White;
            this.grbTargetTemplate.Controls.Add(this.optFTDISuperChg);
            this.grbTargetTemplate.Controls.Add(this.optFTDIRegen);
            this.grbTargetTemplate.Controls.Add(this.optFTDIBattRegen);
            this.grbTargetTemplate.Controls.Add(this.optFTDIidbatt);
            this.grbTargetTemplate.Controls.Add(this.optFTDIDesolf);
            this.grbTargetTemplate.Controls.Add(this.optFTDISBFinto);
            this.grbTargetTemplate.Controls.Add(this.optFTDILadeLight);
            this.grbTargetTemplate.Controls.Add(this.optFTDISpybatt);
            this.grbTargetTemplate.Controls.Add(this.optFTDIGenerico);
            this.grbTargetTemplate.Location = new System.Drawing.Point(20, 184);
            this.grbTargetTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.grbTargetTemplate.Name = "grbTargetTemplate";
            this.grbTargetTemplate.Padding = new System.Windows.Forms.Padding(2);
            this.grbTargetTemplate.Size = new System.Drawing.Size(385, 228);
            this.grbTargetTemplate.TabIndex = 0;
            this.grbTargetTemplate.TabStop = false;
            this.grbTargetTemplate.Text = "Modello Interfaccia";
            // 
            // optFTDIRegen
            // 
            this.optFTDIRegen.AutoSize = true;
            this.optFTDIRegen.Location = new System.Drawing.Point(27, 132);
            this.optFTDIRegen.Margin = new System.Windows.Forms.Padding(2);
            this.optFTDIRegen.Name = "optFTDIRegen";
            this.optFTDIRegen.Size = new System.Drawing.Size(148, 17);
            this.optFTDIRegen.TabIndex = 7;
            this.optFTDIRegen.Text = "Scheda REGENERATOR";
            this.optFTDIRegen.UseVisualStyleBackColor = true;
            // 
            // optFTDIBattRegen
            // 
            this.optFTDIBattRegen.AutoSize = true;
            this.optFTDIBattRegen.Location = new System.Drawing.Point(27, 110);
            this.optFTDIBattRegen.Margin = new System.Windows.Forms.Padding(2);
            this.optFTDIBattRegen.Name = "optFTDIBattRegen";
            this.optFTDIBattRegen.Size = new System.Drawing.Size(201, 17);
            this.optFTDIBattRegen.TabIndex = 6;
            this.optFTDIBattRegen.Text = "Scheda BATTERY REGENERATOR";
            this.optFTDIBattRegen.UseVisualStyleBackColor = true;
            // 
            // optFTDIidbatt
            // 
            this.optFTDIidbatt.AutoSize = true;
            this.optFTDIidbatt.Location = new System.Drawing.Point(27, 89);
            this.optFTDIidbatt.Margin = new System.Windows.Forms.Padding(2);
            this.optFTDIidbatt.Name = "optFTDIidbatt";
            this.optFTDIidbatt.Size = new System.Drawing.Size(166, 17);
            this.optFTDIidbatt.TabIndex = 5;
            this.optFTDIidbatt.Text = "Scheda ID-BATT Programmer";
            this.optFTDIidbatt.UseVisualStyleBackColor = true;
            // 
            // optFTDIDesolf
            // 
            this.optFTDIDesolf.AutoSize = true;
            this.optFTDIDesolf.Location = new System.Drawing.Point(27, 153);
            this.optFTDIDesolf.Margin = new System.Windows.Forms.Padding(2);
            this.optFTDIDesolf.Name = "optFTDIDesolf";
            this.optFTDIDesolf.Size = new System.Drawing.Size(195, 17);
            this.optFTDIDesolf.TabIndex = 4;
            this.optFTDIDesolf.Text = "Scheda Display DESOLFATATORE";
            this.optFTDIDesolf.UseVisualStyleBackColor = true;
            // 
            // optFTDISBFinto
            // 
            this.optFTDISBFinto.AutoSize = true;
            this.optFTDISBFinto.ForeColor = System.Drawing.Color.Green;
            this.optFTDISBFinto.Location = new System.Drawing.Point(27, 195);
            this.optFTDISBFinto.Margin = new System.Windows.Forms.Padding(2);
            this.optFTDISBFinto.Name = "optFTDISBFinto";
            this.optFTDISBFinto.Size = new System.Drawing.Size(167, 17);
            this.optFTDISBFinto.TabIndex = 3;
            this.optFTDISBFinto.Text = "Interfaccia FT201x su HW SB";
            this.optFTDISBFinto.UseVisualStyleBackColor = true;
            this.optFTDISBFinto.CheckedChanged += new System.EventHandler(this.optFTDISBFinto_CheckedChanged);
            // 
            // optFTDILadeLight
            // 
            this.optFTDILadeLight.AutoSize = true;
            this.optFTDILadeLight.Location = new System.Drawing.Point(27, 68);
            this.optFTDILadeLight.Margin = new System.Windows.Forms.Padding(2);
            this.optFTDILadeLight.Name = "optFTDILadeLight";
            this.optFTDILadeLight.Size = new System.Drawing.Size(128, 17);
            this.optFTDILadeLight.TabIndex = 2;
            this.optFTDILadeLight.Text = "Scheda LADE-LIGHT";
            this.optFTDILadeLight.UseVisualStyleBackColor = true;
            // 
            // optFTDISpybatt
            // 
            this.optFTDISpybatt.AutoSize = true;
            this.optFTDISpybatt.Checked = true;
            this.optFTDISpybatt.Location = new System.Drawing.Point(27, 46);
            this.optFTDISpybatt.Margin = new System.Windows.Forms.Padding(2);
            this.optFTDISpybatt.Name = "optFTDISpybatt";
            this.optFTDISpybatt.Size = new System.Drawing.Size(117, 17);
            this.optFTDISpybatt.TabIndex = 1;
            this.optFTDISpybatt.TabStop = true;
            this.optFTDISpybatt.Text = "Scheda SPY-BATT";
            this.optFTDISpybatt.UseVisualStyleBackColor = true;
            // 
            // optFTDIGenerico
            // 
            this.optFTDIGenerico.AutoSize = true;
            this.optFTDIGenerico.ForeColor = System.Drawing.Color.Red;
            this.optFTDIGenerico.Location = new System.Drawing.Point(27, 24);
            this.optFTDIGenerico.Margin = new System.Windows.Forms.Padding(2);
            this.optFTDIGenerico.Name = "optFTDIGenerico";
            this.optFTDIGenerico.Size = new System.Drawing.Size(140, 17);
            this.optFTDIGenerico.TabIndex = 0;
            this.optFTDIGenerico.Text = "Interfaccia base FT201x";
            this.optFTDIGenerico.UseVisualStyleBackColor = true;
            // 
            // btnChiudi
            // 
            this.btnChiudi.Location = new System.Drawing.Point(326, 590);
            this.btnChiudi.Margin = new System.Windows.Forms.Padding(2);
            this.btnChiudi.Name = "btnChiudi";
            this.btnChiudi.Size = new System.Drawing.Size(79, 31);
            this.btnChiudi.TabIndex = 1;
            this.btnChiudi.Text = "Chiudi";
            this.btnChiudi.UseVisualStyleBackColor = true;
            this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
            // 
            // btnApplicaTemplate
            // 
            this.btnApplicaTemplate.Location = new System.Drawing.Point(104, 590);
            this.btnApplicaTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.btnApplicaTemplate.Name = "btnApplicaTemplate";
            this.btnApplicaTemplate.Size = new System.Drawing.Size(79, 31);
            this.btnApplicaTemplate.TabIndex = 2;
            this.btnApplicaTemplate.Text = "Applica";
            this.btnApplicaTemplate.UseVisualStyleBackColor = true;
            this.btnApplicaTemplate.Click += new System.EventHandler(this.btnApplicaTemplate_Click);
            // 
            // lvwListaPorte
            // 
            this.lvwListaPorte.FullRowSelect = true;
            this.lvwListaPorte.HideSelection = false;
            this.lvwListaPorte.Location = new System.Drawing.Point(20, 18);
            this.lvwListaPorte.Margin = new System.Windows.Forms.Padding(2);
            this.lvwListaPorte.Name = "lvwListaPorte";
            this.lvwListaPorte.Size = new System.Drawing.Size(386, 104);
            this.lvwListaPorte.TabIndex = 3;
            this.lvwListaPorte.UseCompatibleStateImageBehavior = false;
            this.lvwListaPorte.View = System.Windows.Forms.View.List;
            this.lvwListaPorte.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwListaPorte_MouseDoubleClick);
            // 
            // txtFtdiSerialId
            // 
            this.txtFtdiSerialId.Location = new System.Drawing.Point(230, 150);
            this.txtFtdiSerialId.Margin = new System.Windows.Forms.Padding(2);
            this.txtFtdiSerialId.Name = "txtFtdiSerialId";
            this.txtFtdiSerialId.Size = new System.Drawing.Size(176, 20);
            this.txtFtdiSerialId.TabIndex = 4;
            // 
            // btnUsbReload
            // 
            this.btnUsbReload.Location = new System.Drawing.Point(20, 136);
            this.btnUsbReload.Margin = new System.Windows.Forms.Padding(2);
            this.btnUsbReload.Name = "btnUsbReload";
            this.btnUsbReload.Size = new System.Drawing.Size(196, 31);
            this.btnUsbReload.TabIndex = 5;
            this.btnUsbReload.Text = "Ricarica Lista";
            this.btnUsbReload.UseVisualStyleBackColor = true;
            this.btnUsbReload.Click += new System.EventHandler(this.btnUsbReload_Click);
            // 
            // lblSelectedSerial
            // 
            this.lblSelectedSerial.AutoSize = true;
            this.lblSelectedSerial.Location = new System.Drawing.Point(227, 133);
            this.lblSelectedSerial.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSelectedSerial.Name = "lblSelectedSerial";
            this.lblSelectedSerial.Size = new System.Drawing.Size(133, 13);
            this.lblSelectedSerial.TabIndex = 6;
            this.lblSelectedSerial.Text = "Seriale scheda selezionata";
            // 
            // txtEsito
            // 
            this.txtEsito.Font = new System.Drawing.Font("Courier New", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEsito.Location = new System.Drawing.Point(20, 416);
            this.txtEsito.Margin = new System.Windows.Forms.Padding(2);
            this.txtEsito.Multiline = true;
            this.txtEsito.Name = "txtEsito";
            this.txtEsito.ReadOnly = true;
            this.txtEsito.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtEsito.Size = new System.Drawing.Size(386, 154);
            this.txtEsito.TabIndex = 7;
            // 
            // btnMostraTemplate
            // 
            this.btnMostraTemplate.Location = new System.Drawing.Point(20, 590);
            this.btnMostraTemplate.Margin = new System.Windows.Forms.Padding(2);
            this.btnMostraTemplate.Name = "btnMostraTemplate";
            this.btnMostraTemplate.Size = new System.Drawing.Size(79, 31);
            this.btnMostraTemplate.TabIndex = 8;
            this.btnMostraTemplate.Text = "Dettaglio";
            this.btnMostraTemplate.UseVisualStyleBackColor = true;
            this.btnMostraTemplate.Click += new System.EventHandler(this.btnMostraTemplate_Click);
            // 
            // btnResetDevice
            // 
            this.btnResetDevice.Location = new System.Drawing.Point(187, 590);
            this.btnResetDevice.Margin = new System.Windows.Forms.Padding(2);
            this.btnResetDevice.Name = "btnResetDevice";
            this.btnResetDevice.Size = new System.Drawing.Size(113, 31);
            this.btnResetDevice.TabIndex = 9;
            this.btnResetDevice.Text = "Reset Device";
            this.btnResetDevice.UseVisualStyleBackColor = true;
            this.btnResetDevice.Click += new System.EventHandler(this.btnResetDevice_Click);
            // 
            // optFTDISuperChg
            // 
            this.optFTDISuperChg.AutoSize = true;
            this.optFTDISuperChg.Location = new System.Drawing.Point(27, 174);
            this.optFTDISuperChg.Margin = new System.Windows.Forms.Padding(2);
            this.optFTDISuperChg.Name = "optFTDISuperChg";
            this.optFTDISuperChg.Size = new System.Drawing.Size(203, 17);
            this.optFTDISuperChg.TabIndex = 8;
            this.optFTDISuperChg.Text = "Scheda DISPLAY SUPERCHARGER";
            this.optFTDISuperChg.UseVisualStyleBackColor = true;
            // 
            // frmFlashFTDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(427, 634);
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
            this.Margin = new System.Windows.Forms.Padding(2);
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
        private System.Windows.Forms.RadioButton optFTDIBattRegen;
        private System.Windows.Forms.RadioButton optFTDIidbatt;
        private System.Windows.Forms.RadioButton optFTDIRegen;
        private System.Windows.Forms.RadioButton optFTDISuperChg;
    }
}