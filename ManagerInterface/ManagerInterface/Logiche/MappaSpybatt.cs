using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;

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
    public class MappaSpybatt
    {
        /*
        public enum StatoScheda : byte { NonCollegata = 0x00, SoloBootloader = 0x01, BLandFW = 0x02, SoloFW = 0x03 };

        public static SerialPort serialeApparato;
        private static MessaggioSpyBatt _mS;
        private parametriSistema _parametri;

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        public string FileHexDump = "c:\\Log\\HexDump.txt";


        // Area Dati:
        public spybattData sbData = new spybattData();           // Testata
        public sbDatiCliente sbCliente = new sbDatiCliente();    // Dati Cliante 
        public sbMemLunga sbUltimoCiclo = new sbMemLunga();
        public sbVariabili sbVariabili = new sbVariabili();
        public sbCalibrazioni Calibrazioni = new sbCalibrazioni();
        public sbStatoFirmware StatoFirmware = new sbStatoFirmware();
        public sbProgrammaRicarica ProgrammaCorrente;


        public SerialMessage.EsitoRisposta UltimaRisposta;
        public MessaggioSpyBatt.comandoInizialeSB IntestazioneSb = new MessaggioSpyBatt.comandoInizialeSB();
        public MessaggioSpyBatt.cicliPresenti CicliPresenti = new MessaggioSpyBatt.cicliPresenti();
        public MessaggioSpyBatt.comandoRTC OrologioSistema = new MessaggioSpyBatt.comandoRTC();
        public MessaggioSpyBatt.cicloAttuale CicloInMacchina = new MessaggioSpyBatt.cicloAttuale();


        private List<MessaggioSpyBatt.MemoriaPeriodoLungo> _CicliMemoriaLunga = new List<MessaggioSpyBatt.MemoriaPeriodoLungo>();
        private List<MessaggioSpyBatt.MemoriaPeriodoBreve> _CicliMemoriaBreve = new List<MessaggioSpyBatt.MemoriaPeriodoBreve>();
        private List<MessaggioSpyBatt.ProgrammaRicarica> _Programmazioni = new List<MessaggioSpyBatt.ProgrammaRicarica>();

        public List<MessaggioSpyBatt.CicloDiCarica> CicliInMemoria = new List<MessaggioSpyBatt.CicloDiCarica>();

        public List<sbMemLunga> CicliMemoriaLunga = new List<sbMemLunga>();
        public List<sbProgrammaRicarica> Programmazioni = new List<sbProgrammaRicarica>();

        public List<_sbMemLunga> DataCicliMemoriaLunga = new List<_sbMemLunga>();
        public List<_sbProgrammaRicarica> DataProgrammazioni = new List<_sbProgrammaRicarica>();


        public sbDataModel ModelloDati = new sbDataModel();
        */


        //*************************************************************************************************************

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");
        public byte[] ImmagineDati;

        /* ----------------------------------------------------------
         *  Dichiarazione eventi per la gestione avanzamento
         * ---------------------------------------------------------
         */
        public event StepHandler Step;
        public delegate void StepHandler(MappaSpybatt msb, ProgressChangedEventArgs e); //sbWaitEventStep e);
                                                                                        // ----------------------------------------------------------


        // Area Dati:
        // -- Testata
        public spybattData sbData = new spybattData();           // Testata
        public sbDatiCliente sbCliente = new sbDatiCliente();    // Dati Cliante

        // -- 
        private List<MessaggioSpyBatt.ProgrammaRicarica> _Programmazioni = new List<MessaggioSpyBatt.ProgrammaRicarica>();

        private List<MessaggioSpyBatt.MemoriaPeriodoLungo> _CicliMemoriaLunga = new List<MessaggioSpyBatt.MemoriaPeriodoLungo>();
        private List<MessaggioSpyBatt.MemoriaPeriodoBreve> _CicliMemoriaBreve = new List<MessaggioSpyBatt.MemoriaPeriodoBreve>();


        //*************************************************************************************************************



        public MappaSpybatt()
        {

        }

        

    
    


    }
}
