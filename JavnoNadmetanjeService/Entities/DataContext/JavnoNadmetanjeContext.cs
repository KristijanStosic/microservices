using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace JavnoNadmetanjeService.Entities
{
    public class JavnoNadmetanjeContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public JavnoNadmetanjeContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Status> Status { get; set; }
        public DbSet<Tip> Tip { get; set; }
        public DbSet<JavnoNadmetanje> JavnoNadmetanje { get; set; }
        public DbSet<Etapa> Etapa { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("JavnoNadmetanjeDB"));
        }

        /// <summary>
        /// Unos inicijalnih podataka u bazu
        /// </summary>
        /// <param name="modelBuilder">Omogucava podesavanja/unos podataka pri kreiranju modela</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>()
                .HasData(new
                {
                    StatusId = Guid.Parse("8457E79A-D24A-4623-A34C-CDE32435DF23"),
                    NazivStatusa = "Prvi krug"
                },
                new
                {
                    StatusId = Guid.Parse("3B7EE65F-EB68-4A32-AE69-DF7FDF463188"),
                    NazivStatusa = "Drugi krug sa starim uslovima"
                },
                new
                {
                    StatusId = Guid.Parse("1588803D-C08C-4D14-9343-954F6DC785E8"),
                    NazivStatusa = "Drugi krug sa novim uslovima"
                });

            modelBuilder.Entity<Tip>()
                .HasData(new
                {
                    TipId = Guid.Parse("D6D56B98-3672-4BDB-A0CB-E916FFE053C8"),
                    NazivTipa = "Javna licitacija"
                },
                new
                {
                    TipId = Guid.Parse("AB5B1FE9-D09F-471D-8E4C-E55EBD7E87B3"),
                    NazivTipa = "Otvaranje zatvorenih ponuda"
                });

            modelBuilder.Entity<Etapa>()
                .HasData(new
                {
                    EtapaId = Guid.Parse("BAF25A2F-68AF-4F36-A300-F4F78BA1D10C"),
                    Datum = DateTime.Now,
                    DanPoRedu = 1,
                    VremePocetka = DateTime.Now.ToString("HH:mm"),
                    VremeKraja = DateTime.Now.AddHours(1).ToString("HH:mm"),
                    ZavrsenaUspesno = false,
                    JavnoNadmetanjeId = Guid.Parse("56A7CFF5-CB0A-46B8-BFC1-93DB415FEEB4")
                },
                new
                {
                    EtapaId = Guid.Parse("0B410560-5868-4F34-8695-098BFC2B53E1"),
                    Datum = DateTime.Now.AddDays(5),
                    DanPoRedu = 1,
                    VremePocetka = DateTime.Now.ToString("HH:mm"),
                    VremeKraja = DateTime.Now.AddHours(2).ToString("HH:mm"),
                    ZavrsenaUspesno = false,
                    JavnoNadmetanjeId = Guid.Parse("6849BBBE-5798-4C04-AA20-58DE420AA578")
                },
                new
                {
                    EtapaId = Guid.Parse("759775E2-7E4B-43D3-A0E5-395CB22F9BB5"),
                    Datum = DateTime.Now.AddDays(2),
                    DanPoRedu = 2,
                    VremePocetka = DateTime.Now.ToString("HH:mm"),
                    VremeKraja = DateTime.Now.AddHours(4).ToString("HH:mm"),
                    ZavrsenaUspesno = true,
                    JavnoNadmetanjeId = Guid.Parse("56A7CFF5-CB0A-46B8-BFC1-93DB415FEEB4")
                }) ;

            modelBuilder.Entity<JavnoNadmetanje>()
                .HasData(new
                {
                    JavnoNadmetanjeId = Guid.Parse("56A7CFF5-CB0A-46B8-BFC1-93DB415FEEB4"),
                    PocetnaCenaHektar = 350.50000000,
                    VisinaDopuneDepozita = 50,
                    PeriodZakupa = 2,
                    IzlicitiranaCena = 400,
                    BrojUcesnika = 5,
                    Krug = 2,
                    Izuzeto = false,
                    StatusId = Guid.Parse("8457E79A-D24A-4623-A34C-CDE32435DF23"),
                    TipId = Guid.Parse("AB5B1FE9-D09F-471D-8E4C-E55EBD7E87B3"),
                },
                new
                {
                    JavnoNadmetanjeId = Guid.Parse("6849BBBE-5798-4C04-AA20-58DE420AA578"),
                    PocetnaCenaHektar = 550.00000000,
                    VisinaDopuneDepozita = 150,
                    PeriodZakupa = 3,
                    IzlicitiranaCena = 1400,
                    BrojUcesnika = 15,
                    Krug = 1,
                    Izuzeto = false,
                    StatusId = Guid.Parse("3B7EE65F-EB68-4A32-AE69-DF7FDF463188"),
                    TipId = Guid.Parse("D6D56B98-3672-4BDB-A0CB-E916FFE053C8")
                },                                
                new
                {
                    JavnoNadmetanjeId = Guid.Parse("B195C4C2-2B26-40AD-8FF6-C212474ACC83"),
                    PocetnaCenaHektar = 1350.50000000,
                    VisinaDopuneDepozita = 3,
                    PeriodZakupa = 1,
                    IzlicitiranaCena = 1000,
                    BrojUcesnika = 7,
                    Krug = 5,
                    Izuzeto = true,
                    StatusId = Guid.Parse("8457E79A-D24A-4623-A34C-CDE32435DF23"),
                    TipId = Guid.Parse("D6D56B98-3672-4BDB-A0CB-E916FFE053C8")
                });

        }
    }
}
