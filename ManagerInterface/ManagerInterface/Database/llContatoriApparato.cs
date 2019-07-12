
//    class llContatoriApparato

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
    public class _llContatoriApparato
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

        public byte[] DataPrimaCarica;
        public UInt32 CntCicliTotali;
        public UInt32 CntCicliStop;
        public UInt32 CntCicliStaccoBatt;
        public UInt32 CntCicliLess3H;
        public UInt32 CntCicli3Hto6H;
        public UInt32 CntCicli6Hto9H;
        public UInt32 CntCicliOver9H;
        public ushort CntProgrammazioni;

        public UInt32 CntCicliBrevi;
        public UInt32 PntNextBreve;

        public UInt32 CntCariche;
        public UInt32 PntNextCarica;

        public ushort CntMemReset;
        public byte[] DataUltimaCancellazione;

        //StringaDataTS(byte[] DataShort)

        public ushort CrcPacchetto { get; set; }

        public byte Stato { get; set; }

        public DateTime IstanteLettura { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IstanteLettura.ToString();
        }
    }

    public class llContatoriApparato
    {
        public string nullID { get { return "000000"; } }
        public _llContatoriApparato llContApp = new _llContatoriApparato();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("llContatoriApparato");
        public bool _datiSalvati;
        public bool _recordPresente;
        //private string _tempId;



        public llContatoriApparato()
        {
            llContApp = new _llContatoriApparato();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }


        public llContatoriApparato(_db connessione)
        {
            valido = false;
            llContApp = new _llContatoriApparato();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }



        private _llContatoriApparato _caricaDati(int _id)
        {
            return (from s in _database.Table<_llContatoriApparato>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                llContApp = _caricaDati(idLocale);
                if (llContApp == null)
                {
                    llContApp = new _llContatoriApparato();
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
            get { return llContApp.IdLocale; }
            set
            {
                if (value != null)
                {
                    llContApp.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }

        public string IdApparato
        {
            get
            {
                return llContApp.IdApparato;
            }

        }

        public byte[] DataPrimaCarica
        {
            get { return llContApp.DataPrimaCarica; }
            set
            {
                if (value != null)
                {
                    llContApp.DataPrimaCarica = value;
                    _datiSalvati = false;
                }
            }
        }


        public string strDataPrimaCarica
        {
            get
            {
                return FunzioniMR.StringaDataTS(llContApp.DataPrimaCarica);
            }

        }

        public uint CntCicliTotali
        {
            get { return llContApp.CntCicliTotali; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliTotali = value;
                    _datiSalvati = false;
                }
            }
        }


        
        public uint CntCicliStop
        {
            get { return llContApp.CntCicliStop; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliStop = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint CntCicliStaccoBatt
        {
            get { return llContApp.CntCicliStaccoBatt; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliStaccoBatt = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint CntCicliLess3H
        {
            get { return llContApp.CntCicliLess3H; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliLess3H = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint CntCicli3Hto6H
        {
            get { return llContApp.CntCicli3Hto6H; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicli3Hto6H = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint CntCicli6Hto9H
        {
            get { return llContApp.CntCicli6Hto9H; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicli6Hto9H = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint CntCicliOver9H
        {
            get { return llContApp.CntCicliOver9H; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliOver9H = value;
                    _datiSalvati = false;
                }
            }
        }

        public ushort CntProgrammazioni
        {
            get { return llContApp.CntProgrammazioni; }
            set
            {
                if (value != null)
                {
                    llContApp.CntProgrammazioni = value;
                    _datiSalvati = false;
                }
            }
        }


        public uint CntCicliBrevi
        {
            get { return llContApp.CntCicliBrevi; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliBrevi = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint PntNextBreve
        {
            get { return llContApp.PntNextBreve; }
            set
            {
                if (value != null)
                {
                    llContApp.PntNextBreve = value;
                    _datiSalvati = false;
                }
            }
        }
        public string strPntNextBreve
        {
            get { return "0x" + llContApp.PntNextBreve.ToString("X4"); }
        }

        public uint CntCariche
        {
            get { return llContApp.CntCariche; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCariche = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint PntNextCarica
        {
            get { return llContApp.PntNextCarica; }
            set
            {
                if (value != null)
                {
                    llContApp.PntNextCarica = value;
                    _datiSalvati = false;
                }
            }
        }
        public string strPntNextCarica
        {
            get { return "0x" + llContApp.PntNextCarica.ToString("X4"); }
        }

        public ushort CntMemReset
        {
            get { return llContApp.CntMemReset; }
            set
            {
                if (value != null)
                {
                    llContApp.CntMemReset = value;
                    _datiSalvati = false;
                }
            }
        }

        public byte[] DataUltimaCancellazione
        {
            get { return llContApp.DataUltimaCancellazione; }
            set
            {
                if (value != null)
                {
                    llContApp.DataUltimaCancellazione = value;
                    _datiSalvati = false;
                }
            }
        }


        public string strDataUltimaCancellazione
        {
            get
            {
                return FunzioniMR.StringaDataTS(llContApp.DataUltimaCancellazione);
            }

        }



        #endregion


    }

}





