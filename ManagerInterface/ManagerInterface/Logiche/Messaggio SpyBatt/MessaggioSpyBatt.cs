 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

using Utility;


namespace ChargerLogic
{
    public partial class MessaggioSpyBatt : SerialMessage
    {

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        private string _lastError;

        private byte[] _idCorrente = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private byte[] _tempId = { 0, 0, 0, 0, 0, 0, 0, 0 };

        public comandoInizialeSB Intestazione;
        public comandoRTC DatiRTCSB;
        public DatiCliente CustomerData;
        public ProgrammaRicarica ProgRicarica;
        public MemoriaPeriodoLungo CicloLungo;
        public MemoriaPeriodoBreve _CicloBreve;
        public PacchettoReadMem _pacchettoMem;
        public VariabiliSpybatt variabiliScheda;
        public CalibrazioniSpybatt valoriCalibrazione;
        public ImmagineDumpMem DumpMem;
        public StatoFirmware StatoFirmwareScheda;
        public ComandoStrategia ComandoStrat;
        public ParametriSpybatt ParametriGenerali;
        public OcBaudRate  BrOCcorrente = OcBaudRate.OFF;
        public OcEchoMode EchoOCcorrente = OcEchoMode.OFF;

        public byte[] LLstopMessage;

        public StatoSig60 StatoTrxOC;
        public byte ReserCounterOC;


        public byte _comandoInvio;
        public byte _pacchettoInviato;
        public byte _esitoPacchetto;
        public EsitoMessaggio EsitoComando;

        public int fwLevel = 0;

        private EsitoRisposta _ultimaRisposta = EsitoRisposta.MessaggioVuoto;

        public MessaggioSpyBatt()
        {
            fwLevel = 0;
        }


        public MessaggioSpyBatt(int LivelloFirmware)
        {
            fwLevel = LivelloFirmware;
        }

        new public EsitoRisposta analizzaMessaggio(byte[] _messaggio, int fwLevel, bool skipHead = false, bool CreateMsgObject = false)
        {
            byte _ret;
            int _startPos;
            int _endPos;
            byte[] _buffArray;
            ushort _tempShort;

            EsitoRisposta _risposta;
            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);
            int preambleLenght;
            //  int _startByte;

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

