using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.WindowsForms;



namespace ChargerLogic
{
    public partial class oxyTabPage : TabPage
    {

        public PlotView oxyContainer;
        public PlotModel GraficoBase;


        // Nella pagina conservo anche i dati per il grafico
        public DatiEstrazione DatiGrafico;


        public oxyTabPage()
        {
            _inizializzaControllo();
        }

        public oxyTabPage(string Page)
        {
            _inizializzaControllo();
            base.Text = Page;
        }
        
        protected void _inizializzaControllo()
        {
            InitializeComponent();
            InizializzaGrafico();

        }


        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        protected override void OnResize(EventArgs evtArg)
        {
            base.OnResize(evtArg);
        }

        /// <summary>
        /// Inizializza il controllo grafico della pagina.
        /// </summary>
        void InizializzaGrafico()
        {
            try
            {
                this.oxyContainer = new PlotView();
                this.SuspendLayout();

                this.oxyContainer.Dock = System.Windows.Forms.DockStyle.Fill;
                this.oxyContainer.Location = new System.Drawing.Point(10, 10);
                //this.oxyContainer.Margin = new System.Windows.Forms.Padding(0);
                this.oxyContainer.Name = "oxyContainer";
                this.oxyContainer.PanCursor = System.Windows.Forms.Cursors.Hand;
                this.oxyContainer.Size = new System.Drawing.Size(400, 300);
                //this.oxyContainer.TabIndex = 0;
                this.oxyContainer.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
                this.oxyContainer.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
                this.oxyContainer.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
                //this.oxyContainer.Click += new System.EventHandler(this.oxyContainerGrSingolo_Click);
                // 

                this.Controls.Add(this.oxyContainer);

                GraficoBase = new OxyPlot.PlotModel
                {
                    Title = "",
                    Subtitle = "",
                    PlotType = OxyPlot.PlotType.XY,
                    Background = OxyPlot.OxyColors.White
                };

                oxyContainer.Model = GraficoBase;

                this.ResumeLayout();
            }
            catch( Exception Ex)
            {
                
            }
        }
    }
}
