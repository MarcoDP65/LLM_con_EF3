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
                        return UltimaLettura.ToShortDateString() + " " + UltimaLettura.ToShortTimeString();
                    }
                }
            }
        }
    }
}
