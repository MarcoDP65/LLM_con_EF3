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

using Utility;

namespace MoriData
{
    public class _llStatoFirmware
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
        public string RevDisplay { get; set; }
        public ushort CRCFirmware { get; set; }
        public uint AddrFlash0 { get; set; }
        public uint LenFlash0 { get; set; }
        public uint AddrFlash1 { get; set; }
        public uint LenFlash1 { get; set; }
        public uint AddrFlash2 { get; set; }
        public uint LenFlash2 { get; set; }
        public uint AddrFlash3 { get; set; }
        public uint LenFlash3 { get; set; }
        public uint AddrFlash4 { get; set; }
        public uint LenFlash4 { get; set; }

        public byte Stato { get; set; }

        public DateTime IstanteLettura { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IstanteLettura.ToString();
        }
    }

    public class llStatoFirmware
    {
        public string nullID { get { return "0000000000000000"; } }
        public _llStatoFirmware _llSFW = new _llStatoFirmware();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        //private string _tempId;



        public llStatoFirmware()
        {
            _llSFW = new _llStatoFirmware();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }


        public llStatoFirmware(_db connessione)
        {
            valido = false;
            _llSFW = new _llStatoFirmware();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }



        private _llStatoFirmware _caricaDati(int _id)
        {
            return (from s in _database.Table<_llStatoFirmware>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                _llSFW = _caricaDati(idLocale);
                if (_llSFW == null)
                {
                    _llSFW = new _llStatoFirmware();
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
            get { return _llSFW.IdLocale; }
            set
            {
                if (value != null)
                {
                    _llSFW.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }
        public string IdApparato
        {
            get { return _llSFW.IdApparato; }
            set
            {
                if (value != null)
                {
                    _llSFW.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }



        public string RevBootloader
        {
            get { return _llSFW.RevBootloader; }
            set
            {
                _llSFW.RevBootloader = value;
                _datiSalvati = false;
            }
        }
        public string strRevBootloader
        {
            get { return FunzioniMR.StringaRevisione(_llSFW.RevBootloader); }
        }

        public string RevDisplay
        {
            get { return _llSFW.RevDisplay; }
            set
            {
                _llSFW.RevDisplay = value;
                _datiSalvati = false;
            }
        }
        public string strRevDisplay
        {
            get { return FunzioniMR.StringaRevisione(_llSFW.RevDisplay); }
        }



        public string RevFirmware
        {
            get { return _llSFW.RevFirmware; }
            set
            {
                _llSFW.RevFirmware = value;
                _datiSalvati = false;
            }
        }
        public string strRevFirmware
        {
            get { return FunzioniMR.StringaRevisione(_llSFW.RevFirmware); }
        }


        public ushort CRCFirmware
        {
            get { return _llSFW.CRCFirmware; }
            set
            {
                _llSFW.CRCFirmware = value;
                _datiSalvati = false;
            }
        }
        public string strCRCFirmware
        {
            get { return _llSFW.CRCFirmware.ToString("X4"); }
        }

        //---------------------------------------------------------------------
        public uint AddrFlash0
        {
            get { return _llSFW.AddrFlash0; }
            set
            {
                _llSFW.AddrFlash0 = value;
                _datiSalvati = false;
            }
        }
        public string strAddrFlash0
        {
            get { return _llSFW.AddrFlash0.ToString("X8"); }
        }


        public uint LenFlash0
        {
            get { return _llSFW.LenFlash0; }
            set
            {
                _llSFW.LenFlash0 = value;
                _datiSalvati = false;
            }
        }
        public string strLenFlash0
        {
            get { return _llSFW.LenFlash0.ToString("X8"); }
        }

        //---------------------------------------------------------------------
        public uint AddrFlash1
        {
            get { return _llSFW.AddrFlash1; }
            set
            {
                _llSFW.AddrFlash1 = value;
                _datiSalvati = false;
            }
        }
        public string strAddrFlash1
        {
            get { return _llSFW.AddrFlash1.ToString("X8"); }
        }

        public uint LenFlash1
        {
            get { return _llSFW.LenFlash1; }
            set
            {
                _llSFW.LenFlash1 = value;
                _datiSalvati = false;
            }
        }
        public string strLenFlash1
        {
            get { return _llSFW.LenFlash1.ToString("X8"); }
        }

        //---------------------------------------------------------------------
        public uint AddrFlash2
        {
            get { return _llSFW.AddrFlash2; }
            set
            {
                _llSFW.AddrFlash2 = value;
                _datiSalvati = false;
            }
        }
        public string strAddrFlash2
        {
            get { return _llSFW.AddrFlash2.ToString("X8"); }
        }

        public uint LenFlash2
        {
            get { return _llSFW.LenFlash2; }
            set
            {
                _llSFW.LenFlash2 = value;
                _datiSalvati = false;
            }
        }
        public string strLenFlash2
        {
            get { return _llSFW.LenFlash2.ToString("X8"); }
        }

        //---------------------------------------------------------------------
        public uint AddrFlash3
        {
            get { return _llSFW.AddrFlash3; }
            set
            {
                _llSFW.AddrFlash3 = value;
                _datiSalvati = false;
            }
        }
        public string strAddrFlash3
        {
            get { return _llSFW.AddrFlash3.ToString("X8"); }
        }

        public uint LenFlash3
        {
            get { return _llSFW.LenFlash3; }
            set
            {
                _llSFW.LenFlash3 = value;
                _datiSalvati = false;
            }
        }
        public string strLenFlash3
        {
            get { return _llSFW.LenFlash3.ToString("X8"); }
        }

        //---------------------------------------------------------------------
        public uint AddrFlash4
        {
            get { return _llSFW.AddrFlash4; }
            set
            {
                _llSFW.AddrFlash4 = value;
                _datiSalvati = false;
            }
        }
        public string strAddrFlash4
        {
            get { return _llSFW.AddrFlash4.ToString("X8"); }
        }

        public uint LenFlash4
        {
            get { return _llSFW.LenFlash4; }
            set
            {
                _llSFW.LenFlash4 = value;
                _datiSalvati = false;
            }
        }
        public string strLenFlash4
        {
            get { return _llSFW.LenFlash4.ToString("X8"); }
        }

        //---------------------------------------------------------------------

        public byte Stato
        {
            get { return _llSFW.Stato; }
            set
            {
                _llSFW.Stato = value;
                _datiSalvati = false;
            }
        }
        public string strStato
        {
            get { return _llSFW.Stato.ToString("X"); }
        }


        #endregion


    }

}

