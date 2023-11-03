using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;


namespace ChargerLogic
{
    public class LogicheBase
    {
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public MoriData.Archivio dbDati;

        public MoriData.Utente currentUser;
        public MoriData.LadeLightDataContext LLContextDb;


        public  LogicheBase()
        {
            XmlConfigurator.Configure();
            Log.Debug("Inizializza Logiche");
            dbDati = new MoriData.Archivio();
            if (dbDati.archivioPresente())
            {
                Log.Debug("Archivio Presente");
            }
            else
            {
                Log.Debug("Archivio da creare");
            }

            LLContextDb = new MoriData.LadeLightDataContext();
            
            currentUser = new MoriData.Utente(dbDati.connessione);
        }



        public void chiudiConn()
        {
          //  dbDati.Close();
        }

    
    }
}
