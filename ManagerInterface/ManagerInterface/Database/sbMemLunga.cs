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
    public class _sbMemLunga
    {
        [PrimaryKey]
        [AutoIncrement]
        public Int32 IdLocale { get; set; }

        [MaxLength(24)]
        [Indexed(Name = "IDMemLunga", Order = 1, Unique = true)]
        public string IdApparato { get; set; }
        [Indexed(Name = "IDMemLunga", Order = 2, Unique = true)]
        public UInt32 IdMemoriaLunga { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        [MaxLength(20)]
        public string LastUser { get; set; }
        public UInt32 DurataBreve = 300;
        public byte TipoEvento { get; set; }
        public ushort IdProgramma { get; set; }
        public UInt32 PuntatorePrimoBreve { get; set; }
        public int NumEventiBrevi { get; set; }
        public string DataOraStart { get; set; }
        public string DataOraFine { get; set; }
        public UInt32 Durata { get; set; }
        public int  TempMin { get; set; }
        public int TempMax { get; set; }
        public UInt32 Vmin { get; set; }
        public UInt32 Vmax { get; set; }
        public short Amin { get; set; }
        public short Amax { get; set; }
        public byte PresenzaElettrolita { get; set; }
        public int Ah { get; set; }
        public Int32 Wh { get; set; }
        public byte CondizioneStop { get; set; }
        public byte FattoreCarica { get; set; }
        public byte StatoCatica { get; set; }
        public int TipoCariatore { get; set; }
        public UInt32 IdCaricatore { get; set; }
        public int AhCaricati { get; set; }
        public int AhScaricati { get; set; }
        public ushort VMaxSbilanciamento { get; set; }
        public UInt32 DurataMancanzaElettrolita { get; set; }
        public float VMaxSbilanciamentoC { get; set; }
        public Int32 WhCaricati { get; set; }
        public Int32 WhScaricati { get; set; }
        public UInt32 DurataSbilanciamento { get; set; }
        public UInt32 DurataOverTemp { get; set; }
        public DateTime DataLastDownload { get; set; }



        public override string ToString()
        {
            return IdApparato + " -> " + IdMemoriaLunga.ToString();
        }
    }

    public class sbStatMemLunga
    {

    }

    public class sbMemLunga
    {
        public string nullID { get { return "0000000000000000"; } }
        public _sbMemLunga _sblm = new _sbMemLunga();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;
        private string _tempId;
        private System.Collections.Generic.List<_sbMemBreve> CicliMemBreveDB = new System.Collections.Generic.List<_sbMemBreve>();
        public System.Collections.Generic.List<sbMemBreve> CicliMemoriaBreve = new System.Collections.Generic.List<sbMemBreve>();
        private System.Collections.Generic.List<ChargerLogic.MessaggioSpyBatt.MemoriaPeriodoBreve> _CicliMemoriaBreve = new System.Collections.Generic.List<ChargerLogic.MessaggioSpyBatt.MemoriaPeriodoBreve>();
        public sbProgrammaRicarica ProgrammaAttivo;
        public bool breviDaAgiornare = false;
        public UInt32 PuntatorePrimoBreveEff = 0;
        public elementiComuni.VersoCorrente VersoScarica = elementiComuni.VersoCorrente.Diretto;
        public UInt32 DurataBreve = 300;
        public int LivelloUser = 2;

        public int LivelloIniziale { get; set; }
        public int LivelloFinale { get; set; }
        public byte StatoCaricaEff { get; set; }


        // Visualizzazione Correnti
        public int DivisoreCorrente = 10;
        public int DivisorePotenza = 10000;

        public byte DecimaliCorrente = 1;
        public byte DecimaliPotenza = 2;

        public sbMemLunga()
        {
            _sblm = new _sbMemLunga();
            valido = false;
            _database = null;
            _datiSalvati = true;
            _recordPresente = false;
            ProgrammaAttivo = new sbProgrammaRicarica();
        }

        public sbMemLunga(_db connessione)
        {
            valido = false;
            _sblm = new _sbMemLunga(); 
            _database = connessione;
            _datiSalvati = true;
            _recordPresente = false;
            ProgrammaAttivo = new sbProgrammaRicarica();
        }

        private _sbMemLunga _caricaDati(int _id)
        {
            if (_database != null)
            {
                return (from s in _database.Table<_sbMemLunga>()
                        where s.IdLocale == _id
                        select s).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private _sbMemLunga _caricaDati(string _IdApparato, uint _IdMemoriaLunga)
        {
            if (_database != null)
            {
            return (from s in _database.Table<_sbMemLunga>()
                    where  s.IdApparato == _IdApparato && s.IdMemoriaLunga == _IdMemoriaLunga
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
        public byte[] DataArrayV4
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
                FunzioniComuni.SplitSShort((short)(_sblm.AhCaricati ), ref _byte1, ref _byte2);
                _datamap[_arrayInit++] = _byte1;
                _datamap[_arrayInit++] = _byte2;

                // Wh Caricati
                FunzioniComuni.SplitSInt32(_sblm.WhCaricati, ref _byte1, ref _byte2, ref _byte3, ref _byte4,3);
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

                return _datamap;
            }

        }



        public bool caricaDati(int idLocale)
        {
            try
            {
                // carico i dati testata lungo da db
                _sblm = _caricaDati(idLocale);
                if (_sblm == null)
                {
                    _sblm = new _sbMemLunga();
                    return false;
                }
                else
                {
                    // se la testata è salvata, carico i brevi
                    CaricaProgramma();
                    CaricaBrevi();

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

        public bool caricaDati(string IdApparato, uint IdMemoriaLunga)
        {
            try
            {
                _sblm = _caricaDati(IdApparato, IdMemoriaLunga);
                if (_sblm == null)
                {
                    _sblm = new _sbMemLunga();
                    _sblm.IdApparato = IdApparato;
                    _sblm.IdMemoriaLunga = IdMemoriaLunga;
                    //azzero sempre il puntatore a breve, la lettura la faccio dopo aver letto il puntatore dall'apparato
                    PuntatorePrimoBreveEff = 0;
                    return false;
                }
                else
                {
                    CaricaProgramma();
                    CaricaBrevi();
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
                if (_sblm.IdApparato != nullID & _sblm.IdApparato != null & _sblm.IdMemoriaLunga != null)
                {

                    _sbMemLunga _TestDati = _caricaDati(_sblm.IdApparato, _sblm.IdMemoriaLunga);
                    if (_TestDati == null)
                    {
                        //nuovo record
                        _sblm.CreationDate = DateTime.Now;
                        _sblm.RevisionDate = DateTime.Now;

                        int _result = _database.Insert(_sblm);
                        _datiSalvati = true;
                    }
                    else
                    {
                        _sblm.IdLocale = _TestDati.IdLocale;
                        _sblm.RevisionDate = DateTime.Now;
                        int _result = _database.Update(_sblm);
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

        public bool CaricaBrevi() 
        {
            try 
            {

                CicliMemoriaBreve.Clear();
                if (_database == null)
                    return false;

                IEnumerable<_sbMemBreve> _TempCicli = _database.Query<_sbMemBreve>("select * from _sbMemBreve where IdApparato = ? and IdMemoriaLunga = ? order by IdMemoriaBreve ", _sblm.IdApparato, _sblm.IdMemoriaLunga);

                foreach (_sbMemBreve Elemento in _TempCicli)
                {
                    sbMemBreve _cLoc;
                    _cLoc = new sbMemBreve(Elemento);
                    CicliMemoriaBreve.Add(_cLoc);

                    /*
                    _cLoc = new sbMemBreve(_database);
                    if (_cLoc.caricaDati(Elemento.IdLocale))
                    {
                        CicliMemoriaBreve.Add(_cLoc);
                    }
                     */ 

                }


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

               
                int _result = _database.InsertAll( CicliMemBreveDB );
                Log.Debug("Brevi salvati su db");
                //CaricaBrevi();
                //Log.Warn("Brevi ricaricati");


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
                if (_database == null)
                    return false;
                SQLiteCommand CancellaCicli =  _database.CreateCommand("delete from _sbMemBreve where IdApparato = ? and IdMemoriaLunga = ? ", _sblm.IdApparato, _sblm.IdMemoriaLunga);
                int esito = CancellaCicli.ExecuteNonQuery();
                CaricaBrevi() ;

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
                SQLiteCommand CancellaRecord = _database.CreateCommand("delete from _sbMemLunga where IdApparato = ? and IdMemoriaLunga = ? ", _sblm.IdApparato, _sblm.IdMemoriaLunga);
                int esito = CancellaRecord.ExecuteNonQuery();
                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("salvaDati: " + Ex.Message + " -> " + Ex.TargetSite.ToString());
                return false;
            }
        }

        /// <summary>
        /// Calcola i valori intermedi per i cicli brevi.
        /// 
        /// I dati sono calcolati come centesimi di Volt gestiti in interi lunghi senza segno
        /// </summary>
        /// <param name="ConsolidaDati">se settato a <c>true</c> registra sul lungo il tempo mancanza elettrolita e il massimo sbilanciamento </param>
        /// <returns></returns>
        public bool CalcolaIntermediBrevi(bool ConsolidaDati = true, double SogliaMaxSbil = 5.0 )
        {
            try
            {
                StruttureBase.ArrayCelle _celleRelative = new StruttureBase.ArrayCelle();
                if(ProgrammaAttivo != null)
                   _celleRelative = FunzioniMR.CalcolaCelleRelative(ProgrammaAttivo.CelleSensori);


                float _tmpVMaxSbilanciamento = 0;
                float _tmpSbil;
                UInt32 _tmpDurataMancanzaElettrolita = 0;
                UInt32 _tmpDurataOverMaxSbil = 0;
                UInt32 _tmpDurataOverTemp = 0;

                int _tSogliaTemp = 55; // Soglia Temperatura; al momento SEMPRE 55 °C


                foreach (sbMemBreve Elemento in CicliMemoriaBreve)
                {

                    Elemento.ValoriIntermedi = new tensioniIntermedie();
                    // Se il 'ProgrammaAttivo' non è caricato, provo a caricarlo
                    if (ProgrammaAttivo != null)
                        if (ProgrammaAttivo.BatteryCells < 1)
                            CaricaProgramma();
                            //Completo con valori cella, se il programma è definito
                            if (ProgrammaAttivo != null)
                    {
                        StruttureBase.ArrayTensioniUS _TensioniCiclo = new StruttureBase.ArrayTensioniUS();
                        _TensioniCiclo.V1 = (ushort)Elemento.V1;
                        _TensioniCiclo.V2 = (ushort)Elemento.V2;
                        _TensioniCiclo.V3 = (ushort)Elemento.V3;
                        _TensioniCiclo.Vbatt = (ushort)Elemento.Vreg;
                        Elemento.ValoriIntermedi.CalcolaIntermedie(_TensioniCiclo, ProgrammaAttivo.CelleSensori, _celleRelative);
                       
                        // se previsto calcolo massimo sbilanciamento e presenza elettrolita

                        if (ConsolidaDati == true)
                        {
                            if (Elemento.PresenzaElettrolita == 0x0F) _tmpDurataMancanzaElettrolita += DurataBreve;
                            _tmpSbil = Elemento.ValoriIntermedi.MassimoSbilanciamento(Elemento.ValoriIntermedi.TensioniCellaRelative);
                            if (_tmpVMaxSbilanciamento < _tmpSbil) _tmpVMaxSbilanciamento = _tmpSbil;
                            Elemento.Vc1 = Elemento.ValoriIntermedi.TensioniCellaAssolute.V1;
                            Elemento.Vc2 = Elemento.ValoriIntermedi.TensioniCellaAssolute.V2;
                            Elemento.Vc3 = Elemento.ValoriIntermedi.TensioniCellaAssolute.V3;
                            Elemento.VcBatt = Elemento.ValoriIntermedi.TensioniCellaAssolute.Vbatt;
                            Elemento.Vcs1 = Elemento.ValoriIntermedi.TensioniCellaRelative.V1;
                            Elemento.Vcs2 = Elemento.ValoriIntermedi.TensioniCellaRelative.V2;
                            Elemento.Vcs3 = Elemento.ValoriIntermedi.TensioniCellaRelative.V3;
                            Elemento.VcsBatt = Elemento.ValoriIntermedi.TensioniCellaRelative.Vbatt;
                            Elemento.MaxSbil = _tmpSbil;

                            // Se _tmpSbil (massimo sbilanciamento calcolato)  supera la soglia max, conteggio il tempo
                            if (_tmpSbil > SogliaMaxSbil) _tmpDurataOverMaxSbil += DurataBreve;
                            if ((Elemento.Tntc > _tSogliaTemp) && (Elemento.Tntc < 150 )) _tmpDurataOverTemp += DurataBreve;


                        }
                    }


                }

                if (ConsolidaDati == true)
                {
                    _sblm.DurataMancanzaElettrolita = _tmpDurataMancanzaElettrolita;
                    _sblm.VMaxSbilanciamentoC = _tmpVMaxSbilanciamento;
                    _sblm.DurataSbilanciamento = _tmpDurataOverMaxSbil;
                    _sblm.DurataOverTemp = _tmpDurataOverTemp;
                    _datiSalvati = false;
                }

                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaBrevi " + Ex.Message);
                return false;
            }
        }

        public bool CaricaProgramma()
        {
            try
            {

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

                return _esito;

            }
            catch (Exception Ex)
            {
                Log.Error("CaricaBrevi " + Ex.Message);
                return false;
            }
        }

        public bool InvertiVersoCorrentiMB(elementiComuni.VersoCorrente Verso)
        {
            try
            {
                bool _esito = true;
                foreach (sbMemBreve _ciclo in CicliMemoriaBreve)
                {
                    _ciclo.VersoScarica = Verso;
                }


                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("ScriviProgramma: " + Ex.Message);
                return false;
            }
        }

        #region Class Parameters

        [OLVColumn( IsVisible= false)]
        public int IdLocale
        {
            get { return _sblm.IdLocale; }
            set
            {
                if (value !=null )
                {
                    _sblm.IdLocale = value;
                    _datiSalvati = false;
                }
            }
        }

        [OLVColumn(IsVisible = true, Width = 100, DisplayIndex = 1, TextAlign = HorizontalAlignment.Right)]
        public uint IdMemoriaLunga
        {
            get { return _sblm.IdMemoriaLunga; }
            set
            {
                if (value != null)
                {
                    _sblm.IdMemoriaLunga = value;
                    _datiSalvati = false;
                }
            }
        }

        [OLVColumn(IsVisible = false)]
        public DateTime CreationDate
        {
            get { return _sblm.CreationDate; }
        }

        [OLVColumn( IsVisible= false)]
        public DateTime RevisionDate
        {
            get { return _sblm.RevisionDate; }
        }

        [OLVColumn(IsVisible = false)]
        public string LastUser
        {
            get { return _sblm.LastUser; }
        }

        public byte TipoEvento
        {
            get { return _sblm.TipoEvento; }
            set
            {
                _sblm.TipoEvento = value ;
                _datiSalvati = false ;
            }
        }

        public ushort IdProgramma
        {
            get { return _sblm.IdProgramma; }
            set
            {
                _sblm.IdProgramma = value;
                _datiSalvati = false;
            }
        }
        public string strIdProgramma
        {
            get { return _sblm.IdProgramma.ToString(); }

        }

        public UInt32 PuntatorePrimoBreve
        {
            get { return _sblm.PuntatorePrimoBreve; }
            set
            {
                _sblm.PuntatorePrimoBreve = value;
                _datiSalvati = false;
            }
        }

        public int NumEventiBrevi
        {
            get { return _sblm.NumEventiBrevi; }
            set
            {
                _sblm.NumEventiBrevi = value;
                _datiSalvati = false;
            }
        }

        public int NumEventiBreviCaricati
        {
            get {
                if (CicliMemoriaBreve != null)
                {
                    return CicliMemoriaBreve.Count();
                }
                else
                {
                    return 0;
                }
            }
        }

        public double PercEventiBreviCaricati
        {
            get
            {
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
            }
        }


        public string strNumEventiBrevi
        {
            get { return NumEventiBreviCaricati.ToString() + " / " + NumEventiBrevi.ToString(); }
        }

        public string DataOraStart
        {
            get { return _sblm.DataOraStart; }
            set
            {
                _sblm.DataOraStart = value;
                _datiSalvati = false;
            }
        }

        public DateTime dtDataOraStart
        {
            get
            {
                DateTime _dataora;


                if (_sblm.DataOraStart.Length != 15)
                {
                    // data non formattata correttamente
                    return DateTime.Now;
                }

                //if (DateTime.TryParseExact(_sbsm.DataOraRegistrazione,"dd/MM/yy HH:mm",CultureInfo.InvariantCulture,DateTimeStyles.None,out _dataora))
                if (DateTime.TryParse(_sblm.DataOraStart, out _dataora))
                {
                    return _dataora;
                }


                return DateTime.Now;

            }
        }

        public string DataOraFine
        {
            get { return _sblm.DataOraFine; }
            set
            {
                _sblm.DataOraFine = value;
                _datiSalvati = false;
            }
        }

        public DateTime dtDataOraFine
        {
            get
            {
                DateTime _dataora;


                if (_sblm.DataOraFine.Length != 15)
                {
                    // data non formattata correttamente
                    return DateTime.Now;
                }

                //if (DateTime.TryParseExact(_sbsm.DataOraRegistrazione,"dd/MM/yy HH:mm",CultureInfo.InvariantCulture,DateTimeStyles.None,out _dataora))
                if (DateTime.TryParse(_sblm.DataOraFine, out _dataora))
                {
                    return _dataora;
                }


                return DateTime.Now;

            }
        }

        public UInt32 Durata
        {
            get { return _sblm.Durata; }
            set
            {
                _sblm.Durata = value;
                _datiSalvati = false;
            }
        }

        public int TempMin
        {
            get
            {
                sbyte _tempTmin = (sbyte)_sblm.TempMin;

                return _tempTmin;
            }
            set
            {
                _sblm.TempMin = value;
                _datiSalvati = false;
            }
        }

        public string strTempMin
        {
            get
            {
                string _stringaTemp;
                sbyte _tempTmin = (sbyte)_sblm.TempMin;

                _stringaTemp = FunzioniMR.StringaTemperatura(_tempTmin) ;

                if (LivelloUser < 1)
                {
                    return _stringaTemp;
                }
                else
                {
                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return _stringaTemp;

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }

            }

        }

        public int TempMax
        {
            get
            {
                sbyte _tempTmax = (sbyte)_sblm.TempMax;

                return _tempTmax;
            }
            set
            {
                _sblm.TempMax = value;
                _datiSalvati = false;
            }
        }

        public string strTempMax
        {
            get
            {
                string _stringaTemp;
                sbyte _tempTmax = (sbyte)_sblm.TempMax;

                _stringaTemp = FunzioniMR.StringaTemperatura(_tempTmax);

                if (LivelloUser < 1)
                {
                    return _stringaTemp;
                }
                else
                {
                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return _stringaTemp;

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }

            }


        }
        
        public UInt32 Vmin
        {
            get { return _sblm.Vmin; }
            set
            {
                _sblm.Vmin = value;
                _datiSalvati = false;
            }
        }

        public string strVmin
        {
            get
            {
                if (LivelloUser < 1)
                {
                    return FunzioniMR.StringaTensione(_sblm.Vmin);
                }
                else
                {


                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return FunzioniMR.StringaTensione(_sblm.Vmin);

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }
               
            }
        }

        public UInt32 Vmax
        {
            get { return _sblm.Vmax; }
            set
            {
                _sblm.Vmax = value;
                _datiSalvati = false;
            }
        }

        public string strVmax
        {
            get
            {
                if (LivelloUser < 1)
                {
                    return FunzioniMR.StringaTensione(_sblm.Vmax);
                }
                else
                {

                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return FunzioniMR.StringaTensione(_sblm.Vmax);

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }
                //return FunzioniMR.StringaTensione(_sblm.Vmax);
            }

        }



        public float VCellMin
        {
            get
            {
                float _vCellMin = (float)_sblm.Vmin;

                if( ProgrammaAttivo != null )
                {
                    if(ProgrammaAttivo.BatteryCells > 0 )
                    {
                        _vCellMin = (float)_sblm.Vmin / (float)ProgrammaAttivo.BatteryCells;
                    }
                   
                }
                return _vCellMin;
            }
        }

        public string strVCellMin
        {
            get
            {
                string _valTensione;

                //return FunzioniMR.StringaTensione(_sblm.Vmin);
                if (ProgrammaAttivo != null)
                {
                    _valTensione = FunzioniMR.StringaTensionePerCella(_sblm.Vmin, ProgrammaAttivo.BatteryCells);
                }
                else
                {
                    _valTensione =  FunzioniMR.StringaTensionePerCella(_sblm.Vmin, 1);
                }

                if (LivelloUser < 1)
                {
                    //return FunzioniMR.StringaTensione(_sblm.Vmin);
                    return _valTensione;
                }
                else
                {


                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return _valTensione;

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }

            }
        }


        public float VCellMax
        {
            get
            {
                float _vCellMax = (float)_sblm.Vmax;

                if (ProgrammaAttivo != null)
                {
                    if (ProgrammaAttivo.BatteryCells > 0)
                    {
                        _vCellMax = (float)_sblm.Vmin / (float)ProgrammaAttivo.BatteryCells;
                    }

                }
                return _vCellMax;
            }
        }

        public string strVCellMax
        {
            get
            {
                string _valTensione;

                //return FunzioniMR.StringaTensione(_sblm.Vmax);
                if (ProgrammaAttivo != null)
                {
                    _valTensione = FunzioniMR.StringaTensionePerCella(_sblm.Vmax, ProgrammaAttivo.BatteryCells);
                }
                else
                {
                    _valTensione = FunzioniMR.StringaTensionePerCella(_sblm.Vmax, 1);
                }

                if (LivelloUser < 1)
                {
                    //return FunzioniMR.StringaTensione(_sblm.Vmax);
                    return _valTensione;
                }
                else
                {


                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return _valTensione;

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }

            }
        }








        public short Amin
        {
            get { return _sblm.Amin; }
            set
            {
                _sblm.Amin = value;
                _datiSalvati = false;
            }
        }
        public string strAmin
        {
            get { return FunzioniMR.StringaCorrente(_sblm.Amin); }
        }
        public string olvAmin
        {
            get
            {
                string _valore = "";
                if (VersoScarica == elementiComuni.VersoCorrente.Diretto || _sblm.TipoEvento == 0xF0)
                {
                    _valore = FunzioniMR.StringaCorrenteOLV((short)_sblm.Amin);
                }
                else
                {
                    _valore = FunzioniMR.StringaCorrenteOLV((short)-_sblm.Amin);
                }
                if (LivelloUser < 1)
                {
                    return _valore;
                }
                else
                {

                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return _valore;

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }

            }





        }

        public string strAmax
        {
            get { return FunzioniMR.StringaCorrente(_sblm.Amax); }
        }
        public string olvAmax
        {
            get
            {
                string _valore = "";
                if (VersoScarica == elementiComuni.VersoCorrente.Diretto || _sblm.TipoEvento == 0xF0)
                {
                    _valore = FunzioniMR.StringaCorrenteOLV((short)_sblm.Amax);
                }
                else
                {
                    _valore = FunzioniMR.StringaCorrenteOLV((short)-_sblm.Amin);
                }

                if (LivelloUser < 1)
                {
                    return _valore;
                }
                else
                {

                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return _valore;

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }

            }
        }

        public short Amax
        {
            get { return _sblm.Amax; }
            set
            {
                _sblm.Amax = value;
                _datiSalvati = false;
            }
        }

        public byte PresenzaElettrolita
        {
            get { return _sblm.PresenzaElettrolita; }
            set
            {
                _sblm.PresenzaElettrolita = value;
                _datiSalvati = false;
            }
        }

        public string strPresenzaElettrolita
        {
            get
            {
                if (LivelloUser < 1)
                {
                    return FunzioniMR.StringaPresenza(_sblm.PresenzaElettrolita);
                }
                else
                {


                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return FunzioniMR.StringaPresenza(_sblm.PresenzaElettrolita);

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }
            }

        }


        public int Ah
        {
            get { return _sblm.Ah; }
            set
            {
                _sblm.Ah = value;
                _datiSalvati = false;
            }
        }

        /*
        // Visualizzazione Correnti
        public int DivisoreCorrente = 10;
        public int DecimaliCorrente = 1;
        public int DecimaliPotenza = 1;
        */


        public string strAh
        {

            get
            {
                if (LivelloUser < 1)
                {
                    return FunzioniMR.StringaCapacita(_sblm.Ah, DivisoreCorrente, DecimaliCorrente);
                }
                else
                {
                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return FunzioniMR.StringaCapacita(_sblm.Ah, DivisoreCorrente, DecimaliCorrente);

                        case 0xAA: // "Pausa
           
                            return "";
                        default:
                            return "";
                    }
                }

            }
        }

        public float ValAh
        {
            get { return FunzioniMR.ValoreEffettivo(_sblm.Ah, DivisoreCorrente); }
        }


        public int AhCaricati
        {
            get { return _sblm.AhCaricati; }
            set
            {
                _sblm.AhCaricati = value;
                _datiSalvati = false;
            }
        }

        public string strAhCaricati
        {
            get
            {
                if (LivelloUser < 1)
                {
                    return FunzioniMR.StringaCapacita(_sblm.AhCaricati, DivisoreCorrente, DecimaliCorrente);
                }
                else
                {
                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return FunzioniMR.StringaCapacita(_sblm.AhCaricati, DivisoreCorrente, DecimaliCorrente);

                        case 0xAA: // "Pausa
                            
                            return "";
                        default:
                            return "";
                    }
                    //return FunzioniMR.StringaTensione(_sblm.Vmin);
                }
            }
        }

        public float ValAhCaricati
        {
            get { return FunzioniMR.ValoreEffettivo(_sblm.AhCaricati, DivisoreCorrente); }
        }

        public int AhScaricati
        {
            get { return _sblm.AhScaricati; }
            set
            {
                _sblm.AhScaricati = value;
                _datiSalvati = false;
            }
        }

        public string strAhScaricati
        {

            get
            {
                if (LivelloUser < 1)
                {
                    return FunzioniMR.StringaCapacita(_sblm.AhScaricati, DivisoreCorrente, DecimaliCorrente);
                }
                else
                {
                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return FunzioniMR.StringaCapacita(_sblm.AhScaricati, DivisoreCorrente, DecimaliCorrente);

                        case 0xAA: // "Pausa
                            
                            return "";
                        default:
                            return "";
                    }
                }
  
            }
        }


        public float ValAhScaricati
        {
            get { return FunzioniMR.ValoreEffettivo(_sblm.AhScaricati, DivisoreCorrente); }
        }

        public Int32 Wh
        {
            get { return _sblm.Wh; }
            set
            {
                _sblm.Wh = value;
                _datiSalvati = false;
            }
        }

        public string strKWh
        {
            get
            {
                if (LivelloUser < 1)
                {
                    return FunzioniMR.StringaPotenza(_sblm.Wh, DivisorePotenza, DecimaliPotenza);
                }
                else
                {
                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return FunzioniMR.StringaPotenza(_sblm.Wh, DivisorePotenza, DecimaliPotenza);

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }
             
            }
        }

        public float ValKWh
        {
            get { return FunzioniMR.ValoreEffettivo(_sblm.Wh, DivisorePotenza); }
        }

        public Int32 WhCaricati
        {
            get { return _sblm.WhCaricati; }
            set
            {
                _sblm.WhCaricati = value;
                _datiSalvati = false;
            }
        }

        public string strKWhCaricati
        {
            get
            {
                if (LivelloUser < 1)
                {
                    return FunzioniMR.StringaPotenza(_sblm.WhCaricati, DivisorePotenza, DecimaliPotenza);
                }
                else
                {
                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return FunzioniMR.StringaPotenza(_sblm.WhCaricati, DivisorePotenza, DecimaliPotenza);

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }
                //return FunzioniMR.StringaTensione(_sblm.Vmin);
            }
        }

        public float ValKWhCaricati
        {
            get { return FunzioniMR.ValoreEffettivo(_sblm.WhCaricati, DivisorePotenza); }
        }

        public Int32 WhScaricati
        {
            get { return _sblm.WhScaricati; }
            set
            {
                _sblm.WhScaricati = value;
                _datiSalvati = false;
            }
        }

        public string strKWhScaricati
        {
            get
            {
                if (LivelloUser < 1)
                {
                    return FunzioniMR.StringaPotenza(_sblm.WhScaricati, DivisorePotenza, DecimaliPotenza);
                }
                else
                {
                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return FunzioniMR.StringaPotenza(_sblm.WhScaricati, DivisorePotenza, DecimaliPotenza);

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }
                //return FunzioniMR.StringaTensione(_sblm.Vmin);
            }
        }

        public float ValKWhScaricati
        {
            get { return FunzioniMR.ValoreEffettivo(_sblm.WhScaricati, DivisorePotenza); }
        }

        public byte CondizioneStop
        {
            get { return _sblm.CondizioneStop; }
            set
            {
                _sblm.CondizioneStop = value;
                _datiSalvati = false;
            }

        }

        public string strCondizioneStop
        {
            get
            {
                if (LivelloUser < 1)
                {
                    return _sblm.CondizioneStop.ToString("X2");
                }
                else
                {
                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                        case 0x0F: // "Scarica"
                            return _sblm.CondizioneStop.ToString("X2");

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }
                //return FunzioniMR.StringaTensione(_sblm.Vmin);
            }
        }

        public byte FattoreCarica
        {
            get { return _sblm.FattoreCarica; }
            set
            {
                _sblm.FattoreCarica = value;
                _datiSalvati = false;
            }

        }
        public string strFattoreCarica
        {
            get { return FunzioniMR.StringaFattoreCarica(_sblm.FattoreCarica); }
        }


        public byte StatoCarica
        {
            get { return _sblm.StatoCatica; }
            set
            {
                _sblm.StatoCatica = value;
                _datiSalvati = false;
            }
        }
        public string strStatoCarica
        {
            get { return FunzioniMR.StringaSoC(_sblm.StatoCatica); }
        }

        /*--------------------------------------------------------------------------------------------*/

        public string strStatoCaricaEff
        {
            get { return FunzioniMR.StringaSoC(StatoCaricaEff); }
        }

        public string strLivelloIniziale
        {

            get
            {

                    return FunzioniMR.StringaCapacita(LivelloIniziale, DivisoreCorrente, DecimaliCorrente);
 
            }
        }

        public string strLivelloFinale
        {

            get
            {

                return FunzioniMR.StringaCapacita(LivelloFinale, DivisoreCorrente, DecimaliCorrente);

            }
        }
        /*--------------------------------------------------------------------------------------------*/


        public int TipoCariatore
        {
            get { return _sblm.TipoCariatore; }
            set
            {
                _sblm.TipoCariatore = value;
                _datiSalvati = false;
            }
        }

        public UInt32 IdCaricatore
        {
            get { return _sblm.IdCaricatore; }
            set
            {
                _sblm.IdCaricatore = value;
                _datiSalvati = false;
            }
        }



        public float VMaxSbilanciamentoC
        {
            get { return _sblm.VMaxSbilanciamentoC; }
            set
            {
                _sblm.VMaxSbilanciamentoC = value;
                _datiSalvati = false;
            }
        }

        public float EffMaxSbilanciamentoC
        {
            get { return (_sblm.VMaxSbilanciamentoC / 100); }
        } 

        public string  strVMaxSbilanciamentoC
        {
            get
            {
                if (LivelloUser < 1)
                {
                    return FunzioniMR.StringaTensioneCella(_sblm.VMaxSbilanciamentoC);
                }
                else
                {
                    switch (_sblm.TipoEvento)
                    {
                        case 0xF0: // "Carica"
                            return "";
                        case 0x0F: // "Scarica"
                            return FunzioniMR.StringaTensioneCella(_sblm.VMaxSbilanciamentoC);

                        case 0xAA: // "Pausa
                            return "";
                        default:
                            return "";
                    }
                }
                //return FunzioniMR.StringaTensione(_sblm.Vmin);
            }

        }



        public UInt32 DurataSbilCelle
        {
            get { return _sblm.DurataSbilanciamento; }
            set
            {
                _sblm.DurataSbilanciamento = value;
                _datiSalvati = false;
            }
        }

        public UInt32 DurataMancanzaElettrolita
        {
            get { return _sblm.DurataMancanzaElettrolita; }
            set
            {
                _sblm.DurataMancanzaElettrolita = value;
                _datiSalvati = false;
            }
        }

        public UInt32 DurataOverTemp
        {
            get { return _sblm.DurataOverTemp; }
            set
            {
                _sblm.DurataOverTemp = value;
                _datiSalvati = false;
            }
        }


        public DateTime DataLastDownload
        {
            get { return _sblm.DataLastDownload; }
            set
            {
                _sblm.DataLastDownload = value;
                _datiSalvati = false;
            }
        }



        #endregion Class Parameter


        public string TensioniCiclo()
        {
            try
            {
                string _tensioni = "";
                float _inVolt;
                _inVolt = Vmin / 100;
                _tensioni = _inVolt.ToString("0.0");
                _inVolt = Vmax / 100;
                _tensioni += "/" + _inVolt.ToString("0.0");
                return _tensioni;
            }
            catch
            {
                return "";
            }
        }

    }


}
