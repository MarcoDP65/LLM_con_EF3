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
    public partial class frmInputDialog : Form
    {
        public string Messaggio = "Valore";
        public string Valore = "";
        public string Titolo = "Inserire il testo";
        int _tastoPremuto = 0;

        private void Inizializza()
        {
            InitializeComponent();
            lblMessaggio.Text = Messaggio;
            txtMessaggio.Text = Valore;
            this.Text = Titolo;
            _tastoPremuto = 0;
        }

        public frmInputDialog()
        {
            Inizializza();
        }

        public frmInputDialog(string Richiesta)
        {
            Messaggio = Richiesta;
            Inizializza();
             
        }

        public frmInputDialog(string Richiesta, string TitoloFinestra)
        {
            Messaggio = Richiesta;
            Titolo = TitoloFinestra;
            Inizializza();

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Valore = txtMessaggio.Text;
            _tastoPremuto = 1;
            this.Close();
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            _tastoPremuto = 2;
            this.Close();
        }

        private void frmInputDialog_Activated(object sender, EventArgs e)
        {
            this.ActiveControl = txtMessaggio;
        }

        public EsitoControlloValore TastoPremuto
        {
            get
            {

                switch (_tastoPremuto)
                {
                    case 0:
                        {
                            return EsitoControlloValore.NonEffettuato;
                        }
                    case 1:
                        {
                            return EsitoControlloValore.EsitoPositivo;
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

        private void frmInputDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
