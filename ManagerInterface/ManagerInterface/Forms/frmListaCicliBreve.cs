using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BrightIdeasSoftware;

using System.Windows.Forms.DataVisualization.Charting;
//using OxyPlot;
//using OxyPlot.Series;

using log4net;
using log4net.Config;

using ChargerLogic;
using MoriData;
using Utility;

namespace PannelloCharger
{
    public partial class frmListaCicliBreve : Form
    {

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        
        public sbMemLunga CicloLungo;
        public System.Collections.Generic.List<MessaggioSpyBatt.MemoriaPeriodoBreve> CicliMemoriaBreve = new System.Collections.Generic.List<MessaggioSpyBatt.MemoriaPeriodoBreve>();
        public MessaggioSpyBatt.MemoriaPeriodoLungo EventoLungo;
        public UnitaSpyBatt _sb;
        public bool[] AttivaIntermedi = new bool[4];
        private OxyPlot.WindowsForms.PlotView oxyContainerCiclo;
        private OxyPlot.PlotModel oxyGraficoCiclo;

        private OxyPlot.WindowsForms.PlotView oxyContainerTensioni;
        private OxyPlot.PlotModel oxyGraficoTensioni;

        public System.Collections.Generic.List<mrDataPoint> ValoriCicloTensioni = new List<mrDataPoint>();
        public System.Collections.Generic.List<mrDataPoint> ValoriCicloCorrenti = new List<mrDataPoint>();
        public System.Collections.Generic.List<mrDataPoint> ValoriCicloTemp = new List<mrDataPoint>();

        public System.Collections.Generic.List<mrDataPoint> ValoriTensioniCellaTot = new List<mrDataPoint>();
        public System.Collections.Generic.List<mrDataPoint> ValoriTensioniCellaS1 = new List<mrDataPoint>();
        public System.Collections.Generic.List<mrDataPoint> ValoriTensioniCellaS2= new List<mrDataPoint>();
        public System.Collections.Generic.List<mrDataPoint> ValoriTensioniCellaS3 = new List<mrDataPoint>();
        public System.Collections.Generic.List<mrDataPoint> ValoriTensioniCellaS4 = new List<mrDataPoint>();

        private sbMemBreve CicloBreve;

        int _idxAmin;
        int _idxAmax;

        
        public frmListaCicliBreve()
        {
            Log.Debug("frmListaCicliBreve start");
            InitializeComponent();
            Log.Debug("frmListaCicliBreve InitializeComponent");
            InizializzaOxyPlotControl();
            Log.Debug("frmListaCicliBreve InizializzaOxyPlotControl");

        }


        private void frmListaCicliBreve_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.Width > 500)
                {
                    tabCicloBreve.Width = this.Width - 50;

                    //crtGraficoCiclo.Width = this.Width - 90;
                    //crtGraficoTensioni.Width = this.Width - 90;
                    flvwCicliBrevi.Width = this.Width - 90;
                    
                    btnChiudi.Left = this.Width - 200;
                    //lvwCicliBatteriaOld.Width = this.Width - 120;
                    if(oxyContainerCiclo != null)
                    {
                        oxyContainerCiclo.Width = this.Width - 90;
                    }
                    if (oxyContainerTensioni != null)
                    {
                        oxyContainerTensioni.Width = this.Width - 90;
                    }

                }

