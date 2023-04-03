//    class llTensioneBatteria
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using log4net;
using log4net.Config;

namespace MoriData
{
    /// <summary>
    /// llProfiloCarica: struttura record per il salvataggio dati modello Lade Light
    /// </summary>
    /// 
    public class llTensioneBatteria
    {
        [PrimaryKey]
        public ushort IdTensione { get; set; }
        public string Descrizione { get; set; }

        public byte Attivo { get; set; }
        public int Ordine { get; set; }

    }


    public class llTensioniModello
    {
        [Indexed(Name = "IDXTensioniModello", Order = 1, Unique = true)]
        public ushort IdTensione { get; set; }
        [Indexed(Name = "IDXTensioniModello", Order = 2, Unique = true)]
        public byte IdModelloLL { get; set; }
        public string TxTensione { get; set; }

        public byte Attivo { get; set; }

    }



}
