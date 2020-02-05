
using log4net;
using MoriData;
using System;
using System.Collections.Generic;
using System.Linq;
using Utility;

namespace ChargerLogic
{
    public class ModelloCiclo
    {
        public ParametriCiclo ValoriCiclo;
        public ParametriCiclo ParametriAttivi;
        public CicloDiCarica DatiModello;
        public List<ParametroLL> ListaParametri;
        public int NumParametriAttivi = 0;

        public _mbProfiloTipoBatt ModelloProfilo { get; set; }
        public mbTipoBatteria Batteria;
        public _mbProfiloCarica Profilo;
        public ushort Tensione { get; set; }
        public ushort NumeroCelle { get; set; }
        public ushort Capacita { get; set; }
        public string NomeProfilo { get; set; }
        public ushort IdProgramma { get; set; }
        public bool DatiSalvati { get; set; }
        public bool RichiestoNuovoId { get; set; }
        public ushort DurataMaxCarica { get; set; }

        public llProgrammaCarica ProfiloRegistrato;



        private static ILog Log = LogManager.GetLogger("ModelloCiclo");


        public ModelloCiclo()
        {
            DatiModello = new CicloDiCarica();
            DatiModello.InizializzaDatiLocali();
            ValoriCiclo = new ParametriCiclo();
            ParametriAttivi = new ParametriCiclo();
            ListaParametri = new List<ParametroLL>();
            ProfiloRegistrato = new llProgrammaCarica();
            RichiestoNuovoId = false;
            DatiSalvati = true;
            NumParametriAttivi = 0;
            ModelloProfilo = null;
        }

        public bool GeneraProgrammaCarica()
        {
            try
            {
                ProfiloRegistrato = new llProgrammaCarica();
                ///// ProfiloRegistrato.IdProfilo = IdProfilo; // tipo dati ????
                // ProfiloRegistrato.IdProgramma = idp
                ProfiloRegistrato.TipoBatteria = Batteria.BatteryTypeId;
                ProfiloRegistrato.IdProgramma = IdProgramma;
                ProfiloRegistrato.ProgramName = NomeProfilo;

                ProfiloRegistrato.BatteryVdef = Tensione;
                ProfiloRegistrato.BatteryAhdef = Capacita;
                ProfiloRegistrato.IdProfilo = Profilo.IdProfiloCaricaLL;

                GeneraListaValori();

                ProfiloRegistrato.ListaParametri = ListaParametri;



                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("GeneraProgrammaCarica: " + Ex.Message );
                return false;
            }
        }


