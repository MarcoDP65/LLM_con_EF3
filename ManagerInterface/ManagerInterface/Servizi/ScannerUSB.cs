using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using log4net.Repository.Hierarchy;

namespace PannelloCharger
{
    public class ScannerUSB
    {
        public enum EsitoScasione: byte { InErrore = 0, NonEseguita = 1 , Parziale = 2 , Incompleta = 3 , Completa = 4, Busy = 5}

        private static ILog Log = LogManager.GetLogger("ScannerUSB");

        public string LadeLightUsbSerialNo;
        public string SpyBattUsbSerialNo;
        public string SeqDesUsbSerialNo;
        public string NoInitSerialNo;
        public string IdBattUsbSerialNo;
        public string RegenUsbSerialNo;


        public int NumDevLadeLight;
        public int NumDevSpyBatt;
        public int NumDevIdBatt;
        public int NumDevSeqDes;
        public int NumDevRegen;
        public int NumDevCharger;
        public int NumDevFTDInoInit;
        public List<UsbDevice> ListaPorte;
        public int Tentativo = 0;
        bool SemaforoRicerca = false;

        FTDI DeviceUsb = new FTDI();
        FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

        public ScannerUSB()
        {
            LadeLightUsbSerialNo = null;
            SpyBattUsbSerialNo = null;
            IdBattUsbSerialNo = null;
            SeqDesUsbSerialNo = null;
            RegenUsbSerialNo = null;
            NoInitSerialNo = null;
            NumDevLadeLight = 0;
            NumDevSpyBatt = 0;
            NumDevIdBatt = 0;
            NumDevSeqDes = 0;
            NumDevRegen = 0;
            NumDevFTDInoInit = 0;
            
            ListaPorte = new List<UsbDevice>();
        }


       public Boolean cercaPorte()
        {
            LadeLightUsbSerialNo = null;
            SpyBattUsbSerialNo = null;
            IdBattUsbSerialNo = null;
            SeqDesUsbSerialNo = null;
            RegenUsbSerialNo = null;
            NoInitSerialNo = null;
            NumDevLadeLight = 0;
            NumDevSpyBatt = 0;
            NumDevIdBatt = 0;
            NumDevSeqDes = 0;
            NumDevRegen = 0;
            NumDevFTDInoInit = 0;

            string campo;
            string chiave;

            ListaPorte.Clear();
            UInt32 ftdiDeviceCount = 0;
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
                    UsbDevice _elemento = new UsbDevice();
                    _elemento.Description = ftdiDeviceList[CicloDev].Description;
                    _elemento.Flags = ftdiDeviceList[CicloDev].Flags;
                    _elemento.ftHandle = ftdiDeviceList[CicloDev].ftHandle;
                    _elemento.ID = ftdiDeviceList[CicloDev].ID;
                    _elemento.LocId = ftdiDeviceList[CicloDev].LocId;
                    _elemento.SerialNumber = ftdiDeviceList[CicloDev].SerialNumber;

                    ListaPorte.Add(_elemento);

                    if (ftdiDeviceList[CicloDev].Description == "LADE LIGHT")
                    {
                        NumDevLadeLight++;
                        LadeLightUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Description == "SPY-BATT")
                    {
                        NumDevSpyBatt++;
                        SpyBattUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Type == FTDI.FT_DEVICE.FT_DEVICE_X_SERIES)
                        Log.Debug("Scansione porte USB: FT_DEVICE_X_SERIES " + ftdiDeviceList[CicloDev].Type.ToString());


                    if (ftdiDeviceList[CicloDev].Description == "FT201X USB I2C")
                    {
                        FTDI.FT_XSERIES_EEPROM_STRUCTURE _ftdiEpromData = new FTDI.FT_XSERIES_EEPROM_STRUCTURE();
                        FTDI _tempFtdi = new FTDI();

                        ftStatus = _tempFtdi.OpenBySerialNumber(ftdiDeviceList[CicloDev].SerialNumber);

                        ftStatus = _tempFtdi.ReadXSeriesEEPROM(_ftdiEpromData);


                        NumDevFTDInoInit++;
                        NoInitSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }


                    if (ftdiDeviceList[CicloDev].Description == "DESOLFATATORE")
                    {
                        NumDevSpyBatt++;
                        NumDevSeqDes++;
                        SpyBattUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                        SeqDesUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Description == "ID-BATT PROGRAMMER")
                    {
                        NumDevIdBatt++;
                        IdBattUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Description == "BATTERY REGENERATOR")
                    {
                        NumDevRegen++;
                        RegenUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Description == "REGENERATOR")
                    {
                        NumDevRegen++;
                        RegenUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Description == "PSW SUPERCHARGER")
                    {
                        NumDevCharger++;
                        RegenUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                }
            }
            else
            {
                Log.Error("Errore scansione porte USB: " + ftStatus.ToString());
                return false;
            }



