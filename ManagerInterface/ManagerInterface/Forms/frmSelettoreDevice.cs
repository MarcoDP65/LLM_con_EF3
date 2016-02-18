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

using BrightIdeasSoftware;
using Utility;
using ChargerLogic;


namespace PannelloCharger
{
    public partial class frmSelettoreDevice : Form
    {

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public ScannerUSB Scanner;
        public List<ScannerUSB.UsbDevice> ListaPorte;
        public parametriSistema _varGlobali;
        public LogicheBase logiche;

        public frmSelettoreDevice()
        {
            InitializeComponent();
        }

        public frmSelettoreDevice(ref parametriSistema varGlobali)
        {
            _varGlobali = varGlobali;
            InitializeComponent();
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


        private void frmSelettoreDevice_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// definisco le colonne e collego i dati della lista dispositivi collegati.
        /// </summary>
        public void MostraLista()
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

                /*
                BrightIdeasSoftware.OLVColumn idBatt = new BrightIdeasSoftware.OLVColumn();
                idBatt.Text = "Flags";
                idBatt.AspectName = "Flags";
                idBatt.Width = 30;
                idBatt.HeaderTextAlign = HorizontalAlignment.Center;
                idBatt.TextAlign = HorizontalAlignment.Left;
                flvwListaDevices.AllColumns.Add(idBatt);

                BrightIdeasSoftware.OLVColumn colBatt = new BrightIdeasSoftware.OLVColumn();
                colBatt.Text = "ftHandle";
                colBatt.AspectName = "ftHandle";
                colBatt.Width = 30;
                colBatt.HeaderTextAlign = HorizontalAlignment.Center;
                colBatt.TextAlign = HorizontalAlignment.Left;
                flvwListaDevices.AllColumns.Add(colBatt);
                */

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

        private void btnChiudi_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void ApriDispositivoSelezionato()
        {
            try
            {
                if (flvwListaDevices.SelectedObject != null)
                {


                    ScannerUSB.UsbDevice _tempCanale = (ScannerUSB.UsbDevice)flvwListaDevices.SelectedObject;
                    if (_tempCanale.SerialNumber != null)
                    {

                        switch (_tempCanale.Description)
                        {
                            case "LADE LIGHT":
                                _varGlobali.usbLadeLightSerNum = _tempCanale.SerialNumber;
                                _varGlobali.CanaleLadeLight = parametriSistema.CanaleDispositivo.USB;
                                ApriLadeLight();
                                return;

                            case "SPY-BATT":
                                _varGlobali.usbSpyBattSerNum = _tempCanale.SerialNumber;
                                _varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                                ApriSpyBatt(_tempCanale.SerialNumber);
                                return;

                            case "FT201X USB I2C":
                                _varGlobali.usbSpyBattSerNum = _tempCanale.SerialNumber;
                                _varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                                ApriSpyBatt();
                                return;

                            case "SEQ-DESO":
                                //_tempCanale.Description = "SPY-BATT";
                                _varGlobali.usbSpyBattSerNum = _tempCanale.SerialNumber;
                                _varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                                ApriSpyBatt();
                                return;

                            case "DESOLFATATORE":
                                //_tempCanale.Description = "SPY-BATT";
                                _varGlobali.usbSpyBattSerNum = _tempCanale.SerialNumber;
                                _varGlobali.CanaleSpyBat = parametriSistema.CanaleDispositivo.USB;
                                ApriDesolfatatore();
                                return;
                            default:
                                break;
                        }

                    }

                }
            }
            catch
            { }

        }
        private void ApriLadeLight()
        {
            try
            {
                bool esitoCanaleApparato = false;

                Log.Debug("Apri Canale Lade Light LL");


                // se la porta seriale non è aperta , la apro
                // -- rev 14/09  se già aperta chiudo e riapro             
                if (_varGlobali.statoCanaleLadeLight())
                {
                    Log.Debug("USB LadeLight aperto - lo richiudo");
                    _varGlobali.chiudiCanaleLadeLight();
                }

                esitoCanaleApparato = _varGlobali.apriLadeLight();

                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(frmCaricabatterie))
                    {
                        form.Activate();
                        return;
                    }
                }
                Log.Debug("NUOVO LL");
                //frmSpyBat sbCorrente = new frmSpyBat(ref varGlobali, true, "", logiche, esitoCanaleApparato, true);

                frmCaricabatterie cbCorrente = new frmCaricabatterie(ref _varGlobali, true);
                cbCorrente.Cursor = Cursors.WaitCursor;

                cbCorrente.MdiParent = this.MdiParent;
                cbCorrente.StartPosition = FormStartPosition.CenterParent;
                //cbCorrente.Cursor = Cursors.WaitCursor;
                cbCorrente.Show();

            }
            catch (Exception Ex)
            {
                Log.Error("frmMain.ApriLadeLight: " + Ex.Message);
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
                sbCorrente.MdiParent = this.MdiParent; ;
                sbCorrente.StartPosition = FormStartPosition.CenterParent;

                this.Cursor = Cursors.Default;

                Log.Debug("PRIMA");
                sbCorrente.Show();
                Log.Debug("DOPO");

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
            ApriDispositivoSelezionato();
        }

        private void flvwListaDevices_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;

                if (_lista.SelectedObject != null)
                {
                    ApriDispositivoSelezionato();
                }
            }
            catch (Exception Ex)
            {
                Log.Error("flvwListaDevices_MouseDoubleClick: " + Ex.Message);
            }
        }

        private void btnAggiorna_Click(object sender, EventArgs e)
        {
            Aggiorna();
        }
    }



}
