
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
    /// llProfiloCarica: struttura record per il salvataggio dati modello Lade Light
    /// </summary>
    /// 
    public class _llProfiloCarica
    {
        [PrimaryKey]
        public byte IdProfiloCaricaLL { get; set; }

        public string NomeProfilo { get; set; }
        public ushort DutataFase2 { get; set; }
        public byte Attivo { get; set; }
        public byte FlagPb { get; set; }
        public byte FlagGel { get; set; }
        public byte FlagLitio { get; set; }
        public byte TipoBatteria { get; set; }
        public short AttesaBMS { get; set; }

        public int Ordine { get; set; }

    }

    public class _llProfiloTipoBatt
    {
        [PrimaryKey]
        public byte IdProfiloCaricaLL { get; set; }
        public byte BatteryTypeId { get; set; }

        public byte Attivo { get; set; }
    }
}




