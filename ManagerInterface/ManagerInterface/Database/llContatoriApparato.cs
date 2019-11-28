
//    class llContatoriApparato

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
    public class _llContatoriApparato
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        [Indexed(Name = "IdxLLContatori", Order = 1, Unique = true)]
        public string IdApparato { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }

        public byte[] DataPrimaCarica { get; set; }
        public UInt32 CntCicliTotali { get; set; }
        public UInt32 CntCicliStop { get; set; }
        public UInt32 CntCicliStaccoBatt { get; set; }
        public UInt32 CntCicliLess3H { get; set; }
        public UInt32 CntCicli3Hto6H { get; set; }
        public UInt32 CntCicli6Hto9H { get; set; }
        public UInt32 CntCicliOver9H { get; set; }
        public UInt32 CntCicliOpportunity { get; set; }
        public ushort CntProgrammazioni { get; set; }

        public UInt32 CntCicliBrevi { get; set; }
        public UInt32 PntNextBreve { get; set; }

        public UInt32 CntCariche { get; set; }
        public UInt32 PntNextCarica { get; set; }

        public ushort CntMemReset { get; set; }
        public byte[] DataUltimaCancellazione { get; set; }

        //StringaDataTS(byte[] DataShort)

        public ushort CrcPacchetto { get; set; }

        public byte Stato { get; set; }

        public DateTime IstanteLettura { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IstanteLettura.ToString();
        }
    }

    public class llContatoriApparato
    {
        public string nullID { get { return "000000"; } }
        public _llContatoriApparato llContApp = new _llContatoriApparato();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("llContatoriApparato");
        public bool _datiSalvati;
        public bool _recordPresente;
        //private string _tempId;

        public llContatoriApparato()
        {
            llContApp = new _llContatoriApparato();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }


        public llContatoriApparato(_db connessione)
        {
            valido = false;
            llContApp = new _llContatoriApparato();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }



        private _llContatoriApparato _caricaDati(int _id)
        {
            return (from s in _database.Table<_llContatoriApparato>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        private _llContatoriApparato _caricaDati(string _idApparato)
        {
            return (from s in _database.Table<_llContatoriApparato>()
                    where s.IdApparato == _idApparato
                    select s).FirstOrDefault();
        }


        public bool caricaDati(int idLocale)
        {
            try
            {
                llContApp = _caricaDati(idLocale);
                if (llContApp == null)
                {
                    llContApp = new _llContatoriApparato();
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool caricaDati(string idApparato)
        {
            try
            {
                llContApp = _caricaDati(idApparato);
                if (llContApp == null)
                {
                    llContApp = new _llContatoriApparato();
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
                
                if (llContApp.IdApparato != nullID & llContApp.IdApparato != null)
                {

                    _llContatoriApparato _TestDati = _caricaDati(llContApp.IdApparato);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        llContApp.CreationDate = DateTime.Now;
                        llContApp.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(llContApp);
                        _datiSalvati = true;
                    }
                    else
                    {
                        llContApp.IdLocale = _TestDati.IdLocale;
                        llContApp.RevisionDate = DateTime.Now;
                        int _result = _database.Update(llContApp);
                        _datiSalvati = true;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
                 
                return false;
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }


        public MessaggioLadeLight.MessaggioAreaContatori  GeneraMessaggio()
        {
            try
            {

                MessaggioLadeLight.MessaggioAreaContatori MsgContatoriLL = new MessaggioLadeLight.MessaggioAreaContatori();

                if (!valido)
                {
                    return null;
                }

                MsgContatoriLL.DataPrimaCarica = FunzioniComuni.ArrayCopy(DataPrimaCarica);
                MsgContatoriLL.CntCicliTotali = CntCicliTotali;
                MsgContatoriLL.CntCicliStaccoBatt = CntCicliStaccoBatt;
                MsgContatoriLL.CntCicliStop = CntCicliStop;
                MsgContatoriLL.CntCicliLess3H = CntCicliLess3H;
                MsgContatoriLL.CntCicli3Hto6H = CntCicli3Hto6H;
                MsgContatoriLL.CntCicli6Hto9H = CntCicli6Hto9H;
                MsgContatoriLL.CntCicliOver9H = CntCicliOver9H;
                MsgContatoriLL.CntProgrammazioni = CntProgrammazioni;
                MsgContatoriLL.CntCicliBrevi = CntCicliBrevi;
                MsgContatoriLL.PntNextBreve = PntNextBreve;
                MsgContatoriLL.CntCariche = CntCariche;
                MsgContatoriLL.PntNextCarica = PntNextCarica;
                MsgContatoriLL.CntMemReset = CntMemReset;
                MsgContatoriLL.DataUltimaCancellazione = FunzioniComuni.ArrayCopy(DataUltimaCancellazione);
                MsgContatoriLL.CntCicliOpportunity = CntCicliOpportunity;

                return MsgContatoriLL;
            }

            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                return null;
            }

        }



        #region Class Parameters

        public int IdLocale
        {
            get { return llContApp.IdLocale; }
            set
            {
                if (value != null)
                {
                    llContApp.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }

        public string IdApparato
        {
            get
            {
                return llContApp.IdApparato;
            }

        }

        public byte[] DataPrimaCarica
        {
            get { return llContApp.DataPrimaCarica; }
            set
            {
                if (value != null)
                {
                    llContApp.DataPrimaCarica = value;
                    _datiSalvati = false;
                }
            }
        }


        public string strDataPrimaCarica
        {
            get
            {
                return FunzioniMR.StringaDataTS(llContApp.DataPrimaCarica);
            }

        }

        public uint CntCicliTotali
        {
            get { return llContApp.CntCicliTotali; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliTotali = value;
                    _datiSalvati = false;
                }
            }
        }


        public uint CntCicliOpportunity
        {
            get { return llContApp.CntCicliOpportunity; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliOpportunity = value;
                    _datiSalvati = false;
                }
            }
        }


        public uint CntCicliStop
        {
            get { return llContApp.CntCicliStop; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliStop = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint CntCicliStaccoBatt
        {
            get { return llContApp.CntCicliStaccoBatt; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliStaccoBatt = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint CntCicliLess3H
        {
            get { return llContApp.CntCicliLess3H; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliLess3H = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint CntCicli3Hto6H
        {
            get { return llContApp.CntCicli3Hto6H; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicli3Hto6H = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint CntCicli6Hto9H
        {
            get { return llContApp.CntCicli6Hto9H; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicli6Hto9H = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint CntCicliOver9H
        {
            get { return llContApp.CntCicliOver9H; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliOver9H = value;
                    _datiSalvati = false;
                }
            }
        }

        public ushort CntProgrammazioni
        {
            get { return llContApp.CntProgrammazioni; }
            set
            {
                if (value != null)
                {
                    llContApp.CntProgrammazioni = value;
                    _datiSalvati = false;
                }
            }
        }


        public uint CntCicliBrevi
        {
            get { return llContApp.CntCicliBrevi; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCicliBrevi = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint PntNextBreve
        {
            get { return llContApp.PntNextBreve; }
            set
            {
                if (value != null)
                {
                    llContApp.PntNextBreve = value;
                    _datiSalvati = false;
                }
            }
        }
        public string strPntNextBreve
        {
            get { return "0x" + llContApp.PntNextBreve.ToString("X4"); }
        }

        public uint CntCariche
        {
            get { return llContApp.CntCariche; }
            set
            {
                if (value != null)
                {
                    llContApp.CntCariche = value;
                    _datiSalvati = false;
                }
            }
        }

        public uint PntNextCarica
        {
            get { return llContApp.PntNextCarica; }
            set
            {
                if (value != null)
                {
                    llContApp.PntNextCarica = value;
                    _datiSalvati = false;
                }
            }
        }
        public string strPntNextCarica
        {
            get { return "0x" + llContApp.PntNextCarica.ToString("X4"); }
        }

        public ushort CntMemReset
        {
            get { return llContApp.CntMemReset; }
            set
            {
                if (value != null)
                {
                    llContApp.CntMemReset = value;
                    _datiSalvati = false;
                }
            }
        }

        public byte[] DataUltimaCancellazione
        {
            get { return llContApp.DataUltimaCancellazione; }
            set
            {
                if (value != null)
                {
                    llContApp.DataUltimaCancellazione = value;
                    _datiSalvati = false;
                }
            }
        }


        public string strDataUltimaCancellazione
        {
            get
            {
                return FunzioniMR.StringaDataTS(llContApp.DataUltimaCancellazione);
            }

        }



        #endregion


    }

}





