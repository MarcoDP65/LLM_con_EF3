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

using Utility;

namespace MoriData
{
    public class _sbProgrammaRicarica
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        public string IdApparato { get; set; }
        public ushort IdProgramma { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }

        public string DataInstallazione { get; set; }
        public ushort BatteryVdef { get; set; }
        public ushort BatteryAhdef { get; set; }
        public byte BatteryType { get; set; }
        public byte BatteryCells { get; set; }
        public byte BatteryCell_1 { get; set; }
        public byte BatteryCell_2 { get; set; }
        public byte BatteryCell_3 { get; set; }

    }

    public class sbProgrammaRicarica
    {

        public string nullID { get { return "0000000000000000"; } }
        public _sbProgrammaRicarica _sbpr = new _sbProgrammaRicarica();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        private string _tempId;
        public StruttureBase.ArrayCelle CelleSensori;



        public sbProgrammaRicarica()
        {
            _sbpr = new _sbProgrammaRicarica();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public sbProgrammaRicarica(_db connessione)
        {
            valido = false;
            _sbpr = new _sbProgrammaRicarica(); 
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _sbProgrammaRicarica _caricaDati(int _id)
        {
            return (from s in _database.Table<_sbProgrammaRicarica>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        private _sbProgrammaRicarica _caricaDati(string _IdApparato, Int32 _IdProgramma)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_sbProgrammaRicarica>()
                        where s.IdApparato == _IdApparato & s.IdProgramma == _IdProgramma 
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
                _sbpr = _caricaDati(idLocale);
                if (_sbpr == null)
                {
                    _sbpr = new _sbProgrammaRicarica();
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool caricaDati(string IdApparato, ushort IdProgramma)
        {
            try
            {
                if (IdProgramma != 0xFFFF)
                {
                    _sbpr = _caricaDati(IdApparato, IdProgramma);
                }

                if (_sbpr == null)
                {
                    _sbpr = new _sbProgrammaRicarica();
                    _sbpr.IdApparato = IdApparato;
                    _sbpr.IdProgramma = IdProgramma;
                    CelleSensori = null;
                    return false;
                }
                else
                {
                    CelleSensori = new StruttureBase.ArrayCelle();
                    CelleSensori.C1 = _sbpr.BatteryCell_1;
                    CelleSensori.C2 = _sbpr.BatteryCell_2;
                    CelleSensori.C3 = _sbpr.BatteryCell_3;
                    CelleSensori.Cbatt = _sbpr.BatteryCells;
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
                if (_sbpr.IdApparato != nullID & _sbpr.IdApparato != null & _sbpr.IdProgramma != null)
                {

                    _sbProgrammaRicarica _TestDati = _caricaDati(_sbpr.IdApparato, _sbpr.IdProgramma);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _sbpr.CreationDate = DateTime.Now;
                        _sbpr.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_sbpr);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _sbpr.IdLocale = _TestDati.IdLocale;
                        _sbpr.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_sbpr);
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

        #region Class Parameters

        public int IdLocale
        {
            get { return _sbpr.IdLocale; }
            set
            {
                if (value !=null )
                {
                    _sbpr.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }
        public string IdApparato
        {
            get { return _sbpr.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbpr.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }
        public ushort IdProgramma
        {
            get { return _sbpr.IdProgramma; }
            set
            {
                if (value != null)
                {
                    _sbpr.IdProgramma = value;
                    _datiSalvati = false;
                }
            }
        }
        public DateTime CreationDate
        {
            get { return _sbpr.CreationDate; }
        }
        public DateTime RevisionDate
        {
            get { return _sbpr.RevisionDate; }
        }
        public string LastUser
        {
            get { return _sbpr.LastUser; }
        }

        public string DataInstallazione
        {
            get { return _sbpr.DataInstallazione; }
            set
            {
                _sbpr.DataInstallazione = value;
                _datiSalvati = false;
            }
        }

        public ushort BatteryVdef
        {
            get { return _sbpr.BatteryVdef; }
            set
            {
                _sbpr.BatteryVdef = value;
                _datiSalvati = false;
            }
        }
        public ushort BatteryAhdef
        {
            get { return _sbpr.BatteryAhdef; }
            set
            {
                _sbpr.BatteryAhdef = value;
                _datiSalvati = false;
            }
        }
        public byte BatteryType
        {
            get { return _sbpr.BatteryType; }
            set
            {
                _sbpr.BatteryType = value;
                _datiSalvati = false;
            }
        }

        public byte BatteryCells
        {
            get { return _sbpr.BatteryCells; }
            set
            {
                _sbpr.BatteryCells = value;
                _datiSalvati = false;
            }
        }
        public byte BatteryCell1
        {
            get { return _sbpr.BatteryCell_1; }
            set
            {
                _sbpr.BatteryCell_1 = value;
                _datiSalvati = false;
            }
        }
        public byte BatteryCell2
        {
            get { return _sbpr.BatteryCell_2; }
            set
            {
                _sbpr.BatteryCell_2 = value;
                _datiSalvati = false;
            }
        }
        public byte BatteryCell3
        {
            get { return _sbpr.BatteryCell_3; }
            set
            {
                _sbpr.BatteryCell_3 = value;
                _datiSalvati = false;
            }
        }



        /// <summary>
        /// Genera il Bytearray per l'esportazione
        /// </summary>
        public byte[] DataArray
        {
            get
            {
                byte[] _datamap = new byte[128];
                int _arrayInit = 0;

                // Variabili temporanee per il passaggio dati
                byte _byte1 = 0;
                byte _byte2 = 0;
                byte _byte3 = 0;
                byte _byte4 = 0;

                // Preparo l'array vuoto
                for (int _i = 0; _i < 128; _i++)
                {
                    _datamap[_i] = 0x00;
                    // 
                }

                //Id Programma
                FunzioniComuni.SplitUshort(_sbpr.IdProgramma, ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte1;


                //Data Installazione
                _datamap[_arrayInit++] = 0;
                _datamap[_arrayInit++] = 0;
                _datamap[_arrayInit++] = 0;


                //Tensione Nominale
                FunzioniComuni.SplitUshort(_sbpr.BatteryVdef, ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte1;


                //Capacità Nominale
                FunzioniComuni.SplitUshort(_sbpr.BatteryAhdef, ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte1;


                //Tipo Batteria
                _datamap[_arrayInit++] = _sbpr.BatteryType;

                //Mappa Celle
                _datamap[_arrayInit++] = _sbpr.BatteryCells;
                _datamap[_arrayInit++] = _sbpr.BatteryCell_3;
                _datamap[_arrayInit++] = _sbpr.BatteryCell_2;
                _datamap[_arrayInit++] = _sbpr.BatteryCell_1;

                // Ulteriori Parametri ciclo
                //_datamap[_arrayInit++] = 0xFF;  //Not Used


                return _datamap;
            }

        }



        #endregion Class Parameter


    }

}
