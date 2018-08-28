
//    class llProgrammaCarica


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
    public class _llProgrammaCarica
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
        public string ProgramName { get; set; }
        public byte BatteryType { get; set; }
        public ushort BatteryVdef { get; set; }
        public ushort BatteryAhdef { get; set; }
        public ushort VSoglia { get; set; }
        public ushort VMax { get; set; }
        public ushort CorrenteMax { get; set; }
        public byte EqualTempoAttesa { get; set; }
        public byte EqualNumImpulsi { get; set; }

        public byte IdProfilo { get; set; }
        public ushort DurataMaxCarica { get; set; }
        public ushort PercTempoFase2 { get; set; }

        public byte AbilitaComunicazioneSpybatt { get; set; }

        public byte TempoErogazioneBMS { get; set; }
        public byte TempoAttesaBMS { get; set; }

        public byte ProgrammaInUso { get; set; }

        public byte TipoRecord { get; set; }
        public byte OpzioniAttive { get; set; } 




    }

    public class llProgrammaCarica
    {

        public string nullID { get { return "LL000000"; } }
        public _llProgrammaCarica _llprc = new _llProgrammaCarica();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("llProgrammaCarica");
        public bool _datiSalvati;
        public bool _recordPresente;

        public List<ParametroLL> ListaParametri;

        public MessaggioLadeLight.MessaggioProgrammazione Datischeda;



        public llProgrammaCarica()
        {
            _llprc = new _llProgrammaCarica();
            ListaParametri = new List<ParametroLL>();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }


        public llProgrammaCarica(_llProgrammaCarica RecordBase)
        {
            _llprc = RecordBase;
            valido = (RecordBase != null);
            _datiSalvati = true;
            _recordPresente = (RecordBase != null);
        }



        public llProgrammaCarica(_db connessione)
        {
            valido = false;
            _llprc = new _llProgrammaCarica();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
        }

        private _llProgrammaCarica _caricaDati(int _id)
        {
            return (from s in _database.Table<_llProgrammaCarica>()
                    where s.IdLocale == _id
                    select s).FirstOrDefault();
        }

        private _llProgrammaCarica _caricaDati(string _IdApparato, Int32 _IdProgramma)
        {
            if (_database != null)
            {
                _llProgrammaCarica _esitoQuery = (from s in _database.Table<_llProgrammaCarica>()
                                                    where s.IdApparato == _IdApparato & s.IdProgramma == _IdProgramma
                                                    select s).FirstOrDefault();
                return _esitoQuery;
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
                _llprc = _caricaDati(idLocale);
                if (_llprc == null)
                {
                    _llprc = new _llProgrammaCarica();
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
                    _llprc = _caricaDati(IdApparato, IdProgramma);
                }

                if (_llprc == null)
                {
                    _llprc = new _llProgrammaCarica();
                    _llprc.IdApparato = IdApparato;
                    _llprc.IdProgramma = IdProgramma;
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
                if (_llprc.IdApparato != nullID & _llprc.IdApparato != null & _llprc.IdProgramma == 0)
                {

                    _llProgrammaCarica _TestDati = _caricaDati(_llprc.IdApparato, _llprc.IdProgramma);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _llprc.CreationDate = DateTime.Now;
                        _llprc.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_llprc);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _llprc.IdLocale = _TestDati.IdLocale;
                        _llprc.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_llprc);
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

        public bool ClonaRecord(string NuovoIdApparato)
        {
            try
            {
                _llProgrammaCarica _newDC = FunzioniComuni.CloneJson<_llProgrammaCarica>(_llprc);
                _newDC.IdApparato = NuovoIdApparato;

                int _result = _database.Insert(_newDC);

                return (_result == 1);
            }
            catch (Exception Ex)
            {
                Log.Error("ClonaRecord: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        public bool GeneraListaParametri()
        {
            try
            {
                ListaParametri = new  List<ParametroLL>();
                ParametroLL _par;

                // Tipo Batteria
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TipoBatteria;
                _par.ValoreParametro = _llprc.BatteryType;
                ListaParametri.Add(_par);

                // Durata Carica
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TempoMassimoCarica;
                _par.ValoreParametro = _llprc.DurataMaxCarica;
                ListaParametri.Add(_par);

                // Durata Fase 2
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TempoT2Min;
                _par.ValoreParametro = _llprc.PercTempoFase2;
                ListaParametri.Add(_par);

                // Tensione Nominale
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneNominale;
                _par.ValoreParametro = _llprc.BatteryVdef;
                ListaParametri.Add(_par);

                // Tensione di soglia
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneSogliaCella;
                _par.ValoreParametro = _llprc.VSoglia;
                ListaParametri.Add(_par);

                // Capacità Nominale
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.CapacitaNominale;
                _par.ValoreParametro = _llprc.BatteryAhdef;
                ListaParametri.Add(_par);

                // Equal fine carica
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.EqualFineCarica;
                _par.ValoreParametro = _llprc.EqualTempoAttesa;
                _par.ValoreParametro = (ushort)(_par.ValoreParametro << 8);
                _par.ValoreParametro += _llprc.EqualNumImpulsi;
                ListaParametri.Add(_par);

                // Riarmo BMS
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.RiarmoBMS;
                _par.ValoreParametro = _llprc.TempoErogazioneBMS;
                _par.ValoreParametro = (ushort)(_par.ValoreParametro << 8);
                _par.ValoreParametro += _llprc.TempoAttesaBMS;
                ListaParametri.Add(_par);

                // Abilitazione SPY-BATT
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.DivisoreK;
                _par.ValoreParametro = _llprc.AbilitaComunicazioneSpybatt;
                ListaParametri.Add(_par);

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("GeneraListaParametri: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        public bool AnalizzaListaParametri()
        {
            try
            {
                byte loval = 0;
                byte hival = 0;

                foreach (ParametroLL _par in ListaParametri)
                {
                    switch (_par.idParametro)
                    {
                        case (byte)SerialMessage.ParametroLadeLight.CapacitaNominale:
                            _llprc.BatteryAhdef = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TempoMassimoCarica:
                            _llprc.DurataMaxCarica = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TensioneSogliaCella:
                            _llprc.VSoglia = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TensioneNominale:
                            _llprc.BatteryVdef = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TempoT2Min:
                            _llprc.PercTempoFase2 = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TipoBatteria:
                            _llprc.BatteryType = (byte)_par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.DivisoreK:
                            _llprc.AbilitaComunicazioneSpybatt = (byte)_par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.EqualFineCarica:
                            FunzioniComuni.SplitUshort(_par.ValoreParametro, ref loval, ref hival);
                            _llprc.EqualTempoAttesa = hival;
                            _llprc.EqualNumImpulsi = loval;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.RiarmoBMS:
                            FunzioniComuni.SplitUshort(_par.ValoreParametro, ref loval, ref hival);
                            _llprc.EqualTempoAttesa = hival;
                            _llprc.EqualNumImpulsi = loval;
                            break;
                        default:
                            break;
                    }
                }
                return true;
            }

            catch (Exception Ex)
            {
                Log.Error("GeneraListaParametri: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }


        #region Class Parameters

        public int IdLocale
        {
            get { return _llprc.IdLocale; }
            set
            {

                _llprc.IdLocale = value;
                _datiSalvati = false;

            }
        }
        public string IdApparato
        {
            get { return _llprc.IdApparato; }
            set
            {
                if (value != null)
                {
                    _llprc.IdApparato = value;
                    _datiSalvati = false;
                }
            }
        }
        public ushort IdProgramma
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.IdProgramma;
                }
                else
                    return 0;
            }
            set
            {
                if (value != null)
                {
                    _llprc.IdProgramma = value;
                    _datiSalvati = false;
                }
            }
        }

        public string strIdProgramma
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.IdProgramma.ToString();
                }
                else
                    return "";
            }

        }

        public DateTime CreationDate
        {
            get { return _llprc.CreationDate; }
        }
        public DateTime RevisionDate
        {
            get { return _llprc.RevisionDate; }
        }
        public string LastUser
        {
            get { return _llprc.LastUser; }
        }

        public string DataInstallazione
        {
            get { return _llprc.DataInstallazione; }
            set
            {
                _llprc.DataInstallazione = value;
                _datiSalvati = false;
            }
        }

        public string ProgramName
        {
            get { return _llprc.ProgramName; }
            set
            {
                if (value != null)
                {
                    _llprc.ProgramName = value;
                    _datiSalvati = false;
                }
            }
        }

        /// <summary>
        /// Tensione Nominale Batteria.
        /// </summary>
        /// <value>
        /// Tensione in V/100.
        /// </value>
        public ushort BatteryVdef
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.BatteryVdef;
                }
                else
                {
                    return 0;
                }
                return 0;

            }
            set
            {
                _llprc.BatteryVdef = value;
                _datiSalvati = false;
            }
        }

        /// <summary>
        /// Capacità nominale batteria.
        /// </summary>
        /// <value>
        /// Capacità in Ah/10.
        /// </value>
        public ushort BatteryAhdef
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.BatteryAhdef;
                }
                else
                    return 0;
            }
            set
            {
                _llprc.BatteryAhdef = value;
                _datiSalvati = false;
            }
        }

        public byte BatteryType
        {
            get
            {
                if (_llprc != null) return _llprc.BatteryType;
                return 0x00;
            }
            set
            {
                _llprc.BatteryType = value;
                _datiSalvati = false;
            }
        }

        public ushort VSoglia
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.VSoglia;
                }
                else
                {
                    return 0;
                }
                return 0;

            }
            set
            {
                _llprc.VSoglia = value;
                _datiSalvati = false;
            }
        }

        public ushort VMax
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.VMax;
                }
                else
                {
                    return 0;
                }
                return 0;

            }
            set
            {
                _llprc.VMax = value;
                _datiSalvati = false;
            }
        }

        public ushort CorrenteMax
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.CorrenteMax;
                }
                else
                {
                    return 0;
                }
                return 0;

            }
            set
            {
                _llprc.CorrenteMax = value;
                _datiSalvati = false;
            }
        }

        public byte EqualTempoAttesa
        {
            get
            {
                if (_llprc != null) return _llprc.EqualTempoAttesa;
                return 0x00;
            }
            set
            {
                _llprc.EqualTempoAttesa = value;
                _datiSalvati = false;
            }
        }

        public byte EqualNumImpulsi
        {
            get
            {
                if (_llprc != null) return _llprc.EqualNumImpulsi;
                return 0x00;
            }
            set
            {
                _llprc.EqualNumImpulsi = value;
                _datiSalvati = false;
            }
        }

        public byte IdProfilo
        {
            get
            {
                if (_llprc != null) return _llprc.IdProfilo;
                return 0x00;
            }
            set
            {
                _llprc.IdProfilo = value;
                _datiSalvati = false;
            }
        }

        public ushort DurataMaxCarica
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.DurataMaxCarica;
                }
                else
                    return 0;
            }
            set
            {
                _llprc.DurataMaxCarica = value;
                _datiSalvati = false;
            }
        }

        public ushort PercTempoFase2
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.PercTempoFase2;
                }
                else
                    return 0;
            }
            set
            {
                _llprc.PercTempoFase2 = value;
                _datiSalvati = false;
            }
        }

        public byte AbilitaComunicazioneSpybatt
        {
            get
            {
                if (_llprc != null) return _llprc.AbilitaComunicazioneSpybatt;
                return 0x00;
            }
            set
            {
                _llprc.AbilitaComunicazioneSpybatt = value;
                _datiSalvati = false;
            }
        }

        public byte TempoErogazioneBMS
        {
            get
            {
                if (_llprc != null) return _llprc.TempoErogazioneBMS;
                return 0x00;
            }
            set
            {
                _llprc.TempoErogazioneBMS = value;
                _datiSalvati = false;
            }
        }

        public byte TempoAttesaBMS
        {
            get
            {
                if (_llprc != null) return _llprc.TempoAttesaBMS;
                return 0x00;
            }
            set
            {
                _llprc.TempoAttesaBMS = value;
                _datiSalvati = false;
            }
        }

        public byte ProgrammaInUso
        {
            get
            {
                if (_llprc != null) return _llprc.ProgrammaInUso;
                return 0x00;
            }
            set
            {
                _llprc.ProgrammaInUso = value;
                _datiSalvati = false;
            }
        }

        public byte TipoRecord
        {
            get
            {
                if (_llprc != null) return _llprc.TipoRecord;
                return 0x00;
            }
            set
            {
                _llprc.TipoRecord = value;
                _datiSalvati = false;
            }
        }
        public string strTipoRecord 
        {
            get
            {
                if (_llprc != null) return _llprc.TipoRecord.ToString("x2");
                return "00";
            }
        }

        public byte OpzioniAttive
        {
            get
            {
                if (_llprc != null) return _llprc.OpzioniAttive;
                return 0x00;
            }
            set
            {
                _llprc.OpzioniAttive = value;
                _datiSalvati = false;
            }
        }
        public string strOpzioniAttive
        {
            get
            {
                if (_llprc != null) return _llprc.OpzioniAttive.ToString("x2");
                return "00";
            }

        }



        #endregion Class Parameter


    }

}
