using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BrightIdeasSoftware;

using ChargerLogic;
using log4net;
using log4net.Config;

using MoriData;
using Utility;

namespace PannelloCharger
{
    public partial class frmCaricabatterie : Form

    {

        public bool ChiamataProxySig60CSInfo(byte ComandoStrategia)
        {
            try
            {

                byte[] _Dati;
                bool _esito;
                int _cmdlen = 3;


                txtStratDataGridTx.Text = "Waiting to send CMD";
                txtStratDataGridRx.Text = "Waiting for Answer";

                _Dati = new byte[_cmdlen];


                for (int _i = 0; _i < _cmdlen; _i++)
                {
                    _Dati[_i] = 0;
                }

                // Ora compongo il comando specifico 
                _Dati[0] = 0x80;   // strategia
                _Dati[1] = 0xA0;   // CMD_QRY
                _Dati[2] = 2;      // lunghezza comando


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
                txtStratDataGridTx.Text = _risposta;
                Application.DoEvents();

                _esito = _cb.ProxySBSig60(ref _Dati);
                
                if (_esito == true)
                {


                     _risposta = "";
                     _colonne = 0;
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
                    txtStratDataGridRx.Text = _risposta;

                    // Mostro i valori
                    txtStratQryVerLib.Text = _Dati[0x03].ToString() + "." + _Dati[0x04].ToString("00") + "." + FunzioniComuni.UshortFromArray(_Dati, 0x05).ToString("000");
                    txtStratQryActSeup.Text = _Dati[0x08].ToString();
                    txtStratQryTensN.Text = FunzioniMR.StringaTensione(FunzioniComuni.UshortFromArray(_Dati, 0x09));
                    txtStratQryCapN.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 0x0B));
                    txtStratQryTensGas.Text = FunzioniMR.StringaTensioneCella(FunzioniComuni.UshortFromArray(_Dati, 0x11));
                    txtStratQryTatt.Text = FunzioniMR.StringaTemperatura(_Dati[0x13]);
                    txtStratQryTalm.Text = FunzioniMR.StringaTemperatura(_Dati[0x14]);
                    txtStratQryTrepr.Text = FunzioniMR.StringaTemperatura(_Dati[0x15]);

                }


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("LanciaComandoTestStrategia: " + Ex.Message);
                return false;
            }

        }

        public bool ChiamataProxySig60CSAvanzamento(byte ComandoStrategia)
        {
            try
            {

                byte[] _Dati;
                bool _esito;
                int _cmdlen = 3;


                txtStratDataGridTx.Text = "Waiting to send CMD_AV";
                txtStratDataGridRx.Text = "Waiting for Answer";

                _Dati = new byte[_cmdlen];


                for (int _i = 0; _i < _cmdlen; _i++)
                {
                    _Dati[_i] = 0;
                }

                // Ora compongo il comando specifico 
                _Dati[0] = 0x80;   // strategia
                _Dati[1] = 0x03;   // CMD_AV
                _Dati[2] = 03;      // lunghezza comando
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
                txtStratDataGridTx.Text = _risposta;

                Application.DoEvents();

                _esito = _cb.ProxySBSig60(ref _Dati);

                if (_esito == true)
                {


                     _risposta = "";
                     _colonne = 0;
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
                    txtStratDataGridRx.Text = _risposta;

                    // Mostro i valori

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
                    //txtStratAVCorrenteIst.Text = FunzioniMR.StringaCorrenteLL(FunzioniComuni.UshortFromArray(_Dati, 0x0D));
                    //txtStratAVCorrenteIst.Text = FunzioniMR.StringaCorrente(FunzioniComuni.ArrayToShort(_Dati, 0x0D, 2));
                    txtStratAVCorrenteIst.Text = FunzioniMR.StringaCorrenteSigned(FunzioniComuni.ArrayToShort(_Dati, 0x0D, 2));
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


        public void ChiamataProxySig60(int Comando)
        {
            bool _esito;
            byte _comando = 0xF7;
            byte[] _tempBuffer = new byte[220];
            int _cmdlen = 220;
            try
            {

                switch (Comando)
                {
                    case 1:
                        _cmdlen = 220;
                        _comando = 0x71;
                        break;
                    case 2:
                        _cmdlen = 04;
                        _comando = 0x72;
                        break;
                    case 3:
                        _cmdlen = 04;
                        _comando = 0x7F;
                        break;
                    case 4:
                        _cmdlen = 02;
                        _comando = 0xA0;
                        break;
                    default:
                        _cmdlen = 220;
                        break;

                }

                _tempBuffer = new byte[_cmdlen];

                txtStratDataGridTx.Text = "";
                txtStratDataGridRx.Text = "";
                for (int _i = 0; _i < _cmdlen; _i++)
                {
                    _tempBuffer[_i] = 0;
                }

                _tempBuffer[0] = 0x80;


                _tempBuffer[1] = _comando;
                if (_cmdlen > 3)
                {
                    _tempBuffer[2] = 0x20;
                    _tempBuffer[3] = 0xFF;
                }

                for (int _i = 4; _i < _cmdlen; _i++)
                {
                    _tempBuffer[_i] = 0x00; // (byte)(_i-4);
                }


                if (true)
                {


                    string _risposta = "";
                    int _colonne = 0;
                    for (int _i = 0; _i < _tempBuffer.Length; _i++)
                    {
                        _risposta += _tempBuffer[_i].ToString("X2") + " ";
                        _colonne += 1;
                        if (_colonne > 0 && (_colonne % 4) == 0) _risposta += "  ";
                        if (_colonne > 15)
                        {
                            _risposta += "\r\n";
                            _colonne = 0;

                        }
                    }
                    txtStratDataGridTx.Text = _risposta;

                }

                Application.DoEvents();

                _esito = _cb.ProxySBSig60(ref _tempBuffer);
                if (_esito == true)
                {


                    string _risposta = "";
                    int _colonne = 0;
                    for (int _i = 0; _i < _cb.DatiRisposta.Length; _i++)
                    {
                        _risposta += _cb.DatiRisposta[_i].ToString("X2") + " ";
                        _colonne += 1;
                        if (_colonne > 0 && (_colonne % 4) == 0) _risposta += "  ";
                        if (_colonne > 15)
                        {
                            _risposta += "\r\n";
                            _colonne = 0;

                        }
                    }
                    txtStratDataGridRx.Text = _risposta;

                }

            }
            catch
            {
            }
        }


        public void ChiamataProxySig60_02()
        {
            bool _esito;
            int Comando = 4;
            byte[] _tempBuffer = new byte[220];
            int _cmdlen = 220;
            try
            {

                switch (Comando)
                {
                    case 1:
                        _cmdlen = 220;
                        break;
                    case 2:
                        _cmdlen = 20;

                        break;
                    default:
                        _cmdlen = 220;
                        break;

                }

                _tempBuffer = new byte[_cmdlen];

                txtStratDataGridTx.Text = "";
                txtStratDataGridRx.Text = "";
                for (int _i = 0; _i < 220; _i++)
                {
                    _tempBuffer[_i] = 0;
                }

                _tempBuffer[0] = 0x80;
                _tempBuffer[1] = 0x72;
                _tempBuffer[2] = 0x20;
                _tempBuffer[3] = 0xFF;


                if (true)
                {


                    string _risposta = "";
                    int _colonne = 0;
                    for (int _i = 0; _i < _tempBuffer.Length; _i++)
                    {
                        _risposta += _tempBuffer[_i].ToString("X2") + " ";
                        _colonne += 1;
                        if (_colonne > 0 && (_colonne % 4) == 0) _risposta += "  ";
                        if (_colonne > 23)
                        {
                            _risposta += "\r\n";
                            _colonne = 0;

                        }
                    }
                    txtStratDataGridTx.Text = _risposta;

                }
                Application.DoEvents();


                _esito = _cb.ProxySBSig60(ref _tempBuffer);
                if (_esito == true)
                {


                    string _risposta = "";
                    int _colonne = 0;
                    for (int _i = 0; _i < _cb.DatiRisposta.Length; _i++)
                    {
                        _risposta += _cb.DatiRisposta[_i].ToString("X2") + " ";
                        _colonne += 1;
                        if (_colonne > 0 && (_colonne % 4) == 0) _risposta += "  ";
                        if (_colonne > 23)
                        {
                            _risposta += "\r\n";
                            _colonne = 0;

                        }
                    }
                    txtStratDataGridRx.Text = _risposta;

                }

            }
            catch
            {
            }
        }




    }
}
