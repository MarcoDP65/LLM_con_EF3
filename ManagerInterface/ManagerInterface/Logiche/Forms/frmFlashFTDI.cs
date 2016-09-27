using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using System.Windows.Forms;
using Utility;

namespace PannelloCharger
{
    public partial class frmFlashFTDI : Form
    {

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        FTDI DeviceUsb = new FTDI();
        FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

        public string SerialeTarget = "";

        public frmFlashFTDI()
        {
            InitializeComponent();
            InizializzaControlli();
        }


        public Boolean cercaPorte()
        {

            string campo;
            string chiave;
            

            UInt32 ftdiDeviceCount = 0;

            lvwListaPorte.Items.Clear();

            // Determine the number of FTDI devices connected to the machine
            ftStatus = DeviceUsb.GetNumberOfDevices(ref ftdiDeviceCount);
            
            // Check status
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                Log.Error("Errore scansione porte USB: " + ftStatus.ToString());
                return false;
            }

            // Allocate storage for device info list
            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];

            // Populate our device list
            ftStatus = DeviceUsb.GetDeviceList(ftdiDeviceList);
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                uint CicloDev = 0;
                for (CicloDev = 0; CicloDev < ftdiDeviceCount; CicloDev++)
                {

                    string[] _device = new string[4];
                    ListViewItem itm;
                    _device[0] = ftdiDeviceList[CicloDev].SerialNumber;
                    _device[1] = ftdiDeviceList[CicloDev].Description;
                    _device[2] = ftdiDeviceList[CicloDev].Type.ToString();
                    itm = new ListViewItem(_device);

                    lvwListaPorte.Items.Add(itm);
                }

            }
            else
            {
                Log.Error("Errore scansione porte USB: " + ftStatus.ToString());
                return false;
            }
            Application.DoEvents();
            return true;
        }

        private void InizializzaControlli()
        {
            try
            {

                lvwListaPorte.Columns.Clear();
                //creo le colonne
                lvwListaPorte.Columns.Add("Serial Id", 100, HorizontalAlignment.Right);
                lvwListaPorte.Columns.Add("Descr.", 140, HorizontalAlignment.Left);
                lvwListaPorte.Columns.Add("Type", 140, HorizontalAlignment.Left);
                lvwListaPorte.View = View.Details;
                lvwListaPorte.FullRowSelect = true;

                //Id Seriale


                txtFtdiSerialId.Text = SerialeTarget;

            }
            catch
            {

            }
        }


        private void MostraDevice()
        {
            try
            {

                txtEsito.Text = "";
                SerialeTarget = txtFtdiSerialId.Text;

                if (SerialeTarget != "")
                {
                    // Tento di connettermi tramite Serial Number

                    ftStatus = DeviceUsb.OpenBySerialNumber(SerialeTarget);
                    if (ftStatus != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        txtEsito.Text = "Failed to open device (error " + ftStatus.ToString() + ")";
                        return;
                    }
                    else
                    {
                        
                        txtEsito.Text = "Canale Ok";

                        FTDI.FT_XSERIES_EEPROM_STRUCTURE _epromCorrente = new FTDI.FT_XSERIES_EEPROM_STRUCTURE();

                        ftStatus = DeviceUsb.ReadXSeriesEEPROM(_epromCorrente);
                        if (ftStatus != FTDI.FT_STATUS.FT_OK)
                        {
                            txtEsito.Text += "\r\nFailed To read e2prom";
                        }
                        else
                        {
                            txtEsito.Text = " --- " + SerialeTarget +" --- ";
                            txtEsito.Text += "\r\nProductID    : >" + _epromCorrente.ProductID.ToString("X4") + "<";
                            txtEsito.Text += "\r\nManufacturer : >" + _epromCorrente.Manufacturer + "<";
                            txtEsito.Text += "\r\nDescription  : >" + _epromCorrente.Description + "<";
                            txtEsito.Text += "\r\nIsVCP        : >" + _epromCorrente.IsVCP.ToString("X2") + "<";
                            txtEsito.Text += "\r\n--------------";
                            txtEsito.Text += "\r\nI2CDeviceId  : >" + _epromCorrente.I2CDeviceId.ToString("X4") + "<";
                            txtEsito.Text += "\r\n--------------";
                            txtEsito.Text += "\r\nCbus0        : >" + _epromCorrente.Cbus0.ToString("X2") + "<";
                            txtEsito.Text += "\r\nCbus1        : >" + _epromCorrente.Cbus1.ToString("X2") + "<";
                            txtEsito.Text += "\r\nCbus2        : >" + _epromCorrente.Cbus2.ToString("X2") + "<";
                            txtEsito.Text += "\r\nCbus3        : >" + _epromCorrente.Cbus3.ToString("X2") + "<";
                            txtEsito.Text += "\r\nCbus4        : >" + _epromCorrente.Cbus4.ToString("X2") + "<";
                            txtEsito.Text += "\r\nCbus5        : >" + _epromCorrente.Cbus5.ToString("X2") + "<";
                            txtEsito.Text += "\r\nCbus6        : >" + _epromCorrente.Cbus6.ToString("X2") + "<";
                            DeviceUsb.Close();

                        }


                    }



                }

            }
            catch
            {

            }
        }

        private void ImpostaDevice()
        {
            try
            {

                txtEsito.Text = "";
                SerialeTarget = txtFtdiSerialId.Text;

                if (SerialeTarget != "")
                {

                    DeviceUsb.ResetDevice();

                    // Tento di connettermi tramite Serial Number

                    ftStatus = DeviceUsb.OpenBySerialNumber(SerialeTarget);
                    if (ftStatus != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        txtEsito.Text = "Failed to open device (error " + ftStatus.ToString() + ")";
                        return;
                    }
                    else
                    {

                        txtEsito.Text = "Canale Ok";

                        FTDI.FT_XSERIES_EEPROM_STRUCTURE _epromCorrente = new FTDI.FT_XSERIES_EEPROM_STRUCTURE();

                        ftStatus = DeviceUsb.ReadXSeriesEEPROM(_epromCorrente);
                        if (ftStatus != FTDI.FT_STATUS.FT_OK)
                        {
                            txtEsito.Text += "\r\nFailed To read e2prom";
                        }
                        else
                        {
                            if (optFTDIGenerico.Checked)
                            {
                                // FTDI Base
                                //-----------------------------------------------------
                                _epromCorrente.ProductID = 0x6015;
                                _epromCorrente.Manufacturer = FunzioniMR.CompletaAZero("FTDI", 32);
                                _epromCorrente.Description = FunzioniMR.CompletaAZero("FT201X USB I2C", 64);
                                _epromCorrente.IsVCP = 0x00;
                                //-----------------------------------------------------
                                _epromCorrente.I2CDeviceId = 0x123456;
                                //-----------------------------------------------------
                                _epromCorrente.Cbus0 = 0x05;
                                _epromCorrente.Cbus1 = 0x00;
                                _epromCorrente.Cbus2 = 0x00;
                                _epromCorrente.Cbus3 = 0x04;
                                _epromCorrente.Cbus4 = 0x11;
                                _epromCorrente.Cbus5 = 0x15;
                                _epromCorrente.Cbus6 = 0x00;
                                //-----------------------------------------------------
                            }

                            if (optFTDILadeLight.Checked)
                            {
                                // LADE LIGHT
                                //-----------------------------------------------------
                                _epromCorrente.ProductID = 0x7A70;
                                _epromCorrente.Manufacturer = FunzioniMR.CompletaAZero("BATTERY CHARGER INDUSTRY", 32);
                                _epromCorrente.Description = FunzioniMR.CompletaAZero("LADE LIGHT", 64);
                                _epromCorrente.IsVCP = 0x01;
                                //-----------------------------------------------------
                                _epromCorrente.I2CDeviceId = 0x123456;
                                //-----------------------------------------------------
                                _epromCorrente.Cbus0 = 0x05;
                                _epromCorrente.Cbus1 = 0x0F;
                                _epromCorrente.Cbus2 = 0x10;
                                _epromCorrente.Cbus3 = 0x04;
                                _epromCorrente.Cbus4 = 0x11;
                                _epromCorrente.Cbus5 = 0x15;
                                _epromCorrente.Cbus6 = 0x00;
                                //-----------------------------------------------------
                            }

                            if (optFTDISpybatt.Checked)
                            {
                                // SPY-BATT
                                //-----------------------------------------------------
                                _epromCorrente.ProductID = 0x7A71;
                                _epromCorrente.Manufacturer = FunzioniMR.CompletaAZero("BATTERY CHARGER INDUSTRY", 32);
                                _epromCorrente.Description = FunzioniMR.CompletaAZero("SPY-BATT", 64);
                                _epromCorrente.IsVCP = 0x01;
                                //-----------------------------------------------------
                                _epromCorrente.I2CDeviceId = 0x123456;
                                //-----------------------------------------------------
                                _epromCorrente.Cbus0 = 0x00;
                                _epromCorrente.Cbus1 = 0x00;
                                _epromCorrente.Cbus2 = 0x00;
                                _epromCorrente.Cbus3 = 0x00;
                                _epromCorrente.Cbus4 = 0x00;
                                _epromCorrente.Cbus5 = 0x0F;
                                _epromCorrente.Cbus6 = 0x00;
                                //-----------------------------------------------------
                            }


                            if (optFTDISBFinto.Checked)
                            {
                                // SPY-BATT
                                //-----------------------------------------------------
                                _epromCorrente.ProductID = 0x6015;
                                _epromCorrente.Manufacturer = FunzioniMR.CompletaAZero("MORI RADDRIZZATORI", 32);
                                _epromCorrente.Description = FunzioniMR.CompletaAZero("SEQ-DESO", 64);
                                _epromCorrente.IsVCP = 0x00;
                                //-----------------------------------------------------
                                _epromCorrente.I2CDeviceId = 0x123456;
                                //-----------------------------------------------------
                                _epromCorrente.Cbus0 = 0x00;
                                _epromCorrente.Cbus1 = 0x00;
                                _epromCorrente.Cbus2 = 0x00;
                                _epromCorrente.Cbus3 = 0x00;
                                _epromCorrente.Cbus4 = 0x00;
                                _epromCorrente.Cbus5 = 0x0F;
                                _epromCorrente.Cbus6 = 0x00;
                                //-----------------------------------------------------
                            }


                            if (optFTDIDesolf.Checked)
                            {
                                // DESOLFATATORE
                                //-----------------------------------------------------
                                _epromCorrente.ProductID = 0x7A77;
                                _epromCorrente.Manufacturer = FunzioniMR.CompletaAZero("MORI RADDRIZZATORI", 32);
                                _epromCorrente.Description = FunzioniMR.CompletaAZero("DESOLFATATORE", 64);
                                _epromCorrente.IsVCP = 0x01;
                                //-----------------------------------------------------
                                _epromCorrente.I2CDeviceId = 0x134567;
                                //-----------------------------------------------------
                                _epromCorrente.Cbus0 = 0x00;
                                _epromCorrente.Cbus1 = 0x00;
                                _epromCorrente.Cbus2 = 0x00;
                                _epromCorrente.Cbus3 = 0x00;
                                _epromCorrente.Cbus4 = 0x00;
                                _epromCorrente.Cbus5 = 0x04;
                                _epromCorrente.Cbus6 = 0x00;
                                //-----------------------------------------------------
                            }




                            // Impostata la classe aggiorno la EEPROM
                            ftStatus = DeviceUsb.WriteXSeriesEEPROM(_epromCorrente);

                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                MessageBox.Show("Aggiornamento non riuscito\n(error " + ftStatus.ToString() + ")", "Aggiornamento FTDI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtEsito.Text = "Failed to write on the device\n(error " + ftStatus.ToString() + ")";

                            }
                            else
                            {

                                MessageBox.Show("EEPROM FTDI Aggiornata\nScollegare e ricollegare il cavo USB", "Scrittura Memoria", MessageBoxButtons.OK);
                            }
                            DeviceUsb.Close();
                        }

                    }



                }

            }
            catch
            {

            }
        }

        
        private void ResetDevice()
        {
            try
            {

                txtEsito.Text = "";
                SerialeTarget = txtFtdiSerialId.Text;

                if (SerialeTarget != "")
                {

                    // Tento di connettermi tramite Serial Number

                    ftStatus = DeviceUsb.OpenBySerialNumber(SerialeTarget);
                    if (ftStatus != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        txtEsito.Text = "Failed to open device (error " + ftStatus.ToString() + ")";
                        return;
                    }
                    else
                    {
                        ftStatus = DeviceUsb.ResetDevice();

                        if (ftStatus != FTDI.FT_STATUS.FT_OK)
                        {
                            MessageBox.Show("Reset non riuscito\n(error " + ftStatus.ToString() + ")", "Reset FTDI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtEsito.Text = "Failed to write on the device\n(error " + ftStatus.ToString() + ")";

                        }
                        else
                        {

                            MessageBox.Show("Reset FTDI completato", "Scrittura Memoria", MessageBoxButtons.OK);
                        }
                        DeviceUsb.Close();
                    }



                }

            }
            catch
            {

            }
        }
        


        private void btnUsbReload_Click(object sender, EventArgs e)
        {
            bool _esito;

            _esito = cercaPorte();
        }

        private void lvwListaPorte_ItemActivate(object sender, EventArgs e)
        {

        }

        private void lvwListaPorte_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView _list = sender as ListView;

            if (_list.SelectedItems.Count > 0)
            {
                txtFtdiSerialId.Text = _list.SelectedItems[0].Text;
            }

        }

        private void btnApplicaTemplate_Click(object sender, EventArgs e)
        {
            ImpostaDevice();
            MostraDevice();
        }

        private void btnMostraTemplate_Click(object sender, EventArgs e)
        {
            MostraDevice();
        }

        private void btnResetDevice_Click(object sender, EventArgs e)
        {
            ResetDevice();
        }
    }
}
