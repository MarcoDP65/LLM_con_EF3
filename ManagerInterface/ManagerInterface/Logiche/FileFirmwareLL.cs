using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using log4net;
using log4net.Config;
using MoriData;
using Utility;

namespace ChargerLogic
{

    class FirmwareLLManager
    {
        public string TestoFirmwareCCS_hex { get; set; }
        public string TestoFirmwareCCS_a01 { get; set; }

        public FileFirmwareLL FirmwareData;

        public BloccoFirmwareLL FirmwareBlock;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public enum ExitCode : byte
        {
            OK = 0,
            FileAssente = 0x01, FileVuoto = 0x02, FormatoFileErrato = 0x03, FileA01Assente = 0x04, FileHEXAssente = 0x05,
            DatiNonPronti = 0x11, FormatoDatiErrato = 0x13, FilesNonAllineati = 0x14,NumeroBlocchiElevato = 0x15, NumeroBlytesElevato = 0x16,
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

        public const int BloccoTx = 64;

        public ExitCode CaricaFileCCS(string FileName)
        {
            string _fileHEXBuffer = "";
            string _fileA01Buffer = "";

            string _fileExt = "";
            string _fileA01 = "";
            string _fileHEX = "";


            ExitCode _esito = ExitCode.ErroreGenerico;

            try
            {
                // File non definito
                if (FileName == "") return ExitCode.FileAssente;


                if (File.Exists(FileName))
                {
                    // Verifico che esista anche il file associato
                    _fileExt = Path.GetExtension(FileName);


                    // se ho verificati la presenza dell' A01, controllo l'HEX
                    _fileA01 = Path.ChangeExtension(FileName, ".a01");
                    _fileHEX = Path.ChangeExtension(FileName, ".hex");

                    if (!File.Exists(_fileA01))
                    {
                        return ExitCode.FileA01Assente;
                    }

                    if (!File.Exists(_fileHEX))
                    {
                        return ExitCode.FileHEXAssente;
                    }


                    // Il file esistono conimcio a leggerli
                    FirmwareData = new FileFirmwareLL();
                    _fileHEXBuffer = File.ReadAllText(_fileHEX);
                    _fileA01Buffer = File.ReadAllText(_fileA01);

                    if(_fileHEXBuffer.Length != _fileA01Buffer.Length)
                    {

                    }

                    _esito = AnalizzaFileCCSLL(_fileHEXBuffer, _fileA01Buffer, ref FirmwareData);

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
        public ExitCode AnalizzaFileCCSLL(string DataFileHEX, string DataFileA01, ref FileFirmwareLL BloccoFirmware)
        {
            try
            {
                bool _eofFound = false;
                BloccoFirmware = new FileFirmwareLL();
                if ( DataFileHEX == "" || DataFileA01 == "" ) return ExitCode.FileAssente;

                string[] _blocchiFWA01 = DataFileA01.Split('$');
                string[] _blocchiFWHEX = DataFileHEX.Split('$');
                string _tempPtr;
                UInt32 _tempPtrNum;
                int numBlocchi;

                // ora suddivido i blocchi, identificati dall'indirizzo con prefisso $A
                // devo avere almeno 2 blocchi ( + 1 vuoto generato dalla funzione split ) e al massimo 5 per cxontenere la testata nei 64 bytes

                if (_blocchiFWA01.Length != _blocchiFWHEX.Length) return ExitCode.FormatoFileErrato;
                numBlocchi = _blocchiFWHEX.Length;

                if (numBlocchi < 3) return ExitCode.FormatoFileErrato;
                if (numBlocchi > 6) return ExitCode.NumeroBlocchiElevato;
                BloccoFirmware.ListaAree.Clear();

                // Il primo è vuoto, lo ignoro
                // ----------------------------------
                // dal secondo spacchetto i dati, spezzando sui terminatori \r\n   

                for (int _blAtuale = 1; _blAtuale < numBlocchi; _blAtuale++)
                {
                    string[] _blocchiFl_A01 = Regex.Split(_blocchiFWA01[_blAtuale], "\r\n");
                    string[] _blocchiFl_HEX = Regex.Split(_blocchiFWHEX[_blAtuale], "\r\n");


                    uint _tempUint;
                    uint _contaBytes = 0;
                    AreaDatiFWLL _AreaCorrente = new AreaDatiFWLL();
                    List<byte> _tempMem = new List<byte>();

                    _tempMem.Clear();
                    _contaBytes = 0;

                    bool _shortAddr;
                    //  I blocchi devono avere la stessa dimensione
                    if (_blocchiFl_A01.Length != _blocchiFl_HEX.Length)
                    {
                        return ExitCode.FormatoFileErrato;
                    }

                    // La prima riga del blocco contiene il marcatore $A3e8000,
                    if (_blocchiFl_A01[0] != _blocchiFl_HEX[0])
                    {
                        return ExitCode.FormatoFileErrato;
                    }

                    // il primo blocco può avere l'indirizzo a 2 o a 3 byte: se è a 2 a tutti gli indirizzi aggiungo in testa 3E se il resto è > 0x8000, 3F se <

                    _tempPtr = _blocchiFl_HEX[0];

                    if(_tempPtr.Length < 7)   // Oltre all'indirizzo puro, nella strnga vanno contate anche la "A" iniziale e la "," finale
                    {
                        _tempPtr = _tempPtr.Substring(1, 4);
                        _shortAddr = true;
                    }
                    else
                    {
                        _tempPtr = _tempPtr.Substring(1, 6);
                        _shortAddr = false;

                    }

                    //_tempPtr = _tempPtr.Substring(1, 6);
                    if (!UInt32.TryParse(_tempPtr, System.Globalization.NumberStyles.HexNumber, null, out _tempPtrNum))
                    {
                        return ExitCode.FormatoFileErrato;
                    }

                    if(_shortAddr)
                    {
                        if (_tempPtrNum < 0x8000)
                            _tempPtrNum += 0x3F0000;
                        else
                            _tempPtrNum += 0x3E0000;

                    }

                    _AreaCorrente.AddrDestPacchetto = _tempPtrNum;

                    for (int _righe = 1; _righe < _blocchiFl_A01.Length; _righe++)
                    {
                        string _contRigaA01 = _blocchiFl_A01[_righe];
                        string _contRigaHEX = _blocchiFl_HEX[_righe];

                        string[] _byteFWA01 = _contRigaA01.Split(' ');
                        string[] _byteFWHEX = _contRigaHEX.Split(' ');

                        int _numByte = _byteFWHEX.Length;

                        for (int _col = 0; _col < _numByte; _col++)
                        {
                            byte _tempByte;
                            string _singleByte;
                            _singleByte = _byteFWA01[_col];
                            if (byte.TryParse(_singleByte, System.Globalization.NumberStyles.HexNumber, null, out _tempByte))
                            {
                                _contaBytes++;
                                _tempMem.Add(_tempByte);
                            }
                            _singleByte = _byteFWHEX[_col];
                            if (byte.TryParse(_singleByte, System.Globalization.NumberStyles.HexNumber, null, out _tempByte))
                            {
                                _contaBytes++;
                                _tempMem.Add(_tempByte);
                            }

                        }


                    }
                    _AreaCorrente.PacchettoDati = new byte[_contaBytes];
                    _AreaCorrente.DimPacchetto = _contaBytes;

                    for (int _i = 0; _i < _contaBytes; _i++)
                    {
                        _AreaCorrente.PacchettoDati[_i] = _tempMem.ElementAt(_i);
                    }



                    BloccoFirmware.ListaAree.Add(_AreaCorrente);
                }

                BloccoFirmware.DatiOK = true;
                return ExitCode.OK;
                

            }
            catch
            {
                return ExitCode.ErroreGenerico;
            }
        }

        public ExitCode GeneraFileLLF(string VersioneFw, string VersioneDisplay, string DataRilascio, string NomeFile, bool CifraFile = true)
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
                FirmwareData.DisplayRelease = VersioneDisplay;
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

        public ExitCode CaricaFileLLF(string NomeFile, bool CifraFile = true)
        {
            ExitCode _esito = ExitCode.ErroreGenerico;
            LastError = "";
            try
            {
                FirmwareData = new FileFirmwareLL();
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

                    FileFirmwareLL _tmpFirmwareData;
                    elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
                    ushort _crc;


                    _tmpFirmwareData = JsonConvert.DeserializeObject<FileFirmwareLL>(_fileImport);
                    ushort _tempCRC = _tmpFirmwareData.crc;
                    _tmpFirmwareData.crc = 0;

                    string _tempSer = JsonConvert.SerializeObject(_tmpFirmwareData);
                    byte[] _tempBSer = FunzioniMR.GetBytes(_tempSer);

                    // rivedere il controllo crc
                    _crc = codCrc.ComputeChecksum(_tempBSer);

                    if (true)//_crc == _tempCRC)
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
            PacchettoDatiFWLL _blocco;

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
                FirmwareBlock = new BloccoFirmwareLL();

                // Carico i dati di testata
                FirmwareBlock.Release = FirmwareData.Release;
                FirmwareBlock.ReleaseDate = FirmwareData.ReleaseDate;

                /* RIPRISTNARE LA VERIFICA VERSIONE, per ora controllo dolo di avere 6 caratteri di app e 6 di display
                if ((FirmwareData.Release.Substring(0, 2) != "1.") && (FirmwareData.Release.Substring(0, 2) != "2."))
                {
                    _esito = ExitCode.ErroreGenerico;
                    return _esito;
                }
                */


                byte[] _tempBuffer = new byte[131072];
                int _currByte = 0;
                int _lenDati = 0;

                //FirmwareBlock.DataFlash = FirmwareData.DataFlash;
                //FirmwareBlock.LenFlash = FirmwareData.LenFlash;

                FirmwareBlock.AddrStartApp = FirmwareData.AddrStartApp;
                
                FirmwareBlock.TotaleBlocchi = 0;
                for (int _ii = 0; _ii < 3; _ii++)
                {
                    FirmwareBlock.ReleaseDateBlock[_ii] = FirmwareData.ReleaseDateBlock[_ii];
                }

                FirmwareBlock.ListaAree = FirmwareData.ListaAree;
                _esito = ExitCode.FormatoDatiErrato;
                // Calcolo il CRC Totale
                FirmwareBlock.crc = codCrc.ComputeChecksum(FirmwareBlock.DataFlash);

                //-----------------------------------------------------------
                //  FLASH 
                //-----------------------------------------------------------
                // Impacchetto i settori Flash 
                //FirmwareBlock.ListaFlash.Clear();

                foreach (AreaDatiFWLL _dati in FirmwareBlock.ListaAree)
                {

                    _blockSize = 0;
                    _blockDiv = _dati.DimDati / BloccoTx;
                    _blockMod = (ushort)(_dati.DimDati % BloccoTx);
                    _dati.ListaPacchetti.Clear();
                    // Prima i blocchi completi
                    for (int _block = 0; _block < _blockDiv; _block++)
                    {
                        _blocco = new PacchettoDatiFWLL();
                        _blocco.DimPacchetto = BloccoTx + 2;
                        _blocco.NumeroPacchetto = (uint)_block;

                        _blocco.PacchettoDati = new byte[BloccoTx];
                        Array.Copy(_dati.PacchettoDati, (_block * BloccoTx), _blocco.PacchettoDati, 0, BloccoTx);
                        _blocco.CRC = codCrc.ComputeChecksum(_blocco.PacchettoDati);
                        _blocco.DatiOK = true;
                        _dati.ListaPacchetti.Add(_blocco);
                        _blockSize += _blocco.DimPacchetto;
                        FirmwareBlock.TotaleBlocchi += 1;

                        // Ora accodo i dati al buffer SENZA CRC di PACCHETTO per il calcolo del CRC generale da passsare in testata e la lunghezza complessiva
                        // compresi i CRC di pacchetto. Questa dimensione può al massimo essere pari a 128K
                        Array.Copy(_dati.PacchettoDati, (_block * BloccoTx), _tempBuffer, _currByte, BloccoTx);
                        _currByte += BloccoTx;
                        _lenDati += _blocco.DimPacchetto ;
                        if (_lenDati > 131072)
                        {
                            _esito = ExitCode.NumeroBlytesElevato;
                            return _esito;
                        }


                    }

                    // poi quello finale se resto > 0
                    if (_blockMod > 0)
                    {
                        _blocco = new PacchettoDatiFWLL();
                        _blocco.DimPacchetto = (ushort)(2 + _blockMod);
                        _blocco.NumeroPacchetto = (uint)_blockDiv;
                        _blocco.PacchettoDati = new byte[_blockMod];
                        Array.Copy(_dati.PacchettoDati, (_blockDiv * BloccoTx), _blocco.PacchettoDati, 0, _blockMod);
                        _blocco.CRC = codCrc.ComputePartialChecksum(_blocco.PacchettoDati, _blockMod);
                        _blocco.DatiOK = true;
                        _dati.ListaPacchetti.Add(_blocco);
                        _blockSize += _blocco.DimPacchetto;
                        FirmwareBlock.TotaleBlocchi += 1;
                        // Ora accodo i dati al buffer SENZA CRC di PACCHETTO per il calcolo del CRC generale da passsare in testata e la lunghezza complessiva
                        // compresi i CRC di pacchetto. Questa dimensione può al massimo essere pari a 128K
                        Array.Copy(_dati.PacchettoDati, (_blockDiv * BloccoTx), _tempBuffer, _currByte, _blockMod);
                        _currByte += _blockMod;
                        _lenDati += _blocco.DimPacchetto;
                        if (_lenDati > 131072)
                        {
                            _esito = ExitCode.NumeroBlytesElevato;
                            return _esito;
                        }
                    }




                }
                // Ora calcolo il CRC globale da inserire in testata

                FirmwareBlock.crc = codCrc.ComputePartialChecksum(_tempBuffer, _currByte);
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
                    FirmwareBlock.MessaggioTestata[_i] = 0x00;

                // Versione FW
                _tempStr = FirmwareBlock.Release + "          ";  // in coda alla revisione aggiungo 8 spazi per completare eventuali stringhe incomplete
                for (int _i = 0; _i < 6; _i++)
                {
                    FirmwareBlock.MessaggioTestata[_startByte] = (byte)_tempStr[_i];
                    _startByte += 1;
                }
                _startByte = 6;
                // Versione FW disp
                _tempStr = FirmwareBlock.Release + "          ";  // in coda alla revisione aggiungo 8 spazi per completare eventuali stringhe incomplete
                for (int _i = 0; _i < 6; _i++)
                {
                    FirmwareBlock.MessaggioTestata[_startByte] = (byte)_tempStr[_i];
                    _startByte += 1;
                }
                _startByte = 12;

                // dimensione pacchetto
                FirmwareBlock.MessaggioTestata[_startByte] = BloccoTx;
                _startByte += 1;

                // Numero pacchetti
                FirmwareBlock.MessaggioTestata[_startByte] = (byte)FirmwareBlock.ListaAree.Count();
                _startByte += 1;

                // CRC
                DataSplitter.splitUshort(FirmwareBlock.crc, ref lsb, ref msb);
                FirmwareBlock.MessaggioTestata[(_startByte + 0)] = msb;
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = lsb;
                _startByte += 2;

                // ora i pacchetti dati

                foreach(AreaDatiFWLL _area in FirmwareBlock.ListaAree)
                {
                    // Start area
                    DataSplitter.splitUint32(_area.AddrDestPacchetto, ref _byte01, ref _byte02, ref _byte03, ref _byte04);

                    FirmwareBlock.MessaggioTestata[(_startByte + 0)] = _byte01;
                    FirmwareBlock.MessaggioTestata[(_startByte + 1)] = _byte02;
                    FirmwareBlock.MessaggioTestata[(_startByte + 2)] = _byte03;
                    FirmwareBlock.MessaggioTestata[(_startByte + 3)] = _byte04;
                    _startByte += 4;

                    // Len Area 
                    DataSplitter.splitUint32(_area.DimDati, ref _byte01, ref _byte02, ref _byte03, ref _byte04);
                    FirmwareBlock.MessaggioTestata[(_startByte + 0)] = _byte01;
                    FirmwareBlock.MessaggioTestata[(_startByte + 1)] = _byte02;
                    FirmwareBlock.MessaggioTestata[(_startByte + 2)] = _byte03;
                    FirmwareBlock.MessaggioTestata[(_startByte + 3)] = _byte04;
                    _startByte += 4;

                }

 
                // i bytes successivi sono già inizializzati a 0xFF

                // Aggiungo il CRC calcolato sui primi 62 byte

                _tempCRC = codCrc.ComputePartialChecksum(FirmwareBlock.MessaggioTestata, 62);

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

                Log.Debug(FunzioniComuni.HexdumpArray(FirmwareBlock.MessaggioTestata,true));

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

        public ExitCode AnalizzaArrayTestata(byte[] ArrayTestata)
        {
            ExitCode _esito = ExitCode.ErroreGenerico;
            try
            {
                if (ArrayTestata.Length >= 62)
                {
                    int _startPos = 0;
                    _esito = ExitCode.FormatoDatiErrato;
                    FirmwareBlock = new BloccoFirmwareLL();
                    FirmwareBlock.AzzeraDati();

                    FirmwareBlock.Release = SerialMessage.ArrayToString(ArrayTestata, _startPos, 6);
                    _startPos += 6;
                    FirmwareBlock.ReleaseDisplay = SerialMessage.ArrayToString(ArrayTestata, _startPos, 6);
                    _startPos += 6;

                    FirmwareBlock.LenPkt = ArrayTestata[_startPos];
                    _startPos += 1;
                    FirmwareBlock.NumSezioni = ArrayTestata[_startPos];
                    _startPos += 1;
                    FirmwareBlock.CrcFW = SerialMessage.ArrayToShort(ArrayTestata, _startPos, 2);
                    _startPos += 2;

                    FirmwareBlock.AddrSez1 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.LenSez1 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.AddrSez2 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.LenSez2 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.AddrSez3 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.LenSez3 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.AddrSez4 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.LenSez4 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.AddrSez5 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.LenSez5 = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    // Vuoto
                    _startPos += 6;

                    // Crc record testata ( primi 62 bytes)
                    FirmwareBlock.CrcTest = SerialMessage.ArrayToShort(ArrayTestata, _startPos, 2);
                    _startPos += 2;


                    // Se i primi 6 bytes (versione APP) sono a FF o a 00 la testata non è valida (vuota)
                    if ((ArrayTestata[0] == 0 && ArrayTestata[1] == 0 && ArrayTestata[2] == 0 && ArrayTestata[3] == 0 && ArrayTestata[4] == 0 && ArrayTestata[5] == 0)
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

        /// <summary>
        /// Verifico se la versione selezionata è installabile sullo spybatt corrente
        /// </summary>
        /// <param name="Versione">Versione del FW da caricare</param>
        /// <param name="HWversion">Versione dell'hardware corrente</param>
        /// <param name="BootLoader">Versione del bootloader corrente</param>
        /// <returns></returns>
        public bool VersioneAmmessa(string Versione, string HWversion = "4", string BootLoader = "1.01.09")
        {
            bool _esito = false;

            try
            {

                return true;

                string _LocalVer = "";
                string _blVersion = "";
                int _blv = 0;

                if (Versione == null) return false;
                if (Versione.Length >= 4) _LocalVer = Versione.Substring(0, 4);


                if (BootLoader == null) return false;
                if (BootLoader.Length >= 4) _blVersion = BootLoader.Substring(0, 4);



                switch (_blVersion)
                {
                    case "1.01":
                        _blv = 1;
                        break;
                    case "1.02":
                        _blv = 2;
                        break;
                    default:
                        _blv = 0;
                        //se non ho un bootloader noto non continuo in ogni caso.
                        return false;
                }



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
                        // Valido con bootloader 1.01.xx
                        return (_blv == 1);
                    case "1.12":
                    case "1.13":
                        // Livello 4 non pubblico;
                        // Valido con bootloader 1.01.xx
                        return (_blv == 1);
                    // return true;

                    case "2.01":
                    case "2.02":
                        // Livello 4;
                        // Valido con bootloader 1.02.xx
                        return (_blv == 2);
                    //return true;

                    case "2.03":
                        // Livello 6/7;
                        // Valido con bootloader 1.02.xx
                        return (_blv == 2);
                    //return true;
                    case "2.04":
                        // Livello 6/7;
                        // Valido con bootloader 1.02.xx
                        return (_blv == 2);
                    //return true;

                    default:
                        return false;

                }

            }
            catch
            {
                return _esito;
            }
        }

    }

    public class FileFirmwareLL
    {
        public string Release { get; set; }
        public string DisplayRelease { get; set; }
        public string ReleaseDate { get; set; }
        public byte[] ReleaseDateBlock { get; set; }
        public uint LenFlash { get; set; }
        public UInt32 AddrStartAppPtr { get; set; }
        public UInt32 AddrStartApp { get; set; }
        public byte[] DataFlash { get; set; }
        public UInt32 DataFlashPtr { get; set; }

        public List<AreaDatiFWLL> ListaAree = new List<AreaDatiFWLL>();

        public ushort crc { get; set; }
        public bool DatiOK { get; set; }

        public FileFirmwareLL()
        {
            AzzeraDati();
        }

        public void AzzeraDati()
        {
            Release = "";
            DisplayRelease = "";
            ReleaseDateBlock = new byte[3];
            ReleaseDateBlock[0] = 1;
            ReleaseDateBlock[1] = 1;
            ReleaseDateBlock[2] = 17;
            LenFlash = 0;
            AddrStartApp = 0;
            crc = 0;
            DataFlash = new byte[0];
            ListaAree.Clear();
            DatiOK = false;

            crc = 0;
            DataFlash = new byte[0];

        }

    }

    public class BloccoFirmwareLL
    {
        public string Release { get; set; }
        public byte[] ReleaseData { get; set; }
   
        public string ReleaseDisplay { get; set; }
        public byte[] ReleaseDispData { get; set; }

        public string ReleaseDate { get; set; }
        public byte[] ReleaseDateBlock { get; set; }

        public int LenPkt { get; set; }   // Lunghezza singolo pacchetto / messaggio. variabile in funzione USB/RS485/BLE
        public int NumSezioni { get; set; }

        public ulong AddrSez1 { get; set; }
        public ulong AddrSez2 { get; set; }
        public ulong AddrSez3 { get; set; }
        public ulong AddrSez4 { get; set; }
        public ulong AddrSez5 { get; set; }

        public ulong LenSez1 { get; set; }
        public ulong LenSez2 { get; set; }
        public ulong LenSez3 { get; set; }
        public ulong LenSez4 { get; set; }
        public ulong LenSez5 { get; set; }

        public short CrcFW { get; set; }
        public short CrcTest { get; set; }


        public uint LenFlash { get; set; }
        public UInt32 AddrStartAppPtr { get; set; }
        public UInt32 AddrStartApp { get; set; }
        public byte[] DataAddraApp { get; set; }
        public byte[] DataFlash { get; set; }
        public UInt32 DataFlashPtr { get; set; }


        public List<AreaDatiFWLL> ListaAree = new List<AreaDatiFWLL>();


        public uint TotaleBlocchi { get; set; }

        public byte[] MessaggioTestata { get; set; }
        public bool TestataOK { get; set; }

        public ushort crc { get; set; }
        public bool DatiOK { get; set; }

        public BloccoFirmwareLL()
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

            AddrSez1 = 0;
            AddrSez2 = 0;
            AddrSez3 = 0;
            AddrSez4 = 0;
            AddrSez5 = 0;

            LenSez1 = 0;
            LenSez2 = 0;
            LenSez3 = 0;
            LenSez4 = 0;
            LenSez5 = 0;

            CrcFW = 0;
            CrcTest = 0;

            NumSezioni = 0;

            MessaggioTestata = new byte[64];
            for (int _i = 0; _i < 64; _i++)
                MessaggioTestata[_i] = 0x00;
            TotaleBlocchi = 0;
            crc = 0;
            DataFlash = new byte[0];
            DatiOK = false;
            TestataOK = false;

        }

    }

    public class PacchettoDatiFWLL
    {
        public byte[] PacchettoDati { get; set; }
        public uint NumeroPacchetto { get; set; }
        public ushort DimPacchetto { get; set; }
        public ushort CRC { get; set; }
        public bool DatiOK { get; set; }


        public PacchettoDatiFWLL()
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


    public class AreaDatiFWLL
    {
        public byte[] PacchettoDati { get; set; }
        public uint DimPacchetto { get; set; }
        public UInt32 AddrDestPacchetto { get; set; }
        public bool DatiOK { get; set; }
        public List<PacchettoDatiFWLL> ListaPacchetti = new List<PacchettoDatiFWLL>();

        public AreaDatiFWLL()
        {
            AzzeraDati();
        }

        public void AzzeraDati()
        {
            ListaPacchetti.Clear();
            DimPacchetto = 0;
            DatiOK = false;

        }

        public uint NumeroPacchetti
        {
            get
            {
                return (uint)ListaPacchetti.Count;
            }
        }

        public uint DimDati
        {
            get
            {
                return (uint)PacchettoDati.Length;
            }
        }

    }


}
