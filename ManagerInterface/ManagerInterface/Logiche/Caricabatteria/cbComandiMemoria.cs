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
    public partial class CaricaBatteria
    {
        public llMemoriaCicli CaricaDatiCiclo(UInt32 StartAddr)
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;
                llMemoriaCicli tempPrg = new llMemoriaCicli();
                MessaggioLadeLight.MessaggioMemoriaLunga ImmagineCarica = new MessaggioLadeLight.MessaggioMemoriaLunga();
                SerialMessage.EsitoRisposta EsitoMsg;


                byte[] _datiTemp = new byte[SizeCharge];
                _esito = LeggiBloccoMemoria(StartAddr, SizeCharge, out _datiTemp);

                if (_esito)
                {
                    EsitoMsg = ImmagineCarica.analizzaMessaggio(_datiTemp, 1);
                    if (EsitoMsg == SerialMessage.EsitoRisposta.MessaggioOk)
                    {
                        tempPrg.IdMemoriaLunga = ImmagineCarica.NumeroCarica;
                        tempPrg.IdSpyBatt = ImmagineCarica.IdSpyBatt;
                        tempPrg.IdProgramma = ImmagineCarica.IdProgrammazione;
                        tempPrg.PuntatorePrimoBreve = ImmagineCarica.PntPrimoBreve;
                        tempPrg.NumEventiBrevi = ImmagineCarica.CntCicliBrevi;
                        tempPrg.DataOraStart = FunzioniMR.StringaTimestamp(ImmagineCarica.DataOraStart);
                        tempPrg.DataOraFine = FunzioniMR.StringaTimestamp(ImmagineCarica.DataOraStop);
                        tempPrg.Ah = ImmagineCarica.AhCaricati;
                        tempPrg.Wh = (int)ImmagineCarica.WhCaricati;


                        return tempPrg;
                    }

                }

                return null;  // llProgrammaCarica
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return null;
            }

        }

        public bool CaricaListaCicli(UInt32 StartAddr, ushort NumRows = 0)
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;

                MemoriaCicli = new List<llMemoriaCicli>();

                if (NumRows < 1) NumRows = 0xFFFF;
                UInt32 AddrCorrente;


                for (byte contacicli = 0; contacicli < NumRows; contacicli++)
                {
                    AddrCorrente = StartAddr + (UInt32)(SizeCharge * contacicli);
                    llMemoriaCicli _tempCiclo = CaricaDatiCiclo(AddrCorrente);
                    if (_tempCiclo.IdMemoriaLunga != 0xFFFFFFFF)
                    {
                        MemoriaCicli.Add(_tempCiclo);
                        _esito = true; // ho almeno 1 programma;
                    }
                    else
                    {
                        break;
                    }

                }


                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }

        }

    }
}
