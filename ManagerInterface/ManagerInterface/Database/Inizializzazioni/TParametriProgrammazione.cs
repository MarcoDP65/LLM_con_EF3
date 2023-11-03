using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoriData
{
    public partial class TParametriProgrammazione
    {
        public int IdLocale { get; set; }
        public string IdApparato { get; set; }
        public string TipoApparato { get; set; }
        public int? IdProgramma { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? RevisionDate { get; set; }
        public string LastUser { get; set; }
        public string DataInstallazione { get; set; }
        public string ProgramName { get; set; }
        public int? BatteryType { get; set; }
        public int? BatteryVdef { get; set; }
        public int? BatteryAhdef { get; set; }
        public int? NumeroCelle { get; set; }
        public int? Vsoglia { get; set; }
        public int? VraccordoF1 { get; set; }
        public int? Vmax { get; set; }
        public int? VcellLimite { get; set; }
        public int? BatteryVminRec { get; set; }
        public int? BatteryVmaxRec { get; set; }
        public int? BatteryVminStop { get; set; }
        public int? CorrenteMax { get; set; }
        public int? CorrenteFase3 { get; set; }
        public int? EqualTempoAttesa { get; set; }
        public int? EqualNumImpulsi { get; set; }
        public int? EqualDurataPausa { get; set; }
        public int? EqualDurataImpulso { get; set; }
        public int? EqualCorrenteImpulso { get; set; }
        public int? IdProfilo { get; set; }
        public int? DurataMaxCarica { get; set; }
        public int? PercTempoFase2 { get; set; }
        public int? DurataMinFase2 { get; set; }
        public int? DurataMaxFase2 { get; set; }
        public int? DurataMaxFase3 { get; set; }
        public int? OpportunityOraInizio { get; set; }
        public int? OpportunityOraFine { get; set; }
        public int? OpportunityDurataMax { get; set; }
        public int? OpportunityTensioneMax { get; set; }
        public int? OpportunityCorrente { get; set; }
        public int? AbilitaContattoSafety { get; set; }
        public int? AbilitaComunicazioneSpybatt { get; set; }
        public int? TempoErogazioneBms { get; set; }
        public int? TempoAttesaBms { get; set; }
        public int? ProgrammaInUso { get; set; }
        public int? TipoRecord { get; set; }
        public int? OpzioniAttive { get; set; }
        public int? IdModelloLl { get; set; }
        public int? PosizioneCorrente { get; set; }
        public int? CapacitaDaRicaricare { get; set; }
        public int? PercCapacitaMassimaRic { get; set; }
        public int? VlimFase0 { get; set; }
        public int? CorrentePrecicloI0 { get; set; }
        public int? CorrenteCaricaI1 { get; set; }
        public int? CorrenteFinaleF2 { get; set; }
        public int? CorrenteRaccordo { get; set; }
        public int? DurataMaxPrecarica { get; set; }
        public string ProgramDescription { get; set; }
    }
}
