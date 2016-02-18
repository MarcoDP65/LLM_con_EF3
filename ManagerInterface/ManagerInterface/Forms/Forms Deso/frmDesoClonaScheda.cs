using System;
using System.Collections;
using System.Collections.Generic;
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
using NextUI.Component;
using NextUI.Frame;

using Common.Logging;
using BrightIdeasSoftware;
using Utility;
using MdiHelper;



namespace PannelloCharger
{
    public partial class frmDesolfatatore : Form
    {

        UnitaSpyBatt _sbTemp;  // classe sb usata come buffer temporaneo per le operazioni di clonatura
        CloneSB DatiClone;

        public bool PreparaClonazioneScheda()
        {
            try
            {
                int _step = 0;
                if (_sbTemp == null) return false;

                DatiClone = new CloneSB();

                // Preparo I dati cliente

                DatiClone.DatiCliente = _sbTemp.sbCliente.DataArray;
                txtClonaDimCliente.Text = DatiClone.DatiCliente.Length.ToString();

                // Carico le programmazioni prima ordinando i cicli

                List<sbProgrammaRicarica> _sortedProg = _sbTemp.Programmazioni.OrderByDescending(o => o.IdProgramma).ToList();
                int _prgToSkip = _sortedProg.Count - 24;
                //----------------------------------------------------- TEST: MANDO SOLO LA 1
                
                foreach (sbProgrammaRicarica _prog in _sortedProg )
                {
                    if (_prgToSkip > 0)
                    {
                        _prgToSkip--;
                    }
                    else
                    {
                        DatiClone.AggiungiProgrammazione(_prog);
                    }
                }
                

               // sbProgrammaRicarica _prog = _sortedProg.First< sbProgrammaRicarica>();
               // DatiClone.AggiungiProgrammazione(_prog);

                _sbTemp.sbData.ContProg = (ushort)DatiClone.NumeroProgrammazioni;
                txtClonaDimProgr.Text = DatiClone.Programmazioni.Length.ToString();
                txtClonaNumProgr.Text = DatiClone.NumeroProgrammazioni.ToString();

                _step = DatiClone.Programmazioni.Length / 200;
                if ((DatiClone.Programmazioni.Length % 200) > 0)
                    _step++;
                txtClonaPktProgr.Text = _step.ToString();

                Log.Warn(" ---------------------------------- Programmazioni -----------------------------------");
                Log.Warn(FunzioniMR.hexdumpArray(DatiClone.Programmazioni, true));

                bool _primoLungo = true;
                int _contalunghi = 0;

                // Carico I lunghi
                List<sbMemLunga> _sortedLong = _sbTemp.CicliMemoriaLunga.OrderBy(o => o.IdMemoriaLunga).ToList();
                int _maxCicli = 10000000;
                if (txtMaxCicli.Text != "")
                    int.TryParse(txtMaxCicli.Text, out _maxCicli);
               
                foreach (sbMemLunga _long in _sortedLong)
                {
                    bool _primoCorto = true;
                    List<sbMemBreve> _sortedShort = _long.CicliMemoriaBreve.OrderBy(o => o.IdMemoriaBreve).ToList();
                    _long.PuntatorePrimoBreve = 0;

                    foreach (sbMemBreve _short in _sortedShort)
                    {

                        DatiClone.AggiungiCicloBreve(_short);
                        if (_primoCorto)
                        {
                            _long.PuntatorePrimoBreveEff = (uint)DatiClone.NumCicliBrevi-1;
                            _primoCorto = false;
                        }
                    }
                    //--------------------------- fORZATO AL PRG 1
                    //_long.IdProgramma = 1;
                    //---------------------------

                    DatiClone.AggiungiCicloLungo(_long);
                    if (++_contalunghi >= _maxCicli) break;
                }

                txtClonaDimLunghi.Text = DatiClone.CicliLunghi.Length.ToString();
                txtClonaNumLunghi.Text = DatiClone.NumCicliLunghi.ToString();
                txtClonaDimBrevi.Text = DatiClone.CicliBrevi.Length.ToString();
                txtClonaNumBrevi.Text = DatiClone.NumCicliBrevi.ToString();

                _step = DatiClone.CicliBrevi.Length / 200;
                if ((DatiClone.CicliBrevi.Length % 200) > 0)
                    _step++;
                txtClonaPktBrevi.Text = _step.ToString();

                _step = DatiClone.CicliLunghi.Length / 200;
                if ((DatiClone.CicliLunghi.Length % 200) > 0)
                    _step++;
                txtClonaPktLunghi.Text = _step.ToString();

                _sbTemp.sbData.ContLunghi = (uint)DatiClone.NumCicliLunghi;
                _sbTemp.sbData.PuntLunghi = (uint)DatiClone.NumCicliLunghi;
                _sbTemp.sbData.ContBrevi = (uint)DatiClone.NumCicliBrevi;
                _sbTemp.sbData.PuntBrevi = (uint)DatiClone.NumCicliBrevi;

                Log.Warn(" ---------------------------------- Brevi -----------------------------------");
                //Log.Warn(FunzioniMR.hexdumpArray(DatiClone.CicliBrevi, true));
                Log.Warn(" ---------------------------------- Lunghi -----------------------------------");
                //Log.Warn(FunzioniMR.hexdumpArray(DatiClone.CicliLunghi, true));


                return true;
            }

            catch
            {
                return false;
            }
        }



