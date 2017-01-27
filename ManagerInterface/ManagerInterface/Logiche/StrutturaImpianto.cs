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

        public bool CaricaIcone()
        {
            try
            {
                ListaIcone = new ImageList();
                ListaIcone.Images.Add(PannelloCharger.Properties.Resources.home);
                ListaIcone.Images.Add(PannelloCharger.Properties.Resources.folder);
                ListaIcone.Images.Add(PannelloCharger.Properties.Resources.plant);

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
