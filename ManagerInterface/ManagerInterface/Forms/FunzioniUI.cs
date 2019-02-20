﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using Newtonsoft.Json;
using System.Windows.Forms;
using log4net;
using log4net.Config;
using ChargerLogic;
using Utility;

namespace PannelloCharger
{
    public class FunzioniUI
    {

        private static ILog Log = LogManager.GetLogger("FunzioniUI");

        public static bool ImpostaTextBoxUshort(ref TextBox txtValore, ushort Valore, ushort Stato, byte TipoDati)
        {
            try
            {
                bool esito = false;
                if (txtValore == null) return false;
                switch(Stato)
                {
                    case 0:
                        {
                            txtValore.Text = "";
                            txtValore.Enabled = false;
                            esito = true;
                            break;
                        }
                    case 1:
                        {
                            txtValore.Enabled = false;
                            txtValore.ReadOnly = true;
                            switch (TipoDati)
                            {
                                case 0:  // valore diretto
                                    {
                                        txtValore.Text = Valore.ToString();
                                        break;
                                    }
                                case 1:  // Tensione
                                    {
                                        txtValore.Text = FunzioniMR.StringaTensione(Valore);
                                        break;
                                    }
                                case 2:  // Corrente
                                    {
                                        txtValore.Text = FunzioniMR.StringaCorrenteLL(Valore);
                                        break;
                                    }
                                default:  // valore diretto
                                    {
                                        txtValore.Text = Valore.ToString();
                                        break;
                                    }

                            }
                            esito = true;
                            break;
                        }
                    case 4:
                    case 5:
                        {
                            txtValore.Enabled = true;
                            txtValore.ReadOnly = (bool)(Stato == 4);
                            switch (TipoDati)
                            {
                                case 0:  // valore diretto
                                    {
                                        txtValore.Text = Valore.ToString();
                                        break;
                                    }
                                case 1:  // Tensione
                                    {
                                        txtValore.Text = FunzioniMR.StringaTensione(Valore);
                                        break;
                                    }
                                case 2:  // Corrente
                                    {
                                        txtValore.Text = FunzioniMR.StringaCorrenteLL(Valore);
                                        break;
                                    }
                                default:  // valore diretto
                                    {
                                        txtValore.Text = Valore.ToString();
                                        break;
                                    }

                            }
                            esito = true;
                            break;
                        }

                    default:
                        {
                            txtValore.Text = "";
                            txtValore.Enabled = false;
                            esito = false;
                            break;
                        }

                }
                return esito;



            }
            catch (Exception Ex)
            {
                Log.Error("ImpostaTextBoxUshort: " + Ex.Message);
                return false;
            }
        }

        public static bool ImpostaCheckBoxUshort(ref CheckBox chkValore,ref Label lblDescription, ushort Valore, ushort Stato, byte TipoDati)
        {
            try
            {
                bool esito = false;
                if (chkValore == null) return false;
                switch (Stato)
                {
                    case 0:
                    case 1:
                        {
                            chkValore.Checked = false;
                            chkValore.Enabled = false;
                            if (lblDescription!= null)
                            {
                                lblDescription.Enabled = false;
                            }
                            esito = true;
                            break;
                        }

                    case 4:
                    case 5:
                        {
                            chkValore.Checked = (bool)(Valore>0);
                            chkValore.Enabled = true;
                            if (lblDescription != null)
                            {
                                lblDescription.Enabled = true;
                            }
                            esito = true;
                            break;
                        }

                    default:
                        {
                            chkValore.Checked = false;
                            chkValore.Enabled = false;
                            if (lblDescription != null)
                            {
                                lblDescription.Enabled = false;
                            }
                            esito = true;
                            break;

                        }

                }
                return esito;



            }
            catch (Exception Ex)
            {
                Log.Error("ImpostaTextBoxUshort: " + Ex.Message);
                return false;
            }
        }

    }
    
}
