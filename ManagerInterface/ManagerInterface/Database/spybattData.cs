using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using System.IO;
using log4net;
using log4net.Config;
using Newtonsoft.Json;
using ChargerLogic;
using Utility;


namespace MoriData
{
    public class _spybatt
    {
        [PrimaryKey][MaxLength(24)]
        public string Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        [MaxLength(8)]
        public string SwVersion { get; set; }
        [MaxLength(8)]
        public string ProductId { get; set; }
        [MaxLength(20)]
        public string Manufacturer { get; set; }
        public byte HwVersion { get; set; }
        public int ProgramCount { get; set; }
        public byte BattConnected { get; set; }
        public int LongMem { get; set; }
        public string Bootloader { get; set; }
        public string StrategyLibrary { get; set; }


        public override string ToString()
        {
            return Id.ToString();
        }
    }

    public class spybattData
    {
        public string nullID { get { return "0000000000000000"; } }
        public _spybatt _sb = new _spybatt();
        public bool valido;
        [JsonIgnore]
        public MoriData._db _database;
        [JsonIgnore]
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        public bool SoloMemoria = false;
        // private string _tempId;

        // Puntatori e contatori record: non da salvare su DB
        public ushort ContProg { get; set; }
        public uint ContLunghi { get; set; }
        public uint PuntLunghi { get; set; }
        public uint ContBrevi { get; set; }
        public uint PuntBrevi { get; set; }


        public spybattData()
        {
            _sb = new _spybatt();
            //            _sbB = new _spybattBuffer(); 
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
            ContProg = 0;
            ContLunghi = 0;
            PuntLunghi = 0;
            ContBrevi = 0;
            PuntBrevi = 0;

            SoloMemoria = true;
        }

        public spybattData(_db connessione)
        {
            valido = false;
            _sb = new _spybatt();
            SoloMemoria = false;
            _database = connessione;
            _datiSalvati = true;
            ContProg = 0;
            ContLunghi = 0;
            PuntLunghi = 0;
            ContBrevi = 0;
            PuntBrevi = 0;
            _recordPresente = false;
       }

        private _spybatt _caricaDati(string _id)
        {
            try
            {
                return (from s in _database.Table<_spybatt>()
                    where s.Id == _id
                    select s).FirstOrDefault();
            }
            catch ( Exception Ex )
            {
                Log.Error("_caricaDati: " + Ex.Message);
                return null;
            }
        }


        public bool caricaDati(string _id)
        {
            try

            {

                _sb = _caricaDati(_id);

                if (_sb == null)
                {
                    _sb = new _spybatt();
                    return false;
                }
                else
                    valido = true;

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("caricaDati: " + Ex.Message);
                return false;
            }

        }

        public bool salvaDati()
        {
            try 
            {
                if (_database == null)
                    return false;
                if (_sb.Id != nullID && _sb.Id != null && SoloMemoria == false )
                {

                    _spybatt _TestDati = _caricaDati(_sb.Id);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _sb.CreationDate = DateTime.Now;
                        _sb.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_sb);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _sb.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_sb);
                        _datiSalvati = true;
                    }
                    
