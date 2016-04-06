
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Cipher;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using System.IO;
using log4net;
using log4net.Config;
using SQLite.Net.Cipher.Model;

namespace MoriData
{

    public class _utente
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [MaxLength(30)]
        public string Username { get; set; }
        [MaxLength(30)]
        public string Password { get; set; }
        [MaxLength(255)]
        public string NomeUtente { get; set; }
        [Secure]
        public int Livello { get; set; }
        public int Lingua { get; set; }
        public int Attivo { get; set; }

        public override string ToString()
        {
            return NomeUtente;
        }

    }


    public class Utente
    {
        public _utente _usr;
        public string username;
        public string login;
        public string password;
        public int livello;
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public Utente()
        {
            valido = false;
        }

        public Utente( _db connessione )
        {
            valido = false;
            _database = connessione;
        }

        public bool cancellaDati()
        {
            try
            {
                SQLiteCommand CancellaRecord = _database.CreateCommand("delete from _utente ");
                int esito = CancellaRecord.ExecuteNonQuery();
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("cancellaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        public int verificaUtente(string login, string password)
        {
            try
            {
                _usr = _database.CaricaUtente(login);
                if (_usr == null)
                {
                    Log.Debug("Non Trovato");
                    return -1;
                }
                else
                {
                    string _decripted = Utility.StringCipher.PasswordDecrypt(_usr.Password);
                    if (_decripted == password)
                    {
                        Log.Debug(_usr.ToString());
                        username = _usr.NomeUtente;
                        login = _usr.Username;

                        if (_usr.Livello == 2 | _usr.Livello == 3)
                        {
                            livello = _usr.Livello;
                            return _usr.Livello;
                        }
                        else
                        {
                            livello = 3;
                            return 3;
                        }

                    }
                    else
                    {
                        return -2;
                    }
                }

            }

            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return -1;
            }
        }

    }//Fine Utente

}