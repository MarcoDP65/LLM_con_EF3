using System;
using System.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using System.Globalization;


using log4net;
using log4net.Config;

using ChargerLogic;
using Utility;


namespace MoriData

{

    public class _NodoStruttura
    {
  
        [PrimaryKey, AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(16)]
        [Indexed(Name = "GuidKey", Order = 1, Unique = true)]
        public string Guid { get; set; }

        public byte[] GuidId { get; set; }

        public byte Tipo { get; set; }

        public Int32 Level { get; set; }
        public string ParentGuid { get; set; }
        public Int32 ParentIdLocale { get; set; }
        
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        [MaxLength(256)]
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public string Icona { get; set; }
        public string IdApparato { get; set; }
        public int Ordine { get; set; }

    }




    public class NodoStruttura
    {
        public enum TipoNodo: byte {Radice = 0x00, Ramo = 0x01,RadiceCloud = 0x10,FogliaSB = 0x80, FogliaLL = 0x81, FogliaDS = 0x82, FogliaDisp = 0x83 };
        public const string GuidBASE  = "00000000-0000-0000-0000-000000000000";
        public const string GuidROOT  = "514c4137-a4b3-4561-b6da-5496dc05ab2a";
        public const string GuidUNDEF = "aac127d9-82d2-42df-95c8-f9d42f4e9a2f";



        public _NodoStruttura _sbNS = new _NodoStruttura();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;

        private Guid _guid;

        public NodoStruttura()
        {
            _sbNS = new _NodoStruttura();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }


