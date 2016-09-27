using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

using Utility;


namespace ChargerLogic
{
    public partial class MessaggioDisplay : SerialMessage
    {

        public class PacchettoReadMem
        {

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public ushort numBytes;
            public UInt32 memAddress;
            public byte[] memData;
            public byte[] memDataDecoded;


            public bool datiPronti;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {

                byte[] _risposta;
                int startByte = 0;
                ushort _tempShort;
                byte _tempByte;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }
                    numBytes = (ushort)(_messaggio.Length / 2);
                    _risposta = new byte[numBytes];
                    memData = new byte[numBytes];

                    if (decodificaArray(_messaggio, ref _risposta))
                    {
                        startByte = 0;
                        memData = _messaggio;
                        memDataDecoded = _risposta;
                    }

                    datiPronti = true;
                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }





            public EsitoRisposta componiMessaggio(byte[] _messaggio)
            {
                ushort _tempShort;
                byte _tempByte;
                byte[] _tempArray;
                byte[] _tempMessaggio = new byte[10];
                byte[] _tempFromShort = new byte[2];

                try
                {
                    //l'intestazione deve essere pronta

                    datiPronti = false;

                    if (_messaggio.Length != 10) { return EsitoRisposta.NonRiconosciuto; }

                    _tempByte = decodificaByte(_messaggio[0], _messaggio[1]);
                    _tempShort = (ushort)(_tempByte);
                    _tempByte = decodificaByte(_messaggio[2], _messaggio[3]);
                    _tempShort = (ushort)((_tempShort << 8) + _tempByte);

                    return EsitoRisposta.MessaggioOk;
                }
                catch
                {
                    return EsitoRisposta.ErroreGenerico;
                }

            }



        }

    }
}
