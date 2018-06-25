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
        public class StatoFirmware
        {

            public string RevBootloader;
            public string RevFirmware;
            public string RevDisplay;
            public byte[] ReleaseDateBlock;
            public byte LenPkt;
            public ushort CRCFirmware;
            public uint AddrFlash0;
            public uint LenFlash0;
            public uint AddrFlash1;
            public uint LenFlash1;
            public uint AddrFlash2;
            public uint LenFlash2;
            public uint AddrFlash3;
            public uint LenFlash3;
            public uint AddrFlash4;
            public uint LenFlash4;

            public byte Stato;
            public DateTime IstanteLettura;

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, int fwLevel)
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
                        Log.Debug(" ---------------------- Info Firmware -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        Stato = _risposta[startByte];
                        startByte += 1;
                        RevBootloader = ArrayToString(_risposta, startByte, 6);
                        startByte += 6;
                        RevFirmware = ArrayToString(_risposta, startByte, 6);
                        startByte += 6;
                        RevDisplay = ArrayToString(_risposta, startByte, 6);
                        startByte += 6;
                        LenPkt = _risposta[startByte];
                        startByte += 1;
                        CRCFirmware = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        AddrFlash0 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash0 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash1 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash1 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash2 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash2 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash3 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash3 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash4 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash4 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        /*
                        ReleaseDateBlock = new byte[3];
                        ReleaseDateBlock[0] = _risposta[startByte];
                        if (ReleaseDateBlock[0] == 0xFF)
                            ReleaseDateBlock[0] = 1;

                        startByte += 1;
                        ReleaseDateBlock[1] = _risposta[startByte];
                        if (ReleaseDateBlock[1] == 0xFF)
                            ReleaseDateBlock[1] = 1;

                        startByte += 1;
                        ReleaseDateBlock[2] = _risposta[startByte];
                        if (ReleaseDateBlock[2] == 0xFF)
                            ReleaseDateBlock[2] = 15;

                        startByte += 1;
                        */
                        //Area non usata
                        startByte += 88;
                        if (startByte < _risposta.Length)
                        {
                            Stato = _risposta[startByte];
                            startByte += 1;
                            if (Stato == 0xFF) Stato = 0x07;
                        }

                        datiPronti = true;
                        IstanteLettura = DateTime.Now;

                    }


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }


        }


        public class ProgrammazioneCarica
        {

            public byte TipoProgrammazione { get; set; }
            public byte OpzioniProgrammazione { get; set; }
            public ushort IdProgrammazione { get; set; }
            public ushort VBattNominale { get; set; }
            public ushort VBattMin { get; set; }
            public ushort VBattMax { get; set; }
            public ushort CapacitaNominale { get; set; }
            public byte NumCelle { get; set; }
            public byte TipoBatteria { get; set; }
            public ushort CapacitaMassima { get; set; }
            public ushort TempoMassimo { get; set; }
            public byte NumeroStep { get; set; }
            public byte StatoRecord { get; set; }





            public byte Stato;
            public DateTime IstanteLettura;

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, int fwLevel)
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
                        Log.Debug(" ---------------------- Info Firmware -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));
                        /*
                        Stato = _risposta[startByte];
                        startByte += 1;
                        RevBootloader = ArrayToString(_risposta, startByte, 6);
                        startByte += 6;
                        RevFirmware = ArrayToString(_risposta, startByte, 6);
                        startByte += 6;
                        RevDisplay = ArrayToString(_risposta, startByte, 6);
                        startByte += 6;
                        LenPkt = _risposta[startByte];
                        startByte += 1;
                        CRCFirmware = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        AddrFlash0 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash0 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash1 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash1 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash2 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash2 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash3 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash3 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash4 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash4 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        */
                        /*
                        ReleaseDateBlock = new byte[3];
                        ReleaseDateBlock[0] = _risposta[startByte];
                        if (ReleaseDateBlock[0] == 0xFF)
                            ReleaseDateBlock[0] = 1;

                        startByte += 1;
                        ReleaseDateBlock[1] = _risposta[startByte];
                        if (ReleaseDateBlock[1] == 0xFF)
                            ReleaseDateBlock[1] = 1;

                        startByte += 1;
                        ReleaseDateBlock[2] = _risposta[startByte];
                        if (ReleaseDateBlock[2] == 0xFF)
                            ReleaseDateBlock[2] = 15;

                        startByte += 1;
                        */
                        //Area non usata
                        startByte += 88;
                        if (startByte < _risposta.Length)
                        {
                            Stato = _risposta[startByte];
                            startByte += 1;
                            if (Stato == 0xFF) Stato = 0x07;
                        }

                        datiPronti = true;
                        IstanteLettura = DateTime.Now;

                    }


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }


        }


        public class PrimoBloccoMemoria
        {
            public string ProduttoreApparato { get; set; }  // 18 byte, fisso MORI RADDRIZZATORI 
            public string NomeApparato { get; set; }        // 10 byte, fisso LADE LIGHT 

            public uint SerialeApparato { get; set; }
            public byte AnnoCodice { get; set; }
            public uint ProgressivoCodice { get; set; }
            public byte TipoApparato { get; set; }
            public uint DataSetupApparato { get; set; }


            public byte[] SerialeZVT { get; set; }
            public string HardwareZVT { get; set; }
            public byte[] SerialePFC { get; set; }
            public string HardwarePFC { get; set; }
            public string SoftwarePFC { get; set; }
            public byte[] SerialeDISP { get; set; }
            public string HardwareDisp { get; set; }
            public string SoftwareDISP { get; set; }
            public uint MaxRecordBrevi { get; set; }
            public ushort MaxRecordCarica { get; set; }
            public uint SizeExternMemory { get; set; }
            public byte MaxProgrammazioni { get; set; }
            public byte ModelloMemoria { get; set; }
            public ushort CrcPacchetto { get; set; }




            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, int fwLevel)
            {

                byte[] _risposta;
                int startByte = 0;
                ushort _tempCRC;
                Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);


                try
                {
                    datiPronti = false;
                    VuotaPacchetto();


                    if (_messaggio.Length <128 )
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    CrcPacchetto = ArrayToUshort(_messaggio, 126, 2);
                    if (CrcPacchetto == 0xFFFF)
                    {
                        // CRC non coerente
                        return EsitoRisposta.MessaggioVuoto;
                    }

                    // Controllo il CRC
                    byte[] _verificaCrc = new byte[126];
                    for ( int _i = 0; _i< 126; _i++)
                    {
                        _verificaCrc[_i] = _messaggio[_i];
                    }
                    _tempCRC = codCrc.ComputeChecksum(_verificaCrc);


                    if (CrcPacchetto != _tempCRC)
                    {
                        // CRC non coerente
                        return EsitoRisposta.BadCRC;

                    }



                    startByte = 0;
                    Log.Debug(" ----------------------  Primo Blocco Memoria  -----------------------------------------");


                    ProduttoreApparato = ArrayToString(_messaggio, startByte, 18);
                    startByte += 18;
                    NomeApparato = ArrayToString(_messaggio, startByte, 10);
                    startByte += 10;
                    SerialeApparato = ArrayToUint32(_messaggio, startByte, 3);
                    startByte += 3;
                    TipoApparato = _messaggio[startByte];
                    startByte += 1;
                    DataSetupApparato = ArrayToUint32(_messaggio, startByte, 3);
                    startByte += 3;
                    SerialeZVT = SubArray(_messaggio, startByte, 8);
                    startByte += 8;
                    HardwareZVT = ArrayToString(_messaggio, startByte, 8);
                    startByte += 8;
                    SerialePFC = SubArray(_messaggio, startByte, 8);
                    startByte += 8;
                    HardwarePFC = ArrayToString(_messaggio, startByte, 8);
                    startByte += 8;
                    SoftwarePFC = ArrayToString(_messaggio, startByte, 8);
                    startByte += 8;
                    SerialeDISP = SubArray(_messaggio, startByte, 8);
                    startByte += 8;
                    HardwareDisp = ArrayToString(_messaggio, startByte, 8);
                    startByte += 8;
                    SoftwareDISP = ArrayToString(_messaggio, startByte, 8);
                    startByte += 8;
                    MaxRecordBrevi = ArrayToUint32(_messaggio, startByte, 3);
                    startByte += 3;
                    MaxRecordCarica = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;
                    SizeExternMemory = ArrayToUint32(_messaggio, startByte, 3);
                    startByte += 3;
                    MaxProgrammazioni = _messaggio[startByte];
                    startByte += 1;
                    ModelloMemoria = _messaggio[startByte];
                    startByte += 1;

                    datiPronti = true;

                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }


            public bool GeneraByteArray()
            {
                try
                {
                    byte[] _datamap = new byte[128];
                    byte[] _dataSet = new byte[126];
                    byte[] _tempString;
                    int _arrayInit = 0;
                    ushort _temCRC = 0x0000;

                    Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

                    // Variabili temporanee per il passaggio dati
                    byte _byte1 = 0;
                    byte _byte2 = 0;
                    byte _byte3 = 0;
                    byte _byte4 = 0;

                    // Preparo l'array vuoto
                    for (int _i = 0; _i < 128; _i++)
                    {
                        _datamap[_i] = 0xFF;
                    }

                    //Produttore Apparato 
                    for (int _i = 0; _i < 18; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(ProduttoreApparato, _i);
                    }

                    //Nome Apparato 
                    for (int _i = 0; _i < 10; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(NomeApparato, _i);
                    }

                    // Seriale Apparato (3 bytes)
                    FunzioniComuni.SplitUint32(SerialeApparato, ref _byte1, ref _byte2, ref _byte3, ref _byte4);

                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    // Tipo Apparato
                    _datamap[_arrayInit++] = TipoApparato;

                    // Data inizializzazione Apparato (3 bytes)
                    FunzioniComuni.SplitUint32(DataSetupApparato, ref _byte1, ref _byte2, ref _byte3, ref _byte4);

                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    //Seriale ZVT
                    if (SerialeZVT == null)
                    {
                        _arrayInit += 8;
                    }
                    else
                    {
                        for (int _i = 0; _i < 8; _i++)
                        {
                            if (_i < SerialeZVT.Length)
                            {
                                _datamap[_arrayInit++] = SerialeZVT[_i];
                            }
                            else
                                _datamap[_arrayInit++] = 0x00;
                        }
                    }
                    // Rev HardwareZVT
                    for (int _i = 0; _i < 8; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(HardwareZVT, _i);
                    }

                    //Seriale PFC
                    if (SerialePFC == null)
                    {
                        _arrayInit += 8;
                    }
                    else
                    {
                        for (int _i = 0; _i < 8; _i++)
                        {
                            if (_i < SerialePFC.Length)
                            {
                                _datamap[_arrayInit++] = SerialePFC[_i];
                            }
                            else
                                _datamap[_arrayInit++] = 0x00;
                        }
                    }

                    // Rev SoftwarePFC
                    for (int _i = 0; _i < 8; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(SoftwarePFC, _i);
                    }


                    //Seriale DISP
                    if (SerialeDISP == null)
                    {
                        _arrayInit += 8;
                    }
                    else
                    {
                        for (int _i = 0; _i < 8; _i++)
                        {
                            if (_i < SerialeDISP.Length)
                            {
                                _datamap[_arrayInit++] = SerialeDISP[_i];
                            }
                            else
                                _datamap[_arrayInit++] = 0x00;
                        }
                    }

                    // Rev Software DISP
                    for (int _i = 0; _i < 8; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(SoftwarePFC, _i);
                    }

                    // Rev Hardware DISP
                    for (int _i = 0; _i < 8; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(SoftwarePFC, _i);
                    }


                    // MaxRecordBrevi (3 bytes)
                    FunzioniComuni.SplitUint32(MaxRecordBrevi, ref _byte1, ref _byte2, ref _byte3, ref _byte4);

                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    // Max Testate carica (2 bytes)
                    FunzioniComuni.SplitUshort(MaxRecordCarica, ref _byte1, ref _byte2);

                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;

                    // SizeExternMemory (3 bytes)
                    FunzioniComuni.SplitUint32(SizeExternMemory, ref _byte1, ref _byte2, ref _byte3, ref _byte4);

                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    //Max Programmazioni
                    _datamap[_arrayInit++] = MaxProgrammazioni;

                    //ModelloMemoria
                    _datamap[_arrayInit++] = ModelloMemoria;

                    for (int _i = 0; _i < 126; _i++)
                    {
                        _dataSet[_i] = _datamap[_i];
                    }
                    
                    _temCRC = codCrc.ComputeChecksum(_dataSet);

                    FunzioniComuni.SplitUshort(_temCRC, ref _byte1, ref _byte2);
                    _datamap[126] = _byte1;
                    _datamap[127] = _byte2;

                    dataBuffer = _datamap;

                    return true;
                }
                catch
                {
                    return false;
                }

            }


            public bool VuotaPacchetto()
            {
                try
                {
                    ProduttoreApparato = "MORI RADDRIZZATORI";
                    NomeApparato = "LADE LIGHT";
                    SerialeApparato = 0;
                    AnnoCodice = 18;
                    ProgressivoCodice = 0;
                    TipoApparato = 1;
                    DataSetupApparato = 0x010112;
                    SerialeZVT = new byte[8] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                    HardwareZVT = "";
                    SerialePFC = new byte[8] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                    HardwarePFC = "";
                    SoftwarePFC = "";
                    SerialeDISP = new byte[8] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                    HardwareDisp = "";
                    SoftwareDISP = "";
                    MaxRecordBrevi = 0;
                    MaxRecordCarica = 0;
                    SizeExternMemory = 0x20000;
                    MaxProgrammazioni = 16;
                    ModelloMemoria = 1;

                    CrcPacchetto = 0;

                    return true;
                }
                catch
                {
                    return false;
                }
            }




        }



    }


}
