using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Utility
{
    public class txtLogger
    {
        private string _fileName;
        private StreamWriter _fileOut;
        private bool _connessioneAttiva;

        public enum DataMode: byte { Append = 0x00, OverWrite = 0x01, Rename = 0x02 }

        public txtLogger()
        {
            try
            {
                _connessioneAttiva = false;
                _fileName = "";
            }
            catch
            {

            }


        }



        public bool Close()
        {
            try
            {
                _connessioneAttiva = false;
                _fileName = "";
                return true;
            }
            catch
            {
                return false;
            }


        }


        // Apro il file di log.... in realtà gestisco solo 
        public bool Open(string NomeFile , DataMode modo = DataMode.Append)
        {
            try
            {
                DateTime _inizio = DateTime.Now;
                string _timestamp = _inizio.ToShortDateString() + " " + _inizio.ToShortTimeString();

                _fileName = NomeFile;
                if (_fileName.Substring(_fileName.Length - 4).ToUpper() == ".TXT")
                    _fileName = _fileName.Substring(0, _fileName.Length - 4);

                switch (modo)
                {
                    case DataMode.Append:
                        _fileName += ".txt";
                        break;
                    case DataMode.OverWrite:
                        _fileName += ".txt";
                        if (File.Exists(_fileName))
                            File.Delete(_fileName);
                        break;
                    case DataMode.Rename:
                        _fileName += _timestamp + ".txt";
                        break;
                    default:
                        break;
                }

                WriteLn("----------------------------------------------------------------");
                WriteLn(" OPEN: " + _timestamp );
                WriteLn("----------------------------------------------------------------");
                WriteLn("");

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool Write( string testo)
        {
            try
            {
                if (string.IsNullOrEmpty(_fileName)) return false;
                using (StreamWriter sw = File.AppendText(_fileName))
                {
                    sw.Write(testo);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes the ln.
        /// </summary>
        /// <param name="testo">The testo.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool WriteLn(string testo)
        {
            try
            {
                if (string.IsNullOrEmpty(_fileName)) return false;
                using (StreamWriter sw = File.AppendText(_fileName))
                {
                    sw.WriteLine(testo);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }




    }
}
