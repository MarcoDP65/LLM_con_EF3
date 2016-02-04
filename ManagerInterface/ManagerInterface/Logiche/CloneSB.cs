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
using MoriData;
using Utility;
using Newtonsoft.Json;

namespace ChargerLogic
{
    public class CloneSB
    {

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");


        public byte[] TestataScheda;
        public byte[] DatiCliente;
        public byte[] Programmazioni;
        public byte[] CicliLunghi;
        public byte[] CicliBrevi;

        int _numLunghi;
        int _numBrevi;
        int _numProg;

        int _ultimoProgramma;
        int _ultimoLungo;
        int _ultimoBreve;


        bool _datiPronti;

        int _dimProgrammazione;
        int _dimBreve;
        int _dimLungo;


        public CloneSB()
        {
            VuotaMatrici();
        }

        public void VuotaMatrici()
        {
            TestataScheda = new byte[64];
            DatiCliente = new byte[0];
            Programmazioni = new byte[0];
            CicliLunghi = new byte[0];
            CicliBrevi = new byte[0];

            _numLunghi =0;
            _numBrevi = 0;
            _numProg = 0;
            _ultimoProgramma = 0;

            _datiPronti = false;
            _dimProgrammazione = 128;
            _dimBreve = 26;
            _dimLungo = 57;
        }


        public elementiComuni.EsitoFunzione AggiungiProgrammazione(sbProgrammaRicarica Programma)
        {
            elementiComuni.EsitoFunzione _esito = elementiComuni.EsitoFunzione.ErroreGenerico ;
            byte[] _tempDati;
            try
            {
                if (Programma == null)
                    return elementiComuni.EsitoFunzione.DatiNonVAlidi;

                //controllo che sia realmente il successivo a quanto in memoria: se è il primo può anche essere <> 1
                if (_ultimoProgramma == 0)
                {
                    _ultimoProgramma = Programma.IdProgramma;
                }
                else
                {
                    if (Programma.IdProgramma == (_ultimoProgramma - 1))   // programmazioni decrescenti
                    {
                        _ultimoProgramma = Programma.IdProgramma;
                    }
                    else
                    {
                        return elementiComuni.EsitoFunzione.DatiNonVAlidi;
                    }
                }


                // incremento l'area dati
                int _oldSize = Programmazioni.Length;
                Array.Resize(ref Programmazioni, _oldSize + _dimProgrammazione);

                // recupero la mappa del programma
                _tempDati = new byte[_dimProgrammazione];
                _tempDati = Programma.DataArray;

                // Accodo i dati
                for(int _i = 0; _i < _dimProgrammazione; _i++ )
                {
                    Programmazioni[_oldSize + _i] = _tempDati[_i];
                }

                _numProg++;
                _esito = elementiComuni.EsitoFunzione.OK;
                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        public elementiComuni.EsitoFunzione AggiungiCicloLungo(sbMemLunga Lungo)
        {
            elementiComuni.EsitoFunzione _esito = elementiComuni.EsitoFunzione.ErroreGenerico;
            byte[] _tempDati;
            try
            {
                if (Lungo == null)
                    return elementiComuni.EsitoFunzione.DatiNonVAlidi;

                //controllo che sia realmente il successivo a quanto in memoria: se è il primo può anche essere <> 1
                if (_ultimoLungo == 0)
                {
                    _ultimoLungo = (int)Lungo.IdMemoriaLunga;
                }
                else
                {
                    if (Lungo.IdMemoriaLunga == (_ultimoLungo + 1))
                    {
                        _ultimoLungo = (int)Lungo.IdMemoriaLunga;
                    }
                    else
                    {
                        return elementiComuni.EsitoFunzione.DatiNonVAlidi;
                    }
                }


                // incremento l'area dati
                int _oldSize = CicliLunghi.Length;
                Array.Resize(ref CicliLunghi, _oldSize + _dimLungo);

                // recupero la mappa del programma
                _tempDati = new byte[_dimLungo];
                _tempDati = Lungo.DataArrayV4;

                Log.Debug("Ciclo " + Lungo.IdMemoriaLunga.ToString("000") + " " +  FunzioniComuni.HexdumpArray(_tempDati));

                // Accodo i dati
                for (int _i = 0; _i < _dimLungo; _i++)
                {
                    CicliLunghi[_oldSize + _i] = _tempDati[_i];
                }

                _numLunghi++;
                _esito = elementiComuni.EsitoFunzione.OK;
                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        public elementiComuni.EsitoFunzione AggiungiCicloBreve(sbMemBreve Breve)
        {
            elementiComuni.EsitoFunzione _esito = elementiComuni.EsitoFunzione.ErroreGenerico;
            byte[] _tempDati;
            try
            {
                if (Breve == null)
                    return elementiComuni.EsitoFunzione.DatiNonVAlidi;

                // incremento l'area dati
                int _oldSize = CicliBrevi.Length;
                Array.Resize(ref CicliBrevi, _oldSize + _dimBreve);

                // recupero la mappa del programma
                _tempDati = new byte[_dimBreve];
                _tempDati = Breve.DataArrayV4;

                // Accodo i dati
                for (int _i = 0; _i < _dimBreve; _i++)
                {
                    CicliBrevi[_oldSize + _i] = _tempDati[_i];
                }

                _numBrevi++;
                _esito = elementiComuni.EsitoFunzione.OK;
                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        public int NumCicliLunghi
        {
            get
            {
                return _numLunghi;
            }
        }

        public int NumCicliBrevi
        {
            get
            {
                return _numBrevi;
            }
        }

        public int NumeroProgrammazioni
        {
            get
            {
                return _numProg;
            }
        }

        public bool DatiPronti
        {
            get
            {
                return _datiPronti;
            }
        }
    }
}
