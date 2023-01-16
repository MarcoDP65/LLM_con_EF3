using BrightIdeasSoftware;
using ChargerLogic;
using log4net;
using MoriData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using Utility;

namespace PannelloCharger
{
    public partial class frmSuperCharger : Form

    {
        parametriSistema _parametri;
        SerialMessage _msg;
        LogicheBase _logiche;

        //public CicloDiCarica ParametriProfilo;
        public ModelloCiclo ModCicloCorrente = new ModelloCiclo();

        private CaricaBatteria _cb;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        private frmAvanzamentoCicli _avCicli = new frmAvanzamentoCicli();
        public bool ApparatoConnesso { get; private set; }
        public FileSetupRigeneratore ImmagineChargerMem = null;

        bool readingMessage = false;
        bool _portaCollegata;
        bool _cbCollegato;
        bool _inLettura = false;

        bool _apparatoPresente = false;

        // Liste per la gestione degli elenchi filtrati di profili e durate
        private List<_mbProfiloCarica> ProfiliCarica;
        private List<llDurataCarica> DurateCarica;
        private List<llTensioneBatteria> TensioniBatteria;
        public bool DatiInCaricamento { get; private set; }
        public bool ProfiloInCaricamento { get; private set; }

        public Color OppChargeAttivo = Color.Red;
        public Color OppChargeSpento = Color.Green;


        public int LeftPosPaOppOraFine { get; set; }
        public int LeftPosPaOppOraInizio { get; set; }

        /* ----------------------------------------------------------
         *  Dichiarazione eventi per la gestione avanzamento
         * ---------------------------------------------------------
         */

        public event StepHandler Step;
        public delegate void StepHandler(CaricaBatteria ull, ProgressChangedEventArgs e); //sbWaitEventStep e);
                                                                                          // ----------------------------------------------------------

        private llMemoriaCicli CicloCorrente;

        public List<llVariabili> ListaValori = new List<llVariabili>();  // lista per listview realtime logger
        public List<ParametriArea> ListaAreeLLF = new List<ParametriArea>();  // lista per listview file Firmware upload
        public List<ParametriArea> ListaAreeCCS = new List<ParametriArea>();  // lista per listview file Firmware upload

        private List<TabPage> PagineNascoste = new List<TabPage>();

        public System.Collections.Generic.List<ValoreLista> ListaBrSig = new List<ValoreLista>()
        {
            new ValoreLista("OFF", SerialMessage.OcBaudRate.OFF, false),
            new ValoreLista("ON 9.6K", SerialMessage.OcBaudRate.br_9k6, false),
            new ValoreLista("ON 19.2K", SerialMessage.OcBaudRate.br_19k2, false),
            new ValoreLista("ON 38.4K", SerialMessage.OcBaudRate.br_38k4, false),
            new ValoreLista("ON 57.6K", SerialMessage.OcBaudRate.br_57k6, false),
        };

        private llParametriApparato _tempParametri;


        public frmSuperCharger(ref parametriSistema _par, bool CaricaDati, string IdApparato, LogicheBase Logiche, bool SerialeCollegata, bool AutoUpdate)
        {
            try
            {
                bool EsitoApertura;
                ApparatoConnesso = false;
                _apparatoPresente = false;
                _parametri = _par;
                ProfiloInCaricamento = false;
                InitializeComponent();
                
                ResizeRedraw = true;


                _logiche = Logiche;
                if (SerialeCollegata)
                {
                    
                    EsitoApertura = attivaCaricabatterie(ref _par, SerialeCollegata);
                    //EsitoApertura = LeggiDatiCaricabatterie(ref _par, SerialeCollegata);

                    ApparatoConnesso = EsitoApertura;
                    InizializzaScheda();
                }
                else
                {
                    ApparatoConnesso = false;
                    if (IdApparato != "")
                    {
                        LeggiCbDaArchivio(ref _par, CaricaDati, IdApparato);
                    }
                }

                //InizializzaScheda();
                applicaAutorizzazioni();
                RidimensionaControlli();

                this.Cursor = Cursors.Arrow;

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }

        }

