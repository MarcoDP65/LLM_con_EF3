using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoriData
{
    public partial class TArticoli
    {
        public string CodArticolo { get; set; }
        public string Descrizione { get; set; }
        public string Linea { get; set; }
        public int? Attivo { get; set; }
        public DateTime? DataInizio { get; set; }
        public DateTime? DataFine { get; set; }
        public int? Vmin { get; set; }
        public int? Vmax { get; set; }
        public int? Amax { get; set; }
        public int? NumeroModuli { get; set; }
        public string TipoModulo { get; set; }
    }
}
