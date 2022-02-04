using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LicnostService.Entities.DataContext
{
    public class LicnostContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public LicnostContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Licnost> Licnost { get; set; }
        public DbSet<Komisija> Komisija { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("LicnostDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Komisija>()
                .HasMany(l => l.ClanoviKomisije)
                .WithMany(k => k.Komisije);

            modelBuilder.Entity<Komisija>()
                .HasOne(l => l.PredsednikKomisije);

            modelBuilder.Entity<Licnost>()
                .HasData(new
                {
                    LicnostId = Guid.Parse("95DBBC3B-B961-4364-9BF5-B6F2B4256393"),
                    Ime = "Stefan",
                    Prezime = "Bulaja",
                    Funkcija = "Sef"
                    
                },
                new
                {
                    LicnostId = Guid.Parse("4DA64D71-EE63-4886-BCD3-FB7AE004A384"),
                    Ime = "Marko",
                    Prezime = "Markovic",
                    Funkcija = "Licitant"
                    
                },
                new
                {
                    LicnostId = Guid.Parse("E3DB1E95-C4DB-4E11-AC52-9B9E26207E1C"),
                    Ime = "Pera",
                    Prezime = "Peric",
                    Funkcija = "Licitant"
                    
                });
            
                modelBuilder.Entity<Komisija>()
                .HasData(new
                {
                    KomisijaId = Guid.Parse("25410D44-6D96-486E-AFD5-1409B906B3DE"),
                    NazivKomisije = "Subotica_Komisija",
                    PredsednikKomisijeId = Guid.Parse("95DBBC3B-B961-4364-9BF5-B6F2B4256393")                 
                },
                new
                {
                    KomisijaId = Guid.Parse("8B2E2F05-3796-444A-9DC2-2C62372C0FA9"),
                    NazivKomisije = "Subotica_Komisija2",
                    PredsednikKomisijeId = Guid.Parse("4DA64D71-EE63-4886-BCD3-FB7AE004A384")
                    
                },
                new
                {
                    KomisijaId = Guid.Parse("289D3F53-8584-49E1-9511-D994A6DDA9A9"),
                    NazivKomisije = "Subotica_Komisija",
                    PredsednikKomisijeId = Guid.Parse("4DA64D71-EE63-4886-BCD3-FB7AE004A384")
                    
                });

            modelBuilder.Entity("KomisijaLicnost").HasData(
    new { KomisijeKomisijaId = Guid.Parse("289D3F53-8584-49E1-9511-D994A6DDA9A9"), ClanoviKomisijeLicnostId = Guid.Parse("E3DB1E95-C4DB-4E11-AC52-9B9E26207E1C") },
    new { KomisijeKomisijaId = Guid.Parse("289D3F53-8584-49E1-9511-D994A6DDA9A9"), ClanoviKomisijeLicnostId = Guid.Parse("95DBBC3B-B961-4364-9BF5-B6F2B4256393") },
    new { KomisijeKomisijaId = Guid.Parse("25410D44-6D96-486E-AFD5-1409B906B3DE"), ClanoviKomisijeLicnostId = Guid.Parse("E3DB1E95-C4DB-4E11-AC52-9B9E26207E1C") });

        }

    }
}
