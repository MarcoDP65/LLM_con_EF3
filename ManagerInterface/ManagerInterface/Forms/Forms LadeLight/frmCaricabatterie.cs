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
using System.Globalization;

namespace PannelloCharger
{
    public partial class frmCaricabatterie : Form

    {
        parametriSistema _parametri;
        SerialMessage _msg ;
        LogicheBase _logiche;

        private CaricaBatteria _cb;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        private frmAvanzamentoCicli _avCicli = new frmAvanzamentoCicli();

        bool readingMessage = false;
        bool _portaCollegata;
        bool _cbCollegato;

        bool _apparatoPresente = false;

        // Liste per la gestione degli elenchi filtrati di profili e durate
        private List<_llProfiloCarica> ProfiliCarica;
        private List<llDurataCarica> DurateCarica;
        private List<llTensioneBatteria> TensioniBatteria;


        /* ----------------------------------------------------------
         *  Dichiarazione eventi per la gestione avanzamento
         * ---------------------------------------------------------
         */
        public event StepHandler Step;
        public delegate void StepHandler(CaricaBatteria ull, ProgressChangedEventArgs e); //sbWaitEventStep e);
                                                                                        // ----------------------------------------------------------



        public List<llVariabili> ListaValori = new List<llVariabili>();  // lista per listview realtime logger
        public List<ParametriArea> ListaAreeLLF = new List<ParametriArea>();  // lista per listview file Firmware upload
        public List<ParametriArea> ListaAreeCCS = new List<ParametriArea>();  // lista per listview file Firmware upload


        public System.Collections.Generic.List<ValoreLista> ListaBrSig = new List<ValoreLista>()
        {
            new ValoreLista("OFF", SerialMessage.OcBaudRate.OFF, false),
            new ValoreLista("ON 9.6K", SerialMessage.OcBaudRate.br_9k6, false),
            new ValoreLista("ON 19.2K", SerialMessage.OcBaudRate.br_19k2, false),
            new ValoreLista("ON 38.4K", SerialMessage.OcBaudRate.br_38k4, false),
            new ValoreLista("ON 57.6K", SerialMessage.OcBaudRate.br_57k6, false),
        };

        private llParametriApparato _tempParametri;


        public frmCaricabatterie(ref parametriSistema _par, bool CaricaDati, string IdApparato, LogicheBase Logiche, bool SerialeCollegata, bool AutoUpdate)
        {
            try
            {

                _parametri = _par;
                InitializeComponent();
                ResizeRedraw = true;

                _logiche = Logiche;
                attivaCaricabatterie(ref _par, SerialeCollegata);
                InizializzaScheda();
                this.Cursor = Cursors.Arrow;

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }

        }

        public frmCaricabatterie(ref parametriSistema _par, bool CaricaDati)
        {
            try
            {
                attivaCaricabatterie(ref _par, CaricaDati);
                InizializzaScheda();

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }

        }


        public void attivaCaricabatterie(ref parametriSistema _par,bool CaricaDati )
        {
            bool _esito;
            try
            {
                //_parametri = _par;
                //InitializeComponent();
                ResizeRedraw = true;
                _msg = new SerialMessage();
                _cb = new CaricaBatteria(ref _parametri);
                InizializzaScheda();
                _esito = _cb.apriPorta();
                if (!_esito)
                {
                    MessageBox.Show(_parametri.lastError, "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Hide();  //Close();
                    return;
                }

                //_esito = _cb.VerificaPresenza();
                _tempParametri = new llParametriApparato();
                _esito = _cb.CaricaApparatoA0();
                if (_esito)
                {

                    _tempParametri = _cb.ParametriApparato;
                    

                    if(_tempParametri.IdApparato == null)
                    {
                        // La scheda risponde correttamente ma non è inizializzata completamente (manca l'ID apparato)....
                        // Attivo solo la tab inizializzazione, se sono abilitato 

                        MessageBox.Show("Scheda controllo non inizializzata", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                       // MascheraTabPages(1);

                        //this.Close();
                        return;
                    }
                    else
                    {
                        if (_tempParametri.IdApparato == "????????" || _tempParametri.IdApparato.Trim() =="" )
                        {
                            // La scheda risponde correttamente ma non è inizializzata completamente (manca l'ID apparato)....
                            // Attivo solo la tab inizializzazione, se sono abilitato 

                            MessageBox.Show("Scheda controllo non inizializzata", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            // this.Close();
                            // MascheraTabPages(1);
                            return;
                        }

                    }

                    _cb.ApparatoLL = new LadeLightData(_logiche.dbDati.connessione, _tempParametri);


                    txtGenIdApparato.Text = _cb.ApparatoLL._ll.Id;
                    txtGenSerialeZVT.Text = _cb.ApparatoLL._ll.SerialeZVT;


                    /*
                    txtMatricola.Text = _cb.Intestazione.Matricola.ToString();
                    txtProgressivo.Text = _cb.Intestazione.Progressivo.ToString();
                    txtDataPrimaInst.Text = _cb.Intestazione.PrimaInstallazione;
                    txtModello.Text = _cb.Intestazione.modello;
                    txtTensione.Text = _cb.Intestazione.tensioneNominale.ToString();
                    txtCorrente.Text = _cb.Intestazione.correnteNominale.ToString();
                    txtFwCb.Text = _cb.Intestazione.revSoftware;
                    txtFwDisp.Text = _cb.Intestazione.revDisplay;
                    txtNumCicli.Text = "2";
                    */

                    _cbCollegato = true;
                    _apparatoPresente = _esito;
                    return;

                }

            }
            catch 
            { }

        }
        public frmCaricabatterie()
        {
            InitializeComponent();
            InizializzaScheda();
        }

        /// <summary>
        /// Inizializza le combo; da rendere dinamico in effetivo
        /// </summary>
        private void InizializzaScheda()
        {
            /*
            cmbPaProfilo.Items.Clear();
            cmbPaProfilo.Items.Add("N.D.");
            cmbPaProfilo.Items.Add("IWa");
            cmbPaProfilo.Items.Add("IU");
            cmbPaProfilo.Items.Add("IUIa");
            cmbPaProfilo.Items.Add("IWa Pb13");
            cmbPaProfilo.Items.Add("IWa Pb11");
            cmbPaProfilo.Items.Add("IWa Pb8");
            cmbPaProfilo.SelectedIndex = 0;
            */

            cmbPaCondStop.Items.Clear();
            cmbPaCondStop.Items.Add("N.D.");
            cmbPaCondStop.Items.Add("Timer");
            cmbPaCondStop.Items.Add("dV/dt");
            cmbPaCondStop.SelectedIndex = 0;

            cmbInitTipoApparato.DataSource = _cb.ModelliLL;
            cmbInitTipoApparato.ValueMember = "IdModelloLL";
            cmbInitTipoApparato.DisplayMember = "NomeModello";

            cmbPaTipoBatteria.DataSource = _parametri.TipiBattria;
            cmbPaTipoBatteria.ValueMember = "BatteryTypeId";
            cmbPaTipoBatteria.DisplayMember = "BatteryType";

            // NUOVA VERSIONE
            InizializzaVistaProgrammazioni();


        }

        public bool ScriviParametriAttuali()
        {
            try
            {
                SerialMessage.cicloAttuale _tempCiclo = new SerialMessage.cicloAttuale();

                byte _numParametri = 0;
                ushort _divK = 10;
                _cb.CicloInMacchina = new SerialMessage.cicloAttuale()
                {
                    LunghezzaNome = (byte)txtPaNomeProfilo.Text.Length,
                    NomeCiclo = txtPaNomeProfilo.Text,
                    TipoCiclo = (byte)cmbPaProfilo.SelectedIndex
                };


                if (txtPaCapacita.Text.Length>0)
                {
                    ushort result;
                    if (ushort.TryParse(txtPaCapacita.Text, out result))
                    {
                        ParametroLL _par = new ParametroLL()
                        {
                            idParametro = (byte)SerialMessage.ParametroLadeLight.CapacitaNominale,
                            ValoreParametro = result
                        };
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }
                }

                if (txtPaCapDaCaricare.Text.Length > 0)
                {
                    ushort result;
                    if (ushort.TryParse(txtPaCapDaCaricare.Text, out result))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.CapacitaDaRicaricare;
                        _par.ValoreParametro = result;
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }
                }

                if (txtPaTempoMax.Text.Length > 0)
                {
                    ushort result;
                    if (ushort.TryParse(txtPaTempoMax.Text, out result))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TempoMassimoCarica;
                        _par.ValoreParametro = result;
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }
                }


                if (txtPaSoglia.Text.Length > 0)
                {
                    Double dresult;
                    ushort result;

                    if (Double.TryParse(txtPaSoglia.Text, out dresult))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneSogliaCella;
                        result = (ushort)(dresult * 100);
                        _par.ValoreParametro = result;
                        txtPaSoglia.Text = dresult.ToString("0.00");
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }

                }


                if (txtPaParDivK.Text.Length > 0)
                {
                    
                    ushort result;

                    if (ushort.TryParse(txtPaParDivK.Text, out result))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.DivisoreK;
                        _par.ValoreParametro = result;
                        txtPaParDivK.Text = result.ToString("0");
                        _divK = result;
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }

                }



                if (txtPaParKp.Text.Length > 0)
                {
                    Double dresult;
                    ushort result;

                    if (Double.TryParse(txtPaParKp.Text, out dresult))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.ParametroKP;
                        result = (ushort)(dresult * _divK);
                        _par.ValoreParametro = result;
                        dresult = (double) result / _divK;
                        txtPaParKp.Text = dresult.ToString(FunzioniMR.StringaModelloDivisore(_divK));
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }

                }

                if (txtPaParKi.Text.Length > 0)
                {
                    Double dresult;
                    ushort result;

                    if (Double.TryParse(txtPaParKi.Text, out dresult))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.ParametroKI;
                        result = (ushort)(dresult * _divK);
                        _par.ValoreParametro = result;
                        dresult = (double) result / _divK;
                        txtPaParKi.Text = dresult.ToString(FunzioniMR.StringaModelloDivisore(_divK));
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }

                }


