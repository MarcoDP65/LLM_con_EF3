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

        public bool CaricaListaCicli(UInt32 StartAddr, ushort NumRows, out object EsitoCaricamento, bool caricaBrevi = false,  bool RunAsinc = false)
        {
            try
            {
                bool _esito = false;
                object _dataRx;
                elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
                elementiComuni.WaitStep _stepBg = new elementiComuni.WaitStep();

                SerialMessage.EsitoRisposta _esitoMsg;
                EsitoCaricamento = null;

                // Vuoto l'elenco corrente
                MemoriaCicli = new List<llMemoriaCicli>();


                if (NumRows < 1) NumRows = 0xFFFF;
                UInt32 AddrCorrente;

                if (RunAsinc)
                {
                    //Preparo l'intestazione della finestra di avanzamento                                                                                                          
                    if (Step != null)
                    {
                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                        _passo.Titolo = "Fase 1 - Caricamento Eventi Lunghi LL";
                        _passo.Eventi = 1;
                        _passo.Step = -1;
                        _passo.EsecuzioneInterrotta = false;
                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                        Step(this, _stepEv);
                    }

                }


                for (byte contacicli = 0; contacicli < NumRows; contacicli++)
                {
                    if (RichiestaInterruzione)
                    {
                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                        _passo.Titolo = "Fase 2 - Caricamento Eventi Brevi";
                        _passo.Eventi = 1;
                        _passo.Step = -1;
                        _passo.EsecuzioneInterrotta = true;
                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                        Step(this, _stepEv);
                        break;
                    }


                    AddrCorrente = StartAddr + (UInt32)(SizeCharge * contacicli);
                    llMemoriaCicli _tempCiclo = CaricaDatiCiclo(AddrCorrente);
                    _tempCiclo.Posizione = contacicli;
                    if (true) //_tempCiclo.IdMemoriaLunga != 0xFFFFFFFF)
                    {
                        MemoriaCicli.Add(_tempCiclo);
                        //prima avanzo il contatore lunghi
                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                        int _progress = 0;
                        double _valProgress = 0;
                        _passo.TipoDati = elementiComuni.tipoMessaggio.MemLungaLL;
                        _passo.Titolo = "";
                        _passo.Eventi = NumRows;
                        _passo.Step = contacicli;
                        if (_passo.Eventi > 0)
                        {
                            _valProgress = (_passo.Step * 100) / _passo.Eventi;
                        }
                        _progress = (int)_valProgress;
                        if (RunAsinc)
                        {
                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                            Step(this, _stepEv);
                        }
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
                EsitoCaricamento = null;
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



        public List<llMemBreve> CaricaListaBrevi(UInt32 StartAddr, ushort NumRows = 0, uint IdCiclo = 0 )
        {

            bool _esito;
            List<llMemBreve> ListaBrevi;
            try
            {
                ListaBrevi = new List<llMemBreve>();


                SerialMessage.EsitoRisposta _esitoMsg;

                if (NumRows < 1) return ListaBrevi;

                UInt32 AddrCorrente;

                if (NumRows == 0xFFFF)
                {
                    // manca il numero cicli. vado in scansione 
                    byte contacicli = 0;
                    bool trovatoUltimo = false;

                    while ( !trovatoUltimo)
                    {
                        AddrCorrente = FirstShort + (StartAddr + contacicli) * SizeShort;
                        llMemBreve _tempBreve = CaricaDatiMemBreve(AddrCorrente);
                        if (_tempBreve.IdMemCiclo == IdCiclo )
                        {
                            ListaBrevi.Add(_tempBreve);
                            contacicli += 1;
                            trovatoUltimo = false;
                        }
                        else
                        {
                            trovatoUltimo = true;
                        }
                    }

                    

                }
                else
                {
                    // Il numero è definito. leggo esattamente i brevi indicati.
                    for (byte contacicli = 0; contacicli < NumRows; contacicli++)
                    {
                        AddrCorrente = FirstShort + (StartAddr + contacicli) * SizeShort;

                        llMemBreve _tempBreve = CaricaDatiMemBreve(AddrCorrente);
                        ListaBrevi.Add(_tempBreve);

                    }
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
