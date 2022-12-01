using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using InTheHand.Net.Sockets;

using BrightIdeasSoftware;
using Utility;
using ChargerLogic;
using InTheHand.Net.Bluetooth;
using InTheHand.Net;
using System.IO;

namespace PannelloCharger
{
    public partial class frmSelettoreDevice : Form
    {

        private static ILog Log = LogManager.GetLogger("frmSelettoreDevice");
        private static Queue<byte> codaDatiSER = new Queue<byte>();  // Buffer per la ricezione dati seriali

        public elementiComuni.TipoConnessione  CanaleAttivo { get; set; }
        public ScannerUSB Scanner;
        public List<ScannerUSB.UsbDevice> ListaPorte;
        public List<DeviceBt> ListaWless;
        public parametriSistema _varGlobali;
        public LogicheBase logiche;
        public CicloDiCarica ParametriProfilo;
        

        private BackgroundWorker bgWlessSWcan;

        public frmSelettoreDevice()
        {
            InitializeComponent();
            InitializeWlessScanner();
        }

        public frmSelettoreDevice(ref parametriSistema varGlobali, elementiComuni.TipoConnessione Canale = elementiComuni.TipoConnessione.USB)
        {
            _varGlobali = varGlobali;
            CanaleAttivo = Canale;
            InitializeComponent();
            if(CanaleAttivo == elementiComuni.TipoConnessione.Bluetooth)
            {
                btnAggiorna.Enabled = false;
                btnAggiornaBT.Enabled = true;
                InitializeWlessScanner();
                MostraListaWless();
            }
            else
            {
                btnAggiorna.Enabled = true;
                btnAggiornaBT.Enabled = false;
                
            }


        }

        public void InitializeWlessScanner()
        {
            try
            {
                bgWlessSWcan = new BackgroundWorker();
                bgWlessSWcan.DoWork += new DoWorkEventHandler(WlessScanner_DoWork);
                bgWlessSWcan.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WlessScanner_WorkComplete);

            }
            catch (Exception Ex)
            {
                Log.Error("InitializeWlessScanner: " + Ex.Message);
            }
        }

        private void WlessScanner_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                List<DeviceBt> devices = new List<DeviceBt>();
                BluetoothClient bc = new BluetoothClient();
                BluetoothDeviceInfo[] array = bc.DiscoverDevices();

