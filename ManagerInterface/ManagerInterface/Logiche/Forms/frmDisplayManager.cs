﻿using System;
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
    public partial class frmDisplayManager : Form
    {

        SerialPort ComPort;
        //public parametriSistema vGlobali;

        private Queue<byte> recievedData = new Queue<byte>();

        elementiComuni.modoDati modo = elementiComuni.modoDati.Import;

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
                InizializzaVistaVariabili();
                InizializzaVistaImmagini();
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmdMemRead_Click(object sender, EventArgs e)
        {
            try
            {
                uint _StartAddr;
                ushort _NumByte;
                bool _esito;

                if (chkMemHex.Checked)
                {
                    if (uint.TryParse(txtMemAddrR.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true) return;
                }
                else
                {

                    if (uint.TryParse(txtMemAddrR.Text, out _StartAddr) != true) return;
                }



                if (ushort.TryParse(txtMemLenR.Text, out _NumByte) != true) return;

                if (_NumByte < 1) _NumByte = 1;
                if (_NumByte > 242) _NumByte = 242;
                txtMemLenR.Text = _NumByte.ToString();

                if (_StartAddr < 0) _StartAddr = 0;
                if (chkMemHex.Checked)
                    txtMemAddrR.Text = _StartAddr.ToString("X6");
                else
                    txtMemAddrR.Text = _StartAddr.ToString();

                txtMemDataGrid.Text = "";
                _esito = LeggiBloccoMemoria(_StartAddr, _NumByte);


            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }

        }

        public bool LeggiBloccoMemoria(uint StartAddr, ushort NumByte)
        {
            try
            {

                byte[] _Dati;
                bool _esito;

                txtMemDataGrid.Text = "";
                _Dati = new byte[NumByte];
                _esito = _disp.LeggiBloccoMemoria(StartAddr, NumByte, out _Dati);


                // Salto il byte 0 che contiene il comando
                if (_esito == true)
                {


                    string _risposta = "";
                    int _colonne = 0;
                    for (int _i = 1; _i < _Dati.Length; _i++)
                    {
                        _risposta += _Dati[_i].ToString("X2") + " ";
                        _colonne += 1;
                        if (_colonne > 0 && (_colonne % 4) == 0) _risposta += "  ";
                        if (_colonne > 15)
                        {
                            _risposta += "\r\n";
                            _colonne = 0;

                        }
                    }
                    txtMemDataGrid.Text = _risposta;

                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LeggiBloccoMemoria: " + Ex.Message);
                return false;
            }

        }


        private void btnSfoglia_Click(object sender, EventArgs e)
        {


            if (modo == elementiComuni.modoDati.Output)
            {
                string _filename = "";

                sfdExportDati.Title = StringheComuni.EsportaDati;
                sfdExportDati.Filter = "SPY-BATT exchange data (*.sbdata)|*.sbdata|All files (*.*)|*.*";
                // Propongo come directory iniziale  user\documents\LADELIGHT Manager\SPY-BATT
                string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                _pathTeorico += "\\LADELIGHT Manager\\SPY-BATT";
                if (!Directory.Exists(_pathTeorico))
                {
                    Directory.CreateDirectory(_pathTeorico);
                }
                sfdExportDati.InitialDirectory = _pathTeorico;

                if (txtNuovoFile.Text != "")
                {

                    sfdExportDati.FileName = txtNuovoFile.Text;

                }

                sfdExportDati.ShowDialog();
                txtNuovoFile.Text = sfdExportDati.FileName;





            }
            else
            {
                ofdImportDati.Title = StringheComuni.ImportaDati;
                ofdImportDati.CheckFileExists = false;
                //ofdImportDati.Filter = "Images (*.sbdata)|*.sbdata|All files (*.*)|*.*";
                // Propongo come directory iniziale  user\documents\LADELIGHT Manager\SPY-BATT
                string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                /*
                _pathTeorico += "\\LADELIGHT Manager\\SPY-BATT";
                if (!Directory.Exists(_pathTeorico))
                {
                    Directory.CreateDirectory(_pathTeorico);
                }
                sfdExportDati.InitialDirectory = _pathTeorico;
                */
                ofdImportDati.ShowDialog();
                
                txtNuovoFile.Text = ofdImportDati.FileName;
                //if (importaDati()) btnDataExport.Enabled = true;
            }
        }

        private void btnImgCaricaFileImmagine_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNuovoFile.Text != "")
                {
                    Image image = Image.FromFile(txtNuovoFile.Text);
                    pbxImgImmagine.Image = image;

                    txtImgBaseDimX.Text = image.Width.ToString();
                    txtImgBaseDimY.Text = image.Height.ToString();
                    txtImgBaseSize.Text = image.Size.ToString();

                    //pbxImgImmagine.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void btnImgSimulaFileImmagine_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bmp = new Bitmap(240, 128);
                int step;
                for (int _y = 0; _y < 128; _y++)
                {
                    for (int _x = 0; _x < 240; _x++)
                    {


                    }
                }

                pbxImgImmagine8b.Image = bmp;
                pbxImgImmagine8b.SizeMode = PictureBoxSizeMode.Zoom;
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void btnImgGeneraArrrayImmagine_Click(object sender, EventArgs e)
        {
            DisplaySetup.Immagine _img = new DisplaySetup.Immagine();

            _img.SetDemo5();
            _img.GeneraImmagine();
            pbxImgImmagine1b.Image = _img.bmp;
            txtImgDimImmagine.Text = _img.Size.ToString();
            txtImgDimX.Text = _img.Width.ToString();
            txtImgDimY.Text = _img.Height.ToString();



        }

        private void btnImgGeneraClasse_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;

                DisplaySetup.Immagine _img = new DisplaySetup.Immagine();

                _img.SetDemo5();
                _img.GeneraImmagine();
                pbxImgImmagine1b.Image = _img.bmp;

                verifica = _disp.CaricaImmagine(_img);

            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void cmdMemClear_Click(object sender, EventArgs e)
        {
            CancellaMemoria();
        }

        void CancellaMemoria()
        {
            try
            {
                bool _esito;
                SerialMessage.Crc16Ccitt codCrc = new SerialMessage.Crc16Ccitt(SerialMessage.InitialCrcValue.NonZero1);


                DialogResult risposta = MessageBox.Show(StringheComuni.CancellaMemoriaR1 + "\n" + StringheComuni.CancellaMemoriaR2,
                StringheComuni.CancellaMemoria,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

                if (risposta == System.Windows.Forms.DialogResult.Yes)
                {

                    _esito = _disp.CancellaInteraMemoria();

                    if (_esito)
                    {
                        MessageBox.Show(StringheComuni.MemoriaCancellata, StringheComuni.CancellaMemoria, MessageBoxButtons.OK);
                    }

               

                }
            }
            catch (Exception Ex)
            {
                Log.Error("CancellaMemoria: " + Ex.Message);
            }
        }

        private void btnRtDrawImage_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;
                ushort _idImgs = FunzioniMR.ConvertiUshort(txtRtValImgId.Text, 1, 0);
                byte _valPosX = FunzioniMR.ConvertiByte(txtRtValImgPosX.Text, 1, 0);
                byte _valPosY = FunzioniMR.ConvertiByte(txtRtValImgPosY.Text, 1, 0);
                byte _valColor = FunzioniMR.ConvertiByte(txtRtValImgColor.Text, 1, 0);

                verifica = _disp.MostraImmagine(_idImgs, _valPosX, _valPosY, _valColor);

            }
            catch (Exception Ex)
            {
                Log.Error("---------------- btnRtDrawImage_Click ------------");
                Log.Error(Ex.Message);
            }
        }

        private void btnRtCLS_Click(object sender, EventArgs e)
        {
            bool verifica;

            verifica = _disp.PulisciSchermo();

        }

        private void frmDisplayManager_Load(object sender, EventArgs e)
        {

            try
            {
                pbxImgImmagine.Width = 240;
                pbxImgImmagine.Height = 128;
                pbxImgImmagine1b.Width = 240;
                pbxImgImmagine1b.Height = 128;
                pbxImgImmagine8b.Width = 240;
                pbxImgImmagine8b.Height = 128;

            }
            catch (Exception Ex)
            {
                Log.Error("---------------- frmDisplayManager_Load ------------");
                Log.Error(Ex.Message);
            }
        }
    

        private void btnModCercaSalvaModello_Click(object sender, EventArgs e)
        {
            try
            {

                string _filename = "";

                sfdExportDati.Title = StringheComuni.EsportaDati;
                sfdExportDati.Filter = "Display Setup (*.lldis)|*.lldis|All files (*.*)|*.*";
                // Propongo come directory iniziale  user\documents\LADELIGHT Manager\DISPLAY
                string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                _pathTeorico += "\\LADELIGHT Manager\\DISPLAY";
                if (!Directory.Exists(_pathTeorico))
                {
                    Directory.CreateDirectory(_pathTeorico);
                }
                sfdExportDati.InitialDirectory = _pathTeorico;

                if (txtNuovoFile.Text != "")
                {

                    sfdExportDati.FileName = txtNuovoFile.Text;

                }

                sfdExportDati.ShowDialog();
                txtModNomeFile.Text = sfdExportDati.FileName;

            }
            catch (Exception Ex)
            {
                Log.Error("---------------- btnModCercaSalvaModello_Click ------------");
                Log.Error(Ex.Message);
            }
        }

        private void btnModCercaCaricaModello_Click(object sender, EventArgs e)
        {
            try
            {

                ofdImportDati.Title = StringheComuni.ImportaDati;
                ofdImportDati.CheckFileExists = false;
                ofdImportDati.Filter = "Display Setup (*.lldis)|*.lldis|All files (*.*)|*.*";
                // Propongo come directory iniziale  user\documents\LADELIGHT Manager\DISPLAY
                string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                _pathTeorico += "\\LADELIGHT Manager\\DISPLAY";
                if (!Directory.Exists(_pathTeorico))
                {
                    Directory.CreateDirectory(_pathTeorico);
                }
                ofdImportDati.InitialDirectory = _pathTeorico;
                ofdImportDati.ShowDialog();

                txtModNomeFile.Text = ofdImportDati.FileName;



                //if (importaDati()) btnDataExport.Enabled = true;



            }
            catch (Exception Ex)
            {
                Log.Error("---------------- btnModCercaSalvaModello_Click ------------");
                Log.Error(Ex.Message);
            }
        }

        private void btnModNuovoModello_Click(object sender, EventArgs e)
        {
            _disp.Data = new DisplaySetup();
            DataDisplay();
        }

        private void ClearDisplay()
        {
            try
            {
                txtModNome.Text = "";
                txtModVersione.Text = "";
                txtModDataCre.Text = "";
                txtModDataMod.Text = "";
                txtModNote.Text = "";
                txtModNumImg.Text = "";
                txtModNumDis.Text = "";
                txtModNumVar.Text = "";
            }

            catch (Exception Ex)
            {
                Log.Error("---------------- DataDisplay ------------");
                Log.Error(Ex.Message);
            }
        }

        private void DataDisplay()
        {
            try
            {
                ClearDisplay();
                if (_disp.Data != null)
                {
                    txtModNome.Text = _disp.Data.Modello.NomeModello;
                    txtModVersione.Text = _disp.Data.Modello.Versione;
                    txtModDataCre.Text = _disp.Data.Modello.DataCreazione.ToString();
                    txtModDataMod.Text = _disp.Data.Modello.DataModifica.ToString();
                    txtModNote.Text = _disp.Data.Modello.Note;
                    txtModNumImg.Text = _disp.Data.Modello.Immagini.Count.ToString();
                    txtModNumDis.Text = _disp.Data.Modello.Schermate.Count.ToString();
                    txtModNumVar.Text = _disp.Data.Modello.Variabili.Count.ToString();
                    //Rigenero le liste
                    flvVarListaVariabili.SetObjects(_disp.Data.Modello.Variabili);
                    flvVarListaVariabili.BuildList();
                    flvImgListaImmagini.SetObjects(_disp.Data.Modello.Immagini);
                    flvImgListaImmagini.BuildList();

                }

            }
            catch (Exception Ex)
            {

                Log.Error("---------------- DataDisplay ------------");
                Log.Error(Ex.Message);
            }
        }

        private void txtModNome_Leave(object sender, EventArgs e)
        {
            _disp.Data.Modello.NomeModello = txtModNome.Text;
        }

        private void txtModVersione_Leave(object sender, EventArgs e)
        {
            _disp.Data.Modello.Versione = txtModVersione.Text;
        }

        private void txtModNote_Leave(object sender, EventArgs e)
        {
            _disp.Data.Modello.Note = txtModNote.Text;
        }

        private void btnModSalvaModello_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtModNomeFile.Text != "")
                {
                    _disp.Data.SalvaFile(txtModNomeFile.Text, false);
                    DataDisplay();
                }

            }
            catch (Exception Ex)
            {

                Log.Error("---------------- btnModSalvaModello_Click ------------");
                Log.Error(Ex.Message);
            }
        }

        private void btnModCaricaModello_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtModNomeFile.Text != "")
                {
                    _disp.Data.CaricaFile(txtModNomeFile.Text);
                    DataDisplay();

                }

            }
            catch (Exception Ex)
            {

                Log.Error("---------------- btnModCaricaModello_Click ------------");
                Log.Error(Ex.Message);
            }
        }

        private void btnVarCrea_Click(object sender, EventArgs e)
        {
            try
            {
                byte _valId = FunzioniMR.ConvertiByte(txtVarIdVariabile.Text, 1, 0);

                if ((txtVarIdVariabile.Text != "") && (txtVarNomeVariabile.Text != ""))
                {
                    // cerco se esiste già e nel caso cancello
                    DisplaySetup.Variabile _var = _disp.Data.Modello.Variabili.Find(x => x.Id == _valId);
                    if (_var == null)
                    {
                        _var = new DisplaySetup.Variabile();
                        _var.Id = _valId;
                        _var.Nome = txtVarNomeVariabile.Text;
                        _disp.Data.Modello.Variabili.Add(_var);
                    }
                    else
                    {
                        _var.Nome = txtVarNomeVariabile.Text;
                    }


                }
                flvVarListaVariabili.SetObjects(_disp.Data.Modello.Variabili);
                flvVarListaVariabili.BuildList();

            }
            catch (Exception Ex)
            {

                Log.Error("---------------- btnVarCrea_Click ------------");
                Log.Error(Ex.Message);
            }
        }

        private void txtVarIdVariabile_Leave(object sender, EventArgs e)
        {

            try
            {

                byte _valId = FunzioniMR.ConvertiByte(txtVarIdVariabile.Text, 1, 0);

                if (_valId < 1)
                {
                    _valId = 1;
                }

                if (_valId > 10)
                {
                    _valId = 10;
                }

                txtVarIdVariabile.Text = _valId.ToString();
                // cerco se esiste già e nel caso cancello
                DisplaySetup.Variabile _var = _disp.Data.Modello.Variabili.Find(x => x.Id == _valId);
                if (_var != null)
                {
                    txtVarNomeVariabile.Text = _var.Nome;
                }
                else
                {
                    txtVarNomeVariabile.Text = "";
                }


            }
            catch (Exception Ex)
            {

                Log.Error("---------------- txtVarIdVariabile_Leave ------------");
                Log.Error(Ex.Message);
            }
           
        }


        /// <summary>
        /// Carico la lista delle variabili definite
        /// </summary>
        private void InizializzaVistaVariabili()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvVarListaVariabili.HeaderUsesThemes = false;
                flvVarListaVariabili.HeaderFormatStyle = _stile;
                flvVarListaVariabili.UseAlternatingBackColors = true;
                flvVarListaVariabili.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvVarListaVariabili.AllColumns.Clear();

                flvVarListaVariabili.View = View.Details;
                flvVarListaVariabili.ShowGroups = false;
                flvVarListaVariabili.GridLines = true;

                BrightIdeasSoftware.OLVColumn colIdVariabile = new BrightIdeasSoftware.OLVColumn();
                colIdVariabile.Text = "ID";
                colIdVariabile.AspectName = "Id";
                colIdVariabile.Width = 30;
                colIdVariabile.HeaderTextAlign = HorizontalAlignment.Left;
                colIdVariabile.TextAlign = HorizontalAlignment.Right;
                flvVarListaVariabili.AllColumns.Add(colIdVariabile);

                BrightIdeasSoftware.OLVColumn colNomeVaribile = new BrightIdeasSoftware.OLVColumn();
                colNomeVaribile.Text = "Nome";
                colNomeVaribile.AspectName = "Nome";
                colNomeVaribile.Width = 300;
                colNomeVaribile.HeaderTextAlign = HorizontalAlignment.Left;
                colNomeVaribile.TextAlign = HorizontalAlignment.Left;
                flvVarListaVariabili.AllColumns.Add(colNomeVaribile);



                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvVarListaVariabili.AllColumns.Add(colRowFiller);

                flvVarListaVariabili.RebuildColumns();
                this.flvVarListaVariabili.SetObjects(_disp.Data.Modello.Variabili);
                flvVarListaVariabili.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }

        /// <summary>
        /// Carico la lista delle variabili definite
        /// </summary>
        private void InizializzaVistaImmagini()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvImgListaImmagini.HeaderUsesThemes = false;
                flvImgListaImmagini.HeaderFormatStyle = _stile;
                flvImgListaImmagini.UseAlternatingBackColors = true;
                flvImgListaImmagini.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvImgListaImmagini.AllColumns.Clear();

                flvImgListaImmagini.View = View.Details;
                flvImgListaImmagini.ShowGroups = false;
                flvImgListaImmagini.GridLines = true;

                BrightIdeasSoftware.OLVColumn colIdImg = new BrightIdeasSoftware.OLVColumn();
                colIdImg.Text = "ID";
                colIdImg.AspectName = "Id";
                colIdImg.Width = 30;
                colIdImg.HeaderTextAlign = HorizontalAlignment.Left;
                colIdImg.TextAlign = HorizontalAlignment.Right;
                flvImgListaImmagini.AllColumns.Add(colIdImg);

                BrightIdeasSoftware.OLVColumn colNomeImg = new BrightIdeasSoftware.OLVColumn();
                colNomeImg.Text = "Nome";
                colNomeImg.AspectName = "Nome";
                colNomeImg.Width = 200;
                colNomeImg.HeaderTextAlign = HorizontalAlignment.Left;
                colNomeImg.TextAlign = HorizontalAlignment.Left;
                flvImgListaImmagini.AllColumns.Add(colNomeImg);

                BrightIdeasSoftware.OLVColumn colWidthImg = new BrightIdeasSoftware.OLVColumn();
                colWidthImg.Text = "Largh";
                colWidthImg.AspectName = "Width";
                colWidthImg.Width = 50;
                colWidthImg.HeaderTextAlign = HorizontalAlignment.Left;
                colWidthImg.TextAlign = HorizontalAlignment.Left;
                flvImgListaImmagini.AllColumns.Add(colWidthImg);


                BrightIdeasSoftware.OLVColumn colHeightImg = new BrightIdeasSoftware.OLVColumn();
                colHeightImg.Text = "Alt";
                colHeightImg.AspectName = "Height";
                colHeightImg.Width = 50;
                colHeightImg.HeaderTextAlign = HorizontalAlignment.Left;
                colHeightImg.TextAlign = HorizontalAlignment.Left;
                flvImgListaImmagini.AllColumns.Add(colHeightImg);



                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvImgListaImmagini.AllColumns.Add(colRowFiller);

                flvImgListaImmagini.RebuildColumns();
                flvImgListaImmagini.SetObjects(_disp.Data.Modello.Immagini);
                flvImgListaImmagini.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaImmagini: " + Ex.Message);
            }
        }

    }
}