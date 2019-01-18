//    class ParametriCiclo



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
    public class ParametriCiclo
    {

        [PrimaryKey]
        public byte IdProfiloCaricaLL { get; set; }
        public ushort BatteryTypeId { get; set; }
        public byte Attivo { get; set; }


        public short AttesaBMS { get; set; }
        public byte AttivaEqual { get; set; }        // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF
        public byte AttivaRiarmoPulse { get; set; }  // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF

        // Tempi
        public ushort TempoT0Max { get; set; }
        public ushort TempoT1Max { get; set; }
        public ushort TempoT2Min { get; set; }
        public ushort TempoT2Max { get; set; }
        public ushort TempoT3Max { get; set; }

        public ushort EqualTempoAttesa { get; set; }
        public ushort EqualTempoImpulso { get; set; }
        public ushort EqualTempoPausa { get; set; }

        public ushort MantTempoAttesa { get; set; }
        public ushort MantTempoMaxErogazione { get; set; }

        public ushort FattoreK { get; set; }

        // Tensioni
        public ushort TensionePrecicloV0 { get; set; }
        public ushort TensioneSogliaVs { get; set; }
        public ushort TensioneRaccordoVr { get; set; }
        public ushort TensioneMassimaVMax { get; set; }
        public ushort TensioneLimiteVLim { get; set; }

        public ushort MantTensIniziale { get; set; }
        public ushort MantTensFinale { get; set; }

        // Correnti
        public ushort CorrenteI0 { get; set; }
        public ushort CorrenteI1 { get; set; }
        public ushort CorrenteFinaleI2 { get; set; }
        public ushort CorrenteMassima { get; set; }
        public ushort CorrenteI3 { get; set; }
        public ushort CorrenteRaccordoIr { get; set; }

        public ushort EqualCorrenteImpulso { get; set; }
        public ushort MantCorrenteImpulso { get; set; }

        public ushort EqualNumImpulsi { get; set; }

        public ushort EqualAttivabile { get; set; }

        public int Ordine { get; set; }

        public bool DatiValidi { get; set; }

        // Parametri gestione flusso
        public byte Esito { get; set; }
        public string Messaggio { get; set; }

        public ParametriCiclo()
        {
            AzzeraValori();
        }

        public bool AzzeraValori()
        {


            IdProfiloCaricaLL = 0;
            BatteryTypeId = 0;
            Attivo = 0;


            AttesaBMS = 0;
            AttivaEqual = 0;        // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF
            AttivaRiarmoPulse = 0; // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF

            // Tempi
            TempoT0Max = 0;
            TempoT1Max = 0;
            TempoT2Min = 0;
            TempoT2Max = 0;
            TempoT3Max = 0;

            EqualTempoAttesa = 0;
            EqualTempoImpulso = 0;
            EqualTempoPausa = 0;

            MantTempoAttesa = 0;
            MantTempoMaxErogazione = 0;

            FattoreK = 0;

            // Tensioni
            TensionePrecicloV0 = 0;
            TensioneSogliaVs = 0;
            TensioneRaccordoVr = 0;
            TensioneMassimaVMax = 0;
            TensioneLimiteVLim = 0;

            MantTensIniziale = 0;
            MantTensFinale = 0;

            // Correnti
            CorrenteI0 = 0;
            CorrenteI1 = 0;
            CorrenteFinaleI2 = 0;
            CorrenteMassima = 0;
            CorrenteI3 = 0;
            CorrenteRaccordoIr = 0;

            EqualCorrenteImpulso = 0;
            MantCorrenteImpulso = 0;
            EqualNumImpulsi = 0;

            EqualAttivabile = 0;

            Ordine = 0;

            Esito = 0;
            Messaggio = "";

            DatiValidi = false;


            return true;
        }
          

    }


}
