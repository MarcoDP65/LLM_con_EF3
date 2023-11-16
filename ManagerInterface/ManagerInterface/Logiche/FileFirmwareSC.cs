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
using System.DirectoryServices.ActiveDirectory;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static ChargerLogic.SerialMessage;

namespace ChargerLogic
{

    class FirmwareSCManager
    {
        public string TestoFirmwareCCS_hex { get; set; }
        public string TestoFirmwareCCS_a01 { get; set; }

        public FileFirmwareSC FirmwareData; // { get; set; }
        // Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

        public BloccoFirmwareSC FirmwareBlock { get; set; }
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

        // Base, collaudato, 64. Test base 240
        public const int BLOCCO_TX = 64;

        public ExitCode CaricaFileSRec(string FileName)
        {
            string _fileHEXBuffer = "";

            string _fileHEX = "";


            ExitCode _esito = ExitCode.ErroreGenerico;

            try
            {
                // File non definito
                if (FileName == "") return ExitCode.FileAssente;


                if (File.Exists(FileName))
                {


                    // Il file esistono conimcio a leggerli
                    FirmwareData = new FileFirmwareSC();

                    _esito = AnalizzaFileSRec(FileName, ref FirmwareData);

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
        public ExitCode AnalizzaFileSRec(string DataFile, ref FileFirmwareSC BloccoFirmware)
        {
            try
            {
                bool _eofFound = false;
                BloccoFirmware = new FileFirmwareSC();

                string _tempPtr;
                UInt32 _tempPtrNum;
                int numBlocchi;

                // Comincio a scorrere il file  .....

                // Read a text file line by line.
                string[] lines = File.ReadAllLines(DataFile);
                string TipoRiga = "";
                int TipoDati = 0;
                string StrLunghezza = "";
                int LunghezzaMsg = 0;
                byte[] RowData;
                byte[] BufferDati = new byte[4194304];
                int BufferSize = 0;
                int BufferPoint = 0;
                UInt32 BuffAddrDestPacchetto = 0;
                UInt32 BuffAddrFinePacchetto = 0;

                byte Datalen;
                uint StartAddr;
                int FirstByte = 0;

                AreaDatiFWSC AreaCorrente = new AreaDatiFWSC(); 


                foreach (string line in lines)
                {
                    TipoRiga = line.Substring(0,2);
                    StrLunghezza = line.Substring(2,2);
                    RowData = FunzioniComuni.HexStringToArray(line.Substring(2), (line.Length -2)/2);
                    StartAddr = 0;
                    FirstByte = 0;
                    TipoDati = 0;
                    Datalen = 0;

                    if (!FunzioniComuni.CheckSrecCrc(RowData))
                        return ExitCode.FormatoDatiErrato;

                    switch (TipoRiga) 
                    {
                        case "S0":
                            {
                                // Intestazione
                                TipoDati = 1;
                                break;
                            }
                        case "S1":
                            {
                                // Sequenza dati - Indirizzo 16 bit
                                Datalen = RowData[0];
                                StartAddr = FunzioniComuni.ArrayToUInt(RowData, 1, 2);
                                FirstByte = 3;
                                TipoDati = 2;
                                break;
                            }
                        case "S2":
                            {
                                // Sequenza dati - Indirizzo 16 bit
                                Datalen = RowData[0];
                                StartAddr = FunzioniComuni.ArrayToUInt(RowData, 1, 3);
                                FirstByte = 4;
                                TipoDati = 2;
                                break;
                            }
                        case "S3":
                            {
                                // Sequenza dati - Indirizzo 16 bit
                                Datalen = RowData[0];
                                StartAddr = FunzioniComuni.ArrayToUInt(RowData, 1, 4);
                                FirstByte = 5;
                                TipoDati = 2;
                                break;
                            }
                        default:
                            {
                                TipoDati = 0;
                                break;
                            }
                    }


                    if (TipoDati == 2)  // Pacchetto dati
                    {
                        int LenEff = Datalen - (FirstByte);
                        if (BufferSize == 0 )   //AreaCorrente.AddrDestPacchetto == 0 && AreaCorrente.DimDati == 0)
                        {
                            // Nuova area, accodo brutalmente

                            BuffAddrDestPacchetto = StartAddr;
                            BuffAddrFinePacchetto = StartAddr + (uint)LenEff;
                            for ( int _pos = 0; _pos < LenEff; _pos ++)
                            {
                                BufferDati[BufferPoint] = RowData[FirstByte + _pos];
                                BufferPoint++;
                            }
                            BufferSize = BufferPoint;

                            // AreaCorrente.PacchettoDati = FunzioniComuni.ArrayAppend(AreaCorrente.PacchettoDati, RowData, FirstByte, LenEff + 1);
                        }
                        else
                        {
                            //Verifico se l'indirizzo è consecutivo al pacchetto precedente, accodo, altrimenti inizio una nuova area
                            if (BuffAddrFinePacchetto == StartAddr)
                            {
                                // Consecutivo
                                BuffAddrFinePacchetto += (uint)LenEff;
                                for (int _pos = 0; _pos < LenEff; _pos++)
                                {
                                    BufferDati[BufferPoint] = RowData[FirstByte + _pos];
                                    BufferPoint++;
                                }
                                BufferSize = BufferPoint;

                                //AreaCorrente.AddrFinePacchetto += (uint)LenEff+1;
                                //AreaCorrente.PacchettoDati = FunzioniComuni.ArrayAppend(AreaCorrente.PacchettoDati, RowData, FirstByte, LenEff + 1);
                            }
                            else
                            {
                                // Cambia indirizzo: salvo il blocco e ne creo uno nuovo
                                AreaCorrente.PacchettoDati = new byte[BufferSize];

                                AreaCorrente.AddrDestPacchetto = BuffAddrDestPacchetto;
                                AreaCorrente.AddrFinePacchetto = BuffAddrFinePacchetto;
                                for (int _pos = 0; _pos < BufferSize; _pos++)
                                {
                                    AreaCorrente.PacchettoDati[_pos] = BufferDati[_pos];
                                }
                                // Non consecutivi, Nuova area, accodo brutalmente
                                BloccoFirmware.ListaAree.Add(AreaCorrente);

                                // Ora vuoto i buffer e riparto
                                BufferDati = new byte[4194304];
                                BufferSize = 0;
                                BufferPoint = 0;

                                BuffAddrDestPacchetto = StartAddr;
                                BuffAddrFinePacchetto = StartAddr + (uint)LenEff;
                                for (int _pos = 0; _pos < LenEff; _pos++)
                                {
                                    BufferDati[BufferPoint] = RowData[FirstByte + _pos];
                                    BufferPoint++;
                                }
                                BufferSize = BufferPoint;

                                AreaCorrente = new AreaDatiFWSC();
                                

                            }

                        }
                    }

                }
                // alla fine, se contiene qualcosa, accodo l'ultima area
                if (BufferSize > 0)
                {
                    AreaCorrente.PacchettoDati = new byte[BufferSize];

                    AreaCorrente.AddrDestPacchetto = BuffAddrDestPacchetto;
                    AreaCorrente.AddrFinePacchetto = BuffAddrFinePacchetto;
                    for (int _pos = 0; _pos < BufferSize; _pos++)
                    {
                        AreaCorrente.PacchettoDati[_pos] = BufferDati[_pos];
                    }
                    
                    BloccoFirmware.ListaAree.Add(AreaCorrente);
                }


                // BloccoFirmware.ListaAree.Add(_AreaCorrente);
                // Blocchi dati elaborati, Ora accodo tutto e calcolo dimensioni e crc globale
                // 16/11/23 -  Calcolo anche il CSC di area
                Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);
                uint DimTotale = 0;
                foreach( AreaDatiFWSC Area in  BloccoFirmware.ListaAree ) 
                {
                    DimTotale += Area.DimDati;
                    codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);
                    Area.CRCPacchetto = codCrc.ComputeChecksum(Area.PacchettoDati);
                }
                BloccoFirmware.LenFlash = DimTotale;
                // Creo l'array generale
                byte[] TempImmagine = new byte[DimTotale];
                uint PosCorrente = 0;
                foreach (AreaDatiFWSC Area in BloccoFirmware.ListaAree)
                {
                    for ( uint Pos = 0; Pos < Area.DimDati;Pos++)
                    {
                        TempImmagine[PosCorrente++] = Area.PacchettoDati[Pos];  
                    }
                }

                codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);
                BloccoFirmware.CrcFlash = codCrc.ComputeChecksum(TempImmagine);

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

        public ExitCode GeneraFileLLSSCF(string VersioneFw,  string DataRilascio, string NomeFile, bool CifraFile = true)
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

        // CaricafileLLSCF

        public ExitCode CaricafileLLSCF(string NomeFile, bool CifraFile = true)
        {
            ExitCode _esito = ExitCode.ErroreGenerico;
            LastError = "";
            try
            {
                FirmwareData = new FileFirmwareSC();
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

                    FileFirmwareSC _tmpFirmwareData;
                    elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
                    ushort _crc;


                    _tmpFirmwareData = JsonConvert.DeserializeObject<FileFirmwareSC>(_fileImport);
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

        /// <summary>
        /// Preapro la testata in funzione del file Esadecomale ottenuto dallo SRec
        /// </summary>
        /// <returns></returns>
        public ExitCode PreparaUpgradeFw(UInt32 StepSize)
        {
            ExitCode _esito = ExitCode.ErroreGenerico;
            elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
            PacchettoDatiFWSC _blocco;

            LastError = "";

            uint _blockDiv;
            ushort _blockMod;

            uint _blockSize = 0;
            uint BloccoTx = BLOCCO_TX;

            try
            {

                if (!FirmwareData.DatiOK)
                {
                    _esito = ExitCode.DatiNonPronti;
                    return _esito;
                }


                BloccoTx = StepSize;

                // Preparo la classe destinazione
                FirmwareBlock = new BloccoFirmwareSC();

                // Carico i dati di testata
                FirmwareBlock.Release = FirmwareData.Release;
                FirmwareBlock.ReleaseDate = FirmwareData.ReleaseDate;
                FirmwareBlock.CrcFW = FirmwareData.CrcFlash;
                FirmwareBlock.LenFlash = FirmwareData.LenFlash;

                /* RIPRISTNARE LA VERIFICA VERSIONE, per ora controllo dolo di avere 6 caratteri di app e 6 di display
                if ((FirmwareData.Release.Substring(0, 2) != "1.") && (FirmwareData.Release.Substring(0, 2) != "2."))
                {
                    _esito = ExitCode.ErroreGenerico;
                    return _esito;
                }
                */

                // Preparo un buffer da 4MB, pari all'intera area disponibile
                byte[] _tempBuffer = new byte[4194304];
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


                foreach (AreaDatiFWSC _dati in FirmwareBlock.ListaAree)
                {

                    _blockSize = 0;
                    _blockDiv = _dati.DimDati / BloccoTx;
                    _blockMod = (ushort)(_dati.DimDati % BloccoTx);
                    _dati.ListaPacchetti.Clear();
                    // Prima i blocchi completi
                    for (int _block = 0; _block < _blockDiv; _block++)
                    {
                        _blocco = new PacchettoDatiFWSC();
                        _blocco.DimPacchetto = (ushort)(BloccoTx + 2);
                        _blocco.NumeroPacchetto = (uint)_block;

                        _blocco.PacchettoDati = new byte[BloccoTx];
                        Array.Copy(_dati.PacchettoDati, (_block * BloccoTx), _blocco.PacchettoDati, 0, BloccoTx);
                        _blocco.CRC = codCrc.ComputeChecksum(_blocco.PacchettoDati);
                        _blocco.DatiOK = true;
                        _dati.ListaPacchetti.Add(_blocco);
                        _blockSize += _blocco.DimPacchetto;
                        FirmwareBlock.TotaleBlocchi += 1;

                        // Ora accodo i dati al buffer SENZA CRC di PACCHETTO per il calcolo del CRC generale da passare in testata e la lunghezza complessiva
                        // compresi i CRC di pacchetto. Questa dimensione può al massimo essere pari a 4MB
                        Array.Copy(_dati.PacchettoDati, (_block * BloccoTx), _tempBuffer, _currByte, BloccoTx);
                        _currByte += (int)BloccoTx;
                        _lenDati += _blocco.DimPacchetto ;
                        if (_lenDati > 4194304)
                        {
                            _esito = ExitCode.NumeroBlytesElevato;
                            return _esito;
                        }


                    }

                    // poi quello finale se resto > 0
                    if (_blockMod > 0)
                    {
                        _blocco = new PacchettoDatiFWSC();
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
                        // compresi i CRC di pacchetto. Questa dimensione può al massimo essere pari a 4MB
                        Array.Copy(_dati.PacchettoDati, (_blockDiv * BloccoTx), _tempBuffer, _currByte, _blockMod);
                        _currByte += _blockMod;
                        _lenDati += _blocco.DimPacchetto;
                        if (_lenDati > 4194304)
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

        public ExitCode ComponiArrayTestata(UInt32 StepSize)
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
                int ArrayLen = 23 + 8 * FirmwareBlock.ListaAree.Count;
                int CrcLen = ArrayLen - 2;
                FirmwareBlock.MessaggioTestata = new byte[ArrayLen];

                for (int _i = 0; _i < ArrayLen; _i++)
                    FirmwareBlock.MessaggioTestata[_i] = 0x00;


                // Lunghezza pacchetti in memoria
                // 24/10 - Lunghezza 4 bytes, uint32
                FunzioniComuni.SplitUint32((UInt32)StepSize, ref _byte01, ref _byte02, ref _byte03, ref _byte04);
                FirmwareBlock.MessaggioTestata[(_startByte + 0)] = _byte01;
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = _byte02;
                FirmwareBlock.MessaggioTestata[(_startByte + 2)] = _byte03;
                FirmwareBlock.MessaggioTestata[(_startByte + 3)] = _byte04;
                _startByte += 4; ;

                // Versione FW
                _tempStr = FirmwareBlock.Release + "          ";  // in coda alla revisione aggiungo 8 spazi per completare eventuali stringhe incomplete
                for (int _i = 0; _i < 10; _i++)
                {
                    FirmwareBlock.MessaggioTestata[_startByte] = (byte)_tempStr[_i];
                    _startByte += 1;
                }
                // Lunghezza FW
                DataSplitter.splitUint32(FirmwareBlock.LenFlash, ref _byte01, ref _byte02, ref _byte03, ref _byte04);
                FirmwareBlock.MessaggioTestata[(_startByte + 0)] = _byte01;
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = _byte02;
                FirmwareBlock.MessaggioTestata[(_startByte + 2)] = _byte03;
                FirmwareBlock.MessaggioTestata[(_startByte + 3)] = _byte04;
                _startByte += 4;

                // Numero aree
                FirmwareBlock.MessaggioTestata[_startByte++] = (byte)FirmwareBlock.ListaAree.Count();

                // CRC
                DataSplitter.splitUshort(FirmwareBlock.CrcFW, ref lsb, ref msb);
                FirmwareBlock.MessaggioTestata[(_startByte + 0)] = msb;
                FirmwareBlock.MessaggioTestata[(_startByte + 1)] = lsb;
                _startByte += 2;

                // ora i pacchetti dati

                foreach (AreaDatiFWSC _area in FirmwareBlock.ListaAree)
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

                    // CRC Area 
                    DataSplitter.splitUshort(_area.CRCPacchetto, ref lsb, ref msb);
                    FirmwareBlock.MessaggioTestata[(_startByte + 0)] = msb;
                    FirmwareBlock.MessaggioTestata[(_startByte + 1)] = lsb;
                    _startByte += 2;

                }

                // Aggiungo il CRC calcolato sui bytes utili 

                byte[] CrcArea = new byte[CrcLen];
                for (int _i = 0; _i < CrcLen; _i++)
                {
                    CrcArea[_i] = FirmwareBlock.MessaggioTestata[_i];
                }


                _tempCRC = codCrc.ComputeChecksum(CrcArea);  // ComputePartialChecksum(FirmwareBlock.MessaggioTestata, _startByte);
                DataSplitter.splitUshort(_tempCRC, ref lsb, ref msb);
                FirmwareBlock.MessaggioTestata[_startByte++] = msb;
                FirmwareBlock.MessaggioTestata[_startByte++] = lsb;

                //TEST -- TEST -- TEST
                // Corrompo la testata cambiando un byte
                //FirmwareBlock.MessaggioTestata[60] = 0x00;
                //FirmwareBlock.MessaggioTestata[61] = 0x00;
                //TEST -- TEST -- TEST


                LastError = "OK";
                Log.Debug("---------------------------------------------------------------------------------------------------------------");
                Log.Debug(" Blocco testata");
                Log.Debug(FunzioniComuni.HexdumpArray(FirmwareBlock.MessaggioTestata, false));
                Log.Debug("---------------------------------------------------------------------------------------------------------------");

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
                if (ArrayTestata.Length >= 29)
                {
                    int _startPos = 0;
                    _esito = ExitCode.FormatoDatiErrato;
                    FirmwareBlock = new BloccoFirmwareSC();
                    FirmwareBlock.AzzeraDati();
                    FirmwareBlock.LenPkt = ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.Release = SerialMessage.ArrayToString(ArrayTestata, _startPos, 10);
                    _startPos += 10;
                    FirmwareBlock.LenFlash = ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.NumSezioni = ArrayTestata[_startPos];
                    _startPos += 1;

                    FirmwareBlock.CrcFW = SerialMessage.ArrayToUshort(ArrayTestata, _startPos, 2);
                    _startPos += 2;

                    // Leggo solo il primo indirizzo
                    FirmwareBlock.AddrSez1 = ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;
                    FirmwareBlock.LenSez1 = ArrayToUint32(ArrayTestata, _startPos, 4);
                    _startPos += 4;

                    /*
                    foreach ( SezioneFirmwareSC Zona in  FirmwareBlock.SezioneFW.OrderBy(SF => SF.NumSezione).ToList() )
                    {
                        Zona.AddrSezione = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                        _startPos += 4;
                        Zona.LenSezione = SerialMessage.ArrayToUint32(ArrayTestata, _startPos, 4);
                        _startPos += 4;
                    }
                    */

                    // Se i primi 4 bytes (Dim pacchetto) sono a FF o a 00 la testata non è valida (vuota)
                    if ((ArrayTestata[0] == 0 && ArrayTestata[1] == 0 && ArrayTestata[2] == 0 && ArrayTestata[3] == 0 )
                        | (ArrayTestata[0] == 0xFF && ArrayTestata[1] == 0xFF && ArrayTestata[2] == 0xFF && ArrayTestata[3] == 0xFF ))
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

    public class FileFirmwareSC
    {
        public string Release { get; set; }
        public string DisplayRelease { get; set; } // Temporaneamente mantenuto per compatibilità. Supercharger non ha display separato
        public string ReleaseDate { get; set; }
        public byte[] ReleaseDateBlock { get; set; }
        public uint LenFlash { get; set; }
        public ushort CrcFlash { get; set; }
        public UInt32 AddrStartAppPtr { get; set; }
        public UInt32 AddrStartApp { get; set; }
        public byte[] DataFlash { get; set; }
        public UInt32 DataFlashPtr { get; set; }

        public List<AreaDatiFWSC> ListaAree = new List<AreaDatiFWSC>();

        public ushort crc { get; set; }
        public bool DatiOK { get; set; }

        public FileFirmwareSC()
        {
            AzzeraDati();
        }

        public void AzzeraDati()
        {
            Release = "";
            ReleaseDateBlock = new byte[3];
            ReleaseDateBlock[0] = 1;
            ReleaseDateBlock[1] = 1;
            ReleaseDateBlock[2] = 23;
            LenFlash = 0;
            CrcFlash = 0;
            AddrStartApp = 0;
            crc = 0;
            DataFlash = new byte[0];
            ListaAree.Clear();
            DatiOK = false;

            crc = 0;
            DataFlash = new byte[0];

        }

    }
    
    
    /// <summary>
    /// Testata Blocco firmware; lunghezza 4K
    /// </summary>
    public class BloccoFirmwareSC
    {
        public string Release { get; set; }
        public byte[] ReleaseData { get; set; }
   

        public string ReleaseDate { get; set; }
        public byte[] ReleaseDateBlock { get; set; }

        public uint LenPkt { get; set; }   // Lunghezza singolo pacchetto / messaggio. variabile in funzione USB/RS485/BLE
        public int NumSezioni { get; set; }
        public UInt32 AddrSez1 { get; set; }
        public UInt32 LenSez1 { get; set; }
        public List<SezioneFirmwareSC>SezioneFW { get; set; }
        public ushort CrcFW { get; set; }
        public short CrcTest { get; set; }


        public uint LenFlash { get; set; }
        public UInt32 AddrStartAppPtr { get; set; }
        public UInt32 AddrStartApp { get; set; }
        public byte[] DataAddraApp { get; set; }
        public byte[] DataFlash { get; set; }
        public UInt32 DataFlashPtr { get; set; }


        public List<AreaDatiFWSC> ListaAree = new List<AreaDatiFWSC>();


        public uint TotaleBlocchi { get; set; }

        public byte[] MessaggioTestata { get; set; }
        public bool TestataOK { get; set; }

        public ushort crc { get; set; }
        public bool DatiOK { get; set; }


        // Parametri per debug

        public int StepDelay { get; set; }
        public int StepTimeout { get; set; }
        public UInt32 StepSize { get; set; }

        public BloccoFirmwareSC()
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
            ReleaseDateBlock[2] = 23;

            NumSezioni = 0;
            SezioneFW = new List<SezioneFirmwareSC>() ;

            CrcFW = 0;
            CrcTest = 0;

            NumSezioni = 0;
            StepDelay = 0;
            StepTimeout = 100;

            MessaggioTestata = new byte[4192];
            for (int _i = 0; _i < 4192; _i++)
                MessaggioTestata[_i] = 0x00;
            TotaleBlocchi = 0;
            crc = 0;
            DataFlash = new byte[0];
            DatiOK = false;
            TestataOK = false;

        }

    }

    public class PacchettoDatiFWSC
    {
        public byte[] PacchettoDati { get; set; }
        public uint NumeroPacchetto { get; set; }
        public ushort DimPacchetto { get; set; }
        public UInt32 AddrDestPacchetto { get; set; }
        public UInt32 AddrFinePacchetto { get; set; }
        public ushort CRC { get; set; }
        public bool DatiOK { get; set; }


        public PacchettoDatiFWSC()
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


    public class AreaDatiFWSC
    {
        public byte[] PacchettoDati { get; set; }
        public uint DimPacchetto { get; set; }
        public UInt32 AddrDestPacchetto { get; set; }
        public UInt32 AddrFinePacchetto { get; set; }
        public ushort CRCPacchetto { get; set; }
        public bool DatiOK { get; set; }
        public List<PacchettoDatiFWSC> ListaPacchetti = new List<PacchettoDatiFWSC>();

        public AreaDatiFWSC()
        {
            AzzeraDati();
        }

        public void AzzeraDati()
        {
            PacchettoDati = new byte[0];
            ListaPacchetti.Clear();
            DimPacchetto = 0;
            AddrDestPacchetto = 0;
            AddrFinePacchetto = 0;
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
    
    
    /// <summary>
    /// Struttura dati per la singola sezione di codice (firmware)
    /// </summary>
    public class SezioneFirmwareSC
    {
        public int NumSezione { get; set; }
        public ulong AddrSezione { get; set; }
        public ulong LenSezione { get; set; }
    }
}
