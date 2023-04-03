using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using log4net;
using log4net.Config;
using MoriData;
using Utility;

namespace ChargerLogic
{
    /// <summary>
    /// Struttura complessa serializzabile per l'esportazione dati 
    /// </summary>
    public class sbDataModel
    {
        public string ID { get; set; }

        public bool testata{ get; set; }
        public bool cliente { get; set; }
        public bool cicliLunghi { get; set; }
        public bool cicliBrevi { get; set; }
        public bool programmazioni { get; set; }

        public _spybatt Testata;
        public _sbDatiCliente Cliente;
        public List<sbDataCicloLungo> CicliLunghi = new List<sbDataCicloLungo>();
        public List<_sbProgrammaRicarica> Programmazioni = new List<_sbProgrammaRicarica>();
        public ushort CRC { get; set; }
    }



    public class sbDataCicloLungo
    {
        public _sbMemLunga TestataCiclo;
        public List<_sbMemBreve> CicliBrevi = new List<_sbMemBreve>(); 
    }
}
