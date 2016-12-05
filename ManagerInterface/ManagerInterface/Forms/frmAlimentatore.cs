using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace PannelloCharger
{
    public partial class frmAlimentatore : Form
    {
        SerialPort _serialPort;
        public NumberFormatInfo NfiEn = new CultureInfo("en-US", false).NumberFormat;

        //bool _WaitForMsg = false;
        //string _LastMsg = "";

        public AlimentatoreTdk Alimentatatore;

        public frmAlimentatore()
        {
            InitializeComponent();
            Alimentatatore = new AlimentatoreTdk();
        }


        // delegate is used to write to a UI control from a non-UI thread
        private delegate void SetTextDeleg(string text);


        private void btnStart_Click(object sender, EventArgs e)
        {
            // Makes sure serial port is open before trying to write
            try
            {
                if (!_serialPort.IsOpen)
                    _serialPort.Open();

                _serialPort.Write("SI\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening/writing to serial port :: " + ex.Message, "Error!");
            }
        }






        private void InizializzaSeriale(string Porta)
        {
            try
            {
                if (Porta != "")
                {
                    _serialPort = new SerialPort(Porta, 9600, Parity.None, 8, StopBits.One);
                    _serialPort.Handshake = Handshake.None;
                    //_serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                    _serialPort.ReadTimeout = 500;
                    _serialPort.WriteTimeout = 500;
                    btnStatoCom.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("InizializzaSeriale: " + ex.Message, " Error!");
            }
        }


        public bool ApriPorta()
        {
            bool _portaAttiva = false;

            try
            {
                Alimentatatore.ApriPorta();
                _portaAttiva = Alimentatatore.PortaConnessa;
                if (_portaAttiva)
                {
                    txtStatoPorta.Text = "OPEN";
                    btnStatoCom.Text = "Chiudi Conn";
                }

                return _portaAttiva;
            }

            catch
            {
                return _portaAttiva;
            }
        }

        public bool ChiudiPorta()
        {
            bool _portaAttiva = false;

            try
            {
                Alimentatatore.ChiudiPorta();
                _portaAttiva = Alimentatatore.PortaConnessa;
                if (!_portaAttiva)
                {
                    txtStatoPorta.Text = "CLOSE";
                    btnStatoCom.Text = "Apri Conn";
                }

                return _portaAttiva;
            }

            catch
            {
                return _portaAttiva;
            }
        }

        public bool SetVOut(float Vout)
        {
            try
            {
                /*
                float _Vout = Vout;
                string _comando = "";
                txtTdkVSetCheck.Text = "";

                if (_Vout > 400) _Vout = 400;
                if (_Vout < 0) _Vout = 0;
                _comando = "PV " + _Vout.ToString("00.000", NfiEn);
                */


                return false;
            }
            catch
            {
                return false;
            }
        }




        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            bool _esito;
            if (txtComPort.Text != "")
            {
                _esito = Alimentatatore.InizializzaSeriale(txtComPort.Text);
                btnStatoCom.Enabled = _esito;
            }
        }

        private void btnStatoCom_Click(object sender, EventArgs e)
        {
            try
            {
                if (Alimentatatore.PortaPresente)
                    if (Alimentatatore.PortaConnessa) ChiudiPorta();
                    else ApriPorta();
            }
            catch
            {

            }
        }









        private void btnVerificaTDK_Click(object sender, EventArgs e)
        {
            bool _esito;

            txtStatCom.Text = "";
            txtComApparato.Text = "";

            if (Alimentatatore.PortaConnessa)
            {

                _esito = Alimentatatore.apparatoPresente(6);
                txtStatCom.Text = Alimentatatore.LastMessage;
                if (_esito)
                {
                    _esito = Alimentatatore.Modello();
                    txtComApparato.Text = Alimentatatore.LastMessage;
                }

                MostraStato();

            }
        }

        public void AspettaRisposta(int Timeout = 1000)
        {
            try
            {

            }
            catch
            {

            }
        }

        private void btnLeggiTensione_Click(object sender, EventArgs e)
        {
            MostraTensioni();
        }

        public void MostraTensioni()
        {
            try
            {
                bool _esito;

                txtTdkVSetCheck.Text = "";
                txtTdkVCheck.Text = "";
                if (Alimentatatore.PortaConnessa)
                {
                    _esito = Alimentatatore.LeggiTensioni();
                    if (_esito)
                    {
                        txtTdkVSetCheck.Text = Alimentatatore.strVimpostati;
                        txtTdkVCheck.Text = Alimentatatore.strVrilevati;
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("MostraTensioni: " + ex.Message, " Error!");
            }
        }

        public void MostraCorrenti()
        {
            try
            {
                bool _esito;

                txtTdkASetCheck.Text = "";
                txtTdkACheck.Text = "";
                if (Alimentatatore.PortaConnessa)
                {
                    _esito = Alimentatatore.LeggiCorrenti();
                    if (_esito)
                    {
                        txtTdkASetCheck.Text = Alimentatatore.strAimpostati;
                        txtTdkACheck.Text = Alimentatatore.strArilevati;
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("MostraTensioni: " + ex.Message, " Error!");
            }
        }

        public void MostraStato()
        {
            try
            {
                bool _esito;


                if (Alimentatatore.PortaConnessa)
                {
                    _esito = Alimentatatore.LeggiStato();
                    if (_esito)
                    {

                        if (Alimentatatore.UscitaAttiva)
                        {
                            txtStato.Text = "ATTIVO";
                            txtStato.ForeColor = Color.Green;
                            btnAttivaUscita.Text = "FERMA";
                        }
                        else
                        {
                            txtStato.Text = "SPENTO";
                            txtStato.ForeColor = Color.Red;
                            btnAttivaUscita.Text = "ATTIVA";
                        }
                    }
                }

                else
                {
                    txtStato.Text = "SCOLLEGATO";
                    txtStato.ForeColor = Color.Gray;
                }





            }
            catch (Exception ex)
            {
                MessageBox.Show("MostraTensioni: " + ex.Message, " Error!");
            }
        }

        private void btnAttivaUscita_Click(object sender, EventArgs e)
        {
            bool _statoCorrente;

            _statoCorrente = Alimentatatore.UscitaAttiva;
            Alimentatatore.ImpostaStato(!_statoCorrente);
            MostraStato();
            MostraTensioni();
            MostraCorrenti();
        }

        private void btnImpostaTensione_Click(object sender, EventArgs e)
        {
            try
            {
                float _tensione;
                bool _setting;

                _setting = float.TryParse(txtTdkVSet.Text, out _tensione);
                if (_setting)
                {
                    Alimentatatore.ImpostaTensione(_tensione);
                    MostraTensioni();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ImpostaTensione: " + ex.Message, " Error!");
            }
        }

        private void btnLeggiCorrente_Click(object sender, EventArgs e)
        {
            MostraCorrenti();
        }

        private void btnImpostaCorrente_Click(object sender, EventArgs e)
        {
            try
            {
                float _corrente;
                bool _setting;

                _setting = float.TryParse(txtTdkASet.Text, out _corrente);
                if (_setting)
                {
                    Alimentatatore.ImpostaCorrente(_corrente);
                    MostraCorrenti();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ImpostaTensione: " + ex.Message, " Error!");
            }
        }

        private void frmAlimentatore_Load(object sender, EventArgs e)
        {

        }
    }





}

