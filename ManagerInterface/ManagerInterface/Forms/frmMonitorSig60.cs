using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class frmMonitorSig60 : Form
    {

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public frmMonitorSig60()
        {
            InitializeComponent();
        }

        private void frmMonitorSig60_Load(object sender, EventArgs e)
        {

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
                //cboDataBits.Items.Clear();
                //cboStopBits.Items.Clear();
                //cboParity.Items.Clear();
                //boHandShaking.Items.Clear();

                if (ArrayComPortsNames.Length > 0)
                {

                    do
                    {
                        index += 1;
                        cboPorts.Items.Add(ArrayComPortsNames[index]);
                        if (ArrayComPortsNames[index] == _portaBase)
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

                    cboDataBits.Text = "8"; //cboDataBits.Items[0].ToString();

                    //Stop Bits

                    cboStopBits.Text = "One"; //cboStopBits.Items[0].ToString();
                    //Parity 
                    cboParity.Items.Add("None");

                    cboParity.Text = "None"; //cboParity.Items[0].ToString();
                    //Handshake
                    cboHandShaking.Text = "None"; //cboHandShaking.Items[0].ToString();
                }

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }




    }
}
