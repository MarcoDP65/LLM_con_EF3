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
using System.Globalization;

namespace PannelloCharger
{
    public partial class frmCaricabatterieV2 : Form
    {


        private void InizializzaListaCariche()
        {
            try
            {
                // strSortIdMemCiclo

                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvCicliListaCariche.HeaderUsesThemes = false;
                flvCicliListaCariche.HeaderFormatStyle = _stile;
                flvCicliListaCariche.UseAlternatingBackColors = true;
                flvCicliListaCariche.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvCicliListaCariche.AllColumns.Clear();
                flvCicliListaCariche.RowHeight = 18;

                flvCicliListaCariche.View = View.Details;
                flvCicliListaCariche.ShowGroups = false;
                flvCicliListaCariche.GridLines = true;
                flvCicliListaCariche.FullRowSelect = true;
                flvCicliListaCariche.MultiSelect = false;


                BrightIdeasSoftware.OLVColumn sortColPosizioneCiclo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Pos",
                    AspectName = "strSortIdMemCiclo",
                    Width = 0,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,


                };


                BrightIdeasSoftware.OLVColumn sortColIdMemCiclo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "ID",
                    AspectName = "strSortIdMemCiclo",
                    Width = 0,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                    
                    
                };

                flvCicliListaCariche.AllColumns.Add(sortColIdMemCiclo);

                BrightIdeasSoftware.OLVColumn colIdMemCiclo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "ID",
                    AspectName = "strIdMemCiclo",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colIdMemCiclo);

