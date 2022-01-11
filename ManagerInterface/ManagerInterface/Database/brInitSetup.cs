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
using ChargerLogic;
using Utility;

namespace MoriData
{
    public class _brInitSetup
    {
        [PrimaryKey]
        public Int32 IdSetup { get; set; }
        //-------------------------------------------------------------
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        public string LastUser { get; set; }
        //-------------------------------------------------------------
        public bool FWArea1Enabled { get; set; }
        public String FWArea1Path { get; set; }
        public String FWArea1Filename { get; set; }
        public bool FWArea2Enabled { get; set; }
        public String FWArea2Path { get; set; }
        public String FWArea2Filename { get; set; }
        public bool ProcSeqEnabled { get; set; }
        public String ProcSeqPath { get; set; }
        public String ProcSeqFilename { get; set; }
        public bool LngEnabled { get; set; }
        public String LngPath { get; set; }
        public String LngFilename { get; set; }
        public bool AreaEnabled { get; set; }
        public int AreaAttiva { get; set; }
        public bool CancellaValori { get; set; }


    }

    public class brInitSetup
    {
        public _brInitSetup Valori;
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("brInitSetup");
        public bool _datiSalvati;
        public bool _recordPresente;

        public brInitSetup()
        {
            Valori = new _brInitSetup();
            AzzeraValori();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public brInitSetup(_db connessione )
        {
            valido = false;
            Valori = new _brInitSetup();
            AzzeraValori();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
            
        }

        public bool AzzeraValori()
        {
            Valori.IdSetup = 1;
            Valori.CreationDate = DateTime.Now;
            Valori.RevisionDate = DateTime.Now;
            Valori.LastUser = "";
            Valori.FWArea1Enabled = false;
            Valori.FWArea1Path = "";
            Valori.FWArea1Filename = "";
            Valori.FWArea2Enabled = false;
            Valori.FWArea2Path = "";
            Valori.FWArea2Filename = "";
            Valori.ProcSeqEnabled = false;
            Valori.ProcSeqPath = "";
            Valori.ProcSeqFilename = "";
            Valori.LngEnabled = false;
            Valori.LngPath = "";
            Valori.LngFilename = "";
            Valori.AreaEnabled = false;
            Valori.AreaAttiva = 1;
            Valori.CancellaValori = false;

            return true;
        }

        private _brInitSetup _caricaDati(int _SetupID)
        {
            return (from s in _database.Table<_brInitSetup>()
                    where s.IdSetup == _SetupID
                    select s).FirstOrDefault();
        }

        public bool caricaDati(int SetupID)
        {
            try
            {
                Valori = _caricaDati( SetupID );
                if (Valori == null)
                {
                    Valori = new _brInitSetup();
                    AzzeraValori();
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

                _brInitSetup _TestDati = _caricaDati(Valori.IdSetup);
                if (_TestDati == null)
                {
                    //nuovo record
                    Valori.CreationDate = DateTime.Now;
                    Valori.RevisionDate = DateTime.Now;

                    int _result = _database.Insert(Valori);
                    _datiSalvati = true;
                }
                else
                {

                    Valori.RevisionDate = DateTime.Now;
                    int _result = _database.Update(Valori);
                    _datiSalvati = true;
                }

                //_database.InsertOrReplace(_sb);
                return true;

            }

            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

    }
}
