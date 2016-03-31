using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System.Linq;
using System.Text;
using NextUI.Component;
using NextUI.Frame;
using System.Drawing;

using PannelloCharger;

namespace ChargerLogic
{
    public class IndicatoreCruscotto
    {

        public enum VersoValori: byte { Ascendente = 0x00, Discendente = 0x01};

        public NextUI.BaseUI.BaseUI cruscotto;
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int size { get; set; }

        public string ValueMask { get; set; }

        public bool MostraValore { get; set; }

        public float MinVal { get; set; }
        public float MaxVal { get; set; }
        public float Lim1 { get; set; }
        public float Lim2 { get; set; }
        public VersoValori Verso { get; set; }
        public int NumRighe { get; set; }
        public int LabelOffset { get; set; }

        //SizeF _dimEtichetta;
        int _NumSezioni;
        SezioneScala[] Sezioni ;



        // definisco un formato testo 'anglosassone' per la formattazione dei nueri nei pannelli grafici ... separatore decimale 'obbligato' è il punto
        public NumberFormatInfo Nfi = new CultureInfo("en-US", false).NumberFormat;

        public string Etichetta;

        private float _valore;

        private CircularFrame _frame;


        public IndicatoreCruscotto()
        {
            PosX = 0;
            PosY = 0;
            MinVal = 0;
            Lim1 = 60;
            Lim2 = 80;
            MaxVal = 100;
            Verso = VersoValori.Ascendente;
            _valore = MinVal;
            MostraValore = true;
            Etichetta = "";
            ValueMask = "0.00";
            _NumSezioni = 3;
            NumRighe = 1;
            LabelOffset = 0;
        }

        public void InizializzaIndicatore(NextUI.BaseUI.BaseUI Cruscotto, int X, int Y, int Size, string Titolo)
        {
            cruscotto = Cruscotto;
            PosX = X;
            PosY = Y;
            size = Size;
            Etichetta = Titolo;
            InizializzaIndicatore();
        }


        public void InizializzaSoglie()
        {
            try
            {
                SezioneScala _sez;

                Sezioni = new SezioneScala[_NumSezioni];

                if(Verso == VersoValori.Ascendente)
                {
                    if(MinVal<MaxVal)
                    {
                        // min --> verde --> L1 --> giallo --> L2 --> Rosso --> Max
                        //VERDE
                        _sez = new SezioneScala();
                        _sez.minimo = MinVal;
                        _sez.massimo = Lim1;
                        _sez.coloreSezione = Color.Green;
                        Sezioni[0] = _sez;
                        //GIALLO
                        _sez = new SezioneScala();
                        _sez.minimo = Lim1;
                        _sez.massimo = Lim2;
                        _sez.coloreSezione = Color.Yellow;
                        Sezioni[1] = _sez;
                        //ROSSO
                        _sez = new SezioneScala();
                        _sez.minimo = Lim2;
                        _sez.massimo = MaxVal;
                        _sez.coloreSezione = Color.Red;
                        Sezioni[2] = _sez;
                    }
                    else
                    {
                        // max --> verde --> L1 --> giallo --> L2 --> Rosso --> min
                        //VERDE
                        _sez = new SezioneScala();
                        _sez.minimo = Lim2;
                        _sez.massimo = MaxVal;
                        _sez.coloreSezione = Color.Green;
                        Sezioni[0] = _sez;
                        //GIALLO
                        _sez = new SezioneScala();
                        _sez.minimo = Lim1;
                        _sez.massimo = Lim2;
                        _sez.coloreSezione = Color.Yellow;
                        Sezioni[1] = _sez;
                        //ROSSO
                        _sez = new SezioneScala();
                        _sez.minimo = Lim1;
                        _sez.massimo = MinVal;
                        _sez.coloreSezione = Color.Red;
                        Sezioni[2] = _sez;

                    }


                }
                else
                {
                    if (MinVal < MaxVal)
                    {
                        // min --> Rosso --> L1 --> giallo --> L2 --> Verde --> Max
                        //VERDE
                        _sez = new SezioneScala();
                        _sez.minimo = MinVal;
                        _sez.massimo = Lim1;
                        _sez.coloreSezione = Color.Red;
                        Sezioni[0] = _sez;
                        //GIALLO
                        _sez = new SezioneScala();
                        _sez.minimo = Lim1;
                        _sez.massimo = Lim2;
                        _sez.coloreSezione = Color.Yellow;
                        Sezioni[1] = _sez;
                        //ROSSO
                        _sez = new SezioneScala();
                        _sez.minimo = Lim2;
                        _sez.massimo = MaxVal;
                        _sez.coloreSezione = Color.Green;
                        Sezioni[2] = _sez;
                    }
                }


            }
            catch
            {

            }
        }

