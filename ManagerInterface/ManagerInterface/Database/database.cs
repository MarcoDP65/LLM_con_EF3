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

namespace MoriData
{


    public class _parametri
    {
        [PrimaryKey]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Parametro { get; set; }
        [MaxLength(255)]
        public int Tipo { get; set; }
        public int idMessaggio { get; set; }
        public string valTesto { get; set; }
        public int valInt { get; set; }
        public DateTime valTime { get; set; }
        public decimal valDec { get; set; }
        public override string ToString()
        {
            string _testo;
            _testo = Parametro;
            switch (Tipo)
            {
                case 1: // Stringa
                    _testo += valTesto;
                    break;
                case 2: // Intero
                    _testo += valInt.ToString();
                    break;
                case 3: // Data/ora
                    _testo += valTime.ToString();
                    break;
                case 4: //decimale
                    _testo += valDec.ToString();
                    break;
            }

            return _testo;
        }
    }
  
    public class _eventoBreve
    {
        [PrimaryKey]
        public int Id { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime DataModifica { get; set; }
        public string UtenteModifica { get; set; }
        [MaxLength(255)]
        public string Parametro { get; set; }
        [MaxLength(255)]
        public int Tipo { get; set; }
        public int idMessaggio { get; set; }
        public string valTesto { get; set; }
        public int valInt { get; set; }
        public DateTime valTime { get; set; }
        public decimal valDec { get; set; }
        public override string ToString()
        {
            string _testo;
            _testo = Parametro;
            switch (Tipo)
            {
                case 1: // Stringa
                    _testo += valTesto;
                    break;
                case 2: // Intero
                    _testo += valInt.ToString();
                    break;
                case 3: // Data/ora
                    _testo += valTime.ToString();
                    break;
                case 4: //decimale
                    _testo += valDec.ToString();
                    break;
            }

            return _testo;
        }
   }



    public class InizializzaDb
    {
     }


    public class _db : SQLiteConnection
    {

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public _db(string _dbName) : base(new SQLitePlatformWin32(), _dbName)
        {
            int _esito;
            Log.Error("Database collegato: " + _dbName);
            //Create Tables
            _esito = CreateTable<_lingue>();
            _esito = CreateTable<_utente>();
            CreateTable<_sbDefSoglia>();
            CreateTable<_sbSoglie>();
            CreateTable<_parametri>();
            CreateTable<_spybatt>();
            CreateTable<_sbMemLunga>();
            CreateTable<_sbMemBreve>();
            CreateTable<_sbProgrammaRicarica>();
            CreateTable<_sbDatiCliente>();

            inizializzaUtente();
            inizializzaDefSoglie(); 
            inizializzaSoglie();

            bool dbOk = checkInit("_utente");
        }

        public  _utente CaricaUtente (string Username)
        {
            return	(from s in Table<_utente> ()
                    where s.Username == Username
                    select s).FirstOrDefault ();
        }

        public _parametri CaricaParametro(int Id)
        {
            return (from s in Table<_parametri>()
                    where s.Id == Id
                    select s).FirstOrDefault();
        }


