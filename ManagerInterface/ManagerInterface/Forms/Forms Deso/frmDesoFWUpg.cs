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

        FirmwareManager _firmMng = new FirmwareManager();


        public void CaricafileCCS()
        {
            FirmwareManager.ExitCode _esito;
            try
            {
                txtFWInFileAddrN1.Text = "";
                txtFWInFileAddrN2.Text = "";
                txtFWInFileAddrP.Text = "";

                txtFWInFileLenN1.Text = "";
                txtFWInFileLenN2.Text = "";
                txtFWInFileLenP.Text = "";

                _esito = _firmMng.CaricaFileCCS(txtFwFileCCS.Text);
                if(_esito == FirmwareManager.ExitCode.OK)
                {
                    btnFWFilePubSave.Enabled = true;
                    txtFWInFileAddrN1.Text = _firmMng.FirmwareData.AddrFlash1.ToString("X4");
                    txtFWInFileAddrN2.Text = _firmMng.FirmwareData.AddrFlash2.ToString("X4");
                    txtFWInFileAddrP.Text = _firmMng.FirmwareData.AddrProxy.ToString("X4");

                    txtFWInFileLenN1.Text = _firmMng.FirmwareData.LenFlash1.ToString();
                    txtFWInFileLenN2.Text = _firmMng.FirmwareData.LenFlash2.ToString();
                    txtFWInFileLenP.Text = _firmMng.FirmwareData.LenProxy.ToString();
                }




            }
            catch
            {

            }
        }


        public bool SalvaFileSBF()
        {
            try
            {
                if (txtFWFileSBFwr.Text != "")
                {
                    if (_firmMng.FirmwareData.DatiOK)
                    {
                        FirmwareManager.ExitCode _esito = FirmwareManager.ExitCode.ErroreGenerico;
                        _esito = _firmMng.GeneraFileSBF(txtFWInFileRev.Text, "", txtFWInFileRevData.Text, txtFWFileSBFwr.Text, false);

                        if(_esito == FirmwareManager.ExitCode.OK)
                        {
                            MessageBox.Show("File generato", "Esportazione pacchetto Firmware", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("File non generato\r\nErroe generale", "Esportazione pacchetto Firmware", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
                return false;
            }

            catch
            {
                return false;
            }
        }



        public void CaricafileSBF()
        {
            FirmwareManager.ExitCode _esito;
            try
            {
                txtFWInSBFRev.Text = "";
                txtFWInSBFDtRev.Text = "";
                txtFWTxFileLenN1.Text = "";
                txtFWTxFileLenN2.Text = "";
                txtFWTxFileLenP.Text = "";
                txtFWTxDataLenN1.Text = "";
                txtFWTxDataLenN2.Text = "";
                txtFWTxDataLenP.Text = "";
                btnFWPreparaTrasmissione.Enabled = false;

                if (txtFWFileSBFrd.Text == "")
                {
                    //messagebox
                    return;
                }

                _esito = _firmMng.CaricaFileSBF(txtFWFileSBFrd.Text);
                if (_esito == FirmwareManager.ExitCode.OK)
                {

                    txtFWInSBFRev.Text = _firmMng.FirmwareData.Release;
                    txtFWInSBFDtRev.Text = FunzioniMR.StringaDataTS(_firmMng.FirmwareData.ReleaseDateBlock);
                    btnFWPreparaTrasmissione.Enabled = true;

                    txtFWTxFileLenN1.Text = _firmMng.FirmwareData.LenFlash1.ToString();
                    txtFWTxFileLenN2.Text = _firmMng.FirmwareData.LenFlash2.ToString();
                    txtFWTxFileLenP.Text = _firmMng.FirmwareData.LenProxy.ToString();


                }
            }
            catch
            {

            }
        }


        private void MostraStato(FirmwareManager.MascheraStato Valore, byte Stato, ref TextBox Cella, bool KOifFalse = false )
        {
            try
            {
                bool _esitocella = false;

                _esitocella = ( Stato & (byte)Valore ) == (byte)Valore ;

                if (_esitocella)
                {
                    Cella.ForeColor = Color.Green;
                    Cella.Text = "OK";
                }
                else
                {
                    if (KOifFalse)
                    {
                        Cella.ForeColor = Color.Black;
                        Cella.Text = "";
                    }
                    else
                    {
                        Cella.ForeColor = Color.Red;
                        Cella.Text = "KO";
                    }
                }

            }
            catch
            {

            }
        }

        public bool CaricaStatoFirmware(ref string IdApparato, LogicheBase Logiche, bool SerialeCollegata)
        {
            bool _esito = false;
            bool _esitoFunzione = false;
            try
            {
                txtFwRevBootloader.Text = "...";
                txtFwRevFirmware.Text = "";
                txtFwStatoMicro.Text = "";
                txtFwStatoHA1.Text = "";
                txtFwStatoHA2.Text = "";
                txtFwStatoSA1.Text = "";
                txtFwStatoSA2.Text = "";
                txtFwAreaTestata.Text = "";
                Log.Debug("----------------------- CaricaStatoFirmware ---------------------------");


                // _esito = caricaDati(IdApparato, Logiche, SerialeCollegata);
                _esito = ApriComunicazione(IdApparato, Logiche, SerialeCollegata);

                if (_esito)
                {
                    IdApparato = _sb.Id;

                    _esito = _sb.CaricaStatoFirmware(IdApparato, SerialeCollegata);
                    if (_esito && (_sb.UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk))
                    {

                        txtFwRevBootloader.Text = _sb.StatoFirmware.strRevBootloader;
                        txtFwRevFirmware.Text = _sb.StatoFirmware.strRevFirmware;

                        MostraStato(FirmwareManager.MascheraStato.Blocco1HW, _sb.StatoFirmware.Stato, ref txtFwStatoHA1, true);
                        MostraStato(FirmwareManager.MascheraStato.Blocco2HW, _sb.StatoFirmware.Stato, ref txtFwStatoHA2, true);
                        MostraStato(FirmwareManager.MascheraStato.Blocco1SW, _sb.StatoFirmware.Stato, ref txtFwStatoSA1, false);
                        MostraStato(FirmwareManager.MascheraStato.Blocco2SW, _sb.StatoFirmware.Stato, ref txtFwStatoSA2, false);
                        MostraStato(FirmwareManager.MascheraStato.FlashmPHW, _sb.StatoFirmware.Stato, ref txtFwStatoMicro, true);

                        _esitoFunzione = true;

                        // Verifico quale blocco è attualmente caricato sul micro

                        if ((_sb.StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.BootLoaderInUso) == (byte)FirmwareManager.MascheraStato.BootLoaderInUso)
                        {
                            txtFwAreaTestata.Text = "BL";
                        }
                        else
                        {
                            bool _esitoMicro1 = false;
                            _esitoMicro1 = (_sb.StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.Blocco1InUso) == (byte)FirmwareManager.MascheraStato.Blocco1InUso;
                            bool _esitoMicro2 = false;
                            _esitoMicro2 = (_sb.StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.Blocco2InUso) == (byte)FirmwareManager.MascheraStato.Blocco2InUso;


                            if (_esitoMicro1)
                            {
                                txtFwAreaTestata.Text = "A1";
                            }
                            else
                            {
                                if (_esitoMicro2)
                                {
                                    txtFwAreaTestata.Text = "A2";
                                }

                            }
                        }
                    }

                }
              
                return _esitoFunzione;

            }
            catch
            {
                return _esitoFunzione;
            }
        }

        public bool CaricaStatoAreaFw(byte IdArea, byte StatoFirmware)
        {
            bool _esito = false;
            byte[] _bufferDati = new byte[64];
            FirmwareManager _tempFW = new FirmwareManager();
            FirmwareManager.ExitCode _esitoFW = FirmwareManager.ExitCode.ErroreGenerico;
            uint _area;

            try
            {

                Log.Info("Lettura area FW 1 ");
                if (IdArea == 1)
                {

                    txtFwRevA1State.Text = "KO";
                    txtFwRevA1State.ForeColor = Color.Red;
                    txtFwRevA1RevFw.Text = "";
                    txtFwRevA1RilFw.Text = "";
                    txtFWRevA1AddrN1.Text = "";
                    txtFWRevA1LenN1.Text = "";
                    txtFWRevA1AddrN2.Text = "";
                    txtFWRevA1LenN2.Text = "";
                    txtFWRevA1AddrP.Text = "";
                    txtFWRevA1LenP.Text = "";
                    _area = 0x1C0000;
                }
                else
                {
                    txtFwRevA2State.Text = "KO";
                    txtFwRevA2State.ForeColor = Color.Red;
                    txtFwRevA2RevFw.Text = "";
                    txtFwRevA2RilFw.Text = "";
                    txtFWRevA2AddrN1.Text = "";
                    txtFWRevA2LenN1.Text = "";
                    txtFWRevA2AddrN2.Text = "";
                    txtFWRevA2LenN2.Text = "";
                    txtFWRevA2AddrP.Text = "";
                    txtFWRevA2LenP.Text = "";
                    _area = 0x1E0000;

                }


                _esito = _sb.LeggiBloccoMemoria(_area, 64, out _bufferDati);


                if (_esito)
                {
                    _esitoFW = _tempFW.AnalizzaArrayTestata(_bufferDati);
                    if (_esitoFW == FirmwareManager.ExitCode.OK && _tempFW.FirmwareBlock.TestataOK)
                    {
                        if (IdArea == 1)
                        {
                            txtFwRevA1State.Text = "OK";
                            txtFwRevA1State.ForeColor = Color.Black;
                            txtFwRevA1RevFw.Text = _tempFW.FirmwareBlock.Release;
                            txtFwRevA1RilFw.Text = _tempFW.FirmwareBlock.ReleaseDate;
                            txtFWRevA1AddrN1.Text = _tempFW.FirmwareBlock.AddrFlash1.ToString();
                            txtFWRevA1LenN1.Text = _tempFW.FirmwareBlock.LenFlash1.ToString();
                            txtFWRevA1AddrN2.Text = _tempFW.FirmwareBlock.AddrFlash2.ToString();
                            txtFWRevA1LenN2.Text = _tempFW.FirmwareBlock.LenFlash2.ToString();
                            txtFWRevA1AddrP.Text = _tempFW.FirmwareBlock.AddrProxy.ToString();
                            txtFWRevA1LenP.Text = _tempFW.FirmwareBlock.LenProxy.ToString();
                        }
                        else
                        {
                            txtFwRevA2State.Text = "OK";
                            txtFwRevA2State.ForeColor = Color.Black;
                            txtFwRevA2RevFw.Text = _tempFW.FirmwareBlock.Release;
                            txtFwRevA2RilFw.Text = _tempFW.FirmwareBlock.ReleaseDate;
                            txtFWRevA2AddrN1.Text = _tempFW.FirmwareBlock.AddrFlash1.ToString();
                            txtFWRevA2LenN1.Text = _tempFW.FirmwareBlock.LenFlash1.ToString();
                            txtFWRevA2AddrN2.Text = _tempFW.FirmwareBlock.AddrFlash2.ToString();
                            txtFWRevA2LenN2.Text = _tempFW.FirmwareBlock.LenFlash2.ToString();
                            txtFWRevA2AddrP.Text = _tempFW.FirmwareBlock.AddrProxy.ToString();
                            txtFWRevA2LenP.Text = _tempFW.FirmwareBlock.LenProxy.ToString();
                        }
                    }

                }

                return _esito;

            }
            catch
            {
                return _esito;
            }
        }

        public bool SwitchAreaFw(string IdApparato, bool SerialeCollegata, byte IdArea)
        {
            bool _esito = false;
            byte[] _bufferDati = new byte[64];
            uint _area;

            try
            {


                _esito = _sb.SwitchFirmware( IdApparato, SerialeCollegata, IdArea);


                if (_esito)
                {

                }

                return _esito;

            }
            catch
            {
                return _esito;
            }
        }


        public bool SwitchAreaBl(string IdApparato, bool SerialeCollegata)
        {
            bool _esito = false;
            byte[] _bufferDati = new byte[64];
            uint _area;

            try
            {


                _esito = _sb.SwitchToBootLoader(IdApparato, SerialeCollegata);


                if (_esito)
                {

                }

                return _esito;

            }
            catch
            {
                return _esito;
            }
        }




        public void PreparaTrasmissioneFW()
        {
            FirmwareManager.ExitCode _esito;
            try
            {
                txtFWTxDataLenN1.Text = "";
                txtFWTxDataLenN2.Text = "";
                txtFWTxDataLenP.Text = "";

                txtFWTxDataAddrN1.Text = "";
                txtFWTxDataAddrN2.Text = "";
                txtFWTxDataAddrP.Text = "";

                txtFWTxDataNumN1.Text = "";
                txtFWTxDataNumN2.Text = "";
                txtFWTxDataNumP.Text = "";
                txtFWTxDataNumTot.Text = "";

                _esito = _firmMng.PreparaUpgradeFw();
                if (_esito == FirmwareManager.ExitCode.OK)
                {
                    _esito = _firmMng.ComponiArrayTestata();
                    if (_esito == FirmwareManager.ExitCode.OK)
                    {
                        txtFWTxDataLenN1.Text = _firmMng.FirmwareBlock.LenFlash1.ToString();
                        txtFWTxDataLenN2.Text = _firmMng.FirmwareBlock.LenFlash2.ToString();
                        txtFWTxDataLenP.Text = _firmMng.FirmwareBlock.LenProxy.ToString();

                        txtFWTxDataNumN1.Text = _firmMng.FirmwareBlock.ListaFlash1.Count().ToString();
                        txtFWTxDataNumN2.Text = _firmMng.FirmwareBlock.ListaFlash2.Count().ToString();
                        txtFWTxDataNumP.Text = _firmMng.FirmwareBlock.ListaProxy.Count().ToString();
                        txtFWTxDataNumTot.Text = _firmMng.FirmwareBlock.TotaleBlocchi.ToString();

                        txtFWTxDataAddrN1.Text = "0x" +  _firmMng.FirmwareBlock.AddrFlash1.ToString("X4");
                        txtFWTxDataAddrN2.Text = "0x" + _firmMng.FirmwareBlock.AddrFlash2.ToString("X4");
                        txtFWTxDataAddrP.Text = "0x" + _firmMng.FirmwareBlock.AddrProxy.ToString("X4");
                        Log.Error("Teststa FW: "+ FunzioniMR.hexdumpArray(_firmMng.FirmwareBlock.MessaggioTestata) );

                        btnFWLanciaTrasmissione.Enabled = true;
                    }

                }



            }
            catch
            {

            }
        }


        private void AggiornaFirmware(bool InviaACK = false)
        {
            try
            {
                Log.Debug("Lancio aggiornamento firmware");
                // verifico se ho caricato i dati

                if (!_firmMng.FirmwareBlock.TestataOK)
                {
                    // La testata non è pronta: esco

                    // Aggiungere messagebox
                    Log.Warn("Tentato aggiornamento firmware con dati non pronti ");

                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                byte _area;
                byte.TryParse(txtFWSBFArea.Text, out _area);
                if (_area != 2)
                    _area = 1;

                txtFWSBFArea.Text = _area.ToString();
                _avCicli.ParametriWorker.MainCount = 100;

                _avCicli.sbLocale = _sb;
                _avCicli.FirmwareBlock = _firmMng.FirmwareBlock;
                _avCicli.FirmwareArea = _area;
                _avCicli.ValStart = 1;
                _avCicli.ValFine = (int)_sb.sbData.LongMem;
                _avCicli.DbDati = _logiche.dbDati.connessione;
                _avCicli.CaricaBrevi = false;
                _avCicli.TipoComando = elementiComuni.tipoMessaggio.AggiornamentoFirmware;
                _avCicli.InviaACK = InviaACK;
                _avCicli.SalvaHexDump = false;
                _avCicli.FileHexDump = "";
                _avCicli.Text = "Aggiornamento Firmware";
                Log.Debug("FRM firmwareUPD : ");


                // Apro il form con le progressbar

                _avCicli.ShowDialog(this);

                this.Cursor = Cursors.Default;
            }

            catch (Exception Ex)
            {
                Log.Error("frmSpyBat.DumpInteraMemoria: " + Ex.Message);

            }
        }



        public bool CaricaSbOrigine(string IdApparato, MoriData._db dbCorrente, bool ApparatoConnesso = false)
        {
            bool _esito = false;
            bool _esitoDati = false;
            try
            {
                parametriSistema _tmpParametri = new parametriSistema();

                _sbTemp = new UnitaSpyBatt(ref _tmpParametri, dbCorrente, _logiche.currentUser.livello);
                _esitoDati = _sbTemp.CaricaCompleto(IdApparato, dbCorrente, ApparatoConnesso);

                _esito = _esitoDati;


                return _esito;
            }
            catch
            {
                return _esito;
            }

        }

        private bool RiscriviTestata(byte[] _tempData)
        {
            bool _esito = false;

            try
            {
                int _lunghezzaValida;
                int _scartati;
                uint _StartAddr = 0;
                ushort _NumByte = 0x80;
                


                _lunghezzaValida = _tempData.Length; 

                if (_lunghezzaValida > 0)
                {
                    _esito = _sb.ScriviBloccoMemoria(_StartAddr, (ushort)_lunghezzaValida, _tempData);

                }
                return _esito;

            }
            catch (Exception Ex)
            {
                Log.Error("VerificaWriteMem: " + Ex.Message);
                return _esito;
            }

        }


    }

}
