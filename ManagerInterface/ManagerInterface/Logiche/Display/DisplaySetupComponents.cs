using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

using SQLite.Net;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;

namespace ChargerLogic
{
    public class ModelloComando
    {
        public enum TipoComando : byte
        {
            ScriviTesto6x8 = 0x68,
            ScriviOra6x8 = 0x64,
            ScriviData6x8 = 0x66,
            ScriviVariabile6x8 = 0xC6,
            ScriviTesto16 = 0x16,
            ScriviVariabile16 = 0xC8,
            DisegnaImmagine = 0x5A,
            ScrollImmagini = 0x5C,
        }

        public TipoComando Codice { get; set; }
        public string Nome { get; set; }
        public bool ValoreStringa { get; set; }
        public bool LunghezzaStringaChr { get; set; }
        public bool LunghezzaStringaPix { get; set; }
        public bool AltezzaStringaPix { get; set; }
        public bool CoordX { get; set; }
        public bool CoordY { get; set; }
        public bool Colore { get; set; }
        public bool TempoOn { get; set; }
        public bool TempoOff { get; set; }
        public bool NumeroVariabile { get; set; }
        public bool NumeroImmagine { get; set; }

    }

    public partial class DisplaySetup
    { 
        public enum BaudRate: byte
        {
            BD_9600   = 0x00,
            BD_19200  = 0x01,
            BD_38400  = 0x02,
            BD_57600  = 0x03,
            BD_115200 = 0x04 
        }

        public class Immagine
        {
            public byte[] ImageBuffer { get; set; }
            [JsonIgnore]
            public Bitmap bmp;
            public byte[] _bmpArray;
            [JsonIgnore]
            public Bitmap bmpBase;
            public byte[] _bmpBaseArray;
            public string Nome;
            public ushort Id { get; set; }
            public byte Numero;
            public byte Lingua;
            /// <summary>
            /// Larghezza in pixel dell'immagine
            /// </summary>
            public byte Width;
            /// <summary>
            /// Altezza in Pixel dell'immagine
            /// </summary>
            public byte Height;
            /// <summary>
            /// Dimensione in Bytes dell'Immagine
            /// Ogni pixel è codificato con un singolo bit
            /// </summary>
            public ushort Size;



            public Immagine()
            {
                Inizializza(240, 128);
            }



            public Immagine(byte Colonne,byte Righe )

            {
                Inizializza(Colonne, Righe);
            }

            public void Clear()
            {
                try
                {
                    ImageBuffer = new byte[Size];
                    bmp = new Bitmap(Width, Height);
                    _bmpArray = new byte[0];
                    _bmpBaseArray = new byte[0];
                }
                catch
                {

                }
            }

