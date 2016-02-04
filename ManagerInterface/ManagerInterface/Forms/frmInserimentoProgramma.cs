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
        public UnitaSpyBatt _sb;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        LogicheBase _logiche;

        public frmInserimentoProgramma(LogicheBase Logiche)
        {
            _logiche = Logiche;
            InitializeComponent();

            ApplicaAutorizzazioni();
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

                if (LivelloCorrente < 3) chkMemProgrammed.Visible = true;
                else
                    chkMemProgrammed.Visible = false;

            }
            catch
            {


            }
        }

        private void frmInserimentoProgramma_Load(object sender, EventArgs e)
        {
            if (_sb == null)
            {
                MessageBox.Show("Apparato Corrente non definito  /N Impossibile continuare","Programmazioe Apparato", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            }
            this.Text = "Nuovo Programma " + Convert.ToString(_sb.sbData.ProgramCount + 1);
        }

        public void salvaProgramma()
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
                _nuovoProg.BatteryType = FunzioniMR.ConvertiByte(txtProgcBattType.Text,1,0);
                _nuovoProg.BatteryCells = FunzioniMR.ConvertiByte(txtProgcCelleTot.Text, 1, 0);
                _nuovoProg.BatteryCell3 = FunzioniMR.ConvertiByte(txtProgcCelleV3.Text, 1, 0);
                _nuovoProg.BatteryCell2 = FunzioniMR.ConvertiByte(txtProgcCelleV2.Text, 1, 0);
                _nuovoProg.BatteryCell1 = FunzioniMR.ConvertiByte(txtProgcCelleV1.Text, 1, 0);

                _sb.ScriviProgramma( _nuovoProg ,(chkMemProgrammed.Checked == true ));

                if (_sb.UltimaRisposta != SerialMessage.EsitoRisposta.MessaggioOk)
                {
                    MessageBox.Show("Programmazione non riuscita", "Programmazioe Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    this.Close();
                }
               
            }
            catch (Exception Ex)
            {
                Log.Error("salvaProgramma: " + Ex.Message);
            }

        }

        private void btnInserisciProgramma_Click(object sender, EventArgs e)
        {
            if (_sb.apparatoPresente)
            {
                salvaProgramma();
                if (chkMemProgrammed.Checked)
                {
                    _sb.AttivaProgramma();
                }
            }
        }
    }
}
