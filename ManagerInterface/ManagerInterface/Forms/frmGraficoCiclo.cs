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
    public partial class frmGraficoCiclo : Form
    {
        public frmGraficoCiclo()
        {
            InitializeComponent();
        }

        public void TipoGrafico(int Id)
        {
            if (Id == 0)
            {
                pbCicloIWa.Visible = true;
                pbCicloIUIa.Visible = false;

            }
            else
            {
                pbCicloIWa.Visible = false;
                pbCicloIUIa.Visible = true;

            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
