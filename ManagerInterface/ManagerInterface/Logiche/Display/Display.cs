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
        private parametriSistema _parametri;

        private static Queue<byte> codaDatiSER = new Queue<byte>();  // Buffer per la ricezione dati seriali
        private static Queue<byte> echoDatiSER = new Queue<byte>();  // Buffer per la ricezione dati seriali


        
        public List<DisplaySetup.Immagine> Immagini = new List<DisplaySetup.Immagine>();
        public List<DisplaySetup.Schermata> Schermate = new List<DisplaySetup.Schermata>();
        public List<DisplaySetup.Variabile> Variabili = new List<DisplaySetup.Variabile>();
        
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        private int _timeOut = 10;
        private DateTime _startRead;
        private SerialMessage.TipoRisposta _ultimaRisposta = SerialMessage.TipoRisposta.NonValido;   // flag per l'indicazioene del tipo dell'ultimo messaggio ricevuto dalla scheda
        public const byte DimBloccoDati = 200;


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

                _mD.Comando = SerialMessage.TipoComando.Start;
                _mD._comando = (byte)SerialMessage.TipoComando.Start;
                _mD.ComponiMessaggio();
                _rxRisposta = false;
                Log.Debug("Display START");
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

        public bool ImpostaBacklight(bool Acceso)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.DI_Backlight;
                _mD._comando = (byte)SerialMessage.TipoComando.DI_Backlight;
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

        public bool ImpostaLed(byte Red,byte Green, byte Blu, byte On, byte Off, byte RedDx, byte GreenDx, byte BluDx, byte OnDx, byte OffDx)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.DI_LedRGB;
                _mD._comando = (byte)SerialMessage.TipoComando.DI_LedRGB;
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

                _mD.Comando = SerialMessage.TipoComando.DI_DrawLine;
                _mD._comando = (byte)SerialMessage.TipoComando.DI_DrawLine;
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


                _mD.Comando = SerialMessage.TipoComando.DI_R_LeggiMemoria;
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
                bool _esito;

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

                _mD.Comando = SerialMessage.TipoComando.DI_W_SalvaImmagineMemoria;
                _mD._comando = (byte)SerialMessage.TipoComando.DI_W_SalvaImmagineMemoria;
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
                    byte _dimCorrente = 0;

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
                        Log.Debug("Display - Invio Corpo immagine; trasmissione di " + _dimCorrente.ToString() + " bites dal punto " + _currPos.ToString());
                        Log.Debug(_mD.hexdumpMessaggio());
                        echoDatiSER.Clear();
                        for (int i = 0; i < _mD.MessageBuffer.Length; i++)
                        {
                            echoDatiSER.Enqueue(_mD.MessageBuffer[i]);
                        }
                        scriviMessaggio(_mD.MessageBuffer, 0, _mD.MessageBuffer.Length);
                        _esito = aspettaRisposta(elementiComuni.TimeoutBase, 0, true);

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
                

                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("DisegnaLinea: " + Ex.Message);
                return _risposta;
            }
        }


        public bool MostraImmagine(ushort Id, byte PosX, byte PosY, byte Color)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.DI_MostraImmagine;
                _mD._comando = (byte)SerialMessage.TipoComando.DI_MostraImmagine;
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

                _mD.Comando = SerialMessage.TipoComando.DI_CancellaDisplay;
                _mD._comando = (byte)SerialMessage.TipoComando.DI_CancellaDisplay;
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


        public bool CaricaListaImmaginiPresenti(ushort Start,ushort Stop, bool ElencaVuote = false, bool CaricaBitmap = false)
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

                Immagini.Clear();

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

                    Thread.Sleep(500);

                }

                return _risposta;
            }
            catch (Exception Ex)
            {
                Log.Error("VerificaPresenza: " + Ex.Message);
                return _risposta;
            }
        }

    }

}
