using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PannelloCharger
{
    public class GestioneTurno
    {
        public enum ModoCorrente : byte
        {
            none = 0x00,
            capacita = 0x01,
            tempo = 0x02,

        }
        public const byte MASK_BASE = 0x00;
        public const byte MASK_CURRENT = 0x03;
        public const byte MASK_EQUAL = 0x04;
        public const byte MASK_BIBER = 0x08;
        public const byte MASK_STATO = 0x80;

    }

    public class ParametriTurno
    {
       // int _numeroTurno;
        int _giornoSettimana;
        OraTurnoMR _inizioCambioTurno;
        OraTurnoMR _fineCambioTurno;
        int _fattoreCarica;
        OpzioniTurno _parametri;
        byte[] _datiTurno;

        public ParametriTurno()
        {
            try
            {
                _inizioCambioTurno = new OraTurnoMR();
                _fineCambioTurno = new OraTurnoMR();
                _fattoreCarica = 100;
                _parametri = new OpzioniTurno();
                _datiTurno = new byte[4];
            }
            catch
            {

            }
        }

        public bool CaricaDati(byte[] Dati)
        {
            try
            {
                bool _esito = false;
                byte _tempdata;
                if (Dati.Length != 4)
                {
                    //lunghezza errata
                    _esito = false;
                    return _esito;
                }

                // Ora inizio
                _tempdata = Dati[0];
                _inizioCambioTurno = new OraTurnoMR(_tempdata);

                // Ora fine
                _tempdata = Dati[1];
                _fineCambioTurno = new OraTurnoMR(_tempdata);

                // Fattore di Carica
                _tempdata = Dati[2];
                _fattoreCarica = (int)_tempdata;




                return _esito ;
            }
            catch
            {
                return false;
            }
        }

    }





    public class OpzioniTurno
    {
        // Singolo byte per la definizione fei parametri turno
        //
        // 7  128 - Stato Turno: Attivo / Riposo   
        // 6   64 - *
        // 5   32 - *
        // 4   16 - * 
        // 3   08 - Flag Biberonaggio
        // 2   04 - Flag Equalizzazone
        // 1   02 - due bit ( quattro opzioni ) 
        // 0   01 - per la definizione del fattore base di calcolo della corrente

        private bool _equalAttivo;
        private bool _biberAttivo;
        private byte _modoCorrente;
        private bool _turnoAttivo;

        private byte _valoreOpzioni;

        public  OpzioniTurno ()
        {
            _equalAttivo = false;
            _biberAttivo = false;
            _turnoAttivo = false;

            _modoCorrente = (byte) GestioneTurno.ModoCorrente.none;

            _valoreOpzioni = 0x00;

        }


        #region PARAMETRI


        public byte StatoBiberonaggio
        {
            get
            {
                if (_biberAttivo)
                    return GestioneTurno.MASK_BIBER;
                else
                    return 0;


            }
        }

        public bool Biberonaggio
        {
            get
            {
                return _biberAttivo;
            }
            set
            {
                _biberAttivo = value;
                _valoreOpzioni = ByteBitWise.BitSet(_valoreOpzioni, _biberAttivo, GestioneTurno.MASK_BIBER);
            }


        }

        public byte StatoEqualizzazione
        {
            get
            {
                if (_equalAttivo)
                    return GestioneTurno.MASK_EQUAL;
                else
                    return 0;


            }
        }

        public bool Equalizzazione
        {
            get
            {
                return _equalAttivo;
            }
            set
            {
                _equalAttivo = value;
                _valoreOpzioni = ByteBitWise.BitSet(_valoreOpzioni, _equalAttivo, GestioneTurno.MASK_EQUAL);
            }


        }

        public byte StatoTurno
        {
            get
            {
                if (_turnoAttivo)
                    return GestioneTurno.MASK_STATO;
                else
                    return 0;


            }
        }

        public bool TurnoAttivo
        {
            get
            {
                return _turnoAttivo;
            }
            set
            {
                _turnoAttivo = value;
                _valoreOpzioni = ByteBitWise.BitSet(_valoreOpzioni, _turnoAttivo, GestioneTurno.MASK_STATO);
            }


        }


        public byte ModoCorrente
        {
            get
            {
                return (byte)(_modoCorrente & GestioneTurno.MASK_CURRENT);
            }
            set
            {
                _modoCorrente = (byte)(value & GestioneTurno.MASK_CURRENT);
            }
        }



        public byte StatoParametri
        {
            get
            {

                    return _valoreOpzioni;


            }
            set
            {
                _valoreOpzioni = value;

                _equalAttivo = ByteBitWise.BitVerify(_valoreOpzioni, GestioneTurno.MASK_EQUAL);
                _biberAttivo = ByteBitWise.BitVerify(_valoreOpzioni, GestioneTurno.MASK_BIBER);
                _turnoAttivo = ByteBitWise.BitVerify(_valoreOpzioni, GestioneTurno.MASK_STATO);
                _modoCorrente = ByteBitWise.ByteVerify(_valoreOpzioni, GestioneTurno.MASK_CURRENT);

            }
        }



        # endregion
    }
}