        public bool EseguiClonazioneScheda()
        {
            try
            {
                byte[] _bufferScrittura = new byte[200];
                uint _startSlot = 0;
                bool _esito;
                int _numPacchetti = 0;
                int _step = 0; 

                uint _StartAddr;

                // step 0 Cliente

                _numPacchetti = 0;
                _step = DatiClone.DatiCliente.Length / 200;
                if ((DatiClone.DatiCliente.Length % 200) > 0)
                    _step++;
                

                if (DatiClone.DatiCliente.Length > 0)
                {
                    _StartAddr = 0x1000;
                    _startSlot = 0;
                    Log.Debug("---------------Dati Cliente-------------------");

                    for (int _ic = 0; _ic < 200; _ic++)
                    {
                        _bufferScrittura[_ic] = 0xFF;
                    }

                    uint _posizione = 0;

                    while ((_startSlot + _posizione) < DatiClone.DatiCliente.Length)
                    {
                        _bufferScrittura[_posizione] = DatiClone.DatiCliente[_startSlot + _posizione];
                        _posizione++;

                        if (_posizione >= 200)
                        {
                            //trasmetto i 200 bytes, vuoto il buffer e riparto
                            _esito = _sb.ScriviBloccoMemoria(_StartAddr + _startSlot, (ushort)_posizione, _bufferScrittura);
                            _numPacchetti++;
                            txtClonaPktCliente.Text = _numPacchetti.ToString() + "/" + _step.ToString();
                            Application.DoEvents();
                            _startSlot += _posizione;
                            _posizione = 0;
                            for (int _ic = 0; _ic < 200; _ic++)
                            {
                                _bufferScrittura[_ic] = 0xFF;
                            }
                        }

                    }
                    // Ora trasmetto il residuo
                    _esito = _sb.ScriviBloccoMemoria(_StartAddr + _startSlot, (ushort)_posizione, _bufferScrittura);
                    _numPacchetti++;
                    txtClonaPktCliente.Text = _numPacchetti.ToString();

                }


                // step 1 programmazioni

                _numPacchetti = 0;
                _step = DatiClone.Programmazioni.Length / 200;
                if ((DatiClone.Programmazioni.Length % 200) > 0)
                    _step++;


                if (DatiClone.NumeroProgrammazioni > 0)
                {
                    _StartAddr = 0x1400;
                    _startSlot = 0;
                    Log.Debug("---------------Programmazioni-------------------");
                    Log.Debug(DatiClone.Programmazioni.Length.ToString() + " bytes in " + DatiClone.NumeroProgrammazioni.ToString() + " programmazioni");

                    for( int _ic = 0; _ic< 200; _ic++)
                    {
                        _bufferScrittura[_ic] = 0xFF;
                    }

                    uint _posizione = 0;

                    while((_startSlot + _posizione ) < DatiClone.Programmazioni.Length)
                    {
                        _bufferScrittura[_posizione] = DatiClone.Programmazioni[_startSlot + _posizione];
                        _posizione++;

                        if(_posizione >= 200 )
                        {
                            //trasmetto i 200 bytes, vuoto il buffer e riparto
                            _esito = _sb.ScriviBloccoMemoria(_StartAddr + _startSlot, (ushort)_posizione, _bufferScrittura);
                            _numPacchetti++;
                            txtClonaPktProgr.Text = _numPacchetti.ToString() + "/" + _step.ToString();
                            Application.DoEvents();
                            _startSlot += _posizione;
                            _posizione = 0;
                            for (int _ic = 0; _ic < 200; _ic++)
                            {
                                _bufferScrittura[_ic] = 0xFF;
                            }
                        }

                    }
                    // Ora trasmetto il residuo
                    _esito = _sb.ScriviBloccoMemoria(_StartAddr + _startSlot, (ushort)_posizione, _bufferScrittura);
                    _numPacchetti++;
                    txtClonaPktProgr.Text = _numPacchetti.ToString() + "/" + _step.ToString();
                    Application.DoEvents();
                }

                // step 2 brevi

                _numPacchetti = 0;
                _step = DatiClone.CicliBrevi.Length / 200;
                if ((DatiClone.CicliBrevi.Length % 200) > 0)
                    _step++;


                if (DatiClone.NumCicliBrevi > 0)
                {
                    _StartAddr = 0x3000;
                    _startSlot = 0;
                    Log.Debug("---------------cicli brevi-------------------");
                    Log.Debug(DatiClone.CicliBrevi.Length.ToString() + " bytes in " + DatiClone.NumCicliBrevi.ToString() + " brevi");

                    for (int _ic = 0; _ic < 200; _ic++)
                    {
                        _bufferScrittura[_ic] = 0xFF;
                    }

                    uint _posizione = 0;

                    while ((_startSlot + _posizione) < DatiClone.CicliBrevi.Length)
                    {
                        _bufferScrittura[_posizione] = DatiClone.CicliBrevi[_startSlot + _posizione];
                        _posizione++;

                        if (_posizione >= 200)
                        {
                            //trasmetto i 200 bytes, vuoto il buffer e riparto
                            _esito = _sb.ScriviBloccoMemoria(_StartAddr + _startSlot, (ushort)_posizione, _bufferScrittura);
                            _numPacchetti++;
                            txtClonaPktBrevi.Text = _numPacchetti.ToString() + "/" + _step.ToString();
                            Application.DoEvents();
                            _startSlot += _posizione;
                            _posizione = 0;
                            for (int _ic = 0; _ic < 200; _ic++)
                            {
                                _bufferScrittura[_ic] = 0xFF;
                            }
                        }

                    }
                    // Ora trasmetto il residuo
                    _esito = _sb.ScriviBloccoMemoria(_StartAddr + _startSlot, (ushort)_posizione, _bufferScrittura);
                    _numPacchetti++;
                    txtClonaPktBrevi.Text = _numPacchetti.ToString() + "/" + _step.ToString();
                    Application.DoEvents();
                }

                // step 3 Lunghi

                _numPacchetti = 0;
                _step = DatiClone.CicliLunghi.Length / 200;
                if ((DatiClone.CicliLunghi.Length % 200) > 0)
                    _step++;

                if (DatiClone.NumCicliLunghi> 0)
                {
                    _StartAddr = 0x130000;
                    _startSlot = 0;
                    Log.Debug("---------------cicli lunghi-------------------");
                    Log.Debug(DatiClone.CicliLunghi.Length.ToString() + " bytes in " + DatiClone.NumCicliLunghi.ToString() + " lunghi");

                    for (int _ic = 0; _ic < 200; _ic++)
                    {
                        _bufferScrittura[_ic] = 0xFF;
                    }

                    uint _posizione = 0;

                    while ((_startSlot + _posizione) < DatiClone.CicliLunghi.Length)
                    {
                        _bufferScrittura[_posizione] = DatiClone.CicliLunghi[_startSlot + _posizione];
                        _posizione++;

                        if (_posizione >= 200)
                        {
                            //trasmetto i 200 bytes, vuoto il buffer e riparto
                            _esito = _sb.ScriviBloccoMemoria(_StartAddr + _startSlot, (ushort)_posizione, _bufferScrittura);
                            _numPacchetti++;
                            txtClonaPktLunghi.Text = _numPacchetti.ToString() + "/" + _step.ToString();
                            Application.DoEvents();
                            _startSlot += _posizione;
                            _posizione = 0;
                            for (int _ic = 0; _ic < 200; _ic++)
                            {
                                _bufferScrittura[_ic] = 0xFF;
                            }
                        }

                    }
                    // Ora trasmetto il residuo
                    _esito = _sb.ScriviBloccoMemoria(_StartAddr + _startSlot, (ushort)_posizione, _bufferScrittura);
                    _numPacchetti++;
                    txtClonaPktLunghi.Text = _numPacchetti.ToString() + "/" + _step.ToString();
                    Application.DoEvents();
                }


                //e alla fine la testata
                byte[] _tempHexData;
                _tempHexData = _sbTemp.sbData.DataArray;
                _esito = _sb.ScriviBloccoMemoria(0, 64, _tempHexData);


                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