        public bool EstraiDaProgrammaCarica()
        {
            try
            {
                if (ProfiloRegistrato == null)
                {
                    // non ho un profilo attivo. esco
                    return false;
                }

                IdProgramma = ProfiloRegistrato.IdProgramma;
                NomeProfilo = ProfiloRegistrato.ProgramName;

                ushort StatoCella = 4;

                Tensione = ProfiloRegistrato.BatteryVdef;
                Capacita = ProfiloRegistrato.BatteryAhdef;
                int numeroValori = 0;
                ListaParametri = ProfiloRegistrato.ListaParametri;
                foreach( ParametroLL dato in ListaParametri)
                {
                    switch((SerialMessage.ParametroLadeLight)dato.idParametro)
                    {
                        case SerialMessage.ParametroLadeLight.TipoBatteria:
                            Batteria = null;
                            mbTipoBatteria TempBatt = (from tb in (List<mbTipoBatteria>)DatiModello.ModelliBatteria
                                                       where tb.BatteryTypeId == dato.ValoreParametro
                                                       select tb).FirstOrDefault();
                            Batteria = TempBatt;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.TipoProfilo:
                            Profilo = null;
                            _mbProfiloCarica TempData = (from td in (List<_mbProfiloCarica>)DatiModello.ProfiliCarica
                                                       where td.IdProfiloCaricaLL == dato.ValoreParametro
                                                       select td).FirstOrDefault();
                            Profilo = TempData;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.NumeroCelle:
                            NumeroCelle = dato.ValoreParametro;
                            numeroValori += 1;
                            break;


                        case SerialMessage.ParametroLadeLight.TipoCassone:
                            ValoriCiclo.TipoCassone = dato.ValoreParametro;
                            ParametriAttivi.TipoCassone = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.ModelloLadeLight:
                            ValoriCiclo.IdModelloLL = dato.ValoreParametro;
                            numeroValori += 1;
                            break;


                        case SerialMessage.ParametroLadeLight.DivisoreK:
                            ValoriCiclo.AbilitaSpyBatt = dato.ValoreParametro;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.Safety:
                            ValoriCiclo.AbilitaSafety = dato.ValoreParametro;
                            numeroValori += 1;
                            break;

                        // Fasi
                        #region "Fasi"
                        // Preciclo
                        case SerialMessage.ParametroLadeLight.TensionePrecicloV0:
                            ValoriCiclo.TensionePrecicloV0 = dato.ValoreParametro;
                            ParametriAttivi.TensionePrecicloV0 = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.CorrentePrecicloI0:
                            ValoriCiclo.CorrenteI0 = dato.ValoreParametro;
                            ParametriAttivi.CorrenteI0 = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.TempoT0Max:
                            ValoriCiclo.TempoT0Max = dato.ValoreParametro;
                            ParametriAttivi.TempoT0Max = StatoCella;
                            numeroValori += 1;
                            break;

                            // Fase 1
                        case SerialMessage.ParametroLadeLight.CorrenteCaricaI1:
                            ValoriCiclo.CorrenteI1 = dato.ValoreParametro;
                            ParametriAttivi.CorrenteI1 = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.TempoT1Max:
                            ValoriCiclo.TempoT1Max = dato.ValoreParametro;
                            ParametriAttivi.TempoT1Max = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.TensioneSogliaF1:
                            ValoriCiclo.TensioneSogliaVs = dato.ValoreParametro;
                            ParametriAttivi.TensioneSogliaVs = StatoCella;
                            numeroValori += 1;
                            break;

                            // Fase 2
                        case SerialMessage.ParametroLadeLight.TensioneRaccordoF1:
                            ValoriCiclo.TensioneRaccordoVr = dato.ValoreParametro;
                            ParametriAttivi.TensioneRaccordoVr = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.CorrenteRaccordo:
                            ValoriCiclo.CorrenteRaccordoIr = dato.ValoreParametro;
                            ParametriAttivi.CorrenteRaccordoIr = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.CorrenteFinaleF2:
                            ValoriCiclo.CorrenteFinaleI2 = dato.ValoreParametro;
                            ParametriAttivi.CorrenteFinaleI2 = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.TensioneMassimaCella:
                            ValoriCiclo.TensioneMassimaVMax = dato.ValoreParametro;
                            ParametriAttivi.TensioneMassimaVMax = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.TempoT2Min:
                            ValoriCiclo.TempoT2Min = dato.ValoreParametro;
                            ParametriAttivi.TempoT2Min = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.TempoT2Max:
                            ValoriCiclo.TempoT2Max = dato.ValoreParametro;
                            ParametriAttivi.TempoT2Max = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.CoeffK:
                            ValoriCiclo.FattoreK = dato.ValoreParametro;
                            ParametriAttivi.FattoreK = StatoCella;
                            numeroValori += 1;
                            break;

                            // Fase 3

                        case SerialMessage.ParametroLadeLight.TempoT3Max:
                            ValoriCiclo.TempoT3Max = dato.ValoreParametro;
                            ParametriAttivi.TempoT3Max = StatoCella;
                            numeroValori += 1;
                            break;
                        #endregion "Fasi"

                        // Equal
                        #region "Equal"
                        case SerialMessage.ParametroLadeLight.EqualAttivo:
                            ValoriCiclo.EqualAttivabile = dato.ValoreParametro;
                            ParametriAttivi.EqualAttivabile = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.EqualFineCaricaAttesa:
                            ValoriCiclo.EqualTempoAttesa = dato.ValoreParametro;
                            ParametriAttivi.EqualTempoAttesa = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.EqualFineCaricaNumImpulsi:
                            ValoriCiclo.EqualNumImpulsi = dato.ValoreParametro;
                            ParametriAttivi.EqualNumImpulsi = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.EqualFineCaricaDurataP:
                            ValoriCiclo.EqualTempoPausa = dato.ValoreParametro;
                            ParametriAttivi.EqualTempoPausa = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.EqualFineCaricaDurataI:
                            ValoriCiclo.EqualTempoImpulso = dato.ValoreParametro;
                            ParametriAttivi.EqualTempoImpulso = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.EqualFineCaricaCorrenteImp:
                            ValoriCiclo.EqualCorrenteImpulso = dato.ValoreParametro;
                            ParametriAttivi.EqualCorrenteImpulso = StatoCella;
                            numeroValori += 1;
                            break;

                        #endregion "Equal"

                        // Mantenimento
                        #region "Mantenimento"
                        case SerialMessage.ParametroLadeLight.MantenimentoAttivo:
                            ValoriCiclo.MantAttivabile = dato.ValoreParametro;
                            ParametriAttivi.MantAttivabile = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.MantenimentoAttesa:
                            ValoriCiclo.MantTempoAttesa = dato.ValoreParametro;
                            ParametriAttivi.MantTempoAttesa = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.MantenimentoTensIniziale:
                            ValoriCiclo.MantTensIniziale = dato.ValoreParametro;
                            ParametriAttivi.MantTensIniziale = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.MantenimentoTensFinale:
                            ValoriCiclo.MantTensFinale = dato.ValoreParametro;
                            ParametriAttivi.MantTensFinale = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.MantenimentoTMaxErogazione:
                            ValoriCiclo.MantTempoMaxErogazione = dato.ValoreParametro;
                            ParametriAttivi.MantTempoMaxErogazione = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.MantenimentoCorrErogazione:
                            ValoriCiclo.MantCorrenteImpulso = dato.ValoreParametro;
                            ParametriAttivi.MantCorrenteImpulso = StatoCella;
                            numeroValori += 1;
                            break;

                        #endregion "Mantenimento"

                        // Opportunity
                        #region "Opportunity Charge"
                        case SerialMessage.ParametroLadeLight.OpportunityAttivo:
                            ValoriCiclo.OpportunityAttivabile = dato.ValoreParametro;
                            ParametriAttivi.OpportunityAttivabile = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.OpportunityOraInizio:
                            ValoriCiclo.OpportunityOraInizio = dato.ValoreParametro;
                            ParametriAttivi.OpportunityOraInizio = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.OpportunityOraFine:
                            ValoriCiclo.OpportunityOraFine = dato.ValoreParametro;
                            ParametriAttivi.OpportunityOraFine = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.OpportunityDurataMax:
                            ValoriCiclo.OpportunityDurataMax = dato.ValoreParametro;
                            ParametriAttivi.OpportunityDurataMax = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.OpportunityCorrente:
                            ValoriCiclo.OpportunityCorrente = dato.ValoreParametro;
                            ParametriAttivi.OpportunityCorrente = StatoCella;
                            numeroValori += 1;
                            break;

                        case SerialMessage.ParametroLadeLight.OpportunityTensioneStop:
                            ValoriCiclo.OpportunityTensioneMax = dato.ValoreParametro;
                            ParametriAttivi.OpportunityTensioneMax = StatoCella;
                            numeroValori += 1;
                            break;

                        #endregion "Opportunity Charge"




                        // Soglie
                        #region"Soglie"
                        case SerialMessage.ParametroLadeLight.TensioneMinimaRiconoscimento:
                            ValoriCiclo.TensRiconoscimentoMin = dato.ValoreParametro;
                            ParametriAttivi.TensRiconoscimentoMin = StatoCella;
                            numeroValori += 1;
                            break;
                        case SerialMessage.ParametroLadeLight.TensioneMassimaRiconoscimento:
                            ValoriCiclo.TensRiconoscimentoMax = dato.ValoreParametro;
                            ParametriAttivi.TensRiconoscimentoMax = StatoCella;
                            numeroValori += 1;
                            break;
                        case SerialMessage.ParametroLadeLight.TensioneMinimaStop:
                            ValoriCiclo.TensMinStop = dato.ValoreParametro;
                            ParametriAttivi.TensMinStop = StatoCella;
                            numeroValori += 1;
                            break;
                        case SerialMessage.ParametroLadeLight.TensioneLimiteCella:
                            ValoriCiclo.TensioneLimiteVLim = dato.ValoreParametro;
                            ParametriAttivi.TensioneLimiteVLim = StatoCella;
                            numeroValori += 1;
                            break;
                        case SerialMessage.ParametroLadeLight.CorrenteMassima:
                            ValoriCiclo.CorrenteMassima = dato.ValoreParametro;
                            ParametriAttivi.CorrenteMassima = StatoCella;
                            numeroValori += 1;
                            break;
                            #endregion "Soglie"
                    }
                }

                CaricaBatteria.EsitoRicalcolo esitoRicalcolo = CalcolaParametriFissi(Batteria._mbTb, Profilo, Tensione, Capacita, NumeroCelle,null);



                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("GeneraProgrammaCarica: " + Ex.Message);
                return false;
            }
        }


