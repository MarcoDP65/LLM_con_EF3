using ChargerLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PannelloCharger
{
    public class DatiStepInizializzazione
    {
        public int IdSetupIniziale { get; set; }

        public string ProduttoreApparato { get; set; }  // 18 byte, fisso MORI RADDRIZZATORI 
        public string NomeApparato { get; set; }        // 10 byte, fisso LADE LIGHT  oppure 16 byte PSW SUPERCHARGER

        public CaricaBatteria.TipoCaricaBatteria SerieApparato { get; set; }

        public uint SerialeApparato { get; set; }
        public byte AnnoCodice { get; set; }
        public uint ProgressivoCodice { get; set; }
        public byte TipoApparato { get; set; }
        public uint DataSetupApparato { get; set; }

        public uint MaxRecordBrevi { get; set; }
        public ushort MaxRecordCarica { get; set; }
        public uint SizeExternMemory { get; set; }

        public byte MaxProgrammazioni { get; set; }
        public byte ModelloMemoria { get; set; }
        public string IDApparato { get; set; }
        public ushort VMin { get; set; }
        public ushort VMax { get; set; }
        public ushort Amax { get; set; }
        public byte PresenzaRabboccatore { get; set; }

        // Gestione Moduli SCHG
        public byte NumeroModuli { get; set; }
        public ushort ModVNom { get; set; }
        public ushort ModANom { get; set; }
        public ushort ModOpzioni { get; set; }
        public ushort ModVMin { get; set; }
        public ushort ModVMax { get; set; }
    }
}
