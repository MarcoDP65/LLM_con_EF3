using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

using SQLite.Net;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;

namespace ChargerLogic
{
    public partial class UnitaDisplay
    {


        public bool scriviMessaggio(byte[] messaggio, int Start, int NumByte)
        {

            try
            {
                //codaDatiSER.Clear();
                echoDatiSER.Clear();
                for (int i = 0; i < NumByte; i++)
                {
                    echoDatiSER.Enqueue(messaggio[i]);
                }


                if (serialeApparato.IsOpen)
                {
                    serialeApparato.Write(messaggio, Start, NumByte);
                    return true;
                }
                else
                    return false;

            }

            catch (Exception Ex)
            {
                Log.Error("scriviMessaggio: " + Ex.Message);
                return false;
            }
        }



        private bool aspettaRisposta(int timeout, int risposteAttese = 1, bool aspettaAck = false, bool runAsync = false, bool modoDeso = false, bool IgnoraContenuto = false, bool SkipHead = false)
        {
            object vuoto;
            return aspettaRisposta(timeout, out vuoto, risposteAttese, aspettaAck, runAsync, elementiComuni.tipoMessaggio.NonDefinito,IgnoraContenuto, SkipHead);
        }





        /// <summary>
        /// Mette il task principale in attesa risposta:  
        /// V2.0 In base al canale attivo aspetto da seriale o leggo da USB 
        /// 
        /// </summary>
        /// <param name="timeout">numero di cicli di attasa da 500 millisecondi l'uno</param>
        /// <returns>true se ricevuta risposta, altrimenti false se interrotto per timeout</returns>
        private bool aspettaRisposta(int timeout, out object esito, int risposteAttese = 1, bool aspettaAck = false, bool runAsync = false, elementiComuni.tipoMessaggio TipoDati = elementiComuni.tipoMessaggio.NonDefinito, bool IgnoraContenuto = false, bool SkipHead = false)
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

                // mi metto in ascolto sul canale USB fino a EOT o a timeout 
                // aspetto 10 mS 
                // System.Threading.Thread.Sleep(100);

                // Check the amount of data available to read
                // In this case we know how much data we are expecting, 
                // so wait until we have all of the bytes we have sent.
                int numBytesAvailable = 0;
                int numTempToRead = 0;
                FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

                int _risposteRicevute = 0;
                int _ackRicevuti = 0;
                int _breakRicevuti = 0;
                SerialMessage.TipoRisposta _msgRicevuto;
                Queue<byte> _dumpDatiSER = new Queue<byte>();

                Log.Debug("Inizio Ascolto " + timeout.ToString() + " - " + risposteAttese.ToString() + " - " + aspettaAck.ToString());

                bool _inAttesa = true;
                bool _echoRemoved = false;
                bool _echoJump = false;
              
                byte[] _msgBase = new byte[_mD.MessageBuffer.Length];
                _msgBase = _mD.MessageBuffer;

