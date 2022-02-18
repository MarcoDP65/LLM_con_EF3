
//    class mbParProfiloTipoBatt


// class mbProfiloCarica
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
    public class _mbProfiloTipoBatt
    {

        [PrimaryKey]
        public byte IdProfiloCaricaLL { get; set; }
        public ushort BatteryTypeId { get; set; }
        public byte Attivo { get; set; }


        public short AttesaBMS { get; set; }
        public byte AttivaEqual { get; set; }        // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF
        public byte AttivaRiarmoPulse { get; set; }  // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF

        // Tempi
        public string TempoT0Max { get; set; }
        public string TempoT1Max { get; set; }
        public string TempoT2Min { get; set; }
        public string TempoT2Max { get; set; }
        public string TempoT3Max { get; set; }
        public string DurataNominale { get; set; }

        public string EqualTempoAttesa { get; set; }
        public string EqualTempoImpulso { get; set; }
        public string EqualTempoPausa { get; set; }

        public string MantTempoAttesa { get; set; }
        public string MantTempoMaxErogazione { get; set; }

        public string FattoreK { get; set; }
        public string FattoreKs { get; set; }

        // Tensioni
        public string TensionePrecicloV0 { get; set; }
        public string TensioneSogliaVs { get; set; }
        public string TensioneRaccordoVr { get; set; }
        public string TensioneMassimaVMax { get; set; }
        public string TensioneLimiteVLim { get; set; }
        public string TensRiconoscimentoMin { get; set; }
        public string TensRiconoscimentoMax { get; set; }

        public string TensMinimaStop { get; set; }

        public string MantTensIniziale { get; set; }
        public string MantTensFinale { get; set; }

        // Correnti
        public string CorrenteI0 { get; set; }
        public string CorrenteI1 { get; set; }
        public string CorrenteFinaleI2 { get; set; }
        public string CorrenteMassima { get; set; }
        public string CorrenteI3 { get; set; }
        public string CorrenteRaccordoIr { get; set; }

        public string EqualCorrenteImpulso { get; set; }
        public string MantCorrenteImpulso { get; set; }

        public string EqualNumImpulsi { get; set; }

        public string OpportunityOraInizio { get; set; }
        public string OpportunityOraFine { get; set; }
        public string OpportunityDurataMax { get; set; }
        public string OpportunityTensioneMax { get; set; }
        public string OpportunityCorrente { get; set; }




        public string EqualAttivabile { get; set; }
        public string MantAttivabile { get; set; }
        public string OpportunityAttivabile { get; set; }



        public string AbilitaSpyBatt { get; set; }
        public string AbilitaSafety { get; set; }

        public int Ordine { get; set; }

    }

    public class mbProfiloTipoBatt
    {
        public _mbProfiloTipoBatt data;
    }
}