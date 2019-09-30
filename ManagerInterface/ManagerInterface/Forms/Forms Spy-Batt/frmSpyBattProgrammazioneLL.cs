using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using MoriData;
using ChargerLogic;
using NextUI.Component;
using NextUI.Frame;

//using Common.Logging;
using BrightIdeasSoftware;
using Utility;
using MdiHelper;
using log4net;
using log4net.Config;



namespace PannelloCharger
{
    public partial class frmSpyBat : Form
    {

        public bool AggiornaProfiloLL(byte[] Profilo)
        {
            try
            {
                byte[] IdBatteria = new byte[6];
                string _tempId;
                if (_sb.sbCliente.BatteryLLId == "")
                {
                    _tempId = "N.D.  ";
                }
                else
                {
                    _tempId = _sb.sbCliente.BatteryLLId + "      ";
                }

                IdBatteria = Encoding.ASCII.GetBytes(_tempId.Substring(0, 6));

                return _sb.SalvaProgrammazioneLL(_sb.Id, true, Profilo, IdBatteria);

            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message, Ex);
                return false;
            }
        }

        public bool InizializzaSchedaLL()
        {
            try
            {
                
                cmbPaTipoLadeLight.DataSource = _sb.DatiBase.ModelliLL;
                cmbPaTipoLadeLight.ValueMember = "IdModelloLL";
                cmbPaTipoLadeLight.DisplayMember = "NomeModello";

                cmbPaTipoBatteria.DataSource = _parametri.ParametriProfilo.ModelliBatteria;  //   TipiBattria;
                cmbPaTipoBatteria.ValueMember = "BatteryTypeId";
                cmbPaTipoBatteria.DisplayMember = "BatteryType";

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("InizializzaSchedaLL: " + ex.Message, ex);
                return false;
            }
        }

        public bool AggiornaSelezioneProfili( )
        {
            try
            {
                // verifico che i parametri base siano definiti
                if (_sb.BattAttiva != null && _sb.CbAttivo != null & _sb.ProgrammaCorrente != null)
                {

                    btnPaProfileRefresh.ForeColor = Color.Red;

                    // Carico i profili in base al tipo
                    ushort TipoBatt = (ushort)_sb.BattAttiva.BatteryTypeId;

                    var Listatemp = from p in _parametri.ParametriProfilo.ProfiliCarica   //_cb.ProfiliCarica
                                    join pt in _parametri.ParametriProfilo.ParametriCarica on p.IdProfiloCaricaLL equals pt.IdProfiloCaricaLL
                                    where pt.BatteryTypeId == TipoBatt
                                    orderby p.Ordine
                                    select p;

                    ProfiliCarica = new List<_mbProfiloCarica>();
                    _mbProfiloCarica profiloDefault = null;
                    foreach (var pc in Listatemp)
                    {
                        ProfiliCarica.Add((_mbProfiloCarica)pc);
                        if (_sb.BattAttiva.StandardChargeProfile == pc.IdProfiloCaricaLL)
                        {
                            profiloDefault = pc;
                        }
                    }


                    cmbPaProfilo.DataSource = ProfiliCarica;
                    cmbPaProfilo.ValueMember = "IdProfiloCaricaLL";
                    cmbPaProfilo.DisplayMember = "NomeProfilo";
                    cmbPaProfilo.SelectedItem = profiloDefault;


                    // Carico le tensioni in base al tipo e verifico che la tensione impostata sia valida
                    byte TipoApp = _sb.CbAttivo.IdModelloLL;
                    var Listatens = from t in _sb.DatiBase.TensioniBatteria
                                    join tm in _sb.DatiBase.TensioniModello on t.IdTensione equals tm.IdTensione
                                    where ( tm.IdModelloLL == TipoApp )
                                    && ( t.IdTensione == _sb.ProgrammaCorrente.BatteryVdef )
                                    select t;

                    if (Listatens.Count() != 1)
                    {
                        // NON VALIDO


                    }
                    else
                    {
                        txtPaTensione.Text = FunzioniMR.StringaTensione(_sb.ProgrammaCorrente.BatteryVdef);
                        txtPaTensione.ReadOnly = true;
                        txtPaNumCelle.Text = _sb.ProgrammaCorrente.BatteryCells.ToString();
                        txtPaNumCelle.ReadOnly = true;
                        txtPaCapacita.Text = FunzioniMR.StringaCapacitaUint(_sb.ProgrammaCorrente.BatteryAhdef,10,0);
                        txtPaCapacita.ReadOnly = true;
                    }



                }
                else
                {
                    ProfiliCarica.Clear();
                    cmbPaProfilo.DataSource = ProfiliCarica;
                    cmbPaProfilo.ValueMember = "IdProfiloCaricaLL";
                    cmbPaProfilo.DisplayMember = "NomeProfilo";
                }




                return true;
            }
            catch (Exception ex)
            {
                Log.Error("AggiornaSelezioneProfili: " + ex.Message, ex);
                return false;
            }
        }


