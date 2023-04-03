using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using log4net;
using log4net.Config;

namespace MoriData
{
    /// <summary>
    /// llProfiloCarica: struttura record per il salvataggio dati modello Lade Light
    /// </summary>
    /// 
    public class llDurataCarica
    {
        [PrimaryKey]
        public ushort IdDurataCaricaLL { get; set; }
        public string Descrizione { get; set; }

            public byte Attivo { get; set; }
        public byte ProfiloPb { get; set; }
        public byte ProfiloGel { get; set; }
        public byte ProfiloLitio { get; set; }

        public int Ordine { get; set; }

        public byte DurataFaseDue(byte TipoBatt)

        {
            try
            {
                byte Esito = 100;

                switch (TipoBatt)
                {
                    case 0x00: //ND
                        Esito = 100;
                        break;
                    case 0x71: //Pb
                        Esito = ProfiloPb;
                        break;
                    case 0x72: //GEL
                        Esito = ProfiloGel;
                        break;
                    case 0x73: //Li
                        Esito = ProfiloLitio;
                        break;
                    default:
                        Esito = ProfiloLitio;
                        break;

                }
                return Esito;
            }
            catch
            {
                return 100;
            }

        }
             // valore in percentuale della durata F2 rispetto a F1



    }


    public class llDurataProfilo
    {
        [Indexed(Name = "IDXDurataProfilo", Order = 1, Unique = true)]
        public ushort IdDurataCaricaLL { get; set; }
        [Indexed(Name = "IDXDurataProfilo", Order = 2, Unique = true)]
        public byte IdProfiloCaricaLL { get; set; }

        public byte Attivo { get; set; }
 
    }



}