        public void InizializzaIndicatore(int IdTemplate = 5)
        {
            try
            {
                

                switch (IdTemplate)
                {
                    case 3:
                        InizializzaIndicatoreTre();
                        break;
                    case 4:
                        InizializzaIndicatoreQuattro();
                        break;
                    case 5:
                        InizializzaIndicatoreCinque();
                        break;

                    default:
                        InizializzaIndicatoreCinque();
                        break;

                }



            }
            catch
            {

            }

        }



        public void InizializzaIndicatoreCinque()
        {
            try
            {
                _frame = new CircularFrame(new Point(PosX, PosY), size);
                cruscotto.Frame.Add(_frame);
                _frame.BackRenderer.CenterColor = Color.LightGray;
                _frame.BackRenderer.EndColor = Color.DimGray;
                //_frame.BackRenderer.CenterColor = Color.White;
                //_frame.BackRenderer.EndColor = Color.WhiteSmoke;

                _frame.FrameRenderer.Outline = NextUI.Renderer.FrameRender.FrameOutline.None;



                Image _tempImg = PannelloCharger.Properties.Resources.cinquea;


                _frame.FrameImage = _tempImg;

                InizializzaSoglie();

                CircularScaleBar bar = new CircularScaleBar(_frame);
                bar.OffsetFromFrame = size * 1 / 10;
                //bar.
                bar.FillGradientType = NextUI.Renderer.RendererGradient.GradientType.DiagonalRight;
                bar.ScaleBarSize = 4;
                bar.FillColor = Color.DarkGray;

                bar.StartValue = MinVal;
                bar.EndValue = MaxVal;
                bar.StartAngle = 30;
                bar.SweepAngle = 120;
                bar.MajorTickNumber = 11;
                bar.MinorTicknumber = 2;
                bar.TickMajor.EnableGradient = false;
                bar.TickMajor.EnableBorder = false;
                bar.TickMajor.FillColor = Color.White;
                //bar.TickMajor.FillColor = Color.Black;
                bar.TickMajor.Height = size / 20; // 15;
                bar.TickMajor.Width = 2;
                bar.TickMajor.Type = TickBase.TickType.RoundedRect;
                bar.TickMinor.EnableGradient = false;
                bar.TickMinor.EnableBorder = false;
                bar.TickMinor.FillColor = Color.Gray;
                bar.TickMajor.TickPosition = TickBase.Position.Inner;
                bar.TickMinor.TickPosition = TickBase.Position.Inner;
                bar.TickLabel.TextDirection = CircularLabel.Direction.Horizontal;
                bar.TickLabel.OffsetFromScale = size / 8; //35;
                bar.TickLabel.LabelFont = new Font(FontFamily.GenericMonospace, FontSize() - 1, FontStyle.Bold);
                bar.TickLabel.FontColor = Color.LightYellow;
                //bar.TickLabel.FontColor = Color.DarkGray;
                _frame.ScaleCollection.Add(bar);

                double _textWidth;
                if (Etichetta != "")
                {
                  
                    using (Bitmap tempImage = new Bitmap(400, 400))
                    {
                        SizeF stringSize = Graphics.FromImage(tempImage).MeasureString(Etichetta, bar.TickLabel.LabelFont);
                        _textWidth = stringSize.Width;
                    }



                    int _labelX = (int)((_frame.Rect.Width / 2) - ( _textWidth /  2 )) + 8 ;
                    int _labelY = _frame.Rect.Height * 7 / 12;
   

                    FrameLabel _titleLabel = new FrameLabel(new Point(_labelX, _labelY), _frame);
                   
                    _titleLabel.LabelText = Etichetta;
                    _titleLabel.LabelFont = new Font(FontFamily.GenericSansSerif, FontSize() + 2, FontStyle.Bold);
                   

                    _titleLabel.FontColor = Color.Yellow;

                    _frame.FrameLabelCollection.Add(_titleLabel);

                }


                for( int _ciclor = 0; _ciclor < _NumSezioni; _ciclor++ )
                {
                    CircularRange _tempRange = new CircularRange(_frame);
                    _tempRange.EnableGradient = false;
                    _tempRange.StartValue = Sezioni[_ciclor].minimo;
                    _tempRange.EndValue = Sezioni[_ciclor].massimo;
                    _tempRange.StartWidth = 10;
                    _tempRange.EndWidth = 10;
                    _tempRange.RangePosition = RangeBase.Position.Inner;
                    _tempRange.FillColor = Sezioni[_ciclor].coloreSezione;

                    bar.Range.Add(_tempRange);
                }


                CircularPointer pointer = new CircularPointer(_frame);
                pointer.CapPointer.Visible = true;
                pointer.CapOnTop = false;
                pointer.BasePointer.Length = size * 2 / 5; // 150;
                pointer.BasePointer.FillColor = Color.Red;
                pointer.BasePointer.PointerShapeType = Pointerbase.PointerType.Type2;
                pointer.BasePointer.OffsetFromCenter = -size / 10; // - 30;

                bar.Pointer.Add(pointer);
                if (MostraValore)
                {
                    NumericalFrame nframe = new NumericalFrame(new Rectangle(_frame.Rect.Width / 2 - 50, _frame.Rect.Height - 75,100, 25));
                    nframe.FrameRenderer.Outline = NextUI.Renderer.FrameRender.FrameOutline.Type3;
                    nframe.FrameRenderer.FrameWidth = 0;

                    for (int i = 0; i < 6; i++)
                    {
                        DigitalPanel7Segment seg = new DigitalPanel7Segment(nframe);
                        //DigitalPanel14Segment seg = new DigitalPanel14Segment(nframe);
                        seg.BackColor = Color.Black;
                        seg.FontThickness = 3;
                        seg.BorderColor = Color.Black;
                        seg.MainColor = Color.Yellow;
                        seg.EnableBorder = true;
                        seg.EnableGlare = false;
                        seg.EnableGradient = false;
                        seg.BackOpacity = 0;
                        //seg.EnableGlare = true;
                        nframe.Indicator.Panels.Add(seg);


                    }
                    _frame.FrameCollection.Add(nframe);

                }

            }
            catch
            {

            }

        }

