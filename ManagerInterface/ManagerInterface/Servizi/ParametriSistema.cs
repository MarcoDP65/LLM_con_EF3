using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using System.Drawing.Printing;
using MoriData;

namespace ChargerLogic
{
   public class parametriSistema
    {
        public enum CanaleDispositivo : uint { Seriale = 0x00, USB = 0x01 };

        public SerialPort serialeCorrente ;
        public FTDI usbCorrente;

        public SerialPort serialeLadeLight;        
        public FTDI usbLadeLight;
        public string usbLadeLightSerNum;


        public SerialPort serialeSpyBatt;
        public FTDI usbSpyBatt;
        public string usbSpyBattSerNum;
      


        public CanaleDispositivo CanaleSpyBat;
        public CanaleDispositivo CanaleLadeLight;

        public string portName = "COM9";
        public Int32 baudRate = 115200;
        public Int16 dataBits = 8;
        public StopBits stopBits = StopBits.One;
        public Parity parityBit = Parity.None;
        public Handshake handShake = Handshake.None;

        public uint baudRateUSB = 3000000;

        public string lastError;

        public string currentUser = "FACTORY";
        public string currentPassword = "factory";
        public CultureInfo currentCulture = new CultureInfo("it");
        public string currentCultureValue = "it";
        public bool currentSaveLogin = true;
        public bool firstRun = false;
        public bool FtdiCaricato = false;

        // Gestione Impostazioni Stampante
        public PrinterSettings ImpostazioniStampante { get; set; }

        public static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public List<sbTipoBatteria> TipiBattria;

        public  parametriSistema()
        {
            try
            {
                lastError = "";

                InizializzaFTDI();

                CaricaImpostazioniDefault();

                InizializzaTipiBatteria();

                if (currentCultureValue != "")
                {
                    currentCulture = new CultureInfo(currentCultureValue);
                }
            }
            catch (Exception Ex)
            {
                Log.Error("parametriSistema: " + Ex.Message);
            }

        }

        private void InizializzaFTDI()
        {
            try
            {
                lastError = "";
                string _path = Environment.GetFolderPath(Environment.SpecialFolder.System);
                bool _installed = File.Exists(_path + Path.DirectorySeparatorChar + "FTD2XX.DLL");
                Log.Debug("Presenza FTDIXX.dll: " + _installed.ToString() + " - " + _path);
                FtdiCaricato = _installed;
                if (_installed)
                {
                    serialeCorrente = new SerialPort();
                    serialeLadeLight = new SerialPort();
                    serialeSpyBatt = new SerialPort();

                    usbCorrente = new FTDI();
                    usbLadeLight = new FTDI();
                    usbSpyBatt = new FTDI();
                }

            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaFTDI: " + Ex.Message);
                lastError = Ex.Message;
            }




        }