                // ora in base al comando cambio lettura:
                switch (_comando)
                {
                    case (byte)TipoComando.ACK_PACKET: // ACK
                        {
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
                        }

                    case (byte)TipoComando.EVENT_MEM_CODE: // 0x6D  ACK Pacchetti in scrittura multipla
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

                            EsitoComando = new EsitoMessaggio();
                            _risposta = EsitoComando.analizzaMessaggio(_buffArray, fwLevel);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.NACK_PACKET: //NAK
                        {
                            _crc = 0;
                            Log.Warn("--- NACK --- NACK --- NACK --- NACK --- NACK --- NACK --- NACK --- NACK --- NACK --- NACK --- NACK ---");
                            return EsitoRisposta.ErroreGenerico;
                        }

                    case (byte)TipoComando.SB_DatiIniziali:  //risposta parametri iniziali
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
                            Intestazione = new comandoInizialeSB();
                            _buffArray = new byte[(_endPos - (preambleLenght + 7))];
                            Array.Copy(_messaggio, preambleLenght + 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            _risposta = Intestazione.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.SB_R_CicloLungo:
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
                            CicloLungo = new MemoriaPeriodoLungo();
                            _buffArray = new byte[(_endPos - (preambleLenght + 7))];
                            Array.Copy(_messaggio, preambleLenght + 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            _risposta = CicloLungo.analizzaMessaggio(_buffArray, fwLevel);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.SB_R_CicloBreve:
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
                            _CicloBreve = new MemoriaPeriodoBreve();
                            _buffArray = new byte[(_endPos - (preambleLenght + 7))];
                            Array.Copy(_messaggio, preambleLenght + 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            _risposta = _CicloBreve.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.SB_R_DatiCliente:
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
                            _risposta = CustomerData.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.SB_R_Programmazione:  // Lettura dati programmazione
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
                            ProgRicarica = new ProgrammaRicarica();
                            _risposta = ProgRicarica.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.SB_W_Programmazione:  // Lettura dell'esito della scrittura programmazionme                         
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

                            // ora leggo la parte dati: deve essere 1 solo byte  ( 2 con la codifica )
                            int _lenMsg = _endPos - (preambleLenght + 7);
                            if (_lenMsg != 2) { return EsitoRisposta.ErroreGenerico; }
                            _startPos = preambleLenght + 1;
                   
                            _ret = decodificaByte(_messaggio[_startPos], _messaggio[_startPos + 1]);
                            Log.Debug(" ---------------------- Scrittura Programmazione -----------------------------------------");
                            Log.Debug("Esito: 0x" + _ret.ToString("x2"));
                            if (ProgRicarica != null)
                            {
                                ProgRicarica.EsitoScrittura = _ret;
                            }

                            break;
                        }

                    case (byte)TipoComando.SB_R_ParametriLettura:  // Lettura dati programmazione
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
                            ParametriGenerali = new ParametriSpybatt();
                            _risposta = ParametriGenerali.analizzaMessaggio(_buffArray, fwLevel);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.SB_R_DumpMemoria:  // Lettura intera memoria
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
                            if (fwLevel > 2)
                            {
                                _buffArray = new byte[(_endPos - (preambleLenght + 11))];
                            }
                            else
                            {
                                _buffArray = new byte[(_endPos - (preambleLenght + 7))];

                            }

                            if (_comando != (byte)TipoComando.SB_R_DumpMemoria)
                            {   // dove non ho la testata corretta recupero i 2 bytes del comando
                                //Array.Copy(_messaggio, preambleLenght - 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                                if (fwLevel > 2)
                                {
                                    // Tolgo i 2 bytes di contatore

                                    Array.Copy(_messaggio, preambleLenght + 3, _buffArray, 0, _endPos - (preambleLenght + 11));
                                }
                                else
                                {
                                    Array.Copy(_messaggio, preambleLenght - 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                                }


                            }
                            else
                            {
                                if (fwLevel > 2)
                                {
                                    // Tolgo i 2 bytes di contatore

                                    Array.Copy(_messaggio, preambleLenght + 5, _buffArray, 0, _endPos - (preambleLenght + 11));
                                }
                                else
                                {
                                    Array.Copy(_messaggio, preambleLenght + 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                                }
                            }
                            // Se la classe destinazione non è definita, la ricreo
                            if (DumpMem == null)
                            {
                                DumpMem = new ImmagineDumpMem();
                            }
                            _risposta = DumpMem.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.SB_R_LeggiMemoria: // Lettura frammento memoria
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
                            _pacchettoMem = new PacchettoReadMem();
                            _buffArray = new byte[(_endPos - (preambleLenght + 7))];
                            if (_comando != (byte)TipoComando.SB_R_LeggiMemoria)
                            {   // dove non ho la testata corretta recupero i 2 bytes del comando
                                Array.Copy(_messaggio, preambleLenght - 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            }
                            else
                            {
                                Array.Copy(_messaggio, preambleLenght + 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            }

                            _risposta = _pacchettoMem.analizzaMessaggio(_buffArray);
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
                            _buffArray = new byte[(_endPos - (preambleLenght + 7))];
                            Array.Copy(_messaggio, preambleLenght + 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            _risposta = variabiliScheda.analizzaMessaggio(_buffArray, fwLevel);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.SB_Cal_LetturaGain:
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
                            _risposta = valoriCalibrazione.analizzaMessaggio(_buffArray, fwLevel);
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

                    case (byte)TipoComando.SB_R_ParametriSIG60:  // Lettura impostazioni SIG60
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
                            byte _tempByte;

                            _buffArray = new byte[(_endPos - (preambleLenght + 7))];
                            Array.Copy(_messaggio, preambleLenght + 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            ushort numBytes = (ushort)(_buffArray.Length / 2);
                            byte[] _rispostaOC = new byte[numBytes];


                            if (decodificaArray(_buffArray, ref _rispostaOC))
                            {
                                _tempByte = _rispostaOC[0];
                                if (Enum.IsDefined(typeof(OcBaudRate), _tempByte))
                                    BrOCcorrente = (OcBaudRate)_tempByte;
                                else
                                    BrOCcorrente = OcBaudRate.OFF;
                                _risposta = StatoTrxOC.analizzaMessaggio(_buffArray);
                                if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }
                            }
                            else
                            {
                                BrOCcorrente = OcBaudRate.OFF;
                            }

                            return EsitoRisposta.MessaggioOk; 

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
                            Intestazione = new comandoInizialeSB();
                            _buffArray = new byte[(_endPos - 29)];
                            Array.Copy(_messaggio, 23, _buffArray, 0, _endPos - 29);
                            _risposta = Intestazione.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case 0x03: //id cicli
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

                            break;
                        }

                    case (byte)TipoComando.SB_ReadRTC: // 0xD3: // read RTC
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
                            DatiRTCSB = new comandoRTC();
                            _buffArray = new byte[16];
                            Array.Copy(_messaggio, 23, _buffArray, 0, 16);
                            _risposta = DatiRTCSB.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.LL_W_FineCarica: // 0xD3: // read RTC
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
                            DatiRTCSB = new comandoRTC();
                            LLstopMessage = new byte[5];
                            Array.Copy(_messaggio, 23, LLstopMessage, 0, 5);

                            //_risposta = DatiRTCSB.analizzaMessaggio(_buffArray);
                            
                            //if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case (byte)TipoComando.SB_W_chgst_Call: // messaggio dati strategia
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
                            if (CreateMsgObject) ComandoStrat = new ComandoStrategia();
                            _buffArray = new byte[(_endPos - (preambleLenght + 7))];
                            Array.Copy(_messaggio, preambleLenght + 1, _buffArray, 0, _endPos - (preambleLenght + 7));
                            _risposta = ComandoStrat.analizzaMessaggio(_buffArray);
                            if (_risposta != EsitoRisposta.MessaggioOk) { return EsitoRisposta.ErroreGenerico; }

                            break;
                        }

                    case 0x02: // read Ciclo Corrente
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

                    default:
                        return EsitoRisposta.NonRiconosciuto;
                       
                }



                return EsitoRisposta.MessaggioOk;

            }
            catch (Exception Ex)
            {
                Log.Error("sb -> analizzaMessaggio: " + Ex.Message);
                _lastError = Ex.Message;

                return EsitoRisposta.ErroreGenerico;
            }

        }

        public EsitoRisposta UltimaRisposta
        {
            get { return _ultimaRisposta; }
        }

        public byte[] serialeCorrente
        {
            get { return _idCorrente; }
        }

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

        public ushort ComponiMessaggioCicloLungo(UInt32 IdCiclo)
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

                _comando = (byte)(TipoComando.SB_R_CicloLungo);
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

                // Id ushort
                splitUint32(IdCiclo, ref _conv32[0], ref _conv32[1], ref _conv32[2], ref _conv32[3]);

                splitUshort(codificaByte(_conv32[0]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                splitUshort(codificaByte(_conv32[1]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                splitUshort(codificaByte(_conv32[2]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 5] = msb;
                MessageBuffer[_arrayInit + 6] = lsb;
                splitUshort(codificaByte(_conv32[3]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 7] = msb;
                MessageBuffer[_arrayInit + 8] = lsb;

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

        public ushort ComponiMessaggioCicloBreve(UInt32 IdCiclo, UInt32 PtrBreve, ushort Pacchetti)
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

                _comando = (byte)(TipoComando.SB_R_CicloBreve);
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

                // Id ushort
                splitUint32(IdCiclo, ref _conv32[0], ref _conv32[1], ref _conv32[2], ref _conv32[3]);

                splitUshort(codificaByte(_conv32[0]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                splitUshort(codificaByte(_conv32[1]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                splitUshort(codificaByte(_conv32[2]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 5] = msb;
                MessageBuffer[_arrayInit + 6] = lsb;
                splitUshort(codificaByte(_conv32[3]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 7] = msb;
                MessageBuffer[_arrayInit + 8] = lsb;
                _arrayInit += 8;

                // Ptr
                splitUint32(PtrBreve, ref _conv32[0], ref _conv32[1], ref _conv32[2], ref _conv32[3]);
                splitUshort(codificaByte(_conv32[1]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                splitUshort(codificaByte(_conv32[2]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                splitUshort(codificaByte(_conv32[3]), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 5] = msb;
                MessageBuffer[_arrayInit + 6] = lsb;
                // splitUshort(codificaByte(_conv32[3]), ref lsb, ref msb);
                // MessageBuffer[_arrayInit + 7] = msb;
                // MessageBuffer[_arrayInit + 8] = lsb;
                _arrayInit += 6;


                // Contatore 
                splitUshort(Pacchetti, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;

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

        public ushort ComponiMessaggioCliente(byte NumPacchetto, int fwLevel)
        {
            ushort _esito = 0;
            ushort _dispositivo;
            byte _comando;
            ushort _tempShort;
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

                _comando = (byte)(TipoComando.SB_W_DatiCliente);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen;
                //int _arrayLimit;


                if (NumPacchetto == 0)
                {
                    // Pacchetto 0 - solo testata

                    if (fwLevel >= 6)
                    {
                        // _arraylen deve essere la lunghezza effettiva del messaggio, senza CRC (4) e codici di controllo iniziali e finali ( stx, end_pac ed etx )
                        _arrayLen = _arrayInit + 4;
                    }
                    else
                    {
                        // _arraylen deve essere la lunghezza effettiva del messaggio, senza CRC (4) e codici di controllo iniziali e finali ( stx, end_pac ed etx )
                        _arrayLen = _arrayInit;
                    }


                    MessageBuffer = new byte[_arrayLen + 7];
                    //Array.Resize(ref MessageBuffer, _arrayLen + 7);                
                    MessageBuffer[0] = serSTX;

                    for (int m = 0; m < _arrayInit; m++)
                    {
                        MessageBuffer[m + 1] = _comandoBase[m];
                    }

                }
                else
                {
                    // dalla versione 1.06 la testatadeve essere sempre presente
                    //_arrayLen = 504;
                    //_arrayInit = 0;
                    //MessageBuffer = new byte[_arrayLen + 7];
                    //Array.Resize(ref MessageBuffer, _arrayLen + 7);                
                    //for (int m = 0; m < _arrayLen; m++)
                    //{
                    //    MessageBuffer[m + 1] = 0x00;
                    //}
                    //MessageBuffer[0] = serSTX;
                    _arrayLen = _arrayInit + 470;
                    MessageBuffer = new byte[_arrayLen + 7];
                    //Array.Resize(ref MessageBuffer, _arrayLen + 7);                
                    for (int m = 0; m < _arrayLen; m++)
                    {
                        MessageBuffer[m + 1] = 0x00;
                    }
                    MessageBuffer[0] = serSTX;

                    for (int m = 0; m < _arrayInit; m++)
                    {
                        MessageBuffer[m + 1] = _comandoBase[m];
                    }

                }



                /// aggiungo il corpo
                /// in base al numero pacchetto (1/4) copongo il testo
                int _i;  //contatore di servizio
                switch (NumPacchetto)
                {
                    case 0x00:
                        if (fwLevel >= 6)
                        {
                            // Reset contatori
                            splitUshort((ushort)CustomerData.ResetLivelloCarica, ref lsbDisp, ref msbDisp);

                            splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                            MessageBuffer[(_arrayInit + 1)] = msb;
                            MessageBuffer[(_arrayInit + 2)] = lsb;

                            splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                            MessageBuffer[(_arrayInit + 3)] = msb;
                            MessageBuffer[(_arrayInit + 4)] = lsb;
                            _arrayInit += 4;
                        }
                        else
                        {
                            //non faccio nulla, solo testata
                        }
                        break;

                    case 0x01:
                        // id pacchetto
                        splitUshort(codificaByte(NumPacchetto), ref lsb, ref msb);
                        MessageBuffer[_arrayInit + 1] = msb;
                        MessageBuffer[_arrayInit + 2] = lsb;
                        _arrayInit += 2;

                        // cliente
                        for (_i = 0; _i < 110; _i++)
                        {

                            splitUshort(_codificaSubString(CustomerData.Client, _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;

                        }
                        // note cliente
                        for (_i = 0; _i < 110; _i++)    // test: 120 eff
                        {
                            splitUshort(_codificaSubString(CustomerData.ClientNote, _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;
                        }

                        // Chiudo a 470 byte effettivi
                        for (_i = _arrayInit; _i < _arrayLen; _i += 2)
                        {
                            splitUshort(_codificaSubString("", _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;
                        }
                        break;

                    case 0x02:
                        // id pacchetto
                        splitUshort(codificaByte(NumPacchetto), ref lsb, ref msb);
                        MessageBuffer[_arrayInit + 1] = msb;
                        MessageBuffer[_arrayInit + 2] = lsb;
                        _arrayInit += 2;

                        // Marca Batteria
                        for (_i = 0; _i < 110; _i++)
                        {

                            splitUshort(_codificaSubString(CustomerData.BatteryBrand, _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;

                        }
                        // modello batteria
                        for (_i = 0; _i < 55; _i++)
                        {
                            splitUshort(_codificaSubString(CustomerData.BatteryModel, _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;
                        }

                        // Id batteria
                        for (_i = 0; _i < 50; _i++)
                        {
                            splitUshort(_codificaSubString(CustomerData.BatteryId, _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;
                        }

                        // Cicli Attesi
                        _tempShort = (ushort)(CustomerData.CicliAttesi);
                        splitUshort(_tempShort, ref lsbDisp, ref msbDisp);

                        splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                        MessageBuffer[_arrayInit + 1] = msb;
                        MessageBuffer[_arrayInit + 2] = lsb;
                        _arrayInit += 2;

                        splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                        MessageBuffer[_arrayInit + 1] = msb;
                        MessageBuffer[_arrayInit + 2] = lsb;
                        _arrayInit += 2;

                        // ID batteria per Ladelight

                        string _tempId;
                        if(CustomerData.BatteryLLId =="")
                        {
                            _tempId = "1    ";
                        }
                        else
                        {
                            _tempId = CustomerData.BatteryLLId + "     ";
                        }

                        for (_i = 0; _i < 5; _i++)
                        {
                            splitUshort(_codificaSubString(_tempId, _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;
                        }



                        // Chiudo a 252 byte effettivi
                        for (_i = _arrayInit; _i < _arrayLen; _i += 2)
                        {
                            splitUshort(_codificaSubString("", _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;
                        }
                        break;


                    case 0x03:
                        // id pacchetto
                        splitUshort(codificaByte(NumPacchetto), ref lsb, ref msb);
                        MessageBuffer[_arrayInit + 1] = msb;
                        MessageBuffer[_arrayInit + 2] = lsb;
                        _arrayInit += 2;

                        // Serial Number
                        for (_i = 0; _i < 20; _i++)
                        {

                            splitUshort(_codificaSubString(CustomerData.SerialNumber, _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;

                        }

                        // Chiudo a 252 byte effettivi
                        for (_i = _arrayInit; _i < _arrayLen; _i += 2)
                        {
                            splitUshort(_codificaSubString("", _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;
                        }
                        break;

                    case 0x04:
                        // id pacchetto
                        splitUshort(codificaByte(NumPacchetto), ref lsb, ref msb);
                        MessageBuffer[_arrayInit + 1] = msb;
                        MessageBuffer[_arrayInit + 2] = lsb;
                        _arrayInit += 2;
                        splitUshort(codificaByte(CustomerData.ModoPianificazione), ref lsb, ref msb);
                        MessageBuffer[_arrayInit + 1] = msb;
                        MessageBuffer[_arrayInit + 2] = lsb;
                        _arrayInit += 2;

                        // in base al modo pianificazione cambio la mappa dati

                        switch((ParametriSetupPro.TipoPianificazione)CustomerData.ModoPianificazione)
                        {
                            case ParametriSetupPro.TipoPianificazione.NonDefinita:  // nessuna pianificazione
                            case ParametriSetupPro.TipoPianificazione.Turni:  // turni base: non implementata
                            case ParametriSetupPro.TipoPianificazione.TurniEsteso:  // turni estesi: non implementata
                            default: // -- registro solo modo pianificazione
                                {
                                    break;
                                }
                            case ParametriSetupPro.TipoPianificazione.Tempo: // tempo base
                                {

                                    splitUshort(codificaByte(CustomerData.ModoBiberonaggio), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;


                                    splitUshort(codificaByte(CustomerData.ModoRabboccatore), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    // ora gli 84 bytes della mappa turni
                                    if (CustomerData.ModelloPianificazione.Length == 84)
                                    {
                                        for (int _mp = 0; _mp < 84; _mp++)
                                        {
                                            splitUshort(codificaByte(CustomerData.ModelloPianificazione[_mp]), ref lsb, ref msb);
                                            MessageBuffer[_arrayInit + 1] = msb;
                                            MessageBuffer[_arrayInit + 2] = lsb;
                                            _arrayInit += 2;
                                        }
                                    }

                                    // Salto lo spazio della struttura contatori (17 bytes)
                                    for (_i = 0; _i < 17; _i += 1)
                                    {
                                        splitUshort(_codificaByte(0x00), ref lsb, ref msb);
                                        MessageBuffer[_arrayInit + 1] = msb;
                                        MessageBuffer[_arrayInit + 2] = lsb;
                                        _arrayInit += 2;
                                    }

                                    // poi i dati di equal:
                                    splitUshort(codificaByte(CustomerData.EqualNumImpulsi), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    splitUshort(codificaByte(CustomerData.EqualNumImpulsiExtra), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    splitUshort(codificaByte(CustomerData.EqualMinErogazione), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    splitUshort(codificaByte(CustomerData.EqualMinPausa), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    splitUshort(codificaByte(CustomerData.EqualMinAttesa), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    splitUshort(codificaByte(CustomerData.EqualNumImpulsiExtra), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    _tempShort = (ushort)(CustomerData.EqualPulseCurrent);
                                    splitUshort(_tempShort, ref lsbDisp, ref msbDisp);

                                    splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    break;
                                }
                            case ParametriSetupPro.TipoPianificazione.TempoEsteso: // tempo esteso
                                {
                                    //MODO_BIBERONAGGIO
                                    splitUshort(codificaByte(CustomerData.ModoBiberonaggio), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    //MODO_RABBOCCATORE
                                    splitUshort(codificaByte(CustomerData.ModoRabboccatore), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    //Vuoto per future estensioni
                                    splitUshort(codificaByte(0x00), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    //EQ_MIN_PULSE
                                    splitUshort(codificaByte(CustomerData.EqualMinErogazione), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    //EQ_MIN_PAUSE
                                    splitUshort(codificaByte(CustomerData.EqualMinPausa), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    //EQ_MIN_WAIT
                                    splitUshort(codificaByte(CustomerData.EqualMinAttesa), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    //EQ_NUM_PULSE
                                    splitUshort(codificaByte(CustomerData.EqualNumImpulsi), ref lsb, ref msb);
                                    MessageBuffer[_arrayInit + 1] = msb;
                                    MessageBuffer[_arrayInit + 2] = lsb;
                                    _arrayInit += 2;

                                    //EQ_PULSE_CURRENT
                                    splitUshort(CustomerData.EqualPulseCurrent, ref lsbDisp, ref msbDisp);

                                    splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                                    MessageBuffer[(_arrayInit + 1)] = msb;
                                    MessageBuffer[(_arrayInit + 2)] = lsb;

                                    splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                                    MessageBuffer[(_arrayInit + 3)] = msb;
                                    MessageBuffer[(_arrayInit + 4)] = lsb;
                                    _arrayInit += 4;

                                    // 27 Bytes liberi
                                    for (_i = 0; _i < 26; _i += 1)
                                    {
                                        splitUshort(_codificaByte(0x00), ref lsb, ref msb);
                                        MessageBuffer[_arrayInit + 1] = msb;
                                        MessageBuffer[_arrayInit + 2] = lsb;
                                        _arrayInit += 2;
                                    }


                                    // ora i 168 bytes della mappa turni
                                    if (CustomerData.ModelloPianificazione.Length == 168)
                                    {
                                        for (int _mp = 0; _mp < 168; _mp++)
                                        {
                                            splitUshort(codificaByte(CustomerData.ModelloPianificazione[_mp]), ref lsb, ref msb);
                                            MessageBuffer[_arrayInit + 1] = msb;
                                            MessageBuffer[_arrayInit + 2] = lsb;
                                            _arrayInit += 2;
                                        }
                                    }

                                    // Salto lo spazio della struttura contatori (17 bytes)
                                    for (_i = 0; _i < 17; _i += 1)
                                    {
                                        splitUshort(_codificaByte(0x00), ref lsb, ref msb);
                                        MessageBuffer[_arrayInit + 1] = msb;
                                        MessageBuffer[_arrayInit + 2] = lsb;
                                        _arrayInit += 2;
                                    }

                                    break;
                                }


                        }



                        // Chiudo a 252 byte effettivi
                        for (_i = _arrayInit; _i < _arrayLen; _i += 2)
                        {
                            splitUshort(_codificaSubString("", _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;
                        }
                        break;


                    default:
                        // id pacchetto
                        splitUshort(codificaByte(NumPacchetto), ref lsb, ref msb);
                        MessageBuffer[_arrayInit + 1] = msb;
                        MessageBuffer[_arrayInit + 2] = lsb;
                        _arrayInit += 2;
                        // Stringa vuota lunga 255
                        for (_i = _arrayInit; _i < _arrayLen; _i += 2)
                        {
                            splitUshort(_codificaSubString("", _i), ref lsb, ref msb);
                            MessageBuffer[_arrayInit + 1] = msb;
                            MessageBuffer[_arrayInit + 2] = lsb;
                            _arrayInit += 2;
                        }
                        break;
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

                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("sb -> ComponiMessaggioCliente: " + Ex.Message);
                _lastError = Ex.Message;

                return _esito;
            }

        }

        public ushort ComponiMessaggioNuovoProgramma(int FwLevel)
        {
            ushort _esito = 0;
            //ushort _tempUshort;
            byte _tempByte;
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

                _comando = (byte)(TipoComando.SB_W_Programmazione);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 256;  // alloco comunque lo spazio per messaggio max possibile

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }





                /*******************************************************************************/
                /* Parte dati                                                                  */
                /*******************************************************************************/
                /* Parametri Base                                                              */
                /*******************************************************************************/
                if (FwLevel >= 6)
                {
                    splitUshort((ushort)ProgRicarica.ResetLivelloCarica, ref lsbDisp, ref msbDisp);

                    splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                    MessageBuffer[(_arrayInit + 1)] = msb;
                    MessageBuffer[(_arrayInit + 2)] = lsb;

                    splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                    MessageBuffer[(_arrayInit + 3)] = msb;
                    MessageBuffer[(_arrayInit + 4)] = lsb;
                    _arrayInit += 4;
                }

                // id programma
                splitUshort(ProgRicarica.IdProgramma, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Data Installazione

                _tempByte = (byte)DateTime.Now.Day;
                splitUshort(codificaByte(_tempByte), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                _tempByte = (byte)DateTime.Now.Month;
                splitUshort(codificaByte(_tempByte), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                int _tempAnno = DateTime.Now.Year;
                _tempByte = (byte)(_tempAnno % 100);
                splitUshort(codificaByte(_tempByte), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Tensione nominale
                splitUshort(ProgRicarica.BatteryVdef, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Corrente nominale
                splitUshort(ProgRicarica.BatteryAhdef, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Battery Type
                splitUshort(codificaByte(ProgRicarica.BatteryType), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Celle Totali
                splitUshort(codificaByte(ProgRicarica.BatteryCells), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Celle 3
                splitUshort(codificaByte(ProgRicarica.BatteryCell3), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Celle 2
                splitUshort(codificaByte(ProgRicarica.BatteryCell2), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Celle 1
                splitUshort(codificaByte(ProgRicarica.BatteryCell1), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Abilita sensore elettrolita
                splitUshort(codificaByte(ProgRicarica.AbilitaPresElett), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Temp Max
                splitUshort(codificaByte(ProgRicarica.TempMax), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Temp Min
                splitUshort(codificaByte(ProgRicarica.TempMin), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Verso Corrente
                splitUshort(codificaByte(ProgRicarica.VersoCorrente), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Numero Spire
                splitUshort(codificaByte(ProgRicarica.NumeroSpire), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                /*******************************************************************************/
                /* Parametri PRO                                                               */
                /*******************************************************************************/

                // Modo Pianificazione
                splitUshort(codificaByte(ProgRicarica.ModoPianificazione), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;


                // Corrente Minima
                splitUshort(ProgRicarica.CorrenteCaricaMin, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;


                // Corrente Massima
                splitUshort(ProgRicarica.CorrenteCaricaMax, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Impulsi Rabboccatore
                splitUshort(codificaByte(ProgRicarica.PulseRabboccatore), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Ripetizioni Rabboccatore
                splitUshort(codificaByte(ProgRicarica.RipetizioniRabboccatore), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;


                // Consenso Biberonaggio
                splitUshort(codificaByte(ProgRicarica.FlagBiberonaggio), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;


                // Coeff Biberonaggio
                splitUshort(codificaByte(ProgRicarica.CoeffBiberonaggio), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Temp Attenzione
                splitUshort(codificaByte(ProgRicarica.TempAttenzione), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Temp Allarme
                splitUshort(codificaByte(ProgRicarica.TempAllarme), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Temp Ripresa
                splitUshort(codificaByte(ProgRicarica.TempRipresa), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;


                // Max Sbilanciamento
                splitUshort(ProgRicarica.MaxSbilanciamento, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Durata Sbilanciamento
                splitUshort(ProgRicarica.TempoSbilanciamento, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Tensione Gas
                splitUshort(ProgRicarica.TensioneGas, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // deriva superiore
                splitUshort(ProgRicarica.DerivaSuperiore, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // deriva inferiore
                splitUshort(ProgRicarica.DerivaInferiore, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Corrente Minima W
                splitUshort(ProgRicarica.MinCorrenteW, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;


                // Corrente max W
                splitUshort(ProgRicarica.MaxCorrenteW, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Max Corremte IMP
                splitUshort(ProgRicarica.MaxCorrenteImp, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Tensione Raccordo
                splitUshort(ProgRicarica.TensioneRaccordo , ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Tensione Finale
                splitUshort(ProgRicarica.TensioneFinale, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;


                // dati pianificazione : Spostati nei dati cliente
                /*
                // Copio comunque i primi 84 bytes
                for (int _cicloByte = 0; _cicloByte < 79; _cicloByte++)
                {
                    splitUshort(codificaByte(0x00), ref lsb, ref msb);
                    MessageBuffer[(_arrayInit + 1)] = msb;
                    MessageBuffer[(_arrayInit + 2)] = lsb;
                    _arrayInit += 2;
                }

                */
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
                Log.Error("sb -> ComponiMessaggioCliente: " + Ex.Message);
                _lastError = Ex.Message;

                return _esito;
            }

        }

        public new ushort ComponiMessaggioTestataFW(byte Blocco, byte[] Intestazione )
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
                // Prima controllo che siano effettivamente 64 byte
                if(Intestazione.Length != 64)
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

                _comando = (byte)(TipoComando.CMD_FW_UPLOAD_MSP);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 130;

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
                Log.Error("sb -> ComponiMessaggioTestataFW: " + Ex.Message);
                _lastError = Ex.Message;

                return _esito;
            }

        }

        public ushort ComponiMessaggioSwitchFW(byte Blocco)
        {
            ushort _esito = 0;
           // ushort _tempUshort;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            try
            {
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

                _comando = (byte)(TipoComando.CMD_FW_UPDATE);
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





                /*******************************************************************************/
                /* Parte dati                                                                  */
                /*******************************************************************************/

                // id Blocco

                splitUshort(codificaByte(Blocco), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;


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
                Log.Error("sb -> ComponiMessaggioSwitchFW: " + Ex.Message);
                _lastError = Ex.Message;

                return _esito;
            }

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

                _comando = (byte)(TipoComando.SB_W_BLON);
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

        /// <summary>
        /// Preparo il messaggio di invio pacchetto dati per l'aggiornamento firmware
        /// </summary>
        /// <param name="NumPacchetto">Progressivo pacchetto (ushort)</param>
        /// <param name="NumBytes">Dimensione del pachetto (byte, deve essere inferioreo uguale a 130)</param>
        /// <param name="Dati">byte array del pacchetto con CRC finale</param>
        /// <returns></returns>
        public new ushort ComponiMessaggioPacchettoDatiFW(ushort NumPacchetto,byte NumBytes, byte[] Dati, ushort CRCPacchetto)
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
                if (Dati.Length >130 )
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

                for (int _i = 0; _i < ( NumBytes - 2 ); _i++)
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
                Log.Error("sb -> ComponiMessaggioDatiFW: " + Ex.Message);
                _lastError = Ex.Message;

                return _esito;
            }

        }

        public new ushort ComponiMessaggioLeggiMem(UInt32 memAddress, ushort numBytes)
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

                _comando = (byte)(TipoComando.SB_R_LeggiMemoria);
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
                //Dal FW 1.0.6 num bytes e 1 solo byte

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
            catch { return _esito; }
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

                _comando = (byte)(TipoComando.SB_W_ScriviMemoria);
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
                _lastError = Ex.Message;

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

                _comando = (byte)(TipoComando.SB_Cancella4K);
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

        public ushort ComponiMessaggioNuovoProgrammaOLD()
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

                _comando = (byte)(TipoComando.SB_W_Programmazione);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 28;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }





                /*******************************************************************************/
                /* Parte dati                                                                  */
                /*******************************************************************************/

                // id programma
                splitUshort(ProgRicarica.IdProgramma, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Data Installazione  al momento fisso 1/1/14
                splitUshort(codificaByte(0x01), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                splitUshort(codificaByte(0x01), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                splitUshort(codificaByte(0x0E), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Tensione nominale
                splitUshort(ProgRicarica.BatteryVdef, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Corrente nominale
                splitUshort(ProgRicarica.BatteryAhdef, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

                // Battery Type
                splitUshort(codificaByte(ProgRicarica.BatteryType), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Celle Totali
                splitUshort(codificaByte(ProgRicarica.BatteryCells), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Celle 3
                splitUshort(codificaByte(ProgRicarica.BatteryCell3), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Celle 2
                splitUshort(codificaByte(ProgRicarica.BatteryCell2), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Celle 1
                splitUshort(codificaByte(ProgRicarica.BatteryCell1), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;





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
                Log.Error("sb -> ComponiMessaggioCliente: " + Ex.Message);
                _lastError = Ex.Message;

                return _esito;
            }

        }

        public ushort ComponiMessaggioLeggiProgrammazioni(ushort IdProgramma)
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

                _comando = (byte)(TipoComando.SB_R_Programmazione);
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

                // id programma
                splitUshort(IdProgramma, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

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

        public ushort ComponiMsgScriviParCalibrazione(byte IdParametro,ushort ValoreCalibrazione)
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

                _comando = (byte)(TipoComando.SB_Cal_InvioDato);
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

                // id parametro

                splitUshort(codificaByte(IdParametro), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;
                _arrayInit += 2;

                // Valore guadagno
                splitUshort(ValoreCalibrazione, ref lsbDisp, ref msbDisp);

                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 1)] = msb;
                MessageBuffer[(_arrayInit + 2)] = lsb;

                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[(_arrayInit + 3)] = msb;
                MessageBuffer[(_arrayInit + 4)] = lsb;
                _arrayInit += 4;

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

        public ushort ComponiMessaggioLeggiInteraMemoria(LadeLightBool SendAck)
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

                _comando = (byte)(TipoComando.SB_R_DumpMemoria);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit;
                if (fwLevel >= 2)
                {
                    _arrayLen += 2;
                }
                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                for (int _ii = 0; _ii < (_arrayLen + 7); _ii++)
                {
                    MessageBuffer[_ii] = 0x00;
                }

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo - Solo da FW 1.08.01 ( 1.09 ) 
                /// 
                if ( fwLevel >= 2 )
                {

                    splitUshort(codificaByte((byte) SendAck), ref lsb, ref msb);
                    MessageBuffer[(_arrayInit + 1)] = msb;
                    MessageBuffer[(_arrayInit + 2)] = lsb;
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


                return _esito;
            }
            catch { return _esito; }
        }

        //--------

        public ushort ComponiMessaggioTestStrategia(byte SottoComando)
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

                _comando = (byte)(TipoComando.SB_W_chgst_Call);
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
                // Prima il sottocomando

                splitUshort(codificaByte(SottoComando), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;

                // Num Bytes (0)
                lsbDisp = 0;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
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


                return _esito;
            }
            catch { return _esito; }
        }

        public ushort ComponiMessaggioBaseStrategia(byte[] Parametri)
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

            // Mi aspetto il comando di chiamata standard


            try
            {
                int _LunghezzaComando = Parametri.Length;
                if (_LunghezzaComando < 10)
                    return _esito;

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

                _comando = (byte)(TipoComando.SB_W_chgst_Call);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + (_LunghezzaComando*2);

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 
                // Prima il sottocomando

                for (int _cmd = 0; _cmd < _LunghezzaComando; _cmd++)
                {

                    splitUshort(codificaByte(Parametri[_cmd]), ref lsb, ref msb);
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


                return _esito;
            }
            catch { return _esito; }
        }

        public ushort ComponiMessaggioOpenStrategia(byte[] Parametri)
        {
            ushort _esito = 0;
            ushort _dispositivo;
            byte _comando;
            byte msbDisp = 0;
            byte lsbDisp = 0;
            byte msb = 0;
            byte lsb = 0;
            byte[] _conv32 = new byte[4];
            int _extradata = Parametri.Length;

            Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

            // Mi aspetto il comando di chiamata standard


            try
            {
                if (Parametri.Length < 2)
                    return _esito;

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

                _comando = (byte)(TipoComando.SB_W_chgst_Call);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + _extradata * 2 ;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 
                // Prima il sottocomando

                for (int _cmd = 0; _cmd < _extradata; _cmd++)
                {

                    splitUshort(codificaByte(Parametri[_cmd]), ref lsb, ref msb);
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


                return _esito;
            }
            catch { return _esito; }
        }

        public ushort ComponiMessaggioScriviParametriLettura( ushort LettureCorrente,ushort LettureTensione,ushort DurataPausa)
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

                _comando = (byte)(TipoComando.SB_W_ParametriLettura);
                splitUshort(codificaByte(_comando), ref lsb, ref msb);
                _comandoBase[(20)] = msb;
                _comandoBase[(21)] = lsb;

                int _arrayInit = _comandoBase.Length;
                int _arrayLen = _arrayInit + 12;

                Array.Resize(ref MessageBuffer, _arrayLen + 7);

                MessageBuffer[0] = serSTX;
                for (int m = 0; m < _arrayInit; m++)
                {
                    MessageBuffer[m + 1] = _comandoBase[m];
                }

                /// aggiungo il corpo
                /// 

                // Num Letture Corrente 
                splitUshort(LettureCorrente, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                _arrayInit += 4;

                // Num Letture Tensione 
                splitUshort(LettureTensione, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                _arrayInit += 4;


                // Secondi Durata Pausa
                splitUshort(DurataPausa, ref lsbDisp, ref msbDisp);
                splitUshort(codificaByte(msbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                splitUshort(codificaByte(lsbDisp), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 3] = msb;
                MessageBuffer[_arrayInit + 4] = lsb;
                _arrayInit += 4;

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

        public ushort ComponiMessaggioScriviParametriSig(OcBaudRate SetupCorrente, OcEchoMode EchoCorrente)
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
                //Comando
                _comando = (byte)(TipoComando.SB_W_ParametriSIG60);
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

                // ON/OFF - Baud Rate 
                splitUshort(codificaByte((byte)SetupCorrente), ref lsb, ref msb);
                MessageBuffer[_arrayInit + 1] = msb;
                MessageBuffer[_arrayInit + 2] = lsb;
                _arrayInit += 2;

                // Echo
                splitUshort(codificaByte((byte)EchoCorrente), ref lsb, ref msb);
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


                return _esito;
            }
            catch { return _esito; }
        }




    }
}