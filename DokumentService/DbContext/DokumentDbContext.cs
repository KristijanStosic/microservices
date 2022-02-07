using System;
using DokumentService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DokumentService.DbContext
{
    public class DokumentDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly IConfiguration _configuration;

        public DokumentDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Dokument> Dokumenti { get; set; }
        public DbSet<TipDokumenta> TipoviDokumenta { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DokumentDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dokument>()
                .HasIndex(td => td.ZavodniBroj)
                .IsUnique();

            modelBuilder.Entity<TipDokumenta>()
                .HasIndex(td => td.NazivTipa)
                .IsUnique();

            modelBuilder.Entity<TipDokumenta>()
                .HasData(
                    new TipDokumenta
                    {
                        Id = Guid.Parse("77CBE573-4AD7-4DB8-B9B1-7A9F45BEC5DB"),
                        NazivTipa =
                            "Potvrda o statusu poljoprivrednog gazdinstava u registru poljoprivrednih gazdinstava"
                    }
                    , new TipDokumenta
                    {
                        Id = Guid.Parse("3E649B2D-5569-4463-80F3-04382EF4A7EB"),
                        NazivTipa = "Izvod iz javne evidencije o nepokretnosti"
                    }
                );

            modelBuilder.Entity<Dokument>()
                .HasData(
                    new Dokument
                    {
                        Id = Guid.Parse("854a5603-31c6-4815-b393-a2b61887ba5e"),
                        ZavodniBroj = "PSPG-1/2022",
                        Datum = DateTime.Now,
                        DatumDonosenjaDokumenta = DateTime.Now.AddDays(3),
                        TipDokumentaId = Guid.Parse("77CBE573-4AD7-4DB8-B9B1-7A9F45BEC5DB")
                    },
                    new Dokument
                    {
                        Id = Guid.Parse("13b6c9df-03b9-4313-af6a-6c2e076e8a0a"),
                        ZavodniBroj = "IJEN-1/2022",
                        Datum = DateTime.Now,
                        DatumDonosenjaDokumenta = DateTime.Now.AddDays(3),
                        TipDokumentaId = Guid.Parse("3E649B2D-5569-4463-80F3-04382EF4A7EB")
                    },
                    new Dokument
                    {
                        Id = Guid.Parse("7147fd52-b938-4cec-8b81-dfa51f123f0c"),
                        ZavodniBroj = "PSPG-2/2022",
                        Datum = DateTime.Now,
                        DatumDonosenjaDokumenta = DateTime.Now.AddDays(3),
                        TipDokumentaId = Guid.Parse("77CBE573-4AD7-4DB8-B9B1-7A9F45BEC5DB")
                    }
                );
        }
    }
}