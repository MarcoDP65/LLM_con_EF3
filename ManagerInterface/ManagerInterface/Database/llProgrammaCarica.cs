
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
        [Indexed(Name = "IDXProgrammaCarica", Order = 1, Unique = true)]
        public string IdApparato { get; set; }

        [Indexed(Name = "IDXProgrammaCarica", Order = 2, Unique = true)]
        public string TipoApparato { get; set; }
        // Identifica l'apparato a cui appartiene l'ID, ovvero dove è stato caricato
        // LL - Ladelight
        // SB - SPY-BATT
        // IB - ID-BATT

        [Indexed(Name = "IDXProgrammaCarica", Order = 2, Unique = true)]

        public ushort IdProgramma { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        public string DataInstallazione { get; set; }
        public string ProgramName { get; set; }
        public ushort BatteryType { get; set; }
        public ushort BatteryVdef { get; set; }
        public ushort BatteryAhdef { get; set; }
        public byte   NumeroCelle { get; set; }

        public ushort VSoglia { get; set; }
        public ushort VRaccordoF1 { get; set; }
        public ushort VMax { get; set; }
        public ushort VCellLimite { get; set; }
        public ushort BatteryVminRec { get; set; }
        public ushort BatteryVmaxRec { get; set; }
        public ushort BatteryVminStop { get; set; }

        public ushort CorrenteMax { get; set; }
        public ushort CorrenteFase3 { get; set; }

        public ushort EqualTempoAttesa { get; set; }
        public ushort EqualNumImpulsi { get; set; }
        public ushort EqualDurataPausa { get; set; }
        public ushort EqualDurataImpulso { get; set; }
        public ushort EqualCorrenteImpulso { get; set; }
        public ushort IdProfilo { get; set; }
        public ushort DurataMaxCarica { get; set; }
        public ushort PercTempoFase2 { get; set; }
        public ushort DurataMinFase2 { get; set; }
        public ushort DurataMaxFase2 { get; set; }
        public ushort DurataMaxFase3 { get; set; }

        public ushort OpportunityOraInizio { get; set; }
        public ushort OpportunityOraFine { get; set; }
        public ushort OpportunityDurataMax { get; set; }
        public ushort OpportunityTensioneMax { get; set; }
        public ushort OpportunityCorrente { get; set; }


        public byte AbilitaContattoSafety { get; set; }
        public byte AbilitaComunicazioneSpybatt { get; set; }
        public byte TempoErogazioneBMS { get; set; }
        public byte TempoAttesaBMS { get; set; }
        public byte ProgrammaInUso { get; set; }
        public byte TipoRecord { get; set; }
        public byte OpzioniAttive { get; set; }
        public byte IdModelloLL { get; set; }

        public bool IsEqual( _llProgrammaCarica ProgCarica)
        {
            try
            {

                if (IdApparato != ProgCarica.IdApparato) return false;
                if (IdProgramma != ProgCarica.IdProgramma) return false;
                if (DataInstallazione != ProgCarica.DataInstallazione ) return false;
                if (ProgramName != ProgCarica.ProgramName) return false;
                if (BatteryType != ProgCarica.BatteryType) return false;
                if (BatteryVdef != ProgCarica.BatteryVdef) return false;
                if (BatteryAhdef != ProgCarica.BatteryAhdef) return false;
                if (VSoglia != ProgCarica.VSoglia) return false;
                if (VRaccordoF1 != ProgCarica.VRaccordoF1) return false;
                if (VMax != ProgCarica.VMax) return false;
                if (VCellLimite != ProgCarica.VCellLimite) return false;
                if (BatteryVminRec != ProgCarica.BatteryVminRec) return false;
                if (BatteryVmaxRec != ProgCarica.BatteryVmaxRec) return false;
                if (BatteryVminStop != ProgCarica.BatteryVminStop) return false;
                if (CorrenteMax != ProgCarica.CorrenteMax) return false;
                if (CorrenteFase3 != ProgCarica.CorrenteFase3) return false;
                if (EqualTempoAttesa != ProgCarica.EqualTempoAttesa) return false;
                if (EqualNumImpulsi != ProgCarica.EqualNumImpulsi) return false;
                if (EqualDurataPausa != ProgCarica.EqualDurataPausa) return false;
                if (EqualDurataImpulso != ProgCarica.EqualDurataImpulso) return false;
                if (EqualCorrenteImpulso != ProgCarica.EqualCorrenteImpulso) return false;
                if (IdProfilo != ProgCarica.IdProfilo) return false;
                if (DurataMaxCarica != ProgCarica.DurataMaxCarica) return false;
                if (PercTempoFase2 != ProgCarica.PercTempoFase2) return false;
                if (DurataMinFase2 != ProgCarica.DurataMinFase2) return false;
                if (DurataMaxFase2 != ProgCarica.DurataMaxFase2) return false;
                if (DurataMaxFase3 != ProgCarica.DurataMaxFase3) return false;
                if (AbilitaComunicazioneSpybatt != ProgCarica.AbilitaComunicazioneSpybatt) return false;
                if (TempoErogazioneBMS != ProgCarica.TempoErogazioneBMS) return false;
                if (TempoAttesaBMS != ProgCarica.TempoAttesaBMS) return false;
                if (ProgrammaInUso != ProgCarica.ProgrammaInUso) return false;
                if (TipoRecord != ProgCarica.TipoRecord) return false;
                if (OpzioniAttive != ProgCarica.OpzioniAttive) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }


    }

    public class llProgrammaCarica
    {
        public parametriSistema Parametri { get; set; }
        public string nullID { get { return "LL000000"; } }
        public _llProgrammaCarica _llprc = new _llProgrammaCarica();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("llProgrammaCarica");
        public bool _datiSalvati;
        public bool _recordPresente;
        public byte PosizioneCorrente { get; set; }
        public bool ProgrammaAttivo { get; set; }


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

                // Tipo Profilo
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TipoProfilo;
                _par.ValoreParametro = _llprc.IdProfilo;
                ListaParametri.Add(_par);

                // Durata Carica
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TempoT1Max;
                _par.ValoreParametro = _llprc.DurataMaxCarica;
                ListaParametri.Add(_par);

                // Durata Fase 2
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TempoT2Min;
                _par.ValoreParametro = _llprc.DurataMinFase2;
                ListaParametri.Add(_par);

                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TempoT2Max;
                _par.ValoreParametro = _llprc.DurataMaxFase2;
                ListaParametri.Add(_par);

                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.CoeffK;
                _par.ValoreParametro = _llprc.PercTempoFase2;
                ListaParametri.Add(_par);

                // Durata Fase 3
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TempoT3Max;
                _par.ValoreParametro = _llprc.DurataMaxFase3;
                ListaParametri.Add(_par);

                // Numero Celle
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.NumeroCelle;
                _par.ValoreParametro = _llprc.NumeroCelle;
                ListaParametri.Add(_par);

                // Tensione Nominale
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneNominale;
                _par.ValoreParametro = _llprc.BatteryVdef;
                ListaParametri.Add(_par);

                // Tensione di soglia
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneSogliaF1;
                _par.ValoreParametro = _llprc.VSoglia;
                ListaParametri.Add(_par);

                // Tensione di raccordo F1
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneRaccordoF1;
                _par.ValoreParametro = _llprc.VRaccordoF1;
                ListaParametri.Add(_par);

                // Tensione Massima
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneMassimaCella;
                _par.ValoreParametro = _llprc.VMax;
                ListaParametri.Add(_par);

                // Tensione Limite
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneLimiteCella;
                _par.ValoreParametro = _llprc.VCellLimite;
                ListaParametri.Add(_par);

                // Tensioni Riconoscimento
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneMinimaRiconoscimento;
                _par.ValoreParametro = _llprc.BatteryVminRec;
                ListaParametri.Add(_par);
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneMassimaRiconoscimento;
                _par.ValoreParametro = _llprc.BatteryVmaxRec;
                ListaParametri.Add(_par);
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.TensioneMinimaStop;
                _par.ValoreParametro = _llprc.BatteryVminStop;
                ListaParametri.Add(_par);

                // Capacità Nominale
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.CapacitaNominale;
                _par.ValoreParametro = _llprc.BatteryAhdef;
                ListaParametri.Add(_par);

                // Corrente Massima (  == Corrente di carica )
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.CorrenteMassima;
                _par.ValoreParametro = _llprc.CorrenteMax;
                ListaParametri.Add(_par);

                // Corrente Fase 3
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.CorrenteF3;
                _par.ValoreParametro = _llprc.CorrenteFase3;
                ListaParametri.Add(_par);



                // Equal fine carica:
                // Attesa Iniziale
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.EqualFineCaricaAttesa;
                _par.ValoreParametro = _llprc.EqualTempoAttesa;
                ListaParametri.Add(_par);
                // Numero Impulsi
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.EqualFineCaricaNumImpulsi;
                _par.ValoreParametro = _llprc.EqualNumImpulsi;
                ListaParametri.Add(_par);
                // Tempo Pausa
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.EqualFineCaricaDurataP;
                _par.ValoreParametro = _llprc.EqualDurataPausa;
                ListaParametri.Add(_par);
                // Tempo Erogazione
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.EqualFineCaricaDurataI;
                _par.ValoreParametro = _llprc.EqualDurataImpulso;
                ListaParametri.Add(_par);
                // Corrente Impulso
                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.EqualFineCaricaCorrenteImp;
                _par.ValoreParametro = _llprc.EqualCorrenteImpulso;
                ListaParametri.Add(_par);

                _par = new ParametroLL();
                _par.idParametro = (byte)SerialMessage.ParametroLadeLight.ModelloLadeLight;
                _par.ValoreParametro = _llprc.IdModelloLL;
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

                        case (byte)SerialMessage.ParametroLadeLight.TipoProfilo:
                            _llprc.IdProfilo = (byte)_par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TipoBatteria:
                            _llprc.BatteryType = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.CapacitaNominale:
                            _llprc.BatteryAhdef = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TempoT1Max:
                            _llprc.DurataMaxCarica = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TempoT2Min:
                            _llprc.DurataMinFase2 = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TempoT2Max:
                            _llprc.DurataMaxFase2= _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.CoeffK:
                            _llprc.PercTempoFase2 = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TempoT3Max:
                            _llprc.DurataMaxFase3 = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.NumeroCelle:
                            _llprc.NumeroCelle = (byte)_par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TensioneNominale:
                            _llprc.BatteryVdef = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TensioneSogliaF1:
                            _llprc.VSoglia = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TensioneRaccordoF1:
                            _llprc.VRaccordoF1 = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TensioneMassimaCella:
                            _llprc.VMax = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TensioneLimiteCella:
                            _llprc.VCellLimite = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TensioneMinimaRiconoscimento:
                            _llprc.BatteryVminRec = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TensioneMassimaRiconoscimento:
                            _llprc.BatteryVmaxRec = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.TensioneMinimaStop:
                            _llprc.BatteryVminStop = _par.ValoreParametro;
                            break;

                        case (byte)SerialMessage.ParametroLadeLight.CorrenteMassima:
                            _llprc.CorrenteMax = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.CorrenteF3:
                            _llprc.CorrenteFase3 = _par.ValoreParametro;
                            break;

                        case (byte)SerialMessage.ParametroLadeLight.EqualFineCaricaAttesa:
                            _llprc.EqualTempoAttesa = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.EqualFineCaricaNumImpulsi:
                            _llprc.EqualNumImpulsi = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.EqualFineCaricaDurataP:
                            _llprc.EqualDurataPausa = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.EqualFineCaricaDurataI:
                            _llprc.EqualDurataImpulso = _par.ValoreParametro;
                            break;
                        case (byte)SerialMessage.ParametroLadeLight.EqualFineCaricaCorrenteImp:
                            _llprc.EqualCorrenteImpulso = _par.ValoreParametro;
                            break;


                        case (byte)SerialMessage.ParametroLadeLight.DivisoreK:
                            _llprc.AbilitaComunicazioneSpybatt = (byte)_par.ValoreParametro;
                            break;

                        case (byte)SerialMessage.ParametroLadeLight.ModelloLadeLight:
                            _llprc.IdModelloLL = (byte)_par.ValoreParametro;
                            break;

                        case (byte)SerialMessage.ParametroLadeLight.RiarmoBMS:
                            FunzioniComuni.SplitUshort(_par.ValoreParametro, ref loval, ref hival);
                            _llprc.TempoAttesaBMS = hival;
                            _llprc.TempoErogazioneBMS = loval;
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

        public bool IsEqual(llProgrammaCarica ProgCarica)
        {
            try
            {
                if (!_llprc.IsEqual(ProgCarica._llprc)) return false;


                return true;
            }
            catch
            {
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

        public string strPosizioneCorrente
        {
            get
            {
                return PosizioneCorrente.ToString("000");
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

            }
            set
            {
                _llprc.BatteryVdef = value;
                _datiSalvati = false;
            }
        }

        public string strBatteryVdef
        {
            get
            {
                if (_llprc != null)
                {
                    return FunzioniMR.StringaTensione(_llprc.BatteryVdef);
                }
                else
                    return "N.D.";
            
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

        public string strBatteryAhdef
        {
            get
            {
                if (_llprc != null)
                {
                    return FunzioniMR.StringaCorrenteLL( _llprc.BatteryAhdef) ;
                }
                else
                    return "N.D.";
            }

        }


        public ushort BatteryType
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

        public byte NumeroCelle
        {
            get
            {
                if (_llprc != null) return _llprc.NumeroCelle;
                return 0x00;
            }
            set
            {
                _llprc.NumeroCelle = value;
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

        public ushort VRaccordoF1
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.VRaccordoF1;
                }
                else
                {
                    return 0;
                }

            }
            set
            {
                _llprc.VRaccordoF1 = value;
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

            }
            set
            {
                _llprc.VMax = value;
                _datiSalvati = false;
            }
        }

        public ushort VCellLimite
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.VCellLimite;
                }
                else
                {
                    return 0;
                }

            }
            set
            {
                _llprc.VCellLimite = value;
                _datiSalvati = false;
            }
        }

        public ushort VMinRec
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.BatteryVminRec;
                }
                else
                {
                    return 0;
                }

            }
            set
            {
                _llprc.BatteryVminRec = value;
                _datiSalvati = false;
            }
        }

        public ushort VMaxRec
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.BatteryVmaxRec;
                }
                else
                {
                    return 0;
                }

            }
            set
            {
                _llprc.BatteryVmaxRec = value;
                _datiSalvati = false;
            }
        }

        public ushort VMinStop
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.BatteryVminStop;
                }
                else
                {
                    return 0;
                }

            }
            set
            {
                _llprc.BatteryVminStop = value;
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

            }
            set
            {
                _llprc.CorrenteMax = value;
                _datiSalvati = false;
            }
        }

        public ushort CorrenteFase3
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.CorrenteFase3;
                }
                else
                {
                    return 0;
                }

            }
            set
            {
                _llprc.CorrenteFase3 = value;
                _datiSalvati = false;
            }
        }

        public ushort EqualTempoAttesa
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

        public ushort EqualNumImpulsi
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

        public ushort EqualDurataPausa
        {
            get
            {
                if (_llprc != null) return _llprc.EqualDurataPausa;
                return 0x00;
            }
            set
            {
                _llprc.EqualDurataPausa = value;
                _datiSalvati = false;
            }
        }

        public ushort EqualDurataImpulso
        {
            get
            {
                if (_llprc != null) return _llprc.EqualDurataImpulso;
                return 0x00;
            }
            set
            {
                _llprc.EqualDurataImpulso = value;
                _datiSalvati = false;
            }
        }

        public ushort EqualCorrenteImpulso
        {
            get
            {
                if (_llprc != null) return _llprc.EqualCorrenteImpulso;
                return 0x00;
            }
            set
            {
                _llprc.EqualCorrenteImpulso = value;
                _datiSalvati = false;
            }
        }
        public ushort IdProfilo
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

        public ushort DurataMinFase2
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.DurataMinFase2;
                }
                else
                    return 0;
            }
            set
            {
                _llprc.DurataMinFase2 = value;
                _datiSalvati = false;
            }
        }

        public ushort DurataMaxFase2
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.DurataMaxFase2;
                }
                else
                    return 0;
            }
            set
            {
                _llprc.DurataMaxFase2 = value;
                _datiSalvati = false;
            }
        }

        public ushort DurataMaxFase3
        {
            get
            {
                if (_llprc != null)
                {
                    return _llprc.DurataMaxFase3;
                }
                else
                    return 0;
            }
            set
            {
                _llprc.DurataMaxFase3 = value;
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

        public byte IdModelloLL
        {
            get
            {
                if (_llprc != null) return _llprc.IdModelloLL;
                return 0x00;
            }
            set
            {
                _llprc.IdModelloLL = value;
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

        public ushort TipoBatteria
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

        public string strTipoBatteria
        {
            get
            {
                try
                {
                    if (_llprc != null)
                    {
                        mbTipoBatteria TipoBase = Parametri.ParametriProfilo.ModelliBatteria.Find(x => x.BatteryTypeId == _llprc.BatteryType);
                        if (TipoBase != null)
                        {
                            return TipoBase.BatteryType;
                        }
                        else
                        {
                            return "N.D.";
                        }

                    }
                    return "";
                }
                catch
                {
                    return "N.D.";
                }
            }

        }


        public ushort TipoProfilo
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

        public string strTipoProfilo
        {
            get
            {
                try
                {
                    if (_llprc != null)
                    {
                        _mbProfiloCarica TipoBase = Parametri.ParametriProfilo.ProfiliCarica.Find(x => x.IdProfiloCaricaLL == _llprc.IdProfilo);
                        if (TipoBase != null)
                        {
                            return TipoBase.NomeProfilo;
                        }
                        else
                        {
                            return "N.D.";
                        }

                    }
                    return "";
                }
                catch
                {
                    return "N.D.";
                }
            }
        }


        



        #endregion Class Parameter


    }

}