        public frmSuperCharger(ref parametriSistema _par, bool CaricaDati)
        {
            try
            {
                ApparatoConnesso = false;


                if (LeggiDatiCaricabatterie(ref _par, CaricaDati))
                {
                    ProfiloInCaricamento = false;
                    InizializzaScheda();
                    applicaAutorizzazioni();
                    RidimensionaControlli();

                }
                else
                {
                    this.Close();
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }

        }


        public bool attivaCaricabatterie(ref parametriSistema _par, bool CaricaDati)
        {
            bool _esito;
            try
            {
                //_parametri = _par;
                //InitializeComponent();
                ResizeRedraw = true;
                _msg = new SerialMessage();
                _cb = new CaricaBatteria(ref _parametri, _logiche.dbDati.connessione, CaricaBatteria.TipoCaricaBatteria.SuperCharger);
                InizializzaScheda();
                _esito = _cb.apriPorta();
                if (!_esito)
                {
                    MessageBox.Show(_parametri.lastError, "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Hide();  //Close();
                    return _esito;
                }

                // Ora apro esplicitamente il canale. se fallisco esco direttamente
               // _cb.apparatoPresente = true;
                _apparatoPresente = true;
                _esito = _cb.StartComunicazione();
                if (!_esito)
                {
                    //MessageBox.Show(_parametri.lastError, "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Hide();  //Close();
                    return _esito;
                }

                _esito = _cb.VerificaPresenza();
                _tempParametri = new llParametriApparato();

                _esito = _cb.CaricaApparatoA0();  
                if (_esito)
                {

                    _tempParametri = _cb.ParametriApparato;

                    txtGenRevFwDisplay.Text = _tempParametri.llParApp.SoftwareDISP;


                    if (_tempParametri.IdApparato == null)
                    {
                        // La scheda risponde correttamente ma non è inizializzata completamente (manca l'ID apparato)....
                        // Attivo solo la tab inizializzazione, se sono abilitato 

                        MessageBox.Show("Scheda controllo non inizializzata", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        return true;
                    }
                    else
                    {
                        if (_tempParametri.IdApparato == "????????" || _tempParametri.IdApparato.Trim() == "")
                        {
                            // La scheda risponde correttamente ma non è inizializzata completamente (manca l'ID apparato)....
                            // Attivo solo la tab inizializzazione, se sono abilitato 

                            MessageBox.Show("Scheda controllo non inizializzata", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            return true;
                        }

                    }

                    _cb.ApparatoLL = new LadeLightData(_logiche.dbDati.connessione, _tempParametri);


                    txtGenIdApparato.Text = _cb.ApparatoLL._ll.Id;
                    txtGenSerialeZVT.Text = _cb.ApparatoLL._ll.SerialeZVT;

                    txtGenMatricola.Text = _cb.Intestazione.Matricola.ToString();
                    txtGenModello.Text = _cb.Intestazione.modello;

                    _cbCollegato = true;

                    CaricaAreaContatori();

                    _cb.LeggiDatiCliente();
                    MostraDatiCliente();

                    // ora carico il ciclo corrente e i contatori programmazioni
                    CaricaProgrammazioni();

                    _cb.ApparatoLL.salvaDati();

                    _apparatoPresente = _esito;
                    return true;

                }

                return false;

            }
            catch
            {
                return false;
            }

        }

        public bool LeggiDatiCaricabatterie(ref parametriSistema _par, bool CaricaDati)
        {
            bool _esito;
            try
            {
                //_parametri = _par;
                //InitializeComponent();
                ResizeRedraw = true;
                _msg = new SerialMessage();
                _cb = new CaricaBatteria(ref _parametri, _logiche.dbDati.connessione, CaricaBatteria.TipoCaricaBatteria.SuperCharger);
                InizializzaScheda();



                _esito = _cb.apriPorta();
                if (_esito)
                {
                    _apparatoPresente = true;
                    // Ora apro esplicitamente il canale. se fallisco esco direttamente
                    _esito = _cb.StartComunicazione();
                    if (_esito)
                    {

                        _tempParametri = new llParametriApparato();

                        Log.Debug("Lancio lettura lunghi");

                        //_avCicli = new frmAvanzamentoCicli();
                        _avCicli.ParametriWorker.MainCount = 100;
                        _avCicli.llLocale = _cb;
                        _avCicli.ValStart = (int)0;
                        _avCicli.DbDati = _logiche.dbDati.connessione;
                        _avCicli.CaricaBrevi = false; // chkCaricaBrevi.Checked;
                        _avCicli.ElementoPilotato = frmAvanzamentoCicli.ControlledDevice.LadeLight;
                        _avCicli.TipoComando = elementiComuni.tipoMessaggio.CaricamentoInizialeLL;

                        // Apro il form con le progressbar
                        _avCicli.ShowDialog(this);


                       // _esito = _cb.LeggiDatiCompleti();
                    }
                }
                else
                {
                    MessageBox.Show(_parametri.lastError, "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Hide();  //Close();
                }
                //return _esito;




                _tempParametri = new llParametriApparato();

                _tempParametri = _cb.ParametriApparato;


                if (_tempParametri.IdApparato == null)
                {
                    // La scheda risponde correttamente ma non è inizializzata completamente (manca l'ID apparato)....
                    // Attivo solo la tab inizializzazione, se sono abilitato 

                    MessageBox.Show("Scheda controllo non inizializzata", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return true;
                }
                else
                {
                    if (_tempParametri.IdApparato == "????????" || _tempParametri.IdApparato.Trim() == "")
                    {
                        // La scheda risponde correttamente ma non è inizializzata completamente (manca l'ID apparato)....
                        // Attivo solo la tab inizializzazione, se sono abilitato 

                        MessageBox.Show("Scheda controllo non inizializzata", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        return true;
                    }

                }

                _cb.ApparatoLL = new LadeLightData(_logiche.dbDati.connessione, _tempParametri);


                txtGenIdApparato.Text = _cb.ApparatoLL._ll.Id;
                txtGenSerialeZVT.Text = _cb.ApparatoLL._ll.SerialeZVT;

                txtGenMatricola.Text = _cb.Intestazione.Matricola.ToString();
                txtGenModello.Text = _cb.Intestazione.modello;

                _cbCollegato = true;

                MostraDatiCliente();

                MostraContatori();

                MostraProgrammazioni();

                InizializzaListaCariche();

                _cb.ApparatoLL.salvaDati();

                _apparatoPresente = _esito;
                return true;



                return false;

            }

            catch
            {
                return false;
            }

        }






        public bool LeggiCbDaArchivio(ref parametriSistema _par, bool CaricaDati, string IdApparato)
        {
            bool _esito;
            try
            {

                ResizeRedraw = true;
                _msg = null;   //new SerialMessage();
                _cb = new CaricaBatteria(ref _parametri, _logiche.dbDati.connessione, CaricaBatteria.TipoCaricaBatteria.SuperCharger);
                _apparatoPresente = false;
                //_esito = _cb.VerificaPresenza();
                _tempParametri = new llParametriApparato();
                _esito = _cb.CaricaApparatoA0(IdApparato);

                if (_esito)
                {

                    _tempParametri = _cb.ParametriApparato;


                    if (_tempParametri.IdApparato == null)
                    {
                        // La scheda risponde correttamente ma non è inizializzata completamente (manca l'ID apparato)....
                        // Attivo solo la tab inizializzazione, se sono abilitato 

                        MessageBox.Show("Scheda controllo non inizializzata", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        return true;
                    }
                    else
                    {
                        if (_tempParametri.IdApparato == "????????" || _tempParametri.IdApparato.Trim() == "")
                        {
                            // La scheda risponde correttamente ma non è inizializzata completamente (manca l'ID apparato)....
                            // Attivo solo la tab inizializzazione, se sono abilitato 

                            MessageBox.Show("Scheda controllo non inizializzata", "Connessione", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            return true;
                        }

                    }

                    _cb.ApparatoLL = new LadeLightData(_logiche.dbDati.connessione, _tempParametri);


                    txtGenIdApparato.Text = _cb.ApparatoLL._ll.Id;
                    txtGenSerialeZVT.Text = _cb.ApparatoLL._ll.SerialeZVT;

                    txtGenMatricola.Text = _cb.Intestazione.Matricola.ToString();
                    txtGenModello.Text = _cb.Intestazione.modello;

                    _cbCollegato = true;

                    CaricaContatori(IdApparato);

                    _cb.DatiCliente = new llDatiCliente(_logiche.dbDati.connessione);
                    _cb.DatiCliente.caricaDati(IdApparato,1);

                    MostraDatiCliente();

                    // ora carico il ciclo corrente e i contatori programmazioni
                    InizializzaScheda();
                    LeggiProgrammazioniDB(IdApparato);

                    // Carico la lista cariche

                    if (_cb.LeggiMemoriaCicliDB(IdApparato))
                    {
                        InizializzaListaCariche();
                    }

                    ModoArchivio();
                    _apparatoPresente = false;
                    return true;

                }

                return false;

            }
            catch
            {
                return false;
            }

        }


        public bool ModoArchivio()
        {
            try
            {

                btnCaricaCliente.Visible = false;
                btnSalveCliente.Visible = false;
                btnCaricaMemoria.Visible = false;
                btnCaricaContatori.Visible = false;
                btnGenAzzzeraContatori.Visible = false;
                btnGenAzzzeraContatoriTot.Visible = false;
                btnGenResetBoard.Visible = false;
                grbCicli.Visible = false;
                grbVarParametriSig.Visible = false;
                btnPaAttivaConfigurazione.Visible = false;
                btnCicloCorrente.Visible = false;
                btnPaProfileChiudiCanale.Visible = false;
                btnPaProfileRefresh.Visible = false;
                chkPaSbloccaValori.Visible = false;
                lblPaSbloccaValori.Visible = false;
                btnPaCaricaCicli.Visible = false;
                btnPaSalvaDati.Visible = false;

                tabCaricaBatterie.TabPages.Remove(tabOrologio);
                tabCaricaBatterie.TabPages.Remove(tabMemRead);
                tabCaricaBatterie.TabPages.Remove(tbpFirmware);
                tabCaricaBatterie.TabPages.Remove(tbpProxySig60);
                tabCaricaBatterie.TabPages.Remove(tabCb02);
                tabCaricaBatterie.TabPages.Remove(tabMonitor);

                tabCaricaBatterie.TabPages.Remove(tabMonitor);

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("ModoArchivio: " + Ex.Message);
                return false;
            }

        }



        public frmSuperCharger()
        {
            InitializeComponent();
            InizializzaScheda();

        }

        /// <summary>
        /// Inizializza le combo; da rendere dinamico in effetivo
        /// </summary>
        private void InizializzaScheda()
        {

            cmbInitTipoApparato.DataSource = _cb.DatiBase.ModelliLL;
            cmbInitTipoApparato.ValueMember = "IdModelloLL";
            cmbInitTipoApparato.DisplayMember = "NomeModello";

            cmbPaTipoBatteria.DataSource = _parametri.ParametriProfilo.ModelliBatteria;  //   TipiBattria;
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
                //ushort _divK = 10;
                _cb.CicloInMacchina = new SerialMessage.cicloAttuale()
                {
                    // LunghezzaNome = (byte)txtPaNomeProfilo.Text.Length,
                    // NomeCiclo = txtPaNomeProfilo.Text,
                    TipoCiclo = (byte)cmbPaProfilo.SelectedIndex
                };


                if (txtPaCapacita.Text.Length > 0)
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


                if (txtPaTempoT2Max.Text.Length > 0)
                {
                    ushort result;
                    if (ushort.TryParse(cmbPaDurataMaxT1.Text, out result))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TempoT1Max;
                        _par.ValoreParametro = result;
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }
                }


                if (txtPaSogliaVs.Text.Length > 0)
                {
                    Double dresult;
                    ushort result;

                    if (Double.TryParse(txtPaSogliaVs.Text, out dresult))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneSogliaF1;
                        result = (ushort)(dresult * 100);
                        _par.ValoreParametro = result;
                        txtPaSogliaVs.Text = dresult.ToString("0.00");
                        _cb.CicloInMacchina.Parametri.Add(_par);
                        _numParametri++;
                    }

                }

                if (txtPaCorrenteI1.Text.Length > 0)
                {
                    ushort result;
                    if (ushort.TryParse(txtPaCorrenteI1.Text, out result))
                    {
                        ParametroLL _par = new ParametroLL();
                        _par.idParametro = (byte)SerialMessage.ParametroLadeLight.CorrenteCaricaI1;
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

                _cb.CicloInMacchina.NumeroParametri = _numParametri;

                _cb.ScriviCicloCorrente();

                return true;
            }
            catch
            {
                return false;
            }
        }



        public bool CopiaParametriCarica(int NumCopie)
        {
            try
            {

                bool esito;


                // Ricarico i valori impostati nelle textbox
                esito = LeggiValoriParametriCarica();

                if (esito)
                {
                    MostraParametriCiclo(false);
                    ModCicloCorrente.GeneraProgrammaCarica();
                    _cb.Programmazioni.ProgrammaAttivo = ModCicloCorrente.ProfiloRegistrato;
                    _cb.SalvaProgrammazioniApparato();

                }

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
                elementiComuni.EsitoVerificaParametri EsitoVer;
                // Se so già che devo salvare, non faccio nemmeno il controllo dati:
                if (ModCicloCorrente.DatiSalvati)
                {
                    // Se almeno 1 parametro è diverso dal ciclo attivo corrente lo salvo come nuovo ciclo
                    EsitoVer = VerificaValoriParametriCarica();
                    if (EsitoVer == elementiComuni.EsitoVerificaParametri.Ok)
                    {
                        //coincide col programma esistente. esco
                        return true;
                    }
                    else
                    {
                        // Almeno 1 valore cambiato --> prenoto il cambio ID
                        ModCicloCorrente.RichiestoNuovoId = true;

                    }
                }
                // Carico i valori impostati nelle textbox
                esito = LeggiValoriParametriCarica();

                ModCicloCorrente.IdProgramma = (ushort)(_cb.Programmazioni.UltimoIdProgamma + 1);
                bool esitoSalvataggio = false;
                if (esito)
                {
                    // Riscrivo i valori nelle textBox per conferma poi salvo i valori 
                    //MostraParametriCiclo(false);
                    ModCicloCorrente.GeneraProgrammaCarica();
                    _cb.Programmazioni.ProgrammaAttivo = ModCicloCorrente.ProfiloRegistrato;
                    _cb.PreparaSalvataggioProgrammazioni();
                    esitoSalvataggio = _cb.SalvaProgrammazioniApparato();

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
                // prima di tutto controllo se ho attivato la connessione spybatt; in questo caso il profilo è tutto a 0
                if (chkPaUsaSpyBatt.Checked)
                {
                    ModCicloCorrente.ValoriCiclo.AzzeraValori();
                    ModCicloCorrente.ValoriCiclo.AbilitaSpyBatt = (ushort)(chkPaUsaSpyBatt.Checked ? 0x000F : 0x00F0);
                    ModCicloCorrente.NomeProfilo = "SPY-BATT";
                }
                else
                {
                    // Nome
                    string _tempStr = txtPaNomeSetup.Text.Trim();
                    // Cassone
                    ModCicloCorrente.ValoriCiclo.TipoCassone = FunzioniMR.ConvertiUshort(txtPaCassone.Text, 1, 0);


                    // Generale
                    ModCicloCorrente.NomeProfilo = _tempStr;
                    //ModCicloCorrente.IdProgramma = FunzioniMR.ConvertiUshort(txtPaIdSetup.Text, 1, 0);

                    // Batteria
                    if (cmbPaTipoBatteria.SelectedItem != null)
                    {
                        ModCicloCorrente.Batteria = (mbTipoBatteria)cmbPaTipoBatteria.SelectedItem;
                    }
                    else
                    {
                        // Non ho una batteria attiva. mi fermo quì
                        return false;
                    }

                    // Tensione
                    ModCicloCorrente.Tensione = FunzioniMR.ConvertiUshort(txtPaTensione.Text, 100, 0);
                    // Numero Celle
                    ModCicloCorrente.NumeroCelle = FunzioniMR.ConvertiByte(txtPaNumCelle.Text, 1, 1);
                    // Capacità
                    ModCicloCorrente.Capacita = FunzioniMR.ConvertiUshort(txtPaCapacita.Text, 10, 0);
                    // Profilo
                    if (cmbPaProfilo.SelectedItem != null)
                    {
                        ModCicloCorrente.Profilo = (_mbProfiloCarica)cmbPaProfilo.SelectedItem;
                    }
                    else
                    {
                        // Non ho un profilo attivo. mi fermo quì
                        return false;
                    }

                    // Flag:
                    // Equal
                    ModCicloCorrente.ValoriCiclo.EqualAttivo = (ushort)(chkPaAttivaEqual.Checked ? 0x000F : 0x00F0);

                    // Mant:
                    ModCicloCorrente.ValoriCiclo.MantAttivo = (ushort)(chkPaAttivaMant.Checked ? 0x000F : 0x00F0);

                    // Usa SB
                    ModCicloCorrente.ValoriCiclo.AbilitaSpyBatt = (ushort)(chkPaUsaSpyBatt.Checked ? 0x000F : 0x00F0);

                    // Usa Safety
                    ModCicloCorrente.ValoriCiclo.AbilitaSafety = (ushort)(chkPaUsaSafety.Checked ? 0x000F : 0x00F0);


                    // Preciclo
                    ModCicloCorrente.ValoriCiclo.CorrenteI0 = FunzioniMR.ConvertiUshort(txtPaPrefaseI0.Text, 10, 0);
                    ModCicloCorrente.ValoriCiclo.TensionePrecicloV0 = FunzioniMR.ConvertiUshort(txtPaSogliaV0.Text, 100, 0);
                    ModCicloCorrente.ValoriCiclo.TempoT0Max = FunzioniMR.ConvertiUshort(txtPaDurataMaxT0.Text, 1, 0);

                    // Fase 1 (I) 
                    ModCicloCorrente.ValoriCiclo.TensioneSogliaVs = FunzioniMR.ConvertiUshort(txtPaSogliaVs.Text, 100, 0);
                    ModCicloCorrente.ValoriCiclo.CorrenteI1 = FunzioniMR.ConvertiUshort(txtPaCorrenteI1.Text, 10, 0);
                    ModCicloCorrente.ValoriCiclo.TempoT1Max = FunzioniMR.ConvertiUshort(cmbPaDurataMaxT1.Text, 1, 0);

                    // Fase 2 (U o W) 
                    ModCicloCorrente.ValoriCiclo.TensioneRaccordoVr = FunzioniMR.ConvertiUshort(txtPaRaccordoF1.Text, 100, 0);
                    ModCicloCorrente.ValoriCiclo.CorrenteRaccordoIr = FunzioniMR.ConvertiUshort(txtPaCorrenteRaccordo.Text, 10, 0);
                    ModCicloCorrente.ValoriCiclo.CorrenteFinaleI2 = FunzioniMR.ConvertiUshort(txtPaCorrenteF3.Text, 10, 0);
                    ModCicloCorrente.ValoriCiclo.TensioneMassimaVMax = FunzioniMR.ConvertiUshort(txtPaVMax.Text, 100, 0);
                    ModCicloCorrente.ValoriCiclo.TempoT2Min = FunzioniMR.ConvertiUshort(txtPaTempoT2Min.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.TempoT2Max = FunzioniMR.ConvertiUshort(txtPaTempoT2Max.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.FattoreK = FunzioniMR.ConvertiUshort(txtPaCoeffK.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.CoeffKc = FunzioniMR.ConvertiUshort(txtPaCoeffKc.Text, 1, 0);


                    // Fase 3 (I) 
                    ModCicloCorrente.ValoriCiclo.TempoT3Max = FunzioniMR.ConvertiUshort(txtPaTempoT3Max.Text, 1, 0);

                    // Equalizzazione
                    ModCicloCorrente.ValoriCiclo.EqualAttivabile = (ushort)(chkPaAttivaEqual.Checked == true ? 0x000F : 0x00F0);
                    ModCicloCorrente.ValoriCiclo.EqualTempoAttesa = FunzioniMR.ConvertiUshort(txtPaEqualAttesa.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.EqualNumImpulsi = FunzioniMR.ConvertiUshort(txtPaEqualNumPulse.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.EqualTempoPausa = FunzioniMR.ConvertiUshort(txtPaEqualPulsePause.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.EqualTempoImpulso = FunzioniMR.ConvertiUshort(txtPaEqualPulseTime.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso = FunzioniMR.ConvertiUshort(txtPaEqualPulseCurrent.Text, 10, 0);

                    // Mantenimento
                    ModCicloCorrente.ValoriCiclo.MantAttivabile = (ushort)(chkPaAttivaMant.Checked == true ? 0x000F : 0x00F0);
                    ModCicloCorrente.ValoriCiclo.MantTempoAttesa = FunzioniMR.ConvertiUshort(txtPaMantAttesa.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.MantTensIniziale = FunzioniMR.ConvertiUshort(txtPaMantVmin.Text, 100, 0);
                    ModCicloCorrente.ValoriCiclo.MantTensFinale = FunzioniMR.ConvertiUshort(txtPaMantVmax.Text, 100, 0);
                    ModCicloCorrente.ValoriCiclo.MantTempoMaxErogazione = FunzioniMR.ConvertiUshort(txtPaMantDurataMax.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.MantCorrenteImpulso = FunzioniMR.ConvertiUshort(txtPaMantCorrente.Text, 10, 0);

                    // Opportunity
                    ModCicloCorrente.ValoriCiclo.OpportunityAttivabile = (ushort)(chkPaAttivaOppChg.Checked == true ? 0x000F : 0x00F0);
                    // non leggo le textbox degli orari: lo slider aggiorna direttamente il parametro
                    //ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = FunzioniMR.ConvertiUshort(txtPaOppOraInizio.Text, 1, 0);  
                    //ModCicloCorrente.ValoriCiclo.OpportunityOraFine = FunzioniMR.ConvertiUshort(txtPaOppOraFine.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.OpportunityDurataMax = FunzioniMR.ConvertiUshort(txtPaOppDurataMax.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.OpportunityCorrente = FunzioniMR.ConvertiUshort(txtPaOppCorrente.Text, 10, 0);
                    ModCicloCorrente.ValoriCiclo.OpportunityTensioneMax = FunzioniMR.ConvertiUshort(txtPaOppVSoglia.Text, 100, 0);



                    // Soglie
                    ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMin = FunzioniMR.ConvertiUshort(txtPaVMinRic.Text, 100, 0);
                    ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMax = FunzioniMR.ConvertiUshort(txtPaVMaxRic.Text, 100, 0);
                    ModCicloCorrente.ValoriCiclo.TensMinStop = FunzioniMR.ConvertiUshort(txtPaVMinStop.Text, 100, 0);
                    ModCicloCorrente.ValoriCiclo.TensioneLimiteVLim = FunzioniMR.ConvertiUshort(txtPaVLimite.Text, 100, 0);
                    ModCicloCorrente.ValoriCiclo.CorrenteMassima = FunzioniMR.ConvertiUshort(txtPaCorrenteMassima.Text, 10, 0);
                }
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("LeggiValoriParametriCarica: " + Ex.Message);
                return false;
            }
        }

        public elementiComuni.EsitoVerificaParametri VerificaValoriParametriCarica()
        {
            try
            {


                // Cassone
                if (ModCicloCorrente.ValoriCiclo.TipoCassone != FunzioniMR.ConvertiUshort(txtPaCassone.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreCassone;

                // Nome
                string _tempStr = txtPaNomeSetup.Text.Trim();
                // Generale
                if (ModCicloCorrente.NomeProfilo != _tempStr) return elementiComuni.EsitoVerificaParametri.ErroreNome;
                //ModCicloCorrente.IdProgramma = FunzioniMR.ConvertiUshort(txtPaIdSetup.Text, 1, 0);

                // Batteria
                if (cmbPaTipoBatteria.SelectedItem != null)
                {
                    mbTipoBatteria tmpBat = (mbTipoBatteria)(cmbPaTipoBatteria.SelectedItem);
                    if (ModCicloCorrente.Batteria.BatteryTypeId != tmpBat.BatteryTypeId) return elementiComuni.EsitoVerificaParametri.BatteriaNonCorretta;
                }
                else
                {
                    // Non ho una batteria attiva. mi fermo quì
                    return elementiComuni.EsitoVerificaParametri.BatteriaNonDefinita;
                }

                // Tensione
                if (ModCicloCorrente.Tensione != FunzioniMR.ConvertiUshort(txtPaTensione.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.BatteriaNonCorretta;
                // Numero Celle
                if (ModCicloCorrente.NumeroCelle != FunzioniMR.ConvertiByte(txtPaNumCelle.Text, 1, 1)) return elementiComuni.EsitoVerificaParametri.BatteriaNonCorretta;
                // Capacità
                if (ModCicloCorrente.Capacita != FunzioniMR.ConvertiUshort(txtPaCapacita.Text, 10, 0)) return elementiComuni.EsitoVerificaParametri.BatteriaNonCorretta;
                // Profilo
                if (cmbPaProfilo.SelectedItem != null)
                {
                    _mbProfiloCarica tmpPC = (_mbProfiloCarica)(cmbPaProfilo.SelectedItem);
                    if (ModCicloCorrente.Profilo.IdProfiloCaricaLL != tmpPC.IdProfiloCaricaLL) return elementiComuni.EsitoVerificaParametri.ProfiloNonCorretto;
                }
                else
                {
                    // Non ho un profilo attivo. mi fermo quì
                    return elementiComuni.EsitoVerificaParametri.ProfiloNonDefinito;
                }

                // Flag:
                // Equal
                //if (ModCicloCorrente.ValoriCiclo.EqualAttivo != (ushort)(chkPaAttivaEqual.Checked ? 0x000F : 0x00F0)) return false;

                // Mant:
                //if (ModCicloCorrente.ValoriCiclo.MantAttivo != (ushort)(chkPaAttivaMant.Checked ? 0x000F : 0x00F0)) return false;

                // Usa SB
                if (ModCicloCorrente.ValoriCiclo.AbilitaSpyBatt != (ushort)(chkPaUsaSpyBatt.Checked ? 0x0000 : 0x00F0)) return elementiComuni.EsitoVerificaParametri.ErroreSpyBatt;


                // Preciclo
                if (ModCicloCorrente.ValoriCiclo.CorrenteI0 != FunzioniMR.ConvertiUshort(txtPaPrefaseI0.Text, 10, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.TensionePrecicloV0 != FunzioniMR.ConvertiUshort(txtPaSogliaV0.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.TempoT0Max != FunzioniMR.ConvertiUshort(txtPaDurataMaxT0.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;

                // Fase 1 (I) 
                if (ModCicloCorrente.ValoriCiclo.TensioneSogliaVs != FunzioniMR.ConvertiUshort(txtPaSogliaVs.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.CorrenteI1 != FunzioniMR.ConvertiUshort(txtPaCorrenteI1.Text, 10, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.TempoT1Max != FunzioniMR.ConvertiUshort(cmbPaDurataMaxT1.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;

                // Fase 2 (U o W) 
                if (ModCicloCorrente.ValoriCiclo.TensioneRaccordoVr != FunzioniMR.ConvertiUshort(txtPaRaccordoF1.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.CorrenteRaccordoIr != FunzioniMR.ConvertiUshort(txtPaCorrenteRaccordo.Text, 10, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.CorrenteFinaleI2 != FunzioniMR.ConvertiUshort(txtPaCorrenteF3.Text, 10, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.TensioneMassimaVMax != FunzioniMR.ConvertiUshort(txtPaVMax.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.TempoT2Min != FunzioniMR.ConvertiUshort(txtPaTempoT2Min.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.TempoT2Max != FunzioniMR.ConvertiUshort(txtPaTempoT2Max.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.FattoreK != FunzioniMR.ConvertiUshort(txtPaCoeffK.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.CoeffKc != FunzioniMR.ConvertiUshort(txtPaCoeffKc.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;

                // Fase 3 (I) 
                if (ModCicloCorrente.ValoriCiclo.TempoT3Max != FunzioniMR.ConvertiUshort(txtPaTempoT3Max.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;

                // Equalizzazione
                if (ModCicloCorrente.ValoriCiclo.EqualAttivabile != (ushort)(chkPaAttivaEqual.Checked == true ? 0x000F : 0x00F0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.EqualTempoAttesa != FunzioniMR.ConvertiUshort(txtPaEqualAttesa.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.EqualNumImpulsi != FunzioniMR.ConvertiUshort(txtPaEqualNumPulse.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.EqualTempoPausa != FunzioniMR.ConvertiUshort(txtPaEqualPulsePause.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.EqualTempoImpulso != FunzioniMR.ConvertiUshort(txtPaEqualPulseTime.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso != FunzioniMR.ConvertiUshort(txtPaEqualPulseCurrent.Text, 10, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;

                // Mantenimento
                if (ModCicloCorrente.ValoriCiclo.MantAttivabile != (ushort)(chkPaAttivaMant.Checked == true ? 0x000F : 0x000F0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.MantTempoAttesa != FunzioniMR.ConvertiUshort(txtPaMantAttesa.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.MantTensIniziale != FunzioniMR.ConvertiUshort(txtPaMantVmin.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.MantTensFinale != FunzioniMR.ConvertiUshort(txtPaMantVmax.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.MantTempoMaxErogazione != FunzioniMR.ConvertiUshort(txtPaMantDurataMax.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.MantCorrenteImpulso != FunzioniMR.ConvertiUshort(txtPaMantCorrente.Text, 10, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;


                // Opportunity
                
                if (ModCicloCorrente.ValoriCiclo.OpportunityAttivabile != (ushort)(chkPaAttivaOppChg.Checked == true ? 0x000F : 0x000F0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                // non leggo le textbox degli orari: lo slider aggiorna direttamente il parametro
                //ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = FunzioniMR.ConvertiUshort(txtPaOppOraInizio.Text, 1, 0);  
                //ModCicloCorrente.ValoriCiclo.OpportunityOraFine = FunzioniMR.ConvertiUshort(txtPaOppOraFine.Text, 1, 0);
                if (ModCicloCorrente.ValoriCiclo.OpportunityDurataMax != FunzioniMR.ConvertiUshort(txtPaOppDurataMax.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.OpportunityCorrente != FunzioniMR.ConvertiUshort(txtPaOppCorrente.Text, 10, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.OpportunityTensioneMax != FunzioniMR.ConvertiUshort(txtPaOppVSoglia.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;

                // Soglie
                if (ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMin != FunzioniMR.ConvertiUshort(txtPaVMinRic.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMax != FunzioniMR.ConvertiUshort(txtPaVMaxRic.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.TensMinStop != FunzioniMR.ConvertiUshort(txtPaVMinStop.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.TensioneLimiteVLim != FunzioniMR.ConvertiUshort(txtPaVLimite.Text, 100, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.CorrenteMassima != FunzioniMR.ConvertiUshort(txtPaCorrenteMassima.Text, 10, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;

                // TRUE => nessun parametro è stato modificato.
                return elementiComuni.EsitoVerificaParametri.Ok;
            }
            catch (Exception Ex)
            {
                Log.Error("LeggiValoriParametriCarica: " + Ex.Message);
                return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
            }
        }



        /// <summary>
        /// Aggiorno il form con i dati di ciclo attivo dell'ultima lettura
        /// </summary>
        /// <returns></returns>
        public bool MostraCicloAttuale(SerialMessage.cicloAttuale CicloAttuale)
        {
            ushort _divK = 10;
            try
            {

                //Prima Vuoto tutto
                //txtPaNomeProfilo.Text = "";

                //cmbPaProfilo.SelectedIndex = 0;
                txtPaCapacita.Text = "";
                txtPaTempoT2Max.Text = "";
                txtPaSogliaVs.Text = "";
                txtPaCorrenteI1.Text = "";
                txtPaTensione.Text = "";

                //cmbPaCondStop.SelectedIndex = 0;
                txtPaCoeffK.Text = "";
                txtPaTempoT2Min.Text = "";
                txtPaTempoT2Max.Text = "";
                chkPaUsaSpyBatt.CheckState = CheckState.Indeterminate;

                if (CicloAttuale.datiPronti)
                {
                    //txtPaNomeProfilo.Text = CicloAttuale.NomeCiclo;
                    cmbPaProfilo.SelectedIndex = CicloAttuale.TipoCiclo;

                    foreach (ParametroLL _par in CicloAttuale.Parametri)
                    {
                        float _tempVal;

                        switch (_par.idParametro)
                        {
                            case (byte)SerialMessage.ParametroLadeLight.CapacitaNominale:
                                txtPaCapacita.Text = _par.ValoreParametro.ToString();
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.TempoT1Max:
                                cmbPaDurataMaxT1.Text = _par.ValoreParametro.ToString();
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.TensioneSogliaF1:
                                _tempVal = (float)_par.ValoreParametro / 100;
                                txtPaSogliaVs.Text = _tempVal.ToString("0.00");
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.CorrenteCaricaI1:
                                txtPaCorrenteI1.Text = _par.ValoreParametro.ToString();
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.TensioneNominale:
                                txtPaTensione.Text = _par.ValoreParametro.ToString();
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
                                //txtPaFreqSwitch.Text = _par.ValoreParametro.ToString();
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.DivisoreK:
                                //txtPaParDivK.Text = _par.ValoreParametro.ToString();
                                _divK = _par.ValoreParametro;
                                if (_divK > 0)
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
                                // txtPaParKp.Text = _tempVal.ToString(FunzioniMR.StringaModelloDivisore(_divK));
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.ParametroKI:
                                _tempVal = (float)_par.ValoreParametro / _divK;
                                // txtPaParKi.Text = _tempVal.ToString(FunzioniMR.StringaModelloDivisore(_divK));
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.ParametroKD:
                                _tempVal = (float)_par.ValoreParametro / _divK;
                                // txtPaParKd.Text = _tempVal.ToString(FunzioniMR.StringaModelloDivisore(_divK));
                                break;
                            case (byte)SerialMessage.ParametroLadeLight.CapacitaDaRicaricare:
                                //  txtPaCapDaCaricare.Text = _par.ValoreParametro.ToString();
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
                txtContNumProgrammazioni.Text = "";
                txtContCaricheOpportunity.Text = "";


                if (_cb.ContatoriLL.valido)
                {
                    txtContDtPrimaCarica.Text = _cb.ContatoriLL.strDataPrimaCarica;
                    txtContDtUltimaCanc.Text = _cb.ContatoriLL.strDataUltimaCancellazione;
                    txtContBreviSalvati.Text = _cb.ContatoriLL.CntCicliBrevi.ToString();
                    txtContCariche3to6.Text = _cb.ContatoriLL.CntCicli3Hto6H.ToString();
                    txtContCariche6to9.Text = _cb.ContatoriLL.CntCicli6Hto9H.ToString();
                    txtContCaricheOver9.Text = _cb.ContatoriLL.CntCicliOver9H.ToString();
                    txtContCaricheSalvate.Text = _cb.ContatoriLL.CntCariche.ToString();
                    txtContCaricheStop.Text = _cb.ContatoriLL.CntCicliStop.ToString();
                    txtContCaricheStrappo.Text = _cb.ContatoriLL.CntCicliStaccoBatt.ToString();
                    txtContCaricheTotali.Text = _cb.ContatoriLL.CntCicliTotali.ToString();
                    txtContCaricheUnder3.Text = _cb.ContatoriLL.CntCicliLess3H.ToString();
                    txtContPntNextBreve.Text = _cb.ContatoriLL.strPntNextBreve;
                    txtContPntNextCarica.Text = _cb.ContatoriLL.strPntNextCarica;
                    txtContNumProgrammazioni.Text = _cb.ContatoriLL.CntProgrammazioni.ToString(); ;
                    txtContCaricheOpportunity.Text = _cb.ContatoriLL.CntCicliOpportunity.ToString();


                    txtContNumCancellazioni.Text = _cb.ContatoriLL.CntMemReset.ToString();
                    if (_cb.ContatoriLL.CntMemReset > 0)
                    {
                        txtContDtUltimaCanc.Text = _cb.ContatoriLL.strDataUltimaCancellazione;
                    }
                    else
                    {
                        txtContDtUltimaCanc.Text = "";
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

                RidimensionaControlli();

            }
            catch (Exception Ex)
            {
                Log.Error("frmCaricabatterie_Resize: " + Ex.Message);
            }

        }


        private void RidimensionaControlli()
        {
            try
            {
                tabCaricaBatterie.Width = this.Width - 42;
                tabCaricaBatterie.Height = this.Height - 75; // 109;

                // Tab Cicli
                flvCicliListaCariche.Width = tabCb04.Width - 20;
                flvCicliListaCariche.Height = tabCb04.Height - 140;

                grbCicli.Top = tabCb04.Height - 110;
                grbCicli.Width = tabCb04.Width - 20;

                //flvCicliListaCariche.Width = tabCb04.Width - 20;
                if (tbpPaListaProfili.Width > 200)
                {
                    flwPaListaConfigurazioni.Width = tbpPaListaProfili.Width - 30;
                }


                LeftPosPaOppOraFine = txtPaOppOraFine.Left;

                LeftPosPaOppOraInizio = txtPaOppOraFine.Left;
                // Inizializzo a True il visible delle tabpages




            }
            catch (Exception Ex)
            {
                Log.Error("RidimensionaControlli: " + Ex.Message);
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
            catch (Exception Ex)
            {
                Log.Error("CaricaListaCicli: " + Ex.Message);
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
            catch (Exception Ex)
            {
                Log.Error("CaricaOrologio: " + Ex.Message);
            }
        }

        public void CaricaCicloAttuale()
        {

            bool _esito;
            try
            {

                _esito = _cb.CaricaProgrammaAttivo();
                if (_esito)
                {

                    MostraCicloAttuale(_cb.CicloInMacchina);

                }

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicloAttuale: " + Ex.Message);
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
            catch (Exception Ex)
            {
                Log.Error("CaricaListaProgrammazioni: " + Ex.Message);
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
            catch (Exception Ex)
            {
                Log.Error("CaricaAreaContatori: " + Ex.Message);
            }
        }


        public void CaricaContatori(string IdApparato)
        {
            bool _esito;
            try
            {



                _esito = _cb.CaricaContatori(IdApparato);


                if (_esito)
                {

                    MostraContatori();

                }

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaAreaContatori: " + Ex.Message);
            }
        }



        public void CaricaVariabili()
        {
            bool _esito;
            try
            {

                _esito = _cb.LeggiVariabili();


                MostraVariabili(_esito, (chkDatiDiretti.Checked == true));
                if (chkParRegistraLetture.Checked)
                {
                    llVariabili _tmpValore = new llVariabili();
                    _tmpValore = _cb.llVariabiliAttuali;
                    _tmpValore.Lettura = ListaValori.Count() + 1;
                    ListaValori.Add(_tmpValore);
                    flvwLettureParametri.SetObjects(ListaValori);
                    Application.DoEvents();
                }


            }
            catch (Exception Ex)
            {
                Log.Error("CaricaVariabili: " + Ex.Message);
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



                if (DatiPresenti)
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
                    RidimensionaControlli();
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

        private void grbCicloCorrente_Enter(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }



        private void pbxIWAsmall_Click(object sender, EventArgs e)
        {
            frmGraficoCiclo frmGrafico = new frmGraficoCiclo();
            //sbDettaglioCar.MdiParent = this;
            frmGrafico.StartPosition = FormStartPosition.CenterParent;
            frmGrafico.TipoGrafico(0);
            frmGrafico.ShowDialog();
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




        private void btnCicloCorrente_Click(object sender, EventArgs e)
        {
            try
            {
                bool Esito;

                Esito = _cb.CaricaProgrammaAttivo();

                if (Esito)
                {
                    //MostraCicloCorrente();
                    ProfiloInCaricamento = true;
                    ModCicloCorrente.ProfiloRegistrato = _cb.Programmazioni.ProgrammaAttivo;
                    ModCicloCorrente.EstraiDaProgrammaCarica();

                    MostraParametriCiclo(true, false);
                    ProfiloInCaricamento = false;
                }
                else
                {
                    MostraParametriCiclo(true, true);
                }



            }
            catch
            {
                ProfiloInCaricamento = false;
            }
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
            bool esitoSalvataggio = false;
            this.Cursor = Cursors.WaitCursor;
            esitoSalvataggio = ScriviParametriCarica();

            if (esitoSalvataggio)
            {

                CaricaProgrammazioni();
                IncrementaContatoreConf();
                MessageBox.Show("Configurazione Aggiornata", "CONFIGURAZIONE", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Aggiornamento configurazione non riuscito", "CONFIGURAZIONE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            this.Cursor = Cursors.Default;
        }


        private void btnLeggiVariabili_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            CaricaVariabili();
            this.Cursor = Cursors.Default;
        }

        private void tmrLetturaAutomatica_Tick(object sender, EventArgs e)
        {
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
                    //string[] arr;

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
            this.Cursor = Cursors.WaitCursor;
            ChiamataProxySig60(36);
            this.Cursor = Cursors.Default;
        }

        private void btnStratTest02_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            ChiamataProxySig60(2);
            this.Cursor = Cursors.Default;
        }

        private void btnStratTestERR_Click(object sender, EventArgs e)
        {
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
                this.Cursor = Cursors.WaitCursor;
                AggiornaFirmware();
                _cb.VerificaPresenza();

                VerificaStatoFw();

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
                bool _memoriaInterna;

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
                _memoriaInterna = chkMemInt.Checked;
                if (_StartAddr < 0) _StartAddr = 0;
                if (chkMemHex.Checked)
                    txtMemAddrR.Text = _StartAddr.ToString("X6");
                else
                    txtMemAddrR.Text = _StartAddr.ToString();

                txtMemDataGrid.Text = "";
                _esito = LeggiBloccoMemoria(_StartAddr, _NumByte, _memoriaInterna);


            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }

        }

        public bool LeggiBloccoMemoria(uint StartAddr, ushort NumByte,bool MemoriaInterna )
        {
            try
            {

                byte[] _Dati;
                bool _esito;

                txtMemDataGrid.Text = "";
                _Dati = new byte[NumByte];
                _esito = _cb.LeggiBloccoMemoria(StartAddr, NumByte, out _Dati, MemoriaInterna);

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
                    if(MemoriaInterna)
                    {
                        txtMemDataGrid.ForeColor = Color.Blue;
                    }
                    else
                    {
                        txtMemDataGrid.ForeColor = Color.Black;
                    }
                    txtMemDataGrid.Text = _risposta;

                }
                else
                {
                    txtMemDataGrid.ForeColor = Color.Red;
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
                    // _cb.ParametriApparato = new llParametriApparato();
                    // 11/02/2019 - Non posso partire e riprogrammare se non sono presenti i dati originali di cui alcuni sevono essere rigidamente conservati.
                    Log.Debug("Dati originali mancanti; inizializzazione non possiblile.");
                    return false;
                }

                // 11/02/2019 - mantengo id e nome preesistenti

                uint TmpInt;
                bool _esito;
                byte[] tempVal;

                TmpInt = 0xFFFFFFFF;
                if (uint.TryParse(txtInitSerialeApparato.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out TmpInt))
                {
                    _cb.ParametriApparato.llParApp.SerialeApparato = TmpInt;
                }

                // Tipo Apparato
                if (cmbInitTipoApparato.SelectedValue == null)
                {
                    // apparato non selezionato
                }
                else
                {
                    _llModelloCb CbAttivo = (_llModelloCb)cmbInitTipoApparato.SelectedItem;
                    _cb.ParametriApparato.llParApp.TipoApparato = (byte)CbAttivo.IdModelloLL;
                    _cb.ParametriApparato.llParApp.FamigliaApparato = (byte)CbAttivo.FamigliaCaricabetteria;

                }

                // Data
                byte[] dataInit = FunzioniMR.toArrayDataTS(txtInitDataInizializ.Text);
                uint DataUint = dataInit[0];
                DataUint = (DataUint << 8) + dataInit[1];
                DataUint = (DataUint << 8) + dataInit[2];
                _cb.ParametriApparato.llParApp.DataSetupApparato = DataUint;

                // Seriale scheda ZVT

                // Seriale scheda PFC


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

                // Rev SW DISP - MANTENGO IL DEFAULT
                // _cb.ParametriApparato.llParApp.SoftwareDISP = txtInitRevFwDISP.Text;

                // Rev HW DISP
                _cb.ParametriApparato.llParApp.HardwareDisp = txtInitRevHwDISP.Text;

                _cb.ParametriApparato.llParApp.IdApparato = txtInitIDApparato.Text;

                // Presenza modulo rabboccatore
                if (chkInitPresenzaRabb.Checked)
                {
                    _cb.ParametriApparato.llParApp.PresenzaRabboccatore = 0xF0;
                }
                else
                {
                    _cb.ParametriApparato.llParApp.PresenzaRabboccatore = 0x0F;
                }


                // Tensioni e corrente max apparato
                _cb.ParametriApparato.llParApp.VMin = FunzioniMR.ConvertiUshort(txtInitVMin.Text, 100, 0);
                _cb.ParametriApparato.llParApp.VMax = FunzioniMR.ConvertiUshort(txtInitVMax.Text, 100, 0);

                _cb.ParametriApparato.llParApp.Amax = FunzioniMR.ConvertiUshort(txtInitAMax.Text, 10, 0);


                _cb.ParametriApparato.llParApp.NumeroModuli = (byte)txtInitBrdNumModuli.Value;
                _cb.ParametriApparato.llParApp.ModVNom = FunzioniMR.ConvertiUshort((double)txtInitBrdVNomModulo.Value, 100, 0);
                _cb.ParametriApparato.llParApp.ModANom = FunzioniMR.ConvertiUshort((double)txtInitBrdANomModulo.Value, 10, 0);
                _cb.ParametriApparato.llParApp.ModOpzioni = FunzioniMR.ConvertiUshort(txtInitBrdOpzioniModulo.Text, 1, 0); 
                _cb.ParametriApparato.llParApp.ModVMin = FunzioniMR.ConvertiUshort(txtInitBrdVMinModulo.Text, 100, 0);
                _cb.ParametriApparato.llParApp.ModVMax= FunzioniMR.ConvertiUshort(txtInitBrdVMaxModulo.Text, 100, 0);
                //_cb.ParametriApparato.llParApp.TipoApparato = (byte)CaricaBatteria.TipoCaricaBatteria.SuperCharger;

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

                byte _annoMatr = 18;

                uint matricola = 0;

                if (byte.TryParse(txtInitAnnoMatricola.Text, out _annoMatr))
                {
                    _annoMatr = (byte)(_annoMatr & 0x3F);
                    _annoMatr = (byte)(_annoMatr << 2);

                }

                matricola = (uint)(_annoMatr << 16);

                uint _numMatr = 0;

                if (uint.TryParse(txtInitNumeroMatricola.Text, out _numMatr))
                {
                    _numMatr = (uint)(_numMatr & 0x0003FFFF);

                }

                matricola += _numMatr;



                txtInitSerialeApparato.Text = matricola.ToString("X6");
                string _tempMatricola = "SC" + txtInitSerialeApparato.Text;
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

                txtInitNumSerDISP.Text = "";
                txtInitRevHwDISP.Text = "";
                txtInitRevFwDISP.Text = "";

                txtInitVMin.Text = "";
                txtInitVMax.Text = "";
                txtInitAMax.Text = "";

                chkInitPresenzaRabb.Checked = false;

                txtInitBrdNumModuli.Value = 1;
                txtInitBrdVNomModulo.Value = 24;
                _inLettura = true;

                _esito = _cb.LeggiParametriApparato();

                if (_esito)
                {
                    txtInitManufactured.Text = _cb.ParametriApparato.llParApp.ProduttoreApparato;
                    txtInitProductId.Text = _cb.ParametriApparato.llParApp.NomeApparato;

                    if (_cb.ParametriApparato.llParApp.IdApparato != "????????" && _cb.ParametriApparato.llParApp.IdApparato != "")
                    {
                        txtInitDataInizializ.Text = FunzioniMR.StringaDataTS(_cb.ParametriApparato.llParApp.DataSetupApparato);
                        txtInitAnnoMatricola.Text = _cb.ParametriApparato.llParApp.AnnoCodice.ToString("00");
                        txtInitNumeroMatricola.Text = _cb.ParametriApparato.llParApp.ProgressivoCodice.ToString("000000").ToUpper();
                        txtInitSerialeApparato.Text = _cb.ParametriApparato.llParApp.SerialeApparato.ToString("x6").ToUpper();
                        txtInitIDApparato.Text = _cb.ParametriApparato.llParApp.IdApparato;

                        //cmbInitTipoApparato.SelectedValue = _cb.ParametriApparato.llParApp.TipoApparato;
                        cmbInitTipoApparato.SelectedItem = cmbInitTipoApparato.Items.OfType<_llModelloCb>().First(f => f.IdModelloLL == _cb.ParametriApparato.llParApp.TipoApparato);

                        txtInitVMin.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.VMin);
                        txtInitVMax.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.VMax);
                        txtInitAMax.Text = FunzioniMR.StringaCorrenteLL(_cb.ParametriApparato.llParApp.Amax);

                    }
                    if (_cb.ParametriApparato.llParApp.SerialeDISP != null)
                    {
                        txtInitNumSerDISP.Text = FunzioniComuni.HexdumpArray(_cb.ParametriApparato.llParApp.SerialeDISP);
                        txtInitRevHwDISP.Text = _cb.ParametriApparato.llParApp.HardwareDisp;
                        txtInitRevFwDISP.Text = _cb.ParametriApparato.llParApp.SoftwareDISP;
                    }

                    txtInitRevFwDISP.Text = _cb.ParametriApparato.llParApp.SoftwareDISP;

                    if (_cb.ParametriApparato.llParApp.PresenzaRabboccatore == 0xF0)
                    {
                        chkInitPresenzaRabb.Checked = true;
                    }

                    txtInitMaxBrevi.Text = _cb.ParametriApparato.llParApp.MaxRecordBrevi.ToString();
                    txtInitMaxLunghi.Text = _cb.ParametriApparato.llParApp.MaxRecordCarica.ToString();
                    txtInitMaxProg.Text = _cb.ParametriApparato.llParApp.MaxProgrammazioni.ToString();
                    txtInitModelloMemoria.Text = _cb.ParametriApparato.llParApp.ModelloMemoria.ToString();

                    // Tensioni e corrente max apparato

                    if (_cb.ParametriApparato.llParApp.NumeroModuli != 0xFF)
                    {
                        txtInitBrdNumModuli.Value = _cb.ParametriApparato.llParApp.NumeroModuli;
                        txtInitBrdVNomModulo.Value = _cb.ParametriApparato.llParApp.ModVNom / 100;
                        txtInitBrdANomModulo.Value = _cb.ParametriApparato.llParApp.ModANom / 10;

                        txtInitBrdVMinModulo.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.ModVMin);
                        txtInitBrdVMaxModulo.Text = FunzioniMR.StringaTensione(_cb.ParametriApparato.llParApp.ModVMax);
                    }
                }

                return _esito;

            }
            catch (Exception Ex)
            {
                Log.Error("SalvaInizializzazione: " + Ex.Message);
                _inLettura = false;
                return false;

            }
        }


        private void txtInitNumLottoZVT_Leave(object sender, EventArgs e)
        {
            try
            {
/*
                txtInitNumSerZVT.Text = "";


                // se cambiato
                if (FunzioniMR.VerificaStringaLottoZVT(txtInitNumLottoZVT.Text))
                {
                    byte[] _tempByte;
                    txtInitNumLottoZVT.Text = txtInitNumLottoZVT.Text.ToUpper();

                    _tempByte = FunzioniMR.CodificaStringaLottoZVT(txtInitNumLottoZVT.Text);

                    if (_tempByte != null)
                    {
                        txtInitNumSerZVT.Text = FunzioniComuni.HexdumpArray(_tempByte, false) + "0A0A";
                    }


                }
                else
                {
                    txtInitNumSerZVT.Text = "";
                }
*/
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
                if (!ProfiloInCaricamento)
                {

                    if (chkPaAttivaEqual.Checked)
                     {
                        ProfiloInCaricamento = true;


  
                        if (ModCicloCorrente.ModelloProfilo == null)
                        {
                            /// TODO: Ricarico il modello corrente --> ???? da verificare dove lo perde
                            ModCicloCorrente.ModelloProfilo = (from p in ModCicloCorrente.DatiModello.ParametriCarica
                                                               where p.BatteryTypeId == ModCicloCorrente.Batteria.BatteryTypeId && p.IdProfiloCaricaLL == ModCicloCorrente.Profilo.IdProfiloCaricaLL
                                                               select p).FirstOrDefault();

                        }



                        ModCicloCorrente.CalcolaEqualizzazione(0x0F0F, ModCicloCorrente.Tensione, ModCicloCorrente.Capacita, _cb.ModelloCorrente);
                        AssegnaEqualCCorrente();
                        //MostraEqualCCorrente();
                        btnPaSalvaDati.Enabled = true;
                        ProfiloInCaricamento = false;

                    }
                    else
                    {
                        ProfiloInCaricamento = true;
                        ModCicloCorrente.CalcolaEqualizzazione(0,0,0, null);
                        AssegnaEqualCCorrente();
                        MostraEqualCCorrente();
                        btnPaSalvaDati.Enabled = true;
                        ProfiloInCaricamento = false;

                    }
                }
            }
            catch (Exception Ex)
            {
                Log.Error("chkPaAttivaEqual_CheckedChanged: " + Ex.Message);

            }

        }

        private void chkPaAttivaMant_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ProfiloInCaricamento)
                {

                    if (chkPaAttivaMant.Checked)
                    {
                        ProfiloInCaricamento = true;



                        if (ModCicloCorrente.ModelloProfilo == null)
                        {
                            /// TODO: Ricarico il modello corrente --> ???? da verificare dove lo perde
                            ModCicloCorrente.ModelloProfilo = (from p in ModCicloCorrente.DatiModello.ParametriCarica
                                                               where p.BatteryTypeId == ModCicloCorrente.Batteria.BatteryTypeId && p.IdProfiloCaricaLL == ModCicloCorrente.Profilo.IdProfiloCaricaLL
                                                               select p).FirstOrDefault();

                        }



                        ModCicloCorrente.CalcolaMantenimento(0x0F0F, ModCicloCorrente.Tensione, ModCicloCorrente.Capacita, _cb.ModelloCorrente);
                        AssegnaMantCCorrente();
                       // MostraMantCCorrente();
                        btnPaSalvaDati.Enabled = true;
                        ProfiloInCaricamento = false;

                    }
                    else
                    {
                        ProfiloInCaricamento = true;
                        ModCicloCorrente.CalcolaMantenimento(0, 0, 0, null);
                        AssegnaMantCCorrente();
                        //MostraEqualCCorrente();
                        btnPaSalvaDati.Enabled = true;
                        ProfiloInCaricamento = false;

                    }
                }
            }
            catch (Exception Ex)
            {
                Log.Error("chkPaAttivaMant_CheckedChanged: " + Ex.Message);

            }

        }

        private void ChkPaAttivaOppChg_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ProfiloInCaricamento)
                {

                    if (chkPaAttivaOppChg.Checked)
                    {
                        ProfiloInCaricamento = true;



                        if (ModCicloCorrente.ModelloProfilo == null)
                        {
                            /// TODO: Ricarico il modello corrente --> ???? da verificare dove lo perde
                            ModCicloCorrente.ModelloProfilo = (from p in ModCicloCorrente.DatiModello.ParametriCarica
                                                               where p.BatteryTypeId == ModCicloCorrente.Batteria.BatteryTypeId && p.IdProfiloCaricaLL == ModCicloCorrente.Profilo.IdProfiloCaricaLL
                                                               select p).FirstOrDefault();

                        }



                        ModCicloCorrente.CalcolaOpportunityChg(0x0F0F, ModCicloCorrente.Tensione, ModCicloCorrente.Capacita, _cb.ModelloCorrente);
                        AssegnaOppCCorrente();

                        btnPaSalvaDati.Enabled = true;
                        ProfiloInCaricamento = false;

                    }
                    else
                    {
                        ProfiloInCaricamento = true;
                        ModCicloCorrente.CalcolaOpportunityChg(0, 0, 0, null);
                        AssegnaOppCCorrente();

                        btnPaSalvaDati.Enabled = true;
                        ProfiloInCaricamento = false;

                    }
                }
            }
            catch (Exception Ex)
            {
                Log.Error("ChkPaAttivaOppChg_CheckedChanged: " + Ex.Message);

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
             if(_apparatoPresente) LeggiInizializzazione();
        }

        private void MascheraTabPages(int MaskLevel)
        {
            try
            {
                // Mask 0 Tutto acceso
                // Mask 1 lascio solo init


                foreach (TabPage _pag in tabCaricaBatterie.TabPages)
                {
                    if (MaskLevel == 1)
                    {
                        if (_pag.Name != "tabInizializzazione")
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
                bool _BatteriaValida = false;
                bool TempProfiloInCaricamento;
                ushort TipoBatt;
                mbTipoBatteria TempBatt = (mbTipoBatteria)cmbPaTipoBatteria.SelectedItem;

                if (TempBatt == null)
                {
                    // Nessuna batteria attiva
                }
                else
                {
                    // ProfiloInCaricamento = true;

                    btnPaProfileRefresh.ForeColor = Color.Red;

                    // Carico i profili in base al tipo
                    TipoBatt = (ushort)TempBatt.BatteryTypeId;

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
                        if (TempBatt.StandardChargeProfile == pc.IdProfiloCaricaLL)
                        {
                            profiloDefault = pc;
                            _BatteriaValida = true;
                            //break;
                        }
                    }

                    TempProfiloInCaricamento = ProfiloInCaricamento;
                    ProfiloInCaricamento = true;

                    cmbPaProfilo.DataSource = ProfiliCarica;
                    cmbPaProfilo.ValueMember = "IdProfiloCaricaLL";
                    cmbPaProfilo.DisplayMember = "NomeProfilo";
                    cmbPaProfilo.SelectedItem = null;

                    ProfiloInCaricamento = TempProfiloInCaricamento;

                    if (_BatteriaValida)
                    {
                        ProfiloInCaricamento = true;
                        cmbPaProfilo.SelectedItem = profiloDefault;
                        ProfiloInCaricamento = TempProfiloInCaricamento;
                        // Carico le tensioni in base al tipo

                        _llParametriApparato TipoLL = _cb.ParametriApparato.llParApp;

                        if (TempBatt.DivisoreCelle == 0 ) // Cella unica
                        {
                            txtPaNumCelle.Visible = false;
                            lblPaNumCelle.Visible = false;
                            txtPaNumCelle.Text = "1";
                        }
                        else
                        {
                            txtPaNumCelle.Visible = true;
                            lblPaNumCelle.Visible = true;
                        }


                        if (TempBatt.TensioniFisse == 0 || TipoLL == null)
                        {
                            // Textbox
                            txtPaTensione.Visible = true;
                            txtPaTensione.Enabled = true;
                            txtPaTensione.Text = FunzioniMR.StringaTensione(TipoLL.VMin);
                            cmbPaTensione.Visible = false;

                        }
                        else
                        {
                            txtPaTensione.Visible = false;
                            cmbPaTensione.Visible = true;

                            byte TipoApp = TipoLL.TipoApparato;

                            var Listatens = from t in _cb.DatiBase.TensioniBatteria
                                            join tm in _cb.DatiBase.TensioniModello on t.IdTensione equals tm.IdTensione
                                            where tm.IdModelloLL == TipoApp
                                            select t;

                            TensioniBatteria = new List<llTensioneBatteria>();
                            foreach (var t in Listatens)
                            {
                                TensioniBatteria.Add((llTensioneBatteria)t);
                            }

                            cmbPaTensione.SelectedItem = null;

                            cmbPaTensione.DataSource = TensioniBatteria;
                            cmbPaTensione.ValueMember = "IdTensione";
                            cmbPaTensione.DisplayMember = "Descrizione";
                            cmbPaTensione.Visible = true;
                            txtPaTensione.Visible = false;

                        }
                        tbcPaSchedeValori.Visible = true;

                        // In ultimo precarico i valori fissi e lancio il ricalcolo
                        if (!ProfiloInCaricamento)
                        {
                            txtPaSogliaVs.Text = FunzioniMR.StringaTensioneCella(TempBatt.VoltSoglia, true);
                            txtPaVMax.Text = FunzioniMR.StringaTensioneCella(TempBatt.VCellaMax, true);
                            txtPaVMinRic.Text = FunzioniMR.StringaTensioneCella(TempBatt.VminRiconoscimento, true);
                            txtPaVMaxRic.Text = FunzioniMR.StringaTensioneCella(TempBatt.VmaxRiconoscimento, true);

                            bool esitoRicalcolo;
                            //DefinisciValoriProfilo();
                            esitoRicalcolo = RicalcolaParametriCiclo(false);
                            MostraParametriCiclo(false, !esitoRicalcolo, chkPaSbloccaValori.Checked);

                        }
                    }
                    else
                    {
                        // Non ho una batteria valida definita. vuoto gli altri parametri

                        cmbPaProfilo.DataSource = null;

                        // Carico le tensioni in base al tipo

                        cmbPaTensione.SelectedItem = null;
                        cmbPaTensione.DataSource = null;

                        cmbPaTensione.Visible = true;
                        txtPaTensione.Visible = false;

                        txtPaNumCelle.Text = "";
                        txtPaCapacita.Text = "";

                        chkPaAttivaEqual.Checked = false;
                        chkPaAttivaMant.Checked = false;
                        chkPaAttivaOppChg.Checked = false;
                        chkPaUsaSpyBatt.Checked = false;
                        chkPaUsaSafety.Checked = false;
                        tbcPaSchedeValori.Visible = false;
                        ModCicloCorrente.ValoriCiclo.AzzeraValori();
                        ModCicloCorrente.Batteria = TempBatt;
                        ModCicloCorrente.Capacita = 0;
                        ModCicloCorrente.NumeroCelle = 0;
                        ModCicloCorrente.Tensione = 0;

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

                btnPaProfileRefresh.ForeColor = Color.Red;
                if (cmbPaProfilo.SelectedItem == null)
                {

                    cmbPaDurataCarica.SelectedItem = null;

                    picPaImmagineProfilo.BackColor = Color.LightGray;
                    picPaImmagineProfilo.Image = null;
                }
                else
                {
                    TipoProf = (byte)((_mbProfiloCarica)cmbPaProfilo.SelectedItem).IdProfiloCaricaLL;

                    if ((_mbProfiloCarica)cmbPaProfilo.SelectedItem != null)
                    {
                        string Grafico = (string)((_mbProfiloCarica)cmbPaProfilo.SelectedItem).Grafico;
                        if (Grafico == "")
                        {
                            picPaImmagineProfilo.BackColor = Color.LightGray;
                            picPaImmagineProfilo.Image = null;

                        }
                        else
                        {
                            ResourceManager rm = Resources.profili.ModelliProfilo.ResourceManager;
                            Bitmap myImage = (Bitmap)rm.GetObject(Grafico);
                            picPaImmagineProfilo.BackColor = Color.White;
                            picPaImmagineProfilo.Image = myImage;

                        }
                    }

                    var Listatemp = from p in _cb.DatiBase.DurateCarica
                                    join pt in _cb.DatiBase.DurateProfilo on p.IdDurataCaricaLL equals pt.IdDurataCaricaLL
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
                    if (!ProfiloInCaricamento)
                    {
                        txtPaNomeSetup.Text = (string)((_mbProfiloCarica)cmbPaProfilo.SelectedItem).NomeProfilo;
                    }

                    ModoProf = (byte)((_mbProfiloCarica)cmbPaProfilo.SelectedItem).AttivaEqual;
                    switch (ModoProf)
                    {
                        case 0x00:
                            chkPaAttivaEqual.Checked = false;
                            chkPaAttivaEqual.Enabled = false;
                            //lblPaAttivaEqual.Enabled = false;
                            break;
                        case 0xF0:
                            chkPaAttivaEqual.Checked = false;
                            chkPaAttivaEqual.Enabled = true;
                            //lblPaAttivaEqual.Enabled = true;
                            break;
                        case 0x0F:
                            chkPaAttivaEqual.Checked = true;
                            chkPaAttivaEqual.Enabled = true;
                            //lblPaAttivaEqual.Enabled = true;
                            break;
                        case 0xFF:
                            chkPaAttivaEqual.Checked = true;
                            chkPaAttivaEqual.Enabled = false;
                            //chkPaAttivaEqual.Enabled = false;

                            break;
                        default:
                            chkPaAttivaEqual.Checked = false;
                            chkPaAttivaEqual.Enabled = false;
                            //chkPaAttivaEqual.Enabled = false;
                            break;

                    }

                    ModoProf = (byte)((_mbProfiloCarica)cmbPaProfilo.SelectedItem).AttivaMant;
                    switch (ModoProf)
                    {
                        case 0x00:
                            chkPaAttivaMant.Checked = false;
                            chkPaAttivaMant.Enabled = false;
                            break;
                        case 0xF0:
                            chkPaAttivaMant.Checked = false;
                            chkPaAttivaMant.Enabled = true;
                            break;
                        case 0xFF:
                            chkPaAttivaMant.Checked = true;
                            chkPaAttivaMant.Enabled = false;
                            break;
                        default:
                            chkPaAttivaMant.Checked = false;
                            chkPaAttivaMant.Enabled = false;
                            break;

                    }
                    ModoProf = (byte)((_mbProfiloCarica)cmbPaProfilo.SelectedItem).AttivaOpportunity;
                    switch (ModoProf)
                    {
                        case 0x00:
                            chkPaAttivaOppChg.Checked = false;
                            chkPaAttivaOppChg.Enabled = false;
                            break;
                        case 0xF0:
                            chkPaAttivaOppChg.Checked = false;
                            chkPaAttivaOppChg.Enabled = true;
                            break;
                        case 0xFF:
                            chkPaAttivaOppChg.Checked = true;
                            chkPaAttivaOppChg.Enabled = false;
                            break;
                        default:
                            chkPaAttivaOppChg.Checked = false;
                            chkPaAttivaOppChg.Enabled = false;
                            break;

                    }


                    ModoProf = (byte)((_mbProfiloCarica)cmbPaProfilo.SelectedItem).AttivaRiarmoPulse;
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
                    if (!ProfiloInCaricamento)
                    {

                        bool esitoRicalcolo;
                        //DefinisciValoriProfilo();
                        esitoRicalcolo = RicalcolaParametriCiclo();
                        MostraParametriCiclo(false, !esitoRicalcolo);

                    }

                }

            }
            catch (Exception Ex)
            {
                Log.Error("cmbPaProfilo_SelectedIndexChanged: " + Ex.Message);
            }


        }

        private void cmbPaDurataCarica_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //byte DurataF2;
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



                // if (cmbPaDurataCarica.SelectedItem == null)
                // {
                txtPaTempoT2Min.Text = "60";
                txtPaTempoT2Max.Text = "210";
                // }
                // else
                // {

                //     DurataF2 = (byte)((llDurataCarica)cmbPaDurataCarica.SelectedItem).DurataFaseDue(TipoBatt);
                //     txtPaTempoT2Min.Text = DurataF2.ToString();
                // }

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
                btnPaProfileRefresh.ForeColor = Color.Red;
                txtPaTensione.Text = ((llTensioneBatteria)cmbPaTensione.SelectedItem).Descrizione;
                if (!ProfiloInCaricamento)
                {
                    if (cmbPaTipoBatteria.SelectedItem == null)
                    {
                        txtPaNumCelle.Text = "1";
                    }
                    else
                    {
                        if (cmbPaTipoBatteria.SelectedItem == null)
                        {
                            txtPaNumCelle.Text = "1";
                        }
                        else
                        {
                            mbTipoBatteria TempBatt = (mbTipoBatteria)cmbPaTipoBatteria.SelectedItem;
                            if (TempBatt.DivisoreCelle == 0)
                            {
                                txtPaNumCelle.Text = "1";
                            }
                            else
                            {
                                txtPaNumCelle.Text = (((llTensioneBatteria)cmbPaTensione.SelectedItem).IdTensione / TempBatt.DivisoreCelle).ToString("0");
                            }


                        }
                    }
                }

            }

            else
            {
                txtPaTensione.Text = "";
                txtPaNumCelle.Text = "1";
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
                Font _carTesto = new Font("Tahoma", 10, FontStyle.Regular);


                flwPaListaConfigurazioni.HeaderUsesThemes = false;
                flwPaListaConfigurazioni.HeaderFormatStyle = _stile;
                flwPaListaConfigurazioni.UseAlternatingBackColors = true;
                flwPaListaConfigurazioni.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flwPaListaConfigurazioni.AllColumns.Clear();

                flwPaListaConfigurazioni.View = View.Details;
                flwPaListaConfigurazioni.ShowGroups = false;
                flwPaListaConfigurazioni.GridLines = true;
                flwPaListaConfigurazioni.Font = _carTesto;
                flwPaListaConfigurazioni.RowHeight = 25;
                flwPaListaConfigurazioni.FullRowSelect = true;

                BrightIdeasSoftware.OLVColumn colIdConfig = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "ID",
                    AspectName = "strIdProgramma",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colIdConfig);

                BrightIdeasSoftware.OLVColumn colPosizione = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Pos",
                    AspectName = "strPosizioneCorrente",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colPosizione);

                BrightIdeasSoftware.OLVColumn colSpyBatt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "SB",
                    AspectName = "strAbilitaComunicazioneSpybatt",
                    Width = 40,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.OK_16, "NO", Properties.Resources.GRAY_16 })

                };
                flwPaListaConfigurazioni.AllColumns.Add(colSpyBatt);

                

                BrightIdeasSoftware.OLVColumn colProgName = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Nome",
                    AspectName = "ProgramName",
                    Width = 100,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,

                };
                flwPaListaConfigurazioni.AllColumns.Add(colProgName);


                BrightIdeasSoftware.OLVColumn colBatteryType = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Tipo Batt.",
                    AspectName = "strTipoBatteria",
                    Width = 300,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colBatteryType);


                BrightIdeasSoftware.OLVColumn colNomeProfilo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Profilo",
                    ToolTipText = "Nome Profilo",
                    AspectName = "strTipoProfilo",
                    Width = 150,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colNomeProfilo);

                BrightIdeasSoftware.OLVColumn colRowBattVNom = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "V",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strBatteryVdef",
                    Width = 120,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Right,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colRowBattVNom);

                BrightIdeasSoftware.OLVColumn colRowBattAhNom = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Ah",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strBatteryAhdef",
                    Width = 120,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Right,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colRowBattAhNom);



                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flwPaListaConfigurazioni.AllColumns.Add(colRowFiller);

                flwPaListaConfigurazioni.Sort(colPosizione);
                flwPaListaConfigurazioni.RebuildColumns();
                flwPaListaConfigurazioni.SetObjects(_cb.Programmazioni.ProgrammiDefiniti);
                flwPaListaConfigurazioni.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }

        private void flwPaListaConfigurazioni_FormatRow(object sender, FormatRowEventArgs e)
        {
            //coloro la riga in base al Tipo Evento
            llProgrammaCarica _progCorrente = (llProgrammaCarica)e.Model;
            if (_progCorrente.ProgrammaAttivo)
            {
                e.Item.BackColor = Color.LightGreen;
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

                LeggiMemoriaCicli();


            }
            catch (Exception Ex)
            {
                Log.Error("btnCicliCaricaLista_Click: " + Ex.Message);
            }
        }

        public bool LeggiMemoriaCicli(bool LeggiBrevi = false)
        {
            try
            {
                uint _StartAddr;
                ushort _NumByte;

                if (uint.TryParse(txtCicliAddrPrmo.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _StartAddr) != true)
                {
                    _StartAddr = 0x1B4000;
                }

                txtCicliAddrPrmo.Text = _StartAddr.ToString("X6");

                if (txtCicliNumRecord.Text == "-1")
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
                    }

                }

                txtCicliNumRecord.Text = _NumByte.ToString();
                
                CaricaListaCariche(_StartAddr, _NumByte);
                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("btnCicliCaricaLista_Click: " + Ex.Message);
                return false;
            }
        }

        private void frmCaricabatterie_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // Chiudo la connessione
                if (_cb.StopComunicazione())
                {
                    //_cb.chiudiPorta();

                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmCaricabatterie_FormClosed: " + Ex.Message);
            }
        }

        private void btnSalvaCaricabatteria_Click(object sender, EventArgs e)
        {

        }

        private void flvCicliListaCariche_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                llMemoriaCicli CicloSel;
                if (flvCicliListaCariche.SelectedObject == null)
                {
                    btnCicliCaricaBrevi.Enabled = false;
                    btnCicliMostraBrevi.Enabled = false;
                }
                else
                {
                    CicloSel = (llMemoriaCicli)flvCicliListaCariche.SelectedObject;

                    if (CicloSel.NumEventiBrevi > 0) btnCicliCaricaBrevi.Enabled = true;
                    if (CicloSel.NumEventiBreviCaricati > 0) btnCicliMostraBrevi.Enabled = true;
                }

            }
            catch (Exception Ex)
            {
                Log.Error("flvCicliListaCariche_SelectedIndexChanged: " + Ex.Message);
            }
        }

        private void btnCicliCaricaBrevi_Click(object sender, EventArgs e)
        {
            try
            {
                //llMemoriaCicli CicloSel;
                if (flvCicliListaCariche.SelectedObject == null)
                {
                    btnCicliCaricaBrevi.Enabled = false;
                    btnCicliMostraBrevi.Enabled = false;
                }
                else
                {

                    CicloCorrente = (llMemoriaCicli)flvCicliListaCariche.SelectedObject;

                    if (CicloCorrente.CicliMemoriaBreve.Count < CicloCorrente.NumEventiBrevi)
                    {
                        // non ho tutto in memoria, ricarico
                        CicloCorrente.CicliMemoriaBreve = CaricaListaBrevi(CicloCorrente.PuntatorePrimoBreve, (ushort)CicloCorrente.NumEventiBrevi, CicloCorrente.IdMemoriaLunga);
                    }

                    MostraDettaglioRiga(CicloCorrente);


                }

            }
            catch (Exception Ex)
            {
                Log.Error("flvCicliListaCariche_SelectedIndexChanged: " + Ex.Message);
            }
        }

        private void MostraDettaglioRiga(llMemoriaCicli CicloSel)
        {
            try
            {
                Log.Debug("MostraDettaglioRiga");

                if (CicloSel != null)
                {
                    Log.Debug("MostraDettaglioRiga LL Start");
                    frmListaCicliBreviLL CicliBreviLL = new frmListaCicliBreviLL();
                    CicliBreviLL.MdiParent = this.MdiParent;
                    CicliBreviLL.StartPosition = FormStartPosition.CenterParent;
                    CicliBreviLL.parametri = _parametri;

                    CicliBreviLL.CicloCorrente = CicloSel;
                    CicliBreviLL.Show();
                    CicliBreviLL.MostraCicli();

                }
            }
            catch (Exception Ex)
            {
                Log.Error("MostraDettaglioRiga: " + Ex.Message);
            }

        }

        private void cmbInitTipoApparato_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {

                _llModelloCb TempMod = (_llModelloCb)cmbInitTipoApparato.SelectedItem;
                //if (_inLettura) return;

                if (TempMod == null)
                {
                    txtInitVMin.Text = "";
                    txtInitVMax.Text = "";
                    txtInitAMax.Text = "";
                }
                else
                {
                    txtInitVMin.Text = TempMod.TensioneMin.ToString("0.00");
                    txtInitVMax.Text = TempMod.TensioneMax.ToString("0.00");
                    txtInitAMax.Text = TempMod.CorrenteMax.ToString("0.0");

                    txtInitBrdVMinModulo.Text = TempMod.TensioneMin.ToString("0.00");
                    txtInitBrdVMaxModulo.Text = TempMod.TensioneMax.ToString("0.00");
                    if(TempMod.TensioneNominale >0)
                        txtInitBrdVNomModulo.Value = (decimal)TempMod.TensioneNominale;
                    txtInitBrdOpzioniModulo.Text = "0";

                }


            }
            catch (Exception Ex)
            {
                Log.Error("cmbInitTipoApparato_SelectedIndexChanged: " + Ex.Message);
            }
        }

        private void rbtMemParametriInit_Click(object sender, EventArgs e)
        {
            if (rbtMemParametriInit.Checked)
            {
                txtMemCFStartAdd.Text = "000000";
                txtMemCFBlocchi.Text = "1";
            }
        }

        private void rbtMemAreaLibera_Click(object sender, EventArgs e)
        {
            if (rbtMemAreaLibera.Checked)
            {
                txtMemCFStartAdd.Text = "0";
                txtMemCFBlocchi.Text = "1";
            }
        }

        private void btnMemResetBoard_Click(object sender, EventArgs e)
        {
            try
            {



            }
            catch (Exception Ex)
            {
                Log.Error("btnMemResetBoard_Click: " + Ex.Message);
            }
        }

        private void tbpPaProfiloAttivo_Click(object sender, EventArgs e)
        {

        }

        private void cmbPaDurataCarica_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void btnPaProfileRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                bool esitoRicalcolo;
                //DefinisciValoriProfilo();
                esitoRicalcolo = RicalcolaParametriCiclo();
                MostraParametriCiclo(false, !esitoRicalcolo, chkPaSbloccaValori.Checked);

            }

            catch (Exception Ex)
            {
                Log.Error("btnPaProfileRefresh_Click: " + Ex.Message);
            }
        }

        public bool DefinisciValoriProfilo()
        {
            try
            {
                sbTipoBatteria TempBatt = (sbTipoBatteria)cmbPaTipoBatteria.SelectedItem;
                _llProfiloCarica TempProfilo = (_llProfiloCarica)cmbPaProfilo.SelectedItem;
                ushort TempVbat = FunzioniMR.ConvertiUshort(txtPaTensione.Text, 100, 0);
                byte TempNumCelle = FunzioniMR.ConvertiByte(txtPaNumCelle.Text, 1, 1);
                ushort TempCap = FunzioniMR.ConvertiUshort(txtPaCapacita.Text, 10, 0);



                btnPaSalvaDati.Enabled = false;

                if (TempBatt == null || TempProfilo == null || TempVbat == 0 || TempCap == 0)
                {
                    return false;
                }

                // Comincio a impostare i parametri. se trovo qualcosa che non mi permette di effettuare la carica cambio colore alla cella e mi fermo.
                // se sono quì è tutto settato !



                return true;
            }

            catch (Exception Ex)
            {
                Log.Error("DefinisciValoriProfilo: " + Ex.Message);
                return false;
            }
        }

        public bool VuotaValoriProfilo()
        {
            try
            {
                // Ripristino le eventuali schede nascoste 

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("DefinisciValoriProfilo: " + Ex.Message);
                return false;
            }

        }

        private void txtPaCapacita_Leave(object sender, EventArgs e)
        {
            try
            {
                bool esitoRicalcolo;
                //DefinisciValoriProfilo();
                esitoRicalcolo = RicalcolaParametriCiclo();
                MostraParametriCiclo(false, !esitoRicalcolo, chkPaSbloccaValori.Checked);

            }
            catch (Exception Ex)
            {
                Log.Error("txtPaCapacita_Leave: " + Ex.Message);
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
                _llModelloCb _ModelloCB = _cb.ModelloCorrente;

                string messaggioErrore;
                //  se non ho i parametri essenziali, Batt, tensione , capacità e profilo non tento nemmeno il calcolo
                if (_Batteria == null) return false;
                if (_Profilo == null) return false;
                if (_Tensione == 0) return false;
                if (_Capacita == 0) return false;
                if (_Celle == 0) return false;
                if (_ModelloCB == null) return false;

                CaricaBatteria.EsitoRicalcolo esitoRicalcolo = ModCicloCorrente.CalcolaParametri(_Batteria._mbTb, _Profilo, _Tensione, _Capacita, _Celle, _ModelloCB);

                btnPaProfileRefresh.ForeColor = Color.Black;
                //MostraParametriCiclo(!esitoRicalcolo);

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
                TabPage TempPagina;

                // Memorizzo la scheda di partenza
                //TempPagina = tbcPaSchedeValori.ActiveTab;



                /// TODO:  Ripristino le eventuali schede nascoste 
                if (ModCicloCorrente.Batteria == null)
                {
                    // SoloClear = true;
                }


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

                    MostraEqualCCorrente();

                }
                else
                {
                    if (ParametriBase)
                    {
                        txtPaNomeSetup.Text = ModCicloCorrente.NomeProfilo;
                        txtPaIdSetup.Text = ModCicloCorrente.IdProgramma.ToString();
                        txtPaCassone.Text = ModCicloCorrente.ValoriCiclo.TipoCassone.ToString();
                        // Allineo il tipo batteria

                        mbTipoBatteria TempBatt = (from tb in (List<mbTipoBatteria>)cmbPaTipoBatteria.DataSource
                                                   where tb.BatteryTypeId == ModCicloCorrente.Batteria.BatteryTypeId
                                                   select tb).FirstOrDefault();
                        cmbPaTipoBatteria.SelectedItem = TempBatt;

                        if (ModCicloCorrente.Batteria != null)
                        {
                            if (ModCicloCorrente.Batteria.TensioniFisse != 0)
                            {
                                llTensioneBatteria TensBatt = (from tb in (List<llTensioneBatteria>)cmbPaTensione.DataSource
                                                               where tb.IdTensione == ModCicloCorrente.Tensione
                                                               select tb).FirstOrDefault();
                                cmbPaTensione.SelectedItem = TensBatt;
                            }

                            FunzioniUI.ImpostaTextBoxUshort(ref txtPaTensione, ModCicloCorrente.Tensione, 1, 1, SbloccaValori, true);
                            txtPaTensione.Visible = true;
                        }

                        if (ModCicloCorrente.Profilo != null)
                        {
                            if (cmbPaProfilo.DataSource != null)
                            {
                                _mbProfiloCarica TempProf = (from tp in (List<_mbProfiloCarica>)cmbPaProfilo.DataSource
                                                             where tp.IdProfiloCaricaLL == ModCicloCorrente.Profilo.IdProfiloCaricaLL
                                                             select tp).FirstOrDefault();
                                cmbPaProfilo.SelectedItem = TempProf;
                            }
                            else
                                cmbPaProfilo.SelectedItem = null;


                        }


                        chkPaUsaSpyBatt.Checked = (ModCicloCorrente.ValoriCiclo.AbilitaSpyBatt == 0x000F);
                        chkPaUsaSafety.Checked = (ModCicloCorrente.ValoriCiclo.AbilitaSafety == 0x000F);


                        //cmbPaProfilo

                    }
                    byte ModoProf;



                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaNumCelle, ModCicloCorrente.NumeroCelle, 4, 3, SbloccaValori, true);
                    if (!SkipCapacità)
                    {
                        // se sono entrato da txtCapacita_change evito di riformattare
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaCapacita, ModCicloCorrente.Capacita, 5, 2, SbloccaValori, true);
                    }


                    // FASE 0 (Preciclo)
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaSogliaV0, ModCicloCorrente.ValoriCiclo.TensionePrecicloV0, ModCicloCorrente.ParametriAttivi.TensionePrecicloV0, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaPrefaseI0, ModCicloCorrente.ValoriCiclo.CorrenteI0, ModCicloCorrente.ParametriAttivi.CorrenteI0, 2, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaDurataMaxT0, ModCicloCorrente.ValoriCiclo.TempoT0Max, ModCicloCorrente.ParametriAttivi.TempoT0Max, 3, SbloccaValori);
                    // Se I0, V0 e T0 sono a 0 con flag " " spengo il tab del preciclo
                    if (ModCicloCorrente.ValoriCiclo.Fase0Attiva && ModCicloCorrente.ParametriAttivi.Fase0Attiva )
                    {
                        //tbcPaSchedeValori.ShowTab("tbpPaPCStep0");
                    }
                    else
                    {
                        //tbcPaSchedeValori.HideTab("tbpPaPCStep0");
                    }

                    // FASE 1 - Sempre visibile
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaSogliaVs, ModCicloCorrente.ValoriCiclo.TensioneSogliaVs, ModCicloCorrente.ParametriAttivi.TensioneSogliaVs, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCorrenteI1, ModCicloCorrente.ValoriCiclo.CorrenteI1, ModCicloCorrente.ParametriAttivi.CorrenteI1, 2, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref cmbPaDurataMaxT1, ModCicloCorrente.ValoriCiclo.TempoT1Max, ModCicloCorrente.ParametriAttivi.TempoT1Max, 3, SbloccaValori);

                    // FASE 2 - Sempre visibile
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaRaccordoF1, ModCicloCorrente.ValoriCiclo.TensioneRaccordoVr, ModCicloCorrente.ParametriAttivi.TensioneRaccordoVr, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCorrenteRaccordo, ModCicloCorrente.ValoriCiclo.CorrenteRaccordoIr, ModCicloCorrente.ParametriAttivi.CorrenteRaccordoIr, 2, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCorrenteF3, ModCicloCorrente.ValoriCiclo.CorrenteFinaleI2, ModCicloCorrente.ParametriAttivi.CorrenteFinaleI2, 2, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaVMax, ModCicloCorrente.ValoriCiclo.TensioneMassimaVMax, ModCicloCorrente.ParametriAttivi.TensioneMassimaVMax, 1, SbloccaValori);

                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaTempoT2Min, ModCicloCorrente.ValoriCiclo.TempoT2Min, ModCicloCorrente.ParametriAttivi.TempoT2Min, 3, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaTempoT2Max, ModCicloCorrente.ValoriCiclo.TempoT2Max, ModCicloCorrente.ParametriAttivi.TempoT2Max, 3, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCoeffK, ModCicloCorrente.ValoriCiclo.FattoreK, ModCicloCorrente.ParametriAttivi.FattoreK, 3, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCoeffKc, ModCicloCorrente.ValoriCiclo.CoeffKc, ModCicloCorrente.ParametriAttivi.CoeffKc, 3, SbloccaValori);
                    
                    // FASE 3
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaTempoT3Max, ModCicloCorrente.ValoriCiclo.TempoT3Max, ModCicloCorrente.ParametriAttivi.TempoT3Max, 3, SbloccaValori);
                    if (ModCicloCorrente.ValoriCiclo.Fase3Attiva && ModCicloCorrente.ParametriAttivi.Fase3Attiva)
                    {
                        //tbcPaSchedeValori.ShowTab("tbpPaPCStep3");
                    }
                    else
                    {
                        //tbcPaSchedeValori.HideTab("tbpPaPCStep3");
                    }

                    // EQUALIZZAZIONE
                    if(ModCicloCorrente.Profilo != null)
                    {
                        ModoProf = (byte)ModCicloCorrente.Profilo.AttivaEqual;
                    }
                    else
                    {
                        ModoProf = 0;
                    }
                    //ModoProf = (byte)ModCicloCorrente.Profilo.AttivaEqual;
                    switch (ModoProf)
                    {
                        case 0x00:
                            ModCicloCorrente.ParametriAttivi.EqualAttivabile = 1;
                            break;
                        case 0xF0:
                            ModCicloCorrente.ParametriAttivi.EqualAttivabile = 4;
                            break;
                        case 0xFF:
                            ModCicloCorrente.ParametriAttivi.EqualAttivabile = 1;
                            break;
                        default:
                            ModCicloCorrente.ParametriAttivi.EqualAttivabile = 1;
                            break;

                    }
                    //ModCicloCorrente.ParametriAttivi.EqualAttivabile = 4;

                    if (ModCicloCorrente.ParametriAttivi.EqualAttivabile == 1)
                    {
                        //tbcPaSchedeValori.HideTab("tbpPaPCEqual");
                    }
                    else
                    {
                        //tbcPaSchedeValori.ShowTab("tbpPaPCEqual" + "");
                    }

                    FunzioniUI.ImpostaCheckBoxUshort(ref chkPaAttivaEqual, ref lblPaAttivaEqual, ModCicloCorrente.ValoriCiclo.EqualAttivabile, ModCicloCorrente.ParametriAttivi.EqualAttivabile, 3, SbloccaValori);
                    txtPaEqualAttesa.Text = "";
                    MostraEqualCCorrente();

                    if (true) // (chkPaAttivaEqual.Checked)
                    {
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaEqualAttesa, ModCicloCorrente.ValoriCiclo.EqualTempoAttesa, ModCicloCorrente.ParametriAttivi.EqualTempoAttesa, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaEqualNumPulse, ModCicloCorrente.ValoriCiclo.EqualNumImpulsi, ModCicloCorrente.ParametriAttivi.EqualNumImpulsi, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaEqualPulsePause, ModCicloCorrente.ValoriCiclo.EqualTempoPausa, ModCicloCorrente.ParametriAttivi.EqualTempoPausa, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaEqualPulseTime, ModCicloCorrente.ValoriCiclo.EqualTempoImpulso, ModCicloCorrente.ParametriAttivi.EqualTempoImpulso, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaEqualPulseCurrent, ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso, ModCicloCorrente.ParametriAttivi.EqualCorrenteImpulso, 2, SbloccaValori);
                    }

                    if (ModCicloCorrente.Profilo != null)
                    {
                        ModoProf = (byte)ModCicloCorrente.Profilo.AttivaMant;
                    }
                    else
                    {
                        ModoProf = 0;
                    }
                    switch (ModoProf)
                    {
                        case 0x00:
                            ModCicloCorrente.ParametriAttivi.MantAttivabile = 1;
                            break;
                        case 0xF0:
                            ModCicloCorrente.ParametriAttivi.MantAttivabile = 4;
                            break;
                        case 0xFF:
                            ModCicloCorrente.ParametriAttivi.MantAttivabile = 1;
                            break;
                        default:
                            ModCicloCorrente.ParametriAttivi.MantAttivabile = 1;
                            break;

                    }



                    FunzioniUI.ImpostaCheckBoxUshort(ref chkPaAttivaMant, ref lblPaAttivaMant, ModCicloCorrente.ValoriCiclo.MantAttivabile, ModCicloCorrente.ParametriAttivi.MantAttivabile, 3, SbloccaValori);

                   
                    if (true) // (chkPaAttivaMant.Checked)
                    {
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantAttesa, ModCicloCorrente.ValoriCiclo.MantTempoAttesa, ModCicloCorrente.ParametriAttivi.MantTempoAttesa, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmin, ModCicloCorrente.ValoriCiclo.MantTensIniziale, ModCicloCorrente.ParametriAttivi.MantTensIniziale, 1, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmax, ModCicloCorrente.ValoriCiclo.MantTensFinale, ModCicloCorrente.ParametriAttivi.MantTensFinale, 1, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantDurataMax, ModCicloCorrente.ValoriCiclo.MantTempoMaxErogazione, ModCicloCorrente.ParametriAttivi.MantTempoMaxErogazione, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantCorrente, ModCicloCorrente.ValoriCiclo.MantCorrenteImpulso, ModCicloCorrente.ParametriAttivi.MantCorrenteImpulso, 2, SbloccaValori);
                    }

                    if (ModCicloCorrente.Profilo != null)
                    {
                        ModoProf = (byte)ModCicloCorrente.Profilo.AttivaOpportunity;
                    }
                    else
                    {
                        ModoProf = 0;
                    }
                    switch (ModoProf)
                    {
                        case 0x00:
                            ModCicloCorrente.ParametriAttivi.OpportunityAttivabile = 1;
                            break;
                        case 0xF0:
                            ModCicloCorrente.ParametriAttivi.OpportunityAttivabile = 4;
                            break;
                        case 0xFF:
                            ModCicloCorrente.ParametriAttivi.OpportunityAttivabile = 1;
                            break;
                        default:
                            ModCicloCorrente.ParametriAttivi.OpportunityAttivabile = 1;
                            break;

                    }
                    ModCicloCorrente.ParametriAttivi.OpportunityAttivabile = 4;

                    FunzioniUI.ImpostaCheckBoxUshort(ref chkPaAttivaOppChg, ref lblPaAttivaOppChg, ModCicloCorrente.ValoriCiclo.OpportunityAttivabile, ModCicloCorrente.ParametriAttivi.OpportunityAttivabile, 3, SbloccaValori);

                    if (true) // (chkPaAttivaOppChg.Checked)
                    {
                        if (ModCicloCorrente.ValoriCiclo.OpportunityOraInizio > ModCicloCorrente.ValoriCiclo.OpportunityOraFine)
                        {
                            FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraInizio, ModCicloCorrente.ValoriCiclo.OpportunityOraInizio, ModCicloCorrente.ParametriAttivi.OpportunityOraInizio, 4, false);
                            FunzioniUI.ImpostaTextBoxUshort(ref  txtPaOppOraFine, ModCicloCorrente.ValoriCiclo.OpportunityOraFine, ModCicloCorrente.ParametriAttivi.OpportunityOraFine, 4, false);
                            chkPaOppNotturno.Checked = true;
                            rslPaOppFinestra.SliderMax = ModCicloCorrente.ValoriCiclo.OpportunityOraInizio;
                            rslPaOppFinestra.SliderMin = ModCicloCorrente.ValoriCiclo.OpportunityOraFine;
                            if (txtPaOppOraFine.Left > txtPaOppOraInizio.Left)
                            {
                                int templeft = txtPaOppOraFine.Left;
                                txtPaOppOraFine.Left = txtPaOppOraInizio.Left;
                                txtPaOppOraInizio.Left = templeft;
                                templeft = lblPaOppOraFine.Left;
                                lblPaOppOraFine.Left = lblPaOppOraInizio.Left;
                                lblPaOppOraInizio.Left = templeft;
                            }

                        }
                        else
                        {
                            FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraInizio, ModCicloCorrente.ValoriCiclo.OpportunityOraInizio, ModCicloCorrente.ParametriAttivi.OpportunityOraInizio, 4, false);
                            FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraFine, ModCicloCorrente.ValoriCiclo.OpportunityOraFine, ModCicloCorrente.ParametriAttivi.OpportunityOraFine, 4, false);
                            chkPaOppNotturno.Checked = false;
                            // per evitare errorisu min e max setto prima i valori ai due estremi 
                            rslPaOppFinestra.SliderMin = 0;
                            rslPaOppFinestra.SliderMax = 1240;
                            // poi imposto i valori corretti
                            rslPaOppFinestra.SliderMin = ModCicloCorrente.ValoriCiclo.OpportunityOraInizio;
                            rslPaOppFinestra.SliderMax = ModCicloCorrente.ValoriCiclo.OpportunityOraFine;

                            if (txtPaOppOraFine.Left < txtPaOppOraInizio.Left)
                            {
                                int templeft = txtPaOppOraFine.Left;
                                txtPaOppOraFine.Left = txtPaOppOraInizio.Left;
                                txtPaOppOraInizio.Left = templeft;
                                templeft = lblPaOppOraFine.Left;
                                lblPaOppOraFine.Left = lblPaOppOraInizio.Left;
                                lblPaOppOraInizio.Left = templeft;
                            }
                        }


                        //FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraInizio, ModCicloCorrente.ValoriCiclo.OpportunityOraInizio, ModCicloCorrente.ParametriAttivi.OpportunityOraInizio, 4, false);
                        //FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraFine, ModCicloCorrente.ValoriCiclo.OpportunityOraFine, ModCicloCorrente.ParametriAttivi.OpportunityOraFine, 4, false);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppVSoglia, ModCicloCorrente.ValoriCiclo.OpportunityTensioneMax, ModCicloCorrente.ParametriAttivi.OpportunityTensioneMax, 1, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppCorrente, ModCicloCorrente.ValoriCiclo.OpportunityCorrente, ModCicloCorrente.ParametriAttivi.OpportunityCorrente, 2, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppDurataMax, ModCicloCorrente.ValoriCiclo.OpportunityDurataMax, ModCicloCorrente.ParametriAttivi.OpportunityDurataMax, 3, SbloccaValori);

                        //chkPaOppNotturno.Checked = (ModCicloCorrente.ValoriCiclo.OpportunityOraInizio > ModCicloCorrente.ValoriCiclo.OpportunityOraFine);

                        OppNotturno(true);
                        
                    }

                    if (ModCicloCorrente.ValoriCiclo.OpportunityTensioneMax > 0 && ModCicloCorrente.ValoriCiclo.OpportunityCorrente>0 && ModCicloCorrente.ValoriCiclo.OpportunityDurataMax >0 )
                    {
                        chkPaAttivaOppChg.Checked = true;
                    }
                    else
                    {
                        chkPaAttivaOppChg.Checked = false;
                    }


                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaVMinRic, ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMin, ModCicloCorrente.ParametriAttivi.TensRiconoscimentoMin, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaVMaxRic, ModCicloCorrente.ValoriCiclo.TensRiconoscimentoMax, ModCicloCorrente.ParametriAttivi.TensRiconoscimentoMax, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaVMinStop, ModCicloCorrente.ValoriCiclo.TensMinStop, ModCicloCorrente.ParametriAttivi.TensMinStop, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaVLimite, ModCicloCorrente.ValoriCiclo.TensioneLimiteVLim, ModCicloCorrente.ParametriAttivi.TensioneLimiteVLim, 1, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaCorrenteMassima, ModCicloCorrente.ValoriCiclo.CorrenteMassima, ModCicloCorrente.ParametriAttivi.CorrenteMassima, 2, SbloccaValori);

                }



                return false;  // ?
            }
            catch (Exception Ex)
            {
                Log.Error("MostraParametriCiclo: " + Ex.Message);
                return false;
            }
        }

        private void txtPaCapacita_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!ProfiloInCaricamento)
                {
                    bool esitoRicalcolo;
                    //DefinisciValoriProfilo();
                    esitoRicalcolo = RicalcolaParametriCiclo(false);
                    MostraParametriCiclo(false, !esitoRicalcolo, chkPaSbloccaValori.Checked, true);

                    btnPaProfileRefresh.ForeColor = Color.Red;

                }

            }
            catch
            {

            }
        }

        private void flvCicliListaCariche_FormatRow(object sender, FormatRowEventArgs e)
        {
            //coloro la riga in base al Tipo Evento
            llMemoriaCicli _testataCiclo = (llMemoriaCicli)e.Model;
            if (_testataCiclo == null) return;
            byte StopReale = (byte)(_testataCiclo.CondizioneStop & 0x3F);

            switch (StopReale)
            {
                case 0x3F:  // "Registrazione non completata"; ex 0xFF
                    e.Item.BackColor = Color.Red;
                    break;
                case 0x00:  //"OK";
                    e.Item.BackColor = (((_testataCiclo.IdMemoriaLunga % 2) != 0) ? Color.Azure : Color.PowderBlue);
                    break;
                case 0x01:  //"Strappo";
                    //e.Item.BackColor = (((_testataCiclo.IdMemoriaLunga % 2) == 0) ? Color.LightYellow:Color.LightGoldenrodYellow);
                    break;
                case 0x04:  //"Assenza rete";
                            //e.Item.BackColor = (((_testataCiclo.IdMemoriaLunga % 2) == 0) ? Color.AliceBlue : Color.LightCyan);
                    break;
                case 0x0B:  //"?";
                    //e.Item.BackColor = (((_testataCiclo.IdMemoriaLunga % 2) == 0) ? Color.MistyRose : Color.LightPink);
                    break;
                default:    // "Evento Anomalo"
                    //e.Item.BackColor = Color.LightYellow;
                    break;
            }

        }

        private void rbtMemParametriInit_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbtMemDatiCliente_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbtMemDatiCliente_Click(object sender, EventArgs e)
        {
            if (rbtMemDatiCliente.Checked)
            {
                txtMemCFStartAdd.Text = "001000";
                txtMemCFBlocchi.Text = "1";
            }
        }

        private void rbtMemProgrammazioni_Click(object sender, EventArgs e)
        {
            if (rbtMemProgrammazioni.Checked)
            {
                txtMemCFStartAdd.Text = "002000";
                txtMemCFBlocchi.Text = "1";
            }
        }

        private void rbtMemContatori_Click(object sender, EventArgs e)
        {
            if (rbtMemContatori.Checked)
            {
                txtMemCFStartAdd.Text = "003000";
                txtMemCFBlocchi.Text = "1";
            }
        }

        private void rbtMemBrevi_Click(object sender, EventArgs e)
        {
            if (rbtMemBrevi.Checked)
            {
                txtMemCFStartAdd.Text = "006000";
                txtMemCFBlocchi.Text = "429";
            }
        }

        private void rbtMemLunghi_Click(object sender, EventArgs e)
        {
            if (rbtMemLunghi.Checked)
            {
                txtMemCFStartAdd.Text = "1B3000";
                txtMemCFBlocchi.Text = "4";
            }
        }

        private void flvCicliListaCariche_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;
                //llMemoriaCicli CicloSel;

                if (flvCicliListaCariche.SelectedObject == null)
                {
                    btnCicliCaricaBrevi.Enabled = false;
                    btnCicliMostraBrevi.Enabled = false;
                }
                else
                {

                    CicloCorrente = (llMemoriaCicli)flvCicliListaCariche.SelectedObject;

                    if (CicloCorrente.CicliMemoriaBreve.Count < CicloCorrente.NumEventiBrevi)
                    {
                        // non ho tutto in memoria, ricarico
                        CicloCorrente.CicliMemoriaBreve = CaricaListaBrevi(CicloCorrente.PuntatorePrimoBreve, (ushort)CicloCorrente.NumEventiBrevi, CicloCorrente.IdMemoriaLunga);
                    }

                    MostraDettaglioRiga(CicloCorrente);

                }

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
                this.Cursor = Cursors.Arrow;
            }

        }

        private void chkPaSbloccaValori_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                MostraParametriCiclo(false, false, chkPaSbloccaValori.Checked);
            }
            catch (Exception Ex)
            {
                Log.Error("chkPaSbloccaValori_CheckedChanged: " + Ex.Message);
            }
        }

        public bool CaricaProgrammazioni()
        {
            try
            {
                bool Esito;

                Esito = _cb.LeggiProgrammazioni();

                if (Esito)
                {
                    MostraProgrammazioni();

                }
                else
                {
                    MostraParametriCiclo(true, true);
                }

                return true;

            }
            catch
            {
                ProfiloInCaricamento = false;
                return false;
            }
        }

        public bool MostraProgrammazioni()
        {
            try
            {
                bool Esito;


                //MostraCicloCorrente();
                ProfiloInCaricamento = true;
                ModCicloCorrente.ProfiloRegistrato = _cb.Programmazioni.ProgrammaAttivo;
                ModCicloCorrente.EstraiDaProgrammaCarica();

                MostraParametriCiclo(true, false, chkPaSbloccaValori.Checked);
                ProfiloInCaricamento = false;

                InizializzaVistaProgrammazioni();


                return true;

            }
            catch
            {
                ProfiloInCaricamento = false;
                return false;
            }
        }



        public bool LeggiProgrammazioniDB(string IdApparato)
        {
            try
            {
                bool Esito;

                Esito = _cb.CaricaProgrammazioniDB(IdApparato);

                if (Esito)
                {
                    //MostraCicloCorrente();
                    ProfiloInCaricamento = true;
                    ModCicloCorrente.ProfiloRegistrato = _cb.Programmazioni.ProgrammaAttivo;
                    //ModCicloCorrente.GeneraListaValori();
                    ModCicloCorrente.EstraiDaProgrammaCarica();

                    MostraParametriCiclo(true, false, chkPaSbloccaValori.Checked);
                    ProfiloInCaricamento = false;

                    InizializzaVistaProgrammazioni();


                }
                else
                {
                    MostraParametriCiclo(true, true);
                }

                return true;

            }
            catch
            {
                ProfiloInCaricamento = false;
                return false;
            }
        }


        private void btnPaCaricaCicli_Click(object sender, EventArgs e)
        {
            try
            {
                bool Esito;

                Esito = CaricaProgrammazioni();

            }
            catch
            {
                ProfiloInCaricamento = false;
            }

        }

        private void btnCicliCaricaArea_Click(object sender, EventArgs e)
        {
            Log.Debug("Lancio lettura lunghi");

            //_avCicli = new frmAvanzamentoCicli();
            _avCicli.ParametriWorker.MainCount = 100;
            _avCicli.llLocale = _cb;
            _avCicli.ValStart = (int)0;

            _avCicli.AddrStart = _cb.Memoria.MappaCorrente.RecordLunghi.AddrArea;
            _avCicli.ValFine = _cb.Memoria.MappaCorrente.RecordLunghi.NumPagine * 0x1000 ;

            _avCicli.DbDati = _logiche.dbDati.connessione;
            _avCicli.CaricaBrevi = false; // chkCaricaBrevi.Checked;
            _avCicli.ElementoPilotato = frmAvanzamentoCicli.ControlledDevice.LadeLight;
            _avCicli.TipoComando = elementiComuni.tipoMessaggio.AreaMemLungaLL;
            Log.Debug("FRM RicaricaCicli: ");

            //_esito = _sb.RicaricaCaricaCicliMemLunga(Inizio, (uint)_sb.sbData.LongMem, _logiche.dbDati.connessione, true, CaricaBrevi);

            // Apro il form con le progressbar
            _avCicli.ShowDialog(this);

            InizializzaListaCariche();

        }

        private void flwPaListaConfigurazioni_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;
                //llMemoriaCicli CicloSel;+

                if (flwPaListaConfigurazioni.SelectedObject == null)
                {
                    btnPaAttivaConfigurazione.Enabled = false;
                }
                else
                {

                    llProgrammaCarica tmpRigaSel = (llProgrammaCarica)flwPaListaConfigurazioni.SelectedObject;

                    if (tmpRigaSel.PosizioneCorrente != 0)
                        btnPaAttivaConfigurazione.Enabled = true;
                    else
                        btnPaAttivaConfigurazione.Enabled = false;


                }

            }
            catch (Exception Ex)
            {
                Log.Error("flwPaListaConfigurazioni_SelectedIndexChanged: " + Ex.Message);
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnPaAttivaConfigurazione_Click(object sender, EventArgs e)
        {
            try
            {
                //FastObjectListView _lista = (FastObjectListView)sender;
                //llMemoriaCicli CicloSel;+

                if (flwPaListaConfigurazioni.SelectedObject == null)
                {
                    return;
                }
                else
                {

                    llProgrammaCarica tmpRigaSel = (llProgrammaCarica)flwPaListaConfigurazioni.SelectedObject;

                    if (tmpRigaSel.PosizioneCorrente == 0)
                        return;
                    else
                    {
                        // Riposiziono tuuuti gli elementi: quelli prima del selezionato in posizione +1, 
                        //                                  il selezionato in posizione 0
                        //                                  quelli dopo il selezioato, invariati
                        bool esitoSalvataggio = false;
                        this.Cursor = Cursors.WaitCursor;
                        foreach (llProgrammaCarica TPC in _cb.Programmazioni.ProgrammiDefiniti)
                        {
                            if (TPC.PosizioneCorrente < tmpRigaSel.PosizioneCorrente)
                            {
                                TPC.PosizioneCorrente += 1;
                            }
                            else
                            {
                                if (TPC.PosizioneCorrente == tmpRigaSel.PosizioneCorrente)
                                {
                                    TPC.PosizioneCorrente = 0;
                                }

                            }
                        }

                        esitoSalvataggio = _cb.SalvaProgrammazioniApparato();
                        CaricaProgrammazioni();

                        if (esitoSalvataggio)
                        {
                            MessageBox.Show("Configurazione Aggiornata", "CONFIGURAZIONE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            IncrementaContatoreConf();
                        }
                        else
                        {
                            MessageBox.Show("Aggiornamento configurazione non riuscito", "CONFIGURAZIONE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }


                        this.Cursor = Cursors.Default;


                    }
                    btnPaAttivaConfigurazione.Enabled = false;


                }

            }
            catch (Exception Ex)
            {
                Log.Error("btnPaAttivaConfigurazione_Click: " + Ex.Message);
                this.Cursor = Cursors.Arrow;
            }

        }

        private void BtnCicliMostraBrevi_Click(object sender, EventArgs e)
        {

        }

        private void BtnGenAzzeraContatori_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Risposta = MessageBox.Show("Confermi l'azzeramento contatori ?", "CONTATORI", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (Risposta == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    _cb.AzzeraContatori();
                    CaricaAreaContatori();
                    this.Cursor = Cursors.Arrow;
                }

            }
            catch (Exception Ex)
            {
                Log.Error("BtnGenAzzzeraContatori_Click: " + Ex.Message);
                this.Cursor = Cursors.Arrow;
            }

        }

        public bool IncrementaContatoreConf()
        {
            try
            {
                bool esito = false;
                _cb.CaricaAreaContatori();
                //esito = _cb.IncrementaConteggioProgrammazioni();
                CaricaAreaContatori();

                return esito;
            }
            catch (Exception Ex)
            {
                Log.Error("BtnGenAzzzeraContatori_Click: " + Ex.Message);
                return false;

            }

        }


        private void BtnGenAzzeraContatoriTot_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult Risposta = MessageBox.Show("Confermi l'azzeramento contatori ?", "CONTATORI", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (Risposta == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    _cb.AzzeraContatori(true);
                    CaricaAreaContatori();
                    this.Cursor = Cursors.Arrow;
                }

            }
            catch (Exception Ex)
            {
                Log.Error("BtnGenAzzeraContatori_Click: " + Ex.Message);
                this.Cursor = Cursors.Arrow;
            }
        }

        private void BtnGenResetBoard_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _cb.ResetScheda();
                this.Cursor = Cursors.Default;

            }
            catch (Exception Ex)
            {
                Log.Error("BtnGenResetBoard_Click: " + Ex.Message);
                this.Cursor = Cursors.Default;
            }
        }

        public bool AttivaSalvataggioConfigurazione()
        {
            try
            {
                bool esito;
                elementiComuni.EsitoVerificaParametri EsitoVer;
                ushort _Tensione = FunzioniMR.ConvertiUshort(txtPaTensione.Text, 100, 0);
                ushort _Capacita = FunzioniMR.ConvertiUshort(txtPaCapacita.Text, 10, 0);
                ushort _Celle = FunzioniMR.ConvertiUshort(txtPaNumCelle.Text, 1, 0);
                _llModelloCb _ModelloCB = _cb.ModelloCorrente;

                // Se almeno 1 parametro è diverso dal ciclo attivo corrente lo salvo come nuovo ciclo
                EsitoVer = VerificaValoriParametriCarica();
                if (EsitoVer == elementiComuni.EsitoVerificaParametri.Ok)
                {
                    //coincide col programma esistente. esco
                   
                    return true;
                }
                else
                {
                    // Almeno 1 valore cambiato --> prenoto il cambio ID
                    ModCicloCorrente.RichiestoNuovoId = true;

                }
                // Carico i valori impostati nelle textbox
                esito = LeggiValoriParametriCarica();
                // Se sono quì ho passato il controllo e almeno 1 dato è cambiato
                ModCicloCorrente.DatiSalvati = false;

                CaricaBatteria.EsitoRicalcolo esitoVerifica = ModCicloCorrente.VerificaParametri(_Tensione, _Capacita, _Celle, _ModelloCB);

                switch (esitoVerifica)
                {
                    case CaricaBatteria.EsitoRicalcolo.OK:
                        // tutti i parametri sono validi. attivo il pulsante
                        btnPaSalvaDati.Enabled = true;
                        esito = true;
                        break;
                    case CaricaBatteria.EsitoRicalcolo.ErrIMax:
                        btnPaSalvaDati.Enabled = false;
                        esito = false;
                        MessageBox.Show("Corrente richiesta troppo elevata", "Verifica Parametri", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case CaricaBatteria.EsitoRicalcolo.ErrIMin:
                        btnPaSalvaDati.Enabled = false;
                        esito = false;
                        break;
                    case CaricaBatteria.EsitoRicalcolo.ErrVMax:
                        btnPaSalvaDati.Enabled = false;
                        esito = false;
                        MessageBox.Show("Tensione richiesta troppo elevata", "Verifica Parametri", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case CaricaBatteria.EsitoRicalcolo.ErrVMin:
                        btnPaSalvaDati.Enabled = false;
                        esito = false;
                        break;
                    case CaricaBatteria.EsitoRicalcolo.ErrGenerico:
                    case CaricaBatteria.EsitoRicalcolo.ParNonValidi:
                    case CaricaBatteria.EsitoRicalcolo.ErrCBNonDef:
                    case CaricaBatteria.EsitoRicalcolo.ErrUndef:
                        {
                            btnPaSalvaDati.Enabled = false;
                            esito = false;
                        }
                        break;
         
                    default:
                        // non faccio nulla ....
                        break;
                }

                return esito;
            }
            catch (Exception Ex)
            {
                Log.Error("AttivaSalvataggioConfigurazione: " + Ex.Message);
                return false;
            }
        }

        #region "Gestione Modifiche"
        private void TxtPaMantVmin_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaMantVmin.ForeColor = Color.Red;
                txtPaMantVmin.Select();
            }
            else
                txtPaMantVmin.ForeColor = Color.Black;
        }

        private void TxtPaMantVmax_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaMantVmax.ForeColor = Color.Red;
                txtPaMantVmax.Select();
            }
            else
                txtPaMantVmax.ForeColor = Color.Black;
        }

        private void TxtPaSogliaV0_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaSogliaV0.ForeColor = Color.Red;
                txtPaSogliaV0.Select();
            }
            else
                txtPaSogliaV0.ForeColor = Color.Black;

        }

        private void TxtPaPrefaseI0_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaPrefaseI0.ForeColor = Color.Red;
                txtPaPrefaseI0.Select();
            }
            else
                txtPaPrefaseI0.ForeColor = Color.Black;
        }

        private void TxtPaDurataMaxT0_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaDurataMaxT0.ForeColor = Color.Red;
                txtPaDurataMaxT0.Select();
            }
            else
                txtPaDurataMaxT0.ForeColor = Color.Black;

        }

        private void TxtPaSogliaVs_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaSogliaVs.ForeColor = Color.Red;
                txtPaSogliaVs.Select();
            }
            else
                txtPaSogliaVs.ForeColor = Color.Black;

        }

        private void TxtPaCorrenteI1_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaCorrenteI1.ForeColor = Color.Red;
                txtPaCorrenteI1.Select();
            }
            else
                txtPaCorrenteI1.ForeColor = Color.Black;

        }

        private void CmbPaDurataMaxT1_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                cmbPaDurataMaxT1.ForeColor = Color.Red;
                cmbPaDurataMaxT1.Select();
            }
            else
                cmbPaDurataMaxT1.ForeColor = Color.Black;

        }

        private void TxtPaRaccordoF1_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaRaccordoF1.ForeColor = Color.Red;
                txtPaRaccordoF1.Select();
            }
            else
                txtPaRaccordoF1.ForeColor = Color.Black;

        }

        private void TxtPaCorrenteRaccordo_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaCorrenteRaccordo.ForeColor = Color.Red;
                txtPaCorrenteRaccordo.Select();
            }
            else
                txtPaCorrenteRaccordo.ForeColor = Color.Black;

        }

        private void TxtPaCorrenteF3_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaCorrenteF3.ForeColor = Color.Red;
                txtPaCorrenteF3.Select();
            }
            else
                txtPaCorrenteF3.ForeColor = Color.Black;

        }






        #endregion


        private void rslPaOppFinestra_ValueChanged(object sender, EventArgs e)
        {
            //            txtPaOppOraInizio    ModCicloCorrente.ValoriCiclo.OpportunityOraInizio
            //            txtPaOppOraFine      ModCicloCorrente.ValoriCiclo.OpportunityOraFine

            ushort FineGiornata;
            if(ProfiloInCaricamento)
            {
                return ;
            }

            if (chkPaOppNotturno.Checked)
            {
                // NOTTURNO
                if (rslPaOppFinestra.SliderMax != ModCicloCorrente.ValoriCiclo.OpportunityOraInizio)
                {
                    if (rslPaOppFinestra.SliderMax < (ModCicloCorrente.DurataMaxCarica)) rslPaOppFinestra.SliderMax = (ModCicloCorrente.DurataMaxCarica);

                    ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = (ushort)rslPaOppFinestra.SliderMax;

                    FineGiornata = (ushort)(1440 - ModCicloCorrente.ValoriCiclo.OpportunityOraInizio);
                    if ((ModCicloCorrente.ValoriCiclo.OpportunityOraFine + FineGiornata) < 240)
                    {
                        ModCicloCorrente.ValoriCiclo.OpportunityOraFine = (ushort)(240 - FineGiornata);
                        rslPaOppFinestra.SliderMin = ModCicloCorrente.ValoriCiclo.OpportunityOraFine;
                    }
                    if ((ModCicloCorrente.ValoriCiclo.OpportunityOraInizio - ModCicloCorrente.ValoriCiclo.OpportunityOraFine) < ModCicloCorrente.DurataMaxCarica)
                    {
                        ModCicloCorrente.ValoriCiclo.OpportunityOraFine = (ushort)(ModCicloCorrente.ValoriCiclo.OpportunityOraInizio - ModCicloCorrente.DurataMaxCarica);
                        rslPaOppFinestra.SliderMin = ModCicloCorrente.ValoriCiclo.OpportunityOraFine;
                    }


                }
                else
                {
                    if (rslPaOppFinestra.SliderMin != ModCicloCorrente.ValoriCiclo.OpportunityOraFine)
                    {
                        if (rslPaOppFinestra.SliderMin >  (1440 - ModCicloCorrente.DurataMaxCarica)) rslPaOppFinestra.SliderMin = (1440 - ModCicloCorrente.DurataMaxCarica); // Se l'inizio passa dopo la mezzanotte  non è più notturno --> inizio < fine
                        ModCicloCorrente.ValoriCiclo.OpportunityOraFine = (ushort)rslPaOppFinestra.SliderMin;
                        FineGiornata = (ushort)(1440 - ModCicloCorrente.ValoriCiclo.OpportunityOraInizio);
                        if ((ModCicloCorrente.ValoriCiclo.OpportunityOraFine + FineGiornata) < 240)
                        {
                            ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = (ushort)(240 - FineGiornata);
                            rslPaOppFinestra.SliderMin = ModCicloCorrente.ValoriCiclo.OpportunityOraFine;
                        }
                        if ((ModCicloCorrente.ValoriCiclo.OpportunityOraInizio - ModCicloCorrente.ValoriCiclo.OpportunityOraFine) < ModCicloCorrente.DurataMaxCarica)
                        {
                            ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = (ushort)(ModCicloCorrente.ValoriCiclo.OpportunityOraFine + ModCicloCorrente.DurataMaxCarica);
                            rslPaOppFinestra.SliderMax = ModCicloCorrente.ValoriCiclo.OpportunityOraInizio;
                        }
                    }
                }

                txtPaOppOraFine.Text = FunzioniMR.StringaOreMinutiLL( ModCicloCorrente.ValoriCiclo.OpportunityOraFine );
                txtPaOppOraInizio.Text = FunzioniMR.StringaOreMinutiLL(ModCicloCorrente.ValoriCiclo.OpportunityOraInizio);
                btnPaSalvaDati.Enabled = true;

            }
            else
            {
                // DIURNO

                if (rslPaOppFinestra.SliderMax != ModCicloCorrente.ValoriCiclo.OpportunityOraFine)
                {
                    if (rslPaOppFinestra.SliderMax < 240) rslPaOppFinestra.SliderMax = 240;

                    ModCicloCorrente.ValoriCiclo.OpportunityOraFine = (ushort)rslPaOppFinestra.SliderMax;
                    FineGiornata = (ushort)(1440 - ModCicloCorrente.ValoriCiclo.OpportunityOraInizio);
                    if ((ModCicloCorrente.ValoriCiclo.OpportunityOraFine - ModCicloCorrente.ValoriCiclo.OpportunityOraInizio) < 240)
                    {
                        ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = (ushort)(ModCicloCorrente.ValoriCiclo.OpportunityOraFine - 240);
                        rslPaOppFinestra.SliderMin = ModCicloCorrente.ValoriCiclo.OpportunityOraInizio;
                    }
                    if ((1440 -ModCicloCorrente.ValoriCiclo.OpportunityOraFine + ModCicloCorrente.ValoriCiclo.OpportunityOraInizio) < ModCicloCorrente.DurataMaxCarica)
                    {
                        ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = (ushort)(ModCicloCorrente.DurataMaxCarica + ModCicloCorrente.ValoriCiclo.OpportunityOraFine - 1440);
                        rslPaOppFinestra.SliderMin = ModCicloCorrente.ValoriCiclo.OpportunityOraInizio;
                    }



                }
                else
                {
                    if (rslPaOppFinestra.SliderMin != ModCicloCorrente.ValoriCiclo.OpportunityOraInizio)
                    {
                        if (rslPaOppFinestra.SliderMin > 1200) rslPaOppFinestra.SliderMin = 1200; // Se l'inizio passa dopo la mezzanotte  non è più notturno --> inizio < fine
                        ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = (ushort)rslPaOppFinestra.SliderMin;
                        if ((ModCicloCorrente.ValoriCiclo.OpportunityOraFine - ModCicloCorrente.ValoriCiclo.OpportunityOraInizio) < 240)
                        {
                            ModCicloCorrente.ValoriCiclo.OpportunityOraFine = (ushort)(ModCicloCorrente.ValoriCiclo.OpportunityOraInizio + 240 );
                            rslPaOppFinestra.SliderMax = ModCicloCorrente.ValoriCiclo.OpportunityOraFine;
                        }
                        if ((1440 - ModCicloCorrente.ValoriCiclo.OpportunityOraFine + ModCicloCorrente.ValoriCiclo.OpportunityOraInizio) < ModCicloCorrente.DurataMaxCarica)
                        {
                            ModCicloCorrente.ValoriCiclo.OpportunityOraFine = (ushort)(ModCicloCorrente.ValoriCiclo.OpportunityOraInizio + 1440 - ModCicloCorrente.DurataMaxCarica);
                            rslPaOppFinestra.SliderMax = ModCicloCorrente.ValoriCiclo.OpportunityOraFine;
                        }

                    }
                }

                txtPaOppOraFine.Text = FunzioniMR.StringaOreMinutiLL(ModCicloCorrente.ValoriCiclo.OpportunityOraFine);
                txtPaOppOraInizio.Text = FunzioniMR.StringaOreMinutiLL(ModCicloCorrente.ValoriCiclo.OpportunityOraInizio);

                
            }

            OppNotturno(true);

        }

        private void ChkPaOppNotturno_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkPaOppNotturno.Checked)
                {
                    ushort tempval;

                    if (ModCicloCorrente.ValoriCiclo.OpportunityOraFine > ModCicloCorrente.ValoriCiclo.OpportunityOraInizio)
                    {
                        //ero effettivamente in notturno. passo a diurno
                        tempval = ModCicloCorrente.ValoriCiclo.OpportunityOraFine;
                        ModCicloCorrente.ValoriCiclo.OpportunityOraFine = ModCicloCorrente.ValoriCiclo.OpportunityOraInizio;
                        ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = tempval;

                        int templeft = txtPaOppOraFine.Left;
                        txtPaOppOraFine.Left = txtPaOppOraInizio.Left;
                        txtPaOppOraInizio.Left = templeft;
                        templeft = lblPaOppOraFine.Left;
                        lblPaOppOraFine.Left = lblPaOppOraInizio.Left;
                        lblPaOppOraInizio.Left = templeft;
                        btnPaSalvaDati.Enabled = true;
                    }

                }
                else
                {
                    ushort tempval;

                    if (ModCicloCorrente.ValoriCiclo.OpportunityOraFine <= ModCicloCorrente.ValoriCiclo.OpportunityOraInizio)
                    {
                        //ero effettivamente in notturno. passo a diurno
                        tempval = ModCicloCorrente.ValoriCiclo.OpportunityOraFine;
                        ModCicloCorrente.ValoriCiclo.OpportunityOraFine = ModCicloCorrente.ValoriCiclo.OpportunityOraInizio;
                        ModCicloCorrente.ValoriCiclo.OpportunityOraInizio = tempval;
                        int templeft = txtPaOppOraFine.Left;
                        txtPaOppOraFine.Left = txtPaOppOraInizio.Left;
                        txtPaOppOraInizio.Left = templeft;
                        templeft = lblPaOppOraFine.Left;
                        lblPaOppOraFine.Left = lblPaOppOraInizio.Left;
                        lblPaOppOraInizio.Left = templeft;
                        btnPaSalvaDati.Enabled = true;
                    }
                }

                txtPaOppOraFine.Text = FunzioniMR.StringaOreMinutiLL(ModCicloCorrente.ValoriCiclo.OpportunityOraFine);
                txtPaOppOraInizio.Text = FunzioniMR.StringaOreMinutiLL(ModCicloCorrente.ValoriCiclo.OpportunityOraInizio);

                OppNotturno(true);
            }
            catch
            {

            }
        }

        public void OppNotturno( bool Notturno )
        {
            try
            {
                if (ModCicloCorrente.ValoriCiclo.OpportunityOraInizio > ModCicloCorrente.ValoriCiclo.OpportunityOraFine)
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

        private void ChkPaUsaSpyBatt_CheckedChanged(object sender, EventArgs e)
        {

            try
            {
                if(chkPaUsaSpyBatt.Checked)
                {
                    tbcPaSchedeValori.Visible = false;
                    cmbPaTipoBatteria.SelectedIndex = 0;
                    chkPaUsaSpyBatt.Checked = true;
                    cmbPaTipoBatteria.Enabled = false;
                    

                    var myImage = new Bitmap(Properties.Resources.ICOspybattBase);
                    picPaImmagineProfilo.BackColor = Color.White;
                    picPaImmagineProfilo.Image = myImage;
                    grbPaImpostazioniLocali.Enabled = false;

                }
                else
                {
                    tbcPaSchedeValori.Visible = true;
                    cmbPaTipoBatteria.Enabled = true;
                    grbPaImpostazioniLocali.Enabled = true;

                    if ((_mbProfiloCarica)cmbPaProfilo.SelectedItem != null)
                    {
                        string Grafico = (string)((_mbProfiloCarica)cmbPaProfilo.SelectedItem).Grafico;
                        if (Grafico == "")
                        {
                            picPaImmagineProfilo.BackColor = Color.LightGray;
                            picPaImmagineProfilo.Image = null;

                        }
                        else
                        {
                            ResourceManager rm = Resources.profili.ModelliProfilo.ResourceManager;
                            Bitmap myImage = (Bitmap)rm.GetObject(Grafico);
                            picPaImmagineProfilo.BackColor = Color.White;
                            picPaImmagineProfilo.Image = myImage;

                        }
                    }

                }



                btnPaSalvaDati.Enabled = true;
            }
            catch
            {
               // btnPaSalvaDati.Enabled = true;
            }

        }

        private void ChkPaUsaSafety_CheckedChanged(object sender, EventArgs e)
        {
            btnPaSalvaDati.Enabled = true;
        }

        private void TxtPaVMax_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaVMax.ForeColor = Color.Red;
                txtPaVMax.Select();
            }
            else
                txtPaVMax.ForeColor = Color.Black;

        }

        private void TxtPaTempoT2Min_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaTempoT2Min.ForeColor = Color.Red;
                txtPaTempoT2Min.Select();
            }
            else
                txtPaTempoT2Min.ForeColor = Color.Black;

        }

        private void TxtPaTempoT2Max_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaTempoT2Max.ForeColor = Color.Red;
                txtPaTempoT2Max.Select();
            }
            else
                txtPaTempoT2Max.ForeColor = Color.Black;

        }

        private void TxtPaCoeffK_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaCoeffK.ForeColor = Color.Red;
                txtPaCoeffK.Select();
            }
            else
                txtPaCoeffK.ForeColor = Color.Black;


        }

        private void TxtPaTempoT3Max_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaTempoT3Max.ForeColor = Color.Red;
                txtPaTempoT3Max.Select();
            }
            else
                txtPaTempoT3Max.ForeColor = Color.Black;

        }

