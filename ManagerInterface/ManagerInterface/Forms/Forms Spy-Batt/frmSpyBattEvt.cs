using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using MoriData;
using ChargerLogic;

namespace PannelloCharger
{
    public class DatiCambiatiEventArgs : EventArgs
    {
        public bool DaSalvare { get; private set; }

        public DatiCambiatiEventArgs(bool Stato )
        {
            DaSalvare = Stato;
        }
    }
}
