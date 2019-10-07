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
using FTD2XX_NET;

using NextUI.Component;
using NextUI.Frame;

using BrightIdeasSoftware;
using Utility;
using MoriData;
using ChargerLogic;
using MdiHelper;
using log4net;
using log4net.Config;
using System.Resources;

namespace PannelloCharger
{

    public partial class frmIdProgrammer : frmSpyBat
    {
        public frmIdProgrammer(ref parametriSistema _par, bool CaricaDati, string IdApparato, LogicheBase Logiche, bool SerialeCollegata, bool AutoUpdate) : base(ref _par, CaricaDati, IdApparato, Logiche, SerialeCollegata,AutoUpdate)
        {

        }

        public frmIdProgrammer() : base()
        {

        }

        public override void applicaAutorizzazioni()
        {
            base.applicaAutorizzazioni();
            AdattaGenerale();
            tabCaricaBatterie.TabPages.Remove(tabStatSintesi);
            tabCaricaBatterie.TabPages.Remove(tabStatCockpit);
            tabCaricaBatterie.TabPages.Remove(tabStatComparazioni);
            tabCaricaBatterie.TabPages.Remove(tabStatistiche);
            tabCaricaBatterie.TabPages.Remove(tabCb04);
            tabCaricaBatterie.TabPages.Remove(tabCb02);                // Configurazione SB
            // tabCaricaBatterie.TabPages.Remove(tbpProfiloLLPro);        Profilo LL
            tabCaricaBatterie.TabPages.Remove(tbpPianificazione);      // Pianificazione
            tabCaricaBatterie.TabPages.Remove(tabCb05);
            tabCaricaBatterie.TabPages.Remove(tabSbFact);
            tabCaricaBatterie.TabPages.Remove(tbpCalibrazioni);
            tabCaricaBatterie.TabPages.Remove(tbpClonaScheda);
            tabCaricaBatterie.TabPages.Remove(tabCalibrazione);
            tabCaricaBatterie.TabPages.Remove(tbpStrategia);
            tabCaricaBatterie.TabPages.Remove(tbpEsp32);
        }

        public void AdattaGenerale()
        {
            try
            {
                grbDatiCliente.Visible = false;
                grbCloneScheda.Visible = false;
                grbMainDlOptions.Visible = false;
                grbTestataContatori.Visible = false;
                grbAbilitazioneReset.Visible = false;
                grbDownloadDati.Visible = false;
                btnSalvaCliente.Visible = false;

                grbTestata.Top = 32;
                grbTestata.Left = 36;
             

            }
            catch(Exception Ex)
            {
                Log.Error("Form IDPrg AdattaGenerale: " + Ex.Message, Ex);
            }

        }
    }
}
