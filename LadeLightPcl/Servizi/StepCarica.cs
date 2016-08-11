using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Common.Logging;
using Utility;
using MdiHelper;
using log4net;
using log4net.Config;


namespace ChargerLogic
{
    public class ValoriStepCarica
    {
        public byte IdStep { get; set; }
        public ushort valIMinima { get; set; }
        public ushort valIMassima { get; set; }
        public ushort valVMinima { get; set; }
        public ushort valVMassima { get; set; }
        public ushort valCapStep { get; set; }
        public ushort valToff { get; set; }
        public ushort valTon { get; set; }
        public byte NumRipetizioni { get; set; }

        public ValoriStepCarica()
        {
            IdStep = 0;
            valIMinima = 0;
            valIMassima = 0;
            valVMinima = 0;
            valVMassima = 0;
            valCapStep = 0;
            valToff = 0;
            valTon = 0;
            NumRipetizioni = 0;
        }

    }


    public class StepCarica
    {
        public ValoriStepCarica Dati;

        public StepCarica()
        {
            Dati = new ValoriStepCarica();
        }

        public bool CaricaDati( byte[] DatiStep )
        {
            try
            {
                if (DatiStep.Length < 16)
                    return false;

                Dati.IdStep = DatiStep[0];
                Dati.valIMinima = FunzioniComuni.UshortFromArray(_Dati, 13);
                Dati.valIMassima = 0;
                Dati.valVMinima = 0;
                Dati.valVMassima = 0;
                Dati.valCapStep = 0;
                Dati.valToff = 0;
                Dati.valTon = 0;
                Dati.NumRipetizioni = DatiStep[15];

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}