                   //_database.InsertOrReplace(_sb);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        public bool CaricaHexDump(byte[] Messaggio, bool DatiPuri = false)
        {
            try 
            {
                MessaggioSpyBatt.comandoInizialeSB _mS = new MessaggioSpyBatt.comandoInizialeSB();
                SerialMessage.EsitoRisposta _esito = SerialMessage.EsitoRisposta.MessaggioVuoto;
                _esito = _mS.analizzaMessaggio(Messaggio, DatiPuri);
                if (_esito == SerialMessage.EsitoRisposta.MessaggioOk)
                {
                    return DaMessaggio(_mS);
                }
                return false;
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        public bool DaMessaggio(MessaggioSpyBatt.comandoInizialeSB _mS)
        {
            try
            {
                if (_mS != null)
                {
                    _sb.SwVersion = _mS.revSoftware;
                    _sb.HwVersion = _mS.revHardware;
                    _sb.Manufacturer = _mS.Manufacturer;
                    _sb.ProductId = _mS.productId;
                    _sb.ProgramCount = _mS.numeroProgramma;
                    _sb.BattConnected = _mS.statoBatteria;
                    _sb.LongMem = (int)_mS.longRecordCounter;
                    return true;
                }
                return false;
            }
            catch (Exception Ex)
            {
                Log.Error("SpyBattData DaMessaggio: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        public MappaMemoria ModelloMemoria()
        {
            return ModelloMemoria(fwLevel);
        }


        /// <summary>
        /// Definisce le aree di allocazione memoria in base alla versione del Firmware 
        /// </summary>
        /// <param name="FWLevel">Livello Firmware</param>
        /// <returns></returns>
        public MappaMemoria ModelloMemoria(int FWLevel)
        {
            MappaMemoria _mappaLocale = new MappaMemoria();
            _mappaLocale.datiValidi = false;

            switch (FWLevel)
            {
                case 0:
                    _mappaLocale.Testata = new ElementoMemoria { StartAddress = 0x00, ElemetSize = 64, NoOfElemets = 1, ExtraMem = 0, EndAddress = 0x003F };
                    _mappaLocale.DatiCliente = new ElementoMemoria { StartAddress = 0x40, ElemetSize = 256, NoOfElemets = 4, ExtraMem = 0, EndAddress = 0x043F };
                    _mappaLocale.Programmazioni = new ElementoMemoria { StartAddress = 0x440, ElemetSize = 128, NoOfElemets = 23, ExtraMem = 0,EndAddress = 0x1FFF };
                    _mappaLocale.MemLunga = new ElementoMemoria { StartAddress = 0x134000, ElemetSize = 48, NoOfElemets = 14677, ExtraMem = 0, EndAddress = 0x1DFFFF };
                    _mappaLocale.MemBreve = new ElementoMemoria { StartAddress = 0x2000, ElemetSize = 26, NoOfElemets = 40206, ExtraMem = 0, EndAddress = 0x133FFF };
                    _mappaLocale.datiValidi = true;
                    break;
                case 1:
                case 2:
                    _mappaLocale.Testata = new ElementoMemoria { StartAddress = 0x00, ElemetSize = 64, NoOfElemets = 1, ExtraMem = 0, EndAddress = 0x003F };
                    _mappaLocale.DatiCliente = new ElementoMemoria { StartAddress = 0x40, ElemetSize = 256, NoOfElemets = 4, ExtraMem = 0, EndAddress = 0x043F };
                    _mappaLocale.Programmazioni = new ElementoMemoria { StartAddress = 0x440, ElemetSize = 128, NoOfElemets = 23, ExtraMem = 0, EndAddress = 0x1FFF };
                    _mappaLocale.MemLunga = new ElementoMemoria { StartAddress = 0x134000, ElemetSize = 51, NoOfElemets = 14677, ExtraMem = 0, EndAddress = 0x1DFFFF };
                    _mappaLocale.MemBreve = new ElementoMemoria { StartAddress = 0x2000, ElemetSize = 26, NoOfElemets = 48049, ExtraMem = 0, EndAddress = 0x133FFF };
                    _mappaLocale.datiValidi = true;
                    break;

                case 3:
                    _mappaLocale.Testata = new ElementoMemoria { StartAddress = 0x00, ElemetSize = 64, NoOfElemets = 1, ExtraMem = 3996, EndAddress = 0x0FFF };
                    _mappaLocale.DatiCliente = new ElementoMemoria { StartAddress = 0x1000, ElemetSize = 240, NoOfElemets = 4, ExtraMem = 0, EndAddress = 0x043F };
                    _mappaLocale.Programmazioni = new ElementoMemoria { StartAddress = 0x1400, ElemetSize = 128, NoOfElemets = 23, ExtraMem = 0, EndAddress = 0x1FFF };
                    _mappaLocale.MemLunga = new ElementoMemoria { StartAddress = 0x130000, ElemetSize = 51, NoOfElemets = 13818, ExtraMem = 0, EndAddress = 0x1DFFFF };
                    _mappaLocale.MemBreve = new ElementoMemoria { StartAddress = 0x3000, ElemetSize = 26, NoOfElemets = 1, ExtraMem = 0, EndAddress = 0x133FFF };
                    _mappaLocale.datiValidi = true;
                    break;
                case 4:
                case 5:
                case 6:
                    _mappaLocale.Testata = new ElementoMemoria { StartAddress = 0x00, ElemetSize = 64, NoOfElemets = 1, ExtraMem = 3996, EndAddress = 0x0FFF };
                    _mappaLocale.DatiCliente = new ElementoMemoria { StartAddress = 0x1000, ElemetSize = 240, NoOfElemets = 4, ExtraMem = 0, EndAddress = 0x043F };
                    _mappaLocale.Programmazioni = new ElementoMemoria { StartAddress = 0x1400, ElemetSize = 128, NoOfElemets = 23, ExtraMem = 0, EndAddress = 0x1FFF };
                    _mappaLocale.MemLunga = new ElementoMemoria { StartAddress = 0x130000, ElemetSize = 57, NoOfElemets = 12359, ExtraMem = 0, EndAddress = 0x1DC000 };
                    _mappaLocale.MemBreve = new ElementoMemoria { StartAddress = 0x3000, ElemetSize = 26, NoOfElemets = 1, ExtraMem = 0, EndAddress = 0x133FFF };
                    _mappaLocale.datiValidi = true;
                    break;

                case 99:
                    _mappaLocale.Testata = new ElementoMemoria { StartAddress = 0x00, ElemetSize = 64, NoOfElemets = 1, ExtraMem = 3996, EndAddress = 0x0FFF };
                    _mappaLocale.DatiCliente = new ElementoMemoria { StartAddress = 0x1024, ElemetSize = 240, NoOfElemets = 4, ExtraMem = 0, EndAddress = 0x043F };
                    _mappaLocale.Programmazioni = new ElementoMemoria { StartAddress = 0x142C, ElemetSize = 128, NoOfElemets = 23, ExtraMem = 0, EndAddress = 0x1FFF };
                    _mappaLocale.MemLunga = new ElementoMemoria { StartAddress = 0x13694E, ElemetSize = 51, NoOfElemets = 13818, ExtraMem = 0, EndAddress = 0x1DFFFF };
                    _mappaLocale.MemBreve = new ElementoMemoria { StartAddress = 0x3068, ElemetSize = 26, NoOfElemets = 1, ExtraMem = 0, EndAddress = 0x133FFF };
                    _mappaLocale.datiValidi = true;
                    break;

                default:
                    _mappaLocale.datiValidi = false;
                    break;

            }

                return _mappaLocale;
        }

        public bool cancellaDati( string IdApparato )
        {
            try
            {            
                SQLiteCommand CancellaRecord;
                int esito;
                Log.Debug("Cancella Apparato ------------------------ ");
                // Cliente
                CancellaRecord = _database.CreateCommand("delete from _sbDatiCliente where IdApparato = ? ", IdApparato);
                esito = CancellaRecord.ExecuteNonQuery();
                Log.Debug("Cancella Cliente: " + esito.ToString());

                // Programmazioni
                CancellaRecord = _database.CreateCommand("delete from _sbProgrammaRicarica where IdApparato = ? ", IdApparato);
                esito = CancellaRecord.ExecuteNonQuery();
                Log.Debug("Cancella Programmazioni: " + esito.ToString());

                // Cicli Brevi
                CancellaRecord = _database.CreateCommand("delete from _sbMemBreve where IdApparato = ? ", IdApparato);
                esito = CancellaRecord.ExecuteNonQuery();
                Log.Debug("Cancella Brevi: " + esito.ToString());

                // Cicli Lunghi
                CancellaRecord = _database.CreateCommand("delete from _sbMemLunga where IdApparato = ? ", IdApparato);
                esito = CancellaRecord.ExecuteNonQuery();
                Log.Debug("Cancella Lunghi: " + esito.ToString());

                //Apparato
                CancellaRecord = _database.CreateCommand("delete from _spybatt where Id = ? ", IdApparato);
                esito = CancellaRecord.ExecuteNonQuery();
                Log.Debug("Cancella Apparato: " + esito.ToString());
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }


        /// <summary>
        /// Genera il Bytearray per l'esportazione
        /// </summary>
        public byte[] DataArray
        {
            get
            {
                byte[] _datamap = new byte[64];
                int _arrayInit = 0;

                // Variabili temporanee per il passaggio dati
                byte _byte1 = 0;
                byte _byte2 = 0;
                byte _byte3 = 0;
                byte _byte4 = 0;

                // Preparo l'array vuoto
                for ( int _i = 0; _i < 64; _i++)
                {
                    _datamap[_i] = 0xFF;
                    // 
                }

                //Revisione SW (main e minor)
                for (int _i = 0; _i < 4; _i++)
                {
                    _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(_sb.SwVersion, _i);
                }

                //Revisione SW (build)
                for (int _i = 5; _i < 7; _i++)
                {
                    _datamap[_arrayInit++] = FunzioniComuni.ByteSubString(_sb.SwVersion, _i);
                }

                //Product ID
                for (int _i = 0; _i < 8; _i++)
                {
                    _datamap[_arrayInit++ ] = FunzioniComuni.ByteSubString(_sb.ProductId, _i);
                }

                //Manufacturer
                for (int _i = 0; _i < 18; _i++)
                {
                    _datamap[_arrayInit++ ] = FunzioniComuni.ByteSubString(_sb.Manufacturer, _i);
                }

                //HW version
                _datamap[_arrayInit++] = _sb.HwVersion;

                // Current Prg Count
                // _sb.ProgramCount
                FunzioniComuni.SplitInt32(ContProg, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                //HW version
                _datamap[_arrayInit++] = _sb.BattConnected;

                // N° record short mem
                FunzioniComuni.SplitUint32(ContBrevi, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                // N° record short mem PTR
                FunzioniComuni.SplitUint32(PuntBrevi, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;


                // N° record long mem 
                FunzioniComuni.SplitUint32(ContLunghi, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                // N° record long mem PTR
                FunzioniComuni.SplitUint32(PuntLunghi, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
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



                return _datamap;
            }

        }



        #region Class Parameters

        public string Id
        {
            get { return _sb.Id; }
            set
            {
                if (value != nullID | _sb.Id != value)
                {
                    _sb.Id = value;
                    _datiSalvati = false;
                }
            }
        }

        public DateTime CreationDate
        {
            get { return _sb.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _sb.RevisionDate; }
        }

        public string LastUser
        {
            get { return _sb.LastUser; }
        }


        public string SwVersion
        {
            get { return _sb.SwVersion; }
            set
            {
                _sb.SwVersion = FunzioniMR.StringaMax( value, 7);
                _datiSalvati = false;
            }
        }

        public string Bootloader
        {
            get { return _sb.Bootloader; }
            set
            {
                _sb.Bootloader = FunzioniMR.StringaMax( value, 7);
                _datiSalvati = false;
            }
        }

        public string StrategyLibrary
        {
            get { return _sb.StrategyLibrary; }
            set
            {
                _sb.StrategyLibrary = FunzioniMR.StringaMax(value, 12);
                _datiSalvati = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int fwLevel
        {
            get
            {
                try {
                    string _LocalVer = ""; // _sb.SwVersion.Substring(0, 4);  // controllo solo i primi 4 caratteri della versione perignorare la build

                    if (_sb.SwVersion == null) return 6; // da -1 a 4 per Marco

                    if (_sb.SwVersion.Length >= 4) _LocalVer = _sb.SwVersion.Substring(0, 4);

                    switch (_LocalVer)
                    {
                        case "1.04":
                        case "1.05":
                        case "1.06":
                            return 0;
                        // break;

                        case "1.07":
                            return 1;
                        // break;

                        case "1.08":
                            if (_sb.SwVersion == "1.08.01")
                                return 2;
                            else
                                return 3;
                        // break;
                        case "1.09":
                            return 3;

                        case "1.10":
                        case "1.11":
                        case "1.12":
                        case "1.13":
                        case "2.01":
                            return 4;
                        case "2.02":
                            {
                                switch (_sb.SwVersion)
                                {
                                    case "2.02.01":
                                    case "2.02.02":
                                    case "2.02.03":
                                        return 4;
                                    case "2.02.04":
                                    case "2.02.05":
                                    case "2.02.06":
                                    case "2.02.07":
                                    case "2.02.08":
                                        return 5;
                                    default:
                                        return 5;
                                }

                            }
                        case "2.03":
                            return 6;


                        default:
                            //variante per marco
                            //return -1;
                            return 6;
                            //  break;
                    }
                }
                catch
                {
                    return -1;
                }
            }

        }

        public bool fwFunzioniPro
        {
            get
            {
                try
                {
                    string _LocalVer = ""; // _sb.SwVersion.Substring(0, 4);  // controllo solo i primi 4 caratteri della versione perignorare la build

                    if (_sb.SwVersion == null) return false;

                    if (_sb.SwVersion.Length >= 4) _LocalVer = _sb.SwVersion.Substring(0, 4);

                    switch (_LocalVer)
                    {
                        case "1.11":
                        case "2.01":
                        case "2.02":
                        case "2.03":
                            return true;

                        default:
                            return false;
                            
                    }
                }
                catch
                {
                    return false;
                }
            }

        }

        public bool fwParametriProgEstesa
        {
            get
            {
                try
                {
                    string _LocalVer = ""; // _sb.SwVersion.Substring(0, 4);  // controllo solo i primi 4 caratteri della versione perignorare la build

                    if (_sb.SwVersion == null) return false;

                    if (_sb.SwVersion.Length >= 4) _LocalVer = _sb.SwVersion.Substring(0, 4);

                    switch (_LocalVer)
                    {
                        case "1.11":
                        case "2.01":
                        case "2.02":
                        case "2.03":
                            return true;

                        default:
                            return false;

                    }
                }
                catch
                {
                    return false;
                }
            }

        }


        public int NumPacchetti
        {
            get
            {
                try
                {
                    string _LocalVer = ""; // _sb.SwVersion.Substring(0, 4);  // controllo solo i primi 4 caratteri della versione perignorare la build

                    if (_sb.SwVersion == null) return -1;

                    if (_sb.SwVersion.Length >= 4) _LocalVer = _sb.SwVersion.Substring(0, 4);

                    switch (_LocalVer)
                    {
                        case "1.04":
                        case "1.05":
                        case "1.06":
                        case "1.08":

                            return 8356;
                        // break;
                        case "1.09":
                            return 8666;

                        case "1.10":
                        case "1.11":
                        case "1.12":
                        case "1.13":
                        case "2.01":
                        case "2.02":
                        case "2.03":
                            return 8666;

                        default:
                            return 8356;
                            //  break;
                    }
                }
                catch
                {
                    return 1;
                }
            }

        }

        public string ProductId
        {
            get { return _sb.ProductId; }
            set
            {
                _sb.ProductId = FunzioniMR.StringaMax(value, 8);
                _datiSalvati = false;
            }
        }


        public string Manufacturer
        {
            get { return _sb.Manufacturer; }
            set
            {
                _sb.Manufacturer = FunzioniMR.StringaMax(value, 20);
                _datiSalvati = false;
            }
        }

        public byte HwVersion
        {
            get { return _sb.HwVersion; }
            set
            {
                _sb.HwVersion = value;
                _datiSalvati = false;
            }
        }        

        public int ProgramCount 
        {
            get { return _sb.ProgramCount; }
            set
            {
                _sb.ProgramCount = value;
                _datiSalvati = false;
            }
        }        

        public byte BattConnected 
        {
            get { return _sb.BattConnected; }
            set
            {
                _sb.BattConnected = value;
                _datiSalvati = false;
            }
        } 

        public int LongMem
        {
            get { return _sb.LongMem; }
            set
            {
                _sb.LongMem = value;
                _datiSalvati = false;
            }
        }  

        #endregion Class Parameter


    }

}
