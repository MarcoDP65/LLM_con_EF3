//    class mbTipoBatteria

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
    public class _mbTipoBatteria
    {
        [PrimaryKey]
        public ushort BatteryTypeId { get; set; }

        public string BatteryType { get; set; }
        public byte idTecnologia { get; set; }
        //public byte idTecnologia { get; set; }

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

    public class mbTipoBatteria
    {

        public _mbTipoBatteria _mbTb = new _mbTipoBatteria();
        public bool valido;
        public MoriData._db _database;
        private static ILog Log = LogManager.GetLogger("mbTipoBatteria");
        public bool _datiSalvati;
        public bool _recordPresente;

        public mbTipoBatteria()
        {
            _mbTb = new _mbTipoBatteria();
            valido = false;
            _datiSalvati = true;
            _recordPresente = false;
        }

        #region Class Parameters

        public ushort BatteryTypeId
        {
            get { return _mbTb.BatteryTypeId; }
            set
            {

                _mbTb.BatteryTypeId = value;
                _datiSalvati = false;

            }
        }

        public string BatteryType
        {
            get { return _mbTb.BatteryType; }
            set
            {
                _mbTb.BatteryType = value;
                _datiSalvati = false;
            }
        }

        public byte SortOrder
        {
            get { return _mbTb.SortOrder; }
            set
            {

                _mbTb.SortOrder = value;
                _datiSalvati = false;

            }
        }

        public byte Obsolete
        {
            get { return _mbTb.Obsolete; }
            set
            {
                _mbTb.Obsolete = value;
                _datiSalvati = false;
            }
        }

        public byte StandardChargeProfile
        {
            get { return _mbTb.StandardChargeProfile; }
            set
            {
                _mbTb.StandardChargeProfile = value;
                _datiSalvati = false;
            }
        }

        public string BaseChargeProfile
        {
            get
            {

                if ((_mbTb.BatteryTypeId & 0x10) == 0x10)
                {
                    return "IWa";
                }
                else
                    return "IUIa";
            }

        }


        public ushort VoltCella
        {
            get { return _mbTb.VoltCella; }
            set { _mbTb.VoltCella = value; _datiSalvati = false; }
        }

        public ushort VminRiconoscimento
        {
            get { return _mbTb.VminRiconoscimento; }
            set { _mbTb.VminRiconoscimento = value; _datiSalvati = false; }
        }

        public ushort VmaxRiconoscimento
        {
            get { return _mbTb.VmaxRiconoscimento; }
            set { _mbTb.VmaxRiconoscimento = value; _datiSalvati = false; }
        }

        public ushort VoltSoglia
        {
            get { return _mbTb.VoltSoglia; }
            set { _mbTb.VoltSoglia = value; _datiSalvati = false; }
        }


        public ushort VCellaMin
        {
            get { return _mbTb.VCellaMin; }
            set { _mbTb.VCellaMin = value; _datiSalvati = false; }
        }

        public ushort VCellaMax
        {
            get { return _mbTb.VCellaMax; }
            set { _mbTb.VCellaMax = value; _datiSalvati = false; }
        }

        public ushort UsaSpybatt
        {
            get { return _mbTb.UsaSpybatt; }
            set { _mbTb.UsaSpybatt = value; _datiSalvati = false; }
        }

        public ushort AbilitaEqual
        {
            get { return _mbTb.AbilitaEqual; }
            set { _mbTb.AbilitaEqual = value; _datiSalvati = false; }
        }

        public ushort AbilitaAttesaBMS
        {
            get { return _mbTb.AbilitaAttesaBMS; }
            set { _mbTb.AbilitaAttesaBMS = value; _datiSalvati = false; }
        }

        public byte TensioniFisse
        {
            get { return _mbTb.TensioniFisse; }
            set { _mbTb.TensioniFisse = value; _datiSalvati = false; }
        }

        public byte DivisoreCelle
        {
            get { return _mbTb.DivisoreCelle; }
            set { _mbTb.DivisoreCelle = value; _datiSalvati = false; }
        }

        #endregion

    }
}

