using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ChargerLogic
{
    public class elementiComuni
    {
        public const int TimeoutBase = 2;   // Durata di default del timeout di comunicazione, espressa in secondi.
        public const int TimeoutLungo = 5;  // Durata di default del timeout di comunicazione per le operazioni di lunga durata, espressa in secondi.
        public enum tipoMessaggio : int { vuoto = 0 ,
                                          MemLunga = 1,
                                          MemBreve = 2,
                                          Programmi = 3 ,
                                          DumpMemoria = 4,
                                          AggiornamentoFirmware = 5,
                                          ClonazioneScheda = 6,
                                          NonDefinito =-1 };

        public enum contenutoMessaggio : int { vuoto = 0, Ack = 1, Nack = 2, Break = 3, Dati = 10, NonValido = -1 };
        public enum EsitoFunzione : byte { OK = 0x00, DatiNonVAlidi = 0x01,NonInSequenza = 0x02,ErroreGenerico = 0xFF}
 

        public enum modoDati : byte { Import = 0x00,Output = 0x01 };
        public enum VersoCorrenti : byte { Diretto = 0x00 , Inverso = 0x01 };

        public enum InitialCrcValue : ushort { Zeros, NonZero1 = 0xffff, NonZero2 = 0x1D0F };
        public class Crc16Ccitt
        {
            const ushort poly = 4129;
            ushort[] table = new ushort[256];
            ushort initialValue = 0;
        

            public ushort ComputeChecksum(byte[] bytes)
            {
                ushort crc = this.initialValue;
                for (int i = 0; i < bytes.Length; ++i)
                {
                    crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
                }
                return crc;
            }


            public ushort ComputePartialChecksum(byte[] bytes,int NumBytes)
            {
                ushort crc = this.initialValue;
                int _tempNum = NumBytes;

                if (_tempNum < 0)
                    _tempNum = 0;

                if (_tempNum > bytes.Length)
                    _tempNum = bytes.Length;

                for (int i = 0; i < _tempNum; ++i)
                {
                    crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
                }
                return crc;
            }


            /// <summary>
            /// Calcolo il CRC complessivo di 3 array distinti
            /// </summary>
            /// <param name="bytes1"></param>
            /// <param name="bytes2"></param>
            /// <param name="bytes3"></param>
            /// <returns></returns>
            public ushort ComputeChecksum3(byte[] bytes1, byte[] bytes2, byte[] bytes3)
            {
                // Poichè il CRC è incrementale faccio passare in sequenza i tre pacchetti e al termine ottengo il CRC complessivo
                ushort crc = this.initialValue;
                for (int i = 0; i < bytes1.Length; ++i)
                {
                    crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes1[i]))]);
                }
                for (int i = 0; i < bytes2.Length; ++i)
                {
                    crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes2[i]))]);
                }
                for (int i = 0; i < bytes3.Length; ++i)
                {
                    crc = (ushort)((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes3[i]))]);
                }

                return crc;
            }


            public byte[] ComputeChecksumBytes(byte[] bytes)
            {
                ushort crc = ComputeChecksum(bytes);
                return BitConverter.GetBytes(crc);
            }

            public Crc16Ccitt(InitialCrcValue initialValue)
            {
                this.initialValue = (ushort)initialValue;
                ushort temp, a;
                for (int i = 0; i < table.Length; ++i)
                {
                    temp = 0;
                    a = (ushort)(i << 8);
                    for (int j = 0; j < 8; ++j)
                    {
                        if (((temp ^ a) & 0x8000) != 0)
                        {
                            temp = (ushort)((temp << 1) ^ poly);
                        }
                        else
                        {
                            temp <<= 1;
                        }
                        a <<= 1;
                    }
                    table[i] = temp;
                }
            }
        }

    }
}
