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
                        tempPrg.ModStop = ImmagineCarica.ModStop;


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


        public llMemBreve CaricaDatiMemBreve(UInt32 StartAddr)
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;
                llMemBreve tempPrg = new llMemBreve();
                MessaggioLadeLight.MessaggioMemoriaBreve ImmagineMemBreve = new MessaggioLadeLight.MessaggioMemoriaBreve();
                SerialMessage.EsitoRisposta EsitoMsg;


                byte[] _datiTemp = new byte[SizeShort];
                _esito = LeggiBloccoMemoria(StartAddr, SizeShort, out _datiTemp);

                if (_esito)
                {
                    EsitoMsg = ImmagineMemBreve.analizzaMessaggio(_datiTemp, 1);
                    if (EsitoMsg == SerialMessage.EsitoRisposta.MessaggioOk)
                    {


                        tempPrg.IdMemCiclo = (int)ImmagineMemBreve.NumeroCarica;
                        tempPrg.IdMemoriaBreve = ImmagineMemBreve.NumeroEvtBreve;
                        tempPrg.TimestampRegistrazione = ImmagineMemBreve.DataOraEvento;
                        tempPrg.VBatt = ImmagineMemBreve.VBatt;
                        tempPrg.IBatt = ImmagineMemBreve.IBatt;
                        tempPrg.IBattMin = ImmagineMemBreve.IBattMin;
                        tempPrg.IBattMax = ImmagineMemBreve.IBattMax;
                        tempPrg.TempBatt = ImmagineMemBreve.TempBatt;
                        tempPrg.TempIGBT1 = ImmagineMemBreve.TempIGBT1;
                        tempPrg.TempIGBT2 = ImmagineMemBreve.TempIGBT2;
                        tempPrg.TempIGBT3 = ImmagineMemBreve.TempIGBT3;
                        tempPrg.TempIGBT4 = ImmagineMemBreve.TempIGBT4;
                        tempPrg.TempDiode = ImmagineMemBreve.TempDiode;
                        tempPrg.VettoreErrori = ImmagineMemBreve.VettoreErrori;
                        tempPrg.DurataBreve = ImmagineMemBreve.DurataBreve;

                        return tempPrg;
                    }

                }

                return null; 
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return null;
            }

        }



        public List<llMemBreve> CaricaListaBrevi(UInt32 StartAddr, ushort NumRows = 0)
        {

            bool _esito;
            List<llMemBreve> ListaBrevi;
            try
            {
                ListaBrevi = new List<llMemBreve>();


                SerialMessage.EsitoRisposta _esitoMsg;

                if (NumRows < 1) return ListaBrevi;

                UInt32 AddrCorrente;


                for (byte contacicli = 0; contacicli < NumRows; contacicli++)
                {
                    AddrCorrente = FirstShort + (StartAddr + contacicli) * SizeShort;
                    // public const byte SizeShort = 26;
                    // public const UInt32 FirstShort = 0x005000;

                    llMemBreve _tempBreve = CaricaDatiMemBreve(AddrCorrente);
                    ListaBrevi.Add(_tempBreve);

                }


                return ListaBrevi;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return null;
            }

        }


    }
}
