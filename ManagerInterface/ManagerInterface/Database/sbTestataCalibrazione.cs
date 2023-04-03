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
    public class ValoriCalibrazione
    {
        public enum EsitoCalibrazione : byte { OK = 0xF0, KO = 0x0F, NotExecuted = 0, Failed = 0xFF };

    }

    public class _sbTestataCalibrazione
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }


        [MaxLength(24)]
        [Indexed(Name = "IDCalibrazione", Order = 1, Unique = true)]
        public string IdApparato { get; set; }
        [Indexed(Name = "IDCalibrazione", Order = 2, Unique = true)]
        public ushort IdEsecuzione { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }


        public string Descrizione { get; set; }
        public DateTime DataEsecuzione { get; set; }
        public string FirmwareAttivo { get; set; }
        public string Operatore { get; set; }
        public ushort Progressivo { get; set; }
        public byte Esito { get; set; }
        public float ErroreMax { get; set; }
        public float ErroreMaxPos { get; set; }
        public float ErroreMaxNeg { get; set; }

        public int Vbat { get; set; }
        public int V1 { get; set; }
        public int V2 { get; set; }
        public int V3 { get; set; }

        public byte Tntc { get; set; }
        public byte PresenzaElettrolita { get; set; }
        public int Spire { get; set; }
        public int CorrenteMax { get; set; }

        public byte EsitoCalibrazione { get; set; }

        public ushort AdcCurrentZero { get; set; }
        public ushort AdcCurrentPos { get; set; }
        public ushort AdcCurrentNeg { get; set; }
        public ushort CurrentPos { get; set; }
        public ushort CurrentNeg { get; set; }


        public override string ToString()
        {
            return IdApparato + ": Calibrazione " + Progressivo.ToString() + " del " + DataEsecuzione.ToString();
        }
    }

    class sbTestataCalibrazione
    {
        public string nullID { get { return "0000000000000000"; } }
        private _sbTestataCalibrazione _sbtc;
        public bool valido { get; set; }
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        private string _tempId;
        private System.Collections.Generic.List<_sbAnalisiCorrente> LettureCorrenteDB = new System.Collections.Generic.List<_sbAnalisiCorrente>();
        public System.Collections.Generic.List<sbAnalisiCorrente> LettureCorrente = new System.Collections.Generic.List<sbAnalisiCorrente>();
        public bool breviDaAgiornare = false;

        public int LivelloUser = 2;

        #region Class Init

        public sbTestataCalibrazione()
        {
            _sbtc = new _sbTestataCalibrazione();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbTestataCalibrazione(_db connessione)
        {
            valido = false;
            _sbtc = new _sbTestataCalibrazione();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        #endregion  Class Init

        #region Class Methods
        private _sbTestataCalibrazione _caricaDati(int _id)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_sbTestataCalibrazione>()
                        where s.IdLocale == _id
                        select s).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private int _ultimaEsecuzione(string _id )
        {
            if (_database != null)
            {
                _sbTestataCalibrazione _tempcal = (from s in _database.Table<_sbTestataCalibrazione>()
                        where s.IdApparato == _id
                        orderby s.IdEsecuzione ascending
                        select s).LastOrDefault();
                int _valore = 0;
                if (_tempcal != null)
                {
                    _valore = _tempcal.IdEsecuzione;
                }
                return _valore;


            }
            else
            {
                return 0;
            }
        }

        private _sbTestataCalibrazione _caricaDati(string _IdApparato, ushort _IdEsecuzione)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_sbTestataCalibrazione>()
                        where s.IdApparato == _IdApparato && s.IdEsecuzione == _IdEsecuzione
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
                _sbtc = _caricaDati(idLocale);
                if (_sbtc == null)
                {
                    _sbtc = new _sbTestataCalibrazione();
                    return false;
                }
                else
                {
                    // se la testata è salvata, carico le misure

                    CaricaLetture();

                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool caricaDati(string IdApparato, ushort IdEsecuzione)
        {
            try
            {
                _sbtc = _caricaDati(IdApparato, IdEsecuzione);
                if (_sbtc == null)
                {
                    _sbtc = new _sbTestataCalibrazione();
                    _sbtc.IdApparato = IdApparato;
                    _sbtc.IdEsecuzione = IdEsecuzione;
                    return false;
                }
                else
                {
                    CaricaLetture();
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
                if (_sbtc.IdApparato != nullID & _sbtc.IdApparato != null & _sbtc.IdEsecuzione == 0)
                {
                    int _progressivo = _ultimaEsecuzione(_sbtc.IdApparato);
                    _sbtc.IdEsecuzione = (ushort)(_progressivo + 1);

                }

                if (_sbtc.IdApparato != nullID & _sbtc.IdApparato != null & _sbtc.IdEsecuzione != 0)
                {

                    _sbTestataCalibrazione _TestDati = _caricaDati(_sbtc.IdApparato, _sbtc.IdEsecuzione);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _sbtc.CreationDate = DateTime.Now;
                        _sbtc.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_sbtc);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _sbtc.IdLocale = _TestDati.IdLocale;
                        _sbtc.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_sbtc);
                        _datiSalvati = true;
                    }

                    if(_datiSalvati)
                    {
                        _datiSalvati = SalvaLetture();
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

        public bool CaricaLetture()
        {
            try
            {

                LettureCorrente.Clear();

                IEnumerable<_sbAnalisiCorrente> _TempCicli = _database.Query<_sbAnalisiCorrente>("select * from _sbAnalisiCorrente where IdApparato = ? and IdEsecuzione = ? order by Lettura ", _sbtc.IdApparato, _sbtc.IdEsecuzione);

                foreach (_sbAnalisiCorrente Elemento in _TempCicli)
                {
                    sbAnalisiCorrente _cLoc;
                    _cLoc = new sbAnalisiCorrente(Elemento);
                    LettureCorrente.Add(_cLoc);

                }


                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaLetture " + Ex.Message);
                return false;
            }
        }

        public bool SalvaLetture()
        {
            try
            {
                DateTime _start = DateTime.Now;
                Log.Debug("Start SalvaLetture ");



                LettureCorrenteDB.Clear();
                SQLiteCommand CancellaCicli = _database.CreateCommand("delete from _sbAnalisiCorrente where IdApparato = ? and IdEsecuzione = ? ", _sbtc.IdApparato, _sbtc.IdEsecuzione);
                int esito = CancellaCicli.ExecuteNonQuery();
                Log.Debug("Letture cancellate da db");


                foreach (sbAnalisiCorrente Elemento in LettureCorrente)
                {
                    Elemento.IdApparato = _sbtc.IdApparato;
                    Elemento.IdEsecuzione = _sbtc.IdEsecuzione;
                    _sbAnalisiCorrente _cLoc;
                    _cLoc = Elemento._sbAC;
                    LettureCorrenteDB.Add(_cLoc);
                }
                Log.Debug("Letture pronte");


                int _result = _database.InsertAll(LettureCorrenteDB);
                Log.Debug("Letture salvate su db");

                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("SalvaLetture " + Ex.Message);
                return false;
            }
        }

        public bool CancellaLetture()
        {
            try
            {
                SQLiteCommand CancellaCicli = _database.CreateCommand("delete from _sbAnalisiCorrente where IdApparato = ? and IdEsecuzione = ? ", _sbtc.IdApparato, _sbtc.IdEsecuzione);
                int esito = CancellaCicli.ExecuteNonQuery();
                CaricaLetture();

                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("CancellaLetture " + Ex.Message);
                return false;
            }
        }

        public bool cancellaDati()
        {
            try
            {
                SQLiteCommand CancellaRecord = _database.CreateCommand("delete from _sbTestataCalibrazione where IdApparato = ? and IdEsecuzione = ? ", _sbtc.IdApparato, _sbtc.IdEsecuzione);
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
            get { return _sbtc.IdLocale; }
            set
            {

                    _sbtc.IdLocale = value;
                    _datiSalvati = false;
                
            }
        }

        public string IdApparato
        {
            get { return _sbtc.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbtc.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }

        public ushort IdEsecuzione
        {
            get { return _sbtc.IdEsecuzione; }
            set
            {
                if (value != null)
                {
                    _sbtc.IdEsecuzione = value;
                    _datiSalvati = false;
                }
            }
        }

        public DateTime CreationDate
        {
            get { return _sbtc.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _sbtc.RevisionDate; }
        }
        
        public string LastUser
        {
            get { return _sbtc.LastUser; }
        }


        public string Descrizione
        {
            get { return _sbtc.Descrizione; }
            set
            {
                _sbtc.Descrizione = value;
                _datiSalvati = false;
            }
        }

        public DateTime DataEsecuzione
        {
            get { return _sbtc.DataEsecuzione; }
            set
            {
                if (value != null)
                {
                    _sbtc.DataEsecuzione = value;
                    _datiSalvati = false;
                }
            }
        }

        public string FirmwareAttivo
        {
            get { return _sbtc.FirmwareAttivo; }
            set
            {
                _sbtc.FirmwareAttivo = value;
                _datiSalvati = false;
            }
        }


        public string Operatore
        {
            get { return _sbtc.Operatore; }
            set
            {
                _sbtc.Operatore = value;
                _datiSalvati = false;
            }
        }

        public ushort Progressivo
        {
            get { return _sbtc.Progressivo; }
            set
            {
                _sbtc.Progressivo = value;
                _datiSalvati = false;
            }
        }

        public byte Esito
        {
            get { return _sbtc.Esito; }
            set
            {
                _sbtc.Esito = value;
                _datiSalvati = false;
            }
        }

        public float ErroreMax
        {
            get { return _sbtc.ErroreMax; }
            set
            {
                _sbtc.ErroreMax = value;
                _datiSalvati = false;
            }
        }

        public float ErroreMaxPos
        {
            get { return _sbtc.ErroreMaxPos; }
            set
            {
                _sbtc.ErroreMaxPos = value;
                _datiSalvati = false;
            }
        }

        public float ErroreMaxNeg
        {
            get { return _sbtc.ErroreMaxNeg; }
            set
            {
                _sbtc.ErroreMaxNeg = value;
                _datiSalvati = false;
            }
        }

        public int Vbat
        {
            get { return _sbtc.Vbat; }
            set
            {
                _sbtc.Vbat = value;
                _datiSalvati = false;
            }
        }

        public int V3
        {
            get { return _sbtc.V3; }
            set
            {
                _sbtc.V3 = value;
                _datiSalvati = false;
            }
        }

        public int V2
        {
            get { return _sbtc.V2; }
            set
            {
                _sbtc.V2 = value;
                _datiSalvati = false;
            }
        }

        public int V1
        {
            get { return _sbtc.V1; }
            set
            {
                _sbtc.V1 = value;
                _datiSalvati = false;
            }
        }

        public int Spire
        {
            get { return _sbtc.Spire; }
            set
            {
                _sbtc.Spire = value;
                _datiSalvati = false;
            }
        }

        public ushort AdcCurrentZero
        {
            get { return _sbtc.AdcCurrentZero; }
            set
            {
                _sbtc.AdcCurrentZero = value;
                _datiSalvati = false;
            }
        }

        public ushort AdcCurrentPos
        {
            get { return _sbtc.AdcCurrentPos; }
            set
            {
                _sbtc.AdcCurrentPos = value;
                _datiSalvati = false;
            }
        }

        public ushort AdcCurrentNeg
        {
            get { return _sbtc.AdcCurrentNeg; }
            set
            {
                _sbtc.AdcCurrentNeg = value;
                _datiSalvati = false;
            }
        }

        public ushort CurrentPos
        {
            get { return _sbtc.CurrentPos; }
            set
            {
                _sbtc.CurrentPos = value;
                _datiSalvati = false;
            }
        }

        public ushort CurrentNeg
        {
            get { return _sbtc.CurrentNeg; }
            set
            {
                _sbtc.CurrentNeg = value;
                _datiSalvati = false;
            }
        }

        public int CorrenteMax
        {
            get { return _sbtc.CorrenteMax; }
            set
            {
                _sbtc.CorrenteMax = value;
                _datiSalvati = false;
            }
        }

        #endregion Class Parameter




    }


}







