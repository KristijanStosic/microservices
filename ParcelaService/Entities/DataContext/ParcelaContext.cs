using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParcelaService.Entities.DataContext
{
    public class ParcelaContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ParcelaContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Klasa> Klasa { get; set; }
        public DbSet<Kultura> Kultura { get; set; }

        public DbSet<Obradivost> Obradivost { get; set; }

        public DbSet<Odvodnjavanje> Odvodnjavanje { get; set; }

        public DbSet<OblikSvojine> OblikSvojine { get; set; }

        public DbSet<ZasticenaZona> ZasticenaZona { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ParcelaDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Klasa>()
                .HasData(new
                {
                    KlasaId = Guid.Parse("f39c9623-c6b6-48cb-b4dd-0340c7431870"),
                    KlasaNaziv = "Prva"
                },
                new
                {
                    KlasaId = Guid.Parse("0ec59e12-b271-471f-9a13-5c9c8ed0eda7"),
                    KlasaNaziv = "Druga"
                },
                new
                {
                    KlasaId = Guid.Parse("e496b563-abb9-48a9-8972-800f41a4a3a1"),
                    KlasaNaziv = "Treca"
                }
                );

            modelBuilder.Entity<Kultura>()
                .HasData(new
                {
                    KulturaId = Guid.Parse("f32bf0e6-cb02-49b0-a035-79e350255742"),
                    NazivKulture = "Njiva"
                },
                new
                {
                    KulturaId = Guid.Parse("dcfc0f60-1683-4f16-919f-b6fbdf361fac"),
                    NazivKulture = "Vinograd"
                },
                new
                {
                    KulturaId = Guid.Parse("d89d9175-bdf0-4066-850a-4232318f80bb"),
                    NazivKulture = "Livada"
                });

            modelBuilder.Entity<Obradivost>()
                .HasData(new
                {
                    ObradivostId = Guid.Parse("1c48c1d4-122b-4bd2-a8fe-188e54c5a88a"),
                    OpisObradivosti = "Moze se raditi redukovana obrada u odredjenoj meri"
                },
                new
                {
                    ObradivostId = Guid.Parse("ec9e3d3e-193d-4de8-bdc4-acc3e4bf834d"),
                    OpisObradivosti = "Ne moze se raditi direktna setva"
                },
                new
                {
                    ObradivostId = Guid.Parse("0d62386e-e188-49a9-a8e6-492fa14baeb4"),
                    OpisObradivosti = "Pogodno gajenje vecine kultura"
                });

            modelBuilder.Entity<Odvodnjavanje>()
                .HasData(new
                {
                    OdvodnjavanjeId = Guid.Parse("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"),
                    OpisOdvodnjavanja = "Odvodnjavanje na parceli nije potrebno."
                },
                new
                {
                    OdvodnjavanjeId = Guid.Parse("d3c4ebaa-178c-4d62-afbc-05819e041021"),
                    OpisOdvodnjavanja = "Odvodnjavanje na parceli nije potrebno."
                },
                new
                {
                    OdvodnjavanjeId = Guid.Parse("6601b7a7-d1be-4216-9844-8d68d680847e"),
                    OpisOdvodnjavanja = "Odvodnjavanje na parceli nije potrebno."
                });

            modelBuilder.Entity<OblikSvojine>()
                .HasData(new
                {
                    OblikSvojineId = Guid.Parse("f5133187-104c-4849-9d53-995e17e51094"),
                    OpisOblikaSvojine = "Privatna svojina"
                },
                new
                {
                    OblikSvojineId = Guid.Parse("03e3208e-eb61-40f1-b9d4-36fb3f63e4c6"),
                    OpisOblikaSvojine = "Zajednicka svojina"
                });
        }
    }
}
