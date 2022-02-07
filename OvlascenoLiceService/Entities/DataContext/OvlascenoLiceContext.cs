using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace OvlascenoLiceService.Entities.DataContext
{
    /// <summary>
    /// Ovlasceno lice DB Context
    /// </summary>
    public class OvlascenoLiceContext : DbContext
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Ovlasceno Lice Context konstruktor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        public OvlascenoLiceContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// DBSet za ovlasceno lice
        /// </summary>
        public DbSet<OvlascenoLice> OvlascenoLice { get; set; }
        /// <summary>
        /// DBSet za broj table
        /// </summary>
        public DbSet<BrojTable> BrojTable { get; set; }

        /// <summary>
        /// Metoda u kojoj se definise konekcioni string za bazu
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("OvlascenoLiceDB"));
        }

        /// <summary>
        /// Popunjavamo bazu inicijalnim podacima
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OvlascenoLice>()
                .HasData(new
                {
                    OvlascenoLiceId = Guid.Parse("5ED44CAB-255D-4BB7-9CC9-828EC90BFAF5"),
                    Ime = "Petar",
                    Prezime = "Petrovic",
                    JMBG = "1002987800035",
                    AdresaId = Guid.Parse("1c989ee3-13b2-4d3b-abeb-c4e6343eace7")
                },
                new
                {
                    OvlascenoLiceId = Guid.Parse("5E1BFFFC-1AEE-4662-BC04-341C35B9EBDC"),
                    Ime = "Marko",
                    Prezime = "Markovic",
                    JMBG = "2004983800022",
                    AdresaId = Guid.Parse("37371ef6-4f25-48b3-9bf2-fe72a81f88d2")
                }, new
                {
                    OvlascenoLiceId = Guid.Parse("E22F999D-5C61-4DCE-965B-9C6667EFC74D"),
                    Ime = "Nemanja",
                    Prezime = "Nenic",
                    BrojPasosa = "0252624",
                    DrzavaId = Guid.Parse("9ce21ce2-7809-4e28-ba74-fd2f1bc6466a")
                });

            modelBuilder.Entity<BrojTable>()
                .HasData(new
                {
                    BrojTableId = Guid.Parse("F7837323-AFEB-4AA9-BA28-CD0912BB1FAC"),
                    OznakaTable = "Talba3",
                    OvlascenoLiceId = Guid.Parse("E22F999D-5C61-4DCE-965B-9C6667EFC74D")
                },
                new
                {
                    BrojTableId = Guid.Parse("97A9C3EC-6AF3-4B02-AD18-097702F62FA0"),
                    OznakaTable = "Tabla12",
                    OvlascenoLiceId = Guid.Parse("E22F999D-5C61-4DCE-965B-9C6667EFC74D")
                }, new
                {
                    BrojTableId = Guid.Parse("33DBD6BE-207B-43A5-AB4F-65AA47C6EE3C"),
                    OznakaTable = "Tabla25",
                    OvlascenoLiceId = Guid.Parse("5E1BFFFC-1AEE-4662-BC04-341C35B9EBDC")
                });
        }
    }
}
