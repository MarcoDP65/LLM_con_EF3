namespace PannelloCharger
{
    partial class frmAlimentatore
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
            this.label13 = new System.Windows.Forms.Label();
            this.btnImpostaTensione = new System.Windows.Forms.Button();
            this.txtTdkVCheck = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTdkVSetCheck = new System.Windows.Forms.TextBox();
            this.txtTdkVSet = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLeggiCorrente = new System.Windows.Forms.Button();
            this.btnImpostaCorrente = new System.Windows.Forms.Button();
            this.txtTdkACheck = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTdkASetCheck = new System.Windows.Forms.TextBox();
            this.txtTdkASet = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.serPortaLambda = new System.IO.Ports.SerialPort(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtStato = new System.Windows.Forms.TextBox();
            this.btnAttivaUscita = new System.Windows.Forms.Button();
            this.btnLeggiTensione = new System.Windows.Forms.Button();
            this.grbTensioni = new System.Windows.Forms.GroupBox();
            this.grbPortaSeriale = new System.Windows.Forms.GroupBox();
            this.btnInit = new System.Windows.Forms.Button();
            this.btnVerificaTDK = new System.Windows.Forms.Button();
            this.btnStatoCom = new System.Windows.Forms.Button();
            this.txtStatoPorta = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtComApparato = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtStatCom = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtComBit = new System.Windows.Forms.TextBox();
            this.txtComBaudrate = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtComPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grbTensioni.SuspendLayout();
            this.grbPortaSeriale.SuspendLayout();
            this.SuspendLayout();
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(131, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(84, 17);
            this.label13.TabIndex = 9;
            this.label13.Text = "Stato Uscita";
            // 
            // btnImpostaTensione
            // 
            this.btnImpostaTensione.Location = new System.Drawing.Point(15, 123);
            this.btnImpostaTensione.Name = "btnImpostaTensione";
            this.btnImpostaTensione.Size = new System.Drawing.Size(94, 32);
            this.btnImpostaTensione.TabIndex = 20;
            this.btnImpostaTensione.Text = "Imposta";
            this.btnImpostaTensione.UseVisualStyleBackColor = true;
            this.btnImpostaTensione.Click += new System.EventHandler(this.btnImpostaTensione_Click);
            // 
            // txtTdkVCheck
            // 
            this.txtTdkVCheck.Location = new System.Drawing.Point(132, 57);
            this.txtTdkVCheck.Name = "txtTdkVCheck";
            this.txtTdkVCheck.ReadOnly = true;
            this.txtTdkVCheck.Size = new System.Drawing.Size(74, 22);
            this.txtTdkVCheck.TabIndex = 8;
            this.txtTdkVCheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(129, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 17);
            this.label10.TabIndex = 7;
            this.label10.Text = "V Rilevati";
            // 
            // txtTdkVSetCheck
            // 
            this.txtTdkVSetCheck.Location = new System.Drawing.Point(15, 85);
            this.txtTdkVSetCheck.Name = "txtTdkVSetCheck";
            this.txtTdkVSetCheck.ReadOnly = true;
            this.txtTdkVSetCheck.Size = new System.Drawing.Size(74, 22);
            this.txtTdkVSetCheck.TabIndex = 6;
            this.txtTdkVSetCheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTdkVSet
            // 
            this.txtTdkVSet.Location = new System.Drawing.Point(15, 57);
            this.txtTdkVSet.Name = "txtTdkVSet";
            this.txtTdkVSet.Size = new System.Drawing.Size(74, 22);
            this.txtTdkVSet.TabIndex = 5;
            this.txtTdkVSet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 17);
            this.label9.TabIndex = 4;
            this.label9.Text = "V Impostati";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLeggiCorrente);
            this.groupBox1.Controls.Add(this.btnImpostaCorrente);
            this.groupBox1.Controls.Add(this.txtTdkACheck);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtTdkASetCheck);
            this.groupBox1.Controls.Add(this.txtTdkASet);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Location = new System.Drawing.Point(297, 192);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(243, 177);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Corrente";
            // 
            // btnLeggiCorrente
            // 
            this.btnLeggiCorrente.Location = new System.Drawing.Point(132, 123);
            this.btnLeggiCorrente.Name = "btnLeggiCorrente";
            this.btnLeggiCorrente.Size = new System.Drawing.Size(94, 32);
            this.btnLeggiCorrente.TabIndex = 23;
            this.btnLeggiCorrente.Text = "Verifica";
            this.btnLeggiCorrente.UseVisualStyleBackColor = true;
            this.btnLeggiCorrente.Click += new System.EventHandler(this.btnLeggiCorrente_Click);
            // 
            // btnImpostaCorrente
            // 
            this.btnImpostaCorrente.Location = new System.Drawing.Point(15, 123);
            this.btnImpostaCorrente.Name = "btnImpostaCorrente";
            this.btnImpostaCorrente.Size = new System.Drawing.Size(94, 32);
            this.btnImpostaCorrente.TabIndex = 22;
            this.btnImpostaCorrente.Text = "Imposta";
            this.btnImpostaCorrente.UseVisualStyleBackColor = false;
            this.btnImpostaCorrente.Click += new System.EventHandler(this.btnImpostaCorrente_Click);
            // 
            // txtTdkACheck
            // 
            this.txtTdkACheck.Location = new System.Drawing.Point(132, 57);
            this.txtTdkACheck.Name = "txtTdkACheck";
            this.txtTdkACheck.ReadOnly = true;
            this.txtTdkACheck.Size = new System.Drawing.Size(74, 22);
            this.txtTdkACheck.TabIndex = 8;
            this.txtTdkACheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(129, 37);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 17);
            this.label11.TabIndex = 7;
            this.label11.Text = "A Rilevati";
            // 
            // txtTdkASetCheck
            // 
            this.txtTdkASetCheck.Location = new System.Drawing.Point(15, 85);
            this.txtTdkASetCheck.Name = "txtTdkASetCheck";
            this.txtTdkASetCheck.ReadOnly = true;
            this.txtTdkASetCheck.Size = new System.Drawing.Size(74, 22);
            this.txtTdkASetCheck.TabIndex = 6;
            this.txtTdkASetCheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTdkASet
            // 
            this.txtTdkASet.Location = new System.Drawing.Point(15, 57);
            this.txtTdkASet.Name = "txtTdkASet";
            this.txtTdkASet.Size = new System.Drawing.Size(74, 22);
            this.txtTdkASet.TabIndex = 5;
            this.txtTdkASet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 37);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 17);
            this.label12.TabIndex = 4;
            this.label12.Text = "A Impostati";
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.LightYellow;
            this.groupBox2.Controls.Add(this.txtStato);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.btnAttivaUscita);
            this.groupBox2.Location = new System.Drawing.Point(29, 405);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(510, 97);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stato Alimentatore";
            // 
            // txtStato
            // 
            this.txtStato.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStato.Location = new System.Drawing.Point(134, 37);
            this.txtStato.Name = "txtStato";
            this.txtStato.Size = new System.Drawing.Size(191, 34);
            this.txtStato.TabIndex = 10;
            this.txtStato.Text = "SPENTO";
            this.txtStato.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnAttivaUscita
            // 
            this.btnAttivaUscita.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAttivaUscita.Location = new System.Drawing.Point(17, 37);
            this.btnAttivaUscita.Name = "btnAttivaUscita";
            this.btnAttivaUscita.Size = new System.Drawing.Size(101, 32);
            this.btnAttivaUscita.TabIndex = 0;
            this.btnAttivaUscita.Text = "ATTIVA";
            this.btnAttivaUscita.UseVisualStyleBackColor = true;
            this.btnAttivaUscita.Click += new System.EventHandler(this.btnAttivaUscita_Click);
            // 
            // btnLeggiTensione
            // 
            this.btnLeggiTensione.Location = new System.Drawing.Point(132, 123);
            this.btnLeggiTensione.Name = "btnLeggiTensione";
            this.btnLeggiTensione.Size = new System.Drawing.Size(94, 32);
            this.btnLeggiTensione.TabIndex = 21;
            this.btnLeggiTensione.Text = "Verifica";
            this.btnLeggiTensione.UseVisualStyleBackColor = true;
            this.btnLeggiTensione.Click += new System.EventHandler(this.btnLeggiTensione_Click);
            // 
            // grbTensioni
            // 
            this.grbTensioni.Controls.Add(this.btnLeggiTensione);
            this.grbTensioni.Controls.Add(this.btnImpostaTensione);
            this.grbTensioni.Controls.Add(this.txtTdkVCheck);
            this.grbTensioni.Controls.Add(this.label10);
            this.grbTensioni.Controls.Add(this.txtTdkVSetCheck);
            this.grbTensioni.Controls.Add(this.txtTdkVSet);
            this.grbTensioni.Controls.Add(this.label9);
            this.grbTensioni.Location = new System.Drawing.Point(27, 192);
            this.grbTensioni.Name = "grbTensioni";
            this.grbTensioni.Size = new System.Drawing.Size(243, 177);
            this.grbTensioni.TabIndex = 7;
            this.grbTensioni.TabStop = false;
            this.grbTensioni.Text = "Tensione";
            // 
            // grbPortaSeriale
            // 
            this.grbPortaSeriale.Controls.Add(this.btnInit);
            this.grbPortaSeriale.Controls.Add(this.btnVerificaTDK);
            this.grbPortaSeriale.Controls.Add(this.btnStatoCom);
            this.grbPortaSeriale.Controls.Add(this.txtStatoPorta);
            this.grbPortaSeriale.Controls.Add(this.label8);
            this.grbPortaSeriale.Controls.Add(this.txtComApparato);
            this.grbPortaSeriale.Controls.Add(this.label7);
            this.grbPortaSeriale.Controls.Add(this.txtStatCom);
            this.grbPortaSeriale.Controls.Add(this.label6);
            this.grbPortaSeriale.Controls.Add(this.label5);
            this.grbPortaSeriale.Controls.Add(this.textBox2);
            this.grbPortaSeriale.Controls.Add(this.label4);
            this.grbPortaSeriale.Controls.Add(this.textBox1);
            this.grbPortaSeriale.Controls.Add(this.label3);
            this.grbPortaSeriale.Controls.Add(this.txtComBit);
            this.grbPortaSeriale.Controls.Add(this.txtComBaudrate);
            this.grbPortaSeriale.Controls.Add(this.label2);
            this.grbPortaSeriale.Controls.Add(this.txtComPort);
            this.grbPortaSeriale.Controls.Add(this.label1);
            this.grbPortaSeriale.Location = new System.Drawing.Point(27, 29);
            this.grbPortaSeriale.Name = "grbPortaSeriale";
            this.grbPortaSeriale.Size = new System.Drawing.Size(513, 143);
            this.grbPortaSeriale.TabIndex = 6;
            this.grbPortaSeriale.TabStop = false;
            this.grbPortaSeriale.Text = "Porta Seriale";
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(253, 41);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(53, 32);
            this.btnInit.TabIndex = 19;
            this.btnInit.Text = "INIT";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // btnVerificaTDK
            // 
            this.btnVerificaTDK.Location = new System.Drawing.Point(404, 91);
            this.btnVerificaTDK.Name = "btnVerificaTDK";
            this.btnVerificaTDK.Size = new System.Drawing.Size(94, 32);
            this.btnVerificaTDK.TabIndex = 19;
            this.btnVerificaTDK.Text = "Verifica";
            this.btnVerificaTDK.UseVisualStyleBackColor = true;
            this.btnVerificaTDK.Click += new System.EventHandler(this.btnVerificaTDK_Click);
            // 
            // btnStatoCom
            // 
            this.btnStatoCom.Enabled = false;
            this.btnStatoCom.Location = new System.Drawing.Point(404, 42);
            this.btnStatoCom.Name = "btnStatoCom";
            this.btnStatoCom.Size = new System.Drawing.Size(94, 32);
            this.btnStatoCom.TabIndex = 18;
            this.btnStatoCom.Text = "connetti";
            this.btnStatoCom.UseVisualStyleBackColor = true;
            this.btnStatoCom.Click += new System.EventHandler(this.btnStatoCom_Click);
            // 
            // txtStatoPorta
            // 
            this.txtStatoPorta.Location = new System.Drawing.Point(312, 47);
            this.txtStatoPorta.Name = "txtStatoPorta";
            this.txtStatoPorta.ReadOnly = true;
            this.txtStatoPorta.Size = new System.Drawing.Size(74, 22);
            this.txtStatoPorta.TabIndex = 17;
            this.txtStatoPorta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(309, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 17);
            this.label8.TabIndex = 16;
            this.label8.Text = "Stato Porta";
            // 
            // txtComApparato
            // 
            this.txtComApparato.Location = new System.Drawing.Point(19, 96);
            this.txtComApparato.Name = "txtComApparato";
            this.txtComApparato.Size = new System.Drawing.Size(228, 22);
            this.txtComApparato.TabIndex = 15;
            this.txtComApparato.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 76);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Apparato";
            // 
            // txtStatCom
            // 
            this.txtStatCom.Location = new System.Drawing.Point(312, 96);
            this.txtStatCom.Name = "txtStatCom";
            this.txtStatCom.Size = new System.Drawing.Size(74, 22);
            this.txtStatCom.TabIndex = 13;
            this.txtStatCom.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(309, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Stato";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(250, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Addr";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(253, 96);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(53, 22);
            this.textBox2.TabIndex = 10;
            this.textBox2.Text = "06";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(213, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Par";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(216, 47);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(31, 22);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "1";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(176, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Bit";
            // 
            // txtComBit
            // 
            this.txtComBit.Location = new System.Drawing.Point(179, 47);
            this.txtComBit.Name = "txtComBit";
            this.txtComBit.Size = new System.Drawing.Size(31, 22);
            this.txtComBit.TabIndex = 6;
            this.txtComBit.Text = "8";
            this.txtComBit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtComBaudrate
            // 
            this.txtComBaudrate.Location = new System.Drawing.Point(99, 47);
            this.txtComBaudrate.Name = "txtComBaudrate";
            this.txtComBaudrate.Size = new System.Drawing.Size(74, 22);
            this.txtComBaudrate.TabIndex = 5;
            this.txtComBaudrate.Text = "9600";
            this.txtComBaudrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(99, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Baudrate";
            // 
            // txtComPort
            // 
            this.txtComPort.Location = new System.Drawing.Point(19, 47);
            this.txtComPort.Name = "txtComPort";
            this.txtComPort.Size = new System.Drawing.Size(74, 22);
            this.txtComPort.TabIndex = 3;
            this.txtComPort.Text = "COM9";
            this.txtComPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Porta COM";
            // 
            // frmAlimentatore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 592);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grbTensioni);
            this.Controls.Add(this.grbPortaSeriale);
            this.Name = "frmAlimentatore";
            this.Text = "Alimentatore";
            this.Load += new System.EventHandler(this.frmAlimentatore_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grbTensioni.ResumeLayout(false);
            this.grbTensioni.PerformLayout();
            this.grbPortaSeriale.ResumeLayout(false);
            this.grbPortaSeriale.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnImpostaTensione;
        private System.Windows.Forms.TextBox txtTdkVCheck;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtTdkVSetCheck;
        private System.Windows.Forms.TextBox txtTdkVSet;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnLeggiCorrente;
        private System.Windows.Forms.Button btnImpostaCorrente;
        private System.Windows.Forms.TextBox txtTdkACheck;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtTdkASetCheck;
        private System.Windows.Forms.TextBox txtTdkASet;
        private System.Windows.Forms.Label label12;
        private System.IO.Ports.SerialPort serPortaLambda;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtStato;
        private System.Windows.Forms.Button btnAttivaUscita;
        private System.Windows.Forms.Button btnLeggiTensione;
        private System.Windows.Forms.GroupBox grbTensioni;
        private System.Windows.Forms.GroupBox grbPortaSeriale;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Button btnVerificaTDK;
        private System.Windows.Forms.Button btnStatoCom;
        private System.Windows.Forms.TextBox txtStatoPorta;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtComApparato;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtStatCom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtComBit;
        private System.Windows.Forms.TextBox txtComBaudrate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtComPort;
        private System.Windows.Forms.Label label1;
    }
}