using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChargerLogic
{
    class ClassiSupporto
    {
    }

    public class ParametroLL
    {
        public byte idParametro { get; set; }
        public ushort ValoreParametro{ get; set; }
    }

    public class ElementoMemoria
    {
        public int StartAddress { get; set; }
        public int ElemetSize { get; set; }
        public int NoOfElemets { get; set; }
        public int ExtraMem { get; set; }
        public int EndAddress { get; set; }

    }

    public class MappaMemoria
    {
        public ElementoMemoria Testata { get; set; }
        public ElementoMemoria DatiCliente { get; set; }
        public ElementoMemoria Programmazioni { get; set; }
        public ElementoMemoria MemLunga { get; set; }
        public ElementoMemoria MemBreve { get; set; }
        public int LivelloFw { get; set; }

        private bool _datiValidi;

        public MappaMemoria()
        {
            Testata = new ElementoMemoria();
            DatiCliente = new ElementoMemoria();
            Programmazioni = new ElementoMemoria();
            MemLunga = new ElementoMemoria();
            MemBreve = new ElementoMemoria();
            _datiValidi = false;
            LivelloFw = -1;
        }

        public bool datiValidi
        {
            get { return _datiValidi; }
            set { _datiValidi = value; }
        }
    }

    public class ParametroCalibrazione
    {
        public byte IdParametro { get; set; }
        public string DescrizioneBase { get; set; }
        public byte NumDecimali { get; set; }

        public ParametroCalibrazione()
        {
            IdParametro = 0xFF;
            DescrizioneBase = "";
        }

        public override string ToString()
        {
            return DescrizioneBase;
        }



    }
}
