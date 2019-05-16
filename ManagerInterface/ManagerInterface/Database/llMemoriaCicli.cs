
//    class llMemoriaCicli



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
using BrightIdeasSoftware;
using System.Windows.Forms;

using MoriChargerLogic;
using ChargerLogic;
using Utility;

namespace MoriData
{
    public class _llMemoriaCicli
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        [Indexed(Name = "IDMemCiclo", Order = 1, Unique = true)]
        public string IdApparato { get; set; }
        [Indexed(Name = "IDMemCiclo", Order = 2, Unique = true)]
        public UInt32 IdMemCiclo { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        public int Posizione { get; set; }
        public ushort DurataBreve = 300;
        public ushort IdProgramma { get; set; }
        public UInt32 PuntatorePrimoBreve { get; set; }
        public int NumEventiBrevi { get; set; }
        public string DataOraStart { get; set; }
        public string DataOraFine { get; set; }
        public UInt32 Durata { get; set; }
        public int TempMin { get; set; }
        public int TempMax { get; set; }
        public UInt32 Vmin { get; set; }
        public UInt32 Vmax { get; set; }
        public short Amin { get; set; }
        public short Amax { get; set; }
        public int Ah { get; set; }
        public Int32 Wh { get; set; }
        public byte CondizioneStop { get; set; }
        public byte[] IdSpyBatt;

        public DateTime DataLastDownload { get; set; }

        public override string ToString()
        {
            return IdApparato + " -> " + IdMemCiclo.ToString();
        }
    }


    public class llMemoriaCicli
    {
        public string nullID { get { return "0000000000000000"; } }
        public _llMemoriaCicli _llmc = new _llMemoriaCicli();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("llMemoriaCicli");
        public bool _datiSalvati;
        public bool _recordPresente;
        //private string _tempId;
        private System.Collections.Generic.List<_llMemBreve> CicliMemBreveDB = new System.Collections.Generic.List<_llMemBreve>();
        public System.Collections.Generic.List<llMemBreve> CicliMemoriaBreve = new System.Collections.Generic.List<llMemBreve>();
        //private System.Collections.Generic.List<ChargerLogic.MessaggioSpyBatt.MemoriaPeriodoBreve> _CicliMemoriaBreve = new System.Collections.Generic.List<ChargerLogic.MessaggioSpyBatt.MemoriaPeriodoBreve>();
        public sbProgrammaRicarica ProgrammaAttivo;
        public bool breviDaAgiornare = false;
        public UInt32 PuntatorePrimoBreveEff = 0;

        public elementiComuni.VersoCorrente VersoScarica = elementiComuni.VersoCorrente.Diretto;

        public UInt32 DurataBreve = 300;
        public int LivelloUser = 2;
        /*
        public int LivelloIniziale { get; set; }
        public int LivelloFinale { get; set; }
        public byte StatoCaricaEff { get; set; }
        */

        // Visualizzazione Correnti
        public int DivisoreCorrente = 10;
        public int DivisorePotenza = 10000;

        public byte DecimaliCorrente = 1;
        public byte DecimaliPotenza = 2;

        public llMemoriaCicli()
        {
            _llmc = new _llMemoriaCicli();
            valido = false;
            _database = null;
            _datiSalvati = true;
            _recordPresente = false;
            ProgrammaAttivo = new sbProgrammaRicarica();
        }

        public llMemoriaCicli(_db connessione)
        {
            valido = false;
            _llmc = new _llMemoriaCicli();
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
            ProgrammaAttivo = new sbProgrammaRicarica();
        }

