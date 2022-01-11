using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

using SQLite.Net;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;
using PannelloCharger;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace ChargerLogic
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UnitaSpyBatt
    {

        /// <summary>
        /// Ritorna true se la funzione CaricaStatoFirmware ha trovato un firmware valido
        /// </summary>
        public bool FirmwarePresente
        {
            get
            {
                return _firmwarePresente;
            }
        }


        /// <summary>
        /// Legge direttamente dallo SPY-BATT collegato i parametri del firmware attivo
        /// </summary>
        /// <param name="IdApparato">ID dell'apparato collegato</param>
        /// <param name="ApparatoConnesso">Se true tento la lettura diretta</param>
        /// <returns></returns>
        public bool CaricaStatoFirmware(string IdApparato, bool ApparatoConnesso)
        {
            try
            {
                bool _esito;
                //
                //                _idCorrente = IdApparato;
                //                
                _esito = false;

                if (ApparatoConnesso)
                {
                    // Eseguo solo se la connessione all'apparato è attiva
                    _mS.StatoFirmwareScheda = new MessaggioSpyBatt.StatoFirmware();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.CMD_INFO_BL;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    skipHead = true;
                    Log.Debug("SB Leggi Stato Firmware");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _firmwarePresente = false;
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                    if (_esito)
                    {
                        StatoFirmware = new MoriData.sbStatoFirmware();
                        StatoFirmware.IdApparato = _idCorrente;

                        StatoFirmware.RevBootloader = _mS.StatoFirmwareScheda.RevBootloader;
                        StatoFirmware.RevFirmware = _mS.StatoFirmwareScheda.RevFirmware;

                        //if ((_mS.StatoFirmwareScheda.RevBootloader != "") && (_mS.StatoFirmwareScheda.RevFirmware != "??????"))
                            _firmwarePresente = true;
                        StatoFirmware.CRCFirmware = _mS.StatoFirmwareScheda.CRCFirmware;
                        StatoFirmware.AddrFlash = _mS.StatoFirmwareScheda.AddrFlash;
                        StatoFirmware.LenFlash = _mS.StatoFirmwareScheda.LenFlash;
                        StatoFirmware.AddrFlash2 = _mS.StatoFirmwareScheda.AddrFlash2;
                        StatoFirmware.LenFlash2 = _mS.StatoFirmwareScheda.LenFlash2;
                        StatoFirmware.AddrProxy = _mS.StatoFirmwareScheda.AddrProxy;
                        StatoFirmware.LenProxy = _mS.StatoFirmwareScheda.LenProxy;
                        StatoFirmware.Stato = _mS.StatoFirmwareScheda.Stato;
                        StatoFirmware.valido = true;

                    }
                }
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaTestataFW: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }

        /// <summary>
        /// Carica l'immagine dell'APP nell'area specificata.
        /// </summary>
        /// <param name="IdApparato"></param>
        /// <param name="ApparatoConnesso"></param>
        /// <param name="Area"></param>
        /// <param name="Firmware"></param>
        /// <param name="RunAsinc"></param>
        /// <returns></returns>
        public bool AggiornaFirmware(string IdApparato, bool ApparatoConnesso, byte Area, BloccoFirmware Firmware, bool RunAsinc = false, bool WaitReconnect = true, bool ResetBeforeStart = true, string StepDescription = "" )
        {
            try
            {
                bool _esito;
                //object _dataRx;
                elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
                elementiComuni.WaitStep _stepBg = new elementiComuni.WaitStep();
                SerialMessage.LadeLightBool _AckPacchetto = SerialMessage.LadeLightBool.False;


                //bool _recordPresente;

                Thread.CurrentThread.CurrentUICulture = _parametri.currentCulture;

                if (true)  //ApparatoConnesso)
                {
                    // Se previsto, prima resetto la scheda

                    if (ResetBeforeStart)
                    {

                        if (Step != null)
                        {
                            elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                            if(ModoDesolfatatore)
                            {
                                _passo.Titolo = "Reset RIGENERATORE";
                                _passo.FaseCorrente = StepDescription;
                            }
                            else
                            {
                                _passo.Titolo = StringheMessaggio.strMsgResetSB;  //"Reset SPY-BATT";
                                _passo.FaseCorrente = "";
                            }

                            _passo.Eventi = 1;
                            _passo.Step = -1;
                            _passo.EsecuzioneInterrotta = false;
                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                            Step(this, _stepEv);
                        }
                        _esito = ResetScheda(false);
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
                                elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                _passo.Eventi = 20;
                                _passo.Step = _tentativi++;
                                _passo.EsecuzioneInterrotta = false;
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                _passo.FaseCorrente = StepDescription;
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
                            elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                            _passo.Titolo = StringheMessaggio.strMsgAggFWFase1;  // "Fase 1 - Invio Testata";
                            _passo.Eventi = 1;
                            _passo.Step = -1;
                            _passo.EsecuzioneInterrotta = false;
                            _passo.FaseCorrente = StepDescription;
                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                            Step(this, _stepEv);
                        }

                    }
                    Log.Debug("----------------------------------------------------------");
                    Log.Debug("Testata FW: " + Firmware.Release);
                    Log.Debug("----------------------------------------------------------");


                    _mS.ComponiMessaggioTestataFW(Area, Firmware.MessaggioTestata);
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);

                    //_esito = aspettaRisposta(elementiComuni.TimeoutBase, out _dataRx, MemorySlice, false, true, elementiComuni.tipoMessaggio.AggiornamentoFirmware);

                    if (!_esito)
                    {
                        if (RunAsinc)
                        {
                            
                            //Preparo l'intestazione della finestra di avanzamento
                            if (Step != null)
                            {
                                elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
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
                                elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = StringheMessaggio.strMsgAggFWFase2;  //"Fase 2 - Invio Dati";
                                _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                _passo.Step = -1;
                                _passo.EsecuzioneInterrotta = false;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                _passo.FaseCorrente = StepDescription;
                                Step(this, _stepEv);
                            }

                            _pacchettoCorrente = 0;
                            int _lastProgress = 0;
                            //fase 2: Flash 1
                            Log.Debug("----------------------------------------------------------");
                            Log.Debug("Blocco 2 - Flash 1 " + Firmware.ListaFlash1.Count.ToString());
                            Log.Debug("----------------------------------------------------------");

                            foreach (PacchettoDatiFW _dataBlock in Firmware.ListaFlash1)
                            {
                                _pacchettoCorrente++;
                                _tmpPacchettoCorrente = (ushort)( _pacchettoCorrente - 1 ) ;

                                //if (_tmpPacchettoCorrente == 3)
                                //    _tmpPacchettoCorrente = 6;

                                _mS.ComponiMessaggioPacchettoDatiFW((ushort)(_tmpPacchettoCorrente), (byte)_dataBlock.DimPacchetto, _dataBlock.PacchettoDati,_dataBlock.CRC);
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
                                            elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                            _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                            _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                            _passo.Step = _pacchettiInviati;
                                            _passo.EsecuzioneInterrotta = false;
                                            _passo.FaseCorrente = StepDescription;
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
                                                elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                                _passo.Titolo = StringheMessaggio.strMsgAggFWFase2err1;  //"Caricamento Applicazione fallito ( Blocco Flash 1 )";
                                                _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                                _passo.Step = -1;
                                                _passo.EsecuzioneInterrotta = true;
                                                _passo.FaseCorrente = StepDescription;
                                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                                Step(this, _stepEv);
                                                return false;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        //Errore pacchetto
                                        Log.Error("FW Update: errore pacchetto flash 1 #" + _pacchettoCorrente);
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

                            //fase 3: Flash 2
                            Log.Debug("----------------------------------------------------------");
                            Log.Debug("Blocco 3 - Flash 2 " + Firmware.ListaFlash2.Count.ToString());
                            Log.Debug("----------------------------------------------------------");


                            foreach (PacchettoDatiFW _dataBlock in Firmware.ListaFlash2)
                            {
                                _pacchettoCorrente++;
                                _tmpPacchettoCorrente = (ushort)(_pacchettoCorrente - 1);

                                //_tmpPacchettoCorrente = 999;

                                _mS.ComponiMessaggioPacchettoDatiFW(_tmpPacchettoCorrente, (byte)_dataBlock.DimPacchetto, _dataBlock.PacchettoDati, _dataBlock.CRC);
                                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                                if (_esito)
                                {
                                    _pacchettiInviati++;
                                    if (Step != null)
                                    {
                                        if (_ultimaRisposta == SerialMessage.TipoRisposta.Ack)
                                        {
                                            int _progress = 0;
                                            double _valProgress = 0;
                                            elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                            _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                            _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                            _passo.Step = _pacchettiInviati;
                                            _passo.EsecuzioneInterrotta = false;
                                            _passo.FaseCorrente = StepDescription;
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
                                                elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                                _passo.Titolo = StringheMessaggio.strMsgAggFWFase2err2;  //"Caricamento Applicazione fallito ( Blocco Flash 2 )";
                                                _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                                _passo.Step = -1;
                                                _passo.EsecuzioneInterrotta = true;
                                                _passo.FaseCorrente = StepDescription;
                                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                                Step(this, _stepEv);
                                                return false;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        //Errore pacchetto
                                        Log.Error("FW Update: errore pacchetto flash 2 #" + _pacchettoCorrente);
                                        return false;
                                    }

                                }


                            }


                            //fase 4: Proxy
                            Log.Debug("----------------------------------------------------------");
                            Log.Debug("Blocco 4 - Proxy Table " + Firmware.ListaProxy.Count.ToString());
                            Log.Debug("----------------------------------------------------------");


                            foreach (PacchettoDatiFW _dataBlock in Firmware.ListaProxy)
                            {
                                _pacchettoCorrente++;
                                _tmpPacchettoCorrente = (ushort)(_pacchettoCorrente - 1);

                                _mS.ComponiMessaggioPacchettoDatiFW(_tmpPacchettoCorrente, (byte)_dataBlock.DimPacchetto, _dataBlock.PacchettoDati, _dataBlock.CRC);
                                _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                                if (_esito)
                                {
                                    _pacchettiInviati++;
                                    if (Step != null)
                                    {
                                        if (_ultimaRisposta == SerialMessage.TipoRisposta.Ack)
                                        {
                                            int _progress = 0;
                                            double _valProgress = 0;
                                            elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                            _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                            _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                            _passo.Step = _pacchettiInviati;
                                            _passo.EsecuzioneInterrotta = false;
                                            _passo.FaseCorrente = StepDescription;
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
                                                Log.Error("FW Update: errore pacchetto Proxy #" + _pacchettiInviati);
                                                elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                                _passo.Titolo = StringheMessaggio.strMsgAggFWFase2err3;  //"Caricamento Applicazione fallito ( Blocco Proxy Table )";
                                                _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                                _passo.Step = -1;
                                                _passo.EsecuzioneInterrotta = true;
                                                _passo.FaseCorrente = StepDescription;
                                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                                                Step(this, _stepEv);
                                                return false;
                                            }

                                        }

                                    }
                                    else
                                    {
                                        //Errore pacchetto
                                        Log.Error("FW Update: errore pacchetto proxy#" + _pacchettoCorrente);
                                        return false;
                                    }

                                }


                            }


                            Log.Debug("----------------------------------------------------------");
                            Log.Debug("Fine TX  " + _pacchettiInviati.ToString());
                            Log.Debug("----------------------------------------------------------");



                            //ora, se previsto aspetto il riavvio e ricollego
                            if(WaitReconnect)
                            {
                                int _progress = 0;
                                double _valProgress = 0;
                                //  mi ricollego aspettando il riavvio
                                //Application.DoEvents();

                                if (Step != null)
                                {
                                    elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                    _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                    if (ModoDesolfatatore)
                                    {
                                        _passo.Titolo = "Fase 3 - riavvio RIGENERATORE";
                                        _passo.FaseCorrente = StepDescription;
                                    }
                                    else
                                    {
                                        _passo.Titolo = StringheMessaggio.strMsgAggFWFase3;  //"Fase 3 - riavvio SPY-BATT";
                                    }
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
                                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                        _passo.Eventi = 20;
                                        _passo.Step = _tentativi++;
                                        _passo.EsecuzioneInterrotta = false;
                                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                        _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                        _passo.FaseCorrente = StepDescription;
                                        _valProgress = (_tentativi * 5) ;

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

                                if(_esito && (!ModoDesolfatatore)) ScriviOrologio();

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



        /// <summary>
        /// Verifico il modulo attivo al momento
        /// </summary>
        /// <returns> 0 = BL  /  1 = APP area 1  /  2 = APP area 2 / -1 = Errore o BL non presente </returns>
        public int FirmwareAttivo()
        {
            try
            {

                if (StatoFirmware == null)
                {
                    //Stato non verificato o BL non presente
                    return -1;
                }


                if ((StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.BootLoaderInUso) == (byte)FirmwareManager.MascheraStato.BootLoaderInUso)
                {
                    //Modo BOOTLOADER

                    return 0;
                }
                else
                {
                    bool _esitoMicro1 = false;
                    _esitoMicro1 = (StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.Blocco1InUso) == (byte)FirmwareManager.MascheraStato.Blocco1InUso;
                    bool _esitoMicro2 = false;
                    _esitoMicro2 = (StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.Blocco2InUso) == (byte)FirmwareManager.MascheraStato.Blocco2InUso;


                    if (_esitoMicro1)
                    {
                        return 1;
                    }
                    else
                    {
                        if (_esitoMicro2)
                        {
                            return 2;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                }
            }

            catch (Exception EX)
            {
                Log.Error("FirmwareAttivo " + EX.Message);
                return -1;
            }
        }








        /// <summary>
        /// Commuta L'app attiva se qualla dell'area indicata come parametro
        /// </summary>
        /// <param name="IdApparato"></param>
        /// <param name="ApparatoConnesso"></param>
        /// <param name="Area"></param>
        /// <returns></returns>
        public bool SwitchFirmware(string IdApparato, bool ApparatoConnesso, byte Area )
        {
            try
            {
                bool _esito;
                object _dataRx;
                elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
                elementiComuni.WaitStep _stepBg = new elementiComuni.WaitStep();
                SerialMessage.LadeLightBool _AckPacchetto = SerialMessage.LadeLightBool.False;


                bool _recordPresente;


                if (ApparatoConnesso)
                {
                    // verificare Firmware.TestataOK



                    //Prima invio la testata

                    Log.Debug("----------------------------------------------------------");
                    Log.Debug(" Switch FW: " + Area.ToString());
                    Log.Debug("----------------------------------------------------------");

                    ControllaAttesa(UltimaScrittura);
                    Log.Debug("----------------------------------------------------------");

                    _mS.ComponiMessaggioSwitchFW(Area);
                    Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);

                    if (!_esito)
                    {
                        Log.Debug(" Switch FW fallito : ");
                        return false;
                    }
                    else
                    {
                        //ricevuto ritorno, verifico la risposta 
                        if (_ultimaRisposta == SerialMessage.TipoRisposta.Ack)
                        {
                            Log.Debug(" Switch FW completato area " + Area.ToString() + " attiva.");
                            return true;
                        }


                    }
                }
                // APPARATO NON CONNESSO --> False
                return false;
            }
            catch (Exception Ex)
            {
                Log.Error("FW Update: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());

                return false;
            }
        }

        /// <summary>
        /// Resetta la scheda e la riavvia caricando solo il BootLoader
        /// </summary>
        /// <param name="IdApparato"></param>
        /// <param name="ApparatoConnesso"></param>
        /// <param name="Area"></param>
        /// <returns></returns>
        public bool SwitchToBootLoader(string IdApparato, bool ApparatoConnesso)
        {
            try
            {
                bool _esito;
                object _dataRx;
                elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
                elementiComuni.WaitStep _stepBg = new elementiComuni.WaitStep();
                SerialMessage.LadeLightBool _AckPacchetto = SerialMessage.LadeLightBool.False;


                bool _recordPresente;


                if (ApparatoConnesso)
                {
                    // verificare Firmware.TestataOK



                    //Prima invio la testata

                    Log.Debug("+---------------------------------------------------------");
                    Log.Debug("| Switch to BootLoader                                    ");
                    Log.Debug("+---------------------------------------------------------");

                    ControllaAttesa(UltimaScrittura);


                    _mS.ComponiMessaggioSwitchBL();

                    Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);

                    if (!_esito)
                    {
                        Log.Debug(" Switch to BL fallito : ");
                        return false;
                    }
                    else
                    {
                        //ricevuto ritorno, verifico la risposta 
                        if (_ultimaRisposta == SerialMessage.TipoRisposta.Ack)
                        {
                            Log.Debug(" Switch to BL completato " );
                            return true;
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


        public bool ScriviBloccoDati(AreaDatiRegen BloccoAttivo, bool RunAsinc = false , string StepDescription = "")
        {


            try
            {

                uint _StartAddr = BloccoAttivo.StartAddress;
                uint _TmpAddr;
                //ushort _NumByte;
                ushort _NumSequenze = (ushort)BloccoAttivo.NumBlocchi;
                bool _esito;
                //byte[] _dataArray;
                int _numBytes;
                //int _cicliCompleti;
                //int _byteResidui;
                //byte[] _tempBuffer;

                //lblMemRegenAvanzamentoWrite.Visible = true;
                //txtMemRegenNumBlocchiWR.Text = BloccoAttivo.IdBlocco.ToString();
                elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
                elementiComuni.WaitStep _stepBg = new elementiComuni.WaitStep();


                // Prima vuoto la memoria

                _numBytes = _NumSequenze * 0x1000;
                ushort _pacchettiInviati = 0;
                ushort _pacchettoCorrente = 0;
                ushort _tmpPacchettoCorrente;

                //Preparo l'intestazione della finestra di avanzamento
                if (Step != null)
                {
                    elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                    _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                    _passo.Titolo = "Aggiornamento memoria";
                    _passo.Eventi = (int)_NumSequenze;
                    _passo.Step = -1;
                    _passo.EsecuzioneInterrotta = false;
                    ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                    _passo.FaseCorrente = StepDescription;
                    Step(this, _stepEv);
                }

                if (_NumSequenze > 0)
                {
                    int _bloccoCorrente;
                    _TmpAddr = _StartAddr;
                    for (int _cicloBlocchi = 0; _cicloBlocchi < _NumSequenze; _cicloBlocchi++)
                    {
                        _bloccoCorrente = _cicloBlocchi + 1;
                        _esito = CancellaBlocco4K(_TmpAddr);
                        if (_esito)
                        {
                            _TmpAddr += 0x1000;
                            if (Step != null)
                            {
                                if (_ultimaRisposta == SerialMessage.TipoRisposta.Ack)
                                {
                                    // Il pacchetto è stato accettato; incremento i contatori e continuo
                                    int _progress = 0;
                                    double _valProgress = 0;
                                    elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                    _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                    _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                    _passo.Eventi = (int)_NumSequenze;
                                    _passo.Step = _bloccoCorrente;
                                    _passo.EsecuzioneInterrotta = false;
                                    _passo.FaseCorrente = StepDescription + " / Canc : " + BloccoAttivo.strIdBlocco;
                                    if (_NumSequenze > 0)
                                    {
                                        _valProgress = (_bloccoCorrente * 100) / _NumSequenze;
                                    }
                                    _progress = (int)_valProgress;

                                    ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                    Step(this, _stepEv);
                                }
                            }
                        
                        }
                        else
                        { return false; }
                    }

                }
                else
                {
                    return false;
                }

                byte[] DataBuffer = BloccoAttivo.Data;


                uint _stepSent = 0;
                uint _posizione = 0;
                uint _datiTrasferiti = 0;
                byte[] _Tempbuffer = new byte[DataBlock];
                _TmpAddr = _StartAddr;
                while ((_numBytes - _datiTrasferiti) > DataBlock)
                {

                    for (int _blockStep = 0; _blockStep < DataBlock; _blockStep++)
                    {
                        _Tempbuffer[_blockStep] = DataBuffer[_posizione];
                        _posizione++;
                        _datiTrasferiti++;
                    }
                    _esito = ScriviBloccoMemoria(_TmpAddr, (ushort)DataBlock, _Tempbuffer);

                    _TmpAddr += DataBlock;
                    _stepSent++;
                    if (_esito)
                    {
                        if (Step != null)
                        {
                            if (_ultimaRisposta == SerialMessage.TipoRisposta.Data)
                            {
                                // Il pacchetto è stato accettato; incremento i contatori e continuo
                                int _progress = 0;
                                double _valProgress = 0;
                                elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                _passo.Eventi = (int)_numBytes;
                                _passo.Step = (int)_datiTrasferiti;
                                _passo.EsecuzioneInterrotta = false;
                                _passo.FaseCorrente = StepDescription + " / Scritt.: " + BloccoAttivo.strIdBlocco;
                                if (_numBytes > 0)
                                {
                                    _valProgress = (_datiTrasferiti * 100) / _numBytes;
                                }
                                _progress = (int)_valProgress;
                                ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                Step(this, _stepEv);
                            }
                        }
                    }
                    else
                    { return false; }


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
                if (_esito)
                {
                    if (Step != null)
                    {
                        if (_ultimaRisposta == SerialMessage.TipoRisposta.Ack)
                        {
                            // Il pacchetto è stato accettato; incremento i contatori e continuo
                            int _progress = 0;
                            double _valProgress = 0;
                            elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                            _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                            _passo.Eventi = (int)_numBytes;
                            _passo.Step = (int)_datiTrasferiti;
                            _passo.EsecuzioneInterrotta = false;
                            _passo.FaseCorrente = StepDescription + " / Fine Scritt.: " + BloccoAttivo.strIdBlocco;
                            if (_numBytes > 0)
                            {
                                _valProgress = (_datiTrasferiti * 100) / _numBytes;
                            }
                            _progress = (int)_valProgress;
                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                            Step(this, _stepEv);
                        }
                    }
                }
                else
                { return false; }


                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
                return false;
            }

        }

        public bool ImpostaAreaAttiva( byte Area, bool WaitReconnect = true, string StepDescription = "")
        {
            try
            {
                bool _esito;
                //object _dataRx;
                elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
                elementiComuni.WaitStep _stepBg = new elementiComuni.WaitStep();
                SerialMessage.LadeLightBool _AckPacchetto = SerialMessage.LadeLightBool.False;

                // Leggo InfoFW e verifico che l'area indicata sia OK

                if (Step != null)
                {
                    elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                    _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                    if (ModoDesolfatatore)
                    {
                        _passo.Titolo = "Impostazione area attiva RIGENERATORE";
                        _passo.FaseCorrente = StepDescription;
                    }
                    else
                    {
                        _passo.Titolo = StringheMessaggio.strMsgResetSB;  //"Reset SPY-BATT";
                        _passo.FaseCorrente = "";
                    }

                    _passo.Eventi = 1;
                    _passo.Step = -1;
                    _passo.EsecuzioneInterrotta = false;
                    _passo.FaseCorrente = StepDescription + " / Impostazione area " + Area.ToString();
                    ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                    Step(this, _stepEv);
                }



                _esito = CaricaStatoFirmware(Id, true);
                if (_esito && (UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk))
                {
                    if (Area == 2)
                    {
                        if ((StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.Blocco2InUso) == (byte)FirmwareManager.MascheraStato.Blocco2InUso)
                        {
                            // Sono già in area 2 
                            return true;
                        }
                        // Verifico se l'area 2 è OK
                        if ((StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.Blocco2SW) != (byte)FirmwareManager.MascheraStato.Blocco2SW)
                        {
                            return false;
                        }

                        _esito = SwitchFirmware(Id, true, 2);


                    }
                    else
                    {
                        if ((StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.Blocco1InUso) == (byte)FirmwareManager.MascheraStato.Blocco1InUso)
                        {
                            // Sono già in area 2 
                            return true;
                        }
                        // Verifico se l'area 2 è OK
                        if ((StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.Blocco1SW) != (byte)FirmwareManager.MascheraStato.Blocco1SW)
                        {
                            return false;
                        }

                        _esito = SwitchFirmware(Id, true, 1);


                    }

                    //ora aspetto il riavvio e ricollego

                    int _progress = 0;
                    double _valProgress = 0;
                    //  mi ricollego aspettando il riavvio
                    //Application.DoEvents();

                    if (Step != null)
                    {
                        elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                        if (ModoDesolfatatore)
                        {
                            _passo.Titolo = "Attesa riavvio RIGENERATORE";
                            _passo.FaseCorrente = StepDescription;
                        }
                        else
                        {
                            _passo.Titolo = StringheMessaggio.strMsgAggFWFase3;  //"Fase 3 - riavvio SPY-BATT";
                        }
                        _passo.Eventi = 1;
                        _passo.Step = -1;
                        _passo.FaseCorrente = StepDescription + " / Attesa riavvio area " + Area.ToString();
                        _passo.EsecuzioneInterrotta = false;
                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(0, _passo);
                        Step(this, _stepEv);
                    }

                    _esito = false;
                    int _tentativi = 5;

                    System.Threading.Thread.Sleep(500);

                    while (!_esito)
                    {
                        System.Threading.Thread.Sleep(100);
                        if (Step != null)
                        {
                            elementiComuni.WaitStep _passo = new elementiComuni.WaitStep();
                            _passo.Eventi = 20;
                            _passo.Step = _tentativi++;
                            _passo.EsecuzioneInterrotta = false;
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                            _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                            _passo.FaseCorrente = StepDescription;
                            _valProgress = (_tentativi );

                            _progress = (int)_valProgress;
                            _passo.FaseCorrente = StepDescription + " / Attesa riavvio area " + Area.ToString();

                            ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                //Log.Debug("Passo " + _risposteRicevute.ToString());
                                Step(this, _stepEv);
                        }

                        _esito = VerificaPresenza();

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
