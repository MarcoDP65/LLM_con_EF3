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
                ushort SizeCharge = Memoria.MappaCorrente.RecordLunghi.SizeMsgDati;

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

        public llMemoriaCicli AnalizzaDatiCiclo(byte[] ArrayDati)
        {
            try
            {
                ushort SizeCharge = Memoria.MappaCorrente.RecordLunghi.SizeMsgDati;

                if (ArrayDati.Length != SizeCharge)
                {
                    // Array di dimensioni errate. non è una carica. Esco prima di iniziare
                    return null;
                }

                bool _esito = false;
                llMemoriaCicli tempPrg = new llMemoriaCicli(DbAttivo);
                MessaggioLadeLight.MessaggioMemoriaLunga ImmagineCarica = new MessaggioLadeLight.MessaggioMemoriaLunga();
                SerialMessage.EsitoRisposta EsitoMsg;

                EsitoMsg = ImmagineCarica.analizzaMessaggio(ArrayDati, ParametriApparato.llParApp.ModelloMemoria);
                if (EsitoMsg == SerialMessage.EsitoRisposta.MessaggioOk)
                {
                    tempPrg.IdApparato = ParametriApparato.IdApparato;
                    tempPrg.IdMemoriaLunga = ImmagineCarica.NumeroCarica;
                    tempPrg.IdSpyBatt = ImmagineCarica.IdSpyBatt;
                    tempPrg.IdProgramma = ImmagineCarica.IdProgrammazione;
                    tempPrg.PuntatorePrimoBreve = ImmagineCarica.PntPrimoBreve;
                    tempPrg.NumEventiBrevi = ImmagineCarica.CntCicliBrevi;
                    tempPrg.DataOraStart = FunzioniMR.StringaTimestamp(ImmagineCarica.DataOraStart);
                    tempPrg.DataOraFine = FunzioniMR.StringaTimestamp(ImmagineCarica.DataOraStop);
                    tempPrg.Ah = ImmagineCarica.AhCaricati;
                    tempPrg.Wh = (int)ImmagineCarica.WhCaricati;
                    tempPrg.Vbat5m = ImmagineCarica.Vbat5m;
                    tempPrg.Ibat5m = ImmagineCarica.Ibat5m;
                    tempPrg.VbatFinale = ImmagineCarica.VbatFinale;
                    tempPrg.IbatFinale = ImmagineCarica.IbatFinale;

                    tempPrg.OpzioniCarica = ImmagineCarica.OpzioniCarica;
                    tempPrg.VettoreErrori = ImmagineCarica.VettoreErrori;


                    tempPrg.ModStop = ImmagineCarica.ModStop;


                    return tempPrg;
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
                ushort SizeCharge = Memoria.MappaCorrente.RecordLunghi.SizeMsgDati;

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
                    for (ushort contacicli = 0; contacicli < NumRows; contacicli++)
                    {
                        AddrCorrente = (uint)(FirstShort + (StartAddr + contacicli) * SizeShort);

                        if (contacicli > 390)
                        {
                            Log.Debug(" Contacicli = " + contacicli.ToString());
                        }

                        llMemBreve _tempBreve = CaricaDatiMemBreve(AddrCorrente);

                        if (_tempBreve == null)
                        {
                            break;
                        }

                        if (_tempBreve.IdMemCiclo == IdCiclo)
                        {
                            ListaBrevi.Add(_tempBreve);
                        }
                        else
                        {
                            break;
                        }
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

        public const int MAX_RETRY = 5;          // num max di tentativi di rilettura prima di abortire l'operazione
    

        public bool LeggiBloccoLunghi(bool RunAsinc = false,int StepIniziale = 0, int StepFinale = 100 )
        {
            try
            {
                bool _esito;
                byte ModMemoria = ParametriApparato.llParApp.ModelloMemoria;
                // innanzitutto verifico che modello memoria usare.

                int _ADDR_START_RECORD_LUNGHI = (int)Memoria.MappaCorrente.RecordLunghi.AddrArea;
                int _LEN_AREA_RECORD_LUNGHI =  Memoria.MappaCorrente.RecordLunghi.NumPagine * 0x01000 ;

  

                if (RunAsinc)
                {
                    //Preparo l'intestazione della finestra di avanzamento                                                                                                          
                    if (Step != null)
                    {
                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                        _passo.Titolo = "Caricamento Memoria Cariche" ;
                        _passo.Eventi = 1;
                        _passo.Step = -1;
                        _passo.EsecuzioneInterrotta = false;
                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(StepIniziale, _passo);
                        Step(this, _stepEv);
                    }

                }
                // controllo che il canale sia aperto. se no riapro o fallisco

                //  _esito = apriPorta();
                if (!apriPorta())
                {
                    elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                    _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                    _passo.Titolo = "Caricabatteria SCOLLEGATO o NON RAGGIUNGIBILE";
                    _passo.Eventi = 1;
                    _passo.Step = -1;
                    _passo.NumTentativi = 0;
                    _passo.EsecuzioneInterrotta = true;
                    ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                    Step(this, _stepEv);
                    Log.Debug("Lettura area lunghi => apertura porta fallita! ");

                    System.Threading.Thread.Sleep(5000);

                }


                Log.Debug("Inizio lettura area lunghi");
                DateTime Inizio = DateTime.Now;
                BloccoLunghi = new byte[_LEN_AREA_RECORD_LUNGHI];
                // inizializzo l'array a FF
                for (int _addr = 0; _addr< _LEN_AREA_RECORD_LUNGHI; _addr ++)
                {
                    BloccoLunghi[_addr] = 0xFF;
                }

                int DimPacchetto = 240;
                int LenPacchetto = 240;
                int NumPacchetto = 0;
                int NumRetry = 0;
                int TotFail = 0;
                bool SkipOver = false;
                bool PacchettoValido = false;
                byte[] TmpBuffer = new byte[LenPacchetto];
                bool FineLettura = false;
                int IndirizzoRelativo = 0;

                while (!FineLettura && !SkipOver)
                {
                    if ((_LEN_AREA_RECORD_LUNGHI - IndirizzoRelativo) >= LenPacchetto)
                    {
                        DimPacchetto = LenPacchetto;
                    }
                    else
                    {
                        DimPacchetto = _LEN_AREA_RECORD_LUNGHI - IndirizzoRelativo;
                    }
                    uint TempAddr = (uint)(_ADDR_START_RECORD_LUNGHI + IndirizzoRelativo);
                    PacchettoValido = false;
                    NumRetry = 0;
                    while (!PacchettoValido)
                    {
                        _esito = LeggiBloccoMemoria(TempAddr, (ushort)DimPacchetto, out TmpBuffer);
                        if (_esito)
                        {
                            for (int _ciclo = 0; _ciclo < DimPacchetto; _ciclo++)
                            {
                                BloccoLunghi[IndirizzoRelativo + _ciclo] = TmpBuffer[_ciclo];
                            }
                            IndirizzoRelativo += DimPacchetto;
                            NumPacchetto++;
                            TotFail += NumeroTentativiLettura - 1;
                            if (IndirizzoRelativo >= _LEN_AREA_RECORD_LUNGHI)
                            {
                                FineLettura = true;
                            }
                            PacchettoValido = true;
                            if (RunAsinc)
                            {
                                if (!RichiestaInterruzione)
                                {
                                    elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                    int _progress = 0;
                                    double _valProgress = 0;
                                    _passo.TipoDati = elementiComuni.tipoMessaggio.AreaMemLungaLL;
                                    _passo.Titolo = "";
                                    _passo.Eventi = _LEN_AREA_RECORD_LUNGHI;
                                    _passo.Step = IndirizzoRelativo;
                                    _passo.NumTentativi = TotFail;
                                    if (_passo.Eventi > 0)
                                    {
                                        _valProgress = _passo.Step * (StepFinale - StepIniziale) / _passo.Eventi;
                                    }
                                    _progress = (int)( _valProgress + StepIniziale ) ;

                                    ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                    Step(this, _stepEv);
                                }
                                else
                                {
                                    elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                    _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                    _passo.Titolo = "Caricamento ANNULLATO";
                                    _passo.Eventi = 1;
                                    _passo.Step = -1;
                                    _passo.NumTentativi = NumRetry;
                                    _passo.EsecuzioneInterrotta = true;
                                    ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                    Step(this, _stepEv);
                                    Log.Debug("Lettura area lunghi => lettura interrotta: " + IndirizzoRelativo.ToString("x4"));

                                    System.Threading.Thread.Sleep(2000);

                                    SkipOver = true;
                                    break;
                                }
                            }

                        }
                        else
                        {
                            // Errore di lettura. se il num tentatovi è inferiore al massimo riprovo, 
                            NumRetry += 1;
                            TotFail += 1;
                            if (NumRetry > MAX_RETRY)
                            {
                                // Oltre il massimo chiudo con errore


                                elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = "Caricamento INTERROTTO";
                                _passo.Eventi = 1;
                                _passo.Step = -1;
                                _passo.NumTentativi = NumRetry;
                                _passo.EsecuzioneInterrotta = true;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                Step(this, _stepEv);
                                Log.Debug("Lettura area lunghi => lettura interrotta: " + IndirizzoRelativo.ToString("x4"));

                                System.Threading.Thread.Sleep(5000);

                                SkipOver = true;
                                break;
                            }
                        }
                    }

                }

                TimeSpan Tempo = DateTime.Now.Subtract(Inizio);
                Log.Debug("Lettura area lunghi => Elapsed: " + Tempo.TotalMilliseconds.ToString() + " ( " + NumPacchetto.ToString() + " )");

                Inizio = DateTime.Now;
                if (!SkipOver)
                {
                    // ora inizio lo spacchettamento
                    AnalizzaAreaLunghi(ContatoriLL.CntCariche, ContatoriLL.PntNextCarica, true, RunAsinc);
                }

                return FineLettura;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

        public bool LeggiBloccoBrevi(UInt32 StartAddr, ushort NumRows , uint IdCiclo , ref byte[] AreaBrevi, bool RunAsinc = false)
        {
            try
            {
                bool _esito;

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

                Log.Debug("Inizio lettura brevi lungo" + IdCiclo.ToString());
                DateTime Inizio = DateTime.Now;

               
                uint AreaSize = (uint)(NumRows * LEN_RECORD_BREVE);
                AreaBrevi = new byte[AreaSize];

                uint DimPacchetto = 240;
                uint LenPacchetto = 240;
                int NumPacchetto = 0;
                byte[] TmpBuffer = new byte[LenPacchetto];
                bool FineLettura = false;
                //uint AddrBase = ADDR_START_RECORD_BREVI + (StartAddr * LEN_RECORD_BREVE);
                uint IndirizzoRelativo = 0;

                while (!FineLettura)
                {
                    if ((AreaSize - IndirizzoRelativo) >= LenPacchetto)
                    {
                        DimPacchetto = LenPacchetto;
                    }
                    else
                    {
                        DimPacchetto = (uint)(AreaSize - IndirizzoRelativo);
                    }

                    uint TempAddr = (uint)((StartAddr * LEN_RECORD_BREVE) + IndirizzoRelativo);
                    _esito = LeggiBloccoMemoria(TempAddr, (ushort)DimPacchetto, out TmpBuffer);
                    if (_esito)
                    {
                        for (int _ciclo = 0; _ciclo < DimPacchetto; _ciclo++)
                        {
                            AreaBrevi[IndirizzoRelativo + _ciclo] = TmpBuffer[_ciclo];
                        }
                        IndirizzoRelativo += DimPacchetto;
                        NumPacchetto++;
                        if (IndirizzoRelativo >= AreaSize)
                        {
                            FineLettura = true;
                        }
                        if (RunAsinc)
                        {
                            elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                            int _progress = 0;
                            double _valProgress = 0;
                            _passo.TipoDati = elementiComuni.tipoMessaggio.AreaMemBreveLL;
                            _passo.Titolo = "";
                            _passo.Eventi = LEN_AREA_RECORD_LUNGHI;
                            _passo.Step = (int)IndirizzoRelativo;
                            if (_passo.Eventi > 0)
                            {
                                _valProgress = (_passo.Step * 100) / _passo.Eventi;
                            }
                            _progress = (int)_valProgress;

                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                            Step(this, _stepEv);
                        }

                    }
                    else
                    {
                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                        _passo.Titolo = "Fase 2 - Caricamento Eventi Brevi";
                        _passo.Eventi = 1;
                        _passo.Step = -1;
                        _passo.EsecuzioneInterrotta = true;
                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                        Step(this, _stepEv);
                        Log.Debug("Lettura area lunghi => lettura interrotta: " + IndirizzoRelativo.ToString("x4"));
                        break;
                    }

                }

                TimeSpan Tempo = DateTime.Now.Subtract(Inizio);
                Log.Debug("Lettura area lunghi => Elapsed: " + Tempo.TotalMilliseconds.ToString() + " ( " + NumPacchetto.ToString() + " )");

                Inizio = DateTime.Now;


                // ora inizio lo spacchettamento
                // AnalizzaAreaLunghi(ContatoriLL.CntCariche, ContatoriLL.PntNextCarica);


                return FineLettura;

            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }

        }

        public bool AnalizzaAreaLunghi(uint UltimaCarica,uint PntProssimaCarica,bool CaricaBrevi, bool RunAsync )
        {
            bool _esito = false;
            try
            {
                ushort SizeCharge = Memoria.MappaCorrente.RecordLunghi.SizeMsgDati;

                MemoriaCicli = new List<llMemoriaCicli>();
                byte[] _tempCarica = new byte[SizeCharge];
                int posizione = 0;
                
                if (PntProssimaCarica < 1) return false;
                UInt32 AddrUltimaCarica = ((PntProssimaCarica - 1) * SizeCharge) ;
                bool DatiValidi = true;
                uint AddrLocale;
                byte[] AreaBrevi = null;
                bool EsitoBrevi;
                while (DatiValidi)
                {
                    for(int _cnt = 0; _cnt < SizeCharge; _cnt++)
                    {
                        AddrLocale = (uint)((AddrUltimaCarica + _cnt));
                        _tempCarica[_cnt] = BloccoLunghi[AddrLocale];
                    }
                    if(AddrUltimaCarica < SizeCharge)
                    {
                        if (AddrUltimaCarica == 0 && (MemoriaCicli.Count+1) >= UltimaCarica)
                        {
                            DatiValidi = false;
                        }
                        //se il puntatore inizliale divanta < 0, riparto dalla fine a meno che abbia raggiunto il conteggio
                        AddrUltimaCarica = (UInt32)((Memoria.MappaCorrente.RecordLunghi.NumPagine * 0x1000 )- SizeCharge) ;
                    }
                    AddrUltimaCarica -= SizeCharge;

                    llMemoriaCicli TempCarica = AnalizzaDatiCiclo(_tempCarica);
                    if (TempCarica == null)
                    {
                        DatiValidi = false;
                    }
                    else
                    {
                        // TempCarica.id
                        if (TempCarica.IdMemoriaLunga == 0xFFFFFFFF)
                        {
                            DatiValidi = false;
                        }
                        else
                        {
                            TempCarica.Posizione = posizione;                         
                            posizione++;

                            if (CaricaBrevi)
                            {
                                //EsitoBrevi = LeggiBloccoBrevi(TempCarica.PuntatorePrimoBreve, (ushort)TempCarica.NumEventiBrevi, TempCarica.IdMemoriaLunga, ref AreaBrevi, RunAsync);
                               // if (EsitoBrevi)
                               // {

                               // }
                            }

                            MemoriaCicli.Add(TempCarica);
                            TempCarica.salvaDati();
                            _esito = true;
                        }

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

        public bool AnalizzaAreaBrevi(ushort NumRows, uint IdCiclo,byte[] AreaBrevi,ref List<llMemBreve> CicliMemBreve)
        {
            bool _esito = false;
            try
            {

                //private System.Collections.Generic.List<_llMemBreve> CicliMemBreveDB = new System.Collections.Generic.List<_llMemBreve>();
                //public System.Collections.Generic.List<llMemBreve> CicliMemoriaBreve = new System.Collections.Generic.List<llMemBreve>();
                ushort SizeCharge = Memoria.MappaCorrente.RecordLunghi.SizeMsgDati;

                CicliMemBreve = new List<llMemBreve>();
                byte[] _tempBreve = new byte[SizeCharge];

                int posizione = 0;

                bool DatiValidi = true;
                uint AddrLocale;
                bool EsitoBrevi = false;
                
                while (DatiValidi)
                {


                    llMemoriaCicli TempCarica = null; //= AnalizzaDatiCiclo(_tempCarica);
                    if (TempCarica == null)
                    {
                        DatiValidi = false;
                    }
                    else
                    {
                        if (TempCarica.IdMemoriaLunga == 0xFFFFFFFF)
                        {
                            DatiValidi = false;
                        }
                        else
                        {
                            TempCarica.Posizione = posizione;
                            posizione++;


                            //EsitoBrevi = LeggiBloccoBrevi(TempCarica.PuntatorePrimoBreve, (ushort)TempCarica.NumEventiBrevi, TempCarica.IdMemoriaLunga, ref AreaBrevi, RunAsinc);
                            if (EsitoBrevi)
                            {

                            }


                            MemoriaCicli.Add(TempCarica);
                            _esito = true;
                        }

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

        public bool ScriviDatiCliente()
        {
            try
            {
                MessaggioLadeLight.MessaggioDatiCliente MsgDatiCli = new MessaggioLadeLight.MessaggioDatiCliente(DatiCliente);
                bool _esito = false;
                if (MsgDatiCli.GeneraByteArray())
                {

                    _esito = CancellaBlocco4K(0x1000);
                    byte[] _datiTemp = MsgDatiCli.dataBuffer;
                    _esito = ScriviBloccoMemoria(0x1000, 236, _datiTemp);
                }

                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return false;
            }

        }

        public bool LeggiDatiCliente(bool RunAsinc = false, int StepIniziale = 0, int StepFinale = 100)
        {
            try
            {
                DatiCliente = new llDatiCliente(DbAttivo);
                DatiCliente.IdApparato = ParametriApparato.IdApparato;
                DatiCliente.IdCliente = 1; // al momento è fisso


                MessaggioLadeLight.MessaggioDatiCliente MsgDatiCli = new MessaggioLadeLight.MessaggioDatiCliente();
                bool _esito = false;
                SerialMessage.EsitoRisposta EsitoMsg;

                byte[] _datiTemp = new byte[236];
                _esito = LeggiBloccoMemoria(0x001000, 236, out _datiTemp);
                if (RunAsinc)
                {
                    //Preparo l'intestazione della finestra di avanzamento                                                                                                          
                    if (Step != null)
                    {
                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                        _passo.Titolo = "Caricamento Dati Cliente";
                        _passo.Eventi = 1;
                        _passo.Step = -1;
                        _passo.EsecuzioneInterrotta = false;
                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(StepFinale, _passo);
                        Step(this, _stepEv);
                    }

                }
                if (_esito)
                {
                    EsitoMsg = MsgDatiCli.analizzaMessaggio(_datiTemp, 1);
                    if (EsitoMsg == SerialMessage.EsitoRisposta.MessaggioOk)
                    {
                        DatiCliente.Client = MsgDatiCli.Cliente;
                        DatiCliente.Description = MsgDatiCli.Descrizione;
                        DatiCliente.Note = MsgDatiCli.Note;
                        DatiCliente.LocalId = MsgDatiCli.IdLocale;
                        DatiCliente.LocalName = MsgDatiCli.NomeLocale;

                        DatiCliente.salvaDati();

                    }
                    else
                        _esito = false;

                }

                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return false;
            }

        }

        public bool LeggiMemoriaCicliDB(string IdApparato)
        {
            try
            {
                MemoriaCicli = new List<llMemoriaCicli>();

                IEnumerable<_llMemoriaCicli> _TempCicli = DbAttivo.Query<_llMemoriaCicli>("select * from _llMemoriaCicli where IdApparato = ? order by IdMemCiclo desc", IdApparato);

                foreach (_llMemoriaCicli Elemento in _TempCicli)
                {
                    llMemoriaCicli _cLoc;
                    _cLoc = new llMemoriaCicli(DbAttivo, Elemento);
                    MemoriaCicli.Add(_cLoc);

                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return false;
            }


        }


        public bool LeggiDatiCompleti(bool RunAsynk)
        {
            try
            {
                DateTime Inizio = DateTime.Now;

                Log.Error(" ------------------------ ");
                Log.Error(" - CB.LeggiDatiCompleti - ");
                Log.Error(" ------------------------ ");

                bool _esito;
                Log.Error("START: " +  FunzioniMR.SecondiTrascorsi(Inizio));

                _esito = CaricaApparatoA0(RunAsynk, 0, 2);
                Log.Error("CaricaApparatoA0: " + FunzioniMR.SecondiTrascorsi(Inizio));
                if (!_esito)
                {
                    UltimoMsgErrore = "Fallito Caricamento area A0";
                    return false;
                }

                _esito = CaricaAreaContatori(RunAsynk, 2, 3);
                Log.Error("CaricaAreaContatori: " + FunzioniMR.SecondiTrascorsi(Inizio));
                if (!_esito)
                {
                    UltimoMsgErrore = "Fallito Caricamento area Contatori";
                    return false;
                }


                _esito = LeggiDatiCliente(RunAsynk, 3, 5);
                Log.Error("LeggiDatiCliente: " + FunzioniMR.SecondiTrascorsi(Inizio));
                if (!_esito)
                {
                    UltimoMsgErrore = "Fallito Caricamento Dati Cliente";
                    return false;
                }

                _esito = LeggiProgrammazioni(RunAsynk, 5, 21);
                Log.Error("LeggiProgrammazioni: " + FunzioniMR.SecondiTrascorsi(Inizio));
                if (!_esito)
                {
                    UltimoMsgErrore = "Fallito Caricamento area Programmazioni";
                    return false;
                }

                _esito = LeggiBloccoLunghi(RunAsynk,21,100);
                Log.Error("LeggiBloccoLunghi: " + FunzioniMR.SecondiTrascorsi(Inizio));
                if (!_esito)
                {
                    UltimoMsgErrore = "Fallito Caricamento area Cariche";
                    return false;
                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("LeggiDatiCompleti: " + Ex.Message);
                return false;
            }

        }



        public bool CaricaBloccoDati(AreaDatiRegen BloccoAttivo,bool RunAsync)
        {
            try
            {

                uint _StartAddr = BloccoAttivo.StartAddress;
                uint _TmpAddr;
                ushort _NumPagine = (ushort)BloccoAttivo.NumBlocchi;
                bool _esito;
                byte[] _dataArray;
                int _numBytes;
                int _cicliCompleti;
                int _byteResidui;
                byte[] _tempBuffer;
                int StepIniziale = 0;
                // txtMemRegenNumBlocchi.Text = BloccoAttivo.IdBlocco.ToString();

                _numBytes = _NumPagine * 0x1000;

                _dataArray = new byte[_numBytes];
                BloccoAttivo.Data = new byte[_numBytes];

                _cicliCompleti = _numBytes / DATABLOCK;
                _byteResidui = _numBytes % DATABLOCK;

                if (RunAsync)
                {
                    //Preparo l'intestazione della finestra di avanzamento                                                                                                          
                    if (Step != null)
                    {
                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                        _passo.Titolo = "Lettura Memoria ";
                        _passo.Eventi = 1;
                        _passo.Step = -1;
                        _passo.EsecuzioneInterrotta = false;
                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(StepIniziale, _passo);
                        Step(this, _stepEv);
                    }

                }


                // lblMemRegenAvanzamentoRead.Text = _cicliCompleti.ToString() + " pacchetti + " + _byteResidui.ToString() + " bytes";

                // Leggo prima i pacchetti interi
                _tempBuffer = new byte[DATABLOCK];

                for (int _step = 0; _step < _cicliCompleti; _step++)
                {
                    _TmpAddr = (uint)(_step * DATABLOCK);
                    _esito = LeggiBloccoMemoria(_StartAddr + _TmpAddr, DATABLOCK, out _tempBuffer);
                    if (_esito)
                    {
                        // Pacchetto letto con successo, accodo i dati
                        for (int _ii = 0; _ii < DATABLOCK; _ii++)
                        {
                            _dataArray[_TmpAddr + _ii] = _tempBuffer[_ii];
                        }
                        //lblMemRegenAvanzamentoRead.Text = _step.ToString() + " di " + _cicliCompleti.ToString();
                        //Application.DoEvents();

                        if (RunAsync)
                        {
                            //Preparo l'intestazione della finestra di avanzamento                                                                                                          
                            if (Step != null)
                            {
                                elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = "Lettura Memoria ";
                                _passo.Eventi = 1;
                                _passo.Step = -1;
                                _passo.EsecuzioneInterrotta = false;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_step, _passo);
                                Step(this, _stepEv);
                            }

                        }
                    }
                    else
                    {
                        //MessageBox.Show("Errore lettura pacchetto " + _step.ToString(), "Esportazione dati", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }



                // Ora Leggo il residuo
                if (_byteResidui > 0)
                {
                    _tempBuffer = new byte[_byteResidui];


                    _TmpAddr = (uint)(_cicliCompleti * DATABLOCK);
                    _esito = LeggiBloccoMemoria(_StartAddr + _TmpAddr, (ushort)_byteResidui, out _tempBuffer);
                    if (_esito)
                    {
                        // Pacchetto letto con successo, accodo i dati
                        for (int _ii = 0; _ii < _byteResidui; _ii++)
                        {
                            _dataArray[_TmpAddr + _ii] = _tempBuffer[_ii];
                        }
                        // lblMemRegenAvanzamentoRead.Text = "Pacchetto Finale";
                        // Application.DoEvents();

                    }
                    else
                    {
                        //MessageBox.Show("Errore lettura pacchetto finale ", "Esportazione dati", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }


                }

                Log.Debug("--- Carica Immagine -------------------");
                // Log.Debug(FunzioniMR.hexdumpArray(_dataArray));
                //ora salvo l'immagine
                BloccoAttivo.Data = _dataArray;
                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
                return false;
            }


        }


        public  bool ScriviBloccoDati(AreaDatiRegen BloccoAttivo, bool RunAsync)
        {
            try
            {

                uint _StartAddr = BloccoAttivo.StartAddress;
                uint _TmpAddr;
                ushort _NumByte;
                ushort _NumPagine = (ushort)BloccoAttivo.NumBlocchi;
                bool _esito;
                byte[] _dataArray;
                int _numBytes;
                int _cicliCompleti;
                int _byteResidui;
                byte[] DataBuffer;
                byte[] _tempBuffer;

                // Prima vuoto la memoria

                _numBytes = _NumPagine * 0x1000;

                if (_NumPagine > 0)
                {
                    int _bloccoCorrente;
                    _TmpAddr = _StartAddr;
                    for (int _cicloBlocchi = 0; _cicloBlocchi < _NumPagine; _cicloBlocchi++)
                    {
                        _bloccoCorrente = _cicloBlocchi + 1;
                        _esito = CancellaBlocco4K(_TmpAddr);
                        if (!_esito)
                        {
                            // MessageBox.Show("Cancellazione del blocco " + _bloccoCorrente.ToString() + " non riuscita", "Cancellazione dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                        {
                            _TmpAddr += 0x1000;
                            //lblMemRegenAvanzamentoWrite.Text = "Cancellazione pagina " + _bloccoCorrente.ToString();

                           // Application.DoEvents();
                        }
                    }

                }
                else
                {
                    return false;
                }

                DataBuffer = BloccoAttivo.Data;


                uint _stepSent = 0;
                uint _posizione = 0;
                uint _datiTrasferiti = 0;
                byte[] _Tempbuffer = new byte[DATABLOCK];
                _TmpAddr = _StartAddr;
                while ((_numBytes - _datiTrasferiti) > DATABLOCK)
                {

                    for (int _blockStep = 0; _blockStep < DATABLOCK; _blockStep++)
                    {
                        _Tempbuffer[_blockStep] = DataBuffer[_posizione];
                        _posizione++;
                        _datiTrasferiti++;
                    }
                    _esito = ScriviBloccoMemoria(_TmpAddr, (ushort)DATABLOCK, _Tempbuffer);

                    _TmpAddr += DATABLOCK;
                    _stepSent++;

                    //lblMemRegenAvanzamentoWrite.Text = "Pacchetto " + _stepSent.ToString();
                    //Application.DoEvents();

                }


                // Ora trasmetto il residuo
                int _residuo = (int)(_numBytes - _datiTrasferiti);
                _Tempbuffer = new byte[_residuo];
                for (int _blockStep = 0; _blockStep < _residuo; _blockStep++)
                {
                    _Tempbuffer[_blockStep] = DataBuffer[_posizione];
                    _posizione++;
                    _datiTrasferiti++;
                }
                _esito = ScriviBloccoMemoria(_TmpAddr, (ushort)_residuo, _Tempbuffer);
                _stepSent++;

                // lblMemRegenAvanzamentoWrite.Text = "Pacchetto " + _stepSent.ToString();
                // Application.DoEvents();


                // lblMemRegenAvanzamentoWrite.Text = _NumSequenze.ToString() + " pagine inserite";
                // lblMemRegenAvanzamentoWrite.Visible = true;
                //             Application.DoEvents();

                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
                return false;
            }

        }

    }
}
