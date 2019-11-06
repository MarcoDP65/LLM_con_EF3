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

        public class StatoScheda
        {

            public byte Pulsante1 { get; set; }
            public byte Pulsante2 { get; set; }
            public byte Pulsante3 { get; set; }
            public byte Pulsante4 { get; set; }
            public byte Pulsante5 { get; set; }


            public DateTime IstanteLettura { get; set; }

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio)
            {

                byte[] _risposta;
                int startByte = 0;

                try
                {
                    datiPronti = false;
                    if (_messaggio.Length < 2)
                    {
                        datiPronti = false;
                        return EsitoRisposta.NonRiconosciuto;
                    }

                    _risposta = new byte[(_messaggio.Length / 2)];

                    if (decodificaArray(_messaggio, ref _risposta))
                    {
                        Log.Debug(" ---------------------- ParametriSpybatt -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        startByte = 0;

                        Pulsante1 = _risposta[startByte++];
                        Pulsante2 = _risposta[startByte++];
                        Pulsante3 = _risposta[startByte++];
                        Pulsante4 = _risposta[startByte++];
                        Pulsante5 = _risposta[startByte++];


                        datiPronti = true;
                        IstanteLettura = DateTime.Now;

                    }


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
