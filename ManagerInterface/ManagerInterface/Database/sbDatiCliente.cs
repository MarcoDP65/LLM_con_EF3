using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net;
using SQLite.Net.Attributes;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using System.IO;
using log4net;
using log4net.Config;
using ChargerLogic;
using Utility;

namespace MoriData
{
    public class _sbDatiCliente
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        public string IdApparato { get; set; }
        public Int32 IdCliente { get; set; }   //Fisso a 1, al momento non è prevista cronologia

        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }

        [MaxLength(5)]
        public string DataUpdate { get; set; }
        [MaxLength(5)]
        public string DataInstall { get; set; }
        [MaxLength(120)]
        public string Client { get; set; }
        [MaxLength(120)]
        public string BatteryBrand { get; set; }
        [MaxLength(60)]
        public string BatteryModel { get; set; }
        [MaxLength(60)]
        public string BatteryId { get; set; }
        [MaxLength(120)]
        public string ClientNote { get; set; }
        public byte ClientCounter { get; set; }
        public int CicliAttesi { get; set; }
        public string SerialNumber { get; set; }

        // Quarto blocco, dati pianificazione
        public byte ModoPianificazione { get; set; }
        public byte ModoRabboccatore { get; set; }
        public byte ModoBiberonaggio { get; set; }
        public byte[] MappaTurni { get; set; }


    }

    public class sbDatiCliente
    {

        public string nullID { get { return "0000000000000000"; } }
        public _sbDatiCliente _sbdc = new _sbDatiCliente();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        private string _tempId;
        public ModelloSettimana PianificazioneCorrente;


        public sbDatiCliente()
        {
            _sbdc = new _sbDatiCliente();
            PianificazioneCorrente = new ModelloSettimana();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbDatiCliente(_db connessione)
        {
            valido = false;
            _sbdc = new _sbDatiCliente();
            PianificazioneCorrente = new ModelloSettimana();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _sbDatiCliente _caricaDati(int _id)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_sbDatiCliente>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private _sbDatiCliente _caricaDati(string _IdApparato, Int32 IdCliente)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_sbDatiCliente>()
                        where s.IdApparato == _IdApparato & s.IdCliente == IdCliente 
                    select s).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                _sbdc = _caricaDati(idLocale);
                if (_sbdc == null)
                {
                    _sbdc = new _sbDatiCliente();
                    PianificazioneCorrente = new ModelloSettimana();
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool caricaDati(string IdApparato, int IdCliente)
        {
            try
            {
                _sbdc = _caricaDati(IdApparato, IdCliente);
                if (_sbdc == null)
                {
                    _sbdc = new _sbDatiCliente();
                    PianificazioneCorrente = new ModelloSettimana();
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool salvaDati()
        {
            try
            {
                if (_sbdc.IdApparato != nullID & _sbdc.IdApparato != null & _sbdc.IdCliente != null)
                {

                    _sbDatiCliente _TestDati = _caricaDati(_sbdc.IdApparato, _sbdc.IdCliente);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _sbdc.CreationDate = DateTime.Now;
                        _sbdc.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_sbdc);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _sbdc.IdLocale = _TestDati.IdLocale;
                        _sbdc.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_sbdc);
                        _datiSalvati = true;
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
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        public bool cancellaDati()
        {
            try
            {
                SQLiteCommand CancellaRecord = _database.CreateCommand("delete from _sbDatiCliente where IdApparato = ? ", _sbdc.IdApparato);
                int esito = CancellaRecord.ExecuteNonQuery();
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        public bool cancellaDati( string IdApparato )
        {
            try
            {
                _sbdc.IdApparato = IdApparato;
                return cancellaDati();
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }


        public bool VuotaRecord( bool SalvaSeriale = false)
        {
            try
            {

                _sbdc.Client = " ";
                _sbdc.BatteryBrand = " ";
                _sbdc.BatteryModel = " ";
                _sbdc.BatteryId = " ";
                _sbdc.ClientNote = " ";
                _sbdc.ClientCounter += 1;
                _sbdc.CicliAttesi = 0;
                if (!SalvaSeriale)
                {
                    _sbdc.SerialNumber = "";
                }

                _sbdc.ModoPianificazione = 0 ;
                _sbdc.ModoRabboccatore = 0;
                _sbdc.ModoBiberonaggio = 0;
                _sbdc.MappaTurni = new byte[84];
                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("VuotaRecord: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }

        }


        #region Class Parameters

        public int IdLocale
        {
            get { return _sbdc.IdLocale; }
            set
            {
                if (value !=null )
                {
                    _sbdc.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }

        public string IdApparato
        {
            get { return _sbdc.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbdc.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }

        public int IdCliente
        {
            get { return _sbdc.IdCliente; }
            set
            {
                if (value != null)
                {
                    _sbdc.IdCliente = value;
                    _datiSalvati = false;
                }
            }
        }

        public DateTime CreationDate
        {
            get { return _sbdc.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _sbdc.RevisionDate; }
        }

        public string LastUser
        {
            get { return _sbdc.LastUser; }
        }

        public string DataUpdate
        {
            get { return _sbdc.DataUpdate; }
            set
            {
                _sbdc.DataUpdate = value;
                _datiSalvati = false;
            }
        }

        public string Client
        {
            get { return _sbdc.Client; }
            set
            {
                _sbdc.Client = FunzioniMR.StringaMax(value, 120);
                _datiSalvati = false;
            }
        }


        public string SerialNumber
        {
            get { return _sbdc.SerialNumber; }
            set
            {
                _sbdc.SerialNumber = FunzioniMR.StringaMax(value, 20);
                _datiSalvati = false;
            }
        }


        public string BatteryBrand
        {
            get { return _sbdc.BatteryBrand; }
            set
            {
                _sbdc.BatteryBrand = FunzioniMR.StringaMax( value, 120);
                _datiSalvati = false;
            }
        }

        public string BatteryModel
        {
            get { return _sbdc.BatteryModel; }
            set
            {
                _sbdc.BatteryModel = FunzioniMR.StringaMax(value, 60);
                _datiSalvati = false;
            }
        }

        public string BatteryId
        {
            get { return _sbdc.BatteryId; }
            set
            {
                _sbdc.BatteryId = FunzioniMR.StringaMax(value, 60);
                _datiSalvati = false;
            }
        }

        public string ClientNote
        {
            get { return _sbdc.ClientNote; }
            set
            {
                _sbdc.ClientNote = FunzioniMR.StringaMax(value, 127);
                _datiSalvati = false;
            }
        }

        public byte ClientCounter
        {
            get { return _sbdc.ClientCounter; }
            set
            {
                _sbdc.ClientCounter = value;
                _datiSalvati = false;
            }
        }

        public int CicliAttesi
        {
            get { return _sbdc.CicliAttesi; }
            set
            {
                _sbdc.CicliAttesi = value;
                _datiSalvati = false;
            }
        }

        public byte ModoPianificazione
        {
            get { return _sbdc.ModoPianificazione; }
            set
            {
                _sbdc.ModoPianificazione = value;
                _datiSalvati = false;
            }
        }


        public byte ModoRabboccatore
        {
            get { return _sbdc.ModoRabboccatore; }
            set
            {
                _sbdc.ModoRabboccatore = value;
                _datiSalvati = false;
            }
        }

        public byte ModoBiberonaggio
        {
            get { return _sbdc.ModoBiberonaggio; }
            set
            {
                _sbdc.ModoBiberonaggio = value;
                _datiSalvati = false;
            }
        }


        public byte[] MappaTurni
        {
            get { return _sbdc.MappaTurni; }
            set
            {
                _sbdc.MappaTurni = value;
                _datiSalvati = false;
            }
        }




        #endregion Class Parameter


        /// <summary>
        /// Genera il Bytearray per l'esportazione
        /// </summary>
        public byte[] DataArray
        {
            get
            {
                byte[] _datamap = new byte[960];
                int _arrayInit = 0;
                // Area composta da 4 settori da 240 bytes l'uno
                // I primi 11 bytes di ogni settore sono il comando
                // 0000000000000000 00000000 22
                // |-- ID SCHEDA -| |-Dev -| cmd

                // Variabili temporanee per il passaggio dati
                byte _byte1 = 0;
                byte _byte2 = 0;
                byte _byte3 = 0;
                byte _byte4 = 0;
                byte[] _tempArr;

                // Preparo l'array vuoto
                for (int _i = 0; _i < 960; _i++)
                {
                    _datamap[_i] = 0x00;
                    // 
                }
                // Testata BLOCCO 1 --> 0x001000
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;

                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;

                _datamap[_arrayInit++] = 0x22;

                //Id pacchetto
                _datamap[_arrayInit++] = 0x01;

                // Zona Dati
                _tempArr = FunzioniComuni.StringToArray(_sbdc.Client, 110);
                for (int _i = 0; _i < 110; _i++)
                    _datamap[_arrayInit++] = _tempArr[_i];

                _tempArr = FunzioniComuni.StringToArray(_sbdc.ClientNote, 110);
                for (int _i = 0; _i < 110; _i++)
                    _datamap[_arrayInit++] = _tempArr[_i];
                // Mi porto al blocco 2

                _arrayInit += 8;

                // Testata BLOCCO 2
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;

                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;

                _datamap[_arrayInit++] = 0x22;

                //Id pacchetto
                _datamap[_arrayInit++] = 0x02;
                // Zona Dati
                _tempArr = FunzioniComuni.StringToArray(_sbdc.BatteryBrand, 110);
                for (int _i = 0; _i < 110; _i++)
                    _datamap[_arrayInit++] = _tempArr[_i];

                _tempArr = FunzioniComuni.StringToArray(_sbdc.BatteryModel, 55);
                for (int _i = 0; _i < 55; _i++)
                    _datamap[_arrayInit++] = _tempArr[_i];

                _tempArr = FunzioniComuni.StringToArray(_sbdc.BatteryId, 50);
                for (int _i = 0; _i < 50; _i++)
                    _datamap[_arrayInit++] = _tempArr[_i];

                FunzioniComuni.SplitUshort((ushort)_sbdc.CicliAttesi, ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte1;

                // Mi porto al blocco 3

                _arrayInit += 11;

                // Testata BLOCCO 3
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;

                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;

                _datamap[_arrayInit++] = 0x22;

                //Id pacchetto
                _datamap[_arrayInit++] = 0x03;
                // Zona Dati
                _tempArr = FunzioniComuni.StringToArray(_sbdc.SerialNumber, 20);
                for (int _i = 0; _i < 20; _i++)
                    _datamap[_arrayInit++] = _tempArr[_i];


                // Mi porto al blocco 4

                _arrayInit += 208;

                // Testata BLOCCO 4
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;

                _datamap[_arrayInit++] = 0x00;
                _datamap[_arrayInit++] = 0x00;

                _datamap[_arrayInit++] = 0x22;

                //Id pacchetto
                _datamap[_arrayInit++] = 0x04;
                // Zona Dati



                return _datamap;
            }

        }


    }

}
