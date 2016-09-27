using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

using SQLite.Net;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;

namespace ChargerLogic
{
    public partial class DisplaySetup
    {
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public DataModel Modello;


        private bool _datisalvati = true;

        /*
        public List<Immagine> Immagini = new List<Immagine>();
        public List<Schermata> Schermate = new List<Schermata>();
        public List<Schermata> Variabili = new List<Schermata>();
        */


        public DisplaySetup()
        {
            Modello = new DataModel();

            Modello.DataCreazione = DateTime.Now;
            Modello.DataModifica = Modello.DataCreazione;
        }



        public bool SalvaFile(string NomeFile, bool Encode = false)
        {
            bool _esito = false;
            ushort _crc;
            elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
            try
            {
                if (NomeFile != "")
                {
                    Log.Debug("--- Esportazione Modello Display ---");
                    Modello.DataModifica = DateTime.Now;
                    Modello.CRC = 0;
                    string _tempSer = JsonConvert.SerializeObject(Modello);
                    byte[] _tepBSer = FunzioniMR.GetBytes(_tempSer);

                    _crc = codCrc.ComputeChecksum(_tepBSer);
                    Modello.CRC = _crc;

                    if (!File.Exists(NomeFile)) File.Create(NomeFile).Close();
                    Log.Debug("file prepara esportazione");
                    string JsonData = JsonConvert.SerializeObject(Modello);
                    Log.Debug("file generato");
                    string JsonEncript;
                    if (Encode)
                    {
                        // Ora cifro i dati
                         JsonEncript = StringCipher.Encrypt(JsonData);
                        Log.Debug("file criptato");
                    }
                    else
                    {
                        JsonEncript = JsonData;
                    }


                    File.WriteAllText(NomeFile, JsonEncript);
                    File.WriteAllText(NomeFile + ".txt", JsonData);

                    Log.Debug("file salvato");

                    _datisalvati = true;
                    _esito = true;
                }

                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return _esito;
            }
        }

        /// <summary>
        /// Importa il file dati, deserializza la classe e verifica la coerenza dei dati
        /// </summary>
        /// <returns></returns>
        public bool CaricaFile(string NomeFile)
        {
            bool _esito = false;
            try
            {

                if (NomeFile != "")
                {

                    if (File.Exists(NomeFile))
                    {
                        Log.Debug("--- Importazione Modello Display ---");
                        Log.Debug("Inizio Import");
                        string _fileDecripted = "";
                        string _fileImport = File.ReadAllText(NomeFile);
                        Log.Debug("file caricato: len = " + _fileImport.Length.ToString());

                        _fileDecripted = StringCipher.Decrypt(_fileImport);
                        if (_fileDecripted != "")
                        {
                            //il file è cifrato
                            Log.Debug("file criptato");
                            _fileImport = _fileDecripted;
                        }
                        Log.Debug("file decriptato");
                        DataModel _importData;
                        elementiComuni.Crc16Ccitt codCrc = new elementiComuni.Crc16Ccitt(elementiComuni.InitialCrcValue.NonZero1);
                        ushort _crc;

                        _importData = JsonConvert.DeserializeObject<DataModel>(_fileImport);

                        Log.Debug("file convertito");

                        ushort _tempCRC = _importData.CRC;
                        _importData.CRC = 0;

                        string _tempSer = JsonConvert.SerializeObject(_importData);
                        byte[] _tempBSer = FunzioniMR.GetBytes(_tempSer);

                        // rivedere il controllo crc
                        _crc = _tempCRC;  // codCrc.ComputeChecksum(_tempBSer);
                        Log.Debug("file verificato");
                        Modello = _importData;

                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }

            catch (Exception Ex)
            {

                Log.Error("CaricaFile Display: " + Ex.Message);
                return false;

            }
        }






    }
}
