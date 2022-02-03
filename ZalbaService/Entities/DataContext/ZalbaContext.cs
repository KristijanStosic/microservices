using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Entities.DataContext
{
    /// <summary>
    /// Žalba DB Context
    /// </summary>
    public class ZalbaContext : DbContext
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Žalba Context konstruktor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public ZalbaContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// DBSet za status žalbe
        /// </summary>
        public DbSet<StatusZalbe> StatusZalbe { get; set; }
        /// <summary>
        /// DBSet za tip žalbe
        /// </summary>
        public DbSet<TipZalbe> TipZalbe { get; set; }
        /// <summary>
        /// DBSet za radnju za žalbu
        /// </summary>
        public DbSet<RadnjaZaZalbu> RadnjaZaZalbu { get; set; }
        /// <summary>
        /// DBSet za žalbu
        /// </summary>
        public DbSet<Zalba> Zalba { get; set; }
        /// <summary>
        /// Metoda u kojoj se definise konekcioni string za bazu
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ZalbaDB"));
        }

        /// <summary>
        /// Unos inicijalnih podataka u bazu
        /// </summary>
        /// <param name="modelBuilder">Omogucava podesavanja/unos podataka pri kreiranju modela</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StatusZalbe>()
                .HasData(new
                {
                    StatusZalbeId = Guid.Parse("6E5E8A67-006B-4AC0-89D0-9711006C0D28"),
                    NazivStatusaZalbe = "Usvojena"
                },
                new
                {
                    StatusZalbeId = Guid.Parse("ABDB833B-0706-4012-8689-C59BED59C6B4"),
                    NazivStatusaZalbe = "Odbijena"
                },
                new
                {
                    StatusZalbeId = Guid.Parse("2C1051D2-0DDF-41A9-BA08-24070A50F4B3"),
                    NazivStatusaZalbe = "Otvorena"
                });

            modelBuilder.Entity<TipZalbe>()
                .HasData(new
                {
                    TipZalbeId = Guid.Parse("10EA64F1-07AB-478D-B8F3-073BCE4610F8"),
                    NazivTipaZalbe = "Zalba na tok javnog nadmetanja"
                },
                new
                {
                    TipZalbeId = Guid.Parse("B4947A43-42E4-4D20-A10B-169C5089AAC6"),
                    NazivTipaZalbe = "Zalba na Odluku o davanju u zakup"
                },
                new
                {
                    TipZalbeId = Guid.Parse("018AC715-4588-4934-BB2D-8BB2F4D1049A"),
                    NazivTipaZalbe = "Zalba na Odluku o davanju na koriscenje"
                });

            modelBuilder.Entity<RadnjaZaZalbu>()
                .HasData(new
                {
                    RadnjaZaZalbuId = Guid.Parse("009AA493-7786-4AAD-9D1A-0F90D57EBBB4"),
                    NazivRadnjeZaZalbu = "JN ide u drugi krug sa novim uslovima"
                },
                new
                {
                    RadnjaZaZalbuId = Guid.Parse("4CCB6D66-18B2-4791-8AFE-B628A4F7C0AF"),
                    NazivRadnjeZaZalbu = "JN ide u drugi krug sa starim uslovima"
                },
                new
                {
                    RadnjaZaZalbuId = Guid.Parse("DF645DD7-3E65-41CD-A1F4-81F936A7DB49"),
                    NazivRadnjeZaZalbu = "JN ne ide u drugi krug"
                });

            modelBuilder.Entity<Zalba>()
                .HasData(new
                {
                    ZalbaId = Guid.NewGuid(),
                    DatumPodnosenja = DateTime.Now,
                    DatumResenja = DateTime.Now.AddDays(50),
                    RazlogZalbe = "Nedovoljno licitanata",
                    Obrazlozenje = "Nema dovoljno licitanata da se odrzi javno nadmetanje",
                    BrojNadmetanja = "100-NN",
                    BrojResenja = "X9NN41HH",
                    StatusZalbeId = Guid.Parse("6E5E8A67-006B-4AC0-89D0-9711006C0D28"),
                    TipZalbeId = Guid.Parse("018AC715-4588-4934-BB2D-8BB2F4D1049A"),
                    RadnjaZaZalbuId = Guid.Parse("009AA493-7786-4AAD-9D1A-0F90D57EBBB4")
                },
                new
                {
                    ZalbaId = Guid.NewGuid(),
                    DatumPodnosenja = DateTime.Now,
                    DatumResenja = DateTime.Now.AddDays(50),
                    RazlogZalbe = "Nepotpuna dokumentacija",
                    Obrazlozenje = "Dokumentacija nije potpuna kako bi se odrzalo javno nadmetanje",
                    BrojNadmetanja = "200-OO",
                    BrojResenja = "IIKK-55",
                    StatusZalbeId = Guid.Parse("6E5E8A67-006B-4AC0-89D0-9711006C0D28"),
                    TipZalbeId = Guid.Parse("B4947A43-42E4-4D20-A10B-169C5089AAC6"),
                    RadnjaZaZalbuId = Guid.Parse("009AA493-7786-4AAD-9D1A-0F90D57EBBB4")
                },
                new
                {
                    ZalbaId = Guid.NewGuid(),
                    DatumPodnosenja = DateTime.Now,
                    DatumResenja = DateTime.Now.AddDays(50),
                    RazlogZalbe = "Nedovoljno uplacenih novcanih sredstava",
                    Obrazlozenje = "Nema dovoljno novcanih sredstava za javno nadmetanje",
                    BrojNadmetanja = "999-AA",
                    BrojResenja = "QWOP44-MM",
                    StatusZalbeId = Guid.Parse("2C1051D2-0DDF-41A9-BA08-24070A50F4B3"),
                    TipZalbeId = Guid.Parse("B4947A43-42E4-4D20-A10B-169C5089AAC6"),
                    RadnjaZaZalbuId = Guid.Parse("4CCB6D66-18B2-4791-8AFE-B628A4F7C0AF")
                });
        }
    }
}
