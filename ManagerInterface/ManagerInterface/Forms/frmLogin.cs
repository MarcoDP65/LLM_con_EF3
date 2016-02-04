using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using ChargerLogic;


namespace PannelloCharger
{
    public partial class frmLogin : Form
    {
        //private int esitoLogin = -1;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        private LogicheBase Logiche;
        public parametriSistema vGlobali ;

        public frmLogin()
        {
            try
            {
               // vGlobali.CaricaImpostazioniDefault();
                InitializeComponent();
                Log.Debug("Creazione Form Login");
            }
            catch (Exception Ex)
            {
                Log.Error("frmLogin: " + Ex.Message);
            }

        }
        public frmLogin(ref LogicheBase _logiche)
        {
            try
            {
               // vGlobali.CaricaImpostazioniDefault();
                Logiche = _logiche;
                InitializeComponent();
                Log.Debug("Creazione Form Login");
            }
            catch (Exception Ex)
            {
                Log.Error("frmLogin: " + Ex.Message);
            }
        }


        public frmLogin(ref LogicheBase _logiche, ref parametriSistema VarGlobali)
        {
            try
            {
                // vGlobali.CaricaImpostazioniDefault();
                vGlobali = VarGlobali;
                Thread.CurrentThread.CurrentUICulture = vGlobali.currentCulture;
                Logiche = _logiche;
                InitializeComponent();
                string path = Directory.GetCurrentDirectory();
                txtPath.Text = path;

                Log.Debug("Creazione Form Login - " + vGlobali.currentCultureValue);
            }
            catch (Exception Ex)
            {
                Log.Error("frmLogin: " + Ex.Message);
            }
        }
        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {

                Log.Debug("LOAD Form Login");
                

                if (vGlobali != null)
                {
                    chkSalvaPassword.Checked = vGlobali.currentSaveLogin;
                    txtUsername.Text = vGlobali.currentUser;
                    if (vGlobali.currentSaveLogin)
                    {
                        txtPassword.Text = vGlobali.currentPassword;
                    }
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmLogin_Load: " + Ex.Message);
            }
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
            catch //(Exception Ex)
            {
               // Log.Error("btnAnnulla_Click: " + Ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {

                Log.Debug("Form Login --> OK");

                if ((txtUsername.Text != "") & (txtPassword.Text != ""))
                {
                    int esito = Logiche.currentUser.verificaUtente(txtUsername.Text, txtPassword.Text);
                    if (esito >= 0)
                    {

                        vGlobali.currentUser = txtUsername.Text ;
                        vGlobali.currentPassword = txtPassword.Text;
                        vGlobali.currentSaveLogin = chkSalvaPassword.Checked;
                        vGlobali.SalvaImpostazioniDefault();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                Log.Error("btnLogin_Click: " + Ex.Message);
            }
        }

        private void txtPath_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                ProcessStartInfo processo = new ProcessStartInfo();
                processo.WorkingDirectory = path;
                processo.UseShellExecute = true;
                processo.FileName = path;
                processo.Verb = "open";
                Process.Start(processo);
            }
            catch(Exception Ex)
            {
                Log.Error("btnLogin_Click: " + Ex.Message);
            }
        }
    }
}
