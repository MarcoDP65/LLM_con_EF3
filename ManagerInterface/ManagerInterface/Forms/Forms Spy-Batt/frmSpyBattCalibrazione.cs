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
using static ChargerLogic.elementiComuni;
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

        // variabili per gestione calibrazione
        private List<_sbTestataCalibrazione> _ListaCalibrazioni = new List<_sbTestataCalibrazione>();
        private List<sbTestataCalibrazione> ListaCalibrazioni = new List<sbTestataCalibrazione>();
        private List<TipoGrafico> LstTipiGrafico = new List<TipoGrafico>();
        public bool CaricaTestateCalibrazioni(string IdApparato, MoriData._db dbCorrente)
        {
            try
            {
                _ListaCalibrazioni.Clear();

                IEnumerable<_sbTestataCalibrazione> _TempCicli = dbCorrente.Query<_sbTestataCalibrazione>("select * from _sbTestataCalibrazione where IdApparato = ? order by IdEsecuzione asc", IdApparato);

                _ListaCalibrazioni.AddRange(_TempCicli);

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return false;
            }
        }

        public void CaricaComboCalibrazioni()
        {
            try
            {
                bool _esito;

                _esito = CaricaTestateCalibrazioni(_sb.Id, _logiche.dbDati.connessione);

                if (_esito)
                {
                    cmbCalListaEsecuzioni.DataSource = _ListaCalibrazioni;
                    cmbCalListaEsecuzioni.DisplayMember = "IdEsecuzione";
                    cmbCalListaEsecuzioni.ValueMember = "IdLocale";
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.CaricaComboCalibrazioni: " + Ex.Message);
            }
        }

        private void CaricaComboGrafici()
        {
            try
            {
                LstTipiGrafico.Clear();

                LstTipiGrafico.Add(new TipoGrafico { Progressivo = 1, Descrizione = "Errore Assoluto",    Codice = 0x01 });
                LstTipiGrafico.Add(new TipoGrafico { Progressivo = 2, Descrizione = "Errore Relativo",    Codice = 0x02 });
                LstTipiGrafico.Add(new TipoGrafico { Progressivo = 3, Descrizione = "Errore Percentuale", Codice = 0x03 });
                LstTipiGrafico.Add(new TipoGrafico { Progressivo = 4, Descrizione = "Valore Rilevato",    Codice = 0x04 });


                cmbCalTipoGrafico.DataSource = LstTipiGrafico;
                cmbCalTipoGrafico.DisplayMember = "Descrizione";
                cmbCalTipoGrafico.ValueMember = "Codice";

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.CaricaComboGrafici: " + Ex.Message);
            }

        }


        private void LanciaSequenza()
        {
            try
            {


                int _ciclo;
                int _start;
                int _stop;
                int _passo;
                int _spire = 1;
                int _stepCount = 0;
                int _millisecondi = 1000;
                bool _esito;

                int.TryParse(txtCalImin.Text, out _start);
                int.TryParse(txtCalImax.Text, out _stop);
                int.TryParse(txtCalIstep.Text, out _passo);
                int.TryParse(txtCalDelay.Text, out _millisecondi);

                int.TryParse(txtCalSpire.Text, out _spire);


                txtCalCurr.Text = "";
                txtCalCurrStep.Text = "";

                txtCalErrore.Text = "";
                txtCalSb.Text = "";

                if (Lambda == null)
                {
                    return;
                }

                // Preparo la lista valori
                ValoriTestCorrente.Clear();
                flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                flvwLettureCorrente.BuildList();
                sbAnalisiCorrente _vac;

                if (chkCalAccendiAlim.Checked)
                {
                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.Alimentatatore.ImpostaStato(true);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }

                flvwLettureCorrente.Refresh();
                Application.DoEvents();

                for (_ciclo = _start; _ciclo <= _stop; _ciclo += _passo)
                {
                    _vac = new sbAnalisiCorrente();

                    _vac.Lettura = (uint)_stepCount;
                    txtCalCurr.Text = _ciclo.ToString();
                    txtCalCurrStep.Text = _stepCount.ToString();
                    Lambda.Alimentatatore.ImpostaCorrente(_ciclo);
                    Lambda.MostraCorrenti();

                    System.Threading.Thread.Sleep(_millisecondi);

                    _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                    Lambda.MostraCorrenti();
                    txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                    if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                    else txtCalSb.ForeColor = Color.Black;

                    txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                    float _corrBase = Lambda.Alimentatatore.Arilevati;

                    _vac.Spire = (uint)_spire;
                    _vac.Ateorici = Lambda.Alimentatatore.Aimpostati;
                    _vac.Areali = Lambda.Alimentatatore.Arilevati;
                    _vac.Aspybatt = (float)(_sb.sbVariabili.CorrenteBatteria / 10);






                    txtCalErrore.Text = "";
                    if (_corrBase != 0)
                    {
                        float _errore = ((float)(_sb.sbVariabili.CorrenteBatteria / 10) - (_corrBase * _spire)) / (_corrBase * _spire);
                        txtCalErrore.Text = _errore.ToString("p2");
                    }

                    _stepCount++;

                    ValoriTestCorrente.Add(_vac);
                    flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                    //flvwLettureCorrente.Refresh();
                    //flvwLettureCorrente.BuildList();
                    Application.DoEvents();

                    System.Threading.Thread.Sleep(500);

                }

                flvwLettureCorrente.Refresh();
                txtCalCurr.Text = _ciclo.ToString();
                txtCalCurrStep.Text = _stepCount.ToString();

                if (chkCalAccendiAlim.Checked)
                {
                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }
                flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                flvwLettureCorrente.BuildList();
                GraficoCorrentiOxy();
            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.LanciaSequenza: " + Ex.Message);
            }
        }

        private void LanciaSequenzaEstesa()
        {


            try
            {

                DialogResult _risposta;
                int _ciclo;
                int _start;
                int _stop;
                int _passo;
                int _spire = 1;
                int _stepCount = 0;
                int _millisecondi = 1000;
                bool _esito;
                bool _apparatoPronto = false;

                float _maxErrPos = 0;
                float _maxErrNeg = 0;

                int.TryParse(txtCalImin.Text, out _start);
                int.TryParse(txtCalImax.Text, out _stop);
                int.TryParse(txtCalIstep.Text, out _passo);
                int.TryParse(txtCalDelay.Text, out _millisecondi);

                int.TryParse(txtCalSpire.Text, out _spire);
                int _passoTest = 0;
                sbAnalisiCorrente _vac;

                txtCalCurr.Text = "";
                txtCalCurrStep.Text = "";

                txtCalErrore.Text = "";
                txtCalSb.Text = "";

                if (Lambda == null)
                {
                    return;
                }



                // Prima Preparo la lista valori
                ValoriTestCorrente.Clear();
                flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                flvwLettureCorrente.BuildList();


                flvwLettureCorrente.Refresh();
                Application.DoEvents();

                for (_ciclo = _start; _ciclo <= _stop; _ciclo += _passo)
                {
                    _vac = new sbAnalisiCorrente();
                    _vac.Lettura = (uint)_stepCount++;
                    _vac.Spire = (uint)_spire;
                    _vac.Ateorici = _ciclo;
                    _vac.Areali = _ciclo;
                    _vac.AspybattAN = 0;
                    _vac.AspybattAP = 0;
                    _vac.AspybattDN = 0;
                    _vac.AspybattDP = 0;
                    ValoriTestCorrente.Add(_vac);
                }


                flvwLettureCorrente.Refresh();
                Application.DoEvents();

                // Ora comincio le corse:
                // Ascendente

                _risposta = MessageBox.Show("Collegare lo SPY-BATT per letture nel verso diretto\n(Nucleo verso il positivo)", "Verifica Polarità", MessageBoxButtons.OKCancel);
                if (_risposta == DialogResult.OK)
                {


                    // se previsto, attivo la linea
                    if (chkCalAccendiAlim.Checked)
                    {
                        // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                        Lambda.Alimentatatore.ImpostaStato(false);
                        Lambda.Alimentatatore.ImpostaCorrente(0);
                        Lambda.Alimentatatore.ImpostaStato(true);
                        Lambda.MostraCorrenti();
                        Lambda.MostraStato();
                    }
                    flvwLettureCorrente.Refresh();
                    GraficoCorrentiComplOxy();
                    Application.DoEvents();
                    _apparatoPronto = false;
                    //poi verifico che lo spybatt sia montato correttamente mandando una corrente di prova
                    do
                    {

                        Lambda.Alimentatatore.ImpostaCorrente(10);
                        Lambda.MostraCorrenti();
                        System.Threading.Thread.Sleep(_millisecondi);
                        _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                        Lambda.MostraCorrenti();

                        if (_sb.sbVariabili.CorrenteBatteria <= 0)
                        {
                            Lambda.Alimentatatore.ImpostaCorrente(0);
                            Lambda.MostraCorrenti();

                            _risposta = MessageBox.Show("SPY-BATT non collegato correttamente\nCollegare lo SPY-BATT per letture nel verso diretto\n(Nucleo verso il positivo)", "Verifica Polarità", MessageBoxButtons.RetryCancel);
                            if (_risposta == DialogResult.Cancel)
                            {
                                Lambda.Alimentatatore.ImpostaCorrente(0);
                                Lambda.MostraCorrenti();
                                if (chkCalAccendiAlim.Checked)
                                {
                                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                    Lambda.Alimentatatore.ImpostaStato(false);
                                    Lambda.MostraStato();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            _apparatoPronto = true;

                            if (chkCalAccendiAlim.Checked & !Lambda.Alimentatatore.UscitaAttiva)
                            {
                                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                Lambda.Alimentatatore.ImpostaStato(true);
                                Lambda.MostraStato();
                                return;
                            }
                        }
                    }
                    while (!_apparatoPronto);

                    // ordine ascendente
                    ValoriTestCorrente.OrderBy(par => par.Lettura);
                    _passoTest = 0;
                    foreach (sbAnalisiCorrente _test in ValoriTestCorrente)
                    {
                        //txtCalCurr.Text = _ciclo.ToString();
                        //txtCalCurrStep.Text = _stepCount.ToString();
                        Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                        Lambda.MostraCorrenti();

                        System.Threading.Thread.Sleep(_millisecondi);
                        _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                        Lambda.MostraCorrenti();
                        txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                        if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                        else txtCalSb.ForeColor = Color.Black;

                        txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                        float _corrBase = Lambda.Alimentatatore.Arilevati;


                        _test.AspybattAP = (float)(_sb.sbVariabili.CorrenteBatteria / 10);

                        txtCalErrore.Text = "";
                        if (_corrBase != 0)
                        {
                            float _errore = Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10) - (_corrBase * _spire)) / (_corrBase * _spire);
                            txtCalErrore.Text = _errore.ToString("p2");
                            if (_corrBase > 10)
                            {
                                if (_errore > _maxErrPos) _maxErrPos = _errore;
                                txtCalErroreMaxPos.Text = _maxErrPos.ToString("p2");
                            }

                        }

                        _stepCount++;


                        flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                        //flvwLettureCorrente.Refresh();
                        //flvwLettureCorrente.BuildList();
                        Application.DoEvents();

                        System.Threading.Thread.Sleep(200);

                    }

                    flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                    GraficoCorrentiComplOxy();
                    Application.DoEvents();

                    // ordine discendente

                    foreach (sbAnalisiCorrente _test in ValoriTestCorrente.OrderByDescending(par => par.Lettura))
                    {
                        Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                        Lambda.MostraCorrenti();

                        System.Threading.Thread.Sleep(_millisecondi);
                        _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                        Lambda.MostraCorrenti();
                        txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                        if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                        else txtCalSb.ForeColor = Color.Black;

                        txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                        float _corrBase = Lambda.Alimentatatore.Arilevati;


                        _test.AspybattDP = (float)(_sb.sbVariabili.CorrenteBatteria / 10);

                        txtCalErrore.Text = "";
                        if (_corrBase != 0)
                        {
                            float _errore = Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10) - (_corrBase * _spire)) / (_corrBase * _spire);
                            txtCalErrore.Text = _errore.ToString("p2");
                            if (_corrBase > 10)
                            {
                                if (_errore > _maxErrPos) _maxErrPos = _errore;
                                txtCalErroreMaxPos.Text = _maxErrPos.ToString("p2");
                            }
                        }

                        _stepCount++;


                        flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                        //flvwLettureCorrente.Refresh();
                        //flvwLettureCorrente.BuildList();
                        Application.DoEvents();

                        System.Threading.Thread.Sleep(200);

                    }
                    flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                    GraficoCorrentiComplOxy();
                    Application.DoEvents();

                }

                flvwLettureCorrente.BuildList();


                flvwLettureCorrente.Refresh();
                Application.DoEvents();

                if (chkCalAccendiAlim.Checked)
                {
                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }
                // Ora faccio invertire il collegamento poi rifaccio il test

                _risposta = MessageBox.Show("Collegare lo SPY-BATT per nel verso inverso\n(Nucleo verso il negativo)", "Verifica Polarità", MessageBoxButtons.OKCancel);
                if (_risposta == DialogResult.OK)

                    _apparatoPronto = false;
                //poi verifico che lo spybatt sia montato correttamente mandando una corrente di prova
                do
                {
                    if (chkCalAccendiAlim.Checked)
                    {
                        // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                        Lambda.Alimentatatore.ImpostaStato(true);
                        Lambda.MostraCorrenti();
                        Lambda.MostraStato();
                    }
                    Lambda.Alimentatatore.ImpostaCorrente(10);
                    Lambda.MostraCorrenti();
                    System.Threading.Thread.Sleep(_millisecondi);
                    _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                    Lambda.MostraCorrenti();

                    if (_sb.sbVariabili.CorrenteBatteria >= 0)
                    {
                        Lambda.Alimentatatore.ImpostaCorrente(0);
                        Lambda.MostraCorrenti();
                        if (chkCalAccendiAlim.Checked)
                        {
                            // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                            Lambda.Alimentatatore.ImpostaStato(false);
                            Lambda.MostraStato();
                            return;
                        }

                        _risposta = MessageBox.Show("SPY-BATT non collegato correttamente\nCollegare lo SPY-BATT per letture nel verso diretto\n(Nucleo verso il positivo)", "Verifica Polarità", MessageBoxButtons.RetryCancel);
                        if (_risposta == DialogResult.Cancel)
                        {
                            Lambda.Alimentatatore.ImpostaCorrente(0);
                            Lambda.MostraCorrenti();
                            if (chkCalAccendiAlim.Checked)
                            {
                                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                Lambda.Alimentatatore.ImpostaStato(false);
                                Lambda.MostraStato();
                                return;
                            }
                        }
                    }
                    else
                    {
                        _apparatoPronto = true;
                        if (chkCalAccendiAlim.Checked & !Lambda.Alimentatatore.UscitaAttiva)
                        {
                            // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                            Lambda.Alimentatatore.ImpostaStato(true);
                            Lambda.MostraStato();
                            return;
                        }
                    }
                }
                while (!_apparatoPronto);

                {
                    // ordine ascendente  
                    foreach (sbAnalisiCorrente _test in ValoriTestCorrente.OrderBy(par => par.Lettura))
                    {

                        {

                            //txtCalCurr.Text = _ciclo.ToString();
                            //txtCalCurrStep.Text = _stepCount.ToString();
                            Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                            Lambda.MostraCorrenti();

                            System.Threading.Thread.Sleep(_millisecondi);
                            _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                            Lambda.MostraCorrenti();
                            txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                            if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                            else txtCalSb.ForeColor = Color.Black;

                            txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                            float _corrBase = Lambda.Alimentatatore.Arilevati;


                            _test.AspybattAN = (float)(_sb.sbVariabili.CorrenteBatteria / 10);

                            txtCalErrore.Text = "";
                            if (_corrBase != 0)
                            {
                                float _errore = Math.Abs((float)(-_sb.sbVariabili.CorrenteBatteria / 10) - (_corrBase * _spire)) / (_corrBase * _spire);
                                txtCalErrore.Text = _errore.ToString("p2");
                                if (_corrBase > 10)
                                {
                                    if (_errore > _maxErrNeg) _maxErrNeg = _errore;
                                    txtCalErroreMaxNeg.Text = _maxErrNeg.ToString("p2");
                                }
                            }

                            _stepCount++;


                            flvwLettureCorrente.SetObjects(ValoriTestCorrente);

                            //flvwLettureCorrente.Refresh();
                            //flvwLettureCorrente.BuildList();
                            Application.DoEvents();

                            System.Threading.Thread.Sleep(100);

                        }
                        GraficoCorrentiComplOxy();
                        Application.DoEvents();
                    }

                    flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                    GraficoCorrentiComplOxy();
                    Application.DoEvents();

                    // ordine discendente
                    foreach (sbAnalisiCorrente _test in ValoriTestCorrente.OrderByDescending(par => par.Lettura))
                    {

                        {

                            //txtCalCurr.Text = _ciclo.ToString();
                            //txtCalCurrStep.Text = _stepCount.ToString();
                            Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                            Lambda.MostraCorrenti();

                            System.Threading.Thread.Sleep(_millisecondi);
                            _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                            Lambda.MostraCorrenti();
                            txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                            if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                            else txtCalSb.ForeColor = Color.Black;

                            txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                            float _corrBase = Lambda.Alimentatatore.Arilevati;


                            _test.AspybattDN = (float)(_sb.sbVariabili.CorrenteBatteria / 10);

                            txtCalErrore.Text = "";
                            if (_corrBase != 0)
                            {
                                float _errore = Math.Abs((float)(-_sb.sbVariabili.CorrenteBatteria / 10) - (_corrBase * _spire)) / (_corrBase * _spire);
                                txtCalErrore.Text = _errore.ToString("p2");
                                if (_corrBase > 10)
                                {
                                    if (_errore > _maxErrNeg) _maxErrNeg = _errore;
                                    txtCalErroreMaxNeg.Text = _maxErrNeg.ToString("p2");
                                }
                            }

                            _stepCount++;


                            flvwLettureCorrente.SetObjects(ValoriTestCorrente);

                            //flvwLettureCorrente.Refresh();
                            //flvwLettureCorrente.BuildList();
                            Application.DoEvents();

                            System.Threading.Thread.Sleep(100);

                        }
                        GraficoCorrentiComplOxy();
                        Application.DoEvents();
                    }

                    flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                    GraficoCorrentiComplOxy();
                    Application.DoEvents();
                }


                if (chkCalAccendiAlim.Checked)
                {
                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }


                flvwLettureCorrente.Refresh();
                txtCalCurr.Text = _ciclo.ToString();
                txtCalCurrStep.Text = _stepCount.ToString();

                if (chkCalAccendiAlim.Checked)
                {
                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }
                flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                flvwLettureCorrente.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.LanciaSequenza: " + Ex.Message);
            }
        }

        private void LanciaSequenzaAssoluta()
        {


            try
            {

                DialogResult _risposta;
                int _ciclo;
                int _start;
                int _stop;
                int _passo;
                int _spire = 1;
                int _stepCount = 0;
                int _millisecondi = 1000;
                bool _esito;
                bool _apparatoPronto = false;

                float _maxErrPos = 0;
                float _maxErrNeg = 0;

                int.TryParse(txtCalImin.Text, out _start);
                int.TryParse(txtCalImax.Text, out _stop);
                int.TryParse(txtCalIstep.Text, out _passo);
                int.TryParse(txtCalDelay.Text, out _millisecondi);

                int.TryParse(txtCalSpire.Text, out _spire);
                int _passoTest = 0;
                sbAnalisiCorrente _vac;

                txtCalCurr.Text = "";
                txtCalCurrStep.Text = "";

                txtCalErrore.Text = "";
                txtCalSb.Text = "";

                if (Lambda == null)
                {
                    return;
                }



                // Prima Preparo la lista valori
                ValoriTestCorrente.Clear();
                flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                flvwLettureCorrente.BuildList();


                flvwLettureCorrente.Refresh();
                Application.DoEvents();

                for (_ciclo = _start; _ciclo <= _stop; _ciclo += _passo)
                {
                    _vac = new sbAnalisiCorrente();
                    _vac.Lettura = (uint)_stepCount++;
                    _vac.Spire = (uint)_spire;
                    _vac.Ateorici = _ciclo;
                    _vac.Areali = _ciclo;
                    _vac.AspybattAN = 0;
                    _vac.AspybattAP = 0;
                    _vac.AspybattDN = 0;
                    _vac.AspybattDP = 0;
                    ValoriTestCorrente.Add(_vac);
                }


                flvwLettureCorrente.Refresh();
                GraficoCorrentiComplOxy();
                Application.DoEvents();

                // Ora comincio le corse:
                // Ascendente

                _risposta = MessageBox.Show("Collegare lo SPY-BATT per letture nel verso diretto", "Verifica Polarità", MessageBoxButtons.OKCancel);
                if (_risposta == DialogResult.OK)
                {


                    // se previsto, attivo la linea
                    if (chkCalAccendiAlim.Checked)
                    {
                        // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                        Lambda.Alimentatatore.ImpostaStato(false);
                        Lambda.Alimentatatore.ImpostaCorrente(0);
                        Lambda.Alimentatatore.ImpostaStato(true);
                        Lambda.MostraCorrenti();
                        Lambda.MostraStato();
                    }
                    flvwLettureCorrente.Refresh();
                    Application.DoEvents();
                    _apparatoPronto = false;
                    //poi verifico che lo spybatt sia montato correttamente mandando una corrente di prova
                    do
                    {

                        Lambda.Alimentatatore.ImpostaCorrente(10);
                        Lambda.MostraCorrenti();
                        System.Threading.Thread.Sleep(_millisecondi);
                        _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                        Lambda.MostraCorrenti();

                        if (_sb.sbVariabili.CorrenteBatteria == 0)
                        {
                            Lambda.Alimentatatore.ImpostaCorrente(0);
                            Lambda.MostraCorrenti();

                            _risposta = MessageBox.Show("SPY-BATT non collegato correttamente", "Verifica Collegamenti", MessageBoxButtons.RetryCancel);
                            if (_risposta == DialogResult.Cancel)
                            {
                                Lambda.Alimentatatore.ImpostaCorrente(0);
                                Lambda.MostraCorrenti();
                                if (chkCalAccendiAlim.Checked)
                                {
                                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                    Lambda.Alimentatatore.ImpostaStato(false);
                                    Lambda.MostraStato();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            _apparatoPronto = true;

                            if (chkCalAccendiAlim.Checked & !Lambda.Alimentatatore.UscitaAttiva)
                            {
                                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                Lambda.Alimentatatore.ImpostaStato(true);
                                Lambda.MostraStato();
                                return;
                            }
                        }
                    }
                    while (!_apparatoPronto);

                    // ordine ascendente
                    ValoriTestCorrente.OrderBy(par => par.Lettura);
                    _passoTest = 0;
                    foreach (sbAnalisiCorrente _test in ValoriTestCorrente)
                    {
                        //txtCalCurr.Text = _ciclo.ToString();
                        //txtCalCurrStep.Text = _stepCount.ToString();
                        Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                        Lambda.MostraCorrenti();

                        System.Threading.Thread.Sleep(_millisecondi);
                        _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                        Lambda.MostraCorrenti();
                        txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                        if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                        else txtCalSb.ForeColor = Color.Black;

                        txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                        float _corrBase = Lambda.Alimentatatore.Arilevati;


                        _test.AspybattAP = Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10));

                        txtCalErrore.Text = "";
                        if (_corrBase != 0)
                        {
                            float _errore = Math.Abs(Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10)) - (_corrBase * _spire)) / (_corrBase * _spire);
                            txtCalErrore.Text = _errore.ToString("p2");
                            if (_corrBase > 10)
                            {
                                if (_errore > _maxErrPos) _maxErrPos = _errore;
                                txtCalErroreMaxPos.Text = _maxErrPos.ToString("p2");
                            }

                        }

                        _stepCount++;


                        flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                        //flvwLettureCorrente.Refresh();
                        //flvwLettureCorrente.BuildList();
                        Application.DoEvents();

                        System.Threading.Thread.Sleep(200);

                    }

                    flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                    GraficoCorrentiComplOxy();
                    Application.DoEvents();

                    // ordine discendente
                    if (chkCalRitornoVeloce.Checked)
                    {
                        // Torno a 0 in 4 passi veloci
                        Lambda.MostraCorrenti();
                        int _stepDiscesa = (int)Lambda.Alimentatatore.Arilevati / 4;
                        for (int _passoDisc = (int)Lambda.Alimentatatore.Arilevati; _passoDisc > 0; _passoDisc -= _stepDiscesa)
                        {
                            if (_passoDisc < 0)
                                _passoDisc = 0;
                            Lambda.Alimentatatore.ImpostaCorrente(_passoDisc);
                            Lambda.MostraCorrenti();
                            System.Threading.Thread.Sleep(500);
                            txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                            Application.DoEvents();
                        }


                    }
                    else
                    {
                        foreach (sbAnalisiCorrente _test in ValoriTestCorrente.OrderByDescending(par => par.Lettura))
                        {
                            Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                            Lambda.MostraCorrenti();

                            System.Threading.Thread.Sleep(_millisecondi);
                            _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                            Lambda.MostraCorrenti();
                            txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                            if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                            else txtCalSb.ForeColor = Color.Black;

                            txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                            float _corrBase = Lambda.Alimentatatore.Arilevati;


                            _test.AspybattDP = Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10));

                            txtCalErrore.Text = "";
                            if (_corrBase != 0)
                            {
                                float _errore = Math.Abs(Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10)) - (_corrBase * _spire)) / (_corrBase * _spire);
                                txtCalErrore.Text = _errore.ToString("p2");
                                if (_corrBase > 10)
                                {
                                    if (_errore > _maxErrPos) _maxErrPos = _errore;
                                    txtCalErroreMaxPos.Text = _maxErrPos.ToString("p2");
                                }
                            }

                            _stepCount++;


                            flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                            //flvwLettureCorrente.Refresh();
                            //flvwLettureCorrente.BuildList();
                            Application.DoEvents();

                            System.Threading.Thread.Sleep(200);

                        }
                    }
                    flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                    GraficoCorrentiComplOxy();
                    Application.DoEvents();

                }

                flvwLettureCorrente.BuildList();


                flvwLettureCorrente.Refresh();
                Application.DoEvents();

                if (chkCalAccendiAlim.Checked)
                {
                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }
                // Ora faccio invertire il collegamento poi rifaccio il test
                if (!chkCalSoloAndata.Checked)
                {
                    _risposta = MessageBox.Show("Collegare il nucleo SPY-BATT nel verso inverso", "Verifica collegamenti", MessageBoxButtons.OKCancel);
                    if (_risposta == DialogResult.OK)

                        _apparatoPronto = false;
                    //poi verifico che lo spybatt sia montato correttamente mandando una corrente di prova
                    do
                    {
                        if (chkCalAccendiAlim.Checked)
                        {
                            // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                            Lambda.Alimentatatore.ImpostaStato(true);
                            Lambda.MostraCorrenti();
                            Lambda.MostraStato();
                        }
                        Lambda.Alimentatatore.ImpostaCorrente(10);
                        Lambda.MostraCorrenti();
                        System.Threading.Thread.Sleep(_millisecondi);
                        _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                        Lambda.MostraCorrenti();

                        if (_sb.sbVariabili.CorrenteBatteria == 0)
                        {
                            Lambda.Alimentatatore.ImpostaCorrente(0);
                            Lambda.MostraCorrenti();
                            if (chkCalAccendiAlim.Checked)
                            {
                                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                Lambda.Alimentatatore.ImpostaStato(false);
                                Lambda.MostraStato();
                                return;
                            }

                            _risposta = MessageBox.Show("SPY-BATT non collegato correttamente", "Verifica ollegamenti", MessageBoxButtons.RetryCancel);
                            if (_risposta == DialogResult.Cancel)
                            {
                                Lambda.Alimentatatore.ImpostaCorrente(0);
                                Lambda.MostraCorrenti();
                                if (chkCalAccendiAlim.Checked)
                                {
                                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                    Lambda.Alimentatatore.ImpostaStato(false);
                                    Lambda.MostraStato();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            _apparatoPronto = true;
                            if (chkCalAccendiAlim.Checked & !Lambda.Alimentatatore.UscitaAttiva)
                            {
                                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                Lambda.Alimentatatore.ImpostaStato(true);
                                Lambda.MostraStato();
                                return;
                            }
                        }
                    }
                    while (!_apparatoPronto);

                    {
                        // ordine ascendente  
                        foreach (sbAnalisiCorrente _test in ValoriTestCorrente.OrderBy(par => par.Lettura))
                        {

                            {

                                //txtCalCurr.Text = _ciclo.ToString();
                                //txtCalCurrStep.Text = _stepCount.ToString();
                                Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                                Lambda.MostraCorrenti();

                                System.Threading.Thread.Sleep(_millisecondi);
                                _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                                Lambda.MostraCorrenti();
                                txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                                if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                                else txtCalSb.ForeColor = Color.Black;

                                txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                                float _corrBase = Lambda.Alimentatatore.Arilevati;


                                _test.AspybattAN = Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10));

                                txtCalErrore.Text = "";
                                if (_corrBase != 0)
                                {
                                    float _errore = Math.Abs(Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10)) - (_corrBase * _spire)) / (_corrBase * _spire);
                                    txtCalErrore.Text = _errore.ToString("p2");
                                    if (_corrBase > 10)
                                    {
                                        if (_errore > _maxErrNeg) _maxErrNeg = _errore;
                                        txtCalErroreMaxNeg.Text = _maxErrNeg.ToString("p2");
                                    }
                                }

                                _stepCount++;


                                flvwLettureCorrente.SetObjects(ValoriTestCorrente);

                                //flvwLettureCorrente.Refresh();
                                //flvwLettureCorrente.BuildList();
                                Application.DoEvents();

                                System.Threading.Thread.Sleep(100);

                            }
                            GraficoCorrentiComplOxy();
                            Application.DoEvents();
                        }

                        flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                        GraficoCorrentiComplOxy();
                        Application.DoEvents();

                        // ordine discendente
                        if (chkCalRitornoVeloce.Checked)
                        {
                            // Torno a 0 in 4 passi veloci
                            Lambda.MostraCorrenti();
                            int _stepDiscesa = (int)Lambda.Alimentatatore.Arilevati / 4;
                            for (int _passoDisc = (int)Lambda.Alimentatatore.Arilevati; _passoDisc > 0; _passoDisc -= _stepDiscesa)
                            {
                                if (_passoDisc < 0)
                                    _passoDisc = 0;
                                Lambda.Alimentatatore.ImpostaCorrente(_passoDisc);
                                Lambda.MostraCorrenti();
                                System.Threading.Thread.Sleep(500);
                                txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                                Application.DoEvents();
                            }


                        }
                        else
                        {
                            foreach (sbAnalisiCorrente _test in ValoriTestCorrente.OrderByDescending(par => par.Lettura))
                            {

                                {

                                    //txtCalCurr.Text = _ciclo.ToString();
                                    //txtCalCurrStep.Text = _stepCount.ToString();
                                    Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                                    Lambda.MostraCorrenti();

                                    System.Threading.Thread.Sleep(_millisecondi);
                                    _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                                    Lambda.MostraCorrenti();
                                    txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                                    if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                                    else txtCalSb.ForeColor = Color.Black;

                                    txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                                    float _corrBase = Lambda.Alimentatatore.Arilevati;


                                    _test.AspybattDN = Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10));

                                    txtCalErrore.Text = "";
                                    if (_corrBase != 0)
                                    {
                                        float _errore = Math.Abs(Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10)) - (_corrBase * _spire)) / (_corrBase * _spire);
                                        txtCalErrore.Text = _errore.ToString("p2");
                                        if (_corrBase > 10)
                                        {
                                            if (_errore > _maxErrNeg) _maxErrNeg = _errore;
                                            txtCalErroreMaxNeg.Text = _maxErrNeg.ToString("p2");
                                        }
                                    }

                                    _stepCount++;


                                    flvwLettureCorrente.SetObjects(ValoriTestCorrente);

                                    //flvwLettureCorrente.Refresh();
                                    //flvwLettureCorrente.BuildList();
                                    Application.DoEvents();

                                    System.Threading.Thread.Sleep(100);

                                }
                                GraficoCorrentiComplOxy();
                                Application.DoEvents();
                            }
                        }

                        flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                        GraficoCorrentiComplOxy();
                        Application.DoEvents();
                    }
                }

                if (chkCalAccendiAlim.Checked)
                {
                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }


                flvwLettureCorrente.Refresh();
                txtCalCurr.Text = _ciclo.ToString();
                txtCalCurrStep.Text = _stepCount.ToString();

                if (chkCalAccendiAlim.Checked)
                {
                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }
                flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                flvwLettureCorrente.BuildList();
                _risposta = MessageBox.Show("Ciclo Completato, salvo i risultati ?", "Analisi Nucleo", MessageBoxButtons.OKCancel);
                if (_risposta == DialogResult.OK)
                {
                    tctCalGeneraExcel_Click(null, null);
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.LanciaSequenza: " + Ex.Message);
            }
        }

        private bool LanciaSequenzaCalibrazione(sbTestataCalibrazione SequenzaCorrente ,bool SalvaDati,int CorrenteMax = 300, int CorrenteVerMax = 300,int Spire = 2)
        {


            try
            {

                frmMessaggioElettrolita _msgElettro;
                DialogResult _risposta;
                int _ciclo;
                int _start;
                int _stop;
                int _passo;
                int _stepCount = 0;
                int _millisecondi = 1000;
                bool _esito;
                bool _apparatoPronto = false;

                float _maxErrPos = 0;
                float _maxErrNeg = 0;

                int _passoTest = 0;

                if (CorrenteMax > 400)
                {
                    CorrenteMax = 400;
                }

                sbAnalisiCorrente _vac;
                EsitoControlloValore _esitoCV;

                // intesto la registrazione
                SequenzaCorrente.IdApparato = _sb.Id;
                SequenzaCorrente.IdEsecuzione = 0;
                SequenzaCorrente.FirmwareAttivo = _sb.sbData.SwVersion;
                SequenzaCorrente.DataEsecuzione = DateTime.Now;
                SequenzaCorrente.Esito = (byte)EsitoControlloValore.NonEffettuato;
                SequenzaCorrente.ErroreMax = 0;
                SequenzaCorrente.ErroreMaxPos = 0;
                SequenzaCorrente.ErroreMaxNeg = 0;
                SequenzaCorrente.Spire = Spire;
                SequenzaCorrente.CorrenteMax = CorrenteMax;

                ValoriTestCorrente.Clear();

                GraficoCorrentiCalComplOxy();
                Application.DoEvents();


                txtCalCurr.Text = "";
                txtCalStepCorrente.Text = "";

                txtCalErrore.Text = "";
                txtCalSb.Text = "";
                lbxCalListaStep.Items.Clear();

                // Step 1: controllo che lo spyBatt sia attivo e collegato
                txtCalStepCorrente.Text = "Verifica Alimentazione ";
                lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                if (!_esito )
                {
                    txtCalStepCorrente.Text = EsitoControllo(EsitoControlloValore.ErroreLetturaSB);
                    lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    Application.DoEvents();

                    return false;
                }
                /*
                txtCalV1.Text = "";
                txtCalV2.Text = "";
                txtCalV3.Text = "";
                txtCalVbatt.Text = "";
                txtCalTemp.Text = "";
                */
                MostraLetture(true);

                _esitoCV = ControllaTensioni(24);

                MostraLetture();

                if (_esitoCV != EsitoControlloValore.EsitoPositivo)
                {
                    txtCalStepCorrente.Text = EsitoControllo(_esitoCV);
                    lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    Application.DoEvents();
                    SequenzaCorrente.Esito = (byte)_esitoCV;
                    SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                    return false;
                }


                //Step 2 verifico temperatura ed elettrolita


                _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                MostraLetture();

                txtCalStepCorrente.Text = "Verifica temperatura";
                lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);

                if ((_sb.sbVariabili.TempNTC < 15) || (_sb.sbVariabili.TempNTC > 60))
                {
                    // Temperatura anomala

                    txtCalStepCorrente.Text = "Rilevata temperatura anomala: " + _sb.sbVariabili.strTempNTC;
                    lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    MessageBox.Show(txtCalStepCorrente.Text, "Verifica Scheda", MessageBoxButtons.OK,MessageBoxIcon.Error);
                    SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreNTC;
                    SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                    return false;

                }

                txtCalStepCorrente.Text = "Rilevata temperatura valida: " + _sb.sbVariabili.strTempNTC;
                lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                txtCalTemp.Text = _sb.sbVariabili.strTempNTC;



                txtCalStepCorrente.Text = "Verifica Elettrolita";
                lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);

                //l'elettrolita non può essere presente
                if (_sb.sbVariabili.PresenzaElettrolita == 0xF0)
                {
                    MessageBox.Show("Rilevata presenza elettrolita a vuoto: Probabile corto !","VERIFICA FALLITA", MessageBoxButtons.OK,  MessageBoxIcon.Error);
                    txtCalStepCorrente.Text = "Verifica Elettrolita fallita";
                    lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    lbxCalListaStep.Items.Add("Probabile corto sulla linea Pres.El.");
                    SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreElettr;
                    SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                    return false;
                }

                //Ciclo fino a che non viene fatto il contatto

                _msgElettro = new frmMessaggioElettrolita(_sb);
                _msgElettro.ShowDialog();
                _esitoCV = _msgElettro.EsitoVerifica;
                _msgElettro.Dispose();

                MostraLetture();


                if (_esitoCV != EsitoControlloValore.EsitoPositivo)
                {
                    if (_esitoCV != EsitoControlloValore.IgnoraVerifica)
                    {
                        txtCalStepCorrente.Text = EsitoControllo(_esitoCV);
                        lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                        Application.DoEvents();
                        SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreElettr;
                        SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                        return false;
                    }
                    else
                    {
                        txtCalStepCorrente.Text = "Controllo Elettrolita non effettuato";
                        lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                        Application.DoEvents();
                    }

                }
                else
                {
                    txtCalStepCorrente.Text = "Controllo Elettrolita superato";
                    lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    Application.DoEvents();
                }

                // ora comincio la fase di calibrazione:
                _risposta = MessageBox.Show("Collegare l'Alimentatore alla presa DIRETTA", "Verifica Polarità", MessageBoxButtons.OKCancel);
                if (_risposta != DialogResult.OK)
                {
                    txtCalStepCorrente.Text = "Calibrazione Annullata";
                    lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    if (chkCalAttivaAlim.Checked)
                    {
                        Lambda.Alimentatatore.ImpostaStato(false);
                        Lambda.MostraStato();
                        MostraLetture();
                    }
                    SequenzaCorrente.Esito = (byte)EsitoControlloValore.AnnullaVerifica;
                    SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                    return false;

                }
                txtCalStepCorrente.Text = "Attivo L'alimentatore " + _sb.sbVariabili.strTempNTC;
                lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                // se previsto, attivo l'alimentatore
                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                if (chkCalAttivaAlim.Checked)
                {
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.Alimentatatore.ImpostaStato(true);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                    MostraLetture();
                }


                 txtCalStepCorrente.Text = "Verifica verso SPY-BATT " + _sb.sbVariabili.strTempNTC;
                lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);


                Application.DoEvents();
                _apparatoPronto = false;
                // 1 Verifico che lo SB sia dritto e montato correttamente mandando una corrente di prova

                do
                {

                    Lambda.Alimentatatore.ImpostaCorrente(10);
                    Lambda.MostraCorrenti();
                    System.Threading.Thread.Sleep(_millisecondi);
                    _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                    Lambda.MostraCorrenti();

                    if (_sb.sbVariabili.CorrenteBatteria == 0)
                    {
                        Lambda.Alimentatatore.ImpostaCorrente(0);
                        Lambda.MostraCorrenti();

                        _risposta = MessageBox.Show("SPY-BATT non collegato correttamente", "Verifica Collegamenti", MessageBoxButtons.RetryCancel);
                        if (_risposta == DialogResult.Cancel)
                        {
                            Lambda.Alimentatatore.ImpostaCorrente(0);
                            Lambda.MostraCorrenti();
                            if (chkCalAttivaAlim.Checked)
                            {
                                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                Lambda.Alimentatatore.ImpostaStato(false);
                                Lambda.MostraStato();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (_sb.sbVariabili.CorrenteBatteria <= 0)
                        {
                            Lambda.Alimentatatore.ImpostaCorrente(0);
                            Lambda.MostraCorrenti();
                            _risposta = MessageBox.Show("SPY-BATT non collegato correttamente; Verso Corrente errato", "Verifica Collegamenti", MessageBoxButtons.RetryCancel,MessageBoxIcon.Exclamation);
                            if (_risposta == DialogResult.Cancel)
                            {

                                if (chkCalAttivaAlim.Checked)
                                {
                                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                    Lambda.Alimentatatore.ImpostaStato(false);
                                    Lambda.MostraStato();
                                    SequenzaCorrente.Esito = (byte)EsitoControlloValore.AnnullaVerifica;
                                    SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            _apparatoPronto = true;

                            if (chkCalAttivaAlim.Checked & !Lambda.Alimentatatore.UscitaAttiva)
                            {
                                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                Lambda.Alimentatatore.ImpostaStato(true);
                                Lambda.MostraStato();
                                return false;
                            }
                        }
                    }
                }
                while (!_apparatoPronto);
                Lambda.Alimentatatore.ImpostaCorrente(0);
                Lambda.MostraCorrenti();
                MostraLetture();

                txtCalStepCorrente.Text = "Verifica Alimentatore OK (" + _sb.sbVariabili.strCorrenteBatteria + "A) ";
                lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);

                // Comincio con la calibrazione positiva:

                Lambda.Alimentatatore.ImpostaCorrente(CorrenteMax);
                System.Threading.Thread.Sleep(_millisecondi * 10);
                Lambda.MostraCorrenti();
                _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                MostraLetture();
                if (_sb.sbVariabili.CorrenteBatteria < 100 )
                {
                    // qualcosa non va, non leggo abbastanza corrente. Spengo e mi fermo
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.Alimentatatore.ImpostaStato(false);
                    txtCalStepCorrente.Text = "Calibrazione positiva fallita (" + _sb.sbVariabili.strCorrenteBatteria + "A) ";
                    lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrentePositiva;
                    SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                    return false;
                }

                _esito = _sb.ModalitaCalibrazione();
                if (_esito)
                {
                    _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                    if (_sb.sbVariabili.MemProgrammed == 0x03)
                    {
                        Lambda.Alimentatatore.LeggiCorrenti();

                        ushort _correnteErogata = (ushort)(Lambda.Alimentatatore.Arilevati * Spire * 10);

                        _esito = _sb.ScriviParametroCal(0x01, _correnteErogata);
                        if (_esito)
                        {
                            txtCalStepCorrente.Text = "Calibrazione positiva OK (" + _correnteErogata.ToString() + ")";
                            lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                        }
                        else
                        {
                            txtCalStepCorrente.Text = "Calibrazione positiva fallita";
                            lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                            SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrentePositiva;
                            SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                            return false;
                        }
                    }
                    else
                    {
                        txtCalStepCorrente.Text = "Attivazione Calibrazione positiva fallita";
                        lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                        SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrentePositiva;
                        SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                        return false;
                    }
                }
                else
                {
                    txtCalStepCorrente.Text = "Attivazione Calibrazione positiva fallita";
                    lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrentePositiva;
                    SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                    return false;
                }

                // Sono ancora in calibrazione, calibro a zero
                System.Threading.Thread.Sleep(_millisecondi);
                Lambda.Alimentatatore.ImpostaCorrente(0);
                System.Threading.Thread.Sleep(_millisecondi * 10);

                _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                MostraLetture();
                if (_sb.sbVariabili.MemProgrammed == 0x03)
                {
                    _esito = _sb.ScriviParametroCal(0x00, 0);
                    if (_esito)
                    {
                        txtCalStepCorrente.Text = "Calibrazione ZERO OK";
                        lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    }
                    else
                    {
                        txtCalStepCorrente.Text = "Calibrazione ZERO fallita";
                        lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                        SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrente;
                        SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                        return false;
                    }
                }
                else
                {
                    txtCalStepCorrente.Text = "Attivazione Calibrazione ZERO fallita";
                    lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrente;
                    SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                    return false;
                }

                //esco dalla calibrazone
                _esito = _sb.ModalitaCalibrazione();
                if (_esito)
                {
                    _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                    MostraLetture();
                    if (_sb.sbVariabili.MemProgrammed == 0x03)
                    {
                        txtCalStepCorrente.Text = "Uscita Calibrazione fallita";
                        lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                        SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrente;
                        SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                        return false;
                    }

                }

                // 1 Verifico che lo SB sia dritto e montato correttamente mandando una corrente di prova
                _risposta = MessageBox.Show("Collegare l'Alimentatore alla presa INVERSA", "Verifica Collegamenti", MessageBoxButtons.RetryCancel);

                do
                {

                    Lambda.Alimentatatore.ImpostaCorrente(10);
                    Lambda.MostraCorrenti();
                    System.Threading.Thread.Sleep(_millisecondi);
                    _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                    Lambda.MostraCorrenti();

                    if (_sb.sbVariabili.CorrenteBatteria == 0)
                    {
                        Lambda.Alimentatatore.ImpostaCorrente(0);
                        Lambda.MostraCorrenti();

                        _risposta = MessageBox.Show("SPY-BATT non collegato correttamente", "Verifica Collegamenti", MessageBoxButtons.RetryCancel);
                        if (_risposta == DialogResult.Cancel)
                        {
                            Lambda.Alimentatatore.ImpostaCorrente(0);
                            Lambda.MostraCorrenti();
                            if (chkCalAttivaAlim.Checked)
                            {
                                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                Lambda.Alimentatatore.ImpostaStato(false);
                                Lambda.MostraStato();
                                SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrenteNegativa;
                                SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (_sb.sbVariabili.CorrenteBatteria > 0)
                        {
                            Lambda.Alimentatatore.ImpostaCorrente(0);
                            Lambda.MostraCorrenti();
                            _risposta = MessageBox.Show("SPY-BATT non collegato correttamente; Verso Corrente errato", "Verifica Collegamenti", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                            if (_risposta == DialogResult.Cancel)
                            {

                                if (chkCalAttivaAlim.Checked)
                                {
                                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                    Lambda.Alimentatatore.ImpostaStato(false);
                                    Lambda.MostraStato();
                                    SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrenteNegativa;
                                    SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            _apparatoPronto = true;

                            if (chkCalAttivaAlim.Checked & !Lambda.Alimentatatore.UscitaAttiva)
                            {
                                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                Lambda.Alimentatatore.ImpostaStato(true);
                                Lambda.MostraStato();
                                SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrenteNegativa;
                                SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                                return false;
                            }
                        }
                    }
                }
                while (!_apparatoPronto);
                Lambda.Alimentatatore.ImpostaCorrente(0);
                Lambda.MostraCorrenti();
                MostraLetture();


                // Ora calibrazione negativa
                Lambda.Alimentatatore.ImpostaCorrente(CorrenteMax);
                System.Threading.Thread.Sleep(_millisecondi * 10 );
                Lambda.MostraCorrenti();
                _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                MostraLetture();
                if (_sb.sbVariabili.CorrenteBatteria > -100)
                {
                    // qualcosa non va, non leggo abbastanza corrente. Spengo e mi fermo
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.Alimentatatore.ImpostaStato(false);
                    txtCalStepCorrente.Text = "Calibrazione Negativa fallita (" + _sb.sbVariabili.strCorrenteBatteria + "A) ";
                    lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrenteNegativa;
                    SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                    return false;
                }

                _esito = _sb.ModalitaCalibrazione();
                if (_esito)
                {

                    _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                    if (_sb.sbVariabili.MemProgrammed == 0x03)
                    {
                        Lambda.Alimentatatore.LeggiCorrenti();

                        ushort _correnteErogata = (ushort)(Lambda.Alimentatatore.Arilevati * Spire * 10);

                        _esito = _sb.ScriviParametroCal(0x02, _correnteErogata);
                        if (_esito)
                        {
                            txtCalStepCorrente.Text = "Calibrazione Negativa OK (" + _correnteErogata.ToString() + ")" ;
                            lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                        }
                        else
                        {
                            txtCalStepCorrente.Text = "Calibrazione Negativa fallita";
                            lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                            SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrenteNegativa;
                            SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                            return false;
                        }
                    }
                    else
                    {
                        txtCalStepCorrente.Text = "Attivazione Calibrazione Negativa fallita";
                        lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                        SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrenteNegativa;
                        SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                        SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrenteNegativa;
                        SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                        return false;
                    }
                }
                else
                {
                    txtCalStepCorrente.Text = "Attivazione Calibrazione Negativa fallita";
                    lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                    SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrenteNegativa;
                    SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                    return false;
                }

                //esco dalla calibrazone
                _esito = _sb.ModalitaCalibrazione();
                if (_esito)
                {
                    _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                    MostraLetture();
                    if (_sb.sbVariabili.MemProgrammed == 0x03)
                    {
                        txtCalStepCorrente.Text = "Uscita Calibrazione fallita";
                        lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);
                        SequenzaCorrente.Esito = (byte)EsitoControlloValore.ErroreCorrenteNegativa;
                        SequenzaCorrente.Descrizione = txtCalStepCorrente.Text;
                        return false;
                    }

                }

                // Ora memorizzo le letture degli ADC
                _esito = _sb.CaricaCalibrazioni(_sb.Id, _apparatoPresente);
                if (_esito)
                {
                    SequenzaCorrente.AdcCurrentZero = _sb.Calibrazioni.AdcCurrentZero;
                    SequenzaCorrente.AdcCurrentPos = _sb.Calibrazioni.AdcCurrentPos;
                    SequenzaCorrente.AdcCurrentNeg = _sb.Calibrazioni.AdcCurrentNeg;

                    SequenzaCorrente.CurrentPos = _sb.Calibrazioni.CurrentPos;
                    SequenzaCorrente.CurrentNeg = _sb.Calibrazioni.CurrentNeg;

                }

                // ho completato le calibrazioni, parto con le curve di verifica





                System.Threading.Thread.Sleep(_millisecondi );
                Lambda.Alimentatatore.ImpostaCorrente(0);
                System.Threading.Thread.Sleep(_millisecondi);





                LanciaSequenzaVerificaCalibrazione(0, CorrenteVerMax,20,Spire);








                txtCalStepCorrente.Text = "Verifica SUPERATA ";
                lbxCalListaStep.Items.Add(txtCalStepCorrente.Text);





                return false;
                 }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.LanciaSequenza: " + Ex.Message);
                return false;
            }
        }

        private void LanciaSequenzaVerificaCalibrazione(int CorrenteBase,int CorrenteVerMax , int Passo, int Spire)
        {


            try
            {

                DialogResult _risposta;
                int _ciclo;
                int _start = CorrenteBase;
                int _stop = CorrenteVerMax;
                int _passo = Passo;
                int _spire = Spire;
                int _stepCount = 0;
                int _millisecondi = 1000;
                bool _esito;
                bool _apparatoPronto = false;

                float _maxErrPos = 0;
                float _maxErrNeg = 0;

                int _passoTest = 0;
                sbAnalisiCorrente _vac;

                txtCalCurr.Text = "";
                txtCalCurrStep.Text = "";

                txtCalErrore.Text = "";
                txtCalSb.Text = "";

                if (Lambda == null)
                {
                    return;
                }



                // Prima Preparo la lista valori
                ValoriTestCorrente.Clear();
                flvwCalCorrentiVerifica.SetObjects(ValoriTestCorrente);
                flvwCalCorrentiVerifica.BuildList();


                flvwCalCorrentiVerifica.Refresh();
                Application.DoEvents();

                _passo = 5;

                for (_ciclo = _start; _ciclo <= _stop; _ciclo += _passo)
                {
                    _vac = new sbAnalisiCorrente();
                    _vac.Lettura = (uint)_stepCount++;
                    _vac.Spire = (uint)_spire;
                    _vac.Ateorici = _ciclo;
                    _vac.Areali = _ciclo;
                    _vac.AspybattAN = 0;
                    _vac.AspybattAP = 0;
                    _vac.AspybattDN = 0;
                    _vac.AspybattDP = 0;
                    ValoriTestCorrente.Add(_vac);
                    if (_ciclo == 50) _passo = 10;
                    //if (_ciclo == 100) _passo = 20;
                }


                flvwCalCorrentiVerifica.Refresh();
                Application.DoEvents();

                // Ora comincio le corse: Arrivo dal test discendente per cui comincio in questo verso


                _risposta = MessageBox.Show("Collegare l'alimentatore alla presa INVERSA", "Verifica Polarità", MessageBoxButtons.OKCancel);
                if (_risposta == DialogResult.OK)

                    _apparatoPronto = false;
                //poi verifico che lo spybatt sia montato correttamente mandando una corrente di prova
                do
                {
                    if (chkCalAccendiAlim.Checked)
                    {
                        // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                        Lambda.Alimentatatore.ImpostaStato(true);
                        Lambda.MostraCorrenti();
                        Lambda.MostraStato();
                    }
                    Lambda.Alimentatatore.ImpostaCorrente(10);
                    Lambda.MostraCorrenti();
                    System.Threading.Thread.Sleep(_millisecondi);
                    _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                    Lambda.MostraCorrenti();
                    MostraLetture();

                    if (_sb.sbVariabili.CorrenteBatteria >= 0)
                    {
                        Lambda.Alimentatatore.ImpostaCorrente(0);
                        Lambda.MostraCorrenti();
                        if (chkCalAccendiAlim.Checked)
                        {
                            // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                            Lambda.Alimentatatore.ImpostaStato(false);
                            Lambda.MostraStato();
                            return;
                        }

                        _risposta = MessageBox.Show("SPY-BATT non collegato correttamente\nCollegare lo SPY-BATT per letture nel verso diretto\n(Nucleo verso il positivo)", "Verifica Polarità", MessageBoxButtons.RetryCancel);
                        if (_risposta == DialogResult.Cancel)
                        {
                            Lambda.Alimentatatore.ImpostaCorrente(0);
                            Lambda.MostraCorrenti();
                            if (chkCalAccendiAlim.Checked)
                            {
                                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                Lambda.Alimentatatore.ImpostaStato(false);
                                Lambda.MostraStato();
                                return;
                            }
                        }
                    }
                    else
                    {
                        _apparatoPronto = true;
                        if (chkCalAccendiAlim.Checked & !Lambda.Alimentatatore.UscitaAttiva)
                        {
                            // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                            Lambda.Alimentatatore.ImpostaStato(true);
                            Lambda.MostraStato();
                            return;
                        }
                    }
                }
                while (!_apparatoPronto);

                {
                    // ordine ascendente  
                    foreach (sbAnalisiCorrente _test in ValoriTestCorrente.OrderBy(par => par.Lettura))
                    {

                        {

                            //txtCalCurr.Text = _ciclo.ToString();
                            //txtCalCurrStep.Text = _stepCount.ToString();
                            Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                            Lambda.MostraCorrenti();

                            System.Threading.Thread.Sleep(_millisecondi);
                            _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                            Lambda.MostraCorrenti();
                            MostraLetture();
                            //txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                            if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                            else txtCalSb.ForeColor = Color.Black;

                            txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                            float _corrBase = Lambda.Alimentatatore.Arilevati;


                            _test.AspybattAN = (float)(_sb.sbVariabili.CorrenteBatteria / 10);

                            txtCalErrore.Text = "";
                            if (_corrBase != 0)
                            {
                                float _errore = Math.Abs((float)(-_sb.sbVariabili.CorrenteBatteria / 10) - (_corrBase * _spire)) / (_corrBase * _spire);
                                txtCalErrore.Text = _errore.ToString("p2");
                                txtCalErrMax.Text = _errore.ToString("p2");
                                if (_corrBase > 10)
                                {
                                    if (_errore > _maxErrNeg) _maxErrNeg = _errore;
                                    txtCalErroreMaxNeg.Text = _maxErrNeg.ToString("p2");
                                    txtCalErrMaxNeg.Text = _maxErrNeg.ToString("p2");
                                }
                            }

                            _stepCount++;


                            GraficoCorrentiCalComplOxy();
                            flvwCalCorrentiVerifica.Refresh();
                            flvwCalCorrentiVerifica.BuildList();
                            Application.DoEvents();

                            System.Threading.Thread.Sleep(100);

                        }
                        flvwCalCorrentiVerifica.Refresh();
                        flvwCalCorrentiVerifica.BuildList();
                        GraficoCorrentiComplOxy();
                        Application.DoEvents();
                    }

                    flvwLettureCorrente.SetObjects(ValoriTestCorrente);
                    GraficoCorrentiComplOxy();
                    Application.DoEvents();

                    // ordine discendente
                    foreach (sbAnalisiCorrente _test in ValoriTestCorrente.OrderByDescending(par => par.Lettura))
                    {

                        {

                            //txtCalCurr.Text = _ciclo.ToString();
                            //txtCalCurrStep.Text = _stepCount.ToString();
                            Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                            Lambda.MostraCorrenti();

                            System.Threading.Thread.Sleep(_millisecondi);
                            _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                            Lambda.MostraCorrenti();
                            //txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                            MostraLetture();
                            if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                            else txtCalSb.ForeColor = Color.Black;

                            txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                            float _corrBase = Lambda.Alimentatatore.Arilevati;


                            _test.AspybattDN = (float)(_sb.sbVariabili.CorrenteBatteria / 10);

                            txtCalErrore.Text = "";
                            if (_corrBase != 0)
                            {
                                float _errore = Math.Abs((float)(-_sb.sbVariabili.CorrenteBatteria / 10) - (_corrBase * _spire)) / (_corrBase * _spire);
                                txtCalErrore.Text = _errore.ToString("p2");
                                if (_corrBase > 10)
                                {
                                    if (_errore > _maxErrNeg) _maxErrNeg = _errore;
                                    txtCalErroreMaxNeg.Text = _maxErrNeg.ToString("p2");
                                }
                            }

                            _stepCount++;


                            GraficoCorrentiCalComplOxy();
                            flvwCalCorrentiVerifica.Refresh();
                            flvwCalCorrentiVerifica.BuildList();
                            Application.DoEvents();

                            System.Threading.Thread.Sleep(100);

                        }
                        GraficoCorrentiCalComplOxy();
                        Application.DoEvents();
                    }

                    flvwCalCorrentiVerifica.SetObjects(ValoriTestCorrente);
                    GraficoCorrentiCalComplOxy();
                    Application.DoEvents();
                }


                if (chkCalAccendiAlim.Checked)
                {
                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }




                // Ascendente

                _risposta = MessageBox.Show("Collegare lo SPY-BATT per letture nel verso diretto\n(Nucleo verso il positivo)", "Verifica Polarità", MessageBoxButtons.OKCancel);
                if (_risposta == DialogResult.OK)
                {


                    // se previsto, attivo la linea
                    if (chkCalAccendiAlim.Checked)
                    {
                        // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                        Lambda.Alimentatatore.ImpostaStato(false);
                        Lambda.Alimentatatore.ImpostaCorrente(0);
                        Lambda.Alimentatatore.ImpostaStato(true);
                        Lambda.MostraCorrenti();
                        Lambda.MostraStato();
                    }
                    flvwCalCorrentiVerifica.Refresh();
                    GraficoCorrentiCalComplOxy();
                    Application.DoEvents();
                    _apparatoPronto = false;
                    //poi verifico che lo spybatt sia montato correttamente mandando una corrente di prova
                    do
                    {

                        Lambda.Alimentatatore.ImpostaCorrente(10);
                        Lambda.MostraCorrenti();
                        System.Threading.Thread.Sleep(_millisecondi);
                        _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                        Lambda.MostraCorrenti();

                        if (_sb.sbVariabili.CorrenteBatteria <= 0)
                        {
                            Lambda.Alimentatatore.ImpostaCorrente(0);
                            Lambda.MostraCorrenti();

                            _risposta = MessageBox.Show("SPY-BATT non collegato correttamente\nCollegare lo SPY-BATT per letture nel verso diretto\n(Nucleo verso il positivo)", "Verifica Polarità", MessageBoxButtons.RetryCancel);
                            if (_risposta == DialogResult.Cancel)
                            {
                                Lambda.Alimentatatore.ImpostaCorrente(0);
                                Lambda.MostraCorrenti();
                                if (chkCalAccendiAlim.Checked)
                                {
                                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                    Lambda.Alimentatatore.ImpostaStato(false);
                                    Lambda.MostraStato();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            _apparatoPronto = true;

                            if (chkCalAccendiAlim.Checked & !Lambda.Alimentatatore.UscitaAttiva)
                            {
                                // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                                Lambda.Alimentatatore.ImpostaStato(true);
                                Lambda.MostraStato();
                                return;
                            }
                        }
                    }
                    while (!_apparatoPronto);

                    // ordine ascendente
                    ValoriTestCorrente.OrderBy(par => par.Lettura);
                    _passoTest = 0;
                    foreach (sbAnalisiCorrente _test in ValoriTestCorrente)
                    {
                        //txtCalCurr.Text = _ciclo.ToString();
                        //txtCalCurrStep.Text = _stepCount.ToString();
                        Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                        Lambda.MostraCorrenti();

                        System.Threading.Thread.Sleep(_millisecondi);
                        _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                        Lambda.MostraCorrenti();
                        txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                        if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                        else txtCalSb.ForeColor = Color.Black;

                        txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                        float _corrBase = Lambda.Alimentatatore.Arilevati;


                        _test.AspybattAP = (float)(_sb.sbVariabili.CorrenteBatteria / 10);

                        txtCalErrore.Text = "";
                        if (_corrBase != 0)
                        {
                            float _errore = Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10) - (_corrBase * _spire)) / (_corrBase * _spire);
                            txtCalErrore.Text = _errore.ToString("p2");
                            txtCalErrMax.Text = _errore.ToString("p2");
                            if (_corrBase > 10)
                            {
                                if (_errore > _maxErrPos) _maxErrPos = _errore;
                                txtCalErroreMaxPos.Text = _maxErrPos.ToString("p2");
                                txtCalErrMaxPos.Text = _maxErrPos.ToString("p2");
                            }

                        }

                        _stepCount++;


                        flvwCalCorrentiVerifica.SetObjects(ValoriTestCorrente);
                        //flvwLettureCorrente.Refresh();
                        //flvwLettureCorrente.BuildList();
                        Application.DoEvents();

                        System.Threading.Thread.Sleep(100);
                        GraficoCorrentiComplOxy();
                        Application.DoEvents();
                    }

                    flvwCalCorrentiVerifica.SetObjects(ValoriTestCorrente);
                    GraficoCorrentiCalComplOxy();
                    Application.DoEvents();

                    // ordine discendente

                    foreach (sbAnalisiCorrente _test in ValoriTestCorrente.OrderByDescending(par => par.Lettura))
                    {
                        Lambda.Alimentatatore.ImpostaCorrente(_test.Ateorici);
                        Lambda.MostraCorrenti();

                        System.Threading.Thread.Sleep(_millisecondi);
                        _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                        Lambda.MostraCorrenti();
                        txtCalSb.Text = _sb.sbVariabili.strCorrenteBatteria;
                        if (_sb.sbVariabili.CorrenteBatteria < 0) txtCalSb.ForeColor = Color.Red;
                        else txtCalSb.ForeColor = Color.Black;

                        txtCalIalim.Text = Lambda.Alimentatatore.Arilevati.ToString("0.0");
                        float _corrBase = Lambda.Alimentatatore.Arilevati;


                        _test.AspybattDP = (float)(_sb.sbVariabili.CorrenteBatteria / 10);

                        txtCalErrore.Text = "";
                        if (_corrBase != 0)
                        {
                            float _errore = Math.Abs((float)(_sb.sbVariabili.CorrenteBatteria / 10) - (_corrBase * _spire)) / (_corrBase * _spire);
                            txtCalErrore.Text = _errore.ToString("p2");
                            txtCalErrMax.Text = _errore.ToString("p2");
                            if (_corrBase > 10)
                            {
                                if (_errore > _maxErrPos) _maxErrPos = _errore;
                                txtCalErroreMaxPos.Text = _maxErrPos.ToString("p2");
                                txtCalErrMaxPos.Text = _maxErrPos.ToString("p2");
                            }
                        }

                        _stepCount++;


                        flvwCalCorrentiVerifica.SetObjects(ValoriTestCorrente);
                        //flvwLettureCorrente.Refresh();
                        //flvwLettureCorrente.BuildList();
                        Application.DoEvents();

                        System.Threading.Thread.Sleep(100);

                    }
                    flvwCalCorrentiVerifica.SetObjects(ValoriTestCorrente);
                    GraficoCorrentiCalComplOxy();
                    Application.DoEvents();

                }

                flvwCalCorrentiVerifica.BuildList();


                flvwCalCorrentiVerifica.Refresh();
                Application.DoEvents();

                if (chkCalAccendiAlim.Checked)
                {
                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }
                // Ora faccio invertire il collegamento poi rifaccio il test


                flvwCalCorrentiVerifica.Refresh();
                txtCalCurr.Text = _ciclo.ToString();
                txtCalCurrStep.Text = _stepCount.ToString();

                if (chkCalAccendiAlim.Checked)
                {
                    // Spengo l'alimentatore, metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(0);
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }
                flvwCalCorrentiVerifica.SetObjects(ValoriTestCorrente);
                flvwCalCorrentiVerifica.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.LanciaSequenza: " + Ex.Message);
            }
        }



        private void TestSequenzaVerificaCalibrazione()
        {


            try
            {

                DialogResult _risposta;
                int _ciclo;
                int _start = 5;
                int _stop = 300;
                int _passo = 20;
                int _spire = 2;
                int _stepCount = 0;
                int _millisecondi = 1000;
                bool _esito;
                bool _apparatoPronto = false;

                float _maxErrPos = 0;
                float _maxErrNeg = 0;

                /*
                int.TryParse(txtCalImin.Text, out _start);
                int.TryParse(txtCalImax.Text, out _stop);
                int.TryParse(txtCalIstep.Text, out _passo);
                int.TryParse(txtCalDelay.Text, out _millisecondi);

                int.TryParse(txtCalSpire.Text, out _spire);
                */

                int _passoTest = 0;
                sbAnalisiCorrente _vac;

                txtCalCurr.Text = "";
                txtCalCurrStep.Text = "";

                txtCalErrore.Text = "";
                txtCalSb.Text = "";

              



                // Prima Preparo la lista valori
                ValoriTestCorrente.Clear();
                flvwCalCorrentiVerifica.SetObjects(ValoriTestCorrente);
                flvwCalCorrentiVerifica.BuildList();


                flvwCalCorrentiVerifica.Refresh();
                Application.DoEvents();

                _passo = 5;

                for (_ciclo = _start; _ciclo <= _stop; _ciclo += _passo)
                {
                    _vac = new sbAnalisiCorrente();
                    _vac.Lettura = (uint)_stepCount++;
                    _vac.Spire = (uint)_spire;
                    _vac.Ateorici = _ciclo;
                    _vac.Areali = _ciclo;
                    _vac.AspybattAN = -(_ciclo * _spire) + 1;
                    _vac.AspybattAP = (_ciclo * _spire) + 2;
                    _vac.AspybattDN = -(_ciclo * _spire) - 1;
                    _vac.AspybattDP = (_ciclo * _spire) - 2;
                    ValoriTestCorrente.Add(_vac);
                    if (_ciclo == 40) _passo = 10;
                    if (_ciclo == 100) _passo = 20;
                }

                GraficoCorrentiCalComplOxy();
                flvwCalCorrentiVerifica.Refresh();
                Application.DoEvents();

                // Ora comincio le corse: Arrivo dal test discendente per cui comincio in questo verso

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.LanciaSequenza: " + Ex.Message);
            }
        }




        private void InizializzaVistaCorrentiCal()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 6, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 7, FontStyle.Bold);

                flvwCalCorrentiVerifica.HeaderUsesThemes = false;
                flvwCalCorrentiVerifica.HeaderFormatStyle = _stile;
                flvwCalCorrentiVerifica.UseAlternatingBackColors = true;
                flvwCalCorrentiVerifica.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvwCalCorrentiVerifica.AllColumns.Clear();

                flvwCalCorrentiVerifica.View = View.Details;
                flvwCalCorrentiVerifica.ShowGroups = false;
                flvwCalCorrentiVerifica.GridLines = true;

                BrightIdeasSoftware.OLVColumn colIdSoglia = new BrightIdeasSoftware.OLVColumn();
                colIdSoglia.Text = "ID";
                colIdSoglia.AspectName = "IdLocale";
                colIdSoglia.Width = 30;
                colIdSoglia.HeaderTextAlign = HorizontalAlignment.Left;
                colIdSoglia.TextAlign = HorizontalAlignment.Right;
                //flvwLettureCorrente.AllColumns.Add(colIdSoglia);

                BrightIdeasSoftware.OLVColumn colLettura = new BrightIdeasSoftware.OLVColumn();
                colLettura.Text = "N.";
                colLettura.AspectName = "Lettura";
                colLettura.Width = 40;
                colLettura.HeaderTextAlign = HorizontalAlignment.Center;
                colLettura.TextAlign = HorizontalAlignment.Right;
                flvwCalCorrentiVerifica.AllColumns.Add(colLettura);

                BrightIdeasSoftware.OLVColumn colCorrTeorica = new BrightIdeasSoftware.OLVColumn();
                colCorrTeorica.Text = "A def";
                colCorrTeorica.AspectName = "strAteorici";
                colCorrTeorica.Width = 50;
                colCorrTeorica.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrTeorica.TextAlign = HorizontalAlignment.Right;
                flvwCalCorrentiVerifica.AllColumns.Add(colCorrTeorica);


                BrightIdeasSoftware.OLVColumn colCorrEffettiva = new BrightIdeasSoftware.OLVColumn();
                colCorrEffettiva.Text = "A eff";
                colCorrEffettiva.AspectName = "strAreali";
                colCorrEffettiva.Width = 50;
                colCorrEffettiva.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrEffettiva.TextAlign = HorizontalAlignment.Right;
                flvwCalCorrentiVerifica.AllColumns.Add(colCorrEffettiva);

                BrightIdeasSoftware.OLVColumn colCorrSpyBatt = new BrightIdeasSoftware.OLVColumn();
                colCorrSpyBatt.Text = "A SB";
                colCorrSpyBatt.AspectName = "strAspybatt";
                colCorrSpyBatt.Width = 50;
                colCorrSpyBatt.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrSpyBatt.TextAlign = HorizontalAlignment.Right;
                flvwCalCorrentiVerifica.AllColumns.Add(colCorrSpyBatt);

                //-------------------------------------------- 

                BrightIdeasSoftware.OLVColumn colCorrSpyBattAP = new BrightIdeasSoftware.OLVColumn();
                colCorrSpyBattAP.Text = "AP(A)";
                colCorrSpyBattAP.ToolTipText = "Corrente SPY-BATT - Ciclo Ascendente Positivo ";
                colCorrSpyBattAP.AspectName = "strAspybattAP";
                colCorrSpyBattAP.Width = 40;
                colCorrSpyBattAP.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrSpyBattAP.TextAlign = HorizontalAlignment.Right;
                flvwCalCorrentiVerifica.AllColumns.Add(colCorrSpyBattAP);

                BrightIdeasSoftware.OLVColumn colCorrSpyBattDP = new BrightIdeasSoftware.OLVColumn();
                colCorrSpyBattDP.Text = "DP(A)";
                colCorrSpyBattDP.ToolTipText = "Corrente SPY-BATT - Ciclo Discendente Positivo ";
                colCorrSpyBattDP.AspectName = "strAspybattDP";
                colCorrSpyBattDP.Width = 50;
                colCorrSpyBattDP.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrSpyBattDP.TextAlign = HorizontalAlignment.Right;
                flvwCalCorrentiVerifica.AllColumns.Add(colCorrSpyBattDP);

                BrightIdeasSoftware.OLVColumn colCorrSpyBattAN = new BrightIdeasSoftware.OLVColumn();
                colCorrSpyBattAN.Text = "AN(A)";
                colCorrSpyBattDP.ToolTipText = "Corrente SPY-BATT - Ciclo Ascendente Negativo ";
                colCorrSpyBattAN.AspectName = "strAspybattAN";
                colCorrSpyBattAN.Width = 50;
                colCorrSpyBattAN.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrSpyBattAN.TextAlign = HorizontalAlignment.Right;
                flvwCalCorrentiVerifica.AllColumns.Add(colCorrSpyBattAN);

                BrightIdeasSoftware.OLVColumn colCorrSpyBattDN = new BrightIdeasSoftware.OLVColumn();
                colCorrSpyBattDN.Text = "DN(A)";
                colCorrSpyBattDP.ToolTipText = "Corrente SPY-BATT - Ciclo Discendente Negativo ";
                colCorrSpyBattDN.AspectName = "strAspybattDN";
                colCorrSpyBattDN.Width = 50;
                colCorrSpyBattDN.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrSpyBattDN.TextAlign = HorizontalAlignment.Right;
                flvwCalCorrentiVerifica.AllColumns.Add(colCorrSpyBattDN);



                //-------------------------------------------- 


                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 50;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvwCalCorrentiVerifica.AllColumns.Add(colRowFiller);

                flvwCalCorrentiVerifica.RebuildColumns();

                flvwCalCorrentiVerifica.SetObjects(ValoriTestCorrente);
                flvwCalCorrentiVerifica.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }


        private void InizializzaOxyGrCalibrazione()
        {
            try
            {
                this.oxyContainerGrCalVerifica = new OxyPlot.WindowsForms.PlotView();
                //this.SuspendLayout();
                // 
                // plot1
                // 
                this.oxyContainerGrCalVerifica.Dock = System.Windows.Forms.DockStyle.Fill;
                this.oxyContainerGrCalVerifica.Location = new System.Drawing.Point(10, 10);
                //this.oxyContainer.Margin = new System.Windows.Forms.Padding(0);
                this.oxyContainerGrCalVerifica.Name = "oxyContainerGrSingolo";
                this.oxyContainerGrCalVerifica.PanCursor = System.Windows.Forms.Cursors.Hand;
                //this.oxyContainerGrCalVerifica.Size = new System.Drawing.Size(517, 452);
                //this.oxyContainer.TabIndex = 0;
                this.oxyContainerGrCalVerifica.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
                this.oxyContainerGrCalVerifica.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
                this.oxyContainerGrCalVerifica.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
                this.oxyContainerGrCalVerifica.Click += new System.EventHandler(this.oxyContainerGrSingolo_Click);
                // 

                pnlCalVerifica.Controls.Add(this.oxyContainerGrCalVerifica);

                oxyGraficoCalVerifica = new OxyPlot.PlotModel
                {
                    Title = "",
                    Subtitle = "",
                    PlotType = OxyPlot.PlotType.XY,
                    Background = OxyPlot.OxyColors.White
                };

                oxyContainerGrCalVerifica.Model = oxyGraficoCalVerifica;

            }

            catch (Exception Ex)
            {
                Log.Error("InizializzaOxyPlotControl: " + Ex.Message);
            }

        }


        public void GraficoCorrentiCalOxy()
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;

                //tabStatGrafici.BackColor = Color.LightYellow;

                // Preparo le serie di valori

                ValoriPuntualiGrCorrenti.Clear();


                int ValMinX;
                int ValMaxX;

                int ValMinY;
                int ValMaxY;

                ValMinX = 0;
                ValMinY = 0;
                ValMaxX = 100;


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                oxyGraficoAnalisi.Series.Clear();
                oxyGraficoAnalisi.Axes.Clear();

                oxyGraficoAnalisi.Background = OxyPlot.OxyColors.LightYellow;
                oxyGraficoAnalisi.PlotAreaBackground = OxyPlot.OxyColors.White;
                oxyGraficoAnalisi.PlotAreaBorderThickness = new OxyPlot.OxyThickness(1, 1, 1, 1);


                oxyGraficoAnalisi.Title = "Correnti";
                oxyGraficoAnalisi.TitleFont = "Utopia";
                oxyGraficoAnalisi.TitleFontSize = 14;


                // Creo gli Assi

                OxyPlot.Axes.CategoryAxis AsseCat = new OxyPlot.Axes.CategoryAxis();
                // AsseCat.MinorStep = 1;

                OxyPlot.Axes.LinearAxis AsseConteggi = new OxyPlot.Axes.LinearAxis();
                // AsseCat.MinorStep = 1;
                // AsseConteggi.Minimum = 0;
                // AsseCo1nteggi.Maximum = 100;



                //Creo le serie:
                OxyPlot.Series.LineSeries serValore = new OxyPlot.Series.LineSeries();
                serValore.Title = "Erogata";
                serValore.DataFieldX = "Lettura";
                serValore.DataFieldY = "Corrente";
                serValore.Color = OxyPlot.OxyColors.Green;

                OxyPlot.Series.LineSeries serLettura = new OxyPlot.Series.LineSeries();
                serLettura.Title = "I Rilevata";
                serLettura.DataFieldX = "Lettura";
                serLettura.DataFieldY = "Corrente";
                serLettura.Color = OxyPlot.OxyColors.Red;



                // carico il Dataset


                float _errore = 0;

                foreach (sbAnalisiCorrente _vac in ValoriTestCorrente)
                {

                    if (_vac.Areali != 0)
                    {
                        _errore = 100 * (_vac.Aspybatt - _vac.Areali * _vac.Spire) / (_vac.Areali * _vac.Spire);
                    }
                    else
                        _errore = 0;

                    serValore.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.Areali * _vac.Spire));
                    serLettura.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.Aspybatt));


                }





                oxyGraficoAnalisi.Axes.Add(AsseCat);



                //serValore.XAxisKey = "Lettura";
                //serValore.YAxisKey = "Corrente";

                oxyGraficoAnalisi.Series.Add(serValore);
                oxyGraficoAnalisi.Series.Add(serLettura);


                oxyGraficoAnalisi.InvalidatePlot(true);

            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }


        public void GraficoCorrentiCalComplOxy()
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;

                byte _modelloAttivo = 0x01;

                //tabStatGrafici.BackColor = Color.LightYellow;

                // Preparo le serie di valori

                ValoriPuntualiGrCalCorrenti.Clear();

                if (cmbCalTipoGrafico.SelectedValue != null)
                {
                    TipoGrafico _tg = (TipoGrafico)cmbCalTipoGrafico.SelectedItem;
                    _modelloAttivo = _tg.Codice;
                }


                int ValMinX;
                int ValMaxX;

                int ValMinY;
                int ValMaxY;

                ValMinX = 0;
                ValMinY = 0;
                ValMaxX = 100;


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                oxyGraficoCalVerifica.Series.Clear();
                oxyGraficoCalVerifica.Axes.Clear();

                oxyGraficoCalVerifica.Background = OxyPlot.OxyColors.LightYellow;
                oxyGraficoCalVerifica.PlotAreaBackground = OxyPlot.OxyColors.White;
                oxyGraficoCalVerifica.PlotAreaBorderThickness = new OxyPlot.OxyThickness(1, 1, 1, 1);

                switch (_modelloAttivo)
                {
                    case 0x01:
                        {
                            oxyGraficoCalVerifica.Title = "Errore Assoluto misura Corrente";
                            break;
                        }
                    case 0x02:
                        {
                            oxyGraficoCalVerifica.Title = "Errore misura Corrente";
                            break;
                        }
                    case 0x03:
                        {
                            oxyGraficoCalVerifica.Title = "% Errore misura Corrente";
                            break;
                        }
                    case 0x04:
                    default:
                        {
                            oxyGraficoCalVerifica.Title = "Misura Corrente Rilevata";
                            break;
                        }
                }



                oxyGraficoCalVerifica.TitleFont = "Utopia";
                oxyGraficoCalVerifica.TitleFontSize = 14;

                //oxyGraficoAnalisi.LegendBackground = OxyPlot.OxyColor.W
                oxyGraficoCalVerifica.LegendBorder = OxyPlot.OxyColors.Black;
                oxyGraficoCalVerifica.LegendPlacement = OxyPlot.LegendPlacement.Inside;
                oxyGraficoCalVerifica.LegendPosition = OxyPlot.LegendPosition.TopLeft;

                // Creo gli Assi

                OxyPlot.Axes.CategoryAxis AsseCat = new OxyPlot.Axes.CategoryAxis();
                AsseCat.MajorStep = 1;
                //AsseCat.Minimum = -5;
                //AsseCat.Maximum = 5;
                AsseCat.Unit = " A ";
                AsseCat.StringFormat = "0";

                OxyPlot.Axes.LinearAxis AsseConteggi = new OxyPlot.Axes.LinearAxis();
                AsseConteggi.AxislineColor = OxyPlot.OxyColors.Red;
                
                AsseConteggi.MajorGridlineStyle = OxyPlot.LineStyle.Solid;
                AsseConteggi.MinorGridlineStyle = OxyPlot.LineStyle.Dot;
                AsseCat.MinorStep = 1;
                AsseConteggi.Minimum = -5;
                AsseConteggi.Maximum = 5;
                AsseConteggi.PositionAtZeroCrossing = true;


                AsseConteggi.TickStyle = OxyPlot.Axes.TickStyle.Outside;
                AsseConteggi.Unit = " % Err ";

                AsseConteggi.AxislineStyle = OxyPlot.LineStyle.Solid;

                AsseConteggi.MajorGridlineColor = OxyPlot.OxyColors.LightBlue;
                AsseConteggi.MinorGridlineColor = OxyPlot.OxyColors.LightBlue;
                AsseConteggi.AxislineColor = OxyPlot.OxyColors.Blue;
                AsseConteggi.TextColor = OxyPlot.OxyColors.Blue;
                AsseConteggi.AxislineThickness = 2;









                //Creo le serie:
                OxyPlot.Series.LineSeries serValore = new OxyPlot.Series.LineSeries();
                serValore.Title = "Erogata";
                serValore.DataFieldX = "Lettura";
                serValore.DataFieldY = "Corrente";
                serValore.Color = OxyPlot.OxyColors.Blue;

                OxyPlot.Series.LineSeries serLetturaAP = new OxyPlot.Series.LineSeries();
                serLetturaAP.Title = "I Asc.Pos.";
                serLetturaAP.DataFieldX = "Lettura";
                serLetturaAP.DataFieldY = "Corrente";
                serLetturaAP.Color = OxyPlot.OxyColors.Green;

                OxyPlot.Series.LineSeries serLetturaDP = new OxyPlot.Series.LineSeries();
                serLetturaDP.Title = "I Disc. Pos";
                serLetturaDP.DataFieldX = "Lettura";
                serLetturaDP.DataFieldY = "Corrente";
                serLetturaDP.Color = OxyPlot.OxyColors.GreenYellow;

                OxyPlot.Series.LineSeries serLetturaAN = new OxyPlot.Series.LineSeries();
                serLetturaAN.Title = "I Asc. Neg.";
                serLetturaAN.DataFieldX = "Lettura";
                serLetturaAN.DataFieldY = "Corrente";
                serLetturaAN.Color = OxyPlot.OxyColors.Red;

                OxyPlot.Series.LineSeries serLetturaDN = new OxyPlot.Series.LineSeries();
                serLetturaDN.Title = "I Disc. Neg";
                serLetturaDN.DataFieldX = "Lettura";
                serLetturaDN.DataFieldY = "Corrente";
                serLetturaDN.Color = OxyPlot.OxyColors.OrangeRed;


                // carico il Dataset


               // float _errore = 0;

                foreach (sbAnalisiCorrente _vac in ValoriTestCorrente)
                {
                    float _label = _vac.Areali * _vac.Spire;
                    AsseCat.Labels.Add(_label.ToString());
                }


                foreach (sbAnalisiCorrente _vac in ValoriTestCorrente)
                {
                    if (_vac.Areali > 0)
                    {
                        float _iEff = _vac.Areali * _vac.Spire;

                        switch (_modelloAttivo)
                        {
                            case 0x01:
                                {
                                    // "Errore Assoluto misura Corrente";

                                    serValore.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, 0));

                                    if (_vac.AspybattAP > 0)
                                        serLetturaAP.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, Math.Abs(_vac.AspybattAP - _iEff)));
                                    if (_vac.AspybattDP > 0)
                                        serLetturaDP.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, Math.Abs(_vac.AspybattDP - _iEff)));

                                    if (_vac.AspybattAN < 0)
                                        serLetturaAN.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, Math.Abs(_vac.AspybattAN + _iEff)));
                                    if (_vac.AspybattDN < 0)
                                        serLetturaDN.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, Math.Abs(_vac.AspybattDN + _iEff)));

                                    break;
                                }
                            case 0x02:
                                {
                                    //"Errore misura Corrente";
                                    serValore.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, 0));

                                    if (_vac.AspybattAP > 0)
                                        serLetturaAP.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, (_vac.AspybattAP - _iEff) ));
                                    if (_vac.AspybattDP > 0)
                                        serLetturaDP.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, (_vac.AspybattDP - _iEff) ));

                                    if (_vac.AspybattAN < 0)
                                        serLetturaAN.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, (_vac.AspybattAN + _iEff)));
                                    if (_vac.AspybattDN < 0)
                                        serLetturaDN.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, (_vac.AspybattDN + _iEff)));

                                break;
                                }
                            case 0x03:
                                {
                                    // "% Errore misura Corrente";
                                    serValore.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, 0));

                                    if (_vac.AspybattAP > 0)
                                        serLetturaAP.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, (_vac.AspybattAP - _iEff) / _iEff * 100));
                                    if (_vac.AspybattDP > 0)
                                        serLetturaDP.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, (_vac.AspybattDP - _iEff) / _iEff * 100));

                                    if (_vac.AspybattAN < 0)
                                        serLetturaAN.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, (_vac.AspybattAN + _iEff) / _iEff * 100));
                                    if (_vac.AspybattDN < 0)
                                        serLetturaDN.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, (_vac.AspybattDN + _iEff) / _iEff * 100));
                                    break;
                                }
                            case 0x04:
                            default:
                                {
                                    // "Misura Corrente Rilevata";

                                    if (_vac.AspybattAP > 0)
                                        serLetturaAP.Points.Add(new OxyPlot.DataPoint(_vac.Lettura,_vac.AspybattAP ));
                                    if (_vac.AspybattDP > 0)
                                        serLetturaDP.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.AspybattDP));

                                    if (_vac.AspybattAN < 0)
                                        serLetturaAN.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.AspybattAN ));
                                    if (_vac.AspybattDN < 0)
                                        serLetturaDN.Points.Add(new OxyPlot.DataPoint(_vac.Lettura, _vac.AspybattDN ));
                                    break;

                                }
                        }




                    }
                }





                oxyGraficoCalVerifica.Axes.Add(AsseCat);



                //serValore.XAxisKey = "Lettura";
                //serValore.YAxisKey = "Corrente";

                oxyGraficoCalVerifica.Series.Add(serValore);
                oxyGraficoCalVerifica.Series.Add(serLetturaAP);
                oxyGraficoCalVerifica.Series.Add(serLetturaDP);
                oxyGraficoCalVerifica.Series.Add(serLetturaAN);
                oxyGraficoCalVerifica.Series.Add(serLetturaDN);


                oxyGraficoCalVerifica.InvalidatePlot(true);

            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }


        private void MostraLetture(bool ClearOnly = false, bool refreshData = false)
        {
            try
            {
                bool _esito;

                txtCalV1.Text = "";
                txtCalV2.Text = "";
                txtCalV3.Text = "";
                txtCalVbatt.Text = "";
                txtCalTemp.Text = "";
                txtCalElettr.Text = "";

                txtCalCurrAlim.Text = "";
                txtCalCurrSB.Text = "";

                txtCalErrMax.Text = "";
                txtCalErrMaxPos.Text = "";
                txtCalErrMaxNeg.Text = "";


                if (ClearOnly)
                {
                    return;
                }

                // Da parameti SPY-BATT
                if(refreshData)
                {
                    _esito = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);

                }
                else
                {
                    _esito = true;
                }
                if (_esito)
                {
                    txtCalV1.Text = _sb.sbVariabili.strTensione1;
                    txtCalV2.Text = _sb.sbVariabili.strTensione2;
                    txtCalV3.Text = _sb.sbVariabili.strTensione3;
                    txtCalVbatt.Text = _sb.sbVariabili.strTensioneIstantanea;
                    txtCalTemp.Text = _sb.sbVariabili.strTempNTC;
                    txtCalElettr.Text = _sb.sbVariabili.strPresenzaElettrolita;

                    txtCalCurrSB.Text = _sb.sbVariabili.strCorrenteBatteria;
                }

                // Da Alimentatore

                if (refreshData)
                {
                    Lambda.MostraCorrenti();
                }

                txtCalCurrAlim.Text = Lambda.Alimentatatore.strArilevati;

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.MostraLetture: " + Ex.Message);
            }
        }

        /// <summary>
        /// Controlla se la lettura in corrente è corrispondente al velore desiderato.
        /// </summary>
        /// <param name="Verso">The verso.</param>
        /// <param name="Corrente">The corrente.</param>
        /// <param name="ErroreAmmesso">Percentuale di errore ammessa.</param>
        /// <param name="Spire">Numero di spire.</param>
        /// <param name="Silent">if set to <c>true</c> [silent].</param>
        /// <returns></returns>
        public EsitoControlloValore ControllaCorrente(VersoCorrente Verso, float Corrente = 20 ,int ErroreAmmesso = 5, int Spire = 1,bool Silent = false)
        {
            try
            {

                float _erroreAmmesso = Math.Abs(Corrente * ErroreAmmesso) / 100;  // Massimo errore ammesso : 5%
                int _loop = 0;
                EsitoControlloValore _esito = EsitoControlloValore.NonEffettuato;
                bool _esitoSB;

                if (Lambda == null)
                {
                    _esito = EsitoControlloValore.AlimentatoreScollegato;
                    return _esito;
                }

                    // Spengo l'alimentatore e metto a 0 la corrente poi lo accendo e parto col ciclo
                    Lambda.Alimentatatore.ImpostaStato(false);
                    Lambda.Alimentatatore.ImpostaCorrente(Corrente);
                if (!Silent)
                {
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }
                Application.DoEvents();

                // Accendo l'alimentatore
                Lambda.Alimentatatore.ImpostaStato(true);

                // Aspetto fino a 2 secondi così si stabilizza l'uscita
                Lambda.Alimentatatore.LeggiCorrenti();
                _loop = 0;
                while (Math.Abs(Lambda.Alimentatatore.Aimpostati - Lambda.Alimentatatore.Arilevati) < _erroreAmmesso)
                {
                    System.Threading.Thread.Sleep(200);
                    Lambda.Alimentatatore.LeggiCorrenti();
                    if (!Silent)
                    {
                        Lambda.MostraCorrenti();
                        Lambda.MostraStato();
                    }
                    _loop++;
                    if ( _loop> 10 )
                    {
                        // sono passati 10 cicli e l'uscita non è ancora al valore desiderato
                        //Spengo ed esco
                        Lambda.Alimentatatore.ImpostaStato(false);
                        if (!Silent)
                        {
                            Lambda.MostraCorrenti();
                            Lambda.MostraStato();
                        }

                        _esito = EsitoControlloValore.ErroreAlimentatore;
                        return _esito;
                    }

                }


                // L'alimentatore sta erogando la corrente richiesta: leggo la corrente rilevata

                _esitoSB = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);

                // prima fermo l'erogazione
                Lambda.Alimentatatore.ImpostaStato(false);
                if (!Silent)
                {
                    Lambda.MostraCorrenti();
                    Lambda.MostraStato();
                }

                if (_esitoSB)
                {
                    float _correnteSB = (float)_sb.sbVariabili.CorrenteBatteria / 10;
                    float _correnteErogata = Lambda.Alimentatatore.Arilevati;

                    if (Verso == VersoCorrente.Inverso)
                        _correnteErogata = _correnteErogata * (-1);


                    _esito = EsitoControlloValore.ErroreLetturaSB;
                    return _esito;

                }
                else
                {
                    _esito = EsitoControlloValore.ErroreLetturaSB;
                    return _esito;
                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.ControllaCorrente: " + Ex.Message);
                return EsitoControlloValore.ErroreGenerico;
            }
        }


        public EsitoControlloValore ControllaTensioni(float Tensione = 24, int ErroreAmmesso = 10, byte schemaContatti = 0x55 ,   bool Silent = false)
        {
            try
            {

                float _tensioneVerificata;
                float _erroreAmmesso = Math.Abs(Tensione * ErroreAmmesso) / 100;  // Massimo errore ammesso : 5%
                float _tensioneRilevata;
                int _loop = 0;
                EsitoControlloValore _esito = EsitoControlloValore.NonEffettuato;
                bool _esitoSB;

                _esitoSB = _sb.CaricaVariabili(_sb.Id, _apparatoPresente);
                if (!_esitoSB) return EsitoControlloValore.ErroreLetturaSB;

                // Verifico la Vbatt
                _tensioneVerificata = Tensione;
                _erroreAmmesso = Math.Abs(_tensioneVerificata * ErroreAmmesso) / 100;
                _tensioneRilevata = (float)(_sb.sbVariabili.TensioneIstantanea) / 100;
                if (Math.Abs(_tensioneRilevata - _tensioneVerificata) > _erroreAmmesso) return EsitoControlloValore.ErroreVBatt;
                txtCalVbatt.Text = _sb.sbVariabili.strTensioneIstantanea;

                // Verifico la V3
                _tensioneVerificata = Tensione * 3/4;
                _erroreAmmesso = Math.Abs(_tensioneVerificata * ErroreAmmesso) / 100;
                _tensioneRilevata = (float)(_sb.sbVariabili.Tensione3) / 100;
                if (Math.Abs(_tensioneRilevata - _tensioneVerificata) > _erroreAmmesso) return EsitoControlloValore.ErroreV3;
                txtCalV3.Text = _sb.sbVariabili.strTensione3;

                // Verifico la V2
                _tensioneVerificata = Tensione / 2;
                _erroreAmmesso = Math.Abs(_tensioneVerificata * ErroreAmmesso) / 100;
                _tensioneRilevata = (float)(_sb.sbVariabili.Tensione2) / 100;
                if (Math.Abs(_tensioneRilevata - _tensioneVerificata) > _erroreAmmesso) return EsitoControlloValore.ErroreV2;
                txtCalV2.Text = _sb.sbVariabili.strTensione2;

                // Verifico la V1
                _tensioneVerificata = Tensione / 4;
                _erroreAmmesso = Math.Abs(_tensioneVerificata * ErroreAmmesso) / 100;
                _tensioneRilevata = (float)(_sb.sbVariabili.Tensione1) / 100;
                if (Math.Abs(_tensioneRilevata - _tensioneVerificata) > _erroreAmmesso) return EsitoControlloValore.ErroreV1;
                txtCalV1.Text = _sb.sbVariabili.strTensione1;


                return EsitoControlloValore.EsitoPositivo;

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.ControllaCorrente: " + Ex.Message);
                return EsitoControlloValore.ErroreGenerico;
            }
        }


        private string EsitoControllo(EsitoControlloValore Result, bool Silent = false, string Titolo = "Controllo Scheda")
        {
            try
            {
                string _imageTitle = "";
                string _message;
                MessageBoxButtons _pulsanti;
                MessageBoxIcon _icona;

                switch(Result)
                {
                    case EsitoControlloValore.EsitoPositivo:
                        {
                            _imageTitle = Titolo;
                            _message = "Controllo Completato con Successo";
                            _pulsanti = MessageBoxButtons.OK;
                            _icona = MessageBoxIcon.Information;
                            break;
                        }

                    case EsitoControlloValore.CorrenteInversa:
                        {
                            _imageTitle = Titolo;
                            _message = "Verso Corrente Errato";
                            _pulsanti = MessageBoxButtons.OK;
                            _icona = MessageBoxIcon.Error;
                            break;
                        }

                    case EsitoControlloValore.IgnoraVerifica:
                        {
                            _imageTitle = Titolo;
                            _message = "Verifica ignorata";
                            _pulsanti = MessageBoxButtons.OK;
                            _icona = MessageBoxIcon.Exclamation;
                            break;
                        }

                    case EsitoControlloValore.AnnullaVerifica:
                        {
                            _imageTitle = Titolo;
                            _message = "Verifica Annullata";
                            _pulsanti = MessageBoxButtons.OK;
                            _icona = MessageBoxIcon.Exclamation;
                            break;
                        }
                    default:
                        {
                            _imageTitle = Titolo;
                            _message = "Errore NON DEFINITO";
                            _pulsanti = MessageBoxButtons.OK;
                            _icona = MessageBoxIcon.Error;
                            break;
                        }

                }
                if (!Silent)
                {
                    DialogResult _esito = MessageBox.Show(_message, _imageTitle, _pulsanti, _icona);
                }

                return _message;

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBatt.EsitoControllo: " + Ex.Message);
                return Ex.Message;
            }
        }

    }
}