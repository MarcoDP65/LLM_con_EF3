using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using MoriData;
using Utility;

namespace ChargerLogic
{

    class FirmwareManager
    {
        public string TestoFirmwareCCS { get; set; }

        public FileFirmware FirmwareData;

        public BloccoFirmware FirmwareBlock;

        public enum ExitCode : byte
        {
            OK = 0,
            FileAssente = 0x01, FileVuoto = 0x02, FormatoFileErrato = 0x03,
            DatiNonPronti = 0x11, FormatoDatiErrato = 0x13,
            ErroreGenerico = 0xFF
        };

        public enum MascheraStato : byte
        {
            Blocco1HW = 0x01,
            Blocco2HW = 0x02,
            FlashmPHW = 0x04,
            Blocco1SW = 0x08,
            Blocco2SW = 0x10,
            BootLoaderInUso = 0x20,
            Blocco1InUso = 0x40,
            Blocco2InUso = 0x80,
        }

        public const int BloccoTx = 128;

        public ExitCode CaricaFileCCS(string FileName)
        {
            string _fileBuffer = "";
            ExitCode _esito = ExitCode.ErroreGenerico;

            try
            {
                // File non definito
                if (FileName == "") return ExitCode.FileAssente;
                if (File.Exists(FileName))
                {
                    // Il file esiste conimcio a leggerlo
                    FirmwareData = new FileFirmware();
                    _fileBuffer = File.ReadAllText(FileName);
                    _esito = AnalizzaFileCCS(_fileBuffer, ref FirmwareData);

                    if (_esito == ExitCode.OK)
                    {

                    }
                }
                return _esito;
            }
            catch
            {
                return _esito;
            }

        }

        public string LastError;

