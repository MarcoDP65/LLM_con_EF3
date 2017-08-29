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
                     byte ErrCode = _Dati[0x02];
                    if (ErrCode == 0)
                    {
                       
                        txtStratQryError.ForeColor = Color.Black;
                    }
                    else
                    {
                        txtStratQryError.ForeColor = Color.Red;

                    }
                    Application.DoEvents();
                    string _errorMsg = "" ;
                    switch (ErrCode)
                    {
                        case 0x00:
                            _errorMsg = "OK (0x00)";
                            break;
                        case 0x70:
                            _errorMsg = "ERR_SB_NOCONFIG";
                            break;
                        case 0x71:
                            _errorMsg = "ERR_SB_NOSTRAT";
                            break;
                        case 0x72:
                            _errorMsg = "ERR_SB_NOCOUNT";
                            break;
                        case 0x73:
                            _errorMsg = "ERR_SB_SOCZERO";
                            break;
                        default:
                            _errorMsg = "N.D. (0x" + ErrCode.ToString("x2") + ")";
                            break;
                    }

                    txtStratQryError.Text = _errorMsg;
                    txtStratQryVerLib.Text = _Dati[0x03].ToString() + "." + _Dati[0x04].ToString("00") + "." + FunzioniComuni.UshortFromArray(_Dati, 0x05).ToString("000");
                    txtStratQryActSeup.Text = _Dati[0x08].ToString();
                    txtStratQryTensN.Text = FunzioniMR.StringaTensione(FunzioniComuni.UshortFromArray(_Dati, 0x09));
                    txtStratQryCapN.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 0x0B));
                    txtStratQryTensGas.Text = FunzioniMR.StringaTensioneCella(FunzioniComuni.UshortFromArray(_Dati, 0x11));
                    txtStratQryTatt.Text = FunzioniMR.StringaTemperatura(_Dati[0x13]);
                    txtStratQryTalm.Text = FunzioniMR.StringaTemperatura(_Dati[0x14]);
                    txtStratQryTrepr.Text = FunzioniMR.StringaTemperatura(_Dati[0x15]);
                    txtStratQryModoPian.Text =  _Dati[0x16].ToString("X2");
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


        //--------------------------------------------------------------------------

        public bool LanciaComandoStrategiaReadTE()
        {
            try
            {

                byte[] _Dati;
                bool _esito;
                txtStratTEGetReal.Text = "";
                txtStratTEGetLocal.Text = "";
                txtStratDataGrid.Text = "";
                _Dati = new byte[252];
                _esito = _sb.ComandoInfoStrategia(0x53, out _Dati);

                if (_esito == true)
                {

                    // Scrivo il dump della risposta
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
                    if (_Dati.Length == 5)
                    {
                        // Mostro i valori
                        txtStratTEGetReal.Text = _Dati[3].ToString("x2");
                        txtStratTEGetLocal.Text = _Dati[4].ToString("x2");
                    }
                    else
                    {
                        txtStratTEGetReal.Text = "N.D.";
                        txtStratTEGetLocal.Text = "N.D.";

                    }
                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LanciaComandoTestStrategia: " + Ex.Message);
                return false;
            }

        }

        public bool LanciaComandoStrategiaWriteTE()
        {
            try
            {

                byte[] _Dati;
                bool _esito;
                byte _tempTE;

                if (txtStratLivcrgSetCapacity.Text != "")
                {
                    if (byte.TryParse(txtStratTESetLocal.Text, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _tempTE) != true)
                        _tempTE = 0xFF;
                }
                else
                {
                    _tempTE = 0xFF;
                }



                txtStratDataGrid.Text = "";
                _Dati = new byte[252];
                _esito = _sb.ComandoStrategiaAggiornaTE(_tempTE,  out _Dati);

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
                    if(_Dati[2]==0)
                    {
                        txtStratTEGetReal.Text = _Dati[3].ToString("x2");
                        txtStratTEGetLocal.Text = _Dati[4].ToString("x2");
                    }
                    else
                    {
                        txtStratTEGetReal.Text = "";
                        txtStratTEGetLocal.Text = "";
                    }


                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LanciaComandoTestStrategia: " + Ex.Message);
                return false;
            }

        }

        //--------------------------------------------------------------------------------

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


        public bool LanciaComandoStrategiaChechPar()
        {
            try
            {

                byte[] _Dati;
                bool _esito;
                byte ComandoStrategia = 0x56;

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
                    txtStratParGetSoC.Text =_Dati[3].ToString();
                    txtStratParGetProg.Text = _Dati[4].ToString();
                    txtStratParGetLng.Text = "";
                    txtStratParGetBrevi.Text = "";
                    /*
                    txtStratLivcrgNeg.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 11));
                    txtStratLivcrgDiscrg.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 5));
                    txtStratLivcrgDiscrgTot.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 9));

                    txtStratLivcrgCapResidua.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 15));
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

        public bool LanciaComandoStrategiaUpdPar(byte ComandoStrategia)
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
                    _tempCapacity = FunzioniMR.ConvertiUshort(txtStratLivcrgSetCapacity.Text, 10, 0xFFFF);
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






        public bool LanciaComandoStrategiaAvanzamentoFase()
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
                ushort _tempMinuti;
                byte _tempFC;
                byte _tempRabb;
                byte _tempLocalTE;



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

                if (txtStratLLMinuti.Text != "")
                {
                    _tempMinuti = FunzioniMR.ConvertiUshort(txtStratLLMinuti.Text, 1, 480 );  //VMax default 96.00 V
                }
                else
                {
                    _tempMinuti = 480;
                }



                if (chkStratLLRabb.Checked)
                    _tempRabb = 0xF0;
                else
                    _tempRabb = 0x0F;

                switch(cmbStratLLTELoc.SelectedIndex)
                {
                    case 1:
                        _tempLocalTE = 0x02;
                        break;
                    case 2:
                        _tempLocalTE = 0x0F;
                        break;
                    case 3:
                        _tempLocalTE = 0xF0;
                        break;
                    case 4:
                        _tempLocalTE = 0xAA;
                        break;
                    default:
                        _tempLocalTE = 0x00;
                        break;

                }


                txtStratDataGrid.Text = "";
                _Dati = new byte[252];




                txtStratIsEsito.ForeColor = Color.Black;

                _esito = _sb.LanciaComandoStrategia(Modo, _tempVmin, _tempVmax, _tempAmax, _tempRabb, _tempFC, _tempMinuti, _tempLocalTE, out _Dati);

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

                    //cmbStratIsSelStep.ValueMember = "Dati.IdStep";
                    cmbStratIsSelStep.DataSource = null;
                    cmbStratIsSelStep.DataSource = PassiStrategia;
                    cmbStratIsSelStep.DisplayMember = "strPasso";
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
                txtStratCurrStepRipetizioni.Text = StepCorrente.strNumRipetizioni;

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
