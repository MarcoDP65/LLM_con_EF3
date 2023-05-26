using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

using ChargerLogic;
using Utility;

namespace MoriData
{
    public class ItemModProfilo
    {


        //public int NumParametriAttivi = 0;
       
        [PrimaryKey]
        public ushort IdProgramma { get; set; }

        //public ProfiloCarica Profilo;
        public ushort Tensione { get; set; }
        public ushort NumeroCelle { get; set; }
        public ushort Capacita { get; set; }
        public ushort TipoBatteria { get; set; }
        //[AllowNull]
        public string NomeProfilo { get; set; }
        //[AllowNull]
        public string NoteProfilo { get; set; }
        public bool DatiSalvati { get; set; }
        public bool RichiestoNuovoId { get; set; }
        public ushort DurataMaxCarica { get; set; }
        public byte IdProfiloCaricaLL { get; set; }   // FK ProfiloCarica
        [Ignore]
        //[AllowNull]
        public ParametriCiclo ValoriCiclo { get; set; }

        [TextBlob("BlobParametri")]
        //[AllowNull]
        public List<ParametroLL> ListaParametri { get; set; }
       // [AllowNull]
        public string BlobParametri { get; set; }
        //[AllowNull]
        public string SerialeDispositivo { get; set; }

        public string strTensione
        { get
            {
                return FunzioniMR.StringaCapacita(Tensione,100,0);
            }         
        }
        public string strCapacita
        {
            get
            {
                return FunzioniMR.StringaCapacita(Capacita ,10,0);
            }
        }
    }

}
