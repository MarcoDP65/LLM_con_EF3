using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace MoriData
{
    public partial class LadeLightDataContext : DbContext
    {
        public LadeLightDataContext()
        {
        }

        public LadeLightDataContext(DbContextOptions<LadeLightDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TArticoli> TArticoli { get; set; }
        public virtual DbSet<TCodiciAssegnati> TCodiciAssegnati { get; set; }
        public virtual DbSet<TContatori> TContatori { get; set; }
        public virtual DbSet<TInitArticolo> TInitArticolo { get; set; }
        public virtual DbSet<TModuli> TModuli { get; set; }
        public virtual DbSet<TParametriProgrammazione> TParametriProgrammazione { get; set; }
        public virtual DbSet<TStepInit> TStepInit { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=192.168.1.50;Initial Catalog=LadeLightData;Persist Security Info=True;User ID=EmmeUser;Password=EmmePWD");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TArticoli>(entity =>
            {
                entity.HasKey(e => e.CodArticolo);

                entity.ToTable("T_ARTICOLI");

                entity.Property(e => e.CodArticolo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Amax).HasColumnName("AMax");

                entity.Property(e => e.DataFine).HasColumnType("datetime");

                entity.Property(e => e.DataInizio).HasColumnType("datetime");

                entity.Property(e => e.Descrizione)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Linea)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoModulo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Vmax).HasColumnName("VMax");

                entity.Property(e => e.Vmin).HasColumnName("VMin");
            });

            modelBuilder.Entity<TCodiciAssegnati>(entity =>
            {
                entity.HasKey(e => e.SerialId);

                entity.ToTable("T_CODICI_ASSEGNATI");

                entity.Property(e => e.SerialId).HasColumnName("SerialID");

                entity.Property(e => e.Amax)
                    .HasColumnName("AMax")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Articolo)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Cliente)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.DataAssegnazione).HasColumnType("datetime");

                entity.Property(e => e.LineaProdotto)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.TipoModuli)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.Vmax)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Vmin)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<TContatori>(entity =>
            {
                entity.HasKey(e => new { e.LineaProdotto, e.Anno });

                entity.ToTable("T_CONTATORI");

                entity.Property(e => e.LineaProdotto)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.DataAssegnazione).HasColumnType("datetime");
            });

            modelBuilder.Entity<TInitArticolo>(entity =>
            {
                entity.HasKey(e => e.IdInizializzazione);

                entity.ToTable("T_INIT_ARTICOLO");

                entity.Property(e => e.IdInizializzazione).HasColumnName("ID_Inizializzazione");

                entity.Property(e => e.CodArticolo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DataCreazione).HasColumnType("datetime");

                entity.Property(e => e.Descrizione)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TModuli>(entity =>
            {
                entity.HasKey(e => e.IdTipoModulo);

                entity.ToTable("T_MODULI");

                entity.Property(e => e.IdTipoModulo)
                    .HasColumnName("Id_TipoModulo")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Amax).HasColumnName("AMax");

                entity.Property(e => e.Anom).HasColumnName("ANom");

                entity.Property(e => e.Descrizione)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.TipoModulo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Vmax).HasColumnName("VMax");

                entity.Property(e => e.Vmin).HasColumnName("VMin");

                entity.Property(e => e.Vnom).HasColumnName("VNom");

                entity.Property(e => e.Wnom).HasColumnName("WNom");
            });

            modelBuilder.Entity<TParametriProgrammazione>(entity =>
            {
                entity.HasKey(e => e.IdLocale);

                entity.ToTable("T_PARAMETRI_PROGRAMMAZIONE");

                entity.Property(e => e.IdLocale).ValueGeneratedNever();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DataInstallazione)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.IdApparato)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsUnicode(false);

                entity.Property(e => e.IdModelloLl).HasColumnName("IdModelloLL");

                entity.Property(e => e.LastUser)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ProgramDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProgramName)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.RevisionDate).HasColumnType("datetime");

                entity.Property(e => e.TempoAttesaBms).HasColumnName("TempoAttesaBMS");

                entity.Property(e => e.TempoErogazioneBms).HasColumnName("TempoErogazioneBMS");

                entity.Property(e => e.TipoApparato)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.VcellLimite).HasColumnName("VCellLimite");

                entity.Property(e => e.VlimFase0).HasColumnName("VLimFase0");

                entity.Property(e => e.Vmax).HasColumnName("VMax");

                entity.Property(e => e.VraccordoF1).HasColumnName("VRaccordoF1");

                entity.Property(e => e.Vsoglia).HasColumnName("VSoglia");
            });

            modelBuilder.Entity<TStepInit>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("T_STEP_INIT");

                entity.HasIndex(e => new { e.IdInizializzazione, e.Step })
                    .HasName("IX_T_STEP_INIT")
                    .IsUnique();

                entity.Property(e => e.Descrizione)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdInizializzazione).HasColumnName("ID_Inizializzazione");

                entity.Property(e => e.ParInt01).HasColumnName("Par_Int_01");

                entity.Property(e => e.ParInt02).HasColumnName("Par_Int_02");

                entity.Property(e => e.ParInt03).HasColumnName("Par_Int_03");

                entity.Property(e => e.ParInt04).HasColumnName("Par_Int_04");

                entity.Property(e => e.ParInt05).HasColumnName("Par_Int_05");

                entity.Property(e => e.ParInt06).HasColumnName("Par_Int_06");

                entity.Property(e => e.ParInt07).HasColumnName("Par_Int_07");

                entity.Property(e => e.ParInt08).HasColumnName("Par_Int_08");

                entity.Property(e => e.ParInt09).HasColumnName("Par_Int_09");

                entity.Property(e => e.ParInt10).HasColumnName("Par_Int_10");

                entity.Property(e => e.ParLongChar01)
                    .HasColumnName("Par_LongChar_01")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParLongChar02)
                    .HasColumnName("Par_LongChar_02")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParVarChar01)
                    .HasColumnName("Par_VarChar_01")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ParVarChar02)
                    .HasColumnName("Par_VarChar_02")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ParVarChar03)
                    .HasColumnName("Par_VarChar_03")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ParVarChar04)
                    .HasColumnName("Par_VarChar_04")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ParVarChar05)
                    .HasColumnName("Par_VarChar_05")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