        public void CaricaImpostazioniDefault()
        {
            try
            {
                currentUser = PannelloCharger.Properties.Settings.Default.utente;
                currentPassword = PannelloCharger.Properties.Settings.Default.password;
                currentCultureValue = PannelloCharger.Properties.Settings.Default.cultureinfo;
                currentSaveLogin = PannelloCharger.Properties.Settings.Default.autoLogin;
                firstRun = PannelloCharger.Properties.Settings.Default.firstRun;
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        public void SalvaImpostazioniDefault()
        {
            try
            {
                PannelloCharger.Properties.Settings.Default.firstRun = firstRun;
                PannelloCharger.Properties.Settings.Default.utente = currentUser;
                PannelloCharger.Properties.Settings.Default.password = currentPassword;
                PannelloCharger.Properties.Settings.Default.cultureinfo = currentCultureValue;
                PannelloCharger.Properties.Settings.Default.autoLogin = currentSaveLogin;
                PannelloCharger.Properties.Settings.Default.Save();
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        public void SalvaParametro(string NomeParametro,string ValoreParametro)
        {
            try
            {

            }
            catch
            {

            }
        }


        public string CaricaParametro(string NomeParametro)
        {
            try
            {
                return "";
            }
            catch
            {
                return "";
            }
        }

        public void impostaCultura(string CodiceLingua)
        {
            try
            {
                currentCultureValue = CodiceLingua;
                currentCulture = new CultureInfo(currentCultureValue);
                PannelloCharger.Properties.Settings.Default.cultureinfo = currentCultureValue;
                PannelloCharger.Properties.Settings.Default.Save();
            }
            catch
            {

            }
        }


        private void InizializzaTipiBatteria()
        {
            try
            {
                TipiBattria = new List<sbTipoBatteria>();

                TipiBattria.Add(new sbTipoBatteria() { BatteryTypeId = 0x00, BatteryType = "N.D.", SortOrder = 0, StandardChargeProfile = 0x00, Obsolete = 0xFF });
                TipiBattria.Add(new sbTipoBatteria() { BatteryTypeId = 0x71, BatteryType = "Pb/Lead", SortOrder = 1, StandardChargeProfile = 0x01, Obsolete = 0x00 });
                TipiBattria.Add(new sbTipoBatteria() { BatteryTypeId = 0x02, BatteryType = "Gel", SortOrder = 2, StandardChargeProfile = 0x0, Obsolete = 0x00 });
                TipiBattria.Add(new sbTipoBatteria() { BatteryTypeId = 0x03, BatteryType = "Lithium", SortOrder = 3, StandardChargeProfile = 0x0, Obsolete = 0x00 });
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaTipiBatteria: " + Ex.Message);
            }
        }

        public sbTipoBatteria TipoBatteria(byte IdCorrente)
        {
            try
            {
                foreach (sbTipoBatteria item in TipiBattria)    
                {
                    if (item.BatteryTypeId == IdCorrente) return item;
                }

                return new sbTipoBatteria() { BatteryTypeId = 0x00, BatteryType = "N.D.", SortOrder = 0, StandardChargeProfile = 0x00, Obsolete = 0xFF };
            }
            catch (Exception Ex)
            {
                Log.Error("TipoBatteria: " + Ex.Message);
                return new sbTipoBatteria() { BatteryTypeId = 0x00, BatteryType = "N.D.", SortOrder = 0, StandardChargeProfile = 0x00, Obsolete = 0xFF };

            }
        }

        #region "Comunicazione SPY-BATT"

        public bool apriSpyBat()
        {
            lastError = "";
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            try
            {
                if (CanaleSpyBat == CanaleDispositivo.USB)
                {
                    if (usbSpyBattSerNum != "")
                    {

                        if (!usbSpyBatt.IsOpen)
                        {

                            // Open device by serial number
                            ftStatus = usbSpyBatt.OpenByIndex(12);

                            ftStatus = usbSpyBatt.OpenBySerialNumber(usbSpyBattSerNum);
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Error("Failed to open device (error " + ftStatus.ToString() + ")");
                                return false;
                            }

                            // Set up device data parameters
                            // Set Baud rate to 9600
                            ftStatus = usbSpyBatt.SetBaudRate(baudRateUSB);
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Error("Failed to set Baud rate (error " + ftStatus.ToString() + ")");
                                return false;
                            }

                            // Set data characteristics - Data bits, Stop bits, Parity
                            ftStatus = usbSpyBatt.SetDataCharacteristics(FTDI.FT_DATA_BITS.FT_BITS_8, FTDI.FT_STOP_BITS.FT_STOP_BITS_1, FTDI.FT_PARITY.FT_PARITY_NONE);
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Error("Failed to set data characteristics (error " + ftStatus.ToString() + ")");
                                return false;
                            }

                            // Set flow control - set RTS/CTS flow control
                            ftStatus = usbSpyBatt.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_RTS_CTS, 0x11, 0x13);
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Error("Failed to set flow control (error " + ftStatus.ToString() + ")");
                                return false;
                            }

                            // Set read timeout to 5 seconds, write timeout to infinite
                            ftStatus = usbSpyBatt.SetTimeouts(5000, 0);
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Error("Failed to set timeouts (error " + ftStatus.ToString() + ")");
                                return false;
                            }

                        }
                        return usbSpyBatt.IsOpen;
                    }
                    return usbSpyBatt.IsOpen;

                }
                else
                {
                    if (!serialeSpyBatt.IsOpen)
                    {
                        serialeSpyBatt.BaudRate = baudRate;
                        serialeSpyBatt.DataBits = dataBits;
                        serialeSpyBatt.Parity = parityBit;
                        serialeSpyBatt.Handshake = handShake;
                        serialeSpyBatt.PortName = portName;

                        serialeSpyBatt.Open();


                    }
                    return serialeSpyBatt.IsOpen;
                }

            }
            catch (Exception Ex)
            {
                lastError = Ex.Message;
                return false;
            }

        }

        public bool scriviMessaggioSpyBatt(byte[] messaggio, int Start, int NumByte )
        {

            try
            {
                if (CanaleSpyBat == CanaleDispositivo.USB)
                {
                    uint bytesScritti = 0;
                    FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
                    ftStatus = usbSpyBatt.Write(messaggio, (uint)NumByte, ref bytesScritti);
                    if (ftStatus != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        Log.Error("Failed to write to device (error " + ftStatus.ToString() + ")");
                        return false;
                    }
                    return true;

                }
                else
                {
                    if (serialeSpyBatt.IsOpen)
                    {
                        serialeSpyBatt.Write(messaggio, Start, NumByte);
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception Ex)
            {
                lastError = Ex.Message;
                return false;
            }       
        }

        public bool statoCanaleSpyBatt()
        {

            try
            {
                if (CanaleSpyBat == CanaleDispositivo.USB)
                {
                    return usbSpyBatt.IsOpen;
                }
                else
                {
                    return serialeSpyBatt.IsOpen;
                }
            }
            catch (Exception Ex)
            {
                lastError = Ex.Message;
                return false;
            }       
        }

        public void chiudiCanaleSpyBatt()
        {

            try
            {
                if (CanaleSpyBat == CanaleDispositivo.USB)
                {
                     usbSpyBatt.Close();
                     return;
                }
                else
                {
                   serialeSpyBatt.Close();
                   return;
                }
            }
            catch (Exception Ex)
            {
                lastError = Ex.Message;
            }
        }
#endregion

#region "Comunicazione LADE Light"

        public bool apriLadeLight()
        {
            lastError = "";
            
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            try
            {
                if (CanaleLadeLight == CanaleDispositivo.USB)
                {
                    Log.Debug("Apro il canale USB: " + usbLadeLightSerNum);

                    if (usbLadeLightSerNum != "")
                    {

                        if (!usbLadeLight.IsOpen)
                        {

                            // Open device by serial number
                            ftStatus = usbLadeLight.OpenBySerialNumber(usbLadeLightSerNum);
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Error("Failed to open device (error " + ftStatus.ToString() + ")");
                                return false;
                            }

                            // Set up device data parameters
                            // Set Baud rate to baudRateUSB
                            ftStatus = usbLadeLight.SetBaudRate(baudRateUSB);
                            Log.Debug("Baudrate USB:" + baudRateUSB.ToString());
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Error("Failed to set Baud rate (error " + ftStatus.ToString() + ")");
                                return false;
                            }

                            // Set data characteristics - Data bits, Stop bits, Parity
                            ftStatus = usbLadeLight.SetDataCharacteristics(FTDI.FT_DATA_BITS.FT_BITS_8, FTDI.FT_STOP_BITS.FT_STOP_BITS_1, FTDI.FT_PARITY.FT_PARITY_NONE);
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Error("Failed to set data characteristics (error " + ftStatus.ToString() + ")");
                                return false;
                            }

                            // Set flow control - set RTS/CTS flow control
                            ftStatus = usbLadeLight.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_RTS_CTS, 0x11, 0x13);
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Error("Failed to set flow control (error " + ftStatus.ToString() + ")");
                                return false;
                            }

                            // Set read timeout to 5 seconds, write timeout to infinite
                            ftStatus = usbLadeLight.SetTimeouts(5000, 0);
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Error("Failed to set timeouts (error " + ftStatus.ToString() + ")");
                                return false;
                            }

                        }
                        Log.Debug("Ladelight USB: IsOpen=" + usbLadeLight.IsOpen.ToString());
                        return usbLadeLight.IsOpen;
                    }
                    return usbLadeLight.IsOpen;

                }
                else
                {
                    if (!serialeLadeLight.IsOpen)
                    {
                        serialeLadeLight.BaudRate = baudRate;
                        serialeLadeLight.DataBits = dataBits;
                        serialeLadeLight.Parity = parityBit;
                        serialeLadeLight.Handshake = handShake;
                        serialeLadeLight.PortName = portName;

                        serialeLadeLight.Open();


                    }
                    return serialeLadeLight.IsOpen;
                }

            }
            catch (Exception Ex)
            {
                lastError = Ex.Message;
                return false;
            }

        }

        public bool scriviMessaggioLadeLight(byte[] messaggio, int Start, int NumByte)
        {

            try
            {
                if (CanaleLadeLight == CanaleDispositivo.USB)
                {
                    uint bytesScritti = 0;
                    FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
                    ftStatus = usbLadeLight.Write(messaggio, (uint)NumByte, ref bytesScritti);
                    Log.Debug("LL Scritura di " + NumByte.ToString() + " Bytes");
                    if (ftStatus != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        Log.Error("Failed to write to device (error " + ftStatus.ToString() + ")");
                        return false;
                    }
                    return true;

                }
                else
                {
                    serialeLadeLight.Write(messaggio, Start, NumByte);
                    return true;
                }
            }
            catch (Exception Ex)
            {
                lastError = Ex.Message;
                return false;
            }
        }

        public bool statoCanaleLadeLight()
        {

            try
            {
                if (CanaleLadeLight == CanaleDispositivo.USB)
                {
                    return usbLadeLight.IsOpen;
                }
                else
                {
                    return serialeLadeLight.IsOpen;
                }
            }
            catch (Exception Ex)
            {
                lastError = Ex.Message;
                return false;
            }
        }

        public void chiudiCanaleLadeLight()
        {

            try
            {
                if (CanaleLadeLight == CanaleDispositivo.USB)
                {
                    usbLadeLight.Close();
                    return;
                }
                else
                {
                    serialeLadeLight.Close();
                    return;
                }
            }
            catch (Exception Ex)
            {
                lastError = Ex.Message;
            }
        }
 #endregion


    }
}
