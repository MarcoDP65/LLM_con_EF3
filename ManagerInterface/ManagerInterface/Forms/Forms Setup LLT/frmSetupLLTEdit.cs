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
    public partial class frmSetupLLTEdit : Form
    {
        public frmSetupLLTEdit()
        {
            InitializeComponent();
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
