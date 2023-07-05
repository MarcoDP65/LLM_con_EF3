using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using MoriData;
using ChargerLogic;
using static ZXing.QrCode.Internal.Version;

namespace PannelloCharger.Forms
{
    public partial class frmSelettoreProfilo : Form
    {

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public List<llProgrammaCarica> ProgrammiDefiniti;
        public MoriData._db _database;
        public parametriSistema Parametri;
        public llProgrammaCarica ProfiloSelezionato = null;
        private llProgrammaCarica _profiloSel = null;

        public frmSelettoreProfilo()
        {
            InitializeComponent();
        }

        private void btnAnnullaSelezione_Click(object sender, EventArgs e)
        {
            _profiloSel = null;
            this.Close();
        }

        public bool CaricaListaProfili(string IdApparato, string TipoApparato)
        {
            try 
            {
                bool _esito;
                llProgrammaCarica _tempPrg;

                ProgrammiDefiniti = new List<llProgrammaCarica>();
                if (_database == null) return false;

                string Sql = "select * from _llProgrammaCarica ";
                if (IdApparato != "" || TipoApparato != "")
                {
                    Sql += " where ";
                    if (IdApparato != "") Sql += " IdApparato = '" + IdApparato + "' ";
                    if (IdApparato != "" && TipoApparato != "") Sql += " and ";
                    if (TipoApparato != "") Sql += " TipoApparato = '" + TipoApparato + "' ";
                }
                Sql += " order by IdProgramma ";

                IEnumerable<_llProgrammaCarica> _TempCicli = _database.Query<_llProgrammaCarica>(Sql);

                foreach (_llProgrammaCarica Elemento in _TempCicli)
                {
                    llProgrammaCarica _cLoc;
                    _cLoc = new llProgrammaCarica(Elemento);
                    _cLoc.Parametri = Parametri;
                    _cLoc.GeneraListaParametri();
                    ProgrammiDefiniti.Add(_cLoc);

                }

                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
                return false;
            }
        }


        public void InizializzaVistaProgrammazioni()
        {
            try
            {
                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);
                Font _carTesto = new Font("Tahoma", 10, FontStyle.Regular);


                flwPaListaConfigurazioni.HeaderUsesThemes = false;
                flwPaListaConfigurazioni.HeaderFormatStyle = _stile;
                flwPaListaConfigurazioni.UseAlternatingBackColors = true;
                flwPaListaConfigurazioni.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flwPaListaConfigurazioni.AllColumns.Clear();

                flwPaListaConfigurazioni.View = View.Details;
                flwPaListaConfigurazioni.ShowGroups = false;
                flwPaListaConfigurazioni.GridLines = true;
                flwPaListaConfigurazioni.Font = _carTesto;
                flwPaListaConfigurazioni.RowHeight = 25;
                flwPaListaConfigurazioni.FullRowSelect = true;
                flwPaListaConfigurazioni.CheckBoxes = false;
                flwPaListaConfigurazioni.CheckedAspectName = "Selezionato";

                BrightIdeasSoftware.OLVColumn colIdConfig = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "ID",
                    AspectName = "strIdProgramma",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colIdConfig);

                BrightIdeasSoftware.OLVColumn colPosizione = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Pos",
                    AspectName = "strPosizioneCorrente",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colPosizione);

                BrightIdeasSoftware.OLVColumn colSpyBatt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "SB",
                    AspectName = "strAbilitaComunicazioneSpybatt",
                    Width = 40,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.OK_16, "NO", Properties.Resources.GRAY_16 })

                };
                flwPaListaConfigurazioni.AllColumns.Add(colSpyBatt);



                BrightIdeasSoftware.OLVColumn colProgName = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Nome",
                    AspectName = "ProgramName",
                    Width = 100,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,

                };
                flwPaListaConfigurazioni.AllColumns.Add(colProgName);


                BrightIdeasSoftware.OLVColumn colBatteryType = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Tipo Batt.",
                    AspectName = "strTipoBatteria",
                    Width = 300,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colBatteryType);


                BrightIdeasSoftware.OLVColumn colNomeProfilo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Profilo",
                    ToolTipText = "Nome Profilo",
                    AspectName = "strTipoProfilo",
                    Width = 150,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colNomeProfilo);

                BrightIdeasSoftware.OLVColumn colRowBattVNom = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "V",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strBatteryVdef",
                    Width = 120,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Right,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colRowBattVNom);

                BrightIdeasSoftware.OLVColumn colRowBattAhNom = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Ah",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strBatteryAhdef",
                    Width = 120,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Right,
                };
                flwPaListaConfigurazioni.AllColumns.Add(colRowBattAhNom);



                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn();
                colRowFiller.Text = "";
                colRowFiller.Width = 60;
                colRowFiller.HeaderTextAlign = HorizontalAlignment.Center;
                colRowFiller.TextAlign = HorizontalAlignment.Right;
                colRowFiller.FillsFreeSpace = true;
                flwPaListaConfigurazioni.AllColumns.Add(colRowFiller);

                flwPaListaConfigurazioni.Sort(colPosizione);
                flwPaListaConfigurazioni.RebuildColumns();
                flwPaListaConfigurazioni.SetObjects(ProgrammiDefiniti);
                flwPaListaConfigurazioni.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }

        private void flwPaListaConfigurazioni_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;

                if (flwPaListaConfigurazioni.SelectedObject == null)
                {
                    _profiloSel = null;
                    btnSelezionaConfigurazione.Enabled = false;
                }
                else
                {
                    _profiloSel = (llProgrammaCarica)flwPaListaConfigurazioni.SelectedObject;
                    btnSelezionaConfigurazione.Enabled = true;
                }

            }
            catch (Exception Ex)
            {
                Log.Error("flwPaListaConfigurazioni_SelectedIndexChanged: " + Ex.Message);
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnSelezionaConfigurazione_Click(object sender, EventArgs e)
        {
            try
            {
                if(_profiloSel != null)
                {
                    ProfiloSelezionato = _profiloSel;
                    this.Close();

                }
            }
            catch (Exception)
            {

            }
        }
    }
}
