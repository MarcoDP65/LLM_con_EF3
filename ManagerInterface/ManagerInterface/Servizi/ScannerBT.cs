using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using InTheHand.Net.Sockets;

namespace PannelloCharger
{
    class ScannerBT
    {

        List<DeviceBt> devices = new List<DeviceBt>();


        public ScannerBT()
        {
            BluetoothClient bc = new BluetoothClient();
            BluetoothDeviceInfo[] DIarray = bc.DiscoverDevices();
        }


    }
}
