using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PannelloCharger.UsbHelper;

namespace PannelloCharger
{
    public static class UsbHelper
    {
        public enum UsbAction : byte { NoOP  = 0, Connect = 1,Disconnect = 2 }
    }

    public class UsbConnectionEventArgs : EventArgs
    {
        public UsbAction ActionDetected { get; set; }

    }


}
