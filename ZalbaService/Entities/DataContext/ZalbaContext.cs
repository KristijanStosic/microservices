using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZalbaService.Entities.DataContext
{
    public class ZalbaContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ZalbaContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<StatusZalbe> StatusZalbe { get; set; }

        public DbSet<TipZalbe> TipZalbe { get; set; }

        public DbSet<RadnjaZaZalbu> RadnjaZaZalbu { get; set; }

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
                    StatusZalbeId = Guid.NewGuid(),
                    NazivStatusaZalbe = "Usvojena"
                },
                new
                {
                    StatusZalbeId = Guid.NewGuid(),
                    NazivStatusaZalbe = "Odbijena"
                },
                new
                {
                    StatusZalbeId = Guid.NewGuid(),
                    NazivStatusaZalbe = "Otvorena"
                });

            modelBuilder.Entity<TipZalbe>()
                .HasData(new
                {
                    TipZalbeId = Guid.NewGuid(),
                    NazivTipaZalbe = "Zalba na tok javnog nadmetanja"
                },
                new
                {
                    TipZalbeId = Guid.NewGuid(),
                    NazivTipaZalbe = "Zalba na Odluku o davanju u zakup"
                },
                new
                {
                    TipZalbeId = Guid.NewGuid(),
                    NazivTipaZalbe = "Zalba na Odluku o davanju na koriscenje"
                });

            modelBuilder.Entity<RadnjaZaZalbu>()
                .HasData(new
                {
                    RadnjaZaZalbuId = Guid.NewGuid(),
                    NazivRadnjeZaZalbu = "JN ide u drugi krug sa novim uslovima"
                },
                new
                {
                    RadnjaZaZalbuId = Guid.NewGuid(),
                    NazivRadnjeZaZalbu = "JN ide u drugi krug sa starim uslovima"
                },
                new
                {
                    RadnjaZaZalbuId = Guid.NewGuid(),
                    NazivRadnjeZaZalbu = "JN ne ide u drugi krug"
                });
        }
    }
}
