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



namespace PannelloCharger
{
    public partial class frmSpyBat : Form
    {
        private PrintDocument _docStampa = new PrintDocument();

        public const int DeltaIntestazione = 15;
        public const int DeltaRiga = 38;
        public const int DeltaZona = 70;


        private int _paginaCorrente = 0;
        private int _modelloStampa = 0;
        private int _pagineStampate = 0;
        private int _paginaGrafico = 0;

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


                // In base al form aperto scelgo la stampa
                int _tabCorrente = tabCaricaBatterie.SelectedIndex;
                string _paginaAttiva = tabCaricaBatterie.TabPages[_tabCorrente].Name;


                _modelloStampa = 0;

                switch (_paginaAttiva)
                {
                    case "tabCb01":
                    case "tabStatSintesi":
                    case "tabStatComparazioni":
                    case "tabCb02":
                    case "tabCb03":
                    case "tabCb04":
                    case "tabCb05":
                        {
                            _pagineStampate = 0;
                            _paginaGrafico = 0;
                            _modelloStampa = 1;
                            break;
                        }
                    case "tabStatistiche":
                        {
                            // Se ho più di 2 pagine OK, ho generato le statistiche
                            _pagineStampate = 0;
                            _paginaGrafico = 1;
                            int _tabStatCorrente = tbcStatistiche.SelectedIndex;
                            string _paginaStatAttiva = tbcStatistiche.TabPages[_tabStatCorrente].Name;

                            if (_paginaStatAttiva == "tabTemporale")
                            {
                                _modelloStampa = 3;
                            }
                            else
                            {

                                // Se voglio che stampi solo le statistiche --> =2
                                // Altrimenti --> =1 così stampa anche la testata
                                _modelloStampa = 1;
                            }
                            break;
                        }
                }

                if (_modelloStampa == 0)
                {
                    //Non sono su un tab in cui è prevista la stampa
                    return;
                }


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

