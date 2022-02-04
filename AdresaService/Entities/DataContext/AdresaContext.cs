using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdresaService.Entities.DataContext
{
    public class AdresaContext : DbContext
    {


        private readonly IConfiguration _configuration;

        public AdresaContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this._configuration = configuration;
        }

        public DbSet<Drzava> Drzave { get; set; }
        public DbSet<Adresa> Adrese { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("AdresaDB"));
        }


        /// <summary>
        /// Popunjava bazu sa nekim inicijalnim podacima
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Drzava>()
                .HasData(new
                {
                    DrzavaId = Guid.Parse("f320743f-6c87-47ca-9f82-50191c1d31ac"),
                    NazivDrzave = "Srbija"
                });

            modelBuilder.Entity<Drzava>()
               .HasData(new
               {
                   DrzavaId = Guid.Parse("9ce21ce2-7809-4e28-ba74-fd2f1bc6466a"),
                   NazivDrzave = "Bosna i hercegovina"
               });
            modelBuilder.Entity<Adresa>()
                .HasData(new
                {
                    AdresaId = Guid.Parse("1c989ee3-13b2-4d3b-abeb-c4e6343eace7"),
                    Ulica = "Branka Ilica",
                    Broj = "1",
                    Mesto = "Novi Sad",
                    PostanskiBroj = "21000",
                    DrzavaId = Guid.Parse("f320743f-6c87-47ca-9f82-50191c1d31ac")
                });
            modelBuilder.Entity<Adresa>()
                .HasData(new
                {
                    AdresaId = Guid.Parse("37371ef6-4f25-48b3-9bf2-fe72a81f88d2"),
                    Ulica = "Solunska",
                    Broj = "23",
                    Mesto = "Bijeljna",
                    PostanskiBroj = "76300",
                    DrzavaId = Guid.Parse("9ce21ce2-7809-4e28-ba74-fd2f1bc6466a")
                });


        }

     }
}
