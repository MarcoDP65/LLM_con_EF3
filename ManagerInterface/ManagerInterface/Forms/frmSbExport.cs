using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using MoriData;
using ChargerLogic;
using log4net;
using log4net.Config;
using BrightIdeasSoftware;
using Utility;
using Newtonsoft.Json;

namespace PannelloCharger
{
    public partial class frmSbExport : Form
    {
        parametriSistema _parametri;
        //MessaggioSpyBatt _msg;
        LogicheBase _logiche;
        //sbDataModel _tempDati;

        //bool _apparatoPresente = false;
        ImageDump _Immagine;
        MessaggioSpyBatt.comandoInizialeSB _NuovaIntestazioneSb;

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
       

        public elementiComuni.modoDati modo = elementiComuni.modoDati.Import;
        //static 
        UnitaSpyBatt _sb;
        //string IdCorrente;


        public frmSbExport()
        {
            InitializeComponent();
 
        }

        public void Setmode(elementiComuni.modoDati azione)
        {
            try 
            {
                modo = azione;

                switch (azione)
                {
                    case elementiComuni.modoDati.Import:
                        {
                            this.Text = StringheComuni.ImportaDati;           // "Importa Dati";
                            btnDataExport.Text = StringheComuni.CaricaDati;   // "Carica Dati";
                            modo = elementiComuni.modoDati.Import;
                            btnDataExport.Enabled = false;
                            btnAnteprima.Visible = true;
                            btnEstract.Visible = false;
                        }
                        break;
                    case elementiComuni.modoDati.Output:
                        {
                            this.Text = StringheComuni.EsportaDati;           // "Esporta Dati";
                            btnDataExport.Text = StringheComuni.SalvaDati;    // "Salva Dati";
                            modo = elementiComuni.modoDati.Output;
                            btnEstract.Visible = false;
                            btnDataExport.Enabled = true;
                            btnAnteprima.Visible = false;
                        }
                        break;
                    case elementiComuni.modoDati.HexDumpRecovery:
                        {
                            this.Text = "HEXDUMP Recovery";                    // "Importa HEXDUMP";
                            btnDataExport.Text = StringheComuni.SalvaDati;     // "Carica Dati";
                            modo = elementiComuni.modoDati.HexDumpRecovery;
                            btnDataExport.Enabled = false;
                            btnAnteprima.Visible = true;
                            btnEstract.Visible = true;
                            btnEstract.Enabled = false;

                        }
                        break;
                    default:
                        {
                            this.Text = StringheComuni.EsportaDati;           // "Esporta Dati";
                            btnDataExport.Text = StringheComuni.SalvaDati;    // "Salva Dati";
                            modo = elementiComuni.modoDati.Output;
                            btnDataExport.Enabled = true;
                            btnAnteprima.Visible = false;
                            btnEstract.Visible = false;
                        }
                        break;
                }

            }
            catch (Exception Ex)
            {
                Log.Error("Setmode: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }
        }



        public frmSbExport(ref parametriSistema _par, bool CaricaDati, string IdApparato, LogicheBase Logiche, bool SerialeCollegata, bool AutoUpdate)
        {
            bool _esito;
            try
            {

                _parametri = _par;
                InitializeComponent();
                btnDataExport.Enabled = false;
                ResizeRedraw = true;
                _logiche = Logiche;
                if (CaricaDati)
                {
                    _sb = new UnitaSpyBatt(ref _parametri, _logiche.dbDati.connessione, Logiche.currentUser.livello);
                    string _idCorrente = IdApparato;
                    _esito = _sb.CaricaCompleto(IdApparato, _logiche.dbDati.connessione);
                    if (_esito) btnDataExport.Enabled = true;
                    MostraDati();

                    string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    _pathTeorico += "\\LADELIGHT Manager\\SPY-BATT";

                    string _nomeProposto;
                    if (_sb.sbCliente.SerialNumber != "" & _sb.sbCliente.SerialNumber!= null)
                        _nomeProposto = _sb.sbCliente.SerialNumber;
                    else
                    {
                        if (_sb.sbCliente.BatteryId != "")
                            _nomeProposto = _sb.sbCliente.BatteryId;
                        else
                            _nomeProposto = _sb.Id;

                    }
                    _nomeProposto += ".sbdata";

                    txtNuovoFile.Text = _pathTeorico + "\\" + _nomeProposto;


                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmSbExport: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }

        
        }
        
        public bool MostraDati()
        {
            try
            {

                txtMatrSB.Text = FunzioniMR.StringaSeriale(_sb.Id);
                txtCliente.Text = _sb.sbCliente.Client;
                txtNote.Text = _sb.sbCliente.ClientNote;
                txtManufcturedBy.Text = _sb.sbData.ProductId;

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("MostraDati: " + Ex.Message);
                return false;
            }
        }

        public bool MostraDatiImmagine()
        {
            try
            {

                txtMatrSB.Text = FunzioniMR.StringaSeriale(_sb.sbData.Id);
                txtCliente.Text = _sb.ModelloDati.Cliente.Client;
                txtNote.Text = _sb.ModelloDati.Cliente.ClientNote;
                txtManufcturedBy.Text = _sb.sbData.ProductId;
                txtNumLunghi.Text = _sb.sbData.ContLunghi.ToString();
                txtNumBrevi.Text = _sb.sbData.ContBrevi.ToString();
                txtManufcturedBy.Text = _sb.sbData.LongMem.ToString() + " / " + _sb.sbData.ContBrevi.ToString() + "  ( Prg: " + _sb.sbData.ProgramCount.ToString() + " )";


                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("MostraDati: " + Ex.Message);
                return false;
            }
        }

        public bool MostraTestataHexDump()
        {
            try
            {

                if (_sb.sbData == null)
                {
                    return false;
                }
                else
                {
                    txtMatrSB.Text = FunzioniMR.StringaSeriale(_sb.sbData.Id);
                    if (_NuovaIntestazioneSb == null)
                        txtCliente.Text = "Record Contatori Mancante";
                    else
                    {

                        if (_NuovaIntestazioneSb != null)
                        {
                            txtCliente.Text = "(N) " + _NuovaIntestazioneSb.longRecordCounter.ToString() + "/" + _NuovaIntestazioneSb.longRecordPoiter.ToString();
                        }
                        else
                        {
                            txtCliente.Text = _Immagine.IntestazioneSb.longRecordCounter.ToString() + "/" + _Immagine.IntestazioneSb.longRecordPoiter.ToString();
                        }

                    }
                    txtFwVersion.Text = _sb.sbData.SwVersion;
                    txtNote.Text = "";
                    txtNumLunghi.Text = _sb.sbData.ContLunghi.ToString();
                    txtNumBrevi.Text = _sb.sbData.ContBrevi.ToString();
                    txtManufcturedBy.Text = _sb.sbData.LongMem.ToString() + " / " + _sb.sbData.ContBrevi.ToString() + "  ( Prg: " + _sb.sbData.ProgramCount.ToString() + " )";


                    return true;
                }
            }
            catch (Exception Ex)
            {
                Log.Error("MostraDati: " + Ex.Message);
                return false;
            }
        }




        private void chkChiudi_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void esportaDati()
        {
            string filePath = "";


            if (txtNuovoFile.Text != "")
            {
                filePath = txtNuovoFile.Text;
                if (!File.Exists(filePath)) File.Create(filePath).Close();
                Log.Debug("file prepara esportazione");
                _sb.PreparaEsportazione(true, true, true, true,true);
                string JsonData = JsonConvert.SerializeObject(_sb.ModelloDati);
                Log.Debug("file generato");
                // Prima comprimo i dati
                string JsonZip = FunzioniComuni.CompressString(JsonData);

                // Ora cifro i dati
                string JsonEncript = StringCipher.Encrypt(JsonZip);
                

                //JsonEncript = JsonData;
                Log.Debug("file criptato");
                File.WriteAllText(filePath, JsonEncript);
                //File.WriteAllText(filePath+ ".txt", JsonData);

                Log.Debug("file salvato");

                //                MessageBox.Show("File generato", "Esportazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MessageBox.Show(StringheComuni.FileGenerato, StringheComuni.EsportazioneDati, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
//                MessageBox.Show("Inserire un nome valido", "Esportazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show( StringheComuni.InserireNome, StringheComuni.EsportazioneDati, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Importa il file dati, deserializza la classe e verifica la coerenza dei dati
        /// </summary>
        /// <returns></returns>
        private bool importaDati()
        {
            try
            {

                string filePath = "";


                if (txtNuovoFile.Text != "")
                {
                    filePath = txtNuovoFile.Text;
                    if (File.Exists(filePath))
                    {
                        Log.Debug("Inizio Import");
                        string _fileDecripted = "";
                        string _fileDecompress = "";
                        string _fileImport = File.ReadAllText(filePath);
                        Log.Debug("file caricato: len = " + _fileImport.Length.ToString());

                        _fileDecripted = StringCipher.Decrypt(_fileImport);
                        if(_fileDecripted != "")
                        {
                            //il file è cifrato
                            Log.Debug("file criptato");
                            _fileImport = _fileDecripted;
                        }
                        Log.Debug("file decriptato");

                        // verifico se è compresso
                        _fileDecompress = FunzioniComuni.DecompressString(_fileImport);
                        if (_fileDecompress != "")
                        {

                            //è compresso
                            _fileImport = _fileDecompress; 

                        }


                        sbDataModel _importData;
                        elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
                        ushort _crc;

                        _importData = JsonConvert.DeserializeObject<sbDataModel>(_fileImport);

                        Log.Debug("file convertito");

                        ushort _tempCRC = _importData.CRC;
                        _importData.CRC = 0;

                        string _tempSer = JsonConvert.SerializeObject(_importData);
                        byte[] _tempBSer = FunzioniMR.GetBytes(_tempSer);

                        // rivedere il controllo crc
                        _crc = _tempCRC;  // codCrc.ComputeChecksum(_tempBSer);
                        Log.Debug("file verificato");


                        if (_crc == _tempCRC)
                        { // I CRC ciincidono: dati validi
                            _sb.ModelloDati = _importData;
                            _sb.importaModello(_logiche.dbDati.connessione,true, true, true, true, true);
                            MostraDati();

                        }
                        else
                        {
//                            MessageBox.Show("File danneggiato: impossibile caricare i dati", "Importazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            MessageBox.Show(StringheComuni.FileDanneggiato, StringheComuni.ImportaDati, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                    }

                    return true;
                }
                else
                {
//                    MessageBox.Show("Inserire un nome valido", "Importazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(StringheComuni.InserireNome, StringheComuni.ImportaDati, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            catch (Exception Ex)
            {
//                MessageBox.Show("Dati non validi", "Importazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(StringheComuni.DatiNonValidi, StringheComuni.ImportaDati, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("MostraDati: " + Ex.Message);
                return false;

            }
        }

        private bool importaHexdump()
        {
            try
            {

                string filePath = "";


                if (txtNuovoFile.Text != "")
                {
                    filePath = txtNuovoFile.Text;
                    if (File.Exists(filePath))
                    {
                        Log.Debug("Inizio Import");
                        string _fileImport = File.ReadAllText(filePath);
                        Log.Debug("file caricato: len = " + _fileImport.Length.ToString());
                        //sbDataModel _importData;
                        _Immagine = JsonConvert.DeserializeObject<ImageDump>(_fileImport);
                        _NuovaIntestazioneSb = _Immagine.IntestazioneSb;
                        Log.Debug("file convertito");
                        _sb.sbData = _Immagine.Testata;

                        //_sb.ModelloDati = _importData;
                        //_sb.importaModello(_logiche.dbDati.connessione, true, true, true, true, true);
                        MostraTestataHexDump();

                    }
                    else
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    //                    MessageBox.Show("Inserire un nome valido", "Importazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show(StringheComuni.InserireNome, StringheComuni.ImportaDati, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            catch (Exception Ex)
            {
                //                MessageBox.Show("Dati non validi", "Importazione dati Apparato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(StringheComuni.DatiNonValidi, StringheComuni.ImportaDati, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("MostraDati: " + Ex.Message);
                return false;

            }
        }


        private bool salvaImportazione()
        {
            try
            {
                _sb.importaModello(_logiche.dbDati.connessione,true, true, true, true, true);
                _sb.sbData._database = _logiche.dbDati.connessione;
                if (_sb.recordPresente(_sb.sbData.Id, _logiche.dbDati.connessione))
                {
                    DialogResult risposta = MessageBox.Show(StringheComuni.DatiGiaPresenti + "\n " + FunzioniMR.StringaSeriale(_sb.ModelloDati.ID) + "  -  " + _sb.ModelloDati.Cliente.ClientNote + " \n Rimpiazzo i dati esistenti ? ", "SPY-BATT", MessageBoxButtons.YesNo);

                    if (risposta == System.Windows.Forms.DialogResult.Yes)
                    {
                        //UnitaSpyBatt _sb = new UnitaSpyBatt(ref _parametri, _logiche.dbDati.connessione);
                        int _numClone = _sb.CreaClone();
                        lblNumClone.Visible = true;
                        txtNumClone.Visible = true;
                        txtNumClone.Text = _numClone.ToString("000");
                        Application.DoEvents();
                        _sb.sbData.cancellaDati(_sb.sbData.Id);
                    }
                    else return false;
                }

                bool _esito = _sb.sbData.salvaDati();

                //_sb.sbCliente._database = _logiche.dbDati.connessione;
                _sb.sbCliente.IdCliente = 1;
                _esito = _esito && _sb.sbCliente.salvaDati();
                Log.Warn("Salva Info Comuni ");
                foreach (sbProgrammaRicarica _prog in _sb.Programmazioni)
                {
                    _prog._database = _logiche.dbDati.connessione;
                    _prog.salvaDati();
                }
                Log.Warn("Fine SalvaProgrammazioni: " + _sb.Programmazioni.Count.ToString());

                foreach (sbMemLunga _lunga in _sb.CicliMemoriaLunga)
                {
                    Log.Warn("Fine SalvaLunga: ---------------------------------------------------------------------------");
                    _lunga._database = _logiche.dbDati.connessione;
                    if(_lunga.IdProgramma != 0)
                    {
                        _lunga.CaricaProgramma();
                    }
                    _lunga.salvaDati();
                    Log.Warn("Fine SalvaLunga: " + _lunga.IdMemoriaLunga.ToString());
                    /*foreach (sbMemBreve _breve in _lunga.CicliMemoriaBreve)
                    {
                        _breve.salvaDati();
                    }
                    Log.Warn("Fine SalvaBrevi: " + _lunga.CicliMemoriaBreve.Count.ToString());
                     */
                    _lunga.SalvaBrevi();
                    Log.Warn("Fine SalvaBrevi compact : " );

                }
                Log.Warn("Fine SalvaLunghi: " + _sb.CicliMemoriaLunga.Count.ToString());
                _sb.ConsolidaBrevi();
                return _esito ;

            }

            catch (Exception Ex)
            {
                MessageBox.Show(StringheComuni.DatiNonValidi, StringheComuni.ImportaDati, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Error("MostraDati: " + Ex.Message);
                return false;

            }
        }


        private void btnDataExport_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "";

                //this.Parent.UseWaitCursor = true;
                this.Cursor = Cursors.WaitCursor;

                switch (modo)
                {
                    case elementiComuni.modoDati.Import:
                        if (salvaImportazione()) this.Close();
                        break;
                    case elementiComuni.modoDati.Output:
                        esportaDati();
                        break;
                    case elementiComuni.modoDati.HexDumpRecovery:
                        if (_sb.ModelloDati.ID == "")
                        {
                            return;
                        }

                        if (salvaImportazione())
                        {
                            ApriSpyBatt(_sb.sbData.Id);
                            this.Close();
                        }
                        break;
                    default:
                        break;
                }
            }
            catch
            {

            }

            finally
            {
                this.Cursor = Cursors.Default;
                //this.Parent.UseWaitCursor = false;
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
            catch (Exception Ex)
            {
                Log.Error("ApriSpyBatt: " + Ex.Message);
            }

        }


        private void btnSfoglia_Click(object sender, EventArgs e)
        {
            try
            {

                lblNumClone.Visible = false;
                txtNumClone.Visible = false;

                switch (modo)
                {
                    case elementiComuni.modoDati.Import:
                        {
                            ofdImportDati.Title = StringheComuni.ImportaDati;
                            ofdImportDati.CheckFileExists = false;
                            ofdImportDati.Filter = "SPY-BATT exchange data (*.sbdata)|*.sbdata|All files (*.*)|*.*";
                            // Propongo come directory iniziale  user\documents\LADELIGHT Manager\SPY-BATT
                            string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            _pathTeorico += "\\LADELIGHT Manager\\SPY-BATT";
                            if (!Directory.Exists(_pathTeorico))
                            {
                                Directory.CreateDirectory(_pathTeorico);
                            }
                            sfdExportDati.InitialDirectory = _pathTeorico;
                            ofdImportDati.ShowDialog();
                            txtNuovoFile.Text = ofdImportDati.FileName;
                            if (importaDati()) btnDataExport.Enabled = true;
                        }
                        break;
                    case elementiComuni.modoDati.Output:
                        {
                            string _filename = "";

                            sfdExportDati.Title = StringheComuni.EsportaDati;
                            sfdExportDati.Filter = "SPY-BATT exchange data (*.sbdata)|*.sbdata|All files (*.*)|*.*";
                            // Propongo come directory iniziale  user\documents\LADELIGHT Manager\SPY-BATT
                            string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            _pathTeorico += "\\LADELIGHT Manager\\SPY-BATT";
                            if (!Directory.Exists(_pathTeorico))
                            {
                                Directory.CreateDirectory(_pathTeorico);
                            }
                            sfdExportDati.InitialDirectory = _pathTeorico;

                            if (txtNuovoFile.Text != "")
                            {
                                sfdExportDati.FileName = txtNuovoFile.Text;

                            }

                            sfdExportDati.ShowDialog();
                            txtNuovoFile.Text = sfdExportDati.FileName;

                        }
                        break;
                    case elementiComuni.modoDati.HexDumpRecovery:
                        {
                            ofdImportDati.Title = "HEXDUMP RECOVERY"; // StringheComuni.ImportaDati;
                            ofdImportDati.CheckFileExists = false;
                            ofdImportDati.Filter = "SPY-BATT HexDump data (*.sbx)|*.sbx|All files (*.*)|*.*";
                            // Propongo come directory iniziale  user\documents\LADELIGHT Manager\SPY-BATT
                            string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            _pathTeorico += "\\LADELIGHT Manager\\SPY-BATT";
                            if (!Directory.Exists(_pathTeorico))
                            {
                                Directory.CreateDirectory(_pathTeorico);
                            }
                            sfdExportDati.InitialDirectory = _pathTeorico;
                            ofdImportDati.ShowDialog();
                            txtNuovoFile.Text = ofdImportDati.FileName;
                            if (importaHexdump()) btnDataExport.Enabled = true;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Log.Error("btnSfoglia_Click: " + Ex.Message);
            }
        }

        private void btnSfogliaAnalisi_Click(object sender, EventArgs e)
        {
            try
            {
                switch (modo)
                {
                    case elementiComuni.modoDati.Import:
                        {

                            txtFileAnalisi.Text = "";
                        }
                        break;
                    case elementiComuni.modoDati.Output:
                        {
                            txtFileAnalisi.Text = "";
                        }
                        break;
                    case elementiComuni.modoDati.HexDumpRecovery:
                        {


                            sfdExportDati.Title = "Esito Analisi HexDump";
                            sfdExportDati.Filter = "TXT (*.txt)|*.txt|All files (*.*)|*.*";
                            // Propongo come directory iniziale  user\documents\LADELIGHT Manager\SPY-BATT
                            string _pathTeorico = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            _pathTeorico += "\\LADELIGHT Manager\\SPY-BATT";
                            if (!Directory.Exists(_pathTeorico))
                            {
                                Directory.CreateDirectory(_pathTeorico);
                            }
                            sfdExportDati.InitialDirectory = _pathTeorico;

                            if (txtFileAnalisi.Text != "")
                            {
                                sfdExportDati.FileName = txtFileAnalisi.Text;

                            }

                            sfdExportDati.ShowDialog();
                            txtFileAnalisi.Text = sfdExportDati.FileName;

                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception Ex)
            {
                Log.Error("btnSfogliaAnalisi_Click: " + Ex.Message);
            }
        }


        private void btnAnteprima_Click(object sender, EventArgs e)
        {
            try
            {
                lblNumClone.Visible = false;
                txtNumClone.Visible = false;

                switch (modo)
                {
                    case elementiComuni.modoDati.Import:
                        if (importaDati()) btnDataExport.Enabled = true;
                        break;
                    case elementiComuni.modoDati.Output:
                        break;
                    case elementiComuni.modoDati.HexDumpRecovery:
                        if (importaHexdump())
                        {
                            //_sb.AnalizzaHexDump(_Immagine.Testata.Id, null, _Immagine, false, true);

                            btnEstract.Enabled = true;
                            btnDataExport.Enabled = true;
                        }
                        break;
                    default:
                        break;
                }

                
            }
            catch (Exception Ex)
            {
               
            }

        }

        private void btnEstract_Click(object sender, EventArgs e)
        {
            try
            {
                //this.UseWaitCursor = true;

                this.Cursor = Cursors.WaitCursor;
                lblNumClone.Visible = false;
                txtNumClone.Visible = false;
                switch (modo)
                {
                    case elementiComuni.modoDati.Import:
                        if (importaDati()) btnDataExport.Enabled = true;
                        break;
                    case elementiComuni.modoDati.Output:
                        break;
                    case elementiComuni.modoDati.HexDumpRecovery:
                        if (importaHexdump())
                        {
                            _NuovaIntestazioneSb = _Immagine.IntestazioneSb;
                            _sb.AnalizzaHexDump(_Immagine.Testata.Id, null, _Immagine, false, true,true, txtNuovoFile.Text+"_decoded");

                            //MostraTestataHexDump();
                            MostraDatiImmagine();

                            if (_NuovaIntestazioneSb == null)
                            {
                                string _tempSer = JsonConvert.SerializeObject(_Immagine);
                                if (!File.Exists(txtNuovoFile.Text)) File.Create(txtNuovoFile.Text).Close();
                                File.WriteAllText(txtNuovoFile.Text, _tempSer);
                                
                            }
                            Log.Warn("---------------------- FILE RIGENERATO ------------------------------");
                            Log.Warn(txtNuovoFile.Text);
                            btnDataExport.Enabled = true;
                        
                        }
                        break;
                    default:
                        break;
                }


            }
            catch (Exception Ex)
            {

            }
            finally
            {
                //this.Parent.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
            }
        }


        private void frmSbExport_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
