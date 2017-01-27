using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using System.Threading;

using NextUI.Component;
using NextUI.Frame;

//using Common.Logging;
using BrightIdeasSoftware;
using Utility;
using MoriData;
using ChargerLogic;
using MdiHelper;
using log4net;
using log4net.Config;



namespace PannelloCharger
{
    public partial class frmTestFunzioni : Form
    {
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public List<GiornoTest> ListaGiorni = new List<GiornoTest>();


        public frmTestFunzioni()
        {
            InitializeComponent();
        }

        private void frmTestFunzioni_Load(object sender, EventArgs e)
        {

        }


        public void MostraLista()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);

                flvwListaTest.HeaderUsesThemes = false;
                flvwListaTest.HeaderFormatStyle = _stile;

                flvwListaTest.AllColumns.Clear();

                flvwListaTest.View = View.Details;
                flvwListaTest.ShowGroups = false;
                flvwListaTest.GridLines = true;
                flvwListaTest.UseAlternatingBackColors = true;
                flvwListaTest.FullRowSelect = true;


                BrightIdeasSoftware.OLVColumn colCli = new BrightIdeasSoftware.OLVColumn();
                colCli.Text = "Giorno";
                colCli.AspectName = "strGiorno";
                colCli.Width = 60;
                colCli.HeaderTextAlign = HorizontalAlignment.Left;
                colCli.TextAlign = HorizontalAlignment.Left;
                flvwListaTest.AllColumns.Add(colCli);

                BrightIdeasSoftware.OLVColumn idBatt = new BrightIdeasSoftware.OLVColumn();
                idBatt.Text = "Giorno Sett";
                idBatt.AspectName = "strGiornoSett";
                idBatt.Width = 60;
                idBatt.HeaderTextAlign = HorizontalAlignment.Center;
                idBatt.TextAlign = HorizontalAlignment.Left;
                flvwListaTest.AllColumns.Add(idBatt);

                BrightIdeasSoftware.OLVColumn colBatt = new BrightIdeasSoftware.OLVColumn();
                colBatt.Text = "Sett.Y"; 
                colBatt.AspectName = "strSettimanaAnno";
                colBatt.Width = 60;
                colBatt.HeaderTextAlign = HorizontalAlignment.Center;
                colBatt.TextAlign = HorizontalAlignment.Left;
                flvwListaTest.AllColumns.Add(colBatt);

                BrightIdeasSoftware.OLVColumn colAnno = new BrightIdeasSoftware.OLVColumn();
                colAnno.Text = "Anno"; 
                colAnno.AspectName = "Anno";
                colAnno.Width = 60;
                colAnno.HeaderTextAlign = HorizontalAlignment.Center;
                colAnno.TextAlign = HorizontalAlignment.Right;
                flvwListaTest.AllColumns.Add(colAnno);

                BrightIdeasSoftware.OLVColumn colChiaveAnno = new BrightIdeasSoftware.OLVColumn();
                colChiaveAnno.Text = "Chiave";
                colChiaveAnno.AspectName = "ChiaveAnno";
                colChiaveAnno.Width = 60;
                colChiaveAnno.HeaderTextAlign = HorizontalAlignment.Center;
                colChiaveAnno.TextAlign = HorizontalAlignment.Right;
                flvwListaTest.AllColumns.Add(colChiaveAnno);


                flvwListaTest.RebuildColumns();

                this.flvwListaTest.SetObjects(ListaGiorni);
                flvwListaTest.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("Mostra Lista: " + Ex.Message);
            }


        }



        private void btnChiudi_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGeneraLista_Click(object sender, EventArgs e)
        {
            try
            {

                int NumGiorni = 1;
                DateTime _giornoStart;
                bool _esito;


                if (!int.TryParse(txtNumeroGiorni.Text, out NumGiorni))
                    NumGiorni = 1;
                if (!DateTime.TryParse(mtxtDataInizio.Text, out _giornoStart))
                    return;

                ListaGiorni.Clear();

                for (int _i = 0; _i < NumGiorni; _i++)
                {
                    TimeSpan delta = new TimeSpan(_i, 0, 0, 0);
                    GiornoTest altroGiorno = new GiornoTest();
                    altroGiorno.giorno = _giornoStart.Add(delta);
                    ListaGiorni.Add(altroGiorno);
                }

                MostraLista();

            }
            catch (Exception Ex)
            {
                Log.Error("Mostra Lista: " + Ex.Message);
            }
        }
    }


    public class GiornoTest
    {
        public DateTime giorno { get; set; }
        public SettimanaMR settimana { get; set; }

        DateTimeFormatInfo dfi= DateTimeFormatInfo.CurrentInfo;
        Calendar _tempCal;


        public GiornoTest()
        {
            _tempCal = dfi.Calendar;
        }

/*

        int _settInizio = _tempCal.GetWeekOfYear(valPeriodo.dtDataOraStart, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        _settimanaStart.settimana = _settInizio;
                        _settimanaStart.anno = valPeriodo.dtDataOraStart.Year;
                        _settimanaStart.chiaveSettimana = _settimanaStart.anno.ToString("0000") + _settimanaStart.settimana.ToString("00");
                        _settimanaStart.settimaneAnno = _tempCal.GetWeekOfYear(new DateTime(valPeriodo.dtDataOraStart.Year,12,31,12,0,0), CalendarWeekRule.FirstDay, DayOfWeek.Monday);

*/



        public string strGiorno
        {
            get
            {
                return giorno.ToShortDateString();
            }
        }

        public string GiornoSett
        {
            get
            {
                return giorno.DayOfWeek.ToString();
            }
        }

        public string Anno
        {
            get
            {
                return giorno.Year.ToString();
            }
        }

        public string strGiornoSett
        {
            get
            {
                int dow = (int)giorno.DayOfWeek;
                return dow.ToString( "0" );
            }
        }

        public string strSettimanaAnno
        {
            get
            {
                return _tempCal.GetWeekOfYear(giorno, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString() ;
            }
        }
        public string ChiaveAnno
        {
            get
            {
                return giorno.Year.ToString("0000") +_tempCal.GetWeekOfYear(giorno, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString("00");
            }
        }


    }
}
