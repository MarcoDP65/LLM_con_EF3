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

namespace MoriData
{
    public class _sbTipoBatteria
    {
        [PrimaryKey]
        public byte BatteryTypeId { get; set; }

        public string BatteryType { get; set; }
        public byte SortOrder { get; set; }
        public byte StandardChargeProfile { get; set; }    // 0x00 IUIa / 0x01 IWa
        public byte Obsolete { get; set; }                 // if != 0 not usable for new setting

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
        #endregion

    }
}
