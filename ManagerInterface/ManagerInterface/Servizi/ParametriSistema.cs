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
using Utility;

namespace ChargerLogic
{
   public class parametriSistema
    {
        public enum CanaleDispositivo : uint { Seriale = 0x00, USB = 0x01 , BTStream = 0x02};

        public Stream streamCorrente;
        public SerialPort serialeCorrente ;
        public FTDI usbCorrente;

        public SerialPort serialeLadeLight;        
        public FTDI usbLadeLight;
        public string usbLadeLightSerNum;

        public Stream streamSpyBatt;
        public SerialPort serialeSpyBatt;
        public FTDI usbSpyBatt;
        public string usbSpyBattSerNum;

        public DateTime UltimoMessaggio;
      


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

                UltimoMessaggio = DateTime.Now;

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

                TipiBattria.Add(new sbTipoBatteria()
                {
                    BatteryTypeId = 0x00,
                    BatteryType = "N.D.",
                    SortOrder = 0,
                    StandardChargeProfile = 0x00,
                    Obsolete = 0xFF,
                    VoltCella = 0,
                    VoltSoglia = 0,
                    VCellaMin = 0,
                    VCellaMax = 0,
                    UsaSpybatt = 0,
                    AbilitaEqual = 0,
                    AbilitaAttesaBMS = 0
                });
                TipiBattria.Add(new sbTipoBatteria()
                {
                    BatteryTypeId = 0x71,
                    BatteryType = "Pb/Lead",
                    SortOrder = 1,
                    StandardChargeProfile = 0x01,
                    Obsolete = 0x00,
                    VoltCella = 200,
                    VoltSoglia = 240,
                    VCellaMin = 100,
                    VCellaMax = 265,
                    UsaSpybatt = 1,
                    AbilitaEqual = 0x300C,
                    AbilitaAttesaBMS = 0x0000
                });
                TipiBattria.Add(new sbTipoBatteria()
                {
                    BatteryTypeId = 0x72,
                    BatteryType = "Gel",
                    SortOrder = 2,
                    StandardChargeProfile = 0x00,
                    Obsolete = 0x00,
                    VoltCella = 200,
                    VoltSoglia = 240,
                    VCellaMin = 100,
                    VCellaMax = 240,
                    UsaSpybatt = 1,
                    AbilitaEqual = 0x0000,
                    AbilitaAttesaBMS = 0x0000
                });
                TipiBattria.Add(new sbTipoBatteria()
                {
                    BatteryTypeId = 0x73,
                    BatteryType = "Lithium",
                    SortOrder = 3,
                    StandardChargeProfile = 0x00,
                    Obsolete = 0x00,
                    VoltCella = 0xFFFF,
                    VoltSoglia = 0,
                    VCellaMin = 0,
                    VCellaMax = 0,
                    UsaSpybatt = 0,
                    AbilitaEqual = 0,
                    AbilitaAttesaBMS = 0x05F0
                });

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
                switch(CanaleSpyBat)
                {
                    case CanaleDispositivo.USB:
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

                    case CanaleDispositivo.Seriale:
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

                    case CanaleDispositivo.BTStream:
                        {
                            if(streamSpyBatt == null)
                            {
                                return false;
                            }

                            if(streamSpyBatt.CanRead && streamSpyBatt.CanWrite)
                            {
                                return true;
                            }

                            return false;
                        }

                    default:
                        {
                            return false;
                        }

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

                switch (CanaleSpyBat)
                {
                    case CanaleDispositivo.USB:
                        {
                            uint bytesScritti = 0;
                            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
                            ftStatus = usbSpyBatt.Write(messaggio, (uint)NumByte, ref bytesScritti);
                            Log.Debug("Scrittura su USB: scritti " + bytesScritti + " bytes");
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Error("Failed to write to device (error " + ftStatus.ToString() + ")");
                                return false;
                            }
                            UltimoMessaggio = DateTime.Now;
                            return true;
                        }

                    case CanaleDispositivo.Seriale:
                        {
                            if (serialeSpyBatt.IsOpen)
                            {
                                Log.Debug("Scrittura su SERIALE");
                                serialeSpyBatt.Write(messaggio, Start, NumByte);
                                UltimoMessaggio = DateTime.Now;
                                return true;
                            }

                            return false;
                        }

                    case CanaleDispositivo.BTStream:
                        {
                           if ( streamSpyBatt.CanWrite)
                            {
                                Log.Debug("Scrittura su STREAM BT");
                                streamSpyBatt.Write(messaggio, Start, NumByte);
                                UltimoMessaggio = DateTime.Now;
                                return true;
                            }
                            return false;
                        }

                    default:
                        {
                            Log.Debug("Canale scrittura non definito  ------- > non scrivo nulla");
                            return false;
                        }
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

                switch (CanaleSpyBat)
                {
                    case CanaleDispositivo.USB:
                        {
                            return usbSpyBatt.IsOpen;
                        }
                    case CanaleDispositivo.Seriale:
                        {
                            return serialeSpyBatt.IsOpen;
                        }
                    case CanaleDispositivo.BTStream:
                        {
                            return (streamSpyBatt.CanRead && streamSpyBatt.CanWrite);
                        }

                    default:
                        {
                            return false;
                        }
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
                switch (CanaleSpyBat)
                {
                    case CanaleDispositivo.USB:
                        {
                            usbSpyBatt.Close();
                            return;
                        }
                    case CanaleDispositivo.Seriale:
                        {
                            serialeSpyBatt.Close();
                            return;
                        }
                    case CanaleDispositivo.BTStream:
                        {
                            streamSpyBatt.Close();
                            return ;
                        }

                    default:
                        {
                            return ;
                        }
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
                    if ( !SytemUtility.PortExist( serialeLadeLight.PortName))
                    {
                        return false;
                    }

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
                    Log.Debug("Comando LL: --> 0x" + hexdumpArray(messaggio));
                    Log.Debug("---------------------------------------------------------");


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

        private string hexdumpArray(byte[] buffer)
        {
            try
            {
                string _risposta = "";

                if (buffer == null)
                    return "";

                for (int _i = 0; _i < buffer.Length; _i++)
                {
                    _risposta += buffer[_i].ToString("X2");
                }
                return _risposta;
            }
            catch
            {
                return "";
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
