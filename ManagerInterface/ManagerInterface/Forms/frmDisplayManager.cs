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
        private DisplaySetup.Immagine _tempImg = new DisplaySetup.Immagine();
        private DisplaySetup.Schermata _tempSch = new DisplaySetup.Schermata();
        private DisplaySetup.Comando _tempCmd = new DisplaySetup.Comando();

        public List<ModelloComando> ModComandi;

        BindingSource bsComandi = new BindingSource();


        public frmDisplayManager()
        {
            InitializeComponent();
            try
            {
                SerialPinChangedEventHandler1 = new SerialPinChangedEventHandler(PinChanged);
                ComPort = new SerialPort();
                _disp = new UnitaDisplay(ref ComPort);
                CaricaModelliComando();
                InizializzaVistaVariabili();
                InizializzaVistaImmagini();
                InizializzaVistaSchermate();
                bsComandi.DataSource = ModComandi;
                cmbSchTipoComando.DataSource = bsComandi.DataSource;
  
                cmbSchTipoComando.DisplayMember = "Nome";
                cmbSchTipoComando.ValueMember = "Codice";

            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }


        public void CaricaModelliComando()
        {
            try
            {
                ModComandi = new List<ModelloComando>();

                ModComandi.Add(new ModelloComando { Codice=ModelloComando.TipoComando.ScriviTesto6x8,    Nome = "Scrivi Testo 6x8",     ValoreStringa = true,   LunghezzaStringaChr = false, AltezzaStringaPix = false, CoordX = true, CoordY = true, Colore = true, TempoOn = false, TempoOff = false, NumeroVariabile = false, NumeroImmagine = false });
                ModComandi.Add(new ModelloComando { Codice=ModelloComando.TipoComando.ScriviOra6x8,      Nome = "Scrivi Ora 6x8",       ValoreStringa = false,  LunghezzaStringaChr = false, AltezzaStringaPix = false, CoordX = true, CoordY = true, Colore = true,TempoOn = false,TempoOff = false,NumeroVariabile = false,NumeroImmagine = false});
                ModComandi.Add(new ModelloComando { Codice=ModelloComando.TipoComando.ScriviData6x8,     Nome = "Scrivi Data 6x8",      ValoreStringa = false,  LunghezzaStringaChr = false, AltezzaStringaPix = false, CoordX = true, CoordY = true, Colore = true,TempoOn = false,TempoOff = false,NumeroVariabile = false,NumeroImmagine = false});
                ModComandi.Add(new ModelloComando { Codice=ModelloComando.TipoComando.ScriviTesto16,     Nome = "Scrivi Testo 16",      ValoreStringa = true,   LunghezzaStringaChr = false, AltezzaStringaPix = false, CoordX = true, CoordY = true, Colore = true,TempoOn = false,TempoOff = false,NumeroVariabile = false,NumeroImmagine = false});
                ModComandi.Add(new ModelloComando { Codice=ModelloComando.TipoComando.ScriviVariabile6x8,Nome = "Scrivi Variabile 6x8", ValoreStringa = false,  LunghezzaStringaChr = false, AltezzaStringaPix = false, CoordX = true, CoordY = true, Colore = true,TempoOn = false,TempoOff = false,NumeroVariabile = false,NumeroImmagine = false});
                ModComandi.Add(new ModelloComando { Codice=ModelloComando.TipoComando.ScriviVariabile16, Nome = "Scrivi Variabile 16",  ValoreStringa = false,  LunghezzaStringaChr = false, AltezzaStringaPix = false, CoordX = true, CoordY = true, Colore = true,TempoOn = false,TempoOff = false,NumeroVariabile = false,NumeroImmagine = false});
                ModComandi.Add(new ModelloComando { Codice=ModelloComando.TipoComando.DisegnaImmagine,   Nome = "Disegna Immagine",     ValoreStringa = false,  LunghezzaStringaChr = false, AltezzaStringaPix = false, CoordX = true, CoordY = true, Colore = true,TempoOn = false,TempoOff = false,NumeroVariabile = false,NumeroImmagine = false});
                ModComandi.Add(new ModelloComando { Codice=ModelloComando.TipoComando.ScrollImmagini,    Nome = "Scroll Immagini",      ValoreStringa = false,  LunghezzaStringaChr = false, AltezzaStringaPix = false, CoordX = true, CoordY = true, Colore = true,TempoOn = false,TempoOff = false,NumeroVariabile = false,NumeroImmagine = false});


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
                ComPort.DataBits = Convert.ToInt16(cboDataBits.Text);
                ComPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cboStopBits.Text);
                ComPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), cboHandShaking.Text);
                ComPort.Parity = (Parity)Enum.Parse(typeof(Parity), cboParity.Text);
                _dataBuffer = new byte[0];
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
                    btnApriComunicazione.ForeColor = Color.Black;
                    btnApriComunicazione.Enabled = true;
                    grbModInvioModello.Enabled = true;
                    btnVarInviaValore.Enabled = true;
                    cmdSchInviaSch.Enabled = true;

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
                    btnApriComunicazione.Enabled = false;
                    grbModInvioModello.Enabled = false;
                    btnVarInviaValore.Enabled = false;
                    cmdSchInviaSch.Enabled = false;
                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("VerificaStatoPorta: " + Ex.Message);
                return false;
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

                byte _valRedDx = FunzioniMR.ConvertiByte(txtRtValRedDx.Text, 1, 0);
                byte _valGreenDx = FunzioniMR.ConvertiByte(txtRtValGreenDx.Text, 1, 0);
                byte _valBluDx = FunzioniMR.ConvertiByte(txtRtValBluDx.Text, 1, 0);

                byte _valOn = FunzioniMR.ConvertiByte(txtRtValTimeOn.Text, 1, 0);
                byte _valOff = FunzioniMR.ConvertiByte(txtRtValTimeOff.Text, 1, 0);

                byte _valOnDx = FunzioniMR.ConvertiByte(txtRtValTimeOnDx.Text, 1, 0);
                byte _valOffDx = FunzioniMR.ConvertiByte(txtRtValTimeOffDx.Text, 1, 0);

                verifica = _disp.ImpostaLed(_valRed, _valGreen, _valBlu, _valOn, _valOff, _valRedDx, _valGreenDx, _valBluDx, _valOnDx, _valOffDx);

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

                verifica = _disp.ImpostaLed(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

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
                //"Image files (*.jpg, *.jpeg, *.bmp, *.png) | *.jpg; *.jpeg; *.bmp; *.png";
                ofdImportDati.Filter = "Immagini (*.jpg, *.jpeg, *.bmp, *.png) | *.jpg; *.jpeg; *.bmp; *.png|All files (*.*)|*.*";
                // Propongo come directory iniziale  user\documents\LADELIGHT Manager\SPY-BATT

                string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
                    _tempImg = new DisplaySetup.Immagine();
                    _tempImg.bmpBase = new Bitmap(txtNuovoFile.Text);

                    pbxImgImmagine.BackColor = Color.Gray;
                    pbxImgImmagine.Image = _tempImg.bmpBase;
                    txtImgBaseSize.Text = _tempImg.bmpBase.Size.ToString();
                    txtImgBaseDimX.Text = _tempImg.bmpBase.Width.ToString();
                    txtImgBaseDimY.Text = _tempImg.bmpBase.Height.ToString();

                    // genero direttamente l'immagine trasformata
                    _tempImg.bmp = _tempImg.bmpBase;

                    pbxImgImmagine8b.BackColor = Color.Gray;
                    pbxImgImmagine8b.Image = _tempImg.bmp;
                    pbxImgImmagine8b.SizeMode = PictureBoxSizeMode.Normal;

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
                // al momento copio semplicemente liimmagine caricata

                _tempImg.bmp = _tempImg.bmpBase;

                pbxImgImmagine8b.BackColor = Color.Gray;
                /*
                _tempImg.bmp.SetPixel(0, 0, Color.Red);
                _tempImg.bmp.SetPixel(1, 0, Color.Red);
                _tempImg.bmp.SetPixel(2, 0, Color.Red);
                */
                pbxImgImmagine8b.Image = _tempImg.bmp;
                pbxImgImmagine8b.SizeMode = PictureBoxSizeMode.Normal;

            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void btnImgGeneraArrayImmagine_Click(object sender, EventArgs e)
        {
            DisplaySetup.Immagine _img = new DisplaySetup.Immagine();

            //_img.SetDemo5();
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

                bool _VerificaNumero;
                ushort _nuovoid;
                /*
                DisplaySetup.Immagine _img = new DisplaySetup.Immagine();

                _img.SetDemo5();
                _img.GeneraImmagine();
                pbxImgImmagine1b.Image = _img.bmp;

                verifica = _disp.CaricaImmagine(_img);
                */

                //  Verifico che lID sia valido 


                _VerificaNumero = ushort.TryParse(txtImgIdImmagine.Text, out _nuovoid);
                if (!_VerificaNumero)
                {
                    // aggiungere messaggio
                    return;
                }
                if (_nuovoid < 1)
                    _nuovoid = 1;
                if (_nuovoid > 255)
                    _nuovoid = 255;
                txtImgNomeImmagine.Text = "IMAGE" + _nuovoid.ToString("000");
                txtImgIdImmagine.Text = _nuovoid.ToString();

                // verifico se l'ID è presente e nel caso tolgo l'esistente
                _VerificaNumero = false;
                foreach (DisplaySetup.Immagine _item in _disp.Data.Modello.Immagini)
                {
                    if (_item.Id == _nuovoid)
                    {
                        // aggiungere messaggio per duplicato
                        _disp.Data.Modello.Immagini.Remove(_item);
                        break;
                    }
                }
                _tempImg.Id = _nuovoid;
                _tempImg.Nome = txtImgNomeImmagineLista.Text;
                _disp.Data.Modello.Immagini.Add(_tempImg);
                InizializzaVistaImmagini();
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
                pbxSchImmagine.Width = 240;
                pbxSchImmagine.Height = 128;



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
                    flvSchListaSchermate.SetObjects(_disp.Data.Modello.Schermate);
                    flvSchListaSchermate.BuildList();

                    cmbRtValVariabile.DataSource = _disp.Data.Modello.Variabili;
                    cmbRtValVariabile.DisplayMember = "Nome";
                    cmbRtValVariabile.ValueMember = "Id";

                    cmbSchIdVariabile.DataSource = _disp.Data.Modello.Variabili;
                    cmbSchIdVariabile.DisplayMember = "Nome";
                    cmbSchIdVariabile.ValueMember = "Id";
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

                if (_valId > 20)
                {
                    _valId = 20;
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


                BrightIdeasSoftware.OLVColumn colValVaribile = new BrightIdeasSoftware.OLVColumn();
                colValVaribile.Text = "Valore";
                colValVaribile.AspectName = "Valore";
                colValVaribile.Width = 200;
                colValVaribile.HeaderTextAlign = HorizontalAlignment.Left;
                colValVaribile.TextAlign = HorizontalAlignment.Left;
                flvVarListaVariabili.AllColumns.Add(colValVaribile);


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


        /// <summary>
        /// Carico la lista delle variabili definite
        /// </summary>
        private void InizializzaVistaSchermate()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvSchListaSchermate.HeaderUsesThemes = false;
                flvSchListaSchermate.HeaderFormatStyle = _stile;
                flvSchListaSchermate.UseAlternatingBackColors = true;
                flvSchListaSchermate.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvSchListaSchermate.AllColumns.Clear();

                flvSchListaSchermate.View = View.Details;
                flvSchListaSchermate.ShowGroups = false; 
                flvSchListaSchermate.GridLines = true;

                BrightIdeasSoftware.OLVColumn colIdImg = new BrightIdeasSoftware.OLVColumn();
                colIdImg.Text = "ID";
                colIdImg.AspectName = "Id";
                colIdImg.Width = 20;
                colIdImg.HeaderTextAlign = HorizontalAlignment.Left;
                colIdImg.TextAlign = HorizontalAlignment.Right;
                flvSchListaSchermate.AllColumns.Add(colIdImg);

                BrightIdeasSoftware.OLVColumn colNomeImg = new BrightIdeasSoftware.OLVColumn();
                colNomeImg.Text = "Nome";
                colNomeImg.AspectName = "NomeLista";
                colNomeImg.Width = 200;
                colNomeImg.HeaderTextAlign = HorizontalAlignment.Left;
                colNomeImg.TextAlign = HorizontalAlignment.Left;
                flvSchListaSchermate.AllColumns.Add(colNomeImg);

                BrightIdeasSoftware.OLVColumn colWidthImg = new BrightIdeasSoftware.OLVColumn();
                colWidthImg.Text = "Cmd";
                colWidthImg.AspectName = "strNumComandi";
                colWidthImg.Width = 20;
                colWidthImg.HeaderTextAlign = HorizontalAlignment.Left;
                colWidthImg.TextAlign = HorizontalAlignment.Left;
                flvSchListaSchermate.AllColumns.Add(colWidthImg);


                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvSchListaSchermate.AllColumns.Add(colRowFiller);

                flvSchListaSchermate.RebuildColumns();
                flvSchListaSchermate.SetObjects(_disp.Data.Modello.Schermate);
                flvSchListaSchermate.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaSchermate: " + Ex.Message);
            }
        }


        /// <summary>
        /// Carico la lista dei comandi definiti per la schermata
        /// </summary>
        private void InizializzaVistaComandi()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvSchListaComandi.HeaderUsesThemes = false;
                flvSchListaComandi.HeaderFormatStyle = _stile;
                flvSchListaComandi.UseAlternatingBackColors = true;
                flvSchListaComandi.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvSchListaComandi.AllColumns.Clear();

                flvSchListaComandi.View = View.Details;
                flvSchListaComandi.ShowGroups = false;
                flvSchListaComandi.GridLines = true;

                BrightIdeasSoftware.OLVColumn colIdImg = new BrightIdeasSoftware.OLVColumn();
                colIdImg.Text = "N°";
                colIdImg.AspectName = "Numero";
                colIdImg.Width = 20;
                colIdImg.HeaderTextAlign = HorizontalAlignment.Left;
                colIdImg.TextAlign = HorizontalAlignment.Right;
                flvSchListaComandi.AllColumns.Add(colIdImg);

                BrightIdeasSoftware.OLVColumn colNomeImg = new BrightIdeasSoftware.OLVColumn();
                colNomeImg.Text = "Nome";
                colNomeImg.AspectName = "DescAttivita";
                colNomeImg.Width = 100;
                colNomeImg.HeaderTextAlign = HorizontalAlignment.Left;
                colNomeImg.TextAlign = HorizontalAlignment.Left;
                flvSchListaComandi.AllColumns.Add(colNomeImg);

                BrightIdeasSoftware.OLVColumn colWidthImg = new BrightIdeasSoftware.OLVColumn();
                colWidthImg.Text = "Pos";
                colWidthImg.AspectName = "Posizione";
                colWidthImg.Width = 60;
                colWidthImg.HeaderTextAlign = HorizontalAlignment.Left;
                colWidthImg.TextAlign = HorizontalAlignment.Center;
                flvSchListaComandi.AllColumns.Add(colWidthImg);


                BrightIdeasSoftware.OLVColumn colColorImg = new BrightIdeasSoftware.OLVColumn();
                colColorImg.Text = "Col";
                colColorImg.AspectName = "Colore";
                colColorImg.Width = 40;
                colColorImg.HeaderTextAlign = HorizontalAlignment.Left;
                colColorImg.TextAlign = HorizontalAlignment.Center;
                flvSchListaComandi.AllColumns.Add(colColorImg);

                BrightIdeasSoftware.OLVColumn colNumVar = new BrightIdeasSoftware.OLVColumn();
                colNumVar.Text = "Var";
                colNumVar.AspectName = "IdVariabile";
                colNumVar.Width = 40;
                colNumVar.HeaderTextAlign = HorizontalAlignment.Left;
                colNumVar.TextAlign = HorizontalAlignment.Center;
                flvSchListaComandi.AllColumns.Add(colNumVar);

                BrightIdeasSoftware.OLVColumn colNumImg = new BrightIdeasSoftware.OLVColumn();
                colNumImg.Text = "Img";
                colNumImg.AspectName = "IdImmagine";
                colNumImg.Width = 40;
                colNumImg.HeaderTextAlign = HorizontalAlignment.Left;
                colNumImg.TextAlign = HorizontalAlignment.Center;
                flvSchListaComandi.AllColumns.Add(colNumImg);

                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvSchListaComandi.AllColumns.Add(colRowFiller);

                flvSchListaComandi.RebuildColumns();
                flvSchListaComandi.SetObjects(_tempSch.Comandi);
                flvSchListaComandi.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaSchermate: " + Ex.Message);
            }
        }


        private void btnStatoImgCarica_Click(object sender, EventArgs e)
        {
            try
            {
                ushort _valStart = FunzioniMR.ConvertiUshort(txtStatoImgStart.Text, 1, 0);
                ushort _valEnd = FunzioniMR.ConvertiUshort(txtStatoImgEnd.Text, 1, 256);
                _disp.Immagini.Clear();
                InizializzaVistaImmaginiPresenti();

                //Application.DoEvents();
                //Thread.Sleep(500);




                _disp.CaricaListaImmaginiPresenti(_valStart, _valEnd);

                InizializzaVistaImmaginiPresenti();



            }
            catch (Exception Ex)
            {
                Log.Error("btnStatoImgCarica_Click: " + Ex.Message);
            }
        }

        private void InizializzaVistaImmaginiPresenti()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvStatoListaImg.HeaderUsesThemes = false;
                flvStatoListaImg.HeaderFormatStyle = _stile;
                flvStatoListaImg.UseAlternatingBackColors = true;
                flvStatoListaImg.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvStatoListaImg.AllColumns.Clear();

                flvStatoListaImg.View = View.Details;
                flvStatoListaImg.ShowGroups = false;
                flvStatoListaImg.GridLines = true;
                flvStatoListaImg.CheckBoxes = false;
                flvStatoListaImg.FullRowSelect = true;

                BrightIdeasSoftware.OLVColumn colIdImg = new BrightIdeasSoftware.OLVColumn();
                colIdImg.Text = "ID";
                colIdImg.AspectName = "Id";
                colIdImg.Width = 30;
                colIdImg.HeaderTextAlign = HorizontalAlignment.Left;
                colIdImg.TextAlign = HorizontalAlignment.Right;
                flvStatoListaImg.AllColumns.Add(colIdImg);

                BrightIdeasSoftware.OLVColumn colNomeImg = new BrightIdeasSoftware.OLVColumn();
                colNomeImg.Text = "Nome";
                colNomeImg.AspectName = "Nome";
                colNomeImg.Width = 100;
                colNomeImg.HeaderTextAlign = HorizontalAlignment.Left;
                colNomeImg.TextAlign = HorizontalAlignment.Left;
                flvStatoListaImg.AllColumns.Add(colNomeImg);

                BrightIdeasSoftware.OLVColumn colWidthImg = new BrightIdeasSoftware.OLVColumn();
                colWidthImg.Text = "Largh";
                colWidthImg.AspectName = "Width";
                colWidthImg.Width = 50;
                colWidthImg.HeaderTextAlign = HorizontalAlignment.Left;
                colWidthImg.TextAlign = HorizontalAlignment.Left;
                flvStatoListaImg.AllColumns.Add(colWidthImg);


                BrightIdeasSoftware.OLVColumn colHeightImg = new BrightIdeasSoftware.OLVColumn();
                colHeightImg.Text = "Alt";
                colHeightImg.AspectName = "Height";
                colHeightImg.Width = 50;
                colHeightImg.HeaderTextAlign = HorizontalAlignment.Left;
                colHeightImg.TextAlign = HorizontalAlignment.Left;
                flvStatoListaImg.AllColumns.Add(colHeightImg);



                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvStatoListaImg.AllColumns.Add(colRowFiller);

                flvStatoListaImg.RebuildColumns();
                flvStatoListaImg.SetObjects(_disp.Immagini);
                flvStatoListaImg.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaImmaginiPresenti: " + Ex.Message);
            }
        }

        private void txtImgIdImmagine_Leave(object sender, EventArgs e)
        {
            try
            {
                bool _VerificaNumero;
                ushort _nuovoid;

                _VerificaNumero = ushort.TryParse(txtImgIdImmagine.Text, out _nuovoid);
                if (!_VerificaNumero)
                    _nuovoid = 1;
                if (_nuovoid < 1)
                    _nuovoid = 1;
                if (_nuovoid > 255)
                    _nuovoid = 255;
                txtImgNomeImmagine.Text = "IMAGE" + _nuovoid.ToString("000");
                txtImgIdImmagine.Text = _nuovoid.ToString();

            }
            catch (Exception Ex)
            {
                Log.Error("txtImgIdImmagine_Leave: " + Ex.Message);
            }
        }



        /// <summary>
        /// Se per il ciclo lungo corrente sono saricati i cicli  brevi, apre la finestra col dettaglio cicli
        /// </summary>
        private void MostraDettaglioImmagineSelezionata()
        {
            try
            {
                Log.Debug("MostraDettaglioImmagineSelezionata");

                if (flvImgListaImmagini.SelectedObject != null)
                {


                    DisplaySetup.Immagine _tempImLoc = (DisplaySetup.Immagine)flvImgListaImmagini.SelectedObject;
                    if (_tempImLoc.bmp != null)
                    {

                    }

                }
            }
            catch (Exception Ex)
            {
                Log.Error("MostraDettaglioImmagineSelezionata: " + Ex.Message);
            }

        }

        private void MostraImmagine(DisplaySetup.Immagine Image)
        {
            try
            {
                Log.Debug("MostraImmagine");

                if (Image != null)
                {
                    pbxImgImmagine.Image = Image.bmp;
                    pbxImgImmagine8b.Image = Image.bmpBase;
                    txtImgNomeImmagineLista.Text = Image.Nome;
                }
            }
            catch (Exception Ex)
            {
                Log.Error("MostraImmagine: " + Ex.Message);
            }

        }

        private void flvImgListaImmagini_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;
                
                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Immagine Img = (DisplaySetup.Immagine)_lista.SelectedObject;
                    MostraImmagine(Img);
                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }

        }

        private void btnImgMostraImmagine_Click(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = flvImgListaImmagini;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Immagine Img = (DisplaySetup.Immagine)_lista.SelectedObject;
                    MostraImmagine(Img);
                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }
        }

        private void btnImgRimuoviImmagine_Click(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = flvImgListaImmagini;

                if (_lista.SelectedObject != null)
                {

                    _disp.Data.Modello.Immagini.Remove((DisplaySetup.Immagine)_lista.SelectedObject);
                   // _lista.RemoveObject(_lista.SelectedObject);
                }
                InizializzaVistaImmagini();
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }
        }

        private void btnRtDrawSchermata_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;
                ushort _idImgs = FunzioniMR.ConvertiUshort(txtRtValSchId.Text, 1, 0);

                verifica = _disp.MostraSchermata(_idImgs );

            }
            catch (Exception Ex)
            {
                Log.Error("---------------- btnRtDrawImage_Click ------------");
                Log.Error(Ex.Message);
            }
        }

        private void btnSchCercaFile_Click(object sender, EventArgs e)
        {
            try
            {
                ofdImportDati.Title = StringheComuni.ImportaDati;
                ofdImportDati.CheckFileExists = false;
                //"Image files (*.jpg, *.jpeg, *.bmp, *.png) | *.jpg; *.jpeg; *.bmp; *.png";
                ofdImportDati.Filter = "Immagini (*.jpg, *.jpeg, *.bmp, *.png) | *.jpg; *.jpeg; *.bmp; *.png|All files (*.*)|*.*";
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

                txtSchNuovoFile.Text = ofdImportDati.FileName;
                //if (importaDati()) btnDataExport.Enabled = true;
            }
            catch (Exception Ex)
            {
                Log.Error("---------------- btnSchCercaFile_Click ------------");
                Log.Error(Ex.Message);

            }
        }

        private void btnSchCaricaFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSchNuovoFile.Text != "")
                {
                    _tempSch = new DisplaySetup.Schermata();
                    _tempSch.bmp = new Bitmap(txtSchNuovoFile.Text);

                    pbxSchImmagine.BackColor = Color.Gray;
                    pbxSchImmagine.Image = _tempSch.bmp;
                    txtSchBaseSize.Text = _tempSch.bmp.Size.ToString();
                    txtSchBaseWidth.Text = _tempSch.bmp.Width.ToString();
                    txtSchBaseHeigh.Text = _tempSch.bmp.Height.ToString();

                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void cmdSchGeneraClasse_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;

                bool _VerificaNumero;
                ushort _nuovoid;

                //  Verifico che lID sia valido 

                _VerificaNumero = ushort.TryParse(txtImgIdImmagine.Text, out _nuovoid);
                if (!_VerificaNumero)
                {
                    // aggiungere messaggio
                    return;
                }
                _VerificaNumero = ushort.TryParse(txtSchBaseID.Text, out _nuovoid);
                if (!_VerificaNumero)
                    _nuovoid = 1;
                if (_nuovoid < 1)
                    _nuovoid = 1;
                if (_nuovoid > 255)
                    _nuovoid = 255;
                txtSchBaseName.Text = "SCREN" + _nuovoid.ToString("000");
                txtSchBaseID.Text = _nuovoid.ToString();

                // verifico se l'ID è presente e nel caso tolgo l'esistente
                _VerificaNumero = false;
                foreach (DisplaySetup.Schermata _item in _disp.Data.Modello.Schermate)
                {
                    if (_item.Id == _nuovoid)
                    {
                        // aggiungere messaggio per duplicato
                        _disp.Data.Modello.Schermate.Remove(_item);
                        break;
                    }
                }
                _tempSch.Id = _nuovoid;
                _tempSch.Nome = "SCREN" + _nuovoid.ToString("000");
                _tempSch.NomeLista = txtSchBaseNomeLista.Text;
                _disp.Data.Modello.Schermate.Add(_tempSch);
                InizializzaVistaSchermate();
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void txtSchBaseID_Leave(object sender, EventArgs e)
        {
            try
            {
                bool _VerificaNumero;
                ushort _nuovoid;

                _VerificaNumero = ushort.TryParse(txtSchBaseID.Text, out _nuovoid);
                if (!_VerificaNumero)
                    _nuovoid = 1;
                if (_nuovoid < 1)
                    _nuovoid = 1;
                if (_nuovoid > 255)
                    _nuovoid = 255;
                txtSchBaseName.Text = "SCREN" + _nuovoid.ToString("000");
                txtSchBaseID.Text = _nuovoid.ToString();

            }
            catch (Exception Ex)
            {
                Log.Error("txtImgIdImmagine_Leave: " + Ex.Message);
            }

        }

        private void flvSchListaSchermate_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Schermata Sch = (DisplaySetup.Schermata)_lista.SelectedObject;
                    _tempSch = Sch;
                    MostraSchermata(_tempSch);
                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }
        }
        private void MostraSchermata(DisplaySetup.Schermata Screen)
        {
            try
            {
                Log.Debug("MostraSchermata");

                if (Screen != null)
                {
                    pbxSchImmagine.Image = Screen.bmp;
                    txtSchBaseNomeLista.Text = Screen.NomeLista;
                    txtSchBaseName.Text = Screen.Nome;
                    txtSchBaseID.Text = Screen.Id.ToString();
                    txtSchBaseWidth.Text = Screen.Width.ToString();

                }
                InizializzaVistaComandi();
            }
            catch (Exception Ex)
            {
                Log.Error("MostraSchermata: " + Ex.Message);
            }

        }

        private void cmdSchMostrtaSch_Click(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = flvSchListaSchermate;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Schermata Screen = (DisplaySetup.Schermata)_lista.SelectedObject;
                    _tempSch = Screen;
                    MostraSchermata(Screen);
                }
            }
            catch (Exception Ex)
            {
                Log.Error("cmdSchMostrtaSch_Click: " + Ex.Message);
            }
        }

        private void cmdSchInviaSch_Click(object sender, EventArgs e)
        {
            try
            {

                // se ho  una schermata valida, compongo il bytearray e spedisco
                FastObjectListView _lista = flvSchListaSchermate;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Schermata Screen = (DisplaySetup.Schermata)_lista.SelectedObject;
                    if (Screen.bmp != null)
                    {
                        Screen.BmpToBuffer();
                        _disp.CaricaSchermata(Screen);
                        _disp.ImpostaBacklight(true);
                        _disp.MostraSchermata(Screen.Id);
                        _disp.ImpostaBacklight(true);


                    }
                    MostraSchermata(Screen);
                }

            }
            catch (Exception Ex)
            {
                Log.Error("cmdSchInviaSch_Click: " + Ex.Message);
            }
        }

        private void cmdSchRimuoviElemento_Click(object sender, EventArgs e)
        {
            try
            {

                // se ho  una schermata valida, compongo il bytearray e spedisco
                FastObjectListView _lista = flvSchListaSchermate;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Schermata Screen = (DisplaySetup.Schermata)_lista.SelectedObject;
                    if (Screen.Id > 0 )
                    {
                        foreach (DisplaySetup.Schermata _item in _disp.Data.Modello.Schermate)
                        {
                            if (_item.Id == Screen.Id)
                            {
                                // aggiungere messaggio per duplicato
                                _disp.Data.Modello.Schermate.Remove(_item);
                                break;
                            }
                        }

                    }

                }
                InizializzaVistaSchermate();
            }
            catch (Exception Ex)
            {
                Log.Error("cmdSchInviaSch_Click: " + Ex.Message);
            }
        }

        private void cmdSchGeneraByteArray_Click(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = flvSchListaSchermate;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Schermata Screen = (DisplaySetup.Schermata)_lista.SelectedObject;
                    Screen.BmpToBuffer();
                }
            }
            catch (Exception Ex)
            {
                Log.Error("cmdSchGeneraByteArray_Click: " + Ex.Message);
            }
        }

        private void btnSchCmdAdd_Click(object sender, EventArgs e)
        {
            try
            {
                SalvaComando();
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void cmdSchNuovaSchermata_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;

                bool _VerificaNumero;
                ushort _nuovoid;

                _tempSch = new DisplaySetup.Schermata();

                MostraSchermata(_tempSch);
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void btnSchCmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;

                bool _VerificaNumero;
                ushort _nuovoid;

                _tempCmd = new DisplaySetup.Comando();
                MostraComando();
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void btnSchCmdDel_Click(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = flvSchListaComandi;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Comando Cmd = (DisplaySetup.Comando)_lista.SelectedObject;

                    _tempSch.Comandi.Remove(Cmd);

                    InizializzaVistaComandi(); 
                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }
        }

        private void btnSchCmdLoad_Click(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = flvSchListaComandi;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Comando Cmd = (DisplaySetup.Comando)_lista.SelectedObject;
                    _tempCmd = Cmd;
                    MostraComando();
                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }
        }

        public void MostraComando()
        {
            try
            {
                bool verifica;

                // Prima vuoto tutto

                txtSchCmdWidth.Text = "";
                txtSchCmdHeigh.Text = "";
                txtSchCmdPosX.Text = "";
                txtSchCmdPosY.Text = "";
                txtSchCmdColor.Text = "";
                txtSchCmdNumVar.Text = "";
                txtSchCmdLenVarChar.Text = "";
                txtSchCmdLenVarPix.Text = "";
                txtSchCmdNumImg.Text = "";
                txtSchCmdTempoON.Text = "";
                txtSchCmdTempoOFF.Text = "";
                txtSchCmdText.Text = "";
                txtSchCmdNum.Text = "0";

                if( _tempCmd != null )
                {
                    cmbSchTipoComando.SelectedItem = null;
                    foreach (ModelloComando _cmd in cmbSchTipoComando.Items)
                    {
                        if(_cmd.Codice == _tempCmd.Attivita)
                        {
                            cmbSchTipoComando.SelectedItem = _cmd;
                            break;
                        }
                    }
                    txtSchCmdWidth.Text = _tempCmd.LenPixStringa.ToString();
                    txtSchCmdHeigh.Text = _tempCmd.HighPixStringa.ToString();
                    txtSchCmdPosX.Text = _tempCmd.PosX.ToString();
                    txtSchCmdPosY.Text = _tempCmd.PosY.ToString();
                    txtSchCmdColor.Text = _tempCmd.Colore.ToString();
                    txtSchCmdNumVar.Text = _tempCmd.IdVariabile.ToString();
                    txtSchCmdLenVarChar.Text = _tempCmd.LenStringa.ToString();
                    cmbSchIdVariabile.SelectedValue = _tempCmd.IdVariabile;
                    txtSchCmdNumImg.Text = _tempCmd.NumImg.ToString();
                    txtSchCmdTempoON.Text = _tempCmd.TimeOnVar.ToString();
                    txtSchCmdTempoOFF.Text = _tempCmd.TimeOffVar.ToString();
                    txtSchCmdText.Text = _tempCmd.Messaggio;
                    txtSchCmdNum.Text = _tempCmd.Numero.ToString();
                    // ((Bitmap)pbxSchImmagine.Image).SetPixel(_tempCmd.PosX, _tempCmd.PosY, Color.Red);
                    // ((Bitmap)pbxSchImmagine.Image).SetPixel(_tempCmd.PosX +1, _tempCmd.PosY, Color.Red);
                    // ((Bitmap)pbxSchImmagine.Image).SetPixel(_tempCmd.PosX, _tempCmd.PosY +1, Color.Red);
                    // ((Bitmap)pbxSchImmagine.Image).SetPixel(_tempCmd.PosX+1, _tempCmd.PosY+1, Color.Red);
                    Pen pen = new Pen(Color.Red);
                    Graphics _g = pbxSchImmagine.CreateGraphics();
                    _g.DrawRectangle(pen, _tempCmd.PosX, _tempCmd.PosY, 12, 12);

                    System.Drawing.Font drawFont = new System.Drawing.Font("Calibri", 7);
                    System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
                    float x = (float)_tempCmd.PosX+1 ;
                    float y = _tempCmd.PosY+1 ;
                    System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
                    _g.DrawString(_tempCmd.Numero.ToString(), drawFont, drawBrush, x, y, drawFormat);


                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        public void SalvaComando()
        {
            try
            {
                bool verifica;
                byte _tempVal;
                _tempCmd = new DisplaySetup.Comando();

                // se non è selezionato un tipo comando esco
                if (cmbSchTipoComando.SelectedItem == null)
                    return;
                ModelloComando _cmd = (ModelloComando)cmbSchTipoComando.SelectedItem;
                _tempCmd.Attivita = _cmd.Codice;
                _tempCmd.DescAttivita = _cmd.Nome;



                if (byte.TryParse(txtSchCmdWidth.Text, out _tempVal))
                    _tempCmd.LenPixStringa = _tempVal;

                if (byte.TryParse(txtSchCmdHeigh.Text, out _tempVal))
                    _tempCmd.HighPixStringa = _tempVal;

                if (byte.TryParse(txtSchCmdPosX.Text, out _tempVal))
                    _tempCmd.PosX = _tempVal;

                if (byte.TryParse(txtSchCmdPosY.Text, out _tempVal))
                    _tempCmd.PosY = _tempVal;

                if (byte.TryParse(txtSchCmdColor.Text, out _tempVal))
                    _tempCmd.Colore = _tempVal;

                if (byte.TryParse(txtSchCmdNumVar.Text, out _tempVal))
                    _tempCmd.IdVariabile = _tempVal;

                if (byte.TryParse(txtSchCmdLenVarChar.Text, out _tempVal))
                    _tempCmd.LenStringa = _tempVal;

                if (byte.TryParse(txtSchCmdNumImg.Text, out _tempVal))
                    _tempCmd.NumImg = _tempVal;

                if (byte.TryParse(txtSchCmdTempoON.Text, out _tempVal))
                    _tempCmd.TimeOnVar = _tempVal;

                if (byte.TryParse(txtSchCmdTempoOFF.Text, out _tempVal))
                    _tempCmd.TimeOffVar = _tempVal;

                _tempCmd.Messaggio = txtSchCmdText.Text;

                if (byte.TryParse(txtSchCmdNum.Text, out _tempVal))
                    _tempCmd.Numero = _tempVal;

                // se nella lista c'è già lo stesso id, lo elimino
                foreach (DisplaySetup.Comando _item in _tempSch.Comandi)
                {
                    if (_item.Numero == _tempCmd.Numero)
                    {
                        _tempSch.Comandi.Remove(_item);
                        break;
                    }
                }



                // Per ora aggiungo e basta
                _tempSch.Comandi.Add(_tempCmd);
                MostraComando();
                InizializzaVistaComandi();
               
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void btnImgInviaImmagine_Click(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = flvImgListaImmagini;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Immagine Img = (DisplaySetup.Immagine)_lista.SelectedObject;
                    _tempImg = Img;
                    InviaImmagine();
                }
            }
            catch (Exception Ex)
            {
                Log.Error("btnImgInviaImmagine_Click: " + Ex.Message);
            }
        }

        private void InviaImmagine()
        {
            try
            {
                if (_tempImg == null)
                    return ;
                if (_tempImg.Id < 1)
                    return;

                _tempImg.Nome = "IMAGE" + _tempImg.Id.ToString("000");
                _tempImg.Width = (byte)_tempImg.bmp.Width;
                _tempImg.Height = (byte)_tempImg.bmp.Height;

                _tempImg.BmpToBuffer();

                _disp.CaricaImmagine(_tempImg);
                MostraImmagine(_tempImg);
                txtImgDimImmagine.Text = _tempImg.Size.ToString();
                txtImgDimX.Text = _tempImg.Width.ToString();
                txtImgDimY.Text = _tempImg.Height.ToString();

            }

            catch (Exception Ex)
            {
                Log.Error("InviaImmagine: " + Ex.Message);
            }
        }




        private void btnRtSetRTC_Click(object sender, EventArgs e)
        {
            try
            {

                _disp.ImpostaRTC();

            }

            catch (Exception Ex)
            {
                Log.Error("InviaImmagine: " + Ex.Message);
            }
        }

        private void btnRtSetBaudRate_Click(object sender, EventArgs e)
        {
            try
            {
                bool Esito;
                if (cmbRtBaudRate.SelectedIndex > 0)
                {
                    DisplaySetup.BaudRate Velocita = (DisplaySetup.BaudRate)cmbRtBaudRate.SelectedIndex;
                    Esito = _disp.ImpostaBaudrate(Velocita);
                    if (Esito && chkRtRiavvioAutomatico.Checked)
                    {
                        ChiudiPorta();
                        cboBaudRate.Text = cmbRtBaudRate.Text;
                        ApriPorta();

                        bool verifica = _disp.VerificaPresenza();

                        if (verifica)
                        {
                            btnApriComunicazione.ForeColor = Color.Green;
                        }
                        else
                        {
                            btnApriComunicazione.ForeColor = Color.Red;
                        }

                    }
                }
            }

            catch (Exception Ex)
            {
                Log.Error("InviaImmagine: " + Ex.Message);
            }
        }

        private void cmbRtValVariabile_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbRtValVariabile.SelectedValue != null)
                    txtRtIdVariabile.Text = cmbRtValVariabile.SelectedValue.ToString();
                else
                    txtVarIdVariabile.Text = "";


            }
            catch (Exception Ex)
            {
                Log.Error("cmbRtValVariabile_SelectedValueChanged: " + Ex.Message);
            }
        }

        private void cmbSchIdVariabile_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSchIdVariabile.SelectedValue != null)
                    txtSchCmdNumVar.Text = cmbSchIdVariabile.SelectedValue.ToString();
                else
                    txtSchCmdNumVar.Text = "";


            }
            catch (Exception Ex)
            {
                Log.Error("cmbRtValVariabile_SelectedValueChanged: " + Ex.Message);
            }
        }

        private void btnRtImpostaVariabile_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;
                byte _idVar = FunzioniMR.ConvertiByte(txtRtIdVariabile.Text, 1, 0);

                verifica = _disp.ImpostaVariabile(_idVar,txtRtValVariabile.Text);

            }
            catch (Exception Ex)
            {
                Log.Error("---------------- btnRtDrawImage_Click ------------");
                Log.Error(Ex.Message);
            }
        }

        private void AggiornaScheda (bool Immagini, bool Schermate, bool Variabili)
        {
            try
            {
                int _tentativo = 0;
                int _maxTentativi = 5;
                int _counter = 0;
                bool _esito;

                pgbModStatoInvio.Visible = true;

                if (Immagini)
                {
                    _counter = 0;
                    _esito = false;

                    pgbModStatoInvio.Minimum = 0;
                    pgbModStatoInvio.Value = 0;
                    pgbModStatoInvio.Maximum = _disp.Data.Modello.Immagini.Count;


                    txtModImmaginiTrasmesse.Text = _counter.ToString();
                    Application.DoEvents();

                    foreach (DisplaySetup.Immagine _img in _disp.Data.Modello.Immagini)
                    {
                        _counter++;
                        pgbModStatoInvio.Value = _counter;
                        Application.DoEvents();
                        for (_tentativo = 1; _tentativo <= _maxTentativi; _tentativo++)
                        {
                            _img.BmpToBuffer();
                            _esito = _disp.CaricaImmagine(_img);
                            
                            txtModImmaginiTrasmesse.Text = _counter.ToString() + "." + _tentativo.ToString();
                            Application.DoEvents();
                            if (_esito)
                            {
                                break;
                            }

                        }
                        if (!_esito)
                        {
                            MessageBox.Show("Caricamento immagine Fallito");
                            break;
                        }

                    }
                }


                _counter = 0;

                if (Schermate)
                {
                    _counter = 0;
                    _esito = false;

                    byte[] _sequenza = new byte[_disp.Data.Modello.Schermate.Count] ;
                    pgbModStatoInvio.Minimum = 0;
                    pgbModStatoInvio.Value = 0;
                    pgbModStatoInvio.Maximum = _disp.Data.Modello.Schermate.Count;

                    txtModSchermateTrasmesse.Text = _counter.ToString();
                    Application.DoEvents();

                    foreach (DisplaySetup.Schermata _sch in _disp.Data.Modello.Schermate)
                    {
                        _sch.BmpToBuffer();

                        _counter++;
                        pgbModStatoInvio.Value = _counter;
                        Application.DoEvents();
                        for (_tentativo = 1; _tentativo <= _maxTentativi; _tentativo++)
                        {
                            _esito = _disp.CaricaSchermata(_sch);
                            _sequenza[_counter-1] = (byte)_sch.Id;
                            
                            txtModSchermateTrasmesse.Text = _counter.ToString() + "." + _tentativo.ToString();
                            Application.DoEvents();
                            if (_esito)
                            {
                                break;
                            }

                        }
                        if (!_esito)
                        {
                            MessageBox.Show("Caricamento schermata Fallito");
                            break;
                        }

                    }

                    _disp.ScrollSchermate(_sequenza, 15);


                }

                _counter = 0;

                if (Variabili)
                {
                    _counter = 0;
                    _esito = false;

                    pgbModStatoInvio.Minimum = 0;
                    pgbModStatoInvio.Value = 0;
                    pgbModStatoInvio.Maximum = _disp.Data.Modello.Variabili.Count;

                    txtModVariabiliTrasmesse.Text = _counter.ToString();
                    Application.DoEvents();

                    foreach (DisplaySetup.Variabile _var in _disp.Data.Modello.Variabili)
                    {

                        _counter++;
                        pgbModStatoInvio.Value = _counter;
                        Application.DoEvents();
                        for (_tentativo = 1; _tentativo <= _maxTentativi; _tentativo++)
                        {
                            _esito = _disp.ImpostaVariabile(_var.Id, _var.Valore);                            
                            txtModVariabiliTrasmesse.Text = _counter.ToString() + "." + _tentativo.ToString();
                            Application.DoEvents();
                            if (_esito)
                            {
                                break;
                            }

                        }
                        if (!_esito)
                        {
                            MessageBox.Show("Caricamento variabili Fallito");
                            break;
                        }
                    }


                }

                pgbModStatoInvio.Visible = false;
            }

            catch (Exception Ex)
            {
                Log.Error("AggiornaScheda: " + Ex.Message);
                pgbModStatoInvio.Visible = false;

            }
        }

        private void btnModInviaImmagini_Click(object sender, EventArgs e)
        {
            AggiornaScheda(true, false,false);
        }

        private void btnModInviaSchermate_Click(object sender, EventArgs e)
        {
            AggiornaScheda(false, true, false);
        }

        private void btnModInviaVariabili_Click(object sender, EventArgs e)
        {
            AggiornaScheda(false, false, true);
        }

        private void btnModAggiornaDisplay_Click(object sender, EventArgs e)
        {
            AggiornaScheda(true, true, true);
        }



        private void btnRtDrawSchSequence_Click(object sender, EventArgs e)
        {
            byte[] ListaSch;
            byte Attesa;
            try
            {

                if (txtRtSeqSchId.Text != "")
                {
                    ListaSch = FunzioniComuni.ToByteValueArray(txtRtSeqSchId.Text, ';', 1);
                    Attesa = FunzioniMR.ConvertiByte(txtRtSeqSchTime.Text, 1, 0);

                    _disp.ScrollSchermate(ListaSch, Attesa);
                }

                else
                {
                    return;
                }


            }
            catch (Exception Ex)
            {
                Log.Error("btnRtDrawSchSequence_Click: " + Ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void flvSchListaComandi_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = flvSchListaComandi;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Comando Cmd = (DisplaySetup.Comando)_lista.SelectedObject;
                    _tempCmd = Cmd;
                    MostraComando();
                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }
        }

        private void btnVarSalvaValore_Click(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = flvVarListaVariabili;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Variabile _TempVar = (DisplaySetup.Variabile)_lista.SelectedObject;
                    _TempVar.Valore = txtVarValore.Text;

                    InizializzaVistaVariabili();

                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }
        }

        private void flvVarListaVariabili_DoubleClick(object sender, EventArgs e)
        {

            try
            {
                FastObjectListView _lista = flvVarListaVariabili;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Variabile _TempVar = (DisplaySetup.Variabile)_lista.SelectedObject; 
                    txtVarValore.Text = _TempVar.Valore;

                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }
        }

        private void btnMemCFExec_Click(object sender, EventArgs e)
        {
            try
            {
                uint _StartAddr;
                int _bloccoCorrente;
                ushort _NumBlocchi;
                bool _esito;

                if (chkMemHex.Checked)
                {
                    if (uint.TryParse(txtMemCFStartAdd.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true) return;
                }
                else
                {

                    if (uint.TryParse(txtMemCFStartAdd.Text, out _StartAddr) != true) return;
                }


                _NumBlocchi = 0;
                if (ushort.TryParse(txtMemCFBlocchi.Text, out _NumBlocchi) != true) return;


                if (_NumBlocchi > 0)
                {

                    for (int _cicloBlocchi = 0; _cicloBlocchi < _NumBlocchi; _cicloBlocchi++)
                    {
                        _bloccoCorrente = _cicloBlocchi + 1;
                        _esito = _disp.CancellaBlocco4K(_StartAddr);
                        if (!_esito)
                        {
                            MessageBox.Show("Cancellazione del blocco " + _bloccoCorrente.ToString() + " non riuscita", "Cancellazione dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            _StartAddr += 0x1000;
                            txtMemCFStartAdd.Text = _StartAddr.ToString("X6");
                            txtMemCFBlocchi.Text = _bloccoCorrente.ToString();
                            Application.DoEvents();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Inserire un numero di blocchi valido", "Esportazione dati Corrente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }




            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }



        }

        private void btnVarInviaValore_Click(object sender, EventArgs e)
        {

            try
            {
                FastObjectListView _lista = flvVarListaVariabili;

                if (_lista.SelectedObject != null)
                {
                    DisplaySetup.Variabile _TempVar = (DisplaySetup.Variabile)_lista.SelectedObject;
                    bool verifica;
                    verifica = _disp.ImpostaVariabile(_TempVar.Id, _TempVar.Valore);

                }
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
            }


        }

        private void btnRtTestLed_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;

                verifica = _disp.ImpostaLed(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

                // 1 RED SX
                txtRtValRed.Text = "*";
                verifica = _disp.ImpostaLed(10, 0, 0, 10, 00, 0, 0, 0, 10, 0);
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
                verifica = _disp.ImpostaLed(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                txtRtValRed.Text = "";

                // 2 Green SX
                txtRtValGreen.Text = "*";
                verifica = _disp.ImpostaLed(0, 10, 0, 10, 00, 0, 0, 0, 10, 0);
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
                verifica = _disp.ImpostaLed(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                txtRtValGreen.Text = "";

                // 3 Blue SX
                txtRtValBlu.Text = "*";
                verifica = _disp.ImpostaLed(0, 0, 10, 10, 00, 0, 0, 0, 10, 0);
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
                verifica = _disp.ImpostaLed(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                txtRtValBlu.Text = "";

                // 4 RED DX
                txtRtValRedDx.Text = "*";
                verifica = _disp.ImpostaLed(0, 0, 0, 10, 0, 10, 0, 0, 10, 0);
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
                verifica = _disp.ImpostaLed(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                txtRtValRedDx.Text = "";

                // 5 RED DX
                txtRtValGreenDx.Text = "*";
                verifica = _disp.ImpostaLed(0, 0, 0, 10, 0, 0, 10, 0, 10, 0);
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
                verifica = _disp.ImpostaLed(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                txtRtValGreenDx.Text = "";

                // 6 RED DX
                txtRtValBluDx.Text = "*";
                verifica = _disp.ImpostaLed(0, 0, 0, 10, 0, 0, 0, 10, 10, 0);
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
                verifica = _disp.ImpostaLed(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                txtRtValBluDx.Text = "";


            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void tbpConnessione_Click(object sender, EventArgs e)
        {

        }

        private void btnRtResetBoard_Click(object sender, EventArgs e)
        {
 
            try
            {
                bool verifica;
                btnRtResetBoard.BackColor = Color.Blue;
                Application.DoEvents();
                if (_disp.ResetScheda())
                {
                    int milliseconds = 2000;
                    System.Threading.Thread.Sleep(milliseconds);
                    verifica = _disp.VerificaPresenza();

                    if (verifica)
                    {
                        btnRtResetBoard.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        btnRtResetBoard.BackColor = Color.LightPink;
                    }
                }
                else
                {
                    btnRtResetBoard.BackColor = Color.Red;
                }

                Application.DoEvents();


            }
            catch (Exception Ex)
            {
                Log.Error("---------------- frmDisplayManager_Load ------------");
                Log.Error(Ex.Message);
            }
        }

        private void btnRtLeggiPulsanti_Click(object sender, EventArgs e)
        {
            try
            {
                bool verifica;
                if (_disp.LeggiStatoScheda())
                {
                    txtRtValBtn01.Text = "0x" + _disp.StatoAttualeScheda.Pulsante1.ToString("x2");
                    txtRtValBtn02.Text = "0x" + _disp.StatoAttualeScheda.Pulsante2.ToString("x2");
                    txtRtValBtn03.Text = "0x" + _disp.StatoAttualeScheda.Pulsante3.ToString("x2");
                    txtRtValBtn04.Text = "0x" + _disp.StatoAttualeScheda.Pulsante4.ToString("x2");
                    txtRtValBtn05.Text = "0x" + _disp.StatoAttualeScheda.Pulsante5.ToString("x2");

                }
                else
                {
                    txtRtValBtn01.Text = "";
                    txtRtValBtn02.Text = "";
                    txtRtValBtn03.Text = "";
                    txtRtValBtn04.Text = "";
                    txtRtValBtn05.Text = "";

                }
            }
            catch (Exception Ex)
            {
                Log.Error("btnRtLeggiPulsanti_Click: " + Ex.Message);
            }

        }

        private void btnStatoSchCarica_Click(object sender, EventArgs e)
        {

        }
    }
    


}
    