        public ModelloCiclo(CicloDiCarica DatiGlobali )
        {
            DatiModello = DatiGlobali;
            ValoriCiclo = new ParametriCiclo();
            ParametriAttivi = new ParametriCiclo();
        }

        public CaricaBatteria.EsitoRicalcolo CalcolaParametri(_mbTipoBatteria Batteria, _mbProfiloCarica Profilo,ushort Tensione, ushort CapacitaDefinita, ushort Celle, _llModelloCb ModelloCB )
        {
            try
            {
                ValoriCiclo = new ParametriCiclo();
                ParametriAttivi = new ParametriCiclo();
             
                bool AttivaArea;


                if (Batteria == null || Profilo == null || Tensione < 1200 || CapacitaDefinita < 50)
                {
                    ValoriCiclo.Esito = 0xFF;
                    ValoriCiclo.Messaggio = "Parametri iniziali non corretti";
                    return CaricaBatteria.EsitoRicalcolo.ParNonValidi;
                }

                ModelloProfilo = (from p in DatiModello.ParametriCarica
                                  where p.BatteryTypeId == Batteria.BatteryTypeId && p.IdProfiloCaricaLL == Profilo.IdProfiloCaricaLL
                                  select p).FirstOrDefault();

                if (ModelloProfilo == null)
                {
                    // Abbinamento batteria / ciclo non previsto
                    ValoriCiclo.Esito = 0xF1;
                    ValoriCiclo.Messaggio = "Abbinamento batteria / ciclo non previsto";
                    return CaricaBatteria.EsitoRicalcolo.ParNonValidi;
                }

                // Prima controllo che tensioni e correnti siano compatibili col CB. se il cb non è indicato, sono compatibili a priori

                if (ModelloCB != null)

                {
                    // Corrente massima richiesta:
                    ValoriCiclo.CorrenteI1 = 0;

                }
                NumeroCelle = Celle;
                Capacita = CapacitaDefinita;
                DurataMaxCarica = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.DurataNominale);

                ParametriAttivi.TempoT0Max = FunzioniComuni.StatoParametro( ModelloProfilo.TempoT0Max);
                ValoriCiclo.TempoT0Max = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TempoT0Max);
                ParametriAttivi.CorrenteI0 = FunzioniComuni.StatoParametro(ModelloProfilo.CorrenteI0);
                ValoriCiclo.CorrenteI0 = FunzioniComuni.CalcolaFormula("C", Capacita, ModelloProfilo.CorrenteI0);
                ParametriAttivi.TensionePrecicloV0 = FunzioniComuni.StatoParametro(ModelloProfilo.TensionePrecicloV0);
                if ((ValoriCiclo.CorrenteI0/10) > ModelloCB.CorrenteMax)
                {
                    return CaricaBatteria.EsitoRicalcolo.ErrIMax;
                }
                
                ValoriCiclo.TensionePrecicloV0 = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TensionePrecicloV0);

