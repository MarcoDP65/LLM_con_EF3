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
using ChargerLogic;

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
        public byte AbilitaPresElett { get; set; }
        public byte TempMin { get; set; }
        public byte TempMax { get; set; }
        public byte VersoCorrente { get; set; }
        public byte NumeroSpire { get; set; }

        // Parametri PRO
        // Testata
        public byte ModoPianificazione { get; set; }
        public ushort MaxCorrente { get; set; }
        public byte FcBase { get; set; }
        public byte FcProfondo { get; set; }
        public byte Rabboccatore { get; set; }
        public byte Biberonaggio { get; set; }
        public byte FattorBiberonaggio { get; set; }
        public byte TempAttenzione { get; set; }
        public byte TempAllarme { get; set; }
        public byte TempRipresa { get; set; }
        public ushort MaxSbilanciamento { get; set; }
        public ushort DurataSbilanciamento { get; set; }
        public ushort TensioneGas { get; set; }
        public ushort DerivaSuperiore { get; set; }
        public ushort DerivaInferiore { get; set; }
        public byte[] DatiPianificazione { get; set; }
        public ushort MaxCorrenteCHG { get; set; }
        public ushort MinCorrenteCHG { get; set; }
        public byte ImpulsiRabboccatore { get; set; }
        public ushort MinCorrenteW { get; set; }
        public ushort MaxCorrenteW { get; set; }
        public ushort MaxCorrenteImp { get; set; }
        public ushort TensioneRaccordo{ get; set; }
        public ushort TensioneFinale { get; set; }


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


        public MessaggioSpyBatt.ProgrammaRicarica.NuoviLivelli ResetContatori { get; set; } = MessaggioSpyBatt.ProgrammaRicarica.NuoviLivelli.MantieniLivelli;



        public sbProgrammaRicarica()
        {
            _sbpr = new _sbProgrammaRicarica();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }


        public sbProgrammaRicarica(_sbProgrammaRicarica RecordBase)
        {
            _sbpr = RecordBase;
            valido = ( RecordBase != null);
            _datiSalvati = true;
            _recordPresente = (RecordBase != null);
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


        public byte AbilitaPresElett
        {
            get { return _sbpr.AbilitaPresElett; }
            set
            {
                _sbpr.AbilitaPresElett = value;
                _datiSalvati = false;
            }
        }


        public byte TempMin
        {
            get { return _sbpr.TempMin; }
            set
            {
                _sbpr.TempMin = value;
                _datiSalvati = false;
            }
        }
        public byte TempMax
        {
            get { return _sbpr.TempMax; }
            set
            {
                _sbpr.TempMax = value;
                _datiSalvati = false;
            }
        }
        public byte VersoCorrente
        {
            get { return _sbpr.VersoCorrente; }
            set
            {
                _sbpr.VersoCorrente = value;
                _datiSalvati = false;
            }
        }

        public byte NumeroSpire
        {
            get { return _sbpr.NumeroSpire; }
            set
            {
                _sbpr.NumeroSpire = value;
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
   
        
        // Parametri PRO



        public byte ModoPianificazione
        {
            get { return _sbpr.ModoPianificazione; }
            set
            {
                _sbpr.ModoPianificazione = value;
                _datiSalvati = false;
            }
        }


        public ushort CorrenteMinimaCHG
        {
            get { return _sbpr.MinCorrenteCHG; }
            set
            {
                _sbpr.MinCorrenteCHG = value;
                _datiSalvati = false;
            }
        }

        public ushort CorrenteMassimaCHG
        {
            get { return _sbpr.MaxCorrenteCHG; }
            set
            {
                _sbpr.MaxCorrenteCHG = value;
                _datiSalvati = false;
            }
        }

        public ushort MinCorrenteW
        {
            get { return _sbpr.MinCorrenteW; }
            set
            {
                _sbpr.MinCorrenteW = value;
                _datiSalvati = false;
            }
        }

        public ushort MaxCorrenteW
        {
            get { return _sbpr.MaxCorrenteW; }
            set
            {
                _sbpr.MaxCorrenteW = value;
                _datiSalvati = false;
            }
        }

        public ushort MaxCorrenteImp
        {
            get { return _sbpr.MaxCorrenteImp; }
            set
            {
                _sbpr.MaxCorrenteImp = value;
                _datiSalvati = false;
            }
        }

        public ushort TensioneRaccordo
        {
            get { return _sbpr.TensioneRaccordo; }
            set
            {
                _sbpr.TensioneRaccordo = value;
                _datiSalvati = false;
            }
        }

        public ushort TensioneFinale
        {
            get { return _sbpr.TensioneFinale; }
            set
            {
                _sbpr.TensioneFinale = value;
                _datiSalvati = false;
            }
        }


        public byte FcBase
        {
            get { return _sbpr.FcBase; }
            set
            {
                _sbpr.FcBase = value;
                _datiSalvati = false;
            }
        }

        public byte FcProfondo
        {
            get { return _sbpr.FcProfondo; }
            set
            {
                _sbpr.FcProfondo = value;
                _datiSalvati = false;
            }
        }

        public byte Rabboccatore
        {
            get { return _sbpr.Rabboccatore; }
            set
            {
                _sbpr.Rabboccatore = value;
                _datiSalvati = false;
            }
        }

        public byte ImpulsiRabboccatore
        {
            get { return _sbpr.ImpulsiRabboccatore; }
            set
            {
                _sbpr.ImpulsiRabboccatore = value;
                _datiSalvati = false;
            }
        }

        public byte Biberonaggio
        {
            get { return _sbpr.Biberonaggio; }
            set
            {
                _sbpr.Biberonaggio = value;
                _datiSalvati = false;
            }
        }

        public byte FattorBiberonaggio
        {
            get { return _sbpr.FattorBiberonaggio; }
            set
            {
                _sbpr.FattorBiberonaggio = value;
                _datiSalvati = false;
            }
        }


        public byte TempAttenzione
        {
            get { return _sbpr.TempAttenzione; }
            set
            {
                _sbpr.TempAttenzione = value;
                _datiSalvati = false;
            }
        }


        public byte TempAllarme
        {
            get { return _sbpr.TempAllarme; }
            set
            {
                _sbpr.TempAllarme = value;
                _datiSalvati = false;
            }
        }


        public byte TempRipresa
        {
            get { return _sbpr.TempRipresa; }
            set
            {
                _sbpr.TempRipresa = value;
                _datiSalvati = false;
            }
        }


        public ushort MaxSbilanciamento
        {
            get { return _sbpr.MaxSbilanciamento; }
            set
            {
                _sbpr.MaxSbilanciamento = value;
                _datiSalvati = false;
            }
        }


        public ushort DurataSbilanciamento
        {
            get { return _sbpr.DurataSbilanciamento; }
            set
            {
                _sbpr.DurataSbilanciamento = value;
                _datiSalvati = false;
            }
        }

        public ushort TensioneGas
        {
            get { return _sbpr.TensioneGas; }
            set
            {
                _sbpr.TensioneGas = value;
                _datiSalvati = false;
            }
        }
        public ushort DerivaSuperiore
        {
            get { return _sbpr.DerivaSuperiore; }
            set
            {
                _sbpr.DerivaSuperiore = value;
                _datiSalvati = false;
            }
        }
        public ushort DerivaInferiore
        {
            get { return _sbpr.DerivaInferiore; }
            set
            {
                _sbpr.DerivaInferiore = value;
                _datiSalvati = false;
            }
        }

        public byte[] DatiPianificazione
        {
            get { return _sbpr.DatiPianificazione; }
            set
            {
                _sbpr.DatiPianificazione = value;
                _datiSalvati = false;
            }
        }

        #endregion Class Parameter


    }

}
