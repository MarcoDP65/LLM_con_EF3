﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;
using log4net;
using log4net.Config;

namespace MoriData
{
    /// <summary>
    /// llProfiloCarica: struttura record per il salvataggio dati modello Lade Light
    /// </summary>
    /// 
    public class _llProfiloCarica
    {
        [PrimaryKey]
        public byte IdProfiloCaricaLL { get; set; }

        public string NomeProfilo { get; set; }
        public ushort DurataFase2 { get; set; }
        public byte Attivo { get; set; }
        public byte FlagPb { get; set; }
        public byte FlagGel { get; set; }
        public byte FlagLitio { get; set; }
        public byte TipoBatteria { get; set; }
        public short AttesaBMS { get; set; }
        public byte AttivaEqual { get; set; }        // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF,  0x0F Libero ON
        public byte AttivaRiarmoPulse { get; set; }  // 0x00 Bloccato OFF, 0xFF bloccato ON, 0xF0 Libero OFF
        public string Grafico { get; set; }          // Nome dell'immagine profilo nel file risorse

        public int Ordine { get; set; }

    }

    public class _llProfiloTipoBatt
    {
        [PrimaryKey]
        public byte IdProfiloCaricaLL { get; set; }
        public byte BatteryTypeId { get; set; }

        public byte Attivo { get; set; }
    }
}




