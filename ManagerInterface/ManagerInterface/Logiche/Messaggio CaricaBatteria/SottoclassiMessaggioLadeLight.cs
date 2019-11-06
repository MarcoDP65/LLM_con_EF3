using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using MoriData;
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
                        Log.Debug(" ---------------------- Programmazione Carica -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        // Parte Fissa:

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
            public string LottoZVT { get; set; }
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
            public string IDApparato { get; set; }
            public uint DataInitZVT { get; set; }
            public uint DataInitPFC { get; set; }
            public uint DataInitDISP { get; set; }
            public ushort VMin { get; set; }
            public ushort VMax { get; set; }
            public ushort Amax { get; set; }
            public byte PresenzaRabboccatore { get; set; }

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


                    if (_messaggio.Length < 236)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    CrcPacchetto = ArrayToUshort(_messaggio, 234, 2);
                    if (CrcPacchetto == 0xFFFF)
                    {
                        // CRC non coerente
                        //return EsitoRisposta.MessaggioVuoto;
                    }
                    else
                    {
                        // Controllo il CRC
                        byte[] _verificaCrc = new byte[234];
                        for (int _i = 0; _i < 234; _i++)
                        {
                            _verificaCrc[_i] = _messaggio[_i];
                        }
                        _tempCRC = codCrc.ComputeChecksum(_verificaCrc);


                        if (CrcPacchetto != _tempCRC)
                        {
                            // CRC non coerente
                            return EsitoRisposta.BadCRC;

                        }
                    }


                    startByte = 0;
                    Log.Debug(" ----------------------  Primo Blocco Memoria  -----------------------------------------");


                    ProduttoreApparato = ArrayToString(_messaggio, startByte, 18);
                    startByte += 18;
                    NomeApparato = ArrayToString(_messaggio, startByte, 10);
                    startByte += 10;
                    SerialeApparato = ArrayToUint32(_messaggio, startByte, 3);
                    if (SerialeApparato != 0xFFFFFF)
                    {
                        ProgressivoCodice = SerialeApparato & 0x03FFFF;
                        AnnoCodice = (byte)(_messaggio[startByte] >> 2);
                    }
                    else
                    {
                        ProgressivoCodice = 0;
                        AnnoCodice = 0;
                    }
                    startByte += 3;
                    TipoApparato = _messaggio[startByte];
                    startByte += 1;
                    DataSetupApparato = ArrayToUint32(_messaggio, startByte, 3);
                    startByte += 3;
                    SerialeZVT = SubArray(_messaggio, startByte, 8);
                    LottoZVT = FunzioniMR.DecodificaStringaLottoZVT(SerialeZVT);
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
                    SoftwareDISP = ArrayToString(_messaggio, startByte, 8);
                    startByte += 8;
                    HardwareDisp = ArrayToString(_messaggio, startByte, 8);
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
                    IDApparato = ArrayToString(_messaggio, startByte, 8);
                    startByte += 8;

                    VMin = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;
                    VMax = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;
                    Amax = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;
                    PresenzaRabboccatore = _messaggio[startByte];
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
                    byte[] _datamap = new byte[236];
                    byte[] _dataSet = new byte[234];
                    int _arrayInit = 0;
                    ushort _temCRC = 0x0000;

                    Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

                    // Variabili temporanee per il passaggio dati
                    byte _byte1 = 0;
                    byte _byte2 = 0;
                    byte _byte3 = 0;
                    byte _byte4 = 0;

                    // Preparo l'array vuoto
                    for (int _i = 0; _i < 236; _i++)
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
                    // Rev HardwarePFC
                    for (int _i = 0; _i < 8; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(HardwarePFC, _i);
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
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(SoftwareDISP, _i);
                    }

                    // Rev Hardware DISP
                    for (int _i = 0; _i < 8; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(HardwareDisp, _i);
                    }


                    // MaxRecordBrevi (3 bytes)
                    // -> MaxRecordBrevi = 0x00E555;
                    FunzioniComuni.SplitUint32(MaxRecordBrevi, ref _byte1, ref _byte2, ref _byte3, ref _byte4);

                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    // Max Testate carica (2 bytes)
                    // -> MaxRecordCarica = 0x01C7;
                    FunzioniComuni.SplitUshort(MaxRecordCarica, ref _byte2, ref _byte1);

                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;

                    // SizeExternMemory (3 bytes)
                    // -> SizeExternMemory = 0x200000;
                    FunzioniComuni.SplitUint32(SizeExternMemory, ref _byte1, ref _byte2, ref _byte3, ref _byte4);

                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    //Max Programmazioni
                    // -> MaxProgrammazioni = 0x10;
                    _datamap[_arrayInit++] = MaxProgrammazioni;

                    //ModelloMemoria
                    // -> ModelloMemoria = 0x01;
                    _datamap[_arrayInit++] = ModelloMemoria;

                    // IDApparato ( Stringa 8 bytes )

                    for (int _i = 0; _i < 8; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(IDApparato, _i);
                    }

                    // VMIN
                    FunzioniComuni.SplitUshort(VMin, ref _byte2, ref _byte1);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;

                    // VMAX
                    FunzioniComuni.SplitUshort(VMax, ref _byte2, ref _byte1);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;

                    // AMAX
                    FunzioniComuni.SplitUshort(Amax, ref _byte2, ref _byte1);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;

                    // Rabboccatore

                    _datamap[_arrayInit++] = PresenzaRabboccatore;


                    for (int _i = 0; _i < 234; _i++)
                    {
                        _dataSet[_i] = _datamap[_i];
                    }

                    _temCRC = codCrc.ComputeChecksum(_dataSet);

                    FunzioniComuni.SplitUshort(_temCRC, ref _byte1, ref _byte2);
                    _datamap[234] = _byte2;
                    _datamap[235] = _byte1;

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

        public class MessaggioProgrammazione
        {
            public byte TipoProgrammazione { get; set; }
            public byte OpzioniProgrammazione { get; set; }
            public ushort IdProgrammazione { get; set; }
            public byte ProgInUse { get; set; }
            public byte[] DataInserimento { get; set; }
            public byte[] AreaParametri { get; set; }
            public byte LenNomeCiclo { get; private set; }
            public string NomeCiclo { get; set; }
            public ushort IdProfilo { get; set; }
            public byte NumParametri { get; set; }
            public List<ParametroLL> Parametri;

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


                    if (_messaggio.Length < 226)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    // Se i primi byte sono a FF -> non è programmato. esco
                    bool _valido = false;
                    for (int _cver = 0; _cver<20; _cver++)
                    {
                        if(_messaggio[_cver] != 0xFF)
                        {
                            // Contriene qualcosa, analizzo il pacchetto
                            _valido = true;
                        }
                    }
                    if(!_valido)
                    {
                        return EsitoRisposta.MessaggioVuoto;
                    }


                    CrcPacchetto = ArrayToUshort(_messaggio, 224, 2);
                    if (CrcPacchetto == 0xFFFF)
                    {
                        // CRC non coerente
                        return EsitoRisposta.MessaggioVuoto;
                    }

                    // Controllo il CRC
                    byte[] _verificaCrc = new byte[224];
                    for (int _i = 0; _i < 224; _i++)
                    {
                        _verificaCrc[_i] = _messaggio[_i];
                    }
                    _tempCRC = codCrc.ComputeChecksum(_verificaCrc);


                    if (CrcPacchetto != _tempCRC)
                    {
                        // CRC non coerente
                        System.Windows.Forms.MessageBox.Show("Errore CRC in lettura programmazione " + CrcPacchetto.ToString("X4") + " - " + _tempCRC.ToString("X4")
                                       , "CONFIGURAZIONE", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information); 

                        // return EsitoRisposta.BadCRC;

                    }


                    startByte = 0;

                    Log.Debug(" ----------------------  Pacchetto Programmazione  -----------------------------------------");

                    
                    TipoProgrammazione = _messaggio[startByte];
                    startByte += 1;
                    OpzioniProgrammazione = _messaggio[startByte];
                    startByte += 1;
                    IdProgrammazione = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

                    //-----------------
                    startByte = 20;   // al momento ignoro tutto fino al byte 20
                    //-----------------
                    ProgInUse = _messaggio[startByte];


                    //-----------------
                    startByte = 32;   // al momento ignoro tutto fino al byte 32
                    //-----------------

                    LenNomeCiclo = _messaggio[startByte];
                    startByte += 1;
                    IdProfilo = _messaggio[startByte];
                    startByte += 1;
                    NumParametri = _messaggio[startByte];
                    startByte += 1;
                    if (LenNomeCiclo > 0)
                    {
                        NomeCiclo = ArrayToString(_messaggio, startByte, LenNomeCiclo);
                        startByte += LenNomeCiclo;
                    }

                    Parametri = new List<ParametroLL>();

                    for (int _i = 0; _i < NumParametri; _i++)
                    {
                        ParametroLL _Par = new ParametroLL();
                        _Par.idParametro = _messaggio[startByte];
                        startByte += 1;
                        _Par.ValoreParametro = ArrayToUshort(_messaggio, startByte, 2);
                        startByte += 2;

                        Parametri.Add(_Par);

                    }

                    AreaParametri = SubArray(_messaggio, 32, 64);


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
                    byte[] _datamap = new byte[226];
                    byte[] _dataSet = new byte[224];
//                    byte[] _tempString;
                    int _arrayInit = 0;
                    ushort _temCRC = 0x0000;

                    Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

                    // Variabili temporanee per il passaggio dati
                    byte _byte1 = 0;
                    byte _byte2 = 0;
                    byte _byte3 = 0;
                    byte _byte4 = 0;

                    // Preparo l'array vuoto
                    for (int _i = 0; _i < 226; _i++)
                    {
                        _datamap[_i] = 0xFF;
                    }

                    // Tipo Programmazione
                    _datamap[_arrayInit++] = TipoProgrammazione;

                    // Opzioni Programmazione
                    _datamap[_arrayInit++] = OpzioniProgrammazione;


                    // Id Programmazione
                    
                    FunzioniComuni.SplitUshort(IdProgrammazione, ref _byte2, ref _byte1);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;

                    // Data inserimento
                    if(DataInserimento == null)
                    {
                        DataInserimento = new byte[5];
                        DateTime OraCorrente = DateTime.Now;
                        DataInserimento[0] = (byte)(OraCorrente.Year - 2000 );
                        DataInserimento[1] = (byte)(OraCorrente.Month);
                        DataInserimento[2] = (byte)(OraCorrente.Day);
                        DataInserimento[3] = (byte)(OraCorrente.Hour);
                        DataInserimento[4] = (byte)(OraCorrente.Minute);
                    }
                    _arrayInit = 0x14;
                    _datamap[_arrayInit++] = DataInserimento[0];
                    _datamap[_arrayInit++] = DataInserimento[1];
                    _datamap[_arrayInit++] = DataInserimento[2];
                    _datamap[_arrayInit++] = DataInserimento[3];
                    _datamap[_arrayInit++] = DataInserimento[4];

                    //Program Used; salto direttamente al byte 0x1F
                    _arrayInit = 0x1F;
                    _datamap[_arrayInit++] = ProgInUse;

                    //--------------------------------------------------------------------------------------------------------------------
                    // Da byte 33 (pos32) registro la stringa parametro nel precedente formato:
                    // 0 - Lunghezza  nome
                    // 1 - Tipo Ciclo
                    // 2 - Numero parametri
                    // 3 .. -Nome (vedi parametro )
                    // Per ogni parametro 
                    //   0    - Id Parametro
                    //   1-2  - Valore Ushort
                    // Dim max per l'area 65 parametri, 208 bytes
                    //--------------------------------------------------------------------------------------------------------------------
                    _arrayInit = 0x20;

                    // Lunghezza nome; Se NomeCiclo è lungo + di 8 lo taglio a 8
                    if (NomeCiclo.Length > 8) NomeCiclo = NomeCiclo.Substring(0, 8);
                    _datamap[_arrayInit++] = (byte)NomeCiclo.Length;

                    // Tipo Ciclo
                    _datamap[_arrayInit++] = (byte)IdProfilo;

                    // Numero Parametri
                    NumParametri = (byte)Parametri.Count;
                    _datamap[_arrayInit++] = NumParametri;

                    // Nome
                    for (int _i = 0; _i < NomeCiclo.Length; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(NomeCiclo, _i);
                    }

                    foreach(ParametroLL _Par in Parametri)
                    {
                        _datamap[_arrayInit++] = _Par.idParametro;

                        FunzioniComuni.SplitUshort(_Par.ValoreParametro, ref _byte2, ref _byte1);
                        _datamap[_arrayInit++] = _byte1;
                        _datamap[_arrayInit++] = _byte2;

                    }





                    for (int _i = 0; _i < 224; _i++)
                    {
                        _dataSet[_i] = _datamap[_i];
                    }

                    _temCRC = codCrc.ComputeChecksum(_dataSet);

                    FunzioniComuni.SplitUshort(_temCRC, ref _byte1, ref _byte2);
                    _datamap[224] = _byte2;
                    _datamap[225] = _byte1;

                    dataBuffer = _datamap;

                    return true;
                }
                catch
                {
                    return false;
                }

            }

            /*
            public bool GeneraByteArrayV2()
            {
                try
                {
                    byte[] _datamap = new byte[226];
                    byte[] _dataSet = new byte[224];
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
                    for (int _i = 0; _i < 226; _i++)
                    {
                        _datamap[_i] = 0xFF;
                    }

                    // Tipo Programmazione
                    _datamap[_arrayInit++] = TipoProgrammazione;

                    // Opzioni Programmazione
                    _datamap[_arrayInit++] = OpzioniProgrammazione;


                    // Id Programmazione

                    FunzioniComuni.SplitUshort(IdProgrammazione, ref _byte2, ref _byte1);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;

                    // Data inserimento
                    if (DataInserimento == null)
                    {
                        DataInserimento = new byte[5];
                        DateTime OraCorrente = DateTime.Now;
                        DataInserimento[0] = (byte)(OraCorrente.Year - 2000);
                        DataInserimento[1] = (byte)(OraCorrente.Month);
                        DataInserimento[2] = (byte)(OraCorrente.Day);
                        DataInserimento[3] = (byte)(OraCorrente.Hour);
                        DataInserimento[4] = (byte)(OraCorrente.Minute);
                    }
                    _arrayInit = 0x14;
                    _datamap[_arrayInit++] = DataInserimento[0];
                    _datamap[_arrayInit++] = DataInserimento[1];
                    _datamap[_arrayInit++] = DataInserimento[2];
                    _datamap[_arrayInit++] = DataInserimento[3];
                    _datamap[_arrayInit++] = DataInserimento[4];

                    //Program Used; salto direttamente al byte 0x1F
                    _arrayInit = 0x1F;
                    _datamap[_arrayInit++] = ProgInUse;

                    //--------------------------------------------------------------------------------------------------------------------
                    // Da byte 33 (pos32) registro la stringa parametro nel precedente formato:
                    // 0 - Lunghezza  nome
                    // 1 - Tipo Ciclo
                    // 2 - Numero parametri
                    // 3 .. -Nome (vedi parametro )
                    // Per ogni parametro 
                    //   0    - Id Parametro
                    //   1-2  - Valore Ushort
                    // Dim max per l'area 64 Bytes
                    //--------------------------------------------------------------------------------------------------------------------
                    _arrayInit = 0x20;

                    // Lunghezza nome; Se NomeCiclo è lungo + di 8 lo taglio a 8
                    if (NomeCiclo.Length > 8) NomeCiclo = NomeCiclo.Substring(0, 8);
                    _datamap[_arrayInit++] = (byte)NomeCiclo.Length;

                    // Tipo Ciclo
                    _datamap[_arrayInit++] = (byte)IdProfilo;

                    // Numero Parametri
                    NumParametri = (byte)Parametri.Count;
                    _datamap[_arrayInit++] = NumParametri;

                    // Nome
                    for (int _i = 0; _i < NomeCiclo.Length; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(NomeCiclo, _i);
                    }

                    foreach (ParametroLL _Par in Parametri)
                    {
                        _datamap[_arrayInit++] = _Par.idParametro;

                        FunzioniComuni.SplitUshort(_Par.ValoreParametro, ref _byte2, ref _byte1);
                        _datamap[_arrayInit++] = _byte1;
                        _datamap[_arrayInit++] = _byte2;


                    }





                    for (int _i = 0; _i < 224; _i++)
                    {
                        _dataSet[_i] = _datamap[_i];
                    }

                    _temCRC = codCrc.ComputeChecksum(_dataSet);

                    FunzioniComuni.SplitUshort(_temCRC, ref _byte1, ref _byte2);
                    _datamap[224] = _byte2;
                    _datamap[225] = _byte1;

                    dataBuffer = _datamap;

                    return true;
                }
                catch
                {
                    return false;
                }

            }
            */

            public bool VuotaPacchetto()
            {
                try
                {

                    TipoProgrammazione = 0;
                    OpzioniProgrammazione = 0;
                    IdProgrammazione = 0;
                    ProgInUse = 0xFF;  // 0xFF = false => ciclo mai usato
                    AreaParametri = null;
                    NomeCiclo = "";
                    IdProfilo = 0 ;
                    NumParametri = 0;
                    Parametri = null; 

                    CrcPacchetto = 0;
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public MessaggioProgrammazione (llProgrammaCarica Programma)
            {
                try
                {
                    if (Programma != null)
                    {

                        TipoProgrammazione = 0;
                        OpzioniProgrammazione = 0;
                        IdProgrammazione = Programma.IdProgramma;
                        ProgInUse = Programma.ProgrammaInUso;
                        NomeCiclo = Programma.ProgramName;
                        DataInserimento = null;
                        IdProfilo = Programma.IdProfilo;

                        Parametri = new List<ParametroLL>();
                        Parametri.Clear();


                        foreach (ParametroLL _par in Programma.ListaParametri)
                        {
                            Parametri.Add(_par);
                        }
                    }
                }
                catch
                {

                }

            }
            public MessaggioProgrammazione()
            {

            }

        }

        public class MessaggioAreaContatori
        {
            public byte[] DataPrimaCarica;
            public UInt32 CntCicliTotali;
            public UInt32 CntCicliStaccoBatt;
            public UInt32 CntCicliStop;
            public UInt32 CntCicliLess3H;
            public UInt32 CntCicli3Hto6H;
            public UInt32 CntCicli6Hto9H;
            public UInt32 CntCicliOver9H;
            public UInt32 CntCicliOpportunity;
            public ushort CntProgrammazioni;

            public UInt32 CntCicliBrevi;
            public UInt32 PntNextBreve;

            public UInt32 CntCariche;
            public UInt32 PntNextCarica;

            public ushort CntMemReset;
            public byte[] DataUltimaCancellazione;


            public ushort CrcPacchetto { get; set; }

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public MessaggioAreaContatori()
            {
                VuotaPacchetto();
            }

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


                    if (_messaggio.Length < 0x38)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }



                    startByte = 0;
                    Log.Debug(" ----------------------  Area Contatori  -----------------------------------------");


                    DataPrimaCarica = SubArray(_messaggio, startByte, 5);
                    startByte += 5;
                    CntCicliTotali = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;
                    CntCicliStop = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;
                    CntCicliStaccoBatt = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;
                    CntCicliLess3H = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;
                    CntCicli3Hto6H = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;
                    CntCicli6Hto9H = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;
                    CntCicliOver9H = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;
                    CntProgrammazioni = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;
                    CntCicliBrevi = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;
                    PntNextBreve = ArrayToUint32(_messaggio, startByte, 3);
                    startByte += 3;
                    CntCariche = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;
                    PntNextCarica = ArrayToUint32(_messaggio, startByte, 3);
                    startByte += 3;
                    CntMemReset = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;
                    if (CntMemReset == 0xFFFF) CntMemReset = 0;
                    DataUltimaCancellazione = SubArray(_messaggio, startByte, 3);
                    startByte += 3;
                    CntCicliOpportunity = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;


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
                    byte[] _datamap = new byte[0x38];
                    byte[] _dataSet = new byte[0x36];
                    int _arrayInit = 0;
                    ushort _temCRC = 0x0000;

                    Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

                    // Variabili temporanee per il passaggio dati
                    byte _byte1 = 0;
                    byte _byte2 = 0;
                    byte _byte3 = 0;
                    byte _byte4 = 0;

                    // Preparo l'array vuoto
                    for (int _i = 0; _i < 0x38; _i++)
                    {
                        _datamap[_i] = 0xFF;

                    }

                    //  DataPrimaCarica
                    _datamap[_arrayInit++] = DataPrimaCarica[0];
                    _datamap[_arrayInit++] = DataPrimaCarica[1];
                    _datamap[_arrayInit++] = DataPrimaCarica[2];
                    _datamap[_arrayInit++] = DataPrimaCarica[3];
                    _datamap[_arrayInit++] = DataPrimaCarica[4];

                    //  CntCicliTotali
                    FunzioniComuni.SplitUint32(CntCicliTotali, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    //  CntCicliStop
                    FunzioniComuni.SplitUint32(CntCicliStop, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    //  CntCicliStaccoBatt
                    FunzioniComuni.SplitUint32(CntCicliStaccoBatt, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;


                    //  CntCicliLess3H
                    FunzioniComuni.SplitUint32(CntCicliLess3H, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    //  CntCicli3Hto6H
                    FunzioniComuni.SplitUint32(CntCicli3Hto6H, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    //  CntCicli6Hto9H
                    FunzioniComuni.SplitUint32(CntCicli6Hto9H, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    //  CntCicliOver9H
                    FunzioniComuni.SplitUint32(CntCicliOver9H, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;


                    //  CntProgrammazioni
                    FunzioniComuni.SplitUshort(CntProgrammazioni, ref _byte1, ref _byte2);
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte1;

                    //  CntCicliBrevi
                    FunzioniComuni.SplitUint32(CntCicliBrevi, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    //  PntNextBreve
                    FunzioniComuni.SplitUint32(CntCicliOver9H, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;


                    //  CntCariche
                    FunzioniComuni.SplitUint32(CntCariche, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    //  PntNextCarica
                    FunzioniComuni.SplitUint32(PntNextCarica, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;


                    //  CntMemReset
                    FunzioniComuni.SplitUshort(CntMemReset, ref _byte1, ref _byte2);
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte1;


                    //  DataUltimaCancellazione
                    _datamap[_arrayInit++] = DataUltimaCancellazione[0];
                    _datamap[_arrayInit++] = DataUltimaCancellazione[1];
                    _datamap[_arrayInit++] = DataUltimaCancellazione[2];

                    //  CntCicliOpportunity
                    FunzioniComuni.SplitUint32(CntCicliOpportunity, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;


                    // Genero il CRC in posizione 0x3A
                    for (int _i = 0; _i < 0x3A; _i++)
                    {
                        _dataSet[_i] = _datamap[_i];
                    }

                    _temCRC = codCrc.ComputeChecksum(_dataSet);

                    FunzioniComuni.SplitUshort(_temCRC, ref _byte1, ref _byte2);
                    _datamap[0x3A] = _byte1;
                    _datamap[0x3B] = _byte2;

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

                    DataPrimaCarica = new byte[5] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

                    CntCicliTotali = 0;
                    CntCicliStop = 0;
                    CntCicliStaccoBatt = 0;
                    CntCicliLess3H = 0;
                    CntCicli3Hto6H = 0;
                    CntCicli6Hto9H = 0;
                    CntCicliOver9H = 0;
                    CntProgrammazioni = 0;
                    CntCicliOpportunity = 0;

                    CntCicliBrevi = 0;
                    PntNextBreve = 0;

                    CntCariche = 0 ;
                    PntNextCarica = 0;

                    CntMemReset = 0;
                    DataUltimaCancellazione = new byte[3] { 0xFF, 0xFF, 0xFF };


                    CrcPacchetto = 0;

                    return true;
                }
                catch
                {
                    return false;
                }
            }




        }

        public class MessaggioMemoriaBreve
        {

            public UInt32 NumeroCarica;
            public ushort NumeroEvtBreve;
            public byte[] DataOraEvento;
            public short VBatt;
            public short IBattMin;
            public short IBatt;
            public short IBattMax;
            public sbyte TempBatt;
            public sbyte TempIGBT1;
            public sbyte TempIGBT2;
            public sbyte TempIGBT3;
            public sbyte TempIGBT4;
            public sbyte TempDiode;
            public UInt32 VettoreErrori;
            public ushort DurataBreve;

            public ushort CrcPacchetto { get; set; }

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public MessaggioMemoriaBreve()
            {
                VuotaPacchetto();
            }

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, int fwLevel)
            {

                //byte[] _risposta;
                int startByte = 0;
                //ushort _tempCRC;
                Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);


                try
                {
                    datiPronti = false;
                    VuotaPacchetto();


                    if (_messaggio.Length < 1)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }



                    startByte = 0;
                    Log.Debug(" ----------------------  Record Breve  -----------------------------------------");



                    NumeroCarica = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;
                    NumeroEvtBreve = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;
                    DataOraEvento = SubArray(_messaggio, startByte, 5);
                    startByte += 5;
                    VBatt = ArrayToShort(_messaggio, startByte, 2);
                    startByte += 2;
                    IBattMin = ArrayToShort(_messaggio, startByte, 2);
                    startByte += 2;
                    IBatt = ArrayToShort(_messaggio, startByte, 2);
                    startByte += 2;
                    IBattMax = ArrayToShort(_messaggio, startByte, 2);
                    startByte += 2;
                    TempBatt = (sbyte)_messaggio[ startByte];
                    startByte += 1;
                    TempIGBT1 = (sbyte)_messaggio[startByte];
                    startByte += 1;
                    TempIGBT2 = (sbyte)_messaggio[startByte];
                    startByte += 1;
                    TempIGBT3 = (sbyte)_messaggio[startByte];
                    startByte += 1;
                    TempIGBT4 = (sbyte)_messaggio[startByte];
                    startByte += 1;
                    TempDiode = (sbyte)_messaggio[startByte];
                    startByte += 1;
                    VettoreErrori = ArrayToUint32(_messaggio, startByte, 3);
                    startByte += 3;
                    DurataBreve = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

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
                    byte[] _datamap = new byte[240];
                    byte[] _dataSet = new byte[238];
                    ushort _temCRC = 0x0000;

                    Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

                    // Variabili temporanee per il passaggio dati
                    byte _byte1 = 0;
                    byte _byte2 = 0;


                    // Preparo l'array vuoto
                    for (int _i = 0; _i < 240; _i++)
                    {
                        _datamap[_i] = 0xFF;
                    }





                    for (int _i = 0; _i < 238; _i++)
                    {
                        _dataSet[_i] = _datamap[_i];
                    }

                    _temCRC = codCrc.ComputeChecksum(_dataSet);

                    FunzioniComuni.SplitUshort(_temCRC, ref _byte1, ref _byte2);
                    _datamap[238] = _byte2;
                    _datamap[239] = _byte1;

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

                    NumeroCarica = 0;
                    NumeroEvtBreve = 0;
                    DataOraEvento = new byte[5] { 01, 01, 18, 0, 0 };
                    VBatt = 0;
                    IBattMin = 0;
                    IBatt = 0;
                    IBattMax = 0;
                    TempBatt = 0;
                    TempIGBT1 = 0;
                    TempIGBT2 = 0;
                    TempIGBT3 = 0;
                    TempIGBT4 = 0;
                    TempDiode = 0;
                    VettoreErrori = 0;
                    DurataBreve = 0;

                    CrcPacchetto = 0;

                    return true;
                }
                catch
                {
                    return false;
                }
            }




        }

        public class MessaggioMemoriaLunga
        {

            public UInt32 NumeroCarica;
            public byte[] IdSpyBatt;
            public ushort IdProgrammazione;
            public UInt32 PntPrimoBreve;
            public ushort CntCicliBrevi;
            public byte[] DataOraStart;
            public ushort Vbat5m;
            public ushort Ibat5m;
            public byte[] DataOraStop;
            public ushort VbatFinale;
            public ushort IbatFinale;
            public ushort AhCaricati;
            public UInt32 WhCaricati;
            public byte OpzioniCarica;
            public UInt32 VettoreErrori;
            public byte ModStop;

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public MessaggioMemoriaLunga()
            {
                VuotaPacchetto();
            }

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, int fwLevel)
            {
                try
                {
                    switch(fwLevel)
                    {
                        case 1:

                            return analizzaMessaggioMappa1(_messaggio);

                        case 2:

                            return analizzaMessaggioMappa2(_messaggio);

                        default:

                            return analizzaMessaggioMappa1(_messaggio);

                    }

                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;

                }

            }

            public EsitoRisposta analizzaMessaggioMappa1(byte[] _messaggio)
            {

                byte[] _risposta;
                int startByte = 0;
                ushort _tempCRC;
                Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);


                try
                {
                    datiPronti = false;
                    VuotaPacchetto();


                    if (_messaggio.Length < 36)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }



                    startByte = 0;
                    Log.Debug(" ----------------------  Record Lungo  -----------------------------------------");
                    
                    NumeroCarica = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;

                    IdSpyBatt = SubArray(_messaggio, startByte, 8);
                    startByte += 8;

                    IdProgrammazione = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

                    PntPrimoBreve = ArrayToUint32(_messaggio, startByte, 3);
                    startByte += 3;

                    DataOraStart = SubArray(_messaggio, startByte, 5);
                    startByte += 5;

                    DataOraStop = SubArray(_messaggio, startByte, 5);
                    startByte += 5;

                    CntCicliBrevi = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

                    AhCaricati = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

                    WhCaricati = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;

                    ModStop = _messaggio[startByte];
                    startByte += 1;

  
                    datiPronti = true;

                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }

            public EsitoRisposta analizzaMessaggioMappa2(byte[] _messaggio)
            {


                Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);


                try
                {
                    int startByte;

                    datiPronti = false;
                    VuotaPacchetto();


                    if (_messaggio.Length < 48)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }



                    startByte = 0;
                    Log.Debug(" ----------------------  Record Lungo  -----------------------------------------");

                    NumeroCarica = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;

                    IdSpyBatt = SubArray(_messaggio, startByte, 8);
                    startByte += 8;

                    IdProgrammazione = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

                    PntPrimoBreve = ArrayToUint32(_messaggio, startByte, 3);
                    startByte += 3;

                    DataOraStart = SubArray(_messaggio, startByte, 5);
                    startByte += 5;
                    
                    Vbat5m = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

                    Ibat5m = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

                    DataOraStop = SubArray(_messaggio, startByte, 5);
                    startByte += 5;

                    VbatFinale = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

                    IbatFinale = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

                    CntCicliBrevi = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

                    AhCaricati = ArrayToUshort(_messaggio, startByte, 2);
                    startByte += 2;

                    WhCaricati = ArrayToUint32(_messaggio, startByte, 4);
                    startByte += 4;

                    OpzioniCarica = _messaggio[startByte];
                    startByte += 1;

                    VettoreErrori = ArrayToUint32(_messaggio, startByte, 3);
                    startByte += 3;

                    ModStop = _messaggio[startByte];
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
                    byte[] _datamap = new byte[240];
                    byte[] _dataSet = new byte[238];
                    ushort _temCRC = 0x0000;

                    Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

                    // Variabili temporanee per il passaggio dati
                    byte _byte1 = 0;
                    byte _byte2 = 0;

                    // Preparo l'array vuoto
                    for (int _i = 0; _i < 240; _i++)
                    {
                        _datamap[_i] = 0xFF;
                    }





                    for (int _i = 0; _i < 238; _i++)
                    {
                        _dataSet[_i] = _datamap[_i];
                    }

                    _temCRC = codCrc.ComputeChecksum(_dataSet);

                    FunzioniComuni.SplitUshort(_temCRC, ref _byte1, ref _byte2);
                    _datamap[238] = _byte2;
                    _datamap[239] = _byte1;

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

                    NumeroCarica = 0;
                    IdSpyBatt = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    IdProgrammazione = 0;
                    PntPrimoBreve = 0;
                    CntCicliBrevi = 0;
                    DataOraStart = new byte[5] { 01, 01, 18, 0, 0 };
                    Vbat5m = 0;
                    Ibat5m = 0;
                    VbatFinale = 0;
                    IbatFinale = 0;              
                    DataOraStop = new byte[5] { 01, 01, 18, 0, 0 };
                    AhCaricati = 0;
                    WhCaricati = 0;
                    OpzioniCarica = 0;
                    VettoreErrori = 0;
                    ModStop = 0;

                    return true;
                }
                catch
                {
                    return false;
                }
            }




        }

        public class MessaggioDatiCliente
        {
            //  0x00 - 0x31 (50)  Cliente
            //  0x32 - 0x63 (50)  Descrizione
            //  0x64 - 0xB3 (80)  Note
            //  0xB4 - 0xC2 (15)  Id Interno
            //  0xC3 - 0xCC (10)  Nome Interno
            //  0xCD - 0xD1 ( 5)  Data installazione
            //  0xD2 - 0xEF liberi - area totale 240 bytes          
            //  0xF0 - 0xF1 ( 2)  CRC interno

            public string Cliente { get; set; }
            public string Descrizione { get; set; }
            public string Note { get; set; }
            public string IdLocale { get; set; }
            public string NomeLocale { get; set; }
            public byte[] DataInstallazione { get; set; }

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


                    if (_messaggio.Length < 236)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    CrcPacchetto = ArrayToUshort(_messaggio, 234, 2);
                    if (CrcPacchetto == 0xFFFF)
                    {
                        // CRC non coerente
                        return EsitoRisposta.MessaggioVuoto;
                    }

                    // Controllo il CRC
                    byte[] _verificaCrc = new byte[234];
                    for (int _i = 0; _i < 234; _i++)
                    {
                        _verificaCrc[_i] = _messaggio[_i];
                    }
                    _tempCRC = codCrc.ComputeChecksum(_verificaCrc);


                    if (CrcPacchetto != _tempCRC)
                    {
                        // CRC non coerente
                        System.Windows.Forms.MessageBox.Show("Errore CRC in lettura dati cliente " + CrcPacchetto.ToString("X4") + " - " + _tempCRC.ToString("X4")
                                       , "CONFIGURAZIONE", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                        // return EsitoRisposta.BadCRC;

                    }


                    startByte = 0;

                    Log.Debug(" ----------------------  Dati Cliente  -----------------------------------------");

                    // cliente
                    Cliente = ArrayToString(_messaggio, startByte, 50);
                    startByte += 50;

                    // Descrizione
                    Descrizione = ArrayToString(_messaggio, startByte, 50);
                    startByte += 50;

                    // Note
                    Note = ArrayToString(_messaggio, startByte, 80);
                    startByte += 80;


                    // Id Interno
                    IdLocale = ArrayToString(_messaggio, startByte, 15);
                    startByte += 15;

                    // Nome Interno
                    NomeLocale = ArrayToString(_messaggio, startByte, 10);
                    startByte += 10;

                    // Data Installazione
                    DataInstallazione = SubArray(_messaggio, startByte, 5);
                    startByte += 5;

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
                    byte[] _datamap = new byte[236];
                    byte[] _dataSet = new byte[234];
                    //                    byte[] _tempString;
                    int _arrayInit = 0;
                    ushort _temCRC = 0x0000;

                    Crc16Ccitt codCrc = new Crc16Ccitt(InitialCrcValue.NonZero1);

                    // Variabili temporanee per il passaggio dati
                    byte _byte1 = 0;
                    byte _byte2 = 0;
                    byte _byte3 = 0;
                    byte _byte4 = 0;

                    // Preparo l'array vuoto
                    for (int _i = 0; _i < 236; _i++)
                    {
                        _datamap[_i] = 0x00;
                    }

                    // Cliente
                    _arrayInit = 0x00;
                    for (int _i = 0; _i < 50; _i++)
                    {
                        if (_i < Cliente.Length) _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(Cliente, _i);
                    }

                    // Descrizione
                    _arrayInit = 0x32;
                    for (int _i = 0; _i < 50; _i++)
                    {
                        if (_i < Descrizione.Length) _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(Descrizione, _i);
                    }


                    // Note
                    _arrayInit = 0x64;
                    for (int _i = 0; _i < 80; _i++)
                    {
                        if (_i < Note.Length) _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(Note, _i);
                    }


                    // Id Interno
                    _arrayInit = 0xB4;
                    for (int _i = 0; _i < 15; _i++)
                    {
                        if (_i < IdLocale.Length) _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(IdLocale, _i);
                    }

                    // Nome Interno
                    _arrayInit = 0xC3;
                    for (int _i = 0; _i < 10; _i++)
                    {
                        if (_i < NomeLocale.Length) _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(NomeLocale, _i);
                    }


                    // Data Installazione
                    _arrayInit = 0xCD;
                    if (DataInstallazione == null)
                    {
                        DataInstallazione = new byte[5];
                        DateTime OraCorrente = DateTime.Now;
                        DataInstallazione[0] = (byte)(OraCorrente.Year - 2000);
                        DataInstallazione[1] = (byte)(OraCorrente.Month);
                        DataInstallazione[2] = (byte)(OraCorrente.Day);
                        DataInstallazione[3] = (byte)(OraCorrente.Hour);
                        DataInstallazione[4] = (byte)(OraCorrente.Minute);
                    }
                    _arrayInit = 0xCD;
                    _datamap[_arrayInit++] = DataInstallazione[0];
                    _datamap[_arrayInit++] = DataInstallazione[1];
                    _datamap[_arrayInit++] = DataInstallazione[2];
                    _datamap[_arrayInit++] = DataInstallazione[3];
                    _datamap[_arrayInit++] = DataInstallazione[4];


                    // Ora calcolo il CRC

                    for (int _i = 0; _i < 234; _i++)
                    {
                        _dataSet[_i] = _datamap[_i];
                    }

                    _temCRC = codCrc.ComputeChecksum(_dataSet);

                    FunzioniComuni.SplitUshort(_temCRC, ref _byte1, ref _byte2);
                    _datamap[0xEA] = _byte2;
                    _datamap[0xEB] = _byte1;

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

                    Cliente = "";
                    Descrizione = "";
                    Note = "";
                    IdLocale = "";
                    NomeLocale = "";
                    DataInstallazione = new byte[5];

                    CrcPacchetto = 0;
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public MessaggioDatiCliente(llDatiCliente Dati)
            {
                try
                {
                    if (Dati != null)
                    {
                        Cliente = Dati.Client;
                        Descrizione = Dati.Description;
                        Note = Dati.Note;
                        IdLocale = Dati.LocalId;
                        NomeLocale = Dati.LocalName;
                        DataInstallazione = new byte[5];
                        for(int _a = 0; _a< 5; _a++)
                        {
                            DataInstallazione[_a] = (byte)_a;
                        }
                    }
                }
                catch
                {

                }

            }
            public MessaggioDatiCliente()
            {
                VuotaPacchetto();
            }

        }


    }


}
