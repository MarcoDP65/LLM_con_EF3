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
                if (azione == elementiComuni.modoDati.Import)
                {
                    this.Text = StringheComuni.ImportaDati;           // "Importa Dati";
                    btnDataExport.Text = StringheComuni.CaricaDati;   // "Carica Dati";
                    modo = elementiComuni.modoDati.Import;
                    btnDataExport.Enabled = false;
                    btnAnteprima.Visible = true;
                }
                else
                {
                    this.Text = StringheComuni.EsportaDati;           // "Esporta Dati";
                    btnDataExport.Text = StringheComuni.SalvaDati;    // "Salva Dati";
                    modo = elementiComuni.modoDati.Output;
                    btnDataExport.Enabled = true;
                    btnAnteprima.Visible = false;
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
                // Ora cifro i dati
                string JsonEncript = StringCipher.Encrypt(JsonData);

                Log.Debug("file criptato");
                File.AppendAllText(filePath, JsonEncript);

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

        private bool salvaImportazione()
        {
            try
            {
                _sb.importaModello(_logiche.dbDati.connessione,true, true, true, true, true);
                if (_sb.recordPresente(_sb.ModelloDati.ID, _logiche.dbDati.connessione))
                {
                    DialogResult risposta = MessageBox.Show(StringheComuni.DatiGiaPresenti + "\n " + FunzioniMR.StringaSeriale(_sb.ModelloDati.ID) + "  -  " + _sb.ModelloDati.Cliente.ClientNote + " \n Rimpiazzo i dati esistenti ? ", "SPY-BATT", MessageBoxButtons.YesNo);

                    if (risposta == System.Windows.Forms.DialogResult.Yes)
                    {
                        //UnitaSpyBatt _sb = new UnitaSpyBatt(ref _parametri, _logiche.dbDati.connessione);
                        _sb.sbData.cancellaDati(_sb.ModelloDati.ID);
                    }
                    else return false;
                }
                bool _esito = _sb.sbData.salvaDati();
                _esito = _esito && _sb.sbCliente.salvaDati();
                Log.Warn("SalvaTesate ");
                foreach (sbProgrammaRicarica _prog in _sb.Programmazioni)
                {
                    _prog.salvaDati();
                }
                Log.Warn("Fine SalvaProgrammazioni: " + _sb.Programmazioni.Count.ToString());

                foreach (sbMemLunga _lunga in _sb.CicliMemoriaLunga)
                {
                    Log.Warn("Fine SalvaLunga: ---------------------------------------------------------------------------");
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

            string filePath = "";

            this.Cursor = Cursors.WaitCursor;
            if (modo == elementiComuni.modoDati.Output)
            {
                esportaDati();
            }
            else
            {
                if (salvaImportazione()) this.Close();
            }
            this.Cursor = Cursors.Default;
        }

      

        private void btnSfoglia_Click(object sender, EventArgs e)
        {


            if (modo == elementiComuni.modoDati.Output)
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
            else
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
        }

        private void btnAnteprima_Click(object sender, EventArgs e)
        {
            if (importaDati()) btnDataExport.Enabled = true;
        }

        private void frmSbExport_Load(object sender, EventArgs e)
        {

        }
    }
}
