using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

using Utility;


namespace ChargerLogic
{
    public partial class MessaggioDisplay : SerialMessage
    {

        public enum ComandoInvioSchermata : byte
        {
            DIS_ScriviTesto68 = 0x68,
            DIS_ScriviOra68 = 0x64,
            DIS_ScriviData68 = 0x66,
            DIS_ScriviVariabile68 = 0xC6,
            DIS_ScriviTesto16 = 0x16,
            DIS_ScriviVariabile16 = 0xC6,
            DIS_DisegnaImmagine = 0x5A,
            DIS_ScrollImmagini = 0x5C,
        };

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        private EsitoRisposta _ultimaRisposta = EsitoRisposta.MessaggioVuoto;

        private byte[] _idCorrente = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private byte[] _tempId = { 0, 0, 0, 0, 0, 0, 0, 0 };

        public string idCorrente
        {
            get
            {
                string _tempId = "";
                for (int _c = 0; _c < 8; _c++)
                {
                    // if (_c > 0) _tempId += ":";
                    _tempId += _idCorrente[_c].ToString("X2");
                }
                return _tempId;
            }
        }

        public byte[] arrayIdCorrente
        {
            get
            {
                return _idCorrente;
            }
        }

        public PacchettoReadMem _pacchettoMem = new PacchettoReadMem();
        public StatoScheda _statoScheda = new StatoScheda();

        public EsitoRisposta analizzaMessaggio(byte[] _messaggio, bool skipHead = false)
        {
            byte _ret;
            int _startPos;
            int _endPos;
            byte[] _buffArray;
            ushort _tempShort;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            EsitoRisposta _risposta;
            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);
            int preambleLenght;

