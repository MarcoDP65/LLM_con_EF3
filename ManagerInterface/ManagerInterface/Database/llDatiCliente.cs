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
    public class _llDatiCliente
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        [Indexed(Name = "IdxLLCliente", Order = 1, Unique = true)]
        public string IdApparato { get; set; }
        [Indexed(Name = "IdxLLCliente", Order = 2, Unique = true)]
        public Int32 IdCliente { get; set; }   //Fisso a 1, al momento non è prevista cronologia

        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        [MaxLength(5)]
        public string DataUpdate { get; set; }
        [MaxLength(5)]
        public string DataInstall { get; set; }

        [MaxLength(50)]
        public string Client { get; set; }

        public int ClientCounter { get; set; }


        [MaxLength(50)]
        public string ClientDescription { get; set; }
        [MaxLength(80)]
        public string ClientNote { get; set; }
        [MaxLength(15)]
        public string ClientLocalId{ get; set; }

        [MaxLength(10)]
        public string ClientLocalName { get; set; }

    }

    public class llDatiCliente
    {

        public string nullID { get { return "0000000000000000"; } }
        public _llDatiCliente _lldc = new _llDatiCliente();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("llDatiCliente");
        public bool _datiSalvati;
        public bool _recordPresente;

        public llDatiCliente()
        {
            _lldc = new _llDatiCliente();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public llDatiCliente(_db connessione)
        {
            valido = false;
            _lldc = new _llDatiCliente();

            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _llDatiCliente _caricaDati(int _id)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_llDatiCliente>()
                        where s.IdLocale == _id
                        select s).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private _llDatiCliente _caricaDati(string _IdApparato, Int32 IdCliente)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_llDatiCliente>()
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
                _lldc = _caricaDati(idLocale);
                if (_lldc == null)
                {
                    _lldc = new _llDatiCliente();
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
                _lldc = _caricaDati(IdApparato, IdCliente);
                if (_lldc == null)
                {
                    _lldc = new _llDatiCliente();
                    
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// Salva i dati correnti nel db locale.
        /// </summary>
        /// <returns><c>true</c> se il salvataggio riesce <c>false</c> altrimenti.</returns>
        public bool salvaDati()
        {
            try
            {
                if (_lldc.IdApparato != nullID & _lldc.IdApparato != null & _lldc.IdCliente != 0)
                {

                    _llDatiCliente _TestDati = _caricaDati(_lldc.IdApparato, _lldc.IdCliente);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _lldc.CreationDate = DateTime.Now;
                        _lldc.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_lldc);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _lldc.IdLocale = _TestDati.IdLocale;
                        _lldc.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_lldc);
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
                SQLiteCommand CancellaRecord = _database.CreateCommand("delete from _llDatiCliente where IdApparato = ? ", _lldc.IdApparato);
                int esito = CancellaRecord.ExecuteNonQuery();
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        public bool cancellaDati(string IdApparato)
        {
            try
            {
                _lldc.IdApparato = IdApparato;
                return cancellaDati();
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }


        public bool VuotaRecord(bool SalvaSeriale = false)
        {
            try
            {

                _lldc.Client = " ";
                _lldc.ClientNote = " ";
                _lldc.ClientDescription = "";
                _lldc.ClientLocalId = "";
                _lldc.ClientCounter += 1;

                if (!SalvaSeriale)
                {
                   // _lldc.ClientLocalId = "";
                }


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
            get { return _lldc.IdLocale; }
            set
            {
                //if (value !=null )
                {
                    _lldc.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }

        public string IdApparato
        {
            get { return _lldc.IdApparato; }
            set
            {
                if (value != null)
                {
                    _lldc.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }

        public int IdCliente
        {
            get { return _lldc.IdCliente; }
            set
            {

                _lldc.IdCliente = value;
                _datiSalvati = false;

            }
        }

        public DateTime CreationDate
        {
            get { return _lldc.CreationDate; }
        }

        public DateTime RevisionDate
        {
            get { return _lldc.RevisionDate; }
        }

        public string LastUser
        {
            get { return _lldc.LastUser; }
        }

        public string DataUpdate
        {
            get { return _lldc.DataUpdate; }
            set
            {
                _lldc.DataUpdate = value;
                _datiSalvati = false;
            }
        }

        public string Client
        {
            get
            {
                if (_lldc != null)
                {
                    return _lldc.Client;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                _lldc.Client = FunzioniMR.StringaMax(value, 50);
                _datiSalvati = false;
            }
        }

        public string LocalId
        {
            get { return _lldc.ClientLocalId; }
            set
            {
                _lldc.ClientLocalId = FunzioniMR.StringaMax(value, 15);
                _datiSalvati = false;
            }
        }


        public string LocalName
        {
            get { return _lldc.ClientLocalName; }
            set
            {
                _lldc.ClientLocalName = FunzioniMR.StringaMax(value, 10);
                _datiSalvati = false;
            }
        }

        

        public string Description
        {
            get { return _lldc.ClientDescription; }
            set
            {
                _lldc.ClientDescription = FunzioniMR.StringaMax(value, 127);
                _datiSalvati = false;
            }
        }

        public string Note
        {
            get { return _lldc.ClientNote; }
            set
            {
                _lldc.ClientNote = FunzioniMR.StringaMax(value, 127);
                _datiSalvati = false;
            }
        }

        public int ClientCounter
        {
            get { return _lldc.ClientCounter; }
            set
            {
                _lldc.ClientCounter = value;
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
                byte[] _datamap = new byte[240];
                int _arrayInit = 0;

                // Variabili temporanee per il passaggio dati
                byte _byte1 = 0;
                byte _byte2 = 0;
                //byte _byte3 = 0;
                //byte _byte4 = 0;
                byte[] _tempArr;

                // Preparo l'array vuoto
                for (int _i = 0; _i < 240; _i++)
                {
                    _datamap[_i] = 0x00;
                    // 
                }

                //Id pacchetto
                _datamap[_arrayInit++] = 0x01;

                // Zona Dati
                _tempArr = FunzioniComuni.StringToArray(_lldc.Client, 110);
                for (int _i = 0; _i < 110; _i++)
                    _datamap[_arrayInit++] = _tempArr[_i];

                _tempArr = FunzioniComuni.StringToArray(_lldc.ClientNote, 110);
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


                return _datamap;
            }

        }


    }

}
