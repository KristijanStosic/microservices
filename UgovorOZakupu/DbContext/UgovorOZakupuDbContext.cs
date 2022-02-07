using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UgovorOZakupu.Entities;

namespace UgovorOZakupu.DbContext
{
    public class UgovorOZakupuDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly IConfiguration _configuration;

        public UgovorOZakupuDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Entities.UgovorOZakupu> UgovoriOZakupu { get; set; }
        public DbSet<TipGarancije> TipoviGarancije { get; set; }
        public DbSet<RokDospeca> RokoviDospeca { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("UgovorOZakupuDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipGarancije>()
                .HasData(
                    new TipGarancije
                    {
                        Id = Guid.Parse("3085ea9a-ea2a-4577-9e03-467812d8ef56"),
                        NazivTipa = "Činidbena garancija"
                    },
                    new TipGarancije
                    {
                        Id = Guid.Parse("be38618a-783c-4850-af91-9645105f54f8"),
                        NazivTipa = "Avansna garancija"
                    }
                );

            modelBuilder.Entity<Entities.UgovorOZakupu>()
                .HasData(
                    new Entities.UgovorOZakupu
                    {
                        Id = Guid.Parse("ea9e8d12-0af9-4926-b022-fd1bd951a3b6"),
                        ZavodniBroj = "UZ1/02-2022",
                        DatumZavodjenja = DateTime.Now.AddDays(-3),
                        RokZaVracanje = DateTime.Now.AddDays(11),
                        MestoPotpisivanja = "Subotica",
                        DatumPotpisivanja = DateTime.Now.AddDays(-3).AddMinutes(-3),
                        TipGarancijeId = Guid.Parse("3085ea9a-ea2a-4577-9e03-467812d8ef56"),
                        DokumentId = Guid.Parse("854a5603-31c6-4815-b393-a2b61887ba5e"),
                        JavnoNadmetanjeId = Guid.Parse("56A7CFF5-CB0A-46B8-BFC1-93DB415FEEB4"),
                        KupacId = Guid.Parse("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"),
                        LicnostId = Guid.Parse("95DBBC3B-B961-4364-9BF5-B6F2B4256393")
                    },
                    new Entities.UgovorOZakupu
                    {
                        Id = Guid.Parse("a0dcd0f0-a6b2-4564-922a-b54880363c39"),
                        ZavodniBroj = "UZ2/02-2022",
                        DatumZavodjenja = DateTime.Now.AddMinutes(-1),
                        RokZaVracanje = DateTime.Now.AddDays(13),
                        MestoPotpisivanja = "Novi Sad",
                        DatumPotpisivanja = DateTime.Now.AddMinutes(-3),
                        TipGarancijeId = Guid.Parse("3085ea9a-ea2a-4577-9e03-467812d8ef56"),
                        DokumentId = Guid.Parse("13b6c9df-03b9-4313-af6a-6c2e076e8a0a"),
                        JavnoNadmetanjeId = Guid.Parse("6849BBBE-5798-4C04-AA20-58DE420AA578"),
                        KupacId = Guid.Parse("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"),
                        LicnostId = Guid.Parse("4DA64D71-EE63-4886-BCD3-FB7AE004A384")
                    },
                    new Entities.UgovorOZakupu
                    {
                        Id = Guid.Parse("42988239-b116-4100-bd15-06869c0d6fba"),
                        ZavodniBroj = "UZ3/02-2022",
                        DatumZavodjenja = DateTime.Now.AddMinutes(5),
                        RokZaVracanje = DateTime.Now.AddDays(14),
                        MestoPotpisivanja = "Beograd",
                        DatumPotpisivanja = DateTime.Now.AddMinutes(3),
                        TipGarancijeId = Guid.Parse("be38618a-783c-4850-af91-9645105f54f8"),
                        DokumentId = Guid.Parse("7147fd52-b938-4cec-8b81-dfa51f123f0c"),
                        JavnoNadmetanjeId = Guid.Parse("B195C4C2-2B26-40AD-8FF6-C212474ACC83"),
                        KupacId = Guid.Parse("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"),
                        LicnostId = Guid.Parse("E3DB1E95-C4DB-4E11-AC52-9B9E26207E1C")
                    }
                );

            modelBuilder.Entity<RokDospeca>().HasData(
                new RokDospeca
                {
                    Rok = 3,
                    UgovorOZakupuId = Guid.Parse("ea9e8d12-0af9-4926-b022-fd1bd951a3b6")
                },
                new RokDospeca
                {
                    Rok = 6,
                    UgovorOZakupuId = Guid.Parse("ea9e8d12-0af9-4926-b022-fd1bd951a3b6")
                },
                new RokDospeca
                {
                    Rok = 7,
                    UgovorOZakupuId = Guid.Parse("a0dcd0f0-a6b2-4564-922a-b54880363c39")
                },
                new RokDospeca
                {
                    Rok = 10,
                    UgovorOZakupuId = Guid.Parse("42988239-b116-4100-bd15-06869c0d6fba")
                }
            );
        }
    }
}