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


        public llMemoriaCicli AnalizzaDatiCiclo(byte[] ArrayDati)
        {
            try
            {

                if (ArrayDati.Length != SizeCharge)
                {
                    // Array di dimensioni errate. non è una carica. Esco prima di iniziare
                    return null;
                }

                bool _esito = false;
                llMemoriaCicli tempPrg = new llMemoriaCicli();
                MessaggioLadeLight.MessaggioMemoriaLunga ImmagineCarica = new MessaggioLadeLight.MessaggioMemoriaLunga();
                SerialMessage.EsitoRisposta EsitoMsg;

                EsitoMsg = ImmagineCarica.analizzaMessaggio(ArrayDati, 1);
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

        public bool LeggiBloccoLunghi(bool RunAsinc = false)
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



                Log.Debug("Inizio lettura ara lunghi");
                DateTime Inizio = DateTime.Now;
                BloccoLunghi = new byte[LEN_AREA_RECORD_LUNGHI];

                int DimPacchetto = 240;
                int LenPacchetto = 240;
                int NumPacchetto = 0;
                byte[] TmpBuffer = new byte[LenPacchetto];
                bool FineLettura = false;
                int IndirizzoRelativo = 0;

                while (!FineLettura)
                {
                    if ((LEN_AREA_RECORD_LUNGHI - IndirizzoRelativo) >= LenPacchetto)
                    {
                        DimPacchetto = LenPacchetto;
                    }
                    else
                    {
                        DimPacchetto = LEN_AREA_RECORD_LUNGHI - IndirizzoRelativo;
                    }
                    uint TempAddr = (uint)(ADDR_START_RECORD_LUNGHI + IndirizzoRelativo);
                    _esito = LeggiBloccoMemoria(TempAddr, (ushort)DimPacchetto, out TmpBuffer);
                    if (_esito)
                    {
                        for (int _ciclo = 0; _ciclo < DimPacchetto; _ciclo++)
                        {
                            BloccoLunghi[IndirizzoRelativo + _ciclo] = TmpBuffer[_ciclo];
                        }
                        IndirizzoRelativo += DimPacchetto;
                        NumPacchetto++;
                        if (IndirizzoRelativo >= LEN_AREA_RECORD_LUNGHI)
                        {
                            FineLettura = true;
                        }
                        if (RunAsinc)
                        {
                            elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                            int _progress = 0;
                            double _valProgress = 0;
                            _passo.TipoDati = elementiComuni.tipoMessaggio.AreaMemLungaLL;
                            _passo.Titolo = "";
                            _passo.Eventi = LEN_AREA_RECORD_LUNGHI;
                            _passo.Step = IndirizzoRelativo;
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
                AnalizzaAreaLunghi(ContatoriLL.CntCariche, ContatoriLL.PntNextCarica, true, RunAsinc);


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

                MemoriaCicli = new List<llMemoriaCicli>();
                byte[] _tempCarica = new byte[SizeCharge];
                int posizione = 0;
                
                if (PntProssimaCarica < 1) return false;
                UInt32 AddrUltimaCarica = (((PntProssimaCarica - 1) * SizeCharge) % 0x4000 );
                bool DatiValidi = true;
                uint AddrLocale;
                byte[] AreaBrevi = null;
                bool EsitoBrevi;
                while (DatiValidi)
                {
                    for(int _cnt = 0; _cnt < SizeCharge; _cnt++)
                    {
                        AddrLocale = (uint)((AddrUltimaCarica + _cnt) % 0x4000);
                        _tempCarica[_cnt] = BloccoLunghi[AddrLocale];
                    }
                    if(AddrUltimaCarica < SizeCharge)
                    {
                        //se il puntatore inizliale divanta < 0, riparto dalla fine
                        AddrUltimaCarica += 0x4000;
                    }
                    AddrUltimaCarica -= SizeCharge;

                    llMemoriaCicli TempCarica = AnalizzaDatiCiclo(_tempCarica);
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

                            if (CaricaBrevi)
                            {
                                //EsitoBrevi = LeggiBloccoBrevi(TempCarica.PuntatorePrimoBreve, (ushort)TempCarica.NumEventiBrevi, TempCarica.IdMemoriaLunga, ref AreaBrevi, RunAsync);
                               // if (EsitoBrevi)
                               // {

                               // }
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

        public bool AnalizzaAreaBrevi(ushort NumRows, uint IdCiclo,byte[] AreaBrevi,ref List<llMemBreve> CicliMemBreve)
        {
            bool _esito = false;
            try
            {

                //private System.Collections.Generic.List<_llMemBreve> CicliMemBreveDB = new System.Collections.Generic.List<_llMemBreve>();
                //public System.Collections.Generic.List<llMemBreve> CicliMemoriaBreve = new System.Collections.Generic.List<llMemBreve>();

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



    }
}
