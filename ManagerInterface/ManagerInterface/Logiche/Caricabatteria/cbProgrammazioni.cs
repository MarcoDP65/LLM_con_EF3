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
    public class cbProgrammazioni
    {
        public ushort UltimoIdProgamma { get; set; }
        public byte NumeroRecordProg { get; set; }
        public llProgrammaCarica ProgrammaAttivo;
        public List<llProgrammaCarica> ProgrammiDefiniti;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("cbProgrammazioni");
        public bool _datiSalvati;
        public bool _recordPresente;

        public string IdCorrente { get; set; }

        public cbProgrammazioni()
        {

            _database = null;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public cbProgrammazioni(_db connessione)
        {

            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public llProgrammaCarica ProgrammazioneAttiva
        {
            get
            {
                if (ProgrammiDefiniti == null) return null;
                if (ProgrammiDefiniti.Count < 1) return null;
                return ProgrammiDefiniti.Find(x => x.PosizioneCorrente == 0);
            }
        }

        public bool SalvaDati()
        {
            return false;
        }



/*
        public bool CaricaProgrammaAttivo()
        {
            try
            {
                ProgrammaAttivo = CaricaProgramma(0);

                return true;

            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return false;
            }
        }

        public llProgrammaCarica CaricaProgramma(byte IdPosizione)
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;
                llProgrammaCarica tempPrg = new llProgrammaCarica();
                MessaggioLadeLight.MessaggioProgrammazione ImmagineCarica = new MessaggioLadeLight.MessaggioProgrammazione();
                SerialMessage.EsitoRisposta EsitoMsg;

                if (IdPosizione > 15) IdPosizione = 15;

                uint StartAddr = (uint)(0x2000 + (256 * IdPosizione));

                byte[] _datiTemp = new byte[226];
                _esito = LeggiBloccoMemoria(StartAddr, 226, out _datiTemp);

                if (_esito)
                {
                    EsitoMsg = ImmagineCarica.analizzaMessaggio(_datiTemp, 1);
                    if (EsitoMsg == SerialMessage.EsitoRisposta.MessaggioOk)
                    {
                        tempPrg.IdProgramma = ImmagineCarica.IdProgrammazione;
                        tempPrg.TipoRecord = ImmagineCarica.TipoProgrammazione;
                        tempPrg.ProgramName = ImmagineCarica.NomeCiclo;
                        tempPrg.IdProfilo = ImmagineCarica.IdProfilo;


                        tempPrg.ListaParametri = ImmagineCarica.Parametri;
                        tempPrg.AnalizzaListaParametri();

                        return tempPrg;
                    }

                }

                return null;  // llProgrammaCarica
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                _lastError = Ex.Message;
                return null;
            }

        }
*/

    }
}
