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
        public ushort TipoCassone { get; set; }

        public ushort IdModelloLL { get; set; }

        public ushort AttesaBMS { get; set; }
        public byte AttivaEqual { get; set; }        // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF
        public byte AttivaRiarmoPulse { get; set; }  // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF

        public ushort CoeffK { get; set; }
        public ushort CoeffKc { get; set; }


        // Tempi
        public ushort TempoT0Max { get; set; }
        public ushort TempoT1Max { get; set; }
        public ushort TempoT2Min { get; set; }
        public ushort TempoT2Max { get; set; }
        public ushort TempoT3Max { get; set; }
        public ushort TempodT { get; set; }
        public ushort TempoFinale { get; set; }

        public ushort EqualTempoAttesa { get; set; }
        public ushort EqualTempoImpulso { get; set; }
        public ushort EqualTempoPausa { get; set; }

        public ushort MantTempoAttesa { get; set; }
        public ushort MantTempoMaxErogazione { get; set; }

        public ushort FattoreK { get; set; }

        public ushort OpportunityOraInizio { get; set; }
        public ushort OpportunityOraFine { get; set; }
        public ushort OpportunityDurataMax { get; set; }


        // Tensioni
        public ushort TensionePrecicloV0 { get; set; }
        public ushort TensioneSogliaVs { get; set; }
        public ushort TensioneRaccordoVr { get; set; }
        public ushort TensioneMassimaVMax { get; set; }
        public ushort TensioneLimiteVLim { get; set; }

        public ushort TensionedV { get; set; }

        public ushort MantTensIniziale { get; set; }
        public ushort MantTensFinale { get; set; }

        public ushort TensRiconoscimentoMin { get; set; }
        public ushort TensRiconoscimentoMax { get; set; }
        public ushort TensMinStop { get; set; }
        public ushort OpportunityTensioneMax { get; set; }


        // Correnti
        public ushort CorrenteI0 { get; set; }
        public ushort CorrenteI1 { get; set; }
        public ushort CorrenteFinaleI2 { get; set; }
        public ushort CorrenteMassima { get; set; }
        public ushort CorrenteI3 { get; set; }
        public ushort CorrenteRaccordoIr { get; set; }

        public ushort EqualCorrenteImpulso { get; set; }
        public ushort MantCorrenteImpulso { get; set; }
        public ushort OpportunityCorrente { get; set; }

        public ushort EqualNumImpulsi { get; set; }

        public ushort EqualAttivabile { get; set; }
        public ushort EqualAttivo { get; set; }

        public ushort MantAttivabile { get; set; }
        public ushort MantAttivo { get; set; }

        public ushort TemperaturaLimite { get; set; }

        public ushort OpportunityAttivabile { get; set; }
        public ushort OpportunityAttivo { get; set; }


        public ushort AbilitaSpyBatt { get; set; }
        public ushort AbilitaSafety { get; set; }

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
            TipoCassone = 0;

            AttesaBMS = 0;
            AttivaEqual = 0;        // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF
            AttivaRiarmoPulse = 0;  // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF

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

            OpportunityOraInizio = 0;
            OpportunityOraFine = 0;
            OpportunityDurataMax = 0;

            FattoreK = 0;

            // Tensioni
            TensionePrecicloV0 = 0;
            TensioneSogliaVs = 0;
            TensioneRaccordoVr = 0;
            TensioneMassimaVMax = 0;
            TensioneLimiteVLim = 0;

            MantTensIniziale = 0;
            MantTensFinale = 0;
            OpportunityTensioneMax = 0;
            TensRiconoscimentoMin = 0;
            TensRiconoscimentoMax = 0;
            TensMinStop = 0;

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
            OpportunityCorrente = 0;

            EqualAttivabile = 0;
            MantAttivabile = 0;

            OpportunityAttivo = 0;
            OpportunityAttivabile = 0;

            AbilitaSafety = 0;
            AbilitaSpyBatt = 0;

            Ordine = 0;

            Esito = 0;
            Messaggio = "";

            DatiValidi = false;


            return true;
        }
          
        public _llProgrammaCarica To_llPC()
        {
            _llProgrammaCarica _llPC = new _llProgrammaCarica();

            _llPC.IdLocale = 0;
            _llPC.IdProgramma = 0;
            //public string ProgramName { get; set; }
            _llPC.BatteryType = BatteryTypeId;
            //public ushort BatteryVdef { get; set; }
            //public ushort BatteryAhdef { get; set; }
            //public byte NumeroCelle { get; set; }

            _llPC.VSoglia = TensioneSogliaVs;
            _llPC.VRaccordoF1 = TensioneRaccordoVr;
            _llPC.VMax = TensioneMassimaVMax;
            _llPC.VCellLimite = TensioneLimiteVLim;
            _llPC.BatteryVminRec = TensRiconoscimentoMin;
            _llPC.BatteryVmaxRec = TensRiconoscimentoMax;
            _llPC.BatteryVminStop = TensMinStop;

            _llPC.CorrenteMax = CorrenteMassima;
            _llPC.CorrenteFase3 = CorrenteI3;

            _llPC.EqualTempoAttesa = EqualTempoAttesa;
            _llPC.EqualNumImpulsi = EqualNumImpulsi;
            _llPC.EqualDurataPausa = EqualTempoPausa;
            _llPC.EqualDurataImpulso = EqualTempoImpulso;
            _llPC.EqualCorrenteImpulso = EqualCorrenteImpulso;

            _llPC.IdProfilo = IdProfiloCaricaLL;
            //_llPC.DurataMaxCarica { get; set; }
            _llPC.PercTempoFase2 = FattoreK;
            _llPC.DurataMinFase2 = TempoT2Min;
            _llPC.DurataMaxFase2 = TempoT2Max;
            _llPC.DurataMaxFase3 = TempoT3Max;


            //_llPC.AbilitaComunicazioneSpybatt = (byte) AbilitaSpyBatt;

            //_llPC.TempoErogazioneBMS { get; set; }
            //_llPC.TempoAttesaBMS { get; set; }

            //_llPC.ProgrammaInUso { get; set; }

            //_llPC.TipoRecord { get; set; }
            //_llPC.OpzioniAttive { get; set; }


            return _llPC;

    }


        #region "Flag Visibilità"
        public bool Fase0Attiva
        {
            get
            {
                if (CorrenteI0 == 0 && TempoT0Max == 0 && TensionePrecicloV0 == 0)
                {
                    return false;
                }
                return true;
            }
        }

        public bool Fase1Attiva
        {
            get
            {
                if (CorrenteI1 == 0 && TempoT1Max == 0)
                {
                    return false;
                }
                return true;
            }
        }
        public bool Fase2Attiva
        {
            get
            {
                if (TempoT2Max == 0)
                {
                    return false;
                }
                return true;
            }
        }
        public bool Fase3Attiva
        {
            get
            {
                if (TempoT3Max == 0)
                {
                    return false;
                }
                return true;
            }
        }


        #endregion
    }


}
