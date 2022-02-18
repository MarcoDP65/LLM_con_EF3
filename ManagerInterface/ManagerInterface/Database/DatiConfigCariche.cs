
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoriData
{
    public class DatiConfigCariche
    {
        public List<_llModelloCb> ModelliLL;
        public List<_llProfiloCarica> ProfiliCarica;
        public List<llDurataCarica> DurateCarica;
        public List<llDurataProfilo> DurateProfilo;
        public List<_llProfiloTipoBatt> ProfiloTipoBatt;
        public List<llTensioneBatteria> TensioniBatteria;
        public List<llTensioniModello> TensioniModello;


        public DatiConfigCariche()
        {
            InizializzaDatiLocali();
        }

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
            // Il primo bit caratterzza la famiglia: 0 supercharger, 1 Lade Light

            // LADE Light
            ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0xFF, NomeModello = "N.D.", CorrenteMin = 0, CorrenteMax = 0, TensioneMin = 0, TensioneMax = 0, Ordine = 0, Trifase = 0, Attivo = 0xff, FamigliaCaricabetteria = ChargerLogic.CaricaBatteria.TipoCaricaBatteria.NonDefinito, TensioneNominale = 0 });
            ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0x81, NomeModello = "LLT.3 24-80V / 120A", CorrenteMin = 10, CorrenteMax = 120, TensioneMin = 24, TensioneMax = 120, Ordine = 1, Trifase = 1, Attivo = 0x01, FamigliaCaricabetteria = ChargerLogic.CaricaBatteria.TipoCaricaBatteria.LadeLight, TensioneNominale = 0 });  // V max--> 80/2*3 = 120
                                                                                                                                                                                                                              //ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0x82, NomeModello = "Trifase 24-48 / 200", CorrenteMin = 10, CorrenteMax = 200, TensioneMin = 24, TensioneMax = 92, Ordine = 2, Trifase = 1, Attivo = 0x00 });   // V max--> 48/2*3 =  92
                                                                                                                                                                                                                              //ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0xC1, NomeModello = "Monofase 24 / 70", CorrenteMin = 10, CorrenteMax = 70, TensioneMin = 24, TensioneMax = 24, Ordine = 3, Trifase = 0, Attivo = 0x00 });

            // SUPERCHARGER - i modelli sono caratterizzati solo per tensione nominale, la corrente è definita sul dispositivo in base ai moduli
            ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0x18, NomeModello = "SCHG 24V", CorrenteMin = 0, CorrenteMax = 0, TensioneMin = 24, TensioneMax = 29, Ordine = 101, Trifase = 1, Attivo = 0xff, FamigliaCaricabetteria = ChargerLogic.CaricaBatteria.TipoCaricaBatteria.SuperCharger,TensioneNominale = 24 });
            // ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0x24, NomeModello = "SCHG 36V", CorrenteMin = 0, CorrenteMax = 0, TensioneMin = 36, TensioneMax = 43, Ordine = 102, Trifase = 1, Attivo = 0xff, FamigliaCaricabetteria = ChargerLogic.CaricaBatteria.TipoCaricaBatteria.SuperCharger,TensioneNominale = 36 });
            ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0x30, NomeModello = "SCHG 48V", CorrenteMin = 0, CorrenteMax = 0, TensioneMin = 48, TensioneMax = 53, Ordine = 103, Trifase = 1, Attivo = 0xff, FamigliaCaricabetteria = ChargerLogic.CaricaBatteria.TipoCaricaBatteria.SuperCharger, TensioneNominale = 48 });
            ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0x50, NomeModello = "SCHG 80V", CorrenteMin = 0, CorrenteMax = 0, TensioneMin = 80, TensioneMax = 96, Ordine = 104, Trifase = 1, Attivo = 0xff, FamigliaCaricabetteria = ChargerLogic.CaricaBatteria.TipoCaricaBatteria.SuperCharger, TensioneNominale = 80 });
            // ModelliLL.Add(new _llModelloCb() { IdModelloLL = 0x5C, NomeModello = "SCHG 92V", CorrenteMin = 0, CorrenteMax = 0, TensioneMin = 92, TensioneMax = 110, Ordine = 105, Trifase = 1, Attivo = 0xff, FamigliaCaricabetteria = ChargerLogic.CaricaBatteria.TipoCaricaBatteria.SuperCharger,TensioneNominale = 92 });



            return true;
        }

        public bool inizializzaProfili()
        {
            ProfiliCarica = new List<_llProfiloCarica>();
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x00, NomeProfilo = "Non Definito", DurataFase2 = 100, Attivo = 2, FlagPb = 0, FlagGel = 0, FlagLitio = 0, Ordine = 0, AttivaRiarmoPulse = 0, AttivaEqual = 0x00, Grafico = "" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x01, NomeProfilo = "IWa", DurataFase2 = 100, Attivo = 0, FlagPb = 0, FlagGel = 0, FlagLitio = 0, Ordine = 1, AttivaRiarmoPulse = 0, AttivaEqual = 0xF0, Grafico = "IWa650" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x02, NomeProfilo = "IU", DurataFase2 = 100, Attivo = 1, FlagPb = 0, FlagGel = 1, FlagLitio = 0, Ordine = 2, AttivaRiarmoPulse = 0, AttivaEqual = 0x00, Grafico = "IU650" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x03, NomeProfilo = "IUIa", DurataFase2 = 100, Attivo = 0, FlagPb = 0, FlagGel = 0, FlagLitio = 0, Ordine = 3, AttivaRiarmoPulse = 0, AttivaEqual = 0x00, Grafico = "IUIa650" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x04, NomeProfilo = "IWa Pb13", DurataFase2 = 60, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 4, AttivaRiarmoPulse = 0, AttivaEqual = 0xF0, Grafico = "IWa650" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x05, NomeProfilo = "IWa Pb11", DurataFase2 = 100, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 5, AttivaRiarmoPulse = 0, AttivaEqual = 0xF0, Grafico = "IWa650" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x06, NomeProfilo = "IWa Pb8", DurataFase2 = 120, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 6, AttivaRiarmoPulse = 0, AttivaEqual = 0xF0, Grafico = "IWa650" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x07, NomeProfilo = "Litio", DurataFase2 = 100, Attivo = 1, FlagPb = 0, FlagGel = 0, FlagLitio = 1, Ordine = 7, AttivaRiarmoPulse = 0xF0, AttivaEqual = 0x00, Grafico = "LITIO650" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x08, NomeProfilo = "IWa Pb13 Equal", DurataFase2 = 60, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 8, AttivaRiarmoPulse = 0, AttivaEqual = 0xFF, Grafico = "IWa650" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x09, NomeProfilo = "IWa Pb11 Equal", DurataFase2 = 100, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 9, AttivaRiarmoPulse = 0, AttivaEqual = 0xFF, Grafico = "IWa650" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x0A, NomeProfilo = "IWa Pb8 Equal", DurataFase2 = 120, Attivo = 1, FlagPb = 1, FlagGel = 0, FlagLitio = 0, Ordine = 10, AttivaRiarmoPulse = 0, AttivaEqual = 0xFF, Grafico = "IWa650" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x0B, NomeProfilo = "Litio con BMS", DurataFase2 = 100, Attivo = 1, FlagPb = 0, FlagGel = 0, FlagLitio = 1, Ordine = 7, AttivaRiarmoPulse = 0xF0, AttivaEqual = 0x00, Grafico = "" });
            ProfiliCarica.Add(new _llProfiloCarica() { IdProfiloCaricaLL = 0x0C, NomeProfilo = "Litio con CAN", DurataFase2 = 100, Attivo = 1, FlagPb = 0, FlagGel = 0, FlagLitio = 1, Ordine = 7, AttivaRiarmoPulse = 0xF0, AttivaEqual = 0x00, Grafico = "" });
            return true;
        }

        public bool inizializzaDurate()
        {
            DurateCarica = new List<llDurataCarica>();
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 780, Descrizione = "13:00", Ordine = 0, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 60, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 720, Descrizione = "12:00", Ordine = 1, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 60, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 660, Descrizione = "11:00", Ordine = 2, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 100, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 600, Descrizione = "10:00", Ordine = 3, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 100, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 540, Descrizione = " 9:00", Ordine = 4, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 100, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 480, Descrizione = " 8:00", Ordine = 5, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 120, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 420, Descrizione = " 7:00", Ordine = 6, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 120, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 360, Descrizione = " 6:00", Ordine = 7, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 120, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 300, Descrizione = " 5:00", Ordine = 8, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 120, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 240, Descrizione = " 4:00", Ordine = 9, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 120, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 180, Descrizione = " 3:00", Ordine = 10, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 140, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 120, Descrizione = " 2:00", Ordine = 11, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 140, Attivo = 1 });
            DurateCarica.Add(new llDurataCarica() { IdDurataCaricaLL = 60, Descrizione = " 1:00", Ordine = 12, ProfiloGel = 100, ProfiloLitio = 100, ProfiloPb = 140, Attivo = 1 });
            return true;
        }

        public bool inizializzaTensioni()
        {
            TensioniBatteria = new List<llTensioneBatteria>();
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 2400, Descrizione = "24,00", Attivo = 1, Ordine = 0 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 3600, Descrizione = "36,00", Attivo = 1, Ordine = 1 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 4800, Descrizione = "48,00", Attivo = 1, Ordine = 2 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 7200, Descrizione = "72,00", Attivo = 1, Ordine = 3 });
            TensioniBatteria.Add(new llTensioneBatteria() { IdTensione = 8000, Descrizione = "80,00", Attivo = 1, Ordine = 4 });
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

            //Piombo IWa 13
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x08, Attivo = 1 });

            //Piombo IWa 11
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x09, Attivo = 1 });

            //Piombo IWa 8
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x0A, Attivo = 1 });

            //Litio BMS
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 720, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 600, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 540, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 420, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 360, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 300, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 240, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 180, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 120, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 60, IdProfiloCaricaLL = 0x0B, Attivo = 1 });

            //Litio CAN
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 780, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 720, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 660, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 600, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 540, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 480, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 420, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 360, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 300, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 240, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 180, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 120, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
            DurateProfilo.Add(new llDurataProfilo() { IdDurataCaricaLL = 60, IdProfiloCaricaLL = 0x0C, Attivo = 1 });
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
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x08, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x09, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x71, IdProfiloCaricaLL = 0x0A, Attivo = 1 });

            // Gel
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x72, IdProfiloCaricaLL = 0x02, Attivo = 1 });

            // Litio
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x73, IdProfiloCaricaLL = 0x07, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x73, IdProfiloCaricaLL = 0x0B, Attivo = 1 });
            ProfiloTipoBatt.Add(new _llProfiloTipoBatt() { BatteryTypeId = 0x73, IdProfiloCaricaLL = 0x0C, Attivo = 1 });

            return true;
        }

        public bool inizializzaTensioniModello()
        {

            TensioniModello = new List<llTensioniModello>();

            // 24/80 - 120 Trifase
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 2400, TxTensione = "24,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 3600, TxTensione = "36,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 4800, TxTensione = "48,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 7200, TxTensione = "72,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x81, IdTensione = 8000, TxTensione = "80,00", Attivo = 1 });

            // 24/48 - 200 Trifase
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x82, IdTensione = 2400, TxTensione = "24,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x82, IdTensione = 3600, TxTensione = "36,00", Attivo = 1 });
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x82, IdTensione = 4800, TxTensione = "48,00", Attivo = 1 });

            // SCHG 24 Trifase
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x18, IdTensione = 2400, TxTensione = "24,00", Attivo = 1 });

            // SCHG 48 Trifase
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x30, IdTensione = 4800, TxTensione = "48,00", Attivo = 1 });

            // SCHG 80 Trifase
            TensioniModello.Add(new llTensioniModello() { IdModelloLL = 0x50, IdTensione = 8000, TxTensione = "80,00", Attivo = 1 });


            return true;
        }

    }
}
