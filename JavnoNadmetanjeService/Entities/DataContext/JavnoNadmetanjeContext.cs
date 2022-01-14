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
                    StatusId = Guid.NewGuid(),
                    NazivStatusa="Prvi krug"
                }, 
                new
                {
                    StatusId = Guid.NewGuid(),
                    NazivStatusa = "Drugi krug sa starim uslovima"
                },
                new
                {
                    StatusId = Guid.NewGuid(),
                    NazivStatusa = "Drugi krug sa novim uslovima"
                });

            modelBuilder.Entity<Tip>()
                .HasData(new
                {
                    TipId = Guid.NewGuid(),
                    NazivTipa = "Javna licitacija"
                },
                new
                {
                    TipId = Guid.NewGuid(),
                    NazivTipa = "Otvaranje zatvorenih ponuda"
                });
        }
    }
}
