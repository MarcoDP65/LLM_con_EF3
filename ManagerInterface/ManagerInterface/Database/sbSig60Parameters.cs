using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using log4net;
using log4net.Config;

using Utility;

namespace MoriData
{
    public class _sbSig60Parameters
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

        public byte OCBaudrate { get; set; }
        public byte ControlReg0 { get; set; }
        public byte ControlReg1 { get; set; }
        public byte ControlReg0_Err { get; set; }
        public byte ControlReg1_Err { get; set; }

        public uint NumLetture { get; set; }
        public uint NumErrori { get; set; }
        public uint NumInterferenze { get; set; }

        public byte Stato { get; set; }
        public byte DatiEstesi { get; set; }

        public DateTime IstanteLettura { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IstanteLettura.ToString();
        }
    }

    public class sbSig60Parameters
    {
        public string nullID { get { return "0000000000000000"; } }
        public _sbSig60Parameters _sbSig = new _sbSig60Parameters();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        private string _tempId;


        public sbSig60Parameters()
        {
            _sbSig = new _sbSig60Parameters();
            _sbSig.OCBaudrate = 0;
            _sbSig.DatiEstesi = 0;
            _sbSig.ControlReg0 = 0;
            _sbSig.ControlReg1 = 0;
            _sbSig.ControlReg0_Err = 0;
            _sbSig.ControlReg1_Err = 0;

            _sbSig.NumLetture = 0;
            _sbSig.NumErrori = 0;
            _sbSig.NumInterferenze = 0;
            _sbSig.OCBaudrate = 0;

            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbSig60Parameters(_db connessione)
        {
            valido = false;
            _sbSig = new _sbSig60Parameters();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _sbSig60Parameters _caricaDati(int _id)
        {
            return (from s in _database.Table<_sbSig60Parameters>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                _sbSig = _caricaDati(idLocale);
                if (_sbSig == null)
                {
                    _sbSig = new _sbSig60Parameters();
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
            get { return _sbSig.IdLocale; }
            set
            {
                if (value != null)
                {
                    _sbSig.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }
        public string IdApparato
        {
            get { return _sbSig.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbSig.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }
        public DateTime CreationDate
        {
            get { return _sbSig.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _sbSig.RevisionDate; }
        }

        public string LastUser
        {
            get { return _sbSig.LastUser; }
        }

        public DateTime IstanteLettura
        {
            get { return _sbSig.IstanteLettura; }
            set
            {
                _sbSig.IstanteLettura = value;
                _datiSalvati = false;
            }
        }

        /**********************************************************************************************/
        /*     -- parametri effettivi                                                                 */
        /**********************************************************************************************/


        public byte OCBaudrate
        {
            get { return _sbSig.OCBaudrate; }
            set
            {
                _sbSig.OCBaudrate = value;
                _datiSalvati = false;
            }
        }

        public byte ControlReg0
        {
            get { return _sbSig.ControlReg0; }
            set
            {
                _sbSig.ControlReg0 = value;
                _datiSalvati = false;
            }
        }

        public byte ControlReg1
        {
            get { return _sbSig.ControlReg1; }
            set
            {
                _sbSig.ControlReg1 = value;
                _datiSalvati = false;
            }
        }

        public byte ControlReg0_Err
        {
            get { return _sbSig.ControlReg0_Err; }
            set
            {
                _sbSig.ControlReg0_Err = value;
                _datiSalvati = false;
            }
        }

        public byte ControlReg1_Err
        {
            get { return _sbSig.ControlReg1_Err; }
            set
            {
                _sbSig.ControlReg1_Err = value;
                _datiSalvati = false;
            }
        }

        public bool DatiEstesi
        {
            get { return (_sbSig.DatiEstesi !=0); }
            set
            {
                if (value)
                {
                    _sbSig.DatiEstesi = 0xFF;
                }
                else
                {
                    _sbSig.DatiEstesi = 0x00;
                }

                _datiSalvati = false;
            }
        }


        public uint NumLetture
        {
            get { return _sbSig.NumLetture; }
            set
            {
                _sbSig.NumLetture = value;
                _datiSalvati = false;
            }
        }

        public uint NumErrori
        {
            get { return _sbSig.NumErrori; }
            set
            {
                _sbSig.NumErrori = value;
                _datiSalvati = false;
            }
        }

        public uint NumInterferenze
        {
            get { return _sbSig.NumInterferenze; }
            set
            {
                _sbSig.NumInterferenze = value;
                _datiSalvati = false;
            }
        }




        #endregion Class Parameter
    }
}
