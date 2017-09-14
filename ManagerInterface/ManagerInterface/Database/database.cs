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


    public class _db : SQLiteConnection
    {



        
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public _db(string _dbName) : base(new SQLitePlatformWin32(), _dbName)
        {
            try
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
                CreateTable<_sbTestataCalibrazione>();
                CreateTable<_sbAnalisiCorrente>();
                CreateTable<_sbParametriGenerali>();
                CreateTable<_NodoStruttura>();
                CreateTable<_lmEventiDb>();

                inizializzaUtente();
                inizializzaDefSoglie();
                inizializzaSoglie();


                inizializzaAlberoNavigazione();


                bool dbOk = checkInit("_utente");
            }

            catch (Exception ex)
            {
                Log.Error("Database non disponibile");
                Log.Error(ex.ToString());
            }
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

                //string _query = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = ";

                return true;

            }
            catch
            {
                return false;
            }
            
        }



        public  bool CheckDbUpdate()
        {
            int _numRecord;
            try
            {
                // Step 1: allineamento ID Base
                var command = this.CreateCommand("select count(*) from _lmEventiDb where IdAttivita = 1 ");
                var result = command.ExecuteScalar<int>();
                if (result < 1)
                {
                    // 1.1 - Verifico quanti sono
                    command = this.CreateCommand("select count(*) from _spybatt where  IdBase isNull");
                    result = command.ExecuteScalar<int>();

                    Log.Debug("Step 1: allineamento ID Base - " + result.ToString() + " da aggiornare");
                    if (result > 0)
                    {

                        command = this.CreateCommand("update _spybatt set IdBase = Id, NumeroClone = 0 where  IdBase isNull ");
                        _numRecord = command.ExecuteNonQuery();

                    }


                }
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
 
            Utente usrBase = new Utente(this);
            usrBase.cancellaDati();

            bool TabellaCompilata = checkInit("_utente");

            if (!TabellaCompilata)
            {
                _utente[] usr = {
                                  new _utente { Id = 1, Username = "FACTORY", Password = Utility.StringCipher.PasswordEncrypt("factory"), NomeUtente = "Livello Factory", Livello = 0, Lingua = 0, Attivo = 1 },
                                  new _utente { Id = 2, Username = "SERVICE", Password = Utility.StringCipher.PasswordEncrypt("MS13ZXY6645AHKY"), NomeUtente = "Livello Service", Livello = 1, Lingua = 0, Attivo = 1 },
                                  new _utente { Id = 3, Username = "POWERUSER", Password = Utility.StringCipher.PasswordEncrypt("PU51KSX33R"), NomeUtente = "Livello Power User", Livello = 2, Lingua = 0, Attivo = 1 },
                                  new _utente { Id = 4, Username = "USER", Password = Utility.StringCipher.PasswordEncrypt("UT16MR28"), NomeUtente = "Livello User", Livello = 3, Lingua = 0, Attivo = 1 }
                                };
 
                InsertAll(usr);
            }
        }

        public void inizializzaParametri()
        {
            bool TabellaCompilata = checkInit("_parametri");
            if (!TabellaCompilata)
            {
                _parametri[] usr = { new _parametri { Id = 10, Parametro = "Autologin", Tipo = 1, idMessaggio= 10, valInt = 1},
                                     new _parametri { Id = 11, Parametro = "Username", Tipo = 2,idMessaggio = 11, valTesto = "FACTORY"},
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
                    new _sbSoglie { IdLocale =  1, Username = "#default#" ,IdApparato = "#base#",IdMisura =  1,TipoMisura = 0 , ValoreInt = 80, ValoreNum = 0 },
                    new _sbSoglie { IdLocale =  2, Username = "#default#" ,IdApparato = "#base#",IdMisura =  2,TipoMisura = 0 , ValoreInt = 54, ValoreNum = 0 },
                    new _sbSoglie { IdLocale =  3, Username = "#default#" ,IdApparato = "#base#",IdMisura =  3,TipoMisura = 0 , ValoreInt = 0, ValoreNum = 0 },
                    new _sbSoglie { IdLocale =  4, Username = "#default#" ,IdApparato = "#base#",IdMisura =  4,TipoMisura = 0 , ValoreInt = 10, ValoreNum = 0 },
                    new _sbSoglie { IdLocale =  5, Username = "#default#" ,IdApparato = "#base#",IdMisura =  5,TipoMisura = 0 , ValoreInt = 55, ValoreNum = 0 },
                    new _sbSoglie { IdLocale =  6, Username = "#default#" ,IdApparato = "#base#",IdMisura =  6,TipoMisura = 0 , ValoreInt = 55, ValoreNum = 0 },
                    new _sbSoglie { IdLocale =  7, Username = "#default#" ,IdApparato = "#base#",IdMisura =  7,TipoMisura = 0 , ValoreInt = 15, ValoreNum = 0 },
                    new _sbSoglie { IdLocale =  8, Username = "#default#" ,IdApparato = "#base#",IdMisura =  8,TipoMisura = 0 , ValoreInt = 15, ValoreNum = 0 },
                    new _sbSoglie { IdLocale =  9, Username = "#default#" ,IdApparato = "#base#",IdMisura =  9,TipoMisura = 0 , ValoreInt =  0, ValoreNum = 0.05 },
                    new _sbSoglie { IdLocale = 10, Username = "#default#" ,IdApparato = "#base#",IdMisura = 10,TipoMisura = 0 , ValoreInt = 1, ValoreNum = 0 },
                    new _sbSoglie { IdLocale = 11, Username = "#default#" ,IdApparato = "#base#",IdMisura = 11,TipoMisura = 0 , ValoreInt = 24, ValoreNum = 0 },
                    new _sbSoglie { IdLocale = 12, Username = "#default#" ,IdApparato = "#base#",IdMisura = 12,TipoMisura = 0 , ValoreInt = 72, ValoreNum = 0 },
                    new _sbSoglie { IdLocale = 13, Username = "#default#" ,IdApparato = "#base#",IdMisura = 13,TipoMisura = 0 , ValoreInt = 168, ValoreNum = 0 },
                    new _sbSoglie { IdLocale = 14, Username = "#default#" ,IdApparato = "#base#",IdMisura = 14,TipoMisura = 0 , ValoreInt = 360, ValoreNum = 0 },
                    new _sbSoglie { IdLocale = 15, Username = "#default#" ,IdApparato = "#base#",IdMisura = 15,TipoMisura = 0 , ValoreInt = 960, ValoreNum = 0 },
                };
                InsertAll(_sgl);
            }
        }

        private _NodoStruttura NodoRoot()
        {
            try
            {
                _NodoStruttura _tempnodo = new _NodoStruttura()
                {
                    IdLocale = 1,
                    Guid = NodoStruttura.GuidROOT,
                    Tipo = (byte)NodoStruttura.TipoNodo.Radice,
                    Level = 0,
                    ParentGuid = NodoStruttura.GuidBASE,

                    Nome = "Questo PC",
                    Descrizione = "Radice struttura archivio",
                    Icona = "root",
                    IdApparato = null
                };
                _tempnodo.ParentIdLocale = _tempnodo.IdLocale;
                return _tempnodo;

            }
 
            catch (Exception ex)
            {
                Log.Error("NodoRoot: " + ex.ToString());
                return null;
            }
        }

        private _NodoStruttura NodoUndef()
        {
            try
            {
                _NodoStruttura _tempnodo = new _NodoStruttura()
                {
                    IdLocale = 2,
                    Guid = NodoStruttura.GuidUNDEF,

                    ParentGuid = NodoStruttura.GuidROOT,
                    Tipo = (byte)NodoStruttura.TipoNodo.Ramo,
                    Level = 1,
                    ParentIdLocale = 1,

                    Nome = "Non Assegnati",
                    Descrizione = "Apparati non Assegnati a livelli gerarchici",
                    Icona = "folder",
                    IdApparato = null
                };

                return _tempnodo;

            }

            catch (Exception ex)
            {
                Log.Error("NodoUndef: " + ex.ToString());
                return null;
            }
        }

        /*
        public bool TableExists(String tableName, SQLiteConnection connection)
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM sqlite_master WHERE type = 'table' AND name = @name";
            cmd.Parameters.Add("@name", DbType.String).Value = tableName;
            return (cmd.ExecuteScalar() != null);
        }
        */
        public void inizializzaAlberoNavigazione()
        {
            bool TabellaCompilata = checkInit("_NodoStruttura");


            if (!TabellaCompilata)
            {

                _NodoStruttura[] _sgl = new _NodoStruttura[2];
                _sgl[0] = NodoRoot();
                _sgl[1] = NodoUndef();


                InsertAll(_sgl);
            }
        }


        public bool VerificaAggiornamentoDB()
        {
            try
            {
                // Step 1: verifico se è presente e attivo il log aggiornamenti




                return true;
            }

            catch (Exception ex)
            {
                Log.Error("VerificaAggiornamentoDB: " + ex.ToString());
                return false;
            }
        }

        public bool VerificaAttività(int IdAttivita)
        {
            try
            {
  


                return true;
            }

            catch (Exception ex)
            {
                Log.Error("VerificaAggiornamentoDB: " + ex.ToString());
                return false;
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

        }

    }

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