                //Se richiesto il preview lo lancio se no mando in stampa direttamente
                if (_setPrinter == DialogResult.OK)
                {
                    if (preview)
                    {
                        ppdlg = new PrintPreviewDialog();
                  
                        ppdlg.Document = _docStampa;
                        ppdlg.Document.PrinterSettings = _parametri.ImpostazioniStampante;
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
                Log.Error("frmSpyBat: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
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
                switch (_modelloStampa)
                {
                    case 1:  // Stampa scheda base
                        {
                            switch (_paginaCorrente)
                            {
                                case 1:
                                    {
                                        _docStampaScheda(doc, _paginaCorrente);
                                        doc.HasMorePages = true;
                                        break;
                                    }
                                case 2:
                                    {
                                        _docStampaCruscotto(doc, _paginaCorrente);
                                        //dopo la testata lancio la setampa statistiche
                                        if (StatistichePresenti() > 0)
                                        {
                                            doc.HasMorePages = true;
                                            _modelloStampa = 2;
                                        }
                                        else
                                        {
                                            doc.HasMorePages = false;
                                            _paginaCorrente = 0;
                                            _pagineStampate = 0;
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        doc.HasMorePages = false;
                                        _paginaCorrente = 0;
                                        _pagineStampate = 0;
                                        break;
                                    }
                            }
                            break;
                        }

                    case 2:  // Stampa grafici
                        {

                            int _paginePreviste = (StatistichePresenti() + 1) / 2;

                            if ( _paginaGrafico > _paginePreviste)   ///_paginaCorrente
                            {
                                doc.HasMorePages = false;
                            }
                            else
                            {
                                _docStampaPaginaGrafici(doc, _paginaCorrente);
                                if (_paginaGrafico <= _paginePreviste)
                                {
                                    doc.HasMorePages = true;
                                }
                                else
                                {
                                    doc.HasMorePages = false;
                                    _paginaCorrente = 0;
                                    _pagineStampate = 0;
                                }

                            }

                            break;
                        }
                    case 3:  // Stampa grafico temporale
                        {


                            _docStampaPaginaAndamento(doc, _paginaCorrente);
                            doc.HasMorePages = false;
                            _paginaCorrente = 0;
                            _pagineStampate = 0;

                            break;
                        }
                }



            }
            catch (Exception Ex)
            {
                Log.Error("_docStampa_PrintPage: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }
        }


        /// <summary>
        /// Stampa la scheda intestazione e sintesi
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pagina"></param>
        public void _docStampaScheda( PrintPageEventArgs doc, int pagina)
        {
            try
            {
                Graphics graphcs = doc.Graphics;
                Image _tempImg = PannelloCharger.Properties.Resources.spyBatt;
                Bitmap logo =   new Bitmap(_tempImg);

                
                //Stampo la filigrana
                //Image _tempFiligrana = PannelloCharger.Properties.Resources.fondino;
                //graphcs.DrawImage(_tempFiligrana, 0, 0);
                // doc.PageSettings
                int _rigaLabel = 100;
                int rigaTesto = 120; 

                RectangleF printableArea = doc.PageSettings.PrintableArea;



                Font _etichette = new Font("Arial", 7);
                Font _valoriBase = new Font("Arial", 10);
                Font _valoriBold = new Font("Arial", 10,FontStyle.Bold);
                Font _titoloBold = new Font("Arial", 24, FontStyle.Bold);
                Font _sottoTitoloBold = new Font("Arial", 18, FontStyle.Bold);


                SolidBrush _pennaBase = new SolidBrush(Color.Black);

                //preparo la pagina
                _pagineStampate++;
                PreparaPagina(graphcs, _pagineStampate);
                //Logo:
                //graphcs.DrawImage(logo, 577, 25, 200, 80);



                Font myfont = new Font("Arial", 12);
                SolidBrush Mybrash = new SolidBrush(Color.Gray);
                float fontHeight = myfont.GetHeight();



                // Dati Testata
                _rigaLabel = 200;



                //Dati Cliente
               
                graphcs.DrawString(grbDatiCliente.Text, _valoriBold, _pennaBase, 50, _rigaLabel);

                _rigaLabel += DeltaRiga;
                graphcs.DrawString(lblCliCliente.Text, _etichette, _pennaBase, 100, _rigaLabel);
                graphcs.DrawString(txtCliente.Text, _valoriBase, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                _rigaLabel += DeltaRiga;
                graphcs.DrawString(lblCliMarcaBatt.Text, _etichette, _pennaBase, 100, _rigaLabel);
                graphcs.DrawString(txtMarcaBat.Text, _valoriBase, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblCliModellpApp.Text, _etichette, _pennaBase, 400, _rigaLabel);
                graphcs.DrawString(txtModelloBat.Text, _valoriBase, _pennaBase, 400, _rigaLabel + DeltaIntestazione);

                _rigaLabel += DeltaRiga;
                graphcs.DrawString(lblCliIdBatteria.Text, _etichette, _pennaBase, 100, _rigaLabel);
                graphcs.DrawString(txtIdBat.Text, _valoriBold, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblCliCicliAttesi.Text, _etichette, _pennaBase, 400, _rigaLabel);
                graphcs.DrawString(txtCliCicliAttesi.Text, _valoriBase, _pennaBase, 400, _rigaLabel + DeltaIntestazione);



                //Dati Spy BATT

                _rigaLabel += DeltaZona;
                graphcs.DrawString(grbTestata.Text, _valoriBold, _pennaBase, 50, _rigaLabel);

                _rigaLabel += DeltaRiga;
                graphcs.DrawString(lblGenID.Text, _etichette, _pennaBase, 100, _rigaLabel);
                graphcs.DrawString(txtMatrSB.Text, _valoriBold, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                _rigaLabel += DeltaRiga;
                graphcs.DrawString(lblFirmwCb.Text, _etichette, _pennaBase, 100 ,_rigaLabel);
                graphcs.DrawString(txtRevHWSb.Text, _valoriBase, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblBootLdrDisp.Text, _etichette, _pennaBase, 300,_rigaLabel);
                graphcs.DrawString(txtRevLdrSb.Text, _valoriBase, _pennaBase, 300,_rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblFirmwDisp.Text, _etichette, _pennaBase, 500,_rigaLabel);
                graphcs.DrawString(txtRevSWSb.Text, _valoriBase, _pennaBase, 500,_rigaLabel + DeltaIntestazione);


                _rigaLabel += DeltaRiga;
                graphcs.DrawString(lblGenSerNum.Text, _etichette, _pennaBase, 100, _rigaLabel);
                graphcs.DrawString(txtSerialNumber.Text, _valoriBase, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblGenFasi.Text, _etichette, _pennaBase, 300, _rigaLabel);
                graphcs.DrawString(txtMainNumLunghi.Text, _valoriBase, _pennaBase, 300, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblGenConf.Text, _etichette, _pennaBase, 500, _rigaLabel);
                graphcs.DrawString(txtMainNumProgr.Text, _valoriBase, _pennaBase, 500, _rigaLabel + DeltaIntestazione);


                // Dati Riepilogo

                // Stato Batteria
                _rigaLabel += DeltaZona;
                graphcs.DrawString(grbStatStato.Text, _valoriBold, _pennaBase, 50, _rigaLabel);


                _rigaLabel += DeltaRiga;
                graphcs.DrawString(lblStatAttivazioe.Text, _etichette, _pennaBase, 100, _rigaLabel);
                graphcs.DrawString(txtStatAttivazione.Text, _valoriBase, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblStatGiorniAtt.Text, _etichette, _pennaBase, 200, _rigaLabel);
                graphcs.DrawString(txtStatGiorniAtt.Text, _valoriBase, _pennaBase, 200, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblStatNumCicli.Text, _etichette, _pennaBase, 300, _rigaLabel);
                graphcs.DrawString(txtStatNCicli.Text, _valoriBase, _pennaBase, 300, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblStatSOH.Text, _etichette, _pennaBase, 400, _rigaLabel);
                graphcs.DrawString(txtStatSOH.Text, _valoriBase, _pennaBase, 400, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblStatSbilCelle.Text, _etichette, _pennaBase, 500, _rigaLabel);
                graphcs.DrawString(txtStatSbilCelle.Text, _valoriBase, _pennaBase, 500, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblStatMancElettr.Text, _etichette, _pennaBase, 600, _rigaLabel);
                graphcs.DrawString(txtStatMancElettr.Text, _valoriBase, _pennaBase, 600, _rigaLabel + DeltaIntestazione);



                // SCARICA
                _rigaLabel += DeltaZona;
                graphcs.DrawString(grbStatScariche.Text, _valoriBold, _pennaBase, 50, _rigaLabel);


                _rigaLabel += DeltaRiga;
                graphcs.DrawString(lblStatTempoInScarica.Text, _etichette, _pennaBase, 100, _rigaLabel);
                graphcs.DrawString(txtTempoInScarica.Text, _valoriBase, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblStatNumScariche.Text, _etichette, _pennaBase, 200, _rigaLabel);
                graphcs.DrawString(txtStatNumScariche.Text, _valoriBase, _pennaBase, 200, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblStatMediaScarica.Text, _etichette, _pennaBase, 300, _rigaLabel);
                graphcs.DrawString(txtStatDoDMedia.Text, _valoriBase, _pennaBase, 300, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblStatNumSovrascariche.Text, _etichette, _pennaBase, 400, _rigaLabel);
                graphcs.DrawString(txtStatNumSovrascariche.Text, _valoriBase, _pennaBase, 400, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label46.Text, _etichette, _pennaBase, 500, _rigaLabel);
                graphcs.DrawString(txtStatNumScaricheOverT.Text, _valoriBase, _pennaBase, 500, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblStatPauseSC.Text, _etichette, _pennaBase, 600, _rigaLabel);
                graphcs.DrawString(txtStatPauseSC.Text, _valoriBase, _pennaBase, 600, _rigaLabel + DeltaIntestazione);



                // CARICA
                _rigaLabel += DeltaZona;
                graphcs.DrawString(grbStatCarica.Text, _valoriBold, _pennaBase, 50, _rigaLabel);


                _rigaLabel += DeltaRiga;
                graphcs.DrawString(lblStatTempoInCarica.Text, _etichette, _pennaBase, 100, _rigaLabel);
                graphcs.DrawString(txtStatTempoInCarica.Text, _valoriBase, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblStatNumCariche.Text, _etichette, _pennaBase, 200, _rigaLabel);
                graphcs.DrawString(txtStatNumCariche.Text, _valoriBase, _pennaBase, 200, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label135.Text, _etichette, _pennaBase, 300, _rigaLabel);
                graphcs.DrawString(txtStatNumCaricheComp.Text, _valoriBase, _pennaBase, 300, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label48.Text, _etichette, _pennaBase, 400, _rigaLabel);
                graphcs.DrawString(txtStatNumCaricheCOverTemp.Text, _valoriBase, _pennaBase, 400, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label136.Text, _etichette, _pennaBase, 500, _rigaLabel);
                graphcs.DrawString(txtStatNumCaricheParz.Text, _valoriBase, _pennaBase, 500, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label47.Text, _etichette, _pennaBase, 600, _rigaLabel);
                graphcs.DrawString(txtStatNumCarichePOverTemp.Text, _valoriBase, _pennaBase, 600, _rigaLabel + DeltaIntestazione);




                // Energia Fornita + Anomalie
                _rigaLabel += DeltaZona;
                graphcs.DrawString(grbStatEnergia.Text, _valoriBold, _pennaBase, 50, _rigaLabel);
                graphcs.DrawString(grbStatEventiAnomali.Text, _valoriBold, _pennaBase, 550, _rigaLabel);


                _rigaLabel += DeltaRiga;
                graphcs.DrawString(lblStatTotEnergia.Text, _etichette, _pennaBase, 100, _rigaLabel);
                graphcs.DrawString(txtStatTotEnergia.Text, _valoriBase, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblStatEnergiaMedia.Text, _etichette, _pennaBase, 200, _rigaLabel);
                graphcs.DrawString(txtStatEnergiaMediaKWh.Text, _valoriBase, _pennaBase, 200, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label45.Text, _etichette, _pennaBase, 300, _rigaLabel);
                graphcs.DrawString(txtStatEnergiaMediaAh.Text, _valoriBase, _pennaBase, 300, _rigaLabel + DeltaIntestazione);



                graphcs.DrawString(label137.Text, _etichette, _pennaBase, 600, _rigaLabel);
                graphcs.DrawString(txtStatNAnomali.Text, _valoriBase, _pennaBase, 600, _rigaLabel + DeltaIntestazione);




                // Configurazione
                _rigaLabel += DeltaZona;
                graphcs.DrawString(grbProgrammazione.Text, _valoriBold, _pennaBase, 50, _rigaLabel);


                _rigaLabel += DeltaRiga;
                graphcs.DrawString(txtTensioneNom.Text, _etichette, _pennaBase, 100, _rigaLabel);
                graphcs.DrawString(txtProgcBattVdef.Text, _valoriBase, _pennaBase, 100, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label16.Text, _etichette, _pennaBase, 200, _rigaLabel);
                graphcs.DrawString(txtProgcBattAhDef.Text, _valoriBase, _pennaBase, 200, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label13.Text, _etichette, _pennaBase, 300, _rigaLabel);
                graphcs.DrawString(txtProgcCelleTot.Text, _valoriBase, _pennaBase, 300, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label14.Text, _etichette, _pennaBase, 400, _rigaLabel);
                graphcs.DrawString(txtProgcCelleV3.Text, _valoriBase, _pennaBase, 400, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(label15.Text, _etichette, _pennaBase, 500, _rigaLabel);
                graphcs.DrawString(txtProgcCelleV2.Text, _valoriBase, _pennaBase, 500, _rigaLabel + DeltaIntestazione);

                graphcs.DrawString(lblCelleP1.Text, _etichette, _pennaBase, 600, _rigaLabel);
                graphcs.DrawString(txtProgcCelleV1.Text, _valoriBase, _pennaBase, 600, _rigaLabel + DeltaIntestazione);

    




            }
            catch (Exception Ex)
            {
                Log.Error("_docStampaScheda: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }
        }

        /// <summary>
        /// Stampa la pagina indicatori cruscotto
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pagina"></param>
        public void _docStampaCruscotto(PrintPageEventArgs doc, int pagina)
        {
            try
            {
                Graphics graphcs = doc.Graphics;
                Image _tempImg = PannelloCharger.Properties.Resources.spyBatt;
                Bitmap logo = new Bitmap(_tempImg);


                //Stampo la filigrana
                //Image _tempFiligrana = PannelloCharger.Properties.Resources.fondino;
                //graphcs.DrawImage(_tempFiligrana, 0, 0);
                // doc.PageSettings
                int _rigaLabel = 100;
                int rigaTesto = 120;

                RectangleF printableArea = doc.PageSettings.PrintableArea;



                Font _etichette = new Font("Arial", 8);
                Font _valoriBase = new Font("Arial", 10);
                Font _valoriBold = new Font("Arial", 10, FontStyle.Bold);
                Font _titoloBold = new Font("Arial", 24, FontStyle.Bold);
                Font _sottoTitoloBold = new Font("Arial", 18, FontStyle.Bold);


                SolidBrush _pennaBase = new SolidBrush(Color.Black);

                Image _tempGauge;

                //Logo:
                //graphcs.DrawImage(logo, 577, 25, 200, 80);
                _pagineStampate++;
                PreparaPagina(graphcs, _pagineStampate);

                //Cruscotti
                graphcs.DrawImage(Ic11.immagine(), 150, 190, 210, 210);

                graphcs.DrawImage(Ic12.immagine(), 450, 190, 210, 210);

                graphcs.DrawImage(Ic13.immagine(), 150, 415, 210, 210);

                graphcs.DrawImage(Ic14.immagine(), 450, 415, 210, 210);

                graphcs.DrawImage(Ic21.immagine(), 150, 640, 210, 210);

                graphcs.DrawImage(Ic22.immagine(), 450, 640, 210, 210);
           
                graphcs.DrawImage(Ic23.immagine(), 150, 865, 210, 210);

                graphcs.DrawImage(Ic24.immagine(), 450, 865, 210, 210);


                // Dati Sintesi




            }
            catch (Exception Ex)
            {
                Log.Error("_docStampaScheda: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }
        }



        /// <summary>
        /// Stampa dei grafici generati
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


                // doc.PageSettings
                int _rigaLabel = 100;
                int rigaTesto = 120;


                Font _etichette = new Font("Arial", 8);
                Font _valoriBase = new Font("Arial", 10);
                Font _valoriBold = new Font("Arial", 10, FontStyle.Bold);
                Font _titoloBold = new Font("Arial", 24, FontStyle.Bold);
                Font _sottoTitoloBold = new Font("Arial", 18, FontStyle.Bold);
                Font _testoSmall = new Font("Arial", 8, FontStyle.Regular);
                Font _testoSmallBold = new Font("Arial", 8, FontStyle.Bold);


                SolidBrush _stileBase = new SolidBrush(Color.Black);
                _pagineStampate++;


                PreparaPagina(graphcs, _pagineStampate);

                Image _tmpGraph;


                // Parte alta
                _tmpGraph = GraficoInstampa(_paginaGrafico, 0);
                if (_tmpGraph != null)
                {
                    graphcs.DrawImage(_tmpGraph, 120, 190, 600, 400);
                }


                //Parte Bassa
                _tmpGraph = GraficoInstampa(_paginaGrafico, 1);
                if (_tmpGraph != null)
                {
                    graphcs.DrawImage(_tmpGraph, 120, 640, 600, 400);

                }

                graphcs.DrawString(stringheStampa.txtPeriodoAttivo, _testoSmall, _stileBase, 50, 1080);
                // Verifico se ho attivato il periodo temporale
                if (optStatPeriodoSel.Checked)
                {
                    string StringaPeriodo = stringheStampa.txtPeriodoDal + " ";
                    StringaPeriodo += dtpStatInizio.Value.ToString("MMM yyyy");
                    StringaPeriodo += " " + stringheStampa.txtPeriodoA + " ";
                    StringaPeriodo += dtpStatFine.Value.ToString("MMM yyyy");
                    graphcs.DrawString(StringaPeriodo, _testoSmallBold, _stileBase, 180, 1080);
                }
                else
                {
                    graphcs.DrawString(optStatInteroIntervallo.Text, _testoSmallBold, _stileBase, 180, 1080);
                }
                _paginaGrafico ++;

            }
            catch (Exception Ex)
            {
                Log.Error("_docStampaScheda: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }
        }


        /// <summary>
        /// Stampa la singola pagina corrente dell'andamento temporale
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pagina"></param>
        public void _docStampaPaginaAndamento(PrintPageEventArgs doc, int pagina)
        {
            try
            {
                Graphics graphcs = doc.Graphics;
                Image _tempImg = PannelloCharger.Properties.Resources.spyBatt;
                Bitmap logo = new Bitmap(_tempImg);

                PreparaPagina(graphcs, pagina);

                int _rigaLabel = 100;
                int rigaTesto = 120;
                _pagineStampate++;
                


                Font _etichette = new Font("Arial", 8);
                Font _valoriBase = new Font("Arial", 10);
                Font _valoriBold = new Font("Arial", 10, FontStyle.Bold);
                Font _titoloBold = new Font("Arial", 24, FontStyle.Bold);
                Font _sottoTitoloBold = new Font("Arial", 18, FontStyle.Bold);


                SolidBrush _pennaBase = new SolidBrush(Color.Black);
                String _titoloPagina = stringheStampa.titGraficoTemporale + "  ";
                oxyTabPage _tempOxy = (oxyTabPage)TbcStatSettimane.SelectedTab;
                _titoloPagina += _tempOxy.Text;

                graphcs.DrawString(_titoloPagina, _sottoTitoloBold, _pennaBase, 50, 190);

                //Calcolo Gli indicatori previsti per questa pagina


                Image _tmpGraph;


                // Parte alta
                _tmpGraph = GraficoTempInstampa(pagina,RotateFlipType.Rotate270FlipNone);


                if (_tmpGraph != null)
                {
                    graphcs.DrawImage(_tmpGraph, 150, 250, 500, 800);
                }



            }
            catch (Exception Ex)
            {
                Log.Error("_docStampaScheda: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
            }
        }


        /// <summary>
        /// Determino il numero dei grafici stampabili
        /// </summary>
        /// <returns></returns>
        private int StatistichePresenti()
        {
            int _numStatistiche = 0;
            // se ho meno di 3 pagine nel tab statistiche non ci sono statistiche pronte
            int _nonStampabili = 2;

            if (tbcStatistiche.TabCount < 3)
                return 0;

            _numStatistiche = tbcStatistiche.TabCount;
            //Se ho generato solo il diagramma temporale, non ho nulla da stampare
            if (chkStatGraficoTemporale.Checked)
                _nonStampabili += 1;

            _numStatistiche = tbcStatistiche.TabCount - _nonStampabili;

            if (_numStatistiche < 0)
                _numStatistiche = 0;

            return _numStatistiche;
        }

        /// <summary>
        /// Ritorna il bitmat dell'oggetto OxyPlot (grafico) da stampare nella posizione indicata
        /// </summary>
        /// <param name="Pagina"></param>
        /// <param name="Posizione">0 = riquadro superiore, 1 = Riquadro inferiore</param>
        /// <returns></returns>
        private Image GraficoInstampa( int Pagina, int Posizione)
        {
            try
            {
                oxyTabPage _grafico;
                int _paginePreviste = (StatistichePresenti() + 1) / 2;
                int _tabRichiesto;

                if (_paginaGrafico > _paginePreviste)
                {
                    return null;
                }
                else
                {
                    if (Posizione > 1) Posizione = 1;
                    if (Posizione < 1) Posizione = 0;

                    // Calcolo la pagina richiesta
                    _tabRichiesto = 1 + (Pagina - 1) * 2 + Posizione ;

                    //Aggiungo 1 se ci sono i grafici temporali
                    if (chkStatGraficoTemporale.Checked) _tabRichiesto += 1;

                    if(_tabRichiesto <( tbcStatistiche.TabCount -1))
                    {
                        _grafico = (oxyTabPage)tbcStatistiche.TabPages[_tabRichiesto];
                        return _grafico.immmagine();
                    }
                    else
                        return null;

                }

                return null;
            }
            catch (Exception Ex)
            {
                Log.Error("GraficoInstampa: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
                return null;
            }
        }


        /// <summary>
        /// Ritorna il bitmat dell'oggetto OxyPlot (grafico) da stampare nella posizione indicata
        /// </summary>
        /// <param name="Pagina"></param>
        /// <param name="Posizione">0 = riquadro superiore, 1 = Riquadro inferiore</param>
        /// <returns></returns>
        private Image GraficoTempInstampa(int Pagina, RotateFlipType rotateFlipType)
        {
            try
            {
                oxyTabPage _grafico;
                Image _tempImg;

                // Verifico che il grafico temporale esista, non sia vuoto e ci sia una pagina selezionata
                if (TbcStatSettimane == null)
                    return null;

                if (TbcStatSettimane.TabCount < 1)
                    return null;

                if (TbcStatSettimane.SelectedTab == null)
                    return null;




                _grafico = (oxyTabPage)TbcStatSettimane.SelectedTab;

                _tempImg = _grafico.immmagine();
                _tempImg.RotateFlip(rotateFlipType);

                return _tempImg;


            }
            catch (Exception Ex)
            {
                Log.Error("GraficoInstampa: " + Ex.Message + " [" + Ex.TargetSite.ToString() + "]");
                return null;
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
                Pagina.DrawString(txtCliente.Text, _sottoTitoloBold, _stileBase, 50, _rigaLabel + 11);

                _rigaLabel += 40;
                Pagina.DrawString(stringheStampa.titSpybattID, _testoSmall, _stileBase, 50, _rigaLabel);
                string TestoId = txtSerialNumber.Text;
                if (TestoId == "") TestoId = txtMatrSB.Text;
                Pagina.DrawString(TestoId, _sottoTitoloBold, _stileBase, 50, _rigaLabel + 11);

                _rigaLabel += 40;
                Pagina.DrawString(stringheStampa.titBatteria, _testoSmall, _stileBase, 50, _rigaLabel);
                Pagina.DrawString(txtIdBat.Text, _testoHeadBold, _stileBase, 50, _rigaLabel + 11);
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


    }
}
