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
using Newtonsoft.Json;

namespace PannelloCharger
{
    public partial class frmSpyBat : Form
    {

        UnitaSpyBatt _sbTemp;  // classe sb usata come buffer temporaneo per le operazioni di clonatura
        CloneSB DatiClone;
        int _statoAppAttuale;

        ImageDump _ImmagineClone;
        MessaggioSpyBatt.comandoInizialeSB _NuovaIntestazioneSb { get; set; }

        bool _immagineValida = false;


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
                    Log.Warn(" ---------------------------------- Lungo " + _long.IdMemoriaLunga.ToString("000") +" -------------------------------");

                    Log.Warn(" ---------------------------------- Brevi -----------------------------------");

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
                    Log.Warn(" ---------------------------------- Lungo -----------------------------------");
                    DatiClone.AggiungiCicloLungo(_long);
                    Log.Warn(" ----------------------------------------------------------------------------");
                    Log.Warn("");


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

               // Log.Warn(" ---------------------------------- Brevi -----------------------------------");
               // Log.Warn(FunzioniMR.hexdumpArray(DatiClone.CicliBrevi, false));
               // Log.Warn(" ---------------------------------- Lunghi -----------------------------------");
               // Log.Warn(FunzioniMR.hexdumpArray(DatiClone.CicliLunghi, false));


                return true;
            }

