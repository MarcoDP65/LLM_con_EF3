namespace PannelloCharger
{
    partial class frmPopupCambioTurmo
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
            this.chkPilotaggio = new System.Windows.Forms.CheckBox();
            this.chkEqualizzazione = new System.Windows.Forms.CheckBox();
            this.pnlOrarioInizio = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudMinutiIn = new System.Windows.Forms.NumericUpDown();
            this.nudOreIn = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlOrarioFine = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudMinutiFin = new System.Windows.Forms.NumericUpDown();
            this.nudOreFin = new System.Windows.Forms.NumericUpDown();
            this.pnlFattoreCarica = new System.Windows.Forms.Panel();
            this.nudFattoreCarica = new System.Windows.Forms.NumericUpDown();
            this.pnlSceltaCorrente = new System.Windows.Forms.Panel();
            this.optCorrenteCapacita = new System.Windows.Forms.RadioButton();
            this.optCorrenteTempo = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnAnnulla = new System.Windows.Forms.Button();
            this.pnlOrarioInizio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutiIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOreIn)).BeginInit();
            this.pnlOrarioFine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutiFin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOreFin)).BeginInit();
            this.pnlFattoreCarica.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFattoreCarica)).BeginInit();
            this.pnlSceltaCorrente.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkPilotaggio
            // 
            this.chkPilotaggio.AutoSize = true;
            this.chkPilotaggio.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPilotaggio.Location = new System.Drawing.Point(351, 282);
            this.chkPilotaggio.Name = "chkPilotaggio";
            this.chkPilotaggio.Size = new System.Drawing.Size(180, 24);
            this.chkPilotaggio.TabIndex = 0;
            this.chkPilotaggio.Text = "Abilita Biberonaggio";
            this.chkPilotaggio.UseVisualStyleBackColor = true;
            // 
            // chkEqualizzazione
            // 
            this.chkEqualizzazione.AutoSize = true;
            this.chkEqualizzazione.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEqualizzazione.Location = new System.Drawing.Point(67, 282);
            this.chkEqualizzazione.Name = "chkEqualizzazione";
            this.chkEqualizzazione.Size = new System.Drawing.Size(195, 24);
            this.chkEqualizzazione.TabIndex = 1;
            this.chkEqualizzazione.Text = "Abilita Equalizzazione";
            this.chkEqualizzazione.UseVisualStyleBackColor = true;
            // 
            // pnlOrarioInizio
            // 
            this.pnlOrarioInizio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOrarioInizio.Controls.Add(this.label2);
            this.pnlOrarioInizio.Controls.Add(this.label1);
            this.pnlOrarioInizio.Controls.Add(this.nudMinutiIn);
            this.pnlOrarioInizio.Controls.Add(this.nudOreIn);
            this.pnlOrarioInizio.Location = new System.Drawing.Point(47, 58);
            this.pnlOrarioInizio.Name = "pnlOrarioInizio";
            this.pnlOrarioInizio.Size = new System.Drawing.Size(223, 63);
            this.pnlOrarioInizio.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Minuti";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "Ore";
            // 
            // nudMinutiIn
            // 
            this.nudMinutiIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinutiIn.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudMinutiIn.Location = new System.Drawing.Point(116, 20);
            this.nudMinutiIn.Maximum = new decimal(new int[] {
            45,
            0,
            0,
            0});
            this.nudMinutiIn.Name = "nudMinutiIn";
            this.nudMinutiIn.Size = new System.Drawing.Size(98, 30);
            this.nudMinutiIn.TabIndex = 8;
            this.nudMinutiIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudOreIn
            // 
            this.nudOreIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudOreIn.Location = new System.Drawing.Point(8, 20);
            this.nudOreIn.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nudOreIn.Name = "nudOreIn";
            this.nudOreIn.Size = new System.Drawing.Size(98, 30);
            this.nudOreIn.TabIndex = 7;
            this.nudOreIn.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(84, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Inizio Cambio Turno";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(373, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Fine Cambio Turno";
            // 
            // pnlOrarioFine
            // 
            this.pnlOrarioFine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOrarioFine.Controls.Add(this.label5);
            this.pnlOrarioFine.Controls.Add(this.label6);
            this.pnlOrarioFine.Controls.Add(this.nudMinutiFin);
            this.pnlOrarioFine.Controls.Add(this.nudOreFin);
            this.pnlOrarioFine.Location = new System.Drawing.Point(342, 57);
            this.pnlOrarioFine.Name = "pnlOrarioFine";
            this.pnlOrarioFine.Size = new System.Drawing.Size(223, 63);
            this.pnlOrarioFine.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(129, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Minuti";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(35, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Ore";
            // 
            // nudMinutiFin
            // 
            this.nudMinutiFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinutiFin.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudMinutiFin.Location = new System.Drawing.Point(116, 20);
            this.nudMinutiFin.Maximum = new decimal(new int[] {
            45,
            0,
            0,
            0});
            this.nudMinutiFin.Name = "nudMinutiFin";
            this.nudMinutiFin.Size = new System.Drawing.Size(98, 30);
            this.nudMinutiFin.TabIndex = 8;
            this.nudMinutiFin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudOreFin
            // 
            this.nudOreFin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudOreFin.Location = new System.Drawing.Point(8, 20);
            this.nudOreFin.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nudOreFin.Name = "nudOreFin";
            this.nudOreFin.Size = new System.Drawing.Size(98, 30);
            this.nudOreFin.TabIndex = 7;
            this.nudOreFin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pnlFattoreCarica
            // 
            this.pnlFattoreCarica.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFattoreCarica.Controls.Add(this.nudFattoreCarica);
            this.pnlFattoreCarica.Location = new System.Drawing.Point(47, 170);
            this.pnlFattoreCarica.Name = "pnlFattoreCarica";
            this.pnlFattoreCarica.Size = new System.Drawing.Size(223, 78);
            this.pnlFattoreCarica.TabIndex = 15;
            // 
            // nudFattoreCarica
            // 
            this.nudFattoreCarica.DecimalPlaces = 2;
            this.nudFattoreCarica.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudFattoreCarica.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudFattoreCarica.Location = new System.Drawing.Point(63, 22);
            this.nudFattoreCarica.Maximum = new decimal(new int[] {
            130,
            0,
            0,
            131072});
            this.nudFattoreCarica.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFattoreCarica.Name = "nudFattoreCarica";
            this.nudFattoreCarica.Size = new System.Drawing.Size(97, 30);
            this.nudFattoreCarica.TabIndex = 14;
            this.nudFattoreCarica.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudFattoreCarica.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // pnlSceltaCorrente
            // 
            this.pnlSceltaCorrente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSceltaCorrente.Controls.Add(this.optCorrenteCapacita);
            this.pnlSceltaCorrente.Controls.Add(this.optCorrenteTempo);
            this.pnlSceltaCorrente.Location = new System.Drawing.Point(342, 170);
            this.pnlSceltaCorrente.Name = "pnlSceltaCorrente";
            this.pnlSceltaCorrente.Size = new System.Drawing.Size(223, 78);
            this.pnlSceltaCorrente.TabIndex = 16;
            // 
            // optCorrenteCapacita
            // 
            this.optCorrenteCapacita.AutoSize = true;
            this.optCorrenteCapacita.Location = new System.Drawing.Point(8, 40);
            this.optCorrenteCapacita.Name = "optCorrenteCapacita";
            this.optCorrenteCapacita.Size = new System.Drawing.Size(174, 21);
            this.optCorrenteCapacita.TabIndex = 1;
            this.optCorrenteCapacita.Text = "In base alla CAPACITA\'";
            this.optCorrenteCapacita.UseVisualStyleBackColor = true;
            // 
            // optCorrenteTempo
            // 
            this.optCorrenteTempo.AutoSize = true;
            this.optCorrenteTempo.Checked = true;
            this.optCorrenteTempo.Location = new System.Drawing.Point(8, 13);
            this.optCorrenteTempo.Name = "optCorrenteTempo";
            this.optCorrenteTempo.Size = new System.Drawing.Size(214, 21);
            this.optCorrenteTempo.TabIndex = 0;
            this.optCorrenteTempo.TabStop = true;
            this.optCorrenteTempo.Text = "In base al TEMPO disponibile";
            this.optCorrenteTempo.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(359, 147);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(165, 20);
            this.label8.TabIndex = 17;
            this.label8.Text = "Limitazione Corrente";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(93, 149);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 20);
            this.label7.TabIndex = 18;
            this.label7.Text = "Fattore di Carica";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(98, 352);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(172, 41);
            this.btnOK.TabIndex = 19;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnAnnulla
            // 
            this.btnAnnulla.Location = new System.Drawing.Point(342, 352);
            this.btnAnnulla.Name = "btnAnnulla";
            this.btnAnnulla.Size = new System.Drawing.Size(172, 41);
            this.btnAnnulla.TabIndex = 20;
            this.btnAnnulla.Text = "Annulla";
            this.btnAnnulla.UseVisualStyleBackColor = true;
            // 
            // frmPopupCambioTurmo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(631, 433);
            this.ControlBox = false;
            this.Controls.Add(this.btnAnnulla);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pnlSceltaCorrente);
            this.Controls.Add(this.pnlFattoreCarica);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pnlOrarioFine);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pnlOrarioInizio);
            this.Controls.Add(this.chkEqualizzazione);
            this.Controls.Add(this.chkPilotaggio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmPopupCambioTurmo";
            this.Text = "Impostazioni Cambio Turno ";
            this.pnlOrarioInizio.ResumeLayout(false);
            this.pnlOrarioInizio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutiIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOreIn)).EndInit();
            this.pnlOrarioFine.ResumeLayout(false);
            this.pnlOrarioFine.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutiFin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOreFin)).EndInit();
            this.pnlFattoreCarica.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudFattoreCarica)).EndInit();
            this.pnlSceltaCorrente.ResumeLayout(false);
            this.pnlSceltaCorrente.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkPilotaggio;
        private System.Windows.Forms.CheckBox chkEqualizzazione;
        private System.Windows.Forms.Panel pnlOrarioInizio;
        private System.Windows.Forms.NumericUpDown nudMinutiIn;
        private System.Windows.Forms.NumericUpDown nudOreIn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlOrarioFine;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudMinutiFin;
        private System.Windows.Forms.NumericUpDown nudOreFin;
        private System.Windows.Forms.Panel pnlFattoreCarica;
        private System.Windows.Forms.NumericUpDown nudFattoreCarica;
        private System.Windows.Forms.Panel pnlSceltaCorrente;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton optCorrenteCapacita;
        private System.Windows.Forms.RadioButton optCorrenteTempo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnAnnulla;
    }
}