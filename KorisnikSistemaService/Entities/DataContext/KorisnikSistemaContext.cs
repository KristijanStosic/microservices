using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace KorisnikSistemaService.Entities.DataContext
{
    public class KorisnikSistemaContext : DbContext
    {

        private readonly IConfiguration _configuration;


        public KorisnikSistemaContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<TipKorisnika> TipKorisnika { get; set; }
        public DbSet<KorisnikSistema> KorisnikSistema { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("KorisnikSistemaDB"));
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipKorisnika>()
                .HasData(new
                {
                    TipKorisnikaId = Guid.Parse("e98b3a1c-eb0c-4e16-8866-a811115cac7c"),
                    NazivTipaKorisnika = "Operater"
                },
                new
                {
                    TipKorisnikaId = Guid.Parse("74e09d41-9b18-493b-9569-7069ea26beef"),
                    NazivTipaKorisnika = "TehnickiSekretar"
                },
                new
                {
                    TipKorisnikaId = Guid.Parse("64591a2c-1aa9-4bb2-aded-deb0956e3e82"),
                    NazivTipaKorisnika = "PrvaKomisija"
                },
                new
                {
                    TipKorisnikaId = Guid.Parse("f368886e-c58b-48fb-82b3-b1caf54275ba"),
                    NazivTipaKorisnika = "Superuser"
                },
                new
                {
                    TipKorisnikaId = Guid.Parse("1e9a8348-c519-405d-b080-cbfdc60db8b6"),
                    NazivTipaKorisnika = "OperaterNadmetanja"
                },
                new
                {
                    TipKorisnikaId = Guid.Parse("213cbfd7-821e-4496-86aa-a370f7804bb7"),
                    NazivTipaKorisnika = "Licitant"
                },
                new
                {
                    TipKorisnikaId = Guid.Parse("909987de-f2fc-4bb8-bd99-3944434ff4cd"),
                    NazivTipaKorisnika = "Menadzer"
                },
                new
                {
                    TipKorisnikaId = Guid.Parse("f76ffdb2-32d6-4e36-84a1-431c5158c028"),
                    NazivTipaKorisnika = "Administrator"
                });


            modelBuilder.Entity<KorisnikSistema>()
                .HasData(new
                {
                    KorisnikSistemaId = Guid.Parse("523e45d9-f7a0-42a0-8850-f64ad734b8f3"),
                    Ime = "Operater",
                    Prezime = "Operater",
                    Email = "Operater@gmail.com",
                    KorisnickoIme ="Operater",
                    Lozinka = BCrypt.Net.BCrypt.HashPassword("Operater"),
                    TipKorisnikaId = Guid.Parse("e98b3a1c-eb0c-4e16-8866-a811115cac7c"),
                },
                new
                {
                    KorisnikSistemaId = Guid.Parse("0f7918d4-1fce-49c0-b638-9274c08c499d"),
                    Ime = "TehnickiSekretar",
                    Prezime = "TehnickiSekretar",
                    Email = "TehnickiSekretar@gmail.com",
                    KorisnickoIme = "TehnickiSekretar",
                    Lozinka = BCrypt.Net.BCrypt.HashPassword("TehnickiSekretar"),
                    TipKorisnikaId = Guid.Parse("74e09d41-9b18-493b-9569-7069ea26beef")
                },
                new
                {
                    KorisnikSistemaId = Guid.Parse("d84f081c-0178-4f89-aec3-13bcb645c6a5"),
                    Ime = "PrvaKomisija",
                    Prezime = "PrvaKomisija",
                    Email = "PrvaKomisija@gmail.com",
                    KorisnickoIme = "PrvaKomisija",
                    Lozinka = BCrypt.Net.BCrypt.HashPassword("PrvaKomisija"),
                    TipKorisnikaId = Guid.Parse("64591a2c-1aa9-4bb2-aded-deb0956e3e82")
                },
                new
                {
                    KorisnikSistemaId = Guid.Parse("2fa88a5b-372e-4c37-bb20-7b786e6d4317"),
                    Ime = "Superuser",
                    Prezime = "Superuser",
                    Email = "Superuser@gmail.com",
                    KorisnickoIme = "Superuser",
                    Lozinka = BCrypt.Net.BCrypt.HashPassword("Superuser"),
                    TipKorisnikaId = Guid.Parse("f368886e-c58b-48fb-82b3-b1caf54275ba")
                },
                new
                {
                    KorisnikSistemaId = Guid.Parse("e4a42018-e71e-4e39-9c88-eae9199f46cd"),
                    Ime = "OperaterNadmetanja",
                    Prezime = "OperaterNadmetanja",
                    Email = "OperaterNadmetanja@gmail.com",
                    KorisnickoIme = "OperaterNadmetanja",
                    Lozinka = BCrypt.Net.BCrypt.HashPassword("OperaterNadmetanja"),
                    TipKorisnikaId = Guid.Parse("1e9a8348-c519-405d-b080-cbfdc60db8b6")
                },
                new
                {
                    KorisnikSistemaId = Guid.Parse("27e3d55f-e73f-4f7c-b49b-34a7e7209f44"),
                    Ime = "Licitant",
                    Prezime = "Licitant",
                    Email = "Licitant@gmail.com",
                    KorisnickoIme = "Licitant",
                    Lozinka = BCrypt.Net.BCrypt.HashPassword("Licitant"),
                    TipKorisnikaId = Guid.Parse("213cbfd7-821e-4496-86aa-a370f7804bb7")

                },
                new
                {
                    KorisnikSistemaId = Guid.Parse("bb71d94a-e97c-4fb7-9e3c-55ea09a65a72"),
                    Ime = "Menadzer",
                    Prezime = "Menadzer",
                    Email = "Menadzer@gmail.com",
                    KorisnickoIme = "Menadzer",
                    Lozinka = BCrypt.Net.BCrypt.HashPassword("Menadzer"),
                    TipKorisnikaId = Guid.Parse("909987de-f2fc-4bb8-bd99-3944434ff4cd"),
                },
                new
                {
                    KorisnikSistemaId = Guid.Parse("bd3a7634-e992-47cd-989c-3ba9aa5368ea"),
                    Ime = "Administrator",
                    Prezime = "Administrator",
                    Email = "Administrator@gmail.com",
                    KorisnickoIme = "Administrator",
                    Lozinka = BCrypt.Net.BCrypt.HashPassword("Administrator"),
                    TipKorisnikaId = Guid.Parse("f76ffdb2-32d6-4e36-84a1-431c5158c028")
                });
                
        }

    }
}
