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
using Utility;
using MoriData;

namespace ChargerLogic
{
    /// <summary>
    /// Classe modello per il salvataggio del Dump Memoria
    /// </summary>
    public class ImageDump
    {
        public enum TipoApparato :byte {  SpyBatt  = 0x00, LadeLight = 0x01, Display = 0x02, Desolfatatore = 0x03};
        public TipoApparato Apparato { get; set; }
        public byte[] DataBuffer { get; set; }
        public spybattData Testata { get; set; }
        public DateTime Timestamp { get; set; }
        public MessaggioSpyBatt.comandoInizialeSB IntestazioneSb { get; set; }
    }
}
