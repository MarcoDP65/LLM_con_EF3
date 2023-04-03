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


    public class _sbParametriGenerali
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }


        [MaxLength(24)]
        [Indexed(Name = "IDParametriGen", Order = 1, Unique = true)]
        public string IdApparato { get; set; }
        [Indexed(Name = "IDParametriGen", Order = 2, Unique = true)]
        public ushort Progressivo { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }

        public DateTime DataInserimento { get; set; }

        public ushort LettureCorrente { get; set; }
        public ushort LettureTensione { get; set; }
        public ushort DurataPausa { get; set; }
        public ushort CausaUltimoReset { get; set; }

        public override string ToString()
        {
            return IdApparato + ": Parametri Generali " + Progressivo.ToString() + " del " + DataInserimento.ToString();
        }
    }

    public class sbParametriGenerali
    {
        public string nullID { get { return "0000000000000000"; } }
        private _sbParametriGenerali _sbpgen;
        public bool valido { get; set; }
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        //private string _tempId;

        public int LivelloUser = 2;

        #region Class Init

        public sbParametriGenerali()
        {
            _sbpgen = new _sbParametriGenerali();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbParametriGenerali(_db connessione)
        {
            valido = false;
            _sbpgen = new _sbParametriGenerali();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        #endregion  Class Init

        #region Class Methods
        private _sbParametriGenerali _caricaDati(int _id)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_sbParametriGenerali>()
                        where s.IdLocale == _id
                        select s).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private ushort _ultimaEsecuzione(string _id)
        {
            if (_database != null)
            {
                _sbParametriGenerali _tempcal = (from s in _database.Table<_sbParametriGenerali>()
                                                   where s.IdApparato == _id
                                                   orderby s.DataInserimento ascending
                                                   select s).LastOrDefault();
                ushort _valore = 0;
                if (_tempcal != null)
                {
                    _valore = _tempcal.Progressivo;
                }
                return _valore;


            }
            else
            {
                return 0;
            }
        }

        private _sbParametriGenerali _caricaDati(string _IdApparato, ushort Progressivo)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_sbParametriGenerali>()
                        where s.IdApparato == _IdApparato && s.Progressivo == Progressivo
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
                // carico i dati testata lungo da db
                _sbpgen = _caricaDati(idLocale);
                if (_sbpgen == null)
                {
                    _sbpgen = new _sbParametriGenerali();
                    return false;
                }



                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool caricaDati(string IdApparato, ushort Progressivo)
        {
            try
            {
                _sbpgen = _caricaDati(IdApparato, Progressivo);
                if (_sbpgen == null)
                {
                    _sbpgen = new _sbParametriGenerali();
                    _sbpgen.IdApparato = IdApparato;
                    _sbpgen.Progressivo = Progressivo;
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
                if (_sbpgen.IdApparato != nullID & _sbpgen.IdApparato != null & _sbpgen.Progressivo == 0)
                {
                    int _progressivo = _ultimaEsecuzione(_sbpgen.IdApparato);
                    _sbpgen.Progressivo = (ushort)(_progressivo + 1);

                }

                if (_sbpgen.IdApparato != nullID & _sbpgen.IdApparato != null & _sbpgen.Progressivo != 0)
                {

                    _sbParametriGenerali _TestDati = _caricaDati(_sbpgen.IdApparato, _sbpgen.Progressivo);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _sbpgen.CreationDate = DateTime.Now;
                        _sbpgen.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_sbpgen);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _sbpgen.IdLocale = _TestDati.IdLocale;
                        _sbpgen.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_sbpgen);
                        _datiSalvati = true;
                    } 

                    return _datiSalvati;
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
        
        public bool cancellaDati()
        {
            try
            {
                SQLiteCommand CancellaRecord = _database.CreateCommand("delete from _sbParametriGenerali where IdApparato = ? and Progressivo = ? ", _sbpgen.IdApparato, _sbpgen.Progressivo);
                int esito = CancellaRecord.ExecuteNonQuery();
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        #endregion Class Methods

        #region Class Parameters

        public int IdLocale
        {
            get { return _sbpgen.IdLocale; }
            set
            {
               
                    _sbpgen.IdLocale = value;
                    _datiSalvati = false;
                
            }
        }

        public string IdApparato
        {
            get { return _sbpgen.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbpgen.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }

        public ushort Progressivo
        {
            get { return _sbpgen.Progressivo; }
            set
            {

                    _sbpgen.Progressivo = value;
                    _datiSalvati = false;
                
            }
        }

        public DateTime CreationDate
        {
            get { return _sbpgen.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _sbpgen.RevisionDate; }
        }

        public string LastUser
        {
            get { return _sbpgen.LastUser; }
        }



        public DateTime DataInserimento
        {
            get { return _sbpgen.DataInserimento; }
            set
            {
                if (value != null)
                {
                    _sbpgen.DataInserimento = value;
                    _datiSalvati = false;
                }
            }
        }


        public ushort LettureCorrente
        {
            get { return _sbpgen.LettureCorrente; }
            set
            {
                _sbpgen.LettureCorrente = value;
                _datiSalvati = false;
            }
        }

        public ushort LettureTensione
        {
            get { return _sbpgen.LettureTensione; }
            set
            {
                _sbpgen.LettureTensione = value;
                _datiSalvati = false;
            }
        }

        public ushort DurataPausa
        {
            get { return _sbpgen.DurataPausa; }
            set
            {
                _sbpgen.DurataPausa = value;
                _datiSalvati = false;
            }
        }

        public ushort CausaUltimoReset
        {
            get { return _sbpgen.CausaUltimoReset; }
            set
            {
                _sbpgen.CausaUltimoReset = value;
                _datiSalvati = false;
            }
        }

        #endregion Class Parameter




    }


}
