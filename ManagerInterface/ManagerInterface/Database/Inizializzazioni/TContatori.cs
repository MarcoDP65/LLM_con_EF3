using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoriData
{
    public partial class TContatori
    {
        public string LineaProdotto { get; set; }
        public int Anno { get; set; }
        public int UltimoValore { get; set; }
        public DateTime? DataAssegnazione { get; set; }
    }
}
