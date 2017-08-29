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
        private bool _modoEsteso;
        private bool _StartDelayed;

        private ushort _minutiDurata = 480;
        private bool _datiCambiati = false;

        private ushort _minutiInizio = 0;
        private ushort _minutiMassimaAttesa = 0;

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

                _turno.flagEqual = true;

            }
            else
            {

                _turno.flagEqual = false;
            }

            DatiSalvati = false;
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
                //mtxInizioEqual.ReadOnly = stato;
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


                if (ModoEsteso)
                {
                    chkEnableEqual.Checked = _turno.flagEqual;
                    _startEqual = _turno.StartEqual;
                    MostraInizioEqual(_startEqual);

                    chkStartDelayed.Checked = _turno.flagStartDelayed;
                    _StartDelayed = _turno.flagStartDelayed;
                    ShowStartDelayed(_StartDelayed);

                    if (_turno.flagStartDelayed)
                    {
                        _minutiDurata = _turno.MinutiDurata;
                        MostraDurata(_turno.MinutiDurata);

                        _minutiInizio = _turno.OrarioStartCarica;
                        MostraOrarioInizio(_turno.OrarioStartCarica);

                        _minutiMassimaAttesa = _turno.MaxMinutiAnticipo;
                        MostraMassimaAttesa(_turno.MaxMinutiAnticipo);
                        chkEnableDeleteDelay.Checked = _turno.flagDeleteDelay;
                    }

                }

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
                /*int _tempOre;
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
                MostraInizioEqual(_startEqual);*/
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

        private void chkStartDelayed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                ShowStartDelayed(chkStartDelayed.Checked);
                _turno.flagStartDelayed = chkStartDelayed.Checked;
                _StartDelayed = _turno.flagStartDelayed;
                DatiSalvati = false;
            }
            catch
            {

            }
        }

        private void ShowStartDelayed(bool enabled)
        {
            try
            {
                mtxInizioCarica.Enabled = enabled;
                mtxAttesaMassima.Enabled = enabled;
                chkEnableDeleteDelay.Enabled = enabled;
                lblAttesaMassima.Enabled = enabled;
                lblInizioCarica.Enabled = enabled;
            }
            catch
            {

            }

        }

        private void chkEnableDeleteDelay_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                _turno.flagDeleteDelay = chkEnableDeleteDelay.Checked;
                DatiSalvati = false;
            }
            catch
            {

            }
        }

        private bool VerificaStartDifferito()
        {
            bool StatoVerifica = false;
            try
            {
                int _tempOre;
                int _tempMin;
                bool _esito;
                string _tempoT;
                string _tempoIns = mtxInizioCarica.Text;
                _tempoIns = _tempoIns.Replace("_", "0");

                _tempoT = _tempoIns.Substring(0, 2);
                _esito = int.TryParse(_tempoT, out _tempOre);
                if (_tempOre > 23) _tempOre = 23;
                if (_tempoIns.Length > 3)
                {
                    _tempoT = _tempoIns.Substring(3, 2);
                    _esito = int.TryParse(_tempoT, out _tempMin);
                }
                else
                {
                    _tempMin = 0;
                }
                if (_tempMin > 59) _tempMin = 59;

                _turno.MinutiStartCarica = (byte)_tempMin;
                _turno.OraStartCarica = (byte)_tempOre;

                StatoVerifica = true;
                return StatoVerifica;
            }
            catch
            {
                return false;
            }
        }

        private bool MostraStartDifferito(ushort MinutiInizio = 0)
        {
            bool _esito = false;
            try
            {
                byte _tempOre;
                byte _tempMinuti;

                _tempOre = (byte)(MinutiInizio / 60);
                _tempMinuti = (byte)(MinutiInizio % 60);
                mtxInizioCarica.Text = _tempOre.ToString("00") + ":" + _tempMinuti.ToString("00");
                _esito = true;

                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        private bool MostraAttesaMassima(ushort MinutiAttesa = 0)
        {

            try
            {
                ushort _tempOre;
                ushort _tempMinuti;

                _tempOre = (ushort)(MinutiAttesa / 60);
                _tempMinuti = (ushort)(MinutiAttesa % 60);

                return MostraAttesaMassima((byte)_tempOre, (byte)_tempMinuti);

            }
            catch
            {
                return false;
            }
        }

        private bool MostraAttesaMassima(byte Ore = 0, byte Minuti = 0)
        {
            bool _esito = false;
            try
            {

                mtxAttesaMassima.Text = Ore.ToString("00") + ":" + Minuti.ToString("00");
                _esito = true;

                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        public bool ModoEsteso
        {
            get
            {
                return _modoEsteso;
            }
            set
            {
                _modoEsteso = value;
                chkStartDelayed.Enabled = value;
                chkEnableEqual.Enabled = value;

            }
        }

        private void mtxInizioCarica_Leave(object sender, EventArgs e)
        {
            VerificaInizioCarica();
        }

        /// <summary>
        /// Verifica che l'orario inizio carica inserito sia un orario valido e lo trasforma in minuti dalle 00:00
        /// </summary>
        /// <returns><c>true</c> se orario valido.</returns>
        private bool VerificaInizioCarica()
        {
            try
            {

                int _tempOre;
                int _tempMin;
                bool _esito;
                string _tempoT;
                string _tempoIns = mtxInizioCarica.Text;
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
                _minutiInizio = (ushort)(_tempOre * 60 + _tempMin);


                if (_turno.MinutiStartCarica != _minutiInizio)
                {
                    _turno.OrarioStartCarica = _minutiInizio;
                    _datiCambiati = true;
                    DatiSalvati = false;
                }
                _turno.OrarioStartCarica = _minutiInizio;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converte l'orario inizio da minuti totali a ORE:MINUTI e lo inserisce nella relativa textbox.
        /// </summary>
        /// <param name="Minuti">Minuti totali dalle 00:00.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool MostraOrarioInizio(ushort Minuti = 0)
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
                    mtxInizioCarica.Text = _tempOre.ToString("00") + ":" + _tempMinuti.ToString("00");
                    _esito = true;
                }
                else
                {
                    mtxInizioCarica.Text = "";
                    _esito = false;
                }
                return _esito;
            }
            catch
            {
                return _esito;
            }
        }

        private void mtxAttesaMassima_Leave(object sender, EventArgs e)
        {
            VerificaMassimaAttesa();
        }

        private bool VerificaMassimaAttesa()
        {
            try
            {

                int _tempOre;
                int _tempMin;
                bool _esito;
                string _tempoT;
                string _tempoIns = mtxAttesaMassima.Text;
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
                _minutiMassimaAttesa = (ushort)(_tempOre * 60 + _tempMin);


                if (_turno.MaxMinutiAnticipo != _minutiMassimaAttesa)
                {
                    _turno.MaxMinutiAnticipo = _minutiMassimaAttesa;
                    _datiCambiati = true;
                    DatiSalvati = false;
                }
                _turno.MaxMinutiAnticipo = _minutiMassimaAttesa;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool MostraMassimaAttesa(ushort Minuti = 0)
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
                    mtxAttesaMassima.Text = _tempOre.ToString("00") + ":" + _tempMinuti.ToString("00");
                    _esito = true;
                }
                else
                {
                    mtxAttesaMassima.Text = "";
                    _esito = false;
                }
                return _esito;
            }
            catch
            {
                return _esito;
            }
        }
    }
}
