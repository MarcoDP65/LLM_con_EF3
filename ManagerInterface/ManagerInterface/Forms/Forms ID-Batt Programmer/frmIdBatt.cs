using BrightIdeasSoftware;
using ChargerLogic;
using log4net;
using MoriData;
using Newtonsoft.Json;
using PannelloCharger.Forms;
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
using static ChargerLogic.MessaggioSpyBatt.EsitoMessaggio;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//  frmSuperCharger

namespace PannelloCharger
{
    public partial class frmIdBatt : Form

    {
        parametriSistema _parametri;
        // SerialMessage _msg;
        LogicheBase _logiche;

        //public CicloDiCarica ParametriProfilo;
        public ModelloCiclo ModCicloCorrente = new ModelloCiclo();

        private IDBatt _cb;

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


        private List<TabPage> PagineNascoste = new List<TabPage>();

        private llParametriApparato _tempParametri;


        public frmIdBatt(ref parametriSistema _par, bool CaricaDati, string IdApparato, LogicheBase Logiche, bool SerialeCollegata, bool AutoUpdate)
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

                LeggiCbDaArchivio(ref _par, CaricaDati, IdApparato, "IB");


                InizializzaScheda();
                applicaAutorizzazioni();
                RidimensionaControlli();

                this.Cursor = Cursors.Arrow;

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }

        }

        public frmIdBatt(ref parametriSistema _par, bool CaricaDati)
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
                //_msg = new SerialMessage();
                _cb = new IDBatt(ref _parametri, _logiche.dbDati.connessione, IDBatt.TipoCaricaBatteria.SuperCharger);
                InizializzaScheda();

                // Ora apro esplicitamente il canale. se fallisco esco direttamente
               // _cb.apparatoPresente = true;
                _apparatoPresente = true;

                _esito = _cb.VerificaPresenza();
                _tempParametri = new llParametriApparato();

                // _esito = _cb.CaricaApparatoA0();  
                _esito = true;
                if (_esito)
                {

                    _tempParametri = _cb.ParametriApparato;


                    // ora carico il ciclo corrente e i contatori programmazioni
                    CaricaProgrammazioni();

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
                _parametri = _par;
                InitializeComponent();
                ResizeRedraw = true;


                //_msg = new SerialMessage();
                _cb = new IDBatt(ref _parametri, _logiche.dbDati.connessione, IDBatt.TipoCaricaBatteria.SuperCharger);
                InizializzaScheda();



                _tempParametri = new llParametriApparato();

                _tempParametri = _cb.ParametriApparato;



                _cb.ApparatoLL = new LadeLightData(_logiche.dbDati.connessione, _tempParametri);


                txtGenIdApparato.Text = _cb.ApparatoLL._ll.Id;
                //txtGenSerialeZVT.Text = _cb.ApparatoLL._ll.SerialeZVT;

                txtGenMatricola.Text = _cb.Intestazione.Matricola.ToString();

                _cbCollegato = true;



                MostraProgrammazioni();

                _cb.ApparatoLL.salvaDati();

                return true;



                return false;

            }

            catch
            {
                return false;
            }

        }






        public bool LeggiCbDaArchivio(ref parametriSistema _par, bool CaricaDati, string IdApparato,string TipoApparato)
        {
            bool _esito=true;
            try
            {

                ResizeRedraw = true;
                _cb = new IDBatt(ref _parametri, _logiche.dbDati.connessione, IDBatt.TipoCaricaBatteria.SuperCharger);
                _cb.CaricaProgrammazioniDB("IDBATT","IB");
                _apparatoPresente = false;
                //_esito = _cb.VerificaPresenza();
                _tempParametri = new llParametriApparato();
                //_esito = _cb.CaricaApparatoA0(IdApparato);

                if (_esito)
                {

                    _tempParametri = _cb.ParametriApparato;


                    //_cb.ApparatoLL = new LadeLightData(_logiche.dbDati.connessione, _tempParametri);


                    txtGenIdApparato.Text = "IDBATT";

                    _cb.Programmazioni.CaricaDatiDB("IDBATT", "IB");

                    //txtGenMatricola.Text = _cb.Intestazione.Matricola.ToString();
                    //txtGenModello.Text = _cb.Intestazione.modello;

                    _cbCollegato = true;

                    //CaricaContatori(IdApparato);

                    //_cb.DatiCliente = new llDatiCliente(_logiche.dbDati.connessione);
                    //_cb.DatiCliente.caricaDati(IdApparato,1);


                    // ora carico il ciclo corrente e i contatori programmazioni
                    InizializzaScheda();
                    LeggiProgrammazioniDB(IdApparato, TipoApparato);


                    //ModoArchivio();
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

                btnCaricaContatori.Visible = false;
                btnGenAzzzeraContatori.Visible = false;
                btnGenAzzzeraContatoriTot.Visible = false;
                btnPaAttivaConfigurazione.Visible = false;
               // btnCicloCorrente.Visible = false;
               // btnPaProfileChiudiCanale.Visible = false;
                btnPaProfileRefresh.Visible = false;
                chkPaSbloccaValori.Visible = false;
                lblPaSbloccaValori.Visible = false;
                btnPaCaricaCicli.Visible = false;
                btnPaSalvaDati.Visible = false;

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("ModoArchivio: " + Ex.Message);
                return false;
            }

        }



        public frmIdBatt()
        {
            InitializeComponent();
            InizializzaScheda();

        }

        /// <summary>
        /// Inizializza le combo; da rendere dinamico in effetivo
        /// </summary>
        private void InizializzaScheda()
        {



            cmbPaTipoBatteria.DataSource = _parametri.ParametriProfilo.ModelliBatteria;  //   TipiBattria;
            cmbPaTipoBatteria.ValueMember = "BatteryTypeId";
            cmbPaTipoBatteria.DisplayMember = "BatteryType";

            cmbGenModelloCB.DataSource = _cb.DatiBase.ModelliLL;
            cmbGenModelloCB.ValueMember = "IdModelloLL";
            cmbGenModelloCB.DisplayMember = "NomeModello";

            //            cmbGenModelloCB.SelectedItem = _cb.DatiBase.ModelliLL.Where(x => ((_llModelloCb)x).IdModelloLL == 0xEE);
            cmbGenModelloCB.SelectedIndex = _cb.DatiBase.ModelliLL.IndexOf(_cb.DatiBase.ModelliLL.FirstOrDefault(x => ((_llModelloCb)x).IdModelloLL == 0xEE));



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
                // Carico i valori impostati nelle textbox
                esito = LeggiValoriParametriCarica();


                bool esitoSalvataggio = false;
                if (esito)
                {
                    // Riscrivo i valori nelle textBox per conferma poi salvo i valori 
                    ModCicloCorrente.GeneraProgrammaCarica(_logiche.dbDati.connessione);



                    _cb.Programmazioni.ProgrammaAttivo = ModCicloCorrente.ProfiloRegistrato;
                    _cb.Programmazioni.ProgrammaAttivo.ListaParametri = ModCicloCorrente.ListaParametri;
                    _cb.Programmazioni.ProgrammaAttivo.AnalizzaListaParametri();
                    _cb.Programmazioni.ProgrammaAttivo.IdApparato = "IDBATT";
                    _cb.Programmazioni.ProgrammaAttivo.TipoApparato = "IB";

                    if (ModCicloCorrente.IdProgramma == 0)
                    {
                        _cb.Programmazioni.ProgrammaAttivo.IdApparato = "IDBATT";
                        _cb.Programmazioni.ProgrammaAttivo.TipoApparato = "IB";

                        //Nuovo programma, creo un nuovo ID
                        ModCicloCorrente.IdProgramma = _cb.Programmazioni.ProgrammaAttivo.NewLocalId("IDBATT", "IB");
                        _cb.Programmazioni.ProgrammaAttivo.IdProgramma = ModCicloCorrente.IdProgramma;
                        if (ModCicloCorrente.IdProgramma == 0)
                        {
                            // Se è ancora a 0, errore ed esco
                            return false;
                        }
                    }

                    esitoSalvataggio = _cb.Programmazioni.ProgrammaAttivo.salvaDati();

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
                    ModCicloCorrente.NomeProfilo = _tempStr;

                    // Descrizione
                    ModCicloCorrente.DescrizioneProfilo = txtPaDescrizioneSetup.Text.Trim();

                    // Cassone
                    ModCicloCorrente.ValoriCiclo.TipoCassone = FunzioniMR.ConvertiUshort(txtPaCassone.Text, 1, 0);


                    // Generale



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
                    ModCicloCorrente.ValoriCiclo.TensionedV = FunzioniMR.ConvertiUshort(txtPadV.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.TempodT = FunzioniMR.ConvertiUshort(txtPadT.Text, 1, 0);
                    ModCicloCorrente.ValoriCiclo.TempoFinale = FunzioniMR.ConvertiUshort(txtPaTempoFin.Text, 1, 0);


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
                    ModCicloCorrente.ValoriCiclo.TemperaturaLimite = FunzioniMR.ConvertiUshort(txtPaTempLimite.Text, 1, 0);

                }
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("LeggiValoriParametriCarica: " + Ex.Message);
                return false;
            }
        }
        public bool DatiProfiloCambiati()
        {
            try
            {
                elementiComuni.EsitoVerificaParametri Test = VerificaValoriParametriCarica();
                if (Test!= elementiComuni.EsitoVerificaParametri.Ok)
                {
                    return false;
                }
                return true;

            }
            catch
            {
                return true;
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
                if (ModCicloCorrente.ValoriCiclo.TensionedV != FunzioniMR.ConvertiUshort(txtPadV.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.TempodT != FunzioniMR.ConvertiUshort(txtPadT.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;
                if (ModCicloCorrente.ValoriCiclo.TempoFinale != FunzioniMR.ConvertiUshort(txtPaTempoFin.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;

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
                if (ModCicloCorrente.ValoriCiclo.TemperaturaLimite != FunzioniMR.ConvertiUshort(txtPaTempLimite.Text, 1, 0)) return elementiComuni.EsitoVerificaParametri.ErroreGenerico;

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
                //txtPaDescrizioneSetup.Text = "";


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
            bool _esito = false;
            try
            {

                

                //_esito = _cb.CaricaAreaContatori();


                if (_esito)
                {

                  //  MostraContatori();

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



                _esito = true; // _cb.CaricaContatori(IdApparato);


                if (_esito)
                {

                    //MostraContatori();

                }

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaAreaContatori: " + Ex.Message);
            }
        }



  
 
 

        private void tabCaricaBatterie_Selected(object sender, TabControlEventArgs e)
        {
            try
            {



                if (e.TabPage == tabProfiloAttuale)
                {
                    RidimensionaControlli();
                    // if (_apparatoPresente) CaricaCicloAttuale(); 
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat_tabCaricaBatterie_Selected: " + Ex.Message);

            }
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

          private void btnPaSalvaDati_Click(object sender, EventArgs e)
        {
            bool esitoSalvataggio = false;
            this.Cursor = Cursors.WaitCursor;
            esitoSalvataggio = ScriviParametriCarica();

            if (esitoSalvataggio)
            {

                CaricaProgrammazioni();
                MessageBox.Show("Configurazione Aggiornata", "CONFIGURAZIONE", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Aggiornamento configurazione non riuscito", "CONFIGURAZIONE", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            CaricaProgrammazioni();

            this.Cursor = Cursors.Default;
        }





        private void btnVarFilesearch_Click(object sender, EventArgs e)
        {
            {
                sfdExportDati.Filter = "Comma Separed (*.csv)|*.csv|All files (*.*)|*.*";
                sfdExportDati.ShowDialog();
                //txtVarFileCicli.Text = sfdExportDati.FileName;

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
            // if(_apparatoPresente) LeggiInizializzazione();
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
                            txtPaTensione.ReadOnly = false;
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
                            chkPaAttivaEqual.Enabled = false;
                            //lblPaAttivaEqual.Enabled = true;
                            break;
                        case 0xFF:
                            chkPaAttivaEqual.Checked = true;
                            chkPaAttivaEqual.Enabled = true;
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
                        case 0x0F:
                            chkPaAttivaMant.Checked = true;
                            chkPaAttivaMant.Enabled = false;
                            break;
                        case 0xFF:
                            chkPaAttivaMant.Checked = true;
                            chkPaAttivaMant.Enabled = true;
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
                        case 0x0F:
                            chkPaAttivaOppChg.Checked = true;
                            chkPaAttivaOppChg.Enabled = false;
                            break;
                        case 0xFF:
                            chkPaAttivaOppChg.Checked = true;
                            chkPaAttivaOppChg.Enabled = true;
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
                        case 0x0F:
                            chkPaAttivaRiarmoBms.Checked = true;
                            chkPaAttivaRiarmoBms.Enabled = false;
                            break;
                        case 0xFF:
                            chkPaAttivaRiarmoBms.Checked = true;
                            chkPaAttivaRiarmoBms.Enabled = true;
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
                flwPaListaConfigurazioni.CheckBoxes = true; 
                flwPaListaConfigurazioni.CheckedAspectName = "Selezionato";

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


 
        private void frmCaricabatterie_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                // Chiudo la connessione


            }
            catch (Exception Ex)
            {
                Log.Error("frmCaricabatterie_FormClosed: " + Ex.Message);
            }
        }

        private void btnSalvaCaricabatteria_Click(object sender, EventArgs e)
        {

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
                ModCicloCorrente = new ModelloCiclo();

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
                        txtPaDescrizioneSetup.Text = ModCicloCorrente.DescrizioneProfilo;
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
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPadV, ModCicloCorrente.ValoriCiclo.TensionedV, ModCicloCorrente.ParametriAttivi.TensionedV, 3, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPadT, ModCicloCorrente.ValoriCiclo.TempodT, ModCicloCorrente.ParametriAttivi.TempodT, 3, SbloccaValori);
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaTempoFin, ModCicloCorrente.ValoriCiclo.TempoFinale, ModCicloCorrente.ParametriAttivi.TempoFinale, 3, SbloccaValori);

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
                        case 0x0F:
                            ModCicloCorrente.ParametriAttivi.EqualAttivabile = 1;
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
                        case 0x0F:
                            ModCicloCorrente.ParametriAttivi.MantAttivabile = 1;
                            break;
                        case 0xFF:
                            ModCicloCorrente.ParametriAttivi.MantAttivabile = 4;
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
                        case 0x0F:
                            ModCicloCorrente.ParametriAttivi.OpportunityAttivabile = 1;
                            break;
                        case 0xFF:
                            ModCicloCorrente.ParametriAttivi.OpportunityAttivabile = 4;
                            break;
                        default:
                            ModCicloCorrente.ParametriAttivi.OpportunityAttivabile = 1;
                            break;

                    }
                    //ModCicloCorrente.ParametriAttivi.OpportunityAttivabile = 4;

                    FunzioniUI.ImpostaCheckBoxUshort(ref chkPaAttivaOppChg, ref lblPaAttivaOppChg, ModCicloCorrente.ValoriCiclo.OpportunityAttivabile, ModCicloCorrente.ParametriAttivi.OpportunityAttivabile, 3, SbloccaValori);

                    if (chkPaAttivaOppChg.Checked)
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
                    FunzioniUI.ImpostaTextBoxUshort(ref txtPaTempLimite, ModCicloCorrente.ValoriCiclo.TemperaturaLimite, ModCicloCorrente.ParametriAttivi.TemperaturaLimite, 3, SbloccaValori);

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
                Esito = _cb.Programmazioni.CaricaDatiDB("IDBATT", "IB");
                //Esito = _cb.LeggiProgrammazioni();

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



        public bool LeggiProgrammazioniDB(string IdApparato, string TipoApparato)
        {
            try
            {
                bool Esito;

                Esito = _cb.CaricaProgrammazioniDB(IdApparato, TipoApparato);

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


        private void flwPaListaConfigurazioni_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;
                //llMemoriaCicli CicloSel;+

                if (flwPaListaConfigurazioni.SelectedObject == null)
                {
                    // btnPaAttivaConfigurazione.Enabled = false;
                }
                else
                {
                    /*
                     
                    llProgrammaCarica tmpRigaSel = (llProgrammaCarica)flwPaListaConfigurazioni.SelectedObject;

                    if (tmpRigaSel.PosizioneCorrente != 0)
                        btnPaAttivaConfigurazione.Enabled = true;
                    else
                        btnPaAttivaConfigurazione.Enabled = false;

                    */

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

                if (flwPaListaConfigurazioni.SelectedObject == null)
                {
                    return;
                }
                else
                {
                    if (_cb.Programmazioni.ProgrammaAttivo != null) 
                    {
                        // verifica se ho mofdifiche
                        if (DatiProfiloCambiati())
                        {
                            // Dati cambiati; messaggio salvataggio
                            DialogResult Risposta = MessageBox.Show("Salvo i dati correnti prima di caricare i nuovi ?", "PARAMETRI CICLO", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            if (Risposta == DialogResult.Cancel)
                            {
                                // Cancello selezione; non faccion nulla
                                return;
                            }
                            if (Risposta == DialogResult.OK)
                            {
                                // --> save
                            }
                        }
                        
                        ProfiloInCaricamento = true;
                        _cb.Programmazioni.ProgrammaAttivo = (llProgrammaCarica)flwPaListaConfigurazioni.SelectedObject;
                        ModCicloCorrente.ProfiloRegistrato = _cb.Programmazioni.ProgrammaAttivo;
                        //ModCicloCorrente.GeneraListaValori();
                        ModCicloCorrente.EstraiDaProgrammaCarica();

                        MostraParametriCiclo(true, false, chkPaSbloccaValori.Checked);
                        ProfiloInCaricamento = false;

                    }
                    tbcPaSottopagina.SelectedIndex = 1;
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
                    //_cb.AzzeraContatori();
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
                    //_cb.AzzeraContatori(true);
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
                //_cb.ResetScheda();
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
                if ( false ) //_cb.StopComunicazione())
                {
                    btnPaSalvaDati.Enabled = false;

                }

            }
            catch (Exception Ex)
            {
                Log.Error("btnPaProfileChiudiCanale_Click: " + Ex.Message);
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
                // verifica se ho mofdifiche
                if (DatiProfiloCambiati())
                {
                    // Dati cambiati; messaggio salvataggio
                    DialogResult Risposta = MessageBox.Show("Salvo i dati correnti prima di caricare i nuovi ?", "PARAMETRI CICLO", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (Risposta == DialogResult.Cancel)
                    {
                        // Cancello selezione; non faccion nulla
                        return;
                    }
                    if (Risposta == DialogResult.OK)
                    {
                        // --> save
                    }
                }

                ModCicloCorrente = new ModelloCiclo();

                _cb.Programmazioni.ProgrammaAttivo = new llProgrammaCarica(_logiche.dbDati.connessione);
                _cb.Programmazioni.ProgrammaAttivo._llprc.IdApparato = "ID-BATT";
                _cb.Programmazioni.ProgrammaAttivo._llprc.TipoApparato = "IB";

                tbcPaSchedeValori.Visible = false;
                cmbPaTipoBatteria.SelectedIndex = 0;
                chkPaUsaSpyBatt.Checked = false;
                cmbPaTipoBatteria.Enabled = true;

                var myImage = new Bitmap(Properties.Resources.ICOLadeLight);
                picPaImmagineProfilo.BackColor = Color.White;
                picPaImmagineProfilo.Image = myImage;
                grbPaImpostazioniLocali.Enabled = true;
                tbcPaSottopagina.SelectedIndex = 1;


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

        private void btnPaNomeFileProfiliSRC_Click(object sender, EventArgs e)
        {
            if (sfdExportDati.InitialDirectory == "") sfdExportDati.InitialDirectory = PannelloCharger.Properties.Settings.Default.pathFilesProfili;
            sfdExportDati.Filter = "LL Profile Parameter File (*.llpp)|*.llpp|All files (*.*)|*.*";
            DialogResult esito = sfdExportDati.ShowDialog();
            if (esito == DialogResult.OK)
            {
                txtPaNomeFileProfili.Text = sfdExportDati.FileName;
                PannelloCharger.Properties.Settings.Default.pathFilesProfili = Path.GetDirectoryName(sfdExportDati.FileName);

            }
        }

        private void btnPaSalvaFile_Click(object sender, EventArgs e)
        {
            try
            {
                bool DatiPresenti = false;

                FileItemsModProfilo FileProfili = new FileItemsModProfilo();    




                AreaDatiRegen BloccoDati;
                llModelloBlocco BloccoAttivo;

                //Se manca il filename esco
                if (txtPaNomeFileProfili.Text == "")
                {
                    return;
                }

                if (_cb.Programmazioni.ProgrammiDefiniti.Count < 1)
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                // public List<llProgrammaCarica> ProgrammiDefiniti;
                FileProfili.ListaProfili = new List<ItemModProfilo>();  
                foreach (llProgrammaCarica Prog in _cb.Programmazioni.ProgrammiDefiniti )
                {
                    if(!chkPaSoloSelezionati.Checked || Prog.Selezionato)
                    {
                        ItemModProfilo Item = new ItemModProfilo();
                        Item.NomeProfilo = Prog.ProgramName;
                        Item.NoteProfilo = Prog.ProgramName;
                        Item.Tensione = Prog.BatteryVdef;
                        Item.Capacita = Prog.BatteryAhdef;
                        Item.NumeroCelle = Prog.NumeroCelle;
                        Item.TipoBatteria = Prog.TipoBatteria;
                        Item.DurataMaxCarica = Prog.DurataMaxCarica;
                        Item.ListaParametri = Prog.ListaParametri;
                        Item.IdProgramma = Prog.IdProgramma;
                        Item.IdProfiloCaricaLL = Prog.IdModelloLL;

                        FileProfili.ListaProfili.Add(Item);
                    }
                }

                FileProfili.DataCreazione = DateTime.Now;


                // Se ho letto qualcosa, salvo il file
                string JsonData = JsonConvert.SerializeObject(FileProfili);
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

                File.WriteAllText(txtPaNomeFileProfili.Text, JsonData);
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }



        public bool MostraCicloCorrente()
        {
            try
            {
                txtPaNomeSetup.Text = "";
                txtPaCapacita.Text = "";

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
                chkPaUsaSpyBatt.Checked = false;
                if (_cb.Programmazioni.ProgrammaAttivo != null)
                {
                    txtPaNomeSetup.Text = _cb.Programmazioni.ProgrammaAttivo.ProgramName;
                    txtPaCapacita.Text = FunzioniMR.StringaCapacita(_cb.Programmazioni.ProgrammaAttivo.BatteryAhdef, 10);
                    List<mbTipoBatteria> Lista = (List<mbTipoBatteria>)(cmbPaTipoBatteria.DataSource);
                    cmbPaTipoBatteria.SelectedItem = Lista.Find(x => x.BatteryTypeId == _cb.Programmazioni.ProgrammaAttivo.BatteryType);
                    List<_llProfiloCarica> ListaP = (List<_llProfiloCarica>)(cmbPaProfilo.DataSource);
                    cmbPaProfilo.SelectedItem = ListaP.Find(x => x.IdProfiloCaricaLL == _cb.Programmazioni.ProgrammaAttivo.IdProfilo);
                    List<llTensioneBatteria> ListaV = (List<llTensioneBatteria>)(cmbPaTensione.DataSource);
                    cmbPaTensione.SelectedItem = ListaV.Find(x => x.IdTensione == _cb.Programmazioni.ProgrammaAttivo.BatteryVdef);
                    txtPaTensione.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.BatteryVdef);
                    List<llDurataCarica> ListaD = (List<llDurataCarica>)(cmbPaDurataCarica.DataSource);

                    cmbPaDurataCarica.SelectedItem = ListaD.Find(x => x.IdDurataCaricaLL == _cb.Programmazioni.ProgrammaAttivo.DurataMaxCarica);
                    txtPaTempoT2Min.Text = _cb.Programmazioni.ProgrammaAttivo.DurataMinFase2.ToString();
                    txtPaTempoT2Max.Text = _cb.Programmazioni.ProgrammaAttivo.DurataMaxFase2.ToString();
                    txtPaCoeffK.Text = _cb.Programmazioni.ProgrammaAttivo.PercTempoFase2.ToString();
                    txtPaTempoT3Max.Text = _cb.Programmazioni.ProgrammaAttivo.DurataMaxFase3.ToString();

                    txtPaSogliaVs.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VSoglia);
                    txtPaRaccordoF1.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VRaccordoF1);
                    txtPaVMax.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VMax);
                    txtPaVLimite.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VCellLimite);
                    txtPaVMinRic.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VMinRec);
                    txtPaVMaxRic.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VMaxRec);

                    txtPaNumCelle.Text = _cb.Programmazioni.ProgrammaAttivo.NumeroCelle.ToString();
                    txtPaCorrenteF3.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.CorrenteFase3);


                    txtPaCorrenteI1.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.CorrenteMax);

                    MostraEqualCCorrente();
                    /*
                    if (_cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi >0 || _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa>0)
                    {
                        chkPaAttivaEqual.Checked = true;
                        txtPaEqualNumPulse.Text = _cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi.ToString();
                        txtPaEqualAttesa.Text = _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa.ToString();
                        txtPaEqualPulsePause.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataPausa.ToString();
                        txtPaEqualPulseTime.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataImpulso.ToString();
                        txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.EqualCorrenteImpulso );

                    }
                    else
                    {
                        chkPaAttivaEqual.Checked = false;
                        txtPaEqualNumPulse.Text = "";
                        txtPaEqualAttesa.Text = "";
                        txtPaEqualPulsePause.Text = "";
                        txtPaEqualPulseTime.Text = "";
                        txtPaEqualPulseCurrent.Text = "";

                    }
                    */

                    chkPaUsaSpyBatt.Checked = (_cb.Programmazioni.ProgrammaAttivo.AbilitaComunicazioneSpybatt == 0);


                    if (_cb.Programmazioni.ProgrammaAttivo.TempoAttesaBMS > 0 || _cb.Programmazioni.ProgrammaAttivo.TempoErogazioneBMS > 0)
                    {
                        chkPaAttivaRiarmoBms.Checked = true;
                        txtPaBMSTempoAttesa.Text = _cb.Programmazioni.ProgrammaAttivo.TempoAttesaBMS.ToString();
                        txtPaBMSTempoErogazione.Text = _cb.Programmazioni.ProgrammaAttivo.TempoErogazioneBMS.ToString();
                    }
                    else
                    {
                        chkPaAttivaRiarmoBms.Checked = false;
                        txtPaBMSTempoAttesa.Text = "";
                        txtPaBMSTempoErogazione.Text = "";
                    }


                }




                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool MostraEqualCCorrente()
        {
            try
            {
                mbTipoBatteria _Batteria = (mbTipoBatteria)cmbPaTipoBatteria.SelectedItem;

                txtPaEqualNumPulse.Text = "";
                txtPaEqualAttesa.Text = "";
                txtPaEqualPulsePause.Text = "";
                txtPaEqualPulseTime.Text = "";
                txtPaEqualPulseCurrent.Text = "";

                if (_cb.Programmazioni.ProgrammaAttivo != null)
                {
                    if ((_Batteria.BatteryTypeId & 0x3000) == 0x3000)
                    {
                        lblPaAttivaEqual.Text = TestiProgrammazione.chkEqualizzazioneLI;
                        tbpPaPCEqual.Text = TestiProgrammazione.tabEqualLI;
                        lblPaEqualNumPulse.Text = TestiProgrammazione.lblNumInpulsiLI;
                        lblPaEqualAttesa.Text = TestiProgrammazione.lblTAttesaImpulsoLI;
                        lblPaEqualPulsePause.Text = TestiProgrammazione.lblTempoPausaLI;
                        lblPaEqualPulseTime.Text = TestiProgrammazione.lblTDurataImpulsoLI;
                        lblPaEqualPulseCurrent.Text = TestiProgrammazione.lblCorrenteImpLI;

                    }
                    else
                    {
                        lblPaAttivaEqual.Text = TestiProgrammazione.chkEqualizzazionePB;
                        tbpPaPCEqual.Text = TestiProgrammazione.tabEqualPB;
                        lblPaEqualNumPulse.Text = TestiProgrammazione.lblNumInpulsiPB;
                        lblPaEqualAttesa.Text = TestiProgrammazione.lblTAttesaImpulsoPB;
                        lblPaEqualPulsePause.Text = TestiProgrammazione.lblTempoPausaPB;
                        lblPaEqualPulseTime.Text = TestiProgrammazione.lblTDurataImpulsoPB;
                        lblPaEqualPulseCurrent.Text = TestiProgrammazione.lblCorrenteImpPB;

                    }

                    if (_cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi > 0 || _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa > 0)
                    {
                        chkPaAttivaEqual.Checked = true;
                        txtPaEqualNumPulse.Text = _cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi.ToString();
                        txtPaEqualAttesa.Text = _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa.ToString();
                        txtPaEqualPulsePause.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataPausa.ToString();
                        txtPaEqualPulseTime.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataImpulso.ToString();
                        txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.EqualCorrenteImpulso);

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

                if (_cb.Programmazioni.ProgrammaAttivo != null)
                {
                    if (_cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi > 0 || _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa > 0)
                    {
                        chkPaAttivaEqual.Checked = true;
                        txtPaEqualNumPulse.Text = _cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi.ToString();
                        txtPaEqualAttesa.Text = _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa.ToString();
                        txtPaEqualPulsePause.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataPausa.ToString();
                        txtPaEqualPulseTime.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataImpulso.ToString();
                        txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.EqualCorrenteImpulso);

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
                if (_cb.Programmazioni.ProgrammaAttivo == null)
                {
                    return false;
                }

                // if (ModCicloCorrente.ValoriCiclo.EqualAttivo == 0xF0F0)

                txtPaEqualNumPulse.Text = ModCicloCorrente.ValoriCiclo.EqualNumImpulsi.ToString();
                _cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi = ModCicloCorrente.ValoriCiclo.EqualNumImpulsi;

                txtPaEqualAttesa.Text = ModCicloCorrente.ValoriCiclo.EqualTempoAttesa.ToString();
                _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa = ModCicloCorrente.ValoriCiclo.EqualTempoAttesa;

                txtPaEqualPulsePause.Text = ModCicloCorrente.ValoriCiclo.EqualTempoPausa.ToString();
                _cb.Programmazioni.ProgrammaAttivo.EqualDurataPausa = ModCicloCorrente.ValoriCiclo.EqualTempoPausa;

                txtPaEqualPulseTime.Text = ModCicloCorrente.ValoriCiclo.EqualTempoImpulso.ToString();
                _cb.Programmazioni.ProgrammaAttivo.EqualDurataImpulso = ModCicloCorrente.ValoriCiclo.EqualTempoImpulso;

                txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso);
                _cb.Programmazioni.ProgrammaAttivo.EqualCorrenteImpulso = ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso;


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

                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantAttesa, ModCicloCorrente.ValoriCiclo.MantTempoAttesa, ModCicloCorrente.ParametriAttivi.MantTempoAttesa, 3, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmin, ModCicloCorrente.ValoriCiclo.MantTensIniziale, ModCicloCorrente.ParametriAttivi.MantTensIniziale, 1, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmax, ModCicloCorrente.ValoriCiclo.MantTensFinale, ModCicloCorrente.ParametriAttivi.MantTensFinale, 1, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantDurataMax, ModCicloCorrente.ValoriCiclo.MantTempoMaxErogazione, ModCicloCorrente.ParametriAttivi.MantTempoMaxErogazione, 3, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantCorrente, ModCicloCorrente.ValoriCiclo.MantCorrenteImpulso, ModCicloCorrente.ParametriAttivi.MantCorrenteImpulso, 2, SbloccaValori);


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

                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraInizio, ModCicloCorrente.ValoriCiclo.OpportunityOraInizio, ModCicloCorrente.ParametriAttivi.OpportunityOraInizio, 4, false);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraFine, ModCicloCorrente.ValoriCiclo.OpportunityOraFine, ModCicloCorrente.ParametriAttivi.OpportunityOraFine, 4, false);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppVSoglia, ModCicloCorrente.ValoriCiclo.OpportunityTensioneMax, ModCicloCorrente.ParametriAttivi.OpportunityTensioneMax, 1, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppCorrente, ModCicloCorrente.ValoriCiclo.OpportunityCorrente, ModCicloCorrente.ParametriAttivi.OpportunityCorrente, 2, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppDurataMax, ModCicloCorrente.ValoriCiclo.OpportunityDurataMax, ModCicloCorrente.ParametriAttivi.OpportunityDurataMax, 3, SbloccaValori);

                OppNotturno((ModCicloCorrente.ValoriCiclo.OpportunityOraInizio >= ModCicloCorrente.ValoriCiclo.OpportunityOraFine));

                return true;
            }
            catch
            {
                return false;
            }

        }

        private void cmbGenModelloCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if(cmbGenModelloCB.SelectedItem != null)
                {
                    _llModelloCb CbSel = (_llModelloCb)cmbGenModelloCB.SelectedItem;

                    _tempParametri = new llParametriApparato();
                    _tempParametri.llParApp.IdApparato = "IDBATT";
                    _tempParametri.llParApp.TipoApparato = CbSel.IdModelloLL;
                    _tempParametri.llParApp.VMin = FunzioniMR.ConvertiUshort(CbSel.TensioneMin, 100, 0);

                    _tempParametri.llParApp.VMax = FunzioniMR.ConvertiUshort(CbSel.TensioneMax, 100, 0);
                    txtGenTensioneMax.Text = FunzioniMR.StringaCapacitaUint(_tempParametri.llParApp.VMax, 100, 1);

                    _tempParametri.llParApp.Amax = FunzioniMR.ConvertiUshort(CbSel.CorrenteMax, 10, 0);
                    txtGenCorrenteMax.Text = FunzioniMR.StringaCapacitaUint(_tempParametri.llParApp.Amax, 10, 0);

                    _cb.ParametriApparato = _tempParametri;
                    _cb.ModelloCorrente = CbSel;

                }
                else
                {
                    _tempParametri = null;
                    txtGenTensioneMax.Text = "";
                    txtGenCorrenteMax.Text = "";
                    _cb.ParametriApparato = _tempParametri;

                }

            }
            catch (Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ModCicloCorrente = new ModelloCiclo();

                _cb.Programmazioni.ProgrammaAttivo = new llProgrammaCarica(_logiche.dbDati.connessione);
                _cb.Programmazioni.ProgrammaAttivo._llprc.IdApparato = "ID-BATT";
                _cb.Programmazioni.ProgrammaAttivo._llprc.TipoApparato = "IB";

                tbcPaSchedeValori.Visible = false;
                cmbPaTipoBatteria.SelectedIndex = 0;
                chkPaUsaSpyBatt.Checked = false;
                cmbPaTipoBatteria.Enabled = true;

                var myImage = new Bitmap(Properties.Resources.ICOLadeLight);
                picPaImmagineProfilo.BackColor = Color.White;
                picPaImmagineProfilo.Image = myImage;
                grbPaImpostazioniLocali.Enabled = true;
                tbcPaSottopagina.SelectedIndex = 1;
            }
            catch (Exception Ex)
            {
                Log.Error("btnPaProfileNEW_Click: " + Ex.Message);
            }
        }

        private void btnPaCancellaSelezionati_Click(object sender, EventArgs e)
        {
            try
            {
                if(_cb.Programmazioni.ProgrammiDefiniti.Count > 0)
                {
                    List<llProgrammaCarica> TempList = new List<llProgrammaCarica>();
                    foreach (llProgrammaCarica tempPrg in _cb.Programmazioni.ProgrammiDefiniti)
                    {
                        if (tempPrg.Selezionato)
                        {
                            TempList.Add(tempPrg);  
                        }
                    }
                    foreach (llProgrammaCarica tempPrg in TempList)
                    {
                        tempPrg._database = _logiche.dbDati.connessione;
                        tempPrg.CancellaRecord();
                        _cb.Programmazioni.ProgrammiDefiniti.Remove(tempPrg);   
                    }


                }
                MostraProgrammazioni();
            }
            catch (Exception)
            {

            }
        }

        private void btnPaGeneraQr_Click(object sender, EventArgs e)
        {
            try
            {
                if (flwPaListaConfigurazioni.SelectedObject != null)
                {
                    llProgrammaCarica ProgCorr = (llProgrammaCarica)flwPaListaConfigurazioni.SelectedObject;
                    if (ProgCorr != null)
                    {
                        IDBattData IDData = new IDBattData();

                        frmQRCode QRCorrente = new frmQRCode();
                        //QRCorrente.MdiParent = this.ParentForm;
                        QRCorrente.StartPosition = FormStartPosition.CenterParent;
                        QRCorrente.lblNomeProfilo.Text = ProgCorr._llprc.ProgramName;
                        QRCorrente.lblDescrizione.Text = ProgCorr._llprc.ProgramDescription;
                        QRCorrente.lblProfilo.Text = ProgCorr.strTipoProfilo;
                        QRCorrente.lblBatteria.Text = ProgCorr.strTipoBatteria;
                        QRCorrente.lblTensione.Text = ProgCorr.strBatteryVdef;
                        QRCorrente.lblCorrente.Text = ProgCorr.strBatteryAhdef;
                        IDData.Name = ProgCorr._llprc.ProgramName;
                        IDData.Profile = ProgCorr.strTipoProfilo;
                        IDData.Description = ProgCorr._llprc.ProgramDescription;
                        IDData.BatteryType = ProgCorr.strTipoBatteria;
                        IDData.BatteryData = ProgCorr.strBatteryVdef + "V - " + ProgCorr.strBatteryAhdef + "Ah";

                        MessaggioLadeLight.MessaggioProgrammazione NuovoPrg = new MessaggioLadeLight.MessaggioProgrammazione(ProgCorr);
                        if (NuovoPrg.GeneraByteArray())
                        {
                            QRCorrente.lblDataArray.Text = FunzioniComuni.HexdumpArray(NuovoPrg.dataBuffer);
                            IDData.Data = QRCorrente.lblDataArray.Text;
                            QRCorrente.CreaQR(IDData);
                        }

                        QRCorrente.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void chkPaRiarmaBms_Click(object sender, EventArgs e)
        {

        }

        private void flwPaListaConfigurazioni_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            try
            {
                btnPaAttivaConfigurazione_Click(null, null);

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCicli: " + Ex.Message);
                this.Cursor = Cursors.Arrow;
            }


        }

        private void btnPaFileInProfiliSRC_Click(object sender, EventArgs e)
        {
            try
            {
                if (sfdImportDati.InitialDirectory == "") sfdImportDati.InitialDirectory = PannelloCharger.Properties.Settings.Default.pathFilesProfili;
                sfdImportDati.Filter = "LL Profile Parameter File (*.llpp)|*.llpp|All files (*.*)|*.*";
                sfdImportDati.ShowDialog();
                txtPaCaricaFileProfili.Text = sfdImportDati.FileName;

                PannelloCharger.Properties.Settings.Default.pathFilesProfili = Path.GetDirectoryName(sfdImportDati.FileName);
            }
            catch (Exception Ex)
            {
                Log.Error("btnGeneraCsv_Click: " + Ex.Message);
            }
        }

        private void btnPaCaricaFile_Click(object sender, EventArgs e)
        {
            try
            {
                bool DatiPresenti = false;

                FileItemsModProfilo FileProfili = new FileItemsModProfilo();




                AreaDatiRegen BloccoDati;
                llModelloBlocco BloccoAttivo;

                //Se manca il filename esco
                if (txtPaNomeFileProfili.Text == "")
                {
                    return;
                }

                if (_cb.Programmazioni.ProgrammiDefiniti.Count < 1)
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                // public List<llProgrammaCarica> ProgrammiDefiniti;
                FileProfili.ListaProfili = new List<ItemModProfilo>();
                foreach (llProgrammaCarica Prog in _cb.Programmazioni.ProgrammiDefiniti)
                {
                    if (!chkPaSoloSelezionati.Checked || Prog.Selezionato)
                    {
                        ItemModProfilo Item = new ItemModProfilo();
                        Item.NomeProfilo = Prog.ProgramName;
                        Item.NoteProfilo = Prog.ProgramName;
                        Item.Tensione = Prog.BatteryVdef;
                        Item.Capacita = Prog.BatteryAhdef;
                        Item.NumeroCelle = Prog.NumeroCelle;
                        Item.TipoBatteria = Prog.TipoBatteria;
                        Item.DurataMaxCarica = Prog.DurataMaxCarica;
                        Item.ListaParametri = Prog.ListaParametri;
                        Item.IdProgramma = Prog.IdProgramma;
                        Item.IdProfiloCaricaLL = Prog.IdModelloLL;

                        FileProfili.ListaProfili.Add(Item);
                    }
                }

                FileProfili.DataCreazione = DateTime.Now;


                // Se ho letto qualcosa, salvo il file
                string JsonData = JsonConvert.SerializeObject(FileProfili);
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

                File.WriteAllText(txtPaNomeFileProfili.Text, JsonData);
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
    }
}

