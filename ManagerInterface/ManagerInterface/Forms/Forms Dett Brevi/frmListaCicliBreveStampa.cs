using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.IO;
using MoriData;
using ChargerLogic;
using NextUI.Component;
using NextUI.Frame;

//using Common.Logging;
using BrightIdeasSoftware;
using Utility;
using MdiHelper;
using log4net;
using log4net.Config;

using OxyPlot;
using OxyPlot.WindowsForms;



namespace PannelloCharger
{
    public partial class frmListaCicliBreve : Form
    {

        private PrintDocument _docStampa = new PrintDocument();

        public const int DeltaIntestazione = 15;
        public const int DeltaRiga = 40;
        public const int DeltaZona = 100;


        private int _paginaCorrente = 0;
//        private int _modelloStampa = 0;


        /// <summary>
        /// lancio la stampa del form.
        /// In funzione della pagina visualizzata, lancio la stampa specifica.
        /// </summary>
        /// <param name="preview"></param>
        /// <param name="SelPrinter"></param>
        public void stampa(bool preview = false, bool SelPrinter = false)
        {
            try
            {


//                _modelloStampa = 0;


                // Inizializzo il documento
                _docStampa.Dispose();
                _docStampa = new PrintDocument();

                PrintDialog printDlg;
                PrintPreviewDialog ppdlg;

                _paginaCorrente = 0;


                _docStampa.PrinterSettings = _parametri.ImpostazioniStampante;
                _docStampa.PrintPage += new PrintPageEventHandler(_docStampa_PrintPage);

                DialogResult _setPrinter;

                // Se è richiesto il pannello setup stampante
                if (SelPrinter)
                {
                    printDlg = new PrintDialog();
                    if (_parametri.ImpostazioniStampante != null)
                    {
                        printDlg.PrinterSettings = _parametri.ImpostazioniStampante;
                    }

                    _setPrinter = printDlg.ShowDialog();
                    if (_setPrinter == DialogResult.OK)
                    {
                        _parametri.ImpostazioniStampante = printDlg.PrinterSettings;
                    }

                }
                else
                {
                    _setPrinter = DialogResult.OK;

                }

                
                if (_parametri.ImpostazioniStampante != null)
                {
                    _docStampa.PrinterSettings = _parametri.ImpostazioniStampante;
                }
                
                //Se richiesto il preview lo lancio se no mando in stampa direttamente
                if (_setPrinter == DialogResult.OK)
                {
                    if (preview)
                    {
                        ppdlg = new PrintPreviewDialog();
                        ppdlg.Document = _docStampa;
                        ppdlg.ShowDialog();
                    }

                    else
                    {
                        _docStampa.Print();
                    }

                }

            }
            catch (Exception Ex)
            {
                Log.Error("frmListaCicliBreve: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }
        }


        /// <summary>
        /// In base alla selezione e all'avanzamento genero la pagina utile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="doc"></param>
        public void _docStampa_PrintPage(object sender, PrintPageEventArgs doc)
        {
            try
            {
                _paginaCorrente++;
                _docStampaPaginaGrafici(doc, _paginaCorrente);
                doc.HasMorePages = false;

            }
            catch (Exception Ex)
            {
                Log.Error("_docStampa_PrintPage: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }
        }


        /// <summary>
        /// Stampa la scheda grafici
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pagina"></param>
        public void _docStampaPaginaGrafici(PrintPageEventArgs doc, int pagina)
        {
            try
            {
                Graphics graphcs = doc.Graphics;
                Image _tempImg = PannelloCharger.Properties.Resources.spyBatt;
                Bitmap logo = new Bitmap(_tempImg);

                PreparaPagina(graphcs, 0);

                // doc.PageSettings
                int _rigaLabel = 100;
                //int rigaTesto = 120;




                Font _etichette = new Font("Arial", 8);
                Font _valoriBase = new Font("Arial", 10);
                Font _valoriBold = new Font("Arial", 10, FontStyle.Bold);
                Font _titoloBold = new Font("Arial", 24, FontStyle.Bold);
                Font _sottoTitoloBold = new Font("Arial", 18, FontStyle.Bold);



                SolidBrush _pennaBase = new SolidBrush(Color.Black);

                String _titoloPagina = oxyGraficoCiclo.Title + "  ";
                _titoloPagina += CicloLungo.IdMemoriaLunga.ToString();

                _titoloPagina += " - " + CicloLungo.dtDataOraStart.ToShortDateString();

                graphcs.DrawString(_titoloPagina, _sottoTitoloBold, _pennaBase, 50, 190);

                //Calcolo Gli indicatori previsti per questa pagina





                //---------------------------------------------------------

                //Calcolo Gli indicatori previsti per questa pagina

                Image _tmpGraph;


                // Parte alta
                _tmpGraph = immagineGrafico(oxyGraficoCiclo,1200,700);
                if (_tmpGraph != null)
                {
                    graphcs.DrawImage(_tmpGraph, 120, 250, 600, 350);
                }


                //Parte Bassa
                _tmpGraph = immagineGrafico(oxyGraficoTensioni, 1200, 700);
                if (_tmpGraph != null)
                {
                    graphcs.DrawImage(_tmpGraph, 120, 640, 600, 350);

                }

                // Configurazione
                _rigaLabel = 1010;
                graphcs.DrawString(grbProgrammazione.Text, _valoriBold, _pennaBase, 50, _rigaLabel);


                _rigaLabel += DeltaRiga;
                graphcs.DrawString(txtTensioneNom.Text, _etichette, _pennaBase, 100, _rigaLabel);
                graphcs.DrawString(txtTensioneNominale.Text, _valoriBase, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label7.Text, _etichette, _pennaBase, 200, _rigaLabel);
                graphcs.DrawString(txtCapacitaNominale.Text, _valoriBase, _pennaBase, 200, _rigaLabel + DeltaIntestazione);
                
                graphcs.DrawString(label3.Text, _etichette, _pennaBase, 300, _rigaLabel);
                graphcs.DrawString(txtCelleTot.Text, _valoriBase, _pennaBase, 300, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label2.Text, _etichette, _pennaBase, 400, _rigaLabel);
                graphcs.DrawString(txtCelleV3.Text, _valoriBase, _pennaBase, 400, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label1.Text, _etichette, _pennaBase, 500, _rigaLabel);
                graphcs.DrawString(txtCelleV2.Text, _valoriBase, _pennaBase, 500, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblCelleP1.Text, _etichette, _pennaBase, 600, _rigaLabel);
                graphcs.DrawString(txtCelleV1.Text, _valoriBase, _pennaBase, 600, _rigaLabel + DeltaIntestazione); 




            }
            catch (Exception Ex)
            {
                Log.Error("_docStampaScheda: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }
        }

        /// <summary>
        /// Inizializzo la pagina con loghi, filigrana e intestazione
        /// </summary>
        /// <param name="Pagina"></param>
        /// <param name="NumeroPagina">Numero della pagina; Non stampato se 0</param>
        private void PreparaPagina(Graphics Pagina, int NumeroPagina)
        {
            try
            {

                Image _tempImg = PannelloCharger.Properties.Resources.spyBatt;
                Bitmap logo = new Bitmap(_tempImg);


                //Stampo la filigrana
                Image _tempFiligrana = PannelloCharger.Properties.Resources.fondino_chiaro;
                Pagina.DrawImage(_tempFiligrana, 0, 0);
                // doc.PageSettings
                int _rigaLabel = 50;
                int rigaTesto = 120;



                Font _etichette = new Font("Arial", 7);
                Font _valoriBase = new Font("Arial", 10);
                Font _valoriBold = new Font("Arial", 10, FontStyle.Bold);
                Font _titolo = new Font("Arial", 24, FontStyle.Regular);
                Font _titoloBold = new Font("Arial", 24, FontStyle.Bold);
                Font _sottoTitolo = new Font("Arial", 16, FontStyle.Regular);
                Font _sottoTitoloBold = new Font("Arial", 18, FontStyle.Bold);

                Font _testoHeadBase = new Font("Arial", 10, FontStyle.Regular);

                Font _testoHead = new Font("Arial", 10, FontStyle.Regular);
                Font _testoHeadBold = new Font("Arial", 12, FontStyle.Bold);

                Font _testoSmall = new Font("Arial", 8, FontStyle.Regular);
                Font _testoSmallBold = new Font("Arial", 8, FontStyle.Bold);


                SolidBrush _stileBase = new SolidBrush(Color.Black);
                Pen _pennaBase = new Pen(_stileBase, 1);

                //Logo:
                Pagina.DrawImage(logo, 577, 25, 200, 80);

                //---------------------------------------------------------

                //Calcolo Gli indicatori previsti per questa pagina


                // Dati Testata
                _rigaLabel = 30;
                Pagina.DrawString(stringheStampa.titCliente, _testoSmall, _stileBase, 50, _rigaLabel);
                Pagina.DrawString(_sb.sbCliente.Client, _sottoTitoloBold, _stileBase, 50, _rigaLabel + 11);

                _rigaLabel += 40;
                Pagina.DrawString(stringheStampa.titSpybattID, _testoSmall, _stileBase, 50, _rigaLabel);
                string TestoId = _sb.sbCliente.SerialNumber;
                if (TestoId == "") TestoId = FunzioniMR.StringaSeriale(_sb.Id);
                Pagina.DrawString(TestoId, _sottoTitoloBold, _stileBase, 50, _rigaLabel + 11);

                _rigaLabel += 40;
                Pagina.DrawString(stringheStampa.titBatteria, _testoSmall, _stileBase, 50, _rigaLabel);
                Pagina.DrawString(_sb.sbCliente.BatteryId, _testoHeadBold, _stileBase, 50, _rigaLabel + 11);
                Pagina.DrawLine(_pennaBase, new Point(50, _rigaLabel + 50), new Point(777, _rigaLabel + 50));

                Pagina.DrawString(stringheStampa.titData, _testoSmall, _stileBase, 50, 1100);
                Pagina.DrawString(DateTime.Now.ToShortDateString(), _testoSmallBold, _stileBase, 150, 1100);

                if (NumeroPagina > 0)
                {
                    Pagina.DrawString(stringheStampa.txtNumPagina, _testoSmall, _stileBase, 680, 1100);
                    Pagina.DrawString(NumeroPagina.ToString(), _testoSmallBold, _stileBase, 720, 1100);
                }

            }
            catch (Exception Ex)
            {
                Log.Error("PreparaPagina: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }

        }



        private Image immagineGrafico(PlotModel Grafico,int width = 1200, int heigth = 800)
        {
            try
            {
                MemoryStream memStream = new MemoryStream();

                PngExporter _pngExporter = new PngExporter { Width = width, Height = heigth }; //, Background = OxyColors.White };
                _pngExporter.Export(Grafico, memStream);

                return Image.FromStream(memStream);

            }
            catch
            {
                return null;
            }
        }

    }
}