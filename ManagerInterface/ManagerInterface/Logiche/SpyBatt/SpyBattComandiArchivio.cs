using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
// using System.Windows.Forms;

using SQLite.Net;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;

namespace ChargerLogic
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UnitaSpyBatt
    {

        public bool CaricaCompleto(string IdApparato, MoriData._db dbCorrente, bool ApparatoConnesso = false)
        {
            try
            {
                bool _esito;
                bool _recordPresente;

                Log.Debug("CaricaCompleto ");
                if (ApparatoConnesso)
                    ControllaAttesa(UltimaScrittura);

                _esito = false;
                _recordPresente = sbData.caricaDati(IdApparato);

                if (_recordPresente)
                {
                    _idCorrente = IdApparato;
                    _recordPresente = sbCliente.caricaDati(IdApparato, 1);
                    _recordPresente = CaricaProgrammazioni(IdApparato, dbCorrente);
                    _recordPresente = CaricaCicliMemLunga(IdApparato, dbCorrente);

                    _esito = true;
                }
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCompleto: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }

        }


        public bool CreaClone(string IdApparato, MoriData._db dbCorrente, bool ApparatoConnesso = false)
        {
            try
            {
                bool _esito;
                bool _recordPresente;

                // 1) genero il clone della testata:
                //    se vecchio record mai gestito creo il promo clone e comincio a salvare. il clone è sempre una immagine statica; 
                //    poi lavoro sempre sul record base (ID puro)

                // 1.1
                


                Log.Debug("CaricaCompleto ");
                if (ApparatoConnesso)
                    ControllaAttesa(UltimaScrittura);

                _esito = false;
                _recordPresente = sbData.caricaDati(IdApparato);

                if (_recordPresente)
                {
                    _idCorrente = IdApparato;
                    _recordPresente = sbCliente.caricaDati(IdApparato, 1);
                    _recordPresente = CaricaProgrammazioni(IdApparato, dbCorrente);
                    _recordPresente = CaricaCicliMemLunga(IdApparato, dbCorrente);

                    _esito = true;
                }
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaCompleto: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return false;
            }

        }




    }
}
