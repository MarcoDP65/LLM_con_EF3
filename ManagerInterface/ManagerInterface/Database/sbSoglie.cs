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

namespace MoriData
{
    /// <summary>
    /// _sbSoglie: struttura record per il salvataggio soglie per i report statistici
    /// </summary>
    public class _sbSoglie
    {
        [PrimaryKey, AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(30)]
        [Indexed(Name = "IDSoglia", Order = 1, Unique = true)]
        public string Username { get; set; }
        [MaxLength(24)]
        [Indexed(Name = "IDSoglia", Order = 2, Unique = true)]
        public string IdApparato { get; set; }
        [Indexed(Name = "IDSoglia", Order = 3, Unique = true)]
        public Int32 IdMisura { get; set; }
        /// <summary>
        /// Gets or sets the tipo misura.
        /// </summary>
        /// <value>
        /// Tipi possibili 0 = intero, 1 Float, 2 Stringa, 3 Data
        /// </value>
        public Int32 TipoMisura { get; set; }
        public Int32 ValoreInt { get; set; }
        public Double ValoreNum { get; set; }
        [MaxLength(20)]
        public string ValoreString { get; set; }
        public DateTime ValoreData{ get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        public string DataOraRegistrazione { get; set; }
    }

    public class DisplaySoglia

    {
        public Int32 IdMisura { get; set; }
        public Int32 TipoMisura { get; set; }
        public string DescMisura { get; set; }
        public Int32 ValoreInt { get; set; }
        public Double ValoreNum { get; set; }
        public string ValoreString { get; set; }
        public DateTime ValoreData { get; set; }
    }



    public class sbSoglia
    {
        public string nullID { get { return "0000000000000000"; } }
        public _sbSoglie _sbso = new _sbSoglie();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        private string _nomeSoglia = "";

        
        public sbSoglia()
        {
            _sbso = new _sbSoglie();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbSoglia(_db connessione)
        {
            valido = false;
            _sbso = new _sbSoglie(); 
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbSoglia(_db connessione, _sbSoglie Soglia)
        {
            valido = true;
            _sbso = Soglia;
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }



        private _sbSoglie _caricaDati(int _id)
        {
            return (from s in _database.Table<_sbSoglie>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        private _sbSoglie _caricaDati(string _IdApparato,string _IdUtente, Int32 _IdMisura)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_sbSoglie>()
                        where s.IdApparato == _IdApparato & s.IdMisura == _IdMisura 
                    select s).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                _sbso = _caricaDati(idLocale);
                if (_sbso == null)
                {
                    _sbso = new _sbSoglie();
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        #region PARAMETRI

        public Int32 IdLocale
        {
            get { return _sbso.IdLocale; }
            set
            {
                _sbso.IdLocale = value;
                _datiSalvati = false;
            }
        }

        public string Username 
        {
            get { return _sbso.Username; }
            set
            {
                _sbso.Username = value;
                _datiSalvati = false;
            }
        }

        public string IdApparato
        {
            get { return _sbso.IdApparato; }
            set
            {
                _sbso.IdApparato = value;
                _datiSalvati = false;
            }
        }

        public Int32 IdMisura

        {
            get { return _sbso.IdMisura; }
            set
            {
                _sbso.IdMisura = value;
                _datiSalvati = false;
            }
        }
        public string NomeSoglia
        {
            get { return _nomeSoglia; }
            set
            {
                _nomeSoglia = value;
            }
        }

        public Int32 TipoMisura
        {
            get { return _sbso.TipoMisura; }
            set
            {
                _sbso.TipoMisura = value;
                _datiSalvati = false;
            }
        }
        public Int32 ValoreInt
        {
            get { return _sbso.ValoreInt; }
            set
            {
                _sbso.ValoreInt = value;
                _datiSalvati = false;
            }
        }
        public Double ValoreNum
        {
            get { return _sbso.ValoreNum; }
            set
            {
                _sbso.ValoreNum = value;
                _datiSalvati = false;
            }
        }

        public string ValoreString
        {
            get { return _sbso.ValoreString; }
            set
            {
                _sbso.ValoreString = value;
                _datiSalvati = false;
            }
        }
        public DateTime ValoreData
        {
            get { return _sbso.ValoreData; }
            set
            {
                _sbso.ValoreData = value;
                _datiSalvati = false;
            }
        }
        public string strValoreSoglia
        {
            get
            {
                // 0 = intero, 1 Float, 2 Stringa, 3 Data

                switch (_sbso.TipoMisura)
                {
                    case 0: //Integer
                        return _sbso.ValoreInt.ToString();
                        
                    case 1: //Float
                        return _sbso.ValoreNum.ToString();
                        
                    case 2: //Stringa
                        return _sbso.ValoreString;
                        
                    case 3: //Data
                        return _sbso.ValoreData.ToString();
                        
                    default:
                        return "";
                }

                
            }

        }

        public DateTime RevisionDate
        {
            get { return _sbso.RevisionDate; }

        }

        public string LastUser
        {
            get { return _sbso.LastUser; }
        }
        public string DataOraRegistrazione
        {
            get { return _sbso.DataOraRegistrazione; }
        }


        #endregion

    }

    public class sbSetSoglie
    {
        public System.Collections.Generic.List<sbSoglia> PacchettoSoglie = new System.Collections.Generic.List<sbSoglia>();
        public string nullID { get { return "0000000000000000"; } }
        public _sbSoglie _sbso = new _sbSoglie();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;

 
        public sbSetSoglie()
        {
            PacchettoSoglie.Clear();
        }

        public bool caricaDefault(string Utente, string Dispositivo )
        {
            if (Utente == "")
                Utente = "#default#";
            if (Dispositivo == "")
                Dispositivo = "#base#";

            if (_database != null)
            {
                PacchettoSoglie.Clear();
                var lista =  from s in _database.Table<_sbSoglie>()
                                   where s.Username == Utente & s.IdApparato == Dispositivo
                             select s;

                sbDefSoglia LabelSoglia = new sbDefSoglia(_database);

                foreach ( _sbSoglie elemento in lista )
                {
                    sbSoglia Soglia = new sbSoglia(_database, elemento);
                    LabelSoglia.caricaDati(elemento.IdMisura);
                    Soglia.NomeSoglia = LabelSoglia.NomeSoglia;
                    PacchettoSoglie.Add(Soglia);
                }

                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Carica il set di soglie per l'utente / apparato corrente
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="User"></param>
        /// <param name="Apparato"></param>
        /// <returns></returns>
        public bool CaricaSoglie(MoriData._db DB, string User, string Apparato)
        {
            try
            {
                _database = DB;


                return caricaDefault("","");
            }
            catch
            {
                return false;
            }
        }

    }
}
