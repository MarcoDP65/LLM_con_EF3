
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
        /*
        public enum TipoSchedulazione : byte
        {
            Tempo = 0x00,
            Turni3x7 = 0x01,
        };
        */
        byte _tipoModello;
        //TipoSchedulazione _modoTurno;
        ParametriSetupPro.TipoPianificazione _modoTurno;
        ushort _minutiDurata;
        byte _inizioCambio;
        byte _finecambio;
        byte _fattoreCarica;
        byte _flagParametri;
        byte[] _modelloDati;

        public ModelloTurno ()
        {
            _modoTurno = ParametriSetupPro.TipoPianificazione.Tempo;
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

                switch (_modoTurno)
                {
                    case ParametriSetupPro.TipoPianificazione.NonDefinita:
                        {
                            _inizioCambio = 0;
                            _finecambio = 0;

                            _minutiDurata = 0;
                            _fattoreCarica = 0;
                            _flagParametri =0;

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

                            break;
                        }
                        
                    case ParametriSetupPro.TipoPianificazione.Turni:
                        {
                            _minutiDurata = 0;
                            _inizioCambio = ModelloDati[0];
                            _finecambio = ModelloDati[1];
                            _fattoreCarica = ModelloDati[2];
                            _flagParametri = ModelloDati[3];

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
            try
            {
                switch (_modoTurno)
                {
                    case ParametriSetupPro.TipoPianificazione.NonDefinita:
                        _modelloDati[0] = 0;
                        _modelloDati[1] = 0;
                        break;
                    case ParametriSetupPro.TipoPianificazione.Tempo:
                        byte HiVal = 0;
                        byte LoVal = 0;
                        FunzioniComuni.SplitUshort(_minutiDurata, ref LoVal, ref HiVal);
                        _modelloDati[0] = HiVal;
                        _modelloDati[1] = LoVal;
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
                        Turno[0].MinutiDurata = 480;
                        Turno[0].FattoreCarica = 102;
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


    }

}
