using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using log4net;
using log4net.Config;

using ChargerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using log4net;
using log4net.Config;

using ChargerLogic;

namespace MoriData
{
    public class SqStepInit
    {
        [Indexed(Name = "MultiColumnPK", Order = 1, Unique = true)]
        public int IdInizializzazione { get; set; }
        [Indexed(Name = "MultiColumnPK", Order = 2, Unique = true)]
        public int Step { get; set; }
        public TipoStep Attivita { get; set; }
        public bool Attivo { get; set; }
        public int? Obbligatorio { get; set; }
        public string Descrizione { get; set; }
        public uint Address { get; set; }
        public string StrDataArray { get; set; }   
        public byte[] DataArray { get; set; } 
        public uint DataLenght { get; set; }    
        public string ParLongChar01 { get; set; }
        public string ParLongChar02 { get; set; }
        public string ParVarChar01 { get; set; }
        public string ParVarChar02 { get; set; }
        public string ParVarChar03 { get; set; }
        public string ParVarChar04 { get; set; }
        public string ParVarChar05 { get; set; }
        public int? ParInt01 { get; set; }
        public int? ParInt02 { get; set; }
        public int? ParInt03 { get; set; }
        public int? ParInt04 { get; set; }
        public int? ParInt05 { get; set; }
        public int? ParInt06 { get; set; }
        public int? ParInt07 { get; set; }
        public int? ParInt08 { get; set; }
        public int? ParInt09 { get; set; }
        public int? ParInt10 { get; set; }

        // ------------------------------------------
        public enum TipoStep : byte { NOOP = 0, Delete4K = 1, WriteInit = 2, AddProfilo = 3, SetTime = 4,ReadInit = 0x10, Reboot = 0xFF }

        public string strTipoAttivita
        {
            get
            {
                return Attivita.ToString();
            }
        }
    }
}
