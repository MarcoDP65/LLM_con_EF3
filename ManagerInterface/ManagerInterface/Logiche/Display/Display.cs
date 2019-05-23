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
        public enum StatoScheda : byte { NonCollegata = 0x00, SoloBootloader = 0x01, BLandFW = 0x02, SoloFW = 0x03 };

        public static SerialPort serialeApparato;
        private static MessaggioDisplay _mD; // = new MessaggioSpyBatt();
 //     private parametriSistema _parametri;

        private static Queue<byte> codaDatiSER = new Queue<byte>();  // Buffer per la ricezione dati seriali
        private static Queue<byte> echoDatiSER = new Queue<byte>();  // Buffer per la ricezione dati seriali

        public List<DisplaySetup.Immagine> Immagini = new List<DisplaySetup.Immagine>();
        public List<DisplaySetup.Schermata> Schermate = new List<DisplaySetup.Schermata>();
        public List<DisplaySetup.Variabile> Variabili = new List<DisplaySetup.Variabile>();
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public DisplaySetup.StatoDisplay StatoAttualeScheda = new DisplaySetup.StatoDisplay();
        private int _timeOut = 10;
        private DateTime _startRead;
        private SerialMessage.TipoRisposta _ultimaRisposta = SerialMessage.TipoRisposta.NonValido;   // flag per l'indicazioene del tipo dell'ultimo messaggio ricevuto dalla scheda
        public const ushort DimBloccoDati = 256;


        //internal delegate void SerialDataReceivedEventHandlerDelegate(object sender, SerialDataReceivedEventArgs e);
        delegate void SetTextCallback(string text);
        string InputData = String.Empty;
        byte[] _dataBuffer = new byte[0];
        int lastByte = 0;
        bool readingMessage = false;
        public int TipoRisposta;
        static bool _rxRisposta;
        public SerialMessage.EsitoRisposta UltimaRisposta;
        public DateTime UltimaScrittura;   // Registro l'istante dell'ultima scrittura
        public byte[] numeroSeriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
        string _idCorrente;
        public bool apparatoPresente = false;
        public DisplaySetup Data = new DisplaySetup();

        public UnitaDisplay(ref SerialPort PortaSeriale )
            {
            _mD = new MessaggioDisplay();
            _mD.Dispositivo = MessaggioSpyBatt.TipoDispositivo.Charger;
            byte[] Seriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] numeroSeriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
            _mD.SerialNumber = Seriale;
            serialeApparato = PortaSeriale;
            //serialeApparato.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceivedDisplay);
            Immagini.Clear();
            Schermate.Clear();
            Variabili.Clear();


        }

        private void port_DataReceivedDisplay(object sender, SerialDataReceivedEventArgs e)
        {
            //string testom = "Dati Ricevuti SB: " + serialeApparato.BytesToRead.ToString();
            bool _trovatoETX = false;
            byte[] data = new byte[serialeApparato.BytesToRead];
            serialeApparato.Read(data, 0, data.Length);
            Log.Debug("Dati Ricevuti SB " + data.Length.ToString());
            for (int _i = 0; _i < data.Length; _i++)
            {
                codaDatiSER.Enqueue(data[_i]);
                if (data[_i] == SerialMessage.serETX)
                {
                    _trovatoETX = true;
                }
            }
            if (_trovatoETX)
            {
                Log.Debug("trovato ETX");
                analizzaCodaDisplay();
            }

        }

        /// <summary>
        /// Apre il canale di cominicazione col display. True se riceve la risposta di ritotno dal dispositivo
        /// </summary>
        /// <returns></returns>
        public bool VerificaPresenza()
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_CONNECT;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_CONNECT;
                _mD.ComponiMessaggio();
                _rxRisposta = false;

                Log.Debug("Display START");
                Log.Debug(_mD.hexdumpMessaggio());

                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true); 
                if ((_esito) && (_ultimaRisposta == SerialMessage.TipoRisposta.Ack))
                //   (_mS._comando == (byte)(MessaggioSpyBatt.TipoComando.ACK_SB))  
                {
                    _idCorrente = _mD.idCorrente;
                    numeroSeriale = _mD.arrayIdCorrente;
                    UltimaScrittura = DateTime.Now;
                    apparatoPresente = true;
                    _risposta = true;
                }
                return _risposta;
            }

            catch (Exception Ex)
            {
                Log.Error("VerificaPresenza: " + Ex.Message);
                return _risposta;
            }
        }

        /// <summary>
        /// Imposta la retroilluminazione del display (istantanea).
        /// </summary>
        /// <param name="Acceso">if set to <c>true</c> [acceso].</param>
        /// <returns></returns>
        public bool ImpostaBacklight(bool Acceso)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_BACKLIGHT;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_BACKLIGHT;
                _mD.ComponiMessaggioBacklight(Acceso);
                _rxRisposta = false;
                Log.Debug("Display Backlight: " + Acceso.ToString());
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("ImpostaBacklight: " + Ex.Message);
                return _risposta;
            }
        }

        /// <summary>
        /// Accende i led in base ai parametri passati.
        /// </summary>
        /// <param name="Red">LED red sx.</param>
        /// <param name="Green">LED green sx.</param>
        /// <param name="Blu">LED blu sx.</param>
        /// <param name="On">Tempo accensione led SX.</param>
        /// <param name="Off">Tempo spegnimento led SX.</param>
        /// <param name="RedDx">LED red dx.</param>
        /// <param name="GreenDx">LED green dx.</param>
        /// <param name="BluDx">LED blu dx.</param>
        /// <param name="OnDx">Tempo accensione led DX.</param>
        /// <param name="OffDx">Tempo spegnimento led DX.</param>
        /// <returns></returns>
        public bool ImpostaLed(byte Red,byte Green, byte Blu, byte On, byte Off, byte RedDx, byte GreenDx, byte BluDx, byte OnDx, byte OffDx)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_LED_RGB;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_LED_RGB;
                _mD.ComponiMessaggioLed(Red, Green, Blu, On, Off, RedDx , GreenDx, BluDx, OnDx, OffDx);
                _rxRisposta = false;
                Log.Debug("Display Led: ");
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                 return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("ImpostaBacklight: " + Ex.Message);
                return _risposta;
            }
        }

        public bool DisegnaLinea(byte Xstart, byte Ystart, byte Xend, byte Yend, byte Color)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_DRAW_LINE;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_DRAW_LINE;
                _mD.ComponiMessaggioTracciaLinea(Xstart, Ystart, Xend, Yend, Color);
                _rxRisposta = false;
                Log.Debug("Display traccia linea: ");
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("DisegnaLinea: " + Ex.Message);
                return _risposta;
            }
        }

        /// <summary>
        /// Carico direttamente da memoria l'area passata come parametro
        /// </summary>
        /// <param name="StartAddr">Indirizzo (iniziale) del blocco da leggere</param>
        /// <param name="NumByte">Numero di byte da leggere (max 242)</param>
        /// <param name="Dati">bytearray dati letti</param>
        /// <returns></returns>
        public bool LeggiBloccoMemoria(uint StartAddr, ushort NumByte, out byte[] Dati)
        {


            try
            {
                bool _esito;

                if (NumByte < 1) NumByte = 1;
                if (NumByte > 242)
                {
                    Dati = null;
                    return false;
                }

                Dati = new byte[NumByte];


                _mD.Comando = SerialMessage.TipoComando.CMD_READ_MEMORY;
                _mD._pacchettoMem = new MessaggioDisplay.PacchettoReadMem();

                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Lettura di " + NumByte.ToString() + " bytes dall'indirizzo " + StartAddr.ToString("X2"));

                _mD.ComponiMessaggioLeggiMem(StartAddr, NumByte);
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                _rxRisposta = false;
                _startRead = DateTime.Now;
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                // Log.Debug(_mS.hexdumpMessaggio());
                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Risposta Lettura memoria:");
                Log.Debug(_mD.hexdumpArray(_mD._pacchettoMem.memDataDecoded));

                for (int _ciclo = 0; ((_ciclo < NumByte) && (_ciclo < _mD._pacchettoMem.numBytes)); _ciclo++)
                {

                    Dati[_ciclo] = _mD._pacchettoMem.memDataDecoded[_ciclo];
                }

                return _esito;


            }


            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                Dati = null;
                return false;
            }
        }

        /// <summary>
        /// Cancella l'intera memoria flash (4MB). Cancella snche i dati relativi alla scheda corrente dal DB Locale
        /// </summary>
        /// <returns></returns>
        public bool CancellaInteraMemoria()
        {
            try
            {
                bool _esito = false ;
                /*
                  
                Temporaneamente disabilitata 

                _mD.Comando = SerialMessage.TipoComando.DI_CancellaInteraMemoria;


                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("SerialMessage.TipoComando.DI_CancellaInteraMemoria");

                _mD.ComponiMessaggio();
                Log.Debug(_mD.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, true);

                // prima di proseguire aspetto 1 secondo
                System.Threading.Thread.Sleep(1000);
                Application.DoEvents();


                Log.Debug(_mD.hexdumpMessaggio());
                Log.Debug("------------------------------------------------------------------------------------------------------------");
                */
                return _esito;


            }


            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Carica l'immagine passata come parametro sulla memoria esterna del dispositivo
        /// </summary>
        /// <param name="Img"></param>
        /// <returns></returns>
        public bool CaricaImmagine(DisplaySetup.Immagine Img)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;
                int NumBlocco = 0;

                //Prima mi accerto che il bytearray sia pronto


                _mD.Comando = SerialMessage.TipoComando.CMD_IMAGE_RX_START;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_IMAGE_RX_START;
                _mD.ComponiMessaggioInviaTestataImmagine(Img);
                _rxRisposta = false;
                Log.Debug("Display- Invio Testata Immagine");
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                if(_esito)
                {
                    // se la testata è arrivata, mando il corpo dell'immagine
                    byte[] _tmpBlocco;
                    ushort _currPos = 0;
                    ushort _dimCorrente = 0;

                    if (Img.Size > DimBloccoDati)
                    {
                        _dimCorrente = DimBloccoDati;
                    }
                    else
                    {
                        _dimCorrente = (byte)Img.Size;
                    }


                    while (_dimCorrente > 0)
                    {
                        _tmpBlocco = new byte[_dimCorrente];
                        Array.Copy(Img.ImageBuffer, _currPos, _tmpBlocco, 0, _dimCorrente);

                        _mD.ComponiMessaggioPacchettoImmagine(_dimCorrente, _tmpBlocco);
                        _rxRisposta = false;
                        Log.Debug("----------------------------------------------------------------------------------------------------");
                        Log.Debug("- Blocco Immagine N° " + ++NumBlocco);
                        Log.Debug("----------------------------------------------------------------------------------------------------");
                        Log.Debug("Display - Invio Corpo immagine; trasmissione di " + _dimCorrente.ToString() + " bites dal punto " + _currPos.ToString());
                        Log.Debug(_mD.hexdumpMessaggio());
                        echoDatiSER.Clear();
                        for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                        {
                            echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                        }
                        scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                        _esito = aspettaRisposta(elementiComuni.TimeoutLungo, 0, true);
                        if (!_esito)
                        {
                            Log.Debug("- Blocco Immagine N° " + ++NumBlocco + " trasferimento falito");
                            return _esito;

                        }
                        else
                        {

                            _currPos += _dimCorrente;
                            int _residuo = Img.Size - _currPos;

                            if (_residuo > DimBloccoDati)
                            {
                                _dimCorrente = DimBloccoDati;
                            }
                            else
                            {
                                _dimCorrente = (byte)_residuo;
                            }
                        }

                    }

                }
                

                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("CaricaImmagine: " + Ex.Message);
                return _risposta;
            }
        }

        /// <summary>
        /// Mostra l'immagine selezionata.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="PosX">The position x.</param>
        /// <param name="PosY">The position y.</param>
        /// <param name="Color">The color.</param>
        /// <returns></returns>
        public bool MostraImmagine(ushort Id, byte PosX, byte PosY, byte Color)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_DRAW_IMAGE;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_DRAW_IMAGE;
                _mD.ComponiMessaggioMostraImmagine(Id, PosX, PosY, Color);
                _rxRisposta = false;
                Log.Debug("Display mostra immagine: ");
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("MostraImmagine: " + Ex.Message);
                return _risposta;
            }
        }

        /// <summary>
        /// Imposta l'RTC del display con data/ora  locali del PC.
        /// </summary>
        /// <returns></returns>
        public bool ImpostaRTC()
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_UPDATE_RTC;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_UPDATE_RTC;
                _mD.ComponiMessaggioSetRTC();
                _rxRisposta = false;
                Log.Debug("Display Set RTC: ");
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("ImpostaRTC: " + Ex.Message);
                return _risposta;
            }
        }


        /// <summary>
        /// Imposta il baudrate della porta 485 del display.
        /// Il comando ritorna un ack alla velocità corrente poi chiude la comunicazione. 
        /// La riconnessione dovrà avvenire alla nuova velocità
        /// </summary>
        /// <param name="Velocita">codice della nuova velocità.</param>
        /// <returns></returns>
        public bool ImpostaBaudrate(DisplaySetup.BaudRate Velocita)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_UART_SWITCH_BDRATE;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_UART_SWITCH_BDRATE;
                _mD.ComponiMessaggioBaudRate(Velocita);
                _rxRisposta = false;
                Log.Debug("Display Set BaudRate: ");
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("ImpostaBaudrate: " + Ex.Message);
                return _risposta;
            }
        }

        public bool MostraSchermata(ushort Id)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_DRAW_SCREEN;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_DRAW_SCREEN;
                _mD.ComponiMessaggioMostraSchermata(Id);
                _rxRisposta = false;
                Log.Debug("Display mostra Schermata: ");
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("DisegnaLinea: " + Ex.Message);
                return _risposta;
            }
        }

        public bool ScrollSchermate(byte[] ListaSch , byte Attesa )
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_SCROLL_SCREEN;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_SCROLL_SCREEN;
                _mD.ComponiMessaggioScrollSchermate(ListaSch, Attesa);
                _rxRisposta = false;
                Log.Debug("ScrollSchermate: ");
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("DisegnaLinea: " + Ex.Message);
                return _risposta;
            }
        }

        public bool PulisciSchermo()
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_CLEAR_DISPLAY;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_CLEAR_DISPLAY;
                _mD.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("Display CLS");
                Log.Debug(_mD.hexdumpMessaggio());
                /*
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                */
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                if ((_esito) && (_ultimaRisposta == SerialMessage.TipoRisposta.Ack))
                //   (_mS._comando == (byte)(MessaggioSpyBatt.TipoComando.ACK_SB))  
                {
                    _risposta = true;
                }
                return _risposta;
            }

            catch (Exception Ex)
            {
                Log.Error("VerificaPresenza: " + Ex.Message);
                return _risposta;
            }
        }

        public bool ResetScheda()
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_RESET_BOARD;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_RESET_BOARD;
                _mD.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("Display Reset Board");
                Log.Debug(_mD.hexdumpMessaggio());

                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                if ((_esito) && (_ultimaRisposta == SerialMessage.TipoRisposta.Ack))
                {
                    _risposta = true;
                }
                return _risposta;
            }

            catch (Exception Ex)
            {
                Log.Error("VerificaPresenza: " + Ex.Message);
                return _risposta;
            }
        }

        public bool LeggiStatoScheda()
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_STATE_DEVICE;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_STATE_DEVICE;
                _mD.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("Display Read State");
                Log.Debug(_mD.hexdumpMessaggio());

                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);

                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1,false);

                if ((_esito)) // && (_ultimaRisposta == SerialMessage.TipoRisposta.Ack))
                {
                    if (_mD._statoScheda != null)
                    {
                        StatoAttualeScheda.Pulsante1 = _mD._statoScheda.Pulsante1;
                        StatoAttualeScheda.Pulsante2 = _mD._statoScheda.Pulsante2;
                        StatoAttualeScheda.Pulsante3 = _mD._statoScheda.Pulsante3;
                        StatoAttualeScheda.Pulsante4 = _mD._statoScheda.Pulsante4;
                        StatoAttualeScheda.Pulsante5 = _mD._statoScheda.Pulsante5;
                        _risposta = true;
                    }
                }
                return _risposta;




            }

            catch (Exception Ex)
            {
                Log.Error("VerificaPresenza: " + Ex.Message);
                return _risposta;
            }
        }

        public bool CaricaListaImmaginiPresenti(ushort Start,ushort Stop, bool ElencaVuote = false, bool CaricaBitmap = false, bool VuotaLista = true)
        {
            bool _risposta = false;
            bool _esitoRead = false;
            byte[] _buffDati = new byte[12];
            try
            {

                if (Stop > 256)
                {
                    Stop = 256;
                }

                if (Start> Stop)
                {
                    Start = Stop;
                }

                if (VuotaLista)Immagini.Clear();


                for (ushort _imgCount = Start; _imgCount <= Stop; _imgCount++)
                {
                    uint _addrImg;
                    _addrImg = (uint)( 0x2000 + (0x1000 * _imgCount));
                    _esitoRead = LeggiBloccoMemoria(_addrImg, 12,out _buffDati);
                    if(_esitoRead)
                    {
                        DisplaySetup.Immagine _tmpImg = new DisplaySetup.Immagine();
                        _tmpImg.Id = _imgCount;
                        _tmpImg.Nome = FunzioniComuni.ArrayToString(_buffDati, 0, 8);
                        _tmpImg.Size = FunzioniComuni.ArrayToUshort(_buffDati, 8, 2);
                        _tmpImg.Width = _buffDati[10];
                        _tmpImg.Width = _buffDati[11];
                        Immagini.Add(_tmpImg);
                        _risposta = true;
                        
                    }
                    //Application.DoEvents();
                    //Thread.Sleep(500);
                }

                return _risposta;
            }
            catch (Exception Ex)
            {
                Log.Error("VerificaPresenza: " + Ex.Message);
                return _risposta;
            }
        }

        public bool CaricaSchermata (DisplaySetup.Schermata Screen)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                if (Screen == null)
                {
                    Log.Info("CaricaSchermata: richiesto inserimento schermata nulla");
                    return false;
                }
                else
                {
                    Log.Info("CaricaSchermata: richiesto inserimento schermata " + Screen.Id.ToString());
                }

                if (Screen.Id != 0)
                {

                    // Step 0 - verificare se necessario: cancellazione memoria

                    // Step 1 : testata schermata
                    
                _mD.Comando = SerialMessage.TipoComando.CMD_SCREEN_RX_START;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_SCREEN_RX_START;
                _mD.ComponiMessaggioInviaTestataSchermata(Screen);
                _rxRisposta = false;
                Log.Debug("Display- Invio Testata Schermata");
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);
                    if (_esito)
                    {
                        // se la testata è arrivata, mando il corpo dell'immagine
                        byte[] _tmpBlocco;
                        ushort _currPos = 0;
                        ushort _dimCorrente = 0;

                        if (Screen.Size > DimBloccoDati)
                        {
                            _dimCorrente = DimBloccoDati;
                        }
                        else
                        {
                            _dimCorrente = Screen.Size;
                        }


                        while (_dimCorrente > 0)
                        {
                            //_tmpBlocco = new byte[_dimCorrente];

                            // Comunque compongo un pacchetto alla dimensione max
                            _tmpBlocco = new byte[DimBloccoDati];

                            Array.Copy(Screen.ImageBuffer, _currPos, _tmpBlocco, 0, _dimCorrente);

                            _mD.ComponiMessaggioPacchettoImmagineScreen(DimBloccoDati, _tmpBlocco);
                            _rxRisposta = false;
                            Log.Debug("Display - Invio Corpo immagine; trasmissione di " + _dimCorrente.ToString() + " bites dal punto " + _currPos.ToString());
                            Log.Debug(_mD.hexdumpMessaggio());
                            echoDatiSER.Clear();
                            for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                            {
                                echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                            }
                            scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                            _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true, false, false, false,false);
                            // Se fallisce un pacchetto esco con esito negativo
                            if(!_esito)
                            {
                                Log.Debug("Trasmissione blocco " + _currPos.ToString() );
                                return _esito;
                            }

                            _currPos += _dimCorrente;
                            int _residuo = Screen.Size - _currPos;

                            if (_residuo > DimBloccoDati)
                            {
                                _dimCorrente = DimBloccoDati;
                            }
                            else
                            {
                                _dimCorrente = (ushort)_residuo;
                            }


                        }

                        // 2 comandi fissi
                        foreach (DisplaySetup.Comando _tmpCmd in Screen.Comandi)
                        {
                            byte LenPacchetto = _tmpCmd.ComponiByteArray();
                  
                            _mD.ComponiMessaggioPacchettoImmagineScreen(LenPacchetto, _tmpCmd.ArrayComando);
                            _rxRisposta = false;
                            Log.Debug("Display - Invio pacchetto Comando " + _tmpCmd.Numero.ToString() + " - " + _tmpCmd.DescAttivita);
                            Log.Debug(_mD.hexdumpMessaggio());
                            echoDatiSER.Clear();
                            for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                            {
                                echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                            }
                            scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                            _esito = aspettaRisposta(elementiComuni.TimeoutLungo,1, true,false,false,false,true);
                            if (!_esito)
                            {
                                Log.Debug("Trasmissione blocco " + _currPos.ToString());
                                return _esito;
                            }
                        }


                    }




                }


                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("CaricaSchermata: " + Ex.Message);
                return _risposta;
            }

        }

        public bool ImpostaVariabile(byte Id, string Valore)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.CMD_RX_VARIABLE;
                _mD._comando = (byte)SerialMessage.TipoComando.CMD_RX_VARIABLE;
                _mD.ComponiMessaggioInviaVariabile(Id,Valore);
                _rxRisposta = false;
                Log.Debug("Display imposta Variabile: ");
                Log.Debug(_mD.hexdumpMessaggio());
                echoDatiSER.Clear();
                for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                {
                    echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                }
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutLungo, 0, true);
                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("DisegnaLinea: " + Ex.Message);
                return _risposta;
            }
        }


        /// <summary>
        /// Cancella fisicamente un blocco di 4K dalla memoria flash mettendo tutti i Bytes a 0xFF
        /// </summary>
        /// <returns></returns>
        public bool CancellaBlocco4K(uint StartAddr)
        {


            try
            {
                bool _esito;
                ///ControllaAttesa(UltimaScrittura);

                _mD.Comando = SerialMessage.TipoComando.CMD_ERASE_4K_MEM;

                Log.Debug("-----------------------------------------------------------------------------------------------------------");
                Log.Debug("Cancellazione di 4Kbytes dall'indirizzo " + StartAddr.ToString("X2"));

                _mD.ComponiMessaggioCancella4KMem(StartAddr);
                Log.Debug(_mD.hexdumpMessaggio());
                _rxRisposta = false;
                _startRead = DateTime.Now;
                scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);

                //Log.Debug("------------------------------------------------------------------------------------------------------------");

                return _esito;


            }


            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return false;
            }
        }





    }

}
