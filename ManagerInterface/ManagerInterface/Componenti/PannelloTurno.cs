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
    public class PannelloTurno :Panel
    {


        private Panel _pnlOrariCambio;
        private Panel _pnlOpzioniTurno;


        private MaskedTextBox _mtbInizioTurno;
        private MaskedTextBox _mtbFineTurno;

        private byte _oraInizioCambio;
        private byte _oraFineCambio;

        private byte _fattoreCarica = 100;

        private byte _opzioniTurno;

        private OraTurnoMR _InizioCambioTurno;
        private OraTurnoMR _FineCambioTurno;


        private Label lblModoTempo;
        private Label lblStatoBiber;
        private Label lblStatoEqual;
        private Label lblFcTurno;
        private NumericUpDown nudFcTurno;


        private bool _inEvidenza = false;

        private bool _solaLettura = false;

        private Panel _pnlParametri;

        private Color _backcolor = new Color();

        public PannelloTurno()
        {
            //_mtbInizioTurno = 0;
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.DoubleClick += new System.EventHandler(this.host_DoubleClick);
            initializeComponent();
        }

        private void initializeComponent()
        {
            try
            {
                _backcolor = Color.Transparent;

                // Inizializzazione oggetti
                _pnlOrariCambio = new Panel();
                _mtbInizioTurno = new MaskedTextBox();
                _mtbFineTurno = new MaskedTextBox();

                // 
                // pnlOrariCambio
                // 

                this._pnlOrariCambio.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this._pnlOrariCambio.Controls.Add(this._mtbFineTurno);
                this._pnlOrariCambio.Controls.Add(this._mtbInizioTurno);
                this._pnlOrariCambio.Location = new System.Drawing.Point(12, 9);
                this._pnlOrariCambio.Name = "_pnlOrariCambio";
                this._pnlOrariCambio.Size = new System.Drawing.Size(114, 30);
                this._pnlOrariCambio.TabIndex = 3;
                this.Controls.Add(_pnlOrariCambio);

                // 
                // mtbInizioTurno
                // 
                this._mtbInizioTurno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this._mtbInizioTurno.Location = new System.Drawing.Point(6, 4);
                this._mtbInizioTurno.Mask = "00:00";
                this._mtbInizioTurno.Name = "_mtbInizioTurno";
                this._mtbInizioTurno.Size = new System.Drawing.Size(48, 24);
                this._mtbInizioTurno.TabIndex = 0;
                this._mtbInizioTurno.ValidatingType = typeof(System.DateTime);
                this._mtbInizioTurno.Leave += new System.EventHandler(this.mtbInizioTurno_Leave);

                // 
                // mtbFineTurno
                // 
                this._mtbFineTurno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this._mtbFineTurno.Location = new System.Drawing.Point(58, 4);
                this._mtbFineTurno.Mask = "00:00";
                this._mtbFineTurno.Name = "_mtbFineTurno";
                this._mtbFineTurno.Size = new System.Drawing.Size(48, 24);
                this._mtbFineTurno.TabIndex = 1;
                this._mtbFineTurno.ValidatingType = typeof(System.DateTime);
                this._mtbFineTurno.Leave += new System.EventHandler(this.mtbFineTurno_Leave);



                _pnlOpzioniTurno = new Panel();
                lblModoTempo = new Label();
                lblStatoBiber = new Label();
                lblStatoEqual = new Label();
                lblFcTurno = new Label();
                nudFcTurno = new NumericUpDown();




                // 
                // pnlOpzioniTurno
                // 
                this._pnlOpzioniTurno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this._pnlOpzioniTurno.Controls.Add(this.lblModoTempo);
                this._pnlOpzioniTurno.Controls.Add(this.lblStatoBiber);
                this._pnlOpzioniTurno.Controls.Add(this.lblStatoEqual);
                this._pnlOpzioniTurno.Controls.Add(this.lblFcTurno);
                this._pnlOpzioniTurno.Controls.Add(this.nudFcTurno);
                this._pnlOpzioniTurno.Location = new System.Drawing.Point(138, 9);
                this._pnlOpzioniTurno.Name = "pnlOpzioniTurno";
                this._pnlOpzioniTurno.Size = new System.Drawing.Size(197, 30);
                this._pnlOpzioniTurno.TabIndex = 6;
                this.Controls.Add(_pnlOpzioniTurno);

                // 
                // lblFcTurno
                // 
                this.lblFcTurno.AutoSize = true;
                this.lblFcTurno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lblFcTurno.Location = new System.Drawing.Point(5, 6);
                this.lblFcTurno.Name = "lblFcTurno";
                this.lblFcTurno.Size = new System.Drawing.Size(37, 17);
                this.lblFcTurno.TabIndex = 3;
                this.lblFcTurno.Text = "F.C.";

                // 
                // lblModoTempo
                // 
                this.lblModoTempo.AutoSize = true;
                this.lblModoTempo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lblModoTempo.ForeColor = System.Drawing.Color.Green;
                this.lblModoTempo.Location = new System.Drawing.Point(174, 6);
                this.lblModoTempo.Name = "lblModoTempo";
                this.lblModoTempo.Size = new System.Drawing.Size(18, 24);
                this.lblModoTempo.TabIndex = 6;
                this.lblModoTempo.Text = "C";
                this.lblModoTempo.Click += new System.EventHandler(this.lblModoTempo_Click);
                // 
                // lblStatoBiber
                // 
                this.lblStatoBiber.AutoSize = true;
                this.lblStatoBiber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lblStatoBiber.ForeColor = System.Drawing.Color.Green;
                this.lblStatoBiber.Location = new System.Drawing.Point(149, 6);
                this.lblStatoBiber.Name = "lblStatoBiber";
                this.lblStatoBiber.Size = new System.Drawing.Size(17, 17);
                this.lblStatoBiber.TabIndex = 5;
                this.lblStatoBiber.Text = "B";
                this.lblStatoBiber.Click += new System.EventHandler(this.lblStatoBiber_Click);
                // 
                // lblStatoEqual
                // 
                this.lblStatoEqual.AutoSize = true;
                this.lblStatoEqual.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.lblStatoEqual.ForeColor = System.Drawing.Color.Green;
                this.lblStatoEqual.Location = new System.Drawing.Point(125, 6);
                this.lblStatoEqual.Name = "lblStatoEqual";
                this.lblStatoEqual.Size = new System.Drawing.Size(18, 17);
                this.lblStatoEqual.TabIndex = 4;
                this.lblStatoEqual.Text = "E";
                this.lblStatoEqual.Click += new System.EventHandler(this.lblStatoEqual_Click);

                // 
                // nudFcTurno
                // 
                this.nudFcTurno.DecimalPlaces = 2;
                this.nudFcTurno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.nudFcTurno.Increment = new decimal(new int[] { 1, 0, 0, 131072});
                this.nudFcTurno.Location = new System.Drawing.Point(44, 4);
                this.nudFcTurno.Maximum = new decimal(new int[] { 130, 0, 0, 131072});
                this.nudFcTurno.Minimum = new decimal(new int[] {   1, 0, 0,      0});
                this.nudFcTurno.Name = "nudFcTurno";
                this.nudFcTurno.Size = new System.Drawing.Size(68, 22);
                this.nudFcTurno.TabIndex = 1;
                this.nudFcTurno.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                this.nudFcTurno.Value = new decimal(new int[] { 1, 0, 0, 0});

                this.BackColor = _backcolor;
                this.Dock = DockStyle.Fill;
                this.Margin = new Padding(1);
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


        public bool SolaLettura
        {
            get
            {
                return _solaLettura;
            }

            set
            {
                _solaLettura = value;
                applicaSolaLettura(_solaLettura);
            }

        }


        private void applicaSolaLettura(bool stato)
        {
            try
            {
               _mtbInizioTurno.ReadOnly = stato;
               _mtbFineTurno.ReadOnly = stato;
               nudFcTurno.ReadOnly = stato;
                nudFcTurno.Enabled = !stato;
            }
            catch
            {

            }
        }


        private void host_DoubleClick(object sender, EventArgs e)
        {
            InEvidenza = !InEvidenza;
            SolaLettura = InEvidenza;
        }


        private void lblStatoEqual_Click(object sender, EventArgs e)
        {
            try
            {
                Label _lble = (Label)sender;
                FontStyle _fst; //= new FontStyle();
                Font _fnt; // = new Font(_lble.Font , _fst);

                if (_solaLettura) return;

                if (_lble.ForeColor == Color.Red)
                {
                    // è disabilitato, abilito
                    _fst = new FontStyle();
                    _fst = FontStyle.Regular | FontStyle.Bold;

                    _fnt = new Font(_lble.Font, _fst);
                    _lble.ForeColor = Color.Green;
                    _lble.Font = _fnt;

                }
                else
                {
                    // è disabilitato, abilito
                    _fst = new FontStyle();
                    _fst = FontStyle.Regular | FontStyle.Italic;

                    _fnt = new Font(_lble.Font, _fst);
                    _lble.ForeColor = Color.Red;
                    _lble.Font = _fnt;

                }
            }
            catch
            {

            }
        }

        private void lblModoTempo_Click(object sender, EventArgs e)
        {
            try
            {
                Label _lble = (Label)sender;
                FontStyle _fst; //= new FontStyle();
                Font _fnt; // = new Font(_lble.Font , _fst);
                if (_solaLettura) return;

                if (_lble.Text == "C")
                {
                    // è disabilitato, abilito
                    _fst = new FontStyle();
                    _fst = FontStyle.Regular | FontStyle.Bold;

                    _fnt = new Font(_lble.Font, _fst);
                    _lble.ForeColor = Color.Green;
                    _lble.Font = _fnt;
                    _lble.Text = "T";

                }
                else
                {
                    // è disabilitato, abilito
                    _fst = new FontStyle();
                    _fst = FontStyle.Regular | FontStyle.Bold;

                    _fnt = new Font(_lble.Font, _fst);
                    _lble.ForeColor = Color.Green;
                    _lble.Font = _fnt;
                    _lble.Text = "C";

                }
            }
            catch
            {

            }
        }

        private void lblStatoBiber_Click(object sender, EventArgs e)
        {
            try
            {
                Label _lble = (Label)sender;
                FontStyle _fst; //= new FontStyle();
                Font _fnt; // = new Font(_lble.Font , _fst);

                if (_solaLettura) return;

                if (_lble.ForeColor == Color.Red)
                {
                    // è disabilitato, abilito
                    _fst = new FontStyle();
                    _fst = FontStyle.Regular | FontStyle.Bold;

                    _fnt = new Font(_lble.Font, _fst);
                    _lble.ForeColor = Color.Green;
                    _lble.Font = _fnt;

                }
                else
                {
                    // è disabilitato, abilito
                    _fst = new FontStyle();
                    _fst = FontStyle.Regular | FontStyle.Italic;

                    _fnt = new Font(_lble.Font, _fst);
                    _lble.ForeColor = Color.Red;
                    _lble.Font = _fnt;

                }
            }
            catch
            {

            }
        }

        private void mtbInizioTurno_Leave(object sender, EventArgs e)
        {
            VerificaStatoTurno();
        }

        private void mtbFineTurno_Leave(object sender, EventArgs e)
        {
            VerificaStatoTurno();
        }

        

        private bool VerificaStatoTurno()
        {
            bool StatoTurno = false;
            try
            {
                if (IsValidTime(_mtbInizioTurno.Text) || IsValidTime(_mtbFineTurno.Text))
                {
                    StatoTurno = true;
                }
                else
                {
                    StatoTurno = false;
                }

                _pnlOpzioniTurno.Enabled = StatoTurno;
                InEvidenza = StatoTurno;


                return StatoTurno;
            }
            catch
            {
                return true;
            }
        }

        private bool IsValidTime(string Value)
        {

            try
            {
                string tempVal = Value.Trim();
                if (tempVal == ":")
                {
                    return false;
                }

                return true;

            }
            catch
            {
                return false;
            }
        }



        public Color Backcolor
        {
            get
            {
                return _backcolor;
            }

            set
            {
                _backcolor = value;
                base.BackColor = _backcolor;
            }

        }


        public bool InEvidenza
        {
            get
            {
                return _inEvidenza;
            }

            set
            {
                _inEvidenza = value;
                this.Invalidate(); ;
            }


        }




        public OraTurnoMR InizioCambioTurno
        {
            get
            {
                return _InizioCambioTurno;
            }
            set
            {
                _InizioCambioTurno = value;
                _mtbInizioTurno.Text = _InizioCambioTurno.Ore.ToString("00") + ":" + _InizioCambioTurno.Minuti.ToString("00");
            }
        }
        public OraTurnoMR FineCambioTurno
        {
            get
            {
                return _FineCambioTurno;
            }
            set
            {
                _FineCambioTurno = value;
                _mtbFineTurno.Text = _FineCambioTurno.Ore.ToString("00") + ":" + _FineCambioTurno.Minuti.ToString("00");
            }
        }

    }



}
