using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using log4net;
using log4net.Config;


namespace MoriData
{
    public class _sbDefSoglia
    {
        [PrimaryKey]
        public Int32 IdLocale { get; set; }
        public Int32 IdSoglia { get; set; }
        public Int32 IdLingua { get; set; }

        [MaxLength(50)]
        public string DescSoglia { get; set; }
        /// <summary>
        /// Gets or sets il tipo dati della misura.
        /// </summary>
        /// <value>
        /// Tipi possibili 0 = intero, 1 Float, 2 Stringa, 3 Data
        /// </value>
        public Int32 TipoMisura { get; set; }
    }

    public class sbDefSoglia
    {
        public _sbDefSoglia _sbdso = new _sbDefSoglia();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;

        
        public sbDefSoglia()
        {
            _sbdso = new _sbDefSoglia();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbDefSoglia(_db connessione)
        {
            valido = false;
            _sbdso = new _sbDefSoglia(); 
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _sbDefSoglia _caricaDati(int _id)
        {
            return (from s in _database.Table<_sbDefSoglia>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }



        public bool caricaDati(int idLocale)
        {
            try
            {
                _sbdso = _caricaDati(idLocale);
                if (_sbdso == null)
                {
                    _sbdso = new _sbDefSoglia();
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

        public Int32 IdMisura

        {
            get { return _sbdso.IdSoglia; }
        }

        public string NomeSoglia
        {
            get { return _sbdso.DescSoglia; }

        }
        #endregion PARAMETRI




    }
}
