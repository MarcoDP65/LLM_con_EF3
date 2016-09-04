
 using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;

using System.Threading;
using System.ComponentModel;

using SQLite.Net;

namespace ChargerLogic
{
    /// <summary>
    /// Classe per la gestione informazioni del singoloturno
    /// </summary>
    class ModelloTurno
    {
        public enum TipoSchedulazione : byte
        {
            Tempo = 0x00,
            Turni387 = 0x01,
        };

        byte _tipoModello;
        ushort _minutiDurata;
        TipoSchedulazione _modoTurno;
        byte _inizioCambio;
        byte _finecambio;
        byte _fattoreCarica;
        byte _flagParametri;
        byte[] _modelloDati;

        public ModelloTurno ()
        {
            _modoTurno = TipoSchedulazione.Tempo;
            _minutiDurata = 0;
            _fattoreCarica = 102;
            _flagParametri = 0x00;
        }

        public bool fromData(byte[] ModelloDati )
        {
            try
            {
                if(ModelloDati.Length < 4)
                {
                    //Lunghezza pacchetto errata
                    return false;
                }

                if(_modoTurno == TipoSchedulazione.Tempo)
                {
                    _inizioCambio =0;
                    _finecambio = 0;

                    _minutiDurata = (ushort)( ModelloDati[0] >> 8 );
                    _minutiDurata += ModelloDati[1];
                    _fattoreCarica = ModelloDati[2];
                    _flagParametri = ModelloDati[3];
                }
                else
                {
                    _minutiDurata = 0;
                    _inizioCambio = ModelloDati[0];
                    _finecambio = ModelloDati[1];
                    _fattoreCarica = ModelloDati[2];
                    _flagParametri = ModelloDati[3];
                }


                return true;
            }
            catch
            {
                return false;
            }

        }

        public byte[] toData()
        {
            _modelloDati = new byte[4];
            try
            {


                if (_modoTurno == TipoSchedulazione.Tempo)
                {
                    byte HiVal = 0;
                    byte LoVal = 0;
                    FunzioniComuni.SplitUshort(_minutiDurata, ref LoVal, ref HiVal);

                }
                else
                {
                    _modelloDati[0] = _inizioCambio;
                    _modelloDati[1] = _finecambio;

                }

                _modelloDati[2] = _fattoreCarica;
                _modelloDati[3] = _flagParametri;

                return _modelloDati;
            }
            catch
            {
                return _modelloDati;
            }

        }





        public TipoSchedulazione ModoTurno
        {
            get
            {
                return _modoTurno;
            }
            set
            {
                _modoTurno = value;
            }
        }



    }
}
