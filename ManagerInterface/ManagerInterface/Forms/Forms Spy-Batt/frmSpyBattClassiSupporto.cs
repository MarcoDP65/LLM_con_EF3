using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using FTD2XX_NET;

using NextUI.Component;
using NextUI.Frame;

using BrightIdeasSoftware;
using Utility;
using MoriData;
using ChargerLogic;
using MdiHelper;
using log4net;
using log4net.Config;



namespace PannelloCharger
{
    class frmSpyBattClassiSupporto
    {
        // nulla.........
    }

    public class Esp32Setting
    {
        public class PowerLevel
        {
            public byte ID { get; set; }
            public String Descrizione { get; set; }
            public byte Ordine { get; set; }

        }

        public class ByteKey
        {
            public byte ID { get; set; }
            public String Descrizione { get; set; }
            public byte Ordine { get; set; }
        }

        public List<PowerLevel> LivelliTx { get; private set; }
        public List<ByteKey> StatoModem { get; private set; }



        public void CaricaLivelli()
        {
            LivelliTx.Clear();
            LivelliTx.Add(new PowerLevel { ID = 0, Descrizione = "-12dbm", Ordine = 0 });
            LivelliTx.Add(new PowerLevel { ID = 1, Descrizione = "-9dbm ", Ordine = 1 });
            LivelliTx.Add(new PowerLevel { ID = 2, Descrizione = "-6dbm ", Ordine = 2 });
            LivelliTx.Add(new PowerLevel { ID = 3, Descrizione = "-3dbm ", Ordine = 3 });
            LivelliTx.Add(new PowerLevel { ID = 4, Descrizione = "-0dbm ", Ordine = 4 });
            LivelliTx.Add(new PowerLevel { ID = 5, Descrizione = "+3dbm ", Ordine = 5 });
            LivelliTx.Add(new PowerLevel { ID = 6, Descrizione = "+6dbm ", Ordine = 6 });
            LivelliTx.Add(new PowerLevel { ID = 7, Descrizione = "+9dbm ", Ordine = 7 });
        }

        public void CaricaStati()
        {
            StatoModem.Clear();
            StatoModem.Add(new ByteKey { ID = 0x00, Descrizione = "Off ", Ordine = 0 });
            StatoModem.Add(new ByteKey { ID = 0x01, Descrizione = "Timed ", Ordine = 1 });
            StatoModem.Add(new ByteKey { ID = 0xF0, Descrizione = "On ", Ordine = 2 });
        }



        public Esp32Setting()
        {
            LivelliTx = new List<PowerLevel>();
            StatoModem = new List<ByteKey>();
            CaricaLivelli();
            CaricaStati();
        }


    }



}
