using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PannelloCharger
{
    class GestioneTempi
    {
    }

    public class SettimanaMR
    {
        public int anno { get; set; }
        public int settimana { get; set; }
        public int Inizio { get; set; }
        public string chiaveSettimana { get; set; }

    }

    public class PeriodoMR
    {
        public int anno { get; set; }
        public int settimana { get; set; }
        public int minutoInizio { get; set; }
        public int giornoInizio { get; set; }
        public int minutoFine { get; set; }
        public int giornoFine { get; set; }
    }

    public class OraTurnoMR
    {
        private int _ore;
        private int _minuti;
        private byte _dataModel;

        public OraTurnoMR()
        {
            _ore = 0;
            _minuti = 0;
            _dataModel = 0x00;

        }

        public OraTurnoMR(byte OraMinuti )
        {
            _ore = EstraiOre( OraMinuti);
            Ore = _ore;
            _minuti = EstraiMinuti( OraMinuti);
            Minuti = _minuti;

        }


        public OraTurnoMR(int ore , int minuti )
        {
            Ore = ore;
            Minuti = minuti;

        }

        public int EstraiOre (byte OraMinuti)
        {
            int _ore = 0;
            byte _tempTime = OraMinuti;

            try
            {
                _ore = (int)((OraMinuti & 0xFC) >> 2) ;

                if (_ore > 23)
                    _ore = 23;

                return _ore;
            }
            catch
            {
                _ore = 0;
                return _ore;
            }
        }

        public int EstraiMinuti(byte OraMinuti)
        {
            int _minuti = 0;
            byte _tempTime = OraMinuti;

            try
            {
                _minuti = (int)(OraMinuti & 0x03);

                if (_minuti > 3)
                    _minuti = 3;
                _minuti = _minuti * 15;
                return _minuti;
            }
            catch
            {
                _ore = 0;
                return _minuti;
            }
        }

       public int Ore
        {
            get
            {
                return _ore;
            }
            set
            {
                byte _TempOre;
                int _oreVal = value;
                if (_oreVal < 0) _oreVal = 0;
                if (_oreVal > 23) _oreVal = 23;
                _TempOre = (byte)_oreVal;

                _ore = _oreVal;
            
                _TempOre = (byte)(_oreVal << 2);
                _dataModel = (byte)((_dataModel & 0x03) | _TempOre);

            }
        }

        public int Minuti
        {
            get
            {
                return _minuti;
            }
            set
            {
                byte _TempMin;
                int _minVal = value;
                int _minCoded; 

                if (_minVal < 0) _minVal = 0;
                if (_minVal > 59) _minVal = 59;

                _minCoded = _minVal / 15;
                _minuti = _minCoded * 15;

                _TempMin = (byte)( _minCoded & 0x03 );

                _dataModel = (byte)((_dataModel & 0xFC) | _TempMin);

            }
        }

    }

}
