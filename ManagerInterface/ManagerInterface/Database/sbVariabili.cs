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
using PannelloCharger;

namespace MoriData
{


    public class _sbVariabili
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

        public ushort TensioneTampone { get; set; }
        public ushort TensioneIstantanea { get; set; }
        public ushort Tensione1 { get; set; }
        public ushort Tensione2 { get; set; }
        public ushort Tensione3 { get; set; }

        public short CorrenteBatteria { get; set; }
        public int AhCaricati { get; set; }
        public int AhScaricati { get; set; }
        public byte TempNTC { get; set; }
        public byte PresenzaElettrolita { get; set; }
        public byte SoC { get; set; }
        public byte RF { get; set; }
        public UInt32 WhScaricati { get; set; }
        public UInt32 WhCaricati { get; set; }
        public byte MemProgrammed { get; set; }
        public byte ConnectionStatus { get; set; }
        public DateTime IstanteLettura { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IstanteLettura.ToString();
        }
    }


    public class sbVariabili
    {
        public string nullID { get { return "0000000000000000"; } }
        public _sbVariabili _sbPar = new _sbVariabili();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        private string _tempId;


        public sbVariabili()
        {
            _sbPar = new _sbVariabili();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbVariabili(_db connessione)
        {
            valido = false;
            _sbPar = new _sbVariabili(); 
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _sbVariabili _caricaDati(int _id)
        {
            return (from s in _database.Table<_sbVariabili>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                _sbPar = _caricaDati(idLocale);
                if (_sbPar == null)
                {
                    _sbPar = new _sbVariabili();
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
            get { return _sbPar.IdLocale; }
            set
            {
                if (value !=null )
                {
                    _sbPar.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }
        public string IdApparato
        {
            get { return _sbPar.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbPar.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }
         public DateTime CreationDate
        {
            get { return _sbPar.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _sbPar.RevisionDate; }
        }

        public string LastUser
        {
            get { return _sbPar.LastUser; }
        }

        public DateTime IstanteLettura
        {
            get { return _sbPar.IstanteLettura; }
            set
            {
                _sbPar.IstanteLettura = value;
                _datiSalvati = false;
            }
        }

        public ushort TensioneTampone
        {
            get { return _sbPar.TensioneTampone; }
            set
            {
                _sbPar.TensioneTampone = value;
                _datiSalvati = false;
            }
        }

        public string strTensioneTampone
        {
            get { return FunzioniMR.StringaTensione((ushort)(_sbPar.TensioneTampone * 10)); }
        }


        public ushort TensioneIstantanea
        {
            get { return _sbPar.TensioneIstantanea; }
            set
            {
                _sbPar.TensioneIstantanea = value;
                _datiSalvati = false;
            }
        }

        public string strTensioneIstantanea
        {
            get { return FunzioniMR.StringaTensione(_sbPar.TensioneIstantanea); }
        }

        public ushort Tensione1
        {
            get { return _sbPar.Tensione1; }
            set
            {
                _sbPar.Tensione1 = value;
                _datiSalvati = false;
            }
        }

        public string strTensione1
        {
            get { return FunzioniMR.StringaTensione(_sbPar.Tensione1); }
        }

        public ushort Tensione2
        {
            get { return _sbPar.Tensione2; }
            set
            {
                _sbPar.Tensione2 = value;
                _datiSalvati = false;
            }
        }
        public string strTensione2
        {
            get { return FunzioniMR.StringaTensione(_sbPar.Tensione2); }
        }

        public ushort Tensione3
        {
            get { return _sbPar.Tensione3; }
            set
            {
                _sbPar.Tensione3 = value;
                _datiSalvati = false;
            }
        }
        public string strTensione3
        {
            get { return FunzioniMR.StringaTensione(_sbPar.Tensione3); }
        }

        public short CorrenteBatteria
        {
            get { return _sbPar.CorrenteBatteria; }
            set
            {
                _sbPar.CorrenteBatteria = value;
                _datiSalvati = false;
            }
        }

        public string strCorrenteBatteria
        {
            get { return FunzioniMR.StringaCorrente(_sbPar.CorrenteBatteria,"0.0"); }
        }

        public int AhCaricati
        {
            get { return _sbPar.AhCaricati; }
            set
            {
                _sbPar.AhCaricati = value;
                _datiSalvati = false;
            }
        }

        public string strAhCaricati
        {
            get { return FunzioniMR.StringaCorrente((short)_sbPar.AhCaricati); }
        }

        public int AhScaricati
        {
            get { return _sbPar.AhScaricati; }
            set
            {
                _sbPar.AhScaricati = value;
                _datiSalvati = false;
            }
        }

        public string strAhScaricati
        {
            get { return FunzioniMR.StringaCorrente((short)_sbPar.AhScaricati); }
        }


        public byte TempNTC
        {
            get { return _sbPar.TempNTC; }
            set
            {
                _sbPar.TempNTC = value;
                _datiSalvati = false;
            }
        }

        public string strTempNTC
        {
            get { return FunzioniMR.StringaByteTemp(_sbPar.TempNTC); }
        }


        public byte PresenzaElettrolita
        {
            get { return _sbPar.PresenzaElettrolita; }
            set
            {
                _sbPar.PresenzaElettrolita = value;
                _datiSalvati = false;
            }
        }
        public string strPresenzaElettrolita
        {
            get { return FunzioniMR.StringaPresenza(_sbPar.PresenzaElettrolita); }
        }

        public byte SoC
        {
            get { return _sbPar.SoC; }
            set
            {
                _sbPar.SoC = value;
                _datiSalvati = false;
            }
        }

        public string strSoC
        {
            get { return FunzioniMR.StringaTemperatura(_sbPar.SoC); }
        }

        public byte RF
        {
            get { return _sbPar.RF; }
            set
            {
                _sbPar.RF = value;
                _datiSalvati = false;
            }
        }

        public string strRF
        {
            get { return FunzioniMR.StringaTemperatura((short)_sbPar.RF); }
        }

        public UInt32 WhScaricati
        {
            get { return _sbPar.WhScaricati; }
            set
            {
                _sbPar.WhScaricati = value;
                _datiSalvati = false;
            }
        }

        public string strWhScaricati
        {
            get { return FunzioniMR.StringaPotenza(_sbPar.WhScaricati,10); }
        }


        public UInt32 WhCaricati
        {
            get { return _sbPar.WhCaricati; }
            set
            {
                _sbPar.WhCaricati = value;
                _datiSalvati = false;
            }
        }

        public string strWhCaricati
        {
            get { return FunzioniMR.StringaPotenza(_sbPar.WhCaricati,10); }
        }

        public byte MemProgrammed
        {
            get { return _sbPar.MemProgrammed; }
            set
            {
                _sbPar.MemProgrammed = value;
                _datiSalvati = false;
            }
        }


        public string strMemProgrammed
        {
            get 
            { 
                string _StatoPrg;
                switch ( _sbPar.MemProgrammed )
                {
                    case 0:
                        _StatoPrg = StringheComuni.StatoPrg00; //"Programmazione NON ATTIVA";
                        break;
                    case 1:
                        _StatoPrg = StringheComuni.StatoPrg01; //"Programmazione ATTIVA; Registrazione NON ATTIVA";
                        break;
                    case 2:
                        _StatoPrg = StringheComuni.StatoPrg02; //"Programmazione ATTIVA; Registrazione ATTIVA";
                        break;
                    case 3:
                        _StatoPrg = StringheComuni.StatoPrg03; //"Calibrazione ATTIVA";
                        break;
                    default:
                        _StatoPrg = StringheComuni.StatoPrgND + " " + _sbPar.MemProgrammed.ToString("X2"); //"Stato NON DEFINITO (" + _sbPar.MemProgrammed.ToString("X2") + ")" ;
                        break;
                }
              
                return _StatoPrg; 
            }


        }

        public byte ConnectionStatus
        {
            get { return _sbPar.ConnectionStatus; }
            set
            {
                _sbPar.ConnectionStatus = value;
                _datiSalvati = false;
            }
        }


        #endregion Class Parameter


    }
}
