using ChargerLogic;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using BrightIdeasSoftware;


namespace PannelloCharger
{
    public partial class frmSetupLLT : Form
    {

        private static ILog Log = LogManager.GetLogger("frmSetupLLT");
        private static Queue<byte> codaDatiSER = new Queue<byte>();  // Buffer per la ricezione dati seriali

        public elementiComuni.TipoConnessione CanaleAttivo { get; set; }
        public ScannerUSB Scanner;
        public List<ScannerUSB.UsbDevice> ListaPorte;
        public parametriSistema _varGlobali;
        public LogicheBase logiche;
        public CicloDiCarica ParametriProfilo;
        public frmMain FormPrincipale { get; set; }
        bool SemaforoRicerca = false;   


        public frmSetupLLT(ref parametriSistema varGlobali, bool CaricaDati, string IdApparato, LogicheBase Logiche, bool SerialeCollegata, bool AutoUpdate)
        {
            try
            {


                InitializeComponent();
                //           UsbNotification.RegisterUsbDeviceNotification(this.Handle);

                Scanner = new ScannerUSB();
                _varGlobali = varGlobali;
                MostraListaUsb();
                AggiornaListaDevices();
                


            }
            catch (Exception Ex)
            {
                Log.Error("frmSetupLLT: " + Ex.Message);
            }

        }

        public void AttivaEventi()
        {
            try 
            {
                FormPrincipale.OnUsbChange += OnUsbConnectionChange;
            }
            catch (Exception Ex)
            {
                Log.Error("AttivaEventi: " + Ex.Message);
            }
        }

        public void AggiornaListaDevices()
        {
            try
            {
                pbRicerca.Visible = true;
                pbRicerca.Show();   
                ScannerUSB.EsitoScasione Esito = ScannerUSB.EsitoScasione.NonEseguita;
                if (Scanner == null) return;
                do
                {
                    Esito = Scanner.CercaPorteNew(999);
                    //Scanner.cercaPorte();
                    ListaPorte = Scanner.ListaPorte;
                    this.flvwListaDevices.SetObjects(ListaPorte);
                    flvwListaDevices.BuildList();
                } while ( !(Esito == ScannerUSB.EsitoScasione.Completa || Esito == ScannerUSB.EsitoScasione.InErrore));

                pbRicerca.Visible = false;
            }
            catch (Exception Ex)
            {
                Log.Error("Aggiorna Lista: " + Ex.Message);
                pbRicerca.Visible = false;
            }
        }

        public async Task<bool> AggiornaListaDevicesAsync()
        {
            try
            {
                if(SemaforoRicerca)
                {
                    Log.Info("SEMAFORO Form");
                    return true;    
                }
                else
                {
                    SemaforoRicerca = true;
                }


                pbRicerca.Visible = true;
                pbRicerca.Show();
                ScannerUSB.EsitoScasione Esito = ScannerUSB.EsitoScasione.NonEseguita;
                if (Scanner == null)
                {
                    SemaforoRicerca = false;
                    return false;
                }
                this.flvwListaDevices.SetObjects(ListaPorte);
                /*
                do
                {
                    
                    Esito = await Scanner.CercaPorteNewAsync();
                    //Scanner.cercaPorte();
                    ListaPorte = Scanner.ListaPorte;
                    //this.flvwListaDevices.SetObjects(ListaPorte);
                    //flvwListaDevices.BuildList();
                    if (Esito != ScannerUSB.EsitoScasione.Completa)
                    {
                        await Task.Delay(500);
                    }

                } while (!(Esito == ScannerUSB.EsitoScasione.Completa  ));
                */

                //this.flvwListaDevices.SetObjects(ListaPorte);

                await Scanner.CaricaPorteAsync();
                this.flvwListaDevices.SetObjects(ListaPorte);
                flvwListaDevices.BuildList();
                pbRicerca.Visible = false;
                pbRicerca.Hide();
                SemaforoRicerca = false;
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("Aggiorna Lista: " + Ex.Message);
                pbRicerca.Visible = false;
                SemaforoRicerca = false;
                return false;
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == UsbNotification.WmDevicechange)
            {
                switch ((int)m.WParam)
                {
                    case UsbNotification.DbtDeviceremovecomplete:
                        //Usb_DeviceRemoved(); // this is where you do your magic
                        listBox1.Items.Add("Device Removed");
                        break;
                    case UsbNotification.DbtDevicearrival:
                        //Usb_DeviceAdded(); // this is where you do your magic
                        listBox1.Items.Add("New Device Arrived");
                        break;
                }
            }

        }

        private async void btnAggiorna_Click(object sender, EventArgs e)
        {
           await AggiornaListaDevicesAsync();
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


                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "",
                    AspectName = "",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                    FillsFreeSpace = true,
                };
                flvwListaDevices.AllColumns.Add(colRowFiller);

                flvwListaDevices.RebuildColumns();

                this.flvwListaDevices.SetObjects(ListaPorte);
                flvwListaDevices.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("Mosra Lista: " + Ex.Message);
            }


        }

        public async void OnUsbConnectionChange(object sender, EventArgs e)
        {
            try
            {
                Log.Info("OnUsbConnectionChange");
                await AggiornaListaDevicesAsync();
            }
            catch (Exception Ex)
            {
                Log.Error("OnUsbConnectionChange: " + Ex.Message);
            }
        }

        private void btnConnetti_Click(object sender, EventArgs e)
        {
            pbRicerca.Visible = !pbRicerca.Visible;
            pbRicerca.Show();
        }

        private void frmSetupLLT_Load(object sender, EventArgs e)
        {
            AttivaEventi();
        }
    }
}

