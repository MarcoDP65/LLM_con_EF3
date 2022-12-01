using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;


namespace ChargerLogic
{
    public class BaudRate
    {
        public int Speed { get; set; }
        public enum BRType : byte { BR_9600 = 0, BR_19200 = 1, BR_38400 = 2, BR_57600 = 3, BR_115200 = 4, BR_CUSTOM = 0x7F }
        public BRType Mode { get; set; }
        public byte[] CmdData { get; protected set; }



        public BaudRate ()
        {
            Mode = BRType.BR_9600;
            Speed = 0;
            CmdData = new byte[4];
        }

        public override string ToString()
        {
            switch(Mode) 
            {
                case BRType.BR_9600:
                    {
                        return "Fix 9600";
                    }
                case BRType.BR_38400:
                    {
                        return "Fix 38400";
                    }
                case BRType.BR_57600:
                    {
                        return "Fix 57600";
                    }
                case BRType.BR_115200:
                    {
                        return "Fix 115200";
                    }
                case BRType.BR_CUSTOM:
                    {
                        return "Cust " + Speed.ToString();
                    }

            }
            return "";
        }

        public uint SetSpeed()
        {
            switch (Mode)
            {
                case BRType.BR_9600:
                    {
                        return 9600;
                    }
                case BRType.BR_38400:
                    {
                        return 38400;
                    }
                case BRType.BR_57600:
                    {
                        return 57600;
                    }
                case BRType.BR_115200:
                    {
                        return 115200;
                    }
                case BRType.BR_CUSTOM:
                    {
                        return (uint)Speed;
                    }

            }
            return 9600;
        }

        public void SetCmdData()
        {

            // Variabili temporanee per il passaggio dati
            byte _byte1 = 0;
            byte _byte2 = 0;
            byte _byte3 = 0;
            byte _byte4 = 0;

            CmdData = new byte[4];

            if (Mode != BRType.BR_CUSTOM)
            {
                CmdData[0] = (byte)Mode;
                CmdData[1] = 0;
                CmdData[2] = 0;
                CmdData[3] = 0;
            }
            else
            {
                CmdData[0] = (byte)Mode;
                FunzioniComuni.SplitInt32(Speed, ref _byte1, ref _byte2, ref _byte3, ref _byte4);
                CmdData[1] = _byte2;
                CmdData[2] = _byte3;
                CmdData[3] = _byte4;
            }
        }

    }
}
