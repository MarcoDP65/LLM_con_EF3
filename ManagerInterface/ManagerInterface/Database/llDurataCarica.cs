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
    public class llDurataCarica
    {
        [PrimaryKey]
        public byte IdDurataCaricaLL { get; set; }

        public string Descrizione { get; set; }
        public byte Attivo { get; set; }
        public byte ProfiloPb { get; set; }
        public byte ProfiloGel { get; set; }
        public byte ProfiloLitio { get; set; }

        public int Ordine { get; set; }

    }


}
