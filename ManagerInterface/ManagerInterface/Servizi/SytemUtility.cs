using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Microsoft.Win32;

namespace Utility
{
    class SytemUtility
    {
        public static bool PortExist(string PortName)
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach( string Porta in ports)
                {
                    if (Porta == PortName) return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }



    }
}
