using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BrightIdeasSoftware;

using ChargerLogic;
using log4net;
using log4net.Config;

using MoriData;
using Utility;
using System.Globalization;

namespace PannelloCharger
{
    public partial class frmCaricabatterieV2 : Form
    {


        private void InizializzaListaCariche()
        {
            try
            {
                // strSortIdMemCiclo

                HeaderFormatStyle _stile = new HeaderFormatStyle();
                _stile.SetBackColor(Color.DarkGray);
                _stile.SetForeColor(Color.Yellow);
                Font _carattere = new Font("Tahoma", 9, FontStyle.Bold);
                _stile.SetFont(_carattere);
                Font _colonnaBold = new Font("Tahoma", 8, FontStyle.Bold);

                flvCicliListaCariche.HeaderUsesThemes = false;
                flvCicliListaCariche.HeaderFormatStyle = _stile;
                flvCicliListaCariche.UseAlternatingBackColors = true;
                flvCicliListaCariche.AlternateRowBackColor = Color.LightGoldenrodYellow;

                flvCicliListaCariche.AllColumns.Clear();
                flvCicliListaCariche.RowHeight = 18;

                flvCicliListaCariche.View = View.Details;
                flvCicliListaCariche.ShowGroups = false;
                flvCicliListaCariche.GridLines = true;
                flvCicliListaCariche.FullRowSelect = true;
                flvCicliListaCariche.MultiSelect = false;


                BrightIdeasSoftware.OLVColumn sortColPosizioneCiclo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Pos",
                    AspectName = "strSortIdMemCiclo",
                    Width = 0,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,


                };

                BrightIdeasSoftware.OLVColumn sortColIdMemCiclo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "ID",
                    AspectName = "strSortIdMemCiclo",
                    Width = 0,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                    
                    
                };

                flvCicliListaCariche.AllColumns.Add(sortColIdMemCiclo);

                BrightIdeasSoftware.OLVColumn colIdMemCiclo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "ID",
                    AspectName = "strIdMemCiclo",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colIdMemCiclo);