            if (NumDevLadeLight > 0 || NumDevSpyBatt > 0 || NumDevFTDInoInit > 0 || NumDevRegen >0 || NumDevIdBatt > 0 || NumDevCharger > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        public async Task<EsitoScasione> CaricaPorteAsync()
        {
            int Esecuzione = 0;
            try
            {
                if (SemaforoRicerca)
                {
                    Log.Info("SEMAFORO Scanner ");
                    return EsitoScasione.Busy;
                }
                else
                {
                    SemaforoRicerca = true;
                }


                Log.Info("CaricaPorteAsync START " + Esecuzione.ToString());
                //var tcs = new TaskCompletionSource<EsitoScasione>();
                EsitoScasione ScanResult = EsitoScasione.NonEseguita;
                await Task.Run(async () =>
                {
                    do
                    {
                        Log.Info("Run START: " + (Esecuzione++).ToString());
                        //EsitoScasione ResultSync
                        ScanResult = CercaPorteNew(Esecuzione);
                        //tcs.SetResult(ResultSync);
                        Log.Info("After SET " + Esecuzione.ToString());

                        //ScanResult = await tcs.Task;
                        if(ScanResult != EsitoScasione.Completa)
                        {
                            await Task.Delay(200);
                        }
                    }
                    while (ScanResult != EsitoScasione.Completa);
                });
                Log.Info("CaricaPorteAsync After Loop " + Esecuzione.ToString());
                SemaforoRicerca = false;
                return ScanResult;

            }

            catch (Exception ex)
            {

                Log.Error("[WIN] CercaPorteNewAsync Error:" + ex.Message, ex);
                SemaforoRicerca = false;
                return EsitoScasione.InErrore;
            }

        }


        public async Task<EsitoScasione> CercaPorteNewAsync()
        {
            try
            {


                Log.Info("CercaPorteNewAsync START " + (Tentativo++).ToString());
                var tcs = new TaskCompletionSource<EsitoScasione>();
                await Task.Run(() =>
                {
                    Log.Info("CercaPorteNew run START: " + (Tentativo).ToString());
                    EsitoScasione ResultSync = CercaPorteNew(Tentativo);
                    Log.Info("CercaPorteNewAsync BEFORE SET");
                    tcs.SetResult(ResultSync);
                    Log.Info("CercaPorteNewAsync After SET");
                });

                Log.Info("CercaPorteNewAsync After TASK " + Tentativo.ToString());
                EsitoScasione Result = await tcs.Task;
                return Result; // tcs.Task;

            }
            catch (Exception ex)
            {

                Log.Error("[WIN] CercaPorteNewAsync Error:" + ex.Message, ex);
                return EsitoScasione.InErrore;
            }

        }


        public EsitoScasione CercaPorteNew(int Run = 0)
        {
            LadeLightUsbSerialNo = null;
            SpyBattUsbSerialNo = null;
            IdBattUsbSerialNo = null;
            SeqDesUsbSerialNo = null;
            RegenUsbSerialNo = null;
            NoInitSerialNo = null;
            NumDevLadeLight = 0;
            NumDevSpyBatt = 0;
            NumDevIdBatt = 0;
            NumDevSeqDes = 0;
            NumDevRegen = 0;
            NumDevFTDInoInit = 0;

            string campo;
            string chiave;


            EsitoScasione Stato = EsitoScasione.NonEseguita;
            ListaPorte.Clear();
            Log.Info("CercaPorteNew Start" + Run.ToString());
            UInt32 ftdiDeviceCount = 0;
            // Determine the number of FTDI devices connected to the machine
            ftStatus = DeviceUsb.GetNumberOfDevices(ref ftdiDeviceCount);
            // Check status
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                Log.Error("Errore scansione porte USB: " + ftStatus.ToString());
                Stato = EsitoScasione.InErrore;
                Log.Info("CercaPorteNew InErrore");
                return Stato;
            }
            // Allocate storage for device info list
            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];
            // Populate our device list
            ftStatus = DeviceUsb.GetDeviceList(ftdiDeviceList);
            Log.Info("CercaPorteNew GetDeviceList");
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                Stato = EsitoScasione.NonEseguita;
                uint CicloDev = 0;
                for (CicloDev = 0; CicloDev < ftdiDeviceCount; CicloDev++)
                {
                    UsbDevice _elemento = new UsbDevice();
                    _elemento.Description = ftdiDeviceList[CicloDev].Description;
                    _elemento.Flags = ftdiDeviceList[CicloDev].Flags;
                    _elemento.ftHandle = ftdiDeviceList[CicloDev].ftHandle;
                    _elemento.ID = ftdiDeviceList[CicloDev].ID;
                    _elemento.LocId = ftdiDeviceList[CicloDev].LocId;
                    _elemento.SerialNumber = ftdiDeviceList[CicloDev].SerialNumber;
                    if (_elemento.ID == 0) Stato = EsitoScasione.Incompleta;
                    ListaPorte.Add(_elemento);

                    if (ftdiDeviceList[CicloDev].Description == "LADE LIGHT")
                    {
                        NumDevLadeLight++;
                        LadeLightUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Description == "SPY-BATT")
                    {
                        NumDevSpyBatt++;
                        SpyBattUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Type == FTDI.FT_DEVICE.FT_DEVICE_X_SERIES)
                        Log.Debug("Scansione porte USB: FT_DEVICE_X_SERIES " + ftdiDeviceList[CicloDev].Type.ToString());


                    if (ftdiDeviceList[CicloDev].Description == "FT201X USB I2C")
                    {
                        FTDI.FT_XSERIES_EEPROM_STRUCTURE _ftdiEpromData = new FTDI.FT_XSERIES_EEPROM_STRUCTURE();
                        FTDI _tempFtdi = new FTDI();

                        ftStatus = _tempFtdi.OpenBySerialNumber(ftdiDeviceList[CicloDev].SerialNumber);

                        ftStatus = _tempFtdi.ReadXSeriesEEPROM(_ftdiEpromData);


                        NumDevFTDInoInit++;
                        NoInitSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }


                    if (ftdiDeviceList[CicloDev].Description == "DESOLFATATORE")
                    {
                        NumDevSpyBatt++;
                        NumDevSeqDes++;
                        SpyBattUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                        SeqDesUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Description == "ID-BATT PROGRAMMER")
                    {
                        NumDevIdBatt++;
                        IdBattUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Description == "BATTERY REGENERATOR")
                    {
                        NumDevRegen++;
                        RegenUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Description == "REGENERATOR")
                    {
                        NumDevRegen++;
                        RegenUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                    if (ftdiDeviceList[CicloDev].Description == "PSW SUPERCHARGER")
                    {
                        NumDevCharger++;
                        RegenUsbSerialNo = ftdiDeviceList[CicloDev].SerialNumber;
                    }

                }
                if (Stato != EsitoScasione.Incompleta)
                {
                    Stato = EsitoScasione.Completa;
                }

                Log.Info("CercaPorteNew End " + Run.ToString() + " - Trovati " + ftdiDeviceCount.ToString() + " Stato " + Stato.ToString());

            }
            else
            {
                Log.Error("Errore scansione porte USB: " + ftStatus.ToString());
                return EsitoScasione.InErrore;
            }