            try
            {
                messaggioRisposta = new byte[29];

                if (_messaggio.Length < 3)
                { return EsitoRisposta.MessaggioVuoto; }

                // scompongo l'intestazione
                // STX
                _ret = _messaggio[0];
                if (_ret != serSTX) return EsitoRisposta.NonRiconosciuto;

                // POi verifico il CRC
                _endPos = _messaggio.Length;
                _startPos = _endPos - 6;
                if (_messaggio[_startPos] != serENDPAC)
                {
                    return EsitoRisposta.NonRiconosciuto;
                }
                _buffArray = new byte[_startPos - 1];

                // controllo CRC
                Array.Copy(_messaggio, 1, _buffArray, 0, (_startPos - 1));
                _startPos++;
                _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                _tempShort = (ushort)(_ret);
                _startPos += 2;
                _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                _tempShort = (ushort)((_tempShort << 8) + _ret);
                _crc = codCrc.ComputeChecksum(_buffArray);

                if (_crc != _tempShort)
                {
                    Log.Debug("analizzaMessaggio Display: BAD CRC");
                    return EsitoRisposta.BadCRC;
                }


                if (skipHead == false)
                {
                    //seriale
                    _startPos = 1;
                    SerialNumber = new byte[8];
                    for (int ciclo = 0; ciclo < 8; ciclo++)
                    {
                        SerialNumber[ciclo] = decodificaByte(_messaggio[_startPos + (2 * ciclo)], _messaggio[_startPos + (2 * ciclo) + 1]);
                    }

                    //tipo
                    _tempId = SerialNumber;
                    _idCorrente = SerialNumber;
                    _startPos = 17;
                    _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                    _dispositivo = (ushort)(_ret);
                    _startPos = 19;
                    _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                    _dispositivo = (ushort)((_dispositivo << 8) + _ret);
                    Dispositivo = 0; // Enum.ToObject(typeof(TipoDispositivo), _dispositivo);
                    _startPos = 21;
                    preambleLenght = 22;
                }
                else
                {

                    SerialNumber = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    Dispositivo = 0;
                    _startPos = 1;
                    preambleLenght = 2;
                }



                // comando
                //_startPos = 21;
                _comando = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                if (_comando == 0x58)
                {
                    Log.Debug("Messaggio Display ricevuto: " + hexdumpArray(_messaggio));
                }
                Log.Debug("Messaggio Display ricevuto: " + _comando.ToString("X2"));

                //preparo la risposta: il preambolo è uguale al messaggio 
                if (!skipHead)
                Array.Copy(_messaggio, messaggioRisposta, 20);


                // ora in base al comando cambio faccio lettura:
                switch (_comando)
                {
                    case (byte)TipoComando.ACK_PACKET:
                        {
                            Log.Debug("Display: ACK");
                            break;
                        }
                        
                    case (byte)TipoComando.NACK_PACKET:
                        {
                            Log.Debug("Display: --> NACK <---");
                            _crc = 0;
                            break;
                        }

                    case (byte)TipoComando.CMD_CONNECT:
                        {
                            Log.Debug("Display: START Comunicazione");
                            _crc = 0;
                            break;
                        }

                    case (byte)TipoComando.CMD_READ_MEMORY: // Lettura frammento memoria
                        {

                            // leggo la parte dati
                            _pacchettoMem = new PacchettoReadMem();
                            _buffArray = new byte[(_endPos - (preambleLenght + 7))];
                            if (_comando != (byte)TipoComando.CMD_READ_MEMORY)
                            {   // dove non ho la testata corretta recupero i 2 bytes del comando
                                Array.Copy(_messaggio, preambleLenght - 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            }
                            else
                            {
                                Array.Copy(_messaggio, preambleLenght + 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            }
                            Log.Debug("Display: Readmem " + hexdumpArray(_buffArray));
                            _risposta = _pacchettoMem.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.CMD_IMAGE_RX_START:
                        break;
                    case (byte)TipoComando.CMD_DRAW_SCREEN:
                        break;
                    case (byte)TipoComando.CMD_RESET_BOARD:
                        break;
                    case (byte)TipoComando.CMD_STATE_DEVICE:
                        {

                            // leggo la parte dati
                            _statoScheda = new StatoScheda();
                            _buffArray = new byte[(_endPos - (preambleLenght + 7))];
                            Array.Copy(_messaggio, preambleLenght + 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            Log.Debug("Display: Read Stato " + hexdumpArray(_buffArray));
                            _risposta = _statoScheda.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    default:
                        {
                            return EsitoRisposta.NonRiconosciuto;
                        }
                }

                return EsitoRisposta.MessaggioOk;

            }
            catch ( Exception Ex)
            {
                Log.Debug("analizzaMessaggio " + Ex.Message);
                return EsitoRisposta.ErroreGenerico;
            }

        }

        public bool ComponiMessaggioBacklight(bool Acceso)
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_BACKLIGHT);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 2;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                // id programma: Stato Illuminazione 00-> Spento / FF -> Acceso
                byte _statoLuci;

                if (Acceso)
                    _statoLuci = 0xFF;
                else
                    _statoLuci = 0x00;

                splitUshort(codificaByte(_statoLuci), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;
                MessageBuffer[_arrayLen + 6] = serETX;


                return true;
            }
            catch { return _esito; }
        }

        public bool ComponiMessaggioBaudRate(DisplaySetup.BaudRate Velocita)
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_UART_SWITCH_BDRATE);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 2;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                // id programma: Stato Illuminazione 00-> Spento / FF -> Acceso

                splitUshort(codificaByte((byte)Velocita), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;
                MessageBuffer[_arrayLen + 6] = serETX;


                return true;
            }
            catch { return _esito; }
        }

        /// <summary>
        /// Compone il messaggio per l'invio all' RTC del display l'ora locale del PC.
        /// </summary>
        /// <returns></returns>
        public bool ComponiMessaggioSetRTC()
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_UPDATE_RTC);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 16;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                DateTime OraPC = DateTime.Now;

                ushort _tempUs =  (ushort)OraPC.Year;

