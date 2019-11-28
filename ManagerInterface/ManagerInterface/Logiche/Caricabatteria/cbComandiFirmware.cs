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
                elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
                //sbWaitStep _stepBg = new sbWaitStep();
                SerialMessage.LadeLightBool _AckPacchetto = SerialMessage.LadeLightBool.False;
                elementiComuni.WaitStep _passo;
                ProgressChangedEventArgs _stepEv;


                //bool _recordPresente;

                Thread.CurrentThread.CurrentUICulture = _parametri.currentCulture;

                ControllaAttesa(UltimaScrittura);

                if (true)  //ApparatoConnesso)
                {
                    // Se previsto, prima resetto la scheda

                    if (false)//ResetBeforeStart)
                    {

                        if (Step != null)
                        {
                            _passo = new elementiComuni.WaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                            _passo.Titolo = StringheMessaggio.strMsgResetSB;  //"Reset SPY-BATT";
                            _passo.Eventi = 1;
                            _passo.Step = -1;
                            _passo.EsecuzioneInterrotta = false;
                            _stepEv = new ProgressChangedEventArgs(0, _passo);
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
                                _passo = new elementiComuni.WaitStep();
                                _passo.Eventi = 20;
                                _passo.Step = _tentativi++;
                                _passo.EsecuzioneInterrotta = false;
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                _passo.TipoDati = elementiComuni.tipoMessaggio.MemLunga;
                                _valProgress = (_tentativi * 5);

                                _progress = (int)_valProgress;
                                // if (_lastProgress != _progress)
                                {
                                     _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                    //Log.Debug("Passo " + _risposteRicevute.ToString());
                                    Step(this, _stepEv);
                                    //_lastProgress = _progress;
                                }
                            }

                            _esito = VerificaPresenza();

                        }



                    }

                    // prima di tutto verifico di essere in modalita bootloader; se non lo sono, commuto

                    _esito = CaricaStatoFirmware(IdApparato, true);

                    if(!_esito)
                    {
                        // non riesco nemmeno a verificare lo stato. FAIL 
                        Log.Error("Switch to BL Failed");
                        _passo = new elementiComuni.WaitStep();
                        _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                        _passo.Titolo = StringheMessaggio.strMsgAggFWFase2err1;  //"Caricamento Applicazione fallito ( Blocco Flash 1 )";
                        _passo.Eventi = 0;
                        _passo.Step = -1;
                        _passo.EsecuzioneInterrotta = true;
                        _stepEv = new ProgressChangedEventArgs(0, 0);
                        Step(this, _stepEv);
                        return false;
                    }




                    


                    //Prima invio la testata

                    if (RunAsinc)
                    {
                        //Preparo l'intestazione della finestra di avanzamento
                        if (Step != null)
                        {
                            _passo = new elementiComuni.WaitStep();
                            _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                            _passo.Titolo = StringheMessaggio.strMsgAggFWFase1;  // "Fase 1 - Invio Testata";
                            _passo.Eventi = 1;
                            _passo.Step = -1;
                            _passo.EsecuzioneInterrotta = false;
                            _stepEv = new ProgressChangedEventArgs(0, _passo);
                            Step(this, _stepEv);
                        }

                    }
                    Log.Debug("----------------------------------------------------------");
                    Log.Debug("Testata FW: " + Firmware.Release);
                    Log.Debug("----------------------------------------------------------");


                    _mS.ComponiMessaggioTestataFW(Area, Firmware.MessaggioTestata);
                    //Log.Debug(_mS.hexdumpMessaggio());
                    //_parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    Log.Debug("----------------------------------------------------------------------------------------------------------------------------------------");

                    _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);

                    Log.Debug("----------------------------------------------------------------------------------------------------------------------------------------");

                    _esito = aspettaRisposta(elementiComuni.Timeout5sec, 0, true);

                    Log.Debug("Invio testata: " + _esito.ToString());
                    Log.Debug("----------------------------------------------------------------------------------------------------------------------------------------");
                    Log.Debug("");


                    //_esito = aspettaRisposta(elementiComuni.TimeoutBase, out _dataRx, MemorySlice, false, true, elementiComuni.tipoMessaggio.AggiornamentoFirmware);

                    if (!_esito)
                    {
                        if (RunAsinc)
                        {

                            //Preparo l'intestazione della finestra di avanzamento
                            if (Step != null)
                            {
                                _passo = new elementiComuni.WaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = StringheMessaggio.strMsgAggFWFase1err1;  //"Caricamento Testata Fallito";
                                _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                _passo.Step = -1;
                                _passo.EsecuzioneInterrotta = true;
                                _stepEv = new ProgressChangedEventArgs(0, _passo);
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
                                _passo = new elementiComuni.WaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = StringheMessaggio.strMsgAggFWFase2;  //"Fase 2 - Invio Dati";
                                _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                _passo.Step = -1;
                                _passo.EsecuzioneInterrotta = false;
                                _stepEv = new ProgressChangedEventArgs(0, _passo);
                                Step(this, _stepEv);
                            }

                            _pacchettoCorrente = 0;
                            int _lastProgress = 0;
                            //fase 2:Invio aree

                            Log.Debug("----------------------------------------------------------");
                            Log.Debug("Blocchi Dati:  " + Firmware.TotaleBlocchi.ToString());
                            Log.Debug("----------------------------------------------------------");

                            int _numAree = 0;
                            foreach (AreaDatiFWLL _areaDati in Firmware.ListaAree)
                            {
                                // aggiorno il titolo
                                Log.Debug("FW Update: area #" + _numAree.ToString());
                                 _passo = new elementiComuni.WaitStep();
                                _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                _passo.Titolo = "Caricamento Area " + _numAree.ToString();
                                _passo.Eventi = (int)_pacchettoCorrente;
                                _passo.Step = -1;
                                _passo.EsecuzioneInterrotta = false;
                                _stepEv = new ProgressChangedEventArgs(0, _passo);
                                Step(this, _stepEv);


                                // ora faccio scorrere i pacchetti dell'area
                                foreach (PacchettoDatiFWLL _pacchettoDati in _areaDati.ListaPacchetti)
                                {
                                    _pacchettoCorrente++;
                                    _tmpPacchettoCorrente = (ushort)(_pacchettoCorrente - 1);
                                    Log.Debug("Passo " + _tmpPacchettoCorrente.ToString() + " - Size " + _pacchettoDati.DimPacchetto.ToString() + " di " + _areaDati.ListaPacchetti.Count.ToString());

                                    _mS.ComponiMessaggioPacchettoDatiFW((ushort)(_tmpPacchettoCorrente), (byte)_pacchettoDati.DimPacchetto, _pacchettoDati.PacchettoDati, _pacchettoDati.CRC);
                                    // _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                                    _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                                    _esito = aspettaRisposta(100, 0, true);


                                    Log.Debug("Passo " + _pacchettoCorrente.ToString() + " inviato con esito " + _esito.ToString());

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
                                                _passo = new elementiComuni.WaitStep();
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
                                                    _stepEv = new ProgressChangedEventArgs(_progress, _passo);
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
                                                    Log.Error("FW Update(1): errore pacchetto #" + _pacchettiInviati + " - Area " + _numAree.ToString());
                                                     _passo = new elementiComuni.WaitStep();
                                                    _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.vuoto;
                                                    _passo.Titolo = StringheMessaggio.strMsgAggFWFase2err1;  //"Caricamento Applicazione fallito ( Blocco Flash 1 )";
                                                    _passo.Eventi = (int)Firmware.TotaleBlocchi;
                                                    _passo.Step = -1;
                                                    _passo.EsecuzioneInterrotta = true;
                                                    _stepEv = new ProgressChangedEventArgs(0, _passo);
                                                    Step(this, _stepEv);
                                                    return false;
                                                }

                                            }
                                        }
                                        else
                                        {
                                            //Errore pacchetto
                                            Log.Error("FW Update(2): errore pacchetto #" + _pacchettiInviati + " - Area " + _numAree.ToString());
                                            return false;
                                        }

                                    }
                                    else
                                    {
                                        //Errore pacchetto
                                        Log.Error("FW Update(3): errore pacchetto #" + _pacchettiInviati + " - Area " + _numAree.ToString());
                                        return false;
                                    }


                                }

                            
                            
                            
                            //dopo la riconnessione, sincronizzo l'orologio
                                
                               // if (_esito) ScriviOrologio();

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
        /// Legge direttamente dal LADE-Light collegato i parametri del firmware attivo
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

                    ControllaAttesa(UltimaScrittura);

                    // Eseguo solo se la connessione all'apparato è attiva
                    _mS.StatoFirmwareScheda = new MessaggioLadeLight.StatoFirmware();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.CMD_INFO_BL;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    //skipHead = true;
                    Log.Debug("LL Leggi Stato Firmware");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _firmwarePresente = false;
                    _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                    if (_esito)
                    {
                        if (_mS._comando == (byte)SerialMessage.TipoComando.NACK_PACKET)
                        {
                            return false;
                        }
                        else
                        {
                            StatoFirmware = new MoriData.llStatoFirmware();
                            StatoFirmware.IdApparato = "";// _idCorrente;

                            StatoFirmware.RevBootloader = _mS.StatoFirmwareScheda.RevBootloader;
                            StatoFirmware.RevFirmware = _mS.StatoFirmwareScheda.RevFirmware;
                            StatoFirmware.RevDisplay = _mS.StatoFirmwareScheda.RevDisplay;

                            //if ((_mS.StatoFirmwareScheda.RevBootloader != "") && (_mS.StatoFirmwareScheda.RevFirmware != "??????"))
                            _firmwarePresente = true;
                            StatoFirmware.Stato = _mS.StatoFirmwareScheda.Stato;
                            StatoFirmware.CRCFirmware = _mS.StatoFirmwareScheda.CRCFirmware;
                            StatoFirmware.AddrFlash0 = _mS.StatoFirmwareScheda.AddrFlash0;
                            StatoFirmware.LenFlash0 = _mS.StatoFirmwareScheda.LenFlash0;
                            StatoFirmware.AddrFlash1 = _mS.StatoFirmwareScheda.AddrFlash1;
                            StatoFirmware.LenFlash1 = _mS.StatoFirmwareScheda.LenFlash1;
                            StatoFirmware.AddrFlash2 = _mS.StatoFirmwareScheda.AddrFlash2;
                            StatoFirmware.LenFlash2 = _mS.StatoFirmwareScheda.LenFlash2;
                            StatoFirmware.AddrFlash3 = _mS.StatoFirmwareScheda.AddrFlash3;
                            StatoFirmware.LenFlash3 = _mS.StatoFirmwareScheda.LenFlash3;
                            StatoFirmware.AddrFlash4 = _mS.StatoFirmwareScheda.AddrFlash4;
                            StatoFirmware.LenFlash4 = _mS.StatoFirmwareScheda.LenFlash4;
                            StatoFirmware.valido = true;
                        }
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

                ControllaAttesa(UltimaScrittura);

                if (ApparatoConnesso)
                {
                    // verificare Firmware.TestataOK



                    //Prima invio la testata

                    Log.Debug("+---------------------------------------------------------");
                    Log.Debug("| Switch to BootLoader                                    ");
                    Log.Debug("+---------------------------------------------------------");


                    _mS.ComponiMessaggioSwitchBL();

                    Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                    _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(50, 0, true);

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
                            Log.Debug(" Switch to BL completato ");
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



        /// <summary>
        /// Forza il riavvio della scheda
        /// </summary>
        /// <returns></returns>
        public bool ResetScheda()
        {


            try
            {
                bool _esito;
                ControllaAttesa(UltimaScrittura);

                _mS.Comando = SerialMessage.TipoComando.CMD_RESET_BOARD;


                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug(" RESET SCHEDA ");

                _mS.ComponiMessaggio();
        

                Log.Debug(_mS.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);

                Log.Debug(_mS.hexdumpMessaggio());
                Log.Debug("------------------------------------------------------------------------------------------------------------");

                // ora attendo la riconnessione:

                
                DateTime Inizio = DateTime.Now;
                bool connesso = false;
                System.Threading.Thread.Sleep(4000);  // aspetto 4 secondi prima di tentare la connessione

                while (!connesso)
                {
                    System.Threading.Thread.Sleep(1000);// aspetto un altro secondo prima di tentare la connessione
                    connesso = StartComunicazione(5);
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




        /// <summary>
        /// Commuta L'app attiva se qualla dell'area indicata come parametro
        /// </summary>
        /// <param name="IdApparato"></param>
        /// <param name="ApparatoConnesso"></param>
        /// <param name="Area"></param>
        /// <returns></returns>
        public bool SwitchFirmware(string IdApparato, bool ApparatoConnesso, byte Area)
        {
            try
            {
                bool _esito;
                object _dataRx;
                elementiComuni.EndStep _esitoBg = new elementiComuni.EndStep();
                elementiComuni.WaitStep _stepBg = new elementiComuni.WaitStep();
                SerialMessage.LadeLightBool _AckPacchetto = SerialMessage.LadeLightBool.False;


                bool _recordPresente;

                ControllaAttesa(UltimaScrittura);

                if (ApparatoConnesso)
                {
                    // verificare Firmware.TestataOK



                    //Prima invio la testata

                    Log.Debug("----------------------------------------------------------");
                    Log.Debug(" Switch FW: " + Area.ToString());
                    Log.Debug("----------------------------------------------------------");


                    _mS.ComponiMessaggioSwitchFW(Area);
                    Log.Debug(_mS.hexdumpArray(_mS.MessageBuffer));
                    _parametri.scriviMessaggioLadeLight(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(50, 1, true);

                    if (!_esito)
                    {
                        Log.Debug(" Switch FW fallito : " + _mS.EsitoRichiestaFW.ToString());
                        return false;
                    }
                    else
                    {
                        //ricevuto ritorno, verifico la risposta 
                        if (_mS.EsitoRichiestaFW == SerialMessage.RequiredActionOutcome.Success)
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




    }
}