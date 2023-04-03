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
    /// <summary>
    /// _llModelloCb: struttura record per il salvataggio dati modello Lade Light
    /// </summary>
    /// 
    public class _llModelloCb
    {
        [PrimaryKey]
        public byte IdModelloLL { get; set; }      // (0b)1xxxxxxx   LadeLight
                                                   // (0b)0VVAAMMM   SuperCharger   
                                                   // VV: volt nominali  01 = 24 | 10 = 48 | 11 = 80  | 00  -  Non Standard
                                                   // AA: Ampere erogati 01 = 80 | 10 = 120| 11 = 160 | 00  -  Non Standard
                                                   // MMM: Moduli presenti                            | 000 -  Non Standard


        public string NomeModello { get; set; }
        public double CorrenteMin { get; set; }
        public double CorrenteMax { get; set; }
        public double TensioneMin { get; set; }
        public double TensioneMax { get; set; }
        public byte Attivo { get; set; }
        public double TensioneNominale { get; set; }

        public ushort Opzioni { get; set; }

        public byte Trifase { get; set; }
        public int Ordine { get; set; }
        public CaricaBatteria.TipoCaricaBatteria FamigliaCaricabetteria { get; set; }

    }


}




