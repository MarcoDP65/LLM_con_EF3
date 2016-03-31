using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using MoriData;
using Utility;
using log4net;
using log4net.Config;
using PannelloCharger;

namespace ChargerLogic
{
    public class StatMemLungaSB
    {
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public System.Collections.Generic.List<sbMemLunga> CicliMemoriaLunga = new System.Collections.Generic.List<sbMemLunga>();
        public System.Collections.Generic.List<sbProgrammaRicarica> Programmazioni = new System.Collections.Generic.List<sbProgrammaRicarica>();
        public System.Collections.Generic.List<_StatMemLungaSB> CicliStatMemLunga = new System.Collections.Generic.List<_StatMemLungaSB>();
        public System.Collections.Generic.List<_StatMemLungaSB> MacrofasiScarica = new System.Collections.Generic.List<_StatMemLungaSB>();
        public System.Collections.Generic.List<_StatCicloSBPeriodo> CicliStatPeriodo = new System.Collections.Generic.List<_StatCicloSBPeriodo>();
        public System.Collections.Generic.List<_sbSoglie> SoglieStatSys = new System.Collections.Generic.List<_sbSoglie>();
        public System.Collections.Generic.List<SettimanaMR> SettimanePresenti = new System.Collections.Generic.List<SettimanaMR>();

        public sbSetSoglie SoglieAnalisi;

        public DatiEstrazione DatiCorrenti;

        #region Variabili Soglia

        int _profonditaDoD;
        int _tMaxScarica;
        int _tMinScarica;
        int _deltaTScarica;
        int _tMaxCaricaComp;
        int _tMaxCaricaParz;
        int _deltaTCaricaComp;
        int _deltaTCaricaParz;
        double _maxSbilanciamento;
        double _minFC;
        int _orePausaDODFascia810;
        int _orePausaDODFascia68;
        int _orePausaDODFascia46;
        int _orePausaDODFascia24;
        int _orePausaDODFascia02;

        int _macroScariche;
        int _totDodScariche;




        #endregion


        private bool _datiPronti = false;
        private sbProgrammaRicarica _lastProg ;
        private int _numCariche;
        private int _numCaricheComplete;
        private int _numCaricheParziali;
        private int _numScariche;
        private int _numPause;
        private int _numAnomali;
        private int _numCicliTot;
        private int _numCicliEff;
        private int _numSovraScariche;
        private int _numPauseScarica;
        private Int32 _kWhtot;
        private Int32 _kWhCaricati;
        private Int32 _Ahtot;
        private double _KWhmediCiclo;
        private double _AhmediCiclo;
        private uint _durataCarica;

        private int _numScaricaOver;
        private int _numCaricaCOver;
        private int _numCaricaPOver;

        private uint _durataScarica;
        private uint _durataPause;
        private uint _durataNoEl;
        private uint _durataSbil;
        private uint _durataOverTempMax;

        private uint _durataFasiAttive;

        private DateTime _dtStart;
        private DateTime _dtFine;
        private int _numGiorni;
        private double _numSecondi;
        private int _cicliAttesi = 1500;
        private int _offsetSettimane = 0;
        private double _EnScaricataNorm = 0;
        private int _totaleScaricato;
        private double _maxSbil;

        private int _numFasiAttive = 0;
        private int _mancanzaElFasiAttive = 0;
        private int _numMacrofasiScarica = 0;
        private int _macroSovrascariche = 0;
        private DateTime _ultimaLettura = DateTime.MinValue;


        public StatMemLungaSB()
        {
            try
            {
                CicliMemoriaLunga.Clear();
                CicliStatMemLunga.Clear();
                CicliStatPeriodo.Clear();
                MacrofasiScarica.Clear();
                SettimanePresenti.Clear();
                azzeraValori();

            }
            catch
            {
            }

        }

        public StatMemLungaSB(System.Collections.Generic.List<sbMemLunga> CicliMemLunga)
        {
            try
            {
                _datiPronti = false;
                CicliMemoriaLunga.Clear();
                CicliStatMemLunga.Clear();
                CicliStatPeriodo.Clear();
                MacrofasiScarica.Clear();
                SettimanePresenti.Clear();
                azzeraValori();

                CicliMemoriaLunga = CicliMemLunga.OrderBy(x => x.IdMemoriaLunga).ToList();
            }
            catch
            {
            }

        }

        /// <summary>
        /// Carica i valori di soglia, assegnando un default se non impostato diversamente tramite il parametro pubblico SoglieAnalisi
        /// </summary>
        /// <returns></returns>
        public bool caricaSoglie()
        {
            _profonditaDoD = 20;              // 1
            _tMaxScarica = 50;                // 2
            _tMinScarica = 0;                 // 3
            _deltaTScarica = 12;              // 4
            _tMaxCaricaComp = 50;             // 5
            _tMaxCaricaParz = 50;             // 6
            _deltaTCaricaComp = 12;           // 7
            _deltaTCaricaParz = 12;           // 8
            _maxSbilanciamento = 0.05;        // 9
            _minFC = 1;                       // 10
            _orePausaDODFascia810 = 5;        // 11
            _orePausaDODFascia68 = 20;        // 12
            _orePausaDODFascia46 = 72;        // 13
            _orePausaDODFascia24 = 480;       // 14
            _orePausaDODFascia02 = 480;       // 15


            if (SoglieAnalisi != null)
            {
                foreach (sbSoglia _sgl in SoglieAnalisi.PacchettoSoglie)
                {
                    switch(_sgl.IdMisura)
                    {
                        case 1: //Profondità DoD
                            _profonditaDoD = 100 - _sgl.ValoreInt;
                            break;
                        case 2: //Tmax Scarica  
                            _tMaxScarica = _sgl.ValoreInt;
                            break;
                        case 3: //Tmin Scarica  
                            _tMinScarica = _sgl.ValoreInt;
                            break;
                        case 4: //Diff T Scarica  
                            _deltaTScarica = _sgl.ValoreInt;
                            break;
                        case 5: //Tmax Carica Completa
                            _tMaxCaricaComp =  _sgl.ValoreInt;
                            break;
                        case 6: //Tmax Carica Parziale
                            _tMaxCaricaParz = _sgl.ValoreInt;
                            break;
                        case 7: //Diff T Carica Completa
                            _deltaTCaricaComp = _sgl.ValoreInt;
                            break;
                        case 8: //Diff T Carica Parziale
                            _deltaTCaricaParz = _sgl.ValoreInt;
                            break;
                        case 9: //Profondità DoD
                            _maxSbilanciamento =  _sgl.ValoreNum;
                            break;
                        case 10: //Min CF
                            _minFC = _sgl.ValoreInt;
                            break;
                        case 11: //Diff T Carica Parziale
                            _orePausaDODFascia810 = _sgl.ValoreInt;
                            break;
                        case 12: //Diff T Carica Parziale
                            _orePausaDODFascia68 = _sgl.ValoreInt;
                            break;
                        case 13: //Diff T Carica Parziale
                            _orePausaDODFascia46 = _sgl.ValoreInt;
                            break;
                        case 14: //Diff T Carica Parziale
                            _orePausaDODFascia24 = _sgl.ValoreInt;
                            break;
                        case 15: //Diff T Carica Parziale
                            _orePausaDODFascia02 = _sgl.ValoreInt;
                            break;

                    }

                }
            }
            return true;
        
        }

