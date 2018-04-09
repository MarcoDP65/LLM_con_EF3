using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using ChargerLogic;
using log4net;
using log4net.Config;

namespace PannelloCharger
{
    public partial class frmSerialPanel : Form
    {
        SerialPort ComPort; // = new SerialPort();
        public parametriSistema vGlobali;

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

        public frmSerialPanel()
        {
            XmlConfigurator.Configure();
            InitializeComponent();
            SerialPinChangedEventHandler1 = new SerialPinChangedEventHandler(PinChanged);
//            ComPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived);
        }
     
        private void btnGetSerialPorts_Click(object sender, EventArgs e)
        {
            if (vGlobali != null)
            {
                Log.Debug(vGlobali.currentUser.ToString());
                ComPort = vGlobali.serialeCorrente;
                //ComPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived);
            }
            else
            {
                Log.Debug("vGlobali non inizializzata");
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

                    do
                    {
                        index += 1;
                        cboPorts.Items.Add(ArrayComPortsNames[index]);
                        if (ArrayComPortsNames[index] == vGlobali.portName)
                        {
                            _portaPresente = true;
                            _currIndex = index;
                        }


                    } while (!((ArrayComPortsNames[index] == ComPortName) || (index == ArrayComPortsNames.GetUpperBound(0))));
                    Array.Sort(ArrayComPortsNames);

                    if (index == ArrayComPortsNames.GetUpperBound(0))
                    {
                        if (_portaPresente)
                            ComPortName = vGlobali.portName;
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
                    cboBaudRate.Text = vGlobali.baudRate.ToString();  //cboBaudRate.Items[0].ToString();
                    //Data Bits
                    cboDataBits.Items.Add(7);
                    cboDataBits.Items.Add(8);
                    //get the first item print it in the text 
                    cboDataBits.Text = vGlobali.dataBits.ToString(); //cboDataBits.Items[0].ToString();

                    //Stop Bits
                    cboStopBits.Items.Add("One");
                    cboStopBits.Items.Add("OnePointFive");
                    cboStopBits.Items.Add("Two");
                    //get the first item print in the text
                    cboStopBits.Text = vGlobali.stopBits.ToString(); //cboStopBits.Items[0].ToString();
                    //Parity 
                    cboParity.Items.Add("None");
                    cboParity.Items.Add("Even");
                    cboParity.Items.Add("Mark");
                    cboParity.Items.Add("Odd");
                    cboParity.Items.Add("Space");
                    //get the first item print in the text
                    cboParity.Text = vGlobali.parityBit.ToString(); //cboParity.Items[0].ToString();
                    //Handshake
                    cboHandShaking.Items.Add("None");
                    cboHandShaking.Items.Add("XOnXOff");
                    cboHandShaking.Items.Add("RequestToSend");
                    cboHandShaking.Items.Add("RequestToSendXOnXOff");
                    //get the first item print it in the text 
                    cboHandShaking.Text = vGlobali.handShake.ToString(); //cboHandShaking.Items[0].ToString();
                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }

        }

     


        private void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string testom = "Risposta-->";
            byte[] data = new byte[ComPort.BytesToRead];
            ComPort.Read(data, 0, data.Length);
            SerialMessage.EsitoRisposta _esito;
            try
            {

                Log.Debug(testom);


                for (int ciclodati = 0; ciclodati < data.Length; ciclodati++)
                {
                    if (data[ciclodati] == SerialMessage.serSTX)
                    { readingMessage = true; }
                    if (readingMessage)
                    {
                        int lastByte = _dataBuffer.Length;
                        Array.Resize(ref _dataBuffer, lastByte + 1);
                        _dataBuffer[lastByte] = data[ciclodati];
                    }

                    if (data[ciclodati] == SerialMessage.serETX)
                    { //fine messaggio
                        readingMessage = false;
                        SerialMessage _smx = new SerialMessage();
                        _smx.MessageBuffer = _dataBuffer;
                        _esito = _smx.analizzaMessaggio(_dataBuffer);

                        Log.Debug(_smx._comando.ToString("X2"));
                        _smx.componiRisposta(_dataBuffer, _esito);
                        ComPort.Write(_smx.messaggioRisposta, 0, _smx.messaggioRisposta.Length);

                        switch (_smx._comando)
                        {
                            case 0x44: // ACK
                                Log.Debug("Comando Ricevuto");

                                break;
                            case 0x45: //NAK
                                Log.Debug("Comando Errato");
                                break;
                            case 0xD3: // read RTC
                                string _dataOra = "Data/ Ora: ";
                                _dataOra += _smx.DatiRTC.giorno + "/" + _smx.DatiRTC.mese + "/" + _smx.DatiRTC.anno;
                                _dataOra += "  - " + _smx.DatiRTC.ore + ":" + _smx.DatiRTC.minuti + ":" + _smx.DatiRTC.secondi;
                                Log.Debug(_dataOra);

                                break;
                            default:
                                Log.Debug("Altro Comando");
                                break;
                        }


                        lastByte = 0;
                        Array.Resize(ref _dataBuffer, 0);
                    }


                    testom += data[ciclodati].ToString("X2");

                }
                Log.Debug(testom);
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

        private void btnTest_Click(object sender, EventArgs e)
        {
            //SerialPinChangedEventHandler1 = new SerialPinChangedEventHandler(PinChanged);
            //ComPort.PinChanged += SerialPinChangedEventHandler1;
            //ComPort.Open();

            //ComPort.RtsEnable = true;
            //ComPort.DtrEnable = true;
            //btnTest.Enabled = false;

        }

        private void btnPortState_Click(object sender, EventArgs e)
        {
          
            if (btnPortState.Text == "Closed")
            {
                btnPortState.Text = "Open";
                ComPort.PortName = Convert.ToString(cboPorts.Text);
                ComPort.BaudRate = Convert.ToInt32(cboBaudRate.Text);
                ComPort.DataBits = Convert.ToInt16(cboDataBits.Text);
                ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cboStopBits.Text);
                ComPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), cboHandShaking.Text);
                ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), cboParity.Text);
                _dataBuffer = new byte[0];
                ComPort.Open();
            }
            else if (btnPortState.Text == "Open")
            {
                btnPortState.Text = "Closed";
                ComPort.Close();
               
            }
        }
        private void rtbOutgoing_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // enter key  
            {
                ComPort.Write("\r\n");
                rtbOutgoing.Text = "";
            }
            else if (e.KeyChar < 32 || e.KeyChar > 126)
            {
                e.Handled = true; // ignores anything else outside printable ASCII range  
            }
            else
            {
                ComPort.Write(e.KeyChar.ToString());
            }
        }
        private void btnHello_Click(object sender, EventArgs e)
        {
            ComPort.Write("Hello World!");
        }

        private void btnHyperTerm_Click(object sender, EventArgs e)
        {

            SerialMessage sm = new SerialMessage();
            ushort prova = sm.codificaByte((byte)(0x6E));
            byte lsb = (byte)(0);
            byte msb = (byte)(0);

            // se la porta è chiusa, la apro
            if (ComPort.IsOpen == false) return;

            sm.Dispositivo = SerialMessage.TipoDispositivo.Charger;
            sm.Comando = SerialMessage.TipoComando.CMD_CONNECT;
            byte[] Seriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
            sm.SerialNumber = Seriale;

            sm.ComponiMessaggio();

            string testom = "";

            for (int i = 0; i < sm.MessageBuffer.Length; i++)
            
                testom += sm.MessageBuffer[i].ToString("X2");

            Log.Debug(testom);

            ComPort.Write(sm.MessageBuffer, 0, sm.MessageBuffer.Length); 
            

        }

        private void frmSerialPanel_Load(object sender, EventArgs e)
        {

        }

        private void cmdLeggiRTC_Click(object sender, EventArgs e)
        {
            try
            {
                SerialMessage sm = new SerialMessage();
                ushort prova = sm.codificaByte((byte)(0x6E));
                byte lsb = (byte)(0);
                byte msb = (byte)(0);

                sm.Dispositivo = SerialMessage.TipoDispositivo.Charger;
                sm.Comando = SerialMessage.TipoComando.CMD_READ_RTC;
                byte[] Seriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
                sm.SerialNumber = Seriale;

                sm.ComponiMessaggio();

                string testom = "";

                for (int i = 0; i < sm.MessageBuffer.Length; i++)

                    testom += sm.MessageBuffer[i].ToString("X2");

                Log.Debug(testom);

                ComPort.Write(sm.MessageBuffer, 0, sm.MessageBuffer.Length);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
  
        }

        private void btnPrimaLettura_Click(object sender, EventArgs e)
        {
            try
            {
                SerialMessage sm = new SerialMessage();
                ushort prova = sm.codificaByte((byte)(0x6E));
                byte lsb = (byte)(0);
                byte msb = (byte)(0);

                sm.Dispositivo = SerialMessage.TipoDispositivo.Charger;
                sm.Comando = SerialMessage.TipoComando.CMD_UART_HOST_CONNECTED;
                byte[] Seriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
                sm.SerialNumber = Seriale;

                sm.ComponiMessaggio();

                string testom = "";

                for (int i = 0; i < sm.MessageBuffer.Length; i++)

                    testom += sm.MessageBuffer[i].ToString("X2");

                Log.Debug(testom);

                ComPort.Write(sm.MessageBuffer, 0, sm.MessageBuffer.Length);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }

        }

      
    }
}
