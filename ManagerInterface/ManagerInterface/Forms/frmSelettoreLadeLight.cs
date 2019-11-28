using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using BrightIdeasSoftware;

using ChargerLogic;
using MoriData;
using Utility;


namespace PannelloCharger
{
    public partial class frmSelettoreLadeLight : Form
    {

        parametriSistema _parametri;
        LogicheBase _logiche;
        public MoriData._db _database;
        IEnumerable<dataUtility.sbListaElementi> ListaLadeLight;

        private static ILog Log = LogManager.GetLogger("frmSelettoreLadeLight");

        public frmSelettoreLadeLight()
        {
            InitializeComponent();
            applicaAutorizzazioni();
            this.Width = 900;
        }

        public frmSelettoreLadeLight(ref parametriSistema _par, LogicheBase Logiche)
        {
            InitializeComponent();
            _parametri = _par;
            ResizeRedraw = true;
            _logiche = Logiche;
            _database = _logiche.dbDati.connessione;
            ListaLadeLight = ListaApparati();
            applicaAutorizzazioni();
            this.Width = 900;
        }

        public void applicaAutorizzazioni()
        {
            try
            {
                bool _enabled;
                bool _readonly;
                bool _visible;
                int LivelloCorrente;
                if (_logiche.currentUser != null)
                {
                    LivelloCorrente = _logiche.currentUser.livello;
                }
                else
                {
                    LivelloCorrente = 99;
                }

                if (LivelloCorrente < 2) _visible = true; else _visible = false;
                txtIdScheda.Visible = _visible;

                if (LivelloCorrente < 3) _visible = true; else _visible = false;
                btnImportaDati.Visible = _visible;


            }
            catch (Exception Ex)
            {
                Log.Error("applicaAutorizzazioni: " + Ex.Message);
            }

        }


        public IEnumerable<dataUtility.sbListaElementi> ListaApparati()
        {
            try
            {
                string _sql = "";
                _sql += "select t1.Id, t1.SwVersion, t1.ProductId, t1.HwVersion, t2.DataInstall, t2.Client, t2.BatteryBrand, t2.BatteryModel, t2.BatteryId, t2.ClientNote,t2.SerialNumber,";
                _sql += "( select max(DataLastDownload) from _sbMemLunga as t99 where t99.IdApparato = t1.Id) as UltimaLettura ";

                _sql += "from _spybatt as t1 left outer join _sbDatiCliente as t2 on t1.Id = t2.IdApparato order by t2.ClientNote,t1.Id ";

                Log.Info(_sql);

                return _database.Query<dataUtility.sbListaElementi>(_sql);
            }
            catch (Exception Ex)
            {
                Log.Error("ListaApparati. " + Ex.Message);
                return null;
            }
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

                flvwListaApparati.HeaderUsesThemes = false;
                flvwListaApparati.HeaderFormatStyle = _stile;

                flvwListaApparati.AllColumns.Clear();

                flvwListaApparati.View = View.Details;
                flvwListaApparati.ShowGroups = false;
                flvwListaApparati.GridLines = true;
                flvwListaApparati.UseAlternatingBackColors = true;
                flvwListaApparati.FullRowSelect = true;

                BrightIdeasSoftware.OLVColumn colIdConfig = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "ID",
                    AspectName = "strIdProgramma",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvwListaApparati.AllColumns.Add(colIdConfig);







                BrightIdeasSoftware.OLVColumn colCli = new BrightIdeasSoftware.OLVColumn();
                colCli.Text = StringheColonneTabelle.ListaApp01Cliente; //"Cliente";
                colCli.AspectName = "Client";
                colCli.Width = 200;
                colCli.HeaderTextAlign = HorizontalAlignment.Left;
                colCli.TextAlign = HorizontalAlignment.Left;
                flvwListaApparati.AllColumns.Add(colCli);

                BrightIdeasSoftware.OLVColumn idBatt = new BrightIdeasSoftware.OLVColumn();
                idBatt.Text = StringheColonneTabelle.ListaApp02IdBatt; //"ID Batt.";
                idBatt.AspectName = "BatteryId";
                idBatt.Width = 100;
                idBatt.HeaderTextAlign = HorizontalAlignment.Center;
                idBatt.TextAlign = HorizontalAlignment.Left;
                flvwListaApparati.AllColumns.Add(idBatt);

                BrightIdeasSoftware.OLVColumn colBatt = new BrightIdeasSoftware.OLVColumn();
                colBatt.Text = StringheColonneTabelle.ListaApp03Batt; // "Batteria";
                colBatt.AspectName = "BatteryBrand";
                colBatt.Width = 100;
                colBatt.HeaderTextAlign = HorizontalAlignment.Center;
                colBatt.TextAlign = HorizontalAlignment.Left;
                flvwListaApparati.AllColumns.Add(colBatt);

                BrightIdeasSoftware.OLVColumn colBattMod = new BrightIdeasSoftware.OLVColumn();
                colBattMod.Text = StringheColonneTabelle.ListaApp04Mod; // "Modello";
                colBattMod.AspectName = "BatteryModel";
                colBattMod.Width = 100;
                colBattMod.HeaderTextAlign = HorizontalAlignment.Center;
                colBattMod.TextAlign = HorizontalAlignment.Right;
                flvwListaApparati.AllColumns.Add(colBattMod);

                BrightIdeasSoftware.OLVColumn colNote = new BrightIdeasSoftware.OLVColumn();
                colNote.Text = StringheColonneTabelle.ListaApp05Note; // "Note";
                colNote.AspectName = "ClientNote";
                colNote.Width = 200;
                colNote.HeaderTextAlign = HorizontalAlignment.Center;
                colNote.TextAlign = HorizontalAlignment.Left;
                flvwListaApparati.AllColumns.Add(colNote);

                BrightIdeasSoftware.OLVColumn colSN = new BrightIdeasSoftware.OLVColumn();
                colSN.Text = StringheColonneTabelle.ListaApp07SN; // "Note";
                colSN.AspectName = "SerialNumber";
                colSN.Width = 100;
                colSN.HeaderTextAlign = HorizontalAlignment.Center;
                colSN.TextAlign = HorizontalAlignment.Left;
                flvwListaApparati.AllColumns.Add(colSN);

                BrightIdeasSoftware.OLVColumn colDul = new BrightIdeasSoftware.OLVColumn();
                colDul.Text = "Ultima Lettura";// StringheColonneTabelle.ListaApp07SN; // "Note";
                colDul.AspectName = "strUltimaLettura";
                colDul.Width = 100;
                colDul.HeaderTextAlign = HorizontalAlignment.Center;
                colDul.TextAlign = HorizontalAlignment.Left;
                flvwListaApparati.AllColumns.Add(colDul);

                BrightIdeasSoftware.OLVColumn colId = new BrightIdeasSoftware.OLVColumn();
                colId.Text = StringheColonneTabelle.ListaApp06IdSb;  // "ID SPY-BATT";
                colId.AspectName = "Id";
                colId.Width = 120;
                colId.HeaderTextAlign = HorizontalAlignment.Left;
                colId.TextAlign = HorizontalAlignment.Left;
                flvwListaApparati.AllColumns.Add(colId);


                flvwListaApparati.RebuildColumns();

                this.flvwListaApparati.SetObjects(ListaLadeLight);
                flvwListaApparati.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("Mostra Lista: " + Ex.Message);
            }


        }





    }
}
