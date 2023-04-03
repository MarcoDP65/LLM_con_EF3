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
    public class _sbStatoFirmware
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

        public string RevBootloader { get; set; }
        public string RevFirmware { get; set; }
        public ushort CRCFirmware { get; set; }
        public uint AddrFlash { get; set; }
        public uint LenFlash { get; set; }
        public uint AddrFlash2 { get; set; }
        public uint LenFlash2 { get; set; }
        public uint AddrProxy { get; set; }
        public uint LenProxy { get; set; }
        public byte Stato { get; set; }

        public DateTime IstanteLettura { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IstanteLettura.ToString();
        }
    }

    public class sbStatoFirmware
    {
        public string nullID { get { return "0000000000000000"; } }
        public _sbStatoFirmware _sbSFW = new _sbStatoFirmware();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        //private string _tempId;



        public sbStatoFirmware()
        {
            _sbSFW = new _sbStatoFirmware();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }


        public sbStatoFirmware(_db connessione)
        {
            valido = false;
            _sbSFW = new _sbStatoFirmware();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }



        private _sbStatoFirmware _caricaDati(int _id)
        {
            return (from s in _database.Table<_sbStatoFirmware>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                _sbSFW = _caricaDati(idLocale);
                if (_sbSFW == null)
                {
                    _sbSFW = new _sbStatoFirmware();
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
            get { return _sbSFW.IdLocale; }
            set
            {
                if (value != null)
                {
                    _sbSFW.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }
        public string IdApparato
        {
            get { return _sbSFW.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbSFW.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }



        public string RevBootloader
        {
            get { return _sbSFW.RevBootloader; }
            set
            {
                _sbSFW.RevBootloader = value;
                _datiSalvati = false;
            }
        }
        public string strRevBootloader
        {
            get { return FunzioniMR.StringaRevisione( _sbSFW.RevBootloader); }
        }


        public string RevFirmware
        {
            get { return _sbSFW.RevFirmware; }
            set
            {
                _sbSFW.RevFirmware = value;
                _datiSalvati = false;
            }
        }
        public string strRevFirmware
        {
            get { return FunzioniMR.StringaRevisione(_sbSFW.RevFirmware); }
        }


        public ushort CRCFirmware
        {
            get { return _sbSFW.CRCFirmware; }
            set
            {
                _sbSFW.CRCFirmware = value;
                _datiSalvati = false;
            }
        }
        public string strCRCFirmware
        {
            get { return _sbSFW.CRCFirmware.ToString("X4"); }
        }

        public uint AddrFlash
        {
            get { return _sbSFW.AddrFlash; }
            set
            {
                _sbSFW.AddrFlash = value;
                _datiSalvati = false;
            }
        }
        public string strAddrFlash
        {
            get { return _sbSFW.AddrFlash.ToString("X8"); }
        }


        public uint LenFlash
        {
            get { return _sbSFW.LenFlash; }
            set
            {
                _sbSFW.LenFlash = value;
                _datiSalvati = false;
            }
        }
        public string strLenFlash
        {
            get { return _sbSFW.LenFlash.ToString("X8"); }
        }


        public uint AddrFlash2
        {
            get { return _sbSFW.AddrFlash2; }
            set
            {
                _sbSFW.AddrFlash2 = value;
                _datiSalvati = false;
            }
        }
        public string strAddrFlash2
        {
            get { return _sbSFW.AddrFlash2.ToString("X8"); }
        }

        public uint LenFlash2
        {
            get { return _sbSFW.LenFlash2; }
            set
            {
                _sbSFW.LenFlash2 = value;
                _datiSalvati = false;
            }
        }
        public string strLenFlash2
        {
            get { return _sbSFW.LenFlash2.ToString("X8"); }
        }

        public uint AddrProxy
        {
            get { return _sbSFW.AddrProxy; }
            set
            {
                _sbSFW.AddrProxy = value;
                _datiSalvati = false;
            }
        }
        public string strAddrProxy
        {
            get { return _sbSFW.AddrProxy.ToString("X8"); }
        }

        public uint LenProxy
        {
            get { return _sbSFW.LenProxy; }
            set
            {
                _sbSFW.LenProxy = value;
                _datiSalvati = false;
            }
        }
        public string strLenProxy
        {
            get { return _sbSFW.LenProxy.ToString("X4"); }
        }

        public byte Stato
        {
            get { return _sbSFW.Stato; }
            set
            {
                _sbSFW.Stato = value;
                _datiSalvati = false;
            }
        }
        public string strStato
        {
            get { return _sbSFW.Stato.ToString("X"); }
        }


        #endregion


    }

}
