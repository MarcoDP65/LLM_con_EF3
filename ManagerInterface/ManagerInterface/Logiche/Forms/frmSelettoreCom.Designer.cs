namespace PannelloCharger
{
    partial class frmSelettoreCom
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
            this.btnAnnulla = new System.Windows.Forms.Button();
            this.btnSalva = new System.Windows.Forms.Button();
            this.grbParametriPorta = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCurrData = new System.Windows.Forms.TextBox();
            this.lblCurrSpeed = new System.Windows.Forms.Label();
            this.txtCurrSpeed = new System.Windows.Forms.TextBox();
            this.txtStop = new System.Windows.Forms.TextBox();
            this.lblStop = new System.Windows.Forms.Label();
            this.lblParita = new System.Windows.Forms.Label();
            this.lblPorta = new System.Windows.Forms.Label();
            this.txtParita = new System.Windows.Forms.TextBox();
            this.txtCurrPort = new System.Windows.Forms.TextBox();
            this.grbRicercaPorte = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboParity = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboStopBits = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.cboDataBits = new System.Windows.Forms.ComboBox();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.cboPorts = new System.Windows.Forms.ComboBox();
            this.btnSelezionaPorta = new System.Windows.Forms.Button();
            this.btnCercaPorte = new System.Windows.Forms.Button();
            this.grbParametriPorta.SuspendLayout();
            this.grbRicercaPorte.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAnnulla
            // 
            this.btnAnnulla.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAnnulla.Location = new System.Drawing.Point(212, 259);
            this.btnAnnulla.Name = "btnAnnulla";
            this.btnAnnulla.Size = new System.Drawing.Size(125, 31);
            this.btnAnnulla.TabIndex = 10;
            this.btnAnnulla.Text = "Annulla";
            this.btnAnnulla.UseVisualStyleBackColor = true;
            this.btnAnnulla.Click += new System.EventHandler(this.btnAnnulla_Click);
            // 
            // btnSalva
            // 
            this.btnSalva.Location = new System.Drawing.Point(58, 259);
            this.btnSalva.Name = "btnSalva";
            this.btnSalva.Size = new System.Drawing.Size(125, 31);
            this.btnSalva.TabIndex = 9;
            this.btnSalva.Text = "Ok";
            this.btnSalva.UseVisualStyleBackColor = true;
            this.btnSalva.Click += new System.EventHandler(this.btnSalva_Click);
            // 
            // grbParametriPorta
            // 
            this.grbParametriPorta.Controls.Add(this.label4);
            this.grbParametriPorta.Controls.Add(this.txtCurrData);
            this.grbParametriPorta.Controls.Add(this.lblCurrSpeed);
            this.grbParametriPorta.Controls.Add(this.txtCurrSpeed);
            this.grbParametriPorta.Controls.Add(this.txtStop);
            this.grbParametriPorta.Controls.Add(this.lblStop);
            this.grbParametriPorta.Controls.Add(this.lblParita);
            this.grbParametriPorta.Controls.Add(this.lblPorta);
            this.grbParametriPorta.Controls.Add(this.txtParita);
            this.grbParametriPorta.Controls.Add(this.txtCurrPort);
            this.grbParametriPorta.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grbParametriPorta.Location = new System.Drawing.Point(12, 31);
            this.grbParametriPorta.Name = "grbParametriPorta";
            this.grbParametriPorta.Size = new System.Drawing.Size(384, 99);
            this.grbParametriPorta.TabIndex = 11;
            this.grbParametriPorta.TabStop = false;
            this.grbParametriPorta.Text = "Porta Corrente";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(199, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 20);
            this.label4.TabIndex = 16;
            this.label4.Text = "Dati";
            // 
            // txtCurrData
            // 
            this.txtCurrData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrData.Location = new System.Drawing.Point(201, 45);
            this.txtCurrData.Name = "txtCurrData";
            this.txtCurrData.Size = new System.Drawing.Size(40, 27);
            this.txtCurrData.TabIndex = 15;
            this.txtCurrData.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblCurrSpeed
            // 
            this.lblCurrSpeed.AutoSize = true;
            this.lblCurrSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrSpeed.Location = new System.Drawing.Point(103, 25);
            this.lblCurrSpeed.Name = "lblCurrSpeed";
            this.lblCurrSpeed.Size = new System.Drawing.Size(69, 20);
            this.lblCurrSpeed.TabIndex = 14;
            this.lblCurrSpeed.Text = "Velocità";
            // 
            // txtCurrSpeed
            // 
            this.txtCurrSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrSpeed.Location = new System.Drawing.Point(107, 45);
            this.txtCurrSpeed.Name = "txtCurrSpeed";
            this.txtCurrSpeed.Size = new System.Drawing.Size(82, 27);
            this.txtCurrSpeed.TabIndex = 13;
            this.txtCurrSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtStop
            // 
            this.txtStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStop.Location = new System.Drawing.Point(312, 45);
            this.txtStop.Name = "txtStop";
            this.txtStop.Size = new System.Drawing.Size(48, 27);
            this.txtStop.TabIndex = 12;
            this.txtStop.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStop.TextChanged += new System.EventHandler(this.txtStop_TextChanged);
            // 
            // lblStop
            // 
            this.lblStop.AutoSize = true;
            this.lblStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStop.Location = new System.Drawing.Point(312, 25);
            this.lblStop.Name = "lblStop";
            this.lblStop.Size = new System.Drawing.Size(43, 20);
            this.lblStop.TabIndex = 11;
            this.lblStop.Text = "Stop";
            this.lblStop.Click += new System.EventHandler(this.lblStop_Click);
            // 
            // lblParita
            // 
            this.lblParita.AutoSize = true;
            this.lblParita.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParita.Location = new System.Drawing.Point(247, 25);
            this.lblParita.Name = "lblParita";
            this.lblParita.Size = new System.Drawing.Size(53, 20);
            this.lblParita.TabIndex = 10;
            this.lblParita.Text = "Parità";
            // 
            // lblPorta
            // 
            this.lblPorta.AutoSize = true;
            this.lblPorta.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPorta.Location = new System.Drawing.Point(19, 25);
            this.lblPorta.Name = "lblPorta";
            this.lblPorta.Size = new System.Drawing.Size(49, 20);
            this.lblPorta.TabIndex = 9;
            this.lblPorta.Text = "Porta";
            // 
            // txtParita
            // 
            this.txtParita.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParita.Location = new System.Drawing.Point(249, 45);
            this.txtParita.Name = "txtParita";
            this.txtParita.Size = new System.Drawing.Size(57, 27);
            this.txtParita.TabIndex = 8;
            this.txtParita.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtCurrPort
            // 
            this.txtCurrPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrPort.Location = new System.Drawing.Point(23, 45);
            this.txtCurrPort.Name = "txtCurrPort";
            this.txtCurrPort.Size = new System.Drawing.Size(70, 27);
            this.txtCurrPort.TabIndex = 7;
            this.txtCurrPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // grbRicercaPorte
            // 
            this.grbRicercaPorte.Controls.Add(this.label6);
            this.grbRicercaPorte.Controls.Add(this.label5);
            this.grbRicercaPorte.Controls.Add(this.cboParity);
            this.grbRicercaPorte.Controls.Add(this.label3);
            this.grbRicercaPorte.Controls.Add(this.label2);
            this.grbRicercaPorte.Controls.Add(this.label1);
            this.grbRicercaPorte.Controls.Add(this.cboStopBits);
            this.grbRicercaPorte.Controls.Add(this.button1);
            this.grbRicercaPorte.Controls.Add(this.cboDataBits);
            this.grbRicercaPorte.Controls.Add(this.cboBaudRate);
            this.grbRicercaPorte.Controls.Add(this.cboPorts);
            this.grbRicercaPorte.Controls.Add(this.btnSelezionaPorta);
            this.grbRicercaPorte.Location = new System.Drawing.Point(12, 31);
            this.grbRicercaPorte.Name = "grbRicercaPorte";
            this.grbRicercaPorte.Size = new System.Drawing.Size(397, 278);
            this.grbRicercaPorte.TabIndex = 13;
            this.grbRicercaPorte.TabStop = false;
            this.grbRicercaPorte.Text = "Check Sistema";
            this.grbRicercaPorte.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(68, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "Stop";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(68, 140);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Parità";
            // 
            // cboParity
            // 
            this.cboParity.Enabled = false;
            this.cboParity.FormattingEnabled = true;
            this.cboParity.Location = new System.Drawing.Point(139, 169);
            this.cboParity.Margin = new System.Windows.Forms.Padding(4);
            this.cboParity.Name = "cboParity";
            this.cboParity.Size = new System.Drawing.Size(186, 24);
            this.cboParity.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(68, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Dati";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 17);
            this.label2.TabIndex = 9;
            this.label2.Text = "Velocità";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Porta";
            // 
            // cboStopBits
            // 
            this.cboStopBits.Enabled = false;
            this.cboStopBits.FormattingEnabled = true;
            this.cboStopBits.Location = new System.Drawing.Point(139, 137);
            this.cboStopBits.Margin = new System.Windows.Forms.Padding(4);
            this.cboStopBits.Name = "cboStopBits";
            this.cboStopBits.Size = new System.Drawing.Size(186, 24);
            this.cboStopBits.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(221, 220);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 27);
            this.button1.TabIndex = 6;
            this.button1.Text = "Annulla";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cboDataBits
            // 
            this.cboDataBits.Enabled = false;
            this.cboDataBits.FormattingEnabled = true;
            this.cboDataBits.Location = new System.Drawing.Point(139, 104);
            this.cboDataBits.Margin = new System.Windows.Forms.Padding(4);
            this.cboDataBits.Name = "cboDataBits";
            this.cboDataBits.Size = new System.Drawing.Size(186, 24);
            this.cboDataBits.TabIndex = 5;
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.FormattingEnabled = true;
            this.cboBaudRate.Location = new System.Drawing.Point(139, 68);
            this.cboBaudRate.Margin = new System.Windows.Forms.Padding(4);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(186, 24);
            this.cboBaudRate.TabIndex = 4;
            // 
            // cboPorts
            // 
            this.cboPorts.FormattingEnabled = true;
            this.cboPorts.Location = new System.Drawing.Point(139, 36);
            this.cboPorts.Margin = new System.Windows.Forms.Padding(4);
            this.cboPorts.Name = "cboPorts";
            this.cboPorts.Size = new System.Drawing.Size(186, 24);
            this.cboPorts.TabIndex = 3;
            // 
            // btnSelezionaPorta
            // 
            this.btnSelezionaPorta.Location = new System.Drawing.Point(68, 220);
            this.btnSelezionaPorta.Name = "btnSelezionaPorta";
            this.btnSelezionaPorta.Size = new System.Drawing.Size(104, 27);
            this.btnSelezionaPorta.TabIndex = 0;
            this.btnSelezionaPorta.Text = "Seleziona";
            this.btnSelezionaPorta.UseVisualStyleBackColor = true;
            this.btnSelezionaPorta.Click += new System.EventHandler(this.btnSelezionaPorta_Click);
            // 
            // btnCercaPorte
            // 
            this.btnCercaPorte.Location = new System.Drawing.Point(127, 171);
            this.btnCercaPorte.Name = "btnCercaPorte";
            this.btnCercaPorte.Size = new System.Drawing.Size(150, 34);
            this.btnCercaPorte.TabIndex = 14;
            this.btnCercaPorte.Text = "Cerca Porte";
            this.btnCercaPorte.UseVisualStyleBackColor = true;
            this.btnCercaPorte.Click += new System.EventHandler(this.btnCercaPorte_Click);
            // 
            // frmSelettoreCom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 320);
            this.Controls.Add(this.grbRicercaPorte);
            this.Controls.Add(this.grbParametriPorta);
            this.Controls.Add(this.btnAnnulla);
            this.Controls.Add(this.btnSalva);
            this.Controls.Add(this.btnCercaPorte);
            this.Name = "frmSelettoreCom";
            this.ShowIcon = false;
            this.Text = "Porta Seriale";
            this.Load += new System.EventHandler(this.frmSelettoreCom_Load);
            this.grbParametriPorta.ResumeLayout(false);
            this.grbParametriPorta.PerformLayout();
            this.grbRicercaPorte.ResumeLayout(false);
            this.grbRicercaPorte.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAnnulla;
        private System.Windows.Forms.Button btnSalva;
        private System.Windows.Forms.GroupBox grbParametriPorta;
        private System.Windows.Forms.TextBox txtStop;
        private System.Windows.Forms.Label lblStop;
        private System.Windows.Forms.Label lblParita;
        private System.Windows.Forms.Label lblPorta;
        private System.Windows.Forms.TextBox txtParita;
        private System.Windows.Forms.TextBox txtCurrPort;
        private System.Windows.Forms.GroupBox grbRicercaPorte;
        private System.Windows.Forms.Button btnCercaPorte;
        private System.Windows.Forms.Button btnSelezionaPorta;
        private System.Windows.Forms.ComboBox cboPorts;
        private System.Windows.Forms.ComboBox cboBaudRate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cboDataBits;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCurrData;
        private System.Windows.Forms.Label lblCurrSpeed;
        private System.Windows.Forms.TextBox txtCurrSpeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboStopBits;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboParity;
    }
}