                BrightIdeasSoftware.OLVColumn colDataOraStart = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Start",
                    AspectName = "DataOraStart",
                    Width = 100,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colDataOraStart);


                BrightIdeasSoftware.OLVColumn colDataOraStop = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Stop",
                    AspectName = "DataOraFine",
                    Width = 100,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colDataOraStop);


                BrightIdeasSoftware.OLVColumn colDurata = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Durata",
                    AspectName = "strDurata",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colDurata);

                BrightIdeasSoftware.OLVColumn colstrV5Min = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "V 5m",
                    AspectName = "strVbat5m",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrV5Min);

                BrightIdeasSoftware.OLVColumn colstrI5Min = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "I 5m",
                    AspectName = "strIbat5m",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrI5Min);

                BrightIdeasSoftware.OLVColumn colstrVFin = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "V Fin",
                    AspectName = "strVbatFinale",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrVFin);


                BrightIdeasSoftware.OLVColumn colstrIFin = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "I Fin",
                    AspectName = "strIbatFinale",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrIFin);

                BrightIdeasSoftware.OLVColumn colstrAh = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Ah",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strAh",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrAh);

                BrightIdeasSoftware.OLVColumn colstrKWh = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "KWh",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strKWh",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrKWh);

                BrightIdeasSoftware.OLVColumn colstrCondizioneStop = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Stop",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strCondizioneStop",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Center,
                };
                flvCicliListaCariche.AllColumns.Add(colstrCondizioneStop);

                
                BrightIdeasSoftware.OLVColumn colstrChargerStop = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Causale Stop",
                    ToolTipText = "Motivo fermata carica",
                    AspectName = "strChargerStop",
                    Width = 150,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colstrChargerStop);

                BrightIdeasSoftware.OLVColumn colstrChargerOpt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Opzioni",
                    ToolTipText = "Opzioni carica",
                    AspectName = "strOpzioniCarica",
                    Width = 150,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colstrChargerOpt);

                BrightIdeasSoftware.OLVColumn colstrChargerErr = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Errori",
                    ToolTipText = "Vettore Errori",
                    AspectName = "strVettoreErrori",
                    Width = 150,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colstrChargerErr);


                BrightIdeasSoftware.OLVColumn colEqualReq = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Eq R",
                    ToolTipText = "Equalizzazione Richiesta",
                    AspectName = "strEqualRequest",
                    Width = 45,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "OK", Properties.Resources.OK_16, "KO", Properties.Resources.KO_16, "OFF", Properties.Resources.GRAY_16 })

                };
                flvCicliListaCariche.AllColumns.Add(colEqualReq);

                BrightIdeasSoftware.OLVColumn colEqualEff = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Eq E",
                    ToolTipText = "Equalizzazione Effettuata",
                    AspectName = "strEqualEffettuato",
                    Width = 45,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "OK", Properties.Resources.OK_16, "KO", Properties.Resources.KO_16, "OFF", Properties.Resources.GRAY_16})

                };
                flvCicliListaCariche.AllColumns.Add(colEqualEff);




                BrightIdeasSoftware.OLVColumn colstrNumEventiBrevi = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Brevi",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strNumEventiBrevi",
                    Width = 80,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colstrNumEventiBrevi);

                BrightIdeasSoftware.OLVColumn colstrPtrBrevi = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Ptr Primo",
                    AspectName = "strPuntatorePrimoBreve",
                    Width = 80,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrPtrBrevi);

                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "",
                    AspectName = "",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                    FillsFreeSpace = true,
                };
                flvCicliListaCariche.AllColumns.Add(colRowFiller);

                flvCicliListaCariche.RebuildColumns();
                flvCicliListaCariche.SetObjects(_cb.MemoriaCicli);
                flvCicliListaCariche.Sort(sortColPosizioneCiclo, SortOrder.Descending);
                flvCicliListaCariche.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }

        private bool VuotaListaCariche()
        {
            try
            {
                _cb.MemoriaCicli = new List<llMemoriaCicli>();
                InizializzaListaCariche();

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("VuotaListaCariche: " + Ex.Message);
                return false;
            }
        }

        public bool CaricaListaCariche( UInt32 StartAddr, ushort NumRows = 0, bool TaskExt = false)
        {
            object EsitoCaricamento;
            bool _esito;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!_cb.ContatoriLL.valido)
                {
                   if(! _cb.CaricaAreaContatori())
                    {
                        this.Cursor = Cursors.Arrow;
                        return false;
                    }
                }

                if (NumRows == 0)
                {
                    NumRows = (ushort)_cb.ContatoriLL.CntCariche;
                }




                if (NumRows > 10)
                {
                    Log.Debug("Lancio lettura lunghi");
                   
                    _avCicli.ParametriWorker.MainCount = 100;
                    _avCicli.llLocale = _cb;
                    _avCicli.ValStart = (int)0;
                    _avCicli.AddrStart = StartAddr;
                    _avCicli.ValFine = (int)NumRows;// _sb.sbData.LongMem;
                    _avCicli.DbDati = _logiche.dbDati.connessione;
                    _avCicli.CaricaBrevi = false; // chkCaricaBrevi.Checked;
                    _avCicli.ElementoPilotato = frmAvanzamentoCicli.ControlledDevice.LadeLight;
                    _avCicli.TipoComando = elementiComuni.tipoMessaggio.MemLungaLL;
                    Log.Debug("FRM RicaricaCicli: ");

                    //_esito = _sb.RicaricaCaricaCicliMemLunga(Inizio, (uint)_sb.sbData.LongMem, _logiche.dbDati.connessione, true, CaricaBrevi);

                    // Apro il form con le progressbar
                    _avCicli.ShowDialog(this);

                    // _esito = _cb.CaricaListaCicli(StartAddr, NumRows, out EsitoCaricamento, false, true);
                    _esito = true;

                }
                else
                {
                    _esito = _cb.CaricaListaCicli(StartAddr, NumRows, out EsitoCaricamento, false, false);
                }

                InizializzaListaCariche();
                this.Cursor = Cursors.Arrow;
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaListaCariche: " + Ex.Message);
                return false;
            }
        }

        public bool 
            MostraCicloCorrente()
        {
            try
            {
                txtPaNomeSetup.Text = "";
                txtPaCapacita.Text = "";

                //cmbPaProfilo.SelectedIndex = 0;
                txtPaCapacita.Text = "";
                txtPaTempoT2Max.Text = "";
                txtPaSogliaVs.Text = "";
                txtPaCorrenteI1.Text = "";
                txtPaTensione.Text = "";

                //cmbPaCondStop.SelectedIndex = 0;
                txtPaCoeffK.Text = "";
                txtPaTempoT2Min.Text = "";
                txtPaTempoT2Max.Text = "";
                chkPaUsaSpyBatt.Checked = false;
                if (_cb.Programmazioni.ProgrammaAttivo != null)
                {
                    txtPaNomeSetup.Text = _cb.Programmazioni.ProgrammaAttivo.ProgramName;
                    txtPaCapacita.Text = FunzioniMR.StringaCapacita(_cb.Programmazioni.ProgrammaAttivo.BatteryAhdef,10) ;
                    List<sbTipoBatteria> Lista = (List<sbTipoBatteria>)(cmbPaTipoBatteria.DataSource);
                    cmbPaTipoBatteria.SelectedItem = Lista.Find(x => x.BatteryTypeId == _cb.Programmazioni.ProgrammaAttivo.BatteryType);
                    List<_llProfiloCarica> ListaP = (List<_llProfiloCarica>)(cmbPaProfilo.DataSource);
                    cmbPaProfilo.SelectedItem = ListaP.Find(x => x.IdProfiloCaricaLL == _cb.Programmazioni.ProgrammaAttivo.IdProfilo);
                    List<llTensioneBatteria> ListaV = (List<llTensioneBatteria>)(cmbPaTensione.DataSource);
                    cmbPaTensione.SelectedItem = ListaV.Find(x => x.IdTensione == _cb.Programmazioni.ProgrammaAttivo.BatteryVdef);
                    txtPaTensione.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.BatteryVdef);
                    List<llDurataCarica> ListaD = (List<llDurataCarica>)(cmbPaDurataCarica.DataSource);

                    cmbPaDurataCarica.SelectedItem = ListaD.Find(x => x.IdDurataCaricaLL == _cb.Programmazioni.ProgrammaAttivo.DurataMaxCarica);
                    txtPaTempoT2Min.Text = _cb.Programmazioni.ProgrammaAttivo.DurataMinFase2.ToString();
                    txtPaTempoT2Max.Text = _cb.Programmazioni.ProgrammaAttivo.DurataMaxFase2.ToString();
                    txtPaCoeffK.Text = _cb.Programmazioni.ProgrammaAttivo.PercTempoFase2.ToString();
                    txtPaTempoT3Max.Text = _cb.Programmazioni.ProgrammaAttivo.DurataMaxFase3.ToString();

                    txtPaSogliaVs.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VSoglia);
                    txtPaRaccordoF1.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VRaccordoF1);
                    txtPaVMax.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VMax);
                    txtPaVLimite.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VCellLimite);
                    txtPaVMinRic.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VMinRec);
                    txtPaVMaxRic.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VMaxRec);

                    txtPaNumCelle.Text = _cb.Programmazioni.ProgrammaAttivo.NumeroCelle.ToString();
                    txtPaCorrenteF3.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.CorrenteFase3);


                    txtPaCorrenteI1.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.CorrenteMax);

                    if (_cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi >0 || _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa>0)
                    {
                        chkPaAttivaEqual.Checked = true;
                        txtPaEqualNumPulse.Text = _cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi.ToString();
                        txtPaEqualAttesa.Text = _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa.ToString();
                        txtPaEqualPulsePause.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataPausa.ToString();
                        txtPaEqualPulseTime.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataImpulso.ToString();
                        txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.EqualCorrenteImpulso );

                    }
                    else
                    {
                        chkPaAttivaEqual.Checked = false;
                        txtPaEqualNumPulse.Text = "";
                        txtPaEqualAttesa.Text = "";
                        txtPaEqualPulsePause.Text = "";
                        txtPaEqualPulseTime.Text = "";
                        txtPaEqualPulseCurrent.Text = "";

                    }

                    chkPaUsaSpyBatt.Checked = (_cb.Programmazioni.ProgrammaAttivo.AbilitaComunicazioneSpybatt == 0);


                    if (_cb.Programmazioni.ProgrammaAttivo.TempoAttesaBMS> 0 || _cb.Programmazioni.ProgrammaAttivo.TempoErogazioneBMS > 0)
                    {
                        chkPaAttivaRiarmoBms.Checked = true;
                        txtPaBMSTempoAttesa.Text = _cb.Programmazioni.ProgrammaAttivo.TempoAttesaBMS.ToString();
                        txtPaBMSTempoErogazione.Text = _cb.Programmazioni.ProgrammaAttivo.TempoErogazioneBMS.ToString();
                    }
                    else
                    {
                        chkPaAttivaRiarmoBms.Checked = false;
                        txtPaBMSTempoAttesa.Text = "";
                        txtPaBMSTempoErogazione.Text = "";
                    }


                }




                return true;
            }
            catch
            {
                return false;
            }

        }

 
        public List<llMemBreve> CaricaListaBrevi(UInt32 StartAddr, ushort NumRows = 0, uint IdCiclo = 0)
        {

            bool _esito;
            List<llMemBreve> ListaBrevi;
            try
            {
                ListaBrevi = new List<llMemBreve>();

                ListaBrevi = _cb.CaricaListaBrevi(StartAddr, NumRows,IdCiclo);

                //InizializzaListaCariche();
                return ListaBrevi;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaListaCariche: " + Ex.Message);
                return null;
            }
        }




    }
}
