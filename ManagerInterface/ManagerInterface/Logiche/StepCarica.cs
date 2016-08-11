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
        public byte TipoStep { get; set; }
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
            TipoStep = 0; 
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

        public bool CaricaDati( byte[] DatiStep, byte NumStep )
        {
            try
            {
                if (DatiStep.Length < 16)
                    return false;

                Dati.IdStep = NumStep;
                Dati.TipoStep = DatiStep[0];
                Dati.valIMinima = FunzioniComuni.UshortFromArray(DatiStep, 1);
                Dati.valIMassima = FunzioniComuni.UshortFromArray(DatiStep, 3);
                Dati.valVMinima = FunzioniComuni.UshortFromArray(DatiStep, 5);
                Dati.valVMassima = FunzioniComuni.UshortFromArray(DatiStep, 7);
                Dati.valCapStep = FunzioniComuni.UshortFromArray(DatiStep, 9);
                Dati.valToff = FunzioniComuni.UshortFromArray(DatiStep, 11);
                Dati.valTon = FunzioniComuni.UshortFromArray(DatiStep, 13);
                Dati.NumRipetizioni = DatiStep[15];

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string strTipoStep
        {
            get
            {
                string _tipo = "";

                switch( Dati.TipoStep )
                {
                    case 0x01:
                        _tipo = "I";
                        break;
                    case 0x02:
                        _tipo = "U";
                        break;
                    case 0x03:
                        _tipo = "W";
                        break;
                    case 0x04:
                        _tipo = "PU";
                        break;
                    case 0x05:
                        _tipo = "M";
                        break;
                    case 0x80:
                        _tipo = "PA";
                        break;
                    default:
                        _tipo = "N.D.";
                        break;
                }


                return _tipo;
            }
        }




        public string strIMinima
        {
            get
            {
                return FunzioniMR.StringaCorrenteLL(Dati.valIMinima);
            }
        }


        public string strIMassima
        {
            get
            {
                return FunzioniMR.StringaCorrenteLL(Dati.valIMassima);
            }
        }


        public string strVMinima
        {
            get
            {
                return FunzioniMR.StringaTensione(Dati.valVMinima);
            }
        }


        public string strVMassima
        {
            get
            {
                return FunzioniMR.StringaTensione(Dati.valVMassima);
            }
        }

        public string strCapStep 
        {
            get
            {
                return FunzioniMR.StringaCorrenteLL(Dati.valCapStep);
            }
        }

        public string strToff
        {
            get
            {
                return Dati.valToff.ToString();
            }
        }

        public string strTon
        {
            get
            {
                return Dati.valTon.ToString();
            }
        }

        public string strNumRipetizioni
        {
            get
            {
                return  Dati.NumRipetizioni.ToString();
            }
        }

        public string strPasso
        {
            get
            {
                return "Passo " + Dati.IdStep.ToString();
            }
        }


    }
}