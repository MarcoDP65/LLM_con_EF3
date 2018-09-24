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
        public class comandoInizialeSB
        {

            byte[] _dataBuffer;
            public byte[] dataBuffer;

            public ushort manufacturerID;
            public string revSoftware;
            public string buildSoftware = "00";
            public string productId;
            public string PrimaInstallazione;
            public string Manufacturer;
            public byte revHardware;
            public ushort numeroProgramma;
            public byte statoBatteria;
            public UInt32 shortRecordCounter;
            public UInt32 shortRecordPointer;
            public UInt32 longRecordCounter;
            public UInt32 longRecordPoiter;
            public UInt32 shortMaxRecord;
            public UInt32 longMaxRecord;
            public UInt32 maxMemLenght;
            public ushort packetLenght;
            public byte maxNumProgram;
            public UInt32 memFree;


            public bool datiPronti;
            public string lastError;

            public comandoInizialeSB()
            {
                dataBuffer = new byte[0];
                manufacturerID = 0;
                revSoftware = "0.00";
                buildSoftware = "00";
                productId = "";
                PrimaInstallazione = "";
                Manufacturer = "";
                revHardware = 0;
                numeroProgramma = 0;
                statoBatteria = 0xF0;
                shortRecordCounter = 0;
                shortRecordPointer = 0;
                longRecordCounter = 0;
                longRecordPoiter = 0;
                shortMaxRecord = 0;
                longMaxRecord = 0;
                maxMemLenght = 0;
                packetLenght = 0;
                maxNumProgram = 0;
                memFree = 0;
            }


            public bool GeneraByteArray()
            {
                try
                {
                    byte[] _datamap = new byte[64];
                    int _arrayInit = 0;

                    // Variabili temporanee per il passaggio dati
                    byte _byte1 = 0;
                    byte _byte2 = 0;
                    byte _byte3 = 0;
                    byte _byte4 = 0;

                    // Preparo l'array vuoto
                    for (int _i = 0; _i < 64; _i++)
                    {
                        _datamap[_i] = 0xFF;
                        // 
                    }

                    //Revisione SW (all)
                    for (int _i = 0; _i < 6; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(revSoftware, _i);
                    }


                    //Product ID
                    for (int _i = 0; _i < 8; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(productId, _i);
                    }

                    //Manufacturer
                    for (int _i = 0; _i < 18; _i++)
                    {
                        _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(Manufacturer, _i);
                    }

                    //HW version
                    _datamap[_arrayInit++] = revHardware;

                    // Current Prg Count
                    // _sb.ProgramCount
                    FunzioniComuni.SplitInt32(numeroProgramma, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    //Battery Connected
                    _datamap[_arrayInit++] = statoBatteria;

                    // N° record short mem
                    FunzioniComuni.SplitUint32(shortRecordCounter, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    // N° record short mem PTR
                    FunzioniComuni.SplitUint32(shortRecordPointer, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;


                    // N° record long mem 
                    FunzioniComuni.SplitUint32(longRecordCounter, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte1;
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    // N° record long mem PTR
                    FunzioniComuni.SplitUint32(longRecordPoiter, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                    _datamap[_arrayInit++] = _byte2;
                    _datamap[_arrayInit++] = _byte3;
                    _datamap[_arrayInit++] = _byte4;

                    // MaxRecord
                    _datamap[_arrayInit++] = 0x00;
                    _datamap[_arrayInit++] = 0x35;
                    _datamap[_arrayInit++] = 0xF5;

                    _datamap[_arrayInit++] = 0x00;
                    _datamap[_arrayInit++] = 0xB9;
                    _datamap[_arrayInit++] = 0x39;

                    _datamap[_arrayInit++] = 0x00;
                    _datamap[_arrayInit++] = 0x20;
                    _datamap[_arrayInit++] = 0x00;

                    _datamap[_arrayInit++] = 0x01;
                    _datamap[_arrayInit++] = 0x00;

                    _datamap[_arrayInit++] = 0x18;

                    _datamap[_arrayInit++] = 0xFF;
                    _datamap[_arrayInit++] = 0xFF;

                    dataBuffer = _datamap;

                    return true;
                }
                catch
                {
                    return false;
                }

            }




            /// <summary>
            /// 
            /// </summary>
            /// <param name="_messaggio">Scompone il pacchetto PARAMETRI INIZIALI</param>
            /// <param name="DatiPuri">Se True il messaggio contiene solo la parte dati e non la parte intestazione (per DumpMem)</param>
            /// <returns></returns>
            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, bool DatiPuri = false)
            {
                //ushort _tempShort;
                //byte _tempByte1;
                //byte _tempByte2;
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
                    bool _esito = false;
                    if (DatiPuri)
                    {
                        _risposta = new byte[(_messaggio.Length)];
                        _risposta = _messaggio;
                        _esito = true;
                    }
                    else
                    {
                        _risposta = new byte[(_messaggio.Length / 2)];
                        _esito = decodificaArray(_messaggio, ref _risposta);
                    }
                    if (_esito)
                    {
                        Log.Debug(" ---------------------- comandoInizialeSB -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;
                        //                     manufacturerID = ArrayToUshort(_risposta, startByte, 2);
                        //                     startByte += 2;
                        manufacturerID = 0;

                        revSoftware = ArrayToString(_risposta, startByte, 4);
                        startByte += 4;
                        if (revSoftware != "1.07")
                        {
                            buildSoftware = ArrayToString(_risposta, startByte, 2);
                            if (buildSoftware == "SP")
                            {
                                buildSoftware = "00";
                            }
                            else
                            {
                                //dalla rev  "1.08" inizia la revisione
                                startByte += 2;
                            }

                        }
                        dataBuffer = _risposta;
                        revSoftware = revSoftware + "." + buildSoftware;
                        productId = ArrayToString(_risposta, startByte, 8);
                        startByte += 8;
                        // nuova versione 6-->8
                        Manufacturer = ArrayToString(_risposta, startByte, 18);
                        startByte += 18;
                        revHardware = _risposta[startByte];
                        startByte += 1;
                        numeroProgramma = ArrayToUint16(_risposta, startByte, 2);
                        if (numeroProgramma == 0xFFFF) numeroProgramma = 0;
                        startByte += 2;
                        statoBatteria = _risposta[startByte];
                        startByte += 1;
                        shortRecordCounter = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        shortRecordPointer = ArrayToUint32(_risposta, startByte, 3);
                        startByte += 3;
                        longRecordCounter = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        longRecordPoiter = ArrayToUint32(_risposta, startByte, 3);
                        startByte += 3;
                        longMaxRecord = ArrayToUint32(_risposta, startByte, 3);
                        startByte += 3;
                        shortMaxRecord = ArrayToUint32(_risposta, startByte, 3);
                        startByte += 3;
                        maxMemLenght = ArrayToUint32(_risposta, startByte, 3);
                        startByte += 3;
                        packetLenght = ArrayToUint16(_risposta, startByte, 2);
                        startByte += 2;
                        maxNumProgram = _risposta[startByte];
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

        public class MemoriaPeriodoLungo
        {
            public byte TipoEvento;
            public UInt32 IdEvento;         //contatore, univoco per apparato
            public UInt32 IdEventoReale;    //contatore, univoco per apparato, dato presente sul record fisico
            public ushort IdProgramma;
            public UInt32 PuntatorePrimoBreve;
            public ushort NumEventiBrevi;
            public byte[] DataOraStart;
            public byte[] DataOraFine;
            public UInt32 Durata;

            public byte TempMin;
            public byte TempMax;
            public ushort Vmin;
            public ushort Vmax;
            public short Amin;
            public short Amax;
            public byte PresenzaElettrolita;
            // 20/05/15 convertito da unsigned a signed
            //public ushort Ah;
            public short Ah;
            public Int32 Wh;
            public Int32 AhCaricati;
            public Int32 WhCaricati;
            public Int32 AhScaricati;
            public Int32 WhScaricati;
            public byte CondizioneStop;
            public byte FattoreCarica;
            public byte StatoCatica;
            public byte ChargerStop;
            public byte TipoCaricatore;
            public UInt32 IdCaricatore;

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, int fwLevel, bool DatiPuri = false)
            {

                byte[] _risposta;
                //int startByte = 0;
                EsitoRisposta _esitoLocale;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    switch (fwLevel)
                    {
                        case 1:
                        case 2:
                        case 3:
                            {
                                _esitoLocale = _analizzaMessaggioLev1(_messaggio, fwLevel, DatiPuri);

                                break;
                            }

                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            {
                                _esitoLocale = _analizzaMessaggioLev4(_messaggio, fwLevel, DatiPuri);
                                break;
                            }
                        default:
                            {
                                _esitoLocale = EsitoRisposta.ErroreGenerico;
                                break;
                            }

                    }


                    return _esitoLocale;
                
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }

            private EsitoRisposta _analizzaMessaggioLev1(byte[] _messaggio, int fwLevel, bool DatiPuri = false)
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

                    //_risposta = new byte[(_messaggio.Length / 2)];

                    //if (decodificaArray(_messaggio, ref _risposta))
                    bool _esito = false;
                    if (DatiPuri)
                    {
                        _risposta = new byte[(_messaggio.Length)];
                        _risposta = _messaggio;
                        _esito = true;
                    }
                    else
                    {
                        _risposta = new byte[(_messaggio.Length / 2)];
                        _esito = decodificaArray(_messaggio, ref _risposta);
                    }
                    if (_esito)
                    {
                        Log.Debug(" ---------------------- MemoriaPeriodoLungo -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;
                        TipoEvento = _risposta[startByte];
                        startByte += 1;
                        IdEvento = ArrayToUint32(_risposta, startByte, 4);
                        IdEventoReale = IdEvento;
                        startByte += 4;
                        IdProgramma = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        PuntatorePrimoBreve = ArrayToUint32(_risposta, startByte, 3);
                        startByte += 3;
                        NumEventiBrevi = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        DataOraStart = SubArray(_risposta, startByte, 5);
                        startByte += 5;
                        DataOraFine = SubArray(_risposta, startByte, 5);
                        startByte += 5;
                        Durata = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        TempMin = _risposta[startByte];
                        startByte += 1;
                        TempMax = _risposta[startByte];
                        startByte += 1;
                        Vmin = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Vmax = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Amin = ArrayToShort(_risposta, startByte, 2);
                        startByte += 2;
                        Amax = ArrayToShort(_risposta, startByte, 2);
                        startByte += 2;
                        PresenzaElettrolita = _risposta[startByte];
                        startByte += 1;
                        // Gestione Correnti
                        // Dalla 1.10.01  (FW 4) sdoppiamento Caricati e scaricati

                        if (true)
                        {

                            if (fwLevel > 0)  // la 1.07 (1) non è /10
                            {
                                Ah = (short)(ArrayToSShort(_risposta, startByte, 2));
                                if (fwLevel > 1)
                                    Ah = (short)(Ah / 10);
                                if (Ah > 0)
                                {
                                    // Simulo la gestione caricati/scaricati
                                    AhScaricati = 0;
                                    AhCaricati = Ah;
                                }
                                else
                                {
                                    // Simulo la gestione caricati/scaricati
                                    AhScaricati = Ah * -1;
                                    AhCaricati = 0;
                                    //Ah = (short)(Ah * -1);
                                }

                            }
                            else
                            {
                                Ah = (short)ArrayToUshort(_risposta, startByte, 2);
                            }
                            startByte += 2;
                            if (fwLevel > 0)
                            {
                                Wh = ArrayToSint32(_risposta, startByte, 4);
                                if (fwLevel > 2)
                                    Wh = Wh / 10;

                                //if (Wh < 0) Wh = Wh * -1;
                            }
                            else
                            {
                                Wh = (Int32)ArrayToUint32(_risposta, startByte, 4);
                            }

                            startByte += 4;
                        }


                        CondizioneStop = _risposta[startByte];
                        startByte += 1;
                        FattoreCarica = _risposta[startByte];
                        startByte += 1;
                        StatoCatica = _risposta[startByte];
                        startByte += 1;
                        ChargerStop = _risposta[startByte];
                        startByte += 1;
                        TipoCaricatore = _risposta[startByte];
                        startByte += 1;
                        IdCaricatore = ArrayToUint32(_risposta, startByte, 3);
                        startByte += 3;

                        datiPronti = true;

                    }


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }

            private EsitoRisposta _analizzaMessaggioLev4(byte[] _messaggio, int fwLevel, bool DatiPuri = false)
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

                    //_risposta = new byte[(_messaggio.Length / 2)];

                    //if (decodificaArray(_messaggio, ref _risposta))
                    bool _esito = false;
                    if (DatiPuri)
                    {
                        _risposta = new byte[(_messaggio.Length)];
                        _risposta = _messaggio;
                        _esito = true;
                    }
                    else
                    {
                        _risposta = new byte[(_messaggio.Length / 2)];
                        _esito = decodificaArray(_messaggio, ref _risposta);
                    }
                    if (_esito)
                    {
                        // Log.Debug(" ---------------------- MemoriaPeriodoLungo -----------------------------------------");
                        Log.Debug( "Lungo: " + FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;
                        TipoEvento = _risposta[startByte];
                        startByte += 1;
                        IdEvento = ArrayToUint32(_risposta, startByte, 4);
                        IdEventoReale = IdEvento;
                        startByte += 4;
                        IdProgramma = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        PuntatorePrimoBreve = ArrayToUint32(_risposta, startByte, 3);
                        startByte += 3;
                        NumEventiBrevi = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        DataOraStart = SubArray(_risposta, startByte, 5);
                        startByte += 5;
                        DataOraFine = SubArray(_risposta, startByte, 5);
                        startByte += 5;
                        Durata = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        TempMin = _risposta[startByte];
                        startByte += 1;
                        TempMax = _risposta[startByte];
                        startByte += 1;
                        Vmin = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Vmax = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Amin = ArrayToShort(_risposta, startByte, 2);
                        startByte += 2;
                        Amax = ArrayToShort(_risposta, startByte, 2);
                        startByte += 2;
                        PresenzaElettrolita = _risposta[startByte];
                        startByte += 1;
                        AhCaricati = (short)(ArrayToSShort(_risposta, startByte, 2));
                        startByte += 2;
                        WhCaricati = ArrayToSint32(_risposta, startByte, 4);
                        startByte += 4;

                        AhScaricati = (short)(ArrayToSShort(_risposta, startByte, 2));
                        startByte += 2;
                        WhScaricati = ArrayToSint32(_risposta, startByte, 4);
                        startByte += 4;
                        if (TipoEvento == (byte)TipoCiclo.Carica)
                        {
                            Ah = (short)(AhCaricati - AhScaricati);
                            Wh = WhCaricati - WhScaricati;
                        }
                        else
                        {
                            Ah = (short)(AhScaricati - AhCaricati);
                            Wh = WhScaricati - WhCaricati;
                        }


                        CondizioneStop = _risposta[startByte];
                        startByte += 1;
                        FattoreCarica = _risposta[startByte];
                        startByte += 1;
                        StatoCatica = _risposta[startByte];
                        startByte += 1;
                        ChargerStop = _risposta[startByte];
                        startByte += 1;
                        TipoCaricatore = _risposta[startByte];
                        startByte += 1;
                        IdCaricatore = ArrayToUint32(_risposta, startByte, 3);
                        startByte += 3;

                        datiPronti = true;

                    }


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }


            public string TensioniCiclo()
            {
                try
                {
                    string _tensioni = "";
                    float _inVolt;
                    _inVolt = Vmin / 100;
                    _tensioni = _inVolt.ToString("0.0");
                    _inVolt = Vmax / 100;
                    _tensioni += "/" + _inVolt.ToString("0.0");
                    return _tensioni;
                }
                catch
                {
                    return "";
                }
            }

            public byte[] RigaTabella()
            {
                return null;
            }

        }

        public class MemoriaPeriodoBreve
        {
            public UInt32 IdEvento;    //contatore, univoco per apparato
            public byte[] DataOraRegistrazione;

            public ushort Vreg;
            public ushort V1;
            public ushort V2;
            public ushort V3;
            public short Amed;
            public short Amin;
            public short Amax;
            public byte Tntc;
            public byte PresenzaElettrolita;
            public byte VbatBk;
            public UInt32 IdCicloLungo;
            public UInt32 PtrPrimoBreve;

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, bool DatiPuri = false , bool NoLog = false)
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

                    //_risposta = new byte[(_messaggio.Length / 2)];
                    //if (decodificaArray(_messaggio, ref _risposta))

                    bool _esito = false;
                    if (DatiPuri)
                    {
                        _risposta = new byte[(_messaggio.Length)];
                        _risposta = _messaggio;
                        _esito = true;
                    }
                    else
                    {
                        _risposta = new byte[(_messaggio.Length / 2)];
                        _esito = decodificaArray(_messaggio, ref _risposta);
                    }
                    if (_esito)
                    {
                        //Log.Debug(" ---------------------- MemoriaPeriodoBreve -----------------------------------------");
                        if(!NoLog) Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;
                        IdEvento = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        DataOraRegistrazione = SubArray(_risposta, startByte, 5);
                        startByte += 5;
                        Vreg = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        V3 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        V2 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        V1 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Amed = ArrayToShort(_risposta, startByte, 2);
                        startByte += 2;
                        Amin = ArrayToShort(_risposta, startByte, 2);
                        startByte += 2;
                        Amax = ArrayToShort(_risposta, startByte, 2);
                        startByte += 2;
                        Tntc = _risposta[startByte];
                        startByte += 1;
                        PresenzaElettrolita = _risposta[startByte];
                        startByte += 1;
                        VbatBk = _risposta[startByte];
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

            public byte[] RigaTabella()
            {
                return null;
            }

        }

        public class CicloBreve
        {
            public UInt32 IdCicloLungo;
            public UInt32 IdSpyBatt;
            public ushort Progressivo;

            public string DataCiclo;
            public string OraCiclo;

            public ushort TensioneIstantanea;
            public ushort Tensione1;
            public ushort Tensione2;
            public ushort Tensione3;

            public ushort CorrenteMedia;
            public ushort CorrenteMin;
            public ushort CorrenteMax;
            public byte SensoreNTC;
            public byte PresenzaElettrolita;
            public byte TensioneBattBK;
            public string DurataCarica;

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, bool DatiPuri = false)
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

                    //_risposta = new byte[(_messaggio.Length / 2)];
                    //if (decodificaArray(_messaggio, ref _risposta))

                    bool _esito = false;
                    if (DatiPuri)
                    {
                        _risposta = new byte[(_messaggio.Length)];
                        _risposta = _messaggio;
                        _esito = true;
                    }
                    else
                    {
                        _risposta = new byte[(_messaggio.Length / 2)];
                        _esito = decodificaArray(_messaggio, ref _risposta);
                    }
                    if (_esito)
                    {

                        Log.Debug(" ---------------------- CicloBreve -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;
                        IdCicloLungo = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;

                        //data e ora
                        startByte += 5;



                        TensioneIstantanea = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Tensione1 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Tensione2 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Tensione3 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        CorrenteMedia = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        CorrenteMin = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        CorrenteMax = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        SensoreNTC = _risposta[startByte];
                        startByte += 1;
                        PresenzaElettrolita = _risposta[startByte];
                        startByte += 1;
                        TensioneBattBK = _risposta[startByte];
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

        public class ProgrammaRicarica
        {
            public UInt32 IdSpyBatt;
            public ushort IdProgramma;

            public string DataInstallazione;
            public byte[] dtInstallazione;

            public ushort BatteryVdef { get; set; }
            public ushort BatteryAhdef { get; set; }
            public byte BatteryType { get; set; }
            public byte BatteryCells { get; set; }
            public byte BatteryCell1 { get; set; }
            public byte BatteryCell2 { get; set; }
            public byte BatteryCell3 { get; set; }
            public byte AbilitaPresElett { get; set; }
            public byte TempMin { get; set; }
            public byte TempMax { get; set; }
            public byte VersoCorrente { get; set; }
            public byte NumeroSpire { get; set; }

            public byte ModoPianificazione { get; set; }
            public ushort CorrenteCaricaMin { get; set; }
            public ushort CorrenteCaricaMax { get; set; }
            public byte PulseRabboccatore { get; set; }
            public byte RipetizioniRabboccatore { get; set; }
            public byte FlagBiberonaggio { get; set; }
            public byte CoeffBiberonaggio { get; set; }
            public byte TempAttenzione { get; set; }
            public byte TempAllarme { get; set; }
            public byte TempRipresa { get; set; }
            public ushort MaxSbilanciamento { get; set; }
            public ushort TempoSbilanciamento { get; set; }
            public ushort TensioneGas { get; set; }
            public ushort DerivaSuperiore { get; set; }
            public ushort DerivaInferiore { get; set; }

            public ushort MinCorrenteW { get; set; }
            public ushort MaxCorrenteW { get; set; }
            public ushort MaxCorrenteImp { get; set; }
            public ushort TensioneRaccordo { get; set; }
            public ushort TensioneFinale { get; set; }

            public NuoviLivelli ResetLivelloCarica { get; set; }

            public byte[] DatiPianificazione { get; set; }

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public byte EsitoScrittura;
            public string lastError;

            public enum NuoviLivelli: ushort { MantieniLivelli = 0x8080, ResetLivelli = 0xFFFF }

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, bool DatiPuri = false)
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

                    //_risposta = new byte[(_messaggio.Length / 2)];
                    //if (decodificaArray(_messaggio, ref _risposta))
                    bool _esito = false;
                    if (DatiPuri)
                    {
                        _risposta = new byte[(_messaggio.Length)];
                        _risposta = _messaggio;
                        _esito = true;
                    }
                    else
                    {
                        _risposta = new byte[(_messaggio.Length / 2)];
                        _esito = decodificaArray(_messaggio, ref _risposta);
                    }
                    if (_esito)
                    {
                        Log.Debug(" ---------------------- ProgrammaRicarica -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;
                        IdProgramma = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        dtInstallazione = SubArray(_risposta, startByte, 3);

                        DataInstallazione = FunzioniMR.StringaDataTS(dtInstallazione);

                        startByte += 3;
                        BatteryVdef = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        BatteryAhdef = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        BatteryType = _risposta[startByte];
                        startByte += 1;
                        BatteryCells = _risposta[startByte];
                        startByte += 1;
                        BatteryCell3 = _risposta[startByte];
                        startByte += 1;
                        BatteryCell2 = _risposta[startByte];
                        startByte += 1;
                        BatteryCell1 = _risposta[startByte];
                        startByte += 1;
                        AbilitaPresElett = _risposta[startByte];
                        startByte += 1;
                        TempMax = _risposta[startByte];
                        startByte += 1;
                        TempMin = _risposta[startByte];
                        startByte += 1;
                        VersoCorrente = _risposta[startByte];
                        startByte += 1;
                        NumeroSpire = _risposta[startByte];
                        startByte += 1;

                        // Dati PRO           
                                                                                               
                        ModoPianificazione = _risposta[startByte];
                        startByte += 1;
                        CorrenteCaricaMin = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        CorrenteCaricaMax = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        PulseRabboccatore = _risposta[startByte];
                        startByte += 1;
                        RipetizioniRabboccatore = _risposta[startByte];
                        startByte += 1;
                        FlagBiberonaggio = _risposta[startByte];
                        startByte += 1;
                        CoeffBiberonaggio = _risposta[startByte];
                        startByte += 1;
                        TempAttenzione = _risposta[startByte];
                        startByte += 1;
                        TempAllarme = _risposta[startByte];
                        startByte += 1;
                        TempRipresa = _risposta[startByte];
                        startByte += 1;
                        MaxSbilanciamento = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        TempoSbilanciamento = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        TensioneGas = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        DerivaSuperiore = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        DerivaInferiore = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        MinCorrenteW = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        MaxCorrenteW = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        MaxCorrenteImp = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        TensioneRaccordo = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        TensioneFinale = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;

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

        public class DatiCliente 
        {

            public UInt32 IdSpyBatt;
            public string Client = "";
            public string BatteryBrand = "";
            public string BatteryModel = "";
            public string BatteryId = "";
            public string BatteryLLId = "";
            public string ClientNote = "";
            public ushort CicliAttesi = 0;
            public string SerialNumber = "";
            public byte[] DataOraUpdate;
            public byte ClientCounter = 0;
            public byte ModoPianificazione = 0 ;
            public byte ModoBiberonaggio = 0;
            public byte ModoRabboccatore = 0;
            public byte[] ModelloPianificazione;
            public byte EqualNumImpulsi { get; set; } = 12;
            public byte EqualNumImpulsiExtra { get; set; } = 4;
            public byte EqualMinErogazione { get; set; } = 5;
            public byte EqualMinPausa { get; set; } = 25;
            public byte EqualMinAttesa { get; set; } = 60;
            public ushort EqualPulseCurrent { get; set; } = 0;

            public NuoviLivelli ResetLivelloCarica { get; set; }
            public enum NuoviLivelli : ushort { MantieniLivelli = 0x0808, ResetLivelli = 0xFFFF }
            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;
            public bool[] PartReceived = new bool[5] { false, false, false, false, false };

            public int stepReceived = 0;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, bool DatiPuri = false)
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


                    //_risposta = new byte[(_messaggio.Length / 2)];
                    //if (decodificaArray(_messaggio, ref _risposta))
                    bool _esito = false;
                    if (DatiPuri)
                    {
                        _risposta = new byte[(_messaggio.Length)];
                        _risposta = _messaggio;
                        _esito = true;
                    }
                    else
                    {
                        _risposta = new byte[(_messaggio.Length / 2)];
                        _esito = decodificaArray(_messaggio, ref _risposta);
                    }
                    if (_esito)
                    {
                        Log.Debug(" ---------------------- DatiCliente -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        byte currStep;
                        startByte = 11;
                        currStep = _risposta[startByte];
                        startByte++;
                        stepReceived++;
                        Log.Debug(stepReceived.ToString() + ": " + FunzioniMR.hexdumpArray(_risposta));
                        Log.Debug(FunzioniMR.hexdumpArray(_messaggio));

                        switch (currStep)
                        {
                            case 1:
                                Client = ArrayToString(_risposta, startByte, 110);
                                startByte += 110;
                                ClientNote = ArrayToString(_risposta, startByte, 110);
                                startByte += 110;
                                PartReceived[0] = true;
                                break;
                            case 2:
                                BatteryBrand = ArrayToString(_risposta, startByte, 110);
                                startByte += 110;
                                BatteryModel = ArrayToString(_risposta, startByte, 55);
                                startByte += 55;
                                // 03/07/15 : ridotto il campo ID a 50 BYTE; i byte 51 e 52 diventano i Cicli Attesi
                                BatteryId = ArrayToString(_risposta, startByte, 50);
                                startByte += 50;
                                CicliAttesi = ArrayToUshort(_risposta, startByte, 2);
                                startByte += 2;
                                BatteryLLId = ArrayToString(_risposta, startByte, 5);
                                startByte += 5;
                                PartReceived[1] = true;
                                break;
                            case 3:
                                SerialNumber = ArrayToString(_risposta, startByte, 20);
                                startByte += 20;
                                PartReceived[2] = true;
                                break;
                            case 4:
                                {
                                    // carico i dati di pianificazione
                                    ModoPianificazione = _risposta[startByte];
                                    startByte += 1;
                                    // in base al ModoPianificazione identifico il tipo mappa.
                                    // 0 / 1 / 2  mappa base con dati pianificazione in 84 byte a partire da 0x04
                                    // 3 / 4      mappa estesa con dati pianificazione in 168 byte a partire da 0x26
                                    switch(ModoPianificazione)
                                    {
                                        case (byte)ParametriSetupPro.TipoPianificazione.NonDefinita:  // nessuna pianificazione
                                        case (byte)ParametriSetupPro.TipoPianificazione.Turni:  // turni base: non implementata
                                        case (byte)ParametriSetupPro.TipoPianificazione.TurniEsteso:  // turni estesi: non implementata
                                        default:
                                            {
                                                ModoBiberonaggio = 0;
                                                ModoRabboccatore = 0;
                                                startByte += 1;
                                                ModelloPianificazione = new byte[84];
                                                for (int _dt = 0; _dt < 84; _dt++)
                                                {
                                                    ModelloPianificazione[_dt] = 0;
                                                }

                                                EqualNumImpulsi = 0;
                                                EqualNumImpulsiExtra = 0;
                                                EqualMinErogazione = 0;
                                                EqualMinPausa = 0;
                                                EqualMinAttesa = 0;
                                                PartReceived[3] = true;
                                                break;
                                            }
                                        case (byte)ParametriSetupPro.TipoPianificazione.Tempo: // tempo base
                                            {

                                                ModoBiberonaggio = _risposta[startByte];
                                                startByte += 1;
                                                ModoRabboccatore = _risposta[startByte];
                                                startByte += 1;
                                                ModelloPianificazione = new byte[84];
                                                for (int _dt = 0; _dt < 84; _dt++)
                                                {
                                                    ModelloPianificazione[_dt] = _risposta[startByte];
                                                    startByte += 1;
                                                }

                                                startByte += 17;  // Area Salvataggio contatori

                                                EqualNumImpulsi = _risposta[startByte];
                                                startByte += 1;
                                                EqualNumImpulsiExtra = _risposta[startByte];
                                                startByte += 1;
                                                EqualMinErogazione = _risposta[startByte];
                                                startByte += 1;
                                                EqualMinPausa = _risposta[startByte];
                                                startByte += 1;
                                                EqualMinAttesa = _risposta[startByte];
                                                startByte += 1;
                                                EqualNumImpulsiExtra = _risposta[startByte];
                                                startByte += 1;
                                                EqualPulseCurrent = ArrayToUshort(_risposta, startByte, 2);
                                                startByte += 2;

                                                PartReceived[3] = true;

                                                break;
                                            }

                                        case (byte)ParametriSetupPro.TipoPianificazione.TempoEsteso: // tempo esteso
                                            {

                                                ModoBiberonaggio = _risposta[startByte];
                                                startByte += 1;
                                                ModoRabboccatore = _risposta[startByte];
                                                startByte += 1;
                                                // byte vuoto
                                                startByte += 1;
                                                EqualMinErogazione = _risposta[startByte];
                                                startByte += 1;
                                                EqualMinPausa = _risposta[startByte];
                                                startByte += 1;
                                                EqualMinAttesa = _risposta[startByte];
                                                startByte += 1;
                                                EqualNumImpulsi = _risposta[startByte];
                                                startByte += 1;
                                                EqualPulseCurrent = ArrayToUshort(_risposta, startByte, 2);
                                                startByte += 2; 

                                                // bytes vuoti
                                                startByte += 26;

                                                ModelloPianificazione = new byte[168];
                                                for (int _dt = 0; _dt < 168; _dt++)
                                                {
                                                    ModelloPianificazione[_dt] = _risposta[startByte];
                                                    startByte += 1;
                                                }

                                                // per ora il resto è vuoto

                                                PartReceived[3] = true;

                                                break;
                                            }

                                    }


                                    break;
                                }
                            default:
                                //stepReceived--;  //  Nulla da fare... pacchetto non previsto
                                break;
                        }

                        if ((PartReceived[0] & PartReceived[1] & PartReceived[2] & PartReceived[3]) || (stepReceived == 4))
                        {
                            datiPronti = true;
                        }

                    }


                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }
        }

        public new class comandoRTC
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

        public new class PacchettoReadMem
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

        public class ImmagineDumpMem
        {

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public ushort numBytes;
            public UInt32 memAddress;
            public byte[] memImage;
            public UInt32 memPtr;
            public byte[] memData;
            public byte[] memDataDecoded;
            public int NumStep;


            public bool datiPronti;

            public ImmagineDumpMem()
            {
                memImage = new byte[2097152];
                memPtr = 0;
                NumStep = 0;
            }


            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {

                byte[] _risposta;
                int startByte = 0;
                //ushort _tempShort;
                //byte _tempByte;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 1)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }
                    numBytes = (ushort)(_messaggio.Length / 2);
                    _risposta = new byte[numBytes];
                    //memData = new byte[numBytes];

                    if (decodificaArray(_messaggio, ref _risposta))
                    {
                        int _pointer;
                        for (_pointer = 0; _pointer < _risposta.Length; _pointer++)
                        {
                            startByte = 0;
                            memImage[memPtr + _pointer] = _risposta[_pointer];
                        }
                        memDataDecoded = _risposta;
                        memPtr += (uint)_pointer;
                        NumStep++;
                    }
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

        public class VariabiliSpybatt
        {

            public ushort TensioneIstantanea;
            public ushort Tensione1;
            public ushort Tensione2;
            public ushort Tensione3;

            public ushort TensioneBattT;


            public short CorrenteBatteria;
            public short AhCaricati;
            public short AhScaricati;
            public byte TempNTC;
            public byte PresenzaElettrolita;
            public byte SoC;
            public byte RF;
            public UInt32 WhScaricati;
            public UInt32 WhCaricati;
            public byte MemProgrammed;
            public byte ConnStatus;

            public DateTime IstanteLettura;

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            /// <summary>
            /// In base al livello Firmware lancio la scomposizione del record Variabili
            /// </summary>
            /// <param name="_messaggio"></param>
            /// <param name="fwLevel"></param>
            /// <returns></returns>
            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, int fwLevel)
            {

                byte[] _risposta;
                int startByte = 0;
                EsitoRisposta _esitoLocale;
                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    switch(fwLevel)
                    {
                        case 1:
                        case 2:
                        case 3:
                            {
                                _esitoLocale = analizzaMessaggioLev1(_messaggio, fwLevel);
                           
                                break;
                            }

                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            {
                                _esitoLocale = analizzaMessaggioLev4(_messaggio, fwLevel);
                                break;
                            }
                        default:
                            {
                                _esitoLocale = EsitoRisposta.ErroreGenerico;
                                break;
                            }

                    }


                    return _esitoLocale;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }

            private EsitoRisposta analizzaMessaggioLev1(byte[] _messaggio, int fwLevel)
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


                        Log.Debug(" ---------------------- VariabiliSpybatt -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;

                        TensioneIstantanea = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Tensione3 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Tensione2 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Tensione1 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        CorrenteBatteria = ArrayToShort(_risposta, startByte, 2);
                        startByte += 2;
                        TempNTC = _risposta[startByte];
                        startByte += 1;
                        PresenzaElettrolita = _risposta[startByte];
                        startByte += 1;
                        // Verificare decimali in base al FW
                        if (fwLevel > 0)
                        {
                            AhScaricati = ArrayToShort(_risposta, startByte, 2);
                            //AhScaricati = (short)(AhScaricati / 10);
                            startByte += 2;
                            AhCaricati = ArrayToShort(_risposta, startByte, 2);
                            //AhCaricati = (short)(AhCaricati / 10);
                            startByte += 2;
                        }
                        else
                        {
                            AhScaricati = (short)ArrayToUshort(_risposta, startByte, 2);
                            startByte += 2;
                            AhCaricati = (short)ArrayToUshort(_risposta, startByte, 2);
                            startByte += 2;
                        }

                        SoC = _risposta[startByte];
                        startByte += 1;
                        RF = _risposta[startByte];
                        startByte += 1;
                        WhScaricati = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        WhCaricati = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        MemProgrammed = _risposta[startByte];
                        startByte += 1;
                        TensioneBattT = _risposta[startByte];
                        startByte += 1;

                        ConnStatus = 0xFF;

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

            private EsitoRisposta analizzaMessaggioLev4(byte[] _messaggio, int fwLevel)
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
                        /*
                        // Dati di prova
                        _risposta = new byte[29]{ 0x12,
                        0x7C,     0x0D,     0xDD,     0x09,
                        0x3C,     0x04,     0x9D,     0xFD,
                        0x50,     0x15,     0x0F,     0x13,
                        0xB6,     0x00,     0x00,     0x64,
                        0xA3,     0x00,     0x03,     0xBD,
                        0x63,     0x00,     0x00,     0x05,
                        0x21,     0x02,     0x3A,     0x1F};
                       */

                        Log.Debug(" ---------------------- VariabiliSpybatt -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;

                        TensioneIstantanea = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Tensione3 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Tensione2 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        Tensione1 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        CorrenteBatteria = ArrayToShort(_risposta, startByte, 2);
                        startByte += 2;
                        TempNTC = _risposta[startByte];
                        startByte += 1;
                        PresenzaElettrolita = _risposta[startByte];
                        startByte += 1;

                        AhScaricati = ArrayToShort(_risposta, startByte, 2);
                        //AhScaricati = (short)(AhScaricati / 10);
                        startByte += 2;
                        AhCaricati = ArrayToShort(_risposta, startByte, 2);
                        //AhCaricati = (short)(AhCaricati / 10);
                        startByte += 2;

                        SoC = _risposta[startByte];
                        startByte += 1;
                        RF = _risposta[startByte];
                        startByte += 1;
                        WhScaricati = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        WhCaricati = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        MemProgrammed = _risposta[startByte];
                        startByte += 1;
                        TensioneBattT = _risposta[startByte];
                        startByte += 1;
                        ConnStatus = _risposta[startByte];
                        startByte += 1;

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

        public class CalibrazioniSpybatt
        {

            public ushort AdcCurrentZero { get; set; }
            public ushort AdcCurrentPos { get; set; }
            public ushort AdcCurrentNeg { get; set; }
            public ushort CurrentPos { get; set; }
            public ushort CurrentNeg { get; set; }
            public ushort GainVbatt { get; set; }
            public ushort ValVbatt { get; set; }
            public ushort GainVbatt3 { get; set; }
            public ushort ValVbatt3 { get; set; }
            public ushort GainVbatt2 { get; set; }
            public ushort ValVbatt2 { get; set; }
            public ushort GainVbatt1 { get; set; }
            public ushort ValVbatt1 { get; set; }
            public DateTime IstanteLettura { get; set; }

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
                        Log.Debug(" ---------------------- CalibrazioniSpybatt -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;

                        AdcCurrentZero = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        AdcCurrentPos = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        CurrentPos = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        AdcCurrentNeg = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        CurrentNeg = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        GainVbatt = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        ValVbatt = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        GainVbatt3 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        ValVbatt3 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        GainVbatt2 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        ValVbatt2 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        GainVbatt1 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        ValVbatt1 = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;


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

        public class ParametriSpybatt
        {

            public ushort LettureTensione { get; set; }
            public ushort LettureCorrente { get; set; }
            public ushort DurataPausa { get; set; }
            public ushort UltimoReset { get; set; }



            public DateTime IstanteLettura { get; set; }

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
                        Log.Debug(" ---------------------- ParametriSpybatt -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;

                        LettureCorrente = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        LettureTensione = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        DurataPausa = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        UltimoReset = 0;
                        if (fwLevel > 6)
                        {
                            UltimoReset = ArrayToUshort(_risposta, startByte, 2);
                            startByte += 2;
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

        public class StatoFirmware
        {

            public string RevBootloader;
            public string RevFirmware;
            public byte[] ReleaseDateBlock;
            public ushort CRCFirmware;
            public uint AddrFlash;
            public uint LenFlash;
            public uint AddrFlash2;
            public uint LenFlash2;
            public uint AddrProxy;
            public uint LenProxy;
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

                        RevBootloader = ArrayToString(_risposta, startByte, 6);
                        startByte += 6;
                        RevFirmware = ArrayToString(_risposta, startByte, 6);
                        startByte += 6;
                        CRCFirmware = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        AddrFlash = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash2 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash2 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrProxy = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenProxy = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
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

                        //Area non usata
                        startByte += 88;
                        if (startByte < _risposta.Length)
                        {
                            Stato = _risposta[startByte];
                            startByte += 1;
                            if (Stato == 0xFF) Stato = 0x07;
                        }
                        else
                        {
                            Stato = 0x07;
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

        public class ComandoStrategia
        {

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public ushort numBytes;
            public UInt32 memAddress;
            public byte[] memData;
            public byte[] memDataDecoded;

            public byte ComandoLibreria;
            public byte LunghezzaDati;
            public byte EsitoChiamata;


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
                    LunghezzaDati = 0;
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

                    if (memData.Length < 3)
                        return EsitoRisposta.RispostaNonValida;



                    ComandoLibreria = memDataDecoded[0];
                    LunghezzaDati = memDataDecoded[1];
                    if ((memDataDecoded.Length - LunghezzaDati) != 3 )
                        return EsitoRisposta.LunghezzaErrata;
                    EsitoChiamata = memData[2];
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

            public string DescComandoLibreria
            {
                get
                {
                    switch (ComandoLibreria)
                    {
                        case 0x01:
                            return "CMD_R - Reset Libreria";

                        case 0x02:
                            return "CMD_IS - Richiesta Strategia";

                        case 0x03:
                            return "CMD_AV - Avanzamento Carica";

                        case 0x04:
                            return "CMD_STP - Arresto Forzato della carica";

                        case 0x05:
                            return "CMD_SIS - Richiesta Strategia";

                        case 0xA0:
                            return "CMD_QRY - Richiesta Informazioni";

                        case 0x54:
                            return "CMD_RDTE - Legge il Tipo Lungo Attuale";

                        case 0x55:
                            return "CMD_WRTE - Forza il Tipo Lungo Attuale";

                        default:
                            return ComandoLibreria.ToString("X2") ;
                  
                    }
                }
            }

            /// <summary>
            /// In base al comando corrente ritorna una stringa con i parametri in chiaro.
            /// </summary>
            /// <value>
            /// The par comando libreria.
            /// </value>
            public string ParComandoLibreria
            {
                get
                {
                    switch (ComandoLibreria)
                    {
                        case 0x01:
                            return "CMD_R - Reset Libreria";

                        case 0x02:
                            return "Richiesta Strategia\nUno\nDue\nTre Quattro e cinque";

                        case 0x03:
                            return "CMD_AV - Avanzamento Carica";

                        case 0x04:
                            return "CMD_STP - Arresto Forzato della carica";

                        case 0x05:
                            return "Richiesta Strategia\nUno\nDue\nTre Quattro e cinque";

                        case 0xA0:
                            return "CMD_QRY - Richiesta Informazioni";

                        case 0x54:
                            return "CMD_RDTE - Legge il Tipo Lungo Attuale";

                        case 0x55:
                            return "CMD_WRTE - Forza il Tipo Lungo Attuale";

                        default:
                            return ComandoLibreria.ToString("X2");

                    }
                }
            }


        }

        public class StatoSig60
        {

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public ushort numBytes;
            public UInt32 memAddress;
            public byte[] memData;
            public byte[] memDataDecoded;

            public byte OCBaudrate;

            public byte ControlReg0;
            public byte ControlReg1;

            public byte ControlReg0_Err;
            public byte ControlReg1_Err;

            public uint NumLetture;
            public uint NumErrori;
            public uint NumInterferenze;

            private bool _datiEstesi = false;
            private bool _datiPronti = false;



            public byte ComandoLibreria;
            public byte LunghezzaDati;
            public byte EsitoChiamata;


            public bool datiPronti;

            public StatoSig60()
            {
                _datiEstesi = false;
                _datiPronti = false;

                NumLetture = 0;
                NumErrori = 0;
                NumInterferenze = 0;

            }

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {

                byte[] _risposta;
                int startByte = 0;
                ushort _tempShort;
                byte _tempByte;

                try
                {
                    datiPronti = false;
                    LunghezzaDati = 0;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }
                    numBytes = (ushort)(_messaggio.Length / 2);
                    _risposta = new byte[numBytes];

                    if (decodificaArray(_messaggio, ref _risposta))
                    {

                        if (_risposta.Length < 1)
                            return EsitoRisposta.RispostaNonValida;


                        if (_risposta.Length == 1)
                        {
                            OCBaudrate = _risposta[0];


                            ControlReg0 = 0x00;
                            ControlReg1 = 0x00;

                            ControlReg0_Err = 0x00;
                            ControlReg1_Err = 0x00;

                            NumLetture = 0;
                            NumErrori = 0;
                            NumInterferenze = 0;

                            _datiEstesi = false;
                            _datiPronti = true;
                            //datiPronti = true;
                            return EsitoRisposta.MessaggioOk;
                        }


                        if (_risposta.Length > 14)
                        {
                            OCBaudrate = _risposta[0];


                            ControlReg0 = _risposta[1];
                            ControlReg1 = _risposta[2];

                            ControlReg0_Err = _risposta[3];
                            ControlReg1_Err = _risposta[4];

                            NumLetture = ArrayToUint32(_risposta, 5, 4);
                            NumInterferenze = ArrayToUint32(_risposta, 9, 4);
                            NumErrori  = ArrayToUint32(_risposta, 13, 4);

                            _datiEstesi = true;
                            _datiPronti = true;
                            return EsitoRisposta.MessaggioOk;
                        }

                        return EsitoRisposta.RispostaNonValida;
                    }
                    return EsitoRisposta.RispostaNonValida;
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


            public bool DatiEstesi
            {
                get
                {
                    return _datiEstesi;
                }
            }

            public bool DatiPronti
            {
                get
                {
                    return _datiPronti;
                }
            }
        }

        public class ComandoEsp32
        {

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public ushort numBytes;
            public UInt32 memAddress;
            public byte[] memData;
            public byte[] memDataDecoded;

            public byte ComandoLibreria;
            public byte LunghezzaDati;
            public byte EsitoChiamata;


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
                    LunghezzaDati = 0;
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

                    if (memData.Length < 3)
                        return EsitoRisposta.RispostaNonValida;



                    ComandoLibreria = memDataDecoded[0];
                    LunghezzaDati = memDataDecoded[1];
                    if ((memDataDecoded.Length - LunghezzaDati) != 3)
                        return EsitoRisposta.LunghezzaErrata;
                    EsitoChiamata = memData[2];
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

            public string DescComandoEsp32
            {
                get
                {
                    switch (ComandoLibreria)
                    {
                        case 0x01:
                            return "CMD_R - Reset Libreria";

                        case 0x02:
                            return "CMD_IS - Richiesta Strategia";

                        case 0x03:
                            return "CMD_AV - Avanzamento Carica";

                        case 0x04:
                            return "CMD_STP - Arresto Forzato della carica";

                        case 0x05:
                            return "CMD_SIS - Richiesta Strategia";

                        case 0xA0:
                            return "CMD_QRY - Richiesta Informazioni";

                        case 0x54:
                            return "CMD_RDTE - Legge il Tipo Lungo Attuale";

                        case 0x55:
                            return "CMD_WRTE - Forza il Tipo Lungo Attuale";

                        default:
                            return ComandoLibreria.ToString("X2");

                    }
                }
            }

            /// <summary>
            /// In base al comando corrente ritorna una stringa con i parametri in chiaro.
            /// </summary>
            /// <value>
            /// </value>
            public string ParComandoEsp32
            {
                get
                {
                    switch (ComandoLibreria)
                    {
                        case 0x01:
                            return "CMD_R - Reset Libreria";

                        case 0x02:
                            return "Richiesta Strategia\nUno\nDue\nTre Quattro e cinque";

                        case 0x03:
                            return "CMD_AV - Avanzamento Carica";

                        case 0x04:
                            return "CMD_STP - Arresto Forzato della carica";

                        case 0x05:
                            return "Richiesta Strategia\nUno\nDue\nTre Quattro e cinque";

                        case 0xA0:
                            return "CMD_QRY - Richiesta Informazioni";

                        case 0x54:
                            return "CMD_RDTE - Legge il Tipo Lungo Attuale";

                        case 0x55:
                            return "CMD_WRTE - Forza il Tipo Lungo Attuale";

                        default:
                            return ComandoLibreria.ToString("X2");

                    }
                }
            }


        }




        public class EsitoMessaggio
        {
            public enum Esiti : byte { OK = 0x0F, KO = 0xF0 };

            public byte CodiceEvento { get; set; }
            public byte Esito { get; set; }
            public bool datiPronti;


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
                        Log.Debug(" ---------------------- EsitoComando -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;
                        CodiceEvento = _risposta[startByte];
                        startByte += 1;
                        Esito = _risposta[startByte];
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
    }
}
