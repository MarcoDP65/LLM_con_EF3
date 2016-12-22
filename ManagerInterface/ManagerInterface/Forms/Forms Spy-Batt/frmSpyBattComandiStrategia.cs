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

        public bool LanciaComandoTestStrategia(byte ComandoStrategia)
        {
            try
            {

                byte[] _Dati;
                bool _esito;

                txtStratDataGrid.Text = "";
                _Dati = new byte[252];
                _esito = _sb.LanciaComandoTestStrategia(ComandoStrategia, out _Dati);

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
                    txtStratDataGrid.Text = _risposta;

                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LanciaComandoTestStrategia: " + Ex.Message);
                return false;
            }

        }

        public bool LanciaComandoStrategiaInfo(byte ComandoStrategia)
        {
            try
            {

                byte[] _Dati;
                bool _esito;
                ushort _tempMin;

                txtStratDataGrid.Text = "";
                _Dati = new byte[252];
                _esito = _sb.LanciaComandoTestStrategia(ComandoStrategia, out _Dati);

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
                    txtStratDataGrid.Text = _risposta;

                    // Mostro i valori
                    txtStratQryVerLib.Text = _Dati[0x03].ToString() + "." + _Dati[0x04].ToString("00") + "." + FunzioniComuni.UshortFromArray(_Dati, 0x05).ToString("000");
                    txtStratQryActSeup.Text = _Dati[0x08].ToString();
                    txtStratQryTensN.Text = FunzioniMR.StringaTensione(FunzioniComuni.UshortFromArray(_Dati, 0x09));
                    txtStratQryCapN.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 0x0B));
                    txtStratQryTensGas.Text = FunzioniMR.StringaTensioneCella(FunzioniComuni.UshortFromArray(_Dati, 0x11));
                    txtStratQryTatt.Text = FunzioniMR.StringaTemperatura(_Dati[0x13]);
                    txtStratQryTalm.Text = FunzioniMR.StringaTemperatura(_Dati[0x14]);
                    txtStratQryTrepr.Text = FunzioniMR.StringaTemperatura(_Dati[0x15]);
                    txtStratQryModoPian.Text = _Dati[0x16].ToString();
                    txtStratQryGg.Text = DataOraMR.SiglaGiorno(_Dati[0x17] + 1) + "-" + _Dati[0x17].ToString();
                    _tempMin = (ushort)((_Dati[0x19] << 8) + _Dati[0x1A]);
                    txtStratQryMinChg.Text = _tempMin.ToString();
                    txtStratQryFC.Text = FunzioniMR.StringaFattoreCarica(_Dati[0x1B]);
                    txtStratQryLLBattId.Text = FunzioniComuni.ArrayToString(_Dati, 0x1D, 5);


                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LanciaComandoTestStrategia: " + Ex.Message);
                return false;
            }

        }

        public bool LanciaComandoStrategiaChechCnt(byte ComandoStrategia)
        {
            try
            {

                byte[] _Dati;
                bool _esito;

                txtStratDataGrid.Text = "";
                _Dati = new byte[252];
                _esito = _sb.ComandoInfoStrategia(ComandoStrategia, out _Dati);

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
                    txtStratDataGrid.Text = _risposta;

                    // Mostro i valori
                    txtStratLivcrgPos.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 13));
                    txtStratLivcrgCrg.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 3));
                    txtStratLivcrgCrgTot.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 7));

                    txtStratLivcrgNeg.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 11));
                    txtStratLivcrgDiscrg.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 5));
                    txtStratLivcrgDiscrgTot.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 9));

                    txtStratLivcrgCapResidua.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 15));


                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LanciaComandoTestStrategia: " + Ex.Message);
                return false;
            }

        }

        public bool LanciaComandoStrategiaUpdCnt(byte ComandoStrategia)
        {
            try
            {

                byte[] _Dati;
                bool _esito;
                ushort _tempCapacity;
                ushort _tempChg;
                ushort _tempDschg;
                if (txtStratLivcrgSetCapacity.Text != "")
                {
                    _tempCapacity = FunzioniMR.ConvertiUshort(txtStratLivcrgSetCapacity.Text ,10,0xFFFF);
                }
                else
                {
                    _tempCapacity = 0xFFFF;
                }

                if (txtStratLivcrgSetDschg.Text != "")
                {
                    _tempDschg = FunzioniMR.ConvertiUshort(txtStratLivcrgSetDschg.Text, 10, 0xFFFF);
                }
                else
                {
                    _tempDschg = 0xFFFF;
                }

                if (txtStratLivcrgSetChg.Text != "")
                {
                    _tempChg = FunzioniMR.ConvertiUshort(txtStratLivcrgSetChg.Text, 10, 0xFFFF);
                }
                else
                {
                    _tempChg = 0xFFFF;
                }


                txtStratDataGrid.Text = "";
                _Dati = new byte[252];
                _esito = _sb.ComandoStrategiaAggiornaContatori(_tempCapacity, _tempDschg, _tempChg, out _Dati);

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
                    txtStratDataGrid.Text = _risposta;

                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LanciaComandoTestStrategia: " + Ex.Message);
                return false;
            }

        }

        public bool LanciaComandoStrategiaAvanzametoFase()
        {
            try
            {

                byte[] _Dati;
                bool _esito;
                byte _comandoInfo = 0x03;

                txtStratDataGrid.Text = "";
                _Dati = new byte[252];
                _esito = _sb.ComandoInfoStrategia(_comandoInfo, out _Dati);

                if (_esito == true & _Dati.Length >3)
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
                    txtStratDataGrid.Text = _risposta;
                    ushort _Erogato;
                    ushort _previsto;
                    // Mostro i valori
                    _Erogato = FunzioniComuni.UshortFromArray(_Dati, 3);

                    _previsto = FunzioniComuni.UshortFromArray(_Dati, 5);
                    txtStratAVErogati.Text = FunzioniMR.StringaCorrenteLL(_Erogato);
                    txtStratAVPrevisti.Text = FunzioniMR.StringaCorrenteLL(_previsto);
                    if (_Erogato <= _previsto)
                        txtStratAVMancanti.Text = FunzioniMR.StringaCorrenteLL((ushort)(  _previsto - _Erogato));
                    else
                        txtStratAVMancanti.Text = "";


                    txtStratAVMinutiResidui.Text = FunzioniComuni.UshortFromArray(_Dati, 0x07).ToString();

                    // Aggiungere i moìinuti trascorsi

                    txtStratAVTensioneIst.Text = FunzioniMR.StringaTensione(FunzioniComuni.UshortFromArray(_Dati, 0x0B));

                    //txtStratAVCorrenteIst.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 0x0D));
                    txtStratAVCorrenteIst.Text = FunzioniMR.StringaCorrenteSigned(FunzioniComuni.ArrayToShort(_Dati, 0x0D,2));

                    txtStratAVTempIst.Text = FunzioniMR.StringaTemperatura(_Dati[0x0F]);

                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LanciaComandoTestStrategia: " + Ex.Message);
                return false;
            }

        }


        public bool LanciaComandoStrategiaLetturaVariabili()
        {
            try
            {

                byte[] _Dati;
                bool _esito;
                byte _comandoInfo = 0x52;

                txtStratDataGrid.Text = "";
                _Dati = new byte[252];
                _esito = _sb.ComandoInfoStrategia(_comandoInfo, out _Dati);

                if (_esito == true & _Dati.Length > 3)
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
                    txtStratDataGrid.Text = _risposta;
                    /*
                    ushort _Erogato;
                    ushort _previsto;
                    // Mostro i valori
                    _Erogato = FunzioniComuni.UshortFromArray(_Dati, 3);
                    _previsto = FunzioniComuni.UshortFromArray(_Dati, 5);
                    txtStratAVErogati.Text = FunzioniMR.StringaCorrenteLL(_Erogato);
                    txtStratAVPrevisti.Text = FunzioniMR.StringaCorrenteLL(_previsto);
                    if (_Erogato <= _previsto)
                        txtStratAVMancanti.Text = FunzioniMR.StringaCorrenteLL((ushort)(_previsto - _Erogato));
                    else
                        txtStratAVMancanti.Text = "";


                    txtStratAVMinutiResidui.Text = FunzioniComuni.UshortFromArray(_Dati, 0x07).ToString();

                    // Aggiungere i moìinuti trascorsi

                    txtStratAVTensioneIst.Text = FunzioniMR.StringaTensione(FunzioniComuni.UshortFromArray(_Dati, 0x0B));
                    txtStratAVCorrenteIst.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 0x0D));
                    txtStratAVTempIst.Text = FunzioniMR.StringaTemperatura(_Dati[0x0F]);
                    */

                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LanciaComandoTestStrategia: " + Ex.Message);
                return false;
            }

        }


        public bool LanciaComandoStrategia(byte Modo)
        {
            try
            {

                byte[] _Dati;
                bool _esito;

                ushort _tempVmin;
                ushort _tempVmax;
                ushort _tempAmax;
                byte _tempFC;
                byte _tempRabb;
                byte[] _StepBuffer = new byte[16];


                if (txtStratLLVmin.Text != "")
                {
                    _tempVmin = FunzioniMR.ConvertiUshort(txtStratLLVmin.Text, 100, 0x960);  // vmin default 24.00 V
                }
                else
                {
                    _tempVmin = 0x00F0;
                }

                if (txtStratLLVmax.Text != "")
                {
                    _tempVmax = FunzioniMR.ConvertiUshort(txtStratLLVmax.Text, 100, 0x2580);  //VMax default 96.00 V
                }
                else
                {
                    _tempVmax = 0x03C0;
                }

                if (txtStratLLFC.Text != "")
                {
                    _tempFC = FunzioniMR.ConvertiByte(txtStratLLFC.Text, 100, 115);
                }
                else
                {
                    _tempFC = 115;
                }



                if (txtStratLLAmax.Text != "")
                {
                    _tempAmax = FunzioniMR.ConvertiUshort(txtStratLLAmax.Text, 10, 0x04B0);
                }
                else  
                {
                    _tempAmax = 0x04B0;
                }

                if (chkStratLLRabb.Checked)
                    _tempRabb = 0xF0;
                else
                    _tempRabb = 0x0F;


                txtStratDataGrid.Text = "";
                _Dati = new byte[252];




                txtStratIsEsito.ForeColor = Color.Black;

                _esito = _sb.LanciaComandoStrategia(Modo, _tempVmin, _tempVmax, _tempAmax, _tempRabb, _tempFC, out _Dati);

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
                    txtStratDataGrid.Text = _risposta;
                    if (_Dati.Length < 4 )
                    {
                        txtStratIsEsito.ForeColor = Color.Red;
                        txtStratIsEsito.Text = "ERR";
                        return false;

                    }

                        // Mostro i valori
                        txtStratIsEsito.Text = _Dati[0x02].ToString();
                    if (_Dati[0x02] > 255)
                    {
                        txtStratIsAhRich.Text = "";
                        txtStratIsMinuti.Text = "";
                        txtStratIsStep.Text ="";
                        txtStratIsNumSpire.Text = "";

                    }
                    else
                    {
                        txtStratIsAhRich.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 0x03));
                        txtStratIsMinuti.Text = FunzioniComuni.UshortFromArray(_Dati, 0x05).ToString();
                        txtStratIsStep.Text = _Dati[0x07].ToString();
                        txtStratIsNumSpire.Text = _Dati[0x08].ToString();
                    }

                    // ora carico la struttura passi
                    PassiStrategia.Clear();
                    for(byte _steps = 0; _steps < _Dati[0x07]; _steps++)
                    {
                        int _startpoint = 15 + _steps * 16;
                        if (_Dati.Length <(_startpoint+16))
                        {
                            //risposta troppo corta
                            break;
                        }

                        Array.Copy(_Dati, _startpoint, _StepBuffer, 0, 16);
                        StepCarica _passoCorrente = new StepCarica();
                        _passoCorrente.CaricaDati(_StepBuffer, (byte)(_steps + 1));
                        PassiStrategia.Add(_passoCorrente);


                    }
                    cmbStratIsSelStep.DisplayMember = "strPasso";
                    //cmbStratIsSelStep.ValueMember = "Dati.IdStep";
                    cmbStratIsSelStep.DataSource = PassiStrategia;
                    cmbStratIsSelStep.Refresh();
                }




                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LanciaComandoTestStrategia: " + Ex.Message);
                return false;
            }

        }

        private bool CancellaPassoCorrente()
        {
            try
            {
                grbStratStepCorrente.Text = "";

                txtStratCurrStepTipo.Text = "";
                txtStratCurrStepImin.Text = "";
                txtStratCurrStepImax.Text = "";
                txtStratCurrStepVmin.Text = "";
                txtStratCurrStepVmax.Text = "";
                txtStratCurrStepToff.Text = "";
                txtStratCurrStepTon.Text = "";
                txtStratCurrStepRipetizioni.Text = "";

                return true;
            }

            catch
            {
                return false;
            }
        }


        private bool MostraPassoCorrente(StepCarica StepCorrente)
        {
            try
            {
                CancellaPassoCorrente();

                if (StepCorrente == null)
                    return false;

                grbStratStepCorrente.Text = StepCorrente.strPasso;

                txtStratCurrStepTipo.Text = StepCorrente.strTipoStep;
                txtStratCurrStepImin.Text = StepCorrente.strIMinima;
                txtStratCurrStepImax.Text = StepCorrente.strIMassima;
                txtStratCurrStepVmin.Text = StepCorrente.strVMinima;
                txtStratCurrStepVmax.Text = StepCorrente.strVMassima;
                txtStratCurrStepTBlocco.Text = StepCorrente.strTBlocco;
                txtStratCurrStepToff.Text = StepCorrente.strToff;
                txtStratCurrStepTon.Text = StepCorrente.strTon;
                txtStratCurrStepRipetizioni.Text = StepCorrente.strPasso;

                return true;
            }

            catch
            {
                return false;
            }
        }


        
        private void inizializzaComboPro()
        {
            try
            {
                cmbModoPianificazione.DataSource = _parametriPro.TipiPianificazione;
                cmbModoPianificazione.DisplayMember = "Descrizione";
                cmbModoPianificazione.ValueMember = "Codice";
                cmbModoPianificazione.SelectedIndex = 0;

                cmbBiberonaggio.DataSource = _parametriPro.OpzioniBib;
                cmbBiberonaggio.DisplayMember = "Descrizione";
                cmbBiberonaggio.ValueMember = "Codice";
                cmbBiberonaggio.SelectedIndex = 0;

                cmbRabboccatore.DataSource = _parametriPro.OpzioniRabb;
                cmbRabboccatore.DisplayMember = "Descrizione";
                cmbRabboccatore.ValueMember = "Codice";
                cmbRabboccatore.SelectedIndex = 0;



            }
            catch (Exception Ex)
            {
                Log.Error("inizializzaComboPro: " + Ex.Message);
            }
        }
     


    }
}
