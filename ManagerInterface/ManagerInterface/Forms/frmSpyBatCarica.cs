using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PannelloCharger
{
    public partial class frmSpyBatCarica : Form
    {
        public frmSpyBatCarica()
        {
            InitializeComponent();
        }

        private void btnChiudi_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
