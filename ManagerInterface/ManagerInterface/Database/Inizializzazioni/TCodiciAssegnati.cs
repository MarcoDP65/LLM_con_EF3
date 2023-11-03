using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoriData
{
    public partial class TCodiciAssegnati
    {
        public int SerialId { get; set; }
        public int? Progressivo { get; set; }
        public string LineaProdotto { get; set; }
        public int? Anno { get; set; }
        public string Articolo { get; set; }
        public string Cliente { get; set; }
        public DateTime? DataAssegnazione { get; set; }
        public int? NumModuli { get; set; }
        public string TipoModuli { get; set; }
        public string Vmin { get; set; }
        public string Vmax { get; set; }
        public string Amax { get; set; }
    }
}
