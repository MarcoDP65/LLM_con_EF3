using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using System.IO;
using log4net;
using log4net.Config;

namespace MoriData
{
    /// <summary>
    /// _llModelloCb: struttura record per il salvataggio dati modello Lade Light
    /// </summary>
    /// 
    public class _llModelloCb
    {
        [PrimaryKey]
        public byte IdModelloLL { get; set; }

        public string NomeModello { get; set; }
        public double CorrenteMin { get; set; }
        public double CorrenteMax { get; set; }
        public double TensioneMin { get; set; }
        public double TensioneMax { get; set; }

        public byte Trifase { get; set; }
        public int Ordine { get; set; }

    }


}