            if (NumDevLadeLight > 0 || NumDevSpyBatt > 0 || NumDevFTDInoInit > 0 || NumDevRegen > 0 || NumDevIdBatt > 0 || NumDevCharger > 0)
            {
                return Stato;
            }
            else
            {
                return Stato;
            }

            return Stato;

        }

        public class UsbDevice
        {
            // Dati presi da FT_DEVICE_INFO_NODE
            //     The device description
            public string Description { get; set; }
            //     Indicates device state. Can be any combination of the following: FT_FLAGS_OPENED,FT_FLAGS_HISPEED
            public uint Flags { get; set; }
            //     The device handle. This value is not used externally and is provided for information only. If the device is not open, this value is 0.
            public IntPtr ftHandle { get; set; }
            //     The Vendor ID and Product ID of the device
            public uint ID { get; set; }
            //     The physical location identifier of the device
            public uint LocId { get; set; }
            //     The device serial number
            public string SerialNumber { get; set; }
            //     Indicates the device type. Can be one of the following: FT_DEVICE_232R, FT_DEVICE_2232C, FT_DEVICE_BM, FT_DEVICE_AM, FT_DEVICE_100AX or FT_DEVICE_UNKNOWN
            public FTDI.FT_DEVICE Type { get; set; }


            public UsbDevice()
            {

            }


            public string strID
            {
                get
                {
                    return ID.ToString("x8");
                }
            }

            public string strLocId
            {
                get
                {
                    return LocId.ToString("x8");
                }
            }

        }

    }
}
