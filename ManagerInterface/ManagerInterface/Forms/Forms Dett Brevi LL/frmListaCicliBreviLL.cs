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
    public partial class frmListaCicliBreviLL : Form
    {

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public llMemoriaCicli CicloCorrente;

        public sbMemLunga CicloLungo;
        public System.Collections.Generic.List<MessaggioSpyBatt.MemoriaPeriodoBreve> CicliMemoriaBreve = new System.Collections.Generic.List<MessaggioSpyBatt.MemoriaPeriodoBreve>();
        public MessaggioSpyBatt.MemoriaPeriodoLungo EventoLungo;
        public UnitaSpyBatt _sb;
        public bool[] AttivaIntermedi = new bool[4];
        private OxyPlot.WindowsForms.PlotView oxyContainerCiclo;
        private OxyPlot.PlotModel oxyGraficoCiclo;

        private OxyPlot.WindowsForms.PlotView oxyContainerTensioni;
        private OxyPlot.PlotModel oxyGraficoTensioni;

        public System.Collections.Generic.List<mrDataPointLl> ValoriCicloTensioni = new List<mrDataPointLl>();
        public System.Collections.Generic.List<mrDataPointLl> ValoriCicloCorrenti = new List<mrDataPointLl>();
        public System.Collections.Generic.List<mrDataPointLl> ValoriCicloTemp = new List<mrDataPointLl>();

        public System.Collections.Generic.List<mrDataPointLl> ValoriTensioniCellaTot = new List<mrDataPointLl>();
        public System.Collections.Generic.List<mrDataPointLl> ValoriTensioniCellaS1 = new List<mrDataPointLl>();
        public System.Collections.Generic.List<mrDataPointLl> ValoriTensioniCellaS2= new List<mrDataPointLl>();
        public System.Collections.Generic.List<mrDataPointLl> ValoriTensioniCellaS3 = new List<mrDataPointLl>();
        public System.Collections.Generic.List<mrDataPointLl> ValoriTensioniCellaS4 = new List<mrDataPointLl>();

        private parametriSistema _parametri;


        int _idxAmin;
        int _idxAmax;

        
        public frmListaCicliBreviLL()
        {
            try
            {
                InitializeComponent();
                InizializzaOxyPlotControl();
            }
            catch (Exception Ex)
            {
                Log.Error("frmListaCicliBreve(): " + Ex.Message);
            }
        }


        public parametriSistema parametri
        {
            get
            {
                return _parametri;
            }
            set
            {
                _parametri = value;
            }
        }



        private void frmListaCicliBreve_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.Width > 500)
                {
                    tabCicloBreve.Width = this.Width - 50;

                    flvCicliListaBrevi.Width = this.Width - 90;
                    
                    btnChiudi.Left = this.Width - 200;

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

                    flvCicliListaBrevi.Height = this.Height - 250;
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


        private void InizializzaListaBrevi()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvCicliListaBrevi.HeaderUsesThemes = false;
                flvCicliListaBrevi.HeaderFormatStyle = _stile;
                flvCicliListaBrevi.UseAlternatingBackColors = true;
                flvCicliListaBrevi.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvCicliListaBrevi.AllColumns.Clear();
                flvCicliListaBrevi.RowHeight = 20;
                flvCicliListaBrevi.View = View.Details;
                flvCicliListaBrevi.ShowGroups = false;
                flvCicliListaBrevi.GridLines = true;
                flvCicliListaBrevi.FullRowSelect = true;
                flvCicliListaBrevi.MultiSelect = false;
                flvCicliListaBrevi.GridLines = true;


                BrightIdeasSoftware.OLVColumn colIdMemCiclo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "ID",
                    AspectName = "strIdMemCiclo",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaBrevi.AllColumns.Add(colIdMemCiclo);

                BrightIdeasSoftware.OLVColumn colIdMemBreve = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "breve",
                    AspectName = "strIdMemoriaBreve",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colIdMemBreve);

                BrightIdeasSoftware.OLVColumn colDataOraStart = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Ora",
                    AspectName = "strTimestamp",
                    Width = 100,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colDataOraStart);


                BrightIdeasSoftware.OLVColumn colstrVBatt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Vmed",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strVBatt",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVBatt);

                BrightIdeasSoftware.OLVColumn colstrIBatt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Imed",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strIBatt",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colstrIBatt);

                BrightIdeasSoftware.OLVColumn colstrIBattMin = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Imin",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strIBattMin",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colstrIBattMin);

                BrightIdeasSoftware.OLVColumn colstrIBattMax = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Imax",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strIBattMax",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colstrIBattMax);

                BrightIdeasSoftware.OLVColumn colstrTBatt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "T batt",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strTempBatt",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colstrTBatt);

                BrightIdeasSoftware.OLVColumn colstrTIGBT1 = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "T i1",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strTempIGBT1",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colstrTIGBT1);

                BrightIdeasSoftware.OLVColumn colstrTIGBT2 = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "T i2",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strTempIGBT2",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colstrTIGBT2);

                BrightIdeasSoftware.OLVColumn colstrTIGBT3 = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "T i3",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strTempIGBT3",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colstrTIGBT3);

                BrightIdeasSoftware.OLVColumn colstrTIGBT4 = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "T i4",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strTempIGBT4",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colstrTIGBT4);

                BrightIdeasSoftware.OLVColumn colstrTempDiode = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "T d1",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strTempDiode",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colstrTempDiode);

                BrightIdeasSoftware.OLVColumn colstrVettoreErrori = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Vett Err",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strVettoreErrori",
                    Width = 80,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettoreErrori);

                BrightIdeasSoftware.OLVColumn colstrVettErrCalibr = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Cal.Corr.",
                    IsHeaderVertical = true,
                    ToolTipText = "Errore Calibrazione corrente zero all'accensione",
                    AspectName = "strErrCalib",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })

                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrCalibr);

                BrightIdeasSoftware.OLVColumn colstrVettErrComm = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Comm.",
                    IsHeaderVertical = true,
                    ToolTipText = "Errore Comunicazione con una periferica",
                    AspectName = "strErrComm",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrComm);

                BrightIdeasSoftware.OLVColumn colstrVettErrVBatt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "V Batt.",
                    IsHeaderVertical = true,
                    ToolTipText = "Tensione batteria non corretta alla partenza",
                    AspectName = "strErrVbatt",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrVBatt);


                BrightIdeasSoftware.OLVColumn colstrVettErrInternal = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Internal",
                    IsHeaderVertical = true,
                    ToolTipText = "Anomalia interna dell'apparecchiatura",
                    AspectName = "strErrInt",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrInternal);

                BrightIdeasSoftware.OLVColumn colstrVettErrSpyBatt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Batt.Err.",
                    IsHeaderVertical = true,
                    ToolTipText = "Anomalia da Spy-batt 01",
                    AspectName = "strErrSB1",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrSpyBatt);

                BrightIdeasSoftware.OLVColumn colstrVettErrFuse = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Fuse",
                    IsHeaderVertical = true,
                    ToolTipText = "Fusibile di uscita Aperto",
                    AspectName = "strErrFuse",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrFuse);

                BrightIdeasSoftware.OLVColumn colstrVettErrAlim = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Alim.",
                    IsHeaderVertical = true,
                    ToolTipText = "Mancanza rete o problema alimentazione",
                    AspectName = "strErrAlim",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrAlim);

                BrightIdeasSoftware.OLVColumn colstrVettErrIbat = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "I Batt.",
                    IsHeaderVertical = true,
                    ToolTipText = "Corrente di Batteria",
                    AspectName = "strErrIbat",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrIbat);


                BrightIdeasSoftware.OLVColumn colstrVettErrStrappo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Strappo",
                    IsHeaderVertical = true,
                    ToolTipText = "Batteria staccata mentre era in carica",
                    AspectName = "strErrStrappo",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrStrappo);


                BrightIdeasSoftware.OLVColumn colstrVettErrParam = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "I Batt.",
                    IsHeaderVertical = true,
                    ToolTipText = "Parametri residenti in memoria non correttia",
                    AspectName = "strErrParam",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrParam);


                BrightIdeasSoftware.OLVColumn colstrVettErrParamSB = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Param SB",
                    IsHeaderVertical = true,
                    ToolTipText = "Parametri SpyBatt non adatti al caricabatteria",
                    AspectName = "strErrParamSB",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrParamSB);


                BrightIdeasSoftware.OLVColumn colstrVettErrExtMem = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Memory",
                    IsHeaderVertical = true,
                    ToolTipText = "Memoria esterna non funzionante",
                    AspectName = "strErrMemExt",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrExtMem);

                BrightIdeasSoftware.OLVColumn colstrVettErrInit = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Init.",
                    IsHeaderVertical = true,
                    ToolTipText = "Memoria NON INIZIALIZZATA",
                    AspectName = "strErrNoInit",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrInit);

                BrightIdeasSoftware.OLVColumn colstrVettErrMaxSD = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Max SD",
                    IsHeaderVertical = true,
                    ToolTipText = "Numero di Shut Down superiore al valore massimo consentito",
                    AspectName = "strErrMaxSD",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrMaxSD);

                BrightIdeasSoftware.OLVColumn colstrVettErrIPK = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Max IPK",
                    IsHeaderVertical = true,
                    ToolTipText = "E’ stato rilevato un Interrupt su PIN I_PK ",
                    AspectName = "strErrMaxIPK",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrIPK);

                BrightIdeasSoftware.OLVColumn colstrVettErrPWHole = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Pw Hole",
                    IsHeaderVertical = true,
                    ToolTipText = "Buco di rete (tempo inferiore a 1 secondo)",
                    AspectName = "strErrPwHole",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrPWHole);

                BrightIdeasSoftware.OLVColumn colstrVettErrPWKO = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Ass.Rete",
                    IsHeaderVertical = true,
                    ToolTipText = "Assenza di rete",
                    AspectName = "strErrPwOff",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrPWKO);

                BrightIdeasSoftware.OLVColumn colstrVettErrPreCT = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "T.Fase 0",
                    IsHeaderVertical = true,
                    ToolTipText = "Timer di Pre Ciclo scaduto e tensione non adeguata",
                    AspectName = "strErrTmr0",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrPreCT);

                BrightIdeasSoftware.OLVColumn colstrVettErrTmrF1 = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "T.Fase 1",
                    IsHeaderVertical = true,
                    ToolTipText = "Timer di Fase 1 scaduto e tensione non adeguata",
                    AspectName = "strErrTmr1",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrTmrF1);

                BrightIdeasSoftware.OLVColumn colstrVettErrDispPulse = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Disp.Puls.",
                    IsHeaderVertical = true,
                    ToolTipText = "Pulsanti display in anomalia",
                    AspectName = "strErrDispPulse",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrDispPulse);

                BrightIdeasSoftware.OLVColumn colstrVettErrPFC = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "PFC",
                    IsHeaderVertical = true,
                    ToolTipText = "Tensione PFC anomala",
                    AspectName = "strErrPFC",
                    Width = 20,
                    Sortable = false,      
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })

                };
                flvCicliListaBrevi.AllColumns.Add(colstrVettErrPFC);

                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = " ",
                    AspectName = "",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                    FillsFreeSpace = true,
                };
                flvCicliListaBrevi.AllColumns.Add(colRowFiller);

                flvCicliListaBrevi.RebuildColumns();
                flvCicliListaBrevi.SetObjects(CicloCorrente.CicliMemoriaBreve);
                flvCicliListaBrevi.BuildList();
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
            try
            {
                string _text = e.SubItem.Text;
                if (_text.Contains("** - "))
                {
                    e.SubItem.Text = e.SubItem.Text.Substring(5);
                    e.SubItem.ForeColor = Color.Red;

                }
            }
            catch (Exception Ex)
            {
                Log.Error("frmListaCicliBreve_Resize: " + Ex.Message);
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
                    tbpAndamentoOxy.Hide();
                }
                else
                {
                    //tbpAndamentoCiclo.Show();
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

                int _ciclo = 0;
  
            // Prima Carico la testata
                // Ciclo Lungo
                txtIdEventoLungo.Text = CicloCorrente.strIdMemCiclo;
                txtInizioEvento.Text = CicloCorrente.DataOraStart;
                txtFineEvento.Text = CicloCorrente.DataOraFine;
                txtCausaleStop.Text = CicloCorrente.strChargerStop;
                this.Text = StringheMessaggio.strTitoloListaBrevi + " " + CicloCorrente.strIdMemCiclo;
                //Programma
                /*
                txtNumProgramma.Text = CicloLungo.ProgrammaAttivo.IdProgramma.ToString();
                txtCapacitaNominale.Text = FunzioniMR.StringaCorrente( (short)CicloLungo.ProgrammaAttivo.BatteryAhdef ) + " Ah";
                txtTensioneNominale.Text = FunzioniMR.StringaTensione( CicloLungo.ProgrammaAttivo.BatteryVdef) + " V";
                txtCelleV1.Text = CicloLungo.ProgrammaAttivo.BatteryCell1.ToString();
                txtCelleV2.Text = CicloLungo.ProgrammaAttivo.BatteryCell2.ToString();
                txtCelleV3.Text = CicloLungo.ProgrammaAttivo.BatteryCell3.ToString();
                txtCelleTot.Text = CicloLungo.ProgrammaAttivo.BatteryCells.ToString();
                */

                //Calcolo la matrice attivazione celle
                /*
                AttivaIntermedi[0] = (CicloLungo.ProgrammaAttivo.BatteryCells > 0);
                AttivaIntermedi[1] = (CicloLungo.ProgrammaAttivo.BatteryCell1 > 0);
                AttivaIntermedi[2] = (CicloLungo.ProgrammaAttivo.BatteryCell2 > 0);
                AttivaIntermedi[3] = (CicloLungo.ProgrammaAttivo.BatteryCell3 > 0);
                */
                // Poi la tabella
                Log.Debug("MostraCicli rigeneraListaCicli");
                InizializzaListaBrevi();

                //rigeneraListaCicli();

                //mascheraColonne(0, false);
                //bool TempoRelativo = chkIntervalloRelativo.Checked;
                Log.Debug("MostraCicli prepara grafici");
                /*
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
                */
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

            }

            catch (Exception Ex)
            {
                Log.Error("InizializzaOxyPlotControl: " + Ex.Message);
            }

        }

        private void oxyContainerCiclo_Click(object sender, EventArgs e)
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
            try
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

            catch (Exception Ex)
            {
                Log.Error("CreateYAxis: " + Ex.Message);
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

                bool _eventoEsterno ;

                int _numIterazioni;
                int _iterazioneCorrente;

                oxyGraficoCiclo.InvalidatePlot(false); 

                DateTime _istanteLettura;

                switch (TipoCiclo)
                {

                    case 0xF0:
                        _Flag = "Carica";
                        _titoloGrafico = StringheStatistica.FaseCarica ;
                        _fattoreCorrente = 1;

                        break;
                    case 0x0F:
                        _Flag = "Scarica";
                        _titoloGrafico = StringheStatistica.FaseScarica;
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
                _eventoEsterno = true;
                _iterazioneCorrente = 0;
                _numIterazioni = CicloLungo.CicliMemoriaBreve.Count - 1;

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
                     ValoriCicloTensioni.Add(new mrDataPointLl { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });

                     if (_tempVal < _vMin) _vMin = _tempVal;
                     if (_tempVal > _vMax) _vMax = _tempVal;


                     //***************************************************************************************

                     _tempVal = (double)(_fattoreCorrente * lettura.Amed / 10);
                    if(_iterazioneCorrente == 0 || _iterazioneCorrente >= _numIterazioni)
                        _eventoEsterno = true;

                    if (_eventoEsterno == true)
                    {
                        _eventoEsterno = false;
                        if (_tempVal < 0)
                            _tempVal = 0;
                    }
                     ValoriCicloCorrenti.Add(new mrDataPointLl { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
                     if (_tempVal < _iMin) _iMin = _tempVal;
                     if (_tempVal > _iMax) _iMax = _tempVal;

                     //***************************************************************************************

                     _tempVal = (double)(lettura.Tntc);
                     ValoriCicloTemp.Add(new mrDataPointLl { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
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
                serTensione.Title = StringheStatistica.Tensione;
                if (TempoRelativo == true)
                {
                    serTensione.DataFieldX = "TimeLapse";
                    serTensione.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.##} V\n\nIstante:" + " {2:hh\\:mm}\n";
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
                serCorrente.Title = StringheStatistica.Corrente;

                if (TempoRelativo == true)
                {
                    serCorrente.DataFieldX = "TimeLapse";
                    serCorrente.TrackerFormatString = serCorrente.Title + "\n\nIstante:" + "{2:hh\\:mm}\nI med={4:0.##} A";
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
                serTemperatura.Title = StringheStatistica.TemperaturaMedia;

                if (TempoRelativo == true)
                {
                    serTemperatura.DataFieldX = "TimeLapse";
                    serTemperatura.TrackerFormatString = serTemperatura.Title + "\n\nIstante:" + "{2:hh\\:mm}\nTemperatura: {4:0.##} °C";
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
                    string _titoloInizio = StringheStatistica.InizioFase + ": ";//"Inizio Fase: ";
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
                    Unit = " V/el ",
                    TitleFontSize = 12,
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    AxisDistance = 15,
                    AxislineStyle = OxyPlot.LineStyle.Solid,
                    Key = "Tensione",
                    MajorGridlineColor = OxyPlot.OxyColors.LightBlue,
                    MinorGridlineColor = OxyPlot.OxyColors.LightBlue,
                    AxislineColor = OxyPlot.OxyColors.Blue,
                    TextColor = OxyPlot.OxyColors.Blue,
                    TitleColor = OxyPlot.OxyColors.Blue,
                    TicklineColor = OxyPlot.OxyColors.Blue,
                    AxislineThickness = 2,
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
                    AxislineColor = OxyPlot.OxyColors.Red,
                    TextColor = OxyPlot.OxyColors.Red,
                    TitleColor = OxyPlot.OxyColors.Red,
                    TicklineColor = OxyPlot.OxyColors.Red,
                    AxisTitleDistance = 2,
                    AxislineThickness = 2,
                    ExtraGridlines = new Double[] { 0 },
                    ExtraGridlineColor = OxyPlot.OxyColors.Black,
                    ExtraGridlineThickness = 2,

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
                    AxislineColor = OxyPlot.OxyColors.Green,
                    TextColor = OxyPlot.OxyColors.Green,
                    TitleColor = OxyPlot.OxyColors.Green,
                    TicklineColor = OxyPlot.OxyColors.Green,
                    AxislineThickness = 2,

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
                oxyGraficoTensioni.PlotMargins = new OxyPlot.OxyThickness(190, 0, 0, 50);
                DateTime _istanteLettura;

                switch (TipoCiclo)
                {

                    case 0xF0:
                        _Flag = "Carica";
                        _titoloGrafico = StringheStatistica.TensioniCarica;
                        _fattoreCorrente = 1;

                        break;
                    case 0x0F:
                        _Flag = "Scarica";
                        _titoloGrafico = StringheStatistica.TensioniScarica;
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
                    ValoriTensioniCellaTot.Add(new mrDataPointLl { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });

                    if (_tempVal < _vMin) _vMin = _tempVal;
                    if (_tempVal > _vMax) _vMax = _tempVal;

                    //************************************ Tensioni Cella S1

                    _tempVal = (double)(lettura.Vcs1 / 100);
                    ValoriTensioniCellaS1.Add(new mrDataPointLl { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
                    if (_tempVal > 0)
                    {
                        _serEnabled[1] = true;
                        if (_tempVal < _vMin) _vMin = _tempVal;
                    }
                    if (_tempVal > _vMax) _vMax = _tempVal;

                    //************************************ Tensioni Cella S2

                    _tempVal = (double)(lettura.Vcs2 / 100);
                    ValoriTensioniCellaS2.Add(new mrDataPointLl { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
                    if (_tempVal > 0)
                    {
                        _serEnabled[2] = true;
                        if (_tempVal < _vMin) _vMin = _tempVal;
                    }
                    if (_tempVal > _vMax) _vMax = _tempVal;

                    //************************************ Tensioni Cella S3

                    _tempVal = (double)(lettura.Vcs3 / 100);
                    ValoriTensioniCellaS3.Add(new mrDataPointLl { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
                    if (_tempVal > 0)
                    {
                        _serEnabled[3] = true;
                        if (_tempVal < _vMin) _vMin = _tempVal;
                    }
                    if (_tempVal > _vMax) _vMax = _tempVal;

                    //************************************ Tensioni Cella S1

                    _tempVal = (double)(lettura.VcsBatt/ 100);
                    ValoriTensioniCellaS4.Add(new mrDataPointLl { Date = _istanteLettura, TimeLapse = _IntervalloLettura, Value = _tempVal });
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
                serTensione.Title = StringheStatistica.TensioneTotale;
                if (TempoRelativo == true)
                {
                    serTensione.DataFieldX = "TimeLapse";
                    serTensione.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.###} V\n\nIstante:" + " {2:hh\\:mm}\n";
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
                serTensSez1.Title = StringheStatistica.Tensione + " " + StringheStatistica.Sezione + " 1";
                if (TempoRelativo == true)
                {
                    serTensSez1.DataFieldX = "TimeLapse";
                    serTensSez1.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.###} V\n\nIstante:" + " {2:hh\\:mm}\n";
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
                serTensSez2.Title = StringheStatistica.Tensione + " " + StringheStatistica.Sezione + " 2";
                if (TempoRelativo == true)
                {
                    serTensSez2.DataFieldX = "TimeLapse";
                    serTensSez2.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.###} V\n\nIstante:" + " {2:hh\\:mm}\n";
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
                serTensSez3.Title = StringheStatistica.Tensione + " " + StringheStatistica.Sezione + " 3";
                if (TempoRelativo == true)
                {
                    serTensSez3.DataFieldX = "TimeLapse";
                    serTensSez3.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.###} V\n\nIstante:" + " {2:hh\\:mm}\n";
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
                serTensSez4.Title = StringheStatistica.Tensione + " " + StringheStatistica.Sezione + " 4";
                if (TempoRelativo == true)
                {
                    serTensSez4.DataFieldX = "TimeLapse";
                    serTensSez4.TrackerFormatString = "\n" + serTensione.Title + ": {4:0.###} V\n\nIstante:" + " {2:hh\\:mm}\n";
                }
                else
                {
                    serTensSez4.DataFieldX = "Date";
                    serTensSez4.TrackerFormatString = serTensione.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nV med={4:0.0} V";

                }
                serTensSez4.DataFieldY = "Value";
                serTensSez4.Color = OxyPlot.OxyColors.Brown;




                OxyPlot.Series.LineSeries serTemperatura = new OxyPlot.Series.LineSeries();
                serTemperatura.Title = StringheStatistica.TemperaturaMedia;
           
                if (TempoRelativo == true)
                {
                    serTemperatura.DataFieldX = "TimeLapse";
                    serTemperatura.TrackerFormatString = serTemperatura.Title + "\n\nIstante:" + "{2:hh\\:mm}\nTemperatura: {4:0.###} °C";
                }
                else
                {
                    serTemperatura.DataFieldX = "Date";
                    serTemperatura.TrackerFormatString = serTemperatura.Title + "\n\nIstante={2:" + _modelloIntervallo + "}\nTemperatura: {4:0.0} °C";

                }

                serTemperatura.DataFieldY = "Value";
                serTemperatura.Color = OxyPlot.OxyColors.Cornsilk;

                Log.Debug("GraficoCiclo: fine serie inizio assi");
                if (TempoRelativo == true)
                {
                    string _titoloInizio = StringheStatistica.InizioFase + ": ";
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
                    TickStyle = OxyPlot.Axes.TickStyle.Outside,
                    Minimum = _vMin,
                    Maximum = _vMax,
                    PositionTier = 0,
                    Unit = " V/el ",
                    //TitlePosition = -4,
                    TitleFontSize = 12,
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    AxisDistance = 15,
                    AxislineStyle = OxyPlot.LineStyle.Solid,
                    Key = "Tensione",
                    MajorGridlineColor = OxyPlot.OxyColors.LightBlue,
                    MinorGridlineColor = OxyPlot.OxyColors.LightBlue,
                };
                
                Log.Debug("GraficoCiclo: fine definizioni aggiunta elementi");

                oxyGraficoTensioni.Axes.Add(VAxisTens);
                //oxyGraficoCiclo.Axes.Add(AAxisCorr);
                //oxyGraficoTensioni.Axes.Add(CAxisTemp);

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
            try
            {
                foreach (OLVColumn Colonna in flvCicliListaBrevi.Columns)
                {
                    Log.Debug(Colonna.Index.ToString() + " - " + Colonna.Name);
                }

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("mascheraColonne: " + Ex.Message);
                return false;
            }
        }

        private void btnChiudi_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSfoglia_Click(object sender, EventArgs e)
        {
            try
            {
                sfdNuovoCSV.Filter = "Comma Separed (*.csv)|*.csv|All files (*.*)|*.*";
                sfdNuovoCSV.ShowDialog();
                txtNuovoFile.Text = sfdNuovoCSV.FileName;
            }
            catch (Exception Ex)
            {
                Log.Error("btnSfoglia_Click: " + Ex.Message);
            }
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



                    MessageBox.Show(StringheComuni.FileGenerato, StringheComuni.EsportazioneDati, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(StringheComuni.InserireNome, StringheComuni.EsportazioneDati, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }

            catch ( Exception Ex )
            {
                Log.Error("btnGeneraCsv_Click: " + Ex.Message);
                MessageBox.Show(Ex.Message, StringheComuni.EsportazioneDati, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
            {
                MostraCicli();
                tabCicloBreve.SelectedTab = tbpDatiCiclo;
                tabCicloBreve.SelectedTab = tbpUtilita;
            }
            catch (Exception Ex)
            {
                Log.Error("chkIntervalloRelativo_CheckedChanged: " + Ex.Message);
            }
        }

        private void tbpAndamentoOxy_Click(object sender, EventArgs e)
        {

        }

        private void txtNuovoFile_TextChanged_1(object sender, EventArgs e)
        {

        }

        public void Reset()
        {
            try
            {
               // foreach (OxyPlot.Axes.Axis axis in oxyGraficoTensioni.Axes)
               //     axis.Reset();
                oxyGraficoTensioni.ResetAllAxes();
                oxyGraficoTensioni.InvalidatePlot(false);

                // foreach (OxyPlot.Axes.Axis axis in oxyGraficoCiclo.Axes)
                //     axis.Reset();
                oxyGraficoCiclo.ResetAllAxes();
                oxyGraficoCiclo.InvalidatePlot(false);



            }

            catch ( Exception Ex )
            {
                Log.Error("Reset: " + Ex.Message);
            }
        }

        private void frmListaCicliBreve_Activated(object sender, EventArgs e)
        {
            try
            {
                frmMain _parent = (frmMain)this.MdiParent;
                _parent.Toolbar.reset();

                _parent.Toolbar.StampaAttiva = true;
                _parent.Toolbar.StampaVisibile = true;

                _parent.Toolbar.ExportAttivo = false;
                _parent.Toolbar.ExportVisibile = true;

                _parent.Toolbar.RefreshAttivo = true;
                _parent.Toolbar.RefreshVisibile = true;
                _parent.AggiornaToolbar(_parent.Toolbar);

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat_Activated: " + Ex.Message);
            }
        }

        private void frmListaCicliBreve_Deactivate(object sender, EventArgs e)
        {
            try
            {
                frmMain _parent = (frmMain)this.MdiParent;
                _parent.Toolbar.reset();
                _parent.AggiornaToolbar(_parent.Toolbar);

            }
            catch (Exception Ex)
            {
                Log.Error("frmSpyBat_Activated: " + Ex.Message);
            }
        }

        private void flvCicliListaBrevi_MouseHover(object sender, EventArgs e)
        {
            int a = 0;
        }
    }

    public class mrDataPointLl
    {
        public DateTime Date { get; set; }
        public TimeSpan TimeLapse { get; set; }
        public double Value { get; set; }
    }
}
