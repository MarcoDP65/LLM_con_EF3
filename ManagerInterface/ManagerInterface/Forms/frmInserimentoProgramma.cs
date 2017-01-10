using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MoriData;
using ChargerLogic;
using log4net;
using log4net.Config;
using BrightIdeasSoftware;
using Utility;



namespace PannelloCharger
{
    public partial class frmInserimentoProgramma : Form
    {
        public bool TestMode = false;
        public UnitaSpyBatt _sb;

        private ParametriSetupPro _parametriPro = new ParametriSetupPro();

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        LogicheBase _logiche;

        private bool _attivaEstese = false;
        private bool _attivaPRO = false;




        public frmInserimentoProgramma(LogicheBase Logiche)
        {
            _logiche = Logiche;
            InitializeComponent();

            cmbTipoBatteria.Items.Add("Pb");
            cmbTipoBatteria.Items.Add("Gel");
            cmbTipoBatteria.SelectedIndex = 0;
            ApplicaAutorizzazioni();

            inizializzaPro();
            inizializzaGrigliaTurni();
        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ApplicaAutorizzazioni()
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

                if (LivelloCorrente < 2)
                {
                    chkMemProgrammed.Visible = true;
                    pbxInverso.Enabled = true;
                    //optInversa.Enabled = true;
                    lblNumSpire.Visible = true;
                    txtNumSpire.Visible = true;
                    chkSondaElPresente.Visible = true;
                    txtTempMax.Visible = true;
                    lblTempMax.Visible = true;
                    txtTempMin.Visible = true;
                    lblTempMin.Visible = true;
                    lblTipoBatteria.Visible = true;
                    cmbTipoBatteria.Visible = true;


                }
                else
                {
                    chkMemProgrammed.Visible = false;
                    pbxInverso.Enabled = false;
                    //optInversa.Enabled = false;
                    lblNumSpire.Visible = false;
                    txtNumSpire.Visible = false;
                    chkSondaElPresente.Visible = false;
                    txtTempMax.Visible = false;
                    lblTempMax.Visible = false;
                    txtTempMin.Visible = false;
                    lblTempMin.Visible = false;
                    lblTipoBatteria.Visible = false;
                    cmbTipoBatteria.Visible = false;
                    cmbTipoBatteria.SelectedIndex = 0;

                }




            }
            catch
            {


            }
        }

        private void frmInserimentoProgramma_Load(object sender, EventArgs e)
        {
            if (TestMode)
            {
                this.Text = "NUOVA CONFIGURAZIONE";
                btnInserisciProgramma.Enabled = false;
                return;
            }

            if (_sb == null)
            {
                //MessageBox.Show("Apparato Corrente non definito  /N Impossibile continuare", "Programmazione Apparato", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                MessageBox.Show(StringheComuni.NotDefined + "/N " + StringheComuni.ImpossibileContinuare,StringheComuni.TitoloCfg, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                
            }

            // A questo punto ho la SB caricata. inizializzo in base alla versione FW

            _attivaEstese = _sb.sbData.fwParametriProgEstesa;
            _attivaPRO = _sb.sbData.fwFunzioniPro;

            // Parametri Estesi
            pcbIcoDiretto.Visible = !_attivaEstese;
            pcbIcoInverso.Visible = !_attivaEstese;
            optVersoDiretto.Visible = _attivaEstese;
            optVersoInverso.Visible = _attivaEstese;

            pbxInverso.Enabled = _attivaEstese;            //optInversa.Enabled = false;
            lblNumSpire.Visible = _attivaEstese;
            txtNumSpire.Visible = _attivaEstese;
            chkSondaElPresente.Visible = _attivaEstese;
            txtTempMax.Visible = _attivaEstese;
            lblTempMax.Visible = _attivaEstese;
            txtTempMin.Visible = _attivaEstese;
            lblTempMin.Visible = _attivaEstese;

            if (!_attivaPRO)
            {
                tabImpostazioni.TabPages.Remove(tbpSetupPro);
                tabImpostazioni.TabPages.Remove(tbpSetupTurni);
                tabImpostazioni.TabPages.Remove(tbpSetupTempo);
            }

            this.Text = StringheComuni.NuovaCfg + " " + Convert.ToString(_sb.sbData.ProgramCount + 1);


 
        }

