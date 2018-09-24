
//    class frmSpyBattProxyEsp

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

        public bool LanciaComandoTestEsp32(byte ComandoEsp32)
        {
            try
            {

                byte[] _Dati;
                bool _esito;

                txtStratDataGrid.Text = "";
                _Dati = new byte[252];
                _esito = _sb.LanciaComandoTestEsp32(ComandoEsp32, out _Dati);

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
                    txtEsp32DataGrid.Text = _risposta;

                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LanciaComandoTestStrategia: " + Ex.Message);
                return false;
            }

        }

        public bool LanciaComandoEsp32Info(byte ComandoStrategia)
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
                    string _errorMsg = "";
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
                    txtStratQryModoPian.Text = _Dati[0x16].ToString("X2");
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

        public bool LanciaComandoEsp32SetLed(byte StatoLed)
        {
            try
            {

                byte[] _Dati;
                bool _esito;
                byte _comandoInfo = 0x52;

                txtStratDataGrid.Text = "";
                _Dati = new byte[252];
                _esito = _sb.ComandoEsp32SetLed(StatoLed, out _Dati);

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




    }
}
