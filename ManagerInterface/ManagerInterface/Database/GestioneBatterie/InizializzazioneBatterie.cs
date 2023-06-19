//    class InizializzazioneBatterie
using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;

using System.Threading;
using System.ComponentModel;

using SQLite;

namespace ChargerLogic
{

    public class CicloDiCarica
    {

        public List<_mbProfiloCarica> ProfiliCarica;
        public List<mbTipoBatteria> ModelliBatteria;
        public List<llDurataCarica> DurateCarica;
        public List<llDurataProfilo> DurateProfilo;
        public List<_llProfiloTipoBatt> ProfiloTipoBatt;
        public List<llTensioneBatteria> TensioniBatteria;
        public List<llTensioniModello> TensioniModello;
        public List<llMemoriaCicli> MemoriaCicli;
        public List<llMemBreve> BreviCicloCorrente;
        public List<_mbProfiloTipoBatt> ParametriCarica;



        public bool InizializzaDatiLocali()
        {
            inizializzaProfili();
            inizializzaBatterie();
            inizializzaDurate();
            inizializzaDurateProfilo();
            inizializzaBatteriaProfilo();
            inizializzaTensioni();
            inizializzaTensioniModello();
            inizializzaParametriCarica();
            return true;
        }

        public bool inizializzaProfili()
        {
            ProfiliCarica = new List<_mbProfiloCarica>();
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x00, NomeProfilo = "Non Definito", DurataFase2 = 100, Attivo = 2, FlagPb = 0, FlagGel = 0, FlagLitio = 0, Ordine = 0, AttivaRiarmoPulse = 0, AttivaEqual = 0x00, AttivaOpportunity = 0x00, AttivaMant = 0xF0, Grafico = "" }) ;
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x01, NomeProfilo = "IWa", DurataFase2 = 100, Attivo = 0, FlagPb = 0, FlagGel = 0, FlagLitio = 0, Ordine = 1, AttivaRiarmoPulse = 0, AttivaEqual = 0xF0, AttivaMant = 0xF0, AttivaOpportunity = 0xF0, Grafico = "IWa650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x02, NomeProfilo = "IU", DurataFase2 = 100, Attivo = 1, FlagPb = 0, FlagGel = 1, FlagLitio = 0, Ordine = 2, AttivaRiarmoPulse = 0, AttivaEqual = 0x00, AttivaMant = 0xF0, AttivaOpportunity = 0xF0 ,Grafico = "IU650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x03, NomeProfilo = "IUIa", DurataFase2 = 100, Attivo = 0, FlagPb = 0, FlagGel = 0, FlagLitio = 0, Ordine = 3, AttivaRiarmoPulse = 0, AttivaEqual = 0x00, AttivaMant = 0xF0, AttivaOpportunity = 0x0F, Grafico = "IUIa650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x04, NomeProfilo = "Pb13h", DurataFase2 = 60, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 4, AttivaRiarmoPulse = 0, AttivaEqual = 0xF0, AttivaMant = 0xF0, AttivaOpportunity = 0xF0, Grafico = "IWa650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x05, NomeProfilo = "Pb11h", DurataFase2 = 100, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 5, AttivaRiarmoPulse = 0, AttivaEqual = 0xF0, AttivaMant = 0xF0, AttivaOpportunity = 0xF0, Grafico = "IWa650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x06, NomeProfilo = "Pb8h", DurataFase2 = 120, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 6, AttivaRiarmoPulse = 0, AttivaEqual = 0xF0, AttivaMant = 0xF0, AttivaOpportunity = 0xF0, Grafico = "IWa650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x07, NomeProfilo = "Litio", DurataFase2 = 100, Attivo = 1, FlagPb = 0, FlagGel = 0, FlagLitio = 1, Ordine = 7, AttivaRiarmoPulse = 0xF0, AttivaEqual = 0xF0, AttivaMant = 0x00, AttivaOpportunity = 0x00, Grafico = "LITIO650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x08, NomeProfilo = "IWa Pb13 Equal", DurataFase2 = 60, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 8, AttivaRiarmoPulse = 0, AttivaEqual = 0xFF, AttivaMant = 0xF0, AttivaOpportunity = 0xF0, Grafico = "IWa650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x09, NomeProfilo = "IWa Pb11 Equal", DurataFase2 = 100, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 9, AttivaRiarmoPulse = 0, AttivaEqual = 0xFF, AttivaMant = 0xF0, AttivaOpportunity = 0xF0, Grafico = "IWa650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x0A, NomeProfilo = "IWa Pb8 Equal", DurataFase2 = 120, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 10, AttivaRiarmoPulse = 0, AttivaEqual = 0xFF, AttivaMant = 0xF0, AttivaOpportunity = 0xF0, Grafico = "IWa650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x0B, NomeProfilo = "Litio con BMS", DurataFase2 = 100, Attivo = 1, FlagPb = 0, FlagGel = 0, FlagLitio = 1, Ordine = 7, AttivaRiarmoPulse = 0xF0, AttivaEqual = 0x00, AttivaMant = 0xF0, AttivaOpportunity = 0x00, Grafico = "" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x0C, NomeProfilo = "Litio con CAN", DurataFase2 = 100, Attivo = 1, FlagPb = 0, FlagGel = 0, FlagLitio = 1, Ordine = 7, AttivaRiarmoPulse = 0xF0, AttivaEqual = 0x00, AttivaMant = 0xF0, AttivaOpportunity = 0x00, Grafico = "" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x0E, NomeProfilo = "SuperCAP", DurataFase2 = 100, Attivo = 1, FlagPb = 0, FlagGel = 0, FlagLitio = 1, Ordine = 8, AttivaRiarmoPulse = 0xF0, AttivaEqual = 0xF0, AttivaMant = 0x00, AttivaOpportunity = 0x00, Grafico = "LITIO650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x0F, NomeProfilo = "QUASAR Std.", DurataFase2 = 100, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 8, AttivaRiarmoPulse = 0x00, AttivaEqual = 0xF0, AttivaMant = 0xFF, AttivaOpportunity = 0x00, Grafico = "Quasar650" });
            ProfiliCarica.Add(new _mbProfiloCarica() { IdProfiloCaricaLL = 0x10, NomeProfilo = "QUASAR Fast", DurataFase2 = 100, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 8, AttivaRiarmoPulse = 0x00, AttivaEqual = 0xF0, AttivaMant = 0xFF, AttivaOpportunity = 0x00, Grafico = "Quasar650" });

