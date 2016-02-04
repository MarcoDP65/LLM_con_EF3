using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using log4net;
using log4net.Config;

using ChargerLogic;


namespace PannelloCharger
{
    public partial class frmSelettoreCom : Form
    {
        parametriSistema localPar;
        SerialPort ComPort; // = new SerialPort();
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

/*
        internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
        internal delegate void SerialPinChangedEventHandlerDelegate(object sender, SerialPinChangedEventArgs e);
        private SerialPinChangedEventHandler SerialPinChangedEventHandler1;
        delegate void SetTextCallback(string text);

*/

        public frmSelettoreCom(ref parametriSistema _parametri)
        {
            InitializeComponent();
            localPar = _parametri;
            txtCurrPort.Text = localPar.portName;
            txtCurrSpeed.Text = localPar.baudRate.ToString();
            txtCurrData.Text = localPar.dataBits.ToString();
            txtParita.Text = localPar.parityBit.ToString();
            txtStop.Text = localPar.stopBits.ToString();
        }

 
        public frmSelettoreCom()
        {
            InitializeComponent();
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSalva_Click(object sender, EventArgs e)
        {
            localPar.portName = txtCurrPort.Text;
            localPar.baudRate = Convert.ToInt32( txtCurrSpeed.Text );
            this.Close();
        }

        private void lblStop_Click(object sender, EventArgs e)
        {

        }

        private void txtStop_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCercaPorte_Click(object sender, EventArgs e)
        {
            grbRicercaPorte.Visible = true;


            if (localPar != null)
            {
                ComPort = localPar.serialeCorrente;
                //ComPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived);
            }
            else
            {
                //Log.Debug("vGlobali non inizializzata");
                return;
            }

            string[] ArrayComPortsNames = null;
            int index = -1;
            string ComPortName = null;

            //Com Ports
            try
            {
                bool _portaPresente = false;
                int _currIndex;

                ArrayComPortsNames = SerialPort.GetPortNames();

                if (ArrayComPortsNames.Length > 0)
                {

                    cboPorts.Items.Clear();
                    do
                    {
                        index += 1;
                        _portaPresente = false;
                        cboPorts.Items.Add(ArrayComPortsNames[index]);
                        if (ArrayComPortsNames[index] == localPar.portName)
                        {
                            _portaPresente = true;
                            _currIndex = index;
                        }


                    } while (!((ArrayComPortsNames[index] == ComPortName) || (index == ArrayComPortsNames.GetUpperBound(0))));
                    Array.Sort(ArrayComPortsNames);

                    if (index == ArrayComPortsNames.GetUpperBound(0))
                    {
                        if (_portaPresente)
                            ComPortName = localPar.portName;
                        else
                            ComPortName = ArrayComPortsNames[0];
                    }
                    //get first item print in text
                    cboPorts.Text = ComPortName; // ArrayComPortsNames[0];
                    //Baud Rate
                    cboBaudRate.Items.Clear();
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
                    cboBaudRate.Text = localPar.baudRate.ToString();  //cboBaudRate.Items[0].ToString();
                    //Data Bits
                    cboDataBits.Items.Clear();
                    cboDataBits.Items.Add(7);
                    cboDataBits.Items.Add(8);
                    //get the first item print it in the text 
                    cboDataBits.Text = localPar.dataBits.ToString(); //cboDataBits.Items[0].ToString();

                    //Stop Bits
                    cboStopBits.Items.Clear();
                    cboStopBits.Items.Add("One");
                    cboStopBits.Items.Add("OnePointFive");
                    cboStopBits.Items.Add("Two");
                    //get the first item print in the text
                    cboStopBits.Text = localPar.stopBits.ToString(); //cboStopBits.Items[0].ToString();
                    //Parity 
                    cboParity.Items.Clear();
                    cboParity.Items.Add("None");
                    cboParity.Items.Add("Even");
                    cboParity.Items.Add("Mark");
                    cboParity.Items.Add("Odd");
                    cboParity.Items.Add("Space");
                    //get the first item print in the text
                    cboParity.Text = localPar.parityBit.ToString(); //cboParity.Items[0].ToString();

                }
            }
            catch (Exception Ex)
            {
                Log.Error("btnCercaPorte_Click: " + Ex.Message);
            }





        }

        private void btnSelezionaPorta_Click(object sender, EventArgs e)
        {
            if (cboPorts.Text != "")
            {
                txtCurrPort.Text = cboPorts.Text;
                txtCurrSpeed.Text = cboBaudRate.Text;
            }

            grbRicercaPorte.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            grbRicercaPorte.Visible = false;
        }

        private void frmSelettoreCom_Load(object sender, EventArgs e)
        {

        }

    }
}
