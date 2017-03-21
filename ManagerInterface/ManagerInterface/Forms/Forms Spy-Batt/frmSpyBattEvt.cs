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
    /// <summary>
    /// Class DatiCambiatiEventArgs.
    /// Gestisce i parametri passati negli eventi Salvato/Da Salvare e Dati Cambiati
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class DatiCambiatiEventArgs : EventArgs
    {
        public bool DaSalvare { get; private set; }

        public DatiCambiatiEventArgs(bool Stato )
        {
            DaSalvare = Stato;
        }
    }
}
