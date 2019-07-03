
//    class llMemBreve
using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using System.IO;
using System.Globalization;

using log4net;
using log4net.Config;

using ChargerLogic;
using Utility;


namespace MoriData

{
    public class _llMemBreve
    {
        [PrimaryKey, AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        [Indexed(Name = "IDMemBreve", Order = 1, Unique = true)]
        public string IdApparato { get; set; }
        [Indexed(Name = "IDMemBreve", Order = 2, Unique = true)]
        public Int32 IdMemCiclo { get; set; }
        [Indexed(Name = "IDMemBreve", Order = 3, Unique = true)]
        public Int32 IdMemoriaBreve { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        public string DataOraRegistrazione { get; set; }
        public byte[] TimestampRegistrazione { get; set; }

        public int VBatt { get; set; }
        public int IBatt { get; set; }
        public int IBattMin { get; set; }
        public int IBattMax { get; set; }
        public byte TempBatt { get; set; }
        public byte TempIGBT1 { get; set; }
        public byte TempIGBT2 { get; set; }
        public byte TempIGBT3 { get; set; }
        public byte TempIGBT4 { get; set; }
        public byte TempDiode { get; set; }
        public UInt32 VettoreErrori { get; set; }
        public ushort DurataBreve { get; set; }


    }

    public class llMemBreve
    {

        public string nullID { get { return "0000000000000000"; } }
        public _llMemBreve _llsm = new _llMemBreve();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("llMemBreve");
        public bool _datiSalvati;
        public bool _recordPresente;

        public llMemBreve()
        {
            _llsm = new _llMemBreve();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public llMemBreve(_llMemBreve _dati)
        {
            _llsm = _dati;
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public llMemBreve(_db connessione)
        {
            valido = false;
            _llsm = new _llMemBreve();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _llMemBreve _caricaDati(int _id)
        {
            return (from s in _database.Table<_llMemBreve>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        private _llMemBreve _caricaDati(string _IdApparato, Int32 _IdMemCiclo, Int32 _IdMemoriaBreve)
        {
            return (from s in _database.Table<_llMemBreve>()
                    where s.IdApparato == _IdApparato & s.IdMemCiclo == _IdMemCiclo & s.IdMemoriaBreve == _IdMemoriaBreve
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                _llsm = _caricaDati(idLocale);
                if (_llsm == null)
                {
                    _llsm = new _llMemBreve();
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool caricaDati(string IdApparato, int IdMemCiclo, int IdMemoriaBreve)
        {
            try
            {
                _llsm = _caricaDati(IdApparato, IdMemCiclo, IdMemoriaBreve);
                if (_llsm == null)
                {
                    _llsm = new _llMemBreve();
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool salvaDati()
        {
            try
            {
                if (_llsm.IdApparato != nullID & _llsm.IdApparato != null & _llsm.IdMemCiclo != 0)
                {

                    _llMemBreve _TestDati = _caricaDati(_llsm.IdApparato, _llsm.IdMemCiclo, _llsm.IdMemoriaBreve);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _llsm.CreationDate = DateTime.Now;
                        _llsm.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_llsm);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _llsm.IdLocale = _TestDati.IdLocale;
                        _llsm.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_llsm);
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


        /// <summary>
        /// Genera il Bytearray per l'esportazione
        /// </summary>
        public byte[] DataArrayV4
        {
            get
            {
                byte[] _datamap = new byte[26];
                int _arrayInit = 0;
                /*
                // Variabili temporanee per il passaggio dati
                byte _byte1 = 0;
                byte _byte2 = 0;
                byte _byte3 = 0;
                byte _byte4 = 0;
                byte[] _tmpTimestamp;

                // Preparo l'array vuoto
                for (int _i = 0; _i < _datamap.Length; _i++)
                {
                    _datamap[_i] = 0xFF;
                    // 
                }

                // N° evento lungo
                FunzioniComuni.SplitInt32(_sbsm.IdMemoriaLunga, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;


                // Timestamp
                _tmpTimestamp = FunzioniComuni.splitStringaTS(_sbsm.DataOraRegistrazione);
                _datamap[_arrayInit++] = _tmpTimestamp[0];
                _datamap[_arrayInit++] = _tmpTimestamp[1];
                _datamap[_arrayInit++] = _tmpTimestamp[2];
                _datamap[_arrayInit++] = _tmpTimestamp[3];
                _datamap[_arrayInit++] = _tmpTimestamp[4];


                // V Batt 
                FunzioniComuni.SplitInt32(_sbsm.Vreg, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;


                FunzioniComuni.SplitInt32(_sbsm.V3, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                FunzioniComuni.SplitInt32(_sbsm.V2, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                FunzioniComuni.SplitInt32(_sbsm.V1, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;



                FunzioniComuni.SplitInt32(_sbsm.Amed, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                FunzioniComuni.SplitInt32(_sbsm.Amin, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                FunzioniComuni.SplitInt32(_sbsm.Amax, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                _datamap[_arrayInit++] = _sbsm.Tntc;
                _datamap[_arrayInit++] = _sbsm.PresenzaElettrolita;
                _datamap[_arrayInit++] = _sbsm.VbatBk;
                */
                return _datamap;
            }

        }

        #region Class Parameters

        public int IdLocale
        {
            get { return _llsm.IdLocale; }
            set
            {

                _llsm.IdLocale = value;
                _datiSalvati = false;

            }
        }
        public string IdApparato
        {
            get { return _llsm.IdApparato; }
            set
            {
                if (value != null)
                {
                    _llsm.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }
        public int IdMemCiclo
        {
            get { return _llsm.IdMemCiclo; }
            set
            {

                _llsm.IdMemCiclo = value;
                _datiSalvati = false;

            }
        }
        public string strIdMemCiclo
        {
            get { return _llsm.IdMemCiclo.ToString(); }
        }

        public int IdMemoriaBreve
        {
            get { return _llsm.IdMemoriaBreve; }
            set
            {
                if (value != null)
                {
                    _llsm.IdMemoriaBreve = value;
                    _datiSalvati = false;
                }
            }
        }
        public string strIdMemoriaBreve
        {
            get { return _llsm.IdMemoriaBreve.ToString(); }
        }

        public DateTime CreationDate
        {
            get { return _llsm.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _llsm.RevisionDate; }
        }

        public string LastUser
        {
            get { return _llsm.LastUser; }
        }

        public string DataOraRegistrazione
        {
            get { return _llsm.DataOraRegistrazione; }
            set
            {
                _llsm.DataOraRegistrazione = value;
                _datiSalvati = false;
            }
        }

        public DateTime dtDataOraRegistrazione
        {
            get
            {
                DateTime _dataora;


                if (_llsm.DataOraRegistrazione.Length != 15)
                {
                    // data non formattata correttamente
                    return DateTime.Now;
                }

                //if (DateTime.TryParseExact(_sbsm.DataOraRegistrazione,"dd/MM/yy HH:mm",CultureInfo.InvariantCulture,DateTimeStyles.None,out _dataora))
                if (DateTime.TryParse(_llsm.DataOraRegistrazione, out _dataora))
                {
                    return _dataora;
                }


                return DateTime.Now;

            }
        }

        // ------------------------------- TIMESTAMP -------------------


        public byte[] TimestampRegistrazione
        {
            get { return _llsm.TimestampRegistrazione; }
            set
            {
                _llsm.TimestampRegistrazione = value;
                _datiSalvati = false;
            }
        }

        public string strTimestamp
        {
            get { return FunzioniMR.StringaTimestamp(_llsm.TimestampRegistrazione); }
        }

        // ------------------------------- TENSIONI -------------------


        public int VBatt
        {
            get { return _llsm.VBatt; }
            set
            {
                _llsm.VBatt = value;
                _datiSalvati = false;
            }
        }

        public string strVBatt
        {
            get { return FunzioniMR.StringaTensione(_llsm.VBatt); }
        }

        // ------------------------------- CORRENTI -------------------
        // -- Media --
        public int IBatt
        {
            get { return _llsm.IBatt; }
            set
            {
                _llsm.IBatt = value;
                _datiSalvati = false;
            }
        }
        public string strIBatt
        {
            get { return FunzioniMR.StringaCorrente((short)_llsm.IBatt); }
        }
        public string olvIBatt
        {
            get
            {
                return FunzioniMR.StringaCorrenteOLV((short)_llsm.IBatt);
            }
        }

        // -- Minima --
        public int IBattMin
        {
            get { return _llsm.IBattMin; }
            set
            {
                _llsm.IBattMin = value;
                _datiSalvati = false;
            }
        }
        public string strIBattMin
        {
            get { return FunzioniMR.StringaCorrente((short)_llsm.IBattMin); }
        }
        public string olvIBattMin
        {
            get
            {
                return FunzioniMR.StringaCorrente((short)_llsm.IBattMin);
            }
        }

        // -- Massima --
        public int IBattMax
        {
            get { return _llsm.IBattMax; }
            set
            {
                _llsm.IBattMax = value;
                _datiSalvati = false;
            }
        }
        public string strIBattMax
        {
            get { return FunzioniMR.StringaCorrente((short)_llsm.IBattMax); }
        }
        public string olvIBattMaxMax
        {
            get
            {
                return FunzioniMR.StringaCorrenteOLV((short)_llsm.IBattMax);
            }
        }



        // ------------------------------- TEMPERATURE -------------------

        public sbyte TempBatt
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_llsm.TempBatt;
                return _tmpTemp;
            }
            set
            {
                _llsm.TempBatt = (byte)value;
                _datiSalvati = false;
            }
        }
        public string strTempBatt
        {
            get
            {
                if (_llsm.TempBatt == 0xFF) return "";
                sbyte _tmpTemp = (sbyte)_llsm.TempBatt;
                return FunzioniMR.StringaTemperatura(_tmpTemp);
            }
        }


        public sbyte TempIGBT1
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_llsm.TempIGBT1;
                return _tmpTemp;
            }
            set
            {
                _llsm.TempIGBT1 = (byte)value;
                _datiSalvati = false;
            }
        }
        public string strTempIGBT1
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_llsm.TempIGBT1;
                return FunzioniMR.StringaTemperatura(_tmpTemp);
            }
        }

        public sbyte TempIGBT2
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_llsm.TempIGBT2;
                return _tmpTemp;
            }
            set
            {
                _llsm.TempIGBT2 = (byte)value;
                _datiSalvati = false;
            }
        }
        public string strTempIGBT2
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_llsm.TempIGBT2;
                return FunzioniMR.StringaTemperatura(_tmpTemp);
            }
        }

        public sbyte TempIGBT3
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_llsm.TempIGBT3;
                return _tmpTemp;
            }
            set
            {
                _llsm.TempIGBT3 = (byte)value;
                _datiSalvati = false;
            }
        }
        public string strTempIGBT3
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_llsm.TempIGBT1;
                return FunzioniMR.StringaTemperatura(_tmpTemp);
            }
        }

        public sbyte TempIGBT4
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_llsm.TempIGBT4;
                return _tmpTemp;
            }
            set
            {
                _llsm.TempIGBT4 = (byte)value;
                _datiSalvati = false;
            }
        }
        public string strTempIGBT4
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_llsm.TempIGBT4;
                return FunzioniMR.StringaTemperatura(_tmpTemp);
            }
        }

        public sbyte TempDiode
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_llsm.TempDiode;
                return _tmpTemp;
            }
            set
            {
                _llsm.TempDiode = (byte)value;
                _datiSalvati = false;
            }
        }
        public string strTempDiode
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_llsm.TempDiode;
                return FunzioniMR.StringaTemperatura(_tmpTemp);
            }
        }



        public UInt32 VettoreErrori
        {
            get { return _llsm.VettoreErrori; }
            set
            {
                _llsm.VettoreErrori = value;
                _datiSalvati = false;
            }
        }


        public string strVettoreErrori
        {
            get
            {
                if (_llsm.VettoreErrori == 0)
                {
                    return "";
                }
                else
                {
                    return _llsm.VettoreErrori.ToString("X6");
                }
            }

        }


        public string strErrCalib
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000001) == 0x000001)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }

        public string strErrComm
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000002) == 0x000002)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }

        public string strErrVbatt
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000004) == 0x000004)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }

        public string strErrInt
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000008) == 0x000008)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }

        public string strErrSB1
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000010) == 0x000010)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }

        public string strErrFuse
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000020) == 0x000020)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }

        public string strErrAlim
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000040) == 0x000040)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }

        public string strErrIbat
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000080) == 0x000080)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }

        public string strErrStrappo
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000100) == 0x000100)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }
        public string strErrParam
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000200) == 0x000200)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }
        public string strErrParamSB
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000400) == 0x000400)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }
        public string strErrMemExt
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x000800) == 0x000800)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }

        }

        public string strErrNoInit
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x001000) == 0x001000)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }
        }

        public string strErrMaxSD
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x002000) == 0x002000)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }
        }

        public string strErrMaxIPK
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x004000) == 0x004000)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }
        }

        public string strErrPwHole
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x008000) == 0x008000)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }
        }

        public string strErrPwOff
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x010000) == 0x010000)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }
        }

        public string strErrTmr0
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x020000) == 0x020000)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }
        }

        public string strErrTmr1
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x040000) == 0x040000)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }
        }

        public string strErrDispPulse
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x080000) == 0x080000)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }
        }

        public string strErrPFC
        {
            get
            {
                if ((_llsm.VettoreErrori & 0x100000) == 0x100000)
                {
                    return "SI";
                }
                else
                {
                    return "";
                }
            }
        }

        public ushort DurataBreve
        {
            get { return _llsm.DurataBreve; }
            set
            {
                _llsm.DurataBreve = value;
                _datiSalvati = false;
            }
        }


        #endregion Class Parameter


    }
}
