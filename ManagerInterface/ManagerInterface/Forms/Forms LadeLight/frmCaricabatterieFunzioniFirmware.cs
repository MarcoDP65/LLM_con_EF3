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

                lvwFWInFileStruct.SetObjects(ListaAreeLLF);
                lvwFWInFileStruct.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }



        public void CaricafileLLCCS()
        {



            FirmwareLLManager.ExitCode _esito;
            try
            {
                txtFWInFileStruct.Text = "";

                _esito = _firmMng.CaricaFileCCS(txtFwFileCCS.Text);

                if (_esito == FirmwareLLManager.ExitCode.OK)
                {
                

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

                        btnFWLanciaTrasmissione.Enabled = true;
                    }

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



    }  // Fine Classe
}
