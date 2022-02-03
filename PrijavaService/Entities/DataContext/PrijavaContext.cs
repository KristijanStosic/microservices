using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PrijavaService.Entities.ManyToMany;

namespace PrijavaService.Entities.DataContext
{
    public class PrijavaContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public PrijavaContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }


        public DbSet<DokFizickaLica> DokFizickaLica { get; set; }
        public DbSet<DokPravnaLica> DokPravnaLica { get; set; }
        public DbSet<Prijava> Prijava { get; set; }
        public DbSet<PrijavaJavnoNadmetanje> PrijavaJavnoNadmetanje { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("PrijavaDB"));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<PrijavaJavnoNadmetanje>()
                .HasOne(p => p.Prijava)
                .WithMany()
                .HasForeignKey("PrijavaId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<PrijavaJavnoNadmetanje>()
                .HasKey(p => new { p.PrijavaId, p.JavnoNadmetanjeId });

            modelBuilder.Entity<DokFizickaLica>()
                .HasData(
                new
                {
                    DokFizickaLicaId = Guid.Parse("522de090-3873-4113-821c-c000f6ef7ae5"),
                    NazivDokumenta = "Dokument FL 1",
                    PrijavaId = Guid.Parse("a370bc58-2cb2-4d8d-9cfb-b444841aeb80")
                },
                new
                {
                    DokFizickaLicaId = Guid.Parse("dd8e0e7b-0c07-45e8-a1ad-1517853e8940"),
                    NazivDokumenta = "Dokument FL 2",
                    PrijavaId = Guid.Parse("a370bc58-2cb2-4d8d-9cfb-b444841aeb80")
                });

             modelBuilder.Entity<DokPravnaLica>()
                .HasData(
                new
                {
                    DokPravnaLicaId = Guid.Parse("6edac589-e1f6-4733-b9d3-da55256ee707"),
                    NazivDokumenta = "Dokument PL 1",
                    PrijavaId = Guid.Parse("3040da81-b4b5-47bd-a47c-f1474341f162")
                },
                new
                {
                    DokPravnaLicaId = Guid.Parse("de1f2617-6def-48c1-a523-218a2b062916"),
                    NazivDokumenta = "Dokument PL 2",
                    PrijavaId = Guid.Parse("3040da81-b4b5-47bd-a47c-f1474341f162")
                });


              modelBuilder.Entity<Prijava>()
               .HasData(
               new
               {
                   PrijavaId = Guid.Parse("3040da81-b4b5-47bd-a47c-f1474341f162"),
                   BrojPrijave = "B22",
                   DatumPrijave = DateTime.Now,
                   MestoPrijave = "Mesto 1",
                   SatPrijave = DateTime.Now.ToString("HH:mm"),
                   ZatvorenaPonuda =  true
               },
               new
               {
                   PrijavaId = Guid.Parse("a370bc58-2cb2-4d8d-9cfb-b444841aeb80"),
                   BrojPrijave = "B255",
                   DatumPrijave = DateTime.Now,
                   MestoPrijave = "Mesto 2",
                   SatPrijave = DateTime.Now.ToString("HH:mm"),
                   ZatvorenaPonuda = false
               });

               modelBuilder.Entity<PrijavaJavnoNadmetanje>()
                 .HasData(
                 new
                 {
                     PrijavaId = Guid.Parse("3040da81-b4b5-47bd-a47c-f1474341f162"),
                     JavnoNadmetanjeId = Guid.Parse("E22F999D-5C61-4DCE-965B-9C6667EFC74D")
                 },
                 new
                 {
                     PrijavaId = Guid.Parse("3040da81-b4b5-47bd-a47c-f1474341f162"),
                     JavnoNadmetanjeId = Guid.Parse("5ED44CAB-255D-4BB7-9CC9-828EC90BFAF5")
                 },
                 new
                 {
                     PrijavaId = Guid.Parse("a370bc58-2cb2-4d8d-9cfb-b444841aeb80"),
                     JavnoNadmetanjeId = Guid.Parse("5ED44CAB-255D-4BB7-9CC9-828EC90BFAF5")
                 });


        }
    }
}
