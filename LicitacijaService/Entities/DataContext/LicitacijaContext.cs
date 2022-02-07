using LicitacijaService.Entities.ManyToMany;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LicitacijaService.Entities.DataContext
{
    public class LicitacijaContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public LicitacijaContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Licitacija> Licitacija { get; set; }
        public DbSet<LicitacijaJavnoNadmetanje> LicitacijaJavnoNadmetanje { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("LicitacijaDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder

            modelBuilder.Entity<LicitacijaJavnoNadmetanje>()
             .HasOne(lj => lj.Licitacija)
            .WithMany()
            .HasForeignKey(lj => lj.LicitacijaId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

            modelBuilder.Entity<LicitacijaJavnoNadmetanje>()
                .HasKey(lj => new { lj.LicitacijaId, lj.JavnoNadmetanjeId });


                

            modelBuilder.Entity<Licitacija>()
                .HasData(new
                {
                    LicitacijaId = Guid.Parse("cac0e0a2-852a-4265-ac71-49c25fb5559b"),
                    BrojLicitacije = 42,
                    GodinaLicitacije = 2012,
                    OgranicenjeLicitacije = 12,
                    RokLicitacije = DateTime.Now.AddDays(-500),
                    KorakCeneLicitacije = 3
                },
                new
                {
                    LicitacijaId = Guid.Parse("2ff32eb3-a7a1-4e8b-a9e1-8ec51f3eca4c"),
                    BrojLicitacije = 43,
                    GodinaLicitacije = 2012,
                    OgranicenjeLicitacije = 13,
                    RokLicitacije = DateTime.Now.AddDays(-489),
                    KorakCeneLicitacije = 2
                },
                new {
                    LicitacijaId = Guid.Parse("fb96a27d-f87f-49b5-98f3-ef6b57e84c3c"),
                    BrojLicitacije = 44,
                    GodinaLicitacije = 2012,
                    OgranicenjeLicitacije = 14,
                    RokLicitacije = DateTime.Now.AddDays(-480),
                    KorakCeneLicitacije = 4
                });
            modelBuilder.Entity<LicitacijaJavnoNadmetanje>()
                .HasData(new {
                    LicitacijaId = Guid.Parse("cac0e0a2-852a-4265-ac71-49c25fb5559b"),
                    JavnoNadmetanjeId = Guid.Parse("56A7CFF5-CB0A-46B8-BFC1-93DB415FEEB4"),
                }, new
                {
                    LicitacijaId = Guid.Parse("cac0e0a2-852a-4265-ac71-49c25fb5559b"),
                    JavnoNadmetanjeId = Guid.Parse("6849BBBE-5798-4C04-AA20-58DE420AA578"),
                }, new
                {
                    LicitacijaId = Guid.Parse("2ff32eb3-a7a1-4e8b-a9e1-8ec51f3eca4c"),
                    JavnoNadmetanjeId = Guid.Parse("B195C4C2-2B26-40AD-8FF6-C212474ACC83"),
                });
                
                

            
        }




    }
}
