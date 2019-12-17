using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net;
using log4net;
using log4net.Config;
using MoriData;
using Utility;

namespace ChargerLogic
{
    /// <summary>
    /// Struttura complessa serializzabile per l'esportazione dati 
    /// </summary>
    public class llDataModel
    {
        public string ID { get; set; }

        public bool testata { get; set; }
        public bool cliente { get; set; }
        public bool contatori { get; set; }
        public bool cicliLunghi { get; set; }
        public bool cicliBrevi { get; set; }
        public bool programmazioni { get; set; }

        public _ladelight Testata;
        public _llDatiCliente Cliente;
        public _llContatoriApparato Contatori;
        public List<llDataCicloLungo> CicliLunghi = new List<llDataCicloLungo>();
        public List<_sbProgrammaRicarica> Programmazioni = new List<_sbProgrammaRicarica>();
        public ushort CRC { get; set; }
    }



    public class llDataCicloLungo
    {
        public _llMemoriaCicli TestataCiclo;
        public List<_llMemBreve> CicliBrevi = new List<_llMemBreve>();
    }
}
