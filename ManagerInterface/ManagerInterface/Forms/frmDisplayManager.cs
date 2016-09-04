using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BrightIdeasSoftware;

using ChargerLogic;
using log4net;
using log4net.Config;

using MoriData;
using Utility;

namespace PannelloCharger
{
    public partial class frmDisplayManager : Form
    {

        SerialPort ComPort;
        //public parametriSistema vGlobali;

        private Queue<byte> recievedData = new Queue<byte>();

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
        internal delegate void SerialPinChangedEventHandlerDelegate(object sender, SerialPinChangedEventArgs e);
        private SerialPinChangedEventHandler SerialPinChangedEventHandler1;
        delegate void SetTextCallback(string text);
        string InputData = String.Empty;
        byte[] _dataBuffer;
        int lastByte = 0;
        bool readingMessage = false;

        public UnitaDisplay _disp;

        public frmDisplayManager()
        {
            InitializeComponent();
            try
            {
                SerialPinChangedEventHandler1 = new SerialPinChangedEventHandler(PinChanged);
                ComPort = new SerialPort();
                _disp = new UnitaDisplay( ref ComPort);
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void btnGetSerialPorts_Click(object sender, EventArgs e)
        {
            try
            {

                string[] ArrayComPortsNames = null;
                int index = -1;
                int _currIndex;
                string ComPortName = null;
                bool _portaPresente = false;
                string _portaBase = PannelloCharger.Properties.Settings.Default.PortaDisplay;
                string _baudRate = PannelloCharger.Properties.Settings.Default.BaudRateDisplay;
                ArrayComPortsNames = SerialPort.GetPortNames();
                cboPorts.Items.Clear();
                cboBaudRate.Items.Clear();
                cboDataBits.Items.Clear();
                cboStopBits.Items.Clear();
                cboParity.Items.Clear();
                cboHandShaking.Items.Clear();

                if (ArrayComPortsNames.Length > 0)
                {

                    do
                    {
                        index += 1;
                        cboPorts.Items.Add(ArrayComPortsNames[index]);
                        if (ArrayComPortsNames[index] == _portaBase )
                        {
                            _portaPresente = true;
                            _currIndex = index;
                        }


                    } while (!((ArrayComPortsNames[index] == ComPortName) || (index == ArrayComPortsNames.GetUpperBound(0))));
                    Array.Sort(ArrayComPortsNames);

                    if (index == ArrayComPortsNames.GetUpperBound(0))
                    {
                        if (_portaPresente)
                            ComPortName = _portaBase;
                        else
                            ComPortName = ArrayComPortsNames[0];
                    }
                    //get first item print in text
                    cboPorts.Text = ComPortName; // ArrayComPortsNames[0];
                    //Baud Rate
                    cboBaudRate.Items.Add(300);
                    cboBaudRate.Items.Add(600);
                    cboBaudRate.Items.Add(1200);
                    cboBaudRate.Items.Add(2400);
                    cboBaudRate.Items.Add(9600);
                    cboBaudRate.Items.Add(14400);
                    cboBaudRate.Items.Add(19200);
                    cboBaudRate.Items.Add(38400);
                    cboBaudRate.Items.Add(57600);
                    cboBaudRate.Items.Add(115200);
                    cboBaudRate.Items.ToString();
                    //get first item print in text
                    cboBaudRate.Text = _baudRate;  //cboBaudRate.Items[0].ToString();
                    //Data Bits
                    cboDataBits.Items.Add(7);
                    cboDataBits.Items.Add(8);
                    //get the first item print it in the text 
                    cboDataBits.Text = "8"; //cboDataBits.Items[0].ToString();

                    //Stop Bits
                    cboStopBits.Items.Add("One");
                    cboStopBits.Items.Add("OnePointFive");
                    cboStopBits.Items.Add("Two");
                    //get the first item print in the text
                    cboStopBits.Text = "One"; //cboStopBits.Items[0].ToString();
                    //Parity 
                    cboParity.Items.Add("None");
                    cboParity.Items.Add("Even");
                    cboParity.Items.Add("Mark");
                    cboParity.Items.Add("Odd");
                    cboParity.Items.Add("Space");
                    //get the first item print in the text
                    cboParity.Text = "None"; //cboParity.Items[0].ToString();
                    //Handshake
                    cboHandShaking.Items.Add("None");
                    cboHandShaking.Items.Add("XOnXOff");
                    cboHandShaking.Items.Add("RequestToSend");
                    cboHandShaking.Items.Add("RequestToSendXOnXOff");
                    //get the first item print it in the text 
                    cboHandShaking.Text = "None"; //cboHandShaking.Items[0].ToString();
                }

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }




        private void SetText(string text)
        {
            this.rtbIncoming.Text += text;
            this.rtbhex.Text += text;
        }

        internal void PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            SerialPinChange SerialPinChange1 = 0;
            bool signalState = false;

            SerialPinChange1 = e.EventType;
            lblCTSStatus.BackColor = Color.Green;
            lblDSRStatus.BackColor = Color.Green;
            lblRIStatus.BackColor = Color.Green;
            lblBreakStatus.BackColor = Color.Green;
            switch (SerialPinChange1)
            {
                case SerialPinChange.Break:
                    lblBreakStatus.BackColor = Color.Red;
                    //MessageBox.Show("Break is Set");
                    break;
                case SerialPinChange.CDChanged:
                    signalState = ComPort.CtsHolding;
                    //  MessageBox.Show("CD = " + signalState.ToString());
                    break;
                case SerialPinChange.CtsChanged:
                    signalState = ComPort.CDHolding;
                    lblCTSStatus.BackColor = Color.Red;
                    //MessageBox.Show("CTS = " + signalState.ToString());
                    break;
                case SerialPinChange.DsrChanged:
                    signalState = ComPort.DsrHolding;
                    lblDSRStatus.BackColor = Color.Red;
                    // MessageBox.Show("DSR = " + signalState.ToString());
                    break;
                case SerialPinChange.Ring:
                    lblRIStatus.BackColor = Color.Red;
                    //MessageBox.Show("Ring Detected");
                    break;
            }
        }

        private void btnPortState_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnPortState.Text == "Apri Porta")
                {
                    //btnPortState.Text = "Chiudi";
                    ComPort.PortName = Convert.ToString(cboPorts.Text);
                    ComPort.BaudRate = Convert.ToInt32(cboBaudRate.Text);
                    ComPort.DataBits = Convert.ToInt16(cboDataBits.Text);
                    ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cboStopBits.Text);
                    ComPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), cboHandShaking.Text);
                    ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), cboParity.Text);
                    _dataBuffer = new byte[0];
                    ComPort.Open();
                }
                else if (btnPortState.Text == "Chiudi")
                {
                    //btnPortState.Text = "Apri Porta";
                    ComPort.Close();

                }

                if (ComPort.IsOpen)
                {
                    // Se la porta è aperta blocco tutto
                    btnPortState.Text = "Chiudi";
                    cboPorts.Enabled = false;
                    cboBaudRate.Enabled = false;
                    cboDataBits.Enabled = false;
                    cboStopBits.Enabled = false;
                    cboParity.Enabled = false;
                    cboHandShaking.Enabled = false;
                    btnGetSerialPorts.Enabled = false;

                }
                else
                {
                    btnPortState.Text = "Apri Porta";
                    cboPorts.Enabled = true;
                    cboBaudRate.Enabled = true;
                    cboDataBits.Enabled = true;
                    cboStopBits.Enabled = true;
                    cboParity.Enabled = true;
                    cboHandShaking.Enabled = true;
                    btnGetSerialPorts.Enabled = true;
                }

                
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void btnApriComunicazione_Click(object sender, EventArgs e)
        {
            bool verifica;
            verifica = _disp.VerificaPresenza();

            if (verifica)
            {
                btnApriComunicazione.ForeColor = Color.Green;
            }
            else
            {
                btnApriComunicazione.ForeColor = Color.Red;
            }

        }

        private void chkRtBacklight_CheckedChanged(object sender, EventArgs e)
        {
            bool verifica;
            verifica = _disp.ImpostaBacklight(chkRtBacklight.Checked);
        }

        private void btnRtSetLed_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;
                byte _valRed = FunzioniMR.ConvertiByte(txtRtValRed.Text, 1, 0);
                byte _valGreen = FunzioniMR.ConvertiByte(txtRtValGreen.Text, 1, 0);
                byte _valBlu = FunzioniMR.ConvertiByte(txtRtValBlu.Text, 1, 0);

                byte _valOn = FunzioniMR.ConvertiByte(txtRtValTimeOn.Text, 1, 0);
                byte _valOff = FunzioniMR.ConvertiByte(txtRtValTimeOff.Text, 1, 0);

                verifica = _disp.ImpostaLed(_valRed, _valGreen, _valBlu, _valOn, _valOff);

            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void btnRtStopLed_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;

                verifica = _disp.ImpostaLed(0, 0, 0, 0, 0);

            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void btnRtDrawLine_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;
                byte _valXs = FunzioniMR.ConvertiByte(txtRtValLineXStart.Text, 1, 0);
                byte _valYs = FunzioniMR.ConvertiByte(txtRtValLineYStart.Text, 1, 0);
                byte _valXe = FunzioniMR.ConvertiByte(txtRtValLineXFine.Text, 1, 0);
                byte _valYe = FunzioniMR.ConvertiByte(txtRtValLineYFine.Text, 1, 0);
                byte _valColor = FunzioniMR.ConvertiByte(txtRtValLineColor.Text, 1, 0);

                verifica = _disp.DisegnaLinea(_valXs, _valYs, _valXe, _valYe, _valColor);

            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }
    }
}
