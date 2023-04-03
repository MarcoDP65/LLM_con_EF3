
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
    public class _mbTecnologia
    {
        [PrimaryKey]
        public ushort IdTecnologia { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        //-------------------------------------------------------------

        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public Byte Attivo { get; set; }

        public int DivisoreCelle { get; set; }

    }

    public class mbTecnologia
    {

        public ushort nullID { get { return 0x0000; } }
        public _mbTecnologia _mbTech = new _mbTecnologia();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("mbTecnologia");
        public bool _datiSalvati;
        public bool _recordPresente;

        public mbTecnologia()
        {
            _mbTech = new _mbTecnologia();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public mbTecnologia(_mbTecnologia _dati)
        {
            _mbTech = _dati;
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public mbTecnologia(_db connessione)
        {
            valido = false;
            _mbTech = new _mbTecnologia();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _mbTecnologia _caricaDati(ushort _id)
        {
            return (from s in _database.Table<_mbTecnologia>()
                    where s.IdTecnologia == _id
                    select s).FirstOrDefault();
        }

        public bool CaricaDati(ushort idLocale)
        {
            try
            {
                _mbTech = _caricaDati(idLocale);
                if (_mbTech == null)
                {
                    _mbTech = new _mbTecnologia();
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
                if (_mbTech.IdTecnologia != 0)
                {

                    _mbTecnologia _TestDati = _caricaDati(_mbTech.IdTecnologia);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _mbTech.CreationDate = DateTime.Now;
                        _mbTech.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_mbTech);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _mbTech.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_mbTech);
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

        public ushort IdTecnologia
        {
            get { return _mbTech.IdTecnologia; }
            set
            {

                _mbTech.IdTecnologia = value;
                _datiSalvati = false;

            }
        }

        public string Nome
        {
            get { return _mbTech.Nome; }
            set
            {

                _mbTech.Nome = value;
                _datiSalvati = false;

            }
        }

        public string Descrizione
        {
            get { return _mbTech.Descrizione; }
            set
            {

                _mbTech.Descrizione = value;
                _datiSalvati = false;

            }
        }
        #endregion

    }
}