                BrightIdeasSoftware.OLVColumn colDataOraStart = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Start",
                    AspectName = "DataOraStart",
                    Width = 100,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colDataOraStart);


                BrightIdeasSoftware.OLVColumn colDataOraStop = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Stop",
                    AspectName = "DataOraFine",
                    Width = 100,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colDataOraStop);


                BrightIdeasSoftware.OLVColumn colDurata = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Durata",
                    AspectName = "strDurata",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colDurata);

                BrightIdeasSoftware.OLVColumn colstrV5Min = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "V 5m",
                    AspectName = "strVbat5m",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrV5Min);

                BrightIdeasSoftware.OLVColumn colstrI5Min = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "I 5m",
                    AspectName = "strIbat5m",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrI5Min);

                BrightIdeasSoftware.OLVColumn colstrVFin = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "V Fin",
                    AspectName = "strVbatFinale",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrVFin);

                BrightIdeasSoftware.OLVColumn colstrIFin = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "I Fin",
                    AspectName = "strIbatFinale",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrIFin);

                BrightIdeasSoftware.OLVColumn colstrAh = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Ah",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strAh",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrAh);

                BrightIdeasSoftware.OLVColumn colstrKWh = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "KWh",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strKWh",
                    Width = 50,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrKWh);

                BrightIdeasSoftware.OLVColumn colstrCondizioneStop = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Stop",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strCondizioneStop",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Center,
                };
                flvCicliListaCariche.AllColumns.Add(colstrCondizioneStop);
                
                BrightIdeasSoftware.OLVColumn colstrChargerStop = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Causale Stop",
                    ToolTipText = "Motivo fermata carica",
                    AspectName = "strChargerStop",
                    Width = 150,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colstrChargerStop);

                BrightIdeasSoftware.OLVColumn colstrChargerOpt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Opzioni",
                    ToolTipText = "Opzioni carica",
                    AspectName = "strOpzioniCarica",
                    Width = 100,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colstrChargerOpt);

                BrightIdeasSoftware.OLVColumn colstrVettOptOC = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Opportunity",
                    IsHeaderVertical = true,
                    ToolTipText = "Opportunity Charge",
                    AspectName = "strOptOpportunity",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.coniglio16, "NO", Properties.Resources.GRAY_16 })

                };
                flvCicliListaCariche.AllColumns.Add(colstrVettOptOC);

                BrightIdeasSoftware.OLVColumn colstrVettOptSB = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "SPY-BATT",
                    IsHeaderVertical = true,
                    ToolTipText = "Profilo SPY-BATT",
                    AspectName = "strOptSpyBatt",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.OK_16, "NO", Properties.Resources.GRAY_16 })

                };
                flvCicliListaCariche.AllColumns.Add(colstrVettOptSB);

                BrightIdeasSoftware.OLVColumn colstrVettOptIDB = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "ID-BATT",
                    IsHeaderVertical = true,
                    ToolTipText = "Profilo ID-BATT",
                    AspectName = "strOptIdBatt",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.OK_16, "NO", Properties.Resources.GRAY_16 })

                };
                flvCicliListaCariche.AllColumns.Add(colstrVettOptIDB);


                BrightIdeasSoftware.OLVColumn colEqualReq = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Eq R",
                    ToolTipText = "Equalizzazione Richiesta",
                    AspectName = "strEqualRequest",
                    Width = 45,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "OK", Properties.Resources.OK_16, "KO", Properties.Resources.KO_16, "OFF", Properties.Resources.GRAY_16 })

                };
                flvCicliListaCariche.AllColumns.Add(colEqualReq);

                BrightIdeasSoftware.OLVColumn colEqualEff = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Eq E",
                    ToolTipText = "Equalizzazione Effettuata",
                    AspectName = "strEqualEffettuato",
                    Width = 45,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "OK", Properties.Resources.OK_16, "KO", Properties.Resources.KO_16, "OFF", Properties.Resources.GRAY_16})

                };
                flvCicliListaCariche.AllColumns.Add(colEqualEff);

                BrightIdeasSoftware.OLVColumn colstrChargerErr = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Errori",
                    ToolTipText = "Vettore Errori",
                    AspectName = "strVettoreErrori",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Center,
                };
                flvCicliListaCariche.AllColumns.Add(colstrChargerErr);


                BrightIdeasSoftware.OLVColumn colstrVettErrCalibr = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Cal.Corr.",
                    IsHeaderVertical = true,
                    ToolTipText = "Errore Calibrazione corrente zero all'accensione",
                    AspectName = "strErrCalib",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })

                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrCalibr);

                BrightIdeasSoftware.OLVColumn colstrVettErrComm = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Comm.",
                    IsHeaderVertical = true,
                    ToolTipText = "Errore Comunicazione con una periferica",
                    AspectName = "strErrComm",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrComm);

                BrightIdeasSoftware.OLVColumn colstrVettErrVBatt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "V Batt.",
                    IsHeaderVertical = true,
                    ToolTipText = "Tensione batteria non corretta alla partenza",
                    AspectName = "strErrVbatt",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrVBatt);


                BrightIdeasSoftware.OLVColumn colstrVettErrInternal = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Internal",
                    IsHeaderVertical = true,
                    ToolTipText = "Anomalia interna dell'apparecchiatura",
                    AspectName = "strErrInt",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrInternal);

                BrightIdeasSoftware.OLVColumn colstrVettErrSpyBatt = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Batt.Err.",
                    IsHeaderVertical = true,
                    ToolTipText = "Anomalia da Spy-batt 01",
                    AspectName = "strErrSB1",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrSpyBatt);

                BrightIdeasSoftware.OLVColumn colstrVettErrFuse = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Fuse",
                    IsHeaderVertical = true,
                    ToolTipText = "Fusibile di uscita Aperto",
                    AspectName = "strErrFuse",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrFuse);

                BrightIdeasSoftware.OLVColumn colstrVettErrAlim = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Alim.",
                    IsHeaderVertical = true,
                    ToolTipText = "Mancanza rete o problema alimentazione",
                    AspectName = "strErrAlim",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrAlim);

                BrightIdeasSoftware.OLVColumn colstrVettErrIbat = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "I Batt.",
                    IsHeaderVertical = true,
                    ToolTipText = "Corrente di Batteria",
                    AspectName = "strErrIbat",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrIbat);


                BrightIdeasSoftware.OLVColumn colstrVettErrStrappo = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Strappo",
                    IsHeaderVertical = true,
                    ToolTipText = "Batteria staccata mentre era in carica",
                    AspectName = "strErrStrappo",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrStrappo);


                BrightIdeasSoftware.OLVColumn colstrVettErrParam = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Param Err.",
                    IsHeaderVertical = true,
                    ToolTipText = "Parametri residenti in memoria non correttia",
                    AspectName = "strErrParam",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrParam);


                BrightIdeasSoftware.OLVColumn colstrVettErrParamSB = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Param SB",
                    IsHeaderVertical = true,
                    ToolTipText = "Parametri SpyBatt non adatti al caricabatteria",
                    AspectName = "strErrParamSB",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrParamSB);


                BrightIdeasSoftware.OLVColumn colstrVettErrExtMem = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Memory",
                    IsHeaderVertical = true,
                    ToolTipText = "Memoria esterna non funzionante",
                    AspectName = "strErrMemExt",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrExtMem);

                BrightIdeasSoftware.OLVColumn colstrVettErrInit = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Init.",
                    IsHeaderVertical = true,
                    ToolTipText = "Memoria NON INIZIALIZZATA",
                    AspectName = "strErrNoInit",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrInit);

                BrightIdeasSoftware.OLVColumn colstrVettErrMaxSD = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Max SD",
                    IsHeaderVertical = true,
                    ToolTipText = "Numero di Shut Down superiore al valore massimo consentito",
                    AspectName = "strErrMaxSD",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrMaxSD);

                BrightIdeasSoftware.OLVColumn colstrVettErrIPK = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Max IPK",
                    IsHeaderVertical = true,
                    ToolTipText = "E’ stato rilevato un Interrupt su PIN I_PK ",
                    AspectName = "strErrMaxIPK",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrIPK);

                BrightIdeasSoftware.OLVColumn colstrVettErrPWHole = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Pw Hole",
                    IsHeaderVertical = true,
                    ToolTipText = "Buco di rete (tempo inferiore a 1 secondo)",
                    AspectName = "strErrPwHole",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrPWHole);

                BrightIdeasSoftware.OLVColumn colstrVettErrPWKO = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Ass.Rete",
                    IsHeaderVertical = true,
                    ToolTipText = "Assenza di rete",
                    AspectName = "strErrPwOff",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrPWKO);

                BrightIdeasSoftware.OLVColumn colstrVettErrPreCT = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "T.Fase 0",
                    IsHeaderVertical = true,
                    ToolTipText = "Timer di Pre Ciclo scaduto e tensione non adeguata",
                    AspectName = "strErrTmr0",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrPreCT);

                BrightIdeasSoftware.OLVColumn colstrVettErrTmrF1 = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "T.Fase 1",
                    IsHeaderVertical = true,
                    ToolTipText = "Timer di Fase 1 scaduto e tensione non adeguata",
                    AspectName = "strErrTmr1",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrTmrF1);

                BrightIdeasSoftware.OLVColumn colstrVettErrDispPulse = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Disp.Puls.",
                    IsHeaderVertical = true,
                    ToolTipText = "Pulsanti display in anomalia",
                    AspectName = "strErrDispPulse",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })
                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrDispPulse);

                BrightIdeasSoftware.OLVColumn colstrVettErrPFC = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "PFC",
                    IsHeaderVertical = true,
                    ToolTipText = "Tensione PFC anomala",
                    AspectName = "strErrPFC",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })

                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrPFC);

                BrightIdeasSoftware.OLVColumn colstrVettErrSEC = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Security",
                    IsHeaderVertical = true,
                    ToolTipText = "Contatto di sicurezza",
                    AspectName = "strErrSecurity",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })

                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrSEC);

                BrightIdeasSoftware.OLVColumn colstrVettErrThermal = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Termica",
                    IsHeaderVertical = true,
                    ToolTipText = "Anomalia Termica",
                    AspectName = "strErrThermal",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })

                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrThermal);


                BrightIdeasSoftware.OLVColumn colstrVettErrUndef = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "N.D.",
                    IsHeaderVertical = true,
                    ToolTipText = "Errore non definito",
                    AspectName = "strErrUndef",
                    Width = 20,
                    Sortable = false,
                    HeaderTextAlign = HorizontalAlignment.Center,
                    TextAlign = HorizontalAlignment.Center,
                    Renderer = new MappedImageRenderer(new Object[] { "SI", Properties.Resources.KO_16, "NO", Properties.Resources.GRAY_16 })

                };
                flvCicliListaCariche.AllColumns.Add(colstrVettErrUndef);











                BrightIdeasSoftware.OLVColumn colstrNumEventiBrevi = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Brevi",
                    //ToolTipText = "Dati Modificabili",
                    AspectName = "strNumEventiBrevi",
                    Width = 80,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Left,
                };
                flvCicliListaCariche.AllColumns.Add(colstrNumEventiBrevi);

                BrightIdeasSoftware.OLVColumn colstrPtrBrevi = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "Ptr Primo",
                    AspectName = "strPuntatorePrimoBreve",
                    Width = 80,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                };
                flvCicliListaCariche.AllColumns.Add(colstrPtrBrevi);

                BrightIdeasSoftware.OLVColumn colRowFiller = new BrightIdeasSoftware.OLVColumn()
                {
                    Text = "",
                    AspectName = "",
                    Width = 60,
                    HeaderTextAlign = HorizontalAlignment.Left,
                    TextAlign = HorizontalAlignment.Right,
                    FillsFreeSpace = true,
                };
                flvCicliListaCariche.AllColumns.Add(colRowFiller);

                flvCicliListaCariche.RebuildColumns();
                flvCicliListaCariche.SetObjects(_cb.MemoriaCicli);
                flvCicliListaCariche.Sort(sortColPosizioneCiclo, SortOrder.Descending);
                flvCicliListaCariche.BuildList();
            }
            catch (Exception Ex)
            {
                Log.Error("InizializzaVistaLunghi: " + Ex.Message);
            }
        }

        private bool VuotaListaCariche()
        {
            try
            {
                _cb.MemoriaCicli = new List<llMemoriaCicli>();
                InizializzaListaCariche();

                return true;
            }
            catch (Exception Ex)
            {
                Log.Error("VuotaListaCariche: " + Ex.Message);
                return false;
            }
        }

        public bool CaricaListaCariche( UInt32 StartAddr, ushort NumRows = 0, bool TaskExt = false)
        {
            object EsitoCaricamento;
            bool _esito;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!_cb.ContatoriLL.valido)
                {
                   if(! _cb.CaricaAreaContatori())
                    {
                        this.Cursor = Cursors.Arrow;
                        return false;
                    }
                }

                if (NumRows == 0)
                {
                    NumRows = (ushort)_cb.ContatoriLL.CntCariche;
                }




                if (NumRows > 10)
                {
                    Log.Debug("Lancio lettura lunghi");
                   
                    _avCicli.ParametriWorker.MainCount = 100;
                    _avCicli.llLocale = _cb;
                    _avCicli.ValStart = (int)0;
                    _avCicli.AddrStart = StartAddr;
                    _avCicli.ValFine = (int)NumRows;// _sb.sbData.LongMem;
                    _avCicli.DbDati = _logiche.dbDati.connessione;
                    _avCicli.CaricaBrevi = false; // chkCaricaBrevi.Checked;
                    _avCicli.ElementoPilotato = frmAvanzamentoCicli.ControlledDevice.LadeLight;
                    _avCicli.TipoComando = elementiComuni.tipoMessaggio.MemLungaLL;
                    Log.Debug("FRM RicaricaCicli: ");

                    //_esito = _sb.RicaricaCaricaCicliMemLunga(Inizio, (uint)_sb.sbData.LongMem, _logiche.dbDati.connessione, true, CaricaBrevi);

                    // Apro il form con le progressbar
                    _avCicli.ShowDialog(this);

                    // _esito = _cb.CaricaListaCicli(StartAddr, NumRows, out EsitoCaricamento, false, true);
                    _esito = true;

                }
                else
                {
                    _esito = _cb.CaricaListaCicli(StartAddr, NumRows, out EsitoCaricamento, false, false);
                }

                InizializzaListaCariche();
                this.Cursor = Cursors.Arrow;
                return _esito;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaListaCariche: " + Ex.Message);
                return false;
            }
        }

        public bool MostraCicloCorrente()
        {
            try
            {
                txtPaNomeSetup.Text = "";
                txtPaCapacita.Text = "";

                //cmbPaProfilo.SelectedIndex = 0;
                txtPaCapacita.Text = "";
                txtPaTempoT2Max.Text = "";
                txtPaSogliaVs.Text = "";
                txtPaCorrenteI1.Text = "";
                txtPaTensione.Text = "";

                //cmbPaCondStop.SelectedIndex = 0;
                txtPaCoeffK.Text = "";
                txtPaTempoT2Min.Text = "";
                txtPaTempoT2Max.Text = "";
                chkPaUsaSpyBatt.Checked = false;
                if (_cb.Programmazioni.ProgrammaAttivo != null)
                {
                    txtPaNomeSetup.Text = _cb.Programmazioni.ProgrammaAttivo.ProgramName;
                    txtPaCapacita.Text = FunzioniMR.StringaCapacita(_cb.Programmazioni.ProgrammaAttivo.BatteryAhdef,10) ;
                    List<sbTipoBatteria> Lista = (List<sbTipoBatteria>)(cmbPaTipoBatteria.DataSource);
                    cmbPaTipoBatteria.SelectedItem = Lista.Find(x => x.BatteryTypeId == _cb.Programmazioni.ProgrammaAttivo.BatteryType);
                    List<_llProfiloCarica> ListaP = (List<_llProfiloCarica>)(cmbPaProfilo.DataSource);
                    cmbPaProfilo.SelectedItem = ListaP.Find(x => x.IdProfiloCaricaLL == _cb.Programmazioni.ProgrammaAttivo.IdProfilo);
                    List<llTensioneBatteria> ListaV = (List<llTensioneBatteria>)(cmbPaTensione.DataSource);
                    cmbPaTensione.SelectedItem = ListaV.Find(x => x.IdTensione == _cb.Programmazioni.ProgrammaAttivo.BatteryVdef);
                    txtPaTensione.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.BatteryVdef);
                    List<llDurataCarica> ListaD = (List<llDurataCarica>)(cmbPaDurataCarica.DataSource);

                    cmbPaDurataCarica.SelectedItem = ListaD.Find(x => x.IdDurataCaricaLL == _cb.Programmazioni.ProgrammaAttivo.DurataMaxCarica);
                    txtPaTempoT2Min.Text = _cb.Programmazioni.ProgrammaAttivo.DurataMinFase2.ToString();
                    txtPaTempoT2Max.Text = _cb.Programmazioni.ProgrammaAttivo.DurataMaxFase2.ToString();
                    txtPaCoeffK.Text = _cb.Programmazioni.ProgrammaAttivo.PercTempoFase2.ToString();
                    txtPaTempoT3Max.Text = _cb.Programmazioni.ProgrammaAttivo.DurataMaxFase3.ToString();

                    txtPaSogliaVs.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VSoglia);
                    txtPaRaccordoF1.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VRaccordoF1);
                    txtPaVMax.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VMax);
                    txtPaVLimite.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VCellLimite);
                    txtPaVMinRic.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VMinRec);
                    txtPaVMaxRic.Text = FunzioniMR.StringaTensione(_cb.Programmazioni.ProgrammaAttivo.VMaxRec);

                    txtPaNumCelle.Text = _cb.Programmazioni.ProgrammaAttivo.NumeroCelle.ToString();
                    txtPaCorrenteF3.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.CorrenteFase3);


                    txtPaCorrenteI1.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.CorrenteMax);

                    MostraEqualCCorrente();
                    /*
                    if (_cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi >0 || _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa>0)
                    {
                        chkPaAttivaEqual.Checked = true;
                        txtPaEqualNumPulse.Text = _cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi.ToString();
                        txtPaEqualAttesa.Text = _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa.ToString();
                        txtPaEqualPulsePause.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataPausa.ToString();
                        txtPaEqualPulseTime.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataImpulso.ToString();
                        txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.EqualCorrenteImpulso );

                    }
                    else
                    {
                        chkPaAttivaEqual.Checked = false;
                        txtPaEqualNumPulse.Text = "";
                        txtPaEqualAttesa.Text = "";
                        txtPaEqualPulsePause.Text = "";
                        txtPaEqualPulseTime.Text = "";
                        txtPaEqualPulseCurrent.Text = "";

                    }
                    */

                    chkPaUsaSpyBatt.Checked = (_cb.Programmazioni.ProgrammaAttivo.AbilitaComunicazioneSpybatt == 0);


                    if (_cb.Programmazioni.ProgrammaAttivo.TempoAttesaBMS> 0 || _cb.Programmazioni.ProgrammaAttivo.TempoErogazioneBMS > 0)
                    {
                        chkPaAttivaRiarmoBms.Checked = true;
                        txtPaBMSTempoAttesa.Text = _cb.Programmazioni.ProgrammaAttivo.TempoAttesaBMS.ToString();
                        txtPaBMSTempoErogazione.Text = _cb.Programmazioni.ProgrammaAttivo.TempoErogazioneBMS.ToString();
                    }
                    else
                    {
                        chkPaAttivaRiarmoBms.Checked = false;
                        txtPaBMSTempoAttesa.Text = "";
                        txtPaBMSTempoErogazione.Text = "";
                    }


                }




                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool MostraEqualCCorrente()
        {
            try
            {
                
                txtPaEqualNumPulse.Text = "";
                txtPaEqualAttesa.Text = "";
                txtPaEqualPulsePause.Text = "";
                txtPaEqualPulseTime.Text = "";
                txtPaEqualPulseCurrent.Text = "";

                if (_cb.Programmazioni.ProgrammaAttivo != null)
                {
                    if (_cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi > 0 || _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa > 0)
                    {
                        chkPaAttivaEqual.Checked = true;
                        txtPaEqualNumPulse.Text = _cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi.ToString();
                        txtPaEqualAttesa.Text = _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa.ToString();
                        txtPaEqualPulsePause.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataPausa.ToString();
                        txtPaEqualPulseTime.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataImpulso.ToString();
                        txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.EqualCorrenteImpulso);

                    }
                    else
                    {
                        chkPaAttivaEqual.Checked = false;
                        txtPaEqualNumPulse.Text = "";
                        txtPaEqualAttesa.Text = "";
                        txtPaEqualPulsePause.Text = "";
                        txtPaEqualPulseTime.Text = "";
                        txtPaEqualPulseCurrent.Text = "";

                        txtPaEqualNumPulse.Enabled = false;
                        txtPaEqualAttesa.Enabled = false;
                        txtPaEqualPulsePause.Enabled = false;
                        txtPaEqualPulseTime.Enabled = false;
                        txtPaEqualPulseCurrent.Enabled = false;

                    }

                }

                return true;
            }
            catch
            {
                return false;
            }

        }


        public bool MostraMantCCorrente()
        {
            try
            {

                txtPaEqualNumPulse.Text = "";
                txtPaEqualAttesa.Text = "";
                txtPaEqualPulsePause.Text = "";
                txtPaEqualPulseTime.Text = "";
                txtPaEqualPulseCurrent.Text = "";

                if (_cb.Programmazioni.ProgrammaAttivo != null)
                {
                    if (_cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi > 0 || _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa > 0)
                    {
                        chkPaAttivaEqual.Checked = true;
                        txtPaEqualNumPulse.Text = _cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi.ToString();
                        txtPaEqualAttesa.Text = _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa.ToString();
                        txtPaEqualPulsePause.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataPausa.ToString();
                        txtPaEqualPulseTime.Text = _cb.Programmazioni.ProgrammaAttivo.EqualDurataImpulso.ToString();
                        txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)_cb.Programmazioni.ProgrammaAttivo.EqualCorrenteImpulso);

                        /*
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantAttesa, ModCicloCorrente.ValoriCiclo.MantTempoAttesa, ModCicloCorrente.ParametriAttivi.MantTempoAttesa, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmin, ModCicloCorrente.ValoriCiclo.MantTensIniziale, ModCicloCorrente.ParametriAttivi.MantTensIniziale, 1, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmax, ModCicloCorrente.ValoriCiclo.MantTensFinale, ModCicloCorrente.ParametriAttivi.MantTensFinale, 1, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantDurataMax, ModCicloCorrente.ValoriCiclo.MantTempoMaxErogazione, ModCicloCorrente.ParametriAttivi.MantTempoMaxErogazione, 3, SbloccaValori);
                        FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantCorrente, ModCicloCorrente.ValoriCiclo.MantCorrenteImpulso, ModCicloCorrente.ParametriAttivi.MantCorrenteImpulso, 2, SbloccaValori);

                        */

                    }
                    else
                    {
                        chkPaAttivaEqual.Checked = false;
                        txtPaMantAttesa.Text = "";
                        txtPaMantVmin.Text = "";
                        txtPaMantVmax.Text = "";
                        txtPaMantDurataMax.Text = "";
                        txtPaMantCorrente.Text = "";

                        txtPaMantAttesa.Enabled = false;
                        txtPaMantVmin.Enabled = false;
                        txtPaMantVmax.Enabled = false;
                        txtPaMantDurataMax.Enabled = false;
                        txtPaMantCorrente.Enabled = false;

                    }

                }

                return true;
            }
            catch
            {
                return false;
            }

        }


        public bool AssegnaEqualCCorrente()
        {
            try
            {

                // if (ModCicloCorrente.ValoriCiclo.EqualAttivo == 0xF0F0)

                txtPaEqualNumPulse.Text = ModCicloCorrente.ValoriCiclo.EqualNumImpulsi.ToString();
                _cb.Programmazioni.ProgrammaAttivo.EqualNumImpulsi = ModCicloCorrente.ValoriCiclo.EqualNumImpulsi;

                txtPaEqualAttesa.Text = ModCicloCorrente.ValoriCiclo.EqualTempoAttesa.ToString();
                _cb.Programmazioni.ProgrammaAttivo.EqualTempoAttesa = ModCicloCorrente.ValoriCiclo.EqualTempoAttesa;

                txtPaEqualPulsePause.Text = ModCicloCorrente.ValoriCiclo.EqualTempoPausa.ToString();
                _cb.Programmazioni.ProgrammaAttivo.EqualDurataPausa = ModCicloCorrente.ValoriCiclo.EqualTempoPausa;

                txtPaEqualPulseTime.Text = ModCicloCorrente.ValoriCiclo.EqualTempoImpulso.ToString();
                _cb.Programmazioni.ProgrammaAttivo.EqualDurataImpulso = ModCicloCorrente.ValoriCiclo.EqualTempoImpulso;

                txtPaEqualPulseCurrent.Text = FunzioniMR.StringaCorrente((short)ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso);
                _cb.Programmazioni.ProgrammaAttivo.EqualCorrenteImpulso = ModCicloCorrente.ValoriCiclo.EqualCorrenteImpulso;


                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool AssegnaMantCCorrente()
        {
            try
            {


                bool SbloccaValori = chkPaSbloccaValori.Checked;

                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantAttesa, ModCicloCorrente.ValoriCiclo.MantTempoAttesa, ModCicloCorrente.ParametriAttivi.MantTempoAttesa, 3, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmin, ModCicloCorrente.ValoriCiclo.MantTensIniziale, ModCicloCorrente.ParametriAttivi.MantTensIniziale, 1, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantVmax, ModCicloCorrente.ValoriCiclo.MantTensFinale, ModCicloCorrente.ParametriAttivi.MantTensFinale, 1, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantDurataMax, ModCicloCorrente.ValoriCiclo.MantTempoMaxErogazione, ModCicloCorrente.ParametriAttivi.MantTempoMaxErogazione, 3, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaMantCorrente, ModCicloCorrente.ValoriCiclo.MantCorrenteImpulso, ModCicloCorrente.ParametriAttivi.MantCorrenteImpulso, 2, SbloccaValori);
 

                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool AssegnaOppCCorrente()
        {
            try
            {


                bool SbloccaValori = chkPaSbloccaValori.Checked;

                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraInizio, ModCicloCorrente.ValoriCiclo.OpportunityOraInizio, ModCicloCorrente.ParametriAttivi.OpportunityOraInizio, 4, false);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppOraFine, ModCicloCorrente.ValoriCiclo.OpportunityOraFine, ModCicloCorrente.ParametriAttivi.OpportunityOraFine, 4, false);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppVSoglia, ModCicloCorrente.ValoriCiclo.OpportunityTensioneMax, ModCicloCorrente.ParametriAttivi.OpportunityTensioneMax, 1, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppCorrente, ModCicloCorrente.ValoriCiclo.OpportunityCorrente, ModCicloCorrente.ParametriAttivi.OpportunityCorrente, 2, SbloccaValori);
                FunzioniUI.ImpostaTextBoxUshort(ref txtPaOppDurataMax, ModCicloCorrente.ValoriCiclo.OpportunityDurataMax, ModCicloCorrente.ParametriAttivi.OpportunityDurataMax, 3, SbloccaValori);

                OppNotturno((ModCicloCorrente.ValoriCiclo.OpportunityOraInizio >= ModCicloCorrente.ValoriCiclo.OpportunityOraFine));

                return true;
            }
            catch
            {
                return false;
            }

        }




        public List<llMemBreve> CaricaListaBrevi(UInt32 StartAddr, ushort NumRows = 0, uint IdCiclo = 0)
        {

            bool _esito;
            List<llMemBreve> ListaBrevi;
            try
            {
                ListaBrevi = new List<llMemBreve>();

                ListaBrevi = _cb.CaricaListaBrevi(StartAddr, NumRows,IdCiclo);

                //InizializzaListaCariche();
                return ListaBrevi;
            }
            catch (Exception Ex)
            {
                Log.Error("CaricaListaCariche: " + Ex.Message);
                return null;
            }
        }




    }
}
