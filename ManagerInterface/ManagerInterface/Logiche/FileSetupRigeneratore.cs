using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargerLogic
{
    public class FileSetupRigeneratore
    {
        public enum Tipologia : byte { None = 0x00, Sequenze = 0x01, Procedure = 0x02, Lingua = 0xF0, Test = 0x04 }

        public DateTime DataCreazionePacchetto { get; set; }

        public byte TipoPacchetto { get; set; }

        public string FwMin { get; set; }
        public string FwMax { get; set; }

        public string Release { get; set; }

        public string Note { get; set; }
        public List<AreaDatiRegen> ListaBlocchi { get; set; }

        public FileSetupRigeneratore()
        {
            ListaBlocchi = new List<AreaDatiRegen>();
        }
    }

    // 09/06/2022 - allargato l'elenco tipologie per rendere la classe usabile anche con altri device
    public class AreaDatiRegen
    {
        public enum TipoArea : byte { Sequenze = 0x01, Procedure = 0x02, Lingua = 0xF0, Contatori = 0x10, Programmazioni = 0x11, CicliLunghi = 0x20, CicliBrevi = 0x21, NonDefinito = 0x00 };

        public int IdBlocco { get; set; }
        public TipoArea Tipo { get; set; }
        public int NumBlocchi { get; set; }
        public uint StartAddress { get; set; }
        public byte[] Data { get; set; }

        public string strIdBlocco
        {
            get
            {
                return IdBlocco.ToString();
            }
        }
        public string strTipoArea
        {
            get
            {
                return Tipo.ToString();
            }
        }
        public string strNumBlocchi
        {
            get
            {
                return NumBlocchi.ToString();
            }
        }
        public string strStartAddress
        {
            get
            {
                return StartAddress.ToString("X6");
            }
        }
        public string strDataSize
        {
            get
            {
                return (Data.Length / 0x1000).ToString() + " KBytes";
            }
        }
    }
}