        /// <summary>
        /// Analizza il contenuto del file CSS e carica la struttura dati
        /// </summary>
        /// <param name="DataFile"></param>
        /// <param name="BloccoFirmware"></param>
        /// <returns></returns>
        public ExitCode AnalizzaFileCCS(string DataFile, ref FileFirmware BloccoFirmware)
        {
            try
            {
                bool _eofFound = false;
                BloccoFirmware = new FileFirmware();
                if (DataFile == "") return ExitCode.FileAssente;


                string[] _blocchiFW = DataFile.Split('@');

                // ora ho 3 o 4 blocchi terminati da @
                // se non sono 4, il file non è strutturato correttamente

                if (!((_blocchiFW.Length == 3)||(_blocchiFW.Length == 4))) return ExitCode.FormatoFileErrato;

                // Il primo è vuoto, lo ignoro
                // ----------------------------------
                // Il secondo contiene il segmento FLASH 1, lo spezzo sui terminatori \r\n         
                string[] _blocchiFl1 = Regex.Split(_blocchiFW[1], "\r\n");
                uint _tempUint;
                uint _contaBytes = 0;
                List<byte> _tempMem = new List<byte>();

                // STEP 1: estraggo l'indirizzo FLASH 1
                //la prima riga è la testata
                uint.TryParse(_blocchiFl1[0], System.Globalization.NumberStyles.HexNumber, null, out _tempUint);
                BloccoFirmware.AddrFlash1 = _tempUint;
                _tempMem.Clear();
                _contaBytes = 0;
                for (int _righe = 1; _righe < _blocchiFl1.Length; _righe++)
                {
                    string _contRiga = _blocchiFl1[_righe];
                    string[] _byteFW = _contRiga.Split(' ');
                    foreach (string _singleByte in _byteFW)
                    {
                        byte _tempByte;
                        if (byte.TryParse(_singleByte, System.Globalization.NumberStyles.HexNumber, null, out _tempByte))
                        {
                            _contaBytes++;
                            _tempMem.Add(_tempByte);

                        }
                    }

                }
                BloccoFirmware.DataFlash1 = new byte[_contaBytes];
                BloccoFirmware.LenFlash1 = _contaBytes;
                for (int _i = 0; _i < _contaBytes; _i++)
                {
                    BloccoFirmware.DataFlash1[_i] = _tempMem.ElementAt(_i);
                }


                // ----------------------------------
                // Il terzo contiene il segmento Proxy Table, lo spezzo sui terminatori \r\n   
                string[] _blocchiPT = Regex.Split(_blocchiFW[2], "\r\n");
                // STEP 3: estraggo l'indirizzo PROXY
                //la prima riga è la testata
                uint.TryParse(_blocchiPT[0], System.Globalization.NumberStyles.HexNumber, null, out _tempUint);
                BloccoFirmware.AddrProxy = _tempUint;
                _tempMem.Clear();
                _contaBytes = 0;
                for (int _righe = 1; _righe < _blocchiPT.Length; _righe++)
                {
                    string _contRiga = _blocchiPT[_righe];
                    string[] _byteFW = _contRiga.Split(' ');
                    foreach (string _singleByte in _byteFW)
                    {
                        if (_singleByte == "q")
                        {
                            //trovato il finefile
                            _eofFound = true;
                        }

                        byte _tempByte;
                        if (byte.TryParse(_singleByte, System.Globalization.NumberStyles.HexNumber, null, out _tempByte))
                        {
                            _contaBytes++;
                            _tempMem.Add(_tempByte);

                        }
                    }

                }
                BloccoFirmware.DataProxy = new byte[_contaBytes];
                BloccoFirmware.LenProxy = (ushort)_contaBytes;

                for (int _i = 0; _i < _contaBytes; _i++)
                {
                    BloccoFirmware.DataProxy[_i] = _tempMem.ElementAt(_i);
                }


                // ----------------------------------
                // Il quarto, se c'è contiene il segmento Flash_2, lo spezzo sui terminatori \r\n  
                if ((_blocchiFW.Length == 4) && (!_eofFound))
                {
                    // esiste la seconda area flash


                    string[] _blocchiFl2 = Regex.Split(_blocchiFW[3], "\r\n");
                    // STEP 4: estraggo l'indirizzo FLASH_2
                    //la prima riga è la testata
                    uint.TryParse(_blocchiFl2[0], System.Globalization.NumberStyles.HexNumber, null, out _tempUint);
                    BloccoFirmware.AddrFlash2 = _tempUint;
                    _tempMem.Clear();
                    _contaBytes = 0;
                    for (int _righe = 1; _righe < _blocchiFl2.Length; _righe++)
                    {
                        string _contRiga = _blocchiFl2[_righe];
                        string[] _byteFW = _contRiga.Split(' ');
                        foreach (string _singleByte in _byteFW)
                        {
                            if (_singleByte == "q")
                            {
                                //trovato il finefile
                                _eofFound = true;
                            }

                            byte _tempByte;
                            if (byte.TryParse(_singleByte, System.Globalization.NumberStyles.HexNumber, null, out _tempByte))
                            {
                                _contaBytes++;
                                _tempMem.Add(_tempByte);

                            }
                        }

                    }
                    BloccoFirmware.DataFlash2 = new byte[_contaBytes];
                    BloccoFirmware.LenFlash2 = _contaBytes;

                    for (int _i = 0; _i < _contaBytes; _i++)
                    {
                        BloccoFirmware.DataFlash2[_i] = _tempMem.ElementAt(_i);
                    }
                }
                else
                {
                    BloccoFirmware.AddrFlash2 = 0;
                    BloccoFirmware.DataFlash2 = new byte[0];
                    BloccoFirmware.LenFlash2 = 0;
                }

                if (_eofFound)
                {
                    BloccoFirmware.DatiOK = true;
                    return ExitCode.OK;
                }

                return ExitCode.FormatoFileErrato;

            }
            catch
            {
                return ExitCode.ErroreGenerico;
            }
        }

