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
    public class DatiSpyBatt
    {      
        public spybattData sbData = new spybattData();           
        public sbDatiCliente sbCliente = new sbDatiCliente();    
        public sbMemLunga sbUltimoCiclo = new sbMemLunga();
        public sbVariabili sbVariabili = new sbVariabili();
        public sbCalibrazioni Calibrazioni = new sbCalibrazioni();
        public sbStatoFirmware StatoFirmware = new sbStatoFirmware();
        public sbProgrammaRicarica ProgrammaCorrente;

        private List<MessaggioSpyBatt.MemoriaPeriodoLungo> _CicliMemoriaLunga = new List<MessaggioSpyBatt.MemoriaPeriodoLungo>();
        private List<MessaggioSpyBatt.MemoriaPeriodoBreve> _CicliMemoriaBreve = new List<MessaggioSpyBatt.MemoriaPeriodoBreve>();
        private List<MessaggioSpyBatt.ProgrammaRicarica> _Programmazioni = new List<MessaggioSpyBatt.ProgrammaRicarica>();

        public List<MessaggioSpyBatt.CicloDiCarica> CicliInMemoria = new List<MessaggioSpyBatt.CicloDiCarica>();

        public List<sbMemLunga> CicliMemoriaLunga = new List<sbMemLunga>();
        public List<sbProgrammaRicarica> Programmazioni = new List<sbProgrammaRicarica>();

        public List<_sbMemLunga> DataCicliMemoriaLunga = new List<_sbMemLunga>();
        public List<_sbProgrammaRicarica> DataProgrammazioni = new List<_sbProgrammaRicarica>();

        public List<ParametroCalibrazione> ParametriCalibrazione = new List<ParametroCalibrazione>();

        public sbDataModel ModelloDati = new sbDataModel();


        public DatiSpyBatt()
        {

        }




    }
}
