using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using MoriData;
using ChargerLogic;
using NextUI.Component;
using NextUI.Frame;

using Common.Logging;
using BrightIdeasSoftware;
using Utility;
using MdiHelper;



namespace PannelloCharger
{
    public partial class frmSpyBat : Form
    {
        public System.Collections.Generic.List<ValoreLista> ListaBrSig = new List<ValoreLista>()
        {
            new ValoreLista("OFF", SerialMessage.OcBaudRate.OFF, false),
            new ValoreLista("ON 9.6K", SerialMessage.OcBaudRate.br_9k6, false),
            new ValoreLista("ON 19.2K", SerialMessage.OcBaudRate.br_19k2, false),
            new ValoreLista("ON 38.4K", SerialMessage.OcBaudRate.br_38k4, false),
            new ValoreLista("ON 57.6K", SerialMessage.OcBaudRate.br_57k6, false),
        };

        public System.Collections.Generic.List<ValoreLista> ListaEchoSig = new List<ValoreLista>()
        {
            new ValoreLista("OFF", SerialMessage.OcEchoMode.OFF, false),
            new ValoreLista("Listen", SerialMessage.OcEchoMode.Listening, false),
            new ValoreLista("Echo", SerialMessage.OcEchoMode.Echo, false),

        };

    }
}
