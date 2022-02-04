﻿
using KupacService.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KupacService.Entities.DataContext
{
    public class KupacContext : DbContext
    {

        private readonly IConfiguration _configuration;
        public KupacContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this._configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           optionsBuilder.UseSqlServer(_configuration.GetConnectionString("KupacDb"));
        }

        public DbSet<Prioritet> Prioriteti { get; set; }
        public DbSet<KontaktOsoba> KontaktOsobe { get; set; }
        public DbSet<Kupac> Kupci { get; set; }
        public DbSet<FizickoLice> FizickaLica { get; set; }
        public DbSet<PravnoLice> PravnaLica { get; set; }
        public DbSet<KupacOvlascenoLice> kupacOvlascenoLice { get; set; }



        /// <summary>
        /// Popunjava bazu sa nekim inicijalnim podacima
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<KupacOvlascenoLice>()
                .HasOne(k => k.Kupac)
                .WithMany()
                .HasForeignKey("KupacId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<KupacOvlascenoLice>()
                .HasKey(ko => new { ko.KupacId, ko.OvlascenoLiceId });

            List<Prioritet> prioriteti = new List<Prioritet>
            {
                new Prioritet()
                {
                    PrioritetId = Guid.Parse("2578e81b-3f01-479a-b790-f52106f639f7"),
                    Opis = "Vlasnik sistema za navodnjavanje"
                },
                 new Prioritet()
                {
                     PrioritetId = Guid.Parse("f2b8faa4-732c-4480-8b0a-34d65e483930"),
                     Opis = "Vlasnik zemljišta koje se graniči sazemljištem koje se daje u zakup"
                }
            };

            modelBuilder.Entity<Prioritet>()
                .HasData(prioriteti);
           

            modelBuilder.Entity<KontaktOsoba>()
                .HasData(new
                {
                    KontaktOsobaId = Guid.Parse("244fb7c4-aab8-4ec4-8960-e48e017bad37"),
                    Ime = "Milan",
                    Prezime = "Drazic",
                    Telefon="0693432534",
                    Funkcija = "Funkcija1"

                });

            modelBuilder.Entity<KontaktOsoba>()
               .HasData(new
               {
                   KontaktOsobaId = Guid.Parse("da2197a4-891f-4a40-a1f2-313962701627"),
                   Ime = "Mark",
                   Prezime = "Todor",
                   Telefon = "0693432534",
                   Funkcija = "Funkcija2"

               });
            
            modelBuilder.Entity<FizickoLice>()
                .HasData(new FizickoLice
                {
                    KupacId = Guid.Parse("febd1c29-90e7-40c2-97f3-1e88495fe98d"),
                    Ime = "Filip",
                    Prezime = "Ivanic",
                    JMBG = "1253627363526",
                    OstvarenaPovrsina = 500,
                    ImaZabranu = false,
                    DatumPocetkaZabrane = new DateTime(),
                    BrojTelefona = "069453432543",
                    Email = "filip@gmail.com",
                    BrojRacuna = "908 ‑ 10501 ‑ 97",
                    AdresaId = Guid.Parse("1C989EE3-13B2-4D3B-ABEB-C4E6343EACE7")
                });
            
            modelBuilder.Entity<PravnoLice>()
                .HasData(new
                {
                    KupacId = Guid.Parse("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"),
                    Naziv = "Firma",
                    MaticniBroj = "1254327363526",
                    Faks = "+1-212-9876543",
                    OstvarenaPovrsina = 200.00,
                    ImaZabranu = true,
                    DatumPocetkaZabrane = DateTime.Now,
                    DuzinaTrajanjaZabraneGod = 2,
                    BrojTelefona = "069453232543",
                    Email = "Firma@gmail.com",
                    BrojRacuna = "934 ‑ 10501 ‑ 97",
                    KontaktOsobaId = Guid.Parse("da2197a4-891f-4a40-a1f2-313962701627")

                });


            modelBuilder.Entity<Kupac>()
               .HasMany(k => k.Prioriteti)
               .WithMany(p => p.Kupci)
               .UsingEntity<Dictionary<string, object>>(
                   "KupacPrioritet",
                   r => r.HasOne<Prioritet>().WithMany().HasForeignKey("PrioritetId"),
                   l => l.HasOne<Kupac>().WithMany().HasForeignKey("KupacId"),
                   je =>
                   {
                       je.HasKey("KupacId", "PrioritetId");
                       je.HasData(
                           new { KupacId = Guid.Parse("febd1c29-90e7-40c2-97f3-1e88495fe98d"),
                               PrioritetId = Guid.Parse("2578e81b-3f01-479a-b790-f52106f639f7") });
                          
                   });



            modelBuilder.Entity<KupacOvlascenoLice>()
                .HasData(
                new { 
                    KupacId =Guid.Parse("febd1c29-90e7-40c2-97f3-1e88495fe98d"),
                    OvlascenoLiceId = Guid.Parse("5ED44CAB-255D-4BB7-9CC9-828EC90BFAF5")
                },
                new
                {
                    KupacId = Guid.Parse("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"),
                    OvlascenoLiceId = Guid.Parse("5ED44CAB-255D-4BB7-9CC9-828EC90BFAF5")
                },
                new
                {
                    KupacId = Guid.Parse("4ba95c01-aa89-4d36-a467-c72b0fcc5b80"),
                    OvlascenoLiceId = Guid.Parse("5E1BFFFC-1AEE-4662-BC04-341C35B9EBDC")
                });





        }
    }
}
