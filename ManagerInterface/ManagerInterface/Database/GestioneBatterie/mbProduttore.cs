
//    class mbProduttore


using System;
using System.Runtime;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using System.Globalization;

using log4net;
using log4net.Config;

using ChargerLogic;
using Utility;


namespace MoriData

{
    public class _mbProduttore
    {
        [PrimaryKey]
        public int IdProduttore { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        //-------------------------------------------------------------

        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public Byte Attivo { get; set; }


    }

    public class mbProduttore
    {

        public ushort nullID { get { return 0x0000; } }
        public _mbProduttore _mbProd = new _mbProduttore();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("mbProduttore");
        public bool _datiSalvati;
        public bool _recordPresente;

        public mbProduttore()
        {
            _mbProd = new _mbProduttore();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public mbProduttore(_mbProduttore _dati)
        {
            _mbProd = _dati;
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public mbProduttore(_db connessione)
        {
            valido = false;
            _mbProd = new _mbProduttore();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _mbProduttore _caricaDati(int _id)
        {
            return (from s in _database.Table<_mbProduttore>()
                    where s.IdProduttore == _id
                    select s).FirstOrDefault();
        }

        public bool CaricaDati(int idLocale)
        {
            try
            {
                _mbProd = _caricaDati(idLocale);
                if (_mbProd == null)
                {
                    _mbProd = new _mbProduttore();
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
                if (_mbProd.IdProduttore != 0)
                {

                    _mbProduttore _TestDati = _caricaDati(_mbProd.IdProduttore);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _mbProd.CreationDate = DateTime.Now;
                        _mbProd.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_mbProd);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _mbProd.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_mbProd);
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


        #region Class Parameters

        public int IdProduttore
        {
            get { return _mbProd.IdProduttore; }
            set
            {

                _mbProd.IdProduttore = value;
                _datiSalvati = false;

            }
        }

        public string Nome
        {
            get { return _mbProd.Nome; }
            set
            {

                _mbProd.Nome = value;
                _datiSalvati = false;

            }
        }

        public string Descrizione
        {
            get { return _mbProd.Descrizione; }
            set
            {

                _mbProd.Descrizione = value;
                _datiSalvati = false;

            }
        }
        #endregion

    }
}

