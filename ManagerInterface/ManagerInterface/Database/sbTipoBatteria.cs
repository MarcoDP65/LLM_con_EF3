using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using log4net;
using log4net.Config;

using Utility;

namespace MoriData
{
    public class _sbTipoBatteria
    {
        [PrimaryKey]
        public byte BatteryTypeId { get; set; }

        public string BatteryType { get; set; }
        public byte SortOrder { get; set; }
        public byte StandardChargeProfile { get; set; }       // 0x00 IUIa / 0x01 IWa / 0x02 LI
        public byte Obsolete { get; set; }                    // if != 0 not usable for new setting
        public ushort VoltCella { get; set; }                 // in V/100
        public ushort VoltSoglia { get; set; }                // in V/100
        public ushort VCellaMin { get; set; }                 // in V/100
        public ushort VCellaMax { get; set; }                 // in V/100
        public ushort UsaSpybatt { get; set; }                // Valori: 0: NO , 1:SI
        public ushort AbilitaEqual { get; set; }              // Valori: 0: NO , 1:SI
        public ushort AbilitaAttesaBMS { get; set; }          // Valori: 0: NO , 1:SI
        public byte TensioniFisse { get; set; }               // Valori: 0: NO , 1:SI
        public ushort VminRiconoscimento { get; set; }        // in V/100
        public ushort VmaxRiconoscimento { get; set; }        // in V/100

        public byte DivisoreCelle { get; set; }               // se 0 cella singola

        public DateTime CreationDate { get; set; }
        public DateTime RevisionDate { get; set; }
        public string LastUser { get; set; }

    }

    public class sbTipoBatteria
    {
        
        public _sbTipoBatteria _sbTb = new _sbTipoBatteria();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public bool _datiSalvati;
        public bool _recordPresente;

        public sbTipoBatteria()
        {
            _sbTb = new _sbTipoBatteria();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        #region Class Parameters

        public byte BatteryTypeId
        {
            get { return _sbTb.BatteryTypeId; }
            set
            {

                _sbTb.BatteryTypeId = value;
                _datiSalvati = false;

            }
        }

        public string BatteryType
        {
            get { return _sbTb.BatteryType; }
            set
            {
                _sbTb.BatteryType = value;
                _datiSalvati = false;
            }
        }

        public byte SortOrder
        {
            get { return _sbTb.SortOrder; }
            set
            {

                _sbTb.SortOrder = value;
                _datiSalvati = false;

            }
        }

        public byte Obsolete
        {
            get { return _sbTb.Obsolete; }
            set
            {
                _sbTb.Obsolete = value;
                _datiSalvati = false;
            }
        }

        public byte StandardChargeProfile
        {
            get { return _sbTb.StandardChargeProfile; }
            set
            {
                _sbTb.StandardChargeProfile = value;
                _datiSalvati = false;
            }
        }

        public string BaseChargeProfile
        {
            get
            {

                if ((_sbTb.BatteryTypeId & 0x10) == 0x10)
                {
                    return "IWa";
                }
                else
                    return "IUIa";
            }

        }


        public ushort VoltCella
        {
            get { return _sbTb.VoltCella; }
            set { _sbTb.VoltCella = value; _datiSalvati = false; }
        }

        public ushort VminRiconoscimento
        {
            get { return _sbTb.VminRiconoscimento; }
            set { _sbTb.VminRiconoscimento = value; _datiSalvati = false; }
        }

        public ushort VmaxRiconoscimento
        {
            get { return _sbTb.VmaxRiconoscimento; }
            set { _sbTb.VmaxRiconoscimento = value; _datiSalvati = false; }
        }

        public ushort VoltSoglia
        {
            get { return _sbTb.VoltSoglia; }
            set { _sbTb.VoltSoglia = value; _datiSalvati = false; }
        }


        public ushort VCellaMin
        {
            get { return _sbTb.VCellaMin; }
            set { _sbTb.VCellaMin = value; _datiSalvati = false; }
        }

        public ushort VCellaMax
        {
            get { return _sbTb.VCellaMax; }
            set { _sbTb.VCellaMax = value; _datiSalvati = false; }
        }

        public ushort UsaSpybatt
        {
            get { return _sbTb.UsaSpybatt; }
            set { _sbTb.UsaSpybatt = value; _datiSalvati = false; }
        }

        public ushort AbilitaEqual
        {
            get { return _sbTb.AbilitaEqual; }
            set { _sbTb.AbilitaEqual = value; _datiSalvati = false; }
        }

        public ushort AbilitaAttesaBMS
        {
            get { return _sbTb.AbilitaAttesaBMS; }
            set { _sbTb.AbilitaAttesaBMS = value; _datiSalvati = false; }
        }

        public byte TensioniFisse
        {
            get { return _sbTb.TensioniFisse; }
            set { _sbTb.TensioniFisse = value; _datiSalvati = false; }
        }

        public byte DivisoreCelle
        {
            get { return _sbTb.DivisoreCelle; }
            set { _sbTb.DivisoreCelle = value; _datiSalvati = false; }
        }

        #endregion

    }
}
