using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;

namespace PannelloCharger
{
    class modelloSigEcho
    {
        public List<echoMessaggio> ListaMessaggi { get; set; }
        public byte[] HexDump { get; set; }
        public string HexDumpText { get; set; }
        public DateTime Timestamp { get; set; }
        public string Note { get; set; }
        public ushort CRC { get; set; }

        public modelloSigEcho()
        {
            Timestamp = DateTime.Now;
            CRC = 0;
            Note = "";
        }
    }
}
