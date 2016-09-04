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
        /*
        public enum ComandoDisplay : byte
        {
            ACK = 0x44,
            NACK = 0x45,


            DI_Start = 0x0F,
            DI_Strobe = 0xFF,
            DI_Stop = 0xF0,

            DI_LedRGB = 0x1E,
            DI_Stato = 0xE1,
            DI_CancellaInteraMemoria = 0x3A,
            DI_Cancella4K = 0x24,
            DI_R_LeggiMemoria = 0x3C,
            DI_W_ScriviMemoria = 0xC3,
            DI_R_InvioImmagineMemoria = 0x4B,

            ReadRTC = 0xD3,
            UpdateRTC = 0xD2,
            FirmwareUpdate = 0xD5,
            PrimaConnessione = 0x01,
            CicloProgrammato = 0x02,
            CicliCarica = 0x03,
            DettagliCiclo = 0x04,
            ProgrammazioneCiclo = 0x05,

            SB_Sstart = 0x17,
            SB_Strobe = 0x1B,
            SB_Stop = 0x1D,
            SB_DatiIniziali = 0x1F,
            SB_R_DatiCliente = 0x20,
            SB_R_Programmazione = 0x21,
            SB_W_DatiCliente = 0x22,
            SB_W_Programmazione = 0x23,
            SB_R_CicloLungo = 0x25,
            SB_R_CicloBreve = 0x26,
            SB_R_Variabili = 0x27,

            SB_R_LeggiMemoria = 0x33,
            SB_W_ScriviMemoria = 0x35,
            SB_R_DumpMemoria = 0x39,
            SB_UpdateRTC = 0x47,
            SB_ReadRTC = 0x48,
            SB_Cal_Enable = 0x3B,
            SB_Cal_InvioDato = 0x3E,
            SB_Cal_LetturaGain = 0x3F,
            SB_R_BootloaderInfo = 0x51,
            SB_W_FirmwareUpdate = 0x53,
            SB_W_FirmwareData = 0x57,
            SB_W_FirmwareSelect = 0x58,
            SB_W_BLON = 0x5B,
            SB_R_APPCHECK = 0x5D,
            SB_W_RESETSCHEDA = 0X5F,
            BREAK = 0x1C,
            SB_ACK = 0x6C,
            SB_ACK_PKG = 0x6D,
            SB_NACK = 0x71,
            SB_W_MemProgrammed = 0x74,
            SB_W_chgst_Call = 0x80,
            LL_SIG60_PROXY = 0x81,
        };
        */
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

        public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
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

            try
            {
                messaggioRisposta = new byte[29];

                if (_messaggio.Length < 3)
                { return EsitoRisposta.MessaggioVuoto; }

                // scompongo l'intestazione
                // STX
                _ret = _messaggio[0];
                if (_ret != serSTX) return EsitoRisposta.NonRiconosciuto;

                //seriale
                _startPos = 1;
                SerialNumber = new byte[8];
                for (int ciclo = 0; ciclo < 8; ciclo++)
                {
                    SerialNumber[ciclo] = decodificaByte(_messaggio[_startPos + (2 * ciclo)], _messaggio[_startPos + (2 * ciclo) + 1]);
                }

                //tipo
                _startPos = 17;
                _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                _dispositivo = (ushort)(_ret);
                _startPos = 19;
                _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                _dispositivo = (ushort)((_dispositivo << 8) + _ret);
                Dispositivo = 0; // Enum.ToObject(typeof(TipoDispositivo), _dispositivo);

                // comando
                _startPos = 21;
                _comando = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);

                Log.Debug("Messaggio Display ricevuto: " + _comando.ToString("X2"));

                //preparo la risposta: il preambolo è uguale al messaggio 
                Array.Copy(_messaggio, messaggioRisposta, 20);

                // ora in base al comando cambio faccio lettura:
                switch (_comando)
                {
                    case (byte)TipoComando.ACK:
                        _startPos = 23;
                        if (_messaggio[_startPos] != serENDPAC)
                        {
                            return EsitoRisposta.NonRiconosciuto;
                        }
                        _buffArray = new byte[22];
                        Array.Copy(_messaggio, 1, _buffArray, 0, 22);
                        _startPos = 24;
                        _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                        _tempShort = (ushort)(_ret);
                        _startPos = 26;
                        _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                        _tempShort = (ushort)((_tempShort << 8) + _ret);
                        _crc = codCrc.ComputeChecksum(_buffArray);

                        if (_crc != _tempShort)
                        { return EsitoRisposta.BadCRC; }

                        break;
                    case (byte)TipoComando.NACK:
                        _crc = 0;
                        break;

                    case (byte)TipoComando.Start:
                        break;

                    /*
                                        case (byte)TipoComando.PrimaConnessione:
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
                                            { return EsitoRisposta.BadCRC; }

                                            // ora leggo la parte dati
                                            Intestazione = new comandoIniziale();
                                            _buffArray = new byte[(_endPos - 29)];
                                            Array.Copy(_messaggio, 23, _buffArray, 0, _endPos - 29);
                                            _risposta = Intestazione.analizzaMessaggio(_buffArray);
                                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                                            break;

                                        case (byte)TipoComando.SB_R_Variabili:
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
                                            { return EsitoRisposta.BadCRC; }

                                            // ora leggo la parte dati
                                            VariabiliAttuali = new VariabiliLadeLight();
                                            _buffArray = new byte[(_endPos - 29)];
                                            Array.Copy(_messaggio, 23, _buffArray, 0, _endPos - 29);
                                            _risposta = VariabiliAttuali.analizzaMessaggio(_buffArray);
                                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }
                                            break;
                    */


                    default:
                        return EsitoRisposta.NonRiconosciuto;
                        break;
                }



                return EsitoRisposta.MessaggioOk;

            }
            catch
            {
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

                _comando = (byte)(TipoComando.DI_Backlight);
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
                    _statoLuci = 0x0F;
                else
                    _statoLuci = 0xF0;

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

        public bool ComponiMessaggioLed(byte ValRed, byte ValGreen, byte ValBlue, byte TimeOn, byte TimeOff)
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

                _comando = (byte)(TipoComando.DI_LedRGB);
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

                splitUshort(codificaByte(ValRed), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(ValGreen), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;
                splitUshort(codificaByte(ValBlue), ref lsb, ref msb);
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

                _comando = (byte)(TipoComando.DI_DrawLine);
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




    }
}
