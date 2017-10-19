using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BrightIdeasSoftware;

using ChargerLogic;
using log4net;
using log4net.Config;

using MoriData;
using Utility;

namespace PannelloCharger
{
    public partial class frmCaricabatterie : Form

    {
    }

    public class llWaitEventStep : EventArgs
    {
        private int _numEventi;
        private int _evCorrente;
        private elementiComuni.tipoMessaggio _TipoDati;
        private elementiComuni.contenutoMessaggio _DatiRicevuti;

        // public enum tipoMessaggio : int { vuoto = 0, MemLunga = 1, MemBreve = 2, Programmi = 3 };
        // public enum contenutoMessaggio : int { vuoto = 0, Ack = 1, Dati = 2, Breack = 3, Nack = 10, NonValido = 99 };

        public int Eventi
        {
            set
            {
                _numEventi = value;
            }
            get
            {
                return this._numEventi;
            }
        }

        public int Step
        {
            set
            {
                _evCorrente = value;
            }
            get
            {
                return this._evCorrente;
            }
        }

        public elementiComuni.tipoMessaggio TipoDati
        {
            set
            {
                _TipoDati = value;
            }
            get
            {
                return this._TipoDati;
            }
        }

        public elementiComuni.contenutoMessaggio DatiRicevuti
        {
            set
            {
                _DatiRicevuti = value;
            }
            get
            {
                return this._DatiRicevuti;
            }
        }

    }


    public class llWaitStep
    {
        private int _numEventi;
        private int _evCorrente;
        private string _titolo;
        private bool _esecuzioneInterrotta;
        private elementiComuni.tipoMessaggio _TipoDati;
        private elementiComuni.contenutoMessaggio _DatiRicevuti;

        // public enum tipoMessaggio : int { vuoto = 0, MemLunga = 1, MemBreve = 2, Programmi = 3 };
        // public enum contenutoMessaggio : int { vuoto = 0, Ack = 1, Dati = 2, Breack = 3, Nack = 10, NonValido = 99 };

        public int Eventi
        {
            set
            {
                _numEventi = value;
            }
            get
            {
                return this._numEventi;
            }
        }

        public int Step
        {
            set
            {
                _evCorrente = value;
            }
            get
            {
                return this._evCorrente;
            }
        }

        public bool EsecuzioneInterrotta
        {
            set
            {
                _esecuzioneInterrotta = value;
            }
            get
            {
                return this._esecuzioneInterrotta;
            }
        }

        public string Titolo
        {
            set
            {
                _titolo = value;
            }
            get
            {
                return this._titolo;
            }
        }
        public elementiComuni.tipoMessaggio TipoDati
        {
            set
            {
                _TipoDati = value;
            }
            get
            {
                return this._TipoDati;
            }
        }

        public elementiComuni.contenutoMessaggio DatiRicevuti
        {
            set
            {
                _DatiRicevuti = value;
            }
            get
            {
                return this._DatiRicevuti;
            }
        }

    }


    public class llEndStep
    {
        private int _numEventiPrev;
        private int _ultimoEvento;
        private double _secondiTot;
        private elementiComuni.tipoMessaggio _TipoDati;
        private elementiComuni.contenutoMessaggio _DatiRicevuti;

        // public enum tipoMessaggio : int { vuoto = 0, MemLunga = 1, MemBreve = 2, Programmi = 3 };
        // public enum contenutoMessaggio : int { vuoto = 0, Ack = 1, Dati = 2, Breack = 3, Nack = 10, NonValido = 99 };

        public int EventiPrevisti
        {
            set
            {
                _numEventiPrev = value;
            }
            get
            {
                return this._numEventiPrev;
            }
        }

        public int UltimoEvento
        {
            set
            {
                _ultimoEvento = value;
            }
            get
            {
                return this._ultimoEvento;
            }
        }

        public double SecondiElaborazione
        {
            set
            {
                _secondiTot = value;
            }
            get
            {
                return this._secondiTot;
            }
        }

        public elementiComuni.tipoMessaggio TipoDati
        {
            set
            {
                _TipoDati = value;
            }
            get
            {
                return this._TipoDati;
            }
        }

        public elementiComuni.contenutoMessaggio DatiRicevuti
        {
            set
            {
                _DatiRicevuti = value;
            }
            get
            {
                return this._DatiRicevuti;
            }
        }

    }

}
