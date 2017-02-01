using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

using SQLite.Net;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;
using System.Drawing;

namespace ChargerLogic
{
    public class StrutturaImpianto
    {
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public ImageList ListaIcone;

        public StrutturaImpianto()
        {

        }

        public StrutturaImpianto(MoriData._db DbConnection)
        {
            _database = DbConnection;
        }

        public ArrayList RadiceStruttura()
        {
            ArrayList _tempList = new ArrayList();
            try
            {
                if(_database == null)
                    return _tempList;

                var _dati = from s in _database.Table<_NodoStruttura>()
                            where s.ParentGuid == NodoStruttura.GuidBASE
                            orderby s.Nome
                            select s;


                foreach (_NodoStruttura item in _dati)
                {
                    NodoStruttura Nodo = new NodoStruttura(item);
                    Nodo._database = _database;
                    _tempList.Add(Nodo);
                }

                return _tempList;
            }
            catch (Exception Ex)
            {
                Log.Error("RadiceStruttura: " + Ex.Message);
                return _tempList;
               

            }
        }





        public ArrayList FigliNodo( string NodeGUID)
        {
            ArrayList _tempList = new ArrayList();
            try
            {
                if (_database == null)
                    return _tempList;

                var _dati = from s in _database.Table<_NodoStruttura>()
                            where s.ParentGuid == NodeGUID
                            orderby s.Nome
                            select s;


                foreach (_NodoStruttura item in _dati)
                {
                    NodoStruttura Nodo = new NodoStruttura(item);
                    Nodo._database = _database;
                    _tempList.Add(Nodo);
                }

                return _tempList;
            }
            catch (Exception Ex)
            {
                Log.Error("RadiceStruttura: " + Ex.Message);
                return _tempList;


            }
        }

        public bool CancellaNodo(string NodeGUID)
        {

            try
            {
                if (_database == null)
                    return false;

                SQLiteCommand CancellaRecord = _database.CreateCommand("delete from _NodoStruttura where Guid = ? ", NodeGUID);
                int esito = CancellaRecord.ExecuteNonQuery();
                return true;

            }
            catch (Exception Ex)
            {
                Log.Error("RadiceStruttura: " + Ex.Message);
                return false;

            }
        }

        public bool CaricaIcone()
        {
            try
            {
                ListaIcone = new ImageList();
                ListaIcone.ImageSize = new Size(24, 24);
                ListaIcone.Images.Add(PannelloCharger.Properties.Resources.home);
                ListaIcone.Images.Add(PannelloCharger.Properties.Resources.folder);
                ListaIcone.Images.Add(PannelloCharger.Properties.Resources.plant);
                ListaIcone.Images.Add(PannelloCharger.Properties.Resources.ico_SpyBatt);

                return true;
            }

            catch (Exception Ex)
            {
                Log.Error("CaricaIcone: " + Ex.Message);
                return false;


            }
        }

        public int IndiceIcona(string NomeIco)
        {
            try
            {
                int _esito = 0;
                if (ListaIcone == null)
                    return 0;
                switch (NomeIco)
                {
                    case "home":
                    case "cloud":
                        _esito = 0;
                        break;
                    case "folder":
                        _esito = 1;
                        break;
                    case "plant":
                        _esito = 2;
                        break;
                    case "spy-batt":
                        _esito = 3;
                        break;
                    default:
                        break;
                }

                return _esito;
            }

            catch (Exception Ex)
            {
                Log.Error("CaricaIcone: " + Ex.Message);
                return 0;


            }
        }


        public bool CercaOrfani()
        {
            try
            {
                // Prima carico gli SpyBatt Orfani

                string _sql = "";
                _sql += " select t1.Id, t1.SwVersion, t1.ProductId, t1.HwVersion, t2.DataInstall, t2.Client, t2.BatteryBrand, t2.BatteryModel, t2.BatteryId, t2.ClientNote,t2.SerialNumber ";
                _sql += " from _spybatt as t1 left outer join _sbDatiCliente as t2 on t1.Id = t2.IdApparato order by t2.ClientNote,t1.Id ";
                _sql += " and t1.Id not in ( select IdApparato from _NodoStruttura) ";
                Log.Info(_sql);

                List<dataUtility.sbListaElementi> _resultSet = _database.Query<dataUtility.sbListaElementi>(_sql);

                foreach (dataUtility.sbListaElementi item in _resultSet)
                {

                    _NodoStruttura _tempnodo = new _NodoStruttura();
                    Guid _GuidFoglia = Guid.NewGuid();
                    _tempnodo.Guid = _GuidFoglia.ToString();
                    _tempnodo.Tipo = (byte)NodoStruttura.TipoNodo.FogliaSB;
                    _tempnodo.Level = 2;
                    //_tempnodo.ParentIdLocale = _tempnodo.IdLocale;
                    _tempnodo.ParentGuid = NodoStruttura.GuidUNDEF;

                    _tempnodo.Nome = item.BatteryId;
                    _tempnodo.Descrizione = item.ClientNote;
                    _tempnodo.Icona = "spy-batt";
                    _tempnodo.IdApparato = item.Id;
                    _tempnodo.CreationDate = DateTime.Now;
                    _tempnodo.RevisionDate = DateTime.Now;
                    int _result = _database.Insert(_tempnodo);
                }

                return true;
            }

            catch (Exception ex)
            {
                Log.Error("NodoRoot: " + ex.ToString());
                return false;
            }
        }





        private _NodoStruttura NodoRoot()
        {
            try
            {
                _NodoStruttura _tempnodo = new _NodoStruttura();
                _tempnodo.IdLocale = 1;
                _tempnodo.Guid = NodoStruttura.GuidROOT;
                _tempnodo.Tipo = (byte)NodoStruttura.TipoNodo.Radice;
                _tempnodo.Level = 0;
                _tempnodo.ParentIdLocale = _tempnodo.IdLocale;
                _tempnodo.ParentGuid = NodoStruttura.GuidBASE;

                _tempnodo.Nome = "Questo PC";
                _tempnodo.Descrizione = "Radice struttura archivio";
                _tempnodo.Icona = "root";
                _tempnodo.IdApparato = null;

                return _tempnodo;

            }

            catch (Exception ex)
            {
                Log.Error("NodoRoot: " + ex.ToString());
                return null;
            }
        }


    }
}
