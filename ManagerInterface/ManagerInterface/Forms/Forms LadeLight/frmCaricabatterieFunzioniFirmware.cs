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
using System.Threading;

namespace PannelloCharger
{
    public partial class frmCaricabatterie : Form

    {
        FirmwareLLManager _firmMng = new FirmwareLLManager();

        private void InizializzaVistaStrutturaAreeCCS()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 7, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                lvwFWInFileStruct.HeaderUsesThemes = false;
                lvwFWInFileStruct.HeaderFormatStyle = _stile;
                lvwFWInFileStruct.UseAlternatingBackColors = true;
                lvwFWInFileStruct.AlternateRowBackColor = Color.LightGoldenrodYellow;

                lvwFWInFileStruct.AllColumns.Clear();

                lvwFWInFileStruct.View = View.Details;
                lvwFWInFileStruct.ShowGroups = false;
                lvwFWInFileStruct.GridLines = true;

                OLVColumn NumArea = new OLVColumn()
                {
                    Text = "Area",
                    ToolTipText = "Area di memoria",
                    AspectName = "strNumArea",
                    Sortable = false,
                    Width = 40,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    IsVisible = true,
                };
                lvwFWInFileStruct.AllColumns.Add(NumArea);

                OLVColumn AddressArea = new OLVColumn()
                {
                    Text = "Addr",
                    ToolTipText = "Indirizzo Area di memoria",
                    AspectName = "strAddrDestPacchetto",
                    Sortable = false,
                    Width = 80,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    IsVisible = true,
                };
                lvwFWInFileStruct.AllColumns.Add(AddressArea);


                OLVColumn DimArea = new OLVColumn()
                {
                    Text = "Dim",
                    ToolTipText = "Dimensione Area di memoria",
                    AspectName = "strNumBytes",
                    Sortable = false,
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Right,
                    IsVisible = true,
                };
                lvwFWInFileStruct.AllColumns.Add(DimArea);


                //-------------------------------------------- 


                OLVColumn colRowFiller = new OLVColumn()
                {
                    Text = "",
                    Sortable = false,
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    FillsFreeSpace = true,
                    IsVisible = true,
                };

                lvwFWInFileStruct.AllColumns.Add(colRowFiller);

                lvwFWInFileStruct.RebuildColumns();

                lvwFWInFileStruct.SetObjects(ListaAreeCCS);
                lvwFWInFileStruct.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }


        /// <summary>
        /// Carico il binario generato da CCS, unisco i due files (.hex e .a01)  e verifico la correttezza formale degli stessi
        /// </summary>
        public void CaricafileLLCCS()
        {



            FirmwareLLManager.ExitCode _esito;
            try
            {
                txtFWInFileStruct.Text = "";
                btnFWLanciaTrasmissione.Enabled = false;
                txtFWInLLFEsito.Text = "";
                ListaAreeCCS.Clear();  

                _esito = _firmMng.CaricaFileCCS(txtFwFileCCS.Text);

                if (_esito == FirmwareLLManager.ExitCode.OK)
                {

                    int NumArea = 0;
                    foreach (AreaDatiFWLL _area in _firmMng.FirmwareData.ListaAree)
                    {
                        ParametriArea _tempPar = new ParametriArea();
                        _tempPar.NumArea = NumArea++;
                        _tempPar.NumBytes = (int)_area.DimDati;
                        _tempPar.AddrDestPacchetto = _area.AddrDestPacchetto;
                        _tempPar.NumPacchetti = 0;
                        ListaAreeCCS.Add(_tempPar);

                    }
                    InizializzaVistaStrutturaAreeCCS();

                    btnFWFilePubSave.Enabled = true;

                    txtFWInFileStruct.Text = _firmMng.FirmwareData.ListaAree.Count().ToString();
                }

    


            }
            catch
            {

            }
        }

        public bool ControllaNomiFilesCCSLL(string FileBase)
        {
            try
            {
                string _fileExt = "";
                string _fileA01 = "";
                string _fileHEX = "";

                bool _a01OK = false;
                bool _hexOK = false;

                if (File.Exists(FileBase))
                {
                    // Verifico che esista anche il file associato
                    _fileExt = Path.GetExtension(FileBase);


                    // se ho verificati la presenza dell' A01, controllo l'HEX
                    _fileA01 = Path.ChangeExtension(FileBase, ".a01");
                    _fileHEX = Path.ChangeExtension(FileBase, ".hex");

                    txtFwFileCCShex.Text = _fileHEX;
                    txtFwFileCCShex.ForeColor = Color.Red;
                    txtFwFileCCSa01.Text = _fileA01;
                    txtFwFileCCSa01.ForeColor = Color.Red;


                    if (File.Exists(_fileA01))
                    {
                        txtFwFileCCSa01.ForeColor = Color.Black;
                        _a01OK = true;
                    }

                    if (File.Exists(_fileHEX))
                    {
                        txtFwFileCCShex.ForeColor = Color.Black;
                        _hexOK = true;
                    }
                }

                return _a01OK && _hexOK;

            }

            catch (Exception)
            {
                return false;
            }
        }