        public void InizializzaIndicatoreQuattro()
        {
            try
            {
                _frame = new CircularFrame(new Point(PosX, PosY), size);
                cruscotto.Frame.Add(_frame);
                _frame.BackRenderer.CenterColor = Color.White;
                _frame.BackRenderer.EndColor = Color.WhiteSmoke;
                //_frame.BackRenderer.CenterColor = Color.White;
                //_frame.BackRenderer.EndColor = Color.WhiteSmoke;

                _frame.FrameRenderer.Outline = NextUI.Renderer.FrameRender.FrameOutline.None;



                Image _tempImg = PannelloCharger.Properties.Resources.cinquea;


                _frame.FrameImage = _tempImg;

                InizializzaSoglie();

                CircularScaleBar bar = new CircularScaleBar(_frame);
                bar.OffsetFromFrame = size * 1 / 10;
                //bar.
                bar.FillGradientType = NextUI.Renderer.RendererGradient.GradientType.DiagonalRight;
                bar.ScaleBarSize = 4;
                bar.FillColor = Color.DarkGray;

                bar.StartValue = MinVal;
                bar.EndValue = MaxVal;
                bar.StartAngle = 30;
                bar.SweepAngle = 120;
                bar.MajorTickNumber = 11;
                bar.MinorTicknumber = 2;
                bar.TickMajor.EnableGradient = false;
                bar.TickMajor.EnableBorder = false;
                bar.TickMajor.FillColor = Color.Black;
                //bar.TickMajor.FillColor = Color.Black;
                bar.TickMajor.Height = size / 20; // 15;
                bar.TickMajor.Width = 2;
                bar.TickMajor.Type = TickBase.TickType.RoundedRect;
                bar.TickMinor.EnableGradient = false;
                bar.TickMinor.EnableBorder = false;
                bar.TickMinor.FillColor = Color.Gray;
                bar.TickMajor.TickPosition = TickBase.Position.Inner;
                bar.TickMinor.TickPosition = TickBase.Position.Inner;
                bar.TickLabel.TextDirection = CircularLabel.Direction.Horizontal;
                bar.TickLabel.OffsetFromScale = size / 8; //35;
                bar.TickLabel.LabelFont = new Font(FontFamily.GenericMonospace, FontSize() - 1, FontStyle.Bold);
                bar.TickLabel.FontColor = Color.Black;
                //bar.TickLabel.FontColor = Color.DarkGray;
                _frame.ScaleCollection.Add(bar);

                double _textWidth;
                if (Etichetta != "")
                {

                    using (Bitmap tempImage = new Bitmap(400, 400))
                    {
                        SizeF stringSize = Graphics.FromImage(tempImage).MeasureString(Etichetta, bar.TickLabel.LabelFont);
                        _textWidth = stringSize.Width;
                    }



                    int _labelX = (int)((_frame.Rect.Width / 2) - (_textWidth / 2)) + 8;
                    int _labelY = _frame.Rect.Height * 7 / 12;


                    FrameLabel _titleLabel = new FrameLabel(new Point(_labelX, _labelY), _frame);

                    _titleLabel.LabelText = Etichetta;
                    _titleLabel.LabelFont = new Font(FontFamily.GenericSansSerif, FontSize() + 2, FontStyle.Bold);

                    // Titolo Indicatore
                    _titleLabel.FontColor = Color.Black;

                    _frame.FrameLabelCollection.Add(_titleLabel);

                }


                for (int _ciclor = 0; _ciclor < _NumSezioni; _ciclor++)
                {
                    CircularRange _tempRange = new CircularRange(_frame);
                    _tempRange.EnableGradient = false;
                    _tempRange.StartValue = Sezioni[_ciclor].minimo;
                    _tempRange.EndValue = Sezioni[_ciclor].massimo;
                    _tempRange.StartWidth = 10;
                    _tempRange.EndWidth = 10;
                    _tempRange.RangePosition = RangeBase.Position.Inner;
                    _tempRange.FillColor = Sezioni[_ciclor].coloreSezione;

                    bar.Range.Add(_tempRange);
                }


                CircularPointer pointer = new CircularPointer(_frame);
                pointer.CapPointer.Visible = true;
                pointer.CapOnTop = false;
                pointer.BasePointer.Length = size * 2 / 5; // 150;
                pointer.BasePointer.FillColor = Color.Red;
                pointer.BasePointer.PointerShapeType = Pointerbase.PointerType.Type2;
                pointer.BasePointer.OffsetFromCenter = -size / 10; // - 30;

                bar.Pointer.Add(pointer);
                if (MostraValore)
                {
                    NumericalFrame nframe = new NumericalFrame(new Rectangle(_frame.Rect.Width / 2 - 50, _frame.Rect.Height - 75, 100, 25));
                    nframe.FrameRenderer.Outline = NextUI.Renderer.FrameRender.FrameOutline.Type3;
                    nframe.FrameRenderer.FrameWidth = 0;

                    for (int i = 0; i < 6; i++)
                    {
                        DigitalPanel7Segment seg = new DigitalPanel7Segment(nframe);
                        //DigitalPanel14Segment seg = new DigitalPanel14Segment(nframe);
                        seg.BackColor = Color.Black;
                        seg.FontThickness = 3;
                        seg.BorderColor = Color.Black;
                        seg.MainColor = Color.Yellow;
                        seg.EnableBorder = true;
                        seg.EnableGlare = false;
                        seg.EnableGradient = false;
                        seg.BackOpacity = 0;
                        //seg.EnableGlare = true;
                        nframe.Indicator.Panels.Add(seg);


                    }
                    _frame.FrameCollection.Add(nframe);

                }

            }
            catch
            {

            }

        }



