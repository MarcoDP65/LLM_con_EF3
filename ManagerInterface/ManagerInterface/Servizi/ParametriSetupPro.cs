using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using System.Drawing.Printing;

namespace ChargerLogic
{
    public class ParametriSetupPro
    {
        public enum ModoRicarica : byte { NonDefinito = 0x00, ProfiloFisso = 0x01, Strategia = 0x02};
        public enum TipoPianificazione : byte { NonDefinita = 0x00, Tempo = 0x01, Turni = 0x02,TempoEsteso = 0x11, TurniEsteso = 0x12 };
        public enum ModoEqualizzazione : byte { NO = 0x00, Full = 0x01};
        public enum BitParametro : byte { Equal= 0, Rabboccatore = 1,Biberonaggio = 2,StartDelayed = 4,DeleteDelay = 5};
        public enum RitardoAvvio : byte
        {
            RitOFF_ForceOFF = 0x00,
            RitON_ForceOFF = 0x10,
            RitON_ForceON = 0x11,
        }

        public List<Pianificazione> ModiRicarica = new List<Pianificazione>
                               {
                                    new Pianificazione { Codice = (byte)ModoRicarica.NonDefinito, Descrizione  = PannelloCharger.StringheComuni.PianificazioneNessuna,FwLevelMin = 0 ,FwLevelMax = 999},
                                    new Pianificazione { Codice = (byte)ModoRicarica.ProfiloFisso, Descrizione  = PannelloCharger.StringheComuni.PianificazioneTempo,FwLevelMin = 0 ,FwLevelMax = 9},
                                    new Pianificazione { Codice = (byte)ModoRicarica.Strategia, Descrizione  = PannelloCharger.StringheComuni.PianificazioneTempoExt,FwLevelMin = 8 ,FwLevelMax = 999},

                               };


        public List<Pianificazione> TipiPianificazione = new List<Pianificazione>
                               {
                                    new Pianificazione { Codice = (byte)TipoPianificazione.NonDefinita, Descrizione  = PannelloCharger.StringheComuni.PianificazioneNessuna,FwLevelMin = 0 ,FwLevelMax = 999},
                                    new Pianificazione { Codice = (byte)TipoPianificazione.Tempo, Descrizione  = PannelloCharger.StringheComuni.PianificazioneTempo,FwLevelMin = 0 ,FwLevelMax = 9},
                                    //new Pianificazione { Codice = 2, Descrizione = "Turni"}  - pianificazione per turni temporaneamente disabilitata
                                    new Pianificazione { Codice = (byte)TipoPianificazione.TempoEsteso, Descrizione  = PannelloCharger.StringheComuni.PianificazioneTempoExt,FwLevelMin = 8 ,FwLevelMax = 999},

                               };

        public List<Pianificazione> OpzioniBib = new List<Pianificazione>
                               {
                                    new Pianificazione { Codice = 0X00, Descrizione  ="OFF"},
                                    new Pianificazione { Codice = 0X01, Descrizione = "Rich."},
                                    new Pianificazione { Codice = 0X0F, Descrizione = "ON"},
                               };

        public List<Pianificazione> OpzioniRabb = new List<Pianificazione>
                               {
                                    new Pianificazione { Codice = 0X00, Descrizione  ="OFF"},
                                    new Pianificazione { Codice = 0X0F, Descrizione = "ON"},
                               };

    

        public List<ValoreByte> FCbase = new List<ValoreByte>
                               {
                                    new ValoreByte(100),
                                    new ValoreByte(101),
                                    new ValoreByte(102),
                                    new ValoreByte(103),
                                    new ValoreByte(104),
                                    new ValoreByte(105),
                                    new ValoreByte(106),
                                    new ValoreByte(107),
                                    new ValoreByte(108),
                                    new ValoreByte(109),
                                    new ValoreByte(110),
                                    new ValoreByte(111),
                                    new ValoreByte(112),
                                    new ValoreByte(113),
                                    new ValoreByte(114),
                                    new ValoreByte(115),

                               };

        public List<ValoreByte> FCprofondo = new List<ValoreByte>
                               {
                                    new ValoreByte(115),
                                    new ValoreByte(116),
                                    new ValoreByte(117),
                                    new ValoreByte(118),
                                    new ValoreByte(119),
                                    new ValoreByte(120),
                                    new ValoreByte(121),
                                    new ValoreByte(122),
                                    new ValoreByte(123),
                                    new ValoreByte(124),
                                    new ValoreByte(125),
                                    new ValoreByte(130),
                                    new ValoreByte(135),


                               };

        public List<ValoreByte> MaxSbilanciamento = new List<ValoreByte>
                               {
                                    new ValoreByte(5),
                                    new ValoreByte(6),
                                    new ValoreByte(7),
                                    new ValoreByte(8),
                                    new ValoreByte(9),
                                    new ValoreByte(10),
                                    new ValoreByte(11),
                                    new ValoreByte(12),
                                    new ValoreByte(13),
                                    new ValoreByte(14),
                                    new ValoreByte(15),
                                    new ValoreByte(20),
                                    new ValoreByte(25),
                               };


    }

    public class Pianificazione
    {
        byte _codice;
        string _descrizione;
        int _fwLevelMin;
        int _fwLevelMax;


        public ParametriSetupPro.TipoPianificazione CodiceTP
        {
            get
            {
                return (ParametriSetupPro.TipoPianificazione)_codice;
            }
        }

        public byte Codice
        {
            get
            {
                return _codice;
            }

            set
            {
                _codice = value;
            }
        }

        public string Descrizione
        {
            get
            {
                return _descrizione;
            }

            set
            {
                _descrizione = value;
            }
        }

        public int FwLevelMin
        {
            get
            {
                return _fwLevelMin;
            }

            set
            {
                _fwLevelMin = value;
            }
        }

        public int FwLevelMax
        {
            get
            {
                return _fwLevelMax;
            }

            set
            {
                _fwLevelMax = value;
            }
        }


    }

    public class ValoreCorrente
    {
        ushort _corrente;

        public float Valore
        {
            get
            {
                return (float)_corrente/100;
            }
            set
            {

            }
        }
        public ushort ValoreUS
        {


            get
            {
                return _corrente;
            }
            set
            {
                _corrente = value;
            }
        }

    }

    public class ValoreByte
    {
        byte _valore;


        public ValoreByte()
        {
            _valore = 0;
        }

        public ValoreByte(byte valore)
        {
            _valore = valore;
        }



        public float Valore
        {
            get
            {
                return (float)_valore / 100;
            }
        }

        public string strValore
        {
            get
            {
                float _tempVal = (float)_valore / 100;
                return _tempVal.ToString("0.00");
            }
        }


        public byte ValoreB
        {


            get
            {
                return _valore;
            }
            set
            {
                _valore = value;
            }
        }



    }

}
