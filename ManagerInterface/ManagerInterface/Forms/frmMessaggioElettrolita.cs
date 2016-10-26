using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using BrightIdeasSoftware;
using Utility;
using MoriData;
using ChargerLogic;
using MdiHelper;
using log4net;
using log4net.Config;
using static ChargerLogic.elementiComuni;



namespace PannelloCharger
{

    public partial class frmMessaggioElettrolita : Form
    {

        private UnitaSpyBatt _sbLocale;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        int _tastoPremuto = 0;
        EsitoControlloValore _esitoVal;

        private void inizializzaForm ()
        {
            InitializeComponent();
            _tastoPremuto = 0;
            pgbAvanzamento.Visible = false;

            //this.Width = 491;
            //this.Height = 306;
            //this.Show();
        }


        public frmMessaggioElettrolita()
        {
            try
            {
                inizializzaForm();
            }
            catch
            {

            }
           
        }

        public frmMessaggioElettrolita(UnitaSpyBatt SbLocale)
        {
            inizializzaForm();
            _sbLocale = SbLocale;

        }

        public EsitoControlloValore LanciaVerifica()
        {
            try
            {
                bool _elettrOk = false;
                int loopCicli = 0;
                System.Threading.Thread.Sleep(500);
                Application.DoEvents();

                pgbAvanzamento.Minimum = 0;
                pgbAvanzamento.Maximum = 100;

                if (!_sbLocale.apparatoPresente)
                {
                    _esitoVal = EsitoControlloValore.ErroreLetturaSB;
                    lblMessaggioChiusura.ForeColor = Color.Red;
                    lblMessaggioChiusura.Text = "SPY-BATT non collegato";
                    return _esitoVal;
                }

                pgbAvanzamento.Visible = true;

                while (!_elettrOk)
                {
                    loopCicli++;
                    if (loopCicli > 100)
                    {
                        lblMessaggioChiusura.ForeColor = Color.Red;
                        lblMessaggioChiusura.Text = "Tempo Attesa scaduto";
                        _esitoVal = EsitoControlloValore.Timeout;
                        return _esitoVal;

                    }
                    pgbAvanzamento.Value = loopCicli;
                    bool _esitoSB = _sbLocale.CaricaVariabili(_sbLocale.Id, _sbLocale.apparatoPresente);

                    if(_esitoSB)
                    {
                        if (_sbLocale.sbVariabili.PresenzaElettrolita == 0xF0)
                        {
                            lblMessaggioChiusura.ForeColor = Color.Green;
                            lblMessaggioChiusura.Text = "Test Sonda SUPERATO";
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(1000);
                            _esitoVal = EsitoControlloValore.EsitoPositivo;
                            return _esitoVal;
                        }
                    }
                    System.Threading.Thread.Sleep(100);
                    Application.DoEvents();

                }



                return EsitoControlloValore.NonEffettuato;

            }
            catch (Exception Ex)
            {
                Log.Error("frmMessaggioElettrolita.LanciaVerifica: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
                return EsitoControlloValore.ErroreGenerico;
            }
        }


        public EsitoControlloValore TastoPremuto
        {
            get
            {

                switch(_tastoPremuto)
                {
                    case 0:
                        {
                            return EsitoControlloValore.NonEffettuato;
                        }
                    case 1:
                        {
                            return EsitoControlloValore.IgnoraVerifica;
                        }
                    case 2:
                        {
                            return EsitoControlloValore.AnnullaVerifica;
                        }
                    default:
                        {
                            return EsitoControlloValore.NonEffettuato;
                        }
                }
                
            }
        }

        public EsitoControlloValore EsitoVerifica
        {
            get
            {
                return _esitoVal;
            }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            _tastoPremuto = 1;
            _esitoVal = EsitoControlloValore.IgnoraVerifica;

            this.Close();
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            _tastoPremuto = 2;
            _esitoVal = EsitoControlloValore.AnnullaVerifica;
            this.Close();
        }

        private void lblMessaggioChiusura_Click(object sender, EventArgs e)
        {

        }

        private void btnRiavvia_Click(object sender, EventArgs e)
        {
            pgbAvanzamento.Value = 0;
            lblMessaggioChiusura.Text = "";

            EsitoControlloValore _verifica = LanciaVerifica();
            if (_verifica == EsitoControlloValore.EsitoPositivo)
            {
                this.Close();
            }
        }

        private void frmMessaggioElettrolita_Load(object sender, EventArgs e)
        {
            this.Width = 400;
            this.Height = 250;
            //System.Threading.Thread.Sleep(500);
            Application.DoEvents();

        }

        private void frmMessaggioElettrolita_Shown(object sender, EventArgs e)
        {
            EsitoControlloValore _verifica = LanciaVerifica();
            if (_verifica == EsitoControlloValore.EsitoPositivo)
            {
                this.Close();
            }
        }
    }






}