                _startFunzione = DateTime.Now;
                _startRicezione = DateTime.Now;
                do
                {
                    // verifico se ci sono dati
                    numBytesAvailable = serialeApparato.BytesToRead;

                    //serialeApparato.BytesToRead

                    if (numBytesAvailable > 0)
                    {
                        // Now that we have the amount of data we want available, read it
                        byte[] readData = new byte[numBytesAvailable];
                        int numBytesRead = serialeApparato.Read(readData, 0, numBytesAvailable);

                        Log.Debug("Dati Ricevuti da Display " + numBytesRead.ToString());
                        for (int _i = 0; _i < numBytesRead; _i++)
                        {
                            if (readData[_i] != 0x00)
                            {
                                codaDatiSER.Enqueue(readData[_i]);
                            }


                            if (readData[_i] == SerialMessage.serETX)
                            {
                                Log.Debug("Trovato Etx in pos " + _i.ToString() +  ", faccio ripartire il timeout");
                                _trovatoETX = true;
                                Log.Debug("Dati in coda DISP " + codaDatiSER.Count.ToString() + "; Echo Removed: " + _echoRemoved.ToString());
                                _startFunzione = DateTime.Now;
                            }

                            if (_trovatoETX)
                            {
                                Log.Debug("trovato ETX (" + _i.ToString() + ")");
                                _trovatoETX = false;
                                _echoJump = false;

                                // Verifico se è un echo e nel caso lo tolgo
                                if ( !_echoRemoved)
                                {
                                    _echoRemoved = true;
                                    bool _trovatoEcho = true;
                                    for (int _y = 0; _y < echoDatiSER.Count; _y++)
                                    {
                                        if(codaDatiSER.ElementAt(_y) !=echoDatiSER.ElementAt(_y))
                                        {
                                            _trovatoEcho = false;
                                            break;
                                        }
                                    }

                                    if (_trovatoEcho)
                                    {
                                        _dumpDatiSER.Clear();
                                        for (int _y = 0; _y < echoDatiSER.Count; _y++)
                                        {
                                           byte _TempB = codaDatiSER.Dequeue();
                                            _dumpDatiSER.Enqueue(_TempB);
                                        }

                                        Log.Debug("ECHO: " + FunzioniComuni.HexdumpQueue(_dumpDatiSER));
                                        
                                        _echoJump = true;
                                        _trovatoETX = false;
                                    }
                                    else
                                    {
                                        Log.Debug("---------------------------  NO ECHO: ----------------------------------------");
                                    }
                                    _inAttesa = true;
                                    //numBytesAvailable = serialeApparato.BytesToRead;

                                    Log.Debug("Dati in coda post Echo " + codaDatiSER.Count.ToString());

                                }

                                if (!_echoJump && !IgnoraContenuto)
                                {

                                    _msgRicevuto = analizzaCodaDisplay(SkipHead);


                                    Log.Debug("Dati in coda SB (USB) No Echo" + codaDatiSER.Count.ToString());
                                    Log.Debug(FunzioniComuni.HexdumpQueue(codaDatiSER));


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
                                                /* 
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
                                                */
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
                                if(IgnoraContenuto)
                                    _inAttesa = false;
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


        /// <summary>
        /// In base al messaggio ricevuto (codice comando) definisce l'azione e l'eventuale risposta
        /// </summary>
        /// <returns></returns>
        private SerialMessage.TipoRisposta analizzaCodaDisplay(bool SkipHead = false)
        {

            SerialMessage.EsitoRisposta _esito;
            bool _trovatoSTX = false;
            byte _tempByte;
            string testom = "";
            bool _inviaRisposta = true;
            SerialMessage.TipoRisposta _datiRicevuti = SerialMessage.TipoRisposta.NonValido;

            try
            {
                testom = "LUNGHEZZA CODA: " + codaDatiSER.Count();
                Log.Debug(testom);
                testom = "";

                while (codaDatiSER.Count() > 0)
                {
                    if (codaDatiSER.Contains(SerialMessage.serETX) == false)
                    {

                        Log.Debug("NON trovato ETX");
                        _datiRicevuti = SerialMessage.TipoRisposta.NonValido;
                        return _datiRicevuti;
                    }


                    _tempByte = codaDatiSER.Peek();
                    if (_tempByte != SerialMessage.serSTX)
                    {
                        _tempByte = codaDatiSER.Dequeue();
                    }
                    else
                    {
                        while (_tempByte != SerialMessage.serETX)
                        {
                            int lastByte = _dataBuffer.Length;
                            Array.Resize(ref _dataBuffer, lastByte + 1);
                            _tempByte = codaDatiSER.Dequeue();
                            _dataBuffer[lastByte] = _tempByte;
                            testom += _tempByte.ToString("X2");
                        }
                        Log.Debug(testom);
                        testom = "";
                        Log.Debug("Trovato ETX SB");
                        readingMessage = false;

                        _mD.MessageBuffer = _dataBuffer;
                        //-----------------------------------------------------------------------------------------
                        // Analizzo il contenuto del messaggio 
                        //-----------------------------------------------------------------------------------------
                        _esito = _mD.analizzaMessaggio(_dataBuffer,SkipHead);
                        UltimaRisposta = _esito; // SerialMessage.EsitoRisposta.MessaggioOk;
                        //-----------------------------------------------------------------------------------------

                        _inviaRisposta = true;
                        Log.Debug("Comando: --> 0x" + _mD._comando.ToString("X2"));
                        switch (_mD._comando)
                        {
                            case (byte)SerialMessage.TipoComando.ACK: 
                                Log.Debug("Comando Ricevuto");
                                _datiRicevuti = SerialMessage.TipoRisposta.Ack;
                                TipoRisposta = 1;
                                _inviaRisposta = false;

                                break;

                            case (byte)SerialMessage.TipoComando.NACK:  
                                TipoRisposta = 2;
                                UltimaRisposta = SerialMessage.EsitoRisposta.NonRiconosciuto;
                                _datiRicevuti = SerialMessage.TipoRisposta.Nack;
                                Log.Debug("Comando Errato: Ricevuto NAK");
                                _inviaRisposta = false;
                                break;

                            case (byte)SerialMessage.TipoComando.BREAK:  // 0x1C: //BREAK
                                TipoRisposta = 2;
                                UltimaRisposta = SerialMessage.EsitoRisposta.MessaggioOk;
                                _datiRicevuti = SerialMessage.TipoRisposta.Break;
                                Log.Debug("Comando Corretto: Ricevuto BREAK --> fermo gli invii");
                                _inviaRisposta = false;
                                break;


                            case (byte)SerialMessage.TipoComando.Start: // 0x0F:
                            case (byte)SerialMessage.TipoComando.Stop:  // 0xF0:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                Log.Debug("Risposta Start/Stop Comunicazione 0x" + _mD._comando.ToString("X2"));
                                _inviaRisposta = false;
                                break;

                            case (byte)SerialMessage.TipoComando.DI_Backlight:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                Log.Debug("Backlight");
                                break;

                            case (byte)SerialMessage.TipoComando.DI_R_LeggiMemoria:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                Log.Debug("Lettura Area Memoria");
                                break;
                            case (byte)SerialMessage.TipoComando.DI_W_SalvaImmagineMemoria:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                Log.Debug("Scrittura Testata Immagine");
                                break;
                            case (byte)SerialMessage.TipoComando.DI_Stato:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                Log.Debug("Lettura Stato");
                                break;

                            default:
                                _datiRicevuti = SerialMessage.TipoRisposta.Data;
                                _inviaRisposta = false;
                                Log.Debug("Altro Comando " + _mD._comando.ToString("X2"));
                                break;
                        }

                        if (_inviaRisposta)
                        {
                            Log.Debug("Esito: " + _mD._comando.ToString("X2"));

                                _mD._comando = (byte)SerialMessage.TipoComando.ACK ;
                                Log.Debug("Mandato ACK");
                            // _datiRicevuti = SerialMessage.TipoRisposta.Ack;
                            _mD._dispositivo = 0x0000;
                            _mD.componiRisposta(_dataBuffer, _esito);

                            //serialeApparato.Write(_mS.messaggioRisposta, 0, _mS.messaggioRisposta.Length);

                            scriviMessaggio(_mD.messaggioRisposta, 0, _mD.messaggioRisposta.Length);
                            Log.Debug(_mD.hexdumpArray(_mD.messaggioRisposta));
                        }
                        _rxRisposta = true;
                        Array.Resize(ref _dataBuffer, 0);
                        //return _datiRicevuti;
                    }

                }
                return _datiRicevuti;
            }

            catch (Exception Ex)
            {
                Log.Error("analizzaCodaDisplay " + Ex.Message);
                _datiRicevuti = SerialMessage.TipoRisposta.NonValido;
                return _datiRicevuti;
            }

        }


     

    }




}

