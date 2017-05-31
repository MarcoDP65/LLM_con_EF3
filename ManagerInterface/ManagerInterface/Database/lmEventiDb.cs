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
    public class _lmEventiDb
    {
        [PrimaryKey, AutoIncrement]
        public Int32 IdRecord { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        public string LastUser { get; set; }

        public string VersioneManager { get; set; }
        public int IdAttivita { get; set; }
        public string Attivita { get; set; }
        public string ElementiInteressati { get; set; }
        public int NumeroElementi { get; set; }

        public int Esito { get; set; }
        public string DescEsito { get; set; }

    }

    public class lmEventiDb
    {

        public _lmEventiDb _lmDb = new _lmEventiDb();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        private string _tempId;


        public lmEventiDb()
        {
            _lmDb = new _lmEventiDb();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public lmEventiDb(_lmEventiDb _dati)
        {
            _lmDb = _dati;
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public lmEventiDb(_db connessione)
        {
            valido = false;
            _lmDb = new _lmEventiDb();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _lmEventiDb _caricaDati(int _id)
        {
            return (from s in _database.Table<_lmEventiDb>()
                    where s.IdRecord == _id
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int IdRecord)
        {
            try
            {
                _lmDb = _caricaDati(IdRecord);
                if (_lmDb == null)
                {
                    _lmDb = new _lmEventiDb();
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
                if ( _lmDb != null )
                {

                    _lmEventiDb _TestDati = _caricaDati(_lmDb.IdRecord);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _lmDb.CreationDate = DateTime.Now;
                        _lmDb.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_lmDb);
                        _datiSalvati = (_result == 1);
                    }
                    else
                    {
                        _lmDb.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_lmDb);
                        _datiSalvati = (_result == 1);
                    }

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


        #region Class Parameters
    

        public int IdRecord
        {
            get { return _lmDb.IdRecord; }
            set
            {

                _lmDb.IdRecord = value;
                _datiSalvati = false;

            }
        }


        public DateTime CreationDate
        {
            get { return _lmDb.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _lmDb.RevisionDate; }
        }

        public string LastUser
        {
            get { return _lmDb.LastUser; }
        }

        public string VersioneManager
        {
            get { return _lmDb.VersioneManager; }
            set
            {
                _lmDb.VersioneManager = value;
                _datiSalvati = false;
            }
        }

        public string ElementiInteressati
        {
            get { return _lmDb.ElementiInteressati; }
            set
            {
                _lmDb.ElementiInteressati = value;
                _datiSalvati = false;
            }
        }

        public int IdAttivita
        {
            get { return _lmDb.IdAttivita; }
            set
            {

                _lmDb.IdAttivita = value;
                _datiSalvati = false;

            }
        }


        public string Attivita
        {
            get { return _lmDb.Attivita; }
            set
            {
                _lmDb.Attivita = value;
                _datiSalvati = false;
            }
        }

        public int NumeroElementi
        {
            get { return _lmDb.NumeroElementi; }
            set
            {
                _lmDb.NumeroElementi = value;
                _datiSalvati = false;
            }
        }

        public string strNumeroElementi
        {
            get { return FunzioniMR.StringaTensione(_lmDb.NumeroElementi); }
        }

        public string DescEsito
        {
            get { return _lmDb.DescEsito; }
            set
            {
                _lmDb.DescEsito = value;
                _datiSalvati = false;
            }
        }

        #endregion Class Parameter


    }
}
