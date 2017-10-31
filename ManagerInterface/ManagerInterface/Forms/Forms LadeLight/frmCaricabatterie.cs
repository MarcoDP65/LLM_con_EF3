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


        public frmCaricabatterie(ref parametriSistema _par, bool CaricaDati, string IdApparato, LogicheBase Logiche, bool SerialeCollegata, bool AutoUpdate)
        {
            bool _esito;
            try
            {

                _parametri = _par;
                InitializeComponent();
                ResizeRedraw = true;

                _logiche = Logiche;
                attivaCaricabatterie(ref _par, SerialeCollegata);
                InizializzaScheda();
                this.Cursor = Cursors.Arrow;
/*                _msg = new SerialMessage();
                _cb = new CaricaBatteria(ref _parametri);
                //_sb = new UnitaSpyBatt(ref _parametri, _logiche.dbDati.connessione);
                string _idCorrente = IdApparato;
                abilitaSalvataggi(false);

                CaricaTestata(IdApparato, Logiche, SerialeCollegata);
*/

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


        public void attivaCaricabatterie(ref parametriSistema _par,bool CaricaDati)
        {
            bool _esito;
            try
            {
                _parametri = _par;
                InitializeComponent();
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
                _esito = _cb.VerificaPresenza();
                if (_esito)
                {

                    txtMatricola.Text = _cb.Intestazione.Matricola.ToString();
                    txtProgressivo.Text = _cb.Intestazione.Progressivo.ToString();
                    txtDataPrimaInst.Text = _cb.Intestazione.PrimaInstallazione;
                    txtModello.Text = _cb.Intestazione.modello;
                    txtTensione.Text = _cb.Intestazione.tensioneNominale.ToString();
                    txtCorrente.Text = _cb.Intestazione.correnteNominale.ToString();
                    txtFwCb.Text = _cb.Intestazione.revSoftware;
                    txtFwDisp.Text = _cb.Intestazione.revDisplay;
                    txtNumCicli.Text = "2";
                    _cbCollegato = true;
                    _apparatoPresente = _esito;
                    return;
                    /*
                    _esito = _cb.CaricaCicli();
                    if (_esito)
                    {
                        txtNumCicli.Text = _cb.CicliPresenti.NumCicli.ToString();
                    }
                    else 
                    { 
                        txtNumCicli.Text = "2";
                    }

                    */

                }


                

                //            ComPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(port_DataReceived);

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
            cmbPaProfilo.Items.Clear();
            cmbPaProfilo.Items.Add("N.D.");
            cmbPaProfilo.Items.Add("IWa");
            cmbPaProfilo.Items.Add("IU");
            cmbPaProfilo.Items.Add("IUIa");
            cmbPaProfilo.Items.Add("IWa Pb13");
            cmbPaProfilo.Items.Add("IWa Pb11");
            cmbPaProfilo.Items.Add("IWa Pb8");
            cmbPaProfilo.SelectedIndex = 0;

            cmbPaCondStop.Items.Clear();
            cmbPaCondStop.Items.Add("N.D.");
            cmbPaCondStop.Items.Add("Timer");
            cmbPaCondStop.Items.Add("dV/dt");
            cmbPaCondStop.SelectedIndex = 0;

           // cmbFSerBaudrateOC.DataSource = Lista;
          //  cmbFSerBaudrateOC.DisplayMember = "descrizione";
          //  cmbFSerBaudrateOC.ValueMember = "SettingValue";

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


                if (txtPaCorrente.Text.Length > 0)
                {
                    ushort result;
                    if (ushort.TryParse(txtPaCorrente.Text, out result))
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
                txtPaCorrente.Text = "";
                txtPaTensione.Text = "";

                cmbPaCondStop.SelectedIndex = 0;
                txtPaCoeffK.Text = "";
                txtPaTempoT2Min.Text = "";
                txtPaTempoT2Max.Text = "";

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
                                txtPaCorrente.Text = _par.ValoreParametro.ToString();
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
                    txtMatricola.Text = _cb.Intestazione.Matricola.ToString();
                    txtProgressivo.Text = _cb.Intestazione.Progressivo.ToString();
                    txtDataPrimaInst.Text = _cb.Intestazione.PrimaInstallazione;
                    txtModello.Text = _cb.Intestazione.modello;
                    txtTensione.Text = _cb.Intestazione.tensioneNominale.ToString();
                    txtCorrente.Text = _cb.Intestazione.correnteNominale.ToString();

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
                    if (_apparatoPresente) CaricaCicloAttuale(); 
                }

                
            
            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat_tabCaricaBatterie_Selected: " + Ex.Message);

            }
        }


        private void CaricaTabelleCicli()
        {

            lvwCicliMacchina.View = View.Details;
            lvwCicliMacchina.GridLines = true;
            lvwCicliMacchina.FullRowSelect = true;
            //Add column header
            lvwCicliMacchina.Columns.Add("Ciclo", 30, HorizontalAlignment.Center);
            lvwCicliMacchina.Columns.Add("Start", 120, HorizontalAlignment.Left);
            lvwCicliMacchina.Columns.Add("V Start", 60, HorizontalAlignment.Center);
            lvwCicliMacchina.Columns.Add("V 5 min", 60, HorizontalAlignment.Center);
            lvwCicliMacchina.Columns.Add("I 5 min", 60, HorizontalAlignment.Center);
            lvwCicliMacchina.Columns.Add("V Stop", 60, HorizontalAlignment.Center);
            lvwCicliMacchina.Columns.Add("I Stop", 60, HorizontalAlignment.Center);
            lvwCicliMacchina.Columns.Add("Ah", 60, HorizontalAlignment.Center);
            lvwCicliMacchina.Columns.Add("Durata", 60, HorizontalAlignment.Center);
            lvwCicliMacchina.Columns.Add("Errori", 60, HorizontalAlignment.Center);
            lvwCicliMacchina.Columns.Add("Cond Stop", 100, HorizontalAlignment.Center);
            lvwCicliMacchina.Columns.Add("T da Prec", 80,HorizontalAlignment.Center);
 
            //Add items in the listview
            string[] arr = new string[12];
            ListViewItem itm;

            arr = new string[12] { "1", "31.1.2014 12.30", "2,11", "2,23", "29,7", "2,63", "14,2", "324", "13:30", "0", "2", "1g 03:40" };
            itm = new ListViewItem(arr);
            lvwCicliMacchina.Items.Add(itm);
            arr = new string[12] { "2", "31.1.2014 12.30", "2,13", "2,25", "29,8", "2,64", "13,9", "336", "13:15", "0", "3", "3g 03:40" };
            itm = new ListViewItem(arr);
            lvwCicliMacchina.Items.Add(itm);
        
        }

        private void btnCaricaCicli_Click(object sender, EventArgs e)
        {

            try 
            {

                CaricaTabelleCicli();
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
            ScriviParametriAttuali();
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
            try
            {
                if (rbtFwBootLdr.Checked)
                {
                    // reset to bl
                    bool _esito = SwitchAreaBl("", true);

                }
            }
            catch (Exception Ex)
            {
                Log.Error("btnFwSwitchArea_Click: " + Ex.Message);
            }
        }

        private void rbtMemAreaLibera_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtMemAreaLibera.Checked) txtMemCFStartAdd.Text = "0";
        }

        private void rbtMemAreaApp1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtMemAreaApp1.Checked) txtMemCFStartAdd.Text = "1C0000";
        }

        private void rbtMemAreaApp2_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtMemAreaApp2.Checked) txtMemCFStartAdd.Text = "1E0000";
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
    }

}
