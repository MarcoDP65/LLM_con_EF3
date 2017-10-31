using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

using Utility;


namespace ChargerLogic
{
    public partial class MessaggioLadeLight : SerialMessage
    {

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        private string _lastError;

        private byte[] _idCorrente = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private byte[] _tempId = { 0, 0, 0, 0, 0, 0, 0, 0 };

        //public comandoInizialeSB Intestazione;
        public comandoRTC DatiRTCSB;
        //public DatiCliente CustomerData;
        //public ProgrammaRicarica ProgRicarica;
        //public MemoriaPeriodoLungo CicloLungo;
        //public MemoriaPeriodoBreve _CicloBreve;
        //public PacchettoReadMem _pacchettoMem;
        //public VariabiliSpybatt variabiliScheda;
        //public CalibrazioniSpybatt valoriCalibrazione;
        //public ImmagineDumpMem DumpMem;
        public StatoFirmware StatoFirmwareScheda;
        //public ComandoStrategia ComandoStrat;
        //public ParametriSpybatt ParametriGenerali;
        public OcBaudRate BrOCcorrente = OcBaudRate.OFF;
        public OcEchoMode EchoOCcorrente = OcEchoMode.OFF;

        public byte[] LLstopMessage;

       // public StatoSig60 StatoTrxOC;
        public byte ReserCounterOC;


        public byte _comandoInvio;
        public byte _pacchettoInviato;
        public byte _esitoPacchetto;
     //   public EsitoMessaggio EsitoComando;

        public int fwLevel = 0;

        private EsitoRisposta _ultimaRisposta = EsitoRisposta.MessaggioVuoto;

        public MessaggioLadeLight()
        {
            fwLevel = 0;
        }

        public ushort ComponiMessaggioSwitchBL()
        {
            ushort _esito = 0;
            //ushort _tempUshort;
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

                _comando = (byte)(TipoComando.CMD_RESET_BOOT);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /*******************************************************************************/
                /* Parte dati                                                                  */
                /*******************************************************************************/

                // nessun dato da inviare

                /*******************************************************************************/
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
                Log.Error("sb -> ComponiMessaggioSwitchBL: " + Ex.Message);
                _lastError = Ex.Message;

                return _esito;
            }

        }

        public new EsitoRisposta analizzaMessaggio(byte[] _messaggio, int fwLevel = 1, bool skipHead = false, bool CreateMsgObject = false)
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
                skipHead = false;
                if (_messaggio.Length < 3)
                { return EsitoRisposta.MessaggioVuoto; }

                // scompongo l'intestazione
                // STX
                _ret = _messaggio[0];
                if (_ret != serSTX) return EsitoRisposta.NonRiconosciuto;

                if (skipHead == false)
                {

                    if (_messaggio.Length < 20)
                    { return EsitoRisposta.LunghezzaErrata; }

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
                    if (_messaggio.Length < 14)
                    { return EsitoRisposta.LunghezzaErrata; }
                    SerialNumber = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    Dispositivo = 0;
                    _startPos = 1;
                    preambleLenght = 2;
                }

                // comando
                byte _tmpComando = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                if (_tmpComando != 0xFF)
                    _comando = _tmpComando;

                //preparo la risposta: il preambolo è uguale al messaggio 
                Array.Copy(_messaggio, messaggioRisposta, 20);

                // ora in base al comando cambio faccio lettura:
                switch (_comando)
                {
                    case (byte)TipoComando.ACK:
                        // case (byte)TipoComando.SB_ACK:

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
                    case (byte)TipoComando.NACK_PACKET: //NAK
                        {
                            _crc = 0;
                            Log.Warn("--- NACK --- NACK --- NACK --- NACK --- NACK --- NACK --- NACK --- NACK --- NACK --- NACK --- NACK ---");
                            return EsitoRisposta.ErroreGenerico;
                        }

                    case (byte)TipoComando.CMD_UART_HOST_CONNECTED:
                        {
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
                        }

                    case 0x99: //ciclo attuale
                        {
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
                        }
                        break;

                    case (byte)TipoComando.CMD_READ_ID_CYCLE_CRG: //id cicli - 0x03
                        {
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
                            CicliPresenti = new cicliPresenti();
                            _buffArray = new byte[(_endPos - 29)];
                            Array.Copy(_messaggio, 23, _buffArray, 0, _endPos - 29);
                            _risposta = CicliPresenti.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }
                        }
                        break;

                    case (byte)TipoComando.ReadRTC: // read RTC - 0xD3:
                        {
                            _startPos = 39;

                            if (_messaggio[_startPos] != serENDPAC)
                            {
                                return EsitoRisposta.NonRiconosciuto;
                            }
                            _buffArray = new byte[38];
                            Array.Copy(_messaggio, 1, _buffArray, 0, 38);
                            _startPos = 40;
                            _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                            _tempShort = (ushort)(_ret);
                            _startPos = 42;
                            _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                            _tempShort = (ushort)((_tempShort << 8) + _ret);
                            _crc = codCrc.ComputeChecksum(_buffArray);

                            if (_crc != _tempShort)
                            { return EsitoRisposta.BadCRC; }

                            // ora leggo la parte dati
                            DatiRTC = new comandoRTC();
                            _buffArray = new byte[16];
                            Array.Copy(_messaggio, 23, _buffArray, 0, 16);
                            _risposta = DatiRTC.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }
                        }
                        break;

                    case (byte)TipoComando.CMD_READ_CYCLE_PROG:
                        {
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
                            CicloInMacchina = new cicloAttuale();
                            _buffArray = new byte[(_endPos - 29)];
                            Array.Copy(_messaggio, 23, _buffArray, 0, _endPos - 29);
                            _risposta = CicloInMacchina.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.SB_R_Variabili:
                        {
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
                        }

                    case (byte)TipoComando.LL_SIG60_PROXY:
                        {
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
                            DatiStrategia = new ProxyComandoStrategia();
                            _buffArray = new byte[(_endPos - 29)];
                            Array.Copy(_messaggio, 23, _buffArray, 0, _endPos - 29);
                            _risposta = DatiStrategia.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }
                            break;
                        }

                    case (byte)TipoComando.CMD_INFO_BL:
                        {
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
                            _buffArray = new byte[(_endPos - (preambleLenght + 7))];
                            Array.Copy(_messaggio, preambleLenght + 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            _risposta = StatoFirmwareScheda.analizzaMessaggio(_buffArray, fwLevel);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }



                    default:
                        return EsitoRisposta.NonRiconosciuto;

                }



                return EsitoRisposta.MessaggioOk;

            }
            catch
            {
                return EsitoRisposta.ErroreGenerico;
            }

        }




    }
}
