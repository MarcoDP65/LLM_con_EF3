using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using OxyPlot;
using OxyPlot.WindowsForms;

namespace ChargerLogic
{

    public class oxyGraficoSingolo : Control
    {

        public PlotView oxyContainer;
        public PlotModel GraficoBase;

        public Point Location = new Point(10, 10);
        public Size Size = new Size(400, 300);


        public oxyGraficoSingolo ()
        {
            _inizializzaControllo();

        }

        public oxyGraficoSingolo(System.Drawing.Point Location, System.Drawing.Size Size)
        {
            base.Size = Size;
            base.Location = Location;
            _inizializzaControllo();

        }


        protected void _inizializzaControllo()
        {
          
            InizializzaGrafico();

        }


        

        /// <summary>
        /// Inizializza il controllo grafico della pagina.
        /// </summary>
        void InizializzaGrafico()
        {
            try
            {
                this.oxyContainer = new PlotView();
                this.oxyContainer.SuspendLayout();

                this.oxyContainer.Dock = System.Windows.Forms.DockStyle.None;
                this.oxyContainer.Location = this.Location;
                //this.oxyContainer.Margin = new System.Windows.Forms.Padding(0);
                this.oxyContainer.Name = "oxyContainer";
                this.oxyContainer.PanCursor = System.Windows.Forms.Cursors.Hand;
                this.oxyContainer.Size = this.Size;
                //this.oxyContainer.TabIndex = 0;
                this.oxyContainer.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
                this.oxyContainer.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
                this.oxyContainer.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
                //this.oxyContainer.Click += new System.EventHandler(this.oxyContainerGrSingolo_Click);
                // 

                //this.Controls.Add(this.oxyContainer);

                GraficoBase = new OxyPlot.PlotModel
                {
                    Title = "",
                    Subtitle = "",
                    PlotType = OxyPlot.PlotType.XY,
                    Background = OxyPlot.OxyColors.PaleVioletRed,
                };

                oxyContainer.Model = GraficoBase;
                oxyContainer.BackColor = Color.Red;
     

                this.ResumeLayout();
            }
            catch (Exception Ex)
            {

            }
        }

    }
}