        public bool verificaParametri()
        {
            try
            {
                sbProgrammaRicarica _nuovoProg = new sbProgrammaRicarica();


                _nuovoProg.BatteryVdef = FunzioniMR.ConvertiUshort(txtProgcBattVdef.Text, 100, 0);
                _nuovoProg.BatteryAhdef = FunzioniMR.ConvertiUshort(txtProgcBattAhDef.Text, 10, 0);
                txtProgcBattType.Text = cmbTipoBatteria.SelectedIndex.ToString();
                _nuovoProg.BatteryType = FunzioniMR.ConvertiByte(txtProgcBattType.Text, 1, 0);
                _nuovoProg.BatteryCells = FunzioniMR.ConvertiByte(txtProgcCelleTot.Text, 1, 0);
                _nuovoProg.BatteryCell3 = FunzioniMR.ConvertiByte(txtProgcCelleV3.Text, 1, 0);
                _nuovoProg.BatteryCell2 = FunzioniMR.ConvertiByte(txtProgcCelleV2.Text, 1, 0);
                _nuovoProg.BatteryCell1 = FunzioniMR.ConvertiByte(txtProgcCelleV1.Text, 1, 0);
                _nuovoProg.VersoCorrente = (byte)elementiComuni.VersoCorrente.Diretto;  // se non impostabile >> diretto
                _nuovoProg.AbilitaPresElett = (byte)elementiComuni.AbilitaElemento.Attivo;

                if (_attivaEstese)
                {
                    if (optVersoInverso.Checked)
                        _nuovoProg.VersoCorrente = (byte)elementiComuni.VersoCorrente.Inverso;
                    if (!chkSondaElPresente.Checked)
                        _nuovoProg.AbilitaPresElett = (byte)elementiComuni.AbilitaElemento.Non_Attivo;

                    _nuovoProg.TempMin = FunzioniMR.ConvertiByte(txtTempMin.Text, 1, 60);
                    _nuovoProg.TempMax = FunzioniMR.ConvertiByte(txtTempMax.Text, 1, 80);


                }


                if (_nuovoProg.BatteryCells < 1) return false;
                if (_nuovoProg.BatteryCells > _nuovoProg.BatteryVdef ) return false;

                if (_nuovoProg.BatteryCell3 != 0)
                {
                    if (_nuovoProg.BatteryCell3 >_nuovoProg.BatteryCells) return false;
                }

                if (_nuovoProg.BatteryCell2 != 0)
                {
                    if (_nuovoProg.BatteryCell3 != 0)
                    {
                        if (_nuovoProg.BatteryCell2 > _nuovoProg.BatteryCell2) return false;
                    }
                    else
                    {
                        if (_nuovoProg.BatteryCell2 > _nuovoProg.BatteryCells) return false;
                    }
                }


                if (_nuovoProg.BatteryCell1 != 0)
                {
                    if (_nuovoProg.BatteryCell2 != 0)
                    {
                        if (_nuovoProg.BatteryCell1 > _nuovoProg.BatteryCell2) return false;
                    }
                    else
                    {
                        if (_nuovoProg.BatteryCell1 > _nuovoProg.BatteryCells) return false;
                    }
                }

                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("salvaProgramma: " + Ex.Message);
                return false;
            }

        }

