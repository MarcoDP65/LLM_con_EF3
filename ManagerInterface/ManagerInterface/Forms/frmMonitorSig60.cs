using System;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.IO.Ports;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using BrightIdeasSoftware;

using ChargerLogic;
using log4net;
using log4net.Config;

using MoriData;
using Utility;
using Newtonsoft.Json;


namespace PannelloCharger
{
    public partial class frmMonitorSig60 : Form
    {

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public SerialPort ComPort;

        public event EventHandler<SerialDataEventArgs> NewSerialDataRecieved;
        private static Queue<byte> codaDatiSER = new Queue<byte>();  // Buffer per la ricezione dati seriali

        private MessaggioSpyBatt _mS = new MessaggioSpyBatt();

        public int TipoRisposta;
        public SerialMessage.EsitoRisposta UltimaRisposta;
        public List<echoMessaggio> ListaMessaggi = new List<echoMessaggio>();

        byte[] _dataBuffer;
        byte[] FullDataBuffer;



        public frmMonitorSig60()
        {
            InitializeComponent();
            ComPort = new SerialPort();
            _dataBuffer = new byte[0];
            FullDataBuffer = new byte[0];
            ListaMessaggi.Clear();
            InizializzaVistaMessaggi();
            btnGetSerialPorts_Click(null,null);

            RidimensionaControlli();
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
                    cboBaudRate.Items.Add(9600);
                    cboBaudRate.Items.Add(19200);
                    cboBaudRate.Items.Add(38400);
                    cboBaudRate.Items.Add(57600);
                    //cboBaudRate.Items.Add(115200);
                    //cboBaudRate.Items.ToString();
                    //get first item print in text
                    cboBaudRate.Text = "9600";  //cboBaudRate.Items[0].ToString();
                    //Data Bits

                    //cboDataBits.Text = "8"; //cboDataBits.Items[0].ToString();

                    //Stop Bits

                    //cboStopBits.Text = "One"; //cboStopBits.Items[0].ToString();
                    //Parity 
                    //cboParity.Items.Add("None");

                    //cboParity.Text = "None"; //cboParity.Items[0].ToString();
                    //Handshake
                    //cboHandShaking.Text = "None"; //cboHandShaking.Items[0].ToString();
                }

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void btnPortState_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnPortState.Text == "Apri Porta")
                {
                    ApriPorta();
                }
                else if (btnPortState.Text == "Chiudi")
                {
                    ChiudiPorta();
                }



            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private bool ChiudiPorta()
        {
            try
            {

                ComPort.Close();
                VerificaStatoPorta();

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("VerificaStatoPorta: " + Ex.Message);
                return false;
            }
        }

        private bool ApriPorta()
        {
            try
            {
                if (cboPorts.Text == "")
                    return false;
                //btnPortState.Text = "Chiudi";
                ComPort.PortName = Convert.ToString(cboPorts.Text);
                ComPort.BaudRate = Convert.ToInt32(cboBaudRate.Text);
                ComPort.DataBits = 8;
                ComPort.StopBits = StopBits.One;
                ComPort.Handshake = Handshake.None;
                ComPort.Parity = Parity.None;
                _dataBuffer = new byte[0];
                cEventHelper.RemoveEventHandler(ComPort, "DataReceived");
                ComPort.DataReceived += new SerialDataReceivedEventHandler(_serialPort_DataReceived);
                cEventHelper.RemoveEventHandler(this, "NewSerialDataRecieved");
                NewSerialDataRecieved += new EventHandler<SerialDataEventArgs>(_spManager_NewSerialDataRecieved);

                if (ComPort.IsOpen) ComPort.Close();

                ComPort.Open();



                VerificaStatoPorta();

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("VerificaStatoPorta: " + Ex.Message);
                return false;
            }
        }

