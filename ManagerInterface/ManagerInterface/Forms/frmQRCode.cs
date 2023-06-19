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
using Newtonsoft.Json;

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
    }
}
