using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using log4net;
using log4net.Config;
using ChargerLogic;

namespace MoriData
{
    public class dataUtility
    {


        public class sbListaElementi
        {
            [PrimaryKey]
            [MaxLength(24)]
            public string Id { get; set; }
            [MaxLength(20)]
            public string SwVersion { get; set; }
            [MaxLength(8)]
            public string ProductId { get; set; }
            [MaxLength(20)]
            public byte HwVersion { get; set; }
            public Int32 IdCliente { get; set; }   //Fisso a 1, al momento non è prevista cronologia
            public string DataInstall { get; set; }
            [MaxLength(120)]
            public string Client { get; set; }
            [MaxLength(120)]
            public string BatteryBrand { get; set; }
            [MaxLength(60)]
            public string BatteryModel { get; set; }
            [MaxLength(60)]
            public string BatteryId { get; set; }
            [MaxLength(120)]
            public string ClientNote { get; set; }
            [MaxLength(20)]
            public string SerialNumber { get; set; }
            public DateTime UltimaLettura { get; set; }

            public override string ToString()
            {
                return Id.ToString();
            }
            public string strUltimaLettura
            {
                get
                {
                    if (UltimaLettura == DateTime.MinValue)
                    {
                        return "";
                    }
                    else
                    {
                        return UltimaLettura.ToString("yyyy/MM/dd") + " " + UltimaLettura.ToShortTimeString();
                    }
                }
            }
        }


        public class llListaElementi
        {
            [PrimaryKey]
            [MaxLength(24)]
            public string IdApparato { get; set; }
            [MaxLength(20)]

            public Int32 IdCliente { get; set; }   //Fisso a 1, al momento non è prevista cronologia
            public string Client { get; set; }
            public string ClientDescription { get; set; }
            public byte TipoApparato { get; set; }
            public byte[] DataPrimaCarica { get; set; }
            public UInt32 CntCicliTotali { get; set; }

            public DateTime UltimaLettura { get; set; }
            public string DtUltimaLettura { get; set; }

            public override string ToString()
            {
                return IdApparato.ToString();
            }

            public string strTipoApparato
            {
                get
                {
                    switch(TipoApparato)
                    {
                        case 0x81:
                            return "LLT.3";
                        default:
                            return "N.D.";
                    }

                }
            }

            public string strUltimaLettura
            {
                get
                {
                    if (UltimaLettura == DateTime.MinValue)
                    {
                        return "";
                    }
                    else
                    {
                        return UltimaLettura.ToString("yyyy/MM/dd") + " " + UltimaLettura.ToString("hh:mm"); //ToShortTimeString();
                    }
                }
            }

            public string strDtUltimaLettura
            {
                get
                {
                    return DtUltimaLettura;
                }
            }


        }



    }

    public class _parametri
    {
        [PrimaryKey]
        public int Id { get; set; }
        [MaxLength(255)]
        public string Parametro { get; set; }
        [MaxLength(255)]
        public int Tipo { get; set; }
        public int idMessaggio { get; set; }
        public string valTesto { get; set; }
        public int valInt { get; set; }
        public DateTime valTime { get; set; }
        public decimal valDec { get; set; }
        public override string ToString()
        {
            string _testo;
            _testo = Parametro;
            switch (Tipo)
            {
                case 1: // Stringa
                    _testo += valTesto;
                    break;
                case 2: // Intero
                    _testo += valInt.ToString();
                    break;
                case 3: // Data/ora
                    _testo += valTime.ToString();
                    break;
                case 4: //decimale
                    _testo += valDec.ToString();
                    break;
            }

            return _testo;
        }
    }

    public class _eventoBreve
    {
        [PrimaryKey]
        public int Id { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime DataModifica { get; set; }
        public string UtenteModifica { get; set; }
        [MaxLength(255)]
        public string Parametro { get; set; }
        [MaxLength(255)]
        public int Tipo { get; set; }
        public int idMessaggio { get; set; }
        public string valTesto { get; set; }
        public int valInt { get; set; }
        public DateTime valTime { get; set; }
        public decimal valDec { get; set; }
        public override string ToString()
        {
            string _testo;
            _testo = Parametro;
            switch (Tipo)
            {
                case 1: // Stringa
                    _testo += valTesto;
                    break;
                case 2: // Intero
                    _testo += valInt.ToString();
                    break;
                case 3: // Data/ora
                    _testo += valTime.ToString();
                    break;
                case 4: //decimale
                    _testo += valDec.ToString();
                    break;
            }

            return _testo;
        }
    }

}
