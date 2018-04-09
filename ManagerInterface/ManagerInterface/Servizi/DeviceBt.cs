using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using InTheHand.Net;
using InTheHand.Net.Sockets;

namespace PannelloCharger
{
    public class DeviceBt
    {
        public BluetoothDeviceInfo DevInf { get; set; }
        public BluetoothAddress Address { get; private set; }
        public string DeviceName { get; set; }
        public bool Authenticated { get; set; }
        public bool Connected { get; set; }
        public ushort Nap { get; set; }
        public uint Sap { get; set; }
        public int Rssi { get; set; }
        public DateTime LastSeen { get; set; }
        public DateTime LastUsed { get; set; }
        public bool Remembered { get; set; }
        public bool SppAvail { get; set; }

        public DeviceBt(BluetoothDeviceInfo device_info)
        {
            try
            {
                DevInf = device_info;
                this.Authenticated = device_info.Authenticated;
                this.Connected = device_info.Connected;
                this.DeviceName = device_info.DeviceName;
                this.LastSeen = device_info.LastSeen;
                this.LastUsed = device_info.LastUsed;
                this.Nap = device_info.DeviceAddress.Nap;
                this.Sap = device_info.DeviceAddress.Sap;
                this.Remembered = device_info.Remembered;
                Rssi = device_info.Rssi;
                Address = device_info.DeviceAddress;
                SppAvail = SppDefined(device_info);
            }
            catch
            {

            }
        }

        public override string ToString()
        {
            return this.DeviceName;
        }

        /// <summary>
        /// Verifica che il servizio SPP sia presente.
        /// </summary>
        /// <returns>System.Boolean.</returns>
        private bool SppDefined(BluetoothDeviceInfo device_info)
        {
            try
            {
                Guid SppGuid = new Guid("{00001101-0000-1000-8000-00805f9b34fb}");
                
                foreach (Guid _guid in device_info.InstalledServices)
                {
                    if (_guid == SppGuid) return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }



        public string strAuthenticated
        {
            get
            {
                
                if (Authenticated)
                    return "SI";
                else
                    return "NO";

            }
        }

        public string strConnected
        {
            get
            {

                if (Connected)
                    return "SI";
                else
                    return "NO";

            }
        }

        public string strSppAvail
        {
            get
            {

                if (SppAvail)
                    return "SI";
                else
                    return "NO";

            }
        }

        public string strRSSI
        {
            get
            {
                return Rssi.ToString();
            }
        }

    }
}
