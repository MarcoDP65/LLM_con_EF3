using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace ChargerLogic
{
    public class SerialMessage
    {
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public enum InitialCrcValue : ushort { Zeros, NonZero1 = 0xffff, NonZero2 = 0x1D0F };
        public enum RequiredActionOutcome : byte {Success = 0x0F,Failed = 0xF0, UnDone = 0xFF,Undefined = 0x00};
        public enum TipoDispositivo : ushort { PcOrSmart = 0xBCBC, Charger = 0x0000, SpyBat = 0x0003 };
        public enum TipoComando : byte { 
            Start = 0x0F,  
            Strobe = 0xFF, 
            Stop = 0xF0,

            ReadRTC = 0xD3, 
            UpdateRTC = 0xD2, 
            FirmwareUpdate = 0xD5,

            CMD_UART_HOST_CONNECTED   = 0x01,
            CMD_READ_CYCLE_PROG       = 0x02,
            CMD_READ_ID_CYCLE_CRG     = 0x03,
            CMD_READ_CYCLE_CRG        = 0x04,
            CMD_PRG_CYCLE_CRG         = 0x05,
            ACK_PACKET                = 0x6C,
            EVENT_MEM_CODE            = 0x6D,
            NACK_PACKET               = 0x71,



            ACK = 0x44,
            NACK = 0x45,
            CMD_CONNECT               = 0x17,
            CMD_DISCONNECT            = 0x1D,
            SB_DatiIniziali = 0x1F,
            SB_R_DatiCliente = 0x20,
            SB_R_Programmazione = 0x21,
            SB_W_DatiCliente = 0x22,
            SB_W_Programmazione = 0x23,
            SB_R_CicloLungo = 0x25,
            SB_R_CicloBreve = 0x26,
            SB_R_Variabili = 0x27,
            SB_CancellaInteraMemoria = 0x28,
            CMD_ERASE_4K_MEM = 0x2E,
            CMD_READ_MEMORY = 0x33,
            CMD_WRITE_MEMORY = 0x35,
            CMD_READ_ALL_MEMORY = 0x39,
            SB_UpdateRTC = 0x47,
            SB_ReadRTC = 0x48,
            SB_Cal_Enable = 0x3B,
            SB_Cal_InvioDato = 0x3E,
            SB_Cal_LetturaGain = 0x3F,
            CMD_INFO_BL                = 0x51,  // stesso per ll
            CMD_FW_UPLOAD_MSP          = 0x53,  // stesso per ll
            CMD_FW_UPLOAD_TMS          = 0x54,  // stesso per ll

            CMD_FW_DATA_SEND           = 0x57,   // stesso per ll
            CMD_FW_UPDATE              = 0x58,  // stesso per ll
            CMD_RESET_BOOT             = 0x5B,
            CMD_CTRL_APP               = 0x5D,
            CMD_RESET_BOARD            = 0X5F,
            BREAK = 0x1C,

            SB_W_MemProgrammed = 0x74,
            SB_W_chgst_Call = 0x80,

            SB_R_ParametriLettura = 0x4F,
            SB_W_ParametriLettura = 0x4E,

            SB_R_ParametriSIG60 = 0x7E,
            SB_W_ParametriSIG60 = 0x7D,

            DI_LedRGB = 0x1E,
            DI_Stato = 0xE1,
            DI_ResetBoard = 0x5F,
            DI_CancellaInteraMemoria = 0x3A,
            DI_Cancella4K = 0x24,
            DI_R_LeggiMemoria = 0x3C,
            DI_W_ScriviMemoria = 0xC3,
            DI_W_ScriviVariabile = 0xC4,
            DI_W_SetRTC = 0xD2,
            DI_W_SalvaImmagineMemoria = 0x4B,
            DI_W_SalvaSchermataMemoria = 0xB4,

            DI_CancellaDisplay = 0x2D,
            DI_Backlight = 0xB3,
            DI_DrawLine = 0xA5,
            DI_MostraImmagine = 0x5A,
            DI_MostraSchermata = 0x96,
            DI_ScrollSchermate = 0x94,
            DI_SwitchBaudRate = 0x74,


            LL_SIG60_PROXY = 0x81,
            LL_CancellaInteraMemoria = 0x29,
            //LL_Cancella4K = 0x2F,
            LL_R_LeggiMemoria = 0x34,
            LL_W_ScriviMemoria = 0x36,
            LL_R_DumpMemoria = 0x37,
            LL_W_FineCarica = 0x63,

        };

        public enum ParametroLadeLight : byte
        {
            TempoMassimoCarica = 0x01,
            TempoT2Min = 0x02,
            TempoT2Max = 0x03,
            TensioneNominale = 0x10,
            TensioneSogliaCella= 0x11,   // 2 decimali
            CorrenteCarica = 0x21,
            CapacitaNominale = 0x31,
            CapacitaDaRicaricare = 0x32,
            FrequenzaSwitching = 0x40,
            DivisoreK = 0x50,  // da trasmettere SEMPRE prima dei parametri K
            ParametroKP = 0x51,
            ParametroKI = 0x52,
            ParametroKD = 0x53,
            CondizioneStop = 0xF0,
            CoeffK = 0xF1,   // 1 decimale
        }

        /// <summary>
        /// Enum OCBaudRate: Setta la velocià di comunicazione sul canale OC
        /// </summary>
        public enum OcBaudRate : byte { OFF = 0x00, br_9k6 = 0x21, br_19k2 = 0x31, br_38k4 = 0x01, br_57k6 = 0x11 };
        public enum OcEchoMode : byte { OFF = 0x00, Listening = 0xAC, Echo = 0xEC };


        // Da rendere dinamico
        public enum TipoCicloLadeLight : byte
        {
            ND = 0x00,
            IWa = 0x01,
            IU = 0x02,
            IUIa = 0x03,
            Nullo = 0xFF,
        }
        public enum CondStopLadeLight : byte
        {
            ND = 0x00,
            Timer = 0x01,
            dVdt = 0x02,
        }
        public enum TipoRisposta : byte { NonValido = 0x00, Ack = 0x01, Nack = 0x02, Break = 0x03, Data = 0xFF };
        public enum EsitoRisposta : byte { 
            MessaggioVuoto = 1, 
            BadCRC = 2, 
            NonRiconosciuto = 3,
            ParametriErrati= 10,
            IdNonCoerente = 11,
            RispostaNonValida = 0x20,
            LunghezzaErrata = 0x21,
            CodiceRispostaErrato = 0x22,
            ErroreGenerico = 99,
            MessaggioOk = 0 }
        public enum TipoCiclo : byte { Carica = 0xF0, Scarica = 0x0F, Pausa = 0xAA , Equal = 0xF1 };
        public enum CodiceStop : byte { BatteriaTamponeBassa = 0x0B,
                                        BatteriaTamponeAlta = 0xB0,
                                        TensioneBatteriaBassa = 0xBD,
                                        VersoCorrenteInvertito = 0xCC,
                                        NuovaProgrammazione = 0x02 };
        public enum LadeLightBool : byte
        {
            True = 0x0F,
            False = 0xF0
        };

        public byte[] messaggioRisposta;
        public const byte serSTX = 0x02;
        public const byte serETX = 0x03;
        public const byte serENDPAC = 0xFF;

        public byte[] MessageBuffer;

        public ushort CRC;

        protected ushort _crc;
        protected byte[] _messaggio;
        protected byte[] _comandoBase;
        public TipoComando Comando;
        public byte[] SerialNumber;
        public ushort _dispositivo;
        public byte _comando;
 
        public TipoDispositivo Dispositivo;
        public comandoRTC DatiRTC;
        public comandoIniziale Intestazione;
        public cicliPresenti CicliPresenti;
        public cicloAttuale CicloInMacchina;
        public VariabiliLadeLight VariabiliAttuali;
        public ProxyComandoStrategia DatiStrategia;
        public PacchettoReadMem _pacchettoMem;
        private DateTime _startRead;

        public bool componiRisposta(byte[] _messaggio, EsitoRisposta Esito)
        {
            byte[] _tempMessaggio = new byte[22];
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
                //preparo la risposta: il preambolo è uguale al messaggio 

                if (_messaggio.Length < 21)
                {  //preparo un messaggio vuoto
                    messaggioRisposta[0] = serSTX;
                    for (int n = 1; n < 21; n++)
                    {
                        messaggioRisposta[n] = 0x30;
                    }
                }
                else
                {
                    Array.Copy(_messaggio, messaggioRisposta, 21);
                }
                // codice esito, 0x44 OK, 0x45 Ko, 0x1C Break
//                if (Esito == EsitoRisposta.MessaggioOk)
//                {
                //splitUshort(codificaByte( (byte)Esito), ref messaggioRisposta[21], ref messaggioRisposta[22]);
                splitUshort(codificaByte((byte)_dispositivo), ref messaggioRisposta[20], ref messaggioRisposta[19]);
                splitUshort(codificaByte(_comando), ref messaggioRisposta[22], ref messaggioRisposta[21]);
                //                }
//                else
//                {
//                    splitUshort(codificaByte(0x45), ref messaggioRisposta[21], ref messaggioRisposta[22]);
//                }

                messaggioRisposta[23] = serENDPAC;

                /// calcolo il crc
                /// 
                Array.Copy(messaggioRisposta, 1, _tempMessaggio, 0, 22);
                _crc = codCrc.ComputeChecksum(_tempMessaggio);

                splitUshort(_crc, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                messaggioRisposta[24] = msb;
                messaggioRisposta[25] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                messaggioRisposta[26] = msb;
                messaggioRisposta[27] = lsb;
                messaggioRisposta[28] = serETX;

                return true;
            }
            catch
            {
                return false;
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
                _ret = decodificaByte(_messaggio[_startPos ], _messaggio[_startPos + 1]);
                _dispositivo = (ushort)(_ret);
                _startPos = 19;
                _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                _dispositivo = (ushort)((_dispositivo << 8) + _ret);
                Dispositivo = 0; // Enum.ToObject(typeof(TipoDispositivo), _dispositivo);

                // comando
                _startPos = 21;
                _comando = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);

                Log.Debug("Messaggio LL ricevuto: " + _comando.ToString("X2"));

                //preparo la risposta: il preambolo è uguale al messaggio 
                Array.Copy(_messaggio, messaggioRisposta, 20);

                // ora in base al comando cambio faccio lettura:
                switch (_comando)
                {
                    case (byte)TipoComando.ACK_PACKET:
                   // case (byte)TipoComando.SB_ACK:

                        _startPos = 23;
                        if (_messaggio[_startPos] != serENDPAC)
                        {
                            return EsitoRisposta.NonRiconosciuto;
                        }
                        _buffArray = new byte[22];
                        Array.Copy(_messaggio, 1, _buffArray, 0, 22);
                        _startPos = 24;
                        _ret = decodificaByte(_messaggio[_startPos ], _messaggio[_startPos + 1]);
                        _tempShort = (ushort)(_ret);
                        _startPos = 26;
                        _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                        _tempShort = (ushort)((_tempShort << 8) + _ret);
                        _crc = codCrc.ComputeChecksum(_buffArray);

                        if (_crc != _tempShort)
                        { return EsitoRisposta.BadCRC; }

                        break;
                    case (byte)TipoComando.NACK:
                  //  case (byte)TipoComando.SB_NACK:
                        _crc = 0;
                        break;

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
                        }
                        break;

                    case 0x99: //ciclo attuale
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

                    case (byte)TipoComando.CMD_READ_ID_CYCLE_CRG: //id cicli - 0x03
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

                        break;

                    case 0xD3: // read RTC
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
                        Array.Copy(_messaggio, 23, _buffArray, 0, 16 );
                        _risposta = DatiRTC.analizzaMessaggio(_buffArray);
                        if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                        break;

                    case (byte)TipoComando.CMD_READ_CYCLE_PROG:
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

                    case (byte)TipoComando.LL_SIG60_PROXY:
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

        public SerialMessage()
        {
            _messaggio = new byte[1] { 0x00 };
            _comandoBase = new byte[22];
            MessageBuffer = new byte[1];
            messaggioRisposta = new byte[29];
            for (int i = 0; i < 22; i++)
            {
                _comandoBase[i] = (byte)(0x00);
            }
        }

        new public cicloAttuale CicloAttivo;

        public bool raggiuntoTimeout(DateTime inizio, int SecondiTimeOut)
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

        public ushort ComponiMessaggio()
        {
            byte[] _vuoto = new byte[0];
            return ComponiMessaggio(_vuoto);
        }

        public ushort ComponiMessaggio(byte[] _corpoMessaggio)
        {
            ushort _esito = 0;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;

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

                _comando = (byte)(Comando);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;


                /// aggiungo il corpo
                /// 


                /// calcolo il crc
                /// 

                _crc = codCrc.ComputeChecksum(_comandoBase);
                CRC = _crc;

                int _arrayLen = _comandoBase.Length;
                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayLen; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }
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

        public ushort ComponiMessaggioNew(byte[] _corpoMessaggio)
        {
            ushort _esito = 0;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try 
            {
                //serial
                for (int i = 0; i <= 7; i++)
                {
                    splitUshort(codificaByte(SerialNumber[i]), ref lsb, ref msb);
                    _comandoBase[(i*2)] = msb;
                    _comandoBase[(i*2)+1] = lsb;
                }
                //dispositivo

                _dispositivo = (ushort)(Dispositivo );
                splitUshort(_dispositivo,ref lsbDisp,ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                _comandoBase[(16)] = msb;
                _comandoBase[(17)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                _comandoBase[(18)] = msb;
                _comandoBase[(19)] = lsb;

                _comando = (byte)(Comando);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;


                int _lenDati = _corpoMessaggio.Length;
                _lenDati = _lenDati * 2;
                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + _lenDati;
                Array.Resize(ref MessageBuffer, _arrayLen + 7);
                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 
                _arrayInit += 1;

                for (int _b = 0; _b < _corpoMessaggio.Length; _b++)
                {
                    splitUshort(codificaByte(_corpoMessaggio[_b]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit] = msb;
                    MessageBuffer[_arrayInit+1] = lsb;
                    _arrayInit += 2;
                }


                /// calcolo il crc
                /// 

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

        public ushort ComponiMessaggioOra()
        {
            ushort _esito = 0;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;

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

                _comando = (byte)(Comando);
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

                // Anno ushort
                splitUshort(DatiRTC.anno, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;

                // Mese byte
                splitUshort(codificaByte(DatiRTC.mese), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 5] = msb;
                MessageBuffer[_arrayInit + 6] = lsb;

                // Giorno byte
                splitUshort(codificaByte(DatiRTC.giorno), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 7] = msb;
                MessageBuffer[_arrayInit + 8] = lsb;

                // giorno sett byte
                splitUshort(codificaByte(DatiRTC.giornoSett), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 9] = msb;
                MessageBuffer[_arrayInit + 10] = lsb;

                // Ore byte
                splitUshort(codificaByte(DatiRTC.ore), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 11] = msb;
                MessageBuffer[_arrayInit + 12] = lsb;

                // Minuti byte
                splitUshort(codificaByte(DatiRTC.minuti), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 13] = msb;
                MessageBuffer[_arrayInit + 14] = lsb;

                // secondi byte
                splitUshort(codificaByte(DatiRTC.secondi), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 15] = msb;
                MessageBuffer[_arrayInit + 16] = lsb;

                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen ];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen );
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

        public ushort ComponiMessaggioCiclo( UInt32 IdCiclo)
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

                _comando = (byte)(TipoComando.CMD_READ_CYCLE_CRG);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 4;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                // Id ushort
                splitUint32(IdCiclo, ref _conv32[0], ref _conv32[1], ref _conv32[2], ref _conv32[3]);

                splitUshort(codificaByte(_conv32[0]), ref lsb, ref msb);
                MessageBuffer[_arrayInit] = msb;
                MessageBuffer[_arrayInit + 1] = lsb;
                splitUshort(codificaByte(_conv32[1]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 2] = msb;
                MessageBuffer[_arrayInit + 3] = lsb;
                splitUshort(codificaByte(_conv32[2]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 4] = msb;
                MessageBuffer[_arrayInit + 5] = lsb;
                splitUshort(codificaByte(_conv32[3]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 6] = msb;
                MessageBuffer[_arrayInit + 7] = lsb;

                /// calcolo il crc
                byte[] _tempMessaggio = new byte[_arrayLen - 1];
                Array.Copy(MessageBuffer, 1, _tempMessaggio, 0, _arrayLen - 1);
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

        public ushort ComponiMessaggioCicloProgrammato()
        {
            ushort _esito = 0;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            int _lenDati = 0;
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

                _comando = (byte)(TipoComando.CMD_PRG_CYCLE_CRG);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                //Calcolo La lunghezza della parte dati
                _lenDati = 3 + CicloInMacchina.LunghezzaNome + 3 * CicloInMacchina.NumeroParametri;
                // Calcolo o spazio
                _lenDati = _lenDati * 2;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + _lenDati;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                // Lunghezza nome
                splitUshort(codificaByte(CicloInMacchina.LunghezzaNome), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;

                // Tipo Ciclo
                splitUshort(codificaByte(CicloInMacchina.TipoCiclo), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;

                // Num Parametri
                splitUshort(codificaByte(CicloInMacchina.NumeroParametri), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;

                // Testo Nome
                for (int _i = 0; _i < CicloInMacchina.LunghezzaNome; _i++)
                {

                    splitUshort(_codificaSubString(CicloInMacchina.NomeCiclo, _i), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    _arrayInit += 2;

                }



                foreach (ParametroLL _par in CicloInMacchina.Parametri)
                {
                    // Id Parametro
                    splitUshort(codificaByte(_par.idParametro), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    _arrayInit += 2;
                    // Valore
                    _conv32[0] = 0;
                    _conv32[1] = 0;

                    splitUshort(_par.ValoreParametro, ref _conv32[0], ref _conv32[1]);

                    splitUshort(codificaByte(_conv32[1]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    splitUshort(codificaByte(_conv32[0]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 3] = msb;
                    MessageBuffer[_arrayInit + 4] = lsb;
                    _arrayInit += 4;

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

                return _esito;
            }
            catch { return _esito; }
        }

        public ushort ComponiMessaggioLeggiMem(UInt32 memAddress, ushort numBytes)
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

                _comando = (byte)(TipoComando.LL_R_LeggiMemoria);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 8;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

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


                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("LL -> ComponiMessaggioScriviMem: " + Ex.Message);
                return _esito;
            }
        }

        public ushort ComponiMessaggioScriviMem(UInt32 memAddress, ushort numBytes, byte[] Dati)
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

                _comando = (byte)(TipoComando.LL_W_ScriviMemoria);
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
                Log.Error("LL -> ComponiMessaggioScriviMem: " + Ex.Message);
                return _esito;
            }

        }

        public ushort ComponiMessaggioCancella4KMem(UInt32 memAddress)
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
            catch (Exception Ex)
            {
                Log.Error("LL -> ComponiMessaggioCancella4KMem: " + Ex.Message);
                return _esito;
            }
        }

        public ushort ComponiMessaggioTestataFW(byte Blocco, byte[] Intestazione)
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
                // Prima controllo che siano effettivamente 66 byte
                if (Intestazione.Length != 64)
                {
                    _esito = 99;
                    return _esito;
                }

                // Unici banchi acettabili 1 e 2 
                if ((Blocco < 1) || (Blocco > 2))
                {
                    _esito = 99;
                    return _esito;
                }

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

                _comando = (byte)(TipoComando.CMD_FW_UPLOAD_TMS);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 2 + 128 ;  // 2 di area + 128 di array messaggio
          
                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }





                /*******************************************************************************/
                /* Parte dati                                                                  */
                /*******************************************************************************/

                // id Blocco

                splitUshort(codificaByte(Blocco), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;


                // Array Dati
                for (int _i = 0; _i < 64; _i++)
                {
                    splitUshort(_codificaByte(Intestazione[_i]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    _arrayInit += 2;

                }



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

                Log.Debug("Corpo: " + hexdumpArray(MessageBuffer));
                //Log.Info("CRC: " + _crc.ToString("X2"));
                //Log.Info("CRC: " + msbDisp.ToString("X2") + lsbDisp.ToString("X2")  );
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("ll -> ComponiMessaggioTestataFW: " + Ex.Message);
                return _esito;
            }

        }

        /// <summary>
        /// Preparo il messaggio di invio pacchetto dati per l'aggiornamento firmware
        /// </summary>
        /// <param name="NumPacchetto">Progressivo pacchetto (ushort)</param>
        /// <param name="NumBytes">Dimensione del pachetto (byte, deve essere inferioreo uguale a 130)</param>
        /// <param name="Dati">byte array del pacchetto con CRC finale</param>
        /// <returns></returns>
        public ushort ComponiMessaggioPacchettoDatiFW(ushort NumPacchetto, byte NumBytes, byte[] Dati, ushort CRCPacchetto)
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
                // Prima controllo che non siano più di 130 byte
                if (Dati.Length > 130)
                {
                    _esito = 99;
                    return _esito;
                }


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

                _comando = (byte)(TipoComando.CMD_FW_DATA_SEND);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + (NumBytes * 2 + 6);

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }





                /*******************************************************************************/
                /* Parte dati                                                                  */
                /*******************************************************************************/
                // Num Pacchetto

                splitUshort(NumPacchetto, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Dim Pacchetto

                splitUshort(codificaByte(NumBytes), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Array Dati

                for (int _i = 0; _i < (NumBytes - 2); _i++)
                {
                    splitUshort(_codificaByte(Dati[_i]), ref lsb, ref msb);
                    MessageBuffer[_arrayInit + 1] = msb;
                    MessageBuffer[_arrayInit + 2] = lsb;
                    _arrayInit += 2;
                }

                // Aggiungo il CRC pacchetto
                splitUshort(CRCPacchetto, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;


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

                Log.Debug("Corpo: " + hexdumpArray(MessageBuffer));
                //Log.Info("CRC: " + _crc.ToString("X2"));
                //Log.Info("CRC: " + msbDisp.ToString("X2") + lsbDisp.ToString("X2")  );
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LL -> ComponiMessaggioDatiFW: " + Ex.Message);
             
                return _esito;
            }

        }


        public static byte decodificaByte(byte msbMsg, byte lsbMsg)
        {

            byte _result = 0;


            try
            {
                if (msbMsg > 0x39)
                { _result = (byte)(msbMsg - 0x37); }
                else
                { _result = (byte)(msbMsg - 0x30); }

                _result = (byte)(_result << 4);

                if (lsbMsg > 0x39)
                { _result += (byte)((lsbMsg - 0x37) & 0x0F); }
                else
                { _result += (byte)((lsbMsg - 0x30) & 0x0F); }
                return _result;

            }
            catch
            {
                return 0;
            }

        }

        public static bool decodificaArray(byte[] _origine, ref byte[] _risultato)
        {
            try
            {
                int _lunghezza = _origine.Length;

                if (_risultato.Length < (_lunghezza / 2))
                { 
                    //array troppo corto
                    return false;
                }

                for (int _ciclo = 0; _ciclo < _lunghezza; _ciclo += 2)
                {
                    _risultato[_ciclo / 2] = decodificaByte(_origine[_ciclo], _origine[_ciclo + 1]);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public  ushort codificaByte(byte _valore)
        { 
         ushort _tempVal = 0x0000;
         byte _tempbyte;

         try
         {

             // high nibble
             _tempbyte = (byte)(_valore & 0xF0);
             _tempbyte = (byte)(_tempbyte >> 4);

             if (_tempbyte <= 9)
             {
                 _tempbyte += (byte)(0x30);
             }
             else
             {
                 _tempbyte += (byte)(0x37);
             }
             _tempVal = (ushort)(_tempbyte << 8 );

             // low nibble
             _tempbyte =  (byte)(_valore & 0x0F);
             if (_tempbyte <= 9)
             {
                 _tempbyte += (byte)(0x30);
             }
             else
             {
                 _tempbyte += (byte)(0x37);
             }
             _tempVal = (ushort)(_tempVal | _tempbyte);

             return _tempVal;
         }
         catch 
         {
             return _tempVal;
         }
        }

        protected static ushort _codificaByte(byte _valore)
        {
            ushort _tempVal = 0x0000;
            byte _tempbyte;

            try
            {

                // high nibble
                _tempbyte = (byte)(_valore & 0xF0);
                _tempbyte = (byte)(_tempbyte >> 4);

                if (_tempbyte <= 9)
                {
                    _tempbyte += (byte)(0x30);
                }
                else
                {
                    _tempbyte += (byte)(0x37);
                }
                _tempVal = (ushort)(_tempbyte << 8);

                // low nibble
                _tempbyte = (byte)(_valore & 0x0F);
                if (_tempbyte <= 9)
                {
                    _tempbyte += (byte)(0x30);
                }
                else
                {
                    _tempbyte += (byte)(0x37);
                }
                _tempVal = (ushort)(_tempVal | _tempbyte);

                return _tempVal;
            }
            catch
            {
                return _tempVal;
            }
        }

        /// <summary>
        /// Codifica il carattere in posizione 'Posizione' della stringa; se oltre la lunghezza effettiva ritorna - codificato - il valore 'valoreNullo'
        /// </summary>
        /// <param name="Testo">Stringa da cui estrarre il carattere</param>
        /// <param name="Posizione">posizione a base 0 del carattere da estrarre</param>
        /// <param name="valoreNullo">valure da forzare se oltre la lunghezza della stringa</param>
        /// <returns>valore codificato su 2 bytes del carattere</returns>
        protected static ushort _codificaSubString(string Testo, int Posizione, byte valoreNullo = 0x00 )
        {
            byte _tempbyte = valoreNullo;

            try
            {
                if (Posizione < Testo.Length)
                {
                    _tempbyte = (byte)Testo[Posizione];
                }
                else
                {
                    _tempbyte = valoreNullo;
                }

                return _codificaByte( _tempbyte );
            }
            catch (Exception Ex)
            {
                Log.Error("_codificaSubString: " + Ex.Message);
                return _codificaByte(_tempbyte);
            }
        }


        /// <summary>
        /// Suddivide il valore passato (Unsugned Short) in 2 byte
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_loVal"></param>
        /// <param name="_hiVal"></param>
        public void splitUshort(ushort _value, ref byte _loVal, ref byte _hiVal)
        {
            _loVal = (byte)(_value & 0xFFu);
            _hiVal = (byte)((_value >> 8) & 0xFFu);
        }

        public void splitUint32( UInt32 _value, ref byte _byte1, ref byte _byte2, ref byte _byte3, ref byte _byte4)

        {
            _byte4 = (byte)(_value & 0x000000FF);
            _byte3 = (byte)((_value >> 8) & 0x000000FF);
            _byte2 = (byte)((_value >> 16) & 0x000000FF);
            _byte1 = (byte)((_value >> 24) & 0x000000FF);
        }

        public void splitInt32( Int32 _value, ref byte _byte1, ref byte _byte2, ref byte _byte3, ref byte _byte4)

        {
            _byte4 = (byte)(_value & 0x000000FF);
            _byte3 = (byte)((_value >> 8) & 0x000000FF);
            _byte2 = (byte)((_value >> 16) & 0x000000FF);
            _byte1 = (byte)((_value >> 24) & 0x000000FF);
        }

        public static string ArrayToString(byte[] source, int start, int lenght)
        {
            string _tempString = "";
            byte[] _tempBuf;
            System.Text.StringBuilder _tempB = new System.Text.StringBuilder();

            try
            {
                _tempBuf = new byte[lenght];
                Array.Copy(source, start, _tempBuf, 0, lenght);
                _tempString = System.Text.Encoding.ASCII.GetString(_tempBuf);
                // tolgo gli 0x00 finali 
                _tempString = _tempString.TrimEnd('\0');
                return _tempString;
            }
            catch
            {
                return "";
            }
        }

        public static byte[] StringToArray(string source, int ArrayLen)
        {
            string _tempString = "";
        

            try
            {
                byte[] _tempBuf = new byte[ArrayLen];
                byte[] _tempData = Encoding.ASCII.GetBytes(source);
                for(int _i = 0; _i < ArrayLen; _i++)
                {
                    if (_i < _tempData.Length)
                        _tempBuf[_i] = _tempData[_i];
                    else
                        _tempBuf[_i] = 0x00;

                }
                return _tempBuf;
            }
            catch
            {
                return null;
            }
        }

        public static ushort ArrayToUshort(byte[] source, int start, int lenght)
        {
            ushort _tempVal16 = 0;

            try
            {
                for (int _i = start; _i < (start + lenght); _i++)
                {
                    if (_i < source.Length)
                    {
                        _tempVal16 = (ushort)(_tempVal16 << 8);
                        _tempVal16 += source[_i];
                    }

                }
                return _tempVal16;
            }
            catch
            {
                return 0;
            }
        }

        public static short ArrayToShort(byte[] source, int start, int lenght)
        {
            short _tempVal16 = 0;
            int _segno = 1;
            byte _tempB;
            try
            {
                for (int _i = start; _i < (start + lenght); _i++)
                {
                    if (_i < source.Length)
                    {
                        _tempB = source[_i];
                        _tempVal16 = (short)(_tempVal16 << 8);
                        _tempVal16 += _tempB;
                    }

                }
                return _tempVal16;
            }
            catch
            {
                return 0;
            }
        }

        public static short ArrayToSShort(byte[] source, int start, int lenght)
        {
            short _tempVal16 = 0;
            int _segno = 1;
            byte _tempB;
            try
            {
                for (int _i = start; _i < (start + lenght); _i++)
                {
                    if (_i < source.Length)
                    {
                        _tempB = source[_i];
                        if (_i == start)
                        {
                            if ((_tempB & 0x80) > 0)
                                _segno = -1;
                            else
                                _segno = 1;

                            _tempB = (byte)(_tempB & 0x7F);
                        }
                        _tempVal16 = (short)(_tempVal16 << 8);
                        _tempVal16 += _tempB;
                    }

                }
                return (short)(_tempVal16 * _segno);
            }
            catch
            {
                return 0;
            }
        }


        public static UInt16 ArrayToUint16(byte[] source, int start, int lenght)
        {
            UInt16 _tempVal16 = 0;

            try
            {
                for (int _i = start; _i < (start + lenght); _i++)
                {
                    if (_i < source.Length)
                    {
                        _tempVal16 = (UInt16)( _tempVal16 << 8 );
                        _tempVal16 += source[_i];
                    }

                }
                return _tempVal16;
            }
            catch
            {
                return 0;
            }
        }

        public static UInt32 ArrayToUint32(byte[] source, int start, int lenght)
        {
            UInt32 _tempVal = 0;
 
            try
            {
                for (int _i = start; _i < (start + lenght); _i++)
                {
                    if (_i < source.Length)
                    {
                        _tempVal = _tempVal << 8;
                        _tempVal += source[_i] ;
                    }

                }
                return _tempVal;
            }
            catch
            {
                return 0;
            }
        }

        public static Int32 ArrayToint32(byte[] source, int start, int lenght)
        {
            Int32 _tempVal = 0;
            int _segno = 1;
            byte _tempB;
            try
            {
                for (int _i = start; _i < (start + lenght); _i++)
                {
                    if (_i < source.Length)
                    {
                        _tempB = source[_i];
                        _tempVal = _tempVal << 8;
                        _tempVal += source[_i];
                    }

                }
                return _tempVal;
            }
            catch
            {
                return 0;
            }
        }


        public static Int32 ArrayToSint32(byte[] source, int start, int lenght)
        {
            Int32 _tempVal = 0;
            int _segno = 1;
            byte _tempB;
            try
            {
                for (int _i = start; _i < (start + lenght); _i++)
                {
                    if (_i < source.Length)
                    {
                        _tempB = source[_i];
                        if (_i == start)
                        {
                            if ((_tempB & 0x80) > 0)
                                _segno = -1;
                            else
                                _segno = 1;

                            _tempB = (byte)(_tempB & 0x7F);
                        }

                        _tempVal = _tempVal << 8;
                        _tempVal += _tempB;// source[_i];
                    }

                }
                return _tempVal * _segno;
            }
            catch
            {
                return 0;
            }
        }
        
        public static byte[] SubArray(byte[] source, int start, int lenght)
        {
            byte[] _tempVal;

            try
            {
                _tempVal = new byte[lenght];

                for (int _i = 0 ; _i <  lenght ; _i++)
                {
                    if (_i < source.Length)
                    {
                        _tempVal[_i] = source[( _i  + start )];
                    }

                }
                return _tempVal;
            }
            catch
            {
                return new byte[lenght];
            }
        }

        public string hexdumpMessaggio()
        {
            try
            {
                string _risposta = "";

                for (int _i = 0; _i < MessageBuffer.Length; _i++)
                {
                    _risposta += MessageBuffer[_i].ToString("X2");
                }
                return _risposta;
            }
            catch
            {
                return "";
            }
        }

        public string hexdumpArray(byte[] buffer)
        {
            try
            {
                string _risposta = "";

                if (buffer == null)
                    return "";

                for (int _i = 0; _i < buffer.Length; _i++)
                {
                    _risposta += buffer[_i].ToString("X2");
                }
                return _risposta;
            }
            catch
            {
                return "";
            }
        }

        public string StringaTensione(uint Tensione)
        {
            try
            {
                string _tensioni = "";
                float _inVolt;
                _inVolt = Tensione / 100;
                _tensioni = _inVolt.ToString("0.0");
                return _tensioni;
            }
            catch
            {
                return "";
            }
        }

        public string StringaCorrente(uint Corrente)
        {
            try
            {
                string _correnti = "";
                float _inAmpere;
                _inAmpere = Corrente / 10;
                _correnti = _inAmpere.ToString("0");
                return _correnti;
            }
            catch
            {
                return "";
            }
        }

        public string StringaPresenza(byte Valore)
        {
            try
            {
                string _Flag = "";
                switch (Valore)
                {
                    case 0xF0: 
                        _Flag = "OK";
                        break;
                    case 0x0F:
                        _Flag = "KO";
                        break;
                    default:
                        _Flag = Valore.ToString("x2");
                        break;
                }

                return _Flag;
            }
            catch
            {
                return "";
            }
        }

        public class Crc16Ccitt
        {
            const ushort poly = 4129;
            ushort[] table = new ushort[256];
            ushort initialValue = 0;

            public ushort ComputeChecksum(byte[] bytes)
            {
                ushort crc = this.initialValue;
                for (int i = 0; i < bytes.Length; ++i)
                {
                    crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
                }
                return crc;
            }

            public byte[] ComputeChecksumBytes(byte[] bytes)
            {
                ushort crc = ComputeChecksum(bytes);
                return BitConverter.GetBytes(crc);
            }

            public Crc16Ccitt(InitialCrcValue initialValue)
            {
                this.initialValue = (ushort)initialValue;
                ushort temp, a;
                for (int i = 0; i < table.Length; ++i)
                {
                    temp = 0;
                    a = (ushort)(i << 8);
                    for (int j = 0; j < 8; ++j)
                    {
                        if (((temp ^ a) & 0x8000) != 0)
                        {
                            temp = (ushort)((temp << 1) ^ poly);
                        }
                        else
                        {
                            temp <<= 1;
                        }
                        a <<= 1;
                    }
                    table[i] = temp;
                }
            }
        }

        public class comandoRTC
        {

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public ushort anno;
            public byte mese;
            public byte giorno;
            public byte giornoSett;
            public byte ore;
            public byte minuti;
            public byte secondi;
            public bool datiPronti;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {
                ushort _tempShort;
                byte _tempByte;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length != 16) { return EsitoRisposta.NonRiconosciuto; }

                    _tempByte = decodificaByte(_messaggio[0], _messaggio[1]);
                    _tempShort = (ushort)( _tempByte);
                    _tempByte = decodificaByte(_messaggio[2], _messaggio[3]);
                    _tempShort = (ushort)((_tempShort << 8) + _tempByte);

                    anno = _tempShort;
                    mese = decodificaByte(_messaggio[4], _messaggio[5]);
                    giorno = decodificaByte(_messaggio[6], _messaggio[7]);
                    giornoSett = decodificaByte(_messaggio[8], _messaggio[9]);
                    ore = decodificaByte(_messaggio[10], _messaggio[11]);
                    minuti = decodificaByte(_messaggio[12], _messaggio[13]);
                    secondi = decodificaByte(_messaggio[14], _messaggio[15]);
                    datiPronti = true;
                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }
            
            }

            public EsitoRisposta componiMessaggio(byte[] _messaggio)
            {
                ushort _tempShort;
                byte _tempByte;
                byte[] _tempArray;
                byte[] _tempMessaggio = new byte[16];
                byte[] _tempFromShort = new byte[2];

                try
                {
                    //l'intestazione deve essere pronta

                    datiPronti = false;
                    if (anno > 2000 & anno < 2100)
                    {
                        _tempArray = new byte[2];
                        _tempArray = BitConverter.GetBytes(anno);

                        _tempShort = _codificaByte(_tempArray[1]);
                        _tempFromShort = BitConverter.GetBytes(_tempShort);


                    }
                    else
                    {
                        return EsitoRisposta.ParametriErrati;
                    }


                    if (_messaggio.Length != 16) { return EsitoRisposta.NonRiconosciuto; }

                    _tempByte = decodificaByte(_messaggio[0], _messaggio[1]);
                    _tempShort = (ushort)(_tempByte);
                    _tempByte = decodificaByte(_messaggio[2], _messaggio[3]);
                    _tempShort = (ushort)((_tempShort << 8) + _tempByte);

                    anno = _tempShort;
                    mese = decodificaByte(_messaggio[4], _messaggio[5]);
                    giorno = decodificaByte(_messaggio[6], _messaggio[7]);
                    giornoSett = decodificaByte(_messaggio[8], _messaggio[9]);
                    ore = decodificaByte(_messaggio[10], _messaggio[11]);
                    minuti = decodificaByte(_messaggio[12], _messaggio[13]);
                    secondi = decodificaByte(_messaggio[14], _messaggio[15]);
                    datiPronti = true;
                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }



        }

        public class cicloAttuale
        {

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public string NomeCiclo = "";
            public byte LunghezzaNome = 0;
            public byte NumeroParametri;
            public byte TipoCiclo;
            public bool datiPronti;
            public List<ParametroLL> Parametri = new List<ParametroLL>();

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {
                ushort _tempShort;
                byte _tempByte;
                int startByte;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    _dataBuffer = new byte[(_messaggio.Length)];
                    if (decodificaArray(_messaggio, ref _dataBuffer))
                    {
                        startByte = 0;
                        LunghezzaNome = _dataBuffer[startByte];
                        if (LunghezzaNome == 0xFF)
                        {
                            //Programmazione corrente non inizializzata 
                            LunghezzaNome = 4;
                            NomeCiclo = "N.D.";
                            TipoCiclo = (byte)TipoCicloLadeLight.ND;
                            Parametri.Clear();
                            datiPronti = true;
                            return EsitoRisposta.MessaggioOk;
                        }

                        startByte += 1;

                        TipoCiclo = _dataBuffer[startByte];
                        startByte += 1;
                        NumeroParametri = _dataBuffer[startByte];
                        startByte += 1;

                        NomeCiclo = ArrayToString(_dataBuffer, startByte, LunghezzaNome);
                        startByte += LunghezzaNome;
                        Parametri.Clear();
                        for (int _ciclo = 0; _ciclo < NumeroParametri; _ciclo ++)
                        {
                            if(startByte > _dataBuffer.Length )
                            {
                                datiPronti = false;
                                return EsitoRisposta.ParametriErrati;
                            }
                            ParametroLL _parLL = new ParametroLL();
                            _parLL.idParametro = _dataBuffer[startByte];
                            startByte += 1;
                     
                            _parLL.ValoreParametro = ArrayToUshort(_dataBuffer, startByte, 2);
                            startByte += 2;
                            Parametri.Add(_parLL);
                        }
                    }
                    datiPronti = true;
                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }

            public EsitoRisposta componiMessaggio(byte[] _messaggio)
            {

                byte[] _tempDurata;


                byte[] _tempMessaggio = new byte[12];

                try
                {
                    //l'intestazione deve essere pronta

                    datiPronti = false;

                    _tempDurata = new byte[4];
                   // _tempDurata = BitConverter.GetBytes(TempoCarica);


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }



        }

        public class cicliPresenti
        {
            public byte NumCicli;
            public System.Collections.Generic.List<UInt32> ListaCicli;



            byte[] _dataBuffer;
            public byte[] dataBuffer; public bool datiPronti;
            public string lastError;


            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {
                 byte[] _risposta;
                int startByte = 0;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    _risposta = new byte[(_messaggio.Length / 2)];

                    if (decodificaArray(_messaggio, ref _risposta))
                    {
                        startByte = 0;
                        NumCicli = _risposta[startByte];
                        startByte += 1;
                        ListaCicli = new List<UInt32>();
             

                        for (int _cicli = 0; _cicli < NumCicli; _cicli++)
                        {
                            UInt32 _idCiclo;
                            _idCiclo = ArrayToUint32(_risposta, startByte, 4);
                            startByte += 4;
                            ListaCicli.Add(_idCiclo);
                        }


                        datiPronti = true;

                    }


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }

        
        }

        public class CicloDiCarica
        {
            public UInt32 IdCiclo;
            public ushort Progressivo;
            public ushort anno;
            public byte mese;
            public byte giorno;
            public byte giornoSett;
            public byte ore;
            public byte minuti;

            public string DataCiclo;
            public string OraCiclo;

            public ushort TensioneIniziale;
            public ushort Tensione5Min;
            public ushort Corrente5Min;
            public ushort TensioneStop;
            public ushort CorrenteStop;
            public ushort AhCaricati;
            public byte OreDurata;
            public byte MinutiDurata;
            public string DurataCarica;
            public byte Errori;
            public byte CondizioneStop;

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {

                byte[] _risposta;
                int startByte = 0;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    _risposta = new byte[(_messaggio.Length / 2)];

                    if (decodificaArray(_messaggio, ref _risposta))
                    {
                        startByte = 0;
                        IdCiclo = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        Progressivo = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;

                        anno = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        mese = _risposta[startByte];
                        startByte += 1;
                        giorno = _risposta[startByte];
                        startByte += 1;
                        DataCiclo = giorno.ToString("00") + "/" + mese.ToString("00") + "/" + anno.ToString("0000");
                        giornoSett = _risposta[startByte];
                        startByte += 1;
                        ore = _risposta[startByte];
                        startByte += 1;
                        minuti = _risposta[startByte];
                        startByte += 1;
                        OraCiclo = ore.ToString("00") + ":" + minuti.ToString("00") ;

                        TensioneIniziale = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Tensione5Min = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Corrente5Min = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        TensioneStop = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        CorrenteStop = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        AhCaricati = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;

                        OreDurata = _risposta[startByte];
                        startByte += 1;
                        MinutiDurata = _risposta[startByte];
                        startByte += 1;
                        DurataCarica = ore.ToString("00") + ":" + minuti.ToString("00");

                        Errori = _risposta[startByte];
                        startByte += 1;
                        CondizioneStop = _risposta[startByte];
                        startByte += 1;

                        datiPronti = true;

                    }


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }


        }

        public class CicloCorrente
        {

            public byte LenNomeCiclo;
            public string NomeCiclo;
            public byte Parametri;



            public string DurataCarica;
            public byte Errori;
            public byte CondizioneStop;

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {

                byte[] _risposta;
                int startByte = 0;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    _risposta = new byte[(_messaggio.Length / 2)];

                    if (decodificaArray(_messaggio, ref _risposta))
                    {


                        Errori = _risposta[startByte];
                        startByte += 1;
                        CondizioneStop = _risposta[startByte];
                        startByte += 1;

                        datiPronti = true;

                    }


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }


        }

        public class ProxyComandoStrategia
        {


            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public byte[] RxBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {

                byte[] _risposta;
                int startByte = 0;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    RxBuffer = new byte[(_messaggio.Length / 2)];

                    if (decodificaArray(_messaggio, ref RxBuffer))
                    {


                        datiPronti = true;

                    }


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }


        }


        public class VariabiliLadeLight
        {

            public byte StatoCorrente;
            public ushort Vbatt;
            public ushort Ibatt;
            public UInt32 AhCaricati;
            public uint SecondiTrascorsi;

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {

                byte[] _risposta;
                int startByte = 0;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    _risposta = new byte[(_messaggio.Length / 2)];

                    if (decodificaArray(_messaggio, ref _risposta))
                    {
                        startByte = 0;
                        Vbatt = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;

                        Ibatt = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;

                        AhCaricati = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;

                        SecondiTrascorsi =  ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;

                        StatoCorrente =  _risposta[startByte];
                        startByte += 1;

                        datiPronti = true;

                    }


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }


        }


        public class comandoIniziale
        {

            byte[] _dataBuffer;
            public byte[] dataBuffer; 

            public string PrimaInstallazione;
            public UInt32 Matricola;
            public UInt32 Progressivo;
            public byte tensioneNominale;
            public ushort correnteNominale;
            public string modello { get; set; }
            public string revSoftware;
            public string revDisplay;
            private byte _lenModello;

            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {
                ushort _tempShort;
                byte _tempByte1;
                byte _tempByte2;
                byte[] _risposta;
                int startByte = 0;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2) 
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    _risposta = new byte[(_messaggio.Length/2)];

                    if(decodificaArray(_messaggio,ref _risposta))
                    {
                        startByte = 0;
                        Matricola = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        Progressivo = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        _lenModello = _risposta[startByte];
                        startByte += 1;
                        modello = ArrayToString(_risposta, startByte, _lenModello);
                        startByte += _lenModello;

                        //anno
                        _tempShort = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        //mese
                        _tempByte1 = _risposta[startByte];
                        startByte += 1;
                        //giorno
                        _tempByte2 = _risposta[startByte];
                        startByte += 1;
                        PrimaInstallazione = _tempByte2.ToString("00") + "/" + _tempByte1.ToString("00") + "/" + _tempShort.ToString("0000");
                        //Salto giorno settimana
                        startByte += 1;
                        tensioneNominale = _risposta[startByte];
                        startByte += 1;
                        correnteNominale = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        revSoftware = "N.D.";
                        revDisplay = "N.D.";

                        if (_risposta.Length >= startByte + 8)
                        {
                            revSoftware = _risposta[startByte].ToString();
                            startByte += 1;
                            revSoftware += "." +_risposta[startByte].ToString("00");
                            startByte += 1;
                            _tempShort = ArrayToUshort(_risposta, startByte, 2);
                            startByte += 2;
                            revSoftware += "." + _tempShort.ToString("0000");

                            revDisplay = _risposta[startByte].ToString();
                            startByte += 1;
                            revDisplay += "." + _risposta[startByte].ToString("00");
                            startByte += 1;
                            _tempShort = ArrayToUshort(_risposta, startByte, 2);
                            startByte += 2;
                            revDisplay += "." + _tempShort.ToString("0000");

                        }


                        datiPronti = true;

                    }


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }

        }

        public class PacchettoReadMem
        {

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public ushort numBytes;
            public UInt32 memAddress;
            public byte[] memData;
            public byte[] memDataDecoded;


            public bool datiPronti;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {

                byte[] _risposta;
                int startByte = 0;
                ushort _tempShort;
                byte _tempByte;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }
                    numBytes = (ushort)(_messaggio.Length / 2);
                    _risposta = new byte[numBytes];
                    memData = new byte[numBytes];

                    if (decodificaArray(_messaggio, ref _risposta))
                    {
                        startByte = 0;
                        memData = _messaggio;
                        memDataDecoded = _risposta;
                    }

                    datiPronti = true;
                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }

            public EsitoRisposta componiMessaggio(byte[] _messaggio)
            {
                ushort _tempShort;
                byte _tempByte;
                byte[] _tempArray;
                byte[] _tempMessaggio = new byte[10];
                byte[] _tempFromShort = new byte[2];

                try
                {
                    //l'intestazione deve essere pronta

                    datiPronti = false;

                    if (_messaggio.Length != 10) { return EsitoRisposta.NonRiconosciuto; }

                    _tempByte = decodificaByte(_messaggio[0], _messaggio[1]);
                    _tempShort = (ushort)(_tempByte);
                    _tempByte = decodificaByte(_messaggio[2], _messaggio[3]);
                    _tempShort = (ushort)((_tempShort << 8) + _tempByte);

                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }


            }

        }


    }


}
