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
    public partial class frmDettagliNodo : Form
    {
        public NodoStruttura NodoCorrente { get; set; }
        public NodoStruttura NodoPadre { get; set; }


        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");



        public frmDettagliNodo()
        {
            InitializeComponent();
        }

        public frmDettagliNodo(string ParentGuid, string Parentname)
        {
            try
            {
                InitializeComponent();

                txtParentName.Text = Parentname;
                txtParentGuid.Text = ParentGuid;

            }

            catch
            {

            }

        }

        public void MostraValori()
        {
            try
            {
                txtParentName.Text = "";
                txtParentGuid.Text = "";
                if(NodoPadre != null)
                {
                    txtParentName.Text = NodoPadre.Nome;
                    txtParentGuid.Text = NodoPadre.Guid;
                }

                txtNome.Text = "";
                txtDescrizione.Text = "";
                txtCurrentGuid.Text = "";
                if(NodoCorrente != null)
                {
                    txtNome.Text = NodoCorrente.Nome;
                    txtDescrizione.Text = NodoCorrente.Descrizione;
                    txtCurrentGuid.Text = NodoCorrente.Guid;
                }





            }
            catch (Exception Ex)
            {
                Log.Error("frmDettagliNodo.MostraValori: " + Ex.Message);
            }

        }



        private void frmDettagliNodo_Load(object sender, EventArgs e)
        {

        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }

            catch (Exception Ex)
            {
                Log.Error("frmDettagliNodo.btnAnnulla_Click: " + Ex.Message);
            }


        }
    }
}
