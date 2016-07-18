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


    }
}
