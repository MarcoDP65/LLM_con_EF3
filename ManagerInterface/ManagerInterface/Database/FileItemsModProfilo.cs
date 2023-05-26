using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoriData
{
    public class FileItemsModProfilo
    {
        public List<ItemModProfilo> ListaProfili { get; set; }
        public DateTime DataCreazione { get; set; }

        public FileItemsModProfilo()
        {
            ListaProfili = new List<ItemModProfilo>();
            DataCreazione = DateTime.Now;
        }

    }

}
