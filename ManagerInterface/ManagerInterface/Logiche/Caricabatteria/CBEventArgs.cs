using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargerLogic
{
    public class CBEventArgs : EventArgs
    {
        public BaudRate CurrentBaudrate { get; set; }
        public String Message { get; set; } 

    }
}
