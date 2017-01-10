using System;
using System.Collections;
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
    public partial class frmDefinizioneImpianto : Form
    {
        parametriSistema _parametri;
        LogicheBase _logiche;
        public MoriData._db _database;
        public StrutturaImpianto PlantStruct;

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");


        public frmDefinizioneImpianto()
        {
            InitializeComponent();
            applicaAutorizzazioni();
            PlantStruct = new StrutturaImpianto();
            this.Width = 900;
        }


        public frmDefinizioneImpianto(ref parametriSistema _par, LogicheBase Logiche)
        {
            InitializeComponent();
           
            _parametri = _par;
            ResizeRedraw = true;
            _logiche = Logiche;
            _database = _logiche.dbDati.connessione;
            PlantStruct = new StrutturaImpianto(_database);

            PlantStruct.CaricaIcone();
            // ListaSpyBatt = ListaApparati();

            InizializzaTreeView();
            applicaAutorizzazioni();
            ImpostaTreeView();
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
               // txtIdScheda.Visible = _visible;

                if (LivelloCorrente < 3) _visible = true; else _visible = false;
               // btnImportaDati.Visible = _visible;


            }
            catch (Exception Ex)
            {
                Log.Error("applicaAutorizzazioni: " + Ex.Message);
            }

        }

        private void InizializzaTreeView()
        {
            try
            {
                

                tlvStrutturaImpianto.HierarchicalCheckboxes = true;
                tlvStrutturaImpianto.Columns.Clear();
                BrightIdeasSoftware.OLVColumn olvColumnName = new OLVColumn();
                olvColumnName.AspectName = "Nome";
                olvColumnName.IsTileViewColumn = true;
                olvColumnName.Text = "Name";
                olvColumnName.UseInitialLetterForGroup = true;
                olvColumnName.Width = 180;
                olvColumnName.WordWrap = true;

                olvColumnName.ImageGetter = delegate (object x) {
                    NodoStruttura _tempNodo = (NodoStruttura)x;
                    return PlantStruct.IndiceIcona(_tempNodo.Icona);
                };

                tlvStrutturaImpianto.Columns.Add(olvColumnName);

                TreeListView.TreeRenderer renderer = this.tlvStrutturaImpianto.TreeColumnRenderer;
                renderer.LinePen = new Pen(Color.Firebrick, 0.5f);
                renderer.LinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }

        }


        private void ImpostaTreeView()
        {

            // TreeListView require two delegates:
            // 1. CanExpandGetter - Can a particular model be expanded?
            // 2. ChildrenGetter - Once the CanExpandGetter returns true, ChildrenGetter should return the list of children

            // CanExpandGetter is called very often! It must be very fast.

            this.tlvStrutturaImpianto.CanExpandGetter = delegate (object x) {
                return !((NodoStruttura)x).IsLeaf;
            };

            // We just want to get the children of the given directory.
            // This becomes a little complicated when we can't (for whatever reason). We need to report the error 
            // to the user, but we can't just call MessageBox.Show() directly, since that would stall the UI thread
            // leaving the tree in a potentially undefined state (not good). We also don't want to keep trying to
            // get the contents of the given directory if the tree is refreshed. To get around the first problem,
            // we immediately return an empty list of children and use BeginInvoke to show the MessageBox at the 
            // earliest opportunity. We get around the second problem by collapsing the branch again, so it's children
            // will not be fetched when the tree is refreshed. The user could still explicitly unroll it again --
            // that's their problem :)
            this.tlvStrutturaImpianto.ChildrenGetter = delegate (object x) {
                ArrayList ChildrenList = new ArrayList();
                try
                {
                    // estraggo i nodi figli
                    NodoStruttura _tempNodo = (NodoStruttura)x;
                    ChildrenList = PlantStruct.FigliNodo(_tempNodo.Guid);
                    return ChildrenList;
                }
                catch (UnauthorizedAccessException ex)
                {
                    this.BeginInvoke((MethodInvoker)delegate () {
                        this.tlvStrutturaImpianto.Collapse(x);
                        MessageBox.Show(this, ex.Message, "tlvStrutturaImpianto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    });
                    return new ArrayList();
                }
            };

            // Once those two delegates are in place, the TreeListView starts working
            // after setting the Roots property.
            tlvStrutturaImpianto.SmallImageList = PlantStruct.ListaIcone;
            // List all drives as the roots of the tree
            ArrayList roots = PlantStruct.RadiceStruttura();
            this.tlvStrutturaImpianto.Roots = roots;
        }
        /*
        private void ImpostaColonne()
        {
            // The column setup here is identical to the File Explorer example tab --
            // nothing specific to the TreeListView. 

            // The only difference is that we don't setup anything to do with grouping,
            // since TreeListViews can't show groups.

             this.olvColumnName.ImageGetter = delegate (object x) {
                return helper.GetImageIndex(((MyFileSystemInfo)x).FullName);
            };

            // Get the size of the file system entity. 
            // Folders and errors are represented as negative numbers
            this.olvColumnSize.AspectGetter = delegate (object x) {
                MyFileSystemInfo myFileSystemInfo = (MyFileSystemInfo)x;

                if (myFileSystemInfo.IsDirectory)
                    return (long)-1;

                try
                {
                    return myFileSystemInfo.Length;
                }
                catch (System.IO.FileNotFoundException)
                {
                    // Mono 1.2.6 throws this for hidden files
                    return (long)-2;
                }
            };

            // Show the size of files as GB, MB and KBs. By returning the actual
            // size in the AspectGetter, and doing the conversion in the 
            // AspectToStringConverter, sorting on this column will work off the
            // actual sizes, rather than the formatted string.
            this.olvColumnSize.AspectToStringConverter = delegate (object x) {
                long sizeInBytes = (long)x;
                if (sizeInBytes < 0) // folder or error
                    return "";
                return Coordinator.FormatFileSize(sizeInBytes);
            };

            // Show the system description for this object
            this.olvColumnFileType.AspectGetter = delegate (object x) {
                return ShellUtilities.GetFileType(((MyFileSystemInfo)x).FullName);
            };

            // Show the file attributes for this object
            // A FlagRenderer masks off various values and draws zero or images based 
            // on the presence of individual bits.
            this.olvColumnAttributes.AspectGetter = delegate (object x) {
                return ((MyFileSystemInfo)x).Attributes;
            };
            FlagRenderer attributesRenderer = new FlagRenderer();
            attributesRenderer.ImageList = imageListSmall;
            attributesRenderer.Add(FileAttributes.Archive, "archive");
            attributesRenderer.Add(FileAttributes.ReadOnly, "readonly");
            attributesRenderer.Add(FileAttributes.System, "system");
            attributesRenderer.Add(FileAttributes.Hidden, "hidden");
            attributesRenderer.Add(FileAttributes.Temporary, "temporary");
            this.olvColumnAttributes.Renderer = attributesRenderer;

            // Tell the filtering subsystem that the attributes column is a collection of flags
            this.olvColumnAttributes.ClusteringStrategy = new FlagClusteringStrategy(typeof(FileAttributes));
        }
        */

        private void btnChiudi_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmDefinizioneImpianto_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.Width > 600)
                {

                    //lvwCicliBatteriaOld.Width = this.Width - 120;
                    tlvStrutturaImpianto.Width = this.Width - 50;
                    btnChiudi.Left = this.Width - 125;
                }

                if (this.Height > 300)
                {

                    tlvStrutturaImpianto.Height = this.Height - 115;
                    //btnApriSpybatt.Top = this.Height - 83;
                    //btnEliminaDati.Top = this.Height - 83;
                    //btnEsportaSpybatt.Top = this.Height - 83;
                    //btnImportaDati.Top = this.Height - 83;
                    btnChiudi.Top = this.Height - 83;
                    //txtIdScheda.Top = this.Height - 83;

                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }
        }

        private void tlvStrutturaImpianto_CellRightClick(object sender, CellRightClickEventArgs e)
        {
            try
            {
                e.MenuStrip = null;// this.DecideRightClickMenu(e.Model, e.Column);
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
            }

        }
    }
}
