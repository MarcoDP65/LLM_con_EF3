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
        public ushort IdDurataCaricaLL { get; set; }
        public string Descrizione { get; set; }

        public byte Attivo { get; set; }
        public byte ProfiloPb { get; set; }
        public byte ProfiloGel { get; set; }
        public byte ProfiloLitio { get; set; }

        public int Ordine { get; set; }

    }


    public class llDurataProfilo
    {
        [Indexed(Name = "IDXDurataProfilo", Order = 1, Unique = true)]
        public ushort IdDurataCaricaLL { get; set; }
        [Indexed(Name = "IDXDurataProfilo", Order = 2, Unique = true)]
        public byte IdProfiloCaricaLL { get; set; }

        public byte Attivo { get; set; }
 
    }



}
