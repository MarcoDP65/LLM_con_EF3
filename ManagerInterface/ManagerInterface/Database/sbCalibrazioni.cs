using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using log4net;
using log4net.Config;
using ChargerLogic;
using Utility;

namespace MoriData
{
    public class _sbCalibrazioni
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        public string IdApparato { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        //------------------------------- Dati Effettivi ---------------------
        public ushort AdcCurrentZero { get; set; }
        public ushort AdcCurrentPos { get; set; }
        public ushort AdcCurrentNeg { get; set; }
        public ushort CurrentPos { get; set; }
        public ushort CurrentNeg { get; set; }
        public ushort GainVbatt { get; set; }
        public ushort ValVbatt { get; set; }
        public ushort GainVbatt3 { get; set; }
        public ushort ValVbatt3 { get; set; }
        public ushort GainVbatt2 { get; set; }
        public ushort ValVbatt2 { get; set; }
        public ushort GainVbatt1 { get; set; }
        public ushort ValVbatt1 { get; set; }
        public DateTime IstanteLettura { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IstanteLettura.ToString();
        }
    }


    public class sbCalibrazioni
    {
        public string nullID { get { return "0000000000000000"; } }
        public _sbCalibrazioni _sbCal = new _sbCalibrazioni();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
       // private string _tempId;


        public sbCalibrazioni()
        {
            _sbCal = new _sbCalibrazioni();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbCalibrazioni(_db connessione)
        {
            valido = false;
            _sbCal = new _sbCalibrazioni();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _sbCalibrazioni _caricaDati(int _id)
        {
            return (from s in _database.Table<_sbCalibrazioni>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                _sbCal = _caricaDati(idLocale);
                if (_sbCal == null)
                {
                    _sbCal = new _sbCalibrazioni();
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
                /*
                if (_sbPar.IdApparato != nullID & _sbPar.IdApparato != null)
                {

                    _sbPar _TestDati = _caricaDati(_sbPar.IdApparato, _sbPar.IdMemoriaLunga, _sbPar.IdMemoriaBreve);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _sbPar.CreationDate = DateTime.Now;
                        _sbPar.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_sbPar);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _sbPar.IdLocale = _TestDati.IdLocale;
                        _sbPar.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_sbPar);
                        _datiSalvati = true;
                    }

                    //_database.InsertOrReplace(_sb);
                    return true;
                }
                else
                {
                    return false;
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

        #region Class Parameters

        public int IdLocale
        {
            get { return _sbCal.IdLocale; }
            set
            {
                //if (value != null)
                {
                    _sbCal.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }
        public string IdApparato
        {
            get { return _sbCal.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbCal.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }
        public DateTime CreationDate
        {
            get { return _sbCal.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _sbCal.RevisionDate; }
        }

        public string LastUser
        {
            get { return _sbCal.LastUser; }
        }

        public DateTime IstanteLettura
        {
            get { return _sbCal.IstanteLettura; }
            set
            {
                _sbCal.IstanteLettura = value;
                _datiSalvati = false;
            }
        }



        public ushort AdcCurrentZero
        {
            get { return _sbCal.AdcCurrentZero; }
            set
            {
                _sbCal.AdcCurrentZero = value;
                _datiSalvati = false;
            }
        }


        public ushort AdcCurrentPos
        {
            get { return _sbCal.AdcCurrentPos; }
            set
            {
                _sbCal.AdcCurrentPos = value;
                _datiSalvati = false;
            }
        }

        public ushort AdcCurrentNeg
        {
            get { return _sbCal.AdcCurrentNeg; }
            set
            {
                _sbCal.AdcCurrentNeg = value;
                _datiSalvati = false;
            }
        }

        public ushort CurrentPos
        {
            get { return _sbCal.CurrentPos; }
            set
            {
                _sbCal.CurrentPos = value;
                _datiSalvati = false;
            }
        }

        public ushort CurrentNeg
        {
            get { return _sbCal.CurrentNeg; }
            set
            {
                _sbCal.CurrentNeg = value;
                _datiSalvati = false;
            }
        }

        public ushort GainVbatt
        {
            get { return _sbCal.GainVbatt; }
            set
            {
                _sbCal.GainVbatt = value;
                _datiSalvati = false;
            }
        }

        public ushort ValVbatt
        {
            get { return _sbCal.ValVbatt; }
            set
            {
                _sbCal.ValVbatt = value;
                _datiSalvati = false;
            }
        }

        public ushort GainVbatt3
        {
            get { return _sbCal.GainVbatt3; }
            set
            {
                _sbCal.GainVbatt3 = value;
                _datiSalvati = false;
            }
        }

        public ushort ValVbatt3
        {
            get { return _sbCal.ValVbatt3; }
            set
            {
                _sbCal.ValVbatt3 = value;
                _datiSalvati = false;
            }
        }


        public ushort GainVbatt2
        {
            get { return _sbCal.GainVbatt2; }
            set
            {
                _sbCal.GainVbatt2 = value;
                _datiSalvati = false;
            }
        }

        public ushort ValVbatt2
        {
            get { return _sbCal.ValVbatt2; }
            set
            {
                _sbCal.ValVbatt2 = value;
                _datiSalvati = false;
            }
        }

        public ushort GainVbatt1
        {
            get { return _sbCal.GainVbatt1; }
            set
            {
                _sbCal.GainVbatt1 = value;
                _datiSalvati = false;
            }
        }

        public ushort ValVbatt1
        {
            get { return _sbCal.ValVbatt1; }
            set
            {
                _sbCal.ValVbatt1 = value;
                _datiSalvati = false;
            }
        }


/*
        public string strMemProgrammed
        {
            get
            {
                string _StatoPrg;
                switch (_sbPar.MemProgrammed)
                {
                    case 0:
                        _StatoPrg = "Programmazione NON ATTIVA";
                        break;
                    case 1:
                        _StatoPrg = "Programmazione ATTIVA; Registrazione NON ATTIVA";
                        break;
                    case 2:
                        _StatoPrg = "Programmazione ATTIVA; Registrazione ATTIVA";
                        break;
                    default:
                        _StatoPrg = "Stato NON DEFINITO (" + _sbPar.MemProgrammed.ToString("X2") + ")";
                        break;
                }

                return _StatoPrg;
            }
        }
*/
        #endregion Class Parameter


    }
}