        /// <summary>
        /// Verifico se sono presenti record nella tabella passata come parametro
        /// usata per verificare se le tabelle di servzio sono inizializzate
        /// </summary>
        /// <param name="tableName">Name of the table to be verified.</param>
        /// <returns></returns>
        public bool checkInit(string tableName)
        {
            //controllo se i record base sono presenti
            try
            {
                //var command = this.CreateCommand("select count(*) from ? ", tableName);
                var command = this.CreateCommand("select count(*) from " + tableName);
                var result = command.ExecuteScalar<int>();
                if (result > 0)
                {
                    Log.Debug("Contiene righe");
                    return true;
                }
                else
                {
                    Log.Debug("Vuota");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Errore cotrollo: " + ex.ToString());

                return false;
            }

        }

        public static bool tableExists(String tableName)
        {
            try
            {
                return true;

            }
            catch
            {
                return false;
            }
            
        }


        #region Inizializza Tabelle
        // valorizza la prima istanza delle tabelle



        public void inizializzaUtente()
        {
            bool TabellaCompilata = checkInit("_utente");
            if (!TabellaCompilata)
            {
                _utente[] usr = { new _utente { Id = 1, Username = "Factory", Password = "factory", NomeUtente = "Livello Factory", Livello = 0, Lingua = 0, Attivo = 1 },
                                  new _utente { Id = 2, Username = "Service", Password = "service", NomeUtente = "Livello Service", Livello = 1, Lingua = 0, Attivo = 1 },
                                  new _utente { Id = 3, Username = "PowerUser", Password = "poweruser", NomeUtente = "Livello Power User", Livello = 2, Lingua = 0, Attivo = 1 },
                                  new _utente { Id = 4, Username = "User", Password = "user", NomeUtente = "Livello User", Livello = 3, Lingua = 0, Attivo = 1 },
                                  new _utente { Id = 5, Username = "FactoryEn", Password = "factory", NomeUtente = "Livello Factory", Livello = 0, Lingua = 1, Attivo = 1 },
                                  new _utente { Id = 6, Username = "ServiceEn", Password = "service", NomeUtente = "Livello Service", Livello = 1, Lingua = 1, Attivo = 1 },
                                  new _utente { Id = 7, Username = "PowerUserEn", Password = "poweruser", NomeUtente = "Livello Power User", Livello = 2, Lingua = 1, Attivo = 1 },
                                  new _utente { Id = 8, Username = "UserEn", Password = "user", NomeUtente = "Livello User", Livello = 3, Lingua = 1, Attivo = 1 }};
 
                InsertAll(usr);
            }
        }

        public void inizializzaParametri()
        {
            bool TabellaCompilata = checkInit("_parametri");
            if (!TabellaCompilata)
            {
                _parametri[] usr = { new _parametri { Id = 10, Parametro = "Autologin", Tipo = 1, idMessaggio= 10, valInt = 1},
                                     new _parametri { Id = 11, Parametro = "Username", Tipo = 2,idMessaggio = 11, valTesto = "Factory"},
                                     new _parametri { Id = 12, Parametro = "Password", Tipo = 2,idMessaggio = 12, valTesto = "factory"}};

                InsertAll(usr);
            }
        }


        public void inizializzaDefSoglie()
        {
            bool TabellaCompilata = checkInit("_sbDefSoglia");
            if (!TabellaCompilata)
            {
                _sbDefSoglia[] _sgl = 
                { 
                    new _sbDefSoglia { IdLocale = 1, IdSoglia = 1, DescSoglia = "Profondità D.O.D.",TipoMisura = 0 , IdLingua = 1 },
                    new _sbDefSoglia { IdLocale = 2, IdSoglia = 2, DescSoglia = "Tmax Scarica",  TipoMisura = 0 , IdLingua = 1 },
                    new _sbDefSoglia { IdLocale = 3, IdSoglia = 3, DescSoglia = "Tmin Scarica",  TipoMisura = 0 , IdLingua = 1 },
                    new _sbDefSoglia { IdLocale = 4, IdSoglia = 4, DescSoglia = "Diff T Scarica",  TipoMisura = 0 , IdLingua = 1  },
                    new _sbDefSoglia { IdLocale = 5, IdSoglia = 5, DescSoglia = "Tmax Carica Completa",  TipoMisura = 0 , IdLingua = 1 },
                    new _sbDefSoglia { IdLocale = 6, IdSoglia = 6, DescSoglia = "Tmax Carica Parziale",  TipoMisura = 0 , IdLingua = 1  },
                    new _sbDefSoglia { IdLocale = 7, IdSoglia = 7, DescSoglia = "Diff T Carica Completa",  TipoMisura = 0 , IdLingua = 1  },
                    new _sbDefSoglia { IdLocale = 8, IdSoglia = 8, DescSoglia = "Diff T Carica Parziale",  TipoMisura = 0 , IdLingua = 1  },
                    new _sbDefSoglia { IdLocale = 9,  IdSoglia= 9, DescSoglia = "DeltaV Sblianciamento Celle",  TipoMisura = 1 , IdLingua = 1 },
                    new _sbDefSoglia { IdLocale = 10, IdSoglia = 10, DescSoglia = "Charge Factor min",  TipoMisura = 0 , IdLingua = 1  },
                    new _sbDefSoglia { IdLocale = 11, IdSoglia = 11, DescSoglia = "Ore Pausa per Fascia D.O.D. 81/100",  TipoMisura = 0 , IdLingua = 1  },
                    new _sbDefSoglia { IdLocale = 12, IdSoglia = 12, DescSoglia = "Ore Pausa per Fascia D.O.D. 61/80", TipoMisura = 0 , IdLingua = 1  },
                    new _sbDefSoglia { IdLocale = 13, IdSoglia = 13, DescSoglia = "Ore Pausa per Fascia D.O.D. 41/60", TipoMisura = 0 , IdLingua = 1  },
                    new _sbDefSoglia { IdLocale = 14, IdSoglia = 14, DescSoglia = "Ore Pausa per Fascia D.O.D. 21/40",  TipoMisura = 0 , IdLingua = 1  },
                    new _sbDefSoglia { IdLocale = 15, IdSoglia = 15, DescSoglia = "Ore Pausa per Fascia D.O.D. 0/20",  TipoMisura = 0 , IdLingua = 1  },
                };


                InsertAll(_sgl);
            }
        }






        public void inizializzaSoglie()
        {
            bool TabellaCompilata = checkInit("_sbSoglie");
            if (!TabellaCompilata)
            {
                _sbSoglie[] _sgl = 
                { 
                    new _sbSoglie { IdLocale = 1, Username = "#default#", IdApparato = "#base#", IdMisura= 1, TipoMisura = 0 ,ValoreInt = 80},
                    new _sbSoglie { IdLocale = 2, Username = "#default#", IdApparato = "#base#", IdMisura= 2, TipoMisura = 0 ,ValoreInt = 55},
                    new _sbSoglie { IdLocale = 3, Username = "#default#", IdApparato = "#base#", IdMisura= 3, TipoMisura = 0 ,ValoreInt = 0},
                    new _sbSoglie { IdLocale = 4, Username = "#default#", IdApparato = "#base#", IdMisura= 4, TipoMisura = 0 ,ValoreInt = 12},
                    new _sbSoglie { IdLocale = 5, Username = "#default#", IdApparato = "#base#", IdMisura= 5, TipoMisura = 0 ,ValoreInt = 55},
                    new _sbSoglie { IdLocale = 6, Username = "#default#", IdApparato = "#base#", IdMisura= 6, TipoMisura = 0 ,ValoreInt = 55},
                    new _sbSoglie { IdLocale = 7, Username = "#default#", IdApparato = "#base#", IdMisura= 7, TipoMisura = 0 ,ValoreInt = 12},
                    new _sbSoglie { IdLocale = 8, Username = "#default#", IdApparato = "#base#", IdMisura= 8, TipoMisura = 0 ,ValoreInt = 12},
                    new _sbSoglie { IdLocale = 9, Username = "#default#", IdApparato = "#base#", IdMisura= 9, TipoMisura = 1 ,ValoreNum = 0.05},
                    new _sbSoglie { IdLocale = 10, Username = "#default#", IdApparato = "#base#", IdMisura= 10, TipoMisura = 0 ,ValoreInt = 1},
                    new _sbSoglie { IdLocale = 11, Username = "#default#", IdApparato = "#base#", IdMisura= 11, TipoMisura = 0 ,ValoreInt = 5},
                    new _sbSoglie { IdLocale = 12, Username = "#default#", IdApparato = "#base#", IdMisura= 12, TipoMisura = 0 ,ValoreInt = 20},
                    new _sbSoglie { IdLocale = 13, Username = "#default#", IdApparato = "#base#", IdMisura= 13, TipoMisura = 0 ,ValoreInt = 72},
                    new _sbSoglie { IdLocale = 14, Username = "#default#", IdApparato = "#base#", IdMisura= 14, TipoMisura = 0 ,ValoreInt = 480},
                    new _sbSoglie { IdLocale = 15, Username = "#default#", IdApparato = "#base#", IdMisura= 15, TipoMisura = 0 ,ValoreInt = 480},
                };


                InsertAll(_sgl);
            }
        }

        #endregion


    }





