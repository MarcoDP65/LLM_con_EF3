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
    public partial class frmSelettoreSpyBatt : Form
    {
        parametriSistema _parametri;
        LogicheBase _logiche;
        public MoriData._db _database;
        IEnumerable<dataUtility.sbListaElementi> ListaSpyBatt;

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public frmSelettoreSpyBatt()
        {
            InitializeComponent();
            applicaAutorizzazioni();
            this.Width = 900;
        }
        
        public frmSelettoreSpyBatt(ref parametriSistema _par, LogicheBase Logiche)
        {
            InitializeComponent();
            _parametri = _par;
            ResizeRedraw = true;
            _logiche = Logiche;
            _database = _logiche.dbDati.connessione;
            ListaSpyBatt = ListaApparati();
            applicaAutorizzazioni();
            this.Width = 900;
        }
        
        private void btnChiudi_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public IEnumerable<_spybatt> ListaApparatiOld()
        {
            try
            {
                return _database.Query<_spybatt>("select * from _spybatt ");
            }
            catch
            {
                return null;
            }
        }


        public IEnumerable<dataUtility.sbListaElementi> ListaApparati()
        {
            try
            {
                string _sql = "";
                _sql += "select t1.Id, t1.SwVersion, t1.ProductId, t1.HwVersion, t2.DataInstall, t2.Client, t2.BatteryBrand, t2.BatteryModel, t2.BatteryId, t2.ClientNote,t2.SerialNumber ";
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

                BrightIdeasSoftware.OLVColumn colId = new BrightIdeasSoftware.OLVColumn();
                colId.Text = StringheColonneTabelle.ListaApp06IdSb;  // "ID SPY-BATT";
                colId.AspectName = "Id";
                colId.Width = 120;
                colId.HeaderTextAlign = HorizontalAlignment.Left;
                colId.TextAlign = HorizontalAlignment.Left;
                flvwListaApparati.AllColumns.Add(colId);


                flvwListaApparati.RebuildColumns();

                this.flvwListaApparati.SetObjects(ListaSpyBatt);
                flvwListaApparati.BuildList();

            }
            catch (Exception Ex)
            {
                Log.Error("Mostra Lista: " + Ex.Message);
            }


        }

 
       private void ApriSpyBatt(string IdApparato)
        {
            try
            {


                frmSpyBat sbCorrente = new frmSpyBat(ref _parametri, true, IdApparato, _logiche, false, false);
                sbCorrente.MdiParent = this.MdiParent;
                sbCorrente.StartPosition = FormStartPosition.CenterParent;
                sbCorrente.Show();


            }
            catch
            {
            }

        }


       private void ApriExportSpyBatt(string IdApparato)
       {
           try
           {

               frmSbExport sbExport = new frmSbExport(ref _parametri, true, IdApparato, _logiche, false, false);
               sbExport.MdiParent = this.MdiParent;
               sbExport.StartPosition = FormStartPosition.CenterParent;
               sbExport.Setmode(elementiComuni.modoDati.Output);
               sbExport.Show();

           }
           catch
           {
           }

       }


       private void ApriImportSpyBatt()
       {
           try
           {

               frmSbExport sbExport = new frmSbExport(ref _parametri, true, "", _logiche, false, false);
               //sbExport.MdiParent = this.MdiParent;
               sbExport.StartPosition = FormStartPosition.CenterParent;
               sbExport.Setmode(elementiComuni.modoDati.Import);
               sbExport.ShowDialog();
               ListaSpyBatt = ListaApparati();
               MostraLista();

           }
           catch (Exception Ex)
           {
               Log.Error(Ex.Message);
           }

       }

        private void MostraDettaglioRiga()
        {
            try
            {
                 if (flvwListaApparati.SelectedObject != null)
                {


                    dataUtility.sbListaElementi _tempSpybat = (dataUtility.sbListaElementi)flvwListaApparati.SelectedObject;
                    if (_tempSpybat.Id != null)
                    {
                        ApriSpyBatt(_tempSpybat.Id);
                    }

                }
            }
            catch
            { }

        }


        private void EliminaRiga()
        {
            try
            {
                if (flvwListaApparati.SelectedObject != null)
                {
                    dataUtility.sbListaElementi _tempSpybat = (dataUtility.sbListaElementi)flvwListaApparati.SelectedObject;
                    if (_tempSpybat.Id != null)
                    {

//                        DialogResult risposta = MessageBox.Show("Vuoi realmente cancellare tutti i dati relativi all'apparato\n " + FunzioniMR.StringaSeriale(_tempSpybat.Id) + "  -  " + _tempSpybat.ClientNote + " ? ", "SPY-BATT", MessageBoxButtons.YesNo);
                        DialogResult risposta = MessageBox.Show(StringheComuni.RichConfermaCanc + "\n " + FunzioniMR.StringaSeriale(_tempSpybat.Id) + "  -  " + _tempSpybat.ClientNote + " ? ", "SPY-BATT", MessageBoxButtons.YesNo);

                        if (risposta == System.Windows.Forms.DialogResult.Yes)
                        {

                            UnitaSpyBatt _sb = new UnitaSpyBatt(ref _parametri, _logiche.dbDati.connessione);
                            _sb.sbData.cancellaDati(_tempSpybat.Id);
                            ListaSpyBatt = ListaApparati();
                            MostraLista();

                        }
                    }

                }
            }
            catch
            { }

        }


        private void EsportaDatiRiga()
        {
            try
            {
                if (flvwListaApparati.SelectedObject != null)
                {


                    dataUtility.sbListaElementi _tempSpybat = (dataUtility.sbListaElementi)flvwListaApparati.SelectedObject;
                    if (_tempSpybat.Id != null)
                    {
                        ApriExportSpyBatt(_tempSpybat.Id);
                        //MessageBox.Show("Esporta Dati", "Esportazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            catch
            { }

        }


        private void btnApriSpybatt_Click(object sender, EventArgs e)
        {
            MostraDettaglioRiga();
        }

        private void frmSelettoreSpyBatt_Load(object sender, EventArgs e)
        {

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

        private void btnEsportaSpybatt_Click(object sender, EventArgs e)
        {
            try
            {
                    EsportaDatiRiga();
            }
            catch (Exception Ex)
            {
                Log.Error("btnEsportaSpybatt_Click: " + Ex.Message);
            }
 
        }

        private void btnImportaDati_Click(object sender, EventArgs e)
        {
            ApriImportSpyBatt();
        }

        private void btnEliminaDati_Click(object sender, EventArgs e)
        {
            EliminaRiga();
        }


        private void frmSelettoreSpyBatt_Resize(object sender, EventArgs e)
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
                    btnApriSpybatt.Top = this.Height - 83;
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

        private void frmSelettoreSpyBatt_Shown(object sender, EventArgs e)
        {
            frmSelettoreSpyBatt_Resize(sender,e);
        }

        private void flvwListaApparati_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                FastObjectListView _lista = (FastObjectListView)sender;

                if (_lista.SelectedObject != null)
                {
                    dataUtility.sbListaElementi _tempSpybat = (dataUtility.sbListaElementi)flvwListaApparati.SelectedObject;
                    if (_tempSpybat.Id != null)
                    {
                        txtIdScheda.Text =  _tempSpybat.Id;
                    }
                }
            }
            catch (Exception Ex)
            {
                Log.Error("flvwListaApparati_MouseDoubleClick: " + Ex.Message);
            }
        }
    }
}
