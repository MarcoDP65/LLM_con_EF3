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



        /// <summary>
        /// Carico la lista delle letture per l'analisi corrente
        /// </summary>
        private void InitVistaBlocchiFwIN()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 7, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvwLettureParametri.HeaderUsesThemes = false;
                flvwLettureParametri.HeaderFormatStyle = _stile;
                flvwLettureParametri.UseAlternatingBackColors = true;
                flvwLettureParametri.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvwLettureParametri.AllColumns.Clear();

                flvwLettureParametri.View = View.Details;
                flvwLettureParametri.ShowGroups = false;
                flvwLettureParametri.GridLines = true;

                BrightIdeasSoftware.OLVColumn colLettura = new BrightIdeasSoftware.OLVColumn();
                colLettura.Text = "N.";
                colLettura.AspectName = "strLettura";
                colLettura.Width = 40;
                colLettura.HeaderTextAlign = HorizontalAlignment.Center;
                colLettura.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colLettura);

                BrightIdeasSoftware.OLVColumn colTimeStamp = new BrightIdeasSoftware.OLVColumn();
                colTimeStamp.Text = "Time";
                colTimeStamp.AspectName = "strOraLettura";
                colTimeStamp.Width = 70;
                colTimeStamp.HeaderTextAlign = HorizontalAlignment.Center;
                colTimeStamp.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colTimeStamp);

                BrightIdeasSoftware.OLVColumn colTempoCiclo = new BrightIdeasSoftware.OLVColumn();
                colTempoCiclo.Text = "T.Ciclo";
                colTempoCiclo.AspectName = "strSecondsFromStart";
                colTempoCiclo.Width = 70;
                colTempoCiclo.HeaderTextAlign = HorizontalAlignment.Center;
                colTempoCiclo.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colTempoCiclo);


                BrightIdeasSoftware.OLVColumn colTensioneIst = new BrightIdeasSoftware.OLVColumn();
                colTensioneIst.Text = "V ist";
                colTensioneIst.AspectName = "strTensioneIstantanea";
                colTensioneIst.Width = 60;
                colTensioneIst.HeaderTextAlign = HorizontalAlignment.Center;
                colTensioneIst.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colTensioneIst);


                BrightIdeasSoftware.OLVColumn colCorrIst = new BrightIdeasSoftware.OLVColumn();
                colCorrIst.Text = "A ist";
                colCorrIst.AspectName = "strCorrenteIstantanea";
                colCorrIst.Width = 60;
                colCorrIst.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrIst.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colCorrIst);


                BrightIdeasSoftware.OLVColumn colCorrCaricata = new BrightIdeasSoftware.OLVColumn();
                colCorrCaricata.Text = "Ah car";
                colCorrCaricata.AspectName = "strAhCaricati";
                colCorrCaricata.Width = 60;
                colCorrCaricata.HeaderTextAlign = HorizontalAlignment.Center;
                colCorrCaricata.TextAlign = HorizontalAlignment.Right;
                flvwLettureParametri.AllColumns.Add(colCorrCaricata);




                //-------------------------------------------- 


                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 50;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flvwLettureParametri.AllColumns.Add(colRowFiller);

                flvwLettureParametri.RebuildColumns();

                flvwLettureParametri.SetObjects(ListaValori);
                flvwLettureParametri.BuildList();
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
                txtFWInSBFRev.Text = "";
                txtFWLibSBFRev.Text = "";
                txtFWInSBFDtRev.Text = "";
                txtFWTxFileLenN1.Text = "";

                txtFWTxFileLenP.Text = "";
                txtFWTxDataLenN1.Text = "";

                txtFWTxDataLenP.Text = "";
                btnFWPreparaTrasmissione.Enabled = false;

                if (txtFWFileSBFrd.Text == "")
                {
                    //messagebox
                    return false;
                }

                _esito = _firmMng.CaricaFileLLF(txtFWFileSBFrd.Text);
                if (_esito == FirmwareLLManager.ExitCode.OK)
                {

                    txtFWInSBFRev.Text = _firmMng.FirmwareData.Release;
                    txtFWInSBFDtRev.Text = FunzioniMR.StringaDataTS(_firmMng.FirmwareData.ReleaseDateBlock);
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
                    //txtFWLibSBFRev.Text = _firmMng.FirmwareData.StrategyLibRelease;
                    //txtFWTxFileLenN1.Text = _firmMng.FirmwareData.LenFlash1.ToString();
                    //txtFWTxFileLenN2.Text = _firmMng.FirmwareData.LenFlash2.ToString();
                    //txtFWTxFileLenP.Text = _firmMng.FirmwareData.LenProxy.ToString();

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
                txtFWTxDataLenN1.Text = "";
                txtFWTxDataLenP.Text = "";

                txtFWTxDataAddrN1.Text = "";

                txtFWTxDataAddrP.Text = "";

                txtFWTxDataNumN1.Text = "";

                txtFWTxDataNumP.Text = "";
                txtFWTxDataNumTot.Text = "";
                btnFWLanciaTrasmissione.Enabled = false;

                _esito = _firmMng.PreparaUpgradeFw();
                if (_esito == FirmwareLLManager.ExitCode.OK)
                {
                    _esito = _firmMng.ComponiArrayTestata();
                    if (_esito == FirmwareLLManager.ExitCode.OK)
                    {
                        /*
                        txtFWTxDataLenN1.Text = _firmMng.FirmwareBlock.LenFlash.ToString();
                       // txtFWTxDataLenP.Text = _firmMng.FirmwareBlock.LenProxy.ToString();

                        txtFWTxDataNumN1.Text = _firmMng.FirmwareBlock.ListaFlash.Count().ToString();
                        //txtFWTxDataNumN2.Text = _firmMng.FirmwareBlock.ListaFlash2.Count().ToString();
                        txtFWTxDataNumP.Text = "1";// _firmMng.FirmwareBlock.ListaProxy.Count().ToString();
                        txtFWTxDataNumTot.Text = _firmMng.FirmwareBlock.TotaleBlocchi.ToString();

                       // txtFWTxDataAddrN1.Text = "0x" + _firmMng.FirmwareBlock.AddrFlash.ToString("X4");
                        //txtFWTxDataAddrN2.Text = "0x" + _firmMng.FirmwareBlock.AddrFlash2.ToString("X4");
                        //txtFWTxDataAddrP.Text = "0x" + _firmMng.FirmwareBlock.AddrProxy.ToString("X4");
                        Log.Error("Teststa FW: " + FunzioniMR.hexdumpArray(_firmMng.FirmwareBlock.MessaggioTestata));
                        */
                        btnFWLanciaTrasmissione.Enabled = true;
                    }

                }



            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat.DumpInteraMemoria: " + Ex.Message);

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
                byte _area;
                byte.TryParse(txtFWSBFArea.Text, out _area);
                if (_area != 2)
                    _area = 1;

                txtFWSBFArea.Text = _area.ToString();
                _avCicli.ParametriWorker.MainCount = 100;

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
