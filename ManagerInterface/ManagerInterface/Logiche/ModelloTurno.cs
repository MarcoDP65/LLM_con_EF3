
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
    /// Classe per la gestione informazioni del singolo turno
    /// </summary>
    public class ModelloTurno
    {



        byte _tipoModello;
        //TipoSchedulazione _modoTurno;
        ParametriSetupPro.TipoPianificazione _modoTurno;
        ushort _minutiDurata;
        byte _inizioCambio;
        byte _finecambio;
        byte _fattoreCarica;
        byte _flagParametri;
        bool _flagEqual;
        bool _flagBiber;
        bool _flagRabbocco;
        bool _flagStartDelayed;
        bool _flagDeleteDelay;

        byte _StartEqual;
        ushort _orarioStartCarica;  // in minuti
        byte _oraStartCarica;
        byte _minStartCarica;

        ushort _maxMinutiAnticipo;  
        ParametriSetupPro.RitardoAvvio _flagRitardoAvvio;




        byte[] _modelloDati;

        public ModelloTurno ()
        {
            _modoTurno = ParametriSetupPro.TipoPianificazione.Tempo;
            _minutiDurata = 0;
            _fattoreCarica = 101;
            _flagParametri = 0x00;
            _flagEqual = false;
            _StartEqual = 0;
            _orarioStartCarica = 0;
            _maxMinutiAnticipo = 0;
            _flagRitardoAvvio = ParametriSetupPro.RitardoAvvio.RitOFF_ForceOFF;
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

                switch (_modoTurno)
                {
                    case ParametriSetupPro.TipoPianificazione.NonDefinita:
                        {
                            _inizioCambio = 0;
                            _finecambio = 0;

                            _minutiDurata = 0;
                            _fattoreCarica = 0;
                            _flagParametri =0;
                            _flagEqual = false;
                            _flagBiber = false;
                            _flagRabbocco = false;
                            _flagStartDelayed = false;
                            _flagDeleteDelay = false;

                            break;
                        }
                    case ParametriSetupPro.TipoPianificazione.Tempo:
                        {
                            _inizioCambio = 0;
                            _finecambio = 0;

                            _minutiDurata = (ushort)(ModelloDati[0] << 8);
                            _minutiDurata += ModelloDati[1];
                            _fattoreCarica = ModelloDati[2];
                            _flagParametri = ModelloDati[3];
                            _flagEqual = false;
                            _flagBiber = false;
                            _flagRabbocco = false;
                            _flagStartDelayed = false;
                            _flagDeleteDelay = false;
                            _flagEqual = ModelloDati[4]==1;
                            _StartEqual = ModelloDati[5];

                            break;
                        }
                        
                    case ParametriSetupPro.TipoPianificazione.Turni:
                        {
                            _minutiDurata = 0;
                            _inizioCambio = ModelloDati[0];
                            _finecambio = ModelloDati[1];
                            _fattoreCarica = ModelloDati[2];
                            _flagParametri = ModelloDati[3];
                            _flagEqual = false;
                            _flagBiber = false;
                            _flagRabbocco = false;
                            _flagStartDelayed = false;
                            _flagDeleteDelay = false;

                            _StartEqual = 0;
                            break;
                        }
                    case ParametriSetupPro.TipoPianificazione.TempoEsteso:
                        { 
                            _inizioCambio = 0;
                            _finecambio = 0;

                            _minutiDurata = (ushort)(ModelloDati[0] << 8);
                            _minutiDurata += ModelloDati[1];
                            _fattoreCarica = ModelloDati[2];
                            _flagParametri = ModelloDati[3];

                            _flagEqual = FunzioniBinarie.GetBit(ModelloDati[3], (int)ParametriSetupPro.BitParametro. Equal);
                            _flagBiber = FunzioniBinarie.GetBit(ModelloDati[3], (int)ParametriSetupPro.BitParametro.Biberonaggio);
                            _flagRabbocco = FunzioniBinarie.GetBit(ModelloDati[3], (int)ParametriSetupPro.BitParametro.Rabboccatore);
                            _flagStartDelayed = FunzioniBinarie.GetBit(ModelloDati[3], (int)ParametriSetupPro.BitParametro.StartDelayed);
                            _flagDeleteDelay = FunzioniBinarie.GetBit(ModelloDati[3], (int)ParametriSetupPro.BitParametro.DeleteDelay);
                            /*
                            _oraStartCarica = ModelloDati[4];
                            _minStartCarica = ModelloDati[5];
                            */
                            _orarioStartCarica = (ushort)(ModelloDati[4] << 8);
                            _orarioStartCarica += ModelloDati[5];

                            _maxMinutiAnticipo = (ushort)(ModelloDati[6] << 8);
                            _maxMinutiAnticipo += ModelloDati[7];

                            break;
                        }

                    default:
                        {
                            _inizioCambio = 0;
                            _finecambio = 0;

                            _minutiDurata = 0;
                            _fattoreCarica = 0;
                            _flagParametri = 0;

                            break;
                        }
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
            byte HiVal;
            byte LoVal;

            try
            {

                switch (_modoTurno)
                {
                    case ParametriSetupPro.TipoPianificazione.NonDefinita:
                        _modelloDati[0] = 0;
                        _modelloDati[1] = 0;
                        break;
                    case ParametriSetupPro.TipoPianificazione.Tempo:
                        _modelloDati = new byte[6];
                        HiVal = 0;
                        LoVal = 0;
                        FunzioniComuni.SplitUshort(_minutiDurata, ref LoVal, ref HiVal);
                        _modelloDati[0] = HiVal;
                        _modelloDati[1] = LoVal;
                        _modelloDati[4] = (byte)(_flagEqual ? 1:0) ;
                        _modelloDati[5] = _StartEqual;
                        break;
                    case ParametriSetupPro.TipoPianificazione.TempoEsteso:
                        _modelloDati = new byte[24];
                        HiVal = 0;
                        LoVal = 0;
                        FunzioniComuni.SplitUshort(_minutiDurata, ref LoVal, ref HiVal);
                        _modelloDati[0] = HiVal;
                        _modelloDati[1] = LoVal;
                        _modelloDati[2] = _fattoreCarica;

                        _flagParametri = FunzioniBinarie.SetBit(_flagParametri, (int)ParametriSetupPro.BitParametro.Equal, _flagEqual);
                        _flagParametri = FunzioniBinarie.SetBit(_flagParametri, (int)ParametriSetupPro.BitParametro.Biberonaggio, _flagBiber);
                        _flagParametri = FunzioniBinarie.SetBit(_flagParametri, (int)ParametriSetupPro.BitParametro.Rabboccatore, _flagRabbocco);
                        _flagParametri = FunzioniBinarie.SetBit(_flagParametri, (int)ParametriSetupPro.BitParametro.StartDelayed, _flagStartDelayed);
                        _flagParametri = FunzioniBinarie.SetBit(_flagParametri, (int)ParametriSetupPro.BitParametro.DeleteDelay, _flagDeleteDelay);
                        _modelloDati[3] = _flagParametri;
                        /*
                        _modelloDati[4] = _oraStartCarica;
                        _modelloDati[5] = _minStartCarica;
                        */
                        FunzioniComuni.SplitUshort(_orarioStartCarica, ref LoVal, ref HiVal);
                        _modelloDati[4] = HiVal;
                        _modelloDati[5] = LoVal;

                        FunzioniComuni.SplitUshort(_maxMinutiAnticipo, ref LoVal, ref HiVal);
                        _modelloDati[6] = HiVal;
                        _modelloDati[7] = LoVal;

                        break;

                    case ParametriSetupPro.TipoPianificazione.Turni:
                        _modelloDati[0] = _inizioCambio;
                        _modelloDati[1] = _finecambio;
                        break;
                    default:
                        _modelloDati[0] = 0;
                        _modelloDati[1] = 0;
                        break;
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



        public ParametriSetupPro.TipoPianificazione ModoTurno
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

        public ushort MinutiDurata
        {
            get
            {
                return _minutiDurata;
            }
            set
            {
                _minutiDurata = value;
            }
        }


        public byte InizioCambio
        {
            get
            {
                return _inizioCambio;
            }
            set
            {
                _inizioCambio = value;
            }
        }

        public byte FineCambio
        {
            get
            {
                return _finecambio;
            }
            set
            {
                _finecambio = value;
            }
        }


        public byte FattoreCarica
        {
            get
            {
                return _fattoreCarica;
            }
            set
            {
                _fattoreCarica = value;
            }
        }

        public byte FlagParametri
        {
            get
            {
                return _flagParametri;
            }
            set
            {
                _flagParametri = value;
            }
        }

        public bool flagEqual
        {
            get
            {
                return _flagEqual;
            }
            set
            {

                
                _flagEqual = value;
            }
        }

        public byte StartEqual
        {
            get
            {
                return _StartEqual;
            }
            set
            {
                _StartEqual = value;
            }
        }

        public bool flagBiber
        {
            get
            {
                return _flagBiber;
            }
            set
            {
                _flagBiber = value;
            }
        }

        public bool flagRabbocco
        {
            get
            {
                return _flagRabbocco;
            }
            set
            {


                _flagRabbocco = value;
            }
        }

        public bool flagStartDelayed
        {
            get
            {
                return _flagStartDelayed;
            }
            set
            {


                _flagStartDelayed = value;
            }
        }

        public bool flagDeleteDelay
        {
            get
            {
                return _flagDeleteDelay;
            }
            set
            {


                _flagDeleteDelay = value;
            }
        }


        public byte OraStartCarica
        {
            get
            {
                return _oraStartCarica;
            }
            set
            {
                if (value > 23)
                {
                    value = 23;
                }
                _oraStartCarica = value;
            }
        }

        public byte MinutiStartCarica
        {
            get
            {
                return _minStartCarica;
            }
            set
            {
                if (value > 59)
                {
                    value = 59;
                }
                _minStartCarica = value;

            }
        }

        public ushort OrarioStartCarica
        {
            get
            {
                return _orarioStartCarica;
            }
            set
            {

                _orarioStartCarica = value;

            }
        }

        public ushort MaxMinutiAnticipo
        {
            get
            {
                return _maxMinutiAnticipo;
            }
            set
            {

                _maxMinutiAnticipo = value;

            }
        }

    }

    public class ModelloGiorno
    {
        private ParametriSetupPro.TipoPianificazione _TipoModello;
        private int _numeroTurni;
        public ModelloTurno[] Turno;
        public int DayOfTheWeek { get; set; }

        byte[] _modelloDati;

        public ParametriSetupPro.TipoPianificazione TipoModello
        {
            get
            {
                return _TipoModello;
            }
        }

        public bool ImpostaModello(ParametriSetupPro.TipoPianificazione Modello)
        {
            bool esito = false;
 
            try
            {
                switch (Modello)
                {
                    case ParametriSetupPro.TipoPianificazione.NonDefinita:
                        _numeroTurni = 1;
                        Turno = new ModelloTurno[_numeroTurni];
                        Turno[0] = new ModelloTurno();
                        Turno[0].ModoTurno = ParametriSetupPro.TipoPianificazione.NonDefinita;
                        Turno[0].MinutiDurata = 0;
                        Turno[0].FattoreCarica = 0;
                        break;
                    case ParametriSetupPro.TipoPianificazione.Tempo:
                        _numeroTurni = 1;
                        Turno = new ModelloTurno[_numeroTurni];
                        Turno[0] = new ModelloTurno();
                        Turno[0].ModoTurno = ParametriSetupPro.TipoPianificazione.Tempo;
                        Turno[0].MinutiDurata = 660;
                        Turno[0].FattoreCarica = 101;
                        break;
                    case ParametriSetupPro.TipoPianificazione.Turni:
                        _numeroTurni = 3;
                        Turno = new ModelloTurno[_numeroTurni];
                        for (int _i = 0; _i < _numeroTurni; _i++)
                        {
                            Turno[_i] = new ModelloTurno();
                            Turno[_i].ModoTurno = ParametriSetupPro.TipoPianificazione.Turni;
                            Turno[_i].FattoreCarica = 102;
                        }
                        break;
                    case ParametriSetupPro.TipoPianificazione.TempoEsteso:
                        _numeroTurni = 1;
                        Turno = new ModelloTurno[_numeroTurni];
                        Turno[0] = new ModelloTurno();
                        Turno[0].ModoTurno = ParametriSetupPro.TipoPianificazione.TempoEsteso;
                        Turno[0].MinutiDurata = 660;
                        Turno[0].FattoreCarica = 101;
                        Turno[0].flagEqual = false;
                        Turno[0].flagRabbocco = false;
                        Turno[0].flagBiber = false;
                        Turno[0].flagStartDelayed = false;
                        Turno[0].MinutiStartCarica = 0;
                        Turno[0].MaxMinutiAnticipo = 0;
                        Turno[0].flagDeleteDelay = false; 

                        break;
                    default:
                        _numeroTurni = 1;
                        Turno = new ModelloTurno[_numeroTurni];
                        Turno[0].ModoTurno = ParametriSetupPro.TipoPianificazione.NonDefinita;
                        Turno[0].MinutiDurata = 0;
                        Turno[0].FattoreCarica = 0;
                        break;
                }


                return esito;
            }
            catch
            {
                esito = false;
                return esito;
            }
        }

        public bool fromData(byte[] ModelloDati)
        {
            try
            {
                if (ModelloDati.Length < 4)
                {
                    //Lunghezza pacchetto errata
                    return false;
                }
                _modelloDati = new byte[ModelloDati.Length];
                for (int _m = 0; _m < ModelloDati.Length; _m++)
                {
                    _modelloDati[_m] = ModelloDati[_m];
                }

                switch (_TipoModello)
                {
                    case ParametriSetupPro.TipoPianificazione.NonDefinita:
                        _numeroTurni = 1;
                        Turno = new ModelloTurno[_numeroTurni];
                        Turno[0].ModoTurno = ParametriSetupPro.TipoPianificazione.NonDefinita;
                        Turno[0].MinutiDurata = 0;
                        Turno[0].FattoreCarica = 0;
                        break;
                    case ParametriSetupPro.TipoPianificazione.Tempo:
                        _numeroTurni = 1;
                        Turno = new ModelloTurno[_numeroTurni];
                        Turno[0].ModoTurno = ParametriSetupPro.TipoPianificazione.Tempo;
                        Turno[0].MinutiDurata = 480;
                        Turno[0].FattoreCarica = 102;
                        break;
                    case ParametriSetupPro.TipoPianificazione.Turni:
                        _numeroTurni = 3;
                        Turno = new ModelloTurno[_numeroTurni];
                        for (int _i = 0; _i < _numeroTurni; _i++)
                        {
                            Turno[_i].ModoTurno = ParametriSetupPro.TipoPianificazione.Turni;
                            Turno[_i].FattoreCarica = 102;
                        }
                        break;
                    case ParametriSetupPro.TipoPianificazione.TempoEsteso:
                        _numeroTurni = 1;
                        Turno = new ModelloTurno[_numeroTurni];
                        Turno[0].ModoTurno = ParametriSetupPro.TipoPianificazione.TempoEsteso;
                        Turno[0].MinutiDurata = 480;
                        Turno[0].FattoreCarica = 102;
                        break;
                    default:
                        _numeroTurni = 1;
                        Turno = new ModelloTurno[_numeroTurni];
                        Turno[0].ModoTurno = ParametriSetupPro.TipoPianificazione.NonDefinita;
                        Turno[0].MinutiDurata = 0;
                        Turno[0].FattoreCarica = 0;
                        break;
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
            _modelloDati = new byte[12];
            try
            {
                switch (_TipoModello)
                {
                    case ParametriSetupPro.TipoPianificazione.NonDefinita:
                        break;
                    case ParametriSetupPro.TipoPianificazione.Tempo:
                        break;
                    case ParametriSetupPro.TipoPianificazione.Turni:
                        break;
                    default:
                        break;
                }

                return _modelloDati;
            }
            catch
            {
                return _modelloDati;
            }

        }



    }

    public class ModelloSettimana
    {

        private ParametriSetupPro.TipoPianificazione _TipoModello;
        private int _numeroTurni;
        private int _numeroGiorni = 7;
        public ModelloGiorno[] Giornata;
        byte[] DataModel;
       

        public ParametriSetupPro.TipoPianificazione TipoModello
        {
            get
            {
                return _TipoModello;
            }
        }


        public bool ImpostaModello(ParametriSetupPro.TipoPianificazione Modello)
        {
            bool esito = false;

            try
            {
                _numeroGiorni = 7;
                Giornata = new ModelloGiorno[_numeroGiorni];

                for (int _i = 0; _i < _numeroGiorni; _i++)

                {
                    Giornata[_i] = new ModelloGiorno();
                    Giornata[_i].ImpostaModello(Modello);
                    Giornata[_i].DayOfTheWeek = _i + 1;
                }
                _TipoModello = Modello;
                return esito;
            }
            catch
            {
                esito = false;
                return esito;
            }
        }

        public int NumeroGiorni
        {
            get
            {
                return _numeroGiorni;
            }

        }

        public bool DecodificaPacchetto(ParametriSetupPro.TipoPianificazione Modello, byte[] Dati)
        {
            bool esito = false;

            try
            {
                switch (Modello)
                {
                    case ParametriSetupPro.TipoPianificazione.NonDefinita:
                        break;
                    case ParametriSetupPro.TipoPianificazione.Tempo:
                        break;
                    case ParametriSetupPro.TipoPianificazione.Turni:
                        break;
                    default:
                        break;
                }


                return esito;
            }
            catch
            {
                esito = false;
                return esito;
            }


        }


        byte _oraStartCarica;
        byte _minStartCarica;

        ushort _maxMinutiAnticipo;


    }

}
