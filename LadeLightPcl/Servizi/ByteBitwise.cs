using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PannelloCharger
{
    public static class ByteBitWise
    {
        public static byte BitSet (byte Value ,bool BitStatus, byte BitMask)
        {
            try
            {
                int _tempCalc;
                int _tempVal = (int)Value;


                _tempCalc = (int)BitMask;
                _tempCalc = ~ _tempCalc;

                _tempVal = _tempVal & _tempCalc;

                if (BitStatus)
                {
                    _tempVal = _tempVal & (int)BitMask;
                }

                return (byte)_tempVal;

            }
            catch
            {
                return Value;
            }
        }

        public static bool BitVerify(byte Value, byte BitMask)
        {
            try
            {
                int _tempCalc = (int)BitMask;
                int _tempVal = (int)Value;
                 
                _tempVal = _tempVal & _tempCalc;

                return (_tempVal == _tempCalc);


            }
            catch
            {
                return false;
            }
        }


        public static byte ByteVerify(byte Value, byte BitMask)
        {
            try
            {
                int _tempCalc = (int)BitMask;
                int _tempVal = (int)Value;

                _tempVal = _tempVal & _tempCalc;

                return (byte)_tempVal ;


            }
            catch
            {
                return 0x00;
            }
        }



    }
}
