
//    class llMappaMemoria

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using log4net;
using log4net.Config;

using Utility;
using ChargerLogic;

namespace MoriData
{
    public class llModelloMemoria
    {

        public string IdModelloLL { get; set; }

        public llModelloBlocco ParametriApparato { get; set; }
        public llModelloBlocco DatiCliente { get; set; }
        public llModelloBlocco Programmazioni { get; set; }
        public llModelloBlocco Contatori { get; set; }
        public llModelloBlocco BufferDati { get; set; }
        public llModelloBlocco RecordBrevi { get; set; }
        public llModelloBlocco RecordLunghi { get; set; }
        public llModelloBlocco FWinfo { get; set; }
        public llModelloBlocco Bootloader { get; set; }
        public llModelloBlocco AreaApp1 { get; set; }
        public llModelloBlocco AreaApp2 { get; set; }

        public override string ToString()
        {
            return IdModelloLL;
        }
    }


    public class llModelloBlocco
    {
        public int IdBlocco { get; set; }
        public string Blocco { get; set; }

        public uint AddrArea { get; set; }
        public ushort NumPagine { get; set; }
        public byte SizeMsgDati { get; set; }
        public ushort StepMsgDati { get; set; }
        public ushort NumMsgDati { get; set; }
        /// <summary>
        ///Indica se i dati sono gestiti direttamente in memoria o con funzioni specifiche.
        /// </summary>
        /// <value>
        /// 0x00 gestione diretta.
        /// </value>
        public byte GestioneDiretta { get; set; }


    }

    public class llMappaMemoria
    {
        public llModelloMemoria MappaCorrente;

        public llMappaMemoria(int FwLevel = 1, CaricaBatteria.TipoCaricaBatteria ModCB = CaricaBatteria.TipoCaricaBatteria.LadeLight)
        {
            MappaCorrente = new llModelloMemoria();


            switch(FwLevel)
            {
                case 1:
                    if(ModCB == CaricaBatteria.TipoCaricaBatteria.SuperCharger)
                    {
                        InitSuperChargerLev1();
                    }
                    else
                    {
                        InitLev1();
                    }
                    break;

                case 2:
                    if (ModCB == CaricaBatteria.TipoCaricaBatteria.SuperCharger)
                    {
                        InitSuperChargerLev1();
                    }
                    else
                    {
                        InitLev2();
                    }
                    break;

                default:
                    if (ModCB == CaricaBatteria.TipoCaricaBatteria.SuperCharger)
                    {
                        InitSuperChargerLev1();
                    }
                    else
                    {
                        InitLev1();
                    }
                    break;

            }

        }

        public void InitLev1()
        {
            MappaCorrente = new llModelloMemoria();

            MappaCorrente.IdModelloLL = "Mappa 1";
            MappaCorrente.ParametriApparato = new llModelloBlocco() { IdBlocco  = 1, AddrArea = 0x000000, NumPagine = 1,SizeMsgDati = 242, StepMsgDati = 0x0100,NumMsgDati = 1,GestioneDiretta = 0 };
            MappaCorrente.DatiCliente = new llModelloBlocco() { IdBlocco = 2, AddrArea = 0x001000, NumPagine = 1, SizeMsgDati = 242, StepMsgDati = 0x0100, NumMsgDati = 1, GestioneDiretta = 0 };
            MappaCorrente.Programmazioni = new llModelloBlocco() { IdBlocco = 3, AddrArea = 0x002000, NumPagine = 1, SizeMsgDati = 242, StepMsgDati = 0x0100, NumMsgDati = 16, GestioneDiretta = 0 };
            MappaCorrente.RecordLunghi = new llModelloBlocco() { IdBlocco = 8, AddrArea = 0x1B3000, NumPagine = 4, SizeMsgDati = 36, StepMsgDati = 0x00F0, NumMsgDati = 16, GestioneDiretta = 0 };
            MappaCorrente.RecordBrevi = new llModelloBlocco() { IdBlocco = 7, AddrArea = 0x006000, NumPagine = 429, SizeMsgDati = 242, StepMsgDati = 0x00F0, NumMsgDati = 16, GestioneDiretta = 0 };

        }

        public void InitLev2()
        {
            MappaCorrente = new llModelloMemoria();

            MappaCorrente.IdModelloLL = "Mappa 2";
            MappaCorrente.ParametriApparato = new llModelloBlocco() { IdBlocco = 1, AddrArea = 0x000000, NumPagine = 1, SizeMsgDati = 242, StepMsgDati = 0x0100, NumMsgDati = 1, GestioneDiretta = 0 };
            MappaCorrente.DatiCliente = new llModelloBlocco() { IdBlocco = 2, AddrArea = 0x001000, NumPagine = 1, SizeMsgDati = 242, StepMsgDati = 0x0100, NumMsgDati = 1, GestioneDiretta = 0 };
            MappaCorrente.Programmazioni = new llModelloBlocco() { IdBlocco = 3, AddrArea = 0x002000, NumPagine = 1, SizeMsgDati = 242, StepMsgDati = 0x0100, NumMsgDati = 16, GestioneDiretta = 0 };
            MappaCorrente.RecordLunghi = new llModelloBlocco() { IdBlocco = 8, AddrArea = 0x1B4000, NumPagine = 3, SizeMsgDati = 48, StepMsgDati = 0x0100, NumMsgDati = 16, GestioneDiretta = 0 };
            MappaCorrente.RecordBrevi = new llModelloBlocco() { IdBlocco = 7, AddrArea = 0x006000, NumPagine = 430, SizeMsgDati = 242, StepMsgDati = 0x0100, NumMsgDati = 16, GestioneDiretta = 0 };

        }

        public void InitSuperChargerLev1()
        {
            MappaCorrente = new llModelloMemoria();

            MappaCorrente.IdModelloLL = "Mappa 1 - SuperCharger";
            MappaCorrente.ParametriApparato = new llModelloBlocco() { IdBlocco = 1, AddrArea = 0x000000, NumPagine = 1, SizeMsgDati = 242, StepMsgDati = 0x0100, NumMsgDati = 1, GestioneDiretta = 0 };
            MappaCorrente.DatiCliente = new llModelloBlocco() { IdBlocco = 2, AddrArea = 0x001000, NumPagine = 1, SizeMsgDati = 242, StepMsgDati = 0x0100, NumMsgDati = 1, GestioneDiretta = 0 };
            MappaCorrente.Programmazioni = new llModelloBlocco() { IdBlocco = 3, AddrArea = 0x002000, NumPagine = 1, SizeMsgDati = 242, StepMsgDati = 0x0100, NumMsgDati = 16, GestioneDiretta = 0 };
            MappaCorrente.RecordLunghi = new llModelloBlocco() { IdBlocco = 8, AddrArea = 0x1B4000, NumPagine = 75, SizeMsgDati = 48, StepMsgDati = 0x0100, NumMsgDati = 16, GestioneDiretta = 0 };
            MappaCorrente.RecordBrevi = new llModelloBlocco() { IdBlocco = 7, AddrArea = 0x006000, NumPagine = 430, SizeMsgDati = 242, StepMsgDati = 0x0100, NumMsgDati = 16, GestioneDiretta = 0 };

        }

    }


}
