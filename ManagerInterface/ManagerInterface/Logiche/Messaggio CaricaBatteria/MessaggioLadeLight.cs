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

        private static ILog Log = LogManager.GetLogger("PannelloChargerLog");

        private string _lastError;

        private byte[] _idCorrente = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private byte[] _tempId = { 0, 0, 0, 0, 0, 0, 0, 0 };

        //public comandoInizialeSB Intestazione;
        public comandoRTC DatiRTCSB;
        //public DatiCliente CustomerData;
        //public ProgrammaRicarica ProgRicarica;
        //public MemoriaPeriodoLungo CicloLungo;
        //public MemoriaPeriodoBreve _CicloBreve;
        //public PacchettoReadMem _pacchettoMem;
        //public VariabiliSpybatt variabiliScheda;
        //public CalibrazioniSpybatt valoriCalibrazione;
        //public ImmagineDumpMem DumpMem;
        public StatoFirmware StatoFirmwareScheda;
        //public ComandoStrategia ComandoStrat;
        //public ParametriSpybatt ParametriGenerali;
        public OcBaudRate BrOCcorrente = OcBaudRate.OFF;
        public OcEchoMode EchoOCcorrente = OcEchoMode.OFF;

        public byte[] LLstopMessage;

       // public StatoSig60 StatoTrxOC;
        public byte ReserCounterOC;


        public byte _comandoInvio;
        public byte _pacchettoInviato;
        public byte _esitoPacchetto;
     //   public EsitoMessaggio EsitoComando;

        public int fwLevel = 0;

        private EsitoRisposta _ultimaRisposta = EsitoRisposta.MessaggioVuoto;

        public MessaggioLadeLight()
        {
            fwLevel = 0;
        }
    }
}
