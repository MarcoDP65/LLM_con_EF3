using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PannelloCharger
{
    public class PanelTestataColonnaTurno : Panel
    {
        private Color _backcolor = new Color();
        private Color _forecolor = new Color();
        private string _titolo;
        private Label lblTitoloTurno;
        private bool _inEvidenza = false;

        public PanelTestataColonnaTurno(string Titolo)
        {
            _titolo = Titolo;
            CreaClasse();

        }


        public PanelTestataColonnaTurno()
        {
            CreaClasse();
        }


        private bool CreaClasse()
        {
            //_mtbInizioTurno = 0;
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            initializeComponent();
            return true;
        }


        private void initializeComponent()
        {
            try
            {
                _backcolor = Color.DimGray;
                _forecolor = Color.White;

                lblTitoloTurno = new Label();

                this.Dock = DockStyle.Fill;
                this.Margin = new Padding(1);
                this.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left);
                this.BackColor = _backcolor;
                this.ForeColor = _forecolor;


                this.Controls.Add(lblTitoloTurno);

                // 
                // lblFcTurno
                // 
                this.lblTitoloTurno.AutoSize = false;
                this.lblTitoloTurno.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                this.lblTitoloTurno.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left);
                this.lblTitoloTurno.TextAlign = ContentAlignment.MiddleCenter;

                this.lblTitoloTurno.Text = _titolo;

               // lblTitoloTurno.BackColor = Color.Gray;
                lblTitoloTurno.ForeColor = Color.White;
                lblTitoloTurno.Width = this.Width;
                lblTitoloTurno.Height = this.Height;
                //this.Invalidate(true);
            }
            catch
            {

            }


        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_inEvidenza)
            {

                using (SolidBrush brush = new SolidBrush(BackColor))
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                e.Graphics.DrawRectangle(Pens.Red, 0, 0, ClientSize.Width - 2, ClientSize.Height - 2);
            }

        }


    }
}
