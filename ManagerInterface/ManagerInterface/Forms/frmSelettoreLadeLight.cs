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
        IEnumerable<dataUtility.llListaElementi> ListaLadeLight;

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


        public IEnumerable<dataUtility.llListaElementi> ListaApparati()
        {
            try
            {
                string _sql = "";
                _sql += "select  t1.IdApparato as IdApparato, 1 as IdCliente, t2.Client as Client, t2.ClientDescription as ClientDescription,";
                _sql += "t1.TipoApparato as TipoApparato,";
                _sql += "t1.RevisionDate as UltimaLettura ";

                _sql += "from _llParametriApparato as t1 left outer join _llDatiCliente as t2 on t1.IdApparato = t2.IdApparato";


                Log.Info(_sql);

                return  _database.Query<dataUtility.llListaElementi>(_sql);
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
                    AspectName = "IdApparato",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvwListaApparati.AllColumns.Add(colIdConfig);

                BrightIdeasSoftware.OLVColumn colClient = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Cliente",
                    AspectName = "Client",
                    Width = 120,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvwListaApparati.AllColumns.Add(colClient);

                BrightIdeasSoftware.OLVColumn colClientDesc = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Descrizione",
                    AspectName = "ClientDescription",
                    Width = 240,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvwListaApparati.AllColumns.Add(colClientDesc);

                BrightIdeasSoftware.OLVColumn colChargerType = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Modello",
                    AspectName = "strTipoApparato",
                    Width = 90,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvwListaApparati.AllColumns.Add(colChargerType);

                BrightIdeasSoftware.OLVColumn colUltimaLettura = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Ultima Lettura",
                    AspectName = "strUltimaLettura",
                    Width = 120,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvwListaApparati.AllColumns.Add(colUltimaLettura);

                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "",
                    AspectName = "",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                    FillsFreeSpace = true,
                };
                flvwListaApparati.AllColumns.Add(colRowFiller);


                flvwListaApparati.RebuildColumns();

                this.flvwListaApparati.SetObjects(ListaLadeLight);
                flvwListaApparati.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("Mostra Lista: " + Ex.Message);
            }


        }

        private void frmSelettoreLadeLight_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.Width > 600)
                {

                    //lvwCicliBatteriaOld.Width = this.Width - 120;
                    flvwListaApparati.Width = this.Width - 50;
                    btnChiudi.Left = this.Width - 125;
                }

                if (this.Height > 300)
                {

                    flvwListaApparati.Height = this.Height - 105;
                    btnApriLadeLight.Top = this.Height - 83;
                    btnEliminaDati.Top = this.Height - 83;
                    btnEsportaSpybatt.Top = this.Height - 83;
                    btnImportaDati.Top = this.Height - 83;
                    btnChiudi.Top = this.Height - 83;
                    txtIdScheda.Top = this.Height - 83;

                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }

        }

        private void btnApriLadeLight_Click(object sender, EventArgs e)
        {
            MostraDettaglioRiga();
        }

        private void MostraDettaglioRiga()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (flvwListaApparati.SelectedObject != null)
                {


                    dataUtility.llListaElementi _tempLadeLight = (dataUtility.llListaElementi)flvwListaApparati.SelectedObject;
                    if (_tempLadeLight.IdApparato != null)
                    {
                        ApriLadeLight(_tempLadeLight.IdApparato);
                    }

                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Log.Error("MostraDettaglioRiga: " + Ex.Message);
                this.Cursor = Cursors.Default;
            }

        }

        private void ApriLadeLight(string IdApparato)
        {
            try
            {

                frmCaricabatterieV2 llCorrente = new frmCaricabatterieV2(ref _parametri, true, IdApparato, _logiche, false, false);
                llCorrente.MdiParent = this.MdiParent;
                llCorrente.StartPosition = FormStartPosition.CenterParent;
                llCorrente.Show();

            }
            catch (Exception Ex)
            {
                Log.Error("ApriSpyBatt: " + Ex.Message);
            }

        }

        private void flvwListaApparati_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;

                if (_lista.SelectedObject != null)
                {
                    MostraDettaglioRiga();
                }
            }
            catch (Exception Ex)
            {
                Log.Error("flvwListaApparati_MouseDoubleClick: " + Ex.Message);
            }

        }

        private void flvwListaApparati_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;

                if (_lista.SelectedObject != null)
                {
                    dataUtility.llListaElementi _tempLadelight = (dataUtility.llListaElementi)flvwListaApparati.SelectedObject;
                    if (_tempLadelight.IdApparato != null)
                    {
                        txtIdScheda.Text = _tempLadelight.IdApparato;
                    }
                }
            }
            catch (Exception Ex)
            {
                Log.Error("flvwListaApparati_SelectedIndexChanged: " + Ex.Message);
            }
        }
    }
}
