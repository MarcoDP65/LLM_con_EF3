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
        public string Attivita { get; set; }
        public string ElementiInteressati { get; set; }
        public int NumeroElementi { get; set; }

        public string Esito { get; set; }

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

                    return false;
                
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }



        #region Class Parameters
        /*

        public int IdLocale
        {
            get { return _sbsm.IdLocale; }
            set
            {
                if (value != null)
                {
                    _sbsm.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }
        public string IdApparato
        {
            get { return _sbsm.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbsm.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }
        public int IdMemoriaLunga
        {
            get { return _sbsm.IdMemoriaLunga; }
            set
            {
                if (value != null)
                {
                    _sbsm.IdMemoriaLunga = value;
                    _datiSalvati = false;
                }
            }
        }

        public int IdMemoriaBreve
        {
            get { return _sbsm.IdMemoriaBreve; }
            set
            {
                if (value != null)
                {
                    _sbsm.IdMemoriaBreve = value;
                    _datiSalvati = false;
                }
            }
        }
        public DateTime CreationDate
        {
            get { return _sbsm.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _sbsm.RevisionDate; }
        }

        public string LastUser
        {
            get { return _sbsm.LastUser; }
        }

        public string DataOraRegistrazione
        {
            get { return _sbsm.DataOraRegistrazione; }
            set
            {
                _sbsm.DataOraRegistrazione = value;
                _datiSalvati = false;
            }
        }

        public DateTime dtDataOraRegistrazione
        {
            get
            {
                DateTime _dataora;


                if (_sbsm.DataOraRegistrazione.Length != 15)
                {
                    // data non formattata correttamente
                    return DateTime.Now;
                }

                //if (DateTime.TryParseExact(_sbsm.DataOraRegistrazione,"dd/MM/yy HH:mm",CultureInfo.InvariantCulture,DateTimeStyles.None,out _dataora))
                if (DateTime.TryParse(_sbsm.DataOraRegistrazione, out _dataora))
                {
                    return _dataora;
                }


                return DateTime.Now;

            }
        }

        public int Vreg
        {
            get { return _sbsm.Vreg; }
            set
            {
                _sbsm.Vreg = value;
                _datiSalvati = false;
            }
        }

        public string strVreg
        {
            get { return FunzioniMR.StringaTensione(_sbsm.Vreg); }
        }

        // Tensioni Assolute

        public int V1
        {
            get { return _sbsm.V1; }
            set
            {
                _sbsm.V1 = value;
                _datiSalvati = false;
            }
        }

        public string strV1
        {
            get { return FunzioniMR.StringaTensione(_sbsm.V1); }
        }

        public int V2
        {
            get { return _sbsm.V2; }
            set
            {
                _sbsm.V2 = value;
                _datiSalvati = false;
            }
        }
        public string strV2
        {
            get { return FunzioniMR.StringaTensione(_sbsm.V2); }
        }

        public int V3
        {
            get { return _sbsm.V3; }
            set
            {
                _sbsm.V3 = value;
                _datiSalvati = false;
            }
        }
        public string strV3
        {
            get { return FunzioniMR.StringaTensione(_sbsm.V3); }
        }

        // Tensioni Assolute per cella

        public float Vc1
        {
            get { return _sbsm.Vc1; }
            set
            {
                _sbsm.Vc1 = value;
                _datiSalvati = false;
            }
        }
        public string strVc1
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vc1); }
        }


        public float Vc2
        {
            get { return _sbsm.Vc2; }
            set
            {
                _sbsm.Vc2 = value;
                _datiSalvati = false;
            }
        }
        public string strVc2
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vc2); }
        }

        public float Vc3
        {
            get { return _sbsm.Vc3; }
            set
            {
                _sbsm.Vc3 = value;
                _datiSalvati = false;
            }
        }
        public string strVc3
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vc3); }
        }



        public float VcBatt
        {
            get { return _sbsm.VcBatt; }
            set
            {
                _sbsm.VcBatt = value;
                _datiSalvati = false;
            }
        }
        public string strVcBatt
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.VcBatt); }
        }

        // Tensioni relative di sezione per cella

        public float Vcs1
        {
            get { return _sbsm.Vcs1; }
            set
            {
                _sbsm.Vcs1 = value;
                _datiSalvati = false;
            }
        }
        public string strVcs1
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vcs1); }
        }


        public float Vcs2
        {
            get { return _sbsm.Vcs2; }
            set
            {
                _sbsm.Vcs2 = value;
                _datiSalvati = false;
            }
        }
        public string strVcs2
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vcs2); }
        }

        public float Vcs3
        {
            get { return _sbsm.Vcs3; }
            set
            {
                _sbsm.Vcs3 = value;
                _datiSalvati = false;
            }
        }
        public string strVcs3
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.Vcs3); }
        }



        public float VcsBatt
        {
            get { return _sbsm.VcsBatt; }
            set
            {
                _sbsm.VcsBatt = value;
                _datiSalvati = false;
            }
        }
        public string strVcsBatt
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.VcsBatt); }
        }


        // Massimo sbilanciamento: riferito alle Tensioni relative di sezione per cella

        public float MaxSbil
        {
            get { return _sbsm.MaxSbil; }
            set
            {
                _sbsm.MaxSbil = value;
                _datiSalvati = false;
            }
        }
        public string strMaxSbil
        {
            get { return FunzioniMR.StringaTensioneCella(_sbsm.MaxSbil); }
        }


        public int Amed
        {
            get { return _sbsm.Amed; }
            set
            {
                _sbsm.Amed = value;
                _datiSalvati = false;
            }
        }

        public string strAmed
        {
            get { return FunzioniMR.StringaCorrente((short)_sbsm.Amed); }
        }
        public string olvAmed
        {
            get
            {
                if (VersoScarica == elementiComuni.VersoCorrente.Diretto)
                {
                    return FunzioniMR.StringaCorrenteOLV((short)_sbsm.Amed);
                }
                else
                {
                    return FunzioniMR.StringaCorrenteOLV((short)-_sbsm.Amed);
                }
            }
        }

        public int Amin
        {
            get { return _sbsm.Amin; }
            set
            {
                _sbsm.Amin = value;
                _datiSalvati = false;
            }
        }

        public string strAmin
        {
            get { return FunzioniMR.StringaCorrente((short)_sbsm.Amin); }
        }
        public string olvAmin
        {
            get
            {
                if (VersoScarica == elementiComuni.VersoCorrente.Diretto)
                {
                    return FunzioniMR.StringaCorrenteOLV((short)_sbsm.Amin);
                }
                else
                {
                    return FunzioniMR.StringaCorrenteOLV((short)-_sbsm.Amax);
                }
            }
        }


        public int Amax
        {
            get { return _sbsm.Amax; }
            set
            {
                _sbsm.Amax = value;
                _datiSalvati = false;
            }
        }

        public string strAmax
        {
            get { return FunzioniMR.StringaCorrente((short)_sbsm.Amax); }
        }
        public string olvAmax
        {
            get
            {
                if (VersoScarica == elementiComuni.VersoCorrente.Diretto)
                {
                    return FunzioniMR.StringaCorrenteOLV((short)_sbsm.Amax);
                }
                else
                {
                    return FunzioniMR.StringaCorrenteOLV((short)-_sbsm.Amin);
                }
            }
        }

        public int Tntc
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_sbsm.Tntc;
                return _tmpTemp;
            }
            set
            {
                _sbsm.Tntc = (byte)value;
                _datiSalvati = false;
            }
        }

        public string strTemp
        {
            get
            {
                sbyte _tmpTemp = (sbyte)_sbsm.Tntc;
                return FunzioniMR.StringaTemperatura(_tmpTemp);
            }
        }


        public byte PresenzaElettrolita
        {
            get { return _sbsm.PresenzaElettrolita; }
            set
            {
                _sbsm.PresenzaElettrolita = value;
                _datiSalvati = false;
            }
        }

        public byte VbatBk
        {
            get { return _sbsm.VbatBk; }
            set
            {
                _sbsm.VbatBk = value;
                _datiSalvati = false;
            }
        }
        public string strVbatBk
        {
            get { return FunzioniMR.StringaTensione(_sbsm.VbatBk * 10); }
        }
        */
        #endregion Class Parameter


    }
}
