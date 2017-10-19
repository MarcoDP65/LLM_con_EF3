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
using PannelloCharger;

namespace ChargerLogic
{
    public partial class CaricaBatteria
    {

        /// <summary>
        /// Carica l'immagine dell'APP nell'area specificata.
        /// </summary>
        /// <param name="IdApparato"></param>
        /// <param name="ApparatoConnesso"></param>
        /// <param name="Area"></param>
        /// <param name="Firmware"></param>
        /// <param name="RunAsinc"></param>
        /// <returns></returns>
        public bool AggiornaFirmware(string IdApparato, bool ApparatoConnesso, byte Area, BloccoFirmwareLL Firmware, bool RunAsinc = false, bool WaitReconnect = true, bool ResetBeforeStart = true)
        {
            try
            {
                bool _esito;
                //object _dataRx;
                sbEndStep _esitoBg = new sbEndStep();
                sbWaitStep _stepBg = new sbWaitStep();
                SerialMessage.LadeLightBool _AckPacchetto = SerialMessage.LadeLightBool.False;


                //bool _recordPresente;

                Thread.CurrentThread.CurrentUICulture = _parametri.currentCulture;

                if (true)  //ApparatoConnesso)
                {
                    // Se previsto, prima resetto la scheda

                    if (false)//ResetBeforeStart)
                    {

                        if (Step != null)
                        {
                            llWaitStep _passo = new llWaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                            _passo.Titolo = StringheMessaggio.strMsgResetSB;  //"Reset SPY-BATT";
                            _passo.Eventi = 1;
                            _passo.Step = -1;
                            _passo.EsecuzioneInterrotta = false;
                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                            Step(this, _stepEv);
                        }
                        //_esito = ResetScheda(false);
                        _esito = true;
                        //ora aspetto il riavvio e ricollego
                        int _progress = 0;
                        double _valProgress = 0;

                        _esito = false;
                        int _tentativi = 0;

                        while (!_esito)
                        {
                            System.Threading.Thread.Sleep(500);
                            if (Step != null)
                            {
                                llWaitStep _passo = new llWaitStep();
                                _passo.Eventi = 20;
                                _passo.Step = _tentativi++;
                                _passo.EsecuzioneInterrotta = false;
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                _valProgress = (_tentativi * 5);

                                _progress = (int)_valProgress;
                                // if (_lastProgress != _progress)
                                {
                                    ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                    //Log.Debug("Passo " + _risposteRicevute.ToString());
                                    Step(this, _stepEv);
                                    //_lastProgress = _progress;
                                }
                            }

                            _esito = VerificaPresenza();

                        }



                    }

                    //Prima invio la testata

                    if (RunAsinc)
                    {
                        //Preparo l'intestazione della finestra di avanzamento
                        if (Step != null)
                        {
                            llWaitStep _passo = new llWaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                            _passo.Titolo = StringheMessaggio.strMsgAggFWFase1;  // "Fase 1 - Invio Testata";
                            _passo.Eventi = 1;
                            _passo.Step = -1;
                            _passo.EsecuzioneInterrotta = false;
                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                            Step(this, _stepEv);
                        }

                    }
                    Log.Debug("----------------------------------------------------------");
                    Log.Debug("Testata FW: " + Firmware.Release);
                    Log.Debug("----------------------------------------------------------");


                    _mS.ComponiMessaggioTestataFW(Area, Firmware.MessaggioTestata);
                    Log.Debug(_mS.hexdumpMessaggio());
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, true);

                    //_esito = aspettaRisposta(elementiComuni.TimeoutBase, out _dataRx, MemorySlice, false, true, elementiComuni.tipoMessaggio.AggiornamentoFirmware);

                    if (!_esito)
                    {
                        if (RunAsinc)
                        {

                            //Preparo l'intestazione della finestra di avanzamento
                            if (Step != null)
                            {
                                sbWaitStep _passo = new sbWaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = StringheMessaggio.strMsgAggFWFase1err1;  //"Caricamento Testata Fallito";
                                _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                _passo.Step = -1;
                                _passo.EsecuzioneInterrotta = true;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                Step(this, _stepEv);
                            }
                        }

                    }
                    else
                    {
                        //testata accettata , mando i dati
                        if (RunAsinc)
                        {
                            ushort _pacchettiInviati = 0;
                            ushort _pacchettoCorrente = 0;
                            ushort _tmpPacchettoCorrente;

                            //Preparo l'intestazione della finestra di avanzamento
                            if (Step != null)
                            {
                                llWaitStep _passo = new llWaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = StringheMessaggio.strMsgAggFWFase2;  //"Fase 2 - Invio Dati";
                                _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                _passo.Step = -1;
                                _passo.EsecuzioneInterrotta = false;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                Step(this, _stepEv);
                            }

                            _pacchettoCorrente = 0;
                            int _lastProgress = 0;
                            //fase 2:Invio aree

                            Log.Debug("----------------------------------------------------------");
                    //        Log.Debug("Blocco 2 - Flash 1 " + Firmware.ListaFlash.Count.ToString());
                            Log.Debug("----------------------------------------------------------");

                    //        foreach (PacchettoDatiFW _dataBlock in Firmware.ListaFlash)
                            {
                                _pacchettoCorrente++;
                                _tmpPacchettoCorrente = (ushort)(_pacchettoCorrente - 1);

                                //if (_tmpPacchettoCorrente == 3)
                                //    _tmpPacchettoCorrente = 6;

                     //           _mS.ComponiMessaggioPacchettoDatiFW((ushort)(_tmpPacchettoCorrente), (byte)_dataBlock.DimPacchetto, _dataBlock.PacchettoDati, _dataBlock.CRC);
                                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                                if (_esito)
                                {
                                    _pacchettiInviati++;
                                    if (Step != null)
                                    {
                                        if (_ultimaRisposta == SerialMessage.TipoRisposta.Ack)
                                        {
                                            // Il pacchetto è stato accettato; incremento i contatori e continuo
                                            int _progress = 0;
                                            double _valProgress = 0;
                                            sbWaitStep _passo = new sbWaitStep();
                                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                            _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                            _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                            _passo.Step = _pacchettiInviati;
                                            _passo.EsecuzioneInterrotta = false;
                                            if (Firmware.TotaleBlocchi > 0)
                                            {
                                                _valProgress = (_pacchettiInviati * 100) / Firmware.TotaleBlocchi;
                                            }
                                            _progress = (int)_valProgress;
                                            if (_lastProgress != _progress)
                                            {
                                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                                //Log.Debug("Passo " + _risposteRicevute.ToString());
                                                Step(this, _stepEv);
                                                _lastProgress = _progress;
                                            }
                                        }
                                        else
                                        {
                                            // Pacchetto non valido, blocco l'aggiornmento
                                            _pacchettiInviati = 0;
                                            _pacchettoCorrente = 0;


                                            //Preparo l'intestazione della finestra di avanzamento
                                            if (Step != null)
                                            {
                                                Log.Error("FW Update: errore pacchetto flash 1 #" + _pacchettiInviati);
                                                sbWaitStep _passo = new sbWaitStep();
                                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                                _passo.Titolo = StringheMessaggio.strMsgAggFWFase2err1;  //"Caricamento Applicazione fallito ( Blocco Flash 1 )";
                                                _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                                _passo.Step = -1;
                                                _passo.EsecuzioneInterrotta = true;
                                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                                Step(this, _stepEv);
                                                return false;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        //Errore pacchetto
                                        Log.Error("FW Update: errore pacchetto flash #" + _pacchettoCorrente);
                                        return false;
                                    }

                                }
                                else
                                {
                                    //Errore pacchetto
                                    Log.Error("FW Update: errore pacchetto flash 1 #" + _pacchettoCorrente);
                                    return false;
                                }


                            }

                            //fase 3: Indirizzo
                            Log.Debug("----------------------------------------------------------");
                            Log.Debug("Blocco 3 - Indirizzo app - 4 byte " ) ;
                            Log.Debug("----------------------------------------------------------");

                            Log.Debug("----------------------------------------------------------");
                            Log.Debug("Fine TX  " + _pacchettiInviati.ToString());
                            Log.Debug("----------------------------------------------------------");



                            //ora, se previsto aspetto il riavvio e ricollego
                            if (WaitReconnect)
                            {
                                int _progress = 0;
                                double _valProgress = 0;
                                //  mi ricollego aspettando il riavvio
                                //Application.DoEvents();

                                if (Step != null)
                                {
                                    sbWaitStep _passo = new sbWaitStep();
                                    _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                    _passo.Titolo = StringheMessaggio.strMsgAggFWFase3;  //"Fase 3 - riavvio SPY-BATT";
                                    _passo.Eventi = 1;
                                    _passo.Step = -1;
                                    _passo.EsecuzioneInterrotta = false;
                                    ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                    Step(this, _stepEv);
                                }

                                _esito = false;
                                int _tentativi = 0;
                                _lastProgress = 0;
                                while (!_esito)
                                {
                                    System.Threading.Thread.Sleep(500);
                                    if (Step != null)
                                    {
                                        sbWaitStep _passo = new sbWaitStep();
                                        _passo.Eventi = 20;
                                        _passo.Step = _tentativi++;
                                        _passo.EsecuzioneInterrotta = false;
                                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                        _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                        _valProgress = (_tentativi * 5);

                                        _progress = (int)_valProgress;
                                        // if (_lastProgress != _progress)
                                        {
                                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                            //Log.Debug("Passo " + _risposteRicevute.ToString());
                                            Step(this, _stepEv);
                                            _lastProgress = _progress;
                                        }
                                    }

                                    _esito = VerificaPresenza();

                                }

                                //dopo la riconnessione, sincronizzo l'orologio

                                if (_esito) ScriviOrologio();

                            }


                        }

                    }


                }
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("FW Update: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());

                return false;
            }
        }




    }
}