
using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using System.Globalization;

using log4net;
using log4net.Config;

using ChargerLogic;
using Utility;


namespace MoriData
{
    class llMemoryImage
    {
        public byte[] MappaGenerale { get; set; }
        public byte[] ParametriIniziali { get; set; }
        public byte[] DatiCliente { get; set; }
        public byte[] Programmazioni { get; set; }
        public byte[] Contatori { get; set; }
        public byte[] RecordBrevi { get; set; }
        public byte[] RecordLunghi { get; set; }
        public byte[] StatoFirmware { get; set; }


        public llMemoryImage()
        {
            MappaGenerale = new byte[2097152];
            ParametriIniziali = new byte[4096];
            DatiCliente = new byte[4096];
            Programmazioni = new byte[4096];
            Contatori = new byte[4096];
            RecordBrevi = new byte[1757184];
            RecordLunghi = new byte[16384];
            StatoFirmware = new byte[4096];
        }

    }
}