        private _llMemoriaCicli _caricaDati(int _id)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_llMemoriaCicli>()
                        where s.IdLocale == _id
                        select s).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private _llMemoriaCicli _caricaDati(string _IdApparato, uint _IdMemCiclo)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_llMemoriaCicli>()
                        where s.IdApparato == _IdApparato && s.IdMemCiclo == _IdMemCiclo
                        select s).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Genera il Bytearray per l'esportazione
        /// </summary>
        public byte[] DataArray
        {
            get
            {
                byte[] _datamap = new byte[57];
                int _arrayInit = 0;

                // Variabili temporanee per il passaggio dati
                byte _byte1 = 0;
                byte _byte2 = 0;
                byte _byte3 = 0;
                byte _byte4 = 0;
                byte[] _tmpTimestamp;

                // Preparo l'array vuoto
                for (int _i = 0; _i < _datamap.Length; _i++)
                {
                    _datamap[_i] = 0xFF;
                    // 
                }
                /*
                //Event Type
                _datamap[_arrayInit++] = _sblm.TipoEvento;

                // N° evento
                FunzioniComuni.SplitUint32(_sblm.IdMemoriaLunga, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                // Current Prg 
                FunzioniComuni.SplitUshort(_sblm.IdProgramma, ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte2;  ////?????????   1 o 2 ???
                _datamap[_arrayInit++] = _byte1;

                // Puntatore al primo breve
                //                FunzioniComuni.SplitUint32(_sblm.PuntatorePrimoBreve, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                FunzioniComuni.SplitUint32(PuntatorePrimoBreveEff, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                // Contatore Brevi 
                FunzioniComuni.SplitUshort((ushort)_sblm.NumEventiBrevi, ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte1;

                //Inizio
                _tmpTimestamp = FunzioniComuni.splitStringaTS(_sblm.DataOraStart);
                _datamap[_arrayInit++] = _tmpTimestamp[0];
                _datamap[_arrayInit++] = _tmpTimestamp[1];
                _datamap[_arrayInit++] = _tmpTimestamp[2];
                _datamap[_arrayInit++] = _tmpTimestamp[3];
                _datamap[_arrayInit++] = _tmpTimestamp[4];

                //Fine
                _tmpTimestamp = FunzioniComuni.splitStringaTS(_sblm.DataOraFine);
                _datamap[_arrayInit++] = _tmpTimestamp[0];
                _datamap[_arrayInit++] = _tmpTimestamp[1];
                _datamap[_arrayInit++] = _tmpTimestamp[2];
                _datamap[_arrayInit++] = _tmpTimestamp[3];
                _datamap[_arrayInit++] = _tmpTimestamp[4];

                // Durata
                FunzioniComuni.SplitUint32(_sblm.Durata, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                // MaxTemp / MinTemp
                _datamap[_arrayInit++] = (byte)_sblm.TempMin;
                _datamap[_arrayInit++] = (byte)_sblm.TempMax;

                // V Min 
                FunzioniComuni.SplitUint32(_sblm.Vmin, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                // V Max 
                FunzioniComuni.SplitUint32(_sblm.Vmax, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                // A Min 
                //                FunzioniComuni.SplitSShort(_sblm.Amin, ref _byte1, ref _byte2);
                FunzioniComuni.SplitShort(_sblm.Amin, ref _byte2, ref _byte1);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;

                // A Max
                //                FunzioniComuni.SplitSShort(_sblm.Amax, ref _byte1, ref _byte2);
                FunzioniComuni.SplitShort(_sblm.Amax, ref _byte2, ref _byte1);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;

                // Elettrolita
                _datamap[_arrayInit++] = _sblm.PresenzaElettrolita;

                // Ah Caricati
                FunzioniComuni.SplitSShort((short)(_sblm.AhCaricati), ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;

                // Wh Caricati
                FunzioniComuni.SplitSInt32(_sblm.WhCaricati, ref _byte1, ref _byte2, ref _byte3, ref _byte4, 3);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;

                // Ah Scaricati
                FunzioniComuni.SplitSShort((short)(_sblm.AhScaricati), ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;

                // Wh Scaricati
                FunzioniComuni.SplitSInt32(_sblm.WhScaricati, ref _byte1, ref _byte2, ref _byte3, ref _byte4, 3);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;
                // _datamap[_arrayInit++] = 0xFF;

                _datamap[_arrayInit++] = _sblm.CondizioneStop;  // Stop
                _datamap[_arrayInit++] = _sblm.FattoreCarica;   // Fattore di Carica
                _datamap[_arrayInit++] = _sblm.StatoCatica;     // Stato carica

                // Caricatore
                FunzioniComuni.SplitInt32(_sblm.TipoCariatore, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;

                FunzioniComuni.SplitUint32(_sblm.IdCaricatore, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                _datamap[_arrayInit++] = _byte2;
                _datamap[_arrayInit++] = _byte3;
                _datamap[_arrayInit++] = _byte4;
                */
                return _datamap;
            }

        }

        public bool caricaDati(int idLocale)
        {
            try
            {
                // carico i dati testata lungo da db
                _llmc = _caricaDati(idLocale);
                if (_llmc == null)
                {
                    _llmc = new _llMemoriaCicli();
                    return false;
                }
                else
                {
                    // se la testata è salvata, carico i brevi

                    //CaricaProgramma();
                    //CaricaBrevi();

                }
                //azzero sempre il puntatore a breve, la lettura la faccio dopo aver letto il puntatore dall'apparato
                PuntatorePrimoBreveEff = 0;
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool caricaDati(string IdApparato, uint IdMemCiclo)
        {
            try
            {
                _llmc = _caricaDati(IdApparato, IdMemoriaLunga);
                if (_llmc == null)
                {
                    _llmc = new _llMemoriaCicli();
                    _llmc.IdApparato = IdApparato;
                    _llmc.IdMemCiclo = IdMemCiclo;
                    //azzero sempre il puntatore a breve, la lettura la faccio dopo aver letto il puntatore dall'apparato
                    PuntatorePrimoBreveEff = 0;
                    return false;
                }
                else
                {
                   // CaricaProgramma();
                   // CaricaBrevi();
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
                if (_llmc.IdApparato != nullID & _llmc.IdApparato != null & _llmc.IdMemCiclo != null)
                {

                    _llMemoriaCicli _TestDati = _caricaDati(_llmc.IdApparato, _llmc.IdMemCiclo);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _llmc.CreationDate = DateTime.Now;
                        _llmc.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_llmc);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _llmc.IdLocale = _TestDati.IdLocale;
                        _llmc.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_llmc);
                        _datiSalvati = true;
                    }

                    //_database.InsertOrReplace(_sb);
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
                // 1. Dati cliente
                _llMemoriaCicli _newML = FunzioniComuni.CloneJson<_llMemoriaCicli>(_llmc);
                _newML.IdApparato = NuovoIdApparato;

                int _result = _database.Insert(_newML);

                if (_result == 1)
                {
                    ClonaBrevi(NuovoIdApparato);
                    return true;
                }
                return false;
            }
            catch (Exception Ex)
            {
                Log.Error("ClonaRecord: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }


        public bool CaricaBrevi()
        {
            try
            {
                /*
                CicliMemoriaBreve.Clear();
                if (_database == null)
                    return false;

                IEnumerable<_sbMemBreve> _TempCicli = _database.Query<_sbMemBreve>("select * from _sbMemBreve where IdApparato = ? and IdMemoriaLunga = ? order by IdMemoriaBreve ", _sblm.IdApparato, _sblm.IdMemoriaLunga);

                foreach (_sbMemBreve Elemento in _TempCicli)
                {
                    sbMemBreve _cLoc;
                    _cLoc = new sbMemBreve(Elemento);
                    CicliMemoriaBreve.Add(_cLoc);
                }
                */
                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaBrevi " + Ex.Message);
                return false;
            }
        }

        public bool SalvaBrevi()
        {
            try
            {
                /*
                DateTime _start = DateTime.Now;
                Log.Debug("Start SalvaBrevi ");
                if (_database == null)
                    return false;

                CicliMemBreveDB.Clear();
                SQLiteCommand CancellaCicli = _database.CreateCommand("delete from _sbMemBreve where IdApparato = ? and IdMemoriaLunga = ? ", _sblm.IdApparato, _sblm.IdMemoriaLunga);
                int esito = CancellaCicli.ExecuteNonQuery();
                Log.Debug("Brevi cancellati da db");


                foreach (sbMemBreve Elemento in CicliMemoriaBreve)
                {
                    _sbMemBreve _cLoc;
                    _cLoc = Elemento._sbsm;
                    _cLoc.CreationDate = DateTime.Now;
                    CicliMemBreveDB.Add(_cLoc);
                }
                Log.Debug("Brevi pronti");


                int _result = _database.InsertAll(CicliMemBreveDB);
                Log.Debug("Brevi salvati su db");
                //CaricaBrevi();
                //Log.Warn("Brevi ricaricati");

            */
                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaBrevi " + Ex.Message);
                return false;
            }
        }

        public bool ClonaBrevi(string NuovoIdApparato)
        {
            try
            {
                /*
                DateTime _start = DateTime.Now;
                Log.Debug("Start SalvaBrevi ");
                if (_database == null)
                    return false;

                CicliMemBreveDB.Clear();

                foreach (sbMemBreve Elemento in CicliMemoriaBreve)
                {
                    _sbMemBreve _newMB = FunzioniComuni.CloneJson<_sbMemBreve>(Elemento._sbsm);
                    _newMB.IdApparato = NuovoIdApparato;
                    _newMB.CreationDate = DateTime.Now;
                    CicliMemBreveDB.Add(_newMB);
                }
                Log.Debug("Brevi pronti");


                int _result = _database.InsertAll(CicliMemBreveDB);
                Log.Debug("Brevi salvati su db");
                */
                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaBrevi " + Ex.Message);
                return false;
            }
        }

        public bool CancellaBrevi()
        {
            try
            {
                /*
                if (_database == null)
                    return false;
                SQLiteCommand CancellaCicli = _database.CreateCommand("delete from _sbMemBreve where IdApparato = ? and IdMemoriaLunga = ? ", _sblm.IdApparato, _sblm.IdMemoriaLunga);
                int esito = CancellaCicli.ExecuteNonQuery();
                CaricaBrevi();
                */
                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("CancellaBrevi " + Ex.Message);
                return false;
            }
        }

        public bool cancellaDati()
        {
            try
            {
                if (_database == null)
                    return false;
                SQLiteCommand CancellaRecord = _database.CreateCommand("delete from _llMemoriaCicli where IdApparato = ? and IdMemoriaLunga = ? ", _llmc.IdApparato, _llmc.IdMemCiclo);
                int esito = CancellaRecord.ExecuteNonQuery();
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }


        public bool CaricaProgramma()
        {
            try
            {
                /*
                sbProgrammaRicarica _tempProgramma = new sbProgrammaRicarica(_database);
                bool _esito = _tempProgramma.caricaDati(_sblm.IdApparato, _sblm.IdProgramma);

                if (_esito)
                {
                    ProgrammaAttivo = _tempProgramma;
                }
                else
                {
                    ProgrammaAttivo = null;
                    //Log.Debug("MemLunga.CaricaProgramma: Non Trovato (" + _sblm.IdApparato.ToString() + " - " + _sblm.IdProgramma.ToString() + ") " + _tempProgramma.ToString());
                }
                */
                return true;// _esito;

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaBrevi " + Ex.Message);
                return false;
            }
        }



        #region Class Parameters

        [OLVColumn(IsVisible = false)]
        public int IdLocale
        {
            get { return _llmc.IdLocale; }
            set
            {

                _llmc.IdLocale = value;
                _datiSalvati = false;

            }
        }

        [OLVColumn(IsVisible = true, Width = 100, DisplayIndex = 1, TextAlign = HorizontalAlignment.Right)]
        public uint IdMemoriaLunga
        {
            get { return _llmc.IdMemCiclo; }
            set
            {

                _llmc.IdMemCiclo = value;
                _datiSalvati = false;

            }
        }

       // [OLVColumn(IsVisible = true, Width = 100, DisplayIndex = 1, TextAlign = HorizontalAlignment.Right)]
        public string strIdMemCiclo
        {
            get { return _llmc.IdMemCiclo.ToString(); }

        }

        // [OLVColumn(IsVisible = true, Width = 100, DisplayIndex = 1, TextAlign = HorizontalAlignment.Right)]
        public string strSortIdMemCiclo
        {
            get { return _llmc.IdMemCiclo.ToString("00000000"); }

        }

        [OLVColumn(IsVisible = false)]
        public DateTime CreationDate
        {
            get { return _llmc.CreationDate; }
        }

        [OLVColumn(IsVisible = false)]
        public DateTime RevisionDate
        {
            get { return _llmc.RevisionDate; }
        }

        [OLVColumn(IsVisible = false)]
        public string LastUser
        {
            get { return _llmc.LastUser; }
        }

        public ushort IdProgramma
        {
            get { return _llmc.IdProgramma; }
            set
            {
                _llmc.IdProgramma = value;
                _datiSalvati = false;
            }
        }
        public string strIdProgramma
        {
            get { return _llmc.IdProgramma.ToString(); }

        }

        public UInt32 PuntatorePrimoBreve
        {
            get { return _llmc.PuntatorePrimoBreve; }
            set
            {
                _llmc.PuntatorePrimoBreve = value;
                _datiSalvati = false;
            }
        }

        public string strPuntatorePrimoBreve
        {
            get
            {
                return _llmc.PuntatorePrimoBreve.ToString("x6");
            }

        }

        public int NumEventiBrevi
        {
            get { return _llmc.NumEventiBrevi; }
            set
            {
                _llmc.NumEventiBrevi = value;
                _datiSalvati = false;
            }
        }

        public int Posizione
        {
            get { return _llmc.Posizione; }
            set
            {
                _llmc.Posizione = value;
                _datiSalvati = false;
            }
        }

        public int NumEventiBreviCaricati
        {
            get
            {
                /*
                if (CicliMemoriaBreve != null)
                {
                    return CicliMemoriaBreve.Count();
                }
                else
                {
                */
                    return 0;
               // }
            }
        }

        public double PercEventiBreviCaricati
        {
            get
            {
                /*
                if (CicliMemoriaBreve != null)
                {
                    if (_sblm.NumEventiBrevi > 0)
                    {
                        return (double)CicliMemoriaBreve.Count() / (double)_sblm.NumEventiBrevi * 100;
                    }
                    else
                    {
                    
                        return 0;
                    }

                }
                else
                {
                    return 0;
                }
                */
                return 0;
            }
        }


        public string strNumEventiBrevi
        {
            get { return NumEventiBreviCaricati.ToString() + " / " + NumEventiBrevi.ToString(); }
        }

        public string DataOraStart
        {
            get { return _llmc.DataOraStart; }
            set
            {
                _llmc.DataOraStart = value;
                _datiSalvati = false;
            }
        }

        public DateTime dtDataOraStart
        {
            get
            {
                DateTime _dataora;


                if (_llmc.DataOraStart.Length != 15)
                {
                    // data non formattata correttamente
                    return DateTime.Now;
                }

                //if (DateTime.TryParseExact(_sbsm.DataOraRegistrazione,"dd/MM/yy HH:mm",CultureInfo.InvariantCulture,DateTimeStyles.None,out _dataora))
                if (DateTime.TryParse(_llmc.DataOraStart, out _dataora))
                {
                    return _dataora;
                }


                return DateTime.Now;

            }
        }

        public string DataOraFine
        {
            get { return _llmc.DataOraFine; }
            set
            {
                _llmc.DataOraFine = value;
                _datiSalvati = false;
            }
        }

        public DateTime dtDataOraFine
        {
            get
            {
                DateTime _dataora;


                if (_llmc.DataOraFine.Length != 15)
                {
                    // data non formattata correttamente
                    return DateTime.Now;
                }

                //if (DateTime.TryParseExact(_sbsm.DataOraRegistrazione,"dd/MM/yy HH:mm",CultureInfo.InvariantCulture,DateTimeStyles.None,out _dataora))
                if (DateTime.TryParse(_llmc.DataOraFine, out _dataora))
                {
                    return _dataora;
                }


                return DateTime.Now;

            }
        }

        public UInt32 Durata
        {
            get { return _llmc.Durata; }
            set
            {
                _llmc.Durata = value;
                _datiSalvati = false;
            }
        }

        public String strDurata
        {
            get { return _llmc.Durata.ToString(); }

        }

        public int Ah
        {
            get { return _llmc.Ah; }
            set
            {
                _llmc.Ah = value;
                _datiSalvati = false;
            }
        }

        public string strAh
        {
            get
            {
                    return FunzioniMR.StringaCapacita(_llmc.Ah, DivisoreCorrente, DecimaliCorrente);
            }
        }

        public float ValAh
        {
            get { return FunzioniMR.ValoreEffettivo(_llmc.Ah, DivisoreCorrente); }
        }


        public Int32 Wh
        {
            get { return _llmc.Wh; }
            set
            {
                _llmc.Wh = value;
                _datiSalvati = false;
            }
        }

        public string strKWh
        {
            get
            {
                return FunzioniMR.StringaPotenza(_llmc.Wh, DivisorePotenza, DecimaliPotenza);
            }
        }

        public float ValKWh
        {
            get { return FunzioniMR.ValoreEffettivo(_llmc.Wh, DivisorePotenza); }
        }


        public byte CondizioneStop
        {
            get { return _llmc.CondizioneStop; }
            set
            {
                _llmc.CondizioneStop = value;
                _datiSalvati = false;
            }

        }

        public string strCondizioneStop
        {
            get
            {
                return _llmc.CondizioneStop.ToString("X2");
            }
        }



        public byte[] IdSpyBatt
        {
            get { return _llmc.IdSpyBatt; }
            set
            {
                _llmc.IdSpyBatt = value;
                _datiSalvati = false;
            }
        }


        public string strIdSpyBatt
        {
            get
            {
                return _llmc.IdSpyBatt.ToString();
            }
        }


        public DateTime DataLastDownload
        {
            get { return _llmc.DataLastDownload; }
            set
            {
                _llmc.DataLastDownload = value;
                _datiSalvati = false;
            }
        }

        public byte ModStop
        {
            get { return _llmc.CondizioneStop; }
            set
            {
                _llmc.CondizioneStop = value;
                _datiSalvati = false;
            }

        }

        public string strChargerStop
        {
            get
            {
                string _descrStato = "?";
                switch (_llmc.CondizioneStop)
                {
                    case 0x00:
                        {
                            _descrStato = "Strappo(00)";
                            break;
                        }
                    case 0x01:
                        {
                            _descrStato = "Ah-SB (01)";
                            break;
                        }

                    case 0x02:
                        {
                            _descrStato = "Ah-LL (02)";
                            break;
                        }
                    case 0x03:
                        {
                            _descrStato = "KBD (03)";
                            break;
                        }
                    case 0x04:
                        {
                            _descrStato = "Adap E (04)";
                            break;
                        }
                    case 0x05:
                        {
                            _descrStato = "Adap I (05)";
                            break;
                        }

                    default:
                        {
                            _descrStato = "N.D. (" + _llmc.CondizioneStop.ToString("x2") + ")";
                            break;
                        }
                }

                return _descrStato;
            }

        }



        #endregion Class Parameter



    }


}

