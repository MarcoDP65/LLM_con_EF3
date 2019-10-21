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
using System.Resources;
using System.Drawing;

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
                    ControllaAttesa(UltimaScrittura);


                    // Eseguo solo se la connessione all'apparato è attiva
                    _mS.variabiliScheda = new MessaggioSpyBatt.VariabiliSpybatt();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.CMD_READ_VARIABLE;
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

                ControllaAttesa(UltimaScrittura);

                //
                //                _idCorrente = IdApparato;
                //                
                _esito = false;

                if (ApparatoConnesso)
                {
                    // Eseguo solo se la connessione all'apparato è attiva
                    _mS.ParametriGenerali = new MessaggioSpyBatt.ParametriSpybatt();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.CMD_READ_PARAM;
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
        public bool CaricaStatoOC(string IdApparato, bool ApparatoConnesso,bool ResetCount)
        {
            try
            {
                bool _esito = false;

                byte[] _dati = new byte[1];
                //
                //                _idCorrente = IdApparato;
                //                

                if (ApparatoConnesso)
                {
                    // Eseguo solo se la connessione all'apparato è attiva
                    //_mS.ParametriGenerali = new MessaggioSpyBatt.ParametriSpybatt();

                    ControllaAttesa(UltimaScrittura);

                    _mS.Comando = MessaggioSpyBatt.TipoComando.CMD_SIG60_READ_SETTING;
                    _mS.StatoTrxOC = new MessaggioSpyBatt.StatoSig60();
                    if (ResetCount)
                        _mS.ReserCounterOC = 0xF0;
                    else
                        _mS.ReserCounterOC = 0x0F;
                    _dati[0] = _mS.ReserCounterOC;
                    StatoSig60 = new sbSig60Parameters();
                    _mS.ComponiMessaggioNew(_dati);
                    _rxRisposta = false;
                    skipHead = true;
                    Log.Debug("SB Lettura Parametri OC");
                    Log.Debug(_mS.hexdumpMessaggio());
                    _parametri.scriviMessaggioSpyBatt(_mS.MessageBuffer, 0, _mS.MessageBuffer.Length);
                    _esito = aspettaRisposta(elementiComuni.TimeoutBase, 1, false);
                    if (_esito)
                    {
                        StatoSig60.OCBaudrate = _mS.StatoTrxOC.OCBaudrate;
                        StatoSig60.DatiEstesi = _mS.StatoTrxOC.DatiEstesi;
                        StatoSig60.ControlReg0 = _mS.StatoTrxOC.ControlReg0;
                        StatoSig60.ControlReg1 = _mS.StatoTrxOC.ControlReg1;
                        StatoSig60.ControlReg0_Err = _mS.StatoTrxOC.ControlReg0_Err;
                        StatoSig60.ControlReg1_Err = _mS.StatoTrxOC.ControlReg1_Err;

                        StatoSig60.NumLetture = _mS.StatoTrxOC.NumLetture;
                        StatoSig60.NumErrori = _mS.StatoTrxOC.NumErrori;
                        StatoSig60.NumInterferenze = _mS.StatoTrxOC.NumInterferenze;

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

        public bool CaricaProgrammazioneLL(string IdApparato, bool ApparatoConnesso)
        {
            try
            {
                bool _esito;

                ControllaAttesa(UltimaScrittura);

                //
                //                _idCorrente = IdApparato;
                //                
                _esito = false;

                if (ApparatoConnesso)
                {
                    // Eseguo solo se la connessione all'apparato è attiva
                    _mS.ParametriGenerali = new MessaggioSpyBatt.ParametriSpybatt();
                    _mS.Comando = MessaggioSpyBatt.TipoComando.CMD_READ_PARAM;
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

        public llProgrammaCarica CaricaProgrammaLL()
        {
            try
            {
                bool _esito = false;
                SerialMessage.EsitoRisposta _esitoMsg;
                llProgrammaCarica tempPrg = new llProgrammaCarica();
                MessaggioLadeLight.MessaggioProgrammazione ImmagineCarica = new MessaggioLadeLight.MessaggioProgrammazione();
                SerialMessage.EsitoRisposta EsitoMsg;

        
                uint StartAddr = (uint)(0x100);

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
                    else
                    {

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


        public bool SalvaProgrammazioneLL(string IdApparato, bool ApparatoConnesso, byte[] NuovoPrg, byte[] IdBatteria )
        {
            try
            {
                bool _esito;
                byte[] BufferPag1 = new byte[4096];
                byte[] BufferPacchetto;
                uint AddrCorrente = 0;
                int BlockSize = 230;
                int LastBlock = 0;



                ControllaAttesa(UltimaScrittura);

                // Step 1: carico l'intero blocco iniziale (4K)
                while ((0x1000 - AddrCorrente) > BlockSize)
                {
                    _esito = LeggiBloccoMemoria(AddrCorrente, (ushort)BlockSize, out BufferPacchetto);
                    if (_esito)
                    {
                        for (int _I = 0; _I < BlockSize; _I++)
                        {
                            BufferPag1[AddrCorrente++] = BufferPacchetto[_I];
                        }

                    }
                    else
                    {
                        Log.Debug("Caricamento dati fallito. Start = " + AddrCorrente.ToString("X6"));
                        return false;
                    }
                }

                // Ora il blocco finale
                LastBlock = (int)(0x1000 - AddrCorrente);
                _esito = LeggiBloccoMemoria(AddrCorrente, (ushort)LastBlock, out BufferPacchetto);
                if (_esito)
                {
                    for (int _I = 0; _I < LastBlock; _I++)
                    {
                        BufferPag1[AddrCorrente++] = BufferPacchetto[_I];
                    }

                }
                else
                {
                    Log.Debug("Caricamento dati fallito. Start = " + AddrCorrente.ToString("X6"));
                    return false;
                }

                Log.Info("Pagina 0: ");
                Log.Info(FunzioniComuni.HexdumpArray(BufferPag1));

                // Ora Inserisco il nuovo prg
                for (int _I = 0; _I < 256; _I++)
                {
                    if (_I < NuovoPrg.Length)
                    {
                        BufferPag1[0x0100 + _I] = NuovoPrg[_I];
                    }
                    else
                    {
                        BufferPag1[0x0100 + _I] = 0xFF;
                    }
                }

                //Inserisco l'ID Batteria LL
                // Ora Inserisco il nuovo prg
                for (int _I = 0; _I < 6; _I++)
                {
                    if (_I < IdBatteria.Length)
                    {
                        BufferPag1[0x01F0 + _I] = IdBatteria[_I];
                    }
                    else
                    {
                        BufferPag1[0x01F0 + _I] = 0xFF;
                    }
                }




                Log.Info("Nuova Pagina 0: ");
                Log.Info(FunzioniComuni.HexdumpArray(BufferPag1));


                //return false;


                // Cancello la pagina
                _esito = CancellaBlocco4K(0x0000);

                //  e ora riscrivo i dati
                BufferPacchetto = new byte[BlockSize];
                // Parto da 0x0100, l'area iniziale viene riscritta dal FW SB
                AddrCorrente = 0x0000;
                while ((0x1000 - AddrCorrente) > BlockSize)
                {
                    for (int _I = 0; _I < BlockSize; _I++)
                    {
                        BufferPacchetto[_I] = BufferPag1[AddrCorrente + _I];

                    }
                    ControllaAttesa(UltimaScrittura);
                    _esito = ScriviBloccoMemoria(AddrCorrente, (ushort)BlockSize, BufferPacchetto);

                    if (_esito)
                    {
                        Log.Debug("Scrittura. Start = " + AddrCorrente.ToString("X6") + " - Bytes " + BlockSize.ToString("X4"));
                    }
                    else
                    {
                        Log.Debug("Scrittura dati fallita. Start = " + AddrCorrente.ToString("X6"));
                        return false;
                    }
                    AddrCorrente += (uint)BlockSize;


                }

                // Ora il blocco finale
                BlockSize = (int)(0x1000 - AddrCorrente);

                for (int _I = 0; _I < BlockSize; _I++)
                {
                    BufferPacchetto[_I] = BufferPag1[AddrCorrente + _I];
                }
                _esito = ScriviBloccoMemoria(AddrCorrente, (ushort)BlockSize, BufferPacchetto);
                AddrCorrente += (uint)BlockSize;
                if (!_esito)
                {
                    Log.Debug("Scrittura dati fallita. Start = " + AddrCorrente.ToString("X6"));
                    return false;
                }

                Log.Debug("Scrittura dati Completata" );

                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("SalvaProgrammazioneLL: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }

        public bool CancellaProgrammazioneLL(string IdApparato, bool ApparatoConnesso )
        {
            try
            {
                bool _esito;
                byte[] BufferPag1 = new byte[4096];
                byte[] BufferPacchetto;
                uint AddrCorrente = 0;
                int BlockSize = 230;
                int LastBlock = 0;



                ControllaAttesa(UltimaScrittura);

                // Step 1: carico l'intero blocco iniziale (4K)
                while ((0x1000 - AddrCorrente) > BlockSize)
                {
                    _esito = LeggiBloccoMemoria(AddrCorrente, (ushort)BlockSize, out BufferPacchetto);
                    if (_esito)
                    {
                        for (int _I = 0; _I < BlockSize; _I++)
                        {
                            BufferPag1[AddrCorrente++] = BufferPacchetto[_I];
                        }

                    }
                    else
                    {
                        Log.Debug("Caricamento dati fallito. Start = " + AddrCorrente.ToString("X6"));
                        return false;
                    }
                }

                // Ora il blocco finale
                LastBlock = (int)(0x1000 - AddrCorrente);
                _esito = LeggiBloccoMemoria(AddrCorrente, (ushort)LastBlock, out BufferPacchetto);
                if (_esito)
                {
                    for (int _I = 0; _I < LastBlock; _I++)
                    {
                        BufferPag1[AddrCorrente++] = BufferPacchetto[_I];
                    }

                }
                else
                {
                    Log.Debug("Caricamento dati fallito. Start = " + AddrCorrente.ToString("X6"));
                    return false;
                }

                Log.Info("Pagina 0: ");
                Log.Info(FunzioniComuni.HexdumpArray(BufferPag1));

                // Ora azzero l'intera zona prg LL
                for (int _I = 0; _I < 256; _I++)
                {
                          BufferPag1[0x0100 + _I] = 0xFF;
                }



                Log.Info("Nuova Pagina 0: ");
                Log.Info(FunzioniComuni.HexdumpArray(BufferPag1));


                //return false;


                // Cancello la pagina
                _esito = CancellaBlocco4K(0x0000);

                //  e ora riscrivo i dati
                BufferPacchetto = new byte[BlockSize];
                // Parto da 0x0100, l'area iniziale viene riscritta dal FW SB
                AddrCorrente = 0x0000;
                while ((0x1000 - AddrCorrente) > BlockSize)
                {
                    for (int _I = 0; _I < BlockSize; _I++)
                    {
                        BufferPacchetto[_I] = BufferPag1[AddrCorrente + _I];

                    }
                    ControllaAttesa(UltimaScrittura);
                    _esito = ScriviBloccoMemoria(AddrCorrente, (ushort)BlockSize, BufferPacchetto);

                    if (_esito)
                    {
                        Log.Debug("Scrittura. Start = " + AddrCorrente.ToString("X6") + " - Bytes " + BlockSize.ToString("X4"));
                    }
                    else
                    {
                        Log.Debug("Scrittura dati fallita. Start = " + AddrCorrente.ToString("X6"));
                        return false;
                    }
                    AddrCorrente += (uint)BlockSize;


                }

                // Ora il blocco finale
                BlockSize = (int)(0x1000 - AddrCorrente);

                for (int _I = 0; _I < BlockSize; _I++)
                {
                    BufferPacchetto[_I] = BufferPag1[AddrCorrente + _I];
                }
                _esito = ScriviBloccoMemoria(AddrCorrente, (ushort)BlockSize, BufferPacchetto);
                AddrCorrente += (uint)BlockSize;
                if (!_esito)
                {
                    Log.Debug("Scrittura dati fallita. Start = " + AddrCorrente.ToString("X6"));
                    return false;
                }

                Log.Debug("Scrittura dati Completata");

                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CancellaProgrammazioneLL: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }
        }



    }
}
