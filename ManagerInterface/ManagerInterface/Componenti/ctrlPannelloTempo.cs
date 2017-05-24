using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChargerLogic;

namespace PannelloCharger
{
    public partial class ctrlPannelloTempo : UserControl
    {

        private byte _fattoreCarica = 101;
        private byte _giorno;
        private byte _startEqual;

        private ushort _minutiDurata = 480;
        private bool _datiCambiati = false;
        
        private bool _solaLettura = false;
        private ModelloTurno _turno = new ModelloTurno();

        public event EventHandler<DatiCambiatiEventArgs> DatiCambiati;


        public ctrlPannelloTempo()
        {
            InitializeComponent();
            Selezionato(false);
        }

        private void ctrlPannelloTempo_Load(object sender, EventArgs e)
        {

        }

        private void chkEnableEqual_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableEqual.Checked)
            {
                mtxInizioEqual.Enabled = true;
                lblOraEqual.Enabled = true;
                _turno.flagEqual = (byte)ParametriSetupPro.ModoEqualizzazione.Full;

            }
            else
            {
                mtxInizioEqual.Enabled = false;
                lblOraEqual.Enabled = false;
                _turno.flagEqual = (byte)ParametriSetupPro.ModoEqualizzazione.NO;
            }
        }
        public bool SolaLettura
        {
            get
            {
                return _solaLettura;
            }

            set


            {
                _solaLettura = value;
                applicaSolaLettura(_solaLettura);
            }

        }


        private void applicaSolaLettura(bool stato)
        {
            try
            {
                mtxInizioEqual.ReadOnly = stato;
                mtxDurataCarica.ReadOnly = stato;
                chkEnableEqual.AutoCheck = !stato;
                nudChargeFactor.ReadOnly = stato;

            }
            catch
            {

            }
        }

        private void nudChargeFactor_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (nudChargeFactor.Value >= 0 && nudChargeFactor.Value < 2)
                {
                    byte _nuovoValore = (byte)(nudChargeFactor.Value * 100);
                    if (_fattoreCarica != _nuovoValore)
                    {
                        _fattoreCarica = _nuovoValore;
                        _turno.FattoreCarica = _nuovoValore;
                        DatiSalvati = false;
                        
                        // TODO: raise dati cambiati

                    }

                }


            }
            catch
            {

            }
        }


        public ModelloTurno Turno
        {
            get
            {
                return _turno;
            }
            set
            {
                _turno = value;
                _minutiDurata = _turno.MinutiDurata;
                MostraDurata(_turno.MinutiDurata);
                _fattoreCarica = _turno.FattoreCarica;
                MostraFC(_fattoreCarica);
                _datiCambiati = false;

                chkEnableEqual.Checked = ((ParametriSetupPro.ModoEqualizzazione)_turno.flagEqual != ParametriSetupPro.ModoEqualizzazione.NO);
                _startEqual = _turno.StartEqual;
                MostraInizioEqual(_startEqual);

            }
        }


        public ushort MinutiDurata
        {
            get
            {
                return _minutiDurata;
            }
            set
            {
                ushort _tempOre;
                ushort _tempMinuti;
                _minutiDurata = value;
                MostraDurata(_minutiDurata);
                _turno.MinutiDurata = _minutiDurata;
                _datiCambiati = true;
            }
        }


        public byte Giorno
        {
            get
            {
                return _giorno;
            }
            set
            {
                _giorno = value;
                _datiCambiati = true;
            }
        }

        private bool MostraDurata(ushort Minuti = 0)
        {
            bool _esito = false;
            try
            {
                ushort _tempOre;
                ushort _tempMinuti;

                if (Minuti != 0)
                {
                    _tempOre = (ushort)(Minuti / 60);
                    _tempMinuti = (ushort)(Minuti % 60);
                    mtxDurataCarica.Text = _tempOre.ToString("00") + ":" + _tempMinuti.ToString("00");
                    _esito = true;
                }
                else
                {
                    mtxDurataCarica.Text = "";
                    _esito = false;
                }
                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        private bool MostraInizioEqual(byte OreMinuti = 0)
        {
            bool _esito = false;
            try
            {
                OraTurnoMR _tempOra = new OraTurnoMR(OreMinuti);

                ushort _tempOre;
                ushort _tempMinuti;
                string pippo = _tempOra.ToString();
                mtxInizioEqual.Text = pippo;  //"00:00";// _tempOra.ToString();

                _esito = true;
                
                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        private bool MostraFC(byte fc = 0)
        {
            bool _esito = false;
            try
            {

                if (fc != 0)
                {

                    nudChargeFactor.Value = (decimal)fc / 100;

                    _esito = true;
                }
                else
                {
                    nudChargeFactor.Value = 1;
                    _esito = false;
                }
                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        private void mtxDurataCarica_Leave(object sender, EventArgs e)
        {
            VerificaDurataFase();
        }


        private bool VerificaDurataFase()
        {
            bool StatoTurno = false;
            try
            {

                int _tempOre;
                int _tempMin;
                bool _esito;
                string _tempoT;
                string _tempoIns = mtxDurataCarica.Text;
                _tempoIns = _tempoIns.Replace("_", "0");

                _tempoT = _tempoIns.Substring(0, 2);
                _esito = int.TryParse(_tempoT, out _tempOre);
                if (_tempoIns.Length > 3)
                {
                    _tempoT = _tempoIns.Substring(3, 2);
                    _esito = int.TryParse(_tempoT, out _tempMin);
                }
                else
                {
                    _tempMin = 0;
                }
                _minutiDurata = (ushort)(_tempOre * 60 + _tempMin);

                if (_minutiDurata >= 360)
                {
                    StatoTurno = true;
                }
                else
                {
                    StatoTurno = false;
                }
                if (_turno.MinutiDurata != _minutiDurata)
                {
                    _turno.MinutiDurata = _minutiDurata;
                    _datiCambiati = true;
                    DatiSalvati = false;
                }
                _turno.MinutiDurata = _minutiDurata;

                return StatoTurno;
            }
            catch
            {
                return true;
            }
        }

        private bool IsValidTime(string Value)
        {

            try
            {
                string tempVal = Value.Trim();
                if (tempVal == ":")
                {
                    return false;
                }

                return true;

            }
            catch
            {
                return false;
            }
        }


        public void Selezionato(bool Attivo)
        {
            try
            {
                if (Attivo)
                    this.BackColor = Color.Yellow;
                else
                    this.BackColor = Color.LightYellow;

            }
            catch
            {

            }
        }




        private void mtxInizioEqual_Leave(object sender, EventArgs e)
        {
            VerificaInizioEqual();
        }


        private bool VerificaInizioEqual()
        {
            bool StatoVerifica = false;
            try
            {
                OraTurnoMR _tempOra;
                int _tempOre;
                int _tempMin;
                bool _esito;
                string _tempoT;
                string _tempoIns = mtxInizioEqual.Text;
                _tempoIns = _tempoIns.Replace("_", "0");

                _tempoT = _tempoIns.Substring(0, 2);
                _esito = int.TryParse(_tempoT, out _tempOre);
                if (_tempoIns.Length > 3)
                {
                    _tempoT = _tempoIns.Substring(3, 2);
                    _esito = int.TryParse(_tempoT, out _tempMin);
                }
                else
                {
                    _tempMin = 0;
                }
                _tempOra = new OraTurnoMR(_tempOre, _tempMin);
                // mtxInizioEqual.Text = _tempOra.ToString();

                _turno.StartEqual = _tempOra.OreMinuti;
                _startEqual = _turno.StartEqual;
                MostraInizioEqual(_startEqual);
                StatoVerifica = true;
                return StatoVerifica;
            }
            catch
            {
                return true;
            }
        }


        /// <summary>
        /// Ritorna o imposta (protected) lo stato di dati salvati.
        /// </summary>
        /// <value>
        ///   <c>true</c> se tutti i dati sono salvati; otherwise, <c>false</c>.
        /// </value>
        public bool DatiSalvati
        {
            get
            {
                return !_datiCambiati;
            }
            protected set
            {
                _datiCambiati = !value;
                if (DatiCambiati != null)
                {
                    DatiCambiati(this, new DatiCambiatiEventArgs(_datiCambiati));
                }

            }

        }



        private void ctrlPannelloTempo_Leave(object sender, EventArgs e)
        {
            Selezionato(false);
        }

        private void ctrlPannelloTempo_Enter(object sender, EventArgs e)
        {
            Selezionato(true);
        }
    }
}
