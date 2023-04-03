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
    public class _llParametriApparato
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        [Indexed(Name = "IdxLLParApparato", Order = 1, Unique = true)]
        public string IdApparato { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }

        public string ProduttoreApparato { get; set; }  // 18 byte, fisso MORI RADDRIZZATORI 
        public string NomeApparato { get; set; }        // 10 byte, fisso LADE LIGHT 

        public uint SerialeApparato { get; set; }
        public byte AnnoCodice { get; set; }
        public uint ProgressivoCodice { get; set; }
        public byte TipoApparato { get; set; }
        public byte FamigliaApparato { get; set; }
        public uint DataSetupApparato { get; set; }

        public String IdLottoZVT { get; set; }       
        public byte[] SerialeZVT { get; set; }
        public string HardwareZVT { get; set; }
        public String IdLottoPFC { get; set; }
        public byte[] SerialePFC { get; set; }
        public string HardwarePFC { get; set; }
        public string SoftwarePFC { get; set; }
        public byte[] SerialeDISP { get; set; }
        public string HardwareDisp { get; set; }
        public string SoftwareDISP { get; set; }
        public uint MaxRecordBrevi { get; set; }      // Da non modificare 
        public ushort MaxRecordCarica { get; set; }   // Da non modificare
        public uint SizeExternMemory { get; set; }    // Da non modificare
        public byte MaxProgrammazioni { get; set; }   // Da non modificare
        public byte ModelloMemoria { get; set; }      // Da non modificare
        public ushort CrcPacchetto { get; set; }

        public byte Stato { get; set; }

        public DateTime IstanteLettura { get; set; }

        public ushort VMin { get; set; }
        public ushort VMax { get; set; }
        public ushort Amax { get; set; }

        public byte PresenzaRabboccatore { get; set; }

        // Gestione Moduli SCHG
        public byte NumeroModuli { get; set; }
        public ushort ModVNom { get; set; }
        public ushort ModANom { get; set; }
        public ushort ModOpzioni { get; set; }
        public ushort ModVMin { get; set; }
        public ushort ModVMax { get; set; }


        public DateTime UltimaLetturaDati { get; set; }
        public String DtUltimaLetturaDati { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IstanteLettura.ToString();
        }
    }

    public class llParametriApparato
    {
        public string nullID { get { return "000000"; } }
        public _llParametriApparato llParApp = new _llParametriApparato();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("llParametriApparato");
        public bool _datiSalvati;
        public bool _recordPresente;
        //private string _tempId;



        public llParametriApparato()
        {
            llParApp = new _llParametriApparato();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }


        public llParametriApparato(_db connessione)
        {
            valido = false;
            llParApp = new _llParametriApparato();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }



        private _llParametriApparato _caricaDati(int _id)
        {
            return (from s in _database.Table<_llParametriApparato>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        private _llParametriApparato _caricaDati(string _idApp)
        {
            return (from s in _database.Table<_llParametriApparato>()
                    where s.IdApparato == _idApp
                    select s).FirstOrDefault();
        }


        public bool caricaDati(int idLocale)
        {
            try
            {
                llParApp = _caricaDati(idLocale);
                if (llParApp == null)
                {
                    llParApp = new _llParametriApparato();
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool caricaDati(string IdApparato)
        {
            try
            {
                llParApp = _caricaDati(IdApparato);
                if (llParApp == null)
                {
                    llParApp = new _llParametriApparato();
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
                
                if (llParApp.IdApparato != nullID & llParApp.IdApparato != null)
                {

                    _llParametriApparato _TestDati = _caricaDati(llParApp.IdApparato);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        llParApp.CreationDate = DateTime.Now;
                        llParApp.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(llParApp);
                        _datiSalvati = true;
                    }
                    else
                    {
                        llParApp.IdLocale = _TestDati.IdLocale;
                        llParApp.RevisionDate = DateTime.Now;
                        int _result = _database.Update(llParApp);
                        _datiSalvati = true;
                    }

                    //_database.InsertOrReplace(_sb);
                    return true;
                }
                else
                {
                    return false;
                }
                
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
            get { return llParApp.IdLocale; }
            set
            {
                if (value != null)
                {
                    llParApp.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }

        public string IdApparato
        {
            get
            {
                return llParApp.IdApparato;
            }

        }

        public uint SerialeApparato
        {
            get
            {
                return llParApp.SerialeApparato;
            }
            set
            {
                llParApp.SerialeApparato = value;
            }

        }

        public byte FamigliaApparato
        {
            get
            {
                return llParApp.FamigliaApparato;
            }
            set
            {
                llParApp.FamigliaApparato = value;
            }

        }

        public bool SchedaInizializzata
        {
            get
            {
                return false;
            }

        }
        public bool ApparatoDefinito
        {
            get
            {
                return false;
            }

        }








        #endregion


    }

}