            catch
            {
                return false;
            }
        }

        public bool InizializzaClonazioneScheda()
        {
           

            try
            {
                
                bool _esito;

                lbxClonaListaStep.Items.Clear();
                Application.DoEvents();

                lbxClonaListaStep.Items.Add("Verifica dati");
                if (_immagineValida != true)
                {
                    txtClonaStatoAttuale.Text = "Dati Non Pronti";
                    Application.DoEvents();
                    return false;
                }

                Application.DoEvents();
                // Mando la scheda in stato booltloader
                _statoAppAttuale = _sb.FirmwareAttivo();
                if (_statoAppAttuale<0)
                {
                    //Scheda non aggiornabile
                    lbxClonaListaStep.Items.Add("Stato Firmware " + _statoAppAttuale + ": Scheda non clonabile ");
                    return false;
                }

                lbxClonaListaStep.Items.Add("Stato Firmware: " + _statoAppAttuale );
                Application.DoEvents();

                if (_statoAppAttuale > 0)
                {
                    // Attiva una app --> vado in modo bl
                    txtClonaStatoAttuale.Text = "Switch to bootloader";
                    Application.DoEvents();
                    _esito = SwitchAreaBl(_sb.Id, _sb.apparatoPresente);

                    if (!_esito)
                    {
                        lbxClonaListaStep.Items.Add("Switch to bootloader fallito ");
                        Application.DoEvents();
                        return false;
                    }
                    else
                    {
                        lbxClonaListaStep.Items.Add("Switch to bootloader");
                        Application.DoEvents();
                    }

                }


                // Clear mem
                txtClonaStatoAttuale.Text = "Cancellazione intera memoria";

                Application.DoEvents();
                _esito = _sb.CancellaInteraMemoria();
                if (!_esito)
                {
                    lbxClonaListaStep.Items.Add("Cancellazione intera memoria fallita");
                    return false;
                }
                else
                {
                    lbxClonaListaStep.Items.Add("Cancellazione intera memoria");
                }
                Application.DoEvents();
                //Pulizia puntiale da 0x000000 a 0x1BBFFF

                uint _StartAddr;
                int _bloccoCorrente;
                ushort _NumBlocchi;

                _StartAddr = 0;
                _NumBlocchi = 476;
                lbxClonaListaStep.Items.Add("Cancellazione Puntuale memoria");
                lbxClonaListaStep.Items.Add("da 0x000000 a 0x1BBFFF");

                pgbCloneAvanzamento.Minimum = 0;
                pgbCloneAvanzamento.Maximum = _NumBlocchi;
                for (int _cicloBlocchi = 0; _cicloBlocchi < _NumBlocchi; _cicloBlocchi++)
                {
                    pgbCloneAvanzamento.Value = _cicloBlocchi;
                    _bloccoCorrente = _cicloBlocchi + 1;
                    _esito = _sb.CancellaBlocco4K(_StartAddr);
                    if (!_esito)
                    {
                        MessageBox.Show("Cancellazione del blocco " + _bloccoCorrente.ToString() + " non riuscita", "Cancellazione dati ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        _StartAddr += 0x1000;

                        txtClonaStatoAttuale.Text = "Cancellazione blocco memoria " + _bloccoCorrente.ToString() + "/476";
                        Application.DoEvents();
                    }

                }
                pgbCloneAvanzamento.Value = 0;
                lbxClonaListaStep.Items.Add("Cancellazione Completata");
                Application.DoEvents();


                return true;
            }
            catch ( Exception Ex)
            {
                Log.Error("SPYBATT InizializzaClonazioneScheda: " + Ex.Message);
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

                lbxClonaListaStep.Items.Add("Trasferimento Dati Cliente");
                Application.DoEvents();
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

                lbxClonaListaStep.Items.Add("Trasferimento Programmazioni");
                Application.DoEvents();

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
                            txtClonaStatoAttuale.Text = "Scrittuta Programmazioni " + _numPacchetti.ToString() + "/" + _step.ToString();
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
                    txtClonaStatoAttuale.Text = "Scrittuta Programmazioni " + _numPacchetti.ToString() + "/" + _step.ToString();
                    Application.DoEvents();
                }

                // step 2 brevi

                lbxClonaListaStep.Items.Add("Trasferimento cicli brevi");

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
                            txtClonaStatoAttuale.Text = "Scrittuta brevi " + _numPacchetti.ToString() + "/" + _step.ToString();
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
                    txtClonaStatoAttuale.Text = "Scrittuta brevi " + _numPacchetti.ToString() + "/" + _step.ToString();
                    Application.DoEvents();
                }

                // step 3 Lunghi
                lbxClonaListaStep.Items.Add("Trasferimento cicli lunghi");

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
                            txtClonaStatoAttuale.Text = "Scrittuta lunghi " + _numPacchetti.ToString() + "/" + _step.ToString();
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
                    txtClonaStatoAttuale.Text = "Scrittuta lunghi " + _numPacchetti.ToString() + "/" + _step.ToString();
                    Application.DoEvents();
                }


                //e alla fine la testata
                txtClonaStatoAttuale.Text = "Scrittuta testata ";
                Application.DoEvents();

                byte[] _tempHexData;
                _tempHexData = _sbTemp.sbData.DataArray;
                _esito = _sb.ScriviBloccoMemoria(0, 64, _tempHexData);

                lbxClonaListaStep.Items.Add("Trasferimento testata");
                lbxClonaListaStep.Items.Add("Operazione completata");
                lbxClonaListaStep.Items.Add("Riattivare l'APP corrente");



                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool EseguiClonazioneSchedaDaDump()
        {
            try
            {
                byte[] _bufferScrittura = new byte[200];
                uint _startSlot = 0;
                bool _esito;
                int _numPacchetti = 0;
                int _step = 0;

                uint _StartAddr = 0x000000;
                uint _EndAddr = 0x1BBFFF;

                // Se valida, sostituisco la testata
                if (_ImmagineClone.IntestazioneSb != null)
                {
                    for (int _ic = 0; _ic < _ImmagineClone.IntestazioneSb.dataBuffer.Length; _ic++)
                    {
                        _ImmagineClone.DataBuffer[_ic] = _ImmagineClone.IntestazioneSb.dataBuffer[_ic];
                    }
                }

                // step 0 Cliente
                //Copia puntiale da 0x000000 a 0x1BBFFF

                _numPacchetti = 0;
                _step = (int)(_EndAddr - _StartAddr) / 200;

                if (((_EndAddr - _StartAddr) % 200) > 0)
                    _step++;


                _startSlot = 0;
                Log.Debug("---------------Area Dati Dump Memoria-------------------");
               

                for (int _ic = 0; _ic < 200; _ic++)
                {
                    _bufferScrittura[_ic] = 0xFF;
                }

                uint _posizione = 0;
                pgbCloneAvanzamento.Maximum = _step;
                pgbCloneAvanzamento.Value = 0;
                while ((_startSlot + _posizione) < _EndAddr )
                {
                    _bufferScrittura[_posizione] = _ImmagineClone.DataBuffer[_startSlot + _posizione];
                    _posizione++;

                    if (_posizione >= 200)
                    {
                        //trasmetto i 200 bytes, vuoto il buffer e riparto
                        _esito = _sb.ScriviBloccoMemoria(_StartAddr + _startSlot, (ushort)_posizione, _bufferScrittura);
                        _numPacchetti++;
                        txtClonaStatoAttuale.Text = "Scrittuta dati " + _numPacchetti.ToString() + "/" + _step.ToString();
                        pgbCloneAvanzamento.Value = _numPacchetti;
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
                txtClonaStatoAttuale.Text = "Scrittuta dati " + _numPacchetti.ToString() + "/" + _step.ToString();
                pgbCloneAvanzamento.Value = _numPacchetti;
                



                lbxClonaListaStep.Items.Add("Trasferimento testata");
                lbxClonaListaStep.Items.Add("Operazione completata");
                lbxClonaListaStep.Items.Add("Riattivare l'APP corrente");
                Application.DoEvents();


                return true;
            }
            catch
            {
                return false;
            }
        }





        private bool importaHexdump()
        {
            try
            {

                string filePath = "";


                if (txtCloneFileImg.Text != "")
                {
                    filePath = txtCloneFileImg.Text;
                    if (File.Exists(filePath))
                    {
                        Log.Debug("Inizio Import");
                        string _fileImport = File.ReadAllText(filePath);
                        Log.Debug("file caricato: len = " + _fileImport.Length.ToString());
                        //sbDataModel _importData;
                        _ImmagineClone = JsonConvert.DeserializeObject<ImageDump>(_fileImport);

                        Log.Debug("file convertito");
                        //_sb.sbData = _ImmagineClone.Testata;

                        //_sb.ModelloDati = _importData;
                        //_sb.importaModello(_logiche.dbDati.connessione, true, true, true, true, true);
                        return MostraTestataHexDump();

                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    //                    MessageBox.Show("Inserire un nome valido", "Importazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(StringheComuni.InserireNome, StringheComuni.ImportaDati, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            catch (Exception Ex)
            {
                //                MessageBox.Show("Dati non validi", "Importazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(StringheComuni.DatiNonValidi, StringheComuni.ImportaDati, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("MostraDati: " + Ex.Message);
                return false;

            }
        }


        public bool MostraTestataHexDump()
        {
            try
            {
                bool _esito = false;
                txtCloneIdSB.BackColor = Color.White;
                txtCloneIdSB.Text = FunzioniMR.StringaSeriale(_ImmagineClone.Testata.Id);
                txtCloneFwVersion.BackColor = Color.White;
                txtCloneFwVersion.Text = _ImmagineClone.Testata.SwVersion;
                if (_ImmagineClone.Testata.SwVersion == _sb.sbData.SwVersion)
                {
                    txtCloneFwVersion.ForeColor = Color.Black;

                    _esito = true;
                }
                else
                {
                    txtCloneFwVersion.ForeColor = Color.Red;
                    txtCloneFwVersion.BackColor = Color.Yellow;
                    _esito = false;
                }
                txtCloneNumLunghi.BackColor = Color.White;
                txtCloneNumLunghi.Text = _ImmagineClone.Testata.LongMem.ToString() + " / " + _ImmagineClone.Testata.LongMem.ToString("X6") + "  ( Prg: " + _ImmagineClone.Testata.ProgramCount.ToString() + " )";
                txtCloneNote.BackColor = Color.White;
                txtCloneNote.Text = _ImmagineClone.Timestamp.ToString();


                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("MostraDati: " + Ex.Message);
                return false;
            }
        }

    }
}