                ParametriAttivi.TempoT1Max = FunzioniComuni.StatoParametro(ModelloProfilo.TempoT1Max);
                ValoriCiclo.TempoT1Max = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TempoT1Max);
                ParametriAttivi.CorrenteI1 = FunzioniComuni.StatoParametro(ModelloProfilo.CorrenteI1);
                ValoriCiclo.CorrenteI1 = FunzioniComuni.CalcolaFormula("C", Capacita, ModelloProfilo.CorrenteI1);
                if ((ValoriCiclo.CorrenteI1 / 10) > ModelloCB.CorrenteMax)
                {
                    return CaricaBatteria.EsitoRicalcolo.ErrIMax;
                }
                ParametriAttivi.TensioneSogliaVs = FunzioniComuni.StatoParametro(ModelloProfilo.TensioneSogliaVs);
                ValoriCiclo.TensioneSogliaVs = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TensioneSogliaVs);

                ParametriAttivi.TensioneRaccordoVr = FunzioniComuni.StatoParametro(ModelloProfilo.TensioneRaccordoVr);
                ValoriCiclo.TensioneRaccordoVr = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TensioneRaccordoVr);
                ParametriAttivi.CorrenteRaccordoIr = FunzioniComuni.StatoParametro(ModelloProfilo.CorrenteRaccordoIr);
                ValoriCiclo.CorrenteRaccordoIr = FunzioniComuni.CalcolaFormula("C", Capacita, ModelloProfilo.CorrenteRaccordoIr);

                ParametriAttivi.TempoT2Min = FunzioniComuni.StatoParametro(ModelloProfilo.TempoT2Min);
                ValoriCiclo.TempoT2Min = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TempoT2Min);
                ParametriAttivi.TempoT2Max = FunzioniComuni.StatoParametro(ModelloProfilo.TempoT2Max);
                ValoriCiclo.TempoT2Max = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TempoT2Max);
                ParametriAttivi.FattoreK = FunzioniComuni.StatoParametro(ModelloProfilo.FattoreK);
                ValoriCiclo.FattoreK = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.FattoreK);

                ParametriAttivi.CorrenteFinaleI2 = FunzioniComuni.StatoParametro(ModelloProfilo.CorrenteFinaleI2);
                ValoriCiclo.CorrenteFinaleI2 = FunzioniComuni.CalcolaFormula("C", Capacita, ModelloProfilo.CorrenteFinaleI2);
                if ((ValoriCiclo.CorrenteFinaleI2 / 10) > ModelloCB.CorrenteMax)
                {
                    return CaricaBatteria.EsitoRicalcolo.ErrIMax;
                }
                ParametriAttivi.TensioneMassimaVMax = FunzioniComuni.StatoParametro(ModelloProfilo.TensioneMassimaVMax);
                ValoriCiclo.TensioneMassimaVMax = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TensioneMassimaVMax);

                ParametriAttivi.TempoT3Max = FunzioniComuni.StatoParametro(ModelloProfilo.TempoT3Max);
                ValoriCiclo.TempoT3Max = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TempoT3Max);

                // equal
                ParametriAttivi.EqualAttivabile = FunzioniComuni.StatoParametro(ModelloProfilo.EqualAttivabile);
                ValoriCiclo.EqualAttivabile = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.EqualAttivabile);

                CalcolaEqualizzazione(ValoriCiclo.EqualAttivabile, Tensione, CapacitaDefinita, ModelloCB);

                // Mant
                ParametriAttivi.MantAttivabile = FunzioniComuni.StatoParametro(ModelloProfilo.MantAttivabile);
                ValoriCiclo.MantAttivabile = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.MantAttivabile);

                CalcolaMantenimento(ValoriCiclo.MantAttivabile,  Tensione,  CapacitaDefinita, ModelloCB);

                // Opportunity
                ParametriAttivi.OpportunityAttivabile = FunzioniComuni.StatoParametro(ModelloProfilo.OpportunityAttivabile);
                ValoriCiclo.OpportunityAttivabile = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.OpportunityAttivabile);

                CalcolaOpportunityChg(ValoriCiclo.OpportunityAttivabile, Tensione, CapacitaDefinita, ModelloCB);



                // Soglie
                ParametriAttivi.TensRiconoscimentoMin = FunzioniComuni.StatoParametro(ModelloProfilo.TensRiconoscimentoMin);
                ValoriCiclo.TensRiconoscimentoMin = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TensRiconoscimentoMin);
                ParametriAttivi.TensRiconoscimentoMax = FunzioniComuni.StatoParametro(ModelloProfilo.TensRiconoscimentoMax);
                ValoriCiclo.TensRiconoscimentoMax = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TensRiconoscimentoMax);
                ParametriAttivi.TensMinStop = FunzioniComuni.StatoParametro(ModelloProfilo.TensMinimaStop);
                ValoriCiclo.TensMinStop = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TensMinimaStop);

                ParametriAttivi.TensioneLimiteVLim = FunzioniComuni.StatoParametro(ModelloProfilo.TensioneLimiteVLim);
                ValoriCiclo.TensioneLimiteVLim = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TensioneLimiteVLim);

                ParametriAttivi.CorrenteMassima = FunzioniComuni.StatoParametro(ModelloProfilo.CorrenteMassima);
                ValoriCiclo.CorrenteMassima = FunzioniComuni.CalcolaFormula("C", Capacita, ModelloProfilo.CorrenteMassima);


                // Varie
                ValoriCiclo.AbilitaSpyBatt = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.AbilitaSpyBatt);
                ParametriAttivi.AbilitaSpyBatt = FunzioniComuni.StatoParametro(ModelloProfilo.AbilitaSpyBatt);

                ValoriCiclo.AbilitaSpyBatt = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.AbilitaSafety);
                ParametriAttivi.AbilitaSafety = FunzioniComuni.StatoParametro(ModelloProfilo.AbilitaSafety);



                return CaricaBatteria.EsitoRicalcolo.OK;
            }
            catch (Exception Ex)
            {
                Log.Error("CalcolaParametri: " + Ex.Message);
                return CaricaBatteria.EsitoRicalcolo.ErrGenerico;
            }
        }

        public CaricaBatteria.EsitoRicalcolo CalcolaParametriFissi(_mbTipoBatteria Batteria, _mbProfiloCarica Profilo, ushort Tensione, ushort CapacitaDefinita, ushort Celle, _llModelloCb ModelloCB)
        {
            try
            {
                //ValoriCiclo = new ParametriCiclo();
                //ParametriAttivi = new ParametriCiclo();
                _mbProfiloTipoBatt ModelloProfilo;
                //bool AttivaArea;


                if (Batteria == null || Profilo == null || Tensione < 1200 || CapacitaDefinita < 50)
                {
                    ValoriCiclo.Esito = 0xFF;
                    ValoriCiclo.Messaggio = "Parametri iniziali non corretti";
                    return CaricaBatteria.EsitoRicalcolo.ParNonValidi;
                }

                ModelloProfilo = (from p in DatiModello.ParametriCarica
                                  where p.BatteryTypeId == Batteria.BatteryTypeId && p.IdProfiloCaricaLL == Profilo.IdProfiloCaricaLL
                                  select p).FirstOrDefault();

                if (ModelloProfilo == null)
                {
                    // Abbinamento batteria / ciclo non previsto
                    ValoriCiclo.Esito = 0xF1;
                    ValoriCiclo.Messaggio = "Abbinamento batteria / ciclo non previsto";
                    return CaricaBatteria.EsitoRicalcolo.ParNonValidi;
                }

                DurataMaxCarica = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.DurataNominale);

                return CaricaBatteria.EsitoRicalcolo.OK;
            }
            catch (Exception Ex)
            {
                Log.Error("CalcolaParametri: " + Ex.Message);
                return CaricaBatteria.EsitoRicalcolo.ErrGenerico;
            }
        }




        public bool CalcolaMantenimento(ushort Attivazione, ushort Tensione, ushort CapacitaDefinita,_llModelloCb ModelloCB)
        {
            try
            {
                bool AzzeraValori = false;

                if (ModelloProfilo == null)
                {
                    // Abbinamento batteria / ciclo non previsto
                    AzzeraValori = true;
                }

                if(Attivazione == 0 || Attivazione == 0xF0F0 )
                {
                    AzzeraValori = true;
                }

                ValoriCiclo.MantAttivo = Attivazione;

                if (AzzeraValori)
                {
                    ParametriAttivi.MantTempoAttesa = 0;
                    ValoriCiclo.MantTempoAttesa = 0;
                    ParametriAttivi.MantTensIniziale = 0;
                    ValoriCiclo.MantTensIniziale = 0;
                    ParametriAttivi.MantTensFinale = 0;
                    ValoriCiclo.MantTensFinale = 0;
                    ParametriAttivi.MantTempoMaxErogazione = 0;
                    ValoriCiclo.MantTempoMaxErogazione = 0;
                    ParametriAttivi.MantCorrenteImpulso = 0;
                    ValoriCiclo.MantCorrenteImpulso = 0;

                }
                else
                {
                    ParametriAttivi.MantTempoAttesa = FunzioniComuni.StatoParametro(ModelloProfilo.MantTempoAttesa);
                    ValoriCiclo.MantTempoAttesa = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.MantTempoAttesa);
                    ParametriAttivi.MantTensIniziale = FunzioniComuni.StatoParametro(ModelloProfilo.MantTensIniziale);
                    ValoriCiclo.MantTensIniziale = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.MantTensIniziale);
                    ParametriAttivi.MantTensFinale = FunzioniComuni.StatoParametro(ModelloProfilo.MantTensFinale);
                    ValoriCiclo.MantTensFinale = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.MantTensFinale);
                    ParametriAttivi.MantTempoMaxErogazione = FunzioniComuni.StatoParametro(ModelloProfilo.MantTempoMaxErogazione);
                    ValoriCiclo.MantTempoMaxErogazione = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.MantTempoMaxErogazione);
                    ParametriAttivi.MantCorrenteImpulso = FunzioniComuni.StatoParametro(ModelloProfilo.MantCorrenteImpulso);
                    ValoriCiclo.MantCorrenteImpulso = FunzioniComuni.CalcolaFormula("C", CapacitaDefinita, ModelloProfilo.MantCorrenteImpulso);
                }






                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("CalcolaParametri: " + Ex.Message);
                return false;
            }
        }

        public bool CalcolaEqualizzazione(ushort Attivazione, ushort Tensione, ushort CapacitaDefinita , _llModelloCb ModelloCB)
        {
            try
            {
                bool AzzeraValori = false;
                
                if (ModelloProfilo == null)
                {
                    // Abbinamento batteria / ciclo non previsto
                    AzzeraValori = true;
                }

                if (Attivazione == 0 || Attivazione == 0xF0F0)
                {
                    AzzeraValori = true;
                }

                ValoriCiclo.EqualAttivo = Attivazione;

                if (AzzeraValori)
                {
                    ParametriAttivi.EqualNumImpulsi = 0;
                    ValoriCiclo.EqualNumImpulsi = 0;
                    ParametriAttivi.EqualTempoAttesa = 0;
                    ValoriCiclo.EqualTempoAttesa = 0;
                    ParametriAttivi.EqualTempoImpulso = 0;
                    ValoriCiclo.EqualTempoImpulso = 0;
                    ParametriAttivi.EqualTempoPausa = 0;
                    ValoriCiclo.EqualTempoPausa = 0;
                    ParametriAttivi.EqualCorrenteImpulso = 0;
                    ValoriCiclo.EqualCorrenteImpulso = 0;

                }
                else
                {
                    ParametriAttivi.EqualNumImpulsi = FunzioniComuni.StatoParametro(ModelloProfilo.EqualNumImpulsi);
                    ValoriCiclo.EqualNumImpulsi = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.EqualNumImpulsi);
                    ParametriAttivi.EqualTempoAttesa = FunzioniComuni.StatoParametro(ModelloProfilo.EqualTempoAttesa);
                    ValoriCiclo.EqualTempoAttesa = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.EqualTempoAttesa);
                    ParametriAttivi.EqualTempoImpulso = FunzioniComuni.StatoParametro(ModelloProfilo.EqualTempoImpulso);
                    ValoriCiclo.EqualTempoImpulso = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.EqualTempoImpulso);
                    ParametriAttivi.EqualTempoPausa = FunzioniComuni.StatoParametro(ModelloProfilo.EqualTempoPausa);
                    ValoriCiclo.EqualTempoPausa = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.EqualTempoPausa);
                    ParametriAttivi.EqualCorrenteImpulso = FunzioniComuni.StatoParametro(ModelloProfilo.EqualCorrenteImpulso);
                    ValoriCiclo.EqualCorrenteImpulso = FunzioniComuni.CalcolaFormula("C", CapacitaDefinita, ModelloProfilo.EqualCorrenteImpulso);
                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("CalcolaEqualizzazione: " + Ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Controllo che tensioni e correnti siano compatibili col CB. se il cb non è indicato, sono non compatibili a priori
        /// </summary>
        /// <param name="ModelloCB"></param>
        /// <returns></returns>
        public CaricaBatteria.EsitoRicalcolo VerificaParametri(ushort Tensione, ushort CapacitaDefinita, ushort Celle, _llModelloCb ModelloCB)
        {
            try
            {
                //ValoriCiclo = new ParametriCiclo();

                if (ModelloCB == null)
                {
                    return CaricaBatteria.EsitoRicalcolo.ErrCBNonDef;
                }

                // Preciclo
                if (((double)(ValoriCiclo.TensionePrecicloV0 * Celle) / 100) > ModelloCB.TensioneMax)
                {
                    return CaricaBatteria.EsitoRicalcolo.ErrVMax;
                }
                if ((ValoriCiclo.CorrenteI0 / 10) > ModelloCB.CorrenteMax)
                {
                    return CaricaBatteria.EsitoRicalcolo.ErrIMax;
                }

                // Fase 1  (I)
                if (   (((double)(ValoriCiclo.TensioneSogliaVs * Celle) / 100) > ModelloCB.TensioneMax) 
                    || (((double)(ValoriCiclo.TensioneRaccordoVr * Celle) / 100) > ModelloCB.TensioneMax))
                {
                    return CaricaBatteria.EsitoRicalcolo.ErrVMax;
                }
                if ((ValoriCiclo.CorrenteI1 / 10) > ModelloCB.CorrenteMax)
                {
                    return CaricaBatteria.EsitoRicalcolo.ErrIMax;
                }

                if ((ValoriCiclo.CorrenteRaccordoIr / 10) > ModelloCB.CorrenteMax)
                {
                    return CaricaBatteria.EsitoRicalcolo.ErrIMax;
                }

                // Fase 2
                if ((ValoriCiclo.CorrenteFinaleI2 / 10) > ModelloCB.CorrenteMax)
                {
                    return CaricaBatteria.EsitoRicalcolo.ErrIMax;
                }



                // equal

                //CalcolaEqualizzazione(ValoriCiclo.EqualAttivabile, Tensione, CapacitaDefinita, ModelloProfilo, ModelloCB);

                // Mant

                //CalcolaMantenimento(ValoriCiclo.MantAttivabile, Tensione, CapacitaDefinita, ModelloProfilo, ModelloCB);

  


                return CaricaBatteria.EsitoRicalcolo.OK;
            }
            catch (Exception Ex)
            {
                Log.Error("CalcolaParametri: " + Ex.Message);
                return CaricaBatteria.EsitoRicalcolo.ErrGenerico;
            }
        }

        public bool CalcolaOpportunityChg(ushort Attivazione, ushort Tensione, ushort CapacitaDefinita,  _llModelloCb ModelloCB)
        {
            try
            {
                bool AzzeraValori = false;
                double FattoreCorrente = 1;
                ushort TempCorrente;

                if (ModelloProfilo == null)
                {
                    // Abbinamento batteria / ciclo non previsto
                    AzzeraValori = true;
                }

                if (Attivazione == 0 || Attivazione == 0xF0F0)
                {
                    AzzeraValori = true;
                }

                ValoriCiclo.OpportunityAttivo = Attivazione;

                if (AzzeraValori)
                {
                    ParametriAttivi.OpportunityOraInizio = 0;
                    ValoriCiclo.OpportunityOraInizio = 0;
                    ParametriAttivi.OpportunityOraFine = 0;
                    ValoriCiclo.OpportunityOraFine = 0;
                    ParametriAttivi.OpportunityDurataMax = 0;
                    ValoriCiclo.OpportunityDurataMax = 0;
                    ParametriAttivi.OpportunityCorrente = 0;
                    ValoriCiclo.OpportunityCorrente = 0;
                    ParametriAttivi.OpportunityTensioneMax = 0;
                    ValoriCiclo.OpportunityTensioneMax = 0;

                }
                else
                {
                    ParametriAttivi.OpportunityOraInizio = FunzioniComuni.StatoParametro(ModelloProfilo.OpportunityOraInizio);
                    ValoriCiclo.OpportunityOraInizio = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.OpportunityOraInizio);
                    ParametriAttivi.OpportunityOraFine = FunzioniComuni.StatoParametro(ModelloProfilo.OpportunityOraFine);
                    ValoriCiclo.OpportunityOraFine = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.OpportunityOraFine);
                    ParametriAttivi.OpportunityCorrente = FunzioniComuni.StatoParametro(ModelloProfilo.OpportunityCorrente);
                    ValoriCiclo.OpportunityCorrente = FunzioniComuni.CalcolaFormula("C", CapacitaDefinita, ModelloProfilo.OpportunityCorrente);
                    TempCorrente = ValoriCiclo.OpportunityCorrente;
                    if (ValoriCiclo.OpportunityCorrente > (ModelloCB.CorrenteMax * 10))
                    {
                        ValoriCiclo.OpportunityCorrente = (ushort)(ModelloCB.CorrenteMax * 10);
                    }

                    FattoreCorrente =  (double)TempCorrente / (double)ValoriCiclo.OpportunityCorrente;

                    ParametriAttivi.OpportunityDurataMax = FunzioniComuni.StatoParametro(ModelloProfilo.OpportunityDurataMax);
                    ValoriCiclo.OpportunityDurataMax =(ushort)(FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.OpportunityDurataMax) * FattoreCorrente);

                    ParametriAttivi.OpportunityTensioneMax = FunzioniComuni.StatoParametro(ModelloProfilo.OpportunityTensioneMax);
                    ValoriCiclo.OpportunityTensioneMax = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.OpportunityTensioneMax);

                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("CalcolaParametri: " + Ex.Message);
                return false;
            }
        }



        public int GeneraListaValori()
        {
            try
            {
                NumParametriAttivi = 0;
                ListaParametri = new List<ParametroLL>();
                ParametroLL NuovoParametro;
                // Parametribase:

                // - Modello Ladelight
                NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.ModelloLadeLight, ValoriCiclo.IdModelloLL);
                ListaParametri.Add(NuovoParametro);
                NumParametriAttivi += 1;


                // - Tipo Batteria
                NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TipoBatteria, Batteria.BatteryTypeId);
                ListaParametri.Add(NuovoParametro);
                NumParametriAttivi += 1;

                // Tensione
                NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TensioneNominale, Tensione);
                ListaParametri.Add(NuovoParametro);
                NumParametriAttivi += 1;

                // Celle
                NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.NumeroCelle, NumeroCelle);
                ListaParametri.Add(NuovoParametro);
                NumParametriAttivi += 1;

                // Capacità
                NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.CapacitaNominale, Capacita);
                ListaParametri.Add(NuovoParametro);
                NumParametriAttivi += 1;

                // Tipo Profilo
                NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TipoProfilo, Profilo.IdProfiloCaricaLL);
                ListaParametri.Add(NuovoParametro);
                NumParametriAttivi += 1;

                // Preciclo
                if (true)  // (ParametriAttivi.TempoT0Max > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TempoT0Max, ValoriCiclo.TempoT0Max);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.CorrenteI0 > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.CorrentePrecicloI0, ValoriCiclo.CorrenteI0);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.TensionePrecicloV0 > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TensionePrecicloV0, ValoriCiclo.TensionePrecicloV0);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                // Fase 1 (I)

                if (true)  // (ParametriAttivi.TempoT1Max > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TempoT1Max, ValoriCiclo.TempoT1Max);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.CorrenteI1 > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.CorrenteCaricaI1, ValoriCiclo.CorrenteI1);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.TensioneSogliaVs > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TensioneSogliaF1, ValoriCiclo.TensioneSogliaVs);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.TensioneRaccordoVr > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TensioneRaccordoF1, ValoriCiclo.TensioneRaccordoVr);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.CorrenteRaccordoIr > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.CorrenteRaccordo, ValoriCiclo.CorrenteRaccordoIr);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }


                if (true)  // (ParametriAttivi.TempoT2Min > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TempoT2Min, ValoriCiclo.TempoT2Min);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.TempoT2Max > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TempoT2Max, ValoriCiclo.TempoT2Max);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.FattoreK > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.CoeffK, ValoriCiclo.FattoreK);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.CorrenteFinaleI2 > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.CorrenteFinaleF2, ValoriCiclo.CorrenteFinaleI2);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.TempoT3Max > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TempoT3Max, ValoriCiclo.TempoT3Max);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.TensioneMassimaVMax > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TensioneMassimaCella, ValoriCiclo.TensioneMassimaVMax);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.AbilitaSpyBatt > 0) --> lo passo sempre
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.DivisoreK, ValoriCiclo.AbilitaSpyBatt);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.AbilitaSafety > 0) --> lo passo sempre
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.Safety, ValoriCiclo.AbilitaSafety);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }


                if (true)  // (ParametriAttivi.TensRiconoscimentoMin > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TensioneMinimaRiconoscimento, ValoriCiclo.TensRiconoscimentoMin);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.TensRiconoscimentoMax > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TensioneMassimaRiconoscimento, ValoriCiclo.TensRiconoscimentoMax);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.TensMinStop > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TensioneMinimaStop, ValoriCiclo.TensMinStop);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.TensioneLimiteVLim > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TensioneLimiteCella, ValoriCiclo.TensioneLimiteVLim);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.CorrenteMassima > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.CorrenteMassima, ValoriCiclo.CorrenteMassima);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.TipoCassone > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.TipoCassone, ValoriCiclo.TipoCassone);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }
                

                if (true)  // (ParametriAttivi.EqualAttivo > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.EqualAttivo, ValoriCiclo.EqualAttivo);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.EqualFineCaricaAttesa > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.EqualFineCaricaAttesa, ValoriCiclo.EqualTempoAttesa);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }
                if (true)  // (ParametriAttivi.EqualNumImpulsi > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.EqualFineCaricaNumImpulsi, ValoriCiclo.EqualNumImpulsi);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }
                if (true)  // (ParametriAttivi.EqualFineCaricaDurataI > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.EqualFineCaricaDurataI, ValoriCiclo.EqualTempoImpulso);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }
                if (true)  // (ParametriAttivi.EqualFineCaricaDurataP > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.EqualFineCaricaDurataP, ValoriCiclo.EqualTempoPausa);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.EqualFineCaricaCorrenteImp > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.EqualFineCaricaCorrenteImp, ValoriCiclo.EqualCorrenteImpulso);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.MantenimentoAttivo > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.MantenimentoAttivo, ValoriCiclo.MantAttivo);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.MantenimentoAttesa > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.MantenimentoAttesa, ValoriCiclo.MantTempoAttesa);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }
                if (true)  // (ParametriAttivi.MantenimentoTensIniziale > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.MantenimentoTensIniziale, ValoriCiclo.MantTensIniziale);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }
                if (true)  // (ParametriAttivi.MantenimentoTensFinale > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.MantenimentoTensFinale, ValoriCiclo.MantTensFinale);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }
                if (true)  // (ParametriAttivi.MantenimentoTMaxErogazione > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.MantenimentoTMaxErogazione, ValoriCiclo.MantTempoMaxErogazione);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }
                if (true)  // (ParametriAttivi.MantenimentoCorrErogazione > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.MantenimentoCorrErogazione, ValoriCiclo.MantCorrenteImpulso);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.OpportunityAttivo > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.OpportunityAttivo, ValoriCiclo.OpportunityAttivo);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.OpportunityOraInizio > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.OpportunityOraInizio, ValoriCiclo.OpportunityOraInizio);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.OpportunityOraFine > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.OpportunityOraFine, ValoriCiclo.OpportunityOraFine);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.OpportunityDurataMax > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.OpportunityDurataMax, ValoriCiclo.OpportunityDurataMax);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.OpportunityCorrente > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.OpportunityCorrente, ValoriCiclo.OpportunityCorrente);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }

                if (true)  // (ParametriAttivi.OpportunityTensioneStop > 0)
                {
                    NuovoParametro = new ParametroLL((byte)SerialMessage.ParametroLadeLight.OpportunityTensioneStop, ValoriCiclo.OpportunityTensioneMax);
                    ListaParametri.Add(NuovoParametro);
                    NumParametriAttivi += 1;
                }




                return NumParametriAttivi;

            }
            catch (Exception Ex)
            {
                Log.Error("GeneraListaValori: " + Ex.Message );
                return NumParametriAttivi;
            }
        }

        public int CaricaListaValori(bool RigeneraPermessi = true )
        {
            try
            {

                ValoriCiclo = new ParametriCiclo();
                if (RigeneraPermessi)
                {
                    ParametriAttivi = new ParametriCiclo();
                }

                NumParametriAttivi = ListaParametri.Count;
                foreach(ParametroLL _par in ListaParametri)
                {
                    SerialMessage.ParametroLadeLight _idPar;
                    _idPar = (SerialMessage.ParametroLadeLight)_par.idParametro;

                    switch(_idPar)
                    {
                        case SerialMessage.ParametroLadeLight.TempoT0Max:                      
                            ValoriCiclo.TempoT0Max = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.TempoT0Max = 1; // Read only                
                            break;          

                        case SerialMessage.ParametroLadeLight.CorrentePrecicloI0:                    
                            ValoriCiclo.CorrenteI0 = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.CorrenteI0 = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.TensionePrecicloV0:
                            ValoriCiclo.TensionePrecicloV0 = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.TensionePrecicloV0 = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.TempoT1Max:
                            ValoriCiclo.TempoT1Max = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.TempoT1Max = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.CorrenteCaricaI1:
                            ValoriCiclo.CorrenteI1 = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.CorrenteI1 = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.TensioneSogliaF1:
                            ValoriCiclo.CorrenteI1 = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.CorrenteI1 = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.TensioneRaccordoF1:
                            ValoriCiclo.TensioneRaccordoVr = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.TensioneRaccordoVr = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.CorrenteRaccordo:
                            ValoriCiclo.CorrenteRaccordoIr = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.CorrenteRaccordoIr = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.TempoT2Min:
                            ValoriCiclo.TempoT2Min = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.TempoT2Min = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.TempoT2Max:
                            ValoriCiclo.TempoT2Max = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.TempoT2Max = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.CoeffK:
                            ValoriCiclo.FattoreK = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.FattoreK = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.CorrenteFinaleF2:
                            ValoriCiclo.CorrenteFinaleI2 = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.CorrenteFinaleI2 = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.TensioneMassimaCella:
                            ValoriCiclo.TensioneMassimaVMax = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.TensioneMassimaVMax = 1; // Read only                
                            break;

                        case SerialMessage.ParametroLadeLight.TempoT3Max:
                            ValoriCiclo.TempoT3Max = _par.ValoreParametro;
                            if (RigeneraPermessi) ParametriAttivi.TempoT3Max = 1; // Read only                
                            break;

                    }

                }

                return NumParametriAttivi;
            }
            catch (Exception Ex)
            {
                Log.Error("GeneraListaValori: " + Ex.Message);
                return NumParametriAttivi;
            }
        }


    }
}
