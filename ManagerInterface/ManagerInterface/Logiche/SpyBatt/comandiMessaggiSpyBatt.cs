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

namespace ChargerLogic
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UnitaSpyBatt
    {
        private bool aspettaRisposta(int timeout, int risposteAttese = 1, bool aspettaAck = false, bool runAsync = false)
        {
            object vuoto;
            return aspettaRisposta(timeout, out vuoto, risposteAttese, aspettaAck, runAsync, elementiComuni.tipoMessaggio.NonDefinito);
        }


        /// <summary>
        /// Mette il task principale in attesa risposta:  
        /// V2.0 In base al canale attivo aspetto da seriale o leggo da USB 
        /// 
        /// </summary>
        /// <param name="timeout">numero di cicli di attasa da 500 millisecondi l'uno</param>
        /// <returns>true se ricevuta risposta, altrimenti false se interrotto per timeout</returns>
        private bool aspettaRisposta(int timeout, out object esito, int risposteAttese = 1, bool aspettaAck = false, bool runAsync = false, elementiComuni.tipoMessaggio TipoDati = elementiComuni.tipoMessaggio.NonDefinito)
        {
            DateTime _startRicezione;
            DateTime _startFunzione;
            bool _trovatoETX = false;
            bool _richiestaCancellata = false;
            int _divider = 1;
            int _lastProgress = -1;
            try
            {
                //if (_parametri.CanaleSpyBat == parametriSistema.CanaleDispositivo.USB)
                //{
                //    _divider = 2;
                // }

                //test ... attivato evento su USB: attendi è solo polling
                // 24/10 - test negativo: in polling è troppo lento
                //         riprisrinata la lettura sincrona

                // entro nel loop e aspetto 
                esito = null;

                if (_parametri.CanaleSpyBat == parametriSistema.CanaleDispositivo.USB)
                {
                    // mi metto in ascolto sul canale USB fino a EOT o a timeout 
                    // aspetto 10 mS 
                    // System.Threading.Thread.Sleep(100);

                    // Check the amount of data available to read
                    // In this case we know how much data we are expecting, 
                    // so wait until we have all of the bytes we have sent.
                    uint numBytesAvailable = 0;
                    uint numTempToRead = 0;
                    FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

                    int _risposteRicevute = 0;
                    int _ackRicevuti = 0;
                    int _breakRicevuti = 0;
                    SerialMessage.TipoRisposta _msgRicevuto;

                    Log.Debug("Inizio Ascolto " + timeout.ToString() + " - " + risposteAttese.ToString() + " - " + aspettaAck.ToString());

                    bool _inAttesa = true;

                    _startFunzione = DateTime.Now;
                    _startRicezione = DateTime.Now;
                    do
                    {
                        // verifico se ci sono dati
                        numBytesAvailable = 0;
                        ftStatus = _parametri.usbSpyBatt.GetRxBytesAvailable(ref numBytesAvailable);
                        if (ftStatus != FTDI.FT_STATUS.FT_OK)
                        {
                            // Errore nella lettura dati disponibili
                            Log.Warn("Failed to get number of bytes available to read (error " + ftStatus.ToString() + ")");
                        }
                        if (numBytesAvailable > 0)
                        {
                            // Now that we have the amount of data we want available, read it
                            byte[] readData = new byte[numBytesAvailable];
                            uint numBytesRead = 0;

                            // Note that the Read method is overloaded, so can read string or byte array data
                            ftStatus = _parametri.usbSpyBatt.Read(readData, numBytesAvailable, ref numBytesRead);
                            if (ftStatus != FTDI.FT_STATUS.FT_OK)
                            {
                                // Wait for a key press
                                Log.Warn("Failed to read data (error " + ftStatus.ToString() + ")");
                                //return false;
                            }

                            Log.Debug("Dati Ricevuti SB (USB) " + numBytesRead.ToString());
                            for (int _i = 0; _i < numBytesRead; _i++)
                            {

                                codaDatiSER.Enqueue(readData[_i]);


                                if (readData[_i] == SerialMessage.serETX)
                                {
                                    Log.Debug("Trovato Etx (USB), faccio ripartire il timeout");
                                    _trovatoETX = true;
                                    Log.Debug("Dati in coda SB (USB) " + codaDatiSER.Count.ToString());
                                    _startFunzione = DateTime.Now;
                                }

                                if (_trovatoETX)
                                {
                                    Log.Debug("trovato ETX");
                                    _msgRicevuto = analizzaCodaSB();
                                    Log.Debug("Dati in coda SB (USB) " + codaDatiSER.Count.ToString());

                                    _trovatoETX = false;
                                    
                                    switch (_msgRicevuto)
                                    {
                                        case SerialMessage.TipoRisposta.Ack:
                                            _ackRicevuti++;
                                            _ultimaRisposta = SerialMessage.TipoRisposta.Ack;
                                            if (aspettaAck && _risposteRicevute >= risposteAttese) _inAttesa = false;
                                            break;

                                        case SerialMessage.TipoRisposta.Data:
                                            _risposteRicevute++;
                                            _ultimaRisposta = SerialMessage.TipoRisposta.Data;
                                            if (TipoDati == elementiComuni.tipoMessaggio.DumpMemoria & _risposteRicevute >= risposteAttese - 1)
                                            {
                                                _richiestaCancellata = true;
                                                Log.Warn("Task cancellato al messaggio " + _risposteRicevute.ToString());
                                            }
                                            // se la gestione eventi è attiva, lancio un evento 
                                            if (runAsync == true)
                                            {
                                                if (Step != null)
                                                {
                                                    int _progress = 0;
                                                    double _valProgress = 0;
                                                    sbWaitStep _passo = new sbWaitStep();
                                                    _passo.DatiRicevuti = elementiComuni.contenutoMessaggio.Dati;
                                                    _passo.TipoDati = TipoDati;
                                                    _passo.Eventi = risposteAttese;
                                                    _passo.Step = _risposteRicevute;
                                                    _passo.EsecuzioneInterrotta = false;
                                                    if (risposteAttese > 0)
                                                    {
                                                        _valProgress = (_risposteRicevute * 100) / risposteAttese;
                                                    }
                                                    _progress = (int)_valProgress;
                                                    if (_lastProgress != _progress)
                                                    {
                                                        ProgressChangedEventArgs _stepEv = new ProgressChangedEventArgs(_progress, _passo);
                                                        Log.Debug("Passo " + _risposteRicevute.ToString());
                                                        Step(this, _stepEv);
                                                        _lastProgress = _progress;
                                                    }
                                                }
                                            }

                                            if (_risposteRicevute >= risposteAttese)
                                            {
                                                _inAttesa = false;
                                            }
                                            break;

                                        case SerialMessage.TipoRisposta.Break:
                                            Log.Debug("Ricevuto BREAK");
                                            _ultimaRisposta = SerialMessage.TipoRisposta.Break;
                                            _breakRicevuti++;
                                            _inAttesa = false;
                                            break;

                                        case SerialMessage.TipoRisposta.Nack:
                                        case SerialMessage.TipoRisposta.NonValido:
                                            _ultimaRisposta = _msgRicevuto;
                                            _breakRicevuti++;
                                            _inAttesa = false;
                                            break;

                                    }
                                }
                            }
                        }

                        System.Threading.Thread.Sleep(10);
                        if (raggiuntoTimeout(_startFunzione, timeout))
                        {
                            Log.Debug("aspettaRisposta.USB raggiunto Timeout");
                            break;
                        }
                        Log.Debug(DateTime.Now.ToShortTimeString());
                    }
                    while (_inAttesa);
                    // se background mode attivo, lancio l'evento di fine elaborazione
                    if (runAsync == true)
                    {
                        sbEndStep _esitoBg = new sbEndStep();
                        TimeSpan _tTrascorso = DateTime.Now.Subtract(_startRicezione);

                        _esitoBg.EventiPrevisti = risposteAttese;
                        _esitoBg.UltimoEvento = _risposteRicevute;
                        _esitoBg.SecondiElaborazione = _tTrascorso.TotalSeconds;
                        // RunWorkerCompletedEventArgs _esito = new RunWorkerCompletedEventArgs(_esitoBg, null, _richiestaCacellata);
                        esito = _esitoBg;
                    }


                    return !_inAttesa;
                }
                else
                {
                    int _cicli = 2;
                    //_rxRisposta = false;
                    string _logx = "";
                    System.Threading.Thread.Sleep(200);
                    do
                    {
                        _cicli++;
                        _logx += "^";
                        if (_rxRisposta)
                        {
                            _logx += "|";
                            Log.Debug(_logx);
                            return true;
                        }
                        System.Threading.Thread.Sleep(50);
                    }
                    while (_rxRisposta == false && (_cicli < (timeout * 10)));
                    Log.Debug(_logx);
                    return _rxRisposta;
                }
            }

            catch (Exception Ex)
            {
                Log.Debug("aspettaRisposta: " + Ex.Message);
                esito = null;
                return false;
            }

        }

        /// <summary>
        /// Raggiunto timeout. 
        /// Verifica se all'istante attuale ho raggiunto il limite di timeout
        /// </summary>
        /// <param name="inizio">Istante da cui calcolare lw durta dell'intervallo</param>
        /// <param name="SecondiTimeOut">Durata in secondi dell'attesa time out.</param>
        /// <returns></returns>
        private bool raggiuntoTimeout(DateTime inizio, int SecondiTimeOut)
        {
            try
            {
                DateTime _ora = DateTime.Now;
                TimeSpan _durata = _ora.Subtract(inizio);

                if (_durata.TotalSeconds > SecondiTimeOut)
                {
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch { return true; }
        }

        private void UsbWaiter()
        {
            Log.Info("USB Start Waiting...");
            do
            {
                SB_USBeventWait.WaitOne();                // Wait for notification
                Log.Info("USB Notified");
                usb_DataReceivedSb();

            } while (true);
        }

    }
}
