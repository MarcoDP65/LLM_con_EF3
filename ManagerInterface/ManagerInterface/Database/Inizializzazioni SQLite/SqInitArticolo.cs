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
    public class SqInitArticolo
    {
        [PrimaryKey]
        public int IdInizializzazione { get; set; }
        public string CodArticolo { get; set; }
        public int? ProgrArticolo { get; set; }
        public int? Predefinito { get; set; }
        public int? Attivo { get; set; }
        public string Descrizione { get; set; }
        public byte IdModelloCB { get; set; }
        public byte TipoApparato { get; set; }
        public byte FamigliaApparato { get; set; }
        public string HardwareDisp { get; set; }
        public string SoftwareDISP { get; set; }
        public ushort VMin { get; set; }
        public ushort VMax { get; set; }
        public ushort Amax { get; set; }

        // Gestione Moduli SCHG
        public byte NumeroModuli { get; set; }
        public ushort ModVNom { get; set; }
        public ushort ModANom { get; set; }
        public ushort ModOpzioni { get; set; }
        public ushort ModVMin { get; set; }
        public ushort ModVMax { get; set; }




        public DateTime? DataCreazione { get; set; }
    }
}
