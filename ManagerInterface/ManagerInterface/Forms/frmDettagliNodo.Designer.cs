namespace PannelloCharger
{
    partial class frmDettagliNodo
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
            this.btnSave = new System.Windows.Forms.Button();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtParentName = new System.Windows.Forms.TextBox();
            this.txtParentGuid = new System.Windows.Forms.TextBox();
            this.cmbTipoNodo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescrizione = new System.Windows.Forms.TextBox();
            this.txtCurrentGuid = new System.Windows.Forms.TextBox();
            this.lblLocalGuid = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAnnulla
            // 
            this.btnAnnulla.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAnnulla.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAnnulla.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAnnulla.Location = new System.Drawing.Point(305, 285);
            this.btnAnnulla.Name = "btnAnnulla";
            this.btnAnnulla.Size = new System.Drawing.Size(125, 31);
            this.btnAnnulla.TabIndex = 12;
            this.btnAnnulla.Text = "Annulla";
            this.btnAnnulla.UseVisualStyleBackColor = true;
            this.btnAnnulla.Click += new System.EventHandler(this.btnAnnulla_Click);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSave.Location = new System.Drawing.Point(174, 285);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(125, 31);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "OK";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPassword.Location = new System.Drawing.Point(29, 96);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(45, 17);
            this.lblPassword.TabIndex = 10;
            this.lblPassword.Text = "Nome";
            // 
            // txtNome
            // 
            this.txtNome.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.Location = new System.Drawing.Point(32, 116);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(352, 27);
            this.txtNome.TabIndex = 9;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblUsername.Location = new System.Drawing.Point(29, 26);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(114, 17);
            this.lblUsername.TabIndex = 8;
            this.lblUsername.Text = "Livello Superiore";
            // 
            // txtParentName
            // 
            this.txtParentName.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParentName.Location = new System.Drawing.Point(32, 46);
            this.txtParentName.MaxLength = 255;
            this.txtParentName.Name = "txtParentName";
            this.txtParentName.ReadOnly = true;
            this.txtParentName.Size = new System.Drawing.Size(282, 21);
            this.txtParentName.TabIndex = 7;
            // 
            // txtParentGuid
            // 
            this.txtParentGuid.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtParentGuid.Location = new System.Drawing.Point(320, 46);
            this.txtParentGuid.MaxLength = 255;
            this.txtParentGuid.Name = "txtParentGuid";
            this.txtParentGuid.ReadOnly = true;
            this.txtParentGuid.Size = new System.Drawing.Size(236, 21);
            this.txtParentGuid.TabIndex = 14;
            // 
            // cmbTipoNodo
            // 
            this.cmbTipoNodo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipoNodo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbTipoNodo.FormattingEnabled = true;
            this.cmbTipoNodo.Location = new System.Drawing.Point(397, 117);
            this.cmbTipoNodo.Name = "cmbTipoNodo";
            this.cmbTipoNodo.Size = new System.Drawing.Size(159, 26);
            this.cmbTipoNodo.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(394, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 16;
            this.label1.Text = "Tipo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(29, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "Descrizione";
            // 
            // txtDescrizione
            // 
            this.txtDescrizione.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescrizione.Location = new System.Drawing.Point(32, 173);
            this.txtDescrizione.Name = "txtDescrizione";
            this.txtDescrizione.Size = new System.Drawing.Size(524, 22);
            this.txtDescrizione.TabIndex = 17;
            // 
            // txtCurrentGuid
            // 
            this.txtCurrentGuid.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentGuid.Location = new System.Drawing.Point(320, 228);
            this.txtCurrentGuid.MaxLength = 255;
            this.txtCurrentGuid.Name = "txtCurrentGuid";
            this.txtCurrentGuid.ReadOnly = true;
            this.txtCurrentGuid.Size = new System.Drawing.Size(236, 21);
            this.txtCurrentGuid.TabIndex = 19;
            // 
            // lblLocalGuid
            // 
            this.lblLocalGuid.AutoSize = true;
            this.lblLocalGuid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblLocalGuid.Location = new System.Drawing.Point(317, 208);
            this.lblLocalGuid.Name = "lblLocalGuid";
            this.lblLocalGuid.Size = new System.Drawing.Size(42, 17);
            this.lblLocalGuid.TabIndex = 20;
            this.lblLocalGuid.Text = "GUID";
            // 
            // frmDettagliNodo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(585, 344);
            this.Controls.Add(this.lblLocalGuid);
            this.Controls.Add(this.txtCurrentGuid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescrizione);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbTipoNodo);
            this.Controls.Add(this.txtParentGuid);
            this.Controls.Add(this.btnAnnulla);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.lblUsername);
            this.Controls.Add(this.txtParentName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDettagliNodo";
            this.ShowIcon = false;
            this.Text = "Nodo";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmDettagliNodo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAnnulla;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtParentName;
        private System.Windows.Forms.TextBox txtParentGuid;
        private System.Windows.Forms.ComboBox cmbTipoNodo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDescrizione;
        private System.Windows.Forms.TextBox txtCurrentGuid;
        private System.Windows.Forms.Label lblLocalGuid;
    }
}