        public ExitCode GeneraFileSBF(string VersioneFw, string DataRilascio, string NomeFile, bool CifraFile = true)
        {
            ExitCode _esito = ExitCode.ErroreGenerico;
            ushort _crc;
            elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
            try
            {

                if (NomeFile == "") return ExitCode.FileAssente;
                if (!File.Exists(NomeFile))
                    File.Create(NomeFile).Close();
                else
                {
                    File.Delete(NomeFile);
                    File.Create(NomeFile).Close();
                }

                FirmwareData.Release = VersioneFw;
                FirmwareData.ReleaseDate = DataRilascio;
                FirmwareData.ReleaseDateBlock = FunzioniMR.toArrayDataTS(DataRilascio);
                //calcolo il CRC di pacchetto con valore CRC 0 poi metto il crc corretto
                FirmwareData.crc = 0;

                string _tempSer = JsonConvert.SerializeObject(FirmwareData);
                byte[] _tepBSer = FunzioniMR.GetBytes(_tempSer);

                _crc = codCrc.ComputeChecksum(_tepBSer);
                FirmwareData.crc = _crc;

                //Serializzo il pacchetto dati
                string JsonData = JsonConvert.SerializeObject(FirmwareData);


                // Cifro i dati


                string JsonEncript = StringCipher.Encrypt(JsonData);

                // ora salvo il file 
                File.AppendAllText(NomeFile, JsonEncript);
                _esito = ExitCode.OK;
                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        public ExitCode CaricaFileSBF(string NomeFile, bool CifraFile = true)
        {
            ExitCode _esito = ExitCode.ErroreGenerico;
            LastError = "";
            try
            {
                FirmwareData = new FileFirmware();
                FirmwareData.DatiOK = false;

                if (File.Exists(NomeFile))
                {
                    string _fileImport = File.ReadAllText(NomeFile);

                    string _fileDecripted = StringCipher.Decrypt(_fileImport);
                    if (_fileDecripted != "")
                    {
                        //il file è cifrato
                      
                        _fileImport = _fileDecripted;
                    }
                    /*
                     
                    Nella versione pubblica rifiutare i files non cifrati

                    else
                    {
                        //MessageBox.Show("File danneggiato: impossibile caricare i dati", "Importazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _esito = ExitCode.FormatoFileErrato;
                        return _esito;
                    }
                    */

                    FileFirmware _tmpFirmwareData;
                    elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
                    ushort _crc;


                    _tmpFirmwareData = JsonConvert.DeserializeObject<FileFirmware>(_fileImport);
                    ushort _tempCRC = _tmpFirmwareData.crc;
                    _tmpFirmwareData.crc = 0;

                    string _tempSer = JsonConvert.SerializeObject(_tmpFirmwareData);
                    byte[] _tempBSer = FunzioniMR.GetBytes(_tempSer);

                    // rivedere il controllo crc
                    _crc = codCrc.ComputeChecksum(_tempBSer);

                    if (_crc == _tempCRC)
                    { // I CRC ciincidono: dati validi
                        _tmpFirmwareData.crc = _tempCRC;
                        FirmwareData = _tmpFirmwareData;
                        FirmwareData.DatiOK = true;
                        _esito = ExitCode.OK;
                    }
                    else
                    {
                        //MessageBox.Show("File danneggiato: impossibile caricare i dati", "Importazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _esito = ExitCode.FormatoFileErrato;
                    }

                }
                else
                {
                    _esito = ExitCode.FileAssente;
                }

                //MessageBox.Show("File Valido", "Importazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return _esito;
            }
            catch (Exception Ex)
            {
                LastError = Ex.Message;
                return _esito;
            }
        }

        public ExitCode PreparaUpgradeFw()
        {
            ExitCode _esito = ExitCode.ErroreGenerico;
            elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
            PacchettoDatiFW _blocco;

            LastError = "";

            uint _blockDiv;
            ushort _blockMod;

            uint _blockSize = 0;


            try
            {

                if (!FirmwareData.DatiOK)
                {
                    _esito = ExitCode.DatiNonPronti;
                    return _esito;
                }



                // Preparo la classe destinazione
                FirmwareBlock = new BloccoFirmware();

                // Carico i dati di testata
                FirmwareBlock.Release = FirmwareData.Release;
                FirmwareBlock.ReleaseDate = FirmwareData.ReleaseDate;

                if ((FirmwareData.Release.Substring(0,2) != "1.") && (FirmwareData.Release.Substring(0, 2) != "2."))
                {
                    _esito = ExitCode.ErroreGenerico;
                    return _esito;
                }


                FirmwareBlock.AddrFlash1 = FirmwareData.AddrFlash1;
                FirmwareBlock.AddrFlash2 = FirmwareData.AddrFlash2;
                FirmwareBlock.AddrProxy = FirmwareData.AddrProxy;

                FirmwareBlock.DataFlash1 = FirmwareData.DataFlash1;
                FirmwareBlock.DataFlash2 = FirmwareData.DataFlash2;
                FirmwareBlock.DataProxy = FirmwareData.DataProxy;

                FirmwareBlock.LenFlash1 = FirmwareData.LenFlash1;
                FirmwareBlock.LenFlash2 = FirmwareData.LenFlash2;
                FirmwareBlock.LenProxy = FirmwareData.LenProxy;
                FirmwareBlock.TotaleBlocchi = 0;
                for( int _ii = 0; _ii<3; _ii++)
                {
                    FirmwareBlock.ReleaseDateBlock[_ii] = FirmwareData.ReleaseDateBlock[_ii];
                }

                _esito = ExitCode.FormatoDatiErrato;
                // Calcolo il CRC Totale
                FirmwareBlock.crc = codCrc.ComputeChecksum3(FirmwareBlock.DataFlash1, FirmwareBlock.DataFlash2, FirmwareBlock.DataProxy);

                //-----------------------------------------------------------
                //  FLASH 1
                //-----------------------------------------------------------
                // Impacchetto il settore Flash 1
                FirmwareBlock.ListaFlash1.Clear();
                _blockSize = 0;
                _blockDiv = FirmwareData.LenFlash1 / BloccoTx;
                _blockMod = (ushort)(FirmwareData.LenFlash1 % BloccoTx);
                // Prima i blocchi completi
                for (int _block = 0; _block < _blockDiv; _block++)
                {
                    _blocco = new PacchettoDatiFW();
                    _blocco.DimPacchetto = BloccoTx + 2;
                    _blocco.NumeroPacchetto = (uint)_block;

                    _blocco.PacchettoDati = new byte[BloccoTx];
                    Array.Copy(FirmwareData.DataFlash1, (_block * BloccoTx), _blocco.PacchettoDati, 0, BloccoTx);
                    _blocco.CRC = codCrc.ComputeChecksum(_blocco.PacchettoDati);
                    _blocco.DatiOK = true;
                    FirmwareBlock.ListaFlash1.Add(_blocco);
                    _blockSize += _blocco.DimPacchetto;
                    FirmwareBlock.TotaleBlocchi += 1;
                }

                // poi quello finale se resto > 0
                if (_blockMod > 0)
                {
                    _blocco = new PacchettoDatiFW();
                    _blocco.DimPacchetto = (ushort)(2 + _blockMod);
                    _blocco.NumeroPacchetto = (uint)_blockDiv;
                    _blocco.PacchettoDati = new byte[_blockMod];
                    Array.Copy(FirmwareData.DataFlash1, (_blockDiv * BloccoTx), _blocco.PacchettoDati, 0, _blockMod);
                    _blocco.CRC = codCrc.ComputePartialChecksum(_blocco.PacchettoDati, _blockMod);
                    _blocco.DatiOK = true;
                    FirmwareBlock.ListaFlash1.Add(_blocco);
                    _blockSize += _blocco.DimPacchetto;
                    FirmwareBlock.TotaleBlocchi += 1;
                }

                //FirmwareBlock.LenFlash1 = _blockSize;
                FirmwareBlock.LenFlash1 = FirmwareData.LenFlash1;

                //-----------------------------------------------------------
                //  FLASH 2
                //-----------------------------------------------------------
                // Impacchetto il settore Flash 2
                FirmwareBlock.ListaFlash2.Clear();
                _blockSize = 0;
                _blockDiv = FirmwareData.LenFlash2 / BloccoTx;
                _blockMod = (ushort)(FirmwareData.LenFlash2 % BloccoTx);
                // Prima i blocchi completi
                for (int _block = 0; _block < _blockDiv; _block++)
                {
                    _blocco = new PacchettoDatiFW();
                    _blocco.DimPacchetto = BloccoTx + 2;
                    _blocco.NumeroPacchetto = (uint)_block;
                    _blocco.PacchettoDati = new byte[BloccoTx];
                    Array.Copy(FirmwareData.DataFlash2, (_block * BloccoTx), _blocco.PacchettoDati, 0, BloccoTx);
                    _blocco.CRC = codCrc.ComputeChecksum(_blocco.PacchettoDati);
                    _blocco.DatiOK = true;
                    FirmwareBlock.ListaFlash2.Add(_blocco);
                    _blockSize += _blocco.DimPacchetto;
                    FirmwareBlock.TotaleBlocchi += 1;
                }


                // poi quello finale se resto > 0
                if (_blockMod > 0)
                {
                    _blocco = new PacchettoDatiFW();
                    _blocco.DimPacchetto = (ushort)(2 + _blockMod);
                    _blocco.NumeroPacchetto = (uint)_blockDiv;
                    _blocco.PacchettoDati = new byte[_blockMod];
                    Array.Copy(FirmwareData.DataFlash2, (_blockDiv * BloccoTx), _blocco.PacchettoDati, 0, _blockMod);
                    _blocco.CRC = codCrc.ComputePartialChecksum(_blocco.PacchettoDati, _blockMod);
                    _blocco.DatiOK = true;
                    FirmwareBlock.ListaFlash2.Add(_blocco);
                    _blockSize += _blocco.DimPacchetto;
                    FirmwareBlock.TotaleBlocchi += 1;
                }
                //FirmwareBlock.LenFlash2 = _blockSize;
                FirmwareBlock.LenFlash2 = FirmwareData.LenFlash2;

                //-----------------------------------------------------------
                //  proxy 
                //-----------------------------------------------------------
                // Impacchetto il settore proxy
                FirmwareBlock.ListaProxy.Clear();
                _blockSize = 0;
                _blockDiv = (uint)FirmwareData.LenProxy / BloccoTx;
                _blockMod = (ushort)(FirmwareData.LenProxy % BloccoTx);
                // Prima i blocchi completi
                for (int _block = 0; _block < _blockDiv; _block++)
                {
                    _blocco = new PacchettoDatiFW();
                    _blocco.DimPacchetto = BloccoTx + 2;
                    _blocco.NumeroPacchetto = (uint)_block;
                    _blocco.PacchettoDati = new byte[BloccoTx];
                    Array.Copy(FirmwareData.DataProxy, (_block * BloccoTx), _blocco.PacchettoDati, 0, BloccoTx);
                    _blocco.CRC = codCrc.ComputeChecksum(_blocco.PacchettoDati);
                    _blocco.DatiOK = true;
                    FirmwareBlock.ListaProxy.Add(_blocco);
                    _blockSize += _blocco.DimPacchetto;
                    FirmwareBlock.TotaleBlocchi += 1;
                }

                // poi quello finale se resto > 0
                if (_blockMod > 0)
                {
                    _blocco = new PacchettoDatiFW();
                    _blocco.DimPacchetto = (ushort)(2 + _blockMod);
                    _blocco.NumeroPacchetto = (uint)_blockDiv;
                    _blocco.PacchettoDati = new byte[_blockMod];
                    Array.Copy(FirmwareData.DataProxy, (_blockDiv * BloccoTx), _blocco.PacchettoDati, 0, _blockMod);
                    _blocco.CRC = codCrc.ComputePartialChecksum(_blocco.PacchettoDati, _blockMod);
                    _blocco.DatiOK = true;
                    FirmwareBlock.ListaProxy.Add(_blocco);
                    _blockSize += _blocco.DimPacchetto;
                    FirmwareBlock.TotaleBlocchi += 1;
                }
                //FirmwareBlock.LenProxy = (ushort)_blockSize;
                FirmwareBlock.LenProxy = FirmwareData.LenProxy;
                FirmwareBlock.DatiOK = true;
                _esito = ExitCode.OK;
                return _esito;
            }
            catch (Exception Ex)
            {
                LastError = Ex.Message;
                return _esito;
            }
        }

        public ExitCode ComponiArrayTestata()
        {
            ExitCode _esito = ExitCode.ErroreGenerico;
            elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
            ushort _tempCRC;
            string _tempStr = "";

            byte msb = 0;
            byte lsb = 0;

            byte _byte01 = 0;
            byte _byte02 = 0;
            byte _byte03 = 0;
            byte _byte04 = 0;




            try
            {

                FirmwareBlock.TestataOK = false;

                if (!FirmwareBlock.DatiOK)
                {
                    _esito = ExitCode.DatiNonPronti;
                    return _esito;
                }

                
                byte _startByte = 0;
                for (int _i = 0; _i < 64; _i++)
                    FirmwareBlock.MessaggioTestata[_i] = 0xFF;

                // Versione FW
                _tempStr = FirmwareBlock.Release + "          ";  // in coda alla revisione aggiungo 8 spazi per completare eventuali stringhe incomplete
                for (int _i = 0; _i < 6; _i++)
                {
                    FirmwareBlock.MessaggioTestata[_startByte] = (byte)_tempStr[_i];
                    _startByte += 1;
                }
                _startByte = 5;
                // CRC
                DataSplitter.splitUshort(FirmwareBlock.crc, ref lsb, ref msb);
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = msb;
                FirmwareBlock.MessaggioTestata[(_startByte + 2)] = lsb;
                _startByte += 2;

                // Addr Flash 1
                DataSplitter.splitUint32(FirmwareBlock.AddrFlash1, ref _byte01, ref _byte02, ref _byte03, ref _byte04);
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = _byte01;
                FirmwareBlock.MessaggioTestata[(_startByte + 2)] = _byte02;
                FirmwareBlock.MessaggioTestata[(_startByte + 3)] = _byte03;
                FirmwareBlock.MessaggioTestata[(_startByte + 4)] = _byte04;
                _startByte += 4;

                // Len Flash 1
                DataSplitter.splitUint32(FirmwareBlock.LenFlash1, ref _byte01, ref _byte02, ref _byte03, ref _byte04);
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = _byte01;
                FirmwareBlock.MessaggioTestata[(_startByte + 2)] = _byte02;
                FirmwareBlock.MessaggioTestata[(_startByte + 3)] = _byte03;
                FirmwareBlock.MessaggioTestata[(_startByte + 4)] = _byte04;
                _startByte += 4;

                // Addr Flash 2
                DataSplitter.splitUint32(FirmwareBlock.AddrFlash2, ref _byte01, ref _byte02, ref _byte03, ref _byte04);
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = _byte01;
                FirmwareBlock.MessaggioTestata[(_startByte + 2)] = _byte02;
                FirmwareBlock.MessaggioTestata[(_startByte + 3)] = _byte03;
                FirmwareBlock.MessaggioTestata[(_startByte + 4)] = _byte04;
                _startByte += 4;

                // Len Flash 2
                DataSplitter.splitUint32(FirmwareBlock.LenFlash2, ref _byte01, ref _byte02, ref _byte03, ref _byte04);
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = _byte01;
                FirmwareBlock.MessaggioTestata[(_startByte + 2)] = _byte02;
                FirmwareBlock.MessaggioTestata[(_startByte + 3)] = _byte03;
                FirmwareBlock.MessaggioTestata[(_startByte + 4)] = _byte04;
                _startByte += 4;


                // Addr Proxy
                DataSplitter.splitUint32(FirmwareBlock.AddrProxy, ref _byte01, ref _byte02, ref _byte03, ref _byte04);
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = _byte01;
                FirmwareBlock.MessaggioTestata[(_startByte + 2)] = _byte02;
                FirmwareBlock.MessaggioTestata[(_startByte + 3)] = _byte03;
                FirmwareBlock.MessaggioTestata[(_startByte + 4)] = _byte04;
                _startByte += 4;

                // Len Proxy
                DataSplitter.splitUshort(FirmwareBlock.LenProxy, ref lsb, ref msb);
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = msb;
                FirmwareBlock.MessaggioTestata[(_startByte + 2)] = lsb;
                _startByte += 2;

                // data rilascio, in 3 bytes nel formato gg/mm/aa
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = FirmwareBlock.ReleaseDateBlock[0];
                FirmwareBlock.MessaggioTestata[(_startByte + 2)] = FirmwareBlock.ReleaseDateBlock[1];
                FirmwareBlock.MessaggioTestata[(_startByte + 3)] = FirmwareBlock.ReleaseDateBlock[2];
                _startByte += 3;

                // i bytes successsivi sono già inizializzati a 0xFF

                // Aggiungo il CRC calcolato sui primi 62 byte
                //if (_arrayLen>64) _arrayLen = 64;
                /// calcolo il crc
                //                byte[] _tempMessaggio = new byte[62];
                //                Array.Copy(FirmwareBlock.MessaggioTestata,0, _tempMessaggio, 0, 62);
                //                _tempCRC = codCrc.ComputeChecksum(_tempMessaggio);

                _tempCRC = codCrc.ComputePartialChecksum(FirmwareBlock.MessaggioTestata,62);

                //Ora salvo il CRC negli ultimi 2 bytes
                // CRC

                DataSplitter.splitUshort(_tempCRC, ref lsb, ref msb);
                FirmwareBlock.MessaggioTestata[62] = msb;
                FirmwareBlock.MessaggioTestata[63] = lsb;

                //TEST -- TEST -- TEST
                // Corrompo la testata cambiando un byte
                //FirmwareBlock.MessaggioTestata[60] = 0x00;
                //FirmwareBlock.MessaggioTestata[61] = 0x00;
                //TEST -- TEST -- TEST




                LastError = "OK";

                FirmwareBlock.TestataOK = true;
                _esito = ExitCode.OK;
                return _esito;
            }
            catch (Exception Ex)
            {
                LastError = Ex.Message;
                return _esito;
            }
        }

        public ExitCode AnalizzaArrayTestata(byte[] ArrayTestata )
        {
            ExitCode _esito = ExitCode.ErroreGenerico;
            try
            {
                if (ArrayTestata.Length >= 62)
                {
                    int _startPos = 0;
                    _esito = ExitCode.FormatoDatiErrato;
                    FirmwareBlock = new BloccoFirmware();
                    FirmwareBlock.AzzeraDati();

                    FirmwareBlock.Release = SerialMessage.ArrayToString(ArrayTestata, _startPos, 6);
                    _startPos += 6;
                    FirmwareBlock.crc = SerialMessage.ArrayToUshort(ArrayTestata, _startPos, 2);
                    _startPos += 2;
                    FirmwareBlock.AddrFlash1 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.LenFlash1 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.AddrFlash2 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.LenFlash2 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.AddrProxy = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.LenProxy = SerialMessage.ArrayToUshort(ArrayTestata, _startPos, 2);
                    _startPos += 2;
                    FirmwareBlock.ReleaseDateBlock[0] = ArrayTestata[_startPos];
                    _startPos += 1;
                    FirmwareBlock.ReleaseDateBlock[1] = ArrayTestata[_startPos];
                    _startPos += 1;
                    FirmwareBlock.ReleaseDateBlock[2] = ArrayTestata[_startPos];
                    _startPos += 1;

                    FirmwareBlock.ReleaseDate = FunzioniMR.StringaDataTS(FirmwareBlock.ReleaseDateBlock);


                    // Se i primi 6 bytes (versione APP) sono a FF o a 00 la testataa non è valida (vuota)
                    if ((ArrayTestata[0] == 0 && ArrayTestata[1] == 0 && ArrayTestata[2] == 0 && ArrayTestata[3] == 0 && ArrayTestata[4] == 0 && ArrayTestata[5] == 0 ) 
                        | (ArrayTestata[0] == 0xFF && ArrayTestata[1] == 0xFF && ArrayTestata[2] == 0xFF && ArrayTestata[3] == 0xFF && ArrayTestata[4] == 0xFF && ArrayTestata[5] == 0xFF))
                    {
                        // Testata non caricata
                        FirmwareBlock.TestataOK = false;
                    }
                    else
                    {
                        FirmwareBlock.TestataOK = true;
                    }

                    _esito = ExitCode.OK;

                    return _esito;
                }

                return _esito;
            }
            catch
            {
                return _esito;
            }

        }

        public bool VersioneAmmessa(string Versione, string HWversion = "4")
        {
            bool _esito = false;

            try
            {

                string _LocalVer = ""; 

                if (Versione == null) return false;
                if (Versione.Length >= 4) _LocalVer = Versione.Substring(0, 4);

                switch (_LocalVer)
                {
                    case "1.04":
                    case "1.05":
                    case "1.06":
                        // Livello  0;
                        return false;
                    // break;

                    case "1.07":
                        // Livello 1;
                        return false;
                    // break;

                    case "1.08":
                        if (Versione == "1.08.01")
                            // Livello 2;
                            return false;
                        else
                            // Livello 3;
                            return false;
                    // break;
                    case "1.09":
                        // Livello 3;
                        return false;

                    case "1.10":
                    case "1.11":

                        // Livello 4;
                        return true;
                    case "1.12":
                    case "1.13":
                        // Livello 4 non pubblico;
                        return true;

                    case "2.01":
                        // Livello 4;
                        return true;


                    default:
                        return false;
                        //  break;
                }

                return _esito;
            }
            catch
            {
                return _esito;
            }
        }
    }

    
    public class FileFirmware
    {
        public string Release { get; set; }
        public string ReleaseDate { get; set; }
        public byte[] ReleaseDateBlock { get; set; }
        public uint AddrFlash1 { get; set; }
        public uint LenFlash1 { get; set; }
        public uint AddrProxy { get; set; }
        public ushort LenProxy { get; set; }
        public uint AddrFlash2 { get; set; }
        public uint LenFlash2 { get; set; }
        public byte[] DataFlash1 { get; set; }
        public byte[] DataProxy { get; set; }
        public byte[] DataFlash2 { get; set; }
        public ushort crc { get; set; }
        public bool DatiOK { get; set; }

        public FileFirmware()
        {
            AzzeraDati();
        }

        public void AzzeraDati()
        {
            Release = "";
            ReleaseDateBlock = new byte[3];
            ReleaseDateBlock[0] = 1;
            ReleaseDateBlock[1] = 1;
            ReleaseDateBlock[2] = 15;
            AddrFlash1 = 0;
            LenFlash1 = 0;
            AddrProxy = 0;
            LenProxy = 0;
            AddrFlash2 = 0;
            LenFlash2 = 0;
            crc = 0;
            DataFlash1 = new byte[0];
            DataFlash2 = new byte[0];
            DataProxy = new byte[0];
            DatiOK = false;

        }

    }

    public class BloccoFirmware
    {
        public string Release { get; set; }
        public string ReleaseDate { get; set; }
        public byte[] ReleaseDateBlock { get; set; }
        public uint AddrFlash1 { get; set; }
        public uint LenFlash1 { get; set; }
        public uint AddrProxy { get; set; }
        public ushort LenProxy { get; set; }
        public uint AddrFlash2 { get; set; }
        public uint LenFlash2 { get; set; }
        public byte[] DataFlash1 { get; set; }
        public byte[] DataProxy { get; set; }
        public byte[] DataFlash2 { get; set; }


        public List<PacchettoDatiFW> ListaFlash1 = new List<PacchettoDatiFW>();
        public List<PacchettoDatiFW> ListaFlash2 = new List<PacchettoDatiFW>();
        public List<PacchettoDatiFW> ListaProxy = new List<PacchettoDatiFW>();

        public uint TotaleBlocchi { get; set; }

        public byte[] MessaggioTestata { get; set; }
        public bool TestataOK { get; set; }

        public ushort crc { get; set; }
        public bool DatiOK { get; set; }

        public BloccoFirmware()
        {
            AzzeraDati();
        }

        public void AzzeraDati()
        {
            Release = "";
            ReleaseDate = "";
            ReleaseDateBlock = new byte[3];
            ReleaseDateBlock[0] = 1;
            ReleaseDateBlock[1] = 1;
            ReleaseDateBlock[2] = 15;
            AddrFlash1 = 0;
            LenFlash1 = 0;
            AddrProxy = 0;
            LenProxy = 0;
            AddrFlash2 = 0;
            LenFlash2 = 0;
            crc = 0;
            DataFlash1 = new byte[0];
            DataFlash2 = new byte[0];
            DataProxy = new byte[0];
            MessaggioTestata = new byte[64];
            for (int _i = 0; _i < 64; _i++)
                MessaggioTestata[_i] = 0x00;
            TotaleBlocchi = 0;
            DatiOK = false;
            TestataOK = false;

        }

    }

    public class PacchettoDatiFW
    {
        public byte[] PacchettoDati { get; set; }
        public uint NumeroPacchetto { get; set; }
        public ushort DimPacchetto { get; set; }
        public ushort CRC { get; set; }
        public bool DatiOK { get; set; }


        public PacchettoDatiFW()
        {
            AzzeraDati();
        }

        public void AzzeraDati()
        {

            NumeroPacchetto = 0;
            DimPacchetto = 0;
            CRC = 0;
            PacchettoDati = new byte[0];
            DatiOK = false;

        }


    }

}
