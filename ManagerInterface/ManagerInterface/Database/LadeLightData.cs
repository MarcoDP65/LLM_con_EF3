
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
    public class _ladelight
    {
        [PrimaryKey]
        [MaxLength(24)]
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
        public string NomeApparato { get; set; }
        public string HwVersion { get; set; }
        public byte TipoApparato { get; set; }

        public string SerialeApparato { get; set; }

        public string SerialeZVT { get; set; }
        public string HardwareZVT { get; set; }
        public string SerialePFC { get; set; }
        public string HardwarePFC { get; set; }
        public string SoftwarePFC { get; set; }
        public string SerialeDISP { get; set; }
        public string HardwareDisp { get; set; }
        public string SoftwareDISP { get; set; }

        public DateTime DataSetupApparato { get; set; }
        public DateTime DataUltimaConnessione { get; set; }

        public int ProgramCount { get; set; }
        public int LongMem { get; set; }
        public string Bootloader { get; set; }

        // per la gestione copie in archivio
        public string IdBase { get; set; }
        public int NumeroClone { get; set; }
        public DateTime DataClone { get; set; }
        public string NoteClone { get; set; }



        public override string ToString()
        {
            return Id.ToString();
        }
    }

    public class LadeLightData
    {
        public string nullID { get { return "0000000000000000"; } }
        public _ladelight _ll = new _ladelight();
        public bool valido;
        [JsonIgnore]
        public MoriData._db _database;
        [JsonIgnore]
        private static ILog Log = LogManager.GetLogger("LadeLightData");
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


        public LadeLightData()
        {
            _ll = new _ladelight();
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

        public LadeLightData(_db connessione)
        {
            valido = false;
            _ll = new _ladelight();
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


        public LadeLightData(_db connessione, llParametriApparato ParametriApparato )
        {
            valido = false;
            _ll = new _ladelight();

            if (ParametriApparato.IdApparato == null)
            {
                // Non configurato
                SoloMemoria = true;
                _recordPresente = false; // ? verificare in questo caso
            }
            else
            {
                SoloMemoria = false;
                _recordPresente = false;
                _database = connessione;

                {

                    _ll = _caricaDati(ParametriApparato.IdApparato);

                    if (_ll == null)
                    {
                        _ll = new _ladelight();
                        _ll.Id = ParametriApparato.IdApparato;
                        _ll.IdBase = ParametriApparato.IdApparato;

                    }

                    _ll.ProductId = "";

                    _ll.Manufacturer = ParametriApparato.llParApp.ProduttoreApparato;
                    _ll.HwVersion = "";
                    _ll.SerialeApparato = ParametriApparato.llParApp.SerialeApparato.ToString("x6");
                    _ll.NomeApparato = ParametriApparato.llParApp.NomeApparato;
                    _ll.SerialeZVT = FunzioniComuni.HexdumpArray(ParametriApparato.llParApp.SerialeZVT);
                    _ll.HardwareZVT = ParametriApparato.llParApp.HardwareZVT;

                }

            }



            
            _datiSalvati = true;
            _recordPresente = false;
        }



        private _ladelight _caricaDati(string _id)
        {
            try
            {
                return (from s in _database.Table<_ladelight>()
                        where s.Id == _id
                        select s).FirstOrDefault();
            }
            catch (Exception Ex)
            {
                Log.Error("_caricaDati: " + Ex.Message);
                return null;
            }
        }


        public bool caricaDati(string _id)
        {
            try

            {

                _ll = _caricaDati(_id);

                if (_ll == null)
                {
                    _ll = new _ladelight();
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
                if (_ll.Id != nullID && _ll.Id != null && SoloMemoria == false)
                {

                    _ladelight _TestDati = _caricaDati(_ll.Id);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _ll.CreationDate = DateTime.Now;
                        _ll.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_ll);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _ll.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_ll);
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
                /*
                MessaggioLadeLight.comandoIniziale _mS = new MessaggioLadeLight.comandoIniziale();
                SerialMessage.EsitoRisposta _esito = SerialMessage.EsitoRisposta.MessaggioVuoto;
                _esito = _mS.analizzaMessaggio(Messaggio, DatiPuri);
                if (_esito == SerialMessage.EsitoRisposta.MessaggioOk)
                {
                    return DaMessaggio(_mS);
                }
                */
                return false;
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        public int NumeroCloni()
        {
            try
            {
                int result = 0;
                SQLiteCommand SqlCmd;

                SqlCmd = _database.CreateCommand("select count(*) from _ladelight where IdBase = ? ", _ll.Id);
                result = SqlCmd.ExecuteScalar<int>();

                return result;

            }
            catch (Exception Ex)
            {
                Log.Error("NumeroCloni. " + Ex.Message);
                return 0;
            }
        }

        public int UltimoClone()
        {
            try
            {
                int result = 0;
                SQLiteCommand SqlCmd;
                SqlCmd = _database.CreateCommand("select max(NumeroClone) from _ladelight where IdBase = ? ", _ll.Id);
                result = SqlCmd.ExecuteScalar<int>();

                return result;

            }
            catch (Exception Ex)
            {
                Log.Error("UltimoClone. " + Ex.Message);
                return 0;
            }
        }

        public bool GeneraClone(out string NuovoId, out _ladelight NuovaTestata, bool SalvaDati)
        {

            bool _esito;
            int _numClone;

            try
            {
                if (_ll == null)
                {
                    // Non parto da un record esistente
                    NuovoId = "";
                    NuovaTestata = null;
                    return false;
                }

                if ((_ll.Id == "") || (_ll.Id == nullID))
                {
                    // Non parto da un record esistente
                    NuovoId = "";
                    NuovaTestata = null;
                    return false;
                }

                // record di partenza è valido, genero l'ID clone

                if (_ll.IdBase == "" || _ll.IdBase == null)
                {
                    // Non parto da un record esistente

                    _ll.IdBase = _ll.Id;
                    _ll.NumeroClone = 0;

                    if (!salvaDati())
                    {
                        NuovoId = "";
                        NuovaTestata = null;
                        return false;
                    }
                }
                _numClone = UltimoClone();
                _numClone += 1;

                NuovoId = _ll.IdBase + "." + _numClone.ToString("000");

                NuovaTestata = new _ladelight();
                NuovaTestata.Id = NuovoId;
                NuovaTestata.CreationDate = DateTime.Now;
                NuovaTestata.RevisionDate = DateTime.Now;
                NuovaTestata.LastUser = _ll.LastUser;
                NuovaTestata.SwVersion = _ll.SwVersion;
                NuovaTestata.ProductId = _ll.ProductId;
                NuovaTestata.Manufacturer = _ll.Manufacturer;
                NuovaTestata.HwVersion = _ll.HwVersion;
                NuovaTestata.ProgramCount = _ll.ProgramCount;
                NuovaTestata.LongMem = _ll.LongMem;
                NuovaTestata.Bootloader = _ll.Bootloader;
                NuovaTestata.IdBase = _ll.IdBase;
                NuovaTestata.NumeroClone = _numClone;
                NuovaTestata.DataClone = DateTime.Now;
                NuovaTestata.NoteClone = "";
                int _salvati = 1;

                if (SalvaDati)
                {
                    _salvati = _database.Insert(NuovaTestata);
                }

                if (_salvati > 0)
                    return true;
                else
                {
                    NuovoId = "";
                    NuovaTestata = null;
                    return false;
                }

            }
            catch (Exception Ex)
            {
                Log.Error("UltimoClone. " + Ex.Message);
                NuovoId = "";
                NuovaTestata = null;
                return false;
            }
        }

        public bool DaMessaggio(MessaggioLadeLight.comandoIniziale _mS)
        {
            try
            {
                /*
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
                */
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
                    _mappaLocale.Programmazioni = new ElementoMemoria { StartAddress = 0x440, ElemetSize = 128, NoOfElemets = 23, ExtraMem = 0, EndAddress = 0x1FFF };
                    _mappaLocale.MemLunga = new ElementoMemoria { StartAddress = 0x134000, ElemetSize = 48, NoOfElemets = 14677, ExtraMem = 0, EndAddress = 0x1DFFFF };
                    _mappaLocale.MemBreve = new ElementoMemoria { StartAddress = 0x2000, ElemetSize = 26, NoOfElemets = 40206, ExtraMem = 0, EndAddress = 0x133FFF };
                    _mappaLocale.datiValidi = true;
                    break;
                case 1:

                default:
                    _mappaLocale.datiValidi = false;
                    break;

            }

            return _mappaLocale;
        }

        public bool cancellaDati(string IdApparato)
        {
            try
            {
                SQLiteCommand CancellaRecord;
                int esito = 0;
                Log.Debug("Cancella Apparato ------------------------ ");
                // Cliente
                // CancellaRecord = _database.CreateCommand("delete from _sbDatiCliente where IdApparato = ? ", IdApparato);
                //esito = CancellaRecord.ExecuteNonQuery();
                Log.Debug("Cancella Cliente: " + esito.ToString());

                // Programmazioni
                // CancellaRecord = _database.CreateCommand("delete from _sbProgrammaRicarica where IdApparato = ? ", IdApparato);
                // esito = CancellaRecord.ExecuteNonQuery();
                Log.Debug("Cancella Programmazioni: " + esito.ToString());

                // Cicli Brevi
                // CancellaRecord = _database.CreateCommand("delete from _sbMemBreve where IdApparato = ? ", IdApparato);
                // esito = CancellaRecord.ExecuteNonQuery();
                Log.Debug("Cancella Brevi: " + esito.ToString());

                // Cicli Lunghi
                // CancellaRecord = _database.CreateCommand("delete from _sbMemLunga where IdApparato = ? ", IdApparato);
                // esito = CancellaRecord.ExecuteNonQuery();
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

        #region Class Parameters

        public string Id
        {
            get { return _ll.Id; }
            set
            {
                if (value != nullID | _ll.Id != value)
                {
                    _ll.Id = value;
                    _datiSalvati = false;
                }
            }
        }

        public string SerialeApparato
        {
            get { return _ll.SerialeApparato; }
            set
            {
                if (value != nullID | _ll.SerialeApparato != value)
                {
                    _ll.SerialeApparato = value;
                    _datiSalvati = false;
                }
            }
        }


        public string SerialeZVT
        {
            get { return _ll.SerialeZVT; }
            set
            {
                if (value != nullID | _ll.SerialeZVT != value)
                {
                    _ll.SerialeZVT = value;
                    _datiSalvati = false;
                }
            }
        }

        public string SerialePFC
        {
            get { return _ll.SerialePFC; }
            set
            {
                if (value != nullID | _ll.SerialePFC != value)
                {
                    _ll.SerialePFC = value;
                    _datiSalvati = false;
                }
            }
        }

        public string SerialeDISP
        {
            get { return _ll.SerialeDISP; }
            set
            {
                if (value != nullID | _ll.SerialeDISP != value)
                {
                    _ll.SerialeDISP = value;
                    _datiSalvati = false;
                }
            }
        }



        public DateTime CreationDate
        {
            get { return _ll.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _ll.RevisionDate; }
        }

        public string LastUser
        {
            get { return _ll.LastUser; }
        }


        public string SwVersion
        {
            get { return _ll.SwVersion; }
            set
            {
                _ll.SwVersion = FunzioniMR.StringaMax(value, 7);
                _datiSalvati = false;
            }
        }

        public string Bootloader
        {
            get { return _ll.Bootloader; }
            set
            {
                _ll.Bootloader = FunzioniMR.StringaMax(value, 7);
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
                try
                {
                    string _LocalVer = ""; // _sb.SwVersion.Substring(0, 4);  // controllo solo i primi 4 caratteri della versione perignorare la build
                    return 1;
                }
                catch
                {
                    return 0;
                }
            }

        }

        public bool fwFunzioniPro
        {
            get
            {
                try
                {

                    return false;

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

                    if (_ll.SwVersion == null) return -1;

                    /*

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
                                            case "2.04":
                                            case "2.05":
                                            case "3.00":
                                            case "3.01":
                                                return 8666;

                                            default:
                                                return 8356;
                                                //  break;
                                        }
                                        */
                    return 0;
                }
                catch
                {
                    return 1;
                }
            }

        }

        public string ProductId
        {
            get { return _ll.ProductId; }
            set
            {
                _ll.ProductId = FunzioniMR.StringaMax(value, 8);
                _datiSalvati = false;
            }
        }


        public string Manufacturer
        {
            get { return _ll.Manufacturer; }
            set
            {
                _ll.Manufacturer = FunzioniMR.StringaMax(value, 20);
                _datiSalvati = false;
            }
        }

        public string HwVersion
        {
            get { return _ll.HwVersion; }
            set
            {
                _ll.HwVersion = value;
                _datiSalvati = false;
            }
        }

        public int ProgramCount
        {
            get { return _ll.ProgramCount; }
            set
            {
                _ll.ProgramCount = value;
                _datiSalvati = false;
            }
        }


        public int LongMem
        {
            get { return _ll.LongMem; }
            set
            {
                _ll.LongMem = value;
                _datiSalvati = false;
            }
        }
        /*
        public string IdBase { get; set; }
        public int NumeroClone { get; set; }
        public DateTime DataClone { get; set; }
        public string NoteClone { get; set; }
        */

        public string IdBase
        {
            get { return _ll.IdBase; }
            set
            {
                if (value != nullID | _ll.IdBase != value)
                {
                    _ll.IdBase = value;
                    _datiSalvati = false;
                }
            }
        }


        public int NumeroClone
        {
            get { return _ll.NumeroClone; }
            set
            {
                _ll.NumeroClone = value;
                _datiSalvati = false;
            }
        }

        public DateTime DataClone
        {
            get { return _ll.DataClone; }
            set
            {
                _ll.DataClone = value;
                _datiSalvati = false;
            }
        }
        public string NoteClone
        {
            get { return _ll.NoteClone; }
            set
            {
                _ll.NoteClone = value;
                _datiSalvati = false;
            }
        }

        #endregion Class Parameter


    }

}