        public NodoStruttura(_NodoStruttura _dati)
        {
            _sbNS = _dati;
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        public NodoStruttura(_db connessione)
        {
            valido = false;
            _sbNS = new _NodoStruttura();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false; 
        }



        private _NodoStruttura _caricaDati(int _id)
        {
            return (from s in _database.Table<_NodoStruttura>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        private _NodoStruttura _caricaDati(string _Guid)
        {
            return (from s in _database.Table<_NodoStruttura>()
                    where s.Guid == _Guid
                    select s).FirstOrDefault();
        }



        public bool caricaDati(int idLocale)
        {
            try
            {
                _sbNS = _caricaDati(idLocale);
                if (_sbNS == null)
                {
                    _sbNS = new _NodoStruttura();
                    _guid = new Guid();
                    return false;
                }
                else
                {
                    _guid = new Guid(_sbNS.Guid);
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool caricaDati(string Guid)
        {
            try
            {
                _sbNS = _caricaDati(Guid);
                if (_sbNS == null)
                {
                    _sbNS = new _NodoStruttura();
                    return false;
                }
                else
                {
                    _guid = new Guid(_sbNS.Guid);
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
                if (_sbNS != null)
                {
                    if( _sbNS.Guid == null || _sbNS.Guid == NodoStruttura.GuidBASE)
                    {
                        return false;
                    }

                    _NodoStruttura _TestDati = _caricaDati(_guid.ToString());
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _sbNS.CreationDate = DateTime.Now;
                        _sbNS.RevisionDate = DateTime.Now;
                        _sbNS.Guid = _guid.ToString();
                        // Se non ho impostato il parent, lo assegno ai non definiti
                        if(_sbNS.ParentGuid == "")
                            _sbNS.ParentGuid = NodoStruttura.GuidUNDEF;
                        int _result = _database.Insert(_sbNS);
                        _datiSalvati = true;
                    }
                    else
                    {
                        //_sbNS.IdLocale = _TestDati.IdLocale;
                        _sbNS.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_sbNS);
                        _datiSalvati = true;
                    }

                    //_database.InsertOrReplace(_sb);
                    return true;
                }
                else
                {

                    // Se il GUID è nullo lo creo e creo il nuovo record
                    return false;
                }
            }
            catch (Exception Ex)
            {
                Log.Error("NodoStruttura.salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }


        public ArrayList ChildrensList()
        {
            try
            {
                ArrayList _figli = new ArrayList();

                return _figli;
            }
            catch
            {
                return new ArrayList();
            }

        }




        public string NuovoGuid()
        {
            try
            {
                _guid =  System.Guid.NewGuid();

                _sbNS.Guid = _guid.ToString();

                return _sbNS.Guid;
            }
            catch (Exception Ex)
            {
                Log.Error("NodoStruttura.NuovoGuid: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return _sbNS.Guid;
            }
        }


        public bool AntenatoDi(NodoStruttura Discendente)
        {
            try
            {
                // Se il nodo non ha figli è cancellabile
                _NodoStruttura _firstChidren = (from s in _database.Table<_NodoStruttura>()
                                                where s.ParentGuid == _sbNS.Guid
                                                select s).FirstOrDefault();


                return (_firstChidren == null);
            }

            catch (Exception Ex)
            {
                Log.Error("NodoStruttura.Cancellabile: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }
   
        public NodoStruttura getParent
        {
            get
            {
                NodoStruttura _tempNodo = null;

                if (_sbNS.ParentGuid == NodoStruttura.GuidROOT || _sbNS.ParentGuid == null)
                    return null;

                return _tempNodo;
            }
        }

        public bool DiscendenteDi(NodoStruttura Antenato)
        {
            try
            {
                // Scorro all'indietro la catena dei parent e verifico che la destinazione non sia nodo discendente
                while(true)
                {
                    if(_sbNS.ParentGuid == NodoStruttura.GuidROOT || _sbNS.ParentGuid == null )
                    {
                        // Sono alla radice, non è discendente
                        return false;
                    }
                    if (_sbNS.ParentGuid == Antenato.Guid)
                    {
                        // Il parentGuid coincide: è il figlio
                        return true;
                    }

                    // non ho trovato, risalgo di un livello
                    NodoStruttura _menoUno = new NodoStruttura(_database);
                    _menoUno.caricaDati(_sbNS.ParentGuid);
                    return _menoUno.DiscendenteDi(Antenato);
                }

 
            }

            catch (Exception Ex)
            {
                Log.Error("NodoStruttura.Cancellabile: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        public bool Cancellabile()
        {
            try
            {
                // Se il nodo non ha figli è cancellabile
                _NodoStruttura _firstChidren = (from s in _database.Table<_NodoStruttura>()
                                                where s.ParentGuid == _sbNS.Guid
                                                select s).FirstOrDefault();
              

                return (_firstChidren==null);
            }

            catch (Exception Ex)
            {
                Log.Error("NodoStruttura.Cancellabile: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }


        #region Parametri
        public int IdLocale
        {
            get { return _sbNS.IdLocale; }
            set
            {
                if (value != null)
                {
                    _sbNS.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }

        public string Guid
        {
            get { return _sbNS.Guid; }
        }




        public string Nome
        {
            get { return _sbNS.Nome; }
            set
            {
                if (value != null)
                {
                    _sbNS.Nome = value;
                    _datiSalvati = false;
                }
            }
        }
        public string Descrizione
        {
            get { return _sbNS.Descrizione; }
            set
            {
                if (value != null)
                {
                    _sbNS.Descrizione = value;
                    _datiSalvati = false;
                }
            }
        }
        public string Icona
        {
            get { return _sbNS.Icona; }
            set
            {
                if (value != null)
                {
                    _sbNS.Icona = value;
                    _datiSalvati = false;
                }
            }
        }

        public string ParentName
        {
            get
            {
                _NodoStruttura _parent = (from s in _database.Table<_NodoStruttura>()
                                          where s.Guid == _sbNS.ParentGuid
                                          select s).FirstOrDefault();
                return _parent.Nome;
            }


        }



        public string ParentGuid
        {
            get { return _sbNS.ParentGuid; }
            set
            {
                if (value != null)
                {
                    _sbNS.ParentGuid = value;
                    _datiSalvati = false;
                }
            }
        }


        public NodoStruttura.TipoNodo Tipo
        {
            get { return (NodoStruttura.TipoNodo)_sbNS.Tipo; }
            set
            {

                _sbNS.Tipo = (byte)value;
                _datiSalvati = false;

            }
        }

        public string IdApparato
        {
            get { return _sbNS.IdApparato; }
            set
            {
                if (value != null)
                {
                    _sbNS.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }


        public bool IsLeaf
        {
            get
            {

                if ((_sbNS.Tipo & 0x80) == 0x80)
                    return true;
                else
                    return false;
            }
        }


        #endregion

    }


}
