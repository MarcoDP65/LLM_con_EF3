using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChargerLogic
{
    public class FileSetupRigeneratore
    {
        public enum TipoArea :  byte { Sequenze = 0x01, Procedure = 0x02, NonDefinito = 0x00 };
        public List<AreaDatiRegen> ListaBlocchi { get; set; }

        public FileSetupRigeneratore()
        {
            ListaBlocchi = new List<AreaDatiRegen>();
        }
    }

    public class AreaDatiRegen
    {
        public int IdBlocco { get; set; }
        public FileSetupRigeneratore.TipoArea Tipo { get; set; }
        public int NumBlocchi { get; set; }
        public uint StartAddress { get; set; }
        public byte[] Data { get; set; }
    }
}
