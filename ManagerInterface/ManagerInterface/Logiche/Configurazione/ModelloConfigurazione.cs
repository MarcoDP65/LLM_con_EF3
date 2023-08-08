using ChargerLogic;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PannelloCharger
{
    public class ModelloConfigurazione
    {
        
        [PrimaryKey]
        public int IdConfigurazione { get; set; }  
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public bool Attivo { get; set; }
        public bool Esito { get; set; }
        public int Avanzamento { get; set; }
        public string CodiceProdotto { get; set; }
        public string CodiceCliente { get; set; }
        public string CodiceConfigurazione { get; set; }


        public int Ordine { get; set; }
        public List<StepConfigurazione> ListaAzioni { get; set; }
    }
}
