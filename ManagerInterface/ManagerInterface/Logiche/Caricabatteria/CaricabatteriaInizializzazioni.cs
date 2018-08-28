using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using FTD2XX_NET;
using MoriData;
using Utility;
using Newtonsoft.Json;

using System.Threading;
using System.ComponentModel;

using SQLite.Net;

namespace ChargerLogic
{
    public partial class CaricaBatteria
    {

        public bool InizializzaDatiLocali()
        {
            inizializzaModelli();
            inizializzaProfili();
            inizializzaDurate();
            inizializzaDurateProfilo();
            inizializzaBatteriaProfilo();
            inizializzaTensioni();
            inizializzaTensioniModello();
            return true;
        }

        public bool inizializzaModelli()
        {
            ModelliLL = new List<_llModelloCb>();
            ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0xFF, NomeModello = "Non Definito", CorrenteMin = 0, CorrenteMax = 0, TensioneMin = 0, TensioneMax = 0, Ordine = 0, Trifase = 0 });
            ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0x81, NomeModello = "Trifase 24-80 / 120", CorrenteMin = 10, CorrenteMax = 120, TensioneMin = 24, TensioneMax = 80, Ordine = 1, Trifase = 1 });
            ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0x82, NomeModello = "Trifase 24-48 / 200", CorrenteMin = 10, CorrenteMax = 200, TensioneMin = 24, TensioneMax = 48, Ordine = 2, Trifase = 1 });
            ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0x01, NomeModello = "Monofase 24 / 70", CorrenteMin = 10, CorrenteMax = 70, TensioneMin = 24, TensioneMax = 24, Ordine = 3, Trifase = 0 });
            return true;
        }

        public bool inizializzaProfili()
        {
            ProfiliCarica = new List<_llProfiloCarica>();
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x00, NomeProfilo = "Non Definito", DutataFase2 = 0, Attivo = 2, FlagPb = 0, FlagGel = 0, FlagLitio = 0, Ordine = 0 });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x01, NomeProfilo = "IWa", DutataFase2 = 0, Attivo = 0, FlagPb = 0, FlagGel = 0, FlagLitio = 0, Ordine = 1 });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x02, NomeProfilo = "IU", DutataFase2 = 0, Attivo = 1, FlagPb = 0, FlagGel = 1, FlagLitio = 0, Ordine = 2 });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x03, NomeProfilo = "IUIa", DutataFase2 = 0, Attivo = 0, FlagPb = 0, FlagGel = 0, FlagLitio = 0, Ordine = 3 });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x04, NomeProfilo = "IWa Pb13", DutataFase2 = 60, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 4 });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x05, NomeProfilo = "IWa Pb11", DutataFase2 = 100, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 5 });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x06, NomeProfilo = "IWa Pb8", DutataFase2 = 120, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 6 });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x07, NomeProfilo = "Litio", DutataFase2 = 0, Attivo = 1, FlagPb = 0, FlagGel = 0, FlagLitio = 1, Ordine = 7 });
            return true;
        }

        public bool inizializzaDurate()
        {
            DurateCarica = new List<llDurataCarica>();
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 780, Descrizione = "13:00", Ordine = 0 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 720, Descrizione = "12:00", Ordine = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 660, Descrizione = "11:00", Ordine = 2 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 600, Descrizione = "10:00", Ordine = 3 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 540, Descrizione = " 9:00", Ordine = 4 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 480, Descrizione = " 8:00", Ordine = 5 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 420, Descrizione = " 7:00", Ordine = 6 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 360, Descrizione = " 6:00", Ordine = 7 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 300, Descrizione = " 5:00", Ordine = 8 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 240, Descrizione = " 4:00", Ordine = 9 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 180, Descrizione = " 3:00", Ordine = 10 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 120, Descrizione = " 2:00", Ordine = 11 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 60, Descrizione = " 1:00", Ordine = 12 });
            return true;
        }

        public bool inizializzaTensioni()
        {
            TensioniBatteria = new List<llTensioneBatteria>();
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 2400, Descrizione = "24.00", Attivo = 1, Ordine = 0 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 3600, Descrizione = "36.00", Attivo = 1, Ordine = 1 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 4800, Descrizione = "48.00", Attivo = 1, Ordine = 2 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 7200, Descrizione = "72.00", Attivo = 1, Ordine = 3 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 8000, Descrizione = "80.00", Attivo = 1, Ordine = 4 });
            return true;
        }

        public bool inizializzaDurateProfilo()
        {
            DurateProfilo = new List<llDurataProfilo>();

            //Litio
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 720, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 600, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 540, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 420, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 360, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 300, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 240, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 180, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 120, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 60, IdProfiloCaricaLL = 0x07, Attivo = 1 });

            //Gel - IU
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 600, IdProfiloCaricaLL = 0x02, Attivo = 1 });

            //Piombo IWa generico
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x01, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x01, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x01, Attivo = 1 });

            //Piombo IWa 13
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x04, Attivo = 1 });

            //Piombo IWa 11
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x05, Attivo = 1 });

            //Piombo IWa 8
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x06, Attivo = 1 });

            return true;
        }

        public bool inizializzaBatteriaProfilo()
        {

            ProfiloTipoBatt = new List<_llProfiloTipoBatt>();

            // Piombo
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x01, Attivo = 0 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x04, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x05, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x06, Attivo = 1 });

            // Gel
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x72, IdProfiloCaricaLL = 0x02, Attivo = 1 });

            // Litio
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x73, IdProfiloCaricaLL = 0x07, Attivo = 1 });

            return true;
        }

        public bool inizializzaTensioniModello()
        {

            TensioniModello = new List<llTensioniModello>();

            // 24/80 - 120 Trifase
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 2400, Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 3600, Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 4800, Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 7200, Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 8000, Attivo = 1 });

            // 24/48 - 200 Trifase
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x82, IdTensione = 2400, Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x82, IdTensione = 3600, Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x82, IdTensione = 4800, Attivo = 1 });

            // 24 - 70 Trifase
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x01, IdTensione = 2400, Attivo = 1 });

            return true;
        }

    }
}
