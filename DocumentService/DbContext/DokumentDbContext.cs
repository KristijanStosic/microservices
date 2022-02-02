using System;
using DocumentService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DocumentService.DbContext
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
            modelBuilder.Entity<TipDokumenta>()
                .HasData(
                    new TipDokumenta
                    {
                        Id = Guid.Parse("77CBE573-4AD7-4DB8-B9B1-7A9F45BEC5DB"),
                        NazivTipa = "Potvrda o statusu poljoprivrednog gazdinstava u registru poljoprivrednih gazdinstava"
                    }
                    ,new TipDokumenta
                    {
                        Id = Guid.Parse("3E649B2D-5569-4463-80F3-04382EF4A7EB"),
                        NazivTipa = "Izvod iz javne evidencije o nepokretnosti"
                    }
                );

            modelBuilder.Entity<Dokument>()
                .HasData(
                    new Dokument
                    {
                        ZavodniBroj = "PSPG-1/2022",
                        Datum = DateTime.Now,
                        DatumDonosenjaDokumenta = DateTime.Now.AddDays(3),
                        TipDokumentaId = Guid.Parse("77CBE573-4AD7-4DB8-B9B1-7A9F45BEC5DB")
                    },
                    new Dokument
                    {
                        ZavodniBroj = "IJEN-1/2022",
                        Datum = DateTime.Now,
                        DatumDonosenjaDokumenta = DateTime.Now.AddDays(3),
                        TipDokumentaId = Guid.Parse("3E649B2D-5569-4463-80F3-04382EF4A7EB")
                    },
                    new Dokument
                    {
                        ZavodniBroj = "PSPG-2/2022",
                        Datum = DateTime.Now,
                        DatumDonosenjaDokumenta = DateTime.Now.AddDays(3),
                        TipDokumentaId = Guid.Parse("77CBE573-4AD7-4DB8-B9B1-7A9F45BEC5DB")
                    },
                    new Dokument
                    {
                        ZavodniBroj = "IJEN-2/2022",
                        Datum = DateTime.Now,
                        DatumDonosenjaDokumenta = DateTime.Now.AddDays(3),
                        TipDokumentaId = Guid.Parse("3E649B2D-5569-4463-80F3-04382EF4A7EB")
                    });
        }
    }
}