        private bool RicalcolaParametriCiclo(bool MessaggioEsito = true)
        {
            try
            {
                mbTipoBatteria _Batteria = (mbTipoBatteria)cmbPaTipoBatteria.SelectedItem;
                _mbProfiloCarica _Profilo = (_mbProfiloCarica)cmbPaProfilo.SelectedItem;
                ushort _Tensione = FunzioniMR.ConvertiUshort(txtPaTensione.Text, 100, 0);
                ushort _Capacita = FunzioniMR.ConvertiUshort(txtPaCapacita.Text, 10, 0);
                ushort _Celle = FunzioniMR.ConvertiUshort(txtPaNumCelle.Text, 1, 0);
                _llModelloCb _ModelloCB = _sb.CbAttivo;

                string messaggioErrore;
                //    null;


                CaricaBatteria.EsitoRicalcolo esitoRicalcolo = _sb.ModCicloCorrente.CalcolaParametri(_Batteria._mbTb, _Profilo, _Tensione, _Capacita, _Celle, _ModelloCB);

                btnPaProfileRefresh.ForeColor = Color.Black;
 

                if (esitoRicalcolo == CaricaBatteria.EsitoRicalcolo.OK)
                {
                    btnPaSalvaDati.Enabled = true;
                }

                else
                {
                    btnPaSalvaDati.Enabled = false;
                    messaggioErrore = "";
                    switch (esitoRicalcolo)
                    {
                        case CaricaBatteria.EsitoRicalcolo.OK:
                            break;
                        case CaricaBatteria.EsitoRicalcolo.ErrIMax:
                        case CaricaBatteria.EsitoRicalcolo.ErrIMin:
                        case CaricaBatteria.EsitoRicalcolo.ErrVMax:
                        case CaricaBatteria.EsitoRicalcolo.ErrVMin:
                            {
                                messaggioErrore = "Il ciclo richiesto non è effettuabile con questo modello di caricabatteria";
                            }
                            break;
                        case CaricaBatteria.EsitoRicalcolo.ErrGenerico:
                        case CaricaBatteria.EsitoRicalcolo.ParNonValidi:
                        case CaricaBatteria.EsitoRicalcolo.ErrUndef:
                            {
                                messaggioErrore = "Il mancano informazioni per poter definire il ciclo di carica richiesto";
                            }
                            break;
                        default:
                            break;
                    }

                    if (MessaggioEsito)
                    {
                        MessageBox.Show(messaggioErrore, "Ricalcolo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }




                return (esitoRicalcolo == CaricaBatteria.EsitoRicalcolo.OK);



            }
            catch (Exception Ex)
            {
                Log.Error("RicalcolaParametriCiclo: " + Ex.Message);
                return false;
            }
        }

        private bool MostraParametriCiclo(bool ParametriBase = false, bool SoloClear = false, bool SbloccaValori = false, bool SkipCapacità = false)
        {
            try
            {
                ///TODO:  Ripristino le eventuali schede nascoste 


                if (SoloClear)
                {
                    if (ParametriBase)
                    {
                        txtPaNomeSetup.Text = "";
                        txtPaIdSetup.Text = "";
                    }

                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaSogliaV0, 0, 0, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaPrefaseI0, 0, 0, 2, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaDurataMaxT0, 0, 0, 3, SbloccaValori);

                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaSogliaVs, 0, 0, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCorrenteI1, 0, 0, 2, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref cmbPaDurataMaxT1, 0, 0, 3, SbloccaValori);

                }
                else
                {
                    if (ParametriBase)
                    {
                        txtPaNomeSetup.Text = _sb.ModCicloCorrente.NomeProfilo;
                        txtPaIdSetup.Text = _sb.ModCicloCorrente.IdProgramma.ToString();
                        txtPaCassone.Text = _sb.ModCicloCorrente.ValoriCiclo.TipoCassone.ToString();
                        // Allineo il modello LL

                        _llModelloCb TempLL = (from tll in (List<_llModelloCb>)cmbPaTipoLadeLight.DataSource
                                               where tll.IdModelloLL == _sb.ModCicloCorrente.ValoriCiclo.IdModelloLL
                                               select tll).FirstOrDefault();
                        cmbPaTipoLadeLight.SelectedItem = TempLL;


                        // Allineo il tipo batteria
                        mbTipoBatteria TempBatt = (from tb in (List<mbTipoBatteria>)cmbPaTipoBatteria.DataSource
                                                   where tb.BatteryTypeId == _sb.ModCicloCorrente.Batteria.BatteryTypeId
                                                   select tb).FirstOrDefault();
                        cmbPaTipoBatteria.SelectedItem = TempBatt;




                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaTensione, _sb.ModCicloCorrente.Tensione, 1, 1, SbloccaValori);
                        txtPaTensione.ReadOnly = true;

                        txtPaNumCelle.Text = _sb.ModCicloCorrente.NumeroCelle.ToString();
                        txtPaNumCelle.ReadOnly = true;



                        if (_sb.ModCicloCorrente.Profilo != null)
                        {
                            _mbProfiloCarica TempProf = (from tp in (List<_mbProfiloCarica>)cmbPaProfilo.DataSource
                                                         where tp.IdProfiloCaricaLL == _sb.ModCicloCorrente.Profilo.IdProfiloCaricaLL
                                                         select tp).FirstOrDefault();
                            cmbPaProfilo.SelectedItem = TempProf;

                        }


                        chkPaUsaSpyBatt.Checked = (_sb.ModCicloCorrente.ValoriCiclo.AbilitaSpyBatt == 0x000F);
                        chkPaUsaSafety.Checked = (_sb.ModCicloCorrente.ValoriCiclo.AbilitaSafety == 0x000F);


                        //cmbPaProfilo

                    }

                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaNumCelle, _sb.ModCicloCorrente.NumeroCelle, 4, 3, SbloccaValori);
                    if (!SkipCapacità)
                    {
                        // se sono entrato da txtCapacita_change evito di riformattare
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaCapacita, _sb.ModCicloCorrente.Capacita, 5, 2, SbloccaValori);
                    }



                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaSogliaV0, _sb.ModCicloCorrente.ValoriCiclo.TensionePrecicloV0, _sb.ModCicloCorrente.ParametriAttivi.TensionePrecicloV0, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaPrefaseI0, _sb.ModCicloCorrente.ValoriCiclo.CorrenteI0, _sb.ModCicloCorrente.ParametriAttivi.CorrenteI0, 2, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaDurataMaxT0, _sb.ModCicloCorrente.ValoriCiclo.TempoT0Max, _sb.ModCicloCorrente.ParametriAttivi.TempoT0Max, 3, SbloccaValori);

                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaSogliaVs, _sb.ModCicloCorrente.ValoriCiclo.TensioneSogliaVs, _sb.ModCicloCorrente.ParametriAttivi.TensioneSogliaVs, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCorrenteI1, _sb.ModCicloCorrente.ValoriCiclo.CorrenteI1, _sb.ModCicloCorrente.ParametriAttivi.CorrenteI1, 2, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref cmbPaDurataMaxT1, _sb.ModCicloCorrente.ValoriCiclo.TempoT1Max, _sb.ModCicloCorrente.ParametriAttivi.TempoT1Max, 3, SbloccaValori);

                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaRaccordoF1, _sb.ModCicloCorrente.ValoriCiclo.TensioneRaccordoVr, _sb.ModCicloCorrente.ParametriAttivi.TensioneRaccordoVr, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCorrenteRaccordo, _sb.ModCicloCorrente.ValoriCiclo.CorrenteRaccordoIr, _sb.ModCicloCorrente.ParametriAttivi.CorrenteRaccordoIr, 2, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCorrenteF3, _sb.ModCicloCorrente.ValoriCiclo.CorrenteFinaleI2, _sb.ModCicloCorrente.ParametriAttivi.CorrenteFinaleI2, 2, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaVMax, _sb.ModCicloCorrente.ValoriCiclo.TensioneMassimaVMax, _sb.ModCicloCorrente.ParametriAttivi.TensioneMassimaVMax, 1, SbloccaValori);

                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaTempoT2Min, _sb.ModCicloCorrente.ValoriCiclo.TempoT2Min, _sb.ModCicloCorrente.ParametriAttivi.TempoT2Min, 3, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaTempoT2Max, _sb.ModCicloCorrente.ValoriCiclo.TempoT2Max, _sb.ModCicloCorrente.ParametriAttivi.TempoT2Max, 3, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCoeffK, _sb.ModCicloCorrente.ValoriCiclo.FattoreK, _sb.ModCicloCorrente.ParametriAttivi.FattoreK, 3, SbloccaValori);

                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaTempoT3Max, _sb.ModCicloCorrente.ValoriCiclo.TempoT3Max, _sb.ModCicloCorrente.ParametriAttivi.TempoT3Max, 3, SbloccaValori);

                    FunzioniUI.ImpostaCheckBoxUshort(ref chkPaAttivaEqual, ref lblPaAttivaEqual, _sb.ModCicloCorrente.ValoriCiclo.EqualAttivabile, _sb.ModCicloCorrente.ParametriAttivi.EqualAttivabile, 3, SbloccaValori);
                    txtPaEqualAttesa.Text = "";
                    MostraEqualCCorrente();

                    if (true) // (chkPaAttivaEqual.Checked)
                    {
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaEqualAttesa, _sb.ModCicloCorrente.ValoriCiclo.EqualTempoAttesa, _sb.ModCicloCorrente.ParametriAttivi.EqualTempoAttesa, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaEqualNumPulse, _sb.ModCicloCorrente.ValoriCiclo.EqualNumImpulsi, _sb.ModCicloCorrente.ParametriAttivi.EqualNumImpulsi, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaEqualPulsePause, _sb.ModCicloCorrente.ValoriCiclo.EqualTempoPausa, _sb.ModCicloCorrente.ParametriAttivi.EqualTempoPausa, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaEqualPulseTime, _sb.ModCicloCorrente.ValoriCiclo.EqualTempoImpulso, _sb.ModCicloCorrente.ParametriAttivi.EqualTempoImpulso, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaEqualPulseCurrent, _sb.ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso, _sb.ModCicloCorrente.ParametriAttivi.EqualCorrenteImpulso, 2, SbloccaValori);
                    }

                    FunzioniUI.ImpostaCheckBoxUshort(ref chkPaAttivaMant, ref lblPaAttivaMant, _sb.ModCicloCorrente.ValoriCiclo.MantAttivabile, _sb.ModCicloCorrente.ParametriAttivi.MantAttivabile, 3, SbloccaValori);


                    if (true) // (chkPaAttivaMant.Checked)
                    {
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantAttesa, _sb.ModCicloCorrente.ValoriCiclo.MantTempoAttesa, _sb.ModCicloCorrente.ParametriAttivi.MantTempoAttesa, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmin, _sb.ModCicloCorrente.ValoriCiclo.MantTensIniziale, _sb.ModCicloCorrente.ParametriAttivi.MantTensIniziale, 1, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmax, _sb.ModCicloCorrente.ValoriCiclo.MantTensFinale, _sb.ModCicloCorrente.ParametriAttivi.MantTensFinale, 1, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantDurataMax, _sb.ModCicloCorrente.ValoriCiclo.MantTempoMaxErogazione, _sb.ModCicloCorrente.ParametriAttivi.MantTempoMaxErogazione, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantCorrente, _sb.ModCicloCorrente.ValoriCiclo.MantCorrenteImpulso, _sb.ModCicloCorrente.ParametriAttivi.MantCorrenteImpulso, 2, SbloccaValori);
                    }



                    FunzioniUI.ImpostaCheckBoxUshort(ref chkPaAttivaOppChg, ref lblPaAttivaOppChg, _sb.ModCicloCorrente.ValoriCiclo.OpportunityAttivabile, _sb.ModCicloCorrente.ParametriAttivi.OpportunityAttivabile, 3, SbloccaValori);

                    if (true) // (chkPaAttivaOppChg.Checked)
                    {
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraInizio, _sb.ModCicloCorrente.ValoriCiclo.OpportunityOraInizio, _sb.ModCicloCorrente.ParametriAttivi.OpportunityOraInizio, 4, false);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraFine, _sb.ModCicloCorrente.ValoriCiclo.OpportunityOraFine, _sb.ModCicloCorrente.ParametriAttivi.OpportunityOraFine, 4, false);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppVSoglia, _sb.ModCicloCorrente.ValoriCiclo.OpportunityTensioneMax, _sb.ModCicloCorrente.ParametriAttivi.OpportunityTensioneMax, 1, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppCorrente, _sb.ModCicloCorrente.ValoriCiclo.OpportunityCorrente, _sb.ModCicloCorrente.ParametriAttivi.OpportunityCorrente, 2, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppDurataMax, _sb.ModCicloCorrente.ValoriCiclo.OpportunityDurataMax, _sb.ModCicloCorrente.ParametriAttivi.OpportunityDurataMax, 3, SbloccaValori);

                        chkPaOppNotturno.Checked = (_sb.ModCicloCorrente.ValoriCiclo.OpportunityOraInizio > _sb.ModCicloCorrente.ValoriCiclo.OpportunityOraFine);
                        OppNotturno(true);

                    }




                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaVMinRic, _sb.ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMin, _sb.ModCicloCorrente.ParametriAttivi.TensRiconoscimentoMin, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaVMaxRic, _sb.ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMax, _sb.ModCicloCorrente.ParametriAttivi.TensRiconoscimentoMax, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaVMinStop, _sb.ModCicloCorrente.ValoriCiclo.TensMinStop, _sb.ModCicloCorrente.ParametriAttivi.TensMinStop, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaVLimite, _sb.ModCicloCorrente.ValoriCiclo.TensioneLimiteVLim, _sb.ModCicloCorrente.ParametriAttivi.TensioneLimiteVLim, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCorrenteMassima, _sb.ModCicloCorrente.ValoriCiclo.CorrenteMassima, _sb.ModCicloCorrente.ParametriAttivi.CorrenteMassima, 2, SbloccaValori);

                }



                return false;
            }
            catch (Exception Ex)
            {
                Log.Error("MostraParametriCiclo: " + Ex.Message);
                return false;
            }
        }

        public void OppNotturno(bool Notturno)
        {
            try
            {
                if (_sb.ModCicloCorrente.ValoriCiclo.OpportunityOraInizio > _sb.ModCicloCorrente.ValoriCiclo.OpportunityOraFine)
                {
                    rslPaOppFinestra.ChannelColor = OppChargeSpento;
                    rslPaOppFinestra.RangeColor = OppChargeAttivo;

                }
                else
                {
                    rslPaOppFinestra.ChannelColor = OppChargeAttivo;
                    rslPaOppFinestra.RangeColor = OppChargeSpento;
                }
            }
            catch
            {

            }
        }


        public bool MostraEqualCCorrente()
        {
            try
            {

                txtPaEqualNumPulse.Text = "";
                txtPaEqualAttesa.Text = "";
                txtPaEqualPulsePause.Text = "";
                txtPaEqualPulseTime.Text = "";
                txtPaEqualPulseCurrent.Text = "";

                if (_sb.ProfiloAttivo != null)
                {
                    if (_sb.ProfiloAttivo.EqualNumImpulsi > 0 || _sb.ProfiloAttivo.EqualTempoAttesa > 0)
                    {
                        chkPaAttivaEqual.Checked = true;
                        txtPaEqualNumPulse.Text = _sb.ProfiloAttivo.EqualNumImpulsi.ToString();
                        txtPaEqualAttesa.Text = _sb.ProfiloAttivo.EqualTempoAttesa.ToString();
                        txtPaEqualPulsePause.Text = _sb.ProfiloAttivo.EqualDurataPausa.ToString();
                        txtPaEqualPulseTime.Text = _sb.ProfiloAttivo.EqualDurataImpulso.ToString();
                        txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)_sb.ProfiloAttivo.EqualCorrenteImpulso);

                    }
                    else
                    {
                        chkPaAttivaEqual.Checked = false;
                        txtPaEqualNumPulse.Text = "";
                        txtPaEqualAttesa.Text = "";
                        txtPaEqualPulsePause.Text = "";
                        txtPaEqualPulseTime.Text = "";
                        txtPaEqualPulseCurrent.Text = "";

                        txtPaEqualNumPulse.Enabled = false;
                        txtPaEqualAttesa.Enabled = false;
                        txtPaEqualPulsePause.Enabled = false;
                        txtPaEqualPulseTime.Enabled = false;
                        txtPaEqualPulseCurrent.Enabled = false;

                    }

                }

                return true;
            }
            catch
            {
                return false;
            }

        }


        public bool MostraMantCCorrente()
        {
            try
            {

                txtPaEqualNumPulse.Text = "";
                txtPaEqualAttesa.Text = "";
                txtPaEqualPulsePause.Text = "";
                txtPaEqualPulseTime.Text = "";
                txtPaEqualPulseCurrent.Text = "";

                if (_sb.ProfiloAttivo != null)
                {
                    if (_sb.ProfiloAttivo.EqualNumImpulsi > 0 || _sb.ProfiloAttivo.EqualTempoAttesa > 0)
                    {
                        chkPaAttivaEqual.Checked = true;
                        txtPaEqualNumPulse.Text = _sb.ProfiloAttivo.EqualNumImpulsi.ToString();
                        txtPaEqualAttesa.Text = _sb.ProfiloAttivo.EqualTempoAttesa.ToString();
                        txtPaEqualPulsePause.Text = _sb.ProfiloAttivo.EqualDurataPausa.ToString();
                        txtPaEqualPulseTime.Text = _sb.ProfiloAttivo.EqualDurataImpulso.ToString();
                        txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)_sb.ProfiloAttivo.EqualCorrenteImpulso);

                    }
                    else
                    {
                        chkPaAttivaEqual.Checked = false;
                        txtPaMantAttesa.Text = "";
                        txtPaMantVmin.Text = "";
                        txtPaMantVmax.Text = "";
                        txtPaMantDurataMax.Text = "";
                        txtPaMantCorrente.Text = "";

                        txtPaMantAttesa.Enabled = false;
                        txtPaMantVmin.Enabled = false;
                        txtPaMantVmax.Enabled = false;
                        txtPaMantDurataMax.Enabled = false;
                        txtPaMantCorrente.Enabled = false;

                    }

                }

                return true;
            }
            catch
            {
                return false;
            }

        }


        public bool AssegnaEqualCCorrente()
        {
            try
            {

                // if (ModCicloCorrente.ValoriCiclo.EqualAttivo == 0xF0F0)

                txtPaEqualNumPulse.Text = _sb.ModCicloCorrente.ValoriCiclo.EqualNumImpulsi.ToString();
                _sb.ProfiloAttivo.EqualNumImpulsi = _sb.ModCicloCorrente.ValoriCiclo.EqualNumImpulsi;

                txtPaEqualAttesa.Text = _sb.ModCicloCorrente.ValoriCiclo.EqualTempoAttesa.ToString();
                _sb.ProfiloAttivo.EqualTempoAttesa = _sb.ModCicloCorrente.ValoriCiclo.EqualTempoAttesa;

                txtPaEqualPulsePause.Text = _sb.ModCicloCorrente.ValoriCiclo.EqualTempoPausa.ToString();
                _sb.ProfiloAttivo.EqualDurataPausa = _sb.ModCicloCorrente.ValoriCiclo.EqualTempoPausa;

                txtPaEqualPulseTime.Text = _sb.ModCicloCorrente.ValoriCiclo.EqualTempoImpulso.ToString();
                _sb.ProfiloAttivo.EqualDurataImpulso = _sb.ModCicloCorrente.ValoriCiclo.EqualTempoImpulso;

                txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)_sb.ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso);
                _sb.ProfiloAttivo.EqualCorrenteImpulso = _sb.ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso;


                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool AssegnaMantCCorrente()
        {
            try
            {


                bool SbloccaValori = chkPaSbloccaValori.Checked;

                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantAttesa, _sb.ModCicloCorrente.ValoriCiclo.MantTempoAttesa, _sb.ModCicloCorrente.ParametriAttivi.MantTempoAttesa, 3, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmin, _sb.ModCicloCorrente.ValoriCiclo.MantTensIniziale, _sb.ModCicloCorrente.ParametriAttivi.MantTensIniziale, 1, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmax, _sb.ModCicloCorrente.ValoriCiclo.MantTensFinale, _sb.ModCicloCorrente.ParametriAttivi.MantTensFinale, 1, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantDurataMax, _sb.ModCicloCorrente.ValoriCiclo.MantTempoMaxErogazione, _sb.ModCicloCorrente.ParametriAttivi.MantTempoMaxErogazione, 3, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantCorrente, _sb.ModCicloCorrente.ValoriCiclo.MantCorrenteImpulso, _sb.ModCicloCorrente.ParametriAttivi.MantCorrenteImpulso, 2, SbloccaValori);


                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool AssegnaOppCCorrente()
        {
            try
            {


                bool SbloccaValori = chkPaSbloccaValori.Checked;

                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraInizio, _sb.ModCicloCorrente.ValoriCiclo.OpportunityOraInizio, _sb.ModCicloCorrente.ParametriAttivi.OpportunityOraInizio, 4, false);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraFine, _sb.ModCicloCorrente.ValoriCiclo.OpportunityOraFine, _sb.ModCicloCorrente.ParametriAttivi.OpportunityOraFine, 4, false);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppVSoglia, _sb.ModCicloCorrente.ValoriCiclo.OpportunityTensioneMax, _sb.ModCicloCorrente.ParametriAttivi.OpportunityTensioneMax, 1, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppCorrente, _sb.ModCicloCorrente.ValoriCiclo.OpportunityCorrente, _sb.ModCicloCorrente.ParametriAttivi.OpportunityCorrente, 2, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppDurataMax, _sb.ModCicloCorrente.ValoriCiclo.OpportunityDurataMax, _sb.ModCicloCorrente.ParametriAttivi.OpportunityDurataMax, 3, SbloccaValori);

                OppNotturno((_sb.ModCicloCorrente.ValoriCiclo.OpportunityOraInizio >= _sb.ModCicloCorrente.ValoriCiclo.OpportunityOraFine));

                return true;
            }
            catch
            {
                return false;
            }
        }




        public bool ScriviParametriCarica()
        {
            try
            {

                bool esito;
                byte[] _datiTemp = new byte[226];

                // Se so già che devo salvare, non faccio nemmeno il controllo dati:
                if (_sb.ModCicloCorrente.DatiSalvati)
                {
                    // Se almeno 1 parametro è diverso dal ciclo attivo corrente lo salvo come nuovo ciclo
                    esito = VerificaValoriParametriCarica();
                    if (esito)
                    {
                        //coincide col programma esistente. esco
                        return true;
                    }
                }
                // Carico i valori impostati nelle textbox
                esito = LeggiValoriParametriCarica();


                // L'id profilo passato al ladelight è l'id della configurazione attiva (non ho uno storico delle programmazioni ll.
                _sb.ModCicloCorrente.IdProgramma = _sb.ProgrammaCorrente.IdProgramma;
                bool esitoSalvataggio = false;
                if (esito)
                {
                    // Riscrivo i valori nelle textBox per conferma poi salvo i valori 
                    MostraParametriCiclo(false);
                    _sb.ModCicloCorrente.GeneraProgrammaCarica();
                    _sb.ProfiloAttivo = _sb.ModCicloCorrente.ProfiloRegistrato;


                    MessaggioLadeLight.MessaggioProgrammazione NuovoPrg;
                    NuovoPrg = new MessaggioLadeLight.MessaggioProgrammazione(_sb.ProfiloAttivo);
                    if (NuovoPrg.GeneraByteArray())
                    {
                        Log.Info("Programma  " + NuovoPrg.IdProgrammazione.ToString() + ":");
                        Log.Info(FunzioniComuni.HexdumpArray(NuovoPrg.dataBuffer));

                        _datiTemp = new byte[226];
                        _datiTemp = NuovoPrg.dataBuffer;

                    }
                


                    byte[] IdBatteria = new byte[6];
                    string _tempId;
                    if (_sb.sbCliente.BatteryLLId == "")
                    {
                        _tempId = "N.D.  ";
                    }
                    else
                    {
                        _tempId = _sb.sbCliente.BatteryLLId + "      ";
                    }

                    IdBatteria = Encoding.ASCII.GetBytes(_tempId.Substring(0, 6));

                    esitoSalvataggio = _sb.SalvaProgrammazioneLL(_sb.Id, true, _datiTemp, IdBatteria);

                }

                return esitoSalvataggio;
            }
            catch
            {
                return false;
            }
        }

        public bool LeggiValoriParametriCarica()
        {
            try
            {


                // Nome
                string _tempStr = txtPaNomeSetup.Text.Trim();
                // Cassone
                _sb.ModCicloCorrente.ValoriCiclo.TipoCassone = FunzioniMR.ConvertiUshort(txtPaCassone.Text, 1, 0);


                // Generale
                _sb.ModCicloCorrente.NomeProfilo = _tempStr;
                //ModCicloCorrente.IdProgramma = FunzioniMR.ConvertiUshort(txtPaIdSetup.Text, 1, 0);
                // Tipo LL
                if (cmbPaTipoLadeLight.SelectedItem != null)
                {
                    _llModelloCb TempLL = (_llModelloCb)cmbPaTipoLadeLight.SelectedItem;
                    _sb.ModCicloCorrente.ValoriCiclo.IdModelloLL = (ushort)TempLL.IdModelloLL;
                }
                else
                {
                    // Non ho una batteria attiva. mi fermo quì
                    return false;
                }



                // Batteria
                if (cmbPaTipoBatteria.SelectedItem != null)
                {
                    _sb.ModCicloCorrente.Batteria = (mbTipoBatteria)cmbPaTipoBatteria.SelectedItem;
                }
                else
                {
                    // Non ho una batteria attiva. mi fermo quì
                    return false;
                }

                // Tensione
                _sb.ModCicloCorrente.Tensione = FunzioniMR.ConvertiUshort(txtPaTensione.Text, 100, 0);
                // Numero Celle
                _sb.ModCicloCorrente.NumeroCelle = FunzioniMR.ConvertiByte(txtPaNumCelle.Text, 1, 1);
                // Capacità
                _sb.ModCicloCorrente.Capacita = FunzioniMR.ConvertiUshort(txtPaCapacita.Text, 10, 0);
                // Profilo
                if (cmbPaProfilo.SelectedItem != null)
                {
                    _sb.ModCicloCorrente.Profilo = (_mbProfiloCarica)cmbPaProfilo.SelectedItem;
                }
                else
                {
                    // Non ho un profilo attivo. mi fermo quì
                    return false;
                }

                // Flag:
                // Equal
                _sb.ModCicloCorrente.ValoriCiclo.EqualAttivo = (ushort)(chkPaAttivaEqual.Checked ? 0x000F : 0x00F0);

                // Mant:
                _sb.ModCicloCorrente.ValoriCiclo.MantAttivo = (ushort)(chkPaAttivaMant.Checked ? 0x000F : 0x00F0);

                // Usa SB
                _sb.ModCicloCorrente.ValoriCiclo.AbilitaSpyBatt = (ushort)(chkPaUsaSpyBatt.Checked ? 0x000F : 0x00F0);

                // Usa Safety
                _sb.ModCicloCorrente.ValoriCiclo.AbilitaSafety = (ushort)(chkPaUsaSafety.Checked ? 0x000F : 0x00F0);


                // Preciclo
                _sb.ModCicloCorrente.ValoriCiclo.CorrenteI0 = FunzioniMR.ConvertiUshort(txtPaPrefaseI0.Text, 10, 0);
                _sb.ModCicloCorrente.ValoriCiclo.TensionePrecicloV0 = FunzioniMR.ConvertiUshort(txtPaSogliaV0.Text, 100, 0);
                _sb.ModCicloCorrente.ValoriCiclo.TempoT0Max = FunzioniMR.ConvertiUshort(txtPaDurataMaxT0.Text, 1, 0);

                // Fase 1 (I) 
                _sb.ModCicloCorrente.ValoriCiclo.TensioneSogliaVs = FunzioniMR.ConvertiUshort(txtPaSogliaVs.Text, 100, 0);
                _sb.ModCicloCorrente.ValoriCiclo.CorrenteI1 = FunzioniMR.ConvertiUshort(txtPaCorrenteI1.Text, 10, 0);
                _sb.ModCicloCorrente.ValoriCiclo.TempoT1Max = FunzioniMR.ConvertiUshort(cmbPaDurataMaxT1.Text, 1, 0);

                // Fase 2 (U o W) 
                _sb.ModCicloCorrente.ValoriCiclo.TensioneRaccordoVr = FunzioniMR.ConvertiUshort(txtPaRaccordoF1.Text, 100, 0);
                _sb.ModCicloCorrente.ValoriCiclo.CorrenteRaccordoIr = FunzioniMR.ConvertiUshort(txtPaCorrenteRaccordo.Text, 10, 0);
                _sb.ModCicloCorrente.ValoriCiclo.CorrenteFinaleI2 = FunzioniMR.ConvertiUshort(txtPaCorrenteF3.Text, 10, 0);
                _sb.ModCicloCorrente.ValoriCiclo.TensioneMassimaVMax = FunzioniMR.ConvertiUshort(txtPaVMax.Text, 100, 0);
                _sb.ModCicloCorrente.ValoriCiclo.TempoT2Min = FunzioniMR.ConvertiUshort(txtPaTempoT2Min.Text, 1, 0);
                _sb.ModCicloCorrente.ValoriCiclo.TempoT2Max = FunzioniMR.ConvertiUshort(txtPaTempoT2Max.Text, 1, 0);
                _sb.ModCicloCorrente.ValoriCiclo.FattoreK = FunzioniMR.ConvertiUshort(txtPaCoeffK.Text, 1, 0);

                // Fase 3 (I) 
                _sb.ModCicloCorrente.ValoriCiclo.TempoT3Max = FunzioniMR.ConvertiUshort(txtPaTempoT3Max.Text, 1, 0);

                // Equalizzazione
                _sb.ModCicloCorrente.ValoriCiclo.EqualAttivabile = (ushort)(chkPaAttivaEqual.Checked == true ? 0x000F : 0x00F0);
                _sb.ModCicloCorrente.ValoriCiclo.EqualTempoAttesa = FunzioniMR.ConvertiUshort(txtPaEqualAttesa.Text, 1, 0);
                _sb.ModCicloCorrente.ValoriCiclo.EqualNumImpulsi = FunzioniMR.ConvertiUshort(txtPaEqualNumPulse.Text, 1, 0);
                _sb.ModCicloCorrente.ValoriCiclo.EqualTempoPausa = FunzioniMR.ConvertiUshort(txtPaEqualPulsePause.Text, 1, 0);
                _sb.ModCicloCorrente.ValoriCiclo.EqualTempoImpulso = FunzioniMR.ConvertiUshort(txtPaEqualPulseTime.Text, 1, 0);
                _sb.ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso = FunzioniMR.ConvertiUshort(txtPaEqualPulseCurrent.Text, 10, 0);

                // Mantenimento
                _sb.ModCicloCorrente.ValoriCiclo.MantAttivabile = (ushort)(chkPaAttivaMant.Checked == true ? 0x000F : 0x00F0);
                _sb.ModCicloCorrente.ValoriCiclo.MantTempoAttesa = FunzioniMR.ConvertiUshort(txtPaMantAttesa.Text, 1, 0);
                _sb.ModCicloCorrente.ValoriCiclo.MantTensIniziale = FunzioniMR.ConvertiUshort(txtPaMantVmin.Text, 100, 0);
                _sb.ModCicloCorrente.ValoriCiclo.MantTensFinale = FunzioniMR.ConvertiUshort(txtPaMantVmax.Text, 100, 0);
                _sb.ModCicloCorrente.ValoriCiclo.MantTempoMaxErogazione = FunzioniMR.ConvertiUshort(txtPaMantDurataMax.Text, 1, 0);
                _sb.ModCicloCorrente.ValoriCiclo.MantCorrenteImpulso = FunzioniMR.ConvertiUshort(txtPaMantCorrente.Text, 10, 0);

                // Opportunity
                _sb.ModCicloCorrente.ValoriCiclo.OpportunityAttivabile = (ushort)(chkPaAttivaOppChg.Checked == true ? 0x000F : 0x00F0);
                // non leggo le textbox degli orari: lo slider aggiorna direttamente il parametro
                //ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = FunzioniMR.ConvertiUshort(txtPaOppOraInizio.Text, 1, 0);  
                //ModCicloCorrente.ValoriCiclo.OpportunityOraFine = FunzioniMR.ConvertiUshort(txtPaOppOraFine.Text, 1, 0);
                _sb.ModCicloCorrente.ValoriCiclo.OpportunityDurataMax = FunzioniMR.ConvertiUshort(txtPaOppDurataMax.Text, 1, 0);
                _sb.ModCicloCorrente.ValoriCiclo.OpportunityCorrente = FunzioniMR.ConvertiUshort(txtPaOppCorrente.Text, 10, 0);
                _sb.ModCicloCorrente.ValoriCiclo.OpportunityTensioneMax = FunzioniMR.ConvertiUshort(txtPaOppVSoglia.Text, 100, 0);



                // Soglie
                _sb.ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMin = FunzioniMR.ConvertiUshort(txtPaVMinRic.Text, 100, 0);
                _sb.ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMax = FunzioniMR.ConvertiUshort(txtPaVMaxRic.Text, 100, 0);
                _sb.ModCicloCorrente.ValoriCiclo.TensMinStop = FunzioniMR.ConvertiUshort(txtPaVMinStop.Text, 100, 0);
                _sb.ModCicloCorrente.ValoriCiclo.TensioneLimiteVLim = FunzioniMR.ConvertiUshort(txtPaVLimite.Text, 100, 0);
                _sb.ModCicloCorrente.ValoriCiclo.CorrenteMassima = FunzioniMR.ConvertiUshort(txtPaCorrenteMassima.Text, 10, 0);

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("LeggiValoriParametriCarica: " + Ex.Message);
                return false;
            }
        }

        public bool VerificaValoriParametriCarica()
        {
            try
            {


                // Cassone
                if (_sb.ModCicloCorrente.ValoriCiclo.TipoCassone != FunzioniMR.ConvertiUshort(txtPaCassone.Text, 1, 0)) return false;

                // Nome
                string _tempStr = txtPaNomeSetup.Text.Trim();
                // Generale
                if (_sb.ModCicloCorrente.NomeProfilo != _tempStr) return false;
                //ModCicloCorrente.IdProgramma = FunzioniMR.ConvertiUshort(txtPaIdSetup.Text, 1, 0);

                // Batteria
                if (cmbPaTipoBatteria.SelectedItem != null)
                {
                    mbTipoBatteria tmpBat = (mbTipoBatteria)(cmbPaTipoBatteria.SelectedItem);
                    if (_sb.ModCicloCorrente.Batteria.BatteryTypeId != tmpBat.BatteryTypeId) return false;
                }
                else
                {
                    // Non ho una batteria attiva. mi fermo quì
                    return false;
                }

                // Tensione
                if (_sb.ModCicloCorrente.Tensione != FunzioniMR.ConvertiUshort(txtPaTensione.Text, 100, 0)) return false;
                // Numero Celle
                if (_sb.ModCicloCorrente.NumeroCelle != FunzioniMR.ConvertiByte(txtPaNumCelle.Text, 1, 1)) return false;
                // Capacità
                if (_sb.ModCicloCorrente.Capacita != FunzioniMR.ConvertiUshort(txtPaCapacita.Text, 10, 0)) return false;
                // Profilo
                if (cmbPaProfilo.SelectedItem != null)
                {
                    _mbProfiloCarica tmpPC = (_mbProfiloCarica)(cmbPaProfilo.SelectedItem);
                    if (_sb.ModCicloCorrente.Profilo.IdProfiloCaricaLL != tmpPC.IdProfiloCaricaLL) return false;
                }
                else
                {
                    // Non ho un profilo attivo. mi fermo quì
                    return false;
                }

                // Flag:
                // Equal
                //if (ModCicloCorrente.ValoriCiclo.EqualAttivo != (ushort)(chkPaAttivaEqual.Checked ? 0x000F : 0x00F0)) return false;

                // Mant:
                //if (ModCicloCorrente.ValoriCiclo.MantAttivo != (ushort)(chkPaAttivaMant.Checked ? 0x000F : 0x00F0)) return false;

                // Usa SB
                if (_sb.ModCicloCorrente.ValoriCiclo.AbilitaSpyBatt != (ushort)(chkPaUsaSpyBatt.Checked ? 0x0000 : 0x00F0)) return false;


                // Preciclo
                if (_sb.ModCicloCorrente.ValoriCiclo.CorrenteI0 != FunzioniMR.ConvertiUshort(txtPaPrefaseI0.Text, 10, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.TensionePrecicloV0 != FunzioniMR.ConvertiUshort(txtPaSogliaV0.Text, 100, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.TempoT0Max != FunzioniMR.ConvertiUshort(txtPaDurataMaxT0.Text, 1, 0)) return false;

                // Fase 1 (I) 
                if (_sb.ModCicloCorrente.ValoriCiclo.TensioneSogliaVs != FunzioniMR.ConvertiUshort(txtPaSogliaVs.Text, 100, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.CorrenteI1 != FunzioniMR.ConvertiUshort(txtPaCorrenteI1.Text, 10, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.TempoT1Max != FunzioniMR.ConvertiUshort(cmbPaDurataMaxT1.Text, 1, 0)) return false;

                // Fase 2 (U o W) 
                if (_sb.ModCicloCorrente.ValoriCiclo.TensioneRaccordoVr != FunzioniMR.ConvertiUshort(txtPaRaccordoF1.Text, 100, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.CorrenteRaccordoIr != FunzioniMR.ConvertiUshort(txtPaCorrenteRaccordo.Text, 10, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.CorrenteFinaleI2 != FunzioniMR.ConvertiUshort(txtPaCorrenteF3.Text, 10, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.TensioneMassimaVMax != FunzioniMR.ConvertiUshort(txtPaVMax.Text, 100, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.TempoT2Min != FunzioniMR.ConvertiUshort(txtPaTempoT2Min.Text, 1, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.TempoT2Max != FunzioniMR.ConvertiUshort(txtPaTempoT2Max.Text, 1, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.FattoreK != FunzioniMR.ConvertiUshort(txtPaCoeffK.Text, 1, 0)) return false;

                // Fase 3 (I) 
                if (_sb.ModCicloCorrente.ValoriCiclo.TempoT3Max != FunzioniMR.ConvertiUshort(txtPaTempoT3Max.Text, 1, 0)) return false;

                // Equalizzazione
                if (_sb.ModCicloCorrente.ValoriCiclo.EqualAttivabile != (ushort)(chkPaAttivaEqual.Checked == true ? 0x000F : 0x00F0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.EqualTempoAttesa != FunzioniMR.ConvertiUshort(txtPaEqualAttesa.Text, 1, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.EqualNumImpulsi != FunzioniMR.ConvertiUshort(txtPaEqualNumPulse.Text, 1, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.EqualTempoPausa != FunzioniMR.ConvertiUshort(txtPaEqualPulsePause.Text, 1, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.EqualTempoImpulso != FunzioniMR.ConvertiUshort(txtPaEqualPulseTime.Text, 1, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso != FunzioniMR.ConvertiUshort(txtPaEqualPulseCurrent.Text, 10, 0)) return false;

                // Mantenimento
                if (_sb.ModCicloCorrente.ValoriCiclo.MantAttivabile != (ushort)(chkPaAttivaMant.Checked == true ? 0x000F : 0x000F0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.MantTempoAttesa != FunzioniMR.ConvertiUshort(txtPaMantAttesa.Text, 1, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.MantTensIniziale != FunzioniMR.ConvertiUshort(txtPaMantVmin.Text, 100, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.MantTensFinale != FunzioniMR.ConvertiUshort(txtPaMantVmax.Text, 100, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.MantTempoMaxErogazione != FunzioniMR.ConvertiUshort(txtPaMantDurataMax.Text, 1, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.MantCorrenteImpulso != FunzioniMR.ConvertiUshort(txtPaMantCorrente.Text, 10, 0)) return false;


                // Opportunity

                if (_sb.ModCicloCorrente.ValoriCiclo.OpportunityAttivabile != (ushort)(chkPaAttivaOppChg.Checked == true ? 0x000F : 0x000F0)) return false;
                // non leggo le textbox degli orari: lo slider aggiorna direttamente il parametro
                //ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = FunzioniMR.ConvertiUshort(txtPaOppOraInizio.Text, 1, 0);  
                //ModCicloCorrente.ValoriCiclo.OpportunityOraFine = FunzioniMR.ConvertiUshort(txtPaOppOraFine.Text, 1, 0);
                if (_sb.ModCicloCorrente.ValoriCiclo.OpportunityDurataMax != FunzioniMR.ConvertiUshort(txtPaOppDurataMax.Text, 1, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.OpportunityCorrente != FunzioniMR.ConvertiUshort(txtPaOppCorrente.Text, 10, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.OpportunityTensioneMax != FunzioniMR.ConvertiUshort(txtPaOppVSoglia.Text, 100, 0)) return false;

                // Soglie
                if (_sb.ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMin != FunzioniMR.ConvertiUshort(txtPaVMinRic.Text, 100, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMax != FunzioniMR.ConvertiUshort(txtPaVMaxRic.Text, 100, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.TensMinStop != FunzioniMR.ConvertiUshort(txtPaVMinStop.Text, 100, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.TensioneLimiteVLim != FunzioniMR.ConvertiUshort(txtPaVLimite.Text, 100, 0)) return false;
                if (_sb.ModCicloCorrente.ValoriCiclo.CorrenteMassima != FunzioniMR.ConvertiUshort(txtPaCorrenteMassima.Text, 10, 0)) return false;

                // TRUE => nessun parametro è stato modificato.
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("LeggiValoriParametriCarica: " + Ex.Message);
                return false;
            }
        }








        



    }
}
