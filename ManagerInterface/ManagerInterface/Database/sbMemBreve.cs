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
    public class _sbMemBreve
    {
        [PrimaryKey,AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        [Indexed(Name = "IDMemBreve", Order = 1, Unique = true)]
        public string IdApparato { get; set; }
        [Indexed(Name = "IDMemBreve", Order = 2, Unique = true)]
        public Int32 IdMemoriaLunga { get; set; }
        [Indexed(Name = "IDMemBreve", Order = 3, Unique = true)]
        public Int32 IdMemoriaBreve { get; set; }   
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        public string DataOraRegistrazione { get; set; }
        public int Vreg { get; set; }
        public int V1 { get; set; }
        public int V2 { get; set; }
        public int V3 { get; set; }
        public int Amed { get; set; }
        public int Amin { get; set; }
        public int Amax { get; set; }
        public byte Tntc { get; set; }
        public byte PresenzaElettrolita { get; set; }
        public byte VbatBk { get; set; }
        public float Vc1 { get; set; }
        public float Vc2 { get; set; }
        public float Vc3 { get; set; }
        public float VcBatt { get; set; }
        public float MaxSbil { get; set; }
        public float Vcs1 { get; set; }
        public float Vcs2 { get; set; }
        public float Vcs3 { get; set; }
        public float VcsBatt { get; set; }
    }

    public class sbMemBreve
    {

        public string nullID { get { return "0000000000000000"; } }
        public _sbMemBreve _sbsm = new _sbMemBreve();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        //private string _tempId;
        public tensioniIntermedie ValoriIntermedi = new tensioniIntermedie();
        public elementiComuni.VersoCorrente VersoScarica = elementiComuni.VersoCorrente.Diretto;

        public sbMemBreve()
        {
            _sbsm = new _sbMemBreve();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbMemBreve(_sbMemBreve _dati)
        {
            _sbsm = _dati;
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbMemBreve(_db connessione)
        {
            valido = false;
            _sbsm = new _sbMemBreve(); 
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _sbMemBreve _caricaDati(int _id)
        {
            return (from s in _database.Table<_sbMemBreve>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        private _sbMemBreve _caricaDati(string _IdApparato, Int32 _IdMemoriaLunga, Int32 _IdMemoriaBreve)
        {
            return (from s in _database.Table<_sbMemBreve>()
                    where s.IdApparato == _IdApparato & s.IdMemoriaLunga == _IdMemoriaLunga & s.IdMemoriaBreve == _IdMemoriaBreve
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                _sbsm = _caricaDati(idLocale);
                if (_sbsm == null)
                {
                    _sbsm = new _sbMemBreve();
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool caricaDati(string IdApparato, int IdMemoriaLunga, int IdMemoriaBreve)
        {
            try
            {
                _sbsm = _caricaDati(IdApparato, IdMemoriaLunga, IdMemoriaBreve);
                if (_sbsm == null)
                {
                    _sbsm = new _sbMemBreve();
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
                if (_sbsm.IdApparato != nullID & _sbsm.IdApparato != null & _sbsm.IdMemoriaLunga != null)
                {

                    _sbMemBreve _TestDati = _caricaDati(_sbsm.IdApparato, _sbsm.IdMemoriaLunga, _sbsm.IdMemoriaBreve);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _sbsm.CreationDate = DateTime.Now;
                        _sbsm.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_sbsm);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _sbsm.IdLocale = _TestDati.IdLocale;
                        _sbsm.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_sbsm);
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
                /*
                FunzioniComuni.SplitSShort((short)_sbsm.Amed, ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;

                FunzioniComuni.SplitSShort((short)_sbsm.Amin, ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;

                FunzioniComuni.SplitSShort((short)_sbsm.Amax, ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;

    */


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

                return _datamap;
            }

        }





        #region Class Parameters

        public int IdLocale
        {
            get { return _sbsm.IdLocale; }
            set
            {
                if (value !=null )
                {
                    _sbsm.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }
        public string IdApparato
        {
            get { return _sbsm.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbsm.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }
        public int IdMemoriaLunga
        {
            get { return _sbsm.IdMemoriaLunga; }
            set
            {
                if (value != null)
                {
                    _sbsm.IdMemoriaLunga = value;
                    _datiSalvati = false;
                }
            }
        }

        public int IdMemoriaBreve
        {
            get { return _sbsm.IdMemoriaBreve; }
            set
            {
                if (value != null)
                {
                    _sbsm.IdMemoriaBreve = value;
                    _datiSalvati = false;
                }
            }
        }
        public DateTime CreationDate
        {
            get { return _sbsm.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _sbsm.RevisionDate; }
        }

        public string LastUser
        {
            get { return _sbsm.LastUser; }
        }

        public string DataOraRegistrazione
        {
            get { return _sbsm.DataOraRegistrazione; }
            set
            {
                _sbsm.DataOraRegistrazione = value;
                _datiSalvati = false;
            }
        }

        public DateTime dtDataOraRegistrazione
        {
            get 
            {
                DateTime _dataora;


                if(_sbsm.DataOraRegistrazione.Length != 15)
                {
                    // data non formattata correttamente
                    return DateTime.Now; 
                }

                //if (DateTime.TryParseExact(_sbsm.DataOraRegistrazione,"dd/MM/yy HH:mm",CultureInfo.InvariantCulture,DateTimeStyles.None,out _dataora))
                if (DateTime.TryParse(_sbsm.DataOraRegistrazione,  out _dataora))
                    {
                    return _dataora;
                }


                return  DateTime.Now; 

            }
        }

        public int Vreg
        {
            get { return _sbsm.Vreg; }
            set
            {
                _sbsm.Vreg = value;
                _datiSalvati = false;
            }
        }

        public string strVreg
        {
            get { return FunzioniMR.StringaTensione(_sbsm.Vreg); }
        }

        // Tensioni Assolute

        public int V1
        {
            get { return _sbsm.V1; }
            set
            {
                _sbsm.V1 = value;
                _datiSalvati = false;
            }
        }

        public string strV1
        {
            get { return FunzioniMR.StringaTensione(_sbsm.V1); }
        }

        public int V2
        {
            get { return _sbsm.V2; }
            set
            {
                _sbsm.V2 = value;
                _datiSalvati = false;
            }
        }
        public string strV2
        {
            get { return FunzioniMR.StringaTensione(_sbsm.V2); }
        }

        public int V3
        {
            get { return _sbsm.V3; }
            set
            {
                _sbsm.V3 = value;
                _datiSalvati = false;
            }
        }
        public string strV3
        {
            get { return FunzioniMR.StringaTensione(_sbsm.V3); }
        }

        // Tensioni Assolute per cella

        public float Vc1
        {
            get { return _sbsm.Vc1; }
            set
            {
                _sbsm.Vc1 = value;
                _datiSalvati = false;
            }
        }
        public string strVc1
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vc1); }    
        }


        public float Vc2
        {
            get { return _sbsm.Vc2; }
            set
            {
                _sbsm.Vc2 = value;
                _datiSalvati = false;
            }
        }
        public string strVc2
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vc2); }
        }

        public float Vc3
        {
            get { return _sbsm.Vc3; }
            set
            {
                _sbsm.Vc3 = value;
                _datiSalvati = false;
            }
        }
        public string strVc3
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vc3); }
        }



        public float VcBatt
        {
            get { return _sbsm.VcBatt; }
            set
            {
                _sbsm.VcBatt = value;
                _datiSalvati = false;
            }
        }
        public string strVcBatt
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.VcBatt); }
        }

        // Tensioni relative di sezione per cella

        public float Vcs1
        {
            get { return _sbsm.Vcs1; }
            set
            {
                _sbsm.Vcs1 = value;
                _datiSalvati = false;
            }
        }
        public string strVcs1
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vcs1); }
        }


        public float Vcs2
        {
            get { return _sbsm.Vcs2; }
            set
            {
                _sbsm.Vcs2 = value;
                _datiSalvati = false;
            }
        }
        public string strVcs2
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vcs2); }
        }

        public float Vcs3
        {
            get { return _sbsm.Vcs3; }
            set
            {
                _sbsm.Vcs3 = value;
                _datiSalvati = false;
            }
        }
        public string strVcs3
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vcs3); }
        }



        public float VcsBatt
        {
            get { return _sbsm.VcsBatt; }
            set
            {
                _sbsm.VcsBatt = value;
                _datiSalvati = false;
            }
        }
        public string strVcsBatt
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.VcsBatt); }
        }


        // Massimo sbilanciamento: riferito alle Tensioni relative di sezione per cella

        public float MaxSbil
        {
            get { return _sbsm.MaxSbil; }
            set
            {
                _sbsm.MaxSbil = value;
                _datiSalvati = false;
            }
        }
        public string strMaxSbil
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.MaxSbil); }
        }


        public int Amed
        {
            get { return _sbsm.Amed; }
            set
            {
                _sbsm.Amed = value;
                _datiSalvati = false;
            }
        }

        public string strAmed
        {
            get { return FunzioniMR.StringaCorrente((short)_sbsm.Amed); }
        }
        public string olvAmed
        {
            get 
            {
                if (VersoScarica == elementiComuni.VersoCorrente.Diretto)
                {
                    return FunzioniMR.StringaCorrenteOLV((short)_sbsm.Amed);
                }
                else
                {
                    return FunzioniMR.StringaCorrenteOLV((short) - _sbsm.Amed);
                }
            }
        }

        public int Amin
        {
            get { return _sbsm.Amin; }
            set
            {
                _sbsm.Amin = value;
                _datiSalvati = false;
            }
        }

        public string strAmin
        {
            get { return FunzioniMR.StringaCorrente((short)_sbsm.Amin); }
        }
        public string olvAmin
        {
            get 
            {
                if (VersoScarica == elementiComuni.VersoCorrente.Diretto)
                {
                    return FunzioniMR.StringaCorrenteOLV((short)_sbsm.Amin);
                }
                else
                {
                    return FunzioniMR.StringaCorrenteOLV((short)-_sbsm.Amax);
                }
            }
        }


        public int Amax
        {
            get { return _sbsm.Amax; }
            set
            {
                _sbsm.Amax = value;
                _datiSalvati = false;
            }
        }

        public string strAmax
        {
            get { return FunzioniMR.StringaCorrente((short)_sbsm.Amax); }
        }
        public string olvAmax
        {
            get
            {
                if (VersoScarica == elementiComuni.VersoCorrente.Diretto)
                {
                    return FunzioniMR.StringaCorrenteOLV((short)_sbsm.Amax );
                }
                else
                {
                    return FunzioniMR.StringaCorrenteOLV((short)-_sbsm.Amin);
                }
            }
        }

        public int Tntc
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_sbsm.Tntc;
                return _tmpTemp;
            }
            set
            {
                _sbsm.Tntc = (byte)value;
                _datiSalvati = false;
            }
        }

        public string strTemp
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_sbsm.Tntc;
                return FunzioniMR.StringaTemperatura(_tmpTemp);
            }
        }


        public byte PresenzaElettrolita
        {
            get { return _sbsm.PresenzaElettrolita; }
            set
            {
                _sbsm.PresenzaElettrolita = value;
                _datiSalvati = false;
            }
        }

        public byte VbatBk
        {
            get { return _sbsm.VbatBk; }
            set
            {
                _sbsm.VbatBk = value;
                _datiSalvati = false;
            }
        }
        public string strVbatBk
        {
            get { return FunzioniMR.StringaTensione(_sbsm.VbatBk * 10); }
        }

        #endregion Class Parameter


    }
}
