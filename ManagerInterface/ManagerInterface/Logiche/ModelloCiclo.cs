
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

using SQLite.Net;

namespace ChargerLogic
{
    class ModelloCiclo
    {
        public ParametriCiclo ValoriCiclo;
        public ParametriCiclo ParametriAttivi;
        public CicloDiCarica DatiModello;

        public ModelloCiclo()
        {
            DatiModello = new CicloDiCarica();
            DatiModello.InizializzaDatiLocali();
            ValoriCiclo = new ParametriCiclo();
            ParametriAttivi = new ParametriCiclo();
        }

        public ModelloCiclo(CicloDiCarica DatiGlobali )
        {
            DatiModello = DatiGlobali;
            ValoriCiclo = new ParametriCiclo();
            ParametriAttivi = new ParametriCiclo();
        }


        public bool CalcolaParametri(_mbTipoBatteria Batteria, _mbProfiloCarica Profilo,ushort Tensione, ushort Capacita, ushort Celle, _llModelloCb ModelloCB )
        {
            try
            {
                ValoriCiclo = new ParametriCiclo();
                ParametriAttivi = new ParametriCiclo();
                _mbProfiloTipoBatt ModelloProfilo;

                if (Batteria == null || Profilo == null || Tensione < 1200 || Capacita < 50)
                {
                    ValoriCiclo.Esito = 0xFF;
                    ValoriCiclo.Messaggio = "Parametri iniziali non corretti";
                    return false;
                }

                ModelloProfilo = (from p in DatiModello.ParametriCarica
                                  where p.BatteryTypeId == Batteria.BatteryTypeId && p.IdProfiloCaricaLL == Profilo.IdProfiloCaricaLL
                                  select p).FirstOrDefault();

                if (ModelloProfilo == null)
                {
                    // Abbinamento batteria / ciclo non previsto
                    ValoriCiclo.Esito = 0xF1;
                    ValoriCiclo.Messaggio = "Abbinamento batteria / ciclo non previsto";
                    return false;
                }

                // Prima controllo che tensioni e correnti siano compatibili col CB. se il cb non è indicato, sono compatibili a priori

                if (ModelloCB != null)

                {
                    // Corrente massima richiesta:
                    ValoriCiclo.CorrenteI1 = 0;

                }

                ParametriAttivi.TempoT0Max = FunzioniComuni.StatoParametro( ModelloProfilo.TempoT0Max);
                ValoriCiclo.TempoT0Max = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TempoT0Max);
                ParametriAttivi.CorrenteI0 = FunzioniComuni.StatoParametro(ModelloProfilo.CorrenteI0);
                ValoriCiclo.CorrenteI0 = FunzioniComuni.CalcolaFormula("C", Capacita, ModelloProfilo.CorrenteI0);
                ParametriAttivi.TensionePrecicloV0 = FunzioniComuni.StatoParametro(ModelloProfilo.TensionePrecicloV0);
                ValoriCiclo.TensionePrecicloV0 = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TensionePrecicloV0);

                ParametriAttivi.TempoT1Max = FunzioniComuni.StatoParametro(ModelloProfilo.TempoT1Max);
                ValoriCiclo.TempoT1Max = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TempoT1Max);
                ParametriAttivi.CorrenteI1 = FunzioniComuni.StatoParametro(ModelloProfilo.CorrenteI1);
                ValoriCiclo.CorrenteI1 = FunzioniComuni.CalcolaFormula("C", Capacita, ModelloProfilo.CorrenteI1);
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
                ParametriAttivi.TensioneMassimaVMax = FunzioniComuni.StatoParametro(ModelloProfilo.TensioneMassimaVMax);
                ValoriCiclo.TensioneMassimaVMax = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TensioneMassimaVMax);

                ParametriAttivi.TempoT3Max = FunzioniComuni.StatoParametro(ModelloProfilo.TempoT3Max);
                ValoriCiclo.TempoT3Max = FunzioniComuni.CalcolaFormula("#", 0, ModelloProfilo.TempoT3Max);


                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