            return true;
        }

        public bool inizializzaBatterie()
        {
            ModelliBatteria = new List<mbTipoBatteria>();

            ModelliBatteria.Add(new mbTipoBatteria() 
            { 
                BatteryTypeId = 0x0000, 
                SortOrder = 0, 
                BatteryType = "N.D.", 
                DivisoreCelle = 200, 
                StandardChargeProfile = 0x00 ,
                OldBatteryTypeId = 0x00,
                TensioniFisse = 0
            });
            ModelliBatteria.Add(new mbTipoBatteria() { BatteryTypeId = 0x1001, SortOrder = 1, BatteryType = "Batteria standard Pb/Lead", DivisoreCelle = 200, StandardChargeProfile = 0x04, OldBatteryTypeId = 0x71, TensioniFisse = 1});
            ModelliBatteria.Add(new mbTipoBatteria() { BatteryTypeId = 0x2001, SortOrder = 2, BatteryType = "Batteria standard GEL/AGM", DivisoreCelle = 200, StandardChargeProfile = 0x02, OldBatteryTypeId = 0x72, TensioniFisse = 1 });
            ModelliBatteria.Add(new mbTipoBatteria() { BatteryTypeId = 0x3001, SortOrder = 3, BatteryType = "Batteria standard Litio", DivisoreCelle = 0, StandardChargeProfile = 0x07, OldBatteryTypeId = 0x00, TensioniFisse = 0 });
            ModelliBatteria.Add(new mbTipoBatteria() { BatteryTypeId = 0x4001, SortOrder = 4, BatteryType = "Super Capacitor", DivisoreCelle = 0, StandardChargeProfile = 0x10, OldBatteryTypeId = 0x00, TensioniFisse = 0 });

            return true;
        }

        public bool inizializzaParametriCarica()
        {
            // Regole composizione formula:
            //     se il valore è "" il parametro è disabilitato 
            //
            // 1 ) 1^cifra: Operazione
            //     = Assegnazione -> valose fisso da assegnare o calcolo su parametro - Sbloccabile
            //     ~ come = ma già sbloccato  
            //
            // 2 ) 2^cifra: Variabile
            //       # -> Valore assoluto -> nessun calcolo
            //       C -> Capacità nominale
            //       V -> Tensione nominale
            //       
            // 3 ) 3^cifra: Operazione
            //       # -> Valore assoluto -> nessun calcolo
            //       @ -> Valore assoluto -> nessun calcolo
            //       * -> Moltiplicazione per il valore indicato
            //       / -> Divisione per il valore indicato
            //       % -> Divisione per il valore indicato diviso 10
            //       es C%65 -> C/6.5
            //       | -> Divisione per il valore indicato diviso 100
            //       es C|65 -> C/0.65




            ParametriCarica = new List<_mbProfiloTipoBatt>() ;

            #region "Ciclo IWa 13 - Pb/Lead"
            ParametriCarica.Add(
                new _mbProfiloTipoBatt()
                {
                    IdProfiloCaricaLL = 0x04,
                    BatteryTypeId = 0x1001,
                    // tempi
                    TempoT0Max = "=##60",
                    TempoT1Max = "=##780",
                    TempoT2Min = "=##060",
                    TempoT2Max = "=##210",
                    TempoT3Max = "",
                    FattoreK = "=##060",
                    DurataNominale = "=##780",
                    // Tensioni
                    TensionePrecicloV0 = "=##190",
                    TensioneSogliaVs = "=##240",
                    TensioneRaccordoVr = "=##245",
                    TensioneMassimaVMax = "=##265",
                    TensioneLimiteVLim = "=##278",
                    TensRiconoscimentoMin = "=##150",
                    TensRiconoscimentoMax = "=##240",
                    TensMinimaStop = "=##180",

                    //Correnti
                    CorrenteI0 = "=C/24",
                    CorrenteI1 = "=C/12",
                    CorrenteFinaleI2 = "=C/24",
                    CorrenteMassima = "=C/5",
                    CorrenteI3 = "",
                    CorrenteRaccordoIr = "=C/13",
                   
                    EqualAttivabile = "~##1",
                    EqualNumImpulsi = "=##12",
                    EqualTempoAttesa = "=##480",
                    EqualTempoImpulso = "=##5",
                    EqualTempoPausa = "=##25",
                    EqualCorrenteImpulso = "=C/24",

                    MantAttivabile = "~##1",
                    MantTempoAttesa = "=##360",
                    MantTensIniziale = "=##210",
                    MantTensFinale = "=##230",
                    MantTempoMaxErogazione = "=##15",
                    MantCorrenteImpulso = "=C/24",

                    OpportunityOraInizio = "=##420",   // 13  ore --> fine alle 18, inizio alle 7
                    OpportunityOraFine = "~##1140",
                    OpportunityDurataMax = "=##240",
                    OpportunityCorrente = "=C/4",
                    OpportunityTensioneMax = "=##240",
                    OpportunityAttivabile = "~##1",
                    TemperaturaLimite = "=##48",

                    AbilitaSpyBatt = "~##1",
                    AbilitaSafety = "~##0",

                } );
            #endregion "Ciclo IWa 13"

            #region "Ciclo IWa 11 - Pb/Lead"
            ParametriCarica.Add(
                new _mbProfiloTipoBatt()
                {
                    IdProfiloCaricaLL = 0x05,
                    BatteryTypeId = 0x1001,
                    // tempi
                    TempoT0Max = "=##060",
                    TempoT1Max = "=##660",
                    TempoT2Min = "=##60",
                    TempoT2Max = "=##210",
                    TempoT3Max = "",
                    FattoreK = "=##60",
                    DurataNominale = "=##660",
                    // Tensioni
                    TensionePrecicloV0 = "=##190",
                    TensioneSogliaVs = "=##240",
                    TensioneRaccordoVr = "=##245",
                    TensioneMassimaVMax = "=##265",
                    TensioneLimiteVLim = "=##278",
                    TensRiconoscimentoMin = "=##150",
                    TensRiconoscimentoMax = "=##240",
                    TensMinimaStop = "=##180",
                    //Correnti
                    CorrenteI0 = "=C/20",
                    CorrenteI1 = "=C/10",
                    CorrenteFinaleI2 = "=C/24",
                    CorrenteMassima = "=C/5",
                    CorrenteI3 = "",
                    CorrenteRaccordoIr = "=C/13",

                    EqualAttivabile = "~##1",
                    EqualNumImpulsi = "=##12",
                    EqualTempoAttesa = "=##480",
                    EqualTempoImpulso = "=##5",
                    EqualTempoPausa = "=##25",
                    EqualCorrenteImpulso = "=C/24",

                    MantAttivabile = "~##1",
                    MantTempoAttesa = "=##360",
                    MantTensIniziale = "=##210",
                    MantTensFinale = "=##230",
                    MantTempoMaxErogazione = "=##15",
                    MantCorrenteImpulso = "=C/24",

                    OpportunityOraInizio = "=##300",   // 11  ore --> fine alle 18, inizio alle 5
                    OpportunityOraFine = "~##1140",
                    OpportunityDurataMax = "=##240",
                    OpportunityCorrente = "=C/4",
                    OpportunityTensioneMax = "=##240",
                    OpportunityAttivabile = "~##1",
                    TemperaturaLimite = "=##48",

                    AbilitaSpyBatt = "~##1",
                    AbilitaSafety = "~##0",


                });
            #endregion "Ciclo IWa 11"

            #region "Ciclo IWa 8 - Pb/Lead"
            ParametriCarica.Add(
                new _mbProfiloTipoBatt()
                {
                    IdProfiloCaricaLL = 0x06,
                    BatteryTypeId = 0x1001,
                    // tempi
                    TempoT0Max = "=##060",
                    TempoT1Max = "=##480",
                    TempoT2Min = "=##60",
                    TempoT2Max = "=##210",
                    TempoT3Max = "",
                    FattoreK = "=##120",
                    DurataNominale = "=##480",

                    // Tensioni
                    TensionePrecicloV0 = "=##190",
                    TensioneSogliaVs = "=##240",
                    TensioneRaccordoVr = "=##245",
                    TensioneMassimaVMax = "=##265",
                    TensioneLimiteVLim = "=##278",
                    TensRiconoscimentoMin = "=##150",
                    TensRiconoscimentoMax = "=##240",
                    TensMinimaStop = "=##180",

                    //Correnti
                    CorrenteI0 = "=C/12",
                    CorrenteI1 = "=C%65",
                    CorrenteFinaleI2 = "=C/24",
                    CorrenteMassima = "=C/5",
                    CorrenteI3 = "",
                    CorrenteRaccordoIr = "=C/13",

                    EqualAttivabile = "~##1",
                    EqualNumImpulsi = "=##12",
                    EqualTempoAttesa = "=##480",
                    EqualTempoImpulso = "=##5",
                    EqualTempoPausa = "=##25",
                    EqualCorrenteImpulso = "=C/24",

                    MantAttivabile = "~##1",
                    MantTempoAttesa = "=##360",
                    MantTensIniziale = "=##210",
                    MantTensFinale = "=##230",
                    MantTempoMaxErogazione = "=##15",
                    MantCorrenteImpulso = "=C/24",

                    OpportunityOraInizio = "=##240",   // 8  ore --> fine alle 20, inizio alle 4
                    OpportunityOraFine = "~##1200",
                    OpportunityDurataMax = "=##240",
                    OpportunityCorrente = "=C/4",
                    OpportunityTensioneMax = "=##240",
                    OpportunityAttivabile = "~##1",
                    TemperaturaLimite = "=##48",

                    AbilitaSpyBatt = "~##1",
                    AbilitaSafety = "~##0",

                });
            #endregion "Ciclo IWa 8"

            #region "Ciclo IU - GEL"
            ParametriCarica.Add(
                new _mbProfiloTipoBatt()
                {
                    IdProfiloCaricaLL = 0x02,
                    BatteryTypeId = 0x2001,
                    // tempi
                    TempoT0Max = "###060",
                    TempoT1Max = "=##600",
                    TempoT2Min = "=##120",
                    TempoT2Max = "=##300",
                    TempoT3Max = "",
                    FattoreK = "=##100",
                    DurataNominale = "=##600",
                    EqualTempoAttesa = "",
                    EqualTempoImpulso = "",
                    EqualTempoPausa = "",
                    MantTempoAttesa = "=##360",
                    MantTempoMaxErogazione = "=##15",
                    // Tensioni
                    TensionePrecicloV0 = "###190",
                    TensioneSogliaVs = "=##240",
                    TensioneRaccordoVr = "",
                    TensioneMassimaVMax = "",
                    TensioneLimiteVLim = "",
                    MantTensIniziale = "=##210",
                    MantTensFinale = "=##230",
                    TensRiconoscimentoMin = "=##150",
                    TensRiconoscimentoMax = "=##240",
                    TensMinimaStop = "=##180",
                    //Correnti
                    CorrenteI0 = "#C/20",
                    CorrenteI1 = "=C/10",
                    CorrenteFinaleI2 = "==##0",
                    CorrenteMassima = "=C/5",
                    CorrenteI3 = "",
                    CorrenteRaccordoIr = "",
                    MantCorrenteImpulso = "=C/50",
                    EqualCorrenteImpulso = "",

                    OpportunityOraInizio = "=##240",   // 10  ore --> fine alle 18, inizio alle 4
                    OpportunityOraFine = "~##1140",
                    OpportunityDurataMax = "=##240",
                    OpportunityCorrente = "=C/4",
                    OpportunityTensioneMax = "=##240",
                    OpportunityAttivabile = "~##1",


                    MantAttivabile = "=##1",

                    EqualNumImpulsi = "",

                    AbilitaSpyBatt = "~##0",
                    AbilitaSafety = "~##0",


                });
            #endregion "Ciclo IU"

            #region "Ciclo IUIa - Pb/Lead"
            ParametriCarica.Add(
                new _mbProfiloTipoBatt()
                {
                    IdProfiloCaricaLL = 0x03,
                    BatteryTypeId = 0x1001,
                    // tempi
                    TempoT0Max = "###060",
                    TempoT1Max = "=##600",
                    TempoT2Min = "=##0",
                    TempoT2Max = "=##300",
                    TempoT3Max = "=##240",
                    FattoreK = "=##100",
                    DurataNominale = "=##600",
                    EqualTempoAttesa = "=##480",
                    EqualTempoImpulso = "=##5",
                    EqualTempoPausa = "=##25",
                    MantTempoAttesa = "=##360",
                    MantTempoMaxErogazione = "=##15",
                    // Tensioni
                    TensionePrecicloV0 = "###190",
                    TensioneSogliaVs = "=##240",
                    TensioneRaccordoVr = "",
                    TensioneMassimaVMax = "=##265",
                    TensioneLimiteVLim = "=##300",
                    MantTensIniziale = "=##210",
                    MantTensFinale = "=##230",
                    TensRiconoscimentoMin = "=##150",
                    TensRiconoscimentoMax = "=##240",
                    TensMinimaStop = "=##180",
                    //Correnti
                    CorrenteI0 = "#C/20",
                    CorrenteI1 = "=C/10",
                    CorrenteFinaleI2 = "=C/24",
                    CorrenteMassima = "=C/5",
                    CorrenteI3 = "",
                    CorrenteRaccordoIr = "",
                    MantCorrenteImpulso = "=C/50",
                    EqualCorrenteImpulso = "=C/24",

                    EqualNumImpulsi = "=##10",
                    EqualAttivabile = "=##1",
                    MantAttivabile = "=##1",

                    OpportunityOraInizio = "=##300",   // 11  ore --> fine alle 18, inizio alle 5
                    OpportunityOraFine = "~##1140",
                    OpportunityDurataMax = "=##240",
                    OpportunityCorrente = "=C/4",
                    OpportunityTensioneMax = "=##240",
                    OpportunityAttivabile = "~##1",


                    AbilitaSpyBatt = "~##0",
                    AbilitaSafety = "~##0",


                });
            #endregion "Ciclo IUIa - Pb/Lead"

            #region "Ciclo IUIa - GEL"
            ParametriCarica.Add(
                new _mbProfiloTipoBatt()
                {
                    IdProfiloCaricaLL = 0x03,
                    BatteryTypeId = 0x2001,
                    // tempi
                    TempoT0Max = "###060",
                    TempoT1Max = "=##600",
                    TempoT2Min = "=##0",
                    TempoT2Max = "=##300",
                    TempoT3Max = "=##240",
                    FattoreK = "=##100",
                    DurataNominale = "=##600",
                    EqualTempoAttesa = "",
                    EqualTempoImpulso = "",
                    EqualTempoPausa = "",
                    MantTempoAttesa = "=##360",
                    MantTempoMaxErogazione = "=##15",
                    // Tensioni
                    TensionePrecicloV0 = "###190",
                    TensioneSogliaVs = "=##240",
                    TensioneRaccordoVr = "",
                    TensioneMassimaVMax = "=##265",
                    TensioneLimiteVLim = "",
                    TensRiconoscimentoMin = "=##150",
                    TensRiconoscimentoMax = "=##240",
                    TensMinimaStop = "=##180",
                    MantTensIniziale = "=##210",
                    MantTensFinale = "=##230",
                    //Correnti
                    CorrenteI0 = "=C/20",
                    CorrenteI1 = "=C/10",
                    CorrenteFinaleI2 = "=C/24",
                    CorrenteMassima = "=C/5",
                    CorrenteI3 = "",
                    CorrenteRaccordoIr = "",
                    MantCorrenteImpulso = "=C/50",
                    EqualCorrenteImpulso = "",

                    EqualNumImpulsi = "",
                    EqualAttivabile = "",
                    MantAttivabile = "",

                    OpportunityOraInizio = "=##300",   // 11  ore --> fine alle 18, inizio alle 5
                    OpportunityOraFine = "~##1140",
                    OpportunityDurataMax = "=##240",
                    OpportunityCorrente = "=C/4",
                    OpportunityTensioneMax = "=##240",
                    OpportunityAttivabile = "~##1",

                    AbilitaSpyBatt = "~##0",
                    AbilitaSafety = "~##0",


                });
            #endregion "Ciclo IUIa - Pb/Lead"

            #region "Ciclo LITIO - Litio"
            ParametriCarica.Add(
                new _mbProfiloTipoBatt()
                {
                    IdProfiloCaricaLL = 0x07,
                    BatteryTypeId = 0x3001,
                    // tempi
                    TempoT0Max = "",
                    TempoT1Max = "=##720",
                    TempoT2Min = "",
                    TempoT2Max = "=##360",
                    TempoT3Max = "",
                    FattoreK = "",
                    FattoreKs = "=##110",
                    DurataNominale = "=##600",
                    // parametri EQUAL usati per riarmo BMS
                    EqualAttivabile = "~##1",
                    EqualTempoAttesa = "=##10",
                    EqualTempoImpulso = "",
                    EqualTempoPausa = "=##10",
                    EqualNumImpulsi = "=##5",
                    // --------------------------------------
                    MantTempoAttesa = "",
                    MantTempoMaxErogazione = "",
                    // Tensioni
                    TensionePrecicloV0 = "",
                    TensioneSogliaVs = "=##000",
                    TensioneRaccordoVr = "",
                    TensioneMassimaVMax = "",
                    TensioneLimiteVLim = "",
                    MantTensIniziale = "",
                    MantTensFinale = "",
                    TensRiconoscimentoMin = "=V|133",
                    TensRiconoscimentoMax = "=V*2",
                    TensMinimaStop = "",
                    //Correnti
                    CorrenteI0 = "",
                    CorrenteI1 = "=C/2",
                    CorrenteFinaleI2 = "=C/20",
                    CorrenteMassima = "=C/1",
                    CorrenteI3 = "",
                    CorrenteRaccordoIr = "",
                    MantCorrenteImpulso = "",
                    EqualCorrenteImpulso = "=C#050",

                    OpportunityOraInizio = "",   // 10  ore --> fine alle 18, inizio alle 4
                    OpportunityOraFine = "",
                    OpportunityDurataMax = "",
                    OpportunityCorrente = "",
                    OpportunityTensioneMax = "",
                    OpportunityAttivabile = " ##0",


                    MantAttivabile = " ##0",


                    AbilitaSpyBatt = "~##0",
                    AbilitaSafety = "~##1",


                });
            #endregion "Ciclo LITIO"

            #region "Ciclo SuperCAP - SuperCapacitor"
            ParametriCarica.Add(
                new _mbProfiloTipoBatt()
                {
                    IdProfiloCaricaLL = 0x10,
                    BatteryTypeId = 0x4001,
                    // tempi
                    TempoT0Max = "",
                    TempoT1Max = "",
                    TempoT2Min = "",
                    TempoT2Max = "",
                    TempoT3Max = "",
                    FattoreK = "",
                    FattoreKs = "=##110",
                    DurataNominale = "=##600",
                    // parametri EQUAL usati per riarmo BMS
                    EqualAttivabile = "~##1",
                    EqualTempoAttesa = "=##10",
                    EqualTempoImpulso = "",
                    EqualTempoPausa = "=##10",
                    EqualNumImpulsi = "=##5",
                    // --------------------------------------
                    MantTempoAttesa = "",
                    MantTempoMaxErogazione = "",
                    // Tensioni
                    TensionePrecicloV0 = "",
                    TensioneSogliaVs = "=##000",
                    TensioneRaccordoVr = "",
                    TensioneMassimaVMax = "",
                    TensioneLimiteVLim = "",
                    MantTensIniziale = "",
                    MantTensFinale = "",
                    TensRiconoscimentoMin = "",
                    TensRiconoscimentoMax = "",
                    TensMinimaStop = "",
                    //Correnti
                    CorrenteI0 = "",
                    CorrenteI1 = "=C/2",
                    CorrenteFinaleI2 = "=C/20",
                    CorrenteMassima = "=C/1",
                    CorrenteI3 = "",
                    CorrenteRaccordoIr = "",
                    MantCorrenteImpulso = "",
                    EqualCorrenteImpulso = "=C#050",

                    OpportunityOraInizio = "",   // 10  ore --> fine alle 18, inizio alle 4
                    OpportunityOraFine = "",
                    OpportunityDurataMax = "",
                    OpportunityCorrente = "",
                    OpportunityTensioneMax = "",
                    OpportunityAttivabile = " ##0",


                    MantAttivabile = " ##0",


                    AbilitaSpyBatt = "~##0",
                    AbilitaSafety = "~##1",


                });
            #endregion "Ciclo SuperCAP"

            #region "Ciclo QUASAR Std - Pb/Lead"
            ParametriCarica.Add(
                new _mbProfiloTipoBatt()
                {
                    IdProfiloCaricaLL = 0x0F,
                    BatteryTypeId = 0x1001,
                    // tempi
                    TempoT0Max = "=##015",
                    TempoT1Max = "=##480",
                    TempoT2Min = "=##60",
                    TempoT2Max = "=##210",
                    TempoT3Max = "",
                    FattoreK = "",
                    DurataNominale = "=##480",
                    TempoFinale = "=##0",
                    TempodT = "=##20",

                    // Tensioni
                    TensionePrecicloV0 = "=##180",
                    TensioneSogliaVs = "=##237",
                    TensioneRaccordoVr = "=##240",
                    TensioneMassimaVMax = "=##270",
                    TensioneLimiteVLim = "=##278",
                    TensRiconoscimentoMin = "=##150",
                    TensRiconoscimentoMax = "=##240",
                    TensMinimaStop = "=##180",
                    TensionedV = "=##020",

                    //Correnti
                    CorrenteI0 = "=C/33",
                    CorrenteI1 = "=C|550",
                    CorrenteFinaleI2 = "=C/20",
                    CorrenteMassima = "=C|250",
                    CorrenteI3 = "",
                    CorrenteRaccordoIr = "=C/08",

                    EqualAttivabile = "~##0",
                    EqualNumImpulsi = "=##12",
                    EqualTempoAttesa = "=##480",
                    EqualTempoImpulso = "=##5",
                    EqualTempoPausa = "=##25",
                    EqualCorrenteImpulso = "=C/20",

                    MantAttivabile = "~##1",
                    MantTempoAttesa = "=##360",
                    MantTensIniziale = "=##210",
                    MantTensFinale = "=##230",
                    MantTempoMaxErogazione = "=##10",
                    MantCorrenteImpulso = "=C/20",

                    OpportunityOraInizio = "=##240",   // 8  ore --> fine alle 20, inizio alle 4
                    OpportunityOraFine = "~##1200",
                    OpportunityDurataMax = "=##240",
                    OpportunityCorrente = "=C/4",
                    OpportunityTensioneMax = "=##240",
                    OpportunityAttivabile = "~##0",
                    TemperaturaLimite = "=##48",
                    AbilitaSpyBatt = "~##1",
                    AbilitaSafety = "~##0",

                });
            #endregion "Ciclo QUASAR Std"

            #region "Ciclo QUASAR Fast - Pb/Lead"
            ParametriCarica.Add(
                new _mbProfiloTipoBatt()
                {
                    IdProfiloCaricaLL = 0x10,
                    BatteryTypeId = 0x1001,
                    // tempi
                    TempoT0Max = "=##015",
                    TempoT1Max = "=##240",
                    TempoT2Min = "=##60",
                    TempoT2Max = "=##210",
                    TempoT3Max = "",
                    FattoreK = "",
                    DurataNominale = "=##480",
                    TempoFinale = "=##0",
                    TempodT = "=##20",

                    // Tensioni
                    TensionePrecicloV0 = "=##180",
                    TensioneSogliaVs = "=##237",
                    TensioneRaccordoVr = "=##240",
                    TensioneMassimaVMax = "=##270",
                    TensioneLimiteVLim = "=##278",
                    TensRiconoscimentoMin = "=##150",
                    TensRiconoscimentoMax = "=##240",
                    TensMinimaStop = "=##180",
                    TensionedV = "=##020",

                    //Correnti
                    CorrenteI0 = "=C/33",
                    CorrenteI1 = "=C|285",
                    CorrenteFinaleI2 = "=C/20",
                    CorrenteMassima = "=C|250",
                    CorrenteI3 = "",
                    CorrenteRaccordoIr = "=C/08",

                    EqualAttivabile = "~##0",
                    EqualNumImpulsi = "=##12",
                    EqualTempoAttesa = "=##480",
                    EqualTempoImpulso = "=##5",
                    EqualTempoPausa = "=##25",
                    EqualCorrenteImpulso = "=C/20",

                    MantAttivabile = "~##1",
                    MantTempoAttesa = "=##360",
                    MantTensIniziale = "=##210",
                    MantTensFinale = "=##230",
                    MantTempoMaxErogazione = "=##10",
                    MantCorrenteImpulso = "=C/20",

                    OpportunityOraInizio = "=##240",   // 8  ore --> fine alle 20, inizio alle 4
                    OpportunityOraFine = "~##1200",
                    OpportunityDurataMax = "=##240",
                    OpportunityCorrente = "=C/4",
                    OpportunityTensioneMax = "=##240",
                    OpportunityAttivabile = "~##0",
                    TemperaturaLimite = "=##48",
                    AbilitaSpyBatt = "~##1",
                    AbilitaSafety = "~##0",

                });
            #endregion "Ciclo QUASAR Fast"

            return true;
        }

         public bool inizializzaDurate()
        {
            DurateCarica = new List<llDurataCarica>();
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 780, Descrizione = "13:00", Ordine = 0,  ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 60,  Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 720, Descrizione = "12:00", Ordine = 1,  ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 60,  Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 660, Descrizione = "11:00", Ordine = 2,  ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 100, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 600, Descrizione = "10:00", Ordine = 3,  ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 100, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 540, Descrizione = " 9:00", Ordine = 4,  ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 100, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 480, Descrizione = " 8:00", Ordine = 5,  ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 120, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 420, Descrizione = " 7:00", Ordine = 6,  ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 120, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 360, Descrizione = " 6:00", Ordine = 7,  ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 120, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 300, Descrizione = " 5:00", Ordine = 8,  ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 120, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 240, Descrizione = " 4:00", Ordine = 9,  ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 120, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 180, Descrizione = " 3:00", Ordine = 10, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 140, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 120, Descrizione = " 2:00", Ordine = 11, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 140, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 60,  Descrizione = " 1:00", Ordine = 12, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 140, Attivo = 1 });
            return true;
        }

        public bool inizializzaTensioni()
        {
            TensioniBatteria = new List<llTensioneBatteria>();
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 2400, Descrizione = "24,00", Attivo = 1, Ordine = 0 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 3600, Descrizione = "36,00", Attivo = 1, Ordine = 1 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 4800, Descrizione = "48,00", Attivo = 1, Ordine = 2 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 7200, Descrizione = "72,00", Attivo = 1, Ordine = 3 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 8000, Descrizione = "80,00", Attivo = 1, Ordine = 4 });
            return true;
        }

        public bool inizializzaDurateProfilo()
        {
            DurateProfilo = new List<llDurataProfilo>();

            //SuperCAP
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 720, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 600, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 540, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 420, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 360, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 300, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 240, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 180, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 120, IdProfiloCaricaLL = 0x10, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL =  60, IdProfiloCaricaLL = 0x10, Attivo = 1 });

            //Litio
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 720, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 600, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 540, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 420, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 360, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 300, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 240, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 180, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 120, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 60,  IdProfiloCaricaLL = 0x07, Attivo = 1 });
            //Gel - IU
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 600, IdProfiloCaricaLL = 0x02, Attivo = 1 });

            //Piombo IWa generico
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x01, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x01, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x01, Attivo = 1 });

            //Piombo IWa 13
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x04, Attivo = 1 });

            //Piombo IWa 11
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x05, Attivo = 1 });

            //Piombo IWa 8
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x06, Attivo = 1 });

            //Piombo IWa 13
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x08, Attivo = 1 });

            //Piombo IWa 11
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x09, Attivo = 1 });

            //Piombo IWa 8
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x0A, Attivo = 1 });

            //Piombo Quasar Std (IWa 8)
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x11, Attivo = 1 });

            //Piombo Quasar Fast ( 4h )
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 240, IdProfiloCaricaLL = 0x12, Attivo = 1 });


            //Litio BMS
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 720, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 600, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 540, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 420, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 360, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 300, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 240, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 180, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 120, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 60,  IdProfiloCaricaLL = 0x0B, Attivo = 1 });

            //Litio CAN
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 720, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 600, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 540, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 420, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 360, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 300, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 240, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 180, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 120, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 60,  IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            return true;
        }

        public bool inizializzaBatteriaProfilo()
        {

            ProfiloTipoBatt = new List<_llProfiloTipoBatt>();

            // Piombo
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x01, Attivo = 0 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x04, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x05, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x06, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x08, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x09, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x0A, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x11, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x12, Attivo = 1 });

            // Gel
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x72, IdProfiloCaricaLL = 0x02, Attivo = 1 });

            // Litio
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x73, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x73, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x73, IdProfiloCaricaLL = 0x0C, Attivo = 1 });

            // SuperCap
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x73, IdProfiloCaricaLL = 0x10, Attivo = 1 });

            return true;
        }

        public bool inizializzaTensioniModello()
        {

            TensioniModello = new List<llTensioniModello>();
            // MAX VALUE 0xEE
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0xEE, IdTensione = 2400, TxTensione = "24,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0xEE, IdTensione = 3600, TxTensione = "36,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0xEE, IdTensione = 4800, TxTensione = "48,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0xEE, IdTensione = 7200, TxTensione = "72,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0xEE, IdTensione = 8000, TxTensione = "80,00", Attivo = 1 });


            // 0x88, NomeModello = "LLT.3.x 24-80V / 120A SCHG"
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x88, IdTensione = 2400, TxTensione = "24,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x88, IdTensione = 3600, TxTensione = "36,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x88, IdTensione = 4800, TxTensione = "48,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x88, IdTensione = 7200, TxTensione = "72,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x88, IdTensione = 8000, TxTensione = "80,00", Attivo = 1 });


            // 24/80 - 120 Trifase
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 2400, TxTensione = "24,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 3600, TxTensione = "36,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 4800, TxTensione = "48,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 7200, TxTensione = "72,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 8000, TxTensione = "80,00", Attivo = 1 });

            // 24/48 - 200 Trifase
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x82, IdTensione = 2400, TxTensione = "24,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x82, IdTensione = 3600, TxTensione = "36,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x82, IdTensione = 4800, TxTensione = "48,00", Attivo = 1 });

            // 24 - 70 Trifase
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x01, IdTensione = 2400, TxTensione = "24,00", Attivo = 1 });

            return true;
        }



    }
}