        public void InizializzaIndicatoreTre()
        {
            try
            {
                _frame = new CircularFrame(new Point(PosX, PosY), size);
                cruscotto.Frame.Add(_frame);
                _frame.BackRenderer.CenterColor = Color.LightGray;
                _frame.BackRenderer.EndColor = Color.DimGray;
                //_frame.BackRenderer.CenterColor = Color.White;
                //_frame.BackRenderer.EndColor = Color.WhiteSmoke;

                _frame.FrameRenderer.Outline = NextUI.Renderer.FrameRender.FrameOutline.None;



                Image _tempImg = PannelloCharger.Properties.Resources.trec;
                //Image _tempImg = PannelloCharger.Properties.Resources.cinquea;


                _frame.FrameImage = _tempImg;


                //_frame.BackImage = Image.FromFile("C:\\log\\due.png");
                InizializzaSoglie();

                CircularScaleBar bar = new CircularScaleBar(_frame);
                bar.OffsetFromFrame = size * 1 / 10;
                //bar.
                bar.FillGradientType = NextUI.Renderer.RendererGradient.GradientType.DiagonalRight;
                bar.ScaleBarSize = 4;
                bar.FillColor = Color.DarkGray;

                bar.StartValue = MinVal;
                bar.EndValue = MaxVal;
                bar.StartAngle = 30;
                bar.SweepAngle = 120;
                bar.MajorTickNumber = 11;
                bar.MinorTicknumber = 2;
                bar.TickMajor.EnableGradient = false;
                bar.TickMajor.EnableBorder = false;
                bar.TickMajor.FillColor = Color.White;
                //bar.TickMajor.FillColor = Color.Black;
                bar.TickMajor.Height = size / 20; // 15;
                bar.TickMajor.Width = 2;
                bar.TickMajor.Type = TickBase.TickType.RoundedRect;
                bar.TickMinor.EnableGradient = false;
                bar.TickMinor.EnableBorder = false;
                bar.TickMinor.FillColor = Color.Gray;
                bar.TickMajor.TickPosition = TickBase.Position.Inner;
                bar.TickMinor.TickPosition = TickBase.Position.Inner;
                bar.TickLabel.TextDirection = CircularLabel.Direction.Horizontal;
                bar.TickLabel.OffsetFromScale = size / 8; //35;
                bar.TickLabel.LabelFont = new Font(FontFamily.GenericMonospace, FontSize() - 1, FontStyle.Bold);
                bar.TickLabel.FontColor = Color.LightYellow;
                //bar.TickLabel.FontColor = Color.DarkGray;
                _frame.ScaleCollection.Add(bar);

                double _textWidth;
                if (Etichetta != "")
                {

                    using (Bitmap tempImage = new Bitmap(400, 400))
                    {
                        SizeF stringSize = Graphics.FromImage(tempImage).MeasureString(Etichetta, bar.TickLabel.LabelFont);
                        _textWidth = stringSize.Width;
                    }



                    int _labelX = (int)((_frame.Rect.Width / 2) - (_textWidth / 2)) + 8;
                    int _labelY = _frame.Rect.Height * 7 / 12;


                    FrameLabel _titleLabel = new FrameLabel(new Point(_labelX, _labelY), _frame);

                    _titleLabel.LabelText = Etichetta;
                    _titleLabel.LabelFont = new Font(FontFamily.GenericSansSerif, FontSize() + 2, FontStyle.Bold);


                    _titleLabel.FontColor = Color.Yellow;

                    _frame.FrameLabelCollection.Add(_titleLabel);

                }


                for (int _ciclor = 0; _ciclor < _NumSezioni; _ciclor++)
                {
                    CircularRange _tempRange = new CircularRange(_frame);
                    _tempRange.EnableGradient = false;
                    _tempRange.StartValue = Sezioni[_ciclor].minimo;
                    _tempRange.EndValue = Sezioni[_ciclor].massimo;
                    _tempRange.StartWidth = 10;
                    _tempRange.EndWidth = 10;
                    _tempRange.RangePosition = RangeBase.Position.Inner;
                    _tempRange.FillColor = Sezioni[_ciclor].coloreSezione;

                    bar.Range.Add(_tempRange);
                }


                CircularPointer pointer = new CircularPointer(_frame);
                pointer.CapPointer.Visible = true;
                pointer.CapOnTop = false;
                pointer.BasePointer.Length = size * 2 / 5; // 150;
                pointer.BasePointer.FillColor = Color.Red;
                pointer.BasePointer.PointerShapeType = Pointerbase.PointerType.Type2;
                pointer.BasePointer.OffsetFromCenter = -size / 10; // - 30;

                bar.Pointer.Add(pointer);
                if (MostraValore)
                {
                    NumericalFrame nframe = new NumericalFrame(new Rectangle(_frame.Rect.Width / 2 - 50, _frame.Rect.Height - 75, 100, 25));
                    nframe.FrameRenderer.Outline = NextUI.Renderer.FrameRender.FrameOutline.Type3;
                    nframe.FrameRenderer.FrameWidth = 0;

                    for (int i = 0; i < 6; i++)
                    {
                        DigitalPanel7Segment seg = new DigitalPanel7Segment(nframe);
                        //DigitalPanel14Segment seg = new DigitalPanel14Segment(nframe);
                        seg.BackColor = Color.Black;
                        seg.FontThickness = 3;
                        seg.BorderColor = Color.Black;
                        seg.MainColor = Color.Yellow;
                        seg.EnableBorder = true;
                        seg.EnableGlare = false;
                        seg.EnableGradient = false;
                        seg.BackOpacity = 0;
                        //seg.EnableGlare = true;
                        nframe.Indicator.Panels.Add(seg);


                    }
                    _frame.FrameCollection.Add(nframe);

                }

            }
            catch
            {

            }

        }


        public void ImpostaValore (float valore)
        {
            try
            {
                _valore = valore;
                float _tempval = Valore;
                _frame.ScaleCollection[0].Pointer[0].Value = _tempval;
                if (MostraValore)
                {
                    ((NumericalFrame)_frame.FrameCollection[0]).Indicator.DisplayValue = _tempval.ToString(ValueMask, Nfi);
                }
            }
            catch { }

        }

        public float Valore
        {
            get
            {
                if (_valore > MaxVal) return MaxVal;
                if (_valore < MinVal) return MinVal;
                return _valore;
            }
            set
            {
                _valore = value;
        
            }
        }

        private int FontSize()
        {
            int _fsize;
            _fsize = size / 20;
            if (_fsize < 6) _fsize = 6;
            return _fsize;
        }


    }



    public class SezioneScala
    {
        public IndicatoreCruscotto.VersoValori Verso { get; set; }

        public float minimo { get; set; }
        public float massimo { get; set; }
        public Color coloreSezione { get; set; }


    }



}
