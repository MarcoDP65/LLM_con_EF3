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

namespace MoriData
{
    public class _sbInfoLibreria
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        public string IdApparato { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }

        public string VersioneLibreria { get; set; }
        public ushort IsSetup { get; set; }
        public ushort TensioneNominale { get; set; }
        public ushort CorrenteNominale { get; set; }
        public uint LenFlash { get; set; }
        public uint AddrFlash2 { get; set; }
        public uint LenFlash2 { get; set; }
        public uint AddrProxy { get; set; }
        public uint LenProxy { get; set; }
        public byte Stato { get; set; }

        public DateTime IstanteLettura { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IstanteLettura.ToString();
        }

    }
    public class sbInfoLibreria
    {
        public _sbInfoLibreria _sBIL;

    }
}