        public bool salvaProgramma()
        {
            try
            {
                sbProgrammaRicarica _nuovoProg = new sbProgrammaRicarica();

                // prima salvo i dati nella classe
                if (_sb.sbData.ProgramCount == 0xFFFF)
                {
                    _nuovoProg.IdProgramma = 1;
                }
                else
                {
                    _nuovoProg.IdProgramma = (ushort)(_sb.sbData.ProgramCount + 1);
                }

                _nuovoProg.BatteryVdef = FunzioniMR.ConvertiUshort(txtProgcBattVdef.Text,100,0);
                _nuovoProg.BatteryAhdef = FunzioniMR.ConvertiUshort(txtProgcBattAhDef.Text, 10, 0);
                txtProgcBattType.Text = cmbTipoBatteria.SelectedIndex.ToString();
                _nuovoProg.BatteryType = FunzioniMR.ConvertiByte(txtProgcBattType.Text,1,0);
                _nuovoProg.BatteryCells = FunzioniMR.ConvertiByte(txtProgcCelleTot.Text, 1, 0);
                _nuovoProg.BatteryCell3 = FunzioniMR.ConvertiByte(txtProgcCelleV3.Text, 1, 0);
                _nuovoProg.BatteryCell2 = FunzioniMR.ConvertiByte(txtProgcCelleV2.Text, 1, 0);
                _nuovoProg.BatteryCell1 = FunzioniMR.ConvertiByte(txtProgcCelleV1.Text, 1, 0);
                _nuovoProg.VersoCorrente = (byte)elementiComuni.VersoCorrente.Diretto;  // se non impostabile >> diretto
                _nuovoProg.AbilitaPresElett = (byte)elementiComuni.AbilitaElemento.Attivo;
                if (chkCliResetContatori.Checked == true)
                {
                    _nuovoProg.ResetContatori = MessaggioSpyBatt.ProgrammaRicarica.NuoviLivelli.ResetLivelli;
                }
                else
                {
                    _nuovoProg.ResetContatori = MessaggioSpyBatt.ProgrammaRicarica.NuoviLivelli.MantieniLivelli;
                }
                if (_attivaEstese)
                {
                    if (optVersoInverso.Checked)
                        _nuovoProg.VersoCorrente = (byte)elementiComuni.VersoCorrente.Inverso;
                    if (!chkSondaElPresente.Checked)
                        _nuovoProg.AbilitaPresElett = (byte)elementiComuni.AbilitaElemento.Non_Attivo;

                    _nuovoProg.TempMin = FunzioniMR.ConvertiByte(txtTempMin.Text, 1, 60);
                    _nuovoProg.TempMax = FunzioniMR.ConvertiByte(txtTempMax.Text, 1, 80);


                }


                 if (_attivaPRO)
                {
                    _nuovoProg.ModoPianificazione = (byte)cmbProModoPianif.SelectedValue;
                    _nuovoProg.CorrenteMinimaCHG = FunzioniMR.ConvertiUshort(txtProMinChargeCurr.Text, 10,0);
                    _nuovoProg.CorrenteMassimaCHG = FunzioniMR.ConvertiUshort(txtProMaxChargeCurr.Text, 10, 0);


                    _nuovoProg.Rabboccatore = (byte)cmbRabboccatore.SelectedValue;
                    _nuovoProg.ImpulsiRabboccatore = FunzioniMR.ConvertiByte(txtProImpulsiRabb.Text, 1, 1);
                    _nuovoProg.Biberonaggio = (byte)cmbBiberonaggio.SelectedValue;
                    _nuovoProg.TempAttenzione = FunzioniMR.ConvertiByte(txtProTempAttenzione.Text, 1, 45);
                    _nuovoProg.TempAllarme = FunzioniMR.ConvertiByte(txtProTempAllarme.Text, 1, 55);
                    _nuovoProg.TempRipresa = FunzioniMR.ConvertiByte(txtProTempRiavvio.Text, 1, 45);
                    //_nuovoProg.MaxSbilanciamento = (ushort)5; // cmbProMaxSbil.SelectedValue;
                    _nuovoProg.DurataSbilanciamento = FunzioniMR.ConvertiUshort(txtProMaxMinutiSbil.Text, 1, 240);
                    _nuovoProg.TensioneGas = FunzioniMR.ConvertiUshort(txtProTensioneGas.Text, 100, 240);
                    _nuovoProg.DerivaInferiore = FunzioniMR.ConvertiMSshort(txtProDerivaUnder.Text, 10, 0x8027);
                    _nuovoProg.DerivaSuperiore = FunzioniMR.ConvertiMSshort(txtProDerivaOver.Text, 10, 0x8027);
                    _nuovoProg.MinCorrenteW = FunzioniMR.ConvertiUshort(txtProMinCurrW.Text, 10, 0);
                    _nuovoProg.MaxCorrenteW = FunzioniMR.ConvertiUshort(txtProMaxCurrW.Text, 10, 0);
                    _nuovoProg.MaxCorrenteImp = FunzioniMR.ConvertiUshort(txtProMaxCurrImp.Text, 10, 0);
                    _nuovoProg.TensioneRaccordo = FunzioniMR.ConvertiUshort(txtProTensioneRaccordo.Text, 100, 245);
                    _nuovoProg.TensioneFinale = FunzioniMR.ConvertiUshort(txtProTensioneFinale.Text, 100, 265);


                }

                 _sb.ScriviProgramma( _nuovoProg ,(chkMemProgrammed.Checked == true ));

                if (_sb.UltimaRisposta != SerialMessage.EsitoRisposta.MessaggioOk)
                {
                   // MessageBox.Show("Programmazione non riuscita", "Programmazioe Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(StringheComuni.CfgFallita, StringheComuni.TitoloCfg, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    return true;
                    //this.Close();
                }
               
            }
            catch (Exception Ex)
            {
                Log.Error("salvaProgramma: " + Ex.Message);
                return false;
            }

        }

