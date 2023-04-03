
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


    public class _llVariabili
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

        public Int32 Lettura { get; set; }
        public ushort TensioneTampone { get; set; }
        public ushort TensioneIstantanea { get; set; }
        public ushort CorrenteIstantanea { get; set; }
        public uint AhCaricati { get; set; }
        public uint SecondsFromStart { get; set; }
        public byte StatoLL { get; set; }
        public DateTime IstanteLettura { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IstanteLettura.ToString();
        }
    }


    public class llVariabili
    {
        public string nullID { get { return "0000000000000000"; } }
        public _llVariabili _llPar = new _llVariabili();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        //private string _tempId;


        public llVariabili()
        {
            _llPar = new _llVariabili();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public llVariabili(_db connessione)
        {
            valido = false;
            _llPar = new _llVariabili();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _llVariabili _caricaDati(int _id)
        {
            return (from s in _database.Table<_llVariabili>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                _llPar = _caricaDati(idLocale);
                if (_llPar == null)
                {
                    _llPar = new _llVariabili();
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
            get { return _llPar.IdLocale; }
            set
            {

                    _llPar.IdLocale = value;
                    _datiSalvati = false;

            }
        }
        public string IdApparato
        {
            get { return _llPar.IdApparato; }
            set
            {
                if (value != null)
                {
                    _llPar.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }
        public DateTime CreationDate
        {
            get { return _llPar.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _llPar.RevisionDate; }
        }

        public string LastUser
        {
            get { return _llPar.LastUser; }
        }

        public DateTime IstanteLettura
        {
            get { return _llPar.IstanteLettura; }
            set
            {
                _llPar.IstanteLettura = value;
                _datiSalvati = false;
            }
        }

        public int Lettura
        {
            get { return _llPar.Lettura; }
            set
            {

                    _llPar.Lettura = value;
                    _datiSalvati = false;
                
            }
        }

        public string strLettura
        {
            get { return _llPar.Lettura.ToString(); }
        }


        public String strOraLettura
        {
            get
            {
                return _llPar.IstanteLettura.ToString("HH:mm:ss");
            }

        }


        public ushort TensioneTampone
        {
            get { return _llPar.TensioneTampone; }
            set
            {
                _llPar.TensioneTampone = value;
                _datiSalvati = false;
            }
        }

        public string strTensioneTampone
        {
            get { return FunzioniMR.StringaTensione((ushort)(_llPar.TensioneTampone * 10)); }
        }


        public ushort TensioneIstantanea
        {
            get { return _llPar.TensioneIstantanea; }
            set
            {
                _llPar.TensioneIstantanea = value;
                _datiSalvati = false;
            }
        }

        public string strTensioneIstantanea
        {
            get { return FunzioniMR.StringaTensioneCella(_llPar.TensioneIstantanea); }
        }


        public ushort CorrenteIstantanea
        {
            get { return _llPar.CorrenteIstantanea; }
            set
            {
                _llPar.CorrenteIstantanea = value;
                _datiSalvati = false;
            }
        }

        /// <summary>
        /// Rotorna il valore attuale della corrente in formato stringa con 1 decimale
        /// </summary>
        public string strCorrenteIstantanea
        {
            get { return FunzioniMR.StringaCorrenteLL(_llPar.CorrenteIstantanea); }
        }

        public uint AhCaricati
        {
            get { return _llPar.AhCaricati; }
            set
            {
                _llPar.AhCaricati = value;
                _datiSalvati = false;
            }
        }

        public string strAhCaricati
        {
            get { return FunzioniMR.StringaCapacitaUint(_llPar.AhCaricati,100,2); }
        }



        public uint SecondsFromStart
        {
            get { return _llPar.SecondsFromStart; }
            set
            {
                _llPar.SecondsFromStart = value;
                _datiSalvati = false;
            }
        }

        public string strSecondsFromStart
        {
            get { return FunzioniMR.StringaDurataFull(_llPar.SecondsFromStart); }
        }


        public byte StatoLL
        {
            get { return _llPar.StatoLL; }
            set
            {
                _llPar.StatoLL = value;
                _datiSalvati = false;
            }
        }

        public string strStatoLL
        {
            get
            {
                string _StatoPrg;
                switch (_llPar.StatoLL)
                {
                    case 0:
                        _StatoPrg = "Fase 0";
                        break;
                    case 1:
                        _StatoPrg = "Fase 1";
                        break;
                    case 2:
                        _StatoPrg = "Fase 2";
                        break;
                    case 3:
                        _StatoPrg = "Fase 3";
                        break;
                    default:
                        _StatoPrg = "N.D. (" + _llPar.StatoLL.ToString("X2") + ")";
                        break;
                }

                return _StatoPrg;
            }
        }

        #endregion Class Parameter


    }
}
