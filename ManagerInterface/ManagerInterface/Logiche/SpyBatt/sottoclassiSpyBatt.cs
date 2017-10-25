using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

using SQLite.Net;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;

namespace ChargerLogic
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UnitaSpyBatt
    {

    }

    public class sbListaLunghiEvt : EventArgs
    {
        private int numLunghe;
        public int EventiLunghi
        {
            set
            {
                numLunghe = value;
            }
            get
            {
                return this.numLunghe;
            }
        }
    }

/*
    public class sbWaitEventStep : EventArgs
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

    public class sbWaitStep
    {
        private int _numEventi;
        private int _evCorrente;
        private string _titolo;
        private bool _esecuzioneInterrotta;
        private elementiComuni.tipoMessaggio _TipoDati;
        private elementiComuni.contenutoMessaggio _DatiRicevuti;

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


    public class sbEndStep
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
    */
    /// <summary>
    /// Registrazione (record e collezione)
    /// Struttura per il passaggio di elenchi nei metodo di estrazione / ricerca
    /// </summary>

    public class Registrazione
    {
        public int IdLocale { get; set; }
        public int IdRegistrazione { get; set; }
        public uint Pointer { get; set; }
        public int NumFigli { get; set; }
        public object Record { get; set; }
        public bool Flag01 { get; set; }
        public bool Flag02 { get; set; }
    }

    public class ListaRegistrazioni
    {
        private int _numRecord;
        private int _numFigli;
        public List<Registrazione> Elenco;

        public ListaRegistrazioni()
        {
            Elenco = new List<Registrazione>();
            _numFigli = 0;
        }

        public int RecordMaster
        {
            set
            {
                _numRecord = value;
            }
            get
            {
                return this._numRecord;
            }
        }
        public int RecordFigli
        {
            set
            {
                _numFigli = value;
            }
            get
            {
                return this._numFigli;
            }
        }
    }

    public class ValoreLista
    {
        public string descrizione { get; set; }
        public SerialMessage.OcBaudRate BrSettingValue { get; set; }
        public SerialMessage.OcEchoMode EcSettingValue { get; set; }

        public Boolean enabled { get; set; }

        public ValoreLista()
        {
            descrizione = "";
            BrSettingValue = SerialMessage.OcBaudRate.OFF;
            EcSettingValue = SerialMessage.OcEchoMode.OFF;
            enabled = false;
        }

        public ValoreLista(string ValDescrizione, SerialMessage.OcBaudRate BRValSettingValue, Boolean ValEnabled)
        {
            descrizione = ValDescrizione;
            BrSettingValue = BRValSettingValue;
            EcSettingValue = SerialMessage.OcEchoMode.OFF;
            enabled = ValEnabled;
        }

        public ValoreLista(string ValDescrizione, SerialMessage.OcEchoMode ECValSettingValue, Boolean ValEnabled)
        {
            descrizione = ValDescrizione;
            BrSettingValue = SerialMessage.OcBaudRate.OFF;
            EcSettingValue = ECValSettingValue;
            enabled = ValEnabled;
        }

    }

}
