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
    public class _sbAnalisiCorrente
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        public string IdApparato { get; set; }
        public uint Progressivo { get; set; }

        public ushort IdEsecuzione { get; set; }
        public uint Ciclo { get; set; }
        public uint Lettura { get; set; }
        public uint Spire { get; set; }
        public DateTime IstanteLettura { get; set; }
        public float Ateorici { get; set; }
        public float Areali { get; set; }
        public float Aspybatt { get; set; }
        public float AspybattAP { get; set; }
        public float AspybattDP { get; set; }
        public float AspybattAN { get; set; }
        public float AspybattDN { get; set; }

    }

    public class sbAnalisiCorrente
    {
        public string nullID { get { return "0000000000000000"; } }
        public _sbAnalisiCorrente _sbAC = new _sbAnalisiCorrente();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        //private string _tempId;


        public sbAnalisiCorrente()
        {
            _sbAC = new _sbAnalisiCorrente();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbAnalisiCorrente(_db connessione)
        {
            valido = false;
            _sbAC = new _sbAnalisiCorrente();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbAnalisiCorrente(_sbAnalisiCorrente _dati)
        {
            _sbAC = _dati;
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _sbAnalisiCorrente _caricaDati(int _id)
        {
            return (from s in _database.Table<_sbAnalisiCorrente>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                _sbAC = _caricaDati(idLocale);
                if (_sbAC == null)
                {
                    _sbAC = new _sbAnalisiCorrente();
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
            get { return _sbAC.IdLocale; }
            set
            {
                //if (value != null)
                {
                    _sbAC.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }
        public string IdApparato
        {
            get { return _sbAC.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbAC.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }


        /*

        DateTime IstanteLettura { get; set; }
        float Ateorici { get; set; }
        float Areali { get; set; }
        float Aspybatt { get; set; }

        */

        public ushort IdEsecuzione
        {
            get { return _sbAC.IdEsecuzione; }
            set
            {
                _sbAC.IdEsecuzione = value;
                _datiSalvati = false;
            }
        }

        public uint Progressivo
        {
            get { return _sbAC.Progressivo; }
            set
            {
                _sbAC.Progressivo = value;
                _datiSalvati = false;
            }
        }
        public string strProgressivo
        {
            get { return _sbAC.Progressivo.ToString(); }
        }

        public uint Ciclo
        {
            get { return _sbAC.Ciclo; }
            set
            {
                _sbAC.Ciclo = value;
                _datiSalvati = false;
            }
        }
        public string strCiclo
        {
            get { return _sbAC.Ciclo.ToString(); }
        }

        public uint Lettura
        {
            get { return _sbAC.Lettura; }
            set
            {
                _sbAC.Lettura = value;
                _datiSalvati = false;
            }
        }
        public string strLettura
        {
            get { return _sbAC.Lettura.ToString(); }
        }

        public uint Spire
        {
            get { return _sbAC.Spire; }
            set
            {
                _sbAC.Spire = value;
                _datiSalvati = false;
            }
        }
        public string strSpire
        {
            get { return _sbAC.Spire.ToString(); }
        }


        public float Ateorici
        {
            get { return _sbAC.Ateorici; }
            set
            {
                _sbAC.Ateorici = value;
                _datiSalvati = false;
            }
        }
        public string strAteorici
        {
            get { return _sbAC.Ateorici.ToString("0.0"); }
        }


        public DateTime IstanteLettura
        {
            get { return _sbAC.IstanteLettura; }
            set
            {
                _sbAC.IstanteLettura = value;
                _datiSalvati = false;
            }
        }
        public string strIstanteLettura
        {
            get { return _sbAC.IstanteLettura.ToString("0.0"); }
        }


        public float Areali
        {
            get { return _sbAC.Areali; }
            set
            {
                _sbAC.Areali = value;
                _datiSalvati = false;
            }
        }
        public string strAreali
        {
            get { return _sbAC.Areali.ToString("0.0"); }
        }

        public float Aspybatt
        {
            get { return _sbAC.Aspybatt; }
            set
            {
                _sbAC.Aspybatt = value;
                _datiSalvati = false;
            }
        }
        public string strAspybatt
        {
            get { return _sbAC.Aspybatt.ToString("0.0"); }
        }

        //----------------------------------------------------

        public float AspybattAP
        {
            get { return _sbAC.AspybattAP; }
            set
            {
                _sbAC.AspybattAP = value;
                _datiSalvati = false;
            }
        }
        public string strAspybattAP
        {
            get { return _sbAC.AspybattAP.ToString("0.0"); }
        }

        public float AspybattDP
        {
            get { return _sbAC.AspybattDP; }
            set
            {
                _sbAC.AspybattDP = value;
                _datiSalvati = false;
            }
        }
        public string strAspybattDP
        {
            get { return _sbAC.AspybattDP.ToString("0.0"); }
        }

        public float AspybattAN
        {
            get { return _sbAC.AspybattAN; }
            set
            {
                _sbAC.AspybattAN = value;
                _datiSalvati = false;
            }
        }
        public string strAspybattAN
        {
            get { return _sbAC.AspybattAN.ToString("0.0"); }
        }

        public float AspybattDN
        {
            get { return _sbAC.AspybattDN; }
            set
            {
                _sbAC.AspybattDN = value;
                _datiSalvati = false;
            }
        }
        public string strAspybattDN
        {
            get { return _sbAC.AspybattDN.ToString("0.0"); }
        }




        #endregion Class Parameter


    }

}