        public bool SalvaFileLLF()
        {
            try
            {
                if (txtFWFileLLFwr.Text != "")
                {
                    if (_firmMng.FirmwareData.DatiOK)
                    {
                        FirmwareLLManager.ExitCode _esito = FirmwareLLManager.ExitCode.ErroreGenerico;
                        _esito = _firmMng.GeneraFileLLF(txtFWInFileRev.Text, txtFWLibInFileRev.Text, txtFWInFileRevData.Text, txtFWFileLLFwr.Text, false);

                        if (_esito == FirmwareLLManager.ExitCode.OK)
                        {
                            MessageBox.Show("File generato", "Esportazione pacchetto Firmware", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("File non generato\r\nErrore generale", "Esportazione pacchetto Firmware", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        public bool CaricafileLLF()
        {
            FirmwareLLManager.ExitCode _esito;
            bool _esitoBool;
            try
            {

                btnFWPreparaTrasmissione.Enabled = false;

                if (txtFWFileSBFrd.Text == "")
                {
                    //messagebox
                    return false;
                }

                _esito = _firmMng.CaricaFileLLF(txtFWFileSBFrd.Text);
                if (_esito == FirmwareLLManager.ExitCode.OK)
                {

                    txtFWInLLFRev.Text = _firmMng.FirmwareData.Release;
                    txtFWInLLFDispRev.Text = _firmMng.FirmwareData.DisplayRelease;

                    txtFWInSBFDtRev.Text = FunzioniMR.StringaDataTS(_firmMng.FirmwareData.ReleaseDateBlock);
                    ListaAreeLLF.Clear();
                    int NumArea = 0;
                    foreach(AreaDatiFWLL _area in _firmMng.FirmwareData.ListaAree)
                    {
                        ParametriArea _tempPar = new ParametriArea();
                        _tempPar.NumArea = NumArea++;
                        _tempPar.NumBytes = (int)_area.DimDati;
                        _tempPar.AddrDestPacchetto = _area.AddrDestPacchetto;
                        _tempPar.NumPacchetti = 0;
                        ListaAreeLLF.Add(_tempPar);

                    }
                    InizializzaVistaListaAreeLLF();



                    // verifico che il firmware sia accettabile, rileggendo la versione BL e FW sulla scheda
                    /*
                    _esitoBool = _sb.CaricaStatoFirmware(_sb.Id, true); /// SerialeCollegata);
                    if (_esitoBool && (_sb.UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk))
                    {
                        _esitoBool = _firmMng.VersioneAmmessa(_firmMng.FirmwareData.Release, _sb.sbData.HwVersion.ToString(), _sb.StatoFirmware.strRevBootloader);
                    }

                    if (!_esitoBool)

                    {
                        MessageBox.Show(StringheMessaggio.strFirmwareNonValido, "Firmware", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    */
                    btnFWPreparaTrasmissione.Enabled = true;


                    return true;

                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void PreparaTrasmissioneFW()
        {
            FirmwareLLManager.ExitCode _esito;
            try
            {

                btnFWLanciaTrasmissione.Enabled = false;
                txtFWInLLFEsito.Text = "";
                _esito = _firmMng.PreparaUpgradeFw();
                if (_esito == FirmwareLLManager.ExitCode.OK)
                {
                    _esito = _firmMng.ComponiArrayTestata();
                    if (_esito == FirmwareLLManager.ExitCode.OK)
                    {

                        ListaAreeLLF.Clear();
                        int NumArea = 0;
                        foreach (AreaDatiFWLL _area in _firmMng.FirmwareBlock.ListaAree)
                        {
                            ParametriArea _tempPar = new ParametriArea();
                            _tempPar.NumArea = NumArea++;
                            _tempPar.NumBytes = (int)_area.DimDati;
                            _tempPar.AddrDestPacchetto = _area.AddrDestPacchetto;
                            _tempPar.NumPacchetti =(int)_area.NumeroPacchetti;

                            ListaAreeLLF.Add(_tempPar);

                        }
                  
                        flwFWFileLLFStruct.SetObjects(ListaAreeLLF);
                        flwFWFileLLFStruct.BuildList();
                        txtFWInLLFEsito.Text = "FW Pronto";
                        btnFWLanciaTrasmissione.Enabled = true;
                    }

                }
                else
                {
                    txtFWInLLFEsito.Text = "Non Valido";
                }



            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat.DumpInteraMemoria: " + Ex.Message);

            }

        }

        private void InizializzaVistaListaAreeLLF()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 7, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flwFWFileLLFStruct.HeaderUsesThemes = false;
                flwFWFileLLFStruct.HeaderFormatStyle = _stile;
                flwFWFileLLFStruct.UseAlternatingBackColors = true;
                flwFWFileLLFStruct.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flwFWFileLLFStruct.AllColumns.Clear();

                flwFWFileLLFStruct.View = View.Details;
                flwFWFileLLFStruct.ShowGroups = false;
                flwFWFileLLFStruct.GridLines = true;

                OLVColumn NumArea = new OLVColumn()
                {
                    Text = "Area",
                    ToolTipText = "Area di memoria",
                    AspectName = "strNumArea",
                    Sortable = false,
                    Width = 40,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    IsVisible = true,
                };
                flwFWFileLLFStruct.AllColumns.Add(NumArea);

                OLVColumn AddressArea = new OLVColumn()
                {
                    Text = "Addr",
                    ToolTipText = "Indirizzo Area di memoria",
                    AspectName = "strAddrDestPacchetto",
                    Sortable = false,
                    Width = 80,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    IsVisible = true,
                };
                flwFWFileLLFStruct.AllColumns.Add(AddressArea);


                OLVColumn DimArea = new OLVColumn()
                {
                    Text = "Dim",
                    ToolTipText = "Dimensione Area di memoria",
                    AspectName = "strNumBytes",
                    Sortable = false,
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Right,
                    IsVisible = true,
                };
                flwFWFileLLFStruct.AllColumns.Add(DimArea);

                OLVColumn NumPacchetti = new OLVColumn()
                {
                    Text = "Pkt",
                    ToolTipText = "Numero Pacchetti",
                    AspectName = "strNumPacchetti",
                    Sortable = false,
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Right,
                    IsVisible = true,
                };
                flwFWFileLLFStruct.AllColumns.Add(NumPacchetti);


                //-------------------------------------------- 


                OLVColumn colRowFiller = new OLVColumn()
                {
                    Text = "",
                    Sortable = false,
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    FillsFreeSpace = true,
                    IsVisible = true,
                };

                flwFWFileLLFStruct.AllColumns.Add(colRowFiller);

                flwFWFileLLFStruct.RebuildColumns();

                flwFWFileLLFStruct.SetObjects(ListaAreeLLF);
                flwFWFileLLFStruct.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }

        private void AggiornaFirmware(bool InviaACK = false)
        {
            try
            {
                bool _esito;
                int _tentativi;

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
                byte _area = (byte)(cmbFWSBFArea.SelectedIndex + 1);
                /*
                byte.TryParse(txtFWSBFArea.Text, out _area);
                if (_area != 2)
                    _area = 1;

                txtFWSBFArea.Text = _area.ToString();
                */
                _avCicli.ParametriWorker.MainCount = 100;

                _avCicli.ElementoPilotato = frmAvanzamentoCicli.ControlledDevice.LadeLight;
                _avCicli.llLocale = _cb;
                _avCicli.FirmwareLLBlock = _firmMng.FirmwareBlock;
                _avCicli.FirmwareArea = _area;
                _avCicli.ValStart = 1;
                _avCicli.ValFine = 0;
                _avCicli.DbDati = null; // _logiche.dbDati.connessione;
                _avCicli.CaricaBrevi = false;
                _avCicli.TipoComando = elementiComuni.tipoMessaggio.AggiornamentoFirmwareLL;
                _avCicli.InviaACK = InviaACK;
                _avCicli.SalvaHexDump = false;
                _avCicli.FileHexDump = "";
                _avCicli.Text = StringheMessaggio.strMsgAggiornamentoFirmware; // "Aggiornamento Firmware";

                Log.Debug("FRM firmwareUPD : ");

                // Apro il form con le progressbar
                _avCicli.ShowDialog(this);

                // aspetto 5 secondi poi mi ricollego
                Application.DoEvents();
                _esito = false;
                _tentativi = 0;
                while (!_esito)
                {
                    System.Threading.Thread.Sleep(1000);
                    Application.DoEvents();
                    _tentativi++;
                    _esito = _cb.VerificaPresenza();
                    Application.DoEvents();
                }



                this.Cursor = Cursors.Default;
            }

            catch (Exception Ex)
            {
                Log.Error("frmSpyBat.DumpInteraMemoria: " + Ex.Message);

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
                txtFwRevDisplay.Text = "";
                txtFwStatoMicro.Text = "";
                txtFwStatoHA1.Text = "";
                txtFwStatoHA2.Text = "";
                txtFwStatoSA1.Text = "";
                txtFwStatoSA2.Text = "";
                txtFwAreaTestata.Text = "";
                btnFwSwitchBL.BackColor = Color.LightGray;
                btnFwSwitchArea1.BackColor = Color.LightGray;
                btnFwSwitchArea2.BackColor = Color.LightGray;
                btnFwSwitchArea1.Enabled = false;
                btnFwSwitchArea2.Enabled = false;


                Log.Debug("----------------------- CaricaStatoFirmware ---------------------------");


                // _esito = caricaDati(IdApparato, Logiche, SerialeCollegata);
                _esito = _cb.apriPorta();

                if (_esito)
                {
                    IdApparato = ""; // _sb.Id;

                    _esito = _cb.CaricaStatoFirmware(IdApparato, SerialeCollegata);

                    if (_esito && (_cb.UltimaRisposta == SerialMessage.EsitoRisposta.MessaggioOk))
                    {

                        txtFwRevBootloader.Text = _cb.StatoFirmware.strRevBootloader;
                        txtFwRevFirmware.Text = _cb.StatoFirmware.strRevFirmware;
                        txtFwRevDisplay.Text = _cb.StatoFirmware.strRevDisplay;

                        MostraStato(FirmwareManager.MascheraStato.Blocco1HW, _cb.StatoFirmware.Stato, ref txtFwStatoHA1, true);
                        MostraStato(FirmwareManager.MascheraStato.Blocco2HW, _cb.StatoFirmware.Stato, ref txtFwStatoHA2, true);
                        if (MostraStato(FirmwareManager.MascheraStato.Blocco1SW, _cb.StatoFirmware.Stato, ref txtFwStatoSA1, false)) btnFwSwitchArea1.Enabled = true;
                        if (MostraStato(FirmwareManager.MascheraStato.Blocco2SW, _cb.StatoFirmware.Stato, ref txtFwStatoSA2, false)) btnFwSwitchArea2.Enabled = true;
                        MostraStato(FirmwareManager.MascheraStato.FlashmPHW, _cb.StatoFirmware.Stato, ref txtFwStatoMicro, true);

                        _esitoFunzione = true;

                        // Verifico quale blocco è attualmente caricato sul micro

                        if ((_cb.StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.BootLoaderInUso) == (byte)FirmwareManager.MascheraStato.BootLoaderInUso)
                        {
                            txtFwAreaTestata.Text = "BL";
                            btnFwSwitchBL.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            bool _esitoMicro1 = false;
                            _esitoMicro1 = (_cb.StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.Blocco1InUso) == (byte)FirmwareManager.MascheraStato.Blocco1InUso;
                            bool _esitoMicro2 = false;
                            _esitoMicro2 = (_cb.StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.Blocco2InUso) == (byte)FirmwareManager.MascheraStato.Blocco2InUso;


                            if (_esitoMicro1)
                            {
                                txtFwAreaTestata.Text = "A1";
                                btnFwSwitchArea1.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                if (_esitoMicro2)
                                {
                                    txtFwAreaTestata.Text = "A2";
                                    btnFwSwitchArea2.BackColor = Color.LightGreen;
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

        private bool MostraStato(FirmwareManager.MascheraStato Valore, byte Stato, ref TextBox Cella, bool KOifFalse = false)
        {
            try
            {
                bool _esitocella = false;

                _esitocella = (Stato & (byte)Valore) == (byte)Valore;

                if (_esitocella)
                {
                    Cella.ForeColor = Color.Green;
                    Cella.Text = "OK";
                    return true;
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
                return false;
            }
            catch
            {
                return false;
            }
        }


        private void VerificaStatoFw()
        {
            try
            {
                string _tempId = "";
                _cb.VerificaPresenza();
                CaricaStatoFirmware(ref _tempId, _logiche, _cb.apparatoPresente);
                //CaricaStatoAreaFw(1, _sb.StatoFirmware.Stato);
                //CaricaStatoAreaFw(2, _sb.StatoFirmware.Stato);
            }
            catch (Exception Ex)
            {
                Log.Error("btnFwCaricaStato_Click: " + Ex.Message);
            }
        }
        /*
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
                    _area = 0x1E0000;
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
                    _area = 0x1F0000;

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
        */
        public bool SwitchAreaFw(string IdApparato, bool SerialeCollegata, byte IdArea)
        {
            bool _esito = false;
            byte[] _bufferDati = new byte[64];
            //uint _area;

            try
            {

                Cursor.Current = Cursors.WaitCursor;
                Log.Debug("Richiesta attivazione area " + IdArea.ToString());
                _esito = _cb.SwitchFirmware(IdApparato, SerialeCollegata, IdArea);

                Log.Debug("Risposta attivazione: " + _esito.ToString());

                if (_esito)
                {
                    // Switch riuscito aspetto 30 secondi, poi mi riconnetto
                    Application.DoEvents();
                    Log.Debug("Inizio attesa riconnessione: " + _esito.ToString());
                    _esito = _cb.AttendiRiconnessione(50,5000);
                    Log.Debug("Risposta riconnessione: " + _esito.ToString());
                    Cursor.Current = Cursors.Default;
                    if (!_esito)
                    {
                        MessageBox.Show(_parametri.lastError, "Riconnessione Fallita", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }
                }
                Cursor.Current = Cursors.Default;
                return _esito;

            }
            catch
            {
                Cursor.Current = Cursors.Default;
                return _esito;
            }
        }
        

        public bool SwitchAreaBl(string IdApparato, bool SerialeCollegata)
        {
            bool _esito = false;
            byte[] _bufferDati = new byte[64];
            //uint _area;

            try
            {

                Cursor.Current = Cursors.WaitCursor;
                _esito = _cb.SwitchToBootLoader(IdApparato, SerialeCollegata);


                if (_esito)
                {
                    // Switch riuscito, mi riconnetto
                    Application.DoEvents();
                    _esito = _cb.AttendiRiconnessione(200, 5000);
                    //Thread.Sleep(2000);
                    //Application.DoEvents();
                    //attivaCaricabatterie(ref _parametri, false);
                    //_esito = reconnectLadeLight();
                    Cursor.Current = Cursors.Default;
                    if (!_esito)
                    {
                        MessageBox.Show(_parametri.lastError, "Riconnessione Fallita", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;
                    }

                }
                Cursor.Current = Cursors.Default;
                return _esito;

            }
            catch
            {
                Cursor.Current = Cursors.Default;
                return _esito;
            }
        }


        public bool reconnectLadeLight()
        {
            bool _esito;
            try
            {


                Log.Debug("----------------------- reconnectSpyBat ---------------------------");

                string _idCorrente = "";// _cb.Id;
                abilitaSalvataggi(false);

                // in futuro, inserire quì il precaricamento delle statistiche
                //CaricaTestata(IdApparato, Logiche, SerialeCollegata);

                // 12/10/15: inizio leggendo lo stato del bootloader, per verificare se c'è un firmware caricato
                _esito = CaricaStatoFirmware(ref _idCorrente, _logiche, _apparatoPresente);
                if (!_esito)
                {
                    // Se non ho il firmware state potrebbe essere una versione precedente
                    // provo a leggere la testata
                    _esito = _cb.VerificaPresenza();
                    if (_idCorrente != "")
                    {
                        //CaricaTestata(_idCorrente, _logiche, _apparatoPresente);
                    }

                }
                else
                {
                    /*
                    if (_cb.FirmwarePresente)
                    {
                        // Se sono in stato BL lo evidenzio e mi fermo, altrimenti leggo la testata
                        if ((_sb.StatoFirmware.Stato & (byte)FirmwareManager.MascheraStato.BootLoaderInUso) == (byte)FirmwareManager.MascheraStato.BootLoaderInUso)
                        {
                            MostraTestata();
                            txtRevSWSb.Text = "BOOTLOADER";
                            txtRevSWSb.ForeColor = Color.Red;

                            // se l'apparato è collegato abilito i salvataggi
                            abilitaSalvataggi(_sb.apparatoPresente);

                            Log.Info("Stato scheda SPY-BATT: LD OK, MODO BOOTLOADER ");

                        }
                        else
                        {
                            //TODO: gestire io DB se la scheda è già in archivio
                            CaricaTestata(_idCorrente, _logiche, _apparatoPresente);
                        }


                    }
                    else
                    {
                        MostraTestata();
                    }
                    */
                }

                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
                return false;
            }

        }


    }  // Fine Classe
}