            public void fromBitmap()
            {
                try
                {

                    byte _NumRow = 0;
                    byte _NumCol = 0;
                    byte _NumRowBlock = 0;

                    bmp = new Bitmap(Width, Height);


                    for (int _count = 0; _count < ImageBuffer.Length; _count++)

                    // for (int _count = 0; _count < Width *2; _count++)
                    {
                        byte _currcol = (byte)(_count % Width);
                        byte _currRowBlock = (byte)(_count / Width);
                        for (byte _currRow = 0; _currRow < 8; _currRow++)
                        {
                            if (((byte)(ImageBuffer[_count] >> _currRow) & 0x01) == 0x01)
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.Black);
                            }
                            else
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.White);
                            }
                        }
                    }

                }
                catch
                {

                }
            }

            public void BmpToBuffer()
            {
                try
                {
                    int _tempWidth = bmp.Width; 

                    int _tempHeight = bmp.Height;
                    int _arraysize = 0;

                    byte _NumRowBlock = 0;

                    _NumRowBlock = (byte)(_tempHeight / 8);

                    if((_tempHeight % 8) != 0)
                    {
                        _NumRowBlock += 1;
                    }
                    _arraysize = _tempWidth * _NumRowBlock;
                    Size = (ushort)_arraysize;
                    ImageBuffer = new byte[_arraysize];


                    for (int _rowBlock = 0; _rowBlock < _NumRowBlock; _rowBlock++)
                    {
                        for (int _CurrCol = 0; _CurrCol < _tempWidth; _CurrCol++)
                        {
                            byte _actPix = 0x00;

                            for (int _rBlock = 0; _rBlock < 8; _rBlock++)
                            {
                                int _rigacorrente = _rowBlock * 8 + _rBlock;
                                
                                if (_rigacorrente < _tempHeight)
                                {
                                    Color _pixelCorrente = bmp.GetPixel(_CurrCol, _rigacorrente);
                                    int grayScale = (int)((_pixelCorrente.R * 0.3) + (_pixelCorrente.G * 0.59) + (_pixelCorrente.B * 0.11));

                                    if (grayScale < 128)
                                    {
                                        byte _tempBlock = (byte)(0x01 << _rBlock);
                                        _actPix = (byte)(_actPix | _tempBlock);
                                    }

                                }


                            }
                            int _posCorrente = _rowBlock * _tempWidth + _CurrCol;
                            ImageBuffer[_posCorrente] = _actPix;

                        }

                    }
                    

                }
                catch
                {

                }
            }


            public void GeneraImmagine()
            {
                try
                {
                    bmp = new Bitmap(Width, Height);

                    byte _NumRowBlock = 0;
                    byte _NumRow = 0;
                    byte _NumCol = 0;


                    for (int _count = 0; _count < ImageBuffer.Length; _count++)
                    {
                        byte _currcol = (byte)(_count % Width) ;
                        byte _currRowBlock = (byte)(_count / Width);
                        for(byte _currRow = 0; _currRow < 8; _currRow++)
                        {
                            if(((byte)(ImageBuffer[_count] >> _currRow ) & 0x01) == 0x01)
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 +( _currRow)), Color.Black);
                            }
                            else
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 +( _currRow)), Color.White);
                            }
                        }
                    }

                }
                catch
                {

                }
            }

            public bool BmpFromBuffer()
            {
                try
                {
                    bmp = new Bitmap(Width, Height);

                    byte _NumRowBlock = 0;
                    byte _NumRow = 0;
                    byte _NumCol = 0;


                    for (int _count = 0; _count < ImageBuffer.Length; _count++)
                    {
                        byte _currcol = (byte)(_count % Width);
                        byte _currRowBlock = (byte)(_count / Width);
                        for (byte _currRow = 0; _currRow < 8; _currRow++)
                        {
                            if (((byte)(ImageBuffer[_count] >> _currRow) & 0x01) == 0x01)
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.Black);
                            }
                            else
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.White);
                            }
                        }
                    }
                    return true;
                }
                
                catch
                {
                    return false;
                }
            }





            public void Inizializza(byte Colonne, byte Righe)
            {
                try
                {
                    Nome = "IMAGE000";
                    Id = 0;
                    Numero = 0;
                    Lingua = 0;
                    Width = Colonne;
                    Height = Righe;
                    bmp = new Bitmap(Width, Height);
                    Size = (ushort)((Width * Height) / 8);
                    if (((Width * Height) % 8) != 0)
                    {
                        Size += 1;
                    }

                    ImageBuffer = new byte[Size];
                }
                catch
                {

                }
            }


            public bool ImgToBytearray()
            {
                try
                {
                    ImageConverter converter = new ImageConverter();
                    _bmpArray =  (byte[])converter.ConvertTo(bmp, typeof(byte[]));
                    _bmpBaseArray = (byte[])converter.ConvertTo(bmpBase, typeof(byte[]));

                    return true;

                }
                catch (Exception Ex)
                {

                    return false;
                }
            }

            public bool BytearrayToImg()
            {
                try
                {

                    MemoryStream ms;

                    if (_bmpArray != null)
                    {
                        ms = new MemoryStream(_bmpArray);
                        Image _tempImg = Image.FromStream(ms);
                        bmp = new Bitmap(_tempImg);
                    }
                    else
                        bmp = null;

                    if (_bmpBaseArray != null)
                    {
                        ms = new MemoryStream(_bmpBaseArray);
                        Image _tempImg = Image.FromStream(ms);
                        bmpBase = new Bitmap(_tempImg);
                    }
                    else
                        bmpBase = null;

                    return true;

                }
                catch (Exception Ex)
                {

                    return false;
                }
            }


            public bool getPixel(byte Col, byte  Row)
            {
                try
                {
                    bool StatoPixel = false;
                    ushort index = 0;

                    //index = Row / 8;


                    return StatoPixel;
                }
                catch
                {
                    return false;
                }
            }

        }

        public class Schermata
        {
            public List<Comando> Comandi = new List<Comando>();

            public byte[] ImageBuffer { get; set; }
            [JsonIgnore]
            public Bitmap bmp;
            public byte[] _bmpArray;
            public string Nome { get; set; }
            public string NomeLista { get; set; }
            public ushort Id { get; set; }
            public byte Numero { get; set; }
            public byte Lingua;
            /// <summary>
            /// Larghezza in pixel dell'immagine
            /// </summary>
            public byte Width;
            /// <summary>
            /// Altezza in Pixel dell'immagine
            /// </summary>
            public byte Height;
            /// <summary>
            /// Dimensione in Bytes dell'Immagine
            /// Ogni pixel è codificato con un singolo bit
            /// </summary>
            public ushort Size;




            public Schermata()
            {
                Inizializza(240, 128);
            }



            public Schermata(byte Colonne, byte Righe)

            {
                Inizializza(Colonne, Righe);
            }

            public void Clear()
            {
                try
                {
                    ImageBuffer = new byte[Size];
                    bmp = new Bitmap(Width, Height);
                    _bmpArray = new byte[0];
                    Comandi.Clear();
                }
                catch
                {

                }
            }

            public void fromBitmap()
            {
                try
                {

                    byte _NumRow = 0;
                    byte _NumCol = 0;
                    byte _NumRowBlock = 0;

                    bmp = new Bitmap(Width, Height);


                    for (int _count = 0; _count < ImageBuffer.Length; _count++)

                    // for (int _count = 0; _count < Width *2; _count++)
                    {
                        byte _currcol = (byte)(_count % Width);
                        byte _currRowBlock = (byte)(_count / Width);
                        for (byte _currRow = 0; _currRow < 8; _currRow++)
                        {
                            if (((byte)(ImageBuffer[_count] >> _currRow) & 0x01) == 0x01)
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.Black);
                            }
                            else
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.White);
                            }
                        }
                    }

                }
                catch
                {

                }
            }

            public void BmpToBuffer()
            {
                try
                {
                    int _tempWidth = bmp.Width;
                    int _tempHeight = bmp.Height;
                    int _arraysize = 0;

                    byte _NumRowBlock = 0;

                    _NumRowBlock = (byte)(_tempHeight / 8);

                    if ((_tempHeight % 8) != 0)
                    {
                        _NumRowBlock += 1;
                    }
                    _arraysize = _tempWidth * _NumRowBlock;

                    ImageBuffer = new byte[_arraysize];


                    for (int _rowBlock = 0; _rowBlock < _NumRowBlock; _rowBlock++)
                    {
                        for (int _CurrCol = 0; _CurrCol < _tempWidth; _CurrCol++)
                        {
                            byte _actPix = 0x00;

                            for (int _rBlock = 0; _rBlock < 8; _rBlock++)
                            {
                                int _rigacorrente = _rowBlock * 8 + _rBlock;

                                if (_rigacorrente < _tempHeight)
                                {
                                    Color _pixelCorrente = bmp.GetPixel(_CurrCol, _rigacorrente);
                                    int grayScale = (int)((_pixelCorrente.R * 0.3) + (_pixelCorrente.G * 0.59) + (_pixelCorrente.B * 0.11));

                                    if((_pixelCorrente.R != 255 ) || (_pixelCorrente.G != 255) || (_pixelCorrente.B != 255) )
                                    { 

                                        grayScale = grayScale;
                                    }
                                        
                                    if (grayScale < 128)
                                    {
                                        byte _tempBlock = (byte)(0x01 << _rBlock);
                                        _actPix = (byte)(_actPix | _tempBlock);
                                    }

                                }


                            }
                            int _posCorrente = _rowBlock * _tempWidth + _CurrCol;
                            ImageBuffer[_posCorrente] = _actPix;

                        }

                    }
                    Log.Info("------------------------------------------------------------------------------------------------------------");
                    Log.Info("-  " + NomeLista + " | Bites totali: " + _arraysize);
                    Log.Info("------------------------------------------------------------------------------------------------------------");
                    Log.Info(FunzioniComuni.HexdumpArray(ImageBuffer));
                    Log.Info("------------------------------------------------------------------------------------------------------------");

                }
                catch (Exception Ex)
                {

                    Log.Error("Schermata.BmpToBuffer : " + Ex.Message);

                }
            }


            public void GeneraImmagine()
            {
                try
                {
                    bmp = new Bitmap(Width, Height);

                    byte _NumRowBlock = 0;
                    byte _NumRow = 0;
                    byte _NumCol = 0;


                    for (int _count = 0; _count < ImageBuffer.Length; _count++)
                    {
                        byte _currcol = (byte)(_count % Width);
                        byte _currRowBlock = (byte)(_count / Width);
                        for (byte _currRow = 0; _currRow < 8; _currRow++)
                        {
                            if (((byte)(ImageBuffer[_count] >> _currRow) & 0x01) == 0x01)
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.Black);
                            }
                            else
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.White);
                            }
                        }
                    }

                }
                catch
                {

                }
            }

            public bool BmpFromBuffer()
            {
                try
                {
                    bmp = new Bitmap(Width, Height);

                    byte _NumRowBlock = 0;
                    byte _NumRow = 0;
                    byte _NumCol = 0;


                    for (int _count = 0; _count < ImageBuffer.Length; _count++)
                    {
                        byte _currcol = (byte)(_count % Width);
                        byte _currRowBlock = (byte)(_count / Width);
                        for (byte _currRow = 0; _currRow < 8; _currRow++)
                        {
                            if (((byte)(ImageBuffer[_count] >> _currRow) & 0x01) == 0x01)
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.Black);
                            }
                            else
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.White);
                            }
                        }
                    }
                    return true;
                }

                catch
                {
                    return false;
                }
            }





            public void Inizializza(byte Colonne, byte Righe)
            {
                try
                {
                    Nome = "SCREN000";
                    Id = 0;
                    Numero = 0;
                    Lingua = 0;
                    Width = Colonne;
                    Height = Righe;
                    bmp = new Bitmap(Width, Height);
                    Size = (ushort)((Width * Height) / 8);
                    if (((Width * Height) % 8) != 0)
                    {
                        Size += 1;
                    }

                    ImageBuffer = new byte[Size];
                }
                catch
                {

                }
            }


            public bool ImgToBytearray()
            {
                try
                {
                    ImageConverter converter = new ImageConverter();
                    _bmpArray = (byte[])converter.ConvertTo(bmp, typeof(byte[]));

                    return true;

                }
                catch (Exception Ex)
                {

                    return false;
                }
            }

            public bool BytearrayToImg()
            {
                try
                {

                    MemoryStream ms;

                    if (_bmpArray != null)
                    {
                        ms = new MemoryStream(_bmpArray);
                        Image _tempImg = Image.FromStream(ms);
                        bmp = new Bitmap(_tempImg);
                    }
                    else
                        bmp = null;

                    return true;

                }
                catch (Exception Ex)
                {

                    return false;
                }
            }

            #region Proprietà

            public int NumComandi
            {
                get
                {
                    return Comandi.Count();
                }
            }

            public string strNumComandi
            {
                get
                {
                    return Comandi.Count().ToString();
                }
            }

            #endregion
        }

        public class Comando
        {
            public byte Numero;

            public enum Attributo: byte
            {
                LunghezzaStringaChr = 0x01,
                LunghezzaStringaPix = 0x02,
                AltezzaStringaPix = 0x03,
                CoordX = 0x10,
                CoordY = 0x11,
                Colore = 0x20,
                TempoOn = 0x30,
                TempoOff = 0x31,
                NumeroVariabile = 0x40,
                NumeroImmagine = 0x41,

            }

            public ModelloComando.TipoComando Attivita;
            public string DescAttivita { get; set; }
            private byte[] _HexMap = new byte[0];

            public byte LenStringa { get; set; }
            public byte LenPixStringa { get; set; }
            public byte HighPixStringa { get; set; }
            public byte PosX { get; set; }
            public byte PosY { get; set; }
            public byte Colore { get; set; }
            public byte IdVariabile { get; set; }
            public ushort IdImmagine { get; set; }
            public byte TimeOnVar { get; set; }
            public byte TimeOffVar { get; set; }
            public byte TimeScroll { get; set; }
            public byte NumImg { get; set; }
            public ushort[] SerieImmagini { get; set; }
            public string Messaggio { get; set; }


            public string  Posizione
            {
                get
                {
                    return PosX.ToString("0") + ":" + PosY.ToString("0");
                }
            }

            public byte ComponiByteArray()
            {
                try
                {
                    byte _len = 0;
                    int _strlen = Messaggio.Length;
                    if (_strlen > 40 )
                    {
                        _strlen = 40;
                        Messaggio = Messaggio.Substring(0, _strlen);
                    }

                    byte[] _tempData = new byte[256];
                    switch (Attivita)
                    {
                        case ModelloComando.TipoComando.ScriviTesto6x8:
                            _len = (byte)(_strlen + 6);
                            _tempData = FunzioniComuni.StringToArray(Messaggio, _strlen, 6);
                            _tempData[0] = (byte)Attivita;
                            _tempData[1] = LenPixStringa;
                            _tempData[2] = HighPixStringa;
                            _tempData[3] = PosX;
                            _tempData[4] = PosY;
                            _tempData[5] = Colore;

                            break;
                        case ModelloComando.TipoComando.ScriviOra6x8:
                            _len = 4;
                            _tempData[0] = (byte)Attivita;
                            _tempData[1] = PosX;
                            _tempData[2] = PosY;
                            _tempData[3] = Colore;
                            break;
                        case ModelloComando.TipoComando.ScriviData6x8:
                            _len = 4;
                            _tempData[0] = (byte)Attivita;
                            _tempData[1] = PosX;
                            _tempData[2] = PosY;
                            _tempData[3] = Colore;
                            break;
                        case ModelloComando.TipoComando.ScriviVariabile6x8:
                            _len = 8;
                            _tempData[0] = (byte)Attivita;
                            _tempData[1] = IdVariabile;
                            _tempData[2] = LenPixStringa;
                            _tempData[3] = PosX;
                            _tempData[4] = PosY;
                            _tempData[5] = Colore;
                            _tempData[6] = TimeOnVar;
                            _tempData[7] = TimeOffVar;
                            break;
                        case ModelloComando.TipoComando.ScriviTesto16:
                            _len = (byte)(_strlen + 5);
                            _tempData = FunzioniComuni.StringToArray(Messaggio, _strlen, 5);
                            _tempData[0] = (byte)Attivita;
                            _tempData[1] = _len;
                            _tempData[2] = PosX;
                            _tempData[3] = PosY;
                            _tempData[4] = Colore;

                            break;
                        case ModelloComando.TipoComando.ScriviVariabile16:
                            _len = 8;
                            _tempData[0] = (byte)Attivita;
                            _tempData[1] = IdVariabile;
                            _tempData[2] = LenStringa;
                            _tempData[3] = PosX;
                            _tempData[4] = PosY;
                            _tempData[5] = Colore;
                            _tempData[6] = TimeOnVar;
                            _tempData[7] = TimeOffVar;
                            break;
                        case ModelloComando.TipoComando.DisegnaImmagine:
                            _len = 6;
                            _tempData[0] = (byte)Attivita;
                            _tempData[1] = 0;
                            _tempData[2] = 1;
                            _tempData[3] = PosX;
                            _tempData[4] = PosY;
                            _tempData[5] = Colore;

                            break;
                        case ModelloComando.TipoComando.ScrollImmagini:
                            // prima determino il numero dei frame
                            byte[] _sequenza;

                            _sequenza = FunzioniComuni.ToByteValueArray(Messaggio, ';', 1);
                            int _seqLen = _sequenza.Length;
                            _len =(byte)(_seqLen + 6 );

                            _tempData[0] = (byte)Attivita;
                            for (int _i = 0; _i < _seqLen; _i++)
                            {
                                _tempData[_i+1] = _sequenza[_i];
                            }

                            _tempData[_seqLen+1] =0x00;  // Fine Sequenza
                            _tempData[_seqLen + 2] = PosX;
                            _tempData[_seqLen + 3] = PosY;
                            _tempData[_seqLen + 4] = Colore;
                            _tempData[_seqLen + 5] = TimeOnVar;
                            break;
                        default:
                            _len = 1;
                            _tempData[0] = (byte)Attivita;
                            break;
                    }

                    _HexMap = new byte[_len];
                    for (int _ch = 0; _ch < _len; _ch++)
                    {
                        _HexMap[_ch] = _tempData[_ch];
                    }

                    return _len;
                }
                catch (Exception Ex)
                {

                    Log.Error("CaricaFile Display: " + Ex.Message);
                    return 0;

                }
            }


            public byte[] ArrayComando
            {
                get
                {
                    return _HexMap;
                }
            }

        }

        public class Variabile
        {
            public string Nome { get; set; }
            public byte Id { get; set; }
            public string Valore { get; set; }
        }

    

        public class DataModel
        {
            public string NomeModello;
            public string Versione;
            public string Note;
            public string VersioneFirmware;
            public string LinguaBase;
            public ushort CRC;

            public DateTime DataCreazione;
            public DateTime DataModifica;

            public List<Immagine> Immagini = new List<Immagine>();
            public List<Schermata> Schermate = new List<Schermata>();
            public List<Variabile> Variabili = new List<Variabile>();
        }

        public class ImmagineOld
        {
            public byte[] ImageBuffer;

            public Bitmap bmp;
            private byte[] _bmpArray;
            public Bitmap bmpBase;
            private byte[] _bmpBaseArray;
            public string Nome;
            public ushort Id { get; set; }
            public byte Numero;
            public byte Lingua;
            /// <summary>
            /// Larghezza in pixel dell'immagine
            /// </summary>
            public byte Width;
            /// <summary>
            /// Altezza in Pixel dell'immagine
            /// </summary>
            public byte Height;
            /// <summary>
            /// Dimensione in Bytes dell'Immagine
            /// Ogni pixel è codificato con un singolo bit
            /// </summary>
            public ushort Size;



            public ImmagineOld()
            {
                Inizializza(240, 128);
            }



            public ImmagineOld(byte Colonne, byte Righe)

            {
                Inizializza(Colonne, Righe);
            }

            public void Clear()
            {
                try
                {
                    ImageBuffer = new byte[Size];
                    bmp = new Bitmap(Width, Height);
                }
                catch
                {

                }
            }

            public void fromBitmap()
            {
                try
                {

                    byte _NumRow = 0;
                    byte _NumCol = 0;
                    byte _NumRowBlock = 0;



                    bmp = new Bitmap(Width, Height);

                    /*
        
                    byte _NumRowBlock = 0;
                    _NumRowBlock = (byte)(Height / 8);
                    for(int _rowBlock = 0; _rowBlock<= _NumRowBlock; _rowBlock++)
                    {
                        for (int _rowBlock = 0; _rowBlock <= _NumRowBlock; _rowBlock++)
                        {

                        }

                    }
                    */





                    for (int _count = 0; _count < ImageBuffer.Length; _count++)

                    // for (int _count = 0; _count < Width *2; _count++)
                    {
                        byte _currcol = (byte)(_count % Width);
                        byte _currRowBlock = (byte)(_count / Width);
                        for (byte _currRow = 0; _currRow < 8; _currRow++)
                        {
                            if (((byte)(ImageBuffer[_count] >> _currRow) & 0x01) == 0x01)
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.Black);
                            }
                            else
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.White);
                            }
                        }
                    }

                }
                catch
                {

                }
            }

            public void BmpToBuffer()
            {
                try
                {
                    int _tempWidth = bmp.Width;
                    int _tempHeight = bmp.Height;
                    int _arraysize = 0;

                    byte _NumRowBlock = 0;

                    _NumRowBlock = (byte)(_tempHeight / 8);

                    if ((_tempHeight % 8) != 0)
                    {
                        _NumRowBlock += 1;
                    }
                    _arraysize = _tempWidth * _NumRowBlock;

                    ImageBuffer = new byte[_arraysize];


                    for (int _rowBlock = 0; _rowBlock < _NumRowBlock; _rowBlock++)
                    {
                        for (int _CurrCol = 0; _CurrCol <= _tempWidth; _CurrCol++)
                        {
                            byte _actPix = 0x00;

                            for (int _rBlock = 0; _rBlock < 8; _rBlock++)
                            {
                                int _rigacorrente = _rowBlock * 8 + _rBlock;

                                if (_rigacorrente < _tempHeight)
                                {
                                    Color _pixelCorrente = bmp.GetPixel(_CurrCol, _rigacorrente);
                                    int grayScale = (int)((_pixelCorrente.R * 0.3) + (_pixelCorrente.G * 0.59) + (_pixelCorrente.B * 0.11));

                                    if (grayScale > 128)
                                    {
                                        int _tempBlock = (0x01 << _rBlock);
                                        _actPix = (byte)(_actPix & _tempBlock);
                                    }

                                }


                            }
                            int _posCorrente = _rowBlock * _tempWidth + _CurrCol;
                            ImageBuffer[_posCorrente] = _actPix;

                        }

                    }


                }
                catch
                {

                }
            }


            /*

                        // extension method
                        public static byte[] imageToByteArray(this System.Drawing.Image image)
                        {
                            using (var ms = new MemoryStream())
                            {
                                image.Save(ms, image.RawFormat);
                                return ms.ToArray();
                            }
                        }
            */


            public void GeneraImmagine()
            {
                try
                {
                    bmp = new Bitmap(Width, Height);

                    byte _NumRowBlock = 0;
                    byte _NumRow = 0;
                    byte _NumCol = 0;


                    for (int _count = 0; _count < ImageBuffer.Length; _count++)
                    {
                        byte _currcol = (byte)(_count % Width);
                        byte _currRowBlock = (byte)(_count / Width);
                        for (byte _currRow = 0; _currRow < 8; _currRow++)
                        {
                            if (((byte)(ImageBuffer[_count] >> _currRow) & 0x01) == 0x01)
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.Black);
                            }
                            else
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.White);
                            }
                        }
                    }

                }
                catch
                {

                }
            }

            public bool BmpFromBuffer()
            {
                try
                {
                    bmp = new Bitmap(Width, Height);

                    byte _NumRowBlock = 0;
                    byte _NumRow = 0;
                    byte _NumCol = 0;


                    for (int _count = 0; _count < ImageBuffer.Length; _count++)
                    {
                        byte _currcol = (byte)(_count % Width);
                        byte _currRowBlock = (byte)(_count / Width);
                        for (byte _currRow = 0; _currRow < 8; _currRow++)
                        {
                            if (((byte)(ImageBuffer[_count] >> _currRow) & 0x01) == 0x01)
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.Black);
                            }
                            else
                            {
                                bmp.SetPixel(_currcol, (_currRowBlock * 8 + (_currRow)), Color.White);
                            }
                        }
                    }
                    return true;
                }

                catch
                {
                    return false;
                }
            }





            public void Inizializza(byte Colonne, byte Righe)
            {
                try
                {
                    Nome = "IMAGE000";
                    Id = 0;
                    Numero = 0;
                    Lingua = 0;
                    Width = Colonne;
                    Height = Righe;
                    bmp = new Bitmap(Width, Height);
                    Size = (ushort)((Width * Height) / 8);
                    if (((Width * Height) % 8) != 0)
                    {
                        Size += 1;
                    }

                    ImageBuffer = new byte[Size];
                }
                catch
                {

                }
            }



            public bool getPixel(byte Col, byte Row)
            {
                try
                {
                    bool StatoPixel = false;
                    ushort index = 0;

                    //index = Row / 8;


                    return StatoPixel;
                }
                catch
                {
                    return false;
                }
            }

            public void SetDemo5()
            {

                byte[] Image005 = new byte[940]
                {
                    0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0xFC , 0xFC , 0xFC , 0xFC , 0xFC , 0xFC,
                    0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80,
                    0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80,
                    0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80,
                    0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0xFC , 0xFC,
                    0xFC , 0xFC , 0xFC , 0xFC , 0x80 , 0x80 , 0x80 , 0x80 , 0x80 , 0x00 , 0x00 , 0x00 , 0x00 , 0x00 , 0xF0 , 0xFC,
                    0xFE , 0xFF , 0x1F , 0x0F , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7,
                    0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7,
                    0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7,
 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7,
 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7,
 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0x0F , 0x1F , 0xFF , 0xFE , 0xFC , 0xF0 , 0xFF , 0xFF , 0xFF , 0xFF,
 0x00 , 0x00 , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F,
 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F,
 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F,
 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F,
 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F,
 0x9F , 0x9F , 0x9F , 0x9F , 0x00 , 0x00 , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0x00 , 0x00,
 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F,
 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F,
 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F,
 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F,
 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F , 0x7F,
 0x7F , 0x7F , 0x00 , 0x00 , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0x00 , 0x00 , 0xFE , 0xFE,
 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE,
 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE,
 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE,
 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE,
 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE , 0xFE,
 0x00 , 0x00 , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0x00 , 0x00 , 0xF9 , 0xF9 , 0xF9 , 0xF9,
 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9,
 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9,
 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9,
 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9,
 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0xF9 , 0x00 , 0x00,
 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0x00 , 0x00 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7,
 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7,
 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7,
 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7,
 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7,
 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0xE7 , 0x00 , 0x00 , 0xFF , 0xFF,
 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0x00 , 0x00 , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F,
 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F,
 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F,
 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F,
 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F,
 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x9F , 0x00 , 0x00 , 0xFF , 0xFF , 0xFF , 0xFF,
 0xFF , 0xFF , 0xFF , 0xFF , 0x00 , 0x00 , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF,
 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF,
 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF,
 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF,
 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF,
 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0xFF , 0x00 , 0x00 , 0xFF , 0xFF , 0xFF , 0xFF , 0x3F , 0x3F,
 0x3F , 0x3F , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C,
 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C,
 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C,
 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C,
 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C,
 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3C , 0x3F , 0x3F , 0x3F , 0x3F
 };

                Nome = "IMAGE005";
                Id = 0;
                Numero = 5;
                Lingua = 0;
                Width = 94;
                Height = 80;
                Size = 940;
                ImageBuffer = Image005;
                bmp = new Bitmap(Width, Height);
            }
        }


    }
}
