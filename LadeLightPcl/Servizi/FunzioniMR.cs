using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;


namespace Utility
{
    public class settings
    {
        public  enum LivelloUtente : byte { Factory = 0x00, 
                                                  Service = 0x01, 
                                                  PowerUser = 0x02,
                                                  User = 0x03,
                                                  NoUser = 0xFF};

        public enum TemperaturaAttiva : int { TMin = 0, TMax = 1, DifferenzaT = 2 };
    }

    public class FunzioniMR
    {

        public static string StringaTensione(uint Tensione)
        {
            try
            {
                string _tensioni = "";
                float _inVolt;
                _inVolt = (float)Tensione / 100;
                _tensioni = _inVolt.ToString("0.0");
                return _tensioni;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaTensioneCella(uint Tensione)
        {
            try
            {
                string _tensioni = "";
                float _inVolt;
                _inVolt = (float)Tensione / 100;
                _tensioni = _inVolt.ToString("0.0");
                return _tensioni;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaTensione(int Tensione)
        {
            try
            {
                string _tensioni = "";
                float _inVolt;
                _inVolt = (float)Tensione / 100;
                _tensioni = _inVolt.ToString("0.00");
                return _tensioni;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaTensionePerCella(uint Tensione, uint NumCelle)
        {
            try
            {
                string _tensioni = "";
                float _inVolt;
                uint _celleEff;

                // se le celle sono 0 non mostro nulla
                if (NumCelle > 0) _celleEff = NumCelle;
                else return "";

                _inVolt = (float)Tensione / (100 * _celleEff);
                _tensioni = _inVolt.ToString("0.00");
                return _tensioni;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaTensioneCella(float Tensione, bool BlankIfZero = false)
        {
            try
            {
                string _tensioni = "";
                if (BlankIfZero && Tensione == 0)
                {
                    // Non faccio nulla, lascio il blank
                }
                else
                {

                    float _inVolt;

                    _inVolt = (float)Tensione / 100;
                    _tensioni = _inVolt.ToString("0.00");
                }
                return _tensioni;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaCorrente(short Corrente, string Formato = "0.0")
        {
            try
            {
                string _correnti = "";
                float _inAmpere;
                _inAmpere = Math.Abs((float)Corrente / 10);
                _correnti = _inAmpere.ToString(Formato);
                return _correnti;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaCorrenteLL(ushort Corrente)
        {
            try
            {
                string _correnti = "";
                float _inAmpere;
                bool _valNeg = ((Corrente & 0x8000) == 0x8000);
                ushort _corrPos = (ushort)(Corrente & 0x7FFF);

                _inAmpere = Math.Abs((float)_corrPos / 10);
                if (_valNeg)
                {
                    _inAmpere *= -1;
                }

                _correnti = _inAmpere.ToString("0.0");
                return _correnti;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaCorrenteOLV(short Corrente)
        {
            try
            {
                string _correnti = "";
                float _inAmpere;
                _inAmpere = Math.Abs((float)Corrente / 10);
                _correnti = _inAmpere.ToString("0.0");
                if (Corrente < 0) _correnti = "-" + _correnti;
                return _correnti;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaCorrenteSigned(short Corrente)
        {
            try
            {
                string _correnti = "";
                float _inAmpere;
                _inAmpere = (float)Corrente / 10;
                _correnti = _inAmpere.ToString("0");
                return _correnti;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaTemperatura(byte Temperatura)
        {
            try
            {
                string _correnti = "";
                float _inGradi;
                _inGradi = Temperatura; // / 10;
                _correnti = _inGradi.ToString("0");
                return _correnti;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Ritorna una stringa di formattazione con un numero di decimali proporzionali al divisore paddato come parametro
        /// Es: 1000 --> "0.000"
        /// </summary>
        /// <param name="Valore"></param>
        /// <returns></returns>
        public static string StringaFattoreCarica(byte Valore)
        {
            try
            {
                string _risposta = "";
                float _inDecimale;
                // 28/07/2015  se RF è 0 il valore non è valido
                if (Valore == 0x00) return "N.D.";

                if (Valore == 0xFF) return ">2.5";

                _inDecimale = (float)Valore / 100;
                _risposta = _inDecimale.ToString("0.00");
                return _risposta;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaSoC(byte Valore)
        {
            try
            {
                string _risposta = "";

                //float _inDecimale;
                //_inDecimale = (float)RF / 100;

                _risposta = Valore.ToString("0") + "%";
                return _risposta;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaTemperatura(int Temperatura)
        {
            try
            {
                string _correnti = "";
                float _inGradi;
                _inGradi = Temperatura;
                _correnti = _inGradi.ToString("0");
                if (Temperatura < 150)
                    return _correnti;
                else
                    return "-";
            }
            catch
            {
                return "";
            }
        }

        public static string StringaByteTemp(byte Temperatura)
        {
            try
            {
                string _gradi = "";
                float _inGradi;
                sbyte _sigTemp = (sbyte)Temperatura;
                _inGradi = _sigTemp;
                _gradi = _inGradi.ToString();
                return _gradi;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaTimestamp(byte[] Dataora)
        {
            try
            {
                if (Dataora == null)
                {
                    return "N.D.";
                }
                else
                {
                    string _timestamp = "";
                    _timestamp += Dataora[0].ToString("00");
                    _timestamp += "/" + Dataora[1].ToString("00");
                    _timestamp += "/" + Dataora[2].ToString("00");
                    _timestamp += "  " + Dataora[3].ToString("00");
                    _timestamp += ":" + Dataora[4].ToString("00");
                    return _timestamp;
                }
            }
            catch
            {
                return "";
            }
        }

        public static string StringaDataTS(byte[] DataShort)
        {
            try
            {
                string _timestamp = "";
                short _tempAnno = 2000;
                _timestamp += DataShort[0].ToString("00");
                _timestamp += "/" + DataShort[1].ToString("00");
                _tempAnno += (short)DataShort[2];
                _timestamp += "/" + _tempAnno.ToString("0000");
                return _timestamp;
            }
            catch
            {
                return "";
            }
        }


        public static string StringaDataTS(uint DataShort)
        {
            try
            {
                byte d1 = 0;
                byte d2 = 0;
                byte d3 = 0;
                byte d4 = 0;

                FunzioniComuni.SplitUint32(DataShort, ref d1, ref d2, ref d3, ref d4);

                string _timestamp = "";
                short _tempAnno = 2000;
                _timestamp += d2.ToString("00");
                _timestamp += "/" + d3.ToString("00");
                _tempAnno += (short)d4;
                _timestamp += "/" + _tempAnno.ToString("0000");
                return _timestamp;
            }
            catch
            {
                return "";
            }
        }


        public static byte[] toArrayDataTS(string DataShort)
        {
            byte[] _arrayResult = new byte[3];
            _arrayResult[0] = 1;
            _arrayResult[1] = 1;
            _arrayResult[2] = 15;

            try
            {

                bool _esitoCast = false;
                DateTime _tempData = new DateTime();
                _esitoCast = DateTime.TryParse(DataShort, out _tempData);
                if (_esitoCast)
                {
                    _arrayResult[0] = (byte)_tempData.Day;
                    _arrayResult[1] = (byte)_tempData.Month;
                    _arrayResult[2] = (byte)(_tempData.Year - 2000);

                }

                return _arrayResult;
            }
            catch
            {
                return _arrayResult;
            }


        }

        public static string StringaDurata(uint Secondi)
        {
            try
            {
                string _tempo = "";
                TimeSpan t = TimeSpan.FromSeconds(Secondi);
                _tempo = string.Format("{0:D2}:{1:D2}", t.Hours, t.Minutes);
                //_tempo += "  / " + Secondi.ToString();
                return _tempo;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaDurataBase(uint Secondi)
        {
            try
            {
                string _tempo = "";
                TimeSpan t = TimeSpan.FromSeconds(Secondi);
                if (t.Days > 0)
                {
                    _tempo = t.Days.ToString() + "g ";
                }
                _tempo += string.Format("{0:D2}:{1:D2}", t.Hours, t.Minutes);
                //_tempo += "  / " + Secondi.ToString();
                return _tempo;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaDurataFull(double Secondi)
        {
            try
            {
                string _tempo = "";
                TimeSpan t = TimeSpan.FromSeconds(Secondi);
                if (t.Days > 0)
                {
                    _tempo = t.Days.ToString() + "g ";
                    // Se moto i giorni, comunque mostro le ore
                    _tempo = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
                }
                else
                {
                    //if (t.Hours > 0)
                    //{
                    _tempo = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
                    //}
                    //else
                    //{
                    //    _tempo = string.Format("{0:D2}:{1:D2}",  t.Minutes, t.Seconds);
                    //}

                }

                return _tempo;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaPresenza(byte Valore)
        {
            try
            {
                string _Flag = "";
                switch (Valore)
                {
                    case 0xF0:
                        _Flag = "OK";
                        break;
                    case 0x0F:
                        _Flag = "KO";
                        break;
                    default:
                        _Flag = Valore.ToString("x2");
                        break;
                }

                return _Flag;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaTipoEvento(int Valore)
        {
            try
            {
                string _Flag = "";
                switch (Valore)
                {
                    case 0xF0:
                        _Flag = "Carica";
                        break;
                    case 0x0F:
                        _Flag = "Scarica";
                        break;
                    case 0xAA:
                        _Flag = "Pausa";
                        break;
                    default:
                        _Flag = "Evento Anomalo (" + Valore.ToString("x2") + ")";
                        break;
                }

                return _Flag;
            }
            catch
            {
                return "";
            }
        }

        public static float ValoreEffettivo(int Valore, int Divisore = 1)
        {
            try
            {
                float _Result = 0;

                if (Divisore != 0)
                {
                    _Result = (float)Valore / (float)Divisore;
                    return _Result;
                }
                else
                    return (float)Valore;

            }
            catch
            {
                return 0;
            }
        }

        public static string StringaCapacita(int Capacita, int divisore = 1, byte Decimali = 1)
        {
            try
            {
                string _correnti = "";
                string _mask = "0.0";

                switch (Decimali)
                {
                    case 0:
                        _mask = "0";
                        break;
                    case 1:
                        _mask = "0.0";
                        break;
                    case 2:
                        _mask = "0.00";
                        break;
                    case 3:
                        _mask = "0.000";
                        break;
                    default:
                        _mask = "0.0";
                        break;
                }



                float _inGradi;
                if (divisore == 0) divisore = 1;
                _inGradi = (float)Capacita / (float)divisore;
                if (divisore != 1) _correnti = _inGradi.ToString(_mask);
                else _correnti = _inGradi.ToString("0");
                return _correnti;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaCapacitaUint(uint Capacita, int divisore = 1, byte Decimali = 1)
        {
            try
            {
                string _correnti = "";
                string _mask = "0.0";

                switch (Decimali)
                {
                    case 0:
                        _mask = "0";
                        break;
                    case 1:
                        _mask = "0.0";
                        break;
                    case 2:
                        _mask = "0.00";
                        break;
                    case 3:
                        _mask = "0.000";
                        break;
                    default:
                        _mask = "0.0";
                        break;
                }



                float _inGradi;
                if (divisore == 0) divisore = 1;
                _inGradi = (float)Capacita / (float)divisore;
                if (divisore != 1) _correnti = _inGradi.ToString(_mask);
                else _correnti = _inGradi.ToString("0");
                return _correnti;
            }
            catch
            {
                return "";
            }
        }

        public static string StringaPotenza(int Potenza, int divisore = 1, byte Decimali = 1)
        {
            try
            {
                string _correnti = "";
                string _mask = "0.0";

                switch (Decimali)
                {
                    case 0:
                        _mask = "0";
                        break;
                    case 1:
                        _mask = "0.0";
                        break;
                    case 2:
                        _mask = "0.00";
                        break;
                    case 3:
                        _mask = "0.000";
                        break;
                    default:
                        _mask = "0.0";
                        break;
                }

                float _inGradi;

                if (divisore == 0) divisore = 1;
                _inGradi = (float)Potenza / (float)(divisore);
                _correnti = _inGradi.ToString(_mask);
                return _correnti;


            }
            catch
            {
                return "";
            }
        }

        public static string StringaPotenza(UInt32 Potenza, int divisore = 1, byte Decimali = 1)
        {
            try
            {
                string _correnti = "";
                string _mask = "0.0";

                switch (Decimali)
                {
                    case 0:
                        _mask = "0";
                        break;
                    case 1:
                        _mask = "0.0";
                        break;
                    case 2:
                        _mask = "0.00";
                        break;
                    case 3:
                        _mask = "0.000";
                        break;
                    default:
                        _mask = "0.0";
                        break;
                }

                float _inGradi;


                if (divisore == 0) divisore = 1;
                _inGradi = (float)Potenza / (float)(1000 * divisore);
                _correnti = _inGradi.ToString(_mask);
                return _correnti;
            }
            catch
            {
                return "";
            }
        }

        public static string CompletaAZero(string StringaBase, int LunghezzaFinale)
        {


            try
            {
                string _strBuffer = StringaBase;
                int _strLen = StringaBase.Length;
                int _mancanti = LunghezzaFinale - _strLen;
                if (_mancanti > 0)
                {

                    for (int _a = 0; _a < _mancanti; _a++)
                    {
                        _strBuffer += "\0";
                    }

                }
                return _strBuffer;

            }
            catch
            {
                return "";
            }

        }

        public static string StringaModelloDivisore(ushort Divisore)
        {
            string _maschera = "0.";

            try
            {

                string _strDiv = Divisore.ToString();
                int _decimali = _strDiv.Length - 1;
                if (_decimali < 0)
                    _decimali = 1;


                if (_decimali > 0)
                {
                    _maschera = "0.";

                    for (int _a = 0; _a < _decimali; _a++)
                    {
                        _maschera += "0";
                    }

                    return _maschera;
                }
                else
                    return "";

            }
            catch
            {
                return "";
            }
        }

        public static string StringaMax(string Testo, int Caratteri)
        {
            try
            {
                if (Testo == null)
                {
                    return "";
                }
                if (Testo.Length <= Caratteri) return Testo;
                else return Testo.Substring(0, Caratteri);
            }
            catch
            {
                return "";
            }
        }

        public static string StringaSeriale(string Codice)
        {
            try
            {
                if (Codice == null)
                    return "";

                if (Codice.Length == 16)
                {
                    string _tempTesto = "";
                    _tempTesto += Codice.Substring(0, 4);
                    _tempTesto += ":" + Codice.Substring(4, 4);
                    _tempTesto += ":" + Codice.Substring(8, 4);
                    _tempTesto += ":" + Codice.Substring(12, 4);

                    return _tempTesto;
                }
                else
                {
                    return Codice;
                }

            }
            catch
            {
                return "";
            }
        }

        public static string StringaRevisione(string Codice)
        {
            try
            {
                if (Codice == null) return "";

                if (Codice.Length == 6)
                {
                    string _tempTesto = "";
                    _tempTesto += Codice.Substring(0, 4);
                    _tempTesto += "." + Codice.Substring(4, 2);

                    return _tempTesto;
                }
                else
                {
                    return Codice;
                }

            }
            catch
            {
                return "";
            }
        }

        public static string hexdumpArray(byte[] buffer, bool SeparaBytes = false)
        {
            try
            {
                string _risposta = "";

                for (int _i = 0; _i < buffer.Length; _i++)
                {
                    _risposta += buffer[_i].ToString("X2");
                    if (SeparaBytes & _i < (buffer.Length - 1))
                    {
                        _risposta += " ";
                    }
                }
                return _risposta;
            }
            catch
            {
                return "";
            }
        }

        public static ushort ConvertiUshort(string Testo, int Fattore = 1, ushort ValIfNull = 0)
        {
            try
            {
                ushort _valore;
                float number;
                bool result = float.TryParse(Testo, out number);
                if (result)
                {
                    _valore = (ushort)(number * Fattore);
                }
                else
                {
                    _valore = ValIfNull;
                }

                return _valore;

            }
            catch
            {
                return ValIfNull;
            }

        }

        public static ushort ConvertiHexUshort(string Testo, ushort ValIfNull = 0)
        {
            try
            {
                ushort _valore;

                bool result = ushort.TryParse(Testo, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _valore);
                
                if (!result)
                {
                    _valore = ValIfNull;
                }

                return _valore;

            }
            catch
            {
                return ValIfNull;
            }

        }


        public static ushort ConvertiMSshort(string Testo, int Fattore = 1, ushort ValIfNull = 0)
        {
            try
            {
                ushort _valore;
                float number;
                float _tempValue;
                ushort _maskNegative = 0x8000;
                bool result = float.TryParse(Testo, out number);
                if (result)
                {
                    _tempValue = Math.Abs(number);
                    _valore = (ushort)(_tempValue * Fattore);
                    if (number < 0)
                    {
                        _valore = (ushort)(_valore | _maskNegative);
                    }
                }
                else
                {
                    _valore = ValIfNull;
                }

                return _valore;

            }
            catch
            {
                return ValIfNull;
            }

        }

        public static byte ConvertiByte(string Testo, int Fattore = 1, byte ValIfNull = 0,byte ValMax = 255)
        {
            try
            {
                byte _valore;
                float number;
                bool result = float.TryParse(Testo, out number);
                if (result)
                {
                    _valore = (byte)(number * Fattore);
                }
                else
                {
                    _valore = ValIfNull;
                }


                if (_valore > ValMax) _valore = ValMax;


                return _valore;

            }
            catch
            {
                return ValIfNull;
            }

        }

        public static StruttureBase.ArrayCelle CalcolaCelleRelative(StruttureBase.ArrayCelle CelleBase)
        {
            try
            {
                //se non passo le celle base,esco
                if (CelleBase == null) return null;

                StruttureBase.ArrayCelle _CelleRelative = new StruttureBase.ArrayCelle();
                byte _cellePrecedenti = 0;

                // pos 1/4
                _CelleRelative.C1 = CelleBase.C1;
                _cellePrecedenti = CelleBase.C1;
                // pos 2/4
                if (CelleBase.C2 > 0)
                {
                    _CelleRelative.C2 = (byte)(CelleBase.C2 - _cellePrecedenti);
                    _cellePrecedenti = CelleBase.C2;
                }
                // pos 3/4
                if (CelleBase.C3 > 0)
                {
                    _CelleRelative.C3 = (byte)(CelleBase.C3 - _cellePrecedenti);
                    _cellePrecedenti = CelleBase.C3;
                }

                // pos 4/4 - Batteria

                if (CelleBase.Cbatt > 0)
                {
                    _CelleRelative.Cbatt = (byte)(CelleBase.Cbatt - _cellePrecedenti);
                    _cellePrecedenti = CelleBase.Cbatt;
                }
                else
                {
                    _CelleRelative.Cbatt = 1;
                }

                return _CelleRelative;
            }
            catch
            {
                return null;
            }

        }

        public static DateTime TimespanToDatetime(TimeSpan Intervallo)
        {
            try
            {
                DateTime _valore; // = new DateTime(1,1,1);
                                  //_valore.Add( Intervallo );

                _valore = Convert.ToDateTime(Intervallo.ToString());
                return _valore;

            }
            catch
            {
                DateTime _valore;
                DateTime.TryParse("01/01/2000 00:00", out _valore);
                return _valore;
            }
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static int SBFwLevel(string NumVersione)
        {

            try
            {
                string _LocalVer = ""; // _sb.SwVersion.Substring(0, 4);  // controllo solo i primi 4 caratteri della versione perignorare la build 

                if (NumVersione == null) return 11; // da -1 a 4 per Marco 

                if (NumVersione.Length >= 4) _LocalVer = NumVersione.Substring(0, 4);

                switch (_LocalVer)
                {
                    case "1.04":
                    case "1.05":
                    case "1.06":
                        return 0;
                    // break; 

                    case "1.07":
                        return 1;
                    // break; 

                    case "1.08":
                        if (NumVersione == "1.08.01")
                            return 2;
                        else
                            return 3;
                    // break; 
                    case "1.09":
                        return 3;

                    case "1.10":
                    case "1.11":
                    case "1.12":
                    case "1.13":
                    case "2.01":
                        return 4;
                    case "2.02":
                        {
                            switch (NumVersione)
                            {
                                case "2.02.01":
                                case "2.02.02":
                                case "2.02.03":
                                    return 4;
                                case "2.02.04":
                                case "2.02.05":
                                case "2.02.06":
                                case "2.02.07":
                                case "2.02.08":
                                    return 5;
                                default:
                                    return 5;
                            }

                        }
                    case "2.03":
                        switch (NumVersione)
                        {
                            case "2.03.05":
                            case "2.03.06":
                                return 7;
                            default:
                                return 6;
                        }


                    case "2.04":
                        switch (NumVersione)
                        {

                            case "2.04.06":
                                return 9;
                            case "2.04.07":
                            case "2.04.08":
                                return 10;
                            case "2.04.10":
                                return 11;
                            default:
                                return 8;
                        }

                    case "3.00":
                    case "3.01":
                        {
                            return 11;
                        }


                    default:
                        //variante per marco 
                        //return -1; 
                        return 11;
                        //  break; 
                }
            }
            catch
            {
                return -1;
            }


        }

        /// <summary>
        /// Verifica se la stringa lotto è formattata correttamente.
        /// formato previsto:  dddd/AA/T/nnn
        /// dove dddd è il ddt di trasporto
        ///        AA è l'anno di riferimento
        ///         T è il codice identificativo del terzista
        ///       nnn è il progressivo del pezzo all'interno del lotto 
        /// I blocchi sono separati dal carattere "/"      
        /// 
        /// </summary>
        /// <param name="IdLotto">The identifier lotto.</param>
        /// <returns>True se OK</returns>
        public static bool VerificaStringaLottoZVT(String IdLotto)
        {
            try
            {
                ushort _valore;

                if (IdLotto.Length != 13) return false;
                if (IdLotto.Substring(4, 1) != "/") return false;
                if (IdLotto.Substring(7, 1) != "/") return false;
                if (IdLotto.Substring(9, 1) != "/") return false;
                if (!(ushort.TryParse(IdLotto.Substring(0, 4), out _valore))) return false;
                if (!(ushort.TryParse(IdLotto.Substring(5, 2), out _valore))) return false;
                if (!(ushort.TryParse(IdLotto.Substring(10, 3), out _valore))) return false;
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static byte[] CodificaStringaLottoZVT(String IdLotto)
        {
            byte[] _tempCodice = new byte[6];
            try
            {
                ushort _valoreUS;
                byte _valoreB;
                byte _loVal = 0;
                byte _hiVal = 0;

                for (int _i = 0; _i < 6; _i++)
                {
                    _tempCodice[_i] = 0;
                }

                if (!VerificaStringaLottoZVT(IdLotto)) return _tempCodice;
                // Documento
                if (ushort.TryParse(IdLotto.Substring(0, 4), out _valoreUS))
                {
                    FunzioniComuni.SplitUshort(_valoreUS, ref _loVal, ref _hiVal);
                    _tempCodice[0] = _hiVal;
                    _tempCodice[1] = _loVal;
                }
                // Anno
                if (byte.TryParse(IdLotto.Substring(5, 2), out _loVal))
                {
                    _tempCodice[2] = _loVal;
                }
                // Terzista
                _valoreB = (byte)IdLotto[8];
                _tempCodice[3] = _valoreB;

                // Progressivo
                if (ushort.TryParse(IdLotto.Substring(10, 3), out _valoreUS))
                {
                    FunzioniComuni.SplitUshort(_valoreUS, ref _loVal, ref _hiVal);
                    _tempCodice[4] = _hiVal;
                    _tempCodice[5] = _loVal;
                }

                return _tempCodice;
            }
            catch
            {

                for (int _i = 0; _i < 6; _i++)
                {
                    _tempCodice[_i] = 0;
                }
                return _tempCodice;
            }


        }

        public static string DecodificaStringaLottoZVT(byte[] IdLotto)
        {
            string _tempStr = "";
            try
            {
                ushort _valoreUS;
                byte _valoreB;
                byte _loVal = 0;
                byte _hiVal = 0;

                int CheckSum = 0;

                for (int _i = 0; _i < 6; _i++)
                {
                    CheckSum += IdLotto[_i];
                }

                if (CheckSum == 0x05FA) return _tempStr;
                // Documento
                _valoreUS = (ushort)((IdLotto[0] << 8) + IdLotto[1]);
                _tempStr += _valoreUS.ToString("0000");
                _tempStr += "/";
                // Anno
                _tempStr += IdLotto[2].ToString("00");
                _tempStr += "/";
                // Terzista
                _tempStr += (char)(IdLotto[3]);
                _tempStr += "/";
                // Progressivo
                _valoreUS = (ushort)((IdLotto[4] << 8) + IdLotto[5]);
                _tempStr += _valoreUS.ToString("000");


                return _tempStr;
            }
            catch
            {
                return "";
            }


        }




    }

    public class tensioniIntermedie
    {
        public StruttureBase.ArrayTensioniUS TensioniBase;
        public StruttureBase.ArrayCelle CelleBase;
        public StruttureBase.ArrayTensioniStr strTensioniCellaAssolute;
        public StruttureBase.ArrayTensioniStr strTensioniCellaRelative;
        public StruttureBase.ArrayTensioniF TensioniCellaAssolute;
        public StruttureBase.ArrayTensioniF TensioniCellaRelative;
        public StruttureBase.ArrayCelle CelleRelative;
        public StruttureBase.ArrayTensioniUS TensioniRelative;
        

        public tensioniIntermedie()
        {
            strTensioniCellaAssolute = new StruttureBase.ArrayTensioniStr();
            strTensioniCellaRelative = new StruttureBase.ArrayTensioniStr();

            TensioniCellaAssolute = new StruttureBase.ArrayTensioniF();
            TensioniCellaRelative = new StruttureBase.ArrayTensioniF();

            TensioniRelative = new StruttureBase.ArrayTensioniUS() ;
        }


        public bool CalcolaIntermedie(StruttureBase.ArrayTensioniUS Tensioni, StruttureBase.ArrayCelle Celle, StruttureBase.ArrayCelle Relative)
        {
            try
            {
                TensioniBase = Tensioni;
                CelleBase = Celle;
                CelleRelative = Relative;

                return CalcolaIntermedie();
            }
            catch
            {
                return false;
            }

        }

        public bool CalcolaIntermedie()
        {
            try
            {
                byte _cellePrecedenti = 0;
                ushort _voltPrecedenti = 0;

                if (CelleBase.CelleCaricate())
                {

#region "Tensioni Assolute"
                    // pos 1/4
                    if (CelleBase.C1 > 0)
                    {
                        TensioniCellaAssolute.V1 = (float)TensioniBase.V1 / (float)CelleBase.C1;
                        strTensioniCellaAssolute.V1 = FunzioniMR.StringaTensioneCella(TensioniCellaAssolute.V1);
                    }
                    else
                    {
                        TensioniCellaAssolute.V1 = 0;
                        strTensioniCellaAssolute.V1 = "";
                    }

                    // pos 2/4
                    if (CelleBase.C2 > 0)
                    {
                        TensioniCellaAssolute.V2 = (float)TensioniBase.V2 / (float)CelleBase.C2;
                        strTensioniCellaAssolute.V2 = FunzioniMR.StringaTensioneCella(TensioniCellaAssolute.V2);
                    }
                    else
                    {
                        TensioniCellaAssolute.V2 = 0;
                        strTensioniCellaAssolute.V2 = "";
                    }

                    // pos 3/4
                    if (CelleBase.C3 > 0)
                    {
                        TensioniCellaAssolute.V3 = (float)TensioniBase.V3 / (float)CelleBase.C3;
                        strTensioniCellaAssolute.V3 = FunzioniMR.StringaTensioneCella(TensioniCellaAssolute.V3);
                    }
                    else
                    {
                        TensioniCellaAssolute.V3 = 0;
                        strTensioniCellaAssolute.V3 = "";
                    }

                    // pos 4/4 - Batteria
                    if (CelleBase.Cbatt > 0)
                    {
                        TensioniCellaAssolute.Vbatt = (float)TensioniBase.Vbatt / (float)CelleBase.Cbatt;
                        strTensioniCellaAssolute.Vbatt = FunzioniMR.StringaTensioneCella(TensioniCellaAssolute.Vbatt);
                    }
                    else
                    {
                        TensioniCellaAssolute.Vbatt = 0;
                        strTensioniCellaAssolute.Vbatt = "";
                    }
#endregion

#region "Tensioni Relative"
                    // pos 1/4
                    //CelleRelative.C1 = (byte)(CelleBase.C1 - _cellePrecedenti);
                    TensioniRelative.V1 =(ushort)(TensioniBase.V1 - _voltPrecedenti);
                    if (CelleRelative.C1 > 0)
                    {
                        TensioniCellaRelative.V1 = (float)(TensioniRelative.V1) / (float)(CelleRelative.C1);
                        strTensioniCellaRelative.V1 = FunzioniMR.StringaTensioneCella(TensioniCellaRelative.V1);
                        _cellePrecedenti = CelleBase.C1;
                        _voltPrecedenti = TensioniBase.V1;

                    }
                    else
                    {
                        TensioniRelative.V1 = 0;
                        strTensioniCellaRelative.V1 = "";
                    }

                    // pos 2/4
                    //CelleRelative.C2 = (byte)(CelleBase.C2 - _cellePrecedenti);
                    TensioniRelative.V2 = (ushort)(TensioniBase.V2 - _voltPrecedenti);
                    if (CelleRelative.C2 > 0)
                    {
                        TensioniCellaRelative.V2 = (float)(TensioniRelative.V2) / (float)(CelleRelative.C2);
                        strTensioniCellaRelative.V2 = FunzioniMR.StringaTensioneCella(TensioniCellaRelative.V2);
                        _cellePrecedenti = CelleBase.C2;
                        _voltPrecedenti = TensioniBase.V2;

                    }
                    else
                    {
                        TensioniRelative.V2 = 0;
                        strTensioniCellaRelative.V2 = "";
                    }

                    // pos 3/4
                    //CelleRelative.C3 = (byte)(CelleBase.C3 - _cellePrecedenti);
                    TensioniRelative.V3 = (ushort)(TensioniBase.V3 - _voltPrecedenti);
                    if (CelleRelative.C3 > 0)
                    {
                        TensioniCellaRelative.V3 = (float)(TensioniRelative.V3) / (float)(CelleRelative.C3);
                        strTensioniCellaRelative.V3 = FunzioniMR.StringaTensioneCella(TensioniCellaRelative.V3);
                        _cellePrecedenti = CelleBase.C3;
                        _voltPrecedenti = TensioniBase.V3;

                    }
                    else
                    {
                        TensioniRelative.V3 = 0;
                        strTensioniCellaRelative.V3 = "";
                    }

                    // pos 4/4 - Batteria
                    //CelleRelative.Cbatt = (byte)(CelleBase.Cbatt - _cellePrecedenti);
                    TensioniRelative.Vbatt = (ushort)(TensioniBase.Vbatt - _voltPrecedenti);
                    if (CelleRelative.Cbatt > 0)
                    {
                        TensioniCellaRelative.Vbatt = (float)(TensioniRelative.Vbatt) / (float)(CelleRelative.Cbatt);
                        strTensioniCellaRelative.Vbatt = FunzioniMR.StringaTensioneCella(TensioniCellaRelative.Vbatt);
                        _cellePrecedenti = CelleBase.Cbatt;
                        _voltPrecedenti = TensioniBase.Vbatt;

                    }
                    else
                    {
                        TensioniRelative.Vbatt = 0;
                        strTensioniCellaRelative.Vbatt = "";
                    }
                    #endregion

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
       
        
        }


        float sbilaciamento(float Val1, float Val2)
        {
            try
            {
                if ((Val1 == 0) || (Val2 == 0)) return 0;
                if (Val1 > Val2)
                {
                    return (Val1 - Val2);
                }
                else
                {
                    return (Val2 - Val1);
                }
            }
            catch
            {
                return 0;
            }
        }

        public float MassimoSbilanciamento(StruttureBase.ArrayTensioniF TensioniCella)
        {
            try
            {
                float _maxSbil = 0;
                float _tempSbil;

                // V1
                _tempSbil = sbilaciamento(TensioniCella.V1, TensioniCella.V2);
                if (_tempSbil > _maxSbil) _maxSbil = _tempSbil;

                _tempSbil = sbilaciamento(TensioniCella.V1, TensioniCella.V3);
                if (_tempSbil > _maxSbil) _maxSbil = _tempSbil;

                _tempSbil = sbilaciamento(TensioniCella.V1, TensioniCella.Vbatt);
                if (_tempSbil > _maxSbil) _maxSbil = _tempSbil;

                // V2
                _tempSbil = sbilaciamento(TensioniCella.V2, TensioniCella.V3);
                if (_tempSbil > _maxSbil) _maxSbil = _tempSbil;

                _tempSbil = sbilaciamento(TensioniCella.V2, TensioniCella.Vbatt);
                if (_tempSbil > _maxSbil) _maxSbil = _tempSbil;


                // V3
                _tempSbil = sbilaciamento(TensioniCella.V3, TensioniCella.Vbatt);
                if (_tempSbil > _maxSbil) _maxSbil = _tempSbil;
                return _maxSbil;

            }
            catch
            {
                return 0;
            }
        }
    
    }

    public class StruttureBase
    {
        public class ArrayTensioniF
        {
            public float V1;
            public float V2;
            public float V3;
            public float Vbatt;

            public ArrayTensioniF()
            {
                V1 = 0;
                V2 = 0;
                V3 = 0;
                Vbatt = 0;
            }        
        }

        public class ArrayTensioniUS
        {
            public ushort V1;
            public ushort V2;
            public ushort V3;
            public ushort Vbatt;
        }

        public class ArrayTensioniStr
        {
            public String V1;
            public String V2;
            public String V3;
            public String Vbatt;

            public ArrayTensioniStr()
            {
                V1 = "";
                V2 = "";
                V3 = "";
                Vbatt = "";
            }
        }

        public class ArrayCelle
        {
            public byte C1;
            public byte C2;
            public byte C3;
            public byte Cbatt;

            public bool CelleCaricate()
            {
                if (C1 + C2 + C3 + Cbatt > 0) return true;
                else return false;
            }

        }
    }

    public class FunzioniAnalisi
    {
        public const float FattoreBaseSOH = 1315;

        /// <summary>
        /// Determino il peso della scarica sul ciclo vita della batteria in funzione delle temperatura
        /// </summary>
        /// <param name="Temperatura"></param>
        /// <returns></returns>
        public static double FattoreTermicoSOH ( double Temperatura )
        {
            try
            {
                double _esito;
                // Fino a 25 il fattore 
                if (Temperatura < 25)
                    return 1;

                if ((Temperatura >= 25) & (Temperatura < 35))
                {
                    _esito = 100 - ((Temperatura - 25) * 4);
                    return ( 100 / _esito );
                }
                if ((Temperatura >= 35) & (Temperatura < 60))
                {
                    _esito = 60 - ((Temperatura - 35) * 2);
                    return (100 / _esito);
                }
                if (Temperatura >= 60)
                    return 10;

                return 1;

            }
            catch
            {
                return 1;
            }

        }
    }

    public class DataSplitter
    {
        /// <summary>
        /// Suddivide il valore passato (Unsigned Short) in 2 byte
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_loVal"></param>
        /// <param name="_hiVal"></param>
        public static void splitUshort(ushort _value, ref byte _loVal, ref byte _hiVal)
        {
            _loVal = (byte)(_value & 0xFFu);
            _hiVal = (byte)((_value >> 8) & 0xFFu);
        }

        /// <summary>
        /// Suddivide il valore passato (Unsigned Integer32) in 4 byte
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_byte1"></param>
        /// <param name="_byte2"></param>
        /// <param name="_byte3"></param>
        /// <param name="_byte4"></param>
        public static void splitUint32(UInt32 _value, ref byte _byte1, ref byte _byte2, ref byte _byte3, ref byte _byte4)
        {
            _byte4 = (byte)(_value & 0x000000FF);
            _byte3 = (byte)((_value >> 8) & 0x000000FF);
            _byte2 = (byte)((_value >> 16) & 0x000000FF);
            _byte1 = (byte)((_value >> 24) & 0x000000FF);
        }

        /// <summary>
        /// Suddivide il valore passato (Integer32) in 4 byte
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_byte1"></param>
        /// <param name="_byte2"></param>
        /// <param name="_byte3"></param>
        /// <param name="_byte4"></param>
        public static void splitInt32(Int32 _value, ref byte _byte1, ref byte _byte2, ref byte _byte3, ref byte _byte4)
        {
            _byte4 = (byte)(_value & 0x000000FF);
            _byte3 = (byte)((_value >> 8) & 0x000000FF);
            _byte2 = (byte)((_value >> 16) & 0x000000FF);
            _byte1 = (byte)((_value >> 24) & 0x000000FF);
        }

    }

    public static class FunzioniComuni
    {
        /// <summary>
        /// Codifica (byte) il carattere in posizione 'Posizione' della stringa; se oltre la lunghezza effettiva ritorna - codificato - il valore 'valoreNullo'
        /// </summary>
        /// <param name="Testo">Stringa da cui estrarre il carattere</param>
        /// <param name="Posizione">posizione a base 0 del carattere da estrarre</param>
        /// <param name="valoreNullo">valure da forzare se oltre la lunghezza della stringa</param>
        /// <returns>valore codificato su 2 bytes del carattere</returns>
        public static byte ByteSubString(string Testo, int Posizione, byte valoreNullo = 0x00)
        {
            byte _tempbyte = valoreNullo;

            try
            {
                if (Testo == null)
                {
                    _tempbyte = valoreNullo;
                }
                else
                {
                    if (Posizione < Testo.Length)
                    {
                        _tempbyte = (byte)Testo[Posizione];
                    }
                    else
                    {
                        _tempbyte = valoreNullo;
                    }
                }
                return _tempbyte;
            }
            catch (Exception Ex)
            {

                return 0xFF;
            }
        }

        /// <summary>
        /// Crea una stringa con la rappresentazione esadecimaledell'array passato
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="SplitByte">Se True separa i bytes con uno spazio</param>
        /// <returns></returns>
        public static string HexdumpArray(byte[] buffer, bool SplitByte = false, bool ModoArray = false)
        {
            try
            {
                string _risposta = "";
                if (buffer != null)
                {
                    for (int _i = 0; _i < buffer.Length; _i++)
                    {
                        if (ModoArray) _risposta += "0x";
                        _risposta += buffer[_i].ToString("X2");
                        if (ModoArray & (_i != (buffer.Length - 1))) _risposta += ",";
                        if (SplitByte & (_i != (buffer.Length - 1))) _risposta += " ";
                    }
                }
                return _risposta;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Crea una stringa con la rappresentazione esadecimale della coda passata 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="SplitByte">Se True separa i bytes con uno spazio</param>
        /// <returns></returns>
        public static string HexdumpQueue(Queue<byte> buffer, bool SplitByte = false)
        {
            try
            {
                string _risposta = "";

                for (int _i = 0; _i < buffer.Count; _i++)
                {
                    _risposta += buffer.ElementAt(_i).ToString("X2");
                    if (SplitByte & (_i != (buffer.Count - 1))) _risposta += " ";
                }
                return _risposta;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Suddivide il valore passato (Unsugned Short) in 2 byte
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_loVal"></param>
        /// <param name="_hiVal"></param>
        public static void SplitUshort(ushort _value, ref byte _loVal, ref byte _hiVal)
        {
            _loVal = (byte)(_value & 0xFFu);
            _hiVal = (byte)((_value >> 8) & 0xFFu);
        }

        /// <summary>
        /// Suddivide il valore passato ( Short) in 2 byte
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_loVal"></param>
        /// <param name="_hiVal"></param>
        public static void SplitShort(short _value, ref byte _loVal, ref byte _hiVal)
        {
            _loVal = (byte)(_value & 0xFFu);
            _hiVal = (byte)((_value >> 8) & 0xFFu);
        }

        /// <summary>
        /// Suddivide il valore passato uint32 nei 4 byte costituenti.
        /// </summary>
        /// <param name="_value">Valore da convertire.</param>
        /// <param name="_byte1">byte1.</param>
        /// <param name="_byte2">byte2.</param>
        /// <param name="_byte3">byte3.</param>
        /// <param name="_byte4">byte4.</param>
        public static void SplitUint32(UInt32 _value, ref byte _byte1, ref byte _byte2, ref byte _byte3, ref byte _byte4)

        {
            _byte4 = (byte)(_value & 0x000000FF);
            _byte3 = (byte)((_value >> 8) & 0x000000FF);
            _byte2 = (byte)((_value >> 16) & 0x000000FF);
            _byte1 = (byte)((_value >> 24) & 0x000000FF);
        }

        public static void SplitInt32(Int32 _value, ref byte _byte1, ref byte _byte2, ref byte _byte3, ref byte _byte4)

        {
            _byte4 = (byte)(_value & 0x000000FF);
            _byte3 = (byte)((_value >> 8) & 0x000000FF);
            _byte2 = (byte)((_value >> 16) & 0x000000FF);
            _byte1 = (byte)((_value >> 24) & 0x000000FF);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_byte1"></param>
        /// <param name="_byte2"></param>
        /// <param name="_byte3"></param>
        /// <param name="_byte4"></param>
        /// <param name="PosSegno">indica su quale byte MSB mettere il bit di segno 0(lsb) - 3(msb)</param>
        public static void SplitSInt32(Int32 value, ref byte _byte1, ref byte _byte2, ref byte _byte3, ref byte _byte4, byte PosSegno = 3)

        {
            Int32 _TempVal = Math.Abs(value);
            _byte4 = (byte)(_TempVal & 0x000000FF);
            _byte3 = (byte)((_TempVal >> 8) & 0x000000FF);
            _byte2 = (byte)((_TempVal >> 16) & 0x000000FF);
            _byte1 = (byte)((_TempVal >> 24) & 0x000000FF);
            if (value < 0)
            {
                switch (PosSegno)
                {
                    case 3:
                        _byte1 = (byte)(_byte1 | 0x80);
                        break;
                    case 2:
                        _byte2 = (byte)(_byte2 | 0x80);
                        break;
                    case 1:
                        _byte3 = (byte)(_byte3 | 0x80);
                        break;
                    case 0:
                        _byte4 = (byte)(_byte4 | 0x80);
                        break;

                }
            }

        }

        public static void SplitSShort(short value, ref byte _byte1, ref byte _byte2)

        {
            short _TempVal = Math.Abs(value);
            _byte2 = (byte)(_TempVal & 0x00FF);
            _byte1 = (byte)((_TempVal >> 8) & 0x00FF);

            if (value < 0)
            {
                _byte1 = (byte)(_byte1 | 0x80);
            }

        }

        /// <summary>
        /// Converte la data/ora passata come argomento in un array di bytes
        /// </summary>
        /// <param name="Timestamp"></param>
        /// <returns></returns>
        public static byte[] splitDatetime(DateTime Timestamp)
        {
            byte[] _tempArray = new byte[6];
            _tempArray[0] = 0;  // anno
            _tempArray[1] = 0;  // mese
            _tempArray[2] = 0;  // giorno
            _tempArray[3] = 0;  // ore
            _tempArray[4] = 0;  // minuti
            _tempArray[5] = 0;  // secondi

            if (Timestamp != null)
            {
                short _tempyear = (short)(Timestamp.Year - 2000);
                if (_tempyear > 200 | _tempyear < 0)
                    _tempyear = 0;

                _tempArray[0] = (byte)_tempyear;
                _tempArray[1] = (byte)Timestamp.Month;
                _tempArray[2] = (byte)Timestamp.Day;
                _tempArray[3] = (byte)Timestamp.Hour;
                _tempArray[4] = (byte)Timestamp.Minute;
                _tempArray[5] = (byte)Timestamp.Second;


            }
            return _tempArray;
        }

        /// <summary>
        /// Converte data ora in formato MR in bute array
        /// </summary>
        /// <param name="Timestamp">Stringa Timestamp: deve essere in formato 'dd/mm/yy' o 'dd/mm/yy hh/mm'</param>
        /// <returns></returns>
        public static byte[] splitStringaTS(string Timestamp)
        {
            byte[] _tempArray = new byte[6];
            _tempArray[0] = 0;  // anno
            _tempArray[1] = 0;  // mese
            _tempArray[2] = 0;  // giorno
            _tempArray[3] = 0;  // ore
            _tempArray[4] = 0;  // minuti
            _tempArray[5] = 0;  // secondi

            if (Timestamp != null)
            {
                if (Timestamp.Length == 15)
                {   // data/ora
                    Byte.TryParse(Timestamp.Substring(0, 2), out _tempArray[0]); // anno
                    Byte.TryParse(Timestamp.Substring(3, 2), out _tempArray[1]); // mese
                    Byte.TryParse(Timestamp.Substring(6, 2), out _tempArray[2]); // giorno
                    Byte.TryParse(Timestamp.Substring(10, 2), out _tempArray[3]); // ore
                    Byte.TryParse(Timestamp.Substring(13, 2), out _tempArray[4]); // minuti
                }

                if (Timestamp.Length == 8)
                {   // solo data
                    Byte.TryParse(Timestamp.Substring(0, 2), out _tempArray[0]); // anno
                    Byte.TryParse(Timestamp.Substring(3, 2), out _tempArray[1]); // mese
                    Byte.TryParse(Timestamp.Substring(6, 2), out _tempArray[2]); // giorno
                }
            }

            return _tempArray;
        }

        public static byte[] StringToArray(string source, int ArrayLen, int Start = 0)
        {
            string _tempString = "";

            try
            {
                byte[] _tempBuf = new byte[ArrayLen + Start];
                for (int _i = 0; _i < Start; _i++)
                {
                    _tempBuf[_i] = 0x00;
                }
                Encoding enc = Encoding.GetEncoding("ASCII");

                byte[] _tempData = enc.GetBytes(source);
                for (int _i = Start; _i < ArrayLen; _i++)
                {
                    if (_i < _tempData.Length)
                        _tempBuf[_i] = _tempData[_i];
                    else
                        _tempBuf[_i] = 0x00;

                }
                return _tempBuf;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Convert Hexadecimals string to array.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="ArrayLen">Length of the array.</param>
        /// <param name="Start">The start.</param>
        /// <returns> Converte la stringa nel valore esadecimale rappresentato allineando il valore a destra e riempiendo </returns>
        public static byte[] HexStringToArray(string source, int ArrayLen)
        {
            string _tempString ;
           
            try
            {

                _tempString = "";
                byte[] _tempBuf = new byte[ArrayLen];
                byte _tempValue;

                     // Preparo l'array esadecimale inizializzato a 0
                for (int _i = 0; _i < ArrayLen; _i++)
                {
                    _tempBuf[_i] = 0x00;
                    _tempString += "00";
                }
           
                // ora carico i bytes, scorrrendo la stringa da dx verso sx 2 caratteri alla volta
                _tempString += source;
                int sourceLen = _tempString.Length;

                string TempByte;
                // la stringa è sicuramente lunga abbastanza per estrarre ArrayLen coppie di cifre
                for (int _i = 1; _i <= ArrayLen; _i++)
                {
                    TempByte = _tempString.Substring(( sourceLen - _i * 2 ), 2);
                    byte.TryParse(TempByte, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out _tempValue);
                    _tempBuf[ArrayLen - _i] = _tempValue;
                }

                return _tempBuf;

            }
            catch
            {
                return null;
            }
        }


        public static ushort UshortFromArray(byte[] data, int position)
        {
            ushort _tempValue = 0;

            try
            {
                if (data.Length <= (position))
                    return _tempValue;

                _tempValue = (ushort)(data[position] << 8);
                _tempValue += (ushort)(data[position + 1]);

                return _tempValue;
            }
            catch
            {
                return _tempValue;
            }
        }

        public static short ArrayToShort(byte[] source, int start, int lenght)
        {
            short _tempVal16 = 0;
            int _segno = 1;
            byte _tempB;
            try
            {
                for (int _i = start; _i < (start + lenght); _i++)
                {
                    if (_i < source.Length)
                    {
                        _tempB = source[_i];
                        _tempVal16 = (short)(_tempVal16 << 8);
                        _tempVal16 += _tempB;
                    }

                }
                return _tempVal16;
            }
            catch
            {
                return 0;
            }
        }

        public static ushort ArrayToUshort(byte[] source, int start, int lenght)
        {
            ushort _tempVal16 = 0;
            int _segno = 1;
            byte _tempB;
            try
            {
                for (int _i = start; _i < (start + lenght); _i++)
                {
                    if (_i < source.Length)
                    {
                        _tempB = source[_i];
                        _tempVal16 = (ushort)(_tempVal16 << 8);
                        _tempVal16 += _tempB;
                    }

                }
                return _tempVal16;
            }
            catch
            {
                return 0;
            }
        }

        public static short ArrayToSShort(byte[] source, int start, int lenght)
        {
            short _tempVal16 = 0;
            int _segno = 1;
            byte _tempB;
            try
            {
                for (int _i = start; _i < (start + lenght); _i++)
                {
                    if (_i < source.Length)
                    {
                        _tempB = source[_i];
                        if (_i == start)
                        {
                            if ((_tempB & 0x80) > 0)
                                _segno = -1;
                            else
                                _segno = 1;

                            _tempB = (byte)(_tempB & 0x7F);
                        }
                        _tempVal16 = (short)(_tempVal16 << 8);
                        _tempVal16 += _tempB;
                    }

                }
                return (short)(_tempVal16 * _segno);
            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        /// Converte il subarray selezionato in stringa.
        /// </summary>
        /// <param name="source">Array origina.</param>
        /// <param name="start">Inizio con base 0.</param>
        /// <param name="lenght">Lunghezza.</param>
        /// <returns></returns>
        public static string ArrayToString(byte[] source, int start, int lenght)
        {


            try
            {
                if ((source.Length - start) < lenght)
                    return "";

                string result = System.Text.Encoding.UTF8.GetString(source, start, lenght);
                return result;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Converte una stringa di interi separati da un simbolo in un Array di interi.
        /// </summary>
        /// <param name="value">Stringa Origine.</param>
        /// <param name="separator">Carattere Separatore.</param>
        /// <param name="DefaultVal">Valore di default se il frammento non è convertibile in intero.</param>
        /// <returns></returns>
        public static int[] ToIntValueArray(string value, char separator, int DefaultVal = 0)
        {
            int[] _intArray = new int[0];

            try
            {
                string[] _valueArray = value.Split(separator);
                _intArray = new int[_valueArray.Length];

                for (int _i = 0; _i < _valueArray.Length; _i++)
                {
                    int _tempI;
                    if (int.TryParse(_valueArray[_i], out _tempI))
                        _intArray[_i] = _tempI;
                    else
                        _intArray[_i] = DefaultVal;

                }


                return _intArray;

            }
            catch
            {
                return _intArray;
            }

        }

        /// <summary>
        /// Converte una stringa di valori separati da un simbolo in un Array di bytes.
        /// </summary>
        /// <param name="value">Stringa Origine.</param>
        /// <param name="separator">Carattere Separatore.</param>
        /// <param name="DefaultVal">Valore di default se il frammento non è convertibile in intero.</param>
        /// <returns></returns>
        public static byte[] ToByteValueArray(string value, char separator, byte DefaultVal = 0)
        {
            byte[] _valArray = new byte[0];

            try
            {
                string[] _valueArray = value.Split(separator);
                _valArray = new byte[_valueArray.Length];

                for (int _i = 0; _i < _valueArray.Length; _i++)
                {
                    byte _tempI;
                    if (byte.TryParse(_valueArray[_i], out _tempI))
                        _valArray[_i] = _tempI;
                    else
                        _valArray[_i] = DefaultVal;

                }


                return _valArray;

            }
            catch
            {
                return _valArray;
            }

        }

        /*
        // Convert the Bitmap to grayscale.
        public static Bitmap ConvertToGrayscale(Bitmap Immagine, bool UseAverage)
        {
            try
            {
                Bitmap _tempBmp;

                _tempBmp = new Bitmap(Immagine.Width, Immagine.Height, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale);
                // using (var gr = Graphics.FromImage(bmp))
                //     gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
                // return bmp;



                return _tempBmp;
            }
            catch (Exception Ex)
            {

                return Immagine;
            }


        }
        */

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            try
            {
                byte[] gZipBuffer = Convert.FromBase64String(compressedText);
                using (var memoryStream = new MemoryStream())
                {
                    int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                    memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                    var buffer = new byte[dataLength];

                    memoryStream.Position = 0;
                    using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        gZipStream.Read(buffer, 0, buffer.Length);
                    }

                    return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                }
            }
            catch (Exception Ex)
            {
                return "";
            }
        }


        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialisation method. NOTE: Private members are not cloned using this method.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T CloneJson<T>(this T source)
        {
            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
        }


        public static ushort CalcolaFormula(string Variabile, ushort ValoreVar, string Formula, int Divisore = 1)
        {
            try
            {
                // Verifico la lunghezza minima (3)
                if (Formula.Length < 4)
                {
                    return 0;
                }

                string Visualizzazione = Formula.Substring(0, 1);
                string VarUsata = Formula.Substring(1, 1);
                string OperazioneUsata = Formula.Substring(2, 1);
                string ValoreFormula = Formula.Substring(3);

                ushort ValParametro;
                ushort ValFormula = 0;
                bool result = ushort.TryParse(ValoreFormula, out ValParametro);

                if (!result)
                {
                    return 0;
                }


                // Controllo che la variabile sia quella in formula
                if (VarUsata != Variabile && Variabile != "#" )
                {
                    return 0;
                }

                switch (OperazioneUsata)
                {
                    case "#":
                    case "@":
                        {
                            ValFormula = ValParametro;
                            break;
                        }


                    case "/":
                        {
                            if (ValParametro != 0)
                            {
                                ValFormula = (ushort)(ValoreVar / ValParametro);
                            }
                            break;
                        }

                    case "*":
                        {
                            ValFormula = (ushort)(ValoreVar * ValParametro);
                            break;
                        }

                    default:
                        {
                            ValFormula = 0;
                            break;
                        }
                }


                return ValFormula;
            }
            catch
            {
                return 0;
            }
        }

        public static ushort StatoParametro(string Formula)
        {
            try
            {
                ushort esito = 0;
                string OperazioneUsata = "";
                if (Formula.Length > 0)
                {
                    OperazioneUsata = Formula.Substring(0, 1);
                }
                
                switch (OperazioneUsata)
                {
                    case "":
                        {
                            esito = 0;
                            break;
                        }
                    case "#":
                        {
                            esito = 1;
                            break;
                        }
                    case "=":
                        {
                            esito = 4;
                            break;
                        }
                    case "~":
                        {
                            esito = 5;
                            break;
                        }

                    default:
                        {
                            esito = 1;
                            break;
                        }
                }

                return esito;
            }

            catch
            {
                return 0;
            }
        }


    }

    public static class FunzioniBinarie
    {
        /// <summary>
        /// Setta il bit in posizione position al valore value
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="position">The position.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns>System.Byte.</returns>
        public static byte SetBit(byte source, int position, bool value = true)
        {

            int _tempMask = 0;
            byte _result = source;
            try
            {
                if (value)
                {
                    // Setto il bit a 1 mettendolo in or con l'origine
                    _tempMask = 0x0001;
                    _tempMask =_tempMask << position;
                    _result = (byte)(source | _tempMask);
                }
                else
                {
                    // Setto il bit a 0 mettendolo in and con l'origine
                    _tempMask = 0x0001;
                    _tempMask = _tempMask << position;
                    _tempMask = ~_tempMask;
                    _result = (byte)(source & _tempMask);
                }

                return _result;
            }
            catch
            {
                return source;
            }
        }


        public static bool GetBit(byte source, int position)
        {

            int _tempMask = 0;
            bool _result = false;
            try
            {

                _tempMask = 0x0001;
                _tempMask = _tempMask << position;

                _result = ((source & _tempMask) == _tempMask);


                return _result;
            }
            catch
            {
                return false;
            }
        }


 


    }

}

