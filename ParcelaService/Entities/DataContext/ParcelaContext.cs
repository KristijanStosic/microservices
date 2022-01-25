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
        }
    }
}