        public bool azzeraValori()
        {
            _lastProg = new sbProgrammaRicarica();
            _lastProg.IdProgramma = 0;
            _datiPronti = false;
            _dtStart = Convert.ToDateTime("01/01/3000 00:00");
            _dtFine = Convert.ToDateTime("01/01/2000 00:00");
            _numCicliTot = 0;
            _numCicliEff = 0;
            _numCariche = 0;
            _numScariche = 0;
            _numSovraScariche = 0;
            _numPauseScarica = 0;
            _numGiorni = 0;
            _numSecondi = 0;
            _durataCarica = 0;
            _durataScarica = 0;
            _durataPause = 0;
            _numCaricheComplete = 0;
            _numCaricheParziali = 0;
            _durataNoEl = 0;
            _numPause = 0;
            _numAnomali = 0;
            _kWhtot = 0;
            _kWhCaricati = 0;
            _EnScaricataNorm = 0;
            _totaleScaricato = 0;
            _maxSbil = 0;
            _durataSbil = 0;
            _KWhmediCiclo = 0;
            _AhmediCiclo = 0;
            _numScaricaOver = 0;
            _numCaricaCOver = 0;
            _numCaricaPOver = 0;
            _numFasiAttive = 0;
            _mancanzaElFasiAttive = 0;
            _numMacrofasiScarica = 0;
            _macroScariche = 0;
            _totDodScariche = 0;
            _macroSovrascariche = 0;
            _durataFasiAttive = 0;
            _durataOverTempMax = 0;
            _ultimaLettura = DateTime.MinValue;

            DatiCorrenti = new DatiEstrazione();

            return true;
        }