        private void btnInserisciProgramma_Click(object sender, EventArgs e)
        {
            if (_sb.apparatoPresente)
            {
                bool _verifica = verificaParametri();

                if(!_verifica)
                {
                    // MessageBox.Show("Programmazione non riuscita", "Programmazioe Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(StringheComuni.CfgNonValida, StringheComuni.TitoloCfg, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                if (salvaProgramma())
                {
                    if (chkMemProgrammed.Checked)
                    {
                        _sb.AttivaProgramma();

                    }
                    this.Close();
                }
            }
        }

        private void pbxInverso_Click(object sender, EventArgs e)
        {

        }

        private void lblNumSpire_Click(object sender, EventArgs e)
        {

        }

        private void txtNumSpire_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtProgcBattVdef_Leave(object sender, EventArgs e)
        {
            try
            {
                ushort _tempTens = FunzioniMR.ConvertiUshort(txtProgcBattVdef.Text, 1, 0);
                if (_tempTens < 24 )
                {
                    _tempTens = 24;
                    txtProgcBattVdef.Text = "24";
                }

                if (_tempTens > 96)
                {
                    _tempTens = 96;
                    txtProgcBattVdef.Text = "96";
                }
                ushort _tempCells = (ushort)(_tempTens / 2);

                txtProgcCelleTot.Text = _tempCells.ToString();
                txtProgcBattVdef.Text = (_tempCells*2).ToString();
            }
            catch
            {

            }
        }

        private void inizializzaPro()
        {
            try
            {
                cmbProModoPianif.DataSource = _parametriPro.TipiPianificazione;
                cmbProModoPianif.DisplayMember = "Descrizione";
                cmbProModoPianif.ValueMember = "Codice";
                cmbProModoPianif.SelectedIndex = 0;

                cmbProMaxSbil.DataSource = _parametriPro.MaxSbilanciamento;
                cmbProMaxSbil.DisplayMember = "strValore";
                cmbProMaxSbil.ValueMember = "ValoreB";

                cmbBiberonaggio.DataSource = _parametriPro.OpzioniBib;
                cmbBiberonaggio.DisplayMember = "Descrizione";
                cmbBiberonaggio.ValueMember = "Codice";
                cmbBiberonaggio.SelectedIndex = 0;

                cmbRabboccatore.DataSource = _parametriPro.OpzioniRabb;
                cmbRabboccatore.DisplayMember = "Descrizione";
                cmbRabboccatore.ValueMember = "Codice";
                cmbRabboccatore.SelectedIndex = 0;



            }
            catch
            {

            }
        }


        private void inizializzaGrigliaTurni()
        {
            try
            {
                OraTurnoMR _OraTempI;
                OraTurnoMR _OraTempF;


                for (int giorno = 1; giorno < 8; giorno++)
                {

                    PannelloTurno P1 = new PannelloTurno();
                    _OraTempI = new OraTurnoMR(5, 45);
                    _OraTempF = new OraTurnoMR(6, 15);
                    P1.InizioCambioTurno = _OraTempI;
                    P1.FineCambioTurno = _OraTempF;
                    P1.BackColor = Color.LightYellow;
                    tlpGrigliaTurni.Controls.Add(P1, 1, giorno+1);

                    PannelloTurno P2 = new PannelloTurno();
                    _OraTempI = new OraTurnoMR(13, 45);
                    _OraTempF = new OraTurnoMR(14, 15);
                    P2.InizioCambioTurno = _OraTempI;
                    P2.FineCambioTurno = _OraTempF;
                    P2.BackColor = Color.LightYellow;
                    tlpGrigliaTurni.Controls.Add(P2, 2, giorno + 1);

                    PannelloTurno P3 = new PannelloTurno();
                    _OraTempI = new OraTurnoMR(21, 45);
                    _OraTempF = new OraTurnoMR(22, 15);
                    P3.InizioCambioTurno = _OraTempI;
                    P3.FineCambioTurno = _OraTempF;
                    P3.BackColor = Color.LightYellow;
                    tlpGrigliaTurni.Controls.Add(P3, 3, giorno + 1);
                }


                lblTurno1.ForeColor = Color.White;
                lblTurno2.ForeColor = Color.White;
                lblTurno3.ForeColor = Color.White;

            }
            catch
            {

            }
        }

        private void txtProgcCelleV3_TextChanged(object sender, EventArgs e)
        {

        }

        private void tlpGrigliaTurni_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void cmbBiberonaggio_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txtTempMin_Leave(object sender, EventArgs e)
        {
            try
            {
                byte _tempTmin = FunzioniMR.ConvertiByte(txtTempMin.Text, 1, 0);
                if (_tempTmin < 10)
                {
                    _tempTmin = 10;
                    txtTempMin.Text = "10";
                }

                if (_tempTmin > 100)
                {
                    _tempTmin = 100;
                    txtTempMin.Text = "100";
                }

            }
            catch (Exception Ex)
            {
                Log.Error("txtTempMin_Leave: " + Ex.Message);
            }

        }

        private void txtTempMax_Leave(object sender, EventArgs e)
        {
            try
            {
                byte _tempTmax = FunzioniMR.ConvertiByte(txtTempMax.Text, 1, 0);
                if (_tempTmax < 10)
                {
                    _tempTmax = 10;
                    txtTempMax.Text = "10";
                }

                if (_tempTmax > 110)
                {
                    _tempTmax = 110;
                    txtTempMax.Text = "110";
                }

            }
            catch (Exception Ex)
            {
                Log.Error("txtTempMin_Leave: " + Ex.Message);
            }

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void txtProMaxMinutiSbil_TextChanged(object sender, EventArgs e)
        {

        }



        private void txtProgcBattAhDef_Leave(object sender, EventArgs e)
        {
            try
            {
                ushort _tempCap = FunzioniMR.ConvertiUshort(txtProgcBattAhDef.Text, 1, 0);
                if (_tempCap < 20)
                {
                    _tempCap = 10;
                    txtProgcBattAhDef.Text = "10";
                }

                if (_tempCap > 2000)
                {
                    _tempCap = 2000;
                    txtProgcBattAhDef.Text = "2000";
                }
                ushort _tempAmps = (ushort)(_tempCap / 12);
                // 28/09/16 - Corrente minima fissa a 5A
                txtProMinChargeCurr.Text = "5";  //_tempAmps.ToString();

                 _tempAmps = (ushort)(_tempCap / 4);
                txtProMaxChargeCurr.Text = _tempAmps.ToString();

                _tempAmps = (ushort)(_tempCap / 13);
                txtProMaxCurrW.Text = _tempAmps.ToString();

                _tempAmps = (ushort)(_tempCap / 24);
                txtProMinCurrW.Text = _tempAmps.ToString();

                _tempAmps = (ushort)(_tempCap / 2);
                txtProMaxCurrImp.Text = _tempAmps.ToString();
            }
            catch (Exception Ex)
            {
                Log.Error("txtProgcBattAhDef_Leave: " + Ex.Message);

            }
        }
    }
}
