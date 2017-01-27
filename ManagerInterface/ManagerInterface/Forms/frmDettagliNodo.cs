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
                    if (NodoCorrente.ParentGuid != "")
                    {
                        txtParentName.Text = NodoCorrente.ParentName;
                        txtParentGuid.Text = NodoCorrente.ParentGuid;
                    }
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

        public bool DatiValidi()
        {
            bool _esito = false;
            try
            {

                if (txtNome.Text == "")
                {
                    txtNome.Focus();
                    return false;
                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("frmDettagliNodo.DatiValidi: " + Ex.Message);
                return _esito;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (DatiValidi())
                {
                    NodoCorrente.Nome = txtNome.Text;
                    NodoCorrente.Descrizione = txtDescrizione.Text;




                    NodoCorrente.Tipo = NodoStruttura.TipoNodo.Ramo;
                    NodoCorrente.Icona = "folder";
                    NodoCorrente.IdApparato = null;




                    if (NodoCorrente.ParentGuid != "")
                    {
                        NodoCorrente.ParentGuid = txtParentGuid.Text ;
                    }
                    if (NodoCorrente.Guid == "")
                        txtCurrentGuid.Text = NodoCorrente.NuovoGuid();


                    if (NodoCorrente.salvaDati())
                        this.Close();

                    txtCurrentGuid.Text = NodoCorrente.Guid;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("frmDettagliNodo.btnSave_Click: " + Ex.Message);
            }

        }
    }
}