                int count = array.Length;
                for (int i = 0; i < count; i++)
                {
                    DeviceBt device = new DeviceBt(array[i]);
                    devices.Add(device);
                }
                e.Result = devices;

            }
            catch (Exception Ex)
            {
                Log.Error("WlessScanner_DoWork: " + Ex.Message);
            }
        }

        private void WlessScanner_WorkComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                List<DeviceBt> devices = (List<DeviceBt>)e.Result;
                ListaWless = devices;
                MostraListaWless();
                this.flvwListaDevices.SetObjects(ListaWless);
                flvwListaDevices.BuildList();
                pbRicerca.Style = ProgressBarStyle.Continuous;
                pbRicerca.MarqueeAnimationSpeed = 0;
                pbRicerca.Visible = false;
                // rigenera lista
                Log.Debug("WlessScanner_WorkComplete: ");
                // ... device_list.ItemsSource = (List<Device>)e.Result;
            }
            catch (Exception Ex)
            {
                Log.Error("WlessScanner_WorkComplete: " + Ex.Message);
            }
        }

        public void Aggiorna()
        {
            try
            {
                if (Scanner == null) return;
                Scanner.cercaPorte();
                ListaPorte = Scanner.ListaPorte;
                this.flvwListaDevices.SetObjects(ListaPorte);
                flvwListaDevices.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("Aggiorna Lista: " + Ex.Message);
            }
        }

        public void AggiornaDaUsb()
        {
            try
            {
                if (Scanner == null) return;
                Scanner.cercaPorte();
                ListaPorte = Scanner.ListaPorte;
                this.flvwListaDevices.SetObjects(ListaPorte);
                flvwListaDevices.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("Aggiorna Lista: " + Ex.Message);
            }
        }





        private void frmSelettoreDevice_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// definisco le colonne e collego i dati della lista dispositivi collegati.
        /// </summary>
        public void MostraListaUsb()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);

                flvwListaDevices.HeaderUsesThemes = false;
                flvwListaDevices.HeaderFormatStyle = _stile;

                flvwListaDevices.AllColumns.Clear();

                flvwListaDevices.View = View.Details;
                flvwListaDevices.ShowGroups = false;
                flvwListaDevices.GridLines = true;
                flvwListaDevices.UseAlternatingBackColors = true;
                flvwListaDevices.FullRowSelect = true;


                BrightIdeasSoftware.OLVColumn colCli = new BrightIdeasSoftware.OLVColumn();
                colCli.Text = "Description";
                colCli.AspectName = "Description";
                colCli.Width = 150;
                colCli.HeaderTextAlign = HorizontalAlignment.Left;
                colCli.TextAlign = HorizontalAlignment.Left;
                flvwListaDevices.AllColumns.Add(colCli);

                BrightIdeasSoftware.OLVColumn colBattMod = new BrightIdeasSoftware.OLVColumn();
                colBattMod.Text = "ID";
                colBattMod.AspectName = "strID";
                colBattMod.Width = 100;
                colBattMod.HeaderTextAlign = HorizontalAlignment.Center;
                colBattMod.TextAlign = HorizontalAlignment.Right;
                flvwListaDevices.AllColumns.Add(colBattMod);

                BrightIdeasSoftware.OLVColumn colNote = new BrightIdeasSoftware.OLVColumn();
                colNote.Text = "LocId";
                colNote.AspectName = "strLocId";
                colNote.Width = 50;
                colNote.HeaderTextAlign = HorizontalAlignment.Center;
                colNote.TextAlign = HorizontalAlignment.Left;
                flvwListaDevices.AllColumns.Add(colNote);

                BrightIdeasSoftware.OLVColumn colId = new BrightIdeasSoftware.OLVColumn();
                colId.Text = "SerialNumber";
                colId.AspectName = "SerialNumber";
                colId.Width = 100;
                colId.HeaderTextAlign = HorizontalAlignment.Left;
                colId.TextAlign = HorizontalAlignment.Left;
                flvwListaDevices.AllColumns.Add(colId);


                flvwListaDevices.RebuildColumns();

                this.flvwListaDevices.SetObjects(ListaPorte);
                flvwListaDevices.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("Mosra Lista: " + Ex.Message);
            }


        }

        public void MostraListaWless()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);

                flvwListaDevices.HeaderUsesThemes = false;
                flvwListaDevices.HeaderFormatStyle = _stile;

                flvwListaDevices.AllColumns.Clear();

                flvwListaDevices.View = View.Details;
                flvwListaDevices.ShowGroups = false;
                flvwListaDevices.GridLines = true;
                flvwListaDevices.UseAlternatingBackColors = true;
                flvwListaDevices.FullRowSelect = true;


                OLVColumn colNome = new OLVColumn()
                {
                    Text = "Name",
                    AspectName = "DeviceName",
                    Width = 200,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvwListaDevices.AllColumns.Add(colNome);

                OLVColumn colAuth = new OLVColumn()
                {
                    Text = "Auth",
                    AspectName = "strAuthenticated",
                    Width = 45,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.OK_16, "NO", Properties.Resources.KO_16, })

                };

                flvwListaDevices.AllColumns.Add(colAuth);

                OLVColumn colSPP = new OLVColumn()
                {
                    Text = "SPP",
                    AspectName = "strSppAvail",
                    Width = 45,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.OK_16, "NO", Properties.Resources.KO_16, })

                };

                flvwListaDevices.AllColumns.Add(colSPP);


                OLVColumn colConnect = new OLVColumn()
                {
                    Text = "Conn",
                    AspectName = "strConnected",
                    Width = 45,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.OK_16, "NO", Properties.Resources.KO_16, })

                };
                flvwListaDevices.AllColumns.Add(colConnect);

                OLVColumn colRSSI = new OLVColumn()
                {
                    Text = "RSSI",
                    AspectName = "strRSSI",
                    Width = 45,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    

                };

                flvwListaDevices.AllColumns.Add(colRSSI);



                flvwListaDevices.RebuildColumns();

                this.flvwListaDevices.SetObjects(ListaWless);
                flvwListaDevices.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("Mosra Lista: " + Ex.Message);
            }


        }





        private void btnChiudi_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void ApriDispositivoSelezionatoUSB()
        {
            try
            {
                if (flvwListaDevices.SelectedObject != null)
                {
                    // Preparo il baudrate specifico per il dispositivo
                    BaudRate BR = new BaudRate();

                    ScannerUSB.UsbDevice _tempCanale = (ScannerUSB.UsbDevice)flvwListaDevices.SelectedObject;
                    if (_tempCanale.SerialNumber != null)
                    {

                        switch (_tempCanale.Description)
                        {
                            case "PSW SUPERCHARGER":
                            case "SUPERCHARGER":
                                _varGlobali.usbLadeLightSerNum = _tempCanale.SerialNumber;
                                BR.Mode = BaudRate.BRType.BR_115200;
                                BR.Speed = 0;
                                _varGlobali.ActiveBaudRate = BR;
                                _varGlobali.CanaleLadeLight = parametriSistema.CanaleDispositivo.USB;
                                ApriSuperCharger();
                                return;

                            case "LADE LIGHT":
                                _varGlobali.usbLadeLightSerNum = _tempCanale.SerialNumber;
                                BR.Mode = BaudRate.BRType.BR_9600;
                                BR.Speed = 0;
                                _varGlobali.ActiveBaudRate = BR;
                                _varGlobali.CanaleLadeLight = parametriSistema.CanaleDispositivo.USB;
                                ApriLadeLight();
                                return;

                            case "SPY-BATT":
                                _varGlobali.usbSpyBattSerNum = _tempCanale.SerialNumber;
                                BR.Mode = BaudRate.BRType.BR_115200;
                                BR.Speed = 0;
                                _varGlobali.ActiveBaudRate = BR;
                                _varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                                ApriSpyBatt(_tempCanale.SerialNumber);
                                return;

                            case "FT201X USB I2C":
                                _varGlobali.usbSpyBattSerNum = _tempCanale.SerialNumber;
                                BR.Mode = BaudRate.BRType.BR_115200;
                                BR.Speed = 0;
                                _varGlobali.ActiveBaudRate = BR;
                                _varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                                ApriSpyBatt();
                                return;

                            case "SEQ-DESO":
                                //_tempCanale.Description = "SPY-BATT";
                                BR.Mode = BaudRate.BRType.BR_115200;
                                BR.Speed = 0;
                                _varGlobali.ActiveBaudRate = BR;
                                _varGlobali.usbSpyBattSerNum = _tempCanale.SerialNumber;
                                _varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                                ApriSpyBatt();
                                return;

                            case "REGENERATOR":
                            case "BATTERY REGENERATOR":
                            case "DESOLFATATORE":
                                //_tempCanale.Description = "DESOLFATATORE";
                                _varGlobali.usbSpyBattSerNum = _tempCanale.SerialNumber;
                                BR.Mode = BaudRate.BRType.BR_115200;
                                BR.Speed = 0;
                                _varGlobali.ActiveBaudRate = BR;
                                _varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                                ApriDesolfatatore();
                                return;

                            case "ID-BATT PROGRAMMER":
                                _varGlobali.usbSpyBattSerNum = _tempCanale.SerialNumber;
                                _varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                                ApriIdProgrammer(_tempCanale.SerialNumber);
                                return;

                                
                            default:
                                break;
                        }

                    }

                }
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.ApriLadeLight: " + Ex.Message);
            }

        }


        private void ApriSuperCharger()
        {
            try
            {
                bool esitoCanaleApparato = false;

                Log.Debug("Apri Canale SuperCharger");
                this.Cursor = Cursors.WaitCursor;

                // se la porta seriale non è aperta , la apro
                // -- rev 14/09  se già aperta chiudo e riapro             
                if (_varGlobali.statoCanaleLadeLight())
                {
                    Log.Debug("USB LadeLight aperto - lo richiudo");
                    _varGlobali.chiudiCanaleLadeLight();
                }

                _varGlobali.TipoCharger = CaricaBatteria.TipoCaricaBatteria.SuperCharger;

                esitoCanaleApparato = _varGlobali.apriLadeLight();
                if (esitoCanaleApparato)
                {
                    foreach (Form form in Application.OpenForms)
                    {
                        if (form.GetType() == typeof(frmSuperCharger))
                        {
                            form.Activate();
                            return;
                        }
                    }
                    Log.Debug("NUOVO SCHG ");

                    frmSuperCharger cbCorrente = new frmSuperCharger(ref _varGlobali, true, "", logiche, esitoCanaleApparato, true);
                    /*
                    if (!cbCorrente.ApparatoConnesso)
                    {
                        cbCorrente.Close();
                        MessageBox.Show("Nessuna risposta dal dispositivo selezionato", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    */
                    cbCorrente.Cursor = Cursors.WaitCursor;

                    cbCorrente.MdiParent = this.MdiParent;
                    cbCorrente.StartPosition = FormStartPosition.Manual;
                    cbCorrente.Location = new Point(5, 5);

                    //cbCorrente.ParametriProfilo = ParametriProfilo;
                    //cbCorrente.Cursor = Cursors.WaitCursor;
                    cbCorrente.Show();
                }
                else
                {
                    MessageBox.Show("Nessuna risposta dal canale selezionato", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.ApriLadeLight: " + Ex.Message);
                this.Cursor = Cursors.Default;
            }

        }

        private void ApriLadeLight()
        {
            try
            {
                bool esitoCanaleApparato = false;

                Log.Debug("Apri Canale Lade Light LL");
                this.Cursor = Cursors.WaitCursor;

                // se la porta seriale non è aperta , la apro
                // -- rev 14/09  se già aperta chiudo e riapro             
                if (_varGlobali.statoCanaleLadeLight())
                {
                    Log.Debug("USB LadeLight aperto - lo richiudo");
                    _varGlobali.chiudiCanaleLadeLight();
                }

                _varGlobali.TipoCharger = CaricaBatteria.TipoCaricaBatteria.LadeLight;
                esitoCanaleApparato = _varGlobali.apriLadeLight();
                if (esitoCanaleApparato)
                {
                    foreach (Form form in Application.OpenForms)
                    {
                        if (form.GetType() == typeof(frmCaricabatterieV2))
                        {
                            form.Activate();
                            return;
                        }
                    }
                    Log.Debug("NUOVO LL");

                    frmCaricabatterieV2 cbCorrente = new frmCaricabatterieV2(ref _varGlobali, true, "", logiche, esitoCanaleApparato, true);

                    if (!cbCorrente.ApparatoConnesso)
                    {
                        cbCorrente.Close();
                        MessageBox.Show("Nessuna risposta dal dispositivo selezionato", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return ;                        
                    }

                    cbCorrente.Cursor = Cursors.WaitCursor;

                    cbCorrente.MdiParent = this.MdiParent;
                    cbCorrente.StartPosition = FormStartPosition.Manual;
                    cbCorrente.Location = new Point(5, 5);

                    //cbCorrente.ParametriProfilo = ParametriProfilo;
                    //cbCorrente.Cursor = Cursors.WaitCursor;
                    cbCorrente.Show();
                }
                else
                {
                    MessageBox.Show("Nessuna risposta dal canale selezionato", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.ApriLadeLight: " + Ex.Message);
                this.Cursor = Cursors.Default;
            }

        }

        private void ApriSpyBatt(string IdScheda = "" )
        {
            try
            {
                bool esitoCanaleApparato = false;
                // se la porta seriale non è aperta , la apro
                // -- rev 14/09  se già aperta chiudo e riapro
                if (_varGlobali.statoCanaleSpyBatt()) _varGlobali.chiudiCanaleSpyBatt();

                this.Cursor = Cursors.WaitCursor;

                esitoCanaleApparato = _varGlobali.apriSpyBat();

                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmSpyBat))
                    {
                        frmSpyBat _tmpFrmSB = (frmSpyBat)form;
                        if (_tmpFrmSB.IdCorrente == IdScheda)
                        {
                            form.Activate();
                            return;
                        }
                    }
                }
                Log.Debug("NUOVO SB");
                frmSpyBat sbCorrente = new frmSpyBat(ref _varGlobali, true, "", logiche, esitoCanaleApparato, true);
                sbCorrente.MdiParent = this.MdiParent; 
                sbCorrente.StartPosition = FormStartPosition.CenterParent;

                this.Cursor = Cursors.Default;

                sbCorrente.Show();


            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.ApriSpyBatt: " + Ex.Message);
                this.Cursor = Cursors.Default;
            }

        }

        private void ApriIdProgrammer(string IdScheda = "")
        {
            try
            {
                bool esitoCanaleApparato = false;
                // se la porta seriale non è aperta , la apro
                // -- rev 14/09  se già aperta chiudo e riapro
                if (_varGlobali.statoCanaleSpyBatt()) _varGlobali.chiudiCanaleSpyBatt();

                this.Cursor = Cursors.WaitCursor;

                esitoCanaleApparato = _varGlobali.apriSpyBat();
                if (esitoCanaleApparato)
                {
                    foreach (Form form in Application.OpenForms)
                    {
                        if (form.GetType() == typeof(frmIdProgrammer))
                        {
                            frmIdProgrammer _tmpFrmIDP = (frmIdProgrammer)form;
                            if (_tmpFrmIDP.IdCorrente == IdScheda)
                            {
                                form.Activate();
                                return;
                            }
                        }
                    }
                    Log.Debug("NUOVO IDP");
                    frmIdProgrammer idpCorrente = new frmIdProgrammer(ref _varGlobali, true, "", logiche, esitoCanaleApparato, true);
                    idpCorrente.MdiParent = this.MdiParent; ;
                    idpCorrente.StartPosition = FormStartPosition.CenterParent;

                    this.Cursor = Cursors.Default;

                    idpCorrente.Show();
                }
                else
                {
                    MessageBox.Show("Nessuna risposta dal dispositivo selezionato", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.ApriSpyBatt: " + Ex.Message);
            }

        }



        private void ApriDesolfatatore()
        {
            try
            {
                bool esitoCanaleApparato = false;
                // se la porta seriale non è aperta , la apro
                // -- rev 14/09  se già aperta chiudo e riapro
                if (_varGlobali.statoCanaleSpyBatt()) _varGlobali.chiudiCanaleSpyBatt();

                this.Cursor = Cursors.WaitCursor;

                esitoCanaleApparato = _varGlobali.apriSpyBat();

                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmDesolfatatore))
                    {
                        form.Activate();
                        return;
                    }
                }
                Log.Debug("NUOVO Desolfatatore");
                frmDesolfatatore sbCorrente = new frmDesolfatatore(ref _varGlobali, true, "", logiche, esitoCanaleApparato, true);
                sbCorrente.MdiParent = this.MdiParent; ;
                sbCorrente.StartPosition = FormStartPosition.CenterParent;

                this.Cursor = Cursors.Default;

                Log.Debug("PRIMA");
                sbCorrente.Show();
                Log.Debug("DOPO");

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.ApriDesolfatatore: " + Ex.Message);
            }

        }


        private void btnConnetti_Click(object sender, EventArgs e)
        {
            ApriDispositivoSelezionatoUSB();
        }

        private void flvwListaDevices_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;
                object _tempObj = _lista.SelectedObject;
                Type TipoLink = _tempObj.GetType();

                if (TipoLink == typeof(DeviceBt))
                {
                    // --> Connetti BT
                    ConnettiApparatoBT();
                }
                else
                if (_lista.SelectedObject != null)
                {
                    ApriDispositivoSelezionatoUSB();
                }
            }
            catch (Exception Ex)
            {
                Log.Error("flvwListaDevices_MouseDoubleClick: " + Ex.Message);
            }
        }

        private bool raggiuntoTimeout(DateTime inizio, int SecondiTimeOut)
        {
            try
            {
                DateTime _ora = DateTime.Now;
                TimeSpan _durata = _ora.Subtract(inizio);

                if (_durata.TotalSeconds > SecondiTimeOut)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch { return true; }
        }


        bool ConnettiApparatoBT()
        {


            try
            {
                DateTime _startFunzione;
                bool InAttesa;
                BluetoothClient cli;

                if (flvwListaDevices.SelectedObject != null)
                {

                   DeviceBt _tempCanale = (DeviceBt)flvwListaDevices.SelectedObject;
                    if (!_tempCanale.Connected)
                    {

                        //BluetoothAddress addr = BluetoothAddress.Parse("001122334455");
                        Guid serviceClass;
                        serviceClass = BluetoothService.SerialPort;
                        // - or - etc
                        // serviceClass = MyConsts.MyServiceUuid
                        //
                        BluetoothAddress addr = _tempCanale.Address;

                        var ep = new InTheHand.Net.BluetoothEndPoint(addr, serviceClass);
                        cli = new BluetoothClient();
                        cli.Connect(ep);




                        //vuoto la coda
                        codaDatiSER = new Queue<byte>();  // Buffer per la ricezione dati seriali

                        _varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.BTStream;
                        _varGlobali.streamSpyBatt = cli.GetStream();
                        _varGlobali.streamSpyBatt.ReadTimeout = 100;
                        //streamSpyBatt.Write(messaggio, Start, NumByte);
                        //byte[] TempOut = { 0x02, 0x36, 0x45, 0x38, 0x31, 0x39, 0x42, 0x34, 0x36, 0x32, 0x38, 0x30, 0x30, 0x32, 0x45, 0x30, 0x30, 0x42, 0x43, 0x42, 0x43, 0x31, 0x43, 0xFF, 0x34, 0x34, 0x44, 0x38, 0x03 };
                        byte[] TempOut = { 0x02, 0x36, 0x45, 0x38, 0x31, 0x39, 0x42, 0x34, 0x36, 0x32, 0x38, 0x30, 0x30, 0x32, 0x45, 0x30, 0x30, 0x42, 0x43, 0x42, 0x43, 0x31, 0x37, 0xFF, 0x37, 0x41, 0x43, 0x42, 0x03 };
                        _varGlobali.streamSpyBatt.Write(TempOut, 0, TempOut.Length);
                        Log.Debug("Dati Inviati uart Wless " + TempOut.Length.ToString());
                        // ora mi metto in ascolto per un pacchetto per un max di 10"
                        int BytesCaricati = 0;
                        byte[] tempdata = new byte[8096];
                        _startFunzione = DateTime.Now;
                        InAttesa = true;
                        int DataAvailable = 0;
                        do
                        {

                            // verifico se ci sono dati
                            DataAvailable = cli.Available;
                            Log.Debug("DataAvailable = " + DataAvailable.ToString());
                            if (DataAvailable > 0)
                            {
                                BytesCaricati = _varGlobali.streamSpyBatt.Read(tempdata, 0, tempdata.Length);
                                Log.Debug("BytesCaricati = " + BytesCaricati.ToString());
                            }
                            else
                            {
                                BytesCaricati = 0;
                                
                            }

                            if (BytesCaricati > 0)
                            {

                                Log.Debug("Dati Ricevuti uart Wless " + BytesCaricati.ToString());
                                for (int _i = 0; _i < BytesCaricati; _i++)
                                {

                                    codaDatiSER.Enqueue(tempdata[_i]);


                                    if (tempdata[_i] == SerialMessage.serETX)
                                    {
                                        Log.Debug("Trovato Etx (USB), faccio ripartire il timeout - Dati in coda SB (USB) " + codaDatiSER.Count.ToString());
                                        string tempMsg = "";
                                        foreach (byte _data in codaDatiSER)
                                        {
                                            tempMsg += _data.ToString("X2");
                                        }
                                        codaDatiSER.Clear();
                                        Log.Debug(tempMsg);

                                        _startFunzione = DateTime.Now;
                                        InAttesa = false;
                                    }
                                }
                            }

                            System.Threading.Thread.Sleep(5);
                            if (raggiuntoTimeout(_startFunzione, 10 ))
                            {
                                Log.Debug("aspettaRisposta.BT raggiunto Timeout");
                                break;
                            }
                            //Log.Debug(DateTime.Now.ToShortTimeString());
                        }
                        while (InAttesa);
                        if (cli.Connected)
                        {
                            Log.Debug("Ricevuti i dati. chiudo la comunicazione");
                            cli.Dispose();
                        }
                        /*
                        Log.Debug("NUOVO SB Wless");
                        frmSpyBat sbCorrente = new frmSpyBat(ref _varGlobali, true, "", logiche,true, true);
                        sbCorrente.MdiParent = this.MdiParent; ;
                        sbCorrente.StartPosition = FormStartPosition.CenterParent;

                        this.Cursor = Cursors.Default;

                        sbCorrente.Show();
                        */
                    }

                }

                return true;
            }
            catch
            {
                return false;
            }
        }



        private void btnAggiorna_Click(object sender, EventArgs e)
        {
            Aggiorna();
        }

        public bool CercaWireless()
        {
            try
            {


                return true;

            }
            catch
            {
                return false;
            }
        }




        private void btnAggiornaBT_Click(object sender, EventArgs e)
        {
            if (!bgWlessSWcan.IsBusy)
            {
                //pb.Visibility = Visibility.Visible;

                ListaWless = new List<DeviceBt>();
                MostraListaWless();
                this.flvwListaDevices.SetObjects(ListaWless);
                flvwListaDevices.BuildList();

                pbRicerca.Style = ProgressBarStyle.Marquee;
                pbRicerca.MarqueeAnimationSpeed = 30;
                pbRicerca.Visible = true;
                bgWlessSWcan.RunWorkerAsync();
            }
        }
    }



}
