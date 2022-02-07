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

        public DbSet<KatastarskaOpstina> KatastarskaOpstina  { get; set; }

        public DbSet<Parcela> Parcela { get; set; }

        public DbSet<DeoParcele> DeoParcele { get; set; }

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
            modelBuilder.Entity<ZasticenaZona>()
                .HasData(new
                {
                    ZasticenaZonaId = Guid.Parse("7663ebd7-8ad6-4e1a-9ed0-f0b3c07caa30"),
                    BrojZasticeneZone = "1"
                },
                new
                {
                    ZasticenaZonaId = Guid.Parse("875fc830-5370-4d21-82bc-ceb8ce77e5d2"),
                    BrojZasticeneZone = "2"
                },
                new
                {
                    ZasticenaZonaId = Guid.Parse("0bb57a3e-5ab2-4ab0-b33b-0e3c53d713d1"),
                    BrojZasticeneZone = "3"
                });
            modelBuilder.Entity<KatastarskaOpstina>()
               .HasData(new
               {
                   KatastarskaOpstinaId = Guid.Parse("9ef61f9a-a078-423c-96f3-c83bd2d1806f"),
                   NazivKatastarskeOpstine = "Čantavir"
               },
               new
               {
                   KatastarskaOpstinaId = Guid.Parse("2e101ea1-f68d-411c-934d-07970563474d"),
                   NazivKatastarskeOpstine = "Bački Vinogradi"
               },
               new
               {
                   KatastarskaOpstinaId = Guid.Parse("8772f89a-65ed-4240-95a8-cfd06775283b"),
                   NazivKatastarskeOpstine = "Bikovo"
               },
               new
               {
                   KatastarskaOpstinaId = Guid.Parse("ce8db946-e6a1-42e3-96b6-3ca9387a64c4"),
                   NazivKatastarskeOpstine = "Đuđin"
               },
               new
               {
                   KatastarskaOpstinaId = Guid.Parse("2fd6ea35-4bec-41a6-b705-87cb1fc4ff20"),
                   NazivKatastarskeOpstine = "Žednik"
               },
               new
               {
                   KatastarskaOpstinaId = Guid.Parse("e5ff87f4-6a6f-48ac-b3e4-2f324d2eb3cb"),
                   NazivKatastarskeOpstine = "Tavankut"
               },
               new
               {
                   KatastarskaOpstinaId = Guid.Parse("6905b37f-4449-41f4-8ac5-9e416dd17ff0"),
                   NazivKatastarskeOpstine = "Bajmok"
               },
               new
               {
                   KatastarskaOpstinaId = Guid.Parse("60e61296-b612-4950-b2d9-8766b82ebdb5"),
                   NazivKatastarskeOpstine = "Donji Grad"
               },
               new
               {
                   KatastarskaOpstinaId = Guid.Parse("571591d6-9d2c-4ca3-99f6-4f3d7df6863b"),
                   NazivKatastarskeOpstine = "Stari Grad"

               },
               new
               {
                   KatastarskaOpstinaId = Guid.Parse("78a28389-ef8f-4713-9640-ce047d99470e"),
                   NazivKatastarskeOpstine = "Novi Grad"

               },
               new
               {
                   KatastarskaOpstinaId = Guid.Parse("a8670b5a-41bc-4f90-9b37-6b78c06d2fd3"),
                   NazivKatastarskeOpstine = "Palić"

               });

            modelBuilder.Entity<Parcela>()
               .HasData(new
               {
                   ParcelaId = Guid.Parse("5d99fd91fcf44975891ba7ae3053ff52"),
                   BrojParcele = "178",
                   PovrsinaParcele = "250",
                   BrojListeNepokretnosti = "8",
                   ZasticenaZonaId = Guid.Parse("7663ebd7-8ad6-4e1a-9ed0-f0b3c07caa30"),
                   OblikSvojineId = Guid.Parse("f5133187-104c-4849-9d53-995e17e51094"),
                   OdvodnjavanjeId = Guid.Parse("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"),
                   KatastarskaOpstinaId = Guid.Parse("9ef61f9a-a078-423c-96f3-c83bd2d1806f"),
 
               },
               new
               {
                   ParcelaId = Guid.Parse("985eacdabe534506a7b2339fc6acf3d6"),
                   BrojParcele = "289",
                   PovrsinaParcele = "100",
                   BrojListeNepokretnosti = "7",
                   ZasticenaZonaId = Guid.Parse("7663ebd7-8ad6-4e1a-9ed0-f0b3c07caa30"),
                   OblikSvojineId = Guid.Parse("f5133187-104c-4849-9d53-995e17e51094"),
                   OdvodnjavanjeId = Guid.Parse("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"),
                   KatastarskaOpstinaId = Guid.Parse("9ef61f9a-a078-423c-96f3-c83bd2d1806f"),
               },
               new
               {
                   ParcelaId = Guid.Parse("73e47b70c8fb43e3beb95f1b627a59bf"),
                   BrojParcele = "158",
                   PovrsinaParcele = "150",
                   BrojListeNepokretnosti = "6",
                   ZasticenaZonaId = Guid.Parse("875fc830-5370-4d21-82bc-ceb8ce77e5d2"),
                   OblikSvojineId = Guid.Parse("f5133187-104c-4849-9d53-995e17e51094"),
                   OdvodnjavanjeId = Guid.Parse("b2f93ba3-ec00-41af-824e-fcd1f0c60c5c"),
                   KatastarskaOpstinaId = Guid.Parse("9ef61f9a-a078-423c-96f3-c83bd2d1806f"),
               });

            modelBuilder.Entity<DeoParcele>()
               .HasData(new
               {
                   DeoParceleId = Guid.Parse("17894615-CA22-4943-87C8-16C246A35879"),
                   RedniBrojDela = "12",
                   PovrsinaDela = "40",
                   KulturaId = Guid.Parse("f32bf0e6-cb02-49b0-a035-79e350255742"),
                   KlasaId = Guid.Parse("f39c9623-c6b6-48cb-b4dd-0340c7431870"),
                   ObradivostId = Guid.Parse("1c48c1d4-122b-4bd2-a8fe-188e54c5a88a"),
                   ParcelaId = Guid.Parse("73e47b70c8fb43e3beb95f1b627a59bf"),
                   KupacId = Guid.Parse("febd1c29-90e7-40c2-97f3-1e88495fe98d")

               }, new
               {
                   DeoParceleId = Guid.Parse("44302280-3611-4667-BCFC-08B4E272BB28"),
                   RedniBrojDela = "14",
                   PovrsinaDela = "15",
                   KulturaId = Guid.Parse("dcfc0f60-1683-4f16-919f-b6fbdf361fac"),
                   KlasaId = Guid.Parse("0ec59e12-b271-471f-9a13-5c9c8ed0eda7"),
                   ObradivostId = Guid.Parse("ec9e3d3e-193d-4de8-bdc4-acc3e4bf834d"),
                   ParcelaId = Guid.Parse("5d99fd91fcf44975891ba7ae3053ff52"),
                   KupacId = Guid.Parse("4ba95c01-aa89-4d36-a467-c72b0fcc5b80")

               }, new
               {
                   DeoParceleId = Guid.Parse("C5BFE49C-518E-4216-A379-A5EC94B9EFE2"),
                   RedniBrojDela = "20",
                   PovrsinaDela = "25",
                   KulturaId = Guid.Parse("d89d9175-bdf0-4066-850a-4232318f80bb"),
                   KlasaId = Guid.Parse("e496b563-abb9-48a9-8972-800f41a4a3a1"),
                   ObradivostId = Guid.Parse("0d62386e-e188-49a9-a8e6-492fa14baeb4"),
                   ParcelaId = Guid.Parse("5d99fd91fcf44975891ba7ae3053ff52"),
                   KupacId = Guid.Parse("4ba95c01-aa89-4d36-a467-c72b0fcc5b80")

               });

        }
    }
}
