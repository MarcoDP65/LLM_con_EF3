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
    public class _llParametriApparato
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

        public string ProduttoreApparato { get; set; }  // 18 byte, fisso MORI RADDRIZZATORI 
        public string NomeApparato { get; set; }        // 10 byte, fisso LADE LIGHT 

        public uint SerialeApparato { get; set; }
        public byte AnnoCodice { get; set; }
        public uint ProgressivoCodice { get; set; }
        public byte TipoApparato { get; set; }
        public uint DataSetupApparato { get; set; }


        public byte[] SerialeZVT { get; set; }
        public string HardwareZVT { get; set; }
        public byte[] SerialePFC { get; set; }
        public string HardwarePFC { get; set; }
        public string SoftwarePFC { get; set; }
        public byte[] SerialeDISP { get; set; }
        public string HardwareDisp { get; set; }
        public string SoftwareDISP { get; set; }
        public uint MaxRecordBrevi { get; set; }
        public ushort MaxRecordCarica { get; set; }
        public uint SizeExternMemory { get; set; }
        public byte MaxProgrammazioni { get; set; }
        public byte ModelloMemoria { get; set; }
        public ushort CrcPacchetto { get; set; }

        public byte Stato { get; set; }

        public DateTime IstanteLettura { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IstanteLettura.ToString();
        }
    }

    public class llParametriApparato
    {
        public string nullID { get { return "0000000000000000"; } }
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
                //_llParApp = _caricaDati(idLocale);
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

        #endregion


    }

}