    public class Archivio
    {
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool isDatabaseExisting = false;
        public bool isDatabaseConnected = false;
        public string dbPath;

        public _db connessione ;


        public Archivio()
        {
            XmlConfigurator.Configure();
            Log.Debug("Inizializza Database");
            var dbPathLoc = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoriCharger.db");
            dbPath = dbPathLoc;
            Log.Debug(dbPathLoc);
            isDatabaseExisting = archivioPresente();

            connessione = new _db(dbPathLoc);
            Log.Debug("Connessione Database");


        }

        public void Close()
        {
            connessione.Close();

        }


        public bool archivioPresente()
        {
            try
            {
                if (File.Exists(dbPath))
                {
                    isDatabaseExisting = true;
                    return true;
                }
                else
                {
                    isDatabaseExisting = false;
                    return false;
                }

            }
            catch (Exception ex)
            {
                Log.Error("Database non verificabile");
                Log.Error(ex.ToString());
                isDatabaseExisting = false;
                return false;
            }
        }

        public bool nuovoArchivio()
        {
            try
            {
                if (File.Exists(dbPath))
                    return true;
                else
                    return false;

            }
            catch
            {
                return false;
            }
        }



        public class Parametro
        {
            public _parametri _par; // = new _utente();
            public MoriData._db _database;

            public bool valido;

//            public Parametro()
//            {
//                valido = false;
//            }

            public Parametro(_db connessione)
            {
                valido = false;
                _database = connessione;

            }

            public _parametri caricaValore(int _id)
            {
                try
                {

                    return _database.CaricaParametro(_id);

                }

                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    return null;
                }
            }

        }//Fine Parametro

    }//Fine Archivio


    public class Db : SQLiteConnection
        {
            private bool isDatabaseExisting = false;
            private bool isDatabaseConnected = false;
            //private SQLiteConnection _db ;

            public Db(string path)
                : base(new SQLitePlatformWin32(), path)
            {
                isDatabaseConnected = true;
            }

        }

    
}