                if (this.Height > 250)
                {
                    tabCicloBreve.Height = this.Height - 100;
                    //grbGeneraExcel.Top = this.Height - 120;
                    btnChiudi.Top = this.Height - 100;

                    flvwCicliBrevi.Height = this.Height - 250;
                    //crtGraficoCiclo.Height = this.Height - 160;
                    //crtGraficoTensioni.Height = this.Height - 160;
                    if (oxyContainerCiclo != null)
                    {
                        oxyContainerCiclo.Height = this.Height - 160;
                    }
                    if (oxyContainerTensioni != null)
                    {

                        oxyContainerTensioni.Height = this.Height - 160;
                    }
                    btnChiudi.Top = this.Height - 80;
                }


            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }



        private void rigeneraListaCicli()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);

                flvwCicliBrevi.HeaderUsesThemes = false;
                flvwCicliBrevi.HeaderFormatStyle = _stile;

                flvwCicliBrevi.AllColumns.Clear();

                flvwCicliBrevi.View = View.Details;
                flvwCicliBrevi.ShowGroups = false;
                flvwCicliBrevi.GridLines = true;

                BrightIdeasSoftware.OLVColumn colIdBreve = new BrightIdeasSoftware.OLVColumn();
                colIdBreve.Text = "Ev.";
                colIdBreve.AspectName = "IdMemoriaBreve";
                colIdBreve.Width = 60;
                colIdBreve.HeaderTextAlign = HorizontalAlignment.Left;
                colIdBreve.TextAlign = HorizontalAlignment.Left;
                flvwCicliBrevi.AllColumns.Add(colIdBreve);

                BrightIdeasSoftware.OLVColumn colDataOra = new BrightIdeasSoftware.OLVColumn();
                colDataOra.Text = "Data/Ora";
                colDataOra.AspectName = "DataOraRegistrazione";
                colDataOra.Width = 100;
                colDataOra.HeaderTextAlign = HorizontalAlignment.Left;
                colDataOra.TextAlign = HorizontalAlignment.Right;
                flvwCicliBrevi.AllColumns.Add(colDataOra);

                BrightIdeasSoftware.OLVColumn colVbat = new BrightIdeasSoftware.OLVColumn();
                colVbat.Text = "V batt";
                colVbat.AspectName = "strVreg";
                colVbat.Width = 60;
                colVbat.HeaderTextAlign = HorizontalAlignment.Center;
                colVbat.TextAlign = HorizontalAlignment.Right;
                flvwCicliBrevi.AllColumns.Add(colVbat);

                //CicloBreve.ValoriIntermedi.strTensioniCellaAssolute.Vbatt
                BrightIdeasSoftware.OLVColumn colV3 = new BrightIdeasSoftware.OLVColumn();
                colV3.Text = "V 3";
                colV3.AspectName = "strV3";
                colV3.Width = 60;
                colV3.HeaderTextAlign = HorizontalAlignment.Center;
                colV3.TextAlign = HorizontalAlignment.Right;
                colV3.IsVisible = AttivaIntermedi[3]; 
                flvwCicliBrevi.AllColumns.Add(colV3);

                BrightIdeasSoftware.OLVColumn colV2 = new BrightIdeasSoftware.OLVColumn();
                colV2.Text = "V 2";
                colV2.AspectName = "strV2";
                colV2.Width = 60;
                colV2.HeaderTextAlign = HorizontalAlignment.Center;
                colV2.TextAlign = HorizontalAlignment.Right;
                colV2.IsVisible = AttivaIntermedi[2]; 
                flvwCicliBrevi.AllColumns.Add(colV2);

                BrightIdeasSoftware.OLVColumn colV1 = new BrightIdeasSoftware.OLVColumn();
                colV1.Text = "V 1";
                colV1.AspectName = "strV1";
                colV1.Width = 60;
                colV1.HeaderTextAlign = HorizontalAlignment.Center;
                colV1.TextAlign = HorizontalAlignment.Right;
                colV1.IsVisible = AttivaIntermedi[1]; 
                flvwCicliBrevi.AllColumns.Add(colV1); 
                
                

                // Tensioni Cella Assolute
                BrightIdeasSoftware.OLVColumn colVbatCAss = new BrightIdeasSoftware.OLVColumn();
                colVbatCAss.Text = "Vc batt";
                colVbatCAss.AspectName = "strVcBatt";
                colVbatCAss.Width = 60;
                colVbatCAss.HeaderTextAlign = HorizontalAlignment.Center;
                colVbatCAss.TextAlign = HorizontalAlignment.Right;
                flvwCicliBrevi.AllColumns.Add(colVbatCAss);

                
                BrightIdeasSoftware.OLVColumn colV3CAss = new BrightIdeasSoftware.OLVColumn();
                colV3CAss.Text = "Vc 3";
                colV3CAss.AspectName = "strVc3";
                colV3CAss.Width = 60;
                colV3CAss.HeaderTextAlign = HorizontalAlignment.Center;
                colV3CAss.TextAlign = HorizontalAlignment.Right;
                colV3CAss.IsVisible = AttivaIntermedi[3];
                flvwCicliBrevi.AllColumns.Add(colV3CAss);


                 
                BrightIdeasSoftware.OLVColumn colV2CAss = new BrightIdeasSoftware.OLVColumn();
                colV2CAss.Text = "Vc 2";
                colV2CAss.AspectName = "strVc2";
                colV2CAss.Width = 60;
                colV2CAss.HeaderTextAlign = HorizontalAlignment.Center;
                colV2CAss.TextAlign = HorizontalAlignment.Right;
                colV2CAss.IsVisible = AttivaIntermedi[2]; 
                flvwCicliBrevi.AllColumns.Add(colV2CAss);


                BrightIdeasSoftware.OLVColumn colV1CAss = new BrightIdeasSoftware.OLVColumn();
                colV1CAss.Text = "Vc 1";
                colV1CAss.AspectName = "strVc1";
                colV1CAss.Width = 60;
                colV1CAss.HeaderTextAlign = HorizontalAlignment.Center;
                colV1CAss.TextAlign = HorizontalAlignment.Right;
                colV1CAss.IsVisible = AttivaIntermedi[1]; 
                flvwCicliBrevi.AllColumns.Add(colV1CAss);

                // Tensioni Cella Relative
                BrightIdeasSoftware.OLVColumn colVbatCRel = new BrightIdeasSoftware.OLVColumn();
                colVbatCRel.Text = "Vcs batt";
                colVbatCRel.ToolTipText = "Tensioni per cella per la sezione 4 ( V4 - V3 )";
                colVbatCRel.AspectName = "strVcsBatt";
                colVbatCRel.Width = 80;
                colVbatCRel.HeaderTextAlign = HorizontalAlignment.Center;
                colVbatCRel.TextAlign = HorizontalAlignment.Right;
                flvwCicliBrevi.AllColumns.Add(colVbatCRel);

                BrightIdeasSoftware.OLVColumn colV3CRel = new BrightIdeasSoftware.OLVColumn();
                colV3CRel.Text = "Vcs 3";
                colVbatCRel.ToolTipText = "Tensioni per cella per la sezione 3 ( V3 - V2 )";
                colV3CRel.AspectName = "strVcs3";
                colV3CRel.Width = 60;
                colV3CRel.HeaderTextAlign = HorizontalAlignment.Center;
                colV3CRel.TextAlign = HorizontalAlignment.Right;
                colV3CRel.IsVisible = AttivaIntermedi[3]; 
                flvwCicliBrevi.AllColumns.Add(colV3CRel);

                BrightIdeasSoftware.OLVColumn colV2CRel = new BrightIdeasSoftware.OLVColumn();
                colV2CRel.Text = "Vcs 2";
                colV2CRel.AspectName = "strVcs2";
                colV2CRel.Width = 60;
                colV2CRel.HeaderTextAlign = HorizontalAlignment.Center;
                colV2CRel.TextAlign = HorizontalAlignment.Right;
                colV2CRel.IsVisible = AttivaIntermedi[2]; 
                flvwCicliBrevi.AllColumns.Add(colV2CRel);


                BrightIdeasSoftware.OLVColumn colV1CRel = new BrightIdeasSoftware.OLVColumn();
                colV1CRel.Text = "Vcs 1";
                colV1CRel.AspectName = "strVcs1";
                colV1CRel.Width = 60;
                colV1CRel.HeaderTextAlign = HorizontalAlignment.Center;
                colV1CRel.TextAlign = HorizontalAlignment.Right;
                colV1CRel.IsVisible = AttivaIntermedi[1]; 
                flvwCicliBrevi.AllColumns.Add(colV1CRel);



                BrightIdeasSoftware.OLVColumn colImed = new BrightIdeasSoftware.OLVColumn();
                colImed.Text = "I med";
                colImed.AspectName = "olvAmed";
                colImed.Width = 60;
                colImed.HeaderTextAlign = HorizontalAlignment.Center;
                colImed.TextAlign = HorizontalAlignment.Right;
                flvwCicliBrevi.AllColumns.Add(colImed);


                BrightIdeasSoftware.OLVColumn colImin = new BrightIdeasSoftware.OLVColumn();
                colImin.Text = "I min";
                colImin.AspectName = "olvAmin";
                colImin.Width = 60;
                colImin.HeaderTextAlign = HorizontalAlignment.Center;
                colImin.TextAlign = HorizontalAlignment.Right;
                flvwCicliBrevi.AllColumns.Add(colImin);
                _idxAmin = colImin.Index;

                BrightIdeasSoftware.OLVColumn colImax = new BrightIdeasSoftware.OLVColumn();
                colImax.Text = "I max";
                colImax.AspectName = "olvAmax";
                colImax.Width = 60;
                colImax.HeaderTextAlign = HorizontalAlignment.Center;
                colImax.TextAlign = HorizontalAlignment.Right;
                flvwCicliBrevi.AllColumns.Add(colImax);
                _idxAmax = colImax.Index;

                BrightIdeasSoftware.OLVColumn colTemp = new BrightIdeasSoftware.OLVColumn();
                colTemp.Text = "C°";
                colTemp.AspectName = "strTemp";
                colTemp.Width = 60;
                colTemp.HeaderTextAlign = HorizontalAlignment.Center;
                colTemp.TextAlign = HorizontalAlignment.Right;
                flvwCicliBrevi.AllColumns.Add(colTemp);

                BrightIdeasSoftware.OLVColumn colElettrolita = new BrightIdeasSoftware.OLVColumn();
                colElettrolita.Text = "Elettrolita";
                colElettrolita.AspectName = "PresenzaElettrolita";
                colElettrolita.AspectGetter = delegate(object _Valore)
                {
                    sbMemBreve _tempVal = (sbMemBreve)_Valore;
                    return FunzioniMR.StringaPresenza(_tempVal.PresenzaElettrolita);
                }; 
                colElettrolita.Width = 80;
                colElettrolita.HeaderTextAlign = HorizontalAlignment.Center;
                colElettrolita.TextAlign = HorizontalAlignment.Right;
                flvwCicliBrevi.AllColumns.Add(colElettrolita);

                BrightIdeasSoftware.OLVColumn colVback = new BrightIdeasSoftware.OLVColumn();
                colVback.Text = "V Bk";
                colVback.AspectName = "strVbatBk";
                colVback.Width = 60;
                colVback.HeaderTextAlign = HorizontalAlignment.Center;
                colVback.TextAlign = HorizontalAlignment.Right;
                flvwCicliBrevi.AllColumns.Add(colVback);

                BrightIdeasSoftware.OLVColumn colFiller = new BrightIdeasSoftware.OLVColumn();
                colFiller.Text = "";
                colFiller.Width = 60;
                colFiller.FillsFreeSpace = true;
                colFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colFiller.TextAlign = HorizontalAlignment.Right;
                flvwCicliBrevi.AllColumns.Add(colFiller);           


                //-------------------------------------------------------------------------
                flvwCicliBrevi.RebuildColumns();

                this.flvwCicliBrevi.SetObjects(CicloLungo.CicliMemoriaBreve);
                flvwCicliBrevi.BuildList();
                Log.Debug("MostraCicli rigeneraListaCicli completed");
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }



        private void flvwCicliBrevi_FormatRow(object sender, FormatRowEventArgs e)
        {

        }

        private void flvwCicliBrevi_FormatCell(object sender, FormatCellEventArgs e)
        {
            string _text = e.SubItem.Text;
            if (_text.Contains("** - "))
            {
                e.SubItem.Text = e.SubItem.Text.Substring(5);
                e.SubItem.ForeColor = Color.Red;

            }
            

        }

        private void flvwCicliBrevi_ItemActivate(object sender, EventArgs e)
        {

            Log.Debug("flvwCicliBatteria_ItemActivate " + sender.ToString());

        }

        public void VisualizzaGrafici ( bool Visibile)
        {
            try
            {
                if (Visibile != true)
                {
                    //tbpAndamentoCiclo.Hide();
                    tbpTensioniCiclo.Hide();
                    tbpAndamentoOxy.Hide();
                }
                else
                {
                    //tbpAndamentoCiclo.Show();
                    tbpTensioniCiclo.Show();
                    tbpAndamentoOxy.Show();
                }

            }
            catch (Exception Ex)
            {
                Log.Error("VisualizzaGrafici: " + Ex.Message);
            }
        }

        public void MostraCicli()
        {
            try
            {
                Log.Debug("MostraCicli");

                string _Flag;

                //rigeneraListaCicli();

                int _ciclo = 0;
  
            // Prima Carico la testata


                // Ciclo Lungo
                txtIdEventoLungo.Text = CicloLungo.IdMemoriaLunga.ToString(); // EventoLungo.IdEvento.ToString();
                txtInizioEvento.Text = CicloLungo.DataOraStart; //_sb.StringaTimestamp(EventoLungo.DataOraStart);
                txtFineEvento.Text = CicloLungo.DataOraFine; //_sb.StringaTimestamp(EventoLungo.DataOraFine);
                this.Text = "Dettaglio Ciclo " + CicloLungo.IdMemoriaLunga.ToString();
                //Programma
                txtNumProgramma.Text = CicloLungo.ProgrammaAttivo.IdProgramma.ToString();
                txtCapacitaNominale.Text = FunzioniMR.StringaCorrente( (short)CicloLungo.ProgrammaAttivo.BatteryAhdef );
                txtTensioneNominale.Text = FunzioniMR.StringaTensione( CicloLungo.ProgrammaAttivo.BatteryVdef);
                txtCelleV1.Text = CicloLungo.ProgrammaAttivo.BatteryCell1.ToString();
                txtCelleV2.Text = CicloLungo.ProgrammaAttivo.BatteryCell2.ToString();
                txtCelleV3.Text = CicloLungo.ProgrammaAttivo.BatteryCell3.ToString();
                txtCelleTot.Text = CicloLungo.ProgrammaAttivo.BatteryCells.ToString();

                //CicloLungo.CalcolaIntermediBrevi();

                //Calcolo la matrice attivazione celle
                AttivaIntermedi[0] = (CicloLungo.ProgrammaAttivo.BatteryCells > 0);
                AttivaIntermedi[1] = (CicloLungo.ProgrammaAttivo.BatteryCell1 > 0);
                AttivaIntermedi[2] = (CicloLungo.ProgrammaAttivo.BatteryCell2 > 0);
                AttivaIntermedi[3] = (CicloLungo.ProgrammaAttivo.BatteryCell3 > 0);

                // Poi la tabella
                Log.Debug("MostraCicli rigeneraListaCicli");
                rigeneraListaCicli();
                mascheraColonne(0, false);
                bool TempoRelativo = chkIntervalloRelativo.Checked;
                Log.Debug("MostraCicli prepara grafici");

                switch (CicloLungo.TipoEvento )
                {
                                    
                    case (byte)SerialMessage.TipoCiclo.Carica:
                        _Flag = "Carica";
                        Log.Debug("MostraCicli grafici 1");
                        VisualizzaGrafici(true);
                        Log.Debug("MostraCicli grafici 2");
                        //GraficoCiclo(CicloLungo.TipoEvento, TempoRelativo);
                        Log.Debug("MostraCicli grafici 3");
                        GraficoCicloOxy(CicloLungo.TipoEvento, TempoRelativo);
                        Log.Debug("MostraCicli grafici 4");
                        GraficoTensioniCicloOxy(CicloLungo.TipoEvento, TempoRelativo);
                        Log.Debug("MostraCicli grafici 5");
                        break;
                    case (byte)SerialMessage.TipoCiclo.Scarica:
                        _Flag = "Scarica";
                        VisualizzaGrafici(true);
                        //GraficoCiclo(CicloLungo.TipoEvento, TempoRelativo);
                        GraficoCicloOxy(CicloLungo.TipoEvento, TempoRelativo);
                        GraficoTensioniCicloOxy(CicloLungo.TipoEvento, TempoRelativo);
                        break;
                    case (byte)SerialMessage.TipoCiclo.Pausa:
                        _Flag = "Pausa";
                        VisualizzaGrafici(false);
                        break;
                    default:
                        _Flag = "Evento Anomalo (" + CicloLungo.TipoEvento.ToString("x2") + ")";
                        VisualizzaGrafici(false);
                        break;
                
                }
                Log.Debug("Fine MostraCicli");

            }
            catch (Exception Ex)
            {
                Log.Error("MostraCicli: " + Ex.Message);
            }

        }

        /// <summary>
        /// Inizializza i controlli grafici OXYplot per andamento Ciclo e Tensioni
        /// </summary>
        private void InizializzaOxyPlotControl()
        {
            try
            {
                //---------------------------------------------------------------
                //   Grafico Andamento Ciclo
                //---------------------------------------------------------------
                this.oxyContainerCiclo = new OxyPlot.WindowsForms.PlotView();
                //this.SuspendLayout();
                // 
                // plot1
                // 
                this.oxyContainerCiclo.Location = new System.Drawing.Point(10, 10);
                //this.oxyContainer.Margin = new System.Windows.Forms.Padding(0);
                this.oxyContainerCiclo.Name = "oxyContainerCiclo";
                this.oxyContainerCiclo.PanCursor = System.Windows.Forms.Cursors.Hand;
                this.oxyContainerCiclo.Size = new System.Drawing.Size(517, 452);
                //this.oxyContainer.TabIndex = 0;
                this.oxyContainerCiclo.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
                this.oxyContainerCiclo.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
                this.oxyContainerCiclo.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
                this.oxyContainerCiclo.Click += new System.EventHandler(this.oxyContainerCiclo_Click);
                // 

                tbpAndamentoOxy.Controls.Add(this.oxyContainerCiclo);
                
                oxyGraficoCiclo = new OxyPlot.PlotModel
                {
                    Title = "",
                    Subtitle = "",
                    PlotType = OxyPlot.PlotType.XY,
                    Background = OxyPlot.OxyColors.White
                };

                oxyContainerCiclo.Model = oxyGraficoCiclo;

                //---------------------------------------------------------------
                //   Grafico Andamento Tensioni
                //---------------------------------------------------------------
                this.oxyContainerTensioni = new OxyPlot.WindowsForms.PlotView();
                //this.SuspendLayout();
                // 
                // plot1
                // 
                this.oxyContainerTensioni.Location = new System.Drawing.Point(10, 10);
                //this.oxyContainer.Margin = new System.Windows.Forms.Padding(0);
                this.oxyContainerTensioni.Name = "oxyContainerTensioni";
                this.oxyContainerTensioni.PanCursor = System.Windows.Forms.Cursors.Hand;
                this.oxyContainerTensioni.Size = new System.Drawing.Size(517, 452);
                //this.oxyContainer.TabIndex = 0;
                this.oxyContainerTensioni.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
                this.oxyContainerTensioni.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
                this.oxyContainerTensioni.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
                this.oxyContainerTensioni.Click += new System.EventHandler(this.oxyContainerTensioni_Click);
                // 

                tbpTensioniCiclo.Controls.Add(this.oxyContainerTensioni);

                oxyGraficoTensioni = new OxyPlot.PlotModel
                {
                    Title = "",
                    Subtitle = "",
                    PlotType = OxyPlot.PlotType.XY,
                    Background = OxyPlot.OxyColors.White
                };

                oxyContainerTensioni.Model = oxyGraficoTensioni;


            }

            catch (Exception Ex)
            {
                Log.Error("InizializzaOxyPlotControl: " + Ex.Message);
            }

        }

        private void oxyContainerCiclo_Click(object sender, EventArgs e)
        {
        }

        private void oxyContainerTensioni_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Creates Y axis for the specified series.
        /// </summary>
        /// <param name="chart">Chart control.</param>
        /// <param name="area">Original chart area.</param>
        /// <param name="series">Series.</param>
        /// <param name="axisOffset">New Y axis offset in relative coordinates.</param>
        /// <param name="labelsSize">Extra space for new Y axis labels in relative coordinates.</param>
        public void CreateYAxis(Chart chart, ChartArea area, Series series, float axisOffset, float labelsSize, string Titolo)
        {
            // Create new chart area for original series
            ChartArea areaSeries = chart.ChartAreas.Add("ChartArea_" + series.Name);
            areaSeries.BackColor = Color.Transparent;
            areaSeries.BorderColor = Color.Transparent;
            areaSeries.Position.FromRectangleF(area.Position.ToRectangleF());
            areaSeries.InnerPlotPosition.FromRectangleF(area.InnerPlotPosition.ToRectangleF());
            areaSeries.AxisX.MajorGrid.Enabled = false;
            areaSeries.AxisX.MajorTickMark.Enabled = false;
            areaSeries.AxisX.LabelStyle.Enabled = false;
            areaSeries.AxisY.MajorGrid.Enabled = false;
            areaSeries.AxisY.MajorTickMark.Enabled = false;
            areaSeries.AxisY.LabelStyle.Enabled = false;
            areaSeries.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;

            series.ChartArea = areaSeries.Name;

            // Create new chart area for axis
            ChartArea areaAxis = chart.ChartAreas.Add("AxisY_" + series.ChartArea);
            areaAxis.BackColor = Color.Transparent;
            areaAxis.BorderColor = Color.Transparent;
            areaAxis.Position.FromRectangleF(chart.ChartAreas[series.ChartArea].Position.ToRectangleF());
            areaAxis.InnerPlotPosition.FromRectangleF(chart.ChartAreas[series.ChartArea].InnerPlotPosition.ToRectangleF());

            // Create a copy of specified series
            Series seriesCopy = chart.Series.Add(series.Name + "_Copy");
            seriesCopy.ChartType = series.ChartType;
            foreach (DataPoint point in series.Points)
            {
                seriesCopy.Points.AddXY(point.XValue, point.YValues[0]);
            }

            // Hide copied series
            seriesCopy.IsVisibleInLegend = false;
            seriesCopy.Color = Color.Transparent;
            seriesCopy.BorderColor = Color.Transparent;
            seriesCopy.ChartArea = areaAxis.Name;

            // Disable grid lines & tickmarks
            areaAxis.AxisX.LineWidth = 0;
            areaAxis.AxisX.MajorGrid.Enabled = false;
            areaAxis.AxisX.MajorTickMark.Enabled = false;
            areaAxis.AxisX.LabelStyle.Enabled = false;
            areaAxis.AxisY.MajorGrid.Enabled = false;
            areaAxis.AxisY.IsStartedFromZero = area.AxisY.IsStartedFromZero;

            // Adjust area position
            areaAxis.Position.X -= axisOffset;
            areaAxis.InnerPlotPosition.X += labelsSize;
            if (Titolo != "")
            {
                // Set axis title
                areaAxis.AxisY.TextOrientation = TextOrientation.Horizontal;
                areaAxis.AxisY.Title = Titolo;
                // Set Title font
                areaAxis.AxisY.TitleFont = new Font("Utopia", 10, FontStyle.Bold);
                // Set Title color
                areaAxis.AxisY.TitleForeColor = Color.Black;
                areaAxis.AxisY.IsLabelAutoFit = false;
                areaAxis.AxisY.LabelStyle.Font = new Font("Utopia", 8, FontStyle.Regular);
                areaAxis.AxisY.TitleAlignment = StringAlignment.Far;
            }

        }


        /// <summary>
        /// GraficoCicloOxy : 
        /// </summary>
        /// <param name="TipoCiclo">Tipo ciclo lungo da visualizzare</param>
        /// <param name="TempoRelativo">Se true Orari di registrazione relativi</param>
        public void GraficoCicloOxy(byte TipoCiclo, bool TempoRelativo = false)
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;
                TimeSpan _IntervalloLettura;
                DateTime _minData = DateTime.MaxValue;
                DateTime _maxData = DateTime.MinValue;

                TimeSpan _minTime = TimeSpan.MaxValue;
                TimeSpan _maxTime = TimeSpan.MinValue;

                double _vMin = double.MaxValue;
                double _vMax = double.MinValue;
                double _iMin = double.MaxValue;
                double _iMax = double.MinValue;
                double _tMin = double.MaxValue;
                double _tMax = double.MinValue;

                double _tempVal;

                oxyGraficoCiclo.InvalidatePlot(false); 

                DateTime _istanteLettura;

                switch (TipoCiclo)
                {

                    case 0xF0:
                        _Flag = "Carica";
                        _titoloGrafico = "Ciclo di Carica";
                        _fattoreCorrente = 1;

                        break;
                    case 0x0F:
                        _Flag = "Scarica";
                        _titoloGrafico = "Ciclo di Scarica";
                        _fattoreCorrente = -1;
                        break;

                    default:
                        _Flag = "Altro";
                        _titoloGrafico = "";
                        break;

                }

                if (TempoRelativo == true)
                {
                    _modelloIntervallo = "HH:mm";
                }
                else
                {
                    _modelloIntervallo = "dd-MM-yy HH:mm";
                }

                tbpAndamentoOxy.BackColor = Color.LightYellow;

                // Preparo le serie di valori

                ValoriCicloTensioni.Clear();
                ValoriCicloCorrenti.Clear();
                ValoriCicloTemp.Clear();

                 _minData = DateTime.MaxValue;
                 _maxData = DateTime.MinValue;

                 _vMin = 0; // double.MaxValue;
                 _vMax = double.MinValue;
                 _iMin = 0; // double.MaxValue;
                 _iMax = double.MinValue;
                 _tMin = 0; // double.MaxValue;
                 _tMax = double.MinValue;

                 Log.Debug("GraficoCiclo: Inizio loop dati");

                 foreach (sbMemBreve lettura in CicloLungo.CicliMemoriaBreve)
                 {



                     _IntervalloLettura = lettura.dtDataOraRegistrazione.Subtract(CicloLungo.dtDataOraStart);
                     if (_IntervalloLettura < _minTime) _minTime = _IntervalloLettura;
                     if (_IntervalloLettura > _maxTime) _maxTime = _IntervalloLettura;


                     _istanteLettura = lettura.dtDataOraRegistrazione;
                     if (_istanteLettura < _minData) _minData = _istanteLettura;
                     if (_istanteLettura > _maxData) _maxData = _istanteLettura;


                     //***************************************************************************************

                     _tempVal = (double)(lettura.VcBatt / 100);
                     ValoriCicloTensioni.Add(new mrDataPoint { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });

                     if (_tempVal < _vMin) _vMin = _tempVal;
                     if (_tempVal > _vMax) _vMax = _tempVal;


                     //***************************************************************************************

                     _tempVal = (double)(_fattoreCorrente * lettura.Amed / 10);
                     ValoriCicloCorrenti.Add(new mrDataPoint { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
                     if (_tempVal < _iMin) _iMin = _tempVal;
                     if (_tempVal > _iMax) _iMax = _tempVal;

                     //***************************************************************************************

                     _tempVal = (double)(lettura.Tntc);
                     ValoriCicloTemp.Add(new mrDataPoint { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
                     if (_tempVal < _tMin) _tMin = _tempVal;
                     if (_tempVal > _tMax) _tMax = _tempVal;

                 }
                // ******************************************************************************************************************************************************

                _vMax = _vMax * 1.25;
                _iMax = _iMax * 1.25;
                _tMax = _tMax * 1.25;

                Log.Debug("GraficoCiclo: fine loop dati");


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                oxyGraficoCiclo.Series.Clear();
                oxyGraficoCiclo.Axes.Clear();

                oxyGraficoCiclo.Background = OxyPlot.OxyColors.LightYellow;
                oxyGraficoCiclo.PlotAreaBackground = OxyPlot.OxyColors.White;
                oxyGraficoCiclo.PlotAreaBorderThickness = new OxyPlot.OxyThickness(3, 3, 3, 3);
               

                oxyGraficoCiclo.Title = _titoloGrafico;
                oxyGraficoCiclo.TitleFont = "Utopia";
                oxyGraficoCiclo.TitleFontSize = 18;

                if (TempoRelativo == true)
                {
                    _modelloIntervallo = "HH:mm";
                }
                else
                {
                    _modelloIntervallo = "dd-MM-yy HH:mm";
                }
                Log.Debug("GraficoCiclo: fine testata inizio serie");
                //Creo le serie:
                OxyPlot.Series.LineSeries serTensione = new OxyPlot.Series.LineSeries();
                serTensione.Title = "Tensione per Cella";
                if (TempoRelativo == true)
                {
                    serTensione.DataFieldX = "TimeLapse";
                    serTensione.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.###} V\n\nIstante:" + " {2:hh\\:mm\\:ss}\n";
                }
                else
                {
                    serTensione.DataFieldX = "Date";
                    serTensione.TrackerFormatString = serTensione.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nV med={4:0.0} V";

                }
                serTensione.DataFieldY = "Value";
                serTensione.Color =  OxyPlot.OxyColors.Blue;
//                serTensione.TrackerFormatString = serTensione.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nV med={4:0.0} V";
//                serTensione.TrackerFormatString = serTensione.Title + "\n\nIstante:"  + "{0}\n{1}: {2:mm\\:ss\\:ms}\n{3}: {4:0.###} V";
                

                OxyPlot.Series.LineSeries serCorrente = new OxyPlot.Series.LineSeries();
                serCorrente.Title = "Corrente Media";

                if (TempoRelativo == true)
                {
                    serCorrente.DataFieldX = "TimeLapse";
                    serCorrente.TrackerFormatString = serCorrente.Title + "\n\nIstante:" + "{2:hh\\:mm\\:ss}\nI med={4:0.###} A";
                }
                else
                {
                    serCorrente.DataFieldX = "Date";
                    serCorrente.TrackerFormatString = serCorrente.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nI med={4:0.0} A";

                }

                serCorrente.DataFieldY = "Value";
                serCorrente.Color = OxyPlot.OxyColors.Red;
                //serCorrente.TrackerFormatString = serCorrente.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nI med={4:0.0} A";


                OxyPlot.Series.LineSeries serTemperatura = new OxyPlot.Series.LineSeries();
                serTemperatura.Title = "Temperatura Media";

                if (TempoRelativo == true)
                {
                    serTemperatura.DataFieldX = "TimeLapse";
                    serTemperatura.TrackerFormatString = serTemperatura.Title + "\n\nIstante:" + "{2:hh\\:mm\\:ss}\nTemperatura: {4:0.###} °C";
                }
                else
                {
                    serTemperatura.DataFieldX = "Date";
                    serTemperatura.TrackerFormatString = serTemperatura.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nTemperatura: {4:0.0} °C";

                }

                serTemperatura.DataFieldY = "Value";
                serTemperatura.Color = OxyPlot.OxyColors.Green;
                //serTemperatura.TrackerFormatString = serTemperatura.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nTemperatura={4:0.0} °C";

                Log.Debug("GraficoCiclo: fine serie inizio assi");
                if (TempoRelativo == true)
                {
                    string _titoloInizio = "Inizio Fase: ";
                    _titoloInizio += CicloLungo.DataOraStart;
                    OxyPlot.Axes.TimeSpanAxis dtAxisTens = new OxyPlot.Axes.TimeSpanAxis
                    {

                        Position = OxyPlot.Axes.AxisPosition.Bottom,
                        MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                        MinorGridlineStyle = OxyPlot.LineStyle.Dot,
                        //StringFormat = _modelloIntervallo,
                        Minimum = OxyPlot.Axes.TimeSpanAxis.ToDouble(_minTime),
                        Maximum = OxyPlot.Axes.TimeSpanAxis.ToDouble(_maxTime),
                        Key = "Tempi",
                        Title = _titoloInizio,
                        TitleFontSize = 12,
                        TitleFontWeight = OxyPlot.FontWeights.Bold,
                        TitleColor = OxyPlot.OxyColors.Blue,

                    };
                    oxyGraficoCiclo.Axes.Add(dtAxisTens);

                }
                else
                {


                    OxyPlot.Axes.DateTimeAxis dtAxisTens = new OxyPlot.Axes.DateTimeAxis

                    {
                        CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek,
                        FirstDayOfWeek = DayOfWeek.Monday,
                        Position = OxyPlot.Axes.AxisPosition.Bottom,
                        MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                        MinorGridlineStyle = OxyPlot.LineStyle.Dot,
                        StringFormat = _modelloIntervallo,
                        Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(_minData),
                        Maximum = OxyPlot.Axes.DateTimeAxis.ToDouble(_maxData),
                        IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                        Key = "Tempi",
                    };

                    oxyGraficoCiclo.Axes.Add(dtAxisTens);

                }


                OxyPlot.Axes.LinearAxis VAxisTens = new OxyPlot.Axes.LinearAxis
                {
                    Position = OxyPlot.Axes.AxisPosition.Left, 
                    MajorGridlineStyle = OxyPlot.LineStyle.Solid, 
                   // MinorGridlineStyle = OxyPlot.LineStyle.Dot, 
                    TickStyle = OxyPlot.Axes.TickStyle.Outside,
                    Minimum = _vMin,
                    Maximum = _vMax,
                    PositionTier = 0,
                    //Title = "Tensione Media per Cella (V)",
                    Unit = " V ",
                    //TitlePosition = -4,
                    TitleFontSize = 12,
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    AxisDistance = 15,
                    AxislineStyle = OxyPlot.LineStyle.Solid,
                    Key = "Tensione",
                    MajorGridlineColor = OxyPlot.OxyColors.LightBlue,
                    MinorGridlineColor = OxyPlot.OxyColors.LightBlue,
                };



                
                OxyPlot.Axes.LinearAxis AAxisCorr = new OxyPlot.Axes.LinearAxis
                {
                    Position = OxyPlot.Axes.AxisPosition.Left, 
                    MajorGridlineStyle = OxyPlot.LineStyle.Solid, 
                    //MinorGridlineStyle = OxyPlot.LineStyle.Dot, 
                    TickStyle = OxyPlot.Axes.TickStyle.Outside,
                    Minimum = _iMin,
                    Maximum = _iMax,
                    PositionTier = 1,
                    Unit = " A ", //"Corrente Media (A)",
                    TitleFontSize = 12,
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    AxisDistance = 15,
                    AxislineStyle = OxyPlot.LineStyle.Solid,
                    Key = "Corrente",
                    MajorGridlineColor = OxyPlot.OxyColors.LightPink,
                    MinorGridlineColor = OxyPlot.OxyColors.LightPink,


                };

                OxyPlot.Axes.LinearAxis CAxisTemp = new OxyPlot.Axes.LinearAxis
                {
                    Position = OxyPlot.Axes.AxisPosition.Left,
                    MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                    //MinorGridlineStyle = OxyPlot.LineStyle.Dot,
                    TickStyle = OxyPlot.Axes.TickStyle.Outside,
                    Minimum = _tMin,
                    Maximum = _tMax,
                    PositionTier = 2,
                    Unit = " °C ",
                    TitleFontSize = 12,
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    AxisDistance = 15,
                    AxislineStyle = OxyPlot.LineStyle.Solid,
                    Key = "Temperatura",
                    MajorGridlineColor = OxyPlot.OxyColors.LightGreen,
                    MinorGridlineColor = OxyPlot.OxyColors.LightGreen,

                };

                Log.Debug("GraficoCiclo: fine definizioni aggiunta elementi");

                oxyGraficoCiclo.Axes.Add(VAxisTens);
                oxyGraficoCiclo.Axes.Add(AAxisCorr);
                oxyGraficoCiclo.Axes.Add(CAxisTemp);

                serTensione.XAxisKey = "Tempi";
                serTensione.YAxisKey = "Tensione";
                serTensione.ItemsSource = ValoriCicloTensioni;
                oxyGraficoCiclo.Series.Add(serTensione);

                serCorrente.XAxisKey = "Tempi";
                serCorrente.YAxisKey = "Corrente"; 
                serCorrente.ItemsSource = ValoriCicloCorrenti;
                oxyGraficoCiclo.Series.Add(serCorrente);

                serTemperatura.XAxisKey = "Tempi";
                serTemperatura.YAxisKey = "Temperatura";
                serTemperatura.ItemsSource = ValoriCicloTemp;
                oxyGraficoCiclo.Series.Add(serTemperatura);

                oxyGraficoCiclo.InvalidatePlot(true);
                Log.Debug("GraficoCiclo: fine generazione");
            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }


        /// <summary>
        /// GraficoCicloOxy : 
        /// </summary>
        /// <param name="TipoCiclo">Tipo ciclo lungo da visualizzare</param>
        /// <param name="TempoRelativo">Se true Orari di registrazione relativi</param>
        public void GraficoTensioniCicloOxy(byte TipoCiclo, bool TempoRelativo = false)
        {
            try
            {
                string _Flag;
                string _titoloGrafico = "";
                string _modelloIntervallo;
                double _fattoreCorrente = 0;
                double _dtInSecondi;
                bool[] _serEnabled = new bool[5];
                TimeSpan _IntervalloLettura;
                DateTime _minData = DateTime.MaxValue;
                DateTime _maxData = DateTime.MinValue;

                TimeSpan _minTime = TimeSpan.MaxValue;
                TimeSpan _maxTime = TimeSpan.MinValue;

                double _vMin = double.MaxValue;
                double _vMax = double.MinValue;
                double _iMin = double.MaxValue;
                double _iMax = double.MinValue;
                double _tMin = double.MaxValue;
                double _tMax = double.MinValue;

                double _tempVal;

                oxyGraficoTensioni.InvalidatePlot(false);

                DateTime _istanteLettura;

                switch (TipoCiclo)
                {

                    case 0xF0:
                        _Flag = "Carica";
                        _titoloGrafico = "Grafico Tensioni in Carica";
                        _fattoreCorrente = 1;

                        break;
                    case 0x0F:
                        _Flag = "Scarica";
                        _titoloGrafico = "Grafico Tensioni in Scarica";
                        _fattoreCorrente = -1;
                        break;

                    default:
                        _Flag = "Altro";
                        _titoloGrafico = "";
                        break;

                }

                if (TempoRelativo == true)
                {
                    _modelloIntervallo = "HH:mm";
                }
                else
                {
                    _modelloIntervallo = "dd-MM-yy HH:mm";
                }

                tbpTensioniCiclo.BackColor = Color.LightYellow;


                _serEnabled[0] = true; // la serie sulla tensione media di batteria è sempre presente
                _serEnabled[1] = false; //Disattivo le altre serie: si attivano se ho valori != 0
                _serEnabled[2] = false;
                _serEnabled[3] = false;
                _serEnabled[4] = false;



                // Preparo le serie di valori

                ValoriTensioniCellaTot.Clear();
                ValoriTensioniCellaS1.Clear();
                ValoriTensioniCellaS2.Clear();
                ValoriTensioniCellaS3.Clear();
                ValoriTensioniCellaS4.Clear();



                _minData = DateTime.MaxValue;
                _maxData = DateTime.MinValue;

                // unico v min e v max per tutte le serie
                _vMin = 0; 
                _vMax = double.MinValue;

                _iMin = 0; // double.MaxValue;
                _iMax = double.MinValue;
                _tMin = 0; // double.MaxValue;
                _tMax = double.MinValue;

                Log.Debug("GraficoCiclo: Inizio loop dati");

                foreach (sbMemBreve lettura in CicloLungo.CicliMemoriaBreve)
                {

                    _IntervalloLettura = lettura.dtDataOraRegistrazione.Subtract(CicloLungo.dtDataOraStart);
                    if (_IntervalloLettura < _minTime) _minTime = _IntervalloLettura;
                    if (_IntervalloLettura > _maxTime) _maxTime = _IntervalloLettura;


                    _istanteLettura = lettura.dtDataOraRegistrazione;
                    if (_istanteLettura < _minData) _minData = _istanteLettura;
                    if (_istanteLettura > _maxData) _maxData = _istanteLettura;


                    //************************************ Tensioni Cella Totali

                    _tempVal = (double)(lettura.VcBatt / 100);
                    ValoriTensioniCellaTot.Add(new mrDataPoint { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });

                    if (_tempVal < _vMin) _vMin = _tempVal;
                    if (_tempVal > _vMax) _vMax = _tempVal;

                    //************************************ Tensioni Cella S1

                    _tempVal = (double)(lettura.Vcs1 / 100);
                    ValoriTensioniCellaS1.Add(new mrDataPoint { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
                    if (_tempVal > 0)
                    {
                        _serEnabled[1] = true;
                        if (_tempVal < _vMin) _vMin = _tempVal;
                    }
                    if (_tempVal > _vMax) _vMax = _tempVal;

                    //************************************ Tensioni Cella S2

                    _tempVal = (double)(lettura.Vcs2 / 100);
                    ValoriTensioniCellaS2.Add(new mrDataPoint { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
                    if (_tempVal > 0)
                    {
                        _serEnabled[2] = true;
                        if (_tempVal < _vMin) _vMin = _tempVal;
                    }
                    if (_tempVal > _vMax) _vMax = _tempVal;

                    //************************************ Tensioni Cella S3

                    _tempVal = (double)(lettura.Vcs3 / 100);
                    ValoriTensioniCellaS3.Add(new mrDataPoint { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
                    if (_tempVal > 0)
                    {
                        _serEnabled[3] = true;
                        if (_tempVal < _vMin) _vMin = _tempVal;
                    }
                    if (_tempVal > _vMax) _vMax = _tempVal;

                    //************************************ Tensioni Cella S1

                    _tempVal = (double)(lettura.VcsBatt/ 100);
                    ValoriTensioniCellaS4.Add(new mrDataPoint { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
                    if (_tempVal > 0)
                    {
                        _serEnabled[4] = true;
                        if (_tempVal < _vMin) _vMin = _tempVal;
                    }
                    if (_tempVal > _vMax) _vMax = _tempVal;

                }
                // ******************************************************************************************************************************************************

                _vMax = _vMax * 1.25;
                _iMax = _iMax * 1.25;
                _tMax = _tMax * 1.25;

                Log.Debug("GraficoCiclo: fine loop dati");


                // Inizializzo il controllo OxyPlot.PlotModel ( oxyGraficoCiclo )
                oxyGraficoTensioni.Series.Clear();
                oxyGraficoTensioni.Axes.Clear();

                oxyGraficoTensioni.Background = OxyPlot.OxyColors.LightYellow;
                oxyGraficoTensioni.PlotAreaBackground = OxyPlot.OxyColors.White;
                oxyGraficoTensioni.PlotAreaBorderThickness = new OxyPlot.OxyThickness(3, 3, 3, 3);


                oxyGraficoTensioni.Title = _titoloGrafico;
                oxyGraficoTensioni.TitleFont = "Utopia";
                oxyGraficoTensioni.TitleFontSize = 18;

                if (TempoRelativo == true)
                {
                    _modelloIntervallo = "HH:mm";
                }
                else
                {
                    _modelloIntervallo = "dd-MM-yy HH:mm";
                }

                
                //Creo le serie:

                // Tensione Media Batteria
                OxyPlot.Series.LineSeries serTensione = new OxyPlot.Series.LineSeries();
                serTensione.Title = "Tensione per Cella";
                if (TempoRelativo == true)
                {
                    serTensione.DataFieldX = "TimeLapse";
                    serTensione.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.###} V\n\nIstante:" + " {2:hh\\:mm\\:ss}\n";
                }
                else
                {
                    serTensione.DataFieldX = "Date";
                    serTensione.TrackerFormatString = serTensione.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nV med={4:0.0} V";

                }
                serTensione.DataFieldY = "Value";
                serTensione.Color = OxyPlot.OxyColors.Blue;



                // Tensione Sezione 1
                OxyPlot.Series.LineSeries serTensSez1 = new OxyPlot.Series.LineSeries();
                serTensSez1.Title = "Tensione per Cella / Sezione 1";
                if (TempoRelativo == true)
                {
                    serTensSez1.DataFieldX = "TimeLapse";
                    serTensSez1.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.###} V\n\nIstante:" + " {2:hh\\:mm\\:ss}\n";
                }
                else
                {
                    serTensSez1.DataFieldX = "Date";
                    serTensSez1.TrackerFormatString = serTensione.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nV med={4:0.0} V";

                }
                serTensSez1.DataFieldY = "Value";
                serTensSez1.Color = OxyPlot.OxyColors.Cyan;

                // Tensione Sezione 2
                OxyPlot.Series.LineSeries serTensSez2 = new OxyPlot.Series.LineSeries();
                serTensSez2.Title = "Tensione per Cella / Sezione 2";
                if (TempoRelativo == true)
                {
                    serTensSez2.DataFieldX = "TimeLapse";
                    serTensSez2.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.###} V\n\nIstante:" + " {2:hh\\:mm\\:ss}\n";
                }
                else
                {
                    serTensSez2.DataFieldX = "Date";
                    serTensSez2.TrackerFormatString = serTensione.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nV med={4:0.0} V";

                }
                serTensSez2.DataFieldY = "Value";
                serTensSez2.Color = OxyPlot.OxyColors.Red;


                // Tensione Sezione 3
                OxyPlot.Series.LineSeries serTensSez3 = new OxyPlot.Series.LineSeries();
                serTensSez3.Title = "Tensione per Cella / Sezione 3";
                if (TempoRelativo == true)
                {
                    serTensSez3.DataFieldX = "TimeLapse";
                    serTensSez3.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.###} V\n\nIstante:" + " {2:hh\\:mm\\:ss}\n";
                }
                else
                {
                    serTensSez3.DataFieldX = "Date";
                    serTensSez3.TrackerFormatString = serTensione.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nV med={4:0.0} V";

                }
                serTensSez3.DataFieldY = "Value";
                serTensSez3.Color = OxyPlot.OxyColors.Green;


                // Tensione Sezione 4
                OxyPlot.Series.LineSeries serTensSez4 = new OxyPlot.Series.LineSeries();
                serTensSez4.Title = "Tensione per Cella / Sezione 4";
                if (TempoRelativo == true)
                {
                    serTensSez4.DataFieldX = "TimeLapse";
                    serTensSez4.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.###} V\n\nIstante:" + " {2:hh\\:mm\\:ss}\n";
                }
                else
                {
                    serTensSez4.DataFieldX = "Date";
                    serTensSez4.TrackerFormatString = serTensione.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nV med={4:0.0} V";

                }
                serTensSez4.DataFieldY = "Value";
                serTensSez4.Color = OxyPlot.OxyColors.Brown;




                OxyPlot.Series.LineSeries serTemperatura = new OxyPlot.Series.LineSeries();
                serTemperatura.Title = "Temperatura Media";

                if (TempoRelativo == true)
                {
                    serTemperatura.DataFieldX = "TimeLapse";
                    serTemperatura.TrackerFormatString = serTemperatura.Title + "\n\nIstante:" + "{2:hh\\:mm\\:ss}\nTemperatura: {4:0.###} °C";
                }
                else
                {
                    serTemperatura.DataFieldX = "Date";
                    serTemperatura.TrackerFormatString = serTemperatura.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nTemperatura: {4:0.0} °C";

                }

                serTemperatura.DataFieldY = "Value";
                serTemperatura.Color = OxyPlot.OxyColors.Cornsilk;

                //serTemperatura.TrackerFormatString = serTemperatura.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nTemperatura={4:0.0} °C";

                Log.Debug("GraficoCiclo: fine serie inizio assi");
                if (TempoRelativo == true)
                {
                    string _titoloInizio = "Inizio Fase: ";
                    _titoloInizio += CicloLungo.DataOraStart;

                    OxyPlot.Axes.TimeSpanAxis dtAxisTens = new OxyPlot.Axes.TimeSpanAxis
                    {

                        Position = OxyPlot.Axes.AxisPosition.Bottom,
                        MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                        MinorGridlineStyle = OxyPlot.LineStyle.Dot,
                        //StringFormat = _modelloIntervallo,
                        Minimum = OxyPlot.Axes.TimeSpanAxis.ToDouble(_minTime),
                        Maximum = OxyPlot.Axes.TimeSpanAxis.ToDouble(_maxTime),
                        Key = "Tempi",
                        Title = _titoloInizio,
                        TitleFontSize = 12,
                        TitleFontWeight = OxyPlot.FontWeights.Bold,
                    };
                    oxyGraficoTensioni.Axes.Add(dtAxisTens);

                }
                else
                {


                    OxyPlot.Axes.DateTimeAxis dtAxisTens = new OxyPlot.Axes.DateTimeAxis

                    {
                        CalendarWeekRule = CalendarWeekRule.FirstFourDayWeek,
                        FirstDayOfWeek = DayOfWeek.Monday,
                        Position = OxyPlot.Axes.AxisPosition.Bottom,
                        MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                        MinorGridlineStyle = OxyPlot.LineStyle.Dot,
                        StringFormat = _modelloIntervallo,
                        Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(_minData),
                        Maximum = OxyPlot.Axes.DateTimeAxis.ToDouble(_maxData),
                        IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                        Key = "Tempi",
                    };

                    oxyGraficoTensioni.Axes.Add(dtAxisTens);

                }


                OxyPlot.Axes.LinearAxis VAxisTens = new OxyPlot.Axes.LinearAxis
                {
                    Position = OxyPlot.Axes.AxisPosition.Left,
                    MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                    // MinorGridlineStyle = OxyPlot.LineStyle.Dot, 
                    TickStyle = OxyPlot.Axes.TickStyle.Outside,
                    Minimum = _vMin,
                    Maximum = _vMax,
                    PositionTier = 0,
                    //Title = "Tensione Media per Cella (V)",
                    Unit = " V ",
                    //TitlePosition = -4,
                    TitleFontSize = 12,
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    AxisDistance = 15,
                    AxislineStyle = OxyPlot.LineStyle.Solid,
                    Key = "Tensione",
                    MajorGridlineColor = OxyPlot.OxyColors.LightBlue,
                    MinorGridlineColor = OxyPlot.OxyColors.LightBlue,
                };


                
                OxyPlot.Axes.LinearAxis CAxisTemp = new OxyPlot.Axes.LinearAxis
                {
                    Position = OxyPlot.Axes.AxisPosition.Left,
                    MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                    //MinorGridlineStyle = OxyPlot.LineStyle.Dot,
                    TickStyle = OxyPlot.Axes.TickStyle.Outside,
                    Minimum = _tMin,
                    Maximum = _tMax,
                    PositionTier = 2,
                    Unit = " °C ",
                    TitleFontSize = 12,
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    AxisDistance = 15,
                    AxislineStyle = OxyPlot.LineStyle.Solid,
                    Key = "Temperatura",
                    MajorGridlineColor = OxyPlot.OxyColors.LightGreen,
                    MinorGridlineColor = OxyPlot.OxyColors.LightGreen,

                };
                
                Log.Debug("GraficoCiclo: fine definizioni aggiunta elementi");

                oxyGraficoTensioni.Axes.Add(VAxisTens);
                //oxyGraficoCiclo.Axes.Add(AAxisCorr);
                oxyGraficoTensioni.Axes.Add(CAxisTemp);

                serTensione.XAxisKey = "Tempi";
                serTensione.YAxisKey = "Tensione";
                serTensione.ItemsSource = ValoriTensioniCellaTot;
                oxyGraficoTensioni.Series.Add(serTensione);


                serTensSez1.XAxisKey = "Tempi";
                serTensSez1.YAxisKey = "Tensione";
                serTensSez1.ItemsSource = ValoriTensioniCellaS1;
                oxyGraficoTensioni.Series.Add(serTensSez1);

                serTensSez2.XAxisKey = "Tempi";
                serTensSez2.YAxisKey = "Tensione";
                serTensSez2.ItemsSource = ValoriTensioniCellaS2;
                oxyGraficoTensioni.Series.Add(serTensSez2);

                serTensSez3.XAxisKey = "Tempi";
                serTensSez3.YAxisKey = "Tensione";
                serTensSez3.ItemsSource = ValoriTensioniCellaS3;
                oxyGraficoTensioni.Series.Add(serTensSez3);

                serTensSez4.XAxisKey = "Tempi";
                serTensSez4.YAxisKey = "Tensione";
                serTensSez4.ItemsSource = ValoriTensioniCellaS4;
                oxyGraficoTensioni.Series.Add(serTensSez4);

                serTemperatura.XAxisKey = "Tempi";
                serTemperatura.YAxisKey = "Temperatura";
                serTemperatura.ItemsSource = ValoriCicloTemp;



                //oxyGraficoTensioni.Series.Add(serTemperatura);

                oxyGraficoTensioni.InvalidatePlot(true);
                Log.Debug("GraficoCiclo: fine generazione");
            }

            catch (Exception Ex)
            {
                Log.Error("GraficoCiclo: " + Ex.Message);
            }

        }
   



        private bool mascheraColonne(int sezione, bool visibile)
        {
            foreach (OLVColumn Colonna in flvwCicliBrevi.Columns)
            {
                Log.Debug(Colonna.Index.ToString() + " - " + Colonna.Name);
            }

            return true;
        }

        private void btnChiudi_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSfoglia_Click(object sender, EventArgs e)
        {
            sfdNuovoCSV.Filter = "Comma Separed (*.csv)|*.csv|All files (*.*)|*.*";
            sfdNuovoCSV.ShowDialog();
            txtNuovoFile.Text = sfdNuovoCSV.FileName;

        }

        private void btnGeneraCsv_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "";
                int _ciclo = 0;


                if (txtNuovoFile.Text != "")
                {
                    filePath = txtNuovoFile.Text;
                    if (!File.Exists(filePath))
                    {
                        File.Create(filePath).Close();
                    }
                    string delimiter = ";";
                    string[][] output = new string[][]
                    {
                         new string[]{"Progr","Num Ev.","Data/Ora","V batt","V 3", "V 2", "V 1", "Vc Batt","Vc 3", "Vc 2", "Vc 1", "Vcs Batt","Vcs 3", "Vcs 2", "Vcs 1","I med", "Imin","IMax","Elettrolita", "Temp","V Bk" } /*add the values that you want inside a csv file. Mostly this function can be used in a foreach loop.*/
                    };

                    int length = output.GetLength(0);
                    StringBuilder sb = new StringBuilder();
                    for (int index = 0; index < length; index++)
                        sb.AppendLine(string.Join(delimiter, output[index]));
                    File.AppendAllText(filePath, sb.ToString());



                    int _elementi = CicliMemoriaBreve.Count;
                    ListViewItem itm;
                    string[] arr;

                    foreach (sbMemBreve _evento in CicloLungo.CicliMemoriaBreve)
                    {

                        output = new string[][]
                          {
                             new string[]{ _evento.IdMemoriaLunga.ToString(),
                                           _evento.IdMemoriaBreve.ToString(),
                                           _evento.DataOraRegistrazione,
                                           _evento.strVreg,
                                           _evento.strV3,
                                           _evento.strV2,
                                           _evento.strV1,
                                           _evento.ValoriIntermedi.strTensioniCellaAssolute.Vbatt,
                                           _evento.ValoriIntermedi.strTensioniCellaAssolute.V3,
                                           _evento.ValoriIntermedi.strTensioniCellaAssolute.V2,
                                           _evento.ValoriIntermedi.strTensioniCellaAssolute.V1, 
                                           _evento.ValoriIntermedi.strTensioniCellaRelative.Vbatt,
                                           _evento.ValoriIntermedi.strTensioniCellaRelative.V3,
                                           _evento.ValoriIntermedi.strTensioniCellaRelative.V2,
                                           _evento.ValoriIntermedi.strTensioniCellaRelative.V1, 
                                           FunzioniMR.StringaCorrenteSigned((short)_evento.Amed ),
                                           FunzioniMR.StringaCorrenteSigned((short)_evento.Amin ), 
                                           FunzioniMR.StringaCorrenteSigned((short)_evento.Amax ),
                                           _evento.PresenzaElettrolita.ToString(),
                                           _evento.strTemp,
                                           _evento.strVbatBk } 
                          };

                        length = output.GetLength(0);
                        sb = new StringBuilder();
                        for (int index = 0; index < length; index++)
                            sb.AppendLine(string.Join(delimiter, output[index]));
                        File.AppendAllText(filePath, sb.ToString());
                        _ciclo++;

                    }



                    MessageBox.Show("File generato", "Esportazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Inserire un nome valido", "Esportazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }

            catch ( Exception Ex )
            {
                Log.Error("btnGeneraCsv_Click: " + Ex.Message);
                MessageBox.Show(Ex.Message, "Esportazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }

        private void frmListaCicliBreve_Load(object sender, EventArgs e)
        {
            frmListaCicliBreve_Resize(sender, e);
        }

        private void txtNuovoFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCelleV1_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkIntervalloRelativo_CheckedChanged(object sender, EventArgs e)
        {
            MostraCicli();
            tabCicloBreve.SelectedTab = tbpDatiCiclo;
            tabCicloBreve.SelectedTab = tbpUtilita;

        }

        private void tbpAndamentoOxy_Click(object sender, EventArgs e)
        {

        }

        private void txtNuovoFile_TextChanged_1(object sender, EventArgs e)
        {

        }
    }

    public class mrDataPoint
    {
        public DateTime Date { get; set; }
        public TimeSpan TimeLapse { get; set; }
        public double Value { get; set; }
    }
}