        /// <summary>
        /// AggregaValori: Consolida i dati dei cicli lunghi e preaggrega i dati per le statistiche
        /// </summary>
        /// <param name="AttivaPeriodo">if set to <c>true</c> [attiva periodo].</param>
        /// <param name="Inizio">Inizio periodo osservazione</param>
        /// <param name="Fine">Fine periodo osservazione (escluso)</param>
        /// <returns></returns>
        public bool aggregaValori(bool AttivaPeriodo, DateTime Inizio,DateTime Fine)
        {

            try
            {
                CicliStatMemLunga.Clear();
                MacrofasiScarica.Clear();
                SettimanePresenti.Clear();
                DateTime _tempDT;

                string DataOraBreve;
                DateTime dtDataOraBreve;

                bool _trovatoEqual = false;

                Log.Debug(" AGGREGA VALORI ");
                // prima riordino la lista per IO ciclo (ordine cronologico): 
                System.Collections.Generic.List<sbMemLunga> _tmpCMemoriaLunga;
                _tmpCMemoriaLunga = CicliMemoriaLunga.OrderBy(x => x.IdMemoriaLunga).ToList();
                HashSet<string> _listaSett = new HashSet<string>();
                byte _statoCaricaPrec = 100 ;
                _StatMemLungaSB valMacroScarica = new _StatMemLungaSB();


                foreach (sbMemLunga ciclo in _tmpCMemoriaLunga)
                {
                    _StatMemLungaSB valCiclo = new _StatMemLungaSB();
                    _StatMemLungaSB valCicloEqual = new _StatMemLungaSB();
                    //---------------------------------------------------------------------------------------------------------------
                    // copio i valori puntuali

                    valCiclo.TipoEvento = ciclo.TipoEvento;
                    valCiclo.IdMemoriaLunga = ciclo.IdMemoriaLunga;
                    valCiclo.IdMemoriaLungaStat = ciclo.IdMemoriaLunga * 10;
                    valCiclo.IdLocale = ciclo.IdLocale;
                    valCiclo.FattoreCarica = ciclo.FattoreCarica;
                    valCiclo.StatoCarica = ciclo.StatoCarica;

                    valCiclo.Wh = (int)(ciclo.ValKWh * 100);
                    valCiclo.Ah = (int)(ciclo.ValAh);
                    valCiclo.DataOraStart = ciclo.DataOraStart;
                    valCiclo.dtDataOraStart = ciclo.dtDataOraStart;
                    valCiclo.DataOraFine = ciclo.DataOraFine;
                    valCiclo.dtDataOraFine = ciclo.dtDataOraFine;
                    // Inizializzo comunque deta e ora dell'interruzione per evitare errori di compilazione
                    DataOraBreve = ciclo.DataOraFine;
                    dtDataOraBreve = ciclo.dtDataOraFine;

                    if (ciclo.DataLastDownload > _ultimaLettura) _ultimaLettura = ciclo.DataLastDownload;

                    //Log.Debug("Ciclo n° " + valCiclo.IdMemoriaLunga.ToString() + " IN ANALISI BASE ");
                    if (AttivaPeriodo != true)
                    {
                        valCiclo.PeriodoValido = true;
                    }
                    else
                    {
                        if ((valCiclo.dtDataOraStart >= Inizio) && (valCiclo.dtDataOraFine < Fine)) valCiclo.PeriodoValido = true;
                        else valCiclo.PeriodoValido = false;

                    }

                    valCiclo.Durata = ciclo.Durata;
                    valCiclo.PresenzaElettrolita = ciclo.PresenzaElettrolita;
                    valCiclo.DurataSbilCelle = ciclo.DurataSbilCelle;
                    valCiclo.DurataMancanzaElettrolita = ciclo.DurataMancanzaElettrolita;


                    valCiclo.TempMin = ciclo.TempMin;
                    valCiclo.TempMax = ciclo.TempMax;
                    valCiclo.DeltaTemp = ciclo.TempMax - ciclo.TempMin;

                    //---------------------------------------------------------------------------------------------------------------
                    _tempDT = ciclo.dtDataOraStart;
                    if (_tempDT < _dtStart) _dtStart = _tempDT;
                    _tempDT = ciclo.dtDataOraFine;
                    if (_tempDT > _dtFine) _dtFine = _tempDT;
                    if (_dtFine >= _dtStart)
                    {
                        TimeSpan _ts = _dtFine - _dtStart;
                        _numGiorni = _ts.Days;
                        _numSecondi = _ts.TotalSeconds;


                    }


                    if (ciclo.EffMaxSbilanciamentoC > _maxSbil) _maxSbil = ciclo.VMaxSbilanciamentoC;
                    if (ciclo.IdProgramma > _lastProg.IdProgramma) _lastProg = ciclo.ProgrammaAttivo;
                    // Se lo sbilaciamento celle supera il limite, agiungo la durata al tempo totale di sbilanciamento
                    //if (ciclo.EffMaxSbilanciamentoC > _maxSbilanciamento) _durataSbil += valCiclo.Durata;
                    // Se lmanca elettrolita (valore 0x0F), agiungo la durata al tempo totale mancanza el.
                    //if (ciclo.PresenzaElettrolita == 0x0F) _durataNoEl += valCiclo.Durata;

                    switch (valCiclo.TipoEvento)
                    {
                        case (byte)SerialMessage.TipoCiclo.Carica:
                            _numFasiAttive += 1;
                            _numCicliEff += 1;
                            _numCicliTot += 1;
                            _numCariche += 1;
                            _durataFasiAttive += ciclo.Durata;
                            _durataSbil += ciclo.DurataSbilCelle;
                            _durataNoEl += ciclo.DurataMancanzaElettrolita;
                            _durataOverTempMax += ciclo.DurataOverTemp;

                            //Controllo la presenza elettrolita
                            if (ciclo.PresenzaElettrolita == 0x0F) _mancanzaElFasiAttive += 1;

                            // Verifico se esiste una fase di equalizzazione

                            // Creo una lista brevi ordinata
                            System.Collections.Generic.List<sbMemBreve> _tmpCMemoriaBreve;
                            _tmpCMemoriaBreve = ciclo.CicliMemoriaBreve.OrderBy(x => x.IdMemoriaBreve).ToList();

                            _trovatoEqual = false;
                            //Cerco il primo breve a corrente 0
                            foreach (sbMemBreve _tmpBreve in _tmpCMemoriaBreve)
                            {
                                if (_tmpBreve.Amed < 15 & _tmpBreve.Amed > -15)
                                {

                                    // trovato il primo breve
                                    int _posPrimo = _tmpCMemoriaBreve.IndexOf(_tmpBreve);
                                    // Se non sono gli ultimo brevi...
                                    if (_posPrimo < (_tmpCMemoriaBreve.Count - 6))
                                    {
                                        _trovatoEqual = true;

                                        //ora verifico se i 6 brevi successivi sono a 0
                                        for (int _idxEqual = 0; _idxEqual < 6; _idxEqual++)
                                        {
                                            sbMemBreve _tmpBreveZero = _tmpCMemoriaBreve.ElementAt(_idxEqual + _posPrimo);
                                            if ((_tmpBreve.Amed > 15 | _tmpBreve.Amed < -15))
                                            {
                                                _trovatoEqual = false;
                                                break;
                                            }


                                        }
                                    }

                                }
                                if (_trovatoEqual)
                                {
                                    DataOraBreve = _tmpBreve.DataOraRegistrazione;
                                    dtDataOraBreve = _tmpBreve.dtDataOraRegistrazione;
                                    break;
                                }
                            }

                            // Trovata  L'equalizzazione; spezzo il ciclo
                            if (_trovatoEqual)
                            {
                                valCicloEqual.TipoEvento = (byte)SerialMessage.TipoCiclo.Equal;
                                valCicloEqual.IdMemoriaLunga = valCiclo.IdMemoriaLunga;
                                valCicloEqual.IdMemoriaLungaStat = valCiclo.IdMemoriaLunga * 10;
                                valCicloEqual.IdLocale = valCiclo.IdLocale;
                                valCicloEqual.FattoreCarica = valCiclo.FattoreCarica;
                                valCicloEqual.StatoCarica = valCiclo.StatoCarica;
                                valCicloEqual.PeriodoValido = valCiclo.PeriodoValido;
                                valCicloEqual.Wh = valCiclo.Wh;
                                valCicloEqual.Ah = valCiclo.Ah;
                                valCicloEqual.PresenzaElettrolita = valCiclo.PresenzaElettrolita;
                                valCicloEqual.TempMin = valCiclo.TempMin;
                                valCicloEqual.TempMax = valCiclo.TempMax;
                                valCicloEqual.DeltaTemp = valCicloEqual.TempMax - valCicloEqual.TempMin;

                                // uso la data del breve per impostare le durate
                                valCicloEqual.DataOraFine = valCiclo.DataOraFine;
                                valCicloEqual.dtDataOraFine = valCiclo.dtDataOraFine;
                                valCicloEqual.DataOraStart = DataOraBreve;
                                valCiclo.DataOraFine = DataOraBreve;
                                valCicloEqual.dtDataOraStart = dtDataOraBreve;
                                valCiclo.dtDataOraFine = dtDataOraBreve;
                                TimeSpan _tsC = valCiclo.dtDataOraFine - valCiclo.dtDataOraStart;
                                TimeSpan _tsE = valCicloEqual.dtDataOraFine - valCicloEqual.dtDataOraStart;

                                valCiclo.Durata = (UInt32)_tsC.TotalSeconds;
                                valCicloEqual.Durata = (UInt32)_tsE.TotalSeconds;

                            }



                            _durataCarica += valCiclo.Durata;
                            if (valCiclo.FattoreCarica >= 100)
                            {
                                _numCaricheComplete += 1;
                                valCiclo.ModoCarica = 1;
                                if (valCiclo.TempMax >= _tMaxCaricaComp) _numCaricaCOver += 1;
                            }
                            else
                            {
                                _numCaricheParziali += 1;
                                if (valCiclo.TempMax >= _tMaxCaricaParz) _numCaricaPOver += 1;

                                valCiclo.ModoCarica = 0;
                            }

                            // imposto lo ststo carica iniziale pari allo stato carica del ciclo precedente
                            valCiclo.StatoCaricaIniziale = _statoCaricaPrec;
                            _statoCaricaPrec = ciclo.StatoCarica;
                            _kWhCaricati += valCiclo.Wh;
                            _numMacrofasiScarica  += 1;  //Ogni carica chiude una macroscarica
                            _macroScariche += 1;  //Ogni carica chiude una macroscarica
                            _totDodScariche += valCiclo.StatoCaricaIniziale; //Determino la profondità di scarica dal livello di carica all'inizio della carica
                            if (valCiclo.StatoCaricaIniziale < 20 )  _macroSovrascariche += 1;
                            //_totaleScaricato += (valCiclo.StatoCaricaIniziale - valCiclo.StatoCarica);

                                // ad ogni carica creo la relativa MacroScarica con profindità pari all'inizio prrecedente


                            break;
                        case (byte)SerialMessage.TipoCiclo.Scarica:
                            double _tmed;
                            _numFasiAttive += 1;
                            _numCicliTot += 1;
                            _numScariche += 1;
                            if (ciclo.PresenzaElettrolita == 0x0F) _mancanzaElFasiAttive += 1;

                            if (valCiclo.StatoCarica <= _profonditaDoD) _numSovraScariche += 1;
                            _kWhtot += valCiclo.Wh;
                            //TODO: Risistemare con AH caricati e scaricati
                            _Ahtot += valCiclo.Ah;
                            _durataScarica += valCiclo.Durata;
                            _tmed = (double)(valCiclo.TempMax + valCiclo.TempMin) / 2;

                            if (valCiclo.TempMax >= _tMaxScarica) _numScaricaOver += 1;

                            _durataFasiAttive += ciclo.Durata;
                            _durataSbil += ciclo.DurataSbilCelle;
                            _durataNoEl += ciclo.DurataMancanzaElettrolita;
                            _durataOverTempMax += ciclo.DurataOverTemp;


                            _EnScaricataNorm += valCiclo.Wh * FunzioniAnalisi.FattoreTermicoSOH(_tmed);
                            // imposto lo ststo carica iniziale pari allo stato carica del ciclo precedente
                            valCiclo.StatoCaricaIniziale = _statoCaricaPrec;
                            _statoCaricaPrec = ciclo.StatoCarica;

                            _totaleScaricato += (valCiclo.StatoCaricaIniziale - valCiclo.StatoCarica);
                            break;
                        case (byte)SerialMessage.TipoCiclo.Pausa:
                            _numCicliTot += 1;
                            _numPause += 1;
                            if (valCiclo.StatoCarica <= 70) _numPauseScarica += 1;
                            _kWhtot += valCiclo.Wh;
                            _durataPause += valCiclo.Durata;


                            break;
                        default:
                            // se tipo ciclo non atalogato, lo conto nelle anomalie ma non nel totale
                            _numAnomali += 1;
                            break;

                    }

                    CicliStatMemLunga.Add(valCiclo);
                    if (_trovatoEqual)
                    {
                        CicliStatMemLunga.Add(valCicloEqual);
                    }
                }

                foreach(_StatMemLungaSB valCiclo in CicliStatMemLunga)
                { 

                    //-----------------------------------------------------------------------------------
                    //  DATI PER GRAFICO TEMPORALE
                    //-----------------------------------------------------------------------------------
                    if (valCiclo.PeriodoValido)
                    {
                        //Log.Debug("Ciclo n° " + valCiclo.IdMemoriaLunga.ToString() + " IN ANALISI ");
                        _StatCicloSBPeriodo valPeriodo = new _StatCicloSBPeriodo();
                        
                        valPeriodo.PeriodoTemporale = new PeriodoMR();
                        valPeriodo.SettimanaRiferimento = new SettimanaMR();
                        valPeriodo.TipoEvento = valCiclo.TipoEvento;
                        valPeriodo.IdMemoriaLunga = valCiclo.IdMemoriaLunga;
                        valPeriodo.IdLocale = valCiclo.IdLocale;
                        valPeriodo.FattoreCarica = valCiclo.FattoreCarica;
                        valPeriodo.StatoCarica = valCiclo.StatoCarica;
                        valPeriodo.Wh = valCiclo.Wh;
                        valPeriodo.DataOraStart = valCiclo.DataOraStart;
                        valPeriodo.dtDataOraStart = valCiclo.dtDataOraStart;
                        valPeriodo.DataOraFine = valCiclo.DataOraFine;
                        valPeriodo.dtDataOraFine = valCiclo.dtDataOraFine;
                        valPeriodo.PeriodoValido = valCiclo.PeriodoValido;

                        valPeriodo.Durata = valCiclo.Durata;
                        valPeriodo.PresenzaElettrolita = valCiclo.PresenzaElettrolita;

                        valPeriodo.TempMin = valCiclo.TempMin;
                        valPeriodo.TempMax = valCiclo.TempMax;
                        valPeriodo.DeltaTemp = valCiclo.DeltaTemp;
                        SettimanaMR _settimanaStart = new SettimanaMR();
                        SettimanaMR _settimanaEnd = new SettimanaMR();

                        DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                        Calendar _tempCal = dfi.Calendar;
                        int _settInizio = _tempCal.GetWeekOfYear(valPeriodo.dtDataOraStart, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                        _settimanaStart.settimana = _settInizio;
                        _settimanaStart.anno = valPeriodo.dtDataOraStart.Year;
                        _settimanaStart.chiaveSettimana = _settimanaStart.anno.ToString("0000") + _settimanaStart.settimana.ToString("00");

                        int _settfine = _tempCal.GetWeekOfYear(valPeriodo.dtDataOraFine, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                        _settimanaEnd.settimana = _settfine;
                        _settimanaEnd.anno = valPeriodo.dtDataOraFine.Year;
                        _settimanaEnd.chiaveSettimana = _settimanaEnd.anno.ToString("0000") + _settimanaEnd.settimana.ToString("00");

                        int _numSettimane;

                        if (_settimanaStart.anno == _settimanaEnd.anno) _numSettimane = _settimanaEnd.settimana - _settimanaStart.settimana;

                        uint _tmpDurata = valPeriodo.Durata;
                        bool _settAggiunta = false;
                        for (int _cicloS = 0; true; _cicloS++)
                        {
                            //Carico l'inizio periodo
                            valPeriodo.PeriodoTemporale.settimana = _settInizio;
                            valPeriodo.PeriodoTemporale.giornoInizio = ((int)_tempCal.GetDayOfWeek(valPeriodo.dtDataOraStart) == 0) ? 7 : (int)_tempCal.GetDayOfWeek(valPeriodo.dtDataOraStart);
                            valPeriodo.PeriodoTemporale.giornoFine = ((int)_tempCal.GetDayOfWeek(valPeriodo.dtDataOraFine) == 0) ? 7 : (int)_tempCal.GetDayOfWeek(valPeriodo.dtDataOraFine);
                            int _istante = (valPeriodo.PeriodoTemporale.giornoInizio - 1) * 288;
                            _istante += _tempCal.GetHour(valPeriodo.dtDataOraStart) * 12;
                            _istante += _tempCal.GetMinute(valPeriodo.dtDataOraStart) / 5;
                            valPeriodo.PeriodoTemporale.minutoInizio = _istante;

                            // se non presente, aggiungo la settimana
                            _settAggiunta = _listaSett.Add(_settimanaStart.chiaveSettimana);
                            if (_settAggiunta) SettimanePresenti.Add(_settimanaStart);
                            valPeriodo.SettimanaRiferimento = _settimanaStart;
                            // se la fine è nella stessa settimana chiudo il ciclo
                            if ((_settimanaStart.anno == _settimanaEnd.anno) && (_settimanaStart.settimana == _settimanaEnd.settimana))
                            {
                                valPeriodo.PeriodoTemporale.giornoFine = ((int)_tempCal.GetDayOfWeek(valPeriodo.dtDataOraFine) == 0) ? 7 : (int)_tempCal.GetDayOfWeek(valPeriodo.dtDataOraFine);

                                _istante = (valPeriodo.PeriodoTemporale.giornoFine - 1) * 288;
                                _istante += _tempCal.GetHour(valPeriodo.dtDataOraFine) * 12;
                                _istante += _tempCal.GetMinute(valPeriodo.dtDataOraFine) / 5;
                                valPeriodo.PeriodoTemporale.minutoFine = _istante;
                                valPeriodo.Durata = (uint)(valPeriodo.PeriodoTemporale.minutoFine - valPeriodo.PeriodoTemporale.minutoInizio) ;
       
                                CicliStatPeriodo.Add(valPeriodo);
                                //Log.Debug("Ciclo n° " + valPeriodo.IdMemoriaLunga.ToString() + " chiuso in settimana ");
                                break;

                            }


                            else
                            {
                                // ciclo fino a che non chiudo il periodo



                                // Chiudo il ciclo alle 24 di domenica e ne apro un altro
                                Log.Debug("Ciclo n° " + valPeriodo.IdMemoriaLunga.ToString() + " non chiuso in settimana ");
                                // Chiudo il primo ciclo
                                valPeriodo.PeriodoTemporale.giornoFine = 7;
                                valPeriodo.PeriodoTemporale.minutoFine = 2016;
                                valPeriodo.Durata = (uint)(valPeriodo.PeriodoTemporale.minutoFine - valPeriodo.PeriodoTemporale.minutoInizio);
                                valPeriodo.SettimanaRiferimento = _settimanaStart;
                                CicliStatPeriodo.Add(valPeriodo);

                                // Ora riapro il secondo

                                 _settInizio = _tempCal.GetWeekOfYear(valPeriodo.dtDataOraFine, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                 _settimanaStart = new SettimanaMR();
                                _settimanaStart.settimana = _settInizio;
                                _settimanaStart.anno = valPeriodo.dtDataOraFine.Year;
                                _settimanaStart.chiaveSettimana = _settimanaStart.anno.ToString("0000") + _settimanaStart.settimana.ToString("00");

                                 _settfine = _tempCal.GetWeekOfYear(valPeriodo.dtDataOraFine, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                                 _settimanaEnd = new SettimanaMR();
                                _settimanaEnd.settimana = _settfine;
                                _settimanaEnd.anno = valPeriodo.dtDataOraStart.Year;
                                _settimanaEnd.chiaveSettimana = _settimanaEnd.anno.ToString("0000") + _settimanaEnd.settimana.ToString("00");

                               // break;



                                _StatCicloSBPeriodo nuovoPeriodo = new _StatCicloSBPeriodo();
                                nuovoPeriodo.PeriodoTemporale = new PeriodoMR();
                                nuovoPeriodo.SettimanaRiferimento = new SettimanaMR();
                                nuovoPeriodo.PeriodoTemporale.settimana = _settfine;
                                nuovoPeriodo.PeriodoTemporale.giornoInizio = 1;  //Lunedì
                                nuovoPeriodo.PeriodoTemporale.minutoInizio = 0;  // ore 00:00
                                nuovoPeriodo.PeriodoTemporale.giornoFine = ((int)_tempCal.GetDayOfWeek(valPeriodo.dtDataOraFine) == 0) ? 7 : (int)_tempCal.GetDayOfWeek(valPeriodo.dtDataOraFine);
                                valPeriodo.PeriodoTemporale.giornoFine = ((int)_tempCal.GetDayOfWeek(valPeriodo.dtDataOraFine) == 0) ? 7 : (int)_tempCal.GetDayOfWeek(valPeriodo.dtDataOraFine);

                                _istante = (valPeriodo.PeriodoTemporale.giornoFine - 1) * 288;
                                _istante += _tempCal.GetHour(valPeriodo.dtDataOraFine) * 12;
                                _istante += _tempCal.GetMinute(valPeriodo.dtDataOraFine) / 5;
                                nuovoPeriodo.PeriodoTemporale.minutoFine = _istante;
                                nuovoPeriodo.Durata = (uint)(nuovoPeriodo.PeriodoTemporale.minutoFine - nuovoPeriodo.PeriodoTemporale.minutoInizio);
       

                                nuovoPeriodo.TipoEvento = valCiclo.TipoEvento;
                                nuovoPeriodo.IdMemoriaLunga = valCiclo.IdMemoriaLunga;
                                nuovoPeriodo.IdLocale = valCiclo.IdLocale;
                                nuovoPeriodo.FattoreCarica = valCiclo.FattoreCarica;
                                nuovoPeriodo.StatoCarica = valCiclo.StatoCarica;
                                nuovoPeriodo.Wh = valCiclo.Wh;
                                nuovoPeriodo.DataOraStart = valCiclo.DataOraStart;
                                nuovoPeriodo.dtDataOraStart = valCiclo.dtDataOraStart;
                                nuovoPeriodo.DataOraFine = valCiclo.DataOraFine;
                                nuovoPeriodo.dtDataOraFine = valCiclo.dtDataOraFine;
                                nuovoPeriodo.PeriodoValido = valCiclo.PeriodoValido;

                                nuovoPeriodo.Durata = valCiclo.Durata;
                                nuovoPeriodo.PresenzaElettrolita = valCiclo.PresenzaElettrolita;

                                nuovoPeriodo.TempMin = valCiclo.TempMin;
                                nuovoPeriodo.TempMax = valCiclo.TempMax;
                                nuovoPeriodo.DeltaTemp = valCiclo.DeltaTemp;

                                // se non presente, aggiungo la settimana
                                _settAggiunta = _listaSett.Add(_settimanaEnd.chiaveSettimana);
                                if (_settAggiunta) SettimanePresenti.Add(_settimanaStart);
                                nuovoPeriodo.SettimanaRiferimento = _settimanaStart;
                                
                                CicliStatPeriodo.Add(nuovoPeriodo);
                                
                                break;

                            }




                        }
                    }





                }

                // Riordino le settimane:
                System.Collections.Generic.List<SettimanaMR> _settimanePresenti = new System.Collections.Generic.List<SettimanaMR>();
                IEnumerable<SettimanaMR> query = SettimanePresenti.OrderBy(SettimanaMR => SettimanaMR.chiaveSettimana);
                _settimanePresenti.Clear();
                foreach (SettimanaMR _sett in query) _settimanePresenti.Add(_sett);
                SettimanePresenti.Clear();
                foreach (SettimanaMR _sett in _settimanePresenti) SettimanePresenti.Add(_sett);

                _datiPronti = true;
                Log.Debug("Presenti " + _listaSett.Count.ToString() + " settimane ");
                return true;
            }
            catch (Exception Ex)
            {
                Log.Debug("aggregaValori: " + Ex.Message);
                return false;
            }
        }

        #region PARAMETRI

        public bool DatiPronti
        {
            get { return _datiPronti; }
        }

        public DateTime AvvioSistema
        {
            get { return _dtStart; }
        }

        public string AvvioSistemaSt
        {
            get { return _dtStart.ToString("dd/MM/yyyy HH:mm"); }
        }

        public int NumeroGiorni
        {
            get { return _numGiorni; }
        }

        public double SecondiTotali
        {
            get { return _numSecondi; }
        }

        public int NumeroCicliSistema
        {
            get { return _numCicliTot; }
        }

        public int NumeroCicli
        {
            get { return _numCicliEff; }
        }

        public int NumeroAnomalie
        {
            get { return _numAnomali; }
        }


        public int NumeroCariche
        {
            get { return _numCariche; }
        }

        public int NumeroCaricheComplete
        {
            get { return _numCaricheComplete; }
        }

        public int NumeroCaricheParziali
        {
            get { return _numCaricheParziali; }
        }

        public int NumeroScariche
        {
            get { return _numScariche; }
        }

        public int NumeroSovrascariche
        {
            get { return _numSovraScariche; }
        }

        public int NumeroScaricheSovraT
        {
            get { return _numScaricaOver; }
        }

        public int NumeroCaricheCompSovraT
        {
            get { return _numCaricaCOver; }
        }

        public int NumeroCaricheParzSovraT
        {
            get { return _numCaricaPOver; }
        }

        public int NumeroPause
        {
            get { return _numPause; }
        }
        public int NumeroPauseBattScarica
        {
            get { return _numPauseScarica; }
        }

        public Int32 WHtotali
        {
            get { return _kWhtot; }
        }

        public Int32 WHCaricatiTotali
        {
            get { return _kWhCaricati; }
        }

        public double KWhCaricati
        {
            get { return (double)(_kWhCaricati/100); }
        }

        public Int32 AhTotali
        {
            get { return _Ahtot; }
        }

        public uint DurataCarica
        {
            get { return _durataCarica; }
        }

        public uint DurataScarica
        {
            get { return _durataScarica; }
        }

        public uint DurataPause
        {
            get { return _durataPause; }
        }

        public double DurataMancanzaElettrolita
        {
            get { return (double)_durataNoEl; }
        }

        public uint DurataFasiAttive
        {
            get { return _durataFasiAttive; }
        }
        public uint DurataMancanzaElFA
        {
            get { return _durataNoEl; }
        }

        public uint DurataSbilanciamentoFA
        {
            get { return _durataSbil; }
        }

        public uint DurataOverTempMax
        {
            get { return _durataOverTempMax; }
        }

        

        public int CicliAttesi
        {
            get { return _cicliAttesi; }
            set { _cicliAttesi = value; }
        }

        public double MassimoSbilanciamento
        {
            get { return (double)_maxSbil; }
        }


        public bool SuperatoMassimoSbilanciamento
        {
            get
            {
                return (bool)(_maxSbil > _maxSbilanciamento);

            }
        }

        public double DurataSbilanciamento
        {
            get { return (double)_durataSbil; }
        }

        public float ProfonditaScaricaMedia()
        {
            float _valmedio = 0;
            if ( NumeroScariche < 1) _valmedio = 0;
            else
            {
                _valmedio = (float)((float)_totaleScaricato / (float)NumeroScariche);
            }

            return _valmedio;
        }

        public float ProfonditaMacroScaricaMedia()
        {

            float _valmedio = 0;
            if (_macroScariche < 1) _valmedio = 0;
            else
            {
                _valmedio = (float)(100 - ((float)_totDodScariche / (float)_macroScariche));
            }

            return _valmedio;
        }

        public float MacroScaricheCritiche()
        {

            float _valmedio = 0;
            if (_macroScariche < 1) _valmedio = 0;
            else
            {
                _valmedio = (float)(((float)_macroSovrascariche / (float)_macroScariche)) * 100;
            }

            return _valmedio;
        }

        public float CaricheParziali()
        {

            float _valmedio = 0;
            if (_numCariche < 1) _valmedio = 0;
            else
            {
                _valmedio = (float)(((float)_numCaricheParziali / (float)_numCariche)) * 100;
            }

            return _valmedio;
        }

        public double Etot()
        {
            try
            {
                if (_cicliAttesi < 1) _cicliAttesi = 1000;
                if (_lastProg.IdProgramma < 1) return 0;

                double _etot;
                // Vdef va diviso per 100 e Ah va diviso per 10
                _etot = (double)_lastProg.BatteryVdef * (double)_lastProg.BatteryAhdef * (double)_cicliAttesi / 1000;
                _etot = _etot / FunzioniAnalisi.FattoreBaseSOH;
                return _etot;

            }
            catch
            {
                return 0;
            }
        }
                    


        public double EnScaricataNorm()
        {
            try
            {

                return _EnScaricataNorm / 100;

            }
            catch
            {
                return 0;
            }

        }


        //soglie
        public int SogliaProfonditaDoD
        {
            get { return _profonditaDoD; }
        }

        public int SogliaTempMaxScarica
        {
            get { return _tMaxScarica; }
        }

        public int SogliaTempMinScarica
        {
            get { return _tMinScarica; }
        }

        public int SogliaDiffTempScarica
        {
            get { return _deltaTScarica; }
        }

        public int SogliaTempMaxCaricaCompleta
        {
            get { return _tMaxCaricaComp; }
        }


        public int SogliaTempMaxCaricaParziale
        {
            get { return _tMaxCaricaParz; }
        }

        public int SogliaDiffTempCaricaCompleta
        {
            get { return _deltaTCaricaComp; }
        }


        public int SogliaDiffTempCaricaParziale
        {
            get { return _deltaTCaricaParz; }
        }

        public double SogliaMassimoSbilanciamento
        {
            get { return _maxSbilanciamento; }
        }

        public double SogliaMinimoChargeFactor
        {
            get { return _minFC; }
        }

        public int SogliaOrePausaDOD0810
        {
            get { return _orePausaDODFascia810; }
        }

        public int SogliaOrePausaDOD0608
        {
            get { return _orePausaDODFascia68; }
        }

        public int SogliaOrePausaDOD0406
        {
            get { return _orePausaDODFascia46; }
        }

        public int SogliaOrePausaDOD0204
        {
            get { return _orePausaDODFascia24; }
        }

        public int SogliaOrePausaDOD0002
        {
            get { return _orePausaDODFascia02; }
        }


        public int NumeroFasiAttive
        {
            get { return _numFasiAttive; }
        }

        public int NumeroFasiMancanzaElettrolita
        {
            get { return _mancanzaElFasiAttive; }
        }

        public DateTime UltimaLettura
        {
            get { return _ultimaLettura; }
        }
        

        #endregion PARAMETRI


        public DatiEstrazione CalcolaArrayGraficoChargeFactor(string Titolo)
        {
            DatiEstrazione _datiComp = new DatiEstrazione();
            try
            {
                int _passo;
                int _passi = 16;
                int _tempValore;

                _datiComp.Titolo = Titolo;
                _datiComp.TotLetture = 0;
                _datiComp.MinX = 0;
                _datiComp.MaxX = 150;
                _datiComp.NumStep = 16;
                _datiComp.TitoloAsseX = StringheStatistica.GrCfAsseX;
                _datiComp.TitoloAsseY = StringheStatistica.GrCfAsseY;

                //inizializzo gli array
                _datiComp.arrayValori = new int[_passi];
                _datiComp.arrayIntervalli = new int[_passi];
                _datiComp.arrayLabel = new string[_passi];
                _datiComp.StepSoglia = (int)(_minFC*10);
                _datiComp.VersoSoglia = DatiEstrazione.Direzione.Discendente;
                string _etichetta;

                for (_passo = 0; _passo < _passi; _passo++)
                {
                    _datiComp.arrayValori[_passo] = 0;
                    _tempValore = _passo * 10;
                    if (_tempValore > 0) _tempValore += 1;
                    _etichetta = ((Double)(_tempValore) /100).ToString("0.00") + " ÷ ";
                    _tempValore = (_passo + 1) * 10;
                    _etichetta = _etichetta + ((Double)(_tempValore) / 100).ToString("0.00");

                    _datiComp.arrayLabel[_passo] = _etichetta;
                }


                foreach (_StatMemLungaSB ciclo in CicliStatMemLunga)
                {
                    if (ciclo.PeriodoValido)
                    {

                        if (ciclo.TipoEvento == (byte)SerialMessage.TipoCiclo.Carica)
                        {
                            _datiComp.TotLetture += 1;
                            int _stepCarica = (int)(ciclo.FattoreCarica / 10);
                            if (_stepCarica > 15) _stepCarica = 15;
                            if (_stepCarica < 0) _stepCarica = 0;
                            _datiComp.arrayValori[_stepCarica] += 1;

                        }
                    }
                }

                _datiComp.MaxY = 0;

                for (_passo = 0; _passo < _passi; _passo++)
                {
                    if (_datiComp.arrayValori[_passo] > _datiComp.MaxY)
                        _datiComp.MaxY = _datiComp.arrayValori[_passo];
                }

                _datiComp.DatiValidi = true;


                return _datiComp;

            }
            catch
            {
                return _datiComp;

            }
        }


        public DatiEstrazione CalcolaArrayGraficoDeepChg(SerialMessage.TipoCiclo TipoInAnalisi, string Titolo)
        {
            DatiEstrazione _datiComp = new DatiEstrazione();
            try
            {
                int _passo;
                int _passi = 10;
                int _tempValore;

                _datiComp.Titolo = Titolo;
                _datiComp.TotLetture = 0;
                _datiComp.MinX = 0;
                _datiComp.MaxX = 10;
                _datiComp.NumStep = 10;
                _datiComp.TitoloAsseX = StringheStatistica.GrDODAsseX;
                _datiComp.TitoloAsseY = StringheStatistica.GrDODAsseY;

                //inizializzo gli array
                _datiComp.arrayValori = new int[_passi];
                _datiComp.arrayIntervalli = new int[_passi];
                _datiComp.arrayLabel = new string[_passi];
                _datiComp.StepSoglia = (100 - _profonditaDoD ) / 10;
                string _etichetta;

                for (_passo = 0; _passo < _passi; _passo++)
                {
                    _datiComp.arrayValori[_passo] = 0;
                    _tempValore = _passo * 10;
                    if (_tempValore > 0) _tempValore += 1;
                     _etichetta = _tempValore.ToString() + " ÷ ";
                    _tempValore = (_passo+1) * 10;
                    _etichetta = _etichetta +_tempValore.ToString() + "%";

                    _datiComp.arrayLabel[_passo] = _etichetta;
                }


                foreach (_StatMemLungaSB ciclo in CicliStatMemLunga)
                {
                    if (ciclo.PeriodoValido)
                    {

                        if (ciclo.TipoEvento == (byte)TipoInAnalisi)
                        {
                            _datiComp.TotLetture += 1;
                            int _stepCarica = (100 - ciclo.StatoCarica) / 10;
                            if (_stepCarica > 9) _stepCarica = 9;
                            if (_stepCarica < 0) _stepCarica = 0;
                            _datiComp.arrayValori[_stepCarica] += 1;

                        }
                    }
                }

                _datiComp.MaxY = 0;

                for (_passo = 0; _passo < _passi; _passo++)
                {
                    if (_datiComp.arrayValori[_passo] >  _datiComp.MaxY)
                        _datiComp.MaxY = _datiComp.arrayValori[_passo];
                }

                _datiComp.DatiValidi = true;


                return _datiComp;

            }
            catch
            {
                return _datiComp;

            }
        }

        public DatiEstrazione CalcolaArrayGraficoMancanzaEl(string Titolo)
        {
            DatiEstrazione _datiComp = new DatiEstrazione();
            try
            {
                //int _passo;
                //int _passi = NumStep + 1;
                //int _tempValore;

                _datiComp.Titolo = Titolo;
                _datiComp.TotLetture = 0;
                _datiComp.MinX = 0;
                _datiComp.MaxX = 0;
                //_datiComp.NumStep = 0;
                _datiComp.TitoloAsseX = StringheStatistica.GrDurCAsseX;
                _datiComp.TitoloAsseY = StringheStatistica.GrDurCAsseY;

                //inizializzo gli array

                _datiComp.NumEvTotali = _numFasiAttive;
                _datiComp.NumEvOK = _numFasiAttive - _mancanzaElFasiAttive;
                _datiComp.NumEvErrore = _mancanzaElFasiAttive;


                _datiComp.DatiValidi = true;


                return _datiComp;

            }
            catch
            {
                return _datiComp;

            }
        }

        public DatiEstrazione CalcolaArrayGraficoDurataCicli(SerialMessage.TipoCiclo TipoInAnalisi, int Modocarica, int MinStep, int NumStep, string Titolo)
        {
            DatiEstrazione _datiComp = new DatiEstrazione();
            try
            {
                int _passo;
                int _passi = NumStep + 1;
                int _tempValore;

                _datiComp.Titolo = Titolo;
                _datiComp.TotLetture = 0;
                _datiComp.MinX = 0;
                _datiComp.MaxX = NumStep;
                _datiComp.NumStep = NumStep;
                _datiComp.TitoloAsseX = StringheStatistica.GrDurCAsseX;
                _datiComp.TitoloAsseY = StringheStatistica.GrDurCAsseY;

                //inizializzo gli array
                _datiComp.arrayValori = new int[_passi];
                _datiComp.arrayIntervalli = new int[_passi];
                _datiComp.arrayLabel = new string[_passi];
                string _etichetta;
                if (MinStep == 0) MinStep = 1;

                for (_passo = 0; _passo < _passi; _passo++)
                {
                    _datiComp.arrayValori[_passo] = 0;
                    _tempValore = _passo * MinStep;

                    _etichetta = string.Format("{0:00}:{1:00}", (_tempValore / 60), _tempValore % 60);
                    // _etichetta = _tempValore.ToString() ;


                    _datiComp.arrayLabel[_passo] = _etichetta;
                }


                foreach (_StatMemLungaSB ciclo in CicliStatMemLunga)
                {

                    if (ciclo.PeriodoValido)
                    {

                        if ((ciclo.TipoEvento == (byte)TipoInAnalisi) && ((ciclo.ModoCarica == Modocarica) || (Modocarica == -1)))
                        {
                            _datiComp.TotLetture += 1;
                            int _durataCiclo = (int)ciclo.Durata / 60;  //Durata in Minuti
                            int _stepCarica = _durataCiclo / MinStep;
                            if (_stepCarica > NumStep) _stepCarica = NumStep;
                            if (_stepCarica < 0) _stepCarica = 0;
                            _datiComp.arrayValori[_stepCarica] += 1;

                        }
                    }

                }
                _datiComp.StepSoglia = NumStep + 1; //La durata carica non ha soglia massima
                _datiComp.MaxY = 0;

                for (_passo = 0; _passo < _passi; _passo++)
                {
                    if (_datiComp.arrayValori[_passo] > _datiComp.MaxY)
                        _datiComp.MaxY = _datiComp.arrayValori[_passo];
                }

                _datiComp.DatiValidi = true;


                return _datiComp;

            }
            catch
            {
                return _datiComp;

            }
        }

        /// <summary>
        /// Calcolas the array grafico temperture cicli.
        /// </summary>
        /// <param name="TipoInAnalisi">The tipo in analisi.</param>
        /// <param name="TempAttiva">Temperatira attiva: 0 minima, 1 Massima, 2 incremento.</param>
        /// <param name="StepTemp">Intervallo Temperatura.</param>
        /// <param name="MinTemp">Temperatura Minima (scala).</param>
        /// <param name="MaxTemp">Temperatura massima (scala).</param>
        /// <param name="LivelloCarica">Livello carica: 1 Completa, 0 Parziale , -1 tutto </param>
        /// <param name="Titolo">The titolo.</param>
        /// <returns></returns>
        public DatiEstrazione CalcolaArrayGraficoTempertureCicli(SerialMessage.TipoCiclo TipoInAnalisi, int TempAttiva, int StepTemp, int MinTemp, int MaxTemp, int ModoCarica, string Titolo)
        {
            DatiEstrazione _datiComp = new DatiEstrazione();
            try
            {
                int _passo;
                int _passi;
                int _tempValore;

                if (StepTemp < 1) StepTemp = 1;
                _passi = ((MaxTemp-MinTemp)/StepTemp) + 1 ;

                _datiComp.Titolo = Titolo;
                _datiComp.TotLetture = 0;
                _datiComp.MinX = MinTemp;
                _datiComp.MaxX = MaxTemp;
                _datiComp.NumStep = _passi;
                switch (TempAttiva)
                {
                    case 0:  // "Tempertura Minima (°C)"
                        _datiComp.TitoloAsseX = StringheStatistica.TitAxTmin;
                        break;
                    case 1:  // "Tempertura Massima (°C)";
                        _datiComp.TitoloAsseX = StringheStatistica.TitAxTmax;
                        break;
                    case 2:  // "Incremento Termico (°C)";
                        _datiComp.TitoloAsseX = StringheStatistica.TitAxTDelta;
                        break;
                    default:
                        _datiComp.TitoloAsseX = StringheStatistica.TitAxTmin;
                        break;
                }

                _datiComp.TitoloAsseY = StringheStatistica.NumeroCicli;

                //inizializzo gli array
                _datiComp.arrayValori = new int[_passi+1];
                _datiComp.arrayIntervalli = new int[_passi+1];
                _datiComp.arrayLabel = new string[_passi+1];
                string _etichetta;

                _tempValore = MinTemp;

                for (_passo = 0; _passo < _passi; _passo++)
                {
                    _datiComp.arrayValori[_passo] = 0;

                    _etichetta = _tempValore.ToString() + " °C";
                    _datiComp.arrayLabel[_passo] = _etichetta;
                    _tempValore += StepTemp;
                }


                foreach (_StatMemLungaSB ciclo in CicliStatMemLunga)
                {
                    int _tempLettura;
                    if (ciclo.PeriodoValido)
                    {

                        if ((ModoCarica == -1) | (ciclo.ModoCarica == ModoCarica))
                        {

                            if (ciclo.TipoEvento == (byte)TipoInAnalisi)
                            {
                                switch (TempAttiva)
                                {
                                    case 0: // "Tempertura Minima (°C)";
                                        _tempLettura = ciclo.TempMin;
                                        break;
                                    case 1: // "Tempertura Massima (°C)";
                                        _tempLettura = ciclo.TempMax;
                                        break;
                                    case 2: // "Incremento Termico (°C)";
                                        _tempLettura = ciclo.DeltaTemp;
                                        break;
                                    default: //"Tempertura Minima (°C)";
                                        _tempLettura = ciclo.TempMin;
                                        break;
                                }

                                _datiComp.TotLetture += 1;

                                int _stepCarica = (_tempLettura - MinTemp) / StepTemp;
                                if (_stepCarica > _passi) _stepCarica = _passi;
                                if (_stepCarica < 0) _stepCarica = 0;
                                _datiComp.arrayValori[_stepCarica] += 1;

                            }
                        }
                    }

                }

                _datiComp.MaxY = 0;

                for (_passo = 0; _passo < _passi; _passo++)
                {
                    if (_datiComp.arrayValori[_passo] > _datiComp.MaxY)
                        _datiComp.MaxY = _datiComp.arrayValori[_passo];
                }

                _datiComp.DatiValidi = true;


                return _datiComp;

            }
            catch
            {
                return _datiComp;

            }
        }

        public DatiEstrazione CalcolaArrayGraficoSettimana(String Settimana, string Titolo)
        {
            DatiEstrazione _datiComp = new DatiEstrazione();
            try
            {
                int _passo;
                int _passi = 0;
                int _tempValore;



                _datiComp.Titolo = Titolo;
                _datiComp.TotLetture = 0;
               // _datiComp.MinX = MinTemp;
               // _datiComp.MaxX = MaxTemp;
                _datiComp.NumStep = _passi;

                _datiComp.TitoloAsseY = StringheStatistica.GrSettTitoloAsseY;

                //inizializzo gli array
                _datiComp.DatiPeriodo.Clear();



                _datiComp.arrayValori = new int[_passi + 1];
                _datiComp.arrayIntervalli = new int[_passi + 1];
                _datiComp.arrayLabel = new string[_passi + 1];
                string _etichetta;

                //_tempValore = MinTemp;


                foreach (_StatCicloSBPeriodo ciclo in CicliStatPeriodo)
                {
                    int _tempLettura;
                    if (ciclo.SettimanaRiferimento.chiaveSettimana == Settimana)
                    {
                        _datiComp.TotLetture += 1;
                        _datiComp.DatiPeriodo.Add(ciclo);
                    }


                }

                _datiComp.DatiValidi = true;


                return _datiComp;
        

            }

            catch
            {
                return _datiComp;

            }
        }
    
    }


    public class _StatMemLungaSB
    {
        public Int32 IdLocale { get; set; }
        public UInt32 IdMemoriaLunga { get; set; }
        public UInt32 IdMemoriaLungaStat { get; set; }
        public byte TipoEvento { get; set; }
        public ushort IdProgramma { get; set; }
        public UInt32 PuntatorePrimoBreve { get; set; }
        public int NumEventiBrevi { get; set; }
        public string DataOraStart { get; set; }
        public string DataOraFine { get; set; }
        public DateTime dtDataOraStart { get; set; }
        public DateTime dtDataOraFine { get; set; }
        public UInt32 Durata { get; set; }
        public int TempMin { get; set; }
        public int TempMax { get; set; }
        public int DeltaTemp { get; set; }

        public UInt32 Vmin { get; set; }
        public UInt32 Vmax { get; set; }
        public short Amin { get; set; }
        public short Amax { get; set; }
        public byte PresenzaElettrolita { get; set; }
        public UInt32 DurataSbilCelle { get; set; }
        public UInt32 DurataMancanzaElettrolita { get; set; }
        public int Ah { get; set; }
        public Int32 Wh { get; set; }
        public byte CondizioneStop { get; set; }
        public byte FattoreCarica { get; set; }
        public byte StatoCarica { get; set; }
        public byte StatoCaricaIniziale { get; set; }

        public int TipoCariatore { get; set; }
        public UInt32 IdCaricatore { get; set; }
        public int AhCaricati { get; set; }
        public int AhScaricati { get; set; }

        public double deltaVmax { get; set; }

        public int ModoCarica { get; set; } // 1 Completa, 0 Parziale , -1 tutto 
        public int LivelloScarica { get; set; } // da fasce scarica ... 0 0/20 1 21/40.... 5 oltre 80% --> sovrascarica
        public bool PeriodoValido { get; set; }


    }


    public class _StatCicloSBPeriodo
    {
        public Int32 IdLocale { get; set; }
        public UInt32 IdMemoriaLunga { get; set; }
        public byte TipoEvento { get; set; }
        public ushort IdProgramma { get; set; }
        public UInt32 PuntatorePrimoBreve { get; set; }
        public int NumEventiBrevi { get; set; }
        public string DataOraStart { get; set; }
        public string DataOraFine { get; set; }
        public DateTime dtDataOraStart { get; set; }
        public DateTime dtDataOraFine { get; set; }
        public PeriodoMR PeriodoTemporale;
        public SettimanaMR SettimanaRiferimento;

        public UInt32 Durata { get; set; }
        public int TempMin { get; set; }
        public int TempMax { get; set; }
        public int DeltaTemp { get; set; }

        public UInt32 Vmin { get; set; }
        public UInt32 Vmax { get; set; }
        public short Amin { get; set; }
        public short Amax { get; set; }
        public byte PresenzaElettrolita { get; set; }
        public int Ah { get; set; }
        public Int32 Wh { get; set; }
        public byte CondizioneStop { get; set; }
        public byte FattoreCarica { get; set; }
        public byte StatoCarica { get; set; }
        public int TipoCariatore { get; set; }
        public UInt32 IdCaricatore { get; set; }
        public int AhCaricati { get; set; }
        public int AhScaricati { get; set; }

        public double deltaV12 { get; set; }
        public double deltaV13 { get; set; }
        public double deltaV14 { get; set; }
        public double deltaV23 { get; set; }
        public double deltaV24 { get; set; }
        public double deltaV34 { get; set; }

        public double deltaVmax { get; set; }

        public UInt32 DurataSbilCelle { get; set; }
        public UInt32 DurataMancanzaElettrolita { get; set; }
        public UInt32 DurataOverTempMax { get; set; }

        public int ModoCarica { get; set; } // 1 Completa, 0 Parziale , -1 tutto 
        public bool PeriodoValido { get; set; }


    }


    public class DatiEstrazione
    {
        public enum  Direzione : byte { Ascendente = 0, Discendente = 1 }
        public string Titolo { get; set; }
        public string Misura { get; set; }
        public int TotLetture { get; set; }
        public int SecIntervallo { get; set; }
        public int MinX { get; set; }
        public int MaxX { get; set; }
        public int MinY { get; set; }
        public int MaxY { get; set; }
        public int NumStep { get; set; }
        public int StepSoglia { get; set; }
        public Direzione VersoSoglia = Direzione.Ascendente;
        public bool DatiValidi { get; set; }

        public string TitoloAsseX { get; set; }
        public string UnitAsseX { get; set; }
        public string KeyAsseX { get; set; }

        public string TitoloAsseY { get; set; }
        public string UnitAsseY { get; set; }
        public string KeyAsseY { get; set; }

        public int[] arrayValori;
        public int[] arrayIntervalli;
        public string[] arrayLabel;

        // per diagrammi torta
        public int NumEvTotali { get; set; }
        public int NumEvErrore { get; set; }
        public int NumEvOK { get; set; }

        public List<_StatCicloSBPeriodo> DatiPeriodo = new List<_StatCicloSBPeriodo>();


        public DatiEstrazione()
        {
            DatiValidi = false;
            Titolo = "";
            TotLetture = 0;
            DatiPeriodo.Clear();
        }

    }
}
