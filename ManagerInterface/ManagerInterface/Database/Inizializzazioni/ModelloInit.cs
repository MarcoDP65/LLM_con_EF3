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
    public class ModelloInitDisplay
    {
        public enum TipoStep : byte { NOOP = 0, Delete4K = 1, WriteInit = 2, AddProfilo = 3, SetTime = 4, Reboot = 0xFF }
        public SqInitArticolo TestataInit { get; set; }
        public List<SqStepInit> ListaStepInit { get; set; }

        public ModelloInitDisplay()
        {
            TestataInit = new SqInitArticolo();
            ListaStepInit = new List<SqStepInit>();
        }

        public int IdInizializzazione
        {
            get
            {
                return TestataInit.IdInizializzazione;
            }

        }
        public string CodArticolo
        {
            get
            {
                return TestataInit.CodArticolo;
            }

        }
        public string Descrizione
        {
            get
            {
                return TestataInit.Descrizione;
            }

        }
        public int NumSteps
        {
            get
            {
                return ListaStepInit.Count();
            }
        }

        public string DescrVoce
        {
            get
            {
                return TestataInit.CodArticolo + " - " + TestataInit.Descrizione;
            }

        }
    }
}
