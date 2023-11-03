using ChargerLogic;
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
using log4net.Config;
using BrightIdeasSoftware;


namespace PannelloCharger.Forms_Archivi
{
    public partial class frmGestioneArchivi : Form
    {
        private static ILog Log = LogManager.GetLogger("Forms_Archivi");


        public parametriSistema _varGlobali;
        public LogicheBase logiche;
        public frmMain FormPrincipale { get; set; }

        public frmGestioneArchivi(ref parametriSistema varGlobali, bool CaricaDati, string IdApparato, LogicheBase Logiche, bool SerialeCollegata, bool AutoUpdate)
        {
            try
            {


                InitializeComponent();
                //           UsbNotification.RegisterUsbDeviceNotification(this.Handle);

                _varGlobali = varGlobali;



            }
            catch (Exception Ex)
            {
                Log.Error("frmSetupLLT: " + Ex.Message);
            }
        }
    }
}
