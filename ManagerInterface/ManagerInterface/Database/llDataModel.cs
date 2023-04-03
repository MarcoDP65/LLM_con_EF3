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
    public class llDataModel
    {
        public string ID { get; set; }

        public _ladelight Testata;
        public _llParametriApparato Parametri;
        public _llDatiCliente Cliente;
        public _llContatoriApparato Contatori;
        public List<_llMemoriaCicli> CicliCarica = new List<_llMemoriaCicli>();
        public List<_llProgrammaCarica> Programmazioni = new List<_llProgrammaCarica>();
        public ushort CRC { get; set; }
    }



    public class llDataCicloLungo
    {
        public _llMemoriaCicli TestataCiclo;
        public List<_llMemBreve> CicliBrevi = new List<_llMemBreve>();
    }
}
