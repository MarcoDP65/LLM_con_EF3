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
    public class _lingue
    {
        [PrimaryKey]
        public int Id { get; set; }
        [MaxLength(30)]
        public string Lingua { get; set; }
        [MaxLength(30)]
        public string InLingua { get; set; }
        public int Ordine { get; set; }
        public int Attivo { get; set; }

        public override string ToString()
        {
            return Lingua;
        }

    }


    public class Lingue
    {
        public _lingue _lng;
        public string username;
        public int livello;
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public Lingue()
        {
            valido = false;
        }

        public Lingue(_db connessione)
        {
            valido = false;
            _database = connessione;
        }


    }//Fine Lingue

}