                if (txtPaParKd.Text.Length > 0)
                {
                    Double dresult;
                    ushort result;

                    if (Double.TryParse(txtPaParKd.Text, out dresult))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.ParametroKD;
                        result = (ushort)(dresult * _divK);
                        _par.ValoreParametro = result;
                        dresult = (double) result / _divK;
                        txtPaParKd.Text = dresult.ToString(FunzioniMR.StringaModelloDivisore(_divK));
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }

                }


                if (txtPaCorrenteMax.Text.Length > 0)
                {
                    ushort result;
                    if (ushort.TryParse(txtPaCorrenteMax.Text, out result))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.CorrenteCarica;
                        _par.ValoreParametro = result;
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }
                }

                if (txtPaTensione.Text.Length > 0)
                {
                    ushort result;
                    if (ushort.TryParse(txtPaTensione.Text, out result))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneNominale;
                        _par.ValoreParametro = result;
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }
                }

                if (cmbPaCondStop.SelectedIndex > -1)
                {
                 
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.CondizioneStop;
                        _par.ValoreParametro = (ushort)cmbPaCondStop.SelectedIndex;
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    
                }

                if (txtPaCoeffK.Text.Length > 0)
                {
                    Double dresult;
                    ushort result;

                    if (Double.TryParse(txtPaCoeffK.Text, out dresult))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.CoeffK;
                        result = (ushort)(dresult * 10);
                        _par.ValoreParametro = result;
                        dresult = result / 10;
                        txtPaCoeffK.Text = dresult.ToString("0.0");
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }

                }

                if (txtPaTempoT2Min.Text.Length > 0)
                {
                    ushort result;
                    if (ushort.TryParse(txtPaTempoT2Min.Text, out result))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TempoT2Min;
                        _par.ValoreParametro = result;
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }
                }

                if (txtPaTempoT2Max.Text.Length > 0)
                {
                    ushort result;
                    if (ushort.TryParse(txtPaTempoT2Max.Text, out result))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TempoT2Max;
                        _par.ValoreParametro = result;
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }
                }

                if (txtPaFreqSwitch.Text.Length > 0)
                {
                    ushort result;
                    if (ushort.TryParse(txtPaFreqSwitch.Text, out result))
                    {
                        ParametroLL _par = new ParametroLL()
                        {
                            idParametro = (byte)SerialMessage.ParametroLadeLight.FrequenzaSwitching,
                            ValoreParametro = result
                        };
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }
                }

                _cb.CicloInMacchina.NumeroParametri = _numParametri;

                _cb.ScriviCicloCorrente();

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
                ushort result;
                float resultF;

                // Se il ciclo attuale non è mai stato usato, sovrascrivo
                if (_cb.ProgrammaAttivo == null)
                {
                    _cb.ProgrammaAttivo = new llProgrammaCarica();

                }

                if (_cb.ProgrammaAttivo.ProgrammaInUso != 0xFF)
                {
                    //scorro la lista, butto l'ultimo se già 16 presenti
                    // AL MOMENTO SOVRASCRIVO SEMPRE LO STESSO
                }

                _cb.ProgrammaAttivo.IdProgramma = 1; // diventa progressivo
                _cb.ProgrammaAttivo.ProgrammaInUso = 0xFF;

                // Nome
                string _tempStr = txtPaNomeSetup.Text.Trim();
                _cb.ProgrammaAttivo.ProgramName = _tempStr;

                _cb.ProgrammaAttivo.BatteryType = (byte)cmbPaTipoBatteria.SelectedValue;

                // Tensione
                _cb.ProgrammaAttivo.BatteryVdef = FunzioniMR.ConvertiUshort(txtPaTensione.Text, 100, 0);
                // Capacità
                _cb.ProgrammaAttivo.BatteryAhdef = FunzioniMR.ConvertiUshort(txtPaCapacita.Text, 10, 0);
                // ID Profilo
                byte TipoProf;

                if (cmbPaProfilo.SelectedItem == null)
                {
                    // Non Definito
                    TipoProf = 0;
                }
                else
                {
                    TipoProf = (byte)((_llProfiloCarica)cmbPaProfilo.SelectedItem).IdProfiloCaricaLL;
                }
                _cb.ProgrammaAttivo.IdProfilo = TipoProf;

                // Durata Carica, default 12 H
                ushort MinutiCarica = 720;
                if (cmbPaDurataCarica.SelectedItem != null)
                {
                    MinutiCarica = (ushort)((llDurataCarica)cmbPaDurataCarica.SelectedItem).IdDurataCaricaLL;
                }

                _cb.ProgrammaAttivo.DurataMaxCarica = MinutiCarica; 

                _cb.ProgrammaAttivo.PercTempoFase2 = FunzioniMR.ConvertiUshort(txtPaTempoT2Min.Text, 1, 0);

                _cb.ProgrammaAttivo.VSoglia = FunzioniMR.ConvertiUshort(txtPaSoglia.Text, 100, 0);
                _cb.ProgrammaAttivo.VMax = FunzioniMR.ConvertiUshort(txtPaVMax.Text, 100, 0);


                _cb.ProgrammaAttivo.EqualTempoAttesa  = FunzioniMR.ConvertiByte(txtPaEqualAttesa.Text, 1, 0);
                _cb.ProgrammaAttivo.EqualNumImpulsi = FunzioniMR.ConvertiByte(txtPaEqualNumPulse.Text, 1, 0);


                _cb.ProgrammaAttivo.TempoErogazioneBMS = FunzioniMR.ConvertiByte(txtPaBMSTempoErogazione.Text, 1, 0);
                _cb.ProgrammaAttivo.TempoAttesaBMS = FunzioniMR.ConvertiByte(txtPaBMSTempoAttesa.Text, 1, 0);



                _cb.ProgrammaAttivo.GeneraListaParametri();

                _cb.SalvaProgrammazioniApparato();

                return true;
            }
            catch
            {
                return false;
            }
        }





        /// <summary>
        /// Aggiorno il form con i dati di ciclo attivo dell'ultima lettura
        /// </summary>
        /// <returns></returns>
        public bool MostraCicloAttuale(SerialMessage.cicloAttuale CicloAttuale )
        {
            ushort _divK = 10;
            try
            {

                //Prima Vuoto tutto
                txtPaNomeProfilo.Text = "";

                cmbPaProfilo.SelectedIndex = 0;
                txtPaCapacita.Text = "";
                txtPaTempoMax.Text = "";
                txtPaSoglia.Text = "";
                txtPaCorrenteMax.Text = "";
                txtPaTensione.Text = "";

                cmbPaCondStop.SelectedIndex = 0;
                txtPaCoeffK.Text = "";
                txtPaTempoT2Min.Text = "";
                txtPaTempoT2Max.Text = "";
                chkPaUsaSpyBatt.CheckState = CheckState.Indeterminate;

                if (CicloAttuale.datiPronti)
                {
                    txtPaNomeProfilo.Text = CicloAttuale.NomeCiclo;
                    cmbPaProfilo.SelectedIndex = CicloAttuale.TipoCiclo;
                     
                    foreach(ParametroLL _par in CicloAttuale.Parametri)
                    {
                        float _tempVal;

                        switch (_par.idParametro)
                        {
                            case (byte)SerialMessage.ParametroLadeLight.CapacitaNominale:
                                txtPaCapacita.Text = _par.ValoreParametro.ToString();
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.TempoMassimoCarica:
                                txtPaTempoMax.Text = _par.ValoreParametro.ToString();
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.TensioneSogliaCella:      
                                _tempVal = (float)_par.ValoreParametro / 100;
                                txtPaSoglia.Text = _tempVal.ToString("0.00");
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.CorrenteCarica:
                                txtPaCorrenteMax.Text = _par.ValoreParametro.ToString();
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.TensioneNominale:
                                txtPaTensione.Text = _par.ValoreParametro.ToString();
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.CondizioneStop:
                                cmbPaCondStop.SelectedIndex = _par.ValoreParametro;
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.CoeffK:
                                _tempVal = (float)_par.ValoreParametro / 10;
                                txtPaCoeffK.Text = _tempVal.ToString("0.0");
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.TempoT2Min:
                                txtPaTempoT2Min.Text = _par.ValoreParametro.ToString();
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.TempoT2Max:
                                txtPaTempoT2Max.Text = _par.ValoreParametro.ToString();
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.FrequenzaSwitching:
                                txtPaFreqSwitch.Text = _par.ValoreParametro.ToString();
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.DivisoreK:
                                txtPaParDivK.Text = _par.ValoreParametro.ToString();
                                _divK = _par.ValoreParametro;
                                if(_divK > 0)
                                {
                                    //chkPaUsaSpyBatt.Checked = true;
                                    chkPaUsaSpyBatt.CheckState = CheckState.Checked;
                                }
                                else
                                {
                                    //chkPaUsaSpyBatt.Checked = false;
                                    chkPaUsaSpyBatt.CheckState = CheckState.Unchecked;
                                }
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.ParametroKP:
                                _tempVal = (float)_par.ValoreParametro / _divK;
                                txtPaParKp.Text = _tempVal.ToString(FunzioniMR.StringaModelloDivisore(_divK));
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.ParametroKI:
                                _tempVal = (float)_par.ValoreParametro / _divK;
                                txtPaParKi.Text = _tempVal.ToString(FunzioniMR.StringaModelloDivisore(_divK));
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.ParametroKD:
                                _tempVal = (float)_par.ValoreParametro / _divK;
                                txtPaParKd.Text = _tempVal.ToString(FunzioniMR.StringaModelloDivisore(_divK));
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.CapacitaDaRicaricare:
                                txtPaCapDaCaricare.Text = _par.ValoreParametro.ToString();
                                break;

                        }


                    }


                }



                return true;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
 
                return false;
            }
        }


        /// <summary>
        /// Aggiorno il form con i dati di ciclo attivo dell'ultima lettura
        /// </summary>
        /// <returns></returns>
        public bool MostraContatori()
        {

            try
            {

                //Prima Vuoto tutto
                txtContDtPrimaCarica.Text = "";
                txtContDtUltimaCanc.Text = "";
                txtContBreviSalvati.Text = "";
                txtContCariche3to6.Text = "";
                txtContCariche6to9.Text = "";
                txtContCaricheOver9.Text = "";
                txtContCaricheSalvate.Text = "";
                txtContCaricheStop.Text = "";
                txtContCaricheStrappo.Text = "";
                txtContCaricheTotali.Text = "";
                txtContCaricheUnder3.Text = "";
                txtContNumCancellazioni.Text = "";
                txtContPntNextBreve.Text = "";
                txtContPntNextCarica.Text = "";

                if (_cb.ContatoriLL.valido)
                {
                    txtContDtPrimaCarica.Text = _cb.ContatoriLL.strDataPrimaCarica;
                    txtContDtUltimaCanc.Text = "";
                    txtContBreviSalvati.Text = _cb.ContatoriLL.CntCicliBrevi.ToString();
                    txtContCariche3to6.Text = _cb.ContatoriLL.CntCicli3Hto6H.ToString();
                    txtContCariche6to9.Text = _cb.ContatoriLL.CntCicli6Hto9H.ToString();
                    txtContCaricheOver9.Text = _cb.ContatoriLL.CntCicliOver9H.ToString();
                    txtContCaricheSalvate.Text = _cb.ContatoriLL.CntCariche.ToString();
                    txtContCaricheStop.Text = _cb.ContatoriLL.CntCicliStop.ToString();
                    txtContCaricheStrappo.Text = _cb.ContatoriLL.CntCicliStaccoBatt.ToString();
                    txtContCaricheTotali.Text = _cb.ContatoriLL.CntCicliTotali.ToString();
                    txtContCaricheUnder3.Text = _cb.ContatoriLL.CntCicliLess3H.ToString();
                    //txtContNumCancellazioni.Text = _cb.ContatoriLL.cn.ToString();
                    txtContPntNextBreve.Text = _cb.ContatoriLL.strPntNextBreve;
                    txtContPntNextCarica.Text = _cb.ContatoriLL.strPntNextCarica;
                }



                return true;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);

                return false;
            }
        }




        private void abilitaSalvataggi(bool _stato)
        {
            /*
            btnRicaricaDati.Enabled = _apparatoPresente;
            btnSalvaCliente.Enabled = _apparatoPresente;
            txtCliente.ReadOnly = !_stato;
            txtNoteCliente.ReadOnly = !_stato;
            txtModelloBat.ReadOnly = !_stato;
            txtMarcaBat.ReadOnly = !_stato;
            txtIdBat.ReadOnly = !_stato;
            */
        }

        private void tabCb01_Click(object sender, EventArgs e)
        {

        }

        private void btnApparatoOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCaricabatterie_Resize(object sender, EventArgs e)
        {

            try
            {
                tabCaricaBatterie.Width = this.Width - 42;
                tabCaricaBatterie.Height = this.Height - 109;

                // Tab Cicli
                flvCicliListaCariche.Width = tabCb04.Width - 20;

            }
            catch
            {

            }

        }

        public void CaricaListaCicli()
        {
            bool _esito;
            try
            {


                _esito = _cb.apriPorta();
                if (!_esito)
                {
                    MessageBox.Show(_parametri.lastError, "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Hide();  //Close();
                    return;
                }
                _esito = _cb.VerificaPresenza();
                if (_esito)
                {
                    /*
                    txtMatricola.Text = _cb.Intestazione.Matricola.ToString();
                    txtProgressivo.Text = _cb.Intestazione.Progressivo.ToString();
                    txtDataPrimaInst.Text = _cb.Intestazione.PrimaInstallazione;
                    txtModello.Text = _cb.Intestazione.modello;
                    txtTensione.Text = _cb.Intestazione.tensioneNominale.ToString();
                    txtCorrente.Text = _cb.Intestazione.correnteNominale.ToString();
                    */

                }

            }
            catch 
            { 
            }
        }

        public void CaricaOrologio()
        {
            bool _esito;
            try
            {

                _esito = _cb.LeggiOrologio();
                if (_esito)
                {
                    txtOraRtc.Text = _cb.OrologioSistema.ore.ToString("00") + ":" + _cb.OrologioSistema.minuti.ToString("00");
                    txtDataRtc.Text = _cb.OrologioSistema.giorno.ToString("00") + "/" + _cb.OrologioSistema.mese.ToString("00") + "/" + _cb.OrologioSistema.anno.ToString("0000");
                }

            }
            catch
            {
            }
        }

        public void CaricaCicloAttuale()
        {
            // NON USATA ??????
            bool _esito;
            try
            {

                _esito = _cb.LeggiCicloAttuale();
                if (_esito)
                {

                    MostraCicloAttuale(_cb.CicloInMacchina);
      
                }

            }
            catch
            {
            }
        }

        public void CaricaListaProgrammazioni()
        {
            bool _esito;
            try
            {


                _esito = _cb.CaricaAreaProgrammi();


                if (_esito)
                {

                    MostraCicloAttuale(_cb.CicloInMacchina);

                }

            }
            catch
            {
            }
        }

        public void CaricaAreaContatori()
        {
            bool _esito;
            try
            {


                _esito = _cb.CaricaAreaContatori();


                if (_esito)
                {

                    MostraContatori();

                }

            }
            catch
            {
            }
        }



        public void CaricaVariabili()
        {
            bool _esito;
            try
            {

                _esito = _cb.LeggiVariabili();

                
                MostraVariabili(_esito, (chkDatiDiretti.Checked == true));
                if(chkParRegistraLetture.Checked)
                {
                    llVariabili _tmpValore = new llVariabili();
                    _tmpValore = _cb.llVariabiliAttuali;
                    _tmpValore.Lettura = ListaValori.Count() + 1;
                    ListaValori.Add(_tmpValore);
                    flvwLettureParametri.SetObjects(ListaValori);
                    Application.DoEvents();
                }


            }
            catch
            {
            }
        }

        private void chkParLetturaAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (chkParLetturaAuto.Checked == true)
            {
                int _intervallo = 1000;
                int.TryParse(txtParIntervalloLettura.Text, out _intervallo);
                tmrLetturaAutomatica.Interval = _intervallo;
                tmrLetturaAutomatica.Enabled = true;

            }
            else
            {
                tmrLetturaAutomatica.Enabled = false;
            }
        }

        /// <summary>
        /// Mostra il valore delle variabili istantanee nella relativa tab.
        /// </summary>
        /// <param name="DatiPresenti">if set to <c>true</c> [dati presenti].</param>
        /// <param name="ValoriReali">Se <c>true</c> mostra i dati nel formato originale.</param>
        private void MostraVariabili(bool DatiPresenti, bool ValoriReali = false)
        {

            try
            {
                //Vuoto i campi


                txtVarVBatt.Text = "";
                txtVarIbatt.Text = "";
                txtVarAhCarica.Text = "";
                txtVarMemProgrammed.Text = "";
                txtVarTempoTrascorso.Text = "";



                if (DatiPresenti )
                {
                    if (ValoriReali)
                    {
                        txtVarVBatt.Text = _cb.llVariabiliAttuali.TensioneIstantanea.ToString();
                        txtVarIbatt.Text = _cb.llVariabiliAttuali.CorrenteIstantanea.ToString();
                        txtVarAhCarica.Text = _cb.llVariabiliAttuali.AhCaricati.ToString();
                        txtVarMemProgrammed.Text = _cb.llVariabiliAttuali.StatoLL.ToString();
                        txtVarTempoTrascorso.Text = _cb.llVariabiliAttuali.SecondsFromStart.ToString();
                    }
                    else
                    {
                        txtVarVBatt.Text = _cb.llVariabiliAttuali.strTensioneIstantanea;
                        txtVarIbatt.Text = _cb.llVariabiliAttuali.strCorrenteIstantanea;
                        if (_cb.llVariabiliAttuali.CorrenteIstantanea < 0) txtVarIbatt.ForeColor = Color.Red;
                        else txtVarIbatt.ForeColor = Color.Black;
                        txtVarAhCarica.Text = _cb.llVariabiliAttuali.strAhCaricati;
                        txtVarTempoTrascorso.Text = _cb.llVariabiliAttuali.strSecondsFromStart;
                        txtVarMemProgrammed.Text = _cb.llVariabiliAttuali.strStatoLL;

    
                    }

                }

            }
            catch (Exception Ex)
            {
                Log.Error("MostraVariabili: " + Ex.Message);
            }


        }


        public void ImpostaOrologio()
        {
            bool _esito;
            try
            {
                _esito = _cb.ScriviOrologio();
                if (_esito)
                {

                    _esito = _cb.LeggiOrologio();
                    if (_esito)
                    {
                        txtOraRtc.Text = _cb.OrologioSistema.ore.ToString("00") + ":" + _cb.OrologioSistema.minuti.ToString("00");
                        txtDataRtc.Text = _cb.OrologioSistema.giorno.ToString("00") + "/" + _cb.OrologioSistema.mese.ToString("00") + "/" + _cb.OrologioSistema.anno.ToString("0000");
                    }
                
                }

            }
            catch
            {
            }
        }


        private void tabCaricaBatterie_Selected(object sender, TabControlEventArgs e)
        {
            try
            {

                if (e.TabPage == tabOrologio)
                {
                    if (_apparatoPresente) CaricaOrologio();
                }

                if (e.TabPage == tabProfiloAttuale)
                {
                   // if (_apparatoPresente) CaricaCicloAttuale(); 
                }

                if (e.TabPage == tbpFirmware)
                {
                    if (_apparatoPresente) VerificaStatoFw();
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat_tabCaricaBatterie_Selected: " + Ex.Message);

            }
        }


 
        private void btnCaricaCicli_Click(object sender, EventArgs e)
        {

            try 
            {

                //CaricaTabelleCicli();
                return;

            }
            catch { }


        }

        private void rbtAccensione01_CheckedChanged(object sender, EventArgs e)
        {
            cmbOreRitardo.Enabled = false;
            cmbMinAccensione.Enabled = false;
            cmbOraAccensione.Enabled = false;
        }

        private void rbtAccensione02_CheckedChanged(object sender, EventArgs e)
        {
            cmbOreRitardo.Enabled = true;
            cmbMinAccensione.Enabled = false;
            cmbOraAccensione.Enabled = false;
        }

        private void rbtAccensione03_CheckedChanged(object sender, EventArgs e)
        {
            cmbOreRitardo.Enabled = false;
            cmbMinAccensione.Enabled = true;
            cmbOraAccensione.Enabled = true;
        }

        private void btnLeggiRtc_Click(object sender, EventArgs e)
        {
            CaricaOrologio();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImpostaOrologio();
        }

        private void txtNumCicli_TextChanged(object sender, EventArgs e)
        {

        }

        private void grbComboSonda_Enter(object sender, EventArgs e)
        {

        }

        private void opSonda02_CheckedChanged(object sender, EventArgs e)
        {
            grbComboSonda.Enabled = true;
        }

        private void opSonda01_CheckedChanged(object sender, EventArgs e)
        {
            grbComboSonda.Enabled = false;

        }

        private void grbCicloCorrente_Enter(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void opbBatGEL_CheckedChanged(object sender, EventArgs e)
        {
            grbBoxPiombo.Enabled = false; 
            grbBoxWet.Enabled = true;
        }

        private void opbBatPB_CheckedChanged(object sender, EventArgs e)
        {
            grbBoxPiombo.Enabled = true;
            grbBoxWet.Enabled = false;

        }

        private void opbCicloIWA_CheckedChanged(object sender, EventArgs e)
        {
            grbTopIUIa.Visible = false;
            grbTopIWa.Visible = true;
            gbFormazione.Enabled = true;
            gbEqualizzazione.Enabled = true;
            gbStopIWa.Visible = true;
            gbStopIUIa.Visible = false;
        }

        private void pbxIWAsmall_Click(object sender, EventArgs e)
        {
            frmGraficoCiclo frmGrafico = new frmGraficoCiclo();
            //sbDettaglioCar.MdiParent = this;
             frmGrafico.StartPosition = FormStartPosition.CenterParent;
             frmGrafico.TipoGrafico(0);
             frmGrafico.ShowDialog();
        }

        private void opbCicloIUIA_CheckedChanged(object sender, EventArgs e)
        {
            grbTopIUIa.Visible = true;
            grbTopIWa.Visible = false;
            gbFormazione.Enabled = false;
            gbEqualizzazione.Enabled = false;
            gbStopIWa.Visible = false;
            gbStopIUIa.Visible = true;

        }

        private void pbminIUIa_Click(object sender, EventArgs e)
        {
            frmGraficoCiclo frmGrafico = new frmGraficoCiclo();
            //sbDettaglioCar.MdiParent = this;
            frmGrafico.StartPosition = FormStartPosition.CenterParent;
            frmGrafico.TipoGrafico(1);
            frmGrafico.ShowDialog();
        }

        private void pbxIWAsmall_Click_1(object sender, EventArgs e)
        {
            frmGraficoCiclo frmGrafico = new frmGraficoCiclo();
            //sbDettaglioCar.MdiParent = this;
            frmGrafico.StartPosition = FormStartPosition.CenterParent;
            frmGrafico.TipoGrafico(0);
            frmGrafico.ShowDialog();
        }

        private void frmCaricabatterie_Load(object sender, EventArgs e)
        {
            grbTopIUIa.Visible = false;
            this.Cursor = Cursors.Arrow;
            this.Width += 10;
        }

        private void pbminIUIa_Click_1(object sender, EventArgs e)
        {
            frmGraficoCiclo frmGrafico = new frmGraficoCiclo();
            //sbDettaglioCar.MdiParent = this;
            frmGrafico.StartPosition = FormStartPosition.CenterParent;
            frmGrafico.TipoGrafico(1);
            frmGrafico.ShowDialog();

        }

        private void rbSov0_CheckedChanged(object sender, EventArgs e)
        {
            cmbSov.Enabled = false;
        }

        private void rbSov1_CheckedChanged(object sender, EventArgs e)
        {
            cmbSov.Enabled = true;
        }

        private void opStop1_CheckedChanged(object sender, EventArgs e)
        {
            gbStop0.Enabled = false;
            gbStop1.Enabled = true;
        }

        private void opStop0_CheckedChanged(object sender, EventArgs e)
        {
            gbStop1.Enabled = false;
            gbStop0.Enabled = true;
        }

        private void opMant1_CheckedChanged(object sender, EventArgs e)
        {
            gbMant1.Enabled = true;
            gbMant2.Enabled = false;
        }

        private void opMant0_CheckedChanged(object sender, EventArgs e)
        {
            gbMant1.Enabled = false;
            gbMant2.Enabled = false;
        }

        private void opMant2_CheckedChanged(object sender, EventArgs e)
        {
            gbMant1.Enabled = false;
            gbMant2.Enabled = true;
        }

        private void btnCicloCorrente_Click(object sender, EventArgs e)
        {
            CaricaCicloAttuale();
        }

        private void btnCaricaMemoria_Click(object sender, EventArgs e)
        {
            bool _esito;
            try
            {

                _esito = _cb.LeggiMemoriaScheda();
                if (_esito)
                {
                 //   tatPaCapacita.Text = _cb.CicloInMacchina.capacita.ToString();
                }

            }
            catch
            {
            }
        }

        private void btnPaSalvaDati_Click(object sender, EventArgs e)
        {
            ScriviParametriCarica();

            // Leggi Parametri.....


        }

        private void btnLeggiVariabili_Click(object sender, EventArgs e)
        {
            bool _esito;
            this.Cursor = Cursors.WaitCursor;
            CaricaVariabili();
            this.Cursor = Cursors.Default;
        }

        private void tmrLetturaAutomatica_Tick(object sender, EventArgs e)
        {
            bool _esito;
            this.Cursor = Cursors.WaitCursor;
            CaricaVariabili();
            this.Cursor = Cursors.Default;
        }

        private void chkParRegistraLetture_CheckedChanged(object sender, EventArgs e)
        {
            if (chkParRegistraLetture.Checked == true)
            {
                ListaValori.Clear();
                InizializzaVistaParametri();
                
            }
        }

        /// <summary>
        /// Carico la lista delle letture per l'analisi corrente
        /// </summary>
        private void InizializzaVistaParametri()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 7, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvwLettureParametri.HeaderUsesThemes = false;
                flvwLettureParametri.HeaderFormatStyle = _stile;
                flvwLettureParametri.UseAlternatingBackColors = true;
                flvwLettureParametri.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvwLettureParametri.AllColumns.Clear();

                flvwLettureParametri.View = View.Details;
                flvwLettureParametri.ShowGroups = false;
                flvwLettureParametri.GridLines = true;

                BrightIdeasSoftware.OLVColumn colLettura = new BrightIdeasSoftware.OLVColumn();
                colLettura.Text = "N.";
                colLettura.AspectName = "strLettura";
                colLettura.Width = 40;
                colLettura.HeaderTextAlign = HorizontalAlignment.Center;
                colLettura.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colLettura);

                BrightIdeasSoftware.OLVColumn colTimeStamp = new BrightIdeasSoftware.OLVColumn();
                colTimeStamp.Text = "Time";
                colTimeStamp.AspectName = "strOraLettura";
                colTimeStamp.Width = 70;
                colTimeStamp.HeaderTextAlign = HorizontalAlignment.Center;
                colTimeStamp.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colTimeStamp);

                BrightIdeasSoftware.OLVColumn colTempoCiclo = new BrightIdeasSoftware.OLVColumn();
                colTempoCiclo.Text = "T.Ciclo";
                colTempoCiclo.AspectName = "strSecondsFromStart";
                colTempoCiclo.Width = 70;
                colTempoCiclo.HeaderTextAlign = HorizontalAlignment.Center;
                colTempoCiclo.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colTempoCiclo);


                BrightIdeasSoftware.OLVColumn colTensioneIst = new BrightIdeasSoftware.OLVColumn();
                colTensioneIst.Text = "V ist";
                colTensioneIst.AspectName = "strTensioneIstantanea";
                colTensioneIst.Width = 60;
                colTensioneIst.HeaderTextAlign = HorizontalAlignment.Center;
                colTensioneIst.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colTensioneIst);


                BrightIdeasSoftware.OLVColumn colCorrIst = new BrightIdeasSoftware.OLVColumn();
                colCorrIst.Text = "A ist";
                colCorrIst.AspectName = "strCorrenteIstantanea";
                colCorrIst.Width = 60;
                colCorrIst.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrIst.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colCorrIst);


                BrightIdeasSoftware.OLVColumn colCorrCaricata = new BrightIdeasSoftware.OLVColumn();
                colCorrCaricata.Text = "Ah car";
                colCorrCaricata.AspectName = "strAhCaricati";
                colCorrCaricata.Width = 60;
                colCorrCaricata.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrCaricata.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colCorrCaricata);




                //-------------------------------------------- 


                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 50;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvwLettureParametri.AllColumns.Add(colRowFiller);

                flvwLettureParametri.RebuildColumns();

                flvwLettureParametri.SetObjects(ListaValori);
                flvwLettureParametri.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }

        private void btnVarFilesearch_Click(object sender, EventArgs e)
        {
            {
                sfdExportDati.Filter = "Comma Separed (*.csv)|*.csv|All files (*.*)|*.*";
                sfdExportDati.ShowDialog();
                txtVarFileCicli.Text = sfdExportDati.FileName;

            }
        }

        private void txtVarGeneraExcel_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "";
                int _ciclo = 0;


                if (txtVarFileCicli.Text != "")
                {
                    filePath = txtVarFileCicli.Text;
                    if (!File.Exists(filePath))
                    {
                        File.Create(filePath).Close();
                    }
                    string delimiter = ";";

                    StringBuilder sb = new StringBuilder();


                    string[][] output = new string[][]
                    {
                         new string[]{"Num Lettura.","Istante","Tempo da Inizio", "V Ist", "A Ist","Ah Caricati" }
                    };

                    int length = output.GetLength(0);
                    sb = new StringBuilder();
                    for (int index = 0; index < length; index++)
                        sb.AppendLine(string.Join(delimiter, output[index]));
                    File.AppendAllText(filePath, sb.ToString());

                    int _elementi = ListaValori.Count;
                    string[] arr;

                    foreach (llVariabili _vac in ListaValori)
                    {

                        output = new string[][]
                          {
                             new string[]{ _vac.strLettura,
                                           _vac.strOraLettura,
                                           _vac.strSecondsFromStart,
                                           _vac.strTensioneIstantanea,
                                           _vac.strCorrenteIstantanea,
                                           _vac.strAhCaricati
                               }

                          };

                        length = output.GetLength(0);
                        sb = new StringBuilder();
                        for (int index = 0; index < length; index++)
                            sb.AppendLine(string.Join(delimiter, output[index]));
                        File.AppendAllText(filePath, sb.ToString());
                        _ciclo++;

                    }
    

                    MessageBox.Show("File generato", "Esportazione dati Corrente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Inserire un nome valido", "Esportazione dati Corrente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }

            catch (Exception Ex)
            {
                Log.Error("btnGeneraCsv_Click: " + Ex.Message);
                MessageBox.Show(Ex.Message, "Esportazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void btnStratTest01_Click(object sender, EventArgs e)
        {
            bool _esito;
            this.Cursor = Cursors.WaitCursor;
            ChiamataProxySig60(36);
            this.Cursor = Cursors.Default;
        }

        private void btnStratTest02_Click(object sender, EventArgs e)
        {
            bool _esito;
            this.Cursor = Cursors.WaitCursor;
            ChiamataProxySig60(2);
            this.Cursor = Cursors.Default;
        }

        private void btnStratTestERR_Click(object sender, EventArgs e)
        {
            bool _esito;
            this.Cursor = Cursors.WaitCursor;
            ChiamataProxySig60(3);
            this.Cursor = Cursors.Default;
        }

        private void btnStratQuery_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            ChiamataProxySig60CSInfo(0xA0);

            this.Cursor = Cursors.Default;
        }

        private void label205_Click(object sender, EventArgs e)
        {

        }

        private void txtStratCurrStepRipetizioni_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnStratSetCharge_Click(object sender, EventArgs e)
        {

        }

        private void btnStratCallAv_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            ChiamataProxySig60CSAvanzamento(0x03);

            this.Cursor = Cursors.Default;
        }

        private void btnFSerVerificaOC_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the Click event of the btnFWFileCCSsearch control.
        /// Apre la finestra gestione file per la ricerca del file CCS (txt) da caricare
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void btnFWFileCCSsearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (sfdImportDati.InitialDirectory == "") sfdImportDati.InitialDirectory = PannelloCharger.Properties.Settings.Default.pathLLFwSource;

                sfdImportDati.Filter = "CCS Generated File (*.hex)|*.hex|All files (*.*)|*.*";
                sfdImportDati.ShowDialog();
                txtFwFileCCS.Text = sfdImportDati.FileName;

                ControllaNomiFilesCCSLL(sfdImportDati.FileName);

                PannelloCharger.Properties.Settings.Default.pathLLFwSource = Path.GetDirectoryName(sfdImportDati.FileName);
            }
            catch (Exception Ex)
            {
                Log.Error("btnGeneraCsv_Click: " + Ex.Message);
            }

        }

        private void btnFWFileSBFsearch_Click(object sender, EventArgs e)
        {
            sfdExportDati.Filter = "LLF LADE Light Firmware File (*.llf)|*.llf|All files (*.*)|*.*";
            sfdExportDati.ShowDialog();
            txtFWFileLLFwr.Text = sfdExportDati.FileName;
            btnFWLanciaTrasmissione.Enabled = false;

        }

        private void btnFWFileCCSLoad_Click(object sender, EventArgs e)
        {
            CaricafileLLCCS();
        }

        private void btnFWFilePubSave_Click(object sender, EventArgs e)
        {
            if (txtFWFileLLFwr.Text != "")
            {
                if (txtFWInFileRev.Text == "")
                {
                    MessageBox.Show("Inserire il numero di release", "Preperazione App", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SalvaFileLLF();
            }
        }

        private void btnFWFileLLFReadSearch_Click(object sender, EventArgs e)
        {
            sfdImportDati.Filter = "LLF LADE Light Firmware File (*.llf)|*.llf|All files (*.*)|*.*";
            sfdImportDati.ShowDialog();
            txtFWFileSBFrd.Text = sfdImportDati.FileName;

            bool _preview = CaricafileLLF();
        }

        private void btnFWFileLLFLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFWFileSBFrd.Text == "") return;

                btnFWLanciaTrasmissione.Enabled = false;
                CaricafileLLF();


                PreparaTrasmissioneFW();
                /*
                bool _esitocella = false;

                _esitocella = ((byte)FirmwareManager.MascheraStato.Blocco1HW & _sb.StatoFirmware.Stato) == (byte)FirmwareManager.MascheraStato.Blocco1HW;
                if (_esitocella == true)
                {
                    txtFWSBFArea.Text = "1";
                }
                else
                {
                    txtFWSBFArea.Text = "2";
                }

                */

                cmbFWSBFArea.SelectedIndex = 0;
            }
            catch (Exception Ex)
            {
                Log.Error("btnFWFileSBFLoad_Click: " + Ex.Message);
            }
        }

        private void btnFWFileSBFLoad_Click(object sender, EventArgs e)
        {
            CaricafileLLCCS();
        }

        private void btnFWLanciaTrasmissione_Click(object sender, EventArgs e)
        {
            try
            {
                bool _esito;
                this.Cursor = Cursors.WaitCursor;
                AggiornaFirmware();
                _cb.VerificaPresenza();
                this.Cursor = Cursors.Default;
            }
            catch
            {
                this.Cursor = Cursors.Default;

            }
        }

        private void btnFWPreparaTrasmissione_Click(object sender, EventArgs e)
        {
            PreparaTrasmissioneFW();
        }

        private void btnFwCaricaStato_Click(object sender, EventArgs e)
        {
            try
            {
                if (_cb.apparatoPresente)
                {
                    VerificaStatoFw();
                }

            }
            catch (Exception Ex)
            {
                Log.Error("btnFwCaricaStato_Click: " + Ex.Message);
            }
        }

        private void btnFwSwitchArea_Click(object sender, EventArgs e)
        {

        }

        private void rbtMemAreaLibera_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtMemAreaLibera.Checked)
            {
                txtMemCFStartAdd.Text = "0";
                txtMemCFBlocchi.Text = "1";
            }
        }

        private void rbtMemAreaApp1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtMemAreaApp1.Checked)
            {
                txtMemCFStartAdd.Text = "1C0000";
                txtMemCFBlocchi.Text = "32";
            }
        }

        private void rbtMemAreaApp2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtMemAreaApp2.Checked)
            {
                txtMemCFStartAdd.Text = "1E0000";
                txtMemCFBlocchi.Text = "32";
            }
        }

        private void btnMemCFExec_Click(object sender, EventArgs e)
        {
            try
            {
                uint _StartAddr;
                int _bloccoCorrente;
                ushort _NumBlocchi;
                bool _esito;

                if (chkMemHex.Checked)
                {
                    if (uint.TryParse(txtMemCFStartAdd.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true) return;
                }
                else
                {

                    if (uint.TryParse(txtMemCFStartAdd.Text, out _StartAddr) != true) return;
                }


                _NumBlocchi = 0;
                if (ushort.TryParse(txtMemCFBlocchi.Text, out _NumBlocchi) != true) return;


                if (_NumBlocchi > 0)
                {

                    for (int _cicloBlocchi = 0; _cicloBlocchi < _NumBlocchi; _cicloBlocchi++)
                    {
                        _bloccoCorrente = _cicloBlocchi + 1;
                        _esito = _cb.CancellaBlocco4K(_StartAddr);
                        if (!_esito)
                        {
                            MessageBox.Show("Cancellazione del blocco " + _bloccoCorrente.ToString() + " non riuscita", "Cancellazione dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            _StartAddr += 0x1000;
                            txtMemCFStartAdd.Text = _StartAddr.ToString("X6");
                            txtMemCFBlocchi.Text = _bloccoCorrente.ToString();
                            Application.DoEvents();
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Inserire un numero di blocchi valido", "Esportazione dati Corrente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }




            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }


        }

        private void btnFwCheckArea_Click(object sender, EventArgs e)
        {

        }

        private void btnFwSwitchBL_Click(object sender, EventArgs e)
        {
            try
            {

                // reset to bl
                bool _esito = SwitchAreaBl("", true);
                VerificaStatoFw();
            }
            catch (Exception Ex)
            {
                Log.Error("btnFwSwitchBL_Click: " + Ex.Message);
            }
        }

        private void btnFwSwitchArea1_Click(object sender, EventArgs e)
        {
            try
            {

                // reset to bl
                bool _esito = SwitchAreaFw("", true, 1);
                VerificaStatoFw();
            }
            catch (Exception Ex)
            {
                Log.Error("btnFwSwitchBL_Click: " + Ex.Message);
            }
        }

        private void btnFwSwitchArea2_Click(object sender, EventArgs e)
        {
            try
            {

                // reset to bl
                bool _esito = SwitchAreaFw("", true, 2);
                VerificaStatoFw();
            }
            catch (Exception Ex)
            {
                Log.Error("btnFwSwitchBL_Click: " + Ex.Message);
            }
        }

        private void cmdMemRead_Click(object sender, EventArgs e)
        {
            try
            {
                uint _StartAddr;
                ushort _NumByte;
                bool _esito;

                if (chkMemHex.Checked)
                {
                    if (uint.TryParse(txtMemAddrR.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true) return;
                }
                else
                {

                    if (uint.TryParse(txtMemAddrR.Text, out _StartAddr) != true) return;
                }



                if (ushort.TryParse(txtMemLenR.Text, out _NumByte) != true) return;

                if (_NumByte < 1) _NumByte = 1;
                if (_NumByte > 242) _NumByte = 242;
                txtMemLenR.Text = _NumByte.ToString();

                if (_StartAddr < 0) _StartAddr = 0;
                if (chkMemHex.Checked)
                    txtMemAddrR.Text = _StartAddr.ToString("X6");
                else
                    txtMemAddrR.Text = _StartAddr.ToString();

                txtMemDataGrid.Text = "";
                _esito = LeggiBloccoMemoria(_StartAddr, _NumByte);


            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }

        }

        public bool LeggiBloccoMemoria(uint StartAddr, ushort NumByte)
        {
            try
            {

                byte[] _Dati;
                bool _esito;

                txtMemDataGrid.Text = "";
                _Dati = new byte[NumByte];
                _esito = _cb.LeggiBloccoMemoria(StartAddr, NumByte, out _Dati);

                if (_esito == true)
                {


                    string _risposta = "";
                    int _colonne = 0;
                    for (int _i = 0; _i < _Dati.Length; _i++)
                    {
                        _risposta += _Dati[_i].ToString("X2") + " ";
                        _colonne += 1;
                        if (_colonne > 0 && (_colonne % 4) == 0) _risposta += "  ";
                        if (_colonne > 15)
                        {
                            _risposta += "\r\n";
                            _colonne = 0;

                        }
                    }
                    txtMemDataGrid.Text = _risposta;

                }
                else
                {
                    txtMemDataGrid.Text = "Lettura Fallita";
                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LeggiBloccoMemoria: " + Ex.Message);
                return false;
            }

        }

        public bool CaricaStatoAreaFw(byte IdArea, byte StatoFirmware)
        {
            bool _esito = false;
            byte[] _bufferDati = new byte[64];
            FirmwareLLManager _tempFW = new FirmwareLLManager();
            FirmwareLLManager.ExitCode _esitoFW = FirmwareLLManager.ExitCode.ErroreGenerico;
            uint _area;

            try
            {

                Log.Info("Lettura area FW 1 ");
                if (IdArea == 1)
                {

                    txtFwRevA1State.Text = "KO";
                    txtFwRevA1State.ForeColor = Color.Red;
                    txtFwRevA1RevFw.Text = "";
                    txtFwRevA1RilFw.Text = "";
                    txtFWRevA1Addr1.Text = "";
                    txtFWRevA1Addr2.Text = "";
                    txtFWRevA1Addr3.Text = "";
                    txtFWRevA1Addr4.Text = "";
                    txtFWRevA1Addr5.Text = "";
                    txtFwRevA1Size.Text = "";
                    txtFwRevA1MsgSize.Text = "";
                    _area = 0x1C0000;
                }
                else
                {
                    txtFwRevA2State.Text = "KO";
                    txtFwRevA2State.ForeColor = Color.Red;
                    txtFwRevA2RevFw.Text = "";
                    txtFwRevA2RilFw.Text = "";
                    txtFWRevA2Addr1.Text = "";
                    txtFWRevA2Addr2.Text = "";
                    txtFWRevA2Addr3.Text = "";
                    txtFWRevA2Addr4.Text = "";
                    txtFWRevA2Addr5.Text = "";
                    txtFwRevA2Size.Text = "";
                    txtFwRevA2MsgSize.Text = "";
                    _area = 0x1E0000;

                }


                _esito = _cb.LeggiBloccoMemoria(_area, 64, out _bufferDati);


                if (_esito)
                {
                    _esitoFW = _tempFW.AnalizzaArrayTestata(_bufferDati);
                    if (_esitoFW == FirmwareLLManager.ExitCode.OK && _tempFW.FirmwareBlock.TestataOK)
                    {
                        if (IdArea == 1)
                        {
                            txtFwRevA1State.Text = "OK";
                            txtFwRevA1State.ForeColor = Color.Black;
                            txtFwRevA1RevFw.Text = _tempFW.FirmwareBlock.Release;
                            txtFwRevA1RilFw.Text = _tempFW.FirmwareBlock.ReleaseDisplay;

                            txtFwRevA1Size.Text = _tempFW.FirmwareBlock.NumSezioni.ToString();
                            txtFwRevA1MsgSize.Text = _tempFW.FirmwareBlock.LenPkt.ToString("X2");


                            txtFWRevA1Addr1.Text = _tempFW.FirmwareBlock.AddrSez1.ToString("X8") + "\n" + _tempFW.FirmwareBlock.LenSez1.ToString("X8");
                            txtFWRevA1Addr2.Text = _tempFW.FirmwareBlock.AddrSez2.ToString("X8") + "\n" + _tempFW.FirmwareBlock.LenSez2.ToString("X8");
                            txtFWRevA1Addr3.Text = _tempFW.FirmwareBlock.AddrSez3.ToString("X8") + "\n" + _tempFW.FirmwareBlock.LenSez3.ToString("X8");
                            txtFWRevA1Addr4.Text = _tempFW.FirmwareBlock.AddrSez4.ToString("X8") + "\n" + _tempFW.FirmwareBlock.LenSez4.ToString("X8");
                            txtFWRevA1Addr5.Text = _tempFW.FirmwareBlock.AddrSez5.ToString("X8") + "\n" + _tempFW.FirmwareBlock.LenSez5.ToString("X8");

                        }
                        else
                        {
                            txtFwRevA2State.Text = "OK";
                            txtFwRevA2State.ForeColor = Color.Black;
                            txtFwRevA2RevFw.Text = _tempFW.FirmwareBlock.Release;
                            txtFwRevA2RilFw.Text = _tempFW.FirmwareBlock.ReleaseDisplay;

                            txtFwRevA2Size.Text = _tempFW.FirmwareBlock.NumSezioni.ToString();
                            txtFwRevA2MsgSize.Text = _tempFW.FirmwareBlock.LenPkt.ToString("X2");

                            txtFWRevA2Addr1.Text = _tempFW.FirmwareBlock.AddrSez1.ToString("X8") + "\n" + _tempFW.FirmwareBlock.LenSez1.ToString("X8");
                            txtFWRevA2Addr2.Text = _tempFW.FirmwareBlock.AddrSez2.ToString("X8") + "\n" + _tempFW.FirmwareBlock.LenSez2.ToString("X8");
                            txtFWRevA2Addr3.Text = _tempFW.FirmwareBlock.AddrSez3.ToString("X8") + "\n" + _tempFW.FirmwareBlock.LenSez3.ToString("X8");
                            txtFWRevA2Addr4.Text = _tempFW.FirmwareBlock.AddrSez4.ToString("X8") + "\n" + _tempFW.FirmwareBlock.LenSez4.ToString("X8");
                            txtFWRevA2Addr5.Text = _tempFW.FirmwareBlock.AddrSez5.ToString("X8") + "\n" + _tempFW.FirmwareBlock.LenSez5.ToString("X8");
                        }
                    }

                }

                return _esito;

            }
            catch
            {
                return _esito;
            }
        }

        private void label103_Click(object sender, EventArgs e)
        {

        }

        private void txtFwStatoSA2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
               _cb.ControllaStatoAreaFW(2);
            }
            catch (Exception Ex)
            {
                Log.Error("txtFwStatoSA2_DoubleClick: " + Ex.Message);
                
            }
        }

 
        private void grbInitDatiBase_Enter(object sender, EventArgs e)
        {

        }

        private void btnScriviInizializzazione_Click(object sender, EventArgs e)
        {
            SalvaInizializzazione();
        }

        public bool SalvaInizializzazione()
        {
            try
            {
                if (_cb.ParametriApparato == null)
                {
                    _cb.ParametriApparato = new llParametriApparato();
                }

                _cb.ParametriApparato.llParApp.ProduttoreApparato = txtInitManufactured.Text;
                _cb.ParametriApparato.llParApp.NomeApparato = txtInitProductId.Text;

                uint TmpInt;
                bool _esito;
                byte[] tempVal;

                TmpInt = 0xFFFFFFFF;
                if (uint.TryParse(txtInitSerialeApparato.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out TmpInt))
                {
                    _cb.ParametriApparato.llParApp.SerialeApparato = TmpInt;
                }

                // Tipo Apparato
                _cb.ParametriApparato.llParApp.TipoApparato = (byte)cmbInitTipoApparato.SelectedValue;

                // Data
                byte[] dataInit = FunzioniMR.toArrayDataTS(txtInitDataInizializ.Text);
                uint DataUint = dataInit[0];
                DataUint = (DataUint << 8) + dataInit[1];
                DataUint = (DataUint << 8) + dataInit[2];
                _cb.ParametriApparato.llParApp.DataSetupApparato = DataUint;

                // Seriale scheda ZVT
                if (txtInitNumSerZVT.Text.Trim() != "")
                {
                    tempVal = FunzioniComuni.HexStringToArray(txtInitNumSerZVT.Text, 8);
                }
                else
                {
                    tempVal = new byte[8] { 0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF};
                }
                _cb.ParametriApparato.llParApp.SerialeZVT = tempVal;

                // Rev HW ZVT
                _cb.ParametriApparato.llParApp.HardwareZVT = txtInitRevHwZVT.Text;


                // Seriale scheda PFC
                if (txtInitNumSerPFC.Text.Trim() != "")
                {
                    tempVal = FunzioniComuni.HexStringToArray(txtInitNumSerPFC.Text, 8);
                }
                else
                {
                    tempVal = new byte[8] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                }
                _cb.ParametriApparato.llParApp.SerialePFC = tempVal;

                // Rev SW PFC
                _cb.ParametriApparato.llParApp.SoftwarePFC = txtInitRevFwPFC.Text;
                // Rev HW PFC
                _cb.ParametriApparato.llParApp.HardwarePFC = txtInitRevHwPFC.Text;



                // Seriale scheda DISP
                if (txtInitNumSerDISP.Text.Trim() != "")
                {
                    tempVal = FunzioniComuni.HexStringToArray(txtInitNumSerDISP.Text, 8);
                }
                else
                {
                    tempVal = new byte[8] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
                }
                _cb.ParametriApparato.llParApp.SerialeDISP = tempVal;

                // Rev SW DISP
                _cb.ParametriApparato.llParApp.SoftwareDISP = txtInitRevFwDISP.Text;
                // Rev HW DISP
                _cb.ParametriApparato.llParApp.HardwareDisp = txtInitRevHwDISP.Text;

                _cb.ParametriApparato.llParApp.IdApparato = txtInitIDApparato.Text;




                _esito = _cb.ScriviParametriApparato();

                return _esito;

            }
            catch (Exception Ex)
            {
                Log.Error("SalvaInizializzazione: " + Ex.Message);
                return false;

            }
        }

        private void cmdMemWrite_Click(object sender, EventArgs e)
        {

        }

        public uint CalcolaMatricola()
        {
            try
            {

                bool _esito;
                byte _annoMatr = 18;

                uint matricola = 0;

                if (byte.TryParse(txtInitAnnoMatricola.Text, out _annoMatr))
                {
                    _annoMatr = (byte)(_annoMatr & 0x3F);
                    _annoMatr = (byte)(_annoMatr << 2);

                }

                matricola = (uint)( _annoMatr << 16 );

                uint _numMatr = 0;

                if (uint.TryParse(txtInitNumeroMatricola.Text, out _numMatr))
                {
                    _numMatr = (uint)(_numMatr & 0x0003FFFF);

                }

                        matricola += _numMatr;



                txtInitSerialeApparato.Text = matricola.ToString("X6");
                string _tempMatricola = "LL" + txtInitSerialeApparato.Text;
                byte[] _arrayMatr = FunzioniComuni.StringToArray(_tempMatricola, 8, 0);

                txtInitIDApparato.Text = _tempMatricola; // FunzioniComuni.HexdumpArray(_arrayMatr,false);

                return matricola;

            }

            catch (Exception Ex)
            {
                Log.Error("CalcolaMatricola: " + Ex.Message);
                return 0;

            }

        }

        private void txtInitAnnoMatricola_Leave(object sender, EventArgs e)
        {
            CalcolaMatricola();
        }

        private void txtInitNumeroMatricola_Leave(object sender, EventArgs e)
        {
            CalcolaMatricola();
        }

        private void txtInitNumSerZVT_Leave(object sender, EventArgs e)
        {
            if (txtInitNumSerZVT.Text.Trim() != "")
            {
                byte[] tempVal;
                tempVal = FunzioniComuni.HexStringToArray(txtInitNumSerZVT.Text, 8);
                txtInitNumSerZVT.Text = FunzioniComuni.HexdumpArray(tempVal);
            }
            else
            {
                txtInitNumSerZVT.Text = "";
            }
        }

        private void txtInitNumSerPFC_Leave(object sender, EventArgs e)
        {
            if (txtInitNumSerPFC.Text.Trim() != "")
            {
                byte[] tempVal;
                tempVal = FunzioniComuni.HexStringToArray(txtInitNumSerPFC.Text, 8);
                txtInitNumSerPFC.Text = FunzioniComuni.HexdumpArray(tempVal);
            }
            else
            {
                txtInitNumSerPFC.Text = "";
            }
        }

        private void txtInitNumSerDISP_Leave(object sender, EventArgs e)
        {
            if (txtInitNumSerDISP.Text.Trim() != "")
            {
                byte[] tempVal;
                tempVal = FunzioniComuni.HexStringToArray(txtInitNumSerDISP.Text, 8);
                txtInitNumSerDISP.Text = FunzioniComuni.HexdumpArray(tempVal);
            }
            else
            {
                txtInitNumSerDISP.Text = "";
            }

        }

        private void tabInizializzazione_Click(object sender, EventArgs e)
        {

        }

        private void btnCaricaInizializzazione_Click(object sender, EventArgs e)
        {
            LeggiInizializzazione();
        }

        public bool LeggiInizializzazione()
        {
            try
            {
                bool _esito;

                txtInitManufactured.Text = "";
                txtInitProductId.Text = "";
                txtInitDataInizializ.Text = "";
                txtInitAnnoMatricola.Text = "";
                txtInitNumeroMatricola.Text = "";
                txtInitSerialeApparato.Text = "";
                txtInitIDApparato.Text = "";

                cmbInitTipoApparato.SelectedValue = 0;

                txtInitNumLottoZVT.Text = "";
                txtInitNumLottoPFC.Text = "";
                txtInitNumSerZVT.Text = "";
                txtInitRevHwZVT.Text = "";

                txtInitNumLottoPFC.Text = "";
                txtInitNumSerPFC.Text = "";
                txtInitRevHwPFC.Text = "";
                txtInitRevFwPFC.Text = "";

                txtInitNumSerDISP.Text = "";
                txtInitRevHwDISP.Text = "";
                txtInitRevFwDISP.Text = "";


                _esito = _cb.LeggiParametriApparato();

                if (_esito)
                {
                    txtInitManufactured.Text = _cb.ParametriApparato.llParApp.ProduttoreApparato;
                    txtInitProductId.Text = _cb.ParametriApparato.llParApp.NomeApparato;

                    if( _cb.ParametriApparato.llParApp.IdApparato != "????????" && _cb.ParametriApparato.llParApp.IdApparato !="")
                    {
                        txtInitDataInizializ.Text = FunzioniMR.StringaDataTS(_cb.ParametriApparato.llParApp.DataSetupApparato);
                        txtInitAnnoMatricola.Text = _cb.ParametriApparato.llParApp.AnnoCodice.ToString("00");
                        txtInitNumeroMatricola.Text = _cb.ParametriApparato.llParApp.ProgressivoCodice.ToString("000000");
                        txtInitSerialeApparato.Text = _cb.ParametriApparato.llParApp.SerialeApparato.ToString("x6");
                        txtInitIDApparato.Text = _cb.ParametriApparato.llParApp.IdApparato;

                        cmbInitTipoApparato.SelectedValue = _cb.ParametriApparato.llParApp.TipoApparato;

                        txtInitNumLottoZVT.Text = _cb.ParametriApparato.llParApp.IdLottoZVT;
                        txtInitNumSerZVT.Text = FunzioniComuni.HexdumpArray(_cb.ParametriApparato.llParApp.SerialeZVT);
                        txtInitRevHwZVT.Text = _cb.ParametriApparato.llParApp.HardwareZVT;

                        txtInitNumLottoPFC.Text = _cb.ParametriApparato.llParApp.IdLottoPFC;
                        txtInitNumSerPFC.Text = FunzioniComuni.HexdumpArray(_cb.ParametriApparato.llParApp.SerialePFC);
                        txtInitRevHwPFC.Text = _cb.ParametriApparato.llParApp.HardwarePFC;
                        txtInitRevFwPFC.Text = _cb.ParametriApparato.llParApp.SoftwarePFC;

                        txtInitNumSerDISP.Text = FunzioniComuni.HexdumpArray(_cb.ParametriApparato.llParApp.SerialeDISP);
                        txtInitRevHwDISP.Text = _cb.ParametriApparato.llParApp.HardwareDisp;
                        txtInitRevFwDISP.Text = _cb.ParametriApparato.llParApp.SoftwareDISP;
                    }


                    txtInitMaxBrevi.Text = _cb.ParametriApparato.llParApp.MaxRecordBrevi.ToString();
                    txtInitMaxLunghi.Text = _cb.ParametriApparato.llParApp.MaxRecordCarica.ToString();
                    txtInitMaxProg.Text = _cb.ParametriApparato.llParApp.MaxProgrammazioni.ToString();
                    txtInitModelloMemoria.Text = _cb.ParametriApparato.llParApp.ModelloMemoria.ToString();

                }

                return _esito;

            }
            catch (Exception Ex)
            {
                Log.Error("SalvaInizializzazione: " + Ex.Message);
                return false;

            }
        }


        private void txtInitNumLottoZVT_Leave(object sender, EventArgs e)
        {
            try
            {

                txtInitNumSerZVT.Text = "";


                // se cambiato
                if (FunzioniMR.VerificaStringaLottoZVT(txtInitNumLottoZVT.Text))
                {
                    byte[] _tempByte;
                    txtInitNumLottoZVT.Text = txtInitNumLottoZVT.Text.ToUpper();

                    _tempByte = FunzioniMR.CodificaStringaLottoZVT(txtInitNumLottoZVT.Text);

                    if (_tempByte != null)
                    {
                        txtInitNumSerZVT.Text = FunzioniComuni.HexdumpArray(_tempByte,false) + "0A0A";
                    }


                }
                else
                {
                    txtInitNumSerZVT.Text = "";
                }
            }
            catch (Exception Ex)
            {
                Log.Error("txtInitNumLottoZVT_Leave: " + Ex.Message);

            }
        }

        private void chkPaAttivaEqual_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkPaAttivaEqual.Checked)
                {
                    txtPaEqualAttesa.Enabled = true;
                    txtPaEqualAttesa.Text = "48";
                    txtPaEqualNumPulse.Enabled = true;
                    txtPaEqualNumPulse.Text = "12";

                }
                else
                {
                    txtPaEqualAttesa.Enabled = false;
                    txtPaEqualAttesa.Text = "";
                    txtPaEqualNumPulse.Enabled = false;
                    txtPaEqualNumPulse.Text = "";

                }
            }
            catch (Exception Ex)
            {
                Log.Error("chkPaAttivaEqual_CheckedChanged: " + Ex.Message);

            }

        }

        private void chkPaAttivaRiarmoBms_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkPaAttivaRiarmoBms.Checked)
                {
                    txtPaBMSTempoAttesa.Enabled = true;
                    txtPaBMSTempoAttesa.Text = "240";
                    txtPaBMSTempoErogazione.Enabled = true;
                    txtPaBMSTempoErogazione.Text = "5";

                }
                else
                {
                    txtPaBMSTempoAttesa.Enabled = false;
                    txtPaBMSTempoAttesa.Text = "";
                    txtPaBMSTempoErogazione.Enabled = false;
                    txtPaBMSTempoErogazione.Text = "";

                }
            }
            catch (Exception Ex)
            {
                Log.Error("chkPaAttivaEqual_CheckedChanged: " + Ex.Message);

            }
        }

        private void tabInizializzazione_Enter(object sender, EventArgs e)
        {
            LeggiInizializzazione();
        }

        private void MascheraTabPages(int MaskLevel)
        {
            try
            {
                // Mask 0 Tutto acceso
                // Mask 1 lascio solo init


                foreach ( TabPage _pag in tabCaricaBatterie.TabPages)
                {
                    if (MaskLevel == 1 )
                    {
                        if(_pag.Name != "tabInizializzazione")
                        {
                            tabCaricaBatterie.TabPages.Remove(_pag);
                        }
                    }


                }



            }
            catch (Exception Ex)
            {
                Log.Error("chkPaAttivaEqual_CheckedChanged: " + Ex.Message);
            }


        }

        private void cmbPaTipoBatteria_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                byte TipoBatt;
                sbTipoBatteria TempBatt = (sbTipoBatteria)cmbPaTipoBatteria.SelectedItem;

                if (TempBatt == null)
                {

                }
                else
                {
                    // Carico i profili in base al tipo
                    TipoBatt = (byte)TempBatt.BatteryTypeId;

                    var Listatemp = from p in _cb.ProfiliCarica
                                    join pt in _cb.ProfiloTipoBatt on p.IdProfiloCaricaLL equals pt.IdProfiloCaricaLL
                                    where pt.BatteryTypeId == TipoBatt
                                    select p;

                    ProfiliCarica = new List<_llProfiloCarica>();

                    foreach (var pc in Listatemp)
                    {
                        ProfiliCarica.Add((_llProfiloCarica)pc);
                    }
                    cmbPaProfilo.SelectedItem = null;

                    cmbPaProfilo.DataSource = ProfiliCarica;
                    cmbPaProfilo.ValueMember = "IdProfiloCaricaLL";
                    cmbPaProfilo.DisplayMember = "NomeProfilo";

                    // Carico le tensioni in base al tipo

                    _llParametriApparato TipoLL = _cb.ParametriApparato.llParApp;

                    if (TempBatt.TensioniFisse == 0 || TipoLL == null)
                    {
                        // Textbox
                        txtPaTensione.Visible = true;
                        cmbPaTensione.Visible = false;

                    }
                    else
                    {
                        txtPaTensione.Visible = false;
                        cmbPaTensione.Visible = true;

                        byte TipoApp = TipoLL.TipoApparato;

                        var Listatens = from t in _cb.TensioniBatteria
                                        join tm in _cb.TensioniModello on t.IdTensione equals tm.IdTensione
                                        where tm.IdModelloLL == TipoApp
                                        select t;

                        TensioniBatteria = new List<llTensioneBatteria>() ;
                        foreach (var t in Listatens)
                        {
                            TensioniBatteria.Add((llTensioneBatteria)t);
                        }

                        cmbPaTensione.SelectedItem = null;

                        cmbPaTensione.DataSource = TensioniBatteria;
                        cmbPaTensione.ValueMember = "IdTensione";
                        cmbPaTensione.DisplayMember = "Descrizione";

                    }

                }
               
            }
            catch (Exception Ex)
            {
                Log.Error("cmbProfiloTipoBatteria_SelectedIndexChanged: " + Ex.Message);
            }
        }

        private void cmbPaProfilo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                byte TipoProf;
                byte ModoProf;

                if (cmbPaProfilo.SelectedItem == null)
                {
                    cmbPaDurataCarica.SelectedItem = null;
                }
                else
                {
                    TipoProf = (byte)((_llProfiloCarica)cmbPaProfilo.SelectedItem).IdProfiloCaricaLL;

                    var Listatemp = from p in _cb.DurateCarica
                                    join pt in _cb.DurateProfilo on p.IdDurataCaricaLL equals pt.IdDurataCaricaLL
                                    where pt.IdProfiloCaricaLL == TipoProf
                                    select p;

                    DurateCarica = new List<llDurataCarica>();

                    foreach (var pc in Listatemp)
                    {
                        DurateCarica.Add((llDurataCarica)pc);
                    }


                    cmbPaDurataCarica.DataSource = DurateCarica;
                    cmbPaDurataCarica.ValueMember = "IdDurataCaricaLL";
                    cmbPaDurataCarica.DisplayMember = "Descrizione";

                    ModoProf = (byte)((_llProfiloCarica)cmbPaProfilo.SelectedItem).AttivaEqual;
                    switch (ModoProf)
                    {
                        case 0x00:
                            chkPaAttivaEqual.Checked = false;
                            chkPaAttivaEqual.Enabled = false;
                            break;
                        case 0xF0:
                            chkPaAttivaEqual.Checked = false;
                            chkPaAttivaEqual.Enabled = true;
                            break;
                        case 0xFF:
                            chkPaAttivaEqual.Checked = true;
                            chkPaAttivaEqual.Enabled = false;
                            break;
                        default:
                            chkPaAttivaEqual.Checked = false;
                            chkPaAttivaEqual.Enabled = false;
                            break;

                    }


                    ModoProf = (byte)((_llProfiloCarica)cmbPaProfilo.SelectedItem).AttivaRiarmoPulse;
                    switch (ModoProf)
                    {
                        case 0x00:
                            chkPaAttivaRiarmoBms.Checked = false;
                            chkPaAttivaRiarmoBms.Enabled = false;
                            break;
                        case 0xF0:
                            chkPaAttivaRiarmoBms.Checked = false;
                            chkPaAttivaRiarmoBms.Enabled = true;
                            break;
                        case 0xFF:
                            chkPaAttivaRiarmoBms.Checked = true;
                            chkPaAttivaRiarmoBms.Enabled = false;
                            break;
                        default:
                            chkPaAttivaRiarmoBms.Checked = false;
                            chkPaAttivaRiarmoBms.Enabled = false;
                            break;

                    }



                }

            }
            catch (Exception Ex)
            {
                Log.Error("cmbPaDurataCarica_SelectedIndexChanged: " + Ex.Message);
            }


        }

        private void cmbPaDurataCarica_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                byte DurataF2;
                byte TipoBatt;
                sbTipoBatteria TempBatt = (sbTipoBatteria)cmbPaTipoBatteria.SelectedItem;

                if (TempBatt == null)
                {
                    TipoBatt = 0;
                }
                else
                {
                    TipoBatt = TempBatt.BatteryTypeId;
                }



                    if (cmbPaDurataCarica.SelectedItem == null)
                {
                    txtPaTempoT2Min.Text = "99";
                    //MessageBox.Show(_parametri.lastError, "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                   
                    DurataF2 = (byte)((llDurataCarica)cmbPaDurataCarica.SelectedItem).DurataFaseDue(TipoBatt);
                    txtPaTempoT2Min.Text = DurataF2.ToString();
                }


                   //MessageBox.Show(_parametri.lastError, "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);



                /*
                byte TipoProf;

                TipoProf = (byte)((_llProfiloCarica)cmbPaProfilo.SelectedItem).IdProfiloCaricaLL;

                var Listatemp = from p in _cb.DurateCarica
                                join pt in _cb.DurateProfilo on p.IdDurataCaricaLL equals pt.IdDurataCaricaLL
                                where pt.IdProfiloCaricaLL == TipoProf
                                select p;

                DurateCarica = new List<llDurataCarica>();

                foreach (var pc in Listatemp)
                {
                    DurateCarica.Add((llDurataCarica)pc);
                }


                cmbPaDurataCarica.DataSource = DurateCarica;
                cmbPaDurataCarica.ValueMember = "IdDurataCaricaLL";
                cmbPaDurataCarica.DisplayMember = "Descrizione";
                */

            }
            catch (Exception Ex)
            {
                Log.Error("cmbPaDurataCarica_SelectedIndexChanged: " + Ex.Message);
            }

        }

        private void cmbPaTensione_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Copio il valore nella textbox collegata
            if (cmbPaTensione.SelectedItem != null)
            {
                txtPaTensione.Text = ((llTensioneBatteria)cmbPaTensione.SelectedItem).Descrizione;
            }
            else
            {
                txtPaTensione.Text = "";
            }

        }


        private void InizializzaVistaProgrammazioni()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flwPaListaConfigurazioni.HeaderUsesThemes = false;
                flwPaListaConfigurazioni.HeaderFormatStyle = _stile;
                flwPaListaConfigurazioni.UseAlternatingBackColors = true;
                flwPaListaConfigurazioni.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flwPaListaConfigurazioni.AllColumns.Clear();

                flwPaListaConfigurazioni.View = View.Details;
                flwPaListaConfigurazioni.ShowGroups = false;
                flwPaListaConfigurazioni.GridLines = true;

                BrightIdeasSoftware.OLVColumn colIdConfig = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Num",
                    AspectName = "strIdProgramma",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colIdConfig);

                BrightIdeasSoftware.OLVColumn colRowTipe = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Type",
                    AspectName = "strTipoRecord",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colRowTipe);


                BrightIdeasSoftware.OLVColumn colRowOpt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Opt",
                    AspectName = "",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colRowOpt);


                BrightIdeasSoftware.OLVColumn colRowReadonly = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "M",
                    ToolTipText = "Dati Modificabili",
                    AspectName = "",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colRowReadonly);


                BrightIdeasSoftware.OLVColumn colRowNomeSetup = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Nome",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "",
                    Width = 200,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colRowNomeSetup);

                BrightIdeasSoftware.OLVColumn colRowBattType = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Tipo",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "",
                    Width = 120,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colRowBattType);

                BrightIdeasSoftware.OLVColumn colRowBattVNom = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "V",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "",
                    Width = 80,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colRowBattVNom);

                BrightIdeasSoftware.OLVColumn colRowBattAhNom = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Ah",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "",
                    Width = 80,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colRowBattAhNom);

                /*
                
                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvwProgrammiCarica.AllColumns.Add(colRowFiller);
                */
                flwPaListaConfigurazioni.RebuildColumns();
                flwPaListaConfigurazioni.SetObjects(_cb.ProgrammiDefiniti);
                flwPaListaConfigurazioni.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }

        private void btnPaCaricaListaProfili_Click(object sender, EventArgs e)
        {
            try
            {

                
                CaricaListaProgrammazioni();
                flwPaListaConfigurazioni.Refresh();
                //flwPaListaConfigurazioni.SetObjects(_cb.ProgrammiDefiniti);
                //flwPaListaConfigurazioni.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("btnPaCaricaListaProfili_Click: " + Ex.Message);
            }
        }

        private void btnCaricaContatori_Click(object sender, EventArgs e)
        {
            try
            {

                CaricaAreaContatori();

            }
            catch (Exception Ex)
            {
                Log.Error("btnPaCaricaListaProfili_Click: " + Ex.Message);
            }

        }

        private void btnCicliVuotaLista_Click(object sender, EventArgs e)
        {
            VuotaListaCariche();
        }

        private void btnCicliCaricaLista_Click(object sender, EventArgs e)
        {
            try
            {
                uint _StartAddr;
                ushort _NumByte;
                bool _esito;

                if (uint.TryParse(txtCicliAddrPrmo.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true)
                {
                    _StartAddr = 0x1B3000;
                }

                txtCicliAddrPrmo.Text = _StartAddr.ToString("X6");

                if ( txtCicliNumRecord.Text =="-1")
                {
                    // tutti i record
                    _NumByte = 0;
                }
                else
                {
                    if (ushort.TryParse(txtCicliNumRecord.Text, out _NumByte) != true)
                    {
                        _NumByte = 0;
                    }
                    else
                    {
                        if (_NumByte < 1) _NumByte = 0;
                       // if (_NumByte > 242) _NumByte = 242;
                    }

                }
                txtCicliNumRecord.Text = _NumByte.ToString();
                CaricaListaCariche(_StartAddr, _NumByte);


            }
            catch (Exception Ex)
            {
                Log.Error("btnCicliCaricaLista_Click: " + Ex.Message);
            }
        }

        private void frmCaricabatterie_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // Chiudo la connessione
                if(_cb.StopComunicazione())
                {
                    _cb.chiudiPorta();

                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmCaricabatterie_FormClosed: " + Ex.Message);
            }
        }
    }

}
