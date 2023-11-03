using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BrightIdeasSoftware;

using ChargerLogic;
using log4net;
using log4net.Config;

using MoriData;
using Utility;

namespace PannelloCharger
{
    public class ParametriAreaSCH
    {
        public int NumArea { get; set; }
        public int NumBytes { get; set; }
        public int NumPacchetti { get; set; }
        public uint DimPacchetto { get; set; }
        public UInt32 AddrDestPacchetto { get; set; }

        public string strNumArea
        {
            get
            {
                return NumArea.ToString("0");
            }

        }

        public string strNumBytes
        {
            get
            {
                if (NumBytes >= 0)
                    return NumBytes.ToString("0");
                else
                    return "";
            }

        }

        public string strNumPacchetti
        {
            get
            {
                if (NumPacchetti >= 0)
                    return NumPacchetti.ToString("0");
                else
                    return "";
            }

        }

        public string strDimPacchetto
        {
            get
            {
                if (DimPacchetto >= 0)
                    return DimPacchetto.ToString("0");
                else
                    return "";
            }

        }

        public string strAddrDestPacchetto
        {
            get
            {
                if (DimPacchetto >= 0)
                    return "0x" + AddrDestPacchetto.ToString("X6");
                else
                    return "";
            }

        }
    }
}
