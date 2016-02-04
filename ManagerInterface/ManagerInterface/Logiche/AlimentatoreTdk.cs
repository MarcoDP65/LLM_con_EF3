using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace PannelloCharger
{
    public class AlimentatoreTdk
    {
        public string PortaCom { get; set;}
        bool _statoCom = false;
        SerialPort _serialPort;
        string _serBuffer;
        public NumberFormatInfo NfiEn = new CultureInfo("en-US", false).NumberFormat;
        string _lastMessage = "";

        string _strVimpostati;
        string _strVrilevati;
        float _Vimpostati;
        float _Vrilevati;

        string _strAimpostati;
        string _strArilevati;
        float _Aimpostati;
        float _Arilevati;

        string _strStato;
        bool _Stato;


        public AlimentatoreTdk()
        {

        }

        /*
        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(100);
            string data = _serialPort.ReadLine();
            this.BeginInvoke(new SetTextDeleg(si_DataReceived), new object[] { data });
        }
        */

        private void port_DataReceivedSb(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                //string testom = "Dati Ricevuti SB: " + serialeApparato.BytesToRead.ToString();
                _serBuffer = "";
                bool _trovatoETX = false;

                Thread.Sleep(100);
                do
                {
                    _serBuffer += _serialPort.ReadLine();
                }
                while (_serBuffer.Length <= 0);
                
                analizzaMessaggioBase(_serBuffer);
            }
            catch (Exception Ex)
            {

            }

        }

        private string _ComDataReceived()
        {
            try
            {
                _serBuffer = "";
                Thread.Sleep(100);
                do
                {
                    //_serialPort.NewLine = "\n";
                    _serBuffer += _serialPort.ReadLine();
                }
                while (_serBuffer.Length <= 0);

                return _serBuffer;
                //analizzaMessaggioBase(_serBuffer);
            }
            catch (Exception Ex)
            {
                return "";
            }

        }



        private bool analizzaMessaggioBase(string Messaggio )
        {
            try
            {

                return false;
            }

            catch (Exception ex)
            {
                return false;
            }
        }





        public bool InizializzaSeriale(string Porta)
        {
            try
            {
                if (Porta != "")
                {
                    _serialPort = new SerialPort(Porta, 9600, Parity.None, 8, StopBits.One);
                    _serialPort.Handshake = Handshake.None;
                    //cEventHelper.RemoveEventHandler(_serialPort, "DataReceived");
                    //_serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                    _serialPort.ReadTimeout = 500;
                    _serialPort.WriteTimeout = 500;
                    _serialPort.NewLine = "\r";
                    return true; 
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ApriPorta()
        {
            bool _portaAttiva = false;

            try
            {
                if (_serialPort != null)
                {
                    if (!(_serialPort.IsOpen)) _serialPort.Open();
                    _portaAttiva = _serialPort.IsOpen;
                }

                return _portaAttiva;
            }

            catch (Exception ex)
            {
                return _portaAttiva;
            }
        }

        public bool ChiudiPorta()
        {
            bool _portaAttiva = false;

            try
            {
                if (_serialPort != null)
                {
                    _serialPort.Close();
                    _portaAttiva = !(_serialPort.IsOpen);
                }

                return _portaAttiva;
            }

            catch
            {
                return _portaAttiva;
            }
        }



        public bool PortaConnessa
        {
            get
            {
                try
                {
                    bool _esito = false;

                    if (_serialPort != null)
                    {
                        _esito = _serialPort.IsOpen;
                    }


                    return _esito;
                }
                catch
                {
                    return false;
                }
            }
        }


        public bool PortaPresente
        {
            get
            {
                try
                {
                    bool _esito = false;

                    if (_serialPort != null)
                    {
                        _esito = true;
                    }


                    return _esito;
                }
                catch
                {
                    return false;
                }
            }
        }


        public bool ScriviMessaggio(string Messaggio, bool TerminaRiga = false, bool silent = true)
        {
            try
            {
                string _messaggio = Messaggio;
                bool _esito = false;
                if (PortaConnessa)
                {
                    if (TerminaRiga) _messaggio += (char)13;
                    _serialPort.Write(_messaggio);
                    _esito = true;
                }

                return _esito;
            }
            catch (Exception ex)
            {
                return false;
            }
        }





        public bool apparatoPresente(int Addr)
        {
            try
            {
                int _addr = Addr;
                string _comando;
                string _risposta;

                bool _esito;
                if (Addr < 0) _addr = 0;
                if (Addr > 30) _addr = 30;

                _comando = "ADR " + Addr.ToString();
                _esito = ScriviMessaggio(_comando, true, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if (_risposta == "OK") return true;
                }
                else
                {
                    return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Modello()
        {
            try
            {
                string _comando;
                string _risposta;

                bool _esito;


                _comando = "IDN?\r";
                _esito = ScriviMessaggio(_comando, false, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if (!_risposta.StartsWith("C")) return true;
                }
                else
                {
                    return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool LeggiTensioni()
        {
            try
            {
                string _comando;
                string _risposta;

                bool _esito;

                _strVimpostati = "";
                _strVrilevati = "";
                _Vimpostati = 0;
                _Vrilevati = 0;

                _comando = "PV?\r";
                _esito = ScriviMessaggio(_comando, false, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if (!_risposta.StartsWith("C"))
                    {
                        _strVimpostati = _risposta;
                        float.TryParse(_risposta, out _Vimpostati);
                    }
                }
                else
                {
                    return false;
                }

                _comando = "MV?\r";
                _esito = ScriviMessaggio(_comando, false, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if (!_risposta.StartsWith("C"))
                    {
                        _strVrilevati = _risposta;
                        float.TryParse(_risposta, NumberStyles.Any, NfiEn, out _Vrilevati);

                        return true;
                    }
                }
                else
                {
                    return false;
                }




                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ImpostaTensione(float Tensione)
        {
            try
            {
                string _comando;
                string _risposta;
                float _tensione = Tensione;
                bool _esito;

                _strVimpostati = "";
                 _Vimpostati = 0;

                if (_tensione < 0) _tensione = 0;
                if (_tensione > 3) _tensione = 3;

                _comando = "PV "+ _tensione.ToString("0.000",NfiEn) + "\r";
                _esito = ScriviMessaggio(_comando, false, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if ( _risposta.StartsWith("C"))
                    {
                        return false;
                    }
                }

                _comando = "PV?\r";
                _esito = ScriviMessaggio(_comando, false, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if (!_risposta.StartsWith("C"))
                    {
                        _strVimpostati = _risposta;
                        float.TryParse(_risposta, out _Vimpostati);

                        return true;
                    }
                }
                else
                {
                    return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }




        public bool LeggiCorrenti()
        {
            try
            {
                string _comando;
                string _risposta;

                bool _esito;

                _strAimpostati = "";
                _strArilevati = "";
                _Aimpostati = 0;
                _Arilevati = 0;

                _comando = "PC?\r";
                _esito = ScriviMessaggio(_comando, false, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if (!_risposta.StartsWith("C"))
                    {
                        _strAimpostati = _risposta;
                        float.TryParse(_risposta,NumberStyles.Any,NfiEn, out _Aimpostati);
                    }
                }
                else
                {
                    return false;
                }

                _comando = "MC?\r";
                _esito = ScriviMessaggio(_comando, false, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if (!_risposta.StartsWith("C"))
                    {
                        _strArilevati = _risposta;
                        float.TryParse(_risposta, NumberStyles.Any, NfiEn, out _Arilevati);

                        return true;
                    }
                }
                else
                {
                    return false;
                }




                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ImpostaCorrente(float Corrente)
        {
            try
            {
                string _comando;
                string _risposta;
                float _corrente = Corrente;
                bool _esito;

                _strVimpostati = "";
                _Vimpostati = 0;

                if (_corrente < 0) _corrente = 0;
                if (_corrente > 400) _corrente = 400;

                _comando = "PC " + _corrente.ToString("0.0", NfiEn) + "\r";
                _esito = ScriviMessaggio(_comando, false, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if (_risposta.StartsWith("C"))
                    {
                        return false;
                    }
                }

                _comando = "PC?\r";
                _esito = ScriviMessaggio(_comando, false, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if (!_risposta.StartsWith("C"))
                    {
                        _strAimpostati = _risposta;
                        float.TryParse(_risposta, out _Aimpostati);

                        return true;
                    }
                }
                else
                {
                    return false;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        public bool LeggiStato()
        {
            try
            {
                string _comando;
                string _risposta;

                bool _esito;

                _strStato = "";
                _Stato = false;

                _comando = "OUT?\r";
                _esito = ScriviMessaggio(_comando, false, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if (!_risposta.StartsWith("C"))
                    {
                        _strStato = _risposta;
                        if (_risposta == "ON") _Stato = true;

                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ImpostaStato(bool Acceso)
        {
            try
            {
                string _comando;
                string _risposta;
                bool _esito;

                if(Acceso) _comando = "OUT ON\r";
                else _comando = "OUT OFF\r";
                _esito = ScriviMessaggio(_comando, false, true);

                if (_esito)
                {
                    _risposta = _ComDataReceived();
                    _lastMessage = _risposta;
                    if (_risposta.StartsWith("C"))
                    {
                        return false;
                    }

                    return LeggiStato();
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #region "Propretà"

        public string LastMessage
        {
            get
            { return _lastMessage; }
        }

        public string strVimpostati
        {
            get
            { return _strVimpostati; }
        }

        public string strVrilevati
        {
            get
            { return _strVrilevati; }
        }

        public float Vimpostati
        {
            get
            { return _Vimpostati; }
        }

        public float Vrilevati
        {
            get
            { return _Vrilevati; }
        }
        //--------------------------------------------
        public string strAimpostati
        {
            get
            { return _strAimpostati; }
        }

        public string strArilevati
        {
            get
            { return _strArilevati; }
        }

        public float Aimpostati
        {
            get
            { return _Aimpostati; }
        }

        public float Arilevati
        {
            get
            { return _Arilevati; }
        }
        //--------------------------------------------
        public bool UscitaAttiva
        {
            get
            { return _Stato; }
        }

        public string strUscita
        {
            get
            { return _strStato; }
        }


        #endregion  "Propretà"

    }






}
