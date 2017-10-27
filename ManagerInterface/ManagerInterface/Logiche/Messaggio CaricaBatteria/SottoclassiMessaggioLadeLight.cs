using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

using Utility;


namespace ChargerLogic
{
    public partial class MessaggioLadeLight : SerialMessage
    {
        public class StatoFirmware
        {

            public string RevBootloader;
            public string RevFirmware;
            public byte[] ReleaseDateBlock;
            public ushort CRCFirmware;
            public uint AddrFlash0;
            public uint LenFlash0;
            public uint AddrFlash1;
            public uint LenFlash1;
            public uint AddrFlash2;
            public uint LenFlash2;
            public uint AddrFlash3;
            public uint LenFlash3;
            public uint AddrFlash4;
            public uint LenFlash4;

            public byte Stato;
            public DateTime IstanteLettura;

            byte[] _dataBuffer;
            public byte[] dataBuffer;
            public bool datiPronti;
            public string lastError;

            public EsitoRisposta analizzaMessaggio(byte[] _messaggio, int fwLevel)
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
                        startByte = 0;
                        Log.Debug(" ---------------------- Info Firmware -----------------------------------------");
                        Log.Debug(FunzioniMR.hexdumpArray(_risposta));

                        RevBootloader = ArrayToString(_risposta, startByte, 6);
                        startByte += 6;
                        RevFirmware = ArrayToString(_risposta, startByte, 6);
                        startByte += 6;
                        CRCFirmware = ArrayToUshort(_risposta, startByte, 2);
                        startByte += 2;
                        AddrFlash0 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash0 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash1 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash1 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash2 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash2 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash3 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash3 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        AddrFlash4 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;
                        LenFlash4 = ArrayToUint32(_risposta, startByte, 4);
                        startByte += 4;

                        ReleaseDateBlock = new byte[3];
                        ReleaseDateBlock[0] = _risposta[startByte];
                        if (ReleaseDateBlock[0] == 0xFF)
                            ReleaseDateBlock[0] = 1;

                        startByte += 1;
                        ReleaseDateBlock[1] = _risposta[startByte];
                        if (ReleaseDateBlock[1] == 0xFF)
                            ReleaseDateBlock[1] = 1;

                        startByte += 1;
                        ReleaseDateBlock[2] = _risposta[startByte];
                        if (ReleaseDateBlock[2] == 0xFF)
                            ReleaseDateBlock[2] = 15;

                        startByte += 1;

                        //Area non usata
                        startByte += 88;
                        if (startByte < _risposta.Length)
                        {
                            Stato = _risposta[startByte];
                            startByte += 1;
                            if (Stato == 0xFF) Stato = 0x07;
                        }
                        else
                        {
                            Stato = 0x07;
                        }

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
