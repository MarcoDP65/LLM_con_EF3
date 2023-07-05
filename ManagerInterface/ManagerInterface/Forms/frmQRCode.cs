using Syncfusion.Windows.Forms.Tools.Renderers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing.Common;
using ZXing.Rendering;
using ZXing;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Newtonsoft.Json;
using System.IO;
using Syncfusion.Windows.Forms.Tools;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Drawing.Layout;
using Utility;

namespace PannelloCharger.Forms
{
    partial class frmQRCode : Form
    {
        private EncodingOptions EncodingOptions { get; set; }
        private Type Renderer { get; set; }
        public frmQRCode()
        {
            InitializeComponent();
            Renderer = typeof(BitmapRenderer);

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        public bool CreaQR()
        {
            try
            {
                if (lblDataArray.Text == "") return false;
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = EncodingOptions ?? new EncodingOptions
                    {
                        Height = picQRCode.Height,
                        Width = picQRCode.Width
                    },
                    Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(Renderer)
                };
                picQRCode.Image = writer.Write(lblDataArray.Text);
                if(lblNomeProfilo.Text !="")
                {
                    txtNomeFilePDF.Text = PannelloCharger.Properties.Settings.Default.pathFilesProfili + "\\" + lblNomeProfilo.Text + ".pdf";
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool CreaQR(IDBattData Data)
        {
            try
            {
                string TempNome = "";
                if (lblDataArray.Text == "") return false;
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = EncodingOptions ?? new EncodingOptions
                    {
                        Height = picQRCode.Height,
                        Width = picQRCode.Width
                    },
                    Renderer = (IBarcodeRenderer<Bitmap>)Activator.CreateInstance(Renderer)
                };
                picQRCode.Image = writer.Write(JsonConvert.SerializeObject(Data));
                if (lblNomeProfilo.Text != "")
                {
                    TempNome = PannelloCharger.Properties.Settings.Default.pathFilesProfili;
                    if (TempNome != "") TempNome += "\\";
                    txtNomeFilePDF.Text = TempNome + lblNomeProfilo.Text + ".pdf";
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void frmQRCode_Load(object sender, EventArgs e)
        {

        }

        private void btnNomeFilePDFSRC_Click(object sender, EventArgs e)
        {
            if (sfdExportDati.InitialDirectory == "") sfdExportDati.InitialDirectory = PannelloCharger.Properties.Settings.Default.pathFilesProfili;
            sfdExportDati.Filter = "PDF File (*.pdf)|*.pdf|All files (*.*)|*.*";
            DialogResult esito = sfdExportDati.ShowDialog();
            if (esito == DialogResult.OK)
            {
                txtNomeFilePDF.Text = sfdExportDati.FileName;
                PannelloCharger.Properties.Settings.Default.pathFilesProfili = Path.GetDirectoryName(sfdExportDati.FileName);

            }
        }

        private void btnSalvaFilePDF_Click(object sender, EventArgs e)
        {
            try
            {


                if (txtNomeFilePDF.Text != "")
                {
                    PdfDocument document;
                    // Create a temporary file

                    document = new PdfDocument();
                    document.Info.Title = "Charge Profile Data Sheet";
                    document.Info.Author = "Mori Raddrizzatori";
                    document.Info.Subject = "Scheda dati profilo di carica per caricabatterie famiglia LADE Light";
                    //document.Info.Keywords = "PDFsharp, XGraphics";

                    // Create an empty page
                    PdfPage page = document.AddPage();

                    // Get an XGraphics object for drawing
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    //Fondino
                    //Stampo la filigrana
                    Image _tempFiligrana = PannelloCharger.Properties.Resources.fondino;
                    MemoryStream mS1 = new MemoryStream();
                    _tempFiligrana.Save(mS1, System.Drawing.Imaging.ImageFormat.Jpeg);
                    XImage imageF = XImage.FromStream(mS1);
                    gfx.DrawImage(imageF, 50, 100, 495, 700);




                    // Penna Contorni
                    XPen penBorder = new XPen(XColors.Black, 0.1);

                    // Create the fonts
                    XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
                    XFont fntHexDump = new XFont("Courier New", 10, XFontStyle.Regular);
                    XFont fntTitolo = new XFont("Verdana", 18, XFontStyle.BoldItalic);
                    XFont fntTesti = new XFont("Verdana", 14, XFontStyle.Bold);
                    XFont fntDescrizioni = new XFont("Verdana", 12, XFontStyle.Italic);
                    XFont fntLabel = new XFont("Verdana", 6, XFontStyle.Regular);
                    XFont fntValori = new XFont("Verdana", 18, XFontStyle.Bold);
                    XFont fntUm = new XFont("Verdana",14, XFontStyle.Bold);


                    XTextFormatter tf = new XTextFormatter(gfx);
                    XRect rect ;
                    XSize RoundedCorner = new XSize(10,10);

                    // Titolo Pagina
                    gfx.DrawString("ID BATT Profile Parameters", fntTitolo, XBrushes.Black, new XRect(70, 70, 455, 50), XStringFormats.TopLeft);

                    gfx.DrawString(lblNomeProfilo.Text, fntTesti, XBrushes.Black, new XRect(70, 150, 455, 50), XStringFormats.TopLeft);
                    gfx.DrawString(lblDescrizione.Text, fntDescrizioni, XBrushes.Black, new XRect(70, 170, 455, 50), XStringFormats.TopLeft);

                    gfx.DrawString(lblTensione.Text, fntValori, XBrushes.Black, new XRect(70, 200, 70, 20), XStringFormats.BottomRight);
                    gfx.DrawString("V", fntUm, XBrushes.Black, new XRect(145, 200, 20, 20), XStringFormats.BottomLeft);

                    gfx.DrawString(lblCorrente.Text, fntValori, XBrushes.Black, new XRect(170, 200, 70, 20), XStringFormats.BottomRight);
                    gfx.DrawString("Ah", fntUm, XBrushes.Black, new XRect(245, 200, 20, 20), XStringFormats.BottomLeft);



                    // Hexdump profilo
                    gfx.DrawString("Profile Data", fntLabel, XBrushes.Black, new XRect(70, 340, 100, 10), XStringFormats.TopLeft);

                    rect = new XRect(70, 350, 455, 140);
                    gfx.DrawRoundedRectangle(penBorder, XBrushes.WhiteSmoke,rect, RoundedCorner);

                    rect = new XRect(85, 365, 425, 110);
                    //gfx.DrawRectangle(XBrushes.WhiteSmoke, rect);
                    tf.Alignment = XParagraphAlignment.Justify;
                    tf.DrawString(FunzioniMR.LongWordWrap(lblDataArray.Text, 70), fntHexDump, XBrushes.Black, rect, XStringFormats.TopLeft);


                    //QR Code
                    MemoryStream mS = new MemoryStream();
                    picQRCode.Image.Save(mS, System.Drawing.Imaging.ImageFormat.Jpeg);
                    XImage image = XImage.FromStream(mS);
                    gfx.DrawImage(image, 425, 150,100,100);
                    gfx.DrawRoundedRectangle(penBorder, XBrushes.Transparent, 425, 150, 100, 100, 10, 10);


                    //new Text().DrawPage(s_document.AddPage());
                    //new Images().DrawPage(s_document.AddPage());


                    // Save the document...
                    document.Save(txtNomeFilePDF.Text);
                    MessageBox.Show("File PDF Generato", "QR Code", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Creazione file non riuscita", "QR Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