        private bool VerificaStatoPorta()
        {
            try
            {
                if (ComPort == null)
                    return false;

                if (ComPort.IsOpen)
                {
                    // Se la porta è aperta blocco tutto
                    btnPortState.Text = "Chiudi";
                    cboPorts.Enabled = false;
                    cboBaudRate.Enabled = false;
                    btnGetSerialPorts.Enabled = false;
                    btnSetSigRegister.ForeColor = Color.Black;
                    btnSetSigRegister.Enabled = true;


                }
                else
                {
                    btnPortState.Text = "Apri Porta";
                    cboPorts.Enabled = true;
                    cboBaudRate.Enabled = true;
                    btnGetSerialPorts.Enabled = true;
                    btnSetSigRegister.Enabled = false;

                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("VerificaStatoPorta: " + Ex.Message);
                return false;
            }
        }

        private void btnGetSigRegister_Click(object sender, EventArgs e)
        {
            try
            {

                string _msg;
                byte[] _comando = new byte[1];

                //txtSerialEcho.Text += "\r\n";
                //Application.DoEvents();

                _comando[0] = 0x0D;

                ScriviMessaggioByte(_comando);
                Thread.Sleep(200);

                _comando[0] = 0x1D;
                ScriviMessaggioByte(_comando);
                Thread.Sleep(200);
                txtSerialEcho.AppendText("\r\n");
            }

            catch (Exception Ex)
            {
                Log.Error("btnGetSigRegister_Click: " + Ex.Message);
            }
        }


        public bool ScriviMessaggio(string Messaggio, bool TerminaRiga = false, bool silent = true)
        {
            try
            {
                string _messaggio = Messaggio;
                bool _esito = false;
                if (PortaConnessa())
                {
                    if (TerminaRiga) _messaggio += (char)13;
                    ComPort.Write(_messaggio);
                    _esito = true;
                }

                return _esito;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ScriviMessaggioByte(byte[] Messaggio)
        {
            try
            {

                bool _esito = false;
                if (PortaConnessa())
                {

                    ComPort.Write(Messaggio,0, Messaggio.Length);
                    _esito = true;
                }

                return _esito;
            }
            catch (Exception ex)
            {
                return false;
            }
        }






        /// <summary>
        /// Verifica se la porta seriale è aperta.
        /// </summary>
        /// <returns></returns>
        public bool PortaConnessa()
        {
            try
            {

                if (ComPort != null)
                {
                    return ComPort.IsOpen;
                }

                return false;
            }
            catch (Exception Ex)
            {
                Log.Error("PortaConnessa: " + Ex.Message);
                return false;
            }
        }


        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int dataLength = ComPort.BytesToRead;
            byte[] data = new byte[dataLength];

            //int nbrDataRead = ComPort.Read(data, 0, dataLength);
            int nbrDataRead = 0;
            for (int _bytes = 0; _bytes < dataLength; _bytes++)
            {
                data[_bytes] = (byte)ComPort.ReadByte();
                nbrDataRead += 1;
            }
            //Log.Debug("_serialPort_DataReceived:  " + FunzioniComuni.HexdumpArray(data));
            if (nbrDataRead == 0)
                return;
            // Send data to whom ever interested
            if (NewSerialDataRecieved != null)
                NewSerialDataRecieved(this, new SerialDataEventArgs(data));

        }

        private void btnSigEchoCLS_Click(object sender, EventArgs e)
        {
            txtSerialEcho.Text = "";
        }


        void _spManager_NewSerialDataRecieved(object sender, SerialDataEventArgs e)
        {
            try
            {
                bool _trovatoETX = false;
                if (this.InvokeRequired)
                {
                    // Using this.Invoke causes deadlock when closing serial port, and BeginInvoke is good practice anyway.
                    this.BeginInvoke(new EventHandler<SerialDataEventArgs>(_spManager_NewSerialDataRecieved), new object[] { sender, e });
                    return;
                }
                int datiRicevuti = e.Data.Length;
                for (int _i = 0; _i < datiRicevuti; _i++)
                {




                    txtSerialEcho.AppendText( e.Data[_i].ToString("X2"));
                    if (e.Data[_i] == 0x03)
                    {
                        txtSerialEcho.AppendText("\r\n");
                    }
                    else
                    {
                        txtSerialEcho.AppendText(" ");
                    }

                    

                    // Accodo al buffer di salvataggio
                    //int lastByte = FullDataBuffer.Length;
                    //Array.Resize(ref FullDataBuffer, lastByte + 1);
                    //FullDataBuffer[lastByte] = e.Data[_i];
                    

                    codaDatiSER.Enqueue(e.Data[_i]);
                    if (e.Data[_i] == SerialMessage.serETX)
                    {
                        Log.Debug("Trovato Etx (SIG)");
                        _trovatoETX = true;
                    }

                    if (_trovatoETX)
                    {
                        Log.Debug("Trovato Etx (USB) --> Analizza Coda");
                        analizzaCodaSIG();

                    }



                }
               
            }
            catch (Exception Ex)
            {
                Log.Error("_spManager_NewSerialDataRecieved: " + Ex.Message);
                
            }
        }

        private void btnSetSigRegister_Click(object sender, EventArgs e)
        {
            string _msg;
            byte[] _comando = new byte[2];

            //txtSerialEcho.AppendText("\r\n");
            //Application.DoEvents();

            _comando[0] = 0x15;
            switch (cboBaudRate.Text)
            {
                case "9600":
                    _comando[1] = 0xEF;
                    break;

                case "19200":
                    _comando[1] = 0xFF;
                    break;
                case "38400":
                    _comando[1] = 0xCF;
                    break;
                case "57600":
                    _comando[1] = 0xDF;
                    break;
                default:
                    _comando[1] = 0xFF;
                    break;

            }

            
            ScriviMessaggioByte(_comando);
            Thread.Sleep(500);
            txtSerialEcho.AppendText("\r\n");

        }


        private SerialMessage.TipoRisposta analizzaCodaSIG()
        {

            SerialMessage.EsitoRisposta _esito;
            bool _trovatoSTX = false;
            byte _tempByte;
            string testom = "";
            bool _inviaRisposta = true;
            SerialMessage.TipoRisposta _datiRicevuti = SerialMessage.TipoRisposta.NonValido;

            try
            {
                testom = "LUNGHEZZA CODA: " + codaDatiSER.Count();
                Log.Debug(testom);
                testom = "";

                while (codaDatiSER.Count() > 0)
                {
                    if (codaDatiSER.Contains(SerialMessage.serETX) == false)
                    {

                        Log.Debug("NON trovato ETX");
                        _datiRicevuti = SerialMessage.TipoRisposta.NonValido;
                        return _datiRicevuti;
                    }


                    _tempByte = codaDatiSER.Peek();
                    if (_tempByte != SerialMessage.serSTX)
                    {
                        _tempByte = codaDatiSER.Dequeue();
                    }
                    else
                    {
                        while (_tempByte != SerialMessage.serETX)
                        {
                            int lastByte = _dataBuffer.Length;
                            Array.Resize(ref _dataBuffer, lastByte + 1);
                            _tempByte = codaDatiSER.Dequeue();
                            _dataBuffer[lastByte] = _tempByte;
                            testom += _tempByte.ToString("X2");
                        }
                        Log.Debug(testom);
                        testom = "";
                        Log.Debug("Trovato ETX");
                        //readingMessage = false;

                        //_mS.MessageBuffer = _dataBuffer;
                        //-----------------------------------------------------------------------------------------
                        // Analizzo il contenuto del messaggio 
                        //-----------------------------------------------------------------------------------------
                        _esito = _mS.analizzaMessaggio(_dataBuffer, 6, false,true);
                        //UltimaRisposta = _esito; // SerialMessage.EsitoRisposta.MessaggioOk;
                        //-----------------------------------------------------------------------------------------

                        _inviaRisposta = false;
                        Log.Debug("Comando: --> 0x" + _mS._comando.ToString("X2"));
                        echoMessaggio _msg = new echoMessaggio();
                        

                        _msg.Dispositivo = _mS.idCorrente;
                        _msg.Sottocomando = "";
                        // In base al serial number riconosco il tipo dispositivo da cui arriva il messaggio
                        switch (_mS.idCorrente)
                        {
                            case "0101010100000000":
                                _msg.TipoDispositivo = "Monitor";
                                _msg.Device = echoMessaggio.TipoDevice.Monitor;
                                break;
                            case "0000000000000000":
                                _msg.TipoDispositivo = "LADE Light";
                                _msg.Device = echoMessaggio.TipoDevice.LADELight;
                                break;
                            default:
                                _msg.TipoDispositivo = "SPY-BATT";
                                _msg.Device = echoMessaggio.TipoDevice.SPYBATT;
                                break;
                        }




                        switch (_mS._comando)
                        {

                            case (byte)SerialMessage.TipoComando.SB_ACK:  //0x6C: // ACK
                                Log.Debug("Comando Ricevuto: SB_ACK");
                                _msg.Comando = "SB_ACK";
                                _msg.DescComando = "Comando Ricevuto";
                                _msg.Parametri = "";
                                break;
                            case (byte)SerialMessage.TipoComando.SB_NACK:  //0x71: // NACK
                                Log.Debug("Comando Ricevuto: SB_NACK");
                                _msg.Comando = "SB_NACK";
                                _msg.DescComando = "Comando Ricevuto";
                                _msg.Parametri = "";
                                break;

                            case (byte)SerialMessage.TipoComando.SB_W_chgst_Call:  //0x6C
                                Log.Debug("Comando Strategia");
                                _msg.Comando = "SB_W_chgst_Call";
                                _msg.Sottocomando = _mS.ComandoStrat.DescComandoLibreria;
                                _msg.DataArray = _mS.ComandoStrat.memDataDecoded;
                                _msg.DescComando = "Comando Strategia";

                                MostraParametriStrategia(ref _msg, _mS.ComandoStrat);

                                break;

                            case (byte)SerialMessage.TipoComando.SB_Sstart:  //0x6C
                                Log.Debug("Apertura Canale SIG");
                                _msg.Comando = "SB_Sstart";
                                _msg.DescComando = "Apertura Canale SIG";
                                _msg.Parametri = "";
                                break;

                            case (byte)SerialMessage.TipoComando.SB_Stop:  //0x1D
                                Log.Debug("Chiusura Canale SIG");
                                _msg.Comando = "SB_Stop";
                                _msg.DescComando = "Chiusura Canale SIG";
                                _msg.Parametri = "";
                                break;

                            default:
                                Log.Debug("Comando Sconosciuto");
                                _msg.Comando = "???? (" + _mS._comando.ToString("x2") + ")";
                                _msg.DescComando = "Comando Sconosciuto";
                                _msg.TipoDispositivo = "N.D.";
                                _msg.Parametri = "";
                                break;
                        }
                        ListaMessaggi.Insert(0,_msg);

                        flvListaComandiSIG.SetObjects(ListaMessaggi);
                        flvListaComandiSIG.BuildList();
                       // flvListaComandiSIG.RefreshObjects(ListaMessaggi);
                        Array.Resize(ref _dataBuffer, 0);
                        //return _datiRicevuti;
                    }

                }
                return _datiRicevuti;
            }

            catch (Exception Ex)
            {
                Log.Error("analizzaCodaSIG " + Ex.Message);
                _datiRicevuti = SerialMessage.TipoRisposta.NonValido;
                return _datiRicevuti;
            }

        }

        public bool MostraParametriStrategia( ref echoMessaggio RigaBase,MessaggioSpyBatt.ComandoStrategia Comando)
        {
            try
            {
                echoMessaggio _msgPar = new echoMessaggio();
                byte[] _Dati = Comando.memDataDecoded;
                string rigaParametri = "";
                switch (Comando.ComandoLibreria)
                {
                    case 0x01: // "CMD_R - Reset Libreria";
                        RigaBase.Parametri = "";
                        break;

                    case 0x02: // "CMD_IS - Richiesta Strategia";
                    case 0x05: // "CMD_SIS - Richiesta Strategia";
                        RigaBase.Parametri = "";
                        if (RigaBase.Device == echoMessaggio.TipoDevice.LADELight)
                        {
                            // ------------------ prima riga -------
                            rigaParametri = "Vmin:" + FunzioniMR.StringaTensione(FunzioniComuni.UshortFromArray(_Dati, 0x02));
                            rigaParametri += " - Vmax:" + FunzioniMR.StringaTensione(FunzioniComuni.UshortFromArray(_Dati, 0x04));
                            rigaParametri += " - Amax:" + FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 0x06));
                            RigaBase.Parametri = rigaParametri;
                        }
                        else
                        {
                            int _numStep = _Dati[0x07];
                            // ------------------ prima riga -------
                            rigaParametri = "Ah:" + FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 0x03));
                            rigaParametri += " - Min:" + FunzioniComuni.UshortFromArray(_Dati, 0x05).ToString();
                            rigaParametri += " - Step:" + _Dati[0x07].ToString();
                            RigaBase.Parametri = rigaParametri;

                          
                            for (byte _steps = 0; _steps < _Dati[0x07]; _steps++)
                            {

                                int _startpoint = 15 + _steps * 16;
                                if (_Dati.Length < (_startpoint + 16))
                                {
                                    //risposta troppo corta
                                    break;
                                }
                                byte[] _StepBuffer = new byte[16];
                                Array.Copy(_Dati, _startpoint, _StepBuffer, 0, 16);
                                StepCarica _passoCorrente = new StepCarica();
                                _passoCorrente.CaricaDati(_StepBuffer, (byte)(_steps + 1));

                                _msgPar = new echoMessaggio();
                                _msgPar.SegueRiga = true;
                                _msgPar.Device = RigaBase.Device;
                                 _msgPar.Comando = (_steps + 1).ToString() + ".3";
                                rigaParametri = "T On:" + _passoCorrente.strTon + " - T Off:" + _passoCorrente.strToff;
                                rigaParametri += " - T Blocco:" + _passoCorrente.strTBlocco;
                                _msgPar.Parametri = rigaParametri;
                                ListaMessaggi.Insert(0, _msgPar);

                                _msgPar = new echoMessaggio();
                                _msgPar.SegueRiga = true;
                                _msgPar.Device = RigaBase.Device;
                                _msgPar.Comando = (_steps + 1).ToString() + ".2";
                                rigaParametri = "Vmin:" + _passoCorrente.strVMinima + " - VMax:" + _passoCorrente.strVMassima;
                                rigaParametri += " - #Rip:" + _passoCorrente.strNumRipetizioni;
                                _msgPar.Parametri = rigaParametri;
                                ListaMessaggi.Insert(0, _msgPar);

                                _msgPar = new echoMessaggio();
                                _msgPar.SegueRiga = true;
                                _msgPar.Device = RigaBase.Device;
                                _msgPar.Comando = (_steps + 1).ToString() + ".1 - " + _passoCorrente.strTipoStep;
                                rigaParametri = "Imin:" + _passoCorrente.strIMinima + " - IMax:" + _passoCorrente.strIMassima;
                                rigaParametri += " - Ah Step:" + _passoCorrente.strCapStep;
                                _msgPar.Parametri = rigaParametri;
                                ListaMessaggi.Insert(0, _msgPar);



                            }




                        }
                        break;

                    case 0x03: // "CMD_AV - Avanzamento Carica"
                        RigaBase.Parametri = "";
                        if (RigaBase.Device == echoMessaggio.TipoDevice.LADELight)
                        {
                            RigaBase.Parametri = "";
                        }
                        else
                        {
                            // ------------------ prima riga -------
                            rigaParametri = "Ah Er:" + FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 3));
                            rigaParametri += " - Ah Rich:" + FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 5));
                            rigaParametri += " - Min res:" + FunzioniComuni.UshortFromArray(_Dati, 0x07).ToString();
                            rigaParametri += " / el:" + FunzioniComuni.UshortFromArray(_Dati, 0x09).ToString();

                            RigaBase.Parametri = rigaParametri;
                            // Ora il pacchetto in ordine inverso

                            _msgPar = new echoMessaggio();
                            _msgPar.SegueRiga = true;
                            _msgPar.Device = RigaBase.Device;
                            _msgPar.Comando = "1";
                            rigaParametri = "V:" + FunzioniMR.StringaTensione(FunzioniComuni.UshortFromArray(_Dati, 0x0B));
                            rigaParametri += " - A: " + FunzioniMR.StringaCorrenteSigned(FunzioniComuni.ArrayToShort(_Dati, 0x0D, 2));
                            rigaParametri += " - °C:" + FunzioniMR.StringaTemperatura(_Dati[0x0F]);
                            _msgPar.Parametri = rigaParametri;
                            ListaMessaggi.Insert(0, _msgPar);

                        }

                        break;

                    case 0x04:
                        RigaBase.Parametri = "";
                        break;


                    case 0xA0: // "CMD_QRY - Richiesta Informazioni"
                        if (RigaBase.Device == echoMessaggio.TipoDevice.LADELight)
                        {
                            RigaBase.Parametri = "";
                        }
                        else
                        {
                            // ------------------ prima riga -------
                            rigaParametri = "Lib:" + _Dati[0x03].ToString() + "." + _Dati[0x04].ToString("00") + "." + FunzioniComuni.UshortFromArray(_Dati, 0x05).ToString("000");
                            rigaParametri += " - Cfg:" + _Dati[0x08].ToString();
                            rigaParametri += " - ID:" + FunzioniComuni.ArrayToString(_Dati, 0x1D, 5);
                            RigaBase.Parametri = rigaParametri;
                            // Ora il pacchetto in ordine inverso
                            _msgPar = new echoMessaggio();
                            _msgPar.SegueRiga = true;
                            _msgPar.Device = RigaBase.Device;
                            _msgPar.Comando = "2";
                            rigaParametri = "Mod. P:" + _Dati[0x16].ToString() + " - Giorno:" + DataOraMR.SiglaGiorno(_Dati[0x17] + 1) + "-" + _Dati[0x17].ToString();
                            rigaParametri += " - Min: " + ((ushort)((_Dati[0x19] << 8) + _Dati[0x1A])).ToString();
                            rigaParametri += " - FC:" + FunzioniMR.StringaFattoreCarica(_Dati[0x1B]);
                            _msgPar.Parametri = rigaParametri;
                            ListaMessaggi.Insert(0, _msgPar);
                            _msgPar = new echoMessaggio();
                            _msgPar.SegueRiga = true;
                            _msgPar.Device = RigaBase.Device;
                            _msgPar.Comando = "1";
                            rigaParametri = "V:" + FunzioniMR.StringaTensione(FunzioniComuni.UshortFromArray(_Dati, 0x09));
                            rigaParametri += " - Ah: " + FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 0x0B));
                            rigaParametri += " - Vgas:" + FunzioniMR.StringaTensioneCella(FunzioniComuni.UshortFromArray(_Dati, 0x11));
                            _msgPar.Parametri = rigaParametri;
                            ListaMessaggi.Insert(0, _msgPar);

                        }

                        break;

                    case 0x54:
                        RigaBase.Parametri = "";
                        break;

                    case 0x55:
                        RigaBase.Parametri = "";
                        break;

                    default:
                        RigaBase.Parametri = "";
                        return false;

                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("btnChiudi_Click " + Ex.Message);
                return false;
            }
        }


        private void btnChiudi_Click(object sender, EventArgs e)
        {
            try
            {

                if (ComPort != null)
                {
                    if (ComPort.IsOpen)
                        ComPort.Close();
                }

                this.Close();

            }
            catch (Exception Ex)
            {
                Log.Error("btnChiudi_Click " + Ex.Message);
            }

        }

        private void InizializzaVistaMessaggi()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);
                //flvListaComandiSIG.RowHeight = 50;

                flvListaComandiSIG.HeaderUsesThemes = false;
                flvListaComandiSIG.HeaderFormatStyle = _stile;
                flvListaComandiSIG.UseAlternatingBackColors = true;
                flvListaComandiSIG.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvListaComandiSIG.AllColumns.Clear();

                flvListaComandiSIG.View = View.Details;
                flvListaComandiSIG.ShowGroups = false;
                flvListaComandiSIG.GridLines = true; 

                BrightIdeasSoftware.OLVColumn colIstante = new BrightIdeasSoftware.OLVColumn();
                colIstante.Text = "Ora";
                colIstante.AspectName = "Istante";
                colIstante.Width = 70;
                colIstante.HeaderTextAlign = HorizontalAlignment.Left;
                colIstante.TextAlign = HorizontalAlignment.Right;
                flvListaComandiSIG.AllColumns.Add(colIstante);

                BrightIdeasSoftware.OLVColumn colDispositivo = new BrightIdeasSoftware.OLVColumn();
                colDispositivo.Text = "Dispositivo";
                colDispositivo.AspectName = "TipoDispositivo";
                colDispositivo.Width = 80;
                colDispositivo.HeaderTextAlign = HorizontalAlignment.Center;
                colDispositivo.TextAlign = HorizontalAlignment.Left;
                flvListaComandiSIG.AllColumns.Add(colDispositivo);

                BrightIdeasSoftware.OLVColumn colSN = new BrightIdeasSoftware.OLVColumn();
                colSN.Text = "S.N.";
                colSN.AspectName = "Dispositivo";
                colSN.Width = 120;
                colSN.HeaderTextAlign = HorizontalAlignment.Center;
                colSN.TextAlign = HorizontalAlignment.Center;
                flvListaComandiSIG.AllColumns.Add(colSN);

                BrightIdeasSoftware.OLVColumn colComando = new BrightIdeasSoftware.OLVColumn();
                colComando.Text = "Comando";
                colComando.AspectName = "Comando";
                colComando.Width = 120;
                colComando.HeaderTextAlign = HorizontalAlignment.Left;
                colComando.TextAlign = HorizontalAlignment.Left;
                flvListaComandiSIG.AllColumns.Add(colComando);


                BrightIdeasSoftware.OLVColumn colSottocomando = new BrightIdeasSoftware.OLVColumn();
                colSottocomando.Text = "SubCmd";
                colSottocomando.AspectName = "Sottocomando";
                colSottocomando.Width = 160;
                colSottocomando.HeaderTextAlign = HorizontalAlignment.Left;
                colSottocomando.TextAlign = HorizontalAlignment.Left;
                
                flvListaComandiSIG.AllColumns.Add(colSottocomando);

                BrightIdeasSoftware.OLVColumn colParametri = new BrightIdeasSoftware.OLVColumn();
                colParametri.Text = "Parametri";
                colParametri.AspectName = "Parametri";
                colParametri.Width = 250;
                colParametri.HeaderTextAlign = HorizontalAlignment.Left;
                colParametri.TextAlign = HorizontalAlignment.Left;
                flvListaComandiSIG.AllColumns.Add(colParametri);

                BrightIdeasSoftware.OLVColumn colHexdump = new BrightIdeasSoftware.OLVColumn();
                colHexdump.Text = "";
                colHexdump.AspectName = "HexdumpData";
                colHexdump.Width = 160;
                colHexdump.HeaderTextAlign = HorizontalAlignment.Left;
                colHexdump.TextAlign = HorizontalAlignment.Left;
                flvListaComandiSIG.AllColumns.Add(colHexdump);

                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvListaComandiSIG.AllColumns.Add(colRowFiller);

                flvListaComandiSIG.RebuildColumns();
                flvListaComandiSIG.SetObjects(ListaMessaggi);
                flvListaComandiSIG.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaImmagini: " + Ex.Message);
            }
        }

        /// <summary>
        /// Intercetta l'evento FormatRow della lista messaggi flvListaComandiSIG e colora la riga in base al tipo apparato.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormatRowEventArgs"/> instance containing the event data.</param>
        private void flvListaComandiSIG_FormatRow(object sender, FormatRowEventArgs e)
        {

            //coloro la riga in base al Tipo Device
            if (e.Model == null) return;
            echoMessaggio messaggio = (echoMessaggio)e.Model;
            switch (messaggio.Device)
            {
                case echoMessaggio.TipoDevice.Monitor:
                    e.Item.BackColor = Color.LightBlue;
                    break;
                case echoMessaggio.TipoDevice.LADELight: 
                    e.Item.BackColor = Color.LightGreen;
                    break;
                case echoMessaggio.TipoDevice.SPYBATT:
                    e.Item.BackColor = Color.LightYellow;
                    break;
                default:    // Device non gestito
                    e.Item.BackColor = Color.LightPink;
                    break;
            }



        }

        private void flvListaComandiSIG_FormatCell(object sender, FormatCellEventArgs e)
        {
            string _text = e.SubItem.Text;
            if (_text.Contains("NACK"))
            {
                //e.SubItem.Text = e.SubItem.Text.Substring(5);
                e.SubItem.ForeColor = Color.Red;
                e.SubItem.BackColor = Color.Yellow;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnSfogliaCarica_Click(object sender, EventArgs e)
        {
            try
            {

                ofdImportDati.Title = StringheComuni.ImportaDati;
                ofdImportDati.CheckFileExists = false;
                ofdImportDati.Filter = "POWERLINE Echo (*.llpwe)|*.llpwe|All files (*.*)|*.*";
                // Propongo come directory iniziale  user\documents\LADELIGHT Manager\LOGGING
                string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                _pathTeorico += "\\LADELIGHT Manager\\LOGGING\\POWERLINE";
                if (!Directory.Exists(_pathTeorico))
                {
                    Directory.CreateDirectory(_pathTeorico);
                }
                ofdImportDati.InitialDirectory = _pathTeorico;
                ofdImportDati.ShowDialog();

                txtEchoFilename.Text = ofdImportDati.FileName;



                //if (importaDati()) btnDataExport.Enabled = true;



            }
            catch (Exception Ex)
            {
                Log.Error("---------------- btnSfogliaCarica_Click ------------");
                Log.Error(Ex.Message);
            }
        }

        private void btnSfogliaSalva_Click(object sender, EventArgs e)
        {
            try
            {

                string _filename = "";

                sfdExportDati.Title = StringheComuni.EsportaDati;
                sfdExportDati.Filter = "POWERLINE Echo (*.llpwe)|*.llpwe|All files (*.*)|*.*";
                // Propongo come directory iniziale  user\documents\LADELIGHT Manager\LOGGING
                string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                _pathTeorico += "\\LADELIGHT Manager\\LOGGING\\POWERLINE";
                if (!Directory.Exists(_pathTeorico))
                {
                    Directory.CreateDirectory(_pathTeorico);
                }
                sfdExportDati.InitialDirectory = _pathTeorico;

                if (txtEchoFilename.Text != "")
                {

                    sfdExportDati.FileName = txtEchoFilename.Text;

                }

                sfdExportDati.ShowDialog();
                txtEchoFilename.Text = sfdExportDati.FileName;

            }
            catch (Exception Ex)
            {
                Log.Error("---------------- btnModCercaSalvaModello_Click ------------");
                Log.Error(Ex.Message);
            }
        }


        public bool SalvaFile(string NomeFile, bool Encode = false)
        {
            bool _esito = false;
            ushort _crc;
            elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
            try
            {
                if (NomeFile != "")
                {


                    Log.Debug("--- Esportazione Modello Dati ---");

                    modelloSigEcho DataModel = new modelloSigEcho();
                    DataModel.ListaMessaggi = ListaMessaggi;
                    DataModel.Note = txtEchoFileNote.Text;
                    DataModel.HexDump = FullDataBuffer;
                    DataModel.HexDumpText = txtSerialEcho.Text;


                    string _tempSer = JsonConvert.SerializeObject(DataModel);
                    byte[] _tepBSer = FunzioniMR.GetBytes(_tempSer);

                    _crc = codCrc.ComputeChecksum(_tepBSer);
                    DataModel.CRC = _crc;

                    if (!File.Exists(NomeFile)) File.Create(NomeFile).Close();
                    Log.Debug("file prepara esportazione");
                    string JsonData = JsonConvert.SerializeObject(DataModel);
                    Log.Debug("file generato");
                    string JsonEncript;
                    if (Encode)
                    {
                        // Ora cifro i dati
                        JsonEncript = StringCipher.Encrypt(JsonData);
                        Log.Debug("file criptato");
                    }
                    else
                    {
                        JsonEncript = JsonData;
                    }


                    File.WriteAllText(NomeFile, JsonEncript);
                    //  File.WriteAllText(NomeFile + ".txt", JsonData);

                    Log.Debug("file salvato");

                    //_datisalvati = true;
                    _esito = true;
                }

                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return _esito;
            }
        }

        /// <summary>
        /// Importa il file dati, deserializza la classe e verifica la coerenza dei dati
        /// </summary>
        /// <returns></returns>
        public bool CaricaFile(string NomeFile)
        {
            bool _esito = false;
            try
            {

                if (NomeFile != "")
                {

                    if (File.Exists(NomeFile))
                    {
                        Log.Debug("--- Importazione Log Sig 60 ---");
                        Log.Debug("Inizio Import");
                        string _fileDecripted = "";
                        string _fileImport = File.ReadAllText(NomeFile);
                        Log.Debug("file caricato: len = " + _fileImport.Length.ToString());

                        _fileDecripted = StringCipher.Decrypt(_fileImport);
                        if (_fileDecripted != "")
                        {
                            //il file è cifrato
                            Log.Debug("file criptato");
                            _fileImport = _fileDecripted;
                        }
                        Log.Debug("file decriptato");
                        modelloSigEcho _importData;
                        elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
                        ushort _crc;

                        _importData = JsonConvert.DeserializeObject<modelloSigEcho>(_fileImport);

                        Log.Debug("file convertito");

                        ushort _tempCRC = _importData.CRC;
                        _importData.CRC = 0;

                        string _tempSer = JsonConvert.SerializeObject(_importData);
                        byte[] _tempBSer = FunzioniMR.GetBytes(_tempSer);

                        // rivedere il controllo crc
                        _crc = _tempCRC;  // codCrc.ComputeChecksum(_tempBSer);
                        Log.Debug("file verificato");
                        ListaMessaggi = _importData.ListaMessaggi;
                        flvListaComandiSIG.SetObjects(ListaMessaggi);
                        flvListaComandiSIG.BuildList();
                        txtEchoFileNote.Text = _importData.Note;
                        txtSerialEcho.Text = _importData.HexDumpText;
                        FullDataBuffer = _importData.HexDump;
                        _dataBuffer = new byte[0];

                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception Ex)
            {

                Log.Error("CaricaFile Display: " + Ex.Message);
                return false;

            }
        }

        private void btnSigLogReset_Click(object sender, EventArgs e)
        {
            try
            {
                ListaMessaggi.Clear();
                flvListaComandiSIG.SetObjects(ListaMessaggi);
                flvListaComandiSIG.BuildList();
                txtSerialEcho.Text = "";
                FullDataBuffer = new byte[0];
                _dataBuffer = new byte[0];

            }
            catch (Exception Ex)
            {

                Log.Error("---------------- btnModSalvaModello_Click ------------");
                Log.Error(Ex.Message);
            }
        
    }

        private void btnSalvaRegistrazione_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEchoFilename.Text != "")
                {
                    SalvaFile(txtEchoFilename.Text, false);
                }

            }
            catch (Exception Ex)
            {

                Log.Error("---------------- btnModSalvaModello_Click ------------");
                Log.Error(Ex.Message);
            }
        }

        private void btnCariacaRegistrazione_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEchoFilename.Text != "")
                {
                    CaricaFile(txtEchoFilename.Text);
                }

            }
            catch (Exception Ex)
            {

                Log.Error("---------------- btnModSalvaModello_Click ------------");
                Log.Error(Ex.Message);
            }
        }


        private void RidimensionaControlli()
        {
            try
            {
                flvListaComandiSIG.Width = flvListaComandiSIG.Width;
                // dim base 1360
                if (this.Width > 800)
                {
                    flvListaComandiSIG.Width = this.Width - 330;
                    txtSerialEcho.Width = this.Width - 55;
                    btnChiudi.Left = this.Width - 132;
                }
                if (this.Height > 700)
                {
                    flvListaComandiSIG.Height = this.Height - 300;
                    txtSerialEcho.Top = this.Height - 270;
                    btnChiudi.Top =   this.Height - 90;
                }

            }
            catch (Exception Ex)
            {

                Log.Error(" RidimensionaControlli: " + Ex.Message);
            }
        }


        private void frmMonitorSig60_Resize(object sender, EventArgs e)
        {
            try
            {
                RidimensionaControlli();
            }
            catch (Exception Ex)
            {

                Log.Error(" frmMonitorSig60_Resize: " + Ex.Message);
            }
        }

        private void frmMonitorSig60_ResizeEnd(object sender, EventArgs e)
        {
            //RidimensionaControlli();
        }

        private void btnCmdStartCom_Click(object sender, EventArgs e)
        {
            try
            {

                _mS.Comando = MessaggioSpyBatt.TipoComando.SB_Sstart;
                _mS.SerialNumber = new byte[8]{ 1,1,1,1,0,0,0,0};
                _mS.ComponiMessaggio();

                Log.Debug("Send SB START");
                ///Log.Debug(_mS.hexdumpMessaggio());
                ScriviMessaggioByte(_mS.MessageBuffer);
                Thread.Sleep(200);

                txtSerialEcho.AppendText("\r\n");
            }

            catch (Exception Ex)
            {
                Log.Error("btnGetSigRegister_Click: " + Ex.Message);
            }
        }

        private void btnCmdStopCom_Click(object sender, EventArgs e)
        {
            try
            {

                _mS.Comando = MessaggioSpyBatt.TipoComando.SB_Stop;
                _mS.SerialNumber = new byte[8] { 1, 1, 1, 1, 0, 0, 0, 0 };
                _mS.ComponiMessaggio();

                Log.Debug("Send SB STOP");
                ScriviMessaggioByte(_mS.MessageBuffer);
                Thread.Sleep(200);

                txtSerialEcho.AppendText("\r\n");
            }

            catch (Exception Ex)
            {
                Log.Error("btnGetSigRegister_Click: " + Ex.Message);
            }
        }
    }

    /// <summary>
    /// EventArgs used to send bytes recieved on serial port
    /// </summary>
    public class SerialDataEventArgs : EventArgs
    {
        public SerialDataEventArgs(byte[] dataInByteArray)
        {
            Data = dataInByteArray;
        }

        /// <summary>
        /// Byte array containing data from serial port
        /// </summary>
        public byte[] Data;
    }
}
