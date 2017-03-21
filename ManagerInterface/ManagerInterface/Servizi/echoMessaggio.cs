using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;

namespace PannelloCharger
{
    public class echoMessaggio
    {

        public enum Tipo: byte { Richiesta = 0x01, Risposta = 0x02, Indeterminato = 0x00,NonValido = 0xFF }
        public enum TipoDevice : byte { LADELight = 0x01, SPYBATT = 0x02,Display = 0x03, PC = 0x04, Indeterminato = 0x00,Monitor = 0xF0,  NonValido = 0xFF }

        public string Dispositivo { get; set; }
        public string TipoDispositivo { get; set; }
        public Tipo TipoMessaggio { get; set; }
        public TipoDevice Device { get; set; }
        public string Comando { get; set; }
        public string Sottocomando { get; set; }
        public byte[] DataArray { get; set; }

        public string DescComando { get; set; }
        public string Parametri { get; set; }
        public bool SegueRiga;
        public DateTime Timestamp { get; set; }

        public echoMessaggio()
        {
            Dispositivo = "";
            TipoDispositivo = "";
            Comando = "";
            Sottocomando = "";
            Parametri = "";
            Device = TipoDevice.Indeterminato;

            SegueRiga = false;
            DataArray = new byte[0];
            TipoMessaggio = Tipo.Indeterminato;
            Timestamp = DateTime.Now;
        }

        #region Proprietà

        public string Istante
        {
            get
            {
                if (SegueRiga) return "";
                if (Timestamp == null) return "N.D.";

                return Timestamp.ToString("HH:mm:ss.fff");
            }
        }

        public string HexdumpData
        {
            get
            {
                if (DataArray == null)
                    return "";
                return FunzioniComuni.HexdumpArray(DataArray,true);
            }
        }

        #endregion

    }
}
