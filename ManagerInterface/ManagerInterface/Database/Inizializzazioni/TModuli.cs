using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoriData
{
    public partial class TModuli
    {
        public string IdTipoModulo { get; set; }
        public string TipoModulo { get; set; }
        public string Descrizione { get; set; }
        public int? Vnom { get; set; }
        public int? Vmin { get; set; }
        public int? Vmax { get; set; }
        public int? Anom { get; set; }
        public int? Amax { get; set; }
        public int? Wnom { get; set; }
        public int? Fasi { get; set; }
        public int? Attivo { get; set; }
    }
}