                // Anno
                splitUshort(_tempUs, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                _arrayInit += 4;

                byte _tempByte;

                //Mese
                _tempByte =  (byte)OraPC.Month;

                splitUshort(codificaByte(_tempByte), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                //Giorno
                _tempByte =  (byte)OraPC.Day;

                splitUshort(codificaByte(_tempByte), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                //Giorno della settimana
                _tempByte = 2; //(byte)OraPC.DayOfWeek;

                splitUshort(codificaByte(_tempByte), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                //ore
                _tempByte = (byte)OraPC.Hour;

                splitUshort(codificaByte(_tempByte), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;


                //Minuti
                _tempByte = (byte)OraPC.Minute;

                splitUshort(codificaByte(_tempByte), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                //secondi
                _tempByte = (byte)OraPC.Second;

                splitUshort(codificaByte(_tempByte), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;



                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;
                MessageBuffer[_arrayLen + 6] = serETX;


                return true;
            }
            catch { return _esito; }
        }

        /// <summary>
        /// Compone il messaggio di comando LED. I ai due canali vengono inviati gli stessi valori.
        /// .
        /// </summary>
        /// <param name="ValRed">The value red.</param>
        /// <param name="ValGreen">The value green.</param>
        /// <param name="ValBlue">The value blue.</param>
        /// <param name="TimeOn">The time on.</param>
        /// <param name="TimeOff">The time off.</param>
        /// <returns></returns>
        public bool ComponiMessaggioLed(byte ValRed, byte ValGreen, byte ValBlue, byte TimeOn, byte TimeOff, byte ValRedDx, byte ValGreenDx, byte ValBlueDx, byte TimeOnDx, byte TimeOffDx)
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_LED_RGB);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 20;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 
                /// ATTENZIONE: Il canale BLU ha i lati DX/SX invertiti rispeto agli altri canali
                ///          -> Dalla rev 1.02.07 i parametri sono stati riordinati

                splitUshort(codificaByte(ValBlue), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(ValRed), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(ValGreen), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                splitUshort(codificaByte(TimeOn), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(TimeOff), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                splitUshort(codificaByte(ValBlueDx), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(ValRedDx), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(ValGreenDx), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                splitUshort(codificaByte(TimeOnDx), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(TimeOffDx), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;



                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;

                MessageBuffer[_arrayLen + 6] = serETX;


                return true;
            }
            catch { return _esito; }
        }

        public bool ComponiMessaggioTracciaLinea(byte Xstart, byte Ystart, byte Xend, byte Yend, byte Color)
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_DRAW_LINE);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 10;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                splitUshort(codificaByte(Xstart), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(Ystart), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(Xend), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                splitUshort(codificaByte(Yend), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(Color), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;



                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;
                MessageBuffer[_arrayLen + 6] = serETX;


                return true;
            }
            catch { return _esito; }
        }

        public bool ComponiMessaggioMostraImmagine(ushort Id, byte PosX, byte PosY, byte Color)
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_DRAW_IMAGE);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 10;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 
                // ID
                splitUshort(Id, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                _arrayInit += 4;


                splitUshort(codificaByte(PosX), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                splitUshort(codificaByte(PosY), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(Color), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;



                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;
                MessageBuffer[_arrayLen + 6] = serETX;


                return true;
            }
            catch { return _esito; }
        }

        public bool ComponiMessaggioMostraSchermata(ushort Id)
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_DRAW_SCREEN);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 2;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 
                // ID
                splitUshort(Id, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;
                /*
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                _arrayInit += 2;
                */




                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;
                MessageBuffer[_arrayLen + 6] = serETX;


                return true;
            }
            catch { return _esito; }
        }

        public bool ComponiMessaggioInviaVariabile(byte Id, string Messaggio)
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_RX_VARIABLE);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 18;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 
                // ID

                splitUshort(codificaByte(Id), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;

                // i primo 8 * 2 caratteri sono il nome
                //Dal FW 1.0.6 num bytes e 1 solo byte
                byte[] _tmpName = new byte[8];
                Messaggio += "        ";
                _tmpName = FunzioniComuni.StringToArray(Messaggio , 8);
                for (int _cicloStr = 0; _cicloStr < 8; _cicloStr++)
                {

                    splitUshort(codificaByte(_tmpName[_cicloStr]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    _arrayInit += 2;
                }

                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;
                MessageBuffer[_arrayLen + 6] = serETX;


                return true;
            }
            catch { return _esito; }
        }
 
        public new bool ComponiMessaggioLeggiMem(UInt32 memAddress, ushort numBytes)
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_READ_MEMORY);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 10;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                // Num Bytes 
                //Dal FW 1.0.6 num bytes e 1 solo byte

                splitUshort(numBytes, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;



                // Indirizzo
                splitUint32(memAddress, ref _conv32[0], ref _conv32[1], ref _conv32[2], ref _conv32[3]);

                splitUshort(codificaByte(_conv32[1]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                splitUshort(codificaByte(_conv32[2]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                splitUshort(codificaByte(_conv32[3]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 5] = msb;
                MessageBuffer[_arrayInit + 6] = lsb;

                //_arrayInit += 6;

                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;


                MessageBuffer[_arrayLen + 6] = serETX;
                _esito = true;

                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        public bool ComponiMessaggioInviaTestataImmagine(DisplaySetup.Immagine Img)
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_IMAGE_RX_START);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 24;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                // i primo 8 * 2 caratteri sono il nome
                //Dal FW 1.0.6 num bytes e 1 solo byte
                byte[] _tmpName = new byte[8];
                _tmpName = FunzioniComuni.StringToArray(Img.Nome, 8);
                for (int _cicloStr = 0; _cicloStr < 8; _cicloStr++)
                {

                    splitUshort(codificaByte(_tmpName[_cicloStr]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    _arrayInit += 2;
                }



                // Size
                splitUshort(Img.Size, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;

                // width
                splitUshort(codificaByte(Img.Width), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;


                // height
                splitUshort(codificaByte(Img.Height), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;


                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;


                MessageBuffer[_arrayLen + 6] = serETX;
                _esito = true;

                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        public ushort ComponiMessaggioPacchettoImmagine(ushort numBytes, byte[] Dati)
        {
            ushort _esito = 0;
            //ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                // Il comando base è vuoto; il comando è costitioto solo  da STX (0x02) + DATi + EPK (0xFF) + crc + ETX (0x03)

                int _arrayInit = _comandoBase.Length;
                int _arrayLen;
                //int _arrayLimit;
                // Lunghezza messaggio: lunghezza testata + ( 1 byte = num bytes da scrivere + 3 bytes = indirizzo base + num byte pacchetto ) * 2 (codifica)
                // gli ulteriori 7 bytes sono i segnali di codifica e il CRC

                if (numBytes > Dati.Length)
                {
                    numBytes = (ushort) Dati.Length;
                }

                _arrayLen = (numBytes * 2);
                _arrayInit = 0;

                MessageBuffer = new byte[_arrayLen + 7];
                //Array.Resize(ref MessageBuffer, _arrayLen + 7);                
                MessageBuffer[0] = serSTX;



                for (int _i = 0; _i < numBytes; _i++)
                {
                    splitUshort(codificaByte(Dati[_i]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    _arrayInit += 2;
                }


                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;

                /// completo il messaggio con CRC e terminatori
                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;

                MessageBuffer[_arrayLen + 6] = serETX;

                //Log.Debug("Corpo: " + hexdumpArray(MessageBuffer));
               return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("DI -> ComponiMessaggioPacchettoImmagine: " + Ex.Message);
                return _esito;
            }

        }

        public new ushort ComponiMessaggioScriviMem(UInt32 memAddress, ushort numBytes, byte[] Dati)
        {
            ushort _esito = 0;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_WRITE_MEMORY);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen;
                //int _arrayLimit;
                // Lunghezza messaggio: lunghezza testata + ( 1 byte = num bytes da scrivere + 3 bytes = indirizzo base + num byte pacchetto ) * 2 (codifica)
                // gli ulteriori 7 bytes sono i segnali di codifica e il CRC

                _arrayLen = _arrayInit + 8 + (numBytes * 2);

                MessageBuffer = new byte[_arrayLen + 7];
                //Array.Resize(ref MessageBuffer, _arrayLen + 7);                
                MessageBuffer[0] = serSTX;

                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }



                // Num Bytes 

                splitUshort(numBytes, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;



                // Indirizzo
                splitUint32(memAddress, ref _conv32[0], ref _conv32[1], ref _conv32[2], ref _conv32[3]);

                splitUshort(codificaByte(_conv32[1]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                splitUshort(codificaByte(_conv32[2]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                splitUshort(codificaByte(_conv32[3]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 5] = msb;
                MessageBuffer[_arrayInit + 6] = lsb;

                _arrayInit += 6;

                // ora aggiungo i dati:
                for (int _i = 0; _i < numBytes; _i++)
                {
                    splitUshort(_codificaByte(Dati[_i]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    _arrayInit += 2;

                }



                //if (_arrayLen>64) _arrayLen = 64;
                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);

                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;

                /// completo il messaggio con CRC e terminatori
                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;

                MessageBuffer[_arrayLen + 6] = serETX;

                Log.Debug("Corpo: " + hexdumpArray(MessageBuffer));
                //Log.Info("CRC: " + _crc.ToString("X2"));
                //Log.Info("CRC: " + msbDisp.ToString("X2") + lsbDisp.ToString("X2")  );
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("sb -> ComponiMessaggioCliente: " + Ex.Message);

                return _esito;
            }

        }

        public new ushort ComponiMessaggioCancella4KMem(UInt32 memAddress)
        {
            ushort _esito = 0;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_ERASE_4K_MEM);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 6;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                // Indirizzo
                splitUint32(memAddress, ref _conv32[0], ref _conv32[1], ref _conv32[2], ref _conv32[3]);

                splitUshort(codificaByte(_conv32[1]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                splitUshort(codificaByte(_conv32[2]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                splitUshort(codificaByte(_conv32[3]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 5] = msb;
                MessageBuffer[_arrayInit + 6] = lsb;

                _arrayInit += 6;

                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;


                MessageBuffer[_arrayLen + 6] = serETX;


                return _esito;
            }
            catch { return _esito; }
        }

        public bool ComponiMessaggioInviaTestataSchermata(DisplaySetup.Schermata Screen)
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_SCREEN_RX_START);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 18;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                // i primi 8 * 2 caratteri sono il nome -  in particolare gli ultimi 3 sono l'ID
                byte[] _tmpName = new byte[8];
                string _tmpNm = "SCREN" + Screen.Id.ToString("000");
         
                _tmpName = FunzioniComuni.StringToArray(_tmpNm, 8);
                for (int _cicloStr = 0; _cicloStr < 8; _cicloStr++)
                {
                    splitUshort(codificaByte(_tmpName[_cicloStr]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    _arrayInit += 2;
                }

                // Num Comandi
                splitUshort(codificaByte((byte)Screen.NumComandi), ref lsb, ref msb);


                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;


                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;


                MessageBuffer[_arrayLen + 6] = serETX;
                _esito = true;

                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        public ushort ComponiMessaggioPacchettoImmagineScreen(ushort numBytes,byte[] Dati)
        {
            ushort _esito = 0;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            ushort MaxDati =512;

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                // Il comando base è vuoto; il comando è costitioto solo  da STX (0x02) + DATi + EPK (0xFF) + crc + ETX (0x03)

                int _arrayInit = _comandoBase.Length;
                int _arrayLen;
                //int _arrayLimit;
                // Lunghezza messaggio: lunghezza testata + ( 1 byte = num bytes da scrivere + 3 bytes = indirizzo base + num byte pacchetto ) * 2 (codifica)
                // gli ulteriori 7 bytes sono i segnali di codifica e il CRC

                if (numBytes > ( Dati.Length))
                {
                    numBytes = (ushort)(Dati.Length );
                }

                if (numBytes > MaxDati)
                {
                    numBytes = (ushort)(MaxDati);
                }
                _arrayLen = (numBytes * 2);
                _arrayInit = 0;

                MessageBuffer = new byte[_arrayLen + 7];
                //Array.Resize(ref MessageBuffer, _arrayLen + 7);                
                MessageBuffer[0] = serSTX;



                for (int _i = 0; _i < numBytes; _i++)
                {
                    splitUshort(codificaByte(Dati[ _i]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    _arrayInit += 2;
                }


                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;

                /// completo il messaggio con CRC e terminatori
                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;

                MessageBuffer[_arrayLen + 6] = serETX;

                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("DI -> ComponiMessaggioPacchettoImmagine: " + Ex.Message);
                return _esito;
            }

        }

        public bool ComponiMessaggioScrollSchermate(byte[] ListaId, byte Attesa)
        {
            bool _esito = false;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i * 2)] = msb;
                    _comandoBase[(i * 2) + 1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo);
                splitUshort(_dispositivo, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(TipoComando.CMD_SCROLL_SCREEN);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + ( ListaId.Length + 2) * 2;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                //  Prima la sequenza

                for (int _cicloStr = 0; _cicloStr < ListaId.Length ; _cicloStr++)
                {
                    splitUshort(codificaByte(ListaId[_cicloStr]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    _arrayInit += 2;
                }

                // Poi il terminatore, 0x00
                splitUshort(codificaByte(0x00), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;

                // Tempo di attesa
                splitUshort(codificaByte(Attesa), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;

                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                CRC = _crc;


                MessageBuffer[_arrayLen + 1] = serENDPAC;

                splitUshort(_crc, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 2] = msb;
                MessageBuffer[_arrayLen + 3] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayLen + 4] = msb;
                MessageBuffer[_arrayLen + 5] = lsb;


                MessageBuffer[_arrayLen + 6] = serETX;
                _esito = true;

                return _esito;
            }
            catch
            {
                return _esito;
            }
        }


    }
}
