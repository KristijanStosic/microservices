
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


        /// <summary>
        /// Popunjava bazu sa nekim inicijalnim podacima
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Prioritet>()
                .HasData(new
                {
                    PrioritetId = Guid.Parse("2578e81b-3f01-479a-b790-f52106f639f7"),
                    Opis = "Vlasnik sistema za navodnjavanje"
                });
            builder.Entity<Prioritet>()
           .HasData(new
           {
               PrioritetId = Guid.Parse("f2b8faa4-732c-4480-8b0a-34d65e483930"),
               Opis = "Vlasnik zemljišta koje se graniči sazemljištem koje se daje u zakup"
           });

            builder.Entity<KontaktOsoba>()
                .HasData(new
                {
                    KontaktOsobaId = Guid.Parse("244fb7c4-aab8-4ec4-8960-e48e017bad37"),
                    Ime = "Milan",
                    Prezime = "Drazic",
                    Telefon="0693432534",
                    Funkcija = "Funkcija1"

                });

            builder.Entity<KontaktOsoba>()
               .HasData(new
               {
                   KontaktOsobaId = Guid.Parse("da2197a4-891f-4a40-a1f2-313962701627"),
                   Ime = "Mark",
                   Prezime = "Todor",
                   Telefon = "0693432534",
                   Funkcija = "Funkcija2"

               });
        }
    }
}
