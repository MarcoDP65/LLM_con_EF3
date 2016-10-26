using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PannelloCharger
{
    public partial class frmRichiestaCodice : Form
    {
        public string CodiceAutorizzazione = "";

        public frmRichiestaCodice()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            CodiceAutorizzazione = txtCodice.Text;
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            CodiceAutorizzazione = "";
        }
    }
}
