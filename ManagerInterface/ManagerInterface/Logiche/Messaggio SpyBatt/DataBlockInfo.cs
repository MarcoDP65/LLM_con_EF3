using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChargerLogic;
using Utility;

namespace PannelloCharger
{
    public class DataBlockInfo
    {
        public string BlockRelease { get; set; }
        public string RegenFwMin { get; set; }
        public string RegenFwMax { get; set; }
        public byte[] DataRilascio { get; set; }
        public byte[] DataInstallazione { get; set; }
        public byte[] DataBuffer { get; set; }
        public bool IsCleanArray { get; set; }
        public DataBlockInfo()
        {
            BlockRelease = "";
            RegenFwMin = "";
            RegenFwMax = "";
            DataRilascio = new byte[3];
            DataInstallazione = new byte[5];
            DataBuffer = null;
            IsCleanArray = true;
        }



        public byte[] ToByteArray()
        {
            try
            {
                string tempString = "";
                byte[] tempVal;

                byte[] Buffer = new byte[32];
                // Release 0 to 6
                tempString = BlockRelease + "       ";
                tempVal = FunzioniComuni.StringToArray(tempString, 7, 0);
                for (int _a = 0; _a < 7; _a++)
                {
                    Buffer[_a] = tempVal[_a];
                }

                // Data Rilascio 7 to 9
                for (int _a = 0; _a < 3; _a++)
                {
                    Buffer[_a + 7] = DataRilascio[_a];
                }

                // fw min 10 to 16
                tempString = RegenFwMin + "       ";
                tempVal = FunzioniComuni.StringToArray(tempString, 7, 0);
                for (int _a = 0; _a < 7; _a++)
                {
                    Buffer[_a + 10] = tempVal[_a];
                }

                // fw max 17 to 23
                tempString = RegenFwMax + "       ";
                tempVal = FunzioniComuni.StringToArray(tempString, 7, 0);
                for (int _a = 0; _a < 7; _a++)
                {
                    Buffer[_a + 17] = tempVal[_a];
                }

                // Data Installazione 24 to 28
                for (int _a = 0; _a < 5; _a++)
                {
                    Buffer[_a + 24] = DataInstallazione[_a];
                }

                return Buffer;
            }
            catch
            {
                return null;
            }
        }

        public bool FromByteArray(byte[] DataSource)
        {
            try
            {
                if (DataSource == null)
                    return false;

                if (DataSource.Length != 32)
                    return false;

                if (FunzioniComuni.IfCleanArray(DataSource, 32, 0))
                {
                    BlockRelease = "";
                    RegenFwMin = "";
                    RegenFwMax = "";
                    DataRilascio = new byte[3];
                    DataInstallazione = new byte[5];
                    IsCleanArray = true;
                    return true;
                }

                DataBuffer = FunzioniComuni.ArrayCopy(DataSource, 32, 0);
                BlockRelease = FunzioniComuni.ArrayToString(DataBuffer, 0, 7);
                DataRilascio = FunzioniComuni.ArrayCopy(DataSource, 3, 7);
                RegenFwMin = FunzioniComuni.ArrayToString(DataBuffer, 10, 7);
                RegenFwMax = FunzioniComuni.ArrayToString(DataBuffer, 17, 7);
                DataInstallazione = FunzioniComuni.ArrayCopy(DataSource, 5, 24);
                IsCleanArray = false;
                return true;
            }
            catch
            {
                return false;
            }
        }




        public DateTime dtDataRilascio
        {
            get
            {
                DateTime tempdata = new DateTime(2000 + DataRilascio[2], DataRilascio[1], DataRilascio[0]);
                return tempdata;
            }
            set
            {
                DataRilascio = new byte[3];
                if (value != null)
                {
                    if (value.Year < 2000)
                    {
                        DataRilascio[2] = 0;
                    }
                    else
                    {
                        DataRilascio[2] = (byte)(value.Year - 2000);
                    }
                    DataRilascio[1] = (byte)(value.Month);
                    DataRilascio[0] = (byte)(value.Day);

                }


            }
        }

        public DateTime dtDataInstallazione
        {
            get
            {
                DateTime tempdata = new DateTime(2000 + DataInstallazione[4], DataInstallazione[3], DataInstallazione[2], DataInstallazione[1], DataInstallazione[0], 0, 0);
                return tempdata;
            }
            set
            {
                DataInstallazione = new byte[5];
                if (value != null)
                {
                    if (value.Year < 2000)
                    {
                        DataInstallazione[4] = 0;
                    }
                    else
                    {
                        DataInstallazione[4] = (byte)(value.Year - 2000);
                    }
                    DataInstallazione[3] = (byte)(value.Month);
                    DataInstallazione[2] = (byte)(value.Day);
                    DataInstallazione[1] = (byte)(value.Hour);
                    DataInstallazione[0] = (byte)(value.Minute);
                }


            }
        }

        public string strDataRilascio
        {
            get
            {
                return FunzioniComuni.StringaDataTS(DataRilascio);
            }
        }

        public string strDataInstallazione
        {
            get
            {
                return FunzioniComuni.StringaTimestamp(DataInstallazione);
            }
        }

    }
}
