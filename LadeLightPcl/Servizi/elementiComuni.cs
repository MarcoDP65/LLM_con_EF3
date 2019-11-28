using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ChargerLogic
{
    public class elementiComuni
    {
        public const int TimeoutBase  = 2;  // Durata di default del timeout di comunicazione, espressa in secondi/10.
        public const int TimeoutLungo = 5;  // Durata di default del timeout di comunicazione per le operazioni di lunga durata, espressa in secondi/10.
        public const int Timeout100ms = 1;  // Durata di default del timeout di comunicazione per le operazioni di lunga durata, espressa in secondi/10.
        public const int Timeout500ms = 5;  // Durata di default del timeout di comunicazione per le operazioni di lunga durata, espressa in secondi/10.
        public const int Timeout1sec = 10;  // Durata di default del timeout di comunicazione per le operazioni di lunga durata, espressa in secondi/10.
        public const int Timeout5sec = 50;  // Durata di default del timeout di comunicazione per le operazioni di lunga durata, espressa in secondi/10.

        public enum tipoMessaggio : int { vuoto = 0 ,
                                          MemLunga = 1,
                                          MemBreve = 2,
                                          Programmi = 3 ,
                                          DumpMemoria = 4,
                                          AggiornamentoFirmware = 5,
                                          ClonazioneScheda = 6,
                                          AggiornamentoFirmwareLL = 10,
                                          MemLungaLL = 11,
                                          MemBreveLL = 12,
                                          ContatoriLL = 13,
                                          ProgrammazioniLL = 14,
                                          AreaMemLungaLL = 15,
                                          AreaMemBreveLL = 16,
                                          NonDefinito = -1 };

        public enum contenutoMessaggio : int { vuoto = 0, Ack = 1, Nack = 2, Break = 3, Dati = 10, NonValido = -1 };
        public enum EsitoFunzione : byte { OK = 0x00, DatiNonVAlidi = 0x01,NonInSequenza = 0x02,ErroreGenerico = 0xFF}


        public enum TipoConnessione : byte { Nessuna = 0x00, Seriale = 0x10, Bluetooth = 0x11, USB = 0x20, WiFi = 0x40};


        public enum modoDati : byte { Import = 0x00,Output = 0x01, HexDumpRecovery = 0x02 };
        public enum AbilitaElemento : byte { Attivo = 0x0F , Non_Attivo = 0xF0 };

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

        /// <summary>
        /// Indicazione del verso corrente da impostare o verificare
        /// </summary>
        public enum VersoCorrente : byte { Diretto = 0x0F, Inverso = 0xF0, NonDefinito = 0x00 };

        public enum EsitoControlloValore : byte
        {
            EsitoPositivo = 0x00,
            NonEffettuato = 0xFF,
            IgnoraVerifica = 0xF1,
            AnnullaVerifica = 0xF2,
            Timeout = 0xF3,
            ErroreGenerico = 0xEE,
            CorrenteInversa = 0x01,
            AlimentatoreScollegato = 0x02,
            LetturaZero = 0x03,
            LetturaNonCoerente = 0x04,
            ErroreAlimentatore = 0x05,
            ErroreLetturaSB = 0x06,
            ErroreCorrente= 0x20,
            ErroreCorrentePositiva = 0x21,
            ErroreCorrenteNegativa = 0x22,
            ErroreTensioni = 0x40,
            ErroreV1 = 0x41,
            ErroreV2 = 0x42,
            ErroreV3 = 0x43,
            ErroreVBatt = 0x44,
            ErroreNTC = 0x45,
            ErroreVBk = 0x46,
            ErroreElettr = 0x47,
        };
    
        public class WaitEventStep : EventArgs
        {
            private int _numEventi;
            private int _evCorrente;
            private tipoMessaggio _TipoDati;
            private contenutoMessaggio _DatiRicevuti;

            public int Eventi
            {
                set
                {
                    _numEventi = value;
                }
                get
                {
                    return this._numEventi;
                }
            }

            public int Step
            {
                set
                {
                    _evCorrente = value;
                }
                get
                {
                    return this._evCorrente;
                }
            }

            public tipoMessaggio TipoDati
            {
                set
                {
                    _TipoDati = value;
                }
                get
                {
                    return this._TipoDati;
                }
            }

            public contenutoMessaggio DatiRicevuti
            {
                set
                {
                    _DatiRicevuti = value;
                }
                get
                {
                    return this._DatiRicevuti;
                }
            }

        }

        public class WaitStep
        {
            private int _numEventi;
            private int _evCorrente;
            private string _titolo;
            private bool _esecuzioneInterrotta;
            private tipoMessaggio _TipoDati;
            private contenutoMessaggio _DatiRicevuti;
            private int _numTentativi;

            public int Eventi
            {
                set
                {
                    _numEventi = value;
                }
                get
                {
                    return this._numEventi;
                }
            }

            public int NumTentativi
            {
                set
                {
                    _numTentativi = value;
                }
                get
                {
                    return this._numTentativi;
                }
            }

            public int Step
            {
                set
                {
                    _evCorrente = value;
                }
                get
                {
                    return this._evCorrente;
                }
            }

            public bool EsecuzioneInterrotta
            {
                set
                {
                    _esecuzioneInterrotta = value;
                }
                get
                {
                    return this._esecuzioneInterrotta;
                }
            }

            public string Titolo
            {
                set
                {
                    _titolo = value;
                }
                get
                {
                    return this._titolo;
                }
            }
            public tipoMessaggio TipoDati
            {
                set
                {
                    _TipoDati = value;
                }
                get
                {
                    return this._TipoDati;
                }
            }

            public contenutoMessaggio DatiRicevuti
            {
                set
                {
                    _DatiRicevuti = value;
                }
                get
                {
                    return this._DatiRicevuti;
                }
            }

        }

        public class EndStep
        {
            private int _numEventiPrev;
            private int _ultimoEvento;
            private double _secondiTot;
            private tipoMessaggio _TipoDati;
            private contenutoMessaggio _DatiRicevuti;

            // public enum tipoMessaggio : int { vuoto = 0, MemLunga = 1, MemBreve = 2, Programmi = 3 };
            // public enum contenutoMessaggio : int { vuoto = 0, Ack = 1, Dati = 2, Breack = 3, Nack = 10, NonValido = 99 };

            public int EventiPrevisti
            {
                set
                {
                    _numEventiPrev = value;
                }
                get
                {
                    return this._numEventiPrev;
                }
            }

            public int UltimoEvento
            {
                set
                {
                    _ultimoEvento = value;
                }
                get
                {
                    return this._ultimoEvento;
                }
            }

            public double SecondiElaborazione
            {
                set
                {
                    _secondiTot = value;
                }
                get
                {
                    return this._secondiTot;
                }
            }

            public tipoMessaggio TipoDati
            {
                set
                {
                    _TipoDati = value;
                }
                get
                {
                    return this._TipoDati;
                }
            }

            public contenutoMessaggio DatiRicevuti
            {
                set
                {
                    _DatiRicevuti = value;
                }
                get
                {
                    return this._DatiRicevuti;
                }
            }

        }



    }
}
