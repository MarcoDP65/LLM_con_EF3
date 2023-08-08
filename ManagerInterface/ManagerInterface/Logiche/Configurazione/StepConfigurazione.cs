using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargerLogic
{
    public class StepConfigurazione
    {
        public enum TipoStep : byte 
        { 
            ConfigurazioneUSB = 0x01, 
            UploadFW =  0x02, 
            InizializzazioneScheda = 0x03, 
            CaricamentoCiclo = 0x04, 
            RiavvioScheda = 0x05, 
            ResetScheda = 0x06, 
            CancellazioneCicli = 0x07,
            CancellazioneContatori = 0x08,
            ImpostazioneData = 0x09
        }

        public string Nome { get; set; }
        public string Descrizione { get; set;}
        public bool Attivo { get; set; }
        public bool Esito { get; set; }
        public int Avanzamento { get; set; }

        public int IdSetup { get; set; }
        public int IdStep { get; set; }

        public int Ordine { get; set; }
        public TipoStep Attivita { get; set;}

        // ora i campi CFG in base all'attività

        

        
    }
}
