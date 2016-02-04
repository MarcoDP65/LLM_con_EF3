using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using  MoriData;

namespace PannelloCharger
{
    public class StatBloccoSoglie
    {
        public sbDefSoglia DatiSoglia;
        public sbSoglia ValGlobale;
        public sbSoglia ValUtente;
        public sbSoglia ValScheda;

        public int idSoglia { get; set; }

        public StatBloccoSoglie()
        {
            DatiSoglia = new sbDefSoglia();
            ValGlobale = new sbSoglia();
            ValUtente = null;
            ValScheda = null;

        }

        public bool CaricaSoglia ( int IdSoglia )
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

    }
}
