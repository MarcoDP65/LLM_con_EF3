﻿using System;
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
        public enum TipoPianificazione : byte { NonDefinita = 0x00, Tempo = 0x01, Turni = 0x02 };
        public List<Pianificazione> TipiPianificazione = new List<Pianificazione>
                               {
                                    new Pianificazione { Codice = 0, Descrizione  ="Tempo"},
                                    new Pianificazione { Codice = 1, Descrizione = "Turni"}
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
