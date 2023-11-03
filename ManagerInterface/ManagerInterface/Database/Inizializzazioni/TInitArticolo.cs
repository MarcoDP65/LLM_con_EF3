using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoriData
{
    public partial class TInitArticolo
    {
        public int IdInizializzazione { get; set; }
        public string CodArticolo { get; set; }
        public int? ProgrArticolo { get; set; }
        public int? Predefinito { get; set; }
        public int? Attivo { get; set; }
        public string Descrizione { get; set; }
        public DateTime? DataCreazione { get; set; }
    }
}
