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
    public class ModelliInitDisplay
    {
        public List<ModelloInitDisplay> ListaModelli;

        public ModelliInitDisplay()
        {
            ListaModelli = new List<ModelloInitDisplay>();
        }

        public bool InitLocale()
        {
            try
            {
                ListaModelli = new List<ModelloInitDisplay>();

                // Modello 1 : LLT IP55 AENA
                ModelloInitDisplay NuovoModello = new ModelloInitDisplay();
                NuovoModello.TestataInit.IdInizializzazione = 1;
                NuovoModello.TestataInit.CodArticolo = "LLT.3.AE.IP55";
                NuovoModello.TestataInit.ProgrArticolo = 1;
                NuovoModello.TestataInit.Predefinito = 0;
                NuovoModello.TestataInit.Attivo = 1;
                NuovoModello.TestataInit.Descrizione = "LL Supercharger IP55 AENA";
                NuovoModello.TestataInit.IdModelloCB = 0x88;
                NuovoModello.TestataInit.FamigliaApparato = (byte)ChargerLogic.CaricaBatteria.TipoCaricaBatteria.SuperCharger;

                NuovoModello.TestataInit.HardwareDisp = "1.030001";

                NuovoModello.TestataInit.VMin = 2400;
                NuovoModello.TestataInit.VMax = 12000;
                NuovoModello.TestataInit.Amax = 1200;

                // Gestione Moduli SCHG
                NuovoModello.TestataInit.NumeroModuli = 1;
                NuovoModello.TestataInit.ModVNom = 8000;
                NuovoModello.TestataInit.ModANom = 1200;
                NuovoModello.TestataInit.ModOpzioni = 0;
                NuovoModello.TestataInit.ModVMin = 2400;
                NuovoModello.TestataInit.ModVMax = 12000;


                NuovoModello.TestataInit.DataCreazione = new DateTime(2023, 11, 8, 12, 00, 00);

//              ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0x88, NomeModello = "LLT.3.x 24-80V / 120A SCHG", CorrenteMin = 10, CorrenteMax = 120, TensioneMin = 24, TensioneMax = 120, Ordine = 1, Trifase = 1, Attivo = 0x01, FamigliaCaricabetteria = ChargerLogic.CaricaBatteria.TipoCaricaBatteria.SuperCharger, TensioneNominale = 0 });  // V max--> 80/2*3 = 120

                // Ora gli step
                // 1 Cancellazione R0 ( azzeramento inizializzazione )
                NuovoModello.ListaStepInit.Add(new SqStepInit() { IdInizializzazione = 1, Step = 1, Attivita = SqStepInit.TipoStep.Delete4K, Attivo = true, Obbligatorio = 1, Descrizione = "Cancellazione Configurazione esistente", Address = 0, });
                // 2 Cancellazione R3000 ( azzeramento contatori )
                NuovoModello.ListaStepInit.Add(new SqStepInit() { IdInizializzazione = 1, Step = 2, Attivita = SqStepInit.TipoStep.Delete4K, Attivo = true, Obbligatorio = 1, Descrizione = "Cancellazione Contatori esistenti", Address = 0x3000, });
                // 3 Cancellazione R2000 ( azzeramento Profili )
                NuovoModello.ListaStepInit.Add(new SqStepInit() { IdInizializzazione = 1, Step = 3, Attivita = SqStepInit.TipoStep.Delete4K, Attivo = true, Obbligatorio = 1, Descrizione = "Cancellazione Programmazioni esistenti", Address = 0x2000, });
                // 4 Reboot
                NuovoModello.ListaStepInit.Add(new SqStepInit() { IdInizializzazione = 1, Step = 4, Attivita = SqStepInit.TipoStep.Reboot, Attivo = true, Obbligatorio = 1, Descrizione = "Riavvio" });
                // 5 Inizializzazione
                NuovoModello.ListaStepInit.Add(new SqStepInit() { IdInizializzazione = 1, Step = 5, Attivita = SqStepInit.TipoStep.WriteInit, Attivo = true, Obbligatorio = 1, Descrizione = "Scrittura Inizializzazione" });
                // 6 Scrittura ciclo base
                NuovoModello.ListaStepInit.Add(new SqStepInit() { IdInizializzazione = 1, Step = 6, Attivita = SqStepInit.TipoStep.AddProfilo, Attivo = true, Obbligatorio = 1, Descrizione = "Scrittura Profilo Base",
                    Address = 0x2000,
                    DataLenght = 226,
                    StrDataArray = "00000001FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF170B0D0F0EFFFFFFFFFFFF000800335350592D42415454E0000056000010000057000031000080000007000006000026000015000001000021000011000012000025000002000003000005000024000004000013000050000F5800001A00001B00001C00001400002200009000006F00006000006100006300006200006400007F0000700000710000720000730000740000AF0000A10000A20000A30000A60000A800001600000800000900005A0000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEA52"
                });
                // 7 Reboot
                NuovoModello.ListaStepInit.Add(new SqStepInit() { IdInizializzazione = 1, Step = 7, Attivita = SqStepInit.TipoStep.Reboot, Attivo = true, Obbligatorio = 1, Descrizione = "Riavvio" });
                // 8 Leggi configurazione
                NuovoModello.ListaStepInit.Add(new SqStepInit() { IdInizializzazione = 1, Step = 8, Attivita = SqStepInit.TipoStep.ReadInit, Attivo = true, Obbligatorio = 1, Descrizione = "Lettura Configurazione" });

                ListaModelli.Add(NuovoModello);




                return false;
            }
            catch
            { 
                return false; 
            }    

        }
    }
}
