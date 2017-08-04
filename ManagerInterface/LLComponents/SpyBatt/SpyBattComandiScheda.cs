using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
// using System.Windows.Forms;

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


        /// <summary>
        /// Legge direttamente dallo SPY-BATT collegato i valori attuali di corrente, tensione e temperatura
        /// </summary>
        /// <param name="IdApparato">ID dell'apparato collegato</param>
        /// <param name="ApparatoConnesso">Se true tento la lettura diretta</param>
        /// <returns></returns>
        public bool CaricaVariabili(string IdApparato, bool ApparatoConnesso)
        {
            try
            {
                bool _esito;
                //
                //                _idCorrente = IdApparato;
                //                
                _esito = false;

                if (ApparatoConnesso)
                {
                    // Eseguo solo se la connessione all'apparato è attiva
                    _mS.variabiliScheda = new MessaggioSpyBatt.VariabiliSpybatt();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.SB_R_Variabili;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    skipHead = true;
                    Log.Debug("SB Carica Variabili");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                    if (_esito)
                    {
                        sbVariabili = new MoriData.sbVariabili();
                        sbVariabili.IdApparato = _idCorrente;
                        sbVariabili.TensioneTampone = (ushort)(_mS.variabiliScheda.TensioneBattT);
                        sbVariabili.TensioneIstantanea = _mS.variabiliScheda.TensioneIstantanea;
                        sbVariabili.Tensione1 = _mS.variabiliScheda.Tensione1;
                        sbVariabili.Tensione2 = _mS.variabiliScheda.Tensione2;
                        sbVariabili.Tensione3 = _mS.variabiliScheda.Tensione3;
                        sbVariabili.CorrenteBatteria = _mS.variabiliScheda.CorrenteBatteria;
                        sbVariabili.AhCaricati = _mS.variabiliScheda.AhCaricati;
                        sbVariabili.AhScaricati = _mS.variabiliScheda.AhScaricati;
                        sbVariabili.TempNTC = _mS.variabiliScheda.TempNTC;
                        sbVariabili.PresenzaElettrolita = _mS.variabiliScheda.PresenzaElettrolita;
                        sbVariabili.SoC = _mS.variabiliScheda.SoC;
                        sbVariabili.RF = _mS.variabiliScheda.RF;
                        sbVariabili.WhScaricati = _mS.variabiliScheda.WhScaricati;
                        sbVariabili.WhCaricati = _mS.variabiliScheda.WhCaricati;
                        sbVariabili.MemProgrammed = _mS.variabiliScheda.MemProgrammed;
                        sbVariabili.IstanteLettura = _mS.variabiliScheda.IstanteLettura;
                        sbVariabili.ConnectionStatus = _mS.variabiliScheda.ConnStatus;

                    }
                }
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaTestata: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }

        /// <summary>
        /// Legge direttamente dallo SPY-BATT collegato i valori dei parametri generali
        /// </summary>
        /// <param name="IdApparato">ID dell'apparato collegato</param>
        /// <param name="ApparatoConnesso">Se true tento la lettura diretta</param>
        /// <returns></returns>
        public bool CaricaParametri(string IdApparato, bool ApparatoConnesso)
        {
            try
            {
                bool _esito;
                //
                //                _idCorrente = IdApparato;
                //                
                _esito = false;

                if (ApparatoConnesso)
                {
                    // Eseguo solo se la connessione all'apparato è attiva
                    _mS.ParametriGenerali = new MessaggioSpyBatt.ParametriSpybatt();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.SB_R_ParametriLettura;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    skipHead = true;
                    Log.Debug("SB Lettura Parametri");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                    if (_esito)
                    {
                        ParametriGenerali = new MoriData.sbParametriGenerali();
                        ParametriGenerali.IdApparato = _idCorrente;
                        ParametriGenerali.LettureCorrente = _mS.ParametriGenerali.LettureCorrente;
                        ParametriGenerali.LettureTensione = _mS.ParametriGenerali.LettureTensione;
                        ParametriGenerali.DurataPausa = _mS.ParametriGenerali.DurataPausa;
                        ParametriGenerali.DataInserimento = _mS.ParametriGenerali.IstanteLettura;
                        ParametriGenerali.CausaUltimoReset = _mS.ParametriGenerali.UltimoReset;

                    }
                }
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCalibrazioni: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }


        /// <summary>
        /// Legge direttamente dallo SPY-BATT collegato i valori dei parametri generali
        /// </summary>
        /// <param name="IdApparato">ID dell'apparato collegato</param>
        /// <param name="ApparatoConnesso">Se true tento la lettura diretta</param>
        /// <returns></returns>
        public bool CaricaStatoOC(string IdApparato, bool ApparatoConnesso)
        {
            try
            {
                bool _esito = false;
                //
                //                _idCorrente = IdApparato;
                //                

                if (ApparatoConnesso)
                {
                    // Eseguo solo se la connessione all'apparato è attiva
                    //_mS.ParametriGenerali = new MessaggioSpyBatt.ParametriSpybatt();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.SB_R_ParametriSIG60;
                    _mS.ComponiMessaggio();
                    _rxRisposta = false;
                    skipHead = true;
                    Log.Debug("SB Lettura Parametri OC");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                    if (_esito)
                    {

                        BrOCcorrente = _mS.BrOCcorrente;
                    }
                }
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCalibrazioni: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }



    }
}
