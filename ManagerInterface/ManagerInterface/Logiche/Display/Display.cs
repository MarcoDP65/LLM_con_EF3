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
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        private int _timeOut = 10;
        private DateTime _startRead;
        private SerialMessage.TipoRisposta _ultimaRisposta = SerialMessage.TipoRisposta.NonValido;   // flag per l'indicazioene del tipo dell'ultimo messaggio ricevuto dalla scheda


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



        public UnitaDisplay(ref SerialPort PortaSeriale )
            {
            _mD = new MessaggioDisplay();
            _mD.Dispositivo = MessaggioSpyBatt.TipoDispositivo.Charger;
            byte[] Seriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] numeroSeriale = { 0, 0, 0, 0, 0, 0, 0, 0 };
            _mD.SerialNumber = Seriale;
            serialeApparato = PortaSeriale;
            //serialeApparato.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceivedDisplay);


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


        public bool ImpostaLed(byte Red,byte Green, byte Blu, byte On, byte Off)
        {
            bool _risposta = false;

            try
            {
                bool _esito = false;

                _mD.Comando = SerialMessage.TipoComando.DI_LedRGB;
                _mD._comando = (byte)SerialMessage.TipoComando.DI_LedRGB;
                _mD.ComponiMessaggioLed(Red, Green, Blu, On, Off);
                _rxRisposta = false;
                Log.Debug("Display Led: ");
                Log.Debug(_mD.hexdumpMessaggio());
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


        public bool scriviMessaggio(byte[] messaggio, int Start, int NumByte)
        {

            try
            {

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





    }

}
