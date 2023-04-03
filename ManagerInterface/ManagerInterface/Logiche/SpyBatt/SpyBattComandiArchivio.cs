using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
// using System.Windows.Forms;

using SQLite;
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


        public int CreaClone() //string IdApparato, MoriData._db dbCorrente, bool ApparatoConnesso = false)
        {
            try
            {
                int _esito;

                // 1) genero il clone della testata:
                //    se vecchio record mai gestito creo il promo clone e comincio a salvare. il clone è sempre una immagine statica; 
                //    poi lavoro sempre sul record base (ID puro)

                // 1.1

                _spybatt _testataClone ;
                string _nuovoId;

                if( sbData.GeneraClone(out _nuovoId, out _testataClone,true))
                {
                    // La testata è clonata; posso clonare gli altri dati

                    // 1. Dati cliente
                    sbCliente.ClonaRecord(_nuovoId);

                    // 2.Programmazioni
                    foreach (sbProgrammaRicarica _Prog in Programmazioni)
                    {
                        _Prog.ClonaRecord(_nuovoId);
                    }

                    // 3.Record Lunghi
                    foreach (sbMemLunga _Lunga in CicliMemoriaLunga)
                    {
                        _Lunga.ClonaRecord(_nuovoId);
                    }

                }


                _esito = _testataClone.NumeroClone;
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("SB.CreaClone: " + Ex.Message);
                Log.Error(Ex.TargetSite.ToString());
                return 0;
            }

        }




    }
}