        private void btnPaProfileChiudiCanale_Click(object sender, EventArgs e)
        {
            try
            {
                if (_cb.StopComunicazione())
                {
                    btnPaSalvaDati.Enabled = false;

                }

            }
            catch (Exception Ex)
            {
                Log.Error("btnPaProfileChiudiCanale_Click: " + Ex.Message);
            }
        }

        private void btnSalveCliente_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SalvaDatiCliente();
                this.Cursor = Cursors.Default;

            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                Log.Error("btnSalveCliente_Click: " + Ex.Message);
            }
        }



        public bool SalvaDatiCliente()
        {
            try
            {
                bool _esito;

                if (_cb.DatiCliente == null)
                {
                    _cb.DatiCliente = new llDatiCliente();
                }

                _cb.DatiCliente.Client = txtCliente.Text.Trim();
                _cb.DatiCliente.Description = txtCliDescrizione.Text.Trim();
                _cb.DatiCliente.Note = txtCliNote.Text.Trim();
                _cb.DatiCliente.LocalId = txtCliCodiceLL.Text.Trim();
                _cb.DatiCliente.LocalName = txtCliNomeIntLL.Text.Trim();

                _esito = _cb.ScriviDatiCliente();
                if (_esito)
                {
                    _cb.LeggiDatiCliente();
                    MostraDatiCliente();
                }
                else
                {
                    MessageBox.Show("Salvataggio non riuscito", "DAI CLIENTE", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return _esito;

            }
            catch (Exception Ex)
            {
                Log.Error("SalvaInizializzazione: " + Ex.Message);
                return false;

            }
        }

        public bool MostraDatiCliente()
        {
            try
            {

                txtCliente.Text = "";
                txtCliDescrizione.Text = "";
                txtCliNote.Text = "";
                txtCliCodiceLL.Text = "";
                txtCliCodiceLL.Text = "";

                if (_cb.DatiCliente == null)
                {
                    return false;
                }

                txtCliente.Text = _cb.DatiCliente.Client;
                txtCliDescrizione.Text = _cb.DatiCliente.Description;
                txtCliNote.Text = _cb.DatiCliente.Note;
                txtCliCodiceLL.Text = _cb.DatiCliente.LocalId;
                txtCliNomeIntLL.Text = _cb.DatiCliente.LocalName;

                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("SalvaInizializzazione: " + Ex.Message);
                return false;

            }
        }

        private void btnCaricaCliente_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _cb.LeggiDatiCliente();
                MostraDatiCliente();
                this.Cursor = Cursors.Default;

            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                Log.Error("btnSalveCliente_Click: " + Ex.Message);
            }
        }

        private void btnMemTestExac_Click(object sender, EventArgs e)
        {
            try
            {
                uint _StartAddr;
                ushort _NumByte;
                int _letturaCorrente;
                int _numErrori;
                int _NumLetture = 0;
                int _NumLettureOK = 0;
                int _NumLettureERR = 0;
                bool _esito;
                byte[] _Dati;

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
                if (_NumByte > 240) _NumByte = 240;
                _Dati = new byte[_NumByte];

                _NumLetture = 0;
                if (int.TryParse(txtMemNumTest.Text, out _NumLetture) != true) return;

                this.Cursor = Cursors.WaitCursor;
                Random random = new System.Random();

                if (_NumLetture > 0)
                {
                    _numErrori = 0;

                    for (int _cicloLetture = 0; _cicloLetture < _NumLetture; _cicloLetture++)
                    {
                        _letturaCorrente = _cicloLetture + 1;
                        if (chkMemTestLenRND.Checked)
                        {
                            _NumByte = (ushort)random.Next(1, 240);
                        }
                        if (chkMemTestAddrRND.Checked)
                        {
                            _StartAddr = (uint)random.Next(0, 0x1F0000);
                        }


                        _esito = _cb.LeggiBloccoMemoria(_StartAddr, _NumByte, out _Dati);

                        if (!_esito)
                        {
                            MessageBox.Show("Lettura " + _letturaCorrente.ToString() + " non riuscita", "Lettura dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else
                        {

                            if(_cb.NumeroTentativiLettura != 1 )
                            {
                                _numErrori += 1;
                            }
                            txtMemNumTestOK.Text = _letturaCorrente.ToString();
                            txtMemNumTestERR.Text = _numErrori.ToString();
                            if (_letturaCorrente % 10 == 0)
                            {
                                Application.DoEvents();
                            }
                        }
                    }

                }
                else
                {
                   // MessageBox.Show("Inserire un numero di blocchi valido", "Esportazione dati Corrente", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                this.Cursor = Cursors.Default;



            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
                this.Cursor = Cursors.Default;
            }

        }

        private void btnFwSwitchApp_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                _cb.ResetScheda();
                VerificaStatoFw();

                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Log.Error("btnFwSwitchApp_Click: " + Ex.Message);
                this.Cursor = Cursors.Default;
            }
        }

        private void btnPaProfileClear_Click(object sender, EventArgs e)
        {
            try
            {
                bool Esito;

                Esito = _cb.Programmazioni.ProgrammaAttivo.AzzeraValori();                          //CaricaProgrammaAttivo();

                if (Esito)
                {
                    //MostraCicloCorrente();
                    ProfiloInCaricamento = true;
                    ModCicloCorrente.ProfiloRegistrato = _cb.Programmazioni.ProgrammaAttivo;
                    ModCicloCorrente.EstraiDaProgrammaCarica();

                    MostraParametriCiclo(true, false);
                    ProfiloInCaricamento = false;
                }
                else
                {
                    MostraParametriCiclo(true, true);
                }


            }

            catch (Exception Ex)
            {
                Log.Error("btnPaProfileClear_Click: " + Ex.Message);
            }
        }

        private void btnMemClearLogExec_Click(object sender, EventArgs e)
        {
            try
            {
                bool Esito;
                bool EsitoDel;

                // Chiedo sconferma azzeramento:

                DialogResult Risposta = MessageBox.Show("Confermi l'azzeramento della scheda corrente ?", "DATI SCHEDA", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (Risposta != DialogResult.OK)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                // Programmazioni
                // 1 blocco dall'indirizzo  0x2000 / 0x2FFF

                if (chkMemCResetProg.Checked)
                {
                    EsitoDel = _cb.CancellaBlocco4K(0x2000);
                    if (!EsitoDel)
                    {
                        MessageBox.Show("Cancellazione PROGRAMMAZIONI non riuscita", "Cancellazione dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Arrow;
                        return;
                    }

                }

                // Azzeramento contatori - Funzione specifica

                if (chkMemCResetCont.Checked)
                {
                    EsitoDel = _cb.AzzeraContatori();
                    if (!EsitoDel)
                    {
                        MessageBox.Show("Cancellazione CONTATORI non riuscita", "Cancellazione dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Arrow;
                        return;
                    }

                }

                // Cicli
                // Brevi:  area totale 429 blocchi da 0x006000 - Cancello solo il primo
                // Lunghi: area totale   4 blocchi da 0x1B3000 - Cancello tutto


                if (chkMemCResetCicli.Checked)
                {
                    EsitoDel = _cb.CancellaBlocco4K(0x006000);
                    if (!EsitoDel)
                    {
                        MessageBox.Show("Cancellazione CICLI non riuscita", "Cancellazione dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Arrow;
                        return;
                    }

                    for (int Ciclo = 0; Ciclo < 4; Ciclo++)
                    {
                        uint Addr = 0x1B3000;

                        EsitoDel = _cb.CancellaBlocco4K(Addr);
                        if (!EsitoDel)
                        {
                            MessageBox.Show("Cancellazione CICLI non riuscita", "Cancellazione dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Cursor = Cursors.Arrow;
                            return;
                        }
                        Addr += 0x1000;
                    }
                }


                // Reboot
                if (chkMemCReboot.Checked)
                {
                    _cb.ResetScheda();
                }

                this.Cursor = Cursors.Arrow;
            }

            catch (Exception Ex)
            {
                Log.Error("btnPaProfileClear_Click: " + Ex.Message);
                this.Cursor = Cursors.Arrow;
            }
        }

        private void txtInitBrdANomModulo_Leave(object sender, EventArgs e)
        {

        }

        private void txtPaCoeffKc_Leave(object sender, EventArgs e)
        {
            if (!AttivaSalvataggioConfigurazione())
            {
                txtPaCoeffKc.ForeColor = Color.Red;
                txtPaCoeffKc.Select();
            }
            else
                txtPaCoeffKc.ForeColor = Color.Black;

        }

        private void lblOraRTC_Click(object sender, EventArgs e)
        {

        }

        private void btnCalScriviGiorno_Click(object sender, EventArgs e)
        {
            bool _esito;            try
            {
                int _giorno = int.Parse(txtCalGiorno.Text);
                int _mese = int.Parse(txtCalMese.Text);
                int _anno = int.Parse(txtCalAnno.Text);
                int _ore = int.Parse(txtCalOre.Text);
                int _minuti = int.Parse(txtCalMinuti.Text);

                _esito = _cb.ForzaOrologio(_giorno, _mese, _anno, _ore, _minuti);
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

        private void btnMemSaveExec_Click(object sender, EventArgs e)
        {
            try
            {
                bool DatiPresenti = false;
                AreaDatiRegen BloccoDati;
                llModelloBlocco BloccoAttivo;

                //Se manca il filename esco
                if(txtMemFileSave.Text == "")
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                // 1 : vuoto l'immagine
                ImmagineChargerMem = new FileSetupRigeneratore();
                // txtMemRegenNumBlocchi.Text = "0";
                // lblMemRegenAvanzamentoRead.Visible = true;
                // Application.DoEvents();


                // Step 1:programmazioni
                if (chkMemFsProgr.Checked)
                {
                    BloccoAttivo = _cb.Memoria.MappaCorrente.Programmazioni;

                    BloccoDati = new AreaDatiRegen();
                    BloccoDati.IdBlocco = 1;
                    BloccoDati.Tipo = AreaDatiRegen.TipoArea.Programmazioni;
                    BloccoDati.StartAddress = BloccoAttivo.AddrArea;
                    BloccoDati.NumBlocchi = BloccoAttivo.NumPagine;

                    if ( _cb.CaricaBloccoDati(BloccoDati,false))
                    {
                        DatiPresenti = true;
                        ImmagineChargerMem.ListaBlocchi.Add(BloccoDati);
                    }
                }

                // Step 2: contatori
                if (chkMemFsCount.Checked)
                {
                    BloccoAttivo = _cb.Memoria.MappaCorrente.Contatori;

                    BloccoDati = new AreaDatiRegen();
                    BloccoDati.IdBlocco = 2;
                    BloccoDati.Tipo = AreaDatiRegen.TipoArea.Contatori;
                    BloccoDati.StartAddress = BloccoAttivo.AddrArea;
                    BloccoDati.NumBlocchi = BloccoAttivo.NumPagine;

                    if (_cb.CaricaBloccoDati(BloccoDati, false))
                    {
                        DatiPresenti = true;
                        ImmagineChargerMem.ListaBlocchi.Add(BloccoDati);
                    }
                }

                // Step 3/4: Cicli
                if (chkMemFsCycle.Checked)
                {
                    BloccoAttivo = _cb.Memoria.MappaCorrente.RecordLunghi;

                    BloccoDati = new AreaDatiRegen();
                    BloccoDati.IdBlocco = 3;
                    BloccoDati.Tipo = AreaDatiRegen.TipoArea.CicliLunghi;
                    BloccoDati.StartAddress = BloccoAttivo.AddrArea;
                    BloccoDati.NumBlocchi = BloccoAttivo.NumPagine;

                    if (_cb.CaricaBloccoDati(BloccoDati, false))
                    {
                        DatiPresenti = true;
                        ImmagineChargerMem.ListaBlocchi.Add(BloccoDati);
                    }

                    BloccoAttivo = _cb.Memoria.MappaCorrente.RecordBrevi;

                    BloccoDati = new AreaDatiRegen();
                    BloccoDati.IdBlocco = 4;
                    BloccoDati.Tipo = AreaDatiRegen.TipoArea.CicliBrevi;
                    BloccoDati.StartAddress = BloccoAttivo.AddrArea;
                    BloccoDati.NumBlocchi = BloccoAttivo.NumPagine;

                    if (_cb.CaricaBloccoDati(BloccoDati, false))
                    {
                        DatiPresenti = true;
                        ImmagineChargerMem.ListaBlocchi.Add(BloccoDati);
                    }

                }


                // Se ho letto qualcosa, salvo il file
                string JsonData = JsonConvert.SerializeObject(ImmagineChargerMem);
                Log.Debug("file generato");
                // Prima comprimo i dati

                if (false) //chkPackComprimiFile.Checked)
                {
                    Log.Debug("file generato");
                    // Prima comprimo i dati
                    string JsonZip = FunzioniComuni.CompressString(JsonData);

                    // Ora cifro i dati
                    // string JsonEncript = StringCipher.Encrypt(JsonZip);
                    // PER INCOMPATIBILITA' DEL METODO DI CIFRATUTA CON NET CORE (XAMARIN)
                    // RIMOSSA LA CIFRATURA DEL PACCHETTO  (16/07/2020)
                    // JsonEncript = JsonData;
                    // Log.Debug("file criptato");

                    JsonData = JsonZip;

                }

                // string JsonZip = FunzioniComuni.CompressString(JsonData);

                File.WriteAllText(txtMemFileSave.Text, JsonData);
                MessageBox.Show("File salvato", "Lettura file dati ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Log.Debug("file salvato");
                this.Cursor = Cursors.Default;

            }
            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
                this.Cursor = Cursors.Default;
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnGenCambiaBaudRate_Click(object sender, EventArgs e)
        {
            try
            {
                bool Esito;
                BaudRate TempBR = new BaudRate();
                TempBR.Mode = BaudRate.BRType.BR_115200;
                TempBR.Speed = 0;

                if (optGenBR115.Checked)
                {
                    TempBR.Mode = BaudRate.BRType.BR_115200;
                    TempBR.Speed = 0;
                }


                if (optGenBR1M.Checked)
                {
                    TempBR.Mode = BaudRate.BRType.BR_CUSTOM;
                    TempBR.Speed = 1000000;
                }


                if (optGenBR3M.Checked)
                {
                    TempBR.Mode = BaudRate.BRType.BR_CUSTOM;
                    TempBR.Speed = 3000000;
                }

                this.Cursor = Cursors.WaitCursor;


                Esito = _cb.ImpostaBaudRate(TempBR);

                this.Cursor = Cursors.Default;

            }

            catch (Exception Ex)
            {
                Log.Error("btnPaProfileClear_Click: " + Ex.Message);
                this.Cursor = Cursors.Default;
            }

        }

        private void btnMemFileSaveSRC_Click(object sender, EventArgs e)
        {
            sfdExportDati.Filter = "LL Memory Image File (*.llmem)|*.llmem|All files (*.*)|*.*";
            DialogResult esito = sfdExportDati.ShowDialog();
            if (esito == DialogResult.OK)
            {
                txtMemFileSave.Text = sfdExportDati.FileName;
            }
        }

        private void btnMemRewriteExec_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = txtMemFileRead.Text;
                string _fileDecompress = "";

                if (File.Exists(filePath))
                {
                    string _infile = File.ReadAllText(filePath);

                    // verifico se è compresso
                    _fileDecompress = FunzioniComuni.DecompressString(_infile);
                    if (_fileDecompress != "")
                    {

                        //è compresso
                        _infile = _fileDecompress;

                    }

                    ImmagineChargerMem = JsonConvert.DeserializeObject<FileSetupRigeneratore>(_infile);
                    Log.Debug("file caricato");
                    if (ImmagineChargerMem != null)
                    {

                        if (ImmagineChargerMem.ListaBlocchi.Count < 1) return;

                        foreach (AreaDatiRegen BloccoDati in ImmagineChargerMem.ListaBlocchi)
                        {
                            bool esito = _cb.ScriviBloccoDati(BloccoDati, false);
                            if (!esito)
                            {
                                MessageBox.Show("Scrittura Area " + BloccoDati.IdBlocco.ToString() + " non riuscita", "Scrittura dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    MessageBox.Show("Scrittura Memoria Sequenze/Procedure completata", "Scrittura dati ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

            catch (Exception Ex)
            {
                Log.Error("cmdMemRead_Click: " + Ex.Message);
            }

        }

        public virtual void applicaAutorizzazioni()
        {
            try
            {
                bool _enabled;
                bool _readonly;
                int LivelloCorrente;
                if (_logiche.currentUser != null)
                {
                    LivelloCorrente = _logiche.currentUser.livello;
                }
                else
                {
                    LivelloCorrente = 99;
                }

                if (LivelloCorrente > 1)
                {
                    tabCaricaBatterie.TabPages.Remove(tabMonitor);
                    tabCaricaBatterie.TabPages.Remove(tbpProxySig60);
                    tabCaricaBatterie.TabPages.Remove(tabMemRead);
                    grbCalData.Visible = false;


//                    txtSerialNumber.ReadOnly = false;
//                    grbAbilitazioneReset.Visible = true;
                }
                if (LivelloCorrente < 2)
                {
//                    grbMainDlOptions.Visible = true;

                }



            }
            catch (Exception Ex)
            {
                Log.Error("applicaAutorizzazioni: " + Ex.Message);
            }

        }




        private void btnPaProfileNEW_Click(object sender, EventArgs e)
        {
            try
            {
                tbcPaSchedeValori.Visible = false;
                cmbPaTipoBatteria.SelectedIndex = 0;
                chkPaUsaSpyBatt.Checked = false;
                cmbPaTipoBatteria.Enabled = true;

                var myImage = new Bitmap(Properties.Resources.ICOLadeLight);
                picPaImmagineProfilo.BackColor = Color.White;
                picPaImmagineProfilo.Image = myImage;
                grbPaImpostazioniLocali.Enabled = true;
            }
            catch (Exception Ex)
            {
                Log.Error("btnPaProfileNEW_Click: " + Ex.Message);
            }
        }

        private void chkPaAttivaEqual_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                lblPaAttivaEqual.Enabled = chkPaAttivaEqual.Enabled;
            }
            catch (Exception Ex)
            {
                Log.Error("chkPaAttivaEqual_EnabledChanged: " + Ex.Message);
            }
        }

        private void chkPaAttivaMant_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                lblPaAttivaMant.Enabled = chkPaAttivaMant.Enabled;
            }
            catch (Exception Ex)
            {
                Log.Error("chkPaAttivaMant_EnabledChanged: " + Ex.Message);
            }
        }

        private void chkPaAttivaOppChg_EnabledChanged(object sender, EventArgs e)
        {
            try
            {
                lblPaAttivaOppChg.Enabled = chkPaAttivaOppChg.Enabled;
            }
            catch (Exception Ex)
            {
                Log.Error("chkPaAttivaOppChg_EnabledChanged: " + Ex.Message);
            }
        }
    }
}

