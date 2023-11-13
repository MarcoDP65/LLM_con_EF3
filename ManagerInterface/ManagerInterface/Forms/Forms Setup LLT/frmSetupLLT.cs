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
using MoriData;
using Utility;
using static ChargerLogic.MessaggioSpyBatt.EsitoMessaggio;
using System.Security.Policy;
using System.Globalization;

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
        public ModelliInitDisplay ModelliSetup;
        public SqInitArticolo TestataAttiva { get; set; }
        public List<SqStepInit> ListaStepAttivi = new List<SqStepInit>();
        public frmMain FormPrincipale { get; set; }
        bool SemaforoRicerca = false;

        private CaricaBatteria _cb;
        parametriSistema _parametri;
        SerialMessage _msg;
        LogicheBase _logiche;
        string CurrentSerialNumber { get; set; }
        private llParametriApparato _tempParametri;


        public frmSetupLLT(ref parametriSistema varGlobali, bool CaricaDati, string IdApparato, LogicheBase Logiche, bool SerialeCollegata, bool AutoUpdate)
        {
            try
            {


                InitializeComponent();
                // UsbNotification.RegisterUsbDeviceNotification(this.Handle);

                Scanner = new ScannerUSB();
                _varGlobali = varGlobali;
                MostraListaUsb();
                AggiornaListaDevices();
                ModelliSetup = new ModelliInitDisplay();
                ModelliSetup.InitLocale();
                AttivaControlli();


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
        public void AttivaControlli()
        {
            try
            {
                // Carico la combo procedure:
                cmbInitTipoApparato.DataSource = ModelliSetup.ListaModelli;
                cmbInitTipoApparato.DisplayMember = "DescrVoce";
                cmbInitTipoApparato.ValueMember = "IdInizializzazione";


            }
            catch (Exception Ex)
            {
                Log.Error("AttivaControlli: " + Ex.Message);
            }
        }

        public void AggiornaListaDevices()
        {
            try
            {
                pbRicerca.Visible = true;
                pbRicerca.Show();
                int Step = 0;
                ScannerUSB.EsitoScasione Esito = ScannerUSB.EsitoScasione.NonEseguita;
                if (Scanner == null) return;
                do
                {
                    Esito = Scanner.CercaPorteNew(999);
                    //Scanner.cercaPorte();
                    ListaPorte = Scanner.ListaPorte;
                    this.flvwListaDevices.SetObjects(ListaPorte);
                    flvwListaDevices.BuildList();
                    Step++;
                    if ( Step > 100 )
                    {
                        Esito = ScannerUSB.EsitoScasione.InErrore;
                    }
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
                bool SerPresente = false;
                if(CurrentSerialNumber != "")
                {
                    foreach( ScannerUSB.UsbDevice TempDev in ListaPorte) 
                    {
                        if(TempDev.SerialNumber == CurrentSerialNumber)
                        {
                            SerPresente = true;
                            break;
                        }
                    }
                }

                if( !SerPresente )
                {
                    VuotaDatiCorrenti();
                    VuotaConfigCorrente();
                    grbInitDatiBase.Enabled = false;
                    grbInizializzazione.Enabled = false;
                    btnEseguiInit.Enabled = false;
                }

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
                colCli.Width = 200;
                colCli.HeaderTextAlign = HorizontalAlignment.Left;
                colCli.TextAlign = HorizontalAlignment.Left;
                flvwListaDevices.AllColumns.Add(colCli);

                BrightIdeasSoftware.OLVColumn colBattMod = new BrightIdeasSoftware.OLVColumn();
                colBattMod.Text = "ID";
                colBattMod.AspectName = "strID";
                colBattMod.Width = 150;
                colBattMod.HeaderTextAlign = HorizontalAlignment.Center;
                colBattMod.TextAlign = HorizontalAlignment.Right;
                flvwListaDevices.AllColumns.Add(colBattMod);

                BrightIdeasSoftware.OLVColumn colNote = new BrightIdeasSoftware.OLVColumn();
                colNote.Text = "LocId";
                colNote.AspectName = "strLocId";
                colNote.Width = 100;
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

        public void MostraListaAttivita()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);

                flvElencoStep.HeaderUsesThemes = false;
                flvElencoStep.HeaderFormatStyle = _stile;

                flvElencoStep.AllColumns.Clear();

                flvElencoStep.View = View.Details;
                flvElencoStep.ShowGroups = false;
                flvElencoStep.GridLines = true;
                flvElencoStep.CheckBoxes = true;
                flvElencoStep.CheckedAspectName = "Attivo";
                flvElencoStep.UseAlternatingBackColors = true;
                flvElencoStep.FullRowSelect = true;



                BrightIdeasSoftware.OLVColumn colStep = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Step",
                    AspectName = "Step",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvElencoStep.AllColumns.Add(colStep);

                BrightIdeasSoftware.OLVColumn colAct = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Tipo",
                    AspectName = "strTipoAttivita",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvElencoStep.AllColumns.Add(colAct);

                BrightIdeasSoftware.OLVColumn colDescr = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Descr",
                    AspectName = "Descrizione",
                    Width = 200,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvElencoStep.AllColumns.Add(colDescr);


                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "",
                    AspectName = "",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                    FillsFreeSpace = true,
                };
                flvElencoStep.AllColumns.Add(colRowFiller);

                flvElencoStep.RebuildColumns();

                this.flvElencoStep.SetObjects(ListaStepAttivi);
                flvElencoStep.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("Mostra Lista: " + Ex.Message);
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
            //pbRicerca.Visible = !pbRicerca.Visible;
            //pbRicerca.Show();
            try
            {
                if (flvwListaDevices.SelectedObject != null)
                {
                    ApriDispositivoSelezionatoUSB();
                }
            }
            catch
            { 
            
            }
        }
        private void flvwListaDevices_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;
                object _tempObj = _lista.SelectedObject;
                if(_tempObj != null)
                {
                    Type TipoLink = _tempObj.GetType();
                    ApriDispositivoSelezionatoUSB();
                }  

            }
            catch (Exception Ex)
            {
                Log.Error("flvwListaDevices_MouseDoubleClick: " + Ex.Message);
            }
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
                                CurrentSerialNumber = _tempCanale.SerialNumber;
                                return;

                            case "LADE LIGHT":
                                //_varGlobali.usbLadeLightSerNum = _tempCanale.SerialNumber;
                                //BR.Mode = BaudRate.BRType.BR_9600;
                                //BR.Speed = 0;
                                //_varGlobali.ActiveBaudRate = BR;
                                //_varGlobali.CanaleLadeLight = parametriSistema.CanaleDispositivo.USB;
                                //ApriLadeLight();
                                return;

                            case "SPY-BATT":
                            case "FT201X USB I2C":
                            case "SEQ-DESO":
                            case "REGENERATOR":
                            case "BATTERY REGENERATOR":
                            case "DESOLFATATORE":
                                return;

                            case "ID-BATT PROGRAMMER":
                                //_varGlobali.usbSpyBattSerNum = _tempCanale.SerialNumber;
                                //_varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                                //ApriIdProgrammer(_tempCanale.SerialNumber);
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
                bool EsitoApertura;

                Log.Debug("Apri Canale SuperCharger");
                this.Cursor = Cursors.WaitCursor;


                //Temporaneo: data la coincidenza del canale, chiudo eventuali finestre supercharger aperte


                // se la porta seriale non è aperta , la apro
                // -- rev 14/09  se già aperta chiudo e riapro             
                if (_varGlobali.statoCanaleLadeLight())
                {
                    Log.Debug("USB LadeLight aperto - lo richiudo");
                    _varGlobali.chiudiCanaleLadeLight();
                }

                _varGlobali.TipoCharger = CaricaBatteria.TipoCaricaBatteria.SuperCharger;
                VuotaConfigCorrente();
                esitoCanaleApparato = _varGlobali.apriLadeLight();
                bool EsitoInit;
                if (esitoCanaleApparato)
                {
                    EsitoApertura = attivaCaricabatterie(ref _varGlobali, esitoCanaleApparato);
                    if(EsitoApertura)
                    {
                        EsitoInit = CaricaStatoDispositivo();
                        grbInitDatiBase.Enabled = true;
                        grbInizializzazione.Enabled = true;
                        cmbInitTipoApparato_SelectedValueChanged(null,null); 
                        //Ora carico i valori di inizializzazione di base:




                    }
                    else
                    {
                        grbInitDatiBase.Enabled = false;
                        grbInizializzazione.Enabled = false;
                    }


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


        public bool attivaCaricabatterie(ref parametriSistema _par, bool CaricaDati)
        {
            bool _esito;
            try
            {

                ResizeRedraw = true;
                _parametri = _par;
                _msg = new SerialMessage();
                _cb = new CaricaBatteria(ref _parametri, _logiche?.dbDati?.connessione, CaricaBatteria.TipoCaricaBatteria.SuperCharger);
                //_cb.OnBaudRateChange += OnBRChange;

                _esito = _cb.apriPorta();
                if (!_esito)
                {
                    MessageBox.Show(_parametri.lastError, "Connessione Fallita", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //this.Hide();  //Close();
                    return _esito;
                }

                // Ora apro esplicitamente il canale. se fallisco esco direttamente
                // _cb.apparatoPresente = true;
                //_apparatoPresente = true;
                _esito = _cb.StartComunicazione();
                if (!_esito)
                {
                    MessageBox.Show(_parametri.lastError, "Apertura Comunicazione fallita", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //this.Hide();  //Close();
                    return _esito;
                }

                _esito = _cb.VerificaPresenza();

                return _esito;

            }
            catch
            {
                return false;
            }

        }

        public bool CaricaStatoDispositivo()
        {
            bool _esito;
            try
            {
                _esito = _cb.VerificaPresenza();
                if (_esito)
                {
                    if (_esito)
                    {
                        //_esito = _cb.CaricaApparatoA0();
                        //_tempParametri = _cb.ParametriApparato;

                        return LeggiInizializzazione();
                    }

                }
                return false;

            }
            catch
            {
                return false;
            }

        }

        public bool VuotaDatiCorrenti()
        {
            txtInitManufactured.Text = "";
            txtInitProductId.Text = "";
            txtInitDataInizializ.Text = "";
            txtInitAnnoMatricola.Text = "";
            txtInitNumeroMatricola.Text = "";
            txtInitSerialeApparato.Text = "";
            txtInitIDApparato.Text = "";
            txtInitTipoApparato.Text = "";
            txtInitNumSerDISP.Text = "";
            txtInitRevHwDISP.Text = "";
            txtInitRevFwDISP.Text = "";

            txtInitVMin.Text = "";
            txtInitVMax.Text = "";
            txtInitAMax.Text = "";

            chkInitPresenzaRabb.Checked = false;

            txtInitBrdNumModuli.Text = "";
            txtInitBrdVNomModulo.Text = "";
            txtInitBrdANomModulo.Text = "";

            txtInitBrdVMinModulo.Text = "";
            txtInitBrdVMaxModulo.Text = "";
            return true;
        }

        public bool VuotaConfigCorrente()
        {
            //txtInitManufactured.Text = "";
            //txtInitProductId.Text = "";
            txtConfDataInizializ.Text = ""; // DateTime.Now.ToString("dd/mm/yyyy");
            txtConfAnnoMatricola.Text = ""; // DateTime.Now.ToString("yy");
            txtConfNumeroMatricola.Text = "";
            txtConfSerialeApparato.Text = "";
            txtConfNumSerDISP.Text = "";

            txtConfRevFwDISP.Text = "";
            txtConfRevHwDISP.Text = "";

            txtConfVMin.Text = "";
            txtConfVMax.Text = "";
            txtConfAMax.Text = "";

            //chkInitPresenzaRabb.Checked = false;

            txtConfBrdNumModuli.Text = "";
            txtConfBrdVNomModulo.Text = "";
            txtConfBrdANomModulo.Text = "";

            txtConfBrdVMinModulo.Text = "";
            txtConfBrdVMaxModulo.Text = "";
            chkConfPresenzaRabb.Checked = false;    
            return true;
        }

        /// <summary>
        /// Carico i dati di configurazione della scheda collegata
        /// </summary>
        /// <returns></returns>
        public bool LeggiInizializzazione()
        {
            try
            {
                bool _esito;

                VuotaDatiCorrenti();
                _esito = _cb.LeggiParametriApparato();

                if (_esito)
                {
                    txtInitManufactured.Text = _cb.ParametriApparato.llParApp.ProduttoreApparato;
                    txtInitProductId.Text = _cb.ParametriApparato.llParApp.NomeApparato;

                    if (_cb.ParametriApparato.llParApp.IdApparato != "????????" && _cb.ParametriApparato.llParApp.IdApparato != "")
                    {
                        txtInitDataInizializ.Text = FunzioniMR.StringaDataTS(_cb.ParametriApparato.llParApp.DataSetupApparato);
                        txtInitAnnoMatricola.Text = _cb.ParametriApparato.llParApp.AnnoCodice.ToString("00");
                        txtInitNumeroMatricola.Text = _cb.ParametriApparato.llParApp.ProgressivoCodice.ToString("000000").ToUpper();
                        txtInitSerialeApparato.Text = _cb.ParametriApparato.llParApp.SerialeApparato.ToString("x6").ToUpper();
                        txtInitIDApparato.Text = _cb.ParametriApparato.llParApp.IdApparato;

                        //cmbInitTipoApparato.SelectedValue = _cb.ParametriApparato.llParApp.TipoApparato;

                        //ModelloInitDisplay TempT = cmbInitTipoApparato.Items.OfType<ModelloInitDisplay>().First(f => f.TestataInit.TipoApparato == _cb.ParametriApparato.llParApp.TipoApparato);
                        txtInitTipoApparato.Text =  "0x" + _cb.ParametriApparato.llParApp.TipoApparato.ToString("x2");

                        //txtInitTipoApparato.text = cmbInitTipoApparato.Items.OfType<_llModelloCb>().First(f => f.IdModelloLL == _cb.ParametriApparato.llParApp.TipoApparato);

                        txtInitVMin.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.VMin);
                        txtInitVMax.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.VMax);
                        txtInitAMax.Text = FunzioniMR.StringaCorrenteLL(_cb.ParametriApparato.llParApp.Amax);

                    }
                    else
                    {
                        txtInitIDApparato.Text = "NON INIZIALIZZATO";
                        return _esito;
                    }

                    if (_cb.ParametriApparato.llParApp.SerialeDISP != null)
                    {
                        txtInitNumSerDISP.Text = FunzioniComuni.HexdumpArray(_cb.ParametriApparato.llParApp.SerialeDISP);
                        txtInitRevHwDISP.Text = _cb.ParametriApparato.llParApp.HardwareDisp;
                        txtInitRevFwDISP.Text = _cb.ParametriApparato.llParApp.SoftwareDISP;
                    }

                    txtInitRevFwDISP.Text = _cb.ParametriApparato.llParApp.SoftwareDISP;

                    if (_cb.ParametriApparato.llParApp.PresenzaRabboccatore == 0xF0)
                    {
                        chkInitPresenzaRabb.Checked = true;
                    }

                    //txtInitMaxBrevi.Text = _cb.ParametriApparato.llParApp.MaxRecordBrevi.ToString();
                    //txtInitMaxLunghi.Text = _cb.ParametriApparato.llParApp.MaxRecordCarica.ToString();
                    //txtInitMaxProg.Text = _cb.ParametriApparato.llParApp.MaxProgrammazioni.ToString();
                    //txtInitModelloMemoria.Text = _cb.ParametriApparato.llParApp.ModelloMemoria.ToString();

                    // Tensioni e corrente max apparato

                    if (_cb.ParametriApparato.llParApp.NumeroModuli != 0xFF)
                    {
                        txtInitBrdNumModuli.Text = _cb.ParametriApparato.llParApp.NumeroModuli.ToString();
                        txtInitBrdVNomModulo.Text = FunzioniMR.StringaTensione( _cb.ParametriApparato.llParApp.ModVNom );// / 100;
                        txtInitBrdANomModulo.Text = FunzioniMR.StringaCorrente((short)_cb.ParametriApparato.llParApp.ModANom);

                        txtInitBrdVMinModulo.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.ModVMin);
                        txtInitBrdVMaxModulo.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.ModVMax);
                    }
                }

                return _esito;

            }
            catch (Exception Ex)
            {
                Log.Error("SalvaInizializzazione: " + Ex.Message);
                //_inLettura = false;
                return false;

            }
        }


        private void frmSetupLLT_Load(object sender, EventArgs e)
        {
            AttivaEventi();
        }

        private void grbInizializzazione_Enter(object sender, EventArgs e)
        {

        }

        private void cmbInitTipoApparato_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                VuotaConfigCorrente();
                if (cmbInitTipoApparato.SelectedItem != null)
                {
                    ModelloInitDisplay ModelloAttivo = (ModelloInitDisplay)cmbInitTipoApparato.SelectedItem;
                    ListaStepAttivi = ModelloAttivo.ListaStepInit;
                    TestataAttiva = ModelloAttivo.TestataInit;
                    MostraListaAttivita();
                    MostraConfigSelezionata(ModelloAttivo);
                    this.ActiveControl = txtConfNumeroMatricola;
                }

            }
            catch
            {

            }
        }

        public bool MostraConfigSelezionata(ModelloInitDisplay ModelloAttivo)
        {
            if (ModelloAttivo == null)
            {
                return false;
            }

            txtConfDataInizializ.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtConfAnnoMatricola.Text = DateTime.Now.ToString("yy");
            txtConfNumeroMatricola.Text = "";
            txtConfSerialeApparato.Text = "";
            txtConfNumSerDISP.Text = "";

            txtConfRevFwDISP.Text = _cb?.ParametriApparato.llParApp.SoftwareDISP;
            txtConfRevHwDISP.Text = ModelloAttivo.TestataInit.HardwareDisp;

            txtConfVMin.Text = FunzioniMR.StringaTensione(ModelloAttivo.TestataInit.VMin);
            txtConfVMax.Text = FunzioniMR.StringaTensione(ModelloAttivo.TestataInit.VMax);
            txtConfAMax.Text = FunzioniMR.StringaCorrenteLL(ModelloAttivo.TestataInit.Amax);

            //chkInitPresenzaRabb.Checked = false;
            chkConfPresenzaRabb.Checked = false;

            txtConfBrdNumModuli.Text = ModelloAttivo.TestataInit.NumeroModuli.ToString();
            txtConfBrdVNomModulo.Text = FunzioniMR.StringaTensione(ModelloAttivo.TestataInit.ModVNom);
            txtConfBrdANomModulo.Text = FunzioniMR.StringaCorrente((short)ModelloAttivo.TestataInit.ModANom);

            txtConfBrdVMinModulo.Text = FunzioniMR.StringaTensione(ModelloAttivo.TestataInit.ModVMin);
            txtConfBrdVMaxModulo.Text = FunzioniMR.StringaTensione(ModelloAttivo.TestataInit.ModVMax);

            return true;
        }


        public bool MostraParametriInizializzazione()
        {
            try
            {
                bool _esito;

                VuotaDatiCorrenti();
                _esito = _cb.LeggiParametriApparato();

                if (_esito)
                {
                    txtInitManufactured.Text = _cb.ParametriApparato.llParApp.ProduttoreApparato;
                    txtInitProductId.Text = _cb.ParametriApparato.llParApp.NomeApparato;

                    if (_cb.ParametriApparato.llParApp.IdApparato != "????????" && _cb.ParametriApparato.llParApp.IdApparato != "")
                    {
                        txtInitDataInizializ.Text = FunzioniMR.StringaDataTS(_cb.ParametriApparato.llParApp.DataSetupApparato);
                        txtInitAnnoMatricola.Text = _cb.ParametriApparato.llParApp.AnnoCodice.ToString("00");
                        txtInitNumeroMatricola.Text = _cb.ParametriApparato.llParApp.ProgressivoCodice.ToString("000000").ToUpper();
                        txtInitSerialeApparato.Text = _cb.ParametriApparato.llParApp.SerialeApparato.ToString("x6").ToUpper();
                        txtInitIDApparato.Text = _cb.ParametriApparato.llParApp.IdApparato;

                        //cmbInitTipoApparato.SelectedValue = _cb.ParametriApparato.llParApp.TipoApparato;
                        txtInitTipoApparato.Text = cmbInitTipoApparato.Items.OfType<ModelloInitDisplay>().First(f => f.TestataInit.TipoApparato == _cb.ParametriApparato.llParApp.TipoApparato).Descrizione;

                        txtInitVMin.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.VMin);
                        txtInitVMax.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.VMax);
                        txtInitAMax.Text = FunzioniMR.StringaCorrenteLL(_cb.ParametriApparato.llParApp.Amax);

                    }
                    else
                    {
                        txtInitIDApparato.Text = "NON INIZIALIZZATO";
                        return _esito;
                    }

                    if (_cb.ParametriApparato.llParApp.SerialeDISP != null)
                    {
                        txtInitNumSerDISP.Text = FunzioniComuni.HexdumpArray(_cb.ParametriApparato.llParApp.SerialeDISP);
                        txtInitRevHwDISP.Text = _cb.ParametriApparato.llParApp.HardwareDisp;
                        txtInitRevFwDISP.Text = _cb.ParametriApparato.llParApp.SoftwareDISP;
                    }

                    txtInitRevFwDISP.Text = _cb.ParametriApparato.llParApp.SoftwareDISP;

                    if (_cb.ParametriApparato.llParApp.PresenzaRabboccatore == 0xF0)
                    {
                        chkInitPresenzaRabb.Checked = true;
                    }

                    // Tensioni e corrente max apparato

                    if (_cb.ParametriApparato.llParApp.NumeroModuli != 0xFF)
                    {
                        txtInitBrdNumModuli.Text = _cb.ParametriApparato.llParApp.NumeroModuli.ToString();
                        txtInitBrdVNomModulo.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.ModVNom);// / 100;
                        txtInitBrdANomModulo.Text = FunzioniMR.StringaCorrente((short)_cb.ParametriApparato.llParApp.ModANom);

                        txtInitBrdVMinModulo.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.ModVMin);
                        txtInitBrdVMaxModulo.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.ModVMax);
                    }
                }

                return _esito;

            }
            catch (Exception Ex)
            {
                Log.Error("SalvaInizializzazione: " + Ex.Message);
                //_inLettura = false;
                return false;

            }
        }

        public bool SalvaInizializzazione()
        {
            try
            {
                if (_cb.ParametriApparato == null)
                {
                    // _cb.ParametriApparato = new llParametriApparato();
                    // 11/02/2019 - Non posso partire e riprogrammare se non sono presenti i dati originali di cui alcuni sevono essere rigidamente conservati.
                    Log.Debug("Dati originali mancanti; inizializzazione non possiblile.");
                    return false;
                }

                // 11/02/2019 - mantengo id e nome preesistenti

                uint TmpInt;
                bool _esito;
                byte[] tempVal;

                TmpInt = 0xFFFFFFFF;
                if (uint.TryParse(txtConfSerialeApparato.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out TmpInt))
                {
                    _cb.ParametriApparato.llParApp.SerialeApparato = TmpInt;
                }

                // Tipo Apparato

                _cb.ParametriApparato.llParApp.TipoApparato = (byte)TestataAttiva.IdModelloCB;
                _cb.ParametriApparato.llParApp.FamigliaApparato = (byte)TestataAttiva.FamigliaApparato;

                // Data
                byte[] dataInit = FunzioniMR.toArrayDataTS(txtConfDataInizializ.Text);
                uint DataUint = dataInit[0];
                DataUint = (DataUint << 8) + dataInit[1];
                DataUint = (DataUint << 8) + dataInit[2];
                _cb.ParametriApparato.llParApp.DataSetupApparato = DataUint;


                // Seriale scheda DISP
                if (txtConfNumSerDISP.Text.Trim() != "")
                {
                    tempVal = FunzioniComuni.HexStringToArray(txtConfNumSerDISP.Text, 8);
                }
                else
                {
                    tempVal = new byte[8] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                }
                _cb.ParametriApparato.llParApp.SerialeDISP = tempVal;

                // Rev SW DISP - MANTENGO IL DEFAULT
                // _cb.ParametriApparato.llParApp.SoftwareDISP = txtInitRevFwDISP.Text;

                // Rev HW DISP
                _cb.ParametriApparato.llParApp.HardwareDisp = txtConfRevHwDISP.Text;

                _cb.ParametriApparato.llParApp.IdApparato = txtConfIDApparato.Text;

                // Presenza modulo rabboccatore
                if (chkConfPresenzaRabb.Checked)
                {
                    _cb.ParametriApparato.llParApp.PresenzaRabboccatore = 0xF0;
                }
                else
                {
                    _cb.ParametriApparato.llParApp.PresenzaRabboccatore = 0x0F;
                }


                // Tensioni e corrente max apparato
                _cb.ParametriApparato.llParApp.VMin = FunzioniMR.ConvertiUshort(txtConfVMin.Text, 100, 0);
                _cb.ParametriApparato.llParApp.VMax = FunzioniMR.ConvertiUshort(txtConfVMax.Text, 100, 0);

                _cb.ParametriApparato.llParApp.Amax = FunzioniMR.ConvertiUshort(txtConfAMax.Text, 10, 0);


                _cb.ParametriApparato.llParApp.NumeroModuli = FunzioniMR.ConvertiByte(txtConfBrdNumModuli.Text);
                _cb.ParametriApparato.llParApp.ModVNom = FunzioniMR.ConvertiUshort(txtConfBrdVNomModulo.Text, 100, 0);
                _cb.ParametriApparato.llParApp.ModANom = FunzioniMR.ConvertiUshort(txtConfBrdANomModulo.Text, 10, 0);
                _cb.ParametriApparato.llParApp.ModOpzioni = 0; // FunzioniMR.ConvertiUshort(txtConfBrdOpzioniModulo.Text, 1, 0);
                _cb.ParametriApparato.llParApp.ModVMin = FunzioniMR.ConvertiUshort(txtConfBrdVMinModulo.Text, 100, 0);
                _cb.ParametriApparato.llParApp.ModVMax = FunzioniMR.ConvertiUshort(txtConfBrdVMaxModulo.Text, 100, 0);

                _esito = _cb.ScriviParametriApparato(true);

                return _esito;

            }
            catch (Exception Ex)
            {
                Log.Error("SalvaInizializzazione: " + Ex.Message);
                return false;

            }
        }


        public uint CalcolaMatricola()
        {
            try
            {

                byte _annoMatr = 18;

                uint matricola = 0;

                if (byte.TryParse(txtConfAnnoMatricola.Text, out _annoMatr))
                {
                    _annoMatr = (byte)(_annoMatr & 0x3F);
                    _annoMatr = (byte)(_annoMatr << 2);

                }

                matricola = (uint)(_annoMatr << 16);

                uint _numMatr = 0;

                if (uint.TryParse(txtConfNumeroMatricola.Text, out _numMatr))
                {
                    _numMatr = (uint)(_numMatr & 0x0003FFFF);

                }

                matricola += _numMatr;



                txtConfSerialeApparato.Text = matricola.ToString("X6");
                string _tempMatricola = "SC" + txtConfSerialeApparato.Text;
                byte[] _arrayMatr = FunzioniComuni.StringToArray(_tempMatricola, 8, 0);

                txtConfIDApparato.Text = _tempMatricola;
                txtConfNumSerDISP.Text = txtConfNumeroMatricola.Text; // txtConfSerialeApparato.Text;

                return matricola;

            }

            catch (Exception Ex)
            {
                Log.Error("CalcolaMatricola: " + Ex.Message);
                return 0;

            }

        }


        private void pgbAvanzamento_Click(object sender, EventArgs e)
        {

        }

        private void txtConfAnnoMatricola_Leave(object sender, EventArgs e)
        {
            CalcolaMatricola();
            btnEseguiInit.Enabled = VerificaDatisetup();
        }

        private void txtConfNumeroMatricola_Leave(object sender, EventArgs e)
        {
            CalcolaMatricola();
            btnEseguiInit.Enabled = VerificaDatisetup();

        }

        public bool VerificaDatisetup()
        {
            try
            {
                if(_cb?.ParametriApparato == null || cmbInitTipoApparato.SelectedItem == null || txtConfIDApparato.Text == "")
                {
                    return false;
                }

                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }

        private void btnEseguiInit_Click(object sender, EventArgs e)
        {
            try
            {
                bool Esito;
                Esito = InizializzaScheda();
                if ( Esito)
                {
                    MessageBox.Show("Inizializzazione completata", "Inizializzazione", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Inizializzazione fallita", "Inizializzazione", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public bool InizializzaScheda()
        {
            try
            {
                bool _esito;
                int TotStep = ListaStepAttivi.Count;
                int NumStep = 0;    
                if (_cb.ParametriApparato == null || cmbInitTipoApparato.SelectedItem == null || txtConfIDApparato.Text == "")
                {
                    return false;
                }

                pgbAvanzamento.Minimum = 0;
                pgbAvanzamento.Maximum = TotStep;
                pgbAvanzamento.Value = NumStep;
                pgbAvanzamento.Visible = true;
                lblAvanzamento.Text = "";
                lblAvanzamento.Visible=true;
                // I parametri sono a posto Comincio ad eseguire gli step

                foreach (SqStepInit CurrStep in ListaStepAttivi)

                {
                    if (CurrStep.Attivo)
                    {
                        lblAvanzamento.Text = CurrStep.Descrizione;

                        switch (CurrStep.Attivita)
                        {
                            case SqStepInit.TipoStep.NOOP:
                                {
                                    // Nulla da fare; next!
                                    break;
                                }
                            case SqStepInit.TipoStep.Reboot:
                                {
                                    _esito = _cb.ResetScheda();
                                    if (!_esito)
                                    {
                                        return false;
                                    }
                                    break;
                                }
                            case SqStepInit.TipoStep.Delete4K:
                                {
                                    _esito = _cb.CancellaBlocco4K(CurrStep.Address, true);
                                    if (!_esito)
                                    {
                                        return false;
                                    }
                                    break;
                                }
                            case SqStepInit.TipoStep.ReadInit:
                                {
                                    _esito = CaricaStatoDispositivo();
                                    if (!_esito)
                                    {
                                        return false;
                                    }
                                    break;
                                }
                            case SqStepInit.TipoStep.SetTime:
                                {
                                    _esito = _cb.ScriviOrologio();
                                    if (!_esito)
                                    {
                                        return false;
                                    }
                                    break;
                                }
                            case SqStepInit.TipoStep.WriteInit:
                                {
                                    _esito = SalvaInizializzazione();
                                    if (!_esito)
                                    {
                                        return false;
                                    }
                                    break;
                                }
                            case SqStepInit.TipoStep.AddProfilo:
                                {
                                    byte[] DataMap = FunzioniComuni.HexStringToArray(CurrStep.StrDataArray, (int)CurrStep.DataLenght);
                                    _esito = _cb.ScriviBloccoMemoria(CurrStep.Address, (ushort)CurrStep.DataLenght, DataMap);
                                    if (!_esito)
                                    {
                                        return false;
                                    }
                                    break;
                                }

                        }
                    }
                    else
                    {
                        lblAvanzamento.Text = "Skip " + CurrStep.Descrizione;
                        Task.Delay(1000).Wait();
                    }

                    // Ora avanzo il cursore....
                    NumStep += 1;
                    pgbAvanzamento.Value = NumStep;
                    Application.DoEvents();
                    Task.Delay(500).Wait();
                }



                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
            finally
            {
                pgbAvanzamento.Visible = false;
                lblAvanzamento.Text = "";
                lblAvanzamento.Visible = false;
            }
            
        }

    }
}

